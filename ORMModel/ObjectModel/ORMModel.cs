using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;

namespace Northface.Tools.ORM.ObjectModel
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
			return classInfo.IsDerivedFrom(RootType.MetaClassGuid) || classInfo.IsDerivedFrom(ExternalConstraint.MetaClassGuid);
		}
		/// <summary>
		/// Attach a deserialized ObjectType, FactType, or Constraint to the model.
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
			Constraint constraint;
			if (null != (objectType = sourceElement as ObjectType))
			{
				objectType.Model = this;
			}
			else if (null != (factType = sourceElement as FactType))
			{
				factType.Model = this;
			}
			else if (null != (constraint = sourceElement as Constraint))
			{
				constraint.Model = this;
			}
		}
		#endregion // MergeContext functions
		#region Deserialization Fixup
		/// <summary>
		/// Return all deserialization fixup listeners for the core object model
		/// </summary>
		[CLSCompliant(false)]
		public static IEnumerable<IDeserializationFixupListener> DeserializationFixupListeners
		{
			get
			{
				yield return InternalConstraint.FixupListener;
				yield return ExternalConstraint.FixupListener;
				yield return NamedElementDictionary.GetFixupListener((int)ORMDeserializationFixupPhase.AddImplicitElements);
				yield return ModelError.FixupListener;
				yield return ReferenceMode.FixupListener;
			}
		}
		#endregion // Deserialization Fixup
	}
	#region NamedElementDictionary and DuplicateNameError integration
	public partial class ORMModel : INamedElementDictionaryParent
	{
		#region INamedElementDictionaryParent implementation
		private NamedElementDictionary myObjectTypesDictionary = null;
		private NamedElementDictionary myFactTypesDictionary = null;
		private NamedElementDictionary myConstraintsDictionary = null;
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
				if (myObjectTypesDictionary == null)
				{
					myObjectTypesDictionary = new ObjectTypeNamedElementDictionary();
				}
				return myObjectTypesDictionary;
			}
			else if (parentMetaRoleGuid == ModelHasFactType.ModelMetaRoleGuid)
			{
				if (myFactTypesDictionary == null)
				{
					myFactTypesDictionary = new FactTypeNamedElementDictionary();
				}
				return myFactTypesDictionary;
			}
			else if (parentMetaRoleGuid == ModelHasConstraint.ModelMetaRoleGuid)
			{
				if (myConstraintsDictionary == null)
				{
					myConstraintsDictionary = new ConstraintNamedElementDictionary();
				}
				return myConstraintsDictionary;
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
		protected object GetAllowDuplicateNamesContextKey(Guid parentMetaRoleGuid, Guid childMetaRoleGuid)
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
		[RuleOn(typeof(ConstraintHasDuplicateNameError))]
		private class RemoveDuplicateConstraintNameErrorRule : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ConstraintHasDuplicateNameError link = e.ModelElement as ConstraintHasDuplicateNameError;
				ConstraintDuplicateNameError error = link.DuplicateNameError;
				if (!error.IsRemoved)
				{
					if (error.ConstraintCollection.Count < 2)
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
			protected override void RaiseDuplicateNameException(NamedElement element, string requestedName)
			{
				throw new InvalidOperationException(string.Format(ResourceStrings.ModelExceptionNameAlreadyUsedByModel, requestedName));
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
				return ResourceStrings.FactTypeDefaultNamePattern;
			}
			/// <summary>
			/// Raise an exception with text specific to a name in a model
			/// </summary>
			/// <param name="element">Element we're attempting to name</param>
			/// <param name="requestedName">The in-use requested name</param>
			protected override void RaiseDuplicateNameException(NamedElement element, string requestedName)
			{
				throw new InvalidOperationException(string.Format(ResourceStrings.ModelExceptionNameAlreadyUsedByModel, requestedName));
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
					Constraint constraint = (Constraint)element;
					if (afterTransaction)
					{
						// We're not in a transaction, but the object model will be in
						// the state we need it because we put it there during a transaction.
						// Just return the collection from the current state of the object model.
						ConstraintDuplicateNameError error = constraint.DuplicateNameError;
						return (error != null) ? error.ConstraintCollection : null;
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
								error = constraint.DuplicateNameError;
								if (error != null && !error.ValidateDuplicates(constraint))
								{
									error = null;
								}
							}
							if (error == null)
							{
								error = ConstraintDuplicateNameError.CreateConstraintDuplicateNameError(constraint.Store);
								constraint.DuplicateNameError = error;
								error.Model = constraint.Model;
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
							ConstraintMoveableCollection typedCollection = (ConstraintMoveableCollection)elementCollection;
							if (notifyAdded == null || !typedCollection.Contains(constraint))
							{
								typedCollection.Add(constraint);
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
						((Constraint)element).DuplicateNameError = null;
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
				Debug.Assert(element is Constraint);
				// UNDONE: How explicit do we want to be on constraint naming?
				return base.GetRootNamePattern(element);
			}
			/// <summary>
			/// Raise an exception with text specific to a name in a model
			/// </summary>
			/// <param name="element">Element we're attempting to name</param>
			/// <param name="requestedName">The in-use requested name</param>
			protected override void RaiseDuplicateNameException(NamedElement element, string requestedName)
			{
				throw new InvalidOperationException(string.Format(ResourceStrings.ModelExceptionNameAlreadyUsedByModel, requestedName));
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
		#endregion // INamedElementDictionaryLink implementation
	}
	public partial class ModelHasConstraint : INamedElementDictionaryLink
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
			get { return ConstraintCollection as ExternalConstraint; }
		}
		#endregion // INamedElementDictionaryLink implementation
	}
	public abstract partial class DuplicateNameError :  IRepresentModelElements, IModelErrorOwner
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
			string newText = string.Format(ErrorFormatText, modelName, elementName);
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
	}
	#endregion // Relationship-specific derivations of DuplicateNameError
	#endregion // NamedElementDictionary and DuplicateNameError integration
}	