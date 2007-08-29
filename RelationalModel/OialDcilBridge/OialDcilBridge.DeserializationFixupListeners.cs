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
		/// Validate Customization Options
		/// </summary>
		ValidateCustomizationOptions = (int)ORMToORMAbstractionBridgeDeserializationFixupPhase.CreateImplicitElements + 5,
		/// <summary>
		/// Validate bridge elements after all core ORM validation is complete
		/// </summary>
		ValidateElements = (int)ORMToORMAbstractionBridgeDeserializationFixupPhase.CreateImplicitElements + 10,
	}
	public partial class ORMAbstractionToConceptualDatabaseBridgeDomainModel : IDeserializationFixupListenerProvider
	{
		#region Fully populate from OIAL
		#region AssimilationPath structure

		// UNDONE#1 - Not correctly mapping the keys of an abosrbed subtype, problem with ConceptTypeRelatesToConceptType??
		// It's working now, but when doing a case with seperation ( C(.id) one-many/spanning B(.id) subtype of A(.id) the foreign key on table C-Has-A is going to A.id not B.id =(

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
		/// Generates the conceptual database model for a given Schema and AbstractionModel.  
		/// The generation of the Conceptual Database Model includes the population of all tables with 
		/// Columns, Uniquenesses, Foreign Keys, and Manditory restrictions.
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
			
			// When there is a assimilation path larger then a single assimilation, it is possible the grandchild elements 
			// will not intially map to the superparent table
			// With the do / while we iterate through the assimilations, mapping the closest, and then using that mapping to find the correct mapping
			// for it's children.
			do
			{
				foreach (KeyValuePair<ConceptType, ConceptTypeAssimilatesConceptType> keyedAssimilation in assimilatedConceptTypes)
				{
						if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(keyedAssimilation.Value) == AssimilationAbsorptionChoice.Absorb)
						{
							MapAbsorbedConceptType(keyedAssimilation.Key, pathDictonary, allAssimilations, assimilationPath, new List<ConceptTypeAssimilatesConceptType>());
						}
						else if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(keyedAssimilation.Value) == AssimilationAbsorptionChoice.Partition)
						{
							MapPartitionedConceptType(keyedAssimilation.Key, mappingPaths, pathDictonary, allAssimilations, assimilationPath, new List<ConceptTypeAssimilatesConceptType>());
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
			Collection<ConceptTypeAssimilatesConceptType> seperatedConceptTypes = new Collection<ConceptTypeAssimilatesConceptType>();
			foreach (ConceptType conceptType in conceptTypes)
			{
				CreateColumns(conceptType, assimilationPath);

				bool isPreferredForChildFound = false;
				foreach (ConceptTypeAssimilatesConceptType conceptTypeAssimilation in GetAssimilationsForConceptType(conceptType))
				{
					if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(conceptTypeAssimilation) == AssimilationAbsorptionChoice.Separate && !seperatedConceptTypes.Contains(conceptTypeAssimilation))
					{
						DoSeparation(conceptTypeAssimilation, ref isPreferredForChildFound);
						seperatedConceptTypes.Add(conceptTypeAssimilation);
					}
				}

				CreateUniquenessConstraints(conceptType, assimilationPath);
			}

			foreach (Table table in schema.TableCollection)
			{
				CreateForeignKeys(table);
				GenerateMandatoryConstraints(table);
			}

			// Change all names to a more apropriate verson.
			NameGeneration.GenerateAllNames(schema);
		}

		/// <summary>
		/// Gets all ConceptType Assimilates ConceptType retaitions containing a given ConceptType as either the Asimmilator 
		/// or the Assimilated ConceptType.
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
		/// Maps all Absorbed ConceptTypes
		/// for a ConceptType it finds the Primary Tables that they map to, 
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
						ConceptType targetMappingPath = assimilatedConceptTypes[assimilation.AssimilatorConceptType];
						if (targetMappingPath != null)
						{
							assimilatedConceptTypes[conceptType] = targetMappingPath;
							TableIsAlsoForConceptType tableForConceptType = new TableIsAlsoForConceptType(TableIsPrimarilyForConceptType.GetTable(targetMappingPath), conceptType);
							assimilationPathDictionary[tableForConceptType] = new AssimilationPath(assimilationPath);
						}
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
					if (isPrimarilyForTable != null && !TableIsAlsoForConceptType.GetConceptType(isPrimarilyForTable).Contains(conceptType))
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
		private static void CreateColumns(ConceptType conceptType, Dictionary<TableIsAlsoForConceptType, AssimilationPath> assimilationPath)
		{
			List<ConceptTypeChild> preferredConceptTypeChildList = GetPreferredConceptTypeChildrenForConceptType(conceptType);
			List<Column> columnsForConceptType = new List<Column>();

			List<ConceptTypeChild> tester = new List<ConceptTypeChild>();

			foreach (ConceptTypeChild conceptTypeChild in ConceptTypeChild.GetLinksToTargetCollection(conceptType))
			{
				tester.Add(conceptTypeChild);
				if (conceptTypeChild.GetType() != typeof(ConceptTypeAssimilatesConceptType))
				{
					columnsForConceptType.AddRange(GetColumnsForConceptTypeChild(conceptTypeChild, new List<ConceptTypeChild>()));
				}
			}
			foreach (ConceptTypeChild assimilatedConceptType in preferredConceptTypeChildList)
			{
				if (assimilatedConceptType.GetType() == typeof(ConceptTypeAssimilatesConceptType))
				{
					columnsForConceptType.AddRange(GetColumnsForConceptTypeChild(assimilatedConceptType, new List<ConceptTypeChild>()));
				}
				else if (assimilatedConceptType.GetType() == typeof(InformationType))
				{
					//columnsForConceptType.AddRange(GetColumnsForConceptTypeChild(assimilatedConceptType, new List<ConceptTypeChild>()));
				}
			}
			// TESTING - UNDONE#1 -- Not needed, should be contained within the first Foreach
			//foreach(ConceptTypeChild relatedConceptType in preferredConceptTypeChildList)
			//{
			//	if (relatedConceptTypeChild.GetType() == typeof(ConceptTypeRelatesToConceptType))
			//	{ 
			//		columnsForConceptType.AddRange(GetColumnsForConceptTypeChild(relatedConceptType, new List<ConceptTypeChild>()));
			//	}
			//}
			// END TESTING
			Table conceptTypeTable = TableIsPrimarilyForConceptType.GetTable(conceptType);
			if (conceptTypeTable != null)
			{
				foreach (Column column in columnsForConceptType)
				{
					Column possibleDuplicateColumn = CheckForDuplicateColumn(conceptTypeTable, column);
					if (possibleDuplicateColumn != null)
					{
						if (preferredConceptTypeChildList.Contains(ColumnHasConceptTypeChild.GetConceptTypeChildPath(possibleDuplicateColumn)[0]))
						{
							// Record CTHasPrimaryIdentifierColumn(CT, "PossibleDuplicateColumn");
						}
					}
					else
					{
						if (preferredConceptTypeChildList.Contains(ColumnHasConceptTypeChild.GetConceptTypeChildPath(column)[0]))
						{
							// Record CTHasPrimaryIdentifierColumn(CT, Column);
						}
						conceptTypeTable.ColumnCollection.Add(column);
					}
				}
			}
			else
			{
				//ReadOnlyCollection<Table> tablesInvolvedWithConceptType = TableIsAlsoForConceptType.GetTable(conceptType);
				//foreach (Table table in tablesInvolvedWithConceptType)
				//{
				//    foreach (Column column in columnsForConceptType)
				//    {
				//        // UNDONE: Why do we need to clone these here, where do we use it?
				//        Column clonedColumn = new Column(column.Store, new PropertyAssignment[] { new PropertyAssignment(Column.NameDomainPropertyId, column.Name) });
				//        List<ConceptTypeChild> clonedConceptTypeChildPath = new List<ConceptTypeChild>(ColumnHasConceptTypeChild.GetConceptTypeChildPath(column));

				//        AssimilationPath assimPath = new AssimilationPath(assimilationPath[table].Path);

				//        int inumerator = 0;

				//        foreach (ConceptTypeAssimilatesConceptType currentConceptTypeAssimilatesConceptType in assimPath.Path)
				//        {
				//            clonedConceptTypeChildPath.Insert(inumerator, (ConceptTypeChild)currentConceptTypeAssimilatesConceptType);
				//            inumerator++;
				//        }

				//        Column possibleDuplicateColumn = CheckForDuplicateColumn(table.Table, clonedColumn);
				//        if (possibleDuplicateColumn == null)
				//        {
				//            // if CTC is in PreferredList
				//            // Record CTCHasPrimaryIdentifierColumn(CT, ClonedColumn);
				//            table.Table.ColumnCollection.Add(clonedColumn);
				//        }
				//    }
				//}
				ReadOnlyCollection<TableIsAlsoForConceptType> tableIsAlsoForConceptTypeInvolvedWithConceptType = TableIsAlsoForConceptType.GetLinksToTable(conceptType);
				foreach (TableIsAlsoForConceptType table in tableIsAlsoForConceptTypeInvolvedWithConceptType)
				{
					foreach (Column column in columnsForConceptType)
					{
						Column clonedColumn = new Column(column.Store, new PropertyAssignment[] { new PropertyAssignment(Column.NameDomainPropertyId, column.Name) });
						List<ConceptTypeChild> clonedConceptTypeChildPath = new List<ConceptTypeChild>(ColumnHasConceptTypeChild.GetConceptTypeChildPath(column));
						if (!assimilationPath.ContainsKey(table))
						{
							assimilationPath[table] = new AssimilationPath(new List<ConceptTypeAssimilatesConceptType>(ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(conceptType)));
						}

						AssimilationPath assimPath = new AssimilationPath(assimilationPath[table].Path);

						int inumerator = 0;
						foreach (ConceptTypeAssimilatesConceptType currentConceptTypeAssimilatesConceptType in assimPath.Path)
						{
							clonedConceptTypeChildPath.Insert(inumerator, (ConceptTypeChild)currentConceptTypeAssimilatesConceptType);
							inumerator++;
						}
						Column possibleDuplicateColumn = CheckForDuplicateColumn(table.Table, clonedColumn);
						if (possibleDuplicateColumn == null)
						{
							// if CTC is in PreferredList
							// Record CTCHasPrimaryIdentifierColumn(CT, ClonedColumn);
							ColumnHasConceptTypeChild.GetConceptTypeChildPath(clonedColumn).AddRange(clonedConceptTypeChildPath);
							table.Table.ColumnCollection.Add(clonedColumn);
						}
						else
						{
							clonedColumn.Delete();
						}

					}
				}

				foreach (Column column in columnsForConceptType)
				{
					column.Delete();
				}
			}
		}

		/// <summary>
		/// GetColumnsForConceptTypeChild gets the <see cref="Column"/> coresponding to a <see cref="ConceptTypeChild"/>.
		/// </summary>
		/// <remarks>
		/// GetColumnsForConceptTypeChild populates a list of Columns pertaining to a ConceptTypeChild, 
		/// Columns are only added if the ConceptTypeChild is an Information Type, if the ConceptTypeChild is of type
		/// ConceptTypeRelatesToConceptType or ConceptTypeAssimilatesConceptType then it recursively calls itself using 
		/// the preffered target ConceptType of the ConceptTypeChild as the new ConceptTypeChildren, adding the result to the ColumnList.
		/// </remarks>
		/// <param name="conceptTypeChild"></param>
		/// <param name="conceptTypeChildPath">ConceptTypeChildPath is populated in GetColumnsForConceptTypeChild,
		/// but may already contain ConceptTypeChildren.</param>
		private static List<Column> GetColumnsForConceptTypeChild(ConceptTypeChild conceptTypeChild, List<ConceptTypeChild> conceptTypeChildPath)
		{
			List<Column> columnList = new List<Column>();
			conceptTypeChildPath.Add(conceptTypeChild);

			if (conceptTypeChild.GetType() == typeof(InformationType))
			{
				Column informationTypeColumn = new Column(conceptTypeChild.Store,
											new PropertyAssignment[]{
											new PropertyAssignment(Column.NameDomainPropertyId, conceptTypeChild.Name)});
				ColumnHasConceptTypeChild.GetConceptTypeChildPath(informationTypeColumn).Clear();
				ColumnHasConceptTypeChild.GetConceptTypeChildPath(informationTypeColumn).AddRange(conceptTypeChildPath);
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
						columnList.AddRange(GetColumnsForConceptTypeChild(child, conceptTypeChildPath));
					}
					else
					{
						List<ConceptTypeChild> clonedConceptTypeChildPath = new List<ConceptTypeChild>(conceptTypeChildPath);
						columnList.AddRange(GetColumnsForConceptTypeChild(child, clonedConceptTypeChildPath));
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
						columnList.AddRange(GetColumnsForConceptTypeChild(child, conceptTypeChildPath));
					}
					else
					{
						List<ConceptTypeChild> clonedConceptTypeChildPath = new List<ConceptTypeChild>(conceptTypeChildPath);
						columnList.AddRange(GetColumnsForConceptTypeChild(child, clonedConceptTypeChildPath));
					}
				}
				return columnList;
			}

			return columnList;
		}

		/// <summary>
		/// Returns null, or any <see cref="Column"/> found that containts a duplicate ConceptTypeChild path.
		/// </summary>
		private static Column CheckForDuplicateColumn(Table conceptTypeTable, Column column)
		{
			bool duplicateFound;
			foreach (Column existingColumn in conceptTypeTable.ColumnCollection)
			{
				duplicateFound = true;

				LinkedElementCollection<ConceptTypeChild> columnConceptTypeChildList = ColumnHasConceptTypeChild.GetConceptTypeChildPath(column);
				LinkedElementCollection<ConceptTypeChild> existingColumnConceptTypeChildList = ColumnHasConceptTypeChild.GetConceptTypeChildPath(existingColumn);

				if (columnConceptTypeChildList.Count == existingColumnConceptTypeChildList.Count)
				{
					int pathCount = columnConceptTypeChildList.Count;
					for (int i = 0; i < pathCount; ++i)
					{
						if (columnConceptTypeChildList[i] != existingColumnConceptTypeChildList[i])
						{
							duplicateFound = false;
						}
					}
					if (duplicateFound)
					{
						return existingColumn;
					}
				}
				else
				{
					duplicateFound = false;
				}
			}

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
			// TESTING
			if (prefferedConceptTypeChildrenList.Count == 0)
			{
				foreach (ConceptTypeAssimilatesConceptType conceptTypeAssimilatesConceptType
					in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType))
				{
					prefferedConceptTypeChildrenList.AddRange(GetPreferredConceptTypeChildrenForConceptType(conceptTypeAssimilatesConceptType.AssimilatorConceptType));
					break;

				}
			}
			if (prefferedConceptTypeChildrenList.Count == 0)
			{
				foreach (ConceptTypeAssimilatesConceptType conceptTypeAssimilatesConceptType
					in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(conceptType))
				{
					prefferedConceptTypeChildrenList.AddRange(GetPreferredConceptTypeChildrenForConceptType(conceptTypeAssimilatesConceptType.AssimilatedConceptType));
					break;
				}
			}

			return prefferedConceptTypeChildrenList;
		}

		/// <summary>
		/// DoSeperation handles the generation of columns and foreign keys when an assimilation is set to seperate.
		/// </summary>
		private static void DoSeparation(ConceptTypeAssimilatesConceptType assimilation, ref bool isPreferredForChildFound)
		{
			Table table = TableIsPrimarilyForConceptType.GetTable(assimilation.AssimilatedConceptType);
			if (table != null)
			{
				if (assimilation.IsPreferredForParent)
				{
					ReferenceConstraint referenceConstraint = new ReferenceConstraint(assimilation.Store, new PropertyAssignment[] { new PropertyAssignment(ReferenceConstraint.NameDomainPropertyId, assimilation.Name) });
					TableContainsReferenceConstraint tableContainsReferenceConstraint = new TableContainsReferenceConstraint(TableIsPrimarilyForConceptType.GetTable(assimilation.AssimilatorConceptType), referenceConstraint);
					ReferenceConstraintTargetsTable referenceConstraintTargetsTable = new ReferenceConstraintTargetsTable(referenceConstraint, table);

					UniquenessConstraint mappedConstraint = new UniquenessConstraint(assimilation.Store, new PropertyAssignment[] { new PropertyAssignment(UniquenessConstraint.NameDomainPropertyId, "Constraint") });
					mappedConstraint.IsPrimary = true;

					List<Column> sourceColumns = ConceptTypeHasPrimaryIdentifierColumns(null, assimilation.Parent);
					foreach (Column sourcecolumn in sourceColumns)
					{
						Column targetColumn = FindTarget(sourcecolumn, assimilation.AssimilatedConceptType);
						ColumnReference relationship = new ColumnReference(sourcecolumn, targetColumn);
						referenceConstraint.ColumnReferenceCollection.Add(relationship);
						mappedConstraint.ColumnCollection.Add(sourcecolumn);
					}
					TableIsPrimarilyForConceptType.GetTable(assimilation.AssimilatorConceptType).UniquenessConstraintCollection.Add(mappedConstraint);

				}
				else if (assimilation.IsPreferredForTarget)
				{
					if (isPreferredForChildFound == false)
					{
						ReferenceConstraint referenceConstraint = new ReferenceConstraint(assimilation.Store, new PropertyAssignment[] { new PropertyAssignment(ReferenceConstraint.NameDomainPropertyId, assimilation.Name) });
						TableContainsReferenceConstraint tableContainsReferenceConstraint = new TableContainsReferenceConstraint(table, referenceConstraint);
						ReferenceConstraintTargetsTable referenceConstraintTargetsTable = new ReferenceConstraintTargetsTable(referenceConstraint, TableIsPrimarilyForConceptType.GetTable(assimilation.AssimilatorConceptType));

						UniquenessConstraint mappedConstraint = new UniquenessConstraint(assimilation.Store, new PropertyAssignment[] { new PropertyAssignment(UniquenessConstraint.NameDomainPropertyId, "Constraint") });
						mappedConstraint.IsPrimary = true;

						List<Column> sourceColumns = ConceptTypeHasPrimaryIdentifierColumns(null, assimilation.AssimilatedConceptType);
						foreach (Column sourcecolumn in sourceColumns)
						{
							Column targetColumn = FindTarget(sourcecolumn, assimilation.AssimilatorConceptType);
							ColumnReference relationship = new ColumnReference(sourcecolumn, targetColumn);
							referenceConstraint.ColumnReferenceCollection.Add(relationship);
							mappedConstraint.ColumnCollection.Add(sourcecolumn);
						}
						table.UniquenessConstraintCollection.Add(mappedConstraint);
						isPreferredForChildFound = true;
					}
				}
				else if (assimilation.IsMandatory)
				{
					ReferenceConstraint referenceConstraint = new ReferenceConstraint(assimilation.Store, new PropertyAssignment[] { new PropertyAssignment(ReferenceConstraint.NameDomainPropertyId, assimilation.Name) });
					TableContainsReferenceConstraint tableContainsReferenceConstraint = new TableContainsReferenceConstraint(TableIsPrimarilyForConceptType.GetTable(assimilation.AssimilatedConceptType), referenceConstraint);
					ReferenceConstraintTargetsTable referenceConstraintTargetsTable = new ReferenceConstraintTargetsTable(referenceConstraint, TableIsPrimarilyForConceptType.GetTable(assimilation.AssimilatorConceptType));
					LinkedElementCollection<ConceptType> assimilations;
					ConceptType conceptType = assimilation.AssimilatedConceptType;
					List<Column> primaryIdentifierColumns = new List<Column>();

					bool primaryIdentifierFound = false;
					while ((assimilations = conceptType.AssimilatorConceptTypeCollection).Count != 0 && !primaryIdentifierFound)
					{
						Table primaryTable = TableIsPrimarilyForConceptType.GetTable(conceptType);

						conceptType = assimilations[0];
						if (primaryTable != null)
						{

							foreach (UniquenessConstraint uniqueness in primaryTable.UniquenessConstraintCollection)
							{
								if (uniqueness.IsPrimary)
								{
									primaryIdentifierColumns.AddRange(uniqueness.ColumnCollection);
									assimilation.IsMandatory = true;
									primaryIdentifierFound = true;
								}
							}
						}

					}
					if (primaryIdentifierColumns.Count == 0)
					{
						primaryIdentifierColumns.AddRange(GetColumnsForConceptTypeChild(assimilation as ConceptTypeChild, new List<ConceptTypeChild>()));
					}

					foreach (Column identifierColumn in primaryIdentifierColumns)
					{
						Column clonedColumn = new Column(identifierColumn.Store, new PropertyAssignment[] { new PropertyAssignment(Column.NameDomainPropertyId, identifierColumn.Name) });
						List<ConceptTypeChild> clonedConceptTypeChildPath = new List<ConceptTypeChild>(ColumnHasConceptTypeChild.GetConceptTypeChildPath(identifierColumn));

						foreach (ConceptTypeChild conceptTypeChild in clonedConceptTypeChildPath)
						{
							conceptTypeChild.IsMandatory = true;
						}
						ColumnHasConceptTypeChild.GetConceptTypeChildPath(clonedColumn).Clear();
						ColumnHasConceptTypeChild.GetConceptTypeChildPath(clonedColumn).AddRange(clonedConceptTypeChildPath);

						TableIsPrimarilyForConceptType.GetTable(assimilation.AssimilatedConceptType).ColumnCollection.Add(clonedColumn);
						ColumnReference relationship = new ColumnReference(clonedColumn, identifierColumn);
						referenceConstraint.ColumnReferenceCollection.Add(relationship);
					}
				}
				else
				{
					Table targetTable = TableIsPrimarilyForConceptType.GetTable(assimilation.AssimilatorConceptType);

					if (targetTable != null)
					{
						ReferenceConstraint referenceConstraint = new ReferenceConstraint(assimilation.Store, new PropertyAssignment[] { new PropertyAssignment(ReferenceConstraint.NameDomainPropertyId, assimilation.Name) });
						TableContainsReferenceConstraint tableContainsReferenceConstraint = new TableContainsReferenceConstraint(table, referenceConstraint);
						ReferenceConstraintTargetsTable referenceConstraintTargetsTable = new ReferenceConstraintTargetsTable(referenceConstraint, targetTable);

						UniquenessConstraint mappedConstraint = new UniquenessConstraint(assimilation.Store, new PropertyAssignment[] { new PropertyAssignment(UniquenessConstraint.NameDomainPropertyId, "Constraint") });
						mappedConstraint.IsPrimary = true;

						/*****************************/
						//InformationType matchingBaseType = ColumnHasConceptTypeChild.GetConceptTypeChildPath(column)[ColumnHasConceptTypeChild.GetConceptTypeChildPath(column).Count - 1] as InformationType;

						LinkedElementCollection<ConceptType> assimilations;
						ConceptType conceptType = assimilation.AssimilatedConceptType;
						List<Column> primaryIdentifierColumns = new List<Column>();

						bool primaryIdentifierFound = false;
						while ((assimilations = conceptType.AssimilatorConceptTypeCollection).Count != 0 && !primaryIdentifierFound)
						{
							conceptType = assimilations[0];

							Table primaryTable = TableIsPrimarilyForConceptType.GetTable(conceptType);

							if (primaryTable != null)
							{
								foreach (UniquenessConstraint uniqueness in primaryTable.UniquenessConstraintCollection)
								{
									if (uniqueness.IsPrimary)
									{
										primaryIdentifierColumns.AddRange(uniqueness.ColumnCollection);
										assimilation.IsMandatory = true;
										primaryIdentifierFound = true;
									}
								}
							}
						}
						if (primaryIdentifierColumns.Count == 0)
						{
							primaryIdentifierColumns.AddRange(GetColumnsForConceptTypeChild(assimilation as ConceptTypeChild, new List<ConceptTypeChild>()));
						}
						/*************************/



						foreach (Column identifierColumn in primaryIdentifierColumns)
						{
							LinkedElementCollection<ConceptTypeChild> ctcpath = ColumnHasConceptTypeChild.GetConceptTypeChildPath(identifierColumn);
							foreach (ConceptTypeChild ctc in ctcpath)
							{
								ctc.IsMandatory = true;
							}
							Column clonedColumn = new Column(identifierColumn.Store, new PropertyAssignment[] { new PropertyAssignment(Column.NameDomainPropertyId, identifierColumn.Name) });
							List<ConceptTypeChild> clonedConceptTypeChildPath = new List<ConceptTypeChild>(ColumnHasConceptTypeChild.GetConceptTypeChildPath(identifierColumn));
							ColumnHasConceptTypeChild.GetConceptTypeChildPath(clonedColumn).Clear();
							ColumnHasConceptTypeChild.GetConceptTypeChildPath(clonedColumn).AddRange(clonedConceptTypeChildPath);
							table.ColumnCollection.Add(clonedColumn);
							ColumnReference relationship = new ColumnReference(clonedColumn, identifierColumn);
							referenceConstraint.ColumnReferenceCollection.Add(relationship);

							mappedConstraint.ColumnCollection.Add(clonedColumn);
						}
						table.UniquenessConstraintCollection.Add(mappedConstraint);
					}
				}
			}
		}

		/// <summary>
		/// CreateForeignKeys looks at a table and generates all Foreign Keys (<see cref="ReferenceConstraint"/>) required by the columns in that table.
		/// </summary>
		/// <param name="table">The table to check for foreign keys on.</param>
		private static void CreateForeignKeys(Table table)
		{
			Dictionary<ConceptTypeRelatesToConceptType, List<Column>> foreignKeyList = new Dictionary<ConceptTypeRelatesToConceptType, List<Column>>();

			foreach (Column column in table.ColumnCollection)
			{
				ConceptTypeRelatesToConceptType conceptTypeRelatesToConceptType = null;
				conceptTypeRelatesToConceptType = GetRelatedConceptTypeForColumn(column);

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
			Store store = table.Store;
			foreach (KeyValuePair<ConceptTypeRelatesToConceptType, List<Column>> keyValuePair in foreignKeyList)
			{
				ReferenceConstraint referenceConstraint = null;
				LinkedElementCollection<ColumnReference> referenceColumns = null;
				foreach (Column column in keyValuePair.Value)
				{
					Column targetColumn = FindTarget(column, keyValuePair.Key.ReferencedConceptType);
					if (targetColumn == null || column == null)
					{
						break;
					}
					if (referenceConstraint == null)
					{
						referenceConstraint = new ReferenceConstraint(store, new PropertyAssignment[] { new PropertyAssignment(ReferenceConstraint.NameDomainPropertyId, keyValuePair.Key.Name) });
						// Add it to the table
						new TableContainsReferenceConstraint(table, referenceConstraint);
						referenceColumns = referenceConstraint.ColumnReferenceCollection;
						new ReferenceConstraintTargetsTable(referenceConstraint, targetColumn.Table);
					}
					referenceColumns.Add(new ColumnReference(column, targetColumn));
				}
			}
		}

		/// <summary>
		/// Returns a collection of <see cref="Column"/> elements that are preferred identifiers for <see cref="ConceptType"/>.
		/// </summary>
		private static List<Column> ConceptTypeHasPrimaryIdentifierColumns(Column column, ConceptType conceptType)
		{
			List<Column> columns = new List<Column>();
			foreach (Uniqueness uniqueness in ConceptTypeHasUniqueness.GetUniquenessCollection(conceptType))
			{
				if (uniqueness.IsPreferred)
				{
					foreach (ConceptTypeChild containedChild in uniqueness.ConceptTypeChildCollection)
					{
						foreach (Column target in ColumnHasConceptTypeChild.GetColumn(containedChild))
						{
							if (ColumnHasConceptTypeChild.GetConceptTypeChildPath(target).Contains(containedChild) && target != column)
							{
								columns.Add(target);
							}
						}
					}
				}
			}
			foreach (ConceptTypeAssimilatesConceptType conceptTypeAssimilatesConceptType in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType))
			{
				if (conceptTypeAssimilatesConceptType.IsPreferredForTarget)
				{
					foreach (Column target in ColumnHasConceptTypeChild.GetColumn((ConceptTypeChild)conceptTypeAssimilatesConceptType))
					{
						foreach (ConceptTypeChild conceptTypeChild in ColumnHasConceptTypeChild.GetConceptTypeChildPath(target))
						{
							if (ColumnHasConceptTypeChild.GetConceptTypeChildPath(target).Contains(conceptTypeChild) && target != column)
							{
								columns.Add(target);
							}
						}
					}
				}
				else if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(conceptTypeAssimilatesConceptType) == AssimilationAbsorptionChoice.Separate)
				{
					Table targetTable = TableIsPrimarilyForConceptType.GetTable(conceptTypeAssimilatesConceptType.AssimilatedConceptType);

					foreach (Column target in targetTable.ColumnCollection)
					{
						foreach (ConceptTypeChild conceptTypeChild in ColumnHasConceptTypeChild.GetConceptTypeChildPath(target))
						{
							if (ColumnHasConceptTypeChild.GetConceptTypeChildPath(target).Contains(conceptTypeChild) && target != column)
							{
								columns.Add(target);
							}
						}
					}
				}
			}
			foreach (ConceptTypeAssimilatesConceptType conceptTypeAssimilatesConceptType in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(conceptType))
			{
				if (conceptTypeAssimilatesConceptType.IsPreferredForParent)
				{
					foreach (Column target in ColumnHasConceptTypeChild.GetColumn((ConceptTypeChild)conceptTypeAssimilatesConceptType))
					{
						foreach (ConceptTypeChild conceptTypeChild in ColumnHasConceptTypeChild.GetConceptTypeChildPath(target))
						{
							if (ColumnHasConceptTypeChild.GetConceptTypeChildPath(target).Contains(conceptTypeChild) && target != column)
							{
								columns.Add(target);
							}
						}
					}
				}
				else if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(conceptTypeAssimilatesConceptType) == AssimilationAbsorptionChoice.Separate)
				{
					Table targetTable = TableIsPrimarilyForConceptType.GetTable(conceptTypeAssimilatesConceptType.AssimilatedConceptType);

					foreach (Column target in targetTable.ColumnCollection)
					{
						foreach (ConceptTypeChild conceptTypeChild in ColumnHasConceptTypeChild.GetConceptTypeChildPath(target))
						{
							if (ColumnHasConceptTypeChild.GetConceptTypeChildPath(target).Contains(conceptTypeChild) && target != column)
							{
								columns.Add(target);
							}
						}	
					}
				}
			}
			


			// Walk the assimililating path looking for possible relationships that may hold the key.
			if (columns.Count == 0)
			{
				InformationType matchingBaseType = ColumnHasConceptTypeChild.GetConceptTypeChildPath(column)[ColumnHasConceptTypeChild.GetConceptTypeChildPath(column).Count - 1] as InformationType;
				LinkedElementCollection<ConceptType> assimilations;

				while (columns.Count == 0 && (assimilations = conceptType.AssimilatorConceptTypeCollection).Count != 0)
				{
					conceptType = assimilations[0];

					Table targetTable = TableIsPrimarilyForConceptType.GetTable(conceptType);

					if (targetTable != null)
					{
						foreach (Column possibleColumn in targetTable.ColumnCollection)
						{
							if (ColumnHasConceptTypeChild.GetConceptTypeChildPath(possibleColumn)[ColumnHasConceptTypeChild.GetConceptTypeChildPath(possibleColumn).Count - 1] == matchingBaseType)
							{
								columns.Add(possibleColumn);
							}
						}
					}
				}
			}

			if (columns.Count == 0)
			{
				InformationType matchingBaseType = ColumnHasConceptTypeChild.GetConceptTypeChildPath(column)[ColumnHasConceptTypeChild.GetConceptTypeChildPath(column).Count - 1] as InformationType;
				LinkedElementCollection<ConceptType> assimilations;

				while (columns.Count == 0 && (assimilations = conceptType.AssimilatorConceptTypeCollection).Count != 0)
				{
					conceptType = assimilations[0];

					Table targetTable = TableIsPrimarilyForConceptType.GetTable(conceptType);

					if (targetTable == null)
					{
						LinkedElementCollection<ConceptType> relatingConceptTypes = ConceptTypeRelatesToConceptType.GetRelatingConceptTypeCollection(conceptType);
						foreach (ConceptType relatingConcept in relatingConceptTypes)
						{
							targetTable = TableIsPrimarilyForConceptType.GetTable(relatingConcept);
							if (targetTable != null)
							{
								break;
							}
						}
					}

					//LinkedElementCollection<ConceptType> relating =	ConceptTypeRelatesToConceptType.GetRelatingConceptTypeCollection(conceptType);
					//int relatingCount = relating.Count;
					//ConceptType[] real = new ConceptType[] { relatingCount };
					//relating.CopyTo(real, 0);			

					if (targetTable != null)
					{
						foreach (Column possibleColumn in targetTable.ColumnCollection)
						{
							if (ColumnHasConceptTypeChild.GetConceptTypeChildPath(possibleColumn)[ColumnHasConceptTypeChild.GetConceptTypeChildPath(possibleColumn).Count - 1] == matchingBaseType)
							{
								columns.Add(possibleColumn);
							}
						}
					}
				}
			}

			// follow the assimilating path looking for relationships that may hold the key
			if (columns.Count == 0)
			{
				InformationType matchingBaseType = ColumnHasConceptTypeChild.GetConceptTypeChildPath(column)[ColumnHasConceptTypeChild.GetConceptTypeChildPath(column).Count - 1] as InformationType;

				LinkedElementCollection<ConceptType> assimilations;
				while (columns.Count == 0 && (assimilations = conceptType.AssimilatedConceptTypeCollection).Count != 0)
				{
					foreach (ConceptType assimilatedConceptType in assimilations)
					{
						Table targetTable = TableIsPrimarilyForConceptType.GetTable(assimilatedConceptType);

						if (targetTable == null)
						{
							LinkedElementCollection<ConceptType> relatingConceptTypes = ConceptTypeRelatesToConceptType.GetRelatingConceptTypeCollection(assimilatedConceptType);
							foreach (ConceptType relatingConcept in relatingConceptTypes)
							{
								targetTable = TableIsPrimarilyForConceptType.GetTable(relatingConcept);
								if (targetTable != null)
								{
									break;
								}
							}
						}
						if (targetTable != null)
						{
							foreach (Column possibleColumn in targetTable.ColumnCollection)
							{
								if (ColumnHasConceptTypeChild.GetConceptTypeChildPath(possibleColumn)[ColumnHasConceptTypeChild.GetConceptTypeChildPath(possibleColumn).Count - 1] == matchingBaseType)
								{
									columns.Add(possibleColumn);
								}
							}
						}
					}
				}
			}

			return columns;
		}

		/// <summary>
		/// FindTarget finds the target column for an input <see cref="ConceptType" />.
		/// </summary>
		/// <param name="column">The <see cref="Column"/>.</param>
		/// <param name="conceptType">The <see cref="ConceptType"/>.</param>
		/// <returns>The target <see cref="Column"/>.</returns>
		private static Column FindTarget(Column column, ConceptType conceptType)
		{
			bool targetFound = true;

			if (TableIsPrimarilyForConceptType.GetTable(conceptType) == null)
			{
				LinkedElementCollection<Table> tables = TableIsAlsoForConceptType.GetTable(conceptType);
				if (tables.Count != 1)
				{
					return null;
				}
			}

			//LinkedElementCollection<ConceptType> relating =	ConceptTypeRelatesToConceptType.GetRelatingConceptTypeCollection(conceptType);
			//int relatingCount = relating.Count;
			//ConceptType[] real = new ConceptType[] { relatingCount };
			//relating.CopyTo(real, 0);	
			List<Column> possibleColumns = ConceptTypeHasPrimaryIdentifierColumns(column, conceptType);

			foreach (Column targetColumn in possibleColumns)
			{
				targetFound = true;

				if (targetColumn.Equals(column))
				{
					targetFound = false;
				}
				else
				{
					//for (int i = 0; i < ColumnHasConceptTypeChild.GetConceptTypeChildPath(targetColumn).Count; ++i)
					//{
						LinkedElementCollection<ConceptTypeChild> leftPath = ColumnHasConceptTypeChild.GetConceptTypeChildPath(column);
						LinkedElementCollection<ConceptTypeChild> rightPath = ColumnHasConceptTypeChild.GetConceptTypeChildPath(targetColumn);

						int leftBuffer = 0;
						int rightBuffer = 0;
						
						if (rightPath[0].GetType() == typeof(ConceptTypeAssimilatesConceptType))
						{
							if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(rightPath[0] as ConceptTypeAssimilatesConceptType) == AssimilationAbsorptionChoice.Separate)
							{
								rightBuffer += 1;
							}
						}
						if (leftPath[0].GetType() == typeof(ConceptTypeAssimilatesConceptType))
						{
							if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(leftPath[0] as ConceptTypeAssimilatesConceptType) == AssimilationAbsorptionChoice.Separate)
							{
								leftBuffer += 1;
							}
						}

						if (leftPath.Count-leftBuffer >= rightPath.Count-rightBuffer)
						{
							for (int reverseIndex = 0; reverseIndex < rightPath.Count-rightBuffer; reverseIndex++)
							{
								if (rightPath[rightPath.Count - 1 - reverseIndex] != leftPath[leftPath.Count - 1 - reverseIndex])
								{
									targetFound = false;
									break;
								}

							}
						}
						else
						{
							for (int reverseIndex = 0; reverseIndex < leftPath.Count-leftBuffer; reverseIndex++)
							{
								if (leftPath[leftPath.Count - 1 - reverseIndex] != rightPath[rightPath.Count - 1 - reverseIndex])
								{
									targetFound = false;
									break;
								}
							}
						}
					//}
					// END TEST

					//if ((i + 1 < leftPath.Count && i + offset < rightPath.Count &&
					//    leftPath[i + 1] != rightPath[i + offset]))
					//{
					//    for (int j = 0; j < leftPath.Count; ++j)
					//    {
					//        if (leftPath[j].GetType() == typeof(InformationType))
					//        {
					//            offset = j;
					//        }
					//    }

					//    if ((i + 1 < leftPath.Count && i + offset < rightPath.Count &&
					//    rightPath[rightPath.Count - 1] != rightPath[offset]))
					//    {

					//        targetFound = false;
					//        break;
					//    }
					//}
				}
				if (targetFound && targetColumn.Table != null)
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
		private static void CreateUniquenessConstraints(ConceptType conceptType, Dictionary<TableIsAlsoForConceptType, AssimilationPath> assimilationPath)
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
							if (ColumnHasConceptTypeChild.GetConceptTypeChildPath(myColumn).Count > 0 && ColumnHasConceptTypeChild.GetConceptTypeChildPath(myColumn)[0] == conceptTypeChild)
							{
								UniquenessConstraintIncludesColumn uniquenessConstraintIncludesColumn = new UniquenessConstraintIncludesColumn(uniquenessConstraint, myColumn);
								if (uniqueness.IsPreferred)
								{
									uniquenessConstraint.IsPrimary = true;
								}
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
			else
			{
				foreach (Uniqueness uniqueness in conceptType.UniquenessCollection)
				{
					foreach (Table table in TableIsAlsoForConceptType.GetTable(conceptType))
					{
						UniquenessConstraint uniquenessConstraint = new UniquenessConstraint(table.Store, new PropertyAssignment[] { new PropertyAssignment(UniquenessConstraint.NameDomainPropertyId, uniqueness.Name) });
						foreach (Column column in table.ColumnCollection)
						{
							foreach (ConceptTypeChild conceptTypeChild in uniqueness.ConceptTypeChildCollection)
							{
								TableIsAlsoForConceptType tableIsAlsoForConceptType = null;

								foreach (TableIsAlsoForConceptType alsoForConceptType in TableIsAlsoForConceptType.GetLinksToConceptType(table))
								{
									if (alsoForConceptType.ConceptType == conceptType)
										tableIsAlsoForConceptType = alsoForConceptType;
								}

								if (tableIsAlsoForConceptType == null)
								{
									tableIsAlsoForConceptType = new TableIsAlsoForConceptType(table, conceptType);
								}

								List<ConceptTypeChild> startPath = new List<ConceptTypeChild>();

								for (int i = 0; i < assimilationPath[tableIsAlsoForConceptType].Path.Count; i++)
								{
									startPath.Add((ConceptTypeChild)assimilationPath[tableIsAlsoForConceptType].Path[i]);
								}
								startPath.Add(conceptTypeChild);

								bool columnConceptTypeChildPathStartsWithAssimilationPathAndConceptTypeChild = true;

								for (int i = 0; i < startPath.Count; i++)
								{
									LinkedElementCollection<ConceptTypeChild> columnPath = ColumnHasConceptTypeChild.GetConceptTypeChildPath(column);

									foreach (ConceptTypeChild ctc in columnPath)
									{

									}

									if (startPath[i] != ColumnHasConceptTypeChild.GetConceptTypeChildPath(column)[i])
									{
										columnConceptTypeChildPathStartsWithAssimilationPathAndConceptTypeChild = false;
										break;
									}
								}

								if (columnConceptTypeChildPathStartsWithAssimilationPathAndConceptTypeChild)
								{
									// Testing...

									//List<Column> tempColumnList = ConceptTypeHasPrimaryIdentifierColumns(column, conceptType);
									//foreach (Column tempColumn in tempColumnList)
									//{
									//    if (tempColumn == column)
									//    {
									//        uniquenessConstraint.IsPrimary = true;
									//    }
									//}

									if (ColumnHasConceptTypeChild.GetConceptTypeChildPath(column)[0].GetType() == typeof(ConceptTypeAssimilatesConceptType))
									{
										if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation((ConceptTypeAssimilatesConceptType)ColumnHasConceptTypeChild.GetConceptTypeChildPath(column)[0]) == AssimilationAbsorptionChoice.Partition)
										{
											uniquenessConstraint.IsPrimary = (uniqueness.IsPreferred) ? true : false;
										}
									}

									UniquenessConstraintIncludesColumn uniquenessConstraintIncludesColumn = new UniquenessConstraintIncludesColumn(uniquenessConstraint, column);
								}
							}
						}

						if (uniquenessConstraint != null)
						{
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
		/// <returns>A <see cref="ConceptTypeRelatesToConceptType"/>.</returns>
		private static ConceptTypeRelatesToConceptType GetRelatedConceptTypeForColumn(Column column)
		{
			foreach (ConceptTypeChild conceptTypeChild in ColumnHasConceptTypeChild.GetConceptTypeChildPath(column))
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

		/// <summary>
		/// Applies Nullable/nonNullable constraints on all columns for a given table.
		/// </summary>
		/// <param name="table">The <see cref="Table"/> to set initial constraints for</param>
		private static void GenerateMandatoryConstraints(Table table)
		{
			foreach (Column column in table.ColumnCollection)
			{
				CheckColumnConstraint(column);
			}
		}

		/// <summary>
		/// CheckColumnConstraint looks at the the ConceptTypeChildPath for a column, setting the column to nullable when some point in the path is not manditory. 
		/// </summary>
		private static void CheckColumnConstraint(Column column)
		{
			bool allStepsMandatory = true;
			foreach (ConceptTypeChild concept in ColumnHasConceptTypeChild.GetConceptTypeChildPath(column))
			{
				if (!concept.IsMandatory)
				{
					// A ConceptTypeAssimilatesConceptType that is set to partition may(will) not be manditory, but the resulting columns should be, 
					// Do we wwant to perform a check on the ConceptTypeChild for that case here, or should we eariler change that Assimilation to a manditory one?
					if (concept.GetType() == typeof(ConceptTypeAssimilatesConceptType) && AssimilationMapping.GetAbsorptionChoiceFromAssimilation(concept as ConceptTypeAssimilatesConceptType) == AssimilationAbsorptionChoice.Partition)
					{ 
					
					}
					else
					{
						allStepsMandatory = false;
						break;
					}
				}
			}
			column.IsNullable = !allStepsMandatory;
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
				yield return AssimilationMapping.FixupListener;
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