using System;
using System.Collections.Generic;
using System.Reflection;
using ORMSolutions.ORMArchitectSDK.TestEngine;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using Microsoft.VisualStudio.Modeling;
using NUnit.Framework;
using NUnitCategory = NUnit.Framework.CategoryAttribute;
using ORMSolutions.ORMArchitect.Framework.Design;

namespace TestSample.SubtypeTests
{
	[ORMTestFixture]
	[TestFixture(Description = "Test the NMinusOneError")]
	public class PreferredPathTests
	{
		#region Boilerplate code
		private IORMToolServices myServices;
		private IORMToolTestServices myTestServices;
		public PreferredPathTests(IORMToolServices services)
		{
			myServices = services;
			myTestServices = (IORMToolTestServices)services.ServiceProvider.GetService(typeof(IORMToolTestServices));
		}
		public PreferredPathTests() : this(Suite.CreateServices()) { }
		#endregion // Boilerplate code
		[Test(Description = "Load/Save with incomplete preferred identification path")]
		[NUnitCategory("Subtype")]
		[NUnitCategory("PreferredIdentificationPath")]
		public void AddPreferredLoadTest1()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}
		/// <summary>
		/// Load and save an ORM file with incomplete preferred identification paths
		/// on a subtype graph. Verify that all implied paths automatically populate.
		/// </summary>
		[ORMTest("Subtype", "PreferredIdentificationPath")]
		public void AddPreferredLoadTest1(Store store)
		{
			// Body intentionally empty
		}
		[Test(Description = "Load/Save with inconsistent preferred identification path")]
		[NUnitCategory("Subtype")]
		[NUnitCategory("PreferredIdentificationPath")]
		public void ClearPreferredLoadTest1()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}
		/// <summary>
		/// Load and save an ORM file with inconsistent preferred identification paths
		/// on a subtype graph. Verify that all ambiguous downstream paths also turn off.
		/// </summary>
		[ORMTest("Subtype", "PreferredIdentificationPath")]
		public void ClearPreferredLoadTest1(Store store)
		{
			// Body intentionally empty
		}
		[Test(Description = "Validate patterns on the SubtypeFact.ProvidesPreferredIdentifier property")]
		[NUnitCategory("Subtype")]
		[NUnitCategory("PreferredIdentificationPath")]
		public void ResolvePreferredTest1()
		{
			Suite.RunNUnitTest(this, myTestServices);
		}
		/// <summary>
		/// Validate patterns on the SubtypeFact.ProvidesPreferredIdentifier property
		/// </summary>
		[ORMTest("Subtype", "PreferredIdentificationPath")]
		public void ResolvePreferredTest1(Store store)
		{
			IElementDirectory directory = store.ElementDirectory;
			SubtypeFact FiveToTwo = (SubtypeFact)directory.GetElement(new Guid("9B85CC3F-FAA1-485B-A6CA-DE65165C6AE5"));
			SubtypeFact FiveToThree = (SubtypeFact)directory.GetElement(new Guid("C48F356A-DC20-4CD8-AD6A-EE4518622FFB"));
			SubtypeFact SixToFive = (SubtypeFact)directory.GetElement(new Guid("321CD412-8DAA-4D35-8D4E-D8D5915F07E0"));
			SubtypeFact SixToSeven = (SubtypeFact)directory.GetElement(new Guid("D6A4C2F2-3F99-4E2F-AB2F-407E03AB6F42"));
			SubtypeFact FiveToFour = (SubtypeFact)directory.GetElement(new Guid("DE1FC770-19A5-4CA9-AEF7-CE1204D80270"));
			SubtypeFact SevenToFour = (SubtypeFact)directory.GetElement(new Guid("71D29425-481E-445B-8F8A-63E9F29F761A"));
			ORMModel model = FiveToTwo.Model;
			RoleValueConstraint valueConstraint = (RoleValueConstraint)model.ConstraintsDictionary.GetElement("RoleValueConstraint1").SingleElement;

			myTestServices.LogValidationErrors("Expected three initial errors");
			
			myTestServices.LogMessage("Scenario 1: Make 5->2 preferred path, sibling and downstream subtypes should all be preferred");
			DomainTypeDescriptor.CreatePropertyDescriptor(FiveToTwo, SubtypeFact.ProvidesPreferredIdentifierDomainPropertyId).SetValue(FiveToTwo, true);
			myTestServices.LogValidationErrors("No errors expected");
			myTestServices.LogMessage("5->4, 6->5, 6->7 are on the preferred path: " + (FiveToFour.ProvidesPreferredIdentifier && SixToFive.ProvidesPreferredIdentifier && SixToSeven.ProvidesPreferredIdentifier).ToString());
			myTestServices.LogMessage("ValueConstraint should be a string type: " + valueConstraint.Text);

			myTestServices.LogMessage("Scenario 2: Make 5->3 preferred path, downstream subtypes are no ambiguous");
			DomainTypeDescriptor.CreatePropertyDescriptor(FiveToThree, SubtypeFact.ProvidesPreferredIdentifierDomainPropertyId).SetValue(FiveToThree, true);
			myTestServices.LogValidationErrors("Two errors expected");
			myTestServices.LogMessage("5->2, 5->4, 6->5, 6->7 are not preferred: " + (!(FiveToTwo.ProvidesPreferredIdentifier || FiveToFour.ProvidesPreferredIdentifier || SixToFive.ProvidesPreferredIdentifier || SixToSeven.ProvidesPreferredIdentifier)).ToString());

			myTestServices.LogMessage("Scenario 3: Make 6->5 preferred path");
			DomainTypeDescriptor.CreatePropertyDescriptor(SixToFive, SubtypeFact.ProvidesPreferredIdentifierDomainPropertyId).SetValue(SixToFive, true);
			myTestServices.LogValidationErrors("No errors expected");
			myTestServices.LogMessage("6->7 is not on the preferred path: " + (!SixToSeven.ProvidesPreferredIdentifier).ToString());
			myTestServices.LogMessage("ValueConstraint should be a number type: " + valueConstraint.Text);

			myTestServices.LogMessage("Scenario 4: Make 6->7 preferred path");
			DomainTypeDescriptor.CreatePropertyDescriptor(SixToSeven, SubtypeFact.ProvidesPreferredIdentifierDomainPropertyId).SetValue(SixToSeven, true);
			myTestServices.LogValidationErrors("No errors expected");
			myTestServices.LogMessage("6->5 is not on the preferred path: " + (!SixToFive.ProvidesPreferredIdentifier).ToString());
			myTestServices.LogMessage("ValueConstraint should be a string type: " + valueConstraint.Text);

			myTestServices.LogMessage("Scenario 5: Delete 7->4 to break subtype structure");
			using (Transaction t = store.TransactionManager.BeginTransaction("Break graph"))
			{
				SevenToFour.Delete();
				t.Commit();
			}
			myTestServices.LogValidationErrors("Expecting invalid graph, detached valuetype, and no reference scheme errors");

			myTestServices.LogMessage("Scenario 6: Delete 6->5 to make graph valid");
			using (Transaction t = store.TransactionManager.BeginTransaction("Graph OK"))
			{
				SixToFive.Delete();
				t.Commit();
			}
			myTestServices.LogValidationErrors("Expecting detached valuetype and no reference scheme errors");

			myTestServices.LogMessage("Scenario 7: Set identifier on 7 to clear errors");
			using (Transaction t = store.TransactionManager.BeginTransaction("Clear errors"))
			{
				SixToSeven.Supertype.ReferenceModeString = "id";
				t.Commit();
			}
			myTestServices.LogValidationErrors("No errors expected");

			myTestServices.LogValidationErrors("Scenario 8: (Undo scenarios 5-7) Make 5->4 preferred path and give 4 an explicit id in one transaction");
			store.UndoManager.Undo();
			store.UndoManager.Undo();
			store.UndoManager.Undo();
			using (Transaction t = store.TransactionManager.BeginTransaction("Change id and make preferred in one transaction"))
			{
				// Note that doing this in the opposite order in two transactions has the same result
				FiveToFour.ProvidesPreferredIdentifier = true;
				FiveToFour.Supertype.ReferenceModeString = "id";
				t.Commit();
			}
			myTestServices.LogValidationErrors("No errors expected");

			myTestServices.LogValidationErrors("Scenario 9: (Undo scenario 8) Make 5->4 preferred path then give 4 an explicit id to make 5 and 6 ambiguous");
			store.UndoManager.Undo();
			using (Transaction t = store.TransactionManager.BeginTransaction("Change preferred path"))
			{
				FiveToFour.ProvidesPreferredIdentifier = true;
				t.Commit();
			}
			using (Transaction t = store.TransactionManager.BeginTransaction("Introduce id that interferes with split identifier path"))
			{
				FiveToFour.Supertype.ReferenceModeString = "id";
				t.Commit();
			}
		}
	}
}