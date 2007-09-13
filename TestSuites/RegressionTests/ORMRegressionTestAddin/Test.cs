using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using EnvDTE80;
using TestHooks;
using Microsoft.VisualStudio.Modeling;
using System.ComponentModel;
using Neumont.Tools.ORM.ObjectModel;
using Accessibility;
using System.Windows.Forms;
using System.IO;

namespace Test
{
	/// <summary>
	/// Note: This version does not use AccessibilityObject.Select or ORMTestHooks.CreatePropertyDescriptor.
	/// SendKeys is use for all hot-keyed commands except ^Z, which had a lot of timing issues (I used Edit.Undo).
	/// Obviously, I'll be very lenient in grading timing issues with this and other commands.
	/// </summary>
	public class Test
	{
		/// <summary>
		/// Test body
		/// </summary>
		/// <param name="DTE">The environment application object</param>
		public static void RunTest(DTE2 DTE)
		{
			DTE.ItemOperations.OpenFile(Environment.GetEnvironmentVariable("ORMFile", EnvironmentVariableTarget.Process), "");
			// The test file has three binary facts in it. FactType1, FactType2, FactType3, and an entity type EntityType1. EntityType1 is connected
			// the first role of the three facts. FactType1 and FactType2 are mandatory and unique on the first role.

			// Standard test handling
			ORMTestHooks hooks = new ORMTestHooks(DTE);
			ORMTestWindow testWindow = hooks.FindORMTestWindow(null);

			using (StreamWriter writer = File.AppendText(Environment.GetEnvironmentVariable("TestLog", EnvironmentVariableTarget.Process)))
			{
				// Grab accessible objects for the diagram and the 4 roles we'll be connecting to
				AccessibleObject accDiagram = CommonTestHooks.FindAccessibleObject(
					testWindow.AccessibleObject,
					new AccessiblePathNode[]{
						new AccessiblePathNode("ORMDiagram")});

				AccessibleObject accRole1_1 = CommonTestHooks.FindAccessibleObject(
					accDiagram,
					new AccessiblePathNode[]{
						new AccessiblePathNode("FactType", "EntityType1A"),
						new AccessiblePathNode("Roles"),
						new AccessiblePathNode("Role", 0)});

				AccessibleObject accRole2_1 = CommonTestHooks.FindAccessibleObject(
					accDiagram,
					new AccessiblePathNode[]{
						new AccessiblePathNode("FactType", "EntityType1B"),
						new AccessiblePathNode("Roles"),
						new AccessiblePathNode("Role", 0)});

				AccessibleObject accRole3_1 = CommonTestHooks.FindAccessibleObject(
					accDiagram,
					new AccessiblePathNode[]{
						new AccessiblePathNode("FactType", "EntityType1C"),
						new AccessiblePathNode("Roles"),
						new AccessiblePathNode("Role", 0)});
				
				AccessibleObject accRole3_2 = CommonTestHooks.FindAccessibleObject(
					accDiagram,
					new AccessiblePathNode[]{
						new AccessiblePathNode("FactType", "EntityType1C"),
						new AccessiblePathNode("Roles"),
						new AccessiblePathNode("Role", 1)});

				// Add the equality constraint and get its accessible object
				testWindow.ActivateToolboxItem("Equality Constraint");
				testWindow.ClickAccessibleObject(accDiagram, 1, ClickLocation.UpperLeft, 50, 50);
				AccessibleObject accEquality = testWindow.GetSelectedAccessibleObjects()[0];
				EqualityConstraint equalityConstraint = (EqualityConstraint)testWindow.TranslateAccessibleObject(accEquality, false);

				bool scenarioPassed;
				// Scenario 1: Attach the two mandatory roles
				testWindow.ClickAccessibleObject(accRole1_1, 2);
				testWindow.ClickAccessibleObject(accRole2_1, 2);
				SendKeys.SendWait("{ESC}");
				scenarioPassed = null != equalityConstraint.EqualityImpliedByMandatoryError;
				writer.WriteLine((scenarioPassed ? "Passed: " : "Failed: ") + "EqualityImpliedByMandatoryError exists when all roles are mandatory");

				// Scenario 2: Remove the mandatory constraint from the second role
				testWindow.ClickAccessibleObject(accRole2_1);
				DTE.ExecuteCommand("OtherContextMenus.ORMDesignerContextMenu.IsMandatory", "");
				scenarioPassed = null == equalityConstraint.EqualityImpliedByMandatoryError;
				writer.WriteLine((scenarioPassed ? "Passed: " : "Failed: ") + "EqualityImpliedByMandatoryError does not exist when some roles are not mandatory");
				
				// Scenario 3: Delete the non-mandatory role
				SendKeys.SendWait("^{DEL}");
				scenarioPassed = null != equalityConstraint.EqualityImpliedByMandatoryError;
				writer.WriteLine((scenarioPassed ? "Passed: " : "Failed: ") + "EqualityImpliedByMandatoryError exists when the last non-mandatory role is deleted");

				// Scenario 4: Delete the role sequence
				DTE.ExecuteCommand("Edit.Undo", "");
				testWindow.ClickAccessibleObject(accEquality);
				testWindow.ClickAccessibleObject(accRole2_1);
				DTE.ExecuteCommand("OtherContextMenus.ORMDesignerContextMenu.DeleteRoleSequence", "");
				scenarioPassed = null != equalityConstraint.EqualityImpliedByMandatoryError;
				writer.WriteLine((scenarioPassed ? "Passed: " : "Failed: ") + "EqualityImpliedByMandatoryError exists when a role sequence with the last non-mandatory role is deleted");

				// Scenario 5: Delete the associated fact
				DTE.ExecuteCommand("Edit.Undo", "");
				testWindow.ClickAccessibleObject(accRole2_1.Parent, 1, ClickLocation.UpperLeft, 1, 1);
				SendKeys.SendWait("^{DEL}");
				scenarioPassed = null != equalityConstraint.EqualityImpliedByMandatoryError;
				writer.WriteLine((scenarioPassed ? "Passed: " : "Failed: ") + "EqualityImpliedByMandatoryError exists when the fact with the last non-mandatory role is deleted");

				// Scenario 6: Add an additional role sequence to a non-mandatory role
				DTE.ExecuteCommand("Edit.Undo", "");
				DTE.ExecuteCommand("Edit.Undo", "");
				testWindow.ClickAccessibleObject(accEquality, 2);
				testWindow.ClickAccessibleObject(accRole3_1, 2);
				SendKeys.SendWait("{ESC}");
				scenarioPassed = null == equalityConstraint.EqualityImpliedByMandatoryError;
				writer.WriteLine((scenarioPassed ? "Passed: " : "Failed: ") + "EqualityImpliedByMandatoryError does not exist a role sequence with a single non-mandatory role is added");

				// Scenario 7: Add a mandatory constraint to the non-mandatory role
				testWindow.ClickAccessibleObject(accRole3_1);
				DTE.ExecuteCommand("OtherContextMenus.ORMDesignerContextMenu.IsMandatory", "");
				scenarioPassed = null != equalityConstraint.EqualityImpliedByMandatoryError;
				writer.WriteLine((scenarioPassed ? "Passed: " : "Failed: ") + "EqualityImpliedByMandatoryError exists when the last non-mandatory role is change to a mandatory role");

				// Scenario 8: Add a second role to the third role sequence
				testWindow.ClickAccessibleObject(accRole3_1, 2);
				testWindow.ClickAccessibleObject(accRole3_2, 2);
				scenarioPassed = null == equalityConstraint.EqualityImpliedByMandatoryError;
				writer.WriteLine((scenarioPassed ? "Passed: " : "Failed: ") + "EqualityImpliedByMandatoryError does not exist if a role is added to make more than one column in at least one role sequence");

				// Scenario 9: Remove the second role from the third role sequence
				testWindow.ClickAccessibleObject(accEquality, 1);
				testWindow.ClickAccessibleObject(accRole3_2, 2);
				testWindow.KeyDown(Keys.ControlKey);
				testWindow.ClickAccessibleObject(accRole3_2, 2);
				testWindow.KeyUp(Keys.ControlKey);
				scenarioPassed = null != equalityConstraint.EqualityImpliedByMandatoryError;
				writer.WriteLine((scenarioPassed ? "Passed: " : "Failed: ") + "EqualityImpliedByMandatoryError exist if a role is removed from a role sequence, leaving a single-column in all role sequences with all mandatory roles");
			}
		}
	}
}
