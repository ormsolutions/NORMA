namespace Neumont.Tools.ORM.Shell
{
	partial class ImportStepOptionsBase
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportStepOptionsBase));
			this.TransformsLabel = new System.Windows.Forms.Label();
			this.ctlTransformsCombo = new System.Windows.Forms.ComboBox();
			this.PropertiesLabel = new System.Windows.Forms.Label();
			this.ctlTransformOptionsPropertyGrid = new System.Windows.Forms.PropertyGrid();
			this.OKButton = new System.Windows.Forms.Button();
			this.AbortButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// TransformsLabel
			// 
			resources.ApplyResources(this.TransformsLabel, "TransformsLabel");
			this.TransformsLabel.Name = "TransformsLabel";
			// 
			// ctlTransformsCombo
			// 
			resources.ApplyResources(this.ctlTransformsCombo, "ctlTransformsCombo");
			this.ctlTransformsCombo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.ctlTransformsCombo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.ctlTransformsCombo.DisplayMember = "Description";
			this.ctlTransformsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ctlTransformsCombo.FormattingEnabled = true;
			this.ctlTransformsCombo.Name = "ctlTransformsCombo";
			this.ctlTransformsCombo.Sorted = true;
			// 
			// PropertiesLabel
			// 
			resources.ApplyResources(this.PropertiesLabel, "PropertiesLabel");
			this.PropertiesLabel.Name = "PropertiesLabel";
			// 
			// ctlTransformOptionsPropertyGrid
			// 
			resources.ApplyResources(this.ctlTransformOptionsPropertyGrid, "ctlTransformOptionsPropertyGrid");
			this.ctlTransformOptionsPropertyGrid.CommandsVisibleIfAvailable = false;
			this.ctlTransformOptionsPropertyGrid.Name = "ctlTransformOptionsPropertyGrid";
			this.ctlTransformOptionsPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
			this.ctlTransformOptionsPropertyGrid.ToolbarVisible = false;
			// 
			// OKButton
			// 
			resources.ApplyResources(this.OKButton, "OKButton");
			this.OKButton.Name = "OKButton";
			this.OKButton.UseVisualStyleBackColor = true;
			this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
			// 
			// AbortButton
			// 
			resources.ApplyResources(this.AbortButton, "AbortButton");
			this.AbortButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.AbortButton.Name = "AbortButton";
			this.AbortButton.UseVisualStyleBackColor = true;
			// 
			// ImportStepOptionsBase
			// 
			this.AcceptButton = this.OKButton;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.AbortButton;
			this.Controls.Add(this.AbortButton);
			this.Controls.Add(this.OKButton);
			this.Controls.Add(this.ctlTransformOptionsPropertyGrid);
			this.Controls.Add(this.PropertiesLabel);
			this.Controls.Add(this.ctlTransformsCombo);
			this.Controls.Add(this.TransformsLabel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ImportStepOptionsBase";
			this.ShowIcon = false;
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label TransformsLabel;
		private System.Windows.Forms.Label PropertiesLabel;
		private System.Windows.Forms.Button OKButton;
		private System.Windows.Forms.Button AbortButton;
		private System.Windows.Forms.ComboBox ctlTransformsCombo;
		private System.Windows.Forms.PropertyGrid ctlTransformOptionsPropertyGrid;
	}
}