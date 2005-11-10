using System;
using System.Reflection;
using Neumont.Tools.ORM.SDK.TestEngine;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;

namespace TestSample
{
	/// <summary>
	/// A sample class to demonstrate how to run a test
	/// </summary>
	[Tests]
	public class Tests
	{
		private IORMToolServices myServices;
		private IORMToolTestServices myTestServices;
		public Tests(IORMToolServices services)
		{
			// Cache the services for future use
			myServices = services;
			// The services from the test tool can be retrieved
			// from the code services service provider.
			myTestServices = (IORMToolTestServices)services.ServiceProvider.GetService(typeof(IORMToolTestServices));
		}
		/// <summary>
		/// A sample test method.
		/// </summary>
		/// <param name="store">A preloaded store. All test methods must have this signature.</param>
		[Test("Sample", "InternalConstraints")]
		public void Test1(Store store)
		{
			// The store has been preloaded at this point with the .orm file specified in the
			// Tests.Test1.Compare.orm resource embedded into this project. If an embedded resource
			// is not found with this name, then a new orm file is created by loading the default
			// 'ORM Model File' template. This creates a model called 'NewModel' with a single diagram
			// attached to it.
			
			// Dump a report of the current model validation errors. The loaded file is missing an
			// internal constraint on a fact type, so we expect errors
			myTestServices.LogValidationErrors("Before constraint repair");

			// Find the fact that that needs fixing and repair it
			ORMModel model = (ORMModel)store.ElementDirectory.GetElements(ORMModel.MetaClassGuid)[0];
			FactType fact = (FactType)model.FactTypesDictionary.GetElement("TestFact").SingleElement;
			Role role = fact.RoleCollection[1];

			// At this point, we either need to open a transaction on the store with
			// using (Transaction t = store.TransactionManager.BeginTransaction("Transaction name"))
			// or we grab the property descriptor, which emulates setting the property through the
			// property grid and pushes a transaction for us.
			role.CreatePropertyDescriptor(store.MetaDataDirectory.FindMetaAttribute(Role.MultiplicityMetaAttributeGuid), role).SetValue(role, RoleMultiplicity.ExactlyOne);

			// Turn the following code back on to demonstrate a comparison failure
			//fact = (FactType)model.FactTypesDictionary.GetElement("FactType2").SingleElement;
			//fact.CreatePropertyDescriptor(store.MetaDataDirectory.FindMetaAttribute(NamedElement.NameMetaAttributeGuid), fact).SetValue(fact, "changeFactType2");

			// After the method exits, the Compare and LogValidationErrors methods will be run automatically against
			// the test service. The expected results for the Compare are in Tests.Test1.Compare.orm. You can also
			// compare at intermediate stages by explicitly running the IORMToolTestServices.Compare function, generally
			// with a reference name to distinguish the intermediate stages from the automatic comparison.
		}


		[Test("Sample", "InternalConstraints")]
		public void Test2(Store store)
		{
			myTestServices.LogValidationErrors("Before constraint duplication/implication repair");

			// Find the fact that that needs fixing and repair it
			ORMModel model = (ORMModel)store.ElementDirectory.GetElements(ORMModel.MetaClassGuid)[0];
			FactType fact = (FactType)model.FactTypesDictionary.GetElement("FactType4").SingleElement;
			fact.RemoveImpliedInternalUniquenessConstraints();
			myTestServices.LogValidationErrors("After constraint duplication/implication repair");
			//Role role = fact.RoleCollection[1];
			//ImpliedInternalUniquenessConstraintError impError1 = fact.ImpliedInternalUniquenessConstraintError;
			store.UndoManager.Undo();
			store.UndoManager.Redo();




			// After the method exits, the Compare and LogValidationErrors methods will be run automatically against
			// the test service. The expected results for the Compare are in Tests.Test1.Compare.orm. You can also
			// compare at intermediate stages by explicitly running the IORMToolTestServices.Compare function, generally
			// with a reference name to distinguish the intermediate stages from the automatic comparison.
		}

	}
}
