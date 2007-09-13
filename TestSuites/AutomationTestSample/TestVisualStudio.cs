using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationTestSample
{
	/// <summary>
	/// Test cases for various functions of the 
	/// Visual Studio IDE environment.
	/// </summary>
	public class TestVisualStudio
	{
		public static void TestLaunchVS8()
		{
			System.Type t = System.Type.GetTypeFromProgID("VisualStudio.DTE.8.0");

			EnvDTE80.DTE2 dteObject = (EnvDTE80.DTE2)System.Activator.CreateInstance(t, true);

			dteObject.ItemOperations.NewFile("General\\Object-Role Modeling File", "ORMRolePlayerRequiredModel", "");

		}
	}
}