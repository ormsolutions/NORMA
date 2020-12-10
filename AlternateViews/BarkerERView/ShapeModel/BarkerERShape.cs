#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright � Neumont University. All rights reserved.                     *
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
using ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge;
using ORMSolutions.ORMArchitect.ORMAbstraction;
using ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker;
using ORMSolutions.ORMArchitect.ORMAbstractionToBarkerERBridge;

namespace ORMSolutions.ORMArchitect.Views.BarkerERView
{
	/// <summary>
	/// Represents the framework for the Barker ER view of the ORM Diagram.
	/// </summary>
	partial class BarkerERDiagram
	{
		#region Validation Rules
		/// <summary>
		/// A key that references a dictionary of table positions in the current transaction context.
		/// </summary>
		public static readonly object BarkerEntityPositionDictionaryKey = new object();
		/// <summary>
		/// DeletingRule: typeof(ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.ConceptTypeIsForObjectType)
		/// Cache the position of the <see cref="BarkerEntityShape"/> corresponding to the object type being deleted
		/// </summary>
		private static void ConceptTypeDetachingFromObjectTypeRule(ElementDeletingEventArgs e)
		{
			ConceptTypeIsForObjectType link = (ConceptTypeIsForObjectType)e.ModelElement;
			ObjectType objectType = link.ObjectType;
			EntityType barkerEntity;
			if (!objectType.IsDeleting &&
				null != (barkerEntity = EntityTypeIsPrimarilyForConceptType.GetEntityType(link.ConceptType)))
			{
				RememberBarkerEntityShapeLocations(objectType, barkerEntity);
			}
		}
		/// <summary>
		/// DeletingRule: typeof(ORMSolutions.ORMArchitect.ORMAbstractionToBarkerERBridge.EntityTypeIsPrimarilyForConceptType)
		/// Cache the position of the <see cref="BarkerEntityShape"/> corresponding to the object type being deleted
		/// </summary>
		private static void ConceptTypeDetachingFromEntityTypeRule(ElementDeletingEventArgs e)
		{
			EntityTypeIsPrimarilyForConceptType link = (EntityTypeIsPrimarilyForConceptType)e.ModelElement;
			ObjectType objectType;
			if (null != (objectType = ConceptTypeIsForObjectType.GetObjectType(link.ConceptType)) &&
				!objectType.IsDeleting)
			{
				RememberBarkerEntityShapeLocations(objectType, link.EntityType);
			}
		}
		/// <summary>
		/// Cache the location of any entity shape associated with the provided <see cref="ObjectType"/> and <see cref="EntityType"/>
		/// </summary>
		private static void RememberBarkerEntityShapeLocations(ObjectType objectType, EntityType entity)
		{
			foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(entity))
			{
				BarkerEntityShape shape = pel as BarkerEntityShape;
				if (pel != null)
				{
					Dictionary<object, object> context = objectType.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
					object tablePositionsObject;
					Dictionary<Guid, PointD> barkerEntityPositions;
					if (!context.TryGetValue(BarkerEntityPositionDictionaryKey, out tablePositionsObject) ||
						(barkerEntityPositions = tablePositionsObject as Dictionary<Guid, PointD>) == null)
					{
						context[BarkerEntityPositionDictionaryKey] = barkerEntityPositions = new Dictionary<Guid, PointD>();
					}
					barkerEntityPositions[objectType.Id] = shape.Location;
				}
			}
		}
		#endregion // Validation Rules
	}

	[ORMSolutions.ORMArchitect.Core.Load.NORMAExtensionLoadKey(
#if NORMA_Official
		"HCTAvwleAvA/D3wlfsONEUu+0UNEUXRfS8g/RhxEgxRcKS3nk4eAji98Gl/S3bv2JxQSHcwThy2lM6T/qhkdWA=="
#else
		"i4ZTtbpPyWFsaqPS3D+eD1ZioxOaKSSyJzcfRhC/HeZ/MeJOFcJNuJrnq9Yz1W6GNaNRXHdcn0z+qZqU+6gaCw=="
#endif
	)]
	partial class BarkerERShapeDomainModel : IModelingEventSubscriber, IRegisterSignalChanges
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
				BarkerEntityShape.ManageEventHandlers(store, eventManager, action);
				BarkerERDiagram.ManageEventHandlers(store, eventManager, action);
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
			yield return new KeyValuePair<Guid, Predicate<ElementPropertyChangedEventArgs>>(BarkerEntityShape.UpdateCounterDomainPropertyId, null);
		}
		IEnumerable<KeyValuePair<Guid, Predicate<ElementPropertyChangedEventArgs>>> IRegisterSignalChanges.GetSignalPropertyChanges()
		{
			return GetSignalPropertyChanges();
		}
		#endregion // IRegisterSignalChanges Implementation
	}
}
