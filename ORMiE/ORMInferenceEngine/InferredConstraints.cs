using System;
using System.Drawing;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ShapeModel;
using ORMSolutions.ORMArchitect.Framework.Diagrams;
using ORMSolutions.ORMArchitect.Framework;
using Microsoft.VisualStudio.Modeling;
using System.Threading;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;

namespace unibz.ORMInferenceEngine
{
	#region NamedElementDictionary and DuplicateNameError integration
	partial class InferredConstraints : INamedElementDictionaryParent, IAlternateElementOwner<SetComparisonConstraint>, IAlternateElementOwner<SetConstraint>, IAlternateElementOwner<FactType>
	{
		#region INamedElementDictionaryParentNode implementation
		[NonSerialized]
		private NamedElementDictionary myInferredConstraintsDictionary;
		/// <summary>
		/// A <see cref="INamedElementDictionary"/> for retrieving any inferred constraint by name.
		/// </summary>
		public INamedElementDictionary InferredConstraintsDictionary
		{
			get
			{
				INamedElementDictionary retVal = myInferredConstraintsDictionary;
				if (retVal == null)
				{
					retVal = myInferredConstraintsDictionary = new InferredConstraintsNamedElementDictionary();
				}
				return retVal;
			}
		}
		INamedElementDictionary INamedElementDictionaryParent.GetCounterpartRoleDictionary(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			if (parentDomainRoleId == SetComparisonConstraintIsInferred.InferredConstraintsDomainRoleId ||
				parentDomainRoleId == SetConstraintIsInferred.InferredConstraintsDomainRoleId)
			{
				return InferredConstraintsDictionary;
			}
			return null;
		}
		object INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			return NamedElementDictionary.BlockDuplicateNamesKey;
		}
		#endregion // INamedElementDictionaryParentNode implementation
		#region Relationship-specific NamedElementDictionary implementations
		#region InferredConstraintsNamedElementDictionary class
		/// <summary>
		/// Dictionary used to set the initial names of constraints and to
		/// generate model validation errors and exceptions for duplicate
		/// element names.
		/// </summary>
		private class InferredConstraintsNamedElementDictionary : NamedElementDictionary
		{
			private sealed class DuplicateNameManager : IDuplicateNameCollectionManager
			{
				#region IDuplicateNameCollectionManager Implementation
				ICollection IDuplicateNameCollectionManager.OnDuplicateElementAdded(ICollection elementCollection, ModelElement element, bool afterTransaction, INotifyElementAdded notifyAdded)
				{
					// Nothing to do, we're blocking duplicate elements from being added for now
					return elementCollection;
				}
				ICollection IDuplicateNameCollectionManager.OnDuplicateElementRemoved(ICollection elementCollection, ModelElement element, bool afterTransaction)
				{
					return elementCollection;
				}
                void IDuplicateNameCollectionManager.AfterCollectionRollback(ICollection collection)
                {
                    // No idea what does it mean and what it is useful for... 
                    //TrackingList trackingList;
                    //if (null != (trackingList = collection as TrackingList))
                    //{
                    //    trackingList.Clear();
                    //    foreach (ElementGrouping grouping in trackingList.NativeCollection)
                    //    {
                    //        trackingList.Add(grouping);
                    //    }
                    //}
                }
                #endregion // IDuplicateNameCollectionManager Implementation
			}
			#region Constructors
			/// <summary>
			/// Default constructor for ConstraintNamedElementDictionary
			/// </summary>
			public InferredConstraintsNamedElementDictionary()
				: base(new DuplicateNameManager())
			{
			}
			#endregion // Constructors
			#region Base overrides
			/// <summary>
			/// Raise an exception with text specific to a name in a model
			/// </summary>
			/// <param name="element">Element we're attempting to name</param>
			/// <param name="requestedName">The in-use requested name</param>
			protected override void ThrowDuplicateNameException(ModelElement element, string requestedName)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The inferred constraint '{0}' is already defined in this model.", requestedName));
			}
			#endregion // Base overrides
		}
		#endregion // InferredConstraintsNamedElementDictionary class
		#endregion // Relationship-specific NamedElementDictionary implementations
		#region IAlternateElementOwner<SetComparisonConstraint> Implementation
		IEnumerable<SetComparisonConstraint> IAlternateElementOwner<SetComparisonConstraint>.OwnedElements
		{
			get
			{
				return this.SetComparisonConstraintCollection;
			}
		}
		bool IAlternateElementOwner<SetComparisonConstraint>.ValidateErrorFor(SetComparisonConstraint element, Type modelErrorType)
		{
			return false;
		}
		DomainClassInfo IAlternateElementOwner<SetComparisonConstraint>.GetOwnedElementClassInfo(Type elementType)
		{
			Type alternateType = null;
			if (elementType == typeof(EqualityConstraint))
			{
				alternateType = typeof(InferredEqualityConstraint);
			}
			else if (elementType == typeof(SubsetConstraint))
			{
				alternateType = typeof(InferredSubsetConstraint);
			}
			else if (elementType == typeof(ExclusionConstraint))
			{
				alternateType = typeof(InferredExclusionConstraint);
			}
			return (alternateType != null) ? Store.DomainDataDirectory.GetDomainClass(alternateType) : null;
		}
		#endregion // IAlternateElementOwner<SetComparisonConstraint> Implementation
		#region IAlternateElementOwner<SetConstraint> Implementation
		IEnumerable<SetConstraint> IAlternateElementOwner<SetConstraint>.OwnedElements
		{
			get
			{
				return this.SetConstraintCollection;
			}
		}
		bool IAlternateElementOwner<SetConstraint>.ValidateErrorFor(SetConstraint element, Type modelErrorType)
		{
			return false;
		}
		DomainClassInfo IAlternateElementOwner<SetConstraint>.GetOwnedElementClassInfo(Type elementType)
		{
			Type alternateType = null;
			if (elementType == typeof(MandatoryConstraint))
			{
				alternateType = typeof(InferredMandatoryConstraint);
			}
			else if (elementType == typeof(UniquenessConstraint))
			{
				alternateType = typeof(InferredUniquenessConstraint);
			}
			else if (elementType == typeof(FrequencyConstraint))
			{
				alternateType = typeof(InferredFrequencyConstraint);
			}
			return (alternateType != null) ? Store.DomainDataDirectory.GetDomainClass(alternateType) : null;
		}
		#endregion // IAlternateElementOwner<SetConstraint> Implementation
		#region IAlternateElementOwner<FactType> Implementation
		IEnumerable<FactType> IAlternateElementOwner<FactType>.OwnedElements
		{
			get
			{
				foreach (FactType factType in this.SubtypeFactCollection)
				{
					yield return factType;
				}
			}
		}
		bool IAlternateElementOwner<FactType>.ValidateErrorFor(FactType element, Type modelErrorType)
		{
			return false;
		}
		DomainClassInfo IAlternateElementOwner<FactType>.GetOwnedElementClassInfo(Type elementType)
		{
			Type alternateType = null;
			if (elementType == typeof(SubtypeFact))
			{
				alternateType = typeof(InferredSubtypeFact);
			}
			return (alternateType != null) ? Store.DomainDataDirectory.GetDomainClass(alternateType) : null;
		}
		#endregion // IAlternateElementOwner<SubtypeFact> Implementation
	}

	partial class SubtypeFactIsInferred : INamedElementDictionaryLink
	{
		#region INamedElementDictionaryLink implementation
		INamedElementDictionaryParentNode INamedElementDictionaryLink.ParentRolePlayer
		{
			get { return ParentRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ParentRolePlayer
		/// Returns Model.
		/// </summary>
		protected INamedElementDictionaryParentNode ParentRolePlayer
		{
			get { return InferredConstraints; }
		}
		INamedElementDictionaryChildNode INamedElementDictionaryLink.ChildRolePlayer
		{
			get { return ChildRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ChildRolePlayer
		/// Returns SetComparisonConstraintCollection.
		/// </summary>
		protected INamedElementDictionaryChildNode ChildRolePlayer
		{
			get { return SubtypeFact; }
		}
        NamedElementDictionaryLinkUse INamedElementDictionaryLink.DictionaryLinkUse
        {
            get
            {
                return DictionaryLinkUse;
            }
        }
        /// <summary>
        /// Implements <see cref="INamedElementDictionaryLink.DictionaryLinkUse"/>.
        /// This link is used both directly for object type names in the model,
        /// and indirectly for value constraint names on value types.
        /// </summary>
        protected static NamedElementDictionaryLinkUse DictionaryLinkUse
        {
            get
            {
                return NamedElementDictionaryLinkUse.DirectDictionary | NamedElementDictionaryLinkUse.DictionaryConnector;
            }
        }
        //INamedElementDictionaryRemoteParent INamedElementDictionaryLink.RemoteParentRolePlayer
        //{
        //    get { return RemoteParentRolePlayer; }
        //}
        ///// <summary>
        ///// Implements INamedElementDictionaryLink.RemoteParentRolePlayer
        ///// Returns null.
        ///// </summary>
        //protected static INamedElementDictionaryRemoteParent RemoteParentRolePlayer
        //{
        //    get { return null; }
        //}
		#endregion // INamedElementDictionaryLink implementation
	}

	partial class SetConstraintIsInferred : INamedElementDictionaryLink
	{
		#region INamedElementDictionaryLink implementation
		INamedElementDictionaryParentNode INamedElementDictionaryLink.ParentRolePlayer
		{
			get { return ParentRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ParentRolePlayer
		/// Returns Model.
		/// </summary>
		protected INamedElementDictionaryParentNode ParentRolePlayer
		{
			get { return InferredConstraints; }
		}
		INamedElementDictionaryChildNode INamedElementDictionaryLink.ChildRolePlayer
		{
			get { return ChildRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ChildRolePlayer
		/// Returns SetComparisonConstraintCollection.
		/// </summary>
		protected INamedElementDictionaryChildNode ChildRolePlayer
		{
			get { return SetConstraint; }
		}
        NamedElementDictionaryLinkUse INamedElementDictionaryLink.DictionaryLinkUse
        {
            get
            {
                return DictionaryLinkUse;
            }
        }
        /// <summary>
        /// Implements <see cref="INamedElementDictionaryLink.DictionaryLinkUse"/>.
        /// This link is used both directly for object type names in the model,
        /// and indirectly for value constraint names on value types.
        /// </summary>
        protected static NamedElementDictionaryLinkUse DictionaryLinkUse
        {
            get
            {
                return NamedElementDictionaryLinkUse.DirectDictionary | NamedElementDictionaryLinkUse.DictionaryConnector;
            }
        }
        //INamedElementDictionaryRemoteParent INamedElementDictionaryLink.RemoteParentRolePlayer
        //{
        //    get { return RemoteParentRolePlayer; }
        //}
        ///// <summary>
        ///// Implements INamedElementDictionaryLink.RemoteParentRolePlayer
        ///// Returns null.
        ///// </summary>
        //protected static INamedElementDictionaryRemoteParent RemoteParentRolePlayer
        //{
        //    get { return null; }
        //}
		#endregion // INamedElementDictionaryLink implementation
	}

	partial class SetComparisonConstraintIsInferred : INamedElementDictionaryLink
	{
		#region INamedElementDictionaryLink implementation
		INamedElementDictionaryParentNode INamedElementDictionaryLink.ParentRolePlayer
		{
			get { return ParentRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ParentRolePlayer
		/// Returns Model.
		/// </summary>
		protected INamedElementDictionaryParentNode ParentRolePlayer
		{
			get { return InferredConstraints; }
		}
		INamedElementDictionaryChildNode INamedElementDictionaryLink.ChildRolePlayer
		{
			get { return ChildRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ChildRolePlayer
		/// Returns SetComparisonConstraintCollection.
		/// </summary>
		protected INamedElementDictionaryChildNode ChildRolePlayer
		{
			get { return SetComparisonConstraint; }
		}
        NamedElementDictionaryLinkUse INamedElementDictionaryLink.DictionaryLinkUse
        {
            get
            {
                return DictionaryLinkUse;
            }
        }
        /// <summary>
        /// Implements <see cref="INamedElementDictionaryLink.DictionaryLinkUse"/>.
        /// This link is used both directly for object type names in the model,
        /// and indirectly for value constraint names on value types.
        /// </summary>
        protected static NamedElementDictionaryLinkUse DictionaryLinkUse
        {
            get
            {
                return NamedElementDictionaryLinkUse.DirectDictionary | NamedElementDictionaryLinkUse.DictionaryConnector;
            }
        }
        //INamedElementDictionaryRemoteParent INamedElementDictionaryLink.RemoteParentRolePlayer
        //{
        //    get { return RemoteParentRolePlayer; }
        //}
        ///// <summary>
        ///// Implements INamedElementDictionaryLink.RemoteParentRolePlayer
        ///// Returns null.
        ///// </summary>
        //protected static INamedElementDictionaryRemoteParent RemoteParentRolePlayer
        //{
        //    get { return null; }
        //}
		#endregion // INamedElementDictionaryLink implementation
	}
	partial class InferredSubsetConstraint : INamedElementDictionaryChild, IHasAlternateOwner<SetComparisonConstraint>
	{
		#region INamedElementDictionaryChild implementation
		void INamedElementDictionaryChild.GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			GetRoleGuids(out parentDomainRoleId, out childDomainRoleId);
		}
		/// <summary>
		/// Implementation of INamedElementDictionaryChild.GetRoleGuids. Identifies
		/// this child as participating in the 'SetComparisonConstraintIsInferred' naming set.
		/// </summary>
		/// <param name="parentDomainRoleId">Guid</param>
		/// <param name="childDomainRoleId">Guid</param>
		protected static new void GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			parentDomainRoleId = SetComparisonConstraintIsInferred.InferredConstraintsDomainRoleId;
			childDomainRoleId = SetComparisonConstraintIsInferred.SetComparisonConstraintDomainRoleId;
		}
		#endregion // INamedElementDictionaryChild implementation
		#region IHasAlternateOwner<SetComparisonConstraint> Implementation
		/// <summary>
		/// Implements <see cref="IHasAlternateOwner{SetComparisonConstraint}.AlternateOwner"/>
		/// </summary>
		protected IAlternateElementOwner<SetComparisonConstraint> AlternateOwner
		{
			get
			{
				return SetComparisonConstraintIsInferred.GetInferredConstraints(this);
			}
			set
			{
				InferredConstraints parent;
				if (null != (parent = value as InferredConstraints) &&
					AlternateOwner == null)
				{
					new SetComparisonConstraintIsInferred(parent, this);
				}
			}
		}
		IAlternateElementOwner<SetComparisonConstraint> IHasAlternateOwner<SetComparisonConstraint>.AlternateOwner
		{
			get
			{
				return AlternateOwner;
			}
			set
			{
				AlternateOwner = value;
			}
		}
		object IHasAlternateOwner.UntypedAlternateOwner
		{
			get
			{
				return AlternateOwner;
			}
		}
		#endregion // IHasAlternateOwner<SetComparisonConstraint> Implementation
	}
	partial class InferredEqualityConstraint : INamedElementDictionaryChild, IHasAlternateOwner<SetComparisonConstraint>
	{
		#region INamedElementDictionaryChild implementation
		void INamedElementDictionaryChild.GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			GetRoleGuids(out parentDomainRoleId, out childDomainRoleId);
		}
		/// <summary>
		/// Implementation of INamedElementDictionaryChild.GetRoleGuids. Identifies
		/// this child as participating in the 'SetComparisonConstraintIsInferred' naming set.
		/// </summary>
		/// <param name="parentDomainRoleId">Guid</param>
		/// <param name="childDomainRoleId">Guid</param>
		protected static new void GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			parentDomainRoleId = SetComparisonConstraintIsInferred.InferredConstraintsDomainRoleId;
			childDomainRoleId = SetComparisonConstraintIsInferred.SetComparisonConstraintDomainRoleId;
		}
		#endregion // INamedElementDictionaryChild implementation
		#region IHasAlternateOwner<SetComparisonConstraint> Implementation
		/// <summary>
		/// Implements <see cref="IHasAlternateOwner{SetComparisonConstraint}.AlternateOwner"/>
		/// </summary>
		protected IAlternateElementOwner<SetComparisonConstraint> AlternateOwner
		{
			get
			{
				return SetComparisonConstraintIsInferred.GetInferredConstraints(this);
			}
			set
			{
				InferredConstraints parent;
				if (null != (parent = value as InferredConstraints) &&
					AlternateOwner == null)
				{
					new SetComparisonConstraintIsInferred(parent, this);
				}
			}
		}
		IAlternateElementOwner<SetComparisonConstraint> IHasAlternateOwner<SetComparisonConstraint>.AlternateOwner
		{
			get
			{
				return AlternateOwner;
			}
			set
			{
				AlternateOwner = value;
			}
		}
		object IHasAlternateOwner.UntypedAlternateOwner
		{
			get
			{
				return AlternateOwner;
			}
		}
		#endregion // IHasAlternateOwner<SetComparisonConstraint> Implementation
	}
	partial class InferredExclusionConstraint : INamedElementDictionaryChild, IHasAlternateOwner<SetComparisonConstraint>
	{
		#region INamedElementDictionaryChild implementation
		void INamedElementDictionaryChild.GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			GetRoleGuids(out parentDomainRoleId, out childDomainRoleId);
		}
		/// <summary>
		/// Implementation of INamedElementDictionaryChild.GetRoleGuids. Identifies
		/// this child as participating in the 'SetComparisonConstraintIsInferred' naming set.
		/// </summary>
		/// <param name="parentDomainRoleId">Guid</param>
		/// <param name="childDomainRoleId">Guid</param>
		protected static new void GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			parentDomainRoleId = SetComparisonConstraintIsInferred.InferredConstraintsDomainRoleId;
			childDomainRoleId = SetComparisonConstraintIsInferred.SetComparisonConstraintDomainRoleId;
		}
		#endregion // INamedElementDictionaryChild implementation
		#region IHasAlternateOwner<SetComparisonConstraint> Implementation
		/// <summary>
		/// Implements <see cref="IHasAlternateOwner{SetComparisonConstraint}.AlternateOwner"/>
		/// </summary>
		protected IAlternateElementOwner<SetComparisonConstraint> AlternateOwner
		{
			get
			{
				return SetComparisonConstraintIsInferred.GetInferredConstraints(this);
			}
			set
			{
				InferredConstraints parent;
				if (null != (parent = value as InferredConstraints) &&
					AlternateOwner == null)
				{
					new SetComparisonConstraintIsInferred(parent, this);
				}
			}
		}
		IAlternateElementOwner<SetComparisonConstraint> IHasAlternateOwner<SetComparisonConstraint>.AlternateOwner
		{
			get
			{
				return AlternateOwner;
			}
			set
			{
				AlternateOwner = value;
			}
		}
		object IHasAlternateOwner.UntypedAlternateOwner
		{
			get
			{
				return AlternateOwner;
			}
		}
		#endregion // IHasAlternateOwner<SetComparisonConstraint> Implementation
	}

	partial class InferredMandatoryConstraint : INamedElementDictionaryChild, IHasAlternateOwner<SetConstraint>
	{
		#region INamedElementDictionaryChild implementation
		void INamedElementDictionaryChild.GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			GetRoleGuids(out parentDomainRoleId, out childDomainRoleId);
		}
		/// <summary>
		/// Implementation of INamedElementDictionaryChild.GetRoleGuids. Identifies
		/// this child as participating in the 'SetConstraintIsInferred' naming set.
		/// </summary>
		/// <param name="parentDomainRoleId">Guid</param>
		/// <param name="childDomainRoleId">Guid</param>
		protected static new void GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			parentDomainRoleId = SetConstraintIsInferred.InferredConstraintsDomainRoleId;
			childDomainRoleId = SetConstraintIsInferred.SetConstraintDomainRoleId;
		}
		#endregion // INamedElementDictionaryChild implementation
		#region IHasAlternateOwner<SetConstraint> Implementation
		/// <summary>
		/// Implements <see cref="IHasAlternateOwner{SetConstraint}.AlternateOwner"/>
		/// </summary>
		protected IAlternateElementOwner<SetConstraint> AlternateOwner
		{
			get
			{
				return SetConstraintIsInferred.GetInferredConstraints(this);
			}
			set
			{
				InferredConstraints parent;
				if (null != (parent = value as InferredConstraints) &&
					AlternateOwner == null)
				{
					new SetConstraintIsInferred(parent, this);
				}
			}
		}
		IAlternateElementOwner<SetConstraint> IHasAlternateOwner<SetConstraint>.AlternateOwner
		{
			get
			{
				return AlternateOwner;
			}
			set
			{
				AlternateOwner = value;
			}
		}
		object IHasAlternateOwner.UntypedAlternateOwner
		{
			get
			{
				return AlternateOwner;
			}
		}
		#endregion // IHasAlternateOwner<SetConstraint> Implementation
	}
	partial class InferredUniquenessConstraint : INamedElementDictionaryChild, IHasAlternateOwner<SetConstraint>
	{
		#region INamedElementDictionaryChild implementation
		void INamedElementDictionaryChild.GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			GetRoleGuids(out parentDomainRoleId, out childDomainRoleId);
		}
		/// <summary>
		/// Implementation of INamedElementDictionaryChild.GetRoleGuids. Identifies
		/// this child as participating in the 'SetConstraintIsInferred' naming set.
		/// </summary>
		/// <param name="parentDomainRoleId">Guid</param>
		/// <param name="childDomainRoleId">Guid</param>
		protected static new void GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			parentDomainRoleId = SetConstraintIsInferred.InferredConstraintsDomainRoleId;
			childDomainRoleId = SetConstraintIsInferred.SetConstraintDomainRoleId;
		}
		#endregion // INamedElementDictionaryChild implementation
		#region IHasAlternateOwner<SetConstraint> Implementation
		/// <summary>
		/// Implements <see cref="IHasAlternateOwner{SetConstraint}.AlternateOwner"/>
		/// </summary>
		protected IAlternateElementOwner<SetConstraint> AlternateOwner
		{
			get
			{
				return SetConstraintIsInferred.GetInferredConstraints(this);
			}
			set
			{
				InferredConstraints parent;
				if (null != (parent = value as InferredConstraints) &&
					AlternateOwner == null)
				{
					new SetConstraintIsInferred(parent, this);
				}
			}
		}
		IAlternateElementOwner<SetConstraint> IHasAlternateOwner<SetConstraint>.AlternateOwner
		{
			get
			{
				return AlternateOwner;
			}
			set
			{
				AlternateOwner = value;
			}
		}
		object IHasAlternateOwner.UntypedAlternateOwner
		{
			get
			{
				return AlternateOwner;
			}
		}
		#endregion // IHasAlternateOwner<SetConstraint> Implementation
	}
	partial class InferredFrequencyConstraint : INamedElementDictionaryChild, IHasAlternateOwner<SetConstraint>
	{
		#region INamedElementDictionaryChild implementation
		void INamedElementDictionaryChild.GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			GetRoleGuids(out parentDomainRoleId, out childDomainRoleId);
		}
		/// <summary>
		/// Implementation of INamedElementDictionaryChild.GetRoleGuids. Identifies
		/// this child as participating in the 'SetConstraintIsInferred' naming set.
		/// </summary>
		/// <param name="parentDomainRoleId">Guid</param>
		/// <param name="childDomainRoleId">Guid</param>
		protected static new void GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			parentDomainRoleId = SetConstraintIsInferred.InferredConstraintsDomainRoleId;
			childDomainRoleId = SetConstraintIsInferred.SetConstraintDomainRoleId;
		}
		#endregion // INamedElementDictionaryChild implementation
		#region IHasAlternateOwner<SetConstraint> Implementation
		/// <summary>
		/// Implements <see cref="IHasAlternateOwner{SetConstraint}.AlternateOwner"/>
		/// </summary>
		protected IAlternateElementOwner<SetConstraint> AlternateOwner
		{
			get
			{
				return SetConstraintIsInferred.GetInferredConstraints(this);
			}
			set
			{
				InferredConstraints parent;
				if (null != (parent = value as InferredConstraints) &&
					AlternateOwner == null)
				{
					new SetConstraintIsInferred(parent, this);
				}
			}
		}
		IAlternateElementOwner<SetConstraint> IHasAlternateOwner<SetConstraint>.AlternateOwner
		{
			get
			{
				return AlternateOwner;
			}
			set
			{
				AlternateOwner = value;
			}
		}
		object IHasAlternateOwner.UntypedAlternateOwner
		{
			get
			{
				return AlternateOwner;
			}
		}
		#endregion // IHasAlternateOwner<SetConstraint> Implementation
	}

	partial class InferredSubtypeFact : INamedElementDictionaryChild, IHasAlternateOwner<FactType>
	{
		#region INamedElementDictionaryChild implementation
		void INamedElementDictionaryChild.GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			GetRoleGuids(out parentDomainRoleId, out childDomainRoleId);
		}
		/// <summary>
		/// Implementation of INamedElementDictionaryChild.GetRoleGuids. Identifies
		/// this child as participating in the 'SetConstraintIsInferred' naming set.
		/// </summary>
		/// <param name="parentDomainRoleId">Guid</param>
		/// <param name="childDomainRoleId">Guid</param>
		protected static new void GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			parentDomainRoleId = SubtypeFactIsInferred.InferredConstraintsDomainRoleId;
			childDomainRoleId = SubtypeFactIsInferred.SubtypeFactDomainRoleId;
		}
		#endregion // INamedElementDictionaryChild implementation
		#region IHasAlternateOwner<FactType> Implementation
		/// <summary>
		/// Implements <see cref="IHasAlternateOwner{FactType}.AlternateOwner"/>
		/// </summary>
		protected IAlternateElementOwner<FactType> AlternateOwner
		{
			get
			{
				return SubtypeFactIsInferred.GetInferredConstraints(this);
			}
			set
			{
				InferredConstraints parent;
				if (null != (parent = value as InferredConstraints) &&
					AlternateOwner == null)
				{
					new SubtypeFactIsInferred(parent, this);
				}
			}
		}
		IAlternateElementOwner<FactType> IHasAlternateOwner<FactType>.AlternateOwner
		{
			get
			{
				return AlternateOwner;
			}
			set
			{
				AlternateOwner = value;
			}
		}
		object IHasAlternateOwner.UntypedAlternateOwner
		{
			get
			{
				return AlternateOwner;
			}
		}
		#endregion // IHasAlternateOwner<SetConstraint> Implementation
		#region Rule Methods
		/// <summary>
		/// AddRule: typeof(SubtypeFactIsInferred), Priority=-1;
		/// </summary>
		private static void InitializeInferredSubtypeFact(ElementAddedEventArgs e)
		{
			((SubtypeFactIsInferred)e.ModelElement).SubtypeFact.InitializeIdentityFactType();
		}
		#endregion // Rule Methods
	}

	// UNDONE: Duplicate INamedElementDictionaryChild for other override inferred constraint types (subtypefact???)
	#endregion // NamedElementDictionary integration
}