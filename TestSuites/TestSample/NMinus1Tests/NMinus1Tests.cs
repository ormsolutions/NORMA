using System;
using System.Collections.Generic;
using System.Reflection;
using Neumont.Tools.ORM.SDK.TestEngine;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;

namespace TestSample.NMinus1Tests
{
	[Tests]
	public class NMinus1Tests
	{
		private IORMToolServices myServices;
		private IORMToolTestServices myTestServices;
		public NMinus1Tests(IORMToolServices services)
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
		[Test("Sample", "InternalConstraints", "NMinus1")]
		public void NMinus1Test1a(Store store)
		{}
		[Test("Sample", "InternalConstraints", "NMinus1")]
		public void NMinus1Test1b(Store store)
		{}
		[Test("Sample", "InternalConstraints", "NMinus1")]
		public void NMinus1Test1c(Store store)
		{}

		[Test("Sample", "InternalConstraints", "NMinus1")]
		public void NMinus1Test2a(Store store)
		{
			myTestServices.LogValidationErrors("Before adding error");

			ORMModel model = (ORMModel)store.ElementDirectory.GetElements(ORMModel.MetaClassGuid)[0];

			FactType fact = (FactType)model.FactTypesDictionary.GetElement("FactType1").SingleElement;
			RoleMoveableCollection roles = fact.RoleCollection;
			InternalConstraintMoveableCollection constraints = fact.InternalConstraintCollection;
			using (Transaction t = store.TransactionManager.BeginTransaction("Fix Constraint"))
			{
				//Make the error
				constraints[0].RoleCollection.Clear();
				constraints[0].RoleCollection.Add(roles[0]);
				
				t.Commit();
			}
			myTestServices.LogValidationErrors("After adding error");

		}


		[Test("Sample", "InternalConstraints", "NMinus1")]
		public void NMinus1Test2b(Store store)
		{
			myTestServices.LogValidationErrors("Before repair");

			ORMModel model = (ORMModel)store.ElementDirectory.GetElements(ORMModel.MetaClassGuid)[0];

			FactType fact = (FactType)model.FactTypesDictionary.GetElement("FactType1").SingleElement;
			RoleMoveableCollection roles = fact.RoleCollection;
			InternalConstraintMoveableCollection constraints = fact.InternalConstraintCollection;
			using (Transaction t = store.TransactionManager.BeginTransaction("Fix Constraint"))
			{
				//do the fixing
				constraints[0].RoleCollection.Clear();
				for(int i=0;i<roles.Count;++i)
				{
					constraints[0].RoleCollection.Add(roles[i]);
				}

				t.Commit();
			}
			myTestServices.LogValidationErrors("After repair");

		}


	}
}