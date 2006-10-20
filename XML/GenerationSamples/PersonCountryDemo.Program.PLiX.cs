using System;
using System.Collections.Generic;
using System.Windows.Forms;
namespace PersonCountryDemo
{
	public static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		public static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new PersonCountryDemoTester());
		}
	}
}
