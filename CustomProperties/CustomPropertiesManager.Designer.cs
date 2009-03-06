namespace ORMSolutions.ORMArchitect.CustomProperties
{
	partial class CustomPropertiesManager
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomPropertiesManager));
			System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Machine");
			System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Model");
			this.tsbAddDefinition = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbDelete = new System.Windows.Forms.ToolStripButton();
			this.tsbDefaultToggle = new System.Windows.Forms.ToolStripButton();
			this.tvCustomProperties = new System.Windows.Forms.TreeView();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tsbAddGroup = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbAddGroupToMachine = new System.Windows.Forms.ToolStripButton();
			this.tsbAddGroupToModel = new System.Windows.Forms.ToolStripButton();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.definitionEditor1 = new ORMSolutions.ORMArchitect.CustomProperties.DefinitionEditor();
			this.groupEditor1 = new ORMSolutions.ORMArchitect.CustomProperties.GroupEditor();
			this.groupBox1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tsbAddDefinition
			// 
			this.tsbAddDefinition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbAddDefinition.Image = ((System.Drawing.Image)(resources.GetObject("tsbAddDefinition.Image")));
			this.tsbAddDefinition.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbAddDefinition.Name = "tsbAddDefinition";
			this.tsbAddDefinition.Size = new System.Drawing.Size(23, 22);
			this.tsbAddDefinition.Text = "Add Property";
			this.tsbAddDefinition.Click += new System.EventHandler(this.tsbAddDefinition_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// tsbDelete
			// 
			this.tsbDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbDelete.Enabled = false;
			this.tsbDelete.Image = ((System.Drawing.Image)(resources.GetObject("tsbDelete.Image")));
			this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbDelete.Name = "tsbDelete";
			this.tsbDelete.Size = new System.Drawing.Size(23, 22);
			this.tsbDelete.Text = "Delete selected group or definition";
			this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
			// 
			// tsbDefaultToggle
			// 
			this.tsbDefaultToggle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbDefaultToggle.Enabled = false;
			this.tsbDefaultToggle.Image = ((System.Drawing.Image)(resources.GetObject("tsbDefaultToggle.Image")));
			this.tsbDefaultToggle.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbDefaultToggle.Name = "tsbDefaultToggle";
			this.tsbDefaultToggle.Size = new System.Drawing.Size(23, 22);
			this.tsbDefaultToggle.Text = "Toggle selected group\'s default status";
			this.tsbDefaultToggle.Click += new System.EventHandler(this.tsbDefaultToggle_Click);
			// 
			// tvCustomProperties
			// 
			this.tvCustomProperties.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvCustomProperties.HideSelection = false;
			this.tvCustomProperties.Location = new System.Drawing.Point(3, 41);
			this.tvCustomProperties.Name = "tvCustomProperties";
			treeNode1.Name = "Node0";
			treeNode1.Text = "Machine";
			treeNode2.Name = "Node1";
			treeNode2.Text = "Model";
			this.tvCustomProperties.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
			this.tvCustomProperties.Size = new System.Drawing.Size(214, 307);
			this.tvCustomProperties.TabIndex = 1;
			this.tvCustomProperties.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvCustomProperties_AfterSelect);
			this.tvCustomProperties.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tvCustomProperties_KeyUp);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.tvCustomProperties);
			this.groupBox1.Controls.Add(this.toolStrip1);
			this.groupBox1.Location = new System.Drawing.Point(15, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(220, 351);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Properties";
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAddGroup,
            this.tsbAddDefinition,
            this.tsbDefaultToggle,
            this.toolStripSeparator2,
            this.tsbAddGroupToMachine,
            this.tsbAddGroupToModel,
            this.toolStripSeparator1,
            this.tsbDelete});
			this.toolStrip1.Location = new System.Drawing.Point(3, 16);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(214, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// tsbAddGroup
			// 
			this.tsbAddGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbAddGroup.Image = ((System.Drawing.Image)(resources.GetObject("tsbAddGroup.Image")));
			this.tsbAddGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbAddGroup.Name = "tsbAddGroup";
			this.tsbAddGroup.Size = new System.Drawing.Size(23, 22);
			this.tsbAddGroup.Text = "Add Group";
			this.tsbAddGroup.Click += new System.EventHandler(this.tsbAddGroup_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// tsbAddGroupToMachine
			// 
			this.tsbAddGroupToMachine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbAddGroupToMachine.Image = ((System.Drawing.Image)(resources.GetObject("tsbAddGroupToMachine.Image")));
			this.tsbAddGroupToMachine.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbAddGroupToMachine.Name = "tsbAddGroupToMachine";
			this.tsbAddGroupToMachine.Size = new System.Drawing.Size(23, 22);
			this.tsbAddGroupToMachine.Text = "Add Group to Machine";
			this.tsbAddGroupToMachine.Click += new System.EventHandler(this.tsbAddGroupToMachine_Click);
			// 
			// tsbAddGroupToModel
			// 
			this.tsbAddGroupToModel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbAddGroupToModel.Image = ((System.Drawing.Image)(resources.GetObject("tsbAddGroupToModel.Image")));
			this.tsbAddGroupToModel.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbAddGroupToModel.Name = "tsbAddGroupToModel";
			this.tsbAddGroupToModel.Size = new System.Drawing.Size(23, 22);
			this.tsbAddGroupToModel.Text = "Add Group to Model";
			this.tsbAddGroupToModel.Click += new System.EventHandler(this.tsbAddGroupToModel_Click);
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(353, 340);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 3;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(434, 340);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// definitionEditor1
			// 
			this.definitionEditor1.Location = new System.Drawing.Point(241, 12);
			this.definitionEditor1.Name = "definitionEditor1";
			this.definitionEditor1.Size = new System.Drawing.Size(268, 300);
			this.definitionEditor1.TabIndex = 1;
			this.definitionEditor1.Visible = false;
			this.definitionEditor1.NameChanged += new ORMSolutions.ORMArchitect.CustomProperties.NameChangedHandler(this.editor_NameChanged);
			// 
			// groupEditor1
			// 
			this.groupEditor1.GroupNode = null;
			this.groupEditor1.Location = new System.Drawing.Point(241, 12);
			this.groupEditor1.Name = "groupEditor1";
			this.groupEditor1.Size = new System.Drawing.Size(268, 300);
			this.groupEditor1.TabIndex = 2;
			this.groupEditor1.Visible = false;
			this.groupEditor1.NameChanged += new ORMSolutions.ORMArchitect.CustomProperties.NameChangedHandler(this.editor_NameChanged);
			// 
			// CustomPropertiesManager
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(521, 375);
			this.Controls.Add(this.definitionEditor1);
			this.Controls.Add(this.groupEditor1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnCancel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CustomPropertiesManager";
			this.Text = "Custom Properties";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolStripButton tsbAddDefinition;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton tsbDelete;
		private System.Windows.Forms.ToolStripButton tsbDefaultToggle;
		private System.Windows.Forms.TreeView tvCustomProperties;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton tsbAddGroup;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private DefinitionEditor definitionEditor1;
		private GroupEditor groupEditor1;
		private System.Windows.Forms.ToolStripButton tsbAddGroupToMachine;
		private System.Windows.Forms.ToolStripButton tsbAddGroupToModel;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
	}
}