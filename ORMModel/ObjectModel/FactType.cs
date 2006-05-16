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
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using System.Globalization;
using Neumont.Tools.ORM.Framework;
using System.ComponentModel;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region IFactConstraint interface
	/// <summary>
	/// A constraint is defined such that it can have
	/// roles that span multiple fact types. The core
	/// model makes it difficult to determine which roles
	/// on a fact are used by a given constraint. FactConstraint
	/// and InternalFactConstraint relationships are generated
	/// automatically, but these have significantly different
	/// mechanisms for getting from the fact type to the constraint
	/// and its roles. The IFactConstraint interface is defined to
	/// smooth over this difference.
	/// </summary>
	public interface IFactConstraint
	{
		/// <summary>
		/// Get the constraint instance bound
		/// to the context fact
		/// </summary>
		IConstraint Constraint { get;}
		/// <summary>
		/// Get the roles associated with both the
		/// constraint and the fact.
		/// </summary>
		IList<Role> RoleCollection { get;}
		/// <summary>
		/// Get the fact type instance associated
		/// with this constraint. All roles in RoleCollection
		/// will be parented to this fact.
		/// </summary>
		FactType FactType { get;}
	}
	#endregion // IFactConstraint interface
	#region Derivation Storage Enum
	/// <summary>
	/// Derivation Storage Types, used to specify how/whether the contents of the fact should be stored in the database
	/// </summary>
	public enum DerivationStorageType
	{
		/// <summary>
		/// Fact is derived but should not be stored
		/// </summary>
		Derived,
		/// <summary>
		/// Fact is derived and should be stored
		/// </summary>
		DerivedAndStored,
		/// <summary>
		/// Fact is paritally derived and should be stored
		/// </summary>
		PartiallyDerived,
	}
	#endregion
	public partial class FactType : INamedElementDictionaryChild, INamedElementDictionaryRemoteParent, IModelErrorOwner, IVerbalizeFilterChildren, IVerbalizeCustomChildren
	{
		#region ReadingOrder acquisition
		/// <summary>
		/// Gets a reading order, first by trying to find it, if one doesn't exist
		/// it will then create a new ReadingOrder. It operates under the assumption
		/// that a transaction has already been started.
		/// </summary>
		public static ReadingOrder GetReadingOrder(FactType theFact, IList<RoleBase> roleOrder)
		{
			ReadingOrder retval = FindMatchingReadingOrder(theFact, roleOrder);
			if (retval == null)
			{
				retval = CreateReadingOrder(theFact, roleOrder);
			}
			return retval;
		}

		/// <summary>
		/// Looks for a ReadingOrder that has the roles in the same order
		/// as the currently selected role order.
		/// </summary>
		/// <returns>The reading order if found, null if it was not.</returns>
		public static ReadingOrder FindMatchingReadingOrder(FactType theFact, IList<RoleBase> roleOrder)
		{
			ReadingOrder retval = null;
			ReadingOrderMoveableCollection readingOrders = theFact.ReadingOrderCollection;
			int roleOrderCount = roleOrder.Count;
			foreach (ReadingOrder order in readingOrders)
			{
				RoleBaseMoveableCollection roles = order.RoleCollection;
				int numRoles = roles.Count;
				if (numRoles == roleOrderCount)
				{
					bool match = true;
					for (int i = 0; i < numRoles; ++i)
					{
						if (roles[i] != roleOrder[i])
						{
							match = false;
							break;
						}
					}
					if (match)
					{
						retval = order;
						break;
					}
				}
			}
			return retval;
		}

		/// <summary>
		/// Gets the reading order that matches the currently displayed order of the
		/// fact that is passed in.
		/// </summary>
		/// <returns>The matching ReadingOrder or null if one does not exist.</returns>
		public static ReadingOrder FindMatchingReadingOrder(FactType theFact)
		{
			RoleBaseMoveableCollection factRoles = theFact.RoleCollection;
			RoleBase[] roleOrder = new RoleBase[factRoles.Count];
			factRoles.CopyTo(roleOrder, 0);
			return FindMatchingReadingOrder(theFact, roleOrder);
		}

		/// <summary>
		/// Creates a new ReadingOrder with the same role sequence as the currently selected one.
		/// A transaction should have been pushed before calling this method. It operates under
		/// the assumption that a transaction has already been started.
		/// </summary>
		/// <returns>Should always return a value unless there was an error creating the ReadingOrder</returns>
		public static ReadingOrder CreateReadingOrder(FactType theFact, IList<RoleBase> roleOrder)
		{
			ReadingOrder retval = null;
			if (roleOrder.Count > 0)
			{
				retval = ReadingOrder.CreateReadingOrder(theFact.Store);
				RoleBaseMoveableCollection readingRoles = retval.RoleCollection;
				int numRoles = roleOrder.Count;
				for (int i = 0; i < numRoles; ++i)
				{
					readingRoles.Add(roleOrder[i]);
				}
				theFact.ReadingOrderCollection.Add(retval);
			}
			return retval;
		}
		#endregion
		#region FactType Specific
		/// <summary>
		/// Get a collection of all constraints associated with this fact,
		/// along with the roles on this fact that are used by each fact
		/// constraint.
		/// </summary>
		/// <value></value>
		public ICollection<IFactConstraint> FactConstraintCollection
		{
			get
			{
				return new FactConstraintCollectionImpl(this);
			}
		}
		/// <summary>
		/// Get an enumeration of constraints of the given type
		/// </summary>
		/// <param name="filterType">The type of constraint to return</param>
		/// <returns>IEnumerable</returns>
		public IEnumerable<SetConstraint> GetInternalConstraints(ConstraintType filterType)
		{
			switch (filterType)
			{
				case ConstraintType.InternalUniqueness:
				case ConstraintType.SimpleMandatory:
					IList constraints = SetConstraintCollection;
					int constraintCount = constraints.Count;
					for (int i = 0; i < constraintCount; ++i)
					{
						IConstraint ic = (IConstraint)constraints[i];
						if (ic.ConstraintType == filterType)
						{
							yield return (SetConstraint)ic;
						}
					}
					break;
			}
		}
		/// <summary>
		/// Get an enumeration of constraints of the given type using generics
		/// </summary>
		/// <typeparam name="T">An internal constraint type</typeparam>
		/// <returns>IEnumerable</returns>
		public IEnumerable<T> GetInternalConstraints<T>() where T : SetConstraint, IConstraint
		{
			IList constraints = SetConstraintCollection;
			int constraintCount = constraints.Count;
			for (int i = 0; i < constraintCount; ++i)
			{
				T ic = constraints[i] as T;
				if (ic != null && ic.ConstraintIsInternal)
				{
					yield return ic;
				}
			}
		}
		/// <summary>
		/// Get the number of internal constraints of the specified constraint type
		/// </summary>
		/// <param name="filterType">The type of constraint to count</param>
		/// <returns>int</returns>
		public int GetInternalConstraintsCount(ConstraintType filterType)
		{
			int retVal = 0;
			// Count the enumerator without foreach to satisfy FxCop
			IEnumerator<SetConstraint> ienum = GetInternalConstraints(filterType).GetEnumerator();
			while (ienum.MoveNext())
			{
				++retVal;
			}
			return retVal;
		}
		/// <summary>
		/// Return the Objectification relationship that
		/// attaches this fact to its nesting type
		/// </summary>
		public Objectification Objectification
		{
			get
			{
				IList links = GetElementLinks(Objectification.NestedFactTypeMetaRoleGuid, false);
				if (links != null && links.Count != 0)
				{
					Debug.Assert(links.Count == 1);
					return (Objectification)links[0];
				}
				return null;
			}
		}
		/// <summary>
		/// Gets the ObjectType that is objectifying this FactType.
		/// </summary>
		public ObjectType NestingType
		{
			get
			{
				return GetCounterpartRolePlayer(Objectification.NestedFactTypeMetaRoleGuid, Objectification.NestingTypeMetaRoleGuid, false) as ObjectType;
			}
			set
			{
				Utility.SetPropertyValidateOneToOne(this, value, Objectification.NestedFactTypeMetaRoleGuid, Objectification.NestingTypeMetaRoleGuid, typeof(Objectification));
			}
		}
		#endregion // FactType Specific
		#region FactConstraintCollection implementation
		private class FactConstraintCollectionImpl : ICollection<IFactConstraint>
		{
			#region Member Variables
			private IList[] myLists;
			#endregion // Member Variables
			#region Constructors
			/// <summary>
			/// Create a FactConstraint collection for the given fact type. Fact constraints
			/// come from multiple links, this puts them all together.
			/// </summary>
			/// <param name="factType">The parent fact type</param>
			public FactConstraintCollectionImpl(FactType factType)
			{
				myLists = new IList[]{
					factType.GetElementLinks(FactSetConstraint.FactTypeCollectionMetaRoleGuid),
					factType.GetElementLinks(FactSetComparisonConstraint.FactTypeCollectionMetaRoleGuid)};
			}
			#endregion // Constructors
			#region ICollection<IFactConstraint> Implementation
			bool ICollection<IFactConstraint>.Contains(IFactConstraint item)
			{
				IList[] lists = myLists;
				int listCount = lists.Length;
				for (int i = 0; i < listCount; ++i)
				{
					if (lists[i].Contains(item))
					{
						return true;
					}
				}
				return false;
			}
			void ICollection<IFactConstraint>.CopyTo(IFactConstraint[] array, int arrayIndex)
			{
				IList[] lists = myLists;
				int listCount = lists.Length;
				int prevTotal = 0;
				for (int i = 0; i < listCount; ++i)
				{
					IList curList = lists[i];
					int curTotal = curList.Count;
					if (curTotal != 0)
					{
						curList.CopyTo(array, prevTotal);
						prevTotal += curTotal;
					}
				}
			}
			int ICollection<IFactConstraint>.Count
			{
				get
				{
					IList[] lists = myLists;
					int listCount = lists.Length;
					int total = 0;
					for (int i = 0; i < listCount; ++i)
					{
						total += lists[i].Count;
					}
					return total;
				}
			}
			bool ICollection<IFactConstraint>.IsReadOnly
			{
				get { return true; }
			}
			bool ICollection<IFactConstraint>.Remove(IFactConstraint item)
			{
				// Not supported for read-only
				throw new InvalidOperationException();
			}
			void ICollection<IFactConstraint>.Add(IFactConstraint item)
			{
				// Not supported for read-only
				throw new InvalidOperationException();
			}
			void ICollection<IFactConstraint>.Clear()
			{
				// Not supported for read-only
				throw new InvalidOperationException();
			}
			#endregion // ICollection<IFactConstraint> Implementation
			#region IEnumerable<IFactConstraint> Implementation
			IEnumerator<IFactConstraint> IEnumerable<IFactConstraint>.GetEnumerator()
			{
				IList[] lists = myLists;
				int listCount = lists.Length;
				for (int i = 0; i < listCount; ++i)
				{
					IList curList = lists[i];
					int curTotal = curList.Count;
					for (int j = 0; j < curTotal; ++j)
					{
						yield return (IFactConstraint)curList[j];
					}
				}
			}
			IEnumerator IEnumerable.GetEnumerator()
			{
				return (this as IEnumerable<IFactConstraint>).GetEnumerator();
			}
			#endregion // IEnumerable<IFactConstraint> Implementation
		}
		#endregion // FactConstraintCollection implementation
		#region Customize property display
		/// <summary>
		/// Distinguish between an objectified and a
		/// normal fact type.
		/// </summary>
		public override string GetClassName()
		{
			Objectification objectification = Objectification;
			return (objectification == null || objectification.IsImplied) ? ResourceStrings.FactType : ResourceStrings.ObjectifiedFactType;
		}
		/// <summary>
		/// Return a simple name instead of a name decorated with the type (the
		/// default for a ModelElement). This is the easiest way to display
		/// clean names in the property grid when we reference properties.
		/// </summary>
		public override string ToString()
		{
			return Name;
		}
		/// <summary>
		/// Return our customized name for the component name
		/// </summary>
		public override string GetComponentName()
		{
			return Name;
		}
		/// <summary>
		/// Standard override. Stop the DerivationStorage property from
		/// displaying if no derivation rule is specified
		/// </summary>
		public override bool ShouldCreatePropertyDescriptor(MetaAttributeInfo metaAttrInfo)
		{
			Guid attributeId = metaAttrInfo.Id;
			if (attributeId == DerivationStorageDisplayMetaAttributeGuid)
			{
				return DerivationRule != null;
			}
			return base.ShouldCreatePropertyDescriptor(metaAttrInfo);
		}
		/// <summary>
		/// Standard override. Stop the Name attribute from being writable on
		/// non-objectified facts
		/// </summary>
		public override bool IsPropertyDescriptorReadOnly(PropertyDescriptor propertyDescriptor)
		{
			ElementPropertyDescriptor descriptor = propertyDescriptor as ElementPropertyDescriptor;
			if (descriptor != null && descriptor.MetaAttributeInfo.Id == NameMetaAttributeGuid)
			{
				return Objectification == null;
			}
			return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
		}
		#endregion // Customize property display
		#region MergeContext functions
		/// <summary>
		/// Support adding root elements and constraints directly to the design surface
		/// </summary>
		/// <param name="elementGroupPrototype">The object representing the serialized data being added to this FactType.</param>
		/// <param name="protoElement">The element to add.</param>
		/// <returns>True if addition is allowed; otherwise, false.</returns>
		protected override bool CanAddChildElement(ElementGroupPrototype elementGroupPrototype, ProtoElementBase protoElement)
		{
			if (protoElement != null)
			{
				MetaClassInfo classInfo = Store.MetaDataDirectory.FindMetaClass(protoElement.MetaClassId);
				if (classInfo.IsDerivedFrom(UniquenessConstraint.MetaClassGuid))
				{
					return "INTERNALUNIQUENESSCONSTRAINT" == (string)elementGroupPrototype.UserData;
				}
			}
			return false;
		}

		/// <summary>
		/// Attach a deserialized InternalUniquenessConstraint to this FactType.
		/// Called after prototypes for these items are dropped onto the diagram
		/// from the toolbox.
		/// </summary>
		/// <param name="sourceElement">The element being added</param>
		/// <param name="elementGroup">The element describing all of the created elements</param>
		public override void MergeRelate(ModelElement sourceElement, ElementGroup elementGroup)
		{
			base.MergeRelate(sourceElement, elementGroup);
			UniquenessConstraint internalConstraint;
			if (null != (internalConstraint = sourceElement as UniquenessConstraint))
			{
				ORMModel model;
				if (internalConstraint.IsInternal)
				{
					internalConstraint.FactTypeCollection.Add(this);
				}
				else if (null != (model = Model))
				{
					model.MergeRelate(sourceElement, elementGroup);
				}
			}
		}
		#endregion // MergeContext functions
		#region CustomStorage handlers
		/// <summary>
		/// Standard override. All custom storage properties are derived, not
		/// stored. Actual changes are handled in FactTypeChangeRule.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <param name="newValue">object</param>
		public override void SetValueForCustomStoredAttribute(MetaAttributeInfo attribute, object newValue)
		{
			Guid attributeGuid = attribute.Id;
			if (attributeGuid == NestingTypeDisplayMetaAttributeGuid ||
				attributeGuid == DerivationRuleDisplayMetaAttributeGuid ||
				attributeGuid == DerivationStorageDisplayMetaAttributeGuid ||
				attributeGuid == NoteTextMetaAttributeGuid)
			{
				// Handled by FactTypeChangeRule
				return;
			}
			else if (attributeGuid == NameMetaAttributeGuid)
			{
				myGeneratedName = (string)newValue;
				// Remainder handled by FactTypeChangeRule
				return;
			}
			base.SetValueForCustomStoredAttribute(attribute, newValue);
		}
		/// <summary>
		/// Standard override. Retrieve values for calculated properties.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <returns></returns>
		public override object GetValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			Guid attributeGuid = attribute.Id;
			if (attributeGuid == NestingTypeDisplayMetaAttributeGuid)
			{
				Objectification objectification = Objectification;
				return (objectification != null && !objectification.IsImplied) ? objectification.NestingType : null;
			}
			else if (attributeGuid == DerivationRuleDisplayMetaAttributeGuid)
			{
				FactTypeDerivationExpression derivation = DerivationRule;

				if (null == derivation || derivation.IsRemoved)
				{
					return string.Empty;
				}
				return derivation.Body;
			}
			else if (attributeGuid == DerivationStorageDisplayMetaAttributeGuid)
			{
				FactTypeDerivationExpression derivation = DerivationRule;
				if (null == derivation || derivation.IsRemoved)
				{
					return DerivationStorageType.Derived;
				}
				return derivation.DerivationStorage;
			}
			else if (attributeGuid == NoteTextMetaAttributeGuid)
			{
				Note currentNote = Note;
				return (currentNote != null) ? currentNote.Text : "";
			}
			else if (attributeGuid == NameMetaAttributeGuid)
			{
				Objectification objectificationLink;
				ObjectType nestingType;
				Store store = Store;
				string retVal = null;
				if (store.InUndo || store.InRedo)
				{
					retVal = myGeneratedName;
				}
				else if (null != (objectificationLink = Objectification) &&
					null != (nestingType = objectificationLink.NestingType))
				{
					// Use the name from the nesting type
					retVal = nestingType.Name;
				}
				else if (!store.TransactionManager.InTransaction)
				{
					retVal = myGeneratedName;
					if (string.IsNullOrEmpty(retVal))
					{
						myGeneratedName = retVal = GenerateName();
					}
				}
				else
				{
					retVal = myGeneratedName;
					if (retVal != null && retVal.Length == 0) // The == null here is a hack. Use myGeneratedName = null before calling to skip setting this during a transaction
					{
						myGeneratedName = retVal = GenerateName();
					}
				}
				return (retVal != null) ? retVal : "";
			}
			return base.GetValueForCustomStoredAttribute(attribute);
		}
		#endregion // CustomStorage handlers
		#region INamedElementDictionaryChild implementation
		void INamedElementDictionaryChild.GetRoleGuids(out Guid parentMetaRoleGuid, out Guid childMetaRoleGuid)
		{
			GetRoleGuids(out parentMetaRoleGuid, out childMetaRoleGuid);
		}
		/// <summary>
		/// Implementation of INamedElementDictionaryChild.GetRoleGuids. Identifies
		/// this child as participating in the 'ModelHasFactType' naming set.
		/// </summary>
		/// <param name="parentMetaRoleGuid">Guid</param>
		/// <param name="childMetaRoleGuid">Guid</param>
		protected static void GetRoleGuids(out Guid parentMetaRoleGuid, out Guid childMetaRoleGuid)
		{
			parentMetaRoleGuid = ModelHasFactType.ModelMetaRoleGuid;
			childMetaRoleGuid = ModelHasFactType.FactTypeCollectionMetaRoleGuid;
		}
		#endregion // INamedElementDictionaryChild implementation
		#region INamedElementDictionaryRemoteParent implementation
		private static readonly Guid[] myRemoteNamedElementDictionaryRoles = new Guid[] { FactTypeHasRole.FactTypeMetaRoleGuid };
		/// <summary>
		/// Implementation of INamedElementDictionaryRemoteParent.GetNamedElementDictionaryLinkRoles. Identifies
		/// this as a remote parent for the 'ModelHasConstraint' naming set.
		/// </summary>
		/// <returns>Guid for the FactTypeHasInternalConstraint.FactType role</returns>
		protected static Guid[] GetNamedElementDictionaryLinkRoles()
		{
			return myRemoteNamedElementDictionaryRoles;
		}
		Guid[] INamedElementDictionaryRemoteParent.GetNamedElementDictionaryLinkRoles()
		{
			return GetNamedElementDictionaryLinkRoles();
		}
		#endregion // INamedElementDictionaryRemoteParent implementation
		#region RoleChangeRule class
		[RuleOn(typeof(FactType))]
		private class FactTypeChangeRule : ChangeRule
		{
			/// <summary>
			/// Forward through the property grid property to the underlying
			/// nesting type property
			/// </summary>
			/// <param name="e"></param>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeGuid = e.MetaAttribute.Id;
				if (attributeGuid == FactType.NestingTypeDisplayMetaAttributeGuid)
				{
					Objectification.CreateExplicitObjectification(e.ModelElement as FactType, e.NewValue as ObjectType);
				}
				else if (attributeGuid == FactType.DerivationRuleDisplayMetaAttributeGuid)
				{
					FactType factType = e.ModelElement as FactType;
					string newVal = e.NewValue as string;
					FactTypeDerivationExpression currentRule = factType.DerivationRule;
					if (string.IsNullOrEmpty(newVal))
					{
						if (currentRule != null)
						{
							currentRule.Body = string.Empty;
						}
					}
					else
					{
						if (null == currentRule)
						{
							currentRule = FactTypeDerivationExpression.CreateFactTypeDerivationExpression(factType.Store);
							factType.DerivationRule = currentRule;
						}
						currentRule.Body = newVal;
					}
				}
				else if (attributeGuid == FactType.DerivationStorageDisplayMetaAttributeGuid)
				{
					FactType factType = e.ModelElement as FactType;
					if (factType.DerivationRule != null)
					{
						factType.DerivationRule.DerivationStorage = (DerivationStorageType)e.NewValue;
					}
				}
				else if (attributeGuid == FactType.NoteTextMetaAttributeGuid)
				{
					// cache the text.
					string newText = (string)e.NewValue;
					FactType factType = e.ModelElement as FactType;
					// Get the note if it exists
					Note note = factType.Note;
					if (note != null)
					{
						// and try to set the text to the cached value.
						note.Text = newText;
					}
					else if (!string.IsNullOrEmpty(newText))
					{
						// Otherwise, create the note and set the text,
						note = Note.CreateNote(factType.Store);
						note.Text = newText;
						// then attach the note to the RootType.
						factType.Note = note;
					}
				}
				else if (attributeGuid == FactType.NameMetaAttributeGuid)
				{
					FactType factType = e.ModelElement as FactType;
					Objectification objectificationLink;
					ObjectType nestingType;
					if (null != (objectificationLink = factType.Objectification) &&
						null != (nestingType = objectificationLink.NestingType))
					{
						nestingType.Name = (string)e.NewValue;
					}
				}
			}
		}
		#endregion // RoleChangeRule class
		#region IModelErrorOwner Members
		/// <summary>
		/// Returns the error associated with the fact.
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			if (filter == 0)
			{
				filter = (ModelErrorUses)(-1);
			}
			if (0 != (filter & ModelErrorUses.BlockVerbalization))
			{
				FactTypeRequiresReadingError noReadingError = this.ReadingRequiredError;
				if (noReadingError != null)
				{
					yield return new ModelErrorUsage(noReadingError, ModelErrorUses.BlockVerbalization);
				}
			}

			if (0 != (filter & ModelErrorUses.Verbalize))
			{
				FactTypeRequiresInternalUniquenessConstraintError noUniquenessError = this.InternalUniquenessConstraintRequiredError;
				if (noUniquenessError != null)
				{
					yield return noUniquenessError;
				}
				// Any duplicate constraint will trigger this. It is possible for multiple duplicated
				// constraints to exist on the fact type, but only one error is shown per fact type.
				ImpliedInternalUniquenessConstraintError dupConstraint = this.ImpliedInternalUniquenessConstraintError;
				if (dupConstraint != null)
				{
					yield return dupConstraint;
				}
			}

			if (0 == (filter & (ModelErrorUses.Verbalize | ModelErrorUses.BlockVerbalization)) || filter == (ModelErrorUses)(-1))
			{
				// The fact name is used in the generated error text, it needs to be an owner
				foreach (FrequencyConstraintContradictsInternalUniquenessConstraintError frequencyContradictionError in FrequencyConstraintContradictsInternalUniquenessConstraintErrorCollection)
				{
					yield return new ModelErrorUsage(frequencyContradictionError, ModelErrorUses.None);
				}

				// NMinusOneError is parented off InternalConstraint. The constraint has a name,
				// but the name is often arbitrary. Including the fact name as well makes the
				// error message much more meaningful.
				foreach (UniquenessConstraint ic in GetInternalConstraints<UniquenessConstraint>())
				{
					NMinusOneError nMinusOneError = ic.NMinusOneError;
					if (nMinusOneError != null)
					{
						yield return new ModelErrorUsage(nMinusOneError, ModelErrorUses.None);
					}
				}
			}
			if (0 == (filter & ModelErrorUses.BlockVerbalization) || filter == (ModelErrorUses)(-1)) // Roles don't verbalize, we need to show these here
			{
				// Show the fact type as an owner of the role errors as well
				// so the fact can be accurately named in the error text. However,
				// we do not validate this error on the fact type, it is done on the role.
				foreach (RoleBase roleBase in RoleCollection)
				{
					Role role = roleBase as Role;
					if (role != null)
					{
						if (0 != (filter & ModelErrorUses.Verbalize))
						{
							foreach (ModelErrorUsage roleError in (role as IModelErrorOwner).GetErrorCollection(filter))
							{
								yield return new ModelErrorUsage(roleError, ModelErrorUses.Verbalize);
							}
						}
						if (0 == (filter & ModelErrorUses.Verbalize) || filter == (ModelErrorUses)(-1))
						{
							IModelErrorOwner valueErrors = role.ValueConstraint as IModelErrorOwner;
							if (valueErrors != null)
							{
								foreach (ModelErrorUsage valueError in valueErrors.GetErrorCollection(filter))
								{
									yield return new ModelErrorUsage(valueError, ModelErrorUses.None);
								}
							}
						}
					}
				}
			}
			if (0 != (filter & (ModelErrorUses.BlockVerbalization | ModelErrorUses.Verbalize)))
			{
				foreach (ReadingOrder readingOrder in ReadingOrderCollection)
				{
					foreach (Reading reading in readingOrder.ReadingCollection)
					{
						foreach (ModelError readingError in (reading as IModelErrorOwner).GetErrorCollection(filter))
						{
							if (readingError is TooFewReadingRolesError)
							{
								if (0 != (filter & ModelErrorUses.BlockVerbalization))
								{
									yield return new ModelErrorUsage(readingError, ModelErrorUses.BlockVerbalization);
								}
							}
							else if (0 != (filter & ModelErrorUses.Verbalize))
							{
								yield return new ModelErrorUsage(readingError, ModelErrorUses.Verbalize);
							}
						}
					}
				}
			}
			// Get errors off the base
			foreach (ModelErrorUsage baseError in base.GetErrorCollection(filter))
			{
				yield return baseError;
			}
		}
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			return GetErrorCollection(filter);
		}

		/// <summary>
		/// Implements IModelErrorOwner.ValidateErrors
		/// </summary>
		protected new void ValidateErrors(INotifyElementAdded notifyAdded)
		{
			// Calls added here need corresponding delayed calls in DelayValidateErrors
			ValidateRequiresReading(notifyAdded);
			ValidateRequiresInternalUniqueness(notifyAdded);
			ValidateImpliedInternalUniqueness(notifyAdded);
		}
		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateErrors(notifyAdded);
		}
		/// <summary>
		/// Implements IModelErrorOwner.DelayValidateErrors
		/// </summary>
		protected new void DelayValidateErrors()
		{
			ORMMetaModel.DelayValidateElement(this, DelayValidateFactTypeRequiresReadingError);
			ORMMetaModel.DelayValidateElement(this, DelayValidateFactTypeRequiresInternalUniquenessConstraintError);
			ORMMetaModel.DelayValidateElement(this, DelayValidateImpliedInternalUniquenessConstraintError);
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion
		#region Validation Methods
		/// <summary>
		/// Validator callback for FactTypeRequiresReadingError
		/// </summary>
		private static void DelayValidateFactTypeRequiresReadingError(ModelElement element)
		{
			(element as FactType).ValidateRequiresReading(null);
		}
		private void ValidateRequiresReading(INotifyElementAdded notifyAdded)
		{
			if (!IsRemoved)
			{
				bool hasError = true;
				Store theStore = Store;
				ORMModel theModel = Model;
				ReadingOrderMoveableCollection readingOrders = ReadingOrderCollection;
				if (readingOrders.Count > 0)
				{
					foreach (ReadingOrder order in readingOrders)
					{
						if (order.ReadingCollection.Count > 0)
						{
							hasError = false;
							break;
						}
					}
				}

				FactTypeRequiresReadingError noReadingError = ReadingRequiredError;
				if (hasError)
				{
					if (noReadingError == null)
					{
						noReadingError = FactTypeRequiresReadingError.CreateFactTypeRequiresReadingError(theStore);
						noReadingError.FactType = this;
						noReadingError.Model = theModel;
						noReadingError.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(noReadingError, true);
						}
					}
				}
				else
				{
					if (noReadingError != null)
					{
						noReadingError.Remove();
					}
				}
			}
		}
		/// <summary>
		/// Validator callback for FactTypeRequiresInternalUniquenessConstraintError
		/// </summary>
		private static void DelayValidateFactTypeRequiresInternalUniquenessConstraintError(ModelElement element)
		{
			(element as FactType).ValidateRequiresInternalUniqueness(null);
		}
		private void ValidateRequiresInternalUniqueness(INotifyElementAdded notifyAdded)
		{
			ORMModel theModel;
			if (!IsRemoved && (null != (theModel = Model)))
			{
				bool hasError = RoleCollection.Count > 1;
				Store theStore = Store;

				if (hasError)
				{
					foreach (UniquenessConstraint iuc in GetInternalConstraints<UniquenessConstraint>())
					{
						if (iuc.Modality == ConstraintModality.Alethic)
						{
							hasError = false;
							break;
						}
					}
				}

				FactTypeRequiresInternalUniquenessConstraintError noUniquenessError = InternalUniquenessConstraintRequiredError;
				
				if (hasError)
				{
					if (noUniquenessError == null)
					{
						noUniquenessError = FactTypeRequiresInternalUniquenessConstraintError.CreateFactTypeRequiresInternalUniquenessConstraintError(theStore);
						noUniquenessError.FactType = this;
						noUniquenessError.Model = theModel;
						noUniquenessError.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(noUniquenessError, true);
						}
					}
				}
				else if (noUniquenessError != null)
				{
					noUniquenessError.Remove();
				}
			}
		}
		/// <summary>
		/// Validator callback for ImpliedInternalUniquenessConstraintError
		/// </summary>
		private static void DelayValidateImpliedInternalUniquenessConstraintError(ModelElement element)
		{
			(element as FactType).ValidateImpliedInternalUniqueness(null);
		}
		private void ValidateImpliedInternalUniqueness(INotifyElementAdded notifyAdded)
		{

			ORMModel theModel;
			if (!IsRemoved && (null != (theModel = Model)))
			{
				Store theStore = Store;
				RoleBaseMoveableCollection factRoles = RoleCollection;
				bool hasError = false;
				int iucCount = GetInternalConstraintsCount(ConstraintType.InternalUniqueness);
				if (iucCount != 0)
				{
					uint[] roleBits = new uint[iucCount];
					const uint deonticBit = 1U << 31;
					int index = 0;
					foreach (UniquenessConstraint ic in GetInternalConstraints<UniquenessConstraint>())
					{
						uint bits = 0;
						RoleMoveableCollection constraintRoles = ic.RoleCollection;
						int roleCount = constraintRoles.Count;
						for (int i = 0; i < roleCount; ++i)
						{
							bits |= 1U << factRoles.IndexOf(constraintRoles[i]);
						}
						if (bits != 0 && ic.Modality == ConstraintModality.Deontic)
						{
							bits |= deonticBit;
						}
						roleBits[index] = bits;
						++index;
					}
					int rbLength = roleBits.Length;
					for (int i = 0; !hasError && i < rbLength - 1; ++i)
					{
						for (int j = i + 1; j < rbLength; ++j)
						{
							uint left = roleBits[i];
							uint right = roleBits[j];
							if (left != 0 && right != 0)
							{
								if (0 != ((left ^ right) & deonticBit))
								{
									// Modality is different. The alethic constraint
									// should not be a subset of the deontic constraint,
									// but there are no restrictions the other way around.
									if ((left & right) == ((0 == (left & deonticBit)) ? left : right))
									{
										hasError = true;
										break;
									}
								}
								else
								{
									uint compare = left & right;
									if ((compare == left) || (compare == right))
									{
										hasError = true;
										break;
									}
								}
							}
						}
					}
				}
				ImpliedInternalUniquenessConstraintError impConstraint = ImpliedInternalUniquenessConstraintError;
				if (hasError)
				{
					if (impConstraint == null)
					{
						impConstraint = ImpliedInternalUniquenessConstraintError.CreateImpliedInternalUniquenessConstraintError(theStore);
						impConstraint.FactType = this;
						impConstraint.Model = theModel;
						impConstraint.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(impConstraint);
						}
					}
				}
				else if (impConstraint != null)
				{
					impConstraint.Remove();
				}
			}
		}
		#endregion // Validation Methods
		#region Automatic Name Generation
		private static void DelayValidateFactTypeNamePartChanged(ModelElement element)
		{
			FactType factType = element as FactType;
			if (!factType.IsRemoved)
			{
				Store store = element.Store;
				string oldGeneratedName = factType.myGeneratedName;
				bool haveNewName = false;
				string newGeneratedName = null;

				// See if the nestedType uses the old automatic name. If it does, then
				// update the automatic name to the the new name.
				ObjectType nestingType = null;
				Objectification objectificationLink;
				if (null != (objectificationLink = factType.Objectification) &&
					null != (nestingType = objectificationLink.NestingType) &&
					!nestingType.IsRemoved)
				{
					newGeneratedName = factType.GenerateName();
					haveNewName = true;
					if (newGeneratedName != oldGeneratedName)
					{
						if (nestingType.Name == oldGeneratedName)
						{
							IDictionary contextInfo = store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
							RuleManager ruleManager = store.RuleManager;
							bool ruleDisabled = false;
							try
							{
								// Force a change in the transaction log so that we can
								// update the generated name as needed
								ruleManager.DisableRule(typeof(FactTypeChangeRule));
								ruleDisabled = true;
								if (string.IsNullOrEmpty(oldGeneratedName))
								{
									factType.myGeneratedName = null; // Set explicitly to null, see notes in GetValueForCustomStoredAttribute
								}
								factType.Name = newGeneratedName;
								contextInfo[ObjectType.AllowDuplicateObjectNamesKey] = null;
								nestingType.Name = newGeneratedName;
							}
							finally
							{
								contextInfo.Remove(ObjectType.AllowDuplicateObjectNamesKey);
								if (ruleDisabled)
								{
									ruleManager.EnableRule(typeof(FactTypeChangeRule));
								}
							}
						}
					}
					else
					{
						newGeneratedName = null;
					}
				}

				if (!haveNewName || newGeneratedName != null)
				{
					// Now move on to any model errors
					foreach (ModelError error in (factType as IModelErrorOwner).GetErrorCollection(ModelErrorUses.None))
					{
						if (0 != (error.RegenerateEvents & RegenerateErrorTextEvents.OwnerNameChange))
						{
							if (newGeneratedName == null)
							{
								newGeneratedName = factType.GenerateName();
								haveNewName = true;
								if (newGeneratedName == oldGeneratedName)
								{
									newGeneratedName = null;
									break; // Look no further, name did not change
								}
								else
								{
									RuleManager ruleManager = store.RuleManager;
									bool ruleDisabled = false;
									try
									{
										// Force a change in the transaction log so that we can
										// update the generated name as needed
										ruleManager.DisableRule(typeof(FactTypeChangeRule));
										ruleDisabled = true;
										if (string.IsNullOrEmpty(oldGeneratedName))
										{
											factType.myGeneratedName = null; // Set explicitly to null, see notes in GetValueForCustomStoredAttribute
										}
										factType.Name = newGeneratedName;
									}
									finally
									{
										if (ruleDisabled)
										{
											ruleManager.EnableRule(typeof(FactTypeChangeRule));
										}
									}
								}
							}
							error.GenerateErrorText();
						}
					}
				}
				if (newGeneratedName == null && !haveNewName)
				{
					// Name did not change, but no one cared, add a simple entry to the transaction log
					if (!string.IsNullOrEmpty(oldGeneratedName))
					{
						factType.Name = "";
					}
				}
			}
		}
		/// <summary>
		/// Helper function to get the current setting for the generated Name property
		/// </summary>
		private string GenerateName()
		{
			string retVal = "";
			if (!IsRemoved)
			{
				// Grab the first reading with no errors from the first reading order
				// Note that the first reading in the first reading order is considered
				// to be the default reading order
				RoleBaseMoveableCollection roles = null;
				string formatText = null;
				ReadingOrderMoveableCollection readingOrders = ReadingOrderCollection;
				int readingOrdersCount = readingOrders.Count;
				for (int i = 0; i < readingOrdersCount && formatText == null; ++i)
				{
					ReadingOrder order = readingOrders[i];
					ReadingMoveableCollection readings = order.ReadingCollection;
					int readingsCount = readings.Count;
					for (int j = 0; j < readingsCount; ++j)
					{
						Reading reading = readings[j];
						if (!ModelError.HasErrors(reading))
						{
							roles = order.RoleCollection;
							formatText = reading.Text;
							break;
						}
					}
				}
				if (roles == null)
				{
					roles = RoleCollection;
				}
				int rolesCount = roles.Count;
				if (rolesCount != 0)
				{
					string[] replacements = new string[rolesCount];
					for (int k = 0; k < rolesCount; ++k)
					{
						ObjectType rolePlayer = roles[k].Role.RolePlayer;
						replacements[k] = (rolePlayer != null) ? rolePlayer.Name : ResourceStrings.ModelReadingEditorMissingRolePlayerText;
					}
					retVal = (formatText == null) ?
						string.Concat(replacements) :
						string.Format(CultureInfo.InvariantCulture, CultureInfo.InvariantCulture.TextInfo.ToTitleCase(formatText), replacements);
					if (!string.IsNullOrEmpty(retVal))
					{
						retVal = retVal.Replace(" ", null);
					}
				}
			}
			return retVal;
		}
		private string myGeneratedName = "";
		/// <summary>
		/// The auto-generated name for this fact type. Based on the
		/// first reading in the first reading order.
		/// </summary>
		public string GeneratedName
		{
			get
			{
				string retVal = myGeneratedName;
				if (string.IsNullOrEmpty(retVal))
				{
					retVal = GenerateName();
					if (retVal.Length != 0)
					{
						if (Store.TransactionManager.InTransaction)
						{
							myGeneratedName = null; // Set explicitly to null, see notes in GetValueForCustomStoredAttribute
							Name = retVal;
						}
						else
						{
							myGeneratedName = retVal;
						}
					}
				}
				return (retVal != null) ? retVal : "";
			}
		}
		#endregion // Automatic Name Generation
		#region Model Validation Rules
		/// <summary>
		/// Internal uniqueness constraints are required for non-unary facts. Requires
		/// validation when roles are added and removed.
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole))]
		private class FactTypeHasRoleAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactType factType = (e.ModelElement as FactTypeHasRole).FactType;
				ORMMetaModel.DelayValidateElement(factType, DelayValidateFactTypeRequiresInternalUniquenessConstraintError);
				ORMMetaModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
			}
		}
		/// <summary>
		/// Internal uniqueness constraints are required for non-unary facts. Requires
		/// validation when roles are added and removed.
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole))]
		private class FactTypeHasRoleRemoveRule : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				FactType factType = (e.ModelElement as FactTypeHasRole).FactType;
				if (!factType.IsRemoved)
				{
					ORMMetaModel.DelayValidateElement(factType, DelayValidateFactTypeRequiresInternalUniquenessConstraintError);
					ORMMetaModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
				}
			}
		}
		/// <summary>
		/// Validate the InternalUniquenessConstraintRequired and ImpliedInternalUniquenessConstraintError
		/// </summary>
		[RuleOn(typeof(FactSetConstraint))]
		private class ModelHasInternalConstraintAddRuleModelValidation : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactSetConstraint link = e.ModelElement as FactSetConstraint;
				if (link.SetConstraintCollection.Constraint.ConstraintType == ConstraintType.InternalUniqueness)
				{
					FactType fact = link.FactTypeCollection;
					ORMMetaModel.DelayValidateElement(fact, DelayValidateFactTypeRequiresInternalUniquenessConstraintError);
					ORMMetaModel.DelayValidateElement(fact, DelayValidateImpliedInternalUniquenessConstraintError);
				}
			}
		}
		/// <summary>
		/// Validate the InternalUniquenessConstraintRequired and ImpliedInternalUniquenessConstraintError
		/// </summary>
		[RuleOn(typeof(FactSetConstraint))]
		private class ModelHasInternalConstraintRemoveRuleModelValidation : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				FactSetConstraint link = e.ModelElement as FactSetConstraint;
				FactType fact = link.FactTypeCollection;
				if (!fact.IsRemoved &&
					link.SetConstraintCollection.Constraint.ConstraintType == ConstraintType.InternalUniqueness)
				{
					ORMMetaModel.DelayValidateElement(fact, DelayValidateFactTypeRequiresInternalUniquenessConstraintError);
					ORMMetaModel.DelayValidateElement(fact, DelayValidateImpliedInternalUniquenessConstraintError);
				}
			}
		}
		[RuleOn(typeof(UniquenessConstraint))]
		private class InternalUniquenessConstraintChangeRule : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeId = e.MetaAttribute.Id;
				if (attributeId == UniquenessConstraint.ModalityMetaAttributeGuid)
				{
					UniquenessConstraint constraint = e.ModelElement as UniquenessConstraint;
					FactTypeMoveableCollection facts;
					if (!constraint.IsRemoved &&
						constraint.IsInternal &&
						1 == (facts = constraint.FactTypeCollection).Count)
					{
						FactType fact = facts[0];
						ORMMetaModel.DelayValidateElement(fact, DelayValidateFactTypeRequiresInternalUniquenessConstraintError);
						ORMMetaModel.DelayValidateElement(fact, DelayValidateImpliedInternalUniquenessConstraintError);
					}
				}
			}
		}
		[RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		private class InternalConstraintCollectionHasConstraintAddedRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				UniquenessConstraint constraint = link.ConstraintRoleSequenceCollection as UniquenessConstraint;
				FactTypeMoveableCollection facts;
				if (constraint != null &&
					constraint.IsInternal &&
					1 == (facts = constraint.FactTypeCollection).Count)
				{
					ORMMetaModel.DelayValidateElement(facts[0], DelayValidateImpliedInternalUniquenessConstraintError);
				}
			}
		}

		[RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		private class InternalConstraintCollectionHasConstraintRemovedRule : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				UniquenessConstraint constraint = link.ConstraintRoleSequenceCollection as UniquenessConstraint;
				FactTypeMoveableCollection facts;
				FactType fact;
				if (constraint != null &&
					!constraint.IsRemoved &&
					constraint.IsInternal &&
					1 == (facts = constraint.FactTypeCollection).Count &&
					!(fact = facts[0]).IsRemoved)
				{
					ORMMetaModel.DelayValidateElement(fact, DelayValidateImpliedInternalUniquenessConstraintError);
				}
			}
		}


		/// <summary>
		/// Calls the validation of all FactType related errors
		/// </summary>
		[RuleOn(typeof(ModelHasFactType))]
		private class ModelHasFactTypeAddRuleModelValidation : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelHasFactType link = e.ModelElement as ModelHasFactType;
				if (link != null)
				{
					FactType fact = link.FactTypeCollection;
					if (fact != null)
					{
						fact.DelayValidateErrors();
					}
				}
			}
		}

		/// <summary>
		/// Only validates ReadingRequiredError
		/// </summary>
		[RuleOn(typeof(FactTypeHasReadingOrder))]
		private class FactTypeHasReadingOrderAddRuleModelValidation : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactType factType = (e.ModelElement as FactTypeHasReadingOrder).FactType;
				ORMMetaModel.DelayValidateElement(factType, DelayValidateFactTypeRequiresReadingError);
				ORMMetaModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
			}
		}
		/// <summary>
		/// Only validates ReadingRequiredError
		/// </summary>
		[RuleOn(typeof(FactTypeHasReadingOrder))]
		private class FactTypeHasReadingOrderRemovedRuleModelValidation : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				FactType factType = (e.ModelElement as FactTypeHasReadingOrder).FactType;
				if (!factType.IsRemoved)
				{
					ORMMetaModel.DelayValidateElement(factType, DelayValidateFactTypeRequiresReadingError);
					ORMMetaModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
				}
			}
		}

		/// <summary>
		/// Only validates ReadingRequiredError
		/// </summary>
		[RuleOn(typeof(ReadingOrderHasReading))]
		private class ReadingOrderHasReadingAddRuleModelValidation : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactType factType = (e.ModelElement as ReadingOrderHasReading).ReadingOrder.FactType;
				if (factType != null)
				{
					ORMMetaModel.DelayValidateElement(factType, DelayValidateFactTypeRequiresReadingError);
					ORMMetaModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
				}
			}
		}
		/// <summary>
		/// Only validates ReadingRequiredError
		/// </summary>
		[RuleOn(typeof(ReadingOrderHasReading))]
		private class ReadingOrderHasReadingRemoveRuleModelValidation : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ReadingOrder ord = (e.ModelElement as ReadingOrderHasReading).ReadingOrder;
				FactType factType;
				if (!ord.IsRemoved &&
					null != (factType = ord.FactType) &&
					!factType.IsRemoved)
				{
					ORMMetaModel.DelayValidateElement(factType, DelayValidateFactTypeRequiresReadingError);
					ORMMetaModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
				}
			}
		}
		[RuleOn(typeof(Reading))]
		private class ValidateFactNameForReadingChange : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				if (e.MetaAttribute.Id == Reading.TextMetaAttributeGuid)
				{
					Reading reading = e.ModelElement as Reading;
					ReadingOrder order;
					FactType factType;
					if (null != reading &&
						!reading.IsRemoved &&
						null != (order = reading.ReadingOrder) &&
						!order.IsRemoved &&
						null != (factType = order.FactType) &&
						!factType.IsRemoved)
					{
						ORMMetaModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
					}
				}
			}
		}
		[RuleOn(typeof(FactTypeHasReadingOrder))]
		private class ValidateFactNameForReadingOrderReorder : RolePlayerPositionChangeRule
		{
			public override void RolePlayerPositionChanged(RolePlayerOrderChangedEventArgs e)
			{
				if (e.SourceMetaRole.Id == FactTypeHasReadingOrder.FactTypeMetaRoleGuid)
				{
					FactType factType = (FactType)e.SourceElement;
					if (!factType.IsRemoved)
					{
						ORMMetaModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
					}
				}
			}
		}
		[RuleOn(typeof(ReadingOrderHasReading))]
		private class ValidateFactNameForReadingReorder : RolePlayerPositionChangeRule
		{
			public override void RolePlayerPositionChanged(RolePlayerOrderChangedEventArgs e)
			{
				if (e.SourceMetaRole.Id == ReadingOrderHasReading.ReadingOrderMetaRoleGuid)
				{
					ReadingOrder order = (ReadingOrder)e.SourceElement;
					FactType factType;
					if (!order.IsRemoved &&
						null != (factType = order.FactType) &&
						!factType.IsRemoved)
					{
						ORMMetaModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
					}
				}
			}
		}
		[RuleOn(typeof(ObjectTypePlaysRole))]
		private class ValidateFactNameForRolePlayerAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				FactType factType = link.PlayedRoleCollection.FactType;
				if (factType != null)
				{
					ORMMetaModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
				}
				RoleProxy proxy;
				if (null != (proxy = link.PlayedRoleCollection.Proxy) &&
					null != (factType = proxy.FactType))
				{
					ORMMetaModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
				}
			}
		}
		[RuleOn(typeof(ObjectTypePlaysRole))]
		private class ValidateFactNameForRolePlayerRemove : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				Role role = link.PlayedRoleCollection;
				FactType factType;
				if (!role.IsRemoved)
				{
					if (null != (factType = role.FactType))
					{
						ORMMetaModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
					}
					RoleProxy proxy;
					if (null != (proxy = role.Proxy) &&
						null != (factType = proxy.FactType))
					{
						ORMMetaModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
					}
				}
			}
		}
		[RuleOn(typeof(ObjectType))]
		private class ValidateFactNameForObjectTypeNameChange : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeId = e.MetaAttribute.Id;
				if (attributeId == ObjectType.NameMetaAttributeGuid)
				{
					ObjectType objectType = e.ModelElement as ObjectType;
					if (!objectType.IsRemoved)
					{
						RoleMoveableCollection playedRoles = objectType.PlayedRoleCollection;
						int playedRolesCount = playedRoles.Count;
						for (int i = 0; i < playedRolesCount; ++i)
						{
							Role role = playedRoles[i];
							FactType factType = role.FactType;
							if (factType != null)
							{
								ORMMetaModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
							}
							RoleProxy proxy;
							if (null != (proxy = role.Proxy) &&
								null != (factType = proxy.FactType))
							{
								ORMMetaModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
							}
						}
					}
				}
			}
		}
		#endregion
		#region AutoFix Methods
		/// <summary>
		/// Remove implied (including duplicate) internal uniqueness constraints. Internal
		/// uniqueness constraint A implies internal uniqueness constraint B if the roles of
		/// A form a subset of the roles of B. Running this method will fix a
		/// FactTypeHasImpliedInternalUniquenessConstraintError on this FactType.
		/// </summary>
		public void RemoveImpliedInternalUniquenessConstraints() 
		{
			int iucCount = GetInternalConstraintsCount(ConstraintType.InternalUniqueness);
			if (iucCount == 0)
			{
				return;
			}
			using (Transaction t = Store.TransactionManager.BeginTransaction(ResourceStrings.RemoveImpliedInternalUniquenessConstraintsTransactionName))
			{
				RoleBaseMoveableCollection factRoles = RoleCollection;
				UniquenessConstraint[] iuc = new UniquenessConstraint[iucCount];
				const uint deonticBit = 1U << 31;
				uint[] roleBits = new uint[iucCount];
				int index = 0;
				foreach (UniquenessConstraint ic in GetInternalConstraints<UniquenessConstraint>())
				{
					iuc[index] = ic;
					uint bits = 0;
					RoleMoveableCollection constraintRoles = ic.RoleCollection;
					int roleCount = constraintRoles.Count;
					for (int i = 0; i < roleCount; ++i)
					{
						bits |= 1U << factRoles.IndexOf(constraintRoles[i]);
					}
					if (bits != 0 && ic.Modality == ConstraintModality.Deontic)
					{
						bits |= deonticBit;
					}
					roleBits[index] = bits;
					++index;
				}
				int rbLength = roleBits.Length;
				uint left, right, compare;
				UniquenessConstraint leftIUC;
				UniquenessConstraint rightIUC;
				for (int i = 0; i < rbLength - 1; ++i)
				{
					leftIUC = iuc[i];
					if (leftIUC == null)
					{
						continue;
					}

					// Do duplicates first to simplify processing of implied cases
					left = roleBits[i];
					for (int j = i + 1; j < rbLength; ++j)
					{
						rightIUC = iuc[j];
						if (rightIUC == null)
						{
							continue;
						}
						right = roleBits[j];
						compare = left & right;
						if (0 != ((left ^ right) & deonticBit))
						{
							// Modality is different. The alethic constraint
							// should not be a subset of the deontic constraint,
							// but there are no restrictions the other way around.
							if ((compare == (left & ~deonticBit)) && (compare == (right & ~deonticBit)))
							{
								// Found a duplicate. Remove the deontic one
								if (0 != (left & deonticBit))
								{
									leftIUC.Remove();
									iuc[i] = null;
									left = 0;
									break;
								}
								else
								{
									rightIUC.Remove();
									iuc[j] = null;
								}
							}
						}
						else
						{
							if ((compare == left) && (compare == right))
							{
								// found a duplicate.
								// Remove the one on the right so we can
								// keep processing this element
								rightIUC.Remove();
								iuc[j] = null;
							}
						}
					}
					if (left == 0)
					{
						continue;
					}
					for (int j = i + 1; j < rbLength; ++j)
					{
						rightIUC = iuc[j];
						right = roleBits[j];
						if (rightIUC == null || right == 0)
						{
							continue;
						}

						compare = left & right;
						if (0 != ((left ^ right) & deonticBit))
						{
							// Modality is different. The alethic constraint
							// should not be a subset of the deontic constraint,
							// but there are no restrictions the other way around.
							if (compare == (left & ~deonticBit))
							{
								// left implies right unless left is deontic
								if (0 == (left & deonticBit))
								{
									rightIUC.Remove();
									iuc[j] = null;
								}
							}
							else if (compare == (right & ~deonticBit))
							{
								// right implies left unless right is deontic
								if (0 == (right & deonticBit))
								{
									leftIUC.Remove();
									iuc[i] = null;
									break;
								}
							}
						}
						else
						{
							if (compare == left)
							{
								// left implies right
								rightIUC.Remove();
								iuc[j] = null;
							}
							else if (compare == right)
							{
								// right implies left
								leftIUC.Remove();
								iuc[i] = null;
								break;
							}
						}
					}
				}
				if (t.HasPendingChanges)
				{
					t.Commit();
				}
			}
		}
		#endregion // AutoFix Methods
		#region IVerbalizeFilterChildren Implementation
		/// <summary>
		/// Implements IVerbalizeFilterChildren.FilterChildVerbalizer
		/// </summary>
		protected CustomChildVerbalizer FilterChildVerbalizer(IVerbalize childVerbalizer, bool isNegative)
		{
			if (!isNegative && Shell.OptionsPage.CurrentCombineMandatoryAndUniqueVerbalization)
			{
				IConstraint constraint = childVerbalizer as IConstraint;
				if (constraint != null && constraint.ConstraintIsInternal)
				{
					RoleBaseMoveableCollection factRoles = RoleCollection;
					if (factRoles.Count == 2)
					{
						ConstraintModality modality = constraint.Modality;
						// See if we want to do an 'exactly one' instead of 'at most one'/'some'
						MandatoryConstraint mandatory;
						UniquenessConstraint iuc;
						if (null != (mandatory = constraint as MandatoryConstraint))
						{
							foreach (ConstraintRoleSequence testConstraint in mandatory.RoleCollection[0].ConstraintRoleSequenceCollection)
							{
								UniquenessConstraint testIuc = testConstraint as UniquenessConstraint;
								if (testIuc != null &&
									testIuc.IsInternal &&
									testIuc.Modality == modality &&
									testIuc.RoleCollection.Count == 1)
								{
									// Don't verbalize the mandatory role
									return CustomChildVerbalizer.Empty;
								}
							}
						}
						else if (null != (iuc = constraint as UniquenessConstraint) && iuc.IsInternal)
						{
							RoleMoveableCollection roles = iuc.RoleCollection;
							if (roles.Count == 1)
							{
								foreach (ConstraintRoleSequence testConstraint in roles[0].ConstraintRoleSequenceCollection)
								{
									MandatoryConstraint testMandatory = testConstraint as MandatoryConstraint;
									if (testMandatory != null &&
										testMandatory.IsSimple &&
										testMandatory.Modality == modality)
									{
										// Fold the verbalizations into one
										CombinedMandatoryUniqueVerbalizer verbalizer = CombinedMandatoryUniqueVerbalizer.GetVerbalizer();
										verbalizer.Initialize(this, iuc);
										return new CustomChildVerbalizer(verbalizer, true);
									}
								}
							}
						}
					}
				}
			}
			return new CustomChildVerbalizer(childVerbalizer);
		}
		CustomChildVerbalizer IVerbalizeFilterChildren.FilterChildVerbalizer(IVerbalize childVerbalizer, bool isNegative)
		{
			return FilterChildVerbalizer(childVerbalizer, isNegative);
		}
		#endregion // IVerbalizeFilterChildren Implementation
		#region CombinedMandatoryUniqueVerbalizer class
		/// <summary>
		/// Non-generated portions of verbalization helper used to verbalize a
		/// combined internal uniqueness constraint and simple mandatory constraint.
		/// </summary>
		private partial class CombinedMandatoryUniqueVerbalizer
		{
			private FactType myFact;
			private UniquenessConstraint myConstraint;
			public void Initialize(FactType fact, UniquenessConstraint constraint)
			{
				myFact = fact;
				myConstraint = constraint;
			}
			private void DisposeHelper()
			{
				myFact = null;
				myConstraint = null;
			}
			private FactType FactType
			{
				get
				{
					return myFact;
				}
			}
			private RoleMoveableCollection RoleCollection
			{
				get
				{
					return myConstraint.RoleCollection;
				}
			}
			private ConstraintModality Modality
			{
				get
				{
					return myConstraint.Modality;
				}
			}
		}
		#endregion // CombinedMandatoryUniqueVerbalizer class
		#region IVerbalizeCustomChildren Implementation
		/// <summary>
		/// Implements IVerbalizeCustomChildren.GetCustomChildVerbalizations
		/// </summary>
		protected IEnumerable<CustomChildVerbalizer> GetCustomChildVerbalizations(bool isNegative)
		{
			if (!isNegative && Shell.OptionsPage.CurrentShowDefaultConstraintVerbalization)
			{
				RoleBaseMoveableCollection factRoles = RoleCollection;
				if (factRoles.Count == 2)
				{
					foreach (UniquenessConstraint contextIuc in GetInternalConstraints<UniquenessConstraint>())
					{
						if (contextIuc.Modality == ConstraintModality.Alethic)
						{
							RoleMoveableCollection roles = contextIuc.RoleCollection;
							if (roles.Count == 1)
							{
								// We have an appropriate context role. See if there
								// a single-role constraint opposite it. If not, then
								// we provide the default verbalization.
								RoleBase oppositeRole = factRoles[0];
								if (object.ReferenceEquals(oppositeRole, roles[0]))
								{
									oppositeRole = factRoles[1];
								}

								bool provideDefault = true;
								foreach (ConstraintRoleSequence sequence in oppositeRole.Role.ConstraintRoleSequenceCollection)
								{
									UniquenessConstraint iucTest = sequence as UniquenessConstraint;
									if (iucTest != null && iucTest.IsInternal && iucTest.RoleCollection.Count == 1 && iucTest.Modality == ConstraintModality.Alethic)
									{
										provideDefault = false;
										break;
									}
								}
								if (provideDefault)
								{
									DefaultBinaryMissingUniquenessVerbalizer verbalizer = DefaultBinaryMissingUniquenessVerbalizer.GetVerbalizer();
									verbalizer.Initialize(this, contextIuc);
									yield return new CustomChildVerbalizer(verbalizer, true);
								}
								break;
							}
						}
					}
				}
			}
		}
		IEnumerable<CustomChildVerbalizer> IVerbalizeCustomChildren.GetCustomChildVerbalizations(bool isNegative)
		{
			return GetCustomChildVerbalizations(isNegative);
		}
		#endregion // IVerbalizeCustomChildren Implementation
		#region DefaultBinaryMissingUniquenessVerbalizer
		private partial class DefaultBinaryMissingUniquenessVerbalizer
		{
			private FactType myFact;
			private UniquenessConstraint myConstraint;
			public void Initialize(FactType fact, UniquenessConstraint constraint)
			{
				myFact = fact;
				myConstraint = constraint;
			}
			private void DisposeHelper()
			{
				myFact = null;
				myConstraint = null;
			}
			private FactType FactType
			{
				get
				{
					return myFact;
				}
			}
			private RoleMoveableCollection RoleCollection
			{
				get
				{
					return myConstraint.RoleCollection;
				}
			}
			private ConstraintModality Modality
			{
				get
				{
					return myConstraint.Modality;
				}
			}
		}
		#endregion // DefaultBinaryMissingUniquenessVerbalizer
	}

	#region FactType Model Validation Errors
	
	#region class FactTypeRequiresReadingError
	partial class FactTypeRequiresReadingError : IRepresentModelElements
	{
		#region overrides

		/// <summary>
		/// Creates error text for when a fact has no readings.
		/// </summary>
		public override void GenerateErrorText()
		{
			string newText = String.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorFactTypeRequiresReadingMessage, FactType.Name, Model.Name);
			if (Name != newText)
			{
				Name = newText;
			}
		}

		/// <summary>
		/// Sets regernate to ModelNameChange | OwnerNameChange
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get 
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}

		#endregion

		#region IRepresentModelElements Members

		/// <summary>
		/// The fact the error belongs to
		/// </summary>
		protected ModelElement[] GetRepresentedElements()
		{
			return new ModelElement[] { this.FactType };
		}

		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion
	}
	#endregion // class FactTypeRequiresReadingError
	#region class FactTypeRequiresInternalUniquenessConstraintError
	partial class FactTypeRequiresInternalUniquenessConstraintError : IRepresentModelElements
	{
		#region overrides

		/// <summary>
		/// Creates error text for when a fact lacks an internal uniqueness constraint.
		/// </summary>
		public override void GenerateErrorText()
		{
			string newText = String.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorFactTypeRequiresInternalUniquenessConstraintMessage, FactType.Name, Model.Name);
			if (Name != newText)
			{
				Name = newText;
			}
		}

		/// <summary>
		/// Sets regernate to ModelNameChange | OwnerNameChange
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		#endregion

		#region IRepresentModelElements Members

		/// <summary>
		/// The fact the error belongs to
		/// </summary>
		protected ModelElement[] GetRepresentedElements()
		{
			return new ModelElement[] { this.FactType };
		}

		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}

		#endregion
	}
	#endregion // class FactTypeRequiresInternalUniquenessConstraintError
	#region class NMinusOneError

	public partial class NMinusOneError : IRepresentModelElements
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			UniquenessConstraint iuc = Constraint;
			FactType factType = iuc.FactTypeCollection[0];
			string newText = string.Format(CultureInfo.InvariantCulture, ResourceStrings.NMinusOneRuleInternalSpan, iuc.Name, factType.Name, factType.Model.Name, factType.RoleCollection.Count - 1);
			if (Name != newText)
			{
				Name = newText;
			}
		}
		/// <summary>
		/// Regenerate the error text when the constraint name changes
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.OwnerNameChange | RegenerateErrorTextEvents.ModelNameChange;
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
			return new ModelElement[] { Constraint };
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion // IRepresentModelElements Implementation
	}
	#endregion //class NMinusOneError

	#endregion // FactType Model Validation Errors

	#region FactTypeDerivationExpression
	public partial class FactTypeDerivationExpression
	{
		[RuleOn(typeof(FactTypeDerivationExpression))]
		private class FactTypeDerivationExpressionChangeRule : ChangeRule
		{
			/// <summary>
			/// check the Body property of the FactTypeDerivationExpression and delete the FactTypeDerivationExpression 
			/// if Body is empty
			/// </summary>
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeGuid = e.MetaAttribute.Id;
				if (attributeGuid == FactTypeDerivationExpression.BodyMetaAttributeGuid)
				{
					FactTypeDerivationExpression ftde = e.ModelElement as FactTypeDerivationExpression;
					if (!ftde.IsRemoved && string.IsNullOrEmpty(ftde.Body))
					{
						ftde.Remove();
					}
				}
			}
		}
	}
	#endregion
}
