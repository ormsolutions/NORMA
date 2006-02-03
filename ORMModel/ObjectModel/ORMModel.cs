using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.Framework;

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
		/// Add implicit elements at this stage. An implicit element is
		/// not serialized and is generally created by a rule once the model
		/// is loaded.
		/// </summary>
		AddImplicitElements = 200,
		/// <summary>
		/// Model errors are stored with the model, but are vulnerable
		/// to the Notepad effect, which can cause errors to be added
		/// or removed from the model. Validate errors after all other
		/// explicit, intrinsic, and implicit elements are in place.
		/// </summary>
		ValidateErrors = 300,
		/// <summary>
		/// Add any presentation elements that are implicit and not
		/// serialized with the model.
		/// </summary>
		AddImplicitPresentationElements = 400,
		/// <summary>
		/// Remove any orphaned presentation elements, meaning any
		/// PresentationElement where the ModelElement role property is null.
		/// Orphaned pels are currently not supported.
		/// </summary>
		RemoveOrphanedPresentationElements = 500,
	}
	#endregion // ORMDeserializationFixupPhase enum
	public partial class ORMModel
	{
		#region Root Xml Element Names
		/// <summary>
		/// The tag name for the element used as the root node of an ORM model
		/// </summary>
		public const string RootXmlElementName = "ORMModel";
		/// <summary>
		/// The namespace for the root node of an ORM model
		/// </summary>
		public const string RootXmlNamespace = "http://schemas.neumont.edu/ORM/2006-01/ORMCore";
		#endregion // Root Xml Element Names
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
		[CLSCompliant(false)]
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
		#region MergeContext functions
		/// <summary>
		/// Support adding root elements and constraints directly to the design surface
		/// </summary>
		/// <param name="elementGroupPrototype"></param>
		/// <param name="protoElement"></param>
		/// <returns></returns>
		protected override bool CanAddChildElement(ElementGroupPrototype elementGroupPrototype, ProtoElementBase protoElement)
		{
			if (protoElement == null)
			{
				return false;
			}
			MetaClassInfo classInfo = Store.MetaDataDirectory.FindMetaClass(protoElement.MetaClassId);
			return classInfo.IsDerivedFrom(RootType.MetaClassGuid) || classInfo.IsDerivedFrom(MultiColumnExternalConstraint.MetaClassGuid) || classInfo.IsDerivedFrom(SingleColumnExternalConstraint.MetaClassGuid);
		}
		/// <summary>
		/// Attach a deserialized ObjectType, FactType, or external constraint to the model.
		/// Called after prototypes for these items are dropped onto the diagram
		/// from the toolbox.
		/// </summary>
		/// <param name="sourceElement">The element being added</param>
		/// <param name="elementGroup">The element describing all of the created elements</param>
		public override void MergeRelate(ModelElement sourceElement, ElementGroup elementGroup)
		{
			base.MergeRelate(sourceElement, elementGroup);
			ObjectType objectType;
			FactType factType;
			SingleColumnExternalConstraint singleColumnConstraint;
			MultiColumnExternalConstraint multiColumnConstraint;
			if (null != (objectType = sourceElement as ObjectType))
			{
				if ("VALUETYPE" == (string) elementGroup.UserData)
				{
					objectType.DataType = DefaultDataType;
				}
				objectType.Model = this;
			}
			else if (null != (factType = sourceElement as FactType))
			{
				factType.Model = this;
			}
			else if (null != (singleColumnConstraint = sourceElement as SingleColumnExternalConstraint))
			{
				singleColumnConstraint.Model = this;
			}
			else if (null != (multiColumnConstraint = sourceElement as MultiColumnExternalConstraint))
			{
				multiColumnConstraint.Model = this;
			}
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
		private NamedElementDictionary myFactTypesDictionary;
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
		/// Returns the Fact Types Dictionary
		/// </summary>
		/// <value>The model FactTypesDictionary</value>
		public INamedElementDictionary FactTypesDictionary
		{
			get
			{
				INamedElementDictionary retVal = myFactTypesDictionary;
				if (retVal == null)
				{
					retVal = myFactTypesDictionary = new FactTypeNamedElementDictionary();
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

		INamedElementDictionary INamedElementDictionaryParent.GetCounterpartRoleDictionary(Guid parentMetaRoleGuid, Guid childMetaRoleGuid)
		{
			return GetCounterpartRoleDictionary(parentMetaRoleGuid, childMetaRoleGuid);
		}
		/// <summary>
		/// Implements INamedElementDictionaryParent.GetCounterpartRoleDictionary
		/// </summary>
		/// <param name="parentMetaRoleGuid">Guid</param>
		/// <param name="childMetaRoleGuid">Guid</param>
		/// <returns>Dictionaries for object types, fact types, and constraints</returns>
		public INamedElementDictionary GetCounterpartRoleDictionary(Guid parentMetaRoleGuid, Guid childMetaRoleGuid)
		{
			if (parentMetaRoleGuid == ModelHasObjectType.ModelMetaRoleGuid)
			{
				return ObjectTypesDictionary;
			}
			else if (parentMetaRoleGuid == ModelHasFactType.ModelMetaRoleGuid)
			{
				return FactTypesDictionary;
			}
			else if (parentMetaRoleGuid == ModelHasMultiColumnExternalConstraint.ModelMetaRoleGuid ||
					 parentMetaRoleGuid == ModelHasSingleColumnExternalConstraint.ModelMetaRoleGuid ||
					 parentMetaRoleGuid == FactTypeHasInternalConstraint.FactTypeMetaRoleGuid ||
					 parentMetaRoleGuid == ValueTypeHasValueConstraint.ValueTypeMetaRoleGuid ||
					 parentMetaRoleGuid == RoleHasValueConstraint.RoleMetaRoleGuid)
			{
				return ConstraintsDictionary;
			}
			return null;
		}
		object INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey(Guid parentMetaRoleGuid, Guid childMetaRoleGuid)
		{
			return GetAllowDuplicateNamesContextKey(parentMetaRoleGuid, childMetaRoleGuid);
		}
		/// <summary>
		/// Implements INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey
		/// </summary>
		/// <param name="parentMetaRoleGuid">Guid</param>
		/// <param name="childMetaRoleGuid">Guid</param>
		/// <returns></returns>
		protected static object GetAllowDuplicateNamesContextKey(Guid parentMetaRoleGuid, Guid childMetaRoleGuid)
		{
			// Use the default settings (allow duplicates during load time only)
			return null;
		}
		#endregion // INamedElementDictionaryParent implementation
		#region Rules to remove duplicate name errors
		[RuleOn(typeof(ObjectTypeHasDuplicateNameError))]
		private class RemoveDuplicateObjectTypeNameErrorRule : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ObjectTypeHasDuplicateNameError link = e.ModelElement as ObjectTypeHasDuplicateNameError;
				ObjectTypeDuplicateNameError error = link.DuplicateNameError;
				if (!error.IsRemoved)
				{
					if (error.ObjectTypeCollection.Count < 2)
					{
						error.Remove();
					}
				}
			}
		}
		[RuleOn(typeof(FactTypeHasDuplicateNameError))]
		private class RemoveDuplicateFactTypeNameErrorRule : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				FactTypeHasDuplicateNameError link = e.ModelElement as FactTypeHasDuplicateNameError;
				FactTypeDuplicateNameError error = link.DuplicateNameError;
				if (!error.IsRemoved)
				{
					if (error.FactTypeCollection.Count < 2)
					{
						error.Remove();
					}
				}
			}
		}
		[RuleOn(typeof(MultiColumnExternalConstraintHasDuplicateNameError)), RuleOn(typeof(SingleColumnExternalConstraintHasDuplicateNameError)), RuleOn(typeof(InternalConstraintHasDuplicateNameError))]
		private class RemoveDuplicateConstraintNameErrorRule : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ModelElement link = e.ModelElement;
				MultiColumnExternalConstraintHasDuplicateNameError mcLink;
				SingleColumnExternalConstraintHasDuplicateNameError scLink;
				InternalConstraintHasDuplicateNameError iLink;
				ConstraintDuplicateNameError error = null;
				if (null != (mcLink = link as MultiColumnExternalConstraintHasDuplicateNameError))
				{
					error = mcLink.DuplicateNameError;
				}
				else if (null != (scLink = link as SingleColumnExternalConstraintHasDuplicateNameError))
				{
					error = scLink.DuplicateNameError;
				}
				else if (null != (iLink = link as InternalConstraintHasDuplicateNameError))
				{
					error = iLink.DuplicateNameError;
				}
				if (error != null && !error.IsRemoved)
				{
					if ((error.MultiColumnExternalConstraintCollection.Count + error.SingleColumnExternalConstraintCollection.Count + error.InternalConstraintCollection.Count) < 2)
					{
						error.Remove();
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
		[CLSCompliant(false)]
		protected class ObjectTypeNamedElementDictionary : NamedElementDictionary
		{
			private class DuplicateNameManager : IDuplicateNameCollectionManager
			{
				#region IDuplicateNameCollectionManager Implementation
				ICollection IDuplicateNameCollectionManager.OnDuplicateElementAdded(ICollection elementCollection, NamedElement element, bool afterTransaction, INotifyElementAdded notifyAdded)
				{
					ObjectType objectType = (ObjectType)element;
					if (afterTransaction)
					{
						// We're not in a transaction, but the object model will be in
						// the state we need it because we put it there during a transaction.
						// Just return the collection from the current state of the object model.
						ObjectTypeDuplicateNameError error = objectType.DuplicateNameError;
						return (error != null) ? error.ObjectTypeCollection : null;
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
								error = ObjectTypeDuplicateNameError.CreateObjectTypeDuplicateNameError(objectType.Store);
								objectType.DuplicateNameError = error;
								error.Model = objectType.Model;
								error.GenerateErrorText();
								if (notifyAdded != null)
								{
									notifyAdded.ElementAdded(error, true);
								}
							}
							elementCollection = error.ObjectTypeCollection;
						}
						else
						{
							// During deserialization fixup (notifyAdded != null), we need
							// to make sure that the element is not already in the collection
							ObjectTypeMoveableCollection typedCollection = (ObjectTypeMoveableCollection)elementCollection;
							if (notifyAdded == null || !typedCollection.Contains(objectType))
							{
								typedCollection.Add(objectType);
							}
						}
						return elementCollection;
					}
				}
				ICollection IDuplicateNameCollectionManager.OnDuplicateElementRemoved(ICollection elementCollection, NamedElement element, bool afterTransaction)
				{
					if (!afterTransaction)
					{
						// Just clear the error. A rule is used to remove the error
						// object itself when there is not longer a duplicate.
						((ObjectType)element).DuplicateNameError = null;
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
			protected override string GetRootNamePattern(NamedElement element)
			{
				ObjectType objectType = (ObjectType)element;
				return objectType.IsValueType ? ResourceStrings.ValueTypeDefaultNamePattern : ResourceStrings.EntityTypeDefaultNamePattern;
			}
			/// <summary>
			/// Raise an exception with text specific to a name in a model
			/// </summary>
			/// <param name="element">Element we're attempting to name</param>
			/// <param name="requestedName">The in-use requested name</param>
			protected override void ThrowDuplicateNameException(NamedElement element, string requestedName)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelExceptionNameAlreadyUsedByModel, requestedName));
			}
			#endregion // Base overrides
		}
		#endregion // ObjectTypeNamedElementDictionary class
		#region FactTypeNamedElementDictionary class
		/// <summary>
		/// Dictionary used to set the initial names of fact types and to
		/// generate model validation errors and exceptions for duplicate
		/// element names.
		/// </summary>
		[CLSCompliant(false)]
		protected class FactTypeNamedElementDictionary : NamedElementDictionary
		{
			private class DuplicateNameManager : IDuplicateNameCollectionManager
			{
				#region IDuplicateNameCollectionManager Implementation
				ICollection IDuplicateNameCollectionManager.OnDuplicateElementAdded(ICollection elementCollection, NamedElement element, bool afterTransaction, INotifyElementAdded notifyAdded)
				{
					FactType factType = (FactType)element;
					if (afterTransaction)
					{
						// We're not in a transaction, but the object model will be in
						// the state we need it because we put it there during a transaction.
						// Just return the collection from the current state of the object model.
						FactTypeDuplicateNameError error = factType.DuplicateNameError;
						return (error != null) ? error.FactTypeCollection : null;
					}
					else
					{
						// Modify the object model to add the error.
						if (elementCollection == null)
						{
							FactTypeDuplicateNameError error = null;
							if (notifyAdded != null)
							{
								// During deserialization fixup, an error
								// may already be attached to the object. Track
								// it down and verify that it is a legitimate error.
								// If it is not legitimate, then generate a new one.
								error = factType.DuplicateNameError;
								if (error != null && !error.ValidateDuplicates(factType))
								{
									error = null;
								}
							}
							if (error == null)
							{
								error = FactTypeDuplicateNameError.CreateFactTypeDuplicateNameError(factType.Store);
								factType.DuplicateNameError = error;
								error.Model = factType.Model;
								error.GenerateErrorText();
								if (notifyAdded != null)
								{
									notifyAdded.ElementAdded(error, true);
								}
							}
							elementCollection = error.FactTypeCollection;
						}
						else
						{
							// During deserialization fixup (notifyAdded != null), we need
							// to make sure that the element is not already in the collection
							FactTypeMoveableCollection typedCollection = (FactTypeMoveableCollection)elementCollection;
							if (notifyAdded == null || !typedCollection.Contains(factType))
							{
								typedCollection.Add(factType);
							}
						}
						return elementCollection;
					}
				}
				ICollection IDuplicateNameCollectionManager.OnDuplicateElementRemoved(ICollection elementCollection, NamedElement element, bool afterTransaction)
				{
					if (!afterTransaction)
					{
						// Just clear the error. A rule is used to remove the error
						// object itself when there is not longer a duplicate.
						((FactType)element).DuplicateNameError = null;
					}
					return elementCollection;
				}
				#endregion // IDuplicateNameCollectionManager Implementation
			}
			#region Constructors
			/// <summary>
			/// Default constructor for FactTypeNamedElementDictionary
			/// </summary>
			public FactTypeNamedElementDictionary() : base(new DuplicateNameManager())
			{
			}
			#endregion // Constructors
			#region Base overrides
			/// <summary>
			/// Provide a localized base name pattern for a new FactType
			/// </summary>
			/// <param name="element">Ignored. Should be a FactType</param>
			/// <returns>A base name string pattern</returns>
			protected override string GetRootNamePattern(NamedElement element)
			{
				Debug.Assert(element is FactType);
				return (element is SubtypeFact) ? ResourceStrings.SubtypeFactDefaultNamePattern : ResourceStrings.FactTypeDefaultNamePattern;
			}
			/// <summary>
			/// Raise an exception with text specific to a name in a model
			/// </summary>
			/// <param name="element">Element we're attempting to name</param>
			/// <param name="requestedName">The in-use requested name</param>
			protected override void ThrowDuplicateNameException(NamedElement element, string requestedName)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelExceptionNameAlreadyUsedByModel, requestedName));
			}
			#endregion // Base overrides
		}
		#endregion // FactTypeNamedElementDictionary class
		#region ConstraintNamedElementDictionary class
		/// <summary>
		/// Dictionary used to set the initial names of constraints and to
		/// generate model validation errors and exceptions for duplicate
		/// element names.
		/// </summary>
		[CLSCompliant(false)]
		protected class ConstraintNamedElementDictionary : NamedElementDictionary
		{
			private class DuplicateNameManager : IDuplicateNameCollectionManager
			{
				#region IDuplicateNameCollectionManager Implementation
				ICollection IDuplicateNameCollectionManager.OnDuplicateElementAdded(ICollection elementCollection, NamedElement element, bool afterTransaction, INotifyElementAdded notifyAdded)
				{
					SingleColumnExternalConstraint scConstraint = null;
					MultiColumnExternalConstraint mcConstraint = null;
					InternalConstraint iConstraint = null;
					ValueConstraint vConstraint = null;
					ConstraintDuplicateNameError existingError = null;
					if (null != (scConstraint = element as SingleColumnExternalConstraint))
					{
						existingError = scConstraint.DuplicateNameError;
					}
					else if (null != (mcConstraint = element as MultiColumnExternalConstraint))
					{
						existingError = mcConstraint.DuplicateNameError;
					}
					else if (null != (iConstraint = element as InternalConstraint))
					{
						existingError = iConstraint.DuplicateNameError;
					}
					else if (null != (vConstraint = element as ValueConstraint))
					{
						existingError = vConstraint.DuplicateNameError;
					}
					Debug.Assert(scConstraint != null || mcConstraint != null || iConstraint != null || vConstraint != null);
					if (afterTransaction)
					{
						// We're not in a transaction, but the object model will be in
						// the state we need it because we put it there during a transaction.
						// Just return the collection from the current state of the object model.
						return (existingError != null) ? existingError.ConstraintCollection : null;
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
								if (error != null && !error.ValidateDuplicates(element))
								{
									error = null;
								}
							}
							if (error == null)
							{
								error = ConstraintDuplicateNameError.CreateConstraintDuplicateNameError(element.Store);
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
								else if (iConstraint != null)
								{
									iConstraint.DuplicateNameError = error;
									Debug.Assert(iConstraint.FactType != null && iConstraint.FactType.Model != null); // Can't get here unless the constraint is attached to an attached fact
									error.Model = iConstraint.FactType.Model;
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
							elementCollection = error.ConstraintCollection;
						}
						else
						{
							// During deserialization fixup (notifyAdded != null), we need
							// to make sure that the element is not already in the collection
							IList typedCollection = (IList)elementCollection;
							if (notifyAdded == null || !typedCollection.Contains(element))
							{
								typedCollection.Add(element);
							}
						}
						return elementCollection;
					}
				}
				ICollection IDuplicateNameCollectionManager.OnDuplicateElementRemoved(ICollection elementCollection, NamedElement element, bool afterTransaction)
				{
					if (!afterTransaction)
					{
						// Just clear the error. A rule is used to remove the error
						// object itself when there is not longer a duplicate.
						MultiColumnExternalConstraint mcConstraint;
						SingleColumnExternalConstraint scConstraint;
						InternalConstraint iConstraint;
						if (null != (scConstraint = element as SingleColumnExternalConstraint))
						{
							scConstraint.DuplicateNameError = null;
						}
						else if (null != (mcConstraint = element as MultiColumnExternalConstraint))
						{
							mcConstraint.DuplicateNameError = null;
						}
						else if (null != (iConstraint = element as InternalConstraint))
						{
							iConstraint.DuplicateNameError = null;
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
			protected override string GetRootNamePattern(NamedElement element)
			{
				Debug.Assert(element is MultiColumnExternalConstraint || element is SingleColumnExternalConstraint || element is InternalConstraint || element is ValueConstraint);
				// UNDONE: How explicit do we want to be on constraint naming?
				return base.GetRootNamePattern(element);
			}
			/// <summary>
			/// Raise an exception with text specific to a name in a model
			/// </summary>
			/// <param name="element">Element we're attempting to name</param>
			/// <param name="requestedName">The in-use requested name</param>
			protected override void ThrowDuplicateNameException(NamedElement element, string requestedName)
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
			get { return ObjectTypeCollection; }
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
			get { return ObjectTypeCollection; }
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
			get { return FactTypeCollection; }
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
			get { return FactTypeCollection; }
		}
		#endregion // INamedElementDictionaryLink implementation
	}
	public partial class ModelHasMultiColumnExternalConstraint : INamedElementDictionaryLink
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
		/// Returns MultiColumnExternalConstraintCollection.
		/// </summary>
		protected INamedElementDictionaryChild ChildRolePlayer
		{
			get { return MultiColumnExternalConstraintCollection; }
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
	public partial class MultiColumnExternalConstraint : INamedElementDictionaryChild
	{
		#region INamedElementDictionaryChild implementation
		void INamedElementDictionaryChild.GetRoleGuids(out Guid parentMetaRoleGuid, out Guid childMetaRoleGuid)
		{
			GetRoleGuids(out parentMetaRoleGuid, out childMetaRoleGuid);
		}
		/// <summary>
		/// Implementation of INamedElementDictionaryChild.GetRoleGuids. Identifies
		/// this child as participating in the 'ModelHasObjectType' naming set.
		/// </summary>
		/// <param name="parentMetaRoleGuid">Guid</param>
		/// <param name="childMetaRoleGuid">Guid</param>
		protected static void GetRoleGuids(out Guid parentMetaRoleGuid, out Guid childMetaRoleGuid)
		{
			parentMetaRoleGuid = ModelHasMultiColumnExternalConstraint.ModelMetaRoleGuid;
			childMetaRoleGuid = ModelHasMultiColumnExternalConstraint.MultiColumnExternalConstraintCollectionMetaRoleGuid;
		}
		#endregion // INamedElementDictionaryChild implementation
	}
	public partial class ModelHasSingleColumnExternalConstraint : INamedElementDictionaryLink
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
		/// Returns SingleColumnExternalConstraintCollection.
		/// </summary>
		protected INamedElementDictionaryChild ChildRolePlayer
		{
			get { return SingleColumnExternalConstraintCollection; }
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
	public partial class FactTypeHasInternalConstraint : INamedElementDictionaryLink
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
			get { return FactType; }
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
			get { return InternalConstraintCollection; }
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
			get { return RoleCollection; }
		}
		#endregion // INamedElementDictionaryLink implementation
	}
	public partial class SingleColumnExternalConstraint : INamedElementDictionaryChild
	{
		#region INamedElementDictionaryChild implementation
		void INamedElementDictionaryChild.GetRoleGuids(out Guid parentMetaRoleGuid, out Guid childMetaRoleGuid)
		{
			GetRoleGuids(out parentMetaRoleGuid, out childMetaRoleGuid);
		}
		/// <summary>
		/// Implementation of INamedElementDictionaryChild.GetRoleGuids. Identifies
		/// this child as participating in the 'ModelHasObjectType' naming set.
		/// </summary>
		/// <param name="parentMetaRoleGuid">Guid</param>
		/// <param name="childMetaRoleGuid">Guid</param>
		protected static void GetRoleGuids(out Guid parentMetaRoleGuid, out Guid childMetaRoleGuid)
		{
			parentMetaRoleGuid = ModelHasSingleColumnExternalConstraint.ModelMetaRoleGuid;
			childMetaRoleGuid = ModelHasSingleColumnExternalConstraint.SingleColumnExternalConstraintCollectionMetaRoleGuid;
		}
		#endregion // INamedElementDictionaryChild implementation
	}
	public partial class InternalConstraint : INamedElementDictionaryChild
	{
		#region INamedElementDictionaryChild implementation
		void INamedElementDictionaryChild.GetRoleGuids(out Guid parentMetaRoleGuid, out Guid childMetaRoleGuid)
		{
			GetRoleGuids(out parentMetaRoleGuid, out childMetaRoleGuid);
		}
		/// <summary>
		/// Implementation of INamedElementDictionaryChild.GetRoleGuids. Identifies
		/// this child as participating in the 'ModelHasConstraint' naming set.
		/// </summary>
		/// <param name="parentMetaRoleGuid">Guid</param>
		/// <param name="childMetaRoleGuid">Guid</param>
		protected static void GetRoleGuids(out Guid parentMetaRoleGuid, out Guid childMetaRoleGuid)
		{
			parentMetaRoleGuid = FactTypeHasInternalConstraint.FactTypeMetaRoleGuid;
			childMetaRoleGuid = FactTypeHasInternalConstraint.InternalConstraintCollectionMetaRoleGuid;
		}
		#endregion // INamedElementDictionaryChild implementation
	}
	public partial class ValueTypeValueConstraint : INamedElementDictionaryChild
	{
		#region INamedElementDictionaryChild implementation
		void INamedElementDictionaryChild.GetRoleGuids(out Guid parentMetaRoleGuid, out Guid childMetaRoleGuid)
		{
			GetRoleGuids(out parentMetaRoleGuid, out childMetaRoleGuid);
		}
		/// <summary>
		/// Implementation of INamedElementDictionaryChild.GetRoleGuids. Identifies
		/// this child as participating in the 'ModelHasConstraint' naming set.
		/// </summary>
		/// <param name="parentMetaRoleGuid">Guid</param>
		/// <param name="childMetaRoleGuid">Guid</param>
		protected static void GetRoleGuids(out Guid parentMetaRoleGuid, out Guid childMetaRoleGuid)
		{
			parentMetaRoleGuid = ValueTypeHasValueConstraint.ValueTypeMetaRoleGuid;
			childMetaRoleGuid = ValueTypeHasValueConstraint.ValueConstraintMetaRoleGuid;
		}
		#endregion // INamedElementDictionaryChild implementation
	}
	public partial class RoleValueConstraint : INamedElementDictionaryChild
	{
		#region INamedElementDictionaryChild implementation
		void INamedElementDictionaryChild.GetRoleGuids(out Guid parentMetaRoleGuid, out Guid childMetaRoleGuid)
		{
			GetRoleGuids(out parentMetaRoleGuid, out childMetaRoleGuid);
		}
		/// <summary>
		/// Implementation of INamedElementDictionaryChild.GetRoleGuids. Identifies
		/// this child as participating in the 'ModelHasConstraint' naming set.
		/// </summary>
		/// <param name="parentMetaRoleGuid">Guid</param>
		/// <param name="childMetaRoleGuid">Guid</param>
		protected static void GetRoleGuids(out Guid parentMetaRoleGuid, out Guid childMetaRoleGuid)
		{
			parentMetaRoleGuid = RoleHasValueConstraint.RoleMetaRoleGuid;
			childMetaRoleGuid = RoleHasValueConstraint.ValueConstraintMetaRoleGuid;
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
		protected abstract IList DuplicateElements{ get;}
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
		/// <param name="testElement"></param>
		/// <returns>true if validation succeeded. false is
		/// returned if testElement does not have a name specified</returns>
		public bool ValidateDuplicates(NamedElement testElement)
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
		private bool ValidateDuplicates(NamedElement testElement, IList duplicates)
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
					NamedElement compareTo = (NamedElement)duplicates[i];
					if (!object.ReferenceEquals(compareTo, testElement) &&
						compareTo.Name != testElement.Name)
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
			IList elements = DuplicateElements;
			string elementName = (elements.Count != 0) ? ((NamedElement)elements[0]).Name : "";
			ORMModel model = Model;
			string modelName = (model != null) ? model.Name : "";
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
		protected ModelElement[] GetRepresentedElements()
		{
			// Pick up all roles played directly by this element. This
			// will get ObjectTypeCollection, FactTypeCollection, etc, but
			// not the owning model. These are non-aggregating roles.
			ICollection elements = DuplicateElements;
			int count = elements.Count;
			if (count == 0)
			{
				return null;
			}
			ModelElement[] retVal = new ModelElement[count];
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
		/// Implements IModelErrorOwner.ErrorCollection
		/// </summary>
		[CLSCompliant(false)]
		protected IEnumerable<ModelError> ErrorCollection
		{
			get
			{
				yield return this;
			}
		}
		IEnumerable<ModelError> IModelErrorOwner.ErrorCollection
		{
			get
			{
				return ErrorCollection;
			}
		}
		/// <summary>
		/// Implements IModelErrorOwner.ValidateErrors
		/// Make sure that the DuplicateNameError is correct
		/// </summary>
		/// <param name="notifyAdded">A callback for notifying
		/// the caller of all objects that are added.</param>
		protected void ValidateErrors(INotifyElementAdded notifyAdded)
		{
			if (!IsRemoved)
			{
				IList duplicates = DuplicateElements;
				// Note that existing name error links are validated when
				// the element is loaded via the IDuplicateNameCollectionManager
				// implementation(s) on the model itself. All remaining duplicate
				// name errors should be errors that are attached to elements whose
				// named was changed externally.
				if (duplicates.Count < 2 ||
					!ValidateDuplicates((NamedElement)duplicates[0], duplicates))
				{
					Remove();
				}
			}
		}
		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateErrors(notifyAdded);
		}
		#endregion // IModelErrorOwner Implementation
	}
	#region Relationship-specific derivations of DuplicateNameError
	public partial class ObjectTypeDuplicateNameError : DuplicateNameError
	{
		/// <summary>
		/// Get the duplicate elements represented by this DuplicateNameError
		/// </summary>
		/// <returns>ObjectTypeCollection</returns>
		protected override IList DuplicateElements
		{
			get
			{
				return ObjectTypeCollection;
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
	}
	public partial class FactTypeDuplicateNameError : DuplicateNameError
	{
		/// <summary>
		/// Get the duplicate elements represented by this DuplicateNameError
		/// </summary>
		/// <returns>FactTypeCollection</returns>
		protected override IList DuplicateElements
		{
			get
			{
				return FactTypeCollection;
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
				return ResourceStrings.ModelErrorModelHasDuplicateFactTypeNames;
			}
		}
	}
	public partial class ConstraintDuplicateNameError : DuplicateNameError
	{
		/// <summary>
		/// Get the duplicate elements represented by this DuplicateNameError
		/// </summary>
		/// <returns>ConstraintCollection</returns>
		protected override IList DuplicateElements
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
		private IList myCompositeList;
		/// <summary>
		/// Return a constraint collection encompassing
		/// single column external, multi column external, and internal constraints
		/// </summary>
		/// <value></value>
		public IList ConstraintCollection
		{
			get
			{
				IList retVal = myCompositeList;
				if (retVal == null)
				{
					myCompositeList = retVal = new CompositeCollection(this);
				}
				return retVal;
			}
		}
		private class CompositeCollection : IList
		{
			#region Member Variables
			private IList myList1;
			private IList myList2;
			private IList myList3;
			private IList myList4;
			#endregion // Member Variables
			#region Constructors
			public CompositeCollection(ConstraintDuplicateNameError error)
			{
				myList1 = error.MultiColumnExternalConstraintCollection;
				myList2 = error.SingleColumnExternalConstraintCollection;
				myList3 = error.InternalConstraintCollection;
				myList4 = error.ValueConstraintCollection;
			}
			#endregion // Constructors
			#region ICollection Implementation
			void ICollection.CopyTo(Array array, int index)
			{
				int baseIndex = index;
				myList1.CopyTo(array, baseIndex);
				baseIndex += myList1.Count;
				myList2.CopyTo(array, baseIndex);
				baseIndex += myList2.Count;
				myList3.CopyTo(array, baseIndex);
				baseIndex += myList3.Count;
				myList4.CopyTo(array, baseIndex);
			}
			int ICollection.Count
			{
				get
				{
					return myList1.Count + myList2.Count + myList3.Count + myList4.Count;
				}
			}
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}
			object ICollection.SyncRoot
			{
				get
				{
					throw new NotImplementedException();
				}
			}
			#endregion // ICollection Implementation
			#region IEnumerable Implementation
			IEnumerator IEnumerable.GetEnumerator()
			{
				foreach (object obj in myList1)
				{
					yield return obj;
				}
				foreach (object obj in myList2)
				{
					yield return obj;
				}
				foreach (object obj in myList3)
				{
					yield return obj;
				}
				foreach (object obj in myList4)
				{
					yield return obj;
				}
			}
			#endregion // IEnumerable Implementation
			#region IList Implementation
			bool IList.Contains(object value)
			{
				if (value is MultiColumnExternalConstraint)
				{
					if (myList1.Contains(value))
					{
						return true;
					}
				}
				else if (value is SingleColumnExternalConstraint)
				{
					if (myList2.Contains(value))
					{
						return true;
					}
				}
				else if (value is InternalConstraint)
				{
					return myList3.Contains(value);
				}
				else if (value is ValueConstraint)
				{
					return myList4.Contains(value);
				}
				return false;
			}
			int IList.IndexOf(object value)
			{
				int retVal = myList1.IndexOf(value);
				if (retVal == -1)
				{
					retVal = myList2.IndexOf(value);
					if (retVal != -1)
					{
						retVal += myList1.Count;
					}
					else
					{
						retVal = myList3.IndexOf(value);
						if (retVal != -1)
						{
							retVal += myList1.Count + myList2.Count;
						}
						else
						{
							retVal = myList4.IndexOf(value);
							if (retVal != -1)
							{
								retVal += myList1.Count + myList2.Count + myList3.Count;
							}
						}
					}
				}
				return retVal;
			}
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}
			bool IList.IsReadOnly
			{
				get
				{
					return true;
				}
			}
			object IList.this[int index]
			{
				get
				{
					int list1Count = myList1.Count;
					if (index >= list1Count)
					{
						index -= list1Count;
						int list2Count = myList2.Count;
						if (index >= list2Count)
						{
							index -= list2Count;
							int list3Count = myList3.Count;
							return (index >= list3Count) ? myList4[index - list3Count] : myList3[index];
						}
						return myList2[index];
					}
					return myList1[index];
				}
				set
				{
					int list1Count = myList1.Count;
					if (index >= list1Count)
					{
						index -= list1Count;
						int list2Count = myList2.Count;
						if (index >= list2Count)
						{
							index -= list2Count;
							int list3Count = myList3.Count;
							if (index >= list3Count)
							{
								myList4[index - list3Count] = value;
							}
							else
							{
								myList3[index] = value;
							}
						}
						else
						{
							myList2[index] = value;
						}
					}
					else
					{
						myList1[index] = value;
					}
				}
			}
			int IList.Add(object value)
			{
				InternalConstraint ic;
				MultiColumnExternalConstraint mcec;
				SingleColumnExternalConstraint scec;
				ValueConstraint vc;
				if (null != (ic = value as InternalConstraint))
				{
					return myList3.Add(ic) + myList1.Count + myList2.Count;
				}
				else if (null != (scec = value as SingleColumnExternalConstraint))
				{
					return myList2.Add(scec) + myList1.Count;
				}
				else if (null != (mcec = value as MultiColumnExternalConstraint))
				{
					return myList1.Add(mcec);
				}
				else if (null != (vc = value as ValueConstraint))
				{
					return myList4.Add(vc);
				}
				else
				{
					throw new InvalidCastException();
				}
			}
			void IList.Clear()
			{
				throw new NotImplementedException(); // Not supported for readonly list
			}
			void IList.Insert(int index, object value)
			{
				throw new NotImplementedException(); // Not supported for readonly list
			}
			void IList.Remove(object value)
			{
				throw new NotImplementedException(); // Not supported for readonly list
			}
			void IList.RemoveAt(int index)
			{
				throw new NotImplementedException(); // Not supported for readonly list
			}
			#endregion // IList Implementation
		}
		#endregion // ConstraintCollection Implementation
	}
	#endregion // Relationship-specific derivations of DuplicateNameError
	#endregion // NamedElementDictionary and DuplicateNameError integration
}	