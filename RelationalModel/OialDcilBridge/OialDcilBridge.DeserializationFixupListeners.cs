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

//#define DO_SEPARATION
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
		#region Fully populate from OIAL
		#region AssimilationPath structure
		
		/// <summary>
		/// AssimilationPath is used for the Value Pair in the TableIsAlsoForConceptType to store the path of the assimilations to the Primary Table.
		/// </summary>
		private struct AssimilationPath
		{
			private List<ConceptTypeAssimilatesConceptType> myPath;

			public List<ConceptTypeAssimilatesConceptType> Path
			{
				get
				{
					return myPath;
				}
			}

			public AssimilationPath(List<ConceptTypeAssimilatesConceptType> assimilations)
			{
				myPath = assimilations;
			}
		}
		#endregion // AssimilationPath structure

		/// <summary>
		/// 
		/// </summary>
		private static void FullyGenerateConceptualDatabaseModel(Schema schema, AbstractionModel sourceModel)
		{
			LinkedElementCollection<Table> tables = schema.TableCollection;
			LinkedElementCollection<ConceptType> conceptTypes = sourceModel.ConceptTypeCollection;
			Store store = schema.Store;

			// Map all InformationTypeFormats to domains in the schema
			// UNDONE: (Phase 2 when we care about datatypes). There is not currently
			// sufficient information in the oial model to add the required predefineddatatype
			// to a generated domain. Use this as pattern code
			//LinkedElementCollection<Domain> domains = schema.DomainCollection;
			//foreach (InformationTypeFormat itf in sourceModel.InformationTypeFormatCollection)
			//{
			//    Domain domain = new Domain(store);
			//    domains.Add(domain);
			//    new DomainIsForInformationTypeFormat(domain, itf);
			//    notifyAdded.ElementAdded(domain, true);
			//}

			Dictionary<TableIsAlsoForConceptType, AssimilationPath> assimilationPath = new Dictionary<TableIsAlsoForConceptType, AssimilationPath>();
			Dictionary<Column, List<ConceptTypeChild>> columnHasConceptTypeChildPath = new Dictionary<Column, List<ConceptTypeChild>>();


			Dictionary<ConceptType, ConceptTypeAssimilatesConceptType> assimilatedConceptTypes = new Dictionary<ConceptType, ConceptTypeAssimilatesConceptType>();
			ReadOnlyCollection<ConceptTypeAssimilatesConceptType> allAssimilations = store.ElementDirectory.FindElements<ConceptTypeAssimilatesConceptType>();
			foreach (ConceptTypeAssimilatesConceptType assimilation in allAssimilations)
			{
				ConceptType assimilatedConceptType = null;
				if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation) == AssimilationAbsorptionChoice.Partition)
				{
					assimilatedConceptType = assimilation.AssimilatorConceptType;
				}
				else if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation) == AssimilationAbsorptionChoice.Absorb)
				{
					assimilatedConceptType = assimilation.AssimilatedConceptType;
				}
				if (assimilatedConceptType != null && assimilatedConceptType.Model == sourceModel)
				{
					assimilatedConceptTypes[assimilatedConceptType] = assimilation;
				}
			}

			foreach (ConceptType conceptType in conceptTypes)
			{
				if (!assimilatedConceptTypes.ContainsKey(conceptType))
				{
					Table conceptTypeTable = new Table(
						conceptType.Store,
						new PropertyAssignment[]{
							new PropertyAssignment(Table.NameDomainPropertyId, conceptType.Name)});
					new TableIsPrimarilyForConceptType(conceptTypeTable, conceptType);

					tables.Add(conceptTypeTable);
				}
			}

			// Create the table mappings for all Absorption and Partioning cases
			Dictionary<ConceptType, List<ConceptType>> mappingPaths = new Dictionary<ConceptType, List<ConceptType>>();
			Dictionary<ConceptType, ConceptType> pathDictonary = new Dictionary<ConceptType, ConceptType>();
			do
			{
				foreach (KeyValuePair<ConceptType, ConceptTypeAssimilatesConceptType> conceptType in assimilatedConceptTypes)
				{
					if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(conceptType.Value) == AssimilationAbsorptionChoice.Absorb)
					{
						MapAbsorbedConceptType(conceptType.Key, pathDictonary, allAssimilations, assimilationPath, new List<ConceptTypeAssimilatesConceptType>());
					}
					else if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(conceptType.Value) == AssimilationAbsorptionChoice.Partition)
					{
						MapPartitionedConceptType(conceptType.Key, mappingPaths, pathDictonary, allAssimilations, assimilationPath, new List<ConceptTypeAssimilatesConceptType>());
					}
				}
			} while (pathDictonary.ContainsValue(null));
			foreach (KeyValuePair<ConceptType, List<ConceptType>> kvp in mappingPaths)
			{
				foreach (ConceptType myConceptType in kvp.Value)
				{
					if (mappingPaths.ContainsKey(myConceptType))
					{
						foreach (ConceptType primaryConceptType in mappingPaths[myConceptType])
						{
							Table isPrimarilyForTable = TableIsPrimarilyForConceptType.GetTable(primaryConceptType);
							if (isPrimarilyForTable != null)
							{
								TableIsAlsoForConceptType tableIsAlsoForConceptType = new TableIsAlsoForConceptType(isPrimarilyForTable, kvp.Key);
							}
						}
					}
				}
			}

			//UNDONE - Need to Implement method DoSeparation for handling Seperation Cases.
			foreach (ConceptType conceptType in conceptTypes)
			{
				CreateColumns(conceptType, assimilationPath, columnHasConceptTypeChildPath);
#if DO_SEPARATION
						bool isPreferredForChildFound = false;

						IEnumerable<ConceptTypeAssimilatesConceptType> conceptTypeAssimilations = GetAssimilationsForConceptType(conceptType);

						foreach (ConceptTypeAssimilatesConceptType assimilation in conceptTypeAssimilations)
						{
							if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation) == AssimilationAbsorptionChoice.Separate)
							{
								//	DoSeparation(assimilation, ref isPreferredForChildFound);
							}
						}
#endif // DO_SEPARATION
				CreateUniquenessConstraints(conceptType, assimilationPath, columnHasConceptTypeChildPath);
			}

			foreach (Table table in schema.TableCollection)
			{
				CreateForeignKeys(table, store, columnHasConceptTypeChildPath);
			}
		}
		
		/// <summary>
		/// Gets all ConceptType Assimilates ConceptType retaitions containing a given ConceptType as either the Asimmilator or the Assimilated ConceptType
		/// </summary>
		private static IEnumerable<ConceptTypeAssimilatesConceptType> GetAssimilationsForConceptType(ConceptType conceptType)
		{
			foreach (ConceptTypeAssimilatesConceptType assimilated in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(conceptType))
			{
				yield return assimilated;
			}
			foreach (ConceptTypeAssimilatesConceptType assimilator in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType))
			{
				yield return assimilator;
			}
		}

		/// <summary>
		/// Maps all Absorbed ConceptTypes, 
		/// For a ConceptType it finds the Primary Tables that they map to, 
		/// and then map a TableIsAlsoForConceptType to the Primary Table with that ConceptType.
		/// </summary>
		/// <param name="conceptType">The ConceptType that is to be checked for Absorbtion.</param>
		/// <param name="assimilatedConceptTypes">The Dictionary assimilatedConceptTypes contains all Absorbed paths, mapping ConceptTypes 
		/// to the ConceptType that they absorb into.</param>
		/// <param name="allAssimilations"></param>
		/// <param name="assimilationPathDictionary"></param>
		/// <param name="assimilationPath">Assimilation Path records the current path of the ConceptType being mapped, 
		/// when a ConceptType's path is fully mapped, this list then gets added to the Dictonary assimilationPathDictionary</param>
		private static void MapAbsorbedConceptType(ConceptType conceptType, Dictionary<ConceptType, ConceptType> assimilatedConceptTypes, ReadOnlyCollection<ConceptTypeAssimilatesConceptType> allAssimilations, Dictionary<TableIsAlsoForConceptType, AssimilationPath> assimilationPathDictionary, List<ConceptTypeAssimilatesConceptType> assimilationPath)
		{
			foreach (ConceptTypeAssimilatesConceptType assimilation in allAssimilations)
			{
				if (assimilation.AssimilatedConceptType == conceptType
					&& AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation) == AssimilationAbsorptionChoice.Absorb)
				{
					assimilationPath.Add(assimilation);

					if (!assimilatedConceptTypes.ContainsKey(conceptType))
					{
						assimilatedConceptTypes[conceptType] = null;

						Table isPrimarilyForTable = TableIsPrimarilyForConceptType.GetTable(assimilation.AssimilatorConceptType);
						if (isPrimarilyForTable != null)
						{
							TableIsAlsoForConceptType tableForConceptType = new TableIsAlsoForConceptType(isPrimarilyForTable, conceptType);
							assimilatedConceptTypes[conceptType] = assimilation.AssimilatorConceptType;
							assimilationPathDictionary[tableForConceptType] = new AssimilationPath(assimilationPath);  // Add assimilatedConceptTypes to TableIsAlsoForConceptType created. --UNDONE--?
						}
						else if (assimilatedConceptTypes.ContainsKey(assimilation.AssimilatorConceptType) && assimilatedConceptTypes[assimilation.AssimilatorConceptType] != null)
						{
							assimilatedConceptTypes[conceptType] = assimilatedConceptTypes[assimilation.AssimilatorConceptType];
							TableIsAlsoForConceptType tableForConceptType = new TableIsAlsoForConceptType(TableIsPrimarilyForConceptType.GetTable(assimilatedConceptTypes[assimilation.AssimilatorConceptType]), conceptType);
							assimilationPathDictionary[tableForConceptType] = new AssimilationPath(assimilationPath);
						}
					}
					else if (assimilatedConceptTypes.ContainsKey(assimilation.AssimilatorConceptType) && assimilatedConceptTypes[conceptType] == null)
					{
						assimilatedConceptTypes[conceptType] = assimilatedConceptTypes[assimilation.AssimilatorConceptType];
						TableIsAlsoForConceptType tableForConceptType = new TableIsAlsoForConceptType(TableIsPrimarilyForConceptType.GetTable(assimilatedConceptTypes[assimilation.AssimilatorConceptType]), conceptType);
						assimilationPathDictionary[tableForConceptType] = new AssimilationPath(assimilationPath);
					}
					else
					{
						MapAbsorbedConceptType(assimilation.AssimilatorConceptType, assimilatedConceptTypes, allAssimilations, assimilationPathDictionary, assimilationPath);
					}
				}
			}
		}
		
		// TODO: Test more.
		/// <summary>
		/// MapPartitionedConceptType finds any partitioned assimilation that a ConceptType plays in and maps a TableIsAlsoForConceptType to the Primary Table.
		/// </summary>
		/// <param name="assimilationPathDictionary"></param>
		/// <param name="assimilatorConceptTypes"></param>
		/// <param name="conceptType"></param>
		/// <param name="mappingPaths">mappingPaths contains a collection of the mappings of all partitions.  Each partitioned ConceptType corrilates to a List of ConceptTypes that is is partitioned into.</param>
		/// <param name="allAssimilations">The Dictionary assimilatedConceptTypes contains all Absorbed paths, mapping ConceptTypes 
		/// to the ConceptType that they absorb into.</param>
		/// <param name="assimilationPath">Assimilation Path records the current path of the ConceptType being mapped, 
		/// when a ConceptType's path is fully mapped, this list then gets added to the Dictonary assimilationPathDictionary</param>
		private static void MapPartitionedConceptType(ConceptType conceptType, Dictionary<ConceptType, List<ConceptType>> mappingPaths, Dictionary<ConceptType, ConceptType> assimilatorConceptTypes, ReadOnlyCollection<ConceptTypeAssimilatesConceptType> allAssimilations, Dictionary<TableIsAlsoForConceptType, AssimilationPath> assimilationPathDictionary, List<ConceptTypeAssimilatesConceptType> assimilationPath)
		{
			foreach (ConceptTypeAssimilatesConceptType assimilation in allAssimilations)
			{
				if (assimilation.AssimilatorConceptType == conceptType && AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation) == AssimilationAbsorptionChoice.Partition)
				{
					if (!mappingPaths.ContainsKey(conceptType))
					{
						mappingPaths[conceptType] = new List<ConceptType>();

					}
					else if (mappingPaths[conceptType] == null)
					{
						mappingPaths[conceptType] = new List<ConceptType>();
					}

					mappingPaths[conceptType].Add(assimilation.AssimilatedConceptType);

					assimilationPath.Add(assimilation);
					Table isPrimarilyForTable = TableIsPrimarilyForConceptType.GetTable(assimilation.AssimilatedConceptType);
					if (isPrimarilyForTable != null)
					{
						TableIsAlsoForConceptType tableIsAlsoForConceptType = new TableIsAlsoForConceptType(isPrimarilyForTable, conceptType);
						assimilationPathDictionary[tableIsAlsoForConceptType] = new AssimilationPath(assimilationPath);
					}
					else
					{
						List<ConceptTypeAssimilatesConceptType> clonedPath = new List<ConceptTypeAssimilatesConceptType>(assimilationPath);
						MapAbsorbedConceptType(assimilation.AssimilatedConceptType, assimilatorConceptTypes, allAssimilations, assimilationPathDictionary, clonedPath);
					}
				}
			}
		}

		// UNDONE - Need to Implement the recording of Primary Identifier Columns.
		/// <summary>
		/// CreateColumns builds a ColumnList by getting all columns for ConceptTypeChildren connected to a ConceptType, 
		/// through Assimilation, as an Information Type, or through a ConceptTypeRelatesToConceptType.  
		/// If the ConceptType has a coresponding Table that is primarily for that ConceptType then the column collection is mapped to that Table.
		/// If the ConceptType does not have a coresponding Table primarily for that ConceptType, 
		/// then checks through each table containing a TableIsAlsoForConceptType with that ConceptType, and adds the Column.
		/// </summary>
		/// <param name="conceptType"></param>
		/// <param name="assimilationPath"></param>
		/// <param name="columnHasConceptTypeChildPath">ColumnHasConceptTypeChildPath contains a List of ConceptTypeChildren 
		/// representing the mapping path for a Column.</param>
		private static void CreateColumns(ConceptType conceptType, Dictionary<TableIsAlsoForConceptType, AssimilationPath> assimilationPath, Dictionary<Column, List<ConceptTypeChild>> columnHasConceptTypeChildPath)
		{
			List<ConceptTypeChild> preferredConceptTypeChildList = GetPreferredConceptTypeChildrenForConceptType(conceptType);
			List<Column> columnsForConceptType = new List<Column>();
			foreach (ConceptTypeChild conceptTypeChild in ConceptTypeChild.GetLinksToTargetCollection(conceptType))
			{
				if (conceptTypeChild.GetType() != typeof(ConceptTypeAssimilatesConceptType))
				{
					columnsForConceptType.AddRange(GetColumnsForConceptTypeChild(conceptTypeChild, columnHasConceptTypeChildPath, new List<ConceptTypeChild>()));
				}
			}
			foreach (ConceptTypeChild assimilatedConceptType in preferredConceptTypeChildList)
			{
				if (assimilatedConceptType.GetType() == typeof(ConceptTypeAssimilatesConceptType))
				{
					columnsForConceptType.AddRange(GetColumnsForConceptTypeChild(assimilatedConceptType, columnHasConceptTypeChildPath, new List<ConceptTypeChild>()));
				}
			}
			Table conceptTypeTable = TableIsPrimarilyForConceptType.GetTable(conceptType);
			if (conceptTypeTable != null)
			{
				foreach (Column column in columnsForConceptType)
				{
					Column possibleDuplicateColumn = CheckForDuplicateColumn(conceptTypeTable, column);
					if (possibleDuplicateColumn != null)
					{
						if (preferredConceptTypeChildList.Contains(columnHasConceptTypeChildPath[possibleDuplicateColumn][0]))
						{
							// Record CTHasPrimaryIdentifierColumn(CT, "PossibleDuplicateColumn");
						}
					}
					else
					{
						if (preferredConceptTypeChildList.Contains(columnHasConceptTypeChildPath[column][0]))
						{
							//UniquenessConstraintIncludesColumn  = new UniquenessConstraintIncludesColumn( , column);			
							// Record CTHasPrimaryIdentifierColumn(CT, Column);
						}
						conceptTypeTable.ColumnCollection.Add(column);
					}
				}
			}
			else
			{
				ReadOnlyCollection<TableIsAlsoForConceptType> tableIsAlsoForConceptTypeInvolvedWithConceptType = TableIsAlsoForConceptType.GetLinksToTable(conceptType);
				foreach (TableIsAlsoForConceptType table in tableIsAlsoForConceptTypeInvolvedWithConceptType)
				{
					foreach (Column column in columnsForConceptType)
					{
						Column clonedColumn = new Column(column.Store, new PropertyAssignment[] { new PropertyAssignment(Column.NameDomainPropertyId, column.Name) });
						AssimilationPath assimPath = new AssimilationPath(assimilationPath[table].Path);

						int inumerator = 0;
						foreach (ConceptTypeAssimilatesConceptType currentConceptTypeAssimilatesConceptType in assimPath.Path)
						{
							columnHasConceptTypeChildPath[column].Insert(inumerator, (ConceptTypeChild)currentConceptTypeAssimilatesConceptType);
							inumerator++;
						}
						Column possibleDuplicateColumn = CheckForDuplicateColumn(table.Table, clonedColumn);
						if (possibleDuplicateColumn == null)
						{
							// if CTC is in PreferredList
							// Record CTCHasPrimaryIdentifierColumn(CT, ClonedColumn);
							table.Table.ColumnCollection.Add(clonedColumn);
						}
					}
				}
			}
		}

		/// <summary>
		/// GetColumnsForConceptTypeChild populates a list of Columns pertaining to a ConceptTypeChild, 
		/// Columns are only added if the ConceptTypeChild is an Information Type, if the ConceptTypeChild is of type
		/// ConceptTypeRelatesToConceptType or ConceptTypeAssimilatesConceptType then it recursively calls itself using 
		/// the preffered target ConceptType of the ConceptTypeChild as the new ConceptTypeChildren, adding the result to the ColumnList.
		/// </summary>
		/// <param name="conceptTypeChild"></param>
		/// <param name="columnHasConceptTypeChildPath">ColumnHasConceptTypeChildPath contains a List of ConceptTypeChildren 
		/// representing the mapping path for a Column.</param>
		/// <param name="conceptTypeChildPath">ConceptTypeChildPath is populated in GetColumnsForConceptTypeChild,
		/// but may already contain ConceptTypeChildren.</param>
		private static List<Column> GetColumnsForConceptTypeChild(ConceptTypeChild conceptTypeChild, Dictionary<Column, List<ConceptTypeChild>> columnHasConceptTypeChildPath, List<ConceptTypeChild> conceptTypeChildPath)
		{
			List<Column> columnList = new List<Column>();
			conceptTypeChildPath.Add(conceptTypeChild);

			if (conceptTypeChild.GetType() == typeof(InformationType))
			{
				Column informationTypeColumn = new Column(conceptTypeChild.Store,
										new PropertyAssignment[]{
											new PropertyAssignment(Column.NameDomainPropertyId, conceptTypeChild.Name)});
				columnHasConceptTypeChildPath[informationTypeColumn] = conceptTypeChildPath;
				columnList.Add(informationTypeColumn);
			}
			else if (conceptTypeChild.GetType() == typeof(ConceptTypeRelatesToConceptType))
			{

				ConceptType relatedConceptType = (ConceptType)conceptTypeChild.Target;
				List<ConceptTypeChild> preferredList = GetPreferredConceptTypeChildrenForConceptType(relatedConceptType);
				foreach (ConceptTypeChild child in preferredList)
				{
					if (child == preferredList[preferredList.Count - 1])
					{
						columnList.AddRange(GetColumnsForConceptTypeChild(child, columnHasConceptTypeChildPath, conceptTypeChildPath));
					}
					else
					{
						List<ConceptTypeChild> clonedConceptTypeHasPathFactType = null;
						columnList.AddRange(GetColumnsForConceptTypeChild(child, columnHasConceptTypeChildPath, clonedConceptTypeHasPathFactType));
					}
				}
				return columnList;
			}
			else if (conceptTypeChild.GetType() == typeof(ConceptTypeAssimilatesConceptType))
			{
				ConceptType targetConceptType = (ConceptType)conceptTypeChild.Target;
				List<ConceptTypeChild> preferredList = GetPreferredConceptTypeChildrenForConceptType(targetConceptType);
				foreach (ConceptTypeChild child in preferredList)
				{
					if (child == preferredList[preferredList.Count - 1])
					{
						columnList.AddRange(GetColumnsForConceptTypeChild(child, columnHasConceptTypeChildPath, conceptTypeChildPath));
					}
					else
					{
						List<ConceptTypeChild> clonedConceptTypeHasPathFactType = conceptTypeChildPath;
						columnList.AddRange(GetColumnsForConceptTypeChild(child, columnHasConceptTypeChildPath, clonedConceptTypeHasPathFactType));
					}
				}
				return columnList;
			}

			return columnList;
		}

		// UNDONE
		/// <summary>
		/// 
		/// </summary>
		/// <param name="conceptTypeTable"></param>
		/// <param name="column"></param>
		private static Column CheckForDuplicateColumn(Table conceptTypeTable, Column column)
		{
			//bool duplicateFound;
			//foreach (Column existingColumn in conceptTypeTable.ColumnCollection)
			//{
			//    duplicateFound = true;
			//    int i = 0;
			//    //foreach (ConceptTypeChild child in column.)
			//    //{

			//    //}
			//}

			return null;
		}

		/// <summary>
		/// GetPreferredConceptTypeChildrenForConceptType parses through the uniquenesses of a single concept type, 
		/// adding all ConceptTypeChildren of any Preferred Uniqueness.  If there are no conceptTypeChildren that meet this critria 
		/// then it checks for anyConceptTypeAssimilatesConceptTypes within the Assimillator collection, and finally through the Assimilated Collection
		/// returning the list of all ConceptTypeChildren that are Prefered for the ConceptType.
		/// </summary>
		private static List<ConceptTypeChild> GetPreferredConceptTypeChildrenForConceptType(ConceptType conceptType)
		{
			List<ConceptTypeChild> prefferedConceptTypeChildrenList = new List<ConceptTypeChild>();
			foreach (Uniqueness uniqueness in conceptType.UniquenessCollection)
			{
				if (uniqueness.IsPreferred)
				{
					foreach (ConceptTypeChild childConceptType in uniqueness.ConceptTypeChildCollection)
					{
						prefferedConceptTypeChildrenList.Add(childConceptType);
					}
					break;
				}
			}
			if (prefferedConceptTypeChildrenList.Count == 0)
			{
				foreach (ConceptTypeAssimilatesConceptType conceptTypeAssimilatesConceptType
					in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType))
				{
					if (conceptTypeAssimilatesConceptType.IsPreferredForTarget)
					{
						prefferedConceptTypeChildrenList.Add(conceptTypeAssimilatesConceptType);
						break;
					}
				}
			}
			if (prefferedConceptTypeChildrenList.Count == 0)
			{
				foreach (ConceptTypeAssimilatesConceptType conceptTypeAssimilatesConceptType
					in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(conceptType))
				{
					if (conceptTypeAssimilatesConceptType.IsPreferredForParent)
					{
						prefferedConceptTypeChildrenList.Add(conceptTypeAssimilatesConceptType);
						break;
					}
				}
			}
			return prefferedConceptTypeChildrenList;
		}

#if DO_SEPARATION
			// UNDONE
			private void DoSeparation(ConceptTypeAssimilatesConceptType assimilation, ref bool isPreferredForChildFound)
			{
				throw new Exception("The method or operation is not implemented.");
			}
#endif // DO_SEPARATION

		/// <summary>
		/// CreateForeignKeys looks at a table and creates the foreign keys between it.
		/// </summary>
		/// <param name="table">The table to check for foreign keys on.</param>
		/// <param name="store">The <see cref="Store" />.</param>
		/// <param name="columnHasConceptTypeChildPath">ColumnHasConceptTypeChildPath contains a List of ConceptTypeChildren representing the mapping path for a Column.</param>
		private static void CreateForeignKeys(Table table, Store store, Dictionary<Column, List<ConceptTypeChild>> columnHasConceptTypeChildPath)
		{
			Dictionary<ConceptTypeRelatesToConceptType, List<Column>> foreignKeyList = new Dictionary<ConceptTypeRelatesToConceptType, List<Column>>();

			foreach (Column column in table.ColumnCollection)
			{
				ConceptTypeRelatesToConceptType conceptTypeRelatesToConceptType = null;
				conceptTypeRelatesToConceptType = GetRelatedConceptTypeForColumn(column, columnHasConceptTypeChildPath);

				if (conceptTypeRelatesToConceptType != null)
				{
					if (foreignKeyList.ContainsKey(conceptTypeRelatesToConceptType))
					{
						foreignKeyList[conceptTypeRelatesToConceptType].Add(column);
					}
					else
					{
						foreignKeyList[conceptTypeRelatesToConceptType] = new List<Column>();
						foreignKeyList[conceptTypeRelatesToConceptType].Add(column);
					}
				}
			}

			foreach (KeyValuePair<ConceptTypeRelatesToConceptType, List<Column>> keyValuePair in foreignKeyList)
			{
				ReferenceConstraint referenceConstraint = new ReferenceConstraint(store);
				foreach (Column column in keyValuePair.Value)
				{
					Column targetColumn = FindTarget(column, keyValuePair.Key.ReferencedConceptType, columnHasConceptTypeChildPath);
					ColumnReference columnReference = new ColumnReference(column, targetColumn);
					referenceConstraint.ColumnReferenceCollection.Add(columnReference);
					TableContainsReferenceConstraint tableContainsReferenceConstraint = new TableContainsReferenceConstraint(table, referenceConstraint);
					ReferenceConstraintTargetsTable referenceConstraintTargetsTable = new ReferenceConstraintTargetsTable(referenceConstraint, targetColumn.Table);
				}
			}
		}

		/// <summary>
		/// FindTarget finds the target column for an input <see cref="ConceptType" />.
		/// </summary>
		/// <param name="column">The <see cref="Column"/>.</param>
		/// <param name="conceptType">The <see cref="ConceptType"/>.</param>
		/// <param name="columnHasConceptTypeChildPath">ColumnHasConceptTypeChildPath contains a List of ConceptTypeChildren representing the mapping path for a Column.</param>
		/// <returns>The target <see cref="Column"/>.</returns>
		private static Column FindTarget(Column column, ConceptType conceptType, Dictionary<Column, List<ConceptTypeChild>> columnHasConceptTypeChildPath)
		{
			bool targetFound = true;

			//if (TableIsAlsoForConceptType.GetLinksToTable(conceptType).Count == 1)
			//{
			List<Column> columns = new List<Column>();
			foreach (Uniqueness uniqueness in ConceptTypeHasUniqueness.GetUniquenessCollection(conceptType))
			{
				if (uniqueness.IsPreferred)
				{
					foreach (ConceptTypeChild containedChild in uniqueness.ConceptTypeChildCollection)
					{
						foreach (Column target in columnHasConceptTypeChildPath.Keys)
						{
							if (columnHasConceptTypeChildPath[target].Contains(containedChild) && target != column)
							{
								columns.Add(target);
							}
						}
					}
					//foreach (UniquenessConstraint uniquenessConstraint in UniquenessConstraintIsForUniqueness.GetUniquenessConstraint(uniqueness))
					//{
					//    columns.AddRange(uniquenessConstraint.ColumnCollection);
					//}
				}
			}

			foreach (Column targetColumn in columns)
			{
				for (int i = 0; i < ColumnHasConceptTypeChild.GetConceptTypeChildPath(targetColumn).Count; ++i)
				{
					if (ColumnHasConceptTypeChild.GetConceptTypeChildPath(column)[i + 1] != ColumnHasConceptTypeChild.GetConceptTypeChildPath(targetColumn)[i])
					{
						targetFound = false;
						break;
					}
				}

				if (targetFound)
				{
					return targetColumn;
				}
			}
			return null;
		}

		/// <summary>
		/// CreateUniquenessConstraints looks at a <see cref="ConceptType"/> and determines if it is unique or not.
		/// </summary>
		/// <param name="conceptType">The <see cref="ConceptType"/>.</param>
		/// <param name="assimilationPath">The assimilation path between a <see cref="Table"/> and <see cref="ConceptType"/>.</param>
		/// <param name="columnHasConceptTypeChildPath">ColumnHasConceptTypeChildPath contains a List of ConceptTypeChildren representing the mapping path for a Column.</param>
		private static void CreateUniquenessConstraints(ConceptType conceptType, Dictionary<TableIsAlsoForConceptType, AssimilationPath> assimilationPath, Dictionary<Column, List<ConceptTypeChild>> columnHasConceptTypeChildPath)
		{
			Table isPrimarilyForTable = TableIsPrimarilyForConceptType.GetTable(conceptType);
			if (isPrimarilyForTable != null)
			{
				foreach (Uniqueness uniqueness in conceptType.UniquenessCollection)
				{
					UniquenessConstraint uniquenessConstraint = new UniquenessConstraint(uniqueness.Store, new PropertyAssignment[] { new PropertyAssignment(UniquenessConstraint.NameDomainPropertyId, uniqueness.Name) });
					foreach (Column myColumn in isPrimarilyForTable.ColumnCollection)
					{
						foreach (ConceptTypeChild conceptTypeChild in uniqueness.ConceptTypeChildCollection)
						{
							if (columnHasConceptTypeChildPath[myColumn].Count > 0 && columnHasConceptTypeChildPath[myColumn][0] == conceptTypeChild)
							{
								UniquenessConstraintIncludesColumn uniquenessConstraintIncludesColumn = new UniquenessConstraintIncludesColumn(uniquenessConstraint, myColumn);
								if (uniqueness.IsPreferred)
								{
									uniquenessConstraint.IsPrimary = true;
								}
							}
						}
						if (!isPrimarilyForTable.UniquenessConstraintCollection.Contains(uniquenessConstraint))
						{
							isPrimarilyForTable.UniquenessConstraintCollection.Add(uniquenessConstraint);
							UniquenessConstraintIsForUniqueness uniquenessConstraintIsForUniqueness = new UniquenessConstraintIsForUniqueness(uniquenessConstraint, uniqueness);
						}
					}
				}
			}
			else
			{
				foreach (Uniqueness uniqueness in conceptType.UniquenessCollection)
				{
					foreach (Table table in TableIsAlsoForConceptType.GetTable(conceptType))
					{
						UniquenessConstraint uniquenessConstraint = null;
						foreach (Column column in table.ColumnCollection)
						{
							foreach (ConceptTypeChild conceptTypeChild in uniqueness.ConceptTypeChildCollection)
							{
								TableIsAlsoForConceptType tableIsAlsoForConceptType = new TableIsAlsoForConceptType(table, conceptType);
								if (columnHasConceptTypeChildPath[column][0] == assimilationPath[tableIsAlsoForConceptType].Path[0] && columnHasConceptTypeChildPath[column][1] == conceptTypeChild)
								{
									UniquenessConstraintIncludesColumn uniquenessConstraintIncludesColumn = new UniquenessConstraintIncludesColumn(uniquenessConstraint, column);
								}
							}
							table.UniquenessConstraintCollection.Add(uniquenessConstraint);
						}
						UniquenessConstraintIsForUniqueness uniquenessConstraintIsForUniqueness = new UniquenessConstraintIsForUniqueness(uniquenessConstraint, uniqueness);
					}
				}
			}
		}

		/// <summary>
		/// Retrieves a <see cref="ConceptTypeRelatesToConceptType"/> relation for the input <see cref="Column"/>.
		/// </summary>
		/// <param name="column">The <see cref="Column"/>.</param>
		/// <param name="columnHasConceptTypeChildPath">ColumnHasConceptTypeChildPath contains a List of ConceptTypeChildren representing the mapping path for a Column.</param>
		/// <returns>A <see cref="ConceptTypeRelatesToConceptType"/>.</returns>
		private static ConceptTypeRelatesToConceptType GetRelatedConceptTypeForColumn(Column column, Dictionary<Column, List<ConceptTypeChild>> columnHasConceptTypeChildPath)
		{
			foreach (ConceptTypeChild conceptTypeChild in columnHasConceptTypeChildPath[column]) // ColumnHasConceptTypeChild.GetConceptTypeChildPath(column)) - GetCTCPath() is coded in, but no methods to add to the CTC Path, Using Dictionary Instead.
			{
				if (conceptTypeChild.GetType() == typeof(InformationType))
				{
					return null;
				}
				else if (conceptTypeChild.GetType() == typeof(ConceptTypeRelatesToConceptType))
				{
					return (ConceptTypeRelatesToConceptType)conceptTypeChild;
				}
				else if (conceptTypeChild.GetType() == typeof(ConceptTypeAssimilatesConceptType))
				{
					if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation((ConceptTypeAssimilatesConceptType)conceptTypeChild) == AssimilationAbsorptionChoice.Separate)
					{ return null; }
				}
			}
			return null;
		}
		
		#endregion // Fully populate from OIAL
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

					FullyGenerateConceptualDatabaseModel(schema, element);

				}
			}
		}
		#endregion // GenerateConceptualDatabaseFixupListener class
	}


}
