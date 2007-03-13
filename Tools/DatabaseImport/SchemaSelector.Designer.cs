namespace Neumont.Tools.ORM.DatabaseImport
{
	partial class SchemaSelector
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchemaSelector));
			this.lblSelect = new System.Windows.Forms.Label();
			this.cbxSelectSchema = new System.Windows.Forms.ComboBox();
			this.btnSelect = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblSelect
			// 
			resources.ApplyResources(this.lblSelect, "lblSelect");
			this.lblSelect.Name = "lblSelect";
			// 
			// cbxSelectSchema
			// 
			this.cbxSelectSchema.FormattingEnabled = true;
			resources.ApplyResources(this.cbxSelectSchema, "cbxSelectSchema");
			this.cbxSelectSchema.Name = "cbxSelectSchema";
			// 
			// btnSelect
			// 
			resources.ApplyResources(this.btnSelect, "btnSelect");
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// SchemaSelector
			// 
			this.AcceptButton = this.btnSelect;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSelect);
			this.Controls.Add(this.cbxSelectSchema);
			this.Controls.Add(this.lblSelect);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SchemaSelector";
			this.ShowIcon = false;
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblSelect;
		private System.Windows.Forms.ComboBox cbxSelectSchema;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.Button btnCancel;

	}
}