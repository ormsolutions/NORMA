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
			this.groupPanel = new System.Windows.Forms.Panel();
			this.btnEditGroupDescription = new System.Windows.Forms.Button();
			this.tbxGroupDescription = new System.Windows.Forms.TextBox();
			this.lblGroupName = new System.Windows.Forms.Label();
			this.lblGroupDescription = new System.Windows.Forms.Label();
			this.tbxGroupName = new System.Windows.Forms.TextBox();
			this.definitionPanel = new System.Windows.Forms.Panel();
			this.lblDefinitionName = new System.Windows.Forms.Label();
			this.tbxDefinitionName = new System.Windows.Forms.TextBox();
			this.lblDefinitionDescription = new System.Windows.Forms.Label();
			this.tbxDefinitionDescription = new System.Windows.Forms.TextBox();
			this.lblDefinitionDataType = new System.Windows.Forms.Label();
			this.lblDefinitionCategory = new System.Windows.Forms.Label();
			this.tbxDefinitionCategory = new System.Windows.Forms.TextBox();
			this.cmbxDefinitionDataType = new System.Windows.Forms.ComboBox();
			this.lblDefinitionDefaultValue = new System.Windows.Forms.Label();
			this.tbxDefinitionDefaultValue = new System.Windows.Forms.TextBox();
			this.lblCustomEnum = new System.Windows.Forms.Label();
			this.tbxDefinitionCustomEnum = new System.Windows.Forms.TextBox();
			this.btnEditCustomEnum = new System.Windows.Forms.Button();
			this.groupBoxModelElements = new System.Windows.Forms.GroupBox();
			this.tvModelElements = new System.Windows.Forms.TreeView();
			this.btnEditDefinitionDescription = new System.Windows.Forms.Button();
			this.chkVerbalizeDefaultValue = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.groupPanel.SuspendLayout();
			this.definitionPanel.SuspendLayout();
			this.groupBoxModelElements.SuspendLayout();
			this.SuspendLayout();
			// 
			// tsbAddDefinition
			// 
			this.tsbAddDefinition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.tsbAddDefinition, "tsbAddDefinition");
			this.tsbAddDefinition.Name = "tsbAddDefinition";
			this.tsbAddDefinition.Click += new System.EventHandler(this.tsbAddDefinition_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			// 
			// tsbDelete
			// 
			this.tsbDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.tsbDelete, "tsbDelete");
			this.tsbDelete.Name = "tsbDelete";
			this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
			// 
			// tsbDefaultToggle
			// 
			this.tsbDefaultToggle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.tsbDefaultToggle, "tsbDefaultToggle");
			this.tsbDefaultToggle.Name = "tsbDefaultToggle";
			this.tsbDefaultToggle.Click += new System.EventHandler(this.tsbDefaultToggle_Click);
			// 
			// tvCustomProperties
			// 
			resources.ApplyResources(this.tvCustomProperties, "tvCustomProperties");
			this.tvCustomProperties.HideSelection = false;
			this.tvCustomProperties.Name = "tvCustomProperties";
			this.tvCustomProperties.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            ((System.Windows.Forms.TreeNode)(resources.GetObject("tvCustomProperties.Nodes"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("tvCustomProperties.Nodes1")))});
			this.tvCustomProperties.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvCustomProperties_AfterSelect);
			this.tvCustomProperties.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tvCustomProperties_KeyUp);
			// 
			// groupBox1
			// 
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.tvCustomProperties);
			this.groupBox1.Controls.Add(this.toolStrip1);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
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
			resources.ApplyResources(this.toolStrip1, "toolStrip1");
			this.toolStrip1.Name = "toolStrip1";
			// 
			// tsbAddGroup
			// 
			this.tsbAddGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.tsbAddGroup, "tsbAddGroup");
			this.tsbAddGroup.Name = "tsbAddGroup";
			this.tsbAddGroup.Click += new System.EventHandler(this.tsbAddGroup_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			// 
			// tsbAddGroupToMachine
			// 
			this.tsbAddGroupToMachine.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.tsbAddGroupToMachine, "tsbAddGroupToMachine");
			this.tsbAddGroupToMachine.Name = "tsbAddGroupToMachine";
			this.tsbAddGroupToMachine.Click += new System.EventHandler(this.tsbAddGroupToMachine_Click);
			// 
			// tsbAddGroupToModel
			// 
			this.tsbAddGroupToModel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			resources.ApplyResources(this.tsbAddGroupToModel, "tsbAddGroupToModel");
			this.tsbAddGroupToModel.Name = "tsbAddGroupToModel";
			this.tsbAddGroupToModel.Click += new System.EventHandler(this.tsbAddGroupToModel_Click);
			// 
			// btnSave
			// 
			resources.ApplyResources(this.btnSave, "btnSave");
			this.btnSave.Name = "btnSave";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// groupPanel
			// 
			resources.ApplyResources(this.groupPanel, "groupPanel");
			this.groupPanel.Controls.Add(this.btnEditGroupDescription);
			this.groupPanel.Controls.Add(this.tbxGroupDescription);
			this.groupPanel.Controls.Add(this.lblGroupName);
			this.groupPanel.Controls.Add(this.lblGroupDescription);
			this.groupPanel.Controls.Add(this.tbxGroupName);
			this.groupPanel.Name = "groupPanel";
			// 
			// btnEditGroupDescription
			// 
			resources.ApplyResources(this.btnEditGroupDescription, "btnEditGroupDescription");
			this.btnEditGroupDescription.Name = "btnEditGroupDescription";
			this.btnEditGroupDescription.UseVisualStyleBackColor = true;
			this.btnEditGroupDescription.Click += new System.EventHandler(this.btnEditGroupDescription_Click);
			// 
			// tbxGroupDescription
			// 
			resources.ApplyResources(this.tbxGroupDescription, "tbxGroupDescription");
			this.tbxGroupDescription.Name = "tbxGroupDescription";
			this.tbxGroupDescription.Tag = "description";
			this.tbxGroupDescription.TextChanged += new System.EventHandler(this.tbxGroupTextChanged);
			// 
			// lblGroupName
			// 
			resources.ApplyResources(this.lblGroupName, "lblGroupName");
			this.lblGroupName.Name = "lblGroupName";
			// 
			// lblGroupDescription
			// 
			resources.ApplyResources(this.lblGroupDescription, "lblGroupDescription");
			this.lblGroupDescription.Name = "lblGroupDescription";
			// 
			// tbxGroupName
			// 
			resources.ApplyResources(this.tbxGroupName, "tbxGroupName");
			this.tbxGroupName.Name = "tbxGroupName";
			this.tbxGroupName.Tag = "name";
			this.tbxGroupName.TextChanged += new System.EventHandler(this.tbxGroupTextChanged);
			// 
			// definitionPanel
			// 
			resources.ApplyResources(this.definitionPanel, "definitionPanel");
			this.definitionPanel.Controls.Add(this.chkVerbalizeDefaultValue);
			this.definitionPanel.Controls.Add(this.cmbxDefinitionDataType);
			this.definitionPanel.Controls.Add(this.btnEditDefinitionDescription);
			this.definitionPanel.Controls.Add(this.lblDefinitionName);
			this.definitionPanel.Controls.Add(this.groupBoxModelElements);
			this.definitionPanel.Controls.Add(this.tbxDefinitionName);
			this.definitionPanel.Controls.Add(this.btnEditCustomEnum);
			this.definitionPanel.Controls.Add(this.lblDefinitionDescription);
			this.definitionPanel.Controls.Add(this.tbxDefinitionCustomEnum);
			this.definitionPanel.Controls.Add(this.tbxDefinitionDescription);
			this.definitionPanel.Controls.Add(this.lblCustomEnum);
			this.definitionPanel.Controls.Add(this.lblDefinitionDataType);
			this.definitionPanel.Controls.Add(this.tbxDefinitionDefaultValue);
			this.definitionPanel.Controls.Add(this.lblDefinitionCategory);
			this.definitionPanel.Controls.Add(this.lblDefinitionDefaultValue);
			this.definitionPanel.Controls.Add(this.tbxDefinitionCategory);
			this.definitionPanel.Name = "definitionPanel";
			// 
			// lblDefinitionName
			// 
			resources.ApplyResources(this.lblDefinitionName, "lblDefinitionName");
			this.lblDefinitionName.Name = "lblDefinitionName";
			// 
			// tbxDefinitionName
			// 
			resources.ApplyResources(this.tbxDefinitionName, "tbxDefinitionName");
			this.tbxDefinitionName.Name = "tbxDefinitionName";
			this.tbxDefinitionName.Tag = "name";
			this.tbxDefinitionName.TextChanged += new System.EventHandler(this.tbx_DefinitionTextChanged);
			// 
			// lblDefinitionDescription
			// 
			resources.ApplyResources(this.lblDefinitionDescription, "lblDefinitionDescription");
			this.lblDefinitionDescription.Name = "lblDefinitionDescription";
			// 
			// tbxDefinitionDescription
			// 
			resources.ApplyResources(this.tbxDefinitionDescription, "tbxDefinitionDescription");
			this.tbxDefinitionDescription.Name = "tbxDefinitionDescription";
			this.tbxDefinitionDescription.Tag = "description";
			this.tbxDefinitionDescription.TextChanged += new System.EventHandler(this.tbx_DefinitionTextChanged);
			// 
			// lblDefinitionDataType
			// 
			resources.ApplyResources(this.lblDefinitionDataType, "lblDefinitionDataType");
			this.lblDefinitionDataType.Name = "lblDefinitionDataType";
			// 
			// lblDefinitionCategory
			// 
			resources.ApplyResources(this.lblDefinitionCategory, "lblDefinitionCategory");
			this.lblDefinitionCategory.Name = "lblDefinitionCategory";
			// 
			// tbxDefinitionCategory
			// 
			resources.ApplyResources(this.tbxDefinitionCategory, "tbxDefinitionCategory");
			this.tbxDefinitionCategory.Name = "tbxDefinitionCategory";
			this.tbxDefinitionCategory.Tag = "category";
			this.tbxDefinitionCategory.TextChanged += new System.EventHandler(this.tbx_DefinitionTextChanged);
			// 
			// cmbxDefinitionDataType
			// 
			resources.ApplyResources(this.cmbxDefinitionDataType, "cmbxDefinitionDataType");
			this.cmbxDefinitionDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbxDefinitionDataType.FormattingEnabled = true;
			this.cmbxDefinitionDataType.Items.AddRange(new object[] {
            resources.GetString("cmbxDefinitionDataType.Items"),
            resources.GetString("cmbxDefinitionDataType.Items1"),
            resources.GetString("cmbxDefinitionDataType.Items2"),
            resources.GetString("cmbxDefinitionDataType.Items3")});
			this.cmbxDefinitionDataType.Name = "cmbxDefinitionDataType";
			this.cmbxDefinitionDataType.SelectedIndexChanged += new System.EventHandler(this.cmbxDataType_SelectedIndexChanged);
			// 
			// lblDefinitionDefaultValue
			// 
			resources.ApplyResources(this.lblDefinitionDefaultValue, "lblDefinitionDefaultValue");
			this.lblDefinitionDefaultValue.Name = "lblDefinitionDefaultValue";
			// 
			// tbxDefinitionDefaultValue
			// 
			resources.ApplyResources(this.tbxDefinitionDefaultValue, "tbxDefinitionDefaultValue");
			this.tbxDefinitionDefaultValue.Name = "tbxDefinitionDefaultValue";
			this.tbxDefinitionDefaultValue.Tag = "defaultValue";
			this.tbxDefinitionDefaultValue.TextChanged += new System.EventHandler(this.tbx_DefinitionTextChanged);
			// 
			// lblCustomEnum
			// 
			resources.ApplyResources(this.lblCustomEnum, "lblCustomEnum");
			this.lblCustomEnum.Name = "lblCustomEnum";
			// 
			// tbxDefinitionCustomEnum
			// 
			resources.ApplyResources(this.tbxDefinitionCustomEnum, "tbxDefinitionCustomEnum");
			this.tbxDefinitionCustomEnum.Name = "tbxDefinitionCustomEnum";
			// 
			// btnEditCustomEnum
			// 
			resources.ApplyResources(this.btnEditCustomEnum, "btnEditCustomEnum");
			this.btnEditCustomEnum.Name = "btnEditCustomEnum";
			this.btnEditCustomEnum.UseVisualStyleBackColor = true;
			this.btnEditCustomEnum.Click += new System.EventHandler(this.btnEditCustomEnum_Click);
			// 
			// groupBoxModelElements
			// 
			resources.ApplyResources(this.groupBoxModelElements, "groupBoxModelElements");
			this.groupBoxModelElements.Controls.Add(this.tvModelElements);
			this.groupBoxModelElements.Name = "groupBoxModelElements";
			this.groupBoxModelElements.TabStop = false;
			// 
			// tvModelElements
			// 
			resources.ApplyResources(this.tvModelElements, "tvModelElements");
			this.tvModelElements.CheckBoxes = true;
			this.tvModelElements.Name = "tvModelElements";
			this.tvModelElements.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            ((System.Windows.Forms.TreeNode)(resources.GetObject("tvModelElements.Nodes"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("tvModelElements.Nodes1"))),
            ((System.Windows.Forms.TreeNode)(resources.GetObject("tvModelElements.Nodes2")))});
			this.tvModelElements.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvModelElements_AfterCheck);
			// 
			// btnEditDefinitionDescription
			// 
			resources.ApplyResources(this.btnEditDefinitionDescription, "btnEditDefinitionDescription");
			this.btnEditDefinitionDescription.Name = "btnEditDefinitionDescription";
			this.btnEditDefinitionDescription.UseVisualStyleBackColor = true;
			this.btnEditDefinitionDescription.Click += new System.EventHandler(this.btnEditDefinitionDescription_Click);
			// 
			// chkVerbalizeDefaultValue
			// 
			resources.ApplyResources(this.chkVerbalizeDefaultValue, "chkVerbalizeDefaultValue");
			this.chkVerbalizeDefaultValue.Name = "chkVerbalizeDefaultValue";
			this.chkVerbalizeDefaultValue.Tag = "verbalizeDefaultValue";
			this.chkVerbalizeDefaultValue.UseVisualStyleBackColor = true;
			this.chkVerbalizeDefaultValue.CheckedChanged += new System.EventHandler(this.chkVerbalizeDefaultValue_CheckedChanged);
			// 
			// CustomPropertiesManager
			// 
			this.AcceptButton = this.btnSave;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.definitionPanel);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.groupPanel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CustomPropertiesManager";
			this.ShowIcon = false;
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.groupPanel.ResumeLayout(false);
			this.groupPanel.PerformLayout();
			this.definitionPanel.ResumeLayout(false);
			this.definitionPanel.PerformLayout();
			this.groupBoxModelElements.ResumeLayout(false);
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
		private System.Windows.Forms.ToolStripButton tsbAddGroupToMachine;
		private System.Windows.Forms.ToolStripButton tsbAddGroupToModel;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.Panel groupPanel;
		private System.Windows.Forms.Button btnEditGroupDescription;
		private System.Windows.Forms.TextBox tbxGroupDescription;
		private System.Windows.Forms.Label lblGroupName;
		private System.Windows.Forms.Label lblGroupDescription;
		private System.Windows.Forms.TextBox tbxGroupName;
		private System.Windows.Forms.Panel definitionPanel;
		private System.Windows.Forms.CheckBox chkVerbalizeDefaultValue;
		private System.Windows.Forms.ComboBox cmbxDefinitionDataType;
		private System.Windows.Forms.Button btnEditDefinitionDescription;
		private System.Windows.Forms.Label lblDefinitionName;
		private System.Windows.Forms.GroupBox groupBoxModelElements;
		private System.Windows.Forms.TreeView tvModelElements;
		private System.Windows.Forms.TextBox tbxDefinitionName;
		private System.Windows.Forms.Button btnEditCustomEnum;
		private System.Windows.Forms.Label lblDefinitionDescription;
		private System.Windows.Forms.TextBox tbxDefinitionCustomEnum;
		private System.Windows.Forms.TextBox tbxDefinitionDescription;
		private System.Windows.Forms.Label lblCustomEnum;
		private System.Windows.Forms.Label lblDefinitionDataType;
		private System.Windows.Forms.TextBox tbxDefinitionDefaultValue;
		private System.Windows.Forms.Label lblDefinitionCategory;
		private System.Windows.Forms.Label lblDefinitionDefaultValue;
		private System.Windows.Forms.TextBox tbxDefinitionCategory;
	}
}