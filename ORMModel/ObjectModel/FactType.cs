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

namespace Neumont.Tools.ORM.ObjectModel
{
	#region IFactConstraint interface
	/// <summary>
	/// A constraint is defined such that it can have
	/// roles that span multiple fact types. The core
	/// model makes it difficult to determine which roles
	/// on a fact are used by a given constraint. ExternalFactConstraint
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
	public partial class FactType : INamedElementDictionaryChild, INamedElementDictionaryRemoteParent, INamedElementDictionaryParent, IModelErrorOwner
	{
		#region ReadingOrder acquisition
		/// <summary>
		/// Gets a reading order, first by trying to find it, if one doesn't exist
		/// it will then create a new ReadingOrder. It operates under the assumption
		/// that a transaction has already been started.
		/// </summary>
		/// <returns></returns>
		public static ReadingOrder GetReadingOrder(FactType theFact, Role[] roleOrder)
		{
			ReadingOrder retval = FindMatchingReadingOrder(theFact, roleOrder);
			if (retval == null)
			{
				retval = CreateReadingOrder(theFact, roleOrder);
			}
			return retval;
		}

		/// <summary>
		/// Lookes for a ReadingOrder that has the roles in the same order
		/// as the currently selected role order.
		/// </summary>
		/// <returns>The reading order if found, null if it was not.</returns>
		public static ReadingOrder FindMatchingReadingOrder(FactType theFact, Role[] roleOrder)
		{
			ReadingOrder retval = null;
			ReadingOrderMoveableCollection readingOrders = theFact.ReadingOrderCollection;
			foreach (ReadingOrder order in readingOrders)
			{
				RoleMoveableCollection roles = order.RoleCollection;
				int numRoles = roles.Count;
				if (numRoles == roleOrder.Length)
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
			RoleMoveableCollection factRoles = theFact.RoleCollection;
			Role[] roleOrder = new Role[factRoles.Count];
			factRoles.CopyTo(roleOrder, 0);
			return FindMatchingReadingOrder(theFact, roleOrder);
		}

		/// <summary>
		/// Creates a new ReadingOrder with the same role sequence as the currently selected one.
		/// A transaction should have been pushed before calling this method. It operates under
		/// the assumption that a transaction has already been started.
		/// </summary>
		/// <returns>Should always return a value unless there was an error creating the ReadingOrder</returns>
		public static ReadingOrder CreateReadingOrder(FactType theFact, Role[] roleOrder)
		{
			ReadingOrder retval = null;
			if (roleOrder.Length > 0)
			{
				retval = ReadingOrder.CreateReadingOrder(theFact.Store);
				RoleMoveableCollection readingRoles = retval.RoleCollection;
				int numRoles = roleOrder.Length;
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
		/// Get a read-only collection of FactConstraint links. Use the
		/// appropriate methods on IFactConstraint to get to the Constraint
		/// and RoleCollection values for each returned constraint.
		/// </summary>
		public ICollection<IFactConstraint> ExternalFactConstraintCollection
		{
			get
			{
				return new FactConstraintCollectionImpl(this, false, true);
			}
		}
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
				return new FactConstraintCollectionImpl(this, true, true);
			}
		}
		/// <summary>
		/// Get an enumeration of constraints of the given type
		/// </summary>
		/// <param name="filterType">The type of constraint to return</param>
		/// <returns>IEnumerable</returns>
		public IEnumerable<InternalConstraint> GetInternalConstraints(ConstraintType filterType)
		{
			IList constraints = InternalConstraintCollection;
			int constraintCount = constraints.Count;
			for (int i = 0; i < constraintCount; ++i)
			{
				InternalConstraint ic = (InternalConstraint)constraints[i];
				if (ic.Constraint.ConstraintType == filterType)
				{
					yield return ic;
				}
			}
		}
		/// <summary>
		/// Get an enumeration of constraints of the given type using generics
		/// </summary>
		/// <typeparam name="T">An internal constraint type</typeparam>
		/// <returns>IEnumerable</returns>
		public IEnumerable<T> GetInternalConstraints<T>() where T : InternalConstraint
		{
			IList constraints = InternalConstraintCollection;
			int constraintCount = constraints.Count;
			for (int i = 0; i < constraintCount; ++i)
			{
				T ic = constraints[i] as T;
				if (ic != null)
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
			IEnumerator<InternalConstraint> ienum = GetInternalConstraints(filterType).GetEnumerator();
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
			/// <param name="includeInternalConstraints">true to include internal fact constraints</param>
			/// <param name="includeExternalConstraints">true to include external fact constraints</param>
			public FactConstraintCollectionImpl(FactType factType, bool includeInternalConstraints, bool includeExternalConstraints)
			{
				Debug.Assert(includeExternalConstraints || includeExternalConstraints);
				int total = 0;
				if (includeInternalConstraints)
				{
					++total;
				}
				if (includeExternalConstraints)
				{
					total += 2;
				}
				myLists = new IList[total];
				int externalIndex = 0;
				if (includeInternalConstraints)
				{
					myLists[0] = factType.InternalConstraintCollection;
					++externalIndex;
				}
				if (includeExternalConstraints)
				{
					myLists[externalIndex] = factType.GetElementLinks(SingleColumnExternalFactConstraint.FactTypeCollectionMetaRoleGuid);
					myLists[externalIndex + 1] = factType.GetElementLinks(MultiColumnExternalFactConstraint.FactTypeCollectionMetaRoleGuid);
				}
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
			return (NestingType == null) ? ResourceStrings.FactType : ResourceStrings.ObjectifiedFactType;
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
				return (classInfo.IsDerivedFrom(InternalUniquenessConstraint.MetaClassGuid));
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
			InternalUniquenessConstraint internalConstraint;
			if (null != (internalConstraint = sourceElement as InternalUniquenessConstraint))
			{
				internalConstraint.FactType = this;
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
			if (attributeGuid == NestingTypeDisplayMetaAttributeGuid)
			{
				// Handled by FactTypeChangeRule
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
				return NestingType;
			}
			return base.GetValueForCustomStoredAttribute(attribute);
		}
		/// <summary>
		/// Standard override. Defer to GetValueForCustomStoredAttribute.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <returns></returns>
		protected override object GetOldValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			return GetValueForCustomStoredAttribute(attribute);
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
		private static readonly Guid[] myRemoteNamedElementDictionaryRoles = new Guid[] { FactTypeHasInternalConstraint.FactTypeMetaRoleGuid };
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
		#region INamedElementDictionaryParent implementation
		INamedElementDictionary INamedElementDictionaryParent.GetCounterpartRoleDictionary(Guid parentMetaRoleGuid, Guid childMetaRoleGuid)
		{
			return GetCounterpartRoleDictionary(parentMetaRoleGuid, childMetaRoleGuid);
		}
		/// <summary>
		/// Implements INamedElementDictionaryParent.GetCounterpartRoleDictionary
		/// </summary>
		/// <param name="parentMetaRoleGuid">Guid</param>
		/// <param name="childMetaRoleGuid">Guid</param>
		/// <returns>Model-owned dictionary for constraints</returns>
		public INamedElementDictionary GetCounterpartRoleDictionary(Guid parentMetaRoleGuid, Guid childMetaRoleGuid)
		{
			if (parentMetaRoleGuid == FactTypeHasInternalConstraint.FactTypeMetaRoleGuid)
			{
				ORMModel model = Model;
				if (model != null)
				{
					return ((INamedElementDictionaryParent)model).GetCounterpartRoleDictionary(parentMetaRoleGuid, childMetaRoleGuid);
				}
			}
			return null;
		}
		/// <summary>
		/// Implements INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey
		/// </summary>
		protected static object GetAllowDuplicateNamesContextKey(Guid parentMetaRoleGuid, Guid childMetaRoleGuid)
		{
			// Use the default settings (allow duplicates during load time only)
			return null;
		}
		object INamedElementDictionaryParent.GetAllowDuplicateNamesContextKey(Guid parentMetaRoleGuid, Guid childMetaRoleGuid)
		{
			return GetAllowDuplicateNamesContextKey(parentMetaRoleGuid, childMetaRoleGuid);
		}
		#endregion // INamedElementDictionaryParent implementation
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
					(e.ModelElement as FactType).NestingType = e.NewValue as ObjectType;
				}
			}
		}
		#endregion // RoleChangeRule class
		#region IModelErrorOwner Members

		/// <summary>
		/// Returns the error associated with the fact.
		/// </summary>
		protected new IEnumerable<ModelError> ErrorCollection
		{
			get
			{
				FactTypeRequiresReadingError noReadingError = this.ReadingRequiredError;
				if (noReadingError != null)
				{
					yield return noReadingError;
				}

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

				// NMinusOneError is parented off InternalConstraint, but it doesn't have
				// a name, so we show the FactType as an owner.
				foreach (InternalUniquenessConstraint ic in GetInternalConstraints<InternalUniquenessConstraint>())
				{
					NMinusOneError nMinusOneError = ic.NMinusOneError;
					if (nMinusOneError != null)
					{
						yield return nMinusOneError;
					}
				}
				
				// Show the fact type as an owner of the role errors as well
				// so the fact can be accurately named in the error text. However,
				// we do not validate this error on the fact type, it is done on the role.
				foreach (Role role in RoleCollection)
				{
					RolePlayerRequiredError rolePlayerRequired = role.RolePlayerRequiredError;
					if (rolePlayerRequired != null)
					{
						yield return rolePlayerRequired;
					}
					RoleValueConstraint valueConstraint = role.ValueConstraint;
					if (valueConstraint != null)
					{
						foreach (ValueRange range in valueConstraint.ValueRangeCollection)
						{
							MinValueMismatchError minError = range.MinValueMismatchError;
							if (minError != null)
							{
								yield return minError;
							}
							MaxValueMismatchError maxError = range.MaxValueMismatchError;
							if (maxError != null)
							{
								yield return maxError;
							}
						}
					}
				}

				// The fact name is used in the generated error text, it needs to be an owner
				foreach (FrequencyConstraintContradictsInternalUniquenessConstraintError frequencyContradictionError in  FrequencyConstraintContradictsInternalUniquenessConstraintErrorCollection)
				{
					yield return frequencyContradictionError;
				}

				// Get errors off the base
				foreach (ModelError baseError in base.ErrorCollection)
				{
					yield return baseError;
				}

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
		protected new static void DelayValidateErrors()
		{
			// UNDONE: DelayedValidation (FactType)
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion
		#region Validation Methods

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
						noReadingError.Model = theModel;
						noReadingError.FactType = this;
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
		private void ValidateRequiresInternalUniqueness(INotifyElementAdded notifyAdded)
		{
			ORMModel theModel;
			if (!IsRemoved && (null != (theModel = Model)))
			{
				bool hasError = RoleCollection.Count > 1;
				Store theStore = Store;

				if (hasError)
				{
					using (IEnumerator<InternalUniquenessConstraint> iucs = GetInternalConstraints<InternalUniquenessConstraint>().GetEnumerator())
					{
						hasError = !iucs.MoveNext();
					}
				}

				FactTypeRequiresInternalUniquenessConstraintError noUniquenessError = InternalUniquenessConstraintRequiredError;
				
				if (hasError)
				{
					if (noUniquenessError == null)
					{
						noUniquenessError = FactTypeRequiresInternalUniquenessConstraintError.CreateFactTypeRequiresInternalUniquenessConstraintError(theStore);
						noUniquenessError.Model = theModel;
						noUniquenessError.FactType = this;
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
		private void ValidateImpliedInternalUniqueness(INotifyElementAdded notifyAdded)
		{

			ORMModel theModel;
			if (!IsRemoved && (null != (theModel = Model)))
			{
				Store theStore = Store;
				RoleMoveableCollection factRoles = RoleCollection;
				bool hasError = false;
				int iucCount = GetInternalConstraintsCount(ConstraintType.InternalUniqueness);
				if (iucCount != 0)
				{
					int[] roleBits = new int[iucCount];
					int index = 0;
					foreach (InternalUniquenessConstraint ic in GetInternalConstraints<InternalUniquenessConstraint>())
					{
						int bits = 0;
						RoleMoveableCollection constraintRoles = ic.RoleCollection;
						int roleCount = constraintRoles.Count;
						for (int i = 0; i < roleCount; ++i)
						{
							bits |= 1 << factRoles.IndexOf(constraintRoles[i]);
						}
						roleBits[index] = bits;
						++index;
					}
					int rbLength = roleBits.Length;
					for (int i = 0; !hasError && i < rbLength - 1; ++i)
					{
						for (int j = i + 1; j < rbLength; ++j)
						{
							int left = roleBits[i];
							int right = roleBits[j];
							if (left != 0 && right != 0)
							{
								int	compare = left & right;
								if ((compare == left) || (compare == right))
								{
									hasError = true;
									break;
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
						impConstraint.Model = theModel;
						impConstraint.FactType = this;
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
		#region Model Validation Rules
		/// <summary>
		/// Internal uniqueness constraints are required for non-unary facts. Requires
		/// validation when roles are added and removed.
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole), FireTime=TimeToFire.LocalCommit)]
		private class FactTypeHasRoleAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
				link.FactType.ValidateRequiresInternalUniqueness(null);
			}
		}
		/// <summary>
		/// Internal uniqueness constraints are required for non-unary facts. Requires
		/// validation when roles are added and removed.
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole), FireTime = TimeToFire.LocalCommit)]
		private class FactTypeHasRoleRemoveRule : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
				link.FactType.ValidateRequiresInternalUniqueness(null);
			}
		}
		/// <summary>
		/// Only validates the InternalUniquenessConstraintRequired error
		/// </summary>
		[RuleOn(typeof(FactTypeHasInternalConstraint), FireTime=TimeToFire.LocalCommit)]
		private class ModelHasInternalConstraintAddRuleModelValidation : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasInternalConstraint link = e.ModelElement as FactTypeHasInternalConstraint;
				FactType fact = link.FactType;
				fact.ValidateRequiresInternalUniqueness(null);
			}
		}


		/// <summary>
		/// Only validates the InternalUniquenessConstraintRequired error
		/// </summary>
		[RuleOn(typeof(FactTypeHasInternalConstraint), FireTime = TimeToFire.LocalCommit)]
		private class ModelHasInternalConstraintRemoveRuleModelValidation : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				FactTypeHasInternalConstraint link = e.ModelElement as FactTypeHasInternalConstraint;
				FactType fact = link.FactType;
				if (!fact.IsRemoved)
				{
					fact.ValidateRequiresInternalUniqueness(null);
				}
			}
		}
		/// <summary>
		///  validates the ImpliedInternalUniquenessConstraintError
		/// </summary>
		[RuleOn(typeof(FactTypeHasInternalConstraint), FireTime = TimeToFire.LocalCommit)]
		private class ImpliedInternalUniquenessConstraintAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasInternalConstraint link = e.ModelElement as FactTypeHasInternalConstraint;
				FactType fact = link.FactType;
				fact.ValidateImpliedInternalUniqueness(null);
			}
		}
		/// <summary>
		///   needed when changing an implied error to a duplicate error 
		/// </summary>
		[RuleOn(typeof(FactTypeHasInternalConstraint), FireTime = TimeToFire.LocalCommit)]
		private class ImpliedInternalUniquenessConstraintRemoveRule : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				FactTypeHasInternalConstraint link = e.ModelElement as FactTypeHasInternalConstraint;
				FactType fact = link.FactType;
				if (!fact.IsRemoved)
				{
					fact.ValidateImpliedInternalUniqueness(null);
				}
			}
		}


		[RuleOn(typeof(ConstraintRoleSequenceHasRole), FireTime = TimeToFire.LocalCommit)]
		private class InternalConstraintCollectionHasConstrintAddedRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				InternalUniquenessConstraint constr = link.ConstraintRoleSequenceCollection as InternalUniquenessConstraint;
				if (constr != null)
				{
					FactType fact = constr.FactType;
					if (fact != null && !fact.IsRemoved)
					{
						fact.ValidateImpliedInternalUniqueness(null);
					}
					
				}
			}
		}

		[RuleOn(typeof(ConstraintRoleSequenceHasRole), FireTime = TimeToFire.LocalCommit)]
		private class InternalConstraintCollectionHasConstraintRemovedRule : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				InternalUniquenessConstraint constr = link.ConstraintRoleSequenceCollection as InternalUniquenessConstraint;
				if (constr != null)
				{
					FactType fact = constr.FactType;
					if (fact != null && !fact.IsRemoved)
					{
						fact.ValidateImpliedInternalUniqueness(null);
					}
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
						// UNDONE: DelayedValidation
						fact.ValidateErrors(null);
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
				FactTypeHasReadingOrder link = e.ModelElement as FactTypeHasReadingOrder;
				FactType fact = link.FactType;
				if (fact.ReadingRequiredError != null)
				{
					fact.ValidateRequiresReading(null);
				}
			}
		}
		/// <summary>
		/// Only validates ReadingRequiredError
		/// </summary>
		[RuleOn(typeof(FactTypeHasReadingOrder), FireTime = TimeToFire.LocalCommit)]
		private class FactTypeHasReadingOrderRemovedRuleModelValidation : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				FactTypeHasReadingOrder link = e.ModelElement as FactTypeHasReadingOrder;
				FactType fact = link.FactType;
				if (!fact.IsRemoved)
				{
					fact.ValidateRequiresReading(null);
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
				ReadingOrderHasReading link = e.ModelElement as ReadingOrderHasReading;
				ReadingOrder ord = link.ReadingOrder;
				FactType fact = ord.FactType;
				if (fact != null)
				{
					fact.ValidateRequiresReading(null);
				}
			}
		}
		/// <summary>
		/// Only validates ReadingRequiredError
		/// </summary>
		[RuleOn(typeof(ReadingOrderHasReading), FireTime = TimeToFire.LocalCommit)]
		private class ReadingOrderHasReadingRemoveRuleModelValidation : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ReadingOrderHasReading link = e.ModelElement as ReadingOrderHasReading;
				ReadingOrder ord = link.ReadingOrder;
				FactType fact;
				if (!ord.IsRemoved &&
					null != (fact = ord.FactType) &&
					!fact.IsRemoved)
				{
					fact.ValidateRequiresReading(null);
				}
			}
		}
		#endregion
		#region AutoFix Methods
		/// <summary>
		/// Remove implied (including duplicate) internal uniquess constraints. Internal
		/// uniqueness constraint A implies internal uniqueness constraint B if the roles of
		/// A form a subset of the roles of B. Running this method will fix a
		/// FactTypeHasImpliedInternalUniquessConstraintError on this FactType.
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
				RoleMoveableCollection factRoles = RoleCollection;
				InternalUniquenessConstraint[] iuc = new InternalUniquenessConstraint[iucCount];
				int[] roleBits = new int[iucCount];
				int index = 0;
				foreach (InternalUniquenessConstraint ic in GetInternalConstraints<InternalUniquenessConstraint>())
				{
					iuc[index] = ic;
					int bits = 0;
					RoleMoveableCollection constraintRoles = ic.RoleCollection;
					int roleCount = constraintRoles.Count;
					for (int i = 0; i < roleCount; ++i)
					{
						bits |= 1 << factRoles.IndexOf(constraintRoles[i]);
					}
					roleBits[index] = bits;
					++index;
				}
				int rbLength = roleBits.Length;
				int left, right, compare;
				InternalUniquenessConstraint leftIUC;
				InternalUniquenessConstraint rightIUC;
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
						if ((compare == left) && (compare == right))
						{
							// found a duplicate.
							// Remove the one on the right so we can
							// keep processing this element
							rightIUC.Remove();
							iuc[j] = null;
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
				if (t.HasPendingChanges)
				{
					t.Commit();
				}
			}
		}
		#endregion // AutoFix Methods
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
			string newText = String.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorFactTypeRequiresInternalUniquessConstraintMessage, FactType.Name, Model.Name);
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
			FactType factType = Constraint.FactType;
			string newText = string.Format(CultureInfo.InvariantCulture, ResourceStrings.NMinusOneRuleInternalSpan, factType.Name, factType.RoleCollection.Count - 1, factType.Model.Name);
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
}
