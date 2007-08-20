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
			/// <summary>
			/// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.SubtypeFact)
			/// Changing the <see cref="P:SubtypeFact.IsPrimary"/> property on a <see cref="SubtypeFact"/>
			/// can modify preferred identification schemes.
			/// </summary>
			private static void SubtypeFactChangedRule(ElementPropertyChangedEventArgs e)
			{
				// UNDONE: Incremental changes, propagate changes to Uniqueness.IsPreferred property
				if (e.DomainProperty.Id == SubtypeFact.IsPrimaryDomainPropertyId)
				{
					SignificantFactTypeChange((FactType)e.ModelElement);
				}
			}
			/// <summary>
			/// RolePlayerChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier)
			/// Changing the preferred identifier for an <see cref="ObjectType"/> is considered to
			/// be a significant change until we support full incremental tracking.
			/// </summary>
			private static void PreferredIdentifierRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			{
				// UNDONE: Incremental changes, propagate changes to Uniqueness.IsPreferred property
				EntityTypeHasPreferredIdentifier link = (EntityTypeHasPreferredIdentifier)e.ElementLink;
				if (e.DomainRole.Id == EntityTypeHasPreferredIdentifier.PreferredIdentifierForDomainRoleId)
				{
					SignificantObjectTypeChange((ObjectType)e.OldRolePlayer);
				}
				SignificantObjectTypeChange(link.PreferredIdentifierFor);
			}
			/// <summary>
			/// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier)
			/// If an <see cref="ObjectType"/> is alive after gateway processing when its preferred identifier
			/// is deleted then it needs to be reprocessed.
			/// </summary>
			private static void PreferredIdentifierDeletedRule(ElementDeletedEventArgs e)
			{
				// UNDONE: Incremental If this gets passed the gateway and is not excluded,
				// then it changed from an EntityType to a ValueType
				ObjectType objectType = ((EntityTypeHasPreferredIdentifier)e.ModelElement).PreferredIdentifierFor;
				if (!objectType.IsDeleted)
				{
					SignificantObjectTypeChange(objectType);
				}
			}
			/// <summary>
			/// RolePlayerChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole)
			/// Revalidate the model when the <see cref="ObjectType">role player</see> of a <see cref="Role"/>
			/// is changed.
			/// </summary>
			private static void RolePlayerRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			{
				// UNDONE: Incremental changes will not be as severe here. Note that adding
				// and deleting role players already triggers the correct actions in the
				// gateway rules. However, a change where none of the parties are excluded
				// simply needs to regenerate for now.
				ObjectTypePlaysRole link = (ObjectTypePlaysRole)e.ElementLink;
				if (e.DomainRole.Id == ObjectTypePlaysRole.PlayedRoleDomainRoleId)
				{
					SignificantFactTypeChange(((Role)e.OldRolePlayer).FactType);
				}
				else
				{
					SignificantObjectTypeChange((ObjectType)e.OldRolePlayer);
				}
				SignificantObjectTypeChange(link.RolePlayer);
				SignificantFactTypeChange(link.PlayedRole.FactType);
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
					TestRebuildAbstractionModel(conceptType.Model))
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
					TestRebuildAbstractionModel(format.Model))
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
					TestRebuildAbstractionModel(conceptType.Model))
				{
					AddTransactedModelElement(conceptType, ModelElementModification.AbstractionElementDetached);
				}
			}
			/// <summary>
			/// DeleteRule: typeof(UniquenessIsForUniquenessConstraint)
			/// </summary>
			private static void UniquenessBridgeDetachedRule(ElementDeletedEventArgs e)
			{
				Uniqueness uniqueness = ((UniquenessIsForUniquenessConstraint)e.ModelElement).Uniqueness;
				if (!uniqueness.IsDeleted)
				{
					// We don't want to rebuild the model for this case. Any significant
					// change that affects the absorption pattern will remove it, and
					// there are no cases where we need to keep it. Just propagate.
					uniqueness.Delete();
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
			[DelayValidatePriority(DomainModelType=typeof(ORMCoreDomainModel), Order=DelayValidatePriorityOrder.AfterDomainModel)]
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
								foreach (ConceptTypeChild child in ConceptTypeChildHasPathFactType.GetConceptTypeChild(factType))
								{
									ValidateMandatory(child, startMandatoryPattern, endMandatoryPattern);
								}
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
			/// <summary>
			/// The <see cref="ConceptTypeChild.IsMandatory">IsMandatory</see> setting may
			/// have change, revalidate if necessary.
			/// </summary>
			/// <param name="child">The <see cref="ConceptTypeChild"/> element to validate</param>
			/// <param name="oldMandatory">The old mandatory pattern for any <see cref="FactType"/> in the path.</param>
			/// <param name="newMandatory">The new mandatory pattern for any <see cref="FactType"/> in the path.</param>
			private static void ValidateMandatory(ConceptTypeChild child, MappingMandatoryPattern oldMandatory, MappingMandatoryPattern newMandatory)
			{
				// We pre filter this and don't both to notify unless a change is possible
				bool mightHaveChanged = false;
				if (child.IsMandatory)
				{
					switch (oldMandatory)
					{
						case MappingMandatoryPattern.BothRolesMandatory:
						case MappingMandatoryPattern.TowardsRoleMandatory:
							switch (newMandatory)
							{
								case MappingMandatoryPattern.OppositeRoleMandatory:
								case MappingMandatoryPattern.NotMandatory:
									mightHaveChanged = true;
									break;
							}
							break;
					}
				}
				else
				{
					switch (oldMandatory)
					{
						case MappingMandatoryPattern.NotMandatory:
						case MappingMandatoryPattern.OppositeRoleMandatory:
							switch (newMandatory)
							{
								case MappingMandatoryPattern.BothRolesMandatory:
								case MappingMandatoryPattern.TowardsRoleMandatory:
									mightHaveChanged = true;
									break;
							}
							break;
					}
				}
				if (mightHaveChanged)
				{
					FrameworkDomainModel.DelayValidateElement(child, ValidateMandatoryDelayed);
				}
			}
			[DelayValidatePriority(ValidationPriority.ValidateMandatory, DomainModelType = typeof(ORMCoreDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void ValidateMandatoryDelayed(ModelElement element)
			{
				if (!element.IsDeleted)
				{
					ConceptTypeChild child = (ConceptTypeChild)element;
					bool newMandatory = true;
					foreach (FactType factType in ConceptTypeChildHasPathFactType.GetPathFactTypeCollection(child))
					{
						FactTypeMapsTowardsRole towardsRoleLink = FactTypeMapsTowardsRole.GetLinkToTowardsRole(factType);
						if (null == towardsRoleLink)
						{
							newMandatory = false;
							break;
						}
						else
						{
							switch (towardsRoleLink.MandatoryPattern)
							{
								case MappingMandatoryPattern.None:
								case MappingMandatoryPattern.NotMandatory:
								case MappingMandatoryPattern.OppositeRoleMandatory:
									newMandatory = false;
									break;
							}
							if (!newMandatory)
							{
								break;
							}
						}
					}
					child.IsMandatory = newMandatory;
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
			[DelayValidatePriority(DomainModelType = typeof(ORMCoreDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
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
			[DelayValidatePriority(DomainModelType = typeof(ORMCoreDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
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