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

				ProcessObjectType(factType.RoleCollection[0].Role.RolePlayer, chain, visitedFactTypes, visitedObjectTypes);

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

				FactTypeMapping mapping;
				FactTypeMappingList mappingList;
				if (myUndecidedOneToOneFactTypeMappings.TryGetValue(factType, out mappingList))
				{
					chain.UndecidedOneToOneFactTypeMappings.Add(mappingList);
				}
				else if (myPredecidedOneToOneFactTypeMappings.TryGetValue(factType, out mapping))
				{
					chain.PredecidedOneToOneFactTypeMappings.Add(mapping);
				}
				else if (myPredecidedManyToOneFactTypeMappings.TryGetValue(factType, out mapping))
				{
					chain.PredecidedManyToOneFactTypeMappings.Add(mapping);
				}
				else
				{
					continue;
				}

				// Record that this fact type has been visited.
				visitedFactTypes[factType] = null;

				LinkedElementCollection<RoleBase> roles = factType.RoleCollection;
				Debug.Assert(roles.Count == 2);
				ObjectType objectType1 = roles[0].Role.RolePlayer;
				ObjectType objectType2 = roles[1].Role.RolePlayer;
				ProcessObjectType(objectType1, chain, visitedFactTypes, visitedObjectTypes);
				ProcessObjectType(objectType2, chain, visitedFactTypes, visitedObjectTypes);
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

		// Stores a list of object types that had been considered valid top-level, but was later found to be deeply mapped away
		private readonly ObjectTypeDictionary myInvalidObjectTypes;

		// A single set of possible fact type mappings represented at a given iteration through all possible fact type mappings
		private readonly ObjectTypeDictionary myPossibleTopLevelConceptTypes;
		private readonly ObjectTypeDictionary myPossibleConceptTypes;


		public FactTypeMappingPermuter(FactTypeMappingDictionary predecidedManyToOneFactTypeMappings, FactTypeMappingDictionary predecidedOneToOneFactTypeMappings, FactTypeMappingListDictionary undecidedOneToOneFactTypeMappings)
		{
			myPredecidedManyToOneFactTypeMappings = predecidedManyToOneFactTypeMappings;
			myPredecidedOneToOneFactTypeMappings = predecidedOneToOneFactTypeMappings;
			myUndecidedOneToOneFactTypeMappings = undecidedOneToOneFactTypeMappings;

			int oneToOneFactTypeCount = predecidedOneToOneFactTypeMappings.Count + undecidedOneToOneFactTypeMappings.Count;
			myDecidedFactTypeMappings = new FactTypeMappingDictionary(predecidedManyToOneFactTypeMappings.Count + oneToOneFactTypeCount);

			foreach (KeyValuePair<FactType, FactTypeMapping> pair in predecidedManyToOneFactTypeMappings)
			{
				myDecidedFactTypeMappings.Add(pair.Key, pair.Value);
			}
			foreach (KeyValuePair<FactType, FactTypeMapping> pair in predecidedOneToOneFactTypeMappings)
			{
				myDecidedFactTypeMappings.Add(pair.Key, pair.Value);
			}

			// Stores a list of object types that had been considered valid top-level, but was later found to be deeply mapped away
			myInvalidObjectTypes = new ObjectTypeDictionary(oneToOneFactTypeCount);
			myPossibleTopLevelConceptTypes = new ObjectTypeDictionary(oneToOneFactTypeCount);
			myPossibleConceptTypes = new ObjectTypeDictionary(oneToOneFactTypeCount);
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
			int largestChainCount;
			// Break up the chains of contiguous one-to-one fact types
			FactTypeChainer chainer = new FactTypeChainer(myPredecidedManyToOneFactTypeMappings, myPredecidedOneToOneFactTypeMappings, myUndecidedOneToOneFactTypeMappings);
			largestChainCount = chainer.Run();

			// Perform one-time pass of top-level types for the decided mappings
			PrecalculateDecidedConceptTypes();

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
				CalculateTopLevelConceptTypes(chain);

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

		private static Permutation ChooseOptimalPermutation(Chain chain)
		{
			// UNDONE: This should do something smart!
			PermutationList smallestPermutationsList = chain.SmallestPermutations;
			Permutation firstFinalMappingState = smallestPermutationsList[0];
			smallestPermutationsList.Clear();
			return firstFinalMappingState;
		}

		private void PrecalculateDecidedConceptTypes()
		{
			// Calculate decided top-level types (these are *not* final until individual permutations have been considered)
			foreach (KeyValuePair<FactType, FactTypeMapping> pair in myDecidedFactTypeMappings)
			{
				ProcessEntity(new ProcessEntityState(pair.Value, null, null, null));
			}
		}

		/// <summary>
		/// An object type (A) is a concept type when:
		///		1. It is independent, or
		///		2. It is a subtype, or
		///		3. Another object type (B) is mapped to it, and
		///			a. There exists a role being mapped to object type A on which there are no constraints, or
		///			b. There exists a role being mapped to object type A on which there is not a preferred uniqueness.
		/// 
		/// A concept type is a top-level concept type when:
		///		1. It is not deeply mapped towards any other concept type.
		/// 
		/// As this method processes the ObjectType passed in it gradually (and probably not in the same pass) executes each of
		/// the above checks to see whether the ObjectType is a concept type or top-level concept type.
		/// </summary>
		/// <param name="state"></param>
		private void ProcessEntity(ProcessEntityState state)
		{
			ObjectType towards = state.Mapping.TowardsObjectType;
			if (myInvalidObjectTypes.ContainsKey(towards))
			{
				return;
			}
			ObjectType from = state.Mapping.FromObjectType;
			Role fromRole = state.Mapping.FromRole;
			if (from.IsIndependent && !(fromRole is SubtypeMetaRole) && !myPossibleConceptTypes.ContainsKey(from))
			{
				// Add the object type as a concept type if it's independent.
				myPossibleConceptTypes.Add(from, true);
				if (state.ConceptTypeGarbage != null)
				{
					state.ConceptTypeGarbage.Add(from);
				}
			}
			// If the |from| OT is a primary identifier, DO NOT add the |towards| OT to the top-level list
			// (Note that it doesn't mean it cannot be added earlier/later, but will not if only role played is primary identifier)
			if (state.Mapping.IsFromPreferredIdentifier)
			{
				return;
			}
			if (!myPossibleConceptTypes.ContainsKey(from))
			{
				// All top-level concept types are at least concept types
				myPossibleConceptTypes.Add(from, true);
				if (state.ConceptTypeGarbage != null && !state.ConceptTypeGarbage.Contains(from))
				{
					state.ConceptTypeGarbage.Add(from);
				}
			}
			MappingDepth mappingType = state.Mapping.MappingDepth;
			// If the |from| OT is mapped away deeply, and has objects mapped to it, remove it and invalidate it as a possible top-level type
			if (myPossibleTopLevelConceptTypes.ContainsKey(from) && mappingType == MappingDepth.Deep)
			{
				myPossibleTopLevelConceptTypes.Remove(from);
				myInvalidObjectTypes.Add(from, true);
				if (state.Restore != null)
				{
					state.Restore.Add(from);
				}
			}
			// First clause in there because |from| could equal |towards|, and may be invalidated in prior conditional
			if (!myInvalidObjectTypes.ContainsKey(towards) && !myPossibleTopLevelConceptTypes.ContainsKey(towards))
			{
				if (state.Garbage != null)
				{
					state.Garbage.Add(towards);
				}
				myPossibleTopLevelConceptTypes.Add(towards, true);
			}
		}

		/// <summary>
		/// This method processes concept type and top-level concept type conditions gradually as it iterates over the chain, rather than 
		/// calculating top-level-ness immediately for every concept type.  The final state of each concept type is valid after this 
		/// method is finished executing.
		/// 
		/// See <see cref="ProcessEntity">ProcessEntity</see> for the algorithm.
		/// </summary>
		/// <param name="chain"></param>
		private void CalculateTopLevelConceptTypes(Chain chain)
		{
			// The smallest overall mapping that we've found
			int smallest = int.MaxValue;
			// These dictionaries track the OTs that are temporarily removed or added for each permutation in the list
			ObjectTypeList garbage = new ObjectTypeList(myDecidedFactTypeMappings.Count);
			ObjectTypeList restore = new ObjectTypeList(myDecidedFactTypeMappings.Count);
			ObjectTypeList conceptTypeGarbage = new ObjectTypeList(myDecidedFactTypeMappings.Count);
			// Now include the permutations in the calculation of top-level types.
			for (int i = 0; i < chain.PossiblePermutations.Count; i++)
			{
				garbage.Clear();
				restore.Clear();
				Permutation state = chain.PossiblePermutations[i];
				foreach (FactTypeMapping mapping in state.Mappings)
				{
					ProcessEntityState entitystate = new ProcessEntityState(mapping, restore, garbage, conceptTypeGarbage);
					ProcessEntity(entitystate);
					if (myPossibleTopLevelConceptTypes.Count > smallest)
					{
						break;
					}
				}
				// Done for this state, so finalize and clean up
				state.TopLevelConceptTypes = myPossibleTopLevelConceptTypes.Count;
				state.ConceptTypes = myPossibleConceptTypes.Count;
				if (state.TopLevelConceptTypes <= smallest)
				{
					smallest = state.TopLevelConceptTypes;
					PermutationList smallestList = chain.SmallestPermutations;
					if (smallestList.Count > 0)
					{
						if (smallestList[0].TopLevelConceptTypes > state.TopLevelConceptTypes)
						{
							smallestList.Clear();
						}
						// Be sure the logic works if you put this |else if| into the primary |if|.
						else if (smallestList[0].ConceptTypes > state.ConceptTypes)
						{
							smallestList.Clear();
						}
					}
					if (smallestList.Count == 0 || smallestList[0].ConceptTypes == state.ConceptTypes)
					{
						smallestList.Add(state);
					}
				}
				// Restore OTs removed from the |myPossibleTopLevelConceptTypes| collection; remove elements that were added to the collection
				foreach (ObjectType ot in garbage)
				{
					myPossibleTopLevelConceptTypes.Remove(ot);
				}
				foreach (ObjectType ot in restore)
				{
					myPossibleTopLevelConceptTypes.Add(ot, true);
					myInvalidObjectTypes.Remove(ot);
				}
				foreach (ObjectType ot in conceptTypeGarbage)
				{
					myPossibleConceptTypes.Remove(ot);
				}
			}
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
