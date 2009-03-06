using System;
using System.Collections.Generic;
using System.Reflection;
using ORMSolutions.ORMArchitectSDK.TestEngine;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using NUnit.Framework;
using NUnitCategory = NUnit.Framework.CategoryAttribute;
using ORMSolutions.ORMArchitect.Framework.Design;

namespace TestSample.FCMinMaxTests
{
	[ORMTestFixture]
	[TestFixture(Description = "Test the FrequencyConstraintExactlyOneError")]
	public class FCExactlyOneTests
	{
		#region Boilerplate code
		private IORMToolServices myServices;
		private IORMToolTestServices myTestServices;
		public FCExactlyOneTests(IORMToolServices services)
		{
			myServices = services;
			myTestServices = (IORMToolTestServices)services.ServiceProvider.GetService(typeof(IORMToolTestServices));
		}
		public FCExactlyOneTests() : this(Suite.CreateServices()) { }
		#endregion

		// ExactlyOneTest1a - Change a valid FrequencyConstraint to have min-max=1, convert to external uniqueness
		// ExactlyOneTest_1b - Change a valid Frequency Constraint to min-max=1, convert to internal uniqueness. Then remove duplicate internal uniqueness.

		#region ExactlyOneTest1a
		[Test(Description = "Verify FrequencyConstraintExactlyOneError is added and automatically fixed")]
		[NUnitCategory("ExternalConstraints")]
		[NUnitCategory("FrequencyConstraintExactlyOneError")]
		public void ExactlyOneTest1a()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}

		[ORMTest("ExternalConstraints", "FrequencyConstraintExactlyOneError")]
		public void ExactlyOneTest1a(Store store)
		{
			myTestServices.LogValidationErrors("No errors expected");

			ORMModel model = store.ElementDirectory.FindElements<ORMModel>()[0];
			FrequencyConstraint constraint = (FrequencyConstraint)model.ConstraintsDictionary.GetElement("TestFrequency").SingleElement;
			DomainTypeDescriptor.CreatePropertyDescriptor(constraint, FrequencyConstraint.MaxFrequencyDomainPropertyId).SetValue(constraint, 1);
			myTestServices.LogValidationErrors("Error introduced");

			using (Transaction t = store.TransactionManager.BeginTransaction("Read the error"))
			{
				constraint.MinFrequency = 3;
				constraint.MaxFrequency = 4;
				t.Commit();
			}
			myTestServices.LogValidationErrors("Error cleared with code");
			store.UndoManager.Undo();
			((IORMToolServices)store).ModelErrorActivationService.ActivateError(constraint, constraint.FrequencyConstraintExactlyOneError);
			myTestServices.LogValidationErrors("Fixing error with error activation service");
		}
		#endregion

		[Test(Description = "Verify FrequencyConstraintExactlyOneError is added and automatically fixed")]
		[NUnitCategory("ConstraintImplication")]
		[NUnitCategory("ImpliedUniquenessConstraintError")]
		[NUnitCategory("FrequencyConstraintExactlyOneError")]
		[NUnitCategory("FrequencyConstraintContradictsUniquenessConstraintError")]

		public void ExactlyOneTest_1b()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}

		/*
		 * ExactlyOneTest_1b() pending fixes on Frequency Constraint on Internally unique role.
		 * -Frequency Constraint on a unique role must throw error:
		 *    -Case 1: FC (min-max) = (1-1) --> Result in [FrequencyConstraintExactlyOneError]
		 *    -Case 2: FC (min-max) = (1-*) --> Result in [FrequencyConstraintContradictsUniquenessConstraintError]
		 */
		[ORMTest("ConstraintImplication", "ImpliedUniquenessConstraintError", "FrequencyConstraintExactlyOneError", "FrequencyConstraintContradictsUniquenessConstraintError")]
		public void ExactlyOneTest_1b(Store store)
		{
			myTestServices.LogValidationErrors("No errors expected");
			ORMModel model = store.ElementDirectory.FindElements<ORMModel>()[0];
			FrequencyConstraint constraint = (FrequencyConstraint)model.ConstraintsDictionary.GetElement("FrequencyConstraint1").SingleElement;
			// DomainTypeDescriptor.CreatePropertyDescriptor(constraint, FrequencyConstraint.MaxFrequencyDomainPropertyId).SetValue(constraint, 1);
			myTestServices.LogValidationErrors("Introduce Error[FrequencyConstraintExactlyOneError]");
			using (Transaction t = store.TransactionManager.BeginTransaction("Read the error"))
			{
				constraint.MinFrequency = 1;
				constraint.MaxFrequency = 1;
				t.Commit();
			}
			myTestServices.LogValidationErrors("Fixing error with error activation service");
			((IORMToolServices)store).ModelErrorActivationService.ActivateError(constraint, constraint.FrequencyConstraintExactlyOneError);
			myTestServices.LogValidationErrors("[FrequencyConstraintExactlyOneError] removed, different error introduced. Removing new error");

			// Deleting one of the two uniqnuess constraints...
			UniquenessConstraint uniqueC = (UniquenessConstraint)model.ConstraintsDictionary.GetElement("InternalUniquenessConstraint5").SingleElement;
			using (Transaction t = store.TransactionManager.BeginTransaction("Read the error"))
			{
				uniqueC.Delete();
				t.Commit();
			}
			myTestServices.LogValidationErrors("All errors resolved");
		}
	}
}
