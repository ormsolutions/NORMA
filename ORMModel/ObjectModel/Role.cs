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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region ValueRoleVisitor delegate definition
	/// <summary>
	/// Visit a role with ValueConstraint information when a dataType or
	/// role change is made that could affect the constraint's existence.
	/// Used with the <see cref="Role.WalkDescendedValueRoles(ObjectType, Role, UniquenessConstraint, ValueRoleVisitor)"/> method.
	/// </summary>
	/// <param name="role">A <see cref="Role"/> that is allowed to have a value constraint associated with it</param>
	/// <param name="pathedRole">A <see cref="PathedRole"/> that is allowed to have a value constraint associated with it</param>
	/// <param name="pathRoot">A <see cref="RolePathObjectTypeRoot"/> that is allowed to have a value constraint associated with it</param>
	/// <param name="dataTypeLink">The link to the data type</param>
	/// <param name="currentValueConstraint">The value constraint for the current role or pathed role.</param>
	/// <param name="previousValueConstraint">The last value constraint encountered during the walk</param>
	/// <returns>true to continue walking</returns>
	public delegate bool ValueRoleVisitor(Role role, PathedRole pathedRole, RolePathObjectTypeRoot pathRoot, ValueTypeHasDataType dataTypeLink, ValueConstraint currentValueConstraint, ValueConstraint previousValueConstraint);
	#endregion // ValueRoleVisitor delegate definition
	#region ReferenceSchemePattern enum
	/// <summary>
	/// Return values for <see cref="Role.GetReferenceSchemePattern(out ObjectType)"/>. Specifying how
	/// a <see cref="Role"/> relates to a reference schemes.
	/// </summary>
	public enum ReferenceSchemeRolePattern
	{
		/// <summary>
		/// The <see cref="Role"/> is not part of a <see cref="FactType"/> that
		/// matches the reference scheme pattern.
		/// </summary>
		None,
		/// <summary>
		/// The <see cref="Role"/> is the non-identifying role of a <see cref="FactType"/> that
		/// matches the simple reference scheme pattern. The non-identifying role has a simple
		/// mandatory constraint and non-preferred single-role uniqueness constraint. The
		/// role player is identified by a preferred single-role uniqueness constraint on
		/// the optional opposite role.
		/// </summary>
		OptionalSimpleIdentifiedRole,
		/// <summary>
		/// The <see cref="Role"/> is the non-identifying role of a <see cref="FactType"/> that
		/// matches the simple reference scheme pattern. The non-identifying role has a simple
		/// mandatory constraint and non-preferred single-role uniqueness constraint. The
		/// role player is identified by a preferred single-role uniqueness constraint on
		/// the mandatory opposite role.
		/// </summary>
		MandatorySimpleIdentifiedRole,
		/// <summary>
		/// The <see cref="Role"/> is the non-identifying role of a <see cref="FactType"/> that
		/// is part of a composite reference scheme. The role player for this role is identified
		/// by a preferred external uniqueness constraint on an opposite role. The mandatory
		/// state of this role is not currently tracked, so we do not break it down further.
		/// </summary>
		CompositeIdentifiedRole,
		/// <summary>
		/// The <see cref="Role"/> is an optional identifying role of a <see cref="FactType"/> that
		/// matches the simple reference scheme pattern. The identifying preferred single-role
		/// uniqueness constraint that is the preferred identifier for the role player of the
		/// opposite role.
		/// </summary>
		OptionalSimpleIdentifierRole,
		/// <summary>
		/// The <see cref="Role"/> is a mandatory identifying role of a <see cref="FactType"/> that
		/// matches the simple reference scheme pattern. The identifying preferred single-role
		/// uniqueness constraint that is the preferred identifier for the role player of the
		/// opposite role.
		/// </summary>
		MandatorySimpleIdentifierRole,
		/// <summary>
		/// The <see cref="Role"/> is an optional identifying role of a <see cref="FactType"/> the
		/// is part of a composite reference scheme pattern. The role is part of the external
		/// preferred uniqueness constraint for the role player on the opposite role.
		/// </summary>
		OptionalCompositeIdentifierRole,
		/// <summary>
		/// The <see cref="Role"/> is a mandatory identifying role of a <see cref="FactType"/> the
		/// is part of a composite reference scheme pattern. The role is part of the external
		/// preferred uniqueness constraint for the role player on the opposite role.
		/// </summary>
		MandatoryCompositeIdentifierRole,
		/// <summary>
		/// The <see cref="Role"/> is the implicit role created for the implied objectification.
		/// No further investigation is done to check the state of the opposite role. The implied
		/// objectification role always has single role uniqueness and mandatory constraints.
		/// </summary>
		ImpliedObjectificationRole,
	}
	#endregion // ReferenceSchemePattern enum
	partial class Role : IModelErrorOwner, IRedirectVerbalization, IVerbalizeChildren, IVerbalizeCustomChildren, INamedElementDictionaryParent, INamedElementDictionaryRemoteChild, IHasIndirectModelErrorOwner, IHierarchyContextEnabled
	{
		#region Helper methods
		#region IndexOf helper method for LinkedElementCollection<RoleBase>
		/// <summary>
		/// Determines the index of a specific Role in the list, resolving
		/// RoleProxy elements as needed
		/// </summary>
		/// <param name="roleBaseCollection">The list in which to locate the role</param>
		/// <param name="value">The Role to locate in the list</param>
		/// <returns>index of object</returns>
		public static int IndexOf(LinkedElementCollection<RoleBase> roleBaseCollection, Role value)
		{
			int count = roleBaseCollection.Count;
			for (int i = 0; i < count; ++i)
			{
				if (roleBaseCollection[i].Role == value)
				{
					return i;
				}
			}
			return -1;
		}
		#endregion // IndexOf helper method for LinkedElementCollection<RoleBase>
		#region ReplaceRole method
		/// <summary>
		/// Replaces <paramref name="existingRole"/> with <paramref name="replacementRole"/>, including altering all relationships
		/// in which <paramref name="existingRole"/> participates.
		/// </summary>
		public static void ReplaceRole(Role existingRole, Role replacementRole)
		{
			// Synchronize the names
			replacementRole.Name = existingRole.Name;

			// Alter the relationships that refer to existingRole to instead refer to replacementRole
			ReadOnlyCollection<ElementLink> elementLinks = DomainRoleInfo.GetAllElementLinks(existingRole);
			int elementLinksCount = elementLinks.Count;
			for (int i = 0; i < elementLinksCount; i++)
			{
				ElementLink elementLink = elementLinks[i];
				ReadOnlyCollection<DomainRoleInfo> domainRoles = elementLink.GetDomainRelationship().DomainRoles;
				DomainRoleInfo domainRoleInfo;
				if (DomainRoleInfo.GetSourceRolePlayer(elementLink) == existingRole)
				{
					Debug.Assert(DomainRoleInfo.GetTargetRolePlayer(elementLink) != existingRole, "We shouldn't have a relationship from ourselves to ourselves.");
					domainRoleInfo = domainRoles[0];
					Debug.Assert(domainRoleInfo.IsSource);
				}
				else
				{
					domainRoleInfo = domainRoles[1];
					Debug.Assert(!domainRoleInfo.IsSource);
				}
				DomainRoleInfo.SetRolePlayer(elementLink, domainRoleInfo.Id, replacementRole);
			}
		}
		#endregion // ReplaceRole method
		#region GetReferenceSchemePattern method
		/// <summary>
		/// Determine if the role is part of a <see cref="FactType"/> that participates
		/// in a simple or complex reference scheme.
		/// </summary>
		/// <returns>A <see cref="ReferenceSchemeRolePattern"/> value</returns>
		public ReferenceSchemeRolePattern GetReferenceSchemePattern()
		{
			ObjectType identifiedEntityType;
			return GetReferenceSchemePattern(out identifiedEntityType);
		}
		/// <summary>
		/// Determine if the role is part of a <see cref="FactType"/> that participates
		/// in a simple or complex reference scheme.
		/// </summary>
		/// <param name="identifiedEntityType">The <see cref="ObjectType"/> that is identified by the reference scheme pattern.</param>
		/// <returns>A <see cref="ReferenceSchemeRolePattern"/> value</returns>
		public ReferenceSchemeRolePattern GetReferenceSchemePattern(out ObjectType identifiedEntityType)
		{
			UniquenessConstraint uniqueness;
			RoleBase oppositeRoleBase;
			LinkedElementCollection<Role> uniquenessRoles;
			ObjectType preferredFor;
			identifiedEntityType = null;
			if (null != (oppositeRoleBase = OppositeRole))
			{
				Role oppositeRole;
				if (null == (oppositeRole = oppositeRoleBase as Role) || oppositeRole is ObjectifiedUnaryRole)
				{
					// These roles always have the same pattern, no need to look any farther
					return ReferenceSchemeRolePattern.ImpliedObjectificationRole;
				}
				else if (null != (uniqueness = SingleRoleAlethicUniquenessConstraint))
				{
					preferredFor = uniqueness.PreferredIdentifierFor;

					if (preferredFor != null)
					{
						// We're on the preferred end of a binary fact type that
						// matches the simple reference scheme pattern
						if (oppositeRole.RolePlayer == preferredFor)
						{
							identifiedEntityType = preferredFor;
							return SingleRoleAlethicMandatoryConstraint == null ?
								ReferenceSchemeRolePattern.OptionalSimpleIdentifierRole :
								ReferenceSchemeRolePattern.MandatorySimpleIdentifierRole;
						}
					}
					else
					{
						preferredFor = RolePlayer;
						if (preferredFor != null &&
							null != (uniqueness = preferredFor.PreferredIdentifier) &&
							(uniquenessRoles = uniqueness.RoleCollection).Contains(oppositeRole))
						{
							identifiedEntityType = preferredFor;
							return uniquenessRoles.Count == 1 ?
								oppositeRole.SingleRoleAlethicMandatoryConstraint == null ?
									ReferenceSchemeRolePattern.OptionalSimpleIdentifiedRole :
									ReferenceSchemeRolePattern.MandatorySimpleIdentifiedRole :
								ReferenceSchemeRolePattern.CompositeIdentifiedRole;
						}
					}
				}
				else if (null != (uniqueness = oppositeRole.SingleRoleAlethicUniquenessConstraint) &&
					null != (preferredFor = oppositeRole.RolePlayer) &&
					null != (uniqueness = preferredFor.PreferredIdentifier) &&
					1 < (uniquenessRoles = uniqueness.RoleCollection).Count &&
					uniquenessRoles.Contains(this))
				{
					identifiedEntityType = preferredFor;
					return SingleRoleAlethicMandatoryConstraint == null ?
						ReferenceSchemeRolePattern.OptionalCompositeIdentifierRole :
						ReferenceSchemeRolePattern.MandatoryCompositeIdentifierRole;
				}
			}
			return ReferenceSchemeRolePattern.None;
		}
		#endregion // GetReferenceSchemePattern method
		#endregion // Helper methods
		#region CustomStorage handlers
		#region CustomStorage setters
		private void SetIsMandatoryValue(bool newValue)
		{
			// Handled by RoleChangeRule
		}
		private void SetMandatoryConstraintNameValue(string newValue)
		{
			// Handled by RoleChangeRule
		}
		private void SetMultiplicityValue(RoleMultiplicity newValue)
		{
			// Handled by RoleChangeRule
		}
		private void SetValueRangeTextValue(string newValue)
		{
			// Handled by RoleChangeRule
		}
		private void SetMandatoryConstraintModalityValue(ConstraintModality newValue)
		{
			// Handled by RoleChangeRule
		}
		private void SetObjectificationOppositeRoleNameValue(string newValue)
		{
			// Handled by RoleChangeRule
		}
		#endregion // CustomStorage setters
		/// <summary>
		/// Gets the explicit <see cref="MandatoryConstraint"/> associated with this <see cref="Role"/>, if any.
		/// To get a single-role alethic mandatory constraint (either explicit or implied) on this Role, use the
		/// <see cref="SingleRoleAlethicMandatoryConstraint"/> property.
		/// </summary>
		public MandatoryConstraint SimpleMandatoryConstraint
		{
			get
			{
				LinkedElementCollection<ConstraintRoleSequence> constraintRoleSequences = ConstraintRoleSequenceCollection;
				int roleSequenceCount = constraintRoleSequences.Count;
				for (int i = 0; i < roleSequenceCount; ++i)
				{
					ConstraintRoleSequence roleSequence = constraintRoleSequences[i];
					IConstraint constraint = roleSequence.Constraint;
					if (constraint != null)
					{
						if (constraint.ConstraintType == ConstraintType.SimpleMandatory)
						{
							return (MandatoryConstraint)constraint;
						}
					}
				}
				return null;
			}
		}

		/// <summary>
		/// Gets an explicit or implicit <see cref="MandatoryConstraint"/> with alethic <see cref="ConstraintModality">modality</see>
		/// associated with this <see cref="Role"/>, if any.
		/// Use the <see cref="SimpleMandatoryConstraint"/> constraint property to get explicit constraint of
		/// any modality.
		/// </summary>
		public MandatoryConstraint SingleRoleAlethicMandatoryConstraint
		{
			get
			{
				LinkedElementCollection<ConstraintRoleSequence> constraintRoleSequences = ConstraintRoleSequenceCollection;
				int roleSequenceCount = constraintRoleSequences.Count;
				for (int i = 0; i < roleSequenceCount; ++i)
				{
					ConstraintRoleSequence roleSequence = constraintRoleSequences[i];
					IConstraint constraint = roleSequence.Constraint;
					if (constraint != null && constraint.Modality == ConstraintModality.Alethic)
					{
						switch (constraint.ConstraintType)
						{
							case ConstraintType.SimpleMandatory:
								return (MandatoryConstraint)constraint;
							case ConstraintType.ImpliedMandatory:
								if (roleSequence.RoleCollection.Count == 1)
								{
									return (MandatoryConstraint)constraint;
								}
								break;
						}
					}
				}
				return null;
			}
		}

		/// <summary>
		/// Gets the <see cref="ConstraintModality.Alethic"/> single role <see cref="ConstraintRoleSequence"/> for the constraint of type <see cref="ConstraintType.InternalUniqueness"/>
		/// associated with this <see cref="Role"/>, if any.
		/// </summary>
		public UniquenessConstraint SingleRoleAlethicUniquenessConstraint
		{
			get
			{
				LinkedElementCollection<ConstraintRoleSequence> constraintRoleSequences = ConstraintRoleSequenceCollection;
				int roleSequenceCount = constraintRoleSequences.Count;
				for (int i = 0; i < roleSequenceCount; ++i)
				{
					ConstraintRoleSequence roleSequence = constraintRoleSequences[i];
					IConstraint constraint = roleSequence.Constraint;
					if (constraint.ConstraintType == ConstraintType.InternalUniqueness && constraint.Modality == ConstraintModality.Alethic && roleSequence.RoleCollection.Count == 1)
					{
						return (UniquenessConstraint)roleSequence;
					}
				}
				return null;
			}
		}

		/// <summary>
		/// Gets the single role internal <see cref="UniquenessConstraint"/>
		/// associated with this <see cref="Role"/>, if any.
		/// </summary>
		public UniquenessConstraint SingleRoleUniquenessConstraint
		{
			get
			{
				LinkedElementCollection<ConstraintRoleSequence> constraintRoleSequences = ConstraintRoleSequenceCollection;
				int roleSequenceCount = constraintRoleSequences.Count;
				for (int i = 0; i < roleSequenceCount; ++i)
				{
					ConstraintRoleSequence roleSequence = constraintRoleSequences[i];
					IConstraint constraint = roleSequence.Constraint;
					if (constraint.ConstraintType == ConstraintType.InternalUniqueness && roleSequence.RoleCollection.Count == 1)
					{
						return (UniquenessConstraint)roleSequence;
					}
				}
				return null;
			}
		}

		private bool GetIsMandatoryValue()
		{
			return SimpleMandatoryConstraint != null;
		}
		private string GetMandatoryConstraintNameValue()
		{
			MandatoryConstraint smc = SimpleMandatoryConstraint;
			return (smc != null) ? smc.Name : String.Empty;
		}
		private RoleMultiplicity GetMultiplicityValue()
		{
			RoleMultiplicity retVal = RoleMultiplicity.Unspecified;
			FactType fact = FactType;
			if (fact != null)
			{
				LinkedElementCollection<RoleBase> roles = fact.RoleCollection;
				if (roles.Count == 2 && !FactType.GetUnaryRoleIndex(roles).HasValue)
				{
					Role oppositeRole = roles[0].Role;
					if (oppositeRole == this)
					{
						oppositeRole = roles[1].Role;
					}
					bool haveMandatory = false;
					bool haveUniqueness = false;
					bool haveDoubleWideUniqueness = false;
					bool tooManyUniquenessConstraints = false;
					foreach (ConstraintRoleSequence roleSet in oppositeRole.ConstraintRoleSequenceCollection)
					{
						IConstraint constraint = roleSet.Constraint;
						if (constraint.Modality == ConstraintModality.Alethic)
						{
							switch (constraint.ConstraintType)
							{
								case ConstraintType.SimpleMandatory:
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
						}
						if (tooManyUniquenessConstraints)
						{
							break;
						}
					}
					if (tooManyUniquenessConstraints)
					{
						retVal = RoleMultiplicity.Indeterminate;
					}
					else if (!haveUniqueness)
					{
						bool haveThisUniqueness = false;
						foreach (ConstraintRoleSequence roleSet in this.ConstraintRoleSequenceCollection)
						{
							UniquenessConstraint oppositeUniqueness = roleSet as UniquenessConstraint;
							if (oppositeUniqueness != null && oppositeUniqueness.IsInternal && oppositeUniqueness.Modality == ConstraintModality.Alethic)
							{
								haveThisUniqueness = true;
								break;
							}
						}
						if (haveThisUniqueness)
						{
							retVal = haveMandatory ? RoleMultiplicity.OneToMany : RoleMultiplicity.ZeroToMany;
						}
					}
					else if (haveDoubleWideUniqueness)
					{
						retVal = haveMandatory ? RoleMultiplicity.OneToMany : RoleMultiplicity.ZeroToMany;
					}
					else
					{
						retVal = haveMandatory ? RoleMultiplicity.ExactlyOne : RoleMultiplicity.ZeroToOne;
					}
				}
			}
			return retVal;
		}
		private string GetValueRangeTextValue()
		{
			RoleValueConstraint valueConstraint = ValueConstraint;
			return (valueConstraint != null) ? valueConstraint.Text : String.Empty;
		}
		private ConstraintModality GetMandatoryConstraintModalityValue()
		{
			MandatoryConstraint smc = SimpleMandatoryConstraint;
			return (smc != null) ? smc.Modality : ConstraintModality.Alethic;
		}
		private string GetObjectificationOppositeRoleNameValue()
		{
			RoleProxy roleProxy = Proxy;
			Role proxyOppositeRole;
			return (roleProxy != null && (proxyOppositeRole = roleProxy.OppositeRole as Role) != null) ? proxyOppositeRole.Name : String.Empty;
		}
		#endregion // CustomStorage handlers
		#region Non-DSL Custom Properties
		/// <summary>
		/// The ObjectType that plays this Role.
		/// </summary>
		[Editor(typeof(Design.RolePlayerPicker), typeof(UITypeEditor))]
		public ObjectType RolePlayerDisplay
		{
			get
			{
				return RolePlayer;
			}
			set
			{
				RolePlayer = value;
			}
		}
		/// <summary>
		/// The user-editable display form of a cardinality constraint.
		/// </summary>
		[DefaultValue(CardinalityConstraint.DefaultCardinalityDisplay)]
		public string CardinalityDisplay
		{
			get
			{
				return CardinalityConstraint.GetEditableRangeDisplay(this.Cardinality);
			}
			set
			{
				CardinalityConstraint.UpdateCardinality(this, value);
			}
		}
		#endregion // Non-DSL Custom Properties
		#region ValueRole methods
		/// <summary>
		/// Retrieve an array of roles starting with a role
		/// attached to a ValueType and ending with this role.
		/// If this role cannot have a ValueConstraint attached
		/// to it, then return <see langword="null"/>.
		/// </summary>
		public Role[] GetValueRoles()
		{
			Role[] retVal;
			GetValueRoles(this, this.RolePlayer, 0, out retVal);
			return retVal;
		}

		/// <summary>
		/// True if a ValueConstraint may be attached to this role. This
		/// duplicates the work of <see cref="GetValueRoles()"/> without actually
		/// retrieving the roles, so call GetValueRoles directly and test for
		/// null if you need the sequence of roles from the value type role back
		/// to this one.
		/// </summary>
		public bool IsValueRole
		{
			get
			{
				Role[] dummy;
				return GetValueRoles(this, this.RolePlayer, -1, out dummy);
			}
		}
		/// <summary>
		/// Ask the <see cref="IsValueRole"/> question with an object
		/// that is not the current role player.
		/// </summary>
		/// <param name="alternateRolePlayer">An alternate role player.</param>
		/// <returns>true if this role attached to the alternate role player would be a value role</returns>
		public bool IsValueRoleForAlternateRolePlayer(ObjectType alternateRolePlayer)
		{
			Role[] dummy;
			return GetValueRoles(this, alternateRolePlayer, -1, out dummy);
		}
		/// <summary>
		/// Recursively retrieve a sequence of roles that are
		/// allowed to have value constraints.
		/// </summary>
		/// <param name="currentRole">The current role to test</param>
		/// <param name="rolePlayer">The role player for the current role.</param>
		/// <param name="depth">The current depth. Pass in -1 to skip populating the output
		/// roles and 0 to seed recursion.</param>
		/// <param name="roles">An array of roles. The roles are in reverse order, so
		/// roles[0] will always have a ValueType as its RolePlayer.</param>
		/// <returns>true if the current role can have a value constraint</returns>
		private static bool GetValueRoles(Role currentRole, ObjectType rolePlayer, int depth, out Role[] roles)
		{
			roles = null;
			if (depth == 100 || depth == -101)
			{
				// Cycling
				return false;
			}
			if (rolePlayer != null)
			{
				if (rolePlayer.IsValueType)
				{
					if (depth < 0)
					{
						return true;
					}
					// This is the first element in the chain and
					// can be used to retrieve the value type.
					roles = new Role[depth + 1];
					roles[0] = currentRole;
				}
				else
				{
					UniquenessConstraint preferredIdentifier = rolePlayer.ResolvedPreferredIdentifier;
					LinkedElementCollection<Role> identifierRoles;
					Role nextRole;
					if (preferredIdentifier != null &&
						(identifierRoles = preferredIdentifier.RoleCollection).Count == 1 &&
						(nextRole = identifierRoles[0]).FactType != currentRole.FactType)
					{
						if (depth < 0)
						{
							return GetValueRoles(nextRole, nextRole.RolePlayer, depth - 1, out roles);
						}
						if (GetValueRoles(nextRole, nextRole.RolePlayer, depth + 1, out roles))
						{
							roles[roles.Length - depth - 1] = currentRole;
						}
					}
				}
			}
			return roles != null;
		}
		/// <summary>
		/// Get all value roles including all roles directly attached to the provided
		/// object type and any roles descended from this one through prefererred identifiers.
		/// Walks the opposite direction of <see cref="Role.GetValueRoles()"/>
		/// </summary>
		/// <param name="anchorType">The <see cref="ObjectType"/> to walk descended roles for</param>
		/// <param name="unattachedRole">A role to test that is not currently attached to the anchorType.
		/// If unattachedRole is not null, then only this role will be tested. Otherwise, all current played
		/// roles will be walked.</param>
		/// <param name="unattachedPreferredIdentifier">A preferred identifier to test that is not currently
		/// attached to the anchorType.</param>
		/// <param name="visitor">A <see cref="ValueRoleVisitor"/> callback delegate.</param>
		public static void WalkDescendedValueRoles(ObjectType anchorType, Role unattachedRole, UniquenessConstraint unattachedPreferredIdentifier, ValueRoleVisitor visitor)
		{
			ValueTypeHasDataType dataTypeLink;
			if (null == unattachedPreferredIdentifier &&
				null != (dataTypeLink = anchorType.GetDataTypeLink()))
			{
				ObjectType unattachedRolePlayer;
				WalkDescendedValueRoles(
					(unattachedRole != null) ? new Role[] { unattachedRole } as IList<Role> : anchorType.PlayedRoleCollection,
					RolePathObjectTypeRoot.GetLinksToRolePathCollection(anchorType),
					dataTypeLink,
					anchorType.ValueConstraint,
					null,
					(null == unattachedRole || null == (unattachedRolePlayer = unattachedRole.RolePlayer)) ? false : !unattachedRolePlayer.IsValueType,
					visitor);
			}
			else
			{
				LinkedElementCollection<Role> roles;
				UniquenessConstraint preferredIdentifier;
				if (null != (preferredIdentifier = unattachedPreferredIdentifier ?? anchorType.ResolvedPreferredIdentifier) &&
					(roles = preferredIdentifier.RoleCollection).Count == 1)
				{
					Role currentRole = roles[0];
					Role[] valueRoles = currentRole.GetValueRoles();
					if (valueRoles != null)
					{
						ValueConstraint nearestValueConstraint = null;
						int valueRolesCount = valueRoles.Length;
						for (int i = valueRolesCount - 1; i >= 0; --i)
						{
							nearestValueConstraint = valueRoles[i].ValueConstraint;
							if (nearestValueConstraint != null)
							{
								break;
							}
						}
						ObjectType valueType = valueRoles[0].RolePlayer;
						dataTypeLink = valueType.GetDataTypeLink();
						if (nearestValueConstraint == null)
						{
							nearestValueConstraint = valueType.ValueConstraint;
						}
						RoleBase nextSkipRole = currentRole.OppositeRoleAlwaysResolveProxy;
						if (nextSkipRole != null)
						{
							WalkDescendedValueRoles(
								(unattachedRole != null) ? new Role[] { unattachedRole } as IList<Role> : anchorType.PlayedRoleCollection,
								RolePathObjectTypeRoot.GetLinksToRolePathCollection(anchorType),
								dataTypeLink,
								nearestValueConstraint,
								nextSkipRole.Role,
								true,
								visitor);
						}
					}
				}
			}
		}
		/// <summary>
		/// Helper method to recursively walk value roles. A value role
		/// is any role that is allowed to have a value constraint.
		/// </summary>
		/// <param name="playedRoles">Roles from an ObjectType to walk. The assumption is made that the
		/// owning ObjectType is either a value type or has a preferred identifier with exactly one role</param>
		/// <param name="dataTypeLink">The data type information for the constraint</param>
		/// <param name="pathRoots">The <see cref="RolePathObjectTypeRoot"/> relationships from a role path associated
		/// with the root path.</param>
		/// <param name="previousValueConstraint">The value constraint nearest this value role.
		/// Any value constraint on the current set of roles must be a subset of the previousValueConstraint.</param>
		/// <param name="skipRole">A role to skip. If the playedRoles came from a preferred identifier,
		/// then the skipRole is the opposite role.</param>
		/// <param name="walkSubtypes">true to walk subtypes. Should be true if the playedRoles come from an
		/// EntityType and false if they come from a ValueType</param>
		/// <param name="visitor">The callback delegate</param>
		/// <returns>true to continue iteration</returns>
		private static bool WalkDescendedValueRoles(IList<Role> playedRoles, IList<RolePathObjectTypeRoot> pathRoots, ValueTypeHasDataType dataTypeLink, ValueConstraint previousValueConstraint, Role skipRole, bool walkSubtypes, ValueRoleVisitor visitor)
		{
			int count = pathRoots.Count;
			for (int i = 0; i < count; ++i)
			{
				RolePathObjectTypeRoot pathRoot = pathRoots[i];
				if (!visitor(null, null, pathRoot, dataTypeLink, pathRoot.ValueConstraint, previousValueConstraint))
				{
					return false;
				}
			}
			count = playedRoles.Count;
			for (int i = 0; i < count; ++i)
			{
				Role role = playedRoles[i];
				SupertypeMetaRole supertypeRole;
				if (role == skipRole)
				{
					// Nothing to do
				}
				else if (null != (supertypeRole = role as SupertypeMetaRole))
				{
					if (walkSubtypes)
					{
						SubtypeFact subtypeFact = (SubtypeFact)role.FactType;
						ObjectType subtype;
						if (subtypeFact.ProvidesPreferredIdentifier &&
							null != (subtype = subtypeFact.Subtype) &&
							subtype.PreferredIdentifier == null)
						{
							if (!WalkDescendedValueRoles(subtype.PlayedRoleCollection, RolePathObjectTypeRoot.GetLinksToRolePathCollection(subtype), dataTypeLink, previousValueConstraint, null, true, visitor))
							{
								return false;
							}
						}
					}
				}
				else if (!(role is SubtypeMetaRole))
				{
					RoleValueConstraint currentValueConstraint = role.ValueConstraint;
					if (!visitor(role, null, null, dataTypeLink, currentValueConstraint, previousValueConstraint))
					{
						return false;
					}
					if (currentValueConstraint != null && !currentValueConstraint.IsDeleted)
					{
						previousValueConstraint = currentValueConstraint;
					}
					foreach (PathedRole pathedRole in PathedRole.GetLinksToRolePathCollection(role))
					{
						// UNDONE: VALUEROLE This does not correctly report a value constraint from a previous
						// path node. Note that this, as well as allowing value restrictions on supertype roles
						// (and possibly other patterns), can result in multiple previous value constraints, so
						// the callback signature may possibly need to be modified here. As of changeset 1442,
						// none of the callbacks use the previousValueConstraint information, so we can ignore
						// this for now.

						// Note that we visit for the pathed role even if no value constraint is present
						// to allow processing for this pathed role.
						if (!visitor(role, pathedRole, null, dataTypeLink, pathedRole.ValueConstraint, previousValueConstraint))
						{
							return false;
						}
					}

					// Walk sequences to find a single-role preferred identifier so
					// we can get to the next link.
					LinkedElementCollection<ConstraintRoleSequence> sequences = role.ConstraintRoleSequenceCollection;
					int sequencesCount = sequences.Count;
					for (int j = 0; j < sequencesCount; ++j)
					{
						UniquenessConstraint constraint = sequences[j] as UniquenessConstraint;
						ObjectType identifierFor;
						if (null != (constraint = sequences[j] as UniquenessConstraint) &&
							null != (identifierFor = constraint.PreferredIdentifierFor) &&
							constraint.RoleCollection.Count == 1)
						{
							RoleBase nextSkipRole = role.OppositeRoleAlwaysResolveProxy;
							if (nextSkipRole == null)
							{
								return false;
							}
							if (!WalkDescendedValueRoles(identifierFor.PlayedRoleCollection, RolePathObjectTypeRoot.GetLinksToRolePathCollection(identifierFor), dataTypeLink, previousValueConstraint, nextSkipRole.Role, true, visitor))
							{
								return false;
							}
						}
					}
				}
			}
			return true;
		}
		#endregion // ValueRole methods
		#region RoleChangeRule
		/// <summary>
		/// ChangeRule: typeof(Role)
		/// Forward through the property grid property to the underlying
		/// generating role property
		/// </summary>
		private static void RoleChangeRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeGuid = e.DomainProperty.Id;
			if (attributeGuid == Role.ValueRangeTextDomainPropertyId)
			{
				Role role = e.ModelElement as Role;
				RoleValueConstraint valueConstraint = role.ValueConstraint;
				if (valueConstraint == null)
				{
					role.ValueConstraint = valueConstraint = new RoleValueConstraint(role.Partition);
				}
				valueConstraint.Text = (string)e.NewValue;
			}
			#region Handle IsMandatory property changes
			else if (attributeGuid == Role.IsMandatoryDomainPropertyId)
			{
				Role role = e.ModelElement as Role;
				if ((bool)e.NewValue)
				{
					// Add a mandatory constraint
					FactType factType;
					if (null == (factType = role.FactType))
					{
						throw new InvalidOperationException(ResourceStrings.ModelExceptionIsMandatoryRequiresAttachedFactType);
					}
					MandatoryConstraint.CreateSimpleMandatoryConstraint(role);
				}
				else
				{
					// Find and remove the mandatory constraint
					LinkedElementCollection<ConstraintRoleSequence> constraintRoleSequences = role.ConstraintRoleSequenceCollection;
					int roleSequenceCount = constraintRoleSequences.Count;
					for (int i = roleSequenceCount - 1; i >= 0; --i) // The indices may change, go backwards
					{
						IConstraint constraint = constraintRoleSequences[i].Constraint;
						if (constraint.ConstraintType == ConstraintType.SimpleMandatory)
						{
							(constraint as ModelElement).Delete();
							// Should only have one of these, but we might as well keep going
							// because any of them would make the property appear to be true
						}
					}
				}
			}
			#endregion // Handle IsMandatory property changes
			#region Handle MandatoryConstraintName property changes
			else if (attributeGuid == Role.MandatoryConstraintNameDomainPropertyId)
			{
				Role role = e.ModelElement as Role;
				MandatoryConstraint smc = role.SimpleMandatoryConstraint;
				if (smc != null)
				{
					smc.Name = (string)e.NewValue;
				}
			}
			#endregion // Handle MandatoryConstraintName property changes
			#region Handle Multiplicity property changes
			else if (attributeGuid == Role.MultiplicityDomainPropertyId)
			{
				RoleMultiplicity oldMultiplicity = (RoleMultiplicity)e.OldValue;
				RoleMultiplicity newMultiplicity = (RoleMultiplicity)e.NewValue;

				if (newMultiplicity != RoleMultiplicity.Unspecified && newMultiplicity != RoleMultiplicity.Indeterminate)
				{
					Role testRole = (Role)e.ModelElement;
					FactType factType = testRole.FactType;
					LinkedElementCollection<RoleBase> factRoles;
					Role role = null;
					if (factType.ImpliedByObjectification != null)
					{
						role = testRole.OppositeRole.Role; // Jumps from proxy to role in objectified fact type
						factType = role.FactType;
						factRoles = factType.RoleCollection;
					}
					else
					{
						factRoles = factType.RoleCollection;
					}
					if (factType == null || factRoles.Count != 2 || FactType.GetUnaryRoleIndex(factRoles).HasValue)
					{
						return; // Ignore the request
					}

					// We implemented this backwards, so switch to the opposite role.
					// Switch is already done for link fact type case.
					if (role == null)
					{
						role = factRoles[0].Role;
						if (role == testRole)
						{
							role = factRoles[1].Role;
						}
					}

					// First take care of the mandatory setting. We
					// can deduce this setting directly from the multiplicity
					// values, so we don't touch the IsMandatory property unless
					// we really need to.
					bool newMandatory;
					switch (newMultiplicity)
					{
						case RoleMultiplicity.OneToMany:
						case RoleMultiplicity.ExactlyOne:
							newMandatory = true;
							break;
						default:
							newMandatory = false;
							break;
					}
					bool oldMandatory;
					switch (oldMultiplicity)
					{
						case RoleMultiplicity.Unspecified:
						case RoleMultiplicity.Indeterminate:
							oldMandatory = !newMandatory; // No data, for the property set
							break;
						case RoleMultiplicity.OneToMany:
						case RoleMultiplicity.ExactlyOne:
							oldMandatory = true;
							break;
						default:
							oldMandatory = false;
							break;
					}
					if (newMandatory ^ oldMandatory)
					{
						MandatoryConstraint simpleMandatory = role.SimpleMandatoryConstraint;
						if (newMandatory)
						{
							if (simpleMandatory != null)
							{
								simpleMandatory.Modality = ConstraintModality.Alethic;
							}
							else
							{
								MandatoryConstraint.CreateSimpleMandatoryConstraint(role);
							}
						}
						else if (simpleMandatory != null && simpleMandatory.Modality == ConstraintModality.Alethic)
						{
							role.IsMandatory = false;
						}
					}

					// Now take care of the one/many changes
					bool newOne;
					switch (newMultiplicity)
					{
						case RoleMultiplicity.ZeroToOne:
						case RoleMultiplicity.ExactlyOne:
							newOne = true;
							break;
						default:
							newOne = false;
							break;
					}
					bool oldOne;
					bool oldBroken = false;
					switch (oldMultiplicity)
					{
						case RoleMultiplicity.Unspecified:
							// Make sure we get into the main code
							oldOne = !newOne;
							break;
						case RoleMultiplicity.Indeterminate:
							oldBroken = oldOne = true;
							break;
						case RoleMultiplicity.ZeroToOne:
						case RoleMultiplicity.ExactlyOne:
							oldOne = true;
							break;
						default:
							oldOne = false;
							break;
					}
					if (oldBroken)
					{
						// If there are multiple uniqueness constraints, then remove
						// all but one. Prefer a single-role constraint to a double-role
						// constraint and pretend that our old value is a many.
						LinkedElementCollection<ConstraintRoleSequence> roleSequences = role.ConstraintRoleSequenceCollection;
						int roleSequenceCount = roleSequences.Count;
						// Go backwards so we can remove constraints
						IConstraint keepCandidate = null;
						int keepRoleMultiplicity = 0;
						bool keepCandidateIsPreferred = false;
						for (int i = roleSequenceCount - 1; i >= 0; --i) // The indices may change, go backwards
						{
							ConstraintRoleSequence roleSequence = roleSequences[i];
							IConstraint constraint = roleSequence.Constraint;
							if (constraint.ConstraintType == ConstraintType.InternalUniqueness && constraint.Modality == ConstraintModality.Alethic)
							{
								int currentMultiplicity = roleSequence.RoleCollection.Count;
								if (keepCandidate == null)
								{
									keepCandidate = constraint;
									keepRoleMultiplicity = currentMultiplicity;
									if (currentMultiplicity == 1)
									{
										keepCandidateIsPreferred = (constraint as UniquenessConstraint).IsPreferred;
									}
								}
								else if (currentMultiplicity < keepRoleMultiplicity)
								{
									keepRoleMultiplicity = currentMultiplicity;
									(keepCandidate as ModelElement).Delete();
									keepCandidate = constraint;
								}
								else
								{
									// Keep a preferred over a non-preferred. Preferred
									// constraints always have a single role.
									if (!keepCandidateIsPreferred &&
										currentMultiplicity == 1 &&
										(constraint as UniquenessConstraint).IsPreferred)
									{
										(keepCandidate as ModelElement).Delete();
										keepCandidate = constraint;
										keepCandidateIsPreferred = true;
									}
									else
									{
										(constraint as ModelElement).Delete();
									}
								}
							}
						}
						if (keepRoleMultiplicity > 1)
						{
							oldOne = false;
						}
					}
					if (oldOne ^ newOne)
					{
						if (newOne)
						{
							// We are considered a 'many' instead of a 'one' either
							// because there is no internal uniqueness constraint attached
							// to this role, or because there is a uniqueness constraint
							// attached to both roles. Figure out which one it is and remove
							// the spanning constraint. The other option is to remove the
							// role from the spanning constraint, but this leaves us with
							// a zero-to-one or 1-to-1 multiplicity on the opposite role,
							// which is a change from the current zero-to-many or one-to-many
							// multiplicity it currently has.
							UniquenessConstraint existingDeonticUniqueness = null;
							LinkedElementCollection<ConstraintRoleSequence> sequences = role.ConstraintRoleSequenceCollection;
							int sequenceCount = sequences.Count;
							// Walk backwards so we can delete
							for (int i = sequenceCount - 1; i >= 0; --i)
							{
								ConstraintRoleSequence roleSequence = sequences[i];
								IConstraint spanningConstraint = roleSequence.Constraint;
								if (spanningConstraint.ConstraintType == ConstraintType.InternalUniqueness)
								{
									UniquenessConstraint currentUniqueness = (UniquenessConstraint)roleSequence;
									if (roleSequence.RoleCollection.Count == 1 &&
										currentUniqueness.Modality == ConstraintModality.Deontic &&
										existingDeonticUniqueness == null)
									{
										existingDeonticUniqueness = currentUniqueness;
									}
									else
									{
										currentUniqueness.Delete();
										// There may be more than one of these, the early broken state check
										// only checked alethic constraints. Do not break.
									}
								}
							}

							// Now create a new uniqueness constraint containing only this role
							if (existingDeonticUniqueness != null)
							{
								existingDeonticUniqueness.Modality = ConstraintModality.Alethic;
							}
							else
							{
								new ConstraintRoleSequenceHasRole(UniquenessConstraint.CreateInternalUniquenessConstraint(factType), role);
							}
						}
						else
						{
							bool oppositeHasUnique = false;
							bool wasUnspecified = oldMultiplicity == RoleMultiplicity.Unspecified;
							UniquenessConstraint existingDeonticSpanningConstraint = null;
							// Switch to a many by removing an internal uniqueness constraint from
							// this role. If the opposite role does not have an internal uniqueness constraint,
							// then we need to automatically create a uniqueness constraint that spans both
							// roles.
							LinkedElementCollection<ConstraintRoleSequence> sequences = role.ConstraintRoleSequenceCollection;
							int sequenceCount = sequences.Count;
							// Walk backwards so we can delete
							for (int i = sequenceCount - 1; i >= 0; --i)
							{
								ConstraintRoleSequence roleSequence = sequences[i];
								IConstraint constraint = roleSequence.Constraint;
								if (constraint.ConstraintType == ConstraintType.InternalUniqueness)
								{
									UniquenessConstraint currentUniqueness = (UniquenessConstraint)roleSequence;
									if (roleSequence.RoleCollection.Count == 2 &&
										currentUniqueness.Modality == ConstraintModality.Deontic &&
										existingDeonticSpanningConstraint == null)
									{
										existingDeonticSpanningConstraint = currentUniqueness;
									}
									else if (currentUniqueness.Modality == ConstraintModality.Alethic)
									{
										currentUniqueness.Delete();
										// Don't break, we may find an existing deontic spanning constraint later in the set
									}
								}
							}
							RoleBase oppositeBaseRole = factRoles[0];
							if (oppositeBaseRole.Role == role)
							{
								oppositeBaseRole = factRoles[1];
							}
							Role oppositeRole = oppositeBaseRole.Role;
							// Unspecified checks the opposite role before saying unspecified, no need to look
							if (!wasUnspecified)
							{
								foreach (ConstraintRoleSequence roleSequence in oppositeRole.ConstraintRoleSequenceCollection)
								{
									IConstraint constraint = roleSequence.Constraint;
									if (constraint.ConstraintType == ConstraintType.InternalUniqueness && constraint.Modality == ConstraintModality.Alethic)
									{
										oppositeHasUnique = true;
										Debug.Assert(roleSequence.RoleCollection.Count == 1);
									}
								}
							}
							if (oppositeHasUnique)
							{
								if (existingDeonticSpanningConstraint != null)
								{
									existingDeonticSpanningConstraint.Delete();
								}
							}
							else
							{
								if (existingDeonticSpanningConstraint != null)
								{
									existingDeonticSpanningConstraint.Modality = ConstraintModality.Alethic;
								}
								else
								{
									// Now create a new uniqueness constraint containing both roles
									LinkedElementCollection<Role> constraintRoles = UniquenessConstraint.CreateInternalUniquenessConstraint(factType).RoleCollection;
									constraintRoles.Add(role);
									constraintRoles.Add(oppositeRole);
								}
							}
						}
					}
				}
			}
			#endregion // Handle Multiplicity property changes
			#region Handle MandatoryConstraintModality property changes
			else if (attributeGuid == Role.MandatoryConstraintModalityDomainPropertyId)
			{
				Role role = e.ModelElement as Role;
				MandatoryConstraint smc = role.SimpleMandatoryConstraint;
				if (smc != null)
				{
					smc.Modality = (ConstraintModality)e.NewValue;
				}
			}
			#endregion // Handle MandatoryConstraintModality property changes
			#region Handle ObjectificationOppositeRoleName property changes
			else if (attributeGuid == Role.ObjectificationOppositeRoleNameDomainPropertyId)
			{
				RoleProxy roleProxy = (e.ModelElement as Role).Proxy;
				Role oppositeRole;
				if (roleProxy != null && (oppositeRole = roleProxy.OppositeRole as Role) != null)
				{
					oppositeRole.Name = (string)e.NewValue;
				}
			}
			#endregion // Handle ObjectificationOppositeRoleName property changes
		}
		#endregion // RoleChangeRule
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Implements IModelErrorOwner.GetErrorCollection
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			if (filter == 0)
			{
				filter = (ModelErrorUses)(-1);
			}
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
			{
				RolePlayerRequiredError requiredError;
				if (null != (requiredError = RolePlayerRequiredError))
				{
					yield return requiredError;
				}
				foreach (ConstraintRoleSequence sequence in ConstraintRoleSequenceCollection)
				{
					MandatoryConstraint mandatoryConstraint = sequence as MandatoryConstraint;
					if (mandatoryConstraint != null)
					{
						foreach (PopulationMandatoryError populationMandatoryError in mandatoryConstraint.PopulationMandatoryErrorCollection)
						{
							yield return populationMandatoryError;
						}
					}
				}
			}
			if (filter == (ModelErrorUses)(-1))
			{
				ReadOnlyCollection<RoleInstance> roleInstances = RoleInstance.GetLinksToObjectTypeInstanceCollection(this);
				int count = roleInstances.Count;
				for (int i = 0; i < count; ++i)
				{
					PopulationUniquenessError populationError = roleInstances[i].PopulationUniquenessError;
					if (populationError != null)
					{
						yield return populationError;
					}
				}

				ReadOnlyCollection<DerivedRoleProjection> projections = DerivedRoleProjection.GetLinksToDerivationProjectionCollection(this);
				count = projections.Count;
				for (int i = 0; i < count; ++i)
				{
					DerivedRoleRequiresCompatibleProjectionError compatibilityError = projections[i].IncompatibleProjectionError;
					if (compatibilityError != null)
					{
						yield return compatibilityError;
					}
				}
			}
			// Get errors off the base
			foreach (ModelErrorUsage baseError in base.GetErrorCollection(filter))
			{
				yield return baseError;
			}
		}
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			return GetErrorCollection(filter);
		}
		/// <summary>
		/// Implements IModelErrorOwner.ValidateErrors
		/// Validate all errors on the external constraint. This
		/// is called during deserialization fixup when rules are
		/// suspended.
		/// </summary>
		/// <param name="notifyAdded">A callback for notifying
		/// the caller of all objects that are added.</param>
		protected new void ValidateErrors(INotifyElementAdded notifyAdded)
		{
			// Calls added here need corresponding delayed calls in DelayValidateErrors
			VerifyRolePlayerRequiredForRule(notifyAdded);
			ValidatePopulationUniquenessError(notifyAdded);
		}
		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateErrors(notifyAdded);
		}
		/// <summary>
		/// Implements IModelErrorOwner.DelayValidateErrors
		/// </summary>
		protected new void DelayValidateErrors()
		{
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateRolePlayerRequiredError);
			FrameworkDomainModel.DelayValidateElement(this, DelayValidatePopulationUniquenessError);
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner Implementation
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] {
					FactTypeHasRole.RoleDomainRoleId,
					ConstraintRoleSequenceHasRole.RoleDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
		#region RolePlayer validation rules
		/// <summary>
		/// AddRule: typeof(ObjectTypePlaysRole), FireTime=LocalCommit, Priority=ORMCoreDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void RolePlayerRequiredAddRule(ElementAddedEventArgs e)
		{
			ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
			link.PlayedRole.VerifyRolePlayerRequiredForRule(null);
		}
		/// <summary>
		/// DeleteRule: typeof(ObjectTypePlaysRole)
		/// </summary>
		private static void RolePlayerRequiredDeleteRule(ElementDeletedEventArgs e)
		{
			ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
			Role role = link.PlayedRole;
			if (!role.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(role, DelayValidateRolePlayerRequiredError);
			}
		}
		/// <summary>
		/// AddRule: typeof(FactTypeHasRole)
		/// Verify that the role has a role player attached to it, and
		/// renumber other role player required error messages when roles are added
		/// and removed.
		/// </summary>
		private static void RolePlayerRequiredForNewRoleAddRule(ElementAddedEventArgs e)
		{
			FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
			Role addedRole = link.Role as Role;
			if (addedRole != null)
			{
				FrameworkDomainModel.DelayValidateElement(addedRole, DelayValidateRolePlayerRequiredError);
				FrameworkDomainModel.DelayValidateElement(addedRole, DelayRenumberErrorsWithRoleNumbersAfterRole);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(FactTypeHasRole)
		/// </summary>
		private static void UpdatedRolePlayerRequiredErrorsDeleteRule(ElementDeletedEventArgs e)
		{
			FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
			FactType factType = link.FactType;
			if (!factType.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(factType, DelayRenumberErrorsWithRoleNumbers);
			}
		}
		private static void DelayRenumberErrorsWithRoleNumbers(ModelElement element)
		{
			RenumberErrorsWithRoleNumbers((FactType)element, null);
		}
		private static void DelayRenumberErrorsWithRoleNumbersAfterRole(ModelElement element)
		{
			Role role = (Role)element;
			FactType fact;
			if (!role.IsDeleted &&
				(null != (fact = role.FactType)))
			{
				RenumberErrorsWithRoleNumbers(fact, role);
			}
		}
		/// <summary>
		/// The error message for role player required events includes the role number.
		/// If a role is added or deleted, then this numbering can change, so we need to
		/// regenerated the text.
		/// </summary>
		/// <param name="factType">The owning factType</param>
		/// <param name="roleAdded">The added role, or null if a role was removed.</param>
		private static void RenumberErrorsWithRoleNumbers(FactType factType, RoleBase roleAdded)
		{
			if (!factType.IsDeleted)
			{
				LinkedElementCollection<RoleBase> roles = factType.RoleCollection;
				bool regenerate = roleAdded == null;
				int roleCount = roles.Count;
				for (int i = 0; i < roleCount; ++i)
				{
					Role currentRole = roles[i] as Role;
					if (regenerate && currentRole != null)
					{
						RolePlayerRequiredError error = currentRole.RolePlayerRequiredError;
						if (error != null)
						{
							error.GenerateErrorText();
						}
						RoleValueConstraint valueConstraint = currentRole.ValueConstraint;
						if (valueConstraint != null)
						{
							foreach (ValueRange range in valueConstraint.ValueRangeCollection)
							{
								MinValueMismatchError minError = range.MinValueMismatchError;
								if (minError != null)
								{
									minError.GenerateErrorText();
								}
								MaxValueMismatchError maxError = range.MaxValueMismatchError;
								if (maxError != null)
								{
									maxError.GenerateErrorText();
								}
							}
							ValueRangeOverlapError rangeOverlap = valueConstraint.ValueRangeOverlapError;
							if (rangeOverlap != null)
							{
								rangeOverlap.GenerateErrorText();
							}
						}
					}
					else if (roleAdded == currentRole)
					{
						// Regenerate on the next pass
						regenerate = true;
					}
				}
			}
		}
		/// <summary>
		/// Delayed validator for RolePlayerRequiredError
		/// </summary>
		private static void DelayValidateRolePlayerRequiredError(ModelElement element)
		{
			(element as Role).VerifyRolePlayerRequiredForRule(null);
		}
		/// <summary>
		/// Utility function to verify that a role player is present for all roles
		/// </summary>
		private void VerifyRolePlayerRequiredForRule(INotifyElementAdded notifyAdded)
		{
			if (!IsDeleted)
			{
				bool hasRolePlayer = true;
				RolePlayerRequiredError rolePlayerRequired;

				if (null == RolePlayer)
				{
					FactType fact = FactType;
					// Don't show an error for roles on implied fact types,
					// this is controlled indirectly by the nested roles
					if (null != fact && null == fact.ImpliedByObjectification)
					{
						hasRolePlayer = false;
						if (null == RolePlayerRequiredError)
						{
							rolePlayerRequired = new RolePlayerRequiredError(Partition);
							rolePlayerRequired.Role = this;
							rolePlayerRequired.Model = fact.ResolvedModel;
							rolePlayerRequired.GenerateErrorText();
							if (notifyAdded != null)
							{
								notifyAdded.ElementAdded(rolePlayerRequired, true);
							}
						}
					}
				}
				if (hasRolePlayer)
				{
					if (null != (rolePlayerRequired = RolePlayerRequiredError))
					{
						rolePlayerRequired.Delete();
					}
				}
			}
		}
		#endregion // RolePlayer validation rules
		#region IRedirectVerbalization Implementation
		/// <summary>
		/// Implements IRedirectVerbalization.SurrogateVerbalizer by deferring to the parent fact
		/// </summary>
		protected IVerbalize SurrogateVerbalizer
		{
			get
			{
				return FactType as IVerbalize;
			}
		}
		IVerbalize IRedirectVerbalization.SurrogateVerbalizer
		{
			get
			{
				return SurrogateVerbalizer;
			}
		}
		#endregion // IRedirectVerbalization Implementation
		#region IVerbalizeCustomChildren Implementation
		/// <summary>
		/// Implements <see cref="IVerbalizeCustomChildren.GetCustomChildVerbalizations"/>
		/// Returns standalone error verbalizer.
		/// </summary>
		protected IEnumerable<CustomChildVerbalizer> GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, IDictionary<string, object> verbalizationOptions, VerbalizationSign sign)
		{
			ErrorReport verbalizer = ErrorReport.GetVerbalizer();
			verbalizer.Initialize(this);
			yield return CustomChildVerbalizer.VerbalizeInstance(verbalizer, true);
		}
		IEnumerable<CustomChildVerbalizer> IVerbalizeCustomChildren.GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, IDictionary<string, object> verbalizationOptions, VerbalizationSign sign)
		{
			return GetCustomChildVerbalizations(filter, verbalizationOptions, sign);
		}
		#region ErrorReport verbalizer class
		partial class ErrorReport : IModelErrorOwner
		{
			private Role myRole;
			public void Initialize(Role role)
			{
				myRole = role;
			}
			public void DisposeHelper()
			{
				myRole = null;
			}
			#region IModelErrorOwner Implementation
			IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
			{
				return ((IModelErrorOwner)myRole).GetErrorCollection(filter);
			}
			void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
			{
			}
			void IModelErrorOwner.DelayValidateErrors()
			{
			}
			#endregion // IModelErrorOwner Implementation
			#region Equality Overrides
			// Override equality operators so that muliple uses of the verbalization helper
			// for this object with different values does not trigger an 'already verbalized'
			// response for later verbalizations.
			/// <summary>
			/// Standard equality override
			/// </summary>
			public override int GetHashCode()
			{
				// Combine the role has code with a nother value so we don't get in a hash
				// bucket with the role itself.
				return Utility.GetCombinedHashCode(myRole != null ? myRole.GetHashCode() : 0, GetType().GetHashCode());
			}
			/// <summary>
			/// Standard equality override
			/// </summary>
			public override bool Equals(object obj)
			{
				ErrorReport other;
				return (null != (other = obj as ErrorReport)) && other.myRole == myRole;
			}
			#endregion // Equality Overrides
		}
		#endregion // ErrorReport verbalizer class
		#endregion // IVerbalizeCustomChildren Implementation
		#region INamedElementDictionaryParent implementation
		INamedElementDictionary INamedElementDictionaryParent.GetCounterpartRoleDictionary(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			return GetCounterpartRoleDictionary(parentDomainRoleId, childDomainRoleId);
		}
		/// <summary>
		/// Implements INamedElementDictionaryParent.GetCounterpartRoleDictionary
		/// </summary>
		/// <param name="parentDomainRoleId">Guid</param>
		/// <param name="childDomainRoleId">Guid</param>
		/// <returns>Model-owned dictionary for constraints</returns>
		protected INamedElementDictionary GetCounterpartRoleDictionary(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			INamedElementDictionary dictionary = null;
			bool forValueConstraint;
			if ((forValueConstraint = (parentDomainRoleId == RoleHasValueConstraint.RoleDomainRoleId)) ||
				parentDomainRoleId == UnaryRoleHasCardinalityConstraint.UnaryRoleDomainRoleId)
			{
				FactType factType;
				if (null != (factType = FactType))
				{
					// If the object type has an alternate owner with a dictionary, then see if that
					// owner has a dictionary that supports this relationship. Otherwise just use
					// dictionary from the model.
					IHasAlternateOwner<FactType> toAlternateOwner;
					INamedElementDictionaryParent dictionaryParent;
					ORMModel model;
					if ((null == (toAlternateOwner = factType as IHasAlternateOwner<FactType>) ||
						null == (dictionaryParent = toAlternateOwner.AlternateOwner as INamedElementDictionaryParent) ||
						null == (dictionary = dictionaryParent.GetCounterpartRoleDictionary(parentDomainRoleId, childDomainRoleId))) &&
						null != (model = factType.Model))
					{
						dictionary = ((INamedElementDictionaryParent)model).GetCounterpartRoleDictionary(parentDomainRoleId, childDomainRoleId);
					}
				}
				if (dictionary == null)
				{
					dictionary = NamedElementDictionary.GetRemoteDictionaryToken(forValueConstraint? typeof(ValueConstraint) : typeof(CardinalityConstraint));
				}
			}
			return dictionary;
		}
		/// <summary>
		/// Implements INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey
		/// </summary>
		protected object GetAllowDuplicateNamesContextKey(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			object retVal = null;
			Dictionary<object, object> contextInfo = Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
			if (!contextInfo.ContainsKey(NamedElementDictionary.DefaultAllowDuplicateNamesKey) &&
				contextInfo.ContainsKey(ORMModel.AllowDuplicateNamesKey))
			{
				// Use their value so they don't have to look up ours again
				retVal = NamedElementDictionary.AllowDuplicateNamesKey;
			}
			return retVal;
		}
		object INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			return GetAllowDuplicateNamesContextKey(parentDomainRoleId, childDomainRoleId);
		}
		#endregion // INamedElementDictionaryParent implementation
		#region INamedElementDictionaryRemoteChild implementation
		private static readonly Guid[] myRemoteNamedElementDictionaryChildRoles = new Guid[] { RoleHasValueConstraint.RoleDomainRoleId };
		/// <summary>
		/// Implements <see cref="INamedElementDictionaryRemoteChild.GetNamedElementDictionaryChildRoles"/>. Identifies
		/// this as a remote parent for the 'ModelHasConstraint' naming set.
		/// </summary>
		/// <returns>Guid for the ValueTypeHasValueConstraint.ValueType role</returns>
		protected static Guid[] GetNamedElementDictionaryChildRoles()
		{
			return myRemoteNamedElementDictionaryChildRoles;
		}
		Guid[] INamedElementDictionaryRemoteChild.GetNamedElementDictionaryChildRoles()
		{
			return GetNamedElementDictionaryChildRoles();
		}
		/// <summary>
		/// Implements <see cref="INamedElementDictionaryRemoteChild.NamedElementDictionaryParentRole"/>
		/// </summary>
		protected static Guid NamedElementDictionaryParentRole
		{
			get
			{
				return FactTypeHasRole.FactTypeDomainRoleId;
			}
		}
		Guid INamedElementDictionaryRemoteChild.NamedElementDictionaryParentRole
		{
			get
			{
				return NamedElementDictionaryParentRole;
			}
		}
		#endregion // INamedElementDictionaryRemoteChild implementation
		#region IHierarchyContextEnabled Members
		/// <summary>
		/// Gets the model that the current <see cref="T:ORMModel"/> that the <see cref="T:ModelElement"/> is related to.
		/// </summary>
		/// <value>The model.</value>
		protected ORMModel Model
		{
			get
			{
				FactType factType = this.FactType;
				return (factType != null) ? factType.ResolvedModel : null;
			}
		}
		ORMModel IHierarchyContextEnabled.Model
		{
			get { return Model; }
		}
		/// <summary>
		/// Gets the FactType that this instance resolves to.
		/// </summary>
		/// <value>The FactType</value>
		protected IHierarchyContextEnabled ForwardHierarchyContextTo
		{
			get { return this.FactType; }
		}
		IHierarchyContextEnabled IHierarchyContextEnabled.ForwardHierarchyContextTo
		{
			get { return ForwardHierarchyContextTo; }
		}
		/// <summary>
		/// Implements <see cref="IHierarchyContextEnabled.GetForcedHierarchyContextElements"/>
		/// </summary>
		protected static IEnumerable<IHierarchyContextEnabled> GetForcedHierarchyContextElements(bool minimalElements)
		{
			return null;
		}
		IEnumerable<IHierarchyContextEnabled> IHierarchyContextEnabled.GetForcedHierarchyContextElements(bool minimalElements)
		{
			return GetForcedHierarchyContextElements(minimalElements);
		}
		/// <summary>
		/// Gets the place priority. The place priority specifies the order in which the element will
		/// be placed on the context diagram.
		/// </summary>
		/// <value>The place priority.</value>
		protected static HierarchyContextPlacementPriority HierarchyContextPlacementPriority
		{
			get { return HierarchyContextPlacementPriority.Low; }
		}
		HierarchyContextPlacementPriority IHierarchyContextEnabled.HierarchyContextPlacementPriority
		{
			get { return HierarchyContextPlacementPriority; }
		}
		/// <summary>
		/// Gets the number of generations to decriment when this object is walked.
		/// </summary>
		/// <value>The number of generations.</value>
		protected static int HierarchyContextDecrementCount
		{
			get { return 0; }
		}
		int IHierarchyContextEnabled.HierarchyContextDecrementCount
		{
			get { return HierarchyContextDecrementCount; }
		}
		/// <summary>
		/// Gets a value indicating whether the path through the diagram should be followed through
		/// this element.
		/// </summary>
		/// <value><c>true</c> to continue walking; otherwise, <c>false</c>.</value>
		protected static bool ContinueWalkingHierarchyContext
		{
			get { return true; }
		}
		bool IHierarchyContextEnabled.ContinueWalkingHierarchyContext
		{
			get { return ContinueWalkingHierarchyContext; }
		}
		#endregion
	}
	partial class RoleBase : IModelErrorDisplayContext
	{
		#region Accessor properties
		/// <summary>
		/// Convert a RoleBase to a Role, resolving a proxy as needed
		/// </summary>
		public Role Role
		{
			get
			{
				Role retVal = this as Role;
				if (retVal == null)
				{
					RoleProxy proxy;
					if (null != (proxy = this as RoleProxy))
					{
						retVal = proxy.TargetRole;
					}
				}
				return retVal;
			}
		}
		/// <summary>
		/// Used as a shortcut to find the the binarized version of the FactType that this
		/// role belongs to.
		/// </summary>
		public FactType BinarizedFactType
		{
			get
			{
				RoleProxy proxy;
				Role role = this as Role;
				if (role != null)
				{
					proxy = role.Proxy;
					if (proxy != null)
					{
						return proxy.FactType;
					}
					return role.FactType;
				}
				else
				{
					proxy = this as RoleProxy;
					Debug.Assert(proxy != null);
					return proxy.FactType;
				}
			}
		}
		/// <summary>
		/// Used as a shortcut to find the opposite RoleBase in a binary FactType.
		/// Returns null if the FactType is not binary.
		/// </summary>
		public RoleBase OppositeRole
		{
			get
			{
				// Only do this if it's a binary fact
				FactType factType = this.FactType;
				if (factType != null)
				{
					LinkedElementCollection<RoleBase> roles = factType.RoleCollection;
					if (roles.Count == 2)
					{
						// loop over the collection and get the other role
						RoleBase oppositeRole = roles[0];
						if (oppositeRole == this)
						{
							return roles[1];
						}
						return oppositeRole;
					}
				}
				return null;
			}
		}
		/// <summary>
		/// Get the opposite role of any role. All roles are involved either
		/// in a binary fact or have a proxy role in a binary fact. If a role
		/// has two opposite roles, then this method will choose the role
		/// opposite the proxy. Use <see cref="OppositeRoleResolveProxy"/>
		/// to choose the opposite role on the non-implied binary fact.
		/// </summary>
		public RoleBase OppositeRoleAlwaysResolveProxy
		{
			get
			{
				Role nativeRole = this as Role;
				if (nativeRole != null)
				{
					RoleProxy proxy = nativeRole.Proxy;
					return (proxy != null) ? proxy.OppositeRole : nativeRole.OppositeRole;
				}
				return OppositeRole;
			}
		}
		/// <summary>
		/// Get the opposite role of any role. All roles are involved either
		/// in a binary fact or have a proxy role in a binary fact. If a role
		/// has two opposite roles, then this method will choose the role
		/// on the non-implied binary fact. Use <see cref="OppositeRoleAlwaysResolveProxy"/>
		/// to choose the opposite role on the implied binary fact.
		/// </summary>
		public RoleBase OppositeRoleResolveProxy
		{
			get
			{
				RoleBase nativeOpposite = OppositeRole;
				if (nativeOpposite != null)
				{
					return nativeOpposite;
				}
				Debug.Assert(!(this is RoleProxy), "A proxy role always has a native opposite role");
				RoleProxy proxy = ((Role)this).Proxy;
				return (proxy != null) ? proxy.OppositeRole : null;

			}
		}
		#endregion // Accessor properties
		#region IModelErrorDisplayContext Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorDisplayContext.ErrorDisplayContext"/>
		/// </summary>
		protected string ErrorDisplayContext
		{
			get
			{
				int roleNumber = 0;
				string contextName = null;
				FactType factType = FactType;
				if (factType != null)
				{
					roleNumber = factType.RoleCollection.IndexOf(this) + 1;
					contextName = ((IModelErrorDisplayContext)factType).ErrorDisplayContext;
				}
				return string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorDisplayContextFactTypeRole, contextName ?? "", roleNumber);
			}
		}
		string IModelErrorDisplayContext.ErrorDisplayContext
		{
			get
			{
				return ErrorDisplayContext;
			}
		}
		#endregion // IModelErrorDisplayContext Implementation
	}
	[ModelErrorDisplayFilter(typeof(FactTypeDefinitionErrorCategory))]
	partial class RolePlayerRequiredError
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext context = (IModelErrorDisplayContext)Role;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorRolePlayerRequiredError, context != null ? (context.ErrorDisplayContext ?? "") : ""));
		}
		/// <summary>
		/// Provide a compact error description
		/// </summary>
		public override string CompactErrorText
		{
			get
			{
				return ResourceStrings.ModelErrorRolePlayerRequiredErrorCompact;
			}
		}
		/// <summary>
		/// Regenerate the error text when the constraint name changes
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.OwnerNameChange | RegenerateErrorTextEvents.ModelNameChange;
			}
		}
		#endregion // Base overrides
	}
}
