namespace Neumont.Tools.ORM.CustomProperties
{
	partial class GroupEditor
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
			this.tbxDescription = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tbxName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnEditDescription = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// tbxDescription
			// 
			this.tbxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tbxDescription.Location = new System.Drawing.Point(80, 29);
			this.tbxDescription.Name = "tbxDescription";
			this.tbxDescription.Size = new System.Drawing.Size(131, 20);
			this.tbxDescription.TabIndex = 3;
			this.tbxDescription.Tag = "description";
			this.tbxDescription.TextChanged += new System.EventHandler(this.tbxTextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(60, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Description";
			// 
			// tbxName
			// 
			this.tbxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tbxName.Location = new System.Drawing.Point(80, 3);
			this.tbxName.Name = "tbxName";
			this.tbxName.Size = new System.Drawing.Size(158, 20);
			this.tbxName.TabIndex = 1;
			this.tbxName.Tag = "name";
			this.tbxName.TextChanged += new System.EventHandler(this.tbxTextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name";
			// 
			// btnEditDescription
			// 
			this.btnEditDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEditDescription.Location = new System.Drawing.Point(213, 27);
			this.btnEditDescription.Name = "btnEditDescription";
			this.btnEditDescription.Size = new System.Drawing.Size(25, 23);
			this.btnEditDescription.TabIndex = 4;
			this.btnEditDescription.Text = "...";
			this.btnEditDescription.UseVisualStyleBackColor = true;
			this.btnEditDescription.Click += new System.EventHandler(this.btnEditDescription_Click);
			// 
			// GroupEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnEditDescription);
			this.Controls.Add(this.tbxDescription);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.tbxName);
			this.Controls.Add(this.label1);
			this.Name = "GroupEditor";
			this.Size = new System.Drawing.Size(238, 300);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbxDescription;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbxName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnEditDescription;
	}
}
