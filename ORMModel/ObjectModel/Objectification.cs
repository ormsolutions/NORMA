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

namespace Neumont.Tools.ORM.ObjectModel
{
	public partial class Objectification
	{
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
				ORMModel model = nestingType.Model;

				// Ensure we have a model so we can add the implied external constraints
				if (model == null)
				{
					model = nestedFact.Model;
					if (model == null)
					{
						throw new InvalidOperationException(ResourceStrings.ModelExceptionObjectificationRequiresModel);
					}
					nestingType.Model = model;
				}

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
						CreateImpliedFactTypeForRole(model, nestingType, roles[i].Role).ImpliedByObjectification = objectificationLink;
					}
				}
			}
		}
		#endregion // ObjectificationAddRule class
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
						disallowed = !(myAllowModification || objectificationLink.IsRemoving);
					}
					else if (null != (objectificationLink = fact.Objectification))
					{
						if (!objectificationLink.IsRemoving)
						{
							Role nestedRole = factRoleLink.RoleCollection.Role;
							int roleIndex = fact.RoleCollection.IndexOf(nestedRole);
							try
							{
								myAllowModification = true;
								objectificationLink.ImpliedFactTypeCollection.RemoveAt(roleIndex);
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
			private bool myAllowModification;
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				Role role = link.PlayedRoleCollection;
				FactType fact = role.FactType;
				if (fact != null)
				{
					Objectification objectificationLink;
					if (null != fact.ImpliedByObjectification)
					{
						if (!myAllowModification)
						{
							ThrowBlockedByObjectificationPatternException();
						}
					}
					else if (null != (objectificationLink = fact.Objectification))
					{
						int roleIndex = fact.RoleCollection.IndexOf(role);
						try
						{
							myAllowModification = true;
							// The role player on the near role of the implied fact must
							// match the role player of the nested role
							objectificationLink.ImpliedFactTypeCollection[roleIndex].RoleCollection[0].Role.RolePlayer = link.RolePlayer;
						}
						finally
						{
							myAllowModification = false;
						}
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
			private bool myAllowModification;
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
						if (!(myAllowModification || objectificationLink.IsRemoving || role.IsRemoving))
						{
							ThrowBlockedByObjectificationPatternException();
						}
					}
					else if (null != (objectificationLink = fact.Objectification))
					{
						if (!objectificationLink.IsRemoving)
						{
							int roleIndex = fact.RoleCollection.IndexOf(role);
							try
							{
								myAllowModification = true;
								// The role player on the near role of the implied fact must
								// match the role player of the nested role
								objectificationLink.ImpliedFactTypeCollection[roleIndex].RoleCollection[0].Role.RolePlayer = null;
							}
							finally
							{
								myAllowModification = false;
							}
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
		#region Helper functions
		/// <summary>
		/// Create an implied fact type, set its far constraints, and add
		/// it to the model. Associating the implied fact with the objectifying
		/// relationship is delayed and left to the caller to avoid triggering
		/// rules prematurely.
		/// </summary>
		/// <param name="model">The model to include the fact in</param>
		/// <param name="nestingType">The objectifying object type</param>
		/// <param name="nestedRole">The role associated with this element</param>
		/// <returns>The created fact type</returns>
		private static FactType CreateImpliedFactTypeForRole(ORMModel model, ObjectType nestingType, Role nestedRole)
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
		#endregion // Objectification implied facts and constraints pattern enforcement
		// UNDONE: Deserialization fixup
	}
}
