using System;
using System.Collections.Generic;
using System.Reflection;
using Neumont.Tools.ORM.SDK.TestEngine;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;

namespace TestSample.DTBlankTests
{
	//For the UnspecifiedDataTypeError
	[Tests]
	public class DTBlankTests
	{
		private IORMToolServices myServices;
		private IORMToolTestServices myTestServices;
		public DTBlankTests(IORMToolServices services)
		{
			myServices = services;
			myTestServices = (IORMToolTestServices)services.ServiceProvider.GetService(typeof(IORMToolTestServices));
		}


		/*	Tests annotated with 1 for a load test and 2 for tests that change the condition causing the error
		 * 1a - tests serialization and deserialization of the error and condition
		 * 1b - tests that if the condition exists but not the error, the error is added on load
		 * 1c - tests that if the error exists but not the condition, the error is removed on load
		 * 2a - Verify that adding the condition adds the bug
		 * 2b - Verify that removing the condition removes the bug
		 */

		//method bodies intentionally empty
		[Test("ExternalConstraints", "DTBlank")]
		public void DTBlankTest1a(Store store)
		{}
		[Test("ExternalConstraints", "DTBlank")]
		public void DTBlankTest1b(Store store)
		{}
		[Test("ExternalConstraints", "DTBlank")]
		public void DTBlankTest1c(Store store)
		{}

		[Test("ExternalConstraints", "DTBlank")]
		public void DTBlankTest2a(Store store)
		{
			
			myTestServices.LogValidationErrors("Before adding error");

			ORMModel model = (ORMModel)store.ElementDirectory.GetElements(ORMModel.MetaClassGuid)[0];
            //ObjectType = model.

            foreach(ObjectType o in model.ValueTypeCollection)
            {
                o.DataType.GetType();
               
                //if(typeof(ValueType) == )
                {
                }
            }
            //model.ValueTypeCollection
            //model.DataTypeCollection;
            
            //using (Transaction t = store.TransactionManager.BeginTransaction("Add invalid data type error"))
            //{
            //    //Make the error

            //    t.Commit();
            //}
			
			myTestServices.LogValidationErrors("After adding error");
		}

		[Test("ExternalConstraints", "DTBlank", "NotYet")]
		public void DTBlankTest2b(Store store)
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
			//if (dataType.GetType() == typeof(FixedLengthTextDataType))
			myTestServices.LogValidationErrors("After removing error");
		}

	}
}