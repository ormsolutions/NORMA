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
		private class ImpliedObjectificationConstraintRoleSequenceHasRoleAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ORMMetaModel.DelayValidateElement((e.ModelElement as ConstraintRoleSequenceHasRole).RoleCollection.FactType, DelayProcessFactTypeForImpliedObjectification);
			}
		}
		#endregion // ImpliedObjectificationConstraintRoleSequenceHasRoleAddRule class
		#region ImpliedObjectificationConstraintRoleSequenceHasRoleRemovingRule class
		/// <summary>
		/// Removes an existing implied Objectification if the implied objectification pattern is no longer present.
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		private class ImpliedObjectificationConstraintRoleSequenceHasRoleRemovingRule : RemovingRule
		{
			public override void ElementRemoving(ElementRemovingEventArgs e)
			{
				ORMMetaModel.DelayValidateElement((e.ModelElement as ConstraintRoleSequenceHasRole).RoleCollection.FactType, DelayProcessFactTypeForImpliedObjectification);
			}
		}
		#endregion // ImpliedObjectificationConstraintRoleSequenceHasRoleRemovingRule class
		#region ImpliedObjectificationFactTypeHasRoleAddRule class
		/// <summary>
		/// 1) Creates a new implied Objectification if the implied objectification pattern is now present.
		/// 2) Changes an implied Objectification to being explicit if a Role in a non-implied FactType is played.
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole))]
		private class ImpliedObjectificationFactTypeHasRoleAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasRole factTypeHasRole = e.ModelElement as FactTypeHasRole;
				ORMMetaModel.DelayValidateElement(factTypeHasRole.FactType, DelayProcessFactTypeForImpliedObjectification);
				ProcessNewPlayerRoleForImpliedObjectification(factTypeHasRole.RoleCollection as Role);
			}
		}
		#endregion // ImpliedObjectificationFactTypeHasRoleAddRule class
		#region ImpliedObjectificationFactTypeHasRoleRemovingRule class
		/// <summary>
		/// Removes an existing implied Objectification if the implied objectification pattern is no longer present.
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole))]
		private class ImpliedObjectificationFactTypeHasRoleRemovingRule : RemovingRule
		{
			public override void ElementRemoving(ElementRemovingEventArgs e)
			{
				ORMMetaModel.DelayValidateElement((e.ModelElement as FactTypeHasRole).FactType, DelayProcessFactTypeForImpliedObjectification);
			}
		}
		#endregion // ImpliedObjectificationFactTypeHasRoleRemovingRule class
		#region ImpliedObjectificationUniquenessConstraintIsInternalChangeRule class
		/// <summary>
		/// Adds or removes an implied Objectification if necessary.
		/// </summary>
		[RuleOn(typeof(UniquenessConstraint))]
		private class ImpliedObjectificationUniquenessConstraintIsInternalChangeRule : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				if (e.MetaAttribute.Id == UniquenessConstraint.IsInternalMetaAttributeGuid)
				{
					ProcessUniquenessConstraintForImpliedObjectification(e.ModelElement as UniquenessConstraint, true);
				}
			}
		}
		#endregion // ImpliedObjectificationUniquenessConstraintIsInternalChangeRule class
		#region ImpliedObjectificationIsImpliedChangeRule class
		/// <summary>
		/// Validates that an objectification that is implied matches the implied objectification pattern.
		/// </summary>
		[RuleOn(typeof(Objectification))]
		private class ImpliedObjectificationIsImpliedChangeRule : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				if (e.MetaAttribute.Id == Objectification.IsImpliedMetaAttributeGuid && (bool)e.NewValue)
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
							throw new InvalidOperationException(ResourceStrings.ModelExceptionObjectificationImpliedMustBeImpliedAndIndependentAndCannotPlayRoleInNonImpliedFact);
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
		private class ImpliedObjectificationObjectifyingTypeIsIndependentChangeRule : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				if (e.MetaAttribute.Id == ObjectType.IsIndependentMetaAttributeGuid && !(bool)e.NewValue)
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
		private class ImpliedObjectificationObjectifyingTypePlaysRoleAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ProcessNewPlayerRoleForImpliedObjectification((e.ModelElement as ObjectTypePlaysRole).PlayedRoleCollection);
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
		private class ObjectificationAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				Objectification objectificationLink = e.ModelElement as Objectification;
				FactType nestedFact = objectificationLink.NestedFactType;
				if (nestedFact.ImpliedByObjectification != null)
				{
					throw new InvalidOperationException(ResourceStrings.ModelExceptionObjectificationImpliedFactObjectified);
				}
				Store store = nestedFact.Store;
				ObjectType nestingType = objectificationLink.NestingType;
				ORMModel model = nestedFact.Model;

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
				RoleBaseMoveableCollection roles = nestedFact.RoleCollection;
				int roleCount = roles.Count;
				if (roleCount != 0)
				{
					for (int i = 0; i < roleCount; ++i)
					{
						Role role = roles[i].Role;
						if (role.Proxy == null)
						{
							CreateImpliedFactTypeForRole(model, nestingType, role, objectificationLink);
						}
					}
				}
			}
		}
		#endregion // ObjectificationAddRule class
		#region ObjectificationRemoveRule class
		/// <summary>
		/// Remove the implied objectifying ObjectType when Objectification is removed.
		/// </summary>
		[RuleOn(typeof(Objectification))]
		private class ObjectificationRemoveRule : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				Objectification objectification = e.ModelElement as Objectification;
				if (objectification.IsImplied)
				{
					ObjectType nestingType = objectification.NestingType;
					if (!nestingType.IsRemoved)
					{
						nestingType.Remove();
					}
				}
				else
				{
					FactType nestedFact = objectification.NestedFactType;
					if (!nestedFact.IsRemoved)
					{
						ORMMetaModel.DelayValidateElement(nestedFact, DelayProcessFactTypeForImpliedObjectification);
					}
				}
			}
		}
		#endregion // ObjectificationRemoveRule class
		#region ImpliedFactTypeAddRule class
		/// <summary>
		/// Rule class to block objectification of implied facts
		/// </summary>
		[RuleOn(typeof(ObjectificationImpliesFactType))]
		private class ImpliedFactTypeAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ObjectificationImpliesFactType link = e.ModelElement as ObjectificationImpliesFactType;
				if (link.ImpliedFactTypeCollection.Objectification != null)
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
		private class RoleAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelElement element = e.ModelElement;
				ConstraintRoleSequenceHasRole sequenceRoleLink;
				FactSetConstraint internalConstraintLink;
				FactType fact;
				Objectification objectificationLink;
				bool disallowed = false;
				if (null != (sequenceRoleLink = element as ConstraintRoleSequenceHasRole))
				{
					ConstraintRoleSequence modifiedSequence = sequenceRoleLink.ConstraintRoleSequenceCollection;
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
								FactTypeMoveableCollection facts = (constraint as SetConstraint).FactTypeCollection;
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
					if (internalConstraintLink.SetConstraintCollection.Constraint.ConstraintIsInternal)
					{
						disallowed = null != internalConstraintLink.FactTypeCollection.ImpliedByObjectification;
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
						Role nestedRole = factRoleLink.RoleCollection.Role;

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
					ThrowBlockedByObjectificationPatternException();
				}
			}
		}
		#endregion // RoleAddRule class
		#region RoleRemovingRule class
		/// <summary>
		/// Synchronize implied fact types when a role is removed from
		/// the nested type.
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole)), RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		private class RoleRemovingRule : RemovingRule
		{
			private bool myAllowModification;
			public override void ElementRemoving(ElementRemovingEventArgs e)
			{
				ModelElement element = e.ModelElement;
				ConstraintRoleSequenceHasRole sequenceRoleLink;
				FactType fact;
				Objectification objectificationLink;
				bool disallowed = false;
				if (null != (sequenceRoleLink = element as ConstraintRoleSequenceHasRole))
				{
					if (!sequenceRoleLink.RoleCollection.IsRemoving)
					{
						ConstraintRoleSequence sequence = sequenceRoleLink.ConstraintRoleSequenceCollection;
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
									FactTypeMoveableCollection facts = (constraint as SetConstraint).FactTypeCollection;
									if (facts.Count == 1)
									{
										fact = facts[0];
										if (null != (objectificationLink = fact.ImpliedByObjectification))
										{
											disallowed = !myAllowModification && !objectificationLink.IsRemoving;
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
						if (!myAllowModification && !objectificationLink.IsRemoving)
						{
							disallowed = true;
							RoleProxy proxy;
							Role proxyRole;
							if (null != (proxy = factRoleLink.RoleCollection as RoleProxy) &&
								null != (proxyRole = proxy.Role))
							{
								disallowed = !proxyRole.IsRemoving;
							}
						}
					}
					else if (null != (objectificationLink = fact.Objectification))
					{
						if (!objectificationLink.IsRemoving)
						{
							try
							{
								myAllowModification = true;
								Role nestedRole = factRoleLink.RoleCollection.Role;
								RoleProxy proxyRole = nestedRole.Proxy;
								Debug.Assert(proxyRole != null, "Proxy is not attached. Safe to ignore, but code path unexpected.");
								if (proxyRole != null)
								{
									proxyRole.FactType.Remove();
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
					ThrowBlockedByObjectificationPatternException();
				}
			}
		}
		#endregion // RoleRemovingRule class
		#region RolePlayerAddRule class
		/// <summary>
		/// Synchronize implied fact types when a role player is
		/// set on an objectified role
		/// </summary>
		[RuleOn(typeof(ObjectTypePlaysRole))]
		private class RolePlayerAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				Role role = link.PlayedRoleCollection;
				FactType fact = role.FactType;
				if (fact != null)
				{
					if (null != fact.ImpliedByObjectification)
					{
						ThrowBlockedByObjectificationPatternException();
					}
				}
			}
		}
		#endregion // RolePlayerAddRule class
		#region RolePlayerRemovingRule class
		/// <summary>
		/// Synchronize implied fact types when a role player is
		/// being removed from an objectified role
		/// </summary>
		[RuleOn(typeof(ObjectTypePlaysRole))]
		private class RolePlayerRemovingRule : RemovingRule
		{
			public override void ElementRemoving(ElementRemovingEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				Role role = link.PlayedRoleCollection;
				ObjectType rolePlayer = link.RolePlayer;
				FactType fact = role.FactType;
				// Note if the roleplayer is removed, then the links all go away
				// automatically. There is no additional work to do or checks to make.
				if (!(rolePlayer.IsRemoved || rolePlayer .IsRemoving) &&
					(null != (fact = role.FactType)))
				{
					Objectification objectificationLink;
					if (null != (objectificationLink = fact.ImpliedByObjectification))
					{
						if (!(objectificationLink.IsRemoving || role.IsRemoving))
						{
							ThrowBlockedByObjectificationPatternException();
						}
					}
				}
			}
		}
		#endregion // RolePlayerRemovingRule class
		#region InternalConstraintChangeRule class
		/// <summary>
		/// Ensure that implied internal constraints cannot change the Modality property
		/// </summary>
		[RuleOn(typeof(SetConstraint))]
		private class InternalConstraintChangeRule : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeId = e.MetaAttribute.Id;
				if (attributeId == SetConstraint.ModalityMetaAttributeGuid)
				{
					SetConstraint constraint = e.ModelElement as SetConstraint;
					if (constraint.Constraint.ConstraintIsInternal)
					{
						FactTypeMoveableCollection facts;
						if (1 == (facts = constraint.FactTypeCollection).Count &&
							facts[0].ImpliedByObjectification != null)
						{
							ThrowBlockedByObjectificationPatternException();
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
		private static void ProcessNewPlayerRoleForImpliedObjectification(Role playedRole)
		{
			if (playedRole != null)
			{
				FactType playedFact = playedRole.FactType;
				if (playedFact != null)
				{
					// If the fact is implied, we don't need to do anything else
					if (playedFact.ImpliedByObjectification != null)
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
			FactTypeMoveableCollection facts = uniquenessConstraint.FactTypeCollection;
			int factsCount = facts.Count;
			for (int i = 0; i < factsCount; ++i)
			{
				ORMMetaModel.DelayValidateElement(facts[i], DelayProcessFactTypeForImpliedObjectification);
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
			if (factType == null || factType.IsRemoved || factType.ImpliedByObjectification != null)
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
						throw new InvalidOperationException(ResourceStrings.ModelExceptionObjectificationImpliedMustBeImpliedAndIndependentAndCannotPlayRoleInNonImpliedFact);
					}
					else
					{
						objectification.Remove();
					}
				}
			}
			else
			{
				// See if we have more than two roles
				if (factType.RoleCollection.Count > 2)
				{
					CreateImpliedObjectificationForFactType(factType, null);
					return;
				}
				// See if we now have a uniqueness constraint that implies an objectification
				foreach (UniquenessConstraint uniquenessConstraint in factType.GetInternalConstraints<UniquenessConstraint>())
				{
					if (uniquenessConstraint.RoleCollection.Count > 1)
					{
						// We now have a uniqueness constraint that implies an objectification, so create it
						CreateImpliedObjectificationForFactType(factType, null);
						return;
					}
				}
			}
		}
		/// <summary>
		/// Creates an implied Objectification for the specified FactType.
		/// NOTE: It is the caller's responsibility to ensure the the specified FactType matches the implied Objectification pattern.
		/// </summary>
		private static void CreateImpliedObjectificationForFactType(FactType factType, INotifyElementAdded notifyAdded)
		{
			Store store = factType.Store;
			ObjectType objectifiedType = ObjectType.CreateAndInitializeObjectType(store,
				new AttributeAssignment[]
				{
					new AttributeAssignment(ObjectType.NameMetaAttributeGuid, factType.Name, store),
					new AttributeAssignment(ObjectType.IsIndependentMetaAttributeGuid, true, store),
				});
			Objectification.CreateAndInitializeObjectification(store,
				new RoleAssignment[]
				{
					new RoleAssignment(Objectification.NestedFactTypeMetaRoleGuid, factType),
					new RoleAssignment(Objectification.NestingTypeMetaRoleGuid, objectifiedType)
				},
				new AttributeAssignment[]
				{
					new AttributeAssignment(Objectification.IsImpliedMetaAttributeGuid, true, store)
				});
			if (notifyAdded == null)
			{
				IDictionary contextInfo = store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
				try
				{
					contextInfo[ObjectType.AllowDuplicateObjectNamesKey] = null;
					objectifiedType.Model = factType.Model;
				}
				finally
				{
					contextInfo.Remove(ObjectType.AllowDuplicateObjectNamesKey);
				}
			}
			else
			{
				objectifiedType.Model = factType.Model;
				// The true addLinks parameter here will pick up both the Objectification and
				// the ModelHasObjectType links, so is sufficient to get all of the elements we
				// created here.
				notifyAdded.ElementAdded(objectifiedType, true);
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
			FactType impliedFact = FactType.CreateFactType(store);
			RoleProxy nearRole = RoleProxy.CreateRoleProxy(store);
			nearRole.TargetRole = nestedRole;
			Role farRole = Role.CreateRole(store);
			RoleBaseMoveableCollection impliedRoles = impliedFact.RoleCollection;
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
			ReadingOrderMoveableCollection readingOrders = impliedFact.ReadingOrderCollection;
			ReadingOrder order = ReadingOrder.CreateReadingOrder(store);
			RoleBaseMoveableCollection orderRoles;
			readingOrders.Add(order);
			orderRoles = order.RoleCollection;
			orderRoles.Add(nearRole);
			orderRoles.Add(farRole);
			Reading reading = Reading.CreateReading(store);
			reading.ReadingOrder = order;
			reading.Text = ResourceStrings.ImpliedFactTypePredicateReading;

			// Add inverse reading
			order = ReadingOrder.CreateReadingOrder(store);
			readingOrders.Add(order);
			orderRoles = order.RoleCollection;
			orderRoles.Add(farRole);
			orderRoles.Add(nearRole);
			reading = Reading.CreateReading(store);
			reading.ReadingOrder = order;
			reading.Text = ResourceStrings.ImpliedFactTypePredicateInverseReading;

			// Attach the objectification to the fact
			impliedFact.ImpliedByObjectification = objectification;

			// Attach the fact to the model
			impliedFact.Model = model;
			return impliedFact;
		}
		/// <summary>
		/// Throw an exception indicating that the current modification is
		/// not allowed by the objectification patterh
		/// </summary>
		private static void ThrowBlockedByObjectificationPatternException()
		{
			throw new InvalidOperationException(ResourceStrings.ModelExceptionObjectificationImpliedElementModified);
		}
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
		private class ObjectificationFixupListener : DeserializationFixupListener<Objectification>
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
			protected override void ProcessElement(Objectification element, Store store, INotifyElementAdded notifyAdded)
			{
				// Note that this assumes xsd validation has occurred (RoleProxy is only on an ImpliedFact, there
				// is 1 Role and 1 RoleProxy, and implied facts must be attached to an Objectification relationship.
				FactType nestedFact = element.NestedFactType;
				ORMModel model = nestedFact.Model;
				ObjectType nestingType = element.NestingType;
				RoleBaseMoveableCollection factRoles = nestedFact.RoleCollection;
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
						if (impliedFact != null && !object.ReferenceEquals(impliedFact.ImpliedByObjectification, element))
						{
							RemoveFact(impliedFact);
							impliedFact = null;
							Debug.Assert(proxy.IsRemoved); // Goes away with delete propagation on the fact
							proxy = null;
						}
						else
						{
							RoleBaseMoveableCollection impliedRoles = impliedFact.RoleCollection;
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
									if (!object.ReferenceEquals(testRolePlayer, nestingType))
									{
										farRole.RolePlayer = nestingType;
										notifyAdded.ElementAdded((ModelElement)farRole.GetElementLinks(ObjectTypePlaysRole.RolePlayerMetaRoleGuid)[0]);
									}
								}
							}
						}
					}
					if (proxy == null)
					{
						// Create the proxy role
						proxy = RoleProxy.CreateRoleProxy(store);
						proxy.TargetRole = factRole;
						notifyAdded.ElementAdded(proxy, true);

						// Create the non-proxy role
						farRole = Role.CreateRole(store);
						farRole.RolePlayer = nestingType;
						notifyAdded.ElementAdded(proxy, true);

						// Create the implied fact and set relationships to existing objects
						impliedFact = FactType.CreateFactType(store);
						proxy.FactType = impliedFact;
						farRole.FactType = impliedFact;
						impliedFact.ImpliedByObjectification = element;
						impliedFact.Model = model;
						notifyAdded.ElementAdded(impliedFact, true);

						// Add forward reading
						ReadingOrderMoveableCollection readingOrders = impliedFact.ReadingOrderCollection;
						ReadingOrder order = ReadingOrder.CreateReadingOrder(store);
						RoleBaseMoveableCollection orderRoles;
						readingOrders.Add(order);
						orderRoles = order.RoleCollection;
						orderRoles.Add(proxy);
						orderRoles.Add(farRole);
						Reading reading = Reading.CreateReading(store);
						reading.ReadingOrder = order;
						reading.Text = ResourceStrings.ImpliedFactTypePredicateReading;
						notifyAdded.ElementAdded(order, true);
						notifyAdded.ElementAdded(reading, false);

						// Add inverse reading
						order = ReadingOrder.CreateReadingOrder(store);
						readingOrders.Add(order);
						orderRoles = order.RoleCollection;
						orderRoles.Add(farRole);
						orderRoles.Add(proxy);
						reading = Reading.CreateReading(store);
						reading.ReadingOrder = order;
						reading.Text = ResourceStrings.ImpliedFactTypePredicateInverseReading;
						notifyAdded.ElementAdded(order, true);
						notifyAdded.ElementAdded(reading, false);
					}

					// Make sure the internal constraint pattern is correct on the far role
					EnsureSingleColumnUniqueAndMandatory(store, model, farRole, notifyAdded);
				}

				// Verify that that are no innapropriate implied facts are attached to the objectification
				FactTypeMoveableCollection impliedFacts = element.ImpliedFactTypeCollection;
				int impliedFactCount = impliedFacts.Count;
				if (impliedFactCount != factRolesCount)
				{
					int leftToRemove = impliedFactCount - factRolesCount;
					Debug.Assert(impliedFactCount > factRolesCount); // We verified and/or added at least this many earlier
					for (int i = impliedFactCount - 1; i >= 0 && leftToRemove != 0; --i)
					{
						FactType impliedFact = impliedFacts[i];
						RoleBaseMoveableCollection impliedRoles = impliedFact.RoleCollection;
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
							}
							Role targetRole;
							if (proxy == null ||
								null == (targetRole = proxy.Role) ||
								!object.ReferenceEquals(targetRole, nestedFact))
							{
								RemoveFact(impliedFact);
								--leftToRemove;
							}
						}
					}
				}

				// Verify the implication pattern
				if (element.IsImplied && (!nestingType.IsIndependent || nestingType.PlayedRoleCollection.Count > factRolesCount))
				{
					element.IsImplied = false;
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
				RoleBaseMoveableCollection factRoles = fact.RoleCollection;
				int roleCount = factRoles.Count;
				for (int i = 0; i < roleCount; ++i)
				{
					Role role = factRoles[i].Role;
					ConstraintRoleSequenceMoveableCollection sequences = role.ConstraintRoleSequenceCollection;
					int sequenceCount = sequences.Count;
					for (int j = sequenceCount - 1; j >= 0; --j)
					{
						SetConstraint ic = sequences[j] as SetConstraint;
						if (ic != null && ic.Constraint.ConstraintIsInternal)
						{
							ic.Remove();
						}
					}
				}
				fact.Remove();
			}
			private static void EnsureSingleColumnUniqueAndMandatory(Store store, ORMModel model, Role role, INotifyElementAdded notifyAdded)
			{
				ConstraintRoleSequenceMoveableCollection sequences = role.ConstraintRoleSequenceCollection;
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
										ic.Remove();
									}
									else
									{
										haveUniqueness = true;
									}
									break;
								case ConstraintType.SimpleMandatory:
									if (haveMandatory)
									{
										ic.Remove();
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
							ic.Remove();
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
		private class ImpliedObjectificationFixupListener : DeserializationFixupListener<FactType>
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
			protected override void ProcessElement(FactType element, Store store, INotifyElementAdded notifyAdded)
			{
				if (!element.IsRemoved &&
					null == element.Objectification &&
					null == element.ImpliedByObjectification)
				{
					bool impliedRequired = false;
					if (element.RoleCollection.Count > 2)
					{
						impliedRequired = true;
					}
					else
					{
						foreach (UniquenessConstraint constraint in element.GetInternalConstraints<UniquenessConstraint>())
						{
							impliedRequired = true;
						}
					}
					if (impliedRequired)
					{
						CreateImpliedObjectificationForFactType(element, notifyAdded);
					}
				}
			}
		}
		#endregion Deserialization Fixup
	}
}
