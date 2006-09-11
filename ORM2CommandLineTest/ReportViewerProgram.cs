using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Neumont.Tools.ORM.SDK.TestReportViewer
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new ReportViewer(args));
		}


	}
}