using System;
using System.Collections.Generic;
using System.Reflection;
using Neumont.Tools.ORM.SDK.TestEngine;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using NUnit.Framework;
using NUnitCategory = NUnit.Framework.CategoryAttribute;

namespace TestSample.NMinus1Tests
{
	[ORMTestFixture]
	[TestFixture(Description = "Test the NMinusOneError")]
	public class NMinus1Tests
	{
		#region Boilerplate code
		private IORMToolServices myServices;
		private IORMToolTestServices myTestServices;
		public NMinus1Tests(IORMToolServices services)
		{
			myServices = services;
			myTestServices = (IORMToolTestServices)services.ServiceProvider.GetService(typeof(IORMToolTestServices));
		}
		public NMinus1Tests() : this(Suite.CreateServices()) { }
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

		[Test(Description = "Load/Save with NMinusOneError")]
		[NUnitCategory("InternalConstraints")]
		[NUnitCategory("NMinusOneError")]
		public void NMinus1Test1a()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}
		[ORMTest("InternalConstraints", "NMinusOneError")]
		public void NMinus1Test1a(Store store)
		{}

		[Test(Description = "Verify NMinusOneError automated automatically added on load")]
		[NUnitCategory("InternalConstraints")]
		[NUnitCategory("NMinusOneError")]
		public void NMinus1Test1b()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}
		[ORMTest("InternalConstraints", "NMinusOneError")]
		public void NMinus1Test1b(Store store)
		{}

		[Test(Description = "Verify NMinusOneError automated automatically removed on load")]
		[NUnitCategory("InternalConstraints")]
		[NUnitCategory("NMinusOneError")]
		public void NMinus1Test1c()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}
		[ORMTest("InternalConstraints", "NMinusOneError")]
		public void NMinus1Test1c(Store store)
		{}

		[Test(Description = "Verify NMinusOneError added automatically")]
		[NUnitCategory("InternalConstraints")]
		[NUnitCategory("NMinusOneError")]
		public void NMinus1Test2a()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}
		[ORMTest("InternalConstraints", "NMinusOneError")]
		public void NMinus1Test2a(Store store)
		{
			myTestServices.LogValidationErrors("Before adding error");

			ORMModel model = (ORMModel)store.ElementDirectory.GetElements(ORMModel.MetaClassGuid)[0];

			FactType fact = (FactType)store.ElementDirectory.GetElement(new Guid("655E4D9B-9835-4BE2-A7BC-FEBE51A32E84"));
			RoleBaseMoveableCollection roles = fact.RoleCollection;
			using (Transaction t = store.TransactionManager.BeginTransaction("Fix Constraint"))
			{
				foreach (UniquenessConstraint constraint in fact.GetInternalConstraints<UniquenessConstraint>())
				{
					//Make the error
					constraint.RoleCollection.Clear();
					constraint.RoleCollection.Add(roles[0].Role);
					break;
				}
				
				t.Commit();
			}
			myTestServices.LogValidationErrors("After adding error");

		}

		[Test(Description = "Verify NMinusOneError removed automatically")]
		[NUnitCategory("InternalConstraints")]
		[NUnitCategory("NMinusOneError")]
		public void NMinus1Test2b()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}
		[ORMTest("InternalConstraints", "NMinusOneError")]
		public void NMinus1Test2b(Store store)
		{
			myTestServices.LogValidationErrors("Before repair");

			ORMModel model = (ORMModel)store.ElementDirectory.GetElements(ORMModel.MetaClassGuid)[0];

			FactType fact = (FactType)store.ElementDirectory.GetElement(new Guid("655E4D9B-9835-4BE2-A7BC-FEBE51A32E84"));
			RoleBaseMoveableCollection roles = fact.RoleCollection;
			using (Transaction t = store.TransactionManager.BeginTransaction("Fix Constraint"))
			{
				foreach (UniquenessConstraint constraint in fact.GetInternalConstraints<UniquenessConstraint>())
				{
					//do the fixing
					constraint.RoleCollection.Clear();
					for (int i = 0; i < roles.Count; ++i)
					{
						constraint.RoleCollection.Add(roles[i].Role);
					}
					break;
				}

				t.Commit();
			}
			myTestServices.LogValidationErrors("After repair");

		}


	}
}