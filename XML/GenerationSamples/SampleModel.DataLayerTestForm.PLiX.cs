namespace SampleModel
{
	public partial class SampleModelTester : System.Windows.Forms.Form
	{
		/// <summary>The main entry point for the application.</summary>
		public static void Main()
		{
			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			System.Windows.Forms.Application.Run(new SampleModelTester());
		}
		public SampleModelTester()
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
			if (disposing && components != null)
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
			this.Text = "NORMA:  SampleModel Data Test Form";
			this.MasterTabControl.Size = new System.Drawing.Size(540, 520);
			this.Controls.Add(MasterTabControl);
			this.Size = new System.Drawing.Size(550, 550);

			this.MasterTabControl.TabPages.Add("Death", "Death");
			DeathCore_InputControl icDeathCore = new DeathCore_InputControl();
			icDeathCore.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MasterTabControl.TabPages["Death"].Controls.Add(icDeathCore);

			this.MasterTabControl.TabPages.Add("PersonBoughtCarFromPersonOnDate", "PersonBoughtCarFromPersonOnDate");
			PersonBoughtCarFromPersonOnDateCore_InputControl icPersonBoughtCarFromPersonOnDateCore = new PersonBoughtCarFromPersonOnDateCore_InputControl();
			icPersonBoughtCarFromPersonOnDateCore.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MasterTabControl.TabPages["PersonBoughtCarFromPersonOnDate"].Controls.Add(icPersonBoughtCarFromPersonOnDateCore);

			this.MasterTabControl.TabPages.Add("Person", "Person");
			PersonCore_InputControl icPersonCore = new PersonCore_InputControl();
			icPersonCore.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MasterTabControl.TabPages["Person"].Controls.Add(icPersonCore);

			this.MasterTabControl.TabPages.Add("PersonDrivesCar", "PersonDrivesCar");
			PersonDrivesCarCore_InputControl icPersonDrivesCarCore = new PersonDrivesCarCore_InputControl();
			icPersonDrivesCarCore.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MasterTabControl.TabPages["PersonDrivesCar"].Controls.Add(icPersonDrivesCarCore);

			this.MasterTabControl.TabPages.Add("PersonHasNickName", "PersonHasNickName");
			PersonHasNickNameCore_InputControl icPersonHasNickNameCore = new PersonHasNickNameCore_InputControl();
			icPersonHasNickNameCore.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MasterTabControl.TabPages["PersonHasNickName"].Controls.Add(icPersonHasNickNameCore);

			this.MasterTabControl.TabPages.Add("Review", "Review");
			ReviewCore_InputControl icReviewCore = new ReviewCore_InputControl();
			icReviewCore.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MasterTabControl.TabPages["Review"].Controls.Add(icReviewCore);

			this.MasterTabControl.TabPages.Add("Task", "Task");
			TaskCore_InputControl icTaskCore = new TaskCore_InputControl();
			icTaskCore.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MasterTabControl.TabPages["Task"].Controls.Add(icTaskCore);

			this.MasterTabControl.TabPages.Add("ValueType1", "ValueType1");
			ValueType1Core_InputControl icValueType1Core = new ValueType1Core_InputControl();
			icValueType1Core.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MasterTabControl.TabPages["ValueType1"].Controls.Add(icValueType1Core);
		}
		#endregion // InitializeComponent method
	}
}
