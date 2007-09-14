namespace BlogDemo
{
	#region BlogCommentCore_InputControl
	public partial class BlogCommentCore_InputControl
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
			Collection_BlogCommentCore_InputControl icCollection = new Collection_BlogCommentCore_InputControl();
			icCollection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Collection"].Controls.Add(icCollection);
			this.actionTabs.TabPages.Add("Create", "Create");
			Create_BlogCommentCore_InputControl icCreate = new Create_BlogCommentCore_InputControl();
			icCreate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Create"].Controls.Add(icCreate);
			this.actionTabs.TabPages.Add("Select", "Select and Edit");
			Select_BlogCommentCore_InputControl icSelect = new Select_BlogCommentCore_InputControl();
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
	#endregion // BlogCommentCore_InputControl
	#region Create_BlogCommentCore_InputControl
	public partial class Create_BlogCommentCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCreate;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.DataGridView dgvCreate;
		private System.Windows.Forms.Panel pnlDisplay;
		private BlogDemoContext.ConnectionDelegate Create_BlogComment_connect;
		private IBlogDemoContext testVar;
		private BlogComment abstractTypeVar;
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
			this.Create_BlogComment_connect = new BlogDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new BlogDemoContext(Create_BlogComment_connect);
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
			this.lblCreate.Text = "Enter data to Create BlogComment by:";
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
			this.dgvCreate.Columns.Add("parentEntryId", "parentEntryId");
			this.dgvCreate.Columns.Add("BlogEntry", "BlogEntry");
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
			this.Name = "icCreateBlogCommentInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCreate).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Create_BlogCommentCore_InputControl
	#region Collection_BlogCommentCore_InputControl
	public partial class Collection_BlogCommentCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCollection;
		private System.Windows.Forms.Button btnCollection;
		private System.Windows.Forms.DataGridView dgvCollection;
		private System.Windows.Forms.Panel pnlDisplay;
		private BlogDemoContext.ConnectionDelegate Collection_BlogComment_connect;
		private IBlogDemoContext testVar;
		private BlogComment abstractTypeVar;
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
			this.Collection_BlogComment_connect = new BlogDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new BlogDemoContext(Collection_BlogComment_connect);
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
			this.lblCollection.Text = "Click the Collection button to refresh the list of BlogComment records.";
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
			this.dgvCollection.Columns.Add("parentEntryId", "parentEntryId");
			this.dgvCollection.Columns.Add("BlogEntry", "BlogEntry");
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
			this.Name = "icCollectionBlogCommentInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCollection).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Collection_BlogCommentCore_InputControl
	#region SelectBlogCommentCore_InputControl
	public partial class Select_BlogCommentCore_InputControl
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
		private BlogDemoContext.ConnectionDelegate Select_BlogComment_connect;
		private IBlogDemoContext testVar;
		private BlogComment abstractTypeVar;
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
			this.lblCurrentObject.Text = "There is no selected BlogComment.";
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
			this.dgvCurrentObject.Columns.Add("parentEntryId", "parentEntryId");
			this.dgvCurrentObject.Columns["parentEntryId"].Visible = false;
			this.dgvCurrentObject.Columns.Add("BlogEntry", "BlogEntry");
			this.dgvCurrentObject.Columns["BlogEntry"].Visible = false;
			this.dgvCurrentObject.Visible = false;
			this.dgvCurrentObject.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvCurrentObject_CellBeginEdit);
			this.Controls.Add(this.pnlSave);
			this.Controls.Add(this.dgvCurrentObject);
			// 
			// connect
			// 
			this.Select_BlogComment_connect = new BlogDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new BlogDemoContext(Select_BlogComment_connect);
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
			this.lblSelect.Text = "Enter data to Select BlogComment by:";
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
			this.dgvSelect.Columns.Add("parentEntryId", "parentEntryId");
			this.dgvSelect.Columns.Add("BlogEntry", "BlogEntry");
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
			this.Name = "icSelectBlogCommentInputControl";
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
	#endregion // Select_BlogCommentCore_InputControl
	#region BlogEntryCore_InputControl
	public partial class BlogEntryCore_InputControl
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
			Collection_BlogEntryCore_InputControl icCollection = new Collection_BlogEntryCore_InputControl();
			icCollection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Collection"].Controls.Add(icCollection);
			this.actionTabs.TabPages.Add("Create", "Create");
			Create_BlogEntryCore_InputControl icCreate = new Create_BlogEntryCore_InputControl();
			icCreate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Create"].Controls.Add(icCreate);
			this.actionTabs.TabPages.Add("Select", "Select and Edit");
			Select_BlogEntryCore_InputControl icSelect = new Select_BlogEntryCore_InputControl();
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
	#endregion // BlogEntryCore_InputControl
	#region Create_BlogEntryCore_InputControl
	public partial class Create_BlogEntryCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCreate;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.DataGridView dgvCreate;
		private System.Windows.Forms.Panel pnlDisplay;
		private BlogDemoContext.ConnectionDelegate Create_BlogEntry_connect;
		private IBlogDemoContext testVar;
		private BlogEntry abstractTypeVar;
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
			this.Create_BlogEntry_connect = new BlogDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new BlogDemoContext(Create_BlogEntry_connect);
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
			this.lblCreate.Text = "Enter data to Create BlogEntry by:";
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
			this.dgvCreate.Columns.Add("BlogEntry_Id", "BlogEntry_Id");
			this.dgvCreate.Columns.Add("entryTitle", "entryTitle");
			this.dgvCreate.Columns.Add("entryBody", "entryBody");
			this.dgvCreate.Columns.Add("postedDate_MDYValue", "postedDate_MDYValue");
			this.dgvCreate.Columns.Add("userId", "userId");
			this.dgvCreate.Columns.Add("BlogComment", "BlogComment");
			this.dgvCreate.Columns.Add("NonCommentEntry", "NonCommentEntry");
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
			this.Name = "icCreateBlogEntryInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCreate).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Create_BlogEntryCore_InputControl
	#region Collection_BlogEntryCore_InputControl
	public partial class Collection_BlogEntryCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCollection;
		private System.Windows.Forms.Button btnCollection;
		private System.Windows.Forms.DataGridView dgvCollection;
		private System.Windows.Forms.Panel pnlDisplay;
		private BlogDemoContext.ConnectionDelegate Collection_BlogEntry_connect;
		private IBlogDemoContext testVar;
		private BlogEntry abstractTypeVar;
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
			this.Collection_BlogEntry_connect = new BlogDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new BlogDemoContext(Collection_BlogEntry_connect);
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
			this.lblCollection.Text = "Click the Collection button to refresh the list of BlogEntry records.";
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
			this.dgvCollection.Columns.Add("BlogEntry_Id", "BlogEntry_Id");
			this.dgvCollection.Columns.Add("entryTitle", "entryTitle");
			this.dgvCollection.Columns.Add("entryBody", "entryBody");
			this.dgvCollection.Columns.Add("postedDate_MDYValue", "postedDate_MDYValue");
			this.dgvCollection.Columns.Add("userId", "userId");
			this.dgvCollection.Columns.Add("BlogComment", "BlogComment");
			this.dgvCollection.Columns.Add("NonCommentEntry", "NonCommentEntry");
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
			this.Name = "icCollectionBlogEntryInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCollection).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Collection_BlogEntryCore_InputControl
	#region SelectBlogEntryCore_InputControl
	public partial class Select_BlogEntryCore_InputControl
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
		private BlogDemoContext.ConnectionDelegate Select_BlogEntry_connect;
		private IBlogDemoContext testVar;
		private BlogEntry abstractTypeVar;
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
			this.cbxSelectionMode.Items.Add("BlogEntry_Id");
			// 
			// lblCurrentObject
			// 
			this.lblCurrentObject = new System.Windows.Forms.Label();
			this.lblCurrentObject.Location = new System.Drawing.Point(0, 45);
			this.lblCurrentObject.Width = 300;
			this.lblCurrentObject.Name = "lblCurrentObject";
			this.lblCurrentObject.Text = "There is no selected BlogEntry.";
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
			this.dgvCurrentObject.Columns.Add("BlogEntry_Id", "BlogEntry_Id");
			this.dgvCurrentObject.Columns["BlogEntry_Id"].Visible = false;
			this.dgvCurrentObject.Columns.Add("entryTitle", "entryTitle");
			this.dgvCurrentObject.Columns["entryTitle"].Visible = false;
			this.dgvCurrentObject.Columns.Add("entryBody", "entryBody");
			this.dgvCurrentObject.Columns["entryBody"].Visible = false;
			this.dgvCurrentObject.Columns.Add("postedDate_MDYValue", "postedDate_MDYValue");
			this.dgvCurrentObject.Columns["postedDate_MDYValue"].Visible = false;
			this.dgvCurrentObject.Columns.Add("userId", "userId");
			this.dgvCurrentObject.Columns["userId"].Visible = false;
			this.dgvCurrentObject.Columns.Add("BlogComment", "BlogComment");
			this.dgvCurrentObject.Columns["BlogComment"].Visible = false;
			this.dgvCurrentObject.Columns.Add("NonCommentEntry", "NonCommentEntry");
			this.dgvCurrentObject.Columns["NonCommentEntry"].Visible = false;
			this.dgvCurrentObject.Visible = false;
			this.dgvCurrentObject.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvCurrentObject_CellBeginEdit);
			this.Controls.Add(this.pnlSave);
			this.Controls.Add(this.dgvCurrentObject);
			// 
			// connect
			// 
			this.Select_BlogEntry_connect = new BlogDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new BlogDemoContext(Select_BlogEntry_connect);
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
			this.lblSelect.Text = "Enter data to Select BlogEntry by:";
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
			this.dgvSelect.Columns.Add("BlogEntry_Id", "BlogEntry_Id");
			this.dgvSelect.Columns.Add("entryTitle", "entryTitle");
			this.dgvSelect.Columns.Add("entryBody", "entryBody");
			this.dgvSelect.Columns.Add("postedDate_MDYValue", "postedDate_MDYValue");
			this.dgvSelect.Columns.Add("userId", "userId");
			this.dgvSelect.Columns.Add("BlogComment", "BlogComment");
			this.dgvSelect.Columns.Add("NonCommentEntry", "NonCommentEntry");
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
			this.Name = "icSelectBlogEntryInputControl";
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
	#endregion // Select_BlogEntryCore_InputControl
	#region BlogEntryLabelCore_InputControl
	public partial class BlogEntryLabelCore_InputControl
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
			Collection_BlogEntryLabelCore_InputControl icCollection = new Collection_BlogEntryLabelCore_InputControl();
			icCollection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Collection"].Controls.Add(icCollection);
			this.actionTabs.TabPages.Add("Create", "Create");
			Create_BlogEntryLabelCore_InputControl icCreate = new Create_BlogEntryLabelCore_InputControl();
			icCreate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Create"].Controls.Add(icCreate);
			this.actionTabs.TabPages.Add("Select", "Select and Edit");
			Select_BlogEntryLabelCore_InputControl icSelect = new Select_BlogEntryLabelCore_InputControl();
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
	#endregion // BlogEntryLabelCore_InputControl
	#region Create_BlogEntryLabelCore_InputControl
	public partial class Create_BlogEntryLabelCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCreate;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.DataGridView dgvCreate;
		private System.Windows.Forms.Panel pnlDisplay;
		private BlogDemoContext.ConnectionDelegate Create_BlogEntryLabel_connect;
		private IBlogDemoContext testVar;
		private BlogEntryLabel abstractTypeVar;
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
			this.Create_BlogEntryLabel_connect = new BlogDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new BlogDemoContext(Create_BlogEntryLabel_connect);
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
			this.lblCreate.Text = "Enter data to Create BlogEntryLabel by:";
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
			this.dgvCreate.Columns.Add("blogEntryId(BlogEntry_Id)", "blogEntryId(BlogEntry_Id)");
			this.dgvCreate.Columns.Add("blogLabelId(BlogLabel_Id)", "blogLabelId(BlogLabel_Id)");
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
			this.Name = "icCreateBlogEntryLabelInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCreate).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Create_BlogEntryLabelCore_InputControl
	#region Collection_BlogEntryLabelCore_InputControl
	public partial class Collection_BlogEntryLabelCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCollection;
		private System.Windows.Forms.Button btnCollection;
		private System.Windows.Forms.DataGridView dgvCollection;
		private System.Windows.Forms.Panel pnlDisplay;
		private BlogDemoContext.ConnectionDelegate Collection_BlogEntryLabel_connect;
		private IBlogDemoContext testVar;
		private BlogEntryLabel abstractTypeVar;
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
			this.Collection_BlogEntryLabel_connect = new BlogDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new BlogDemoContext(Collection_BlogEntryLabel_connect);
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
			this.lblCollection.Text = "Click the Collection button to refresh the list of BlogEntryLabel records.";
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
			this.dgvCollection.Columns.Add("blogEntryId(BlogEntry_Id)", "blogEntryId(BlogEntry_Id)");
			this.dgvCollection.Columns.Add("blogLabelId(BlogLabel_Id)", "blogLabelId(BlogLabel_Id)");
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
			this.Name = "icCollectionBlogEntryLabelInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCollection).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Collection_BlogEntryLabelCore_InputControl
	#region SelectBlogEntryLabelCore_InputControl
	public partial class Select_BlogEntryLabelCore_InputControl
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
		private BlogDemoContext.ConnectionDelegate Select_BlogEntryLabel_connect;
		private IBlogDemoContext testVar;
		private BlogEntryLabel abstractTypeVar;
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
			this.cbxSelectionMode.Items.Add("InternalUniquenessConstraint20");
			// 
			// lblCurrentObject
			// 
			this.lblCurrentObject = new System.Windows.Forms.Label();
			this.lblCurrentObject.Location = new System.Drawing.Point(0, 45);
			this.lblCurrentObject.Width = 300;
			this.lblCurrentObject.Name = "lblCurrentObject";
			this.lblCurrentObject.Text = "There is no selected BlogEntryLabel.";
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
			this.dgvCurrentObject.Columns.Add("blogEntryId(BlogEntry_Id)", "blogEntryId(BlogEntry_Id)");
			this.dgvCurrentObject.Columns["blogEntryId(BlogEntry_Id)"].Visible = false;
			this.dgvCurrentObject.Columns.Add("blogLabelId(BlogLabel_Id)", "blogLabelId(BlogLabel_Id)");
			this.dgvCurrentObject.Columns["blogLabelId(BlogLabel_Id)"].Visible = false;
			this.dgvCurrentObject.Visible = false;
			this.dgvCurrentObject.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvCurrentObject_CellBeginEdit);
			this.Controls.Add(this.pnlSave);
			this.Controls.Add(this.dgvCurrentObject);
			// 
			// connect
			// 
			this.Select_BlogEntryLabel_connect = new BlogDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new BlogDemoContext(Select_BlogEntryLabel_connect);
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
			this.lblSelect.Text = "Enter data to Select BlogEntryLabel by:";
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
			this.dgvSelect.Columns.Add("blogEntryId(BlogEntry_Id)", "blogEntryId(BlogEntry_Id)");
			this.dgvSelect.Columns.Add("blogLabelId(BlogLabel_Id)", "blogLabelId(BlogLabel_Id)");
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
			this.Name = "icSelectBlogEntryLabelInputControl";
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
	#endregion // Select_BlogEntryLabelCore_InputControl
	#region BlogLabelCore_InputControl
	public partial class BlogLabelCore_InputControl
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
			Collection_BlogLabelCore_InputControl icCollection = new Collection_BlogLabelCore_InputControl();
			icCollection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Collection"].Controls.Add(icCollection);
			this.actionTabs.TabPages.Add("Create", "Create");
			Create_BlogLabelCore_InputControl icCreate = new Create_BlogLabelCore_InputControl();
			icCreate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Create"].Controls.Add(icCreate);
			this.actionTabs.TabPages.Add("Select", "Select and Edit");
			Select_BlogLabelCore_InputControl icSelect = new Select_BlogLabelCore_InputControl();
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
	#endregion // BlogLabelCore_InputControl
	#region Create_BlogLabelCore_InputControl
	public partial class Create_BlogLabelCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCreate;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.DataGridView dgvCreate;
		private System.Windows.Forms.Panel pnlDisplay;
		private BlogDemoContext.ConnectionDelegate Create_BlogLabel_connect;
		private IBlogDemoContext testVar;
		private BlogLabel abstractTypeVar;
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
			this.Create_BlogLabel_connect = new BlogDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new BlogDemoContext(Create_BlogLabel_connect);
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
			this.lblCreate.Text = "Enter data to Create BlogLabel by:";
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
			this.dgvCreate.Columns.Add("title", "title");
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
			this.Name = "icCreateBlogLabelInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCreate).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Create_BlogLabelCore_InputControl
	#region Collection_BlogLabelCore_InputControl
	public partial class Collection_BlogLabelCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCollection;
		private System.Windows.Forms.Button btnCollection;
		private System.Windows.Forms.DataGridView dgvCollection;
		private System.Windows.Forms.Panel pnlDisplay;
		private BlogDemoContext.ConnectionDelegate Collection_BlogLabel_connect;
		private IBlogDemoContext testVar;
		private BlogLabel abstractTypeVar;
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
			this.Collection_BlogLabel_connect = new BlogDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new BlogDemoContext(Collection_BlogLabel_connect);
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
			this.lblCollection.Text = "Click the Collection button to refresh the list of BlogLabel records.";
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
			this.dgvCollection.Columns.Add("BlogLabel_Id", "BlogLabel_Id");
			this.dgvCollection.Columns.Add("title", "title");
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
			this.Name = "icCollectionBlogLabelInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCollection).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Collection_BlogLabelCore_InputControl
	#region SelectBlogLabelCore_InputControl
	public partial class Select_BlogLabelCore_InputControl
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
		private BlogDemoContext.ConnectionDelegate Select_BlogLabel_connect;
		private IBlogDemoContext testVar;
		private BlogLabel abstractTypeVar;
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
			this.cbxSelectionMode.Items.Add("BlogLabel_Id");
			// 
			// lblCurrentObject
			// 
			this.lblCurrentObject = new System.Windows.Forms.Label();
			this.lblCurrentObject.Location = new System.Drawing.Point(0, 45);
			this.lblCurrentObject.Width = 300;
			this.lblCurrentObject.Name = "lblCurrentObject";
			this.lblCurrentObject.Text = "There is no selected BlogLabel.";
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
			this.dgvCurrentObject.Columns.Add("BlogLabel_Id", "BlogLabel_Id");
			this.dgvCurrentObject.Columns["BlogLabel_Id"].Visible = false;
			this.dgvCurrentObject.Columns.Add("title", "title");
			this.dgvCurrentObject.Columns["title"].Visible = false;
			this.dgvCurrentObject.Visible = false;
			this.dgvCurrentObject.Columns["BlogLabel_Id"].ReadOnly = true;
			this.dgvCurrentObject.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvCurrentObject_CellBeginEdit);
			this.Controls.Add(this.pnlSave);
			this.Controls.Add(this.dgvCurrentObject);
			// 
			// connect
			// 
			this.Select_BlogLabel_connect = new BlogDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new BlogDemoContext(Select_BlogLabel_connect);
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
			this.lblSelect.Text = "Enter data to Select BlogLabel by:";
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
			this.dgvSelect.Columns.Add("BlogLabel_Id", "BlogLabel_Id");
			this.dgvSelect.Columns.Add("title", "title");
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
			this.Name = "icSelectBlogLabelInputControl";
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
	#endregion // Select_BlogLabelCore_InputControl
	#region NonCommentEntryCore_InputControl
	public partial class NonCommentEntryCore_InputControl
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
			Collection_NonCommentEntryCore_InputControl icCollection = new Collection_NonCommentEntryCore_InputControl();
			icCollection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Collection"].Controls.Add(icCollection);
			this.actionTabs.TabPages.Add("Create", "Create");
			Create_NonCommentEntryCore_InputControl icCreate = new Create_NonCommentEntryCore_InputControl();
			icCreate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Create"].Controls.Add(icCreate);
			this.actionTabs.TabPages.Add("Select", "Select and Edit");
			Select_NonCommentEntryCore_InputControl icSelect = new Select_NonCommentEntryCore_InputControl();
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
	#endregion // NonCommentEntryCore_InputControl
	#region Create_NonCommentEntryCore_InputControl
	public partial class Create_NonCommentEntryCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCreate;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.DataGridView dgvCreate;
		private System.Windows.Forms.Panel pnlDisplay;
		private BlogDemoContext.ConnectionDelegate Create_NonCommentEntry_connect;
		private IBlogDemoContext testVar;
		private NonCommentEntry abstractTypeVar;
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
			this.Create_NonCommentEntry_connect = new BlogDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new BlogDemoContext(Create_NonCommentEntry_connect);
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
			this.lblCreate.Text = "Enter data to Create NonCommentEntry by:";
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
			this.dgvCreate.Columns.Add("BlogEntry", "BlogEntry");
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
			this.Name = "icCreateNonCommentEntryInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCreate).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Create_NonCommentEntryCore_InputControl
	#region Collection_NonCommentEntryCore_InputControl
	public partial class Collection_NonCommentEntryCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCollection;
		private System.Windows.Forms.Button btnCollection;
		private System.Windows.Forms.DataGridView dgvCollection;
		private System.Windows.Forms.Panel pnlDisplay;
		private BlogDemoContext.ConnectionDelegate Collection_NonCommentEntry_connect;
		private IBlogDemoContext testVar;
		private NonCommentEntry abstractTypeVar;
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
			this.Collection_NonCommentEntry_connect = new BlogDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new BlogDemoContext(Collection_NonCommentEntry_connect);
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
			this.lblCollection.Text = "Click the Collection button to refresh the list of NonCommentEntry records.";
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
			this.dgvCollection.Columns.Add("BlogEntry", "BlogEntry");
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
			this.Name = "icCollectionNonCommentEntryInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCollection).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Collection_NonCommentEntryCore_InputControl
	#region SelectNonCommentEntryCore_InputControl
	public partial class Select_NonCommentEntryCore_InputControl
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
		private BlogDemoContext.ConnectionDelegate Select_NonCommentEntry_connect;
		private IBlogDemoContext testVar;
		private NonCommentEntry abstractTypeVar;
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
			this.lblCurrentObject.Text = "There is no selected NonCommentEntry.";
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
			this.dgvCurrentObject.Columns.Add("BlogEntry", "BlogEntry");
			this.dgvCurrentObject.Columns["BlogEntry"].Visible = false;
			this.dgvCurrentObject.Visible = false;
			this.dgvCurrentObject.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvCurrentObject_CellBeginEdit);
			this.Controls.Add(this.pnlSave);
			this.Controls.Add(this.dgvCurrentObject);
			// 
			// connect
			// 
			this.Select_NonCommentEntry_connect = new BlogDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new BlogDemoContext(Select_NonCommentEntry_connect);
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
			this.lblSelect.Text = "Enter data to Select NonCommentEntry by:";
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
			this.dgvSelect.Columns.Add("BlogEntry", "BlogEntry");
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
			this.Name = "icSelectNonCommentEntryInputControl";
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
	#endregion // Select_NonCommentEntryCore_InputControl
	#region UserCore_InputControl
	public partial class UserCore_InputControl
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
			Collection_UserCore_InputControl icCollection = new Collection_UserCore_InputControl();
			icCollection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Collection"].Controls.Add(icCollection);
			this.actionTabs.TabPages.Add("Create", "Create");
			Create_UserCore_InputControl icCreate = new Create_UserCore_InputControl();
			icCreate.Dock = System.Windows.Forms.DockStyle.Fill;
			this.actionTabs.TabPages["Create"].Controls.Add(icCreate);
			this.actionTabs.TabPages.Add("Select", "Select and Edit");
			Select_UserCore_InputControl icSelect = new Select_UserCore_InputControl();
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
	#endregion // UserCore_InputControl
	#region Create_UserCore_InputControl
	public partial class Create_UserCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCreate;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.DataGridView dgvCreate;
		private System.Windows.Forms.Panel pnlDisplay;
		private BlogDemoContext.ConnectionDelegate Create_User_connect;
		private IBlogDemoContext testVar;
		private User abstractTypeVar;
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
			this.Create_User_connect = new BlogDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new BlogDemoContext(Create_User_connect);
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
			this.lblCreate.Text = "Enter data to Create User by:";
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
			this.dgvCreate.Columns.Add("firstName", "firstName");
			this.dgvCreate.Columns.Add("lastName", "lastName");
			this.dgvCreate.Columns.Add("username", "username");
			this.dgvCreate.Columns.Add("password", "password");
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
			this.Name = "icCreateUserInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCreate).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Create_UserCore_InputControl
	#region Collection_UserCore_InputControl
	public partial class Collection_UserCore_InputControl
	{
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblCollection;
		private System.Windows.Forms.Button btnCollection;
		private System.Windows.Forms.DataGridView dgvCollection;
		private System.Windows.Forms.Panel pnlDisplay;
		private BlogDemoContext.ConnectionDelegate Collection_User_connect;
		private IBlogDemoContext testVar;
		private User abstractTypeVar;
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
			this.Collection_User_connect = new BlogDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new BlogDemoContext(Collection_User_connect);
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
			this.lblCollection.Text = "Click the Collection button to refresh the list of User records.";
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
			this.dgvCollection.Columns.Add("firstName", "firstName");
			this.dgvCollection.Columns.Add("lastName", "lastName");
			this.dgvCollection.Columns.Add("username", "username");
			this.dgvCollection.Columns.Add("password", "password");
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
			this.Name = "icCollectionUserInputControl";
			this.Size = new System.Drawing.Size(530, 490);
			((System.ComponentModel.ISupportInitialize)this.dgvCollection).EndInit();
			this.ResumeLayout(false);
		}
		#endregion // InitializeComponent method
	}
	#endregion // Collection_UserCore_InputControl
	#region SelectUserCore_InputControl
	public partial class Select_UserCore_InputControl
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
		private BlogDemoContext.ConnectionDelegate Select_User_connect;
		private IBlogDemoContext testVar;
		private User abstractTypeVar;
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
			this.cbxSelectionMode.Items.Add("ExternalUniquenessConstraint1");
			// 
			// lblCurrentObject
			// 
			this.lblCurrentObject = new System.Windows.Forms.Label();
			this.lblCurrentObject.Location = new System.Drawing.Point(0, 45);
			this.lblCurrentObject.Width = 300;
			this.lblCurrentObject.Name = "lblCurrentObject";
			this.lblCurrentObject.Text = "There is no selected User.";
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
			this.dgvCurrentObject.Columns.Add("firstName", "firstName");
			this.dgvCurrentObject.Columns["firstName"].Visible = false;
			this.dgvCurrentObject.Columns.Add("lastName", "lastName");
			this.dgvCurrentObject.Columns["lastName"].Visible = false;
			this.dgvCurrentObject.Columns.Add("username", "username");
			this.dgvCurrentObject.Columns["username"].Visible = false;
			this.dgvCurrentObject.Columns.Add("password", "password");
			this.dgvCurrentObject.Columns["password"].Visible = false;
			this.dgvCurrentObject.Visible = false;
			this.dgvCurrentObject.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvCurrentObject_CellBeginEdit);
			this.Controls.Add(this.pnlSave);
			this.Controls.Add(this.dgvCurrentObject);
			// 
			// connect
			// 
			this.Select_User_connect = new BlogDemoContext.ConnectionDelegate(GetConnection);
			// 
			// testVar
			// 
			this.testVar = new BlogDemoContext(Select_User_connect);
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
			this.lblSelect.Text = "Enter data to Select User by:";
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
			this.dgvSelect.Columns.Add("firstName", "firstName");
			this.dgvSelect.Columns.Add("lastName", "lastName");
			this.dgvSelect.Columns.Add("username", "username");
			this.dgvSelect.Columns.Add("password", "password");
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
			this.Name = "icSelectUserInputControl";
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
	#endregion // Select_UserCore_InputControl
}
