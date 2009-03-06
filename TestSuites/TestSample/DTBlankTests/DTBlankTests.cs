using System;
using System.Collections.Generic;
using System.Reflection;
using ORMSolutions.ORMArchitectSDK.TestEngine;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using NUnit.Framework;
using NUnitCategory = NUnit.Framework.CategoryAttribute;

namespace TestSample.DTBlankTests
{
	/// <summary>
	/// Tests for the UnspecifiedDataTypeError
	/// </summary>
	[ORMTestFixture]
	[TestFixture(Description="Test the DataTypeNotSpecifiedError")]
	public class DTBlankTests
	{
		#region Boilerplate code
		private IORMToolServices myServices;
		private IORMToolTestServices myTestServices;
		public DTBlankTests(IORMToolServices services)
		{
			myServices = services;
			myTestServices = (IORMToolTestServices)services.ServiceProvider.GetService(typeof(IORMToolTestServices));
		}
		public DTBlankTests() : this(Suite.CreateServices()) { }
		#endregion // Boilerplate code

		/*	Tests annotated with 1 for a load test and 2 for tests that change the condition causing the error
		 * 1a - tests serialization and deserialization of the error and condition
		 * 1b - tests that if the condition exists but not the error, the error is added on load
		 * 1c - tests that if the error exists but not the condition, the error is removed on load
		 * 2a - Verify that adding the condition adds the bug
		 * 2b - Verify that removing the condition removes the bug
		 */


		[Test(Description = "Load/Save with DataTypeNotSpecifiedError")]
		[NUnitCategory("DataTypeNotSpecifiedError")]
		public void DTBlankTest1a()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}
		[ORMTest("DataTypeNotSpecifiedError")]
		public void DTBlankTest1a(Store store)
		{}

		[Test(Description = "Verify DataTypeNotSpecifiedError added automatically on load")]
		[NUnitCategory("DataTypeNotSpecifiedError")]
		public void DTBlankTest1b()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}
		[ORMTest("DataTypeNotSpecifiedError")]
		public void DTBlankTest1b(Store store)
		{}

		[Test(Description = "Verify DataTypeNotSpecifiedError removed automatically on load")]
		[NUnitCategory("DataTypeNotSpecifiedError")]
		public void DTBlankTest1c()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}
		[ORMTest("DataTypeNotSpecifiedError")]
		public void DTBlankTest1c(Store store)
		{}

		[Test(Description = "Add DataTypeNotSpecified Error")]
		[NUnitCategory("DataTypeNotSpecifiedError")]
		public void DTBlankTest2a()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}
		[ORMTest("DataTypeNotSpecifiedError")]
		public void DTBlankTest2a(Store store)
		{
			myTestServices.LogValidationErrors("Before adding error");

			ORMModel model = store.ElementDirectory.FindElements<ORMModel>()[0];
			model.Store.ElementDirectory.FindElement(UnspecifiedDataType.DomainClassId);
			UnspecifiedDataType unspecified = model.Store.ElementDirectory.FindElements<UnspecifiedDataType>()[0];
			
			ObjectType o = (ObjectType)model.ObjectTypesDictionary.GetElement("WifeId").SingleElement;


			using (Transaction t = store.TransactionManager.BeginTransaction("Add invalid data type error"))
			{
				//Make the error
				o.DataType = unspecified;
				t.Commit();
			}
			
			myTestServices.LogValidationErrors("After adding error");
		}

		[Test(Description = "Remove DataTypeNotSpecified Error")]
		[NUnitCategory("DataTypeNotSpecifiedError")]
		public void DTBlankTest2b()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}
		[ORMTest("DataTypeNotSpecifiedError")]
		public void DTBlankTest2b(Store store)
		{

			myTestServices.LogValidationErrors("Before removing error");
			ORMModel model = store.ElementDirectory.FindElements<ORMModel>()[0]; 
			FloatingPointNumericDataType numeric = model.Store.ElementDirectory.FindElements<FloatingPointNumericDataType>()[0];
			ObjectType o = (ObjectType)model.ObjectTypesDictionary.GetElement("WifeId").SingleElement;
			using (Transaction t = store.TransactionManager.BeginTransaction("Remove invalid data type error"))
			{
				//remove
				o.DataType = numeric;
				t.Commit();
			}

			myTestServices.LogValidationErrors("After removing error");
		}

	}
}