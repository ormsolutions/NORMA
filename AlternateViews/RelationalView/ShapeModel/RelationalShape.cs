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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Shell;
using Neumont.Tools.RelationalModels.ConceptualDatabase;
using Neumont.Tools.ORMToORMAbstractionBridge;
using Neumont.Tools.ORMAbstraction;
using Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge;

namespace Neumont.Tools.ORM.Views.RelationalView
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
		/// DeletingRule: typeof(Neumont.Tools.ORMToORMAbstractionBridge.ConceptTypeIsForObjectType)
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
		/// DeletingRule: typeof(Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.TableIsPrimarilyForConceptType)
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

	partial class RelationalShapeDomainModel : IModelingEventSubscriber
	{
		#region IModelingEventSubscriber Implementation
		/// <summary>
		/// Hack implementation to turn on the FixupDiagram rules before the model loads.
		/// Normally this would be done with a coordinated fixup listener. However, we
		/// have no control over the DSL-generated FixupDiagram rule without jumping through
		/// a lot of hoops, so we do it here, which fires after the rules are created and
		/// before the model loads.
		/// </summary>
		void IModelingEventSubscriber.ManagePreLoadModelingEventHandlers(ModelingEventManager eventManager, bool isReload, EventHandlerAction action)
		{
			if (action == EventHandlerAction.Add)
			{
				Store.RuleManager.EnableRule(typeof(FixUpDiagram));
			}
		}
		void IModelingEventSubscriber.ManagePostLoadModelingEventHandlers(ModelingEventManager eventManager, bool isReload, EventHandlerAction action)
		{
			TableShape.ManageEventHandlers(Store, eventManager, action);
		}
		void IModelingEventSubscriber.ManageSurveyQuestionModelingEventHandlers(ModelingEventManager eventManager, bool isReload, EventHandlerAction action)
		{
		}
		#endregion // IModelingEventSubscriber Implementation
	}
}
