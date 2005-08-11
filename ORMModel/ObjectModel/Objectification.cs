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

				// Create an equality constraint with two role sequences
				EqualityConstraint equalityConstraint = EqualityConstraint.CreateEqualityConstraint(store);
				// Defer adding the constraint to the model and the sequences to the constraint to hold off error checking on the constraint until the end
				MultiColumnExternalConstraintRoleSequence nearSequence = MultiColumnExternalConstraintRoleSequence.CreateMultiColumnExternalConstraintRoleSequence(store);
				RoleMoveableCollection nearSequenceRoles = nearSequence.RoleCollection;
				MultiColumnExternalConstraintRoleSequence farSequence = MultiColumnExternalConstraintRoleSequence.CreateMultiColumnExternalConstraintRoleSequence(store);
				RoleMoveableCollection farSequenceRoles = farSequence.RoleCollection;
				
				// Add implied fact types, one for each role
				RoleMoveableCollection roles = nestedFact.RoleCollection;
				int roleCount = roles.Count;
				if (roleCount != 0)
				{
					bool generateNearConstraints = 0 != nestedFact.GetInternalConstraintsCount(ConstraintType.InternalUniqueness);
					Role[] nearImpliedRoles = null;
					FactType[] impliedFacts = null;
					if (generateNearConstraints)
					{
						nearImpliedRoles = new Role[roleCount];
						impliedFacts = new FactType[roleCount];
					}
					for (int i = 0; i < roleCount; ++i)
					{
						Role nestedRole = roles[i];

						// Create the implied fact and attach roles to it
						FactType impliedFact = FactType.CreateFactType(store);
						Role nearRole = Role.CreateRole(store);
						Role farRole = Role.CreateRole(store);
						RoleMoveableCollection impliedRoles = impliedFact.RoleCollection;
						impliedRoles.Add(nearRole);
						impliedRoles.Add(farRole);

						// Add standard constraints and role players
						nearRole.Multiplicity = RoleMultiplicity.ExactlyOne; // Adds uniqueness and mandatory constraints to far role
						farRole.RolePlayer = nestingType;
						ObjectType nearRolePlayer = nestedRole.RolePlayer;
						if (nearRolePlayer != null)
						{
							nearRole.RolePlayer = nearRolePlayer;
						}

						// UNDONE: Each of the readings need to be modified if we're in
						// a ring situation. The replacement values needs to be the (1-based)
						// number of the occurrence of that role player type in the collection.
						// Add forward reading
						ReadingOrderMoveableCollection readingOrders = impliedFact.ReadingOrderCollection;
						ReadingOrder order = ReadingOrder.CreateReadingOrder(store);
						RoleMoveableCollection orderRoles;
						readingOrders.Add(order);
						orderRoles = order.RoleCollection;
						orderRoles.Add(nearRole);
						orderRoles.Add(farRole);
						Reading reading = Reading.CreateReading(store);
						reading.ReadingOrder = order;
						reading.Text = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ImpliedFactTypePredicateReading, "");

						// Add inverse reading
						order = ReadingOrder.CreateReadingOrder(store);
						readingOrders.Add(order);
						orderRoles = order.RoleCollection;
						orderRoles.Add(farRole);
						orderRoles.Add(nearRole);
						reading = Reading.CreateReading(store);
						reading.ReadingOrder = order;
						reading.Text = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ImpliedFactTypePredicateInverseReading, "");

						// Attach the roles to equality constraint
						nearSequenceRoles.Add(nestedRole);
						farSequenceRoles.Add(nearRole);
						impliedFact.Model = model;
						if (generateNearConstraints)
						{
							// Postpone setting the implied objectification until all
							// constraints are added
							nearImpliedRoles[i] = nearRole;
							impliedFacts[i] = impliedFact;
						}
						else
						{
							impliedFact.ImpliedByObjectification = objectificationLink;
						}
					}
					if (generateNearConstraints)
					{
						foreach (InternalConstraint iuc in nestedFact.GetInternalConstraints(ConstraintType.InternalUniqueness))
						{
							RoleMoveableCollection constraintRoles = iuc.RoleCollection;
							int constraintRoleCount = constraintRoles.Count;
							if (constraintRoleCount == 1)
							{
								// Adding a role will automatically bind the constraint to the fact
								InternalUniquenessConstraint.CreateInternalUniquenessConstraint(store).RoleCollection.Add(nearImpliedRoles[roles.IndexOf(constraintRoles[0])]);
							}
							else if (constraintRoleCount != 0)
							{
								ExternalUniquenessConstraint euc = ExternalUniquenessConstraint.CreateExternalUniquenessConstraint(store);
								RoleMoveableCollection eucRoles = euc.RoleCollection;
								for (int i = 0; i < constraintRoleCount; ++i)
								{
									eucRoles.Add(nearImpliedRoles[roles.IndexOf(constraintRoles[i])]);
								}
								euc.Model = model;
								euc.ImpliedByObjectification = objectificationLink;
							}
						}
						for (int i = 0; i < roleCount; ++i)
						{
							impliedFacts[i].ImpliedByObjectification = objectificationLink;
						}
					}
				}

				// Hook up equality constraint
				nearSequence.ExternalConstraint = equalityConstraint;
				farSequence.ExternalConstraint = equalityConstraint;
				equalityConstraint.ImpliedByObjectification = objectificationLink;
				equalityConstraint.Model = model;
			}
		}
		#endregion // Objectification implied facts and constraints pattern enforcement
	}
}
