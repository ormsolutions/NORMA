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
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.Modeling;
using Neumont.Tools.ORMAbstraction;

namespace Neumont.Tools.ORMToORMAbstractionBridge
{
	partial class AbstractionModelIsForORMModel
	{
		private class InformationTypeFormatWithFactTypes
		{
			InformationTypeFormat informationTypeFormat;
			IList<FactType> factTypes;

			public InformationTypeFormat InformationTypeFormat
			{
				get { return informationTypeFormat; }
				set { informationTypeFormat = value; }
			}

			public IList<FactType> FactTypes
			{
				get { return factTypes; }
				set { factTypes = value; }
			}

			public InformationTypeFormatWithFactTypes()
			{
				factTypes = new List<FactType>();
			}

			public InformationTypeFormatWithFactTypes(InformationTypeFormat informationTypeFormat, IList<FactType> factTypes)
			{
				this.informationTypeFormat = informationTypeFormat;
				// UNDONE: There might be a better place to clone this list...
				this.factTypes = new List<FactType>(factTypes);
			}
		}

		#region FactTypeMapping class
		/// <summary>
		/// Indicates towards which <see cref="Role"/> a binary <see cref="FactType"/> is mapped,
		/// as well as the <see cref="MappingDepth"/> of that mapping.
		/// </summary>
		/// <remarks>
		/// This type is the non-transacted counterpart to <see cref="FactTypeMapsTowardsRole"/>.
		/// </remarks>
		[Serializable]
		[DebuggerDisplay("FactTypeMapping (TowardsRole={FactType.RoleCollection.IndexOf(TowardsRoleDebug)}, Depth={MappingDepth}, FactType={FactType.Name})")]
		private sealed class FactTypeMapping
		{
			private readonly FactType factType;
			private readonly Role fromRole;
			private readonly Role towardsRole;
			private readonly ObjectType fromObjectType;
			private readonly ObjectType towardsObjectType;
			private readonly MappingDepth mappingDepth;
			private readonly bool isFromPreferredIdentifier;

			/// <value>
			/// The <see cref="FactType"/> that this mapping is for.
			/// </value>
			public FactType FactType
			{
				get { return factType; }
			}

			/// <value>
			/// The <see cref="ObjectType"/> away from which the <see cref="FactType"/> has been mapped.
			/// </value>
			public ObjectType FromObjectType
			{
				get { return fromObjectType; }
			}

			/// <value>
			/// The <see cref="ObjectType"/> toward which the <see cref="FactType"/> has been mapped.
			/// </value>
			public ObjectType TowardsObjectType
			{
				get { return towardsObjectType; }
			}

			/// <value>
			/// The <see cref="Role"/> of the <see cref="ObjectType"/> away from which the <see cref="FactType"/> has been mapped.
			/// </value>
			public Role FromRole
			{
				get { return fromRole; }
			}

			/// <value>
			/// The <see cref="Role"/> of the <see cref="ObjectType"/> toward which the <see cref="FactType"/> has been mapped.
			/// </value>
			public Role TowardsRole
			{
				get { return towardsRole; }
			}

			/// <summary>
			/// Used by the <see cref="DebuggerDisplayAttribute"/> for this type.
			/// </summary>
			private RoleBase TowardsRoleDebug
			{
				get { return (RoleBase)towardsRole.Proxy ?? towardsRole; }
			}

			public MappingDepth MappingDepth
			{
				get { return mappingDepth; }
			}

			public bool IsFromPreferredIdentifier
			{
				get { return isFromPreferredIdentifier; }
			}

			public FactTypeMapping(FactType factType, Role fromRole, Role towardsRole, MappingDepth mappingDepth)
			{
				this.factType = factType;
				this.fromRole = fromRole;
				this.towardsRole = towardsRole;
				this.mappingDepth = mappingDepth;

				this.fromObjectType = fromRole.RolePlayer;
				this.towardsObjectType = towardsRole.RolePlayer;
				this.isFromPreferredIdentifier = DetermineWhetherFromIsPreferredIdentifier();
			}

			private bool DetermineWhetherFromIsPreferredIdentifier()
			{
				foreach (ConstraintRoleSequence constraintRoleSequence in fromRole.ConstraintRoleSequenceCollection)
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