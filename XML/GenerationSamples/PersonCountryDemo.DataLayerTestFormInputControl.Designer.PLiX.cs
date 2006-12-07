namespace PersonCountryDemo
{
	#region CountryCore_InputControl
	public partial class CountryCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.TabControl actionTabs;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
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
			Collection_CountryCore_InputControl icCollection = new Collection_CountryCore_InputControl();
			icCollection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Collection"].Controls.Add(icCollection);
			this.actionTabs.TabPages.Add("Create", "Create");
			Create_CountryCore_InputControl icCreate = new Create_CountryCore_InputControl();
			icCreate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Create"].Controls.Add(icCreate);
			this.actionTabs.TabPages.Add("Select", "Select and Edit");
			Select_CountryCore_InputControl icSelect = new Select_CountryCore_InputControl();
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
	#endregion // CountryCore_InputControl
	#region Create_CountryCore_InputControl
	public partial class Create_CountryCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCreate;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.DataGridView dgvCreate;
		private System.Windows.Forms.Panel pnlDisplay;
		private PersonCountryDemoContext.ConnectionDelegate Create_Country_connect;
		private IPersonCountryDemoContext testVar;
		private Country abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
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
			this.Create_Country_connect = new PersonCountryDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new PersonCountryDemoContext(Create_Country_connect);
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
			this.lblCreate.Text = "Enter data to Create Country by:";
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
			this.dgvCreate.Columns.Add("Country_name", "Country_name");
			this.dgvCreate.Columns.Add("Region_Region_code", "Region_Region_code");
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
			this.Name = "icCreateCountryInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCreate).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Create_CountryCore_InputControl
	#region Collection_CountryCore_InputControl
	public partial class Collection_CountryCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCollection;
		private System.Windows.Forms.Button btnCollection;
		private System.Windows.Forms.DataGridView dgvCollection;
		private System.Windows.Forms.Panel pnlDisplay;
		private PersonCountryDemoContext.ConnectionDelegate Collection_Country_connect;
		private IPersonCountryDemoContext testVar;
		private Country abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
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
			this.Collection_Country_connect = new PersonCountryDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new PersonCountryDemoContext(Collection_Country_connect);
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
			this.lblCollection.Text = "Click the Collection button to refresh the list of Country records.";
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
			this.dgvCollection.Columns.Add("Country_name", "Country_name");
			this.dgvCollection.Columns.Add("Region_Region_code", "Region_Region_code");
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
			this.Name = "icCollectionCountryInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCollection).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Collection_CountryCore_InputControl
	#region SelectCountryCore_InputControl
	public partial class Select_CountryCore_InputControl
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
		private PersonCountryDemoContext.ConnectionDelegate Select_Country_connect;
		private IPersonCountryDemoContext testVar;
		private Country abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
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
			this.cbxSelectionMode.Items.Add("Country_name");
			// 
			// lblCurrentObject
			// 
			this.lblCurrentObject = new System.Windows.Forms.Label();
			this.lblCurrentObject.Location = new System.Drawing.Point(0, 45);
			this.lblCurrentObject.Width = 300;
			this.lblCurrentObject.Name = "lblCurrentObject";
			this.lblCurrentObject.Text = "There is no selected Country.";
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
			this.dgvCurrentObject.Columns.Add("Country_name", "Country_name");
			this.dgvCurrentObject.Columns["Country_name"].Visible = false;
			this.dgvCurrentObject.Columns.Add("Region_Region_code", "Region_Region_code");
			this.dgvCurrentObject.Columns["Region_Region_code"].Visible = false;
			this.dgvCurrentObject.Visible = false;
			this.dgvCurrentObject.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvCurrentObject_CellBeginEdit);
			this.Controls.Add(this.pnlSave);
			this.Controls.Add(this.dgvCurrentObject);
			// 
			// connect
			// 
			this.Select_Country_connect = new PersonCountryDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new PersonCountryDemoContext(Select_Country_connect);
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
			this.lblSelect.Text = "Enter data to Select Country by:";
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
			this.dgvSelect.Columns.Add("Country_name", "Country_name");
			this.dgvSelect.Columns.Add("Region_Region_code", "Region_Region_code");
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
			this.Name = "icSelectCountryInputControl";
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
	#endregion // Select_CountryCore_InputControl
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
			if (disposing && (components != null))
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
		private PersonCountryDemoContext.ConnectionDelegate Create_Person_connect;
		private IPersonCountryDemoContext testVar;
		private Person abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
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
			this.Create_Person_connect = new PersonCountryDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new PersonCountryDemoContext(Create_Person_connect);
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
			this.dgvCreate.Columns.Add("LastName", "LastName");
			this.dgvCreate.Columns.Add("FirstName", "FirstName");
			this.dgvCreate.Columns.Add("Title", "Title");
			this.dgvCreate.Columns.Add("Country", "Country");
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
		private PersonCountryDemoContext.ConnectionDelegate Collection_Person_connect;
		private IPersonCountryDemoContext testVar;
		private Person abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
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
			this.Collection_Person_connect = new PersonCountryDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new PersonCountryDemoContext(Collection_Person_connect);
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
			this.dgvCollection.Columns.Add("LastName", "LastName");
			this.dgvCollection.Columns.Add("FirstName", "FirstName");
			this.dgvCollection.Columns.Add("Title", "Title");
			this.dgvCollection.Columns.Add("Country", "Country");
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
		private PersonCountryDemoContext.ConnectionDelegate Select_Person_connect;
		private IPersonCountryDemoContext testVar;
		private Person abstractTypeVar;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
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
			this.dgvCurrentObject.Columns.Add("LastName", "LastName");
			this.dgvCurrentObject.Columns["LastName"].Visible = false;
			this.dgvCurrentObject.Columns.Add("FirstName", "FirstName");
			this.dgvCurrentObject.Columns["FirstName"].Visible = false;
			this.dgvCurrentObject.Columns.Add("Title", "Title");
			this.dgvCurrentObject.Columns["Title"].Visible = false;
			this.dgvCurrentObject.Columns.Add("Country", "Country");
			this.dgvCurrentObject.Columns["Country"].Visible = false;
			this.dgvCurrentObject.Visible = false;
			this.dgvCurrentObject.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvCurrentObject_CellBeginEdit);
			this.Controls.Add(this.pnlSave);
			this.Controls.Add(this.dgvCurrentObject);
			// 
			// connect
			// 
			this.Select_Person_connect = new PersonCountryDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new PersonCountryDemoContext(Select_Person_connect);
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
			this.dgvSelect.Columns.Add("LastName", "LastName");
			this.dgvSelect.Columns.Add("FirstName", "FirstName");
			this.dgvSelect.Columns.Add("Title", "Title");
			this.dgvSelect.Columns.Add("Country", "Country");
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
}
