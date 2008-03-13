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
		/// Columns, Uniquenesses, Foreign Keys, and Mandatory restrictions.
		/// </summary>
		private static void FullyGenerateConceptualDatabaseModel(Schema schema, AbstractionModel sourceModel, INotifyElementAdded notifyAdded)
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
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(conceptTypeTable, true);
					}
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
								if (notifyAdded != null)
								{
									notifyAdded.ElementAdded(tableIsAlsoForConceptType);
								}
							}
						}
					}
				}
			}
			#endregion Old code for detemining the Assimilation and Partition paths from the bottom up.
#endif // OLDPARTITIONCODE


			// For every concept type create all columns that they represent, map uniquenesses that they participate in.
			foreach (ConceptType conceptType in conceptTypes)
			{
				CreateColumns(conceptType, notifyAdded);
				CreateUniquenessConstraints(conceptType, notifyAdded);
			}

			// Make a second pass over the concept types to population separation columns, constraint, and uniquenesses.
			// Note that we do this second so that any target columns already exist
			Dictionary<ConceptTypeAssimilatesConceptType, object> separatedConceptTypes = null;
			foreach (ConceptType conceptType in conceptTypes)
			{
				bool isPreferredForChildFound = false;
				foreach (ConceptTypeAssimilatesConceptType conceptTypeAssimilation in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType))//GetAssimilationsForConceptType(conceptType))
				{
					if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(conceptTypeAssimilation) == AssimilationAbsorptionChoice.Separate &&
						(separatedConceptTypes == null || !separatedConceptTypes.ContainsKey(conceptTypeAssimilation)))
					{
						DoSeparation(conceptTypeAssimilation, ref isPreferredForChildFound, notifyAdded);
						if (separatedConceptTypes == null)
						{
							separatedConceptTypes = new Dictionary<ConceptTypeAssimilatesConceptType, object>();
						}
						separatedConceptTypes.Add(conceptTypeAssimilation, null);
					}
				}
			}

			// For each table in the schema generate any foreign keys it contains and detemine witch of it's columns are mandatory and nullable.
			foreach (Table table in schema.TableCollection)
			{
				CreateForeignKeys(table, notifyAdded);
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
				ConceptTypeAssimilatesConceptType[] assimilationPathReversed = assimilationPath.ToArray();
				Array.Reverse(assimilationPathReversed);
				tableIsAlsoForConceptType.AssimilationPath.AddRange(assimilationPathReversed);
			}
			// For all Assimilations that the ConceptType is the Parent of, if the assimilation is set to absorb, 
			// then recursively add the ConceptTypes it assimilates to the table as a TableIsAlsoForConceptType as well.
			foreach (ConceptTypeAssimilatesConceptType assimilation in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(conceptType))
			{
				if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation) == AssimilationAbsorptionChoice.Absorb)
				{
					assimilationPath.Push(assimilation);

					ProcessConceptTypeForTable(assimilation.AssimilatedConceptType, table, assimilationPath);

#if DEBUG
					ConceptTypeAssimilatesConceptType poppedAssimilation =
#endif // DEBUG
					assimilationPath.Pop();
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

#if DEBUG
					ConceptTypeAssimilatesConceptType poppedAssimilation =
#endif // DEBUG
					assimilationPath.Pop();
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
		private static void CreateColumns(ConceptType conceptType, INotifyElementAdded notifyAdded)
		{
			List<Column> columnsForConceptType = new List<Column>();

			foreach (InformationType informationType in InformationType.GetLinksToInformationTypeFormatCollection(conceptType))
			{
				columnsForConceptType.Add(CreateColumnForInformationType(informationType, new Stack<ConceptTypeChild>()));
			}
			foreach (ConceptTypeRelatesToConceptType conceptTypeRelation in ConceptTypeRelatesToConceptType.GetLinksToRelatedConceptTypeCollection(conceptType))
			{
				columnsForConceptType.AddRange(GetColumnsForConceptTypeRelation(conceptTypeRelation, new Stack<ConceptTypeChild>()));
			}
			foreach (ConceptTypeAssimilatesConceptType assimilation in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(conceptType))
			{
				switch (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation))
				{
					case AssimilationAbsorptionChoice.Absorb:
						// Ignore this, it will be handled by this method when it is run for the absorbed concept type.
						break;
					case AssimilationAbsorptionChoice.Partition:
						// Ignore this too.
						break;
					case AssimilationAbsorptionChoice.Separate:
						// UNDONE: Handle separate here? If so, pull in the preferred identifier columns for the assimilated concept type if IsPreferredForParent is true.
						break;
				}
			}
			foreach (ConceptTypeAssimilatesConceptType assimilation in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType))
			{
				switch (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation))
				{
					case AssimilationAbsorptionChoice.Absorb:
						// Ignore this too.
						break;
					case AssimilationAbsorptionChoice.Partition:
						// Ignore this too.
						break;
					case AssimilationAbsorptionChoice.Separate:
						// UNDONE: Handle separate here? If so, pull in the preferred identifier columns for the assimilator concept type if IsPreferredForTarget is true.
						break;
				}
			}


			Table conceptTypeTable = TableIsPrimarilyForConceptType.GetTable(conceptType);
			if (conceptTypeTable != null)
			{
				conceptTypeTable.ColumnCollection.AddRange(columnsForConceptType);
				if (notifyAdded != null)
				{
					foreach (Column column in columnsForConceptType)
					{
						notifyAdded.ElementAdded(column, true);
					}
				}
			}
			else
			{
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
						clonedConceptTypeChildPath.AddRange(ColumnHasConceptTypeChild.GetConceptTypeChildPath(column));

						Column possibleDuplicateColumn = CheckForDuplicateColumn(tableIsAlsoForConceptType.Table, clonedColumn);
						if (possibleDuplicateColumn == null)
						{
							tableIsAlsoForConceptType.Table.ColumnCollection.Add(clonedColumn);
							if (notifyAdded != null)
							{
								notifyAdded.ElementAdded(clonedColumn);
							}
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

		private static Column CreateColumnForInformationType(InformationType informationType, Stack<ConceptTypeChild> conceptTypeChildPath)
		{
			conceptTypeChildPath.Push(informationType);
			Column column = new Column(informationType.Store,
								new PropertyAssignment[]{
									new PropertyAssignment(Column.NameDomainPropertyId, informationType.Name)});
			ConceptTypeChild[] conceptTypeChildPathReverse = conceptTypeChildPath.ToArray();
			Array.Reverse(conceptTypeChildPathReverse);
			ColumnHasConceptTypeChild.GetConceptTypeChildPath(column).AddRange(conceptTypeChildPathReverse);
			conceptTypeChildPath.Pop();
			return column;
		}
		private static List<Column> GetColumnsForConceptTypeRelation(ConceptTypeRelatesToConceptType conceptTypeRelation, Stack<ConceptTypeChild> conceptTypeChildPath)
		{
			conceptTypeChildPath.Push(conceptTypeRelation);
			List<Column> columns = GetPreferredIdentifierColumnsForConceptType(conceptTypeRelation.RelatedConceptType, conceptTypeChildPath);
			conceptTypeChildPath.Pop();
			return columns;
		}

		private static List<Column> GetPreferredIdentifierColumnsForConceptType(ConceptType conceptType, Stack<ConceptTypeChild> conceptTypeChildPath)
		{
			foreach (Uniqueness uniqueness in conceptType.UniquenessCollection)
			{
				if (uniqueness.IsPreferred)
				{
					LinkedElementCollection<ConceptTypeChild> uniquenessConceptTypeChildren = uniqueness.ConceptTypeChildCollection;
					List<Column> columns = new List<Column>(uniquenessConceptTypeChildren.Count);

					foreach (ConceptTypeChild conceptTypeChild in uniquenessConceptTypeChildren)
					{
						InformationType informationType = conceptTypeChild as InformationType;
						if (informationType != null)
						{
							columns.Add(CreateColumnForInformationType(informationType, conceptTypeChildPath));
						}
						else
						{
							Debug.Assert(conceptTypeChild is ConceptTypeRelatesToConceptType, "Uniquenesses can't contain ConceptTypeAssimilations.");
							columns.AddRange(GetColumnsForConceptTypeRelation((ConceptTypeRelatesToConceptType)conceptTypeChild, conceptTypeChildPath));
						}
					}

					return columns;
				}
			}
			foreach (ConceptTypeAssimilatesConceptType assimilation in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType))
			{
				if (assimilation.IsPreferredForTarget)
				{
					bool isAbsorb = (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation) == AssimilationAbsorptionChoice.Absorb);
					
					conceptTypeChildPath.Push(assimilation);

					List<Column> columns = GetPreferredIdentifierColumnsForConceptType(assimilation.AssimilatorConceptType, conceptTypeChildPath);

					conceptTypeChildPath.Pop();

					return columns;
				}
			}
			foreach (ConceptTypeAssimilatesConceptType assimilation in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(conceptType))
			{
				if (assimilation.IsPreferredForParent)
				{
					bool isPartition = (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation) == AssimilationAbsorptionChoice.Partition);

					conceptTypeChildPath.Push(assimilation);
					
					List<Column> columns = GetPreferredIdentifierColumnsForConceptType(assimilation.AssimilatedConceptType, conceptTypeChildPath);

					conceptTypeChildPath.Pop();
					
					return columns;
				}
			}
			Debug.Fail("Couldn't find preferred identifier for concept type.");
			throw new InvalidOperationException();
		}

		/// <summary>
		/// A callback delegate for <see cref="GetColumnsForConceptTypeChild"/>
		/// </summary>
		/// <param name="childInformationType">The current <see cref="InformationType"/></param>
		/// <param name="childPathStack">The path to the <paramref name="childInformationType"/>. Converting
		/// the stack to an array and reversing will give the assimilation path. The childInformationType will be on the top of the stack.
		/// The stack is <see langword="null"/> if no stack is passed into GetColumnsForConceptTypeChild.</param>
		private delegate void ProcessChildInformationType(InformationType childInformationType, Stack<ConceptTypeChild> childPathStack);
		/// <summary>
		/// GetColumnsForConceptTypeChild gets the <see cref="Column"/> coresponding to a <see cref="ConceptTypeChild"/>.
		/// </summary>
		/// <remarks>
		/// GetColumnsForConceptTypeChild enables a callback for each InformationType. The callback delegate can create
		/// a new column or use the callback for information purposes only.
		/// </remarks>
		/// <param name="conceptTypeChild">The child to process</param>
		/// <param name="conceptTypeChildPath">A stack of preceding children in the path. Provide an empty starting stack
		/// if stack information is required for the callback.</param>
		/// <param name="informationTypeCallback">A callback method to process informaton types</param>
		private static void GetColumnsForConceptTypeChild(ConceptTypeChild conceptTypeChild, Stack<ConceptTypeChild> conceptTypeChildPath, ProcessChildInformationType informationTypeCallback)
		{
			if (conceptTypeChildPath != null)
			{
				conceptTypeChildPath.Push(conceptTypeChild);
			}
			InformationType informationType = conceptTypeChild as InformationType;

			if (null != (informationType = conceptTypeChild as InformationType))
			{
				informationTypeCallback(informationType, conceptTypeChildPath);
			}
			else
			{
				ConceptType recursePreferredForConceptType = null;
				if (conceptTypeChild is ConceptTypeRelatesToConceptType)
				{
					recursePreferredForConceptType = (ConceptType)conceptTypeChild.Target;
				}
				else if (conceptTypeChild is ConceptTypeAssimilatesConceptType)
				{
					recursePreferredForConceptType = conceptTypeChild.Parent;
				}
				if (recursePreferredForConceptType != null)
				{
					IList<ConceptTypeChild> preferredList = GetPreferredConceptTypeChildrenForConceptType(recursePreferredForConceptType);
					int preferredCount = preferredList.Count;
					for (int i = 0; i < preferredCount; ++i)
					{
						GetColumnsForConceptTypeChild(preferredList[i], conceptTypeChildPath, informationTypeCallback);
					}
				}
			}
			if (conceptTypeChildPath != null)
			{
				conceptTypeChildPath.Pop();
			}
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
		/// returning the list of all ConceptTypeChildren that are Preferred for the ConceptType.
		/// </summary>
		private static IList<ConceptTypeChild> GetPreferredConceptTypeChildrenForConceptType(ConceptType conceptType)
		{
			foreach (Uniqueness uniqueness in conceptType.UniquenessCollection)
			{
				if (uniqueness.IsPreferred)
				{
					return uniqueness.ConceptTypeChildCollection;
				}
			}
			ConceptTypeAssimilatesConceptType bestAssimilation = null;
			ConceptTypeAssimilatesConceptType preferredForParent = null;
			foreach (ConceptTypeAssimilatesConceptType assimilation
				in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType))
			{
				if (assimilation.IsPreferredForTarget)
				{
					bestAssimilation = assimilation;
					break;
				}
				else if (preferredForParent == null && assimilation.IsPreferredForParent)
				{
					preferredForParent = assimilation;
					// Keep going to find the target preference if available
				}
			}
			if (null != bestAssimilation ||
				null != (bestAssimilation = preferredForParent))
			{
				return new ConceptTypeChild[] { bestAssimilation };
			}

			Debug.Fail("Couldn't find preferred identifier.");
			return new ConceptTypeChild[0];
		}

		/// <summary>
		/// DoSeparation handles the generation of columns and foreign keys when an assimilation is set to separate.
		/// </summary>
		private static void DoSeparation(ConceptTypeAssimilatesConceptType assimilation, ref bool isPreferredForChildFound, INotifyElementAdded notifyAdded)
		{
			Table table = TableIsPrimarilyForConceptType.GetTable(assimilation.AssimilatedConceptType);
			if (table != null)
			{
				if (!isPreferredForChildFound)
				{
					Store store = table.Store;
					if (assimilation.IsPreferredForParent)
					{
						ReferenceConstraint referenceConstraint = new ReferenceConstraint(store, new PropertyAssignment[] { new PropertyAssignment(ReferenceConstraint.NameDomainPropertyId, assimilation.Name) });
						TableContainsReferenceConstraint tableContainsReferenceConstraint = new TableContainsReferenceConstraint(TableIsPrimarilyForConceptType.GetTable(assimilation.AssimilatorConceptType), referenceConstraint);
						ReferenceConstraintTargetsTable referenceConstraintTargetsTable = new ReferenceConstraintTargetsTable(referenceConstraint, table);

						UniquenessConstraint mappedConstraint = new UniquenessConstraint(store, new PropertyAssignment[] { new PropertyAssignment(UniquenessConstraint.NameDomainPropertyId, "Constraint") });
						mappedConstraint.IsPrimary = true;

						List<Column> sourceColumns = ConceptTypeHasPrimaryIdentifierColumns(null, assimilation.Parent);
						foreach (Column sourcecolumn in sourceColumns)
						{
							Column targetColumn = FindTarget(sourcecolumn, assimilation.AssimilatedConceptType);
							ColumnReference relationship = new ColumnReference(sourcecolumn, targetColumn);
							if (notifyAdded != null)
							{
								notifyAdded.ElementAdded(relationship);
							}
							referenceConstraint.ColumnReferenceCollection.Add(relationship);
							mappedConstraint.ColumnCollection.Add(sourcecolumn);
						}
						TableIsPrimarilyForConceptType.GetTable(assimilation.AssimilatorConceptType).UniquenessConstraintCollection.Add(mappedConstraint);
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(mappedConstraint, true);
							notifyAdded.ElementAdded(referenceConstraint, true);
						}
					}
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
							ReferenceConstraint referenceConstraint = new ReferenceConstraint(store, new PropertyAssignment[] { new PropertyAssignment(ReferenceConstraint.NameDomainPropertyId, assimilation.Name) });
							TableContainsReferenceConstraint tableContainsReferenceConstraint = new TableContainsReferenceConstraint(table, referenceConstraint);
							ReferenceConstraintTargetsTable referenceConstraintTargetsTable = new ReferenceConstraintTargetsTable(referenceConstraint, targetTable);

							UniquenessConstraint mappedConstraint = new UniquenessConstraint(store, new PropertyAssignment[] { new PropertyAssignment(UniquenessConstraint.NameDomainPropertyId, "Constraint") });

							bool seenPrimary = false;
							if (!assimilation.IsPreferredForTarget)
							{
								GetColumnsForConceptTypeChild(
									GetPreferredConceptTypeChildrenForConceptType(assimilation.AssimilatedConceptType)[0],
									null,
									delegate(InformationType childInformationType, Stack<ConceptTypeChild> childPathStack)
									{
										seenPrimary = true;
									});
							}
							if (!seenPrimary)
							{
								mappedConstraint.IsPrimary = true;
							}

							LinkedElementCollection<ConceptType> assimilations;
							ConceptType conceptType = assimilation.AssimilatedConceptType;

							bool primaryIdentifierFound = false;
							while (!primaryIdentifierFound && (assimilations = conceptType.AssimilatorConceptTypeCollection).Count != 0)
							{
								conceptType = assimilations[0];

								Table primaryTable = TableIsPrimarilyForConceptType.GetTable(conceptType);

								if (primaryTable != null)
								{
									foreach (UniquenessConstraint uniqueness in primaryTable.UniquenessConstraintCollection)
									{
										if (uniqueness.IsPrimary)
										{
											foreach (Column identifierColumn in uniqueness.ColumnCollection)
											{
												Column clonedColumn = new Column(store, new PropertyAssignment[] { new PropertyAssignment(Column.NameDomainPropertyId, identifierColumn.Name) });
												LinkedElementCollection<ConceptTypeChild> clonedColumnPath = ColumnHasConceptTypeChild.GetConceptTypeChildPath(clonedColumn);
												clonedColumnPath.Add(assimilation);
												clonedColumnPath.AddRange(ColumnHasConceptTypeChild.GetConceptTypeChildPath(identifierColumn));
												new TableContainsColumn(table, clonedColumn);
												new ReferenceConstraintContainsColumnReference(referenceConstraint, new ColumnReference(clonedColumn, identifierColumn));
												if (notifyAdded != null)
												{
													// This notify takes care of the column and the column reference. The
													// reference constraint containment will be done with the reference constraint
													// notification, as will all columns in mappedConstraint. The cloned column
													// is intentionally added to the mapped constraint after this point
													notifyAdded.ElementAdded(clonedColumn, true);
												}
												new UniquenessConstraintIncludesColumn(mappedConstraint, clonedColumn);
											}
											primaryIdentifierFound = true;
										}
									}

								}
							}
							if (!primaryIdentifierFound)
							{
								GetColumnsForConceptTypeChild(
									assimilation,
									new Stack<ConceptTypeChild>(),
									delegate(InformationType childInformationType, Stack<ConceptTypeChild> childPathStack)
									{
										Column informationTypeColumn = new Column(
											childInformationType.Store,
											new PropertyAssignment[] { new PropertyAssignment(Column.NameDomainPropertyId, childInformationType.Name) });
										ConceptTypeChild[] pathArray = childPathStack.ToArray();
										int pathLength = pathArray.Length;
										if (pathLength > 1)
										{
											Array.Reverse(pathArray);
										}
										ColumnHasConceptTypeChild.GetConceptTypeChildPath(informationTypeColumn).AddRange(pathArray);
										new TableContainsColumn(table, informationTypeColumn);

										// Create a column reference to the other column
										foreach (Column possibleColumn in targetTable.ColumnCollection)
										{
											LinkedElementCollection<ConceptTypeChild> possiblePath = ColumnHasConceptTypeChild.GetConceptTypeChildPath(possibleColumn);
											int possiblePathCount = possiblePath.Count;

											if (possiblePathCount < pathLength)
											{
												// We're looking for the end of the separated path to match
												// the full possible path, and the Parent of the previous
												// ConceptTypeChild in the separated path to match the
												// concept type for the target table.
												bool isMatch = true;
												int sizeDifference = pathLength - possiblePathCount;
												for (int i = possiblePathCount - 1; i >= 0; --i)
												{
													if (possiblePath[i] != pathArray[i + sizeDifference])
													{
														isMatch = false;
														break;
													}
												}
												if (isMatch && pathArray[sizeDifference - 1].Parent == parentConcept)
												{
													new ReferenceConstraintContainsColumnReference(referenceConstraint, new ColumnReference(informationTypeColumn, possibleColumn));
													break;
												}
											}
										}
										if (notifyAdded != null)
										{
											// See previous comments on ElementAdded. Not binding the column to
											// mappedConstraint until after this point is intentional
											notifyAdded.ElementAdded(informationTypeColumn, true);
										}
										new UniquenessConstraintIncludesColumn(mappedConstraint, informationTypeColumn);
									});
							}
							new TableContainsUniquenessConstraint(table, mappedConstraint);
							if (notifyAdded != null)
							{
								notifyAdded.ElementAdded(mappedConstraint, true);
								notifyAdded.ElementAdded(referenceConstraint, true);
							}
							isPreferredForChildFound = true;
						}
					}
				}
			}
			else
			{
				// If the separation is further partitioned then it will not have a secondary child table, further assimilations or separations are ok though.
				ReadOnlyCollection<ConceptTypeAssimilatesConceptType> childAssimilations = ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(assimilation.AssimilatedConceptType);
				bool isPartitioned = false;
				foreach (ConceptTypeAssimilatesConceptType possiblePartition in childAssimilations)
				{
					if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(possiblePartition) == AssimilationAbsorptionChoice.Partition)
					{
						isPartitioned = true;
						break;
					}
				}
				if (isPartitioned)
				{
					foreach (ConceptTypeAssimilatesConceptType partition in childAssimilations)
					{
						bool preferred = false;
						DoSeparation(partition, ref preferred, notifyAdded);
					}
				}
			}
		}

		/// <summary>
		/// CreateForeignKeys looks at a table and generates all Foreign Keys (<see cref="ReferenceConstraint"/>) required by the columns in that table.
		/// </summary>
		/// <param name="table">The table to check for foreign keys on.</param>
		/// <param name="notifyAdded">Deserialization callback</param>
		private static void CreateForeignKeys(Table table, INotifyElementAdded notifyAdded)
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
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(referenceConstraint, true);
						}
					}
					ColumnReference columnReference = new ColumnReference(column, targetColumn);
					referenceColumns.Add(columnReference);
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(columnReference, true);
					}
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

					bool containsPartitions = false;
					foreach (ConceptTypeAssimilatesConceptType act in childCollection)
					{
						if (AssimilationMapping.GetAssimilationMappingFromAssimilation(act).AbsorptionChoice == AssimilationAbsorptionChoice.Partition)
						{

							containsPartitions = true;
							break;
						}
					}
					if (!containsPartitions)
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
					else
					{
						foreach (ConceptTypeAssimilatesConceptType act in childCollection)
						{
							columns.AddRange(ConceptTypeHasPrimaryIdentifierColumns(column, act.AssimilatedConceptType));
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
		/// <param name="sourceColumn">The <see cref="Column"/>.</param>
		/// <param name="targetConceptType">The <see cref="ConceptType"/>.</param>
		/// <returns>The target <see cref="Column"/>.</returns>
		private static Column FindTarget(Column sourceColumn, ConceptType targetConceptType)
		{
			Table targetTable = TableIsPrimarilyForConceptType.GetTable(targetConceptType);
			TableIsAlsoForConceptType tableIsAlsoForConceptType;
			if (targetTable == null)
			{
				ReadOnlyCollection<TableIsAlsoForConceptType> tableIsAlsoForConceptTypeLinks = TableIsAlsoForConceptType.GetLinksToTable(targetConceptType);
				if (tableIsAlsoForConceptTypeLinks.Count != 1)
				{
					return null;
				}
				else
				{
					tableIsAlsoForConceptType = tableIsAlsoForConceptTypeLinks[0];
					targetTable = tableIsAlsoForConceptType.Table;
				}
			}
			else
			{
				tableIsAlsoForConceptType = null;
			}

			LinkedElementCollection<ConceptTypeChild> sourceCtcPath = ColumnHasConceptTypeChild.GetConceptTypeChildPath(sourceColumn);
			List<ConceptTypeChild> targetCtcPath = new List<ConceptTypeChild>(sourceCtcPath.Count);
			
			// This handles removing the source concept type's assimilation path,
			// and the relationship itself.
			// UNDONE: Can the relationship portion of this ever have more than one CTC in it? If so, this will break badly.
			bool finishedWithAssimilationPath = false;
			bool copyPath = false;
			for (int i = 0; i < sourceCtcPath.Count; i++)
			{
				ConceptTypeChild currentSourcePathChild = sourceCtcPath[i];
				if (copyPath)
				{
					targetCtcPath.Add(currentSourcePathChild);
				}
				else
				{
					if (!finishedWithAssimilationPath)
					{
						ConceptTypeAssimilatesConceptType currentAssimilation = currentSourcePathChild as ConceptTypeAssimilatesConceptType;
						if (currentAssimilation != null)
						{
							if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(currentAssimilation) != AssimilationAbsorptionChoice.Separate)
							{
								continue;
							}
						}
						finishedWithAssimilationPath = true;
					}
					if (currentSourcePathChild.Target == targetConceptType)
					{
						// This is the end of the assimilations and we're now at the start of the path to the preferred identifier.
						copyPath = true;
					}
				}
			}

			if (tableIsAlsoForConceptType != null)
			{
				LinkedElementCollection<ConceptTypeAssimilatesConceptType> targetAssimilationPath = TableIsAlsoForConceptTypeHasAssimilationPath.GetAssimilationPath(tableIsAlsoForConceptType);
				InformationType informationType = (InformationType)sourceCtcPath[sourceCtcPath.Count - 1];
				ConceptType informationTypeParent = informationType.Parent;

				// Remove everything at and after index, and add everything before it
				int index;
				int officialAssimilationPathCount = targetAssimilationPath.Count;
				for (index = 0; index < officialAssimilationPathCount; index++)
				{
					if (targetAssimilationPath[index].Parent == informationTypeParent)
					{
						break;
					}
				}

				int removeCount = officialAssimilationPathCount - index;
				for (int i = officialAssimilationPathCount - 1, j = 0; i >= index; i--, j++)
				{
					if (targetCtcPath[j] != targetAssimilationPath[i])
					{
						// Handle alternate assimilation paths
						ConceptType testAssimilator = targetAssimilationPath[index].AssimilatorConceptType;
						int targetPathCount = targetCtcPath.Count;
						for (int k = j + 1; k < targetPathCount; ++k)
						{
							ConceptTypeAssimilatesConceptType targetAssimilation = targetCtcPath[k] as ConceptTypeAssimilatesConceptType;
							Debug.Assert(targetAssimilation != null, "Alternate assimilation paths should rejoin before we run out of assimilations.");
							if (targetAssimilation != null)
							{
								if (targetAssimilation.AssimilatorConceptType == testAssimilator)
								{
									removeCount = k + 1;
									break;
								}
							}
						}
						break;
					}
				}

				targetCtcPath.RemoveRange(0, removeCount);

				for (int i = index - 1; i >= 0; i--)
				{
					targetCtcPath.Insert(0, targetAssimilationPath[i]);
				}
			}

			foreach (Column possibleColumn in targetTable.ColumnCollection)
			{
				if (possibleColumn == sourceColumn)
				{
					continue;
				}
				bool isMatch = true;
				LinkedElementCollection<ConceptTypeChild> possibleCtcPath = ColumnHasConceptTypeChild.GetConceptTypeChildPath(possibleColumn);
				
				if (possibleCtcPath.Count == targetCtcPath.Count)
				{
					for (int i = targetCtcPath.Count - 1; i >= 0; i--)
					{
						if (possibleCtcPath[i] != targetCtcPath[i])
						{
							isMatch = false;
							break;
						}
					}
					if (isMatch)
					{
						return possibleColumn;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// CreateUniquenessConstraints looks at a <see cref="ConceptType"/> and determines if it is unique or not.
		/// </summary>
		/// <param name="conceptType">The <see cref="ConceptType"/>.</param>
		/// <param name="notifyAdded">Deserialization callback when elements added</param>
		private static void CreateUniquenessConstraints(ConceptType conceptType, INotifyElementAdded notifyAdded)
		{
			// UNDONE: Look here for possible problems with ring constraints
			Table isPrimarilyForTable = TableIsPrimarilyForConceptType.GetTable(conceptType);
			if (isPrimarilyForTable != null)
			{
				List<Uniqueness> alreadyDone = new List<Uniqueness>();
				foreach (Uniqueness uniqueness in conceptType.UniquenessCollection)
				{
					UniquenessConstraint uniquenessConstraint = new UniquenessConstraint(uniqueness.Store, new PropertyAssignment[] { new PropertyAssignment(UniquenessConstraint.NameDomainPropertyId, uniqueness.Name), new PropertyAssignment(UniquenessConstraint.IsPrimaryDomainPropertyId, uniqueness.IsPreferred) });
					new TableContainsUniquenessConstraint(isPrimarilyForTable, uniquenessConstraint);
					new UniquenessConstraintIsForUniqueness(uniquenessConstraint, uniqueness);
					foreach (ConceptTypeChild conceptTypeChild in uniqueness.ConceptTypeChildCollection)
					{
						foreach (Column column in ColumnHasConceptTypeChild.GetColumn(conceptTypeChild))
						{
							if (column.Table == isPrimarilyForTable && ColumnHasConceptTypeChild.GetConceptTypeChildPath(column)[0] == conceptTypeChild)
							{
								new UniquenessConstraintIncludesColumn(uniquenessConstraint, column);
							}
						}
					}
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(uniquenessConstraint, true);
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
						LinkedElementCollection<Column> tableColumns = table.ColumnCollection;
						LinkedElementCollection<ConceptTypeAssimilatesConceptType> assimilationPath = tableIsAlsoForConceptType.AssimilationPath;
						int assimilationPathCount = assimilationPath.Count;

						LinkedElementCollection<ConceptTypeChild> uniquenessConceptTypeChildren = uniqueness.ConceptTypeChildCollection;

						UniquenessConstraint uniquenessConstraint = new UniquenessConstraint(table.Store, new PropertyAssignment[] { new PropertyAssignment(UniquenessConstraint.NameDomainPropertyId, uniqueness.Name) });
						uniquenessConstraint.Table = table;
						new UniquenessConstraintIsForUniqueness(uniquenessConstraint, uniqueness);
						LinkedElementCollection<Column> uniquenessColumns = uniquenessConstraint.ColumnCollection;

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

						foreach (Column column in tableColumns)
						{
							LinkedElementCollection<ConceptTypeChild> ctcPath = ColumnHasConceptTypeChild.GetConceptTypeChildPath(column);
							bool pathMatches = ctcPath.Count >= assimilationPathCount + 1;
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

						Debug.Assert(matchingColumns.Count >= uniquenessConceptTypeChildren.Count);

						foreach (ConceptTypeChild conceptTypeChild in uniqueness.ConceptTypeChildCollection)
						{
							for (int i = matchingColumns.Count - 1; i >= 0; i--)
							{
								Column column = matchingColumns[i];
								if (ColumnHasConceptTypeChild.GetConceptTypeChildPath(column)[assimilationPathCount] == conceptTypeChild)
								{
									uniquenessColumns.Add(column);
									matchingColumns.RemoveAt(i);
								}
							}
						}
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(uniquenessConstraint, true);
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
				ConceptTypeRelatesToConceptType relation;
				if (null != (relation = conceptTypeChild as ConceptTypeRelatesToConceptType))
				{
					return relation;
				}
				ConceptTypeAssimilatesConceptType assimilation = conceptTypeChild as ConceptTypeAssimilatesConceptType;
				if (assimilation == null || // We have an InformationType
					AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation) == AssimilationAbsorptionChoice.Separate)
				{
					return null;
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
				CheckColumnConstraint(column, table);
			}
		}

		/// <summary>
		/// CheckColumnConstraint looks at the the ConceptTypeChildPath for a column, setting the column to nullable when some point in the path is not mandatory. 
		/// </summary>
		private static void CheckColumnConstraint(Column column)
		{
			CheckColumnConstraint(column, null);
		}
		/// <summary>
		/// CheckColumnConstraint looks at the the ConceptTypeChildPath for a column, setting the column to nullable when some point in the path is not mandatory. 
		/// </summary>
		private static void CheckColumnConstraint(Column column, Table table)
		{
			bool allStepsMandatory = true;
			ConceptType lastTarget = null;
			bool firstPass = true;
			foreach (ConceptTypeChild child in ColumnHasConceptTypeChild.GetConceptTypeChildPath(column))
			{
				if (!child.IsMandatory)
				{
					ConceptTypeAssimilatesConceptType assimilation = child as ConceptTypeAssimilatesConceptType;
					if (assimilation != null)
					{
						// The IsMandatory property applies when stepping parent-to-target, However, stepping target-to-parent
						// is always considered mandatory. See if we're in this situation.
						if (firstPass)
						{
							if (table == null)
							{
								table = column.Table;
							}
							lastTarget = TableIsPrimarilyForConceptType.GetConceptType(table);

						}
						if (lastTarget != null &&
							lastTarget == assimilation.Target)
						{
							lastTarget = assimilation.Parent;
							firstPass = false;
							continue;
						}
					}
					allStepsMandatory = false;
					break;
				}
				lastTarget = child.Target as ConceptType;
				firstPass = false;
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

					FullyGenerateConceptualDatabaseModel(schema, element, notifyAdded);
				}
			}
		}
		#endregion // GenerateConceptualDatabaseFixupListener class
	}
}
