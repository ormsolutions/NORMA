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
					int internalConstraintsCount = nestedFact.GetInternalConstraintsCount(ConstraintType.InternalUniqueness);
					bool generateNearConstraints = 0 != internalConstraintsCount;
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
						Role nearRole;
						FactType impliedFact = CreateImpliedFactTypeForRole(model, nestingType, roles[i], out nearRole);

						// Attach the roles to equality constraint
						nearSequenceRoles.Add(nestedRole);
						farSequenceRoles.Add(nearRole);

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
						// Attach implied uniqueness constraints before officially objectifying
						// the implied facts. Note that when later rules modify these constraints
						// they pattern match on the existing implied fact types. We want to get
						// the same pattern upfront and with edits. Therefore, if there
						// are any duplicates in the set of internal uniqueness constraints, then
						// we need to match the pattern here by matching the pattern on earlier constraints.
						// Of course, these are error conditions on the objectified fact, but that doesn't mean
						// we need to propagate all of the errors.
						InternalUniquenessConstraint[] iucs = new InternalUniquenessConstraint[internalConstraintsCount];
						int iCurrentIuc = 0;
						foreach (InternalUniquenessConstraint iuc in nestedFact.GetInternalConstraints<InternalUniquenessConstraint>())
						{
							iucs[iCurrentIuc] = iuc;
							++iCurrentIuc;
						}
						for (iCurrentIuc = 0; iCurrentIuc < internalConstraintsCount; ++iCurrentIuc)
						{
							InternalConstraint iuc = iucs[iCurrentIuc];
							RoleMoveableCollection constraintRoles = iuc.RoleCollection;
							int constraintRoleCount = constraintRoles.Count;
							bool skip = false;

							// See if this is equivalent to a previous constraint
							for (int iTestIuc = 0; iTestIuc < iCurrentIuc; ++iTestIuc)
							{
								RoleMoveableCollection testRoles = iucs[iTestIuc].RoleCollection;
								if (testRoles.Count == constraintRoleCount)
								{
									int iTestRole;
									for (iTestRole = 0; iTestRole < constraintRoleCount; ++iTestRole)
									{
										if (!constraintRoles.Contains(testRoles[iTestRole]))
										{
											break;
										}
									}
									if (iTestRole == constraintRoleCount)
									{
										skip = true;
										break;
									}
								}
							}
							if (skip)
							{
								continue;
							}

							if (constraintRoleCount == 1)
							{
								// Adding a role will automatically bind the constraint to the fact
								// so we don't need to set the InternalUniquenessConstraint.FactType
								// property here.
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

						// Now objectify the implied facts, which enables all of the validation
						// rules we've been ignoring up to this point
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
		[RuleOn(typeof(FactTypeHasRole)), RuleOn(typeof(ConstraintRoleSequenceHasRole)), RuleOn(typeof(FactTypeHasInternalConstraint))]
		private class RoleAddRule : AddRule
		{
			private bool myAllowModification;
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelElement element = e.ModelElement;
				ConstraintRoleSequenceHasRole sequenceRoleLink;
				FactTypeHasInternalConstraint internalConstraintLink;
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
								fact = (constraint as InternalConstraint).FactType;
								if (fact != null)
								{
									if (null != fact.ImpliedByObjectification)
									{
										disallowed = true; // We don't trigger adds when this rule is active
									}
									else if (constraintType == ConstraintType.InternalUniqueness &&
											 null != (objectificationLink = fact.Objectification))
									{
										// We need to remove any internal or external
										// constraint associated with the near role on
										// the implied fact type associated with all other roles
										// in this constraint sequence. The implied sequences
										// are rebuilt when the local transaction commits, but
										// all removes happen at this stage.
										RoleMoveableCollection modifiedRoles = modifiedSequence.RoleCollection;
										int sequenceRoleCount = modifiedRoles.Count;
										if (sequenceRoleCount > 1)
										{
											Role modifiedRole = sequenceRoleLink.RoleCollection;
											FactTypeMoveableCollection impliedFacts = objectificationLink.ImpliedFactTypeCollection;
											RoleMoveableCollection factRoles = fact.RoleCollection;
											RuleManager ruleManager = fact.Store.RuleManager;
											try
											{
												// Temporarily disable removing rule so we can remove implied constraints
												ruleManager.DisableRule(typeof(RoleRemovingRule));
												for (int iSequenceRole = 0; iSequenceRole < sequenceRoleCount; ++iSequenceRole)
												{
													Role sequenceRole = modifiedRoles[iSequenceRole];
													if (object.ReferenceEquals(sequenceRole, modifiedRole))
													{
														continue;
													}
													FactType impliedFact = impliedFacts[factRoles.IndexOf(sequenceRole)];
													ConstraintRoleSequenceMoveableCollection impliedSequences = impliedFact.RoleCollection[0].ConstraintRoleSequenceCollection;
													int sequencesCount = impliedSequences.Count;
													for (int i = sequencesCount - 1; i >= 0; --i) // Go backwards so we can remove
													{
														ConstraintRoleSequence impliedSequence = impliedSequences[i];
														if (!impliedSequence.IsRemoving)
														{
															IConstraint impliedConstraint = impliedSequence.Constraint;
															if (impliedConstraint != null)
															{
																switch (impliedConstraint.ConstraintType)
																{
																	case ConstraintType.InternalUniqueness:
																	case ConstraintType.ExternalUniqueness:
																		// For both of the constraint types we're looking for,
																		// the sequence is the constraint, so we can remove it directly
																		Debug.Assert(object.ReferenceEquals(impliedConstraint, impliedSequence));
																		impliedSequence.Remove();
																		break;
																}
															}
														}
													}
												}
											}
											finally
											{
												ruleManager.EnableRule(typeof(RoleRemovingRule));
											}
										}
									}
								}
								break;
							case ConstraintType.ExternalUniqueness:
								// These constraints are always modified/created without the
								// objectification link in place, so we never trigger this
								// action from any of our rules.
								disallowed = null != (constraint as ExternalUniquenessConstraint).ImpliedByObjectification;
								break;
							case ConstraintType.Equality:
								if (!myAllowModification)
								{
									disallowed = null != (constraint as EqualityConstraint).ImpliedByObjectification;
								}
								break;
						}
					}
				}
				else if (null != (internalConstraintLink = element as FactTypeHasInternalConstraint))
				{
					disallowed = null != internalConstraintLink.FactType.ImpliedByObjectification;
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
						EqualityConstraint impliedEquality = objectificationLink.ImpliedEqualityConstraint;
						// If the implied equality constraint is not established yet, then
						// we're in the process of the initial construction of the implied elements.
						if (impliedEquality != null)
						{
							ObjectType nestingType = objectificationLink.NestingType;
							Role nestedRole = factRoleLink.RoleCollection;
							int roleIndex = fact.RoleCollection.IndexOf(nestedRole);

							// Create and populate new fact type
							Role nearRole;
							FactType impliedFact = CreateImpliedFactTypeForRole(nestingType.Model, nestingType, nestedRole, out nearRole);
							// Setting impliedFact.ImpliedByObjectification will always add these to the end
							// of the ImpliedFactTypeCollection, but we need them order, so explicitly insert.
							objectificationLink.ImpliedFactTypeCollection.Insert(roleIndex, impliedFact);

#if DEBUG
							// Note that at this point, with an inline add on the role link, there
							// is no way for the role to already be associated with an internal
							// constraint, so we don't do any work here to synchronize the implied
							// uniqueness constraints.
							foreach (ConstraintRoleSequence testSequence in nestedRole.ConstraintRoleSequenceCollection)
							{
								IConstraint testConstraint = testSequence.Constraint;
								if (testConstraint != null && testConstraint.ConstraintStorageStyle == ConstraintStorageStyle.InternalConstraint)
								{
									Debug.Fail("Unexpected association with an internal constraint");
								}
							}
#endif // DEBUG

							MultiColumnExternalConstraintRoleSequenceMoveableCollection equalitySequences = impliedEquality.RoleSequenceCollection;
							try
							{
								myAllowModification = true;
								equalitySequences[0].RoleCollection.Insert(roleIndex, nestedRole);
								equalitySequences[1].RoleCollection.Insert(roleIndex, nearRole);
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
									fact = (constraint as InternalConstraint).FactType;
									if (fact != null)
									{
										if (null != (objectificationLink = fact.ImpliedByObjectification))
										{
											disallowed = !myAllowModification && !objectificationLink.IsRemoving;
										}
										else if (constraintType == ConstraintType.InternalUniqueness &&
												 null != (objectificationLink = fact.Objectification) &&
												 !objectificationLink.IsRemoving)
										{
											// We need to remove any internal or external
											// constraint associated with the near role on
											// the implied fact type associated with all roles
											// in this constraint sequence.
											int roleIndex = fact.RoleCollection.IndexOf(sequenceRoleLink.RoleCollection);
											FactType impliedFact = objectificationLink.ImpliedFactTypeCollection[roleIndex];
											try
											{
												myAllowModification = true;
												ConstraintRoleSequenceMoveableCollection impliedSequences = impliedFact.RoleCollection[0].ConstraintRoleSequenceCollection;
												int sequencesCount = impliedSequences.Count;
												for (int i = sequencesCount - 1; i >= 0; --i) // Go backwards so we can remove
												{
													ConstraintRoleSequence impliedSequence = impliedSequences[i];
													if (!impliedSequence.IsRemoving)
													{
														IConstraint impliedConstraint = impliedSequence.Constraint;
														if (impliedConstraint != null)
														{
															switch (impliedConstraint.ConstraintType)
															{
																case ConstraintType.InternalUniqueness:
																case ConstraintType.ExternalUniqueness:
																	// For both of the constraint types we're looking for,
																	// the sequence is the constraint, so we can remove it directly
																	Debug.Assert(object.ReferenceEquals(impliedConstraint, impliedSequence));
																	impliedSequence.Remove();
																	break;
															}
														}
													}
												}
											}
											finally
											{
												myAllowModification = false;
											}
										}
									}
									break;
								case ConstraintType.ExternalUniqueness:
									disallowed = !myAllowModification && null != (objectificationLink = (constraint as ExternalUniquenessConstraint).ImpliedByObjectification) && !objectificationLink.IsRemoving;
									break;
								case ConstraintType.Equality:
									disallowed = null != (objectificationLink = (constraint as EqualityConstraint).ImpliedByObjectification) && !objectificationLink.IsRemoving;
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
							Role nestedRole = factRoleLink.RoleCollection;
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
							objectificationLink.ImpliedFactTypeCollection[roleIndex].RoleCollection[0].RolePlayer = link.RolePlayer;
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
				FactType fact = role.FactType;
				if (fact != null)
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
								objectificationLink.ImpliedFactTypeCollection[roleIndex].RoleCollection[0].RolePlayer = null;
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
		#region UpdateImplicitUniquenessAddRule class
		/// <summary>
		/// AddRule class to match or add implicit uniqueness constraints
		/// when an edit to an internal constraint on an objectified
		/// fact is committed. Note that all removes happen with inline
		/// rules, this is used solely for adding new constraints.
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole), FireTime=TimeToFire.LocalCommit)]
		private class UpdateImplicitUniquenessAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				BuildImpliedConstraintsForRule(e.ModelElement as ConstraintRoleSequenceHasRole);
			}
		}
		#endregion // UpdateImplicitUniquenessAddRule class
		#region UpdateImplicitUniquenessRemoveRule class
		/// <summary>
		/// RemoveRule class to match or add implicit uniqueness constraints
		/// when an edit to an internal constraint on an objectified
		/// fact is committed. Note that all removes happen with inline
		/// rules, this is used solely for adding new constraints.
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole), FireTime = TimeToFire.LocalCommit)]
		private class UpdateImplicitUniquenessRemoveRule : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				BuildImpliedConstraintsForRule(e.ModelElement as ConstraintRoleSequenceHasRole);
			}
		}
		#endregion // UpdateImplicitUniquenessAddRule class
		#region Helper functions
		private static void BuildImpliedConstraintsForRule(ConstraintRoleSequenceHasRole roleSequenceLink)
		{
			InternalUniquenessConstraint iuc = roleSequenceLink.ConstraintRoleSequenceCollection as InternalUniquenessConstraint;
			if (iuc != null)
			{
				FactType fact = null;
				Role sequenceRole = roleSequenceLink.RoleCollection;
				if (!iuc.IsRemoved)
				{
					fact = iuc.FactType;
				}
				if (fact == null && !sequenceRole.IsRemoved)
				{
					fact = sequenceRole.FactType;
				}
				Objectification objectificationLink;
				if (null != fact &&
					null != (objectificationLink = fact.Objectification))
				{
					foreach (InternalUniquenessConstraint factIuc in fact.GetInternalConstraints<InternalUniquenessConstraint>())
					{
						BuildImpliedConstraints(fact, objectificationLink, factIuc);
					}
				}
			}
		}
		/// <summary>
		/// Build implied constraints on implied fact types for the
		/// provided constraint. New constraints are added only if
		/// a matching constraint does not already exist.
		/// </summary>
		/// <param name="fact">The objectified fact</param>
		/// <param name="objectificationLink">The link to the objectification (equivalent to fact.Objectification)</param>
		/// <param name="basedOnConstraint">The InternalUniquenessConstraint that requires a mirroring implied constraint</param>
		private static void BuildImpliedConstraints(FactType fact, Objectification objectificationLink, InternalUniquenessConstraint basedOnConstraint)
		{
			FactTypeMoveableCollection impliedFacts = objectificationLink.ImpliedFactTypeCollection;
			RoleMoveableCollection factRoles = fact.RoleCollection;
			RoleMoveableCollection basedOnRoles = basedOnConstraint.RoleCollection;
			int basedOnRoleCount = basedOnRoles.Count;
			bool haveExistingConstraintMatch = false;
			if (basedOnRoleCount == 1)
			{
				Role nearImpliedRole = impliedFacts[factRoles.IndexOf(basedOnRoles[0])].RoleCollection[0];
				ConstraintRoleSequenceMoveableCollection existingImpliedConstraints = nearImpliedRole.ConstraintRoleSequenceCollection;
				int existingImpliedConstraintCount = existingImpliedConstraints.Count;
				for (int i = 0; i < existingImpliedConstraintCount; ++i)
				{
					InternalUniquenessConstraint impliedIuc = existingImpliedConstraints[i] as InternalUniquenessConstraint;
					Debug.Assert(impliedIuc == null || impliedIuc.RoleCollection.Count == 1); // These are all implied constraints on binary facts, internal constraints should always have one role only
					if (impliedIuc != null && impliedIuc.RoleCollection.Count == 1)
					{
						haveExistingConstraintMatch = true;
						break;
					}
				}
				if (!haveExistingConstraintMatch)
				{
					Store store = fact.Store;
					RuleManager ruleMgr = store.RuleManager;
					try
					{
						ruleMgr.DisableRule(typeof(RoleAddRule));
						InternalUniquenessConstraint.CreateInternalUniquenessConstraint(fact.Store).RoleCollection.Add(nearImpliedRole);
					}
					finally
					{
						ruleMgr.EnableRule(typeof(RoleAddRule));
					}
				}
			}
			else
			{
				ExternalUniquenessConstraintMoveableCollection impliedExternalUniquenessConstraints = objectificationLink.ImpliedExternalUniquenessConstraintCollection;
				int impliedExternalUniquenessConstraintsCount = impliedExternalUniquenessConstraints.Count;
				if (impliedExternalUniquenessConstraintsCount != 0)
				{
					for (int i = 0; i < impliedExternalUniquenessConstraintsCount; ++i)
					{
						ExternalUniquenessConstraint euc = impliedExternalUniquenessConstraints[i];

						// Match based on role index. The set of indices of the
						// facts associated with the implied constraints in the implied
						// facts collection must match the set of indices of the corresponding roles
						// in the base on constraint.
						FactTypeMoveableCollection eucFacts = euc.FactTypeCollection;
						if (eucFacts.Count == basedOnRoleCount)
						{
							int j = 0;
							for (j = 0; j < basedOnRoleCount; ++j)
							{
								if (!basedOnRoles.Contains(factRoles[impliedFacts.IndexOf(eucFacts[j])]))
								{
									break;
								}
							}
							if (j == basedOnRoleCount)
							{
								haveExistingConstraintMatch = true;
								break;
							}
						}
					}
				}
				if (!haveExistingConstraintMatch)
				{
					ExternalUniquenessConstraint euc = ExternalUniquenessConstraint.CreateExternalUniquenessConstraint(fact.Store);
					RoleMoveableCollection eucRoles = euc.RoleCollection;
					for (int i = 0; i < basedOnRoleCount; ++i)
					{
						eucRoles.Add(impliedFacts[factRoles.IndexOf(basedOnRoles[i])].RoleCollection[0]);
					}
					euc.Model = fact.Model;
					euc.ImpliedByObjectification = objectificationLink;
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
		/// <param name="nearRole">The near role of the newly generated fact</param>
		/// <returns>The created fact type</returns>
		private static FactType CreateImpliedFactTypeForRole(ORMModel model, ObjectType nestingType, Role nestedRole, out Role nearRole)
		{
			// Create the implied fact and attach roles to it
			Store store = model.Store;
			FactType impliedFact = FactType.CreateFactType(store);
			nearRole = Role.CreateRole(store);
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
			// Alternately, the readings we'll be able to generate are so ugly anyway
			// that the validation error with a direct jump to improve them will actually
			// be beneficial, not harmful, so we should not try to automatically repair
			// the readings at this point.

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
