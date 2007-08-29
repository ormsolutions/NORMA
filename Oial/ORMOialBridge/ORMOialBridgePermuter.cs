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
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORMAbstraction;
using Microsoft.VisualStudio.Modeling;
using System.IO;
using System.Diagnostics;

namespace Neumont.Tools.ORMToORMAbstractionBridge
{
	#region Chain calculator
	sealed class FactTypeChainer
	{
		private readonly FactTypeMappingDictionary myPredecidedManyToOneFactTypeMappings;
		private readonly FactTypeMappingDictionary myPredecidedOneToOneFactTypeMappings;
		private readonly FactTypeMappingListDictionary myUndecidedOneToOneFactTypeMappings;
		private readonly ChainList myChains;

		public FactTypeChainer(FactTypeMappingDictionary predecidedManyToOneFactTypeMappings, FactTypeMappingDictionary predecidedOneToOneFactTypeMappings, FactTypeMappingListDictionary undecidedOneToOneFactTypeMappings)
		{
			myChains = new ChainList();

			myPredecidedManyToOneFactTypeMappings = predecidedManyToOneFactTypeMappings;
			myPredecidedOneToOneFactTypeMappings = predecidedOneToOneFactTypeMappings;
			myUndecidedOneToOneFactTypeMappings = undecidedOneToOneFactTypeMappings;
		}

		public int Run()
		{
			BuildChains();

			// Delete empty chains and find the largest chain.
			int largestChainCount = 0;
			for (int i = myChains.Count - 1; i >= 0; i--)
			{
				Chain chain = myChains[i];
				if (chain.UndecidedOneToOneFactTypeMappings.Count <= 0)
				{
					myChains.RemoveAt(i);
					continue;
				}
				int chainCount = chain.OneToOneFactTypeCount;
				if (chainCount > largestChainCount)
				{
					largestChainCount = chainCount;
				}
			}

			return largestChainCount;
		}

		public ChainList Chains
		{
			get { return myChains; }
		}

		private void BuildChains()
		{
			int factTypesCount = myUndecidedOneToOneFactTypeMappings.Count + myPredecidedOneToOneFactTypeMappings.Count;
			Dictionary<FactType, object> visitedFactTypes = new Dictionary<FactType, object>(factTypesCount);
			Dictionary<ObjectType, object> visitedObjectTypes = new Dictionary<ObjectType, object>(factTypesCount * 2);


			FactTypeMappingDictionary.Enumerator predecidedEnumerator = myPredecidedOneToOneFactTypeMappings.GetEnumerator();
			FactTypeMappingListDictionary.Enumerator undecidedEnumerator = myUndecidedOneToOneFactTypeMappings.GetEnumerator();
			while (true)
			{
				KeyValuePair<FactType, FactTypeMapping> predecidedPair;
				KeyValuePair<FactType, FactTypeMappingList> undecidedPair;
				FactType factType;
				// Find a fact type that we haven't already visited.
				while (true)
				{
					if (predecidedEnumerator.MoveNext())
					{
						predecidedPair = predecidedEnumerator.Current;
						factType = predecidedPair.Key;
					}
					else if (undecidedEnumerator.MoveNext())
					{
						undecidedPair = undecidedEnumerator.Current;
						factType = undecidedPair.Key;
					}
					else
					{
						return;
					}

					if (!visitedFactTypes.ContainsKey(factType))
					{
						// We've found a fact type that hasn't been visited, so break out of the loop.
						break;
					}
				}

				// We've found a new path, so create a chain for it.
				Chain chain = new Chain();
				myChains.Add(chain);

				// Assuming ProcessObjectType works correctly, we will never hit an object type a second time,
				// so we can clear the Dictionary here and then use it as a record of all object types in the chain.
				visitedObjectTypes.Clear();

				ProcessObjectType(factType.RoleCollection[0].Role.RolePlayer, chain, visitedFactTypes, visitedObjectTypes);

				// Record all object types that are in the chain.
				chain.ObjectTypes.AddRange(visitedObjectTypes.Keys);
			}
		}
		private void ProcessObjectType(ObjectType objectType, Chain chain, Dictionary<FactType, object> visitedFactTypes, Dictionary<ObjectType, object> visitedObjectTypes)
		{
			if (visitedObjectTypes.ContainsKey(objectType))
			{
				// We've already visited this object type.
				return;
			}
			// Record that this object type has been visited.
			visitedObjectTypes[objectType] = null;
			foreach (Role role in objectType.PlayedRoleCollection)
			{
				FactType factType = role.BinarizedFactType;
				// If we've already visited this fact type, go on to the next one.
				if (visitedFactTypes.ContainsKey(factType))
				{
					continue;
				}

				// Indicates whether we should follow the fact type and include
				// the object type at the other end of it in the current chain.
				bool traverseFactType;
				FactTypeMapping mapping;
				FactTypeMappingList mappingList;
				// Check for undecided one-to-one mappings...
				if (myUndecidedOneToOneFactTypeMappings.TryGetValue(factType, out mappingList))
				{
					traverseFactType = true;
					chain.UndecidedOneToOneFactTypeMappings.Add(mappingList);
				}
				// ...or predecided one-to-one mappings...
				else if (myPredecidedOneToOneFactTypeMappings.TryGetValue(factType, out mapping))
				{
					traverseFactType = true;
					chain.PredecidedOneToOneFactTypeMappings.Add(mapping);
				}
				// ...or predecided many-to-one mappings towards this object type.
				else if (myPredecidedManyToOneFactTypeMappings.TryGetValue(factType, out mapping) && mapping.TowardsObjectType == objectType)
				{
					traverseFactType = false;
					chain.PredecidedManyToOneFactTypeMappings.Add(mapping);
				}
				else
				{
					continue;
				}

				// Record that this fact type has been visited.
				visitedFactTypes[factType] = null;

				// We don't want to include the object type at the other end of this
				// fact type in the chain, so continue on with the next played role.
				if (!traverseFactType)
				{
					continue;
				}

				// At most one of the two roles will be played by a different object type.
				LinkedElementCollection<RoleBase> roles = factType.RoleCollection;
				Debug.Assert(roles.Count == 2);
				ObjectType objectType1 = roles[0].Role.RolePlayer;
				if (objectType1 != objectType)
				{
					// We found the role played by a different object type, so there is no need to check the other role.
					ProcessObjectType(objectType1, chain, visitedFactTypes, visitedObjectTypes);
				}
				else
				{
					// The first role was played by this object tpye, so we need to check the second role.
					ObjectType objectType2 = roles[1].Role.RolePlayer;
					if (objectType2 != objectType)
					{
						ProcessObjectType(objectType2, chain, visitedFactTypes, visitedObjectTypes);
					}
				}
			}
		}
	}
	#endregion

	#region FactTypeMappingPermuter
	sealed class FactTypeMappingPermuter
	{
		private readonly FactTypeMappingDictionary myPredecidedManyToOneFactTypeMappings;
		private readonly FactTypeMappingDictionary myPredecidedOneToOneFactTypeMappings;
		private readonly FactTypeMappingListDictionary myUndecidedOneToOneFactTypeMappings;

		private readonly FactTypeMappingDictionary myDecidedFactTypeMappings;


		public FactTypeMappingPermuter(FactTypeMappingDictionary predecidedManyToOneFactTypeMappings, FactTypeMappingDictionary predecidedOneToOneFactTypeMappings, FactTypeMappingListDictionary undecidedOneToOneFactTypeMappings)
		{
			myPredecidedManyToOneFactTypeMappings = predecidedManyToOneFactTypeMappings;
			myPredecidedOneToOneFactTypeMappings = predecidedOneToOneFactTypeMappings;
			myUndecidedOneToOneFactTypeMappings = undecidedOneToOneFactTypeMappings;

			myDecidedFactTypeMappings = new FactTypeMappingDictionary(predecidedManyToOneFactTypeMappings.Count + predecidedOneToOneFactTypeMappings.Count + undecidedOneToOneFactTypeMappings.Count);

			foreach (KeyValuePair<FactType, FactTypeMapping> pair in predecidedManyToOneFactTypeMappings)
			{
				myDecidedFactTypeMappings.Add(pair.Key, pair.Value);
			}
			foreach (KeyValuePair<FactType, FactTypeMapping> pair in predecidedOneToOneFactTypeMappings)
			{
				myDecidedFactTypeMappings.Add(pair.Key, pair.Value);
			}
		}

		/// <summary>
		/// Runs the permuter, and adds the final decided mapping to the decidedFactTypeMappings dictionary specified when this instance was constructed.
		/// </summary>
		public FactTypeMappingDictionary Run()
		{
			PermuteFactTypeMappings();
			return myDecidedFactTypeMappings;
		}

		private void PermuteFactTypeMappings()
		{
			// UNDONE: We should consider whether we can determine what object types will collapse earlier in the process, which would allow us to
			// potentially eliminate multiple permutations that have the same result. (Rationale: When an object type is collapsed, it doesn't matter
			// whether the mappings away from it are shallow or deep; they both result in the same output.)

			int largestChainCount;
			// Break up the chains of contiguous one-to-one fact types
			FactTypeChainer chainer = new FactTypeChainer(myPredecidedManyToOneFactTypeMappings, myPredecidedOneToOneFactTypeMappings, myUndecidedOneToOneFactTypeMappings);
			largestChainCount = chainer.Run();

			// Perform one-time pass of top-level types for the decided mappings
			//PrecalculateDecidedConceptTypes();

			// This is used in PermuteFactTypeMappings(). We allocate it once, here, for permformance reasons.
			FactTypeMappingList newlyDecidedFactTypeMappings = new FactTypeMappingList(largestChainCount);

			// This is used to prevent deep mappings of an object type in two directions.
			Dictionary<ObjectType, object> deeplyMappedObjectTypes = new Dictionary<ObjectType, object>(largestChainCount);

			// Loop through each chain, calculating the permutations.
			foreach (Chain chain in chainer.Chains)
			{
				// Find the object types that we already know have deep mappings away from them.
				foreach (FactTypeMapping decidedMapping in chain.PredecidedOneToOneFactTypeMappings)
				{
					if (decidedMapping.MappingDepth == MappingDepth.Deep)
					{
						deeplyMappedObjectTypes[decidedMapping.FromObjectType] = null;
					}
				}

				// UNDONE: Eventually we should actually check this and warn the user if it would take too long on their machine.
				// This would need to be calculated, though. A hard-coded limit wouldn't be appropriate here.
				//int maxPermutations = CalculateMaxNumberOfPermutations(chain.UndecidedOneToOneFactTypeMappings);

				PermuteFactTypeMappings(chain.PossiblePermutations, chain.UndecidedOneToOneFactTypeMappings, newlyDecidedFactTypeMappings, deeplyMappedObjectTypes, 0);
				EliminateInvalidPermutations(chain);
				FindSmallestPermutationsInTermsOfConceptTypes(chain);
				EliminatePermutationsWithIdenticalResults(chain);

				// Add each mapping from the optimal permutation to the "global" set of decided mappings.
				foreach (FactTypeMapping optimalMapping in ChooseOptimalPermutation(chain).Mappings)
				{
					myDecidedFactTypeMappings.Add(optimalMapping.FactType, optimalMapping);
				}

				// Clear the set of object types that have some deep mapping away from them (they will be different for the next chain).
				deeplyMappedObjectTypes.Clear();
			}
		}

		private static FactTypeMapping FindDeepMappingAwayFromObjectType(ObjectType objectType, FactTypeMappingList predecidedOneToOneFactTypeMappings, FactTypeMappingList permutationFactTypeMappings)
		{
			// UNDONE: Figure out a way we can do this off of PlayedRoles instead, which should be faster.
			foreach (FactTypeMapping mapping in predecidedOneToOneFactTypeMappings)
			{
				if (mapping.MappingDepth == MappingDepth.Deep && mapping.FromObjectType == objectType)
				{
					return mapping;
				}
			}
			foreach (FactTypeMapping mapping in permutationFactTypeMappings)
			{
				if (mapping.MappingDepth == MappingDepth.Deep && mapping.FromObjectType == objectType)
				{
					return mapping;
				}
			}
			return null;
		}

		private static void FindSmallestPermutationsInTermsOfConceptTypes(Chain chain)
		{
			int maxPossibleObjectTypes = chain.ObjectTypes.Count;

			// These object types definitely DO result in concept types, but MAY or MAY NOT result in top-level concept types
			Dictionary<ObjectType, object> predecidedObjectTypesThatAreIndependentOrSubtypesOrHaveNonPreferredIdentifierMappingsTowards = new Dictionary<ObjectType, object>(maxPossibleObjectTypes);

			// These object types definitely DO NOT result in top-level concept types, but MAY or MAY NOT result in concept types
			Dictionary<ObjectType, object> predecidedObjectTypesThatHaveDeepMappingsAway = new Dictionary<ObjectType, object>(maxPossibleObjectTypes);

			// By just examining the predecided mappings, we can determine the specified truth values for the following:
			// Object Type is Concept Type				{true, unknown}
			// Object Type is Top Level Concept Type	{false, unknown}

			foreach (FactTypeMapping mapping in chain.PredecidedManyToOneFactTypeMappings)
			{
				Debug.Assert(mapping.MappingDepth == MappingDepth.Shallow);
				if (!mapping.IsFromPreferredIdentifier)
				{
					predecidedObjectTypesThatAreIndependentOrSubtypesOrHaveNonPreferredIdentifierMappingsTowards[mapping.TowardsObjectType] = null;
				}
			}

			foreach (FactTypeMapping mapping in chain.PredecidedOneToOneFactTypeMappings)
			{
				if (!mapping.IsFromPreferredIdentifier)
				{
					predecidedObjectTypesThatAreIndependentOrSubtypesOrHaveNonPreferredIdentifierMappingsTowards[mapping.TowardsObjectType] = null;
				}
				if (mapping.FromRole is SubtypeMetaRole)
				{
					predecidedObjectTypesThatAreIndependentOrSubtypesOrHaveNonPreferredIdentifierMappingsTowards[mapping.FromObjectType] = null;
				}
				if (mapping.MappingDepth == MappingDepth.Deep)
				{
					predecidedObjectTypesThatHaveDeepMappingsAway[mapping.FromObjectType] = null;
				}
			}

			foreach (ObjectType objectType in chain.ObjectTypes)
			{
				if (objectType.IsAnyIndependent())
				{
					predecidedObjectTypesThatAreIndependentOrSubtypesOrHaveNonPreferredIdentifierMappingsTowards[objectType] = null;
				}
			}

			Dictionary<ObjectType, object> permutationObjectTypesThatHaveNonPreferredIdentifierMappingsTowards = new Dictionary<ObjectType, object>(maxPossibleObjectTypes);
			Dictionary<ObjectType, object> permutationObjectTypesThatHaveDeepMappingsAway = new Dictionary<ObjectType, object>(maxPossibleObjectTypes);

			Dictionary<ObjectType, object> notConceptTypes = new Dictionary<ObjectType, object>(maxPossibleObjectTypes);
			Dictionary<ObjectType, object> nonTopLevelConceptTypes = new Dictionary<ObjectType, object>(maxPossibleObjectTypes);
			Dictionary<ObjectType, object> topLevelConceptTypes = new Dictionary<ObjectType, object>(maxPossibleObjectTypes);

			PermutationList smallestPermutationsInTermsOfConceptTypes = chain.SmallestPermutationsInTermsOfConceptTypes;

			int minTopLevelConceptTypesCount = int.MaxValue;
			int minNonTopLevelConceptTypesCount = int.MaxValue;

			foreach (Permutation permutation in chain.PossiblePermutations)
			{
				bool isNotOptimalPermutation = false;

				permutationObjectTypesThatHaveDeepMappingsAway.Clear();
				permutationObjectTypesThatHaveNonPreferredIdentifierMappingsTowards.Clear();

				notConceptTypes.Clear();
				nonTopLevelConceptTypes.Clear();
				topLevelConceptTypes.Clear();

				foreach (FactTypeMapping mapping in permutation.Mappings)
				{
					if (!mapping.IsFromPreferredIdentifier)
					{
						permutationObjectTypesThatHaveNonPreferredIdentifierMappingsTowards[mapping.TowardsObjectType] = null;
					}
					if (mapping.MappingDepth == MappingDepth.Deep)
					{
						permutationObjectTypesThatHaveDeepMappingsAway[mapping.FromObjectType] = null;
					}
				}

				foreach (ObjectType objectType in chain.ObjectTypes)
				{
					bool isIndependentOrSubtypeOrHasNonPreferredIdentifierMappingsTowards = predecidedObjectTypesThatAreIndependentOrSubtypesOrHaveNonPreferredIdentifierMappingsTowards.ContainsKey(objectType) ||
						permutationObjectTypesThatHaveNonPreferredIdentifierMappingsTowards.ContainsKey(objectType);
					bool hasDeepMappingsAway = predecidedObjectTypesThatHaveDeepMappingsAway.ContainsKey(objectType) || permutationObjectTypesThatHaveDeepMappingsAway.ContainsKey(objectType);

					if (isIndependentOrSubtypeOrHasNonPreferredIdentifierMappingsTowards)
					{
						if (hasDeepMappingsAway)
						{
							nonTopLevelConceptTypes[objectType] = null;
							if ((topLevelConceptTypes.Count == minTopLevelConceptTypesCount) && (nonTopLevelConceptTypes.Count > minNonTopLevelConceptTypesCount))
							{
								// We now have more non-top-level concept types than the minimum, and the same number of top-level concept types as the minimum, so throw this permutation out.
								isNotOptimalPermutation = true;
								break;
							}
						}
						else
						{
							topLevelConceptTypes[objectType] = null;
							if (topLevelConceptTypes.Count > minTopLevelConceptTypesCount)
							{
								// We now have more top-level concept types than the minimum, so throw this permutation out.
								isNotOptimalPermutation = true;
								break;
							}
						}
					}
				}

				if (isNotOptimalPermutation)
				{
					// This isn't an optimal permutation, so go on to the next one.
					continue;
				}

				Debug.Assert(topLevelConceptTypes.Count <= minTopLevelConceptTypesCount, "Permutations with greater than the minimum number of top-level concept types should have been rejected inline.");

				if (topLevelConceptTypes.Count < minTopLevelConceptTypesCount)
				{
					// We have a new minimum number of top-level concept types (and hence a new minimum number of non-top-level concept types as well).
					minTopLevelConceptTypesCount = topLevelConceptTypes.Count;
					minNonTopLevelConceptTypesCount = nonTopLevelConceptTypes.Count;
					smallestPermutationsInTermsOfConceptTypes.Clear();
				}
				else
				{
					// We have the same number of top-level concept types as the minimum, so we need to check the number of non-top-level concept types.
					if (nonTopLevelConceptTypes.Count > minNonTopLevelConceptTypesCount)
					{
						// This isn't an optimal permutation, so go on to the next one.
						continue;
					}
					else if (nonTopLevelConceptTypes.Count < minNonTopLevelConceptTypesCount)
					{
						// We have a new minimum number of non-top-level concept type.
						minNonTopLevelConceptTypesCount = nonTopLevelConceptTypes.Count;
						smallestPermutationsInTermsOfConceptTypes.Clear();
					}
				}
				permutation.SetConceptTypes(topLevelConceptTypes, nonTopLevelConceptTypes);
				smallestPermutationsInTermsOfConceptTypes.Add(permutation);
			}
		}

		/// <summary>
		/// Eliminates <see cref="Permutation"/>s that have the same set of top-level concept types and non-top-level concept types,
		/// which will always result in the same OIAL.
		/// </summary>
		private static void EliminatePermutationsWithIdenticalResults(Chain chain)
		{
			PermutationList smallestPermutationsInTermsOfConceptTypes = chain.SmallestPermutationsInTermsOfConceptTypes;
			for (int currentPermutationIndex = 0; currentPermutationIndex < smallestPermutationsInTermsOfConceptTypes.Count - 1; currentPermutationIndex++)
			{
				Permutation currentPermutation = smallestPermutationsInTermsOfConceptTypes[currentPermutationIndex];
				for (int comparisonPermutationIndex = smallestPermutationsInTermsOfConceptTypes.Count - 1; comparisonPermutationIndex > currentPermutationIndex; comparisonPermutationIndex--)
				{
					Permutation comparisonPermutation = smallestPermutationsInTermsOfConceptTypes[comparisonPermutationIndex];

					bool hasIdenticalConceptTypes = true;
					// Check if comparisonPermutation has the same top-level concept types.
					foreach (ObjectType topLevelConceptType in currentPermutation.TopLevelConceptTypes.Keys)
					{
						if (!comparisonPermutation.TopLevelConceptTypes.ContainsKey(topLevelConceptType))
						{
							hasIdenticalConceptTypes = false;
							break;
						}
					}
					if (hasIdenticalConceptTypes)
					{
						// Check if comparisonPermutation has the same non-top-level concept types.
						foreach (ObjectType nonTopLevelConceptType in currentPermutation.NonTopLevelConceptTypes.Keys)
						{
							if (!comparisonPermutation.NonTopLevelConceptTypes.ContainsKey(nonTopLevelConceptType))
							{
								hasIdenticalConceptTypes = false;
								break;
							}
						}

						if (hasIdenticalConceptTypes)
						{
							// comparisonPermutation has the same set of top-level and non-top-level concept types, so we can get rid of it.
							// It is entirely arbitrary which one we get rid of, so we'll keep the first since it is easier.
							smallestPermutationsInTermsOfConceptTypes.RemoveAt(comparisonPermutationIndex);
						}
					}
				}
			}
		}



		private static Permutation ChooseOptimalPermutation(Chain chain)
		{
			// UNDONE: This should do something smart!
			PermutationList smallestPermutationsInTermsOfConceptTypes = chain.SmallestPermutationsInTermsOfConceptTypes;
			Permutation firstFinalMappingState = smallestPermutationsInTermsOfConceptTypes[0];
			//smallestPermutationsInTermsOfConceptTypes.Clear();
			return firstFinalMappingState;
		}

		/// <summary>
		/// Returns the maximum number of <see cref="Permutation"/>s that could result for the specified set of undecided <see cref="FactTypeMapping"/>s.
		/// </summary>
		private static int CalculateMaxNumberOfPermutations(FactTypeMappingListList undecidedMappings)
		{
			int maxPermutations = 1;
			foreach (FactTypeMappingList mappingList in undecidedMappings)
			{
				maxPermutations *= mappingList.Count;
			}
			return maxPermutations;
		}

		/// <summary>
		/// Permutes the <see cref="FactTypeMapping"/>s, recursively.
		/// </summary>
		/// <param name="permutations">The collection containing <see cref="Permutation"/>s that have already been calculated, and to which newly calculated <see cref="Permutation"/>s are added.</param>
		/// <param name="undecidedMappings">The list of potential <see cref="FactTypeMapping"/>s for each <see cref="FactType"/> that need to be permuted.</param>
		/// <param name="decidedMappings">The <see cref="FactTypeMapping"/>s that have already been decided for this branch of the permutation. Should initially be empty.</param>
		/// <param name="deeplyMappedObjectTypes"><see cref="ObjectType"/>s that already have some deep <see cref="FactTypeMapping"/> away from them.</param>
		/// <param name="currentPosition">The index into <paramref name="undecidedMappings"/> that should be processed.</param>
		private static void PermuteFactTypeMappings(PermutationList permutations, FactTypeMappingListList undecidedMappings, FactTypeMappingList decidedMappings, Dictionary<ObjectType, object> deeplyMappedObjectTypes, int currentPosition)
		{
			int nextPosition = currentPosition + 1;
			bool isLastUndecided = (nextPosition == undecidedMappings.Count);
			foreach (FactTypeMapping potentialMapping in undecidedMappings[currentPosition])
			{
				if (potentialMapping.MappingDepth == MappingDepth.Deep && deeplyMappedObjectTypes.ContainsKey(potentialMapping.FromObjectType))
				{
					// We already have a deep mapping away from this object type, so we can skip this potential mapping (which would result in an illegal permutation).
					continue;
				}

				// Put this potential mapping onto the stack of decided mappings.
				decidedMappings.Add(potentialMapping);

				if (isLastUndecided)
				{
					// This is the end of the list of undecided fact types, so we can create a permutation.
					permutations.Add(new Permutation(new FactTypeMappingList(decidedMappings)));
				}
				else
				{
					if (potentialMapping.MappingDepth == MappingDepth.Deep)
					{
						// This is a deep mapping, so add the from object type to the set of deeply mapped object types.
						deeplyMappedObjectTypes[potentialMapping.FromObjectType] = null;
					}

					// Go on to the potential mappings for the next undecided fact type.
					PermuteFactTypeMappings(permutations, undecidedMappings, decidedMappings, deeplyMappedObjectTypes, nextPosition);

					if (potentialMapping.MappingDepth == MappingDepth.Deep)
					{
						// Remove the from object type from the set of deeply mapped object types.
						deeplyMappedObjectTypes.Remove(potentialMapping.FromObjectType);
					}
				}

				// Pop this potential mapping off the stack of decided mappings so that we can go on to the next potential mapping.
				decidedMappings.RemoveAt(decidedMappings.Count - 1);
			}
		}

		/// <summary>
		/// Eliminates any <see cref="Permutation"/>s that contain cyclical deep <see cref="FactTypeMapping"/>s.
		/// </summary>
		/// <param name="chain">
		/// The <see cref="Chain"/> for which invalid <see cref="Permutation"/>s should be eliminated.
		/// </param>
		private static void EliminateInvalidPermutations(Chain chain)
		{
			Debug.Assert(chain.UndecidedOneToOneFactTypeMappings.Count > 0);

			int factTypeCount = chain.OneToOneFactTypeCount;

			FactTypeMappingList predecidedOneToOneFactTypeMappings = chain.PredecidedOneToOneFactTypeMappings;
			PermutationList possiblePermutations = chain.PossiblePermutations;

			Dictionary<FactType, object> visited = new Dictionary<FactType, object>(factTypeCount);
			Dictionary<FactType, object> current = new Dictionary<FactType, object>(factTypeCount);

			Debug.Assert(possiblePermutations[0].Mappings.Count > 0);
			// All of the permutations will always contain the same number of mappings, so we can calculate it here.
			int mappingsCount = possiblePermutations[0].Mappings.Count;

			for (int permutationIndex = possiblePermutations.Count - 1; permutationIndex >= 0; permutationIndex--)
			{
				// We're checking a new permutation, so clear the visited dictionary.
				visited.Clear();
				bool permutationIsInvalid = false;
				Permutation permutation = possiblePermutations[permutationIndex];
				FactTypeMappingList mappings = permutation.Mappings;

				int mappingIndex = 0;
				do
				{
					FactTypeMapping mapping = null;
					FactType factType = null;
					// Find a deep mapping that we haven't already visited.
					while (mappingIndex < mappingsCount)
					{
						mapping = mappings[mappingIndex++];
						factType = mapping.FactType;
						if (mapping.MappingDepth == MappingDepth.Deep && !visited.ContainsKey(factType))
						{
							break;
						}
					}

					if (mapping == null)
					{
						break;
					}

					// Record that the fact type has been visited
					visited[factType] = null;
					if (mapping.MappingDepth != MappingDepth.Deep)
					{
						// If we hit this, we have no more deep mappings in the permutation.
						break;
					}

					// We're following a new path, so clear the current dictionary.
					current.Clear();
					// Follow this path until we hit an object type that has no deep mappings away from it, or find a cycle.
					while (true)
					{
						if (current.ContainsKey(factType))
						{
							// We're back to a fact type we already processed, which means that there is a cycle, and this permutation is illegal.
							possiblePermutations.RemoveAt(permutationIndex);
							permutationIsInvalid = true;
							break;
						}
						// Add the fact type to the list of those seen on this path already.
						current[factType] = null;
						// Also add the fact type to the list of those seen already.
						visited[factType] = null;

						// Find the next hop on this path.
						ObjectType towardsObjectType = mapping.TowardsObjectType;
						mapping = FindDeepMappingAwayFromObjectType(towardsObjectType, predecidedOneToOneFactTypeMappings, mappings);

						if (mapping == null)
						{
							// The object type has no deep mappings away from it, so we continue with the outer loop.
							break;
						}

						// Let this loop continue and process the fact type for the next hop.
						factType = mapping.FactType;
					}
				}
				while (!permutationIsInvalid && (visited.Count < factTypeCount));
			}
		}
	}
	#endregion
}