using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.Modeling;
using System.Diagnostics;
using System.IO;
using System.Collections;
using System.Threading;
using Neumont.Tools.ORMAbstraction;

namespace Neumont.Tools.ORMToORMAbstractionBridge
{
	#region OIAL support code
	enum OialModelElementAction
	{
		Add = 1,
		Change = 2,
		Delete = 4,
	}
	#endregion
	public partial class AbstractionModelIsForORMModel
	{
		#region ModelElement transaction support
		private static object Key = new object();

		private static Dictionary<ModelElement, int> EnsureContextKeyExists(Store store)
		{
			Dictionary<object, object> contextDictionary = store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
			object elementList = null;

			if (!contextDictionary.TryGetValue(Key, out elementList))
			{
				elementList = new Dictionary<ModelElement, int>();
				contextDictionary.Add(Key, elementList);
			}
			return (Dictionary<ModelElement, int>)elementList;
		}

		private static bool IsValidConstraintType(ConstraintRoleSequenceHasRole constraintSequence, ref IConstraint constraint)
		{
			if ((constraint = constraintSequence.ConstraintRoleSequence.Constraint) == null)
			{
				// This is not a constraint we care about.
				return false;
			}

			ConstraintType type = constraint.ConstraintType;
			return (type == ConstraintType.InternalUniqueness || type == ConstraintType.SimpleMandatory);
		}

		private static void AddTransactedModelElement(ModelElement element, OialModelElementAction action)
		{
			Dictionary<ModelElement, int> elementList;
			elementList = EnsureContextKeyExists(element.Store);

			if (elementList.ContainsKey(element))
			{
				elementList[element] = (elementList[element] | (int)action);
			}
			else
			{
				elementList.Add(element, (int)action);
			}
		}
		#endregion
		#region Model validation
		/// <summary>
		/// Delays the validate model.
		/// </summary>
		/// <param name="element">The element.</param>
		public static void DelayValidateModel(ModelElement element)
		{
			ORMModel model = (ORMModel)element;
			Dictionary<object, object> contextDictionary = element.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;

			if (contextDictionary.ContainsKey(Key))
			{
				// Get the elements affected within the transaction
				Dictionary<ModelElement, int> elementList = (Dictionary<ModelElement, int>)contextDictionary[Key];

				// Elements that were both added & deleted in the transaction can be ignored for map change analysis
				EliminateRedundantElements(elementList);

				// TODO: scan for changes that actually affect the model; all others can be filtered out

				if (elementList.Count == 0)
				{
					return;
				}
			}

			// Get the link from the given ORMModel
			AbstractionModelIsForORMModel oialModelIsForORMModel = AbstractionModelIsForORMModel.GetLinkToAbstractionModel(model);

			// If the link exists...
			if (oialModelIsForORMModel != null)
			{
				AbstractionModel oialModel;
				if (null != (oialModel = oialModelIsForORMModel.AbstractionModel))
				{
					oialModel.Delete();
				}

				// Delete it.
				oialModelIsForORMModel.Delete();
			}

			AbstractionModel oial = new AbstractionModel(model.Store);
			oialModelIsForORMModel = new AbstractionModelIsForORMModel(oial, model);

			// Apply ORM to OIAL algorithm
			oialModelIsForORMModel.TransformORMtoOial();
		}

		/// <summary>
		/// Removes elements from the list that were both created and deleted in the transaction.
		/// </summary>
		/// <param name="elementList">The list of elements that were affected by the transaction.</param>
		private static void EliminateRedundantElements(Dictionary<ModelElement, int> elementList)
		{
			foreach (ModelElement element in elementList.Keys)
			{
				int actions = elementList[element];
				if ((actions & (int)OialModelElementAction.Add) != 0 && (actions & (int)OialModelElementAction.Delete) != 0)
				{
					elementList.Remove(element);
				}
			}
		}
		#endregion
		#region Deserialization FixupListeners
		/// <summary>
		/// This accessor returns an <see cref="OialObjectTypeFixupListener"/>.
		/// </summary>
		public static IDeserializationFixupListener ORMModelHasObjectTypeFixupListener
		{
			get { return new OialObjectTypeFixupListener(); }
		}

		/// <summary>
		/// This class is the <see cref="ObjectType"/> fixuplistener.
		/// We listen for <see cref="ObjectType"/> objects as they deserialize and DelayValidate the Model.
		/// </summary>
		private sealed class OialObjectTypeFixupListener : DeserializationFixupListener<ObjectType>
		{
			/// <summary>
			/// <see cref="OialObjectTypeFixupListener"/> Constructor.
			/// </summary>
			public OialObjectTypeFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateImplicitStoredElements)
			{
			}

			/// <summary>
			/// When an <see cref="ObjectType"/> is deserialized we DelayValidate the Model.
			/// </summary>
			protected sealed override void ProcessElement(ObjectType element, Store store, INotifyElementAdded notifyAdded)
			{
				ObjectType objectType = element as ObjectType;
				ORMModel model = objectType.Model;
				AbstractionModel oil = AbstractionModelIsForORMModel.GetAbstractionModel(model);
				if (oil == null)
				{
					oil = new AbstractionModel(store);
					AbstractionModelIsForORMModel oialModelIsForORMModel = new AbstractionModelIsForORMModel(oil, model);
					notifyAdded.ElementAdded(oil, true);
				}
				ORMCoreDomainModel.DelayValidateElement(objectType.Model, DelayValidateModel);
			}
		}

		/// <summary>
		/// This accessor returns an <see cref="OialFactTypeFixupListener"/>.
		/// </summary>
		public static IDeserializationFixupListener ORMModelHasFactTypeFixupListener
		{
			get { return new OialFactTypeFixupListener(); }
		}

		/// <summary>
		/// This FixupListener listens for <see cref="FactType"/> objects that are being Deserialized.
		/// </summary>
		private sealed class OialFactTypeFixupListener : DeserializationFixupListener<FactType>
		{
			/// <summary>
			/// <see cref="OialFactTypeFixupListener"/> Constructor.
			/// </summary>
			public OialFactTypeFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateImplicitStoredElements)
			{
			}

			/// <summary>
			/// When a <see cref="FactType"/> is deserialized we DelayValidate the Model.
			/// </summary>
			protected sealed override void ProcessElement(FactType element, Store store, INotifyElementAdded notifyAdded)
			{
				FactType fact = element as FactType;
				ORMModel model = fact.Model;
				AbstractionModel oil = AbstractionModelIsForORMModel.GetAbstractionModel(model);
				if (oil == null)
				{
					oil = new AbstractionModel(store);
					AbstractionModelIsForORMModel oialModelIsForORMModel = new AbstractionModelIsForORMModel(oil, model);
					notifyAdded.ElementAdded(oil, true);
				}
				ORMCoreDomainModel.DelayValidateElement(model, DelayValidateModel);
			}
		}

		/// <summary>
		/// This accessor returns an <see cref="OialModelHasSetConstraintFixupListener"/>.
		/// </summary>
		public static IDeserializationFixupListener ORMModelModelHasSetConstraintFixupListener
		{
			get { return new OialModelHasSetConstraintFixupListener(); }
		}

		/// <summary>
		/// This FixupListener listens for <see cref="SetConstraint"/> objects during deserialization.
		/// </summary>
		private sealed class OialModelHasSetConstraintFixupListener : DeserializationFixupListener<ModelHasSetConstraint>
		{
			/// <summary>
			/// <see cref="OialModelHasSetConstraintFixupListener"/> Constructor.
			/// </summary>
			public OialModelHasSetConstraintFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateImplicitStoredElements)
			{
			}

			/// <summary>
			/// When a <see cref="SetConstraint"/> is deserialized we DelayValidate the Model.
			/// </summary>
			protected sealed override void ProcessElement(ModelHasSetConstraint element, Store store, INotifyElementAdded notifyAdded)
			{
				ModelHasSetConstraint setConstraint = element as ModelHasSetConstraint;
				ORMModel model = setConstraint.Model;
				AbstractionModel oil = AbstractionModelIsForORMModel.GetAbstractionModel(model);
				if (oil == null)
				{
					oil = new AbstractionModel(store);
					AbstractionModelIsForORMModel oialModelIsForORMModel = new AbstractionModelIsForORMModel(oil, model);
					notifyAdded.ElementAdded(oil, true);
				}
				ORMCoreDomainModel.DelayValidateElement(model, DelayValidateModel);
			}
		}
		#endregion // Deserialization FixupListeners
		#region ORM to OIAL Algorithm Methods
		/// <summary>
		/// Transforms the data in <see cref="ORMModel"/> into this <see cref="AbstractionModel"/>.
		/// </summary>
		private void TransformORMtoOial()
		{
			ORMModel model = this.ORMModel;
			FactTypeMappingDictionary decidedFactTypeMappings = null;
			FactTypeMappingListDictionary undecidedFactTypeMappings = null;
			LinkedElementCollection<FactType> modelFactTypes = model.FactTypeCollection;

			InitialFactTypeMappings(modelFactTypes, out decidedFactTypeMappings, out undecidedFactTypeMappings);
			FilterFactTypeMappings(decidedFactTypeMappings, undecidedFactTypeMappings);

			LiveOialPermuter permuter = new LiveOialPermuter(decidedFactTypeMappings, undecidedFactTypeMappings);
			permuter.Run();
			// smallestPermutationsList now contains those permutations which result in the smallest number of possible top-level concept types
			FinalMappingStateList smallestPermutationsList = permuter.SmallestPermutationsList;

			// TODO: if more than one possible permutation, further decide.
			Debug.Assert(smallestPermutationsList.Count == 1, "There was more than one item in smallestPermutationsList.  We do not currently have a process for further decision.");

			FinalMappingState mapstate = smallestPermutationsList[0];
			foreach (DecidedMappingStateEntry entry in mapstate.Mappings)
			{
				decidedFactTypeMappings.Add(entry.FactType, entry.Mapping);
			}

			GenerateOialModel(decidedFactTypeMappings);
		}

		/// <summary>
		/// Determines the obvious fact type mappings, and all other potential mappings.
		/// </summary>
		/// <param name="modelFactTypes">The <see cref="FactType"/> objects of the model</param>
		/// <param name="decidedFactTypeMappings">The decided <see cref="FactTypeMapping"/> objects.</param>
		/// <param name="undecidedFactTypeMappings">The undecided <see cref="FactTypeMapping"/> possibilities.</param>
		private void InitialFactTypeMappings(IList<FactType> modelFactTypes, out FactTypeMappingDictionary decidedFactTypeMappings, out FactTypeMappingListDictionary undecidedFactTypeMappings)
		{
			decidedFactTypeMappings = new FactTypeMappingDictionary();
			undecidedFactTypeMappings = new FactTypeMappingListDictionary();

			// For each fact type in the model...
			foreach (FactType factType in modelFactTypes)
			{
				SubtypeFact subtypeFact = factType as SubtypeFact;

				// If it's a subtype relation...
				if (subtypeFact != null)
				{
					Role subtypeRole = subtypeFact.SubtypeRole;
					Role supertypeRole = subtypeFact.SupertypeRole;

					// Map deeply toward the supertype.
					FactTypeMapping factTypeMapping = new FactTypeMapping(subtypeFact, subtypeRole, supertypeRole, FactTypeMappingType.Deep);

					decidedFactTypeMappings.Add(subtypeFact, factTypeMapping);
				}
				else
				{
					LinkedElementCollection<RoleBase> roles = factType.RoleCollection;
					int roleCount = roles.Count;

					// If it's a binary fact type...
					if (roleCount == 2)
					{
						// If factType is implicitly or explicity objectified...
						if (factType.Objectification != null)
						{
							continue;
						}

						Role firstRole = roles[0].Role;
						Role secondRole = roles[1].Role;
						ObjectType firstRolePlayer = firstRole.RolePlayer;
						ObjectType secondRolePlayer = secondRole.RolePlayer;
						UniquenessConstraint firstRoleUniquenessConstraint = (UniquenessConstraint)firstRole.SingleRoleAlethicUniquenessConstraint;
						UniquenessConstraint secondRoleUniquenessConstraint = (UniquenessConstraint)secondRole.SingleRoleAlethicUniquenessConstraint;
						bool firstRoleIsUnique = (firstRoleUniquenessConstraint != null);
						bool secondRoleIsUnique = (secondRoleUniquenessConstraint != null);

						// If only firstRole is unique...
						if (firstRoleIsUnique && !secondRoleIsUnique)
						{
							// Shallow map toward firstRolePlayer.
							FactTypeMapping factTypeMapping = new FactTypeMapping(factType, secondRole, firstRole, FactTypeMappingType.Shallow);

							decidedFactTypeMappings.Add(factType, factTypeMapping);
						}
						else if (!firstRoleIsUnique && secondRoleIsUnique) // ...only secondRole is unique...
						{
							// Shallow map toward secondRolePlayer.
							FactTypeMapping factTypeMapping = new FactTypeMapping(factType, firstRole, secondRole, FactTypeMappingType.Shallow);

							decidedFactTypeMappings.Add(factType, factTypeMapping);
						}
						else if (firstRoleIsUnique && secondRoleIsUnique) // ...both roles are unique...
						{
							MandatoryConstraint firstRoleMandatoryConstraint = firstRole.SimpleMandatoryConstraint;
							MandatoryConstraint secondRoleMandatoryConstraint = secondRole.SimpleMandatoryConstraint;
							bool firstRoleIsMandatory = (firstRoleMandatoryConstraint != null && firstRoleMandatoryConstraint.Modality == ConstraintModality.Alethic);
							bool secondRoleIsMandatory = (secondRoleMandatoryConstraint != null && secondRoleMandatoryConstraint.Modality == ConstraintModality.Alethic);

							// If this is a ring fact type...
							if (firstRolePlayer.Id == secondRolePlayer.Id)
							{
								// If only firstRole is mandatory...
								if (firstRoleIsMandatory && !secondRoleIsMandatory)
								{
									// Shallow map toward firstRolePlayer (mandatory role player).
									FactTypeMapping factTypeMapping = new FactTypeMapping(factType, firstRole, secondRole, FactTypeMappingType.Shallow);

									decidedFactTypeMappings.Add(factType, factTypeMapping);
								}
								else if (!firstRoleIsMandatory && secondRoleIsMandatory) // ...only secondRole is mandatory...
								{
									// Shallow map toward secondRolePlayer (mandatory role player).
									FactTypeMapping factTypeMapping = new FactTypeMapping(factType, secondRole, firstRole, FactTypeMappingType.Shallow);

									decidedFactTypeMappings.Add(factType, factTypeMapping);
								}
								else // ...otherwise...
								{
									// Shallow map toward firstRolePlayer.
									FactTypeMapping factTypeMapping = new FactTypeMapping(factType, firstRole, secondRole, FactTypeMappingType.Shallow);

									decidedFactTypeMappings.Add(factType, factTypeMapping);
								}
							}
							else // ...not a ring fact type...
							{
								FactTypeMappingList potentialFactTypeMappings = new FactTypeMappingList();

								// If neither role is mandatory...
								if (!(firstRoleIsMandatory || secondRoleIsMandatory))
								{
									// Shallow map toward firstRolePlayer.
									potentialFactTypeMappings.Add(new FactTypeMapping(factType, secondRole, firstRole, FactTypeMappingType.Shallow));
									// Shallow map toward secondRolePlayer.
									potentialFactTypeMappings.Add(new FactTypeMapping(factType, firstRole, secondRole, FactTypeMappingType.Shallow));
								}
								else if (firstRoleIsMandatory && !secondRoleIsMandatory) // ...only firstRole is mandatory...
								{
									// Shallow map toward firstRolePlayer.
									potentialFactTypeMappings.Add(new FactTypeMapping(factType, secondRole, firstRole, FactTypeMappingType.Shallow));
									// Deep map toward secondRolePlayer.
									potentialFactTypeMappings.Add(new FactTypeMapping(factType, firstRole, secondRole, FactTypeMappingType.Deep));
								}
								else if (!firstRoleIsMandatory && secondRoleIsMandatory) // ...only secondRole is mandatory...
								{
									// Deep map toward firstRolePlayer.
									potentialFactTypeMappings.Add(new FactTypeMapping(factType, secondRole, firstRole, FactTypeMappingType.Deep));
									// Shallow map toward secondRolePlayer.
									potentialFactTypeMappings.Add(new FactTypeMapping(factType, firstRole, secondRole, FactTypeMappingType.Shallow));
								}
								else // ...both roles are mandatory...
								{
									bool firstRoleIsUniqueAndPreferred = firstRoleIsUnique && firstRoleUniquenessConstraint.IsPreferred;
									bool secondRoleIsUniqueAndPreferred = secondRoleIsUnique && secondRoleUniquenessConstraint.IsPreferred;

									// If firstRole is not preferred...
									if (!firstRoleIsUniqueAndPreferred)
									{
										// Shallow map toward firstRolePlayer.
										potentialFactTypeMappings.Add(new FactTypeMapping(factType, secondRole, firstRole, FactTypeMappingType.Shallow));
									}

									// If seccondRole is not preferred...
									if (!secondRoleIsUniqueAndPreferred)
									{
										// Shallow map toward secondRolePlayer.
										potentialFactTypeMappings.Add(new FactTypeMapping(factType, firstRole, secondRole, FactTypeMappingType.Shallow));
									}

									// Deep map toward firstRolePlayer.
									potentialFactTypeMappings.Add(new FactTypeMapping(factType, secondRole, firstRole, FactTypeMappingType.Deep));
									// Deep map toward secondRolePlayer.
									potentialFactTypeMappings.Add(new FactTypeMapping(factType, firstRole, secondRole, FactTypeMappingType.Deep));
								}

								undecidedFactTypeMappings.Add(factType, potentialFactTypeMappings);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Runs various algorithms on the undecided fact type mappings in an attempt to decide as many as possible.
		/// </summary>
		/// <param name="decidedFactTypeMappings">The decided <see cref="FactTypeMapping"/> objects.</param>
		/// <param name="undecidedFactTypeMappings">The undecided <see cref="FactTypeMapping"/> possibilities.</param>
		private void FilterFactTypeMappings(FactTypeMappingDictionary decidedFactTypeMappings, FactTypeMappingListDictionary undecidedFactTypeMappings)
		{
			RemoveImpossiblePotentialFactTypeMappings(decidedFactTypeMappings, undecidedFactTypeMappings);

			int changeCount = 0;
			do
			{
				changeCount = MapTrivialOneToOneFactTypesWithTwoMandatories(decidedFactTypeMappings, undecidedFactTypeMappings);
			} while (changeCount > 0);
		}

		/// <summary>
		/// Populates this <see cref="AbstractionModel"/> given the decided <see cref="FactTypeMapping"/> objects.
		/// </summary>
		/// <param name="factTypeMappings">The decided <see cref="FactTypeMapping"/> objects.</param>
		private void GenerateOialModel(FactTypeMappingDictionary factTypeMappings)
		{
			GenerateInformationTypeFormats();
			GenerateConceptTypes(factTypeMappings);
			GenerateConceptTypeChildren(factTypeMappings);
			GenerateFactTypeMappings(factTypeMappings);
			GenerateUniqueness(factTypeMappings);
			GenerateAssociations();
		}
		#region Filter Algorithms Methods
		/// <summary>
		/// Filters out deep potential mappings that are from an <see cref="ObjectType"/> that already has a decided deep
		/// mapping from it.
		/// </summary>
		/// <param name="decidedFactTypeMappings">The decided <see cref="FactTypeMapping"/> objects.</param>
		/// <param name="undecidedFactTypeMappings">The undecided <see cref="FactTypeMapping"/> possibilities.</param>
		private void RemoveImpossiblePotentialFactTypeMappings(FactTypeMappingDictionary decidedFactTypeMappings, FactTypeMappingListDictionary undecidedFactTypeMappings)
		{
			ObjectTypeList deeplyMappedObjectTypes = new ObjectTypeList();

			// For each decided fact type mapping...
			foreach (KeyValuePair<FactType, FactTypeMapping> decidedFactTypeMapping in decidedFactTypeMappings)
			{
				FactTypeMapping factTypeMapping = decidedFactTypeMapping.Value;

				// If it's a deep mapping...
				if (factTypeMapping.MappingType == FactTypeMappingType.Deep)
				{
					deeplyMappedObjectTypes.Add(factTypeMapping.FromObjectType);
				}
			}

			FactTypeList factsPendingDeletion = new FactTypeList();

			foreach (KeyValuePair<FactType, FactTypeMappingList> undecidedFactTypeMapping in undecidedFactTypeMappings)
			{
				FactType factType = undecidedFactTypeMapping.Key;
				FactTypeMappingList potentialFactTypeMappings = undecidedFactTypeMapping.Value;

				// For each potential fact type mapping...
				for (int i = potentialFactTypeMappings.Count - 1; i >= 0; i--)
				{
					FactTypeMapping potentialFactTypeMapping = potentialFactTypeMappings[i];

					// If it is maped away from an ObjectType that is already determined to be mapped elsewhere...
					if (deeplyMappedObjectTypes.Contains(potentialFactTypeMapping.FromObjectType) && potentialFactTypeMapping.MappingType == FactTypeMappingType.Deep)
					{
						// Remove it as a possibility.
						potentialFactTypeMappings.RemoveAt(i);
					}
				}

				// If there is only one possibility left...
				if (potentialFactTypeMappings.Count == 1)
				{
					// Mark it decided.
					decidedFactTypeMappings.Add(factType, potentialFactTypeMappings[0]);
					factsPendingDeletion.Add(factType);
				}
			}

			// Delete each undecided (now decided) fact type mapping marked for deletion.
			foreach (FactType key in factsPendingDeletion)
			{
				undecidedFactTypeMappings.Remove(key);
			}
		}

		/// <summary>
		/// Maps one-to-one fact types with two simple alethic mandatories away from the object type that has no possible
		/// (potential or decided) deep mappings away from it.  If both role players meet this criteria then no action is
		/// taken.
		/// </summary>
		/// <param name="decidedFactTypeMappings">The decided <see cref="FactTypeMapping"/> objects.</param>
		/// <param name="undecidedFactTypeMappings">The undecided <see cref="FactTypeMapping"/> posibilities.</param>
		/// <returns>The number of previously undecided fact type mappings that are now decided.</returns>
		private int MapTrivialOneToOneFactTypesWithTwoMandatories(FactTypeMappingDictionary decidedFactTypeMappings, FactTypeMappingListDictionary undecidedFactTypeMappings)
		{
			int changeCount = 0;
			FactTypeList factsPendingDeletion = new FactTypeList();

			foreach (KeyValuePair<FactType, FactTypeMappingList> undecidedFactTypeMapping in undecidedFactTypeMappings)
			{
				FactType factType = undecidedFactTypeMapping.Key;
				FactTypeMappingList potentialFactTypeMappings = undecidedFactTypeMapping.Value;
				LinkedElementCollection<RoleBase> roles = factType.RoleCollection;
				int roleCount = roles.Count;

				Debug.Assert(roleCount == 2, "All fact type mappings should be for fact types with exactly two roles.");

				Role firstRole = roles[0].Role;
				Role secondRole = roles[1].Role;
				ObjectType firstRolePlayer = firstRole.RolePlayer;
				ObjectType secondRolePlayer = secondRole.RolePlayer;
				bool firstRoleIsUnique = (firstRole.SingleRoleAlethicUniquenessConstraint != null);
				bool secondRoleIsUnique = (secondRole.SingleRoleAlethicUniquenessConstraint != null);
				MandatoryConstraint firstRoleMandatoryConstraint = firstRole.SimpleMandatoryConstraint;
				MandatoryConstraint secondRoleMandatoryConstraint = secondRole.SimpleMandatoryConstraint;
				bool firstRoleIsMandatory = (firstRoleMandatoryConstraint != null && firstRoleMandatoryConstraint.Modality == ConstraintModality.Alethic);
				bool secondRoleIsMandatory = (secondRoleMandatoryConstraint != null && secondRoleMandatoryConstraint.Modality == ConstraintModality.Alethic);

				// If this is a one-to-one fact type with two simple alethic mandatories...
				if (firstRoleIsUnique && secondRoleIsUnique && firstRoleIsMandatory && secondRoleIsMandatory)
				{
					bool firstRolePlayerHasPossibleDeepMappingsAway = ObjectTypeHasPossibleDeepMappingsAway(firstRolePlayer, factType, decidedFactTypeMappings, undecidedFactTypeMappings);
					bool secondRolePlayerHasPossibleDeepMappingsAway = ObjectTypeHasPossibleDeepMappingsAway(secondRolePlayer, factType, decidedFactTypeMappings, undecidedFactTypeMappings);

					// TODO: This may not be strong enough.
					// If secondRolePlayer has no possible deep mappings away from it...
					if (firstRolePlayerHasPossibleDeepMappingsAway && !secondRolePlayerHasPossibleDeepMappingsAway)
					{
						// Deep map toward firstRolePlayer
						FactTypeMapping factTypeMapping = new FactTypeMapping(factType, secondRole, firstRole, FactTypeMappingType.Deep);

						decidedFactTypeMappings.Add(factType, factTypeMapping);
						factsPendingDeletion.Add(factType);
						++changeCount;
					}
					else if (!firstRolePlayerHasPossibleDeepMappingsAway && secondRolePlayerHasPossibleDeepMappingsAway) // ...firstRolePlayer...
					{
						// Deep map toward secondRolePlayer
						FactTypeMapping factTypeMapping = new FactTypeMapping(factType, firstRole, secondRole, FactTypeMappingType.Deep);

						decidedFactTypeMappings.Add(factType, factTypeMapping);
						factsPendingDeletion.Add(factType);
						++changeCount;
					}
				}
			}

			// Delete each undecided (now decided) fact type mapping marked for deletion.
			foreach (FactType key in factsPendingDeletion)
			{
				undecidedFactTypeMappings.Remove(key);
			}

			return changeCount;
		}

		/// <summary>
		/// Determins wheather or not an object type has any possible (potential or decided)
		/// deep fact type mappings away from it.
		/// </summary>
		/// <param name="objectType">The object type to consider</param>
		/// <param name="excludedFactType">The fact type to ignore.  This parameter may be null.</param>
		/// <param name="decidedFactTypeMappings">The decided <see cref="FactTypeMapping"/> objects.</param>
		/// <param name="undecidedFactTypeMappings">The undecided <see cref="FactTypeMapping"/> possibilities.</param>
		/// <returns>True if objectType has possible deep mappings away from it, otherwise false.</returns>
		private bool ObjectTypeHasPossibleDeepMappingsAway(ObjectType objectType, FactType excludedFactType, FactTypeMappingDictionary decidedFactTypeMappings, FactTypeMappingListDictionary undecidedFactTypeMappings)
		{
			LinkedElementCollection<Role> rolesPlayed = ObjectTypePlaysRole.GetPlayedRoleCollection(objectType);

			foreach (Role rolePlayed in rolesPlayed)
			{
				FactType factType = rolePlayed.BinarizedFactType;

				if (factType == excludedFactType)
				{
					continue;
				}

				FactTypeMapping decidedFactTypeMapping;
				bool decidedFactTypeMappingExists = decidedFactTypeMappings.TryGetValue(factType, out decidedFactTypeMapping);
				FactTypeMappingList potentialFactTypeMappings;
				bool potentialFactTypeMappingsExist = undecidedFactTypeMappings.TryGetValue(factType, out potentialFactTypeMappings);

				// If there's a decided deep fact type mapping away from objectType...
				if (decidedFactTypeMappingExists && decidedFactTypeMapping.MappingType == FactTypeMappingType.Deep && decidedFactTypeMapping.FromObjectType == objectType)
				{
					return true;
				}
				else if (potentialFactTypeMappingsExist)
				{
					foreach (FactTypeMapping potentialFactTypeMapping in potentialFactTypeMappings)
					{
						// If there's a potential deep fact type mapping away from objectType...
						if (potentialFactTypeMapping.MappingType == FactTypeMappingType.Deep && potentialFactTypeMapping.FromObjectType == objectType)
						{
							return true;
						}
					}
				}
			}

			return false;
		}
		#endregion // Filter Algorithms Methods
		#region Generation Algorightm Methods
		/// <summary>
		/// Generates the <see cref="InformationTypeFormat"/> objects and adds them to the model.
		/// </summary>
		private void GenerateInformationTypeFormats()
		{
			ORMModel model = this.ORMModel;
			IEnumerable<ObjectType> modelValueTypes = model.ValueTypeCollection;
			AbstractionModel oialModel = this.AbstractionModel;

			// For each ValueType in the model...
			foreach (ObjectType valueType in modelValueTypes)
			{
				// Create InformationTypeFormat.
				PropertyAssignment namePropertyAssignment = new PropertyAssignment(InformationTypeFormat.NameDomainPropertyId, valueType.Name);
				InformationTypeFormat informationTypeFormat = new InformationTypeFormat(Store, namePropertyAssignment);
				InformationTypeFormatIsForValueType informationTypeFormatIsForValueType = new InformationTypeFormatIsForValueType(informationTypeFormat, valueType);
				// TODO: information type format data types

				// Add it to the model.
				oialModel.InformationTypeFormatCollection.Add(informationTypeFormat);
			}
		}

		/// <summary>
		/// Generages the <see cref="ConceptType"/> objects along with any relationships that they have and adds them to the
		/// model.
		/// </summary>
		/// <param name="factTypeMappings">A dictionary of all the final decided FactTypeMapping objects.</param>
		private void GenerateConceptTypes(FactTypeMappingDictionary factTypeMappings)
		{
			ORMModel model = this.ORMModel;
			LinkedElementCollection<ObjectType> modelObjectTypes = model.ObjectTypeCollection;
			AbstractionModel oialModel = this.AbstractionModel;

			// For each object type in the model...
			foreach (ObjectType objectType in modelObjectTypes)
			{
				// If it should have a conctpt type...
				if (ObjectTypeIsConceptType(objectType, factTypeMappings))
				{
					// Create the ConceptType object.
					PropertyAssignment name = new PropertyAssignment(ConceptType.NameDomainPropertyId, objectType.Name);
					ConceptType conceptType = new ConceptType(Store, name);
					ConceptTypeIsForObjectType conceptTypeIsForObjectType = new ConceptTypeIsForObjectType(conceptType, objectType);

					// Add it to the model.
					oialModel.ConceptTypeCollection.Add(conceptType);

					// If this conceptType is for a ValueType...
					if (objectType.IsValueType)
					{
						InformationTypeFormat valueTypeInformationTypeFormat = InformationTypeFormatIsForValueType.GetInformationTypeFormat(objectType);

						RoleAssignment conceptTypeRole = new RoleAssignment(InformationType.ConceptTypeDomainRoleId, conceptType);
						RoleAssignment informationTypeFormat = new RoleAssignment(InformationType.InformationTypeFormatDomainRoleId, valueTypeInformationTypeFormat);
						RoleAssignment[] roleAssignments = { conceptTypeRole, informationTypeFormat };
						PropertyAssignment isMandatory = new PropertyAssignment(InformationType.IsMandatoryDomainPropertyId, true);
						PropertyAssignment informationTypeNameProperty = new PropertyAssignment(InformationType.NameDomainPropertyId, String.Concat(objectType.Name, "Value"));
						PropertyAssignment[] informationTypePropertyAssignments = { isMandatory, informationTypeNameProperty };

						// ConceptType for conceptType gets an InformationType that references InformationTypeFormat for conceptType.
						InformationType informationType = new InformationType(Store, roleAssignments, informationTypePropertyAssignments);

						PropertyAssignment uniquenessNameProperty = new PropertyAssignment(Uniqueness.NameDomainPropertyId, String.Concat(objectType.Name, "Uniqueness"));
						PropertyAssignment isPreferred = new PropertyAssignment(Uniqueness.IsPreferredDomainPropertyId, true);
						PropertyAssignment[] uniquenessPropertyAssignments = { uniquenessNameProperty, isPreferred };

						// Uniqueness constraint
						Uniqueness uniqueness = new Uniqueness(Store, uniquenessPropertyAssignments);
						UniquenessIncludesConceptTypeChild uniquenessIncludesConceptTypeChild = new UniquenessIncludesConceptTypeChild(uniqueness, informationType);

						conceptType.UniquenessCollection.Add(uniqueness);
					}
				}
			}
		}

		/// <summary>
		/// Generate the <see cref="ConceptTypeChild"/> objects of the <see cref="AbstractionModel"/>.
		/// </summary>
		/// <param name="factTypeMappings">The decided <see cref="FactTypeMapping"/> objects.</param>
		private void GenerateConceptTypeChildren(FactTypeMappingDictionary factTypeMappings)
		{
			foreach (FactTypeMapping factTypeMapping in factTypeMappings.Values)
			{
				string towardsName = ResolveRoleName(factTypeMapping.TowardsRole);
				string fromName = ResolveRoleName(factTypeMapping.FromRole);

				ConceptType towardsConceptType = ConceptTypeIsForObjectType.GetConceptType(factTypeMapping.TowardsObjectType);

				// If this is not going toward a object type that has a concept type...
				if (towardsConceptType == null)
				{
					continue;
				}

				MandatoryConstraint towardsRoleMandatoryConstraint = factTypeMapping.TowardsRole.SimpleMandatoryConstraint;
				bool towardsRoleIsMandatory = (towardsRoleMandatoryConstraint != null && towardsRoleMandatoryConstraint.Modality == ConstraintModality.Alethic);

				if (ObjectTypeIsConceptType(factTypeMapping.FromObjectType, factTypeMappings))
				{
					ConceptType fromConceptType = ConceptTypeIsForObjectType.GetConceptType(factTypeMapping.FromObjectType);

					if (factTypeMapping.MappingType == FactTypeMappingType.Deep)
					{
						bool factTypeIsSubtype = factTypeMapping.FactType is SubtypeFact;

						UniquenessConstraint fromRoleUniquenessConstraint = (UniquenessConstraint)factTypeMapping.FromRole.SingleRoleAlethicUniquenessConstraint;
						UniquenessConstraint towardsRoleUniquenessConstraint = (UniquenessConstraint)factTypeMapping.TowardsRole.SingleRoleAlethicUniquenessConstraint;
						bool fromRoleIsPreferred = fromRoleUniquenessConstraint.IsPreferred;
						bool towardsRoleIsPreferred = towardsRoleUniquenessConstraint.IsPreferred;

						RoleAssignment assimilatorConceptType = new RoleAssignment(ConceptTypeAssimilatesConceptType.AssimilatorConceptTypeDomainRoleId, towardsConceptType);
						RoleAssignment assimilatedConceptType = new RoleAssignment(ConceptTypeAssimilatesConceptType.AssimilatedConceptTypeDomainRoleId, fromConceptType);
						RoleAssignment[] roleAssignments = { assimilatorConceptType, assimilatedConceptType };

						PropertyAssignment isMandatory = new PropertyAssignment(ConceptTypeAssimilatesConceptType.IsMandatoryDomainPropertyId, towardsRoleIsMandatory);
						PropertyAssignment name = new PropertyAssignment(ConceptTypeAssimilatesConceptType.NameDomainPropertyId, towardsName);
						PropertyAssignment oppositeName = new PropertyAssignment(ConceptTypeAssimilatesConceptType.OppositeNameDomainPropertyId, fromName);
						PropertyAssignment refersToSubtype = new PropertyAssignment(ConceptTypeAssimilatesConceptType.RefersToSubtypeDomainPropertyId, factTypeIsSubtype);
						PropertyAssignment isPreferredForParent = new PropertyAssignment(ConceptTypeAssimilatesConceptType.IsPreferredForParentDomainPropertyId, fromRoleIsPreferred);
						PropertyAssignment isPreferredForTarget = new PropertyAssignment(ConceptTypeAssimilatesConceptType.IsPreferredForTargetDomainPropertyId, towardsRoleIsPreferred);

						PropertyAssignment[] propertyAssignments = { isMandatory, name, oppositeName, refersToSubtype, isPreferredForParent, isPreferredForTarget };

						// ConceptType for factTypeMapping's TowardsObjectType assimilates ConceptType for factTypeMapping's FromObjectType.
						ConceptTypeAssimilatesConceptType assimilatedConceptTypeRef = new ConceptTypeAssimilatesConceptType(Store, roleAssignments, propertyAssignments);
						ConceptTypeChildHasPathFactType conceptTypeChildHasPathFactType = new ConceptTypeChildHasPathFactType(assimilatedConceptTypeRef, factTypeMapping.FactType);
					}
					else
					{
						RoleAssignment relatingConceptType = new RoleAssignment(ConceptTypeRelatesToConceptType.RelatingConceptTypeDomainRoleId, towardsConceptType);
						RoleAssignment relatedConceptType = new RoleAssignment(ConceptTypeRelatesToConceptType.RelatedConceptTypeDomainRoleId, fromConceptType);
						RoleAssignment[] roleAssignments = { relatingConceptType, relatedConceptType };

						PropertyAssignment isMandatory = new PropertyAssignment(ConceptTypeRelatesToConceptType.IsMandatoryDomainPropertyId, towardsRoleIsMandatory);
						PropertyAssignment name = new PropertyAssignment(ConceptTypeRelatesToConceptType.NameDomainPropertyId, towardsName);
						PropertyAssignment oppositeName = new PropertyAssignment(ConceptTypeRelatesToConceptType.OppositeNameDomainPropertyId, fromName);
						PropertyAssignment[] propertyAssignments = { isMandatory, name, oppositeName };

						// ConceptType for factTypeMapping's TowardsObjectType relates to ConcpetType for factTypeMapping's FromObjectType.
						ConceptTypeRelatesToConceptType relatingConceptTypeRef = new ConceptTypeRelatesToConceptType(Store, roleAssignments, propertyAssignments);
						ConceptTypeChildHasPathFactType conceptTypeChildHasPathFactType = new ConceptTypeChildHasPathFactType(relatingConceptTypeRef, factTypeMapping.FactType);
					}
				}
				else
				{
					IList<FactType> factTypes = new List<FactType>();
					factTypes.Add(factTypeMapping.FactType);

					foreach (InformationTypeFormatWithFactTypes fromInformationTypeFormatWithFactTypes in GetCollapsedConceptTypeChildren(factTypeMapping.FromObjectType, factTypes))
					{
						RoleAssignment conceptType = new RoleAssignment(InformationType.ConceptTypeDomainRoleId, towardsConceptType);
						RoleAssignment informationTypeFormat = new RoleAssignment(InformationType.InformationTypeFormatDomainRoleId, fromInformationTypeFormatWithFactTypes.InformationTypeFormat);
						RoleAssignment[] roleAssignments = { conceptType, informationTypeFormat };
						PropertyAssignment isMandatory = new PropertyAssignment(InformationType.IsMandatoryDomainPropertyId, towardsRoleIsMandatory);
						// HACK: find a better way to get the name of this.
						PropertyAssignment name = new PropertyAssignment(InformationType.NameDomainPropertyId, fromName);
						PropertyAssignment[] propertyAssignments = { isMandatory, name };

						// ConceptType for factTypeMapping's TowardsConceptType gets an InformationType that references ConceptType for factTypeMapping's FromObjectType.
						InformationType informationType = new InformationType(Store, roleAssignments, propertyAssignments);

						foreach (FactType factType in fromInformationTypeFormatWithFactTypes.FactTypes)
						{
							ConceptTypeChildHasPathFactType conceptTypeChildHasPathFactType = new ConceptTypeChildHasPathFactType(informationType, factType);
						}
					}
				}
			}
		}

		/// <summary>
		/// Generates the <see cref="Uniqueness"/> objects for the <see cref="AbstractionModel"/>.
		/// </summary>
		/// <param name="factTypeMappings">The decided <see cref="FactTypeMapping"/> objects.</param>
		private void GenerateUniqueness(FactTypeMappingDictionary factTypeMappings)
		{
			// TODO: clean this up.
			AbstractionModel oialModel = this.AbstractionModel;

			// For each concept type in the model...
			foreach (ConceptType conceptType in oialModel.ConceptTypeCollection)
			{
				ObjectType objectType = ConceptTypeIsForObjectType.GetObjectType(conceptType);

				// For each role played by its object type...
				foreach (Role role in objectType.PlayedRoleCollection)
				{
					// If factType is implicitly or explicity objectified...
					if (role.FactType.RoleCollection.Count != 2 || role.FactType.Objectification != null)
					{
						continue;
					}

					Role oppositeRole = role.OppositeRoleAlwaysResolveProxy as Role;

					if (oppositeRole == null)
					{
						continue;
					}

					// For each constraint on the opposite role...
					foreach (ConstraintRoleSequence constraintRoleSequence in oppositeRole.ConstraintRoleSequenceCollection)
					{
						UniquenessConstraint uninquenessConstraint = constraintRoleSequence as UniquenessConstraint;

						// If it is a uniqueness constraint...
						if (uninquenessConstraint != null)
						{
							if (UniquenessIsForUniquenessConstraint.GetUniqueness(uninquenessConstraint) != null)
							{
								continue;
							}

							bool allChildrenMapTowardObjectType = true;
							IList<FactType> factTypes = new List<FactType>();

							foreach (Role childRole in uninquenessConstraint.RoleCollection)
							{
								FactType binarizedFactType = childRole.BinarizedFactType;
								FactTypeMapping factTypeMapping = factTypeMappings[binarizedFactType];

								if (factTypeMapping.TowardsObjectType != objectType)
								{
									allChildrenMapTowardObjectType = false;
									break;
								}
								else
								{
									factTypes.Add(binarizedFactType);
								}
							}

							if (allChildrenMapTowardObjectType)
							{
								IList<ConceptTypeChild> conceptTypeChildren = new List<ConceptTypeChild>();
								bool childWasAssimilation = false;

								foreach (FactType factType in factTypes)
								{

									foreach (ConceptTypeChild conceptTypeChild in ConceptTypeChildHasPathFactType.GetConceptTypeChild(factType))
									{
										if (conceptTypeChild is ConceptTypeAssimilatesConceptType)
										{
											childWasAssimilation = true;
											break;
										}

										conceptTypeChildren.Add(conceptTypeChild);
									}

									if (childWasAssimilation)
									{
										break;
									}
								}

								if (!childWasAssimilation)
								{
									PropertyAssignment name = new PropertyAssignment(Uniqueness.NameDomainPropertyId, uninquenessConstraint.Name);
									PropertyAssignment isPreferred = new PropertyAssignment(Uniqueness.IsPreferredDomainPropertyId, uninquenessConstraint.IsPreferred);
									PropertyAssignment[] propertyAssignments = { name, isPreferred };

									// Create uniquenesss
									Uniqueness uniqueness = new Uniqueness(Store, propertyAssignments);
									uniqueness.ConceptType = conceptType;
									UniquenessIsForUniquenessConstraint uniquenessIsForUniquenessConstraint = new UniquenessIsForUniquenessConstraint(uniqueness, uninquenessConstraint);

									foreach (ConceptTypeChild conceptTypeChild in conceptTypeChildren)
									{
										UniquenessIncludesConceptTypeChild uniquenessIncludesConceptTypeChild = new UniquenessIncludesConceptTypeChild(uniqueness, conceptTypeChild);
									}
								}
							}
						}
					}
				}
			}
		}

		private void GenerateFactTypeMappings(FactTypeMappingDictionary factTypeMappings)
		{
			foreach (FactTypeMapping mapping in factTypeMappings.Values)
			{
				FactTypeMapsTowardsRole r = new FactTypeMapsTowardsRole(mapping.FactType, mapping.TowardsRole);
				r.Depth = (mapping.MappingType == FactTypeMappingType.Deep ? MappingDepth.Deep : MappingDepth.Shallow);
			}
		}

		private void GenerateAssociations()
		{
			AbstractionModel oialModel = this.AbstractionModel;

			// UNDONE: Portions of this may need to change depending on what we do for unary objectification.
			foreach (ConceptType ct in oialModel.ConceptTypeCollection)
			{
				ObjectType ot = ConceptTypeIsForObjectType.GetObjectType(ct);
				Objectification objectification = ot.Objectification;
				if (objectification != null)
				{
					LinkedElementCollection<ConceptTypeChild> associationChildren =
						ConceptTypeHasChildAsPartOfAssociation.GetTargetCollection(ct);

					// CT becomes an associationChild for all concept types related to the binarized fact types
					foreach (FactType factType in objectification.ImpliedFactTypeCollection)
					{
						LinkedElementCollection<ConceptTypeChild> childrenForFactType =
							ConceptTypeChildHasPathFactType.GetConceptTypeChild(factType);

						Debug.Assert(childrenForFactType.Count == 1, "Can this not be 1?");

						associationChildren.AddRange(ConceptTypeChildHasPathFactType.GetConceptTypeChild(factType));
					}
				}
			}
		}
		#endregion // Generation Algorightm Methods
		#region Helper Methods
		/// <summary>
		/// Resolve the name that will be used for a <see cref="ConceptTypeChild"/> given the <see cref="Role"/> it's resulting from.
		/// </summary>
		/// <param name="role">The <see cref="Role"/> that the <see cref="ConceptTypeChild"/> is resulting from.</param>
		/// <returns>The name to use for the <see cref="ConceptTypeChild"/>.</returns>
		private string ResolveRoleName(Role role)
		{
			// HACK: This is only here until we implement a better alternative.
			string name = role.Name;

			if (String.IsNullOrEmpty(name))
			{
				name = role.RolePlayer.Name;
			}

			return name;
		}

		/// <summary>
		/// Determins wheather an <see cref="ObjectType"/> is destined to be a top-level <see cref="ConceptType"/> or not.
		/// </summary>
		/// <param name="objectType">The <see cref="ObjectType"/> to test.</param>
		/// <param name="factTypeMappings">A dictionary of all the final decided <see cref="FactTypeMapping"/> objects.</param>
		/// <returns>Returns true if the <see cref="ObjectType"/> is destined to be a top-level <see cref="ConceptType"/>.</returns>
		private bool ObjectTypeIsTopLevelConceptType(ObjectType objectType, FactTypeMappingDictionary factTypeMappings)
		{
			if (ObjectTypeIsConceptType(objectType, factTypeMappings))
			{
				LinkedElementCollection<Role> roles = ObjectTypePlaysRole.GetPlayedRoleCollection(objectType);

				foreach (Role role in roles)
				{
					FactType factType = role.BinarizedFactType;
					FactTypeMapping factTypeMapping = factTypeMappings[factType];

					if (factTypeMapping.FromObjectType == objectType && factTypeMapping.MappingType == FactTypeMappingType.Deep)
					{
						return false;
					}
				}

				return true;
			}

			return false;
		}

		/// <summary>
		/// Determins wheather an <see cref="ObjectType"/> should have a <see cref="ConceptType"/> or not.
		/// </summary>
		/// <param name="objectType">The <see cref="ObjectType"/> to test.</param>
		/// <param name="factTypeMappings">A dictionary of all the final decided <see cref="FactTypeMapping"/> objects.</param>
		/// <returns>Returns true if the <see cref="ObjectType"/> should have <see cref="ConceptType"/>.</returns>
		private bool ObjectTypeIsConceptType(ObjectType objectType, FactTypeMappingDictionary factTypeMappings)
		{
			// If objectType is independent...
			if (objectType.IsIndependent)
			{
				return true;
			}

			LinkedElementCollection<Role> rolesPlayed = ObjectTypePlaysRole.GetPlayedRoleCollection(objectType);

			foreach (Role role in rolesPlayed)
			{
				// If objectType is a subtype...
				if (role is SubtypeMetaRole)
				{
					return true;
				}

				FactType factType = role.BinarizedFactType;
				FactTypeMapping factTypeMapping = factTypeMappings[factType];

				// If fact type mapping is toward objectType...
				if (factTypeMapping.TowardsObjectType == objectType)
				{
					// If there are no constraints on this role...
					if (factTypeMapping.FromRole.ConstraintRoleSequenceCollection.Count == 0)
					{
						return true;
					}

					foreach (ConstraintRoleSequence constraintRoleSequence in factTypeMapping.FromRole.ConstraintRoleSequenceCollection)
					{
						UniquenessConstraint uninquenessConstraint = constraintRoleSequence as UniquenessConstraint;

						// If fromRole does not have a preferred uniqueness constraint on it...
						if (uninquenessConstraint != null && !uninquenessConstraint.IsPreferred)
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Gets the collapsed <see cref="ConceptTypeChild"/> object(s).
		/// </summary>
		/// <param name="objectType">The <see cref="ObjectType"/> for which to determine the <see cref="ConceptTypeChild"/>.</param>
		/// <param name="factTypes">Any <see cref="FactType"/> objects that were walked to get here.</param>
		/// <returns>
		/// The determined <see cref="ConceptTypeChild"/> objects along with the <see cref="FactType"/> objects that were
		/// walked to get them.
		/// </returns>
		private IEnumerable<InformationTypeFormatWithFactTypes> GetCollapsedConceptTypeChildren(ObjectType objectType, IList<FactType> factTypes)
		{
			InformationTypeFormat informationTypeFormat = InformationTypeFormatIsForValueType.GetInformationTypeFormat(objectType);

			// If this objectType has an informationTypeFormat...
			if (informationTypeFormat != null)
			{
				yield return new InformationTypeFormatWithFactTypes(informationTypeFormat, factTypes);
			}
			else
			{
				LinkedElementCollection<Role> rolesPlayed = ObjectTypePlaysRole.GetPlayedRoleCollection(objectType);

				foreach (Role role in rolesPlayed)
				{
					FactType factType = role.BinarizedFactType;
					Role oppositeRole = (Role)role.OppositeRoleAlwaysResolveProxy;

					// Recursivly call this for each of objectType's preferred identifier object types.
					foreach (ConstraintRoleSequence constraintRoleSequence in oppositeRole.ConstraintRoleSequenceCollection)
					{
						UniquenessConstraint uniquenessConstraint = constraintRoleSequence as UniquenessConstraint;

						if (uniquenessConstraint != null && uniquenessConstraint.IsPreferred)
						{
							factTypes.Add(factType);

							foreach (InformationTypeFormatWithFactTypes childInformationTypeFormatWithFactTypes in GetCollapsedConceptTypeChildren(oppositeRole.RolePlayer, factTypes))
							{
								yield return childInformationTypeFormatWithFactTypes;
							}

							factTypes.Remove(factType);
						}
					}
				}
			}
		}
		#endregion // Helper Methods
		#endregion // ORM to OIAL Algorithm Methods
	}
}
