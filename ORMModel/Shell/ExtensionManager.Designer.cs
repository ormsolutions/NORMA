namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// The form that allows users to select or deselect Custom Extensions.
	/// </summary>
	partial class ExtensionManager
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtensionManager));
			this.lvExtensions = new System.Windows.Forms.ListView();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
			this.label1 = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lvExtensions
			// 
			resources.ApplyResources(this.lvExtensions, "lvExtensions");
			this.lvExtensions.CheckBoxes = true;
			this.lvExtensions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
			this.columnHeader4,
			this.columnHeader5,
			this.columnHeader6});
			this.lvExtensions.Name = "lvExtensions";
			this.lvExtensions.UseCompatibleStateImageBehavior = false;
			this.lvExtensions.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader4
			// 
			resources.ApplyResources(this.columnHeader4, "columnHeader4");
			// 
			// columnHeader5
			// 
			resources.ApplyResources(this.columnHeader5, "columnHeader5");
			// 
			// columnHeader6
			// 
			resources.ApplyResources(this.columnHeader6, "columnHeader6");
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Name = "label1";
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// ExtensionManager
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lvExtensions);
			this.MinimizeBox = false;
			this.Name = "ExtensionManager";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView lvExtensions;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
	}
}
