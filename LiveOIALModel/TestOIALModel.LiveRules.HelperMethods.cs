#region Copyright Notice
// Common Public License Copyright Notice
// /**************************************************************************\
// * Neumont Object-Role Modeling Architect for Visual Studio                 *
// *                                                                          *
// * Copyright © Neumont University. All rights reserved.                     *
// *                                                                          *
// * The use and distribution terms for this software are covered by the      *
// * Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
// * can be found in the file CPL.txt at the root of this distribution.       *
// * By using this software in any fashion, you are agreeing to be bound by   *
// * the terms of this license.                                               *
// *                                                                          *
// * You must not remove this notice, or any other, from this software.       *
// \**************************************************************************/
#endregion // Copyright Notice

using System;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Neumont.Tools.ORM.TestOIALModel
{
	public partial class LiveOIALModel
	{
		#region Diagnostic Information For ORM Model Elements
		/// <summary>
		/// Gets the number of alethic internal constraints of the specified constraint type.
		/// </summary>
		/// <param name="factType">The <see cref="T:Neumont.Tools.ORM.ObjectModel.FactType"/> for which alethic internal constraints
		/// should be counted.</param>
		/// <param name="constraintType">The type of constraint to count.</param>
		/// <returns>The number of alethic internal constraints of the specified constraint type.</returns>
		private static int GetAlethicInternalConstraintsCount(FactType factType, ConstraintType constraintType)
		{
			LinkedElementCollection<SetConstraint> factSetConstraints = factType.SetConstraintCollection;
			int factSetConstraintCount = factSetConstraints.Count;
			int retVal = 0;

			for (int i = 0; i < factSetConstraintCount; ++i)
			{
				IConstraint constraint = (IConstraint)factSetConstraints[i];
				if (constraint.ConstraintType == constraintType && constraint.Modality == ConstraintModality.Alethic && constraint.ConstraintIsInternal)
				{
					++retVal;
				}
			}
			return retVal;
		}
		/// <summary>
		/// Gets a collection which can iterated over of <see cref="FactType"/> objects for
		/// a specific <see cref="ObjectType"/>.
		/// </summary>
		/// <param name="objectTypeRoleCollection">The PlayedRoleCollection of the <see cref="ObjectType"/> of interest</param>
		/// <param name="startingRole">If the <see cref="ObjectType"/> of interest has a specific <see cref="Role"/> whose
		/// <see cref="FactType"/> should not be checked, pass it here. Otherwise, pass null.</param>
		/// <returns>IEnumerable of <see cref="FactType"/> objects</returns>
		private static IEnumerable<FactType> GetFunctionalDependentRoles(LinkedElementCollection<Role> objectTypeRoleCollection, Role startingRole)
		{
			foreach (Role role in objectTypeRoleCollection)
			{
				// If null is passed for the starting role, then although this check will be executed, it will
				// never reach the continue statement. Also we do not want to interpret any fact types that are
				// not binarized.
				FactType roleFactType = role.FactType;
				if (role.Equals(startingRole) || roleFactType.Objectification != null || roleFactType.RoleCollection.Count != 2 || (roleFactType.DerivationStorageDisplay == DerivationStorageType.Derived && !string.IsNullOrEmpty(roleFactType.DerivationRuleDisplay)))
				{
					continue;
				}
				// If it is a functional role
				Role oppositeRole = role.OppositeRole.Role;					// CHANGE: Role to RoleBase
				RoleMultiplicity roleMultiplicity = oppositeRole.Multiplicity;

				if (roleMultiplicity == RoleMultiplicity.ZeroToOne ||
					roleMultiplicity == RoleMultiplicity.ExactlyOne)
				{
					// If there are no constraints in the role-sequence collection, then it is a
					// functional role. If there are constraints but no uniqueness constraints, it
					// is a functional role. If there are constraints and one is an internal uniqueness
					// constraint and it is not primary, then it is a functional role. If there are
					// constraints and one is an external uniqueness constraint and it is not primary,
					// then it is a functional role. If there are constraints and one is an internal
					// uniqueness constraint and another is an external uniqueness constraint and either
					// one is primary, then it is not a functional role.
					bool isPreferredIdentifier = false;
					bool isDependent = false;
					foreach (ConstraintRoleSequence constraintRoleSequence in oppositeRole.ConstraintRoleSequenceCollection)
					{
						IConstraint constraint = constraintRoleSequence.Constraint;
						ConstraintType constraintType = constraint.ConstraintType;
						if (constraintType == ConstraintType.InternalUniqueness ||
							constraintType == ConstraintType.ExternalUniqueness)
						{
							if (constraint.PreferredIdentifierFor != null)
							{
								isPreferredIdentifier = true;
								break;
							}
							else
							{
								isDependent = true;
								break;
							}
						}
					}
					if (!isPreferredIdentifier && isDependent)
					{
						yield return roleFactType;
					}
				}
			}
		}
		/// <summary>
		/// Gets the mandatory constraint modality of a Role in a way parsable by the OIALModel.
		/// </summary>
		/// <param name="role">The <see cref="T:Neumont.Tools.ORM.ObjectModel.Role"/> whose mandatory constraint (or lack thereof) will
		/// be evaluated.</param>
		/// <returns>The mandatory constraint modality of a Role in a way parsable by the OIALModel.</returns>
		private static MandatoryConstraintModality GetMandatoryModality(Role role)
		{
			MandatoryConstraintModality retVal = MandatoryConstraintModality.NotMandatory;
			if (role.IsMandatory)
			{
				switch (role.MandatoryConstraintModality)
				{
					case ConstraintModality.Alethic:
						retVal = MandatoryConstraintModality.Alethic;
						break;
					case ConstraintModality.Deontic:
						retVal = MandatoryConstraintModality.Deontic;
						break;
				}
			}
			return retVal;
		}
		/// <summary>
		/// Resolves the name on a role for a <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptTypeChild"/>. If the role
		/// has a name, that name is returned; otherwise, that Role's role player's name is returned.
		/// </summary>
		/// <param name="role">The <see cref="T:Neumont.Tools.ORM.ObjectModel.RoleBase"/> for which a name should be resolved.</param>
		/// <returns>The name of the <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptTypeChild"/>.</returns>
		private static string GetNameForConceptTypeChild(Role role)
		{
			string retVal = role.Name;
			if (string.IsNullOrEmpty(retVal))
			{
				retVal = role.RolePlayer.Name;
			}
			return retVal;
		}
		/// <summary>
		/// Determines whether the specified <see cref="T:Neumont.Tools.ORM.ObjectModel.ObjectType"/> is a Concept Type.
		/// </summary>
		/// <param name="oialModel">The <see cref="T:Neumont.Tools.ORM.OIALModel.OIALModel"/> to which Fact Type Absorptions are generated.</param>
		/// <param name="objectType">The <see cref="T:Neumont.Tools.ORM.ObjectModel.ObjectType"/> to check.</param>
		/// <param name="currentFactType">The fact type that should not count when considering concept type absorptions.</param>
		/// <returns><see langword="true"/> if the Object Type is a Concept Type. Otherwise, <see langword="false" />.</returns>
		private static bool IsConceptType(LiveOIALModel oialModel, ObjectType objectType, FactType currentFactType)
		{
			IEnumerable<FactType> factTypes = GetFunctionalRoles(objectType.PlayedRoleCollection, null);
			Guid objectTypeId = objectType.Id;
			foreach (FactType factType in factTypes)
			{
				if (factType == currentFactType)
				{
					continue;
				}

				// This code assumes that all one-to-one fact types have fact type absorptions recorded
				AbsorbedFactType oldAbsorption;
				if (oialModel.myAbsorbedFactTypes.TryGetValue(factType.Id, out oldAbsorption) &&
					(oldAbsorption.AbsorberId == objectTypeId || oldAbsorption.AbsorptionType == FactAbsorptionType.Fully))
				{
					return true;
				}
			}

			return false;
		}
		#endregion // Diagnostic Information For ORM Model Elements
		#region One To One Absorption Algorithms
		/// <summary>
		/// Checks whether the fact type is a preferred-identifier fact. If it is, does the appropriate absorption.
		/// </summary>
		/// <param name="preferredIdentifierConstraint">The <see cref="T:Neumont.Tools.ORM.ObjectModel.UniquenessConstraint"/> that
		/// represents the preferred identifier of one of the two role players.
		/// </param>
		/// <param name="oialModel">The <see cref="T:Neumont.Tools.ORM.OIALModel.LiveOIALModel"/> in which this processing takes place.</param>
		/// <param name="store">The current <see cref="T:Microsoft.VisualStudio.Modeling.Store"/> of which this model is a part.</param>
		/// <param name="factType">The <see cref="T:Neumont.Tools.ORM.ObjectModel.FactType"/> that represents the fact type that was changed.</param>
		/// <returns>The ID of the absorbing <see cref="T:Neumont.Tools.ORM.ObjectModel.ObjectType"/>, if any.</returns>
		private static Guid DoOneToOnePreferredIdentifierAbsorption(UniquenessConstraint preferredIdentifierConstraint,
			LiveOIALModel oialModel, Store store, FactType factType)
		{
			Guid absorberId = Guid.Empty;

			LinkedElementCollection<Role> preferredIdentifierRoleCollection = preferredIdentifierConstraint.RoleCollection;
			Role pathRole = preferredIdentifierRoleCollection[0];
			ObjectType identifiedObjectType = preferredIdentifierConstraint.PreferredIdentifierFor;
			ObjectType preferredIdentifierObjectType = pathRole.RolePlayer;

			ConceptType identifiedConceptType = ConceptTypeHasObjectType.GetConceptType(identifiedObjectType);
			ConceptType preferredIdentifierConceptType = ConceptTypeHasObjectType.GetConceptType(preferredIdentifierObjectType);

			if (preferredIdentifierRoleCollection.Count == 1 && pathRole.FactType == factType)
			{
				if (identifiedConceptType == null)
				{
					// If there isn't a concept type already for the opposite role player,
					// then delete all the concept type children and replace it with a concept type ref.
					identifiedConceptType = CreateConceptType(oialModel, store, identifiedObjectType, preferredIdentifierConceptType,
						pathRole);
				}

				if (preferredIdentifierConceptType == null)
				{
					preferredIdentifierConceptType = CreateConceptType(oialModel, store, preferredIdentifierObjectType, null, pathRole);
				}

				AbsorbConceptType(oialModel, identifiedObjectType, preferredIdentifierObjectType, MandatoryConstraintModality.Alethic, pathRole.OppositeRole.Role);

				absorberId = preferredIdentifierObjectType.Id;
			}

			return absorberId;
		}
		/// <summary>
		/// Calculates the absorption "winner" on a one-to-one fact type and modifies the OIAL model appropriately.
		/// </summary>
		/// <param name="rolePlayer">One role player of the one-to-one fact type.</param>
		/// <param name="oppositeRolePlayer">The other role player of the one-to-one fact type.</param>
		/// <param name="oldAbsorption">The old <see cref="T:Neumont.Tools.ORM.OIALModel.LiveOIALModel.FactTypeAbsorption"/> that represents
		/// the old absorption data for this fact type.</param>
		/// <param name="oialModel">The <see cref="T:Neumont.Tools.ORM.OIALModel.LiveOIALModel"/> in which this processing takes place.</param>
		/// <param name="store">The current <see cref="T:Microsoft.VisualStudio.Modeling.Store"/> of which this model is a part.</param>
		/// <param name="role">The <see cref="T:Neumont.Tools.ORM.ObjectModel.Role"/> that the <paramref name="rolePlayer"/> plays in
		/// this fact type.</param>
		/// <param name="oppositeRole">The <see cref="T:Neumont.Tools.ORM.ObjectModel.Role"/> that the <paramref name="oppositeRolePlayer"/> plays in
		/// this fact type.</param>
		/// <returns>The ID of the absorbing <see cref="T:Neumont.Tools.ORM.ObjectModel.ObjectType"/>, if any.</returns>
		private static Guid DoOneToOneAbsorptionForDoubleMandatories(ObjectType rolePlayer, ObjectType oppositeRolePlayer, AbsorbedFactType oldAbsorption,
			LiveOIALModel oialModel, Store store, Role role, Role oppositeRole)
		{
			// Not a preferred identifier fact. So just a normal one-to-one type fully absorption.
			ConceptType conceptType = ConceptTypeHasObjectType.GetConceptType(rolePlayer);
			ConceptType oppositeConceptType = ConceptTypeHasObjectType.GetConceptType(oppositeRolePlayer);

			Debug.Assert(conceptType != null || oppositeConceptType != null,
				"Both role players of a one-to-one fact type with two mandatory roles did not have associated Concept Types.");

			int firstRoleCount = GetFunctionalNonDependentRoleCount(rolePlayer.PlayedRoleCollection, null);
			int secondRoleCount = GetFunctionalNonDependentRoleCount(oppositeRolePlayer.PlayedRoleCollection, null);

			Guid absorberId = Guid.Empty;
			Guid oldAbsorberId = oldAbsorption.AbsorberId;

			ConceptType winningConceptType = null;
			ConceptType losingConceptType = null;
			Role pathRole = null;

			if (oldAbsorberId == rolePlayer.Id)
			{
				// ConceptType for rolePlayer should exist.
				Debug.Assert(conceptType != null, "Concept Type did not exist for absorber object type.");

				if (oppositeConceptType == null)
				{
					oppositeConceptType = CreateConceptType(oialModel, store, oppositeRolePlayer, conceptType);
				}
				winningConceptType = conceptType;
				losingConceptType = oppositeConceptType;
				pathRole = oppositeRole;
			}
			else if (oldAbsorberId == oppositeRolePlayer.Id)
			{
				// ConceptType for oppositeRolePlayer should exist.
				Debug.Assert(oppositeConceptType != null, "Concept Type did not exist for absorber object type.");

				if (conceptType == null)
				{
					conceptType = CreateConceptType(oialModel, store, rolePlayer, oppositeConceptType);
				}
				winningConceptType = oppositeConceptType;
				losingConceptType = conceptType;
				pathRole = role;
			}
			else
			{
				Debug.Fail("Absorber was not playing a role in this fact type.");
			}
			// Determine the absorption pattern
			if (firstRoleCount > secondRoleCount)
			{
				AbsorbConceptType(oialModel, oppositeRolePlayer, rolePlayer, MandatoryConstraintModality.Alethic, role);
				absorberId = rolePlayer.Id;
			}
			else if (firstRoleCount == secondRoleCount)
			{
				AbsorbConceptType(oialModel, losingConceptType.ObjectType, winningConceptType.ObjectType, MandatoryConstraintModality.Alethic, pathRole);
				absorberId = winningConceptType.ObjectType.Id;
			}
			else
			{
				AbsorbConceptType(oialModel, rolePlayer, oppositeRolePlayer, MandatoryConstraintModality.Alethic, oppositeRole);
				absorberId = oppositeRolePlayer.Id;
			}
			return absorberId;
		}
		/// <summary>
		/// Resolves fact type absorptions for fact types other than the one of which the <paramref name="ignoredRole"/> is a part.
		/// </summary>
		/// <param name="model">The <see cref="T:Neumont.Tools.ORM.OIALModel.LiveOIALModel"/> that contains all OIAL Model elements.</param>
		/// <param name="conceptType">The <see cref="T:Neumont.Tools.ORM.OIALModel.ObjectType"/> whose
		/// Oother played roles should be evaluated. If a <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> does
		/// not yet exist for the object type you are interpreting, it should be created with
		/// <see cref="M:T:Neumont.Tools.ORM.OIALModel.CreateConceptType"/>, and you should not use this method.</param>
		/// <param name="ignoredRole">The <see cref="T:Neumont.Tools.ORM.ObjectModel.Role"/> whose Fact Type
		/// has already been evaluated for absorption change.</param>
		private static void ResolveFactTypeAbsorptionsAfterChangingFunctionalRoles(LiveOIALModel model, ConceptType conceptType, Role ignoredRole)
		{
			ObjectType objectType = conceptType.ObjectType;
			Store store = objectType.Store;

			if (!IsConceptType(model, objectType, ignoredRole.FactType))
			{
				RemoveConceptType(conceptType);
				// RemoveConceptType should handle resolving any references.
				return;
			}

			LinkedElementCollection<Role> playedRoles = objectType.PlayedRoleCollection;

			IEnumerable<FactType> functionalDependentFacts = GetFunctionalDependentRoles(playedRoles, ignoredRole);

			int rolePlayerFunctionalNonDependentRoleCount = GetFunctionalNonDependentRoleCount(playedRoles, ignoredRole);

			foreach (FactType factType in functionalDependentFacts)
			{
				Guid id = factType.Id;
				AbsorbedFactType absorbedFactType;
				bool exists = model.myAbsorbedFactTypes.TryGetValue(id, out absorbedFactType);
				Debug.Assert(exists, "Absorbed fact type did not exist for a functional-dependent fact.");

				int mandatoryCount = GetAlethicInternalConstraintsCount(factType, ConstraintType.SimpleMandatory);
				LinkedElementCollection<RoleBase> roles = factType.RoleCollection;
				ObjectType oppositeRolePlayer = null;
				Role rolePlayerRole = roles[0].Role;
				Role oppositeRolePlayerRole = roles[1].Role;

				if (rolePlayerRole.RolePlayer != objectType)
				{
					rolePlayerRole = oppositeRolePlayerRole;
					oppositeRolePlayerRole = roles[0].Role;
				}

				if (mandatoryCount == 0)
				{
					int oppositeRolePlayerFunctionalNonDependentRoleCount = GetFunctionalNonDependentRoleCount(oppositeRolePlayer.PlayedRoleCollection,
						oppositeRolePlayerRole);


					Guid newAbsorber = Guid.Empty;
					if (rolePlayerFunctionalNonDependentRoleCount >= oppositeRolePlayerFunctionalNonDependentRoleCount)
					{
						// If the absorption is already going the right way, there is no need to change it.
						if (absorbedFactType.AbsorberId != objectType.Id)
						{
							// Should be a concept type ref between oppositeRolePlayer to current rolePlayer
							LinkedElementCollection<ConceptTypeChild> conceptTypeChildren =
								ConceptTypeChildHasPathRole.GetConceptTypeChild(rolePlayerRole);
							Debug.Assert(conceptTypeChildren.Count == 1 && conceptTypeChildren[0] is ConceptTypeRef,
								String.Format("Unexpected absorption pattern when evaluating other one-to-one fact types in which {0} plays a role.",
									objectType.Name));

							GenerateConceptTypeRef(objectType, oppositeRolePlayer, oppositeRolePlayerRole, null);

							// Deletes the old relationship.
							conceptTypeChildren.Clear();
							newAbsorber = objectType.Id;
						}
					}
					else
					{
						if (absorbedFactType.AbsorberId != oppositeRolePlayer.Id)
						{
							// Should be a concept type ref between oppositeRolePlayer to current rolePlayer
							LinkedElementCollection<ConceptTypeChild> conceptTypeChildren =
								ConceptTypeChildHasPathRole.GetConceptTypeChild(oppositeRolePlayerRole);
							Debug.Assert(conceptTypeChildren.Count == 1 && conceptTypeChildren[0] is ConceptTypeRef,
								String.Format("Unexpected absorption pattern when evaluating other one-to-one fact types in which {0} plays a role.",
									objectType.Name));

							GenerateConceptTypeRef(oppositeRolePlayer, objectType, oppositeRolePlayerRole, null);

							// Deletes the old relationship.
							conceptTypeChildren.Clear();
							newAbsorber = oppositeRolePlayer.Id;
						}
					}
					if (newAbsorber != Guid.Empty)
					{
						model.myAbsorbedFactTypes[factType.Id] = new AbsorbedFactType(newAbsorber, FactAbsorptionType.FactOnly);
					}
				}
				else if (mandatoryCount == 2)
				{
					UniquenessConstraint rolePlayerPreferredIdentifier = objectType.PreferredIdentifier;
					UniquenessConstraint oppositeRolePlayerPreferredIdentifier = oppositeRolePlayer.PreferredIdentifier;
					bool rolePlayerPreferredIdentifierExists = rolePlayerPreferredIdentifier != null;
					bool oppositeRolePlayerPreferredIdentifierExists = oppositeRolePlayerPreferredIdentifier != null;
					ConceptType oppositeConceptType = ConceptTypeHasObjectType.GetConceptType(oppositeRolePlayer);
					Guid absorberId = Guid.Empty;

					// The only difference between fully and fact-only absorptions is that fact-only absorptions need existing
					// information types deleted. The rest is the same.
					if (rolePlayerPreferredIdentifierExists)
					{
						absorberId = DoOneToOnePreferredIdentifierAbsorption(rolePlayerPreferredIdentifier,
							model, store, factType);
					}
					if (oppositeRolePlayerPreferredIdentifierExists && absorberId == Guid.Empty)
					{
						absorberId = DoOneToOnePreferredIdentifierAbsorption(oppositeRolePlayerPreferredIdentifier,
							model, store, factType);
					}
					if (absorberId == Guid.Empty)
					{
						absorberId = DoOneToOneAbsorptionForDoubleMandatories(objectType, oppositeRolePlayer, absorbedFactType, model, store,
							rolePlayerRole, oppositeRolePlayerRole);
					}
					model.myAbsorbedFactTypes[factType.Id] = new AbsorbedFactType(absorberId, FactAbsorptionType.Fully);
				}
			}
		}
		#endregion // One To One Absorption Algorithms
		#region Replacement Algorithms
		/// <summary>
		/// Replaces a <see cref="T:Neumont.Tools.ORM.OIALModel.InformationType"/> with a <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptTypeRef"/>.
		/// </summary>
		/// <param name="preferredIdentifierRoles">The roles that represent the role collection of the preferred-identifier uniqueness
		/// constraint.</param>
		/// <param name="newConceptType">The new <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> to reference.</param>
		/// <param name="absorbingConceptType">The <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> that shouldn't be given
		/// a  <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptTypeRef"/>.</param>
		private static void ReplaceConceptTypeChildrenWithConceptTypeRef(LinkedElementCollection<Role> preferredIdentifierRoles,
			ConceptType newConceptType, ConceptType absorbingConceptType)
		{
			int preferredIdentifierRoleCount = preferredIdentifierRoles.Count;

			// We need to add the Concept Type Ref only once per set of information types,
			// so we keep track of it through a boolean value
			bool addedConceptTypeRef = false;
			for (int i = 0; i < preferredIdentifierRoleCount; ++i)
			{
				LinkedElementCollection<ConceptTypeChild> conceptTypeChildren =
					ConceptTypeChildHasPathRole.GetConceptTypeChild(preferredIdentifierRoles[i]);

				if (addedConceptTypeRef)
				{
					DeleteConceptTypeChildren(conceptTypeChildren);
				}
				else if (conceptTypeChildren.Count != 0) // Nothing to delete (and no ConceptTypeRef will be created)
				{
					addedConceptTypeRef = true;
					DeleteConceptTypeChildrenAndReferenceConceptType(conceptTypeChildren,
						newConceptType, absorbingConceptType);
				}
			}
		}
		/// <summary>
		/// Deletes Concept Type Children and replaces their deletion with a referenced Concept Type.
		/// </summary>
		/// <param name="conceptTypeChildren">A set of <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptTypeChild"/> that should be deleted.</param>
		private static void DeleteConceptTypeChildren(LinkedElementCollection<ConceptTypeChild> conceptTypeChildren)
		{
			int conceptTypeChildCount = conceptTypeChildren.Count;

			for (int i = conceptTypeChildCount - 1; i >= 0; --i)
			{
				ConceptTypeChild conceptTypeChild = conceptTypeChildren[i];
				ConceptType parent = conceptTypeChild.Parent;
				conceptTypeChild.Delete();
			}
		}
		/// <summary>
		/// Deletes Concept Type Children and replaces their deletion with a referenced Concept Type.
		/// </summary>
		/// <param name="conceptTypeChildren">A set of <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptTypeChild"/> that should be deleted.</param>
		/// <param name="newConceptType">The new <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> to reference.</param>
		/// <param name="absorbingConceptType">The <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> that shouldn't be given
		/// a  <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptTypeRef"/>.</param>
		private static void DeleteConceptTypeChildrenAndReferenceConceptType(LinkedElementCollection<ConceptTypeChild> conceptTypeChildren,
			ConceptType newConceptType, ConceptType absorbingConceptType)
		{
			int conceptTypeChildCount = conceptTypeChildren.Count;

			for (int i = conceptTypeChildCount - 1; i >= 0; --i)
			{
				ConceptTypeChild conceptTypeChild = conceptTypeChildren[i];
				ConceptType parent = conceptTypeChild.Parent;
				if (parent != absorbingConceptType)
				{
					MandatoryConstraintModality mandatory = conceptTypeChild.Mandatory;
					string name = conceptTypeChild.Name;
					LinkedElementCollection<RoleBase> pathRoleCollection = conceptTypeChild.PathRoleCollection;
					parent.ReferencedConceptTypeCollection.Add(newConceptType);
					pathRoleCollection.RemoveAt(pathRoleCollection.Count - 1);

					ReadOnlyCollection<ConceptTypeRef> conceptTypeRefs = ConceptTypeRef.GetLinksToReferencingConceptType(newConceptType);
					ConceptTypeRef lastRef = conceptTypeRefs[conceptTypeRefs.Count - 1];
					lastRef.PathRoleCollection.AddRange(pathRoleCollection);
					lastRef.Name = newConceptType.ObjectType.Name;
					lastRef.Mandatory = mandatory;
					lastRef.OppositeName = parent.Name;
				}
				conceptTypeChild.Delete();
			}
		}
		/// <summary>
		/// Replaces a <see cref="T:Neumont.Tools.ORM.OIALModel.InformationType"/> with a number of
		/// <see cref="T:Neumont.Tools.ORM.OIALModel.InformationType"/> elements.
		/// </summary>
		/// <param name="potentialPathRoles">The roles that represent the role collection of possible path roles.</param>
		/// <param name="newConceptType">The new <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> to reference.</param>
		/// <param name="absorbingConceptType">The <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> that shouldn't be given
		/// a  <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptTypeRef"/>.</param>
		private static void ReplaceConceptTypeChildrenWithInformationType(LinkedElementCollection<Role> potentialPathRoles,
			ConceptType newConceptType, ConceptType absorbingConceptType)
		{
			int potentialPathRoleCount = potentialPathRoles.Count;

			UniquenessConstraint preferredIdentifier = newConceptType.ObjectType.PreferredIdentifier;

			for (int i = 0; i < potentialPathRoleCount; ++i)
			{
				LinkedElementCollection<ConceptTypeChild> conceptTypeChildren =
					ConceptTypeChildHasPathRole.GetConceptTypeChild(potentialPathRoles[i]);
				DeleteConceptTypeChildrenAndGenerateInformationTypes(conceptTypeChildren,
					newConceptType, absorbingConceptType);
			}

			if (preferredIdentifier != null)
			{
				if (preferredIdentifier.RoleCollection.Count != 1)
				{
					ChildSequenceUniquenessConstraint childSequenceUniquenessConstraint =
						GetPreferredChildSequenceUniquenessConstraint(preferredIdentifier);

					if (childSequenceUniquenessConstraint != null)
					{
						RegenerateChildSequenceUniquenessConstraintChildren(preferredIdentifier, childSequenceUniquenessConstraint);
					}
				}
			}
		}
		/// <summary>
		/// Deletes Concept Type Children and replaces their deletion with a referenced Concept Type.
		/// </summary>
		/// <param name="conceptTypeChildren">A set of <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptTypeChild"/> that should be deleted.</param>
		/// <param name="newConceptType">The new <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> to reference.</param>
		/// <param name="absorbingConceptType">The <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> that shouldn't be given
		/// an  <see cref="T:Neumont.Tools.ORM.OIALModel.InformationType"/>.</param>
		private static void DeleteConceptTypeChildrenAndGenerateInformationTypes(LinkedElementCollection<ConceptTypeChild> conceptTypeChildren,
			ConceptType newConceptType, ConceptType absorbingConceptType)
		{
			int conceptTypeChildCount = conceptTypeChildren.Count;

			UniquenessConstraint preferredIdentifier = newConceptType.ObjectType.PreferredIdentifier;
			IList<Role> preferredIdentifierRoles = null;
			int preferredIdentifierRoleCount = 0;
			bool isCompositelyIdentified = false;

			if (preferredIdentifier != null) // Possible for value types to be concept types, in which case they will not have a preferred identifier.
			{
				preferredIdentifierRoles = preferredIdentifier.RoleCollection;
				preferredIdentifierRoleCount = preferredIdentifierRoles.Count;
				isCompositelyIdentified = preferredIdentifierRoleCount > 1;
			}

			InformationTypeFormat valueType = InformationTypeFormatHasObjectType.GetInformationTypeFormat(newConceptType.ObjectType);
			for (int i = conceptTypeChildCount - 1; i >= 0; --i)
			{
				ConceptTypeChild conceptTypeChild = conceptTypeChildren[i];
				ConceptType parent = conceptTypeChild.Parent;
				if (parent != absorbingConceptType)
				{
					MandatoryConstraintModality mandatory = conceptTypeChild.Mandatory;
					string name = conceptTypeChild.Name;
					LinkedElementCollection<RoleBase> pathRoleCollection = conceptTypeChild.PathRoleCollection;

					// TODO: Abstract into Information Type method.
					if (preferredIdentifierRoles != null)
					{
						for (int j = 0; j < preferredIdentifierRoleCount; ++j)
						{
							Role preferredIdentifierRole = preferredIdentifierRoles[j];
							ObjectType identifierObjectType = preferredIdentifierRole.RolePlayer;

							parent.InformationTypeFormatCollection.Add(InformationTypeFormatHasObjectType.GetInformationTypeFormat(identifierObjectType));
							ReadOnlyCollection<InformationType> informationTypes = InformationType.GetLinksToInformationTypeFormatCollection(parent);
							InformationType lastInformationType = informationTypes[informationTypes.Count - 1];

							CheckForAbsorbedConceptTypeAndAddUniqueness(conceptTypeChild, lastInformationType);
							lastInformationType.PathRoleCollection.AddRange(pathRoleCollection);
							lastInformationType.PathRoleCollection.Add(preferredIdentifierRole);

							string roleName = preferredIdentifierRole.Name;
							lastInformationType.Name = isCompositelyIdentified ? string.IsNullOrEmpty(roleName) ?
								string.Concat(name, "_", preferredIdentifierRole.RolePlayer.Name) : string.Concat(name, "_", roleName) : name;
							lastInformationType.Mandatory = mandatory;
						}
					}
					else
					{
						parent.InformationTypeFormatCollection.Add(valueType);
						ReadOnlyCollection<InformationType> informationTypes = InformationType.GetLinksToInformationTypeFormatCollection(parent);
						InformationType lastInformationType = informationTypes[informationTypes.Count - 1];
						CheckForAbsorbedConceptTypeAndAddUniqueness(conceptTypeChild, lastInformationType);
						lastInformationType.PathRoleCollection.AddRange(pathRoleCollection);

						Debug.Assert(pathRoleCollection.Count == 1, "More than path role for a Concept Type relationship with a Value Type.");

						lastInformationType.Name = GetNameForConceptTypeChild(pathRoleCollection[0].Role);
						lastInformationType.Mandatory = mandatory;
					}
				}
				conceptTypeChild.Delete();
			}
		}
		/// <summary>
		/// Generates a <see cref="T:Neumont.Tools.ORM.OIALModel.InformationType"/> between a Concept Type and another object type.
		/// </summary>
		/// <param name="containingConceptType">The <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> which will absorb
		/// an InformationTypeFormat..</param>
		/// <param name="informationTypeObjectType">The <see cref="T:Neumont.Tools.ORM.ObjectModel.ObjectType"/> to which an InformationType
		/// will be generated.</param>
		/// <param name="pathRole">The <see cref="T:Neumont.Tools.ORM.ObjectModel.Role"/> on which the
		/// <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptTypeRef"/> occurs.</param>
		/// <param name="oldConceptTypeChild">The old <see cref="T:Neumont.Tools.ORM.OIALModel.InformationType"/> for this relationship. This
		/// is deleted.</param>
		private static void GenerateInformationType(LiveOIALModel model, ConceptType containingConceptType, ObjectType informationTypeObjectType, Role pathRole,
			ConceptTypeChild oldConceptTypeChild)
		{
			string baseName = pathRole.Name;
			Store store = model.Store;
			bool forceRoleNames = true;
			if (string.IsNullOrEmpty(baseName))
			{
				forceRoleNames = false;
				baseName = informationTypeObjectType.Name;
			}

			model.GetInformationTypesInternal(model.Store, containingConceptType, pathRole, baseName, true, new LinkedList<RoleBase>(),
				GetMandatoryModality(pathRole.OppositeRole.Role), model.GetSingleChildConstraints(pathRole, store), false, false);
			oldConceptTypeChild.Delete();
		}
		/// <summary>
		/// Generates a <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptTypeRef"/> between two Concept Types.
		/// </summary>
		/// <param name="referencingObjectType">The <see cref="T:Neumont.Tools.ORM.ObjectModel.ObjectType"/> whose underlying
		/// <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> references another <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/>.</param>
		/// <param name="referencedObjectType">The <see cref="T:Neumont.Tools.ORM.ObjectModel.ObjectType"/> whose underlying
		/// <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> is referenced by another
		/// <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/>.</param>
		/// <param name="pathRole">The <see cref="T:Neumont.Tools.ORM.ObjectModel.Role"/> on which the
		/// <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptTypeRef"/> occurs.</param>
		/// <param name="oldConceptTypeChild">The old <see cref="T:Neumont.Tools.ORM.OIALModel.InformationType"/> for this relationship. This
		/// is deleted.</param>
		private static void GenerateConceptTypeRef(ObjectType referencingObjectType, ObjectType referencedObjectType, Role pathRole,
	ConceptTypeChild oldConceptTypeChild)
		{
			ConceptType referencingConceptType = ConceptTypeHasObjectType.GetConceptType(referencingObjectType);
			ConceptType referencedConceptType = ConceptTypeHasObjectType.GetConceptType(referencedObjectType);

			if (referencingConceptType == null)
			{
				throw new ArgumentException("Referencing object type did not have an associated Concept Type.", "referencingObjectType");
			}
			else if (referencedConceptType == null)
			{
				throw new ArgumentException("Referenced object type did not have an associated Concept Type.", "referencedObjectType");
			}

			Store store = referencedObjectType.Store;

			referencingConceptType.ReferencedConceptTypeCollection.Add(referencedConceptType);

			ReadOnlyCollection<ConceptTypeRef> conceptTypeRefs = ConceptTypeRef.GetLinksToReferencedConceptTypeCollection(referencingConceptType);
			ConceptTypeRef conceptTypeRef = conceptTypeRefs[conceptTypeRefs.Count - 1];

			if (oldConceptTypeChild == null)
			{
				Role absorbingRole = pathRole.OppositeRole.Role;

				Debug.Assert(absorbingRole.RolePlayer == referencingObjectType,
					"The absorbing role's role player is not the original Concept Type.");
				conceptTypeRef.Mandatory = GetMandatoryModality(absorbingRole);
				conceptTypeRef.Name = GetNameForConceptTypeChild(pathRole);

				conceptTypeRef.PathRoleCollection.Add(pathRole);

				LinkedElementCollection<ConstraintRoleSequence> roleConstraints = pathRole.ConstraintRoleSequenceCollection;
				int roleConstraintCount = roleConstraints.Count;

				for (int i = 0; i < roleConstraintCount; ++i)
				{
					ConstraintRoleSequence roleSequence = roleConstraints[i];
					UniquenessConstraint uConstraint = roleSequence.Constraint as UniquenessConstraint;
					if (uConstraint != null && uConstraint.RoleCollection.Count == 1)
					{
						conceptTypeRef.SingleChildConstraintCollection.Add(
							new SingleChildUniquenessConstraint(store,
							new PropertyAssignment(SingleChildUniquenessConstraint.IsPreferredDomainPropertyId, uConstraint.IsPreferred),
							new PropertyAssignment(SingleChildUniquenessConstraint.ModalityDomainPropertyId, uConstraint.Modality),
							new PropertyAssignment(SingleChildUniquenessConstraint.NameDomainPropertyId, uConstraint.Name)));
					}
				}
			}
			else
			{
				conceptTypeRef.Mandatory = oldConceptTypeChild.Mandatory;
				conceptTypeRef.Name = oldConceptTypeChild.Name;
				LinkedElementCollection<RoleBase> oldPathRoles = oldConceptTypeChild.PathRoleCollection;

				oldPathRoles.RemoveAt(oldPathRoles.Count - 1);

				conceptTypeRef.PathRoleCollection.AddRange(oldPathRoles);
				conceptTypeRef.SingleChildConstraintCollection.AddRange(oldConceptTypeChild.SingleChildConstraintCollection);

				CheckForAbsorbedConceptTypeAndAddUniqueness(oldConceptTypeChild, conceptTypeRef);
				oldConceptTypeChild.Delete();
			}
			conceptTypeRef.OppositeName = referencingObjectType.Name;
		}
		#endregion // Replacement Algorithms
		#region Concept Type Manipulation
		/// <summary>
		/// Makes the specified concept type children mandatory (with a specified modality) if and only if
		/// they are children of the specified parent Object Type.
		/// </summary>
		/// <param name="conceptTypeChildren">The list of concept type children to make mandatory.</param>
		/// <param name="parentObjectType">The parent <see cref="T:Neumont.Tools.ORM.ObjectModel.ObjectType"/> for which children should be
		/// made alethicly mandatory.</param>
		/// <param name="modality">The modality of the mandatory constraint.</param>
		private static void ChangeConceptTypeChildrenMandatoryModality(IList<ConceptTypeChild> conceptTypeChildren,
			ObjectType parentObjectType, MandatoryConstraintModality modality)
		{
			int conceptTypeChildrenCount = conceptTypeChildren.Count;

			for (int i = 0; i < conceptTypeChildrenCount; ++i)
			{
				// Change the mandatory property only for children that are absorbed directly into
				// the role player for which a concept type exists.
				ConceptTypeChild conceptTypeChild = conceptTypeChildren[i];
				if (conceptTypeChild.Parent.ObjectType == parentObjectType)
				{
					conceptTypeChild.Mandatory = modality;
				}
			}
		}
		/// <summary>
		/// Absorbs a <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> into another and resolves the link's attributes, including path roles.
		/// </summary>
		/// <param name="oialModel">The <see cref="T:Neumont.Tools.ORM.OIALModel.OIALModel"/> where that contains these concept types.</param>
		/// <param name="absorbee">The <see cref="T:Neumont.Tools.ORM.OIALModel.ObjectType"/> being absorbed into
		/// another <see cref="T:Neumont.Tools.ORM.OIALModel.ObjectType"/>.</param>
		/// <param name="absorber">The <see cref="T:Neumont.Tools.ORM.OIALModel.ObjectType"/> absorbing the other
		/// <see cref="T:Neumont.Tools.ORM.OIALModel.ObjectType"/></param>
		/// <param name="mandatory">The modality of the mandatory relationship between the Concept Types.</param>
		/// <param name="pathRole">The <see cref="T:Neumont.Tools.ORM.ObjectModel.Role"/> on which the absorbee
		/// <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> is absorbed.</param>
		private static void AbsorbConceptType(LiveOIALModel oialModel, ObjectType absorbeeObjectType, ObjectType absorberObjectType, MandatoryConstraintModality mandatory, Role pathRole)
		{
			ConceptType absorbee = ConceptTypeHasObjectType.GetConceptType(absorbeeObjectType);
			ConceptType absorber = ConceptTypeHasObjectType.GetConceptType(absorberObjectType);
			Store store = oialModel.Store;

			if (absorbee == null)
			{
				absorbee = CreateConceptType(oialModel, store, absorbeeObjectType, absorber);
			}
			if (absorber == null)
			{
				absorber = CreateConceptType(oialModel, store, absorberObjectType, null);
			}

			bool isAlreadyAbsorbed = absorbee.AbsorbingConceptType == absorber;
			if (!isAlreadyAbsorbed)
			{
				if (absorber.Model == null && (absorber.AbsorbingConceptType == null || absorber.AbsorbingConceptType == absorbee))
				{
					absorber.Model = oialModel;
					absorber.AbsorbingConceptType = null;
				}
				if (absorbee.Model != null)
				{
					absorbee.Model = null;
				}
				absorbee.AbsorbingConceptType = absorber;
			}
			ConceptTypeAbsorbedConceptType absorbedConceptType = ConceptTypeAbsorbedConceptType.GetLinkToAbsorbingConceptType(absorbee);
			if (!isAlreadyAbsorbed)
			{
				absorbedConceptType.Name = absorbee.Name;
				absorbedConceptType.PathRoleCollection.Add(pathRole);
			}
			// Only the mandatory modality might change if it's already absorbed.
			absorbedConceptType.Mandatory = mandatory;
		}
		/// <summary>
		/// Creates a new <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> while resolving old Concept Type Children
		/// that reference this <see cref="T:Neumont.Tools.ORM.ObjectModel.ObjectType"/>.
		/// </summary>
		/// <param name="oialModel">The <see cref="T:Neumont.Tools.ORM.OIALModel.O"/></param>
		/// <param name="store">The <see cref="T:Microsoft.VisualStudio.Modeling.Store"/> to which the new
		/// <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> is added.</param>
		/// <param name="objectType">The <see cref="T:Neumont.Tools.ORM.ObjectModel.ObjectType"/> for which a
		/// <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> is created.</param>
		/// <param name="absorbingConceptType">A <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> that might absorb
		/// the new <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/>. Can be null.</param>
		/// <returns>The new <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> that was created.</returns>
		private static ConceptType CreateConceptType(LiveOIALModel oialModel, Store store, ObjectType objectType, ConceptType absorbingConceptType)
		{
			ConceptType conceptType = new ConceptType(store,
						new PropertyAssignment(ConceptType.NameDomainPropertyId, objectType.Name));
			UniquenessConstraint preferredIdentifier = objectType.PreferredIdentifier;
			ChildSequenceUniquenessConstraint constraint = GetPreferredChildSequenceUniquenessConstraint(objectType.PreferredIdentifier);
			conceptType.ObjectType = objectType;

			if (objectType.IsValueType)
			{
				ReplaceConceptTypeChildrenWithConceptTypeRef(objectType.PlayedRoleCollection, conceptType,
					absorbingConceptType);
			}
			else
			{
				ReplaceConceptTypeChildrenWithConceptTypeRef(objectType.PreferredIdentifier.RoleCollection, conceptType,
					absorbingConceptType);
			}
			oialModel.InformationTypesAndConceptTypeRefs(store, conceptType);


			if (constraint == null || preferredIdentifier.RoleCollection.Count != 1)
			{
				RegenerateChildSequenceUniquenessConstraintChildren(preferredIdentifier, constraint);
			}


			return conceptType;
		}
		/// <summary>
		/// Gets the preferred identifier <see cref="T:Neumont.Tools.ORM.OIALModel.ChildSequenceUniquenessConstraint"/> for
		/// a <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> based on its associated <see cref="T:Neumont.Tools.ORM.ObjectModel.ObjectType"/>'s
		/// preferred identifier.
		/// </summary>
		/// <param name="preferredIdentifier">The <see cref="T:Neumont.Tools.ORM.ObjectModel.UniquenessConstraint"/> which is the preferred
		/// identifier of some <see cref="T:Neumont.Tools.ORM.ObjectModel.ObjectType"/> that has an associated
		/// <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/>.</param>
		/// <returns>The preferred identifier <see cref="T:Neumont.Tools.ORM.OIALModel.ChildSequenceUniquenessConstraint"/>.</returns>
		private static ChildSequenceUniquenessConstraint GetPreferredChildSequenceUniquenessConstraint(UniquenessConstraint preferredIdentifier)
		{
			LinkedElementCollection<Role> preferredIdentifierRoles = preferredIdentifier.RoleCollection;
			int preferredIdentifierRoleCount = preferredIdentifierRoles.Count;

			if (preferredIdentifierRoleCount > 0)
			{
				Role role = preferredIdentifierRoles[0];
				LinkedElementCollection<ConceptTypeChild> conceptTypeChildren = ConceptTypeChildHasPathRole.GetConceptTypeChild(role);
				int conceptTypeChildCount = conceptTypeChildren.Count;

				if (conceptTypeChildCount > 0)
				{
					LinkedElementCollection<ChildSequence> childSequences =
						ChildSequenceHasConceptTypeChild.GetChildSequenceCollection(conceptTypeChildren[0]);
					int childSequenceCount = childSequences.Count;

					for (int i = 0; i < childSequenceCount; ++i)
					{
						ChildSequenceUniquenessConstraint uniquenessConstraint =
							ChildSequenceConstraintHasChildSequence.GetChildSequenceConstraint(childSequences[i]) as ChildSequenceUniquenessConstraint;

						if (uniquenessConstraint != null && uniquenessConstraint.Name == preferredIdentifier.Name)
						{
							return uniquenessConstraint;
						}
					}
				}
			}
			// Shouldn't reach here.
			return null;
		}
		/// <summary>
		/// Regenerates the children for a <see cref="T:Neumont.Tools.ORM.OIALModel.ChildSequenceUniquenessConstraint"/>.
		/// </summary>
		/// <param name="uniquenessConstraint">The preferred identifier <see cref="T:Neumont.Tools.ORM.OIALModel.UniquenessConstraint"/>
		/// for an <see cref="T:Neumont.Tools.ORM.ObjectModel.ObjectType"/> that has an associated
		/// <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/>.</param>
		/// <param name="childSequenceUniquenessConstraint">The <see cref="T:Neumont.Tools.ORM.OIALModel.ChildSequenceUniquenessConstraint"/>
		/// for which to regenerate children.</param>
		private static void RegenerateChildSequenceUniquenessConstraintChildren(UniquenessConstraint uniquenessConstraint,
			ChildSequenceUniquenessConstraint childSequenceUniquenessConstraint)
		{
			LinkedElementCollection<ConceptTypeChild> conceptTypeChildren = childSequenceUniquenessConstraint.ChildSequence.ConceptTypeChildCollection;
			conceptTypeChildren.Clear();
			LinkedElementCollection<Role> roleCollection = uniquenessConstraint.RoleCollection;
			List<ConceptTypeChild> conceptTypeHasChildCollection =
				GetConceptTypeChildRelationshipsForSetConstraints(roleCollection, false);
			if (conceptTypeHasChildCollection == null)
			{
				return;
			}
			int collectionCount = roleCollection.Count;
			if (conceptTypeHasChildCollection.Count != 0)
			{
				conceptTypeChildren.AddRange(conceptTypeHasChildCollection);

				if (uniquenessConstraint != null)
				{
					bool isPreferred = uniquenessConstraint.IsPreferred;
					bool ignoreIfPrimary = !uniquenessConstraint.IsInternal;
					bool shouldBeUnique = false;
					if (ignoreIfPrimary)
					{
						for (int i = 0; i < conceptTypeHasChildCollection.Count; ++i)
						{
							ConceptTypeChild conceptTypeChild = conceptTypeHasChildCollection[i];
							LinkedElementCollection<RoleBase> pathRoles = conceptTypeChild.PathRoleCollection;
							int pathRoleCount = pathRoles.Count;
							if (pathRoleCount > 1)
							{
								Role secondPathRole = pathRoles[pathRoleCount - 2].Role;
								LinkedElementCollection<ConstraintRoleSequence> constraints = secondPathRole.ConstraintRoleSequenceCollection;
								for (int j = 0; j < constraints.Count; ++j)
								{
									UniquenessConstraint uConstraint = constraints[j].Constraint as UniquenessConstraint;
									if (uConstraint != null && uConstraint.IsPreferred && uConstraint != uniquenessConstraint)
									{
										int uConstraintRoleCount = uniquenessConstraint.RoleCollection.Count;
										if (uConstraintRoleCount > 1)
										{
											ignoreIfPrimary = true;
											shouldBeUnique = false;
											break;
										}
										else
										{
											ignoreIfPrimary = false;
											shouldBeUnique = false;
											break;
										}
									}
									else
									{
										ignoreIfPrimary = false;
										shouldBeUnique = true;
									}
								}
							}
						}
					}
					if (shouldBeUnique && isPreferred)
					{
						isPreferred = false;
					}
					childSequenceUniquenessConstraint.IsPreferred = isPreferred;
					childSequenceUniquenessConstraint.Modality = uniquenessConstraint.Modality;
					childSequenceUniquenessConstraint.ShouldIgnore = ignoreIfPrimary;
				}
			}
		}
		/// <summary>
		/// Creates a new <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> while resolving old Concept Type Children
		/// that reference this <see cref="T:Neumont.Tools.ORM.ObjectModel.ObjectType"/>.
		/// </summary>
		/// <param name="oialModel">The <see cref="T:Neumont.Tools.ORM.OIALModel.OIALModel"/> to which the
		/// new <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> will be attached.</param>
		/// <param name="store">The <see cref="T:Microsoft.VisualStudio.Modeling.Store"/> to which the new
		/// <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> is added.</param>
		/// <param name="objectType">The <see cref="T:Neumont.Tools.ORM.ObjectModel.ObjectType"/> for which a
		/// <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> is created.</param>
		/// <param name="absorbingConceptType">A <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> that might absorb
		/// the new <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/>. Can be null.</param>
		/// <param name="pathRole">The path role on which the <see cref="T:Neumont.Tools.ORM.ObjectModel.ObjectType"/> is absorbed.</param>
		/// <returns>The new <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> that was created.</returns>
		private static ConceptType CreateConceptType(LiveOIALModel oialModel, Store store, ObjectType objectType, ConceptType absorbingConceptType, Role pathRole)
		{
			ConceptType conceptType = new ConceptType(store,
						new PropertyAssignment(ConceptType.NameDomainPropertyId, objectType.Name));

			DeleteConceptTypeChildrenAndReferenceConceptType(ConceptTypeChildHasPathRole.GetConceptTypeChild(pathRole),
				conceptType, absorbingConceptType);
			conceptType.ObjectType = objectType;
			oialModel.InformationTypesAndConceptTypeRefs(store, conceptType);

			return conceptType;
		}
		/// <summary>
		/// Removes an existing <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> while resolving old Concept Type Children
		/// that reference this <see cref="T:Neumont.Tools.ORM.ObjectModel.ObjectType"/>.
		/// </summary>
		/// <param name="conceptType">The <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptType"/> that should be deleted.</param>
		private static void RemoveConceptType(ConceptType conceptType)
		{
			ObjectType objectType = conceptType.ObjectType;
			ReplaceConceptTypeChildrenWithInformationType(objectType.PlayedRoleCollection,
				conceptType, null);

			conceptType.Delete();
		}
		/// <summary>
		/// Checks for whether the passed <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptTypeChild"/> is
		/// an absorbed Concept Type link. If so, adds a <see cref="T:Neumont.Tools.ORM.OIALModel.SingleChildUniquenessConstraint"/>
		/// </summary>
		/// <param name="oldConceptTypeChild">The old <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptTypeChild"/>.</param>
		/// <param name="newConceptTypeChild">The new <see cref="T:Neumont.Tools.ORM.OIALModel.ConceptTypeChild"/>.</param>
		private static void CheckForAbsorbedConceptTypeAndAddUniqueness(ConceptTypeChild oldConceptTypeChild, ConceptTypeChild newConceptTypeChild)
		{
			Store store = oldConceptTypeChild.Store;

			if (oldConceptTypeChild is ConceptTypeAbsorbedConceptType && !(newConceptTypeChild is ConceptTypeAbsorbedConceptType))
			{
				newConceptTypeChild.SingleChildConstraintCollection.Add(
						new SingleChildUniquenessConstraint(store,
						new PropertyAssignment(SingleChildUniquenessConstraint.ModalityDomainPropertyId, ConstraintModality.Alethic),
						new PropertyAssignment(SingleChildUniquenessConstraint.IsPreferredDomainPropertyId, false))
					);
			}
		}
		#endregion // Concept Type Manipulation
	}
}
