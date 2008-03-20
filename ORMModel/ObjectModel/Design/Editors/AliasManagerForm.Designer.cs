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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AliasManagerForm));
			this.virtualTreeControl = new Neumont.Tools.Modeling.Shell.StandardVirtualTreeControl();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// virtualTreeControl
			// 
			resources.ApplyResources(this.virtualTreeControl, "virtualTreeControl");
			this.virtualTreeControl.HasGridLines = true;
			this.virtualTreeControl.HasHorizontalGridLines = true;
			this.virtualTreeControl.HasLines = false;
			this.virtualTreeControl.HasRootLines = false;
			this.virtualTreeControl.HasVerticalGridLines = true;
			this.virtualTreeControl.LabelEditSupport = ((Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeLabelEditActivationStyles)(((Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeLabelEditActivationStyles.Explicit | Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeLabelEditActivationStyles.Delayed)
						| Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeLabelEditActivationStyles.ImmediateSelection)));
			this.virtualTreeControl.Name = "virtualTreeControl";
			this.virtualTreeControl.LabelEditControlChanged += new System.EventHandler(this.virtualTreeControl_LabelEditControlChanged);
			this.virtualTreeControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.virtualTreeControl_KeyDown);
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// AliasManagerForm
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.virtualTreeControl);
			this.MinimizeBox = false;
			this.Name = "AliasManagerForm";
			this.ShowIcon = false;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AliasManagerForm_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion

		private Neumont.Tools.Modeling.Shell.StandardVirtualTreeControl virtualTreeControl;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
	}
}