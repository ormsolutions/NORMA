using System;
using System.Collections.Generic;
using System.Reflection;
using Neumont.Tools.ORM.SDK.TestEngine;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using NUnit.Framework;
using NUnitCategory = NUnit.Framework.CategoryAttribute;

namespace TestSample.FCMinMaxTests
{
	[ORMTestFixture]
	[TestFixture(Description = "Test the FrequencyConstraintMinMaxError")]
	public class FCMinMaxTests
	{
		#region Boilerplate code
		private IORMToolServices myServices;
		private IORMToolTestServices myTestServices;
		public FCMinMaxTests(IORMToolServices services)
		{
			myServices = services;
			myTestServices = (IORMToolTestServices)services.ServiceProvider.GetService(typeof(IORMToolTestServices));
		}
		public FCMinMaxTests() : this(Suite.CreateServices()) { }
		#endregion // Boilerplate code
		/*	Tests annotated with 1 for a load test and 2 for tests that change the condition causing the error
		 * 1a - tests serialization and deserialization of the error and condition
		 * 1b - tests that if the condition exists but not the error, the error is added on load
		 * 1c - tests that if the error exists but not the condition, the error is removed on load
		 * 2a - Verify that adding the condition adds the bug
		 * 2b - Verify that removing the condition removes the bug
		 */

		//method bodies intentionally empty. Loading the file should
		//fixup the object model, which will be verified on save.

		[Test(Description = "Load/Save with FrequencyConstraintMinMaxError")]
		[NUnitCategory("ExternalConstraints")]
		[NUnitCategory("FrequencyConstraintMinMaxError")]
		public void FCMinMaxTest1a()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}
		[ORMTest("ExternalConstraints", "FrequencyConstraintMinMaxError")]
		public void FCMinMaxTest1a(Store store)
		{ }

		[Test(Description = "Verify FrequencyConstraintMinMaxError added automatically on load")]
		[NUnitCategory("ExternalConstraints")]
		[NUnitCategory("FrequencyConstraintMinMaxError")]
		public void FCMinMaxTest1b()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}
		[ORMTest("ExternalConstraints", "FrequencyConstraintMinMaxError")]
		public void FCMinMaxTest1b(Store store)
		{ }

		[Test(Description = "Verify FrequencyConstraintMinMaxError added automatically removed on load")]
		[NUnitCategory("ExternalConstraints")]
		[NUnitCategory("FrequencyConstraintMinMaxError")]
		public void FCMinMaxTest1c()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}
		[ORMTest("ExternalConstraints", "FrequencyConstraintMinMaxError")]
		public void FCMinMaxTest1c(Store store)
		{}

		[Test(Description = "Verify FrequencyConstraintMinMaxError added when error condition is added")]
		[NUnitCategory("ExternalConstraints")]
		[NUnitCategory("FrequencyConstraintMinMaxError")]
		public void FCMinMaxTest2a()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}
		[ORMTest("ExternalConstraints", "FrequencyConstraintMinMaxError")]
		public void FCMinMaxTest2a(Store store)
		{
			
			myTestServices.LogValidationErrors("Before adding error");

			ORMModel model = (ORMModel)store.ElementDirectory.GetElements(ORMModel.MetaClassGuid)[0];
			FrequencyConstraint constraint = (FrequencyConstraint)model.ConstraintsDictionary.GetElement("FrequencyConstraint1").SingleElement; 
			int min = constraint.MinFrequency;
			int max = constraint.MaxFrequency;
			using (Transaction t = store.TransactionManager.BeginTransaction("Fix Constraint"))
			{
				//Make the error
				constraint.MinFrequency = max;
				constraint.MaxFrequency = min;
				t.Commit();
			}
			
			myTestServices.LogValidationErrors("After adding error");
		}

		[Test(Description = "Verify FrequencyConstraintMinMaxError removed when error condition is resolved")]
		[NUnitCategory("ExternalConstraints")]
		[NUnitCategory("FrequencyConstraintMinMaxError")]
		public void FCMinMaxTest2b()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}
		[ORMTest("ExternalConstraints", "FrequencyConstraintMinMaxError")]
		public void FCMinMaxTest2b(Store store)
		{

			myTestServices.LogValidationErrors("Before removing error");

			ORMModel model = (ORMModel)store.ElementDirectory.GetElements(ORMModel.MetaClassGuid)[0];
			FrequencyConstraint constraint = (FrequencyConstraint)model.ConstraintsDictionary.GetElement("FrequencyConstraint1").SingleElement;
			int min = constraint.MinFrequency;
			int max = constraint.MaxFrequency;
			using (Transaction t = store.TransactionManager.BeginTransaction("Fix Constraint"))
			{
				//Fix the error
				constraint.MinFrequency = max;
				constraint.MaxFrequency = min;
				t.Commit();
			}

			myTestServices.LogValidationErrors("After removing error");
		}

	}
}