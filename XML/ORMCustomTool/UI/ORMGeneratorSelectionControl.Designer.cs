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

namespace Neumont.Tools.ORM.ORMCustomTool
{
	partial class ORMGeneratorSelectionControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.Label label_GeneratedFilesFor;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ORMGeneratorSelectionControl));
			this.virtualTreeControl = new Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeControl();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.textBox_ORMFileName = new System.Windows.Forms.TextBox();
			this.button_Cancel = new System.Windows.Forms.Button();
			this.button_SaveChanges = new System.Windows.Forms.Button();
			label_GeneratedFilesFor = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label_GeneratedFilesFor
			// 
			resources.ApplyResources(label_GeneratedFilesFor, "label_GeneratedFilesFor");
			label_GeneratedFilesFor.FlatStyle = System.Windows.Forms.FlatStyle.System;
			label_GeneratedFilesFor.Name = "label_GeneratedFilesFor";
			// 
			// virtualTreeControl
			// 
			resources.ApplyResources(this.virtualTreeControl, "virtualTreeControl");
			this.virtualTreeControl.ImageList = this.imageList1;
			this.virtualTreeControl.Name = "virtualTreeControl";
			this.virtualTreeControl.StandardCheckBoxes = true;
			// 
			// imageList1
			// 
			this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			resources.ApplyResources(this.imageList1, "imageList1");
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// textBox_ORMFileName
			// 
			resources.ApplyResources(this.textBox_ORMFileName, "textBox_ORMFileName");
			this.textBox_ORMFileName.Name = "textBox_ORMFileName";
			this.textBox_ORMFileName.ReadOnly = true;
			this.textBox_ORMFileName.TabStop = false;
			// 
			// button_Cancel
			// 
			resources.ApplyResources(this.button_Cancel, "button_Cancel");
			this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button_Cancel.Name = "button_Cancel";
			this.button_Cancel.UseVisualStyleBackColor = true;
			// 
			// button_SaveChanges
			// 
			resources.ApplyResources(this.button_SaveChanges, "button_SaveChanges");
			this.button_SaveChanges.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button_SaveChanges.Name = "button_SaveChanges";
			this.button_SaveChanges.UseVisualStyleBackColor = true;
			// 
			// ORMGeneratorSelectionControl
			// 
			this.AcceptButton = this.button_SaveChanges;
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.CancelButton = this.button_Cancel;
			resources.ApplyResources(this, "$this");
			this.Controls.Add(this.button_SaveChanges);
			this.Controls.Add(this.button_Cancel);
			this.Controls.Add(this.textBox_ORMFileName);
			this.Controls.Add(label_GeneratedFilesFor);
			this.Controls.Add(this.virtualTreeControl);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "ORMGeneratorSelectionControl";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeControl virtualTreeControl;
		private System.Windows.Forms.TextBox textBox_ORMFileName;
		private System.Windows.Forms.Button button_Cancel;
		private System.Windows.Forms.Button button_SaveChanges;
		private System.Windows.Forms.ImageList imageList1;


	}
}
