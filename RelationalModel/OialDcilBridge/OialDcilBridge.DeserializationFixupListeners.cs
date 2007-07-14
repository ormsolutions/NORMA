#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
*                                                                          *
* The use and distribution terms for this software are covered by the      *
* Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
* can be found in the file CPL.txt at the root of this distribution.       *
* By using this software in any fashion, you are agreeing to be bound by   *
* the terms of this license.                                               *
*                                                                          *
* You must not remove this notice, or any other, from this software.       *
\**************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using Neumont.Tools.Modeling;
using Neumont.Tools.ORMToORMAbstractionBridge;
using Neumont.Tools.ORMAbstraction;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.RelationalModels.ConceptualDatabase;
using System.Collections.ObjectModel;

namespace Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge
{
	/// <summary>
	/// The public fixup phase for the ORM abstraction bridge model
	/// </summary>
	public enum ORMAbstractionToConceptualDatabaseBridgeDeserializationFixupPhase
	{
		/// <summary>
		/// Validate bridge elements after all core ORM validation is complete
		/// </summary>
		ValidateElements = (int)ORMToORMAbstractionBridgeDeserializationFixupPhase.CreateImplicitElements + 10,
	}
	public partial class ORMAbstractionToConceptualDatabaseBridgeDomainModel : IDeserializationFixupListenerProvider
	{
		#region IDeserializationFixupListenerProvider Implementation
		/// <summary>
		/// Implements <see cref="IDeserializationFixupListenerProvider.DeserializationFixupListenerCollection"/>
		/// </summary>
		protected static IEnumerable<IDeserializationFixupListener> DeserializationFixupListenerCollection
		{
			get
			{
				yield return new GenerateConceptualDatabaseFixupListener();
			}
		}
		IEnumerable<IDeserializationFixupListener> IDeserializationFixupListenerProvider.DeserializationFixupListenerCollection
		{
			get
			{
				return DeserializationFixupListenerCollection;
			}
		}
		/// <summary>
		/// Implements <see cref="IDeserializationFixupListenerProvider.DeserializationFixupPhaseType"/>
		/// </summary>
		protected static Type DeserializationFixupPhaseType
		{
			get
			{
				return typeof(ORMAbstractionToConceptualDatabaseBridgeDeserializationFixupPhase);
			}
		}
		Type IDeserializationFixupListenerProvider.DeserializationFixupPhaseType
		{
			get
			{
				return DeserializationFixupPhaseType;
			}
		}
		#endregion // IDeserializationFixupListenerProvider Implementation
		#region GenerateConceptualDatabaseFixupListener class
		private class GenerateConceptualDatabaseFixupListener : DeserializationFixupListener<AbstractionModel>
		{
			/// <summary>
			/// Create a new fixup listener
			/// </summary>
			public GenerateConceptualDatabaseFixupListener()
				: base((int)ORMAbstractionToConceptualDatabaseBridgeDeserializationFixupPhase.ValidateElements)
			{
			}
			/// <summary>
			/// Verify that an abstraction model has an appropriate conceptual database model and bridge
			/// </summary>
			protected override void ProcessElement(AbstractionModel element, Store store, INotifyElementAdded notifyAdded)
			{
				Schema schema = SchemaIsForAbstractionModel.GetSchema(element);
				if (schema == null)
				{
					// See if we already have a catalog defined
					// UNDONE: Not sure why we even have a catalog in the model, but ConceptualDatabase currently
					// lists it as the root element and we can't serialize without one.
					ReadOnlyCollection<Catalog> catalogs = store.ElementDirectory.FindElements<Catalog>();
					Catalog catalog = null;
					if (catalogs.Count != 0)
					{
						catalog = catalogs[0];
					}
					else
					{
						catalog = new Catalog(store);
						notifyAdded.ElementAdded(catalog);
					}

					// Create the initial schema and notify
					schema = new Schema(
						store,
						new PropertyAssignment[]{
						new PropertyAssignment(Schema.NameDomainPropertyId, element.Name)});
					new SchemaIsForAbstractionModel(schema, element);
					schema.Catalog = catalog;
					notifyAdded.ElementAdded(schema, true);

					// Map all InformationTypeFormats to domains in the schema
					// UNDONE: (Phase 2 when we care about datatypes). There is not currently
					// sufficient information in the oial model to add the required predefineddatatype
					// to a generated domain. Use this as pattern code
					//LinkedElementCollection<Domain> domains = schema.DomainCollection;
					//foreach (InformationTypeFormat itf in element.InformationTypeFormatCollection)
					//{
					//    Domain domain = new Domain(store);
					//    domains.Add(domain);
					//    new DomainIsForInformationTypeFormat(domain, itf);
					//    notifyAdded.ElementAdded(domain, true);
					//}
				}
			}
		}
		#endregion // GenerateConceptualDatabaseFixupListener class
	}
}
