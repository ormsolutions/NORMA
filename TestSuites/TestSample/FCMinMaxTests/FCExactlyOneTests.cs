using System;
using System.Collections.Generic;
using System.Reflection;
using Neumont.Tools.ORM.SDK.TestEngine;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using NUnit.Framework;
using NUnitCategory = NUnit.Framework.CategoryAttribute;
using Neumont.Tools.Modeling.Design;

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

		//  * ExactlyOneTest1a - Change a valid FrequencyConstraint to have min-max=1, convert to external uniqueness

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
	}
}
