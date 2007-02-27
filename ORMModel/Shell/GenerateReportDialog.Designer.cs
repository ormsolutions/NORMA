namespace Neumont.Tools.ORM.Shell
{
    partial class GenerateReportDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenerateReportDialog));
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblOutput = new System.Windows.Forms.Label();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.btnGenerate = new System.Windows.Forms.Button();
			this.txtOutputDirectory = new System.Windows.Forms.TextBox();
			this.gbOptions = new System.Windows.Forms.GroupBox();
			this.chkLbOptions = new Neumont.Tools.Modeling.Design.FlagsEnumListBox();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.gbOptions.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// lblOutput
			// 
			resources.ApplyResources(this.lblOutput, "lblOutput");
			this.lblOutput.Name = "lblOutput";
			this.lblOutput.Click += new System.EventHandler(this.lblOutput_Click);
			// 
			// btnBrowse
			// 
			this.btnBrowse.CausesValidation = false;
			resources.ApplyResources(this.btnBrowse, "btnBrowse");
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// btnGenerate
			// 
			resources.ApplyResources(this.btnGenerate, "btnGenerate");
			this.btnGenerate.Name = "btnGenerate";
			this.btnGenerate.UseVisualStyleBackColor = true;
			this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
			// 
			// txtOutputDirectory
			// 
			this.txtOutputDirectory.CausesValidation = false;
			resources.ApplyResources(this.txtOutputDirectory, "txtOutputDirectory");
			this.txtOutputDirectory.Name = "txtOutputDirectory";
			this.txtOutputDirectory.TextChanged += new System.EventHandler(this.txtOutputDirectory_TextChanged);
			// 
			// gbOptions
			// 
			resources.ApplyResources(this.gbOptions, "gbOptions");
			this.gbOptions.Controls.Add(this.chkLbOptions);
			this.gbOptions.Name = "gbOptions";
			this.gbOptions.TabStop = false;
			// 
			// chkLbOptions
			// 
			this.chkLbOptions.CausesValidation = false;
			this.chkLbOptions.CheckOnClick = true;
			resources.ApplyResources(this.chkLbOptions, "chkLbOptions");
			this.chkLbOptions.FormattingEnabled = true;
			this.chkLbOptions.Name = "chkLbOptions";
			this.chkLbOptions.Value = null;
			// 
			// GenerateReportDialog
			// 
			this.AcceptButton = this.btnGenerate;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ControlBox = false;
			this.Controls.Add(this.gbOptions);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.lblOutput);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.txtOutputDirectory);
			this.Controls.Add(this.btnGenerate);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "GenerateReportDialog";
			this.ShowIcon = false;
			this.gbOptions.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.TextBox txtOutputDirectory;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.GroupBox gbOptions;
        private Neumont.Tools.Modeling.Design.FlagsEnumListBox chkLbOptions;
        private System.Windows.Forms.Button btnCancel;
    }
}