namespace BlogDemo
{
	public partial class BlogDemoTester : System.Windows.Forms.Form
	{
		/// <summary>The main entry point for the application.</summary>
		public static void Main()
		{
			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			System.Windows.Forms.Application.Run(new BlogDemoTester());
		}
		public BlogDemoTester()
		{
			this.InitializeComponent();
		}
		/// <summary>Required designer variable.</summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TabControl MasterTabControl;
		/// <summary>Clean up any resources being used.</summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}
		#region InitializeComponent method
		/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
		private void InitializeComponent()
		{
			this.MasterTabControl = new System.Windows.Forms.TabControl();
			this.MasterTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MasterTabControl.Multiline = true;
			this.components = new System.ComponentModel.Container();
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Text = "NORMA:  BlogDemo Data Test Form";
			this.MasterTabControl.Size = new System.Drawing.Size(540, 520);
			this.Controls.Add(MasterTabControl);
			this.Size = new System.Drawing.Size(550, 550);

			this.MasterTabControl.TabPages.Add("BlogComment", "BlogComment");
			BlogCommentCore_InputControl icBlogCommentCore = new BlogCommentCore_InputControl();
			icBlogCommentCore.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MasterTabControl.TabPages["BlogComment"].Controls.Add(icBlogCommentCore);

			this.MasterTabControl.TabPages.Add("BlogEntry", "BlogEntry");
			BlogEntryCore_InputControl icBlogEntryCore = new BlogEntryCore_InputControl();
			icBlogEntryCore.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MasterTabControl.TabPages["BlogEntry"].Controls.Add(icBlogEntryCore);

			this.MasterTabControl.TabPages.Add("BlogEntryLabel", "BlogEntryLabel");
			BlogEntryLabelCore_InputControl icBlogEntryLabelCore = new BlogEntryLabelCore_InputControl();
			icBlogEntryLabelCore.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MasterTabControl.TabPages["BlogEntryLabel"].Controls.Add(icBlogEntryLabelCore);

			this.MasterTabControl.TabPages.Add("BlogLabel", "BlogLabel");
			BlogLabelCore_InputControl icBlogLabelCore = new BlogLabelCore_InputControl();
			icBlogLabelCore.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MasterTabControl.TabPages["BlogLabel"].Controls.Add(icBlogLabelCore);

			this.MasterTabControl.TabPages.Add("NonCommentEntry", "NonCommentEntry");
			NonCommentEntryCore_InputControl icNonCommentEntryCore = new NonCommentEntryCore_InputControl();
			icNonCommentEntryCore.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MasterTabControl.TabPages["NonCommentEntry"].Controls.Add(icNonCommentEntryCore);

			this.MasterTabControl.TabPages.Add("User", "User");
			UserCore_InputControl icUserCore = new UserCore_InputControl();
			icUserCore.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MasterTabControl.TabPages["User"].Controls.Add(icUserCore);
		}
		#endregion // InitializeComponent method
	}
}
