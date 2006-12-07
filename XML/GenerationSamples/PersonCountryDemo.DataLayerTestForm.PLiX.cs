namespace PersonCountryDemo
{
	public partial class PersonCountryDemoTester : System.Windows.Forms.Form
	{
		/// <summary>The main entry point for the application.</summary>
		public static void Main()
		{
			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			System.Windows.Forms.Application.Run(new PersonCountryDemoTester());
		}
		public PersonCountryDemoTester()
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
			this.Text = "NORMA:  PersonCountryDemo Data Test Form";
			this.MasterTabControl.Size = new System.Drawing.Size(540, 520);
			this.Controls.Add(MasterTabControl);
			this.Size = new System.Drawing.Size(550, 550);

			this.MasterTabControl.TabPages.Add("Country", "Country");
			CountryCore_InputControl icCountryCore = new CountryCore_InputControl();
			icCountryCore.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MasterTabControl.TabPages["Country"].Controls.Add(icCountryCore);

			this.MasterTabControl.TabPages.Add("Person", "Person");
			PersonCore_InputControl icPersonCore = new PersonCore_InputControl();
			icPersonCore.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MasterTabControl.TabPages["Person"].Controls.Add(icPersonCore);
		}
		#endregion // InitializeComponent method
	}
}
