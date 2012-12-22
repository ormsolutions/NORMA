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
using ORMSolutions.ORMArchitect.ORMAbstraction;
using ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge;
using ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker;
using ORMSolutions.ORMArchitect.ORMAbstractionToBarkerERBridge;
using Barker = ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker;

namespace ORMSolutions.ORMArchitect.Views.BarkerERView
{
	[DiagramMenuDisplay(DiagramMenuDisplayOptions.BlockRename | DiagramMenuDisplayOptions.Required, typeof(BarkerERDiagram), BarkerERDiagram.NameResourceName, "Diagram.TabImage", "Diagram.BrowserImage", NestedDiagramInitializerTypeName="DiagramInitializer")]
	partial class BarkerERDiagram
	{
		#region DiagramInitializer class
		/// <summary>
		/// Perform custom initialization for newly loaded or freshly created diagram.
		/// </summary>
		private class DiagramInitializer : IDiagramInitialization
		{
			#region IDiagramInitialization Implementation
			bool IDiagramInitialization.CreateRequiredDiagrams(Store store)
			{
				// Use the default implementation: create a new diagram of the given type
				// and call InitializeDiagram.
				return false;
			}
			void IDiagramInitialization.InitializeDiagram(Diagram diagram)
			{
				if (null == diagram.Subject)
				{
					Store store = diagram.Store;
					ReadOnlyCollection<BarkerErModel> barkerModels = store.ElementDirectory.FindElements<BarkerErModel>(false);
					if (barkerModels.Count != 0)
					{
						diagram.Associate(barkerModels[0]);
					}
				}
			}
			#endregion // IDiagramInitialization Implementation
		}
		#endregion // DiagramInitializer class
		private const string NameResourceName = "Diagram.MenuDisplayName";

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public BarkerERDiagram(Store store, params PropertyAssignment[] propertyAssignments)
			: this(store != null ? store.DefaultPartition : null, propertyAssignments)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public BarkerERDiagram(Partition partition, params PropertyAssignment[] propertyAssignments)
			: base(partition, propertyAssignments)
		{
			this.Name = ORMSolutions.ORMArchitect.Framework.Design.ResourceAccessor<BarkerERDiagram>.ResourceManager.GetString(NameResourceName);
			NodeToNodeClearance = new SizeD(1.75, 1.75);
		}
		/// <summary>
		/// Customize childshape create to set an initial location for a <see cref="BarkerEntityShape"/>
		/// </summary>
		protected override ShapeElement CreateChildShape(ModelElement element)
		{
			object barkerEntityPositionsObject;
			Dictionary<Guid, PointD> barkerEntityPositions;
			PointD initialLocation;
			BarkerErModelContainsBinaryAssociation modelContainsAssociation;
			EntityType barkerEntity;
			ConceptType conceptType;
			ObjectType objectType;
			if (null != (barkerEntity = element as EntityType))
			{
				if (element.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.TryGetValue(BarkerEntityPositionDictionaryKey, out barkerEntityPositionsObject) &&
					null != (barkerEntityPositions = barkerEntityPositionsObject as Dictionary<Guid, PointD>) &&
					null != (conceptType = EntityTypeIsPrimarilyForConceptType.GetConceptType(barkerEntity)) &&
					null != (objectType = ConceptTypeIsForObjectType.GetObjectType(conceptType)) &&
					barkerEntityPositions.TryGetValue(objectType.Id, out initialLocation))
				{
					return new BarkerEntityShape(
						this.Partition,
						new PropertyAssignment(BarkerEntityShape.AbsoluteBoundsDomainPropertyId, new RectangleD(initialLocation, new SizeD(1, 0.3))));
				}
			}
			else if (null != (modelContainsAssociation = element as BarkerErModelContainsBinaryAssociation))
			{
				return new AssociationConnector(Partition);
			}
			return base.CreateChildShape(element);
		}
		/// <summary>
		/// Stop all auto shape selection on transaction commit except when
		/// the item is being dropped.
		/// </summary>
		public override IList FixUpDiagramSelection(ShapeElement newChildShape)
		{
			if (DropTargetContext.HasDropTargetContext(Store.TransactionManager.CurrentTransaction))
			{
				return base.FixUpDiagramSelection(newChildShape);
			}
			return null;
		}
		/// <summary>
		/// ChangeRule: typeof(BarkerERDiagram)
		/// Disallows changing the name of the Barker ER Diagram
		/// Changes the name of the <see cref="BarkerERDiagram"/> to
		/// its default name if changed by a user.
		/// </summary>
		private static void NameChangedRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeId = e.DomainProperty.Id;
			if (attributeId == Diagram.NameDomainPropertyId)
			{
				BarkerERDiagram diagram = e.ModelElement as BarkerERDiagram;
				string name = ORMSolutions.ORMArchitect.Framework.Design.ResourceAccessor<BarkerERDiagram>.ResourceManager.GetString(NameResourceName);
				if (diagram != null && diagram.Name != name)
				{
					diagram.Name = name;
				}
			}
		}
		/// <summary>
		/// ChangeRule: typeof(ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker.Attribute)
		/// Update Barker entity size when <see cref="Barker.Attribute.IsMandatory"/> or <see cref="Barker.Attribute.Name"/> changes.
		/// </summary>
		private static void DisplayAttributePropertyChangedRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeId = e.DomainProperty.Id;
			if (attributeId == Barker.Attribute.IsMandatoryDomainPropertyId ||
				attributeId == Barker.Attribute.NameDomainPropertyId)
			{
				UpdateEntityPresentationSize(((Barker.Attribute)e.ModelElement).EntityType);
			}
		}
		/// <summary>
		/// ChangeRule: typeof(ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker.EntityType)
		/// Update table size when <see cref="EntityType.Name"/> changes.
		/// </summary>
		private static void DisplayEntityTypePropertyChangedRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeId = e.DomainProperty.Id;
			if (attributeId == EntityType.NameDomainPropertyId)
			{
				UpdateEntityPresentationSize((EntityType)e.ModelElement);
			}
		}
		private static void UpdateEntityPresentationSize(EntityType table)
		{
			if (table != null)
			{
				foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(table))
				{
					BarkerEntityShape shape;
					AttributeElementListCompartment compartment;
					if (null != (shape = pel as BarkerEntityShape) &&
						null != (compartment = shape.FindCompartment("AttributesCompartment") as AttributeElementListCompartment))
					{
						compartment.InvalidateOrUpdateSize();
					}
				}
			}
		}
		private static void UpdateEntitiesForConceptType(ConceptType conceptType, Predicate<ConceptType> conceptTypeFilter, Predicate<EntityType> entityFilter)
		{
			EntityType primaryEntity = EntityTypeIsPrimarilyForConceptType.GetEntityType(conceptType);
			if (primaryEntity != null && (entityFilter == null || !entityFilter(primaryEntity)))
			{
				UpdateEntityPresentationSize(primaryEntity);
			}
			Predicate<ConceptType> recurseConceptTypeFilter =
				delegate(ConceptType testConceptType)
				{
					return testConceptType == conceptType ||
						(conceptTypeFilter != null && conceptTypeFilter(testConceptType));
				};
			Predicate<EntityType> recurseEntityFilter =
				delegate(EntityType testEntity)
				{
					return testEntity == primaryEntity;
				};
			foreach (ConceptType relatingConceptType in ConceptTypeRelatesToConceptType.GetRelatingConceptTypeCollection(conceptType))
			{
				if (relatingConceptType == conceptType || (conceptTypeFilter != null && conceptTypeFilter(relatingConceptType)))
				{
					continue;
				}
				UpdateEntitiesForConceptType(
					relatingConceptType,
					recurseConceptTypeFilter,
					recurseEntityFilter);
			}
			foreach (ConceptType assimilatedConceptType in ConceptTypeAssimilatesConceptType.GetAssimilatedConceptTypeCollection(conceptType))
			{
				if (assimilatedConceptType == conceptType || (conceptTypeFilter != null && conceptTypeFilter(assimilatedConceptType)))
				{
					continue;
				}
				UpdateEntitiesForConceptType(
					assimilatedConceptType,
					recurseConceptTypeFilter,
					recurseEntityFilter);
			}
		}
		/// <summary>
		/// AddRule: typeof(ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker.BarkerErModelContainsBinaryAssociation), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
		/// Add a <see cref="AssociationConnector"/> to the diagram
		/// </summary>
		private static void BinaryAssociationAddedRule(ElementAddedEventArgs e)
		{
			BarkerErModelContainsBinaryAssociation link = (BarkerErModelContainsBinaryAssociation)e.ModelElement;
			BinaryAssociation association;
			BarkerErModel barkerModel;
			if (null != (association = link.BinaryAssociation) &&
				null != (barkerModel = link.BarkerErModel))
			{
				FixUpDiagram(barkerModel, link);
			}
		}
		/// <summary>
		/// Stop the DSLTools framework from placing a shape that has a non-empty location
		/// </summary>
		protected override void OnChildConfigured(ShapeElement child, bool childWasPlaced, bool createdDuringViewFixup)
		{
			if (!childWasPlaced)
			{
				BarkerEntityShape shape = child as BarkerEntityShape;
				if (shape != null && shape.Location != BoundsRules.GetCompliantBounds(child, RectangleD.Empty).Location)
				{
					IDictionary unplacedShapes = UnplacedShapesContext.GetUnplacedShapesMap(Store.TransactionManager.CurrentTransaction.TopLevelTransaction, this.Id);
					if (unplacedShapes.Contains(child))
					{
						unplacedShapes.Remove(child);
					}
				}
			}
			base.OnChildConfigured(child, childWasPlaced, createdDuringViewFixup);
		}
		/// <summary>
		/// Correctly connect a <see cref="AssociationConnector"/>
		/// </summary>
		protected override void OnChildConfiguring(ShapeElement child, bool createdDuringViewFixup)
		{
			AssociationConnector connector;
			if (null != (connector = child as AssociationConnector))
			{
				BarkerErModelContainsBinaryAssociation link = (BarkerErModelContainsBinaryAssociation)child.ModelElement;
				BarkerEntityShape sourceShape = null;
				EntityType sourceEntity;
				if (null != (sourceEntity = link.BinaryAssociation.RoleCollection[0].EntityType))
				{
					foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(sourceEntity))
					{
						BarkerEntityShape testShape = pel as BarkerEntityShape;
						if (testShape != null && testShape.Diagram == this)
						{
							sourceShape = testShape;
							break;
						}
					}
				}
				if (null != sourceShape)
				{
					foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(link.BinaryAssociation.RoleCollection[1].EntityType))
					{
						BarkerEntityShape targetShape = pel as BarkerEntityShape;
						if (targetShape != null && targetShape.Diagram == this)
						{
							connector.Connect(sourceShape, targetShape);
							return;
						}
					}
				}
			}
			base.OnChildConfiguring(child, createdDuringViewFixup);
		}
		/// <summary>
		/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> for <see cref="BarkerERDiagram"/>s.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="eventManager">The <see cref="ModelingEventManager"/> used to manage the <see cref="EventHandler{TEventArgs}"/>s.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		public static void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{
			foreach (BarkerERDiagram diagram in store.ElementDirectory.FindElements<BarkerERDiagram>(false))
			{
				switch (action)
				{
					case EventHandlerAction.Add:
						diagram.SubscribeCompartmentItemsEvents();
						break;
					case EventHandlerAction.Remove:
						diagram.UnsubscribeCompartmentItemsEvents();
						break;
				}
			}
		}
	}
}
