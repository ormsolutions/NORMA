using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Modeling;
using System.ComponentModel;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using System.Windows.Forms;
using System.IO;
using ORMSolutions.ORMArchitectSDK.TestEngine;
using NUnitCategory = NUnit.Framework.CategoryAttribute;
using NUnit.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using TestEngine = ORMSolutions.ORMArchitectSDK.TestEngine;
using System.Diagnostics;
using ORMRegressionTestAddin;

namespace AutomationTestSample
{
	/// <summary>
	/// Test plan for feature: RolePlayerRequiredError
	/// <remarks>
	/// ORM Definition: Each role in a fact type must be 
	/// associated with an ObjectType.
	/// </remarks>
	/// </summary>
	[CLSCompliant(false)]
	public class RolePlayerRequiredErrorTests
	{
		public static void Test1a(DTE2 DTE)
		{
			DTE.ItemOperations.NewFile("General\\Object-Role Modeling File", "ORMRolePlayerRequiredModel", "");

			ORMTestHooks testHooks = new ORMTestHooks(DTE);
			ORMTestWindow testWindow = testHooks.FindORMTestWindow(null);

			if (CommonTestHooks.ActivateToolboxItem(DTE, "ORM Designer", "Entity Type"))
			{
				SendKeys.SendWait("{Enter}");
			}

			if (CommonTestHooks.ActivateToolboxItem(DTE, "ORM Designer", "Entity Type"))
			{
				SendKeys.SendWait("{Enter}");
			}

			// Find the accesible object for the diagram
			AccessibleObject accDiagram = CommonTestHooks.FindAccessibleObject(
					testWindow.AccessibleObject,
					new AccessiblePathNode[] {
                            new AccessiblePathNode("ORMDiagram")
                        });

			// Find the accessible object for the newly added Entity Type
			AccessibleObject accEntityType = CommonTestHooks.FindAccessibleObject(
					accDiagram,
					new AccessiblePathNode[] {
                            new AccessiblePathNode("EntityType", 0)
                        });

			// Click on the accessible object for the newly created entity type.
			testWindow.ClickAccessibleObject(accEntityType);
		}
	}
}
