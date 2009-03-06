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
		#region ORM Error Filtering Methods
		private bool ShouldIgnoreObjectType(ObjectType objectType)
		{
			return ORMElementGateway.IsElementExcluded(objectType);
		}
		private bool ShouldIgnoreFactType(FactType factType)
		{
			return ORMElementGateway.IsElementExcluded(factType) || (null != factType.Objectification && factType.UnaryRole == null);
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
			#region Public methods
			/// <summary>
			/// Returns <see langword="true"/> if an element is currently excluded from 
			/// </summary>
			public static bool IsElementExcluded(ORMModelElement element)
			{
				return null != ExcludedORMModelElement.GetLinkToAbstractionModel(element);
			}
			/// <summary>
			/// A callback used to notify when an element has been deleted. Used during deserialization.
			/// </summary>
			/// <param name="element">The <see cref="ORMModelElement"/> being excluded.</param>
			public delegate void NotifyORMElementExcluded(ORMModelElement element);
			/// <summary>
			/// Perfom initial gateway checks when an <see cref="ORMModel"/> is first considered for absorption
			/// </summary>
			/// <param name="model">The <see cref="ORMModel"/> to verify and exclude elements in</param>
			/// <param name="notifyExcluded">A callback notifying when an element is being excluded. Used during deserialization. Can be <see langword="null"/>.</param>
			public static void Initialize(ORMModel model, NotifyORMElementExcluded notifyExcluded)
			{
				AbstractionModel abstractionModel = AbstractionModelIsForORMModel.GetAbstractionModel(model);
				foreach (ObjectType objectType in model.ObjectTypeCollection)
				{
					if (!IsElementExcluded(objectType) &&
						!ShouldConsiderObjectType(objectType, null, false))
					{
						ExcludeObjectType(objectType, abstractionModel, true, notifyExcluded);
					}
				}
				foreach (FactType factType in model.FactTypeCollection)
				{
					if (!IsElementExcluded(factType) &&
						!ShouldConsiderFactType(factType, null, false))
					{
						ExcludeFactType(factType, abstractionModel, true, notifyExcluded);
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
			/// <param name="ignoreFactTypesFilteredForThisObjectType">Don't block consideration of this <paramref name="objectType"/> if a <see cref="FactType"/> is excluded because this <see cref="ObjectType"/> is currently excluded.</param>
			/// <param name="ignoreFactType">Succeed even if this <see cref="FactType"/> is excluded. Can be <see langword="null"/>.</param>
			/// <returns><see langword="true"/> if the <paramref name="objectType"/> passes all necessary conditions for consideration.</returns>
			private static bool ShouldConsiderObjectType(ObjectType objectType, FactType ignoreFactType, bool ignoreFactTypesFilteredForThisObjectType)
			{
				// Look at the error states we care about. If any of these error
				// states are present then we do not consider.
				// Note that any changes to the list of errors must correspond to changes in
				// ObjectTypeErrorAddedRule and ObjectTypeErrorDeletedRule
				if (null == objectType.ReferenceSchemeError &&
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
							if (factType != ignoreFactType &&
								IsElementExcluded(factType))
							{
								if (ignoreFactTypesFilteredForThisObjectType && ShouldConsiderFactType(factType, objectType, true))
								{
									// This pattern is used only during delay validation. A cleaner model would
									// be a delegate callback, but it isn't worth the additional overhead given
									// that this would be the only code that would ever run there.
									FilterModifiedFactType(factType, true);
								}
								else
								{
									return false;
								}
							}
						}
					}
					else if (!objectType.IsValueType)
					{
						// If this is a subtype, then we need to resolve
						// the preferred identifier back to a non-excluded super type
						ObjectType preferridentifierFrom = null;
						ObjectType.WalkSupertypes(
							objectType,
							delegate(ObjectType type, int depth, bool isPrimary)
							{
								ObjectTypeVisitorResult result = ObjectTypeVisitorResult.Continue;
								if (isPrimary)
								{
									if (IsElementExcluded(type))
									{
										result = ObjectTypeVisitorResult.Stop;
									}
									else if (type.PreferredIdentifier != null)
									{
										preferridentifierFrom = type;
										result = ObjectTypeVisitorResult.Stop;
									}
									else
									{
										result = ObjectTypeVisitorResult.SkipFollowingSiblings; // We already have the primary, no need to look further at this level
									}
								}
								else if (depth != 0)
								{
									result = ObjectTypeVisitorResult.SkipChildren;
								}
								return result;
							});
						return preferridentifierFrom != null;
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
			/// <param name="ignoreRolePlayer">No not excluded the <paramref name="factType"/> because this <see cref="ObjectType"/> is currently excluded.</param>
			/// <param name="ignoreRolePlayersFilteredForThisFactType">Do block consideration because a role player is filtered for this <paramref name="factType"/>.</param>
			/// <returns><see langword="true"/> if the <paramref name="factType"/> passes all necessary conditions for consideration.</returns>
			private static bool ShouldConsiderFactType(FactType factType, ObjectType ignoreRolePlayer, bool ignoreRolePlayersFilteredForThisFactType)
			{
				// Note that any changes to the list of errors must correspond to changes in
				// FactTypeErrorAddedRule and FactTypeErrorDeletedRule
				FactTypeDerivationExpression derivation;
				if (null == factType.InternalUniquenessConstraintRequiredError &&
					null == factType.ImpliedInternalUniquenessConstraintError &&
					(null == (derivation = factType.DerivationRule) || (derivation.DerivationStorage != DerivationStorageType.Derived || factType is SubtypeFact)))
				{
					foreach (RoleBase role in factType.RoleCollection)
					{
						ObjectType rolePlayer = role.Role.RolePlayer;
						if (rolePlayer == null ||
							(ignoreRolePlayer != rolePlayer &&
							IsElementExcluded(rolePlayer) &&
							!(!ignoreRolePlayersFilteredForThisFactType || ShouldConsiderObjectType(rolePlayer, factType, true))))
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
			[DelayValidatePriority(ValidationPriority.GatewayNewFactType, DomainModelType = typeof(ORMCoreDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void FilterNewFactType(ModelElement element)
			{
				ModelHasFactType link = element as ModelHasFactType;
				FactType factType = link.FactType;
				if (ShouldConsiderFactType(factType, null, false))
				{
					AddFactType(factType);
				}
				else
				{
					ExcludeFactType(factType);
				}
			}
			[DelayValidatePriority(ValidationPriority.GatewayNewObjectType, DomainModelType = typeof(ORMCoreDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void FilterNewObjectType(ModelElement element)
			{
				ModelHasObjectType link = element as ModelHasObjectType;
				ObjectType objectType = link.ObjectType;
				if (ShouldConsiderObjectType(objectType, null, false))
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
			[DelayValidatePriority(ValidationPriority.GatewayReconsiderFactType, DomainModelType = typeof(ORMCoreDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void FilterModifiedFactTypeDelayed(ModelElement element)
			{
				if (!element.IsDeleted)
				{
					FactType factType = (FactType)element;
					ExcludedORMModelElement exclusionLink = ExcludedORMModelElement.GetLinkToAbstractionModel(factType);
					if (ShouldConsiderFactType(factType, null, true))
					{
						if (exclusionLink != null)
						{
							exclusionLink.Delete();
							AddFactType(factType);
							foreach (IFactConstraint factConstraint in factType.FactConstraintCollection)
							{
								ObjectType preferredFor = factConstraint.Constraint.PreferredIdentifierFor;
								if (preferredFor != null && IsElementExcluded(preferredFor))
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
			[DelayValidatePriority(ValidationPriority.GatewayReconsiderObjectType, DomainModelType = typeof(ORMCoreDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void FilterModifiedObjectTypeDelayed(ModelElement element)
			{
				if (!element.IsDeleted)
				{
					ObjectType objectType = (ObjectType)element;
					ExcludedORMModelElement exclusionLink = ExcludedORMModelElement.GetLinkToAbstractionModel(objectType);
					if (ShouldConsiderObjectType(objectType, null, true))
					{
						if (exclusionLink != null)
						{
							exclusionLink.Delete();
							AddObjectType(objectType);

							// Consider readding associated fact types
							foreach (Role playedRole in objectType.PlayedRoleCollection)
							{
								FactType factType = playedRole.FactType;
								if (IsElementExcluded(factType))
								{
									FilterModifiedFactType(factType, false);
								}
								RoleProxy proxy = playedRole.Proxy;
								if (proxy != null)
								{
									factType = proxy.FactType;
									if (IsElementExcluded(factType))
									{
										FilterModifiedFactType(factType, false);
									}
								}
							}

							// Consider readding subtypes excluded because this element was excluded
							if (!objectType.IsValueType)
							{
								// Excluding an object type can leave a downstream subtype without an
								// identifier, which excludes them. Note we only go one level deep
								// as this will recurse naturally on the next level.
								ObjectType.WalkSubtypes(objectType, delegate(ObjectType type, int depth, bool isPrimary)
								{
									switch (depth)
									{
										case 0:
											return ObjectTypeVisitorResult.Continue;
										case 1:
											if (isPrimary)
											{
												if (type.PreferredIdentifier == null)
												{
													FilterModifiedObjectType(type);
												}
											}
											return ObjectTypeVisitorResult.SkipChildren;
										default:
											return ObjectTypeVisitorResult.Stop;
									}
								});
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
					ExcludeFactType(factType, model, false, null);
				}
			}
			private static void ExcludeFactType(FactType factType, AbstractionModel model, bool forceCreate, NotifyORMElementExcluded notifyExcluded)
			{
				if (forceCreate ||
					null == ExcludedORMModelElement.GetAbstractionModel(factType))
				{
					if (null == factType.Objectification || factType.UnaryRole != null)
					{
						new ExcludedORMModelElement(factType, model);
					}
					if (notifyExcluded != null)
					{
						notifyExcluded(factType);
					}
					foreach (IFactConstraint constraint in factType.FactConstraintCollection)
					{
						ObjectType preferredFor = constraint.Constraint.PreferredIdentifierFor;
						if (preferredFor != null)
						{
							ExcludeObjectType(preferredFor, model, false, notifyExcluded);
						}
					}
				}
			}
			private static void ExcludeObjectType(ObjectType objectType)
			{
				AbstractionModel model = AbstractionModelIsForORMModel.GetAbstractionModel(objectType.Model);
				if (model != null)
				{
					ExcludeObjectType(objectType, model, false, null);
				}
			}
			private static void ExcludeObjectType(ObjectType objectType, AbstractionModel model, bool forceCreate, NotifyORMElementExcluded notifyExcluded)
			{
				if (forceCreate ||
					null == ExcludedORMModelElement.GetAbstractionModel(objectType))
				{
					new ExcludedORMModelElement(objectType, model);
					if (notifyExcluded != null)
					{
						notifyExcluded(objectType);
					}

					// Excluding an object type leaves a FactType with a null role player,
					// so the associated fact types also need to be excluded.
					foreach (Role playedRole in objectType.PlayedRoleCollection)
					{
						ExcludeFactType(playedRole.FactType, model, false, notifyExcluded);
						RoleProxy proxy = playedRole.Proxy;
						if (proxy != null)
						{
							ExcludeFactType(proxy.FactType, model, false, notifyExcluded);
						}
					}

					if (!objectType.IsValueType)
					{
						// Excluding an object type can leave a downstream subtype without an
						// identifier, so exclude those as well. Note we only go one level deep
						// as this will recurse naturally on the next level.
						ObjectType.WalkSubtypes(objectType, delegate(ObjectType type, int depth, bool isPrimary)
						{
							switch (depth)
							{
								case 0:
									return ObjectTypeVisitorResult.Continue;
								case 1:
									if (isPrimary)
									{
										if (type.PreferredIdentifier == null)
										{
											ExcludeObjectType(type, model, false, notifyExcluded);
										}
									}
									return ObjectTypeVisitorResult.SkipChildren;
								default:
									return ObjectTypeVisitorResult.Stop;
							}
						});
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
			[DelayValidatePriority(ValidationPriority.GatewayAddElement, DomainModelType = typeof(ORMCoreDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
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
			[DelayValidatePriority(ValidationPriority.GatewayAddElement, DomainModelType = typeof(ORMCoreDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
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
			[DelayValidatePriority(ValidationPriority.GatewayRemoveExcludedBridgeRelationships, DomainModelType = typeof(ORMCoreDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
			private static void ExclusionAdded(ModelElement element)
			{
				ExcludedORMModelElement link = (ExcludedORMModelElement)element;
				// The link may have been deleted since it was added, essentially readding the
				// object. This is the reason for the delayed validation: we don't want to
				// prematurely delete existing links
				if (!link.IsDeleted)
				{
					// Note that any objects deleted here need to be kept in sync with the ORMModelFixupListener implementation
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
			/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasFactType)
			/// Automatically exclude any newly added <see cref="FactType"/>s from consideration
			/// in the abstraction model.
			/// </summary>
			private static void FactTypeAddedRule(ElementAddedEventArgs e)
			{
				FrameworkDomainModel.DelayValidateElement(e.ModelElement, FilterNewFactType);
			}
			/// <summary>
			/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasObjectType)
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
			/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasEntityTypeRequiresReferenceSchemeError)
			/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasObjectTypeRequiresPrimarySupertypeError)
			/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasPreferredIdentifierRequiresMandatoryError)
			/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasCompatibleSupertypesError)
			/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasUnspecifiedDataTypeError)
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
			/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasEntityTypeRequiresReferenceSchemeError)
			/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasObjectTypeRequiresPrimarySupertypeError)
			/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasPreferredIdentifierRequiresMandatoryError)
			/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasCompatibleSupertypesError)
			/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasUnspecifiedDataTypeError)
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
			#region Preferred Identifier Tracking Rules
			/// <summary>
			/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier)
			/// Handle cases where subtypes have no preferred identifier. Note that this
			/// is not necessarily an error condition in the core model, so we will not always
			/// an error deleting notification on this object type, but this condition blocks
			/// absorption on directly and indirectly subtyped objects that now need to be reconsidered.
			/// </summary>
			private static void PreferredIdentifierAddedRule(ElementAddedEventArgs e)
			{
				ObjectType objectType = ((EntityTypeHasPreferredIdentifier)e.ModelElement).PreferredIdentifierFor;
				ObjectType.WalkSubtypes(objectType, delegate(ObjectType type, int depth, bool isPrimary)
				{
					if (isPrimary || depth == 0)
					{
						FilterModifiedObjectType(type);
					}
					return ObjectTypeVisitorResult.Continue;
				});
			}
			#endregion // Preferred Identifier Tracking Rules
			#region Objectification Tracking Rules
			/// <summary>
			/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification)
			/// Objectification FactTypes are automatically excluded, so changing
			/// the objectification state is the same as adding/removing a blocking
			/// error.
			/// </summary>
			private static void ObjectificationAddedRule(ElementAddedEventArgs e)
			{
				ProcessFactTypeForObjectificationAdded(((Objectification)e.ModelElement).NestedFactType);
			}
			private static void ProcessFactTypeForObjectificationAdded(FactType factType)
			{
				if (factType.UnaryRole == null)
				{
					ExcludedORMModelElement excludedLink = ExcludedORMModelElement.GetLinkToAbstractionModel(factType);
					if (excludedLink != null)
					{
						// We don't keep the exclusion link on objectified FactTypes, but deleting
						// it does not imply any additional processing because we were already not
						// considering this FactType
						excludedLink.Delete();
					}
					else
					{
						FilterModifiedFactType(factType, false); // false because new implied FactTypes will get notifications on their own
					}
				}
			}
			private static void ProcessFactTypeForObjectificationDeleted(FactType factType)
			{
				if (!factType.IsDeleted && factType.UnaryRole == null)
				{
					FilterModifiedFactType(factType, false); // false because there are no implied facttypes without an objectification
				}
			}
			/// <summary>
			/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification)
			/// </summary>
			private static void ObjectificationDeletedRule(ElementDeletedEventArgs e)
			{
				ProcessFactTypeForObjectificationDeleted(((Objectification)e.ModelElement).NestedFactType);
			}
			/// <summary>
			/// RolePlayerChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification)
			/// </summary>
			private static void ObjectificationRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			{
				Objectification link = (Objectification)e.ElementLink;
				if (e.DomainRole.Id == Objectification.NestedFactTypeDomainRoleId)
				{
					ProcessFactTypeForObjectificationDeleted((FactType)e.OldRolePlayer);
					ProcessFactTypeForObjectificationAdded(link.NestedFactType);
				}
			}
			#endregion // Objectification Tracking Rules
			#region RolePlayer tracking rules
			/// <summary>
			/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole)
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
			/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole)
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
			/// RolePlayerChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole)
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
			#region FactType derivation tracking rules
			/// <summary>
			/// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeDerivationExpression)
			/// Derived FactTypes should not be absorbed
			/// </summary>
			private static void FactTypeDerivationChangedRule(ElementPropertyChangedEventArgs e)
			{
				Guid propertyId = e.DomainProperty.Id;
				if (propertyId == FactTypeDerivationExpression.DerivationStorageDomainPropertyId)
				{
					DerivationStorageType oldStorage = (DerivationStorageType)e.OldValue;
					DerivationStorageType newStorage = (DerivationStorageType)e.NewValue;
					bool oldIgnoreFactType = oldStorage == DerivationStorageType.Derived;
					bool newIgnoreFactType = newStorage == DerivationStorageType.Derived;
					if (oldStorage != newStorage)
					{
						FactTypeDerivationExpression derivation = (FactTypeDerivationExpression)e.ModelElement;
						if (!derivation.IsDeleted)
						{
							FactType factType = derivation.FactType;
							if (!(factType is SubtypeFact))
							{
								FilterModifiedFactType(factType, true);
							}
						}
					}
				}
			}
			/// <summary>
			/// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasDerivationExpression)
			/// Derived FactTypes should not be absorbed
			/// </summary>
			private static void FactTypeDerivationAddedRule(ElementAddedEventArgs e)
			{
				FactTypeHasDerivationExpression link = (FactTypeHasDerivationExpression)e.ModelElement;
				if (link.DerivationRule.DerivationStorage == DerivationStorageType.Derived && !(link.FactType is SubtypeFact))
				{
					FilterModifiedFactType(link.FactType, true);
				}
			}
			/// <summary>
			/// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasDerivationExpression)
			/// Derived FactTypes should not be absorbed
			/// </summary>
			private static void FactTypeDerivationDeletedRule(ElementDeletedEventArgs e)
			{
				FactTypeHasDerivationExpression link = (FactTypeHasDerivationExpression)e.ModelElement;
				FactType factType = link.FactType;
				if (!factType.IsDeleted && link.DerivationRule.DerivationStorage == DerivationStorageType.Derived && !(factType is SubtypeFact))
				{
					FilterModifiedFactType(factType, true);
				}
			}
			#endregion // FactType derivation tracking rules
			#endregion // Rules to defer specific changes to general filter
		}
		#endregion // ORMElementGateway class
	}
}