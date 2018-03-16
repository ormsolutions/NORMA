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
    partial class InferredUnsatisfiableDomain : INamedElementDictionaryParent, IAlternateElementOwner<ObjectType>, IAlternateElementOwner<FactType>
	{
		#region INamedElementDictionaryParentNode implementation
		[NonSerialized]
		private NamedElementDictionary myUnsatisfiableElementsDictionary;
		/// <summary>
		/// A <see cref="INamedElementDictionary"/> for retrieving any unsatisfiable element by name.
		/// </summary>
		public INamedElementDictionary UnsatisfiableElementsDictionary
		{
			get
			{
				INamedElementDictionary retVal = myUnsatisfiableElementsDictionary;
				if (retVal == null)
				{
					retVal = myUnsatisfiableElementsDictionary = new UnsatisfiableElementsNamedElementDictionary();
				}
				return retVal;
			}
		}
		INamedElementDictionary INamedElementDictionaryParent.GetCounterpartRoleDictionary(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			if (parentDomainRoleId == UnsatisfiableFactType.FactTypeDomainRoleId ||
				parentDomainRoleId == UnsatisfiableObjectType.ObjectTypeDomainRoleId)
			{
				return UnsatisfiableElementsDictionary;
			}
			return null;
		}
		object INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			return NamedElementDictionary.BlockDuplicateNamesKey;
		}
		#endregion // INamedElementDictionaryParentNode implementation
		#region Relationship-specific NamedElementDictionary implementations
		#region UnsatisfiableElementsNamedElementDictionary class
		/// <summary>
		/// Dictionary used to set the initial names of constraints and to
		/// generate model validation errors and exceptions for duplicate
		/// element names.
		/// </summary>
		private class UnsatisfiableElementsNamedElementDictionary : NamedElementDictionary
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
			public UnsatisfiableElementsNamedElementDictionary()
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
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The inferred unsatisfiable domain '{0}' is already defined in this model.", requestedName));
			}
			#endregion // Base overrides
		}
		#endregion // InferredElementsNamedElementDictionary class
		#endregion // Relationship-specific NamedElementDictionary implementations
		#region IAlternateElementOwner<ObjectType> Implementation
		IEnumerable<ObjectType> IAlternateElementOwner<ObjectType>.OwnedElements
		{
			get
			{
				return this.ObjectTypeCollection;
			}
		}
		bool IAlternateElementOwner<ObjectType>.ValidateErrorFor(ObjectType element, Type modelErrorType)
		{
			return false;
		}
		DomainClassInfo IAlternateElementOwner<ObjectType>.GetOwnedElementClassInfo(Type elementType)
		{
			Type alternateType = null;
			if (elementType == typeof(ObjectType))
			{
				alternateType = typeof(UnsatisfiableObjectType);
			}
			return (alternateType != null) ? Store.DomainDataDirectory.GetDomainClass(alternateType) : null;
		}
		#endregion // IAlternateElementOwner<SetComparisonConstraint> Implementation
		#region IAlternateElementOwner<FactType> Implementation
		IEnumerable<FactType> IAlternateElementOwner<FactType>.OwnedElements
		{
			get
			{
				return this.FactTypeCollection;
			}
		}
		bool IAlternateElementOwner<FactType>.ValidateErrorFor(FactType element, Type modelErrorType)
		{
			return false;
		}
		DomainClassInfo IAlternateElementOwner<FactType>.GetOwnedElementClassInfo(Type elementType)
		{
			Type alternateType = null;
			if (elementType == typeof(FactType))
			{
				alternateType = typeof(UnsatisfiableFactType);
			}
			return (alternateType != null) ? Store.DomainDataDirectory.GetDomainClass(alternateType) : null;
		}
		#endregion // IAlternateElementOwner<FactType> Implementation
	}

	partial class UnsatisfiableFactType : INamedElementDictionaryLink
	{
		#region INamedElementDictionaryLink implementation
		/// <summary>
        /// Implements <see cref="INamedElementDictionaryLink.ParentRolePlayer"/>
        /// Returns the associated <see cref="p:Model"/>.
		/// </summary>
		INamedElementDictionaryParentNode INamedElementDictionaryLink.ParentRolePlayer
		{
			get { return InferredUnsatisfiableDomain; }
		}
		/// <summary>
        /// Implements <see cref="INamedElementDictionaryLink.ChildRolePlayer"/>
		/// Returns FactType.
		/// </summary>
        INamedElementDictionaryChildNode INamedElementDictionaryLink.ChildRolePlayer
		{
			get { return FactType; }
		}
        /// <summary>
		/// Implements <see cref="INamedElementDictionaryLink.DictionaryLinkUse"/>.
		/// This link is used both directly for object type names in the model,
		/// and indirectly for value constraint names on value types.
		/// </summary>
        NamedElementDictionaryLinkUse INamedElementDictionaryLink.DictionaryLinkUse
        {
            get
            {
                return NamedElementDictionaryLinkUse.DirectDictionary | NamedElementDictionaryLinkUse.DictionaryConnector;
            }
        }
		#endregion // INamedElementDictionaryLink implementation
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
		//#endregion // INamedElementDictionaryLink implementation
	}

	partial class UnsatisfiableObjectType : INamedElementDictionaryLink
	{
		#region INamedElementDictionaryLink implementation
		/// <summary>
		/// Implements INamedElementDictionaryLink.ParentRolePlayer
		/// Returns Model.
		/// </summary>
		INamedElementDictionaryParentNode INamedElementDictionaryLink.ParentRolePlayer
		{
			get { return InferredUnsatisfiableDomain; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ChildRolePlayer
		/// Returns SetComparisonConstraintCollection.
		/// </summary>
		INamedElementDictionaryChildNode INamedElementDictionaryLink.ChildRolePlayer
		{
			get { return ObjectType; }
		}
        /// <summary>
        /// Implements <see cref="INamedElementDictionaryLink.DictionaryLinkUse"/>.
        /// This link is used both directly for object type names in the model,
        /// and indirectly for value constraint names on value types.
        /// </summary>
        NamedElementDictionaryLinkUse INamedElementDictionaryLink.DictionaryLinkUse
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

	// UNDONE: Duplicate INamedElementDictionaryChild for other override inferred constraint types (subtypefact???)
	#endregion // NamedElementDictionary integration
}