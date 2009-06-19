using System;
using System.Reflection;
using ORMSolutions.ORMArchitectSDK.TestEngine;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using NUnit.Framework;
using NUnitCategory = NUnit.Framework.CategoryAttribute;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge;
namespace RelationalTests.FullRegeneration
{
	/// <summary>
	/// Tests to verify stability of full generating of abstraction
	/// and relational models from an existing ORM file.
	/// </summary>
	[ORMTestFixture]
	[TestFixture(Description="Relational model initial generation")]
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
		#region Relational Load Tests
		/// <summary>
		/// NUnit
		/// </summary>
		[Test(Description = "Relational Load")]
		[NUnitCategory("Relational")]
		[NUnitCategory("FullRegeneration")]
		public void Test1()
		{
			// Forward the call
			Suite.RunNUnitTest(this, myTestServices);
		}
		/// <summary>
		/// Test full regeneration of a medium-sized ORM model. This
		/// is an ORM model with the Abstraction/ConceptualDatabase
		/// extension hand-added to the top of the file. All other
		/// required extensions should load automatically.
		/// </summary>
		[ORMTest("Relational", "FullRegeneration")]
		public void Test1(Store store)
		{
		}
		/// <summary>
		/// NUnit
		/// </summary>
		[Test(Description = "Relational Load")]
		[NUnitCategory("Relational")]
		[NUnitCategory("FullRegeneration")]
		public void Test2()
		{
			// Forward the call
			Suite.RunNUnitTest(this, myTestServices);
		}
		/// <summary>
		/// Test full regeneration of a simple model. This model
		/// used to generate an extra table for the cmValue valueType.
		/// The load file is an ORM model with the Abstraction/ConceptualDatabase
		/// extension hand-added to the top of the file. All other
		/// required extensions should load automatically.
		/// </summary>
		[ORMTest("Relational", "FullRegeneration")]
		public void Test2(Store store)
		{
			myTestServices.Compare(store, (MethodInfo)MethodInfo.GetCurrentMethod(), "WithIndependent");
			ORMModel model = store.ElementDirectory.FindElements<ORMModel>()[0];
			ObjectType objectType = (ObjectType)model.ObjectTypesDictionary.GetElement("SomeLength").FirstElement;
			DomainTypeDescriptor.CreatePropertyDescriptor(objectType, ObjectType.IsIndependentDomainPropertyId).SetValue(objectType, false);
		}
		/// <summary>
		/// NUnit
		/// </summary>
		[Test(Description = "Relational Load")]
		[NUnitCategory("Relational")]
		[NUnitCategory("FullRegeneration")]
		public void Test3()
		{
			// Forward the call
			Suite.RunNUnitTest(this, myTestServices);
		}
		/// <summary>
		/// Test full regeneration of a model featuring non-absorbed composite
		/// preferred identifiers.
		/// </summary>
		[ORMTest("Relational", "FullRegeneration")]
		public void Test3(Store store)
		{
			myTestServices.Compare(store, (MethodInfo)MethodInfo.GetCurrentMethod(), "OriginalOrder");
			ORMModel model = store.ElementDirectory.FindElements<ORMModel>()[0];
			ObjectType objectType = (ObjectType)model.ObjectTypesDictionary.GetElement("A").FirstElement;
			myTestServices.LogMessage("Reorder columns on composite pid with no referenced composite elements");
			using (Transaction t = store.TransactionManager.BeginTransaction("Reorder columns on pid with no referenced composite elements"))
			{
				objectType.PreferredIdentifier.RoleCollection.Move(0, 1);
				t.Commit();
			}
			objectType = (ObjectType)model.ObjectTypesDictionary.GetElement("E").FirstElement;
			myTestServices.Compare(store, (MethodInfo)MethodInfo.GetCurrentMethod(), "AfterReorder");

			myTestServices.LogMessage("Reorder columns on composite pid with referenced composite elements");
			using (Transaction t = store.TransactionManager.BeginTransaction("Reorder columns on pid with no referenced composite elements"))
			{
				objectType.PreferredIdentifier.RoleCollection.Move(0, 1);
				t.Commit();
			}
		}
		/// <summary>
		/// NUnit
		/// </summary>
		[Test(Description = "Relational Load")]
		[NUnitCategory("Relational")]
		[NUnitCategory("FullRegeneration")]
		public void Test4()
		{
			// Forward the call
			Suite.RunNUnitTest(this, myTestServices);
		}
		/// <summary>
		/// Test full regeneration of collapsing object type
		/// with a composite preferred identifier with references
		/// to something other than simple information types.
		/// </summary>
		[ORMTest("Relational", "FullRegeneration")]
		public void Test4(Store store)
		{
		}
		/// <summary>
		/// NUnit
		/// </summary>
		[Test(Description = "Relational Load")]
		[NUnitCategory("Relational")]
		[NUnitCategory("FullRegeneration")]
		public void Test5()
		{
			// Forward the call
			Suite.RunNUnitTest(this, myTestServices);
		}
		/// <summary>
		/// Test full regeneration of collapsing multiple one-many
		/// and unary objectifications into a single table.
		/// </summary>
		[ORMTest("Relational", "FullRegeneration")]
		public void Test5(Store store)
		{
			myTestServices.Compare(store, (MethodInfo)MethodInfo.GetCurrentMethod(), "FullyAbsorbed");

			myTestServices.LogMessage("Separate a one-to-main objectification");
			ORMModel model = store.ElementDirectory.FindElements<ORMModel>()[0];
			ObjectType birthObjectType = (ObjectType)model.ObjectTypesDictionary.GetElement("Birth").FirstElement;
			FactType factTypeToSeparate = birthObjectType.PreferredIdentifier.RoleCollection[0].Proxy.FactType;
			MappingCustomizationModel customizationModel;
			using (Transaction t = store.TransactionManager.BeginTransaction("Separate assimilated objectification"))
			{
				customizationModel = new MappingCustomizationModel(store);
				AssimilationMapping mapping = new AssimilationMapping(store, new PropertyAssignment(AssimilationMapping.AbsorptionChoiceDomainPropertyId, AssimilationAbsorptionChoice.Separate));
				new AssimilationMappingCustomizesFactType(mapping, factTypeToSeparate);
				mapping.Model = customizationModel;
				t.Commit();
			}
			myTestServices.Compare(store, (MethodInfo)MethodInfo.GetCurrentMethod(), "SeparateObjectification");

			myTestServices.LogMessage("Add a longer assimilation chain with a separate end point");
			ObjectType partyObjectType = (ObjectType)model.ObjectTypesDictionary.GetElement("Party").FirstElement;
			AssimilationMapping partyIsThingAssimilationMapping;
			using (Transaction t = store.TransactionManager.BeginTransaction("Longer assimilation chain"))
			{
				partyObjectType.ReferenceModeDisplay = ""; // Using ReferenceModeDisplay instead of ReferenceModeString to automatically kill Party_id
				ObjectType thingObjectType = new ObjectType(store, new PropertyAssignment(ObjectType.NameDomainPropertyId, "Thing"), new PropertyAssignment(ObjectType.IsIndependentDomainPropertyId, true));
				thingObjectType.Model = model;
				thingObjectType.ReferenceModeString = "id";
				SubtypeFact partyIsThingSubtypeFact = SubtypeFact.Create(partyObjectType, thingObjectType);
				partyIsThingAssimilationMapping = new AssimilationMapping(store, new PropertyAssignment(AssimilationMapping.AbsorptionChoiceDomainPropertyId, AssimilationAbsorptionChoice.Separate));
				new AssimilationMappingCustomizesFactType(partyIsThingAssimilationMapping, partyIsThingSubtypeFact);
				partyIsThingAssimilationMapping.Model = customizationModel;
				t.Commit();
			}

			myTestServices.Compare(store, (MethodInfo)MethodInfo.GetCurrentMethod(), "SeparateRemoteSupertype");

			myTestServices.LogMessage("Remove the remote separation");
			using (Transaction t = store.TransactionManager.BeginTransaction(""))
			{
				partyIsThingAssimilationMapping.AbsorptionChoice = AssimilationAbsorptionChoice.Absorb;
				t.Commit();
			}
		}
		/// <summary>
		/// NUnit
		/// </summary>
		[Test(Description = "Relational Load")]
		[NUnitCategory("Relational")]
		[NUnitCategory("FullRegeneration")]
		public void Test6()
		{
			// Forward the call
			Suite.RunNUnitTest(this, myTestServices);
		}
		/// <summary>
		/// Test full regeneration of a concept type relation
		/// to an absorbed subtype with its own identifier.
		/// </summary>
		[ORMTest("Relational", "FullRegeneration")]
		public void Test6(Store store)
		{
		}
		/// <summary>
		/// NUnit
		/// </summary>
		[Test(Description = "Relational Load")]
		[NUnitCategory("Relational")]
		[NUnitCategory("FullRegeneration")]
		public void Test7()
		{
			// Forward the call
			Suite.RunNUnitTest(this, myTestServices);
		}
		/// <summary>
		/// Test full regeneration of a subtyping hierarchy with
		/// identifiers at multiple levels and concept type
		/// relations to the concept types at each level.
		/// </summary>
		[ORMTest("Relational", "FullRegeneration")]
		public void Test7(Store store)
		{
		}
		/// <summary>
		/// NUnit
		/// </summary>
		[Test(Description = "Relational Load")]
		[NUnitCategory("Relational")]
		[NUnitCategory("FullRegeneration")]
		public void Test8()
		{
			// Forward the call
			Suite.RunNUnitTest(this, myTestServices);
		}
		/// <summary>
		/// Test full regeneration of a separated concept type
		/// assimilation that is preferred for the parent
		/// concept type.
		/// </summary>
		[ORMTest("Relational", "FullRegeneration")]
		public void Test8(Store store)
		{
		}
		/// <summary>
		/// NUnit
		/// </summary>
		[Test(Description = "Relational Load")]
		[NUnitCategory("Relational")]
		[NUnitCategory("FullRegeneration")]
		public void Test9()
		{
			// Forward the call
			Suite.RunNUnitTest(this, myTestServices);
		}
		/// <summary>
		/// Test full regeneration of a concept type relation
		/// to a concept type that inherits its preferred
		/// identifier and is absorbed into a supertype
		/// table through a multi-step assimilation path.
		/// </summary>
		[ORMTest("Relational", "FullRegeneration")]
		public void Test9(Store store)
		{
		}
		#endregion // Relational Load tests
	}
}
