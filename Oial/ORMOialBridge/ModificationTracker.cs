using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.Modeling;
using System.Diagnostics;
using System.Collections;
using Neumont.Tools.ORMAbstraction;
using System.Collections.ObjectModel;

namespace Neumont.Tools.ORMToORMAbstractionBridge
{
	public partial class AbstractionModelIsForORMModel
	{
		#region Regeneration rule delay validation methods
		private static partial class ModificationTracker
		{
			#region ORM modification rule methods
			/// <summary>
			/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole)
			/// </summary>
			private static void ConstraintRoleAddedRule(ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = (ConstraintRoleSequenceHasRole)e.ModelElement;
				IConstraint constraint = link.ConstraintRoleSequence.Constraint;
				if (IsRelevantConstraint(constraint))
				{
					FactTypeConstraintPatternChanged(link.Role.FactType);
				}
				// UNDONE: Incremental uniqueness changes
			}
			/// <summary>
			/// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole)
			/// </summary>
			private static void ConstraintRoleDeletedRule(ElementDeletedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = (ConstraintRoleSequenceHasRole)e.ModelElement;
				IConstraint constraint = link.ConstraintRoleSequence.Constraint;
				if (IsRelevantConstraint(constraint))
				{
					FactTypeConstraintPatternChanged(link.Role.FactType);
				}
				// UNDONE: Incremental uniqueness changes
			}
			/// <summary>
			/// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectType)
			/// </summary>
			private static void ObjectTypeChangedRule(ElementPropertyChangedEventArgs e)
			{
				Guid propertyId = e.DomainProperty.Id;
				if (propertyId == ObjectType.IsIndependentDomainPropertyId)
				{
					SignificantObjectTypeChange((ObjectType)e.ModelElement);
				}
				else if (propertyId == ObjectType.NameDomainPropertyId)
				{
					string newName = (string)e.NewValue;
					ObjectType objectType = (ObjectType)e.ModelElement;
					ConceptType conceptType = ConceptTypeIsForObjectType.GetConceptType(objectType);
					if (null != conceptType)
					{
						conceptType.Name = newName;
					}
					InformationTypeFormat informationTypeFormat = InformationTypeFormatIsForValueType.GetInformationTypeFormat(objectType);
					if (null != informationTypeFormat)
					{
						informationTypeFormat.Name = newName;
					}
				}
			}
			/// <summary>
			/// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.SetConstraint)
			/// </summary>
			private static void SetConstraintChangedRule(ElementPropertyChangedEventArgs e)
			{
				Guid propertyId = e.DomainProperty.Id;
				if (propertyId == SetConstraint.ModalityDomainPropertyId)
				{
					SetConstraint constraint = (SetConstraint)e.ModelElement;
					if (IsRelevantConstraint(constraint.Constraint))
					{
						foreach (FactType factType in constraint.FactTypeCollection)
						{
							FactTypeConstraintPatternChanged(factType);
						}
					}
				}
				else if (propertyId == SetConstraint.NameDomainPropertyId)
				{
					UniquenessConstraint uniquenessConstraint;
					Uniqueness uniqueness;
					if (null != (uniquenessConstraint = e.ModelElement as UniquenessConstraint) &&
						null != (uniqueness = UniquenessIsForUniquenessConstraint.GetUniqueness(uniquenessConstraint)))
					{
						uniqueness.Name = uniquenessConstraint.Name;
					}
				}
			}
			/// <summary>
			/// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.ORMModel)
			/// Synchronize the <see cref="P:AbstractionModel.Name">name</see> of the <see cref="AbstractionModel"/>
			/// with the <see cref="P:ORMModel.Name">name</see> of the <see cref="ORMModel"/>
			/// </summary>
			private static void ORMModelChangedRule(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == ORMModel.NameDomainPropertyId)
				{
					ORMModel model = (ORMModel)e.ModelElement;
					AbstractionModel abstractionModel = AbstractionModelIsForORMModel.GetAbstractionModel(model);
					if (abstractionModel != null)
					{
						abstractionModel.Name = model.Name;
					}
				}
			}
			#endregion // ORM modification rule methods
			#region Bridge deletion rule methods
			/// <summary>
			/// DeleteRule: typeof(ConceptTypeIsForObjectType)
			/// </summary>
			private static void ConceptTypeBridgeDetachedRule(ElementDeletedEventArgs e)
			{
				ConceptType conceptType = ((ConceptTypeIsForObjectType)e.ModelElement).ConceptType;
				if (!conceptType.IsDeleted &&
					!RebuildingAbstractionModel(conceptType.Model))
				{
					AddTransactedModelElement(conceptType, ModelElementModification.AbstractionElementDetached);
				}
			}
			/// <summary>
			/// DeleteRule: typeof(InformationTypeFormatIsForValueType)
			/// </summary>
			private static void InformationTypeFormatBridgeDetachedRule(ElementDeletedEventArgs e)
			{
				InformationTypeFormat format = ((InformationTypeFormatIsForValueType)e.ModelElement).InformationTypeFormat;
				if (!format.IsDeleted &&
					!RebuildingAbstractionModel(format.Model))
				{
					AddTransactedModelElement(format, ModelElementModification.AbstractionElementDetached);
				}
			}
			/// <summary>
			/// DeleteRule: typeof(ConceptTypeChildHasPathFactType)
			/// </summary>
			private static void ConceptTypeChildPathBridgeDetachedRule(ElementDeletedEventArgs e)
			{
				ConceptTypeChild child = ((ConceptTypeChildHasPathFactType)e.ModelElement).ConceptTypeChild;
				ConceptType conceptType = child.Parent;
				if (conceptType != null &&
					!conceptType.IsDeleted &&
					!RebuildingAbstractionModel(conceptType.Model))
				{
					AddTransactedModelElement(conceptType, ModelElementModification.AbstractionElementDetached);
				}
			}
			#endregion // Bridge deletion rule methods
			#region General validation helper methods
			private static bool IsRelevantConstraint(IConstraint constraint)
			{
				if (constraint != null)
				{
					switch (constraint.ConstraintType)
					{
						case ConstraintType.InternalUniqueness:
						case ConstraintType.ImpliedMandatory:
						case ConstraintType.SimpleMandatory:
							return true;
					}
				}
				return false;
			}
			/// <summary>
			/// The constraint pattern for a <see cref="FactType"/> may have changed
			/// </summary>
			private static void FactTypeConstraintPatternChanged(FactType factType)
			{
				if (factType != null &&
					!factType.IsDeleted &&
					!ORMElementGateway.IsElementExcluded(factType))
				{
					FrameworkDomainModel.DelayValidateElement(factType, FactTypeConstraintPatternChangedDelayed);
				}
			}
			private static void FactTypeConstraintPatternChangedDelayed(ModelElement element)
			{
				FactType factType;
				if (!element.IsDeleted &&
					!ORMElementGateway.IsElementExcluded(factType = (FactType)element))
				{
					// If we're not previously mapped, then we will have been added at this point
					FactTypeMapsTowardsRole mapToRole = FactTypeMapsTowardsRole.GetLinkToTowardsRole(factType);
					if (mapToRole != null)
					{
						MappingMandatoryPattern startMandatoryPattern = mapToRole.MandatoryPattern;
						if (mapToRole.SynchronizeMappingPatterns())
						{
							MappingMandatoryPattern endMandatoryPattern = mapToRole.MandatoryPattern;
							if (endMandatoryPattern != startMandatoryPattern)
							{
								// UNDONE: Propagate IsMandatory changes directly to the model
							}
						}
						else
						{
							AddTransactedModelElement(factType, ModelElementModification.ORMElementChanged);
							FrameworkDomainModel.DelayValidateElement(factType.Model, DelayValidateModel);
						}
					}
				}
			}
			private static void SignificantFactTypeChange(FactType factType)
			{
				if (factType != null &&
					!factType.IsDeleted &&
					!ORMElementGateway.IsElementExcluded(factType))
				{
					FrameworkDomainModel.DelayValidateElement(factType, SignificantFactTypeChangeDelayed);
				}
			}
			private static void SignificantFactTypeChangeDelayed(ModelElement element)
			{
				FactType factType;
				ORMModel model;
				if (!element.IsDeleted &&
					!ORMElementGateway.IsElementExcluded(factType = (FactType)element) &&
					null != (model = factType.Model))
				{
					Objectification objectification = factType.Objectification;
					if (objectification != null)
					{
						foreach (FactType impliedFactType in objectification.ImpliedFactTypeCollection)
						{
							AddTransactedModelElement(impliedFactType, ModelElementModification.ORMElementChanged);
							FrameworkDomainModel.DelayValidateElement(model, DelayValidateModel);
						}
					}
					else
					{
						AddTransactedModelElement(factType, ModelElementModification.ORMElementChanged);
						FrameworkDomainModel.DelayValidateElement(model, DelayValidateModel);
					}
				}
			}
			private static void SignificantObjectTypeChange(ObjectType objectType)
			{
				if (objectType != null &&
					!objectType.IsDeleted &&
					!ORMElementGateway.IsElementExcluded(objectType))
				{
					FrameworkDomainModel.DelayValidateElement(objectType, SignificantObjectTypeChangeDelayed);
				}
			}
			private static void SignificantObjectTypeChangeDelayed(ModelElement element)
			{
				ObjectType objectType;
				ORMModel model;
				if (!element.IsDeleted &&
					!ORMElementGateway.IsElementExcluded(objectType = (ObjectType)element) &&
					null != (model = objectType.Model))
				{
					AddTransactedModelElement(objectType, ModelElementModification.ORMElementChanged);
					FrameworkDomainModel.DelayValidateElement(objectType.Model, DelayValidateModel);
				}
			}
			#endregion // General validation helper methods
		}
		#endregion // Regeneration rule delay validation methods
	}
}