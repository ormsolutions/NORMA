#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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
using System.Diagnostics;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.ORMAbstraction;

namespace ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge
{
	#region FactTypeMappingPermuter
	partial class AbstractionModelIsForORMModel
	{
		private sealed partial class FactTypeMappingPermuter
		{
			#region Chain calculator
			private sealed class FactTypeChainer
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

				public int Run(FactTypeMappingDictionary allDecidedMappings)
				{
					BuildChains();
					ReduceChains(allDecidedMappings);

					// Delete empty chains and find the largest chain.
					int largestChainCount = 0;
					for (int i = myChains.Count - 1; i >= 0; --i)
					{
						Chain chain = myChains[i];
						if (chain.UndecidedOneToOneFactTypeMappings.Count == 0)
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

					ChainList chains = myChains;
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
						chains.Add(chain);

						// Assuming ProcessObjectType works correctly, we will never hit an object type a second time,
						// so we can clear the Dictionary here and then use it as a record of all object types in the chain.
						visitedObjectTypes.Clear();

						ProcessObjectType(factType.RoleCollection[0].Role.RolePlayer, chain, visitedFactTypes, visitedObjectTypes);

						// Record all object types that are in the chain.
						chain.ObjectTypes.AddRange(visitedObjectTypes.Keys);
					}
				}
				/// <summary>
				/// Apply more stringent criteria to unusually long chains to ensure that
				/// the permutation process will not explode.
				/// </summary>
				private void ReduceChains(FactTypeMappingDictionary allDecidedMappings)
				{
					// Make sure all of the chains are of a size that we can actually permute
					FactTypeMappingDictionary predecidedMappings = myPredecidedOneToOneFactTypeMappings;
					FactTypeMappingListDictionary undecidedMappings = myUndecidedOneToOneFactTypeMappings;
					foreach (Chain chain in myChains)
					{
						chain.EnsureReasonablePermutations(undecidedMappings, predecidedMappings, allDecidedMappings);
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
				FactTypeMappingDictionary allDecidedMappings = myDecidedFactTypeMappings;
				// Break up the chains of contiguous one-to-one fact types
				FactTypeChainer chainer = new FactTypeChainer(myPredecidedManyToOneFactTypeMappings, myPredecidedOneToOneFactTypeMappings, myUndecidedOneToOneFactTypeMappings);

				// If some chains end up at a size we cannot process, then running
				// the chainer can move some mappings from undecided to predecided.
				// We pass in the decided mappings so that we do not lose track
				// of these entries.
				largestChainCount = chainer.Run(allDecidedMappings);

				// Perform one-time pass of top-level types for the decided mappings
				//PrecalculateDecidedConceptTypes();

				// This is used in PermuteFactTypeMappings(). We allocate it once, here, for permformance reasons.
				FactTypeMappingList newlyDecidedFactTypeMappings = new FactTypeMappingList(largestChainCount);

				// This is used to prevent deep mappings of an object type in two directions.
				Dictionary<ObjectType, object> deeplyMappedObjectTypes = new Dictionary<ObjectType, object>(largestChainCount);
				ChainPermutationState permutationStateHelper = new ChainPermutationState();
				// Used to eliminate deep mappings towards simple identifiers
				Dictionary<ObjectType, bool> nonPreferredFunctionalObjectTypesCache = new Dictionary<ObjectType, bool>();

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

					PermuteFactTypeMappings(chain.PossiblePermutations, chain.UndecidedOneToOneFactTypeMappings, newlyDecidedFactTypeMappings, deeplyMappedObjectTypes, 0);
					EliminateInvalidPermutations(chain, nonPreferredFunctionalObjectTypesCache);
					FindSmallestPermutationsInTermsOfConceptTypes(chain, permutationStateHelper);
#if FALSE
					// UNDONE: See notes on EliminatePermutationsWithIdenticalResults
					EliminatePermutationsWithIdenticalResults(chain);
#endif // FALSE

					// Add each mapping from the optimal permutation to the "global" set of decided mappings.
					foreach (FactTypeMapping optimalMapping in ChooseOptimalPermutation(chain).Mappings)
					{
						allDecidedMappings.Add(optimalMapping.FactType, optimalMapping);
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
			#region ObjectTypeStates enum
			/// <summary>
			/// States of an object type used to evaluate a permutation
			/// </summary>
			[Flags]
			private enum ObjectTypeStates : uint
			{
				/// <summary>
				/// No state
				/// </summary>
				None = 0,
				/// <summary>
				/// The object type is a subtype
				/// </summary>
				IsSubtype = 1,
				/// <summary>
				/// The object type should be treated independently
				/// </summary>
				IsIndependent = 2,
				/// <summary>
				/// Predetermined mappings contain one or more mappings toward
				/// this object that are not part of the preferred identifier
				/// </summary>
				HasPredecidedNonPreferredIdentifierMappingTowards = 4,
				/// <summary>
				/// These object types definitely DO result in concept types, but MAY or MAY NOT result in top-level concept types
				/// </summary>
				PredecidedMustHaveConceptType = IsSubtype | IsIndependent | HasPredecidedNonPreferredIdentifierMappingTowards,
				/// <summary>
				/// There is a predecided deep mapping away from this object
				/// </summary>
				HasPredecidedDeepMappingAway = 8,
				/// <summary>
				/// These object types definitely DO NOT result in top-level concept types, but MAY or MAY NOT result in concept types
				/// </summary>
				PredecidedMustNotHaveTopLevelConceptType = HasPredecidedDeepMappingAway,
				/// <summary>
				/// A mask for the object settings that do not change
				/// </summary>
				PredecidedState = IsSubtype | IsIndependent | HasPredecidedNonPreferredIdentifierMappingTowards | HasPredecidedDeepMappingAway,
				/// <summary>
				/// The current permutation contains one or more mappings
				/// toward this object type that are not part of the preferred identifier
				/// </summary>
				PermutationHasNonPreferredIdentifierMappingTowards = 0x10,
				/// <summary>
				/// In this permutation, these object types definitely DO result in concept types, but MAY or MAY NOT result in top-level concept types
				/// </summary>
				PermutationMustHaveConceptType = PermutationHasNonPreferredIdentifierMappingTowards,
				/// <summary>
				/// The current permutation has one or more deeep mappings
				/// away from this object type
				/// </summary>
				PermutationHasDeepMappingAway = 0x20,
				/// <summary>
				/// In this permatutation, these object types definitely DO NOT result in top-level concept types, but MAY or MAY NOT result in concept types
				/// </summary>
				PermutationMustNotHaveTopLevelConceptType = PermutationHasDeepMappingAway,
				/// <summary>
				/// The object type is top level in the current permutation
				/// </summary>
				TopLevelInPermutation = 0x40,
				/// <summary>
				/// The object type is not top level in the current permutation
				/// </summary>
				NonTopLevelInPermutation = 0x80,
				/// <summary>
				/// All of the permutation state fields
				/// </summary>
				PermutationState = PermutationHasNonPreferredIdentifierMappingTowards | PermutationHasDeepMappingAway | TopLevelInPermutation | NonTopLevelInPermutation,
			}
			#endregion // ObjectTypeStates enum
			#region ChainPermutationState class
			/// <summary>
			/// Helper structure for <see cref="FindSmallestPermutationsInTermsOfConceptTypes"/>
			/// Stores known state about the current object types and permutations in a single structure
			/// </summary>
			private class ChainPermutationState
			{
				#region Fields
				/// <summary>
				/// A mask to apply to a state that indicates the last permutation
				/// number the permutation data was used for. Applied after shifting.
				/// </summary>
				private const uint PermutationIdentifierAfterShiftMask = 0x000fffff;
				/// <summary>
				/// The number of bits to shift the state value to get the identifier bits
				/// </summary>
				private const int PermutationIdentifierShift = 12;
				/// <summary>
				/// A mask to apply to a state that indicates the last permutation
				/// number the permutation data was used for. Applied after shifting.
				/// </summary>
				private const uint PermutationIdentifierBeforeShiftMask = 0xfffff000;
				private Dictionary<ObjectType, ObjectTypeStates> myStates;
				private int myTopLevelCount;
				private int myNonTopLevelCount;
				/// <summary>
				/// A field to store in the current state to determine if the
				/// permutation fields apply to the current permutation
				/// </summary>
				private uint myPermutationIdentifier;
				#endregion // Fields
				#region Constructor
				public ChainPermutationState()
				{
					myStates = new Dictionary<ObjectType, ObjectTypeStates>();
				}
				#endregion // Constructor
				#region State management methods
				/// <summary>
				/// Set the specific state flag(s) for this object type
				/// </summary>
				private void SetState(ObjectType objectType, ObjectTypeStates state)
				{
					Dictionary<ObjectType, ObjectTypeStates> dictionary = myStates;
					ObjectTypeStates existingState;
					if (dictionary.TryGetValue(objectType, out existingState))
					{
						if ((existingState & state) == state)
						{
							return;
						}
						state |= existingState;
					}
					dictionary[objectType] = state;
				}
				/// <summary>
				/// Set the specific state flag(s) for this object type and permutation
				/// </summary>
				/// <returns><see langword="true"/> if the state changes</returns>
				private bool SetState(ObjectType objectType, ObjectTypeStates state, uint permutationIdentifier)
				{
					Dictionary<ObjectType, ObjectTypeStates> dictionary = myStates;
					ObjectTypeStates existingState;
					if (dictionary.TryGetValue(objectType, out existingState))
					{
						uint existingPermutationIdentifier = (uint)existingState >> PermutationIdentifierShift; // Note that shift is sufficient without mask, C# 0 fills uint right shift
						if (existingPermutationIdentifier != permutationIdentifier)
						{
							existingState = (ObjectTypeStates)(((uint)existingState & ~(PermutationIdentifierBeforeShiftMask | (uint)ObjectTypeStates.PermutationState)) | (permutationIdentifier << PermutationIdentifierShift) | (uint)(existingState & ObjectTypeStates.PredecidedState));
						}
						else if ((existingState & state) == state)
						{
							return false;
						}
						state |= existingState;
					}
					else
					{
						state |= (ObjectTypeStates)(permutationIdentifier << PermutationIdentifierShift);
					}
					dictionary[objectType] = state;
					return true;
				}
				/// <summary>
				/// The the state for the specified object type in the current permutation
				/// </summary>
				public ObjectTypeStates GetState(ObjectType objectType)
				{
					ObjectTypeStates state;
					if (myStates.TryGetValue(objectType, out state))
					{
						return CalculateState(state);
					}
					return ObjectTypeStates.None;
				}
				private ObjectTypeStates CalculateState(ObjectTypeStates storedState)
				{
					if (((uint)storedState >> PermutationIdentifierShift) == myPermutationIdentifier)
					{
						return storedState & (ObjectTypeStates.PredecidedState | ObjectTypeStates.PermutationState);
					}
					return storedState & ObjectTypeStates.PredecidedState;
				}
				#endregion // State management methods
				#region Chain initialization
				/// <summary>
				/// Load the object type states for the chain into the cached state
				/// </summary>
				/// <param name="chain">The <see cref="Chain"/> to load</param>
				public void BeginChainEvaluation(Chain chain)
				{
					myStates.Clear();

					// By just examining the predecided mappings, we can determine the specified truth values for the following:
					// Object Type is Concept Type              {true, unknown}
					// Object Type is Top Level Concept Type    {false, unknown}
					foreach (FactTypeMapping mapping in chain.PredecidedManyToOneFactTypeMappings)
					{
						Debug.Assert(mapping.MappingDepth == MappingDepth.Shallow);
						if (!mapping.IsFromPreferredIdentifier)
						{
							SetState(mapping.TowardsObjectType, ObjectTypeStates.HasPredecidedNonPreferredIdentifierMappingTowards);
						}
					}

					foreach (FactTypeMapping mapping in chain.PredecidedOneToOneFactTypeMappings)
					{
						if (!mapping.IsFromPreferredIdentifier)
						{
							SetState(mapping.TowardsObjectType, ObjectTypeStates.HasPredecidedNonPreferredIdentifierMappingTowards);
						}

						ObjectTypeStates fromState = 0;
						if (mapping.FromRole is SubtypeMetaRole)
						{
							fromState |= ObjectTypeStates.IsSubtype;
						}
						if (mapping.MappingDepth == MappingDepth.Deep)
						{
							fromState |= ObjectTypeStates.HasPredecidedDeepMappingAway;
						}
						if (fromState != 0)
						{
							SetState(mapping.FromObjectType, fromState);
						}
					}

					foreach (ObjectType objectType in chain.ObjectTypes)
					{
						if (objectType.TreatAsIndependent)
						{
							SetState(objectType, ObjectTypeStates.IsIndependent);
						}
					}
				}
				#endregion // Chain initialization
				#region Permutation evaluation
				/// <summary>
				/// Begin evaluation of a single permutation
				/// </summary>
				/// <param name="permutation"></param>
				public void BeginPermutation(Permutation permutation)
				{
					// Clear counts
					myTopLevelCount = 0;
					myNonTopLevelCount = 0;

					// Get the new permutation number
					uint permutationIdentifier = myPermutationIdentifier;
					if (permutationIdentifier == PermutationIdentifierAfterShiftMask)
					{
						permutationIdentifier = 1; // Initial predecided only state is 0, avoid using 0
					}
					else
					{
						++permutationIdentifier;
					}
					myPermutationIdentifier = permutationIdentifier;

					foreach (FactTypeMapping mapping in permutation.Mappings)
					{
						if (!mapping.IsFromPreferredIdentifier)
						{
							SetState(mapping.TowardsObjectType, ObjectTypeStates.PermutationHasNonPreferredIdentifierMappingTowards, permutationIdentifier);
						}
						if (mapping.MappingDepth == MappingDepth.Deep)
						{
							SetState(mapping.FromObjectType, ObjectTypeStates.PermutationHasDeepMappingAway, permutationIdentifier);
						}
					}
				}
				/// <summary>
				/// Mark the specified <see cref="ObjectType"/> as top level in the current permutation
				/// </summary>
				/// <returns><see langword="true"/> if the state changes</returns>
				public bool SetTopLevel(ObjectType objectType)
				{
					if (SetState(objectType, ObjectTypeStates.TopLevelInPermutation, myPermutationIdentifier))
					{
						++myTopLevelCount;
						return true;
					}
					return false;
				}
				/// <summary>
				/// Get the total current number of top level object types
				/// </summary>
				public int TopLevelCount
				{
					get
					{
						return myTopLevelCount;
					}
				}
				/// <summary>
				/// Mark the specified <see cref="ObjectType"/> as not top level in the current permutation
				/// </summary>
				/// <returns><see langword="true"/> if the state changes</returns>
				public bool SetNonTopLevel(ObjectType objectType)
				{
					if (SetState(objectType, ObjectTypeStates.NonTopLevelInPermutation, myPermutationIdentifier))
					{
						++myNonTopLevelCount;
						return true;
					}
					return false;
				}
				/// <summary>
				/// Get the total current number of non top level object types
				/// </summary>
				public int NonTopLevelCount
				{
					get
					{
						return myNonTopLevelCount;
					}
				}
				#endregion // Permutation evaluation
				#region Report resulting state
				/// <summary>
				/// Enumerate instances that pass the requested file
				/// </summary>
				/// <param name="filter">One or more state flags. Any set flag satisfies the filter</param>
				public IEnumerable<KeyValuePair<ObjectType, ObjectTypeStates>> EnumerateStates(ObjectTypeStates filter)
				{
					foreach (KeyValuePair<ObjectType, ObjectTypeStates> pair in myStates)
					{
						if (0 != (CalculateState(pair.Value) & filter))
						{
							yield return pair;
						}
					}
				}
				#endregion // Report resulting state
			}
			#endregion // ChainPermutationState class
			private static void FindSmallestPermutationsInTermsOfConceptTypes(Chain chain, ChainPermutationState permutationState)
		{
			permutationState.BeginChainEvaluation(chain);

			IList<Permutation> smallestPermutationsInTermsOfConceptTypes = chain.SmallestPermutationsInTermsOfConceptTypes;

			int minTopLevelConceptTypesCount = int.MaxValue;
			int minNonTopLevelConceptTypesCount = int.MaxValue;

			foreach (Permutation permutation in chain.PossiblePermutations)
			{
				bool isNotOptimalPermutation = false;
				permutationState.BeginPermutation(permutation);

				foreach (ObjectType objectType in chain.ObjectTypes)
				{
					ObjectTypeStates startingState = permutationState.GetState(objectType);
					if (0 != (startingState & (ObjectTypeStates.PredecidedMustHaveConceptType | ObjectTypeStates.PermutationMustHaveConceptType)))
					{
						if (0 != (startingState & (ObjectTypeStates.PredecidedMustNotHaveTopLevelConceptType | ObjectTypeStates.PermutationMustNotHaveTopLevelConceptType)))
						{
							if (permutationState.SetNonTopLevel(objectType) &&
								(permutationState.TopLevelCount == minTopLevelConceptTypesCount) &&
								(permutationState.NonTopLevelCount > minNonTopLevelConceptTypesCount))
							{
								// We now have more non-top-level concept types than the minimum, and the same number of top-level concept types as the minimum, so throw this permutation out.
								isNotOptimalPermutation = true;
								break;
							}
						}
						else
						{
							if (permutationState.SetTopLevel(objectType) &&
								(permutationState.TopLevelCount > minTopLevelConceptTypesCount))
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

				int topLevelCount = permutationState.TopLevelCount;
				int nonTopLevelCount = permutationState.NonTopLevelCount;
				Debug.Assert(topLevelCount <= minTopLevelConceptTypesCount, "Permutations with greater than the minimum number of top-level concept types should have been rejected inline.");

				if (topLevelCount < minTopLevelConceptTypesCount)
				{
					// We have a new minimum number of top-level concept types (and hence a new minimum number of non-top-level concept types as well).
					minTopLevelConceptTypesCount = topLevelCount;
					minNonTopLevelConceptTypesCount = nonTopLevelCount;
					smallestPermutationsInTermsOfConceptTypes.Clear();
				}
				else
				{
					// We have the same number of top-level concept types as the minimum, so we need to check the number of non-top-level concept types.
					if (nonTopLevelCount > minNonTopLevelConceptTypesCount)
					{
						// This isn't an optimal permutation, so go on to the next one.
						continue;
					}
					else if (nonTopLevelCount < minNonTopLevelConceptTypesCount)
					{
						// We have a new minimum number of non-top-level concept type.
						minNonTopLevelConceptTypesCount = nonTopLevelCount;
						smallestPermutationsInTermsOfConceptTypes.Clear();
					}
				}
				permutation.SetConceptTypes(permutationState);
				smallestPermutationsInTermsOfConceptTypes.Add(permutation);
			}
		}

#if FALSE // The current implementation does not care about the reduced set, and this algorithm does not just eliminate equivalent elements
			/// <summary>
			/// Eliminates <see cref="Permutation"/>s that have the same set of top-level concept types and non-top-level concept types,
			/// which will always result in the same OIAL.
			/// </summary>
			private static void EliminatePermutationsWithIdenticalResults(Chain chain)
			{
				IList<Permutation> smallestPermutationsInTermsOfConceptTypes = chain.SmallestPermutationsInTermsOfConceptTypes;
				for (int currentPermutationIndex = 0; currentPermutationIndex < smallestPermutationsInTermsOfConceptTypes.Count - 1; currentPermutationIndex++)
				{
					Permutation currentPermutation = smallestPermutationsInTermsOfConceptTypes[currentPermutationIndex];
					for (int comparisonPermutationIndex = smallestPermutationsInTermsOfConceptTypes.Count - 1; comparisonPermutationIndex > currentPermutationIndex; comparisonPermutationIndex--)
					{
						Permutation comparisonPermutation = smallestPermutationsInTermsOfConceptTypes[comparisonPermutationIndex];
						if (currentPermutation.IsIdentical(comparisonPermutation))
						{
							// comparisonPermutation has the same set of top-level and non-top-level concept types, so we can get rid of it.
							// It is entirely arbitrary which one we get rid of, so we'll keep the first since it is easier.
							smallestPermutationsInTermsOfConceptTypes.RemoveAt(comparisonPermutationIndex);
						}
					}
				}
			}
#endif // FALSE

			private static Permutation ChooseOptimalPermutation(Chain chain)
			{
				IList<Permutation> permutations = chain.SmallestPermutationsInTermsOfConceptTypes;
				int permutationCount = permutations.Count;
				Permutation firstFinalMappingState;
				if (permutationCount > 1)
				{
					// Prioritize permutations based on mandatory, entity/value, and mapping depth patterns.
					long[] weights = new long[permutationCount];
					int[] order = new int[permutationCount];
					for (int i = 0; i < permutationCount; ++i)
					{
						weights[i] = GetPermutationPriority(permutations[i]);
						order[i] = i;
					}
					Array.Sort<int>(
						order,
						delegate(int left, int right)
						{
							return weights[right].CompareTo(weights[left]);
						});
					firstFinalMappingState = permutations[order[0]];
				}
				else
				{
					firstFinalMappingState = permutations[0];
				}
				//smallestPermutationsInTermsOfConceptTypes.Clear();
				return firstFinalMappingState;
			}
			private static long GetPermutationPriority(Permutation permutation)
			{
				// Prioritize the remaining elements in terms of mandatories, then entity/value balance, then deep/shallow.
				// 1) An unbalanced mandatory constraint is given highest priority, outweighing all other concerns.
				//    For a shallow, prefer a mapping towards the mandatory role. For a deep mapping, prefer a mapping
				//    away from the mandatory role.
				// 2) A double mandatory is given next priority, outweighing all entity/value issues
				// 3) A mapping between an entity and value gets the next highest priority
				// 4) A mapping between two entities is next (value-to-value gets no points)
				// 5) A deep mapping has a higher priority than an shallow mapping, but does not override
				//    any other concerns.
				FactTypeMappingList mappings = permutation.Mappings;
				int mappingCount = mappings.Count;
				const long deepMappingValue = 1;
				long balancedEntityValue = mappingCount + 1;
				long unbalancedEntityValue = balancedEntityValue * mappingCount + 1;
				long balancedMandatoryValue = unbalancedEntityValue * mappingCount + 1;
				long unbalancedMandatoryValue = unbalancedEntityValue * mappingCount + 1;
				long retVal = 0;
				for (int i = 0; i < mappingCount; ++i)
				{
					FactTypeMapping mapping = mappings[i];
					// Completely ignore implied mandatory. If both are implied mandatory, then this ranks the same as non-mandatory
					bool fromMandatory = mapping.FromRoleExplicitlyMandatory;
					bool toMandatory = mapping.TowardsRoleExplicitlyMandatory;
					bool deepMapping = mapping.MappingDepth == MappingDepth.Deep;
					if (fromMandatory)
					{
						if (toMandatory)
						{
							retVal += balancedMandatoryValue;
						}
						else if (deepMapping)
						{
							retVal += unbalancedMandatoryValue;
						}
					}
					else if (toMandatory && !deepMapping)
					{
						retVal += unbalancedMandatoryValue;
					}
					if (mapping.FromValueType)
					{
						if (!mapping.TowardsValueType)
						{
							retVal += unbalancedEntityValue;
						}
					}
					else if (!mapping.TowardsValueType)
					{
						retVal += balancedEntityValue; // Entity to entity gets higher priority than value/value
					}
					if (deepMapping)
					{
						if (mapping.IsFromPreferredIdentifier)
						{
							retVal -= deepMappingValue; // Discourage deep mapping away from the preferred identifier
						}
						else
						{
							retVal += deepMappingValue;
						}
					}
				}
				return retVal;
			}

			/// <summary>
			/// Returns the maximum number of <see cref="Permutation"/>s that could result for the specified set of undecided <see cref="FactTypeMapping"/>s.
			/// </summary>
			private static double CalculateMaxNumberOfPermutations(FactTypeMappingListList undecidedMappings)
			{
				double maxPermutations = 1;
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
			private static void PermuteFactTypeMappings(IList<Permutation> permutations, FactTypeMappingListList undecidedMappings, FactTypeMappingList decidedMappings, Dictionary<ObjectType, object> deeplyMappedObjectTypes, int currentPosition)
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
			/// <param name="nonPreferredFuntionalObjectTypes">A dictionary to cache state information
			/// for potentially deeply mapped <see cref="ObjectType"/>s</param>
			private static void EliminateInvalidPermutations(Chain chain, Dictionary<ObjectType, bool> nonPreferredFuntionalObjectTypes)
			{
				Debug.Assert(chain.UndecidedOneToOneFactTypeMappings.Count > 0);

				int factTypeCount = chain.OneToOneFactTypeCount;

				FactTypeMappingList predecidedOneToOneFactTypeMappings = chain.PredecidedOneToOneFactTypeMappings;
				IList<Permutation> possiblePermutations = chain.PossiblePermutations;

				Dictionary<FactType, object> visited = new Dictionary<FactType, object>(factTypeCount);
				Dictionary<FactType, object> current = new Dictionary<FactType, object>(factTypeCount);

				Debug.Assert(possiblePermutations[0].Mappings.Count > 0);
				// All of the permutations will always contain the same number of mappings, so we can calculate it here.
				int mappingsCount = possiblePermutations[0].Mappings.Count;

				int permutationIndex;
				for (permutationIndex = possiblePermutations.Count - 1; permutationIndex >= 0; permutationIndex--)
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

					if (!permutationIsInvalid)
					{
						// We now filter out a common pattern where an ObjectType plays
						// no functional roles to justify a ConceptType and is not chained with
						// another deep mapping that satisfies this criteria.
						// If the permuation is valid at this point, then there are no cycles
						// in the deep maps for the permuation, so we can safely run this routine
						// recursively.
						for (int i = 0; i < mappingsCount; ++i)
						{
							if (IsBlockedDeepMapping(mappings, i, nonPreferredFuntionalObjectTypes))
							{
								possiblePermutations.RemoveAt(permutationIndex);
								break;
							}
						}
					}
				}

				// If there are two or more remaining permutations with a deep mapping away
				// from the same object type via two different non-subtype fact types, or if
				// there is a subtype deep mapping away and other fact type deep mappings away,
				// then eliminate the fact type-based deep mappings. The choice of which of these
				// deep mappings ends up as an assimilation is arbitrary, so we choose to allow none
				// of them.
				permutationIndex = possiblePermutations.Count - 1;
				BitTracker usesDeepFromSameObjectType = new BitTracker();
				while (permutationIndex > 0) // There is no previous to check for the last item, so stop at 1
				{
					FactTypeMappingList mappings = possiblePermutations[permutationIndex].Mappings;
					for (int i = 0; i < mappingsCount; ++i)
					{
						FactTypeMapping mapping = mappings[i];
						if (mapping.MappingDepth == MappingDepth.Deep)
						{
							ObjectType fromObjectType = mapping.FromObjectType;
							FactType viaFactType = mapping.FactType;
							bool viaSubtype = mapping.IsSubtype;
							usesDeepFromSameObjectType.Reset(permutationIndex + 1);
							bool removeTrackedPermutation = false;
							if (!viaSubtype)
							{
								usesDeepFromSameObjectType[permutationIndex] = true;
							}
							for (int j = permutationIndex - 1; j >= 0; --j)
							{
								FactTypeMappingList testMappings = possiblePermutations[j].Mappings;
								for (int k = 0; k < mappingsCount; ++k)
								{
									FactTypeMapping testMapping = testMappings[k];
									if (testMapping.MappingDepth == MappingDepth.Deep &&
										testMapping.FromObjectType == fromObjectType)
									{
										FactType testViaFactType = testMapping.FactType;
										if (viaSubtype)
										{
											if (!(testMapping.IsSubtype))
											{
												usesDeepFromSameObjectType[j] = true;
												removeTrackedPermutation = true;
											}
										}
										else if (testMapping.IsSubtype)
										{
											removeTrackedPermutation = true;
										}
										else
										{
											usesDeepFromSameObjectType[j] = true;
											if (testViaFactType != viaFactType)
											{
												removeTrackedPermutation = true;
											}
										}
									}
								}
							}
							if (removeTrackedPermutation)
							{
								for (int j = permutationIndex; j >= 0; --j)
								{
									if (usesDeepFromSameObjectType[j])
									{
										possiblePermutations.RemoveAt(j);
										if (j != permutationIndex)
										{
											--permutationIndex;
										}
									}
								}
								break;
							}
						}
					}
					--permutationIndex;
				}
			}
			private static bool IsBlockedDeepMapping(FactTypeMappingList mappings, int currentMappingIndex, Dictionary<ObjectType, bool> nonPreferredFuntionalObjectTypes)
			{
				FactTypeMapping mapping = mappings[currentMappingIndex];
				ObjectType towards;
				if (mapping.MappingDepth == MappingDepth.Deep && !ObjectTypePlaysNonIdentifierFunctionalRoles((towards = mapping.TowardsObjectType), nonPreferredFuntionalObjectTypes))
				{
					int mappingsCount = mappings.Count;
					int j = 1;
					for (; j < mappingsCount; ++j)
					{
						int testIndex = (j + currentMappingIndex) % mappingsCount;
						FactTypeMapping testMapping = mappings[testIndex];
						if (testMapping.MappingDepth == MappingDepth.Deep && testMapping.FromObjectType == towards && !IsBlockedDeepMapping(mappings, testIndex, nonPreferredFuntionalObjectTypes))
						{
							break;
						}
					}
					if (j == mappingsCount)
					{
						return true;
					}
				}
				return false;
			}
			/// <summary>
			/// Test if an <see cref="ObjectType"/> plays non-identifier functional roles,
			/// meaning a functional role that is not opposite part of the preferred identifier for
			/// this ObjectType and is not part of the preferred identifier for any other object.
			/// Also returns true if <see cref="ObjectType.TreatAsIndependent"/> is set.
			/// </summary>
			private static bool ObjectTypePlaysNonIdentifierFunctionalRoles(ObjectType objectType, Dictionary<ObjectType, bool> alreadyTested)
			{
				bool retVal;
				if (alreadyTested != null && alreadyTested.TryGetValue(objectType, out retVal))
				{
					return retVal;
				}
				else if (objectType.TreatAsIndependent)
				{
					if (alreadyTested != null)
					{
						alreadyTested[objectType] = true;
					}
					return true;
				}
				foreach (Role role in objectType.PlayedRoleCollection)
				{
					UniquenessConstraint functionalUniqueness = role.SingleRoleAlethicUniquenessConstraint;
					if (functionalUniqueness == null || functionalUniqueness.IsPreferred)
					{
						continue;
					}
					Role oppositeRole = role.OppositeRoleAlwaysResolveProxy as Role;
					retVal = true;
					if (oppositeRole != null)
					{
						foreach (ConstraintRoleSequence sequence in oppositeRole.ConstraintRoleSequenceCollection)
						{
							UniquenessConstraint oppositeUniqueness = sequence as UniquenessConstraint;
							if (oppositeUniqueness != null &&
								oppositeUniqueness.PreferredIdentifierFor == objectType)
							{
								retVal = false;
								break;
							}
						}
					}
					if (retVal)
					{
						if (alreadyTested != null)
						{
							alreadyTested[objectType] = true;
						}
						return true;
					}
				}
				if (alreadyTested != null)
				{
					alreadyTested[objectType] = false;
				}
				return false;
			}
		}
	}
	#endregion
}