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

namespace Neumont.Tools.ORM.Design
{
	/// <summary>
	/// Customer Reference mode editor usercontrol
	/// </summary>
	partial class CustomReferenceModeEditor
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.myTree = new Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeControl();
			this.SuspendLayout();
// 
// myTree
// 
			this.myTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.myTree.HasGridLines = true;
			this.myTree.HasLines = false;
			this.myTree.HasRootLines = false;
			this.myTree.IsDragSource = false;
			this.myTree.LabelEditSupport = ((Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeLabelEditActivationStyles)((Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeLabelEditActivationStyles.Explicit | Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeLabelEditActivationStyles.Delayed)));
			this.myTree.Location = new System.Drawing.Point(0, 0);
			this.myTree.MultiColumnHighlight = true;
			this.myTree.Name = "myTree";
			this.myTree.Size = new System.Drawing.Size(313, 329);
			this.myTree.TabIndex = 0;
			this.myTree.Text = "virtualTreeControl";
// 
// CustomReferenceModeEditor
// 
			this.Controls.Add(this.myTree);
			this.Name = "CustomReferenceModeEditor";
			this.Size = new System.Drawing.Size(313, 329);
			this.ResumeLayout(false);

		}

		#endregion

		private Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeControl myTree;
	}
}
