namespace ORMSolutions.ORMArchitect.Framework.Shell
{
	partial class DiagramOrderDialog
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiagramOrderDialog));
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.UpButton = new System.Windows.Forms.Button();
			this.DownButton = new System.Windows.Forms.Button();
			this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.DiagramsList = new ORMSolutions.ORMArchitect.Framework.Shell.StandardVirtualTreeControl();
			this.SuspendLayout();
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
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// UpButton
			// 
			resources.ApplyResources(this.UpButton, "UpButton");
			this.UpButton.Name = "UpButton";
			this.ToolTip.SetToolTip(this.UpButton, resources.GetString("UpButton.ToolTip"));
			this.UpButton.UseMnemonic = false;
			this.UpButton.UseVisualStyleBackColor = true;
			this.UpButton.Click += new System.EventHandler(this.UpButton_Click);
			// 
			// DownButton
			// 
			resources.ApplyResources(this.DownButton, "DownButton");
			this.DownButton.Name = "DownButton";
			this.ToolTip.SetToolTip(this.DownButton, resources.GetString("DownButton.ToolTip"));
			this.DownButton.UseMnemonic = false;
			this.DownButton.UseVisualStyleBackColor = true;
			this.DownButton.Click += new System.EventHandler(this.DownButton_Click);
			// 
			// DiagramsList
			// 
			this.DiagramsList.AllowDrop = true;
			resources.ApplyResources(this.DiagramsList, "DiagramsList");
			this.DiagramsList.HasButtons = false;
			this.DiagramsList.HasLines = false;
			this.DiagramsList.HasRootButtons = false;
			this.DiagramsList.HasRootLines = false;
			this.DiagramsList.LabelEditSupport = Microsoft.VisualStudio.VirtualTreeGrid.VirtualTreeLabelEditActivationStyles.None;
			this.DiagramsList.MultiColumnHighlight = true;
			this.DiagramsList.Name = "DiagramsList";
			this.DiagramsList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.DiagramsList.SelectionChanged += new System.EventHandler(this.DiagramsList_SelectionChanged);
			// 
			// DiagramOrderDialog
			// 
			this.AcceptButton = this.btnOK;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.DiagramsList);
			this.Controls.Add(this.DownButton);
			this.Controls.Add(this.UpButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.KeyPreview = true;
			this.MinimizeBox = false;
			this.Name = "DiagramOrderDialog";
			this.ShowIcon = false;
			this.Load += new System.EventHandler(this.DiagramOrderDialog_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DiagramOrderDialog_FormClosing);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DiagramOrderDialog_KeyDown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button UpButton;
		private System.Windows.Forms.Button DownButton;
		private System.Windows.Forms.ToolTip ToolTip;
		private StandardVirtualTreeControl DiagramsList;
	}
}