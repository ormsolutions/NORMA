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
using System.Runtime.Serialization;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.ORMAbstraction;

namespace ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge
{
	partial class AbstractionModelIsForORMModel
	{
		#region FactTypeMapping class
		/// <summary>
		/// Flags used by the <see cref="FactTypeMapping"/> class
		/// </summary>
		[Flags]
		private enum FactTypeMappingFlags
		{
			/// <summary>
			/// No flags
			/// </summary>
			None = 0,
			/// <summary>
			/// This is a deep mapping
			/// </summary>
			DeepMapping = 1,
			/// <summary>
			/// The towards role player is a value type
			/// </summary>
			TowardsValueType = 2,
			/// <summary>
			/// The towards role is mandatory (implied or explicit)
			/// </summary>
			TowardsRoleMandatory = 4,
			/// <summary>
			/// The mandatory constraint on the towards role is implied
			/// </summary>
			TowardsRoleImpliedMandatory = 8,
			/// <summary>
			/// The from role player is a value type
			/// </summary>
			FromValueType = 0x10,
			/// <summary>
			/// The from role is mandatory (implied or explicit)
			/// </summary>
			FromRoleMandatory = 0x20,
			/// <summary>
			/// The mandatory constraint on the from role is implied
			/// </summary>
			FromRoleImpliedMandatory = 0x40,
			/// <summary>
			/// This mapping is for a subtype fact
			/// </summary>
			Subtype = 0x80,
			/// <summary>
			/// The mapping is from a preferred identifier. Note that this is
			/// calculated automatically in the constructor.
			/// </summary>
			FromPreferredIdentifier = 0x100,
		}
		/// <summary>
		/// Indicates towards which <see cref="Role"/> a binary <see cref="FactType"/> is mapped,
		/// as well as the <see cref="MappingDepth"/> of that mapping.
		/// </summary>
		/// <remarks>
		/// This type is the non-transacted counterpart to <see cref="FactTypeMapsTowardsRole"/>.
		/// </remarks>
		[Serializable]
		[DebuggerDisplay("FactTypeMapping (TowardsRole={FactType.RoleCollection.IndexOf((RoleBase)towardsRole.Proxy ?? towardsRole)}, Depth={MappingDepth}, FactType={FactType.Name})")]
		private sealed class FactTypeMapping
		{
			private readonly FactType myFactType;
			private readonly Role myFromRole;
			private readonly Role myTowardsRole;
			private readonly ObjectType myFromObjectType;
			private readonly ObjectType myTowardsObjectType;
			private readonly FactTypeMappingFlags myFlags;

			/// <value>
			/// The <see cref="FactType"/> that this mapping is for.
			/// </value>
			public FactType FactType
			{
				get { return myFactType; }
			}

			/// <value>
			/// The <see cref="ObjectType"/> away from which the <see cref="FactType"/> has been mapped.
			/// </value>
			public ObjectType FromObjectType
			{
				get { return myFromObjectType; }
			}

			/// <value>
			/// The <see cref="ObjectType"/> toward which the <see cref="FactType"/> has been mapped.
			/// </value>
			public ObjectType TowardsObjectType
			{
				get { return myTowardsObjectType; }
			}

			/// <value>
			/// The <see cref="Role"/> of the <see cref="ObjectType"/> away from which the <see cref="FactType"/> has been mapped.
			/// </value>
			public Role FromRole
			{
				get { return myFromRole; }
			}

			/// <value>
			/// The <see cref="Role"/> of the <see cref="ObjectType"/> toward which the <see cref="FactType"/> has been mapped.
			/// </value>
			public Role TowardsRole
			{
				get { return myTowardsRole; }
			}

			public MappingDepth MappingDepth
			{
				get { return 0 != (myFlags & FactTypeMappingFlags.DeepMapping) ? MappingDepth.Deep : MappingDepth.Shallow; }
			}

			public bool IsFromPreferredIdentifier
			{
				get { return 0 != (myFlags & FactTypeMappingFlags.FromPreferredIdentifier); }
			}

			public bool IsSubtype
			{
				get { return 0 != (myFlags & FactTypeMappingFlags.Subtype); }
			}

			public bool FromValueType
			{
				get { return 0 != (myFlags & FactTypeMappingFlags.FromValueType); }
			}

			public bool FromRoleMandatory
			{
				get { return 0 != (myFlags & FactTypeMappingFlags.FromRoleMandatory); }
			}

			public bool FromRoleExplicitlyMandatory
			{
				get { return FactTypeMappingFlags.FromRoleMandatory == (myFlags & (FactTypeMappingFlags.FromRoleMandatory | FactTypeMappingFlags.FromRoleImpliedMandatory)); }
			}

			public bool TowardsValueType
			{
				get { return 0 != (myFlags & FactTypeMappingFlags.TowardsValueType); }
			}

			public bool TowardsRoleMandatory
			{
				get { return 0 != (myFlags & FactTypeMappingFlags.TowardsRoleMandatory); }
			}

			public bool TowardsRoleExplicitlyMandatory
			{
				get { return FactTypeMappingFlags.TowardsRoleMandatory == (myFlags & (FactTypeMappingFlags.TowardsRoleMandatory | FactTypeMappingFlags.TowardsRoleImpliedMandatory)); }
			}

			public FactTypeMappingFlags Flags
			{
				get { return myFlags; }
			}

			public FactTypeMapping(FactType factType, Role fromRole, Role towardsRole, FactTypeMappingFlags flags)
			{
				myFactType = factType;
				myFromRole = fromRole;
				myTowardsRole = towardsRole;
				myFromObjectType = fromRole.RolePlayer;
				myTowardsObjectType = towardsRole.RolePlayer;
				if (0 == (flags & FactTypeMappingFlags.FromPreferredIdentifier) &&
					DetermineWhetherFromIsPreferredIdentifier())
				{
					flags |= FactTypeMappingFlags.FromPreferredIdentifier;
				}
				myFlags = flags;
			}

			private bool DetermineWhetherFromIsPreferredIdentifier()
			{
				foreach (ConstraintRoleSequence constraintRoleSequence in myFromRole.ConstraintRoleSequenceCollection)
				{
					UniquenessConstraint uniquenessConstraint = constraintRoleSequence as UniquenessConstraint;
					if (uniquenessConstraint != null && uniquenessConstraint.IsPreferred)
					{
						return true;
					}
				}
				return false;
			}
		}
		#endregion // FactTypeMapping class

		#region Permutation structures
		[Serializable]
		private sealed class FactTypeMappingDictionary : Dictionary<FactType, FactTypeMapping>
		{
			public FactTypeMappingDictionary()
			{
			}

			public FactTypeMappingDictionary(int size)
				: base(size)
			{
			}

			public FactTypeMappingDictionary(FactTypeMappingDictionary values)
				: base(values.Count)
			{
				foreach (KeyValuePair<FactType, FactTypeMapping> pair in values)
				{
					Add(pair.Key, pair.Value);
				}
			}

			private FactTypeMappingDictionary(SerializationInfo info, StreamingContext context)
				: base(info, context)
			{
			}
		}
		[Serializable]
		private sealed class FactTypeMappingListDictionary : Dictionary<FactType, FactTypeMappingList>
		{
			public FactTypeMappingListDictionary()
			{
			}

			public FactTypeMappingListDictionary(int size)
				: base(size)
			{
			}
			private FactTypeMappingListDictionary(SerializationInfo info, StreamingContext context)
				: base(info, context)
			{
			}
		}
		[Serializable]
		private sealed class FactTypeMappingList : List<FactTypeMapping>
		{
			public FactTypeMappingList()
			{
			}

			public FactTypeMappingList(int size)
				: base(size)
			{
			}

			public FactTypeMappingList(IEnumerable<FactTypeMapping> collection)
				: base(collection)
			{
			}
		}
		[Serializable]
		private sealed class FactTypeMappingListList : List<FactTypeMappingList>
		{
			public FactTypeMappingListList()
			{
			}
			public FactTypeMappingListList(int capacity)
				: base(capacity)
			{
			}
		}
		partial class FactTypeMappingPermuter
		{
			[Serializable]
			private sealed class Permutation
			{
				#region ConceptTypeEntry struct
				private struct ConceptTypeEntry : IEquatable<ConceptTypeEntry>
				{
					public static IComparer<ConceptTypeEntry> Comparer = new ConceptTypeEntryComparer();
					private class ConceptTypeEntryComparer : IComparer<ConceptTypeEntry>
					{
						#region IComparer<ConceptTypeEntry> Implementation
						public int Compare(ConceptTypeEntry x, ConceptTypeEntry y)
						{
							// Put top level first
							if (x.IsTopLevel)
							{
								if (!y.IsTopLevel)
								{
									return -1;
								}
							}
							else if (y.IsTopLevel)
							{
								return 1;
							}
							return x.ObjectType.Id.CompareTo(y.ObjectType.Id);
						}
					}
					public ConceptTypeEntry(ObjectType objectType, bool isTopLevel)
					{
						this.ObjectType = objectType;
						this.IsTopLevel = isTopLevel;
					}
					public ObjectType ObjectType;
					public bool IsTopLevel;
					#endregion // IComparer<ConceptTypeEntry> Implementation

					#region IEquatable<ConceptTypeEntry> Implementation
					public bool Equals(ConceptTypeEntry other)
					{
						return ObjectType == other.ObjectType && IsTopLevel == other.IsTopLevel;
					}
					#endregion // IEquatable<ConceptTypeEntry> Implementation
				}
				#endregion // ConceptTypeEntry struct
				private readonly FactTypeMappingList myMappings;
				private ConceptTypeEntry[] myConceptTypes;

				public Permutation(FactTypeMappingList mappings)
				{
					myMappings = mappings;
				}

				public FactTypeMappingList Mappings
				{
					get { return myMappings; }
				}
				/// <summary>
				/// Test if the opposite permutation is identical
				/// </summary>
				public bool IsIdentical(Permutation otherPermutation)
				{
					ConceptTypeEntry[] leftEntries = myConceptTypes;
					ConceptTypeEntry[] rightEntries = otherPermutation.myConceptTypes;
					if (leftEntries != null && rightEntries != null)
					{
						int count = leftEntries.Length;
						if (rightEntries.Length == count)
						{
							for (int i = 0; i < count; ++i)
							{
								if (!leftEntries[i].Equals(rightEntries[i]))
								{
									return false;
								}
							}
							return true;
						}
					}
					return false;
				}
				/// <summary>
				/// Set top level and non top level concept types after
				/// the optimal permutation state has been determined
				/// </summary>
				/// <param name="permutationState"><see cref="ChainPermutationState"/> containing the top level and non top level ObjectTypes</param>
				public void SetConceptTypes(ChainPermutationState permutationState)
				{
					Debug.Assert(myConceptTypes == null);
					int count = permutationState.TopLevelCount + permutationState.NonTopLevelCount;
					ConceptTypeEntry[] entries = new ConceptTypeEntry[count];
					if (count != 0)
					{
						int index = 0;
						foreach (KeyValuePair<ObjectType, ObjectTypeStates> pair in permutationState.EnumerateStates(ObjectTypeStates.TopLevelInPermutation | ObjectTypeStates.NonTopLevelInPermutation))
						{
							entries[index] = new ConceptTypeEntry(pair.Key, 0 != (pair.Value & ObjectTypeStates.TopLevelInPermutation));
							++index;
						}
						Debug.Assert(index == count);
						if (count > 1)
						{
							Array.Sort<ConceptTypeEntry>(entries, ConceptTypeEntry.Comparer);
						}
					}
					myConceptTypes = entries;
				}
			}

			[Serializable]
			private sealed class ChainList : List<Chain>
			{
			}

			[Serializable]
			private sealed class Chain
			{
				private readonly List<ObjectType> myObjectTypes;
				private readonly FactTypeMappingList myPredecidedManyToOneFactTypeMappings;
				private readonly FactTypeMappingList myPredecidedOneToOneFactTypeMappings;
				private readonly FactTypeMappingListList myUndecidedOneToOneFactTypeMappings;
				private readonly List<Permutation> myPossiblePermutations;
				private readonly List<Permutation> mySmallestPermutationsInTermsOfConceptTypes;

				public Chain()
				{
					myObjectTypes = new List<ObjectType>();
					myPredecidedManyToOneFactTypeMappings = new FactTypeMappingList();
					myPredecidedOneToOneFactTypeMappings = new FactTypeMappingList();
					myUndecidedOneToOneFactTypeMappings = new FactTypeMappingListList();
					myPossiblePermutations = new List<Permutation>();
					mySmallestPermutationsInTermsOfConceptTypes = new List<Permutation>();
				}

				/// <summary>
				/// Returns the number of one-to-one FactTypes in this Chain.
				/// </summary>
				public int OneToOneFactTypeCount
				{
					get
					{
						return myPredecidedOneToOneFactTypeMappings.Count + myUndecidedOneToOneFactTypeMappings.Count;
					}
				}

				/// <summary>
				/// The set of all <see cref="ObjectType"/>s that play a role in a one-to-one <see cref="FactType"/>
				/// in this <see cref="Chain"/>.
				/// </summary>
				public List<ObjectType> ObjectTypes
				{
					get
					{
						return myObjectTypes;
					}
				}

				/// <summary>
				/// Many-to-one FactTypeMappings that are part of this chain but were decided before the permutation phase.
				/// </summary>
				public FactTypeMappingList PredecidedManyToOneFactTypeMappings
				{
					get
					{
						return myPredecidedManyToOneFactTypeMappings;
					}
				}

				/// <summary>
				/// One-to-one FactTypeMappings that are part of this chain but were decided before the permutation phase.
				/// </summary>
				public FactTypeMappingList PredecidedOneToOneFactTypeMappings
				{
					get
					{
						return myPredecidedOneToOneFactTypeMappings;
					}
				}

				/// <summary>
				/// The potential mappings of the undecided one-to-one FactTypes that are in this Chain.
				/// </summary>
				public FactTypeMappingListList UndecidedOneToOneFactTypeMappings
				{
					get { return myUndecidedOneToOneFactTypeMappings; }
				}

				/// <summary>
				/// Contains a list of all possible permutations for this chain.  Fully populated after LiveOialPermuter.PermuteFactTypeMappingsRecurse is finished with the chain.
				/// </summary>
				public IList<Permutation> PossiblePermutations
				{
					get { return myPossiblePermutations; }
				}

				/// <summary>
				/// The entries from PossiblePermutations which map to the smallest number of top-level concept types and overall concept types.
				/// </summary>
				public IList<Permutation> SmallestPermutationsInTermsOfConceptTypes
				{
					get { return mySmallestPermutationsInTermsOfConceptTypes; }
				}
			}
		}
	}
	#endregion
}