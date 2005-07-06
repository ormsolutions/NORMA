#region	Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.VirtualTreeGrid;

#endregion

namespace Northface.Tools.ORM.ObjectModel
{
	///	<summary>
	///	Custom reference mode editor.
	///	</summary>
	public partial class CustomReferenceModeEditor : UserControl
	{
		private ReferenceModeHeaderBranch myHeaders;

		/// <summary>
		/// Default constructor
		/// </summary>
		public CustomReferenceModeEditor()
		{
			InitializeComponent();
			myTree.SetColumnHeaders(new VirtualTreeColumnHeader[]{
				new	VirtualTreeColumnHeader(ResourceStrings.ModelReferenceModeEditorNameColumn),
				new	VirtualTreeColumnHeader(ResourceStrings.ModelReferenceModeEditorKindColumn),
				new	VirtualTreeColumnHeader(ResourceStrings.ModelReferenceModeEditorFormatStringColumn)}
				, true);
			ITree treeData = new MultiColumnTree(3);
			myHeaders = new ReferenceModeHeaderBranch();
			treeData.Root = myHeaders;
			myTree.MultiColumnTree = (IMultiColumnTree)treeData;
		}

		#region methods

		/// <summary>
		/// Sets the Reference modes
		/// </summary>
		/// <param name="model"></param>
		public void SetModel(ORMModel model)
		{
			myHeaders.SetModel(model);
		}
		#endregion

/* Removed for FxCop compliance, not currently used
		private void DeleteMenu_Click(object sender, EventArgs e)
		{
			Delete(sender as VirtualTreeControl);
		}

		private void Delete(VirtualTreeControl tree)
		{
			if (tree != null)
			{
				VirtualTreeItemInfo info = tree.Tree.GetItemInfo(tree.CurrentIndex, tree.CurrentColumn, false);
				CustomReferenceModesBranch modes = info.Branch as CustomReferenceModesBranch;
				if (modes != null)
				{
					modes.Delete(info.Row, info.Column);
				}
			}
		}
Removed for FxCop compliance, not currently used */
		/// <summary>
		/// Get the tree control for this editor
		/// </summary>
		public VirtualTreeControl TreeControl
		{
			get
			{
				return myTree;
			}
		}
	}
}
