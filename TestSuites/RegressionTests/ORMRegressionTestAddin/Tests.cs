using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using EnvDTE80;
using System.ComponentModel;
using Neumont.Tools.ORM.ObjectModel;
using Accessibility;
using System.IO;
using NUnitCategory = NUnit.Framework.CategoryAttribute;
using NUnit.Framework;
using Neumont.Tools.Modeling.Design;

namespace ORMRegressionTestAddin
{
	public class Test
	{
		public static void RunTests(DTE2 DTE)
		{
			// AutomationTestSample.RolePlayerRequiredErrorTests.Test1a(DTE);
			// AutomationTestSample.ToolboxTests.TestActivateAddToolboxItem(DTE);
			AutomationTestSample.TestVisualStudio.TestLaunchVS8();
		}
	}
}