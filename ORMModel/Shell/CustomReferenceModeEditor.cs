#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
* Copyright © Matthew Curland. All rights reserved.                        *
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
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.VirtualTreeGrid;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Shell;
using System.Globalization;
using Microsoft.VisualStudio.Shell.Interop;
using Neumont.Tools.Modeling.Shell;

namespace Neumont.Tools.ORM.Shell
{
	partial class ORMReferenceModeEditorToolWindow
	{
		partial class ReferenceModeViewForm
		{
			///	<summary>
			///	Custom reference mode editor.
			///	</summary>
			private sealed class CustomReferenceModeEditor : UserControl
			{
				private class CustomVirtualTreeControl : StandardVirtualTreeControl
				{
					private IServiceProvider myServiceProvider;
					public CustomVirtualTreeControl(IServiceProvider serviceProvider)
					{
						myServiceProvider = serviceProvider;
					}
					/// <summary>
					/// Display a message the way VS wants us to
					/// </summary>
					/// <param name="exception"></param>
					/// <returns></returns>
					protected override bool DisplayException(Exception exception)
					{
						VsShellUtilities.ShowMessageBox(
							myServiceProvider,
							exception.Message,
							ResourceStrings.PackageOfficialName,
							OLEMSGICON.OLEMSGICON_INFO,
							OLEMSGBUTTON.OLEMSGBUTTON_OK,
							OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
						return true;
					}
				}
				private readonly VirtualTreeControl myTree;
				private readonly ReferenceModeHeaderBranch myHeaders;

				/// <summary>
				/// Default constructor
				/// </summary>
				public CustomReferenceModeEditor(IServiceProvider serviceProvider)
				{
					VirtualTreeControl tree = this.myTree = new CustomVirtualTreeControl(serviceProvider);
					this.SuspendLayout();
					// 
					// myTree
					// 
					tree.Dock = DockStyle.Fill;
					tree.HasGridLines = true;
					tree.HasLines = false;
					tree.HasRootLines = false;
					tree.IsDragSource = false;
					tree.LabelEditSupport = VirtualTreeLabelEditActivationStyles.Explicit | VirtualTreeLabelEditActivationStyles.Delayed | VirtualTreeLabelEditActivationStyles.ImmediateSelection;
					tree.MultiColumnHighlight = true;
					tree.Name = "myTree";
					tree.TabIndex = 0;
					// 
					// CustomReferenceModeEditor
					// 
					this.Controls.Add(tree);
					this.Name = "CustomReferenceModeEditor";
					this.Size = new System.Drawing.Size(313, 329);
					this.ResumeLayout(false);

					tree.SetColumnHeaders(new VirtualTreeColumnHeader[]{
						new	VirtualTreeColumnHeader(ResourceStrings.ModelReferenceModeEditorNameColumn),
						new	VirtualTreeColumnHeader(ResourceStrings.ModelReferenceModeEditorKindColumn),
						new	VirtualTreeColumnHeader(ResourceStrings.ModelReferenceModeEditorFormatStringColumn)}
						, true);
					MultiColumnTree treeData = new StandardMultiColumnTree(3);
					((ITree)treeData).Root = myHeaders = new ReferenceModeHeaderBranch();
					tree.MultiColumnTree = (IMultiColumnTree)treeData;
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
	}
}