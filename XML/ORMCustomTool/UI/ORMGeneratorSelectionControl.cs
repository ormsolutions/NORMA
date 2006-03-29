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
					new VirtualTreeColumnHeader("Generator Name", 0.30f, VirtualTreeColumnHeaderStyles.ColumnPositionLocked | VirtualTreeColumnHeaderStyles.DragDisabled),
					new VirtualTreeColumnHeader("Generated File Name", 0.70f, VirtualTreeColumnHeaderStyles.ColumnPositionLocked | VirtualTreeColumnHeaderStyles.DragDisabled)
				}, true);
			MainBranch mainBranch;
			tree.Root = mainBranch = this._mainBranch = new MainBranch(this);
			this.virtualTreeControl.ShowToolTips = true;

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

		protected override void OnClosed(EventArgs e)
		{
			if (_savedChanges)
			{
				if (_originalBuildItemGroup != null)
				{
					_project.RemoveItemGroup(_originalBuildItemGroup);
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
