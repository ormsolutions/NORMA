#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright � Neumont University. All rights reserved.                     *
* Copyright � ORM Solutions, LLC. All rights reserved.                     *
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
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;
using System.Diagnostics;
using ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge;
using ORMSolutions.ORMArchitect.ORMAbstraction;
using ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase;
using ORMCore = ORMSolutions.ORMArchitect.Core.ObjectModel;
using System.Linq;

namespace ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge
{
	partial class ORMAbstractionToConceptualDatabaseBridgeDomainModel
	{
		#region Regeneration rule delay validation methods
		private static partial class ModificationTracker
		{
			#region Abstraction model modification rules
			// For now, the abstraction model is fully regenerated whenever
			// an potential mapping change occurs. If there were any
			// concept types in the model they will be deleted and regenerated,
			// so it is currently sufficient to listen to a limited number of changes
			// and fully regenerate below the schema. The added rules are only needed
			// for cases where we start from an empty ORM model

			/// <summary>
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.ORMAbstraction.ConceptTypeChild)
			/// </summary>
			private static void ConceptTypeChildChangedRule(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == ConceptTypeChild.IsMandatoryDomainPropertyId)
				{
					ValidateAssociatedColumnsIsNullable((ConceptTypeChild)e.ModelElement);
				}
			}
			/// <summary>
			/// AddRule: typeof(ORMSolutions.ORMArchitect.ORMAbstraction.AbstractionModelHasConceptType)
			/// </summary>
			private static void ConceptTypeAddedRule(ElementAddedEventArgs e)
			{
				RebuildAbstractionModel(((AbstractionModelHasConceptType)e.ModelElement).Model);
			}
			/// <summary>
			/// DeleteRule: typeof(ORMSolutions.ORMArchitect.ORMAbstraction.AbstractionModelHasConceptType)
			/// </summary>
			private static void ConceptTypeDeletedRule(ElementDeletedEventArgs e)
			{
				RebuildAbstractionModel(((AbstractionModelHasConceptType)e.ModelElement).Model);
			}
			/// <summary>
			/// AddRule: typeof(ORMSolutions.ORMArchitect.ORMAbstraction.AbstractionModelHasInformationTypeFormat)
			/// </summary>
			private static void InformationTypeFormatAddedRule(ElementAddedEventArgs e)
			{
				RebuildAbstractionModel(((AbstractionModelHasInformationTypeFormat)e.ModelElement).Model);
			}
			/// <summary>
			/// DeleteRule: typeof(ORMSolutions.ORMArchitect.ORMAbstraction.AbstractionModelHasInformationTypeFormat)
			/// </summary>
			private static void InformationTypeFormatDeletedRule(ElementDeletedEventArgs e)
			{
				RebuildAbstractionModel(((AbstractionModelHasInformationTypeFormat)e.ModelElement).Model);
			}
			/// <summary>
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.ORMAbstraction.InverseConceptTypeChild)
			/// </summary>
			private static void InverseConceptTypeChildChangedRule(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == InverseConceptTypeChild.PairIsMandatoryDomainPropertyId)
				{
					InverseConceptTypeChild childLink = (InverseConceptTypeChild)e.ModelElement;
					ValidateAssociatedColumnsIsNullable(childLink.PositiveChild);
					ValidateAssociatedColumnsIsNullable(childLink.NegativeChild);
				}
			}
			/// <summary>
			/// AddRule: typeof(AssimilationMappingCustomizesFactType)
			/// </summary>
			private static void AssimilationMappingAddedRule(ElementAddedEventArgs e)
			{
				ORMCore.ORMModel ormModel = ((AssimilationMappingCustomizesFactType)e.ModelElement).FactType.Model;
				if (ormModel != null)
				{
					RebuildAbstractionModel(AbstractionModelIsForORMModel.GetAbstractionModel(ormModel));
				}
			}
			/// <summary>
			/// ChangeRule: typeof(AssimilationMapping)
			/// </summary>
			private static void AssimilationMappingChangedRule(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == AssimilationMapping.AbsorptionChoiceDomainPropertyId)
				{
					ORMCore.FactType factType;
					ORMCore.ORMModel ormModel;
					if (null != (factType = ((AssimilationMapping)e.ModelElement).FactType) &&
						null != (ormModel = factType.Model))
					{
						RebuildAbstractionModel(AbstractionModelIsForORMModel.GetAbstractionModel(ormModel));
					}
				}
			}
			private static void RebuildAbstractionModel(AbstractionModel model)
			{
				if (model != null &&
					!model.IsDeleted)
				{
					FrameworkDomainModel.DelayValidateElement(model, RebuildForAbstractionModelDelayed);
				}
			}
			/// <summary>
			/// DeleteRule: typeof(UniquenessConstraintIsForUniqueness)
			/// Propagate deletion of a Uniqueness
			/// </summary>
			private static void UniquenessDeletedRule(ElementDeletedEventArgs e)
			{
				UniquenessConstraint constraint = ((UniquenessConstraintIsForUniqueness)e.ModelElement).UniquenessConstraint;
				if (!constraint.IsDeleted)
				{
					constraint.Delete();
				}
			}
			/// <summary>
			/// RolePlayerPositionChangeRule: typeof(ORMSolutions.ORMArchitect.ORMAbstraction.UniquenessIncludesConceptTypeChild)
			/// Reorder the columns in a uniqueness constraint when the order changes
			/// </summary>
			private static void UniquenessConstraintRoleOrderChanged(RolePlayerOrderChangedEventArgs e)
			{
				Uniqueness uniqueness;
				LinkedElementCollection<UniquenessConstraint> constraints;
				int constraintCount;
				if (null != (uniqueness = e.SourceElement as Uniqueness) &&
					e.SourceDomainRole.Id == UniquenessIncludesConceptTypeChild.UniquenessDomainRoleId &&
					0 != (constraintCount = (constraints = UniquenessConstraintIsForUniqueness.GetUniquenessConstraint(uniqueness)).Count))
				{
					LinkedElementCollection<ConceptTypeChild> conceptTypeChildren = uniqueness.ConceptTypeChildCollection;
					int conceptTypeChildCount = conceptTypeChildren.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						LinkedElementCollection<Column> constraintColumns = constraints[i].ColumnCollection;
						int constraintColumnCount = constraintColumns.Count;
						if (constraintColumnCount == conceptTypeChildCount)
						{
							constraintColumns.Move(e.OldOrdinal, e.NewOrdinal);
						}
						else
						{
							// UNDONE: The question of exactly how many columns are associated with a reference to
							// a conceptTypeChild is highly non-trivial. Punt on the issue for now by regenerating
							// the model.
							ConceptType conceptType = uniqueness.ConceptType;
							if (conceptType != null)
							{
								RebuildAbstractionModel(conceptType.Model);
							}
						}
					}
				}
			}
			/// <summary>
			/// DeletingRule: typeof(ORMSolutions.ORMArchitect.ORMAbstraction.UniquenessIncludesConceptTypeChild)
			/// Remove a column in a uniqueness constraint when a concept type child is removed from the
			/// corresponding abstraction construct
			/// </summary>
			private static void UniquenessConstraintRoleDeleting(ElementDeletingEventArgs e)
			{
				UniquenessIncludesConceptTypeChild link = (UniquenessIncludesConceptTypeChild)e.ModelElement;
				Uniqueness uniqueness = link.Uniqueness;
				LinkedElementCollection<UniquenessConstraint> constraints;
				int constraintCount;
				if (!uniqueness.IsDeleting &&
					!link.ConceptTypeChild.IsDeleting &&
					0 != (constraintCount = (constraints = UniquenessConstraintIsForUniqueness.GetUniquenessConstraint(uniqueness)).Count))
				{
					int removeAtIndex = UniquenessIncludesConceptTypeChild.GetLinksToConceptTypeChildCollection(uniqueness).IndexOf(link);
					for (int i = 0; i < constraintCount; ++i)
					{
						constraints[i].ColumnCollection.RemoveAt(removeAtIndex);
					}
				}
			}
			[DelayValidatePriority(DomainModelType = typeof(AbstractionDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void RebuildForAbstractionModelDelayed(ModelElement element)
			{
				if (!element.IsDeleted)
				{
					AbstractionModel model = (AbstractionModel)element;
					Schema schema = SchemaIsForAbstractionModel.GetSchema(model);
					if (schema != null)
					{
						// Clear any customizations to stop rules and events from modifying
						// the customizations during rebuild.
						SchemaCustomization initialCustomization = SchemaCustomization.SetCustomization(schema, null);
						schema.TableCollection.Clear();
						schema.DomainCollection.Clear();
						FullyGenerateConceptualDatabaseModel(schema, model, initialCustomization, null);
						SchemaCustomization.SetCustomization(schema, new SchemaCustomization(schema));
					}
				}
			}
			private static void ValidateAssociatedColumnsIsNullable(ConceptTypeChild child)
			{
				foreach (Column column in ColumnHasConceptTypeChild.GetColumn(child))
				{
					FrameworkDomainModel.DelayValidateElement(column, ValidateColumnIsNullableDelayed);
				}
			}
			[DelayValidatePriority(10, DomainModelType = typeof(AbstractionDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void ValidateColumnIsNullableDelayed(ModelElement element)
			{
				// Check if the element survived regeneration. Note priority is after RebuildForAbstractionModelDelayed
				if (!element.IsDeleted)
				{
					UpdateColumnNullability((Column)element);
				}
			}
			#endregion // Abstraction model modification rules
			#region Bridge element modification rules
			/// <summary>
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.ORMAbstraction.AbstractionModel)
			/// Update the schema name when the abstraction model name changes
			/// </summary>
			private static void AbstractionModelChangedRule(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == AbstractionModel.NameDomainPropertyId)
				{
					AbstractionModel abstractionModel = (AbstractionModel)e.ModelElement;
					Schema schema = SchemaIsForAbstractionModel.GetSchema(abstractionModel);
					if (schema != null &&
						!schema.CustomName)
					{
						schema.Name = abstractionModel.Name;
					}
				}
			}
			#endregion // Bridge element modification rules
			#region Name modification rules
			/// <summary>
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.Column)
			/// </summary>
			private static void ColumnChangedRule(ElementPropertyChangedEventArgs e)
			{
				Guid propertyId = e.DomainProperty.Id;
				Column column;
				Table table;
				Schema schema;
				SchemaCustomization customization;
				if (propertyId == Column.IsNullableDomainPropertyId)
				{
					column = (Column)e.ModelElement;
					if (null != (table = column.Table) &&
						table.ColumnOrder != ColumnOrdering.Custom)
					{
						ValidateTableNameChanged(table);
					}
				}
				else if (propertyId == Column.CustomNameDomainPropertyId)
				{
					column = (Column)e.ModelElement;
					if (null != (table = column.Table) &&
						null != (schema = table.Schema) &&
						null != (customization = SchemaCustomization.GetCustomization(schema)))
					{
						customization.CustomizeColumnName(column, (bool)e.NewValue ? column.Name : null);
						ValidateSchemaNamesChanged(schema);
					}
				}
				else if (propertyId == Column.NameDomainPropertyId)
				{
					column = (Column)e.ModelElement;
					if (column.CustomName &&
						null != (table = column.Table) &&
						null != (schema = table.Schema) &&
						null != (customization = SchemaCustomization.GetCustomization(schema)))
					{
						customization.CustomizeColumnName(column, (string)e.NewValue);
						ValidateSchemaNamesChanged(schema);
					}
				}
			}
			/// <summary>
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.Table)
			/// </summary>
			private static void TableChangedRule(ElementPropertyChangedEventArgs e)
			{
				Guid propertyId = e.DomainProperty.Id;
				Table table;
				Schema schema;
				SchemaCustomization customization;
				if (propertyId == Table.CustomNameDomainPropertyId)
				{
					table = (Table)e.ModelElement;
					if (null != (schema = table.Schema) &&
						null != (customization = SchemaCustomization.GetCustomization(schema)))
					{
						customization.CustomizeTableName(table, (bool)e.NewValue ? table.Name : null);
						ValidateSchemaNamesChanged(schema);
					}
				}
				else if (propertyId == Table.NameDomainPropertyId)
				{
					table = (Table)e.ModelElement;
					if (table.CustomName &&
						null != (schema = table.Schema) &&
						null != (customization = SchemaCustomization.GetCustomization(schema)))
					{
						customization.CustomizeTableName(table, (string)e.NewValue);
						ValidateSchemaNamesChanged(schema);
					}
				}
				else if (propertyId == Table.ColumnOrderDomainPropertyId)
				{
					table = (Table)e.ModelElement;
					if (null != (schema = table.Schema) &&
						null != (customization = SchemaCustomization.GetCustomization(schema)))
					{
						if (((ColumnOrdering)e.NewValue == ColumnOrdering.Custom) ^ ((ColumnOrdering)e.OldValue == ColumnOrdering.Custom))
						{
							FrameworkDomainModel.DelayValidateElement(table, DelayValidateCustomizeColumnPositions);
						}
						ValidateSchemaNamesChanged(schema);
					}
				}
			}
			/// <summary>
			/// RolePlayerPositionChangeRule: typeof(ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.TableContainsColumn)
			/// </summary>
			private static void ColumnOrderChangedRule(RolePlayerOrderChangedEventArgs e)
			{
				if (e.SourceDomainRole.Id == TableContainsColumn.TableDomainRoleId)
				{
					Table table = (Table)e.SourceElement;
					if (table.ColumnOrder == ColumnOrdering.Custom)
					{
						FrameworkDomainModel.DelayValidateElement(table, DelayValidateCustomizeColumnPositions);
					}
				}
			}
			/// <summary>
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.Schema)
			/// </summary>
			private static void SchemaChangedRule(ElementPropertyChangedEventArgs e)
			{
				Guid propertyId = e.DomainProperty.Id;
				Schema schema;
				SchemaCustomization customization;
				if (propertyId == Schema.DefaultColumnOrderDomainPropertyId)
				{
					// Note that this does not affect custom ordering, so there is nothing to
					// record in the schema customizations
					ValidateSchemaNamesChanged((Schema)e.ModelElement);
				}
				else if (propertyId == Schema.CustomNameDomainPropertyId)
				{
					schema = (Schema)e.ModelElement;
					if (null != (customization = SchemaCustomization.GetCustomization(schema)))
					{
						customization.CustomizeSchemaName(schema, (bool)e.NewValue ? schema.Name : null);
						// Don't use the full ValidateSchemaNamesChanged, which rebuilds the customization
						// object and regenerates all names.
						FrameworkDomainModel.DelayValidateElement(schema, DelayValidateSchemaNameChanged);
					}
				}
				else if (propertyId == Schema.NameDomainPropertyId)
				{
					schema = (Schema)e.ModelElement;
					if (schema.CustomName &&
						null != (customization = SchemaCustomization.GetCustomization(schema)))
					{
						customization.CustomizeSchemaName(schema, (string)e.NewValue);
						// Don't use the full ValidateSchemaNamesChanged, which rebuilds the customization
						// object and regenerates all names.
						FrameworkDomainModel.DelayValidateElement(schema, DelayValidateSchemaNameChanged);
					}
				}
			}
			/// <summary>
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.ORMAbstraction.ConceptType)
			/// </summary>
			private static void ConceptTypeChangedRule(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == ConceptType.NameDomainPropertyId)
				{
					ValidateTableNameChanged(TableIsPrimarilyForConceptType.GetTable((ConceptType)e.ModelElement));
				}
			}
			/// <summary>
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactType)
			/// </summary>
			private static void FactTypeNameChangedRule(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == ORMCore.FactType.NameChangedDomainPropertyId)
				{
					FactTypeNamePartChanged((ORMCore.FactType)e.ModelElement);
				}
			}
			/// <summary>
			/// AddRule: typeof(DefaultReferenceModeNamingCustomizesORMModel)
			/// </summary>
			private static void DefaultReferenceModeNamingCustomizesORMModelAddedRule(ElementAddedEventArgs e)
			{
				ValidateDefaultReferenceModeNamingChanged(e.ModelElement as DefaultReferenceModeNamingCustomizesORMModel);
			}
			/// <summary>
			/// ChangeRule: typeof(ReferenceModeNaming)
			/// </summary>
			private static void DefaultReferenceModeNamingChangedRule(ElementPropertyChangedEventArgs e)
			{
				ValidateDefaultReferenceModeNamingChanged(DefaultReferenceModeNamingCustomizesORMModel.GetLinkToORMModel(e.ModelElement as RelationalDefaultReferenceModeNaming));
			}
			/// <summary>
			/// AddRule: typeof(ReferenceModeNamingCustomizesObjectType)
			/// </summary>
			private static void ReferenceModeNamingCustomizesObjectTypeAddedRule(ElementAddedEventArgs e)
			{
				ValidateReferenceModeNamingChanged(e.ModelElement as ReferenceModeNamingCustomizesObjectType);
			}
			/// <summary>
			/// ChangeRule: typeof(DefaultReferenceModeNaming)
			/// </summary>
			private static void ReferenceModeNamingChangedRule(ElementPropertyChangedEventArgs e)
			{
				ValidateReferenceModeNamingChanged(ReferenceModeNamingCustomizesObjectType.GetLinkToObjectType(e.ModelElement as RelationalReferenceModeNaming));
			}
			private static void ValidateDefaultReferenceModeNamingChanged(DefaultReferenceModeNamingCustomizesORMModel defaultReferenceModeNamingCustomizesORMModel)
			{
				if (null != defaultReferenceModeNamingCustomizesORMModel)
				{
					ORMCore.ORMModel model = defaultReferenceModeNamingCustomizesORMModel.ORMModel;
					if (null != model)
					{
						foreach (ConceptType conceptType in AbstractionModelHasConceptType.GetConceptTypeCollection(
								AbstractionModelIsForORMModel.GetAbstractionModel(model)))
						{
							ValidateConceptTypeNameChanged(conceptType);
						}
					}
				}
			}
			private static void ValidateReferenceModeNamingChanged(ReferenceModeNamingCustomizesObjectType referenceModeNamingCustomizesObjectType)
			{
				if (null != referenceModeNamingCustomizesObjectType)
				{
					ORMCore.ObjectType objectType = referenceModeNamingCustomizesObjectType.ObjectType;
					if (objectType != null)
					{
						ConceptType conceptType = ConceptTypeIsForObjectType.GetConceptType(objectType);
						if (null == conceptType)
						{
							foreach (ORMCore.Role role in objectType.PlayedRoleCollection)
							{
								foreach (ConceptTypeChild conceptTypeChild in ConceptTypeChildHasPathFactType.GetConceptTypeChild(role.FactType))
								{
									ValidateConceptTypeChildNameChanged(conceptTypeChild);
								}
							}
						}
						else
						{
							ValidateConceptTypeNameChanged(conceptType);
						}
					}
				}
			}
			/// <summary>
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ReferenceModeKind)
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.CustomReferenceModeKind)
			/// </summary>
			private static void ReferenceModeChangedRule(ElementPropertyChangedEventArgs e)
			{
				Guid propertyId = e.DomainProperty.Id;
				if (propertyId == ORMCore.ReferenceModeKind.FormatStringDomainPropertyId ||
					propertyId == ORMCore.CustomReferenceMode.CustomFormatStringDomainPropertyId)
				{
					ReferenceModeSettingsChanged(e.ModelElement.Store);
				}
			}
			/// <summary>
			/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ReferenceModeHasReferenceModeKind)
			/// </summary>
			private static void ReferenceModeKindAddedRule(ElementAddedEventArgs e)
			{
				ReferenceModeSettingsChanged(e.ModelElement.Store);
			}
			/// <summary>
			/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ReferenceModeHasReferenceModeKind)
			/// </summary>
			private static void ReferenceModeKindDeletedRule(ElementDeletedEventArgs e)
			{
				ReferenceModeSettingsChanged(e.ModelElement.Store);
			}
			/// <summary>
			/// RolePlayerChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ReferenceModeHasReferenceModeKind)
			/// </summary>
			private static void ReferenceModeKindRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			{
				if (e.DomainRole.Id == ORMCore.ReferenceModeHasReferenceModeKind.KindDomainRoleId)
				{
					ReferenceModeSettingsChanged(e.ElementLink.Store);
				}
			}
			private static void ReferenceModeSettingsChanged(Store store)
			{
				foreach (Schema schema in store.ElementDirectory.FindElements<Schema>(true))
				{
					ValidateSchemaNamesChanged(schema);
				}
			}
			private static void ValidateConceptTypeNameChanged(ConceptType conceptType)
			{
				if (null != conceptType)
				{
					FrameworkDomainModel.DelayValidateElement(conceptType, DelayValidateConceptTypeNameChanged);
				}
			}
			[DelayValidatePriority(20, DomainModelType = typeof(AbstractionDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void DelayValidateConceptTypeNameChanged(ModelElement element)
			{
				if (!element.IsDeleted)
				{
					ConceptType conceptType = (ConceptType)element;
					Table table1 = TableIsPrimarilyForConceptType.GetTable(conceptType);
					if (null != table1)
					{
						ValidateSchemaNamesChanged(table1.Schema);
					}
					foreach (Table table2 in TableIsAlsoForConceptType.GetTable(conceptType))
					{
						ValidateSchemaNamesChanged(table2.Schema);
					}
				}
			}
			/// <summary>
			/// Helper function to resolve an existing absorbed assimilations from an object type.
			/// </summary>
			private static IEnumerable<ConceptTypeAssimilatesConceptType> AbsorbedAssimilationsFromObjectType(ORMCore.ObjectType objectType)
			{
				ConceptType conceptType;
				if (null != objectType &&
					null != (conceptType = ConceptTypeIsForObjectType.GetConceptType(objectType)))
				{
					foreach (ConceptTypeAssimilatesConceptType assimilation in ConceptTypeAssimilatesConceptType.GetLinksToAssimilatorConceptTypeCollection(conceptType))
					{
						if (AssimilationMapping.GetAbsorptionChoiceFromAssimilation(assimilation) == AssimilationAbsorptionChoice.Absorb)
						{
							yield return assimilation;
						}
					}
				}
			}

			/// <summary>
			/// Determine which absorbed assilimations need the existence of an absorption indicator column to be validated
			/// </summary>
			private static void DelayValidateAbsorptionIndicator(ORMCore.ConstraintRoleSequenceHasRole roleLink)
			{
				ORMCore.MandatoryConstraint constraint;
				if (null != (constraint = roleLink.ConstraintRoleSequence as ORMCore.MandatoryConstraint) &&
					constraint.Modality == ORMCore.ConstraintModality.Alethic)
				{
					// Mandatory roles can span different object types and affect different columns, so we
					// need to test all of the roles, not just the modified one.
					ORMCore.Role initialRole = roleLink.Role;
					ORMCore.ObjectType initialRolePlayer = roleLink.Role.RolePlayer;
					if (constraint.IsDeleted || constraint.IsDeleting)
					{
						if (initialRolePlayer != null)
						{
							foreach (ConceptTypeAssimilatesConceptType assimilation in AbsorbedAssimilationsFromObjectType(initialRolePlayer))
							{
								foreach (Table table in TableIsAlsoForConceptType.GetTable(assimilation.AssimilatedConceptType))
								{
									FrameworkDomainModel.DelayValidateElementPair(assimilation, table, DelayValidateAbsorptionIndicator);
								}
							}
						}
					}
					else
					{
						foreach (ORMCore.Role constrainedRole in constraint.RoleCollection)
						{
							ORMCore.ObjectType rolePlayer = constrainedRole.RolePlayer;
							if (constrainedRole == initialRole || initialRolePlayer != rolePlayer) // Some duplicate processing is possible, but unusual enough we don't care
							{
								foreach (ConceptTypeAssimilatesConceptType assimilation in AbsorbedAssimilationsFromObjectType(rolePlayer))
								{
									foreach (Table table in TableIsAlsoForConceptType.GetTable(assimilation.AssimilatedConceptType))
									{
										FrameworkDomainModel.DelayValidateElementPair(assimilation, table, DelayValidateAbsorptionIndicator);
									}
								}
							}
						}
					}
				}
			}
			/// <summary>
			/// Determine which absorbed assilimations need the existence of an absorption indicator column to be validated
			/// </summary>
			private static void DelayValidateAbsorptionIndicator(ORMCore.MandatoryConstraint constraint)
			{
				ORMCore.ObjectType firstRolePlayer = null;
				foreach (ORMCore.Role constrainedRole in constraint.RoleCollection)
				{
					ORMCore.ObjectType rolePlayer = constrainedRole.RolePlayer;
					if (firstRolePlayer == null)
					{
						firstRolePlayer = rolePlayer;
					}
					else if (rolePlayer == firstRolePlayer)
					{
						continue;
					}
					foreach (ConceptTypeAssimilatesConceptType assimilation in AbsorbedAssimilationsFromObjectType(rolePlayer))
					{
						foreach (Table table in TableIsAlsoForConceptType.GetTable(assimilation.AssimilatedConceptType))
						{
							FrameworkDomainModel.DelayValidateElementPair(assimilation, table, DelayValidateAbsorptionIndicator);
						}
					}
				}
			}
			/// <summary>
			/// Delay validate incrementable existence of absorption indicator columns. element1 is an
			/// assimilation, element2 is a Table. Including the table as a pair lets us test if the
			/// table has been deleted with the full regeneration independently of what happens with the
			/// absorption model. This manages absorption indicator columns and inverse relationships
			/// on columns mapped to unary fact types.
			/// </summary>
			[DelayValidatePriority(5, DomainModelType = typeof(AbstractionDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void DelayValidateAbsorptionIndicator(ModelElement element1, ModelElement element2)
			{
				if (element1.IsDeleted || element2.IsDeleted)
				{
					return;
				}

				FrameworkDomainModel.DelayValidateElement(element1, DelayValidateAbsorptionIndicator);
			}
			/// <summary>
			/// The second half, with assimilations passed through from <see cref="DelayValidateAbsorptionIndicator(ModelElement, ModelElement)"/>.
			/// </summary>
			[DelayValidatePriority(6, DomainModelType = typeof(AbstractionDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void DelayValidateAbsorptionIndicator(ModelElement element)
			{
				ConceptTypeAssimilatesConceptType assimilation = (ConceptTypeAssimilatesConceptType)element;
				// Note that an absorption choice change triggers regeneration, so we do not need to verify that
				// this is still absorbed.
				bool isSelfEvident = AssimilationMapping.AssimilationIsSelfEvident(assimilation) == AssimilationMapping.AssimilationEvidence.MandatoryEvidence;
				InverseConceptTypeChild inverseChildLink = null;
				ConceptTypeChild inverseChild = null;
				if (!assimilation.RefersToSubtype)
				{
					if (null != (inverseChildLink = InverseConceptTypeChild.GetLinkToNegativeInverseChild(assimilation) ?? InverseConceptTypeChild.GetLinkToPositiveInverseChild(assimilation)))
					{
						inverseChild = inverseChildLink.PositiveChild; // guess
						if (inverseChild == assimilation)
						{
							inverseChild = inverseChildLink.NegativeChild; // guessed wrong
						}
					}
				}

				if (inverseChild != null)
				{
					if (inverseChild is InformationType)
					{
						// This is placed as the inverse of the information, which means this half of the
						// negatable unary is objectified but the other is not.
						foreach (ColumnHasConceptTypeChild columnLink in ColumnHasConceptTypeChild.GetLinksToColumn(inverseChild))
						{
							ColumnHasInverseConceptTypeChild inverseColumnLink = ColumnHasInverseConceptTypeChild.GetLinkToInverseConceptTypeChild(columnLink);
							if (inverseColumnLink != null)
							{
								if (isSelfEvident)
								{
									inverseColumnLink.Delete();
								}
								// else we already have what we need
							}
							else
							{
								columnLink.InverseConceptTypeChild = assimilation;
								FrameworkDomainModel.DelayValidateElement(columnLink.Column, ValidateColumnIsNullableDelayed);
								ValidateSchemaNamesChanged(columnLink.Column.Table?.Schema);
							}
						}
					}
					else
					{
						// Both the positive and negative ends are assimilations. With dynamic cases either end can be bound to
						// the column or the inverse. It is likely these will be the same for all tables (multiple are possible with
						// partitioning) that use this, but we shouldn't rely on this, so each table is tracked separately.
						LinkedElementCollection<Table> tables = TableIsAlsoForConceptType.GetTable(assimilation.AssimilatedConceptType);
						int tableCount;
						if (0 != (tableCount = tables.Count))
						{
							int remainingTableCount = tableCount;
							Table table;
							Column column;
							int tableIndex;
							BitTracker tableHandled = new BitTracker(tableCount);

							// See if this is an existing inverse
							foreach (ColumnHasInverseConceptTypeChild inverseConceptTypeLink in ColumnHasInverseConceptTypeChild.GetLinksToInverseChildNode(assimilation))
							{
								column = inverseConceptTypeLink.ColumnChildNode.Column;
								table = column?.Table;
								if (-1 != (tableIndex = tables.IndexOf(table)))
								{
									if (isSelfEvident)
									{
										// Reaonly collection, OK to delete
										inverseConceptTypeLink.Delete();
										FrameworkDomainModel.DelayValidateElement(column, ValidateColumnIsNullableDelayed);
										// Removing the inverse won't change the column name
									}
									// Otherwise, we're handled for this table (column inversion exists)

									if (0 == --remainingTableCount)
									{
										break;
									}
									tableHandled[tableIndex] = true;
								}
							}

							if (remainingTableCount != 0)
							{
								foreach (ColumnHasConceptTypeChild inverseColumnLink in ColumnHasConceptTypeChild.GetLinksToColumn(inverseChild).Where(link => link.AbsorptionIndicator))
								{
									column = inverseColumnLink.Column;
									table = column.Table;
									if (-1 != (tableIndex = tables.IndexOf(table)))
									{
										if (isSelfEvident)
										{
											if (inverseColumnLink.InverseConceptTypeChild != null)
											{
												inverseColumnLink.InverseConceptTypeChild = null;
												FrameworkDomainModel.DelayValidateElement(column, ValidateColumnIsNullableDelayed);
											}
										}
										else if (inverseColumnLink.InverseConceptTypeChild == null)
										{
											inverseColumnLink.InverseConceptTypeChild = assimilation;
											FrameworkDomainModel.DelayValidateElement(column, ValidateColumnIsNullableDelayed);
										}

										if (0 == --remainingTableCount)
										{
											break;
										}
										tableHandled[tableIndex] = true;
									}
								}

								if (remainingTableCount != 0)
								{
									foreach (ColumnHasConceptTypeChild columnLink in ColumnHasConceptTypeChild.GetLinksToColumn(assimilation).Where(link => link.AbsorptionIndicator))
									{
										column = columnLink.Column;
										table = column.Table;
										if (-1 != (tableIndex = tables.IndexOf(table)))
										{
											if (isSelfEvident)
											{
												// If there is an inverse, delete it and move the concept type to this link. Otherwise,
												// delete the column.
												ColumnHasInverseConceptTypeChild inverseColumnLink = ColumnHasInverseConceptTypeChild.GetLinkToInverseConceptTypeChild(columnLink);
												if (inverseColumnLink != null)
												{
													columnLink.ConceptTypeChild = inverseColumnLink.InverseConceptTypeChild;
													inverseColumnLink.Delete();
												}
												else
												{
													columnLink.Delete();
												}
												FrameworkDomainModel.DelayValidateElement(column, ValidateColumnIsNullableDelayed);
											}

											if (0 == --remainingTableCount)
											{
												break;
											}
											tableHandled[tableIndex] = true;
										}
									}

									if (remainingTableCount != 0 && !isSelfEvident)
									{
										for (int i = 0; i < tableCount; ++i)
										{
											if (!tableHandled[i])
											{
												CreateDynamicAbsorptionIndicatorColumn(assimilation, tables[i]);
												if (--remainingTableCount == 0)
												{
													break;
												}
											}
										}
									}
								}
							}
						}
					}
				}
				else if (isSelfEvident)
				{
					foreach (ColumnHasConceptTypeChild columnLink in ColumnHasConceptTypeChild.GetLinksToColumn(assimilation).Where(link => link.AbsorptionIndicator))
					{
						columnLink.Column.Delete();
					}
				}
				else
				{
					IList<Table> handledTables = ColumnHasConceptTypeChild.GetLinksToColumn(assimilation)
						.Where(link => link.AbsorptionIndicator)
						.Select(link => link.Column.Table)
						.Where(t => t != null)
						.ToList();
					foreach (Table table in TableIsAlsoForConceptType.GetTable(assimilation.AssimilatedConceptType).Where(t => handledTables.IndexOf(t) == -1))
					{
						CreateDynamicAbsorptionIndicatorColumn(assimilation, table);
					}
				}
			}
			private static void CreateDynamicAbsorptionIndicatorColumn(ConceptTypeAssimilatesConceptType assimilation, Table table)
			{
				Column column = new Column(
					assimilation.Partition,
					new PropertyAssignment(Column.NameDomainPropertyId, assimilation.AssimilatedConceptType.Name)
				);
				new TableContainsColumn(table, column);

				TableIsAlsoForConceptType secondaryType = TableIsAlsoForConceptType.GetLink(table, assimilation.AssimilatedConceptType);
				if (secondaryType != null)
				{
					ColumnHasConceptTypeChild lastLink = null;
					foreach (ConceptTypeAssimilatesConceptType pathAssimilation in secondaryType.AssimilationPath)
					{
						lastLink = new ColumnHasConceptTypeChild(column, pathAssimilation);
					}
					lastLink.AbsorptionIndicator = true;
				}

				FrameworkDomainModel.DelayValidateElement(column, ValidateColumnIsNullableDelayed);
				ValidateSchemaNamesChanged(table.Schema);
			}
			/// <summary>
			/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole)
			/// Dynamically track absorption indicator columns
			/// </summary>
			private static void MandatoryRoleAddedRule(ElementAddedEventArgs e)
			{
				DelayValidateAbsorptionIndicator((ORMCore.ConstraintRoleSequenceHasRole)e.ModelElement);
			}
			/// <summary>
			/// DeletedRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole)
			/// </summary>
			private static void MandatoryRoleDeletedRule(ElementDeletedEventArgs e)
			{
				ORMCore.ConstraintRoleSequenceHasRole link = (ORMCore.ConstraintRoleSequenceHasRole)e.ModelElement;
				ORMCore.Role role = link.Role;
				if (role.IsDeleted)
				{
					return;
				}

				DelayValidateAbsorptionIndicator(link);
			}
			/// <summary>
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.MandatoryConstraint)
			/// </summary>
			private static void MandatoryChangedRule(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == ORMCore.SetConstraint.ModalityDomainPropertyId)
				{
					DelayValidateAbsorptionIndicator((ORMCore.MandatoryConstraint)e.ModelElement);
				}
			}
			/// <summary>
			/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasDerivationRule)
			/// </summary>
			private static void FactTypeDerivationRuleAddedRule(ElementAddedEventArgs e)
			{
				ORMCore.FactType factType = ((ORMCore.FactTypeHasDerivationRule)e.ModelElement).FactType;
				ORMCore.ObjectType objectifyingType = factType.NestingType;
				if (objectifyingType != null)
				{
					foreach (ConceptTypeAssimilatesConceptType assimilation in AbsorbedAssimilationsFromObjectType(objectifyingType))
					{
						foreach (Table table in TableIsAlsoForConceptType.GetTable(assimilation.AssimilatedConceptType))
						{
							FrameworkDomainModel.DelayValidateElementPair(assimilation, table, DelayValidateAbsorptionIndicator);
						}
					}
				}
			}
			/// <summary>
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeDerivationRule)
			/// </summary>
			private static void FactTypeDerivationRuleChangedRule(ElementPropertyChangedEventArgs e)
			{
				Guid propertyId = e.DomainProperty.Id;
				ORMCore.ObjectType objectifyingType;
				if ((propertyId == ORMCore.FactTypeDerivationRule.DerivationCompletenessDomainPropertyId || propertyId == ORMCore.FactTypeDerivationRule.DerivationStorageDomainPropertyId) &&
					null != (objectifyingType = ((ORMCore.FactTypeDerivationRule)e.ModelElement)?.FactType?.NestingType))
				{
					foreach (ConceptTypeAssimilatesConceptType assimilation in AbsorbedAssimilationsFromObjectType(objectifyingType))
					{
						foreach (Table table in TableIsAlsoForConceptType.GetTable(assimilation.AssimilatedConceptType))
						{
							FrameworkDomainModel.DelayValidateElementPair(assimilation, table, DelayValidateAbsorptionIndicator);
						}
					}
				}
			}
			/// <summary>
			/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasDerivationRule)
			/// </summary>
			private static void FactTypeDerivationRuleDeletedRule(ElementDeletedEventArgs e)
			{
				ORMCore.FactType factType = ((ORMCore.FactTypeHasDerivationRule)e.ModelElement).FactType;
				ORMCore.ObjectType objectifyingType;
				if (!factType.IsDeleted &&
					null != (objectifyingType = factType.NestingType))
				{
					foreach (ConceptTypeAssimilatesConceptType assimilation in AbsorbedAssimilationsFromObjectType(objectifyingType))
					{
						foreach (Table table in TableIsAlsoForConceptType.GetTable(assimilation.AssimilatedConceptType))
						{
							FrameworkDomainModel.DelayValidateElementPair(assimilation, table, DelayValidateAbsorptionIndicator);
						}
					}
				}
			}
			/// <summary>
			/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeHasDerivationRule)
			/// </summary>
			private static void SubtypeDerivationRuleAddedRule(ElementAddedEventArgs e)
			{
				foreach (ConceptTypeAssimilatesConceptType assimilation in AbsorbedAssimilationsFromObjectType(((ORMCore.SubtypeHasDerivationRule)e.ModelElement).Subtype))
				{
					foreach (Table table in TableIsAlsoForConceptType.GetTable(assimilation.AssimilatedConceptType))
					{
						FrameworkDomainModel.DelayValidateElementPair(assimilation, table, DelayValidateAbsorptionIndicator);
					}
				}
			}
			/// <summary>
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeDerivationRule)
			/// </summary>
			private static void SubtypeDerivationRuleChangedRule(ElementPropertyChangedEventArgs e)
			{
				Guid propertyId = e.DomainProperty.Id;
				if (propertyId == ORMCore.SubtypeDerivationRule.DerivationCompletenessDomainPropertyId || propertyId == ORMCore.SubtypeDerivationRule.DerivationStorageDomainPropertyId)
				{
					foreach (ConceptTypeAssimilatesConceptType assimilation in AbsorbedAssimilationsFromObjectType(((ORMCore.SubtypeDerivationRule)e.ModelElement).Subtype))
					{
						foreach (Table table in TableIsAlsoForConceptType.GetTable(assimilation.AssimilatedConceptType))
						{
							FrameworkDomainModel.DelayValidateElementPair(assimilation, table, DelayValidateAbsorptionIndicator);
						}
					}
				}
			}
			/// <summary>
			/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeHasDerivationRule)
			/// </summary>
			private static void SubtypeDerivationRuleDeletedRule(ElementDeletedEventArgs e)
			{
				ORMCore.ObjectType subtype = ((ORMCore.SubtypeHasDerivationRule)e.ModelElement).Subtype;
				if (!subtype.IsDeleted)
				{
					foreach (ConceptTypeAssimilatesConceptType assimilation in AbsorbedAssimilationsFromObjectType(subtype))
					{
						foreach (Table table in TableIsAlsoForConceptType.GetTable(assimilation.AssimilatedConceptType))
						{
							FrameworkDomainModel.DelayValidateElementPair(assimilation, table, DelayValidateAbsorptionIndicator);
						}
					}
				}
			}
			/// <summary>
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.NameGenerator)
			/// Regenerate names when settings change
			/// </summary>
			private static void NameGeneratorSettingsChangedRule(ElementPropertyChangedEventArgs e)
			{
				ORMCore.NameGenerator generator = (ORMCore.NameGenerator)e.ModelElement;
				Store store = generator.Store;
				if (store.DomainDataDirectory.GetDomainClass(RelationalNameGenerator.DomainClassId).IsDerivedFrom(generator.GetDomainClass()))
				{
					foreach (Schema schema in store.ElementDirectory.FindElements<Schema>(true))
					{
						ValidateSchemaNamesChanged(schema);
					}
				}
			}
			/// <summary>
			/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasAbbreviation)
			/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.RecognizedPhraseHasAbbreviation)
			/// Regenerate names when an abbreviation is added
			/// </summary>
			private static void AbbreviationAddedRule(ElementAddedEventArgs e)
			{
				ORMCore.ElementHasAlias link = (ORMCore.ElementHasAlias)e.ModelElement;
				Store store = link.Store;
				if (store.DomainDataDirectory.GetDomainClass(RelationalNameGenerator.DomainClassId).IsDerivedFrom(link.Alias.NameConsumerDomainClass))
				{
					foreach (Schema schema in store.ElementDirectory.FindElements<Schema>(true))
					{
						ValidateSchemaNamesChanged(schema);
					}
				}
			}
			/// <summary>
			/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasAbbreviation)
			/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.RecognizedPhraseHasAbbreviation)
			/// Regenerate names when an abbreviation is deleted
			/// </summary>
			private static void AbbreviationDeletedRule(ElementDeletedEventArgs e)
			{
				ORMCore.ElementHasAlias link = (ORMCore.ElementHasAlias)e.ModelElement;
				ORMCore.NameAlias alias;
				if (!link.Element.IsDeleted &&
					null == (alias = ((ORMCore.ElementHasAlias)link).Alias).RefinedInstance)
				{
					Store store = link.Store;
					if (store.DomainDataDirectory.GetDomainClass(RelationalNameGenerator.DomainClassId).IsDerivedFrom(link.Alias.NameConsumerDomainClass))
					{
						foreach (Schema schema in store.ElementDirectory.FindElements<Schema>(true))
						{
							ValidateSchemaNamesChanged(schema);
						}
					}
				}
			}
			/// <summary>
			/// DeletingRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelContainsRecognizedPhrase)
			/// Regenerate names when a recognized phrase with relational-targeted aliases is deleting
			/// </summary>
			private static void RecognizedPhraseDeletingRule(ElementDeletingEventArgs e)
			{
				ORMCore.ModelContainsRecognizedPhrase link = (ORMCore.ModelContainsRecognizedPhrase)e.ModelElement;
				Store store = link.Store;
				DomainClassInfo relationalInfo = store.DomainDataDirectory.GetDomainClass(RelationalNameGenerator.DomainClassId);
				foreach (ORMCore.NameAlias alias in link.RecognizedPhrase.AbbreviationCollection)
				{
					if (relationalInfo.IsDerivedFrom(alias.NameConsumerDomainClass))
					{
						foreach (Schema schema in store.ElementDirectory.FindElements<Schema>(true))
						{
							ValidateSchemaNamesChanged(schema);
						}
						break;
					}
				}
			}
			/// <summary>
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.NameAlias)
			/// Regenerate names when an abbreviation is changed
			/// </summary>
			private static void AbbreviationChangedRule(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == ORMCore.NameAlias.NameDomainPropertyId)
				{
					ORMCore.NameAlias abbreviation = (ORMCore.NameAlias)e.ModelElement;
					Store store = abbreviation.Store;
					if (store.DomainDataDirectory.GetDomainClass(RelationalNameGenerator.DomainClassId).IsDerivedFrom(abbreviation.NameConsumerDomainClass))
					{
						foreach (Schema schema in store.ElementDirectory.FindElements<Schema>(true))
						{
							ValidateSchemaNamesChanged(schema);
						}
					}
				}
			}
			/// <summary>
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.RecognizedPhrase)
			/// Regenerate names whan an associated expression to match is modified
			/// </summary>
			private static void RecognizedPhraseChangedRule(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == ORMCore.RecognizedPhrase.NameDomainPropertyId)
				{
					ORMCore.RecognizedPhrase phrase = (ORMCore.RecognizedPhrase)e.ModelElement;
					Store store = phrase.Store;
					DomainClassInfo baseClassInfo = store.DomainDataDirectory.GetDomainClass(RelationalNameGenerator.DomainClassId);
					foreach (ORMCore.NameAlias abbreviation in phrase.AbbreviationCollection)
					{
						if (baseClassInfo.IsDerivedFrom(abbreviation.NameConsumerDomainClass))
						{
							foreach (Schema schema in store.ElementDirectory.FindElements<Schema>(true))
							{
								ValidateSchemaNamesChanged(schema);
							}
							break;
						}
					}
				}
			}
			/// <summary>
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Role)
			/// </summary>
			private static void RoleNameChangedRule(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == ORMCore.Role.NameDomainPropertyId)
				{
					ORMCore.FactType factType = ((ORMCore.Role)e.ModelElement).FactType;
					if (null != factType)
					{
						FactTypeNamePartChanged(factType);
					}
				}
			}
			private static void FactTypeNamePartChanged(ORMCore.FactType factType)
			{
				bool checkPrimaryFactType = true;
				ORMCore.Objectification objectification = factType.Objectification;
				if (null != objectification)
				{
					foreach (ORMCore.FactType impliedFactType in objectification.ImpliedFactTypeCollection)
					{
						foreach (ConceptTypeChild child in ConceptTypeChildHasPathFactType.GetConceptTypeChild(impliedFactType))
						{
							ValidateConceptTypeChildNameChanged(child);
						}
					}
					checkPrimaryFactType = factType.UnaryRole != null;
				}
				if (checkPrimaryFactType)
				{
					foreach (ConceptTypeChild child in ConceptTypeChildHasPathFactType.GetConceptTypeChild(factType))
					{
						ValidateConceptTypeChildNameChanged(child);
					}
				}
			}
			private static void ValidateConceptTypeChildNameChanged(ConceptTypeChild child)
			{
				if (null != child)
				{
					FrameworkDomainModel.DelayValidateElement(child, DelayValidateConceptTypeChildNameChanged);
				}
			}
			[DelayValidatePriority(20, DomainModelType = typeof(AbstractionDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void DelayValidateConceptTypeChildNameChanged(ModelElement element)
			{
				if (!element.IsDeleted)
				{
					ConceptTypeChild child = (ConceptTypeChild)element;
					foreach (Column column in ColumnHasConceptTypeChild.GetColumn(child))
					{
						ValidateSchemaNamesChanged(column.Table.Schema);
					}
				}
			}
			private static void ValidateTableNameChanged(Table table)
			{
				if (null != table)
				{
					FrameworkDomainModel.DelayValidateElement(table, DelayValidateTableNameChanged);
				}
			}
			[DelayValidatePriority(20, DomainModelType = typeof(AbstractionDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void DelayValidateTableNameChanged(ModelElement element)
			{
				if (!element.IsDeleted)
				{
					ValidateSchemaNamesChanged(((Table)element).Schema);
				}
			}
			private static void ValidateSchemaNamesChanged(Schema schema)
			{
				if (null != schema)
				{
					FrameworkDomainModel.DelayValidateElement(schema, DelayValidateSchemaNamesChanged);
				}
			}
			[DelayValidatePriority(0, DomainModelType = typeof(AbstractionDomainModel), Order = DelayValidatePriorityOrder.BeforeDomainModel)]
			private static void DelayValidateCustomizeColumnPositions(ModelElement element)
			{
				if (!element.IsDeleted)
				{
					Table table = (Table)element;
					Schema schema;
					SchemaCustomization customization;
					if (null != (schema = table.Schema) &&
						null != (customization = SchemaCustomization.GetCustomization(schema)))
					{
						customization.CustomizeColumnPositions(table, table.ColumnOrder == ColumnOrdering.Custom);
					}
				}
			}
			[DelayValidatePriority(30, DomainModelType = typeof(AbstractionDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void DelayValidateSchemaNamesChanged(ModelElement element)
			{
				if (!element.IsDeleted)
				{
					Schema schema = (Schema)element;
					// Disable customization tracking during name generation, and
					// reset customizations on completion.
					SchemaCustomization customization = SchemaCustomization.SetCustomization(schema, null);
					NameGeneration.GenerateAllNames(schema, customization);
					SchemaCustomization.SetCustomization(schema, new SchemaCustomization(schema));
				}
			}
			/// <summary>
			/// A lightweight version of the previous routine that just updates
			/// the schema name instead of all names.
			/// </summary>
			[DelayValidatePriority(30, DomainModelType = typeof(AbstractionDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void DelayValidateSchemaNameChanged(ModelElement element)
			{
				if (!element.IsDeleted)
				{
					Schema schema = (Schema)element;
					// Disable customization tracking during any name change to
					// avoid redundant processing.
					SchemaCustomization customization = SchemaCustomization.SetCustomization(schema, null);
					AbstractionModel abstractionModel;
					string schemaName;
					if (customization != null &&
						null != (schemaName = customization.CustomizedSchemaName))
					{
						schema.Name = schemaName;
					}
					else if (null != (abstractionModel = SchemaIsForAbstractionModel.GetAbstractionModel(schema)))
					{
						schema.Name = abstractionModel.Name;
					}
					if (customization != null)
					{
						// Don't regenerate the customization object for a schema name change,
						// just use the previous settings.
						SchemaCustomization.SetCustomization(schema, customization);
					}
				}
			}
#endregion // Name modification rules
		}
#endregion // Regeneration rule delay validation methods
	}
}