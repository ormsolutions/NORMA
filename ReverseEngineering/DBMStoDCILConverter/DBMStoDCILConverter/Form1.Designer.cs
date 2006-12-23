namespace DBMStoDCILConverter
{
	partial class Form1
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
			this.listView1 = new System.Windows.Forms.ListView();
			this.colName = new System.Windows.Forms.ColumnHeader();
			this.btnSelectAll = new System.Windows.Forms.Button();
			this.btnSelectNone = new System.Windows.Forms.Button();
			this.txtOutputFile = new System.Windows.Forms.TextBox();
			this.lblOutput = new System.Windows.Forms.Label();
			this.btnConvert = new System.Windows.Forms.Button();
			this.txtSchema = new System.Windows.Forms.TextBox();
			this.lblSchema = new System.Windows.Forms.Label();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// listView1
			// 
			this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)));
			this.listView1.CheckBoxes = true;
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName});
			this.listView1.GridLines = true;
			this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listView1.Location = new System.Drawing.Point(12, 69);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(558, 442);
			this.listView1.TabIndex = 2;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// colName
			// 
			this.colName.Text = "Name";
			this.colName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.colName.Width = 537;
			// 
			// btnSelectAll
			// 
			this.btnSelectAll.Location = new System.Drawing.Point(11, 517);
			this.btnSelectAll.Name = "btnSelectAll";
			this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
			this.btnSelectAll.TabIndex = 3;
			this.btnSelectAll.Text = "Select All";
			this.btnSelectAll.UseVisualStyleBackColor = true;
			this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
			// 
			// btnSelectNone
			// 
			this.btnSelectNone.Location = new System.Drawing.Point(92, 517);
			this.btnSelectNone.Name = "btnSelectNone";
			this.btnSelectNone.Size = new System.Drawing.Size(75, 23);
			this.btnSelectNone.TabIndex = 4;
			this.btnSelectNone.Text = "Select None";
			this.btnSelectNone.UseVisualStyleBackColor = true;
			this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
			// 
			// txtOutputFile
			// 
			this.txtOutputFile.Location = new System.Drawing.Point(76, 12);
			this.txtOutputFile.Name = "txtOutputFile";
			this.txtOutputFile.Size = new System.Drawing.Size(346, 20);
			this.txtOutputFile.TabIndex = 5;
			// 
			// lblOutput
			// 
			this.lblOutput.AutoSize = true;
			this.lblOutput.Location = new System.Drawing.Point(12, 15);
			this.lblOutput.Name = "lblOutput";
			this.lblOutput.Size = new System.Drawing.Size(58, 13);
			this.lblOutput.TabIndex = 6;
			this.lblOutput.Text = "Output File";
			// 
			// btnConvert
			// 
			this.btnConvert.Location = new System.Drawing.Point(428, 23);
			this.btnConvert.Name = "btnConvert";
			this.btnConvert.Size = new System.Drawing.Size(142, 23);
			this.btnConvert.TabIndex = 7;
			this.btnConvert.Text = "Convert";
			this.btnConvert.UseVisualStyleBackColor = true;
			this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
			// 
			// txtSchema
			// 
			this.txtSchema.Location = new System.Drawing.Point(76, 38);
			this.txtSchema.Name = "txtSchema";
			this.txtSchema.Size = new System.Drawing.Size(346, 20);
			this.txtSchema.TabIndex = 8;
			// 
			// lblSchema
			// 
			this.lblSchema.AutoSize = true;
			this.lblSchema.Location = new System.Drawing.Point(12, 41);
			this.lblSchema.Name = "lblSchema";
			this.lblSchema.Size = new System.Drawing.Size(46, 13);
			this.lblSchema.TabIndex = 9;
			this.lblSchema.Text = "Schema";
			// 
			// dataGridView1
			// 
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Location = new System.Drawing.Point(590, 15);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(559, 519);
			this.dataGridView1.TabIndex = 10;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1159, 546);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.lblSchema);
			this.Controls.Add(this.txtSchema);
			this.Controls.Add(this.btnConvert);
			this.Controls.Add(this.lblOutput);
			this.Controls.Add(this.txtOutputFile);
			this.Controls.Add(this.btnSelectNone);
			this.Controls.Add(this.btnSelectAll);
			this.Controls.Add(this.listView1);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.Button btnSelectAll;
		private System.Windows.Forms.Button btnSelectNone;
		private System.Windows.Forms.TextBox txtOutputFile;
		private System.Windows.Forms.Label lblOutput;
		private System.Windows.Forms.Button btnConvert;
		private System.Windows.Forms.TextBox txtSchema;
		private System.Windows.Forms.Label lblSchema;
		private System.Windows.Forms.DataGridView dataGridView1;

	}
}

