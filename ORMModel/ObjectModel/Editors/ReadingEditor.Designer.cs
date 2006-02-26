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

namespace Neumont.Tools.ORM.ObjectModel.Editors
{
	/// <summary>
	/// Generated code behind for ReadingEditor control.
	/// </summary>
	partial class ReadingEditor
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
			this.components = new System.ComponentModel.Container();
			this.mySplitContainer = new System.Windows.Forms.SplitContainer();
			this.tvwReadingOrder = new System.Windows.Forms.TreeView();
			this.vtrReadings = new Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeControl();
			this.myImageList = new System.Windows.Forms.ImageList(this.components);
			this.lstReadings = new System.Windows.Forms.ListBox();
			this.mySplitContainer.Panel1.SuspendLayout();
			this.mySplitContainer.Panel2.SuspendLayout();
			this.mySplitContainer.SuspendLayout();
			this.SuspendLayout();
// 
// mySplitContainer
// 
			this.mySplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mySplitContainer.Location = new System.Drawing.Point(0, 0);
			this.mySplitContainer.Name = "mySplitContainer";
// 
// Panel1
// 
			this.mySplitContainer.Panel1.Controls.Add(this.tvwReadingOrder);
// 
// Panel2
// 
			this.mySplitContainer.Panel2.Controls.Add(this.vtrReadings);
			this.mySplitContainer.Panel2.Controls.Add(this.lstReadings);
			this.mySplitContainer.Size = new System.Drawing.Size(541, 276);
			this.mySplitContainer.SplitterDistance = 181;
			this.mySplitContainer.TabIndex = 6;
			this.mySplitContainer.Text = "splitContainer1";
// 
// tvwReadingOrder
// 
			this.tvwReadingOrder.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvwReadingOrder.HideSelection = false;
			this.tvwReadingOrder.Location = new System.Drawing.Point(0, 0);
			this.tvwReadingOrder.Name = "tvwReadingOrder";
			this.tvwReadingOrder.ShowLines = false;
			this.tvwReadingOrder.Size = new System.Drawing.Size(181, 276);
			this.tvwReadingOrder.TabIndex = 7;
			this.tvwReadingOrder.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwReadingOrder_AfterSelect);
// 
// vtrReadings
// 
			this.vtrReadings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.vtrReadings.HasGridLines = true;
			this.vtrReadings.HasLines = false;
			this.vtrReadings.HasRootButtons = false;
			this.vtrReadings.HasRootLines = false;
			this.vtrReadings.ImageList = this.myImageList;
			this.vtrReadings.LabelEditSupport = ((Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeLabelEditActivationStyles)((Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeLabelEditActivationStyles.Explicit | Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeLabelEditActivationStyles.Delayed)));
			this.vtrReadings.Location = new System.Drawing.Point(0, 0);
			this.vtrReadings.Name = "vtrReadings";
			this.vtrReadings.Size = new System.Drawing.Size(356, 276);
			this.vtrReadings.StandardCheckBoxes = true;
			this.vtrReadings.TabIndex = 1;
			this.vtrReadings.Text = "Readings";
// 
// myImageList
// 
			this.myImageList.ImageSize = new System.Drawing.Size(16, 16);
			this.myImageList.TransparentColor = System.Drawing.Color.Transparent;
// 
// lstReadings
// 
			this.lstReadings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstReadings.FormattingEnabled = true;
			this.lstReadings.IntegralHeight = false;
			this.lstReadings.Location = new System.Drawing.Point(0, 0);
			this.lstReadings.Name = "lstReadings";
			this.lstReadings.Size = new System.Drawing.Size(356, 276);
			this.lstReadings.TabIndex = 0;
// 
// ReadingEditor
// 
			this.Controls.Add(this.mySplitContainer);
			this.Name = "ReadingEditor";
			this.Size = new System.Drawing.Size(541, 276);
			this.mySplitContainer.Panel1.ResumeLayout(false);
			this.mySplitContainer.Panel2.ResumeLayout(false);
			this.mySplitContainer.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer mySplitContainer;
		private System.Windows.Forms.TreeView tvwReadingOrder;
		private System.Windows.Forms.ListBox lstReadings;
		private Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeControl vtrReadings;
		private System.Windows.Forms.ImageList myImageList;




	}
}
