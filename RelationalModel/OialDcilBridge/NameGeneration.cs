//#define DEBUGCOLUMNPATH
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.ORMAbstraction;
using ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge;
using ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase;
using UniquenessConstraint = ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.UniquenessConstraint;
using ORMUniquenessConstraint = ORMSolutions.ORMArchitect.Core.ObjectModel.UniquenessConstraint;

namespace ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge
{
	#region IDatabaseNameGenerator interface
	/// <summary>
	/// Generate relational names for elements contained in a <see cref="Schema"/>
	/// </summary>
	public interface IDatabaseNameGenerator
	{
		/// <summary>
		/// Generate a name for the provided <paramref name="table"/>
		/// </summary>
		/// <param name="table">A <see cref="Table"/> element</param>
		/// <param name="phase">The current phase of the name to generate. As the phase number goes
		/// higher the returned name should be more complex. The initial request will be 0, with additional
		/// requested incremented 1 from the previous name request.</param>
		/// <returns>A name for <paramref name="table"/>.</returns>
		string GenerateTableName(Table table, int phase);
		/// <summary>
		/// Generate a name for the provided <paramref name="column"/>
		/// </summary>
		/// <param name="column">A <see cref="Column"/> element</param>
		/// <param name="phase">The current phase of the name to generate. As the phase number goes
		/// higher the returned name should be more complex. The initial request will be 0, with additional
		/// requested incremented 1 from the previous name request.</param>
		/// <returns>A name for <paramref name="column"/>.</returns>
		string GenerateColumnName(Column column, int phase);
		/// <summary>
		/// Generate a name for the provided <paramref name="constraint"/>
		/// </summary>
		/// <param name="constraint">A <see cref="Constraint"/> element</param>
		/// <param name="phase">The current phase of the name to generate. As the phase number goes
		/// higher the returned name should be more complex. The initial request will be 0, with additional
		/// requested incremented 1 from the previous name request.</param>
		/// <returns>A name for <paramref name="constraint"/>.</returns>
		string GenerateConstraintName(Constraint constraint, int phase);
	}
	#endregion // IDatabaseNameGenerator interface
	#region ORMAbstractionToConceptualDatabaseBridgeDomainModel.NameGeneration class
	partial class ORMAbstractionToConceptualDatabaseBridgeDomainModel
	{
		private static class NameGeneration
		{
			#region GenerateAllNames method
			/// <summary>
			/// Generate names for the provided <see cref="Schema"/>
			/// </summary>
			public static void GenerateAllNames(Schema schema, SchemaCustomization customization)
			{
				// Verify the schema name
				AbstractionModel abstraction;
				string customSchemaName;
				if (null != (customSchemaName = 
						((null == customization ||
						null == (customSchemaName = customization.CustomizedSchemaName)) &&
						null != (abstraction = SchemaIsForAbstractionModel.GetAbstractionModel(schema))) ?
					abstraction.Name :
					null))
				{
					schema.Name = customSchemaName;
				}

				IDatabaseNameGenerator nameGenerator = new DefaultDatabaseNameGenerator(schema.Store);
				if (customization != null &&
					!customization.CustomizesTablesOrColumns)
				{
					customization = null;
				}

				LinkedElementCollection<Table> tables = schema.TableCollection;

				// Generate table names
				Utility.GenerateUniqueNames<Table>(
					tables,
					delegate(Table table, int phase)
					{
						string tableName;
						if (customization != null &&
							null != (tableName = customization.GetCustomizedTableName(table)))
						{
							if (phase == 0)
							{
								// Make sure this is set or it won't be remembered as a custom name.
								// The schema customization should currently be null, so this won't
								// trigger any rules.
								table.CustomName = true;
								return tableName;
							}
							return null; // Don't offer a new name on collision
						}
						return nameGenerator.GenerateTableName(table, phase);
					},
					delegate(Table table)
					{
						return customization != null && customization.GetCustomizedTableName(table) != null;
					},
					delegate(Table table, string elementName)
					{
						table.Name = elementName;
					});

				foreach (Table table in tables)
				{
					//column names
					LinkedElementCollection<Column> columns = table.ColumnCollection;
					AutomaticColumnOrdering orderingAlgorithm = ResolveOrderingAlgorithm(table, schema);
					bool isCustomized;
					Utility.GenerateUniqueNames<Column>(
						SortColumns(table, columns, orderingAlgorithm, true, customization, out isCustomized),
						delegate(Column column, int phase)
						{
							string columnName;
							if (customization != null &&
								null != (columnName = customization.GetCustomizedColumnName(column)))
							{
								if (phase == 0)
								{
									// Make sure this is set or it won't be remembered as a custom name.
									// The schema customization should currently be null, so this won't
									// trigger any rules.
									column.CustomName = true;
									return columnName;
								}
								return null; // Don't offer a new name on collision
							}
							return nameGenerator.GenerateColumnName(column, phase);
						},
						delegate(Column column)
						{
							return customization != null && customization.GetCustomizedColumnName(column) != null;
						},
						delegate(Column column, string elementName)
						{
							column.Name = elementName;
						});
					if (isCustomized && table.ColumnOrder != ColumnOrdering.Custom)
					{
						table.ColumnOrder = ColumnOrdering.Custom;
						orderingAlgorithm = schema.DefaultColumnOrder;
					}
					IList<Column> sortedColumns = SortColumns(table, columns, orderingAlgorithm, false, customization, out isCustomized);
					if (sortedColumns != columns)
					{
						int columnCount = sortedColumns.Count - 1; // Last one will fall in naturally, -1 is sufficient
						for (int i = 0; i < columnCount; ++i)
						{
							columns.Move(sortedColumns[i], i);
						}
					}
				}

				// Constraint names, unique across the schema
				Utility.GenerateUniqueNames<Constraint>(
					IterateConstraints(schema),
					delegate(Constraint constraint, int phase)
					{
						return nameGenerator.GenerateConstraintName(constraint, phase);
					},
					null,
					delegate(Constraint constraint, string elementName)
					{
						constraint.Name = elementName;
					});
			}
			private sealed class ColumnSorter : IComparer<int>
			{
				private IList<Column> myColumns;
				private int[] myPreferredOrder;
				private AutomaticColumnOrdering myAlgorithm;
				private bool myIgnoreNames;
				private bool myPreferredOrderSecondary;
				public ColumnSorter(IList<Column> columns, int[] preferredOrder, AutomaticColumnOrdering orderingAlgorithm, bool preferredOrderSecondary, bool ignoreNames)
				{
					myColumns = columns;
					myPreferredOrder = preferredOrder;
					myAlgorithm = orderingAlgorithm;
					myPreferredOrderSecondary = preferredOrderSecondary;
					myIgnoreNames = ignoreNames;
				}
				public IList<Column> Apply()
				{
					IList<Column> columns = myColumns;
					int count = columns.Count;
					int[] indices = new int[count];
					int i;
					for (i = 0; i < count; ++i)
					{
						indices[i] = i;
					}
					Array.Sort<int>(indices, this);
					for (i = 0; i < count; ++i)
					{
						if (indices[i] != i)
						{
							break;
						}
					}
					if (i == count)
					{
						return columns;
					}
					Column[] newColumns = new Column[count];
					for (i = 0; i < count; ++i)
					{
						newColumns[i] = columns[indices[i]];
					}
					return newColumns;
				}
				/// <summary>
				/// Determine the overall automatic sort group for a column.
				/// The grouping is based on the current ordering algorithm.
				/// </summary>
				private int GetAutoSortGroup(Column column)
				{
					bool isUnique = false;
					AutomaticColumnOrdering algorithm;
					switch (algorithm = myAlgorithm)
					{
						case AutomaticColumnOrdering.PrimaryOther:
						case AutomaticColumnOrdering.PrimaryMandatoryOther:
						case AutomaticColumnOrdering.PrimaryMandatoryUniqueOther:
						case AutomaticColumnOrdering.PrimaryUniqueMandatoryOther:
						case AutomaticColumnOrdering.PrimaryUniqueOther:
							foreach (UniquenessConstraint constraint in UniquenessConstraintIncludesColumn.GetUniquenessConstraints(column))
							{
								if (constraint.IsPrimary)
								{
									return 0;
								}
								isUnique = true;
							}
							switch (algorithm)
							{
								case AutomaticColumnOrdering.PrimaryUniqueOther:
									return isUnique ? 1 : 2;
								case AutomaticColumnOrdering.PrimaryUniqueMandatoryOther:
									return isUnique ? 1 : (column.IsNullable ? 3 : 2);
								default:
									return (!column.IsNullable && (algorithm != AutomaticColumnOrdering.PrimaryOther)) ? 1 : ((isUnique && (algorithm == AutomaticColumnOrdering.PrimaryMandatoryUniqueOther)) ? 2 : 3);
							}
						case AutomaticColumnOrdering.MandatoryOther:
							return column.IsNullable ? 1 : 0;
						default:
							return 0;
					}
				}
				int IComparer<int>.Compare(int x, int y)
				{
					int[] preferredOrder = myPreferredOrder;
					bool preferredOrderSecondary = preferredOrder != null && myPreferredOrderSecondary;
					int tempX;
					int tempY;
					if (preferredOrder != null &&
						!preferredOrderSecondary)
					{
						tempX = preferredOrder[x];
						tempY = preferredOrder[y];
						if (tempX != -1)
						{
							if (tempY == -1)
							{
								return -1;
							}
							else if (tempY != tempX)
							{
								return tempX - tempY;
							}
						}
						else if (tempY != -1)
						{
							return 1;
						}
					}
					IList<Column> columns = myColumns;
					Column columnX = columns[x];
					Column columnY = columns[y];
					tempX = GetAutoSortGroup(columnX);
					tempY = GetAutoSortGroup(columnY);
					if (tempX != tempY)
					{
						return tempX - tempY;
					}
					else if (preferredOrderSecondary)
					{
						tempX = preferredOrder[x];
						tempY = preferredOrder[y];
						if (tempX != -1)
						{
							if (tempY == -1)
							{
								return -1;
							}
							else if (tempY != tempX)
							{
								return tempX - tempY;
							}
						}
						else if (tempY != -1)
						{
							return 1;
						}
					}
					if (!myIgnoreNames)
					{
						if (0 != (tempX = string.Compare(columnX.Name, columnY.Name, StringComparison.CurrentCulture)))
						{
							return tempX;
						}
					}
					return columnX.Id.CompareTo(columnY.Id);
				}
			}
			private static AutomaticColumnOrdering ResolveOrderingAlgorithm(Table table, Schema schema)
			{
				switch (table.ColumnOrder)
				{
					case ColumnOrdering.AutoPrimaryMandatoryUniqueOther:
						return AutomaticColumnOrdering.PrimaryMandatoryUniqueOther;
					case ColumnOrdering.AutoPrimaryMandatoryOther:
						return AutomaticColumnOrdering.PrimaryMandatoryOther;
					case ColumnOrdering.AutoPrimaryUniqueMandatoryOther:
						return AutomaticColumnOrdering.PrimaryUniqueMandatoryOther;
					case ColumnOrdering.AutoPrimaryUniqueOther:
						return AutomaticColumnOrdering.PrimaryUniqueOther;
					case ColumnOrdering.AutoPrimaryOther:
						return AutomaticColumnOrdering.PrimaryOther;
					case ColumnOrdering.AutoMandatoryOther:
						return AutomaticColumnOrdering.MandatoryOther;
					case ColumnOrdering.AutoByColumnName:
						return AutomaticColumnOrdering.ByColumnName;
					//case ColumnOrdering.AutoSchemaDefault:
					//case ColumnOrdering.Custom:
					default:
						return schema.DefaultColumnOrder;
				}
			}
			private static IList<Column> SortColumns(Table table, IList<Column> startColumns, AutomaticColumnOrdering orderingAlgorithm, bool ignoreNames, SchemaCustomization customizations, out bool isCustomized)
			{
				int count = startColumns.Count;
				int unorderedCount = 0;
				isCustomized = false;
				if (count < 2)
				{
					return startColumns;
				}
				int[] preferredOrder = null;
				if (customizations != null)
				{
					for (int i = 0; i < count; ++i)
					{
						int customizedIndex = customizations.GetCustomizedColumnPosition(startColumns[i]);
						if (customizedIndex != -1)
						{
							if (preferredOrder == null)
							{
								preferredOrder = new int[count];
								for (int j = 0; j < i; ++j)
								{
									++unorderedCount;
									preferredOrder[j] = -1;
								}
							}
							preferredOrder[i] = customizedIndex;
						}
						else if (preferredOrder != null)
						{
							++unorderedCount;
							preferredOrder[i] = -1;
						}
					}
				}
				bool preferredSortSecondary = false;
				if (unorderedCount != 0 &&
					!customizations.GetHasCustomOrderedColumns(table))
				{
					if ((count - unorderedCount) > 1)
					{
						// Don't switch to custom unless more than one
						preferredSortSecondary = true;
					}
					else
					{
						preferredOrder = null;
					}
				}
				isCustomized = preferredOrder != null;
				// If there is a partial sort order, then we're looking at two possible cases:
				// 1) The table was previously custom ordered and a new column has been added. In this
				//    case, we add the new columns to the end of the table.
				// 2) The table was not previously custom sorted, but has custom ordered columns. This
				//    happens in partition and separation cases. In this case, we want the custom
				//    columns to sort relative to each other, but not to the new columns.
				// In the first case, we want the preferred ordering to take the highest sort priority (before grouping).
				// In the second case, we want the preferred ordering to be prioritized after grouping.
				return new ColumnSorter(startColumns, preferredOrder, orderingAlgorithm, preferredSortSecondary, ignoreNames).Apply();
			}
			private static IEnumerable<Constraint> IterateConstraints(Schema schema)
			{
				foreach (Table table in schema.TableCollection)
				{
					foreach (Constraint constraint in TableContainsConstraint.GetConstraintCollection(table))
					{
						yield return constraint;
					}
				}
			}
			#endregion // GenerateAllNames method
			#region DefaultDatabaseNameGenerator class
			private class DefaultDatabaseNameGenerator : IDatabaseNameGenerator
			{
				#region Member variables
				private NameGenerator myTableGenerator;
				private NameGenerator myColumnGenerator;
				private ORMModel myORMModel;
				private Store myStore;
				#endregion // Member variables
				#region Constructor
				/// <summary>
				/// Create a new <see cref="DefaultDatabaseNameGenerator"/>
				/// </summary>
				/// <param name="store">The context <see cref="Store"/></param>
				public DefaultDatabaseNameGenerator(Store store)
				{
					myStore = store;
				}
				#endregion // Constructor
				#region IDatabaseNameGenerator Members
				string IDatabaseNameGenerator.GenerateTableName(Table table, int phase)
				{
					return GenerateTableName(table, phase);
				}
				string IDatabaseNameGenerator.GenerateColumnName(Column column, int phase)
				{
					return GenerateColumnName(column, phase);
				}
				string IDatabaseNameGenerator.GenerateConstraintName(Constraint constraint, int phase)
				{
					return GenerateConstraintName(constraint, phase);
				}
				#endregion
				#region Accessor properties
				private NameGenerator ColumnNameGenerator
				{
					get
					{
						NameGenerator retVal = myColumnGenerator;
						if (retVal == null)
						{
							myColumnGenerator = retVal = NameGenerator.GetGenerator(myStore, typeof(RelationalNameGenerator), typeof(ColumnNameUsage), null);
						}
						return retVal;
					}
				}
				private NameGenerator TableNameGenerator
				{
					get
					{
						NameGenerator retVal = myTableGenerator;
						if (retVal == null)
						{
							myTableGenerator = retVal = NameGenerator.GetGenerator(myStore, typeof(RelationalNameGenerator), typeof(TableNameUsage), null);
						}
						return retVal;
					}
				}
				private ORMModel ContextModel
				{
					get
					{
						ORMModel retVal = myORMModel;
						if (retVal == null)
						{
							foreach (ORMModel model in myStore.ElementDirectory.FindElements<ORMModel>(false))
							{
								myORMModel = retVal = model;
								break;
							}
						}
						return retVal;
					}
				}
				#endregion // Accessor properties
				#region Name generation methods
				private string GenerateTableName(Table table, int phase)
				{
					if (phase != 0)
					{
						return null;
					}
					NamePart singleName = default(NamePart);
					List<NamePart> nameCollection = null;
					NameGenerator nameGenerator = TableNameGenerator;

					ReferenceModeNaming.SeparateObjectTypeParts(
						ConceptTypeIsForObjectType.GetObjectType(TableIsPrimarilyForConceptType.GetConceptType(table)),
						nameGenerator,
						delegate(NamePart newPart, int? insertIndex)
						{
							NamePart.AddToNameCollection(ref singleName, ref nameCollection, newPart, insertIndex.HasValue ? insertIndex.Value : -1, true);
						});

					string finalName = NamePart.GetFinalName(singleName, nameCollection, nameGenerator, ContextModel);
					return string.IsNullOrEmpty(finalName) ? "TABLE" : finalName;
				}
				private string GenerateColumnName(Column column, int phase)
				{
					if (phase > 1)
					{
						return null;
					}
					bool decorateWithPredicateText = phase == 1;
					LinkedNode<ColumnPathStep> currentNode = GetColumnPath(column);

					NameGenerator generator = ColumnNameGenerator;
					ORMModel contextModel = ContextModel;

					if (currentNode == null)
					{
						return NamePart.GetFinalName(ResourceStrings.NameGenerationValueTypeValueColumn, null, generator, contextModel);
					}

					// Prepare for adding name parts. The single NamePart string is used when
					// possible to avoid using the list for a single entry
					NamePart singleName = default(NamePart);
					List<NamePart> nameCollection = null;
					AddNamePart addPart = delegate(NamePart newPart, int? insertIndex)
						{
							NamePart.AddToNameCollection(ref singleName, ref nameCollection, newPart, insertIndex.HasValue ? insertIndex.Value : -1, true);
						};
					ObjectType previousResolvedSupertype = null;
					ObjectType previousResolvedObjectType = null;
					ConceptType primaryConceptType = TableIsPrimarilyForConceptType.GetConceptType(column.Table);
					if (primaryConceptType != null)
					{
						previousResolvedSupertype = previousResolvedObjectType = ConceptTypeIsForObjectType.GetObjectType(primaryConceptType);
					}
					ObjectType previousPreviousResolvedSupertype = null;
					ObjectType previousPreviousResolvedObjectType = null;
					bool firstPass = true;
					bool treatNextIdentifierAsFirstStep = false;
					bool lastStepConsumedNextNode = false;
					bool lastStepUsedExplicitRoleName = false;
					do
					{
						LinkedNode<ColumnPathStep> nextLoopNode = currentNode.Next;
						ColumnPathStep step = currentNode.Value;
						ColumnPathStepFlags stepFlags = step.Flags;
						if (treatNextIdentifierAsFirstStep)
						{
							treatNextIdentifierAsFirstStep = false;
							if (0 != (stepFlags & ColumnPathStepFlags.IsIdentifier))
							{
								firstPass = true;
							}
						}
						if (0 != (stepFlags & ColumnPathStepFlags.ForwardAssimilation))
						{
							if (!(lastStepConsumedNextNode || lastStepUsedExplicitRoleName) || currentNode.Previous.Value.TargetObjectType != step.StartingObjectType)
							{
								ReferenceModeNaming.SeparateObjectTypeParts(step.ResolvedSupertype, generator, addPart);
								lastStepConsumedNextNode = false;
								lastStepUsedExplicitRoleName = false;
							}
							// Note that there is no else clause to reset the flags, keep them with their current values
						}
						else if (0 != (stepFlags & ColumnPathStepFlags.ReverseAssimilation))
						{
							// Don't add names for reverse path types
							lastStepConsumedNextNode = false;
							lastStepUsedExplicitRoleName = false;
							treatNextIdentifierAsFirstStep = firstPass;
							if (nextLoopNode == null)
							{
								// Unusual, but can happen with a ValueType with its own table
								ReferenceModeNaming.SeparateObjectTypeParts(step.ResolvedSupertype, generator, addPart);
							}
						}
						else
						{
							bool decorate = 0 != (stepFlags & ColumnPathStepFlags.RequiresDecoration);
							ColumnPathStep nextAssimilationStep;
							bool incorporateNextAssimilation = nextLoopNode != null && (0 != (ColumnPathStepFlags.ForwardAssimilation & (nextAssimilationStep = nextLoopNode.Value).Flags)) && nextAssimilationStep.StartingObjectType == step.TargetObjectType;
							if (decorate ||
								incorporateNextAssimilation || 
								(nextLoopNode == null && (0 == (stepFlags & ColumnPathStepFlags.IsIdentifier) || !(lastStepConsumedNextNode || lastStepUsedExplicitRoleName))))
							{
								LinkedNode<ColumnPathStep> nextNode = nextLoopNode;
								if (!decorate &&
									nextNode == null &&
									0 != (stepFlags & ColumnPathStepFlags.IsIdentifier))
								{
									LinkedNode<ColumnPathStep> previousNode = currentNode.Previous;
									if (previousNode != null)
									{
										ColumnPathStep previousStep = previousNode.Value;
										ColumnPathStepFlags previousFlags = previousStep.Flags;
										if (0 == (previousFlags & (ColumnPathStepFlags.RequiresDecoration | ColumnPathStepFlags.ForwardAssimilation | ColumnPathStepFlags.ReverseAssimilation)) &&
											previousResolvedSupertype.ReferenceModePattern != null)
										{
											// Special condition when the last node is a simple identifier:
											// Back up one step to enable picking up the role or hyphen-bound
											// name instead of just the predicate text.
											stepFlags = previousFlags;
											step = previousStep;
											nextNode = currentNode;
											previousResolvedSupertype = previousPreviousResolvedSupertype;
											previousResolvedObjectType = previousPreviousResolvedObjectType;
										}
									}
								}
								Role nearRole = step.FromRole;
								Role farRole = (0 == (stepFlags & ColumnPathStepFlags.ObjectifiedFactType)) ?
									nearRole.OppositeRoleAlwaysResolveProxy.Role :
									nearRole.OppositeRole.Role;
								string explicitFarRoleName = farRole.Name;
								FactType factType = nearRole.FactType;
								LinkedElementCollection<RoleBase> factTypeRoles = factType.RoleCollection;
								bool isUnary = FactType.GetUnaryRoleIndex(factTypeRoles).HasValue;
								LinkedElementCollection<ReadingOrder> readingOrders = null;
								IReading reading = null;
								if ((decorate && decorateWithPredicateText) || (isUnary && string.IsNullOrEmpty(explicitFarRoleName)))
								{
									readingOrders = factType.ReadingOrderCollection;
									reading = factType.GetMatchingReading(readingOrders, null, nearRole, null, factTypeRoles, MatchingReadingOptions.NoFrontText | (isUnary ? MatchingReadingOptions.AllowAnyOrder : MatchingReadingOptions.None));
								}
								lastStepConsumedNextNode = false;
								lastStepUsedExplicitRoleName = false;
								if (reading != null && !reading.IsDefault)
								{
									string readingText = string.Format(CultureInfo.CurrentCulture, reading.Text, isUnary ? "\x1" : "", "\x1");
									int splitPosition = readingText.LastIndexOf('\x1');
									if (splitPosition > 0)
									{
										addPart(readingText.Substring(0, splitPosition), null);
									}
									if (!isUnary)
									{
										if (nextNode != null)
										{
											ColumnPathStep nextStep = nextNode.Value;
											ColumnPathStepFlags nextFlags;
											if (incorporateNextAssimilation ||
												(0 != ((nextFlags = nextStep.Flags) & ColumnPathStepFlags.RequiresDecoration) &&
												0 != (nextFlags & (ColumnPathStepFlags.ForwardAssimilation | ColumnPathStepFlags.ReverseAssimilation)) &&
												step.TargetObjectType == nextStep.StartingObjectType))
											{
												// Note that this does not interfere with the earlier 'back up one step'
												// because the checked flags are different.
												nextLoopNode = nextNode.Next;
												lastStepConsumedNextNode = ReferenceModeNaming.ResolveObjectTypeName<ReferenceModeNamingCustomizesObjectType, RelationalDefaultReferenceModeNaming, DefaultReferenceModeNamingCustomizesORMModel, MappingCustomizationModel>(
													contextModel, // If multiple schemas were possible the schema would be passed here. As is, any element will do.
													nextStep.ResolvedObjectType,
													(nextLoopNode != null) ? nextLoopNode.Value.TargetObjectType : null,
													nextStep.ResolvedSupertypeVerifyPreferred,
													true,
													ReferenceModeNamingUse.ReferenceToEntityType,
													generator,
													addPart);
											}
											else
											{
												lastStepConsumedNextNode = ReferenceModeNaming.ResolveObjectTypeName<ReferenceModeNamingCustomizesObjectType, RelationalDefaultReferenceModeNaming, DefaultReferenceModeNamingCustomizesORMModel, MappingCustomizationModel>(
													contextModel, // If multiple schemas were possible the schema would be passed here. As is, any element will do.
													step.ResolvedObjectType,
													nextNode.Value.TargetObjectType,
													step.ResolvedSupertypeVerifyPreferred,
													true,
													ReferenceModeNamingUse.ReferenceToEntityType,
													generator,
													addPart);
											}
										}
										else
										{
											ReferenceModeNaming.ResolveObjectTypeName<ReferenceModeNamingCustomizesObjectType, RelationalDefaultReferenceModeNaming, DefaultReferenceModeNamingCustomizesORMModel, MappingCustomizationModel>(
												contextModel, // If multiple schemas were possible the schema would be passed here. As is, any element will do.
												(!firstPass || 0 != (stepFlags & ColumnPathStepFlags.IsIdentifier)) ? previousResolvedSupertype : null,
												step.TargetObjectType,
												previousResolvedObjectType,
												false,
												firstPass ? ReferenceModeNamingUse.PrimaryIdentifier : ReferenceModeNamingUse.ReferenceToEntityType, // Ignored if first parameter is null
												generator,
												addPart);
										}
									}
									if ((readingText.Length - splitPosition) > 1)
									{
										addPart(readingText.Substring(splitPosition + 1), null);
									}
								}
								else if (!string.IsNullOrEmpty(explicitFarRoleName))
								{
									addPart(explicitFarRoleName, null);
									lastStepUsedExplicitRoleName = true;
								}
								else
								{
									// Find an appropriate hyphen bind in the available readings
									string hyphenBoundFormatString = null;
									ReadingOrder readingOrder;
									if (0 != (readingOrders ?? (readingOrders = factType.ReadingOrderCollection)).Count &&
										null != (readingOrder = isUnary ? readingOrders[0] : FactType.FindMatchingReadingOrder(readingOrders, new RoleBase[] { nearRole, farRole })))
									{
										foreach (Reading testReading in readingOrder.ReadingCollection)
										{
											hyphenBoundFormatString = VerbalizationHyphenBinder.GetFormatStringForHyphenBoundRole(testReading, farRole, isUnary);
											if (hyphenBoundFormatString != null)
											{
												break;
											}
										}
									}
									int splitPosition = 0;
									if (hyphenBoundFormatString != null)
									{
										hyphenBoundFormatString = string.Format(CultureInfo.CurrentCulture, hyphenBoundFormatString, "\x1");
										splitPosition = hyphenBoundFormatString.IndexOf('\x1');
										if (splitPosition != 0)
										{
											addPart(hyphenBoundFormatString.Substring(0, splitPosition), null);
										}
									}
									if (nextNode != null)
									{
											ColumnPathStep nextStep = nextNode.Value;
											ColumnPathStepFlags nextFlags;
											if (incorporateNextAssimilation ||
												(0 != ((nextFlags = nextStep.Flags) & ColumnPathStepFlags.RequiresDecoration) &&
												0 != (nextFlags & (ColumnPathStepFlags.ForwardAssimilation | ColumnPathStepFlags.ReverseAssimilation)) &&
												step.TargetObjectType == nextStep.StartingObjectType))
											{
												// Note that this does not interfere with the earlier 'back up one step'
												// because the checked flags are different.

												// Advance the loop one step
												previousPreviousResolvedSupertype = previousResolvedSupertype;
												previousPreviousResolvedObjectType = previousResolvedObjectType;
												previousResolvedSupertype = (0 != (step.Flags & ColumnPathStepFlags.ReverseAssimilation)) ? step.ResolvedSupertypeVerifyPreferred : step.ResolvedSupertype;
												previousResolvedObjectType = step.ResolvedObjectTypeVerifyPreferred;
												nextLoopNode = nextNode.Next;
												step = nextStep;
												lastStepConsumedNextNode = ReferenceModeNaming.ResolveObjectTypeName<ReferenceModeNamingCustomizesObjectType, RelationalDefaultReferenceModeNaming, DefaultReferenceModeNamingCustomizesORMModel, MappingCustomizationModel>(
													contextModel, // If multiple schemas were possible the schema would be passed here. As is, any element will do.
													nextStep.ResolvedObjectType,
													(nextLoopNode != null) ? nextLoopNode.Value.TargetObjectType : null,
													nextStep.ResolvedSupertypeVerifyPreferred,
													true,
													ReferenceModeNamingUse.ReferenceToEntityType,
													generator,
													addPart);
											}
											else
											{
												lastStepConsumedNextNode = ReferenceModeNaming.ResolveObjectTypeName<ReferenceModeNamingCustomizesObjectType, RelationalDefaultReferenceModeNaming, DefaultReferenceModeNamingCustomizesORMModel, MappingCustomizationModel>(
													contextModel, // If multiple schemas were possible the schema would be passed here. As is, any element will do.
													step.ResolvedObjectType,
													nextNode.Value.TargetObjectType,
													step.ResolvedSupertypeVerifyPreferred,
													true,
													ReferenceModeNamingUse.ReferenceToEntityType,
													generator,
													addPart);
											}
									}
									else
									{
										ReferenceModeNaming.ResolveObjectTypeName<ReferenceModeNamingCustomizesObjectType, RelationalDefaultReferenceModeNaming, DefaultReferenceModeNamingCustomizesORMModel, MappingCustomizationModel>(
											contextModel, // If multiple schemas were possible the schema would be passed here. As is, any element will do.
											(!firstPass || 0 != (stepFlags & ColumnPathStepFlags.IsIdentifier)) ? previousResolvedObjectType : null,
											step.TargetObjectType,
											previousResolvedSupertype,
											false,
											firstPass ? ReferenceModeNamingUse.PrimaryIdentifier : ReferenceModeNamingUse.ReferenceToEntityType, // Ignored if first parameter is null
											generator,
											addPart);
									}
									if (hyphenBoundFormatString != null &&
										(hyphenBoundFormatString.Length - splitPosition) > 1)
									{
										addPart(hyphenBoundFormatString.Substring(splitPosition + 1), null);
									}
								}
							}
							else
							{
								lastStepConsumedNextNode = false;
								lastStepUsedExplicitRoleName = false;
							}
						}
						previousPreviousResolvedSupertype = previousResolvedSupertype;
						previousPreviousResolvedObjectType = previousResolvedObjectType;
						previousResolvedSupertype = (0 != (step.Flags & ColumnPathStepFlags.ReverseAssimilation)) ? step.ResolvedSupertypeVerifyPreferred :  step.ResolvedSupertype;
						previousResolvedObjectType = step.ResolvedObjectTypeVerifyPreferred;
						currentNode = nextLoopNode;
						firstPass = false;
					} while (currentNode != null);
					string finalName = NamePart.GetFinalName(singleName, nameCollection, generator, contextModel);
					if (string.IsNullOrEmpty(finalName))
					{
						return (phase == 0) ? "COLUMN" : null;
					}
					return finalName;
				}
				private string GenerateConstraintName(Constraint constraint, int phase)
				{
					if (phase > 1)
					{
						return null;
					}
					// UNDONE: The PK, UC, FK suffixes should have generator options to control them
					ReferenceConstraint refConstraint;
					UniquenessConstraint uniquenessConstraint;
					if (null != (uniquenessConstraint = constraint as UniquenessConstraint))
					{
						// If we map to a single constraint in the ORM model and that constraint
						// name is not a default generated name, then use that name.
						Uniqueness uniqueness = UniquenessConstraintIsForUniqueness.GetUniqueness(uniquenessConstraint);
						if (uniqueness != null)
						{
							ORMUniquenessConstraint ormUniqueness = UniquenessIsForUniquenessConstraint.GetUniquenessConstraint(uniqueness);
							if (ormUniqueness != null)
							{
								string currentName = ormUniqueness.Name;
								string defaultNamePattern = ((IDefaultNamePattern)ormUniqueness).DefaultNamePattern;
								if (!(Utility.IsNumberDecoratedName(currentName, string.IsNullOrEmpty(defaultNamePattern) ? TypeDescriptor.GetClassName(ormUniqueness) : defaultNamePattern)))
								{
									return (phase == 1 ? uniquenessConstraint.Table.Name + "_" : "") + currentName + (uniquenessConstraint.IsPrimary ? "_PK" : "_UC");
								}
							}
						}
						if (phase != 0)
						{
							return null;
						}
						return uniquenessConstraint.Table.Name + (uniquenessConstraint.IsPrimary ? "_PK" : "_UC");
					}
					if (phase != 0)
					{
						return null;
					}
					if (null != (refConstraint = constraint as ReferenceConstraint))
					{
						return refConstraint.SourceTable.Name + "_FK";
					}
					return constraint.Name;
				}
				#endregion // Name generation methods
				#region Column analysis
				/// <summary>
				/// Flag values for <see cref="ColumnPathStep"/> elements
				/// </summary>
				[Flags]
				private enum ColumnPathStepFlags
				{
					/// <summary>
					/// Flags not set
					/// </summary>
					None = 0,
					/// <summary>
					/// An assimilation that has not been separated or partitioned
					/// </summary>
					ForwardAssimilation = 1,
					/// <summary>
					/// An assimilation that has been separated or partitioned
					/// </summary>
					ReverseAssimilation = 2,
					/// <summary>
					/// A FactType that is part of an identifier
					/// </summary>
					IsIdentifier = 4,
					/// <summary>
					/// The step is passed the identification stage
					/// </summary>
					PassedIdentifier = 8,
					/// <summary>
					/// This step needs special name generation
					/// </summary>
					RequiresDecoration = 0x10,
					/// <summary>
					/// Use an objectified <see cref="FactType"/> directly
					/// instead of resolve a proxy role to get the implied
					/// <see cref="FactType"/>. Used in cases of unary and
					/// one-to-many objectification.
					/// </summary>
					ObjectifiedFactType = 0x20,
					/// <summary>
					/// An assimilation corresponds to a subtype relationship
					/// </summary>
					AssimilationIsSubtype = 0x40,
					/// <summary>
					/// An assimilation step was processed as a FactType
					/// </summary>
					DeclinedAssimilation = 0x80,
					/// <summary>
					/// One or more steps in the subtype chain are not identifying
					/// </summary>
					NonPreferredSubtype = 0x100,
					/// <summary>
					/// Track if the assimilation is points towards
					/// the subtype. Maintained separately from the
					/// forward/reverse notions.
					/// </summary>
					AssimilationTowardsSubtype = 0x200,
				}
				[DebuggerDisplay("{System.String.Concat(ObjectType.Name, (FromRole != null) ? System.String.Concat(\", \", FromRole.FactType.Name) : \"\", \" Flags=\", Flags.ToString(\"g\"))}")]
				private struct ColumnPathStep
				{
					private ColumnPathStepFlags myFlags;
					private ObjectType myObjectType;
					private ObjectType myAlternateObjectType;
					private Role myFromRole;
					public ColumnPathStep(Role fromRole, ObjectType targetObjectType, ObjectType alternateObjectType, ColumnPathStepFlags flags)
					{
						myFromRole = fromRole;
						myObjectType = targetObjectType;
						myAlternateObjectType = alternateObjectType;
						myFlags = flags;
					}
					public Role FromRole
					{
						get
						{
							return myFromRole;
						}
					}
					public ObjectType ObjectType
					{
						get
						{
							return myObjectType;
						}
					}
					/// <summary>
					/// The object type we're moving towards with during this step
					/// </summary>
					public ObjectType TargetObjectType
					{
						get
						{
							return (0 == (myFlags & ColumnPathStepFlags.AssimilationTowardsSubtype)) ? myObjectType : myAlternateObjectType;
						}
					}
					/// <summary>
					/// The object type we're starting with during this step
					/// </summary>
					public ObjectType StartingObjectType
					{
						get
						{
							return (0 == (myFlags & ColumnPathStepFlags.AssimilationTowardsSubtype)) ? myAlternateObjectType : myObjectType;
						}
					}
					/// <summary>
					/// Return the resolved subtype without verifying preferred identification
					/// </summary>
					public ObjectType ResolvedObjectType
					{
						get
						{
							ObjectType standard = myObjectType;
							ObjectType alternate = myAlternateObjectType;
							if (alternate == null || alternate == standard)
							{
								return standard;
							}
							ColumnPathStepFlags flags = myFlags;
							if (0 != (flags & ColumnPathStepFlags.AssimilationTowardsSubtype))
							{
								return alternate;
							}
							if (0 != (flags & ColumnPathStepFlags.DeclinedAssimilation))
							{
								return alternate;
							}
							return standard;
						}
					}
					/// <summary>
					/// Get the resolved object type element. If steps between the
					/// supertype and subtype are not preferred, this returns the subtype.
					/// properties.
					/// </summary>
					public ObjectType ResolvedObjectTypeVerifyPreferred
					{
						get
						{
							ObjectType standard = myObjectType;
							ObjectType alternate = myAlternateObjectType;
							if (alternate == null || alternate == standard)
							{
								return standard;
							}
							ColumnPathStepFlags flags = myFlags;
							if (0 != (flags & (ColumnPathStepFlags.AssimilationTowardsSubtype | ColumnPathStepFlags.NonPreferredSubtype)))
							{
								ObjectType swap = alternate;
								alternate = standard;
								standard = swap;
							}
							if (0 != (flags & (ColumnPathStepFlags.DeclinedAssimilation)))
							{
								return alternate;
							}
							return standard;
						}
					}
					/// <summary>
					/// Get the resolved supertype element. Accessor to correctly interpret
					/// the <see cref="ObjectType"/> and <see cref="AlternateObjectType"/>
					/// properties.
					/// </summary>
					public ObjectType ResolvedSupertype
					{
						get
						{
							ObjectType standard = myObjectType;
							ObjectType alternate = myAlternateObjectType;
							if (alternate == null || alternate == standard)
							{
								return standard;
							}
							ColumnPathStepFlags flags = myFlags;
							if (0 != (flags & ColumnPathStepFlags.AssimilationTowardsSubtype))
							{
								ObjectType swap = alternate;
								alternate = standard;
								standard = swap;
							}
							if (0 == (flags & (ColumnPathStepFlags.DeclinedAssimilation)))
							{
								return alternate;
							}
							return standard;
						}
					}
					/// <summary>
					/// Get the resolved supertype if the chain between the
					/// supertype and subtype is all preferred. Otherwise, return
					/// the subtype.
					/// </summary>
					public ObjectType ResolvedSupertypeVerifyPreferred
					{
						get
						{
							ObjectType standard = myObjectType;
							ObjectType alternate = myAlternateObjectType;
							if (alternate == null || alternate == standard)
							{
								return standard;
							}
							ColumnPathStepFlags flags = myFlags;
							if (0 != (flags & ColumnPathStepFlags.AssimilationTowardsSubtype))
							{
								if (0 == (flags & ColumnPathStepFlags.NonPreferredSubtype))
								{
									return standard;
								}
								return alternate;
							}
							if (0 == (flags & (ColumnPathStepFlags.DeclinedAssimilation | ColumnPathStepFlags.NonPreferredSubtype)))
							{
								return alternate;
							}
							return standard;
						}
					}
					/// <summary>
					/// An alternate object type, interpreted based on flag settings
					/// </summary>
					public ObjectType AlternateObjectType
					{
						get
						{
							return myAlternateObjectType ?? myObjectType;
						}
					}
					/// <summary>
					///  Current flags settings for this step
					/// </summary>
					public ColumnPathStepFlags Flags
					{
						get
						{
							return myFlags;
						}
						set
						{
							myFlags = value;
						}
					}
					/// <summary>
					/// Test for equality without checking the state of the flags or the resolved supertype
					/// </summary>
					public bool CoreEquals(ColumnPathStep otherStep)
					{
						return myObjectType == otherStep.myObjectType && myFromRole == otherStep.myFromRole;
					}
				}
				private Table myColumnStepTable;
				private Dictionary<Column, LinkedNode<ColumnPathStep>> myColumnSteps;
				private LinkedNode<ColumnPathStep> GetColumnPath(Column column)
				{
					Table table = column.Table;
					if (myColumnStepTable == table)
					{
						return myColumnSteps[column];
					}
					Dictionary<Column, LinkedNode<ColumnPathStep>> columnSteps = myColumnSteps;
					if (myColumnStepTable == null)
					{
						myColumnSteps = columnSteps = new Dictionary<Column, LinkedNode<ColumnPathStep>>();
					}
					else
					{
						columnSteps.Clear();
					}
					myColumnStepTable = table;
#if DEBUGCOLUMNPATH
					Debug.WriteLine("Table: " + table.Name);
					Debug.Indent();
					Debug.Indent();
#endif // DEBUGCOLUMNPATH

					// Seed the cross-cutting dictionary and seed it with the main represented
					// ObjectTypes to force decorations on columns that loop back into this table.
					Dictionary<ObjectType, LinkedNode<LinkedNode<ColumnPathStep>>> objectTypeToSteps = new Dictionary<ObjectType, LinkedNode<LinkedNode<ColumnPathStep>>>();
					ConceptType primaryConceptType = TableIsPrimarilyForConceptType.GetConceptType(table);
					if (primaryConceptType != null)
					{
						ObjectType targetObjectType = ConceptTypeIsForObjectType.GetObjectType(primaryConceptType);
						objectTypeToSteps[targetObjectType] = new LinkedNode<LinkedNode<ColumnPathStep>>(new LinkedNode<ColumnPathStep>(new ColumnPathStep(null, targetObjectType, null, ColumnPathStepFlags.None)));
					}
					foreach (ConceptType secondaryConceptType in TableIsAlsoForConceptType.GetConceptType(table))
					{
						ObjectType targetObjectType = ConceptTypeIsForObjectType.GetObjectType(secondaryConceptType);
						objectTypeToSteps[targetObjectType] = new LinkedNode<LinkedNode<ColumnPathStep>>(new LinkedNode<ColumnPathStep>(new ColumnPathStep(null, targetObjectType, null, ColumnPathStepFlags.None)));
					}
					LinkedElementCollection<Column> columns = table.ColumnCollection;
					int columnCount = columns.Count;
					for (int iColumn = 0; iColumn < columnCount; ++iColumn)
					{
						Column currentColumn = columns[iColumn];
#if DEBUGCOLUMNPATH
						Debug.Unindent();
						Debug.WriteLine("Column: " + currentColumn.Name);
						Debug.Indent();
#endif // DEBUGCOLUMNPATH
						LinkedElementCollection<ConceptTypeChild> childPath = ColumnHasConceptTypeChild.GetConceptTypeChildPath(currentColumn);
						int childPathCount = childPath.Count;
						LinkedNode<ColumnPathStep> headNode = null;
						LinkedNode<ColumnPathStep> tailNode = null;
						bool passedIdentifier = false;
						bool processTailDelayed = false;
						ConceptType nextComingFromConceptType = primaryConceptType;
						Objectification assimilationObjectification = null;
						for (int iChild = 0; iChild < childPathCount; ++iChild)
						{
							ConceptType comingFromConceptType = nextComingFromConceptType;
							ConceptTypeChild child = childPath[iChild];
							ConceptTypeAssimilatesConceptType assimilation = child as ConceptTypeAssimilatesConceptType;
							bool reverseAssimilation = false;
							bool forwardToReverseTransition = false;
							bool towardsSubtype = false;
							if (assimilation != null)
							{
								if (comingFromConceptType == assimilation.Parent)
								{
									nextComingFromConceptType = assimilation.ReferencedConceptType;
								}
								else
								{
									towardsSubtype = true;
									nextComingFromConceptType = assimilation.Parent;
								}
#if DEBUGCOLUMNPATH
								Debug.WriteLine("From: " + comingFromConceptType.Name + " To: " + nextComingFromConceptType.Name + "(" + (assimilation.RefersToSubtype && assimilation.IsPreferredForTarget).ToString() + ")");
#endif // DEBUGCOLUMNPATH
								if (tailNode != null)
								{
									// If we're already moving down an assimilation path in a specific direction then keep
									// going that direction
									ColumnPathStep pathStep = tailNode.Value;
									ColumnPathStepFlags stepFlags = pathStep.Flags;
									if (0 != (stepFlags & ColumnPathStepFlags.ForwardAssimilation))
									{
										// Keep going forward
										if (0 != (stepFlags & ColumnPathStepFlags.AssimilationIsSubtype) &&
											AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation, true) != AssimilationAbsorptionChoice.Absorb &&
											comingFromConceptType == (0 == (stepFlags & ColumnPathStepFlags.AssimilationTowardsSubtype) ? assimilation.AssimilatedConceptType : assimilation.AssimilatorConceptType))
										{
											forwardToReverseTransition = true;
										}
									}
									else if (0 != (stepFlags & ColumnPathStepFlags.ReverseAssimilation))
									{
										// Keep going backward
										reverseAssimilation = true;
									}
									else
									{
										// Figure it out from this step
										reverseAssimilation = AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation, true) != AssimilationAbsorptionChoice.Absorb;
									}
								}
								else
								{
									reverseAssimilation = AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation, true) != AssimilationAbsorptionChoice.Absorb;
								}
							}
							else
							{
								nextComingFromConceptType = child.Target as ConceptType;
#if DEBUGCOLUMNPATH
								Debug.WriteLine("From: " + comingFromConceptType.Name + " To: " + ((nextComingFromConceptType != null) ? nextComingFromConceptType.Name : ((InformationTypeFormat)child.Target).Name));
#endif // DEBUGCOLUMNPATH
							}
							LinkedElementCollection<FactType> factTypes = ConceptTypeChildHasPathFactType.GetPathFactTypeCollection(child);
							int factTypeCount = factTypes.Count;
							for (int iFactType = 0; iFactType < factTypeCount; ++iFactType)
							{
								FactType factType = factTypes[iFactType];
								Role targetRole = FactTypeMapsTowardsRole.GetTowardsRole(factType).Role;
								ColumnPathStepFlags flags = passedIdentifier ? ColumnPathStepFlags.PassedIdentifier : 0;
								ColumnPathStep pathStep = default(ColumnPathStep);
								bool processPreviousTail = false;
								Objectification previousAssimilationObjectification = assimilationObjectification;
								assimilationObjectification = null;
								bool processAsFactType = true;
								if (assimilation != null)
								{
									Role nonAssimilationTargetRole = targetRole;
									if (!towardsSubtype)
									{
										targetRole = targetRole.OppositeRoleAlwaysResolveProxy.Role;
									}
									processAsFactType = false;
									Objectification objectification;
									bool assimilationIsSubtype = assimilation.RefersToSubtype;
									bool secondarySubtype = assimilationIsSubtype && !assimilation.IsPreferredForTarget;
									if (!assimilationIsSubtype &&
										null != (objectification = factType.ImpliedByObjectification) &&
										objectification.NestingType == targetRole.RolePlayer)
									{
										assimilationObjectification = objectification;
									}
									if (tailNode != null)
									{
										pathStep = tailNode.Value;
										ColumnPathStepFlags tailFlags = pathStep.Flags;
										if (0 != (tailFlags & (ColumnPathStepFlags.ForwardAssimilation | ColumnPathStepFlags.ReverseAssimilation)))
										{
											bool tailIsSubtype = 0 != (tailFlags & ColumnPathStepFlags.AssimilationIsSubtype);
											if (tailIsSubtype && assimilationIsSubtype)
											{
												if (forwardToReverseTransition)
												{
													flags |= ColumnPathStepFlags.DeclinedAssimilation;
													targetRole = towardsSubtype ? targetRole.OppositeRoleAlwaysResolveProxy.Role : nonAssimilationTargetRole;
													processAsFactType = true;
												}
												else
												{
													if (secondarySubtype && 0 == (tailFlags & ColumnPathStepFlags.NonPreferredSubtype))
													{
														tailNode.Value = new ColumnPathStep(pathStep.FromRole, pathStep.ObjectType, pathStep.AlternateObjectType, tailFlags | ColumnPathStepFlags.NonPreferredSubtype);
													}
													// If this is a subtype chain, then keep going, using the first
													// subtype in the chain as a node used in the final name.
													continue;
												}
											}
											else if (assimilationObjectification != null)
											{
												// The type of assimilation has changed, but we have an objectifying object type,
												// so we treat it like a separate link in the chain, or the previous element was
												// also not a subtype.
												tailNode.Value = new ColumnPathStep(pathStep.FromRole, pathStep.ObjectType, nonAssimilationTargetRole.RolePlayer, pathStep.Flags);
												processPreviousTail = processTailDelayed;
											}
											else
											{
												flags |= ColumnPathStepFlags.DeclinedAssimilation;
												targetRole = towardsSubtype ? targetRole.OppositeRoleAlwaysResolveProxy.Role : nonAssimilationTargetRole;
												processAsFactType = true;
											}
										}
									}
									else if (!assimilationIsSubtype && assimilationObjectification == null)
									{
										flags |= ColumnPathStepFlags.DeclinedAssimilation;
										targetRole = towardsSubtype ? targetRole.OppositeRoleAlwaysResolveProxy.Role : nonAssimilationTargetRole;
										processAsFactType = true;
									}
									if (!processAsFactType)
									{
										if (reverseAssimilation)
										{
											flags |= ColumnPathStepFlags.ReverseAssimilation;
										}
										else
										{
											flags |= ColumnPathStepFlags.ForwardAssimilation;
										}
										if (assimilationIsSubtype)
										{
											flags |= ColumnPathStepFlags.AssimilationIsSubtype;
											if (secondarySubtype)
											{
												flags |= ColumnPathStepFlags.NonPreferredSubtype;
											}
											if (towardsSubtype)
											{
												flags |= ColumnPathStepFlags.AssimilationTowardsSubtype;
											}
										}
										pathStep = new ColumnPathStep(
											null,
											towardsSubtype ? ConceptTypeIsForObjectType.GetObjectType(comingFromConceptType) : targetRole.RolePlayer,
											towardsSubtype ? targetRole.RolePlayer : null,
											flags);
										processTailDelayed = true;
									}
								}
								if (processAsFactType)
								{
									bool haveStep = false;
									if (tailNode != null)
									{
										pathStep = tailNode.Value;
										ColumnPathStepFlags tailFlags = pathStep.Flags;
										if (0 != (tailFlags & (ColumnPathStepFlags.ForwardAssimilation | ColumnPathStepFlags.ReverseAssimilation)))
										{
											RoleProxy oppositeProxy;
											Role objectifiedResolvedProxyRole;
											Role objectifiedOppositeRole;
											if (previousAssimilationObjectification != null &&
												factType.ImpliedByObjectification == previousAssimilationObjectification &&
												null != (oppositeProxy = targetRole.OppositeRole as RoleProxy) &&
												null != (objectifiedOppositeRole = (objectifiedResolvedProxyRole = oppositeProxy.Role).OppositeRole as Role))
											{
												flags |= ColumnPathStepFlags.ObjectifiedFactType;
												// Replace both factTypes with the original unobjectified FactType
												ObjectType fromObjectType = objectifiedOppositeRole.RolePlayer;
												if (pathStep.ObjectType == previousAssimilationObjectification.NestingType)
												{
													// Trivial path leading in, remove the subtype completely
													processTailDelayed = false;
													tailNode.Value = new ColumnPathStep(objectifiedOppositeRole, objectifiedResolvedProxyRole.RolePlayer, null, flags);
													ProcessTailNode(tailNode, objectTypeToSteps);
													continue;
												}
												tailNode.Value = new ColumnPathStep(pathStep.FromRole, pathStep.ObjectType, fromObjectType, pathStep.Flags);
												pathStep = new ColumnPathStep(objectifiedOppositeRole, objectifiedResolvedProxyRole.RolePlayer, null, flags);
												haveStep = true;
											}
											else
											{
												// Add a resolved supertype to the forward subtype to
												// allow later steps to be compared to this one.
												tailNode.Value = new ColumnPathStep(pathStep.FromRole, pathStep.ObjectType, targetRole.RolePlayer, pathStep.Flags);
											}
										}
										else if (!processTailDelayed && 0 != (tailFlags & ColumnPathStepFlags.DeclinedAssimilation))
										{
											// Add a resolved supertype to the previous step
											tailNode.Value = new ColumnPathStep(pathStep.FromRole, pathStep.ObjectType, targetRole.RolePlayer, pathStep.Flags);
										}
									}
									if (!haveStep)
									{
										Role oppositeRole = targetRole.OppositeRoleAlwaysResolveProxy.Role;
										ORMUniquenessConstraint pid = targetRole.RolePlayer.PreferredIdentifier;
										if (pid != null && pid.RoleCollection.Contains(oppositeRole))
										{
											flags |= ColumnPathStepFlags.IsIdentifier;
											passedIdentifier = true;
										}
										pathStep = new ColumnPathStep(targetRole, oppositeRole.RolePlayer, null, flags);
									}
									processPreviousTail = processTailDelayed;
									processTailDelayed = false;
								}
								if (processPreviousTail && tailNode != null)
								{
									ProcessTailNode(tailNode, objectTypeToSteps);
								}
								LinkedNode<ColumnPathStep> newNode = new LinkedNode<ColumnPathStep>(pathStep);
								if (tailNode == null)
								{
									headNode = tailNode = newNode;
								}
								else
								{
									tailNode.SetNext(newNode, ref headNode);
									tailNode = newNode;
								}
								if (!processTailDelayed)
								{
									ProcessTailNode(tailNode, objectTypeToSteps);
								}
							}
						}
						if (processTailDelayed)
						{
							ProcessTailNode(tailNode, objectTypeToSteps);
						}
						columnSteps.Add(currentColumn, headNode);
					}
#if DEBUGCOLUMNPATH
					Debug.Unindent();
					Debug.Unindent();
#endif // DEBUGCOLUMNPATH
					return columnSteps[column];
				}
				private static void ProcessTailNode(LinkedNode<ColumnPathStep> tailNode, Dictionary<ObjectType, LinkedNode<LinkedNode<ColumnPathStep>>> objectTypeToSteps)
				{
					ColumnPathStep step = tailNode.Value;
					ObjectType objectType = step.TargetObjectType;
					Role fromRole = step.FromRole;
					LinkedNode<LinkedNode<ColumnPathStep>> existingNodesHead;
					LinkedNode<LinkedNode<ColumnPathStep>> newNode = new LinkedNode<LinkedNode<ColumnPathStep>>(tailNode);
					if (objectTypeToSteps.TryGetValue(objectType, out existingNodesHead))
					{
						LinkedNode<LinkedNode<ColumnPathStep>> crossCutNode = existingNodesHead;
						do
						{
							LinkedNode<ColumnPathStep> pathNode = crossCutNode.Value;
							ColumnPathStep currentStep = pathNode.Value;
							if (currentStep.FromRole != fromRole)
							{
								// If there is any path divergence coming into the ObjectType
								// then all paths into the node need decorating directly, or
								// an early node needs decorating if this is part of the preferred
								// identifier of the earlier node but not the current one.
								bool decorateStep = false;
								bool decorateStepPathNonIdentifier = false;
								LinkedNode<ColumnPathStep> stepPathNonIdentifierNode = GetPreviousNonIdentifierNode(tailNode);
								LinkedNode<LinkedNode<ColumnPathStep>> crossCutNode2 = existingNodesHead;
								do
								{
									LinkedNode<ColumnPathStep> pathNode2 = crossCutNode2.Value;
									LinkedNode<ColumnPathStep> pathNonIdentifierNode = GetPreviousNonIdentifierNode(pathNode2);
									if (pathNonIdentifierNode != stepPathNonIdentifierNode &&
										((pathNonIdentifierNode == null ^ stepPathNonIdentifierNode == null) ||
										(!pathNonIdentifierNode.Value.CoreEquals(stepPathNonIdentifierNode.Value))))
									{
										if (pathNonIdentifierNode != null)
										{
											if (pathNode2 != pathNonIdentifierNode)
											{
												pathNode2 = null;
												DecorateCrossCutNonIdentifiers(objectTypeToSteps[pathNonIdentifierNode.Value.ObjectType]);
											}
										}
										decorateStepPathNonIdentifier = true;
									}
									else
									{
										decorateStep = true;
									}
									if (pathNode2 != null)
									{
										ColumnPathStep pathStep2 = pathNode2.Value;
										if (0 == (pathStep2.Flags & ColumnPathStepFlags.RequiresDecoration))
										{
											pathStep2.Flags |= ColumnPathStepFlags.RequiresDecoration;
											pathNode2.Value = pathStep2;
										}
									}
									crossCutNode2 = crossCutNode2.Next;
								} while (crossCutNode2 != null);
								if (decorateStepPathNonIdentifier && stepPathNonIdentifierNode != null)
								{
									if (stepPathNonIdentifierNode == tailNode)
									{
										decorateStep = true;
									}
									else
									{
										DecorateCrossCutNonIdentifiers(objectTypeToSteps[stepPathNonIdentifierNode.Value.ObjectType]);
									}
								}
								if (decorateStep)
								{
									step.Flags |= ColumnPathStepFlags.RequiresDecoration;
									tailNode.Value = step;
								}
								break;
							}
							crossCutNode = crossCutNode.Next;
						} while (crossCutNode != null);
						newNode.SetNext(existingNodesHead, ref newNode);
					}
					objectTypeToSteps[objectType] = newNode;
				}
				/// <summary>
				/// Helper for <see cref="ProcessTailNode"/>
				/// </summary>
				private static void DecorateCrossCutNonIdentifiers(LinkedNode<LinkedNode<ColumnPathStep>> nonIdentifierCrossCutNode)
				{
					do
					{
						LinkedNode<ColumnPathStep> nonIdentifierNode = nonIdentifierCrossCutNode.Value;
						ColumnPathStep nonIdentifierStep = nonIdentifierNode.Value;
						ColumnPathStepFlags nonIdentifierStepFlags = nonIdentifierStep.Flags;
						if (0 == (nonIdentifierStepFlags & (ColumnPathStepFlags.IsIdentifier | ColumnPathStepFlags.PassedIdentifier | ColumnPathStepFlags.RequiresDecoration)))
						{
							nonIdentifierStep.Flags |= ColumnPathStepFlags.RequiresDecoration;
							nonIdentifierNode.Value = nonIdentifierStep;
						}
						nonIdentifierCrossCutNode = nonIdentifierCrossCutNode.Next;
					} while (nonIdentifierCrossCutNode != null);
				}
				/// <summary>
				/// Helper for <see cref="ProcessTailNode"/>
				/// </summary>
				private static LinkedNode<ColumnPathStep> GetPreviousNonIdentifierNode(LinkedNode<ColumnPathStep> stepNode)
				{
					LinkedNode<ColumnPathStep> currentNode = stepNode;
					do
					{
						ColumnPathStepFlags flags = currentNode.Value.Flags;
						if (0 == (flags & (ColumnPathStepFlags.IsIdentifier | ColumnPathStepFlags.PassedIdentifier)))
						{
							return currentNode;
						}
						currentNode = currentNode.Previous;
					} while (currentNode != null);
					return null;
				}
				#endregion // Column analysis
			}
			#endregion // DefaultDatabaseNameGenerator class
		}
	}
	#endregion // ORMAbstractionToConceptualDatabaseBridgeDomainModel.NameGeneration class
	#region ORMAbstractionToConceptualDatabaseBridgeDomainModel.RelationalNameGenerator Class
	partial class RelationalNameGenerator
	{
		/// <summary>
		/// Specify properties that are initialized with modified default properties.
		/// </summary>
		protected override StandardNameGeneratorProperty AlternateDefaultStandardProperties
		{
			get
			{
				return string.IsNullOrEmpty(NameUsage) ? StandardNameGeneratorProperty.SpacingFormat : StandardNameGeneratorProperty.CasingOption;
			}
		}
		/// <summary>
		/// Provide values for alternate defaults of a fully constructed instance.
		/// </summary>
		protected override object GetAlternateDefaultStandardPropertyValue(StandardNameGeneratorProperty property)
		{
			if (property == StandardNameGeneratorProperty.SpacingFormat)
			{
				return NameGeneratorSpacingFormat.Remove;
			}
			else if (property == StandardNameGeneratorProperty.CasingOption)
			{
				return NameUsage == "RelationalColumn" ? NameGeneratorCasingOption.Camel : NameGeneratorCasingOption.Pascal;
			}
			return null;
		}
	}
	#endregion // ORMAbstractionToConceptualDatabaseBridgeDomainModel.RelationalNameGenerator Class
}
