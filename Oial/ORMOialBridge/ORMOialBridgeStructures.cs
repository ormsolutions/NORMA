using System;
using System.Collections.Generic;
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

	struct FactTypeMapping
	{
		private FactType factType;
		private Role fromRole;
		private Role towardsRole;
		private ObjectType fromObjectType;
		private ObjectType towardsObjectType;
		private MappingDepth mappingDepth;
		private bool isFromPreferredIdentifier;

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
			this.isFromPreferredIdentifier = false;
			DetermineWhetherFromIsPreferredIdentifier();
		}

		private void DetermineWhetherFromIsPreferredIdentifier()
		{
			foreach (ConstraintRoleSequence constraintRoleSequence in fromRole.ConstraintRoleSequenceCollection)
			{
				UniquenessConstraint uniquenessConstraint = constraintRoleSequence as UniquenessConstraint;
				// If fromRole does not have a preferred uniqueness constraint on it...
				if (uniquenessConstraint != null && uniquenessConstraint.IsPreferred)
				{
					this.isFromPreferredIdentifier = true;
					return;
				}
			}
		}
	}
	#region Permutation structures
	class FactTypeLinkedElementCollection : LinkedElementCollection<FactType> { }
	class ObjectTypeLinkedElementCollection : LinkedElementCollection<ObjectType> { }
	class FactTypeList : List<FactType> { }
	class ObjectTypeList : List<ObjectType>
	{
		public ObjectTypeList()
		{
		}

		public ObjectTypeList(int size)
			: base(size)
		{
		}
	}
	class ObjectTypeListDictionary : Dictionary<ObjectType, ObjectTypeList> { }
	class ObjectTypeDictionary : Dictionary<ObjectType, bool>
	{
		public ObjectTypeDictionary(int size)
			: base(size)
		{
		}
	}

	class FactTypeMappingDictionary : Dictionary<FactType, FactTypeMapping>
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
	}

	class FactTypeMappingListDictionary : Dictionary<FactType, FactTypeMappingList>
	{
		public FactTypeMappingListDictionary()
		{
		}

		public FactTypeMappingListDictionary(int size)
			: base(size)
		{
		}

		public FactTypeMappingListDictionary(FactTypeMappingListDictionary values)
			: base(values.Count)
		{
			foreach (KeyValuePair<FactType, FactTypeMappingList> pair in values)
			{
				FactTypeMappingList oldlist = pair.Value;
				FactTypeMappingList list = new FactTypeMappingList(oldlist.Count);
				for (int i = 0; i < oldlist.Count; i++)
				{
					list.Add(oldlist[i]);
				}
				Add(pair.Key, list);
			}
		}
	}

	class FinalMappingStateList : List<FinalMappingState>
	{
		private int myTopLevelConceptTypes = 0;

		public int TopLevelConceptTypes
		{
			get { return myTopLevelConceptTypes; }
			set { myTopLevelConceptTypes = value; }
		}
	}
	class FactTypeMappingList : List<FactTypeMapping>
	{
		public FactTypeMappingList()
		{
		}

		public FactTypeMappingList(int size)
			: base(size)
		{
		}
	}
	class FactTypeMappingListDictionaryList : List<FactTypeMappingListDictionary> { }

	class DecidedMappingStateEntryList : List<DecidedMappingStateEntry>
	{
		private FactTypeMappingDictionary myFactTypeMappings;

		public DecidedMappingStateEntryList(int size)
			: base(size)
		{
			myFactTypeMappings = new FactTypeMappingDictionary(size);
		}

		public DecidedMappingStateEntryList(DecidedMappingStateEntryList values)
			: this(values.Count)
		{
			this.AddRange(values);
		}

		public DecidedMappingStateEntryList(FactTypeMappingDictionary values)
			: this(values.Count)
		{
			foreach (KeyValuePair<FactType, FactTypeMapping> pair in values)
			{
				this.Add(new DecidedMappingStateEntry(pair.Key, pair.Value));
			}
		}

		public new void Add(DecidedMappingStateEntry value)
		{
			myFactTypeMappings.Add(value.FactType, value.Mapping);
			base.Add(value);
		}

		public bool TryGetByFactType(FactType factType, out FactTypeMapping mapping)
		{
			return (myFactTypeMappings.TryGetValue(factType, out mapping));
		}
	}

	class UndecidedMappingStateEntryList : List<UndecidedMappingStateEntry>
	{
		public UndecidedMappingStateEntryList(int size)
			: base(size)
		{
		}

		public UndecidedMappingStateEntryList(UndecidedMappingStateEntryList values)
			: this(values.Count)
		{
			this.AddRange(values);
		}
	}

	struct DecidedMappingStateEntry
	{
		public FactType FactType;
		public FactTypeMapping Mapping;

		public DecidedMappingStateEntry(FactType factType, FactTypeMapping mapping)
		{
			FactType = factType;
			Mapping = mapping;
		}
	}

	struct UndecidedMappingStateEntry
	{
		public FactType FactType;
		public FactTypeMappingList MappingList;

		public UndecidedMappingStateEntry(FactType factType, FactTypeMappingList mappingList)
		{
			FactType = factType;
			MappingList = mappingList;
		}
	}

	class MappingState
	{
		public FactTypeMappingDictionary Decided;
		public FactTypeMappingListDictionary Undecided;

		public MappingState(FactTypeMappingDictionary decided, FactTypeMappingListDictionary undecided)
		{
			Decided = decided;
			Undecided = undecided;
		}
	}

	class FinalMappingState
	{
		private DecidedMappingStateEntryList myMappings = null;
		private int myTopLevelConceptTypes = 0;
		private int myConceptTypes = 0;

		public FinalMappingState(MappingState state)
		{
			myMappings = new DecidedMappingStateEntryList(state.Decided);
		}

		public FinalMappingState()
		{
			myMappings = new DecidedMappingStateEntryList(0);
		}

		public DecidedMappingStateEntryList Mappings
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

	struct PermutationState
	{
		public MappingState MappingState;
		public FinalMappingStateList PossibleFactTypeMappings;
		public ObjectTypeList DeepMappings;
		public bool IsRoot;

		public PermutationState(MappingState mappingState, FinalMappingStateList possibleFactTypeMappings, ObjectTypeList deepMappings, bool isRoot)
		{
			MappingState = mappingState;
			PossibleFactTypeMappings = possibleFactTypeMappings;
			DeepMappings = deepMappings;
			IsRoot = isRoot;
		}
	}

	struct ProcessEntityState
	{
		public FactTypeMapping Mapping;
		public ObjectTypeList Restore;
		public ObjectTypeList Garbage;
		public ObjectTypeList ConceptTypeGarbage;

		public ProcessEntityState(FactTypeMapping mapping, ObjectTypeList restore, ObjectTypeList garbage, ObjectTypeList conceptTypeGarbage)
		{
			Mapping = mapping;
			Restore = restore;
			Garbage = garbage;
			ConceptTypeGarbage = conceptTypeGarbage;
		}
	}

	class ChainList : List<Chain> { }

	class Chain
	{
		FactTypeMappingListDictionary myMappingList = new FactTypeMappingListDictionary(10);
		FinalMappingStateList myPossibleFactTypeMappings = new FinalMappingStateList();
		FinalMappingStateList mySmallestPermutationsList = new FinalMappingStateList();

		public void Add(FactType factType, FactTypeMappingList value)
		{
			myMappingList.Add(factType, value);
		}

		public void Remove(FactType factType)
		{
			myMappingList.Remove(factType);
		}

		public FactTypeMappingListDictionary MappingList
		{
			get { return myMappingList; }
		}

		public FinalMappingStateList PossibleFactTypeMappings
		{
			get { return myPossibleFactTypeMappings; }
		}

		public FinalMappingStateList SmallestPermutationsList
		{
			get { return mySmallestPermutationsList; }
		}
	}
	#endregion
}