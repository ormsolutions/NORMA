using System;
using System.Reflection;
using Neumont.Tools.ORM.SDK.TestEngine;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using NUnit.Framework;
using NUnitCategory = NUnit.Framework.CategoryAttribute;
//using Neumont.Tools.ORM.
using Neumont.Tools.Modeling.Design;
namespace TestSample.Sample
{
	/// <summary>
	/// A sample class to demonstrate how to run a test. The class runs
	/// using both the ORMTestDriver console application and the NUnit
	/// testing tools. The Xml output is easier to see and analyze with
	/// the ORMTestDriver application, but there are significantly more tools
	/// for processing NUnit output.
	/// </summary>
	[ORMTestFixture]
	[TestFixture(Description="Sample ORM Test Cases")]
	public class Tests
	{
		#region Boilerplate code for ORMTestDriver integration
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
		#endregion // Boilerplate code for ORMTestDriver integration
		#region Additional boilerplate code for NUnit integration
		public Tests() : this(Suite.CreateServices()) { }
		#endregion // Additional boilerplate code for NUnit integration
		#region Sample test - missing internal uniqueness constraint
		/// <summary>
		/// A sample NUnit test method. Automatically forwards
		/// to test of the same name with a loaded store.
		/// </summary>
		[Test(Description = "Sample Test")]
		[NUnitCategory("Sample")]
		[NUnitCategory("InternalConstraints")]
		public void Test1()
		{
			// Forward the call
			Suite.RunNUnitTest(this, myTestServices);
		}
		/// <summary>
		/// A sample test method.
		/// </summary>
		/// <param name="store">A preloaded store. All test methods must have this signature.</param>
		[ORMTest("Sample", "InternalConstraints")]
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
			//ORMModel model = (ORMModel)store.ElementDirectory.GetElement(ORMModel.DomainClassId);
			ORMModel model = store.ElementDirectory.FindElements<ORMModel>()[0];
			FactType fact = (FactType)store.ElementDirectory.GetElement(new Guid("CB33E377-9ADC-46BA-8E1B-24AE46BC0854"));
			Role role = fact.RoleCollection[1].Role;

			// At this point, we either need to open a transaction on the store with
			// using (Transaction t = store.TransactionManager.BeginTransaction("Transaction name"))
			// or we grab the property descriptor, which emulates setting the property through the
			// property grid and pushes a transaction for us.
			
			DomainTypeDescriptor.CreatePropertyDescriptor(role, Role.MultiplicityDomainPropertyId).SetValue(role, RoleMultiplicity.ExactlyOne);

			// Turn the following code back on to demonstrate a comparison failure
			
					// ~~ outdated code, do not turn on ~~//
			//fact = (FactType)model.FactTypesDictionary.GetElement("FactType2").SingleElement;
			//fact.CreatePropertyDescriptor(store.MetaDataDirectory.FindMetaAttribute(NamedElement.NameMetaAttributeGuid), fact).SetValue(fact, "changeFactType2");
					// ~~ /outdated code, do not turn on ~~//
			
			
			// After the method exits, the Compare and LogValidationErrors methods will be run automatically against
			// the test service. The expected results for the Compare are in Tests.Test1.Compare.orm. You can also
			// compare at intermediate stages by explicitly running the IORMToolTestServices.Compare function, generally
			// with a reference name to distinguish the intermediate stages from the automatic comparison.
		}
		#endregion // Sample test - missing internal uniqueness constraint
		#region constraint duplication error
		[Test(Description = "Clear implied internal uniqueness")]
		[NUnitCategory("InternalConstraints")]
		public void Test2()
		{
			// Forward the call
			Suite.RunNUnitTest(this, myTestServices);
		}
		[ORMTest("InternalConstraints")]
		public void Test2(Store store)
		{
			myTestServices.LogValidationErrors("Before constraint duplication/implication repair");

			// Find the fact that that needs fixing and repair it
			ORMModel model = store.ElementDirectory.FindElements<ORMModel>()[0];
			FactType fact = (FactType)store.ElementDirectory.GetElement(new Guid("009787C8-37C8-43EB-933A-7C5B469D1310"));
			fact.RemoveImpliedInternalUniquenessConstraints();
			myTestServices.LogValidationErrors("After constraint duplication/implication repair");
		}
		#endregion //constraint duplication error
	}
}
