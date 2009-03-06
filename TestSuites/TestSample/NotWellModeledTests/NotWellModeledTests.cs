using System;
using System.Collections.Generic;
using System.Reflection;
using ORMSolutions.ORMArchitectSDK.TestEngine;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using NUnit.Framework;
using NUnitCategory = NUnit.Framework.CategoryAttribute;
using ORMSolutions.ORMArchitect.Framework.Design;

namespace TestSample.NotWellModeledTests
{
	[ORMTestFixture]
	[TestFixture(Description = "Test the Implication Errors")]
	public class NotWellModeledTests
	{
		#region Boilerplate code
		private IORMToolServices myServices;
		private IORMToolTestServices myTestServices;
		public NotWellModeledTests(IORMToolServices services)
		{
			myServices = services;
			myTestServices = (IORMToolTestServices)services.ServiceProvider.GetService(typeof(IORMToolTestServices));
		}
		public NotWellModeledTests() : this(Suite.CreateServices()) { }
		#endregion

		// SubsetMandatory_1a : 

		#region SubsetMandatory_1a
		[Test(Description = "")]
		[NUnitCategory("NotWellModeledErrors")]
		[NUnitCategory("NotWellModeledSubsetAndMandatoryError")]

		public void SubsetMandatory_1a()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}

		[ORMTest("NotWellModeledErrors", "NotWellModeledSubsetAndMandatoryError")]
		public void SubsetMandatory_1a(Store store)
		{
			myTestServices.LogValidationErrors("No Errors Found Initially");
			ORMModel model = store.ElementDirectory.FindElements<ORMModel>()[0];
			Role role_2 = (Role)store.ElementDirectory.GetElement(new Guid("82DF5594-2020-4CA3-8154-FD92EE83F726"));

			myTestServices.LogValidationErrors("Intoduce Error: Make a role of supertype mandatory");
			using (Transaction t = store.TransactionManager.BeginTransaction("Add simple mandatory constraint"))
			{
				role_2.IsMandatory = true;
				t.Commit();
			}
			myTestServices.LogValidationErrors("Error Found. Calling Undo to remove error...");
			store.UndoManager.Undo();
			myTestServices.LogValidationErrors("Error removed with undo.");


			myTestServices.LogValidationErrors("Intoduce Error: Make a role of subtype mandatory");
			using (Transaction t = store.TransactionManager.BeginTransaction("Add simple mandatory constraint"))
			{
				role_2.IsMandatory = true;
				t.Commit();
			}
			myTestServices.LogValidationErrors("Error Found. Calling Undo to remove error...");
			using (Transaction t = store.TransactionManager.BeginTransaction("Add simple mandatory constraint"))
			{
				role_2.IsMandatory = false;
				t.Commit();
			}
			myTestServices.LogValidationErrors("Error is removed with changing property value...");
		}
		#endregion
	}
}
