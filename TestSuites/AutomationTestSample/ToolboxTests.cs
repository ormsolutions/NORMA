using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Modeling;
using System.ComponentModel;
using Neumont.Tools.ORM.ObjectModel;
using System.Windows.Forms;
using System.IO;
using Neumont.Tools.ORM.SDK.TestEngine;
using NUnitCategory = NUnit.Framework.CategoryAttribute;
using NUnit.Framework;
using Neumont.Tools.Modeling.Design;
using TestEngine = Neumont.Tools.ORM.SDK.TestEngine;
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
	[TestFixture]
	public class ToolboxTests
	{
		[SetUp]
		public void Initialize()
		{

		}

		[Test]
		public void TestActivateAddToolboxItem(DTE2 DTE)
		{
			DTE.ItemOperations.NewFile("General\\Object-Role Modeling File", "ORMRolePlayerRequiredModel", "");

			ORMTestHooks testHooks = new ORMTestHooks(DTE);
			ORMTestWindow testWindow = testHooks.FindORMTestWindow(null);

			// Activate the type and hit enter to add to the model
			CommonTestHooks.ActivateToolboxItem(DTE, "ORM Designer", "Entity Type");
			SendKeys.SendWait("{Enter}");

			// Activate the type and hit enter to add to the model
			CommonTestHooks.ActivateToolboxItem(DTE, "ORM Designer", "Value Type");
			SendKeys.SendWait("{Enter}");

			// Activate the type and hit enter to add to the model
			CommonTestHooks.ActivateToolboxItem(DTE, "ORM Designer", "Objectified Fact Type");
			SendKeys.SendWait("{Enter}");

			// Activate the type and hit enter to add to the model
			CommonTestHooks.ActivateToolboxItem(DTE, "ORM Designer", "Binary Fact Type");
			SendKeys.SendWait("{Enter}");

			// Activate the type and hit enter to add to the model
			CommonTestHooks.ActivateToolboxItem(DTE, "ORM Designer", "Ternary Fact Type");
			SendKeys.SendWait("{Enter}");

			// Activate the type and hit enter to add to the model
			CommonTestHooks.ActivateToolboxItem(DTE, "ORM Designer", "External Uniquess Constraint");
			SendKeys.SendWait("{Enter}");

			// Activate the type and hit enter to add to the model
			CommonTestHooks.ActivateToolboxItem(DTE, "ORM Designer", "Equality Constraint");
			SendKeys.SendWait("{Enter}");

			// Activate the type and hit enter to add to the model
			CommonTestHooks.ActivateToolboxItem(DTE, "ORM Designer", "Inclusive Or Constraint");
			SendKeys.SendWait("{Enter}");

			// Activate the type and hit enter to add to the model
			CommonTestHooks.ActivateToolboxItem(DTE, "ORM Designer", "Exclusion Constraint");
			SendKeys.SendWait("{Enter}");

			// Activate the type and hit enter to add to the model
			CommonTestHooks.ActivateToolboxItem(DTE, "ORM Designer", "Exclusive Or Constraint");
			SendKeys.SendWait("{Enter}");

			// Activate the type and hit enter to add to the model
			CommonTestHooks.ActivateToolboxItem(DTE, "ORM Designer", "Subset Constraint");
			SendKeys.SendWait("{Enter}");

			// Activate the type and hit enter to add to the model
			CommonTestHooks.ActivateToolboxItem(DTE, "ORM Designer", "Frequency Constraint");
			SendKeys.SendWait("{Enter}");

			// Activate the type and hit enter to add to the model
			CommonTestHooks.ActivateToolboxItem(DTE, "ORM Designer", "Ring Constraint");
			SendKeys.SendWait("{Enter}");

			// Activate the type and hit enter to add to the model
			CommonTestHooks.ActivateToolboxItem(DTE, "ORM Designer", "Model Note");
			SendKeys.SendWait("{Enter}");

			// Connectors can not be added to the model
			// This includes: Model Note Connector, Subtype Connector, Role Connector, as well 
			// as the Pointer.
		}
	}
}
