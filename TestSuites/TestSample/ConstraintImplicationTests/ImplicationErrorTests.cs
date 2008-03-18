using System;
using System.Collections.Generic;
using System.Reflection;
using Neumont.Tools.ORM.SDK.TestEngine;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using NUnit.Framework;
using NUnitCategory = NUnit.Framework.CategoryAttribute;
using Neumont.Tools.Modeling.Design;

namespace TestSample.ConstraintImplicationTests
{
	[ORMTestFixture]
	[TestFixture(Description = "Test the Implication Errors")]
	public class ImplicationErrorTests
	{
		#region Boilerplate code
		private IORMToolServices myServices;
		private IORMToolTestServices myTestServices;
		public ImplicationErrorTests(IORMToolServices services)
		{
			myServices = services;
			myTestServices = (IORMToolTestServices)services.ServiceProvider.GetService(typeof(IORMToolTestServices));
		}
		public ImplicationErrorTests() : this(Suite.CreateServices()) { }
		#endregion

		//EqualityMandatoryImplied_1a: Add a Simple Mandatory Constraint to introduce the error, [EqualityImpliedByMandatoryError][EqualityOrSubsetImpliedByMandatoryError], then Undo() to remove.
		//EqualityMandatoryImplied_1b: Add a Simple Mandatory Constraint to introduce the error, [EqualityImpliedByMandatoryError][EqualityOrSubsetImpliedByMandatoryError], then Remove the Equality Constraint to remove.

		#region EqualityMandatoryImplied_1a
		[Test(Description = "Add a simple mandatory constraint to conflict with existing Equality Constraint")]
		[NUnitCategory("ConstraintContradictions")]
		[NUnitCategory("EqualityImpliedByMandatoryError")]
		[NUnitCategory("EqualityOrSubsetImpliedByMandatoryError")]
		public void EqualityMandatoryImplied_1a()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}

		[ORMTest("ConstraintContradictions", "EqualityImpliedByMandatoryError", "EqualityOrSubsetImpliedByMandatoryError")]
		public void EqualityMandatoryImplied_1a(Store store)
		{
			myTestServices.LogValidationErrors("No Errors Found Initialliy");

			ORMModel model = store.ElementDirectory.FindElements<ORMModel>()[0];
			Role role = (Role)store.ElementDirectory.GetElement(new Guid("1C424E34-8369-41EC-850F-FD24E7B30C7A"));

			using (Transaction t = store.TransactionManager.BeginTransaction("Add Mandatory Constraint"))
			{
				role.IsMandatory = true;
				t.Commit();
			}
			myTestServices.LogValidationErrors("Error is Introduced");
			store.UndoManager.Undo();
			myTestServices.LogValidationErrors("Error is removed with Undo");
		}
		#endregion

		#region EqualityMandatoryImplied_1b
		[Test(Description = "Add a simple mandatory constraint to conflict with existing Equality Constraint")]
		[NUnitCategory("ConstraintContradictions")]
		[NUnitCategory("EqualityImpliedByMandatoryError")]
		[NUnitCategory("EqualityOrSubsetImpliedByMandatoryError")]
		public void EqualityMandatoryImplied_1b()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}

		[ORMTest("ConstraintContradictions", "EqualityImpliedByMandatoryError", "EqualityOrSubsetImpliedByMandatoryError")]
		public void EqualityMandatoryImplied_1b(Store store)
		{
			myTestServices.LogValidationErrors("No Errors Found Initialliy");

			ORMModel model = store.ElementDirectory.FindElements<ORMModel>()[0];
			Role role = (Role)store.ElementDirectory.GetElement(new Guid("1C424E34-8369-41EC-850F-FD24E7B30C7A"));
			EqualityConstraint constraint = (EqualityConstraint)model.ConstraintsDictionary.GetElement("EqualityConstraint1").SingleElement;

			using (Transaction t = store.TransactionManager.BeginTransaction("Add Mandatory Constraint"))
			{
				role.IsMandatory = true;
				t.Commit();
			}
			myTestServices.LogValidationErrors("Error is Introduced");

			using (Transaction t = store.TransactionManager.BeginTransaction("Remove Equality Constraint"))
			{
				constraint.Delete();
				t.Commit();
			}
			myTestServices.LogValidationErrors("Error is removed with removal of Equality Constraint");
		}
		#endregion

		#region UniqueUniqueImplied_1a
		[Test(Description = "")]
		[NUnitCategory("ConstraintImplications")]
		[NUnitCategory("ImplicationError")]
		[NUnitCategory("ExternalUniquenessConstraint")]

		public void UniqueUniqueImplied_1a()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}

		[ORMTest("ConstraintImplications", "ImplicationError", "ExternalUniquenessConstraint")]
		public void UniqueUniqueImplied_1a(Store store)
		{
			myTestServices.LogValidationErrors("Start with one Implication Error");
			ORMModel model = store.ElementDirectory.FindElements<ORMModel>()[0];
			UniquenessConstraint constraint = (UniquenessConstraint)model.ConstraintsDictionary.GetElement("ExternalUniquenessConstraint_A").SingleElement;
			myTestServices.LogMessage("Removing the Implied Uniqueness Costraint...");
			using (Transaction t = store.TransactionManager.BeginTransaction("Remove Implied Uniqueness Costraint"))
			{
				constraint.Delete();
				t.Commit();
			}
			myTestServices.LogValidationErrors("Error is removed with removal of Implied Constraint");
		}
		#endregion

		#region MandatoryMandatory_1a
		[Test(Description = "")]
		[NUnitCategory("ConstraintImplications")]
		[NUnitCategory("ImplicationError")]
		[NUnitCategory("MandatoryConstraints")]
		public void MandatoryMandatory_1a()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}

		[ORMTest("ConstraintImplications", "ImplicationError", "MandatoryConstraints")]
		public void MandatoryMandatory_1a(Store store)
		{
			myTestServices.LogValidationErrors("Start with no Error");
			ORMModel model = store.ElementDirectory.FindElements<ORMModel>()[0];
			Role role_A = (Role)store.ElementDirectory.GetElement(new Guid("FA2156EE-8612-47F3-8DB8-861814BAD997"));
			Role role_B = (Role)store.ElementDirectory.GetElement(new Guid("7DD25522-062E-4134-8B7A-F453FD979281"));
			MandatoryConstraint constraint = (MandatoryConstraint)model.ConstraintsDictionary.GetElement("InclusiveOrConstraint1").SingleElement;

			myTestServices.LogMessage("Introducing error via creating additional implied mandatory constraint");
			using (Transaction t = store.TransactionManager.BeginTransaction("Add Mandatory Costraint"))
			{
				role_A.IsMandatory = true;
				t.Commit();
			}
			myTestServices.LogValidationErrors("Error Received. Proceed to removal via removing the other Mandatory Constraint");
			using (Transaction t = store.TransactionManager.BeginTransaction("Remove a Mandatory Costraint"))
			{
				constraint.Delete();
				t.Commit();
			}
			myTestServices.LogValidationErrors("Error is removed.");
		}
		#endregion

		#region MandatoryMandatory_1b
		[Test(Description = "")]
		[NUnitCategory("ConstraintImplications")]
		[NUnitCategory("ImplicationError")]
		[NUnitCategory("MandatoryConstraints")]
		public void MandatoryMandatory_1b()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}

		[ORMTest("ConstraintImplications", "ImplicationError", "MandatoryConstraints")]
		public void MandatoryMandatory_1b(Store store)
		{
			myTestServices.LogValidationErrors("Start with one Implication Error");
			ORMModel model = store.ElementDirectory.FindElements<ORMModel>()[0];
			MandatoryConstraint constraint_1 = (MandatoryConstraint)model.ConstraintsDictionary.GetElement("InclusiveOrConstraint1").SingleElement;
			MandatoryConstraint constraint_2 = (MandatoryConstraint)model.ConstraintsDictionary.GetElement("InclusiveOrConstraint2").SingleElement;

			using (Transaction t = store.TransactionManager.BeginTransaction("Remove a Mandatory Costraint"))
			{
				constraint_1.Delete();
				t.Commit();
			}
			myTestServices.LogValidationErrors("Error is removed with removal of a Constraint");
			myTestServices.LogValidationErrors("Undo to bring to initial condition");			
			store.UndoManager.Undo();

			myTestServices.LogValidationErrors("Proceed to remove the other Mandatory Constraint");
			using (Transaction t = store.TransactionManager.BeginTransaction("Remove a Mandatory Costraint"))
			{
				constraint_2.Delete();
				t.Commit();
			}
			myTestServices.LogValidationErrors("All Errors Removed");
		}
		#endregion
	}
}
