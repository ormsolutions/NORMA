namespace SampleModel
{
	#region DeathCore_InputControl
	public partial class DeathCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.TabControl actionTabs;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// actionTabs
			// 
			this.actionTabs = new System.Windows.Forms.TabControl();
			this.actionTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.Size = new System.Drawing.Size(530, 485);
			this.actionTabs.Location = new System.Drawing.Point(0, 0);
			this.actionTabs.Name = "actionTabs";
			this.actionTabs.TabPages.Add("Collection", "Collection");
			Collection_DeathCore_InputControl icCollection = new Collection_DeathCore_InputControl();
			icCollection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Collection"].Controls.Add(icCollection);
			this.actionTabs.TabPages.Add("Create", "Create");
			Create_DeathCore_InputControl icCreate = new Create_DeathCore_InputControl();
			icCreate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Create"].Controls.Add(icCreate);
			this.actionTabs.TabPages.Add("Select", "Select and Edit");
			Select_DeathCore_InputControl icSelect = new Select_DeathCore_InputControl();
			icSelect.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Select"].Controls.Add(icSelect);
			// 
			// this
			// 
			this.Controls.Add(actionTabs);
			this.Size = new System.Drawing.Size(540, 500);
		}
		#endregion // InitializeComponent method
	}
	#endregion // DeathCore_InputControl
	#region Create_DeathCore_InputControl
	public partial class Create_DeathCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCreate;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.DataGridView dgvCreate;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Create_Death_connect;
		private ISampleModelContext testVar;
		private Death abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// connect
			// 
			this.Create_Death_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Create_Death_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblCreate = new System.Windows.Forms.Label();
			this.lblCreate.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblCreate.Name = "lblCreate";
			this.lblCreate.Text = "Enter data to Create Death by:";
			// 
			// btn
			// 
			this.btnCreate = new System.Windows.Forms.Button();
			this.btnCreate.Location = new System.Drawing.Point(400, 10);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.TabIndex = 3;
			this.btnCreate.Text = "Create";
			this.btnCreate.UseVisualStyleBackColor = true;
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// dgv
			// 
			this.dgvCreate = new System.Windows.Forms.DataGridView();
			this.dgvCreate.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCreate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCreate.Name = "dgvCreate";
			this.dgvCreate.TabIndex = 0;
			this.dgvCreate.Columns.Add("Date_YMD", "Date_YMD");
			this.dgvCreate.Columns.Add("DeathCause_DeathCause_Type", "DeathCause_DeathCause_Type");
			this.dgvCreate.Columns.Add("isinvolvedin{1}", "isinvolvedin{1}");
			this.dgvCreate.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvCreate.Height = 75;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnCreate);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvCreate);
			this.Controls.Add(this.lblCreate);
			this.Name = "icCreateDeathInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCreate).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Create_DeathCore_InputControl
	#region Collection_DeathCore_InputControl
	public partial class Collection_DeathCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCollection;
		private System.Windows.Forms.Button btnCollection;
		private System.Windows.Forms.DataGridView dgvCollection;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Collection_Death_connect;
		private ISampleModelContext testVar;
		private Death abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// connect
			// 
			this.Collection_Death_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Collection_Death_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblCollection = new System.Windows.Forms.Label();
			this.lblCollection.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblCollection.Name = "lblCollection";
			this.lblCollection.Text = "Click the Collection button to refresh the list of Death records.";
			// 
			// btn
			// 
			this.btnCollection = new System.Windows.Forms.Button();
			this.btnCollection.Location = new System.Drawing.Point(400, 10);
			this.btnCollection.Name = "btnCollection";
			this.btnCollection.TabIndex = 3;
			this.btnCollection.Text = "Collection";
			this.btnCollection.UseVisualStyleBackColor = true;
			this.btnCollection.Click += new System.EventHandler(this.btnCollection_Click);
			// 
			// dgv
			// 
			this.dgvCollection = new System.Windows.Forms.DataGridView();
			this.dgvCollection.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCollection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCollection.Name = "dgvCollection";
			this.dgvCollection.TabIndex = 0;
			this.dgvCollection.Columns.Add("Date_YMD", "Date_YMD");
			this.dgvCollection.Columns.Add("DeathCause_DeathCause_Type", "DeathCause_DeathCause_Type");
			this.dgvCollection.Columns.Add("isinvolvedin{1}", "isinvolvedin{1}");
			this.dgvCollection.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvCollection.Height = 300;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnCollection);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvCollection);
			this.Controls.Add(this.lblCollection);
			this.Name = "icCollectionDeathInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCollection).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Collection_DeathCore_InputControl
	#region SelectDeathCore_InputControl
	public partial class Select_DeathCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblSelectionMode;
		private System.Windows.Forms.ComboBox cbxSelectionMode;
		private System.Windows.Forms.Label lblCurrentObject;
		private System.Windows.Forms.DataGridView dgvCurrentObject;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel pnlSave;
		private bool editMode;
		private System.Windows.Forms.Label lblNeedToSave;
		private System.Windows.Forms.Label lblSelect;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.DataGridView dgvSelect;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Select_Death_connect;
		private ISampleModelContext testVar;
		private Death abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.editMode = false;
			// 
			// lblNeedToSave
			// 
			this.lblNeedToSave = new System.Windows.Forms.Label();
			this.lblNeedToSave.Location = new System.Drawing.Point(0, 10);
			this.lblNeedToSave.Name = "lblNeedToSave";
			this.lblNeedToSave.Size = new System.Drawing.Size(200, 15);
			this.lblNeedToSave.Text = "";
			// 
			// btnCancel
			// 
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnCancel.Location = new System.Drawing.Point(300, 10);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnSave
			// 
			this.btnSave = new System.Windows.Forms.Button();
			this.btnSave.Location = new System.Drawing.Point(400, 10);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 7;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// pnlSave
			// 
			this.pnlSave = new System.Windows.Forms.Panel();
			this.pnlSave.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlSave.AutoSize = true;
			this.pnlSave.Location = new System.Drawing.Point(0, 0);
			this.pnlSave.Name = "pnlSave";
			this.pnlSave.AutoScroll = true;
			this.pnlSave.TabIndex = 5;
			this.pnlSave.Controls.Add(this.btnSave);
			this.pnlSave.Controls.Add(this.btnCancel);
			this.pnlSave.Controls.Add(this.lblNeedToSave);
			this.pnlSave.Visible = false;
			// 
			// lblSelectionMode
			// 
			this.lblSelectionMode = new System.Windows.Forms.Label();
			this.lblSelectionMode.Location = new System.Drawing.Point(0, 10);
			this.lblSelectionMode.Name = "lblSelectionMode";
			this.lblSelectionMode.Text = "SelectionMode:";
			// 
			// cbxSelectionMode
			// 
			this.cbxSelectionMode = new System.Windows.Forms.ComboBox();
			this.cbxSelectionMode.Location = new System.Drawing.Point(100, 10);
			this.cbxSelectionMode.Name = "cbxSelectionMode";
			this.cbxSelectionMode.Size = new System.Drawing.Size(200, 15);
			this.cbxSelectionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxSelectionMode.TabIndex = 2;
			this.cbxSelectionMode.SelectedIndexChanged += new System.EventHandler(this.cbxSelectionMode_SelectedIndexChanged);
			// 
			// lblCurrentObject
			// 
			this.lblCurrentObject = new System.Windows.Forms.Label();
			this.lblCurrentObject.Location = new System.Drawing.Point(0, 45);
			this.lblCurrentObject.Width = 300;
			this.lblCurrentObject.Name = "lblCurrentObject";
			this.lblCurrentObject.Text = "There is no selected Death.";
			// 
			// dgvCurrentObject
			// 
			this.dgvCurrentObject = new System.Windows.Forms.DataGridView();
			this.dgvCurrentObject.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCurrentObject.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCurrentObject.Location = new System.Drawing.Point(0, 265);
			this.dgvCurrentObject.Name = "dgvCurrentObject";
			this.dgvCurrentObject.Size = new System.Drawing.Size(500, 150);
			this.dgvCurrentObject.TabIndex = 4;
			this.dgvCurrentObject.Columns.Add("Date_YMD", "Date_YMD");
			this.dgvCurrentObject.Columns["Date_YMD"].Visible = false;
			this.dgvCurrentObject.Columns.Add("DeathCause_DeathCause_Type", "DeathCause_DeathCause_Type");
			this.dgvCurrentObject.Columns["DeathCause_DeathCause_Type"].Visible = false;
			this.dgvCurrentObject.Columns.Add("isinvolvedin{1}", "isinvolvedin{1}");
			this.dgvCurrentObject.Columns["isinvolvedin{1}"].Visible = false;
			this.dgvCurrentObject.Visible = false;
			this.dgvCurrentObject.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvCurrentObject_CellBeginEdit);
			this.Controls.Add(this.pnlSave);
			this.Controls.Add(this.dgvCurrentObject);
			// 
			// connect
			// 
			this.Select_Death_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Select_Death_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblSelect = new System.Windows.Forms.Label();
			this.lblSelect.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSelect.Name = "lblSelect";
			this.lblSelect.Text = "Enter data to Select Death by:";
			// 
			// btn
			// 
			this.btnSelect = new System.Windows.Forms.Button();
			this.btnSelect.Location = new System.Drawing.Point(400, 10);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.TabIndex = 3;
			this.btnSelect.Text = "Select";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// dgv
			// 
			this.dgvSelect = new System.Windows.Forms.DataGridView();
			this.dgvSelect.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvSelect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvSelect.Name = "dgvSelect";
			this.dgvSelect.TabIndex = 0;
			this.dgvSelect.Columns.Add("Date_YMD", "Date_YMD");
			this.dgvSelect.Columns.Add("DeathCause_DeathCause_Type", "DeathCause_DeathCause_Type");
			this.dgvSelect.Columns.Add("isinvolvedin{1}", "isinvolvedin{1}");
			this.dgvSelect.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvSelect.Height = 75;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnSelect);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvSelect);
			this.Controls.Add(this.lblSelect);
			this.Name = "icSelectDeathInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvSelect).EndInit();
			this.ResumeLayout(false);
			this.dgvCurrentObject.Height = this.dgvSelect.Height;
			this.pnlDisplay.Controls.Add(this.lblCurrentObject);
			this.pnlDisplay.Controls.Add(this.cbxSelectionMode);
			this.pnlDisplay.Controls.Add(this.lblSelectionMode);

			if (this.cbxSelectionMode.Items.Count > 0)
			{
				this.cbxSelectionMode.SelectedIndex = 0;
			}
		}
		#endregion // InitializeComponent method
	}
	#endregion // Select_DeathCore_InputControl
	#region PersonBoughtCarFromPersonOnDateCore_InputControl
	public partial class PersonBoughtCarFromPersonOnDateCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.TabControl actionTabs;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// actionTabs
			// 
			this.actionTabs = new System.Windows.Forms.TabControl();
			this.actionTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.Size = new System.Drawing.Size(530, 485);
			this.actionTabs.Location = new System.Drawing.Point(0, 0);
			this.actionTabs.Name = "actionTabs";
			this.actionTabs.TabPages.Add("Collection", "Collection");
			Collection_PersonBoughtCarFromPersonOnDateCore_InputControl icCollection = new Collection_PersonBoughtCarFromPersonOnDateCore_InputControl();
			icCollection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Collection"].Controls.Add(icCollection);
			this.actionTabs.TabPages.Add("Create", "Create");
			Create_PersonBoughtCarFromPersonOnDateCore_InputControl icCreate = new Create_PersonBoughtCarFromPersonOnDateCore_InputControl();
			icCreate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Create"].Controls.Add(icCreate);
			this.actionTabs.TabPages.Add("Select", "Select and Edit");
			Select_PersonBoughtCarFromPersonOnDateCore_InputControl icSelect = new Select_PersonBoughtCarFromPersonOnDateCore_InputControl();
			icSelect.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Select"].Controls.Add(icSelect);
			// 
			// this
			// 
			this.Controls.Add(actionTabs);
			this.Size = new System.Drawing.Size(540, 500);
		}
		#endregion // InitializeComponent method
	}
	#endregion // PersonBoughtCarFromPersonOnDateCore_InputControl
	#region Create_PersonBoughtCarFromPersonOnDateCore_InputControl
	public partial class Create_PersonBoughtCarFromPersonOnDateCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCreate;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.DataGridView dgvCreate;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Create_PersonBoughtCarFromPersonOnDate_connect;
		private ISampleModelContext testVar;
		private PersonBoughtCarFromPersonOnDate abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// connect
			// 
			this.Create_PersonBoughtCarFromPersonOnDate_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Create_PersonBoughtCarFromPersonOnDate_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblCreate = new System.Windows.Forms.Label();
			this.lblCreate.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblCreate.Name = "lblCreate";
			this.lblCreate.Text = "Enter data to Create PersonBoughtCarFromPersonOnDate by:";
			// 
			// btn
			// 
			this.btnCreate = new System.Windows.Forms.Button();
			this.btnCreate.Location = new System.Drawing.Point(400, 10);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.TabIndex = 3;
			this.btnCreate.Text = "Create";
			this.btnCreate.UseVisualStyleBackColor = true;
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// dgv
			// 
			this.dgvCreate = new System.Windows.Forms.DataGridView();
			this.dgvCreate.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCreate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCreate.Name = "dgvCreate";
			this.dgvCreate.TabIndex = 0;
			this.dgvCreate.Columns.Add("CarSold_vin", "CarSold_vin");
			this.dgvCreate.Columns.Add("SaleDate_YMD", "SaleDate_YMD");
			this.dgvCreate.Columns.Add("Buyer(Person_id)", "Buyer(Person_id)");
			this.dgvCreate.Columns.Add("Seller(Person_id)", "Seller(Person_id)");
			this.dgvCreate.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvCreate.Height = 75;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnCreate);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvCreate);
			this.Controls.Add(this.lblCreate);
			this.Name = "icCreatePersonBoughtCarFromPersonOnDateInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCreate).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Create_PersonBoughtCarFromPersonOnDateCore_InputControl
	#region Collection_PersonBoughtCarFromPersonOnDateCore_InputControl
	public partial class Collection_PersonBoughtCarFromPersonOnDateCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCollection;
		private System.Windows.Forms.Button btnCollection;
		private System.Windows.Forms.DataGridView dgvCollection;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Collection_PersonBoughtCarFromPersonOnDate_connect;
		private ISampleModelContext testVar;
		private PersonBoughtCarFromPersonOnDate abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// connect
			// 
			this.Collection_PersonBoughtCarFromPersonOnDate_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Collection_PersonBoughtCarFromPersonOnDate_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblCollection = new System.Windows.Forms.Label();
			this.lblCollection.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblCollection.Name = "lblCollection";
			this.lblCollection.Text = "Click the Collection button to refresh the list of PersonBoughtCarFromPersonOnDate records.";
			// 
			// btn
			// 
			this.btnCollection = new System.Windows.Forms.Button();
			this.btnCollection.Location = new System.Drawing.Point(400, 10);
			this.btnCollection.Name = "btnCollection";
			this.btnCollection.TabIndex = 3;
			this.btnCollection.Text = "Collection";
			this.btnCollection.UseVisualStyleBackColor = true;
			this.btnCollection.Click += new System.EventHandler(this.btnCollection_Click);
			// 
			// dgv
			// 
			this.dgvCollection = new System.Windows.Forms.DataGridView();
			this.dgvCollection.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCollection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCollection.Name = "dgvCollection";
			this.dgvCollection.TabIndex = 0;
			this.dgvCollection.Columns.Add("CarSold_vin", "CarSold_vin");
			this.dgvCollection.Columns.Add("SaleDate_YMD", "SaleDate_YMD");
			this.dgvCollection.Columns.Add("Buyer(Person_id)", "Buyer(Person_id)");
			this.dgvCollection.Columns.Add("Seller(Person_id)", "Seller(Person_id)");
			this.dgvCollection.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvCollection.Height = 300;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnCollection);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvCollection);
			this.Controls.Add(this.lblCollection);
			this.Name = "icCollectionPersonBoughtCarFromPersonOnDateInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCollection).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Collection_PersonBoughtCarFromPersonOnDateCore_InputControl
	#region SelectPersonBoughtCarFromPersonOnDateCore_InputControl
	public partial class Select_PersonBoughtCarFromPersonOnDateCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblSelectionMode;
		private System.Windows.Forms.ComboBox cbxSelectionMode;
		private System.Windows.Forms.Label lblCurrentObject;
		private System.Windows.Forms.DataGridView dgvCurrentObject;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel pnlSave;
		private bool editMode;
		private System.Windows.Forms.Label lblNeedToSave;
		private System.Windows.Forms.Label lblSelect;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.DataGridView dgvSelect;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Select_PersonBoughtCarFromPersonOnDate_connect;
		private ISampleModelContext testVar;
		private PersonBoughtCarFromPersonOnDate abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.editMode = false;
			// 
			// lblNeedToSave
			// 
			this.lblNeedToSave = new System.Windows.Forms.Label();
			this.lblNeedToSave.Location = new System.Drawing.Point(0, 10);
			this.lblNeedToSave.Name = "lblNeedToSave";
			this.lblNeedToSave.Size = new System.Drawing.Size(200, 15);
			this.lblNeedToSave.Text = "";
			// 
			// btnCancel
			// 
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnCancel.Location = new System.Drawing.Point(300, 10);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnSave
			// 
			this.btnSave = new System.Windows.Forms.Button();
			this.btnSave.Location = new System.Drawing.Point(400, 10);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 7;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// pnlSave
			// 
			this.pnlSave = new System.Windows.Forms.Panel();
			this.pnlSave.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlSave.AutoSize = true;
			this.pnlSave.Location = new System.Drawing.Point(0, 0);
			this.pnlSave.Name = "pnlSave";
			this.pnlSave.AutoScroll = true;
			this.pnlSave.TabIndex = 5;
			this.pnlSave.Controls.Add(this.btnSave);
			this.pnlSave.Controls.Add(this.btnCancel);
			this.pnlSave.Controls.Add(this.lblNeedToSave);
			this.pnlSave.Visible = false;
			// 
			// lblSelectionMode
			// 
			this.lblSelectionMode = new System.Windows.Forms.Label();
			this.lblSelectionMode.Location = new System.Drawing.Point(0, 10);
			this.lblSelectionMode.Name = "lblSelectionMode";
			this.lblSelectionMode.Text = "SelectionMode:";
			// 
			// cbxSelectionMode
			// 
			this.cbxSelectionMode = new System.Windows.Forms.ComboBox();
			this.cbxSelectionMode.Location = new System.Drawing.Point(100, 10);
			this.cbxSelectionMode.Name = "cbxSelectionMode";
			this.cbxSelectionMode.Size = new System.Drawing.Size(200, 15);
			this.cbxSelectionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxSelectionMode.TabIndex = 2;
			this.cbxSelectionMode.SelectedIndexChanged += new System.EventHandler(this.cbxSelectionMode_SelectedIndexChanged);
			this.cbxSelectionMode.Items.Add("InternalUniquenessConstraint23");
			this.cbxSelectionMode.Items.Add("InternalUniquenessConstraint24");
			this.cbxSelectionMode.Items.Add("InternalUniquenessConstraint25");
			// 
			// lblCurrentObject
			// 
			this.lblCurrentObject = new System.Windows.Forms.Label();
			this.lblCurrentObject.Location = new System.Drawing.Point(0, 45);
			this.lblCurrentObject.Width = 300;
			this.lblCurrentObject.Name = "lblCurrentObject";
			this.lblCurrentObject.Text = "There is no selected PersonBoughtCarFromPersonOnDate.";
			// 
			// dgvCurrentObject
			// 
			this.dgvCurrentObject = new System.Windows.Forms.DataGridView();
			this.dgvCurrentObject.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCurrentObject.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCurrentObject.Location = new System.Drawing.Point(0, 265);
			this.dgvCurrentObject.Name = "dgvCurrentObject";
			this.dgvCurrentObject.Size = new System.Drawing.Size(500, 150);
			this.dgvCurrentObject.TabIndex = 4;
			this.dgvCurrentObject.Columns.Add("CarSold_vin", "CarSold_vin");
			this.dgvCurrentObject.Columns["CarSold_vin"].Visible = false;
			this.dgvCurrentObject.Columns.Add("SaleDate_YMD", "SaleDate_YMD");
			this.dgvCurrentObject.Columns["SaleDate_YMD"].Visible = false;
			this.dgvCurrentObject.Columns.Add("Buyer(Person_id)", "Buyer(Person_id)");
			this.dgvCurrentObject.Columns["Buyer(Person_id)"].Visible = false;
			this.dgvCurrentObject.Columns.Add("Seller(Person_id)", "Seller(Person_id)");
			this.dgvCurrentObject.Columns["Seller(Person_id)"].Visible = false;
			this.dgvCurrentObject.Visible = false;
			this.dgvCurrentObject.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvCurrentObject_CellBeginEdit);
			this.Controls.Add(this.pnlSave);
			this.Controls.Add(this.dgvCurrentObject);
			// 
			// connect
			// 
			this.Select_PersonBoughtCarFromPersonOnDate_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Select_PersonBoughtCarFromPersonOnDate_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblSelect = new System.Windows.Forms.Label();
			this.lblSelect.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSelect.Name = "lblSelect";
			this.lblSelect.Text = "Enter data to Select PersonBoughtCarFromPersonOnDate by:";
			// 
			// btn
			// 
			this.btnSelect = new System.Windows.Forms.Button();
			this.btnSelect.Location = new System.Drawing.Point(400, 10);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.TabIndex = 3;
			this.btnSelect.Text = "Select";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// dgv
			// 
			this.dgvSelect = new System.Windows.Forms.DataGridView();
			this.dgvSelect.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvSelect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvSelect.Name = "dgvSelect";
			this.dgvSelect.TabIndex = 0;
			this.dgvSelect.Columns.Add("CarSold_vin", "CarSold_vin");
			this.dgvSelect.Columns.Add("SaleDate_YMD", "SaleDate_YMD");
			this.dgvSelect.Columns.Add("Buyer(Person_id)", "Buyer(Person_id)");
			this.dgvSelect.Columns.Add("Seller(Person_id)", "Seller(Person_id)");
			this.dgvSelect.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvSelect.Height = 75;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnSelect);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvSelect);
			this.Controls.Add(this.lblSelect);
			this.Name = "icSelectPersonBoughtCarFromPersonOnDateInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvSelect).EndInit();
			this.ResumeLayout(false);
			this.dgvCurrentObject.Height = this.dgvSelect.Height;
			this.pnlDisplay.Controls.Add(this.lblCurrentObject);
			this.pnlDisplay.Controls.Add(this.cbxSelectionMode);
			this.pnlDisplay.Controls.Add(this.lblSelectionMode);

			if (this.cbxSelectionMode.Items.Count > 0)
			{
				this.cbxSelectionMode.SelectedIndex = 0;
			}
		}
		#endregion // InitializeComponent method
	}
	#endregion // Select_PersonBoughtCarFromPersonOnDateCore_InputControl
	#region PersonCore_InputControl
	public partial class PersonCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.TabControl actionTabs;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// actionTabs
			// 
			this.actionTabs = new System.Windows.Forms.TabControl();
			this.actionTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.Size = new System.Drawing.Size(530, 485);
			this.actionTabs.Location = new System.Drawing.Point(0, 0);
			this.actionTabs.Name = "actionTabs";
			this.actionTabs.TabPages.Add("Collection", "Collection");
			Collection_PersonCore_InputControl icCollection = new Collection_PersonCore_InputControl();
			icCollection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Collection"].Controls.Add(icCollection);
			this.actionTabs.TabPages.Add("Create", "Create");
			Create_PersonCore_InputControl icCreate = new Create_PersonCore_InputControl();
			icCreate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Create"].Controls.Add(icCreate);
			this.actionTabs.TabPages.Add("Select", "Select and Edit");
			Select_PersonCore_InputControl icSelect = new Select_PersonCore_InputControl();
			icSelect.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Select"].Controls.Add(icSelect);
			// 
			// this
			// 
			this.Controls.Add(actionTabs);
			this.Size = new System.Drawing.Size(540, 500);
		}
		#endregion // InitializeComponent method
	}
	#endregion // PersonCore_InputControl
	#region Create_PersonCore_InputControl
	public partial class Create_PersonCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCreate;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.DataGridView dgvCreate;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Create_Person_connect;
		private ISampleModelContext testVar;
		private Person abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// connect
			// 
			this.Create_Person_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Create_Person_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblCreate = new System.Windows.Forms.Label();
			this.lblCreate.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblCreate.Name = "lblCreate";
			this.lblCreate.Text = "Enter data to Create Person by:";
			// 
			// btn
			// 
			this.btnCreate = new System.Windows.Forms.Button();
			this.btnCreate.Location = new System.Drawing.Point(400, 10);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.TabIndex = 3;
			this.btnCreate.Text = "Create";
			this.btnCreate.UseVisualStyleBackColor = true;
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// dgv
			// 
			this.dgvCreate = new System.Windows.Forms.DataGridView();
			this.dgvCreate.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCreate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCreate.Name = "dgvCreate";
			this.dgvCreate.TabIndex = 0;
			this.dgvCreate.Columns.Add("FirstName", "FirstName");
			this.dgvCreate.Columns.Add("Date_YMD", "Date_YMD");
			this.dgvCreate.Columns.Add("LastName", "LastName");
			this.dgvCreate.Columns.Add("OptionalUniqueString", "OptionalUniqueString");
			this.dgvCreate.Columns.Add("HatType_ColorARGB", "HatType_ColorARGB");
			this.dgvCreate.Columns.Add("HatType_HatTypeStyle_HatTypeStyle_Description", "HatType_HatTypeStyle_HatTypeStyle_Description");
			this.dgvCreate.Columns.Add("OwnsCar_vin", "OwnsCar_vin");
			this.dgvCreate.Columns.Add("Gender_Gender_Code", "Gender_Gender_Code");
			this.dgvCreate.Columns.Add("hasParents", "hasParents");
			this.dgvCreate.Columns.Add("OptionalUniqueDecimal", "OptionalUniqueDecimal");
			this.dgvCreate.Columns.Add("MandatoryUniqueDecimal", "MandatoryUniqueDecimal");
			this.dgvCreate.Columns.Add("MandatoryUniqueString", "MandatoryUniqueString");
			this.dgvCreate.Columns.Add("OptionalUniqueTinyInt", "OptionalUniqueTinyInt");
			this.dgvCreate.Columns.Add("MandatoryUniqueTinyInt", "MandatoryUniqueTinyInt");
			this.dgvCreate.Columns.Add("OptionalNonUniqueTinyInt", "OptionalNonUniqueTinyInt");
			this.dgvCreate.Columns.Add("MandatoryNonUniqueTinyInt", "MandatoryNonUniqueTinyInt");
			this.dgvCreate.Columns.Add("MandatoryNonUniqueUnconstrainedDecimal", "MandatoryNonUniqueUnconstrainedDecimal");
			this.dgvCreate.Columns.Add("MandatoryNonUniqueUnconstrainedFloat", "MandatoryNonUniqueUnconstrainedFloat");
			this.dgvCreate.Columns.Add("Husband", "Husband");
			this.dgvCreate.Columns.Add("ValueType1DoesSomethingElseWith", "ValueType1DoesSomethingElseWith");
			this.dgvCreate.Columns.Add("Wife", "Wife");
			this.dgvCreate.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvCreate.Height = 75;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnCreate);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvCreate);
			this.Controls.Add(this.lblCreate);
			this.Name = "icCreatePersonInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCreate).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Create_PersonCore_InputControl
	#region Collection_PersonCore_InputControl
	public partial class Collection_PersonCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCollection;
		private System.Windows.Forms.Button btnCollection;
		private System.Windows.Forms.DataGridView dgvCollection;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Collection_Person_connect;
		private ISampleModelContext testVar;
		private Person abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// connect
			// 
			this.Collection_Person_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Collection_Person_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblCollection = new System.Windows.Forms.Label();
			this.lblCollection.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblCollection.Name = "lblCollection";
			this.lblCollection.Text = "Click the Collection button to refresh the list of Person records.";
			// 
			// btn
			// 
			this.btnCollection = new System.Windows.Forms.Button();
			this.btnCollection.Location = new System.Drawing.Point(400, 10);
			this.btnCollection.Name = "btnCollection";
			this.btnCollection.TabIndex = 3;
			this.btnCollection.Text = "Collection";
			this.btnCollection.UseVisualStyleBackColor = true;
			this.btnCollection.Click += new System.EventHandler(this.btnCollection_Click);
			// 
			// dgv
			// 
			this.dgvCollection = new System.Windows.Forms.DataGridView();
			this.dgvCollection.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCollection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCollection.Name = "dgvCollection";
			this.dgvCollection.TabIndex = 0;
			this.dgvCollection.Columns.Add("FirstName", "FirstName");
			this.dgvCollection.Columns.Add("Date_YMD", "Date_YMD");
			this.dgvCollection.Columns.Add("LastName", "LastName");
			this.dgvCollection.Columns.Add("OptionalUniqueString", "OptionalUniqueString");
			this.dgvCollection.Columns.Add("HatType_ColorARGB", "HatType_ColorARGB");
			this.dgvCollection.Columns.Add("HatType_HatTypeStyle_HatTypeStyle_Description", "HatType_HatTypeStyle_HatTypeStyle_Description");
			this.dgvCollection.Columns.Add("OwnsCar_vin", "OwnsCar_vin");
			this.dgvCollection.Columns.Add("Gender_Gender_Code", "Gender_Gender_Code");
			this.dgvCollection.Columns.Add("hasParents", "hasParents");
			this.dgvCollection.Columns.Add("OptionalUniqueDecimal", "OptionalUniqueDecimal");
			this.dgvCollection.Columns.Add("MandatoryUniqueDecimal", "MandatoryUniqueDecimal");
			this.dgvCollection.Columns.Add("MandatoryUniqueString", "MandatoryUniqueString");
			this.dgvCollection.Columns.Add("OptionalUniqueTinyInt", "OptionalUniqueTinyInt");
			this.dgvCollection.Columns.Add("MandatoryUniqueTinyInt", "MandatoryUniqueTinyInt");
			this.dgvCollection.Columns.Add("OptionalNonUniqueTinyInt", "OptionalNonUniqueTinyInt");
			this.dgvCollection.Columns.Add("MandatoryNonUniqueTinyInt", "MandatoryNonUniqueTinyInt");
			this.dgvCollection.Columns.Add("MandatoryNonUniqueUnconstrainedDecimal", "MandatoryNonUniqueUnconstrainedDecimal");
			this.dgvCollection.Columns.Add("MandatoryNonUniqueUnconstrainedFloat", "MandatoryNonUniqueUnconstrainedFloat");
			this.dgvCollection.Columns.Add("Husband", "Husband");
			this.dgvCollection.Columns.Add("ValueType1DoesSomethingElseWith", "ValueType1DoesSomethingElseWith");
			this.dgvCollection.Columns.Add("Wife", "Wife");
			this.dgvCollection.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvCollection.Height = 300;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnCollection);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvCollection);
			this.Controls.Add(this.lblCollection);
			this.Name = "icCollectionPersonInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCollection).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Collection_PersonCore_InputControl
	#region SelectPersonCore_InputControl
	public partial class Select_PersonCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblSelectionMode;
		private System.Windows.Forms.ComboBox cbxSelectionMode;
		private System.Windows.Forms.Label lblCurrentObject;
		private System.Windows.Forms.DataGridView dgvCurrentObject;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel pnlSave;
		private bool editMode;
		private System.Windows.Forms.Label lblNeedToSave;
		private System.Windows.Forms.Label lblSelect;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.DataGridView dgvSelect;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Select_Person_connect;
		private ISampleModelContext testVar;
		private Person abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.editMode = false;
			// 
			// lblNeedToSave
			// 
			this.lblNeedToSave = new System.Windows.Forms.Label();
			this.lblNeedToSave.Location = new System.Drawing.Point(0, 10);
			this.lblNeedToSave.Name = "lblNeedToSave";
			this.lblNeedToSave.Size = new System.Drawing.Size(200, 15);
			this.lblNeedToSave.Text = "";
			// 
			// btnCancel
			// 
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnCancel.Location = new System.Drawing.Point(300, 10);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnSave
			// 
			this.btnSave = new System.Windows.Forms.Button();
			this.btnSave.Location = new System.Drawing.Point(400, 10);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 7;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// pnlSave
			// 
			this.pnlSave = new System.Windows.Forms.Panel();
			this.pnlSave.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlSave.AutoSize = true;
			this.pnlSave.Location = new System.Drawing.Point(0, 0);
			this.pnlSave.Name = "pnlSave";
			this.pnlSave.AutoScroll = true;
			this.pnlSave.TabIndex = 5;
			this.pnlSave.Controls.Add(this.btnSave);
			this.pnlSave.Controls.Add(this.btnCancel);
			this.pnlSave.Controls.Add(this.lblNeedToSave);
			this.pnlSave.Visible = false;
			// 
			// lblSelectionMode
			// 
			this.lblSelectionMode = new System.Windows.Forms.Label();
			this.lblSelectionMode.Location = new System.Drawing.Point(0, 10);
			this.lblSelectionMode.Name = "lblSelectionMode";
			this.lblSelectionMode.Text = "SelectionMode:";
			// 
			// cbxSelectionMode
			// 
			this.cbxSelectionMode = new System.Windows.Forms.ComboBox();
			this.cbxSelectionMode.Location = new System.Drawing.Point(100, 10);
			this.cbxSelectionMode.Name = "cbxSelectionMode";
			this.cbxSelectionMode.Size = new System.Drawing.Size(200, 15);
			this.cbxSelectionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxSelectionMode.TabIndex = 2;
			this.cbxSelectionMode.SelectedIndexChanged += new System.EventHandler(this.cbxSelectionMode_SelectedIndexChanged);
			this.cbxSelectionMode.Items.Add("ExternalUniquenessConstraint1");
			this.cbxSelectionMode.Items.Add("ExternalUniquenessConstraint2");
			this.cbxSelectionMode.Items.Add("MandatoryUniqueDecimal");
			this.cbxSelectionMode.Items.Add("MandatoryUniqueString");
			this.cbxSelectionMode.Items.Add("MandatoryUniqueTinyInt");
			this.cbxSelectionMode.Items.Add("OptionalUniqueDecimal");
			this.cbxSelectionMode.Items.Add("OptionalUniqueString");
			this.cbxSelectionMode.Items.Add("OptionalUniqueTinyInt");
			this.cbxSelectionMode.Items.Add("OwnsCar_vin");
			// 
			// lblCurrentObject
			// 
			this.lblCurrentObject = new System.Windows.Forms.Label();
			this.lblCurrentObject.Location = new System.Drawing.Point(0, 45);
			this.lblCurrentObject.Width = 300;
			this.lblCurrentObject.Name = "lblCurrentObject";
			this.lblCurrentObject.Text = "There is no selected Person.";
			// 
			// dgvCurrentObject
			// 
			this.dgvCurrentObject = new System.Windows.Forms.DataGridView();
			this.dgvCurrentObject.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCurrentObject.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCurrentObject.Location = new System.Drawing.Point(0, 265);
			this.dgvCurrentObject.Name = "dgvCurrentObject";
			this.dgvCurrentObject.Size = new System.Drawing.Size(500, 150);
			this.dgvCurrentObject.TabIndex = 4;
			this.dgvCurrentObject.Columns.Add("FirstName", "FirstName");
			this.dgvCurrentObject.Columns["FirstName"].Visible = false;
			this.dgvCurrentObject.Columns.Add("Date_YMD", "Date_YMD");
			this.dgvCurrentObject.Columns["Date_YMD"].Visible = false;
			this.dgvCurrentObject.Columns.Add("LastName", "LastName");
			this.dgvCurrentObject.Columns["LastName"].Visible = false;
			this.dgvCurrentObject.Columns.Add("OptionalUniqueString", "OptionalUniqueString");
			this.dgvCurrentObject.Columns["OptionalUniqueString"].Visible = false;
			this.dgvCurrentObject.Columns.Add("HatType_ColorARGB", "HatType_ColorARGB");
			this.dgvCurrentObject.Columns["HatType_ColorARGB"].Visible = false;
			this.dgvCurrentObject.Columns.Add("HatType_HatTypeStyle_HatTypeStyle_Description", "HatType_HatTypeStyle_HatTypeStyle_Description");
			this.dgvCurrentObject.Columns["HatType_HatTypeStyle_HatTypeStyle_Description"].Visible = false;
			this.dgvCurrentObject.Columns.Add("OwnsCar_vin", "OwnsCar_vin");
			this.dgvCurrentObject.Columns["OwnsCar_vin"].Visible = false;
			this.dgvCurrentObject.Columns.Add("Gender_Gender_Code", "Gender_Gender_Code");
			this.dgvCurrentObject.Columns["Gender_Gender_Code"].Visible = false;
			this.dgvCurrentObject.Columns.Add("hasParents", "hasParents");
			this.dgvCurrentObject.Columns["hasParents"].Visible = false;
			this.dgvCurrentObject.Columns.Add("OptionalUniqueDecimal", "OptionalUniqueDecimal");
			this.dgvCurrentObject.Columns["OptionalUniqueDecimal"].Visible = false;
			this.dgvCurrentObject.Columns.Add("MandatoryUniqueDecimal", "MandatoryUniqueDecimal");
			this.dgvCurrentObject.Columns["MandatoryUniqueDecimal"].Visible = false;
			this.dgvCurrentObject.Columns.Add("MandatoryUniqueString", "MandatoryUniqueString");
			this.dgvCurrentObject.Columns["MandatoryUniqueString"].Visible = false;
			this.dgvCurrentObject.Columns.Add("OptionalUniqueTinyInt", "OptionalUniqueTinyInt");
			this.dgvCurrentObject.Columns["OptionalUniqueTinyInt"].Visible = false;
			this.dgvCurrentObject.Columns.Add("MandatoryUniqueTinyInt", "MandatoryUniqueTinyInt");
			this.dgvCurrentObject.Columns["MandatoryUniqueTinyInt"].Visible = false;
			this.dgvCurrentObject.Columns.Add("OptionalNonUniqueTinyInt", "OptionalNonUniqueTinyInt");
			this.dgvCurrentObject.Columns["OptionalNonUniqueTinyInt"].Visible = false;
			this.dgvCurrentObject.Columns.Add("MandatoryNonUniqueTinyInt", "MandatoryNonUniqueTinyInt");
			this.dgvCurrentObject.Columns["MandatoryNonUniqueTinyInt"].Visible = false;
			this.dgvCurrentObject.Columns.Add("MandatoryNonUniqueUnconstrainedDecimal", "MandatoryNonUniqueUnconstrainedDecimal");
			this.dgvCurrentObject.Columns["MandatoryNonUniqueUnconstrainedDecimal"].Visible = false;
			this.dgvCurrentObject.Columns.Add("MandatoryNonUniqueUnconstrainedFloat", "MandatoryNonUniqueUnconstrainedFloat");
			this.dgvCurrentObject.Columns["MandatoryNonUniqueUnconstrainedFloat"].Visible = false;
			this.dgvCurrentObject.Columns.Add("Husband", "Husband");
			this.dgvCurrentObject.Columns["Husband"].Visible = false;
			this.dgvCurrentObject.Columns.Add("ValueType1DoesSomethingElseWith", "ValueType1DoesSomethingElseWith");
			this.dgvCurrentObject.Columns["ValueType1DoesSomethingElseWith"].Visible = false;
			this.dgvCurrentObject.Columns.Add("Wife", "Wife");
			this.dgvCurrentObject.Columns["Wife"].Visible = false;
			this.dgvCurrentObject.Visible = false;
			this.dgvCurrentObject.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvCurrentObject_CellBeginEdit);
			this.Controls.Add(this.pnlSave);
			this.Controls.Add(this.dgvCurrentObject);
			// 
			// connect
			// 
			this.Select_Person_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Select_Person_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblSelect = new System.Windows.Forms.Label();
			this.lblSelect.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSelect.Name = "lblSelect";
			this.lblSelect.Text = "Enter data to Select Person by:";
			// 
			// btn
			// 
			this.btnSelect = new System.Windows.Forms.Button();
			this.btnSelect.Location = new System.Drawing.Point(400, 10);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.TabIndex = 3;
			this.btnSelect.Text = "Select";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// dgv
			// 
			this.dgvSelect = new System.Windows.Forms.DataGridView();
			this.dgvSelect.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvSelect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvSelect.Name = "dgvSelect";
			this.dgvSelect.TabIndex = 0;
			this.dgvSelect.Columns.Add("FirstName", "FirstName");
			this.dgvSelect.Columns.Add("Date_YMD", "Date_YMD");
			this.dgvSelect.Columns.Add("LastName", "LastName");
			this.dgvSelect.Columns.Add("OptionalUniqueString", "OptionalUniqueString");
			this.dgvSelect.Columns.Add("HatType_ColorARGB", "HatType_ColorARGB");
			this.dgvSelect.Columns.Add("HatType_HatTypeStyle_HatTypeStyle_Description", "HatType_HatTypeStyle_HatTypeStyle_Description");
			this.dgvSelect.Columns.Add("OwnsCar_vin", "OwnsCar_vin");
			this.dgvSelect.Columns.Add("Gender_Gender_Code", "Gender_Gender_Code");
			this.dgvSelect.Columns.Add("hasParents", "hasParents");
			this.dgvSelect.Columns.Add("OptionalUniqueDecimal", "OptionalUniqueDecimal");
			this.dgvSelect.Columns.Add("MandatoryUniqueDecimal", "MandatoryUniqueDecimal");
			this.dgvSelect.Columns.Add("MandatoryUniqueString", "MandatoryUniqueString");
			this.dgvSelect.Columns.Add("OptionalUniqueTinyInt", "OptionalUniqueTinyInt");
			this.dgvSelect.Columns.Add("MandatoryUniqueTinyInt", "MandatoryUniqueTinyInt");
			this.dgvSelect.Columns.Add("OptionalNonUniqueTinyInt", "OptionalNonUniqueTinyInt");
			this.dgvSelect.Columns.Add("MandatoryNonUniqueTinyInt", "MandatoryNonUniqueTinyInt");
			this.dgvSelect.Columns.Add("MandatoryNonUniqueUnconstrainedDecimal", "MandatoryNonUniqueUnconstrainedDecimal");
			this.dgvSelect.Columns.Add("MandatoryNonUniqueUnconstrainedFloat", "MandatoryNonUniqueUnconstrainedFloat");
			this.dgvSelect.Columns.Add("Husband", "Husband");
			this.dgvSelect.Columns.Add("ValueType1DoesSomethingElseWith", "ValueType1DoesSomethingElseWith");
			this.dgvSelect.Columns.Add("Wife", "Wife");
			this.dgvSelect.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvSelect.Height = 75;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnSelect);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvSelect);
			this.Controls.Add(this.lblSelect);
			this.Name = "icSelectPersonInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvSelect).EndInit();
			this.ResumeLayout(false);
			this.dgvCurrentObject.Height = this.dgvSelect.Height;
			this.pnlDisplay.Controls.Add(this.lblCurrentObject);
			this.pnlDisplay.Controls.Add(this.cbxSelectionMode);
			this.pnlDisplay.Controls.Add(this.lblSelectionMode);

			if (this.cbxSelectionMode.Items.Count > 0)
			{
				this.cbxSelectionMode.SelectedIndex = 0;
			}
		}
		#endregion // InitializeComponent method
	}
	#endregion // Select_PersonCore_InputControl
	#region PersonDrivesCarCore_InputControl
	public partial class PersonDrivesCarCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.TabControl actionTabs;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// actionTabs
			// 
			this.actionTabs = new System.Windows.Forms.TabControl();
			this.actionTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.Size = new System.Drawing.Size(530, 485);
			this.actionTabs.Location = new System.Drawing.Point(0, 0);
			this.actionTabs.Name = "actionTabs";
			this.actionTabs.TabPages.Add("Collection", "Collection");
			Collection_PersonDrivesCarCore_InputControl icCollection = new Collection_PersonDrivesCarCore_InputControl();
			icCollection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Collection"].Controls.Add(icCollection);
			this.actionTabs.TabPages.Add("Create", "Create");
			Create_PersonDrivesCarCore_InputControl icCreate = new Create_PersonDrivesCarCore_InputControl();
			icCreate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Create"].Controls.Add(icCreate);
			this.actionTabs.TabPages.Add("Select", "Select and Edit");
			Select_PersonDrivesCarCore_InputControl icSelect = new Select_PersonDrivesCarCore_InputControl();
			icSelect.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Select"].Controls.Add(icSelect);
			// 
			// this
			// 
			this.Controls.Add(actionTabs);
			this.Size = new System.Drawing.Size(540, 500);
		}
		#endregion // InitializeComponent method
	}
	#endregion // PersonDrivesCarCore_InputControl
	#region Create_PersonDrivesCarCore_InputControl
	public partial class Create_PersonDrivesCarCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCreate;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.DataGridView dgvCreate;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Create_PersonDrivesCar_connect;
		private ISampleModelContext testVar;
		private PersonDrivesCar abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// connect
			// 
			this.Create_PersonDrivesCar_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Create_PersonDrivesCar_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblCreate = new System.Windows.Forms.Label();
			this.lblCreate.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblCreate.Name = "lblCreate";
			this.lblCreate.Text = "Enter data to Create PersonDrivesCar by:";
			// 
			// btn
			// 
			this.btnCreate = new System.Windows.Forms.Button();
			this.btnCreate.Location = new System.Drawing.Point(400, 10);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.TabIndex = 3;
			this.btnCreate.Text = "Create";
			this.btnCreate.UseVisualStyleBackColor = true;
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// dgv
			// 
			this.dgvCreate = new System.Windows.Forms.DataGridView();
			this.dgvCreate.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCreate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCreate.Name = "dgvCreate";
			this.dgvCreate.TabIndex = 0;
			this.dgvCreate.Columns.Add("DrivesCar_vin", "DrivesCar_vin");
			this.dgvCreate.Columns.Add("DrivenByPerson(Person_id)", "DrivenByPerson(Person_id)");
			this.dgvCreate.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvCreate.Height = 75;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnCreate);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvCreate);
			this.Controls.Add(this.lblCreate);
			this.Name = "icCreatePersonDrivesCarInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCreate).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Create_PersonDrivesCarCore_InputControl
	#region Collection_PersonDrivesCarCore_InputControl
	public partial class Collection_PersonDrivesCarCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCollection;
		private System.Windows.Forms.Button btnCollection;
		private System.Windows.Forms.DataGridView dgvCollection;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Collection_PersonDrivesCar_connect;
		private ISampleModelContext testVar;
		private PersonDrivesCar abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// connect
			// 
			this.Collection_PersonDrivesCar_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Collection_PersonDrivesCar_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblCollection = new System.Windows.Forms.Label();
			this.lblCollection.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblCollection.Name = "lblCollection";
			this.lblCollection.Text = "Click the Collection button to refresh the list of PersonDrivesCar records.";
			// 
			// btn
			// 
			this.btnCollection = new System.Windows.Forms.Button();
			this.btnCollection.Location = new System.Drawing.Point(400, 10);
			this.btnCollection.Name = "btnCollection";
			this.btnCollection.TabIndex = 3;
			this.btnCollection.Text = "Collection";
			this.btnCollection.UseVisualStyleBackColor = true;
			this.btnCollection.Click += new System.EventHandler(this.btnCollection_Click);
			// 
			// dgv
			// 
			this.dgvCollection = new System.Windows.Forms.DataGridView();
			this.dgvCollection.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCollection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCollection.Name = "dgvCollection";
			this.dgvCollection.TabIndex = 0;
			this.dgvCollection.Columns.Add("DrivesCar_vin", "DrivesCar_vin");
			this.dgvCollection.Columns.Add("DrivenByPerson(Person_id)", "DrivenByPerson(Person_id)");
			this.dgvCollection.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvCollection.Height = 300;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnCollection);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvCollection);
			this.Controls.Add(this.lblCollection);
			this.Name = "icCollectionPersonDrivesCarInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCollection).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Collection_PersonDrivesCarCore_InputControl
	#region SelectPersonDrivesCarCore_InputControl
	public partial class Select_PersonDrivesCarCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblSelectionMode;
		private System.Windows.Forms.ComboBox cbxSelectionMode;
		private System.Windows.Forms.Label lblCurrentObject;
		private System.Windows.Forms.DataGridView dgvCurrentObject;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel pnlSave;
		private bool editMode;
		private System.Windows.Forms.Label lblNeedToSave;
		private System.Windows.Forms.Label lblSelect;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.DataGridView dgvSelect;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Select_PersonDrivesCar_connect;
		private ISampleModelContext testVar;
		private PersonDrivesCar abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.editMode = false;
			// 
			// lblNeedToSave
			// 
			this.lblNeedToSave = new System.Windows.Forms.Label();
			this.lblNeedToSave.Location = new System.Drawing.Point(0, 10);
			this.lblNeedToSave.Name = "lblNeedToSave";
			this.lblNeedToSave.Size = new System.Drawing.Size(200, 15);
			this.lblNeedToSave.Text = "";
			// 
			// btnCancel
			// 
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnCancel.Location = new System.Drawing.Point(300, 10);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnSave
			// 
			this.btnSave = new System.Windows.Forms.Button();
			this.btnSave.Location = new System.Drawing.Point(400, 10);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 7;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// pnlSave
			// 
			this.pnlSave = new System.Windows.Forms.Panel();
			this.pnlSave.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlSave.AutoSize = true;
			this.pnlSave.Location = new System.Drawing.Point(0, 0);
			this.pnlSave.Name = "pnlSave";
			this.pnlSave.AutoScroll = true;
			this.pnlSave.TabIndex = 5;
			this.pnlSave.Controls.Add(this.btnSave);
			this.pnlSave.Controls.Add(this.btnCancel);
			this.pnlSave.Controls.Add(this.lblNeedToSave);
			this.pnlSave.Visible = false;
			// 
			// lblSelectionMode
			// 
			this.lblSelectionMode = new System.Windows.Forms.Label();
			this.lblSelectionMode.Location = new System.Drawing.Point(0, 10);
			this.lblSelectionMode.Name = "lblSelectionMode";
			this.lblSelectionMode.Text = "SelectionMode:";
			// 
			// cbxSelectionMode
			// 
			this.cbxSelectionMode = new System.Windows.Forms.ComboBox();
			this.cbxSelectionMode.Location = new System.Drawing.Point(100, 10);
			this.cbxSelectionMode.Name = "cbxSelectionMode";
			this.cbxSelectionMode.Size = new System.Drawing.Size(200, 15);
			this.cbxSelectionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxSelectionMode.TabIndex = 2;
			this.cbxSelectionMode.SelectedIndexChanged += new System.EventHandler(this.cbxSelectionMode_SelectedIndexChanged);
			this.cbxSelectionMode.Items.Add("InternalUniquenessConstraint18");
			// 
			// lblCurrentObject
			// 
			this.lblCurrentObject = new System.Windows.Forms.Label();
			this.lblCurrentObject.Location = new System.Drawing.Point(0, 45);
			this.lblCurrentObject.Width = 300;
			this.lblCurrentObject.Name = "lblCurrentObject";
			this.lblCurrentObject.Text = "There is no selected PersonDrivesCar.";
			// 
			// dgvCurrentObject
			// 
			this.dgvCurrentObject = new System.Windows.Forms.DataGridView();
			this.dgvCurrentObject.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCurrentObject.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCurrentObject.Location = new System.Drawing.Point(0, 265);
			this.dgvCurrentObject.Name = "dgvCurrentObject";
			this.dgvCurrentObject.Size = new System.Drawing.Size(500, 150);
			this.dgvCurrentObject.TabIndex = 4;
			this.dgvCurrentObject.Columns.Add("DrivesCar_vin", "DrivesCar_vin");
			this.dgvCurrentObject.Columns["DrivesCar_vin"].Visible = false;
			this.dgvCurrentObject.Columns.Add("DrivenByPerson(Person_id)", "DrivenByPerson(Person_id)");
			this.dgvCurrentObject.Columns["DrivenByPerson(Person_id)"].Visible = false;
			this.dgvCurrentObject.Visible = false;
			this.dgvCurrentObject.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvCurrentObject_CellBeginEdit);
			this.Controls.Add(this.pnlSave);
			this.Controls.Add(this.dgvCurrentObject);
			// 
			// connect
			// 
			this.Select_PersonDrivesCar_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Select_PersonDrivesCar_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblSelect = new System.Windows.Forms.Label();
			this.lblSelect.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSelect.Name = "lblSelect";
			this.lblSelect.Text = "Enter data to Select PersonDrivesCar by:";
			// 
			// btn
			// 
			this.btnSelect = new System.Windows.Forms.Button();
			this.btnSelect.Location = new System.Drawing.Point(400, 10);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.TabIndex = 3;
			this.btnSelect.Text = "Select";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// dgv
			// 
			this.dgvSelect = new System.Windows.Forms.DataGridView();
			this.dgvSelect.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvSelect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvSelect.Name = "dgvSelect";
			this.dgvSelect.TabIndex = 0;
			this.dgvSelect.Columns.Add("DrivesCar_vin", "DrivesCar_vin");
			this.dgvSelect.Columns.Add("DrivenByPerson(Person_id)", "DrivenByPerson(Person_id)");
			this.dgvSelect.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvSelect.Height = 75;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnSelect);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvSelect);
			this.Controls.Add(this.lblSelect);
			this.Name = "icSelectPersonDrivesCarInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvSelect).EndInit();
			this.ResumeLayout(false);
			this.dgvCurrentObject.Height = this.dgvSelect.Height;
			this.pnlDisplay.Controls.Add(this.lblCurrentObject);
			this.pnlDisplay.Controls.Add(this.cbxSelectionMode);
			this.pnlDisplay.Controls.Add(this.lblSelectionMode);

			if (this.cbxSelectionMode.Items.Count > 0)
			{
				this.cbxSelectionMode.SelectedIndex = 0;
			}
		}
		#endregion // InitializeComponent method
	}
	#endregion // Select_PersonDrivesCarCore_InputControl
	#region PersonHasNickNameCore_InputControl
	public partial class PersonHasNickNameCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.TabControl actionTabs;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// actionTabs
			// 
			this.actionTabs = new System.Windows.Forms.TabControl();
			this.actionTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.Size = new System.Drawing.Size(530, 485);
			this.actionTabs.Location = new System.Drawing.Point(0, 0);
			this.actionTabs.Name = "actionTabs";
			this.actionTabs.TabPages.Add("Collection", "Collection");
			Collection_PersonHasNickNameCore_InputControl icCollection = new Collection_PersonHasNickNameCore_InputControl();
			icCollection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Collection"].Controls.Add(icCollection);
			this.actionTabs.TabPages.Add("Create", "Create");
			Create_PersonHasNickNameCore_InputControl icCreate = new Create_PersonHasNickNameCore_InputControl();
			icCreate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Create"].Controls.Add(icCreate);
			this.actionTabs.TabPages.Add("Select", "Select and Edit");
			Select_PersonHasNickNameCore_InputControl icSelect = new Select_PersonHasNickNameCore_InputControl();
			icSelect.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Select"].Controls.Add(icSelect);
			// 
			// this
			// 
			this.Controls.Add(actionTabs);
			this.Size = new System.Drawing.Size(540, 500);
		}
		#endregion // InitializeComponent method
	}
	#endregion // PersonHasNickNameCore_InputControl
	#region Create_PersonHasNickNameCore_InputControl
	public partial class Create_PersonHasNickNameCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCreate;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.DataGridView dgvCreate;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Create_PersonHasNickName_connect;
		private ISampleModelContext testVar;
		private PersonHasNickName abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// connect
			// 
			this.Create_PersonHasNickName_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Create_PersonHasNickName_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblCreate = new System.Windows.Forms.Label();
			this.lblCreate.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblCreate.Name = "lblCreate";
			this.lblCreate.Text = "Enter data to Create PersonHasNickName by:";
			// 
			// btn
			// 
			this.btnCreate = new System.Windows.Forms.Button();
			this.btnCreate.Location = new System.Drawing.Point(400, 10);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.TabIndex = 3;
			this.btnCreate.Text = "Create";
			this.btnCreate.UseVisualStyleBackColor = true;
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// dgv
			// 
			this.dgvCreate = new System.Windows.Forms.DataGridView();
			this.dgvCreate.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCreate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCreate.Name = "dgvCreate";
			this.dgvCreate.TabIndex = 0;
			this.dgvCreate.Columns.Add("NickName", "NickName");
			this.dgvCreate.Columns.Add("Person(Person_id)", "Person(Person_id)");
			this.dgvCreate.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvCreate.Height = 75;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnCreate);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvCreate);
			this.Controls.Add(this.lblCreate);
			this.Name = "icCreatePersonHasNickNameInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCreate).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Create_PersonHasNickNameCore_InputControl
	#region Collection_PersonHasNickNameCore_InputControl
	public partial class Collection_PersonHasNickNameCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCollection;
		private System.Windows.Forms.Button btnCollection;
		private System.Windows.Forms.DataGridView dgvCollection;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Collection_PersonHasNickName_connect;
		private ISampleModelContext testVar;
		private PersonHasNickName abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// connect
			// 
			this.Collection_PersonHasNickName_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Collection_PersonHasNickName_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblCollection = new System.Windows.Forms.Label();
			this.lblCollection.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblCollection.Name = "lblCollection";
			this.lblCollection.Text = "Click the Collection button to refresh the list of PersonHasNickName records.";
			// 
			// btn
			// 
			this.btnCollection = new System.Windows.Forms.Button();
			this.btnCollection.Location = new System.Drawing.Point(400, 10);
			this.btnCollection.Name = "btnCollection";
			this.btnCollection.TabIndex = 3;
			this.btnCollection.Text = "Collection";
			this.btnCollection.UseVisualStyleBackColor = true;
			this.btnCollection.Click += new System.EventHandler(this.btnCollection_Click);
			// 
			// dgv
			// 
			this.dgvCollection = new System.Windows.Forms.DataGridView();
			this.dgvCollection.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCollection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCollection.Name = "dgvCollection";
			this.dgvCollection.TabIndex = 0;
			this.dgvCollection.Columns.Add("NickName", "NickName");
			this.dgvCollection.Columns.Add("Person(Person_id)", "Person(Person_id)");
			this.dgvCollection.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvCollection.Height = 300;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnCollection);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvCollection);
			this.Controls.Add(this.lblCollection);
			this.Name = "icCollectionPersonHasNickNameInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCollection).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Collection_PersonHasNickNameCore_InputControl
	#region SelectPersonHasNickNameCore_InputControl
	public partial class Select_PersonHasNickNameCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblSelectionMode;
		private System.Windows.Forms.ComboBox cbxSelectionMode;
		private System.Windows.Forms.Label lblCurrentObject;
		private System.Windows.Forms.DataGridView dgvCurrentObject;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel pnlSave;
		private bool editMode;
		private System.Windows.Forms.Label lblNeedToSave;
		private System.Windows.Forms.Label lblSelect;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.DataGridView dgvSelect;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Select_PersonHasNickName_connect;
		private ISampleModelContext testVar;
		private PersonHasNickName abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.editMode = false;
			// 
			// lblNeedToSave
			// 
			this.lblNeedToSave = new System.Windows.Forms.Label();
			this.lblNeedToSave.Location = new System.Drawing.Point(0, 10);
			this.lblNeedToSave.Name = "lblNeedToSave";
			this.lblNeedToSave.Size = new System.Drawing.Size(200, 15);
			this.lblNeedToSave.Text = "";
			// 
			// btnCancel
			// 
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnCancel.Location = new System.Drawing.Point(300, 10);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnSave
			// 
			this.btnSave = new System.Windows.Forms.Button();
			this.btnSave.Location = new System.Drawing.Point(400, 10);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 7;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// pnlSave
			// 
			this.pnlSave = new System.Windows.Forms.Panel();
			this.pnlSave.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlSave.AutoSize = true;
			this.pnlSave.Location = new System.Drawing.Point(0, 0);
			this.pnlSave.Name = "pnlSave";
			this.pnlSave.AutoScroll = true;
			this.pnlSave.TabIndex = 5;
			this.pnlSave.Controls.Add(this.btnSave);
			this.pnlSave.Controls.Add(this.btnCancel);
			this.pnlSave.Controls.Add(this.lblNeedToSave);
			this.pnlSave.Visible = false;
			// 
			// lblSelectionMode
			// 
			this.lblSelectionMode = new System.Windows.Forms.Label();
			this.lblSelectionMode.Location = new System.Drawing.Point(0, 10);
			this.lblSelectionMode.Name = "lblSelectionMode";
			this.lblSelectionMode.Text = "SelectionMode:";
			// 
			// cbxSelectionMode
			// 
			this.cbxSelectionMode = new System.Windows.Forms.ComboBox();
			this.cbxSelectionMode.Location = new System.Drawing.Point(100, 10);
			this.cbxSelectionMode.Name = "cbxSelectionMode";
			this.cbxSelectionMode.Size = new System.Drawing.Size(200, 15);
			this.cbxSelectionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxSelectionMode.TabIndex = 2;
			this.cbxSelectionMode.SelectedIndexChanged += new System.EventHandler(this.cbxSelectionMode_SelectedIndexChanged);
			this.cbxSelectionMode.Items.Add("InternalUniquenessConstraint33");
			// 
			// lblCurrentObject
			// 
			this.lblCurrentObject = new System.Windows.Forms.Label();
			this.lblCurrentObject.Location = new System.Drawing.Point(0, 45);
			this.lblCurrentObject.Width = 300;
			this.lblCurrentObject.Name = "lblCurrentObject";
			this.lblCurrentObject.Text = "There is no selected PersonHasNickName.";
			// 
			// dgvCurrentObject
			// 
			this.dgvCurrentObject = new System.Windows.Forms.DataGridView();
			this.dgvCurrentObject.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCurrentObject.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCurrentObject.Location = new System.Drawing.Point(0, 265);
			this.dgvCurrentObject.Name = "dgvCurrentObject";
			this.dgvCurrentObject.Size = new System.Drawing.Size(500, 150);
			this.dgvCurrentObject.TabIndex = 4;
			this.dgvCurrentObject.Columns.Add("NickName", "NickName");
			this.dgvCurrentObject.Columns["NickName"].Visible = false;
			this.dgvCurrentObject.Columns.Add("Person(Person_id)", "Person(Person_id)");
			this.dgvCurrentObject.Columns["Person(Person_id)"].Visible = false;
			this.dgvCurrentObject.Visible = false;
			this.dgvCurrentObject.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvCurrentObject_CellBeginEdit);
			this.Controls.Add(this.pnlSave);
			this.Controls.Add(this.dgvCurrentObject);
			// 
			// connect
			// 
			this.Select_PersonHasNickName_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Select_PersonHasNickName_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblSelect = new System.Windows.Forms.Label();
			this.lblSelect.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSelect.Name = "lblSelect";
			this.lblSelect.Text = "Enter data to Select PersonHasNickName by:";
			// 
			// btn
			// 
			this.btnSelect = new System.Windows.Forms.Button();
			this.btnSelect.Location = new System.Drawing.Point(400, 10);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.TabIndex = 3;
			this.btnSelect.Text = "Select";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// dgv
			// 
			this.dgvSelect = new System.Windows.Forms.DataGridView();
			this.dgvSelect.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvSelect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvSelect.Name = "dgvSelect";
			this.dgvSelect.TabIndex = 0;
			this.dgvSelect.Columns.Add("NickName", "NickName");
			this.dgvSelect.Columns.Add("Person(Person_id)", "Person(Person_id)");
			this.dgvSelect.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvSelect.Height = 75;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnSelect);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvSelect);
			this.Controls.Add(this.lblSelect);
			this.Name = "icSelectPersonHasNickNameInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvSelect).EndInit();
			this.ResumeLayout(false);
			this.dgvCurrentObject.Height = this.dgvSelect.Height;
			this.pnlDisplay.Controls.Add(this.lblCurrentObject);
			this.pnlDisplay.Controls.Add(this.cbxSelectionMode);
			this.pnlDisplay.Controls.Add(this.lblSelectionMode);

			if (this.cbxSelectionMode.Items.Count > 0)
			{
				this.cbxSelectionMode.SelectedIndex = 0;
			}
		}
		#endregion // InitializeComponent method
	}
	#endregion // Select_PersonHasNickNameCore_InputControl
	#region ReviewCore_InputControl
	public partial class ReviewCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.TabControl actionTabs;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// actionTabs
			// 
			this.actionTabs = new System.Windows.Forms.TabControl();
			this.actionTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.Size = new System.Drawing.Size(530, 485);
			this.actionTabs.Location = new System.Drawing.Point(0, 0);
			this.actionTabs.Name = "actionTabs";
			this.actionTabs.TabPages.Add("Collection", "Collection");
			Collection_ReviewCore_InputControl icCollection = new Collection_ReviewCore_InputControl();
			icCollection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Collection"].Controls.Add(icCollection);
			this.actionTabs.TabPages.Add("Create", "Create");
			Create_ReviewCore_InputControl icCreate = new Create_ReviewCore_InputControl();
			icCreate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Create"].Controls.Add(icCreate);
			this.actionTabs.TabPages.Add("Select", "Select and Edit");
			Select_ReviewCore_InputControl icSelect = new Select_ReviewCore_InputControl();
			icSelect.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Select"].Controls.Add(icSelect);
			// 
			// this
			// 
			this.Controls.Add(actionTabs);
			this.Size = new System.Drawing.Size(540, 500);
		}
		#endregion // InitializeComponent method
	}
	#endregion // ReviewCore_InputControl
	#region Create_ReviewCore_InputControl
	public partial class Create_ReviewCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCreate;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.DataGridView dgvCreate;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Create_Review_connect;
		private ISampleModelContext testVar;
		private Review abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// connect
			// 
			this.Create_Review_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Create_Review_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblCreate = new System.Windows.Forms.Label();
			this.lblCreate.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblCreate.Name = "lblCreate";
			this.lblCreate.Text = "Enter data to Create Review by:";
			// 
			// btn
			// 
			this.btnCreate = new System.Windows.Forms.Button();
			this.btnCreate.Location = new System.Drawing.Point(400, 10);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.TabIndex = 3;
			this.btnCreate.Text = "Create";
			this.btnCreate.UseVisualStyleBackColor = true;
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// dgv
			// 
			this.dgvCreate = new System.Windows.Forms.DataGridView();
			this.dgvCreate.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCreate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCreate.Name = "dgvCreate";
			this.dgvCreate.TabIndex = 0;
			this.dgvCreate.Columns.Add("Car_vin", "Car_vin");
			this.dgvCreate.Columns.Add("Rating_Nr_Integer", "Rating_Nr_Integer");
			this.dgvCreate.Columns.Add("Criterion_Name", "Criterion_Name");
			this.dgvCreate.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvCreate.Height = 75;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnCreate);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvCreate);
			this.Controls.Add(this.lblCreate);
			this.Name = "icCreateReviewInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCreate).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Create_ReviewCore_InputControl
	#region Collection_ReviewCore_InputControl
	public partial class Collection_ReviewCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCollection;
		private System.Windows.Forms.Button btnCollection;
		private System.Windows.Forms.DataGridView dgvCollection;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Collection_Review_connect;
		private ISampleModelContext testVar;
		private Review abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// connect
			// 
			this.Collection_Review_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Collection_Review_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblCollection = new System.Windows.Forms.Label();
			this.lblCollection.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblCollection.Name = "lblCollection";
			this.lblCollection.Text = "Click the Collection button to refresh the list of Review records.";
			// 
			// btn
			// 
			this.btnCollection = new System.Windows.Forms.Button();
			this.btnCollection.Location = new System.Drawing.Point(400, 10);
			this.btnCollection.Name = "btnCollection";
			this.btnCollection.TabIndex = 3;
			this.btnCollection.Text = "Collection";
			this.btnCollection.UseVisualStyleBackColor = true;
			this.btnCollection.Click += new System.EventHandler(this.btnCollection_Click);
			// 
			// dgv
			// 
			this.dgvCollection = new System.Windows.Forms.DataGridView();
			this.dgvCollection.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCollection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCollection.Name = "dgvCollection";
			this.dgvCollection.TabIndex = 0;
			this.dgvCollection.Columns.Add("Car_vin", "Car_vin");
			this.dgvCollection.Columns.Add("Rating_Nr_Integer", "Rating_Nr_Integer");
			this.dgvCollection.Columns.Add("Criterion_Name", "Criterion_Name");
			this.dgvCollection.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvCollection.Height = 300;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnCollection);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvCollection);
			this.Controls.Add(this.lblCollection);
			this.Name = "icCollectionReviewInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCollection).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Collection_ReviewCore_InputControl
	#region SelectReviewCore_InputControl
	public partial class Select_ReviewCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblSelectionMode;
		private System.Windows.Forms.ComboBox cbxSelectionMode;
		private System.Windows.Forms.Label lblCurrentObject;
		private System.Windows.Forms.DataGridView dgvCurrentObject;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel pnlSave;
		private bool editMode;
		private System.Windows.Forms.Label lblNeedToSave;
		private System.Windows.Forms.Label lblSelect;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.DataGridView dgvSelect;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Select_Review_connect;
		private ISampleModelContext testVar;
		private Review abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.editMode = false;
			// 
			// lblNeedToSave
			// 
			this.lblNeedToSave = new System.Windows.Forms.Label();
			this.lblNeedToSave.Location = new System.Drawing.Point(0, 10);
			this.lblNeedToSave.Name = "lblNeedToSave";
			this.lblNeedToSave.Size = new System.Drawing.Size(200, 15);
			this.lblNeedToSave.Text = "";
			// 
			// btnCancel
			// 
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnCancel.Location = new System.Drawing.Point(300, 10);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnSave
			// 
			this.btnSave = new System.Windows.Forms.Button();
			this.btnSave.Location = new System.Drawing.Point(400, 10);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 7;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// pnlSave
			// 
			this.pnlSave = new System.Windows.Forms.Panel();
			this.pnlSave.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlSave.AutoSize = true;
			this.pnlSave.Location = new System.Drawing.Point(0, 0);
			this.pnlSave.Name = "pnlSave";
			this.pnlSave.AutoScroll = true;
			this.pnlSave.TabIndex = 5;
			this.pnlSave.Controls.Add(this.btnSave);
			this.pnlSave.Controls.Add(this.btnCancel);
			this.pnlSave.Controls.Add(this.lblNeedToSave);
			this.pnlSave.Visible = false;
			// 
			// lblSelectionMode
			// 
			this.lblSelectionMode = new System.Windows.Forms.Label();
			this.lblSelectionMode.Location = new System.Drawing.Point(0, 10);
			this.lblSelectionMode.Name = "lblSelectionMode";
			this.lblSelectionMode.Text = "SelectionMode:";
			// 
			// cbxSelectionMode
			// 
			this.cbxSelectionMode = new System.Windows.Forms.ComboBox();
			this.cbxSelectionMode.Location = new System.Drawing.Point(100, 10);
			this.cbxSelectionMode.Name = "cbxSelectionMode";
			this.cbxSelectionMode.Size = new System.Drawing.Size(200, 15);
			this.cbxSelectionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxSelectionMode.TabIndex = 2;
			this.cbxSelectionMode.SelectedIndexChanged += new System.EventHandler(this.cbxSelectionMode_SelectedIndexChanged);
			this.cbxSelectionMode.Items.Add("InternalUniquenessConstraint26");
			// 
			// lblCurrentObject
			// 
			this.lblCurrentObject = new System.Windows.Forms.Label();
			this.lblCurrentObject.Location = new System.Drawing.Point(0, 45);
			this.lblCurrentObject.Width = 300;
			this.lblCurrentObject.Name = "lblCurrentObject";
			this.lblCurrentObject.Text = "There is no selected Review.";
			// 
			// dgvCurrentObject
			// 
			this.dgvCurrentObject = new System.Windows.Forms.DataGridView();
			this.dgvCurrentObject.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCurrentObject.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCurrentObject.Location = new System.Drawing.Point(0, 265);
			this.dgvCurrentObject.Name = "dgvCurrentObject";
			this.dgvCurrentObject.Size = new System.Drawing.Size(500, 150);
			this.dgvCurrentObject.TabIndex = 4;
			this.dgvCurrentObject.Columns.Add("Car_vin", "Car_vin");
			this.dgvCurrentObject.Columns["Car_vin"].Visible = false;
			this.dgvCurrentObject.Columns.Add("Rating_Nr_Integer", "Rating_Nr_Integer");
			this.dgvCurrentObject.Columns["Rating_Nr_Integer"].Visible = false;
			this.dgvCurrentObject.Columns.Add("Criterion_Name", "Criterion_Name");
			this.dgvCurrentObject.Columns["Criterion_Name"].Visible = false;
			this.dgvCurrentObject.Visible = false;
			this.dgvCurrentObject.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvCurrentObject_CellBeginEdit);
			this.Controls.Add(this.pnlSave);
			this.Controls.Add(this.dgvCurrentObject);
			// 
			// connect
			// 
			this.Select_Review_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Select_Review_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblSelect = new System.Windows.Forms.Label();
			this.lblSelect.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSelect.Name = "lblSelect";
			this.lblSelect.Text = "Enter data to Select Review by:";
			// 
			// btn
			// 
			this.btnSelect = new System.Windows.Forms.Button();
			this.btnSelect.Location = new System.Drawing.Point(400, 10);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.TabIndex = 3;
			this.btnSelect.Text = "Select";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// dgv
			// 
			this.dgvSelect = new System.Windows.Forms.DataGridView();
			this.dgvSelect.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvSelect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvSelect.Name = "dgvSelect";
			this.dgvSelect.TabIndex = 0;
			this.dgvSelect.Columns.Add("Car_vin", "Car_vin");
			this.dgvSelect.Columns.Add("Rating_Nr_Integer", "Rating_Nr_Integer");
			this.dgvSelect.Columns.Add("Criterion_Name", "Criterion_Name");
			this.dgvSelect.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvSelect.Height = 75;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnSelect);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvSelect);
			this.Controls.Add(this.lblSelect);
			this.Name = "icSelectReviewInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvSelect).EndInit();
			this.ResumeLayout(false);
			this.dgvCurrentObject.Height = this.dgvSelect.Height;
			this.pnlDisplay.Controls.Add(this.lblCurrentObject);
			this.pnlDisplay.Controls.Add(this.cbxSelectionMode);
			this.pnlDisplay.Controls.Add(this.lblSelectionMode);

			if (this.cbxSelectionMode.Items.Count > 0)
			{
				this.cbxSelectionMode.SelectedIndex = 0;
			}
		}
		#endregion // InitializeComponent method
	}
	#endregion // Select_ReviewCore_InputControl
	#region TaskCore_InputControl
	public partial class TaskCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.TabControl actionTabs;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// actionTabs
			// 
			this.actionTabs = new System.Windows.Forms.TabControl();
			this.actionTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.Size = new System.Drawing.Size(530, 485);
			this.actionTabs.Location = new System.Drawing.Point(0, 0);
			this.actionTabs.Name = "actionTabs";
			this.actionTabs.TabPages.Add("Collection", "Collection");
			Collection_TaskCore_InputControl icCollection = new Collection_TaskCore_InputControl();
			icCollection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Collection"].Controls.Add(icCollection);
			this.actionTabs.TabPages.Add("Create", "Create");
			Create_TaskCore_InputControl icCreate = new Create_TaskCore_InputControl();
			icCreate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Create"].Controls.Add(icCreate);
			this.actionTabs.TabPages.Add("Select", "Select and Edit");
			Select_TaskCore_InputControl icSelect = new Select_TaskCore_InputControl();
			icSelect.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Select"].Controls.Add(icSelect);
			// 
			// this
			// 
			this.Controls.Add(actionTabs);
			this.Size = new System.Drawing.Size(540, 500);
		}
		#endregion // InitializeComponent method
	}
	#endregion // TaskCore_InputControl
	#region Create_TaskCore_InputControl
	public partial class Create_TaskCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCreate;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.DataGridView dgvCreate;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Create_Task_connect;
		private ISampleModelContext testVar;
		private Task abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// connect
			// 
			this.Create_Task_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Create_Task_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblCreate = new System.Windows.Forms.Label();
			this.lblCreate.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblCreate.Name = "lblCreate";
			this.lblCreate.Text = "Enter data to Create Task by:";
			// 
			// btn
			// 
			this.btnCreate = new System.Windows.Forms.Button();
			this.btnCreate.Location = new System.Drawing.Point(400, 10);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.TabIndex = 3;
			this.btnCreate.Text = "Create";
			this.btnCreate.UseVisualStyleBackColor = true;
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// dgv
			// 
			this.dgvCreate = new System.Windows.Forms.DataGridView();
			this.dgvCreate.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCreate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCreate.Name = "dgvCreate";
			this.dgvCreate.TabIndex = 0;
			this.dgvCreate.Columns.Add("Person", "Person");
			this.dgvCreate.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvCreate.Height = 75;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnCreate);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvCreate);
			this.Controls.Add(this.lblCreate);
			this.Name = "icCreateTaskInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCreate).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Create_TaskCore_InputControl
	#region Collection_TaskCore_InputControl
	public partial class Collection_TaskCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCollection;
		private System.Windows.Forms.Button btnCollection;
		private System.Windows.Forms.DataGridView dgvCollection;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Collection_Task_connect;
		private ISampleModelContext testVar;
		private Task abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// connect
			// 
			this.Collection_Task_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Collection_Task_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblCollection = new System.Windows.Forms.Label();
			this.lblCollection.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblCollection.Name = "lblCollection";
			this.lblCollection.Text = "Click the Collection button to refresh the list of Task records.";
			// 
			// btn
			// 
			this.btnCollection = new System.Windows.Forms.Button();
			this.btnCollection.Location = new System.Drawing.Point(400, 10);
			this.btnCollection.Name = "btnCollection";
			this.btnCollection.TabIndex = 3;
			this.btnCollection.Text = "Collection";
			this.btnCollection.UseVisualStyleBackColor = true;
			this.btnCollection.Click += new System.EventHandler(this.btnCollection_Click);
			// 
			// dgv
			// 
			this.dgvCollection = new System.Windows.Forms.DataGridView();
			this.dgvCollection.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCollection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCollection.Name = "dgvCollection";
			this.dgvCollection.TabIndex = 0;
			this.dgvCollection.Columns.Add("Person", "Person");
			this.dgvCollection.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvCollection.Height = 300;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnCollection);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvCollection);
			this.Controls.Add(this.lblCollection);
			this.Name = "icCollectionTaskInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCollection).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Collection_TaskCore_InputControl
	#region SelectTaskCore_InputControl
	public partial class Select_TaskCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblSelectionMode;
		private System.Windows.Forms.ComboBox cbxSelectionMode;
		private System.Windows.Forms.Label lblCurrentObject;
		private System.Windows.Forms.DataGridView dgvCurrentObject;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel pnlSave;
		private bool editMode;
		private System.Windows.Forms.Label lblNeedToSave;
		private System.Windows.Forms.Label lblSelect;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.DataGridView dgvSelect;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Select_Task_connect;
		private ISampleModelContext testVar;
		private Task abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.editMode = false;
			// 
			// lblNeedToSave
			// 
			this.lblNeedToSave = new System.Windows.Forms.Label();
			this.lblNeedToSave.Location = new System.Drawing.Point(0, 10);
			this.lblNeedToSave.Name = "lblNeedToSave";
			this.lblNeedToSave.Size = new System.Drawing.Size(200, 15);
			this.lblNeedToSave.Text = "";
			// 
			// btnCancel
			// 
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnCancel.Location = new System.Drawing.Point(300, 10);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnSave
			// 
			this.btnSave = new System.Windows.Forms.Button();
			this.btnSave.Location = new System.Drawing.Point(400, 10);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 7;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// pnlSave
			// 
			this.pnlSave = new System.Windows.Forms.Panel();
			this.pnlSave.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlSave.AutoSize = true;
			this.pnlSave.Location = new System.Drawing.Point(0, 0);
			this.pnlSave.Name = "pnlSave";
			this.pnlSave.AutoScroll = true;
			this.pnlSave.TabIndex = 5;
			this.pnlSave.Controls.Add(this.btnSave);
			this.pnlSave.Controls.Add(this.btnCancel);
			this.pnlSave.Controls.Add(this.lblNeedToSave);
			this.pnlSave.Visible = false;
			// 
			// lblSelectionMode
			// 
			this.lblSelectionMode = new System.Windows.Forms.Label();
			this.lblSelectionMode.Location = new System.Drawing.Point(0, 10);
			this.lblSelectionMode.Name = "lblSelectionMode";
			this.lblSelectionMode.Text = "SelectionMode:";
			// 
			// cbxSelectionMode
			// 
			this.cbxSelectionMode = new System.Windows.Forms.ComboBox();
			this.cbxSelectionMode.Location = new System.Drawing.Point(100, 10);
			this.cbxSelectionMode.Name = "cbxSelectionMode";
			this.cbxSelectionMode.Size = new System.Drawing.Size(200, 15);
			this.cbxSelectionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxSelectionMode.TabIndex = 2;
			this.cbxSelectionMode.SelectedIndexChanged += new System.EventHandler(this.cbxSelectionMode_SelectedIndexChanged);
			// 
			// lblCurrentObject
			// 
			this.lblCurrentObject = new System.Windows.Forms.Label();
			this.lblCurrentObject.Location = new System.Drawing.Point(0, 45);
			this.lblCurrentObject.Width = 300;
			this.lblCurrentObject.Name = "lblCurrentObject";
			this.lblCurrentObject.Text = "There is no selected Task.";
			// 
			// dgvCurrentObject
			// 
			this.dgvCurrentObject = new System.Windows.Forms.DataGridView();
			this.dgvCurrentObject.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCurrentObject.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCurrentObject.Location = new System.Drawing.Point(0, 265);
			this.dgvCurrentObject.Name = "dgvCurrentObject";
			this.dgvCurrentObject.Size = new System.Drawing.Size(500, 150);
			this.dgvCurrentObject.TabIndex = 4;
			this.dgvCurrentObject.Columns.Add("Person", "Person");
			this.dgvCurrentObject.Columns["Person"].Visible = false;
			this.dgvCurrentObject.Visible = false;
			this.dgvCurrentObject.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvCurrentObject_CellBeginEdit);
			this.Controls.Add(this.pnlSave);
			this.Controls.Add(this.dgvCurrentObject);
			// 
			// connect
			// 
			this.Select_Task_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Select_Task_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblSelect = new System.Windows.Forms.Label();
			this.lblSelect.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSelect.Name = "lblSelect";
			this.lblSelect.Text = "Enter data to Select Task by:";
			// 
			// btn
			// 
			this.btnSelect = new System.Windows.Forms.Button();
			this.btnSelect.Location = new System.Drawing.Point(400, 10);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.TabIndex = 3;
			this.btnSelect.Text = "Select";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// dgv
			// 
			this.dgvSelect = new System.Windows.Forms.DataGridView();
			this.dgvSelect.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvSelect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvSelect.Name = "dgvSelect";
			this.dgvSelect.TabIndex = 0;
			this.dgvSelect.Columns.Add("Person", "Person");
			this.dgvSelect.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvSelect.Height = 75;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnSelect);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvSelect);
			this.Controls.Add(this.lblSelect);
			this.Name = "icSelectTaskInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvSelect).EndInit();
			this.ResumeLayout(false);
			this.dgvCurrentObject.Height = this.dgvSelect.Height;
			this.pnlDisplay.Controls.Add(this.lblCurrentObject);
			this.pnlDisplay.Controls.Add(this.cbxSelectionMode);
			this.pnlDisplay.Controls.Add(this.lblSelectionMode);

			if (this.cbxSelectionMode.Items.Count > 0)
			{
				this.cbxSelectionMode.SelectedIndex = 0;
			}
		}
		#endregion // InitializeComponent method
	}
	#endregion // Select_TaskCore_InputControl
	#region ValueType1Core_InputControl
	public partial class ValueType1Core_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.TabControl actionTabs;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// actionTabs
			// 
			this.actionTabs = new System.Windows.Forms.TabControl();
			this.actionTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.Size = new System.Drawing.Size(530, 485);
			this.actionTabs.Location = new System.Drawing.Point(0, 0);
			this.actionTabs.Name = "actionTabs";
			this.actionTabs.TabPages.Add("Collection", "Collection");
			Collection_ValueType1Core_InputControl icCollection = new Collection_ValueType1Core_InputControl();
			icCollection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Collection"].Controls.Add(icCollection);
			this.actionTabs.TabPages.Add("Create", "Create");
			Create_ValueType1Core_InputControl icCreate = new Create_ValueType1Core_InputControl();
			icCreate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Create"].Controls.Add(icCreate);
			this.actionTabs.TabPages.Add("Select", "Select and Edit");
			Select_ValueType1Core_InputControl icSelect = new Select_ValueType1Core_InputControl();
			icSelect.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Select"].Controls.Add(icSelect);
			// 
			// this
			// 
			this.Controls.Add(actionTabs);
			this.Size = new System.Drawing.Size(540, 500);
		}
		#endregion // InitializeComponent method
	}
	#endregion // ValueType1Core_InputControl
	#region Create_ValueType1Core_InputControl
	public partial class Create_ValueType1Core_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCreate;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.DataGridView dgvCreate;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Create_ValueType1_connect;
		private ISampleModelContext testVar;
		private ValueType1 abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// connect
			// 
			this.Create_ValueType1_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Create_ValueType1_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblCreate = new System.Windows.Forms.Label();
			this.lblCreate.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblCreate.Name = "lblCreate";
			this.lblCreate.Text = "Enter data to Create ValueType1 by:";
			// 
			// btn
			// 
			this.btnCreate = new System.Windows.Forms.Button();
			this.btnCreate.Location = new System.Drawing.Point(400, 10);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.TabIndex = 3;
			this.btnCreate.Text = "Create";
			this.btnCreate.UseVisualStyleBackColor = true;
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// dgv
			// 
			this.dgvCreate = new System.Windows.Forms.DataGridView();
			this.dgvCreate.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCreate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCreate.Name = "dgvCreate";
			this.dgvCreate.TabIndex = 0;
			this.dgvCreate.Columns.Add("ValueType1Value", "ValueType1Value");
			this.dgvCreate.Columns.Add("DoesSomethingWithPerson", "DoesSomethingWithPerson");
			this.dgvCreate.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvCreate.Height = 75;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnCreate);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvCreate);
			this.Controls.Add(this.lblCreate);
			this.Name = "icCreateValueType1InputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCreate).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Create_ValueType1Core_InputControl
	#region Collection_ValueType1Core_InputControl
	public partial class Collection_ValueType1Core_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCollection;
		private System.Windows.Forms.Button btnCollection;
		private System.Windows.Forms.DataGridView dgvCollection;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Collection_ValueType1_connect;
		private ISampleModelContext testVar;
		private ValueType1 abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			// 
			// connect
			// 
			this.Collection_ValueType1_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Collection_ValueType1_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblCollection = new System.Windows.Forms.Label();
			this.lblCollection.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblCollection.Name = "lblCollection";
			this.lblCollection.Text = "Click the Collection button to refresh the list of ValueType1 records.";
			// 
			// btn
			// 
			this.btnCollection = new System.Windows.Forms.Button();
			this.btnCollection.Location = new System.Drawing.Point(400, 10);
			this.btnCollection.Name = "btnCollection";
			this.btnCollection.TabIndex = 3;
			this.btnCollection.Text = "Collection";
			this.btnCollection.UseVisualStyleBackColor = true;
			this.btnCollection.Click += new System.EventHandler(this.btnCollection_Click);
			// 
			// dgv
			// 
			this.dgvCollection = new System.Windows.Forms.DataGridView();
			this.dgvCollection.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCollection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCollection.Name = "dgvCollection";
			this.dgvCollection.TabIndex = 0;
			this.dgvCollection.Columns.Add("ValueType1Value", "ValueType1Value");
			this.dgvCollection.Columns.Add("DoesSomethingWithPerson", "DoesSomethingWithPerson");
			this.dgvCollection.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvCollection.Height = 300;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnCollection);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvCollection);
			this.Controls.Add(this.lblCollection);
			this.Name = "icCollectionValueType1InputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCollection).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Collection_ValueType1Core_InputControl
	#region SelectValueType1Core_InputControl
	public partial class Select_ValueType1Core_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblSelectionMode;
		private System.Windows.Forms.ComboBox cbxSelectionMode;
		private System.Windows.Forms.Label lblCurrentObject;
		private System.Windows.Forms.DataGridView dgvCurrentObject;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel pnlSave;
		private bool editMode;
		private System.Windows.Forms.Label lblNeedToSave;
		private System.Windows.Forms.Label lblSelect;
		private System.Windows.Forms.Button btnSelect;
		private System.Windows.Forms.DataGridView dgvSelect;
		private System.Windows.Forms.Panel pnlDisplay;
		private SampleModelContext.ConnectionDelegate Select_ValueType1_connect;
		private ISampleModelContext testVar;
		private ValueType1 abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.editMode = false;
			// 
			// lblNeedToSave
			// 
			this.lblNeedToSave = new System.Windows.Forms.Label();
			this.lblNeedToSave.Location = new System.Drawing.Point(0, 10);
			this.lblNeedToSave.Name = "lblNeedToSave";
			this.lblNeedToSave.Size = new System.Drawing.Size(200, 15);
			this.lblNeedToSave.Text = "";
			// 
			// btnCancel
			// 
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnCancel.Location = new System.Drawing.Point(300, 10);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnSave
			// 
			this.btnSave = new System.Windows.Forms.Button();
			this.btnSave.Location = new System.Drawing.Point(400, 10);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 7;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// pnlSave
			// 
			this.pnlSave = new System.Windows.Forms.Panel();
			this.pnlSave.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlSave.AutoSize = true;
			this.pnlSave.Location = new System.Drawing.Point(0, 0);
			this.pnlSave.Name = "pnlSave";
			this.pnlSave.AutoScroll = true;
			this.pnlSave.TabIndex = 5;
			this.pnlSave.Controls.Add(this.btnSave);
			this.pnlSave.Controls.Add(this.btnCancel);
			this.pnlSave.Controls.Add(this.lblNeedToSave);
			this.pnlSave.Visible = false;
			// 
			// lblSelectionMode
			// 
			this.lblSelectionMode = new System.Windows.Forms.Label();
			this.lblSelectionMode.Location = new System.Drawing.Point(0, 10);
			this.lblSelectionMode.Name = "lblSelectionMode";
			this.lblSelectionMode.Text = "SelectionMode:";
			// 
			// cbxSelectionMode
			// 
			this.cbxSelectionMode = new System.Windows.Forms.ComboBox();
			this.cbxSelectionMode.Location = new System.Drawing.Point(100, 10);
			this.cbxSelectionMode.Name = "cbxSelectionMode";
			this.cbxSelectionMode.Size = new System.Drawing.Size(200, 15);
			this.cbxSelectionMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxSelectionMode.TabIndex = 2;
			this.cbxSelectionMode.SelectedIndexChanged += new System.EventHandler(this.cbxSelectionMode_SelectedIndexChanged);
			this.cbxSelectionMode.Items.Add("ValueType1Value");
			// 
			// lblCurrentObject
			// 
			this.lblCurrentObject = new System.Windows.Forms.Label();
			this.lblCurrentObject.Location = new System.Drawing.Point(0, 45);
			this.lblCurrentObject.Width = 300;
			this.lblCurrentObject.Name = "lblCurrentObject";
			this.lblCurrentObject.Text = "There is no selected ValueType1.";
			// 
			// dgvCurrentObject
			// 
			this.dgvCurrentObject = new System.Windows.Forms.DataGridView();
			this.dgvCurrentObject.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvCurrentObject.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvCurrentObject.Location = new System.Drawing.Point(0, 265);
			this.dgvCurrentObject.Name = "dgvCurrentObject";
			this.dgvCurrentObject.Size = new System.Drawing.Size(500, 150);
			this.dgvCurrentObject.TabIndex = 4;
			this.dgvCurrentObject.Columns.Add("ValueType1Value", "ValueType1Value");
			this.dgvCurrentObject.Columns["ValueType1Value"].Visible = false;
			this.dgvCurrentObject.Columns.Add("DoesSomethingWithPerson", "DoesSomethingWithPerson");
			this.dgvCurrentObject.Columns["DoesSomethingWithPerson"].Visible = false;
			this.dgvCurrentObject.Visible = false;
			this.dgvCurrentObject.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvCurrentObject_CellBeginEdit);
			this.Controls.Add(this.pnlSave);
			this.Controls.Add(this.dgvCurrentObject);
			// 
			// connect
			// 
			this.Select_ValueType1_connect = new SampleModelContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new SampleModelContext(Select_ValueType1_connect);
			// 
			// abstractTypeVar
			// 
			this.abstractTypeVar = null;
			// 
			// lbl
			// 
			this.lblSelect = new System.Windows.Forms.Label();
			this.lblSelect.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblSelect.Name = "lblSelect";
			this.lblSelect.Text = "Enter data to Select ValueType1 by:";
			// 
			// btn
			// 
			this.btnSelect = new System.Windows.Forms.Button();
			this.btnSelect.Location = new System.Drawing.Point(400, 10);
			this.btnSelect.Name = "btnSelect";
			this.btnSelect.TabIndex = 3;
			this.btnSelect.Text = "Select";
			this.btnSelect.UseVisualStyleBackColor = true;
			this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
			// 
			// dgv
			// 
			this.dgvSelect = new System.Windows.Forms.DataGridView();
			this.dgvSelect.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgvSelect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvSelect.Name = "dgvSelect";
			this.dgvSelect.TabIndex = 0;
			this.dgvSelect.Columns.Add("ValueType1Value", "ValueType1Value");
			this.dgvSelect.Columns.Add("DoesSomethingWithPerson", "DoesSomethingWithPerson");
			this.dgvSelect.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.dgvSelect.Height = 75;
			// 
			// pnlDisplay
			// 
			this.pnlDisplay = new System.Windows.Forms.Panel();
			this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlDisplay.AutoSize = true;
			this.pnlDisplay.Location = new System.Drawing.Point(0, 0);
			this.pnlDisplay.Name = "pnlDisplay";
			this.pnlDisplay.AutoScroll = true;
			this.pnlDisplay.TabIndex = 1;
			this.pnlDisplay.Controls.Add(this.btnSelect);
			// 
			// this
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(this.pnlDisplay);
			this.Controls.Add(this.dgvSelect);
			this.Controls.Add(this.lblSelect);
			this.Name = "icSelectValueType1InputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvSelect).EndInit();
			this.ResumeLayout(false);
			this.dgvCurrentObject.Height = this.dgvSelect.Height;
			this.pnlDisplay.Controls.Add(this.lblCurrentObject);
			this.pnlDisplay.Controls.Add(this.cbxSelectionMode);
			this.pnlDisplay.Controls.Add(this.lblSelectionMode);

			if (this.cbxSelectionMode.Items.Count > 0)
			{
				this.cbxSelectionMode.SelectedIndex = 0;
			}
		}
		#endregion // InitializeComponent method
	}
	#endregion // Select_ValueType1Core_InputControl
}
