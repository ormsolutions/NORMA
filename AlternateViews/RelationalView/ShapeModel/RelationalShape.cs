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
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.OIALModel;
using System.Drawing;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace Neumont.Tools.ORM.Views.RelationalView
{
	/// <summary>
	/// Represents the framework for the relational schema view of the ORM Diagram.
	/// </summary>
	/// <remarks>
	/// Based on the current version of DCIL.xsd.
	/// </remarks>
	internal partial class RelationalModel
	{
		#region Validation Rules
		[RuleOn(typeof(Table))]
		private sealed partial class DelayedFixUpDiagram : AddRule
		{
			private static readonly FixUpDiagram myFixUpDiagram = new FixUpDiagram();
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelElement childElement = e.ModelElement;
				if (childElement is Table)
				{
					ORMCoreDomainModel.DelayValidateElement(childElement, delegate(ModelElement target)
					{
						myFixUpDiagram.ElementAdded(e);
					});
				}
			}
		}

		[RuleOn(typeof(TableHasColumn))]
		private sealed partial class DelayedCompartmentItemAddRule : AddRule
		{
			private static readonly CompartmentItemAddRule myCompartmentItemAddRule = new CompartmentItemAddRule();

			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelElement childElement = e.ModelElement;
				TableHasColumn tableColumn = childElement as TableHasColumn;
				if (tableColumn != null)
				{
					ORMCoreDomainModel.DelayValidateElement(childElement,
						delegate(ModelElement target)
						{
							myCompartmentItemAddRule.ElementAdded(e);
						});
				}
			}

		}

		[RuleOn(typeof(TableReferencesTable), InitiallyDisabled = true)]
		private sealed partial class DelayedForeignKeyItemAddRule : AddRule
		{
			private static readonly FixUpDiagram myFixUpDiagram = new FixUpDiagram();
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				TableReferencesTable tableRef = e.ModelElement as TableReferencesTable;
				if (tableRef != null)
				{
					ORMCoreDomainModel.DelayValidateElement(tableRef, delegate(ModelElement target)
					{
						myFixUpDiagram.ElementAdded(e);
					});
				}
			}
		}

		[RuleOn(typeof(RelationalDiagram))] // AddRule
		private sealed partial class DelayedRelationalDiagramAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				RelationalDiagram relDiagram = e.ModelElement as RelationalDiagram;
				if (relDiagram != null)
				{
					relDiagram.Store.RuleManager.EnableRule(typeof(DelayedForeignKeyItemAddRule));
					ORMCoreDomainModel.DelayValidateElement(relDiagram, delegate(ModelElement target)
					{
						RelationalModel relModel = relDiagram.ModelElement as RelationalModel;
						if (relModel != null)
						{
							relModel.GenerateTables(relModel.OIALModel);
						}
					});
				}
			}
		}

		[RuleOn(typeof(RelationalDiagram))] // DeleteRule
		private sealed partial class RelationalDiagramDeleteRule : DeleteRule
		{
			public override void ElementDeleted(ElementDeletedEventArgs e)
			{
				RelationalDiagram relDiagram = e.ModelElement as RelationalDiagram;
				if (relDiagram != null)
				{
					relDiagram.Store.RuleManager.DisableRule(typeof(DelayedForeignKeyItemAddRule));
				}
			}
		}

		[RuleOn(typeof(OIALModelHasORMModel))]
		private sealed partial class DelayedOIALModelAddedRule : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				OIALModelHasORMModel link = e.ModelElement as OIALModelHasORMModel;
				ORMCoreDomainModel.DelayValidateElement(link.OIALModel, AddRelationalModel);
			}

			private void AddRelationalModel(ModelElement element)
			{
				OIALModel.OIALModel oialModel = element as OIALModel.OIALModel;
				if (oialModel != null)
				{
					RelationalModel relationalModel = new RelationalModel(oialModel.Store,
						new PropertyAssignment(RelationalModel.NameDomainPropertyId, oialModel.Name));
					relationalModel.OIALModel = oialModel;
				}
			}
		}
		/// <summary>
		/// Called whenever a <see cref="ConceptType"/> is added to the model, so that we can regenerate the RelationalView.
		/// </summary>
		[RuleOn(typeof(OIALModelHasConceptType))] // AddRule
		private sealed partial class DelayedConceptTypeAddedRule : AddRule
		{
			/// <summary>
			/// Standard override. Process the added <see cref="ConceptType"/>.
			/// </summary>
			/// <param name="e"></param>
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelElement childElement = e.ModelElement;
				if (childElement is OIALModelHasConceptType)
				{
					ORMCoreDomainModel.DelayValidateElement(childElement, AddTables);
				}
			}
			/// <summary>
			/// The callback function sent to <see cref="ORMCoreDomainModel.DelayValidateElement"/> to add function to the
			/// <see cref="RelationalModel"/>.
			/// </summary>
			/// <param name="element">ConceptType</param>
			private void AddTables(ModelElement element)
			{
				OIALModelHasConceptType link = element as OIALModelHasConceptType;
				OIALModel.OIALModel oialModel = link.Model;
				RelationalModel relationalModel = RelationalModelHasOIALModel.GetRelationalModel(oialModel);
				// Create a new relational model if there is no RelationalModel currently associated with the OIALModel.
				if (relationalModel == null)
				{
					relationalModel = new RelationalModel(oialModel.Store);
					relationalModel.OIALModel = oialModel;
				}
				LinkedElementCollection<PresentationElement> presentationElement = PresentationViewsSubject.GetPresentation(relationalModel);
				if (presentationElement.Count != 0 && ++myConceptTypeCount == oialModel.ConceptTypeCollection.Count)
				{
					myConceptTypeCount = 0;
					relationalModel.GenerateTables(oialModel);
				}
			}
		}
		#endregion // Validation Rules
		#region Initializers
		/// <summary>
		/// The string that denotes a primary key.
		/// </summary>
		private const string PRIMARY_KEY = "PK";
		/// <summary>
		/// The string that prepends an alternate key.
		/// </summary>
		private const string ALTERNATE_KEY = "U";
		/// <summary>
		/// The string that prepends a foreign key.
		/// </summary>
		private const string FOREIGN_KEY = "FK";
		///// <summary>
		///// A counter for foreign key constraints used when generating <see cref="ForeignKey"/> names.
		///// </summary>
		//private static int myForeignKeyCount = 0;
		/// <summary>
		/// A counter for <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/>s that call the
		/// <see cref="T:Neumont.Tools.ORM.Views.RelationalView.RelationalModel.DelayedConceptTypeAddedRule"/>. Ensures
		/// that only one call to generate tables is made per <see cref="T:Neumont.Tools.ORM.OIALModel.OIALModel"/> regeneration.
		/// </summary>
		private static int myConceptTypeCount = 0;
		/// <summary>
		/// Generates the <see cref="T:Neumont.Tools.ORM.Views.RelationalView.Table"/> objects for the <see cref="RelationalModel"/>.
		/// </summary>
		/// <param name="model">The <see cref="T:Neumont.Tools.ORM.OIALModel.OIALModel"/> on which the
		/// <see cref="T:Neumont.Tools.ORM.Views.RelationalView.Table"/>s are based.</param>
		private void GenerateTables(OIALModel.OIALModel model)
		{
			using (Transaction t = Store.TransactionManager.BeginTransaction())
			{
				myConceptTypeCount = 0;
				// Reset the tables and foreign key count.
				TableCollection.Clear();

				// Generate the tables for each ConceptType
				LinkedElementCollection<ConceptType> conceptTypeCollection = model.ConceptTypeCollection;
				foreach (ConceptType conceptType in conceptTypeCollection)
				{
					Table table = new Table(Store,
						new PropertyAssignment(Table.NameDomainPropertyId, conceptType.Name));
					table.ConceptType = conceptType;
					table.RelationalModel = this;
				}

				// Generate the columns for each ConceptType
				LinkedElementCollection<Table> tables = this.TableCollection;

				int uniquenessCount = 0, foreignKeyCount = 0;
				foreach (Table table in tables)
				{
					GenerateColumnsForConceptType(table, model, table.ConceptType, true, ref uniquenessCount, ref foreignKeyCount);
					uniquenessCount = 0;
					foreignKeyCount = 0;
				}
				if (t.HasPendingChanges)
				{
					t.Commit();
				}
			}
		}
		#endregion // Initializers
		#region Getting Columns For Concept Types
		/// <summary>
		/// Generates and adds <see cref="T:Neumont.Tools.ORM.Views.RelationalView.Column"/>s to the
		/// <see cref="T:Neumont.Tools.ORM.Views.RelationalView.Table"/> passed.
		/// </summary>
		/// <param name="oialModel">The <see cref="T:Neumont.Tools.ORM.OIALModel.OIALModel"/> which contains information about the
		/// passed <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/>.</param>
		/// <param name="conceptType">The <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> whose columns are of interest.</param>
		/// <param name="isTopLevel">If the concept type being passed is top level, then <see langword="true"/>. Otherwise, <see langword="false"/>.</param>
		/// <param name="table">The <see cref="T:Neumont.Tools.ORM.Views.RelationalView.Table"/> to which
		/// <see cref="T:Neumont.Tools.ORM.Views.RelationalView.Column"/> objects should be added.</param>
		/// <param name="uniquenessConstraintCount">The current number of <see cref="T:Neumont.Tools.ORM.Views.RelationalView.UniquenessConstraint"/>
		/// objects on the current table.</param>
		/// <returns><see cref="T:System.Collections.IEnumerable&lt;Column&gt;"/></returns>
		private IEnumerable<Column> GenerateColumnsForConceptType(Table table, OIALModel.OIALModel oialModel, ConceptType conceptType, bool isTopLevel, ref int uniquenessConstraintCount, ref int foreignKeyCount)
		{
			Store theStore = Store;

			List<Column> tableColumns = new List<Column>();

			// Gets the prefix for all columns created. This usually occurs for absorbed ConceptTypes.
			string prefix = GetPrefix(conceptType);

			// Get the ConceptTypeChild objects associated with this ConceptType.
			ReadOnlyCollection<ConceptTypeChild> conceptTypeChildren = ConceptTypeChild.GetLinksToTargetCollection(conceptType);


			foreach (ConceptTypeChild ctc in conceptTypeChildren)
			{
				InformationType informationType = null;
				ConceptTypeRef conceptTypeRef = null;
				ConceptTypeAbsorbedConceptType absorbedConceptType = null;
				if ((informationType = ctc as InformationType) != null)
				{
					Column column = GetColumnForInformationType(informationType, isTopLevel, prefix, false);
					tableColumns.Add(column);
					foreach (SingleChildUniquenessConstraint uConstraint in informationType.SingleChildConstraintCollection)
					{
						bool isPrimary = isTopLevel ? uConstraint.IsPreferred : false;
						string constraintName = isPrimary ? PRIMARY_KEY : string.Concat(ALTERNATE_KEY, ++uniquenessConstraintCount);

						// Uniqueness constraints cannot be preferred if the ConceptType of interest is not top-level.
						UniquenessConstraint relationalUniquenessConstraint = new UniquenessConstraint(theStore,
							new PropertyAssignment(UniquenessConstraint.NameDomainPropertyId, constraintName),
							new PropertyAssignment(UniquenessConstraint.IsPreferredDomainPropertyId, isPrimary));
						relationalUniquenessConstraint.ColumnCollection.Add(column);
						table.ConstraintCollection.Add(relationalUniquenessConstraint);
					}
				}
				else if ((conceptTypeRef = ctc as ConceptTypeRef) != null)
				{
					string newPrefix = string.Concat(prefix, conceptTypeRef.Name);
					bool isNullable = conceptTypeRef.Mandatory == MandatoryConstraintModality.Alethic && isTopLevel;
					IEnumerable<Column> foreignKeyColumns = GetPreferredIdentifierColumnsForConceptTypeRef(oialModel, conceptTypeRef, isNullable, newPrefix);
					tableColumns.AddRange(foreignKeyColumns);
					ForeignKey foreignKey = new ForeignKey(theStore,
						new PropertyAssignment(ForeignKey.NameDomainPropertyId, string.Concat(FOREIGN_KEY, ++foreignKeyCount)));
					foreignKey.Table = table;
					foreignKey.ColumnCollection.AddRange(foreignKeyColumns);
					table.ReferencedTableCollection.Add(FindTable(conceptTypeRef.ReferencedConceptType));
					// Get the link we just created
					ReadOnlyCollection<TableReferencesTable> tableReferences = TableReferencesTable.GetLinksToReferencedTableCollection(table);
					int tableReferenceCount = tableReferences.Count;
					TableReferencesTable tableRef = tableReferences[tableReferenceCount - 1];
					tableRef.ForeignKey = foreignKey;
				}
				else if ((absorbedConceptType = ctc as ConceptTypeAbsorbedConceptType) != null)
				{
					// Generate columns that this absorbed concept type would create, if any.
					tableColumns.AddRange(
						GenerateColumnsForConceptType(table, oialModel, absorbedConceptType.AbsorbedConceptType, false, ref uniquenessConstraintCount, ref foreignKeyCount)
						);
				}
			}


			// Add alethic uniqueness constraints that refer to more than one ConceptTypeChild to the model.
			foreach (ChildSequenceConstraint childSequenceConstraint in oialModel.ChildSequenceConstraintCollection)
			{
				ChildSequenceUniquenessConstraint uConstraint = childSequenceConstraint as ChildSequenceUniquenessConstraint;
				if (uConstraint != null && uConstraint.Modality == ConstraintModality.Alethic)
				{
					bool addConstraint = false, shouldContinue = false;
					LinkedElementCollection<ConceptTypeChild> conceptTypeChildCollection = uConstraint.ChildSequence.ConceptTypeChildCollection;
					foreach (ConceptTypeChild conceptTypeChild in conceptTypeChildCollection)
					{
						if (conceptTypeChild.Parent == conceptType)
						{
							addConstraint = true;
						}
						else
						{
							addConstraint = false;
							break;
						}
					}
					
					//if (shouldContinue)
					//{
					//    continue;
					//}
					// Ensures that the parents of all ConceptTypeChild objects in this UniquenessConstraint belong to this ConceptType
					if (addConstraint)
					{
						// If the first path role of any ConceptTypeChild (The first is chosen arbitrarily here.) does not have
						// the corresponding uniqueness constraint on it, then it will not be mapped to the logical level.

						bool firstRoleHasUniquenessConstraint = false;
						LinkedElementCollection<ConstraintRoleSequence> roleConstraints = conceptTypeChildCollection[0].PathRoleCollection[0].Role.ConstraintRoleSequenceCollection;
						foreach (ConstraintRoleSequence constraintRoleSequence in roleConstraints)
						{
							ObjectModel.UniquenessConstraint objUniquenessConstraint = constraintRoleSequence.Constraint as ObjectModel.UniquenessConstraint;
							if (objUniquenessConstraint != null && objUniquenessConstraint.IsPreferred)
							{
								firstRoleHasUniquenessConstraint = true;
								break;
							}
						}
						if (!firstRoleHasUniquenessConstraint)
						{
							continue;
						}
						bool isPrimary = isTopLevel ? uConstraint.IsPreferred : false;
						string constraintName = isPrimary ? "PK" : string.Concat("U", ++uniquenessConstraintCount);
						UniquenessConstraint uniquenessConstraint = new UniquenessConstraint(theStore,
							new PropertyAssignment(UniquenessConstraint.NameDomainPropertyId, constraintName),
							new PropertyAssignment(UniquenessConstraint.IsPreferredDomainPropertyId, isPrimary));
						uniquenessConstraint.ColumnCollection.AddRange(GetColumnReferences(oialModel, conceptTypeChildCollection, tableColumns, prefix));
						table.ConstraintCollection.Add(uniquenessConstraint);
					}
				}
			}

			if (isTopLevel)
			{
				// Ordering the columns in the table
				List<Column> primaryKeyColumns = new List<Column>();
				List<Column> alternateKeyedColumns = new List<Column>();
				List<Column> otherMandatoryColumns = new List<Column>();
				List<Column> otherColumns = new List<Column>();
				for (int i = 0; i < tableColumns.Count; ++i)
				{
					int newAppendage = 1;
					Column column = tableColumns[i];
					for (int j = 0; j < tableColumns.Count; ++j)
					{
						Column newColumn = tableColumns[j];
						if (column.Name == newColumn.Name && !column.Equals(newColumn))
						{
							newColumn.Name += ++newAppendage;
						}
					}
					LinkedElementCollection<Constraint> constraints = column.ConstraintCollection;
					bool isPrimary = false, isKeyed = false;
					foreach (Constraint constraint in constraints)
					{
						UniquenessConstraint uniquenessConstraint = constraint as UniquenessConstraint;
						if (uniquenessConstraint != null)
						{
							if (uniquenessConstraint.IsPreferred)
							{
								isPrimary = true;
							}
							else
							{
								isKeyed = true;
							}
						}
					}
					if (isPrimary)
					{
						primaryKeyColumns.Add(column);
					}
					else if (isKeyed)
					{
						alternateKeyedColumns.Add(column);
					}
					else if (column.IsMandatory)
					{
						otherMandatoryColumns.Add(column);
					}
					else
					{
						otherColumns.Add(column);
					}
				}
				LinkedElementCollection<Column> actualTableColumns = table.ColumnCollection;
				actualTableColumns.AddRange(primaryKeyColumns);
				actualTableColumns.AddRange(alternateKeyedColumns);
				actualTableColumns.AddRange(otherMandatoryColumns);
				actualTableColumns.AddRange(otherColumns);
			}
			return tableColumns;
		}
		/// <summary>
		/// Retrieves an <see cref="T:System.Collections.IEnumerable&lt;T&gt;"/> of <see cref="T:Neumont.Tools.ORM.Views.RelationalView.Column"/> objects
		/// which represent the preferred identifier columns for a <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptTypeRef"/>.
		/// </summary>
		/// <param name="oialModel">The <see cref="T:Neumont.Tools.ORM.OIALModel.OIALModel"/> which contains information about the
		/// passed <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptTypeRef"/>.</param>
		/// <param name="conceptTypeRef">The <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> whose target columns are of interest.</param>
		/// <param name="isTopLevel">If the concept type being passed is top level, then <see langword="true"/>. Otherwise, <see langword="false"/>.</param>
		/// <param name="prefix">A <see cref="T:System.String"/> which is prepended to the names of all columns generated by this method call.</param>
		/// <returns><see cref="T:System.Collections.IEnumerable&lt;Column&gt;"/></returns>
		private IEnumerable<Column> GetPreferredIdentifierColumnsForConceptTypeRef(OIALModel.OIALModel oialModel, ConceptTypeRef conceptTypeRef, bool isTopLevel, string prefix)
		{
			return GetPreferredIdentifierColumnsForConceptType(oialModel, conceptTypeRef.ReferencedConceptType, isTopLevel, prefix);
		}
		/// <summary>
		/// Retrieves an <see cref="T:System.Collections.IEnumerable&lt;T&gt;"/> of <see cref="T:Neumont.Tools.ORM.Views.RelationalView.Column"/> objects
		/// which represent the preferred identifier columns for a <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/>.
		/// </summary>
		/// <param name="oialModel">The <see cref="T:Neumont.Tools.ORM.OIALModel.OIALModel"/> which contains information about the
		/// passed <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptTypeRef"/>.</param>
		/// <param name="conceptTypeRef">The <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> whose target columns are of interest.</param>
		/// <param name="isTopLevel">If the concept type being passed is top level, then <see langword="true"/>. Otherwise, <see langword="false"/>.</param>
		/// <param name="prefix">A <see cref="T:System.String"/> which is prepended to the names of all columns generated by this method call.</param>
		/// <returns><see cref="T:System.Collections.IEnumerable&lt;Column&gt;"/></returns>
		private IEnumerable<Column> GetPreferredIdentifierColumnsForConceptType(OIALModel.OIALModel oialModel, ConceptType conceptType, bool isTopLevel, string prefix)
		{
			ICollection<InformationType> preferredIdentifierInformationTypes = GetPreferredIdentifierInformationTypes(conceptType);

			ICollection<ChildSequenceUniquenessConstraint> conceptTypeUniquenessConstraints = null;

			List<Column> columns = new List<Column>();

			if (preferredIdentifierInformationTypes == null)
			{
				conceptTypeUniquenessConstraints = GetPreferredChildSequenceUniquenessConstraints(oialModel.ChildSequenceConstraintCollection, conceptType);
				ConceptType absorbingConceptType;
				if (conceptTypeUniquenessConstraints != null)
				{
					foreach (ChildSequenceUniquenessConstraint uConstraint in conceptTypeUniquenessConstraints)
					{
						foreach (ConceptTypeChild child in uConstraint.ChildSequence.ConceptTypeChildCollection)
						{
							InformationType informationType;
							ConceptTypeRef conceptTypeRef;
							if ((informationType = child as InformationType) != null)
							{
								columns.Add(GetColumnForInformationType(informationType, isTopLevel, prefix + GetPrefix(informationType.ConceptType), false));
							}
							else if ((conceptTypeRef = child as ConceptTypeRef) != null)
							{
								columns.AddRange(GetPreferredIdentifierColumnsForConceptTypeRef(oialModel, conceptTypeRef, isTopLevel, prefix));
							}
							else
							{
								ConceptTypeAbsorbedConceptType conceptTypeAbsorbedConceptType = child as ConceptTypeAbsorbedConceptType;
								Debug.Assert(conceptTypeAbsorbedConceptType == null);

								Debug.Fail("Constraints can exist on Absorbed Concept Types...");
							}
						}
					}
				}
				else if ((absorbingConceptType = conceptType.AbsorbingConceptType) != null)
				{
					// This concept type has neither preferred identifier information types nor preferred child sequence uniqueness constraints
					columns.AddRange(GetPreferredIdentifierColumnsForConceptType(oialModel, absorbingConceptType, isTopLevel, prefix));
				}
				else
				{
					Debug.Fail("Concept Type doesn't contain a preferred uniqueness constraint and isn't nested inside of another Concept Type.");
				}
			}
			else
			{
				int count = preferredIdentifierInformationTypes.Count;
				bool prefixIsName = count == 1;
				foreach (InformationType informationType in preferredIdentifierInformationTypes)
				{
					ProcessRefName(informationType, ref prefix, ref prefixIsName);
					columns.Add(GetColumnForInformationType(informationType, isTopLevel, prefix, prefixIsName));
				}
			}
			return columns;
		}
		/// <summary>
		/// Creates and returns a <see cref="T:Neumont.Tools.ORM.Views.RelationalView.Column"/> for the passed
		/// <see cref="T:Neumont.Tools.ORM.OIALModel.InformationType"/>.
		/// </summary>
		/// <param name="informationType">An <see cref="T:Neumont.Tools.ORM.OIALModel.InformationType"/> for which a 
		/// <see cref="T:Neumont.Tools.ORM.Views.RelationalView.Column"/> should be generated.</param>
		/// <param name="isTopLevel">If the <see cref="T:Neumont.Tools.ORM.OIALModel.InformationType"/> is top level, then <see langword="true"/>.
		/// Otherwise, <see langword="false"/></param>
		/// <param name="prefix">The prefix to be prepended to the name of this <see cref="T:Neumont.Tools.ORM.Views.RelationalView.Column"/>.</param>
		/// <param name="prefixIsName">If the <paramref name="prefix"/> should be the name, then <see langword="true"/>.
		/// Otherwise, <see langword="false"/>.</param>
		/// <returns><see cref="T:Neumont.Tools.ORM.Views.RelationalView.Column"/></returns>
		private Column GetColumnForInformationType(InformationType informationType, bool isTopLevel, string prefix, bool prefixIsName)
		{
			string dataType = GetDataType(informationType.InformationTypeFormat.ValueType);
			Store theStore = Store;
			string name = CamelCase(prefixIsName ? prefix : string.Concat(prefix, informationType.Name));
			Column column = new Column(theStore,
				new PropertyAssignment(Column.NameDomainPropertyId, name),
				new PropertyAssignment(Column.IsMandatoryDomainPropertyId, isTopLevel && informationType.Mandatory == MandatoryConstraintModality.Alethic),
				new PropertyAssignment(Column.DataTypeDomainPropertyId, dataType));
			return column;
		}
		#endregion // Getting Columns
		#region Column References
		/// <summary>
		/// Gets a iterable collection of <see cref="Column"/>s which are referenced by a <see cref="UniquenessConstraint"/>.
		/// </summary>
		/// <param name="model">The <see cref="OIALModel"/> referenced by this <see cref="RelationalModel"/>.</param>
		/// <param name="conceptTypeChildren">A <see cref="LinkedElementCollection&lt;ConceptTypeChild&gt;"/> which references all
		/// the <see cref="ConceptTypeChild"/> relationships contained by a <see cref="UniquenessConstraint"/>.</param>
		/// <param name="table">The <see cref="Table"/> on which <see cref="UniquenessConstraints"/> are currently being generated.</param>
		/// <returns>IEnumerable&lt;Column&gt;</returns>
		private IEnumerable<Column> GetColumnReferences(OIALModel.OIALModel model, LinkedElementCollection<ConceptTypeChild> conceptTypeChildren, ICollection<Column> tableColumns, string prefix)
		{
			List<Column> columns = new List<Column>();
			ConceptTypeRef conceptTypeRef = null;
			ConceptTypeAbsorbedConceptType conceptTypeAbsorbedConceptType = null;
			InformationType informationType = null;

			// Iterates through the ConceptTypeChild relationships passed to this method and gets column references based on the type
			// of the ConceptTypeChild relationships.
			foreach (ConceptTypeChild conceptTypeChild in conceptTypeChildren)
			{
				if ((informationType = conceptTypeChild as InformationType) != null)
				{
					columns.Add(GetColumnRefForInformationType(columns, tableColumns, informationType, GetPrefix(informationType.ConceptType), false));
				}
				else if ((conceptTypeRef = conceptTypeChild as ConceptTypeRef) != null)
				{
					columns.AddRange(GetPreferredIdentifierColumnRefsForConceptType(tableColumns, model, conceptTypeRef.ReferencedConceptType, false, string.Concat(prefix, conceptTypeRef.Name)));
				}
				else if ((conceptTypeAbsorbedConceptType = conceptTypeChild as ConceptTypeAbsorbedConceptType) != null)
				{
					columns.AddRange(GetPreferredIdentifierColumnRefsForConceptType(tableColumns, model, conceptTypeAbsorbedConceptType.AbsorbedConceptType, false, string.Concat(prefix, conceptTypeRef.Name, "_")));
				}
			}
			return columns;
		}
		/// <summary>
		/// Retrieves an <see cref="T:System.Collections.IEnumerable&lt;T&gt;"/> of <see cref="T:Neumont.Tools.ORM.Views.RelationalView.Column"/> objects
		/// which represent the preferred identifier columns for a <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/>.
		/// </summary>
		/// <param name="tableColumns">The collection of <see cref="T:Neumont.Tools.ORM.Views.RelationalView.Column"/> objects that comprise the
		/// current <see cref="T:Neumont.Tools.ORM.Views.RelationalView.Table" /></param>
		/// <param name="oialModel">The <see cref="T:Neumont.Tools.ORM.OIALModel.OIALModel"/> which contains information about the
		/// passed <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptTypeRef"/>.</param>
		/// <param name="conceptType">The <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> whose target columns are of interest.</param>
		/// <param name="isTopLevel">If the concept type being passed is top level, then <see langword="true"/>. Otherwise, <see langword="false"/>.</param>
		/// <param name="prefix">A <see cref="T:System.String"/> which is prepended to the names of all columns generated by this method call.</param>
		/// <returns><see cref="T:System.Collections.IEnumerable&lt;Column&gt;"/></returns>
		private IEnumerable<Column> GetPreferredIdentifierColumnRefsForConceptType(ICollection<Column> tableColumns, Neumont.Tools.ORM.OIALModel.OIALModel oialModel, ConceptType conceptType, bool isTopLevel, string prefix)
		{
			List<Column> list = new List<Column>();

			ICollection<InformationType> preferredIdentifierInformationTypes = GetPreferredIdentifierInformationTypes(conceptType);

			ICollection<ChildSequenceUniquenessConstraint> conceptTypeUniquenessConstraints = null;

			if (preferredIdentifierInformationTypes == null)
			{
				conceptTypeUniquenessConstraints = GetPreferredChildSequenceUniquenessConstraints(oialModel.ChildSequenceConstraintCollection, conceptType);
				ConceptType absorbingConceptType;
				if (conceptTypeUniquenessConstraints != null)
				{
					foreach (ChildSequenceUniquenessConstraint uConstraint in conceptTypeUniquenessConstraints)
					{
						foreach (ConceptTypeChild child in uConstraint.ChildSequence.ConceptTypeChildCollection)
						{
							InformationType informationType;
							ConceptTypeRef conceptTypeRef;
							if ((informationType = child as InformationType) != null)
							{
								list.Add(GetColumnRefForInformationType(list, tableColumns, informationType, prefix + GetPrefix(informationType.ConceptType), false));
							}
							else if ((conceptTypeRef = child as ConceptTypeRef) != null)
							{
								list.AddRange(GetPreferredIdentifierColumnRefsForConceptType(tableColumns, oialModel, conceptTypeRef.ReferencedConceptType, isTopLevel, prefix));
							}
							else
							{
								ConceptTypeAbsorbedConceptType conceptTypeAbsorbedConceptType = child as ConceptTypeAbsorbedConceptType;
								Debug.Assert(conceptTypeAbsorbedConceptType == null);

								Debug.Fail("Constraints can exist on Absorbed Concept Types...");
							}
						}
					}
				}
				else if ((absorbingConceptType = conceptType.AbsorbingConceptType) != null)
				{
					// This concept type has neither preferred identifier information types nor preferred child sequence uniqueness constraints
					list.AddRange(GetPreferredIdentifierColumnRefsForConceptType(tableColumns, oialModel, conceptType.AbsorbingConceptType, isTopLevel, prefix));
				}
				else
				{
					Debug.Fail("Concept Type doesn't contain a preferred uniqueness constraint and isn't nested inside of another Concept Type.");
				}
			}
			else
			{
				int count = preferredIdentifierInformationTypes.Count;
				bool prefixIsName = count == 1;
				foreach (InformationType informationType in preferredIdentifierInformationTypes)
				{
					ProcessRefName(informationType, ref prefix, ref prefixIsName);
					list.Add(GetColumnRefForInformationType(list, tableColumns, informationType, prefix, prefixIsName));
				}
			}
			return list;
		}
		/// <summary>
		/// Returns a <see cref="T:Neumont.Tools.ORM.Views.RelationalView.Column"/> present in the passed <paramref name="tableColumns"/> object.
		/// </summary>
		/// <param name="columns">A list of columns.</param>
		/// <param name="tableColumns">A collection of <see cref="T:Neumont.Tools.ORM.Views.RelationalView.Column"/> objects that the
		/// associated <see cref="T:Neumont.Tools.ORM.Views.RelationalView.Table"/> currently contains.</param>
		/// <param name="informationType">An <see cref="T:Neumont.Tools.ORM.OIALModel.InformationType"/> for which a 
		/// <see cref="T:Neumont.Tools.ORM.Views.RelationalView.Column"/> should be generated.</param>
		/// <param name="prefix">The prefix to be prepended to the name of this <see cref="T:Neumont.Tools.ORM.Views.RelationalView.Column"/>.</param>
		/// <param name="prefixIsName">If the <paramref name="prefix"/> should be the name, then <see langword="true"/>.
		/// Otherwise, <see langword="false"/>.</param>
		/// <returns><see cref="T:Neumont.Tools.ORM.Views.RelationalView.Column"/></returns>
		private Column GetColumnRefForInformationType(ICollection<Column> columns, ICollection<Column> tableColumns, InformationType informationType, string prefix, bool prefixIsName)
		{
			string name = CamelCase(prefixIsName ? prefix : string.Concat(prefix, informationType.Name));
			foreach (Column column in tableColumns)
			{
				if (name == column.Name && !columns.Contains(column))
				{
					return column;
				}
			}
			Debug.Fail("No column found for this information type.");
			return null;

		}
		#endregion // Column References
		#region Helper Methods
		/// <summary>
		/// Gets the proper reference name to use as the name of a <see cref="T:Neumont.Tools.ORM.Views.RelationalView.Column"/>
		/// </summary>
		/// <param name="informationType">The <see cref="T:Neumont.Tools.ORM.OIALModel.InformationType"/> which is a preferred identifier
		/// for a <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptTypeRef"/>.</param>
		/// <param name="prefix">The current <see cref="T:System.String"/> designated as a prefix for the name.</param>
		/// <param name="prefixIsName">If the <paramref name="prefix"/> should be the entire name, then <see langword="true"/>.
		/// Otherwise, <see langword="false"/>.</param>
		private void ProcessRefName(InformationType informationType, ref string prefix, ref bool prefixIsName)
		{
			string informationTypeName = informationType.Name;
			if (informationTypeName.ToLower().StartsWith(prefix.ToLower()))
			{
				prefix = informationTypeName;
				prefixIsName = true;
			}
		}
		/// <summary>
		/// Retrieves a collection of <see cref="T:Neumont.Tools.ORM.OIALModel.ChildSequenceUniquenessConstraint"/> objects that relate
		/// to a <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/>.
		/// </summary>
		/// <param name="childSequenceConstraints">The ChildSequenceConstraintCollection of the <see cref="T:Neumont.Tools.ORM.OIALModel.OIALModel"/>.
		/// </param>
		/// <param name="conceptType">The <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> whose constraints are of interest.</param>
		/// <returns><see cref="System.Collections.Generics&lt;ChildSequenceUniquenessConstraint&gt;"/> if there are any in the collection.
		/// Otherwise, <see langword="null"/>.</returns>
		private ICollection<ChildSequenceUniquenessConstraint> GetPreferredChildSequenceUniquenessConstraints(LinkedElementCollection<ChildSequenceConstraint> childSequenceConstraints, ConceptType conceptType)
		{
			List<ChildSequenceUniquenessConstraint> conceptTypeUniquenessConstraints = new List<ChildSequenceUniquenessConstraint>();
			foreach (ChildSequenceConstraint childSequenceConstraint in childSequenceConstraints)
			{
				ChildSequenceUniquenessConstraint uConstraint = childSequenceConstraint as ChildSequenceUniquenessConstraint;
				if (uConstraint != null && uConstraint.IsPreferred)
				{
					bool addConstraint = false;
					foreach (ConceptTypeChild conceptTypeChild in uConstraint.ChildSequence.ConceptTypeChildCollection)
					{
						if (conceptTypeChild.Parent == conceptType)
						{
							addConstraint = true;
						}
					}
					if (addConstraint)
					{
						conceptTypeUniquenessConstraints.Add(uConstraint);
					}
				}
			}
			return conceptTypeUniquenessConstraints.Count == 0 ? null : conceptTypeUniquenessConstraints;
		}
		/// <summary>
		/// Gets the <see cref="T:Neumont.Tools.ORM.OIALModel.InformationType"/> objects that comprise the preferred identifier
		/// for the passed <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/>, if any.
		/// </summary>
		/// <param name="conceptType">The <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> whose preferred identifier
		/// is of interest.</param>
		/// <returns><see cref="T:System.Collections.Generic.IEnumerable&lt;InformationType&gt;"/>, if any exist.
		/// Otherwise, <see langword="null"/>.</returns>
		private ICollection<InformationType> GetPreferredIdentifierInformationTypes(ConceptType conceptType)
		{
			ReadOnlyCollection<ConceptTypeChild> conceptTypeInformationTypes = ConceptTypeChild.GetLinksToTargetCollection(conceptType);

			List<InformationType> preferredIdentifierInformationTypes = new List<InformationType>();
			foreach (ConceptTypeChild ctc in conceptTypeInformationTypes)
			{
				InformationType informationType = ctc as InformationType;
				if (informationType != null)
				{
					foreach (SingleChildUniquenessConstraint uConstraint in informationType.SingleChildConstraintCollection)
					{
						if (uConstraint.IsPreferred)
						{
							preferredIdentifierInformationTypes.Add(informationType);
						}
					}
				}
			}
			ConceptType absorbingConceptType;
			if (preferredIdentifierInformationTypes.Count == 0 && (absorbingConceptType = conceptType.AbsorbingConceptType) != null)
			{
				return GetPreferredIdentifierInformationTypes(absorbingConceptType);
			}
			return preferredIdentifierInformationTypes.Count == 0 ? null : preferredIdentifierInformationTypes;
		}

		private static string CamelCase(string columnName)
		{
			string[] splitStrings = columnName.Split('_');
			int count = splitStrings.Length;
			string firstWord = splitStrings[0];
			splitStrings[0] = string.Concat(firstWord[0].ToString().ToLower(), firstWord.Remove(0, 1));
			for (int i = 1; i < count; ++i)
			{
				string word = splitStrings[i];
				string firstLetter = word[0].ToString();
				splitStrings[i] = string.Concat(firstLetter.ToUpper(), word.Remove(0,1));
			}
			return string.Concat(splitStrings);
		}
		/// <summary>
		/// Retrieves the <see cref="T:Neumont.Tools.ORM.Views.RelationalView.Table"/> associated with a
		/// <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/>.
		/// </summary>
		/// <param name="conceptType">The <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> whose associated table is of interest.</param>
		/// <returns><see cref="T:Neumont.Tools.ORM.Views.RelationalView.Table"/> if it exists. Otherwise, <see langword="null"/>.</returns>
		private Table FindTable(ConceptType conceptType)
		{
			foreach (Table table in this.TableCollection)
			{
				if (table.ConceptType == conceptType)
				{
					return table;
				}
			}
			ConceptType absorbingConceptType = conceptType.AbsorbingConceptType;
			if (absorbingConceptType != null)
			{
				Table table = FindTable(absorbingConceptType);
				if (table != null)
				{
					return table;
				}
			}
			Debug.Fail("No table created for this concept type.");
			return null;
		}
		/// <summary>
		/// Gets the SQL-Server compliant data type for the <see cref="T:Neumont.Tools.ORM.ObjectModel.ObjectType"/>.
		/// </summary>
		/// <param name="valueType">The <see cref="T:Neumont.Tools.ORM.ObjectModel.ObjectType"/> whose data type is of interest.</param>
		/// <returns>The data type, a <see cref="T:System.String"/>.</returns>
		private static string GetDataType(ObjectType valueType)
		{
			return GetDataTypeInternal(valueType).ToLowerInvariant();
		}
		/// <summary>
		/// Gets the SQL-Server compliant data type for the <see cref="T:Neumont.Tools.ORM.ObjectModel.ObjectType"/>.
		/// </summary>
		/// <param name="valueType">The <see cref="T:Neumont.Tools.ORM.ObjectModel.ObjectType"/> whose data type is of interest.</param>
		/// <returns>The data type, a <see cref="T:System.String"/>.</returns>
		private static string GetDataTypeInternal(ObjectType valueType)
		{
			// A boolean data type for unaries will not have an associated value type
			if (valueType == null)
			{
				return "BIT";
			}
			DataType dataType = valueType.DataType;
			int precision = valueType.Length;
			int scale = valueType.Scale;
			if (dataType is NumericDataType || dataType is OtherDataType)
			{
				if (dataType is AutoCounterNumericDataType || dataType is OtherDataType || dataType is SignedIntegerNumericDataType || dataType is UnsignedIntegerNumericDataType || (!(dataType is FloatingPointNumericDataType) && scale <= 0))
				{
					return "BIGINT";
				}
				else if (dataType is FloatingPointNumericDataType)
				{
					return "FLOAT" + (precision > 0 ? "(" + precision + ")" : string.Empty);
				}
				else
				{
					return "DECIMAL(" + Math.Max(precision, 0) + ", " + Math.Max(scale, 0) + ")";
				}
			}
			else if (dataType is LogicalDataType)
			{
				return "BIT";
			}
			else if (dataType is VariableLengthTextDataType || dataType is LargeLengthTextDataType)
			{
				return "NVARCHAR(" + (precision <= 0 ? "MAX" : precision.ToString()) + ")";
			}
			else if (dataType is FixedLengthTextDataType)
			{
				return "NCHAR(" + (precision <= 0 ? "MAX" : precision.ToString()) + ")";
			}
			else if (dataType is RawDataDataType)
			{
				return "VARBINARY(" + (precision <= 0 ? "MAX" : precision.ToString()) + ")";
			}
			else if (dataType is TemporalDataType)
			{
				return "DATETIME";
			}
			else
			{
				return "UNSPECIFIED";
			}
		}
		/// <summary>
		/// Gets the prefix for the names of <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptTypeChild"/> objects of a
		/// <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/>.
		/// </summary>
		/// <param name="conceptType">The <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> whose prefix is of interest.</param>
		/// <returns>The prefix, a <see cref="T:System.String"/>.</returns>
		private static string GetPrefix(ConceptType conceptType)
		{
			ConceptType parentConceptType = conceptType.AbsorbingConceptType;
			if (parentConceptType != null)
			{
				return GetPrefix(parentConceptType) + conceptType.Name + "_";
			}
			return string.Empty;
		}

		#endregion // Helper Methods
	}

	internal partial class RelationalDiagram
	{
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
	}
}