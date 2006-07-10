using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.VirtualTreeGrid;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.Shell;

namespace Neumont.Tools.ORM.Design
{
	/// <summary>
	/// Generated code behind for ReadingEditor control.
	/// </summary>
	partial class ReadingEditor : UserControl
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReadingEditor));
			this.myImageList = new System.Windows.Forms.ImageList(this.components);
			this.mySplitContainer = new System.Windows.Forms.ContainerControl();
			this.vtrReadings = new Neumont.Tools.ORM.Design.ReadingEditor.CustomVirtualTreeControl();
			this.mySplitContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// myImageList
			// 
			this.myImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("myImageList.ImageStream")));
			this.myImageList.TransparentColor = System.Drawing.Color.Transparent;
			this.myImageList.Images.SetKeyName(0, "Search 2.ico");
			// 
			// mySplitContainer
			// 
			this.mySplitContainer.Controls.Add(this.vtrReadings);
			this.mySplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mySplitContainer.Location = new System.Drawing.Point(0, 0);
			this.mySplitContainer.Name = "mySplitContainer";
			this.mySplitContainer.Size = new System.Drawing.Size(541, 276);
			this.mySplitContainer.TabIndex = 6;
			this.mySplitContainer.Text = "splitContainer1";
			// 
			// vtrReadings
			// 
			this.vtrReadings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.vtrReadings.HasHorizontalGridLines = true;
			this.vtrReadings.HasLines = false;
			this.vtrReadings.HasRootButtons = false;
			this.vtrReadings.HasRootLines = false;
			this.vtrReadings.ImageList = this.myImageList;
			this.vtrReadings.LabelEditSupport = ((VirtualTreeLabelEditActivationStyles)((((VirtualTreeLabelEditActivationStyles.Explicit | VirtualTreeLabelEditActivationStyles.Delayed)
						| VirtualTreeLabelEditActivationStyles.ImmediateMouse)
						| VirtualTreeLabelEditActivationStyles.ImmediateSelection)));
			this.vtrReadings.Location = new System.Drawing.Point(0, 0);
			this.vtrReadings.Name = "vtrReadings";
			this.vtrReadings.Size = new System.Drawing.Size(541, 276);
			this.vtrReadings.TabIndex = 1;
			this.vtrReadings.Text = "Readings";
			this.vtrReadings.ContextMenuInvoked += new ContextMenuEventHandler(this.OnContextMenuInvoked);
			this.vtrReadings.SelectionChanged += new EventHandler(this.OnTreeControlSelectionChanged);
			// 
			// ReadingEditor
			// 
			this.Controls.Add(this.mySplitContainer);
			this.Name = "ReadingEditor";
			this.Size = new System.Drawing.Size(541, 276);
			this.mySplitContainer.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ContainerControl mySplitContainer;
						//Must use the Custom control which overrides the height for headers for use with
						//column permutation when no header is desired -- see summary of CustomVirtualTreeControl
		private CustomVirtualTreeControl vtrReadings;
	}
}
