using System;
using System.Collections.Generic;
using System.Reflection;
using Neumont.Tools.ORM.SDK.TestEngine;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using NUnit.Framework;
using NUnitCategory = NUnit.Framework.CategoryAttribute;
using Neumont.Tools.Modeling.Design;

namespace TestSample.ConstraintContradictionTests
{
	[ORMTestFixture]
	[TestFixture(Description = "Test Constraint Contradiction/Implication Errors")]
	public class ConstraintContradictionTests
	{
		#region Boilerplate code
		private IORMToolServices myServices;
		private IORMToolTestServices myTestServices;
		public ConstraintContradictionTests(IORMToolServices services)
		{
			myServices = services;
			myTestServices = (IORMToolTestServices)services.ServiceProvider.GetService(typeof(IORMToolTestServices));
		}
		public ConstraintContradictionTests()
			: this(Suite.CreateServices())
		{ }
		#endregion

		//ConstContradictTests_1a: Remove Exclusion Constraint to remove the error, [ExclusionContradictsEqualityError].
		//ConstContradictTests_1b: Remove Equality Constraint to remove the error, [ExclusionContradictsEqualityError].
		//ExclusionSubsetContra_1a: Remove Exclusion Constraint to remove the error, [ExclusionContradictsSubsetError].
		//ExclusionSubsetContra_1b: Remove Subset Constraint to remove the error, [ExclusionContradictsSubsetError].
		//ExclusionMandatoryContra_1a: Add a Simple Mandatory Constraint to introduce the error, [ExclusionContradictsMandatoryError], then Undo() to remove.
		//ExclusionMandatoryContra_1b: Add a Simple Mandatory Constraint to introduce the error, [ExclusionContradictsMandatoryError], then Remove the Exclusion Constraint to remove.


		#region ConstContradictTests_1a
		[Test(Description = "Remove Exclusion Constraint and Verify ExclusionContradictsEqualityError is also removed")]
		[NUnitCategory("ConstraintContradictions")]
		[NUnitCategory("ExclusionContradictsEqualityError")]
		public void ConstContradictTests_1a()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}

		[ORMTest("ConstraintContradictions", "ExclusionContradictsEqualityError")]
		public void ConstContradictTests_1a(Store store)
		{
			myTestServices.LogValidationErrors("ExclusionContradictsEqualityError exists");

			ORMModel model = store.ElementDirectory.FindElements<ORMModel>()[0];
			ExclusionConstraint constraint = (ExclusionConstraint)model.ConstraintsDictionary.GetElement("ExConstraint").FirstElement;
			myTestServices.LogValidationErrors("Removing Exclusion Constraint");

			using (Transaction t = store.TransactionManager.BeginTransaction("Remove Ex Constraint"))
			{
				constraint.Delete();
				t.Commit();
			}
			myTestServices.LogValidationErrors("Error Removed");
		}
		#endregion

		#region ConstContradictTests_1b
		[Test(Description = "Remove Equality Constraint and Verify ExclusionContradictsEqualityError is also removed")]
		[NUnitCategory("ConstraintContradictions")]
		[NUnitCategory("ExclusionContradictsEqualityError")]
		public void ConstContradictTests_1b()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}

		[ORMTest("ConstraintContradictions", "ExclusionContradictsEqualityError")]
		public void ConstContradictTests_1b(Store store)
		{
			myTestServices.LogValidationErrors("ExclusionContradictsEqualityError exists");

			ORMModel model = store.ElementDirectory.FindElements<ORMModel>()[0];
			EqualityConstraint constraint = (EqualityConstraint)model.ConstraintsDictionary.GetElement("EqConstraint").FirstElement;
			myTestServices.LogValidationErrors("Removing Equality Constraint");

			using (Transaction t = store.TransactionManager.BeginTransaction("Remove Eq Constraint"))
			{
				constraint.Delete();
				t.Commit();
			}
			myTestServices.LogValidationErrors("Error Removed");
		}
		#endregion

		#region ExclusionSubsetContra_1a
		[Test(Description = "Remove Exclusion Constraint and Verify ExclusionContradictsSubsetError is also removed")]
		[NUnitCategory("ConstraintContradictions")]
		[NUnitCategory("ExclusionContradictsSubsetError")]
		public void ExclusionSubsetContra_1a()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}

		[ORMTest("ConstraintContradictions", "ExclusionContradictsSubsetError")]
		public void ExclusionSubsetContra_1a(Store store)
		{
			myTestServices.LogValidationErrors("ExclusionContradictsSubsetError exists");

			ORMModel model = store.ElementDirectory.FindElements<ORMModel>()[0];
			ExclusionConstraint constraint = (ExclusionConstraint)model.ConstraintsDictionary.GetElement("ExclusionConstraint1").FirstElement;
			myTestServices.LogValidationErrors("Removing Exclusion Constraint");

			using (Transaction t = store.TransactionManager.BeginTransaction("Remove Ex Constraint"))
			{
				constraint.Delete();
				t.Commit();
			}
			myTestServices.LogValidationErrors("Error Removed");
		}
		#endregion

		#region ExclusionSubsetContra_1b
		[Test(Description = "Remove Subset Constraint and Verify ExclusionContradictsSubsetError is also removed")]
		[NUnitCategory("ConstraintContradictions")]
		[NUnitCategory("ExclusionContradictsSubsetError")]
		public void ExclusionSubsetContra_1b()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}

		[ORMTest("ConstraintContradictions", "ExclusionContradictsSubsetError")]
		public void ExclusionSubsetContra_1b(Store store)
		{
			myTestServices.LogValidationErrors("ExclusionContradictsSubsetError exists");

			ORMModel model = store.ElementDirectory.FindElements<ORMModel>()[0];
			SubsetConstraint constraint = (SubsetConstraint)model.ConstraintsDictionary.GetElement("SubsetConstraint1").FirstElement;
			myTestServices.LogValidationErrors("Removing Subset Constraint");

			using (Transaction t = store.TransactionManager.BeginTransaction("Remove subset Constraint"))
			{
				constraint.Delete();
				t.Commit();
			}
			myTestServices.LogValidationErrors("Error Removed");
		}
		#endregion

		#region ExclusionMandatoryContra_1a
		[Test(Description= "Add a simple mandatory constraint to conflict with existing Exclusion Constraint")]
		[NUnitCategory("ConstraintContradictions")]
		[NUnitCategory("ExclusionContradictsMandatoryError")]
		public void ExclusionMandatoryContra_1a()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}

		[ORMTest("ConstraintContradictions", "ExclusionContradictsMandatoryError")]
		public void ExclusionMandatoryContra_1a(Store store)
		{
			myTestServices.LogValidationErrors("No Errors Found Initialliy");

			ORMModel model = store.ElementDirectory.FindElements<ORMModel>()[0];

			// Input file(ExclusionMandatoryContra_1a.Load.orm) specific code.
			// Recommend finding a better method of retrieving the role in context.
			FactType factType = model.FactTypeCollection[2];
			Role role = (Role)factType.RoleCollection[0];

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

		#region ExclusionMandatoryContra_1b
		[Test(Description = "Add a simple mandatory constraint to conflict with existing Exclusion Constraint")]
		[NUnitCategory("ConstraintContradictions")]
		[NUnitCategory("ExclusionContradictsMandatoryError")]
		public void ExclusionMandatoryContra_1b()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}

		[ORMTest("ConstraintContradictions", "ExclusionContradictsMandatoryError")]
		public void ExclusionMandatoryContra_1b(Store store)
		{
			myTestServices.LogValidationErrors("No Errors Found Initialliy");

			ORMModel model = store.ElementDirectory.FindElements<ORMModel>()[0];
			Role role = (Role)store.ElementDirectory.GetElement(new Guid("1C424E34-8369-41EC-850F-FD24E7B30C7A"));
			ExclusionConstraint constraint = (ExclusionConstraint)model.ConstraintsDictionary.GetElement("ExclusionConstraint1").SingleElement;
			using (Transaction t = store.TransactionManager.BeginTransaction("Add Mandatory Constraint"))
			{
				role.IsMandatory = true;
				t.Commit();
			}
			myTestServices.LogValidationErrors("Error is Introduced");

			using (Transaction t = store.TransactionManager.BeginTransaction("Add Mandatory Constraint"))
			{
				constraint.Delete();
				t.Commit();
			}
			myTestServices.LogValidationErrors("Error is removed upon Removal of Exclusion Constraint");
		}
		#endregion


	}
}
