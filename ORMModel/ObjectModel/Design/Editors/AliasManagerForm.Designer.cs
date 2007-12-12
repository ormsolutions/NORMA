namespace Neumont.Tools.ORM.ObjectModel.Design
{
	partial class AliasManagerForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.virtualTreeControl = new Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeControl();
			this.SuspendLayout();
			// 
			// virtualTreeControl
			// 
			this.virtualTreeControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.virtualTreeControl.Location = new System.Drawing.Point(0, 0);
			this.virtualTreeControl.Name = "virtualTreeControl";
			this.virtualTreeControl.Size = new System.Drawing.Size(324, 257);
			this.virtualTreeControl.TabIndex = 0;
			this.virtualTreeControl.Text = "customVirtualTreeControl1";
			// 
			// AliasManagerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(324, 257);
			this.Controls.Add(this.virtualTreeControl);
			this.Name = "AliasManagerForm";
			this.Text = "Alias Manager";
			this.ResumeLayout(false);

		}

		#endregion

		private Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeControl virtualTreeControl;
	}
}