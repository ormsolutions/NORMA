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
using System.Collections.Generic;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge;
using ORMSolutions.ORMArchitect.ORMAbstraction;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase;
using System.Collections.ObjectModel;
using System.Diagnostics;
using ORMCore = ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge
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
		#region Algorithm Version Constants
		/// <summary>
		/// The algorithm version written to the file for the core algorithm
		/// </summary>
		public const string CurrentCoreAlgorithmVersion = "1.004";
		/// <summary>
		/// The algorithm version written to the file for the name generation algorithm
		/// </summary>
		public const string CurrentNameAlgorithmVersion = "1.012";
		#endregion // Algorithm Version Constants
		#region Fully populate from OIAL

		#region PathStack class
		/// <summary>
		/// A LIFO (last in, first out) stack that is enumerated in FIFO (first in, first out) order.
		/// </summary>
		[Serializable]
		private sealed class PathStack<T> : List<T>
			where T : class
		{
			public PathStack()
				: base()
			{
			}
			public PathStack(int initialCapacity)
				: base(initialCapacity)
			{
			}

			public void Push(T item)
			{
				this.Add(item);
			}
			public void Pop()
			{
				this.RemoveAt(this.Count - 1);
			}
			public void PopAndAssert(T expectedItem)
			{
				int lastIndex = this.Count - 1;
				System.Diagnostics.Debug.Assert(this[lastIndex] == expectedItem);
				this.RemoveAt(lastIndex);
			}
		}
		#endregion // PathStack class

		#region CastEnumerator struct
		/// <summary>
		/// An enumerator wrapper that casts each item to a more derived type.
		/// </summary>
		[Serializable]
		private struct CastEnumerator<TInput, TInputEnumerator, TOutput> : IEnumerable<TOutput>, IEnumerator<TOutput>
			where TInputEnumerator : struct, IEnumerator<TInput>
			where TOutput : TInput
		{
			private TInputEnumerator _enumerator;
			public CastEnumerator(TInputEnumerator enumerator)
			{
				this._enumerator = enumerator;
			}
			public CastEnumerator<TInput, TInputEnumerator, TOutput> GetEnumerator()
			{
				CastEnumerator<TInput, TInputEnumerator, TOutput> enumerator = this;
				enumerator.Reset();
				return enumerator;
			}
			IEnumerator<TOutput> IEnumerable<TOutput>.GetEnumerator()
			{
				return this.GetEnumerator();
			}
			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
			public void Dispose()
			{
				this._enumerator.Dispose();
			}
			public bool MoveNext()
			{
				return this._enumerator.MoveNext();
			}
			public void Reset()
			{
				this._enumerator.Reset();
			}
			public TOutput Current
			{
				get
				{
					return (TOutput)this._enumerator.Current;
				}
			}
			object System.Collections.IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}
		}
		#endregion // CastEnumerable struct


		// UNDONE: OTHER_ASSIMILATION_MAPPING: Additional assimilation mappings (e.g. "reverse absorb") will likely
		// be added in the future. Some of the areas of the code that will need to change to support this are marked
		// with the comment "UNDONE: OTHER_ASSIMILATION_MAPPING".


		/// <summary>
		/// Generates the conceptual database model for a given Schema and AbstractionModel.
		/// The generation of the Conceptual Database Model includes the population of all tables with
		/// Columns, Uniquenesses, Foreign Keys, and Mandatory restrictions.
		/// </summary>
		private static void FullyGenerateConceptualDatabaseModel(Schema schema, AbstractionModel sourceModel, INotifyElementAdded notifyAdded)
		{
			LinkedElementCollection<Table> tables = schema.TableCollection;
			LinkedElementCollection<ConceptType> conceptTypes = sourceModel.ConceptTypeCollection;
			Partition partition = schema.Partition;

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

			// Generate a table for each concept type that needs one.
			foreach (ConceptType conceptType in conceptTypes)
			{
				bool needsTable = true;
				foreach (ConceptTypeAssimilatesConceptType assimilation
					in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType))
				{
					if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation) == AssimilationAbsorptionChoice.Absorb)
					{
						// If we find any absorb assimilation that this concept type is the target of, we don't need a table.
						needsTable = false;
						break;
					}
				}
				if (needsTable)
				{
					foreach (ConceptTypeAssimilatesConceptType assimilation
						in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(conceptType))
					{
						if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation) == AssimilationAbsorptionChoice.Partition)
						{
							// If we find any partition assimilation that this concept type is the parent of, we don't need a table.
							needsTable = false;
							break;
						}
					}
					if (needsTable)
					{
						// This concept type was neither absorbed by any parent nor partitioned into any target, so we need to
						// generate a table for it.
						Table table = new Table(partition, new PropertyAssignment(Table.NameDomainPropertyId, conceptType.Name));

						TableIsPrimarilyForConceptType primarilyForLink = new TableIsPrimarilyForConceptType(table, conceptType);

						tables.Add(table);
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(table, true);
						}
					}
				}
			}

			PathStack<ConceptTypeChild> conceptTypeChildPath = new PathStack<ConceptTypeChild>();
			// Create the columns and uniqueness constraints for each table based on the concept type children
			// of the concept type that the table is primarily for.
			foreach (Table table in tables)
			{
				LinkedElementCollection<Column> tableColumns = table.ColumnCollection;
				LinkedElementCollection<UniquenessConstraint> tableUniquenessConstraints = table.UniquenessConstraintCollection;
				ConceptType conceptType = TableIsPrimarilyForConceptType.GetConceptType(table);

				GenerateContentForConceptTypeChildren(table, tableColumns, tableUniquenessConstraints, conceptTypeChildPath, conceptType, true, true);

				Debug.Assert(conceptTypeChildPath.Count == 0,
					"The path stack should be empty after processing a concept type.");

				if (notifyAdded != null)
				{
					foreach (Column column in tableColumns)
					{
						notifyAdded.ElementAdded(column, true);
					}
					foreach (UniquenessConstraint uniquenessConstraint in tableUniquenessConstraints)
					{
						notifyAdded.ElementAdded(uniquenessConstraint, true);
					}
				}
			}

			// Create any reference constraints (foreign keys) for concept type relations and
			// separated concept type assimilations.
			foreach (Table table in tables)
			{
				ConceptType conceptType = TableIsPrimarilyForConceptType.GetConceptType(table);

				GenerateReferenceConstraintsForConceptTypeChildren(table, conceptTypeChildPath, conceptType);

				Debug.Assert(conceptTypeChildPath.Count == 0,
					"The path stack should be empty after processing a concept type.");

				if (notifyAdded != null)
				{
					foreach (ReferenceConstraint referenceConstraint in table.ReferenceConstraintCollection)
					{
						notifyAdded.ElementAdded(referenceConstraint, true);
						foreach (ColumnReference columnReference in referenceConstraint.ColumnReferenceCollection)
						{
							notifyAdded.ElementAdded(columnReference, false);
						}
					}
				}
			}

			// Change all names to a more appropriate version.
			NameGeneration.GenerateAllNames(schema);
		}

		/// <summary>
		/// Indicates whether <paramref name="assimilation"/> maps to the <see cref="Table"/>
		/// for <see cref="ConceptTypeAssimilatesConceptType.AssimilatorConceptType"/>.
		/// </summary>
		private static bool SeparateConceptTypeAssimilationMapsToParentConceptType(ConceptTypeAssimilatesConceptType assimilation)
		{
			// UNDONE: We currently map 'separate' assimilations to the parent table only if they provide the
			// preferred identifier for the parent, but in the future we may map them there for other reasons
			// as well.

			// UNDONE: We may also want to consider the path prior to reaching this assimilation. If this
			// wouldn't actually end up being the primary key, then we don't necessarily need to include
			// it in the parent table rather than the target table.
			return assimilation.IsPreferredForParent;
		}

		#region Column and UniquenessConstraint generation methods
		/// <summary>
		/// Generates a new <see cref="Column"/> in <paramref name="tableColumns"/> for <paramref name="informationType"/>.
		/// </summary>
		/// <param name="tableColumns">
		/// The <see cref="LinkedElementCollection{Column}"/> to which the new <see cref="Column"/> should be added.
		/// </param>
		/// <param name="conceptTypeChildPath">
		/// The path taken from the <see cref="ReadOnlyLinkedElementCollection{Column}.SourceElement"/> of
		/// <paramref name="tableColumns"/> to reach <paramref name="informationType"/> (not including
		/// <paramref name="informationType"/> itself).
		/// </param>
		/// <param name="informationType">
		/// The <see cref="InformationType"/> for which a new <see cref="Column"/> should be generated.
		/// </param>
		/// <param name="isMandatorySoFar">
		/// Indicates whether all of the steps in <paramref name="conceptTypeChildPath"/> so far are mandatory
		/// (not including <paramref name="informationType"/> itself).
		/// </param>
		private static Column GenerateColumnForInformationType(LinkedElementCollection<Column> tableColumns, PathStack<ConceptTypeChild> conceptTypeChildPath, InformationType informationType, bool isMandatorySoFar)
		{
			conceptTypeChildPath.Push(informationType);

			Column column = new Column(informationType.Partition,
				new PropertyAssignment(Column.NameDomainPropertyId, informationType.Name),
				new PropertyAssignment(Column.IsNullableDomainPropertyId, !(isMandatorySoFar && informationType.IsMandatory)));
			tableColumns.Add(column);

			ColumnHasConceptTypeChild.GetConceptTypeChildPath(column).AddRange(conceptTypeChildPath);

			conceptTypeChildPath.Pop(); // We don't need to assert here since we're not recursing.

			return column;
		}

		/// <summary>
		/// Generates new <see cref="Column">Columns</see> and <see cref="UniquenessConstraint">UniquenessConstraints</see>
		/// in <paramref name="table"/> for all of the appropriate <see cref="ConceptTypeChild">ConceptTypeChildren</see>
		/// that involve <paramref name="conceptType"/>.
		/// </summary>
		/// <param name="table">
		/// The <see cref="Table"/> for which new <see cref="Column">Columns</see> and
		/// <see cref="UniquenessConstraint">UniquenessConstraints</see> should be generated.
		/// </param>
		/// <param name="tableColumns">
		/// The <see cref="LinkedElementCollection{Column}"/> to which the new <see cref="Column">Columns</see> should
		/// be added. <paramref name="table"/> must be the <see cref="ReadOnlyLinkedElementCollection{Column}.SourceElement"/>
		/// of this <see cref="LinkedElementCollection{Column}"/>.
		/// </param>
		/// <param name="tableUniquenessConstraints">
		/// The <see cref="LinkedElementCollection{UniquenessConstraint}"/> to which the new
		/// <see cref="UniquenessConstraint">UniquenessConstraints</see> should be added. <paramref name="table"/> must be the
		/// <see cref="ReadOnlyLinkedElementCollection{UniquenessConstraint}.SourceElement"/> of this
		/// <see cref="LinkedElementCollection{UniquenessConstraint}"/>.
		/// </param>
		/// <param name="conceptTypeChildPath">
		/// The path taken from <paramref name="table"/> to reach <paramref name="conceptType"/>. The last path step
		/// must be a <see cref="ConceptTypeChild"/> that reaches <paramref name="conceptType"/>, unless
		/// <paramref name="table"/> <see cref="TableIsPrimarilyForConceptType">is primarily for</see>
		/// <paramref name="conceptType"/>, in which case this path must be empty.
		/// </param>
		/// <param name="conceptType">
		/// The <see cref="ConceptType"/> for which new <see cref="Column">Columns</see> and
		/// <see cref="UniquenessConstraint">UniquenessConstraints</see> should be generated.
		/// </param>
		/// <param name="isMandatorySoFar">
		/// Indicates whether all of the steps in <paramref name="conceptTypeChildPath"/> so far are mandatory,
		/// including the last path step that reaches <paramref name="conceptType"/>. If <paramref name="table"/>
		/// <see cref="TableIsPrimarilyForConceptType">is primarily for</see> <paramref name="conceptType"/> (and
		/// therefore <paramref name="conceptTypeChildPath"/> is empty), this parameter must be <see langword="true"/>.
		/// </param>
		/// <param name="isPreferredSoFar">
		/// Indicates whether all of the steps in <paramref name="conceptTypeChildPath"/> so far are part of the
		/// preferred identifier of <paramref name="table"/>, including the last path step that reaches
		/// <paramref name="conceptType"/>. If <paramref name="table"/> <see cref="TableIsPrimarilyForConceptType">
		/// is primarily for</see> <paramref name="conceptType"/> (and therefore <paramref name="conceptTypeChildPath"/>
		/// is empty), this parameter must be <see langword="true"/>.
		/// </param>
		private static void GenerateContentForConceptTypeChildren(Table table, LinkedElementCollection<Column> tableColumns, LinkedElementCollection<UniquenessConstraint> tableUniquenessConstraints, PathStack<ConceptTypeChild> conceptTypeChildPath, ConceptType conceptType, bool isMandatorySoFar, bool isPreferredSoFar)
		{
			Dictionary<ConceptTypeChild, List<Column>> newColumnsByConceptTypeChild = new Dictionary<ConceptTypeChild, List<Column>>();

			// Make a column for each information type.
			foreach (InformationType informationType in InformationType.GetLinksToInformationTypeFormatCollection(conceptType))
			{
				Column newColumn = GenerateColumnForInformationType(tableColumns, conceptTypeChildPath, informationType, isMandatorySoFar);
				List<Column> newColumns = new List<Column>(1);
				newColumns.Add(newColumn);
				newColumnsByConceptTypeChild.Add(informationType, newColumns);
			}

			// Make columns for the preferred identifier of each related concept type.
			foreach (ConceptTypeRelatesToConceptType relation
				in ConceptTypeRelatesToConceptType.GetLinksToRelatedConceptTypeCollection(conceptType))
			{
				conceptTypeChildPath.Push(relation);

				List<Column> newColumns = GenerateColumnsForConceptTypePreferredIdentifier(
					table, tableColumns, conceptTypeChildPath,
					relation.RelatedConceptType,
					isMandatorySoFar && relation.IsMandatory);
				newColumnsByConceptTypeChild.Add(relation, newColumns);

				conceptTypeChildPath.PopAndAssert(relation);
			}

			// Map uniqueness constraints on any information types and/or concept type relations.
			foreach (Uniqueness oialUniquenessConstraint in conceptType.UniquenessCollection)
			{
				UniquenessConstraint newUniquenessConstraint = new UniquenessConstraint(table.Partition,
					new PropertyAssignment(UniquenessConstraint.NameDomainPropertyId, oialUniquenessConstraint.Name),
					new PropertyAssignment(UniquenessConstraint.IsPrimaryDomainPropertyId, isPreferredSoFar && oialUniquenessConstraint.IsPreferred));

				tableUniquenessConstraints.Add(newUniquenessConstraint);
				UniquenessConstraintIsForUniqueness uniquenessConstraintIsForUniqueness =
					new UniquenessConstraintIsForUniqueness(newUniquenessConstraint, oialUniquenessConstraint);

				LinkedElementCollection<Column> newUniquenessConstraintColumns = newUniquenessConstraint.ColumnCollection;
				foreach (ConceptTypeChild conceptTypeChild in oialUniquenessConstraint.ConceptTypeChildCollection)
				{
					newUniquenessConstraintColumns.AddRange(newColumnsByConceptTypeChild[conceptTypeChild]);
				}
			}

			// Deal with the assimilations for which the current concept type is the parent.
			foreach (ConceptTypeAssimilatesConceptType assimilation
				in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(conceptType))
			{
				conceptTypeChildPath.Push(assimilation);
				switch (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation))
				{
					case AssimilationAbsorptionChoice.Absorb:
						if (TableIsAlsoForConceptType.GetLink(table, assimilation.AssimilatedConceptType) != null)
						{
							// There is already a relationship between this concept type and table, meaning
							// that we already reached this concept type via another assimilation path.
							break;
						}
						TableIsAlsoForConceptType tableIsAlsoForConceptType =
							new TableIsAlsoForConceptType(table, assimilation.AssimilatedConceptType);

						// The conceptTypeChildPath is also the assimilation path: We will get
						// here only if everything before this point is an assimilation, since
						// this method is recursively called only for absorb and partition.
						tableIsAlsoForConceptType.AssimilationPath.AddRange(
							new CastEnumerator<ConceptTypeChild, PathStack<ConceptTypeChild>.Enumerator,
								ConceptTypeAssimilatesConceptType>(conceptTypeChildPath.GetEnumerator()));

						GenerateContentForConceptTypeChildren(table, tableColumns,
							tableUniquenessConstraints,	conceptTypeChildPath,
							assimilation.AssimilatedConceptType,
							isMandatorySoFar && assimilation.IsMandatory,
							isPreferredSoFar && assimilation.IsPreferredForParent);
						break;

					case AssimilationAbsorptionChoice.Partition:
						// We don't need to do anything for this case,
						// since it is mapping away from the current concept type.
						break;

					case AssimilationAbsorptionChoice.Separate:
						if (SeparateConceptTypeAssimilationMapsToParentConceptType(assimilation))
						{
							List<Column> newColumns = GenerateColumnsForConceptTypePreferredIdentifier(table,
								tableColumns, conceptTypeChildPath,
								assimilation.AssimilatedConceptType,
								// Assimilations should always be mandatory if they provide the preferred identifier
								// for the parent, but it doesn't hurt to check anyway until the object model
								// enforces this.
								isMandatorySoFar && assimilation.IsMandatory);

							// Create a uniqueness constraint for the preferred identifier of the concept type being separated.
							UniquenessConstraint newUniquenessConstraint = new UniquenessConstraint(table.Partition,
								new PropertyAssignment(UniquenessConstraint.NameDomainPropertyId, assimilation.AssimilatedConceptType.Name),
								new PropertyAssignment(UniquenessConstraint.IsPrimaryDomainPropertyId, isPreferredSoFar && assimilation.IsPreferredForParent));
							tableUniquenessConstraints.Add(newUniquenessConstraint);
							newUniquenessConstraint.ColumnCollection.AddRange(newColumns);
						}
						break;

					default:
						// UNDONE: OTHER_ASSIMILATION_MAPPING
						Debug.Fail("Unexpected AssimilationAbsorptionChoice value.");
						break;
				}
				conceptTypeChildPath.PopAndAssert(assimilation);
			}

			// Deal with the assimilations for which the current concept type is the target.
			foreach (ConceptTypeAssimilatesConceptType assimilation
				in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType))
			{
				conceptTypeChildPath.Push(assimilation);
				switch (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation))
				{
					case AssimilationAbsorptionChoice.Absorb:
						// We don't need to do anything for this case,
						// since it is mapping away from the current concept type.
						break;

					case AssimilationAbsorptionChoice.Partition:
						if (TableIsAlsoForConceptType.GetLink(table, assimilation.AssimilatorConceptType) != null)
						{
							// There is already a relationship between this concept type and table, meaning
							// that we already reached this concept type via another assimilation path.
							break;
						}
						TableIsAlsoForConceptType tableIsAlsoForConceptType =
							new TableIsAlsoForConceptType(table, assimilation.AssimilatorConceptType);

						// The conceptTypeChildPath is also the assimilation path: We will get
						// here only if everything before this point is an assimilation, since
						// this method is recursively called only for absorb and partition.
						tableIsAlsoForConceptType.AssimilationPath.AddRange(
							new CastEnumerator<ConceptTypeChild, PathStack<ConceptTypeChild>.Enumerator,
								ConceptTypeAssimilatesConceptType>(conceptTypeChildPath.GetEnumerator()));

						GenerateContentForConceptTypeChildren(table, tableColumns,
							tableUniquenessConstraints, conceptTypeChildPath,
							assimilation.AssimilatorConceptType,
							isMandatorySoFar /* Assimilations are always mandatory from the target side. */,
							isPreferredSoFar && assimilation.IsPreferredForTarget);
						break;

					case AssimilationAbsorptionChoice.Separate:
						if (!SeparateConceptTypeAssimilationMapsToParentConceptType(assimilation))
						{
							List<Column> newColumns = GenerateColumnsForConceptTypePreferredIdentifier(table,
								tableColumns, conceptTypeChildPath,
								assimilation.AssimilatorConceptType,
								isMandatorySoFar /* Assimilations are always mandatory from the target side. */);

							// Create a uniqueness constraint for the preferred identifier of the concept type being separated.
							UniquenessConstraint newUniquenessConstraint = new UniquenessConstraint(table.Partition,
								new PropertyAssignment(UniquenessConstraint.NameDomainPropertyId, assimilation.AssimilatorConceptType.Name),
								new PropertyAssignment(UniquenessConstraint.IsPrimaryDomainPropertyId, isPreferredSoFar && assimilation.IsPreferredForTarget));
							tableUniquenessConstraints.Add(newUniquenessConstraint);
							newUniquenessConstraint.ColumnCollection.AddRange(newColumns);
						}
						break;

					default:
						// UNDONE: OTHER_ASSIMILATION_MAPPING
						Debug.Fail("Unexpected AssimilationAbsorptionChoice value.");
						break;
				}
				conceptTypeChildPath.PopAndAssert(assimilation);
			}
		}

		/// <summary>
		/// Generates new <see cref="Column">Columns</see> in <paramref name="table"/> for the preferred identifier
		/// <see cref="ConceptTypeChild">ConceptTypeChildren</see> of <paramref name="conceptType"/>.
		/// </summary>
		/// <param name="table">
		/// The <see cref="Table"/> for which new <see cref="Column">Columns</see> should be generated.
		/// </param>
		/// <param name="tableColumns">
		/// The <see cref="LinkedElementCollection{Column}"/> to which the new <see cref="Column">Columns</see> should
		/// be added. <paramref name="table"/> must be the <see cref="ReadOnlyLinkedElementCollection{Column}.SourceElement"/>
		/// of this <see cref="LinkedElementCollection{Column}"/>.
		/// </param>
		/// <param name="conceptTypeChildPath">
		/// The path taken from <paramref name="table"/> to reach <paramref name="conceptType"/>. The last path step
		/// must be a <see cref="ConceptTypeChild"/> that reaches <paramref name="conceptType"/>.
		/// </param>
		/// <param name="conceptType">
		/// The <see cref="ConceptType"/> for which new <see cref="Column">Columns</see> should be generated.
		/// </param>
		/// <param name="isMandatorySoFar">
		/// Indicates whether all of the steps in <paramref name="conceptTypeChildPath"/> so far are mandatory,
		/// including the last path step that reaches <paramref name="conceptType"/>.
		/// </param>
		/// <returns>
		/// A <see cref="List{Column}"/> containing the new <see cref="Column">Columns</see>.
		/// </returns>
		private static List<Column> GenerateColumnsForConceptTypePreferredIdentifier(Table table, LinkedElementCollection<Column> tableColumns, PathStack<ConceptTypeChild> conceptTypeChildPath, ConceptType conceptType, bool isMandatorySoFar)
		{
			List<Column> newColumns = new List<Column>();
			GenerateColumnsForConceptTypePreferredIdentifier(table, tableColumns, conceptTypeChildPath, conceptType, isMandatorySoFar, newColumns);
			return newColumns;
		}

		/// <summary>
		/// Generates new <see cref="Column">Columns</see> in <paramref name="table"/> for the preferred identifier
		/// <see cref="ConceptTypeChild">ConceptTypeChildren</see> of <paramref name="conceptType"/>.
		/// </summary>
		/// <param name="table">
		/// The <see cref="Table"/> for which new <see cref="Column">Columns</see> should be generated.
		/// </param>
		/// <param name="tableColumns">
		/// The <see cref="LinkedElementCollection{Column}"/> to which the new <see cref="Column">Columns</see> should
		/// be added. <paramref name="table"/> must be the <see cref="ReadOnlyLinkedElementCollection{Column}.SourceElement"/>
		/// of this <see cref="LinkedElementCollection{Column}"/>.
		/// </param>
		/// <param name="conceptTypeChildPath">
		/// The path taken from <paramref name="table"/> to reach <paramref name="conceptType"/>. The last path step
		/// must be a <see cref="ConceptTypeChild"/> that reaches <paramref name="conceptType"/>.
		/// </param>
		/// <param name="conceptType">
		/// The <see cref="ConceptType"/> for which new <see cref="Column">Columns</see> should be generated.
		/// </param>
		/// <param name="isMandatorySoFar">
		/// Indicates whether all of the steps in <paramref name="conceptTypeChildPath"/> so far are mandatory,
		/// including the last path step that reaches <paramref name="conceptType"/>.
		/// </param>
		/// <param name="newColumns">
		/// The <see cref="List{Column}"/> to which new <see cref="Column">Columns</see> should be added.
		/// </param>
		private static void GenerateColumnsForConceptTypePreferredIdentifier(Table table, LinkedElementCollection<Column> tableColumns, PathStack<ConceptTypeChild> conceptTypeChildPath, ConceptType conceptType, bool isMandatorySoFar, List<Column> newColumns)
		{
			// First look for a preferred uniqueness constriant.
			foreach (Uniqueness uniquenessConstraint in conceptType.UniquenessCollection)
			{
				if (uniquenessConstraint.IsPreferred)
				{
					// We found one, so we just need to process the concept type children
					// that are included in it.
					foreach (ConceptTypeChild conceptTypeChild in uniquenessConstraint.ConceptTypeChildCollection)
					{
						InformationType informationType = conceptTypeChild as InformationType;
						if (informationType != null)
						{
							Column column = GenerateColumnForInformationType(tableColumns, conceptTypeChildPath, informationType, isMandatorySoFar);
							newColumns.Add(column);
							continue;
						}

						ConceptTypeRelatesToConceptType relation = conceptTypeChild as ConceptTypeRelatesToConceptType;
						if (relation != null)
						{
							conceptTypeChildPath.Push(relation);

							GenerateColumnsForConceptTypePreferredIdentifier(table,
								tableColumns, conceptTypeChildPath,
								relation.RelatedConceptType,
								isMandatorySoFar && relation.IsMandatory,
								newColumns);

							conceptTypeChildPath.PopAndAssert(relation);
							continue;
						}

						Debug.Fail("OIAL uniqueness constraints can't include concept type assimilations.");
					}
					return;
				}
			}

			// We didn't find a preferred uniqueness constraint, so look for a preferred
			// assimilation of this concept type. Although there can be more than one, we
			// only need the first, since they will all resolve to the same thing.
			foreach (ConceptTypeAssimilatesConceptType assimilation
				in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType))
			{
				if (assimilation.IsPreferredForTarget)
				{
					conceptTypeChildPath.Push(assimilation);

					GenerateColumnsForConceptTypePreferredIdentifier(table,
						tableColumns, conceptTypeChildPath,
						assimilation.AssimilatorConceptType,
						isMandatorySoFar /* Assimilations are always mandatory from the target side. */,
						newColumns);

					conceptTypeChildPath.PopAndAssert(assimilation);
					return;
				}
			}

			// We found neither a preferred uniqueness constraint nor a preferred assimilation
			// of this concept type, so the preferred identifier must be provided by a concept
			// type that is assimilated by this concept type.
			foreach (ConceptTypeAssimilatesConceptType assimilation
				in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(conceptType))
			{
				if (assimilation.IsPreferredForParent)
				{
					conceptTypeChildPath.Push(assimilation);

					GenerateColumnsForConceptTypePreferredIdentifier(table,
						tableColumns, conceptTypeChildPath,
						assimilation.AssimilatedConceptType,
						isMandatorySoFar && assimilation.IsMandatory,
						newColumns);

					conceptTypeChildPath.PopAndAssert(assimilation);
					return;
				}
			}

			Debug.Fail("Couldn't find preferred identifier for concept type.");
			throw new InvalidOperationException();
		}
		#endregion // Column and UniquenessConstraint generation methods


		#region ReferenceConstraint generation methods
		/// <summary>
		/// Generates new <see cref="ReferenceConstraint">ReferenceConstraints</see> in <paramref name="table"/>
		/// for all of the appropriate <see cref="ConceptTypeChild">ConceptTypeChildren</see> that involve
		/// <paramref name="conceptType"/>.
		/// </summary>
		/// <param name="table">
		/// The <see cref="Table"/> for which new <see cref="ReferenceConstraint">ReferenceConstraints</see>
		/// should be generated.
		/// </param>
		/// <param name="conceptTypeChildPath">
		/// The path taken from <paramref name="table"/> to reach <paramref name="conceptType"/>. The last path step
		/// must be a <see cref="ConceptTypeChild"/> that reaches <paramref name="conceptType"/>, unless
		/// <paramref name="table"/> <see cref="TableIsPrimarilyForConceptType">is primarily for</see>
		/// <paramref name="conceptType"/>, in which case this path must be empty.
		/// </param>
		/// <param name="conceptType">
		/// The <see cref="ConceptType"/> for which new <see cref="ReferenceConstraint">ReferenceConstraints</see>
		/// should be generated.
		/// </param>
		private static void GenerateReferenceConstraintsForConceptTypeChildren(Table table, PathStack<ConceptTypeChild> conceptTypeChildPath, ConceptType conceptType)
		{
			// Make foreign keys for each concept type relation.
			foreach (ConceptTypeRelatesToConceptType relation
				in ConceptTypeRelatesToConceptType.GetLinksToRelatedConceptTypeCollection(conceptType))
			{
				conceptTypeChildPath.Push(relation);
				GenerateReferenceConstraint(table, relation.RelatedConceptType, relation.Name, conceptTypeChildPath);
				conceptTypeChildPath.PopAndAssert(relation);
			}

			// Deal with the assimilations for which the current concept type is the parent.
			foreach (ConceptTypeAssimilatesConceptType assimilation
				in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatedConceptTypeCollection(conceptType))
			{
				conceptTypeChildPath.Push(assimilation);
				switch (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation))
				{
					case AssimilationAbsorptionChoice.Absorb:
						// Recurse and let the assimilated concept type handle any
						// reference constraints that it needs.
						GenerateReferenceConstraintsForConceptTypeChildren(table, conceptTypeChildPath,
							assimilation.AssimilatedConceptType);
						break;

					case AssimilationAbsorptionChoice.Partition:
						// We don't need to do anything for this case,
						// since it is mapping away from the current concept type.
						break;

					case AssimilationAbsorptionChoice.Separate:
						if (SeparateConceptTypeAssimilationMapsToParentConceptType(assimilation))
						{
							// Make a foreign key for the separation.
							ConceptType targetConceptType = assimilation.AssimilatedConceptType;
							GenerateReferenceConstraint(table, targetConceptType, targetConceptType.Name,
								conceptTypeChildPath);
						}
						break;

					default:
						// UNDONE: OTHER_ASSIMILATION_MAPPING
						Debug.Fail("Unexpected AssimilationAbsorptionChoice value.");
						break;
				}
				conceptTypeChildPath.PopAndAssert(assimilation);
			}

			// Deal with the assimilations for which the current concept type is the target.
			foreach (ConceptTypeAssimilatesConceptType assimilation
				in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType))
			{
				conceptTypeChildPath.Push(assimilation);
				switch (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation))
				{
					case AssimilationAbsorptionChoice.Absorb:
						// We don't need to do anything for this case,
						// since it is mapping away from the current concept type.
						break;

					case AssimilationAbsorptionChoice.Partition:
						// Recurse and let the assimilator concept type handle any
						// reference constraints that it needs.
						GenerateReferenceConstraintsForConceptTypeChildren(table, conceptTypeChildPath,
							assimilation.AssimilatorConceptType);
						break;

					case AssimilationAbsorptionChoice.Separate:
						if (!SeparateConceptTypeAssimilationMapsToParentConceptType(assimilation))
						{
							// Make a foreign key for the separation.
							ConceptType targetConceptType = assimilation.AssimilatorConceptType;
							GenerateReferenceConstraint(table, targetConceptType, targetConceptType.Name,
								conceptTypeChildPath);
						}
						break;

					default:
						// UNDONE: OTHER_ASSIMILATION_MAPPING
						Debug.Fail("Unexpected AssimilationAbsorptionChoice value.");
						break;
				}
				conceptTypeChildPath.PopAndAssert(assimilation);
			}
			
		}
		
		/// <summary>
		/// Generates a <see cref="ReferenceConstraint"/> from <paramref name="sourceTable"/>
		/// to the <see cref="Table"/> that is for <paramref name="targetConceptType"/>, based
		/// on <paramref name="sourceTableStartPath"/>.
		/// </summary>
		/// <param name="sourceTable">
		/// The <see cref="ReferenceConstraint.SourceTable"/> of the new <see cref="ReferenceConstraint"/>.
		/// </param>
		/// <param name="targetConceptType">
		/// The <see cref="ConceptType"/> that the new <see cref="ReferenceConstraint"/> should
		/// target. This must be the <see cref="ConceptType"/> that is reached at the end of
		/// <paramref name="sourceTableStartingPath"/>.
		/// </param>
		/// <param name="referenceConstraintName">
		/// The <see cref="Constraint.Name"/> of the new <see cref="ReferenceConstraint"/>.
		/// </param>
		/// <param name="sourceTableStartingPath">
		/// The path from <paramref name="sourceTable"/> that the <see cref="Column">Columns</see>
		/// that should be part of the new <see cref="ReferenceConstraint"/> begin with.
		/// </param>
		private static void GenerateReferenceConstraint(Table sourceTable, ConceptType targetConceptType, string referenceConstraintName, PathStack<ConceptTypeChild> sourceTableStartingPath)
		{
			Table targetTable;
			List<Column> sourceColumns;
			if (null == (targetTable = GetTargetTableForReferenceConstraint(targetConceptType)) ||
				// Find the columns in the source table that start with the correct path. These will already be in the correct
				// order, since we create them based off the order in the preferred identifier of the target concept type. This
				// can be empty for secondary assimilation paths.
				null == (sourceColumns = GetColumnsForStartingPath(sourceTable, sourceTableStartingPath)))
			{
				return;
			}

			// Create the reference constraint and set its name, source table, and target table.
			ReferenceConstraint referenceConstraint = new ReferenceConstraint(sourceTable.Partition,
					new PropertyAssignment(ReferenceConstraint.NameDomainPropertyId, referenceConstraintName));
			referenceConstraint.SourceTable = sourceTable;
			referenceConstraint.TargetTable = targetTable;

			Column[] targetColumns = new Column[sourceColumns.Count];

			// Find the target column for each source column.
			LinkedElementCollection<ColumnReference> columnReferences = referenceConstraint.ColumnReferenceCollection;
			LinkedElementCollection<Column> targetTableColumns = targetTable.ColumnCollection;

			// Everything at an index less than sourcePathStartIndex will be a non-separate concept type assimilation
			// (except for sourcePathStartIndex - 1, which will be the concept type relation or separate concept type
			// assimilation that we are generating the reference constraint for).
			int sourcePathStartIndex = sourceTableStartingPath.Count;

			for (int columnIndex = 0; columnIndex < targetColumns.Length; columnIndex++)
			{
				Column sourceColumn = sourceColumns[columnIndex];

				LinkedElementCollection<ConceptTypeChild> sourcePath =
					ColumnHasConceptTypeChild.GetConceptTypeChildPath(sourceColumn);
				int sourcePathCount = sourcePath.Count;

				// To determine the concept type child path of the target column (the "desired path"), we
				// start with the portion of the source column's path after the concept type relation or
				// separate concept type assimilation that we are making a reference constraint for. Then,
				// two changes need to be made to the desired path before it will match the target column's
				// path:
				// 1) Remove any concept type assimilations at the beginning of the desired path that lead
				//    us to a concept type that the target table is primarily or also for.
				// 2) The last concept type reached in step 1 is the concept type that we reached from the
				//    last concept type assimilation that we removed. If we did not remove any concept type
				//    assimilations, the last concept type is the targetConceptType that was passed in to
				//    this method.
				//    If the table is also for (rather than primarily for) this last concept type, the
				//    assimilation path from the target table to this last concept type is inserted at
				//    the beginning of the desired path.

				int sourcePathIndex = sourcePathStartIndex;
				// The current concept type is the concept type that we are coming from as we walk the path.
				ConceptType currentConceptType = targetConceptType;
				// The primary concept type is the concept type that the target table is primarily for.
				ConceptType primaryConceptType = TableIsPrimarilyForConceptType.GetConceptType(targetTable);
				while (true)
				{
					ConceptTypeAssimilatesConceptType assimilationPathStep =
						sourcePath[sourcePathIndex] as ConceptTypeAssimilatesConceptType;
					if (assimilationPathStep == null)
					{
						// We hit a concept type relation or an information type, so we're done.
						break;
					}
					ConceptType nextConceptType = assimilationPathStep.AssimilatedConceptType;
					if (nextConceptType == currentConceptType)
					{
						nextConceptType = assimilationPathStep.AssimilatorConceptType;
						Debug.Assert(nextConceptType != currentConceptType,
							"The parent and target of a concept type assimilation must not be the same concept type.");
					}

					if (nextConceptType != primaryConceptType &&
						TableIsAlsoForConceptType.GetLink(targetTable, nextConceptType) == null)
					{
						break;
					}
					
					// The next concept type is still part of the target table, so we don't include
					// it in the desired path, and we do keep walking the path.
					sourcePathIndex++;
					currentConceptType = nextConceptType;
				}

				List<ConceptTypeChild> desiredPath;
				if (currentConceptType != primaryConceptType)
				{
					// Since the last concept type we hit is not the concept type that the
					// table is primarily for, we need to look for a column that starts with
					// the assimilation path from the table to that concept type.
					TableIsAlsoForConceptType tableIsAlsoForConceptType =
						TableIsAlsoForConceptType.GetLink(targetTable, currentConceptType);
					Debug.Assert(tableIsAlsoForConceptType != null);
					LinkedElementCollection<ConceptTypeAssimilatesConceptType> assimilationPath =
						tableIsAlsoForConceptType.AssimilationPath;

					desiredPath =
						new List<ConceptTypeChild>(assimilationPath.Count + (sourcePathCount - sourcePathIndex));
					foreach (ConceptTypeAssimilatesConceptType assimilation in assimilationPath)
					{
						desiredPath.Add(assimilation);
					}
				}
				else
				{
					desiredPath = new List<ConceptTypeChild>(sourcePathCount - sourcePathIndex);
				}

				// Add the rest of the source path to the path that we're looking for.
				for (int i = sourcePathIndex; i < sourcePathCount; i++)
				{
					desiredPath.Add(sourcePath[i]);
				}

				Column targetColumn = GetColumnForExactPath(targetTableColumns, desiredPath);
				if (targetColumn == null)
				{
					// Something went very wrong. Delete the reference constraint and bail out.
					// This reference constraint will obviously be missing, but at least we won't
					// be blocking legal changes to the OIAL model.
					Debug.Fail("Could not find target column for reference constraint.");
					referenceConstraint.Delete();
					return;
				}

				// Record the target column (so that we can use it to find the target uniqueness constraint).
				targetColumns[columnIndex] = targetColumn;

				// Create the relationship between the source and target columns.
				columnReferences.Add(new ColumnReference(sourceColumn, targetColumn));
			}

			// Find the uniqueness constraint that the reference constraint is targeting.
			LinkedElementCollection<UniquenessConstraint> uniquenessConstraints =
				UniquenessConstraintIncludesColumn.GetUniquenessConstraints(targetColumns[0]);
			foreach (UniquenessConstraint uniquenessConstraint in uniquenessConstraints)
			{
				LinkedElementCollection<Column> uniquenessConstraintColumns = uniquenessConstraint.ColumnCollection;
				if (uniquenessConstraintColumns.Count != targetColumns.Length)
				{
					continue;
				}
				// See if we have the right columns.
				// Because the order must always match between the reference constraint
				// and the uniqueness constraint, we check it in order rather than
				// using Contains.
				int targetColumnsIndex;
				for (targetColumnsIndex = 0; targetColumnsIndex < targetColumns.Length; targetColumnsIndex++)
				{
					if (uniquenessConstraintColumns[targetColumnsIndex] != targetColumns[targetColumnsIndex])
					{
						break;
					}
				}

				if (targetColumnsIndex == targetColumns.Length)
				{
					// We found the target uniqueness constraint.
					referenceConstraint.TargetUniquenessConstraint = uniquenessConstraint;
					break;
				}
			}

			Debug.Assert(referenceConstraint.TargetUniquenessConstraint != null,
				"Could not find target uniqueness constraint for reference constraint.");
		}

		/// <summary>
		/// Retrieves the <see cref="Table"/> that a <see cref="ReferenceConstraint"/> should
		/// target for <paramref name="targetConceptType"/>.
		/// </summary>
		private static Table GetTargetTableForReferenceConstraint(ConceptType targetConceptType)
		{
			Table targetTable = TableIsPrimarilyForConceptType.GetTable(targetConceptType);
			if (targetTable == null)
			{
				LinkedElementCollection<Table> tables = TableIsAlsoForConceptType.GetTable(targetConceptType);
				if (tables.Count == 1)
				{
					targetTable = tables[0];
				}
				else
				{
					// UNDONE: Handle concept types being mapped into more than one table.
					// The most common occurrence of this is for partition cases.
					// Don't forget to check that it actually was mapped into multiple tables
					// (rather than none)!
				}
			}
			return targetTable;
		}

		/// <summary>
		/// Retrieves the <see cref="Column"/> in <paramref name="columns"/> that has a
		/// <see cref="ConceptTypeChild"/> <see cref="ColumnHasConceptTypeChild">path</see>
		/// that exactly matches <paramref name="desiredPath"/>.
		/// </summary>
		private static Column GetColumnForExactPath(LinkedElementCollection<Column> columns, List<ConceptTypeChild> desiredPath)
		{
			foreach (Column column in columns)
			{
				if (ColumnConceptTypeChildPathExactlyMatches(column, desiredPath))
				{
					return column;
				}
			}
			return null;
		}

		/// <summary>
		/// Retrieves the <see cref="Column">Columns</see> in <paramref name="table"/> that have
		/// a <see cref="ConceptTypeChild"/> <see cref="ColumnHasConceptTypeChild">path</see> that
		/// starts with <paramref name="startingPath"/>.
		/// </summary>
		private static List<Column> GetColumnsForStartingPath(Table table, PathStack<ConceptTypeChild> startingPath)
		{
			List<Column> columns = null;
			foreach (Column column in table.ColumnCollection)
			{
				if (ColumnConceptTypeChildPathStartsWith(column, startingPath))
				{
					(columns ?? (columns = new List<Column>())).Add(column);
				}
			}
			return columns;
		}

		/// <summary>
		/// Indicates whether <paramref name="column"/> has a <see cref="ConceptTypeChild"/>
		/// <see cref="ColumnHasConceptTypeChild">path</see> that exactly matches
		/// <paramref name="desiredPath"/>.
		/// </summary>
		private static bool ColumnConceptTypeChildPathExactlyMatches(Column column, List<ConceptTypeChild> desiredPath)
		{
			LinkedElementCollection<ConceptTypeChild> actualPath = ColumnHasConceptTypeChild.GetConceptTypeChildPath(column);
			if (actualPath.Count != desiredPath.Count)
			{
				// It can't match it if it is a different length than it.
				return false;
			}
			// Walk the path backwards, since the end is more likely to differ than the beginning.
			for (int i = desiredPath.Count - 1; i >= 0; --i)
			{
				if (actualPath[i] != desiredPath[i])
				{
					return false;
				}
			}
			return true;
		}
		/// <summary>
		/// Indicates whether <paramref name="column"/> has a <see cref="ConceptTypeChild"/>
		/// <see cref="ColumnHasConceptTypeChild">path</see> that starts with
		/// <paramref name="startingPath"/>.
		/// </summary>
		private static bool ColumnConceptTypeChildPathStartsWith(Column column, PathStack<ConceptTypeChild> startingPath)
		{
			LinkedElementCollection<ConceptTypeChild> actualPath = ColumnHasConceptTypeChild.GetConceptTypeChildPath(column);
			int startingPathCount = startingPath.Count;
			if (actualPath.Count <= startingPathCount)
			{
				// It can't start with it if it is not longer than it.
				return false;
			}
			// Walk the path backwards, since the end is more likely to differ than the beginning.
			for (int i = startingPathCount - 1; i >= 0; --i)
			{
				if (startingPath[i] != actualPath[i])
				{
					return false;
				}
			}
			return true;
		}
		#endregion // ReferenceConstraint generation methods
		#endregion // Fully populate from OIAL
		#region Incremental update methods
		/// <summary>
		/// Updates the <see cref="Column.IsNullable">nullability</see> of
		/// <paramref name="column"/>, if necessary, to reflect the current
		/// state of the model.
		/// </summary>
		private static void UpdateColumnNullability(Column column)
		{
			bool isNullable = false;
			ConceptType previousConceptType = null;
			foreach (ConceptTypeChild child in ColumnHasConceptTypeChild.GetConceptTypeChildPath(column))
			{
				ConceptTypeAssimilatesConceptType assimilation = child as ConceptTypeAssimilatesConceptType;
				if (assimilation != null)
				{
					if (previousConceptType == null)
					{
						// This is the first concept type child in the path. To determine what our starting concept
						// type is, we need to look at which concept type the table is primarily for. We only need
						// to do this for assimilations, since for information types and relations the starting
						// concept type will always be the parent.
						previousConceptType = TableIsPrimarilyForConceptType.GetConceptType(column.Table);
						Debug.Assert(previousConceptType != null);
					}

					if (assimilation.Parent == previousConceptType)
					{
						// We are walking this assimilation from parent to target, so we go off the IsMandatory value.
						if (!child.IsMandatory)
						{
							isNullable = true;
							break;
						}

						// The next concept type in the path is the target (assimilated concept type).
						previousConceptType = assimilation.AssimilatedConceptType;
					}
					else
					{
						Debug.Assert(assimilation.Target == previousConceptType);

						// We are walking this assimilation from target to parent, so it is always mandatory.
						// The next concept type in the path is the parent (assimilator concept type).
						previousConceptType = assimilation.AssimilatorConceptType;
					}
				}
				else
				{
					// This is not an assimilation, so we always go off of the IsMandatory value.
					if (!child.IsMandatory)
					{
						isNullable = true;
						break;
					}

					// Get the next concept type in the path (or null if this is an information type and therefore the
					// end of the path).
					previousConceptType = child.Target as ConceptType;
					Debug.Assert((previousConceptType != null) || (child is InformationType));
				}
			}
			if (column.IsNullable != isNullable)
			{
				column.IsNullable = isNullable;
			}
		}
		#endregion // Incremental update methods
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
				yield return ReferenceModeNaming.FixupListener;
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
					SchemaGenerationSetting generationSetting = new SchemaGenerationSetting(store, new PropertyAssignment(SchemaGenerationSetting.CoreAlgorithmVersionDomainPropertyId, CurrentCoreAlgorithmVersion), new PropertyAssignment(SchemaGenerationSetting.NameAlgorithmVersionDomainPropertyId, CurrentNameAlgorithmVersion));
					new GenerationSettingTargetsSchema(generationSetting, schema);
					new ORMCore.GenerationStateHasGenerationSetting(ORMCore.GenerationState.EnsureGenerationState(store), generationSetting);
					schema.Catalog = catalog;
					notifyAdded.ElementAdded(schema, true);

					FullyGenerateConceptualDatabaseModel(schema, element, notifyAdded);
				}
				else
				{
					SchemaGenerationSetting generationSetting = GenerationSettingTargetsSchema.GetGenerationSetting(schema);
					bool regenerateAll = generationSetting == null || generationSetting.CoreAlgorithmVersion != CurrentCoreAlgorithmVersion;
					bool regenerateNames = false;
					if (!regenerateAll)
					{
						foreach (Table table in schema.TableCollection)
						{
							if (null == TableIsPrimarilyForConceptType.GetLinkToConceptType(table))
							{
								regenerateAll = true;
								break;
							}
							// Theoretically we should also check that all columns and uniqueness constraints
							// are pathed back to the abstraction model. However, this is far from a full validation,
							// and the scenario we're trying to cover is the abstraction model regenerating during
							// load and removing our bridge elements. The table check above is sufficient.
						}
						regenerateNames = !regenerateAll && generationSetting.NameAlgorithmVersion != CurrentNameAlgorithmVersion;
						generationSetting.NameAlgorithmVersion = CurrentNameAlgorithmVersion;
					}
					else
					{
						if (generationSetting == null)
						{
							generationSetting = new SchemaGenerationSetting(store, new PropertyAssignment(SchemaGenerationSetting.CoreAlgorithmVersionDomainPropertyId, CurrentCoreAlgorithmVersion), new PropertyAssignment(SchemaGenerationSetting.NameAlgorithmVersionDomainPropertyId, CurrentNameAlgorithmVersion));
							new GenerationSettingTargetsSchema(generationSetting, schema);
							new ORMCore.GenerationStateHasGenerationSetting(ORMCore.GenerationState.EnsureGenerationState(store), generationSetting);
						}
						else
						{
							regenerateNames = generationSetting.NameAlgorithmVersion != CurrentNameAlgorithmVersion;
							generationSetting.CoreAlgorithmVersion = CurrentCoreAlgorithmVersion;
							generationSetting.NameAlgorithmVersion = CurrentNameAlgorithmVersion;
						}
					}
					if (regenerateAll)
					{
						schema.TableCollection.Clear();
						schema.DomainCollection.Clear();
						FullyGenerateConceptualDatabaseModel(schema, element, notifyAdded);
					}
					else if (regenerateNames)
					{
						NameGeneration.GenerateAllNames(schema);
					}
				}
			}
		}
		#endregion // GenerateConceptualDatabaseFixupListener class
	}
}
