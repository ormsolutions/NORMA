using Microsoft.VisualStudio.VirtualTreeGrid;
namespace Neumont.Tools.ORM.Framework.DynamicSurveyTreeGrid
{
	partial class SurveyTreeControl
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
			this.myTreeControl = new Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeControl();
			this.SuspendLayout();
			// 
			// myTreeControl
			// 
			this.myTreeControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.myTreeControl.Location = new System.Drawing.Point(0, 0);
			this.myTreeControl.Name = "myTreeControl";
			this.myTreeControl.Size = new System.Drawing.Size(150, 150);
			this.myTreeControl.TabIndex = 0;
			// 
			// SurveyTreeControl
			// 
			this.Controls.Add(this.myTreeControl);
			this.Name = "SurveyTreeControl";
			this.ResumeLayout(false);

		}
		private VirtualTreeControl myTreeControl;
		#endregion
	}
}
