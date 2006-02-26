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

namespace Neumont.Tools.ORM.ObjectModel
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
