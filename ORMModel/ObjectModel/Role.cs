using System;
using System.Collections;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;

namespace Northface.Tools.ORM.ObjectModel
{
	#region RoleCardinality enum
	/// <summary>
	/// Define the cardinality for the roles. The role
	/// cardinality is currently displayed only on roles
	/// associated with binary fact types and is calculated
	/// based on the existing mandatory and internal uniqueness
	/// constraints associated with the fact.
	/// </summary>
	[CLSCompliant(true)]
	public enum RoleCardinality
	{
		/// <summary>
		/// Insufficient constraints are present to
		/// determine the user intention.
		/// </summary>
		Unspecified,
		/// <summary>
		/// Too many constraints are present to determine
		/// the user intention.
		/// </summary>
		Indeterminate,
		/// <summary>
		/// 0...1
		/// </summary>
		ZeroToOne,
		/// <summary>
		/// 0...n
		/// </summary>
		ZeroToMany,
		/// <summary>
		/// 1
		/// </summary>
		ExactlyOne,
		/// <summary>
		/// 1...n
		/// </summary>
		OneToMany,
	}
	#endregion // RoleCardinality enum
	public partial class Role
	{
		#region CustomStorage handlers
		/// <summary>
		/// Standard override. All custom storage properties are derived, not
		/// stored. Actual changes are handled in RoleChangeRule.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <param name="newValue">object</param>
		public override void SetValueForCustomStoredAttribute(MetaAttributeInfo attribute, object newValue)
		{
			Guid attributeGuid = attribute.Id;
			if (attributeGuid == RolePlayerDisplayMetaAttributeGuid ||
				attributeGuid == IsMandatoryMetaAttributeGuid ||
				attributeGuid == CardinalityMetaAttributeGuid)
			{
				// Handled by RoleChangeRule
				return;
			}
			base.SetValueForCustomStoredAttribute(attribute, newValue);
		}
		/// <summary>
		/// Standard override. Retrieve values for calculated properties.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <returns></returns>
		public override object GetValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			Guid attributeGuid = attribute.Id;
			if (attributeGuid == RolePlayerDisplayMetaAttributeGuid)
			{
				return RolePlayer;
			}
			else if (attributeGuid == IsMandatoryMetaAttributeGuid)
			{
				ConstraintRoleSetMoveableCollection constraintRoleSets = ConstraintRoleSetCollection;
				int roleSetCount = constraintRoleSets.Count;
				for (int i = 0; i < roleSetCount; ++i)
				{
					ConstraintRoleSet roleSet = constraintRoleSets[i];
					Constraint constraint = roleSet.Constraint;
					if (constraint.ConstraintType == ConstraintType.Mandatory)
					{
						return true;
					}
				}
				return false;
			}
			else if (attributeGuid == CardinalityMetaAttributeGuid)
			{
				RoleCardinality retVal = RoleCardinality.Unspecified;
				FactType fact = FactType;
				if (fact != null)
				{
					RoleMoveableCollection roles = fact.RoleCollection;
					if (roles.Count == 2)
					{
						bool haveMandatory = false;
						bool haveUniqueness = false;
						bool haveDoubleWideUniqueness = false;
						bool tooManyUniquenessConstraints = false;
						foreach (ConstraintRoleSet roleSet in ConstraintRoleSetCollection)
						{
							Constraint constraint = roleSet.Constraint;
							switch (constraint.ConstraintType)
							{
								case ConstraintType.Mandatory:
									// Ignore multiple mandatories. Unlike
									// condition, and we ignore it in the IsMandatory
									// getter anyway.
									haveMandatory = true;
									break;
								case ConstraintType.InternalUniqueness:
									if (haveUniqueness)
									{
										tooManyUniquenessConstraints = true;
									}
									else
									{
										haveUniqueness = true;
										if (roleSet.RoleCollection.Count == 2)
										{
											haveDoubleWideUniqueness = true;
										}
									}
									break;
							}
							if (tooManyUniquenessConstraints)
							{
								break;
							}
						}
						if (tooManyUniquenessConstraints)
						{
							retVal = RoleCardinality.Indeterminate;
						}
						else if (!haveUniqueness)
						{
							bool haveOppositeUniqueness = false;
							Role oppositeRole = roles[0];
							if (object.ReferenceEquals(oppositeRole, this))
							{
								oppositeRole = roles[1];
							}
							foreach (ConstraintRoleSet roleSet in oppositeRole.ConstraintRoleSetCollection)
							{
								if (roleSet.Constraint.ConstraintType == ConstraintType.InternalUniqueness)
								{
									haveOppositeUniqueness = true;
									break;
								}
							}
							if (haveOppositeUniqueness)
							{
								retVal = haveMandatory ? RoleCardinality.OneToMany : RoleCardinality.ZeroToMany;
							}
						}
						else if (haveDoubleWideUniqueness)
						{
							retVal = haveMandatory ? RoleCardinality.OneToMany : RoleCardinality.ZeroToMany;
						}
						else
						{
							retVal = haveMandatory ? RoleCardinality.ExactlyOne : RoleCardinality.ZeroToOne;
						}

					}
				}
				return retVal;
			}
			return base.GetValueForCustomStoredAttribute(attribute);
		}
		/// <summary>
		/// Standard override. Defer to GetValueForCustomStoredAttribute.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <returns></returns>
		protected override object GetOldValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			return GetValueForCustomStoredAttribute(attribute);
		}
		/// <summary>
		/// Standard override determine when derived attributes are
		/// displayed in the property grid. Called for all attributes.
		/// </summary>
		/// <param name="metaAttrInfo">MetaAttributeInfo</param>
		/// <returns></returns>
		public override bool ShouldCreatePropertyDescriptor(MetaAttributeInfo metaAttrInfo)
		{
			Guid attributeGuid = metaAttrInfo.Id;
			if (attributeGuid == CardinalityMetaAttributeGuid)
			{
				FactType fact = FactType;
				// Display for binary fact types
				return fact != null && fact.RoleCollection.Count == 2;
			}
			return base.ShouldCreatePropertyDescriptor(metaAttrInfo);
		}
		#endregion // CustomStorage handlers
		#region RoleChangeRule class
		[RuleOn(typeof(Role))]
		private class RoleChangeRule : ChangeRule
		{
			/// <summary>
			/// Forward through the property grid property to the underlying
			/// generating role property
			/// </summary>
			/// <param name="e"></param>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeGuid = e.MetaAttribute.Id;
				if (attributeGuid == Role.RolePlayerDisplayMetaAttributeGuid)
				{
					(e.ModelElement as Role).RolePlayer = e.NewValue as ObjectType;
				}
				else if (attributeGuid == Role.IsMandatoryMetaAttributeGuid)
				{
					Role role = e.ModelElement as Role;
					if ((bool)e.NewValue)
					{
						// Add a mandatory constraint
						Store store = role.Store;
						FactType factType;
						ORMModel model;
						if ((null == (factType = role.FactType)) ||
							(null == (model = factType.Model)))
						{
							throw new InvalidOperationException(ResourceStrings.ModelExceptionIsMandatoryRequiresAttachedFactType);
						}
						InternalConstraintRoleSet roleSet = InternalConstraintRoleSet.CreateInternalConstraintRoleSet(store);
						roleSet.RoleCollection.Add(role);
						InternalConstraint constraint = MandatoryConstraint.CreateMandatoryConstraint(store);
						constraint.Model = model;
						constraint.RoleSet = roleSet;
					}
					else
					{
						// Find and remove the mandatory constraint
						ConstraintRoleSetMoveableCollection constraintRoleSets = role.ConstraintRoleSetCollection;
						int roleSetCount = constraintRoleSets.Count;
						for (int i = roleSetCount - 1; i >= 0; --i) // The indices may change, go backwards
						{
							Constraint constraint = constraintRoleSets[i].Constraint;
							if (constraint.ConstraintType == ConstraintType.Mandatory)
							{
								constraint.Remove();
								// Should only have one of these, but we might as well keep going
								// because any of them would make the property appear to be true
							}
						}
					}
				}
				else if (attributeGuid == Role.CardinalityMetaAttributeGuid)
				{
					RoleCardinality oldCardinality = (RoleCardinality)e.OldValue;
					RoleCardinality newCardinality = (RoleCardinality)e.NewValue;

					if (newCardinality != RoleCardinality.Unspecified && newCardinality != RoleCardinality.Indeterminate)
					{
						Role role = e.ModelElement as Role;
						FactType factType = role.FactType;
						RoleMoveableCollection factRoles = factType.RoleCollection;
						if (factType == null || factRoles.Count != 2)
						{
							return; // Ignore the request
						}

						// First take care of the mandatory setting. We
						// can deduce this setting directly from the cardinality
						// values, so we don't touch the IsMandatory property unless
						// we really need to.
						bool newMandatory;
						switch (newCardinality)
						{
							case RoleCardinality.OneToMany:
							case RoleCardinality.ExactlyOne:
								newMandatory = true;
								break;
							default:
								newMandatory = false;
								break;
						}
						bool oldMandatory;
						switch (oldCardinality)
						{
							case RoleCardinality.Unspecified:
							case RoleCardinality.Indeterminate:
								oldMandatory = !newMandatory; // No data, for the property set
								break;
							case RoleCardinality.OneToMany:
							case RoleCardinality.ExactlyOne:
								oldMandatory = true;
								break;
							default:
								oldMandatory = false;
								break;
						}
						if (newMandatory ^ oldMandatory)
						{
							role.IsMandatory = newMandatory;
						}

						// Now take care of the one/many changes
						bool newOne;
						switch (newCardinality)
						{
							case RoleCardinality.ZeroToOne:
							case RoleCardinality.ExactlyOne:
								newOne = true;
								break;
							default:
								newOne = false;
								break;
						}
						bool oldOne;
						bool oldBroken = false;
						switch (oldCardinality)
						{
							case RoleCardinality.Indeterminate:
								oldBroken = oldOne = true;
								break;
							case RoleCardinality.ZeroToOne:
							case RoleCardinality.ExactlyOne:
								oldOne = true;
								break;
							default:
								// Assume many for unspecified.
								oldOne = false;
								break;
						}
						if (oldBroken)
						{
							// If there are multiple uniqueness constraints, then remove
							// all but one. Prefer a single-role constraint to a double-role
							// constraint and pretend that our old value is a many.
							ConstraintRoleSetMoveableCollection roleSets = role.ConstraintRoleSetCollection;
							int roleSetCount = roleSets.Count;
							// Go backwards so we can remove constraints
							Constraint keepCandidate = null;
							int keepRoleCardinality = 0;
							bool keepCandidateIsPreferred = false;
							for (int i = roleSetCount - 1; i >= 0; --i) // The indices may change, go backwards
							{
								ConstraintRoleSet roleSet = roleSets[i];
								Constraint constraint = roleSet.Constraint;
								if (constraint.ConstraintType == ConstraintType.InternalUniqueness)
								{
									int currentCardinality = roleSet.RoleCollection.Count;
									if (keepCandidate == null)
									{
										keepCandidate = constraint;
										keepRoleCardinality = currentCardinality;
										if (currentCardinality == 1)
										{
											keepCandidateIsPreferred = (constraint as InternalUniquenessConstraint).IsPreferred;
										}
									}
									else if (currentCardinality < keepRoleCardinality)
									{
										keepRoleCardinality = currentCardinality;
										keepCandidate.Remove();
										keepCandidate = constraint;
									}
									else
									{
										// Keep a preferred over a non-preferred. Preferred
										// constraints always have a single role.
										if (!keepCandidateIsPreferred &&
											currentCardinality == 1 &&
											(constraint as InternalUniquenessConstraint).IsPreferred)
										{
											keepCandidate.Remove();
											keepCandidate = constraint;
											keepCandidateIsPreferred = true;
										}
										else
										{
											constraint.Remove();
										}
									}
								}
							}
							if (keepRoleCardinality > 1)
							{
								oldOne = false;
							}
						}
						if (oldOne ^ newOne)
						{
							Store store = role.Store;
							if (newOne)
							{
								// We are considered a 'many' instead of a 'one' either
								// because there is no internal uniqueness constraint attached
								// to this role, or because there is a uniqueness constraint
								// attached to both roles. Figure out which one it is and
								// clear this role from the full-fact constraint if it is there.
								foreach (ConstraintRoleSet roleSet in role.ConstraintRoleSetCollection)
								{
									if (roleSet.Constraint.ConstraintType == ConstraintType.InternalUniqueness)
									{
										RoleMoveableCollection roles = roleSet.RoleCollection;
										Debug.Assert(roles.Count == 2);
										roles.Remove(role);
										break;
									}
								}

								// Now create a new uniqueness constraint containing only this role
								InternalUniquenessConstraint iuc = InternalUniquenessConstraint.CreateInternalUniquenessConstraint(store);
								iuc.Model = factType.Model;
								InternalConstraintRoleSet newRoleSet = InternalConstraintRoleSet.CreateInternalConstraintRoleSet(store);
								newRoleSet.RoleCollection.Add(role);
								iuc.RoleSet = newRoleSet;
							}
							else
							{
								// Switch to a many by removing an internal uniqueness constraint from
								// this role. If the opposite role does not have an internal uniqueness constraint,
								// then we need to automatically create a uniqueness constraint that spans both
								// roles.
								foreach (ConstraintRoleSet roleSet in role.ConstraintRoleSetCollection)
								{
									if (roleSet.Constraint.ConstraintType == ConstraintType.InternalUniqueness)
									{
										Debug.Assert(roleSet.RoleCollection.Count == 1);
										roleSet.Remove();
										break;
									}
								}
								Role oppositeRole = factRoles[0];
								if (object.ReferenceEquals(oppositeRole, role))
								{
									oppositeRole = factRoles[1];
								}
								bool oppositeHasUnique = false;
								foreach (ConstraintRoleSet roleSet in oppositeRole.ConstraintRoleSetCollection)
								{
									if (roleSet.Constraint.ConstraintType == ConstraintType.InternalUniqueness)
									{
										oppositeHasUnique = true;
										Debug.Assert(roleSet.RoleCollection.Count == 1);
									}
								}
								if (!oppositeHasUnique)
								{
									// Now create a new uniqueness constraint containing both roles
									InternalUniquenessConstraint iuc = InternalUniquenessConstraint.CreateInternalUniquenessConstraint(store);
									iuc.Model = factType.Model;
									InternalConstraintRoleSet newRoleSet = InternalConstraintRoleSet.CreateInternalConstraintRoleSet(store);
									RoleMoveableCollection setRoles = newRoleSet.RoleCollection;
									setRoles.Add(role);
									setRoles.Add(oppositeRole);
									iuc.RoleSet = newRoleSet;
								}
							}
						}
					}
				}
			}
		}
		#endregion // RoleChangeRule class
	}
}