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
	[TestFixture(Description = "Test the FrequencyConstraintExactlyOneError")]
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



	}
}
