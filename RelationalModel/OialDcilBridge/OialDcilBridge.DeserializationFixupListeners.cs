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
using System.Diagnostics;

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
		#region AssimilationPath class

		// UNDONE#1 - Not correctly mapping the keys of an abosrbed subtype, problem with ConceptTypeRelatesToConceptType??
		// It's working now, but when doing a case with separation ( C(.id) one-many/spanning B(.id) subtype of A(.id) the foreign key on table C-Has-A is going to A.id not B.id =(

		/// <summary>
		/// AssimilationPath is used for the Value Pair in the TableIsAlsoForConceptType to store the path of the assimilations to the Primary Table.
		/// </summary>
		private class AssimilationPath
		{
			private List<ConceptTypeAssimilatesConceptType> myPath;
			private ConceptTypeAssimilatesConceptType mySingleAssimilation;
			private AssimilationPath myAlternateTraversalPath;

			public IEnumerable<ConceptTypeAssimilatesConceptType> Path
			{
				get
				{
					return (IEnumerable<ConceptTypeAssimilatesConceptType>)myPath ?? ((mySingleAssimilation != null) ? new ConceptTypeAssimilatesConceptType[] { mySingleAssimilation } : new ConceptTypeAssimilatesConceptType[0]);
				}
			}

			public void Add(ConceptTypeAssimilatesConceptType assimilation)
			{
				if (myPath == null)
				{
					ConceptTypeAssimilatesConceptType single = mySingleAssimilation;
					if (single == null)
					{
						mySingleAssimilation = assimilation;
					}
					else
					{
						myPath = new List<ConceptTypeAssimilatesConceptType>();
						myPath.Add(single);
						myPath.Add(assimilation);
						mySingleAssimilation = null;
					}
				}
				else
				{
					myPath.Add(assimilation);
				}
			}

			public void AddRange(IEnumerable<ConceptTypeAssimilatesConceptType> assimilations)
			{
				if (myPath != null)
				{
					myPath.AddRange(assimilations);
				}
				else
				{
					myPath = new List<ConceptTypeAssimilatesConceptType>();
					myPath.Add(mySingleAssimilation);
					myPath.AddRange(assimilations);
				}
			}

			public AssimilationPath AlternateTraversalPath
			{
				get
				{
					return myAlternateTraversalPath;
				}
				set
				{
					AssimilationPath existingAlternate = myAlternateTraversalPath;
					if (existingAlternate != value)
					{
						if (existingAlternate != null)
						{
							value.AlternateTraversalPath = existingAlternate;
						}
						myAlternateTraversalPath = value;
					}
				}
			}

			public AssimilationPath(AssimilationPath startingPath)
			{
				if (startingPath != null)
				{
					if (startingPath.myPath != null)
					{
						myPath = new List<ConceptTypeAssimilatesConceptType>(startingPath.myPath);
					}
					else
					{
						mySingleAssimilation = startingPath.mySingleAssimilation;
					}
				}
			}
			public AssimilationPath(IList<ConceptTypeAssimilatesConceptType> startingPath)
			{
				int count;
				if (startingPath != null && 0 != (count = startingPath.Count))
				{
					if (count == 1)
					{
						mySingleAssimilation = startingPath[0];
					}
					else
					{
						myPath = new List<ConceptTypeAssimilatesConceptType>(startingPath);
					}
				}
			}
		}
		#endregion // AssimilationPath class

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

			// These are not in use any more, may want to keep around for making the linked assimilation path list.
			Dictionary<TableIsAlsoForConceptType, AssimilationPath> assimilationPathDictionary = new Dictionary<TableIsAlsoForConceptType, AssimilationPath>();
			Dictionary<Column, List<ConceptTypeChild>> columnHasConceptTypeChildPath = new Dictionary<Column, List<ConceptTypeChild>>();

			Dictionary<ConceptType, ConceptTypeAssimilatesConceptType> assimilatedConceptTypes = new Dictionary<ConceptType, ConceptTypeAssimilatesConceptType>();
			ReadOnlyCollection<ConceptTypeAssimilatesConceptType> allAssimilations = store.ElementDirectory.FindElements<ConceptTypeAssimilatesConceptType>();
			foreach (ConceptTypeAssimilatesConceptType assimilation in allAssimilations)
			{
				ConceptType assimilatedConceptType = null;
				switch (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation))
				{
					case AssimilationAbsorptionChoice.Absorb:
						assimilatedConceptType = assimilation.AssimilatedConceptType;
						break;
					case AssimilationAbsorptionChoice.Partition:
						assimilatedConceptType = assimilation.AssimilatorConceptType;
						break;
				}
				if (assimilatedConceptType != null && assimilatedConceptType.Model == sourceModel)
				{
					assimilatedConceptTypes[assimilatedConceptType] = assimilation;
				}
			}

			// Generate all tables for the schema.
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

			// Create a stack for ConceptTypeAssimilatesConceptType elements to track the assimilation or partition path of a ConceptType
			Stack<ConceptTypeAssimilatesConceptType> assimilationPath = new Stack<ConceptTypeAssimilatesConceptType>();
			foreach (Table table in tables)
			{
				// For each table that a concept type is primarily for find all ConceptTypes that are also for that table.
				ProcessConceptTypeForTable(TableIsPrimarilyForConceptType.GetConceptType(table), table, assimilationPath);
			}
#if OLDPARTITIONCODE
			#region Old code for detemining the Assimilation and Partition paths from the bottom up.
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
					if (!pathDictonary.ContainsKey(keyedAssimilation.Key) || pathDictonary[keyedAssimilation.Key] == null)
						switch (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(keyedAssimilation.Value))
						{
							case AssimilationAbsorptionChoice.Absorb:
								MapAbsorbedConceptType(keyedAssimilation.Key, pathDictonary, assimilationPathDictionary, null);
								break;
							case AssimilationAbsorptionChoice.Partition:
								MapPartitionedConceptType(keyedAssimilation.Key, mappingPaths, pathDictonary, assimilationPathDictionary, null);
								break;
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
			#endregion Old code for detemining the Assimilation and Partition paths from the bottom up.
#endif // OLDPARTITIONCODE


			Collection<ConceptTypeAssimilatesConceptType> separatedConceptTypes = new Collection<ConceptTypeAssimilatesConceptType>();

			// For every concept type create all columns that they represent, perform separations, and map unqiuenesses that they participate in.
			foreach (ConceptType conceptType in conceptTypes)
			{
				CreateColumns(conceptType);

				bool isPreferredForChildFound = false;
				foreach (ConceptTypeAssimilatesConceptType conceptTypeAssimilation in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType))//GetAssimilationsForConceptType(conceptType))
				{
					if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(conceptTypeAssimilation) == AssimilationAbsorptionChoice.Separate && !separatedConceptTypes.Contains(conceptTypeAssimilation))
					{
						DoSeparation(conceptTypeAssimilation, ref isPreferredForChildFound);
						separatedConceptTypes.Add(conceptTypeAssimilation);
					}
				}

				CreateUniquenessConstraints(conceptType);
			}

			// For each table in the schema generate any foreign keys it contains and detemine witch of it's columns are manditory and nullible.
			foreach (Table table in schema.TableCollection)
			{
				CreateForeignKeys(table);
				GenerateMandatoryConstraints(table);
			}

			// Change all names to a more apropriate verson.
			NameGeneration.GenerateAllNames(schema);
		}


		/// <summary>
		/// Maps the TableIsPrimarilyForConceptType for a given ConceptType and a given Table.
		/// </summary>
		/// <param name="conceptType">The ConceptType that plays a role in the <see cref="TableIsPrimarilyForConceptType"/> table.</param>
		/// <param name="table">The table that the concept type is also for</param>
		/// <param name="assimilationPath">The path of <see cref="ConceptTypeAssimilatesConceptType"/> elements that the concept type.</param>
		private static void ProcessConceptTypeForTable(ConceptType conceptType, Table table, Stack<ConceptTypeAssimilatesConceptType> assimilationPath)
		{
			// If the concept type is not primarily for a table, and the concept type has not yet been mapped to the final table, then map it, and record it's AssimilationPath.
			if (TableIsPrimarilyForConceptType.GetTable(conceptType) == null && TableIsAlsoForConceptType.GetLink(table, conceptType) == null)
			{
				TableIsAlsoForConceptType tableIsAlsoForConceptType = new TableIsAlsoForConceptType(table, conceptType);
				tableIsAlsoForConceptType.AssimilationPath.AddRange(assimilationPath);
			}
			// For all Assimilations that the ConceptType is the Parent of, if the assimilation is set to absorb, 
			// then recursively add the ConceptTypes it assimilates to the table as a TableIsAlsoForConceptType as well.
			foreach (ConceptTypeAssimilatesConceptType assimilation in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(conceptType))
			{
				if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation) == AssimilationAbsorptionChoice.Absorb)
				{
					assimilationPath.Push(assimilation);

					ProcessConceptTypeForTable(assimilation.AssimilatedConceptType, table, assimilationPath);

					ConceptTypeAssimilatesConceptType poppedAssimilation = assimilationPath.Pop();
					Debug.Assert(assimilation == poppedAssimilation);
				}
			}

			// For all Assimilations that the ConceptType is the Target of, if the assimilation is set to partition, 
			// then recursively add the ConceptTypes that assimilate it to the table as a TableIsAlsoForConceptType as well.
			foreach (ConceptTypeAssimilatesConceptType assimilation in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType))
			{
				if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation) == AssimilationAbsorptionChoice.Partition)
				{
					assimilationPath.Push(assimilation);

					ProcessConceptTypeForTable(assimilation.AssimilatorConceptType, table, assimilationPath);

					ConceptTypeAssimilatesConceptType poppedAssimilation = assimilationPath.Pop();
					Debug.Assert(assimilation == poppedAssimilation);
				}
			}
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

#if OLDPARTITIONCODE
		/// <summary> Deprecated Method for Mapping Absorbed Assimilations.
		/// Maps all Absorbed ConceptTypes
		/// for a ConceptType it finds the Primary Tables that they map to, 
		/// and then map a TableIsAlsoForConceptType to the Primary Table with that ConceptType.
		/// </summary>
		/// <param name="conceptType">The ConceptType that is to be checked for Absorbtion.</param>
		/// <param name="assimilatedConceptTypes">The Dictionary assimilatedConceptTypes contains all Absorbed paths, mapping ConceptTypes 
		/// to the ConceptType that they absorb into.</param>
		/// <param name="assimilationPathDictionary"></param>
		/// <param name="assimilationPath">Assimilation Path records the current path of the ConceptType being mapped, 
		/// when a ConceptType's path is fully mapped, this list then gets added to the Dictonary assimilationPathDictionary</param>
		private static void MapAbsorbedConceptType(ConceptType conceptType, Dictionary<ConceptType, ConceptType> assimilatedConceptTypes, Dictionary<TableIsAlsoForConceptType, AssimilationPath> assimilationPathDictionary, AssimilationPath assimilationPath)
		{
			foreach (ConceptTypeAssimilatesConceptType assimilation in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType))
			{
				if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation) == AssimilationAbsorptionChoice.Absorb)
				{
					(assimilationPath ?? (assimilationPath = new AssimilationPath((AssimilationPath)null))).Add(assimilation);

					ConceptType assimilatorConceptType = assimilation.AssimilatorConceptType;
					ConceptType oppositeConceptType;
					if (!assimilatedConceptTypes.ContainsKey(conceptType))
					{
						assimilatedConceptTypes[conceptType] = null;

						Table isPrimarilyForTable = TableIsPrimarilyForConceptType.GetTable(assimilatorConceptType);
						if (isPrimarilyForTable != null)
						{

							// TableIsAlsoForConceptType tableForConceptType = new TableIsAlsoForConceptType(isPrimarilyForTable, conceptType);
							assimilatedConceptTypes[conceptType] = assimilation.AssimilatorConceptType;
							AddToAssimilationPathDictionary(assimilationPathDictionary, isPrimarilyForTable, conceptType, new AssimilationPath(assimilationPath));
							// assimilationPathDictionary[tableForConceptType] =   // Add assimilatedConceptTypes to TableIsAlsoForConceptType created. --UNDONE--?
						}
						else if (assimilatedConceptTypes.TryGetValue(assimilatorConceptType, out oppositeConceptType) && oppositeConceptType != null && assimilation.IsPreferredForTarget)
						{
							assimilatedConceptTypes[conceptType] = oppositeConceptType;
							AssimilationPath previousMappingPath;

							TableIsAlsoForConceptType previousMapping = TableIsAlsoForConceptType.GetLink(TableIsPrimarilyForConceptType.GetTable(oppositeConceptType), assimilatorConceptType);
							if (assimilationPathDictionary.TryGetValue(previousMapping, out previousMappingPath))
							{
								assimilationPath.AddRange(previousMappingPath.Path);
							}

							AddToAssimilationPathDictionary(assimilationPathDictionary, TableIsPrimarilyForConceptType.GetTable(oppositeConceptType), conceptType, new AssimilationPath(assimilationPath));
							// TableIsAlsoForConceptType tableForConceptType = new TableIsAlsoForConceptType(TableIsPrimarilyForConceptType.GetTable(oppositeConceptType), conceptType);
							// assimilationPathDictionary[tableForConceptType] = new AssimilationPath(assimilationPath);
						}
					}
					else if (assimilatedConceptTypes.TryGetValue(assimilatorConceptType, out oppositeConceptType) && oppositeConceptType != null && assimilation.IsPreferredForTarget)
					{
						ConceptType targetMappingPath = assimilatedConceptTypes[assimilatorConceptType];
						if (targetMappingPath != null)
						{
							assimilatedConceptTypes[conceptType] = targetMappingPath;

							AddToAssimilationPathDictionary(assimilationPathDictionary, TableIsPrimarilyForConceptType.GetTable(targetMappingPath), conceptType, new AssimilationPath(assimilationPath));
							// TableIsAlsoForConceptType tableForConceptType = new TableIsAlsoForConceptType(TableIsPrimarilyForConceptType.GetTable(targetMappingPath), conceptType);
							// assimilationPathDictionary[tableForConceptType] = new AssimilationPath(assimilationPath);
						}
					}
					else
					{
						MapAbsorbedConceptType(assimilation.AssimilatorConceptType, assimilatedConceptTypes, assimilationPathDictionary, assimilationPath);
					}
				}
			}
		}
#endif // OLDPARTITIONCODE

		/// <summary>
		/// Adds a TableIsAlsoForConceptType to the dictionary allong with an AssimilationPath, or, if the TableIsAlsoForConceptType already exits, appends the AssimilationPath to the TableIsAlsoForConceptType's AssimilationPath as an AlternateTraversalPath.
		/// </summary>
		private static void AddToAssimilationPathDictionary(Dictionary<TableIsAlsoForConceptType, AssimilationPath> dictionary, Table isPrimarilyForTable, ConceptType conceptType, AssimilationPath path) //TableIsAlsoForConceptType key
		{
			AssimilationPath existingPath;

			TableIsAlsoForConceptType key = TableIsAlsoForConceptType.GetLink(isPrimarilyForTable, conceptType);
			if (key != null && dictionary.TryGetValue(key, out existingPath))
			{
				if (existingPath != path)
				{
					existingPath.AlternateTraversalPath = path;
				}
			}
			else
			{
				key = new TableIsAlsoForConceptType(isPrimarilyForTable, conceptType);
				dictionary.Add(key, path);
			}
		}

#if OLDPARTITIONCODE
		/// <summary> Depriciated Method for mapping assimilations that are set to Partition.
		/// MapPartitionedConceptType finds any partitioned assimilation that a ConceptType plays in and maps a TableIsAlsoForConceptType to the Primary Table.
		/// </summary>
		/// <param name="assimilationPathDictionary"></param>
		/// <param name="assimilatorConceptTypes"></param>
		/// <param name="conceptType"></param>
		/// <param name="mappingPaths">mappingPaths contains a collection of the mappings of all partitions.  Each partitioned ConceptType corrilates to a List of ConceptTypes that is is partitioned into.</param>
		/// <param name="assimilationPath">Assimilation Path records the current path of the ConceptType being mapped, 
		/// when a ConceptType's path is fully mapped, this list then gets added to the Dictonary assimilationPathDictionary</param>
		private static void MapPartitionedConceptType(ConceptType conceptType, Dictionary<ConceptType, List<ConceptType>> mappingPaths, Dictionary<ConceptType, ConceptType> assimilatorConceptTypes, Dictionary<TableIsAlsoForConceptType, AssimilationPath> assimilationPathDictionary, AssimilationPath assimilationPath)
		{
			foreach (ConceptTypeAssimilatesConceptType assimilation in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(conceptType))
			{
				if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation) == AssimilationAbsorptionChoice.Partition)
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

					(assimilationPath ?? (assimilationPath = new AssimilationPath((AssimilationPath)null))).Add(assimilation);
					Table isPrimarilyForTable = TableIsPrimarilyForConceptType.GetTable(assimilation.AssimilatedConceptType);
					if (isPrimarilyForTable != null && !TableIsAlsoForConceptType.GetConceptType(isPrimarilyForTable).Contains(conceptType))
					{
						TableIsAlsoForConceptType tableIsAlsoForConceptType = new TableIsAlsoForConceptType(isPrimarilyForTable, conceptType);
						assimilationPathDictionary[tableIsAlsoForConceptType] = new AssimilationPath(assimilationPath);
					}
					else
					{
						MapAbsorbedConceptType(assimilation.AssimilatedConceptType, assimilatorConceptTypes, assimilationPathDictionary, new AssimilationPath(assimilationPath));
					}
				}
			}
		}
#endif // OLDPARTITIONCODE

		/// <summary>
		/// Creates columns for a ConceptType for every Table that the ConceptType plays a role in, as a TableIsPrimarilyForConceptType or TableIsAlsoForConceptType relationship.
		/// </summary>

		private static void CreateColumns(ConceptType conceptType)
		{
			List<ConceptTypeChild> preferredConceptTypeChildList = GetPreferredConceptTypeChildrenForConceptType(conceptType);
			List<Column> columnsForConceptType = new List<Column>();


			foreach (ConceptTypeChild conceptTypeChild in ConceptTypeChild.GetLinksToTargetCollection(conceptType))
			{
				if (!(conceptTypeChild is ConceptTypeAssimilatesConceptType))
				{
					columnsForConceptType.AddRange(GetColumnsForConceptTypeChild(conceptTypeChild, new List<ConceptTypeChild>()));
				}
			}
			foreach (ConceptTypeChild assimilatedConceptType in preferredConceptTypeChildList)
			{
				if (assimilatedConceptType is ConceptTypeAssimilatesConceptType)
				{
					columnsForConceptType.AddRange(GetColumnsForConceptTypeChild(assimilatedConceptType, new List<ConceptTypeChild>()));
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
				foreach (TableIsAlsoForConceptType tableIsAlsoForConceptType in tableIsAlsoForConceptTypeInvolvedWithConceptType)
				{
					foreach (Column column in columnsForConceptType)
					{
						Column clonedColumn = new Column(column.Store, new PropertyAssignment[] { new PropertyAssignment(Column.NameDomainPropertyId, column.Name) });

						LinkedElementCollection<ConceptTypeChild> clonedConceptTypeChildPath = ColumnHasConceptTypeChild.GetConceptTypeChildPath(clonedColumn);
						foreach (ConceptTypeAssimilatesConceptType assimilation in tableIsAlsoForConceptType.AssimilationPath)
						{
							clonedConceptTypeChildPath.Add(assimilation);
						}
						foreach (ConceptTypeChild child in ColumnHasConceptTypeChild.GetConceptTypeChildPath(column))
						{
							if (!clonedConceptTypeChildPath.Contains(child))
							{
								clonedConceptTypeChildPath.Add(child);
							}
						}
						//clonedConceptTypeChildPath.AddRange(ColumnHasConceptTypeChild.GetConceptTypeChildPath(column));

						Column possibleDuplicateColumn = CheckForDuplicateColumn(tableIsAlsoForConceptType.Table, clonedColumn);
						if (possibleDuplicateColumn == null)
						{
							// if CTC is in PreferredList
							// Record CTCHasPrimaryIdentifierColumn(CT, ClonedColumn);

							tableIsAlsoForConceptType.Table.ColumnCollection.Add(clonedColumn);
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
				ConceptType targetConceptType = (ConceptType)conceptTypeChild.Parent;
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
						prefferedConceptTypeChildrenList.AddRange(GetPreferredConceptTypeChildrenForConceptType(conceptTypeAssimilatesConceptType.AssimilatorConceptType));
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
		/// DoSeparation handles the generation of columns and foreign keys when an assimilation is set to separate.
		/// </summary>
		private static void DoSeparation(ConceptTypeAssimilatesConceptType assimilation, ref bool isPreferredForChildFound)
		{
			Table table = TableIsPrimarilyForConceptType.GetTable(assimilation.AssimilatedConceptType);
			if (table != null)
			{
				if (isPreferredForChildFound == false)
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
							ConceptType parentConcept = assimilation.AssimilatorConceptType;
							ConceptTypeAssimilatesConceptType assimilationPath = assimilation;
							Table targetTable = null;

							while (targetTable == null)
							{
								targetTable = TableIsPrimarilyForConceptType.GetTable(parentConcept);
								if (targetTable == null)
								{
									parentConcept = parentConcept.AssimilatorConceptTypeCollection[0];
								}
								else
								{
									break;
								}
							}

							if (targetTable != null)
							{
								ReferenceConstraint referenceConstraint = new ReferenceConstraint(assimilation.Store, new PropertyAssignment[] { new PropertyAssignment(ReferenceConstraint.NameDomainPropertyId, assimilation.Name) });
								TableContainsReferenceConstraint tableContainsReferenceConstraint = new TableContainsReferenceConstraint(table, referenceConstraint);
								ReferenceConstraintTargetsTable referenceConstraintTargetsTable = new ReferenceConstraintTargetsTable(referenceConstraint, targetTable);

								UniquenessConstraint mappedConstraint = new UniquenessConstraint(assimilation.Store, new PropertyAssignment[] { new PropertyAssignment(UniquenessConstraint.NameDomainPropertyId, "Constraint") });

								// If it is prefered for target set the constraint as primary, otherwise perform the check in the defualt case.
								mappedConstraint.IsPrimary = true;

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
									primaryIdentifierFound = true;
								}

								foreach (Column identifierColumn in primaryIdentifierColumns)
								{
									LinkedElementCollection<ConceptTypeChild> ctcpath = ColumnHasConceptTypeChild.GetConceptTypeChildPath(identifierColumn);
									if (primaryIdentifierFound == true)
									{
										foreach (ConceptTypeChild ctc in ctcpath)
										{
											ctc.IsMandatory = true;
										}
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
							isPreferredForChildFound = true;
						}
					}
					//else if (assimilation.IsMandatory)
					//{

					//    Table targetTable = null;
					//    ConceptType parentConcept = assimilation.AssimilatorConceptType;
					//    ConceptTypeAssimilatesConceptType assimilationPath = assimilation;

					//    LinkedElementCollection<ConceptType> assimilations = null;
					//    ConceptType conceptType = assimilation.AssimilatedConceptType;
					//    List<Column> primaryIdentifierColumns = new List<Column>();

					//    bool primaryIdentifierFound = false;

					//    while (targetTable == null && !primaryIdentifierFound)
					//    {
					//        targetTable = TableIsPrimarilyForConceptType.GetTable(parentConcept);
					//        if (targetTable == null)
					//        {
					//            parentConcept = parentConcept.AssimilatorConceptTypeCollection[0];
					//        }
					//        else
					//        {
					//            foreach (UniquenessConstraint uniqueness in targetTable.UniquenessConstraintCollection)
					//            {
					//                if (uniqueness.IsPrimary)
					//                {
					//                    primaryIdentifierColumns.AddRange(uniqueness.ColumnCollection);
					//                    assimilation.IsMandatory = true;
					//                    primaryIdentifierFound = true;
					//                }
					//            }
					//        }
					//    } 

					//    ReferenceConstraint referenceConstraint = new ReferenceConstraint(assimilation.Store, new PropertyAssignment[] { new PropertyAssignment(ReferenceConstraint.NameDomainPropertyId, assimilation.Name) });
					//    TableContainsReferenceConstraint tableContainsReferenceConstraint = new TableContainsReferenceConstraint(TableIsPrimarilyForConceptType.GetTable(assimilation.AssimilatedConceptType), referenceConstraint);
					//    ReferenceConstraintTargetsTable referenceConstraintTargetsTable = new ReferenceConstraintTargetsTable(referenceConstraint, targetTable);


					//    if (primaryIdentifierColumns.Count == 0)
					//    {
					//        primaryIdentifierColumns.AddRange(GetColumnsForConceptTypeChild(assimilation as ConceptTypeChild, new List<ConceptTypeChild>()));
					//    }

					//    foreach (Column identifierColumn in primaryIdentifierColumns)
					//    {
					//        Column clonedColumn = new Column(identifierColumn.Store, new PropertyAssignment[] { new PropertyAssignment(Column.NameDomainPropertyId, identifierColumn.Name) });
					//        List<ConceptTypeChild> clonedConceptTypeChildPath = new List<ConceptTypeChild>(ColumnHasConceptTypeChild.GetConceptTypeChildPath(identifierColumn));

					//        foreach (ConceptTypeChild conceptTypeChild in clonedConceptTypeChildPath)
					//        {
					//            conceptTypeChild.IsMandatory = true;
					//        }
					//        ColumnHasConceptTypeChild.GetConceptTypeChildPath(clonedColumn).Clear();
					//        ColumnHasConceptTypeChild.GetConceptTypeChildPath(clonedColumn).AddRange(clonedConceptTypeChildPath);

					//        TableIsPrimarilyForConceptType.GetTable(assimilation.AssimilatedConceptType).ColumnCollection.Add(clonedColumn);
					//        ColumnReference relationship = new ColumnReference(clonedColumn, identifierColumn);
					//        referenceConstraint.ColumnReferenceCollection.Add(relationship);
					//    }
					//}
					else
					{
						ConceptType parentConcept = assimilation.AssimilatorConceptType;
						ConceptTypeAssimilatesConceptType assimilationPath = assimilation;
						Table targetTable = null;

						while (targetTable == null)
						{
							targetTable = TableIsPrimarilyForConceptType.GetTable(parentConcept);
							if (targetTable == null)
							{
								parentConcept = parentConcept.AssimilatorConceptTypeCollection[0];
							}
							else
							{
								break;
							}
						}

						if (targetTable != null)
						{
							ReferenceConstraint referenceConstraint = new ReferenceConstraint(assimilation.Store, new PropertyAssignment[] { new PropertyAssignment(ReferenceConstraint.NameDomainPropertyId, assimilation.Name) });
							TableContainsReferenceConstraint tableContainsReferenceConstraint = new TableContainsReferenceConstraint(table, referenceConstraint);
							ReferenceConstraintTargetsTable referenceConstraintTargetsTable = new ReferenceConstraintTargetsTable(referenceConstraint, targetTable);

							UniquenessConstraint mappedConstraint = new UniquenessConstraint(assimilation.Store, new PropertyAssignment[] { new PropertyAssignment(UniquenessConstraint.NameDomainPropertyId, "Constraint") });

							if (GetColumnsForConceptTypeChild(GetPreferredConceptTypeChildrenForConceptType(assimilation.AssimilatedConceptType)[0], new List<ConceptTypeChild>()) == null)
							{
								mappedConstraint.IsPrimary = true;
							}

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
								primaryIdentifierFound = true;
							}

							foreach (Column identifierColumn in primaryIdentifierColumns)
							{
								LinkedElementCollection<ConceptTypeChild> ctcpath = ColumnHasConceptTypeChild.GetConceptTypeChildPath(identifierColumn);
								if (primaryIdentifierFound == true)
								{
									foreach (ConceptTypeChild ctc in ctcpath)
									{
										ctc.IsMandatory = true;
									}
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

							isPreferredForChildFound = true;
						}
					}
				}
			}
			else
			{
				// If the seperation is further partitioned then it will not have a secondary child table, further assimilations or seperations are ok though.
				ReadOnlyCollection<ConceptTypeAssimilatesConceptType> childAssimilations = ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(assimilation.AssimilatedConceptType);
				bool isPartitioned = false;
				foreach (ConceptTypeAssimilatesConceptType possiblePartition in childAssimilations)
				{
					if (AssimilationMapping.GetAssimilationMappingFromAssimilation(possiblePartition).AbsorptionChoice == AssimilationAbsorptionChoice.Partition)
					{
						isPartitioned = true;
						break;
					}
				}
				if (isPartitioned)
				{
					bool prefered = false;
					foreach (ConceptTypeAssimilatesConceptType partition in childAssimilations)
					{
						DoSeparation(partition, ref prefered);
						prefered = false;
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
					// UNDONE: Problem occuring with A is partitioned to B, C; C is partitioned to D, E. Setting A-C to Partition
					ReadOnlyCollection<ConceptTypeAssimilatesConceptType> childCollection = ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(conceptTypeAssimilatesConceptType.AssimilatedConceptType);

					/**/
					bool containsPartitions = false;
					foreach (ConceptTypeAssimilatesConceptType act in childCollection)
					{
						if (AssimilationMapping.GetAssimilationMappingFromAssimilation(act).AbsorptionChoice == AssimilationAbsorptionChoice.Partition)
						{

							containsPartitions = true;
							break;
						}
					}
					/**/
					/**/
					if (!containsPartitions)
					{
						/**/
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
						/**/
					}
					else
					{
						foreach (ConceptTypeAssimilatesConceptType act in childCollection)
						{
							columns.AddRange(ConceptTypeHasPrimaryIdentifierColumns(column, act.AssimilatedConceptType));
						}
					}
					/**/
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

					if (leftPath.Count - leftBuffer >= rightPath.Count - rightBuffer)
					{
						for (int reverseIndex = 0; reverseIndex < rightPath.Count - rightBuffer; reverseIndex++)
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
						for (int reverseIndex = 0; reverseIndex < leftPath.Count - leftBuffer; reverseIndex++)
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
		private static void CreateUniquenessConstraints(ConceptType conceptType)
		{
			// UNDONE: Look here for possible problems with ring contstraints
			Table isPrimarilyForTable = TableIsPrimarilyForConceptType.GetTable(conceptType);
			if (isPrimarilyForTable != null)
			{
				List<Uniqueness> alreadyDone = new List<Uniqueness>();
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
					foreach (TableIsAlsoForConceptType tableIsAlsoForConceptType in TableIsAlsoForConceptType.GetLinksToTable(conceptType))
					{
						Table table = tableIsAlsoForConceptType.Table;
						LinkedElementCollection<Column> columns = table.ColumnCollection;
						LinkedElementCollection<ConceptTypeAssimilatesConceptType> assimilationPath = tableIsAlsoForConceptType.AssimilationPath;
						int assimilationPathCount = assimilationPath.Count;

						LinkedElementCollection<ConceptTypeChild> uniquenessConceptTypeChildren = uniqueness.ConceptTypeChildCollection;

						UniquenessConstraint uniquenessConstraint = new UniquenessConstraint(table.Store, new PropertyAssignment[] { new PropertyAssignment(UniquenessConstraint.NameDomainPropertyId, uniqueness.Name) });
						uniquenessConstraint.Table = table;
						new UniquenessConstraintIsForUniqueness(uniquenessConstraint, uniqueness);

						bool isPreferred = uniqueness.IsPreferred;
						if (isPreferred)
						{
							foreach (ConceptTypeAssimilatesConceptType assimilation in assimilationPath)
							{
								AssimilationAbsorptionChoice absorptionChoice = AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation);
								if (absorptionChoice == AssimilationAbsorptionChoice.Absorb)
								{
									if (!assimilation.IsPreferredForParent)
									{
										isPreferred = false;
										break;
									}
								}
								else
								{
									Debug.Assert(absorptionChoice == AssimilationAbsorptionChoice.Partition);
									if (!assimilation.IsPreferredForTarget)
									{
										isPreferred = false;
										break;
									}
								}
							}
							if (isPreferred)
							{
								uniquenessConstraint.IsPrimary = true;
							}
						}

						List<Column> matchingColumns = new List<Column>(uniquenessConceptTypeChildren.Count);

						foreach (Column column in columns)
						{
							LinkedElementCollection<ConceptTypeChild> ctcPath = ColumnHasConceptTypeChild.GetConceptTypeChildPath(column);
							bool pathMatches = ctcPath.Count == assimilationPathCount + 1;
							if (pathMatches)
							{
								for (int i = 0; i < assimilationPathCount; i++)
								{
									if (assimilationPath[i] != ctcPath[i])
									{
										pathMatches = false;
										break;
									}
								}
								if (pathMatches)
								{
									matchingColumns.Add(column);
								}
							}
						}

						Debug.Assert(matchingColumns.Count == uniquenessConceptTypeChildren.Count);

						foreach (ConceptTypeChild conceptTypeChild in uniqueness.ConceptTypeChildCollection)
						{
							for (int i = matchingColumns.Count - 1; i >= 0; i--)
							{
								Column column = matchingColumns[i];
								if (ColumnHasConceptTypeChild.GetConceptTypeChildPath(column)[assimilationPathCount] == conceptTypeChild)
								{
									uniquenessConstraint.ColumnCollection.Add(column);
									matchingColumns.RemoveAt(i);
									break;
								}
							}
						}
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
