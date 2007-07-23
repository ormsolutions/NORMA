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
	class UndecidedMappingChains
	{
		FactTypeMappingListDictionary myUndecided = null;
		FactTypeMappingDictionary myDecidedOneToOnes = null;
		ChainList myChains = new ChainList();
		ObjectTypeListDictionary myFromTo = new ObjectTypeListDictionary();

		public UndecidedMappingChains(FactTypeMappingListDictionary undecided, FactTypeMappingDictionary myDecidedOneToOneFactTypeMappings)
		{
			myUndecided = undecided;
			myDecidedOneToOnes = myDecidedOneToOneFactTypeMappings;
		}
#if PERMUTATION_DEBUG_OUTPUT
		public void Run(StreamWriter sw, ref int largestChain)
		{
			DateTime starttime = DateTime.Now;

			AddDecidedOneToOnes();
			BuildData();
			SplitChains();
			RemoveDecidedOneToOnes();
			DeleteEmptyChains();

			DateTime endtime = DateTime.Now;

			// If you remove logging, need to retain at least the largestChain code & loop
			sw.WriteLine("Chain Timer: " + endtime.Subtract(starttime).ToString());
			sw.WriteLine("Chains:");
			int i = 0;
			foreach (FactTypeMappingListDictionary chain in myChains)
			{
				if (chain.Count > largestChain)
					largestChain = chain.Count;

				/*
				sw.WriteLine("\t" + i + ":");
				foreach (FactType factType in chain.Keys)
				{

					sw.WriteLine("\t\t" + factType.Name);
					FactTypeMappingList list = chain[factType];
					foreach (FactTypeMapping mapping in list)
					{
						sw.WriteLine("\t\t\t" + mapping.FromObjectType.ToString() + " to " + mapping.TowardsObjectType.ToString() + " (" + mapping.MappingType.ToString() + ")");
					}
				}
				 */
			}
			sw.WriteLine("");
			sw.Flush();
		}
#else
		public void Run(ref int largestChain)
		{
			AddDecidedOneToOnes();
			BuildData();
			SplitChains();
			RemoveDecidedOneToOnes();
			DeleteEmptyChains();
		}
#endif
		public ChainList Chains
		{
			get { return myChains; }
		}

		private void DeleteEmptyChains()
		{
			for (int i = 0; i < myChains.Count; )
			{
				if (myChains[i].MappingList.Count == 0)
				{
					myChains.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}

		private void AddDecidedOneToOnes()
		{
			foreach (KeyValuePair<FactType, FactTypeMapping> pair in myDecidedOneToOnes)
			{
				FactTypeMappingList list = new FactTypeMappingList(1);
				list.Add(pair.Value);
				myUndecided.Add(pair.Key, list);
			}
		}

		private void BuildData()
		{
			// Build all of the from->to mappings
			foreach (FactType factType in myUndecided.Keys)
			{
				FactTypeMappingList maplist = myUndecided[factType];
				foreach (FactTypeMapping mapping in maplist)
				{
					// For each mapping, add the "towards" to the "from" list
					ObjectType fromObject = mapping.FromObjectType;
					ObjectType toObject = mapping.TowardsObjectType;
					ObjectTypeList list = null;

					// from->to
					if (!myFromTo.TryGetValue(fromObject, out list))
					{
						list = new ObjectTypeList();
						myFromTo.Add(fromObject, list);
					}
					if (!list.Contains(toObject))
					{
						list.Add(toObject);
					}
				}
			}
		}

		private void SplitChains()
		{
			// Set up chains of from->to, then analyze all chains where the "to" for one is the "from" for another.
			// When there is no "from" corresponding to a "to" for a given mapping, the chain stops.

			// Create the list that ensures we don't double-back on ourselves
			Dictionary<ObjectType, bool> visited = new Dictionary<ObjectType, bool>();

			// Loop through and get the chains!
			foreach (ObjectType objectType in myFromTo.Keys)
			{
				if (visited.ContainsKey(objectType))
				{
					continue;
				}

				Chain chain = new Chain();
				FindChain(objectType, chain, visited);
				myChains.Add(chain);
			}
		}

		private void RemoveDecidedOneToOnes()
		{
			foreach (KeyValuePair<FactType, FactTypeMapping> pair in myDecidedOneToOnes)
			{
				myUndecided.Remove(pair.Key);
				foreach (Chain chain in myChains)
				{
					chain.Remove(pair.Key);
				}
			}
		}

		private void FindChain(ObjectType from, Chain chain, Dictionary<ObjectType, bool> visited)
		{
			ObjectTypeList toList;
			if (!myFromTo.TryGetValue(from, out toList))
			{
				return;
			}
			if (toList == null || toList.Count == 0)
			{
				return;
			}

			if (!visited.ContainsKey(from))
			{
				visited.Add(from, true);
			}

			foreach (ObjectType towards in toList)
			{
				if (visited.ContainsKey(towards))
				{
					continue;
				}

				// look for an undecided mapping matching the |from| and |toward| values (and vice versa)
				foreach (KeyValuePair<FactType, FactTypeMappingList> pair in myUndecided)
				{
					FactType mappingFactType = pair.Key;
					FactTypeMappingList mappings = pair.Value;
					if (mappings.Count > 0 && ((mappings[0].FromObjectType.Equals(from) && mappings[0].TowardsObjectType.Equals(towards)) ||
						(mappings[0].FromObjectType.Equals(towards) && mappings[0].TowardsObjectType.Equals(from))))
					{
						chain.Add(mappingFactType, mappings);
						FindChain(towards, chain, visited);
					}
				}
			}
		}
	}
	#endregion
	#region LiveOialPermuter
	class LiveOialPermuter
	{
		FactTypeMappingDictionary myDecidedFactTypeMappings = null;
		FactTypeMappingDictionary myDecidedOneToOneFactTypeMappings = null;
		FactTypeMappingListDictionary myUndecidedFactTypeMappings = null;
		// Stores a list of object types that had been considered valid top-level, but was later found to be deeply mapped away
		ObjectTypeDictionary myInvalidObjectTypes = null;
		FinalMappingStateList myPossibleFactTypeMappings = new FinalMappingStateList();
		// List of all smallest mappings of the diagram (contains only permutation elements)
		FinalMappingStateList mySmallestPermutationsList = new FinalMappingStateList();
		// A single set of possible fact type mappings represented at a given iteration through all possible fact type mappings
		ObjectTypeDictionary myPossibleTopLevelConceptTypes = null;
		ObjectTypeDictionary myPossibleConceptTypes = null;
#if PERMUTATION_DEBUG_OUTPUT
		StreamWriter sw;
#endif

		public FinalMappingStateList SmallestPermutationsList
		{
			get { return mySmallestPermutationsList; }
		}

		public LiveOialPermuter(FactTypeMappingDictionary decidedFactTypeMappings, FactTypeMappingListDictionary undecidedFactTypeMappings)
		{
			myDecidedFactTypeMappings = decidedFactTypeMappings;
			myUndecidedFactTypeMappings = undecidedFactTypeMappings;
		}

		public void Run()
		{
#if PERMUTATION_DEBUG_OUTPUT
			sw = new StreamWriter("output.txt");
			DateTime start;
			DateTime end;

			start = DateTime.Now;
#endif
			FindDecidedOneToOneMappings();
#if PERMUTATION_DEBUG_OUTPUT
			end = DateTime.Now;
			sw.WriteLine("FindDecidedOneToOneMappings: " +end.Subtract(start).ToString());

			start = DateTime.Now;
#endif
			PermuteFactTypeMappings();
#if PERMUTATION_DEBUG_OUTPUT
			end = DateTime.Now;
			sw.WriteLine("PermuteFactTypeMappings: " +end.Subtract(start).ToString());
			sw.Close();
#endif
		}

		private void FindDecidedOneToOneMappings()
		{
			myDecidedOneToOneFactTypeMappings = new FactTypeMappingDictionary(myDecidedFactTypeMappings.Count);
			FactTypeMapping mapping;
			Role firstRole;
			Role secondRole;
			UniquenessConstraint firstRoleUniquenessConstraint;
			UniquenessConstraint secondRoleUniquenessConstraint;
			bool firstRoleIsUnique;
			bool secondRoleIsUnique;
			foreach (KeyValuePair<FactType, FactTypeMapping> pair in myDecidedFactTypeMappings)
			{
				mapping = pair.Value;
				firstRole = mapping.FromRole;
				secondRole = mapping.TowardsRole;
				firstRoleUniquenessConstraint = (UniquenessConstraint)firstRole.SingleRoleAlethicUniquenessConstraint;
				secondRoleUniquenessConstraint = (UniquenessConstraint)secondRole.SingleRoleAlethicUniquenessConstraint;
				firstRoleIsUnique = (firstRoleUniquenessConstraint != null);
				secondRoleIsUnique = (secondRoleUniquenessConstraint != null);

				if (firstRoleIsUnique && secondRoleIsUnique)
				{
					myDecidedOneToOneFactTypeMappings.Add(pair.Key, pair.Value);
				}
			}
		}

		private void PermuteFactTypeMappings()
		{
			int largestChain = 0;
			// Break up the chains of contiguous undecided fact types
			UndecidedMappingChains chains = new UndecidedMappingChains(myUndecidedFactTypeMappings, myDecidedOneToOneFactTypeMappings);
#if PERMUTATION_DEBUG_OUTPUT
			chains.Run(sw, ref largestChain);
#else
			chains.Run(ref largestChain);
#endif
			// This object validates against deep mapping an object type in two directions
			ObjectTypeList deepMappings = new ObjectTypeList(largestChain);
			FactTypeMappingDictionary myDecidedFactTypeMappings = new FactTypeMappingDictionary(300000);
			// Stores a list of object types that had been considered valid top-level, but was later found to be deeply mapped away
			myInvalidObjectTypes = new ObjectTypeDictionary(myDecidedFactTypeMappings.Count);
			myPossibleTopLevelConceptTypes = new ObjectTypeDictionary(myDecidedFactTypeMappings.Count);
			myPossibleConceptTypes = new ObjectTypeDictionary(myDecidedFactTypeMappings.Count);
			// Perform one-time pass of top-level types for the decided mappings
			PrecalculateDecidedConceptTypes();

			// Loop through each chain, obtaining the permutations
			foreach (Chain chain in chains.Chains)
			{
				MappingState initialState = new MappingState(myDecidedFactTypeMappings, chain.MappingList);
				PermuteFactTypeMappingsRecurse(new PermutationState(initialState, chain.PossibleFactTypeMappings, deepMappings, true));
				CalculateTopLevelConceptTypes(chain);
				ChooseBest(chain);
			}
			BuildFinalMappingList(chains.Chains);
			OutputResults(myPossibleFactTypeMappings);
		}

		private void ChooseBest(Chain chain)
		{
			// UNDONE: This should do something smart!
			FinalMappingStateList smallestPermutationsList = chain.SmallestPermutationsList;
			FinalMappingState firstFinalMappingState = smallestPermutationsList[0];
			smallestPermutationsList.Clear();
			smallestPermutationsList.Add(firstFinalMappingState);
		}

		private void BuildFinalMappingList(ChainList chains)
		{
			FinalMappingState state = new FinalMappingState();
			foreach (Chain chain in chains)
			{
				Debug.Assert(chain.SmallestPermutationsList.Count == 1);
				FinalMappingState chainstate = chain.SmallestPermutationsList[0];
				foreach (DecidedMappingStateEntry entry in chainstate.Mappings)
				{
					state.Mappings.Add(entry);
				}
			}
			mySmallestPermutationsList.Add(state);
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
		///			b. There exists a role being mapped to object type A on which there is a uniqueness that is not preferred.
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
#if PERMUTATION_DEBUG_OUTPUT
			DateTime start = DateTime.Now;
#endif
			// The smallest overall mapping that we've found
			int smallest = int.MaxValue;
			// These dictionaries track the OTs that are temporarily removed or added for each permutation in the list
			ObjectTypeList garbage = new ObjectTypeList(myDecidedFactTypeMappings.Count);
			ObjectTypeList restore = new ObjectTypeList(myDecidedFactTypeMappings.Count);
			ObjectTypeList conceptTypeGarbage = new ObjectTypeList(myDecidedFactTypeMappings.Count);
			// Now include the permutations in the calculation of top-level types.
			for (int i = 0; i < chain.PossibleFactTypeMappings.Count; i++)
			{
				garbage.Clear();
				restore.Clear();
				FinalMappingState state = chain.PossibleFactTypeMappings[i];
				foreach (DecidedMappingStateEntry mappingState in state.Mappings)
				{
					ProcessEntityState entitystate = new ProcessEntityState(mappingState.Mapping, restore, garbage, conceptTypeGarbage);
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
					FinalMappingStateList smallestList = chain.SmallestPermutationsList;
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
#if PERMUTATION_DEBUG_OUTPUT
			DateTime endat = DateTime.Now;
			// Output values to a file
			StreamWriter s = new StreamWriter("output_combinations.txt");
			s.WriteLine("Smallest mapping: " + smallest.ToString());
			s.WriteLine("# calculated as smallest (efficiency): " + mySmallestPermutationsList.Count);
			s.WriteLine("Time: " + endat.Subtract(start).ToString());
			/*
			int p = 0;
			foreach (FinalMappingState state in smallestList)
			{
				p++;
				sw.WriteLine("\t" + p.ToString() + " (" + state.TopLevelConceptTypes.ToString() + " TLCTs)");
				foreach (KeyValuePair<FactType, FactTypeMapping> pair in myDecidedFactTypeMappings)
				{
					FactTypeMapping mapping = pair.Value;
					s.WriteLine("\t\t" + mapping.FromObjectType.ToString() + " to " + mapping.TowardsObjectType.ToString() + " (" + mapping.MappingType.ToString() + ")");
				}
				foreach (DecidedMappingStateEntry mapping in state.Mappings)
				{
					s.WriteLine("\t\t" + mapping.Mapping.FromObjectType.ToString() + " to " + mapping.Mapping.TowardsObjectType.ToString() + " (" + mapping.Mapping.MappingType.ToString() + ")");
				}
			}
			 */
			s.Close();
#endif
		}

		private void OutputResults(FinalMappingStateList finalStates)
		{
#if PERMUTATION_DEBUG_OUTPUT
			// Statistics
			sw.WriteLine("Total permutations: " + finalStates.Count.ToString());

			/*
			// Headings
			string top = "Fact Type".PadRight(40) + "| " + "From".PadRight(40) + "| " + "To".PadRight(40) + "| " + "Type".PadRight(10);
			sw.WriteLine(top);
			sw.WriteLine("".PadLeft(top.Length, '-'));

			// Permutation states
			foreach (FinalMappingState finalState in finalStates)
			{
				foreach (KeyValuePair<FactType, FactTypeMapping> pair in finalState.Mappings)
				{
					FactTypeMapping mapping = pair.Value;
					sw.Write(mapping.FactType.Name.PadRight(40));
					sw.Write("| " + mapping.FromObjectType.Name.PadRight(40));
					sw.Write("| " + mapping.TowardsObjectType.Name.PadRight(40));
					sw.Write("| " + mapping.MappingType.ToString().PadRight(10));
					sw.WriteLine();
				}
				sw.WriteLine();
			}
			 */
#endif
		}

		private void PermuteFactTypeMappingsRecurse(PermutationState permstate)
		{
			MappingState mapstate = permstate.MappingState;
			FactTypeMappingListDictionary undecided = new FactTypeMappingListDictionary(mapstate.Undecided);
			// Loop through each undecided mapping state
			foreach (KeyValuePair<FactType, FactTypeMappingList> pair in mapstate.Undecided)
			{
				FactType factType = pair.Key;
				FactTypeMappingList maplist = pair.Value;
				for (int i = 0; i < maplist.Count; i++)
				{
					FactTypeMapping mapping = maplist[i];
					MappingDepth mappingType = mapping.MappingDepth;
					// An object type can only be deeply mapped once
					if (mappingType == MappingDepth.Deep && permstate.DeepMappings.Contains(mapping.FromObjectType))
					{
						// The permutation never makes to the "possible" list since it would map the object type deeply in two directions
						break;
					}
					// Temporarily set the state of decided & undecided collections
					mapstate.Decided.Add(factType, mapping);
					undecided.Remove(factType);
					// Generate a new state to represent the collections
					MappingState newmapstate = new MappingState(mapstate.Decided, undecided);
					PermutationState newpermstate = new PermutationState(newmapstate, permstate.PossibleFactTypeMappings, permstate.DeepMappings, false);
					// If there are no more undecided states left, add it as a final state
					if (undecided.Count == 0)
					{
						permstate.PossibleFactTypeMappings.Add(new FinalMappingState(permstate.MappingState));
					}
					else
					{
						if (mappingType == MappingDepth.Deep)
						{
							permstate.DeepMappings.Add(mapping.FromObjectType);
						}
						PermuteFactTypeMappingsRecurse(newpermstate);
						if (mappingType == MappingDepth.Deep)
						{
							permstate.DeepMappings.Remove(mapping.FromObjectType);
						}
					}
					// Restore from temporary state
					mapstate.Decided.Remove(factType);
					undecided.Add(factType, maplist);
				}
				if (permstate.IsRoot)
				{
					return;
				}
			}
		}
	}
	#endregion
}
