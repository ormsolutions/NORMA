using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.Modeling;
using System.Diagnostics;
using System.Collections;
using Neumont.Tools.ORMAbstraction;

namespace Neumont.Tools.ORMToORMAbstractionBridge
{
	public partial class FactTypeMapsTowardsRole
	{
		#region Constructors and static Create methods
		/// <summary>
		/// Creates a FactTypeMapsTowardsRole link in the same Partition as the given FactType
		/// </summary>
		/// <param name="source">FactType to use as the source of the relationship.</param>
		/// <param name="target">RoleBase to use as the target of the relationship.</param>
		/// <param name="depth">Initial value for the <see cref="Depth"/> property.</param>
		public static FactTypeMapsTowardsRole Create(FactType source, RoleBase target, MappingDepth depth)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			MappingMandatoryPattern mandatoryPattern;
			MappingUniquenessPattern uniquenessPattern;
			GetMappingPatterns(target, out uniquenessPattern, out mandatoryPattern);
			return new FactTypeMapsTowardsRole(
				source.Partition,
				new RoleAssignment[] { new RoleAssignment(FactTypeMapsTowardsRole.FactTypeDomainRoleId, source), new RoleAssignment(FactTypeMapsTowardsRole.TowardsRoleDomainRoleId, target) },
				new PropertyAssignment[] { new PropertyAssignment(DepthDomainPropertyId, depth), new PropertyAssignment(UniquenessPatternDomainPropertyId, uniquenessPattern), new PropertyAssignment(MandatoryPatternDomainPropertyId, mandatoryPattern)});
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new link is to be created.</param>
		/// <param name="roleAssignments">List of relationship role assignments.</param>
		/// <param name="propertyAssignments">List of properties assignments to set on the new link.</param>
		public FactTypeMapsTowardsRole(Partition partition, RoleAssignment[] roleAssignments, PropertyAssignment[] propertyAssignments)
			: base(partition, roleAssignments, propertyAssignments)
		{
		}
		#endregion // Constructors and static Create methods
		#region Mapping pattern methods
		/// <summary>
		/// Update the <see cref="MandatoryPattern"/> and <see cref="UniquenessPattern"/> if the current constraints
		/// on the associated <see cref="FactType"/> cannot change the <see cref="Depth"/> or direction of the mapping.
		/// If the current mapping can be influenced, then do nothing and return <see langword="false"/>
		/// </summary>
		/// <returns><see langword="true"/> if the current set of constraints on the FactType cannot change the current mapping depth or direction.</returns>
		public bool SynchronizeMappingPatterns()
		{
			bool retVal = false;
			MappingUniquenessPattern newUniquenessPattern;
			MappingMandatoryPattern newMandatoryPattern;
			GetMappingPatterns(TowardsRole, out newUniquenessPattern, out newMandatoryPattern);
			MappingUniquenessPattern oldUniquenessPattern = UniquenessPattern;
			if (newUniquenessPattern == oldUniquenessPattern)
			{
				MappingMandatoryPattern oldMandatoryPattern = MandatoryPattern;
				if (newMandatoryPattern == oldMandatoryPattern)
				{
					retVal = true;
				}
				else
				{
					switch (oldUniquenessPattern)
					{
						case MappingUniquenessPattern.OneToMany:
							// Mandatory constraints to not affect one-to-many mapping
							retVal = true;
							break;
						case MappingUniquenessPattern.OneToOne:
							// UNDONE: The answers here assume that a reduced set of
							// choices will result in the same answer. This is a conjecture
							// at this point that is likely to be true, but we're not sure.
							switch (oldMandatoryPattern)
							{
								case MappingMandatoryPattern.BothRolesMandatory:
									switch (newMandatoryPattern)
									{
										case MappingMandatoryPattern.TowardsRoleMandatory:
										case MappingMandatoryPattern.NotMandatory:
											retVal = Depth == MappingDepth.Shallow;
											break;
										case MappingMandatoryPattern.OppositeRoleMandatory:
											retVal = true;
											break;
									}
									break;
								case MappingMandatoryPattern.OppositeRoleMandatory:
								case MappingMandatoryPattern.TowardsRoleMandatory:
									retVal = newMandatoryPattern == MappingMandatoryPattern.NotMandatory && Depth == MappingDepth.Shallow;
									break;
							}
							break;
					}
					if (retVal)
					{
						MandatoryPattern = newMandatoryPattern;
					}
				}
			}
			else if (oldUniquenessPattern == MappingUniquenessPattern.OneToOne &&
				newUniquenessPattern == MappingUniquenessPattern.OneToMany &&
				Depth == MappingDepth.Shallow)
			{
				// Shallow towards current TowardsRole is the only remaining possibility, nothing will change
				retVal = true;
				MandatoryPattern = newMandatoryPattern;
				UniquenessPattern = newUniquenessPattern;
			}
			return retVal;
		}
		private static void GetMappingPatterns(RoleBase towardsRole, out MappingUniquenessPattern uniquenessPattern, out MappingMandatoryPattern mandatoryPattern)
		{
			Role role = towardsRole.Role;
			GetMappingPatterns(role, role.OppositeRoleAlwaysResolveProxy.Role, out uniquenessPattern, out mandatoryPattern);
		}
		private static void GetMappingPatterns(Role towardsRole, Role fromRole, out MappingUniquenessPattern uniquenessPattern, out MappingMandatoryPattern mandatoryPattern)
		{
			if (towardsRole.FactType is SubtypeFact)
			{
				uniquenessPattern = MappingUniquenessPattern.Subtype;
				mandatoryPattern = (towardsRole is SubtypeMetaRole) ? MappingMandatoryPattern.TowardsRoleMandatory : MappingMandatoryPattern.OppositeRoleMandatory;
			}
			else
			{
				bool towardsRoleUnique;
				bool towardsRoleMandatory;
				bool fromRoleUnique;
				bool fromRoleMandatory;
				GetUniqueAndMandatory(towardsRole, out towardsRoleUnique, out towardsRoleMandatory);
				GetUniqueAndMandatory(fromRole, out fromRoleUnique, out fromRoleMandatory);
				uniquenessPattern = towardsRoleUnique ?
					(fromRoleUnique ? MappingUniquenessPattern.OneToOne : MappingUniquenessPattern.OneToMany) :
					MappingUniquenessPattern.ManyToOne;
				mandatoryPattern = towardsRoleMandatory ?
					(fromRoleMandatory ? MappingMandatoryPattern.BothRolesMandatory : MappingMandatoryPattern.TowardsRoleMandatory) :
					(fromRoleMandatory ? MappingMandatoryPattern.OppositeRoleMandatory : MappingMandatoryPattern.NotMandatory);
			}
		}
		private static void GetUniqueAndMandatory(Role role, out bool hasUniqueness, out bool hasMandatory)
		{
			hasUniqueness = false;
			hasMandatory = false;
			LinkedElementCollection<ConstraintRoleSequence> constraintRoleSequences = role.ConstraintRoleSequenceCollection;
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
							hasMandatory = true;
							if (hasUniqueness)
							{
								return;
							}
							break;
						case ConstraintType.InternalUniqueness:
							// Ignore spanning internal constraints, these are treated as external
							// uniquenesses in the binarized form
							if (roleSequence.RoleCollection.Count == 1)
							{
								hasUniqueness = true;
								if (hasMandatory)
								{
									return;
								}
							}
							break;
						case ConstraintType.ImpliedMandatory:
							if (roleSequence.RoleCollection.Count == 1)
							{
								hasMandatory = true;
								if (hasUniqueness)
								{
									return;
								}
							}
							break;
					}
				}
			}
		}
		#endregion // Mapping pattern methods
	}
}