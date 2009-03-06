using System;
using System.Collections.Generic;
using System.Reflection;
using ORMSolutions.ORMArchitectSDK.TestEngine;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using NUnit.Framework;
using NUnitCategory = NUnit.Framework.CategoryAttribute;
using ORMSolutions.ORMArchitect.Framework.Design;

namespace TestSample.SamplePopulationTests
{
	[ORMTestFixture]
	[TestFixture(Description = "Test SamplePopulation Errors")]
	public class PopulationTests
	{
		#region Boilerplate code
		private IORMToolServices myServices;
		private IORMToolTestServices myTestServices;
		public PopulationTests(IORMToolServices services)
		{
			myServices = services;
			myTestServices = (IORMToolTestServices)services.ServiceProvider.GetService(typeof(IORMToolTestServices));
		}
        public PopulationTests() : this(Suite.CreateServices()) { }
		#endregion // Boilerplate code
		//method bodies intentionally empty. Loading the file should
		//fixup the object model, which will be verified on save.

		[Test(Description = "Load/Save with NMinusOneError")]
		[NUnitCategory("SamplePopulation")]
		[NUnitCategory("PopulationMandatoryError")]
		public void PopulationSampleTest()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}
        [ORMTest("SamplePopulation", "PopulationMandatoryError")]
        public void PopulationSampleTest(Store store)
		{
            myTestServices.LogValidationErrors("Expect V2=100 Population Mandatory");

            myTestServices.LogMessage("Testing IsIndependent and PopulationMandatoryError associated with implied mandatory constraint");
			ORMModel model = store.ElementDirectory.FindElements<ORMModel>()[0];
            ObjectType v2 = (ObjectType)model.ObjectTypesDictionary.GetElement("V2").SingleElement;
            using (Transaction t = store.TransactionManager.BeginTransaction("Set IsIndependent"))
            {
                v2.IsIndependent = true;
                t.Commit();
            }
            myTestServices.LogValidationErrors("Set IsIndependent, No error expected after explicit transaction");
            store.UndoManager.Undo();
            DomainTypeDescriptor.CreatePropertyDescriptor(v2, ObjectType.IsIndependentDomainPropertyId).SetValue(v2, true);
            myTestServices.LogValidationErrors("Set IsIndependent, No error expected after implicit transaction");
            store.UndoManager.Undo();

            myTestServices.LogMessage("Testing PopulationMandatoryError by turning on a SimpleMandatory constraint.");
            RoleBase rightRole = v2.PlayedRoleCollection[0];
            RoleBase leftRole = rightRole.OppositeRole;
            FactType factType = rightRole.FactType;
            ObjectType populateMe = leftRole.Role.RolePlayer;
            LinkedElementCollection<ObjectTypeInstance> populateMeInstances = populateMe.ObjectTypeInstanceCollection;
            LinkedElementCollection<ObjectTypeInstance> v2Instances = v2.ObjectTypeInstanceCollection;
            using (Transaction t = store.TransactionManager.BeginTransaction("Set IsMandatory"))
            {
                leftRole.Role.IsMandatory = true;
                t.Commit();
            }
            myTestServices.LogValidationErrors("Set IsMandatory, expect new errors for instances 'Tom' and 'Dick'");
            FactTypeInstance factTypeInstance;
            using (Transaction t = store.TransactionManager.BeginTransaction("Populate first row"))
            {
                factTypeInstance = new FactTypeInstance(store);
                factTypeInstance.FactType = factType;
                new FactTypeRoleInstance(leftRole.Role, populateMeInstances[0]).FactTypeInstance = factTypeInstance;
                new FactTypeRoleInstance(rightRole.Role, v2Instances[0]).FactTypeInstance = factTypeInstance;
                t.Commit();
            }

            myTestServices.LogMessage("Adding second population row one role at a time");
            myTestServices.LogValidationErrors("Add a complete FactTypeInstance for Tom, 1 error remaining for 'Dick'");
            using (Transaction t = store.TransactionManager.BeginTransaction("Populate second row"))
            {
                factTypeInstance = new FactTypeInstance(store);
                factTypeInstance.FactType = factType;
                new FactTypeRoleInstance(leftRole.Role, populateMeInstances[1]).FactTypeInstance = factTypeInstance;
                t.Commit();
            }
            myTestServices.LogValidationErrors("Add a partial FactTypeInstance for Dick, 1 error remaining for partial FactTypeInstance");
            using (Transaction t = store.TransactionManager.BeginTransaction("Finish second row populations"))
            {
                new FactTypeRoleInstance(rightRole.Role, v2Instances[0]).FactTypeInstance = factTypeInstance;
                t.Commit();
            }
       }
	}
}