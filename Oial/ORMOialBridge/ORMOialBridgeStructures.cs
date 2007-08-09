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
	class InformationTypeFormatWithFactTypes
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
			this.factTypes = factTypes;
		}
	}

	[Serializable]
	[DebuggerDisplay("FactTypeMapping (TowardsRole={FactType.RoleCollection.IndexOf(FromRole)}, Depth={MappingDepth}, FactType={FactType.Name})")]
	sealed class FactTypeMapping
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
				// If fromRole does not have a preferred uniqueness constraint on it...
				if (uniquenessConstraint != null && uniquenessConstraint.IsPreferred)
				{
					return true;
				}
			}
			return false;
		}
	}
	#region Permutation structures
	[Serializable]
	sealed class FactTypeList : List<FactType>
	{
	}
	[Serializable]
	sealed class ObjectTypeList : List<ObjectType>
	{
		public ObjectTypeList()
		{
		}

		public ObjectTypeList(int size)
			: base(size)
		{
		}
	}
	[Serializable]
	sealed class ObjectTypeDictionary : Dictionary<ObjectType, bool>
	{
		public ObjectTypeDictionary(int capacity)
			: base(capacity)
		{
		}

		private ObjectTypeDictionary(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
	[Serializable]
	sealed class FactTypeMappingDictionary : Dictionary<FactType, FactTypeMapping>
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
	sealed class FactTypeMappingListDictionary : Dictionary<FactType, FactTypeMappingList>
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
	sealed class PermutationList : List<Permutation>
	{
	}
	[Serializable]
	sealed class FactTypeMappingList : List<FactTypeMapping>
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
	sealed class FactTypeMappingListList : List<FactTypeMappingList>
	{
		public FactTypeMappingListList()
		{
		}
		public FactTypeMappingListList(int capacity)
			: base(capacity)
		{
		}
	}

	[Serializable]
	sealed class Permutation
	{
		private readonly FactTypeMappingList myMappings;
		private int myTopLevelConceptTypes;
		private int myConceptTypes;

		public Permutation(FactTypeMappingList mappings)
		{
			myMappings = mappings;
		}

		public FactTypeMappingList Mappings
		{
			get { return myMappings; }
		}

		public int TopLevelConceptTypes
		{
			get { return myTopLevelConceptTypes; }
			set { myTopLevelConceptTypes = value; }
		}

		public int ConceptTypes
		{
			get { return myConceptTypes; }
			set { myConceptTypes = value; }
		}
	}

	[Serializable]
	struct ProcessEntityState
	{
		public readonly FactTypeMapping Mapping;
		public readonly ObjectTypeList Restore;
		public readonly ObjectTypeList Garbage;
		public readonly ObjectTypeList ConceptTypeGarbage;

		public ProcessEntityState(FactTypeMapping mapping, ObjectTypeList restore, ObjectTypeList garbage, ObjectTypeList conceptTypeGarbage)
		{
			Mapping = mapping;
			Restore = restore;
			Garbage = garbage;
			ConceptTypeGarbage = conceptTypeGarbage;
		}
	}

	[Serializable]
	sealed class ChainList : List<Chain>
	{
	}

	[Serializable]
	sealed class Chain
	{
		private readonly FactTypeMappingList myPredecidedOneToOneFactTypeMappings;
		private readonly FactTypeMappingListList myUndecidedFactTypeMappings;
		private readonly PermutationList myPossiblePermutations;
		private readonly PermutationList mySmallestPermutations;

		public Chain()
		{
			myPredecidedOneToOneFactTypeMappings = new FactTypeMappingList();
			myUndecidedFactTypeMappings = new FactTypeMappingListList();
			myPossiblePermutations = new PermutationList();
			mySmallestPermutations = new PermutationList();
		}

		/// <summary>
		/// Returns the number of FactTypes in this Chain.
		/// </summary>
		public int FactTypeCount
		{
			get
			{
				return myPredecidedOneToOneFactTypeMappings.Count + myUndecidedFactTypeMappings.Count;
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
		/// The potential mappings of the undecided FactTypes that are in this Chain.
		/// </summary>
		public FactTypeMappingListList UndecidedFactTypeMappings
		{
			get { return myUndecidedFactTypeMappings; }
		}

		/// <summary>
		/// Contains a list of all possible permutations for this chain.  Fully populated after LiveOialPermuter.PermuteFactTypeMappingsRecurse is finished with the chain.
		/// </summary>
		public PermutationList PossiblePermutations
		{
			get { return myPossiblePermutations; }
		}

		/// <summary>
		/// The entries from PossiblePermutations which map to the smallest number of top-level concept types.
		/// </summary>
		public PermutationList SmallestPermutations
		{
			get { return mySmallestPermutations; }
		}
	}
	#endregion
}