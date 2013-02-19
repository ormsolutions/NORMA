#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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
using System.Text;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;
using System.Diagnostics;
using System.Collections;
using ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge;
using ORMSolutions.ORMArchitect.ORMAbstraction;
using ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase;
using System.Collections.ObjectModel;
using ORMCore = ORMSolutions.ORMArchitect.Core.ObjectModel;

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
				bool canBeNullable = !child.IsMandatory;
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
				ValidateDefaultReferenceModeNamingChanged(DefaultReferenceModeNamingCustomizesORMModel.GetLinkToORMModel(e.ModelElement as DefaultReferenceModeNaming));
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
				ValidateReferenceModeNamingChanged(ReferenceModeNamingCustomizesObjectType.GetLinkToObjectType(e.ModelElement as ReferenceModeNaming));
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
				if (!link.Element.IsDeleted)
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