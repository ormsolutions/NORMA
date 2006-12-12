using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORM.TestOIALModel
{
	public partial class LiveOIALModel
	{
		/// <summary>
		/// Encapsulates functions common to handling MandatoryConstraint rules.
		/// </summary>
		private sealed partial class MandatoryConstraintRule
		{
			/// <summary>
			/// Handles changes to the OIAL Model when a
			/// <see cref="T:Neumont.Tools.ORM.ObjectModel.MandatoryConstraint" /> is added to the ORM Model.
			/// </summary>
			[RuleOn(typeof(MandatoryConstraint))]
			private sealed partial class MandatoryConstraintAddRule : AddRule
			{
				/// <summary>
				/// Processes changes to the OIAL Model when a MandatoryConstraint is added to the ORM Model.
				/// </summary>
				/// <param name="e"><see cref="T:Microsoft.VisualStudio.Modeling.ElementAddedEventArgs" /></param>
				public sealed override void ElementAdded(ElementAddedEventArgs e)
				{
					ORMCoreDomainModel.DelayValidateElement(e.ModelElement, ProcessElement);
				}

				/// <summary>
				/// A callback function to process changes to the OIAL Model when a MandatoryConstraint is added to the ORM Model.
				/// </summary>
				/// <param name="element"><see cref="T:Neumont.Tools.ORM.ObjectModel.MandatoryConstraint" /></param>
				private static void ProcessElement(ModelElement element)
				{
					MandatoryConstraint mandatoryRoleConstraint = element as MandatoryConstraint;
					Debug.Assert(mandatoryRoleConstraint != null, "Element was not of the expected type when executing a rule.");
					ProcessElementAdded(mandatoryRoleConstraint);
				}
			}

			/// <summary>
			/// Handles changes to the OIAL Model when a
			/// <see cref="T:Neumont.Tools.ORM.ObjectModel.MandatoryConstraint" /> is deleted from the ORM Model.
			/// </summary>
			[RuleOn(typeof(MandatoryConstraint))]
			private sealed partial class MandatoryConstraintDeletingRule : DeletingRule
			{
				/// <summary>
				/// Processes changes to the OIAL Model when a MandatoryConstraint is deleted from the ORM Model.
				/// </summary>
				/// <param name="e"><see cref="T:Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs" /></param>
				public sealed override void ElementDeleting(ElementDeletingEventArgs e)
				{
					// Rule fired twice by two other rules - TODO: Talk to Matt about whether this is supposed to happen.
					MandatoryConstraint mandatoryRoleConstraint = e.ModelElement as MandatoryConstraint;
					Debug.Assert(mandatoryRoleConstraint != null, "Element was not of the expected type when executing a rule.");
					ProcessElementDeleting(mandatoryRoleConstraint, MandatoryConstraintModality.Alethic);
				}
			}

			/// <summary>
			/// Handles changes to the OIAL Model when a
			/// <see cref="T:Neumont.Tools.ORM.ObjectModel.MandatoryConstraint" /> is changed on the ORM Model.
			/// </summary>
			[RuleOn(typeof(MandatoryConstraint))]
			private sealed partial class MandatoryConstraintChangeRule : ChangeRule
			{
				/// <summary>
				/// Processes changes to the OIAL Model when a MandatoryConstraint is changed on the ORM Model.
				/// </summary>
				/// <param name="e"><see cref="T:Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs" /></param>
				public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
				{
					MandatoryConstraint mandatoryRoleConstraint = e.ModelElement as MandatoryConstraint;
					Guid attributeGuid = e.DomainProperty.Id;
					Debug.Assert(mandatoryRoleConstraint != null, "Element was not of the expected type when executing a rule.");

					if (!mandatoryRoleConstraint.IsSimple)
					{
						return;
					}

					if (attributeGuid == MandatoryConstraint.ModalityDomainPropertyId)
					{
						ConstraintModality newModality = (ConstraintModality)e.NewValue;

						if (newModality == ConstraintModality.Alethic)
						{
							// Modality was changed from Deontic to Alethic. Simulate Add.
							ProcessElementAdded(mandatoryRoleConstraint);
						}
						else
						{
							// Modality was changed from Alethic to Deontic. Simulate Remove.
							ProcessElementDeleting(mandatoryRoleConstraint, MandatoryConstraintModality.Deontic);
						}
					}
				}
			}

			/// <summary>
			/// Processes a <see cref="T:Neumont.Tools.ORM.ObjectModel.MandatoryConstraint"/> that has been added to the model.
			/// </summary>
			/// <param name="mandatoryRoleConstraint">The constraint that was added.</param>
			private static void ProcessElementAdded(MandatoryConstraint mandatoryRoleConstraint)
			{
				LiveOIALModel liveOIALModel = OIALModelHasORMModel.GetOIALModel(mandatoryRoleConstraint.Model);
				Store store = liveOIALModel.Store;

				if (!mandatoryRoleConstraint.IsSimple)
				{
					return;
				}

				LinkedElementCollection<Role> roles = mandatoryRoleConstraint.RoleCollection;
				Debug.Assert(roles.Count == 1, "Role collection count should be 1 for a simple mandatory role constraint.");

				Role mandatoryConstraintRole = roles[0];
				FactType factType = mandatoryConstraintRole.FactType;
				ObjectType rolePlayer = mandatoryConstraintRole.RolePlayer;

				LinkedElementCollection<RoleBase> factRoles = factType.RoleCollection;
				int factRoleCount = factRoles.Count;

				// Simple mandatory role constraints do not affect mapping if the fact type does not have two roles.
				if (factRoleCount == 2)
				{
					LinkedElementCollection<SetConstraint> factSetConstraints = factType.SetConstraintCollection;
					int factSetConstraintCount = factSetConstraints.Count;

					int alethicUniquenessConstraintCount = 0;
					int alethicMandatoryRoleConstraintCount = 0;
					int deonticMandatoryRoleConstraintCount = 0;

					IList<Role> uniqueRoles = new List<Role>();
					bool foundManyToManyUniquenessConstraint = false;
					for (int i = 0; i < factSetConstraintCount; ++i)
					{
						SetConstraint setConstraint = factSetConstraints[i];
						UniquenessConstraint uniquenessConstraint = setConstraint as UniquenessConstraint;
						if (!foundManyToManyUniquenessConstraint && uniquenessConstraint != null &&
							uniquenessConstraint.Modality == ConstraintModality.Alethic)
						{
							if (uniquenessConstraint.FactTypeCollection.Count > 1)
							{
								// Swallow the external constraint
								continue;
							}
							if (uniquenessConstraint.RoleCollection.Count > 1)
							{
								// Many-to-many uniqueness constraint. Do nothing.
								alethicUniquenessConstraintCount = -1;
								foundManyToManyUniquenessConstraint = true;
								continue;
							}
							uniqueRoles.Add(uniquenessConstraint.RoleCollection[0]);
							++alethicUniquenessConstraintCount;
							continue;
						}
						MandatoryConstraint mandatoryConstraint = setConstraint as MandatoryConstraint;
						if (mandatoryConstraint != null)
						{
							if (mandatoryConstraint.Modality == ConstraintModality.Alethic)
							{
								++alethicMandatoryRoleConstraintCount;
							}
							else
							{
								++deonticMandatoryRoleConstraintCount;
							}
						}
					}

					// Many-to-one fact type.
					if (alethicUniquenessConstraintCount == 1)
					{
						Role uniqueRole = uniqueRoles[0];
						if (uniqueRole == mandatoryConstraintRole)
						{
							ChangeConceptTypeChildrenMandatoryModality(ConceptTypeChildHasPathRole.GetConceptTypeChild(uniqueRole.OppositeRole),
								rolePlayer, MandatoryConstraintModality.Alethic);
						}
						// If the unique role is not the mandatory constraint role, then no change is necessary.
					}
					else if (alethicUniquenessConstraintCount > 1) // One-to-one fact type.
					{
						// Although an error, people are allowed to put more than two uniqueness constraints on a binary fact type
						Role oppositeRole = mandatoryConstraintRole.OppositeRole.Role;
						ObjectType oppositeRolePlayer = oppositeRole.RolePlayer;
						AbsorbedFactType oldAbsorption;
						bool success = liveOIALModel.myAbsorbedFactTypes.TryGetValue(factType.Id, out oldAbsorption);
						Debug.Assert(success, "Fact type was not absorbed.");


						if (alethicMandatoryRoleConstraintCount == 1)
						{
							ConceptType oppositeConceptType = ConceptTypeHasObjectType.GetConceptType(oppositeRolePlayer);

							if (oppositeConceptType == null)
							{
								Debug.Assert(oldAbsorption.AbsorptionType == FactAbsorptionType.FactOnly, "Fact type was not absorbed fact-only.");
								ChangeConceptTypeChildrenMandatoryModality(
									ConceptTypeChildHasPathRole.GetConceptTypeChild(mandatoryConstraintRole.OppositeRole),
									rolePlayer, MandatoryConstraintModality.Alethic);
							}
							else
							{
								// If execution hits this point, we know that the opposite role player has a Concept Type
								// If so, then we have a full absorption.
								liveOIALModel.myAbsorbedFactTypes[factType.Id] = new AbsorbedFactType(oppositeRolePlayer.Id, FactAbsorptionType.Fully);

								ConceptType conceptType = ConceptTypeHasObjectType.GetConceptType(rolePlayer);
								Debug.Assert(oppositeConceptType != null, "Opposite concept type did not exist when doing a one-to-one absorption change.");

								if (conceptType == null)
								{
									conceptType = CreateConceptType(liveOIALModel, store, rolePlayer, oppositeConceptType);
								}

								// Opposite role cannot be alethicly mandatory for execution to come here.
								MandatoryConstraintModality mandatory =
									oppositeRole.MandatoryConstraintModality == ConstraintModality.Deontic ?
									MandatoryConstraintModality.Deontic : MandatoryConstraintModality.NotMandatory;

								AbsorbConceptType(liveOIALModel, rolePlayer, oppositeRolePlayer, mandatory, mandatoryConstraintRole);
							}
						}
						else if (alethicMandatoryRoleConstraintCount == 2)
						{
							Guid absorberId = Guid.Empty;
							// Check if the fact type is the preferred identifier for a role player.
							UniquenessConstraint rolePlayerPreferredIdentifier = rolePlayer.PreferredIdentifier;
							UniquenessConstraint oppositeRolePlayerPreferredIdentifier = oppositeRolePlayer.PreferredIdentifier;
							bool rolePlayerPreferredIdentifierExists = rolePlayerPreferredIdentifier != null;
							bool oppositeRolePlayerPreferredIdentifierExists = oppositeRolePlayerPreferredIdentifier != null;
							ConceptType conceptType = ConceptTypeHasObjectType.GetConceptType(rolePlayer);
							ConceptType oppositeConceptType = ConceptTypeHasObjectType.GetConceptType(oppositeRolePlayer);

							// The only difference between fully and fact-only absorptions is that fact-only absorptions need existing
							// information types deleted. The rest is the same.
							if (rolePlayerPreferredIdentifierExists)
							{
								absorberId = DoOneToOnePreferredIdentifierAbsorption(rolePlayerPreferredIdentifier,
									liveOIALModel, store, factType);
							}
							if (oppositeRolePlayerPreferredIdentifierExists && absorberId == Guid.Empty)
							{
								absorberId = DoOneToOnePreferredIdentifierAbsorption(oppositeRolePlayerPreferredIdentifier,
									liveOIALModel, store, factType);
							}
							if (absorberId == Guid.Empty)
							{
								absorberId = DoOneToOneAbsorptionForDoubleMandatories(rolePlayer, oppositeRolePlayer, oldAbsorption, liveOIALModel, store,
									mandatoryConstraintRole, oppositeRole);
							}
							// Change the absorption type to Fully and the absorbing object type.
							liveOIALModel.myAbsorbedFactTypes[factType.Id] = new AbsorbedFactType(absorberId, FactAbsorptionType.Fully);
						}
						else if (alethicMandatoryRoleConstraintCount == 0)
						{
							// Do nothing.
						}
						else
						{
							Debug.Fail("Can there legally be more than two simple mandatory constraints on a binary fact type?");
						}
					}
				}
			}
			/// <summary>
			/// Processes a <see cref="T:Neumont.Tools.ORM.ObjectModel.MandatoryConstraint"/> that has been removed from the model.
			/// </summary>
			/// <param name="mandatoryRoleConstraint">The mandatory constraint that was removed from the model.</param>
			/// <param name="newModality">The modality of the opposite rolef</param>
			private static void ProcessElementDeleting(MandatoryConstraint mandatoryRoleConstraint, MandatoryConstraintModality newModality)
			{
				ORMModel model = mandatoryRoleConstraint.Model;
				if (model == null || !mandatoryRoleConstraint.IsSimple)
				{
					return;
				}
				LiveOIALModel liveOIALModel = OIALModelHasORMModel.GetOIALModel(mandatoryRoleConstraint.Model);
				Store store = liveOIALModel.Store;

				LinkedElementCollection<Role> roles = mandatoryRoleConstraint.RoleCollection;
				Debug.Assert(roles.Count == 1, "Role collection count should be 1 for a simple mandatory role constraint.");

				Role mandatoryConstraintRole = roles[0];
				FactType factType = mandatoryConstraintRole.FactType;
				ObjectType rolePlayer = mandatoryConstraintRole.RolePlayer;

				LinkedElementCollection<RoleBase> factRoles = factType.RoleCollection;
				int factRoleCount = factRoles.Count;

				// Simple mandatory role constraints do not affect mapping if the fact type does not have two roles.
				if (factRoleCount == 2)
				{
					LinkedElementCollection<SetConstraint> factSetConstraints = factType.SetConstraintCollection;
					int factSetConstraintCount = factSetConstraints.Count;

					int alethicUniquenessConstraintCount = 0;
					int alethicMandatoryRoleConstraintCount = 0;
					int deonticMandatoryRoleConstraintCount = 0;

					IList<Role> uniqueRoles = new List<Role>();
					bool foundManyToManyUniquenessConstraint = false;
					for (int i = 0; i < factSetConstraintCount; ++i)
					{
						SetConstraint setConstraint = factSetConstraints[i];
						UniquenessConstraint uniquenessConstraint = setConstraint as UniquenessConstraint;
						if (!foundManyToManyUniquenessConstraint && uniquenessConstraint != null &&
							uniquenessConstraint.Modality == ConstraintModality.Alethic)
						{
							if (uniquenessConstraint.FactTypeCollection.Count > 1)
							{
								// Swallow the external constraint
								continue;
							}
							if (uniquenessConstraint.RoleCollection.Count > 1)
							{
								// Many-to-many uniqueness constraint. Do nothing.
								alethicUniquenessConstraintCount = -1;
								foundManyToManyUniquenessConstraint = true;
								continue;
							}
							uniqueRoles.Add(uniquenessConstraint.RoleCollection[0]);
							++alethicUniquenessConstraintCount;
							continue;
						}
						MandatoryConstraint mandatoryConstraint = setConstraint as MandatoryConstraint;
						if (mandatoryConstraint != null)
						{
							if (mandatoryConstraint.Modality == ConstraintModality.Alethic)
							{
								++alethicMandatoryRoleConstraintCount;
							}
							else
							{
								++deonticMandatoryRoleConstraintCount;
							}
						}
					}

					// Many-to-one fact type.
					if (alethicUniquenessConstraintCount == 1)
					{
						Role uniqueRole = uniqueRoles[0];
						if (uniqueRole == mandatoryConstraintRole)
						{
							ChangeConceptTypeChildrenMandatoryModality(ConceptTypeChildHasPathRole.GetConceptTypeChild(uniqueRole.OppositeRole),
								rolePlayer, newModality);
						}
						// If the unique role is not the mandatory constraint role, then no change is necessary.
					}
					else if (alethicUniquenessConstraintCount > 1) // One-to-one fact type.
					{
						Role oppositeRole = mandatoryConstraintRole.OppositeRole.Role;
						ObjectType oppositeRolePlayer = oppositeRole.RolePlayer;
						AbsorbedFactType oldAbsorption;
						bool success = liveOIALModel.myAbsorbedFactTypes.TryGetValue(factType.Id, out oldAbsorption);
						Debug.Assert(success, "Fact type was not absorbed.");

						ConceptType mandatoryConceptType = ConceptTypeHasObjectType.GetConceptType(oppositeRolePlayer);
						ConceptType nonMandatoryConceptType = ConceptTypeHasObjectType.GetConceptType(rolePlayer);

						if (alethicMandatoryRoleConstraintCount == 2)
						{
							Debug.Assert(mandatoryConceptType != null && nonMandatoryConceptType != null,
	"A Concept Type was null for a one-to-one absorption with two mandatory role constraints.");

							int nonMandatoryRolePlayerFunctionalNonDependentRoleCount =
								GetFunctionalNonDependentRoleCount(rolePlayer.PlayedRoleCollection, null);

							if (nonMandatoryRolePlayerFunctionalNonDependentRoleCount == 0)
							{
								if (IsConceptType(liveOIALModel, mandatoryConceptType.ObjectType, factType))
								{
									ConceptTypeAbsorbedConceptType conceptTypeLink =
										ConceptTypeAbsorbedConceptType.GetLinkToAbsorbingConceptType(mandatoryConceptType);
									conceptTypeLink.Mandatory = MandatoryConstraintModality.NotMandatory;
									liveOIALModel.myAbsorbedFactTypes[factType.Id] =
										new AbsorbedFactType(oppositeRolePlayer.Id, FactAbsorptionType.Fully);
								}
								else
								{
									// Absorb fact-only into opposite role player
									RemoveConceptType(nonMandatoryConceptType);
									liveOIALModel.myAbsorbedFactTypes[factType.Id] =
										new AbsorbedFactType(oppositeRolePlayer.Id, FactAbsorptionType.FactOnly);
								}
							}
							else
							{

								AbsorbConceptType(liveOIALModel, mandatoryConceptType.ObjectType,
									nonMandatoryConceptType.ObjectType, MandatoryConstraintModality.Alethic, mandatoryConstraintRole);
								liveOIALModel.myAbsorbedFactTypes[factType.Id] = new AbsorbedFactType(rolePlayer.Id, FactAbsorptionType.Fully);
							}
						}
						else if (alethicMandatoryRoleConstraintCount == 1 || deonticMandatoryRoleConstraintCount == 2)
						{
							int oppositeRolePlayerFunctionalNonDependentRoleCount =
								GetFunctionalNonDependentRoleCount(oppositeRolePlayer.PlayedRoleCollection, null);

							if (oldAbsorption.AbsorptionType == FactAbsorptionType.FactOnly)
							{
								Debug.Assert(oppositeRolePlayerFunctionalNonDependentRoleCount == 0,
									"Absorption was fact only but the non-mandatory role player is playing functional roles.");

								Debug.Assert(oldAbsorption.AbsorberId == rolePlayer.Id,
									"The old absorber of this fact type was not the mandatory role player.");

								ChangeConceptTypeChildrenMandatoryModality(ConceptTypeChildHasPathRole.GetConceptTypeChild(oppositeRole),
									rolePlayer, MandatoryConstraintModality.NotMandatory);
							}
							else
							{
								Debug.Assert(mandatoryConceptType != null && nonMandatoryConceptType != null,
									"A Concept Type was null for a one-to-one absorption with two mandatory role constraints.");

								int rolePlayerFunctionalNonDependentRoleCount = GetFunctionalNonDependentRoleCount(rolePlayer.PlayedRoleCollection, null);
								ConceptType rolePlayerConceptType = ConceptTypeHasObjectType.GetConceptType(rolePlayer);

								if (rolePlayerFunctionalNonDependentRoleCount >= oppositeRolePlayerFunctionalNonDependentRoleCount)
								{
									// Generate ConceptTypeRef from rolePlayer to oppositeRolePlayer
									LinkedElementCollection<ConceptTypeChild> conceptTypeChildren =
										ConceptTypeChildHasPathRole.GetConceptTypeChild(mandatoryConstraintRole);
									Debug.Assert(conceptTypeChildren.Count == 1, "Should have only one concept type child.");
									if (IsConceptType(liveOIALModel, rolePlayer, factType))
									{
										ReplaceConceptTypeChildrenWithConceptTypeRef(rolePlayer.PlayedRoleCollection, rolePlayerConceptType,
											null);
									}
									else
									{
										RemoveConceptType(rolePlayerConceptType);
									}
									liveOIALModel.myAbsorbedFactTypes[factType.Id] = new AbsorbedFactType(rolePlayer.Id, FactAbsorptionType.FactOnly);
								}
								else
								{
									// Remove ConceptType for rolePlayer and change the Fact Type Absorptions
									RemoveConceptType(rolePlayerConceptType);
									liveOIALModel.myAbsorbedFactTypes[factType.Id] =
										new AbsorbedFactType(oppositeRolePlayer.Id, FactAbsorptionType.FactOnly);
								}
							}
						}
					}
				}
			}
		}
	}
}
