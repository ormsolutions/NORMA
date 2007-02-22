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
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(232, 292);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(82, 24);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Location = new System.Drawing.Point(12, 9);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(87, 13);
            this.lblOutput.TabIndex = 1;
            this.lblOutput.Text = "Output Directory:";
            this.lblOutput.Click += new System.EventHandler(this.lblOutput_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.CausesValidation = false;
            this.btnBrowse.Location = new System.Drawing.Point(290, 6);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(26, 20);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Enabled = false;
            this.btnGenerate.Location = new System.Drawing.Point(115, 292);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(110, 24);
            this.btnGenerate.TabIndex = 2;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // txtOutputDirectory
            // 
            this.txtOutputDirectory.CausesValidation = false;
            this.txtOutputDirectory.Location = new System.Drawing.Point(105, 6);
            this.txtOutputDirectory.Name = "txtOutputDirectory";
            this.txtOutputDirectory.Size = new System.Drawing.Size(183, 20);
            this.txtOutputDirectory.TabIndex = 3;
            this.txtOutputDirectory.TextChanged += new System.EventHandler(this.txtOutputDirectory_TextChanged);
            // 
            // gbOptions
            // 
            this.gbOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbOptions.Controls.Add(this.chkLbOptions);
            this.gbOptions.Location = new System.Drawing.Point(12, 43);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Size = new System.Drawing.Size(306, 240);
            this.gbOptions.TabIndex = 1;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Options:";
            // 
            // chkLbOptions
            // 
            this.chkLbOptions.CausesValidation = false;
            this.chkLbOptions.CheckOnClick = true;
            this.chkLbOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkLbOptions.FormattingEnabled = true;
            this.chkLbOptions.Location = new System.Drawing.Point(3, 16);
            this.chkLbOptions.Name = "chkLbOptions";
            this.chkLbOptions.Size = new System.Drawing.Size(300, 214);
            this.chkLbOptions.TabIndex = 0;
            this.chkLbOptions.Value = null;
            // 
            // GenerateReportDialog
            // 
            this.AcceptButton = this.btnGenerate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(329, 328);
            this.Controls.Add(this.gbOptions);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblOutput);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtOutputDirectory);
            this.Controls.Add(this.btnGenerate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "GenerateReportDialog";
            this.Text = "Verbalization Report Generator";
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