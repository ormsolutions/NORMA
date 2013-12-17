namespace ORMSolutions.ORMArchitect.CustomProperties
{
	partial class DefinitionEditor
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
			System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node5");
			System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Node6");
			System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
			System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Node10");
			System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Node11");
			System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Node9", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5});
			System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Node13");
			System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Node14");
			System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Node12", new System.Windows.Forms.TreeNode[] {
            treeNode7,
            treeNode8});
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.tvModelElements = new System.Windows.Forms.TreeView();
			this.btnEditCustomEnum = new System.Windows.Forms.Button();
			this.tbxCustomEnum = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.tbxDefaultValue = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cmbxDataType = new System.Windows.Forms.ComboBox();
			this.tbxCategory = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.tbxDescription = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tbxName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnEditDescription = new System.Windows.Forms.Button();
			this.chkVerbalizeDefaultValue = new System.Windows.Forms.CheckBox();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.tvModelElements);
			this.groupBox2.Location = new System.Drawing.Point(2, 197);
			this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(314, 202);
			this.groupBox2.TabIndex = 15;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "&Model Elements";
			// 
			// tvModelElements
			// 
			this.tvModelElements.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tvModelElements.CheckBoxes = true;
			this.tvModelElements.Location = new System.Drawing.Point(3, 19);
			this.tvModelElements.Name = "tvModelElements";
			treeNode1.Name = "Node5";
			treeNode1.Text = "Node5";
			treeNode2.Name = "Node6";
			treeNode2.Text = "Node6";
			treeNode3.Name = "Node0";
			treeNode3.Text = "Node0";
			treeNode4.Name = "Node10";
			treeNode4.Text = "Node10";
			treeNode5.Name = "Node11";
			treeNode5.Text = "Node11";
			treeNode6.Name = "Node9";
			treeNode6.Text = "Node9";
			treeNode7.Name = "Node13";
			treeNode7.Text = "Node13";
			treeNode8.Name = "Node14";
			treeNode8.Text = "Node14";
			treeNode9.Name = "Node12";
			treeNode9.Text = "Node12";
			this.tvModelElements.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode6,
            treeNode9});
			this.tvModelElements.Size = new System.Drawing.Size(305, 177);
			this.tvModelElements.TabIndex = 0;
			this.tvModelElements.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvModelElements_AfterCheck);
			// 
			// btnEditCustomEnum
			// 
			this.btnEditCustomEnum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEditCustomEnum.Location = new System.Drawing.Point(284, 162);
			this.btnEditCustomEnum.Name = "btnEditCustomEnum";
			this.btnEditCustomEnum.Size = new System.Drawing.Size(33, 28);
			this.btnEditCustomEnum.TabIndex = 14;
			this.btnEditCustomEnum.Text = "...";
			this.btnEditCustomEnum.UseVisualStyleBackColor = true;
			this.btnEditCustomEnum.Visible = false;
			this.btnEditCustomEnum.Click += new System.EventHandler(this.btnEditCustomEnum_Click);
			// 
			// tbxCustomEnum
			// 
			this.tbxCustomEnum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tbxCustomEnum.Location = new System.Drawing.Point(105, 165);
			this.tbxCustomEnum.Name = "tbxCustomEnum";
			this.tbxCustomEnum.Size = new System.Drawing.Size(176, 22);
			this.tbxCustomEnum.TabIndex = 13;
			this.tbxCustomEnum.Visible = false;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(2, 169);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(95, 17);
			this.label4.TabIndex = 12;
			this.label4.Text = "Custom Enum";
			this.label4.Visible = false;
			// 
			// tbxDefaultValue
			// 
			this.tbxDefaultValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tbxDefaultValue.Location = new System.Drawing.Point(105, 133);
			this.tbxDefaultValue.Name = "tbxDefaultValue";
			this.tbxDefaultValue.Size = new System.Drawing.Size(209, 22);
			this.tbxDefaultValue.TabIndex = 10;
			this.tbxDefaultValue.Tag = "defaultValue";
			this.tbxDefaultValue.TextChanged += new System.EventHandler(this.tbx_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(2, 137);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(93, 17);
			this.label3.TabIndex = 9;
			this.label3.Text = "Default &Value";
			// 
			// cmbxDataType
			// 
			this.cmbxDataType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.cmbxDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbxDataType.FormattingEnabled = true;
			this.cmbxDataType.Items.AddRange(new object[] {
            "string",
            "integer",
            "decimal",
            "datetime"});
			this.cmbxDataType.Location = new System.Drawing.Point(105, 100);
			this.cmbxDataType.Name = "cmbxDataType";
			this.cmbxDataType.Size = new System.Drawing.Size(209, 24);
			this.cmbxDataType.TabIndex = 8;
			this.cmbxDataType.SelectedIndexChanged += new System.EventHandler(this.cmbxDataType_SelectedIndexChanged);
			// 
			// tbxCategory
			// 
			this.tbxCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tbxCategory.Location = new System.Drawing.Point(105, 68);
			this.tbxCategory.Name = "tbxCategory";
			this.tbxCategory.Size = new System.Drawing.Size(209, 22);
			this.tbxCategory.TabIndex = 6;
			this.tbxCategory.Tag = "category";
			this.tbxCategory.TextChanged += new System.EventHandler(this.tbx_TextChanged);
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(2, 71);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(65, 17);
			this.label10.TabIndex = 5;
			this.label10.Text = "&Category";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(2, 103);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(74, 17);
			this.label9.TabIndex = 7;
			this.label9.Text = "Data &Type";
			// 
			// tbxDescription
			// 
			this.tbxDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tbxDescription.Location = new System.Drawing.Point(105, 36);
			this.tbxDescription.Name = "tbxDescription";
			this.tbxDescription.Size = new System.Drawing.Size(176, 22);
			this.tbxDescription.TabIndex = 3;
			this.tbxDescription.Tag = "description";
			this.tbxDescription.TextChanged += new System.EventHandler(this.tbx_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(2, 39);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(79, 17);
			this.label2.TabIndex = 2;
			this.label2.Text = "&Description";
			// 
			// tbxName
			// 
			this.tbxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tbxName.Location = new System.Drawing.Point(105, 4);
			this.tbxName.Name = "tbxName";
			this.tbxName.Size = new System.Drawing.Size(209, 22);
			this.tbxName.TabIndex = 1;
			this.tbxName.Tag = "name";
			this.tbxName.TextChanged += new System.EventHandler(this.tbx_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(2, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(45, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "&Name";
			// 
			// btnEditDescription
			// 
			this.btnEditDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEditDescription.Location = new System.Drawing.Point(284, 33);
			this.btnEditDescription.Name = "btnEditDescription";
			this.btnEditDescription.Size = new System.Drawing.Size(33, 28);
			this.btnEditDescription.TabIndex = 4;
			this.btnEditDescription.Text = "...";
			this.btnEditDescription.UseVisualStyleBackColor = true;
			this.btnEditDescription.Click += new System.EventHandler(this.btnEditDescription_Click);
			// 
			// chkVerbalizeDefaultValue
			// 
			this.chkVerbalizeDefaultValue.AutoSize = true;
			this.chkVerbalizeDefaultValue.Location = new System.Drawing.Point(2, 169);
			this.chkVerbalizeDefaultValue.Name = "chkVerbalizeDefaultValue";
			this.chkVerbalizeDefaultValue.Size = new System.Drawing.Size(178, 21);
			this.chkVerbalizeDefaultValue.TabIndex = 11;
			this.chkVerbalizeDefaultValue.Tag = "verbalizeDefaultValue";
			this.chkVerbalizeDefaultValue.Text = "Ver&balize Default Value";
			this.chkVerbalizeDefaultValue.UseVisualStyleBackColor = true;
			this.chkVerbalizeDefaultValue.CheckedChanged += new System.EventHandler(this.chkVerbalizeDefaultValue_CheckedChanged);
			// 
			// DefinitionEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.chkVerbalizeDefaultValue);
			this.Controls.Add(this.btnEditDescription);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.btnEditCustomEnum);
			this.Controls.Add(this.tbxCustomEnum);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.tbxDefaultValue);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cmbxDataType);
			this.Controls.Add(this.tbxCategory);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.tbxDescription);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.tbxName);
			this.Controls.Add(this.label1);
			this.Name = "DefinitionEditor";
			this.Size = new System.Drawing.Size(317, 399);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button btnEditCustomEnum;
		private System.Windows.Forms.TextBox tbxCustomEnum;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox tbxDefaultValue;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cmbxDataType;
		private System.Windows.Forms.TextBox tbxCategory;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox tbxDescription;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbxName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnEditDescription;
		private System.Windows.Forms.TreeView tvModelElements;
		private System.Windows.Forms.CheckBox chkVerbalizeDefaultValue;
	}
}
