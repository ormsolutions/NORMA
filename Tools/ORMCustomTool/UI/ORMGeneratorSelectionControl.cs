#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.VirtualTreeGrid;
#if VISUALSTUDIO_10_0
using Microsoft.Build.Construction;
#else
using Microsoft.Build.BuildEngine;
#endif

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
		private bool _savedChanges;
		private readonly string _sourceFileName;
		private Dictionary<string, string> _removedItems;
		private readonly string _projectItemRelativePath;
		private readonly IServiceProvider _serviceProvider;
#if VISUALSTUDIO_10_0
		private readonly ProjectRootElement _project;
		private readonly ProjectItemGroupElement _originalItemGroup;
		private readonly ProjectItemGroupElement _itemGroup;
		private readonly Dictionary<string, ProjectItemElement> _itemsByGenerator;
#else // VISUALSTUDIO_10_0
		private readonly Project _project;
		private readonly BuildItemGroup _originalItemGroup;
		private readonly BuildItemGroup _itemGroup;
		private readonly Dictionary<string, BuildItem> _itemsByGenerator;
#endif // VISUALSTUDIO_10_0
		private ORMGeneratorSelectionControl()
		{
			this.InitializeComponent();
			textBox_ORMFileName.Left = label_GeneratedFilesFor.Right + textBox_ORMFileName.Margin.Left;
		}

		public ORMGeneratorSelectionControl(EnvDTE.ProjectItem projectItem, IServiceProvider serviceProvider)
			: this()
		{
			_projectItem = projectItem;
			_serviceProvider = serviceProvider;
#if VISUALSTUDIO_10_0
			ProjectRootElement project = ProjectRootElement.TryOpen(projectItem.ContainingProject.FullName);
			string projectFullPath = project.FullPath;
#else // VISUALSTUDIO_10_0
			Project project = Engine.GlobalEngine.GetLoadedProject(projectItem.ContainingProject.FullName);
			string projectFullPath = project.FullFileName;
#endif // VISUALSTUDIO_10_0
			_project = project;

			string projectItemRelativePath = (string)projectItem.Properties.Item("LocalPath").Value;
			projectItemRelativePath = (new Uri(projectFullPath)).MakeRelativeUri(new Uri(projectItemRelativePath)).ToString();
			_projectItemRelativePath = projectItemRelativePath;

#if VISUALSTUDIO_10_0
			ProjectItemGroupElement originalItemGroup = ORMCustomTool.GetItemGroup(project, projectItemRelativePath);
			ProjectItemGroupElement itemGroup;
			if (originalItemGroup == null)
			{
				itemGroup = project.AddItemGroup();
				itemGroup.Condition = string.Concat(ITEMGROUP_CONDITIONSTART, projectItemRelativePath, ITEMGROUP_CONDITIONEND);
			}
			else
			{
				itemGroup = project.AddItemGroup();
				itemGroup.Condition = originalItemGroup.Condition;
				foreach (ProjectItemElement item in originalItemGroup.Items)
				{
					ProjectItemElement newItem = itemGroup.AddItem(item.ItemType, item.Include);
					newItem.Condition = item.Condition;
					foreach (ProjectMetadataElement metadataElement in item.Metadata)
					{
						newItem.AddMetadata(metadataElement.Name, metadataElement.Value);
					}
				}
			}
#else // VISUALSTUDIO_10_0
			BuildItemGroup originalItemGroup = ORMCustomTool.GetItemGroup(project, projectItemRelativePath);
			BuildItemGroup itemGroup;
			if (originalItemGroup == null)
			{
				itemGroup = project.AddNewItemGroup();
				itemGroup.Condition = string.Concat(ITEMGROUP_CONDITIONSTART, projectItemRelativePath, ITEMGROUP_CONDITIONEND);
			}
			else
			{
				itemGroup = project.AddNewItemGroup();
				itemGroup.Condition = originalItemGroup.Condition;
				foreach (BuildItem item in originalItemGroup)
				{
					BuildItem newItem = itemGroup.AddNewItem(item.Name, item.Include, false);
					newItem.Condition = item.Condition;
					item.CopyCustomMetadataTo(newItem);
				}
			}
#endif // VISUALSTUDIO_10_0
			_originalItemGroup = originalItemGroup;
			_itemGroup = itemGroup;

			string condition = itemGroup.Condition.Trim();

			string sourceFileName = this._sourceFileName = projectItem.Name;

			this.textBox_ORMFileName.Text = sourceFileName;

			this.button_SaveChanges.Click += new EventHandler(this.SaveChanges);
			this.button_Cancel.Click += new EventHandler(this.Cancel);

			ITree tree = (ITree)(this.virtualTreeControl.MultiColumnTree = new MultiColumnTree(2));
			this.virtualTreeControl.SetColumnHeaders(new VirtualTreeColumnHeader[]
				{
					// TODO: Localize these.
					new VirtualTreeColumnHeader("Generated File Format", 0.30f, VirtualTreeColumnHeaderStyles.ColumnPositionLocked | VirtualTreeColumnHeaderStyles.DragDisabled),
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
			int modifierCount = totalCount - mainBranch.Branches.Count;
			tree.Root = (lastPrimary == -1) ? (IBranch)mainBranch : new BranchPartition(
				mainBranch,
				primaryIndices,
				new BranchPartitionSection(0, lastPrimary + 1, null),
				new BranchPartitionSection(totalCount - modifierCount, modifierCount, "Generated File Modifiers"),
				new BranchPartitionSection(lastPrimary + 1, totalCount - lastPrimary - modifierCount - 1, "Intermediate and Secondary Files")); // UNDONE: Localize Header
			this.virtualTreeControl.ShowToolTips = true;
			this.virtualTreeControl.FullCellSelect = true;

#if VISUALSTUDIO_10_0
			Dictionary<string, ProjectItemElement> buildItemsByGenerator = this._itemsByGenerator = new Dictionary<string, ProjectItemElement>(itemGroup.Count, StringComparer.OrdinalIgnoreCase);
			foreach (ProjectItemElement buildItem in itemGroup.Items)
#else // VISUALSTUDIO_10_0
			Dictionary<string, BuildItem> buildItemsByGenerator = this._itemsByGenerator = new Dictionary<string, BuildItem>(itemGroup.Count, StringComparer.OrdinalIgnoreCase);
			foreach (BuildItem buildItem in itemGroup)
#endif // VISUALSTUDIO_10_0
			{
				// Do this very defensively so that the dialog can still be opened if a project is out
				// of step with the generators registered on a specific machine.
				string generatorNameData = buildItem.GetEvaluatedMetadata(ITEMMETADATA_ORMGENERATOR);
				string[] generatorNames; // The first string is the primary generator, others are the format modifiers
				int generatorNameCount;
				IORMGenerator primaryGenerator;
				MainBranch.OutputFormatBranch primaryFormatBranch;
				if (!String.IsNullOrEmpty(generatorNameData) &&
					String.Equals(buildItem.GetEvaluatedMetadata(ITEMMETADATA_DEPENDENTUPON), sourceFileName, StringComparison.OrdinalIgnoreCase) &&
					null != (generatorNames = generatorNameData.Split((char[])null, StringSplitOptions.RemoveEmptyEntries)) &&
					0 != (generatorNameCount = generatorNames.Length) &&
					ORMCustomTool.ORMGenerators.TryGetValue(generatorNames[0], out primaryGenerator) &&
					mainBranch.Branches.TryGetValue(primaryGenerator.ProvidesOutputFormat, out primaryFormatBranch))
				{
					System.Diagnostics.Debug.Assert(primaryFormatBranch.SelectedORMGenerator == null);
					primaryFormatBranch.SelectedORMGenerator = primaryGenerator;
					buildItemsByGenerator.Add(generatorNames[0], buildItem);

					// Format modifiers are attached to the end of the list
					for (int i = 1; i < generatorNameCount; ++i )
					{
						MainBranch.OutputFormatBranch modifierBranch = primaryFormatBranch.NextModifier;
						string findName = generatorNames[i];
						while (modifierBranch != null)
						{
							IORMGenerator testGenerator = modifierBranch.ORMGenerators[0];
							if (testGenerator.OfficialName == findName)
							{
								modifierBranch.SelectedORMGenerator = testGenerator;
								break;
							}
							modifierBranch = modifierBranch.NextModifier;
						}
					}
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

#if VISUALSTUDIO_10_0
		private IDictionary<string, ProjectItemElement> BuildItemsByGenerator
#else
		private IDictionary<string, BuildItem> BuildItemsByGenerator
#endif
		{
			get
			{
				return this._itemsByGenerator;
			}
		}

#if VISUALSTUDIO_10_0
		private void RemoveRemovedItem(ProjectItemElement buildItem)
#else
		private void RemoveRemovedItem(BuildItem buildItem)
#endif
		{
			if (_removedItems != null)
			{
				string key = ORMCustomToolUtility.GetItemInclude(buildItem);
				if (_removedItems.ContainsKey(key))
				{
					_removedItems.Remove(key);
				}
			}
		}

#if VISUALSTUDIO_10_0
		private void AddRemovedItem(ProjectItemElement buildItem)
#else
		private void AddRemovedItem(BuildItem buildItem)
#endif
		{
			Dictionary<string, string> items = _removedItems;
			if (items == null)
			{
				items = new Dictionary<string, string>();
				_removedItems = items;
			}
			string key = ORMCustomToolUtility.GetItemInclude(buildItem);
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
							loadedExtensions = (new ORMExtensionManager(_projectItem)).GetLoadedExtensions(_serviceProvider);
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
					_savedChanges = ORMExtensionManager.EnsureExtensions(_projectItem, _serviceProvider, requiredExtensions.Values);
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
				if (_originalItemGroup != null)
				{
					try
					{
#if VISUALSTUDIO_10_0
						_project.RemoveChild(_originalItemGroup);
#else
						_project.RemoveItemGroup(_originalItemGroup);
#endif
					}
					catch (InvalidOperationException)
					{
						// Swallow
					}
				}

#if VISUALSTUDIO_10_0
				Dictionary<string, ProjectItemElement> removeItems = new Dictionary<string, ProjectItemElement>();
#else
				Dictionary<string, BuildItem> removeItems = new Dictionary<string, BuildItem>();
#endif
				string tmpFile = null;
				try
				{
					EnvDTE.ProjectItems projectItems = _projectItem.ProjectItems;
#if VISUALSTUDIO_10_0
					string itemDirectory = (new FileInfo((string)_project.FullPath)).DirectoryName;
					foreach (ProjectItemElement item in _itemGroup.Items)
#else
					string itemDirectory = (new FileInfo((string)_project.FullFileName)).DirectoryName;
					foreach (BuildItem item in this._itemGroup)
#endif
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

#if VISUALSTUDIO_10_0
				foreach (ProjectItemGroupElement group in this._project.ItemGroups)
#else
				foreach (BuildItemGroup group in this._project.ItemGroups)
#endif
				{
					if (group.Condition.Trim() == this._itemGroup.Condition.Trim())
					{
						continue;
					}
#if VISUALSTUDIO_10_0
					foreach (ProjectItemElement item in group.Items)
#else
					foreach (BuildItem item in group)
#endif
					{
						if (removeItems.ContainsKey(item.Include))
						{
							removeItems[item.Include] = item;
						}
					}
				}
				foreach (string key in removeItems.Keys)
				{
#if VISUALSTUDIO_10_0
					ProjectItemElement removeItem;
					ProjectElementContainer removeFrom;
					if (null != (removeItem =removeItems[key]) &&
						null != (removeFrom = removeItem.Parent))
					{
						removeFrom.RemoveChild(removeItem);
					}
#else
					BuildItem removeItem = removeItems[key];
					if (removeItem != null)
					{
						_project.RemoveItem(removeItem);
					}
#endif
				}

				VSLangProj.VSProjectItem vsProjectItem = _projectItem.Object as VSLangProj.VSProjectItem;
				if (vsProjectItem != null)
				{
					vsProjectItem.RunCustomTool();
				}
			}
			else
			{
#if VISUALSTUDIO_10_0
				_project.RemoveChild(_itemGroup);
#else
				_project.RemoveItemGroup(_itemGroup);
#endif
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
