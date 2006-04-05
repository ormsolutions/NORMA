#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
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

namespace Neumont.Tools.ORM.ORMCustomTool
{
	internal sealed partial class ORMGeneratorSelectionControl : Form
	{
		// TODO: Remove these once this is nested within ORMCustomTool.
		private const string ITEMGROUP_CONDITIONSTART = "Exists('";
		private const string ITEMGROUP_CONDITIONEND = "')";
		private const string ITEMMETADATA_ORMGENERATOR = "ORMGenerator";
		private const string ITEMMETADATA_DEPENDENTUPON = "DependentUpon";

		private readonly MainBranch _mainBranch;
		private readonly EnvDTE.ProjectItem _projectItem;
		private readonly Project _project;
		private readonly BuildItemGroup _originalBuildItemGroup;
		private bool _savedChanges;

		private ORMGeneratorSelectionControl()
		{
			this.InitializeComponent();
		}
		public ORMGeneratorSelectionControl(EnvDTE.ProjectItem projectItem)
			: this()
		{
			Project project = Engine.GlobalEngine.GetLoadedProject(projectItem.ContainingProject.FullName);
			BuildItemGroup originalBuildItemGroup = ORMCustomTool.GetBuildItemGroup(project, projectItem.Name);
			_projectItem = projectItem;
			this._project = project;
			this._originalBuildItemGroup = originalBuildItemGroup;

			BuildItemGroup buildItemGroup;
			if (originalBuildItemGroup == null)
			{
				buildItemGroup = project.AddNewItemGroup();
				buildItemGroup.Condition = string.Concat(ITEMGROUP_CONDITIONSTART, projectItem.Name, ITEMGROUP_CONDITIONEND);
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
			string sourceFileName = condition.Substring(ITEMGROUP_CONDITIONSTART.Length, condition.Length - (ITEMGROUP_CONDITIONSTART.Length + ITEMGROUP_CONDITIONEND.Length));
			this._sourceFileName = sourceFileName;
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
			MainBranch mainBranch;
			tree.Root = mainBranch = this._mainBranch = new MainBranch(this);
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

		private readonly string _sourceFileName;
		private string SourceFileName
		{
			get
			{
				return this._sourceFileName;
			}
		}

		private readonly BuildItemGroup _buildItemGroup;
		private BuildItemGroup BuildItemGroup
		{
			get
			{
				return this._buildItemGroup;
			}
		}

		private readonly Dictionary<string, BuildItem> _buildItemsByGenerator;
		private IDictionary<string, BuildItem> BuildItemsByGenerator
		{
			get
			{
				return this._buildItemsByGenerator;
			}
		}
		private Dictionary<string, string> _removedItems;
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
				if (_originalBuildItemGroup != null)
				{
					_project.RemoveItemGroup(_originalBuildItemGroup);
				}
				VSLangProj.VSProjectItem vsProjectItem = _projectItem.Object as VSLangProj.VSProjectItem;
				if (vsProjectItem != null)
				{
					vsProjectItem.RunCustomTool();
					if (DialogResult.Yes == MessageBox.Show(this, "The Neumont ORM Generator must save and reload your project for changes to take effect. Would you like to reload now?", "Neumont ORM Generator", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
					{
						EnvDTE.DTE dte = _projectItem.DTE;
						dte.ExecuteCommand("File.SaveAll", "");
						string solutionPath = dte.Solution.FullName;
						dte.ExecuteCommand("File.CloseSolution", "");
						dte.Solution.Open(solutionPath);
					}
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
