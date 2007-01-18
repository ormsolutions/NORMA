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
		#region Uniqueness Constraint Rules
		/// <summary>
		/// Encapsulates functions common to handling UniquenessConstraint rules.
		/// </summary>
		private sealed partial class UniquenessConstraintRule
		{
			#region Add
			/// <summary>
			/// A rule for changes made to model that occur when adding a uniqueness constraint
			/// </summary>
			[RuleOn(typeof(UniquenessConstraint))]
			private sealed partial class UniquenessConstraintAddRule : AddRule
			{
				/// <summary>
				/// This rule is called when a uniqueness constraint is added to the model
				/// </summary>
				/// <param name="e"><see cref="T:Microsoft.VisualStudio.Modeling.ElementAddedEventArgs" /></param>
				public override void ElementAdded(ElementAddedEventArgs e)
				{
					ORMCoreDomainModel.DelayValidateElement(e.ModelElement, ProcessElement);
				}
				/// <summary>
				/// LiveOial process for adding an internal uniqueness constraint
				/// corresponds to documentation 'Internal Uniqueness Constraints' part I
				/// </summary>
				/// <param name="element">The current element from the model, in this case a uniqueness constraint</param>
				private static void ProcessElement(ModelElement element)
				{
					UniquenessConstraint uniquenessConstraint = element as UniquenessConstraint;
					LiveOIALModel live = OIALModelHasORMModel.GetOIALModel(uniquenessConstraint.Model);
					Store store = uniquenessConstraint.Store;
					LinkedElementCollection<Role> uniquenessConstraintRole = uniquenessConstraint.RoleCollection;
					Role uniquenessRole = uniquenessConstraintRole[0];
					Role oppositeRole = uniquenessRole.OppositeRole.Role;
					FactType factType = uniquenessRole.FactType;
					ObjectType rolePlayer = uniquenessRole.RolePlayer;
					ObjectType oppositeRolePlayer = oppositeRole.RolePlayer;
					MandatoryConstraintModality constraintModality = GetMandatoryModality(uniquenessRole);
					MandatoryConstraintModality oppositeConstraintModality = GetMandatoryModality(oppositeRole);

					int uniquenessConstraintRoleCount = uniquenessConstraint.RoleCollection.Count;
					int roleCount = factType.RoleCollection.Count;
					int uniquenessConstraintCount = GetAlethicInternalConstraintsCount(factType, ConstraintType.InternalUniqueness);

					//Cases One and Four
					//Do this if the fact type is unary
					if (roleCount == 1)
					{
						//Case One
						return;
					}

					//Cases Two and Three
					//Do this if the fact type is binary and has no uc
					if (roleCount == 2 && uniquenessConstraintCount == 1)
					{
						//Case Two
						if (uniquenessConstraintRoleCount == 1 && roleCount == 1)
						{
							bool shouldResolveRoles = true;
							ConceptType containingConceptType = ConceptTypeHasObjectType.GetConceptType(rolePlayer);
							if (containingConceptType == null)
							{
								shouldResolveRoles = false;
								containingConceptType = CreateConceptType(live, store, rolePlayer, null);
							}

							//if uniqueness constraint added spans only one role check is opposite is a CT
							if (IsConceptType(live, oppositeRolePlayer, factType))
							{
								//Create a Concept type
								GenerateConceptTypeRef(rolePlayer, oppositeRolePlayer, oppositeRole, null);
							}
							else
							{
								//Create Information Type
								GenerateInformationType(live, containingConceptType, oppositeRolePlayer, oppositeRole, null);
							}

							if (shouldResolveRoles)
							{
								ResolveFactTypeAbsorptionsAfterChangingFunctionalRoles(live, containingConceptType, uniquenessRole);
							}
						}
						//Case Three if uniqueness constraint added that spans both roles
						if (uniquenessConstraintRoleCount == 2 && uniquenessConstraintCount == 1)
						{
							//A concept type must be generated for the object type behind the fact type. 
							//For each of the implied facts, do Case Two, but remember there is no need to 
							//re-interpret one-to-one fact type absorption on the new concept type because it 
							//has just been created. Finally, create a Child Sequence Uniqueness Constraint 
							//that references A and B.

							ConceptType compositeConceptType =
								CreateConceptType(live, store, factType.Objectification.NestingType, null);
						}
					}

					//Case Four
					if (uniquenessConstraintRoleCount == 1 && uniquenessConstraintCount == 2)
					{
						//Count the number of mandatory constraints on the binary fact type. 
						//Depending on the number of mandatory constraints on the fact type, 
						//calculate which object type wins the absorption and record the result 
						//in FactTypeAbsorptions. 
						//Make sure to remove the Information Type or the Concept Type Ref that existed previously.

						AbsorbedFactType absorbedFactType = live.CalculateAbsorptionForOneToOneFactType(factType);

						if (absorbedFactType.AbsorptionType == FactAbsorptionType.Fully)
						{
							if (absorbedFactType.AbsorberId == rolePlayer.Id)
							{
								AbsorbConceptType(live, oppositeRolePlayer, rolePlayer, constraintModality, oppositeRole);
							}

							else
							{
								AbsorbConceptType(live, rolePlayer, oppositeRolePlayer, oppositeConstraintModality, uniquenessRole);
							}
						}
						else
						{
							ConceptTypeChildHasPathRole.GetConceptTypeChild(uniquenessRole).Clear();

							if (IsConceptType(live, rolePlayer, factType))
							{
								//Remove CT and generate a CTR
								GenerateConceptTypeRef(rolePlayer, oppositeRolePlayer, oppositeRole, null);
							}
							else
							{
								//Remove information type for the opposite role player
								ConceptType containingConceptType = ConceptTypeHasObjectType.GetConceptType(rolePlayer);
								GenerateInformationType(live, containingConceptType, oppositeRolePlayer, oppositeRole, null);
							}
						}
						if (absorbedFactType != AbsorbedFactType.Empty)
						{
							live.myAbsorbedFactTypes[factType.Id] = absorbedFactType;
						}
					}
				}
			}
			#endregion	// Add
			#region Deleting
			/// <summary>
			/// Handles changes to the OIAL Model when a
			/// <see cref="T:Neumont.Tools.ORM.ObjectModel.UniquenessConstraint" /> is deleted from the ORM Model.
			/// </summary>
			[RuleOn(typeof(UniquenessConstraint))]
			private sealed partial class UniquenessConstraintDeletingRule : DeletingRule
			{
				private LinkedElementCollection<Role> myConstraintRoleCollection;
				private Store myStore;
				private LiveOIALModel myOialModel;

				/// <summary>
				/// Processes changes to the OIAL Model when a UniquenessConstraint is deleted from the ORM Model.
				/// </summary>
				/// <param name="e"><see cref="T:Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs" /></param>
				public sealed override void ElementDeleting(ElementDeletingEventArgs e)
				{
					UniquenessConstraint uniquenessConstraint = e.ModelElement as UniquenessConstraint;
					Debug.Assert(uniquenessConstraint != null, "Element was not of the expected type when executing a rule.");

					// HACK: The rule is getting called twice per delete.  This keeps us from processing it the second time.
					if (uniquenessConstraint.Model != null)
					{
						myOialModel = OIALModelHasORMModel.GetOIALModel(uniquenessConstraint.Model);
						myStore = myOialModel.Store;

						myConstraintRoleCollection = uniquenessConstraint.RoleCollection;
						int constraintRoleCount = myConstraintRoleCollection.Count;

						#region Single Role Uniqueness Constraint
						// If constraint spans a single role...
						if (constraintRoleCount == 1)
						{
							ProcessSingleRoleUC();
						}
						#endregion // Single Role Uniqueness Constraint
					}
				}

				private void ProcessSingleRoleUC()
				{
					Role role = myConstraintRoleCollection[0];
					FactType factType = role.FactType;
					LinkedElementCollection<RoleBase> factTypeRoleCollection = factType.RoleCollection;
					int factTypeRoleCount = factTypeRoleCollection.Count;

					#region Binary
					// If fact type is binary...
					if (factTypeRoleCount == 2)
					{
						ProcessBinarySingleRole(role, factType);
					}
					#endregion // Binary
					#region Unary
					else if (factTypeRoleCount == 1) // ...unary...
					{
						// Do nothing.
					}
					#endregion // Unary
					#region N-ary
					else // ...n-ary...
					{
						Debug.Fail("It is a model error to have a single role uniqueness constraint on an N-ary fact type.");
					}
					#endregion // N-ary
				}

				private void ProcessBinarySingleRole(Role role, FactType factType)
				{
					ObjectType rolePlayer = role.RolePlayer;
					int internalUniquenessCount = GetAlethicInternalConstraintsCount(factType, ConstraintType.InternalUniqueness);
					Guid rolePlayerId = rolePlayer.Id;
					ConceptType conceptType = ConceptTypeHasObjectType.GetConceptType(rolePlayer);
					Guid factTypeId = factType.Id;

					Role oppositeRole = role.OppositeRole.Role;
					ObjectType oppositeRolePlayer = oppositeRole.RolePlayer;
					Guid oppositeRolePlayerId = oppositeRolePlayer.Id;
					ConceptType oppositeConceptType = ConceptTypeHasObjectType.GetConceptType(oppositeRolePlayer);

					#region One-to-many
					// If internal uniqueness constraint pattern for fact type is one-to-many...
					if (internalUniquenessCount == 1)
					{
						ProcessBinaryOneToMany(conceptType, oppositeRole);
					}
					#endregion // One-to-many
					#region One-to-one
					else if (internalUniquenessCount == 2) // ...one-to-one...
					{
						ProcessBinaryOneToOne(role, rolePlayer, rolePlayerId, conceptType, factTypeId, oppositeRolePlayer, oppositeRolePlayerId, oppositeConceptType);
					}
					#endregion
					#region Other
					else // ...other...
					{
						Debug.Fail("The internal uniqueness count on a binary fact type is not between 1 and 2.");
					}
					#endregion // Other
				}

				private void ProcessBinaryOneToOne(Role role, ObjectType rolePlayer, Guid rolePlayerId, ConceptType conceptType, 
					Guid factTypeId, ObjectType oppositeRolePlayer, Guid oppositeRolePlayerId, ConceptType oppositeConceptType)
				{
					AbsorbedFactType absorption = myOialModel.myAbsorbedFactTypes[factTypeId];
					Guid absorberId = absorption.AbsorberId;
					FactAbsorptionType absorptionType = absorption.AbsorptionType;

					#region Two Concept Types
					// If there are two concept types...
					if (conceptType != null && oppositeConceptType != null)
					{
						#region oppositeRolePlayer absorbing (Fact Only)
						// If oppositeRolePlayer is absorbing factType (fact only)...
						if (absorberId == oppositeRolePlayerId && absorptionType == FactAbsorptionType.FactOnly)
						{
							// remove ConceptTypeReference to conceptType in oppositeConceptType
							oppositeConceptType.ReferencedConceptTypeCollection.Remove(conceptType);

							// create ConceptTypeReference to oppositeConceptType in conceptType
							conceptType.ReferencedConceptTypeCollection.Add(oppositeConceptType);
						}
						#endregion // oppositeRolePlayer absorbing (Fact Only)
						#region rolePlayer absorbing (Fact Only)
						else if (absorberId == rolePlayerId && absorptionType == FactAbsorptionType.FactOnly) // ...rolePlayer...(fact only)...
						{
							// TODO: remove SingleChildUniquenessConstraint for ConceptTypeReference to conceptType in oppositeConceptType
						}
						#endregion // rolePlayer absorbing (Fact Only)
						#region rolePlayer absorbing (Fully)
						else if (absorberId == rolePlayerId && absorptionType == FactAbsorptionType.Fully) // ...rolePlayer...(fully)...
						{
							LinkedElementCollection<Role> rolePlayerPlayedRoles = rolePlayer.PlayedRoleCollection;
							int rolePlayerFunctionRoleCount = GetFunctionalRoleCount(rolePlayerPlayedRoles, role);
							MandatoryConstraintModality roleMandatoryModality = GetMandatoryModality(role);

							// remove the Absorbed Concept Type of conceptType from oppositeConceptType
							oppositeConceptType.AbsorbedConceptTypeCollection.Remove(conceptType);

							// If rolePlayer plays other functional roles...
							if (rolePlayerFunctionRoleCount > 0)
							{
								// TODO: keep the Concept Type for A and add a Concept Type Reference to A in B.
							}
							else // ...does not play...
							{
								// TODO: delete the Concept Type and add an Information Type for A in B.
							}

							if (roleMandatoryModality == MandatoryConstraintModality.Alethic)
							{
								// TODO: set the relationship’s Mandatory value to MandatoryConstraintModality.Alethic.
							}
						}
						#endregion // rolePlayer absorbing (Fully)
						#region Other
						else
						{
							Debug.Fail("A supposed-to-be impossible case occured.");
						}
						#endregion // Other
					}
					#endregion // Two Concept Types
					#region One Concept Type (Fact Only absorbtion)
					else if (conceptType != null || oppositeConceptType != null && absorptionType == FactAbsorptionType.FactOnly) // ...one concept type...(fact only)...
					{
						#region rolePlayer absorbing
						// If rolePlayer is absorbing factType...
						if (absorberId == rolePlayerId)
						{
							Debug.Assert(conceptType == null, "conceptType was expected to be null.");

							// create a Concept Type for oppositeRolePlayer
							oppositeConceptType = CreateConceptType(myOialModel, myStore, oppositeRolePlayer, null);

							// add a ConceptTypeReference to conceptType in oppositeConceptType
							oppositeConceptType.ReferencedConceptTypeCollection.Add(conceptType);
						}
						#endregion // rolePlayer absorbing
						#region oppositeRolePlayer absorbing
						else if (absorberId == oppositeRolePlayerId) // ...oppositeRolePlayer...
						{
							Debug.Assert(oppositeConceptType == null, "oppositeConceptType was expected to be null.");

							// TODO: remove SingleChildUniquenessConstraint for the InformationType of rolePlayer in oppositeConceptType.
						}
						#endregion // oppositeRolePlayer absorbing
						#region Other
						else
						{
							Debug.Fail("A one-to-one binary factType is not being absorbed into either role player.");
						}
						#endregion // Other
					}
					#endregion // One Concept Type (Fact Only absorbtion)
					#region Other
					else // ...no concept types...
					{
						Debug.Fail("A one to one fact type was not being absorbed.");
					}
					#endregion // Other

					// TODO: update model's absorbtion map

					// TODO: check absorbtion of one-to-one facts played by rolePlayer
					// REMOVED: ResolveFactTypeAbsorptionsAfterChangingFunctionalRoles(oialModel, conceptType, role);
				}

				private void ProcessBinaryOneToMany(ConceptType conceptType, Role oppositeRole)
				{
					LinkedElementCollection<ConceptTypeChild> conceptTypeChildren = ConceptTypeChildHasPathRole.GetConceptTypeChild(oppositeRole);
					int conceptTypeChildCount = conceptTypeChildren.Count;

					// For each potential child...
					for (int i = conceptTypeChildCount - 1; i >= 0; i--)
					{
						ConceptTypeChild conceptTypeChild = conceptTypeChildren[i];
						ConceptType parent = conceptTypeChild.Parent;

						// If this is the one we're looking for...
						if (parent == conceptType)
						{
							// delete it
							conceptTypeChild.Delete();
						}
					}

					// TODO: delete child sequence constraints

					// TODO: check absorbtion of one-to-one facts played by rolePlayer
					// REMOVED: ResolveFactTypeAbsorptionsAfterChangingFunctionalRoles(oialModel, conceptType, role);
				}
			}
			#endregion // Deleting
			#region Change
			/// <summary>
			/// Handles changes to the OIAL Model when a
			/// <see cref="T:Neumont.Tools.ORM.ObjectModel.UniquenessConstraint" /> is changed on the ORM Model.
			/// </summary>
			[RuleOn(typeof(UniquenessConstraint))]
			private sealed partial class UniquenessConstraintChangeRule : ChangeRule
			{
				/// <summary>
				/// Processes changes to the OIAL Model when a UniquenessConstraint is changed on the ORM Model.
				/// </summary>
				/// <param name="e"><see cref="T:Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs" /></param>
				public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
				{
					UniquenessConstraint uniquenessConstraint = e.ModelElement as UniquenessConstraint;
					Guid attributeGuid = e.DomainProperty.Id;
					Debug.Assert(uniquenessConstraint != null, "Element was not of the expected type when executing a rule.");

					if (attributeGuid == UniquenessConstraint.IsPreferredDomainPropertyId)
					{
						bool isPreferred = (bool)e.NewValue;

						// UNDONE: This case might be handled by EntityTypeHasPreferredIdentifier Add/Remove instead.

						if (isPreferred)
						{
							// Now a preferred identifier for some ObjectType.
							// Change the single child/child sequence uniqueness constraint to be preferred.
							// Re-evaluate is ignored.
						}
						else
						{
							// Now lost a preferred identifier for this ObjectType.
							// Change the single child/child sequence uniqueness constraint to be preferred.
							// Set IsIgnored to false.
						}
					}
					else if (attributeGuid == UniquenessConstraint.ModalityDomainPropertyId)
					{
						ConstraintModality newModality = (ConstraintModality)e.NewValue;

						if (newModality == ConstraintModality.Alethic)
						{
							// Modality was changed from Deontic to Alethic. Simulate Add
						}
						else
						{
							// Modality was changed from Alethic to Deontic. Simulate Remove.
						}
					}
					else if (attributeGuid == UniquenessConstraint.NameDomainPropertyId)
					{
						// Change the name of the corresponding constraint in OIAL if it exists.
					}
				}
			}
			#endregion	// Change
		}
		#endregion // Uniqueness Constraint Rules
	}
}
