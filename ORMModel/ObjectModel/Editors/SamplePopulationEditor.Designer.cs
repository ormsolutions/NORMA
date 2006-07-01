namespace Neumont.Tools.ORM.ObjectModel.Editors
{
	partial class SamplePopulationEditor
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
			this.vtrSamplePopulation = new Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeControl();
			this.lblNoSelection = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// vtrSamplePopulation
			// 
			this.vtrSamplePopulation.Dock = System.Windows.Forms.DockStyle.Fill;
			this.vtrSamplePopulation.HasGridLines = true;
			this.vtrSamplePopulation.HasHorizontalGridLines = true;
			this.vtrSamplePopulation.HasVerticalGridLines = true;
			this.vtrSamplePopulation.LabelEditSupport = Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeLabelEditActivationStyles.Delayed;
			this.vtrSamplePopulation.Location = new System.Drawing.Point(0, 0);
			this.vtrSamplePopulation.Name = "vtrSamplePopulation";
			this.vtrSamplePopulation.Size = new System.Drawing.Size(407, 179);
			this.vtrSamplePopulation.TabIndex = 0;
			this.vtrSamplePopulation.Text = "virtualTreeControl1";
			this.vtrSamplePopulation.Visible = false;
			this.vtrSamplePopulation.SelectionChanged += new System.EventHandler(this.vtrSamplePopulation_SelectionChanged);
			// 
			// lblNoSelection
			// 
			this.lblNoSelection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblNoSelection.Location = new System.Drawing.Point(0, 0);
			this.lblNoSelection.Name = "lblNoSelection";
			this.lblNoSelection.Size = new System.Drawing.Size(407, 179);
			this.lblNoSelection.TabIndex = 1;
			this.lblNoSelection.Text = "Please select a FactType, a ValueType, or an EntityType with a preferred identifier to begin editing instances.";
			this.lblNoSelection.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// SamplePopulationEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.vtrSamplePopulation);
			this.Controls.Add(this.lblNoSelection);
			this.Name = "SamplePopulationEditor";
			this.Size = new System.Drawing.Size(407, 179);
			this.ResumeLayout(false);

		}

		#endregion

		private Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeControl vtrSamplePopulation;
		private System.Windows.Forms.Label lblNoSelection;

	}
}
