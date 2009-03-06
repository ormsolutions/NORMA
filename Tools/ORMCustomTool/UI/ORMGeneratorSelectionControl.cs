#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
*                                                                          *
* The use and distribution terms for this software are covered by the      *
* Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
* can be found in the file CPL.txt at the root of this distribution.       *
* By using this software in any fashion, you are agreeing to be bound by   *
* the terms of this license.                                               *
*                                                                          *
* You must not remove this notice, or any other, from this software.       *
\**************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Build.BuildEngine;
using Microsoft.VisualStudio.VirtualTreeGrid;
using System.IO;

namespace ORMSolutions.ORMArchitect.ORMCustomTool
{
	internal sealed partial class ORMGeneratorSelectionControl : Form
	{
		// TODO: Remove these once this is nested within ORMCustomTool.
		private const string ITEMGROUP_CONDITIONSTART = "Exists('";
		private const string ITEMGROUP_CONDITIONEND = "')";
		private const string ITEMMETADATA_DEPENDENTUPON = "DependentUpon";
		private const string ITEMMETADATA_GENERATOR = "Generator";
		private const string ITEMMETADATA_ORMGENERATOR = "ORMGenerator";

		private readonly MainBranch _mainBranch;
		private readonly EnvDTE.ProjectItem _projectItem;
		private readonly Project _project;
		private readonly BuildItemGroup _originalBuildItemGroup;
		private bool _savedChanges;
		private readonly string _sourceFileName;
		private readonly BuildItemGroup _buildItemGroup;
		private readonly Dictionary<string, BuildItem> _buildItemsByGenerator;
		private Dictionary<string, string> _removedItems;
		private readonly string _projectItemRelativePath;
		private ORMGeneratorSelectionControl()
		{
			this.InitializeComponent();
		}

		public ORMGeneratorSelectionControl(EnvDTE.ProjectItem projectItem)
			: this()
		{
			Project project = Engine.GlobalEngine.GetLoadedProject(projectItem.ContainingProject.FullName);
			string projectFullPath = project.FullFileName;
			string projectItemRelativePath = (string)projectItem.Properties.Item("LocalPath").Value;
			projectItemRelativePath = (new Uri(projectFullPath)).MakeRelativeUri(new Uri(projectItemRelativePath)).ToString();
			this._projectItemRelativePath = projectItemRelativePath;

			BuildItemGroup originalBuildItemGroup = ORMCustomTool.GetBuildItemGroup(project, projectItemRelativePath);
			_projectItem = projectItem;
			this._project = project;
			this._originalBuildItemGroup = originalBuildItemGroup;

			BuildItemGroup buildItemGroup;
			if (originalBuildItemGroup == null)
			{
				buildItemGroup = project.AddNewItemGroup();
				buildItemGroup.Condition = string.Concat(ITEMGROUP_CONDITIONSTART, projectItemRelativePath, ITEMGROUP_CONDITIONEND);
			}
			else
			{
				buildItemGroup = project.AddNewItemGroup();
				buildItemGroup.Condition = originalBuildItemGroup.Condition;
				foreach (BuildItem item in originalBuildItemGroup)
				{
					BuildItem newItem = buildItemGroup.AddNewItem(item.Name, item.Include, false);
					newItem.Condition = item.Condition;
					item.CopyCustomMetadataTo(newItem);
				}
			}
			this._buildItemGroup = buildItemGroup;

			string condition = buildItemGroup.Condition.Trim();

			string sourceFileName = this._sourceFileName = projectItem.Name;

			this.textBox_ORMFileName.Text = sourceFileName;

			this.button_SaveChanges.Click += new EventHandler(this.SaveChanges);
			this.button_Cancel.Click += new EventHandler(this.Cancel);

			ITree tree = (ITree)(this.virtualTreeControl.MultiColumnTree = new MultiColumnTree(2));
			this.virtualTreeControl.SetColumnHeaders(new VirtualTreeColumnHeader[]
				{
					// TODO: Localize these.
					new VirtualTreeColumnHeader("Generated Format", 0.30f, VirtualTreeColumnHeaderStyles.ColumnPositionLocked | VirtualTreeColumnHeaderStyles.DragDisabled),
					new VirtualTreeColumnHeader("Generated File Name", 1f, VirtualTreeColumnHeaderStyles.ColumnPositionLocked | VirtualTreeColumnHeaderStyles.DragDisabled)
				}, true);
			MainBranch mainBranch = this._mainBranch = new MainBranch(this);
			int totalCount = mainBranch.VisibleItemCount;
			int[] primaryIndices = new int[totalCount];
			for (int i = 0; i < totalCount; ++i)
			{
				if (mainBranch.IsPrimaryDisplayItem(i))
				{
					primaryIndices[i] = i - totalCount;
				}
				else
				{
					primaryIndices[i] = i + 1;
				}
			}
			Array.Sort<int>(primaryIndices);
			int lastPrimary = -1;
			for (int i = 0; i < totalCount; ++i)
			{
				int modifiedIndex = primaryIndices[i];
				if (modifiedIndex < 0)
				{
					primaryIndices[i] = modifiedIndex + totalCount;
				}
				else
				{
					if (lastPrimary == -1)
					{
						lastPrimary = i - 1;
					}
					primaryIndices[i] = modifiedIndex - 1;
				}
			}
			tree.Root = (lastPrimary == -1) ? (IBranch)mainBranch : new BranchPartition(
				mainBranch,
				primaryIndices,
				new BranchPartitionSection(0, lastPrimary + 1, null),
				new BranchPartitionSection(lastPrimary + 1, totalCount - lastPrimary - 1, "Intermediate and Secondary Files")); // UNDONE: Localize Header
			this.virtualTreeControl.ShowToolTips = true;
			this.virtualTreeControl.FullCellSelect = true;

			Dictionary<string, BuildItem> buildItemsByGenerator = this._buildItemsByGenerator = new Dictionary<string, BuildItem>(buildItemGroup.Count, StringComparer.OrdinalIgnoreCase);
			foreach (BuildItem buildItem in buildItemGroup)
			{
				string ormGeneratorName = buildItem.GetEvaluatedMetadata(ITEMMETADATA_ORMGENERATOR);
				if (!String.IsNullOrEmpty(ormGeneratorName) && String.Equals(buildItem.GetEvaluatedMetadata(ITEMMETADATA_DEPENDENTUPON), sourceFileName, StringComparison.OrdinalIgnoreCase))
				{
					IORMGenerator ormGenerator = ORMCustomTool.ORMGenerators[ormGeneratorName];
					MainBranch.OutputFormatBranch outputFormatBranch = mainBranch.Branches[ormGenerator.ProvidesOutputFormat];
					System.Diagnostics.Debug.Assert(outputFormatBranch.SelectedORMGenerator == null);
					outputFormatBranch.SelectedORMGenerator = ormGenerator;
					buildItemsByGenerator.Add(ormGeneratorName, buildItem);
				}
			}
		}

		private string SourceFileName
		{
			get
			{
				return this._sourceFileName;
			}
		}

		private BuildItemGroup BuildItemGroup
		{
			get
			{
				return this._buildItemGroup;
			}
		}

		private IDictionary<string, BuildItem> BuildItemsByGenerator
		{
			get
			{
				return this._buildItemsByGenerator;
			}
		}

		private void RemoveRemovedItem(BuildItem buildItem)
		{
			if (_removedItems != null)
			{
				string key = buildItem.FinalItemSpec;
				if (_removedItems.ContainsKey(key))
				{
					_removedItems.Remove(key);
				}
			}
		}

		private void AddRemovedItem(BuildItem buildItem)
		{
			Dictionary<string, string> items = _removedItems;
			if (items == null)
			{
				items = new Dictionary<string, string>();
				_removedItems = items;
			}
			string key = buildItem.FinalItemSpec;
			items[key] = key;
		}

		protected override void OnClosed(EventArgs e)
		{
			if (_savedChanges)
			{
				// Make sure the current document has the necessary
				// extensions loaded.
				// UNDONE: We should be able to do this with the document
				// closed or open as text as well via a registered service
				// on the ORMDesignerPackage, but this is sufficient for now.
				Dictionary<string, string> requiredExtensions = null;
				string[] loadedExtensions = null;
				foreach (IORMGenerator selectedGenerator in _mainBranch.SelectedGenerators)
				{
					foreach (string requiredExtension in selectedGenerator.GetRequiredExtensionsForInputFormat("ORM"))
					{
						if (loadedExtensions == null)
						{
							loadedExtensions = (new ORMExtensionManager(_projectItem)).GetLoadedExtensions();
						}
						if (Array.BinarySearch<string>(loadedExtensions, requiredExtension) < 0)
						{
							if (requiredExtensions == null)
							{
								requiredExtensions = new Dictionary<string, string>();
							}
							else if (requiredExtensions.ContainsKey(requiredExtension))
							{
								continue;
							}
							requiredExtensions.Add(requiredExtension, requiredExtension);
						}
					}
				}
				if (requiredExtensions != null)
				{
					_savedChanges = ORMExtensionManager.EnsureExtensions(_projectItem, requiredExtensions.Values);
				}
			}
			if (_savedChanges)
			{
				// Delete the removed items from the project
				if (_removedItems != null)
				{
					EnvDTE.ProjectItems subItems = _projectItem.ProjectItems;
					foreach (string itemName in _removedItems.Keys)
					{
						try
						{
							EnvDTE.ProjectItem subItem = subItems.Item(itemName);
							if (subItem != null)
							{
								subItem.Delete();
							}
						}
						catch (ArgumentException)
						{
							// Swallow
						}
					}
				}
				// throw away the original build item group
				if (_originalBuildItemGroup != null)
				{
					_project.RemoveItemGroup(_originalBuildItemGroup);
				}

				Dictionary<string, BuildItem> removeItems = new Dictionary<string, BuildItem>();
				string tmpFile = null;
				try
				{
					EnvDTE.ProjectItems projectItems = _projectItem.ProjectItems;
					string itemDirectory = (new FileInfo((string)_project.FullFileName)).DirectoryName;
					foreach (BuildItem item in this._buildItemGroup)
					{
						string filePath = string.Concat(itemDirectory, Path.DirectorySeparatorChar, item.Include);
						string fileName = (new FileInfo(item.Include)).Name;
						if (File.Exists(filePath))
						{
							try
							{
								projectItems.AddFromFile(filePath);
							}
							catch (ArgumentException)
							{
								// Swallow
							}
						}
						else
						{
							if (tmpFile == null)
							{
								tmpFile = Path.GetTempFileName();
							}
							EnvDTE.ProjectItem projectItem = projectItems.AddFromTemplate(tmpFile, fileName);
							string customTool = item.GetMetadata(ITEMMETADATA_GENERATOR);
							if (!string.IsNullOrEmpty(customTool))
							{
								projectItem.Properties.Item("CustomTool").Value = customTool;
							}
						}
						removeItems[item.Include] = null;
					}
				}
				finally
				{
					if (tmpFile != null)
					{
						File.Delete(tmpFile);
					}
				}

				foreach (BuildItemGroup group in this._project.ItemGroups)
				{
					if (group.Condition.Trim() == this._buildItemGroup.Condition.Trim())
					{
						continue;
					}
					foreach (BuildItem item in group)
					{
						if (removeItems.ContainsKey(item.Include))
						{
							removeItems[item.Include] = item;
						}
					}
				}
				foreach (string key in removeItems.Keys)
				{
					if (removeItems[key] != null)
					{
						this._project.RemoveItem(removeItems[key]);
					}
				}

				VSLangProj.VSProjectItem vsProjectItem = _projectItem.Object as VSLangProj.VSProjectItem;
				if (vsProjectItem != null)
				{
					vsProjectItem.RunCustomTool();
				}
			}
			else
			{
				_project.RemoveItemGroup(_buildItemGroup);
			}
			base.OnClosed(e);
		}

		private void SaveChanges(object sender, EventArgs e)
		{
			this._savedChanges = true;
			this.Close();
		}

		private void Cancel(object sender, EventArgs e)
		{
			this._savedChanges = false;
			this.Close();
		}
	}
}
