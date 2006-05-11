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
		// UNDONE: Handle unary objectifications (both implied and explicit)
		#region ObjectificationNameChangeRule class
		/// <summary>
		/// Propagate name changes between FactType and ObjectType elements in an Objectification relationship.
		/// </summary>
		[RuleOn(typeof(ObjectType))]
		[RuleOn(typeof(FactType))]
		private class ObjectificationNameChangeRule : ChangeRule
		{
			private bool myCalledRecursively;
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				if (myCalledRecursively)
				{
					return;
				}
				if (e.MetaAttribute.Id == RootType.NameMetaAttributeGuid)
				{
					ModelElement modelElement = e.ModelElement;
					FactType factType;
					ObjectType objectType;
					if ((objectType = modelElement as ObjectType) != null)
					{
						if ((factType = objectType.NestedFactType) != null)
						{
							// The ObjectType name changed, so change the FactType name to match
							try
							{
								myCalledRecursively = true;
								factType.Name = (string)e.NewValue;
							}
							finally
							{
								myCalledRecursively = false;
							}
						}
					}
					else if ((factType = modelElement as FactType) != null)
					{
						if ((objectType = factType.NestingType) != null)
						{
							// The FactType name changed, so change the ObjectType name to match
							try
							{
								myCalledRecursively = true;
								objectType.Name = (string)e.NewValue;
							}
							finally
							{
								myCalledRecursively = false;
							}
						}
					}
				}
			}
		}
		#endregion // ObjectificationNameChangeRule class
		#region Implied Objectification creation and removal
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
		/// Creates a new implied Objectification if the implied objectification pattern is now present.
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole))]
		private class ImpliedObjectificationFactTypeHasRoleAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ORMMetaModel.DelayValidateElement((e.ModelElement as FactTypeHasRole).FactType, DelayProcessFactTypeForImpliedObjectification);
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
		#endregion // Implied Objectification creation and removal
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

				// Set the nested FactType name to the nesting ObjectType name
				nestedFact.Name = nestingType.Name;

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
			FactType factType = element as FactType;
			// We don't need to process implied FactTypes, since they can never be objectified
			if (factType == null || factType.ImpliedByObjectification != null || factType.IsRemoved)
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
					// We don't have a uniqueness constraint that implies the objectification, so remove it
					objectification.Remove();
				}
			}
			else
			{
				// See if we have more than two roles
				if (factType.RoleCollection.Count > 2)
				{
					CreateImpliedObjectificationForFactType(factType);
					return;
				}
				// See if we now have a uniqueness constraint that implies an objectification
				foreach (UniquenessConstraint uniquenessConstraint in factType.GetInternalConstraints<UniquenessConstraint>())
				{
					if (uniquenessConstraint.RoleCollection.Count > 1)
					{
						// We now have a uniqueness constraint that implies an objectification, so create it
						CreateImpliedObjectificationForFactType(factType);
						return;
					}
				}
			}
		}
		private static void CreateImpliedObjectificationForFactType(FactType factType)
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
			objectifiedType.Model = factType.Model;
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
		// UNDONE: Deserialization fixup
	}
}
