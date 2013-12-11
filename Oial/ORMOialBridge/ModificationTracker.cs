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
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework;
using System.Diagnostics;
using System.Collections;
using ORMSolutions.ORMArchitect.ORMAbstraction;
using System.Collections.ObjectModel;

namespace ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge
{
	public partial class AbstractionModelIsForORMModel
	{
		#region Regeneration rule delay validation methods
		private static partial class ModificationTracker
		{
			#region ORM modification rule methods
			/// <summary>
			/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole)
			/// </summary>
			private static void ConstraintRoleAddedRule(ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = (ConstraintRoleSequenceHasRole)e.ModelElement;
				IConstraint constraint = link.ConstraintRoleSequence.Constraint;
				if (constraint != null &&
					!(constraint is IHasAlternateOwner))
				{
					if (IsRelevantConstraint(constraint))
					{
						FactTypeConstraintPatternChanged(link.Role.BinarizedFactType);
					}
					switch (constraint.ConstraintType)
					{
						// UNDONE: Incremental uniqueness changes
						case ConstraintType.InternalUniqueness:
						case ConstraintType.ExternalUniqueness:
							SignificantUniquenessConstraintChange((UniquenessConstraint)constraint);
							SignificantObjectTypeChange(constraint.PreferredIdentifierFor);
							break;
					}
				}
			}
			/// <summary>
			/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole)
			/// </summary>
			private static void ConstraintRoleDeletedRule(ElementDeletedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = (ConstraintRoleSequenceHasRole)e.ModelElement;
				IConstraint constraint = link.ConstraintRoleSequence.Constraint;
				if (constraint != null &&
					!(constraint is IHasAlternateOwner))
				{
					if (IsRelevantConstraint(constraint))
					{
						FactTypeConstraintPatternChanged(link.Role.BinarizedFactType);
					}
					switch (constraint.ConstraintType)
					{
						// UNDONE: Incremental uniqueness changes
						case ConstraintType.InternalUniqueness:
						case ConstraintType.ExternalUniqueness:
							SignificantObjectTypeChange(constraint.PreferredIdentifierFor);
							break;
					}
				}
			}
			/// <summary>
			/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasDataType)
			/// </summary>
			private static void DataTypeAddedRule(ElementAddedEventArgs e)
			{
				ValueTypeHasDataType link = (ValueTypeHasDataType)e.ModelElement;
				if (link.DataType is AutoCounterNumericDataType)
				{
					SignificantObjectTypeChange(link.ValueType);
				}
			}
			/// <summary>
			/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasDataType)
			/// </summary>
			private static void DataTypeDeletedRule(ElementDeletedEventArgs e)
			{
				ValueTypeHasDataType link = (ValueTypeHasDataType)e.ModelElement;
				ObjectType objectType;
				if (link.DataType is AutoCounterNumericDataType &&
					!(objectType = link.ValueType).IsDeleted)
				{
					SignificantObjectTypeChange(objectType);
				}
			}
			/// <summary>
			/// RolePlayerChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasDataType)
			/// </summary>
			private static void DataTypeChangedRule(RolePlayerChangedEventArgs e)
			{
				if (e.DomainRole.Id == ValueTypeHasDataType.DataTypeDomainRoleId)
				{
					if (e.OldRolePlayer is AutoCounterNumericDataType || e.NewRolePlayer is AutoCounterNumericDataType)
					{
						SignificantObjectTypeChange(((ValueTypeHasDataType)e.ElementLink).ValueType);
					}
				}
				else if (((ValueTypeHasDataType)e.ElementLink).DataType is AutoCounterNumericDataType)
				{
					SignificantObjectTypeChange((ObjectType)e.OldRolePlayer);
					SignificantObjectTypeChange((ObjectType)e.NewRolePlayer);
				}
			}
			/// <summary>
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType)
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
					FrameworkDomainModel.DelayValidateElement(e.ModelElement, UpdateNamesForObjectTypeDelayed);
				}
			}
			 /// <summary>
			 /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasReadingOrder)
			 /// </summary>
			 private static void ReadingOrderAddedRule(ElementAddedEventArgs e)
			 {
				 FrameworkDomainModel.DelayValidateElement(((FactTypeHasReadingOrder)e.ModelElement).FactType, UpdateChildNamesForFactTypeDelayed);
			 }
			 /// <summary>
			 /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasReadingOrder)
			 /// </summary>
			 private static void ReadingOrderDeletedRule(ElementDeletedEventArgs e)
			 {
				 FactType factType;
				 if (!(factType = ((FactTypeHasReadingOrder)e.ModelElement).FactType).IsDeleted)
				 {
					FrameworkDomainModel.DelayValidateElement(factType, UpdateChildNamesForFactTypeDelayed);
				 }
			 }
			 /// <summary>
			 /// RolePlayerPositionChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasReadingOrder)
			 /// </summary>
			 private static void ReadingOrderReorderedRule(RolePlayerOrderChangedEventArgs e)
			 {
				 if (e.SourceDomainRole.Id == FactTypeHasReadingOrder.FactTypeDomainRoleId)
				 {
					 FrameworkDomainModel.DelayValidateElement(e.SourceElement, UpdateChildNamesForFactTypeDelayed);
				 }
			 }
			 /// <summary>
			 /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrderHasReading)
			 /// </summary>
			 private static void ReadingAddedRule(ElementAddedEventArgs e)
			 {
				 FactType factType;
				 if (null != (factType = ((ReadingOrderHasReading)e.ModelElement).ReadingOrder.FactType))
				 {
					 FrameworkDomainModel.DelayValidateElement(factType, UpdateChildNamesForFactTypeDelayed);
				 }
			 }
			 /// <summary>
			 /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Reading)
			 /// </summary>
			 private static void ReadingChangedRule(ElementPropertyChangedEventArgs e)
			 {
				 ReadingOrder order;
				 FactType factType;
				 if (e.DomainProperty.Id == Reading.TextDomainPropertyId &&
					 null != (order = ((Reading)e.ModelElement).ReadingOrder) &&
					 null != (factType = order.FactType))
				 {
					 FrameworkDomainModel.DelayValidateElement(factType, UpdateChildNamesForFactTypeDelayed);
				 }
			 }
			 /// <summary>
			 /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrderHasReading)
			 /// </summary>
			 private static void ReadingDeletedRule(ElementDeletedEventArgs e)
			 {
				 ReadingOrder order;
				 FactType factType;
				 if (!(order = ((ReadingOrderHasReading)e.ModelElement).ReadingOrder).IsDeleted &&
					 null != (factType = order.FactType))
				 {
					 FrameworkDomainModel.DelayValidateElement(factType, UpdateChildNamesForFactTypeDelayed);
				 }
			 }
			 /// <summary>
			 /// RolePlayerPositionChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrderHasReading)
			 /// </summary>
			 private static void ReadingReorderedRule(RolePlayerOrderChangedEventArgs e)
			 {
				 FactType factType;
				 if (e.SourceDomainRole.Id == ReadingOrderHasReading.ReadingOrderDomainRoleId &&
					 null != (factType = ((ReadingOrder)e.SourceElement).FactType))
				 {
					 FrameworkDomainModel.DelayValidateElement(factType, UpdateChildNamesForFactTypeDelayed);
				 }
			 }
			/// <summary>
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Role)
			/// </summary>
			private static void RoleChangedRule(ElementPropertyChangedEventArgs e)
			{
				FactType factType;
				if (e.DomainProperty.Id == Role.NameDomainPropertyId &&
					null != (factType = ((Role)e.ModelElement).FactType))
				{
					FrameworkDomainModel.DelayValidateElement(factType, UpdateChildNamesForFactTypeDelayed);
				}
			}
			/// <summary>
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint)
			/// </summary>
			private static void SetConstraintChangedRule(ElementPropertyChangedEventArgs e)
			{
				Guid propertyId = e.DomainProperty.Id;
				if (propertyId == SetConstraint.ModalityDomainPropertyId)
				{
					SetConstraint constraint = (SetConstraint)e.ModelElement;
					if (IsRelevantConstraint(constraint.Constraint))
					{
						foreach (Role role in constraint.RoleCollection)
						{
							// Note that constraint.FactTypeCollection does not resolve the
							// BinarizedFactType. Notifying twice on one FactType is harmless
							// due to delayed validation.
							FactTypeConstraintPatternChanged(role.BinarizedFactType);
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
			/// RolePlayerPositionChangeRule: typeof(ConstraintRoleSequenceHasRole)
			/// Propagate order changes in a uniqueness constraint to an absorbed uniqueness
			/// </summary>
			private static void UniquenessConstraintRoleOrderChanged(RolePlayerOrderChangedEventArgs e)
			{
				UniquenessConstraint constraint;
				Uniqueness uniqueness;
				if (null != (constraint = e.SourceElement as UniquenessConstraint) &&
					e.SourceDomainRole.Id == ConstraintRoleSequenceHasRole.ConstraintRoleSequenceDomainRoleId &&
					null != (uniqueness = UniquenessIsForUniquenessConstraint.GetUniqueness(constraint)))
				{
					uniqueness.ConceptTypeChildCollection.Move(e.OldOrdinal, e.NewOrdinal);
				}
			}
			/// <summary>
			/// DeletingRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole)
			/// Propagate role deletion in a uniqueness constraint to an absorbed uniqueness
			/// </summary>
			private static void UniquenessConstraintRoleDeleting(ElementDeletingEventArgs e)
			{
				UniquenessConstraint constraint;
				Uniqueness uniqueness;
				ConstraintRoleSequenceHasRole link = (ConstraintRoleSequenceHasRole)e.ModelElement;
				ConstraintRoleSequence sequence = link.ConstraintRoleSequence;
				if (!sequence.IsDeleting &&
					null != (constraint = sequence as UniquenessConstraint) &&
					!link.Role.IsDeleting &&
					null != (uniqueness = UniquenessIsForUniquenessConstraint.GetUniqueness(constraint)))
				{
					uniqueness.ConceptTypeChildCollection.RemoveAt(ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(sequence).IndexOf(link));
				}
			}
			/// <summary>
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel)
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
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact)
			/// Changing the <see cref="P:SubtypeFact.IsPrimary"/> property on a <see cref="SubtypeFact"/>
			/// can modify preferred identification schemes.
			/// </summary>
			private static void SubtypeFactChangedRule(ElementPropertyChangedEventArgs e)
			{
				// UNDONE: Incremental changes, propagate changes to Uniqueness.IsPreferred property
				if (e.DomainProperty.Id == SubtypeFact.ProvidesPreferredIdentifierDomainPropertyId)
				{
					SignificantFactTypeChange((FactType)e.ModelElement);
				}
			}
			/// <summary>
			/// RolePlayerChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier)
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
			/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier)
			/// If an <see cref="ObjectType"/> is alive after gateway processing when its preferred identifier
			/// is deleted then it needs to be reprocessed.
			/// </summary>
			private static void PreferredIdentifierDeletedRule(ElementDeletedEventArgs e)
			{
				// UNDONE: Incremental If this gets past the gateway and is not excluded,
				// then it changed from an EntityType to a ValueType
				ObjectType objectType = ((EntityTypeHasPreferredIdentifier)e.ModelElement).PreferredIdentifierFor;
				if (!objectType.IsDeleted)
				{
					SignificantObjectTypeChange(objectType);
				}
			}
			/// <summary>
			/// RolePlayerChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole)
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
					SignificantFactTypeChange(((Role)e.OldRolePlayer).BinarizedFactType);
				}
				else
				{
					SignificantObjectTypeChange((ObjectType)e.OldRolePlayer);
				}
				SignificantObjectTypeChange(link.RolePlayer);
				SignificantFactTypeChange(link.PlayedRole.BinarizedFactType);
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
					null != factType.Model &&
					!ORMElementGateway.IsElementExcluded(factType))
				{
					FrameworkDomainModel.DelayValidateElement(factType, FactTypeConstraintPatternChangedDelayed);
				}
			}
			[DelayValidatePriority(DomainModelType=typeof(ORMCoreDomainModel), Order=DelayValidatePriorityOrder.AfterDomainModel)]
			private static void FactTypeConstraintPatternChangedDelayed(ModelElement element)
			{
				FactType factType;
				ORMModel model;
				if (!element.IsDeleted &&
					null != (model = (factType = (FactType)element).Model) &&
					!ORMElementGateway.IsElementExcluded(factType))
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
							FrameworkDomainModel.DelayValidateElement(model, DelayValidateModel);
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
			private static void SignificantUniquenessConstraintChange(UniquenessConstraint constraint)
			{
				if (constraint != null &&
					!constraint.IsDeleted &&
					!ORMElementGateway.IsElementExcluded(constraint))
				{
					FrameworkDomainModel.DelayValidateElement(constraint, SignificantUniquenessConstraintChangeDelayed);
				}
			}
			[DelayValidatePriority(DomainModelType = typeof(ORMCoreDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void SignificantUniquenessConstraintChangeDelayed(ModelElement element)
			{
				UniquenessConstraint constraint;
				ORMModel model;
				if (!element.IsDeleted &&
					null != (model = (constraint = (UniquenessConstraint)element).Model) &&
					!ORMElementGateway.IsElementExcluded(constraint))
				{
					AddTransactedModelElement(constraint, ModelElementModification.ORMElementChanged);
					FrameworkDomainModel.DelayValidateElement(model, DelayValidateModel);
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
			[DelayValidateReplaces("UpdateChildNamesForFactTypeDelayed")]
			[DelayValidateReplaces("UpdateChildNamesForFactTypeDelayedWorker")]
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
						LinkedElementCollection<FactType> impliedFactTypes = objectification.ImpliedFactTypeCollection;
						int impliedFactTypeCount = impliedFactTypes.Count;
						for (int i = 0; i < impliedFactTypeCount; ++i)
						{
							FactType impliedFactType = impliedFactTypes[i];
							AddTransactedModelElement(impliedFactType, ModelElementModification.ORMElementChanged);
						}
						if (impliedFactTypeCount == 1)
						{
							AddTransactedModelElement(factType, ModelElementModification.ORMElementChanged);
						}
					}
					else
					{
						AddTransactedModelElement(factType, ModelElementModification.ORMElementChanged);
					}
					FrameworkDomainModel.DelayValidateElement(model, DelayValidateModel);
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
			[DelayValidateReplaces("UpdateNamesForObjectTypeDelayed")]
			private static void SignificantObjectTypeChangeDelayed(ModelElement element)
			{
				ObjectType objectType;
				ORMModel model;
				if (!element.IsDeleted &&
					null != (model = (objectType = (ObjectType)element).Model) &&
					!ORMElementGateway.IsElementExcluded(objectType))
				{
					AddTransactedModelElement(objectType, ModelElementModification.ORMElementChanged);
					FrameworkDomainModel.DelayValidateElement(model, DelayValidateModel);
				}
			}
			[DelayValidatePriority(DomainModelType = typeof(ORMCoreDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void UpdateNamesForObjectTypeDelayed(ModelElement element)
			{
				if (!element.IsDeleted)
				{
					ObjectType objectType = (ObjectType)element;
					string objectTypeName = objectType.Name;
					ConceptType conceptType = ConceptTypeIsForObjectType.GetConceptType(objectType);
					LinkedElementCollection<FactType> pathFactTypes;
					int factTypeCount;
					RoleBase towardsRole;
					RoleBase oppositeRole;
					if (null != conceptType)
					{
						// Precheck name to minimize downstream calls, the property change
						// will check itself.
						if (conceptType.Name != objectTypeName)
						{
							conceptType.Name = objectTypeName;
							foreach (ConceptTypeReferencesConceptType reference in ConceptTypeReferencesConceptType.GetLinksToReferencingConceptTypeCollection(conceptType))
							{
								pathFactTypes = ConceptTypeChildHasPathFactType.GetPathFactTypeCollection(reference);
								if (0 != (factTypeCount = pathFactTypes.Count) &&
									null != (towardsRole = FactTypeMapsTowardsRole.GetTowardsRole(pathFactTypes[factTypeCount - 1])) &&
									null != (oppositeRole = towardsRole.OppositeRole))
								{
									reference.OppositeName = ResolveRoleName(oppositeRole);
								}
							}
							foreach (ConceptTypeReferencesConceptType reference in ConceptTypeReferencesConceptType.GetLinksToReferencedConceptTypeCollection(conceptType))
							{
								pathFactTypes = ConceptTypeChildHasPathFactType.GetPathFactTypeCollection(reference);
								if (0 != (factTypeCount = pathFactTypes.Count) &&
									null != (towardsRole = FactTypeMapsTowardsRole.GetTowardsRole(pathFactTypes[factTypeCount - 1])))
								{
									reference.Name = ResolveRoleName(towardsRole);
								}
							}
						}
					}
					InformationTypeFormat informationTypeFormat = InformationTypeFormatIsForValueType.GetInformationTypeFormat(objectType);
					if (null != informationTypeFormat)
					{
						if (informationTypeFormat.Name != objectTypeName)
						{
							informationTypeFormat.Name = objectTypeName;
							foreach (InformationType informationType in InformationType.GetLinksToConceptTypeCollection(informationTypeFormat))
							{
								pathFactTypes = ConceptTypeChildHasPathFactType.GetPathFactTypeCollection(informationType);
								if (0 != (factTypeCount = pathFactTypes.Count) &&
									null != (towardsRole = FactTypeMapsTowardsRole.GetTowardsRole(pathFactTypes[factTypeCount - 1])) &&
									null != (oppositeRole = towardsRole.OppositeRole))
								{
									informationType.Name = ResolveRoleName(oppositeRole);
								}
							}
						}
					}
				}
			}
			[DelayValidatePriority(DomainModelType = typeof(ORMCoreDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void UpdateChildNamesForFactTypeDelayed(ModelElement element)
			{
				if (!element.IsDeleted)
				{
					// The naming algorithm uses readings for both link fact types and objectified fact types.
					// Objectified fact types will not have attached concept type children, so we just validate
					// the link fact types on a change. Defer one level in case both the link and objectified
					// fact types are changed in the same transaction.
					FactType factType = (FactType)element;
					Objectification objectification;
					if (null != (objectification = factType.Objectification))
					{
						foreach (FactType impliedFactType in objectification.ImpliedFactTypeCollection)
						{
							FrameworkDomainModel.DelayValidateElement(impliedFactType, UpdateChildNamesForFactTypeDelayedWorker);
						}
					}
					else
					{
						FrameworkDomainModel.DelayValidateElement(factType, UpdateChildNamesForFactTypeDelayedWorker);
					}
				}
			}
			[DelayValidatePriority(1, DomainModelType = typeof(ORMCoreDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void UpdateChildNamesForFactTypeDelayedWorker(ModelElement element)
			{
				if (!element.IsDeleted)
				{
					FactType factType = (FactType)element;
					foreach (ConceptTypeChild child in ConceptTypeChildHasPathFactType.GetConceptTypeChild(factType))
					{
						LinkedElementCollection<FactType> factTypePath = ConceptTypeChildHasPathFactType.GetPathFactTypeCollection(child);
						int pathCount;
						RoleBase towardsRole;
						RoleBase fromRole;
						if (0 != (pathCount = factTypePath.Count) &&
							factType == factTypePath[pathCount - 1] &&
							null != (towardsRole = FactTypeMapsTowardsRole.GetTowardsRole(factType)) &&
							null != (fromRole = towardsRole.OppositeRole))
						{
							string resolvedName = ResolveRoleName(fromRole);
							ConceptTypeReferencesConceptType reference;
							if (null != (reference = child as ConceptTypeReferencesConceptType))
							{
								// Match original backwards pattern
								reference.OppositeName = resolvedName;
								child.Name = ResolveRoleName(towardsRole);
							}
							else
							{
								child.Name = resolvedName;
							}
						}
					}
				}
			}
			#endregion // General validation helper methods
		}
		#endregion // Regeneration rule delay validation methods
	}
}