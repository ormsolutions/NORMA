using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.Framework;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region IConstraint interface
	/// <summary>
	/// Generic information shared by all constraint
	/// implementers. Constraints are model differently
	/// depending on the RoleSequenceStyle and ownership
	/// of the constraint (internal/external). External
	/// constraints that conceptually have a single role
	/// in multiple rolesets are modeled as a single role
	/// set to reduce the object model size.
	/// </summary>
	[CLSCompliant(true)]
	public interface IConstraint
	{
		/// <summary>
		/// Get the type of this constraint
		/// </summary>
		ConstraintType ConstraintType { get; }
		/// <summary>
		/// Get the role settings for this constraint
		/// </summary>
		RoleSequenceStyles RoleSequenceStyles { get; }
		/// <summary>
		/// Get the details of how this constraint is stored.
		/// </summary>
		ConstraintStorageStyle ConstraintStorageStyle { get; }
		/// <summary>
		/// Retrieve the model for the current constraint
		/// </summary>
		ORMModel Model { get; }
		/// <summary>
		/// Get the constraint modality.
		/// </summary>
		ConstraintModality Modality { get; }
	}
	#endregion // IConstraint interface
	#region Constraint class
	/// <summary>
	/// A utility class with static helper methods
	/// </summary>
	public static class ConstraintUtility
	{
		#region ConstraintUtility specific
		/// <summary>
		/// The minimum number of required role sequences
		/// </summary>
		/// <param name="constraint">Constraint class to test</param>
		public static int RoleSequenceCountMinimum(IConstraint constraint)
		{
			int retVal = 1;
			switch (constraint.RoleSequenceStyles & RoleSequenceStyles.SequenceMultiplicityMask)
			{
				case RoleSequenceStyles.MultipleRowSequences:
				case RoleSequenceStyles.TwoRoleSequences:
					retVal = 2;
					break;
#if DEBUG
				case RoleSequenceStyles.OneRoleSequence:
					break;
				default:
					Debug.Assert(false); // Shouldn't be here
					break;
#endif // DEBUG
			}
			return retVal;
		}
		/// <summary>
		/// The maximum number of required role sequences, or -1 if no max
		/// </summary>
		/// <param name="constraint">Constraint class to test</param>
		public static int RoleSequenceCountMaximum(IConstraint constraint)
		{
			int retVal = 1;
			switch (constraint.RoleSequenceStyles & RoleSequenceStyles.SequenceMultiplicityMask)
			{
				case RoleSequenceStyles.MultipleRowSequences:
					retVal = -1;
					break;
				case RoleSequenceStyles.TwoRoleSequences:
					retVal = 2;
					break;
#if DEBUG
				case RoleSequenceStyles.OneRoleSequence:
					break;
				default:
					Debug.Assert(false); // Shouldn't be here
					break;
#endif // DEBUG
			}
			return retVal;
		}
		#endregion // ConstraintUtility specific
		#region ConstraintRoleSequenceHasRoleRemoved class
		/// <summary>
		/// Rule that fires when a constraint has a RoleSequence Removed
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole), FireTime = TimeToFire.LocalCommit)]
		private class ConstraintRoleSequenceHasRoleRemoved : RemoveRule
		{
			/// <summary>
			/// Runs when roleset element is removed.  If there are no more roles in the role collection
			/// then the entire roleset is removed
			/// </summary>
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				ConstraintRoleSequence roleSequence = link.ConstraintRoleSequenceCollection;
				if (!roleSequence.IsRemoved)
				{
					if (roleSequence.RoleCollection.Count == 0)
					{
						roleSequence.Remove();
					}
				}
			}
		}
		#endregion //ConstraintRoleSequenceHasRoleRemoved class
	}
	#endregion // Constraint class
	#region InternalConstraint class
	public partial class InternalConstraint
	{
		#region InternalConstraint Specific
		/// <summary>
		/// The internal storage style of this constraint.
		/// </summary>
		/// <value>ConstraintStorageStyle.InternalConstraint</value>
		public ConstraintStorageStyle ConstraintStorageStyle
		{
			get
			{
				return ConstraintStorageStyle.InternalConstraint;
			}
		}
		/// <summary>
		/// Ensure that the role is owned by the same
		/// fact type as the constraint. This method should
		/// be called from inside a transaction
		/// and will throw
		/// </summary>
		/// <param name="role"></param>
		private void EnsureConsistentRoleOwner(Role role)
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
		#region Role owner validation rules
		/// <summary>
		/// If a role is added to an internal constraint then it must
		/// have the same owning facttype as the constraint.
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		private class ConstraintRoleSequenceHasRoleAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				InternalConstraint constraint = link.ConstraintRoleSequenceCollection.Constraint as InternalConstraint;
				if (constraint != null)
				{
					constraint.EnsureConsistentRoleOwner(link.RoleCollection);
				}
			}
		}
		/// <summary>
		/// If an internal constraint with existing roles is added
		/// to a fact type, then make sure that all of the roles
		/// are parented to the same fact type
		/// </summary>
		[RuleOn(typeof(FactTypeHasInternalConstraint))]
		private class FactTypeHasInternalConstraintAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				EnsureConsistentRoleOwnerForRoleSequence(e.ModelElement as FactTypeHasInternalConstraint);
			}
		}
		/// <summary>
		/// Helper function to support the same fact constraint fixup
		/// during both deserialization and rules.
		/// </summary>
		/// <param name="link">An internal constraint added to a fact type</param>
		private static void EnsureConsistentRoleOwnerForRoleSequence(FactTypeHasInternalConstraint link)
		{
			InternalConstraint roleSequence = link.InternalConstraintCollection;
			RoleMoveableCollection roles = roleSequence.RoleCollection;
			int roleCount = roles.Count;
			if (roleCount != 0)
			{
				for (int i = 0; i < roleCount; ++i)
				{
					Role role = roles[i];
					// Call for each role, not just the first. This
					// enforces that all roles are in the same fact type.
					roleSequence.EnsureConsistentRoleOwner(role);
				}
			}
		}
		#endregion // Role owner validation rules
	}
	#endregion // InternalConstraint class
	#region SingleColumnExternalConstraint class
	public partial class SingleColumnExternalConstraint : IModelErrorOwner
	{
		#region SingleColumnExternalConstraint Specific
		/// <summary>
		/// The internal storage style of this constraint.
		/// </summary>
		/// <value>ConstraintStorageStyle.SingleColumnExternalConstraint</value>
		public ConstraintStorageStyle ConstraintStorageStyle
		{
			get
			{
				return ConstraintStorageStyle.SingleColumnExternalConstraint;
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
					IList existingFactConstraints = fact.GetElementLinks(SingleColumnExternalFactConstraint.FactTypeCollectionMetaRoleGuid);
					int listCount = existingFactConstraints.Count;
					for (int i = 0; i < listCount; ++i)
					{
						SingleColumnExternalFactConstraint testFactConstraint = (SingleColumnExternalFactConstraint)existingFactConstraints[i];
						if (testFactConstraint.SingleColumnExternalConstraintCollection == this)
						{
							retVal = testFactConstraint;
							break;
						}
					}
					if (retVal == null)
					{
						fact.SingleColumnExternalConstraintCollection.Add(this);
					}
				}
			}
			return retVal;
		}
		#endregion // SingleColumnExternalConstraint Specific
		#region SingleColumnExternalConstraint synchronization rules
		/// <summary>
		/// If a role is added after the role sequence is already attached,
		/// then create the corresponding ExternalFactConstraint and ExternalRoleConstraint
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		private class ConstraintRoleSequenceHasRoleAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				SingleColumnExternalConstraint constraint = link.ConstraintRoleSequenceCollection.Constraint as SingleColumnExternalConstraint;
				if (constraint != null && constraint.Model != null)
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
		/// If a role sequence is added that already contains roles, then
		/// make sure the corresponding ExternalFactConstraint and ExternalRoleConstraint
		/// objects are created for each role. Note that a single column external
		/// constraint is a role sequence.
		/// </summary>
		[RuleOn(typeof(ModelHasSingleColumnExternalConstraint))]
		private class ConstraintHasRoleSequenceAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				EnsureFactConstraintForRoleSequence(e.ModelElement as ModelHasSingleColumnExternalConstraint);
			}
		}
		/// <summary>
		/// Helper function to support the same fact constraint fixup
		/// during both deserialization and rules.
		/// </summary>
		/// <param name="link">A roleset link added to the constraint</param>
		private static void EnsureFactConstraintForRoleSequence(ModelHasSingleColumnExternalConstraint link)
		{
			SingleColumnExternalConstraint roleSequence = link.SingleColumnExternalConstraintCollection;
			// The following line gets the links instead of the counterparts,
			// which are provided by roleSequence.RoleCollection
			IList roleLinks = roleSequence.GetElementLinks(ConstraintRoleSequenceHasRole.ConstraintRoleSequenceCollectionMetaRoleGuid);
			int roleCount = roleLinks.Count;
			if (roleCount != 0)
			{
				for (int i = 0; i < roleCount; ++i)
				{
					ConstraintRoleSequenceHasRole roleLink = (ConstraintRoleSequenceHasRole)roleLinks[i];
					ExternalFactConstraint factConstraint = roleSequence.EnsureFactConstraintForRole(roleLink.RoleCollection);
					if (factConstraint != null)
					{
						factConstraint.ConstrainedRoleCollection.Add(roleLink);
					}
				}
			}
		}
		#endregion // SingleColumnExternalConstraint synchronization rules
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
				CompatibleRolePlayerTypeError typeCompatibility;
				if (null != (typeCompatibility = CompatibleRolePlayerTypeError))
				{
					yield return typeCompatibility;
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
			VerifyCompatibleRolePlayerTypeForRule(notifyAdded);
		}
		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateErrors(notifyAdded);
		}
		#endregion // IModelErrorOwner Implementation
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// adds the implicit ExternalFactConstraint elements.
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
		private class ExternalConstraintFixupListener : DeserializationFixupListener<SingleColumnExternalConstraint>
		{
			/// <summary>
			/// ExternalConstraintFixupListener constructor
			/// </summary>
			public ExternalConstraintFixupListener() : base((int)ORMDeserializationFixupPhase.AddImplicitElements)
			{
			}
			/// <summary>
			/// Process elements by added an ExternalFactConstraint for
			/// each roleset
			/// </summary>
			/// <param name="element">An ExternalConstraint element</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected override void ProcessElement(SingleColumnExternalConstraint element, Store store, INotifyElementAdded notifyAdded)
			{
				IList links = element.GetElementLinks(ModelHasSingleColumnExternalConstraint.SingleColumnExternalConstraintCollectionMetaRoleGuid);
				int linksCount = links.Count;
				for (int i = 0; i < linksCount; ++i)
				{
					EnsureFactConstraintForRoleSequence(links[i] as ModelHasSingleColumnExternalConstraint);
					IList factLinks = element.GetElementLinks(SingleColumnExternalFactConstraint.SingleColumnExternalConstraintCollectionMetaRoleGuid);
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
		// UNDONE: MultiColumnExternalConstraint error checking rules need to be
		// ported to act on single column as well. Single column needs to look like
		// multiple role sets to the end user, except that only a single column is allowed
		// in each row. There, it is appropriate to attach the TooFew/TooMany-RoleSequencesError
		// objects here as well.


		/// <summary>
		/// Verify CompatibleRolePlayertypeForRule Used to verify compatibility for single column constraints.
		/// </summary>
		/// <param name="notifyAdded">If not null, this is being called during
		/// load when rules are not in place. Any elements that are added
		/// must be notified back to the caller.</param>
		private void VerifyCompatibleRolePlayerTypeForRule(INotifyElementAdded notifyAdded)
		{
			CompatibleRolePlayerTypeError compatibleError;
			if (0 == (((IConstraint)this).RoleSequenceStyles & RoleSequenceStyles.CompatibleColumns))
			{
				if (notifyAdded != null)
				{
					// Check on load, but not later. Constraint types don't switch on the fly
					if (null != (compatibleError = CompatibleRolePlayerTypeError))
					{
						compatibleError.Remove();
					}
				}
				return;
			}

			bool isCompatible = true;

			RoleMoveableCollection roles = RoleCollection;
			int roleCount = roles.Count;
			if (roleCount > 1)
			{
				// We will only test incompatibility if we find more than
				// only role that actually has a role player. We'll cache the
				// full set of supertypes for the first roleplayer we find,
				// then walk the supertypes for all other types to find an
				// intersection with the first set.
				ObjectType firstRolePlayer = null;
				Collection<ObjectType> superTypesCache = null;

				for (int i = 0; i < roleCount; ++i)
				{
					Role currentRole = roles[i];
					ObjectType currentRolePlayer = currentRole.RolePlayer;
					if (currentRolePlayer != null)
					{
						if (firstRolePlayer == null)
						{
							// Store the first role player. We won't populate until
							// we're sure we need to.
							firstRolePlayer = currentRolePlayer;
						}
						else
						{
							if (superTypesCache == null)
							{
								// Populate the cache
								superTypesCache = new Collection<ObjectType>();
								ObjectType.WalkSupertypes(firstRolePlayer, delegate(ObjectType type)
								{
									superTypesCache.Add(type);
									return true;
								});
							}
							// If the type is contained, WalkSupertype will return false because the iteration
							// did not complete.
							isCompatible = !ObjectType.WalkSupertypes(currentRolePlayer, delegate(ObjectType type)
							{
								// Continue iteration if the type is recognized in the cache
								return !superTypesCache.Contains(type);
							});

							if (!isCompatible)
							{
								//If the error is not present, add it to the model
								if (null == CompatibleRolePlayerTypeError)
								{
									compatibleError = CompatibleRolePlayerTypeError.CreateCompatibleRolePlayerTypeError(Store);
									compatibleError.Model = Model;
									compatibleError.SingleColumnExternalConstraint = this;
									compatibleError.GenerateErrorText();
									if (notifyAdded != null)
									{
										notifyAdded.ElementAdded(compatibleError, true);
									}
								}
								return;
							}
						}
					}
				}
			}
			//If the matches are compatible, then remove any errors that may be present
			if (isCompatible)
			{
				if (null != (compatibleError = CompatibleRolePlayerTypeError))
				{
					compatibleError.Remove();
				}
			}
		}

		//		Add and Remove Rules for a Single Column Constraint to enforce the Compatibility between fact types


		/// <summary>
		/// Add Rule for VerifyCompatibleRolePlayer when a Role/Object relationship is added
		/// </summary>
		[RuleOn(typeof(ObjectTypePlaysRole), FireTime = TimeToFire.TopLevelCommit)]
		private class EnforceRoleSequenceValidityForFactTypeAdd : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				Role role = link.PlayedRoleCollection;
				ConstraintRoleSequenceMoveableCollection roleSequences = role.ConstraintRoleSequenceCollection;
				int count = roleSequences.Count;
				for (int i = 0; i < count; ++i)
				{
					SingleColumnExternalConstraint sequence = roleSequences[i] as SingleColumnExternalConstraint;
					if (sequence != null)
					{
						sequence.VerifyCompatibleRolePlayerTypeForRule(null);
					}
				}
			}
		}

		/// <summary>
		///Remove Rule for VerifyCompatibleRolePlayer when a Role/Object relationship is removed
		/// </summary>
		[RuleOn(typeof(ObjectTypePlaysRole), FireTime = TimeToFire.TopLevelCommit)]
		private class EnforceRoleSequenceValidityForFactTypeRemove : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				Role role = link.PlayedRoleCollection;
				ConstraintRoleSequenceMoveableCollection roleSequences = role.ConstraintRoleSequenceCollection;
				int count = roleSequences.Count;
				for (int i = 0; i < count; ++i)
				{

					SingleColumnExternalConstraint sequence = roleSequences[i] as SingleColumnExternalConstraint;
					if (sequence != null)
					{
						sequence.VerifyCompatibleRolePlayerTypeForRule(null);
					}
				}
			}
		}
		#endregion // Error synchronization rules
	}
	#endregion // SingleColumnExternalConstraint class
	#region MultiColumnExternalConstraint class
	public partial class MultiColumnExternalConstraint : IModelErrorOwner
	{
		#region MultiColumnExternalConstraint Specific
		/// <summary>
		/// The internal storage style of this constraint.
		/// </summary>
		/// <value>ConstraintStorageStyle.MultiColumnExternalConstraint</value>
		public ConstraintStorageStyle ConstraintStorageStyle
		{
			get
			{
				return ConstraintStorageStyle.MultiColumnExternalConstraint;
			}
		}
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
				IList untypedList = GetElementLinks(MultiColumnExternalFactConstraint.MultiColumnExternalConstraintCollectionMetaRoleGuid);
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
					IList existingFactConstraints = fact.GetElementLinks(MultiColumnExternalFactConstraint.FactTypeCollectionMetaRoleGuid);
					int listCount = existingFactConstraints.Count;
					for (int i = 0; i < listCount; ++i)
					{
						MultiColumnExternalFactConstraint testFactConstraint = (MultiColumnExternalFactConstraint)existingFactConstraints[i];
						if (testFactConstraint.MultiColumnExternalConstraintCollection == this)
						{
							retVal = testFactConstraint;
							break;
						}
					}
					if (retVal == null)
					{
						fact.MultiColumnExternalConstraintCollection.Add(this);
					}
				}
			}
			return retVal;
		}
		#endregion // MultiColumnExternalConstraint Specific
		#region MultiColumnExternalConstraint synchronization rules
		/// <summary>
		/// If a role is added after the role sequence is already attached,
		/// then create the corresponding ExternalFactConstraint and ExternalRoleConstraint
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		private class ConstraintRoleSequenceHasRoleAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				MultiColumnExternalConstraint constraint = link.ConstraintRoleSequenceCollection.Constraint as MultiColumnExternalConstraint;
				if (constraint != null && constraint.Model != null)
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
		/// If a role sequence is added that already contains roles, then
		/// make sure the corresponding ExternalFactConstraint and ExternalRoleConstraint
		/// objects are created for each role.
		/// </summary>
		[RuleOn(typeof(MultiColumnExternalConstraintHasRoleSequence), FireTime = TimeToFire.LocalCommit)]
		private class ConstraintHasRoleSequenceAdded : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				EnsureFactConstraintForRoleSequence(e.ModelElement as MultiColumnExternalConstraintHasRoleSequence);
			}
		}
		/// <summary>
		/// Helper function to support the same fact constraint fixup
		/// during both deserialization and rules.
		/// </summary>
		/// <param name="link">A roleset link added to the constraint</param>
		private static void EnsureFactConstraintForRoleSequence(MultiColumnExternalConstraintHasRoleSequence link)
		{
			MultiColumnExternalConstraintRoleSequence roleSequence = link.RoleSequenceCollection;
			// The following line gets the links instead of the counterparts,
			// which are provided by roleSequence.RoleCollection
			IList roleLinks = roleSequence.GetElementLinks(ConstraintRoleSequenceHasRole.ConstraintRoleSequenceCollectionMetaRoleGuid);
			int roleCount = roleLinks.Count;
			if (roleCount != 0)
			{
				MultiColumnExternalConstraint constraint = link.ExternalConstraint;
				for (int i = 0; i < roleCount; ++i)
				{
					ConstraintRoleSequenceHasRole roleLink = (ConstraintRoleSequenceHasRole)roleLinks[i];
					ExternalFactConstraint factConstraint = constraint.EnsureFactConstraintForRole(roleLink.RoleCollection);
					if (factConstraint != null)
					{
						factConstraint.ConstrainedRoleCollection.Add(roleLink);
					}
				}
			}
		}
		/// <summary>
		/// Rip an ExternalFactConstraint relationship when its last role
		/// goes away. Note that this rule also affects single column external
		/// constraints, but we only need to write it once.
		/// </summary>
		[RuleOn(typeof(ExternalRoleConstraint), FireTime = TimeToFire.LocalCommit, Priority = 1000)]
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
		#endregion // MultiColumnExternalConstraint synchronization rules
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// adds the implicit ExternalFactConstraint elements.
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
		private class ExternalConstraintFixupListener : DeserializationFixupListener<MultiColumnExternalConstraint>
		{
			/// <summary>
			/// ExternalConstraintFixupListener constructor
			/// </summary>
			public ExternalConstraintFixupListener() : base((int)ORMDeserializationFixupPhase.AddImplicitElements)
			{
			}
			/// <summary>
			/// Process elements by added an ExternalFactConstraint for
			/// each roleset
			/// </summary>
			/// <param name="element">An ExternalConstraint element</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected override void ProcessElement(MultiColumnExternalConstraint element, Store store, INotifyElementAdded notifyAdded)
			{
				IList links = element.GetElementLinks(MultiColumnExternalConstraintHasRoleSequence.ExternalConstraintMetaRoleGuid);
				int linksCount = links.Count;
				for (int i = 0; i < linksCount; ++i)
				{
					EnsureFactConstraintForRoleSequence(links[i] as MultiColumnExternalConstraintHasRoleSequence);
					IList factLinks = element.GetElementLinks(MultiColumnExternalFactConstraint.MultiColumnExternalConstraintCollectionMetaRoleGuid);
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
		#region VerifyRoleSequenceCountForRule
		/// <summary>
		/// Add, remove, and otherwise validate the current set of
		/// errors for this constraint.
		/// </summary>
		/// <param name="notifyAdded">If not null, this is being called during
		/// load when rules are not in place. Any elements that are added
		/// must be notified back to the caller.</param>
		private void VerifyRoleSequenceCountForRule(INotifyElementAdded notifyAdded)
		{
			if (!IsRemoved)
			{
				int minCount = ConstraintUtility.RoleSequenceCountMinimum(this);
				int maxCount;
				int currentCount = RoleSequenceCollection.Count;
				Store store = Store;
				TooFewRoleSequencesError insufficientError;
				TooManyRoleSequencesError extraError;
				bool removeTooFew = false;
				bool removeTooMany = false;
				bool tooFewOrTooMany = false;
				if (currentCount < minCount)
				{
					tooFewOrTooMany = true;
					if (null == TooFewRoleSequencesError)
					{
						insufficientError = TooFewRoleSequencesError.CreateTooFewRoleSequencesError(store);
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
					if ((-1 != (maxCount = ConstraintUtility.RoleSequenceCountMaximum(this))) && (currentCount > maxCount))
					{
						tooFewOrTooMany = true;
						if (null == TooManyRoleSequencesError)
						{
							extraError = TooManyRoleSequencesError.CreateTooManyRoleSequencesError(store);
							extraError.Model = Model;
							extraError.Constraint = this;
							extraError.GenerateErrorText();
							if (notifyAdded != null)
							{
								notifyAdded.ElementAdded(extraError, true);
							}
						}
					}
					else
					{
						removeTooMany = true;
					}
				}
				if (removeTooFew && null != (insufficientError = TooFewRoleSequencesError))
				{
					insufficientError.Remove();
				}
				if (removeTooMany && null != (extraError = TooManyRoleSequencesError))
				{
					extraError.Remove();
				}

				VerifyRoleSequenceArityForRule(notifyAdded, tooFewOrTooMany);
			}
		}
		#endregion // VerifyRoleSequenceCountForRule
		#region VerifyRoleSequenceArityForRule
		/// <summary>
		/// Add, remove, and otherwise validate the current set of
		/// errors for this constraint.
		/// </summary>
		/// <param name="notifyAdded">If not null, this is being called during
		/// load when rules are not in place. Any elements that are added
		/// must be notified back to the caller.</param>
		private void VerifyRoleSequenceArityForRule(INotifyElementAdded notifyAdded)
		{
			if (null == TooFewRoleSequencesError && null == TooManyRoleSequencesError)
			{
				VerifyRoleSequenceArityForRule(notifyAdded, false);
			}
		}

		/// <summary>
		/// Add, remove, and otherwise validate the current set of
		/// errors for this constraint.
		/// </summary>
		/// <param name="notifyAdded">If not null, this is being called during
		/// load when rules are not in place. Any elements that are added
		/// must be notified back to the caller.</param>
		/// <param name="tooFewOrTooManySequences">Represents correct number of sequences
		/// for current constraint.  If the constraint has too few or too many sequences, 
		/// will remove this error if present.</param>
		private void VerifyRoleSequenceArityForRule(INotifyElementAdded notifyAdded, bool tooFewOrTooManySequences)
		{
			ExternalConstraintRoleSequenceArityMismatchError arityError;
			bool arityValid = true;
			int currentCount = RoleSequenceCollection.Count;
			Store store = Store;

			if (tooFewOrTooManySequences)
			{
				arityError = ArityMismatchError;
				if (arityError != null)
				{
					arityError.Remove(); // Can't validate arity with the wrong number of role sequences
				}
			}
			else
			{
				if (currentCount != 0)
				{
					IList sequences = RoleSequenceCollection;
					int arity = ((ConstraintRoleSequence)sequences[0]).RoleCollection.Count;
					for (int i = 1; i < currentCount; ++i)
					{
						if (arity != ((ConstraintRoleSequence)sequences[i]).RoleCollection.Count)
						{
							arityValid = false;
							arityError = ArityMismatchError;
							if (arityError == null)
							{
								arityError = ExternalConstraintRoleSequenceArityMismatchError.CreateExternalConstraintRoleSequenceArityMismatchError(store);
								arityError.Model = Model;
								arityError.Constraint = this;
								arityError.GenerateErrorText();
								if (notifyAdded != null)
								{
									notifyAdded.ElementAdded(arityError, true);
								}
							}
							break;
						}
					}
				}
				if (arityValid)
				{
					if (null != (arityError = ArityMismatchError))
					{
						arityError.Remove();
					}
				}
			}
			VerifyCompatibleRolePlayerTypeForRule(notifyAdded);
		}
		#endregion // VerifyRoleSequenceArityForRule
		#region VerifyCompatibleRolePlayerTypeForRule
		/// <summary>
		/// Add, remove, and otherwise validate the current CompatibleRolePlayerType
		/// errors
		/// </summary>
		/// <param name="notifyAdded">If not null, this is being called during
		/// load when rules are not in place. Any elements that are added
		/// must be notified back to the caller.</param>
		/// <param name="tooFewOrTooManySequencesOrArity">Represents correct number of sequences
		/// for current constraint.  If the constraint has too few, too many, or the wrong arity of 
		/// sequences, it will remove this error if present.</param>
		private void VerifyCompatibleRolePlayerTypeForRule(INotifyElementAdded notifyAdded, bool tooFewOrTooManySequencesOrArity)
		{
			Debug.Assert(0 != (((IConstraint)this).RoleSequenceStyles & RoleSequenceStyles.CompatibleColumns)); // All multi column externals support column compatibility
			CompatibleRolePlayerTypeError compatibleError;
			Store store = Store;

			//We don't want to display the error if arity error present or toofeworTooMany sequence errors are present
			if (tooFewOrTooManySequencesOrArity)
			{
				CompatibleRolePlayerTypeErrorCollection.Clear();
			}
			else
			{
				MultiColumnExternalConstraintRoleSequenceMoveableCollection sequences = RoleSequenceCollection;
				int sequenceCount = sequences.Count;

				if (sequenceCount > 1)
				{
					// Cache the role collection so we're not regenerating them all the time
					RoleMoveableCollection[] roleCollections = new RoleMoveableCollection[sequenceCount];
					for (int i = 0; i < sequenceCount; ++i)
					{
						roleCollections[i] = sequences[i].RoleCollection;
					}

					CompatibleRolePlayerTypeErrorMoveableCollection startingErrors = CompatibleRolePlayerTypeErrorCollection;
					int startingErrorCount = startingErrors.Count;
					int nextStartingError = 0;

					// Verify each column individually
					int rolePlayerCount = roleCollections[0].Count;
					for (int column = 0; column < rolePlayerCount; ++column)
					{
						// We will only test incompatibility if we find more than
						// only role that actually has a role player. We'll cache the
						// full set of supertypes for the first roleplayer we find,
						// then walk the supertypes for all other types to find an
						// intersection with the first set.
						ObjectType firstRolePlayer = null;
						Collection<ObjectType> superTypesCache = null;

						for (int sequence = 0; sequence < sequenceCount; ++sequence)
						{
							Role currentRole = roleCollections[sequence][column];
							ObjectType currentRolePlayer = currentRole.RolePlayer;
							if (currentRolePlayer != null)
							{
								if (firstRolePlayer == null)
								{
									// Store the first role player. We won't populate until
									// we're sure we need to.
									firstRolePlayer = currentRolePlayer;
								}
								else
								{
									if (superTypesCache == null)
									{
										// Populate the cache
										superTypesCache = new Collection<ObjectType>();
										ObjectType.WalkSupertypes(firstRolePlayer, delegate(ObjectType type)
										{
											superTypesCache.Add(type);
											return true;
										});
									}
									// If the type is contained, WalkSupertype will return false because the iteration
									// did not complete.
									bool isCompatible = !ObjectType.WalkSupertypes(currentRolePlayer, delegate(ObjectType type)
									{
										// Continue iteration if the type is recognized in the cache
										return !superTypesCache.Contains(type);
									});
									if (!isCompatible)
									{
										// If a starting error is present, then adjust its column
										// property and regenerate the text
										if (nextStartingError < startingErrorCount)
										{
											compatibleError = startingErrors[nextStartingError];
											++nextStartingError;
											compatibleError.Column = column;
											compatibleError.GenerateErrorText();
										}
										else
										{
											// We need a new error, create it from scratch
											compatibleError = CompatibleRolePlayerTypeError.CreateCompatibleRolePlayerTypeError(store);
											compatibleError.Model = Model;
											compatibleError.Column = column;
											compatibleError.MultiColumnExternalConstraint = this;
											compatibleError.GenerateErrorText();
											if (notifyAdded != null)
											{
												notifyAdded.ElementAdded(compatibleError, true);
											}
										}
									}
								}
							}
						}
					}

					// If any errors are left, then remove them, we have enough
					for (int i = startingErrorCount - 1; i >= nextStartingError; --i)
					{
						startingErrors[i].Remove();
					}
				}
				else
				{
					CompatibleRolePlayerTypeErrorCollection.Clear();
				}
			}
		}
		private void VerifyCompatibleRolePlayerTypeForRule(INotifyElementAdded notifyAdded)
		{
			if (null == TooFewRoleSequencesError && null == TooManyRoleSequencesError && null == ArityMismatchError)
			{
				VerifyCompatibleRolePlayerTypeForRule(notifyAdded, false);
			}
			else
			{
				VerifyCompatibleRolePlayerTypeForRule(notifyAdded, true);
			}
		}
		#endregion // VerifyCompatibleRolePlayerTypeForRule
		#region Add/Remove Rules
		[RuleOn(typeof(MultiColumnExternalConstraintHasRoleSequence), FireTime = TimeToFire.TopLevelCommit)]
		private class EnforceRoleSequenceCardinalityForAdd : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				MultiColumnExternalConstraintHasRoleSequence link = e.ModelElement as MultiColumnExternalConstraintHasRoleSequence;
				link.ExternalConstraint.VerifyRoleSequenceCountForRule(null);
			}
		}
		[RuleOn(typeof(ModelHasMultiColumnExternalConstraint), FireTime = TimeToFire.TopLevelCommit)]
		private class EnforceRoleSequenceCardinalityForConstraintAdd : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelHasMultiColumnExternalConstraint link = e.ModelElement as ModelHasMultiColumnExternalConstraint;
				MultiColumnExternalConstraint externalConstraint = link.MultiColumnExternalConstraintCollection as MultiColumnExternalConstraint;
				if (externalConstraint != null)
				{
					externalConstraint.VerifyRoleSequenceCountForRule(null);
				}
			}
		}
		[RuleOn(typeof(MultiColumnExternalConstraintHasRoleSequence), FireTime = TimeToFire.LocalCommit)]
		private class EnforceRoleSequenceCardinalityForRemove : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				MultiColumnExternalConstraintHasRoleSequence link = e.ModelElement as MultiColumnExternalConstraintHasRoleSequence;
				MultiColumnExternalConstraint externalConstraint = link.ExternalConstraint;
				if (externalConstraint != null && !externalConstraint.IsRemoved)
				{
					externalConstraint.VerifyRoleSequenceCountForRule(null);
				}
			}
		}
		/// <summary>
		/// Add Rule for arity and compatibility checking when ExternalConstraints roles are added
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole), FireTime = TimeToFire.LocalCommit)]
		private class EnforceRoleSequenceValidityForAdd : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				MultiColumnExternalConstraintRoleSequence sequence = link.ConstraintRoleSequenceCollection as MultiColumnExternalConstraintRoleSequence;
				if (sequence != null)
				{
					MultiColumnExternalConstraint constraint = sequence.ExternalConstraint;
					if (constraint != null)
					{
						sequence.ExternalConstraint.VerifyRoleSequenceArityForRule(null);
						sequence.ExternalConstraint.VerifyCompatibleRolePlayerTypeForRule(null);
					}
				}
			}
		}

		//Remove Rule for VerifyCompatibleRolePlayer when ExternalConstraints roles are removed
		[RuleOn(typeof(ConstraintRoleSequenceHasRole), FireTime = TimeToFire.LocalCommit)]
		private class EnforceRoleSequenceValidityForRemove : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				MultiColumnExternalConstraintRoleSequence sequence = link.ConstraintRoleSequenceCollection as MultiColumnExternalConstraintRoleSequence;
				if (sequence != null)
				{
					MultiColumnExternalConstraint externalConstraint = sequence.ExternalConstraint;
					if (externalConstraint != null && !externalConstraint.IsRemoved)
					{
						externalConstraint.VerifyRoleSequenceArityForRule(null);
						externalConstraint.VerifyCompatibleRolePlayerTypeForRule(null);
					}
				}
			}
		}

		//Add Rule for VerifyCompatibleRolePlayer when a Role/Object relationship is added
		[RuleOn(typeof(ObjectTypePlaysRole), FireTime = TimeToFire.LocalCommit)]
		private class EnforceRoleSequenceValidityForFactTypeAdd : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				Role role = link.PlayedRoleCollection;
				ConstraintRoleSequenceMoveableCollection roleSequences = role.ConstraintRoleSequenceCollection;
				int count = roleSequences.Count;
				for (int i = 0; i < count; ++i)
				{
					MultiColumnExternalConstraintRoleSequence sequence = roleSequences[i] as MultiColumnExternalConstraintRoleSequence;
					if (sequence != null)
					{
						sequence.ExternalConstraint.VerifyCompatibleRolePlayerTypeForRule(null);
					}
				}
			}
		}

		//Remove Rule for VerifyCompatibleRolePlayer when a Role/Object relationship is removed
		[RuleOn(typeof(ObjectTypePlaysRole), FireTime = TimeToFire.LocalCommit)]
		private class EnforceRoleSequenceValidityForFactTypeRemove : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				Role role = link.PlayedRoleCollection;
				ConstraintRoleSequenceMoveableCollection roleSequences = role.ConstraintRoleSequenceCollection;
				int count = roleSequences.Count;
				for (int i = 0; i < count; ++i)
				{
					MultiColumnExternalConstraintRoleSequence sequence = roleSequences[i] as MultiColumnExternalConstraintRoleSequence;
					if (sequence != null && !sequence.IsRemoved)
					{
						MultiColumnExternalConstraint externalConstraint = sequence.ExternalConstraint;
						if (externalConstraint != null && !externalConstraint.IsRemoved)
						{
							externalConstraint.VerifyCompatibleRolePlayerTypeForRule(null);
						}
					}
				}
			}
		}
		#endregion // Add/Remove Rules
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
				TooManyRoleSequencesError tooMany;
				TooFewRoleSequencesError tooFew;
				ExternalConstraintRoleSequenceArityMismatchError arityMismatch;
				if (null != (tooMany = TooManyRoleSequencesError))
				{
					yield return tooMany;
				}
				if (null != (tooFew = TooFewRoleSequencesError))
				{
					yield return tooFew;
				}
				if (null != (arityMismatch = ArityMismatchError))
				{
					yield return arityMismatch;
				}
				foreach (CompatibleRolePlayerTypeError compatibleTypeError in CompatibleRolePlayerTypeErrorCollection)
				{
					yield return compatibleTypeError;
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
			VerifyRoleSequenceCountForRule(notifyAdded);
			VerifyRoleSequenceArityForRule(notifyAdded);
			VerifyCompatibleRolePlayerTypeForRule(notifyAdded);
		}
		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateErrors(notifyAdded);
		}
		#endregion // IModelErrorOwner Implementation
	}
	public partial class MultiColumnExternalConstraint : IConstraint
	{
		#region IConstraint Implementation
		ORMModel IConstraint.Model
		{
			get
			{
				return Model;
			}
		}
		ConstraintType IConstraint.ConstraintType
		{
			get
			{
				Debug.Assert(false); // Implement on derived class
				throw new NotImplementedException();
			}
		}
		RoleSequenceStyles IConstraint.RoleSequenceStyles
		{
			get
			{
				Debug.Assert(false); // Implement on derived class
				throw new NotImplementedException();
			}
		}
		#endregion // IConstraint Implementation
	}
	public partial class SingleColumnExternalConstraint : IConstraint
	{
		#region IConstraint Implementation
		ORMModel IConstraint.Model
		{
			get
			{
				return Model;
			}
		}
		ConstraintType IConstraint.ConstraintType
		{
			get
			{
				Debug.Assert(false); // Implement on derived class
				throw new NotImplementedException();
			}
		}
		RoleSequenceStyles IConstraint.RoleSequenceStyles
		{
			get
			{
				Debug.Assert(false); // Implement on derived class
				throw new NotImplementedException();
			}
		}
		#endregion // IConstraint Implementation
	}
	#endregion // MultiColumnExternalConstraint class
	#region FactConstraint classes
	public partial class InternalConstraint : IFactConstraint, IConstraint
	{
		#region IFactConstraint Implementation
		IConstraint IFactConstraint.Constraint
		{
			get
			{
				return Constraint; // Implemented for ConstraintRoleSequence
			}
		}
		IList<Role> IFactConstraint.RoleCollection
		{
			get
			{
				return IFactConstraintRoleCollection;
			}
		}
		/// <summary>
		/// Implements IFactConstraint.RoleCollection
		/// </summary>
		[CLSCompliant(false)]
		protected IList<Role> IFactConstraintRoleCollection
		{
			get
			{
				RoleMoveableCollection roles = RoleCollection;
				int roleCount = roles.Count;
				Role[] typedList = new Role[roleCount];
				if (roleCount != 0)
				{
					roles.CopyTo(typedList, 0);
				}
				return typedList;
			}
		}
		FactType IFactConstraint.FactType
		{
			get
			{
				// Defer to generated relationship property
				return FactType;
			}
		}
		#endregion // IFactConstraint Implementation
		#region IConstraint Implementation
		ORMModel IConstraint.Model
		{
			get
			{
				return Model;
			}
		}
		/// <summary>
		/// Implements IConstraint.Model. Defers to the
		/// model of the owning fact type.
		/// </summary>
		protected ORMModel Model
		{
			get
			{
				FactType factType = FactType;
				return (factType != null) ? factType.Model : null;
			}
		}
		ConstraintType IConstraint.ConstraintType
		{
			get
			{
				Debug.Assert(false); // Implement on derived class
				throw new NotImplementedException();
			}
		}
		RoleSequenceStyles IConstraint.RoleSequenceStyles
		{
			get
			{
				Debug.Assert(false); // Implement on derived class
				throw new NotImplementedException();
			}
		}
		#endregion // IConstraint Implementation
	}
	public partial class ExternalFactConstraint : IFactConstraint
	{
		#region IFactConstraint Implementation
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
				ConstraintRoleSequenceHasRoleMoveableCollection roleSequenceLinks = ConstrainedRoleCollection;
				int roleSequenceLinksCount = roleSequenceLinks.Count;
				Role[] typedList = new Role[roleSequenceLinksCount];
				for (int i = 0; i < roleSequenceLinksCount; ++i)
				{
					typedList[i] = roleSequenceLinks[i].RoleCollection;
				}
				return typedList;
			}
		}
		FactType IFactConstraint.FactType
		{
			get
			{
				// This is for the compiler, which doesn't like the protected FactType property.
				// Overriding on the derived types enables IFactConstraint support without
				// a double virtual call to get the data.
				Debug.Assert(false);
				return FactType;
			}
		}
		IConstraint IFactConstraint.Constraint
		{
			get
			{
				// This is for the compiler, which doesn't like the protected FactType property.
				// Overriding on the derived types enables IFactConstraint support without
				// a double virtual call to get the data.
				Debug.Assert(false);
				return Constraint;
			}
		}
		/// <summary>
		/// Retrieve the fact type from a derived class
		/// </summary>
		protected abstract FactType FactType {get;}
		/// <summary>
		/// Retrieve the constraint from a derived class
		/// </summary>
		protected abstract IConstraint Constraint { get;}
		#endregion // IFactConstraint Implementation
	}
	public partial class MultiColumnExternalFactConstraint : IFactConstraint
	{
		FactType IFactConstraint.FactType
		{
			get
			{
				return FactType;
			}
		}
		IConstraint IFactConstraint.Constraint
		{
			get
			{
				return Constraint;
			}
		}
		/// <summary>
		/// Implements FactType for IFactConstraint and ExternalFactConstraint
		/// by deferring to generated FactTypeCollection accessor
		/// </summary>
		protected sealed override FactType FactType
		{
			get
			{
				return FactTypeCollection;
			}
		}
		/// <summary>
		/// Implements Constraint for IFactConstraint and ExternalFactConstraint
		/// by deferring to generated MultiColumnExternalConstraintCollection accessor
		/// </summary>
		protected sealed override IConstraint Constraint
		{
			get
			{
				return MultiColumnExternalConstraintCollection;
			}
		}
	}
	public partial class SingleColumnExternalFactConstraint : IFactConstraint
	{
		FactType IFactConstraint.FactType
		{
			get
			{
				return FactType;
			}
		}
		IConstraint IFactConstraint.Constraint
		{
			get
			{
				return Constraint;
			}
		}
		/// <summary>
		/// Implements FactType for IFactConstraint and ExternalFactConstraint
		/// by deferring to generated FactTypeCollection accessor
		/// </summary>
		protected sealed override FactType FactType
		{
			get
			{
				return FactTypeCollection;
			}
		}
		/// <summary>
		/// Implements Constraint for IFactConstraint and ExternalFactConstraint
		/// by deferring to generated SingleColumnExternalConstraintCollection accessor
		/// </summary>
		protected sealed override IConstraint Constraint
		{
			get
			{
				return SingleColumnExternalConstraintCollection;
			}
		}
	}
	#endregion // FactConstraint classes
	#region ConstraintRoleSequence classes
	public partial class ConstraintRoleSequence
	{
		#region ConstraintRoleSequence Specific
		/// <summary>
		/// Get the constraint that owns this role sequence
		/// </summary>
		public abstract IConstraint Constraint { get;}
		#endregion // ConstraintRoleSequence Specific
	}
	public partial class InternalConstraint
	{
		#region ConstraintRoleSequence overrides
		/// <summary>
		/// An internal constraint is its own role sequence.
		/// Return this.
		/// </summary>
		public override IConstraint Constraint
		{
			get
			{
				return this;
			}
		}
		#endregion // ConstraintRoleSequence overrides
	}
	public partial class MultiColumnExternalConstraintRoleSequence
	{
		#region ConstraintRoleSequence overrides
		/// <summary>
		/// Get the external constraint that owns this role sequence
		/// </summary>
		public override IConstraint Constraint
		{
			get
			{
				return ExternalConstraint;
			}
		}
		#endregion // ConstraintRoleSequence overrides
	}
	public partial class SingleColumnExternalConstraint
	{
		#region ConstraintRoleSequence overrides
		/// <summary>
		/// A single column external constraint is its own role sequence.
		/// Return this.
		/// </summary>
		public override IConstraint Constraint
		{
			get
			{
				return this;
			}
		}
		#endregion // ConstraintRoleSequence overrides
	}
	#endregion // ConstraintRoleSequence classes
	#region InternalUniquenessConstraint class
	public partial class InternalUniquenessConstraint : IModelErrorOwner
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
				return IsPreferred ? false : !TestAllowPreferred(null, false);
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
				RoleMoveableCollection constraintRoles = RoleCollection;
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
						if ((rolePlayer != null) &&
							(forType == null || object.ReferenceEquals(forType, rolePlayer)) &&
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
						Role role = constraint.RoleCollection[0];
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
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Returns the error associated with the constraint.
		/// </summary>
		[CLSCompliant(false)]
		protected IEnumerable<ModelError> ErrorCollection
		{
			get
			{
				NMinusOneError nMinusOneError = NMinusOneError;
				if (nMinusOneError != null)
				{
					yield return nMinusOneError;
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
		protected void ValidateErrors(INotifyElementAdded notifyAdded)
		{
			VerifyNMinusOneForRule(notifyAdded);
		}
		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateErrors(notifyAdded);
		}
		#endregion // IModelErrorOwner Implementation
		#region NMinusOneError Validation
		/// <summary>
		/// Add, remove, and otherwise validate the current NMinusOne errors
		/// </summary>
		/// <param name="notifyAdded">If not null, this is being called during
		/// load when rules are not in place. Any elements that are added
		/// must be notified back to the caller.</param>
		private void VerifyNMinusOneForRule(INotifyElementAdded notifyAdded)
		{
			if (!IsRemoved)
			{
				FactType theFactType = FactType;
				if (theFactType != null && !theFactType.IsRemoved)
				{
					if (null != theFactType.InternalUniquenessConstraintRequiredError)
					{
						if (NMinusOneError != null)
						{
							NMinusOneError.Remove();
						}
					}
					else
					{
						bool hasError = RoleCollection.Count < theFactType.RoleCollection.Count - 1;

						NMinusOneError noNMinusOneError = NMinusOneError;
						if (hasError)
						{
							//Adding the Error to the model
							if (noNMinusOneError == null)
							{
								noNMinusOneError = NMinusOneError.CreateNMinusOneError(Store);
								noNMinusOneError.Model = theFactType.Model;
								noNMinusOneError.Constraint = this;
								noNMinusOneError.GenerateErrorText();
								if (notifyAdded != null)
								{
									notifyAdded.ElementAdded(noNMinusOneError, true);
								}
							}
							else
							{
								//Error is already present, but the number of facts in the sequence could have changed, so we
								//need to regenerate the error text so the correct number of roles is present.
								noNMinusOneError.GenerateErrorText();
							}
						}
						else if (noNMinusOneError != null)
						{
							//Removing error
							noNMinusOneError.Remove();
						}
					}
				}
			}
		}
		/// <summary>
		/// Only validates NMinusOneError
		/// Checks when Internal constraint is added
		/// </summary>
		[RuleOn(typeof(FactTypeHasInternalConstraint), FireTime = TimeToFire.LocalCommit)]
		private class NMinusOneAddRuleModelValidation : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasInternalConstraint link = e.ModelElement as FactTypeHasInternalConstraint;
				InternalUniquenessConstraint constraint = link.InternalConstraintCollection as InternalUniquenessConstraint;
				if (constraint != null)
				{
					constraint.VerifyNMinusOneForRule(null);
				}
			}
		}
		/// <summary>
		/// Only validates NMinusOneError
		/// Checks when Internal constraint is removed
		/// </summary>
		[RuleOn(typeof(FactTypeHasInternalConstraint), FireTime = TimeToFire.LocalCommit)]
		private class NMinusOneRemoveRuleModelValidation : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				FactTypeHasInternalConstraint link = e.ModelElement as FactTypeHasInternalConstraint;
				InternalUniquenessConstraint constraint = link.InternalConstraintCollection as InternalUniquenessConstraint;
				if (constraint != null)
				{
					constraint.VerifyNMinusOneForRule(null);
				}
			}
		}
		/// <summary>
		/// Only validates NMinusOneError
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole), FireTime = TimeToFire.LocalCommit)]
		private class NMinusOneAddRuleModelConstraintAddValidation : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				InternalUniquenessConstraint constraint = link.ConstraintRoleSequenceCollection as InternalUniquenessConstraint;
				if (constraint != null)
				{
					constraint.VerifyNMinusOneForRule(null);
				}
			}
		}
		/// <summary>
		/// Only validates NMinusOneError
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole), FireTime = TimeToFire.LocalCommit)]
		private class NMinusOneRemoveRuleModelConstraintRemoveValidation : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				InternalUniquenessConstraint constraint = link.ConstraintRoleSequenceCollection as InternalUniquenessConstraint;
				if (constraint != null)
				{
					constraint.VerifyNMinusOneForRule(null);
				}
			}
		}
		/// <summary>
		/// Only validates NMinusOneError
		/// Used for Adding roles to the role sequence check
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole), FireTime = TimeToFire.LocalCommit)]
		private class NMinusOneAddRuleModelFactAddValidation : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
				FactType fact = link.FactType;
				if (fact != null)
				{
					foreach (InternalUniquenessConstraint constraint in fact.GetInternalConstraints<InternalUniquenessConstraint>())
					{
						constraint.VerifyNMinusOneForRule(null);
					}
				}
			}
		}
		/// <summary>
		/// Only validates NMinusOneError
		/// Used for Removing roles to the role sequence check
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole), FireTime = TimeToFire.LocalCommit)]
		private class NMinusOneRemoveRuleModelFactRemoveValidation : RemoveRule
		{
			public override void ElementRemoved(ElementRemovedEventArgs e)
			{
				FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
				FactType fact = link.FactType;
				if (fact != null && !fact.IsRemoved)
				{
					foreach (InternalUniquenessConstraint constraint in fact.GetInternalConstraints<InternalUniquenessConstraint>())
					{
						if (!constraint.IsRemoved)
						{
							constraint.VerifyNMinusOneForRule(null);
						}
					}
				}
			}
		}
		#endregion // NMinusOneError Validation
	}
	#endregion // InternalUniquenessConstraint class
	#region EqualityConstraint class
	public partial class EqualityConstraint : IModelErrorOwner
	{
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
		protected new IEnumerable<ModelError> ErrorCollection
		{
			get
			{
				foreach (ModelError baseError in base.ErrorCollection)
				{
					yield return baseError;
				}
				EqualityIsImpliedByMandatoryError equalityImplied;
				if (null != (equalityImplied = EqualityIsImpliedByMandatoryError))
				{
					yield return equalityImplied;
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
		protected new void ValidateErrors(INotifyElementAdded notifyAdded)
		{
			base.ValidateErrors(notifyAdded);
			VerifyNoRolesAreMandatory(notifyAdded, false);
		}
		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateErrors(notifyAdded);
		}
		#endregion // IModelErrorOwner Implementation
		#region Error synchronization rules
		/// <summary>
		/// Verifies that no mandatory roles are connected to the equality constraint.
		/// </summary>
		/// <param name="notifyAdded">If not null, this is being called during
		/// load when rules are not in place. Any elements that are added
		/// must be notified back to the caller.</param>
		/// <param name="forceError">We know we need the error, don't check</param>
		private void VerifyNoRolesAreMandatory(INotifyElementAdded notifyAdded, bool forceError)
		{
			if (!IsRemoved)
			{
				bool noError = true;
				EqualityIsImpliedByMandatoryError impliedEqualityError;
				if (!forceError)
				{
					MultiColumnExternalConstraintRoleSequenceMoveableCollection sequences = RoleSequenceCollection;
					int roleSequenceCount = sequences.Count;

					for (int i = 0; i < roleSequenceCount && noError; ++i)
					{
						RoleMoveableCollection roleCollection = sequences[i].RoleCollection;
						int roleCount = roleCollection.Count;
						for (int j = 0; j < roleCount; ++j)
						{
							Role currentRole = roleCollection[j];
							ConstraintRoleSequenceMoveableCollection roleConstraints = currentRole.ConstraintRoleSequenceCollection;
							int constraintCount = roleConstraints.Count;
							for (int counter = 0; counter < constraintCount; ++counter)
							{
								IConstraint currentConstraint = roleConstraints[counter].Constraint;
								if (currentConstraint.ConstraintType == ConstraintType.SimpleMandatory)
								{
									SimpleMandatoryConstraint mandatory = currentConstraint as SimpleMandatoryConstraint;
									if (!mandatory.IsRemoving)
									{
										noError = false;
									}
									break; // There will only be one simple mandatory constraint on any given role
								}
							}
						}
					}
				}
				if (forceError || !noError)
				{
					if (null == EqualityIsImpliedByMandatoryError)
					{
						impliedEqualityError = EqualityIsImpliedByMandatoryError.CreateEqualityIsImpliedByMandatoryError(Store);
						impliedEqualityError.Model = Model;
						impliedEqualityError.EqualityConstraint = this;
						impliedEqualityError.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(impliedEqualityError, true);
						}
					}
				}
				else if (noError && null != (impliedEqualityError = EqualityIsImpliedByMandatoryError))
				{
					impliedEqualityError.Remove();
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="mandatoryContraint">The mandatory constraint being added or removed</param>
		/// <param name="forAdd">This is called from an add rule, so we will always need the error</param>
		private static void VerifyMandatoryHasNoEqualityConstraints(SimpleMandatoryConstraint mandatoryContraint, bool forAdd)
		{
			RoleMoveableCollection roles = mandatoryContraint.RoleCollection;
			if (roles.Count != 0)
			{
				Role currentRole = roles[0];
				ConstraintRoleSequenceMoveableCollection constraints = currentRole.ConstraintRoleSequenceCollection;
				int constraintCount = constraints.Count;
				for (int i = 0; i < constraintCount; ++i)
				{
					IConstraint currentConstraint = constraints[i].Constraint;
					if (currentConstraint.ConstraintType == ConstraintType.Equality)
					{
						(currentConstraint as EqualityConstraint).VerifyNoRolesAreMandatory(null, forAdd);
					}
				}
			}
		}
		#endregion //Error synchronization rules
		#region ConstraintRoleSequenceHasRoleClasses
		/// <summary>
		/// Check to see if mandatory constraints are still implied by equality when removing a mandatory
		/// constraint.
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole), FireTime = TimeToFire.Inline)]
		private class ConstraintRoleSequenceHasRoleRemoved : RemovingRule
		{
			/// <summary>
			/// Runs when roleset element is removing. It calls to verify that no mandatory roles are 
			/// connected to the EqualityConstraint.
			/// </summary>
			public override void ElementRemoving(ElementRemovingEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				ConstraintRoleSequence roleSequences = link.ConstraintRoleSequenceCollection;
				IConstraint constraint = roleSequences.Constraint;
				switch (constraint.ConstraintType)
				{
					case ConstraintType.Equality:
						EqualityConstraint equality = constraint as EqualityConstraint;
						if (!equality.IsRemoved)
						{
							equality.VerifyNoRolesAreMandatory(null, false);
						}
						break;
					case ConstraintType.SimpleMandatory:
						//Find my my equality constraint and check to see if my error message can be
						//removed.
						SimpleMandatoryConstraint mandatory = constraint as SimpleMandatoryConstraint;
						VerifyMandatoryHasNoEqualityConstraints(mandatory, false);
						break;
				}
			}
		}
		/// <summary>
		/// Check to see if mandatory constraints are implied by equality when adding an equality
		/// constraint.
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole), FireTime = TimeToFire.LocalCommit)]
		private class ConstraintRoleSequenceHasRoleAdded : AddRule
		{
			/// <summary>
			/// Runs when roleset element is being added. It calls to verify that no mandatory roles are 
			/// connected to the EqualityConstraint.
			/// </summary>
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				ConstraintRoleSequence roleSequences = link.ConstraintRoleSequenceCollection;
				IConstraint constraint = roleSequences.Constraint;
				switch (constraint.ConstraintType)
				{
					case ConstraintType.Equality:
						EqualityConstraint equality = constraint as EqualityConstraint;
						if (!equality.IsRemoved)
						{
							equality.VerifyNoRolesAreMandatory(null, false);
						}
						break;
					case ConstraintType.SimpleMandatory:
						//Find my my equality constraint and check to see if my error message can be
						//removed.
						SimpleMandatoryConstraint mandatory = constraint as SimpleMandatoryConstraint;
						VerifyMandatoryHasNoEqualityConstraints(mandatory, true);
						break;
				}
			}
		}
		#endregion
	}
	#endregion // EqualityConstraint class
	#region EntityTypeHasPreferredIdentifier pattern enforcement
	public partial class EntityTypeHasPreferredIdentifier
	{
		#region Remove testing for preferred identifier
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
				ConstraintRoleSequence constraint = PreferredIdentifier;
				if (!constraint.IsRemoving && !constraint.IsRemoved)
				{
					ObjectType forType = PreferredIdentifierFor;
					if (!forType.IsRemoving && !forType.IsRemoved)
					{
						InternalUniquenessConstraint iuc;
						//ExternalUniquenessConstraint euc;
						if (null != (iuc = constraint as InternalUniquenessConstraint))
						{
							RoleMoveableCollection roles;
							Role constraintRole;
							FactType factType;
							if (1 == (roles = iuc.RoleCollection).Count &&
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
									foreach (ConstraintRoleSequence testRoleSequence in oppositeRole.ConstraintRoleSequenceCollection)
									{
										IConstraint testConstraint = testRoleSequence.Constraint;
										if (testConstraint != null && !(testConstraint as ModelElement).IsRemoving)
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
													if (testRoleSequence.RoleCollection.Count == 1)
													{
														haveOppositeUniqueness = true;
													}
													break;
												case ConstraintType.SimpleMandatory:
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
						//else if (null != (euc = constraint as ExternalUniquenessConstraint))
						//{
						//	// UNDONE: Preferred external uniqueness. Requires path information.
						//}
					}
				}
				if (remove)
				{
					Remove();
				}
			}
		}
		#endregion // Remove testing for preferred identifier
		#region TestRemovePreferredIdentifierRemovingRule class
		/// <summary>
		/// A rule to determine if a mandatory condition for
		/// a preferred identifier link has been eliminated.
		/// Remove the preferred identifier if this happens.
		/// </summary>
		[RuleOn(typeof(ObjectTypePlaysRole)), RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		private class TestRemovePreferredIdentifierRemovingRule : RemovingRule
		{
			/// <summary>
			/// See if a preferred identifier is still valid
			/// </summary>
			/// <param name="e"></param>
			public override void ElementRemoving(ElementRemovingEventArgs e)
			{
				ModelElement element = e.ModelElement;
				ObjectTypePlaysRole rolePlayerLink;
				ConstraintRoleSequenceHasRole roleConstraintLink;
				ObjectType rolePlayer = null;
				if (null != (rolePlayerLink = element as ObjectTypePlaysRole))
				{
					rolePlayer = rolePlayerLink.RolePlayer;
				}
				else if (null != (roleConstraintLink = element as ConstraintRoleSequenceHasRole))
				{
					InternalConstraint internalRoleSequence;
					IConstraint constraint;
					if (null != (internalRoleSequence = roleConstraintLink.ConstraintRoleSequenceCollection as InternalConstraint) &&
						null != (constraint = internalRoleSequence.Constraint))
					{
						switch (constraint.ConstraintType)
						{
							case ConstraintType.InternalUniqueness:
							case ConstraintType.SimpleMandatory:
								Role role = roleConstraintLink.RoleCollection;
								if (role != null)
								{
									rolePlayer = role.RolePlayer;
								}
								break;
						}
					}
				}
				if (rolePlayer != null && !rolePlayer.IsRemoving)
				{
					IList links = rolePlayer.GetElementLinks(EntityTypeHasPreferredIdentifier.PreferredIdentifierForMetaRoleGuid);
					// Don't for each, the iterator doesn't like it when you remove elements
					int linksCount = links.Count;
					Debug.Assert(linksCount <= 1); // Should be a 1-1 relationship
					for (int i = linksCount - 1; i >= 0; --i)
					{
						EntityTypeHasPreferredIdentifier identifierLink = links[i] as EntityTypeHasPreferredIdentifier;
						identifierLink.TestRemovePreferredIdentifier();
					}
				}
			}
		}
		#endregion // TestRemovePreferredIdentifierRemovingRule class
		#region TestRemovePreferredIdentifierAddRule class
		/// <summary>
		/// A rule to determine if a mandatory condition for
		/// a preferred identifier link has been eliminated by
		/// adding an element.
		/// Remove the identifier if this happens.
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole)), RuleOn(typeof(ConstraintRoleSequenceHasRole))]
		private class TestRemovePreferredIdentifierAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ModelElement element = e.ModelElement;
				FactTypeHasRole roleLink;
				ConstraintRoleSequenceHasRole constraintLink;
				if (null != (roleLink = element as FactTypeHasRole))
				{
					FactType fact = roleLink.FactType;
					foreach (InternalConstraint constraint in fact.GetInternalConstraints(ConstraintType.InternalUniqueness))
					{
						constraint.PreferredIdentifierFor = null;
					}
				}
				else if (null != (constraintLink = element as ConstraintRoleSequenceHasRole))
				{
					ConstraintRoleSequence sequence = constraintLink.ConstraintRoleSequenceCollection;
					IConstraint constraint = sequence.Constraint;
					if (constraint != null && constraint.ConstraintType == ConstraintType.InternalUniqueness)
					{
						// A preferred identifier on an internal uniqueness constraint reqduires
						// the constraint to have one role only. If we already have a preferred
						// identifier on this role, then we must have one already, so adding an
						// additional role is bad.
						sequence.PreferredIdentifierFor = null;
					}
				}
			}
		}
		#endregion // TestRemovePreferredIdentifierAddRule class
		#region PreferredIdentifierAddedRule class
		/// <summary>
		/// Verify that all preconditions hold for adding a primary
		/// identifier and extend modifiable conditions as needed.
		/// </summary>
		[RuleOn(typeof(EntityTypeHasPreferredIdentifier))]
		private class PreferredIdentifierAddedRule : AddRule
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

				IConstraint constraint = link.PreferredIdentifier as IConstraint;
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
							Role role = iuc.RoleCollection[0];
							Role oppositeRole = null;
							FactType factType = role.FactType;
							foreach (Role factRole in factType.RoleCollection)
							{
								if (!object.ReferenceEquals(role, factRole))
								{
									oppositeRole = factRole;
									break;
								}
							}
							oppositeRole.IsMandatory = true; // Make sure it is mandatory
							bool needOppositeConstraint = true;
							foreach (ConstraintRoleSequence roleSequence in oppositeRole.ConstraintRoleSequenceCollection)
							{
								if (roleSequence.Constraint.ConstraintType == ConstraintType.InternalUniqueness &&
									roleSequence.RoleCollection.Count == 1)
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
								oppositeIuc.RoleCollection.Add(oppositeRole); // Automatically sets FactType, setting it again will remove and delete the new constraint
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
	}
	#endregion // EntityTypeHasPreferredIdentifier pattern enforcement
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
	#region DisjunctiveMandatoryConstraint class
	public partial class DisjunctiveMandatoryConstraint : IModelErrorOwner
	{
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
		protected new IEnumerable<ModelError> ErrorCollection
		{
			get
			{
				foreach (ModelError baseError in base.ErrorCollection)
				{
					yield return baseError;
				}
				SimpleMandatoryImpliesDisjunctiveMandatoryError impliedDisjunctive = ImpliedBySimpleMandatoryError;
				if (impliedDisjunctive != null)
				{
					yield return impliedDisjunctive;
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
		protected new void ValidateErrors(INotifyElementAdded notifyAdded)
		{
			base.ValidateErrors(notifyAdded);
			ValidateImpliedDisjunctiveError(notifyAdded, false);
		}
		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateErrors(notifyAdded);
		}
		#endregion // IModelErrorOwner Implementation
		#region Error Rules
		/// <summary>
		/// Verify that we the disjunctive mandatory constraint is not attached to a role that
		/// also has a simple mandatory
		/// </summary>
		/// <param name="notifyAdded">Set during deserialization</param>
		/// <param name="forceError">Set to true if we already know that the error condition will be there,
		/// such as when a simplemandatory constraint is added</param>
		private void ValidateImpliedDisjunctiveError(INotifyElementAdded notifyAdded, bool forceError)
		{
			if (!IsRemoved)
			{
				bool hasError = forceError;
				Store theStore = Store;
				if (!hasError)
				{
					RoleMoveableCollection roles = RoleCollection;
					int roleCount = roles.Count;
					for (int i = 0; !hasError && i < roleCount; i++)
					{
						Role role = roles[i];
						if (!role.IsRemoving)
						{
							ConstraintRoleSequenceMoveableCollection attachedSequences = role.ConstraintRoleSequenceCollection;
							int roleSequenceCount = attachedSequences.Count;
							for (int j = 0; j < roleSequenceCount; ++j)
							{
								ConstraintRoleSequence roleSequence = attachedSequences[j];
								if (!roleSequence.IsRemoving)
								{
									IConstraint constraint = roleSequence.Constraint;
									if (constraint.ConstraintType == ConstraintType.SimpleMandatory)
									{
										hasError = true;
										break;
									}
								}
							}
						}
					}
				}
				SimpleMandatoryImpliesDisjunctiveMandatoryError noImpliedMandatoryError = ImpliedBySimpleMandatoryError;
				if (hasError)
				{
					if (noImpliedMandatoryError == null)
					{
						noImpliedMandatoryError = SimpleMandatoryImpliesDisjunctiveMandatoryError.CreateSimpleMandatoryImpliesDisjunctiveMandatoryError(theStore);
						noImpliedMandatoryError.Model = Model;
						noImpliedMandatoryError.DisjunctiveMandatoryConstraint = this;
						noImpliedMandatoryError.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(noImpliedMandatoryError, true);
						}
					}
				}
				else if (noImpliedMandatoryError != null)
				{
					noImpliedMandatoryError.Remove();
				}
			}
		}
		/// <summary>
		/// Verify that the SimpleMandatoryImpliesDisjunctiveMandatoryError is present/not present when
		/// a simple mandatory constraint is added to a role that also participates in a DisjunctiveMandatoryConstraint
		/// </summary>
		/// <param name="mandatoryContraint">The SimpleMandatoryConstraint to test</param>
		/// <param name="forAdd">True if the constraint was just added, false otherwise</param>
		private static void VerifyMandatoryHasNoDisjunctivMandatoryConstraints(SimpleMandatoryConstraint mandatoryContraint, bool forAdd)
		{
			RoleMoveableCollection roles = mandatoryContraint.RoleCollection;
			if (roles.Count != 0)
			{
				Role currentRole = roles[0];
				ConstraintRoleSequenceMoveableCollection constraints = currentRole.ConstraintRoleSequenceCollection;
				int constraintCount = constraints.Count;
				IConstraint currentConstraint;
				for (int i = 0; i < constraintCount; ++i)
				{
					currentConstraint = constraints[i].Constraint;
					if (currentConstraint.ConstraintType == ConstraintType.DisjunctiveMandatory)
					{
						(currentConstraint as DisjunctiveMandatoryConstraint).ValidateImpliedDisjunctiveError(null, forAdd);
					}
				}
			}
		}
		#endregion //Error Rules
		#region VerifyImpliedDisjunctiveMandatoryRole Add/Remove Methods
		[RuleOn(typeof(ConstraintRoleSequenceHasRole), FireTime = TimeToFire.LocalCommit)]
		private class VerifyImpliedDisjunctiveMandatoryRoleAdd : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				ConstraintRoleSequence roleSequences = link.ConstraintRoleSequenceCollection;
				IConstraint constraint = roleSequences.Constraint;
				switch (constraint.ConstraintType)
				{
					case ConstraintType.SimpleMandatory:
						SimpleMandatoryConstraint simpleMandatory = constraint as SimpleMandatoryConstraint;
						VerifyMandatoryHasNoDisjunctivMandatoryConstraints(simpleMandatory, true);
						break;
					case ConstraintType.DisjunctiveMandatory:
						DisjunctiveMandatoryConstraint mandatory = constraint as DisjunctiveMandatoryConstraint;
						mandatory.ValidateImpliedDisjunctiveError(null, false);
						break;

				}
			}
		}
		[RuleOn(typeof(ConstraintRoleSequenceHasRole), FireTime = TimeToFire.Inline)]
		private class VerifyImpliedDisjunctiveMandatoryRoleRemoved : RemovingRule
		{
			/// <summary>
			/// Runs when roleset element is removing. It calls to verify that no mandatory roles are connected to the EqualityConstraint.
			/// </summary>
			public override void ElementRemoving(ElementRemovingEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				ConstraintRoleSequence roleSequences = link.ConstraintRoleSequenceCollection;
				IConstraint constraint = roleSequences.Constraint;
				switch (constraint.ConstraintType)
				{
					case ConstraintType.SimpleMandatory:
						SimpleMandatoryConstraint simpleMandatory = constraint as SimpleMandatoryConstraint;
						VerifyMandatoryHasNoDisjunctivMandatoryConstraints(simpleMandatory, false);
						break;
					case ConstraintType.DisjunctiveMandatory:
						DisjunctiveMandatoryConstraint mandatory = constraint as DisjunctiveMandatoryConstraint;
						mandatory.ValidateImpliedDisjunctiveError(null, false);
						break;

				}
			}
		}
		#endregion //VerifyImpliedDisjunctiveMandatoryRole Add/Remove Methods
	}
	#endregion // DisjunctiveMandatoryConstraint class
	#region FrequencyConstraint class
	public partial class FrequencyConstraint : IModelErrorOwner
	{
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Returns the error associated with the constraint.
		/// </summary>
		[CLSCompliant(false)]
		protected new IEnumerable<ModelError> ErrorCollection
		{
			get
			{
				foreach (ModelError error in base.ErrorCollection)
				{
					yield return error;
				}
				FrequencyConstraintMinMaxError minMaxError = FrequencyConstraintMinMaxError;
				if (minMaxError != null)
				{
					yield return minMaxError;
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
			base.ValidateErrors(notifyAdded);
			VerifyMinMaxRule(notifyAdded);
		}
		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			ValidateErrors(notifyAdded);
		}
		#endregion //IModelErrorOwner Implementation
		#region MinMaxError Validation
		/// <summary>
		/// Add, remove, and otherwise validate the current NMinusOne errors
		/// </summary>
		/// <param name="notifyAdded">If not null, this is being called during
		/// load when rules are not in place. Any elements that are added
		/// must be notified back to the caller.</param>
		private void VerifyMinMaxRule(INotifyElementAdded notifyAdded)
		{
			if (IsRemoved)
			{
				return;
			}

			FrequencyConstraintMinMaxError minMaxError = FrequencyConstraintMinMaxError;
			int min = MinFrequency;
			int max = MaxFrequency;
			if (max > 0 && min > max)
			{
				//Adding the Error to the model
				if (minMaxError == null)
				{
					minMaxError = FrequencyConstraintMinMaxError.CreateFrequencyConstraintMinMaxError(Store);
					minMaxError.FrequencyConstraint = this;
					minMaxError.Model = Model;
					minMaxError.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(minMaxError, true);
					}
				}
			}
			else if (minMaxError != null)
			{
				minMaxError.Remove();
			}
		}
		#endregion // MinMaxError Validation
		#region FrequencyConstraintMinMaxRule class
		[RuleOn(typeof(FrequencyConstraint), FireTime = TimeToFire.LocalCommit)]
		private class FrequencyConstraintMinMaxRule : ChangeRule
		{
			public override void ElementAttributeChanged(ElementAttributeChangedEventArgs e)
			{
				Guid attributeId = e.MetaAttribute.Id;
				if (attributeId == FrequencyConstraint.MinFrequencyMetaAttributeGuid ||
					attributeId == FrequencyConstraint.MaxFrequencyMetaAttributeGuid)
				{
					FrequencyConstraint fc = e.ModelElement as FrequencyConstraint;
					if (!fc.IsRemoved)
					{
						fc.VerifyMinMaxRule(null);
					}
				}
			}
		}
		#endregion // FrequencyConstraintMinMaxRule class
		#region FrequencyConstraintMinMaxAddRule class
		[RuleOn(typeof(FrequencyConstraint), FireTime = TimeToFire.LocalCommit)]
		private class FrequencyConstraintMinMaxAddRule : AddRule
		{

			public override void ElementAdded(ElementAddedEventArgs e)
			{

				FrequencyConstraint fc = e.ModelElement as FrequencyConstraint;
				if (!fc.IsRemoved)
				{
					fc.VerifyMinMaxRule(null);
				}
				
			}

		}
		#endregion // FrequencyConstraintMinMaxAddRule class

	}
	#endregion // FrequencyConstraint class
	#region ModelError classes
	public partial class TooManyRoleSequencesError : IRepresentModelElements
	{
		#region Base overrides
		/// <summary>
		/// Generate and set text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			MultiColumnExternalConstraint parent = Constraint;
			string parentName = (parent != null) ? parent.Name : "";
			string currentText = Name;
			string newText = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorConstraintHasTooManyRoleSequencesText, parentName);
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
	public partial class TooFewRoleSequencesError : IRepresentModelElements
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			MultiColumnExternalConstraint parent = this.Constraint;
			string parentName = (parent != null) ? parent.Name : "";
			string currentText = Name;
			string newText = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorConstraintHasTooFewRoleSequencesText, parentName);
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
	public partial class ExternalConstraintRoleSequenceArityMismatchError : IRepresentModelElements
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			MultiColumnExternalConstraint parent = this.Constraint;
			string parentName = (parent != null) ? parent.Name : "";
			string currentText = Name;
			string newText = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorConstraintExternalConstraintArityMismatch, parentName);
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

	public partial class CompatibleRolePlayerTypeError : IRepresentModelElements
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			MultiColumnExternalConstraint multiColumnParent = MultiColumnExternalConstraint;
			NamedElement namedParent;
			bool useColumn;
			if (multiColumnParent != null)
			{
				namedParent = multiColumnParent;
				useColumn = multiColumnParent.RoleSequenceCollection[0].RoleCollection.Count > 1;
			}
			else
			{
				namedParent = SingleColumnExternalConstraint;
				useColumn = false;
			}
			Debug.Assert(namedParent != null, "Parent must be single column or multi column");
			string parentName = (namedParent != null) ? namedParent.Name : "";
			string modelName = this.Model.Name;
			string currentText = Name;
			string newText = useColumn ?
				string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorMultiColumnConstraintCompatibleRolePlayerTypeError, parentName, modelName, (Column + 1).ToString(CultureInfo.InvariantCulture)) :
				string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorSingleColumnConstraintCompatibleRolePlayerTypeError, parentName, modelName);
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
			return new ModelElement[] { ParentConstraint };
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion // IRepresentModelElements Implementation
		#region Accessor Properties
		/// <summary>
		/// Return either the single column or multi column
		/// constraint associated with this error.
		/// </summary>
		public NamedElement ParentConstraint
		{
			get
			{
				NamedElement retVal = MultiColumnExternalConstraint;
				return (retVal != null) ? retVal : SingleColumnExternalConstraint;
			}
		}
		#endregion // Accessor Properties
	}
	public partial class FrequencyConstraintMinMaxError : IRepresentModelElements
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			FrequencyConstraint parent = this.FrequencyConstraint;
			string parentName = (parent != null) ? parent.Name : "";
			string currentText = Name;
			string newText = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorFrequencyConstraintMinMaxError, parentName, Model.Name);
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
			return new ModelElement[] { FrequencyConstraint };
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion // IRepresentModelElements Implementation
	
	}
	public partial class EqualityIsImpliedByMandatoryError : IRepresentModelElements
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			EqualityConstraint parent = this.EqualityConstraint;
			string parentName = (parent != null) ? parent.Name : "";
			string currentText = Name;
			string newText = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorExternalEqualityIsImpliedByMandatoryError, parentName, this.Model.Name);
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
				return RegenerateErrorTextEvents.ModelNameChange | RegenerateErrorTextEvents.OwnerNameChange;
			}
		}
		#endregion //Base overrides
		#region IRepresentModelElements Implementation
		/// <summary>
		/// Implements IRepresentModelElements.GetRepresentedElements
		/// </summary>
		/// <returns></returns>
		protected ModelElement[] GetRepresentedElements()
		{
			return new ModelElement[] { EqualityConstraint };
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion //IRepresentModelElements Implementation
	}
	public partial class SimpleMandatoryImpliesDisjunctiveMandatoryError : IRepresentModelElements
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			SingleColumnExternalConstraint parent = this.DisjunctiveMandatoryConstraint;
			string parentName = (parent != null) ? parent.Name : "";
			string modelName = this.Model.Name;
			string currentText = Name;
			string newText = string.Format(CultureInfo.InvariantCulture, ResourceStrings.SimpleMandatoryImpliesDisjunctiveMandatoryError, parentName, modelName);
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
			return new ModelElement[] { DisjunctiveMandatoryConstraint };
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
	#region RoleSequenceStyles enum
	/// <summary>
	/// Flags describing the style of role sequences required
	/// by each type of constraint
	/// </summary>
	[Flags]
	[CLSCompliant(true)]
	public enum RoleSequenceStyles
	{
		/// <summary>
		/// Constraint uses a single role sequence
		/// </summary>
		OneRoleSequence = 1,
		/// <summary>
		/// Constraint uses exactly two role sequences
		/// </summary>
		TwoRoleSequences = 2,
		/// <summary>
		/// Constraint uses >=2 role sequences
		/// </summary>
		MultipleRowSequences = 4,
		/// <summary>
		/// A mask to extract the sequence multiplicity values
		/// </summary>
		SequenceMultiplicityMask = OneRoleSequence | TwoRoleSequences | MultipleRowSequences,
		/// <summary>
		/// Each role sequence contains exactly one role
		/// </summary>
		OneRolePerSequence = 8,
		/// <summary>
		/// Each role sequence contains exactly two roles
		/// </summary>
		TwoRolesPerSequence = 0x10,
		/// <summary>
		/// Each role sequence can contain >=1 roles
		/// </summary>
		MultipleRolesPerSequence = 0x20,
		/// <summary>
		/// The role sequence must contain n or n-1 roles. Applicable
		/// to OneRoleSequence constraints only
		/// </summary>
		AtLeastCountMinusOneRolesPerSequence = 0x40,
		/// <summary>
		/// A mask to extract the row multiplicity values
		/// </summary>
		RoleMultiplicityMask = OneRolePerSequence | TwoRolesPerSequence | MultipleRolesPerSequence | AtLeastCountMinusOneRolesPerSequence,
		/// <summary>
		/// The order of the role sequences is significant
		/// </summary>
		OrderedRoleSequences = 0x80,
		/// <summary>
		/// Each of the columns must be type compatible
		/// </summary>
		CompatibleColumns = 0x100,
	}
	#endregion // RoleSequenceStyles enum
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
		SimpleMandatory,
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
		/// An disjunctive mandatory constraint. Applied to
		/// single-role sets of compatible roles from multiple fact types.
		/// </summary>
		DisjunctiveMandatory,
		/// <summary>
		/// An external subset constraint. Applied to 2
		/// sets of compatible roles.
		/// </summary>
		Subset,
	}	
	#endregion // ConstraintType enum
	#region ConstraintStorageStyle enum
	/// <summary>
	/// A list of possible ways to store a constraint's role sequences.
	/// </summary>
	[CLSCompliant(true)]
	public enum ConstraintStorageStyle
	{
		/// <summary>
		/// The constraint is stored as a single role sequence.
		/// </summary>
		InternalConstraint,
		/// <summary>
		/// The constraint is stored as a single role sequence, but with the
		/// rule that all roles must be compatible.  This is because the contraint
		/// is technically a single column with many rows at the conceptual level,
		/// but it's easier to store as a single row of many columns.
		/// </summary>
		SingleColumnExternalConstraint,
		/// <summary>
		/// The contraint is stored as a sequence of role sequences.  Each role
		/// in a given role sequence must be compatible with roles in the same
		/// position of all other role sequences.
		/// </summary>
		MultiColumnExternalConstraint,
	}
	#endregion // ConstraintStorageStyle enum
	#region ConstraintModality enum
	/// <summary>
	/// A list of Constraint Modalities.
	/// </summary>
	[CLSCompliant(true)]
	public enum ConstraintModality
	{
		/// <summary>
		/// The constraint must hold
		/// </summary>
		Alethic,
		/// <summary>
		/// The constraint should hold
		/// </summary>
		Deontic
	}
	#endregion
	#region ConstraintType and RoleSequenceStyles implementation for all constraints
	public partial class SimpleMandatoryConstraint : IConstraint
	{
		#region IConstraint Implementation
		/// <summary>
		/// Implements IConstraint.ConstraintType. Returns ConstraintType.SimpleMandatory.
		/// </summary>
		protected static ConstraintType ConstraintType
		{
			get
			{
				return ConstraintType.SimpleMandatory;
			}
		}
		ConstraintType IConstraint.ConstraintType
		{
			get
			{
				return ConstraintType;
			}
		}
		/// <summary>
		/// Implements IConstraint.RoleSequenceStyles. Returns {OneRoleSequence, OneRolePerSequence}.
		/// </summary>
		protected static RoleSequenceStyles RoleSequenceStyles
		{
			get
			{
				return RoleSequenceStyles.OneRoleSequence | RoleSequenceStyles.OneRolePerSequence;
			}
		}
		RoleSequenceStyles IConstraint.RoleSequenceStyles
		{
			get
			{
				return RoleSequenceStyles;
			}
		}
		#endregion // IConstraint Implementation
	}
	public partial class InternalUniquenessConstraint : IConstraint
	{
		#region IConstraint Implementation
		/// <summary>
		/// Implements IConstraint.ConstraintType. Returns ConstraintType.InternalUniqueness.
		/// </summary>
		protected static ConstraintType ConstraintType
		{
			get
			{
				return ConstraintType.InternalUniqueness;
			}
		}
		ConstraintType IConstraint.ConstraintType
		{
			get
			{
				return ConstraintType;
			}
		}
		/// <summary>
		/// Implements IConstraint.RoleSequenceStyles. Returns {OneRoleSequence, AtLeastCountMinusOneRolesPerSequence}.
		/// </summary>
		protected static RoleSequenceStyles RoleSequenceStyles
		{
			get
			{
				return RoleSequenceStyles.OneRoleSequence | RoleSequenceStyles.AtLeastCountMinusOneRolesPerSequence;
			}
		}
		RoleSequenceStyles IConstraint.RoleSequenceStyles
		{
			get
			{
				return RoleSequenceStyles;
			}
		}
		#endregion // IConstraint Implementation
	}
	public partial class FrequencyConstraint : IConstraint
	{
		#region IConstraint Implementation
		/// <summary>
		/// Implements IConstraint.ConstraintType. Returns ConstraintType.InternalUniqueness.
		/// </summary>
		protected static ConstraintType ConstraintType
		{
			get
			{
				return ConstraintType.Frequency;
			}
		}
		ConstraintType IConstraint.ConstraintType
		{
			get
			{
				return ConstraintType;
			}
		}
		/// <summary>
		/// Implements IConstraint.RoleSequenceStyles. Returns {OneRoleSequence, MultipleRolesPerSequence}.
		/// </summary>
		protected static RoleSequenceStyles RoleSequenceStyles
		{
			get
			{
				return RoleSequenceStyles.OneRoleSequence | RoleSequenceStyles.MultipleRolesPerSequence;
			}
		}
		RoleSequenceStyles IConstraint.RoleSequenceStyles
		{
			get
			{
				return RoleSequenceStyles;
			}
		}
		#endregion // IConstraint Implementation
	}
	public partial class RingConstraint : IConstraint
	{
		#region IConstraint Implementation
		/// <summary>
		/// Implements IConstraint.ConstraintType. Returns ConstraintType.InternalUniqueness.
		/// </summary>
		protected static ConstraintType ConstraintType
		{
			get
			{
				return ConstraintType.Ring;
			}
		}
		ConstraintType IConstraint.ConstraintType
		{
			get
			{
				return ConstraintType;
			}
		}
		/// <summary>
		/// Implements IConstraint.RoleSequenceStyles. Returns {TwoRoleSequences, OneRolePerSequence, CompatibleColumns}.
		/// </summary>
		protected static RoleSequenceStyles RoleSequenceStyles
		{
			get
			{
				return RoleSequenceStyles.TwoRoleSequences | RoleSequenceStyles.OneRolePerSequence | RoleSequenceStyles.CompatibleColumns;
			}
		}
		RoleSequenceStyles IConstraint.RoleSequenceStyles
		{
			get
			{
				return RoleSequenceStyles;
			}
		}
		#endregion // IConstraint Implementation
	}
	public partial class ExternalUniquenessConstraint : IConstraint
	{
		#region IConstraint Implementation
		/// <summary>
		/// Implements IConstraint.ConstraintType. Returns ConstraintType.InternalUniqueness.
		/// </summary>
		protected static ConstraintType ConstraintType
		{
			get
			{
				return ConstraintType.ExternalUniqueness;
			}
		}
		ConstraintType IConstraint.ConstraintType
		{
			get
			{
				return ConstraintType;
			}
		}
		/// <summary>
		/// Implements IConstraint.RoleSequenceStyles. Returns {MultipleRowSequences, OneRolePerSequence}.
		/// </summary>
		protected static RoleSequenceStyles RoleSequenceStyles
		{
			get
			{
				return RoleSequenceStyles.MultipleRowSequences | RoleSequenceStyles.OneRolePerSequence;
			}
		}
		RoleSequenceStyles IConstraint.RoleSequenceStyles
		{
			get
			{
				return RoleSequenceStyles;
			}
		}
		#endregion // IConstraint Implementation
	}
	public partial class EqualityConstraint : IConstraint
	{
		#region IConstraint Implementation
		/// <summary>
		/// Implements IConstraint.ConstraintType. Returns ConstraintType.InternalUniqueness.
		/// </summary>
		protected static ConstraintType ConstraintType
		{
			get
			{
				return ConstraintType.Equality;
			}
		}
		ConstraintType IConstraint.ConstraintType
		{
			get
			{
				return ConstraintType;
			}
		}
		/// <summary>
		/// Implements IConstraint.RoleSequenceStyles. Returns {MultipleRowSequences, MultipleRolesPerSequence, CompatibleColumns}.
		/// </summary>
		protected static RoleSequenceStyles RoleSequenceStyles
		{
			get
			{
				return RoleSequenceStyles.MultipleRowSequences | RoleSequenceStyles.MultipleRolesPerSequence | RoleSequenceStyles.CompatibleColumns;
			}
		}
		RoleSequenceStyles IConstraint.RoleSequenceStyles
		{
			get
			{
				return RoleSequenceStyles;
			}
		}
		#endregion // IConstraint Implementation
	}
	public partial class ExclusionConstraint : IConstraint
	{
		#region IConstraint Implementation
		/// <summary>
		/// Implements IConstraint.ConstraintType. Returns ConstraintType.InternalUniqueness.
		/// </summary>
		protected static ConstraintType ConstraintType
		{
			get
			{
				return ConstraintType.Exclusion;
			}
		}
		ConstraintType IConstraint.ConstraintType
		{
			get
			{
				return ConstraintType;
			}
		}
		/// <summary>
		/// Implements IConstraint.RoleSequenceStyles. Returns {MultipleRowSequences, MultipleRowSequences, CompatibleColumns}.
		/// </summary>
		protected static RoleSequenceStyles RoleSequenceStyles
		{
			get
			{
				return RoleSequenceStyles.MultipleRowSequences | RoleSequenceStyles.MultipleRolesPerSequence | RoleSequenceStyles.CompatibleColumns;
			}
		}
		RoleSequenceStyles IConstraint.RoleSequenceStyles
		{
			get
			{
				return RoleSequenceStyles;
			}
		}
		#endregion // IConstraint Implementation
	}
	public partial class DisjunctiveMandatoryConstraint : IConstraint
	{
		#region IConstraint Implementation
		/// <summary>
		/// Implements IConstraint.ConstraintType. Returns ConstraintType.InternalUniqueness.
		/// </summary>
		protected static ConstraintType ConstraintType
		{
			get
			{
				return ConstraintType.DisjunctiveMandatory;
			}
		}
		ConstraintType IConstraint.ConstraintType
		{
			get
			{
				return ConstraintType;
			}
		}
		/// <summary>
		/// Implements IConstraint.RoleSequenceStyles. Returns {MultipleRowSequences, OneRolePerSequence}.
		/// </summary>
		protected static RoleSequenceStyles RoleSequenceStyles
		{
			get
			{
				return RoleSequenceStyles.MultipleRowSequences | RoleSequenceStyles.OneRolePerSequence;
			}
		}
		RoleSequenceStyles IConstraint.RoleSequenceStyles
		{
			get
			{
				return RoleSequenceStyles;
			}
		}
		#endregion // IConstraint Implementation
	}
	public partial class SubsetConstraint : IConstraint
	{
		#region IConstraint Implementation
		/// <summary>
		/// Implements IConstraint.ConstraintType. Returns ConstraintType.InternalUniqueness.
		/// </summary>
		protected static ConstraintType ConstraintType
		{
			get
			{
				return ConstraintType.Subset;
			}
		}
		ConstraintType IConstraint.ConstraintType
		{
			get
			{
				return ConstraintType;
			}
		}
		/// <summary>
		/// Implements IConstraint.RoleSequenceStyles. Returns {TwoRoleSequences, MultipleRolesPerSequence, OrderedRoleSequences, CompatibleColumns}.
		/// </summary>
		protected static RoleSequenceStyles RoleSequenceStyles
		{
			get
			{
				return RoleSequenceStyles.TwoRoleSequences | RoleSequenceStyles.MultipleRolesPerSequence | RoleSequenceStyles.OrderedRoleSequences | RoleSequenceStyles.CompatibleColumns;
			}
		}
		RoleSequenceStyles IConstraint.RoleSequenceStyles
		{
			get
			{
				return RoleSequenceStyles;
			}
		}
		#endregion // IConstraint Implementation
	}
	#endregion // ConstraintType and RoleSequenceStyles implementation for all constraints
}
