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
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.Framework;

namespace Neumont.Tools.ORM.ObjectModel
{
	public partial class Objectification
	{
		// UNDONE: Handle unary objectifications (both implied and explicit)
		#region Implied Objectification creation, removal, and pattern enforcement
		#region ImpliedObjectificationConstraintRoleSequenceHasRoleAddRule class
		/// <summary>
		/// Creates a new implied Objectification if the implied objectification pattern is now present.
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		private sealed class ImpliedObjectificationConstraintRoleSequenceHasRoleAddRule : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				FactType factType = (e.ModelElement as ConstraintRoleSequenceHasRole).Role.FactType;
				if (factType != null)
				{
					ORMCoreModel.DelayValidateElement(factType, DelayProcessFactTypeForImpliedObjectification);
					ObjectType nestingType = factType.NestingType;
					if (nestingType != null && null == nestingType.PreferredIdentifier)
					{
						ORMCoreModel.DelayValidateElement(nestingType, DelayProcessObjectifyingTypeForPreferredIdentifier);
					}
				}
			}
		}
		#endregion // ImpliedObjectificationConstraintRoleSequenceHasRoleAddRule class
		#region ImpliedObjectificationConstraintRoleSequenceHasRoleDeletingRule class
		/// <summary>
		/// Removes an existing implied Objectification if the implied objectification pattern is no longer present.
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		private sealed class ImpliedObjectificationConstraintRoleSequenceHasRoleDeletingRule : DeletingRule
		{
			public sealed override void ElementDeleting(ElementDeletingEventArgs e)
			{
				ORMCoreModel.DelayValidateElement((e.ModelElement as ConstraintRoleSequenceHasRole).Role.FactType, DelayProcessFactTypeForImpliedObjectification);
			}
		}
		#endregion // ImpliedObjectificationConstraintRoleSequenceHasRoleDeletingRule class
		#region ImpliedObjectificationFactTypeHasRoleAddRule class
		/// <summary>
		/// 1) Creates a new implied Objectification if the implied objectification pattern is now present.
		/// 2) Changes an implied Objectification to being explicit if a Role in a non-implied FactType is played.
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole))]
		private sealed class ImpliedObjectificationFactTypeHasRoleAddRule : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasRole factTypeHasRole = e.ModelElement as FactTypeHasRole;
				ORMCoreModel.DelayValidateElement(factTypeHasRole.FactType, DelayProcessFactTypeForImpliedObjectification);
				ProcessNewPlayedRoleForImpliedObjectification(factTypeHasRole.Role as Role);
			}
		}
		#endregion // ImpliedObjectificationFactTypeHasRoleAddRule class
		#region ImpliedObjectificationFactTypeHasRoleDeletingRule class
		/// <summary>
		/// Removes an existing implied Objectification if the implied objectification pattern is no longer present.
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole))]
		private sealed class ImpliedObjectificationFactTypeHasRoleDeletingRule : DeletingRule
		{
			public sealed override void ElementDeleting(ElementDeletingEventArgs e)
			{
				ORMCoreModel.DelayValidateElement((e.ModelElement as FactTypeHasRole).FactType, DelayProcessFactTypeForImpliedObjectification);
			}
		}
		#endregion // ImpliedObjectificationFactTypeHasRoleDeletingRule class
		#region ImpliedObjectificationUniquenessConstraintIsInternalChangeRule class
		/// <summary>
		/// Adds or removes an implied Objectification if necessary.
		/// </summary>
		[RuleOn(typeof(UniquenessConstraint))]
		private sealed class ImpliedObjectificationUniquenessConstraintIsInternalChangeRule : ChangeRule
		{
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == UniquenessConstraint.IsInternalDomainPropertyId)
				{
					ProcessUniquenessConstraintForImpliedObjectification(e.ModelElement as UniquenessConstraint, true);
				}
			}
		}
		#endregion // ImpliedObjectificationUniquenessConstraintIsInternalChangeRule class
		#region UniquessConstraintAddRule class
		/// <summary>
		/// Ensure that an objectifying type with a single candidate internal uniqueness
		/// constraint on the objectified fact uses that constraint as its preferred identifier.
		/// </summary>
		[RuleOn(typeof(ModelHasSetConstraint))]
		private sealed class UniquenessConstraintAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				UniquenessConstraint constraint = (e.ModelElement as ModelHasSetConstraint).SetConstraint as UniquenessConstraint;
				if (constraint != null && constraint.IsInternal)
				{
					LinkedElementCollection<FactType> facts = constraint.FactTypeCollection;
					if (facts.Count != 0)
					{
						ObjectType nestingType = facts[0].NestingType;
						if (nestingType != null)
						{
							ORMCoreModel.DelayValidateElement(nestingType, DelayProcessObjectifyingTypeForPreferredIdentifier);
						}
					}
				}
			}
		}
		#endregion // UniquessConstraintAddRule class
		#region UniquessConstraintDeletingRule class
		/// <summary>
		/// Ensure that an objectifying type with a single candidate internal uniqueness
		/// constraint on the objectified fact uses that constraint as its preferred identifier.
		/// </summary>
		[RuleOn(typeof(ModelHasSetConstraint))]
		private sealed class UniquenessConstraintDeletingRule : DeletingRule
		{
			public override void ElementDeleting(ElementDeletingEventArgs e)
			{
				UniquenessConstraint constraint = (e.ModelElement as ModelHasSetConstraint).SetConstraint as UniquenessConstraint;
				if (constraint != null && constraint.IsInternal)
				{
					LinkedElementCollection<FactType> facts = constraint.FactTypeCollection;
					FactType testFact;
					Objectification objectification;
					if (facts.Count != 0 &&
						!(testFact = facts[0]).IsDeleting &&
						null != (objectification = testFact.Objectification) &&
						!objectification.IsDeleting)
					{
						ORMCoreModel.DelayValidateElement(objectification.NestingType, DelayProcessObjectifyingTypeForPreferredIdentifier);
					}
				}
			}
		}
		#endregion // UniquessConstraintDeletingRule class
		#region PreferredIdentifierDeletingRule class
		/// <summary>
		/// Make sure than an objectifying type gets a preferred identifier if
		/// the existing preferred identifier is deleted and a single internal
		/// uniqueness constraint on the objectified fact is available.
		/// </summary>
		[RuleOn(typeof(EntityTypeHasPreferredIdentifier))]
		private sealed class PreferredIdentifierDeletingRule : DeletingRule
		{
			public override void  ElementDeleting(ElementDeletingEventArgs e)
			{
				ObjectType objectType = (e.ModelElement as EntityTypeHasPreferredIdentifier).PreferredIdentifierFor;
				Objectification objectification;
				if (!objectType.IsDeleting &&
					null != (objectification = objectType.Objectification) &&
					!objectification.IsDeleting)
				{
					ORMCoreModel.DelayValidateElement(objectType, DelayProcessObjectifyingTypeForPreferredIdentifier);
				}
			}
		}
		#endregion // PreferredIdentifierDeletingRule class
		#region ImpliedObjectificationIsImpliedChangeRule class
		/// <summary>
		/// Validates that an objectification that is implied matches the implied objectification pattern.
		/// </summary>
		[RuleOn(typeof(Objectification))]
		private sealed class ImpliedObjectificationIsImpliedChangeRule : ChangeRule
		{
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == Objectification.IsImpliedDomainPropertyId && (bool)e.NewValue)
				{
					Objectification objectification = e.ModelElement as Objectification;
					
					// Check the implication pattern
					ProcessFactTypeForImpliedObjectification(objectification.NestedFactType, true);
					
					// If we're still objectified, check that we are only doing things that are allowed
					if (objectification.IsImplied)
					{
						ObjectType nestingType = objectification.NestingType;
						if (nestingType != null && (!nestingType.IsIndependent || nestingType.PlayedRoleCollection.Count != objectification.ImpliedFactTypeCollection.Count))
						{
							throw InvalidImpliedObjectificationException();
						}
					}
				}
			}
		}
		#endregion // ImpliedObjectificationIsImpliedChangeRule class
		#region ImpliedObjectificationObjectifyingTypeIsIndependentChangeRule class
		/// <summary>
		/// Changes an implied Objectification to being explicit if IsIndependent is changed.
		/// </summary>
		[RuleOn(typeof(ObjectType))]
		private sealed class ImpliedObjectificationObjectifyingTypeIsIndependentChangeRule : ChangeRule
		{
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == ObjectType.IsIndependentDomainPropertyId && !(bool)e.NewValue)
				{
					ObjectType nestingType = e.ModelElement as ObjectType;
					FactType nestedFact = nestingType.NestedFactType;
					Objectification objectification;
					if (nestedFact != null && (objectification = nestedFact.Objectification).IsImplied)
					{
						objectification.IsImplied = false;
					}
				}
			}
		}
		#endregion // ImpliedObjectificationObjectifyingTypeIsIndependentChangeRule class
		#region ImpliedObjectificationObjectifyingTypePlaysRoleAddRule class
		/// <summary>
		/// Changes an implied Objectification to being explicit if a Role in a non-implied FactType is played.
		/// </summary>
		[RuleOn(typeof(ObjectTypePlaysRole))]
		private sealed class ImpliedObjectificationObjectifyingTypePlaysRoleAddRule : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ProcessNewPlayedRoleForImpliedObjectification((e.ModelElement as ObjectTypePlaysRole).PlayedRole);
			}
		}
		#endregion // ImpliedObjectificationObjectifyingTypePlaysRoleAddRule class
		#endregion // Implied Objectification creation, removal, and pattern enforcement
		#region Objectification implied facts and constraints pattern enforcement
		#region ObjectificationAddRule class
		/// <summary>
		/// Create implied facts and constraints when an item is objectified
		/// </summary>
		[RuleOn(typeof(Objectification))]
		private sealed class ObjectificationAddRule : AddRule
		{
			/// <summary>
			/// Create implied facts and constraints as needed
			/// </summary>
			/// <param name="objectification">The objectification relationship to process</param>
			/// <param name="nestedFactType">The nested fact to process. Pulled from objectification.NestedFactType if null.</param>
			/// <param name="nestingType">The nesting object type to process. Pulled from objectification.NestingType if null.</param>
			public static void ProcessObjectificationAdded(Objectification objectification, FactType nestedFactType, ObjectType nestingType)
			{
				if (nestedFactType == null)
				{
					nestedFactType = objectification.NestedFactType;
				}
				if (nestedFactType.ImpliedByObjectification != null)
				{
					throw new InvalidOperationException(ResourceStrings.ModelExceptionObjectificationImpliedFactObjectified);
				}
				if (nestingType == null)
				{
					nestingType = objectification.NestingType;
				}
				ORMCoreModel.DelayValidateElement(nestingType, DelayProcessObjectifyingTypeForPreferredIdentifier);
				Store store = nestedFactType.Store;
				ORMModel model = nestedFactType.Model;

				// Comments in this and other related procedures will refer to
				// the 'near' end and 'far' end of the implied elements. The
				// near end refers to the nested fact type and its role players, the
				// far end is the nesting type. The pattern specifies that a binary fact type
				// is implied between each role player and the objectified type. The far
				// role players on the implied fact types always have a single role internal
				// constraint and a simple mandatory constraint.

				// Once we have a model, we can begin to add implied
				// facts and constraints. Note that we do not set the
				// ImpliedByObjectification property on any object
				// until all are completed because any modifications
				// to these implied elements is strictly monitored once
				// this relationship is established.

				// Add implied fact types, one for each role
				LinkedElementCollection<RoleBase> roles = nestedFactType.RoleCollection;
				int roleCount = roles.Count;
				if (roleCount != 0)
				{
					bool ruleDisabled = false;
					try
					{
						for (int i = 0; i < roleCount; ++i)
						{
							Role role = roles[i].Role;
							RoleProxy proxy = role.Proxy;
							if (proxy == null)
							{
								CreateImpliedFactTypeForRole(model, nestingType, role, objectification);
							}
							else
							{
								RoleBase oppositeRoleBase;
								Role oppositeRole;
								if (null != (oppositeRoleBase = proxy.OppositeRole) &&
									null != (oppositeRole = oppositeRoleBase as Role) &&
									(nestingType != oppositeRole.RolePlayer))
								{
									// Move an existing proxy fact to the correct nesting type
									if (!ruleDisabled)
									{
										store.RuleManager.DisableRule(typeof(RolePlayerAddRule));
										ruleDisabled = true;
									}
									oppositeRole.RolePlayer = nestingType;
								}
							}
						}
					}
					finally
					{
						if (ruleDisabled)
						{
							store.RuleManager.EnableRule(typeof(RolePlayerAddRule));
						}
					}
				}
			}
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ProcessObjectificationAdded(e.ModelElement as Objectification, null, null);
			}
		}
		#endregion // ObjectificationAddRule class
		#region ObjectificationDeleteRule class
		/// <summary>
		/// Remove the implied objectifying ObjectType when Objectification is removed.
		/// </summary>
		[RuleOn(typeof(Objectification))]
		private sealed class ObjectificationDeleteRule : DeleteRule
		{
			/// <summary>
			/// Remove the implied objectifying ObjectType when Objectification is removed
			/// and delay validated the previously nested fact type.
			/// </summary>
			/// <param name="objectification">The objectification relationship to process</param>
			/// <param name="nestedFactType">The nested fact to process. Pulled from objectification.NestedFactType if null.</param>
			/// <param name="nestingType">The nesting object type to process. Pulled from objectification.NestingType if null.</param>
			public static void ProcessObjectificationDeleted(Objectification objectification, FactType nestedFactType, ObjectType nestingType)
			{
				if (objectification.IsImplied)
				{
					if (nestingType == null)
					{
						nestingType = objectification.NestingType;
					}
					if (!nestingType.IsDeleted)
					{
						nestingType.Delete();
					}
				}
				else
				{
					if (nestedFactType == null)
					{
						nestedFactType = objectification.NestedFactType;
					}
					if (!nestedFactType.IsDeleted)
					{
						ORMCoreModel.DelayValidateElement(nestedFactType, DelayProcessFactTypeForImpliedObjectification);
					}
				}
			}
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				ProcessObjectificationDeleted(e.ModelElement as Objectification, null, null);
			}
		}
		#endregion // ObjectificationDeleteRule class
		#region ObjectificationRolePlayerChangeRule class
		/// <summary>
		/// Process Objectification role player changes
		/// </summary>
		[RuleOn(typeof(Objectification))]
		private class ObjectificationRolePlayerChangeRule : RolePlayerChangeRule
		{
			public override void RolePlayerChanged(RolePlayerChangedEventArgs e)
			{
				Objectification link = e.ElementLink as Objectification;
				if (link.IsDeleted)
				{
					return;
				}
				Guid changedRoleGuid = e.DomainRole.Id;
				ObjectType oldObjectType = null;
				FactType oldFactType = null;
				if (changedRoleGuid == Objectification.NestingTypeDomainRoleId)
				{
					oldObjectType = (ObjectType)e.OldRolePlayer;
				}
				else
				{
					oldFactType = (FactType)e.OldRolePlayer;
				}
				ObjectificationDeleteRule.ProcessObjectificationDeleted(link, oldFactType, oldObjectType);
				ObjectificationAddRule.ProcessObjectificationAdded(link, null, null);
			}
		}
		#endregion // ObjectificationRolePlayerChangeRule class
		#region ImpliedFactTypeAddRule class
		/// <summary>
		/// Rule class to block objectification of implied facts
		/// </summary>
		[RuleOn(typeof(ObjectificationImpliesFactType))]
		private sealed class ImpliedFactTypeAddRule : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ObjectificationImpliesFactType link = e.ModelElement as ObjectificationImpliesFactType;
				if (link.ImpliedFactType.Objectification != null)
				{
					throw new InvalidOperationException(ResourceStrings.ModelExceptionObjectificationImpliedFactObjectified);
				}
			}
		}
		#endregion // ImpliedFactTypeAddRule class
		#region RoleAddRule class
		/// <summary>
		/// Synchronize implied fact types when a role is added
		/// to the nested type.
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole)), RuleOn(typeof(ConstraintRoleSequenceHasRole)), RuleOn(typeof(FactSetConstraint))]
		private sealed class RoleAddRule : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelElement element = e.ModelElement;
				ConstraintRoleSequenceHasRole sequenceRoleLink;
				FactSetConstraint internalConstraintLink;
				FactType fact;
				Objectification objectificationLink;
				bool disallowed = false;
				if (null != (sequenceRoleLink = element as ConstraintRoleSequenceHasRole))
				{
					ConstraintRoleSequence modifiedSequence = sequenceRoleLink.ConstraintRoleSequence;
					IConstraint constraint = modifiedSequence.Constraint;
					if (constraint != null)
					{
						ConstraintType constraintType = constraint.ConstraintType;
						switch (constraintType)
						{
							case ConstraintType.SimpleMandatory:
							case ConstraintType.InternalUniqueness:
								// Do not allow direct modification. This rule is disabled
								// when constraints on existing fact types are modified
								LinkedElementCollection<FactType> facts = (constraint as SetConstraint).FactTypeCollection;
								if (facts.Count == 1)
								{
									fact = facts[0];
									if (null != fact.ImpliedByObjectification)
									{
										disallowed = true; // We don't trigger adds when this rule is active
									}
								}
								break;
						}
					}
				}
				else if (null != (internalConstraintLink = element as FactSetConstraint))
				{
					if (internalConstraintLink.SetConstraint.Constraint.ConstraintIsInternal)
					{
						disallowed = null != internalConstraintLink.FactType.ImpliedByObjectification;
					}
				}
				else
				{
					FactTypeHasRole factRoleLink = element as FactTypeHasRole;
					fact = factRoleLink.FactType;
					if (null != (objectificationLink = fact.ImpliedByObjectification))
					{
						// Our code only adds these before linking the implied objectification,
						// so we always throw at this point
						disallowed = true;
					}
					else if (null != (objectificationLink = fact.Objectification))
					{
						ObjectType nestingType = objectificationLink.NestingType;
						Role nestedRole = factRoleLink.Role.Role;

						// Create and populate new fact type
						if (nestedRole.Proxy == null)
						{
							CreateImpliedFactTypeForRole(nestingType.Model, nestingType, nestedRole, objectificationLink);
						}
					}
				}

				// Throw if the modification was disallowed by the objectification pattern
				if (disallowed)
				{
					throw BlockedByObjectificationPatternException();
				}
			}
		}
		#endregion // RoleAddRule class
		#region RoleDeletingRule class
		/// <summary>
		/// Synchronize implied fact types when a role is removed from
		/// the nested type.
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole)), RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		private sealed class RoleDeletingRule : DeletingRule
		{
			private bool myAllowModification;
			public sealed override void ElementDeleting(ElementDeletingEventArgs e)
			{
				ModelElement element = e.ModelElement;
				ConstraintRoleSequenceHasRole sequenceRoleLink;
				FactType fact;
				Objectification objectificationLink;
				bool disallowed = false;
				if (null != (sequenceRoleLink = element as ConstraintRoleSequenceHasRole))
				{
					if (!sequenceRoleLink.Role.IsDeleting)
					{
						ConstraintRoleSequence sequence = sequenceRoleLink.ConstraintRoleSequence;
						IConstraint constraint = sequence.Constraint;
						if (constraint != null)
						{
							ConstraintType constraintType = constraint.ConstraintType;
							switch (constraintType)
							{
								case ConstraintType.SimpleMandatory:
								case ConstraintType.InternalUniqueness:
									// Do not allow direct modification. This rule is disabled
									// when constraints on existing fact types are modified
									LinkedElementCollection<FactType> facts = (constraint as SetConstraint).FactTypeCollection;
									if (facts.Count == 1)
									{
										fact = facts[0];
										if (null != (objectificationLink = fact.ImpliedByObjectification))
										{
											disallowed = !myAllowModification && !objectificationLink.IsDeleting;
										}
									}
									break;
							}
						}
					}
				}
				else
				{
					FactTypeHasRole factRoleLink = element as FactTypeHasRole;
					fact = factRoleLink.FactType;
					if (null != (objectificationLink = fact.ImpliedByObjectification))
					{ 
						// Our code only adds these before linking the implied objectification,
						// so we always throw at this point
						if (!myAllowModification && !objectificationLink.IsDeleting)
						{
							disallowed = true;
							RoleProxy proxy;
							Role proxyRole;
							if (null != (proxy = factRoleLink.Role as RoleProxy) &&
								null != (proxyRole = proxy.Role))
							{
								disallowed = !proxyRole.IsDeleting;
							}
						}
					}
					else if (null != (objectificationLink = fact.Objectification))
					{
						if (!objectificationLink.IsDeleting)
						{
							try
							{
								myAllowModification = true;
								Role nestedRole = factRoleLink.Role.Role;
								RoleProxy proxyRole = nestedRole.Proxy;
								Debug.Assert(proxyRole != null, "Proxy is not attached. Safe to ignore, but code path unexpected.");
								if (proxyRole != null)
								{
									proxyRole.FactType.Delete();
								}
							}
							finally
							{
								myAllowModification = false;
							}
						}
					}
				}

				// Throw if the modification was disallowed by the objectification pattern
				if (disallowed)
				{
					throw BlockedByObjectificationPatternException();
				}
			}
		}
		#endregion // RoleDeletingRule class
		#region RolePlayerAddRule class
		/// <summary>
		/// Synchronize implied fact types when a role player is
		/// set on an objectified role
		/// </summary>
		[RuleOn(typeof(ObjectTypePlaysRole))]
		private sealed class RolePlayerAddRule : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				Role role = link.PlayedRole;
				FactType fact = role.FactType;
				if (fact != null)
				{
					if (null != fact.ImpliedByObjectification)
					{
						throw BlockedByObjectificationPatternException();
					}
				}
			}
		}
		#endregion // RolePlayerAddRule class
		#region RolePlayerDeletingRule class
		/// <summary>
		/// Synchronize implied fact types when a role player is
		/// being removed from an objectified role
		/// </summary>
		[RuleOn(typeof(ObjectTypePlaysRole))]
		private sealed class RolePlayerDeletingRule : DeletingRule
		{
			public sealed override void ElementDeleting(ElementDeletingEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				Role role = link.PlayedRole;
				ObjectType rolePlayer = link.RolePlayer;
				FactType fact = role.FactType;
				// Note if the roleplayer is removed, then the links all go away
				// automatically. There is no additional work to do or checks to make.
				if (!(rolePlayer.IsDeleted || rolePlayer .IsDeleting) &&
					(null != (fact = role.FactType)))
				{
					Objectification objectificationLink;
					if (null != (objectificationLink = fact.ImpliedByObjectification))
					{
						if (!(objectificationLink.IsDeleting || role.IsDeleting))
						{
							throw BlockedByObjectificationPatternException();
						}
					}
				}
			}
		}
		#endregion // RolePlayerDeletingRule class
		#region InternalConstraintChangeRule class
		/// <summary>
		/// Ensure that implied internal constraints cannot change the Modality property
		/// </summary>
		[RuleOn(typeof(SetConstraint))]
		private sealed class InternalConstraintChangeRule : ChangeRule
		{
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				Guid attributeId = e.DomainProperty.Id;
				if (attributeId == SetConstraint.ModalityDomainPropertyId)
				{
					SetConstraint constraint = e.ModelElement as SetConstraint;
					if (constraint.Constraint.ConstraintIsInternal)
					{
						LinkedElementCollection<FactType> facts;
						if (1 == (facts = constraint.FactTypeCollection).Count &&
							facts[0].ImpliedByObjectification != null)
						{
							throw BlockedByObjectificationPatternException();
						}
					}
				}
			}
		}
		#endregion // InternalConstraintChangeRule class
		#endregion // Objectification implied facts and constraints pattern enforcement
		#region Helper functions
		/// <summary>
		/// If a newly played Role is in a non-implied FactType, then the role player cannot be an implied objectifying ObjectType
		/// </summary>
		private static void ProcessNewPlayedRoleForImpliedObjectification(Role playedRole)
		{
			if (playedRole != null)
			{
				FactType playedFact = playedRole.FactType;
				if (playedFact != null)
				{
					LinkedElementCollection<RoleBase> roles;
					// If the fact is implied, we don't need to do anything else
					if (playedFact.ImpliedByObjectification != null || ((roles = playedFact.RoleCollection).Count > 0 && roles[0] is RoleProxy))
					{
						return;
					}

					ObjectType nestingType = playedRole.RolePlayer;
					Objectification objectification;
					if (nestingType != null && (objectification = nestingType.Objectification) != null && objectification.IsImplied)
					{
						// The newly-played role is in a non-implied fact, so the objectification is no longer implied
						objectification.IsImplied = false;
					}
				}
			}
		}
		private static void ProcessUniquenessConstraintForImpliedObjectification(UniquenessConstraint uniquenessConstraint, bool changingIsInternal)
		{
			if (uniquenessConstraint == null || (!uniquenessConstraint.IsInternal && !changingIsInternal))
			{
				return;
			}
			LinkedElementCollection<FactType> facts = uniquenessConstraint.FactTypeCollection;
			int factsCount = facts.Count;
			for (int i = 0; i < factsCount; ++i)
			{
				ORMCoreModel.DelayValidateElement(facts[i], DelayProcessFactTypeForImpliedObjectification);
			}
		}
		/// <summary>
		/// Delay validation callback for implied objectification
		/// </summary>
		private static void DelayProcessFactTypeForImpliedObjectification(ModelElement element)
		{
			ProcessFactTypeForImpliedObjectification(element as FactType, false);
		}
		/// <summary>
		/// Check for the implied objectification pattern, and add or remove it as appropriate.
		/// </summary>
		private static void ProcessFactTypeForImpliedObjectification(FactType factType, bool throwOnFailure)
		{
			// We don't need to process implied FactTypes, since they can never be objectified
			if (factType == null || factType.IsDeleted || factType.ImpliedByObjectification != null)
			{
				return;
			}

			Objectification objectification = factType.Objectification;
			if (objectification != null)
			{
				if (objectification.IsImplied)
				{
					// See if we have more than two roles
					if (factType.RoleCollection.Count > 2)
					{
						return;
					}
					// Make sure that we still have a uniqueness constraint that implies the objectification
					foreach (UniquenessConstraint uniquenessConstraint in factType.GetInternalConstraints<UniquenessConstraint>())
					{
						if (uniquenessConstraint.RoleCollection.Count > 1)
						{
							return;
						}
					}
					// We don't have enough roles or a uniqueness constraint that implies the objectification, so get rid of
					// the implied objectification
					if (throwOnFailure)
					{
						throw InvalidImpliedObjectificationException();
					}
					else
					{
						objectification.NestingType.Delete();
					}
				}
			}
			else
			{
				// See if we have more than two roles
				if (factType.RoleCollection.Count > 2)
				{
					CreateObjectificationForFactType(factType, true, null);
					return;
				}
				// See if we now have a uniqueness constraint that implies an objectification
				foreach (UniquenessConstraint uniquenessConstraint in factType.GetInternalConstraints<UniquenessConstraint>())
				{
					if (uniquenessConstraint.RoleCollection.Count > 1)
					{
						// We now have a uniqueness constraint that implies an objectification, so create it
						CreateObjectificationForFactType(factType, true, null);
						return;
					}
				}
			}
		}
		/// <summary>
		/// Creates an <see cref="Objectification"/> for the specified <see cref="FactType"/>.
		/// NOTE: If <paramref name="isImplied"/> is <see langword="true"/>, it is the caller's responsibility to ensure that the
		/// specified <see cref="FactType"/> matches the implied <see cref="Objectification"/> pattern.
		/// </summary>
		public static void CreateObjectificationForFactType(FactType factType, bool isImplied, INotifyElementAdded notifyAdded)
		{
			Store store = factType.Store;
			ObjectType objectifyingType = new ObjectType(store,
				new PropertyAssignment(ObjectType.NameDomainPropertyId, factType.Name),
				new PropertyAssignment(ObjectType.IsIndependentDomainPropertyId, isImplied));
			new Objectification(store,
				new RoleAssignment[]
				{
					new RoleAssignment(Objectification.NestedFactTypeDomainRoleId, factType),
					new RoleAssignment(Objectification.NestingTypeDomainRoleId, objectifyingType)
				},
				new PropertyAssignment[]
				{
					new PropertyAssignment(Objectification.IsImpliedDomainPropertyId, isImplied)
				});
			if (notifyAdded == null)
			{
				Dictionary<object, object> contextInfo = store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
				try
				{
					contextInfo[ObjectType.AllowDuplicateObjectNamesKey] = null;
					objectifyingType.Model = factType.Model;
				}
				finally
				{
					contextInfo.Remove(ObjectType.AllowDuplicateObjectNamesKey);
				}
			}
			else
			{
				objectifyingType.Model = factType.Model;
				// The true addLinks parameter here will pick up both the Objectification and
				// the ModelHasObjectType links, so is sufficient to get all of the elements we
				// created here.
				notifyAdded.ElementAdded(objectifyingType, true);
			}

			// If the implied fact has a single internal uniqueness constraint
			// then automatically use it as the preferred identifier.
			UniquenessConstraint preferredConstraint = null;
			if (notifyAdded == null)
			{
				foreach (UniquenessConstraint candidateConstraint in factType.GetInternalConstraints<UniquenessConstraint>())
				{
					if (preferredConstraint != null)
					{
						// We found more than one, don't use it
						preferredConstraint = null;
						break;
					}
					else if (candidateConstraint.PreferredIdentifierFor != null)
					{
						break;
					}
					preferredConstraint = candidateConstraint;
				}
				if (preferredConstraint != null)
				{
					objectifyingType.PreferredIdentifier = preferredConstraint;
				}
			}
			else
			{
				// This is called during fixup potentially before the fact/constraint relationships
				// are established, so we need to use the primary relationship (the role) to
				// find what we're after.
				LinkedElementCollection<RoleBase> factRoles = factType.RoleCollection;
				int factRoleCount = factRoles.Count;
				bool breakOut = false;
				for (int i = 0; i < factRoleCount && !breakOut; ++i)
				{
					Role role = (Role)factRoles[i]; // This must be a role, not a proxy, use exception cast
					LinkedElementCollection<ConstraintRoleSequence> sequences = role.ConstraintRoleSequenceCollection;
					int sequenceCount = sequences.Count;
					for (int j = 0; j < sequenceCount; ++j)
					{
						UniquenessConstraint candidateConstraint;
						if (null != (candidateConstraint = sequences[j] as UniquenessConstraint) &&
							candidateConstraint.IsInternal)
						{
							if (preferredConstraint != null)
							{
								if (candidateConstraint == preferredConstraint)
								{
									continue;
								}
								// We found more than one, don't use it
								preferredConstraint = null;
								breakOut = true;
								break;
							}
							else if (candidateConstraint.PreferredIdentifierFor != null)
							{
								breakOut = true;
								break;
							}
							preferredConstraint = candidateConstraint;
						}
					}
				}
				if (preferredConstraint != null)
				{
					notifyAdded.ElementAdded(new EntityTypeHasPreferredIdentifier(objectifyingType, preferredConstraint), false);
				}
			}
		}
		/// <summary>
		/// Create an implied fact type, set its far constraints, and add
		/// it to the model. Associating the implied fact with the objectifying
		/// relationship is delayed and left to the caller to avoid triggering
		/// rules prematurely.
		/// </summary>
		/// <param name="model">The model to include the fact in</param>
		/// <param name="nestingType">The objectifying object type</param>
		/// <param name="nestedRole">The role associated with this element</param>
		/// <param name="objectification">The Objectification that implies the FactType</param>
		/// <returns>The created fact type</returns>
		private static FactType CreateImpliedFactTypeForRole(ORMModel model, ObjectType nestingType, Role nestedRole, Objectification objectification)
		{
			// Create the implied fact and attach roles to it
			Store store = model.Store;
			FactType impliedFact = new FactType(store);
			RoleProxy nearRole = new RoleProxy(store);
			nearRole.TargetRole = nestedRole;
			Role farRole = new Role(store);
			LinkedElementCollection<RoleBase> impliedRoles = impliedFact.RoleCollection;
			impliedRoles.Add(nearRole);
			impliedRoles.Add(farRole);

			// Add standard constraints and role players
			MandatoryConstraint.CreateSimpleMandatoryConstraint(farRole);
			UniquenessConstraint.CreateInternalUniquenessConstraint(store).RoleCollection.Add(farRole);
			farRole.RolePlayer = nestingType;

			// UNDONE: Each of the readings need to be modified if we're in
			// a ring situation. The replacement values needs to be the (1-based)
			// number of the occurrence of that role player type in the collection.
			// Alternately, the readings we'll be able to generate are so ugly anyway
			// that the validation error with a direct jump to improve them will actually
			// be beneficial, not harmful, so we should not try to automatically repair
			// the readings at this point.

			// Add forward reading
			LinkedElementCollection<ReadingOrder> readingOrders = impliedFact.ReadingOrderCollection;
			ReadingOrder order = new ReadingOrder(store);
			LinkedElementCollection<RoleBase> orderRoles;
			readingOrders.Add(order);
			orderRoles = order.RoleCollection;
			orderRoles.Add(nearRole);
			orderRoles.Add(farRole);
			Reading reading = new Reading(store);
			reading.ReadingOrder = order;
			reading.Text = ResourceStrings.ImpliedFactTypePredicateReading;

			// Add inverse reading
			order = new ReadingOrder(store);
			readingOrders.Add(order);
			orderRoles = order.RoleCollection;
			orderRoles.Add(farRole);
			orderRoles.Add(nearRole);
			reading = new Reading(store);
			reading.ReadingOrder = order;
			reading.Text = ResourceStrings.ImpliedFactTypePredicateInverseReading;

			// Attach the objectification to the fact
			impliedFact.ImpliedByObjectification = objectification;

			// Attach the fact to the model
			impliedFact.Model = model;
			return impliedFact;
		}
		/// <summary>
		/// Creates an Objectification between the specified FactType and ObjectType. If the FactType already has an implied
		/// Objectification, it will be merged with the new objectifying ObjectType.
		/// </summary>
		public static void CreateExplicitObjectification(FactType nestedFactType, ObjectType nestingType)
		{
			Objectification objectification = nestedFactType.Objectification;
			if (objectification != null && objectification.IsImplied)
			{
				// Ignore the nestingType being set to null if the current Objectification is implied
				if (nestingType != null)
				{
					// We already have an implied Objectification, so modify it for the new ObjectType
					RuleManager ruleManager = objectification.Store.RuleManager;
					ObjectType impliedObjectifyingType = objectification.NestingType;
					objectification.NestingType = nestingType;
					bool addRuleDisabled = false;
					bool removingRuleDisabled = false;
					try
					{
						ruleManager.DisableRule(typeof(RolePlayerAddRule));
						addRuleDisabled = true;
						ruleManager.DisableRule(typeof(RolePlayerDeletingRule));
						removingRuleDisabled = true;
						foreach (FactType impliedFactType in objectification.ImpliedFactTypeCollection)
						{
							LinkedElementCollection<RoleBase> roles = impliedFactType.RoleCollection;
							Debug.Assert(roles.Count == 2,
								"When this method is called, we should be at a stable point, so implied Fact Types should always have exactly two Roles.");
							Role role = roles[1] as Role;
							if (role == null)
							{
								role = roles[0] as Role;
								Debug.Assert(role != null);
							}
							role.RolePlayer = nestingType;
						}
					}
					finally
					{
						if (addRuleDisabled)
						{
							ruleManager.EnableRule(typeof(RolePlayerAddRule));
						}
						if (removingRuleDisabled)
						{
							ruleManager.EnableRule(typeof(RolePlayerDeletingRule));
						}
					}
					objectification.IsImplied = false;
					impliedObjectifyingType.Delete();
				}
			}
			else
			{
				// We don't have an implied Objectification, so we don't need to do anything special.
				nestedFactType.NestingType = nestingType;
			}
		}
		/// <summary>
		/// Make sure that an objectifying EntityType without a preferred identifier attached
		/// to a fact type with one internal uniqueness constraint is assigned that uniqueness
		/// constraint as its preferred identifier.
		/// </summary>
		/// <param name="element">ObjectType to process</param>
		private static void DelayProcessObjectifyingTypeForPreferredIdentifier(ModelElement element)
		{
			ObjectType objectType;
			FactType nestedFactType;
			if (null != (objectType = element as ObjectType) &&
				!objectType.IsDeleted &&
				null == objectType.PreferredIdentifier &&
				null != (nestedFactType = objectType.NestedFactType))
			{
				UniquenessConstraint useConstraint = null;
				foreach (UniquenessConstraint testConstraint in nestedFactType.GetInternalConstraints<UniquenessConstraint>())
				{
					if (testConstraint.RoleCollection.Count != 0)
					{
						if (useConstraint == null)
						{
							useConstraint = testConstraint;
						}
						else
						{
							useConstraint = null;
							break;
						}
					}
				}
				if (useConstraint != null)
				{
					objectType.PreferredIdentifier = useConstraint;
				}
			}
		}
		#region Exception Helpers
		/// <summary>
		/// Returns an exception indicating that the current modification is
		/// not allowed by the objectification pattern.
		/// </summary>
		private static InvalidOperationException BlockedByObjectificationPatternException()
		{
			return new InvalidOperationException(ResourceStrings.ModelExceptionObjectificationImpliedElementModified);
		}
		/// <summary>
		/// Returns an exception indicating that the implied objectification is not actually
		/// implied or the nesting type is doing things that are not allowed.
		/// </summary>
		private static InvalidOperationException InvalidImpliedObjectificationException()
		{
			return new InvalidOperationException(ResourceStrings.ModelExceptionObjectificationImpliedMustBeImpliedAndIndependentAndCannotPlayRoleInNonImpliedFact);
		}
		#endregion // Exception Helpers
		#endregion // Helper functions
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// validates all model errors and adds errors to the task provider.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new ObjectificationFixupListener();
			}
		}
		/// <summary>
		/// A listener class to enforce valid subtype facts on load.
		/// Invalid subtype patterns will either be fixed up or completely
		/// removed.
		/// </summary>
		private sealed class ObjectificationFixupListener : DeserializationFixupListener<Objectification>
		{
			/// <summary>
			/// Create a new ObjectificationFixupListener
			/// </summary>
			public ObjectificationFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateImplicitStoredElements)
			{
			}
			/// <summary>
			/// Make sure the objectification pattern is present and correct.
			/// </summary>
			/// <param name="element">An Objectification instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(Objectification element, Store store, INotifyElementAdded notifyAdded)
			{
				// Note that this assumes xsd validation has occurred (RoleProxy is only on an ImpliedFact, there
				// is 1 Role and 1 RoleProxy, and implied facts must be attached to an Objectification relationship.
				FactType nestedFact = element.NestedFactType;
				ORMModel model = nestedFact.Model;
				ObjectType nestingType = element.NestingType;
				LinkedElementCollection<RoleBase> factRoles = nestedFact.RoleCollection;
				int factRolesCount = factRoles.Count;

				// Make sure each of the facts has a properly constructed role proxy
				for (int i = 0; i < factRolesCount; ++i)
				{
					Role factRole = (Role)factRoles[i];
					Role farRole = null; // The role on the implied fact with the nesting type as a role player
					FactType impliedFact = null;
					RoleProxy proxy = factRole.Proxy;
					if (proxy != null)
					{
						// Make sure the proxy is appropriate
						impliedFact = proxy.FactType;
						if (impliedFact != null && impliedFact.ImpliedByObjectification != element)
						{
							RemoveFact(impliedFact);
							impliedFact = null;
							Debug.Assert(proxy.IsDeleted); // Goes away with delete propagation on the fact
							proxy = null;
						}
						else
						{
							LinkedElementCollection<RoleBase> impliedRoles = impliedFact.RoleCollection;
							if (impliedRoles.Count != 2)
							{
								RemoveFact(impliedFact);
								impliedFact = null;
								proxy = null;
							}
							else
							{
								farRole = impliedRoles[0] as Role;
								if (farRole == null)
								{
									farRole = impliedRoles[1] as Role;
									Debug.Assert(farRole != null);
									if (farRole == null)
									{
										RemoveFact(impliedFact);
										impliedFact = null;
										proxy = null;
									}
								}
								if (farRole != null)
								{
									ObjectType testRolePlayer = farRole.RolePlayer;
									if (testRolePlayer != nestingType)
									{
										farRole.RolePlayer = nestingType;
										notifyAdded.ElementAdded(ObjectTypePlaysRole.GetRolePlayer(farRole));
									}
								}
							}
						}
					}
					if (proxy == null)
					{
						// Create the proxy role
						proxy = new RoleProxy(store);
						proxy.TargetRole = factRole;
						notifyAdded.ElementAdded(proxy, true);

						// Create the non-proxy role
						farRole = new Role(store);
						farRole.RolePlayer = nestingType;
						notifyAdded.ElementAdded(proxy, true);

						// Create the implied fact and set relationships to existing objects
						impliedFact = new FactType(store);
						proxy.FactType = impliedFact;
						farRole.FactType = impliedFact;
						impliedFact.ImpliedByObjectification = element;
						impliedFact.Model = model;
						notifyAdded.ElementAdded(impliedFact, true);

						// Add forward reading
						LinkedElementCollection<ReadingOrder> readingOrders = impliedFact.ReadingOrderCollection;
						ReadingOrder order = new ReadingOrder(store);
						LinkedElementCollection<RoleBase> orderRoles;
						readingOrders.Add(order);
						orderRoles = order.RoleCollection;
						orderRoles.Add(proxy);
						orderRoles.Add(farRole);
						Reading reading = new Reading(store);
						reading.ReadingOrder = order;
						reading.Text = ResourceStrings.ImpliedFactTypePredicateReading;
						notifyAdded.ElementAdded(order, true);
						notifyAdded.ElementAdded(reading, false);

						// Add inverse reading
						order = new ReadingOrder(store);
						readingOrders.Add(order);
						orderRoles = order.RoleCollection;
						orderRoles.Add(farRole);
						orderRoles.Add(proxy);
						reading = new Reading(store);
						reading.ReadingOrder = order;
						reading.Text = ResourceStrings.ImpliedFactTypePredicateInverseReading;
						notifyAdded.ElementAdded(order, true);
						notifyAdded.ElementAdded(reading, false);
					}

					// Make sure the internal constraint pattern is correct on the far role
					EnsureSingleColumnUniqueAndMandatory(store, model, farRole, notifyAdded);
				}

				// Verify that that are no innapropriate implied facts are attached to the objectification
				LinkedElementCollection<FactType> impliedFacts = element.ImpliedFactTypeCollection;
				int impliedFactCount = impliedFacts.Count;
				if (impliedFactCount != factRolesCount)
				{
					int leftToRemove = impliedFactCount - factRolesCount;
					Debug.Assert(impliedFactCount > factRolesCount); // We verified and/or added at least this many earlier
					for (int i = impliedFactCount - 1; i >= 0 && leftToRemove != 0; --i)
					{
						FactType impliedFact = impliedFacts[i];
						LinkedElementCollection<RoleBase> impliedRoles = impliedFact.RoleCollection;
						if (impliedRoles.Count != 2)
						{
							RemoveFact(impliedFact);
							--leftToRemove;
						}
						else
						{
							RoleProxy proxy = impliedRoles[0] as RoleProxy;
							if (proxy == null)
							{
								proxy = impliedRoles[1] as RoleProxy;
								Debug.Assert(proxy != null, "At least one role in an implied fact should be a proxy.");
							}
							Role targetRole;
							if (proxy == null ||
								null == (targetRole = proxy.Role) ||
								nestedFact != targetRole.FactType)
							{
								RemoveFact(impliedFact);
								--leftToRemove;
							}
						}
					}
				}

				// Verify the implication pattern
				if (element.IsImplied)
				{
					if (!nestingType.IsIndependent || nestingType.PlayedRoleCollection.Count > factRolesCount || factRolesCount < 2)
					{
						element.IsImplied = false;
					}
					else if (factRolesCount == 2)
					{
						// We require a multi-role internal uniqueness constraint to be implied
						// The internal constraints may not be attached to the fact yet
						// as this also happens during deserialization fixup, so get the
						// constraints from the non-derivable role connections
						bool canBeImplied = false;
						for (int i = 0; i < 2 && !canBeImplied; ++i)
						{
							Role role = (Role)factRoles[i].Role;
							LinkedElementCollection<ConstraintRoleSequence> constraints = role.ConstraintRoleSequenceCollection;
							int constraintCount = constraints.Count;
							for (int j = 0; j < constraintCount; ++j)
							{
								UniquenessConstraint uc = constraints[j] as UniquenessConstraint;
								if (uc != null && uc.IsInternal && uc.RoleCollection.Count > 1)
								{
									canBeImplied = true;
									break;
								}
							}
						}
						if (!canBeImplied)
						{
							element.IsImplied = false;
						}
					}
				}
			}
			/// <summary>
			/// Internal constraints are not fully connected at this point (FactSetConstraint instances
			/// are not implicitly constructed until a later phase), so we need to work a little harder
			/// to remove them.
			/// </summary>
			/// <param name="fact">The fact to clear of external constraints</param>
			private static void RemoveFact(FactType fact)
			{
				LinkedElementCollection<RoleBase> factRoles = fact.RoleCollection;
				int roleCount = factRoles.Count;
				for (int i = 0; i < roleCount; ++i)
				{
					Role role = factRoles[i].Role;
					LinkedElementCollection<ConstraintRoleSequence> sequences = role.ConstraintRoleSequenceCollection;
					int sequenceCount = sequences.Count;
					for (int j = sequenceCount - 1; j >= 0; --j)
					{
						SetConstraint ic = sequences[j] as SetConstraint;
						if (ic != null && ic.Constraint.ConstraintIsInternal)
						{
							ic.Delete();
						}
					}
				}
				fact.Delete();
			}
			private static void EnsureSingleColumnUniqueAndMandatory(Store store, ORMModel model, Role role, INotifyElementAdded notifyAdded)
			{
				LinkedElementCollection<ConstraintRoleSequence> sequences = role.ConstraintRoleSequenceCollection;
				int sequenceCount = sequences.Count;
				bool haveUniqueness = false;
				bool haveMandatory = false;
				SetConstraint ic;
				for (int i = sequenceCount - 1; i >= 0; --i)
				{
					ic = sequences[i] as SetConstraint;
					if (ic != null && ic.Constraint.ConstraintIsInternal)
					{
						if (ic.RoleCollection.Count == 1 && ic.Modality == ConstraintModality.Alethic)
						{
							switch (ic.Constraint.ConstraintType)
							{
								case ConstraintType.InternalUniqueness:
									if (haveUniqueness)
									{
										ic.Delete();
									}
									else
									{
										haveUniqueness = true;
									}
									break;
								case ConstraintType.SimpleMandatory:
									if (haveMandatory)
									{
										ic.Delete();
									}
									else
									{
										haveMandatory = true;
									}
									break;
							}
						}
						else
						{
							ic.Delete();
						}
					}
				}
				if (!haveUniqueness)
				{
					ic = UniquenessConstraint.CreateInternalUniquenessConstraint(store);
					ic.RoleCollection.Add(role);
					ic.Model = model;
					notifyAdded.ElementAdded(ic, true);
				}
				if (!haveMandatory)
				{
					ic = MandatoryConstraint.CreateSimpleMandatoryConstraint(store);
					ic.RoleCollection.Add(role);
					ic.Model = model;
					notifyAdded.ElementAdded(ic, true);
				}
			}
		}
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// verifies that all facts that should have implied objectifications
		/// have implied objectifications
		/// </summary>
		public static IDeserializationFixupListener ImpliedFixupListener
		{
			get
			{
				return new ImpliedObjectificationFixupListener();
			}
		}
		/// <summary>
		/// A listener class to enforce valid implied objectification patterns on load.
		/// Any fact that does not have a required implied objectification but should have
		/// one will be fixed. Unnecessary implied objectification elements are removed
		/// during Objectification validation.
		/// </summary>
		private sealed class ImpliedObjectificationFixupListener : DeserializationFixupListener<FactType>
		{
			/// <summary>
			/// Create a new SubtypeFactFixupListener
			/// </summary>
			public ImpliedObjectificationFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateImplicitStoredElements)
			{
			}
			/// <summary>
			/// Make sure that any fact type that does not have an implied objectification
			/// but needs one gets an implied objectification. Note that populating the implied
			/// objectification is left up to the other fixup listener.
			/// </summary>
			/// <param name="element">An FactType instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(FactType element, Store store, INotifyElementAdded notifyAdded)
			{
				if (!element.IsDeleted &&
					null == element.Objectification &&
					null == element.ImpliedByObjectification)
				{
					bool impliedRequired = false;
					LinkedElementCollection<RoleBase> roles = element.RoleCollection;
					int roleCount = roles.Count;
					if (roleCount > 2)
					{
						impliedRequired = true;
					}
					else
					{
						// The internal constraints may not be attached to the fact yet
						// as this also happens during deserialization fixup, so get the
						// constraints from the non-derived role connections
						for (int i = 0; i < roleCount && !impliedRequired; ++i)
						{
							Role role = roles[i].Role;
							LinkedElementCollection<ConstraintRoleSequence> constraints = role.ConstraintRoleSequenceCollection;
							int constraintCount = constraints.Count;
							for (int j = 0; j < constraintCount; ++j)
							{
								UniquenessConstraint uc = constraints[j] as UniquenessConstraint;
								if (uc != null && uc.IsInternal && uc.RoleCollection.Count > 1)
								{
									impliedRequired = true;
									break;
								}
							}
						}
					}
					if (impliedRequired)
					{
						CreateObjectificationForFactType(element, true, notifyAdded);
					}
				}
			}
		}
		#endregion Deserialization Fixup
	}
}
