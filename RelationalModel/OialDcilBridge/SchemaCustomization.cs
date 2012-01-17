#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © ORM SOlutions, LLC. All rights reserved.                     *
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
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge;
using ORMSolutions.ORMArchitect.ORMAbstraction;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge
{
	#region SchemaCustomization class
	/// <summary>
	/// Class to cache schema customizations based on stable representations
	/// of the schema elements, then match those customizations to an modified
	/// version of the schema.
	/// </summary>
	public sealed class SchemaCustomization
	{
		#region ColumnKey struct
		private struct ColumnKey
		{
			#region Member Variables
			private IList<Guid> myIds;
			#endregion // Member Variables
			#region Constructors
			public ColumnKey(IList<Guid> ids)
			{
				myIds = ids;
			}
			#endregion // Constructors
			#region Equality overrides
			/// <summary>
			/// Equals operator override
			/// </summary>
			public static bool operator ==(ColumnKey key1, ColumnKey key2)
			{
				return key1.Equals(key2);
			}
			/// <summary>
			/// Not equals operator override
			/// </summary>
			public static bool operator !=(ColumnKey key1, ColumnKey key2)
			{
				return !(key1.Equals(key2));
			}
			/// <summary>
			/// Standard Equals override
			/// </summary>
			public override bool Equals(object obj)
			{
				return obj is ColumnKey && Equals((ColumnKey)obj);
			}
			/// <summary>
			/// Typed Equals method
			/// </summary>
			public bool Equals(ColumnKey obj)
			{
				IList<Guid> ids = myIds;
				IList<Guid> otherIds = obj.myIds;
				if (ids == null)
				{
					return otherIds == null;
				}
				else if (otherIds == null)
				{
					return false;
				}
				int count = ids.Count;
				if (otherIds.Count == count)
				{
					for (int i = 0; i < count; ++i)
					{
						if (!ids[i].Equals(otherIds[i]))
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}
			/// <summary>
			/// Standard override
			/// </summary>
			public override int GetHashCode()
			{
				return Utility.GetCombinedHashCode<Guid>(myIds);
			}
			#endregion // Equality overrides
		}
		#endregion // ColumnKey struct
		#region ColumnInfo struct
		private struct ColumnInfo
		{
			public readonly string Name;
			public readonly int Position;
			public static readonly ColumnInfo Empty = new ColumnInfo(null, -1);
			public ColumnInfo(string name, int position)
			{
				Name = name;
				Position = position;
			}
			public ColumnInfo SetName(string name)
			{
				return new ColumnInfo(name, Position);
			}
			public ColumnInfo SetPosition(int position)
			{
				return new ColumnInfo(Name, position);
			}
			public bool IsEmpty
			{
				get
				{
					return Name == null && Position == -1;
				}
			}
		}
		#endregion // ColumnInfo struct
		#region TableInfo struct
		private struct TableInfo
		{
			public readonly string Name;
			public readonly bool HasCustomOrderedColumns;
			public static readonly TableInfo Empty = new TableInfo(null, false);
			public TableInfo(string name, bool hasCustomOrderedColumns)
			{
				Name = name;
				HasCustomOrderedColumns = hasCustomOrderedColumns;
			}
			public TableInfo SetName(string name)
			{
				return new TableInfo(name, HasCustomOrderedColumns);
			}
			public TableInfo SetHasCustomOrderedColumns(bool hasCustomOrderedColumns)
			{
				return new TableInfo(Name, hasCustomOrderedColumns);
			}
			public bool IsEmpty
			{
				get
				{
					return Name == null && !HasCustomOrderedColumns;
				}
			}
		}
		#endregion // TableInfo struct
		#region Member Variables
		private Dictionary<Guid, TableInfo> myTableData = null;
		private Dictionary<ColumnKey, ColumnInfo> myColumnData = null;
		private List<Guid> myIdList;
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// Create a schema customization object for the current
		/// state of a schema
		/// </summary>
		/// <param name="schema">The schema to customize, or
		/// <see langword="null"/> if it is known that there
		/// are no customizations for the schema.</param>
		public SchemaCustomization(Schema schema)
		{
			Dictionary<ColumnKey, ColumnInfo> columnData = null;
			Dictionary<Guid, TableInfo> tableData = null;
			List<Guid> idList = new List<Guid>();
			if (schema != null)
			{
				columnData = new Dictionary<ColumnKey, ColumnInfo>();
				Guid tableKey;
				foreach (Table table in schema.TableCollection)
				{
					bool customOrder = table.ColumnOrder == ColumnOrdering.Custom;
					bool customTableName = table.CustomName;
					if ((customOrder || customTableName) &&
						GetTableKey(table, out tableKey))
					{
						(tableData ?? (tableData = new Dictionary<Guid, TableInfo>()))[tableKey] = new TableInfo(customTableName ? table.Name : null, customOrder);
					}
					int columnIndex = 0;
					Guid[] keyIds;
					if (customOrder)
					{
						foreach (Column column in table.ColumnCollection)
						{
							if (null != (keyIds = GetKeyIds(column, idList)))
							{
								columnData[new ColumnKey(keyIds)] = new ColumnInfo(column.CustomName ? column.Name : null, columnIndex);
							}
							++columnIndex;
						}
					}
					else
					{
						foreach (Column column in table.ColumnCollection)
						{
							if (column.CustomName &&
								null != (keyIds = GetKeyIds(column, idList)))
							{
								columnData[new ColumnKey(keyIds)] = new ColumnInfo(column.Name, -1);
							}
							++columnIndex;
						}
					}
				}
				if (columnData.Count == 0)
				{
					columnData = null;
				}
			}
			myTableData = tableData;
			myColumnData = columnData;
			myIdList = idList;
		}
		#endregion // Constructor
		#region Public accessor methods
		/// <summary>
		/// Get the customizations for a given schema
		/// </summary>
		public static SchemaCustomization GetCustomization(Schema schema)
		{
			object customizationsObject;
			Dictionary<Guid, SchemaCustomization> customizations;
			SchemaCustomization customization;
			return (schema.Store.PropertyBag.TryGetValue(typeof(SchemaCustomization), out customizationsObject) &&
				null != (customizations = customizationsObject as Dictionary<Guid, SchemaCustomization>) &&
				customizations.TryGetValue(schema.Id, out customization)) ? customization : null;
		}
		/// <summary>
		/// Get the customizations for a given schema, and return the previous
		/// customization (if any).
		/// </summary>
		public static SchemaCustomization SetCustomization(Schema schema, SchemaCustomization customization)
		{
			object customizationsObject;
			Dictionary<Guid, SchemaCustomization> customizations;
			Type key = typeof(SchemaCustomization);
			Dictionary<object, object> bag = schema.Store.PropertyBag;
			SchemaCustomization prev = null;
			if (!bag.TryGetValue(key, out customizationsObject) ||
				null == (customizations = customizationsObject as Dictionary<Guid, SchemaCustomization>))
			{
				if (customization == null)
				{
					return null;
				}
				bag[key] = customizations = new Dictionary<Guid, SchemaCustomization>();
			}
			Guid schemaId = schema.Id;
			if (customization == null)
			{
				if (customizations.TryGetValue(schemaId, out prev))
				{
					customizations.Remove(schemaId);
				}
			}
			else
			{
				customizations.TryGetValue(schemaId, out prev);
				customizations[schemaId] = customization;
			}
			return prev;
		}
		/// <summary>
		/// Record or remove a current customization for a table name.
		/// </summary>
		/// <param name="table">The <see cref="Table"/> to customize.</param>
		/// <param name="name">The custom name, or <see langword="null"/> to clear
		/// any customization.</param>
		public void CustomizeTableName(Table table, string name)
		{
			Dictionary<Guid, TableInfo> tableData;
			Guid tableKey;
			if (null == (tableData = myTableData))
			{
				if (name == null)
				{
					return;
				}
				if (GetTableKey(table, out tableKey))
				{
					myTableData = tableData = new Dictionary<Guid, TableInfo>();
				}
				else
				{
					return;
				}
			}
			else if (!GetTableKey(table, out tableKey))
			{
				return;
			}
			TableInfo info;
			if (tableData.TryGetValue(tableKey, out info))
			{
				if ((info = info.SetName(name)).IsEmpty)
				{
					tableData.Remove(tableKey);
				}
				else
				{
					tableData[tableKey] = info;
				}
			}
			else if (name != null)
			{
				tableData[tableKey] = TableInfo.Empty.SetName(name);
			}
		}
		/// <summary>
		/// Get a customized table name
		/// </summary>
		/// <param name="table">The target table.</param>
		/// <returns>The customized name, or <see langword="null"/></returns>
		public string GetCustomizedTableName(Table table)
		{
			TableInfo info;
			return FindTableInfo(table, out info) ? info.Name : null;
		}
		/// <summary>
		/// Determine if columns in a table are recorded as custom ordered
		/// </summary>
		public bool GetHasCustomOrderedColumns(Table table)
		{
			TableInfo info;
			return FindTableInfo(table, out info) && info.HasCustomOrderedColumns;
		}
		/// <summary>
		/// Record or remove a current customization for a column name.
		/// </summary>
		/// <param name="column">The <see cref="Column"/> to customize.</param>
		/// <param name="name">The custom name, or <see langword="null"/> to clear
		/// any customization.</param>
		public void CustomizeColumnName(Column column, string name)
		{
			Dictionary<ColumnKey, ColumnInfo> columnData;
			Guid[] keyIds;
			if (null == (columnData = myColumnData))
			{
				if (name == null)
				{
					return;
				}
				if (null != (keyIds = GetKeyIds(column, myIdList))){
					myColumnData = columnData = new Dictionary<ColumnKey, ColumnInfo>();
					columnData[new ColumnKey(keyIds)] = ColumnInfo.Empty.SetName(name);
				}
			}
			else if (null != (keyIds = GetKeyIds(column, myIdList)))
			{
				ColumnKey key = new ColumnKey(keyIds);
				ColumnInfo info;
				if (columnData.TryGetValue(key, out info))
				{
					if ((info = info.SetName(name)).IsEmpty)
					{
						columnData.Remove(key);
					}
					else
					{
						columnData[key] = info;
					}
				}
				else if (name != null)
				{
					columnData[key] = ColumnInfo.Empty.SetName(name);
				}
			}
		}
		/// <summary>
		/// Enable or disable customization for all columns in a table.
		/// </summary>
		/// <param name="table">The <see cref="Table"/> to customize</param>
		/// <param name="isCustom"><see langword="true"/> to track customized
		/// positions, <see langword="false"/> to clear tracking.</param>
		public void CustomizeColumnPositions(Table table, bool isCustom)
		{
			// Track the table separately. This allows us to distinguish between
			// previously custom ordered tables with new unordered columns, and new
			// tables with custom ordered columns pulled from another table.
			Dictionary<Guid, TableInfo> tableData;
			Guid tableKey;
			if (null == (tableData = myTableData))
			{
				if (isCustom &&
					GetTableKey(table, out tableKey))
				{
					myTableData = tableData = new Dictionary<Guid, TableInfo>();
					tableData[tableKey] = TableInfo.Empty.SetHasCustomOrderedColumns(true);
				}
			}
			else if (GetTableKey(table, out tableKey))
			{
				TableInfo tableInfo;
				if (tableData.TryGetValue(tableKey, out tableInfo))
				{
					if ((tableInfo = tableInfo.SetHasCustomOrderedColumns(isCustom)).IsEmpty)
					{
						tableData.Remove(tableKey);
					}
					else
					{
						tableData[tableKey] = tableInfo;
					}
				}
				else if (isCustom)
				{
					tableData[tableKey] = TableInfo.Empty.SetHasCustomOrderedColumns(true);
				}
			}

			LinkedElementCollection<Column> columns = table.ColumnCollection;
			int count = columns.Count;
			if (count == 0)
			{
				return;
			}
			Dictionary<ColumnKey, ColumnInfo> columnData;
			Guid[] keyIds;
			if (null == (columnData = myColumnData))
			{
				if (!isCustom)
				{
					return;
				}
				myColumnData = columnData = new Dictionary<ColumnKey, ColumnInfo>();
			}
			for (int i = 0; i < count; ++i)
			{
				Column column = columns[i];
				if (null != (keyIds = GetKeyIds(column, myIdList)))
				{
					ColumnKey key = new ColumnKey(keyIds);
					ColumnInfo columnInfo;
					if (columnData.TryGetValue(key, out columnInfo))
					{
						if (isCustom)
						{
							columnData[key] = columnInfo.SetPosition(i);
						}
						else if ((columnInfo = columnInfo.SetPosition(-1)).IsEmpty)
						{
							columnData.Remove(key);
						}
						else
						{
							columnData[key] = columnInfo;
						}
					}
					else if (isCustom)
					{
						columnData[key] = ColumnInfo.Empty.SetPosition(i);
					}
				}
			}
		}
		/// <summary>
		/// Get a customized column name
		/// </summary>
		/// <param name="column">The target column</param>
		/// <returns>The customized name, or <see langword="null"/></returns>
		public string GetCustomizedColumnName(Column column)
		{
			ColumnInfo info;
			return FindColumnInfo(column, out info) ? info.Name : null;
		}
		/// <summary>
		/// Get a customized column position
		/// </summary>
		/// <param name="column">The target column</param>
		/// <returns>The custom position, or -1</returns>
		public int GetCustomizedColumnPosition(Column column)
		{
			ColumnInfo info;
			return FindColumnInfo(column, out info) ? info.Position : -1;
		}
		/// <summary>
		/// Test if there are any current customizations
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				Dictionary<Guid, TableInfo> tableData;
				Dictionary<ColumnKey, ColumnInfo> columnData;
				return !((null != (tableData = myTableData) && tableData.Count != 0) || (null != (columnData = myColumnData) && columnData.Count != 0));
			}
		}
		#endregion // Public accessor methods
		#region Event Handlers
		/// <summary>
		/// Manage events associated with the schema customization state.
		/// The primary function of schema customization is to reassociate
		/// schema customizations when a schema is deleted and recreated.
		/// Therefore, the data is maintained outside the transacted store
		/// and must be updated with events to remain consistent across
		/// undo and redo operations.
		/// </summary>
		/// <param name="eventManager">The <see cref="ModelingEventManager"/> to attach
		/// to. The assumption is that this is called for state change events on document
		/// loaded.</param>
		/// <param name="store">The <see cref="Store"/> used to retrieve type information.</param>
		/// <param name="action">The action to take (add or remove the handler)</param>
		public static void ManageModelingEventHandlers(ModelingEventManager eventManager, Store store, EventHandlerAction action)
		{
			DomainDataDirectory dataDir = store.DomainDataDirectory;
			eventManager.AddOrRemoveHandler(dataDir.FindDomainClass(Column.DomainClassId), new EventHandler<ElementPropertyChangedEventArgs>(ColumnPropertyChanged), action);
			eventManager.AddOrRemoveHandler(dataDir.FindDomainClass(Table.DomainClassId), new EventHandler<ElementPropertyChangedEventArgs>(TablePropertyChanged), action);
			eventManager.AddOrRemoveHandler(dataDir.FindDomainRole(TableContainsColumn.ColumnDomainRoleId), new EventHandler<RolePlayerOrderChangedEventArgs>(ColumnOrderChanged), action);
		}
		private static void ColumnPropertyChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			// Transitions:
			// Name changed, CustomName set->update schema customization
			// CustomName changed: add or remove customization
			ModelElement element = e.ModelElement;
			Column column = null;
			Table table;
			Schema schema;
			SchemaCustomization customization;
			string updatedName = null;
			bool customNameChanged = false;
			if (!element.IsDeleted)
			{
				Guid propertyId = e.DomainProperty.Id;
				if (propertyId == Column.NameDomainPropertyId)
				{
					column = (Column)element;
					if (column.CustomName)
					{
						updatedName = column.Name;
						customNameChanged = true;
					}
				}
				else if (propertyId == Column.CustomNameDomainPropertyId)
				{
					column = (Column)element;
					if (column.CustomName)
					{
						updatedName = column.Name;
					}
					customNameChanged = true;
				}
				if (customNameChanged &&
					null != (table = column.Table) &&
					null != (schema = table.Schema) &&
					null != (customization = SchemaCustomization.GetCustomization(schema)))
				{
					customization.CustomizeColumnName(column, updatedName);
				}
			}
		}
		private static void TablePropertyChanged(object sender, ElementPropertyChangedEventArgs e)
		{
			// Transitions:
			// Name changed, CustomName set->update schema customization
			// CustomName changed: add or remove customization
			ModelElement element = e.ModelElement;
			Table table = null;
			Schema schema;
			SchemaCustomization customization;
			string updatedName = null;
			bool customNameChanged = false;
			if (!element.IsDeleted)
			{
				Guid propertyId = e.DomainProperty.Id;
				if (propertyId == Table.NameDomainPropertyId)
				{
					table = (Table)element;
					if (table.CustomName)
					{
						updatedName = table.Name;
						customNameChanged = true;
					}
				}
				else if (propertyId == Table.CustomNameDomainPropertyId)
				{
					table = (Table)element;
					if (table.CustomName)
					{
						updatedName = table.Name;
					}
					customNameChanged = true;
				}
				else if (propertyId == Table.ColumnOrderDomainPropertyId)
				{
					table = (Table)element;
					bool isCustom = (ColumnOrdering)e.NewValue == ColumnOrdering.Custom;
					if ((isCustom ^ ((ColumnOrdering)e.OldValue == ColumnOrdering.Custom)) &&
						null != (schema = table.Schema) &&
						null != (customization = SchemaCustomization.GetCustomization(schema)))
					{
						// UNDONE: Consider batching this into an 'element events ended'
						customization.CustomizeColumnPositions(table, isCustom);
					}
				}
				if (customNameChanged &&
					null != (schema = table.Schema) &&
					null != (customization = SchemaCustomization.GetCustomization(schema)))
				{
					customization.CustomizeTableName(table, updatedName);
				}
			}
		}
		private static void ColumnOrderChanged(object sender, RolePlayerOrderChangedEventArgs e)
		{
			ModelElement element = e.SourceElement;
			if (!element.IsDeleted)
			{
				Table table = (Table)element;
				Schema schema;
				SchemaCustomization customization;
				if (table.ColumnOrder == ColumnOrdering.Custom &&
					null != (schema = table.Schema) &&
					null != (customization = SchemaCustomization.GetCustomization(schema)))
				{
					// UNDONE: Consider batching this into an 'element events ended'
					customization.CustomizeColumnPositions(table, true);
				}
			}
		}
		#endregion // Event Handlers
		#region Key generation
		private static Guid[] GetKeyIds(Column column, List<Guid> idList)
		{
			int minKeySize;
			return BuildKey(column, idList, out minKeySize) ? idList.ToArray() : null;
		}
		/// <summary>
		/// Get a minimal unique identifier for a column based on mapped roles. The identifer
		/// will stop as soon as the fact type is used in a single column or in a partitioned
		/// or separated column.
		/// </summary>
		/// <param name="column">The <see cref="Column"/> to analyze</param>
		/// <param name="idList">Scratch list used to determine keys</param>
		/// <param name="minKeySize">The minimum number of ids that uniquely
		/// identify this column. The actually key may be longer than this
		/// minimimum for partitioned and separated tables. Key comparisons
		/// may be performed down to this minimum size.</param>
		/// <returns><see langword="true"/> if the key was available.</returns>
		private static bool BuildKey(Column column, List<Guid> idList, out int minKeySize)
		{
			idList.Clear();
			int uniqueUseIndex = -1;
			minKeySize = 0;
			LinkedElementCollection<ConceptTypeChild> childNodes = ColumnHasConceptTypeChild.GetConceptTypeChildPath(column);
			for (int i = childNodes.Count - 1; i >= 0; --i)
			{
				ConceptTypeChild child = childNodes[i];
				bool uniqueChild = ColumnHasConceptTypeChild.GetLinksToColumn(child).Count == 1;
				LinkedElementCollection<FactType> pathFactTypes = ConceptTypeChildHasPathFactType.GetPathFactTypeCollection(child);
				int factTypeCount = pathFactTypes.Count;
				InformationType infoType;
				ObjectType objectType;
				if (0 != (factTypeCount = pathFactTypes.Count))
				{
					for (int j = factTypeCount - 1; j >= 0; --j)
					{
						FactType factType = pathFactTypes[j];
						idList.Add(FactTypeMapsTowardsRole.GetTowardsRole(factType).Id);
						if (-1 == uniqueUseIndex &&
							uniqueChild &&
							ConceptTypeChildHasPathFactType.GetLinksToConceptTypeChild(factType).Count == 1)
						{
							minKeySize = idList.Count;
							uniqueUseIndex = minKeySize - 1;
						}
					}
					ConceptTypeAssimilatesConceptType assimilation;
					if (uniqueUseIndex != -1 &&
						null != (assimilation = child as ConceptTypeAssimilatesConceptType) &&
						AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation) != AssimilationAbsorptionChoice.Absorb)
					{
						uniqueUseIndex = idList.Count - 1;
					}
				}
				else if (null != (infoType = child as InformationType) &&
					null != (objectType = ConceptTypeIsForObjectType.GetObjectType(infoType.ConceptType)))
				{
					// Happens for a value column in an object type table, which has one concept type child
					idList.Add(objectType.Id);
				}
			}
			int count = idList.Count;
			if (0 == count)
			{
				return false;
			}
			if (uniqueUseIndex != -1 &&
				(uniqueUseIndex + 1) < count)
			{
				idList.RemoveRange(uniqueUseIndex + 1, count - uniqueUseIndex - 1);
			}
			if (minKeySize == 0)
			{
				minKeySize = idList.Count;
			}
			return true;
		}
		#endregion // Key generation
		#region Table lookup
		private bool GetTableKey(Table table, out Guid key)
		{
			ConceptType conceptType;
			ObjectType objectType;
			if (null != (conceptType = TableIsPrimarilyForConceptType.GetConceptType(table)) &&
				null != (objectType = ConceptTypeIsForObjectType.GetObjectType(conceptType)))
			{
				key = objectType.Id;
				return true;
			}
			key = Guid.Empty;
			return false;
		}
		private bool FindTableInfo(Table table, out TableInfo info)
		{
			Dictionary<Guid, TableInfo> tableData;
			Guid tableKey;
			if (null != (tableData = myTableData) &&
				GetTableKey(table, out tableKey))
			{
				return tableData.TryGetValue(tableKey, out info);
			}
			info = TableInfo.Empty;
			return false;
		}
		#endregion // Table lookup
		#region Column lookup
		private bool FindColumnInfo(Column column, out ColumnInfo info)
		{
			Dictionary<ColumnKey, ColumnInfo> columnData;
			List<Guid> idList = myIdList;
			int minCount;
			if (null != (columnData = myColumnData) &&
				BuildKey(column, idList, out minCount))
			{
				int count = idList.Count;
				ColumnKey key = new ColumnKey(idList);
				while (count >= minCount)
				{
					if (columnData.TryGetValue(key, out info))
					{
						return true;
					}
					if (--count != 0)
					{
						// Find a less-specific column from the previous state.
						// The theory is that related columns will be found in
						// the same place so that comparing the position
						// values will be meaningful.
						idList.RemoveAt(count);
					}
				}
			}
			info = ColumnInfo.Empty;
			return false;
		}
		#endregion // Column lookup
	}
	#endregion // SchemaCustomization class
}
