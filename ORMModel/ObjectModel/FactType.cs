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
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.Modeling;

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
	public partial class FactType : INamedElementDictionaryChild, INamedElementDictionaryRemoteParent, IModelErrorOwner, IVerbalizeCustomChildren, IHierarchyContextEnabled
	{
		#region ReadingOrder acquisition
		/// <summary>
		/// Gets a reading order, first by trying to find it, if one doesn't exist
		/// it will then create a new ReadingOrder. It operates under the assumption
		/// that a transaction has already been started.
		/// </summary>
		public ReadingOrder GetReadingOrder(IList<RoleBase> roleOrder)
		{
			ReadingOrder retVal = FindMatchingReadingOrder(roleOrder);
			if (retVal == null)
			{
				retVal = CreateReadingOrder(roleOrder);
			}
			return retVal;
		}

		/// <summary>
		/// Looks for a ReadingOrder that has the roles in the same order
		/// as the currently selected role order.
		/// </summary>
		/// <param name="roleOrder">An IList of <see cref="RoleBase"/> elements. </param>
		/// <returns>The reading order if found, null if it was not.</returns>
		public ReadingOrder FindMatchingReadingOrder(IList<RoleBase> roleOrder)
		{
			ReadingOrder retval = null;
			LinkedElementCollection<ReadingOrder> readingOrders = ReadingOrderCollection;
			int roleOrderCount = roleOrder.Count;
			foreach (ReadingOrder order in readingOrders)
			{
				LinkedElementCollection<RoleBase> roles = order.RoleCollection;
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
		/// Creates a new ReadingOrder with the same role sequence as the currently selected one.
		/// A transaction should have been pushed before calling this method. It operates under
		/// the assumption that a transaction has already been started.
		/// </summary>
		/// <returns>Should always return a value unless there was an error creating the ReadingOrder</returns>
		public ReadingOrder CreateReadingOrder(IList<RoleBase> roleOrder)
		{
			ReadingOrder retval = null;
			if (roleOrder.Count > 0)
			{
				retval = new ReadingOrder(Store);
				LinkedElementCollection<RoleBase> readingRoles = retval.RoleCollection;
				int numRoles = roleOrder.Count;
				for (int i = 0; i < numRoles; ++i)
				{
					readingRoles.Add(roleOrder[i]);
				}
				ReadingOrderCollection.Add(retval);
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
			LinkedElementCollection<SetConstraint> constraints = SetConstraintCollection;
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
				return Objectification.GetLinkToNestingType(this);
			}
		}
		#endregion // FactType Specific
		#region FactConstraintCollection implementation
		private sealed class FactConstraintCollectionImpl : ICollection<IFactConstraint>
		{
			#region Member Variables
			private readonly ReadOnlyCollection<ElementLink>[] myLists;
			#endregion // Member Variables
			#region Constructors
			/// <summary>
			/// Create a FactConstraint collection for the given fact type. Fact constraints
			/// come from multiple links, this puts them all together.
			/// </summary>
			/// <param name="factType">The parent fact type</param>
			public FactConstraintCollectionImpl(FactType factType)
			{
				myLists = new ReadOnlyCollection<ElementLink>[]{
					DomainRoleInfo.GetElementLinks<ElementLink>(factType, FactSetConstraint.FactTypeDomainRoleId),
					DomainRoleInfo.GetElementLinks<ElementLink>(factType, FactSetComparisonConstraint.FactTypeDomainRoleId)};
			}
			#endregion // Constructors
			#region ICollection<IFactConstraint> Implementation
			bool ICollection<IFactConstraint>.Contains(IFactConstraint item)
			{
				ElementLink link = item as ElementLink;
				if (link != null)
				{
					ReadOnlyCollection<ElementLink>[] lists = myLists;
					for (int i = 0; i < lists.Length; ++i)
					{
						if (lists[i].Contains(link))
						{
							return true;
						}
					}
				}
				return false;
			}
			void ICollection<IFactConstraint>.CopyTo(IFactConstraint[] array, int arrayIndex)
			{
				ReadOnlyCollection<ElementLink>[] lists = myLists;
				int prevTotal = 0;
				for (int i = 0; i < lists.Length; ++i)
				{
					ReadOnlyCollection<ElementLink> curList = lists[i];
					int curTotal = curList.Count;
					if (curTotal != 0)
					{
						((IList)curList).CopyTo(array, prevTotal);
						prevTotal += curTotal;
					}
				}
			}
			int ICollection<IFactConstraint>.Count
			{
				get
				{
					ReadOnlyCollection<ElementLink>[] lists = myLists;
					int total = 0;
					for (int i = 0; i < lists.Length; ++i)
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
				throw new NotSupportedException();
			}
			void ICollection<IFactConstraint>.Add(IFactConstraint item)
			{
				// Not supported for read-only
				throw new NotSupportedException();
			}
			void ICollection<IFactConstraint>.Clear()
			{
				// Not supported for read-only
				throw new NotSupportedException();
			}
			#endregion // ICollection<IFactConstraint> Implementation
			#region IEnumerable<IFactConstraint> Implementation
			IEnumerator<IFactConstraint> IEnumerable<IFactConstraint>.GetEnumerator()
			{
				ReadOnlyCollection<ElementLink>[] lists = myLists;
				for (int i = 0; i < lists.Length; ++i)
				{
					ReadOnlyCollection<ElementLink> curList = lists[i];
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
		/// Return a simple name instead of a name decorated with the type (the
		/// default for a ModelElement). This is the easiest way to display
		/// clean names in the property grid when we reference properties.
		/// </summary>
		public override string ToString()
		{
			return Name;
		}
		#endregion // Customize property display
		#region MergeContext functions
		/// <summary>
		/// Support adding root elements and constraints directly to the design surface
		/// </summary>
		/// <param name="rootElement">The element to add.</param>
		/// <param name="elementGroupPrototype">The object representing the serialized data being added to this <see cref="FactType"/>.</param>
		/// <returns><see langword="true"/> if addition is allowed; otherwise, <see langword="false"/>.</returns>
		protected override bool CanMerge(ProtoElementBase rootElement, ElementGroupPrototype elementGroupPrototype)
		{
			if (rootElement != null)
			{
				DomainClassInfo classInfo = Store.DomainDataDirectory.FindDomainClass(rootElement.DomainClassId);
				if (classInfo.IsDerivedFrom(UniquenessConstraint.DomainClassId))
				{
					return elementGroupPrototype.UserData == ORMModel.InternalUniquenessConstraintUserDataKey;
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
		protected override void MergeRelate(ModelElement sourceElement, ElementGroup elementGroup)
		{
			base.MergeRelate(sourceElement, elementGroup);
			UniquenessConstraint internalConstraint;
			if (null != (internalConstraint = sourceElement as UniquenessConstraint))
			{
				if (internalConstraint.IsInternal)
				{
					internalConstraint.FactTypeCollection.Add(this);
				}
			}
		}
		#endregion // MergeContext functions
		#region CustomStorage handlers
		private void SetNestingTypeDisplayValue(ObjectType newValue)
		{
			// Handled by FactTypeChangeRule
		}
		private void SetDerivationRuleDisplayValue(string newValue)
		{
			// Handled by FactTypeChangeRule
		}
		private void SetDerivationStorageDisplayValue(DerivationStorageType newValue)
		{
			// Handled by FactTypeChangeRule
		}
		private void SetNoteTextValue(string newValue)
		{
			// Handled by FactTypeChangeRule
		}
		private void SetNameValue(string newValue)
		{
			UndoManager undoManager;
			if ((newValue != null && newValue.Length == 0) ||
					((undoManager = Store.UndoManager).InUndo || undoManager.InRedo))
			{
				// We only set this in undo/redo scenarios so that the initial
				// change on a writable property comes indirectly from the objectifying
				// type changing its name. Anywhere that sets the Name property to
				// put a change in the transaction log needs to set myGeneratedName independently
				// after this call.
				myGeneratedName = newValue;
			}
			// Remainder handled by FactTypeChangeRule
		}
		private ObjectType GetNestingTypeDisplayValue()
		{
			Objectification objectification = Objectification;
			return (objectification != null && !objectification.IsImplied) ? objectification.NestingType : null;
		}
		private string GetDerivationRuleDisplayValue()
		{
			FactTypeDerivationExpression derivation = DerivationRule;
			return (derivation == null || derivation.IsDeleted) ? String.Empty : derivation.Body;
		}
		private DerivationStorageType GetDerivationStorageDisplayValue()
		{
			FactTypeDerivationExpression derivation = DerivationRule;
			return (derivation == null || derivation.IsDeleted) ? DerivationStorageType.Derived : derivation.DerivationStorage;
		}
		private string GetNoteTextValue()
		{
			Note currentNote = Note;
			return (currentNote != null) ? currentNote.Text : String.Empty;
		}
		private string GetNameValue()
		{
			Objectification objectification;
			ObjectType nestingType;
			Store store = Store;
			if (store.InUndo || store.InRedo)
			{
				return myGeneratedName;
			}
			else if ((objectification = Objectification) != null && (nestingType = objectification.NestingType) != null)
			{
				// Use the name from the nesting type
				return nestingType.Name;
			}
			else if (!store.TransactionManager.InTransaction)
			{
				string generatedName = myGeneratedName;
				return String.IsNullOrEmpty(generatedName) ? myGeneratedName = GenerateName() : generatedName;
			}
			else
			{
				string generatedName = myGeneratedName;
				if ((object)generatedName != null && generatedName.Length == 0)
				{
					// The == null here is a hack. Use myGeneratedName = null before calling to skip setting this during a transaction
					return myGeneratedName = GenerateName();
				}
				return generatedName ?? String.Empty;
			}
		}
		private void OnFactTypeNameChanged()
		{
			TransactionManager tmgr = Store.TransactionManager;
			if (tmgr.InTransaction)
			{
				NameChanged = tmgr.CurrentTransaction.SequenceNumber;
			}
		}
		private long GetNameChangedValue()
		{
			TransactionManager tmgr = Store.TransactionManager;
			if (tmgr.InTransaction)
			{
				// Subtract 1 so that we get a difference in the transaction log
				return unchecked(tmgr.CurrentTransaction.SequenceNumber - 1);
			}
			else
			{
				return 0L;
			}
		}
		private void SetNameChangedValue(long newValue)
		{
			// Nothing to do, we're just trying to create a transaction log entry
		}
		#endregion // CustomStorage handlers
		#region INamedElementDictionaryChild implementation
		void INamedElementDictionaryChild.GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			GetRoleGuids(out parentDomainRoleId, out childDomainRoleId);
		}
		/// <summary>
		/// Implementation of INamedElementDictionaryChild.GetRoleGuids. Identifies
		/// this child as participating in the 'ModelHasFactType' naming set.
		/// </summary>
		/// <param name="parentDomainRoleId">Guid</param>
		/// <param name="childDomainRoleId">Guid</param>
		protected static void GetRoleGuids(out Guid parentDomainRoleId, out Guid childDomainRoleId)
		{
			parentDomainRoleId = ModelHasFactType.ModelDomainRoleId;
			childDomainRoleId = ModelHasFactType.FactTypeDomainRoleId;
		}
		#endregion // INamedElementDictionaryChild implementation
		#region INamedElementDictionaryRemoteParent implementation
		private static readonly Guid[] myRemoteNamedElementDictionaryRoles = new Guid[] { FactTypeHasRole.FactTypeDomainRoleId };
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
		#region FactTypeChangeRule class
		[RuleOn(typeof(FactType))] // ChangeRule
		private sealed partial class FactTypeChangeRule : ChangeRule
		{
			/// <summary>
			/// Forward through the property grid property to the underlying
			/// nesting type property
			/// </summary>
			/// <param name="e"></param>
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				Guid attributeGuid = e.DomainProperty.Id;
				if (attributeGuid == FactType.NestingTypeDisplayDomainPropertyId)
				{
					Objectification.CreateExplicitObjectification(e.ModelElement as FactType, e.NewValue as ObjectType);
				}
				else if (attributeGuid == FactType.DerivationRuleDisplayDomainPropertyId)
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
							currentRule = new FactTypeDerivationExpression(factType.Store);
							factType.DerivationRule = currentRule;
						}
						currentRule.Body = newVal;
					}
				}
				else if (attributeGuid == FactType.DerivationStorageDisplayDomainPropertyId)
				{
					FactType factType = e.ModelElement as FactType;
					if (factType.DerivationRule != null)
					{
						factType.DerivationRule.DerivationStorage = (DerivationStorageType)e.NewValue;
					}
				}
				else if (attributeGuid == FactType.NoteTextDomainPropertyId)
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
						note = new Note(factType.Store);
						note.Text = newText;
						// then attach the note to the RootType.
						factType.Note = note;
					}
				}
				else if (attributeGuid == FactType.NameDomainPropertyId)
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
		#endregion // FactTypeChangeRule class
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Returns the error associated with the fact.
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			if (filter == 0)
			{
				filter = (ModelErrorUses)(-1);
			}
			if (0 != (filter & (ModelErrorUses.BlockVerbalization | ModelErrorUses.DisplayPrimary)))
			{
				FactTypeRequiresReadingError noReadingError = this.ReadingRequiredError;
				if (noReadingError != null)
				{
					yield return new ModelErrorUsage(noReadingError, ModelErrorUses.BlockVerbalization);
				}
			}

			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
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

			if (0 == (filter & (ModelErrorUses.Verbalize | ModelErrorUses.BlockVerbalization | ModelErrorUses.DisplayPrimary)) || filter == (ModelErrorUses)(-1))
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
			if (0 == (filter & (ModelErrorUses.BlockVerbalization | ModelErrorUses.DisplayPrimary)) || filter == (ModelErrorUses)(-1)) // Roles don't verbalize, we need to show these here
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
						if (filter == (ModelErrorUses)(-1))
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
				if (0 != (filter & (ModelErrorUses.BlockVerbalization | ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
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
			}
			if (filter == ModelErrorUses.DisplayPrimary || filter == ModelErrorUses.Verbalize)
			{
				// If we're objectified, list primary errors from the objectifying type
				// here as well. Note that we should verbalize anything we list in our
				// validation errors
				ObjectType nestingType = NestingType;
				if (nestingType != null)
				{
					// Always ask for 'DisplayPrimary', even if we're verbalizing
					// None of these should list as blocking verbalization here, even if they're blocking on the nesting
					foreach (ModelError nestingError in (nestingType as IModelErrorOwner).GetErrorCollection(ModelErrorUses.DisplayPrimary))
					{
						yield return new ModelErrorUsage(nestingError, ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary);
					}
				}
			}

			if (filter == (ModelErrorUses)(- 1))
			{
				LinkedElementCollection<FactTypeInstance> factTypeInstances = this.FactTypeInstanceCollection;
				int factTypeInstanceCount = factTypeInstances.Count;
				for(int i = 0; i < factTypeInstanceCount; ++i)
				{
					FactTypeInstance factTypeInstance = factTypeInstances[i];
					foreach (ModelErrorUsage usage in (factTypeInstance as IModelErrorOwner).GetErrorCollection(filter))
					{
						yield return usage;
					}
					LinkedElementCollection<FactTypeRoleInstance> roleInstances = factTypeInstance.RoleInstanceCollection;
					int roleInstanceCount = roleInstances.Count;
					for (int j = 0; j < roleInstanceCount; ++j)
					{
						PopulationUniquenessError populationError = roleInstances[j].PopulationUniquenessError;
						if (populationError != null)
						{
							yield return populationError;
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
			ORMCoreDomainModel.DelayValidateElement(this, DelayValidateFactTypeRequiresReadingError);
			ORMCoreDomainModel.DelayValidateElement(this, DelayValidateFactTypeRequiresInternalUniquenessConstraintError);
			ORMCoreDomainModel.DelayValidateElement(this, DelayValidateImpliedInternalUniquenessConstraintError);
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner Implementation
		#region Virtual methods for implicit reading support
		/// <summary>
		/// Allow fact types to have implicit readings without using the ReadingCollection
		/// </summary>
		public virtual bool HasImplicitReadings
		{
			get
			{
				return false;
			}
		}
		/// <summary>
		/// Generate the implicit name for this FactType. Called only
		/// if the <see cref="HasImplicitReadings"/> property returns
		/// <see langword="true"/>
		/// </summary>
		/// <returns>An empty string</returns>
		protected virtual string GenerateImplicitName()
		{
			Debug.Fail("GenerateImplicitName must be overridden if HasImplicitReadings is overridden to return true.");
			return "";
		}
		/// <summary>
		/// Generate an implicit reading for the specified lead role. Called only
		/// if the <see cref="HasImplicitReadings"/> property returns <see langword="true"/>
		/// </summary>
		/// <param name="leadRole">The role that should begin the reading</param>
		/// <returns><see cref="IReading"/></returns>
		protected virtual IReading GetImplicitReading(RoleBase leadRole)
		{
			Debug.Fail("GetImplicitReading must be overridden if HasImplicitReadings is overridden to return true.");
			return null;
		}
		#region ImplicitReading class
		/// <summary>
		/// A helper class used to add implicit readings. Can be used to
		/// implement <see cref="GetImplicitReading"/>.
		/// </summary>
		protected sealed class ImplicitReading : IReading
		{
			#region Member Variables
			private string myReadingText;
			private IList<RoleBase> myRoleCollection;
			#endregion // Member Variables
			#region Constructors
			/// <summary>
			/// Create a new implicit reading
			/// </summary>
			/// <param name="readingText">The reading format string</param>
			/// <param name="roleOrder">The role order to use.</param>
			public ImplicitReading(string readingText, IList<RoleBase> roleOrder)
			{
				myRoleCollection = roleOrder;
				myReadingText = readingText;
			}
			#endregion // Constructors
			#region IReading Implementation
			string IReading.Text
			{
				get
				{
					return myReadingText;
				}
			}

			IList<RoleBase> IReading.RoleCollection
			{
				get
				{
					return myRoleCollection;
				}
			}
			bool IReading.IsEditable
			{
				get
				{
					return false;
				}
			}
			#endregion //IReading Implementation
		}
		#endregion // ImplicitReading class
		#endregion // Virtual methods for implicit reading support
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
			if (!IsDeleted)
			{
				// UNDONE: On the next format change these errors should not
				// be allowed to load or the readings to be created, so we don't have
				// to look for readings or the reading required error if HasImplicitReadings is true.
				bool hasError = !HasImplicitReadings;
				if (hasError)
				{
					LinkedElementCollection<ReadingOrder> readingOrders = ReadingOrderCollection;
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
				}

				FactTypeRequiresReadingError noReadingError = ReadingRequiredError;
				if (hasError)
				{
					if (noReadingError == null)
					{
						noReadingError = new FactTypeRequiresReadingError(Store);
						noReadingError.FactType = this;
						noReadingError.Model = Model;
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
						noReadingError.Delete();
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
			if (!IsDeleted && (null != (theModel = Model)))
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
						noUniquenessError = new FactTypeRequiresInternalUniquenessConstraintError(theStore);
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
					noUniquenessError.Delete();
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
			if (!IsDeleted && (null != (theModel = Model)))
			{
				Store theStore = Store;
				LinkedElementCollection<RoleBase> factRoles = RoleCollection;
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
						LinkedElementCollection<Role> constraintRoles = ic.RoleCollection;
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
						impConstraint = new ImpliedInternalUniquenessConstraintError(theStore);
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
					impConstraint.Delete();
				}
			}
		}
		#endregion // Validation Methods
		#region Automatic Name Generation
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// synchronizes the initial name settings with any objectifying fact.
		/// </summary>
		public static IDeserializationFixupListener NameFixupListener
		{
			get
			{
				return new GeneratedNameFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation. Properly initializes the myGeneratedName field
		/// </summary>
		private sealed class GeneratedNameFixupListener : DeserializationFixupListener<Objectification>
		{
			/// <summary>
			/// ExternalConstraintFixupListener constructor
			/// </summary>
			public GeneratedNameFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateElementNames)
			{
			}
			/// <summary>
			/// Process objectification elements
			/// </summary>
			/// <param name="element">An Objectification element</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(Objectification element, Store store, INotifyElementAdded notifyAdded)
			{
				if (!element.IsDeleted)
				{
					FactType factType = element.NestedFactType;
					string generatedName = factType.GenerateName();
					if (generatedName == element.NestingType.Name)
					{
						factType.myGeneratedName = generatedName;
					}
				}
			}
		}
		#endregion // Deserialization Fixup
		private static void DelayValidateFactTypeNamePartChanged(ModelElement element)
		{
			FactType factType = element as FactType;
			if (!factType.IsDeleted)
			{
				Store store = element.Store;
				string oldGeneratedName = factType.myGeneratedName;
				bool haveNewName = false;
				string newGeneratedName = null;
				bool raiseEvent = true;

				// See if the nestedType uses the old automatic name. If it does, then
				// update the automatic name to the the new name.
				ObjectType nestingType = null;
				Objectification objectificationLink;
				if (null != (objectificationLink = factType.Objectification) &&
					null != (nestingType = objectificationLink.NestingType) &&
					!nestingType.IsDeleted)
				{
					newGeneratedName = factType.GenerateName();
					haveNewName = true;
					if (newGeneratedName != oldGeneratedName)
					{
						if (nestingType.Name == oldGeneratedName)
						{
							Dictionary<object, object> contextInfo = store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo;
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
									factType.myGeneratedName = null; // Set explicitly to null, see notes in GetNameValue
								}
								factType.Name = newGeneratedName;
								factType.myGeneratedName = newGeneratedName; // See notes in SetNameValue on setting myGeneratedName
								contextInfo[ORMModel.AllowDuplicateNamesKey] = null;
								nestingType.Name = newGeneratedName;
							}
							finally
							{
								contextInfo.Remove(ORMModel.AllowDuplicateNamesKey);
								if (ruleDisabled)
								{
									ruleManager.EnableRule(typeof(FactTypeChangeRule));
								}
							}
						}
						else
						{
							// Rule updates for this case are handled in ValidateFactNameForObjectTypeNameChange
							haveNewName = false;
							newGeneratedName = null;
							raiseEvent = false;
						}
					}
					else
					{
						newGeneratedName = null;
					}
				}

				if (raiseEvent && (!haveNewName || newGeneratedName != null))
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
											factType.myGeneratedName = null; // Set explicitly to null, see notes in GetNameValue
										}
										factType.Name = newGeneratedName;
										factType.myGeneratedName = newGeneratedName; // See notes in SetNameValue on setting myGeneratedName
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
				if (raiseEvent)
				{
					factType.OnFactTypeNameChanged();
				}
			}
		}
		/// <summary>
		/// Helper function to get the current setting for the generated Name property
		/// </summary>
		private string GenerateName()
		{
			string retVal = "";
			if (!IsDeleted)
			{
				if (HasImplicitReadings)
				{
					return GenerateImplicitName();
				}
				// Grab the first reading with no errors from the first reading order
				// Note that the first reading in the first reading order is considered
				// to be the default reading order
				LinkedElementCollection<RoleBase> roles = null;
				string formatText = null;
				LinkedElementCollection<ReadingOrder> readingOrders = ReadingOrderCollection;
				int readingOrdersCount = readingOrders.Count;
				for (int i = 0; i < readingOrdersCount && formatText == null; ++i)
				{
					ReadingOrder order = readingOrders[i];
					LinkedElementCollection<Reading> readings = order.ReadingCollection;
					int readingsCount = readings.Count;
					for (int j = 0; j < readingsCount; ++j)
					{
						Reading reading = readings[j];
						if (!ModelError.HasErrors(reading, ModelErrorUses.DisplayPrimary))
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
						replacements[k] = (rolePlayer != null) ? rolePlayer.Name.Replace('-', ' ') : ResourceStrings.ModelReadingEditorMissingRolePlayerText;
					}
					retVal = (formatText == null) ?
						string.Concat(replacements) :
						string.Format(CultureInfo.InvariantCulture, CultureInfo.InvariantCulture.TextInfo.ToTitleCase(formatText.Replace('-', ' ')), replacements);
					if (!string.IsNullOrEmpty(retVal))
					{
						retVal = retVal.Replace(" ", null);
					}
				}
			}
			return retVal;
		}
		private string myGeneratedName = String.Empty;
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
							myGeneratedName = null; // Set explicitly to null, see notes in GetNameValue
							Name = retVal;
						}
						else
						{
							myGeneratedName = retVal;
						}
					}
				}
				return retVal ?? String.Empty;
			}
		}
		/// <summary>
		/// Override to use our own name handling
		/// </summary>
		protected override void MergeConfigure(ElementGroup elementGroup)
		{
			// Do not forward to the base here. The base calls SetUniqueName,
			// but we don't enforce unique names on the generated FactType name.

			// If the Objectification is set during merge, then we need to
			// make sure the names are in sync.
			ObjectType nestingType = NestingType;
			if (nestingType != null)
			{
				string generatedName = GenerateName();
				string nestingTypeName = nestingType.Name;
				if (nestingTypeName.Length == 0 || generatedName == nestingTypeName)
				{
					myGeneratedName = generatedName;
				}
			}
		}
		#endregion // Automatic Name Generation
		#region Model Validation Rules
		/// <summary>
		/// Internal uniqueness constraints are required for non-unary facts. Requires
		/// validation when roles are added and removed.
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole))] // AddRule
		private sealed partial class FactTypeHasRoleAddRule : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				FactType factType = (e.ModelElement as FactTypeHasRole).FactType;
				ORMCoreDomainModel.DelayValidateElement(factType, DelayValidateFactTypeRequiresInternalUniquenessConstraintError);
				ORMCoreDomainModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
			}
		}
		/// <summary>
		/// Internal uniqueness constraints are required for non-unary facts. Requires
		/// validation when roles are added and removed.
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole))] // DeleteRule
		private sealed partial class FactTypeHasRoleDeleteRule : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				FactType factType = (e.ModelElement as FactTypeHasRole).FactType;
				if (!factType.IsDeleted)
				{
					ORMCoreDomainModel.DelayValidateElement(factType, DelayValidateFactTypeRequiresInternalUniquenessConstraintError);
					ORMCoreDomainModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
				}
			}
		}
		/// <summary>
		/// Validate the InternalUniquenessConstraintRequired and ImpliedInternalUniquenessConstraintError
		/// </summary>
		[RuleOn(typeof(FactSetConstraint))] // AddRule
		private sealed partial class ModelHasInternalConstraintAddRuleModelValidation : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				FactSetConstraint link = e.ModelElement as FactSetConstraint;
				if (link.SetConstraint.Constraint.ConstraintType == ConstraintType.InternalUniqueness)
				{
					FactType fact = link.FactType;
					ORMCoreDomainModel.DelayValidateElement(fact, DelayValidateFactTypeRequiresInternalUniquenessConstraintError);
					ORMCoreDomainModel.DelayValidateElement(fact, DelayValidateImpliedInternalUniquenessConstraintError);
				}
			}
		}
		/// <summary>
		/// Validate the InternalUniquenessConstraintRequired and ImpliedInternalUniquenessConstraintError
		/// </summary>
		[RuleOn(typeof(FactSetConstraint))] // DeleteRule
		private sealed partial class ModelHasInternalConstraintDeleteRuleModelValidation : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				FactSetConstraint link = e.ModelElement as FactSetConstraint;
				FactType fact = link.FactType;
				if (!fact.IsDeleted &&
					link.SetConstraint.Constraint.ConstraintType == ConstraintType.InternalUniqueness)
				{
					ORMCoreDomainModel.DelayValidateElement(fact, DelayValidateFactTypeRequiresInternalUniquenessConstraintError);
					ORMCoreDomainModel.DelayValidateElement(fact, DelayValidateImpliedInternalUniquenessConstraintError);
				}
			}
		}
		[RuleOn(typeof(UniquenessConstraint))] // ChangeRule
		private sealed partial class InternalUniquenessConstraintChangeRule : ChangeRule
		{
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				Guid attributeId = e.DomainProperty.Id;
				if (attributeId == UniquenessConstraint.ModalityDomainPropertyId)
				{
					UniquenessConstraint constraint = e.ModelElement as UniquenessConstraint;
					LinkedElementCollection<FactType> facts;
					if (!constraint.IsDeleted &&
						constraint.IsInternal &&
						1 == (facts = constraint.FactTypeCollection).Count)
					{
						FactType fact = facts[0];
						ORMCoreDomainModel.DelayValidateElement(fact, DelayValidateFactTypeRequiresInternalUniquenessConstraintError);
						ORMCoreDomainModel.DelayValidateElement(fact, DelayValidateImpliedInternalUniquenessConstraintError);
					}
				}
			}
		}
		[RuleOn(typeof(ConstraintRoleSequenceHasRole))] // AddRule
		private sealed partial class InternalConstraintCollectionHasConstraintAddedRule : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				UniquenessConstraint constraint = link.ConstraintRoleSequence as UniquenessConstraint;
				LinkedElementCollection<FactType> facts;
				if (constraint != null &&
					constraint.IsInternal &&
					1 == (facts = constraint.FactTypeCollection).Count)
				{
					ORMCoreDomainModel.DelayValidateElement(facts[0], DelayValidateImpliedInternalUniquenessConstraintError);
				}
			}
		}

		[RuleOn(typeof(ConstraintRoleSequenceHasRole))] // DeleteRule
		private sealed partial class InternalConstraintCollectionHasConstraintDeleteRule : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				UniquenessConstraint constraint = link.ConstraintRoleSequence as UniquenessConstraint;
				LinkedElementCollection<FactType> facts;
				FactType fact;
				if (constraint != null &&
					!constraint.IsDeleted &&
					constraint.IsInternal &&
					1 == (facts = constraint.FactTypeCollection).Count &&
					!(fact = facts[0]).IsDeleted)
				{
					ORMCoreDomainModel.DelayValidateElement(fact, DelayValidateImpliedInternalUniquenessConstraintError);
				}
			}
		}


		/// <summary>
		/// Calls the validation of all FactType related errors
		/// </summary>
		[RuleOn(typeof(ModelHasFactType))] // AddRule
		private sealed partial class ModelHasFactTypeAddRuleModelValidation : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelHasFactType link = e.ModelElement as ModelHasFactType;
				if (link != null)
				{
					FactType fact = link.FactType;
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
		[RuleOn(typeof(FactTypeHasReadingOrder))] // AddRule
		private sealed partial class FactTypeHasReadingOrderAddRuleModelValidation : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				FactType factType = (e.ModelElement as FactTypeHasReadingOrder).FactType;
				ORMCoreDomainModel.DelayValidateElement(factType, DelayValidateFactTypeRequiresReadingError);
				ORMCoreDomainModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
			}
		}
		/// <summary>
		/// Only validates ReadingRequiredError
		/// </summary>
		[RuleOn(typeof(FactTypeHasReadingOrder))] // DeleteRule
		private sealed partial class FactTypeHasReadingOrderDeleteRuleModelValidation : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				FactType factType = (e.ModelElement as FactTypeHasReadingOrder).FactType;
				if (!factType.IsDeleted)
				{
					ORMCoreDomainModel.DelayValidateElement(factType, DelayValidateFactTypeRequiresReadingError);
					ORMCoreDomainModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
				}
			}
		}

		/// <summary>
		/// Only validates ReadingRequiredError
		/// </summary>
		[RuleOn(typeof(ReadingOrderHasReading))] // AddRule
		private sealed partial class ReadingOrderHasReadingAddRuleModelValidation : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				FactType factType = (e.ModelElement as ReadingOrderHasReading).ReadingOrder.FactType;
				if (factType != null)
				{
					ORMCoreDomainModel.DelayValidateElement(factType, DelayValidateFactTypeRequiresReadingError);
					ORMCoreDomainModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
				}
			}
		}
		/// <summary>
		/// Only validates ReadingRequiredError
		/// </summary>
		[RuleOn(typeof(ReadingOrderHasReading))] // DeleteRule
		private sealed partial class ReadingOrderHasReadingDeleteRuleModelValidation : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				ReadingOrder ord = (e.ModelElement as ReadingOrderHasReading).ReadingOrder;
				FactType factType;
				if (!ord.IsDeleted &&
					null != (factType = ord.FactType) &&
					!factType.IsDeleted)
				{
					ORMCoreDomainModel.DelayValidateElement(factType, DelayValidateFactTypeRequiresReadingError);
					ORMCoreDomainModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
				}
			}
		}
		[RuleOn(typeof(Reading))] // ChangeRule
		private sealed partial class ValidateFactNameForReadingChange : ChangeRule
		{
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				if (e.DomainProperty.Id == Reading.TextDomainPropertyId)
				{
					Reading reading = e.ModelElement as Reading;
					ReadingOrder order;
					FactType factType;
					if (null != reading &&
						!reading.IsDeleted &&
						null != (order = reading.ReadingOrder) &&
						!order.IsDeleted &&
						null != (factType = order.FactType) &&
						!factType.IsDeleted)
					{
						ORMCoreDomainModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
					}
				}
			}
		}
		[RuleOn(typeof(FactTypeHasReadingOrder))] // RolePlayerPositionChangeRule
		private sealed partial class ValidateFactNameForReadingOrderReorder : RolePlayerPositionChangeRule
		{
			public override void RolePlayerPositionChanged(RolePlayerOrderChangedEventArgs e)
			{
				if (e.SourceDomainRole.Id == FactTypeHasReadingOrder.FactTypeDomainRoleId)
				{
					FactType factType = (FactType)e.SourceElement;
					if (!factType.IsDeleted)
					{
						ORMCoreDomainModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
					}
				}
			}
		}
		[RuleOn(typeof(ReadingOrderHasReading))] // RolePlayerPositionChangeRule
		private sealed partial class ValidateFactNameForReadingReorder : RolePlayerPositionChangeRule
		{
			public override void RolePlayerPositionChanged(RolePlayerOrderChangedEventArgs e)
			{
				if (e.SourceDomainRole.Id == ReadingOrderHasReading.ReadingOrderDomainRoleId)
				{
					ReadingOrder order = (ReadingOrder)e.SourceElement;
					FactType factType;
					if (!order.IsDeleted &&
						null != (factType = order.FactType) &&
						!factType.IsDeleted)
					{
						ORMCoreDomainModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
					}
				}
			}
		}
		[RuleOn(typeof(ObjectTypePlaysRole))] // AddRule
		private sealed partial class ValidateFactNameForRolePlayerAdded : AddRule
		{
			public static void Process(ObjectTypePlaysRole link, Role playedRole)
			{
				if (playedRole == null)
				{
					playedRole = link.PlayedRole;
				}
				FactType factType = playedRole.FactType;
				if (factType != null)
				{
					ORMCoreDomainModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
				}
				RoleProxy proxy;
				if (null != (proxy = playedRole.Proxy) &&
					null != (factType = proxy.FactType))
				{
					ORMCoreDomainModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
				}
			}
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				Process(e.ModelElement as ObjectTypePlaysRole, null);
			}
		}
		[RuleOn(typeof(ObjectTypePlaysRole))] // DeleteRule
		private sealed partial class ValidateFactNameForRolePlayerDelete : DeleteRule
		{
			public static void Process(ObjectTypePlaysRole link, Role playedRole)
			{
				if (playedRole == null)
				{
					playedRole = link.PlayedRole;
				}
				FactType factType;
				if (!playedRole.IsDeleted)
				{
					if (null != (factType = playedRole.FactType))
					{
						ORMCoreDomainModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
					}
					RoleProxy proxy;
					if (null != (proxy = playedRole.Proxy) &&
						null != (factType = proxy.FactType))
					{
						ORMCoreDomainModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
					}
				}
			}
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				Process(e.ModelElement as ObjectTypePlaysRole, null);
			}
		}
		[RuleOn(typeof(ObjectTypePlaysRole))] // RolePlayerChangeRule
		private sealed partial class ValidateFactNameForRolePlayerRolePlayerChange : RolePlayerChangeRule
		{
			public sealed override void RolePlayerChanged(RolePlayerChangedEventArgs e)
			{
				Guid changedRoleGuid = e.DomainRole.Id;
				Role oldRole = null;
				if (changedRoleGuid == ObjectTypePlaysRole.PlayedRoleDomainRoleId)
				{
					oldRole = (Role)e.OldRolePlayer;
				}
				ObjectTypePlaysRole link = e.ElementLink as ObjectTypePlaysRole;
				ValidateFactNameForRolePlayerDelete.Process(link, oldRole);
				ValidateFactNameForRolePlayerAdded.Process(link, null);
			}
		}
		[RuleOn(typeof(ObjectType))] // ChangeRule
		private sealed partial class ValidateFactNameForObjectTypeNameChange : ChangeRule
		{
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				Guid attributeId = e.DomainProperty.Id;
				if (attributeId == ObjectType.NameDomainPropertyId)
				{
					ObjectType objectType = e.ModelElement as ObjectType;
					if (!objectType.IsDeleted)
					{
						LinkedElementCollection<Role> playedRoles = objectType.PlayedRoleCollection;
						int playedRolesCount = playedRoles.Count;
						for (int i = 0; i < playedRolesCount; ++i)
						{
							Role role = playedRoles[i];
							FactType factType = role.FactType;
							if (factType != null)
							{
								ORMCoreDomainModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
							}
							RoleProxy proxy;
							if (null != (proxy = role.Proxy) &&
								null != (factType = proxy.FactType))
							{
								ORMCoreDomainModel.DelayValidateElement(factType, DelayValidateFactTypeNamePartChanged);
							}
						}
						FactType nestedFact = objectType.NestedFactType;
						if (nestedFact != null)
						{
							string newName = (string)e.NewValue;
							if (newName.Length != 0)
							{
								nestedFact.RegenerateErrorText();
								nestedFact.OnFactTypeNameChanged();
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// Rule helper function to regenerate error text
		/// </summary>
		private void RegenerateErrorText()
		{
			foreach (ModelError error in GetErrorCollection(ModelErrorUses.None))
			{
				if (0 != (error.RegenerateEvents & RegenerateErrorTextEvents.OwnerNameChange))
				{
					error.GenerateErrorText();
				}
			}
		}
		/// <summary>
		/// Update the fact type name when an objectification is added
		/// </summary>
		[RuleOn(typeof(Objectification), Priority = 1)] // AddRule
		private sealed partial class ValidateFactNameForObjectificationAdded : AddRule
		{
			public static void Process(Objectification link)
			{
				ObjectType nestingObjectType = link.NestingType;
				if (nestingObjectType.Name.Length != 0)
				{
					FactType nestedFactType = link.NestedFactType;
					nestedFactType.RegenerateErrorText();
					nestedFactType.OnFactTypeNameChanged();
				}
			}
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				Process(e.ModelElement as Objectification);
			}
		}
		/// <summary>
		/// Update the fact type name when an objectification is deleted
		/// </summary>
		[RuleOn(typeof(Objectification), Priority = 1)] // DeleteRule
		private sealed partial class ValidateFactNameForObjectificationDelete : DeleteRule
		{
			public static void Process(Objectification link, FactType nestedFactType, ObjectType nestingObjectType)
			{
				if (nestedFactType == null)
				{
					nestedFactType = link.NestedFactType;
				}
				if (nestingObjectType == null)
				{
					nestingObjectType = link.NestingType;
				}
				if (!nestedFactType.IsDeleted && (nestingObjectType.IsDeleted || nestingObjectType.Name.Length != 0))
				{
					nestedFactType.RegenerateErrorText();
					nestedFactType.OnFactTypeNameChanged();
				}
			}
			public override void ElementDeleted(ElementDeletedEventArgs e)
			{
				Process(e.ModelElement as Objectification, null, null);
			}
		}
		[RuleOn(typeof(Objectification), Priority = 1)] // RolePlayerChangeRule
		private sealed partial class ValidateFactNameForObjectificationRolePlayerChange : RolePlayerChangeRule
		{
			public sealed override void RolePlayerChanged(RolePlayerChangedEventArgs e)
			{
				Guid changedRoleGuid = e.DomainRole.Id;
				FactType oldFactType = null;
				ObjectType oldObjectType = null;
				if (changedRoleGuid == Objectification.NestingTypeDomainRoleId)
				{
					oldObjectType = (ObjectType)e.OldRolePlayer;
				}
				else if (changedRoleGuid == Objectification.NestedFactTypeDomainRoleId)
				{
					oldFactType = (FactType)e.OldRolePlayer;
				}
				Objectification link = e.ElementLink as Objectification;
				ValidateFactNameForObjectificationDelete.Process(link, oldFactType, oldObjectType);
				ValidateFactNameForObjectificationAdded.Process(link);
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
				LinkedElementCollection<RoleBase> factRoles = RoleCollection;
				UniquenessConstraint[] iuc = new UniquenessConstraint[iucCount];
				const uint deonticBit = 1U << 31;
				uint[] roleBits = new uint[iucCount];
				int index = 0;
				foreach (UniquenessConstraint ic in GetInternalConstraints<UniquenessConstraint>())
				{
					iuc[index] = ic;
					uint bits = 0;
					LinkedElementCollection<Role> constraintRoles = ic.RoleCollection;
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
									leftIUC.Delete();
									iuc[i] = null;
									left = 0;
									break;
								}
								else
								{
									rightIUC.Delete();
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
								rightIUC.Delete();
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
									rightIUC.Delete();
									iuc[j] = null;
								}
							}
							else if (compare == (right & ~deonticBit))
							{
								// right implies left unless right is deontic
								if (0 == (right & deonticBit))
								{
									leftIUC.Delete();
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
								rightIUC.Delete();
								iuc[j] = null;
							}
							else if (compare == right)
							{
								// right implies left
								leftIUC.Delete();
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
		#region ImpliedUniqueVerbalizer class
		/// <summary>
		/// Non-generated portions of verbalization helper used to verbalize a
		/// single-role internal uniqueness constraint on a proxy role.
		/// </summary>
		private partial class ImpliedUniqueVerbalizer
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
			private LinkedElementCollection<Role> RoleCollection
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
			private bool IsPreferred
			{
				get
				{
					return myConstraint.IsPreferred;
				}
			}
		}
		#endregion // ImpliedUniqueVerbalizer class
		#region ImpliedMandatoryVerbalizer class
		/// <summary>
		/// Non-generated portions of verbalization helper used to verbalize a
		/// simple mandatory constraint on a proxy role.
		/// </summary>
		private partial class ImpliedMandatoryVerbalizer
		{
			private FactType myFact;
			private MandatoryConstraint myConstraint;
			public void Initialize(FactType fact, MandatoryConstraint constraint)
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
			private LinkedElementCollection<Role> RoleCollection
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
		#endregion // ImpliedMandatoryVerbalizer class
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
			private LinkedElementCollection<Role> RoleCollection
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
		#region FactTypeInstanceVerbalizer class
		/// <summary>
		/// Non-generated portions of verbalization helper used to verbalize a
		/// combined internal uniqueness constraint and simple mandatory constraint.
		/// </summary>
		private partial class FactTypeInstanceVerbalizer
		{
			private FactType myFact;
			private FactTypeInstance myInstance;
			public void Initialize(FactType fact, FactTypeInstance instance)
			{
				myFact = fact;
				myInstance = instance;
			}
			private void DisposeHelper()
			{
				myFact = null;
				myInstance = null;
			}
			private FactType FactType
			{
				get
				{
					return myFact;
				}
			}
			private FactTypeInstance Instance
			{
				get
				{
					return myInstance;
				}
			}
		}
		#endregion // FactTypeInstanceVerbalizer class
		#region IVerbalizeCustomChildren Implementation
		/// <summary>
		/// Implements IVerbalizeCustomChildren.GetCustomChildVerbalizations. Responsible
		/// for internal constraints, combinations of internals, and defaults
		/// </summary>
		protected IEnumerable<CustomChildVerbalizer> GetCustomChildVerbalizations(bool isNegative)
		{
			LinkedElementCollection<SetConstraint> setConstraints = SetConstraintCollection;
			int setConstraintCount = setConstraints.Count;
			if (setConstraintCount != 0)
			{
				// All internal constraints (and combinations) are non-aggregated, so they
				// are verbalized as custom children.
				bool lookForDefault = !isNegative && Shell.OptionsPage.CurrentShowDefaultConstraintVerbalization;
				bool lookForCombined = !isNegative && Shell.OptionsPage.CurrentCombineMandatoryAndUniqueVerbalization;

				LinkedElementCollection<RoleBase> factRoles = RoleCollection;
				if (2 == factRoles.Count)
				{
					// UNDONE: Internal verbalization of proxy roles
					Role[] roles = new Role[2];
					RoleProxy proxy = null;
					int primaryRoleCount;
					if (ImpliedByObjectification == null)
					{
						roles[0] = (Role)factRoles[0];
						roles[1] = (Role)factRoles[1];
						primaryRoleCount = 2;
					}
					else
					{
						// Find the proxy, and put the opposite role in the 0 slot
						proxy = factRoles[0] as RoleProxy;
						if (proxy != null)
						{
							roles[0] = (Role)factRoles[1];
						}
						else
						{
							proxy = (RoleProxy)factRoles[1];
							roles[0] = (Role)factRoles[0];
						}
						primaryRoleCount = 1;
					}
					// Array of single role constraints.
					//    Index 1 == Left/Right
					//    Index 2 == Alethic/Deontic
					//    Index 3 == Unique/Mandatory
					SetConstraint[, ,] singleRoleConstraints = new SetConstraint[2, 2, 2];

					// A single-role uniqueness constraint with an implied default
					SetConstraint constraintWithImpliedOppositeDefault = null;

					// Don't run loop at end if we don't have anything to verbalize
					bool haveSingles = false;

					for (int i = 0; i < setConstraintCount; ++i)
					{
						SetConstraint constraint = setConstraints[i];
						IConstraint iConstraint = constraint.Constraint;
						if (iConstraint.ConstraintIsInternal)
						{
							LinkedElementCollection<Role> constraintRoles = constraint.RoleCollection;
							if (constraintRoles.Count == 1)
							{
								// Handle singles specially at the end so that
								// the verbalization order is less dependent on the
								// constraint order in the model
								Role constraintRole = constraintRoles[0];
								for (int iRole = 0; iRole < primaryRoleCount; ++iRole)
								{
									if (constraintRole == roles[iRole])
									{
										int modalityIndex = (constraint.Modality == ConstraintModality.Alethic) ? 0 : 1;
										int constraintIndex = (iConstraint.ConstraintType == ConstraintType.InternalUniqueness) ? 0 : 1;
										if (singleRoleConstraints[iRole, modalityIndex, constraintIndex] == null) // Skip duplicate
										{
											singleRoleConstraints[iRole, modalityIndex, constraintIndex] = constraint;
											haveSingles = true;
											if (lookForDefault && modalityIndex == 0 && constraintIndex == 0) // Alethic Uniqueness constraint
											{
												if (constraintWithImpliedOppositeDefault != null)
												{
													lookForDefault = false;
													constraintWithImpliedOppositeDefault = null;
												}
												else
												{
													constraintWithImpliedOppositeDefault = constraint;
												}
											}
										}
									}
								}
							}
							else
							{
								yield return new CustomChildVerbalizer((IVerbalize)constraint);
							}
						}
					}

					if (proxy != null)
					{
						// Pick up single role internal constraints from the proxy target role and
						// add them to the second column
						LinkedElementCollection<ConstraintRoleSequence> sequences = proxy.TargetRole.ConstraintRoleSequenceCollection;
						int sequenceCount = sequences.Count;
						for (int i = 0; i < sequenceCount; ++i)
						{
							SetConstraint constraint = sequences[i] as SetConstraint;
							IConstraint iConstraint;
							if (constraint != null &&
								(iConstraint = constraint.Constraint).ConstraintIsInternal &&
								constraint.RoleCollection.Count == 1)
							{
								int modalityIndex = (constraint.Modality == ConstraintModality.Alethic) ? 0 : 1;
								int constraintIndex = (iConstraint.ConstraintType == ConstraintType.InternalUniqueness) ? 0 : 1;
								if (singleRoleConstraints[1, modalityIndex, constraintIndex] == null) // Skip duplicate
								{
									singleRoleConstraints[1, modalityIndex, constraintIndex] = constraint;
									haveSingles = true;
									if (lookForDefault && modalityIndex == 0 && constraintIndex == 0) // Alethic Uniqueness constraint
									{
										if (constraintWithImpliedOppositeDefault != null)
										{
											lookForDefault = false;
											constraintWithImpliedOppositeDefault = null;
										}
										else
										{
											constraintWithImpliedOppositeDefault = constraint;
										}
									}
								}
							}
						}
					}

					if (haveSingles)
					{
						// Walk the single role constraints and try to combine them
						// Group by modality/constraintType/role position
						for (int modalityIndex = 0; modalityIndex < 2; ++modalityIndex)
						{
							for (int roleIndex = 0; roleIndex < 2; ++roleIndex)
							{
								UniquenessConstraint uniquenessConstraint = singleRoleConstraints[roleIndex, modalityIndex, 0] as UniquenessConstraint;
								MandatoryConstraint mandatoryConstraint = singleRoleConstraints[roleIndex, modalityIndex, 1] as MandatoryConstraint;
								if (lookForCombined)
								{
									if (uniquenessConstraint != null && mandatoryConstraint != null)
									{
										// Combine verbalizations into one
										CombinedMandatoryUniqueVerbalizer verbalizer = CombinedMandatoryUniqueVerbalizer.GetVerbalizer();
										verbalizer.Initialize(this, uniquenessConstraint);
										yield return new CustomChildVerbalizer(verbalizer, true);
									}
									else if (uniquenessConstraint != null)
									{
										if (roleIndex == 1 && proxy != null)
										{
											// Make sure the readings come from the implied fact
											ImpliedUniqueVerbalizer verbalizer = ImpliedUniqueVerbalizer.GetVerbalizer();
											verbalizer.Initialize(this, uniquenessConstraint);
											yield return new CustomChildVerbalizer(verbalizer, true);
										}
										else
										{
											yield return new CustomChildVerbalizer(uniquenessConstraint);
										}
									}
									else if (mandatoryConstraint != null)
									{
										if (roleIndex == 1 && proxy != null)
										{
											// Make sure the readings come from the implied fact
											ImpliedMandatoryVerbalizer verbalizer = ImpliedMandatoryVerbalizer.GetVerbalizer();
											verbalizer.Initialize(this, mandatoryConstraint);
											yield return new CustomChildVerbalizer(verbalizer, true);
										}
										else
										{
											yield return new CustomChildVerbalizer(mandatoryConstraint);
										}
									}
								}
								else
								{
									if (uniquenessConstraint != null)
									{
										if (roleIndex == 1 && proxy != null)
										{
											// Make sure the readings come from the implied fact
											ImpliedUniqueVerbalizer verbalizer = ImpliedUniqueVerbalizer.GetVerbalizer();
											verbalizer.Initialize(this, uniquenessConstraint);
											yield return new CustomChildVerbalizer(verbalizer, true);
										}
										else
										{
											yield return new CustomChildVerbalizer(uniquenessConstraint);
										}
									}
									if (mandatoryConstraint != null)
									{
										if (roleIndex == 1 && proxy != null)
										{
											// Make sure the readings come from the implied fact
											ImpliedMandatoryVerbalizer verbalizer = ImpliedMandatoryVerbalizer.GetVerbalizer();
											verbalizer.Initialize(this, mandatoryConstraint);
											yield return new CustomChildVerbalizer(verbalizer, true);
										}
										else
										{
											yield return new CustomChildVerbalizer(mandatoryConstraint);
										}
									}
								}
							}
						}
					}

					if (constraintWithImpliedOppositeDefault != null)
					{
						DefaultBinaryMissingUniquenessVerbalizer verbalizer = DefaultBinaryMissingUniquenessVerbalizer.GetVerbalizer();
						verbalizer.Initialize(this, (UniquenessConstraint)constraintWithImpliedOppositeDefault);
						yield return new CustomChildVerbalizer(verbalizer, true);
					}
				}
				else
				{
					// Easy case, just verbalize all internal constraints as entered
					for (int i = 0; i < setConstraintCount; ++i)
					{
						SetConstraint constraint = setConstraints[i];
						if (constraint.Constraint.ConstraintIsInternal)
						{
							yield return new CustomChildVerbalizer((IVerbalize)constraint);
						}
					}
				}
			}
			if (ReadingRequiredError == null)
			{
				LinkedElementCollection<FactTypeInstance> instances = FactTypeInstanceCollection;
				int instanceCount = instances.Count;
				if (instanceCount != 0)
				{
					yield return new CustomChildVerbalizer(FactTypeInstanceBlockStart.GetVerbalizer(), true);
					for (int i = 0; i < instanceCount; ++i)
					{
						FactTypeInstanceVerbalizer verbalizer = FactTypeInstanceVerbalizer.GetVerbalizer();
						verbalizer.Initialize(this, instances[i]);
						yield return new CustomChildVerbalizer(verbalizer, true);
					}
					yield return new CustomChildVerbalizer(FactTypeInstanceBlockEnd.GetVerbalizer(), true);
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
			private LinkedElementCollection<Role> RoleCollection
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
		#region IHierarchyContextEnabled Members
		/// <summary>
		/// Gets the contextable object that this instance should resolve to.
		/// </summary>
		/// <value>The forward context. Null if none</value>
		/// <remarks>For example a role should resolve to a fact type since a role is displayed with a fact type</remarks>
		protected static IHierarchyContextEnabled ForwardHierarchyContextTo
		{
			get
			{
				return null;
			}
		}
		/// <summary>
		/// Gets the elements that the current instance is dependant on for display.
		/// The returned elements will be forced to display in the context window.
		/// </summary>
		/// <value>The dependant context elements.</value>
		protected IEnumerable<IHierarchyContextEnabled> ForcedHierarchyContextElementCollection
		{
			get
			{
				LinkedElementCollection<RoleBase> collection = RoleCollection;
				int collectionCount = collection.Count;
				for (int i = 0; i < collectionCount; ++i)
				{
					IHierarchyContextEnabled rolePlayer = collection[i].Role.RolePlayer as IHierarchyContextEnabled;
					if (rolePlayer != null)
					{
						yield return rolePlayer;
					}
				}
			}
		}
		/// <summary>
		/// Gets the place priority. The place priority specifies the order in which the element will
		/// be placed on the context diagram.
		/// </summary>
		/// <value>The place priority.</value>
		protected HierarchyContextPlacementPriority HierarchyContextPlacementPriority
		{
			get
			{
				if (this.Objectification == null)
				{
					return HierarchyContextPlacementPriority.Medium;
				}
				else
				{
					return HierarchyContextPlacementPriority.High;
				}
			}
		}
		/// <summary>
		/// Gets the number of generations to decriment when this object is walked.
		/// </summary>
		/// <value>The number of generations.</value>
		protected int HierarchyContextDecrementCount
		{
			get
			{
				if (NestingType != null)
				{
					return 1;
				}
				return 0;
			}
		}
		/// <summary>
		/// Gets a value indicating whether the path through the diagram should be followed through
		/// this element.
		/// </summary>
		/// <value><c>true</c> to continue walking; otherwise, <c>false</c>.</value>
		protected static bool ContinueWalkingHierarchyContext
		{
			get { return true; }
		}
		#region IHierarchyContextEnabled Members
		int IHierarchyContextEnabled.HierarchyContextDecrementCount
		{
			get { return HierarchyContextDecrementCount; }
		}
		bool IHierarchyContextEnabled.ContinueWalkingHierarchyContext
		{
			get { return ContinueWalkingHierarchyContext; }
		}
		IHierarchyContextEnabled IHierarchyContextEnabled.ForwardHierarchyContextTo
		{
			get { return ForwardHierarchyContextTo; }
		}
		IEnumerable<IHierarchyContextEnabled> IHierarchyContextEnabled.ForcedHierarchyContextElementCollection
		{
			get { return ForcedHierarchyContextElementCollection; }
		}
		HierarchyContextPlacementPriority IHierarchyContextEnabled.HierarchyContextPlacementPriority
		{
			get { return HierarchyContextPlacementPriority; }
		}
		#endregion
		#endregion
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
			if (ErrorText != newText)
			{
				ErrorText = newText;
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
			if (ErrorText != newText)
			{
				ErrorText = newText;
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
			if (ErrorText != newText)
			{
				ErrorText = newText;
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
		[RuleOn(typeof(FactTypeDerivationExpression))] // ChangeRule
		private sealed partial class FactTypeDerivationExpressionChangeRule : ChangeRule
		{
			/// <summary>
			/// check the Body property of the FactTypeDerivationExpression and delete the FactTypeDerivationExpression 
			/// if Body is empty
			/// </summary>
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				Guid attributeGuid = e.DomainProperty.Id;
				if (attributeGuid == FactTypeDerivationExpression.BodyDomainPropertyId)
				{
					FactTypeDerivationExpression ftde = e.ModelElement as FactTypeDerivationExpression;
					if (!ftde.IsDeleted && string.IsNullOrEmpty(ftde.Body))
					{
						ftde.Delete();
					}
				}
			}
		}
	}
	#endregion
}
