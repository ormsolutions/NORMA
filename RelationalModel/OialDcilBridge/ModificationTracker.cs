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
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.Modeling;
using System.Diagnostics;
using System.Collections;
using Neumont.Tools.ORMToORMAbstractionBridge;
using Neumont.Tools.ORMAbstraction;
using Neumont.Tools.RelationalModels.ConceptualDatabase;
using System.Collections.ObjectModel;
using ORMCore = Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge
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
			/// ChangeRule: typeof(Neumont.Tools.ORMAbstraction.ConceptTypeChild)
			/// </summary>
			private static void ConceptTypeChildChangedRule(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == ConceptTypeChild.IsMandatoryDomainPropertyId)
				{
					ValidateAssociatedColumnsIsNullable((ConceptTypeChild)e.ModelElement);
				}
			}
			/// <summary>
			/// AddRule: typeof(Neumont.Tools.ORMAbstraction.AbstractionModelHasConceptType)
			/// </summary>
			private static void ConceptTypeAddedRule(ElementAddedEventArgs e)
			{
				RebuildAbstractionModel(((AbstractionModelHasConceptType)e.ModelElement).Model);
			}
			/// <summary>
			/// DeleteRule: typeof(Neumont.Tools.ORMAbstraction.AbstractionModelHasConceptType)
			/// </summary>
			private static void ConceptTypeDeletedRule(ElementDeletedEventArgs e)
			{
				RebuildAbstractionModel(((AbstractionModelHasConceptType)e.ModelElement).Model);
			}
			/// <summary>
			/// AddRule: typeof(Neumont.Tools.ORMAbstraction.AbstractionModelHasInformationTypeFormat)
			/// </summary>
			private static void InformationTypeFormatAddedRule(ElementAddedEventArgs e)
			{
				RebuildAbstractionModel(((AbstractionModelHasInformationTypeFormat)e.ModelElement).Model);
			}
			/// <summary>
			/// DeleteRule: typeof(Neumont.Tools.ORMAbstraction.AbstractionModelHasInformationTypeFormat)
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
			[DelayValidatePriority(DomainModelType = typeof(AbstractionDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void RebuildForAbstractionModelDelayed(ModelElement element)
			{
				if (!element.IsDeleted)
				{
					AbstractionModel model = (AbstractionModel)element;
					Schema schema = SchemaIsForAbstractionModel.GetSchema(model);
					if (schema != null)
					{
						schema.TableCollection.Clear();
						schema.DomainCollection.Clear();
						FullyGenerateConceptualDatabaseModel(schema, model, null);
					}
				}
			}
			private static void ValidateAssociatedColumnsIsNullable(ConceptTypeChild child)
			{
				bool canBeNullable = !child.IsMandatory;
				foreach (Column column in ColumnHasConceptTypeChild.GetColumn(child))
				{
					if (canBeNullable ? !column.IsNullable : column.IsNullable)
					{
						FrameworkDomainModel.DelayValidateElement(column, ValidateColumnIsNullableDelayed);
					}
				}
			}
			[DelayValidatePriority(10, DomainModelType = typeof(AbstractionDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void ValidateColumnIsNullableDelayed(ModelElement element)
			{
				// Check if the element survived regeneration. Note priority is after RebuildForAbstractionModelDelayed
				if (!element.IsDeleted)
				{
					CheckColumnConstraint((Column)element);
				}
			}
			#endregion // Abstraction model modification rules
			#region Bridge element modification rules
			/// <summary>
			/// ChangeRule: typeof(Neumont.Tools.ORMAbstraction.AbstractionModel)
			/// Update the schema name when the abstraction model name changes
			/// </summary>
			private static void AbstractionModelChangedRule(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == AbstractionModel.NameDomainPropertyId)
				{
					AbstractionModel abstractionModel = (AbstractionModel)e.ModelElement;
					Schema schema = SchemaIsForAbstractionModel.GetSchema(abstractionModel);
					if (schema != null)
					{
						schema.Name = abstractionModel.Name;
					}
				}
			}
			#endregion // Bridge element modification rules
			#region Name modification rules
			/// <summary>
			/// ChangeRule: typeof(Neumont.Tools.ORMAbstraction.ConceptType)
			/// </summary>
			private static void ConceptTypeChangedRule(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == ConceptType.NameDomainPropertyId)
				{
					ValidateTableNameChanged(TableIsPrimarilyForConceptType.GetTable((ConceptType)e.ModelElement));
				}
			}
			/// <summary>
			/// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.FactType)
			/// </summary>
			private static void FactTypeNameChangedRule(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == ORMCore.FactType.NameChangedDomainPropertyId)
				{
					foreach (ConceptTypeChild child in ConceptTypeChildHasPathFactType.GetConceptTypeChild((ORMCore.FactType)e.ModelElement))
					{
						ValidateConceptTypeChildNameChanged(child);
					}
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
			/// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.NameGenerator)
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
			/// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.Role)
			/// </summary>
			private static void RoleNameChangedRule(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == ORMCore.Role.NameDomainPropertyId)
				{
					ORMCore.FactType factType = ((ORMCore.Role)e.ModelElement).FactType;
					if (null != factType)
					{
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
						}
						else
						{
							foreach (ConceptTypeChild child in ConceptTypeChildHasPathFactType.GetConceptTypeChild(factType))
							{
								ValidateConceptTypeChildNameChanged(child);
							}
						}
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
			[DelayValidatePriority(30, DomainModelType = typeof(AbstractionDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void DelayValidateSchemaNamesChanged(ModelElement element)
			{
				if (!element.IsDeleted)
				{
					NameGeneration.GenerateAllNames((Schema)element);
				}
			}
			#endregion // Name modification rules
		}
		#endregion // Regeneration rule delay validation methods
	}
}