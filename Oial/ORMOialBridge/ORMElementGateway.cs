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
		#region ORM Error Filtering Methods
		private bool ShouldIgnoreObjectType(ObjectType objectType)
		{
			return ORMElementGateway.IsElementExcluded(objectType);
		}
		private bool ShouldIgnoreFactType(FactType factType)
		{
			return ORMElementGateway.IsElementExcluded(factType) || null != factType.Objectification;
		}
		#endregion // ORM Error Filtering Methods
		#region ORMElementGateway class
		/// <summary>
		/// A set of rules that check preconditions on <see cref="ObjectType"/> and <see cref="FactType"/>
		/// instances before allowing them to be considered for absorption. An element is added for absorption
		/// when it is passes this gateway, and deleted when it violates a precondition.
		/// </summary>
		private static partial class ORMElementGateway
		{
			#region ValidationPriority enum
			/// <summary>
			/// DelayValidate ordering constants
			/// </summary>
			private static class ValidationPriority
			{
				public const int NewObjectType = -100;
				public const int ReconsiderObjectType = -90;
				public const int NewFactType = -80;
				public const int ReconsiderFactType = -70;
				public const int RemoveExcludedBridgeRelationships = -60;
				public const int AddElement = -50;
			}
			#endregion // ValidationPriority enum
			#region Public methods
			/// <summary>
			/// Returns <see langword="true"/> if an element is currently excluded from 
			/// </summary>
			public static bool IsElementExcluded(ORMModelElement element)
			{
				return null != ExcludedORMModelElement.GetLinkToAbstractionModel(element);
			}
			/// <summary>
			/// Perfom initial gateway checks when an <see cref="ORMModel"/> is first considered for absorption
			/// </summary>
			/// <param name="model">The <see cref="ORMModel"/> to verify and exclude elements in</param>
			public static void Initialize(ORMModel model)
			{
				AbstractionModel abstractionModel = AbstractionModelIsForORMModel.GetAbstractionModel(model);
				foreach (ObjectType objectType in model.ObjectTypeCollection)
				{
					if (!IsElementExcluded(objectType) &&
						!ShouldConsiderObjectType(objectType, null))
					{
						ExcludeObjectType(objectType, abstractionModel, true);
					}
				}
				foreach (FactType factType in model.FactTypeCollection)
				{
					if (!IsElementExcluded(factType) &&
						!ShouldConsiderFactType(factType, false))
					{
						ExcludeFactType(factType, abstractionModel, true);
					}
				}
			}
			#endregion // Public methods
			#region ShouldConsider* methods, determine if an element should be filtered
			/// <summary>
			/// Determine if an <see cref="ObjectType"/> should be considered during
			/// absorption. Considers the object state, and the current exclusion
			/// markings on its preferred identifier.
			/// </summary>
			/// <param name="objectType">The <see cref="ObjectType"/> to test</param>
			/// <param name="ignoreFactType">Succeed even if this <see cref="FactType"/> is excluded. Can be <see langword="null"/>.</param>
			/// <returns><see langword="true"/> if the <paramref name="objectType"/> passes all necessary conditions for consideration.</returns>
			private static bool ShouldConsiderObjectType(ObjectType objectType, FactType ignoreFactType)
			{
				// Look at the error states we care about. If any of these error
				// states are present then we do not consider.
				// Note that any changes to the list of errors must correspond to changes in
				// ObjectTypeErrorAddedRule and ObjectTypeErrorDeletedRule
				if (null == objectType.ReferenceSchemeError &&
					null == objectType.ObjectTypeRequiresPrimarySupertypeError &&
					null == objectType.PreferredIdentifierRequiresMandatoryError &&
					null == objectType.CompatibleSupertypesError &&
					null == objectType.DataTypeNotSpecifiedError)
				{
					UniquenessConstraint pid = objectType.PreferredIdentifier;
					if (pid != null)
					{
						// Make sure none of the associated FactTypes are not excluded.
						// Not that this causes a recursive situation, so the answers here
						// can change over time and an ObjectType that can be considered now
						// may not be valid for consideration by the time the model is processed.
						foreach (FactType factType in pid.FactTypeCollection)
						{
							if (factType != ignoreFactType && IsElementExcluded(factType))
							{
								return false;
							}
						}
					}
					return true;
				}
				return false;
			}
			/// <summary>
			/// Determine if an <see cref="FactType"/> should be considered during
			/// absorption. Considers the state of the FactType, and the current exclusion
			/// markings on its role players.
			/// </summary>
			/// <param name="factType">The <see cref="FactType"/> to test</param>
			/// <param name="ignoreRolePlayersFilteredForThisFactType">Do block consideration because a role player is filtered for this <paramref name="factType"/>.</param>
			/// <returns><see langword="true"/> if the <paramref name="factType"/> passes all necessary conditions for consideration.</returns>
			private static bool ShouldConsiderFactType(FactType factType, bool ignoreRolePlayersFilteredForThisFactType)
			{
				// Note that any changes to the list of errors must correspond to changes in
				// FactTypeErrorAddedRule and FactTypeErrorDeletedRule
				if (null == factType.InternalUniquenessConstraintRequiredError &&
					null == factType.ImpliedInternalUniquenessConstraintError)
				{
					foreach (RoleBase role in factType.RoleCollection)
					{
						ObjectType rolePlayer = role.Role.RolePlayer;
						if (rolePlayer == null ||
							(IsElementExcluded(rolePlayer) &&
							!(!ignoreRolePlayersFilteredForThisFactType || !ShouldConsiderObjectType(rolePlayer, factType))))
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}
			#endregion // ShouldConsider* methods, determine if an element should be filtered
			#region FilterNew* methods, determine filtering for newly created elements
			[DelayValidatePriority(ValidationPriority.NewFactType)]
			private static void FilterNewFactType(ModelElement element)
			{
				ModelHasFactType link = element as ModelHasFactType;
				FactType factType = link.FactType;
				if (ShouldConsiderFactType(factType, false))
				{
					AddFactType(factType);
				}
				else
				{
					ExcludeFactType(factType);
				}
			}
			[DelayValidatePriority(ValidationPriority.NewObjectType)]
			private static void FilterNewObjectType(ModelElement element)
			{
				ModelHasObjectType link = element as ModelHasObjectType;
				ObjectType objectType = link.ObjectType;
				if (ShouldConsiderObjectType(objectType, null))
				{
					AddObjectType(objectType);
				}
				else
				{
					ExcludeObjectType(objectType);
				}
			}
			#endregion // FilterNew* methods, determine filtering for newly created elements
			#region FilterModified* methods, modify filtering of existing elements
			/// <summary>
			/// A <see cref="FactType"/> has been directly or indirectly modified in
			/// such a way that its exclusion status in the absorption process may change.
			/// </summary>
			/// <param name="factType">The modified <see cref="FactType"/></param>
			/// <param name="filterImpliedFactTypes">Set to <see langword="true"/> to check for and filter implied fact types</param>
			private static void FilterModifiedFactType(FactType factType, bool filterImpliedFactTypes)
			{
				if (factType != null && !factType.IsDeleted)
				{
					FrameworkDomainModel.DelayValidateElement(factType, FilterModifiedFactTypeDelayed);
					Objectification objectification;
					if (filterImpliedFactTypes && null != (objectification = factType.Objectification))
					{
						foreach (FactType impliedFactType in objectification.ImpliedFactTypeCollection)
						{
							FrameworkDomainModel.DelayValidateElement(impliedFactType, FilterModifiedFactTypeDelayed);
						}
					}
				}
			}
			[DelayValidatePriority(ValidationPriority.ReconsiderFactType)]
			private static void FilterModifiedFactTypeDelayed(ModelElement element)
			{
				if (!element.IsDeleted)
				{
					FactType factType = (FactType)element;
					ExcludedORMModelElement exclusionLink = ExcludedORMModelElement.GetLinkToAbstractionModel(factType);
					if (ShouldConsiderFactType(factType, true))
					{
						if (exclusionLink != null)
						{
							exclusionLink.Delete();
							AddFactType(factType);
							foreach (IFactConstraint factConstraint in factType.FactConstraintCollection)
							{
								ObjectType preferredFor = factConstraint.Constraint.PreferredIdentifierFor;
								if (preferredFor != null)
								{
									FilterModifiedObjectType(preferredFor);
								}
							}
						}
					}
					else if (exclusionLink == null)
					{
						ExcludeFactType(factType);
					}
				}
			}
			/// <summary>
			/// A <see cref="ObjectType"/> has been directly or indirectly modified in
			/// such a way that its exclusion status in the absorption process may change.
			/// </summary>
			/// <param name="objectType">The modified <see cref="ObjectType"/></param>
			private static void FilterModifiedObjectType(ObjectType objectType)
			{
				if (objectType != null && !objectType.IsDeleted)
				{
					FrameworkDomainModel.DelayValidateElement(objectType, FilterModifiedObjectTypeDelayed);
				}
			}
			[DelayValidatePriority(ValidationPriority.ReconsiderObjectType)]
			private static void FilterModifiedObjectTypeDelayed(ModelElement element)
			{
				if (!element.IsDeleted)
				{
					ObjectType objectType = (ObjectType)element;
					ExcludedORMModelElement exclusionLink = ExcludedORMModelElement.GetLinkToAbstractionModel(objectType);
					if (ShouldConsiderObjectType(objectType, null))
					{
						if (exclusionLink != null)
						{
							exclusionLink.Delete();
							AddObjectType(objectType);
							foreach (Role playedRole in objectType.PlayedRoleCollection)
							{
								FilterModifiedFactType(playedRole.FactType, false);
								RoleProxy proxy = playedRole.Proxy;
								if (proxy != null)
								{
									FilterModifiedFactType(proxy.FactType, false);
								}
							}
						}
					}
					else if (exclusionLink == null)
					{
						ExcludeObjectType(objectType);
					}
				}
			}
			#endregion // FilterModified* methods, modify filtering of existing elements
			#region Exclude* methods, exclude elements from absorption consideration
			private static void ExcludeFactType(FactType factType)
			{
				AbstractionModel model = AbstractionModelIsForORMModel.GetAbstractionModel(factType.Model);
				if (model != null)
				{
					ExcludeFactType(factType, model, false);
				}
			}
			private static void ExcludeFactType(FactType factType, AbstractionModel model)
			{
				ExcludeFactType(factType, model, false);
			}
			private static void ExcludeFactType(FactType factType, AbstractionModel model, bool forceCreate)
			{
				if (forceCreate ||
					null == ExcludedORMModelElement.GetAbstractionModel(factType))
				{
					new ExcludedORMModelElement(factType, model);
					foreach (IFactConstraint constraint in factType.FactConstraintCollection)
					{
						ObjectType preferredFor = constraint.Constraint.PreferredIdentifierFor;
						if (preferredFor != null)
						{
							ExcludeObjectType(preferredFor, model);
						}
					}
				}
			}
			private static void ExcludeObjectType(ObjectType objectType)
			{
				AbstractionModel model = AbstractionModelIsForORMModel.GetAbstractionModel(objectType.Model);
				if (model != null)
				{
					ExcludeObjectType(objectType, model, false);
				}
			}
			private static void ExcludeObjectType(ObjectType objectType, AbstractionModel model)
			{
				ExcludeObjectType(objectType, model, false);
			}
			private static void ExcludeObjectType(ObjectType objectType, AbstractionModel model, bool forceCreate)
			{
				if (forceCreate ||
					null == ExcludedORMModelElement.GetAbstractionModel(objectType))
				{
					new ExcludedORMModelElement(objectType, model);
					foreach (Role playedRole in objectType.PlayedRoleCollection)
					{
						ExcludeFactType(playedRole.FactType, model);
						RoleProxy proxy = playedRole.Proxy;
						if (proxy != null)
						{
							ExcludeFactType(proxy.FactType, model);
						}
					}
				}
			}
			#endregion // Exclude* methods, exclude elements from absorption consideration
			#region Add* methods, add elements to be considered for absorption
			/// <summary>
			/// Consider a new <see cref="FactType"/> for absorption. Any FactType
			/// that is no longer excluded is also consider to be new to the absorption algorithm.
			/// </summary>
			private static void AddFactType(FactType factType)
			{
				if (!factType.IsDeleted)
				{
					FrameworkDomainModel.DelayValidateElement(factType, AddFactTypeDelayed);
				}
			}
			/// <summary>
			/// Delay validation callback used when the state of a <see cref="FactType"/>
			/// has changed such that it may or may not be included in the abstraction model.
			/// </summary>
			[DelayValidatePriority(ValidationPriority.AddElement)]
			private static void AddFactTypeDelayed(ModelElement element)
			{
				if (!element.IsDeleted)
				{
					FactType factType = (FactType)element;
					// Do a final exclusion check. FactTypes may be added to the list for consideration
					// but removed later on.
					if (!IsElementExcluded(factType))
					{
						FrameworkDomainModel.DelayValidateElement(factType.Model, DelayValidateModel);
						AddTransactedModelElement(factType, ModelElementModification.ORMElementAdded);
					}
				}
			}
			/// <summary>
			/// Consider a new <see cref="ObjectType"/> for absorption. Any ObjectType
			/// that is no longer excluded is also consider to be new to the absorption algorithm.
			/// </summary>
			private static void AddObjectType(ObjectType objectType)
			{
				if (!objectType.IsDeleted)
				{
					FrameworkDomainModel.DelayValidateElement(objectType, AddObjectTypeDelayed);
				}
			}
			/// <summary>
			/// Delay validation callback used when the state of a <see cref="ObjectType"/>
			/// has changed such that it may or may not be included in the abstraction model.
			/// </summary>
			[DelayValidatePriority(ValidationPriority.AddElement)]
			private static void AddObjectTypeDelayed(ModelElement element)
			{
				if (!element.IsDeleted)
				{
					ObjectType objectType = (ObjectType)element;
					// Do a final exclusion check. FactTypes may be added to the list for consideration
					// but removed later on.
					if (!IsElementExcluded(objectType))
					{
						FrameworkDomainModel.DelayValidateElement(objectType.Model, DelayValidateModel);
						AddTransactedModelElement(objectType, ModelElementModification.ORMElementAdded);
					}
				}
			}
			#endregion // Add* methods, add elements to be considered for absorption
			#region ExcludedORMModelElement rules, remove elements from the bridge model when an element is excluded
			/// <summary>
			/// AddRule: typeof(ExcludedORMModelElement)
			/// Remove any existing bridge relationships when an element is excluded
			/// </summary>
			private static void ElementExclusionAddedRule(ElementAddedEventArgs e)
			{
				FrameworkDomainModel.DelayValidateElement(e.ModelElement, ExclusionAdded);
			}
			[DelayValidatePriority(ValidationPriority.RemoveExcludedBridgeRelationships)]
			private static void ExclusionAdded(ModelElement element)
			{
				ExcludedORMModelElement link = (ExcludedORMModelElement)element;
				// The link may have been deleted since it was added, essentially readding the
				// object. This is the reason for the delayed validation: we don't want to
				// prematurely delete existing links
				if (!link.IsDeleted)
				{
					ObjectType objectType;
					FactType factType;
					if (null != (objectType = link.ExcludedElement as ObjectType))
					{
						ConceptTypeIsForObjectType conceptTypeLink;
						InformationTypeFormatIsForValueType formatLink;
						if (null != (conceptTypeLink = ConceptTypeIsForObjectType.GetLinkToConceptType(objectType)))
						{
							conceptTypeLink.Delete();
						}
						if (null != (formatLink = InformationTypeFormatIsForValueType.GetLinkToInformationTypeFormat(objectType)))
						{
							formatLink.Delete();
						}
					}
					else if (null != (factType = link.ExcludedElement as FactType))
					{
						FactTypeMapsTowardsRole directionLink;
						if (null != (directionLink = FactTypeMapsTowardsRole.GetLinkToTowardsRole(factType)))
						{
							directionLink.Delete();
						}
						ReadOnlyCollection<ConceptTypeChildHasPathFactType> pathLinks = ConceptTypeChildHasPathFactType.GetLinksToConceptTypeChild(factType);
						int pathCount = pathLinks.Count;
						for (int i = pathCount - 1; i >= 0; --i)
						{
							pathLinks[i].Delete();
						}
					}
				}
			}
			#endregion // ExcludedORMModelElement rules, remove elements from the bridge model when an element is excluded
			#region Rules to defer specific changes to general filter methods
			#region Add rules
			/// <summary>
			/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ModelHasFactType)
			/// Automatically exclude any newly added <see cref="FactType"/>s from consideration
			/// in the abstraction model.
			/// </summary>
			private static void FactTypeAddedRule(ElementAddedEventArgs e)
			{
				FrameworkDomainModel.DelayValidateElement(e.ModelElement, FilterNewFactType);
			}
			/// <summary>
			/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ModelHasObjectType)
			/// Automatically exclude any newly added <see cref="FactType"/>s from consideration
			/// in the abstraction model.
			/// </summary>
			private static void ObjectTypeAddedRule(ElementAddedEventArgs e)
			{
				FrameworkDomainModel.DelayValidateElement(e.ModelElement, FilterNewObjectType);
			}
			#endregion // Add rules
			#region Model error tracking rules
			/// <summary>
			/// AddRule: typeof(FactTypeHasImpliedInternalUniquenessConstraintError)
			/// AddRule: typeof(FactTypeHasFactTypeRequiresInternalUniquenessConstraintError)
			/// An error that precludes absorption has been introduced on a <see cref="FactType"/>
			/// </summary>
			private static void FactTypeErrorAddedRule(ElementAddedEventArgs e)
			{
				ElementAssociatedWithModelError link = (ElementAssociatedWithModelError)e.ModelElement;
				FactType factType = (FactType)link.AssociatedElement;
				// If we're currently excluded, then adding either of these errors will
				// not change the situation
				if (!IsElementExcluded(factType))
				{
					FilterModifiedFactType(factType, true);
				}
			}
			/// <summary>
			/// DeleteRule: typeof(FactTypeHasImpliedInternalUniquenessConstraintError)
			/// DeleteRule: typeof(FactTypeHasFactTypeRequiresInternalUniquenessConstraintError)
			/// An error that precludes absorption has been removed from a <see cref="FactType"/>
			/// </summary>
			private static void FactTypeErrorDeletedRule(ElementDeletedEventArgs e)
			{
				ElementAssociatedWithModelError link = (ElementAssociatedWithModelError)e.ModelElement;
				FactType factType = (FactType)link.AssociatedElement;
				if (!factType.IsDeleted)
				{
					FilterModifiedFactType(factType, true);
				}
			}
			/// <summary>
			/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasEntityTypeRequiresReferenceSchemeError)
			/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasObjectTypeRequiresPrimarySupertypeError)
			/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasPreferredIdentifierRequiresMandatoryError)
			/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasCompatibleSupertypesError)
			/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ValueTypeHasUnspecifiedDataTypeError)
			/// An error that precludes absorption has been introduced on an <see cref="ObjectType"/>
			/// </summary>
			private static void ObjectTypeErrorAddedRule(ElementAddedEventArgs e)
			{
				ElementAssociatedWithModelError link = (ElementAssociatedWithModelError)e.ModelElement;
				ModelElement element = link.AssociatedElement;
				ValueTypeHasDataType dataTypeLink;
				ObjectType objectType;
				if ((null != (objectType = element as ObjectType) ||
					// Handles the ValueTypeHasUnspecifiedDataTypeError
					(null != (dataTypeLink = element as ValueTypeHasDataType) &&
					null != (objectType = dataTypeLink.ValueType))) &&
					// If we're currently excluded, then adding any of these errors will
					// not change the situation
					!IsElementExcluded(objectType))
				{
					FilterModifiedObjectType(objectType);
				}
			}
			/// <summary>
			/// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasEntityTypeRequiresReferenceSchemeError)
			/// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasObjectTypeRequiresPrimarySupertypeError)
			/// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasPreferredIdentifierRequiresMandatoryError)
			/// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasCompatibleSupertypesError)
			/// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ValueTypeHasUnspecifiedDataTypeError)
			/// An error that precludes absorption has been removed from an <see cref="ObjectType"/>
			/// </summary>
			private static void ObjectTypeErrorDeletedRule(ElementDeletedEventArgs e)
			{
				ElementAssociatedWithModelError link = (ElementAssociatedWithModelError)e.ModelElement;
				ModelElement element = link.AssociatedElement;
				ObjectType objectType;
				ValueTypeHasDataType dataTypeLink;
				if ((null != (objectType = element as ObjectType) ||
					// Handles the ValueTypeHasUnspecifiedDataTypeError
					(null != (dataTypeLink = element as ValueTypeHasDataType) &&
					!dataTypeLink.IsDeleted &&
					null != (objectType = dataTypeLink.ValueType))) &&
					!objectType.IsDeleted)
				{
					FilterModifiedObjectType(objectType);
				}
			}
			#endregion // Model error tracking rules
			#region RolePlayer tracking rules
			/// <summary>
			/// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole)
			/// </summary>
			private static void RolePlayerAddedRule(ElementAddedEventArgs e)
			{
				ProcessRolePlayerAdded((ObjectTypePlaysRole)e.ModelElement);
			}
			private static void ProcessRolePlayerAdded(ObjectTypePlaysRole link)
			{
				Role role = link.PlayedRole;
				FilterModifiedFactType(role.FactType, false);
				RoleProxy proxy = role.Proxy;
				if (proxy != null)
				{
					FilterModifiedFactType(proxy.FactType, false);
				}
				FilterModifiedObjectType(link.RolePlayer);
			}
			/// <summary>
			/// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole)
			/// </summary>
			private static void RolePlayerDeletedRule(ElementDeletedEventArgs e)
			{
				ProcessRolePlayerDeleted((ObjectTypePlaysRole)e.ModelElement, null, null);
			}
			private static void ProcessRolePlayerDeleted(ObjectTypePlaysRole link, ObjectType rolePlayer, Role role)
			{
				if (role == null)
				{
					role = link.PlayedRole;
				}
				if (rolePlayer == null)
				{
					rolePlayer = link.RolePlayer;
				}
				FilterModifiedFactType(role.FactType, false);
				RoleProxy proxy = role.Proxy;
				if (proxy != null)
				{
					FilterModifiedFactType(proxy.FactType, false);
				}
				FilterModifiedObjectType(rolePlayer);
			}
			/// <summary>
			/// RolePlayerChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole)
			/// </summary>
			private static void RolePlayerRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ElementLink as ObjectTypePlaysRole;
				Guid changedRoleGuid = e.DomainRole.Id;
				if (changedRoleGuid == ObjectTypePlaysRole.PlayedRoleDomainRoleId)
				{
					ProcessRolePlayerDeleted(link, null, (Role)e.OldRolePlayer);
					ProcessRolePlayerAdded(link);
				}
				else
				{
					ProcessRolePlayerDeleted(link, (ObjectType)e.OldRolePlayer, null);
					ProcessRolePlayerAdded(link);
				}
			}
			#endregion // RolePlayer tracking rules
			#endregion // Rules to defer specific changes to general filter
		}
		#endregion // ORMElementGateway class
	}
}