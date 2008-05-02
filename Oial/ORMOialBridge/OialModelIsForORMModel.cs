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
using System.Collections.ObjectModel;

namespace Neumont.Tools.ORMToORMAbstractionBridge
{
	public partial class AbstractionModelIsForORMModel
	{
		#region CurrentAlgorithmVersion constant
		/// <summary>
		/// The algorithm version written to the file
		/// </summary>
		public const string CurrentAlgorithmVersion = "1.003";
		#endregion // CurrentAlgorithmVersion constant
		#region ValidationPriority enum
		/// <summary>
		/// DelayValidate ordering constants. Handles non-zero values for
		/// validation methods in this class and nested classes.
		/// </summary>
		private static class ValidationPriority
		{
			/// <summary>
			/// A new <see cref="ObjectType"/> has been added to the ORM model, establish
			/// gateway exclusion relationships.
			/// </summary>
			public const int GatewayNewObjectType = -100;
			/// <summary>
			/// An <see cref="ObjectType"/> has been modified in the ORM model, establish
			/// gateway exclusion relationships.
			/// </summary>
			public const int GatewayReconsiderObjectType = -90;
			/// <summary>
			/// A <see cref="FactType"/> has been modified in the ORM model, establish
			/// gateway exclusion relationships.
			/// </summary>
			public const int GatewayNewFactType = -80;
			/// <summary>
			/// A <see cref="FactType"/> has been modified in the ORM model, establish
			/// gateway exclusion relationships.
			/// </summary>
			public const int GatewayReconsiderFactType = -70;
			/// <summary>
			/// Gateway exclusion relationships have been added, remove other existing
			/// bridge relationships
			/// </summary>
			public const int GatewayRemoveExcludedBridgeRelationships = -60;
			/// <summary>
			/// A new element has been added and passed gateway filtering
			/// </summary>
			public const int GatewayAddElement = -50;
			/// <summary>
			/// Validate the model. Current rebuilds the entire model.
			/// </summary>
			public const int ValidateModel = 100;
			/// <summary>
			/// Reset mandatory properties. This is done after ValidateModel
			/// so that we don't waste time 
			/// </summary>
			public const int ValidateMandatory = 110;
		}
		#endregion // ValidationPriority enum
		#region Element tracking transaction support
		#region ModelElementModification enum
		/// <summary>
		/// An enumeration of changes that may affect the ORM model
		/// </summary>
		[Flags]
		private enum ModelElementModification
		{
			/// <summary>
			/// An element from the ORM object model was added
			/// </summary>
			ORMElementAdded = 1,
			/// <summary>
			/// An element from the ORM object model was changed
			/// </summary>
			ORMElementChanged = 2,
			/// <summary>
			/// An element from the ORM object model was deleted
			/// </summary>
			ORMElementDeleted = 4,
			/// <summary>
			/// A bridge element was removed from this abstraction element
			/// </summary>
			AbstractionElementDetached = 8,
		}
		#endregion // ModelElementModification enum
		private static readonly object Key = new object();

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

		private static void AddTransactedModelElement(ModelElement element, ModelElementModification action)
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
		#endregion // Element tracking transaction support
		#region Model validation
		private bool myRebuildingAbstractionModel;
		/// <summary>
		/// Delays the validate model.
		/// </summary>
		/// <param name="element">The element.</param>
		[DelayValidatePriority(ValidationPriority.ValidateModel, DomainModelType = typeof(ORMCoreDomainModel), Order = DelayValidatePriorityOrder.AfterDomainModel)]
		private static void DelayValidateModel(ModelElement element)
		{
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

			ORMModel model = (ORMModel)element;
			Store store = model.Store;

			// Get the link from the given ORMModel
			AbstractionModelIsForORMModel oialModelIsForORMModel = AbstractionModelIsForORMModel.GetLinkToAbstractionModel(model);
			AbstractionModel oialModel = null;

			// If the link exists, clear it out. There is no need to recreate it completely.
			if (oialModelIsForORMModel != null)
			{
				// Make sure the RebuildingAbstractionModel returns true during the rebuild
				oialModelIsForORMModel.myRebuildingAbstractionModel = true;
				try
				{
					oialModel = oialModelIsForORMModel.AbstractionModel;
					oialModel.ConceptTypeCollection.Clear();
					oialModel.InformationTypeFormatCollection.Clear();

					ReadOnlyCollection<FactTypeMapsTowardsRole> factTypeMaps = store.ElementDirectory.FindElements<FactTypeMapsTowardsRole>(false);
					int factTypeMapCount = factTypeMaps.Count;
					for (int i = factTypeMapCount - 1; i >= 0; --i)
					{
						FactTypeMapsTowardsRole factTypeMap = factTypeMaps[i];
						if (factTypeMap.FactType.Model == model)
						{
							factTypeMap.Delete();
						}
					}
					// Apply ORM to OIAL algorithm
					oialModelIsForORMModel.TransformORMtoOial();
				}
				finally
				{
					oialModelIsForORMModel.myRebuildingAbstractionModel = false;
				}
			}
			else
			{
				oialModel = new AbstractionModel(
					store,
					new PropertyAssignment(AbstractionModel.NameDomainPropertyId, model.Name));
				oialModelIsForORMModel = new AbstractionModelIsForORMModel(oialModel, model);

				// Set initial object exclusion states
				ORMElementGateway.Initialize(model, null);

				// Apply ORM to OIAL algorithm
				oialModelIsForORMModel.TransformORMtoOial();
			}
			if (oialModel != null)
			{
				AbstractionModelGenerationSetting generationSetting = GenerationSettingTargetsAbstractionModel.GetGenerationSetting(oialModel);
				if (generationSetting == null)
				{
					generationSetting = new AbstractionModelGenerationSetting(store, new PropertyAssignment(AbstractionModelGenerationSetting.AlgorithmVersionDomainPropertyId, CurrentAlgorithmVersion));
					new GenerationSettingTargetsAbstractionModel(generationSetting, oialModel);
					new GenerationStateHasGenerationSetting(GenerationState.EnsureGenerationState(store), generationSetting);
				}
				else
				{
					generationSetting.AlgorithmVersion = CurrentAlgorithmVersion;
				}
			}
		}
		/// <summary>
		/// Initiate a model rebuild if a rebuild is not already under way
		/// </summary>
		/// <param name="model">The <see cref="AbstractionModel"/> to check for rebuilding</param>
		/// <returns><see langword="true"/> if the requested rebuild will occur.</returns>
		private static bool TestRebuildAbstractionModel(AbstractionModel model)
		{
			AbstractionModelIsForORMModel link = AbstractionModelIsForORMModel.GetLinkToORMModel(model);
			bool allow = !(link != null && link.myRebuildingAbstractionModel);
			if (allow)
			{
				FrameworkDomainModel.DelayValidateElement(link.ORMModel, DelayValidateModel);
			}
			return allow;
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
				if ((actions & (int)ModelElementModification.ORMElementAdded) != 0 && (actions & (int)ModelElementModification.ORMElementDeleted) != 0)
				{
					elementList.Remove(element);
				}
			}
		}
		#endregion
		#region ORM to OIAL Algorithm Methods
		/// <summary>
		/// Transforms the data in <see cref="ORMModel"/> into this <see cref="AbstractionModel"/>.
		/// </summary>
		private void TransformORMtoOial()
		{
			ORMModel model = this.ORMModel;
			LinkedElementCollection<FactType> modelFactTypes = model.FactTypeCollection;

			FactTypeMappingDictionary decidedManyToOneFactTypeMappings = new FactTypeMappingDictionary();
			FactTypeMappingDictionary decidedOneToOneFactTypeMappings = new FactTypeMappingDictionary();
			FactTypeMappingListDictionary undecidedOneToOneFactTypeMappings = new FactTypeMappingListDictionary();

			PerformInitialFactTypeMappings(modelFactTypes, decidedManyToOneFactTypeMappings, decidedOneToOneFactTypeMappings, undecidedOneToOneFactTypeMappings);
			FilterFactTypeMappings(decidedManyToOneFactTypeMappings, decidedOneToOneFactTypeMappings, undecidedOneToOneFactTypeMappings);

			FactTypeMappingPermuter permuter = new FactTypeMappingPermuter(decidedManyToOneFactTypeMappings, decidedOneToOneFactTypeMappings, undecidedOneToOneFactTypeMappings);
			FactTypeMappingDictionary decidedFactTypeMappings = permuter.Run();

			GenerateOialModel(decidedFactTypeMappings);
		}

		/// <summary>
		/// Determines the obvious fact type mappings, and all other potential mappings.
		/// </summary>
		/// <param name="modelFactTypes">The <see cref="FactType"/> objects of the model</param>
		/// <param name="decidedManyToOneFactTypeMappings">The decided many-to-one <see cref="FactTypeMapping"/> objects.</param>
		/// <param name="decidedOneToOneFactTypeMappings">The decided one-to-one <see cref="FactTypeMapping"/> objects.</param>
		/// <param name="undecidedOneToOneFactTypeMappings">The undecided <see cref="FactTypeMapping"/> possibilities.</param>
		private void PerformInitialFactTypeMappings(LinkedElementCollection<FactType> modelFactTypes, FactTypeMappingDictionary decidedManyToOneFactTypeMappings, FactTypeMappingDictionary decidedOneToOneFactTypeMappings, FactTypeMappingListDictionary undecidedOneToOneFactTypeMappings)
		{
			// For each fact type in the model...
			foreach (FactType factType in modelFactTypes)
			{
				if (ShouldIgnoreFactType(factType))
				{
					continue;
				}
				SubtypeFact subtypeFact = factType as SubtypeFact;

				// If it's a subtype relation...
				if (subtypeFact != null)
				{
					Role subtypeRole = subtypeFact.SubtypeRole;
					Role supertypeRole = subtypeFact.SupertypeRole;

					// Map deeply toward the supertype.
					FactTypeMapping factTypeMapping = new FactTypeMapping(subtypeFact, subtypeRole, supertypeRole, MappingDepth.Deep);

					decidedOneToOneFactTypeMappings.Add(subtypeFact, factTypeMapping);
				}
				else
				{
					LinkedElementCollection<RoleBase> roles = factType.RoleCollection;

					Debug.Assert(roles.Count == 2 && factType.Objectification == null, "Non-binarized fact types should have been filtered out already.");
					
					Role firstRole = roles[0].Role;
					Role secondRole = roles[1].Role;
					ObjectType firstRolePlayer = firstRole.RolePlayer;
					ObjectType secondRolePlayer = secondRole.RolePlayer;
					UniquenessConstraint firstRoleUniquenessConstraint = (UniquenessConstraint)firstRole.SingleRoleAlethicUniquenessConstraint;
					UniquenessConstraint secondRoleUniquenessConstraint = (UniquenessConstraint)secondRole.SingleRoleAlethicUniquenessConstraint;
					bool firstRoleIsUnique = (firstRoleUniquenessConstraint != null);
					bool secondRoleIsUnique = (secondRoleUniquenessConstraint != null);

					// We don't need to worry about shallow mappings towards preferred identifiers on many-to-ones, since the preferred
					// identifier pattern ensures that these cases will always map towards the object type being identified anyway.

					// If only firstRole is unique...
					if (firstRoleIsUnique && !secondRoleIsUnique)
					{
						// Shallow map toward firstRolePlayer.
						FactTypeMapping factTypeMapping = new FactTypeMapping(factType, secondRole, firstRole, MappingDepth.Shallow);

						decidedManyToOneFactTypeMappings.Add(factType, factTypeMapping);
					}
					else if (!firstRoleIsUnique && secondRoleIsUnique) // ...only secondRole is unique...
					{
						// Shallow map toward secondRolePlayer.
						FactTypeMapping factTypeMapping = new FactTypeMapping(factType, firstRole, secondRole, MappingDepth.Shallow);

						decidedManyToOneFactTypeMappings.Add(factType, factTypeMapping);
					}
					else if (firstRoleIsUnique && secondRoleIsUnique) // ...both roles are unique...
					{
						bool firstRoleIsMandatory = (firstRole.SingleRoleAlethicMandatoryConstraint != null);
						bool secondRoleIsMandatory = (secondRole.SingleRoleAlethicMandatoryConstraint != null);

						// If this is a ring fact type...
						if (firstRolePlayer == secondRolePlayer)
						{
							// If only firstRole is mandatory...
							if (firstRoleIsMandatory && !secondRoleIsMandatory)
							{
								// Shallow map toward firstRolePlayer (mandatory role player).
								FactTypeMapping factTypeMapping = new FactTypeMapping(factType, firstRole, secondRole, MappingDepth.Shallow);

								decidedOneToOneFactTypeMappings.Add(factType, factTypeMapping);
							}
							else if (!firstRoleIsMandatory && secondRoleIsMandatory) // ...only secondRole is mandatory...
							{
								// Shallow map toward secondRolePlayer (mandatory role player).
								FactTypeMapping factTypeMapping = new FactTypeMapping(factType, secondRole, firstRole, MappingDepth.Shallow);

								decidedOneToOneFactTypeMappings.Add(factType, factTypeMapping);
							}
							else // ...otherwise...
							{
								// Shallow map toward firstRolePlayer.
								FactTypeMapping factTypeMapping = new FactTypeMapping(factType, firstRole, secondRole, MappingDepth.Shallow);

								decidedOneToOneFactTypeMappings.Add(factType, factTypeMapping);
							}
						}
						else // ...not a ring fact type...
						{
							// These are used to make sure that we never shallowly map towards a preferred identifier.
							bool firstRoleIsUniqueAndPreferred = firstRoleIsUnique && firstRoleUniquenessConstraint.IsPreferred;
							bool secondRoleIsUniqueAndPreferred = secondRoleIsUnique && secondRoleUniquenessConstraint.IsPreferred;

							FactTypeMappingList potentialFactTypeMappings = new FactTypeMappingList();

							// If neither role is mandatory...
							if (!firstRoleIsMandatory && !secondRoleIsMandatory)
							{
								// If firstRole is not preferred...
								if (!firstRoleIsUniqueAndPreferred)
								{
									// Shallow map toward firstRolePlayer.
									potentialFactTypeMappings.Add(new FactTypeMapping(factType, secondRole, firstRole, MappingDepth.Shallow));
								}

								// If seccondRole is not preferred...
								if (!secondRoleIsUniqueAndPreferred)
								{
									// Shallow map toward secondRolePlayer.
									potentialFactTypeMappings.Add(new FactTypeMapping(factType, firstRole, secondRole, MappingDepth.Shallow));
								}
							}
							else if (firstRoleIsMandatory && !secondRoleIsMandatory) // ...only firstRole is mandatory...
							{
								// If firstRole is not preferred...
								if (!firstRoleIsUniqueAndPreferred)
								{
									// Shallow map toward firstRolePlayer.
									potentialFactTypeMappings.Add(new FactTypeMapping(factType, secondRole, firstRole, MappingDepth.Shallow));
								}

								// If seccondRole is not preferred...
								if (!secondRoleIsUniqueAndPreferred)
								{
									// Shallow map toward secondRolePlayer.
									potentialFactTypeMappings.Add(new FactTypeMapping(factType, firstRole, secondRole, MappingDepth.Shallow));
								}

								// Deep map toward secondRolePlayer.
								potentialFactTypeMappings.Add(new FactTypeMapping(factType, firstRole, secondRole, MappingDepth.Deep));
							}
							else if (!firstRoleIsMandatory && secondRoleIsMandatory) // ...only secondRole is mandatory...
							{
								// If firstRole is not preferred...
								if (!firstRoleIsUniqueAndPreferred)
								{
									// Shallow map toward firstRolePlayer.
									potentialFactTypeMappings.Add(new FactTypeMapping(factType, secondRole, firstRole, MappingDepth.Shallow));
								}

								// Deep map toward firstRolePlayer.
								potentialFactTypeMappings.Add(new FactTypeMapping(factType, secondRole, firstRole, MappingDepth.Deep));

								// If seccondRole is not preferred...
								if (!secondRoleIsUniqueAndPreferred)
								{
									// Shallow map toward secondRolePlayer.
									potentialFactTypeMappings.Add(new FactTypeMapping(factType, firstRole, secondRole, MappingDepth.Shallow));
								}
							}
							else // ...both roles are mandatory...
							{
								// If firstRole is not preferred...
								if (!firstRoleIsUniqueAndPreferred)
								{
									// Shallow map toward firstRolePlayer.
									potentialFactTypeMappings.Add(new FactTypeMapping(factType, secondRole, firstRole, MappingDepth.Shallow));
								}

								// If seccondRole is not preferred...
								if (!secondRoleIsUniqueAndPreferred)
								{
									// Shallow map toward secondRolePlayer.
									potentialFactTypeMappings.Add(new FactTypeMapping(factType, firstRole, secondRole, MappingDepth.Shallow));
								}

								// Deep map toward firstRolePlayer.
								potentialFactTypeMappings.Add(new FactTypeMapping(factType, secondRole, firstRole, MappingDepth.Deep));

								// Deep map toward secondRolePlayer.
								potentialFactTypeMappings.Add(new FactTypeMapping(factType, firstRole, secondRole, MappingDepth.Deep));
							}

							Debug.Assert(potentialFactTypeMappings.Count > 0);
							if (potentialFactTypeMappings.Count == 1)
							{
								decidedOneToOneFactTypeMappings.Add(factType, potentialFactTypeMappings[0]);
							}
							else
							{
								undecidedOneToOneFactTypeMappings.Add(factType, potentialFactTypeMappings);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Runs various algorithms on the undecided fact type mappings in an attempt to decide as many as possible.
		/// </summary>
		/// <param name="decidedManyToOneFactTypeMappings">The decided many-to-one <see cref="FactTypeMapping"/> objects.</param>
		/// <param name="decidedOneToOneFactTypeMappings">The decided one-to-one <see cref="FactTypeMapping"/> objects.</param>
		/// <param name="undecidedOneToOneFactTypeMappings">The undecided <see cref="FactTypeMapping"/> possibilities.</param>
		private void FilterFactTypeMappings(FactTypeMappingDictionary decidedManyToOneFactTypeMappings, FactTypeMappingDictionary decidedOneToOneFactTypeMappings, FactTypeMappingListDictionary undecidedOneToOneFactTypeMappings)
		{
			bool changed;
			do
			{
				RemoveImpossiblePotentialFactTypeMappings(decidedOneToOneFactTypeMappings, undecidedOneToOneFactTypeMappings);

				changed = MapTrivialOneToOneFactTypesWithTwoMandatories(decidedOneToOneFactTypeMappings, undecidedOneToOneFactTypeMappings) > 0;
			} while (changed);
		}

		#region Filter Algorithms Methods
		/// <summary>
		/// Filters out deep potential mappings that are from an <see cref="ObjectType"/> that already has a decided deep
		/// mapping from it.
		/// </summary>
		/// <param name="decidedOneToOneFactTypeMappings">The decided <see cref="FactTypeMapping"/> objects.</param>
		/// <param name="undecidedOneToOneFactTypeMappings">The undecided <see cref="FactTypeMapping"/> possibilities.</param>
		private void RemoveImpossiblePotentialFactTypeMappings(FactTypeMappingDictionary decidedOneToOneFactTypeMappings, FactTypeMappingListDictionary undecidedOneToOneFactTypeMappings)
		{
			Dictionary<ObjectType, object> deeplyMappedObjectTypes = new Dictionary<ObjectType, object>(decidedOneToOneFactTypeMappings.Count + undecidedOneToOneFactTypeMappings.Count);

			// For each decided fact type mapping...
			foreach (FactTypeMapping factTypeMapping in decidedOneToOneFactTypeMappings.Values)
			{
				// If it's a deep mapping...
				if (factTypeMapping.MappingDepth == MappingDepth.Deep)
				{
					deeplyMappedObjectTypes[factTypeMapping.FromObjectType] = null;
				}
			}

			List<FactType> factsPendingDeletion = new List<FactType>();

			foreach (KeyValuePair<FactType, FactTypeMappingList> undecidedFactTypeMapping in undecidedOneToOneFactTypeMappings)
			{
				FactType factType = undecidedFactTypeMapping.Key;
				FactTypeMappingList potentialFactTypeMappings = undecidedFactTypeMapping.Value;

				// For each potential fact type mapping...
				for (int i = potentialFactTypeMappings.Count - 1; i >= 0; i--)
				{
					FactTypeMapping potentialFactTypeMapping = potentialFactTypeMappings[i];

					// If it is maped away from an ObjectType that is already determined to be mapped elsewhere...
					if (potentialFactTypeMapping.MappingDepth == MappingDepth.Deep &&  deeplyMappedObjectTypes.ContainsKey(potentialFactTypeMapping.FromObjectType))
					{
						// Remove it as a possibility.
						potentialFactTypeMappings.RemoveAt(i);
					}
				}

				Debug.Assert(potentialFactTypeMappings.Count > 0);

				// If there is only one possibility left...
				if (potentialFactTypeMappings.Count == 1)
				{
					// Mark it decided.
					decidedOneToOneFactTypeMappings.Add(factType, potentialFactTypeMappings[0]);
					factsPendingDeletion.Add(factType);
				}
			}

			// Delete each undecided (now decided) fact type mapping marked for deletion.
			foreach (FactType key in factsPendingDeletion)
			{
				undecidedOneToOneFactTypeMappings.Remove(key);
			}
		}

		/// <summary>
		/// Maps one-to-one fact types with two simple alethic mandatories away from the object type that has no possible
		/// (potential or decided) deep mappings away from it.  If both role players meet this criteria then no action is
		/// taken.
		/// </summary>
		/// <param name="decidedOneToOneFactTypeMappings">The decided <see cref="FactTypeMapping"/> objects.</param>
		/// <param name="undecidedOneToOneFactTypeMappings">The undecided <see cref="FactTypeMapping"/> posibilities.</param>
		/// <returns>The number of previously potential one-to-one fact type mappings that are now decided.</returns>
		private int MapTrivialOneToOneFactTypesWithTwoMandatories(FactTypeMappingDictionary decidedOneToOneFactTypeMappings, FactTypeMappingListDictionary undecidedOneToOneFactTypeMappings)
		{
			List<FactType> factTypesPendingDeletion = new List<FactType>();

			foreach (KeyValuePair<FactType, FactTypeMappingList> undecidedPair in undecidedOneToOneFactTypeMappings)
			{
				FactType factType = undecidedPair.Key;
				FactTypeMappingList potentialFactTypeMappings = undecidedPair.Value;
				LinkedElementCollection<RoleBase> roles = factType.RoleCollection;

				Debug.Assert(roles.Count == 2, "All fact type mappings should be for fact types with exactly two roles.");

				Role firstRole = roles[0].Role;
				Role secondRole = roles[1].Role;
				ObjectType firstRolePlayer = firstRole.RolePlayer;
				ObjectType secondRolePlayer = secondRole.RolePlayer;

				// If this is a one-to-one fact type with two simple alethic mandatories...
				if (firstRole.SingleRoleAlethicMandatoryConstraint != null && secondRole.SingleRoleAlethicMandatoryConstraint != null)
				{
					FactTypeMapping deepMappingTowardsFirstRole = null;
					FactTypeMapping deepMappingTowardsSecondRole = null;

					// Find our potential deep mappings.
					foreach (FactTypeMapping potentialFactTypeMapping in potentialFactTypeMappings)
					{
						if (potentialFactTypeMapping.MappingDepth == MappingDepth.Deep)
						{
							if (potentialFactTypeMapping.TowardsRole == firstRole)
							{
								deepMappingTowardsFirstRole = potentialFactTypeMapping;
							}
							else
							{
								Debug.Assert(potentialFactTypeMapping.TowardsRole == secondRole);
								deepMappingTowardsSecondRole = potentialFactTypeMapping;
							}
						}
					}

					bool firstRolePlayerHasPossibleDeepMappingsAway = ObjectTypeHasPossibleDeepMappingsAway(firstRolePlayer, factType, decidedOneToOneFactTypeMappings, undecidedOneToOneFactTypeMappings);
					bool secondRolePlayerHasPossibleDeepMappingsAway = ObjectTypeHasPossibleDeepMappingsAway(secondRolePlayer, factType, decidedOneToOneFactTypeMappings, undecidedOneToOneFactTypeMappings);

					// UNDONE: We need to do cycle checking at or before this point, since otherwise this can create a decided deep mapping cycle.

					// If secondRolePlayer has no possible deep mappings away from it, and firstRolePlayer does...
					if (firstRolePlayerHasPossibleDeepMappingsAway && !secondRolePlayerHasPossibleDeepMappingsAway)
					{
						// Make sure that this deep mapping was one of our potentials.
						if (deepMappingTowardsFirstRole != null)
						{
							Debug.Assert(firstRole.SingleRoleAlethicUniquenessConstraint != null);
							// Make sure we're not going towards a preferred identifier. Doing so is valid, and sometimes optimal,
							// but not always, hence the permuter needs to consider it.
							if (!firstRole.SingleRoleAlethicUniquenessConstraint.IsPreferred)
							{
								// Deep map toward firstRolePlayer
								decidedOneToOneFactTypeMappings.Add(factType, deepMappingTowardsFirstRole);
								factTypesPendingDeletion.Add(factType);
							}
						}
					}
					// ...and vice-versa...
					else if (!firstRolePlayerHasPossibleDeepMappingsAway && secondRolePlayerHasPossibleDeepMappingsAway)
					{
						// Make sure that this deep mapping was one of our potentials.
						if (deepMappingTowardsSecondRole != null)
						{
							Debug.Assert(secondRole.SingleRoleAlethicUniquenessConstraint != null);
							// Make sure we're not going towards a preferred identifier. Doing so is valid, and sometimes optimal,
							// but not always, hence the permuter needs to consider it.
							if (!secondRole.SingleRoleAlethicUniquenessConstraint.IsPreferred)
							{
								// Deep map toward secondRolePlayer
								decidedOneToOneFactTypeMappings.Add(factType, deepMappingTowardsSecondRole);
								factTypesPendingDeletion.Add(factType);
							}
						}
					}
				}
			}

			// Delete each undecided (now decided) fact type mapping marked for deletion.
			foreach (FactType key in factTypesPendingDeletion)
			{
				undecidedOneToOneFactTypeMappings.Remove(key);
			}

			return factTypesPendingDeletion.Count;
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
				// NOTE: We don't need the ShouldIgnoreFactType filter here, because fact types that we want to ignore won't be in the dictionaries in the first place.
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
				if (decidedFactTypeMappingExists && decidedFactTypeMapping.MappingDepth == MappingDepth.Deep && decidedFactTypeMapping.FromObjectType == objectType)
				{
					return true;
				}
				else if (potentialFactTypeMappingsExist)
				{
					foreach (FactTypeMapping potentialFactTypeMapping in potentialFactTypeMappings)
					{
						// If there's a potential deep fact type mapping away from objectType...
						if (potentialFactTypeMapping.MappingDepth == MappingDepth.Deep && potentialFactTypeMapping.FromObjectType == objectType)
						{
							return true;
						}
					}
				}
			}

			return false;
		}
		#endregion // Filter Algorithms Methods

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

		#region Generation Algorithm Methods
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
				if (ShouldIgnoreObjectType(valueType))
				{
					continue;
				}
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
				if (ShouldIgnoreObjectType(objectType))
				{
					continue;
				}
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
		/// Generates the <see cref="ConceptTypeChild">concept type children</see> for each
		/// <see cref="ConceptType"/> in the <see cref="AbstractionModel">OIAL model</see>.
		/// </summary>
		/// <param name="factTypeMappings">
		/// The set of all decided <see cref="FactTypeMapping">fact type mappings</see>.
		/// </param>
		private void GenerateConceptTypeChildren(FactTypeMappingDictionary factTypeMappings)
		{
			List<FactType> factTypePath = new List<FactType>();

			foreach (ConceptType conceptType in this.AbstractionModel.ConceptTypeCollection)
			{
				ObjectType objectType = ConceptTypeIsForObjectType.GetObjectType(conceptType);

				// NOTE: We don't need the ShouldIgnoreObjectType filter here, because object
				// types that we want to ignore won't be in the OIAL model in the first place.

				foreach (Role playedRole in objectType.PlayedRoleCollection)
				{
					// NOTE: We don't need the ShouldIgnoreFactType filter here, because fact types that
					// we want to ignore won't be in the set of fact type mappings in the first place.
					FactType playedFactType = playedRole.BinarizedFactType;

					FactTypeMapping factTypeMapping;

					if (factTypeMappings.TryGetValue(playedFactType, out factTypeMapping) &&
						factTypeMapping.TowardsRole == playedRole)
					{
						// The fact type has a mapping and that mapping is towards the role played by
						// this concept type, so we need to generate concept type children for it.
						GenerateConceptTypeChildrenForFactTypeMapping(factTypeMappings, conceptType, factTypeMapping, factTypePath, true);
					}
				}
			}
		}

		/// <summary>
		/// Generates the appropriate <see cref="ConceptTypeChild">concept type children</see> in <paramref name="parentConceptType"/>
		/// for <paramref name="factTypeMapping"/>.
		/// </summary>
		/// <param name="factTypeMappings">
		/// The set of all decided <see cref="FactTypeMapping">fact type mappings</see>.
		/// </param>
		/// <param name="parentConceptType">
		/// The <see cref="ConceptType"/> into which <see cref="ConceptTypeChild">concept type children</see> should be generated.
		/// </param>
		/// <param name="factTypeMapping">
		/// The <see cref="FactTypeMapping"/> for which <see cref="ConceptTypeChild">concept type children</see> should be generated.
		/// </param>
		/// <param name="factTypePath">
		/// The path of <see cref="FactType">fact types</see> leading from <paramref name="parentConceptType"/> to <paramref name="factTypeMapping"/>
		/// </param>
		/// <param name="isMandatorySoFar">
		/// Indicates whether every step in <paramref name="factTypePath"/> is mandatory for the parent concept type (towards object type).
		/// </param>
		private static void GenerateConceptTypeChildrenForFactTypeMapping(FactTypeMappingDictionary factTypeMappings, ConceptType parentConceptType, FactTypeMapping factTypeMapping, List<FactType> factTypePath, bool isMandatorySoFar)
		{
			// Push the current fact type onto the path.
			factTypePath.Add(factTypeMapping.FactType);

			bool isMandatory = isMandatorySoFar && (factTypeMapping.TowardsRole.SingleRoleAlethicMandatoryConstraint != null);

			ConceptTypeChild newConceptTypeChild;

			ConceptType fromConceptType = ConceptTypeIsForObjectType.GetConceptType(factTypeMapping.FromObjectType);
			if (fromConceptType != null)
			{
				// The mapping is coming from a concept type, so we will create a concept type reference to it.

				// Set up the property assignments that are common to both kinds of concept type references.
				PropertyAssignment isMandatoryPropertyAssignment = new PropertyAssignment(ConceptTypeChild.IsMandatoryDomainPropertyId, isMandatory);

				string name = ResolveRoleName(factTypeMapping.FromRole);
				string oppositeName = ResolveRoleName(factTypeMapping.TowardsRole);

				// UNDONE: Yes, these are backwards, but they need to remain so for compatibility reasons until we do a file format change.
				PropertyAssignment namePropertyAssignment = new PropertyAssignment(ConceptTypeChild.NameDomainPropertyId, oppositeName);
				PropertyAssignment oppositeNamePropertyAssignment = new PropertyAssignment(ConceptTypeReferencesConceptType.OppositeNameDomainPropertyId, name);

				if (factTypeMapping.MappingDepth == MappingDepth.Deep)
				{
					// Since this is a deep mapping, we will create a concept type assimilation for it.

					SubtypeFact subtypeFact = factTypeMapping.FactType as SubtypeFact;

					// UNDONE: The handling here for IsPreferredForParent and IsPreferredForTarget may not be correct
					// if we have more than one fact type in the fact type path.

					bool isPreferredForTarget;
					if (subtypeFact != null)
					{
						// For subtype assimilations, IsPreferredForTarget matches the ProvidesPreferredIdentifier
						// property of the ORM subtype fact.
						isPreferredForTarget = subtypeFact.ProvidesPreferredIdentifier;
					}
					else
					{
						// For non-subtype assimilations, IsPreferredForTarget is true if the role played by the object
						// type corresponding to the parent concept type has the preferred identifying uniqueness constraint
						// for the target concept type.
						isPreferredForTarget = factTypeMapping.TowardsRole.SingleRoleAlethicUniquenessConstraint.IsPreferred;
					}

					bool isPreferredForParent = factTypeMapping.IsFromPreferredIdentifier;
					// The IsPreferredForParent property on concept type assimilations indicates that the assimilation, on its own,
					// provides the preferred identifier for the assimilating concept type. Although the IsFromPreferredIdentifier
					// property on the fact type mapping will be true even if the from role is part of a multi-role preferred identifier,
					// ORM currently doesn't allow a role with a single role alethic uniqueness constraint (which is required for this to
					// be a deep mapping) to be part of any other uniqueness constraint. However, this may change in the future as our
					// handling of derivations, implications, equivalences, and logical rules becomes more sophisticated. We assert here
					// in order to make this case easier to catch if it happens, since this method may need to be adjusted in that case
					// to ensure that it continues to produce correct results.
					Debug.Assert(!isPreferredForParent || factTypeMapping.FromRole.SingleRoleAlethicUniquenessConstraint.IsPreferred);

					newConceptTypeChild = new ConceptTypeAssimilatesConceptType(parentConceptType.Partition,
						new RoleAssignment[]
						{
							new RoleAssignment(ConceptTypeAssimilatesConceptType.AssimilatorConceptTypeDomainRoleId, parentConceptType),
							new RoleAssignment(ConceptTypeAssimilatesConceptType.AssimilatedConceptTypeDomainRoleId, fromConceptType)
						},
						new PropertyAssignment[]
						{
							isMandatoryPropertyAssignment,
							namePropertyAssignment,
							oppositeNamePropertyAssignment,
							new PropertyAssignment(ConceptTypeAssimilatesConceptType.RefersToSubtypeDomainPropertyId, subtypeFact != null),
							new PropertyAssignment(ConceptTypeAssimilatesConceptType.IsPreferredForParentDomainPropertyId, isPreferredForParent),
							new PropertyAssignment(ConceptTypeAssimilatesConceptType.IsPreferredForTargetDomainPropertyId, isPreferredForTarget),
						});
				}
				else
				{
					Debug.Assert(factTypeMapping.MappingDepth == MappingDepth.Shallow,
						"Collapse mappings should not come from object types that have a concept type.");
					
					// Since this is a shallow mapping, we will create a concept type relation for it.
					newConceptTypeChild = new ConceptTypeRelatesToConceptType(parentConceptType.Partition,
						new RoleAssignment[]
						{
							new RoleAssignment(ConceptTypeRelatesToConceptType.RelatingConceptTypeDomainRoleId, parentConceptType),
							new RoleAssignment(ConceptTypeRelatesToConceptType.RelatedConceptTypeDomainRoleId, fromConceptType)
						},
						new PropertyAssignment[]
						{
							isMandatoryPropertyAssignment,
							namePropertyAssignment,
							oppositeNamePropertyAssignment
						});
				}
			}
			else
			{
				// The mapping is not coming from a concept type, meaning that we either need an information
				// type for an atomic value type (which will already have an information type format created
				// for it), or we need to collapse the preferred identifier of an entity type or structured
				// value type.

				InformationTypeFormat fromInformationTypeFormat = InformationTypeFormatIsForValueType.GetInformationTypeFormat(factTypeMapping.FromObjectType);
				if (fromInformationTypeFormat != null)
				{
					// We have an information type format, which means that we need to create an information type.

					string name = ResolveRoleName(factTypeMapping.FromRole);

					newConceptTypeChild = new InformationType(parentConceptType.Partition,
						new RoleAssignment[]
						{
							new RoleAssignment(InformationType.ConceptTypeDomainRoleId, parentConceptType),
							new RoleAssignment(InformationType.InformationTypeFormatDomainRoleId, fromInformationTypeFormat)
						},
						new PropertyAssignment[]
						{
							new PropertyAssignment(ConceptTypeChild.IsMandatoryDomainPropertyId, isMandatory),
							new PropertyAssignment(ConceptTypeChild.NameDomainPropertyId, name)
						});
				}
				else
				{
					// We do not have an information type format, which means that we need to collapse the fact
					// types in the preferred identifier of the FromObjectType into the parent concept type.

					newConceptTypeChild = null;

					UniquenessConstraint preferredIdentifier = factTypeMapping.FromObjectType.PreferredIdentifier;
					Debug.Assert(preferredIdentifier != null);

					foreach (Role preferredIdentifierRole in preferredIdentifier.RoleCollection)
					{
						// NOTE: We don't need the ShouldIgnoreFactType filter here, because we would have ignored
						// this object type if we were ignoring any of the fact types in its preferred identifier.
						FactType preferredIdentifierFactType = preferredIdentifierRole.BinarizedFactType;

						FactTypeMapping preferredIdentifierFactTypeMapping = factTypeMappings[preferredIdentifierFactType];

						if (preferredIdentifierFactType == factTypeMapping.FactType)
						{
							// We just got back to the fact that we were already mapping. This should only happen
							// when the object type has a single fact type in its preferred identifier and it is
							// deeply mapped away from the object type.
							Debug.Assert(preferredIdentifier.RoleCollection.Count == 1 && preferredIdentifierFactTypeMapping.MappingDepth == MappingDepth.Deep);

							// UNDONE: For now, we just ignore this fact type entirely. What we should be doing is:
							// 1) If everything along the path is mandatory, then we're done, since we know that instances of the parent
							// concept type always identify an instance of the object type that we're trying to map.
							// 2) Otherwise, check if there are any other relationships that would allow use to derive whether an instance
							// of the parent concept type identifies an instance of the object type that we're trying to map. Examples
							// of things that would allow us to do this would be a mandatory role played by the object type that we're
							// we're trying to map that gets absorbed into some concept type. The reference to an instance of this concept
							// type from it allows us to tell if this instance identifies an instance of the object type.
							// 3) If no other relationship allows us to derive this information, we need to add a boolean information type
							// that indicates for each instance of the parent concept type whether it identifies an instance of the object
							// type that we're trying to map.
							break;
						}

						// If we have a single fact type in the preferred identifier, it might be mapped
						// deeply away from the object type that we are collapsing. For this case, we need
						// to create a "fake" mapping and process it instead.
						if (preferredIdentifierFactTypeMapping.TowardsRole == preferredIdentifierRole)
						{
							// Make sure this is actually the situation we are trying to handle, since it shouldn't be possible in any other scenario.
							Debug.Assert(preferredIdentifier.RoleCollection.Count == 1 && preferredIdentifierFactTypeMapping.MappingDepth == MappingDepth.Deep);

							// UNDONE: Would we ever want to use a depth other than shallow here? Probably not, but it might be worth looking in to.
							preferredIdentifierFactTypeMapping = new FactTypeMapping(preferredIdentifierFactType, preferredIdentifierFactTypeMapping.TowardsRole, preferredIdentifierFactTypeMapping.FromRole, MappingDepth.Shallow);
						}

						GenerateConceptTypeChildrenForFactTypeMapping(factTypeMappings, parentConceptType, preferredIdentifierFactTypeMapping, factTypePath, isMandatory);
					}
				}
			}

			// If we created a new concept type child, populate its fact type path.
			if (newConceptTypeChild != null)
			{
				foreach (FactType pathFactType in factTypePath)
				{
					ConceptTypeChildHasPathFactType conceptTypeChildHasPathFactType = new ConceptTypeChildHasPathFactType(newConceptTypeChild, pathFactType);
				}
			}

			// Pop the current fact type off of the path.
			Debug.Assert(factTypePath[factTypePath.Count - 1] == factTypeMapping.FactType, "Fact type path stack is corrupt.");
			factTypePath.RemoveAt(factTypePath.Count - 1);
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
					if (ShouldIgnoreFactType(role.BinarizedFactType))
					{
						continue;
					}

					Role oppositeRole = role.OppositeRoleAlwaysResolveProxy.Role;

					// For each constraint on the opposite role...
					foreach (ConstraintRoleSequence constraintRoleSequence in oppositeRole.ConstraintRoleSequenceCollection)
					{
						UniquenessConstraint uninquenessConstraint = constraintRoleSequence as UniquenessConstraint;

						// If it is a uniqueness constraint...
						if (uninquenessConstraint != null && uninquenessConstraint.Modality == ConstraintModality.Alethic)
						{
							if (UniquenessIsForUniquenessConstraint.GetUniqueness(uninquenessConstraint) != null)
							{
								continue;
							}

							bool hasFactTypeThatShouldBeIgnored = false;
							bool allChildrenMapTowardObjectType = true;
							IList<FactType> factTypes = new List<FactType>();

							foreach (Role childRole in uninquenessConstraint.RoleCollection)
							{
								FactType binarizedFactType = childRole.BinarizedFactType;
								if (ShouldIgnoreFactType(binarizedFactType))
								{
									hasFactTypeThatShouldBeIgnored = true;
									break;
								}
								FactTypeMapping factTypeMapping = factTypeMappings[binarizedFactType];

								if (factTypeMapping.TowardsRole != childRole.OppositeRoleAlwaysResolveProxy.Role)
								{
									allChildrenMapTowardObjectType = false;
									break;
								}
								else
								{
									factTypes.Add(binarizedFactType);
								}
							}
							if (hasFactTypeThatShouldBeIgnored)
							{
								continue;
							}

							if (allChildrenMapTowardObjectType)
							{
								IList<ConceptTypeChild> conceptTypeChildren = new List<ConceptTypeChild>();
								bool skipThisUniquenessConstraint = false;

								foreach (FactType factType in factTypes)
								{
									bool childWasAssimilation = false;
									bool missedChild = true;
									foreach (ConceptTypeChild conceptTypeChild in ConceptTypeChildHasPathFactType.GetConceptTypeChild(factType))
									{
										if (conceptTypeChild.Parent != conceptType)
										{
											// This ConceptTypeChild is of a different ConceptType, so go on to the next ConceptTypeChild.
											continue;
										}
										if (conceptTypeChild is ConceptTypeAssimilatesConceptType)
										{
											childWasAssimilation = true;
											break;
										}

										missedChild = false;
										conceptTypeChildren.Add(conceptTypeChild);
									}

									if (childWasAssimilation)
									{
										skipThisUniquenessConstraint = true;
										break;
									}
									if (missedChild)
									{
										// We couldn't find a ConceptTypeChild for this FactType, so just bail out.
										skipThisUniquenessConstraint = true;
										break;
									}
								}

								if (!skipThisUniquenessConstraint)
								{
									PropertyAssignment name = new PropertyAssignment(Uniqueness.NameDomainPropertyId, uninquenessConstraint.Name);
									PropertyAssignment isPreferred = new PropertyAssignment(Uniqueness.IsPreferredDomainPropertyId, uninquenessConstraint.IsPreferred);
									PropertyAssignment[] propertyAssignments = { name, isPreferred };

									// Create uniquenesss
									Uniqueness uniqueness = new Uniqueness(Store, propertyAssignments);
									uniqueness.ConceptType = conceptType;
									new UniquenessIsForUniquenessConstraint(uniqueness, uninquenessConstraint);

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
				// Note that there will currently be no existing maps, but this is
				// likely to change once we do more incremental work. If the FactTypeMapsTowardsRole
				// instance already exists for this FactType, then the following will throw.
				// Any role changes here need to properly update the existing link's mapping
				// pattern properties. This should be done with an OnRolePlayerChanged call
				// (or a rule) inside FactTypeMapsTowardsRole.
				FactTypeMapsTowardsRole.Create(mapping.FactType, mapping.TowardsRole, mapping.MappingDepth);
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
					LinkedElementCollection<ConceptTypeChild> associationChildren = ConceptTypeHasChildAsPartOfAssociation.GetTargetCollection(ct);

					// CT becomes an associationChild for all concept types related to the binarized fact types
					foreach (FactType factType in objectification.ImpliedFactTypeCollection)
					{
						foreach (ConceptTypeChild conceptTypeChild in ConceptTypeChildHasPathFactType.GetConceptTypeChild(factType))
						{
							if (conceptTypeChild.Parent == ct || conceptTypeChild.Target == ct)
							{
								associationChildren.Add(conceptTypeChild);
							}
						} 
					}
				}
			}
		}
		#endregion // Generation Algorithm Methods
		#region Helper Methods
		/// <summary>
		/// Resolve the name that will be used for a <see cref="ConceptTypeChild"/> given the <see cref="Role"/> it's resulting from.
		/// </summary>
		/// <param name="role">The <see cref="Role"/> that the <see cref="ConceptTypeChild"/> is resulting from.</param>
		/// <returns>The name to use for the <see cref="ConceptTypeChild"/>.</returns>
		private static string ResolveRoleName(Role role)
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
					if (ShouldIgnoreFactType(factType))
					{
						continue;
					}
					FactTypeMapping factTypeMapping = factTypeMappings[factType];

					if (factTypeMapping.FromObjectType == objectType && factTypeMapping.MappingDepth == MappingDepth.Deep)
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
		/// <remarks>
		/// An <see cref="ObjectType"/> should have a <see cref="ConceptType"/> if:
		/// <list>
		/// <item><description>It is independent.</description></item>
		/// <item><description>It is a subtype.</description></item>
		/// <item><description>It has a <see cref="FactTypeMapping"/> towards it for a <see cref="FactType"/> that is not part of its preferred identifier.</description></item>
		/// </list>
		/// </remarks>
		/// <param name="objectType">The <see cref="ObjectType"/> to test.</param>
		/// <param name="factTypeMappings">A dictionary of all the final decided <see cref="FactTypeMapping"/> objects.</param>
		/// <returns>Returns true if the <see cref="ObjectType"/> should have <see cref="ConceptType"/>.</returns>
		private bool ObjectTypeIsConceptType(ObjectType objectType, FactTypeMappingDictionary factTypeMappings)
		{
			// If objectType is independent...
			if (objectType.TreatAsIndependent)
			{
				return true;
			}

			foreach (Role role in ObjectTypePlaysRole.GetPlayedRoleCollection(objectType))
			{
				FactType factType = role.BinarizedFactType;
				if (ShouldIgnoreFactType(factType))
				{
					continue;
				}

				// If it is a subtype fact, we need a concept type. Although the algorithm only calls for this in the case
				// of subtype meta roles, supertype meta roles will always match the patterns below, so we can immediately
				// return true for them as well.
				if (factType is SubtypeFact)
				{
					return true;
				}

				FactTypeMapping factTypeMapping = factTypeMappings[factType];

				// If fact type mapping is toward objectType...
				if (factTypeMapping.TowardsObjectType == objectType)
				{
					bool isPartOfPreferredIdentifier = false;
					foreach (ConstraintRoleSequence constraintRoleSequence in factTypeMapping.FromRole.ConstraintRoleSequenceCollection)
					{
						UniquenessConstraint uninquenessConstraint = constraintRoleSequence as UniquenessConstraint;
						if (uninquenessConstraint != null && uninquenessConstraint.IsPreferred)
						{
							isPartOfPreferredIdentifier = true;
							break;
						}
					}

					if (!isPartOfPreferredIdentifier)
					{
						// This FactType is not part of the preferred identifier.
						return true;
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
					FactType factType = role.FactType;
					// UNDONE: Double check whether this actually needs to be filtered. -Kevin
					if (ShouldIgnoreFactType(factType))
					{
						continue;
					}
					Role oppositeRole = role.OppositeRoleAlwaysResolveProxy.Role;

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
