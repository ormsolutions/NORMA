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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.Modeling;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region ORMDeserializationFixupPhase enum
	/// <summary>
	/// Fixup stages supported during ORM deserialization
	/// </summary>
	public enum ORMDeserializationFixupPhase
	{
		/// <summary>
		/// Add any intrinsic elements at this stage. Intrinsic elements
		/// are not serialized but are always present in the model. For example,
		/// intrinsic data types or intrinsic reference modes.
		/// </summary>
		AddIntrinsicElements = 100,
		/// <summary>
		/// Verify that any implied elements that are serialized with the model
		/// but must follow a proscribed pattern based on another serialized element.
		/// This stage may both add and remove elements.
		/// </summary>
		ValidateImplicitStoredElements = 200,
		/// <summary>
		/// Add implicit elements at this stage. An implicit element is
		/// not serialized and is generally created by a rule once the model
		/// is loaded.
		/// </summary>
		AddImplicitElements = 300,
		/// <summary>
		/// Element names should be tracked and validated after
		/// all intrinsic, implicitstored, and implicit elements are in place.
		/// </summary>
		ValidateElementNames = 400,
		/// <summary>
		/// Model errors are stored with the model, but are vulnerable
		/// to the Notepad effect, which can cause errors to be added
		/// or removed from the model. Validate errors after all other
		/// explicit, intrinsic, and implicit elements are in place.
		/// </summary>
		ValidateErrors = 500,
		/// <summary>
		/// Add any presentation elements that are implicit and not
		/// serialized with the model.
		/// </summary>
		AddImplicitPresentationElements = 600,
		/// <summary>
		/// Remove any orphaned presentation elements, meaning any
		/// PresentationElement where the ModelElement role property is null.
		/// Orphaned pels are currently not supported.
		/// </summary>
		RemoveOrphanedPresentationElements = 700,
	}
	#endregion // ORMDeserializationFixupPhase enum
	public partial class ORMModel
	{
		/// <summary>
		/// Used as the value for <see cref="ElementGroup.UserData"/> to indicate that the
		/// <see cref="ObjectType"/> should be a ValueType.
		/// </summary>
		public static readonly object ValueTypeUserDataKey = new object();
		/// <summary>
		/// Used as the value for <see cref="ElementGroup.UserData"/> to indicate that the
		/// <see cref="UniquenessConstraint"/> is internal.
		/// </summary>
		public static readonly object InternalUniquenessConstraintUserDataKey = new object();

		#region Entity- and ValueType specific collections
		/// <summary>
		/// All of the entity types in the object types collection.
		/// </summary>
		[CLSCompliant(false)]
		public IEnumerable<ObjectType> EntityTypeCollection
		{
			get
			{
				return RestrictedObjectTypeCollection(false);
			}
		}
		/// <summary>
		/// All of the value types in the object types collection.
		/// </summary>
		public IEnumerable<ObjectType> ValueTypeCollection
		{
			get
			{
				return RestrictedObjectTypeCollection(true);
			}
		}
		private IEnumerable<ObjectType> RestrictedObjectTypeCollection(bool valueType)
		{
			foreach (ObjectType obj in ObjectTypeCollection)
			{
				if (obj.IsValueType == valueType)
				{
					yield return obj;
				}
			}
		}
		#endregion // Entity- and ValueType specific collections
		#region ErrorCollection
		#region ErrorCollection's Generated Accessor Code
		/// <summary>
		/// The ErrorCollection
		/// </summary>
		public IEnumerable<ModelError> ErrorCollection
		{
			get
			{
				foreach (ModelError modelError in ModelHasError.GetErrorCollection(this))
				{
					yield return modelError;
				}
				foreach (ModelError modelError in base.GetErrorCollection(ModelErrorUses.None))
				{
					yield return modelError;
				}
			}
		}
		#endregion
		#endregion // ErrorCollection
		#region MergeContext functions
		private void MergeRelateObjectType(ModelElement sourceElement, ElementGroup elementGroup)
		{
			ObjectType objectType = sourceElement as ObjectType;
			if (elementGroup.UserData == ORMModel.ValueTypeUserDataKey)
			{
				objectType.DataType = DefaultDataType;
			}
			this.ObjectTypeCollection.Add(objectType);
		}
		private void MergeDisconnectObjectType(ModelElement sourceElement)
		{
			ObjectType objectType = sourceElement as ObjectType;
			// Delete link for path ModelHasObjectType.ObjectTypeCollection
			foreach (ElementLink link in ModelHasObjectType.GetLinks(this, objectType))
			{
				// Delete the link, but without possible delete propagation to the element since it's moving to a new location.
				link.Delete(ModelHasObjectType.ModelDomainRoleId, ModelHasObjectType.ObjectTypeDomainRoleId);
			}
		}
		private bool CanMergeSetConstraint(ProtoElementBase rootElement, ElementGroupPrototype elementGroupPrototype)
		{
			return elementGroupPrototype.UserData != ORMModel.InternalUniquenessConstraintUserDataKey;
		}
		#endregion // MergeContext functions
	}
	#region NamedElementDictionary and DuplicateNameError integration
	public partial class ORMModel : INamedElementDictionaryParent
	{
		#region INamedElementDictionaryParent implementation
		[NonSerialized]
		private NamedElementDictionary myObjectTypesDictionary;
		[NonSerialized]
		private NamedElementDictionary myConstraintsDictionary;
		/// <summary>
		/// Returns the Object Types Dictionary
		/// </summary>
		/// <value>The model ObjectTypesDictionary </value>
		public INamedElementDictionary ObjectTypesDictionary
		{
			get
			{
				INamedElementDictionary retVal = myObjectTypesDictionary;
				if (retVal == null)
				{
					retVal = myObjectTypesDictionary = new ObjectTypeNamedElementDictionary();
				}
				return retVal;
			}
		}
		/// <summary>
		/// Returns the Constraints Dictionary
		/// </summary>
		/// <value>The model ConstraintsDictionary.</value>
		public INamedElementDictionary ConstraintsDictionary
		{
			get
			{
				INamedElementDictionary retVal = myConstraintsDictionary;
				if (retVal == null)
				{
					retVal = myConstraintsDictionary = new ConstraintNamedElementDictionary();
				}
				return retVal;
			}
		}

		INamedElementDictionary INamedElementDictionaryParent.GetCounterpartRoleDictionary(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			return GetCounterpartRoleDictionary(parentDomainRoleId, childDomainRoleId);
		}
		/// <summary>
		/// Implements INamedElementDictionaryParent.GetCounterpartRoleDictionary
		/// </summary>
		/// <param name="parentDomainRoleId">Guid</param>
		/// <param name="childDomainRoleId">Guid</param>
		/// <returns>Dictionaries for object types, fact types, and constraints</returns>
		public INamedElementDictionary GetCounterpartRoleDictionary(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			if (parentDomainRoleId == ModelHasObjectType.ModelDomainRoleId)
			{
				return ObjectTypesDictionary;
			}
			else if (parentDomainRoleId == ModelHasSetComparisonConstraint.ModelDomainRoleId ||
					 parentDomainRoleId == ModelHasSetConstraint.ModelDomainRoleId ||
					 parentDomainRoleId == ValueTypeHasValueConstraint.ValueTypeDomainRoleId ||
					 parentDomainRoleId == RoleHasValueConstraint.RoleDomainRoleId)
			{
				return ConstraintsDictionary;
			}
			return null;
		}
		object INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			return GetAllowDuplicateNamesContextKey(parentDomainRoleId, childDomainRoleId);
		}
		/// <summary>
		/// Implements INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey
		/// </summary>
		protected object GetAllowDuplicateNamesContextKey(Guid parentDomainRoleId, Guid childDomainRoleId)
		{
			object retVal = null;
			Dictionary<object, object> contextInfo = Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
			if (!contextInfo.ContainsKey(NamedElementDictionary.DefaultAllowDuplicateNamesKey) &&
				contextInfo.ContainsKey(ObjectType.AllowDuplicateObjectNamesKey))
			{
				// Use to their value so they don't have to look up ours again
				retVal = NamedElementDictionary.AllowDuplicateNamesKey;
			}
			return retVal;
		}
		#endregion // INamedElementDictionaryParent implementation
		#region Rules to remove duplicate name errors
		[RuleOn(typeof(ObjectTypeHasDuplicateNameError))] // DeleteRule
		private sealed partial class RemoveDuplicateObjectTypeNameErrorRule : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				ObjectTypeHasDuplicateNameError link = e.ModelElement as ObjectTypeHasDuplicateNameError;
				ObjectTypeDuplicateNameError error = link.DuplicateNameError;
				if (!error.IsDeleted)
				{
					if (error.ObjectTypeCollection.Count < 2)
					{
						error.Delete();
					}
				}
			}
		}
		[RuleOn(typeof(SetComparisonConstraintHasDuplicateNameError)), RuleOn(typeof(SetConstraintHasDuplicateNameError)), RuleOn(typeof(ValueConstraintHasDuplicateNameError))] // DeleteRule
		private sealed partial class RemoveDuplicateConstraintNameErrorRule : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				ModelElement link = e.ModelElement;
				SetComparisonConstraintHasDuplicateNameError mcLink;
				SetConstraintHasDuplicateNameError scLink;
				ValueConstraintHasDuplicateNameError vLink;
				ConstraintDuplicateNameError error = null;
				if (null != (mcLink = link as SetComparisonConstraintHasDuplicateNameError))
				{
					error = mcLink.DuplicateNameError;
				}
				else if (null != (scLink = link as SetConstraintHasDuplicateNameError))
				{
					error = scLink.DuplicateNameError;
				}
				else if (null != (vLink = link as ValueConstraintHasDuplicateNameError))
				{
					error = vLink.DuplicateNameError;
				}
				if (error != null && !error.IsDeleted)
				{
					if ((error.SetComparisonConstraintCollection.Count + error.SetConstraintCollection.Count + error.ValueConstraintCollection.Count) < 2)
					{
						error.Delete();
					}
				}
			}
		}
		#endregion // Rules to remove duplicate name errors
		#region Relationship-specific NamedElementDictionary implementations
		#region ObjectTypeNamedElementDictionary class
		/// <summary>
		/// Dictionary used to set the initial names of object and value types and to
		/// generate model validation errors and exceptions for duplicate
		/// element names.
		/// </summary>
		protected class ObjectTypeNamedElementDictionary : NamedElementDictionary
		{
			private sealed class DuplicateNameManager : IDuplicateNameCollectionManager
			{
				#region TrackingList class
				private sealed class TrackingList : List<ObjectType>
				{
					private readonly LinkedElementCollection<ObjectType> myNativeCollection;
					public TrackingList(ObjectTypeDuplicateNameError error)
					{
						myNativeCollection = error.ObjectTypeCollection;
					}
					public LinkedElementCollection<ObjectType> NativeCollection
					{
						get
						{
							return myNativeCollection;
						}
					}
				}
				#endregion // TrackingList class
				#region IDuplicateNameCollectionManager Implementation
				ICollection IDuplicateNameCollectionManager.OnDuplicateElementAdded(ICollection elementCollection, ModelElement element, bool afterTransaction, INotifyElementAdded notifyAdded)
				{
					ObjectType objectType = (ObjectType)element;
					if (afterTransaction)
					{
						if (elementCollection == null)
						{
							ObjectTypeDuplicateNameError error = objectType.DuplicateNameError;
							if (error != null)
							{
								// We're not in a transaction, but the object model will be in
								// the state we need it because we put it there during a transaction.
								// Just return the collection from the current state of the object model.
								TrackingList trackingList = new TrackingList(error);
								trackingList.Add(objectType);
								elementCollection = trackingList;
							}
						}
						else
						{
							((TrackingList)elementCollection).Add(objectType);
						}
						return elementCollection;
					}
					else
					{
						// Modify the object model to add the error.
						if (elementCollection == null)
						{
							ObjectTypeDuplicateNameError error = null;
							if (notifyAdded != null)
							{
								// During deserialization fixup, an error
								// may already be attached to the object. Track
								// it down and verify that it is a legitimate error.
								// If it is not legitimate, then generate a new one.
								error = objectType.DuplicateNameError;
								if (error != null && !error.ValidateDuplicates(objectType))
								{
									error = null;
								}
							}
							if (error == null)
							{
								error = new ObjectTypeDuplicateNameError(objectType.Store);
								objectType.DuplicateNameError = error;
								error.Model = objectType.Model;
								error.GenerateErrorText();
								if (notifyAdded != null)
								{
									notifyAdded.ElementAdded(error, true);
								}
							}
							TrackingList trackingList = new TrackingList(error);
							trackingList.Add(objectType);
							elementCollection = trackingList;
						}
						else
						{
							TrackingList trackingList = (TrackingList)elementCollection;
							trackingList.Add(objectType);
							// During deserialization fixup (notifyAdded != null), we need
							// to make sure that the element is not already in the collection
							LinkedElementCollection<ObjectType> typedCollection = trackingList.NativeCollection;
							if (notifyAdded == null || !typedCollection.Contains(objectType))
							{
								typedCollection.Add(objectType);
							}
						}
						return elementCollection;
					}
				}
				ICollection IDuplicateNameCollectionManager.OnDuplicateElementRemoved(ICollection elementCollection, ModelElement element, bool afterTransaction)
				{
					TrackingList trackingList = (TrackingList)elementCollection;
					ObjectType objectType = (ObjectType)element;
					trackingList.Remove(objectType);
					if (!afterTransaction)
					{
						// Just clear the error. A rule is used to remove the error
						// object itself when there is no longer a duplicate.
						objectType.DuplicateNameError = null;
					}
					return elementCollection;
				}
				#endregion // IDuplicateNameCollectionManager Implementation
			}
			#region Constructors
			/// <summary>
			/// Default constructor for ObjectTypeNamedElementDictionary
			/// </summary>
			public ObjectTypeNamedElementDictionary() : base(new DuplicateNameManager())
			{
			}
			#endregion // Constructors
			#region Base overrides
			/// <summary>
			/// Provide different base names for entity types and value types
			/// </summary>
			/// <param name="element">The element to test</param>
			/// <returns>A base name string pattern</returns>
			protected override string GetRootNamePattern(ModelElement element)
			{
				return ((ObjectType)element).IsValueType ? ResourceStrings.ValueTypeDefaultNamePattern : ResourceStrings.EntityTypeDefaultNamePattern;
			}
			/// <summary>
			/// Return a default name and allow duplicates for auto-generated names on objectifying types
			/// </summary>
			protected override string GetDefaultName(ModelElement element)
			{
				ObjectType objectType = (ObjectType)element;
				Objectification objectificationLink;
				FactType nestedFact;
				if (null != (objectificationLink = objectType.Objectification) &&
					null != (nestedFact = objectificationLink.NestedFactType))
				{
					return nestedFact.GeneratedName;
				}
				return null;
			}
			/// <summary>
			/// Raise an exception with text specific to a name in a model
			/// </summary>
			/// <param name="element">Element we're attempting to name</param>
			/// <param name="requestedName">The in-use requested name</param>
			protected override void ThrowDuplicateNameException(ModelElement element, string requestedName)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelExceptionNameAlreadyUsedByModel, requestedName));
			}
			#endregion // Base overrides
		}
		#endregion // ObjectTypeNamedElementDictionary class
		#region ConstraintNamedElementDictionary class
		/// <summary>
		/// Dictionary used to set the initial names of constraints and to
		/// generate model validation errors and exceptions for duplicate
		/// element names.
		/// </summary>
		protected class ConstraintNamedElementDictionary : NamedElementDictionary
		{
			private sealed class DuplicateNameManager : IDuplicateNameCollectionManager
			{
				#region TrackingList class
				private sealed class TrackingList : List<ModelElement>
				{
					private readonly LinkedElementCollection<SetComparisonConstraint> myNativeMCCollection;
					private readonly LinkedElementCollection<SetConstraint> myNativeSCCollection;
					private readonly LinkedElementCollection<ValueConstraint> myNativeVCCollection;
					public TrackingList(ConstraintDuplicateNameError error)
					{
						myNativeMCCollection = error.SetComparisonConstraintCollection;
						myNativeSCCollection = error.SetConstraintCollection;
						myNativeVCCollection = error.ValueConstraintCollection;
					}
					public LinkedElementCollection<SetComparisonConstraint> NativeMultiColumnCollection
					{
						get
						{
							return myNativeMCCollection;
						}
					}
					public LinkedElementCollection<SetConstraint> NativeSingleColumnCollection
					{
						get
						{
							return myNativeSCCollection;
						}
					}
					public LinkedElementCollection<ValueConstraint> NativeValueCollection
					{
						get
						{
							return myNativeVCCollection;
						}
					}
				}
				#endregion // TrackingList class
				#region IDuplicateNameCollectionManager Implementation
				ICollection IDuplicateNameCollectionManager.OnDuplicateElementAdded(ICollection elementCollection, ModelElement element, bool afterTransaction, INotifyElementAdded notifyAdded)
				{
					ORMNamedElement namedElement = (ORMNamedElement)element;
					SetConstraint scConstraint = null;
					SetComparisonConstraint mcConstraint = null;
					ValueConstraint vConstraint = null;
					ConstraintDuplicateNameError existingError = null;
					if (null != (scConstraint = element as SetConstraint))
					{
						existingError = scConstraint.DuplicateNameError;
					}
					else if (null != (mcConstraint = element as SetComparisonConstraint))
					{
						existingError = mcConstraint.DuplicateNameError;
					}
					else if (null != (vConstraint = element as ValueConstraint))
					{
						existingError = vConstraint.DuplicateNameError;
					}
					Debug.Assert(scConstraint != null || mcConstraint != null || vConstraint != null);
					if (afterTransaction)
					{
						if (elementCollection == null)
						{
							if (existingError != null)
							{
								// We're not in a transaction, but the object model will be in
								// the state we need it because we put it there during a transaction.
								// Just return the collection from the current state of the object model.
								TrackingList trackingList = new TrackingList(existingError);
								trackingList.Add(element);
								elementCollection = trackingList;
							}
						}
						else
						{
							((TrackingList)elementCollection).Add(element);
						}
						return elementCollection;
					}
					else
					{
						// Modify the object model to add the error.
						if (elementCollection == null)
						{
							ConstraintDuplicateNameError error = null;
							if (notifyAdded != null)
							{
								// During deserialization fixup, an error
								// may already be attached to the object. Track
								// it down and verify that it is a legitimate error.
								// If it is not legitimate, then generate a new one.
								error = existingError;
								if (error != null && !error.ValidateDuplicates(namedElement))
								{
									error = null;
								}
							}
							if (error == null)
							{
								error = new ConstraintDuplicateNameError(element.Store);
								if (scConstraint != null)
								{
									scConstraint.DuplicateNameError = error;
									error.Model = scConstraint.Model;
								}
								else if (mcConstraint != null)
								{
									mcConstraint.DuplicateNameError = error;
									error.Model = mcConstraint.Model;
								}
								else if (vConstraint != null)
								{
									vConstraint.DuplicateNameError = error;
									ValueTypeValueConstraint vTypeValue;
									RoleValueConstraint roleValue;
									if (null != (vTypeValue = vConstraint as ValueTypeValueConstraint))
									{
										error.Model = vTypeValue.ValueType.Model;
									}
									else if (null != (roleValue = vConstraint as RoleValueConstraint))
									{
										error.Model = roleValue.Role.FactType.Model;
									}
								}
								error.GenerateErrorText();
								if (notifyAdded != null)
								{
									notifyAdded.ElementAdded(error, true);
								}
							}
							TrackingList trackingList = new TrackingList(error);
							trackingList.Add(element);
							elementCollection = trackingList;
						}
						else
						{
							TrackingList trackingList = (TrackingList)elementCollection;
							trackingList.Add(element);
							// During deserialization fixup (notifyAdded != null), we need
							// to make sure that the element is not already in the collection
							if (null != mcConstraint)
							{
								LinkedElementCollection<SetComparisonConstraint> typedCollection = trackingList.NativeMultiColumnCollection;
								if (notifyAdded == null || !typedCollection.Contains(mcConstraint))
								{
									typedCollection.Add(mcConstraint);
								}
							}
							else if (null != scConstraint)
							{
								LinkedElementCollection<SetConstraint> typedCollection = trackingList.NativeSingleColumnCollection;
								if (notifyAdded == null || !typedCollection.Contains(scConstraint))
								{
									typedCollection.Add(scConstraint);
								}
							}
							else if (null != vConstraint)
							{
								LinkedElementCollection<ValueConstraint> typedCollection = trackingList.NativeValueCollection;
								if (notifyAdded == null || !typedCollection.Contains(vConstraint))
								{
									typedCollection.Add(vConstraint);
								}
							}
						}
						return elementCollection;
					}
				}
				ICollection IDuplicateNameCollectionManager.OnDuplicateElementRemoved(ICollection elementCollection, ModelElement element, bool afterTransaction)
				{
					TrackingList trackingList = (TrackingList)elementCollection;
					trackingList.Remove(element);
					if (!afterTransaction)
					{
						// Just clear the error. A rule is used to remove the error
						// object itself when there is no longer a duplicate.
						SetComparisonConstraint mcConstraint;
						SetConstraint scConstraint;
						ValueConstraint vConstraint;
						if (null != (scConstraint = element as SetConstraint))
						{
							scConstraint.DuplicateNameError = null;
						}
						else if (null != (mcConstraint = element as SetComparisonConstraint))
						{
							mcConstraint.DuplicateNameError = null;
						}
						else if (null != (vConstraint = element as ValueConstraint))
						{
							vConstraint.DuplicateNameError = null;
						}
					}
					return elementCollection;
				}
				#endregion // IDuplicateNameCollectionManager Implementation
			}
			#region Constructors
			/// <summary>
			/// Default constructor for ConstraintNamedElementDictionary
			/// </summary>
			public ConstraintNamedElementDictionary() : base(new DuplicateNameManager())
			{
			}
			#endregion // Constructors
			#region Base overrides
			/// <summary>
			/// Provide a localized base name pattern for a new Constraint
			/// </summary>
			/// <param name="element">Ignored. Should be a FactType</param>
			/// <returns>A base name string pattern</returns>
			protected override string GetRootNamePattern(ModelElement element)
			{
				Debug.Assert(element is SetComparisonConstraint || element is SetConstraint || element is ValueConstraint);
				// UNDONE: How explicit do we want to be on constraint naming?
				return base.GetRootNamePattern(element);
			}
			/// <summary>
			/// Raise an exception with text specific to a name in a model
			/// </summary>
			/// <param name="element">Element we're attempting to name</param>
			/// <param name="requestedName">The in-use requested name</param>
			protected override void ThrowDuplicateNameException(ModelElement element, string requestedName)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelExceptionNameAlreadyUsedByModel, requestedName));
			}
			#endregion // Base overrides
		}
		#endregion // ConstraintNamedElementDictionary class
		#endregion // Relationship-specific NamedElementDictionary implementations
	}
	public partial class ModelHasObjectType : INamedElementDictionaryLink
	{
		#region INamedElementDictionaryLink implementation
		INamedElementDictionaryParent INamedElementDictionaryLink.ParentRolePlayer
		{
			get { return ParentRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ParentRolePlayer
		/// Returns Model.
		/// </summary>
		protected INamedElementDictionaryParent ParentRolePlayer
		{
			get { return Model; }
		}
		INamedElementDictionaryChild INamedElementDictionaryLink.ChildRolePlayer
		{
			get { return ChildRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ChildRolePlayer
		/// Returns ObjectTypeCollection.
		/// </summary>
		protected INamedElementDictionaryChild ChildRolePlayer
		{
			get { return ObjectType; }
		}
		INamedElementDictionaryRemoteParent INamedElementDictionaryLink.RemoteParentRolePlayer
		{
			get { return RemoteParentRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.RemoteParentRolePlayer
		/// Returns null.
		/// </summary>
		protected INamedElementDictionaryRemoteParent RemoteParentRolePlayer
		{
			get { return ObjectType; }
		}
		#endregion // INamedElementDictionaryLink implementation
	}
	public partial class ModelHasFactType : INamedElementDictionaryLink
	{
		#region INamedElementDictionaryLink implementation
		INamedElementDictionaryParent INamedElementDictionaryLink.ParentRolePlayer
		{
			get { return ParentRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ParentRolePlayer
		/// Returns Model.
		/// </summary>
		protected INamedElementDictionaryParent ParentRolePlayer
		{
			get { return Model; }
		}
		INamedElementDictionaryChild INamedElementDictionaryLink.ChildRolePlayer
		{
			get { return ChildRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ChildRolePlayer
		/// Returns ObjectTypeCollection.
		/// </summary>
		protected INamedElementDictionaryChild ChildRolePlayer
		{
			get { return FactType; }
		}
		INamedElementDictionaryRemoteParent INamedElementDictionaryLink.RemoteParentRolePlayer
		{
			get { return RemoteParentRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.RemoteParentRolePlayer
		/// Returns FactTypeCollection.
		/// </summary>
		protected INamedElementDictionaryRemoteParent RemoteParentRolePlayer
		{
			get { return FactType; }
		}
		#endregion // INamedElementDictionaryLink implementation
	}
	public partial class ModelHasSetComparisonConstraint : INamedElementDictionaryLink
	{
		#region INamedElementDictionaryLink implementation
		INamedElementDictionaryParent INamedElementDictionaryLink.ParentRolePlayer
		{
			get { return ParentRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ParentRolePlayer
		/// Returns Model.
		/// </summary>
		protected INamedElementDictionaryParent ParentRolePlayer
		{
			get { return Model; }
		}
		INamedElementDictionaryChild INamedElementDictionaryLink.ChildRolePlayer
		{
			get { return ChildRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ChildRolePlayer
		/// Returns SetComparisonConstraintCollection.
		/// </summary>
		protected INamedElementDictionaryChild ChildRolePlayer
		{
			get { return SetComparisonConstraint; }
		}
		INamedElementDictionaryRemoteParent INamedElementDictionaryLink.RemoteParentRolePlayer
		{
			get { return RemoteParentRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.RemoteParentRolePlayer
		/// Returns null.
		/// </summary>
		protected static INamedElementDictionaryRemoteParent RemoteParentRolePlayer
		{
			get { return null; }
		}
		#endregion // INamedElementDictionaryLink implementation
	}
	public partial class SetComparisonConstraint : INamedElementDictionaryChild
	{
		#region INamedElementDictionaryChild implementation
		void INamedElementDictionaryChild.GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			GetRoleGuids(out parentDomainRoleId, out childDomainRoleId);
		}
		/// <summary>
		/// Implementation of INamedElementDictionaryChild.GetRoleGuids. Identifies
		/// this child as participating in the 'ModelHasObjectType' naming set.
		/// </summary>
		/// <param name="parentDomainRoleId">Guid</param>
		/// <param name="childDomainRoleId">Guid</param>
		protected static void GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			parentDomainRoleId = ModelHasSetComparisonConstraint.ModelDomainRoleId;
			childDomainRoleId = ModelHasSetComparisonConstraint.SetComparisonConstraintDomainRoleId;
		}
		#endregion // INamedElementDictionaryChild implementation
	}
	public partial class ModelHasSetConstraint : INamedElementDictionaryLink
	{
		#region INamedElementDictionaryLink implementation
		INamedElementDictionaryParent INamedElementDictionaryLink.ParentRolePlayer
		{
			get { return ParentRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ParentRolePlayer
		/// Returns Model.
		/// </summary>
		protected INamedElementDictionaryParent ParentRolePlayer
		{
			get { return Model; }
		}
		INamedElementDictionaryChild INamedElementDictionaryLink.ChildRolePlayer
		{
			get { return ChildRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ChildRolePlayer
		/// Returns SetConstraintCollection.
		/// </summary>
		protected INamedElementDictionaryChild ChildRolePlayer
		{
			get { return SetConstraint; }
		}
		INamedElementDictionaryRemoteParent INamedElementDictionaryLink.RemoteParentRolePlayer
		{
			get { return RemoteParentRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.RemoteParentRolePlayer
		/// Returns null.
		/// </summary>
		protected static INamedElementDictionaryRemoteParent RemoteParentRolePlayer
		{
			get { return null; }
		}
		#endregion // INamedElementDictionaryLink implementation
	}
	public partial class ValueTypeHasValueConstraint : INamedElementDictionaryLink
	{
		#region INamedElementDictionaryLink implementation
		INamedElementDictionaryParent INamedElementDictionaryLink.ParentRolePlayer
		{
			get { return ParentRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ParentRolePlayer
		/// Returns FactType.
		/// </summary>
		protected INamedElementDictionaryParent ParentRolePlayer
		{
			get { return ValueType; }
		}
		INamedElementDictionaryChild INamedElementDictionaryLink.ChildRolePlayer
		{
			get { return ChildRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ChildRolePlayer
		/// Returns InternalConstraintCollection.
		/// </summary>
		protected INamedElementDictionaryChild ChildRolePlayer
		{
			get { return ValueConstraint; }
		}
		INamedElementDictionaryRemoteParent INamedElementDictionaryLink.RemoteParentRolePlayer
		{
			get { return RemoteParentRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.RemoteParentRolePlayer
		/// Returns null
		/// </summary>
		protected static INamedElementDictionaryRemoteParent RemoteParentRolePlayer
		{
			get { return null; }
		}
		#endregion // INamedElementDictionaryLink implementation
	}
	public partial class RoleHasValueConstraint : INamedElementDictionaryLink
	{
		#region INamedElementDictionaryLink implementation
		INamedElementDictionaryParent INamedElementDictionaryLink.ParentRolePlayer
		{
			get { return ParentRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ParentRolePlayer
		/// Returns FactType.
		/// </summary>
		protected INamedElementDictionaryParent ParentRolePlayer
		{
			get { return Role; }
		}
		INamedElementDictionaryChild INamedElementDictionaryLink.ChildRolePlayer
		{
			get { return ChildRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ChildRolePlayer
		/// Returns InternalConstraintCollection.
		/// </summary>
		protected INamedElementDictionaryChild ChildRolePlayer
		{
			get { return ValueConstraint; }
		}
		INamedElementDictionaryRemoteParent INamedElementDictionaryLink.RemoteParentRolePlayer
		{
			get { return RemoteParentRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.RemoteParentRolePlayer
		/// Returns null
		/// </summary>
		protected static INamedElementDictionaryRemoteParent RemoteParentRolePlayer
		{
			get { return null; }
		}
		#endregion // INamedElementDictionaryLink implementation
	}
	public partial class FactTypeHasRole : INamedElementDictionaryLink
	{
		#region INamedElementDictionaryLink implementation
		INamedElementDictionaryParent INamedElementDictionaryLink.ParentRolePlayer
		{
			get { return ParentRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ParentRolePlayer
		/// Returns FactType.
		/// </summary>
		protected static INamedElementDictionaryParent ParentRolePlayer
		{
			get { return null; }
		}
		INamedElementDictionaryChild INamedElementDictionaryLink.ChildRolePlayer
		{
			get { return ChildRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.ChildRolePlayer
		/// Returns InternalConstraintCollection.
		/// </summary>
		protected static INamedElementDictionaryChild ChildRolePlayer
		{
			get { return null; }
		}
		INamedElementDictionaryRemoteParent INamedElementDictionaryLink.RemoteParentRolePlayer
		{
			get { return RemoteParentRolePlayer; }
		}
		/// <summary>
		/// Implements INamedElementDictionaryLink.RemoteParentRolePlayer
		/// Returns null
		/// </summary>
		protected INamedElementDictionaryRemoteParent RemoteParentRolePlayer
		{
			get { return Role as INamedElementDictionaryRemoteParent; }
		}
		#endregion // INamedElementDictionaryLink implementation
	}
	public partial class SetConstraint : INamedElementDictionaryChild
	{
		#region INamedElementDictionaryChild implementation
		void INamedElementDictionaryChild.GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			GetRoleGuids(out parentDomainRoleId, out childDomainRoleId);
		}
		/// <summary>
		/// Implementation of INamedElementDictionaryChild.GetRoleGuids. Identifies
		/// this child as participating in the 'ModelHasObjectType' naming set.
		/// </summary>
		/// <param name="parentDomainRoleId">Guid</param>
		/// <param name="childDomainRoleId">Guid</param>
		protected static void GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			parentDomainRoleId = ModelHasSetConstraint.ModelDomainRoleId;
			childDomainRoleId = ModelHasSetConstraint.SetConstraintDomainRoleId;
		}
		#endregion // INamedElementDictionaryChild implementation
	}
	public partial class ValueTypeValueConstraint : INamedElementDictionaryChild
	{
		#region INamedElementDictionaryChild implementation
		void INamedElementDictionaryChild.GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			GetRoleGuids(out parentDomainRoleId, out childDomainRoleId);
		}
		/// <summary>
		/// Implementation of INamedElementDictionaryChild.GetRoleGuids. Identifies
		/// this child as participating in the 'ModelHasConstraint' naming set.
		/// </summary>
		/// <param name="parentDomainRoleId">Guid</param>
		/// <param name="childDomainRoleId">Guid</param>
		protected static void GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			parentDomainRoleId = ValueTypeHasValueConstraint.ValueTypeDomainRoleId;
			childDomainRoleId = ValueTypeHasValueConstraint.ValueConstraintDomainRoleId;
		}
		#endregion // INamedElementDictionaryChild implementation
	}
	public partial class RoleValueConstraint : INamedElementDictionaryChild
	{
		#region INamedElementDictionaryChild implementation
		void INamedElementDictionaryChild.GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			GetRoleGuids(out parentDomainRoleId, out childDomainRoleId);
		}
		/// <summary>
		/// Implementation of INamedElementDictionaryChild.GetRoleGuids. Identifies
		/// this child as participating in the 'ModelHasConstraint' naming set.
		/// </summary>
		/// <param name="parentDomainRoleId">Guid</param>
		/// <param name="childDomainRoleId">Guid</param>
		protected static void GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			parentDomainRoleId = RoleHasValueConstraint.RoleDomainRoleId;
			childDomainRoleId = RoleHasValueConstraint.ValueConstraintDomainRoleId;
		}
		#endregion // INamedElementDictionaryChild implementation
	}
	public abstract partial class DuplicateNameError : IRepresentModelElements, IModelErrorOwner
	{
		#region DuplicateNameError Specific
		/// <summary>
		/// Get a list of elements with the same name. The
		/// returned elements will all come from a
		/// generated metarole collections.
		/// </summary>
		protected abstract IList<ORMNamedElement> DuplicateElements{ get;}
		/// <summary>
		/// Get the text to display the duplicate error information. Replacement
		/// field {0} is replaced by the model name, field {1} is replaced by the
		/// element name.
		/// </summary>
		protected abstract string ErrorFormatText { get;}
		/// <summary>
		/// Verify that all of the duplicate elements attached to
		/// this error actually have the same name.
		/// </summary>
		/// <returns>true if validation succeeded. false is
		/// returned if testElement does not have a name specified</returns>
		public bool ValidateDuplicates(ORMNamedElement testElement)
		{
			return ValidateDuplicates(testElement, null);
		}
		/// <summary>
		/// Helper function to allow ValidateDuplicates call from
		/// IModelErrorOwner.ValidateErrors with expensive second
		/// call to the DuplicateElements function.
		/// </summary>
		/// <param name="testElement">The element to test</param>
		/// <param name="duplicates">Pre-fetched duplicates, or null</param>
		/// <returns>true if validation succeeded. false is
		/// returned if testElement does not have a name specified</returns>
		private bool ValidateDuplicates(ORMNamedElement testElement, IList<ORMNamedElement> duplicates)
		{
			string testName = testElement.Name;
			if (testName.Length > 0)
			{
				if (duplicates == null)
				{
					duplicates = DuplicateElements;
				}
				int duplicatesCount = duplicates.Count;
				for (int i = 0; i < duplicatesCount; ++i)
				{
					ORMNamedElement compareTo = duplicates[i];
					if (compareTo != testElement && compareTo.Name != testElement.Name)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}
		#endregion // DuplicateNameError Specific
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			IList<ORMNamedElement> elements = DuplicateElements;
			string elementName = (elements.Count != 0) ? (elements[0]).Name : string.Empty;
			ORMModel model = Model;
			string modelName = (model != null) ? model.Name : string.Empty;
			string newText = string.Format(CultureInfo.InvariantCulture, ErrorFormatText, modelName, elementName);
			string currentText = Name;
			if (currentText != newText)
			{
				Name = newText;
			}
		}
		/// <summary>
		/// Regenerate the error text when the model name changes.
		/// An owner name change will drop the error, so there is
		/// no reason to regenerate on owner name change.
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange;
			}
		}
		#endregion // Base overrides
		#region IRepresentModelElements Implementation
		/// <summary>
		/// Implements IRepresentModelElements.GetRepresentedElements
		/// </summary>
		/// <returns></returns>
		protected ORMNamedElement[] GetRepresentedElements()
		{
			// Pick up all roles played directly by this element. This
			// will get ObjectTypeCollection, FactTypeCollection, etc, but
			// not the owning model. These are non-aggregating roles.
			IList<ORMNamedElement> elements = DuplicateElements;
			int count = elements.Count;
			if (count == 0)
			{
				return null;
			}
			ORMNamedElement[] retVal = new ORMNamedElement[count];
			elements.CopyTo(retVal, 0);
			return retVal;
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion // IRepresentModelElements Implementation
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Implements IModelErrorOwner.GetErrorCollection
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			yield return new ModelErrorUsage(this);
			foreach (ModelErrorUsage modelErrorUsage in base.GetErrorCollection(filter))
			{
				yield return modelErrorUsage;
			}
		}
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			return GetErrorCollection(filter);
		}
		/// <summary>
		/// Implements IModelErrorOwner.ValidateErrors
		/// Make sure that the DuplicateNameError is correct
		/// </summary>
		/// <param name="notifyAdded">A callback for notifying
		/// the caller of all objects that are added.</param>
		protected new void ValidateErrors(INotifyElementAdded notifyAdded)
		{
			if (!IsDeleted)
			{
				IList<ORMNamedElement> duplicates = DuplicateElements;
				// Note that existing name error links are validated when
				// the element is loaded via the IDuplicateNameCollectionManager
				// implementation(s) on the model itself. All remaining duplicate
				// name errors should be errors that are attached to elements whose
				// named was changed externally.
				if (duplicates.Count < 2 ||
					!ValidateDuplicates(duplicates[0], duplicates))
				{
					Delete();
				}
			}
		}
		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateErrors(notifyAdded);
		}
		/// <summary>
		/// Implements IModelErrorOwner.DelayValidateErrors
		/// </summary>
		protected static new void DelayValidateErrors()
		{
			// No implementation required
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner Implementation
	}
	#region Relationship-specific derivations of DuplicateNameError
	public partial class ObjectTypeDuplicateNameError : DuplicateNameError, IHasIndirectModelErrorOwner
	{
		/// <summary>
		/// Get the duplicate elements represented by this DuplicateNameError
		/// </summary>
		/// <returns>ObjectTypeCollection</returns>
		protected override IList<ORMNamedElement> DuplicateElements
		{
			get
			{
				return ObjectTypeCollection.ToArray();
			}
		}
		/// <summary>
		/// Get the text to display the duplicate error information. Replacement
		/// field {0} is replaced by the model name, field {1} is replaced by the
		/// element name.
		/// </summary>
		protected override string ErrorFormatText
		{
			get
			{
				return ResourceStrings.ModelErrorModelHasDuplicateObjectTypeNames;
			}
		}
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { ObjectTypeHasDuplicateNameError.DuplicateNameErrorDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
	}
	public partial class ConstraintDuplicateNameError : DuplicateNameError, IHasIndirectModelErrorOwner
	{
		/// <summary>
		/// Get the duplicate elements represented by this DuplicateNameError
		/// </summary>
		/// <returns>ConstraintCollection</returns>
		protected override IList<ORMNamedElement> DuplicateElements
		{
			get
			{
				return ConstraintCollection;
			}
		}
		/// <summary>
		/// Get the text to display the duplicate error information. Replacement
		/// field {0} is replaced by the model name, field {1} is replaced by the
		/// element name.
		/// </summary>
		protected override string ErrorFormatText
		{
			get
			{
				return ResourceStrings.ModelErrorModelHasDuplicateConstraintNames;
			}
		}
		#region ConstraintCollection Implementation
		[NonSerialized]
		private CompositeCollection myCompositeList;
		/// <summary>
		/// Return a constraint collection encompassing
		/// single column external, multi column external, internal constraints, and value constraints
		/// </summary>
		/// <value></value>
		public IList<ORMNamedElement> ConstraintCollection
		{
			get
			{
				return myCompositeList ?? (myCompositeList = new CompositeCollection(this));
			}
		}
		private sealed class CompositeCollection : IList<ORMNamedElement>
		{
			#region Member Variables
			private readonly LinkedElementCollection<SetComparisonConstraint> myList1;
			private readonly LinkedElementCollection<SetConstraint> myList2;
			private readonly LinkedElementCollection<ValueConstraint> myList3;
			#endregion // Member Variables
			#region Constructors
			public CompositeCollection(ConstraintDuplicateNameError error)
			{
				myList1 = error.SetComparisonConstraintCollection;
				myList2 = error.SetConstraintCollection;
				myList3 = error.ValueConstraintCollection;
			}
			#endregion // Constructors
			#region IList<ORMNamedElement> Implementation
			int IList<ORMNamedElement>.IndexOf(ORMNamedElement value)
			{
				SetComparisonConstraint setComparisonConstraint;
				SetConstraint setConstraint;
				ValueConstraint valueConstraint;
				if ((setComparisonConstraint = value as SetComparisonConstraint) != null)
				{
					return myList1.IndexOf(setComparisonConstraint);
				}
				else if ((setConstraint = value as SetConstraint) != null)
				{
					return myList2.IndexOf(setConstraint);
				}
				else if ((valueConstraint = value as ValueConstraint) != null)
				{
					return myList3.IndexOf(valueConstraint);
				}
				return -1;
			}
			ORMNamedElement IList<ORMNamedElement>.this[int index]
			{
				get
				{
					int list1Count = myList1.Count;
					if (index >= list1Count)
					{
						index -= list1Count;
						int list2Count = myList2.Count;
						return (index >= list2Count) ? (ORMNamedElement)myList3[index - list2Count] : myList2[index];
					}
					return myList1[index];
				}
				set
				{
					throw new NotSupportedException(); // Not supported for readonly list
				}
			}
			void IList<ORMNamedElement>.Insert(int index, ORMNamedElement value)
			{
				throw new NotSupportedException(); // Not supported for readonly list
			}
			void IList<ORMNamedElement>.RemoveAt(int index)
			{
				throw new NotSupportedException(); // Not supported for readonly list
			}
			#endregion // IList<ORMNamedElement> Implementation
			#region ICollection<ORMNamedElement> Implementation
			void ICollection<ORMNamedElement>.CopyTo(ORMNamedElement[] array, int index)
			{
				int baseIndex = index;
				int nextCount = myList1.Count;
				if (nextCount != 0)
				{
					((ICollection)myList1).CopyTo(array, baseIndex);
					baseIndex += nextCount;
				}
				nextCount = myList2.Count;
				if (nextCount != 0)
				{
					((ICollection)myList2).CopyTo(array, baseIndex);
					baseIndex += nextCount;
				}
				nextCount = myList3.Count;
				if (nextCount != 0)
				{
					((ICollection)myList3).CopyTo(array, baseIndex);
				}
			}
			int ICollection<ORMNamedElement>.Count
			{
				get
				{
					return myList1.Count + myList2.Count + myList3.Count;
				}
			}
			bool ICollection<ORMNamedElement>.Contains(ORMNamedElement value)
			{
				SetComparisonConstraint setComparisonConstraint;
				SetConstraint setConstraint;
				ValueConstraint valueConstraint;
				if ((setComparisonConstraint = value as SetComparisonConstraint) != null)
				{
					return myList1.Contains(setComparisonConstraint);
				}
				else if ((setConstraint = value as SetConstraint) != null)
				{
					return myList2.Contains(setConstraint);
				}
				else if ((valueConstraint = value as ValueConstraint) != null)
				{
					return myList3.Contains(valueConstraint);
				}
				return false;
			}
			bool ICollection<ORMNamedElement>.IsReadOnly
			{
				get
				{
					return true;
				}
			}
			void ICollection<ORMNamedElement>.Add(ORMNamedElement value)
			{
				throw new NotSupportedException(); // Not supported for readonly list
			}
			void ICollection<ORMNamedElement>.Clear()
			{
				throw new NotSupportedException(); // Not supported for readonly list
			}
			bool ICollection<ORMNamedElement>.Remove(ORMNamedElement value)
			{
				throw new NotSupportedException(); // Not supported for readonly list
			}
			#endregion // ICollection<ORMNamedElement> Implementation
			#region IEnumerable<ORMNamedElement> Implementation
			IEnumerator<ORMNamedElement> IEnumerable<ORMNamedElement>.GetEnumerator()
			{
				foreach (ORMNamedElement element in myList1)
				{
					yield return element;
				}
				foreach (ORMNamedElement element in myList2)
				{
					yield return element;
				}
				foreach (ORMNamedElement element in myList3)
				{
					yield return element;
				}
			}
			#endregion // IEnumerable<ORMNamedElement> Implementation
			#region IEnumerable Implementation
			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable<ORMNamedElement>)this).GetEnumerator();
			}
			#endregion // IEnumerable Implementation
		}
		#endregion // ConstraintCollection Implementation
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		/// </summary>
		protected static Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[]{
					SetComparisonConstraintHasDuplicateNameError.DuplicateNameErrorDomainRoleId,
					SetConstraintHasDuplicateNameError.DuplicateNameErrorDomainRoleId,
					ValueConstraintHasDuplicateNameError.DuplicateNameErrorDomainRoleId};
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
	}
	#endregion // Relationship-specific derivations of DuplicateNameError
	#endregion // NamedElementDictionary and DuplicateNameError integration
}
