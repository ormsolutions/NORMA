#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © Matthew Curland. All rights reserved.                        *
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
		AddIntrinsicElements = StandardFixupPhase.AddIntrinsicElements,
		/// <summary>
		/// Verify any implied elements that are serialized with the model
		/// but must follow a proscribed pattern based on another serialized element.
		/// This stage may both add and remove elements.
		/// </summary>
		ValidateImplicitStoredElements = StandardFixupPhase.ValidateImplicitStoredElements,
		/// <summary>
		/// Add implicit elements at this stage. An implicit element is
		/// not serialized and is generally created by a rule once the model
		/// is loaded.
		/// </summary>
		AddImplicitElements = StandardFixupPhase.AddImplicitElements,
		/// <summary>
		/// Element names should be tracked and validated after
		/// all intrinsic, implicitstored, and implicit elements are in place.
		/// </summary>
		ValidateElementNames = StandardFixupPhase.LastModelElementPhase + 100,
		/// <summary>
		/// Model errors are stored with the model, but are vulnerable
		/// to the Notepad effect, which can cause errors to be added
		/// or removed from the model. Validate errors after all other
		/// explicit, intrinsic, and implicit elements are in place.
		/// </summary>
		ValidateErrors = StandardFixupPhase.LastModelElementPhase + 200,
		/// <summary>
		/// Fixup stored presentation elements
		/// </summary>
		ValidateStoredPresentationElements = StandardFixupPhase.ValidateStoredPresentationElements,
		/// <summary>
		/// Add any presentation elements that are implicit and not
		/// serialized with the model.
		/// </summary>
		AddImplicitPresentationElements = StandardFixupPhase.AddImplicitPresentationElements,
	}
	#endregion // ORMDeserializationFixupPhase enum
	partial class ORMModelBase
	{
		#region CustomStorage handlers
		private string GetDefinitionTextValue()
		{
			Definition currentDefinition = Definition;
			return (currentDefinition != null) ? currentDefinition.Text : String.Empty;
		}
		private string GetNoteTextValue()
		{
			Note currentNote = this.Note;
			return (currentNote != null) ? currentNote.Text : String.Empty;
		}
		private void SetDefinitionTextValue(string newValue)
		{
			if (!Store.InUndoRedoOrRollback)
			{
				Definition definition = Definition;
				if (definition != null)
				{
					definition.Text = newValue;
				}
				else if (!string.IsNullOrEmpty(newValue))
				{
					Definition = new Definition(Store, new PropertyAssignment(Definition.TextDomainPropertyId, newValue));
				}
			}
		}
		private void SetNoteTextValue(string newValue)
		{
			if (!Store.InUndoRedoOrRollback)
			{
				Note note = Note;
				if (note != null)
				{
					note.Text = newValue;
				}
				else if (!string.IsNullOrEmpty(newValue))
				{
					Note = new Note(Store, new PropertyAssignment(Note.TextDomainPropertyId, newValue));
				}
			}
		}
		private ModelErrorDisplayFilter GetModelErrorDisplayFilterDisplayValue()
		{
			return ModelErrorDisplayFilter;
		}
		private void SetModelErrorDisplayFilterDisplayValue(ModelErrorDisplayFilter newValue)
		{
			if (!Store.InUndoRedoOrRollback)
			{
				ModelErrorDisplayFilter = newValue;
			}
		}
		#endregion // CustomStorage handlers
		#region MergeContext functions
		private void MergeRelateObjectType(ModelElement sourceElement, ElementGroup elementGroup)
		{
			ObjectType objectType = sourceElement as ObjectType;
			if (elementGroup.UserData == ORMModel.ValueTypeUserDataKey)
			{
				objectType.DataType = ((ORMModel)this).DefaultDataType;
			}
			this.ObjectTypeCollection.Add(objectType);
		}
		private void MergeDisconnectObjectType(ModelElement sourceElement)
		{
			ObjectType objectType = sourceElement as ObjectType;
			// Delete link for path ModelHasObjectType.ObjectTypeCollection
			foreach (ElementLink link in ModelHasObjectType.GetLinks((ORMModel)this, objectType))
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
	partial class ORMModel : IVerbalizeCustomChildren, IVerbalizeFilterChildrenByRole
	{
		#region ElementGroup.UserData keys
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
		#endregion // ElementGroup.UserData keys
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
		#region Event integration
		/// <summary>
		/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> for state in the
		/// model that is not maintained automatically in the store.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="eventManager">The <see cref="ModelingEventManager"/> used to manage the <see cref="EventHandler{TEventArgs}"/>s.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		public static void ManageModelStateEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{
			ManageReferenceModeModelStateEventHandlers(store, eventManager, action);
		}
		#endregion // Event integration
		#region IVerbalizeCustomChildren Implementation
		/// <summary>
		/// Implements <see cref="IVerbalizeCustomChildren.GetCustomChildVerbalizations"/>.
		/// Explicitly verbalizes the definitions and notes fields
		/// </summary>
		protected IEnumerable<CustomChildVerbalizer> GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, bool isNegative)
		{
			Definition definition;
			if (null != (definition = Definition) &&
				(filter == null || !filter.FilterChildVerbalizer(definition, isNegative).IsBlocked))
			{
				yield return CustomChildVerbalizer.VerbalizeInstance(definition);
			}
			Note note = Note;
			if (null != (note = Note) &&
				(filter == null || !filter.FilterChildVerbalizer(note, isNegative).IsBlocked))
			{
				yield return CustomChildVerbalizer.VerbalizeInstance(note);
			}
		}
		IEnumerable<CustomChildVerbalizer> IVerbalizeCustomChildren.GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, bool isNegative)
		{
			return GetCustomChildVerbalizations(filter, isNegative);
		}
		#endregion // IVerbalizeCustomChildren Implementation
		#region IVerbalizeFilterChildrenByRole Implementation
		/// <summary>
		/// Implements <see cref="IVerbalizeFilterChildrenByRole.BlockEmbeddedVerbalization"/>.
		/// All relationships of the core domain model are blocked, with individal elements
		/// turned back on via the <see cref="IVerbalizeCustomChildren"/> implementation.
		/// </summary>
		protected bool BlockEmbeddedVerbalization(DomainRoleInfo embeddingRole)
		{
			return embeddingRole.DomainModel.Id == ORMCoreDomainModel.DomainModelId;
		}
		bool IVerbalizeFilterChildrenByRole.BlockEmbeddedVerbalization(DomainRoleInfo embeddingRole)
		{
			return BlockEmbeddedVerbalization(embeddingRole);
		}
		#endregion // IVerbalizeFilterChildrenByRole Implementation
	}
	#region Indirect merge support
	partial class ORMModel
	{
		/// <summary>
		/// Forward <see cref="IMergeElements.CanMerge"/> requests to <see cref="IMergeIndirectElements{ORMModel}"/> extensions
		/// </summary>
		protected override bool CanMerge(ProtoElementBase rootElement, ElementGroupPrototype elementGroupPrototype)
		{
			bool retVal = base.CanMerge(rootElement, elementGroupPrototype);
			if (!retVal)
			{
				IMergeIndirectElements<ORMModel>[] extenders = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IMergeIndirectElements<ORMModel>>();
				if (extenders != null)
				{
					for (int i = 0; i < extenders.Length && !retVal; ++i)
					{
						retVal = extenders[i].CanMergeIndirect(this, rootElement, elementGroupPrototype);
					}
				}
			}
			return retVal;
		}
		/// <summary>
		/// Forward <see cref="IMergeElements.ChooseMergeTarget(ElementGroup)"/> requests to <see cref="IMergeIndirectElements{ORMModel}"/> extensions
		/// </summary>
		protected override ModelElement ChooseMergeTarget(ElementGroup elementGroup)
		{
			ModelElement retVal = base.ChooseMergeTarget(elementGroup);
			if (retVal == null)
			{
				IMergeIndirectElements<ORMModel>[] extenders = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IMergeIndirectElements<ORMModel>>();
				if (extenders != null)
				{
					for (int i = 0; i < extenders.Length && retVal == null; ++i)
					{
						retVal = extenders[i].ChooseIndirectMergeTarget(this, elementGroup);
					}
				}
			}
			return retVal;
		}
		/// <summary>
		/// Forward <see cref="IMergeElements.ChooseMergeTarget(ElementGroupPrototype)"/> requests to <see cref="IMergeIndirectElements{ORMModel}"/> extensions
		/// </summary>
		protected override ModelElement ChooseMergeTarget(ElementGroupPrototype elementGroupPrototype)
		{
			ModelElement retVal = base.ChooseMergeTarget(elementGroupPrototype);
			if (retVal == null)
			{
				IMergeIndirectElements<ORMModel>[] extenders = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IMergeIndirectElements<ORMModel>>();
				if (extenders != null)
				{
					for (int i = 0; i < extenders.Length && retVal == null; ++i)
					{
						retVal = extenders[i].ChooseIndirectMergeTarget(this, elementGroupPrototype);
					}
				}
			}
			return retVal;
		}
		/// <summary>
		/// Forward <see cref="IMergeElements.MergeRelate"/> requests to <see cref="IMergeIndirectElements{ORMModel}"/> extensions
		/// </summary>
		protected override void MergeRelate(ModelElement sourceElement, ElementGroup elementGroup)
		{
			IMergeIndirectElements<ORMModel>[] extenders = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IMergeIndirectElements<ORMModel>>();
			if (extenders != null)
			{
				for (int i = 0; i < extenders.Length; ++i)
				{
					if (extenders[i].MergeRelateIndirect(this, sourceElement, elementGroup))
					{
						return;
					}
				}
			}
			base.MergeRelate(sourceElement, elementGroup);
		}
		/// <summary>
		/// Forward <see cref="IMergeElements.MergeDisconnect"/> requests to <see cref="IMergeIndirectElements{ORMModel}"/> extensions
		/// </summary>
		protected override void MergeDisconnect(ModelElement sourceElement)
		{
			IMergeIndirectElements<ORMModel>[] extenders = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IMergeIndirectElements<ORMModel>>();
			if (extenders != null)
			{
				for (int i = 0; i < extenders.Length; ++i)
				{
					if (extenders[i].MergeDisconnectIndirect(this, sourceElement))
					{
						return;
					}
				}
			}
			base.MergeDisconnect(sourceElement);
		}
		/// <summary>
		/// Forward <see cref="IMergeElements.MergeConfigure"/> requests to <see cref="IMergeIndirectElements{ORMModel}"/> extensions
		/// </summary>
		protected override void MergeConfigure(ElementGroup elementGroup)
		{
			IMergeIndirectElements<ORMModel>[] extenders = ((IFrameworkServices)Store).GetTypedDomainModelProviders<IMergeIndirectElements<ORMModel>>();
			if (extenders != null)
			{
				for (int i = 0; i < extenders.Length; ++i)
				{
					if (extenders[i].MergeConfigureIndirect(this, elementGroup))
					{
						return;
					}
				}
			}
			base.MergeConfigure(elementGroup);
		}
	}
	#endregion // Indirect merge support
	#region NamedElementDictionary and DuplicateNameError integration
	public partial class ORMModel : INamedElementDictionaryParent
	{
		#region Public token values
		/// <summary>
		/// A key to set in the top-level transaction context to indicate that
		/// we should generate duplicate name errors for like-named objects or constraints
		/// instead of throwing an exception.
		/// </summary>
		public static readonly object AllowDuplicateNamesKey = new object();
		/// <summary>
		/// A key to set in the top-level transaction context to indicate that
		/// we should generate duplicate name errors for like-named objects instead of
		/// throwing an exception.
		/// </summary>
		public static readonly object AllowDuplicateConstraintNamesKey = new object();
		#endregion // Public token values
		#region INamedElementDictionaryParent implementation
		[NonSerialized]
		private NamedElementDictionary myObjectTypesDictionary;
		[NonSerialized]
		private NamedElementDictionary myConstraintsDictionary;
		[NonSerialized]
		private RecognizedPhraseNamedElementDictionary myRecognizedPhrasesDictionary;
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

		/// <summary>
		/// Returns the Recognized Word Dictionary
		/// </summary>
		/// <value>The model RecognizedPhraseDictionary</value>
		public INamedElementDictionary RecognizedPhrasesDictionary
		{
			get
			{
				INamedElementDictionary retVal = myRecognizedPhrasesDictionary;
				if (retVal == null)
				{
					retVal = myRecognizedPhrasesDictionary = new RecognizedPhraseNamedElementDictionary();
				}
				return retVal;
			}
		}
		/// <summary>
		/// Get the <see cref="RecognizedPhrase"/> elements starting with a specific <paramref name="startingWord"/>
		/// within a <paramref name="nameGenerator"/> context
		/// </summary>
		/// <param name="startingWord">The initial word to test</param>
		/// <param name="nameGenerator">The <see cref="NameGenerator"/> context to retrieve an abbreviation for</param>
		/// <returns>An enumeration of <see cref="NameAlias"/> elements. The corresponding <see cref="RecognizedPhrase"/>
		/// can be return from the <see cref="NameAlias.Element"/> property</returns>
		public IEnumerable<NameAlias> GetRecognizedPhrasesStartingWith(string startingWord, NameGenerator nameGenerator)
		{
			RecognizedPhraseNamedElementDictionary dictionary = myRecognizedPhrasesDictionary;
			if (dictionary != null)
			{
				ModelElement singleElement;
				NameAlias alias;
				LocatedElement matchedPhrase = ((INamedElementDictionary)dictionary).GetElement(startingWord);
				if (!matchedPhrase.IsEmpty)
				{
					singleElement = matchedPhrase.SingleElement;
					if (singleElement != null)
					{
						alias = nameGenerator.FindMatchingAlias(((RecognizedPhrase)singleElement).AbbreviationCollection);
						if (alias != null)
						{
							yield return alias;
						}
					}
					else
					{
						foreach (ModelElement element in matchedPhrase.MultipleElements)
						{
							alias = nameGenerator.FindMatchingAlias(((RecognizedPhrase)element).AbbreviationCollection);
							if (alias != null)
							{
								yield return alias;
							}
						}
					}
				}
				matchedPhrase = dictionary.GetMultiWordPhrasesStartingWith(startingWord);
				if (!matchedPhrase.IsEmpty)
				{
					singleElement = matchedPhrase.SingleElement;
					if (singleElement != null)
					{
						alias = nameGenerator.FindMatchingAlias(((RecognizedPhrase)singleElement).AbbreviationCollection);
						if (alias != null)
						{
							yield return alias;
						}
					}
					else
					{
						foreach (ModelElement element in matchedPhrase.MultipleElements)
						{
							alias = nameGenerator.FindMatchingAlias(((RecognizedPhrase)element).AbbreviationCollection);
							if (alias != null)
							{
								yield return alias;
							}
						}
					}
				}
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
		protected INamedElementDictionary GetCounterpartRoleDictionary(Guid parentDomainRoleId, Guid childDomainRoleId)
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
			else if (parentDomainRoleId == ModelContainsRecognizedPhrase.RecognizedPhraseDomainRoleId)
			{
				return RecognizedPhrasesDictionary;
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
				contextInfo.ContainsKey(ORMModel.AllowDuplicateNamesKey))
			{
				// Use their value so they don't have to look up ours again
				retVal = NamedElementDictionary.AllowDuplicateNamesKey;
			}
			return retVal;
		}
		#endregion // INamedElementDictionaryParent implementation
		#region Rules to remove duplicate name errors
		/// <summary>
		/// DeleteRule: typeof(ObjectTypeHasDuplicateNameError)
		/// </summary>
		private static void DuplicateObjectTypeNameObjectTypeDeleteRule(ElementDeletedEventArgs e)
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
		/// <summary>
		/// DeleteRule: typeof(SetComparisonConstraintHasDuplicateNameError)
		/// DeleteRule: typeof(SetConstraintHasDuplicateNameError)
		/// DeleteRule: typeof(ValueConstraintHasDuplicateNameError)
		/// </summary>
		private static void DuplicateConstraintNameConstraintDeleteRule(ElementDeletedEventArgs e)
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

		/// <summary>
		/// DeleteRule: typeof(RecognizedPhraseHasDuplicateNameError)
		/// </summary>
		private static void DuplicateRecognizedPhraseDeleteRule(ElementDeletedEventArgs e)
		{
			RecognizedPhraseHasDuplicateNameError link = e.ModelElement as RecognizedPhraseHasDuplicateNameError;
			RecognizedPhraseDuplicateNameError error = link.DuplicateNameError;
			if (!error.IsDeleted)
			{
				if (error.RecognizedPhraseCollection.Count < 2)
				{
					error.Delete();
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
			public ObjectTypeNamedElementDictionary()
				: base(new DuplicateNameManager())
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
					return nestedFact.DefaultName;
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
			public ConstraintNamedElementDictionary()
				: base(new DuplicateNameManager())
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
				MandatoryConstraint mandatoryConstraint;
				if (null != (mandatoryConstraint = element as MandatoryConstraint))
				{
					if (mandatoryConstraint.ExclusiveOrExclusionConstraint != null)
					{
						// Use the normal class name, not the one modified for the property grid.
						// Note that we let the normal one (ExclusiveOrConstraint) go through
						// for the exclusion constraint.
						return ResourceStrings.DisjunctiveMandatoryConstraint;
					}
				}
				// UNDONE: How explicit do we want to be on constraint naming? Note that if this is changed, then
				// we also need to update the ValueRange.DataTypeDeleting rule, which assumes the base implementation.
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
		#region RecognizedPhraseNamedElementDictionary class
		/// <summary>
		/// Callback method to retrieve an existing alias owner
		/// </summary>
		/// <param name="container">An <see cref="ORMModel"/> element</param>
		/// <param name="recognizedPhraseName">The name for an existing element</param>
		/// <returns>A <see cref="RecognizedPhrase"/> or <see langword="null"/></returns>
		private static ModelElement GetExistingRecognizedPhrase(ModelElement container, string recognizedPhraseName)
		{
			ORMModel model;
			INamedElementDictionary phraseDictionary;
			if (null != (model = container as ORMModel) &&
				null != (phraseDictionary = model.myRecognizedPhrasesDictionary))
			{
				LocatedElement existingElement = phraseDictionary.GetElement(recognizedPhraseName);
				if (!existingElement.IsEmpty)
				{
					return existingElement.SingleElement as RecognizedPhrase;
				}
			}
			return null;
		}
		/// <summary>
		/// Dictionary used to add <see cref="RecognizedPhrase"/> elements by name.
		/// Also generates model validation errors and exceptions for duplicate
		/// element names.
		/// </summary>
		protected class RecognizedPhraseNamedElementDictionary : NamedElementDictionary, INamedElementDictionary
		{	
			private sealed class DuplicateNameManager : IDuplicateNameCollectionManager
			{
				#region TrackingList class
				private sealed class TrackingList : List<RecognizedPhrase>
				{
					private readonly LinkedElementCollection<RecognizedPhrase> myRecognizedPhrases;
					public TrackingList(RecognizedPhraseDuplicateNameError error)
					{
						myRecognizedPhrases = error.RecognizedPhraseCollection;
					}
					public LinkedElementCollection<RecognizedPhrase> RecognizedPhrases
					{
						get { return myRecognizedPhrases; }
					}
				}
				#endregion //TrackingList class
				#region IDuplicateNameCollectionManager Implementation

				public ICollection OnDuplicateElementAdded(ICollection elementCollection, ModelElement element, bool afterTransaction, INotifyElementAdded notifyAdded)
				{
					RecognizedPhrase recognizedPhrase = (RecognizedPhrase)element;
					if (afterTransaction)
					{
						if (elementCollection == null)
						{
							RecognizedPhraseDuplicateNameError error = recognizedPhrase.DuplicateNameError;
							if (error != null)
							{
								// We're not in a transaction, but the object model will be in
								// the state we need it because we put it there during a transaction.
								// Just return the collection from the current state of the object model.
								TrackingList trackingList = new TrackingList(error);
								ModelElement existingElement = GetExistingRecognizedPhrase(element, recognizedPhrase.Name);
								if (existingElement == null)
								{
									trackingList.Add(recognizedPhrase);
								}
								elementCollection = trackingList;
							}
						}
						else
						{
							if (!((IList)elementCollection).Contains(recognizedPhrase))
							{
								((TrackingList)elementCollection).Add(recognizedPhrase);
							}
						}
						return elementCollection;
					}
					else
					{
						// Modify the object model to add the error.
						if (elementCollection == null)
						{
							RecognizedPhraseDuplicateNameError error = null;
							if (notifyAdded != null)
							{
								// During deserialization fixup, an error
								// may already be attached to the object. Track
								// it down and verify that it is a legitimate error.
								// If it is not legitimate, then generate a new one.
								error = recognizedPhrase.DuplicateNameError;
								if (error != null && !error.ValidateDuplicates(recognizedPhrase))
								{
									error = null;
								}
							}
							if (error == null)
							{
								error = new RecognizedPhraseDuplicateNameError(recognizedPhrase.Store);
								recognizedPhrase.DuplicateNameError = error;
								error.Model = recognizedPhrase.Model;
								error.GenerateErrorText();
								if (notifyAdded != null)
								{
									notifyAdded.ElementAdded(error, true);
								}
							}
							TrackingList trackingList = new TrackingList(error);
							trackingList.Add(recognizedPhrase);
							elementCollection = trackingList;
						}
						else
						{
							TrackingList trackingList = (TrackingList)elementCollection;
							trackingList.Add(recognizedPhrase);
							// During deserialization fixup (notifyAdded != null), we need
							// to make sure that the element is not already in the collection
							LinkedElementCollection<RecognizedPhrase> typedCollection = trackingList.RecognizedPhrases;
							if (notifyAdded == null || !typedCollection.Contains(recognizedPhrase))
							{
								typedCollection.Add(recognizedPhrase);
							}
						}
						return elementCollection;
					}
				}

				public ICollection OnDuplicateElementRemoved(ICollection elementCollection, ModelElement element, bool afterTransaction)
				{
					TrackingList trackingList = (TrackingList)elementCollection;
					RecognizedPhrase recognizedPhrase = (RecognizedPhrase)element;
					trackingList.Remove(recognizedPhrase);
					if (!afterTransaction)
					{
						// Just clear the error. A rule is used to remove the error
						// object itself when there is no longer a duplicate.
						recognizedPhrase.DuplicateNameError = null;
					}
					return elementCollection;

				}

				#endregion // IDuplicateNameCollectionManager Implementation
			}
			/// <summary>
			/// Public constructor
			/// </summary>
			public RecognizedPhraseNamedElementDictionary()
				:
				base(new DuplicateNameManager(), StringComparer.CurrentCultureIgnoreCase)
			{
			}
			#region Base overrides
			/// <summary>
			/// Raise an exception with text specific to a name in a model
			/// </summary>
			/// <param name="element">Element we're attempting to name</param>
			/// <param name="requestedName">The in-use requested name</param>
			protected override void ThrowDuplicateNameException(ModelElement element, string requestedName)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorModelHasDuplicateRecognizedPhrases, ((RecognizedPhrase)element).Model.Name,  requestedName));
			}
			#endregion // Base overrides
			#region INamedElementDictionary Reimplementation
			#region StartsWith helper methods and fields
			private Dictionary<string, LocatedElement> myStartsWithWordDictionary;
			/// <summary>
			/// A supplemental lookup to the <see cref="ORMModel.RecognizedPhrasesDictionary"/> that
			/// looks up multi-word phrases based on the starting word
			/// </summary>
			/// <param name="startingWord">The first word in the phrase</param>
			public LocatedElement GetMultiWordPhrasesStartingWith(string startingWord)
			{
				Dictionary<string, LocatedElement> startsWithDictionary;
				LocatedElement retVal;
				if (null != (startsWithDictionary = myStartsWithWordDictionary) &&
					startsWithDictionary.TryGetValue(startingWord, out retVal))
				{
					return retVal;
				}
				return LocatedElement.Empty;
			}
			private void AddToStartsWith(RecognizedPhrase phrase, string startsWithName)
			{
				Dictionary<string, LocatedElement> startsWithDictionary = myStartsWithWordDictionary;
				if (startsWithDictionary == null)
				{
					myStartsWithWordDictionary = startsWithDictionary = new Dictionary<string, LocatedElement>(StringComparer.CurrentCultureIgnoreCase);
					startsWithDictionary.Add(startsWithName, new LocatedElement(phrase));
				}
				else
				{
					LocatedElement existingElement;
					if (startsWithDictionary.TryGetValue(startsWithName, out existingElement))
					{
						ModelElement currentSingle = existingElement.SingleElement;
						if (currentSingle == null)
						{
							((List<RecognizedPhrase>)existingElement.MultipleElements).Add(phrase);
						}
						else
						{
							startsWithDictionary[startsWithName] = new LocatedElement(new List<RecognizedPhrase>(new RecognizedPhrase[] { (RecognizedPhrase)currentSingle, phrase }));
						}
					}
					else
					{
						startsWithDictionary.Add(startsWithName, new LocatedElement(phrase));
					}
				}
			}
			private void RemoveFromStartsWith(RecognizedPhrase phrase, string startsWithName)
			{
				Dictionary<string, LocatedElement> startsWithDictionary;
				LocatedElement existingElement;
				if (null != (startsWithDictionary = myStartsWithWordDictionary) &&
					startsWithDictionary.TryGetValue(startsWithName, out existingElement))
				{
					ModelElement currentSingle = existingElement.SingleElement;
					if (currentSingle != null)
					{
						startsWithDictionary.Remove(startsWithName);
					}
					else
					{
						List<RecognizedPhrase> phrases = (List<RecognizedPhrase>)existingElement.MultipleElements;
						phrases.Remove(phrase);
						if (phrases.Count == 1)
						{
							startsWithDictionary[startsWithName] = new LocatedElement(phrases[0]);
						}
					}
				}
			}
			#endregion // StartsWith helper methods and fields
			/// <summary>
			/// Implements <see cref="INamedElementDictionary.AddElement"/>
			/// </summary>
			protected new void AddElement(ModelElement element, DuplicateNameAction duplicateAction, INotifyElementAdded notifyAdded)
			{
				base.AddElement(element, duplicateAction, notifyAdded);
				RecognizedPhrase phrase = (RecognizedPhrase)element;
				string newName = phrase.Name;
				int spaceIndex = newName.IndexOf(' '); // Note that spaces are normalized well before this point
				if (spaceIndex != -1)
				{
					AddToStartsWith(phrase, newName.Substring(0, spaceIndex));
				}
			}
			void INamedElementDictionary.AddElement(ModelElement element, DuplicateNameAction duplicateAction, INotifyElementAdded notifyAdded)
			{
				AddElement(element, duplicateAction, notifyAdded);
			}
			/// <summary>
			/// Implements <see cref="INamedElementDictionary.RemoveElement"/>
			/// </summary>
			protected new bool RemoveElement(ModelElement element, string alternateElementName, DuplicateNameAction duplicateAction)
			{
				if (base.RemoveElement(element, alternateElementName, duplicateAction))
				{
					RecognizedPhrase phrase = (RecognizedPhrase)element;
					string removeName = alternateElementName ?? phrase.Name;
					int spaceIndex = removeName.IndexOf(' '); // Note that spaces are normalized well before this point
					if (spaceIndex != -1)
					{
						RemoveFromStartsWith(phrase, removeName.Substring(0, spaceIndex));
					}
					return true;
				}
				return false;
			}
			bool INamedElementDictionary.RemoveElement(ModelElement element, string alternateElementName, DuplicateNameAction duplicateAction)
			{
				return RemoveElement(element, alternateElementName, duplicateAction);
			}
			/// <summary>
			/// Implements <see cref="INamedElementDictionary.ReplaceElement"/>
			/// </summary>
			protected new void ReplaceElement(ModelElement originalElement, ModelElement replacementElement, DuplicateNameAction duplicateAction)
			{
				// Note that this is implemented the same as the base class
				RemoveElement(originalElement, null, duplicateAction);
				AddElement(replacementElement, duplicateAction, null);
			}
			void INamedElementDictionary.ReplaceElement(ModelElement originalElement, ModelElement replacementElement, DuplicateNameAction duplicateAction)
			{
				ReplaceElement(originalElement, replacementElement, duplicateAction);
			}
			/// <summary>
			/// Implements <see cref="INamedElementDictionary.RenameElement"/>
			/// </summary>
			protected new void RenameElement(ModelElement element, string oldName, string newName, DuplicateNameAction duplicateAction)
			{
				base.RenameElement(element, oldName, newName, duplicateAction);
				RecognizedPhrase phrase = (RecognizedPhrase)element;
			}
			void INamedElementDictionary.RenameElement(ModelElement element, string oldName, string newName, DuplicateNameAction duplicateAction)
			{
				RenameElement(element, oldName, newName, duplicateAction);
				string oldStartsWith = null;
				RecognizedPhrase phrase = (RecognizedPhrase)element;
				int oldSpaceIndex = oldName.IndexOf(' '); // Note that spaces are normalized well before this point
				int newSpaceIndex = newName.IndexOf(' ');
				if (oldSpaceIndex != -1)
				{
					oldStartsWith = oldName.Substring(0, oldSpaceIndex);
				}
				if (newSpaceIndex != -1)
				{
					string newStartsWith = newName.Substring(0, newSpaceIndex);
					if (oldStartsWith != null &&
						StringComparer.CurrentCultureIgnoreCase.Equals(oldStartsWith, newStartsWith))
					{
						// Nothing to do, the leading and trailing phrases are the same
						oldStartsWith = null;
					}
					else
					{
						AddToStartsWith(phrase, newStartsWith);
					}
				}
				if (oldStartsWith != null)
				{
					RemoveFromStartsWith(phrase, oldStartsWith);
				}
			}
			#endregion // INamedElementDictionary Reimplementation
		}
		#endregion // RecognizedPhraseNamedElementDictionary class
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
	public partial class ModelContainsRecognizedPhrase : INamedElementDictionaryLink
	{
		#region INamedElementDictionaryLink implementation
		INamedElementDictionaryParent INamedElementDictionaryLink.ParentRolePlayer
		{
			get { return ParentRolePlayer; }
		}
		/// <summary>
		/// Implements <see cref="INamedElementDictionaryLink.ParentRolePlayer"/>
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
		/// Implements <see cref="INamedElementDictionaryLink.ChildRolePlayer"/>
		/// Returns <see cref="RecognizedPhrase"/>.
		/// </summary>
		protected INamedElementDictionaryChild ChildRolePlayer
		{
			get { return RecognizedPhrase; }
		}
		INamedElementDictionaryRemoteParent INamedElementDictionaryLink.RemoteParentRolePlayer
		{
			get { return RemoteParentRolePlayer; }
		}
		/// <summary>
		/// Implements <see cref="INamedElementDictionaryLink.RemoteParentRolePlayer"/>
		/// Returns <see langword="null"/>
		/// </summary>
		protected INamedElementDictionaryRemoteParent RemoteParentRolePlayer
		{
			get { return null; }
		}
		#endregion // INamedElementDictionaryLink implementation
	}
	public partial class RecognizedPhrase : INamedElementDictionaryChild
	{
		#region INamedElementDictionaryChild implementation
		void INamedElementDictionaryChild.GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			GetRoleGuids(out parentDomainRoleId, out childDomainRoleId);
		}
		/// <summary>
		/// Implementation of <see cref="INamedElementDictionaryChild.GetRoleGuids"/>. Identifies
		/// this child as participating in the 'NameGeneratorContainsRecognizedPhrase' naming set.
		/// </summary>
		protected static void GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			parentDomainRoleId = ModelContainsRecognizedPhrase.RecognizedPhraseDomainRoleId;
			childDomainRoleId = ModelContainsRecognizedPhrase.RecognizedPhraseDomainRoleId;
		}
		#endregion // INamedElementDictionaryChild implementation
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
		protected abstract IList<ModelElement> DuplicateElements { get;}
		/// <summary>
		/// Get the text to display the duplicate error information. Replacement
		/// field {0} is replaced by the model name, field {1} is replaced by the
		/// element name.
		/// </summary>
		protected abstract string ErrorFormatText { get;}
		/// <summary>
		/// Get the name of an element. The default implementation uses
		/// the <see cref="DomainClassInfo.NameDomainProperty"/> to determine
		/// the name. Derived classes can produce a more efficient implementation
		/// if they know the actual element type.
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		protected virtual string GetElementName(ModelElement element)
		{
			DomainPropertyInfo nameProperty = element.GetDomainClass().NameDomainProperty;
			Debug.Assert(nameProperty != null, "Duplicate names should only be checked on elements with names");
			return nameProperty.GetValue(element) as string;
		}
		/// <summary>
		/// Verify that all of the duplicate elements attached to
		/// this error actually have the same name.
		/// </summary>
		/// <returns>true if validation succeeded. false is
		/// returned if testElement does not have a name specified</returns>
		public bool ValidateDuplicates(ModelElement testElement)
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
		private bool ValidateDuplicates(ModelElement testElement, IList<ModelElement> duplicates)
		{
			string testName = GetElementName(testElement);
			if (testName.Length > 0)
			{
				if (duplicates == null)
				{
					duplicates = DuplicateElements;
				}
				int duplicatesCount = duplicates.Count;
				for (int i = 0; i < duplicatesCount; ++i)
				{
					ModelElement compareTo = duplicates[i];
					if (compareTo != testElement && GetElementName(compareTo) != testName)
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
			IList<ModelElement> elements = DuplicateElements;
			string elementName = (elements.Count != 0) ? GetElementName(elements[0]) : string.Empty;
			ORMModel model = Model;
			string modelName = (model != null) ? model.Name : string.Empty;
			string newText = string.Format(CultureInfo.InvariantCulture, ErrorFormatText, modelName, elementName);
			string currentText = ErrorText;
			if (currentText != newText)
			{
				ErrorText = newText;
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
		/// Implements <see cref="IRepresentModelElements.GetRepresentedElements"/>
		/// </summary>
		protected new ModelElement[] GetRepresentedElements()
		{
			// Pick up all roles played directly by this element. This
			// will get ObjectTypeCollection, FactTypeCollection, etc, but
			// not the owning model. These are non-aggregating roles.
			IList<ModelElement> elements = DuplicateElements;
			int count = elements.Count;
			if (count == 0)
			{
				return null;
			}
			ModelElement[] retVal = elements as ModelElement[];
			if (retVal == null)
			{
				retVal = new ModelElement[count];
				elements.CopyTo(retVal, 0);
			}
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
				IList<ModelElement> duplicates = DuplicateElements;
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
	[ModelErrorDisplayFilter(typeof(NameErrorCategory))]
	public partial class ObjectTypeDuplicateNameError : DuplicateNameError, IHasIndirectModelErrorOwner
	{
		/// <summary>
		/// Get the duplicate elements represented by this DuplicateNameError
		/// </summary>
		/// <returns>ObjectTypeCollection</returns>
		protected override IList<ModelElement> DuplicateElements
		{
			get
			{
				return ObjectTypeCollection.ToArray();
			}
		}
		/// <summary>
		/// Provide an efficient name lookup
		/// </summary>
		protected override string GetElementName(ModelElement element)
		{
			return ((ORMNamedElement)element).Name;
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
	[ModelErrorDisplayFilter(typeof(NameErrorCategory))]
	public partial class ConstraintDuplicateNameError : DuplicateNameError, IHasIndirectModelErrorOwner
	{
		/// <summary>
		/// Get the duplicate elements represented by this DuplicateNameError
		/// </summary>
		/// <returns>ConstraintCollection</returns>
		protected override IList<ModelElement> DuplicateElements
		{
			get
			{
				return ConstraintCollection as IList<ModelElement>;
			}
		}
		/// <summary>
		/// Provide an efficient name lookup
		/// </summary>
		protected override string GetElementName(ModelElement element)
		{
			return ((ORMNamedElement)element).Name;
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
	[ModelErrorDisplayFilter(typeof(NameErrorCategory))]
	public partial class RecognizedPhraseDuplicateNameError : DuplicateNameError, IHasIndirectModelErrorOwner
	{
		/// <summary>
		/// Returns the list of DuplicateElements 
		/// </summary>
		protected override IList<ModelElement> DuplicateElements
		{
			get { return RecognizedPhraseCollection.ToArray(); }
		}
		/// <summary>
		/// Provide an efficient name lookup
		/// </summary>
		protected override string GetElementName(ModelElement element)
		{
			return ((ORMNamedElement)element).Name;
		}
		/// <summary>
		/// Text to be displayed when an error is thrown.
		///</summary>
		//TODO: Add this to the ResourceStrings collection
		protected override string ErrorFormatText
		{
			get { return "Duplicate recognized word exists in the model."; }
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
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { RecognizedPhraseHasDuplicateNameError.DuplicateNameErrorDomainRoleId };
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
