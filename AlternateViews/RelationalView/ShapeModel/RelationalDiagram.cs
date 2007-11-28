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
using Neumont.Tools.ORMAbstraction;
using Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge;
using Neumont.Tools.ORMToORMAbstractionBridge;

namespace Neumont.Tools.ORM.Views.RelationalView
{
	[DiagramMenuDisplay(DiagramMenuDisplayOptions.BlockRename | DiagramMenuDisplayOptions.Required, typeof(RelationalDiagram), RelationalDiagram.NameResourceName, "Diagram.TabImage")]
	partial class RelationalDiagram
	{
		private const string NameResourceName = "Diagram.MenuDisplayName";

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public RelationalDiagram(Store store, params PropertyAssignment[] propertyAssignments)
			: this(store != null ? store.DefaultPartition : null, propertyAssignments)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public RelationalDiagram(Partition partition, params PropertyAssignment[] propertyAssignments)
			: base(partition, propertyAssignments)
		{
			this.Name = Neumont.Tools.Modeling.Design.ResourceAccessor<RelationalDiagram>.ResourceManager.GetString(NameResourceName);
		}
		public override void OnInitialize()
		{
			base.OnInitialize();
			if (this.Subject == null)
			{
				ReadOnlyCollection<Catalog> modelElements = this.Store.DefaultPartition.ElementDirectory.FindElements<Catalog>();
				if (modelElements.Count != 0)
				{
					this.Associate(modelElements[0]);
				}
			}
		}
		/// <summary>
		/// Customize childshape create to set an initial location for a <see cref="TableShape"/>
		/// </summary>
		protected override ShapeElement CreateChildShape(ModelElement element)
		{
			object tablePositionsObject;
			Dictionary<Guid, PointD> tablePositions;
			PointD initialLocation;
			ReferenceConstraintTargetsTable referenceConstraintTargetsTable;
			Table table;
			ConceptType conceptType;
			ObjectType objectType;
			if (null != (table = element as Table))
			{
				if (element.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.TryGetValue(TablePositionDictionaryKey, out tablePositionsObject) &&
					null != (tablePositions = tablePositionsObject as Dictionary<Guid, PointD>) &&
					null != (conceptType = TableIsPrimarilyForConceptType.GetConceptType(table)) &&
					null != (objectType = ConceptTypeIsForObjectType.GetObjectType(conceptType)) &&
					tablePositions.TryGetValue(objectType.Id, out initialLocation))
				{
					return new TableShape(
						this.Partition,
						new PropertyAssignment(TableShape.AbsoluteBoundsDomainPropertyId, new RectangleD(initialLocation, new SizeD(1, 0.3))));
				}
			}
			else if (null != (referenceConstraintTargetsTable = element as ReferenceConstraintTargetsTable))
			{
				return new ForeignKeyConnector(Partition);
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
		/// ChangeRule: typeof(RelationalDiagram)
		/// Disallows changing the name of the Relational Diagram
		/// Changes the name of the <see cref="T:Neumont.Tools.ORM.Views.RelationalDiagram"/> to
		/// its default name if changed by a user.
		/// </summary>
		private static void NameChangedRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeId = e.DomainProperty.Id;
			if (attributeId == Diagram.NameDomainPropertyId)
			{
				RelationalDiagram diagram = e.ModelElement as RelationalDiagram;
				string name = Neumont.Tools.Modeling.Design.ResourceAccessor<RelationalDiagram>.ResourceManager.GetString(NameResourceName);
				if (diagram != null && diagram.Name != name)
				{
					diagram.Name = name;
				}
			}
			else if (attributeId == RelationalDiagram.DisplayDataTypesDomainPropertyId)
			{
				foreach (PresentationElement pel in ((RelationalDiagram)e.ModelElement).NestedChildShapes)
				{
					TableShape shape;
					ColumnElementListCompartment compartment;
					if (null != (shape = pel as TableShape) &&
						null != (compartment = shape.FindCompartment("ColumnsCompartment") as ColumnElementListCompartment))
					{
						compartment.InvalidateOrUpdateSize();
					}
				}
			}
		}
		/// <summary>
		/// ChangeRule: typeof(Neumont.Tools.RelationalModels.ConceptualDatabase.Column)
		/// Update table size when <see cref="Column.IsNullable"/> or <see cref="Column.Name"/> changes.
		/// </summary>
		private static void DisplayColumnPropertyChangedRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeId = e.DomainProperty.Id;
			if (attributeId == Column.IsNullableDomainPropertyId ||
				attributeId == Column.NameDomainPropertyId)
			{
				UpdateTablePresentationSize(((Column)e.ModelElement).Table);
			}
		}
		/// <summary>
		/// ChangeRule: typeof(Neumont.Tools.RelationalModels.ConceptualDatabase.Table)
		/// Update table size when <see cref="Table.Name"/> changes.
		/// </summary>
		private static void DisplayTablePropertyChangedRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeId = e.DomainProperty.Id;
			if (attributeId == Table.NameDomainPropertyId)
			{
				UpdateTablePresentationSize((Table)e.ModelElement);
			}
		}
		private static void UpdateTablePresentationSize(Table table)
		{
			if (table != null)
			{
				foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(table))
				{
					TableShape shape;
					ColumnElementListCompartment compartment;
					if (null != (shape = pel as TableShape) &&
						null != (compartment = shape.FindCompartment("ColumnsCompartment") as ColumnElementListCompartment))
					{
						compartment.InvalidateOrUpdateSize();
					}
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.ValueTypeHasDataType)
		/// Update table size when a column datatype is changed.
		/// </summary>
		private static void DataTypeChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == ValueTypeHasDataType.DataTypeDomainRoleId)
			{
				UpdateTablesOnDataTypeChange((ValueTypeHasDataType)e.ElementLink);
			}
		}
		private static void UpdateTablesOnDataTypeChange(ValueTypeHasDataType link)
		{
			InformationTypeFormat format;
			LinkedElementCollection<ConceptType> conceptTypes;
			if (null != (format = InformationTypeFormatIsForValueType.GetInformationTypeFormat(link.ValueType)) &&
				0 != (conceptTypes = InformationType.GetConceptTypeCollection(format)).Count)
			{
				foreach (ConceptType conceptType in conceptTypes)
				{
					UpdateTablesForConceptType(conceptType, null, null);
				}
			}
		}
		private static void UpdateTablesForConceptType(ConceptType conceptType, Predicate<ConceptType> conceptTypeFilter, Predicate<Table> tableFilter)
		{
			Table primaryTable = TableIsPrimarilyForConceptType.GetTable(conceptType);
			LinkedElementCollection<Table> secondaryTables = TableIsAlsoForConceptType.GetTable(conceptType);
			if (primaryTable != null && (tableFilter == null || !tableFilter(primaryTable)))
			{
				UpdateTablePresentationSize(primaryTable);
			}
			foreach (Table secondaryTable in secondaryTables)
			{
				if (tableFilter == null || !tableFilter(secondaryTable))
				{
					UpdateTablePresentationSize(secondaryTable);
				}
			}
			Predicate<ConceptType> recurseConceptTypeFilter =
				delegate(ConceptType testConceptType)
				{
					return testConceptType == conceptType ||
						(conceptTypeFilter != null && conceptTypeFilter(testConceptType));
				};
			Predicate<Table> recurseTableFilter =
				delegate(Table testTable)
				{
					return testTable == primaryTable || secondaryTables.Contains(testTable);
				};
			foreach (ConceptType relatingConceptType in ConceptTypeRelatesToConceptType.GetRelatingConceptTypeCollection(conceptType))
			{
				if (relatingConceptType == conceptType || (conceptTypeFilter != null && conceptTypeFilter(relatingConceptType)))
				{
					continue;
				}
				UpdateTablesForConceptType(
					relatingConceptType,
					recurseConceptTypeFilter,
					recurseTableFilter);
			}
			foreach (ConceptType assimilatedConceptType in ConceptTypeAssimilatesConceptType.GetAssimilatedConceptTypeCollection(conceptType))
			{
				if (assimilatedConceptType == conceptType || (conceptTypeFilter != null && conceptTypeFilter(assimilatedConceptType)))
				{
					continue;
				}
				UpdateTablesForConceptType(
					assimilatedConceptType,
					recurseConceptTypeFilter,
					recurseTableFilter);
			}
		}
		/// <summary>
		/// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.ValueTypeHasDataType)
		/// Update table size when a column datatype facet is changed
		/// </summary>
		private static void DataTypeFacetChangedRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeId = e.DomainProperty.Id;
			if (attributeId == ValueTypeHasDataType.ScaleDomainPropertyId ||
				attributeId == ValueTypeHasDataType.LengthDomainPropertyId)
			{
				UpdateTablesOnDataTypeChange((ValueTypeHasDataType)e.ModelElement);
			}
		}
		/// <summary>
		/// AddRule: typeof(Neumont.Tools.RelationalModels.ConceptualDatabase.ReferenceConstraintTargetsTable), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
		/// Add a <see cref="ForeignKeyConnector"/> to the diagram
		/// </summary>
		private static void ReferenceConstraintAddedRule(ElementAddedEventArgs e)
		{
			ReferenceConstraintTargetsTable link = (ReferenceConstraintTargetsTable)e.ModelElement;
			Table sourceTable;
			Schema schema;
			Catalog catalog;
			if (null != (sourceTable = link.ReferenceConstraint.SourceTable) &&
				null != (schema = sourceTable.Schema) &&
				null != (catalog = schema.Catalog))
			{
				FixUpDiagram(catalog, link);
			}
		}
		/// <summary>
		/// Stop the DSLTools framework from placing a shape that has a non-empty location
		/// </summary>
		protected override void OnChildConfigured(ShapeElement child, bool childWasPlaced, bool createdDuringViewFixup)
		{
			if (!childWasPlaced)
			{
				TableShape shape = child as TableShape;
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
		/// Correctly connect a <see cref="ForeignKeyConnector"/>
		/// </summary>
		protected override void OnChildConfiguring(ShapeElement child, bool createdDuringViewFixup)
		{
			ForeignKeyConnector foreignKeyConnector;
			if (null != (foreignKeyConnector = child as ForeignKeyConnector))
			{
				ReferenceConstraintTargetsTable link = (ReferenceConstraintTargetsTable)child.ModelElement;
				TableShape sourceShape = null;
				Table sourceTable;
				if (null != (sourceTable = link.ReferenceConstraint.SourceTable))
				{
					foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(sourceTable))
					{
						TableShape testShape = pel as TableShape;
						if (testShape != null && testShape.Diagram == this)
						{
							sourceShape = testShape;
							break;
						}
					}
				}
				if (null != sourceShape)
				{
					foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(link.TargetTable))
					{
						TableShape targetShape = pel as TableShape;
						if (targetShape != null && targetShape.Diagram == this)
						{
							foreignKeyConnector.Connect(sourceShape, targetShape);
							return;
						}
					}
				}
			}
			base.OnChildConfiguring(child, createdDuringViewFixup);
		}
		/// <summary>
		/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> for <see cref="RelationalDiagram"/>s.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="eventManager">The <see cref="ModelingEventManager"/> used to manage the <see cref="EventHandler{TEventArgs}"/>s.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		public static void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{
			foreach (RelationalDiagram diagram in store.ElementDirectory.FindElements<RelationalDiagram>(false))
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
