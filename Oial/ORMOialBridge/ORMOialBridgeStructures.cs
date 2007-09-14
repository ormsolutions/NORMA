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
			// UNDONE: There might be a better place to clone this list...
			this.factTypes = new List<FactType>(factTypes);
		}
	}

	[Serializable]
	[DebuggerDisplay("FactTypeMapping (TowardsRole={FactType.RoleCollection.IndexOf(TowardsRoleDebug)}, Depth={MappingDepth}, FactType={FactType.Name})")]
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
		private Dictionary<ObjectType, object> myTopLevelConceptTypes;
		private Dictionary<ObjectType, object> myNonTopLevelConceptTypes;

		public Permutation(FactTypeMappingList mappings)
		{
			myMappings = mappings;
		}

		public FactTypeMappingList Mappings
		{
			get { return myMappings; }
		}

		public void SetConceptTypes(Dictionary<ObjectType, object> topLevelConceptTypes, Dictionary<ObjectType, object> nonTopLevelConceptTypes)
		{
			Debug.Assert(myTopLevelConceptTypes == null && myNonTopLevelConceptTypes == null);
			myTopLevelConceptTypes = new Dictionary<ObjectType,object>(topLevelConceptTypes);
			myNonTopLevelConceptTypes = new Dictionary<ObjectType,object>(nonTopLevelConceptTypes);
		}

		public Dictionary<ObjectType, object> TopLevelConceptTypes
		{
			get
			{
				return myTopLevelConceptTypes;
			}
		}

		public Dictionary<ObjectType, object> NonTopLevelConceptTypes
		{
			get
			{
				return myNonTopLevelConceptTypes;
			}
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
		private readonly ObjectTypeList myObjectTypes;
		private readonly FactTypeMappingList myPredecidedManyToOneFactTypeMappings;
		private readonly FactTypeMappingList myPredecidedOneToOneFactTypeMappings;
		private readonly FactTypeMappingListList myUndecidedOneToOneFactTypeMappings;
		private readonly PermutationList myPossiblePermutations;
		private readonly PermutationList mySmallestPermutationsInTermsOfConceptTypes;

		public Chain()
		{
			myObjectTypes = new ObjectTypeList();
			myPredecidedManyToOneFactTypeMappings = new FactTypeMappingList();
			myPredecidedOneToOneFactTypeMappings = new FactTypeMappingList();
			myUndecidedOneToOneFactTypeMappings = new FactTypeMappingListList();
			myPossiblePermutations = new PermutationList();
			mySmallestPermutationsInTermsOfConceptTypes = new PermutationList();
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
		public ObjectTypeList ObjectTypes
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
		public PermutationList PossiblePermutations
		{
			get { return myPossiblePermutations; }
		}

		/// <summary>
		/// The entries from PossiblePermutations which map to the smallest number of top-level concept types and overall concept types.
		/// </summary>
		public PermutationList SmallestPermutationsInTermsOfConceptTypes
		{
			get { return mySmallestPermutationsInTermsOfConceptTypes; }
		}
	}
	#endregion
}