#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Shell;
using ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase;
using ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge;
using ORMSolutions.ORMArchitect.ORMAbstraction;
using ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge;

namespace ORMSolutions.ORMArchitect.Views.RelationalView
{
	/// <summary>
	/// Represents the framework for the relational schema view of the ORM Diagram.
	/// </summary>
	partial class RelationalDiagram
	{
		#region Validation Rules
		/// <summary>
		/// A key that references a dictionary of table positions in the current transaction context.
		/// </summary>
		public static readonly object TablePositionDictionaryKey = new object();
		/// <summary>
		/// DeletingRule: typeof(ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.ConceptTypeIsForObjectType)
		/// Cache the position of the <see cref="TableShape"/> corresponding to the object type being deleted
		/// </summary>
		private static void ConceptTypeDetachingFromObjectTypeRule(ElementDeletingEventArgs e)
		{
			ConceptTypeIsForObjectType link = (ConceptTypeIsForObjectType)e.ModelElement;
			ObjectType objectType = link.ObjectType;
			Table table;
			if (!objectType.IsDeleting &&
				null != (table = TableIsPrimarilyForConceptType.GetTable(link.ConceptType)))
			{
				RememberTableShapeLocations(objectType, table);
			}
		}
		/// <summary>
		/// DeletingRule: typeof(ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.TableIsPrimarilyForConceptType)
		/// Cache the position of the <see cref="TableShape"/> corresponding to the object type being deleted
		/// </summary>
		private static void ConceptTypeDetachingFromTableRule(ElementDeletingEventArgs e)
		{
			TableIsPrimarilyForConceptType link = (TableIsPrimarilyForConceptType)e.ModelElement;
			ObjectType objectType;
			if (null != (objectType = ConceptTypeIsForObjectType.GetObjectType(link.ConceptType)) &&
				!objectType.IsDeleting)
			{
				RememberTableShapeLocations(objectType, link.Table);
			}
		}
		/// <summary>
		/// Cache the location of any table shape associated with the provided <see cref="ObjectType"/> and <see cref="Table"/>
		/// </summary>
		private static void RememberTableShapeLocations(ObjectType objectType, Table table)
		{
			foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(table))
			{
				TableShape shape = pel as TableShape;
				if (pel != null)
				{
					Dictionary<object, object> context = objectType.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
					object tablePositionsObject;
					Dictionary<Guid, PointD> tablePositions;
					if (!context.TryGetValue(TablePositionDictionaryKey, out tablePositionsObject) ||
						(tablePositions = tablePositionsObject as Dictionary<Guid, PointD>) == null)
					{
						context[TablePositionDictionaryKey] = tablePositions = new Dictionary<Guid, PointD>();
					}
					tablePositions[objectType.Id] = shape.Location;
				}
			}
		}
		#endregion // Validation Rules
	}

	partial class RelationalShapeDomainModel : IModelingEventSubscriber, IRegisterSignalChanges
	{
		#region IModelingEventSubscriber Implementation
		void IModelingEventSubscriber.ManageModelingEventHandlers(ModelingEventManager eventManager, EventSubscriberReasons reasons, EventHandlerAction action)
		{
			if (action == EventHandlerAction.Add && 0 != (reasons & EventSubscriberReasons.DocumentLoading))
			{
				// Hack implementation to turn on the FixupDiagram rules before the model loads.
				// Normally this would be done with a coordinated fixup listener. However, we
				// have no control over the DSL-generated FixupDiagram rule without jumping through
				// a lot of hoops, so we do it here, which fires after the rules are created and
				// before the model loads.
				Store.RuleManager.EnableRule(typeof(FixUpDiagram));
			}
			if (0 != (reasons & EventSubscriberReasons.DocumentLoaded))
			{
				Store store = Store;
				TableShape.ManageEventHandlers(store, eventManager, action);
				RelationalDiagram.ManageEventHandlers(store, eventManager, action);
			}
		}
		#endregion // IModelingEventSubscriber Implementation
		#region IRegisterSignalChanges Implementation
		/// <summary>
		/// Implements <see cref="IRegisterSignalChanges.GetSignalPropertyChanges"/>
		/// </summary>
		protected IEnumerable<KeyValuePair<Guid, Predicate<ElementPropertyChangedEventArgs>>> GetSignalPropertyChanges()
		{
			// Our UpdateCounter properties are also signals that should not trigger a user-visible change.
			yield return new KeyValuePair<Guid, Predicate<ElementPropertyChangedEventArgs>>(TableShape.UpdateCounterDomainPropertyId, null);
		}
		IEnumerable<KeyValuePair<Guid, Predicate<ElementPropertyChangedEventArgs>>> IRegisterSignalChanges.GetSignalPropertyChanges()
		{
			return GetSignalPropertyChanges();
		}
		#endregion // IRegisterSignalChanges Implementation
	}
}
