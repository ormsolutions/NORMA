using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
namespace Northface.Tools.ORM.ObjectModel
{
	public partial class Constraint
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
	}
	public partial class ExternalConstraint : INamedElementDictionaryChild, IModelErrorOwner
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
				IList untypedList = GetElementLinks(ExternalFactConstraint.ConstraintCollectionMetaRoleGuid);
				int elementCount = untypedList.Count;
				ExternalFactConstraint[] typedList = new ExternalFactConstraint[elementCount];
				untypedList.CopyTo(typedList, 0);
				return typedList;
			}
		}
		private static ExternalFactConstraint EnsureFactConstraintForRole(Role role, ExternalConstraint constraint)
		{
			ExternalFactConstraint retVal = null;
			FactType fact = role.FactType;
			if (fact != null)
			{
				IList<ExternalFactConstraint> existingFactConstraints = fact.ExternalFactConstraintCollection;
				int listCount = existingFactConstraints.Count;
				for (int i = 0; i < listCount; ++i)
				{
					ExternalFactConstraint testFactConstraint = existingFactConstraints[i];
					if (testFactConstraint.ConstraintCollection == constraint)
					{
						retVal = testFactConstraint;
						break;
					}
				}
				if (retVal == null)
				{
					fact.ConstraintCollection.Add(constraint);
				}
			}
			return retVal;
		}
		#endregion // ExternalConstraint Specific
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
		protected void GetRoleGuids(out Guid parentMetaRoleGuid, out Guid childMetaRoleGuid)
		{
			parentMetaRoleGuid = ModelHasConstraint.ModelMetaRoleGuid;
			childMetaRoleGuid = ModelHasConstraint.ConstraintCollectionMetaRoleGuid;
		}
		#endregion // INamedElementDictionaryChild implementation
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
					ExternalFactConstraint factConstraint = ExternalConstraint.EnsureFactConstraintForRole(link.RoleCollection, constraint);
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
				ExternalConstraintHasRoleSet link = e.ModelElement as ExternalConstraintHasRoleSet;
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
						ExternalFactConstraint factConstraint = ExternalConstraint.EnsureFactConstraintForRole(roleLink.RoleCollection, constraint);
						if (factConstraint != null)
						{
							factConstraint.ConstrainedRoleCollection.Add(roleLink);
						}
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
		#region Error synchronization rules
		private void VerifyRoleSetCountForRule()
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
				link.ExternalConstraint.VerifyRoleSetCountForRule();
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
					externalConstraint.VerifyRoleSetCountForRule();
				}
			}
		}
		[RuleOn(typeof(ExternalConstraintHasRoleSet), FireTime = TimeToFire.TopLevelCommit)]
		private class EnforceRoleSetCardinalityForRemove : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ExternalConstraintHasRoleSet link = e.ModelElement as ExternalConstraintHasRoleSet;
				link.ExternalConstraint.VerifyRoleSetCountForRule();
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
		#endregion // IModelErrorOwner Implementation
	}
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
