using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
namespace Northface.Tools.ORM.ObjectModel
{
	#region Constraint class
	public partial class Constraint : INamedElementDictionaryChild
	{
		#region Constraint specific
		/// <summary>
		/// The minimum number of required role sets
		/// </summary>
		public int RoleSetCountMinimum
		{
			get
			{
				int retVal = 1;
				switch (RoleSetStyles & RoleSetStyles.SetMultiplicityMask)
				{
					case RoleSetStyles.MultipleRowSets:
					case RoleSetStyles.TwoRoleSets:
						retVal = 2;
						break;
#if DEBUG
					case RoleSetStyles.OneRoleSet:
						break;
					default:
						Debug.Assert(false); // Shouldn't be here
						break;
#endif // DEBUG
				}
				return retVal;
			}
		}
		/// <summary>
		/// The maximum number of required role sets, or -1 if no max
		/// </summary>
		public int RoleSetCountMaximum
		{
			get
			{
				int retVal = 1;
				switch (RoleSetStyles & RoleSetStyles.SetMultiplicityMask)
				{
					case RoleSetStyles.MultipleRowSets:
						retVal = -1;
						break;
					case RoleSetStyles.TwoRoleSets:
						retVal = 2;
						break;
#if DEBUG
					case RoleSetStyles.OneRoleSet:
						break;
					default:
						Debug.Assert(false); // Shouldn't be here
						break;
#endif // DEBUG
				}
				return retVal;
			}
		}
		#endregion // Constraint specific
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
		protected void GetRoleGuids(out Guid parentMetaRoleGuid, out Guid childMetaRoleGuid)
		{
			parentMetaRoleGuid = ModelHasConstraint.ModelMetaRoleGuid;
			childMetaRoleGuid = ModelHasConstraint.ConstraintCollectionMetaRoleGuid;
		}
		#endregion // INamedElementDictionaryChild implementation
		#region ConstraintRoleSetHasRoleRemoved class
		/// <summary>
		/// Rule that fires when a constraint has a RoleSet Removed
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSetHasRole))]
		private class ConstraintRoleSetHasRoleRemoved : RemoveRule
		{
			/// <summary>
			/// Runs when roleset element is removed.  If there are no more roles in the role collection
			/// then the entire roleset is removed
			/// </summary>
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ConstraintRoleSetHasRole link = e.ModelElement as ConstraintRoleSetHasRole;
				ConstraintRoleSet roleSet = link.ConstraintRoleSetCollection;
				if (!roleSet.IsRemoved)
				{
					if (roleSet.RoleCollection.Count == 0)
					{
						roleSet.Remove();
					}
				}
			}
		}
			#endregion //ConstraintRoleSetHasRoleRemoved class
		#region InternalConstraintHasRoleSetRemoved class
		/// <summary>
		/// Rule that fires when an internal constraint has a RoleSet Removed
		/// </summary>
		[RuleOn(typeof(InternalConstraintHasRoleSet))]
		private class InternalConstraintHasRoleSetRemoved : RemoveRule
		{
			/// <summary>
			/// Runs when roleset element is removed.  If there are no more roles in the role collection
			/// then the entire roleset is removed
			/// </summary>
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				InternalConstraintHasRoleSet link = e.ModelElement as InternalConstraintHasRoleSet;
				InternalConstraint constraint = link.InternalConstraint;
				if (!constraint.IsRemoved)
				{
					constraint.Remove();
				}
			}
		}
		#endregion //InternalConstraintHasRoleSetRemoved class
		#region ExternalConstraintHasRoleSetRemoved class
		/// <summary>
		/// Rule that fires when an external constraint has a RoleSet Removed
		/// </summary>
		[RuleOn(typeof(ExternalConstraintHasRoleSet))]
		private class ExternalConstraintHasRoleSetRemoved : RemoveRule
		{
			/// <summary>
			/// Runs then roleset element is removed.  If there are no more roles in the role collection
			/// then the entire roleset is removed
			/// </summary>
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ExternalConstraintHasRoleSet link = e.ModelElement as ExternalConstraintHasRoleSet;
				ExternalConstraint constraint = link.ExternalConstraint;
				if (!constraint.IsRemoved)
				{
					if (constraint.RoleSetCollection.Count == 0)
					{
						constraint.Remove();
					}
				}
			}
		}
			#endregion //ExternalConstraintHasRoleSetRemoved class
	}
	#endregion // Constraint class
	#region InternalConstraint class
	public partial class InternalConstraint
	{
		#region InternalConstraint Specific
		/// <summary>
		/// Ensure that the link directly from the
		/// constraint to the fact type exists. This
		/// method should be called from inside a transaction
		/// and will throw
		/// </summary>
		/// <param name="role"></param>
		private void EnsureFactConstraintForRole(Role role)
		{
			FactType existingFactType = FactType;
			FactType candidateFactType = role.FactType;
			if (candidateFactType != null)
			{
				if (existingFactType == null)
				{
					FactType = candidateFactType;
				}
				else if (!object.ReferenceEquals(existingFactType, candidateFactType))
				{
					throw new InvalidOperationException(ResourceStrings.ModelExceptionInternalConstraintInconsistentRoleOwners);
				}
			}
		}
		#endregion // InternalConstraint Specific
		#region InternalFactConstraint synchronization rules
		/// <summary>
		/// If a role is added after the role set is already attached,
		/// then create the corresponding ExternalFactConstraint and ExternalRoleConstraint
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSetHasRole))]
		private class ConstraintRoleSetHasRoleAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ConstraintRoleSetHasRole link = e.ModelElement as ConstraintRoleSetHasRole;
				InternalConstraint constraint = link.ConstraintRoleSetCollection.Constraint as InternalConstraint;
				if (constraint != null)
				{
					// Note that this will throw if the role owner
					// is incorrect
					constraint.EnsureFactConstraintForRole(link.RoleCollection);
				}
			}
		}
		/// <summary>
		/// If a role set is added that already contains roles, then
		/// make sure the corresponding InternalFactConstraint
		/// object is created for each role.
		/// </summary>
		[RuleOn(typeof(InternalConstraintHasRoleSet))]
		private class ConstraintHasRoleSetAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				EnsureFactConstraintForRoleSet(e.ModelElement as InternalConstraintHasRoleSet);
			}
		}
		/// <summary>
		/// Helper function to support the same fact constraint fixup
		/// during both deserialization and rules.
		/// </summary>
		/// <param name="link">A roleset link added to the constraint</param>
		private static void EnsureFactConstraintForRoleSet(InternalConstraintHasRoleSet link)
		{
			InternalConstraintRoleSet roleSet = link.RoleSet;
			RoleMoveableCollection roles = roleSet.RoleCollection;
			int roleCount = roles.Count;
			if (roleCount != 0)
			{
				InternalConstraint constraint = link.InternalConstraint;
				for (int i = 0; i < roleCount; ++i)
				{
					Role role = roles[i];
					// Call for each role, not just the first. This
					// enforces that all roles are in the same fact type.
					constraint.EnsureFactConstraintForRole(role);
				}
			}
		}
		#endregion // InternalFactConstraint synchronization rules
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// adds the implicit InternalFactConstraint elements.
		/// </summary>
		[CLSCompliant(false)]
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new InternalConstraintFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation. Adds implicit InternalFactConstraint relationships
		/// </summary>
		private class InternalConstraintFixupListener : DeserializationFixupListener<InternalConstraint>
		{
			/// <summary>
			/// InternalFactConstraintFixupListener constructor
			/// </summary>
			public InternalConstraintFixupListener() : base((int)ORMDeserializationFixupPhase.AddImplicitElements)
			{
			}
			/// <summary>
			/// Process elements by added an InternalFactConstraint for
			/// each roleset
			/// </summary>
			/// <param name="element">An InternalConstraint element</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected override void ProcessElement(InternalConstraint element, Store store, INotifyElementAdded notifyAdded)
			{
				IList links = element.GetElementLinks(InternalConstraintHasRoleSet.InternalConstraintMetaRoleGuid);
				int linksCount = links.Count;
				for (int i = 0; i < linksCount; ++i)
				{
					EnsureFactConstraintForRoleSet(links[i] as InternalConstraintHasRoleSet);
					IList factLinks = element.GetElementLinks(InternalFactConstraint.InternalConstraintCollectionMetaRoleGuid);
					int factLinksCount = factLinks.Count;
					for (int j = 0; j < factLinksCount; ++j)
					{
						// Notify that the link was added. Note that we don't set
						// addLinks to true here because there should only be one element
						notifyAdded.ElementAdded(factLinks[j] as ModelElement);
					}
				}
			}
		}
		#endregion // Deserialization Fixup
	}
	#endregion // InternalConstraint class
	#region ExternalConstraint class
	public partial class ExternalConstraint : IModelErrorOwner
	{
		#region ExternalConstraint Specific
		/// <summary>
		/// Get a read-only list of FactConstraint links. To get the
		/// fact type from here, use the FactTypeCollection property on the returned
		/// object. To get to the roles, use the ConstrainedRoleCollection property.
		/// </summary>
		[CLSCompliant(false)]
		public IList<ExternalFactConstraint> ExternalFactConstraintCollection
		{
			get
			{
				IList untypedList = GetElementLinks(ExternalFactConstraint.ExternalConstraintCollectionMetaRoleGuid);
				int elementCount = untypedList.Count;
				ExternalFactConstraint[] typedList = new ExternalFactConstraint[elementCount];
				untypedList.CopyTo(typedList, 0);
				return typedList;
			}
		}
		/// <summary>
		/// Ensure that an ExternalFactConstraint exists between the
		/// fact type owning the passed in role and this constraint.
		/// ExternalFactConstraint links are generated automatically
		/// and should never be directly created.
		/// </summary>
		/// <param name="role">The role to attach</param>
		/// <returns>The associated ExternalFactConstraint relationship</returns>
		private ExternalFactConstraint EnsureFactConstraintForRole(Role role)
		{
			ExternalFactConstraint retVal = null;
			FactType fact = role.FactType;
			if (fact != null)
			{
				while (retVal == null) // Will run at most twice
				{
					IList existingFactConstraints = fact.GetElementLinks(ExternalFactConstraint.FactTypeCollectionMetaRoleGuid);
					int listCount = existingFactConstraints.Count;
					for (int i = 0; i < listCount; ++i)
					{
						ExternalFactConstraint testFactConstraint = (ExternalFactConstraint)existingFactConstraints[i];
						if (testFactConstraint.ExternalConstraintCollection == this)
						{
							retVal = testFactConstraint;
							break;
						}
					}
					if (retVal == null)
					{
						fact.ExternalConstraintCollection.Add(this);
					}
				}
			}
			return retVal;
		}
		#endregion // ExternalConstraint Specific
		#region ExternalFactConstraint synchronization rules
		/// <summary>
		/// If a role is added after the role set is already attached,
		/// then create the corresponding ExternalFactConstraint and ExternalRoleConstraint
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSetHasRole))]
		private class ConstraintRoleSetHasRoleAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ConstraintRoleSetHasRole link = e.ModelElement as ConstraintRoleSetHasRole;
				ExternalConstraint constraint = link.ConstraintRoleSetCollection.Constraint as ExternalConstraint;
				if (constraint != null)
				{
					ExternalFactConstraint factConstraint = constraint.EnsureFactConstraintForRole(link.RoleCollection);
					if (factConstraint != null)
					{
						factConstraint.ConstrainedRoleCollection.Add(link);
					}
				}
			}
		}
		/// <summary>
		/// If a role set is added that already contains roles, then
		/// make sure the corresponding ExternalFactConstraint and ExternalRoleConstraint
		/// objects are created for each role.
		/// </summary>
		[RuleOn(typeof(ExternalConstraintHasRoleSet))]
		private class ConstraintHasRoleSetAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				EnsureFactConstraintForRoleSet(e.ModelElement as ExternalConstraintHasRoleSet);
			}
		}
		/// <summary>
		/// Helper function to support the same fact constraint fixup
		/// during both deserialization and rules.
		/// </summary>
		/// <param name="link">A roleset link added to the constraint</param>
		private static void EnsureFactConstraintForRoleSet(ExternalConstraintHasRoleSet link)
		{
			ExternalConstraintRoleSet roleSet = link.RoleSetCollection;
			// The following line gets the links instead of the counterparts,
			// which are provided by roleSet.RoleCollection
			IList roleLinks = roleSet.GetElementLinks(ConstraintRoleSetHasRole.ConstraintRoleSetCollectionMetaRoleGuid);
			int roleCount = roleLinks.Count;
			if (roleCount != 0)
			{
				ExternalConstraint constraint = link.ExternalConstraint;
				for (int i = 0; i < roleCount; ++i)
				{
					ConstraintRoleSetHasRole roleLink = (ConstraintRoleSetHasRole)roleLinks[i];
					ExternalFactConstraint factConstraint = constraint.EnsureFactConstraintForRole(roleLink.RoleCollection);
					if (factConstraint != null)
					{
						factConstraint.ConstrainedRoleCollection.Add(roleLink);
					}
				}
			}
		}
		[RuleOn(typeof(ExternalRoleConstraint))]
		private class ExternalRoleConstraintRemoved : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ExternalRoleConstraint link = e.ModelElement as ExternalRoleConstraint;
				ExternalFactConstraint factConstraint = link.FactConstraintCollection;
				if (!factConstraint.IsRemoved)
				{
					if (factConstraint.ConstrainedRoleCollection.Count == 0)
					{
						factConstraint.Remove();
					}
				}
			}
		}
		#endregion // ExternalFactConstraint synchronization rules
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// adds the implicit InternalFactConstraint elements.
		/// </summary>
		[CLSCompliant(false)]
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new ExternalConstraintFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation. Adds implicit ExternalFactConstraint relationships
		/// </summary>
		private class ExternalConstraintFixupListener : DeserializationFixupListener<ExternalConstraint>
		{
			/// <summary>
			/// InternalFactConstraintFixupListener constructor
			/// </summary>
			public ExternalConstraintFixupListener() : base((int)ORMDeserializationFixupPhase.AddImplicitElements)
			{
			}
			/// <summary>
			/// Process elements by added an InternalFactConstraint for
			/// each roleset
			/// </summary>
			/// <param name="element">An ExternalConstraint element</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected override void ProcessElement(ExternalConstraint element, Store store, INotifyElementAdded notifyAdded)
			{
				IList links = element.GetElementLinks(ExternalConstraintHasRoleSet.ExternalConstraintMetaRoleGuid);
				int linksCount = links.Count;
				for (int i = 0; i < linksCount; ++i)
				{
					EnsureFactConstraintForRoleSet(links[i] as ExternalConstraintHasRoleSet);
					IList factLinks = element.GetElementLinks(ExternalFactConstraint.ExternalConstraintCollectionMetaRoleGuid);
					int factLinksCount = factLinks.Count;
					for (int j = 0; j < factLinksCount; ++j)
					{
						// Notify that the link was added. Note that we set
						// addLinks to true here because we expect ExternalRoleConstraint
						// links to be attached to each ExternalFactConstraint
						notifyAdded.ElementAdded(factLinks[j] as ModelElement, true);
					}
				}
			}
		}
		#endregion // Deserialization Fixup
		#region Error synchronization rules
		/// <summary>
		/// Add, remove, and otherwise validate the current set of
		/// errors for this constraint.
		/// </summary>
		/// <param name="notifyAdded">If not null, this is being called during
		/// load when rules are not in place. Any elements that are added
		/// must be notified back to the caller.</param>
		private void VerifyRoleSetCountForRule(INotifyElementAdded notifyAdded)
		{
			if (!IsRemoved)
			{
				int minCount = RoleSetCountMinimum;
				int maxCount;
				int currentCount = RoleSetCollection.Count;
				Store store = Store;
				TooFewRoleSetsError insufficientError;
				TooManyRoleSetsError extraError;
				bool removeTooFew = false;
				bool removeTooMany = false;
				if (currentCount < minCount)
				{
					if (null == TooFewRoleSetsError)
					{
						insufficientError = TooFewRoleSetsError.CreateTooFewRoleSetsError(store);
						insufficientError.Model = Model;
						insufficientError.Constraint = this;
						insufficientError.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(insufficientError, true);
						}
					}
					removeTooMany = true;
				}
				else
				{
					removeTooFew = true;
					if ((-1 != (maxCount = RoleSetCountMaximum)) && (currentCount > maxCount))
					{
						extraError = TooManyRoleSetsError.CreateTooManyRoleSetsError(store);
						extraError.Model = Model;
						extraError.Constraint = this;
						extraError.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(extraError, true);
						}
					}
					else
					{
						removeTooMany = true;
					}
				}
				if (removeTooFew && null != (insufficientError = TooFewRoleSetsError))
				{
					insufficientError.Remove();
				}
				if (removeTooMany && null != (extraError = TooManyRoleSetsError))
				{
					extraError.Remove();
				}
			}
		}
		[RuleOn(typeof(ExternalConstraintHasRoleSet), FireTime = TimeToFire.TopLevelCommit)]
		private class EnforceRoleSetCardinalityForAdd : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ExternalConstraintHasRoleSet link = e.ModelElement as ExternalConstraintHasRoleSet;
				link.ExternalConstraint.VerifyRoleSetCountForRule(null);
			}
		}
		[RuleOn(typeof(ModelHasConstraint), FireTime = TimeToFire.TopLevelCommit)]
		private class EnforceRoleSetCardinalityForConstraintAdd : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelHasConstraint link = e.ModelElement as ModelHasConstraint;
				ExternalConstraint externalConstraint = link.ConstraintCollection as ExternalConstraint;
				if (externalConstraint != null)
				{
					externalConstraint.VerifyRoleSetCountForRule(null);
				}
			}
		}
		[RuleOn(typeof(ExternalConstraintHasRoleSet), FireTime = TimeToFire.TopLevelCommit)]
		private class EnforceRoleSetCardinalityForRemove : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ExternalConstraintHasRoleSet link = e.ModelElement as ExternalConstraintHasRoleSet;
				link.ExternalConstraint.VerifyRoleSetCountForRule(null);
			}
		}
		#endregion // Error synchronization rules
		#region IModelErrorOwner Implementation
		IEnumerable<ModelError> IModelErrorOwner.ErrorCollection
		{
			get
			{
				return ErrorCollection;
			}
		}
		/// <summary>
		/// Implements IModelErrorOwner.ErrorCollection
		/// </summary>
		[CLSCompliant(false)]
		protected IEnumerable<ModelError> ErrorCollection
		{
			get
			{
				TooManyRoleSetsError tooMany;
				TooFewRoleSetsError tooFew;
				if (null != (tooMany = TooManyRoleSetsError))
				{
					yield return tooMany;
				}
				if (null != (tooFew = TooFewRoleSetsError))
				{
					yield return tooFew;
				}
			}
		}
		/// <summary>
		/// Implements IModelErrorOwner.ValidateErrors
		/// Validate all errors on the external constraint. This
		/// is called during deserialization fixup when rules are
		/// suspended.
		/// </summary>
		/// <param name="notifyAdded">A callback for notifying
		/// the caller of all objects that are added.</param>
		protected void ValidateErrors(INotifyElementAdded notifyAdded)
		{
			VerifyRoleSetCountForRule(notifyAdded);
		}
		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateErrors(notifyAdded);
		}
		#endregion // IModelErrorOwner Implementation
	}
	#endregion // ExternalConstraint class
	#region FactConstraint classes
	public partial class InternalFactConstraint : IFactConstraint
	{
		#region IFactConstraint Implementation
		Constraint IFactConstraint.Constraint
		{
			get
			{
				return Constraint;
			}
		}
		/// <summary>
		/// Implements IFactConstraint.Constraint
		/// </summary>
		protected Constraint Constraint
		{
			get
			{
				return InternalConstraintCollection;
			}
		}
		IList<Role> IFactConstraint.RoleCollection
		{
			get
			{
				return RoleCollection;
			}
		}
		/// <summary>
		/// Implements IFactConstraint.RoleCollection
		/// </summary>
		[CLSCompliant(false)]
		protected IList<Role> RoleCollection
		{
			get
			{
				RoleMoveableCollection roles = InternalConstraintCollection.RoleSet.RoleCollection;
				int roleCount = roles.Count;
				Role[] typedList = new Role[roleCount];
				Debug.Assert(roleCount > 0); // This object should not exist otherwise
				roles.CopyTo(typedList, 0);
				return typedList;
			}
		}
		#endregion // IFactConstraint Implementation
	}
	public partial class ExternalFactConstraint : IFactConstraint
	{
		#region IFactConstraint Implementation
		Constraint IFactConstraint.Constraint
		{
			get
			{
				return Constraint;
			}
		}
		/// <summary>
		/// Implements IFactConstraint.Constraint
		/// </summary>
		protected Constraint Constraint
		{
			get
			{
				return ExternalConstraintCollection;
			}
		}
		IList<Role> IFactConstraint.RoleCollection
		{
			get
			{
				return RoleCollection;
			}
		}
		/// <summary>
		/// Implements IFactConstraint.RoleCollection
		/// </summary>
		[CLSCompliant(false)]
		protected IList<Role> RoleCollection
		{
			get
			{
				ConstraintRoleSetHasRoleMoveableCollection roleSetLinks = ConstrainedRoleCollection;
				int roleSetLinksCount = roleSetLinks.Count;
				Role[] typedList = new Role[roleSetLinksCount];
				for (int i = 0; i < roleSetLinksCount; ++i)
				{
					typedList[i] = roleSetLinks[i].RoleCollection;
				}
				return typedList;
			}
		}
		#endregion // IFactConstraint Implementation
	}
	#endregion // FactConstraint classes
	#region ConstraintRoleSet classes
	public partial class ConstraintRoleSet
	{
		#region ConstraintRoleSet Specific
		/// <summary>
		/// Get the constraint that owns this role set
		/// </summary>
		public abstract Constraint Constraint { get;}
		#endregion // ConstraintRoleSet Specific
	}
	public partial class InternalConstraintRoleSet
	{
		#region ConstraintRoleSet overrides
		/// <summary>
		/// Get the internal constraint that owns this role set
		/// </summary>
		public override Constraint Constraint
		{
			get
			{
				return InternalConstraint;
			}
		}
		#endregion // ConstraintRoleSet overrides
	}
	public partial class ExternalConstraintRoleSet
	{
		#region ConstraintRoleSet overrides
		/// <summary>
		/// Get the external constraint that owns this role set
		/// </summary>
		public override Constraint Constraint
		{
			get
			{
				return ExternalConstraint;
			}
		}
		#endregion // ConstraintRoleSet overrides
	}
	#endregion // ConstraintRoleSet classes
	#region InternalUniquenessConstraint class
	public partial class InternalUniquenessConstraint
	{
		#region CustomStorage handlers
		/// <summary>
		/// Standard override. Retrieve values for calculated properties.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <returns></returns>
		public override object GetValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			Guid attributeId = attribute.Id;
			if (attributeId == IsPreferredMetaAttributeGuid)
			{
				return PreferredIdentifierFor != null;
			}
			return base.GetValueForCustomStoredAttribute(attribute);
		}
		/// <summary>
		/// Standard override. All custom storage properties are derived, not
		/// stored. Actual changes are handled in InternalUniquenessConstraintChangeRule.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <param name="newValue">object</param>
		public override void SetValueForCustomStoredAttribute(MetaAttributeInfo attribute, object newValue)
		{
			Guid attributeGuid = attribute.Id;
			if (attributeGuid == IsPreferredMetaAttributeGuid)
			{
				// Handled by InternalUniquenessConstraintChangeRule
				return;
			}
			base.SetValueForCustomStoredAttribute(attribute, newValue);
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
		#region Customize property display
		/// <summary>
		/// Ensure that the Preferred property is readonly
		/// when the InternalUniquenessConstraintChangeRule is
		/// unable to make it true.
		/// </summary>
		/// <param name="propertyDescriptor"></param>
		/// <returns></returns>
		public override bool IsPropertyDescriptorReadOnly(PropertyDescriptor propertyDescriptor)
		{
			ElementPropertyDescriptor descriptor = propertyDescriptor as ElementPropertyDescriptor;
			if (descriptor != null && descriptor.MetaAttributeInfo.Id == IsPreferredMetaAttributeGuid)
			{
				return !TestAllowPreferred(null, false);
			}
			return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
		}
		/// <summary>
		/// Test to see if this constraint can be turned
		/// into a preferred uniqueness constraint
		/// </summary>
		/// <param name="forType">If set, verify that the constraint
		/// can be the preferred identifier for this type. Can be null.</param>
		/// <param name="throwIfFalse">If true, thrown instead of returning false</param>
		/// <returns>true if the test succeeds</returns>
		public bool TestAllowPreferred(ObjectType forType, bool throwIfFalse)
		{
			if (forType != null || !IsPreferred)
			{
				// To be considered for the preferred reference
				// mode on an object, the following must hold:
				// 1) The constraint must have one role
				// 2) The fact it is on must be binary
				// 3) The opposite role player must be an entity type
				// 4) A full-predicate constraint cannot be specified (this
				//    will also indicate a model error, but should still be checked)
				// The other conditions (the opposite role is mandatory and also
				// has a single-role uniqueness constraint) will be enforced in
				// the rule that makes the change.
				// Note that there is no requirement on the type of the object attached
				// to the preferred constraint role. If the primary object is created for
				// a RefMode object type, then a ValueType is required, but this is
				// not a requirement for all role players on preferred identifier constraints.
				RoleMoveableCollection constraintRoles = RoleSet.RoleCollection;
				if (constraintRoles.Count == 1) // Condition 1
				{
					Role role = constraintRoles[0];
					RoleMoveableCollection factRoles = role.FactType.RoleCollection;
					if (factRoles.Count == 2) // Condition 2
					{
						Role oppositeRole = factRoles[0];
						if (object.ReferenceEquals(oppositeRole, role))
						{
							oppositeRole = factRoles[1];
						}
						ObjectType rolePlayer = oppositeRole.RolePlayer;
						if ((forType == null || object.ReferenceEquals(forType, rolePlayer)) &&
							!rolePlayer.IsValueType) // Condition 3
						{
							// UNDONE: Check condition 4. This
							// will be much easier to do when a FactConstraint
							// is generated for internal fact types
							return true;
						}
					}
				}
			}
			if (throwIfFalse)
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionInvalidInternalPreferredIdentifierPreConditions);
			}
			return false;
		}
		#endregion // Customize property display
		#region InternalUniquenessConstraintChangeRule class
		[RuleOn(typeof(InternalUniquenessConstraint))]
		private class InternalUniquenessConstraintChangeRule : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeId = e.MetaAttribute.Id;
				if (attributeId == InternalUniquenessConstraint.IsPreferredMetaAttributeGuid)
				{
					InternalUniquenessConstraint constraint = e.ModelElement as InternalUniquenessConstraint;
					if ((bool)e.NewValue)
					{
						// The preconditions for all of this are verified in the UI, and
						// are verified again in the PreferredIdentifierAddRule. If any
						// of this throws it is because the preconditions are violated,
						// but this will be such a rare condition that I don't go
						// out of my way to validate it. Calling code can always use
						// the TestAllowPreferred method to get a cleaner exception.
						Role role = constraint.RoleSet.RoleCollection[0];
						Role oppositeRole = null;
						foreach (Role factRole in role.FactType.RoleCollection)
						{
							if (!object.ReferenceEquals(role, factRole))
							{
								oppositeRole = factRole;
								break;
							}
						}

						// Let the PreferredIdentiferAddedRule do all the work
						constraint.PreferredIdentifierFor = oppositeRole.RolePlayer;
					}
					else
					{
						constraint.PreferredIdentifierFor = null;
					}
				}
			}
		}
		#endregion // InternalUniquenessConstraintChangeRule class
	}
	#endregion // InternalUniquenessConstraint class
	#region PreferredIdentifierAddedRule class
	/// <summary>
	/// Verify that all preconditions hold for adding a primary
	/// identifier and extend modifiable conditions as needed.
	/// </summary>
	[RuleOn(typeof(EntityTypeHasPreferredIdentifier))]
	public class PreferredIdentifierAddedRule : AddRule
	{
		/// <summary>
		/// Check preconditions on an internal or external
		/// constraint.
		/// </summary>
		/// <param name="e"></param>
		public override void ElementAdded(ElementAddedEventArgs e)
		{
			EntityTypeHasPreferredIdentifier link = e.ModelElement as EntityTypeHasPreferredIdentifier;

			// Enforce that a preferred identifier is set only for unobjectified
			// entity types. The other parts of this (don't allow this to be set
			// for object types with preferred identifiers) is enforced in
			// ObjectType.CheckForIncompatibleRelationshipRule
			ObjectType entityType = link.PreferredIdentifierFor;
			if (entityType.IsValueType || entityType.NestedFactType != null)
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionEnforcePreferredIdentifierForUnobjectifiedEntityType);
			}

			Constraint constraint = link.PreferredIdentifier;
			switch (constraint.ConstraintType)
			{
				case ConstraintType.InternalUniqueness:
				{
					InternalUniquenessConstraint iuc = constraint as InternalUniquenessConstraint;
					iuc.TestAllowPreferred(link.PreferredIdentifierFor, true);

					// TestAllowPreferred verifies that the types and arities and that
					// no constraints need to be deleted to make this happen. Addition
					// constraints that are automatically added all happen on the opposite
					// role, so find tye, add constraints as needed, and then let this
					// pass through to finish creating the preferred identifier link.
					Role role = iuc.RoleSet.RoleCollection[0];
					Role oppositeRole = null;
					foreach (Role factRole in role.FactType.RoleCollection)
					{
						if (!object.ReferenceEquals(role, factRole))
						{
							oppositeRole = factRole;
							break;
						}
					}
					oppositeRole.IsMandatory = true; // Make sure it is mandatory
					bool needOppositeConstraint = true;
					foreach (ConstraintRoleSet roleSet in oppositeRole.ConstraintRoleSetCollection)
					{
						if (roleSet.Constraint.ConstraintType == ConstraintType.InternalUniqueness &&
							roleSet.RoleCollection.Count == 1)
						{
							needOppositeConstraint = false;
							break;
						}
					}
					if (needOppositeConstraint)
					{
						// Create a uniqueness constraint on the opposite role to make
						// this a 1-1 binary fact type.
						Store store = iuc.Store;
						InternalUniquenessConstraint oppositeIuc = InternalUniquenessConstraint.CreateInternalUniquenessConstraint(store);
						InternalConstraintRoleSet roleSet = InternalConstraintRoleSet.CreateInternalConstraintRoleSet(store);
						roleSet.RoleCollection.Add(oppositeRole);
						oppositeIuc.RoleSet = roleSet;
						oppositeIuc.Model = iuc.Model;
					}
					break;
				}
				case ConstraintType.ExternalUniqueness:
					// UNDONE: Preferred external uniqueness. Requires path information.
					break;
				default:
					throw new InvalidOperationException(ResourceStrings.ModelExceptionPreferredIdentifierMustBeUniquenessConstraint);
			}
		}
		/// <summary>
		/// This rule checkes preconditions for adding a primary
		/// identifier link. Fire it before the link is added
		/// to the transaction log.
		/// </summary>
		public override bool FireBefore
		{
			get
			{
				return true;
			}
		}
	}
	#endregion // PreferredIdentifierAddedRule class
	#region Remove testing for preferred identifier
	public partial class EntityTypeHasPreferredIdentifier
	{
		/// <summary>
		/// Call from a Removing rule to determine if the
		/// preferred identifier still fits all of the requirements.
		/// All required elements are tested for existence and IsRemoving.
		/// If any required elements are missing, then the link itself is removed.
		/// </summary>
		public void TestRemovePreferredIdentifier()
		{
			if (!IsRemoving && !IsRemoved)
			{
				// This is a bit tricky because we always have to look
				// at the links to test removing, so we can't use
				// the generated property accessors unless they have
				// remove propagation set on the opposite end.
				bool remove = true;
				Constraint constraint = PreferredIdentifier;
				if (!constraint.IsRemoving && !constraint.IsRemoved)
				{
					ObjectType forType = PreferredIdentifierFor;
					if (!forType.IsRemoving && !forType.IsRemoved)
					{
						InternalUniquenessConstraint iuc;
						ExternalUniquenessConstraint euc;
						if (null != (iuc = constraint as InternalUniquenessConstraint))
						{
							ConstraintRoleSet roleSet;
							RoleMoveableCollection roles;
							Role constraintRole;
							FactType factType;
							if (null != (roleSet = iuc.RoleSet) &&
								!roleSet.IsRemoving &&
								1 == (roles = roleSet.RoleCollection).Count &&
								!(constraintRole = roles[0]).IsRemoving &&
								null != (factType = constraintRole.FactType) &&
								!factType.IsRemoving &&
								2 == (roles = factType.RoleCollection).Count)
							{
								// Make sure we have exactly one additional single-roled internal
								// uniqueness constraint on the opposite role, and that the
								// opposite role is mandatatory, and that the role player is still
								// connected.
								Role oppositeRole = roles[0];
								if (object.ReferenceEquals(oppositeRole, constraintRole))
								{
									oppositeRole = roles[1];
								}

								// Test for attached object type. It is very common
								// to edit the link directly, so we need to check the
								// link itself, not the counterpart.
								bool rolePlayerOK = false;
								foreach (ObjectTypePlaysRole rolePlayerLink in oppositeRole.GetElementLinks(ObjectTypePlaysRole.PlayedRoleCollectionMetaRoleGuid))
								{
									if (!rolePlayerLink.IsRemoving)
									{
										rolePlayerOK = true;
										break;
									}
								}

								if (rolePlayerOK)
								{
									bool haveOppositeUniqueness = false;
									bool haveOppositeMandatory = false;
									foreach (ConstraintRoleSet testRoleSet in oppositeRole.ConstraintRoleSetCollection)
									{
										Constraint testConstraint = testRoleSet.Constraint;
										if (testConstraint != null && !testConstraint.IsRemoving)
										{
											switch (testConstraint.ConstraintType)
											{
												case ConstraintType.InternalUniqueness:
													if (haveOppositeUniqueness)
													{
														// Should only have one
														haveOppositeUniqueness = false;
														break;
													}
													if (testRoleSet.RoleCollection.Count == 1)
													{
														haveOppositeUniqueness = true;
													}
													break;
												case ConstraintType.Mandatory:
													haveOppositeMandatory = true;
													break;
											}
										}
									}
									if (haveOppositeUniqueness && haveOppositeMandatory)
									{
										remove = false;
									}
								}
							}
						}
						else if (null != (euc = constraint as ExternalUniquenessConstraint))
						{
							// UNDONE: Preferred external uniqueness. Requires path information.
						}
					}
				}
				if (remove)
				{
					Remove();
				}
			}
		}
	}
	#endregion // Remove testing for preferred identifier
	#region TestRemovePreferredIdentifierRule class
	/// <summary>
	/// A rule to determine if a mandatory condition for
	/// a preferred identifier link has been eliminated.
	/// Remove the rule if this happens.
	/// </summary>
	[RuleOn(typeof(ObjectTypePlaysRole)), RuleOn(typeof(ModelHasConstraint))]
	public class TestRemovePreferredIdentifierRule : RemovingRule
	{
		/// <summary>
		/// See if a preferred identifier is still valid
		/// </summary>
		/// <param name="e"></param>
		public override void ElementRemoving(ElementRemovingEventArgs e)
		{
			ModelElement element = e.ModelElement;
			ObjectTypePlaysRole roleLink;
			ModelHasConstraint constraintLink;
			if (null != (roleLink = element as ObjectTypePlaysRole))
			{
				ObjectType rolePlayer = roleLink.RolePlayer;
				if (!rolePlayer.IsRemoving)
				{
					IList links = rolePlayer.GetElementLinks(EntityTypeHasPreferredIdentifier.PreferredIdentifierForMetaRoleGuid);
					// Don't for each, the iterator doesn't like it when you remove elements
					int linksCount = links.Count;
					for (int i = 0; i < linksCount; ++i)
					{
						EntityTypeHasPreferredIdentifier identifierLink = links[i] as EntityTypeHasPreferredIdentifier;
						identifierLink.TestRemovePreferredIdentifier();
					}
				}
			}
			else if (null != (constraintLink = element as ModelHasConstraint))
			{
				// UNDONE: Handle delete code for constraint removal
				// UNDONE: Note that a mandatory constraint needs to be ripped
				// automatically when a note goes away.
			}
		}
	}
	#endregion // TestRemovePreferredIdentifierRule class
	#region ExternalUniquenessConstraint class
	public partial class ExternalUniquenessConstraint
	{
		#region CustomStorage handlers
		/// <summary>
		/// Standard override. Retrieve values for calculated properties.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <returns></returns>
		public override object GetValueForCustomStoredAttribute(MetaAttributeInfo attribute)
		{
			Guid attributeId = attribute.Id;
			if (attributeId == IsPreferredMetaAttributeGuid)
			{
				return PreferredIdentifierFor != null;
			}
			return base.GetValueForCustomStoredAttribute(attribute);
		}
		/// <summary>
		/// Standard override. All custom storage properties are derived, not
		/// stored. Actual changes are handled in ExternalUniquenessConstraintChangeRule.
		/// </summary>
		/// <param name="attribute">MetaAttributeInfo</param>
		/// <param name="newValue">object</param>
		public override void SetValueForCustomStoredAttribute(MetaAttributeInfo attribute, object newValue)
		{
			Guid attributeGuid = attribute.Id;
			if (attributeGuid == IsPreferredMetaAttributeGuid)
			{
				// Handled by ExternalUniquenessConstraintChangeRule
				return;
			}
			base.SetValueForCustomStoredAttribute(attribute, newValue);
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
		#region Customize property display
		/// <summary>
		/// Ensure that the Preferred property is readonly
		/// when the InternalUniquenessConstraintChangeRule is
		/// unable to make it true.
		/// </summary>
		/// <param name="propertyDescriptor"></param>
		/// <returns></returns>
		public override bool IsPropertyDescriptorReadOnly(PropertyDescriptor propertyDescriptor)
		{
			ElementPropertyDescriptor descriptor = propertyDescriptor as ElementPropertyDescriptor;
			if (descriptor != null && descriptor.MetaAttributeInfo.Id == IsPreferredMetaAttributeGuid)
			{
				// UNDONE: Preferred external uniqueness. Requires path information.
				return true;
			}
			return base.IsPropertyDescriptorReadOnly(propertyDescriptor);
		}
		#endregion // Customize property display
		#region ExternalUniquenessConstraintChangeRule class
		[RuleOn(typeof(ExternalUniquenessConstraint))]
		private class ExternalUniquenessConstraintChangeRule : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeId = e.MetaAttribute.Id;
				if (attributeId == ExternalUniquenessConstraint.IsPreferredMetaAttributeGuid)
				{
					ExternalUniquenessConstraint constraint = e.ModelElement as ExternalUniquenessConstraint;
					if ((bool)e.NewValue)
					{
						// UNDONE: Preferred external uniqueness. Requires path information.
					}
					else
					{
						constraint.PreferredIdentifierFor = null;
					}
				}
			}
		}
		#endregion // ExternalUniquenessConstraintChangeRule class
	}
	#endregion // ExternalUniquenessConstraint class
	#region ModelError classes
	public partial class TooManyRoleSetsError : IRepresentModelElements
	{
		#region Base overrides
		/// <summary>
		/// Generate and set text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			Constraint parent = Constraint;
			string parentName = (parent != null) ? parent.Name : "";
			string currentText = Name;
			string newText = string.Format(ResourceStrings.ModelErrorConstraintHasTooManyRoleSetsText, parentName);
			if (currentText != newText)
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
				return RegenerateErrorTextEvents.OwnerNameChange;
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
			return new ModelElement[]{Constraint};
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion // IRepresentModelElements Implementation
	}
	public partial class TooFewRoleSetsError : IRepresentModelElements
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			Constraint parent = Constraint;
			string parentName = (parent != null) ? parent.Name : "";
			string currentText = Name;
			string newText = string.Format(ResourceStrings.ModelErrorConstraintHasTooFewRoleSetsText, parentName);
			if (currentText != newText)
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
				return RegenerateErrorTextEvents.OwnerNameChange;
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
	#endregion // ModelError classes
	#region ExclusionType enum
	/// <summary>
	/// Represents the current setting on an exclusion constraint
	/// </summary>
	[CLSCompliant(true)]
	public enum ExclusionType
	{
		/// <summary>
		/// An exclusion constraint
		/// </summary>
		Exclusion,
		/// <summary>
		/// An inclusive-or (disjunctive mandatory) constraint
		/// </summary>
		InclusiveOr,
		/// <summary>
		/// An exclusive-or constraint
		/// </summary>
		ExclusiveOr,
	}
	#endregion // ExclustionType enum
	#region RoleSetStyles enum
	/// <summary>
	/// Flags describing the style of role sets required
	/// by each type of constraint
	/// </summary>
	[Flags]
	[CLSCompliant(true)]
	public enum RoleSetStyles
	{
		/// <summary>
		/// Constraint uses a single role set
		/// </summary>
		OneRoleSet = 1,
		/// <summary>
		/// Constraint uses exactly two role sets
		/// </summary>
		TwoRoleSets = 2,
		/// <summary>
		/// Constraint uses >=2 role sets
		/// </summary>
		MultipleRowSets = 4,
		/// <summary>
		/// A mask to extract the set multiplicity values
		/// </summary>
		SetMultiplicityMask = OneRoleSet | TwoRoleSets | MultipleRowSets,
		/// <summary>
		/// Each role set contains exactly one role
		/// </summary>
		OneRolePerSet = 8,
		/// <summary>
		/// Each role set contains exactly two roles
		/// </summary>
		TwoRolesPerSet = 0x10,
		/// <summary>
		/// Each role set can contain >=1 roles
		/// </summary>
		MultipleRolesPerSet = 0x20,
		/// <summary>
		/// The role set must contain n or n-1 roles. Applicable
		/// to OneRoleSet constraints only
		/// </summary>
		AtLeastCountMinusOneRolesPerSet = 0x40,
		/// <summary>
		/// A mask to extract the row multiplicity values
		/// </summary>
		RoleMultiplicityMask = OneRolePerSet | TwoRolesPerSet | MultipleRolesPerSet | AtLeastCountMinusOneRolesPerSet,
		/// <summary>
		/// The order of the role sets is significant
		/// </summary>
		OrderedRoleSets = 0x80,
	}
	#endregion // RoleSetStyles enum
	#region ConstraintType enum
	/// <summary>
	/// A list of constraint types.
	/// </summary>
	[CLSCompliant(true)]
	public enum ConstraintType
	{
		/// <summary>
		/// An mandatory constraint. Applied
		/// to a single role.
		/// </summary>
		Mandatory,
		/// <summary>
		/// An internal uniqueness constraint. Applied to
		/// one or more roles from the same fact type.
		/// </summary>
		InternalUniqueness,
		/// <summary>
		/// A frequency constraint. Applied to one or
		/// more roles from the same fact type.
		/// </summary>
		Frequency,
		/// <summary>
		/// A ring constraint. Applied to two roles
		/// from the same fact type. Directional.
		/// </summary>
		Ring,
		/// <summary>
		/// An external uniqueness constraint. Applied to
		/// sets of compatible roles from multiple fact types.
		/// </summary>
		ExternalUniqueness,
		/// <summary>
		/// An external equality constraint. Applied to
		/// sets of compatible roles from multiple fact types.
		/// </summary>
		Equality,
		/// <summary>
		/// An external exclusion constraint. Applied to
		/// sets of compatible roles from multiple fact types.
		/// </summary>
		Exclusion,
		/// <summary>
		/// An external subset constraint. Applied to 2
		/// sets of compatible roles.
		/// </summary>
		Subset,
	}
	#endregion // ConstraintType enum
	#region ConstraintType and RoleSetStyles implementation for all constraints
	public partial class Constraint
	{
		/// <summary>
		/// Get the type of this constraint
		/// </summary>
		public abstract ConstraintType ConstraintType { get; }
		/// <summary>
		/// Get the role settings for this constraint
		/// </summary>
		public abstract RoleSetStyles RoleSetStyles { get; }
	}
	public partial class MandatoryConstraint
	{
		/// <summary>
		/// Required override. Returns ConstraintType.Mandatory.
		/// </summary>
		public override ConstraintType ConstraintType
		{
			get
			{
				return ConstraintType.Mandatory;
			}
		}
		/// <summary>
		/// Required override. Returns {OneRoleSet, OneRolePerSet}.
		/// </summary>
		public override RoleSetStyles RoleSetStyles
		{
			get
			{
				return RoleSetStyles.OneRoleSet | RoleSetStyles.OneRolePerSet;
			}
		}
	}
	public partial class InternalUniquenessConstraint
	{
		/// <summary>
		/// Required override. Returns ConstraintType.InternalUniqueness.
		/// </summary>
		public override ConstraintType ConstraintType
		{
			get
			{
				return ConstraintType.InternalUniqueness;
			}
		}
		/// <summary>
		/// Required override. Returns {OneRoleSet, AtLeastCountMinusOneRolesPerSet}.
		/// </summary>
		public override RoleSetStyles RoleSetStyles
		{
			get
			{
				return RoleSetStyles.OneRoleSet | RoleSetStyles.AtLeastCountMinusOneRolesPerSet;
			}
		}
	}
	public partial class FrequencyConstraint
	{
		/// <summary>
		/// Required override. Returns ConstraintType.Frequency.
		/// </summary>
		public override ConstraintType ConstraintType
		{
			get
			{
				return ConstraintType.Frequency;
			}
		}
		/// <summary>
		/// Required override. Returns {OneRoleSet, MultipleRolesPerSet}.
		/// </summary>
		public override RoleSetStyles RoleSetStyles
		{
			get
			{
				return RoleSetStyles.OneRoleSet | RoleSetStyles.MultipleRolesPerSet;
			}
		}
	}
	public partial class RingConstraint
	{
		/// <summary>
		/// Required override. Returns ConstraintType.Ring.
		/// </summary>
		public override ConstraintType ConstraintType
		{
			get
			{
				return ConstraintType.Ring;
			}
		}
		/// <summary>
		/// Required override. Returns {OneRoleSet, TwoRolesPerSet}.
		/// </summary>
		public override RoleSetStyles RoleSetStyles
		{
			get
			{
				return RoleSetStyles.OneRoleSet | RoleSetStyles.TwoRolesPerSet;
			}
		}
	}
	public partial class ExternalUniquenessConstraint
	{
		/// <summary>
		/// Required override. Returns ConstraintType.ExternalUniqueness.
		/// </summary>
		public override ConstraintType ConstraintType
		{
			get
			{
				return ConstraintType.ExternalUniqueness;
			}
		}
		/// <summary>
		/// Required override. Returns {MultipleRowSets, OneRolePerSet}.
		/// </summary>
		public override RoleSetStyles RoleSetStyles
		{
			get
			{
				return RoleSetStyles.MultipleRowSets | RoleSetStyles.OneRolePerSet;
			}
		}
	}
	public partial class EqualityConstraint
	{
		/// <summary>
		/// Required override. Returns ConstraintType.Equality.
		/// </summary>
		public override ConstraintType ConstraintType
		{
			get
			{
				return ConstraintType.Equality;
			}
		}
		/// <summary>
		/// Required override. Returns {MultipleRowSets, MultipleRolesPerSet}.
		/// </summary>
		public override RoleSetStyles RoleSetStyles
		{
			get
			{
				return RoleSetStyles.MultipleRowSets | RoleSetStyles.MultipleRolesPerSet;
			}
		}
	}
	public partial class ExclusionConstraint
	{
		/// <summary>
		/// Required override. Returns ConstraintType.Exclusion.
		/// </summary>
		public override ConstraintType ConstraintType
		{
			get
			{
				return ConstraintType.Exclusion;
			}
		}
		/// <summary>
		/// Required override. Returns {MultipleRowSets, MultipleRowSets} if ExclusiontType
		/// is Exclusion, and  {MultipleRowSets, OneRolePerSet} otherwise.
		/// </summary>
		public override RoleSetStyles RoleSetStyles
		{
			get
			{
				switch (ExclusionType)
				{
					case ExclusionType.Exclusion:
						return RoleSetStyles.MultipleRowSets | RoleSetStyles.MultipleRolesPerSet;
					default:
						return RoleSetStyles.MultipleRowSets | RoleSetStyles.OneRolePerSet;
				}
			}
		}
	}
	public partial class SubsetConstraint
	{
		/// <summary>
		/// Required override. Returns ConstraintType.Subset.
		/// </summary>
		public override ConstraintType ConstraintType
		{
			get
			{
				return ConstraintType.Subset;
			}
		}
		/// <summary>
		/// Required override. Returns {TwoRoleSets, MultipleRolesPerSet, OrderedRoleSets}.
		/// </summary>
		public override RoleSetStyles RoleSetStyles
		{
			get
			{
				return RoleSetStyles.TwoRoleSets | RoleSetStyles.MultipleRolesPerSet | RoleSetStyles.OrderedRoleSets;
			}
		}
	}
	#endregion // ConstraintType and RoleSetStyles implementation for all constraints
}
