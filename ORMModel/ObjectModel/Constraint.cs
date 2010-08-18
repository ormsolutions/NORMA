#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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
using System.IO;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
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
	public interface IConstraint
	{
		/// <summary>
		/// Get the type of this constraint
		/// </summary>
		ConstraintType ConstraintType
		{
			get;
		}
		/// <summary>
		/// Get the role settings for this constraint
		/// </summary>
		RoleSequenceStyles RoleSequenceStyles
		{
			get;
		}
		/// <summary>
		/// Get the details of how this constraint is stored.
		/// </summary>
		ConstraintStorageStyle ConstraintStorageStyle
		{
			get;
		}
		/// <summary>
		/// Get details on whether this is an internal or external constraint
		/// </summary>
		bool ConstraintIsInternal
		{
			get;
		}
		/// <summary>
		/// Get details on whether this is an implied constraint
		/// </summary>
		bool ConstraintIsImplied
		{
			get;
		}
		/// <summary>
		/// Retrieve the model for the current constraint
		/// </summary>
		ORMModel Model
		{
			get;
		}
		/// <summary>
		/// Get or set the constraint modality.
		/// </summary>
		ConstraintModality Modality
		{
			get;
			set;
		}
		/// <summary>
		/// Get or set the preferred identifier for this constraint.
		/// Valid for uniqueness constraint types.
		/// </summary>
		ObjectType PreferredIdentifierFor
		{
			get;
			set;
		}
		/// <summary>
		/// Called during a transaction to tell the constraint
		/// to revalidate all column compatibility settings.
		/// </summary>
		void ValidateColumnCompatibility();

		/// <summary>
		/// Returns a list of IntersectingConstraintValidation objects
		/// each of which represents an intersecting constraint pattern the constraint
		/// can participate in.
		/// </summary>
		/// <returns></returns>
		IList<IntersectingConstraintValidation> GetIntersectingConstraintValidationInfo();
	}
	/// <summary>
	/// All interesections between different costraints that produce errors belong to one of the
	/// defined patterns in this enum and each pattern is validated in its own way
	/// </summary>
	public enum IntersectingConstraintPattern
	{
		/// <summary>
		/// Constraint pattern not specified
		/// </summary>
		None,
		/// <summary>
		/// Pertains to 2 SetContaints (Uniqueness or Mandatory) existing together
		/// For example, simple mandatory implies disjunctive mandatory; internal uniqueness implies
		/// external uniqueness (when the constraint at question cannot be a subset of
		/// its ConstraintsInPotentialConflict)
		/// </summary>
		SetConstraintSubset,
		/// <summary>
		/// Pertains to two set constraints on the same set of roles.
		/// For example, a frequency constraint should not be associated
		/// with the same roles as a uniqueness constraint.
		/// </summary>
		SetConstraintSameSet,
		/// <summary>
		/// Pertains to 2 SetComparisonContaints (Exclusion, Equality, Subset) existing
		/// in a subset relationship.  This pattern is used for a set comparison constraint that cannot be a 
		/// subset of another set comparison constraint (for example, exclusion cannot be a subset of 
		/// equality)
		/// </summary>
		SetComparisonConstraintSubset,
		/// <summary>
		/// Pertains to 2 SetComparisonContaints (Exclusion, Equality, Subset) existing
		/// in a subset relationship.  This pattern is used for a set comparison constraint that cannot be a 
		/// superset of another set comparison constraint (for example, equality cannot be a superset of 
		/// exclusion)
		/// </summary>
		SetComparisonConstraintSuperset,
		/// <summary>
		/// Occurs when there is a mandatory constraint on the role that belongs to the role sequence subset
		/// constraint is going toward
		/// </summary>
		SubsetImpliedByMandatory,
		/// <summary>
		/// Occurs when there is a mandatory constraint on the role that belongs to the role sequence subset
		/// constraint is going from
		/// </summary>
		SubsetContradictsMandatory,
		/// <summary>
		/// Occurs when 2 or more roles in a column that participates in relationship with Equality
		/// constraint is mandatory
		/// </summary>
		EqualityImpliedByMandatory,
		/// <summary>
		/// Occurs when one role in a column that participates in relationship with Equality
		/// constraint is mandatory and the other is not
		/// </summary>
		NotWellModeledEqualityAndMandatory,
		/// <summary>
		/// Occurs when at least role in a column that participates in relationship with Exclusion
		/// constraint is mandatory
		/// </summary>
		ExclusionContradictsMandatory,
	}
	/// <summary>
	/// Options modifying the behavior of the <see cref="IntersectingConstraintPattern"/> enums.
	/// </summary>
	[Flags]
	public enum IntersectingConstraintPatternOptions
	{
		/// <summary>
		/// No options specified
		/// </summary>
		None,
		/// <summary>
		/// Constraint intersection patterns should be validated regardless of the relative strength of the modality on the intersecting constraint and the context constraint
		/// </summary>
		IntersectingConstraintModalityIgnored = 0,
		/// <summary>
		/// Constraint intersection patterns should be validated if the modality of the intersecting constraint is not stronger than the context constraint
		/// </summary>
		IntersectingConstraintModalityNotStronger = 1,
		/// <summary>
		/// Constraint intersection patterns should be validated if the modality of the intersecting constraint is not weaker than the context constraint
		/// </summary>
		IntersectingConstraintModalityNotWeaker = 2,
		/// <summary>
		/// A mask value representating all modality flags
		/// </summary>
		IntersectingConstraintModalityMask = IntersectingConstraintPatternOptions.IntersectingConstraintModalityNotStronger | IntersectingConstraintPatternOptions.IntersectingConstraintModalityNotWeaker,
	}


	/// <summary>
	/// Used for set constraints: mandatory and uniqueness
	/// in the cases when one implies the other of the same type
	/// </summary>
	public struct IntersectingConstraintValidation
	{
		private IntersectingConstraintPattern myPattern;
		private IntersectingConstraintPatternOptions myPatternOptions;
		private Guid? myDomainRoleToError;
		private Guid? myIntersectingDomainRoleToError;
		private ConstraintType[] myConstraintTypesInPotentialConflict;

		/// <summary>
		/// Constructor for the struct IntersectingConstraintValidation
		/// </summary>
		/// <param name="pattern">Pattern this constraint can participate in and produce error</param>
		/// <param name="options">Options modifying the pattern behavior</param>
		/// <param name="domainRoleToError">Identifier of the <see cref="DomainRoleInfo"/> attached to the context constraint
		/// that leads to the associated error created for this instance. Set to <see langword="null"/> if an error is not attached
		/// to this constraint.</param>
		/// <param name="intersectingDomainRoleToError">Identifier of the <see cref="DomainRoleInfo"/> attached to the constraint
		/// that intersects with the context constraint that leads to the associated error created for this instance. Set to
		/// <see langword="null"/> if an error is not attached to the intersecting constraint.</param>
		/// <param name="constraintTypesInPotentialConflict">Given the pattern, what type should the other constraint
		/// be of to produce the error</param>
		public IntersectingConstraintValidation(
			IntersectingConstraintPattern pattern,
			IntersectingConstraintPatternOptions options,
			Guid? domainRoleToError,
			Guid? intersectingDomainRoleToError,
			params ConstraintType[] constraintTypesInPotentialConflict)
		{
			this.myPattern = pattern;
			this.myPatternOptions = options;
			this.myDomainRoleToError = domainRoleToError;
			this.myIntersectingDomainRoleToError = intersectingDomainRoleToError;
			this.myConstraintTypesInPotentialConflict = constraintTypesInPotentialConflict;
		}
#if FALSE
		/// <summary>
		/// Checks if the current instance of IntersectingConstraintValidation and the
		/// instance passed in are the same based on all fields of the struct
		/// </summary>
		/// <param name="other">
		/// The instance of IntersectionValidationPattern to compare to
		/// </param>
		/// <returns>Returns true if all fields of the two instances are the same</returns>
		public bool Equals(IntersectingConstraintValidation other)
		{
			if (this.myPattern == other.myPattern &&
				this.myPatternOptions == other.myPatternOptions &&
				this.myDomainRoleToError == other.myDomainRoleToError)
			{
				ConstraintType[] thisConstraintTypes = this.myConstraintTypesInPotentialConflict;
				ConstraintType[] otherConstraintTypes = this.myConstraintTypesInPotentialConflict;
				int typeCount = thisConstraintTypes.Length;
				if (typeCount == otherConstraintTypes.Length)
				{
					int i = 0;
					for (; i < typeCount; ++i)
					{
						if (thisConstraintTypes[i] != otherConstraintTypes[i])
						{
							break;
						}
					}
					if (i == typeCount)
					{
						return true;
					}
				}
			}
			return false;
		}
#endif // FALSE
		/// <summary>
		/// For each IntersectingConstraintPattern, there are 2 types of constraints:
		/// the one for which the validation is being done and the other that can conlict with it.
		/// For example, for SetComparisonConstraintSubset pattern, if the validation is being done
		/// for exclusion constraint, it can potentially conflict with equality constraint, because
		/// exclusion cannot be a subset of equality
		/// </summary>
		public ConstraintType[] ConstraintTypesInPotentialConflict
		{
			get
			{
				return myConstraintTypesInPotentialConflict;
			}
		}
		/// <summary>
		/// The pattern a constraint can participate in with another constraint to produce an error
		/// </summary>
		public IntersectingConstraintPattern IntersectionValidationPattern
		{
			get
			{
				return myPattern;
			}
		}
		/// <summary>
		/// The options applied to the pattern to produce the error
		/// </summary>
		public IntersectingConstraintPatternOptions IntersectionValidationOptions
		{
			get
			{
				return myPatternOptions;
			}
		}
		/// <summary>
		/// Identifier of the <see cref="DomainRoleInfo"/> attached to the context constraint
		/// that leads to the associated error created for this instance.
		/// to this constraint.
		/// </summary>
		public Guid? DomainRoleToError
		{
			get
			{
				return myDomainRoleToError;
			}
		}
		/// <summary>
		/// Identifier of the <see cref="DomainRoleInfo"/> attached to the constraint
		/// that intersects with the context constraint that leads to the associated error created for this instance.
		/// </summary>
		public Guid? IntersectingDomainRoleToError
		{
			get
			{
				return myIntersectingDomainRoleToError;
			}
		}
		/// <summary>
		/// Determine if the modality of a context and intersecting constraint
		/// allows constraint validation based on the provided options.
		/// </summary>
		/// <param name="contextModality">The modality of the context constraint</param>
		/// <param name="intersectingModality">The modality of the intersecting constraint</param>
		/// <returns><see langword="true"/> if the constraint validation should continue.</returns>
		public bool TestModality(ConstraintModality contextModality, ConstraintModality intersectingModality)
		{
			bool retVal = true;
			switch (myPatternOptions & IntersectingConstraintPatternOptions.IntersectingConstraintModalityMask)
			{
				case IntersectingConstraintPatternOptions.IntersectingConstraintModalityNotStronger:
					if (contextModality == ConstraintModality.Deontic)
					{
						retVal = intersectingModality != ConstraintModality.Alethic;
					}
					break;
				case IntersectingConstraintPatternOptions.IntersectingConstraintModalityNotWeaker:
					if (contextModality == ConstraintModality.Alethic)
					{
						retVal = intersectingModality != ConstraintModality.Deontic;
					}
					break;
			}
			return retVal;
		}
	}
	#endregion // IConstraint interface
	#region Constraint class
	/// <summary>
	/// A utility class with static helper methods
	/// </summary>
	public static partial class ConstraintUtility
	{
		#region ConstraintUtility specific
		/// <summary>
		/// The minimum number of required role sequences
		/// </summary>
		/// <param name="constraint">Constraint class to test</param>
		public static int RoleSequenceCountMinimum(IConstraint constraint)
		{
			if (constraint.ConstraintIsInternal)
			{
				return 0;
			}
			int retVal = 1;
			switch (constraint.RoleSequenceStyles & RoleSequenceStyles.SequenceMultiplicityMask)
			{
				case RoleSequenceStyles.MultipleRowSequences:
				case RoleSequenceStyles.TwoRoleSequences:
					retVal = 2;
					break;
#if DEBUG
				case RoleSequenceStyles.OneOrMoreRoleSequences:
				case RoleSequenceStyles.OneRoleSequence:
					break;
				default:
					Debug.Fail("Shouldn't be here");
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
			if (constraint.ConstraintIsInternal)
			{
				return int.MaxValue;
			}
			int retVal = 1;
			switch (constraint.RoleSequenceStyles & RoleSequenceStyles.SequenceMultiplicityMask)
			{
				case RoleSequenceStyles.OneOrMoreRoleSequences:
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
					Debug.Fail("Shouldn't be here");
					break;
#endif // DEBUG
			}
			return retVal;
		}
		#endregion // ConstraintUtility specific
		#region ConstraintRoleSequenceHasRoleDeletedRule
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleSequenceHasRole), FireTime=LocalCommit, Priority=ORMCoreDomainModel.BeforeDelayValidateRulePriority;
		/// Runs when ConstraintRoleSequenceHasRole element is removed. 
		/// If there are no more roles in the role collection then the
		/// entire ConstraintRoleSequence is removed
		/// </summary>
		private static void ConstraintRoleSequenceHasRoleDeletedRule(ElementDeletedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			ConstraintRoleSequence roleSequence = link.ConstraintRoleSequence;
			if (!roleSequence.IsDeleted)
			{
				if (roleSequence.RoleCollection.Count == 0)
				{
					roleSequence.Delete();
				}
			}
		}
		#endregion //ConstraintRoleSequenceHasRoleDeletedRule
	}
	#endregion // Constraint class
	#region SetConstraint class
	public partial class SetConstraint : IModelErrorOwner, IModelErrorDisplayContext
	{
		#region SetConstraint Specific
		/// <summary>
		/// Ensure that an FactConstraint exists between the
		/// fact type owning the passed in role and this constraint.
		/// FactConstraint links are generated automatically
		/// and should never be directly created.
		/// </summary>
		/// <param name="role">The role to attach</param>
		/// <param name="createdAndInitialized">Returns true if creating the
		/// relationship initialized it indirectly.</param>
		/// <returns>The associated FactConstraint relationship.</returns>
		private FactConstraint EnsureFactConstraintForRole(Role role, out bool createdAndInitialized)
		{
			createdAndInitialized = false;
			FactConstraint retVal = null;
			FactType fact = role.FactType;
			if (fact != null)
			{
				ReadOnlyCollection<FactSetConstraint> existingFactConstraints = DomainRoleInfo.GetElementLinks<FactSetConstraint>(fact, FactSetConstraint.FactTypeDomainRoleId);
				int listCount = existingFactConstraints.Count;
				for (int i = 0; i < listCount; ++i)
				{
					FactSetConstraint testFactConstraint = existingFactConstraints[i];
					if (testFactConstraint.SetConstraint == this)
					{
						retVal = testFactConstraint;
						break;
					}
				}
				if (retVal == null)
				{
					retVal = new FactSetConstraint(fact, this);
					createdAndInitialized = retVal.ConstrainedRoleCollection.Count != 0;
				}
			}
			return retVal;
		}
		#endregion // SetConstraint Specific
		#region CustomStorage Handling
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
		private string GetDefinitionTextValue()
		{
			Definition currentDefinition = Definition;
			return (currentDefinition != null) ? currentDefinition.Text : String.Empty;
		}
		private string GetNoteTextValue()
		{
			Note currentNote = Note;
			return (currentNote != null) ? currentNote.Text : String.Empty;
		}
		#endregion // CustomStorage Handling
		#region SetConstraint synchronization rules
		/// <summary>
		/// Make sure the model for the constraint and fact are consistent
		/// </summary>
		private static void EnforceNoForeignFactTypes(FactSetConstraint link)
		{
			FactType fact = link.FactType;
			SetConstraint constraint = link.SetConstraint;
			ORMModel factModel = fact.Model;
			ORMModel constraintModel = constraint.Model;
			if ((constraint as IConstraint).ConstraintIsInternal)
			{
				// Check for internal constraint pattern violation when facts are attached we're at it
				if (1 != constraint.FactTypeCollection.Count)
				{
					throw new InvalidOperationException(ResourceStrings.ModelExceptionConstraintEnforceSingleFactTypeForInternalConstraint);
				}
			}
			if (factModel != null)
			{
				if (constraintModel == null)
				{
					constraint.Model = factModel;
				}
				else if (factModel != constraintModel)
				{
					throw new InvalidOperationException(ResourceStrings.ModelExceptionConstraintEnforceNoForeignFactTypes);
				}
			}
			else if (constraintModel != null)
			{
				fact.Model = constraintModel;
			}
		}
		/// <summary>
		/// AddRule: typeof(FactSetConstraint)
		/// Ensure that a fact and constraint have a consistent owning model
		/// </summary>
		private static void FactSetConstraintAddedRule(ElementAddedEventArgs e)
		{
			EnforceNoForeignFactTypes(e.ModelElement as FactSetConstraint);
		}
		/// <summary>
		/// DeleteRule: typeof(FactSetConstraint)
		/// Ensure that an internal constraint always goes away with its fact type
		/// </summary>
		private static void FactSetConstraintDeletedRule(ElementDeletedEventArgs e)
		{
			FactSetConstraint link = e.ModelElement as FactSetConstraint;
			SetConstraint constraint = link.SetConstraint;
			if (!constraint.IsDeleted && !constraint.IsDeleting && constraint.Constraint.ConstraintIsInternal && constraint.Model != null)
			{
				constraint.Delete();
			}
		}
		/// <summary>
		/// AddRule: typeof(ModelHasFactType)
		/// Ensure that a newly added fact that is already attached to constraints
		/// has a consistent model for the constraints
		/// </summary>
		private static void FactTypeAddedRule(ElementAddedEventArgs e)
		{
			ModelHasFactType link = e.ModelElement as ModelHasFactType;
			ReadOnlyCollection<ElementLink> existingConstraintLinks = DomainRoleInfo.GetElementLinks<ElementLink>(link.FactType, FactSetConstraint.FactTypeDomainRoleId);
			int existingLinksCount = existingConstraintLinks.Count;
			for (int i = 0; i < existingLinksCount; ++i)
			{
				EnforceNoForeignFactTypes(existingConstraintLinks[i] as FactSetConstraint);
			}
		}
		/// <summary>
		/// AddRule: typeof(ConstraintRoleSequenceHasRole)
		/// Add Rule for arity and compatibility checking when Single Column ExternalConstraints roles are added
		/// </summary>
		private static void EnforceRoleSequenceValidityForRoleAddRule(ElementAddedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			SetConstraint constraint = link.ConstraintRoleSequence as SetConstraint;
			if (constraint != null)
			{
				FrameworkDomainModel.DelayValidateElement(constraint, DelayValidateRoleSequenceCountErrors);
				FrameworkDomainModel.DelayValidateElement(constraint, DelayValidateCompatibleRolePlayerTypeError);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleSequenceHasRole)
		/// Deleted Rule for arity and compatibility checking when Single Column ExternalConstraints roles are added
		/// </summary>
		private static void EnforceRoleSequenceValidityForRoleDeleteRule(ElementDeletedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			SetConstraint constraint = link.ConstraintRoleSequence as SetConstraint;
			if (constraint != null && !constraint.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(constraint, DelayValidateRoleSequenceCountErrors);
				FrameworkDomainModel.DelayValidateElement(constraint, DelayValidateCompatibleRolePlayerTypeError);
			}
		}
		/// <summary>
		/// AddRule: typeof(ConstraintRoleSequenceHasRole)
		/// If a role is added after the role sequence is already attached,
		/// then create the corresponding FactConstraint and ExternalRoleConstraint
		/// </summary>
		private static void ConstraintRoleSequenceHasRoleAddedRule(ElementAddedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			SetConstraint constraint = link.ConstraintRoleSequence.Constraint as SetConstraint;
			if (constraint != null)
			{
				bool createdAndInitialized;
				FactConstraint factConstraint = constraint.EnsureFactConstraintForRole(link.Role, out createdAndInitialized);
				if (factConstraint != null && !createdAndInitialized)
				{
					factConstraint.ConstrainedRoleCollection.Add(link);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(ModelHasSetConstraint)
		/// If a role sequence is added that already contains roles, then
		/// make sure the corresponding FactConstraint and ExternalRoleConstraint
		/// objects are created for each role. Note that a single column external
		/// constraint is a role sequence.
		/// </summary>
		private static void ConstraintAddedRule(ElementAddedEventArgs e)
		{
			ModelHasSetConstraint link = e.ModelElement as ModelHasSetConstraint;
			// Add implied fact constraint elements
			EnsureFactConstraintForRoleSequence(link);

			// Register for delayed error validation
			IModelErrorOwner errorOwner = link.SetConstraint as IModelErrorOwner;
			if (errorOwner != null)
			{
				errorOwner.DelayValidateErrors();
			}
		}
		/// <summary>
		/// Helper function to support the same fact constraint fixup
		/// during both deserialization and rules.
		/// </summary>
		/// <param name="link">A roleset link added to the constraint</param>
		private static void EnsureFactConstraintForRoleSequence(ModelHasSetConstraint link)
		{
			SetConstraint roleSequence = link.SetConstraint;
			// The following line gets the links instead of the counterparts,
			// which are provided by roleSequence.RoleCollection
			IList roleLinks = DomainRoleInfo.GetElementLinks<ElementLink>(roleSequence, ConstraintRoleSequenceHasRole.ConstraintRoleSequenceDomainRoleId);
			int roleCount = roleLinks.Count;
			if (roleCount != 0)
			{
				for (int i = 0; i < roleCount; ++i)
				{
					ConstraintRoleSequenceHasRole roleLink = (ConstraintRoleSequenceHasRole)roleLinks[i];
					bool createdAndInitialized;
					FactConstraint factConstraint = roleSequence.EnsureFactConstraintForRole(roleLink.Role, out createdAndInitialized);
					if (factConstraint != null && !createdAndInitialized)
					{
						LinkedElementCollection<ConstraintRoleSequenceHasRole> constrainedRoles = factConstraint.ConstrainedRoleCollection;
						if (!constrainedRoles.Contains(roleLink))
						{
							constrainedRoles.Add(roleLink);
						}
					}
				}
			}
		}
		#endregion // SetConstraint synchronization rules
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Implements IModelErrorOwner.GetErrorCollection
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			ModelErrorUses startFilter = filter;
			if (filter == 0)
			{
				filter = (ModelErrorUses)(-1);
			}
			if (0 != (filter & (ModelErrorUses.BlockVerbalization | ModelErrorUses.DisplayPrimary)))
			{
				TooFewRoleSequencesError tooFew;
				if (null != (tooFew = TooFewRoleSequencesError))
				{
					yield return new ModelErrorUsage(tooFew, ModelErrorUses.BlockVerbalization);
				}
				TooManyRoleSequencesError tooMany;
				if (null != (tooMany = TooManyRoleSequencesError))
				{
					yield return new ModelErrorUsage(tooMany, ModelErrorUses.BlockVerbalization);
				}
				CompatibleRolePlayerTypeError typeCompatibility;
				if (null != (typeCompatibility = CompatibleRolePlayerTypeError))
				{
					yield return typeCompatibility;
				}
			}
			if (filter == ModelErrorUses.BlockVerbalization)
			{
				// We can't verbalize if there are constrained facts without readings or
				// any constrained roles without role players
				ReadOnlyCollection<FactSetConstraint> factLinks = FactSetConstraint.GetLinksToFactTypeCollection(this);
				int count = factLinks.Count;

				// Show the reading errors first
				for (int i = 0; i < count; ++i)
				{
					FactTypeRequiresReadingError noReadingError = factLinks[i].FactType.ReadingRequiredError;
					if (noReadingError != null)
					{
						yield return noReadingError;
					}
				}

				if (!(this as IConstraint).ConstraintIsInternal)
				{
					// Show the missing role errors
					for (int i = 0; i < count; ++i)
					{
						LinkedElementCollection<ConstraintRoleSequenceHasRole> constrainedRoles = factLinks[i].ConstrainedRoleCollection;
						int constrainedRoleCount = constrainedRoles.Count;
						for (int j = 0; j < constrainedRoleCount; ++j)
						{
							Role role = constrainedRoles[j].Role;
							RolePlayerRequiredError noRolePlayerError = role.RolePlayerRequiredError;
							if (noRolePlayerError != null)
							{
								yield return noRolePlayerError;
							}
						}
					}
				}
			}
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
			{
				ConstraintDuplicateNameError duplicateName = DuplicateNameError;
				if (duplicateName != null)
				{
					yield return duplicateName;
				}
				ImplicationError implicationError = ImplicationError;
				if (implicationError != null)
				{
					yield return implicationError;
				}
			}

			// Get errors off the base
			foreach (ModelErrorUsage baseError in base.GetErrorCollection(startFilter))
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
		/// Validate all errors on the external constraint. This
		/// is called during deserialization fixup when rules are
		/// suspended.
		/// </summary>
		/// <param name="notifyAdded">A callback for notifying
		/// the caller of all objects that are added.</param>
		protected new void ValidateErrors(INotifyElementAdded notifyAdded)
		{
			// Calls added here need corresponding delayed calls in DelayValidateErrors
			base.ValidateErrors(notifyAdded);
			VerifyCompatibleRolePlayerTypeForRule(notifyAdded);
			VerifyRoleSequenceCountForRule(notifyAdded);
			ValidateSetConstraintSubsetPattern(notifyAdded);
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
			base.DelayValidateErrors();
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateCompatibleRolePlayerTypeError);
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateRoleSequenceCountErrors);
			DelayValidateConstraintPatternError(this);
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner Implementation
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// adds the implicit FactConstraint elements.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new ExternalConstraintFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation. Adds implicit FactConstraint relationships
		/// </summary>
		private sealed class ExternalConstraintFixupListener : DeserializationFixupListener<SetConstraint>
		{
			/// <summary>
			/// ExternalConstraintFixupListener constructor
			/// </summary>
			public ExternalConstraintFixupListener()
				: base((int)ORMDeserializationFixupPhase.AddImplicitElements)
			{
			}
			/// <summary>
			/// Process elements by added an FactConstraint for
			/// each roleset
			/// </summary>
			/// <param name="element">An ExternalConstraint element</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(SetConstraint element, Store store, INotifyElementAdded notifyAdded)
			{
				ReadOnlyCollection<ModelHasSetConstraint> links = DomainRoleInfo.GetElementLinks<ModelHasSetConstraint>(element, ModelHasSetConstraint.SetConstraintDomainRoleId);
				IConstraint constraint = element as IConstraint;
				int linksCount = links.Count;
				for (int i = 0; i < linksCount; ++i)
				{
					EnsureFactConstraintForRoleSequence(links[i]);
				}
				ReadOnlyCollection<FactSetConstraint> factLinks = DomainRoleInfo.GetElementLinks<FactSetConstraint>(element, FactSetConstraint.SetConstraintDomainRoleId);
				int factLinksCount = factLinks.Count;
				// Validate subtype constraint patterns before sending additional element added notifications.
				// The XML schema is current very lax on exclusion constraints on Sub/SupertypeMetaRoles, so
				// any constraints on any combination of roles/metaroles load successfully. Eliminate any
				// constraints with patterns that will raise exceptions inside the tool.
				if (!constraint.ConstraintIsInternal && constraint.ConstraintType != ConstraintType.ImpliedMandatory)
				{
					bool seenSubtypeFact = false;
					bool invalidSubtypeConstraint = false;
					for (int j = 0; j < factLinksCount; ++j)
					{
						FactSetConstraint factLink = factLinks[j];
						if (factLink.FactType is SubtypeFact)
						{
							if (!seenSubtypeFact)
							{
								if (j > 0)
								{
									invalidSubtypeConstraint = true;
								}
								seenSubtypeFact = true;
								if (constraint.ConstraintType == ConstraintType.SimpleMandatory)
								{
									invalidSubtypeConstraint = true;
									break;
								}
							}
							LinkedElementCollection<ConstraintRoleSequenceHasRole> roleLinks = factLink.ConstrainedRoleCollection;
							if (roleLinks.Count != 1 || !(roleLinks[0].Role is SupertypeMetaRole))
							{
								invalidSubtypeConstraint = true;
								break;
							}
						}
						else if (seenSubtypeFact)
						{
							invalidSubtypeConstraint = true;
							break;
						}
					}
					if (invalidSubtypeConstraint)
					{
						// All of the relationships we're dealing with here have delete
						// progation set, so we do not need additional rules or code to
						// remove the elements.
						element.Delete();
						return;
					}
				}
				for (int j = 0; j < factLinksCount; ++j)
				{
					// Notify that the link was added. Note that we set
					// addLinks to true here because we expect ExternalRoleConstraint
					// links to be attached to each FactConstraint
					notifyAdded.ElementAdded(factLinks[j], true);
				}
			}
		}
		#endregion // Deserialization Fixup
		#region Error synchronization rules
		#region VerifyRoleSequenceCountForRule
		/// <summary>
		/// Validator callback for CompatibleRolePlayerTypeError
		/// </summary>
		private static void DelayValidateRoleSequenceCountErrors(ModelElement element)
		{
			(element as SetConstraint).VerifyRoleSequenceCountForRule(null);
		}
		/// <summary>
		/// Add, remove, and otherwise validate the current set of
		/// errors for this constraint.
		/// </summary>
		/// <param name="notifyAdded">If not null, this is being called during
		/// load when rules are not in place. Any elements that are added
		/// must be notified back to the caller.</param>
		private void VerifyRoleSequenceCountForRule(INotifyElementAdded notifyAdded)
		{
			if (!IsDeleted)
			{
				int minCount = ConstraintUtility.RoleSequenceCountMinimum(this);
				int maxCount;
				int currentCount = RoleCollection.Count;
				Store store = Store;
				TooFewRoleSequencesError insufficientError;
				TooManyRoleSequencesError extraError;
				bool removeTooFew = false;
				bool removeTooMany = false;
				if (currentCount < minCount)
				{
					if (null == TooFewRoleSequencesError)
					{
						insufficientError = new TooFewRoleSequencesError(store);
						insufficientError.SetConstraint = this;
						insufficientError.Model = Model;
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
						if (null == TooManyRoleSequencesError)
						{
							extraError = new TooManyRoleSequencesError(store);
							extraError.SetConstraint = this;
							extraError.Model = Model;
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
					insufficientError.Delete();
				}
				if (removeTooMany && null != (extraError = TooManyRoleSequencesError))
				{
					extraError.Delete();
				}
			}
		}
		#endregion // VerifyRoleSequenceCountForRule
		/// <summary>
		/// Validator callback for CompatibleRolePlayerTypeError
		/// </summary>
		private static void DelayValidateCompatibleRolePlayerTypeError(ModelElement element)
		{
			(element as SetConstraint).VerifyCompatibleRolePlayerTypeForRule(null);
		}
		/// <summary>
		/// Verify CompatibleRolePlayertypeForRule Used to verify compatibility for single column constraints.
		/// </summary>
		/// <param name="notifyAdded">If not null, this is being called during
		/// load when rules are not in place. Any elements that are added
		/// must be notified back to the caller.</param>
		private void VerifyCompatibleRolePlayerTypeForRule(INotifyElementAdded notifyAdded)
		{
			CompatibleRolePlayerTypeError compatibleError;
			if (IsDeleted)
			{
				return;
			}
			if (0 == (((IConstraint)this).RoleSequenceStyles & RoleSequenceStyles.CompatibleColumns))
			{
				if (notifyAdded != null)
				{
					// Check on load, but not later. Constraint types don't switch on the fly
					if (null != (compatibleError = CompatibleRolePlayerTypeError))
					{
						compatibleError.Delete();
					}
				}
				return;
			}

			bool isCompatible = true;

			LinkedElementCollection<Role> roles = RoleCollection;
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
								ObjectType.WalkSupertypes(firstRolePlayer, delegate(ObjectType type, int depth, bool isPrimary)
								{
									superTypesCache.Add(type);
									return ObjectTypeVisitorResult.Continue;
								});
							}
							// If the type is contained, WalkSupertype will return false because the iteration
							// did not complete.
							isCompatible = !ObjectType.WalkSupertypes(currentRolePlayer, delegate(ObjectType type, int depth, bool isPrimary)
							{
								// Continue iteration if the type is recognized in the cache
								return superTypesCache.Contains(type) ? ObjectTypeVisitorResult.Stop : ObjectTypeVisitorResult.Continue;
							});

							if (!isCompatible)
							{
								//If the error is not present, add it to the model
								if (null == CompatibleRolePlayerTypeError)
								{
									compatibleError = new CompatibleRolePlayerTypeError(Store);
									compatibleError.SetConstraint = this;
									compatibleError.Model = Model;
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
					compatibleError.Delete();
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(ObjectTypePlaysRole)
		/// Add Rule for VerifyCompatibleRolePlayer when a Role/Object relationship is added
		/// </summary>
		private static void EnforceRoleSequenceValidityForRolePlayerAddRule(ElementAddedEventArgs e)
		{
			ProcessEnforceRoleSequenceValidityForRolePlayerAdd(e.ModelElement as ObjectTypePlaysRole);
		}
		/// <summary>
		/// Shared rule helper method
		/// </summary>
		private static void ProcessEnforceRoleSequenceValidityForRolePlayerAdd(ObjectTypePlaysRole link)
		{
			Role role = link.PlayedRole;
			LinkedElementCollection<ConstraintRoleSequence> roleSequences = role.ConstraintRoleSequenceCollection;
			int count = roleSequences.Count;
			for (int i = 0; i < count; ++i)
			{
				SetConstraint sequence = roleSequences[i] as SetConstraint;
				if (sequence != null && 0 != (((IConstraint)sequence).RoleSequenceStyles & RoleSequenceStyles.CompatibleColumns))
				{
					FrameworkDomainModel.DelayValidateElement(sequence, DelayValidateCompatibleRolePlayerTypeError);
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ObjectTypePlaysRole)
		/// Delete Rule for VerifyCompatibleRolePlayer when a Role/Object relationship is removed
		/// </summary>
		private static void EnforceRoleSequenceValidityForRolePlayerDeleteRule(ElementDeletedEventArgs e)
		{
			ProcessEnforceRoleSequenceValidityForRolePlayerDelete(e.ModelElement as ObjectTypePlaysRole, null);
		}
		/// <summary>
		/// Rule helper method
		/// </summary>
		private static void ProcessEnforceRoleSequenceValidityForRolePlayerDelete(ObjectTypePlaysRole link, Role role)
		{
			if (role == null)
			{
				role = link.PlayedRole;
			}
			LinkedElementCollection<ConstraintRoleSequence> roleSequences = role.ConstraintRoleSequenceCollection;
			int count = roleSequences.Count;
			for (int i = 0; i < count; ++i)
			{

				SetConstraint sequence = roleSequences[i] as SetConstraint;
				if (sequence != null && 0 != (((IConstraint)sequence).RoleSequenceStyles & RoleSequenceStyles.CompatibleColumns))
				{
					FrameworkDomainModel.DelayValidateElement(sequence, DelayValidateCompatibleRolePlayerTypeError);
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ObjectTypePlaysRole)
		/// Forward ObjectTypePlaysRole role player changes to corresponding Add/Delete rules
		/// </summary>
		private static void EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			ObjectTypePlaysRole link = e.ElementLink as ObjectTypePlaysRole;
			if (link.IsDeleted)
			{
				return;
			}
			Guid changedRoleGuid = e.DomainRole.Id;
			if (changedRoleGuid == ObjectTypePlaysRole.PlayedRoleDomainRoleId)
			{
				ProcessEnforceRoleSequenceValidityForRolePlayerDelete(link, (Role)e.OldRolePlayer);
				ProcessEnforceRoleSequenceValidityForRolePlayerAdd(link);
			}
			else
			{
				ProcessEnforceRoleSequenceValidityForRolePlayerDelete(link, null);
				// Both add and delete end up calling the same delay validation routine, just run one of them
				// EnforceRoleSequenceValidityForRolePlayerAdd.Process(link);
			}
		}
		#endregion // Error synchronization rules
		#region IModelErrorDisplayContext Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorDisplayContext.ErrorDisplayContext"/>
		/// </summary>
		protected string ErrorDisplayContext
		{
			get
			{
				ORMModel model = Model;
				return string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorDisplayContextSetConstraint, Name, model != null ? model.Name : "");
			}
		}
		string IModelErrorDisplayContext.ErrorDisplayContext
		{
			get
			{
				return ErrorDisplayContext;
			}
		}
		#endregion // IModelErrorDisplayContext Implementation
	}
	public partial class SetConstraint : IConstraint
	{
		#region IConstraint Implementation
		ORMModel IConstraint.Model
		{
			get
			{
				return Model;
			}
		}
		/// <summary>
		/// Implements IConstraint.ConstraintStorageStyle
		/// </summary>
		protected ConstraintStorageStyle ConstraintStorageStyle
		{
			get
			{
				return ConstraintStorageStyle.SetConstraint;
			}
		}
		ConstraintStorageStyle IConstraint.ConstraintStorageStyle
		{
			get
			{
				return ConstraintStorageStyle;
			}
		}
		/// <summary>
		/// Implements IConstraint.ConstraintIsInternal
		/// </summary>
		protected static bool ConstraintIsInternal
		{
			get
			{
				return false;
			}
		}
		bool IConstraint.ConstraintIsInternal
		{
			get
			{
				return ConstraintIsInternal;
			}
		}
		/// <summary>
		/// Implements IConstraint.ConstraintIsImplied
		/// </summary>
		protected static bool ConstraintIsImplied
		{
			get
			{
				return false;
			}
		}
		bool IConstraint.ConstraintIsImplied
		{
			get
			{
				return ConstraintIsImplied;
			}
		}
		ConstraintType IConstraint.ConstraintType
		{
			get
			{
				Debug.Fail("Implement on derived class");
				throw new NotImplementedException();
			}
		}
		RoleSequenceStyles IConstraint.RoleSequenceStyles
		{
			get
			{
				Debug.Fail("Implement on derived class");
				throw new NotImplementedException();
			}
		}
		ObjectType IConstraint.PreferredIdentifierFor
		{
			get
			{
				return null;
			}
			set
			{
			}
		}
		/// <summary>
		/// Implements IConstraint.ValidateColumnCompatibility
		/// </summary>
		protected void ValidateColumnCompatibility()
		{
			if (!IsDeleting && !IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(this, DelayValidateCompatibleRolePlayerTypeError);
			}
		}
		void IConstraint.ValidateColumnCompatibility()
		{
			ValidateColumnCompatibility();
		}
		/// <summary>
		/// Implements IConstraint.GetIntersectingConstraintValidationInfo.
		/// Default implementation returns an empty IList.
		/// </summary>
		protected static IList<IntersectingConstraintValidation> GetIntersectingConstraintValidationInfo()
		{
			return null;
		}
		IList<IntersectingConstraintValidation> IConstraint.GetIntersectingConstraintValidationInfo()
		{
			return GetIntersectingConstraintValidationInfo();
		}
		#endregion // IConstraint Implementation
		#region Pattern Rules
		/// <summary>
		/// AddRule: typeof(ConstraintRoleSequenceHasRole)
		/// </summary>
		private static void SetConstraintRoleSequenceHasRoleAddedRule(ElementAddedEventArgs e)
		{
			ProcessSetConstraintPattern((e.ModelElement as ConstraintRoleSequenceHasRole).ConstraintRoleSequence as SetConstraint, false);
		}
		/// <summary>
		/// DeletingRule: typeof(ConstraintRoleSequenceHasRole)
		/// </summary>
		private static void SetConstraintRoleSequenceHasRoleDeletingRule(ElementDeletingEventArgs e)
		{
			ProcessSetConstraintPattern((e.ModelElement as ConstraintRoleSequenceHasRole).ConstraintRoleSequence as SetConstraint, false);
		}
		/// <summary>
		/// ChangeRule: typeof(SetConstraint)
		/// Verify pattern modality changes
		/// </summary>
		private static void ModalityChangeRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == SetConstraint.ModalityDomainPropertyId)
			{
				ProcessSetConstraintPattern(e.ModelElement as SetConstraint, true);
			}
		}
		private static void ProcessSetConstraintPattern(SetConstraint setConstraint, bool modalityChange)
		{
			if (setConstraint != null)
			{
				IConstraint constraint = setConstraint.Constraint;
				ConstraintType constraintType = constraint.ConstraintType;
				IList<IntersectingConstraintValidation> validations = constraint.GetIntersectingConstraintValidationInfo();
				if (validations == null)
				{
					return;
				}
				int validationCount = validations.Count;
				ConstraintModality modality = setConstraint.Modality;
				for (int iValidation = 0; iValidation < validationCount; ++iValidation)
				{
					IntersectingConstraintValidation validationInfo = validations[iValidation];
					if (modalityChange && IntersectingConstraintPatternOptions.IntersectingConstraintModalityIgnored == (validationInfo.IntersectionValidationOptions & IntersectingConstraintPatternOptions.IntersectingConstraintModalityMask))
					{
						continue;
					}
					ConstraintType[] intersectingTypes = validationInfo.ConstraintTypesInPotentialConflict;
					switch (validationInfo.IntersectionValidationPattern)
					{
						case IntersectingConstraintPattern.SetConstraintSubset:
						case IntersectingConstraintPattern.SetConstraintSameSet:
							FrameworkDomainModel.DelayValidateElement(setConstraint, DelayValidateSetConstraintSubsetPattern);
							if (Array.IndexOf<ConstraintType>(intersectingTypes, constraintType) != -1)
							{
								// Any constraint of one of the listed types that intersects this one
								// must also be validated.
								IList<Role> roles = setConstraint.RoleCollection;
								int roleCount = roles.Count;
								for (int i = 0; i < roleCount; ++i)
								{
									Role selectedRole = roles[i];
									LinkedElementCollection<ConstraintRoleSequence> sequences = selectedRole.ConstraintRoleSequenceCollection;
									int sequenceCount = sequences.Count;
									for (int j = 0; j < sequenceCount; ++j)
									{
										ConstraintRoleSequence eligibleSequence = sequences[j];
										if (eligibleSequence != setConstraint)
										{
											IConstraint eligibleConstraint = eligibleSequence.Constraint;
											if (eligibleConstraint.ConstraintStorageStyle == ConstraintStorageStyle.SetConstraint &&
												(modalityChange || validationInfo.TestModality(eligibleConstraint.Modality, modality)) &&
												Array.IndexOf<ConstraintType>(validationInfo.ConstraintTypesInPotentialConflict, constraintType) != -1)
											{
												// The delayed validation mechanism automatically takes care of any duplicate requests
												FrameworkDomainModel.DelayValidateElement(eligibleSequence, DelayValidateSetConstraintSubsetPattern);
											}
										}
									}
								}
							}
							break;
						default:
							// Isn't a SetConstraint, move on
							break;
					}
				}
			}
			return;
		}
		#endregion
		#region Constraint Pattern Validation
		/// <summary>
		/// Validator callback for SetConstraint Subset Pattern
		/// </summary>
		protected static void DelayValidateSetConstraintSubsetPattern(ModelElement element)
		{
			(element as SetConstraint).ValidateSetConstraintSubsetPattern(null);
		}

		/// <summary>
		/// Validates the SetConstraint Subset Pattern
		/// </summary>
		protected void ValidateSetConstraintSubsetPattern(INotifyElementAdded notifyAdded)
		{
			IList<IntersectingConstraintValidation> validations = this.Constraint.GetIntersectingConstraintValidationInfo();
			if (validations == null)
			{
				return;
			}
			ConstraintModality modality = Modality;
			int validationCount = validations.Count;
			for (int i = 0; i < validationCount; ++i)
			{
				IntersectingConstraintValidation validationInfo = validations[i];
				bool sameSet;
				switch (validationInfo.IntersectionValidationPattern)
				{
					case IntersectingConstraintPattern.SetConstraintSubset:
						sameSet = false;
						break;
					case IntersectingConstraintPattern.SetConstraintSameSet:
						sameSet = true;
						break;
					default:
						continue;
				}
				bool hasError = false;
				LinkedElementCollection<Role> constraintRoles = this.RoleCollection;
				int constraintRoleCount = constraintRoles.Count;
				for (int iConstraintRole = 0; iConstraintRole < constraintRoleCount; ++iConstraintRole)
				{
					Role constraintRole = constraintRoles[iConstraintRole];
					LinkedElementCollection<ConstraintRoleSequence> intersectingSequences = constraintRole.ConstraintRoleSequenceCollection;
					int intersectingSequenceCount = intersectingSequences.Count;
					for (int iIntersectingSequence = 0; !hasError && iIntersectingSequence < intersectingSequenceCount; ++iIntersectingSequence)
					{
						ConstraintRoleSequence intersectingSequence = intersectingSequences[iIntersectingSequence];
						if (intersectingSequence != this)
						{
							IConstraint intersectingConstraint = intersectingSequence.Constraint;
							if (validationInfo.TestModality(modality, intersectingConstraint.Modality) &&
								(validationInfo.ConstraintTypesInPotentialConflict as IList<ConstraintType>).Contains(intersectingConstraint.ConstraintType))
							{
								SetConstraint intersectingSetConstraint = (SetConstraint)intersectingSequence;
								LinkedElementCollection<Role> intersectingRoles = intersectingSetConstraint.RoleCollection;
								int intersectingRoleCount = intersectingRoles.Count;
								if (sameSet ? intersectingRoleCount == constraintRoleCount : intersectingRoleCount <= constraintRoleCount) // Can't be a subset if the count is greater
								{
									hasError = true; // Assume we have the problem, disprove it
									for (int iIntersectingRole = 0; hasError && iIntersectingRole < intersectingRoleCount; ++iIntersectingRole)
									{
										Role intersectingRole = intersectingRoles[iIntersectingRole];
										// Finding a role that is not contained in the set of roles for this constraint
										// means that it is not a true subset.
										if (intersectingRole != constraintRole && constraintRoles.IndexOf(intersectingRole) == -1)
										{
											hasError = false;
										}
									}
								}
							}
						}
					}
					Guid domainRoleErrorId = validationInfo.DomainRoleToError.Value;
					Store store = Store;
					ModelError error = (ModelError)DomainRoleInfo.GetLinkedElement(this, domainRoleErrorId);
					if (hasError)
					{
						if (error == null)
						{
							error = (ModelError)store.ElementFactory.CreateElement(store.DomainDataDirectory.FindDomainRole(domainRoleErrorId).OppositeDomainRole.RolePlayer);
							DomainRoleInfo.SetLinkedElement(this, domainRoleErrorId, error);
							error.Model = Constraint.Model;
							error.GenerateErrorText();
							if (notifyAdded != null)
							{
								notifyAdded.ElementAdded(error, true);
							}
						}
					}
					else if (error != null)
					{
						error.Delete();
					}
				}
			}
		}
		#endregion
	}
	public partial class SetConstraint : IHierarchyContextEnabled
	{
		#region IHierarchyContextEnabled Members
		/// <summary>
		/// Implements <see cref="IHierarchyContextEnabled.ForwardHierarchyContextTo"/>
		/// </summary>
		protected static IHierarchyContextEnabled ForwardHierarchyContextTo
		{
			get
			{
				return null;
			}
		}
		IHierarchyContextEnabled IHierarchyContextEnabled.ForwardHierarchyContextTo
		{
			get
			{
				return ForwardHierarchyContextTo;
			}
		}
		/// <summary>
		/// Implements <see cref="IHierarchyContextEnabled.GetForcedHierarchyContextElements"/>
		/// </summary>
		protected static IEnumerable<IHierarchyContextEnabled> GetForcedHierarchyContextElements(bool minimalElements)
		{
			return null;
		}
		IEnumerable<IHierarchyContextEnabled> IHierarchyContextEnabled.GetForcedHierarchyContextElements(bool minimalElements)
		{
			return GetForcedHierarchyContextElements(minimalElements);
		}
		/// <summary>
		/// Implements <see cref="IHierarchyContextEnabled.HierarchyContextPlacementPriority"/>
		/// </summary>
		protected static HierarchyContextPlacementPriority HierarchyContextPlacementPriority
		{
			get
			{
				return HierarchyContextPlacementPriority.VeryLow;
			}
		}
		HierarchyContextPlacementPriority IHierarchyContextEnabled.HierarchyContextPlacementPriority
		{
			get
			{
				return HierarchyContextPlacementPriority;
			}
		}
		/// <summary>
		/// Implements <see cref="IHierarchyContextEnabled.HierarchyContextDecrementCount"/>
		/// </summary>
		protected static int HierarchyContextDecrementCount
		{
			get
			{
				return 0;
			}
		}
		int IHierarchyContextEnabled.HierarchyContextDecrementCount
		{
			get
			{
				return HierarchyContextDecrementCount;
			}
		}
		/// <summary>
		/// Implements <see cref="IHierarchyContextEnabled.ContinueWalkingHierarchyContext"/>
		/// </summary>
		protected static bool ContinueWalkingHierarchyContext
		{
			get
			{
				return false;
			}
		}
		bool IHierarchyContextEnabled.ContinueWalkingHierarchyContext
		{
			get
			{
				return ContinueWalkingHierarchyContext;
			}
		}
		#endregion
	}
	#endregion // SetConstraint class
	#region SetComparisonConstraint class
	public partial class SetComparisonConstraint : IModelErrorOwner
	{
		#region SetComparisonConstraint Specific
		/// <summary>
		/// Get a read-only list of FactConstraint links. To get the
		/// fact type from here, use the FactTypeCollection property on the returned
		/// object. To get to the roles, use the ConstrainedRoleCollection property.
		/// </summary>
		public ReadOnlyCollection<FactConstraint> FactConstraintCollection
		{
			get
			{
				return DomainRoleInfo.GetElementLinks<FactConstraint>(this, FactSetComparisonConstraint.SetComparisonConstraintDomainRoleId);
			}
		}
		/// <summary>
		/// Ensure that an FactConstraint exists between the
		/// fact type owning the passed in role and this constraint.
		/// FactConstraint links are generated automatically
		/// and should never be directly created.
		/// </summary>
		/// <param name="role">The role to attach</param>
		/// <param name="createdAndInitialized">Returns true if creating the
		/// relationship initialized it indirectly.</param>
		/// <returns>The associated FactConstraint relationship.</returns>
		private FactConstraint EnsureFactConstraintForRole(Role role, out bool createdAndInitialized)
		{
			createdAndInitialized = false;
			FactConstraint retVal = null;
			FactType fact = role.FactType;
			if (fact != null)
			{
				ReadOnlyCollection<FactSetComparisonConstraint> existingFactConstraints = DomainRoleInfo.GetElementLinks<FactSetComparisonConstraint>(fact, FactSetComparisonConstraint.FactTypeDomainRoleId);
				int listCount = existingFactConstraints.Count;
				for (int i = 0; i < listCount; ++i)
				{
					FactSetComparisonConstraint testFactConstraint = existingFactConstraints[i];
					if (testFactConstraint.SetComparisonConstraint == this)
					{
						retVal = testFactConstraint;
						break;
					}
				}
				if (retVal == null)
				{
					retVal = new FactSetComparisonConstraint(fact, this);
					createdAndInitialized = retVal.ConstrainedRoleCollection.Count != 0;
				}
			}
			return retVal;
		}
		#endregion // SetComparisonConstraint Specific
		#region CustomStorage Handling
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
		private string GetDefinitionTextValue()
		{
			Definition currentDefinition = Definition;
			return (currentDefinition != null) ? currentDefinition.Text : String.Empty;
		}
		private string GetNoteTextValue()
		{
			Note currentNote = Note;
			return (currentNote != null) ? currentNote.Text : String.Empty;
		}
		#endregion // CustomStorage Handling
		#region SetComparisonConstraint synchronization rules
		/// <summary>
		/// Make sure the model for the constraint and fact are consistent
		/// </summary>
		private static void EnforceNoForeignFactTypes(FactSetComparisonConstraint link)
		{
			FactType fact = link.FactType;
			SetComparisonConstraint constraint = link.SetComparisonConstraint;
			ORMModel factModel = fact.Model;
			ORMModel constraintModel = constraint.Model;
			if (factModel != null)
			{
				if (constraintModel == null)
				{
					constraint.Model = factModel;
				}
				else if (factModel != constraintModel)
				{
					throw new InvalidOperationException(ResourceStrings.ModelExceptionConstraintEnforceNoForeignFactTypes);
				}
			}
			else if (constraintModel != null)
			{
				fact.Model = constraintModel;
			}
		}
		/// <summary>
		/// AddRule: typeof(FactSetComparisonConstraint)
		/// Ensure that a fact type and constraint have a consistent owning model
		/// </summary>
		private static void FactSetComparisonConstraintAddedRule(ElementAddedEventArgs e)
		{
			EnforceNoForeignFactTypes(e.ModelElement as FactSetComparisonConstraint);
		}
		/// <summary>
		/// AddRule: typeof(ModelHasFactType)
		/// Ensure that a newly added fact type that is already attached to constraints
		/// has a consistent model for the constraints
		/// </summary>
		private static void FactTypeAddedRule(ElementAddedEventArgs e)
		{
			ModelHasFactType link = e.ModelElement as ModelHasFactType;
			ReadOnlyCollection<FactSetComparisonConstraint> existingConstraintLinks = DomainRoleInfo.GetElementLinks<FactSetComparisonConstraint>(link.FactType, FactSetComparisonConstraint.FactTypeDomainRoleId);
			int existingLinksCount = existingConstraintLinks.Count;
			for (int i = 0; i < existingLinksCount; ++i)
			{
				EnforceNoForeignFactTypes(existingConstraintLinks[i]);
			}
		}
		/// <summary>
		/// AddRule: typeof(ConstraintRoleSequenceHasRole)
		/// If a role is added after the role sequence is already attached,
		/// then create the corresponding FactConstraint and ExternalRoleConstraint
		/// </summary>
		private static void ConstraintRoleSequenceHasRoleAddedRule(ElementAddedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			SetComparisonConstraint constraint = link.ConstraintRoleSequence.Constraint as SetComparisonConstraint;
			if (constraint != null && constraint.Model != null)
			{
				bool createdAndInitialized;
				FactConstraint factConstraint = constraint.EnsureFactConstraintForRole(link.Role, out createdAndInitialized);
				if (factConstraint != null && !createdAndInitialized)
				{
					factConstraint.ConstrainedRoleCollection.Add(link);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(SetComparisonConstraintHasRoleSequence)
		/// If a role sequence is added that already contains roles, then
		/// make sure the corresponding FactConstraint and ExternalRoleConstraint
		/// objects are created for each role.
		/// </summary>
		private static void ConstraintHasRoleSequenceAddedRule(ElementAddedEventArgs e)
		{
			SetComparisonConstraintHasRoleSequence link = (SetComparisonConstraintHasRoleSequence)e.ModelElement;
			EnsureFactConstraintForRoleSequence(link);
			SetComparisonConstraintRoleSequence newSequence = link.RoleSequence;
			SetComparisonConstraint constraint = link.ExternalConstraint;
			foreach (SetComparisonConstraintRoleSequence sequence in constraint.RoleSequenceCollection)
			{
				if (newSequence == null)
				{
					// We have sequences after the new one, reset any numbered errors on them
					FrameworkDomainModel.DelayValidateElement(constraint, DelayRenumberErrorsWithSequenceNumbers);
					break;
				}
				else if (newSequence == sequence)
				{
					newSequence = null;
				}
			}
		}
		/// <summary>
		/// Helper function to support the same fact constraint fixup
		/// during both deserialization and rules.
		/// </summary>
		/// <param name="link">A roleset link added to the constraint</param>
		private static void EnsureFactConstraintForRoleSequence(SetComparisonConstraintHasRoleSequence link)
		{
			SetComparisonConstraintRoleSequence roleSequence = link.RoleSequence;
			// The following line gets the links instead of the counterparts,
			// which are provided by roleSequence.RoleCollection
			ReadOnlyCollection<ConstraintRoleSequenceHasRole> roleLinks = DomainRoleInfo.GetElementLinks<ConstraintRoleSequenceHasRole>(roleSequence, ConstraintRoleSequenceHasRole.ConstraintRoleSequenceDomainRoleId);
			int roleCount = roleLinks.Count;
			if (roleCount != 0)
			{
				SetComparisonConstraint constraint = link.ExternalConstraint;
				for (int i = 0; i < roleCount; ++i)
				{
					ConstraintRoleSequenceHasRole roleLink = roleLinks[i];
					bool createdAndInitialized;
					FactConstraint factConstraint = constraint.EnsureFactConstraintForRole(roleLink.Role, out createdAndInitialized);
					if (factConstraint != null && !createdAndInitialized)
					{
						LinkedElementCollection<ConstraintRoleSequenceHasRole> constrainedRoles = factConstraint.ConstrainedRoleCollection;
						if (!constrainedRoles.Contains(roleLink))
						{
							constrainedRoles.Add(roleLink);
						}
					}
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ExternalRoleConstraint), FireTime=LocalCommit, Priority=ORMCoreDomainModel.BeforeDelayValidateRulePriority;
		/// Rip a FactConstraint relationship when its last role
		/// goes away. Note that this rule also affects single column external
		/// constraints, but we only need to write it once.
		/// </summary>
		private static void ExternalRoleConstraintDeletedRule(ElementDeletedEventArgs e)
		{
			ExternalRoleConstraint link = e.ModelElement as ExternalRoleConstraint;
			FactConstraint factConstraint = link.FactConstraint;
			if (!factConstraint.IsDeleted)
			{
				if (factConstraint.ConstrainedRoleCollection.Count == 0)
				{
					factConstraint.Delete();
				}
			}
		}
		#region SetComparisonConstraintRoleSequenceDeletedRule
		/// <summary>
		/// DeleteRule: typeof(SetComparisonConstraintHasRoleSequence), FireTime=LocalCommit, Priority=ORMCoreDomainModel.BeforeDelayValidateRulePriority;
		/// Rule that fires when a set comparison constraint has a role seqeuence removed.
		/// The constraint itself is removed when the last sequence is removed.
		/// </summary>
		private static void SetComparisonConstraintRoleSequenceDeletedRule(ElementDeletedEventArgs e)
		{
			SetComparisonConstraintHasRoleSequence link = e.ModelElement as SetComparisonConstraintHasRoleSequence;
			SetComparisonConstraint constraint = link.ExternalConstraint;
			if (!constraint.IsDeleted)
			{
				if (constraint.RoleSequenceCollection.Count == 0)
				{
					constraint.Delete();
				}
				else
				{
					FrameworkDomainModel.DelayValidateElement(constraint, DelayRenumberErrorsWithSequenceNumbers);
				}
			}
		}
		/// <summary>
		/// RolePlayerPositionChangeRule: typeof(SetComparisonConstraintHasRoleSequence)
		/// Renumber validation errors when sequences are reordered.
		/// </summary>
		private static void SetComparisonConstraintRoleSequencePositionChangeRule(RolePlayerOrderChangedEventArgs e)
		{
			if (e.SourceDomainRole.Id == SetComparisonConstraintHasRoleSequence.ExternalConstraintDomainRoleId)
			{
				FrameworkDomainModel.DelayValidateElement(e.SourceElement, DelayRenumberErrorsWithSequenceNumbers);
			}
		}
		/// <summary>
		/// Reset numbers on errors associated with contained role sequences
		/// </summary>
		private static void DelayRenumberErrorsWithSequenceNumbers(ModelElement element)
		{
			if (element.IsDeleted)
			{
				return;
			}
			foreach (ConstraintRoleSequence roleSequence in ((SetComparisonConstraint)element).RoleSequenceCollection)
			{
				foreach (ModelError error in ((IModelErrorOwner)roleSequence).GetErrorCollection(ModelErrorUses.None))
				{
					error.GenerateErrorText();
				}
			}
		}
		#endregion //SetComparisonConstraintRoleSequenceDeletedRule
		#endregion // SetComparisonConstraint synchronization rules
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// adds the implicit FactConstraint elements.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new ExternalConstraintFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation. Adds implicit FactConstraint relationships
		/// </summary>
		private sealed class ExternalConstraintFixupListener : DeserializationFixupListener<SetComparisonConstraint>
		{
			/// <summary>
			/// ExternalConstraintFixupListener constructor
			/// </summary>
			public ExternalConstraintFixupListener()
				: base((int)ORMDeserializationFixupPhase.AddImplicitElements)
			{
			}
			/// <summary>
			/// Process elements by added an FactConstraint for
			/// each roleset
			/// </summary>
			/// <param name="element">An ExternalConstraint element</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(SetComparisonConstraint element, Store store, INotifyElementAdded notifyAdded)
			{
				ReadOnlyCollection<SetComparisonConstraintHasRoleSequence> links = DomainRoleInfo.GetElementLinks<SetComparisonConstraintHasRoleSequence>(element, SetComparisonConstraintHasRoleSequence.ExternalConstraintDomainRoleId);
				int linksCount = links.Count;
				for (int i = 0; i < linksCount; ++i)
				{
					EnsureFactConstraintForRoleSequence(links[i]);
				}
				ReadOnlyCollection<FactSetComparisonConstraint> factLinks = DomainRoleInfo.GetElementLinks<FactSetComparisonConstraint>(element, FactSetComparisonConstraint.SetComparisonConstraintDomainRoleId);
				int factLinksCount = factLinks.Count;
				// Validate subtype constraint patterns before sending additional element added notifications.
				// The XML schema is current very lax on exclusion constraints on Sub/SupertypeMetaRoles, so
				// any constraints on any combination of roles/metaroles load successfully. Eliminate any
				// constraints with patterns that will raise exceptions inside the tool.
				bool seenSubtypeFact = false;
				bool invalidSubtypeConstraint = false;
				for (int j = 0; j < factLinksCount; ++j)
				{
					FactSetComparisonConstraint factLink = factLinks[j];
					if (factLink.FactType is SubtypeFact)
					{
						if (!seenSubtypeFact)
						{
							if (j > 0)
							{
								invalidSubtypeConstraint = true;
							}
							seenSubtypeFact = true;
							if ((element as IConstraint).ConstraintType != ConstraintType.Exclusion)
							{
								invalidSubtypeConstraint = true;
								break;
							}
						}
						LinkedElementCollection<ConstraintRoleSequenceHasRole> roleLinks = factLink.ConstrainedRoleCollection;
						int roleLinksCount = roleLinks.Count;
						if (roleLinks.Count != 1 || !(roleLinks[0].Role is SupertypeMetaRole))
						{
							invalidSubtypeConstraint = true;
							break;
						}
					}
					else if (seenSubtypeFact)
					{
						invalidSubtypeConstraint = true;
						break;
					}
				}
				if (invalidSubtypeConstraint)
				{
					// All of the relationships we're dealing with here have delete
					// progation set, so we do not need additional rules or code to
					// remove the elements.
					element.Delete();
					return;
				}
				else if (seenSubtypeFact)
				{
					LinkedElementCollection<SetComparisonConstraintRoleSequence> sequences = element.RoleSequenceCollection;
					if (factLinksCount != sequences.Count)
					{
						// All of the roles pass muster, but they are arranged in an
						// invalid pattern. The exclusion constraint must be single
						// column to be valid. Break down and rebuild the constraint
						// using supertype roles from the subtype facts.
						SubtypeFact[] subtypeFacts = new SubtypeFact[factLinksCount];
						for (int j = 0; j < factLinksCount; ++j)
						{
							subtypeFacts[j] = (SubtypeFact)factLinks[j].FactType;
						}
						sequences.Clear();
						for (int j = 0; j < factLinksCount; ++j)
						{
							SetComparisonConstraintRoleSequence sequence = new SetComparisonConstraintRoleSequence(store);
							sequence.RoleCollection.Add(subtypeFacts[j].SupertypeRole);
							EnsureFactConstraintForRoleSequence(new SetComparisonConstraintHasRoleSequence(element, sequence));
						}
						factLinks = DomainRoleInfo.GetElementLinks<FactSetComparisonConstraint>(element, FactSetComparisonConstraint.SetComparisonConstraintDomainRoleId);
					}
				}
				for (int j = 0; j < factLinksCount; ++j)
				{
					// Notify that the link was added. Note that we set
					// addLinks to true here because we expect ExternalRoleConstraint
					// links to be attached to each FactConstraint
					notifyAdded.ElementAdded(factLinks[j], true);
				}
			}
		}
		#endregion // Deserialization Fixup
		#region Error synchronization rules
		#region VerifyRoleSequenceCountForRule
		/// <summary>
		/// Validator callback for CompatibleRolePlayerTypeError
		/// </summary>
		private static void DelayValidateRoleSequenceCountErrors(ModelElement element)
		{
			(element as SetComparisonConstraint).VerifyRoleSequenceCountForRule(null);
		}
		/// <summary>
		/// Add, remove, and otherwise validate the current set of
		/// errors for this constraint.
		/// </summary>
		/// <param name="notifyAdded">If not null, this is being called during
		/// load when rules are not in place. Any elements that are added
		/// must be notified back to the caller.</param>
		private void VerifyRoleSequenceCountForRule(INotifyElementAdded notifyAdded)
		{
			if (!IsDeleted)
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
					if (null == this.TooFewRoleSequencesError)
					{
						insufficientError = new TooFewRoleSequencesError(store);
						insufficientError.SetComparisonConstraint = this;
						insufficientError.Model = Model;
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
							extraError = new TooManyRoleSequencesError(store);
							extraError.SetComparisonConstraint = this;
							extraError.Model = Model;
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
					insufficientError.Delete();
				}
				if (removeTooMany && null != (extraError = TooManyRoleSequencesError))
				{
					extraError.Delete();
				}

				VerifyRoleSequenceArityForRule(notifyAdded, tooFewOrTooMany);
			}
		}
		#endregion // VerifyRoleSequenceCountForRule
		#region VerifyRoleSequenceArityForRule
		/// <summary>
		/// Validator callback for ArityMismatchError
		/// </summary>
		private static void DelayValidateArityMismatchError(ModelElement element)
		{
			(element as SetComparisonConstraint).VerifyRoleSequenceArityForRule(null);
		}
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
					arityError.Delete(); // Can't validate arity with the wrong number of role sequences
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
								arityError = new ExternalConstraintRoleSequenceArityMismatchError(store);
								arityError.Constraint = this;
								arityError.Model = Model;
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
						arityError.Delete();
					}
				}
			}
			VerifyCompatibleRolePlayerTypeForRule(notifyAdded, tooFewOrTooManySequences || !arityValid);
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
				LinkedElementCollection<SetComparisonConstraintRoleSequence> sequences = RoleSequenceCollection;
				int sequenceCount = sequences.Count;

				if (sequenceCount > 1)
				{
					// Cache the role collection so we're not regenerating them all the time
					LinkedElementCollection<Role>[] roleCollections = new LinkedElementCollection<Role>[sequenceCount];
					for (int i = 0; i < sequenceCount; ++i)
					{
						roleCollections[i] = sequences[i].RoleCollection;
					}

					LinkedElementCollection<CompatibleRolePlayerTypeError> startingErrors = CompatibleRolePlayerTypeErrorCollection;
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
										ObjectType.WalkSupertypes(firstRolePlayer, delegate(ObjectType type, int depth, bool isPrimary)
										{
											superTypesCache.Add(type);
											return ObjectTypeVisitorResult.Continue;
										});
									}
									// If the type is contained, WalkSupertype will return false because the iteration
									// did not complete.
									bool isCompatible = !ObjectType.WalkSupertypes(currentRolePlayer, delegate(ObjectType type, int depth, bool isPrimary)
									{
										// Continue iteration if the type is recognized in the cache
										return superTypesCache.Contains(type) ? ObjectTypeVisitorResult.Stop : ObjectTypeVisitorResult.Continue;
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
											compatibleError = new CompatibleRolePlayerTypeError(store);
											compatibleError.Column = column;
											compatibleError.SetComparisonConstraint = this;
											compatibleError.Model = Model;
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
						startingErrors[i].Delete();
					}
				}
				else
				{
					CompatibleRolePlayerTypeErrorCollection.Clear();
				}
			}
		}
		/// <summary>
		/// Validator callback for CompatibleRolePlayerTypeError
		/// </summary>
		[DelayValidatePriority(1)] // Run after arity validation is complete
		private static void DelayValidateCompatibleRolePlayerTypeError(ModelElement element)
		{
			(element as SetComparisonConstraint).VerifyCompatibleRolePlayerTypeForRule(null);
		}
		private void VerifyCompatibleRolePlayerTypeForRule(INotifyElementAdded notifyAdded)
		{
			VerifyCompatibleRolePlayerTypeForRule(
				notifyAdded,
				!(null == TooFewRoleSequencesError && null == TooManyRoleSequencesError && null == ArityMismatchError));
		}
		#endregion // VerifyCompatibleRolePlayerTypeForRule
		#region Add/Remove Rules
		/// <summary>
		/// AddRule: typeof(SetComparisonConstraintHasRoleSequence)
		/// </summary>
		private static void EnforceRoleSequenceCardinalityForSequenceAddRule(ElementAddedEventArgs e)
		{
			SetComparisonConstraintHasRoleSequence link = e.ModelElement as SetComparisonConstraintHasRoleSequence;
			FrameworkDomainModel.DelayValidateElement(link.ExternalConstraint, DelayValidateRoleSequenceCountErrors);
		}
		/// <summary>
		/// AddRule: typeof(ModelHasSetComparisonConstraint)
		/// Do initial validation when a new SetComparisonConstraint is added to a model
		/// </summary>
		private static void EnforceRoleSequenceCardinalityForConstraintAddRule(ElementAddedEventArgs e)
		{
			ModelHasSetComparisonConstraint link = e.ModelElement as ModelHasSetComparisonConstraint;
			IModelErrorOwner errorOwner = link.SetComparisonConstraint as IModelErrorOwner;
			if (errorOwner != null)
			{
				errorOwner.DelayValidateErrors();
			}
		}
		/// <summary>
		/// DeleteRule: typeof(SetComparisonConstraintHasRoleSequence)
		/// </summary>
		private static void EnforceRoleSequenceCardinalityForSequenceDeleteRule(ElementDeletedEventArgs e)
		{
			SetComparisonConstraintHasRoleSequence link = e.ModelElement as SetComparisonConstraintHasRoleSequence;
			SetComparisonConstraint externalConstraint = link.ExternalConstraint;
			if (externalConstraint != null && !externalConstraint.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(externalConstraint, DelayValidateRoleSequenceCountErrors);
			}
		}
		/// <summary>
		/// AddRule: typeof(ConstraintRoleSequenceHasRole)
		/// Add Rule for arity and compatibility checking when SetComparisonConstraint roles are added
		/// </summary>
		private static void EnforceRoleSequenceValidityForRoleAddRule(ElementAddedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			SetComparisonConstraintRoleSequence sequence = link.ConstraintRoleSequence as SetComparisonConstraintRoleSequence;
			if (sequence != null)
			{
				SetComparisonConstraint constraint = sequence.ExternalConstraint;
				if (constraint != null)
				{
					FrameworkDomainModel.DelayValidateElement(constraint, DelayValidateArityMismatchError);
					FrameworkDomainModel.DelayValidateElement(constraint, DelayValidateCompatibleRolePlayerTypeError);
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleSequenceHasRole)
		/// Delete Rule for VerifyCompatibleRolePlayer when SetComparisonConstraint roles are removed
		/// </summary>
		private static void EnforceRoleSequenceValidityForRoleDeleteRule(ElementDeletedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			SetComparisonConstraintRoleSequence sequence = link.ConstraintRoleSequence as SetComparisonConstraintRoleSequence;
			if (sequence != null)
			{
				SetComparisonConstraint externalConstraint = sequence.ExternalConstraint;
				if (externalConstraint != null && !externalConstraint.IsDeleted)
				{
					FrameworkDomainModel.DelayValidateElement(externalConstraint, DelayValidateArityMismatchError);
					FrameworkDomainModel.DelayValidateElement(externalConstraint, DelayValidateCompatibleRolePlayerTypeError);
				}
			}
		}
		/// <summary>
		/// RolePlayerPositionChangeRule: typeof(ConstraintRoleSequenceHasRole)
		/// </summary>
		private static void EnforceRoleSequenceValidityForRoleReorderRule(RolePlayerOrderChangedEventArgs e)
		{
			SetComparisonConstraintRoleSequence sequence;
			if (e.SourceDomainRole.Id == ConstraintRoleSequenceHasRole.ConstraintRoleSequenceDomainRoleId &&
				null != (sequence = e.SourceElement as SetComparisonConstraintRoleSequence))
			{
				SetComparisonConstraint externalConstraint = sequence.ExternalConstraint;
				if (externalConstraint != null && !externalConstraint.IsDeleted)
				{
					FrameworkDomainModel.DelayValidateElement(externalConstraint, DelayValidateCompatibleRolePlayerTypeError);
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(ObjectTypePlaysRole)
		/// Add Rule for VerifyCompatibleRolePlayer when a Role/Object relationship is added
		/// </summary>
		private static void EnforceRoleSequenceValidityForRolePlayerAddRule(ElementAddedEventArgs e)
		{
			ProcessEnforceRoleSequenceValidityForRolePlayerAdd(e.ModelElement as ObjectTypePlaysRole);
		}
		/// <summary>
		/// Rule helper method
		/// </summary>
		private static void ProcessEnforceRoleSequenceValidityForRolePlayerAdd(ObjectTypePlaysRole link)
		{
			Role role = link.PlayedRole;
			LinkedElementCollection<ConstraintRoleSequence> roleSequences = role.ConstraintRoleSequenceCollection;
			int count = roleSequences.Count;
			for (int i = 0; i < count; ++i)
			{
				SetComparisonConstraintRoleSequence sequence = roleSequences[i] as SetComparisonConstraintRoleSequence;
				if (sequence != null)
				{
					SetComparisonConstraint externalConstraint = sequence.ExternalConstraint;
					if (externalConstraint != null)
					{
						FrameworkDomainModel.DelayValidateElement(externalConstraint, DelayValidateCompatibleRolePlayerTypeError);
					}
				}
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ObjectTypePlaysRole)
		/// Remove Rule for VerifyCompatibleRolePlayer when a Role/Object relationship is removed
		/// </summary>
		private static void EnforceRoleSequenceValidityForRolePlayerDeleteRule(ElementDeletedEventArgs e)
		{
			ProcessEnforceRoleSequenceValidityForRolePlayerDelete(e.ModelElement as ObjectTypePlaysRole, null);
		}
		/// <summary>
		/// Rule helper method
		/// </summary>
		private static void ProcessEnforceRoleSequenceValidityForRolePlayerDelete(ObjectTypePlaysRole link, Role role)
		{
			if (role == null)
			{
				role = link.PlayedRole;
			}
			if (!role.IsDeleted)
			{
				LinkedElementCollection<ConstraintRoleSequence> roleSequences = role.ConstraintRoleSequenceCollection;
				int count = roleSequences.Count;
				for (int i = 0; i < count; ++i)
				{
					SetComparisonConstraintRoleSequence sequence = roleSequences[i] as SetComparisonConstraintRoleSequence;
					if (sequence != null && !sequence.IsDeleted)
					{
						SetComparisonConstraint externalConstraint = sequence.ExternalConstraint;
						if (externalConstraint != null && !externalConstraint.IsDeleted)
						{
							FrameworkDomainModel.DelayValidateElement(externalConstraint, DelayValidateCompatibleRolePlayerTypeError);
						}
					}
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ObjectTypePlaysRole)
		/// Forward ObjectTypePlaysRole role player changes to corresponding Add/Delete rules
		/// </summary>
		private static void EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			ObjectTypePlaysRole link = e.ElementLink as ObjectTypePlaysRole;
			if (link.IsDeleted)
			{
				return;
			}
			Guid changedRoleGuid = e.DomainRole.Id;
			if (changedRoleGuid == ObjectTypePlaysRole.PlayedRoleDomainRoleId)
			{
				ProcessEnforceRoleSequenceValidityForRolePlayerDelete(link, (Role)e.OldRolePlayer);
				ProcessEnforceRoleSequenceValidityForRolePlayerAdd(link);
			}
			else
			{
				ProcessEnforceRoleSequenceValidityForRolePlayerDelete(link, null);
				// Both add and delete end up calling the same delay validation routine, just run one of them
				// EnforceRoleSequenceValidityForRolePlayerAdd.Process(link);
			}
		}
		#endregion // Add/Remove Rules
		#endregion // Error synchronization rules
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Implements IModelErrorOwner.GetErrorCollection
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			ModelErrorUses startFilter = filter;
			if (filter == 0)
			{
				filter = (ModelErrorUses)(-1);
			}
			if (0 != (filter & (ModelErrorUses.BlockVerbalization | ModelErrorUses.DisplayPrimary)))
			{
				TooManyRoleSequencesError tooMany;
				TooFewRoleSequencesError tooFew;
				ExternalConstraintRoleSequenceArityMismatchError arityMismatch;
				if (null != (tooMany = TooManyRoleSequencesError))
				{
					yield return new ModelErrorUsage(tooMany, ModelErrorUses.BlockVerbalization);
				}
				if (null != (tooFew = TooFewRoleSequencesError))
				{
					yield return new ModelErrorUsage(tooFew, ModelErrorUses.BlockVerbalization);
				}
				if (null != (arityMismatch = ArityMismatchError))
				{
					yield return arityMismatch;
				}
			}
			if (filter == ModelErrorUses.BlockVerbalization)
			{
				// We can't verbalize if there are constrained facts without readings or
				// any constrained roles without role players
				ReadOnlyCollection<FactSetComparisonConstraint> factLinks = FactSetComparisonConstraint.GetLinksToFactTypeCollection(this);
				int count = factLinks.Count;

				// Show the reading errors first
				for (int i = 0; i < count; ++i)
				{
					FactTypeRequiresReadingError noReadingError = factLinks[i].FactType.ReadingRequiredError;
					if (noReadingError != null)
					{
						yield return noReadingError;
					}
				}

				// Show the missing role errors
				for (int i = 0; i < count; ++i)
				{
					LinkedElementCollection<ConstraintRoleSequenceHasRole> constrainedRoles = factLinks[i].ConstrainedRoleCollection;
					int constrainedRoleCount = constrainedRoles.Count;
					bool seenErrorRole = false;
					for (int j = 0; j < constrainedRoleCount; ++j)
					{
						Role role = constrainedRoles[j].Role;

						RolePlayerRequiredError noRolePlayerError = role.RolePlayerRequiredError;
						if (noRolePlayerError != null)
						{
							if (seenErrorRole)
							{
								// Make sure we don't get a duplicate. This is rare but possible.
								int k = 0;
								for (; k < j; ++k)
								{
									if (constrainedRoles[k].Role == role)
									{
										break;
									}
								}
								if (k != j)
								{
									continue;
								}
							}
							seenErrorRole = true;
							yield return noRolePlayerError;
						}
					}
				}
			}
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
			{
				foreach (CompatibleRolePlayerTypeError compatibleTypeError in CompatibleRolePlayerTypeErrorCollection)
				{
					yield return compatibleTypeError;
				}
				ConstraintDuplicateNameError duplicateName = DuplicateNameError;
				if (duplicateName != null)
				{
					yield return duplicateName;
				}

				ImplicationError implicationError = ImplicationError;
				if (implicationError != null)
				{
					yield return implicationError;
				}
				EqualityOrSubsetImpliedByMandatoryError mandatoriesImply = EqualityOrSubsetImpliedByMandatoryError;
				if (mandatoriesImply != null)
				{
					yield return mandatoriesImply;
				}

				ExclusionContradictsEqualityError exclusionEqualityError = ExclusionContradictsEqualityError;
				if (exclusionEqualityError != null)
				{
					yield return exclusionEqualityError;
				}

				ExclusionContradictsSubsetError exclusionSubsetError = ExclusionContradictsSubsetError;
				if (exclusionSubsetError != null)
				{
					yield return exclusionSubsetError;
				}
			}
			// Display individual sequence errors
			foreach (SetComparisonConstraintRoleSequence roleSequence in RoleSequenceCollection)
			{
				foreach (ModelErrorUsage sequenceError in ((IModelErrorOwner)roleSequence).GetErrorCollection(startFilter))
				{
					yield return sequenceError;
				}
			}
			// Get errors off the base
			foreach (ModelErrorUsage baseError in base.GetErrorCollection(startFilter))
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
		/// Validate all errors on the external constraint. This
		/// is called during deserialization fixup when rules are
		/// suspended.
		/// </summary>
		/// <param name="notifyAdded">A callback for notifying
		/// the caller of all objects that are added.</param>
		protected new void ValidateErrors(INotifyElementAdded notifyAdded)
		{
			// Calls added here need corresponding delayed calls in DelayValidateErrors
			VerifyRoleSequenceCountForRule(notifyAdded);
			// VerifyRoleSequenceArityForRule(notifyAdded); // This is called by VeryRoleSequenceCountForRule
			// VerifyCompatibleRolePlayerTypeForRule(notifyAdded); // This is called by VerifyRoleSequenqeArityForRule
			SetComparisonConstraintRoleSequence.ValidateIntersectingConstraints(this, notifyAdded);
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
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateRoleSequenceCountErrors);
			// FrameworkDomainModel.DelayValidateElement(this, DelayValidateArityMismatchError); // This is called by DelayValidateRoleSequenceCountErrors
			// FrameworkDomainModel.DelayValidateElement(this, DelayValidateCompatibleRolePlayerTypeError); // This is called by DelayValidateArityMismatchError
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner Implementation
	}
	public partial class SetComparisonConstraint : IConstraint
	{
		#region IConstraint Implementation
		/// <summary>
		/// Implements IConstraint.GetIntersectingConstraintValidationInfo.
		/// Default implementation returns an empty IList.
		/// </summary>
		protected static IList<IntersectingConstraintValidation> GetIntersectingConstraintValidationInfo()
		{
			return null;
		}
		IList<IntersectingConstraintValidation> IConstraint.GetIntersectingConstraintValidationInfo()
		{
			return GetIntersectingConstraintValidationInfo();
		}
		ORMModel IConstraint.Model
		{
			get
			{
				return Model;
			}
		}
		/// <summary>
		/// Implements IConstraint.ConstraintStorageStyle
		/// </summary>
		protected ConstraintStorageStyle ConstraintStorageStyle
		{
			get
			{
				return ConstraintStorageStyle.SetComparisonConstraint;
			}
		}
		ConstraintStorageStyle IConstraint.ConstraintStorageStyle
		{
			get
			{
				return ConstraintStorageStyle;
			}
		}
		ConstraintType IConstraint.ConstraintType
		{
			get
			{
				Debug.Fail("Implement on derived class");
				throw new NotImplementedException();
			}
		}
		RoleSequenceStyles IConstraint.RoleSequenceStyles
		{
			get
			{
				Debug.Fail("Implement on derived class");
				throw new NotImplementedException();
			}
		}
		/// <summary>
		/// Implements IConstraint.ConstraintIsInternal
		/// </summary>
		protected static bool ConstraintIsInternal
		{
			get
			{
				return false;
			}
		}
		bool IConstraint.ConstraintIsInternal
		{
			get
			{
				return ConstraintIsInternal;
			}
		}
		/// <summary>
		/// Implements IConstraint.ConstraintIsImplied
		/// </summary>
		protected static bool ConstraintIsImplied
		{
			get
			{
				return false;
			}
		}
		bool IConstraint.ConstraintIsImplied
		{
			get
			{
				return ConstraintIsImplied;
			}
		}
		ObjectType IConstraint.PreferredIdentifierFor
		{
			get
			{
				return null;
			}
			set
			{
			}
		}
		/// <summary>
		/// Implements IConstraint.ValidateColumnCompatibility
		/// </summary>
		protected void ValidateColumnCompatibility()
		{
			if (!IsDeleted && !IsDeleting)
			{
				FrameworkDomainModel.DelayValidateElement(this, DelayValidateCompatibleRolePlayerTypeError);
			}
		}
		void IConstraint.ValidateColumnCompatibility()
		{
			ValidateColumnCompatibility();
		}
		#endregion // IConstraint Implementation
	}
	public partial class SetComparisonConstraint : IHierarchyContextEnabled
	{
		#region IHierarchyContextEnabled Members
		/// <summary>
		/// Implements <see cref="IHierarchyContextEnabled.ForwardHierarchyContextTo"/>
		/// </summary>
		protected static IHierarchyContextEnabled ForwardHierarchyContextTo
		{
			get
			{
				return null;
			}
		}
		IHierarchyContextEnabled IHierarchyContextEnabled.ForwardHierarchyContextTo
		{
			get
			{
				return ForwardHierarchyContextTo;
			}
		}
		/// <summary>
		/// Implements <see cref="IHierarchyContextEnabled.GetForcedHierarchyContextElements"/>
		/// </summary>
		protected static IEnumerable<IHierarchyContextEnabled> GetForcedHierarchyContextElements(bool minimalElements)
		{
			return null;
		}
		IEnumerable<IHierarchyContextEnabled> IHierarchyContextEnabled.GetForcedHierarchyContextElements(bool minimalElements)
		{
			return GetForcedHierarchyContextElements(minimalElements);
		}
		/// <summary>
		/// Implements <see cref="IHierarchyContextEnabled.HierarchyContextPlacementPriority"/>
		/// </summary>
		protected static HierarchyContextPlacementPriority HierarchyContextPlacementPriority
		{
			get
			{
				return HierarchyContextPlacementPriority.VeryLow;
			}
		}
		HierarchyContextPlacementPriority IHierarchyContextEnabled.HierarchyContextPlacementPriority
		{
			get
			{
				return HierarchyContextPlacementPriority;
			}
		}
		/// <summary>
		/// Implements <see cref="IHierarchyContextEnabled.HierarchyContextDecrementCount"/>
		/// </summary>
		protected static int HierarchyContextDecrementCount
		{
			get
			{
				return 0;
			}
		}
		int IHierarchyContextEnabled.HierarchyContextDecrementCount
		{
			get
			{
				return HierarchyContextDecrementCount;
			}
		}
		/// <summary>
		/// Implements <see cref="IHierarchyContextEnabled.ContinueWalkingHierarchyContext"/>
		/// </summary>
		protected static bool ContinueWalkingHierarchyContext
		{
			get
			{
				return false;
			}
		}
		bool IHierarchyContextEnabled.ContinueWalkingHierarchyContext
		{
			get
			{
				return ContinueWalkingHierarchyContext;
			}
		}
		#endregion
	}
	#endregion // SetComparisonConstraint class
	#region FactConstraint classes
	public partial class FactConstraint : IFactConstraint
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
		protected IList<Role> RoleCollection
		{
			get
			{
				LinkedElementCollection<ConstraintRoleSequenceHasRole> roleSequenceLinks = ConstrainedRoleCollection;
				int roleSequenceLinksCount = roleSequenceLinks.Count;
				Role[] typedList = new Role[roleSequenceLinksCount];
				for (int i = 0; i < roleSequenceLinksCount; ++i)
				{
					typedList[i] = roleSequenceLinks[i].Role;
				}
				return typedList;
			}
		}
		IConstraint IFactConstraint.Constraint
		{
			get
			{
				return (IConstraint)Constraint;
			}
		}
		#endregion // IFactConstraint Implementation
	}
	#endregion // FactConstraint classes
	#region ConstraintRoleSequence classes
	public partial class ConstraintRoleSequence : IModelErrorOwner
	{
		#region Rules
		/// <summary>
		/// Attach rule notifications for subtype hierarchy changes
		/// </summary>
		/// <param name="store">The context <see cref="Store"/></param>
		public static void EnableRuleNotifications(Store store)
		{
			ObjectType.AddSubtypeHierarchyChangeRuleNotification(store, ValidateAttachedConstraintColumnCompatibility);
		}
		/// <summary>
		/// Walk constraint attached to roles associated with a subtype change.
		/// </summary>
		private static void ValidateAttachedConstraintColumnCompatibility(ObjectType type)
		{
			LinkedElementCollection<Role> playedRoles = type.PlayedRoleCollection;
			int playedRoleCount = playedRoles.Count;
			for (int i = 0; i < playedRoleCount; ++i)
			{
				Role playedRole = playedRoles[i];
				if (!playedRole.IsDeleting)
				{
					DelayValidateFactTypeJoinPaths(playedRole.FactType, false);
					LinkedElementCollection<ConstraintRoleSequence> sequences = playedRole.ConstraintRoleSequenceCollection;
					int sequenceCount = sequences.Count;
					for (int j = 0; j < sequenceCount; ++j)
					{
						ConstraintRoleSequence sequence = sequences[j];
						if (!sequence.IsDeleting)
						{
							IConstraint constraint = sequence.Constraint;
							if (constraint != null &&
								0 != (constraint.RoleSequenceStyles & RoleSequenceStyles.CompatibleColumns))
							{
								constraint.ValidateColumnCompatibility();
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ConstraintRoleSequenceHasRole)
		/// Other rules are not set up to handle role player changes on ConstraintRoleSequence.
		/// The NORMA UI never attempts this operation.
		/// Throw if any attempt is made to directly modify roles on a ConstraintRoleSequence
		/// relationship after it has been created.
		/// </summary>
		private static void BlockRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			throw new InvalidOperationException(ResourceStrings.ModelExceptionConstraintRoleSequenceHasRoleEnforceNoRolePlayerChange);
		}
		/// <summary>
		/// AddRule: typeof(ConstraintRoleSequenceHasRole)
		/// </summary>
		private static void ConstraintRoleSequenceHasRoleAddedRule(ElementAddedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = (ConstraintRoleSequenceHasRole)e.ModelElement;
			ConstraintRoleSequence sequence = link.ConstraintRoleSequence;
			sequence.DelayValidatePatternError();
			sequence.DelayValidateJoinPath();
			SetComparisonConstraintRoleSequence setComparisonSequence = link.ConstraintRoleSequence as SetComparisonConstraintRoleSequence;
			if (setComparisonSequence != null)
			{
				ProcessSetComparisonConstraintPattern(
					setComparisonSequence.ExternalConstraint,
					link.Role,
					null,
					false,
					delegate(IConstraint matchConstraint)
					{
						DelayValidateConstraintPatternError(matchConstraint);
						return true;
					});
			}
		}
		/// <summary>
		/// DeletingRule: typeof(ConstraintRoleSequence)
		/// </summary>
		private static void SetComparisonConstraintHasRoleDeletingRule(ElementDeletingEventArgs e)
		{
			ConstraintRoleSequence sequence = (ConstraintRoleSequence)e.ModelElement;
			SetComparisonConstraint curSetComparisonConstraint = sequence.Constraint as SetComparisonConstraint;

			if (curSetComparisonConstraint != null)
			{
				if (curSetComparisonConstraint.IsDeleting)
				{
					HandleConstraintDeleting(null, curSetComparisonConstraint);
				}
			}
		}
		/// <summary>
		/// DeletingRule: typeof(SetConstraint)
		/// </summary>
		private static void SetConstraintDeletingRule(ElementDeletingEventArgs e)
		{
			SetConstraint curSetConstraint = e.ModelElement as SetConstraint;

			if (curSetConstraint != null)
			{
				if (curSetConstraint.IsDeleting)
				{
					HandleConstraintDeleting(curSetConstraint, null);
				}
			}
		}
		/// <summary>
		/// ChangeRule: typeof(SetComparisonConstraint)
		/// Verify pattern modality changes
		/// </summary>
		private static void ModalityChangeRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == SetComparisonConstraint.ModalityDomainPropertyId)
			{
				ProcessSetComparisonConstraintPattern(
					(SetComparisonConstraint)e.ModelElement,
					null,
					null,
					true,
					delegate(IConstraint matchConstraint)
					{
						DelayValidateConstraintPatternError(matchConstraint);
						return true;
					});
			}
		}
		private static void HandleConstraintDeleting(SetConstraint deletedSetConstraint, SetComparisonConstraint deletedSetComparisonConstraint)
		{
			IConstraint deletedConstraint = null;

			if (deletedSetConstraint != null)
			{
				deletedConstraint = deletedSetConstraint;
			}
			else if (deletedSetComparisonConstraint != null)
			{
				deletedConstraint = deletedSetComparisonConstraint;
			}


			if (deletedConstraint != null)
			{
				#region Get constraints in potential conflict
				//Let's assebmle a collection of all constraints in potential conflict
				//for each validationInfo object
				IList<IntersectingConstraintValidation> validations = deletedConstraint.GetIntersectingConstraintValidationInfo();
				int validationCount;
				if (validations == null || 0 == (validationCount = validations.Count))
				{
					return;
				}

				List<ConstraintType> allConstraintTypesInPotentialConflict = new List<ConstraintType>();
				foreach (IntersectingConstraintValidation validationInfo in validations)
				{
					allConstraintTypesInPotentialConflict.AddRange(validationInfo.ConstraintTypesInPotentialConflict);
				}

				if (deletedSetConstraint != null)
				//Get role collection from the set constraint
				{
					CheckIfAnyRolesInCollectionCanConflict(
						deletedSetConstraint.RoleCollection,
						allConstraintTypesInPotentialConflict,
						delegate(IConstraint matchConstraint)
						{
							if (matchConstraint != deletedConstraint)
							{
								DelayValidateConstraintPatternError(matchConstraint);
							}
							return true;
						});
				}
				else
				//This is not a set constraint, but rather setComparison: let's get all roleCollections from all
				//the constraint's roleSequences
				{
					LinkedElementCollection<SetComparisonConstraintRoleSequence> sequenceCollection = deletedSetComparisonConstraint.RoleSequenceCollection;

					foreach (SetComparisonConstraintRoleSequence sequence in sequenceCollection)
					{
						CheckIfAnyRolesInCollectionCanConflict(
							sequence.RoleCollection,
							allConstraintTypesInPotentialConflict,
							delegate(IConstraint matchConstraint)
							{
								if (matchConstraint != deletedConstraint)
								{
									if (matchConstraint != deletedConstraint)
									{
										DelayValidateConstraintPatternError(matchConstraint);
									}
								}
								return true;
							});
					}
				}
				#endregion
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleSequenceHasRole)
		/// </summary>
		private static void ConstraintRoleSequenceHasRoleDeletedRule(ElementDeletedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			ConstraintRoleSequence sequence = link.ConstraintRoleSequence;

			if (!sequence.IsDeleted)
			{
				sequence.DelayValidatePatternError();
				sequence.DelayValidateJoinPath();
			}
		}
		#region Join path validation rules
		/// <summary>
		/// AddRule: typeof(ConstraintRoleProjectedFromCalculatedPathValue)
		/// </summary>
		private static void ConstraintRoleProjectedFromCalculatedValueAddedRule(ElementAddedEventArgs e)
		{
			ConstraintRoleProjection constraintRoleProjection = ((ConstraintRoleProjectedFromCalculatedPathValue)e.ModelElement).ConstraintRoleProjection;
			constraintRoleProjection.ProjectedFromConstant = null;
			constraintRoleProjection.ProjectedFromPathRoot = null;
			constraintRoleProjection.ProjectedFromPathedRole = null;
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleProjectedFromCalculatedPathValue), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void ConstraintRoleProjectedFromCalculatedValueDeletedRule(ElementDeletedEventArgs e)
		{
			DeleteIfEmpty(((ConstraintRoleProjectedFromCalculatedPathValue)e.ModelElement).ConstraintRoleProjection);
		}
		private static void DeleteIfEmpty(ConstraintRoleProjection constraintRoleProjection)
		{
			if (!constraintRoleProjection.IsDeleted &&
				null == constraintRoleProjection.ProjectedFromPathedRole &&
				null == constraintRoleProjection.ProjectedFromPathRoot &&
				null == constraintRoleProjection.ProjectedFromCalculatedValue &&
				null == constraintRoleProjection.ProjectedFromConstant)
			{
				constraintRoleProjection.Delete();
			}
		}
		/// <summary>
		/// AddRule: typeof(ConstraintRoleProjectedFromPathConstant)
		/// </summary>
		private static void ConstraintRoleProjectedFromConstantAddedRule(ElementAddedEventArgs e)
		{
			ConstraintRoleProjection constraintRoleProjection = ((ConstraintRoleProjectedFromPathConstant)e.ModelElement).ConstraintRoleProjection;
			constraintRoleProjection.ProjectedFromPathedRole = null;
			constraintRoleProjection.ProjectedFromPathRoot = null;
			constraintRoleProjection.ProjectedFromCalculatedValue = null;
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleProjectedFromPathConstant), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void ConstraintRoleProjectedFromConstantDeletedRule(ElementDeletedEventArgs e)
		{
			DeleteIfEmpty(((ConstraintRoleProjectedFromPathConstant)e.ModelElement).ConstraintRoleProjection);
		}
		/// <summary>
		/// AddRule: typeof(ConstraintRoleProjectedFromPathedRole)
		/// </summary>
		private static void ConstraintRoleProjectedFromPathedRoleAddedRule(ElementAddedEventArgs e)
		{
			ConstraintRoleProjection constraintRoleProjection = ((ConstraintRoleProjectedFromPathedRole)e.ModelElement).ConstraintRoleProjection;
			constraintRoleProjection.ProjectedFromPathRoot = null;
			constraintRoleProjection.ProjectedFromConstant = null;
			constraintRoleProjection.ProjectedFromCalculatedValue = null;
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleProjectedFromPathedRole), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void ConstraintRoleProjectedFromPathedRoleDeletedRule(ElementDeletedEventArgs e)
		{
			DeleteIfEmpty(((ConstraintRoleProjectedFromPathedRole)e.ModelElement).ConstraintRoleProjection);
		}
		/// <summary>
		/// AddRule: typeof(ConstraintRoleProjectedFromRolePathRoot)
		/// </summary>
		private static void ConstraintRoleProjectedFromPathRootAddedRule(ElementAddedEventArgs e)
		{
			ConstraintRoleProjection constraintRoleProjection = ((ConstraintRoleProjectedFromRolePathRoot)e.ModelElement).ConstraintRoleProjection;
			constraintRoleProjection.ProjectedFromConstant = null;
			constraintRoleProjection.ProjectedFromCalculatedValue = null;
			constraintRoleProjection.ProjectedFromPathedRole = null;
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleProjectedFromRolePathRoot), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// </summary>
		private static void ConstraintRoleProjectedFromPathRootDeletedRule(ElementDeletedEventArgs e)
		{
			DeleteIfEmpty(((ConstraintRoleProjectedFromRolePathRoot)e.ModelElement).ConstraintRoleProjection);
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleProjection), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
		/// Delete a join path projection if there are no more contained role projections.
		/// </summary>
		private static void ConstraintRoleProjectionDeletedRule(ElementDeletedEventArgs e)
		{
			ConstraintRoleSequenceJoinPathProjection projection = ((ConstraintRoleProjection)e.ModelElement).JoinPathProjection;
			if (!projection.IsDeleted &&
				projection.ProjectedRoleCollection.Count == 0)
			{
				projection.Delete();
			}
		}
		/// <summary>
		/// AddRule: typeof(ConstraintRoleSequenceHasJoinPath)
		/// Verify error state when a join path is added
		/// </summary>
		private static void JoinPathAddedRule(ElementAddedEventArgs e)
		{
			FrameworkDomainModel.DelayValidateElement(((ConstraintRoleSequenceHasJoinPath)e.ModelElement).RoleSequence, DelayValidateJoinPath);
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleSequenceHasJoinPath)
		/// Validate join path error
		/// </summary>
		private static void JoinPathDeletedRule(ElementDeletedEventArgs e)
		{
			ConstraintRoleSequence roleSequence = ((ConstraintRoleSequenceHasJoinPath)e.ModelElement).RoleSequence;
			if (!roleSequence.IsDeleted)
			{
				FrameworkDomainModel.DelayValidateElement(roleSequence, DelayValidateJoinPath);
			}
		}
		/// <summary>
		/// AddRule: typeof(ObjectTypePlaysRole)
		/// </summary>
		private static void RolePlayerAddedRule(ElementAddedEventArgs e)
		{
			DelayValidateFactTypeJoinPaths(((ObjectTypePlaysRole)e.ModelElement).PlayedRole.FactType, true);
		}
		/// <summary>
		/// DeleteRule: typeof(ObjectTypePlaysRole)
		/// </summary>
		private static void RolePlayerDeletedRule(ElementDeletedEventArgs e)
		{
			Role role = ((ObjectTypePlaysRole)e.ModelElement).PlayedRole;
			if (!role.IsDeleted)
			{
				DelayValidateFactTypeJoinPaths(role.FactType, true);
			}
		}
		/// <summary>
		/// RolePlayerChangeRule: typeof(ObjectTypePlaysRole)
		/// </summary>
		private static void RolePlayerRolePlayerChangedRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == ObjectTypePlaysRole.RolePlayerDomainRoleId)
			{
				Role role = ((ObjectTypePlaysRole)e.ElementLink).PlayedRole;
				if (!role.IsDeleted)
				{
					DelayValidateFactTypeJoinPaths(role.FactType, true);
				}
			}
			else
			{
				DelayValidateFactTypeJoinPaths(((Role)e.OldRolePlayer).FactType, true);
				DelayValidateFactTypeJoinPaths(((Role)e.NewRolePlayer).FactType, true);
			}
		}
		/// <summary>
		/// AddRule: typeof(FactTypeHasRole)
		/// </summary>
		private static void RoleAddedRule(ElementAddedEventArgs e)
		{
			DelayValidateFactTypeJoinPaths(((FactTypeHasRole)e.ModelElement).FactType, false);
		}
		/// <summary>
		/// DeleteRule: typeof(FactTypeHasRole)
		/// </summary>
		private static void RoleDeletedRule(ElementDeletedEventArgs e)
		{
			DelayValidateFactTypeJoinPaths(((FactTypeHasRole)e.ModelElement).FactType, false);
		}
		/// <summary>
		/// RolePlayerPositionChangeRule: typeof(ConstraintRoleSequenceHasRole)
		/// </summary>
		private static void ConstraintRolePositionChangedRule(RolePlayerOrderChangedEventArgs e)
		{
			if (e.SourceDomainRole.Id == ConstraintRoleSequenceHasRole.ConstraintRoleSequenceDomainRoleId)
			{
				((ConstraintRoleSequence)e.SourceElement).DelayValidateJoinPath();
			}
		}
		/// <summary>
		/// AddRule: typeof(Objectification)
		/// </summary>
		private static void ObjectificationAddedRule(ElementAddedEventArgs e)
		{
			DelayValidateFactTypeJoinPaths(((Objectification)e.ModelElement).NestedFactType, false);
		}
		/// <summary>
		/// DeleteRule: typeof(Objectification)
		/// </summary>
		private static void ObjectificationDeletedRule(ElementDeletedEventArgs e)
		{
			FactType objectifiedFactType = ((Objectification)e.ModelElement).NestedFactType;
			if (!objectifiedFactType.IsDeleted)
			{
				DelayValidateFactTypeJoinPaths(objectifiedFactType, false);
			}
		}
		#endregion // Join path validation rules
		#endregion // Rules
		#region Constraint Overlap Validation
		/// <summary>
		/// Register pattern validation for this sequencye
		/// </summary>
		protected void DelayValidatePatternError()
		{
			ModelElement constraint = Constraint as ModelElement;
			if (constraint != null)
			{
				FrameworkDomainModel.DelayValidateElement(constraint, DelayValidatePatternErrorForConstraint);
			}
			else
			{
				FrameworkDomainModel.DelayValidateElement(this, DelayValidatePatternErrorForSequence);
			}
		}
		/// <summary>
		/// A pass through validator used to map a role sequence that may not
		/// have been added to a constraint when the constraint was added to
		/// validation on the constrant itself. Most pattern validation should
		/// go directly to the constraint, not the sequence, because for set comparison
		/// constraint cases, the sequence can be detached before validation, making it
		/// impossible to determine the constraint to validate.
		/// </summary>
		[DelayValidatePriority(-1)]
		private static void DelayValidatePatternErrorForSequence(ModelElement element)
		{
			ModelElement constraint = ((ConstraintRoleSequence)element).Constraint as ModelElement;
			if (constraint != null)
			{
				FrameworkDomainModel.DelayValidateElement(constraint, DelayValidatePatternErrorForConstraint);
			}
		}
		/// <summary>
		/// Validator callback for MandatoryImpliedByMandatoryError
		/// and UniquenessImpliedByUniquenessError
		/// </summary>
		private static void DelayValidatePatternErrorForConstraint(ModelElement element)
		{
			ValidateConstraintPatternError(element as IConstraint, null);
		}
		/// <summary>
		/// A helper function to delay validate pattern errors on an <see cref="IConstraint"/>
		/// </summary>
		protected static void DelayValidateConstraintPatternError(IConstraint constraint)
		{
			// Delay validate. Note that DelayValidateElement will automatically filter out duplicates
			ModelElement element = (ModelElement)constraint;
			if (!element.IsDeleted && !element.IsDeleting)
			{
				FrameworkDomainModel.DelayValidateElement(element, DelayValidatePatternErrorForConstraint);
			}
		}

		/// <summary>
		/// Validates a subset of predefined constraint patterns (cases where SetConstraints conflict
		/// with SetComparison constraints)
		/// </summary>
		protected static void ValidateConstraintPatternError(IConstraint constraint, INotifyElementAdded notifyAdded)
		{
			if (constraint != null)
			{
				ValidateConstraintPatternErrorWithKnownConstraint(notifyAdded, constraint, IntersectingConstraintPattern.None, null);
			}
		}
		private static void ValidateConstraintPatternErrorWithKnownConstraint(INotifyElementAdded notifyAdded,	IConstraint currentConstraint, IntersectingConstraintPattern pattern, List<IConstraint> constraintsAlreadyValidated)
		{
			#region Declare and Assign necessary variables
			if (constraintsAlreadyValidated != null)
			{
				constraintsAlreadyValidated.Add(currentConstraint);
			}
			IList<IntersectingConstraintValidation> validations = currentConstraint.GetIntersectingConstraintValidationInfo();
			int validationCount;
			if (validations == null || 0 == (validationCount = validations.Count))
			{
				return;
			}

			LinkedElementCollection<SetComparisonConstraintRoleSequence> sequences = null;
			IList<IConstraint> constraintsPossiblyConflicting = null;
			SetComparisonConstraint currentSetComparisonConstraint = null;
			SetConstraint currentSetConstraint = null;
			LinkedElementCollection<Role> setConstraintRoles = null;
			ConstraintModality currentModality;
			Store store;
			if (null != (currentSetConstraint = currentConstraint as SetConstraint))
			{
				currentModality = currentSetConstraint.Modality;
				setConstraintRoles = currentSetConstraint.RoleCollection;
				store = currentSetConstraint.Store;
			}
			else
			{
				currentSetComparisonConstraint = (SetComparisonConstraint)currentConstraint;
				currentModality = currentSetComparisonConstraint.Modality;
				sequences = currentSetComparisonConstraint.RoleSequenceCollection;
				store = currentSetComparisonConstraint.Store;
			}
			#endregion

			for (int iValidation = 0; iValidation < validationCount; ++iValidation)
			{
				#region Declare and Assign necessary variables; decide whether to go on
				IntersectingConstraintValidation validationInfo = validations[iValidation];
				IntersectingConstraintPattern curPattern = validationInfo.IntersectionValidationPattern;

				//If the code needed to be run for a specific validation pettern...
				if (pattern != IntersectingConstraintPattern.None && pattern != curPattern)
				{
					continue;
				}

				IList<ConstraintType> constraintTypesInPotentialConflict = validationInfo.ConstraintTypesInPotentialConflict;

				if (currentSetConstraint != null)
				{
					CheckIfAnyRolesInCollectionCanConflict(
						setConstraintRoles,
						constraintTypesInPotentialConflict,
						delegate(IConstraint matchConstraint)
						{
							if (matchConstraint != currentSetConstraint &&
								validationInfo.TestModality(currentModality, matchConstraint.Modality))
							{
								if (constraintsPossiblyConflicting == null)
								{
									constraintsPossiblyConflicting = new List<IConstraint>();
									constraintsPossiblyConflicting.Add(matchConstraint);
								}
								else if (!constraintsPossiblyConflicting.Contains(matchConstraint))
								{
									constraintsPossiblyConflicting.Add(matchConstraint);
								}
							}
							return true;
						});
				}
				else
				{
					foreach (ConstraintRoleSequence sequence in sequences)
					{
						CheckIfAnyRolesInCollectionCanConflict(
							sequence.RoleCollection,
							constraintTypesInPotentialConflict,
							delegate(IConstraint matchConstraint)
							{
								if (matchConstraint != currentSetComparisonConstraint &&
									validationInfo.TestModality(currentModality, matchConstraint.Modality))
								{
									if (constraintsPossiblyConflicting == null)
									{
										constraintsPossiblyConflicting = new List<IConstraint>();
										constraintsPossiblyConflicting.Add(matchConstraint);
									}
									else if (!constraintsPossiblyConflicting.Contains(matchConstraint))
									{
										constraintsPossiblyConflicting.Add(matchConstraint);
									}
								}
								return true;
							});
					}
				}

				//Get these GUIDs from data
				DomainRoleInfo towardsErrorRoleInfo = null;
				ModelError error = null;
				Guid? domainRoleErrorId = validationInfo.DomainRoleToError;
				if (domainRoleErrorId.HasValue)
				{
					towardsErrorRoleInfo = store.DomainDataDirectory.GetDomainRole(domainRoleErrorId.Value);
					error = (ModelError)towardsErrorRoleInfo.GetLinkedElement((ModelElement)currentConstraint);
				}

				//This variable is used in the code for each validation pattern; but it will be reset
				//for each pattern; this is the default value that can be changed by validation code
				bool hasError = (error != null);

				bool shouldExecuteValidationCode = false;

				if (constraintsPossiblyConflicting == null ||
					constraintsPossiblyConflicting.Count == 0)
				{
					hasError = false;
				}
				else
				{
					if (currentSetConstraint != null)
					{
						// If notifyAdded is not null, then the opposite constraint will be validated via the
						// same call path this one was validated by
						if (notifyAdded == null)
						{
							foreach (IConstraint constraintToValidate in constraintsPossiblyConflicting)
							{
								if (constraintToValidate != currentConstraint)
								{
									bool recurse = true;
									if (constraintsAlreadyValidated == null)
									{
										constraintsAlreadyValidated = new List<IConstraint>();
										constraintsAlreadyValidated.Add(currentConstraint);
									}
									else if (constraintsAlreadyValidated.Contains(constraintToValidate))
									{
										// UNDONE: CONSTRAINTINTERSECTION: make sure we can remove the error
										recurse = false;
										hasError = false;
										error = null;
									}
									if (recurse)
									{
										ValidateConstraintPatternErrorWithKnownConstraint(null, constraintToValidate, pattern, constraintsAlreadyValidated);
									}
								}
							}
						}
					}
					else
					{
						//If it is SetComparisonConstraint
						shouldExecuteValidationCode = true;
					}
				}

				if (!shouldExecuteValidationCode && hasError)
				{
					hasError = (error != null);
				}
				#endregion

				switch (curPattern)
				{
					#region SetComparison-only patterns

					case IntersectingConstraintPattern.SetComparisonConstraintSubset:
						ValidateSetComparisonConstraintSubsetPattern(currentSetComparisonConstraint, notifyAdded, validationInfo, sequences);
						break;
					case IntersectingConstraintPattern.SetComparisonConstraintSuperset:

						List<ConstraintRoleSequence> constraintsToValidateWithSubset = null;
						foreach (ConstraintRoleSequence crs in sequences)
						{
							foreach (Role role in crs.RoleCollection)
							{
								ProcessSetComparisonConstraintPattern(
									currentSetComparisonConstraint,
									role,
									validationInfo,
									false,
									delegate(IConstraint matchConstraint)
									{
										Debug.Assert(matchConstraint.ConstraintStorageStyle == ConstraintStorageStyle.SetComparisonConstraint);
										if (matchConstraint != currentSetComparisonConstraint)
										{
											if (constraintsToValidateWithSubset == null)
											{
												constraintsToValidateWithSubset = new List<ConstraintRoleSequence>();
												constraintsToValidateWithSubset.Add(((SetComparisonConstraint)matchConstraint).RoleSequenceCollection[0]);
											}
											else
											{
												SetComparisonConstraintRoleSequence matchSequence = ((SetComparisonConstraint)matchConstraint).RoleSequenceCollection[0];
												if (!constraintsToValidateWithSubset.Contains(matchSequence))
												{
													constraintsToValidateWithSubset.Add(matchSequence);
												}
											}
										}
										return true;
									});
							}
						}

						if (constraintsToValidateWithSubset == null)
						{
							if (error != null)
							{
								//Validate other constraints on this error
								int validatedCount = 0;

								foreach (IConstraint constr in GetAllConstraintsOnError(error))
								{
									if (constr != currentSetComparisonConstraint)
									{
										bool recurse = true;
										if (constraintsAlreadyValidated == null)
										{
											constraintsAlreadyValidated = new List<IConstraint>();
											constraintsAlreadyValidated.Add(currentConstraint);
										}
										else if (constraintsAlreadyValidated.Contains(constr))
										{
											recurse = false;
										}
										if (recurse)
										{
											validatedCount++;
											ValidateConstraintPatternErrorWithKnownConstraint(
												notifyAdded,
												constr,
												IntersectingConstraintPattern.SetComparisonConstraintSuperset,
												constraintsAlreadyValidated);
										}
									}
								}

								if (validatedCount == 0)
								{
									error.Delete();
								}
							}
						}
						else
						{
							foreach (ConstraintRoleSequence crs in constraintsToValidateWithSubset)
							{
								IConstraint recurseConstraint = crs.Constraint;
								if (recurseConstraint != currentConstraint)
								{
									bool recurse = true;
									if (constraintsAlreadyValidated == null)
									{
										constraintsAlreadyValidated = new List<IConstraint>();
										constraintsAlreadyValidated.Add(currentConstraint);
									}
									else if (constraintsAlreadyValidated.Contains(recurseConstraint))
									{
										recurse = false;
									}
									if (recurse)
									{
										ValidateConstraintPatternErrorWithKnownConstraint(
											notifyAdded,
											recurseConstraint,
											IntersectingConstraintPattern.SetComparisonConstraintSubset,
											constraintsAlreadyValidated);
									}
								}
							}
						}
						break;
					#endregion

					#region Specialized cases
					case IntersectingConstraintPattern.SubsetImpliedByMandatory:
						if (shouldExecuteValidationCode)
						{
							hasError = false;
							//For this pattern: there can be an error only if there are more than one sequences on
							//the constraint and the constraint is only on one column
							if (sequences.Count == 2 && CheckIfHasOneColumn(sequences))
							{
								// The error occurs when simple mandatory is on the top role
								CheckIfAnyRolesInCollectionCanConflict(
									sequences[1].RoleCollection,
									constraintTypesInPotentialConflict,
									delegate(IConstraint matchConstraint)
									{
										hasError = true;
										return false;
									});
								// Note that first (non-arrow) sequence does not matter for simple mandatory implication
								// Both roles mandatory imply an equality, which implies a subset.
								// UNDONE: handle disjunctive mandatory
								// For disjunctive mandatory, if both roles (and no non-intersecting roles) are
								// present in the disjunctive mandatory constraint, then the combination of the
								// mandatory and subset is equivalent to a simple mandatory on the second role.
								// UNDONE: For an implied disjunctive mandatory (and potentially for the two constraints
								// that could reduce to one mandatory), we may need another form of constraint that
								// is marked as a 'reduction'. Any reduction constraint should not be shown. Any
								// constraint that is reduced (aka alternately mapped, consider extending this notion
								// to FactType/ObjectType reductions as well) should not be shown.
							}
						}

						if (hasError)
						{
							HandleError(store, true, ref error, domainRoleErrorId.Value, notifyAdded, currentConstraint);
						}
						else if (error != null)
						{
							error.Delete();
						}
						break;
					case IntersectingConstraintPattern.SubsetContradictsMandatory:
						IList<IConstraint> contradictingMandatory = null;

						if (shouldExecuteValidationCode)
						{
							//For this pattern: there can be an error only if there are more than one sequences on
							//the constraint
							if (sequences.Count == 2)
							{
								//The error occurs when simple mandatory is on the bottom role
								//TODO: handle disjunctive mandatory too
								CheckIfAnyRolesInCollectionCanConflict(
									sequences[0].RoleCollection,
									constraintTypesInPotentialConflict,
									delegate(IConstraint matchConstraint)
									{
										if (contradictingMandatory == null)
										{
											contradictingMandatory = new List<IConstraint>();
										}
										contradictingMandatory.Add(matchConstraint);
										return true;
									});
								hasError = contradictingMandatory != null;

								//The top role of the subset relationship should not be mandatory for the error to occur
								if (hasError)
								{
									CheckIfAnyRolesInCollectionCanConflict(
										sequences[1].RoleCollection,
										constraintTypesInPotentialConflict,
										delegate(IConstraint matchConstraint)
										{
											hasError = false;
											return false;
										});
								}
							}
						}

						if (hasError)
						{
							Guid mandatoryDomainRoleId = MandatoryConstraintHasNotWellModeledSubsetAndMandatoryError.MandatoryConstraintDomainRoleId;

							//Updating error object
							Hashtable constraints = new Hashtable();
							constraints.Add(currentConstraint, domainRoleErrorId);

							foreach (IConstraint c in contradictingMandatory)
							{
								constraints.Add(c, mandatoryDomainRoleId);
							}

							UpdateErrorObject(ref error, constraints);



							HandleError(store, false, ref error, domainRoleErrorId.Value, notifyAdded, currentConstraint);
							HandleError(store, true, ref error, mandatoryDomainRoleId, null, contradictingMandatory);
						}
						else if (error != null)
						{
							error.Delete();
						}
						break;
					//However, if it has a mandatory on one role and the other one is optional - it is
					//a contradiction error
					case IntersectingConstraintPattern.NotWellModeledEqualityAndMandatory:
						HandleExclusionOrEqualityAndMandatory(sequences, shouldExecuteValidationCode,
							false, 1, error, validationInfo,
							notifyAdded, currentConstraint, hasError);
						break;
					//In order for an equality constraint to be implied by mandatories - it must be on roles
					//with at least 2 mandatory constraints
					case IntersectingConstraintPattern.EqualityImpliedByMandatory:
						HandleExclusionOrEqualityAndMandatory(sequences, shouldExecuteValidationCode,
							false, -1, error, validationInfo,
							notifyAdded, currentConstraint, hasError);
						break;
					case IntersectingConstraintPattern.ExclusionContradictsMandatory:
						//In order for an exclusion constraint to constradict with mandatories - there
						//must be at least one mandatory role
						HandleExclusionOrEqualityAndMandatory(sequences, shouldExecuteValidationCode,
							true, 1, error, validationInfo,
							notifyAdded, currentConstraint, hasError);
						break;


					#endregion
					default:
						//This is a pattern that is not handled in this routine
						break;
				}
			}
		}

		private static IEnumerable<IConstraint> GetAllConstraintsOnError(ModelError error)
		{
			foreach (ElementLink link in DomainRoleInfo.GetAllElementLinks(error))
			{
				IConstraint cur = DomainRoleInfo.GetSourceRolePlayer(link) as IConstraint;

				if (cur != null)
				{
					yield return cur;
				}
			}
		}

		/// <summary>
		/// Callback function for <see cref="CheckIfAnyRolesInCollectionCanConflict"/> and <see cref="ProcessSetComparisonConstraintPattern"/>
		/// </summary>
		/// <param name="constraint">Matching <see cref="IConstraint"/> to add</param>
		/// <returns><see langword="true"/> to continue iteration</returns>
		private delegate bool ConstraintMatch(IConstraint constraint);

		/// <summary>
		/// Checks if any role is attached to a potentially conflicting constraint
		/// </summary>
		/// <param name="relatedRoles">RoleCollection for roles that can be attached to conflicting constraints</param>
		/// <param name="constraintTypesInPotentialConflict">Constraint types that can be conflicting</param>
		/// <param name="matchCallback">Callback delegate of type <see cref="ConstraintMatch"/></param>
		private static void CheckIfAnyRolesInCollectionCanConflict(
			LinkedElementCollection<Role> relatedRoles,
			IList<ConstraintType> constraintTypesInPotentialConflict,
			ConstraintMatch matchCallback)
		{
			int roleCount = relatedRoles.Count;
			for (int i = 0; i < roleCount; ++i)
			{
				Role role = relatedRoles[i];
				LinkedElementCollection<ConstraintRoleSequence> sequences = role.ConstraintRoleSequenceCollection;
				int sequenceCount = sequences.Count;
				for (int j = 0; j < sequenceCount; ++j)
				{
					IConstraint currentConstraint = sequences[j].Constraint;

					if (currentConstraint != null &&
						constraintTypesInPotentialConflict.Contains(currentConstraint.ConstraintType) &&
						!matchCallback(currentConstraint))
					{
						return;
					}
				}
			}
		}

		private static bool CheckIfHasOneColumn(LinkedElementCollection<SetComparisonConstraintRoleSequence> sequences)
		{
			bool hasOneColumn = true;

			foreach (SetComparisonConstraintRoleSequence sequence in sequences)
			{
				if (sequence.RoleCollection.Count > 1)
				{
					hasOneColumn = false;
					break;
				}
			}

			return hasOneColumn;
		}

		/// <summary>
		/// Once you have a list of every constraint that needs to be attached to the error, this method will go through this
		/// list and make sure you are not attaching a new instance of the same error to these constraints (some of them may
		/// already be linked to the ones you need)
		/// </summary>
		/// <param name="error"></param>
		/// <param name="allConstraintsThatNeedToBeAttached_And_TheirRoleIds">
		/// The key is Iconstraint and the value is a Guis of the role this constraint plays in the relationship with this error
		/// </param>
		private static void UpdateErrorObject(ref ModelError error, Hashtable allConstraintsThatNeedToBeAttached_And_TheirRoleIds)
		{
			if (error == null)
			{
				ModelError temp = null;

				ICollection constraints = allConstraintsThatNeedToBeAttached_And_TheirRoleIds.Keys;

				//Find if the error we need is already attached to some of conflicting constraints
				foreach (IConstraint badConstraint in constraints)
				{
					temp = (ModelError)DomainRoleInfo.GetLinkedElement((ModelElement)badConstraint,
						(Guid)allConstraintsThatNeedToBeAttached_And_TheirRoleIds[badConstraint]);

					if (temp != null)
					{
						if (error != null)
						{
							Debug.Assert(temp.Equals(error));
						}

						error = temp;
					}
				}
			}
		}
		/// <summary>
		/// Takes care of attaching the constraints to the error and updating error text if requested
		/// </summary>
		/// <param name="store">Context <see cref="Store"/></param>
		/// <param name="generateText">True to generated text for the error</param>
		/// <param name="error">The error being created or modified</param>
		/// <param name="domainRoleErrorId">The role on the error object that is opposite the error to create</param>
		/// <param name="notifyAdded"><see cref="INotifyElementAdded"/> callback, set during deserialization</param>
		/// <param name="conflictingConstraints">All constraints that need to be added to the error</param>
		private static void HandleError(
			Store store,
			bool generateText,
			ref ModelError error,
			Guid domainRoleErrorId,
			INotifyElementAdded notifyAdded,
			params IConstraint[] conflictingConstraints)
		{
			HandleError(store, generateText, ref error, domainRoleErrorId, notifyAdded, (IList<IConstraint>)conflictingConstraints);
		}
		/// <summary>
		/// Takes care of attaching the constraints to the error and updating error text if requested
		/// </summary>
		/// <param name="store">Context <see cref="Store"/></param>
		/// <param name="generateText">True to generated text for the error</param>
		/// <param name="error">The error being created or modified</param>
		/// <param name="domainRoleErrorId">The role on the error object that is opposite the error to create</param>
		/// <param name="notifyAdded"><see cref="INotifyElementAdded"/> callback, set during deserialization</param>
		/// <param name="conflictingConstraints">All constraints that need to be added to the error</param>
		private static void HandleError(
			Store store,
			bool generateText,
			ref ModelError error,
			Guid domainRoleErrorId,
			INotifyElementAdded notifyAdded,
			IList<IConstraint> conflictingConstraints)
		{
			int constraintCount;
			if (conflictingConstraints == null ||
				0 == (constraintCount = conflictingConstraints.Count))
			{
				return;
			}
			if (error == null)
			{
				//Create it
				error = (ModelError)store.ElementFactory.CreateElement(store.DomainDataDirectory.FindDomainRole(domainRoleErrorId).OppositeDomainRole.RolePlayer);
			}

			ModelElement errorLinked;

			for (int i = 0; i < constraintCount; ++i)
			{
				IConstraint curConstraint = conflictingConstraints[i];
				errorLinked = DomainRoleInfo.GetLinkedElement((ModelElement)curConstraint, domainRoleErrorId);

				if (errorLinked != error || errorLinked == null)
				{
					//Note: If the constraint can play with at most one instance of this type of error, 
					//when you create another error of the same type that involves the same constraint:
					//this line would replace the error end of the link to point to the newly created
					//error instead of the previously created error

					//However: right now the validation code relies on each constraint playing with at
					//most one type of error

					DomainRoleInfo.SetLinkedElement((ModelElement)curConstraint, domainRoleErrorId, error);
				}
			}


			if (generateText)
			{
				error.Model = conflictingConstraints[0].Model;
				error.GenerateErrorText();
			}

			if (notifyAdded != null)
			{
				notifyAdded.ElementAdded(error, true);
			}
		}

		/// <summary>
		/// Checks if there is a model error, handles attaching it to appropriate objects
		/// and returns true is the error was found
		/// </summary>
		/// <param name="sequences"></param>
		/// <param name="shouldExecuteValidationCode"></param>
		/// <param name="isExclusion"></param>
		/// <param name="minNumViolatingConstraints"></param>
		/// <param name="error"></param>
		/// <param name="validationInfo"></param>
		/// <param name="notifyAdded"></param>
		/// <param name="curConstraint"></param>
		/// <param name="hasErrorDefault"></param>
		/// <returns></returns>
		private static bool HandleExclusionOrEqualityAndMandatory(
			LinkedElementCollection<SetComparisonConstraintRoleSequence> sequences,
			bool shouldExecuteValidationCode,
			bool isExclusion, int minNumViolatingConstraints,
			ModelError error,
			IntersectingConstraintValidation validationInfo,
			INotifyElementAdded notifyAdded,
			IConstraint curConstraint,
			bool hasErrorDefault)
		{
			Guid? optionalDomainRoleErrorId = validationInfo.DomainRoleToError;
			if (!optionalDomainRoleErrorId.HasValue)
			{
				return false;
			}
			Guid domainRoleErrorId = optionalDomainRoleErrorId.Value;
			IList<IConstraint> constrFound = null;
			bool hasError = hasErrorDefault;
			Store store = ((ModelElement)curConstraint).Store;

			if (shouldExecuteValidationCode &&
				(0 < minNumViolatingConstraints ||
				0 < (minNumViolatingConstraints = (sequences != null) ? sequences.Count : 0)))
			{
				int numOfViolatingConstraints = 0;

				//For these patterns: there can be an error only if there are more than one sequences on
				//the constraint and the constraint is only on one column
				if (sequences.Count > 1 && CheckIfHasOneColumn(sequences))
				{
					//The error occurs when simple mandatory is on several roles
					//TODO: handle disjunctive mandatory too

					foreach (SetComparisonConstraintRoleSequence sequence in sequences)
					{
						CheckIfAnyRolesInCollectionCanConflict(
							sequence.RoleCollection,
							validationInfo.ConstraintTypesInPotentialConflict,
							delegate(IConstraint matchConstraint)
							{
								if (constrFound == null)
								{
									constrFound = new List<IConstraint>();
								}
								constrFound.Add(matchConstraint);
								return true;
							});
					}
					if (constrFound != null)
					{
						numOfViolatingConstraints = constrFound.Count;
					}
				}


				if (numOfViolatingConstraints >= minNumViolatingConstraints)
				{
					hasError = true;
				}
				else
				{
					hasError = false;
				}
			}

			if (hasError)
			{
				if (isExclusion)
				{
					Guid mandatoriesDomainRoleId = MandatoryConstraintHasExclusionContradictsMandatoryError.MandatoryConstraintDomainRoleId;

					//Updating error object
					Hashtable constraints = new Hashtable();
					constraints.Add(curConstraint, domainRoleErrorId);

					if (constrFound != null)
					{
						foreach (IConstraint c in constrFound)
						{
							constraints.Add(c, mandatoriesDomainRoleId);
						}
					}

					UpdateErrorObject(ref error, constraints);

					//!isExclusion means that if it is not exclusion constraint - no more error handling will
					//occur, so error text needs to be generated now
					HandleError(store, !isExclusion, ref error, domainRoleErrorId, notifyAdded, curConstraint);
					HandleError(store, true, ref error, mandatoriesDomainRoleId, null, constrFound);
				}
				else
				{
					//!isExclusion means that if it is not exclusion constraint - no more error handling will
					//occur, so error text needs to be generated now
					HandleError(store, !isExclusion, ref error, domainRoleErrorId, notifyAdded, curConstraint);
				}
			}
			else if (error != null)
			{
				error.Delete();
			}

			return hasError;
		}	

		/// <summary>
		/// Validates the SetComparisonConstraint Subset Pattern
		/// </summary>
		/// <param name="setComparsionConstraint">The constraint that might be a subset of another constraint, which
		/// would result in an error</param>
		/// <param name="notifyAdded"></param>
		/// <param name="validationInfo">Validation information of the current constraint</param>
		/// <param name="constraintSequences">Sequences linked to the constraint</param>
		private static void ValidateSetComparisonConstraintSubsetPattern(
			SetComparisonConstraint setComparsionConstraint,
			INotifyElementAdded notifyAdded,
			IntersectingConstraintValidation validationInfo,
			LinkedElementCollection<SetComparisonConstraintRoleSequence> constraintSequences)
		{
			int constraintSequenceCount = constraintSequences.Count;
			if (validationInfo.IntersectionValidationPattern != IntersectingConstraintPattern.SetComparisonConstraintSubset)
			{
				return;
			}

			#region Validation Code
			ConstraintModality currentModality = setComparsionConstraint.Modality;
			List<IConstraint> constraintsToCheck = null;
			for (int iConstraintSequence = 0; iConstraintSequence < constraintSequenceCount; ++iConstraintSequence)
			{
				ConstraintRoleSequence sequence = constraintSequences[iConstraintSequence];
				LinkedElementCollection<Role> roles = sequence.RoleCollection;
				int roleCount = roles.Count;
				for (int iRole = 0; iRole < roleCount; ++iRole)
				{
					Role selectedRole = roles[iRole];
					LinkedElementCollection<ConstraintRoleSequence> eligibleSequences = selectedRole.ConstraintRoleSequenceCollection;
					int eligibleSequenceCount = eligibleSequences.Count;
					for (int k = 0; k < eligibleSequenceCount; ++k)
					{
						ConstraintRoleSequence eligibleSequence = eligibleSequences[k];
						IConstraint intersectingConstraint = eligibleSequence.Constraint;
						if (intersectingConstraint != setComparsionConstraint &&
							validationInfo.TestModality(currentModality, intersectingConstraint.Modality) &&
							(validationInfo.ConstraintTypesInPotentialConflict as IList<ConstraintType>).Contains(intersectingConstraint.ConstraintType))
						{
							if (constraintsToCheck == null)
							{
								constraintsToCheck = new List<IConstraint>();
								constraintsToCheck.Add(intersectingConstraint);
							}
							else if (!constraintsToCheck.Contains(intersectingConstraint))
							{
								constraintsToCheck.Add(intersectingConstraint);
							}
						}
					}
				}
			}
			int constraintsToCheckCount = (constraintsToCheck == null) ? 0 : constraintsToCheck.Count;
			List<IConstraint> constraintsInError = null;
			// Loop through constraints to check for overlap
			for (int iConstraintsToCheck = 0; iConstraintsToCheck < constraintsToCheckCount; ++iConstraintsToCheck)
			{
				SetComparisonConstraint constraint = constraintsToCheck[iConstraintsToCheck] as SetComparisonConstraint;
				Debug.Assert(constraint != null);
				LinkedElementCollection<SetComparisonConstraintRoleSequence> intersectingSequences = constraint.RoleSequenceCollection;
				int intersectingSequenceCount = intersectingSequences.Count;
				// Loop through the ConstraintRoleSequences of the validating constraint
				bool conflicting = true;
				for (int iConstraintSequence = 0; conflicting && iConstraintSequence < constraintSequenceCount; ++iConstraintSequence)
				{
					ConstraintRoleSequence constraintSequence = constraintSequences[iConstraintSequence];
					LinkedElementCollection<Role> constraintRoles = constraintSequence.RoleCollection;
					int constraintRoleCount = constraintRoles.Count;
					// If no roles, bail out
					if (constraintRoleCount > 0)
					{
						// Pull out the first role for checking
						Role firstConstraintRole = constraintRoles[0];
						bool sequenceConflict = false;
						// Loop through the ConstraintRoleSequences of the currently selected constraint
						for (int iIntersectingSequence = 0; !sequenceConflict && iIntersectingSequence < intersectingSequenceCount; ++iIntersectingSequence)
						{
							ConstraintRoleSequence intersectingSequence = intersectingSequences[iIntersectingSequence];
							LinkedElementCollection<Role> intersectingRoles = intersectingSequence.RoleCollection;
							int intersectingRoleCount = intersectingRoles.Count;
							// Loop through the Roles of the currently selected ConstraintRoleSequence
							for (int iIntersectingRole = 0; !sequenceConflict && iIntersectingRole < intersectingRoleCount && iIntersectingRole <= intersectingRoleCount - constraintRoleCount; ++iIntersectingRole)
							{
								Role intersectingRole = intersectingRoles[iIntersectingRole];
								// If the currently selected role matches the first constraint role, check the rest
								if (intersectingRole == firstConstraintRole)
								{
									bool roleOrderConflict = true;
									// Loops through the remaining roles of the current constraint role sequence of the validating constraint
									for (int iConstraintRole = 1; roleOrderConflict && iConstraintRole < constraintRoleCount; ++iConstraintRole)
									{
										if (constraintRoles[iConstraintRole] != intersectingRoles[iIntersectingRole + iConstraintRole])
										{
											roleOrderConflict = false;
										}
									}
									// Found a matching role order, pass it up
									if (roleOrderConflict)
									{
										sequenceConflict = true;
									}
								}
							}
						}
						// Didn't find a sequence that conflicts with the current one being checked, so no conflict
						if (!sequenceConflict)
						{
							conflicting = false;
						}
					}
				}
				if (conflicting)
				{
					if (constraintsInError == null)
					{
						constraintsInError = new List<IConstraint>();
					}
					constraintsInError.Add(constraint);
				}
			}

			#endregion

			#region Handling the error
			int constraintsInErrorCount = (constraintsInError == null) ? 0 : constraintsInError.Count;
			Guid domainRoleErrorId = validationInfo.DomainRoleToError.Value;
			Store store = setComparsionConstraint.Store;
			DomainRoleInfo constraintRoleInfo = store.DomainDataDirectory.FindDomainRole(domainRoleErrorId);
			DomainRoleInfo errorRoleInfo = constraintRoleInfo.OppositeDomainRole;

			Multiplicity errorToItsConstraintsMultiplicity = errorRoleInfo.Multiplicity;
			DomainClassInfo errorType = errorRoleInfo.RolePlayer;

			if (errorToItsConstraintsMultiplicity == Multiplicity.OneMany ||
				errorToItsConstraintsMultiplicity == Multiplicity.ZeroMany)
			{
				//If the multiplicity is one-to-many ot zero-to-many: all constraintsInError found need to be attached
				//to the error

				ModelError error = (ModelError)DomainRoleInfo.GetLinkedElement(setComparsionConstraint, domainRoleErrorId);
				if (constraintsInErrorCount != 0)
				{
					//For this pattern: there can be an error only if there are more than one sequences on
					//the constraint 
					if (constraintSequenceCount > 1)
					{
						//Updating error object
						Hashtable constraints = new Hashtable();
						constraints.Add(setComparsionConstraint, domainRoleErrorId);

						foreach (IConstraint c in constraintsInError)
						{
							constraints.Add(c, domainRoleErrorId);
						}

						UpdateErrorObject(ref error, constraints);


						//Need to attach the error to all: the current constraint and all constraints, which were found to conflict with it
						HandleError(store, false, ref error, domainRoleErrorId, notifyAdded, setComparsionConstraint);
						HandleError(store, true, ref error, domainRoleErrorId, notifyAdded, constraintsInError);
					}
				}
				else if (error != null)
				{
					// UNDONE: This is too strong. Currently, if multiple constraints are in contradiction, then they
					// are all put together in one error. This is not quite right (if B contradicts A and C contradicts A,
					// then we say that {A,B,C} are in contradiction, but B and C do not necessarily contradict). In this
					// case, deleting the error here will actually removing a remaining contradiction. Given that this is an
					// unusual case (generally, two constraints contradict each other), we should instead create two contradiction
					// errors in this case (A contradicts B, A contradicts C). Without this change, we would have to reprocess
					// any remaining constraints at this point to see if they are still in contradiction.
					error.Delete();
				}
			}
			else
			{
				//If the multiplicity is one-to-one ot zero-to-one: each constraint found to conflict needs to be
				//attached to the appropriate error

				for (int iConstraintInPotentialConflict = 0; iConstraintInPotentialConflict < constraintsToCheckCount; ++iConstraintInPotentialConflict)
				{
					IConstraint curConstraint = constraintsToCheck[iConstraintInPotentialConflict];
					ModelError error = (ModelError)DomainRoleInfo.GetLinkedElement((ModelElement)curConstraint, domainRoleErrorId);

					if (constraintsInError != null && constraintsInError.Contains(curConstraint))
					{
						//For this pattern: there can be an error only if there are more than one sequences on
						//the constraint 
						if (constraintSequenceCount > 1)
						{
							//Attach the error only to the current constraint
							HandleError(store, true, ref error, domainRoleErrorId, notifyAdded, curConstraint);
						}
					}
					else if (error != null) //there was an error but not anymore
					{
						error.Delete();
					}
				}
				if (constraintsInErrorCount == 0)
				{
					ModelError error = (ModelError)DomainRoleInfo.GetLinkedElement(setComparsionConstraint, domainRoleErrorId);
					if (error != null)
					{
						error.Delete();
					}
				}
			}
			#endregion
		}

		/// <summary>
		/// Returns the List of RoleSequenceCollections that represent the setComparisonConstraints that need to be validated for 
		/// SetComparisonConstraintSubset validation pattern
		/// </summary>
		/// <param name="setComparisonConstraint"></param>
		/// <param name="role"></param>
		/// <param name="validationOfInterest">if specified - only constraints relevant to conflicting with thise validation
		/// will be returned</param>
		/// <param name="modalityChange">true if only modality changes should be considered</param>
		/// <param name="matchCallback">Callback delegate of type <see cref="ConstraintMatch"/></param>
		private static void ProcessSetComparisonConstraintPattern(SetComparisonConstraint setComparisonConstraint, Role role, IntersectingConstraintValidation? validationOfInterest, bool modalityChange, ConstraintMatch matchCallback)
		{
			if (setComparisonConstraint != null)
			{
				IList<IntersectingConstraintValidation> validations = (setComparisonConstraint as IConstraint).GetIntersectingConstraintValidationInfo();
				if (validations != null)
				{
					int validationCount = validations.Count;
					ConstraintModality constraintModality = setComparisonConstraint.Modality;
					for (int iValidation = 0; iValidation < validationCount; ++iValidation)
					{
						IntersectingConstraintValidation validationInfo = validations[iValidation];
						if (validationOfInterest != null)
						{
							if (!validationOfInterest.Equals(validationInfo))
							{
								continue;
							}
						}
						if (modalityChange && IntersectingConstraintPatternOptions.IntersectingConstraintModalityIgnored == (validationInfo.IntersectionValidationOptions & IntersectingConstraintPatternOptions.IntersectingConstraintModalityMask))
						{
							continue;
						}
						switch (validationInfo.IntersectionValidationPattern)
						{
							case IntersectingConstraintPattern.SetComparisonConstraintSubset:
								if (!modalityChange || (IntersectingConstraintPatternOptions.IntersectingConstraintModalityIgnored != (validationInfo.IntersectionValidationOptions & IntersectingConstraintPatternOptions.IntersectingConstraintModalityMask)))
								{
									if (!matchCallback(setComparisonConstraint))
									{
										return;
									}
								}
								break;
							case IntersectingConstraintPattern.SetComparisonConstraintSuperset:
								LinkedElementCollection<SetComparisonConstraintRoleSequence> comparisonConstraintSequences = setComparisonConstraint.RoleSequenceCollection;
								int comparisonConstraintSequencesCount = comparisonConstraintSequences.Count;
								for (int i = 0; i < comparisonConstraintSequencesCount; ++i)
								{
									SetComparisonConstraintRoleSequence comparisonConstraintSequence = comparisonConstraintSequences[i];
									List<Role> comparisonConstraintRoles = new List<Role>(comparisonConstraintSequence.RoleCollection);
									if (role != null && !comparisonConstraintRoles.Contains(role))
									{
										comparisonConstraintRoles.Add(role);
									}
									int comparisonConstraintRoleCount = comparisonConstraintRoles.Count;
									for (int j = 0; j < comparisonConstraintRoleCount; ++j)
									{
										Role selectedRole = comparisonConstraintRoles[j];
										LinkedElementCollection<ConstraintRoleSequence> sequences = selectedRole.ConstraintRoleSequenceCollection;
										int sequenceCount = sequences.Count;
										for (int k = 0; k < sequenceCount; ++k)
										{
											ConstraintRoleSequence eligibleSequence = sequences[k];
											IConstraint eligibleConstraint = eligibleSequence.Constraint;
											if (eligibleConstraint != setComparisonConstraint &&
												(modalityChange || validationInfo.TestModality(constraintModality, eligibleConstraint.Modality)) &&
												(validationInfo.ConstraintTypesInPotentialConflict as IList<ConstraintType>).Contains(eligibleConstraint.ConstraintType) &&
												!matchCallback(eligibleConstraint))
											{
												return;
											}
										}
									}
								}
								break;
						}
					}
				}
			}
		}
		#endregion // Constraint Overlap Validation
		#region JoinPath Validation
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// adds the implicit FactConstraint elements.
		/// </summary>
		public static IDeserializationFixupListener JoinPathFixupListener
		{
			get
			{
				return new AutomaticJoinPathFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation. Adds implicit FactConstraint relationships
		/// </summary>
		private sealed class AutomaticJoinPathFixupListener : DeserializationFixupListener<ConstraintRoleSequence>
		{
			/// <summary>
			/// ExternalConstraintFixupListener constructor
			/// </summary>
			public AutomaticJoinPathFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateImplicitStoredElements)
			{
			}
			/// <summary>
			/// Ignore constraint types that never have join
			/// </summary>
			protected override bool VerifyElementType(ModelElement element)
			{
				SetConstraint constraint = element as SetConstraint;
				if (constraint != null)
				{
					switch (((IConstraint)constraint).ConstraintType)
					{
						// These are based on properties, can't be checked in VerifyElementType
						// case ConstraintType.InternalUniqueness:
						// case ConstraintType.SimpleMandatory:
						// case ConstraintType.ImpliedMandatory:
						case ConstraintType.DisjunctiveMandatory:
							return false;
					}
				}
				return true;
			}
			/// <summary>
			/// Generate and verify automatic join paths
			/// </summary>
			protected sealed override void ProcessElement(ConstraintRoleSequence element, Store store, INotifyElementAdded notifyAdded)
			{
				if (!element.IsDeleted)
				{
					element.ValidateJoinPath(notifyAdded, false, true);
				}
			}
		}
		#endregion // Deserialization Fixup
		/// <summary>
		/// ChangeRule: typeof(ConstraintRoleSequenceJoinPath)
		/// </summary>
		private static void AutomaticJoinPathChangeRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == ConstraintRoleSequenceJoinPath.IsAutomaticDomainPropertyId)
			{
				if ((bool)e.NewValue)
				{
					ConstraintRoleSequenceJoinPath joinPath = (ConstraintRoleSequenceJoinPath)e.ModelElement;
					ConstraintRoleSequence roleSequence;
					if (null != (roleSequence = joinPath.RoleSequence))
					{
						// Force a nested transaction to run the join path validation
						// in an isolated block so we can roll back.
						using (Transaction t = roleSequence.Store.TransactionManager.BeginTransaction("Force automatic validation"))
						{
							FrameworkDomainModel.DelayValidateElement(roleSequence, DelayValidateJoinPath);
							t.Commit();
						}
						if (joinPath.IsDeleted)
						{
							throw new InvalidOperationException(ResourceStrings.ModelExceptionConstraintRoleSequenceAutomaticJoinPathNotAvailable);
						}
					}
				}
			}
		}
		/// <summary>
		/// Register a fact type for automatic join path analysis
		/// </summary>
		/// <param name="factType">The <see cref="FactType"/> to process.</param>
		/// <param name="processLinkedFactType"><see langword="true"/> if the objectified fact type for a link fact type should
		/// be processed instead.</param>
		private static void DelayValidateFactTypeJoinPaths(FactType factType, bool processLinkedFactType)
		{
			if (factType != null && !factType.IsDeleted && !(factType is SubtypeFact))
			{
				Objectification objectification = factType.ImpliedByObjectification;
				if (objectification != null)
				{
					if (!processLinkedFactType)
					{
						return;
					}
					factType = objectification.NestedFactType;
				}
				FrameworkDomainModel.DelayValidateElement(factType, DelayValidateAutomaticJoinPaths);
			}
		}
		/// <summary>
		/// Determine any intersection constraint that have possible join paths
		/// and register them for validation.
		/// </summary>
		/// <param name="element"><see cref="FactType"/></param>
		[DelayValidatePriority(-1)] // Do this early
		private static void DelayValidateAutomaticJoinPaths(ModelElement element)
		{
			if (element.IsDeleted)
			{
				return;
			}
			foreach (RoleBase roleBase in ((FactType)element).RoleCollection)
			{
				Role role = roleBase as Role;
				if (role != null)
				{
					foreach (ConstraintRoleSequence roleSequence in role.ConstraintRoleSequenceCollection)
					{
						roleSequence.DelayValidateJoinPath();
					}
				}
			}
		}
		private void DelayValidateJoinPath()
		{
			SetConstraint setConstraint = this as SetConstraint;
			if (setConstraint != null)
			{
				switch (((IConstraint)setConstraint).ConstraintType)
				{
					case ConstraintType.InternalUniqueness:
					case ConstraintType.ImpliedMandatory:
					case ConstraintType.SimpleMandatory:
					case ConstraintType.DisjunctiveMandatory:
						// These never have join paths, do not bother tracking them
						return;
				}
			}
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateJoinPath);
		}
		private static void DelayValidateJoinPath(ModelElement element)
		{
			if (element.IsDeleted)
			{
				return;
			}
			((ConstraintRoleSequence)element).ValidateJoinPath(null, true, true);
		}
		/// <summary>
		/// Verify if a join path is required or can be implicitly created.
		/// </summary>
		private void ValidateJoinPath(INotifyElementAdded notifyAdded, bool verifyError, bool verifyAutomaticPath)
		{
			bool checkBinaryOppositeRolePlayerPattern = false;
			bool joinNotNeeded = false;
			IConstraint constraint = Constraint;
			// Check the constraint patterns to shortcircuit requirements
			if (constraint != null)
			{
				switch (constraint.ConstraintType)
				{
					case ConstraintType.ImpliedMandatory:
					case ConstraintType.SimpleMandatory:
					case ConstraintType.DisjunctiveMandatory:
						// These types never have a join path, and this is enforced
						// by the XML schema so they cannot be present on load.
						return;
					case ConstraintType.InternalUniqueness:
						// This should not have a join path, but is not blocked by the schema.
						if (notifyAdded != null)
						{
							joinNotNeeded = true;
							break;
						}
						return;
					case ConstraintType.Frequency:
					case ConstraintType.ExternalUniqueness:
						checkBinaryOppositeRolePlayerPattern = true;
						break;
				}
			}
			ConstraintRoleSequenceJoinPath joinPath = JoinPath;
			JoinPathRequiredError pathRequiredError = verifyError ? JoinPathRequiredError : null;
			if (!joinNotNeeded)
			{
				ReadOnlyCollection<ConstraintRoleSequenceHasRole> constraintRoles = ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(this);
				int constraintRoleCount = constraintRoles.Count;
				joinNotNeeded = true;
				if (constraintRoleCount > 1)
				{
					Role firstRole = constraintRoles[0].Role;
					FactType firstFactType = firstRole.FactType;
					ObjectType resolvedOppositeRolePlayer = null;
					ObjectType resolvedOppositeRolePlayerAlternate = null; // Note that the alternate will not be set if the first choice is not
					bool checkedFirstOppositeRolePlayer = false;
#if BLOCK_TRIVIAL_JOINPATHS
					bool multipleCompatibleOppositeRolePlayers = false;
					bool usesOppositeObjectification = false;
#endif // BLOCK_TRIVIAL_JOINPATHS
					BitTracker usedObjectifyingRole = new BitTracker(constraintRoleCount);
					for (int i = 1; i < constraintRoleCount; ++i)
					{
						Role currentRole = constraintRoles[i].Role;
						FactType currentFactType = currentRole.FactType;
						if (currentFactType != firstFactType)
						{
							if (checkBinaryOppositeRolePlayerPattern)
							{
								Role oppositeRole;
								RoleProxy proxy;
								ObjectType oppositeRolePlayer;
								ObjectType objectifyingRolePlayer;
								ObjectType compatibleSupertype;
								if (!checkedFirstOppositeRolePlayer)
								{
									checkedFirstOppositeRolePlayer = true;
									oppositeRole = firstRole.OppositeRole as Role;
									if (oppositeRole != null)
									{
										resolvedOppositeRolePlayer = oppositeRole.RolePlayer;
									}
									if (null != (proxy = firstRole.Proxy) &&
										null != (oppositeRole = proxy.OppositeRole as Role))
									{
										if (resolvedOppositeRolePlayer == null)
										{
											resolvedOppositeRolePlayer = oppositeRole.RolePlayer;
#if BLOCK_TRIVIAL_JOINPATHS
											usesOppositeObjectification = true;
#endif // BLOCK_TRIVIAL_JOINPATHS
											// All roles from the same objectified fact type will
											// have the same opposite role player. They all need to
											// be moved to the opposite role.
											usedObjectifyingRole[0] = true;
											for (int j = 1; j < constraintRoleCount; ++j)
											{
												if (j != i && constraintRoles[j].Role.FactType == firstFactType)
												{
													usedObjectifyingRole[j] = true;
												}
											}
										}
										else
										{
											resolvedOppositeRolePlayerAlternate = oppositeRole.RolePlayer;
										}
									}
								}
								if (null != resolvedOppositeRolePlayer)
								{
									oppositeRolePlayer = null != (oppositeRole = currentRole.OppositeRole as Role) ? oppositeRole.RolePlayer : null;
									// Check for an exact match
									if (oppositeRolePlayer != null)
									{
										if (oppositeRolePlayer == resolvedOppositeRolePlayer)
										{
											resolvedOppositeRolePlayerAlternate = null;
											continue;
										}
										else if (oppositeRolePlayer == resolvedOppositeRolePlayerAlternate)
										{
											resolvedOppositeRolePlayer = resolvedOppositeRolePlayerAlternate;
											resolvedOppositeRolePlayerAlternate = null;
#if BLOCK_TRIVIAL_JOINPATHS
											usesOppositeObjectification = true;
#endif // BLOCK_TRIVIAL_JOINPATHS
											// Use objectification for all roles on the original fact type
											usedObjectifyingRole[0] = true;
											for (int j = 1; j < constraintRoleCount; ++j)
											{
												if (j != i && constraintRoles[j].Role.FactType == firstFactType)
												{
													usedObjectifyingRole[j] = true;
												}
											}
											continue;
										}
									}
									objectifyingRolePlayer = (null != (proxy = currentRole.Proxy) && null != (oppositeRole = proxy.OppositeRole as Role)) ? oppositeRole.RolePlayer : null;
									if (objectifyingRolePlayer != null)
									{
										if (objectifyingRolePlayer == resolvedOppositeRolePlayer)
										{
											resolvedOppositeRolePlayerAlternate = null;
#if BLOCK_TRIVIAL_JOINPATHS
											usesOppositeObjectification = true;
#endif // BLOCK_TRIVIAL_JOINPATHS
											usedObjectifyingRole[i] = true;
											continue;
										}
										else if (objectifyingRolePlayer == resolvedOppositeRolePlayerAlternate)
										{
											resolvedOppositeRolePlayer = resolvedOppositeRolePlayerAlternate;
											resolvedOppositeRolePlayerAlternate = null;
#if BLOCK_TRIVIAL_JOINPATHS
											usesOppositeObjectification = true;
#endif // BLOCK_TRIVIAL_JOINPATHS
											usedObjectifyingRole[i] = true;
											continue;
										}
									}

									// We didn't get an exact type match. Repeat the exact match logic with
									// the more expensive tests for compatibility.
									if (oppositeRolePlayer != null)
									{
										if (null != (compatibleSupertype = ResolveCompatibleSupertype(resolvedOppositeRolePlayer, oppositeRolePlayer)))
										{
											resolvedOppositeRolePlayer = compatibleSupertype;
											resolvedOppositeRolePlayerAlternate = null;
#if BLOCK_TRIVIAL_JOINPATHS
											multipleCompatibleOppositeRolePlayers = true;
#endif // BLOCK_TRIVIAL_JOINPATHS
											// Note that we'll create an automatic join path for this case,
											// but we leave joinNotNeeded as true to distinguish a subsequent
											// break from the pattern. (Reapply comment applies to remaining three cases).
											continue;
										}
										else if (null != resolvedOppositeRolePlayerAlternate &&
											null != (compatibleSupertype = ResolveCompatibleSupertype(resolvedOppositeRolePlayerAlternate, oppositeRolePlayer)))
										{
											resolvedOppositeRolePlayer = compatibleSupertype;
											resolvedOppositeRolePlayerAlternate = null;

											// Use objectification for all roles on the original fact type
											usedObjectifyingRole[0] = true;
											for (int j = 1; j < constraintRoleCount; ++j)
											{
												if (j != i && constraintRoles[j].Role.FactType == firstFactType)
												{
													usedObjectifyingRole[j] = true;
												}
											}
#if BLOCK_TRIVIAL_JOINPATHS
											usesOppositeObjectification = true;
											multipleCompatibleOppositeRolePlayers = true;
#endif // BLOCK_TRIVIAL_JOINPATHS
											continue;
										}
									}
									if (objectifyingRolePlayer != null)
									{
										if (null != (compatibleSupertype = ResolveCompatibleSupertype(resolvedOppositeRolePlayer, objectifyingRolePlayer)))
										{
											resolvedOppositeRolePlayer = compatibleSupertype;
											resolvedOppositeRolePlayerAlternate = null;
											usedObjectifyingRole[i] = true;
#if BLOCK_TRIVIAL_JOINPATHS
											usesOppositeObjectification = true;
											multipleCompatibleOppositeRolePlayers = true;
#endif // BLOCK_TRIVIAL_JOINPATHS
											continue;
										}
										else if (null != resolvedOppositeRolePlayerAlternate &&
											null != (compatibleSupertype = ResolveCompatibleSupertype(resolvedOppositeRolePlayerAlternate, objectifyingRolePlayer)))
										{
											resolvedOppositeRolePlayer = compatibleSupertype;
											resolvedOppositeRolePlayerAlternate = null;
											usedObjectifyingRole[i] = true;
#if BLOCK_TRIVIAL_JOINPATHS
											usesOppositeObjectification = true;
											multipleCompatibleOppositeRolePlayers = true;
#endif // BLOCK_TRIVIAL_JOINPATHS
											continue;
										}
									}
								}
							}
							joinNotNeeded = false;
							break;
						}
					}
					if (joinNotNeeded)
					{
#if BLOCK_TRIVIAL_JOINPATHS

						if (multipleCompatibleOppositeRolePlayers || usesOppositeObjectification)
#else // !BLOCK_TRIVIAL_JOINPATHS
						if (resolvedOppositeRolePlayer != null)
#endif // BLOCK_TRIVIAL_JOINPATHS
						{
							// Build and/or verify an automatic join path centered
							// on resolved object type.
							joinNotNeeded = false;
							if (verifyAutomaticPath)
							{
								if (joinPath != null)
								{
									if (joinPath.IsAutomatic)
									{
										VerifyAutomaticJoinPath(joinPath, resolvedOppositeRolePlayer, constraintRoles, usedObjectifyingRole, notifyAdded);
									}
								}
								else
								{
									joinPath = new ConstraintRoleSequenceJoinPath(
										Store,
										new PropertyAssignment(ConstraintRoleSequenceJoinPath.IsAutomaticDomainPropertyId, true));
									joinPath.RoleSequence = this;
									if (notifyAdded != null)
									{
										notifyAdded.ElementAdded(joinPath, true);
									}
									VerifyAutomaticJoinPath(joinPath, resolvedOppositeRolePlayer, constraintRoles, usedObjectifyingRole, notifyAdded);
								}
							}
						}
					}
					else if (verifyAutomaticPath && (joinPath == null || joinPath.IsAutomatic))
					{
						joinPath = VerifyAutomaticJoinPath(joinPath, constraintRoles, notifyAdded);
					}
				}
			}
			if (joinNotNeeded)
			{
				if (joinPath != null)
				{
					joinPath.Delete();
				}
				if (verifyError && pathRequiredError != null)
				{
					pathRequiredError.Delete();
				}
			}
			else if (verifyError)
			{
				if (joinPath != null)
				{
					if (pathRequiredError != null)
					{
						pathRequiredError.Delete();
					}
				}
				else if (pathRequiredError == null)
				{
					ORMModel model;
					if (null != constraint &&
						null != (model = constraint.Model))
					{
						pathRequiredError = new JoinPathRequiredError(Store);
						pathRequiredError.RoleSequence = this;
						pathRequiredError.Model = model;
						pathRequiredError.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(pathRequiredError, true);
						}
					}
				}
				else if (notifyAdded == null && constraint != null && constraint.ConstraintStorageStyle == ConstraintStorageStyle.SetComparisonConstraint) // All error text is regenerated on load, set comparison sequence number may have changed
				{
					pathRequiredError.GenerateErrorText();
				}
			}
		}
		/// <summary>
		/// Helper method to find a compatible supertype for two object types. Returns null
		/// if the types are not compatible.
		/// </summary>
		private static ObjectType ResolveCompatibleSupertype(ObjectType objectType1, ObjectType objectType2)
		{
			ObjectType[] compatibleTypes = ObjectType.GetNearestCompatibleTypes(new ObjectType[] { objectType1, objectType2 });
			while (compatibleTypes.Length > 1)
			{
				compatibleTypes = ObjectType.GetNearestCompatibleTypes(compatibleTypes);
			}
			if (compatibleTypes.Length != 0)
			{
				return compatibleTypes[0];
			}
			return null;
		}
		/// <summary>
		/// Preliminary data for populating an automatic role path without
		/// populating the actual model.
		/// </summary>
		private struct AutomaticJoinPathData
		{
			public ConstraintRoleSequenceHasRole ProjectedOnConstraintRole;
			public Role Role;
			public PathedRolePurpose Purpose;
			public AutomaticJoinPathData(ConstraintRoleSequenceHasRole constraintRole, Role role, PathedRolePurpose purpose)
			{
				ProjectedOnConstraintRole = constraintRole;
				Role = role;
				Purpose = purpose;
			}
		}
		/// <summary>
		/// Helper method for ValidateJoinPath.
		/// If possible, use the existing model automatically choose fact types to create a
		/// join path based on a sequence of constraint roles.
		/// </summary>
		/// <param name="existingJoinPath">An existing automatic join path to verify.</param>
		/// <param name="constraintRoles">The constraint roles to bind to.</param>
		/// <param name="notifyAdded">Initial load notification callback.</param>
		private ConstraintRoleSequenceJoinPath VerifyAutomaticJoinPath(ConstraintRoleSequenceJoinPath existingJoinPath, ReadOnlyCollection<ConstraintRoleSequenceHasRole> constraintRoles, INotifyElementAdded notifyAdded)
		{
			// Get current information
			int constraintRoleCount = constraintRoles.Count;
			ConstraintRoleSequenceHasRole constraintRole = constraintRoles[0];
			Role previousRole = constraintRole.Role;
			if (previousRole.RolePlayer == null)
			{
				if (existingJoinPath != null)
				{
					existingJoinPath.Delete();
				}
				return null;
			}
			List<AutomaticJoinPathData> pathData = new List<AutomaticJoinPathData>(3 * constraintRoleCount); // Plenty of space
			FactType previousFactType = previousRole.FactType;
			ObjectType previousFactTypeObjectifiedBy = previousFactType.NestingType;
			LinkedElementCollection<RoleBase> previousFactTypeRoles = null;
			int previousFactTypeRoleCount = 0;
			int? previousFactTypeUnaryRoleIndex = null;
			pathData.Add(new AutomaticJoinPathData(constraintRole, previousRole, PathedRolePurpose.PostInnerJoin));
			int lastFactTypeEntryIndex = 0;
			bool incompleteOrAmbiguousPath = false;
			for (int i = 1; i < constraintRoleCount; ++i)
			{
				constraintRole = constraintRoles[i];
				Role currentRole = constraintRole.Role;
				FactType currentFactType = currentRole.FactType;
				if (currentFactType == previousFactType)
				{
					if (currentRole.RolePlayer == null)
					{
						incompleteOrAmbiguousPath = true;
						break;
					}
					pathData.Add(new AutomaticJoinPathData(constraintRole, currentRole, PathedRolePurpose.SameFactType));
					previousRole = currentRole;
				}
				else
				{
					ObjectType currentFactTypeObjectifiedBy = currentFactType.NestingType;
					LinkedElementCollection<RoleBase> currentFactTypeRoles = null;
					int currentFactTypeRoleCount = 0;
					int? currentFactTypeUnaryRoleIndex = null;

					// Peek ahead to see the pattern (one or many constrained roles) for the next fact type
					int constrainedRoleCountInCurrentFactType = 1;
					for (int j = i + 1; j < constraintRoleCount; ++j)
					{
						if (currentFactType != constraintRoles[j].Role.FactType)
						{
							break;
						}
						++constrainedRoleCountInCurrentFactType;
					}

					// Match steps from one fact type to the next
					int constrainedInPreviousFactTypeCount = i - lastFactTypeEntryIndex;
					if (constrainedInPreviousFactTypeCount > 1)
					{
						// We've already stepped at least one role within this fact type.
						// The preferred approach is to join through the role player of
						// the last step. See if we can find exactly one fact type that
						// will get us there.
						ObjectType previousRolePlayer = previousRole.RolePlayer;
						if (previousRolePlayer == null)
						{
							incompleteOrAmbiguousPath = true;
							break;
						}
						bool connectThroughLeftObjectifyingType;
						bool connectThroughRightObjectifyingType;
						if (constrainedRoleCountInCurrentFactType > 1)
						{
							ObjectType currentRolePlayer = currentRole.RolePlayer;
							if (currentRolePlayer == null)
							{
								incompleteOrAmbiguousPath = true;
								break;
							}
							connectThroughLeftObjectifyingType = false;
							if (currentRolePlayer == previousRolePlayer ||
								(connectThroughLeftObjectifyingType = (currentRolePlayer == previousFactTypeObjectifiedBy)))
							{
								if (connectThroughLeftObjectifyingType)
								{
									// Get out of the objectified fact type. The previous role in this
									// context represents a use in the link fact type, not the objectified fact type,
									// so it needs to be repeated with a join.
									pathData.Add(new AutomaticJoinPathData(null, previousRole, PathedRolePurpose.PostInnerJoin));
									// Note that we do not need to deal with unary objectification here because
									// a unary fact type cannot have more than one constrained role.
									pathData.Add(new AutomaticJoinPathData(null, previousRole.OppositeRoleAlwaysResolveProxy.Role, PathedRolePurpose.SameFactType));
								}
								// Shared role player from a SameFactTypeRole to a target. Note that a projection from a
								// PostInnerJoin will back up later to project from the start or entry role this is joined to.
								pathData.Add(new AutomaticJoinPathData(constraintRole, currentRole, PathedRolePurpose.PostInnerJoin));
								previousFactType = currentFactType;
								previousFactTypeObjectifiedBy = currentFactTypeObjectifiedBy;
								previousFactTypeRoles = null; // Also invalidates unary and count locals
								previousRole = currentRole;
								lastFactTypeEntryIndex = i;
								continue;
							}
						}

						// Match unconstrained roles in the target fact type to the current object type
						Role matchedRole = null;
						connectThroughLeftObjectifyingType = false;
						connectThroughRightObjectifyingType = currentFactTypeObjectifiedBy == previousRolePlayer;
						if (currentFactTypeRoles == null)
						{
							currentFactTypeRoles = currentFactType.RoleCollection;
							currentFactTypeRoleCount = currentFactTypeRoles.Count;
							currentFactTypeUnaryRoleIndex = FactType.GetUnaryRoleIndex(currentFactTypeRoles);
						}
						for (int currentFactTypeRoleIndex = 0; currentFactTypeRoleIndex < currentFactTypeRoleCount; ++currentFactTypeRoleIndex)
						{
							Role testRole;
							if (currentFactTypeUnaryRoleIndex.HasValue)
							{
								if (currentFactTypeRoleIndex != currentFactTypeUnaryRoleIndex.Value)
								{
									continue;
								}
								// Use the unary role to proceed, even though it is constrained. Join directly off it.
								testRole = currentFactTypeRoles[currentFactTypeRoleIndex].Role;
							}
							else
							{
								// Look for an unconstrained role
								testRole = currentFactTypeRoles[currentFactTypeRoleIndex].Role;
								int j = 0;
								for (; j < constrainedRoleCountInCurrentFactType; ++j)
								{
									if (constraintRoles[i + j].Role == testRole)
									{
										break;
									}
								}
								if (j != constrainedRoleCountInCurrentFactType)
								{
									continue;
								}
							}
							ObjectType testRolePlayer = testRole.RolePlayer;
							if (testRolePlayer == null)
							{
								incompleteOrAmbiguousPath = true;
								break;
							}
							if (testRolePlayer == previousRolePlayer)
							{
								if (matchedRole == null && !connectThroughRightObjectifyingType)
								{
									matchedRole = testRole;
								}
								else
								{
									incompleteOrAmbiguousPath = true;
									break;
								}
							}
							else if (testRolePlayer == previousFactTypeObjectifiedBy)
							{
								if (matchedRole == null && !connectThroughRightObjectifyingType)
								{
									matchedRole = testRole;
									connectThroughLeftObjectifyingType = true;
								}
								else
								{
									incompleteOrAmbiguousPath = true;
									break;
								}
							}
						}
						if (incompleteOrAmbiguousPath)
						{
							break;
						}
						else if (matchedRole != null || connectThroughRightObjectifyingType)
						{
							if (connectThroughRightObjectifyingType)
							{
								if (currentFactTypeUnaryRoleIndex.HasValue)
								{
									Role objectifiedUnaryRole = currentRole.ObjectifiedUnaryRole;
									pathData.Add(new AutomaticJoinPathData(null, objectifiedUnaryRole.OppositeRole.Role, PathedRolePurpose.PostInnerJoin));
									pathData.Add(new AutomaticJoinPathData(constraintRole, objectifiedUnaryRole, PathedRolePurpose.SameFactType));
								}
								else
								{
									pathData.Add(new AutomaticJoinPathData(null, currentRole.OppositeRoleAlwaysResolveProxy.Role, PathedRolePurpose.PostInnerJoin));
									if (constrainedRoleCountInCurrentFactType > 1)
									{
										// Step across the link fact type then join into the objectified fact type
										pathData.Add(new AutomaticJoinPathData(null, currentRole, PathedRolePurpose.SameFactType));
										pathData.Add(new AutomaticJoinPathData(constraintRole, currentRole, PathedRolePurpose.PostInnerJoin));
									}
									else
									{
										// Project directly on the link fact type
										pathData.Add(new AutomaticJoinPathData(constraintRole, currentRole, PathedRolePurpose.SameFactType));
									}
								}
							}
							else if (matchedRole == currentRole) // Unary case
							{
								pathData.Add(new AutomaticJoinPathData(constraintRole, currentRole, PathedRolePurpose.PostInnerJoin));
							}
							else
							{
								if (connectThroughLeftObjectifyingType)
								{
									if (currentFactTypeUnaryRoleIndex.HasValue)
									{
										Role objectifiedUnaryRole = currentRole.ObjectifiedUnaryRole;
										pathData.Add(new AutomaticJoinPathData(null, objectifiedUnaryRole.OppositeRole.Role, PathedRolePurpose.PostInnerJoin));
										pathData.Add(new AutomaticJoinPathData(constraintRole, objectifiedUnaryRole, PathedRolePurpose.SameFactType));
									}
									else
									{
										pathData.Add(new AutomaticJoinPathData(null, currentRole.OppositeRoleAlwaysResolveProxy.Role, PathedRolePurpose.PostInnerJoin));
										if (constrainedRoleCountInCurrentFactType > 1)
										{
											// Step across the link fact type then join into the objectified fact type
											pathData.Add(new AutomaticJoinPathData(null, currentRole, PathedRolePurpose.SameFactType));
											pathData.Add(new AutomaticJoinPathData(constraintRole, currentRole, PathedRolePurpose.PostInnerJoin));
										}
										else
										{
											// Project directly on the link fact type
											pathData.Add(new AutomaticJoinPathData(constraintRole, currentRole, PathedRolePurpose.SameFactType));
										}
									}
								}
								else
								{
									pathData.Add(new AutomaticJoinPathData(null, matchedRole, PathedRolePurpose.PostInnerJoin));
									pathData.Add(new AutomaticJoinPathData(constraintRole, currentRole, PathedRolePurpose.SameFactType));
								}
							}
							previousFactType = currentFactType;
							previousFactTypeRoles = currentFactTypeRoles;
							previousFactTypeRoleCount = currentFactTypeRoleCount;
							previousFactTypeUnaryRoleIndex = currentFactTypeUnaryRoleIndex;
							previousFactTypeObjectifiedBy = currentFactTypeObjectifiedBy;
							previousRole = currentRole;
							lastFactTypeEntryIndex = i;
							continue;
						}
					}

					// Attempt to match the role player for an unconstrained role on the left.
					// On the right, either match an unconstrained role or the current role if
					// there are multiple roles played in the current fact type.
					Role matchedLeftRole = null;
					Role matchedRightRole = null;
					if (previousFactTypeObjectifiedBy != null)
					{
						if (currentFactTypeRoles == null)
						{
							currentFactTypeRoles = currentFactType.RoleCollection;
							currentFactTypeRoleCount = currentFactTypeRoles.Count;
							currentFactTypeUnaryRoleIndex = FactType.GetUnaryRoleIndex(currentFactTypeRoles);
						}
						for (int rightRoleIndex = 0; rightRoleIndex < currentFactTypeRoleCount; ++rightRoleIndex)
						{
							Role testRightRole;
							if (currentFactTypeUnaryRoleIndex.HasValue)
							{
								if (currentFactTypeUnaryRoleIndex.Value != rightRoleIndex)
								{
									continue;
								}
								testRightRole = currentFactTypeRoles[rightRoleIndex].Role;
							}
							else
							{
								testRightRole = currentFactTypeRoles[rightRoleIndex].Role;
								if (constrainedRoleCountInCurrentFactType == 1 ||
									testRightRole != currentRole)
								{
									int j = 0;
									for (; j < constrainedRoleCountInCurrentFactType; ++j)
									{
										if (constraintRoles[i + j].Role == testRightRole)
										{
											break;
										}
									}
									if (j != constrainedRoleCountInCurrentFactType)
									{
										continue;
									}
								}
							}
							ObjectType testRightRolePlayer = testRightRole.RolePlayer;
							if (testRightRolePlayer == null)
							{
								incompleteOrAmbiguousPath = true;
								break;
							}
							if (testRightRolePlayer == previousFactTypeObjectifiedBy)
							{
								if (matchedLeftRole == null && matchedRightRole == null)
								{
									// The matching left role null indicates
									// an objectification join on the left.
									matchedRightRole = testRightRole;
								}
								else
								{
									incompleteOrAmbiguousPath = true;
									break;
								}
							}
						}
					}
					if (!incompleteOrAmbiguousPath)
					{
						if (previousFactTypeRoles == null)
						{
							previousFactTypeRoles = previousFactType.RoleCollection;
							previousFactTypeRoleCount = previousFactTypeRoles.Count;
							previousFactTypeUnaryRoleIndex = FactType.GetUnaryRoleIndex(previousFactTypeRoles);
						}
						if (matchedRightRole == null)
						{
							for (int leftRoleIndex = 0; leftRoleIndex < previousFactTypeRoleCount; ++leftRoleIndex)
							{
								Role testLeftRole;
								if (previousFactTypeUnaryRoleIndex.HasValue)
								{
									if (leftRoleIndex != previousFactTypeUnaryRoleIndex.Value)
									{
										continue;
									}
									// Use the unary role to proceed, even though it is constrained. Join directly off it.
									testLeftRole = previousFactTypeRoles[leftRoleIndex].Role;
								}
								else
								{
									// Test an unconstrained role
									testLeftRole = previousFactTypeRoles[leftRoleIndex].Role;
									int j = 0;
									for (; j < constrainedInPreviousFactTypeCount; ++j)
									{
										if (constraintRoles[lastFactTypeEntryIndex + j].Role == testLeftRole)
										{
											break;
										}
									}
									if (j != constrainedInPreviousFactTypeCount)
									{
										continue;
									}
								}
								ObjectType testLeftRolePlayer = testLeftRole.RolePlayer;
								if (testLeftRolePlayer == null)
								{
									incompleteOrAmbiguousPath = true;
									break;
								}
								if (currentFactTypeObjectifiedBy == testLeftRolePlayer)
								{
									if (matchedLeftRole == null && matchedRightRole == null)
									{
										// Leave the matchedRightRole null to indicate a join through the objectification.
										// Note that the objectifying entity type cannot be a role player, so if we match
										// here then we will not match a role player as well.
										matchedLeftRole = testLeftRole;
									}
									else
									{
										incompleteOrAmbiguousPath = true;
									}
								}
								else
								{
									if (currentFactTypeRoles == null)
									{
										currentFactTypeRoles = currentFactType.RoleCollection;
										currentFactTypeRoleCount = currentFactTypeRoles.Count;
										currentFactTypeUnaryRoleIndex = FactType.GetUnaryRoleIndex(currentFactTypeRoles);
									}
									for (int rightRoleIndex = 0; rightRoleIndex < currentFactTypeRoleCount; ++rightRoleIndex)
									{
										Role testRightRole;
										if (currentFactTypeUnaryRoleIndex.HasValue)
										{
											if (currentFactTypeUnaryRoleIndex.Value != rightRoleIndex)
											{
												continue;
											}
											testRightRole = currentFactTypeRoles[rightRoleIndex].Role;
										}
										else
										{
											testRightRole = currentFactTypeRoles[rightRoleIndex].Role;
											if (constrainedRoleCountInCurrentFactType == 1 ||
												testRightRole != currentRole)
											{
												int j = 0;
												for (; j < constrainedRoleCountInCurrentFactType; ++j)
												{
													if (constraintRoles[i + j].Role == testRightRole)
													{
														break;
													}
												}
												if (j != constrainedRoleCountInCurrentFactType)
												{
													continue;
												}
											}
										}
										ObjectType testRightRolePlayer = testRightRole.RolePlayer;
										if (testRightRolePlayer == null)
										{
											incompleteOrAmbiguousPath = true;
											break;
										}
										if (testRightRolePlayer == testLeftRolePlayer)
										{
											if (matchedLeftRole == null && matchedRightRole == null)
											{
												matchedLeftRole = testLeftRole;
												matchedRightRole = testRightRole;
											}
											else
											{
												incompleteOrAmbiguousPath = true;
												break;
											}
										}
									}
								}
								if (incompleteOrAmbiguousPath)
								{
									break;
								}
							}
						}
					}
					if (matchedLeftRole == null && matchedRightRole == null)
					{
						incompleteOrAmbiguousPath = true;
					}
					if (incompleteOrAmbiguousPath)
					{
						break;
					}
					if (matchedLeftRole == null)
					{
						// Objectification coming from the left
						if (previousFactTypeUnaryRoleIndex.HasValue)
						{
							Role objectifiedUnaryRole = previousRole.ObjectifiedUnaryRole;
							// We have already added a role for the unary role. Switch this added role to the
							// objectified unary role.
							int replaceIndex = pathData.Count - 1;
							AutomaticJoinPathData unaryRoleData = pathData[replaceIndex];
							unaryRoleData.Role = objectifiedUnaryRole;
							pathData[replaceIndex] = unaryRoleData;
							pathData.Add(new AutomaticJoinPathData(null, objectifiedUnaryRole.OppositeRole.Role, PathedRolePurpose.SameFactType));
						}
						else
						{
							// Only one role was used in the objectified fact type, then not adding an additional
							// join step switches the path to use the link fact type.
							if (constrainedInPreviousFactTypeCount > 1)
							{
								pathData.Add(new AutomaticJoinPathData(null, previousRole, PathedRolePurpose.PostInnerJoin));
							}
							pathData.Add(new AutomaticJoinPathData(null, previousRole.OppositeRoleAlwaysResolveProxy.Role, PathedRolePurpose.SameFactType));
						}
					}
					else if (!previousFactTypeUnaryRoleIndex.HasValue)
					{
						pathData.Add(new AutomaticJoinPathData(null, matchedLeftRole, PathedRolePurpose.SameFactType));
					}
					if (matchedRightRole == currentRole)
					{
						pathData.Add(new AutomaticJoinPathData(constraintRole, currentRole, PathedRolePurpose.PostInnerJoin));
					}
					else if (matchedRightRole != null)
					{
						pathData.Add(new AutomaticJoinPathData(null, matchedRightRole, PathedRolePurpose.PostInnerJoin));
						pathData.Add(new AutomaticJoinPathData(constraintRole, currentRole, PathedRolePurpose.SameFactType));
					}
					else
					{
						if (currentFactTypeRoles == null)
						{
							currentFactTypeRoles = currentFactType.RoleCollection;
							currentFactTypeRoleCount = currentFactTypeRoles.Count;
							currentFactTypeUnaryRoleIndex = FactType.GetUnaryRoleIndex(currentFactTypeRoles);
						}
						if (currentFactTypeUnaryRoleIndex.HasValue)
						{
							// Objectified unary pattern
							Role objectifiedUnaryRole = currentRole.ObjectifiedUnaryRole;
							pathData.Add(new AutomaticJoinPathData(null, objectifiedUnaryRole.OppositeRole.Role, PathedRolePurpose.PostInnerJoin));
							pathData.Add(new AutomaticJoinPathData(constraintRole, objectifiedUnaryRole, PathedRolePurpose.SameFactType));
						}
						else
						{
							// Objectified binary or n-ary pattern
							pathData.Add(new AutomaticJoinPathData(null, currentRole.OppositeRoleAlwaysResolveProxy.Role, PathedRolePurpose.PostInnerJoin));
							if (constrainedRoleCountInCurrentFactType == 1)
							{
								pathData.Add(new AutomaticJoinPathData(constraintRole, currentRole, PathedRolePurpose.SameFactType));
							}
							else
							{
								pathData.Add(new AutomaticJoinPathData(null, currentRole, PathedRolePurpose.SameFactType));
								pathData.Add(new AutomaticJoinPathData(constraintRole, currentRole, PathedRolePurpose.PostInnerJoin));
							}
						}
					}
					previousFactType = currentFactType;
					previousFactTypeRoles = currentFactTypeRoles;
					previousFactTypeRoleCount = currentFactTypeRoleCount;
					previousFactTypeUnaryRoleIndex = currentFactTypeUnaryRoleIndex;
					previousFactTypeObjectifiedBy = currentFactTypeObjectifiedBy;
					previousRole = currentRole;
					lastFactTypeEntryIndex = i;
				}
			}
			if (incompleteOrAmbiguousPath)
			{
				if (existingJoinPath != null)
				{
					existingJoinPath.Delete();
				}
				return null;
			}

			// Fix up the permanent model based on the calculated path
			ConstraintRoleSequenceJoinPath retVal = existingJoinPath;
			LeadRolePath leadRolePath = null;
			RolePathObjectTypeRoot rootObjectTypeLink = null;
			ObjectType rootObjectType = constraintRoles[0].Role.RolePlayer;
			ReadOnlyCollection<PathedRole> pathedRoles = null;
			int pathedRoleCount = 0;
			Store store = Store;
			if (retVal != null)
			{
				// The only thing we want in the join path is a lead path with the initial
				// object type as the root. This is a straight join path: there are no splits
				// or conditions.
				
				// Find an appropriate lead role path to put in automatic form. If the path is shared,
				// then we do not want to delete it outright or make it automatic. Detaching a shared
				// path from its owner allows it to be reattached with another owner that currently shares it.
				foreach (RolePathOwnerOwnsLeadRolePath rolePathLink in RolePathOwnerOwnsLeadRolePath.GetLinksToLeadRolePathCollection(retVal))
				{
					LeadRolePath testRolePath;
					if (leadRolePath == null &&
						0 == (testRolePath = rolePathLink.RolePath).SharedWithPathOwnerCollection.Count)
					{
						leadRolePath = testRolePath;
					}
					else
					{
						rolePathLink.Delete();
					}
				}
				retVal.SharedLeadRolePathCollection.Clear();
				if (leadRolePath != null)
				{
					rootObjectTypeLink = leadRolePath.PathRoot;
					if (rootObjectTypeLink == null)
					{
						rootObjectTypeLink = new RolePathObjectTypeRoot(leadRolePath, rootObjectType);
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(rootObjectTypeLink, false);
						}
					}
					else if (rootObjectTypeLink.RootObjectType != rootObjectType)
					{
						rootObjectTypeLink.RootObjectType = rootObjectType;
					}
					leadRolePath.CalculatedValueCollection.Clear();
					leadRolePath.SubPathCollection.Clear();
					leadRolePath.SplitCombinationOperator = LogicalCombinationOperator.And;
					leadRolePath.SplitIsNegated = false;
					pathedRoles = leadRolePath.PathedRoleCollection;
					pathedRoleCount = pathedRoles.Count;
				}
			}
			else
			{
				retVal = new ConstraintRoleSequenceJoinPath(
					store,
					new PropertyAssignment(ConstraintRoleSequenceJoinPath.IsAutomaticDomainPropertyId, true));
				retVal.RoleSequence = this;
				if (notifyAdded != null)
				{
					notifyAdded.ElementAdded(retVal, true);
				}
			}
			if (leadRolePath == null)
			{
				leadRolePath = new LeadRolePath(store);
				leadRolePath.PathOwner = retVal;
				rootObjectTypeLink = new RolePathObjectTypeRoot(leadRolePath, rootObjectType);
				if (notifyAdded != null)
				{
					notifyAdded.ElementAdded(leadRolePath, true);
				}
			}
			ConstraintRoleSequenceJoinPathProjection pathProjection = null;
			int stepCount = pathData.Count;
			PathedRole lastBindablePathedRole = null;
			for (int i = 0; i < stepCount; ++i)
			{
				AutomaticJoinPathData stepData = pathData[i];
				PathedRolePurpose stepPurpose = stepData.Purpose;
				Role stepRole = stepData.Role;
				PathedRole pathedRole = null;
				if (i < pathedRoleCount)
				{
					PathedRole existingPathedRole = pathedRoles[i];
					if (existingPathedRole.Role == stepRole)
					{
						PathedRolePurpose existingPurpose = existingPathedRole.PathedRolePurpose;
						switch (existingPurpose)
						{
							case PathedRolePurpose.SameFactType:
							case PathedRolePurpose.PostInnerJoin:
								if (existingPurpose == stepPurpose)
								{
									pathedRole = existingPathedRole;
									pathedRole.IsNegated = false;
								}
								break;
							case PathedRolePurpose.PostOuterJoin:
								if (stepPurpose == PathedRolePurpose.PostInnerJoin)
								{
									// Just switch an inner to an outer. Note that stepPurpose will never ask for an outer join.
									pathedRole = existingPathedRole;
									pathedRole.PathedRolePurpose = PathedRolePurpose.PostInnerJoin;
									pathedRole.IsNegated = false;
								}
								break;
						}
					}
					if (pathedRole == null)
					{
						for (int j = pathedRoleCount - 1; j >= i; --j)
						{
							pathedRoles[j].Delete();
						}
						pathedRoleCount = i;
					}
				}
				if (pathedRole == null)
				{
					pathedRole = stepPurpose == PathedRolePurpose.SameFactType ?
						new PathedRole(leadRolePath, stepRole) :
						new PathedRole(
							store,
							new RoleAssignment[]{
								new RoleAssignment(PathedRole.RolePathDomainRoleId, leadRolePath),
								new RoleAssignment(PathedRole.RoleDomainRoleId, stepRole)},
							new PropertyAssignment[]{
								new PropertyAssignment(PathedRole.PathedRolePurposeDomainPropertyId, stepPurpose)});
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(pathedRole, false);
					}
				}
				switch (stepPurpose)
				{
					case PathedRolePurpose.SameFactType:
						lastBindablePathedRole = pathedRole;
						break;
				}
				// UNDONE: For a unary role in the path, we need a 'when true' condition on the opposite
				// implied unary role.
				ConstraintRoleSequenceHasRole projectOnConstraintRole = stepData.ProjectedOnConstraintRole;
				if (projectOnConstraintRole != null)
				{
					ConstraintRoleProjection roleProjection = null;
					ConstraintRoleProjectedFromRolePathRoot pathRootProjectionLink = null;
					ConstraintRoleProjectedFromPathedRole pathedRoleProjectionLink = null;
					if (null == pathProjection &&
						null == (pathProjection = ConstraintRoleSequenceJoinPathProjection.GetLink(retVal, leadRolePath)))
					{
						pathProjection = new ConstraintRoleSequenceJoinPathProjection(retVal, leadRolePath);
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(pathProjection);
						}
					}
					else
					{
						roleProjection = ConstraintRoleProjection.GetLink(pathProjection, projectOnConstraintRole);
						if (roleProjection != null)
						{
							if (lastBindablePathedRole == null)
							{
								pathRootProjectionLink = ConstraintRoleProjectedFromRolePathRoot.GetLinkToProjectedFromPathRoot(roleProjection);
							}
							else
							{
								pathedRoleProjectionLink = ConstraintRoleProjectedFromPathedRole.GetLinkToProjectedFromPathedRole(roleProjection);
							}
						}
					}
					if (roleProjection == null)
					{
						roleProjection = new ConstraintRoleProjection(pathProjection, projectOnConstraintRole);
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(roleProjection);
						}
					}

					if (lastBindablePathedRole == null)
					{
						// Use the path root for the first projection
						if (pathRootProjectionLink == null)
						{
							pathRootProjectionLink = new ConstraintRoleProjectedFromRolePathRoot(roleProjection, rootObjectTypeLink);
							if (notifyAdded != null)
							{
								notifyAdded.ElementAdded(pathRootProjectionLink);
								// Rules aren't turned on, so the other projection options won't clear automatically
								roleProjection.ProjectedFromCalculatedValue = null;
								roleProjection.ProjectedFromPathedRole = null;
								roleProjection.ProjectedFromConstant = null;
							}
						}
						else if (pathRootProjectionLink.Source != rootObjectTypeLink)
						{
							pathRootProjectionLink.Source = rootObjectTypeLink;
						}
					}
					else if (pathedRoleProjectionLink == null)
					{
						pathedRoleProjectionLink = new ConstraintRoleProjectedFromPathedRole(roleProjection, lastBindablePathedRole);
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(pathedRoleProjectionLink);
							// Rules aren't turned on, so the other projection options won't clear automatically
							roleProjection.ProjectedFromCalculatedValue = null;
							roleProjection.ProjectedFromPathRoot = null;
							roleProjection.ProjectedFromConstant = null;
						}
					}
					else if (pathedRoleProjectionLink.Source != lastBindablePathedRole)
					{
						pathedRoleProjectionLink.Source = lastBindablePathedRole;
					}
				}
			}
			for (int i = pathedRoleCount - 1; i >= stepCount; --i)
			{
				pathedRoles[i].Delete();
			}
			return retVal;
		}
		/// <summary>
		/// Helper method for ValidateJoinPath.
		/// Verify that a join path based on a single shared role player
		/// has the correct structure.
		/// </summary>
		/// <param name="joinPath">An automatic <see cref="ConstraintRoleSequenceJoinPath"/></param>
		/// <param name="resolvedObjectType">The shared object type, already established as the role player
		/// or a supertype of the role player for all roles opposite the far roles.</param>
		/// <param name="constraintRoles">The constraint roles to bind to.</param>
		/// <param name="useObjectifyingRole">Precalculated bit tracker with values set to true if the
		/// opposite role should be resolved as the objectifying role.</param>
		/// <param name="notifyAdded">Initial load notification callback.</param>
		private static void VerifyAutomaticJoinPath(ConstraintRoleSequenceJoinPath joinPath, ObjectType resolvedObjectType, ReadOnlyCollection<ConstraintRoleSequenceHasRole> constraintRoles, BitTracker useObjectifyingRole, INotifyElementAdded notifyAdded)
		{
			// The only thing we want in the join path is a lead path with the resolved object type as the root
			// and an 'and' tail split.
			Store store = joinPath.Store;

			// Find an appropriate lead role path to put in automatic form. If the path is shared,
			// then we do not want to delete it outright or make it automatic. Detaching a shared
			// path from its owner allows it to be reattached with another owner that currently shares it.
			LeadRolePath leadRolePath = null;
			foreach (RolePathOwnerOwnsLeadRolePath rolePathLink in RolePathOwnerOwnsLeadRolePath.GetLinksToLeadRolePathCollection(joinPath))
			{
				LeadRolePath testRolePath;
				if (leadRolePath == null &&
					0 == (testRolePath = rolePathLink.RolePath).SharedWithPathOwnerCollection.Count)
				{
					leadRolePath = testRolePath;
				}
				else
				{
					rolePathLink.Delete();
				}
			}
			joinPath.SharedLeadRolePathCollection.Clear();
			
			LinkedElementCollection<RoleSubPath> subPaths;
			if (leadRolePath == null)
			{
				leadRolePath = new LeadRolePath(store);
				leadRolePath.PathOwner = joinPath;
				leadRolePath.RootObjectType = resolvedObjectType;
				if (notifyAdded != null)
				{
					notifyAdded.ElementAdded(leadRolePath, true);
				}
				subPaths = leadRolePath.SubPathCollection;
			}
			else
			{
				leadRolePath.CalculatedValueCollection.Clear();
				LinkedElementCollection<Role> leadRoles = leadRolePath.RoleCollection;
				subPaths = leadRolePath.SubPathCollection;
				if (leadRoles.Count != 0)
				{
					// Wrong pattern, clear all
					leadRoles.Clear();
					subPaths.Clear();
				}
				leadRolePath.SplitCombinationOperator = LogicalCombinationOperator.And;
				leadRolePath.SplitIsNegated = false;
			}
			// We need one subpath per role with an entry on the near role and the far
			// role projected on the constraint role.
			ConstraintRoleSequenceJoinPathProjection pathProjection = null;
			ConstraintRoleProjection roleProjection;
			ConstraintRoleProjectedFromPathedRole projectionLink;
			int roleCount = constraintRoles.Count;
			int subPathCount = subPaths.Count;
			int firstUnverifiedSubpathIndex = 0;
			for (int i = 0; i < roleCount; ++i)
			{
				ConstraintRoleSequenceHasRole roleLink = constraintRoles[i];
				Role farRole = roleLink.Role;
				Role nearRole = (Role)(useObjectifyingRole[i] ? farRole.OppositeRoleAlwaysResolveProxy : farRole.OppositeRole);
				if (firstUnverifiedSubpathIndex < subPathCount)
				{
					// See if we can find an existing subpath that begins with the correct role
					int j = firstUnverifiedSubpathIndex;
					for (; j < subPathCount; ++j)
					{
						RoleSubPath matchSubPath = subPaths[j];
						LinkedElementCollection<Role> testRoles = matchSubPath.RoleCollection;
						int testRoleCount = testRoles.Count;
						if (testRoleCount != 0)
						{
							if (testRoles[0] == nearRole)
							{
								// Use this subpath, fixup and position correctly
								if (j > firstUnverifiedSubpathIndex)
								{
									subPaths.Move(j, firstUnverifiedSubpathIndex);
								}
								++firstUnverifiedSubpathIndex;
								PathedRole farPathedRole;
								if (testRoleCount == 1)
								{
									farPathedRole = new PathedRole(matchSubPath, farRole);
									if (notifyAdded != null)
									{
										notifyAdded.ElementAdded(farPathedRole, false);
									}
								}
								else if (!(farRole == testRoles[1] &&
									(farPathedRole = matchSubPath.PathedRoleCollection[1]).PathedRolePurpose == PathedRolePurpose.SameFactType))
								{
									for (int k = testRoleCount; k >= 1; --k)
									{
										testRoles.RemoveAt(k);
									}
									farPathedRole = new PathedRole(matchSubPath, farRole);
									if (notifyAdded != null)
									{
										notifyAdded.ElementAdded(farPathedRole, false);
									}
								}

								roleProjection = null;
								projectionLink = null;
								if (null == pathProjection &&
									null == (pathProjection = ConstraintRoleSequenceJoinPathProjection.GetLink(joinPath, leadRolePath)))
								{
									pathProjection = new ConstraintRoleSequenceJoinPathProjection(joinPath, leadRolePath);
									if (notifyAdded != null)
									{
										notifyAdded.ElementAdded(pathProjection);
									}
								}
								else
								{
									roleProjection = ConstraintRoleProjection.GetLink(pathProjection, roleLink);
									if (roleProjection != null)
									{
										projectionLink = ConstraintRoleProjectedFromPathedRole.GetLinkToProjectedFromPathedRole(roleProjection);
									}
								}
								if (roleProjection == null)
								{
									roleProjection = new ConstraintRoleProjection(pathProjection, roleLink);
									if (notifyAdded != null)
									{
										notifyAdded.ElementAdded(roleProjection);
									}
								}
								if (projectionLink == null)
								{
									projectionLink = new ConstraintRoleProjectedFromPathedRole(roleProjection, farPathedRole);
									if (notifyAdded != null)
									{
										notifyAdded.ElementAdded(projectionLink);
										// Rules aren't turned on, so the other projection options won't clear automatically
										roleProjection.ProjectedFromCalculatedValue = null;
										roleProjection.ProjectedFromPathRoot = null;
										roleProjection.ProjectedFromConstant = null;
									}
								}
								else if (projectionLink.Source != farPathedRole)
								{
									projectionLink.Source = farPathedRole;
								}
								matchSubPath.RootObjectType = null;
								matchSubPath.SubPathCollection.Clear();
								break;
							}
						}
					}
					if (j < subPathCount)
					{
						continue;
					}
				}
				RoleSubPath subPath = new RoleSubPath(store);
				if (firstUnverifiedSubpathIndex < subPathCount)
				{
					subPaths.Insert(i, subPath);
				}
				else
				{
					subPaths.Add(subPath);
				}
				++firstUnverifiedSubpathIndex;
				++subPathCount;
				new PathedRole(
					store,
					new RoleAssignment[]{
						new RoleAssignment(PathedRole.RolePathDomainRoleId, subPath),
						new RoleAssignment(PathedRole.RoleDomainRoleId, nearRole)},
					new PropertyAssignment[]{
						new PropertyAssignment(PathedRole.PathedRolePurposeDomainPropertyId, PathedRolePurpose.PostInnerJoin)});
				PathedRole newFarPathedRole = new PathedRole(subPath, farRole);
				if (notifyAdded != null)
				{
					notifyAdded.ElementAdded(subPath, true);
				}

				// Project the far role on the constraint role sequence role
				roleProjection = null;
				projectionLink = null;
				if (null == pathProjection &&
					null == (pathProjection = ConstraintRoleSequenceJoinPathProjection.GetLink(joinPath, leadRolePath)))
				{
					pathProjection = new ConstraintRoleSequenceJoinPathProjection(joinPath, leadRolePath);
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(pathProjection);
					}
				}
				else
				{
					roleProjection = ConstraintRoleProjection.GetLink(pathProjection, roleLink);
					if (roleProjection != null)
					{
						projectionLink = ConstraintRoleProjectedFromPathedRole.GetLinkToProjectedFromPathedRole(roleProjection);
					}
				}
				if (roleProjection == null)
				{
					roleProjection = new ConstraintRoleProjection(pathProjection, roleLink);
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(roleProjection);
					}
				}
				if (projectionLink == null)
				{
					projectionLink = new ConstraintRoleProjectedFromPathedRole(roleProjection, newFarPathedRole);
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(projectionLink);
						// Rules aren't turned on, so the other projection options won't clear automatically
						roleProjection.ProjectedFromConstant = null;
						roleProjection.ProjectedFromPathRoot = null;
						roleProjection.ProjectedFromCalculatedValue = null;
					}
				}
				else if (projectionLink.Source != newFarPathedRole)
				{
					projectionLink.Source = newFarPathedRole;
				}
			}

			// Clean up any unused subpaths
			for (int i = subPathCount - 1; i >= firstUnverifiedSubpathIndex; --i)
			{
				subPaths[i].Delete();
			}
		}
		#endregion // JoinPath Validation
		#region ConstraintRoleSequence Specific
		/// <summary>
		/// Get the constraint that owns this role sequence
		/// </summary>
		public abstract IConstraint Constraint
		{
			get;
		}
		#endregion // ConstraintRoleSequence Specific
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Returns the error associated with the constraint.
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			ModelErrorUses startFilter = filter;
			if (filter == 0)
			{
				filter = (ModelErrorUses)(-1);
			}
			// UNDONE: This should eventually be ModelErrorUses.BlockVerbalization for the
			// JoinPathRequiredError, but we need verbalization with the join path before
			// we refuse to verbalize at all.
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
			{
				JoinPathRequiredError joinPathRequiredError = JoinPathRequiredError;
				if (joinPathRequiredError != null)
				{
					yield return joinPathRequiredError;
				}

				ConstraintRoleSequenceJoinPath joinPath = JoinPath;
				if (joinPath != null)
				{
					foreach (ModelErrorUsage derivationError in ((IModelErrorOwner)joinPath).GetErrorCollection(startFilter))
					{
						yield return derivationError;
					}
				}
			}
			// Get errors off the base
			foreach (ModelErrorUsage baseError in base.GetErrorCollection(startFilter))
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
			ValidateJoinPath(notifyAdded, true, false);
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
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateJoinPath);
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner Implementation
	}
	public partial class SetComparisonConstraintRoleSequence : IHasIndirectModelErrorOwner, IModelErrorDisplayContext
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
		#region Error validation
		/// <summary>
		/// Called during initial validation of a <see cref="SetComparisonConstraint"/> to
		/// verify intersecting constraints.
		/// </summary>
		public static void ValidateIntersectingConstraints(IConstraint constraint, INotifyElementAdded notifyAdded)
		{
			ValidateConstraintPatternError(constraint, notifyAdded);
		}
		#endregion // Error validation
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { SetComparisonConstraintHasRoleSequence.RoleSequenceDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
		#region IModelErrorDisplayContext Implementation
		/// <summary>
		/// Implements <see cref="IModelErrorDisplayContext.ErrorDisplayContext"/>
		/// </summary>
		string IModelErrorDisplayContext.ErrorDisplayContext
		{
			get
			{
				string modelName = null;
				int sequenceNumber = 0;
				string constraintName = null;
				SetComparisonConstraint constraint = ExternalConstraint;
				if (constraint != null)
				{
					constraintName = constraint.Name;
					ORMModel model = constraint.Model;
					if (model != null)
					{
						modelName = model.Name;
					}
					sequenceNumber = constraint.RoleSequenceCollection.IndexOf(this) + 1;
				}
				return string.Format(CultureInfo.CurrentCulture, ResourceStrings.ModelErrorDisplayContextSetComparisonConstraintSequence, constraintName ?? "", modelName ?? "", sequenceNumber);
			}
		}
		#endregion // IModelErrorDisplayContext Implementation
	}
	public partial class SetConstraint
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
			if (!IsDeleting && !IsDeleted)
			{
				// This is a bit tricky because we always have to look
				// at the links to test removing, so we can't use
				// the generated property accessors unless they have
				// remove propagation set on the opposite end.
				bool remove = true;
				UniquenessConstraint constraint = PreferredIdentifier;
				if (!constraint.IsDeleting && !constraint.IsDeleted)
				{
					ObjectType forType = PreferredIdentifierFor;
					if (!forType.IsDeleting && !forType.IsDeleted)
					{
						Objectification objectification = forType.Objectification;
						if (constraint.IsInternal)
						{
							ReadOnlyCollection<ConstraintRoleSequenceHasRole> constraintRoleLinks = DomainRoleInfo.GetElementLinks<ConstraintRoleSequenceHasRole>(constraint, ConstraintRoleSequenceHasRole.ConstraintRoleSequenceDomainRoleId);
							int constraintRoleLinkCount = constraintRoleLinks.Count;
							if (constraintRoleLinkCount == 0)
							{
								// Intentionally empty
							}
							else if (objectification != null &&
								objectification.NestedFactType == constraint.FactTypeCollection[0])
							{
								remove = objectification.IsDeleting;
							}
							else
							{
								// ConstraintRoleSequenceHasRole is not a propagate delete
								// relationship, so the link can be gone without the role
								// being removed. Get the links first instead of using the
								// generated counterpart collection.
								ConstraintRoleSequenceHasRole constraintRoleLink;
								LinkedElementCollection<RoleBase> factRoles;
								Role constraintRole;
								FactType factType;
								if (1 == constraintRoleLinkCount &&
									!(constraintRoleLink = constraintRoleLinks[0]).IsDeleting &&
									!(constraintRole = constraintRoleLink.Role).IsDeleting &&
									null != (factType = constraintRole.FactType) &&
									!factType.IsDeleting &&
									2 == (factRoles = factType.RoleCollection).Count)
								{
									// Make sure we have exactly one additional single-roled internal
									// uniqueness constraint on the opposite role, and that the
									// opposite role is mandatory, and that the role player is still
									// connected.
									Role oppositeRole = factRoles[0].Role;
									if (oppositeRole == constraintRole)
									{
										oppositeRole = factRoles[1].Role;
									}

									// Test for attached object type. It is very common
									// to edit the link directly, so we need to check the
									// link itself, not the counterpart.
									bool rolePlayerOK = false;
									if (!oppositeRole.IsDeleting)
									{
										ObjectTypePlaysRole rolePlayerLink = ObjectTypePlaysRole.GetLinkToRolePlayer(oppositeRole);
										if (rolePlayerLink != null &&
											!rolePlayerLink.IsDeleting &&
											forType == rolePlayerLink.RolePlayer)
										{
											rolePlayerOK = true;
										}
									}

									if (rolePlayerOK)
									{
										bool haveOppositeUniqueness = false;
										bool haveOppositeMandatory = false;
										ReadOnlyCollection<ConstraintRoleSequenceHasRole> oppositeRoleConstraintLinks = DomainRoleInfo.GetElementLinks<ConstraintRoleSequenceHasRole>(oppositeRole, ConstraintRoleSequenceHasRole.RoleDomainRoleId);
										int oppositeRoleConstraintLinkCount = oppositeRoleConstraintLinks.Count;
										for (int i = 0; i < oppositeRoleConstraintLinkCount; ++i)
										{
											ConstraintRoleSequenceHasRole oppositeRoleConstraintLink = oppositeRoleConstraintLinks[i];
											ConstraintRoleSequence testRoleSequence;
											SetConstraint testConstraint;
											if (!oppositeRoleConstraintLink.IsDeleting &&
												!(testRoleSequence = oppositeRoleConstraintLink.ConstraintRoleSequence).IsDeleting &&
												null != (testConstraint = testRoleSequence as SetConstraint) &&
												testConstraint.Modality == ConstraintModality.Alethic)
											{
												switch (testConstraint.Constraint.ConstraintType)
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
						}
						else
						{
							// See list of conditions in UniquenessConstraint.TestAllowPreferred
							ReadOnlyCollection<ConstraintRoleSequenceHasRole> constraintRoleLinks = DomainRoleInfo.GetElementLinks<ConstraintRoleSequenceHasRole>(constraint, ConstraintRoleSequenceHasRole.ConstraintRoleSequenceDomainRoleId);
							ConstraintRoleSequenceHasRole constraintRoleLink;
							Role constraintRole;
							int allRolesCount = constraintRoleLinks.Count;
							RoleProxy proxy;
							FactType impliedFact;
							int remainingRolesCount = 0;
							FactType factType;
							int proxyRoleCount = 0;
							if (objectification != null && objectification.IsDeleting)
							{
								objectification = null;
							}
							for (int i = 0; i < allRolesCount; ++i)
							{
								if (!(constraintRoleLink = constraintRoleLinks[i]).IsDeleting &&
									!(constraintRole = constraintRoleLink.Role).IsDeleting)
								{
									++remainingRolesCount;
									if (objectification != null &&
										null != (proxy = constraintRole.Proxy) &&
										!proxy.IsDeleting &&
										null != (impliedFact = proxy.FactType) &&
										!impliedFact.IsDeleting &&
										impliedFact.ImpliedByObjectification == objectification)
									{
										++proxyRoleCount;
									}
								}
							}
							if (remainingRolesCount != 0) // Condition 1
							{
								int remainingFactsCount = 0;
								ReadOnlyCollection<FactSetConstraint> factSetConstraints = DomainRoleInfo.GetElementLinks<FactSetConstraint>(constraint, FactSetConstraint.SetConstraintDomainRoleId);
								int factSetConstraintCount = factSetConstraints.Count;
								for (int i = 0; i < factSetConstraintCount; ++i)
								{
									FactSetConstraint factConstraint = factSetConstraints[i];
									if (!factConstraint.IsDeleting)
									{
										factType = factConstraint.FactType;
										if (!factType.IsDeleting)
										{
											LinkedElementCollection<ConstraintRoleSequenceHasRole> attachedLinks = factConstraint.ConstrainedRoleCollection;
											int attachedLinksCount = attachedLinks.Count;
											for (int j = 0; j < attachedLinksCount; ++j)
											{
												if (!attachedLinks[j].IsDeleting)
												{
													++remainingFactsCount;
													break;
												}
											}
										}
									}
								}
								if ((remainingFactsCount + ((proxyRoleCount == 0) ? 0 : (proxyRoleCount - 1))) == remainingRolesCount) // Condition 2
								{
									int constraintRoleIndex = 0;
									for (; constraintRoleIndex < allRolesCount; ++constraintRoleIndex)
									{
										if (!(constraintRoleLink = (ConstraintRoleSequenceHasRole)constraintRoleLinks[constraintRoleIndex]).IsDeleting &&
											!(constraintRole = constraintRoleLink.Role).IsDeleting)
										{
											if (objectification != null &&
												null != (proxy = constraintRole.Proxy) &&
												!proxy.IsDeleting &&
												null != (impliedFact = proxy.FactType) &&
												!impliedFact.IsDeleting &&
												impliedFact.ImpliedByObjectification == objectification)
											{
												// The remaining binary fact conditions are verified
												// by the objectification pattern on implied facts.
												continue;
											}
											factType = constraintRole.FactType;
											LinkedElementCollection<RoleBase> factRoles = factType.RoleCollection;
											if (factRoles.Count != 2)
											{
												break;
											}
											Role oppositeRole = factRoles[0].Role;
											if (oppositeRole == constraintRole)
											{
												oppositeRole = factRoles[1].Role;
											}
											if (oppositeRole.IsDeleting)
											{
												break;
											}
											ObjectType currentRolePlayer = null;
											// Don't use oppositeRole.RolePlayer, this will pick up
											// a removing role player, which is exactly the condition we're
											// looking for.
											ReadOnlyCollection<ObjectTypePlaysRole> rolePlayerLinks = DomainRoleInfo.GetElementLinks<ObjectTypePlaysRole>(oppositeRole, ObjectTypePlaysRole.PlayedRoleDomainRoleId);
											int rolePlayerLinksCount = rolePlayerLinks.Count;
											for (int i = 0; i < rolePlayerLinksCount; ++i)
											{
												ObjectTypePlaysRole rolePlayerLink = rolePlayerLinks[i];
												if (!rolePlayerLink.IsDeleting)
												{
													ObjectType testRolePlayer = rolePlayerLink.RolePlayer;
													if (!testRolePlayer.IsDeleting)
													{
														currentRolePlayer = testRolePlayer;
														break;
													}
												}
											}
											if (forType != currentRolePlayer)
											{
												break; // Condition 4
											}
											bool haveSingleRoleInternalUniqueness = false;
											foreach (ConstraintRoleSequence oppositeSequence in oppositeRole.ConstraintRoleSequenceCollection)
											{
												UniquenessConstraint oppositeUniqueness = oppositeSequence as UniquenessConstraint;
												if (oppositeUniqueness != null && oppositeUniqueness.IsInternal && oppositeUniqueness.Modality == ConstraintModality.Alethic)
												{
													ReadOnlyCollection<ConstraintRoleSequenceHasRole> roleLinks = DomainRoleInfo.GetElementLinks<ConstraintRoleSequenceHasRole>(oppositeSequence, ConstraintRoleSequenceHasRole.ConstraintRoleSequenceDomainRoleId);
													int roleLinkCount = roleLinks.Count;
													int remainingCount = 0;
													for (int i = 0; i < roleLinkCount; ++i)
													{
														ConstraintRoleSequenceHasRole roleLink = roleLinks[i];
														if (!roleLink.IsDeleting)
														{
															++remainingCount;
														}
													}
													if (remainingCount == 1)
													{
														haveSingleRoleInternalUniqueness = true;
														continue; // Not a condition from TestAllowPreferred, but set in rule when constraint was added
													}
												}
											}
											if (!haveSingleRoleInternalUniqueness)
											{
												break;
											}
										}
									}
									if (constraintRoleIndex == allRolesCount)
									{
										// All roles verified
										remove = false;
									}
								}
							}
							if (!remove)
							{
								forType.ValidateMandatoryRolesForPreferredIdentifier();
							}
						}
					}
				}
				if (remove)
				{
					this.Delete();
				}
			}
		}
		#endregion // Remove testing for preferred identifier
		#region TestRemovePreferredIdentifierDeletingRule
		/// <summary>
		/// DeletingRule: typeof(ObjectTypePlaysRole)
		/// DeletingRule: typeof(ConstraintRoleSequenceHasRole)
		/// A rule to determine if a mandatory condition for
		/// a preferred identifier link has been eliminated.
		/// Remove the preferred identifier if this happens.
		/// </summary>
		private static void TestRemovePreferredIdentifierDeletingRule(ElementDeletingEventArgs e)
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
				IConstraint constraint;
				if (null != (constraint = roleConstraintLink.ConstraintRoleSequence.Constraint))
				{
					switch (constraint.ConstraintType)
					{
						case ConstraintType.DisjunctiveMandatory:
						case ConstraintType.InternalUniqueness:
						case ConstraintType.SimpleMandatory:
							Role role = roleConstraintLink.Role;
							if (role != null)
							{
								rolePlayer = role.RolePlayer;
							}
							break;
						case ConstraintType.ExternalUniqueness:
							rolePlayer = constraint.PreferredIdentifierFor;
							break;
					}
				}
			}
			if (rolePlayer != null && !rolePlayer.IsDeleting)
			{
				EntityTypeHasPreferredIdentifier identifierLink = EntityTypeHasPreferredIdentifier.GetLinkToPreferredIdentifier(rolePlayer);
				if (identifierLink != null)
				{
					identifierLink.TestRemovePreferredIdentifier();
				}
			}
		}
		#endregion // TestRemovePreferredIdentifierDeletingRule
		#region TestRemovePreferredIdentifierRolePlayerChangeRule
		/// <summary>
		/// RolePlayerChangeRule: typeof(ObjectTypePlaysRole)
		/// A rule to determine if a mandatory condition for
		/// a preferred identifier link has been eliminated.
		/// Remove the preferred identifier if this happens.
		/// </summary>
		private static void TestRemovePreferredIdentifierRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			if (e.DomainRole.Id == ObjectTypePlaysRole.RolePlayerDomainRoleId)
			{
				ObjectType rolePlayer = (ObjectType)e.OldRolePlayer;
				EntityTypeHasPreferredIdentifier identifierLink;
				if (!rolePlayer.IsDeleted &&
					null != (identifierLink = EntityTypeHasPreferredIdentifier.GetLinkToPreferredIdentifier(rolePlayer)))
				{
					identifierLink.TestRemovePreferredIdentifier();
				}
			}
		}
		#endregion // TestRemovePreferredIdentifierRolePlayerChangeRule
		#region TestRemovePreferredIdentifierRoleAddRule
		/// <summary>
		/// AddRule: typeof(FactTypeHasRole)
		/// A rule to determine if a role has been added to a fact that
		/// has a preferred identifier attached to one of its constraints.
		/// </summary>
		private static void TestRemovePreferredIdentifierRoleAddRule(ElementAddedEventArgs e)
		{
			FactTypeHasRole roleLink = e.ModelElement as FactTypeHasRole;
			FactType fact = roleLink.FactType;
			foreach (IFactConstraint factConstraint in fact.FactConstraintCollection)
			{
				UniquenessConstraint constraint = factConstraint.Constraint as UniquenessConstraint;
				if (constraint != null)
				{
					ObjectType forType = constraint.PreferredIdentifierFor;
					if (forType != null)
					{
						Objectification objectification;
						if (!(null != (objectification = forType.Objectification) &&
							fact == objectification.NestedFactType))
						{
							// If the preferred identifier is already there, then
							// the fact is binary and removing the role will
							// invalidate the prerequisites. Remove the identifier.
							// Note that the setter for most of the constraint implementations
							// is empty, this will only apply to internal and external
							// uniqueness constraints.
							constraint.PreferredIdentifierFor = null;
						}
					}
				}
			}
		}
		#endregion // TestRemovePreferredIdentifierRoleAddRule
		#region TestRemovePreferredIdentifierConstraintRoleAddRule
		/// <summary>
		/// AddRule: typeof(ConstraintRoleSequenceHasRole)
		/// A rule to determine if a role has been added to a constraint
		/// that is acting as a preferred identifier
		/// </summary>
		private static void TestRemovePreferredIdentifierConstraintRoleAddRule(ElementAddedEventArgs e)
		{
			ConstraintRoleSequenceHasRole constraintLink = e.ModelElement as ConstraintRoleSequenceHasRole;
			ConstraintRoleSequence sequence = constraintLink.ConstraintRoleSequence;
			IConstraint constraint = sequence.Constraint;
			if (constraint != null)
			{
				switch (constraint.ConstraintType)
				{
					case ConstraintType.InternalUniqueness:
						ObjectType preferredFor = constraint.PreferredIdentifierFor;
						if (preferredFor != null)
						{
							FactType nestedFactType = preferredFor.NestedFactType;
							if (nestedFactType != null && nestedFactType == constraintLink.Role.FactType)
							{
								// Adding a link to an internal constraint that is the preferred
								// identifier for an objectifying type is always valid
								break;
							}
						}
						// A preferred identifier on an internal uniqueness constraint requires
						// the constraint to have one role only. If we already have a preferred
						// identifier on this role, then we must have one already, so adding an
						// additional role is bad.
						constraint.PreferredIdentifierFor = null;
						// There are also problems if the role is added to the opposite single
						// role constraint, which must have a single-column internal uniqueness
						// constraint over it for both internal and external identifiers.
						UniquenessConstraint iuc = constraint as UniquenessConstraint;
						LinkedElementCollection<FactType> facts = iuc.FactTypeCollection;
						if (facts.Count == 1)
						{
							FactType fact = facts[0];
							LinkedElementCollection<RoleBase> roles = fact.RoleCollection;
							if (roles.Count == 2)
							{
								Role oldRole = roles[0].Role;
								if (oldRole == constraintLink.Role)
								{
									// Unlikely but possible (you'd need to insert instead of add)
									oldRole = roles[1].Role;
								}
								ObjectType oldRolePlayer;
								UniquenessConstraint preferredIdentifier;
								if ((null != (oldRolePlayer = oldRole.RolePlayer)) &&
									!oldRolePlayer.IsDeleted &&
									(null != (preferredIdentifier = oldRolePlayer.PreferredIdentifier)))
								{
									LinkedElementCollection<FactType> testFacts = preferredIdentifier.FactTypeCollection;
									int testFactsCount = testFacts.Count;
									for (int i = 0; i < testFactsCount; ++i)
									{
										// If this fact is involved in the external preferred identifier, then
										// the prerequisites for the pattern no longer hold
										if (fact == testFacts[i])
										{
											oldRolePlayer.PreferredIdentifier = null;
											break;
										}
									}
								}
							}
						}
						break;
					case ConstraintType.ExternalUniqueness:
						{
							// A preferred identifier on an external uniqueness constraint
							// can be extended to include a new role if the role is on a binary
							// fact opposite the object being identified. The opposite role must
							// have a single-column internal uniqueness constraint. Given that
							// we add the uniqueness constraint automatically when the preferred
							// identifier is added, it is also appropriate to add it here to preserve
							// the pattern.
							// Note that we'll go one step further here to keep the pattern. If the
							// opposite role player is not set then we'll set it automatically.
							ObjectType identifierFor = constraint.PreferredIdentifierFor;
							if (identifierFor != null)
							{
								bool clearIdentifier = true;
								Role nearRole = constraintLink.Role;
								FactType factType = nearRole.FactType;
								if (null != factType)
								{
									Objectification objectification = identifierFor.Objectification;
									RoleProxy proxy;
									FactType impliedFact;
									LinkedElementCollection<RoleBase> factRoles;
									if (null != (objectification = identifierFor.Objectification) &&
										null != (proxy = nearRole.Proxy) &&
										null != (impliedFact = proxy.FactType) &&
										objectification == impliedFact.ImpliedByObjectification)
									{
										clearIdentifier = false;
									}
									else if ((factRoles = factType.RoleCollection).Count == 2)
									{
										Role oppositeRole = factRoles[0].Role;
										if (oppositeRole == nearRole)
										{
											oppositeRole = factRoles[1].Role;
										}
										ObjectType oppositeRolePlayer = oppositeRole.RolePlayer;
										if (oppositeRolePlayer == null || oppositeRolePlayer == identifierFor)
										{
											bool haveSingleRoleInternalUniqueness = false;
											foreach (ConstraintRoleSequence roleSequence in oppositeRole.ConstraintRoleSequenceCollection)
											{
												if (roleSequence.Constraint.ConstraintType == ConstraintType.InternalUniqueness && roleSequence.RoleCollection.Count == 1)
												{
													haveSingleRoleInternalUniqueness = true;
													break;
												}
											}
											if (!haveSingleRoleInternalUniqueness)
											{
												UniquenessConstraint oppositeIuc = UniquenessConstraint.CreateInternalUniquenessConstraint(oppositeRole.Store);
												oppositeIuc.RoleCollection.Add(oppositeRole); // Automatically sets FactType
											}
											if (oppositeRolePlayer == null)
											{
												oppositeRole.RolePlayer = identifierFor;
											}
											clearIdentifier = false;
										}
									}
								}
								if (clearIdentifier)
								{
									// Could not maintain the pattern
									constraint.PreferredIdentifierFor = null;
								}
							}
						}
						break;
				}
			}
		}
		#endregion // TestRemovePreferredIdentifierConstraintRoleAddRule
		#region TestRemovePreferredIdentifierObjectificationAddRule
		/// <summary>
		/// AddRule: typeof(Objectification)
		/// A rule to determine if an internal constraint on one of its
		/// roles is the preferred identifier for a direct role player. This
		/// pattern is not allowed.
		/// </summary>
		private static void TestRemovePreferredIdentifierObjectificationAddRule(ElementAddedEventArgs e)
		{
			TestRemovePreferredIdentifiersForObjectificationAdded((e.ModelElement as Objectification).NestedFactType);
		}
		/// <summary>
		/// Rule helper method
		/// </summary>
		private static void TestRemovePreferredIdentifiersForObjectificationAdded(FactType factType)
		{
			LinkedElementCollection<RoleBase> roles = factType.RoleCollection;
			int roleCount = roles.Count;
			for (int i = 0; i < roleCount; ++i)
			{
				// Implied facts cannot be objectified, we will never see
				// role proxies here, so the exception cast is fine.
				Role role = (Role)roles[i];
				ObjectType rolePlayer;
				UniquenessConstraint pid;
				LinkedElementCollection<FactType> facts;
				if (null != (rolePlayer = role.RolePlayer) &&
					null != (pid = rolePlayer.PreferredIdentifier) &&
					1 == (facts = pid.FactTypeCollection).Count &&
					facts[0] == factType)
				{
					rolePlayer.PreferredIdentifier = null;
				}
			}
		}
		#endregion // TestRemovePreferredIdentifierObjectificationAddRule
		#region TestRemovePreferredIdentifierObjectificationRolePlayerChangeRule
		/// <summary>
		/// RolePlayerChangeRule: typeof(Objectification)
		/// </summary>
		private static void TestRemovePreferredIdentifierObjectificationRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			Objectification link = e.ElementLink as Objectification;
			if (link.IsDeleted)
			{
				return;
			}
			Guid changedRoleGuid = e.DomainRole.Id;
			if (changedRoleGuid == Objectification.NestedFactTypeDomainRoleId)
			{
				TestRemovePreferredIdentifiersForObjectificationAdded(link.NestedFactType);
			}
		}
		#endregion // TestRemovePreferredIdentifierObjectificationRolePlayerChangeRule
		#region PreferredIdentifierAddedRule
		/// <summary>
		/// AddRule: typeof(EntityTypeHasPreferredIdentifier)
		/// Verify that all preconditions hold for adding a primary
		/// identifier and extend modifiable conditions as needed.
		/// </summary>
		private static void PreferredIdentifierAddedRule(ElementAddedEventArgs e)
		{
			ProcessPreferredIdentifierAdded(e.ModelElement as EntityTypeHasPreferredIdentifier);
		}
		/// <summary>
		/// Check preconditions on an internal or external uniqueness constraint.
		/// </summary>
		private static void ProcessPreferredIdentifierAdded(EntityTypeHasPreferredIdentifier link)
		{
			// Enforce that a preferred identifier is set only for unobjectified
			// entity types. The other parts of this (don't allow this to be set
			// for object types with preferred identifiers) is enforced in
			// ObjectType.CheckForIncompatibleRelationshipRule
			ObjectType entityType = link.PreferredIdentifierFor;
			if (entityType.IsValueTypeCheckDeleting)
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionEnforcePreferredIdentifierForEntityType);
			}

			IConstraint constraint = link.PreferredIdentifier as IConstraint;
			switch (constraint.ConstraintType)
			{
				case ConstraintType.InternalUniqueness:
					{
						UniquenessConstraint iuc = constraint as UniquenessConstraint;
						iuc.TestAllowPreferred(entityType, true);

						// TestAllowPreferred verifies role player types, fact arities, and that
						// no constraints need to be deleted to make this happen. Additional
						// constraints that are automatically added all happen on the opposite
						// role, so find it, add constraints as needed, and then let this
						// pass through to finish creating the preferred identifier link.
						Role role = iuc.RoleCollection[0];

						// If we're adding a constraint pattern to the objectifying type from
						// an internal constraint on its nested type, then the required opposite
						// role pattern is already enforced by the Objectification relationship.
						// We do not need to enforce it here.
						if (role.Proxy == null || role.FactType != entityType.NestedFactType)
						{
							Role oppositeRole = null;
							FactType factType = role.FactType;
							foreach (RoleBase roleBase in factType.RoleCollection)
							{
								Role factRole = roleBase.Role;
								if (role != factRole)
								{
									oppositeRole = factRole;
									break;
								}
							}
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
								UniquenessConstraint oppositeIuc = UniquenessConstraint.CreateInternalUniquenessConstraint(store);
								oppositeIuc.RoleCollection.Add(oppositeRole); // Automatically sets FactType
							}
							oppositeRole.IsMandatory = true; // Make sure it is mandatory
						}
						break;
					}
				case ConstraintType.ExternalUniqueness:
					{
						UniquenessConstraint euc = constraint as UniquenessConstraint;
						euc.TestAllowPreferred(entityType, true);
						Objectification objectification = entityType.Objectification;

						// TestAllowPreferred verifies role player types and fact arities of the
						// associated fact types and that no constraints need to be deleted
						// to make this happen. Additional constraints that are automatically
						// added all happen on the opposite role, so find it, add constraints as needed,
						// and then let this pass through to finish creating the preferred identifier link.

						// Note that we cannot automatically add mandatory constraints as we did
						// with the internal uniqueness cases (the result is ambiguous), and we do
						// not enforce constraints on this side of the fact. The other cases
						// cases are handled as validation errors.
						LinkedElementCollection<Role> roles = euc.RoleCollection;
						int roleCount = roles.Count;
						Store store = euc.Store;
						for (int i = 0; i < roleCount; ++i)
						{
							Role role = roles[i];
							RoleProxy proxyRole;
							FactType impliedFactType;

							if (null != objectification &&
								null != (proxyRole = role.Proxy) &&
								null != (impliedFactType = proxyRole.FactType) &&
								impliedFactType.ImpliedByObjectification == objectification)
							{
								// The opposite role pattern is enforced by the objectification pattern
								continue;
							}

							Role oppositeRole = null;
							FactType factType = role.FactType;
							foreach (RoleBase roleBase in factType.RoleCollection)
							{
								Role factRole = roleBase.Role;
								if (role != factRole)
								{
									oppositeRole = factRole;
									break;
								}
							}
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
								UniquenessConstraint oppositeIuc = UniquenessConstraint.CreateInternalUniquenessConstraint(store);
								oppositeIuc.RoleCollection.Add(oppositeRole); // Automatically sets FactType
							}
						}
						break;
					}
			}
		}
		#endregion // PreferredIdentifierAddedRule
		#region PreferredIdentifierRolePlayerChangeRule
		/// <summary>
		/// RolePlayerChangeRule: typeof(EntityTypeHasPreferredIdentifier)
		/// Verify that all preconditions hold for adding a primary
		/// identifier and extend modifiable conditions as needed.
		/// Defers to <see cref="ProcessPreferredIdentifierAdded"/>.
		/// </summary>
		private static void PreferredIdentifierRolePlayerChangeRule(RolePlayerChangedEventArgs e)
		{
			ProcessPreferredIdentifierAdded(e.ElementLink as EntityTypeHasPreferredIdentifier);
		}
		#endregion // PreferredIdentifierRolePlayerChangeRule
		#region ModalityChangeRule
		/// <summary>
		/// ChangeRule: typeof(SetConstraint)
		/// Modify preferred identifier status for modality changes
		/// </summary>
		private static void ModalityChangeRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == SetConstraint.ModalityDomainPropertyId)
			{
				SetConstraint setConstraint = e.ModelElement as SetConstraint;
				EntityTypeHasPreferredIdentifier identifierLink;
				bool testRemoveOpposite = false;
				bool testMandatoryRequired = false;
				bool notAlethic = setConstraint.Modality != ConstraintModality.Alethic;
				switch ((setConstraint as IConstraint).ConstraintType)
				{
					case ConstraintType.InternalUniqueness:
						testRemoveOpposite = notAlethic;
						goto case ConstraintType.ExternalUniqueness;
					case ConstraintType.ExternalUniqueness:
						if (notAlethic)
						{
							// Preferred identifiers must be alethic
							identifierLink = EntityTypeHasPreferredIdentifier.GetLinkToPreferredIdentifierFor(setConstraint as UniquenessConstraint);
							if (identifierLink != null)
							{
								testRemoveOpposite = false;
								identifierLink.Delete();
							}
						}
						break;
					case ConstraintType.SimpleMandatory:
						testRemoveOpposite = notAlethic;
						testMandatoryRequired = true;
						break;
					case ConstraintType.DisjunctiveMandatory:
						testMandatoryRequired = true;
						break;
				}
				if (testRemoveOpposite)
				{
					// Remove preferred identifiers for modality changes on
					// constraints required by the preferred identifier pattern.
					LinkedElementCollection<Role> roles;
					ObjectType rolePlayer;
					if ((roles = setConstraint.RoleCollection).Count == 1 &&
						null != (rolePlayer = roles[0].RolePlayer) &&
						null != (identifierLink = EntityTypeHasPreferredIdentifier.GetLinkToPreferredIdentifier(rolePlayer)))
					{
						// Note that this will also call ObjectType.ValidateMandatoryRolesForPreferredIdentifier
						testMandatoryRequired = false; // TestRemovePreferredIdentifier will do the same test
						identifierLink.TestRemovePreferredIdentifier();
					}
				}
				if (testMandatoryRequired)
				{
					LinkedElementCollection<Role> roles = setConstraint.RoleCollection;
					int roleCount = roles.Count;
					for (int i = 0; i < roleCount; ++i)
					{
						Role role = roles[i];
						ObjectType objectType;
						UniquenessConstraint pid;
						if (null != (objectType = role.RolePlayer) &&
							null != (pid = objectType.PreferredIdentifier) &&
							!pid.IsInternal &&
							pid.FactTypeCollection.Contains(role.FactType))
						{
							objectType.ValidateMandatoryRolesForPreferredIdentifier();
						}
					}
				}
			}
		}
		#endregion // ModalityChangeRule
	}
	#endregion // EntityTypeHasPreferredIdentifier pattern enforcement
	#region UniquenessConstraint class
	public partial class UniquenessConstraint : IModelErrorOwner, IHasIndirectModelErrorOwner
	{
		#region CustomStorage handlers
		private bool GetIsPreferredValue()
		{
			return PreferredIdentifierFor != null;
		}
		private void SetIsPreferredValue(bool newValue)
		{
			// Handled by UniquenessConstraintChangeRule
		}
		#endregion // CustomStorage handlers
		#region Customize property display
		/// <summary>
		/// Test to see if this constraint can be turned
		/// into a preferred uniqueness constraint
		/// </summary>
		/// <param name="forType">If set, verify that the constraint
		/// can be the preferred identifier for this type. Can be null.</param>
		/// <param name="throwIfFalse">If true, throw instead of returning false</param>
		/// <returns>true if the test succeeds</returns>
		public bool TestAllowPreferred(ObjectType forType, bool throwIfFalse)
		{
			if ((forType != null || !IsPreferred) &&
				Modality == ConstraintModality.Alethic)
			{
				// To be considered for the preferred reference
				// mode on an object, the following must hold:
				// 1) The constraint must have at least one role (Note that there will be a model error for exactly one)
				// 2a) The constraint is attached to one fact type that is objectified by forType (2b and 3-5 are implied at this point).
				//     Note that a constraint that is internal to an objectified fact cannot be a preferred identifier for
				//     role players on that fact.
				// 2b) Or the constraint roles must all come from distinct facts (an internal uniqueness constraint role,
				//    regardless of whether it is primary or not, must not be attached to a role with a single-role internal
				//    uniqueness constraint on it. However, the opposite role must have this condition. Therefore, two
				//    roles from a preferred constraint cannot share the same binary fact).
				// 3) Each fact must be binary
				// 4) The opposite role player for each fact must be set to the same object
				// 5) The opposite role player must be an entity type
				// 6) The opposite role must not be an objectified unary role
				// The other conditions (at least one opposite role is mandatory and all
				// opposite roles have a single-role uniqueness constraint) will either
				// be enforced (single-role uniqueness) in the rule that makes  the change
				// end up as model validation errors (opposite role must be mandatory).
				// Note that there is no requirement on the type of the object attached
				// to the preferred constraint roles. If the primary object is created for
				// a RefMode object type, then a ValueType is required, but this is
				// not a requirement for all role players on preferred identifier constraints.
				LinkedElementCollection<Role> constraintRoles = RoleCollection;
				int constraintRoleCount = constraintRoles.Count;
				LinkedElementCollection<FactType> constraintFacts;
				int constraintFactCount;
				bool objectified = false;
				if (constraintRoleCount != 0 &&
					(!(objectified = (1 == (constraintFactCount = (constraintFacts = FactTypeCollection).Count) &&
					null != forType &&
					constraintFacts[0] == forType.NestedFactType)) ||
					(constraintFactCount == constraintRoleCount))) // Condition 1 and 2 (bail if 2a
				{
					int constraintRoleIndex = 0;
					ObjectType prevRolePlayer = null;
					ObjectType prevImpliedRolePlayer = null;
					bool havePreviousRolePlayer = false;
					for (; constraintRoleIndex < constraintRoleCount; ++constraintRoleIndex)
					{
						bool patternOK = false;
						Role role = constraintRoles[constraintRoleIndex];
						RoleProxy proxy = role.Proxy;
						FactType factType = role.FactType;
						LinkedElementCollection<RoleBase> factRoles = null;
						bool directBinary;
						patternOK = false;
						if (!(factType is SubtypeFact) && 
							((directBinary = (!objectified && (factRoles = factType.RoleCollection).Count == 2)) ||
							proxy != null)) // Condition 3 (RoleProxy can only be attached to a binary fact)
						{
							ObjectType rolePlayer = null;
							ObjectType impliedRolePlayer = null;
							if (directBinary)
							{
								Role oppositeRole = factRoles[0].Role;
								if (oppositeRole == role)
								{
									oppositeRole = factRoles[1].Role;
								}
								if (oppositeRole is ObjectifiedUnaryRole) // Condition 6, Can't be opposite an objectified unary role
								{
									break;
								}
								rolePlayer = oppositeRole.RolePlayer;
							}
							if (proxy != null)
							{
								impliedRolePlayer = factType.NestingType;
							}

							if (rolePlayer != null || impliedRolePlayer != null)
							{
								if (havePreviousRolePlayer)
								{
									if (prevRolePlayer != null &&
										(prevRolePlayer == rolePlayer ||
										prevRolePlayer == impliedRolePlayer)) // Condition 4
									{
										patternOK = true;
										prevImpliedRolePlayer = null;
									}
									if (!patternOK &&
										prevImpliedRolePlayer != null &&
										(prevImpliedRolePlayer == rolePlayer ||
										prevImpliedRolePlayer == impliedRolePlayer)) // Condition 4
									{
										patternOK = true;
										prevRolePlayer = prevImpliedRolePlayer;
										prevImpliedRolePlayer = null;
									}
								}
								else
								{
									if (rolePlayer != null &&
										(forType == null || forType == rolePlayer) &&
										!rolePlayer.IsValueTypeCheckDeleting) // Condition 5
									{
										patternOK = true;
										prevRolePlayer = rolePlayer;
									}
									if (impliedRolePlayer != null &&
										(forType == null || forType == impliedRolePlayer))
									{
										Debug.Assert(!impliedRolePlayer.IsValueTypeCheckDeleting, "Objectifying types cannot be value types"); // Condition 5
										patternOK = true;
										prevImpliedRolePlayer = impliedRolePlayer;
									}
								}
							}
						}
						if (!patternOK)
						{
							break;
						}
					}
					if (constraintRoleIndex == constraintRoleCount)
					{
						return true;
					}
				}
				else if (objectified)
				{
					return true;
				}
			}
			if (throwIfFalse)
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionInvalidPreferredIdentifierPreConditions);
			}
			return false;
		}
		#endregion // Customize property display
		#region UniquenessConstraint Specific
		/// <summary>
		/// Returns true if this is a single role internal uniqueness constraint.
		/// If this is true, then the constraint might match the reference mode
		/// pattern between the objectifying EntityType and one of its role
		/// players. This requires special handling.
		/// </summary>
		public bool IsObjectifiedSingleRolePreferredIdentifier
		{
			get
			{
				ObjectType preferredFor = PreferredIdentifierFor;
				Objectification objectification;
				LinkedElementCollection<Role> constraintRoles;
				RoleProxy proxy;
				FactType impliedFact;
				Role role;
				return preferredFor != null &&
					null != (objectification = preferredFor.Objectification) &&
					1 == (constraintRoles = RoleCollection).Count &&
					((role = constraintRoles[0]) is ObjectifiedUnaryRole ||
					(null != (proxy = role.Proxy) &&
					null != (impliedFact = proxy.FactType) &&
					impliedFact.ImpliedByObjectification == objectification));
			}
		}
		/// <summary>
		/// Returns true if this uniqueness constraint is internal
		/// to an objectified <see cref="FactType"/> that is objectified
		/// by the <see cref="ObjectType"/> the constraint is a preferred
		/// identifier, or if the preferred identifier is part of a unary
		/// objectification FactType.
		/// </summary>
		public bool IsObjectifiedPreferredIdentifier
		{
			get
			{
				ObjectType preferredFor;
				FactType objectifiedFactType;
				LinkedElementCollection<FactType> pidFactTypes;
				Role unaryRole;
				ObjectifiedUnaryRole objectifiedUnaryRole;
				FactType identifierFactType;
				return IsInternal &&
					null != (preferredFor = PreferredIdentifierFor) &&
					null != (objectifiedFactType = preferredFor.NestedFactType) &&
					1 == (pidFactTypes = FactTypeCollection).Count &&
					((identifierFactType = pidFactTypes[0]) == objectifiedFactType ||
					(null != (unaryRole = objectifiedFactType.UnaryRole) &&
					null != (objectifiedUnaryRole = unaryRole.ObjectifiedUnaryRole) &&
					identifierFactType == objectifiedUnaryRole.FactType));
			}
		}
		#endregion // UniquenessConstraint Specific
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Returns the error associated with the constraint.
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			if (filter == 0)
			{
				filter = (ModelErrorUses)(-1);
			}
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
			{
				NMinusOneError nMinusOneError = NMinusOneError;
				if (nMinusOneError != null)
				{
					yield return nMinusOneError;
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
			base.ValidateErrors(notifyAdded);
			// Calls added here need corresponding delayed calls in DelayValidateErrors
			VerifyNMinusOneForRule(notifyAdded);
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
			base.DelayValidateErrors();
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateNMinusOneError);
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner Implementation
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			if (PreferredIdentifierFor != null)
			{
				// Creating a static readonly guid array is causing static field initialization
				// ordering issues with the partial classes. Defer initialization.
				Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
				if (linkRoles == null)
				{
					myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { EntityTypeHasPreferredIdentifier.PreferredIdentifierDomainRoleId };
				}
				return linkRoles;
			}
			return null;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
		#region NMinusOneError Validation
		/// <summary>
		/// Validator callback for NMinusOneError
		/// </summary>
		private static void DelayValidateNMinusOneError(ModelElement element)
		{
			(element as UniquenessConstraint).VerifyNMinusOneForRule(null);
		}
		/// <summary>
		/// Add, remove, and otherwise validate the current NMinusOne errors
		/// </summary>
		/// <param name="notifyAdded">If not null, this is being called during
		/// load when rules are not in place. Any elements that are added
		/// must be notified back to the caller.</param>
		private void VerifyNMinusOneForRule(INotifyElementAdded notifyAdded)
		{
			if (!IsDeleted)
			{
				LinkedElementCollection<FactType> facts;
				NMinusOneError error = NMinusOneError;
				FactType factType;
				FactTypeDerivationRule derivationRule;
				if (IsInternal &&
					Modality == ConstraintModality.Alethic &&
					1 == (facts = FactTypeCollection).Count &&
					(null == (derivationRule = (factType = facts[0]).DerivationRule) ||
					derivationRule.ExternalDerivation ||
					derivationRule.DerivationCompleteness != DerivationCompleteness.FullyDerived) &&
					RoleCollection.Count < factType.RoleCollection.Count - 1)
				{
					//Adding the Error to the model
					if (error == null)
					{
						error = new NMinusOneError(Store);
						error.Constraint = this;
						error.Model = factType.Model;
						error.GenerateErrorText();
						if (notifyAdded != null)
						{
							notifyAdded.ElementAdded(error, true);
						}
					}
					else
					{
						//Error is already present, but the number of facts in the sequence could have changed, so we
						//need to regenerate the error text so the correct number of roles is present.
						error.GenerateErrorText();
					}
				}
				else if (error != null)
				{
					//Removing error
					error.Delete();
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(FactSetConstraint)
		/// Only validates NMinusOneError
		/// Checks when Internal constraint is added
		/// </summary>
		private static void NMinusOneConstraintAddRule(ElementAddedEventArgs e)
		{
			SetConstraint constraint = (e.ModelElement as FactSetConstraint).SetConstraint;
			IModelErrorOwner errorOwner;
			if (constraint.Constraint.ConstraintIsInternal &&
				null != (errorOwner = constraint as IModelErrorOwner))
			{
				errorOwner.DelayValidateErrors();
			}
		}
		/// <summary>
		/// AddRule: typeof(ConstraintRoleSequenceHasRole)
		/// Only validates NMinusOneError
		/// </summary>
		private static void NMinusOneConstraintRoleAddRule(ElementAddedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			UniquenessConstraint constraint = link.ConstraintRoleSequence as UniquenessConstraint;
			if (constraint != null && constraint.IsInternal && constraint.Modality == ConstraintModality.Alethic)
			{
				FrameworkDomainModel.DelayValidateElement(constraint, DelayValidateNMinusOneError);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(ConstraintRoleSequenceHasRole)
		/// Only validates NMinusOneError
		/// </summary>
		private static void NMinusOneConstraintRoleDeleteRule(ElementDeletedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			UniquenessConstraint constraint = link.ConstraintRoleSequence as UniquenessConstraint;
			if (constraint != null && !constraint.IsDeleted && constraint.IsInternal && constraint.Modality == ConstraintModality.Alethic)
			{
				FrameworkDomainModel.DelayValidateElement(constraint, DelayValidateNMinusOneError);
			}
		}
		private static void DelayValidateInternalUniquenessConstraintsForNMinusOneRule(FactType factType)
		{
			if (factType != null && !factType.IsDeleted)
			{
				foreach (UniquenessConstraint constraint in factType.GetInternalConstraints<UniquenessConstraint>())
				{
					if (constraint.Modality == ConstraintModality.Alethic)
					{
						FrameworkDomainModel.DelayValidateElement(constraint, DelayValidateNMinusOneError);
					}
				}
			}
		}
		/// <summary>
		/// AddRule: typeof(FactTypeHasRole)
		/// Only validates NMinusOneError
		/// Used for Adding roles to the role sequence check
		/// </summary>
		private static void NMinusOneFactTypeRoleAddRule(ElementAddedEventArgs e)
		{
			DelayValidateInternalUniquenessConstraintsForNMinusOneRule(((FactTypeHasRole)e.ModelElement).FactType);
		}
		/// <summary>
		/// DeleteRule: typeof(FactTypeHasRole)
		/// Only validates NMinusOneError
		/// Used for Removing roles to the role sequence check
		/// </summary>
		private static void NMinusOneFactTypeRoleDeleteRule(ElementDeletedEventArgs e)
		{
			DelayValidateInternalUniquenessConstraintsForNMinusOneRule(((FactTypeHasRole)e.ModelElement).FactType);
		}
		/// <summary>
		/// AddRule: typeof(FactTypeHasDerivationRule)
		/// </summary>
		private static void NMinusOneFactTypeDerivationRuleAddedRule(ElementAddedEventArgs e)
		{
			FactTypeHasDerivationRule link = (FactTypeHasDerivationRule)e.ModelElement;
			FactTypeDerivationRule derivationRule = link.DerivationRule;
			if (derivationRule.DerivationCompleteness == DerivationCompleteness.FullyDerived &&
				!derivationRule.ExternalDerivation)
			{
				DelayValidateInternalUniquenessConstraintsForNMinusOneRule(link.FactType);
			}
		}
		/// <summary>
		/// ChangeRule: typeof(FactTypeDerivationRule)
		/// </summary>
		private static void NMinusOneFactTypeDerivationRuleChangedRule(ElementPropertyChangedEventArgs e)
		{
			FactType factType;
			Guid domainPropertyId = e.DomainProperty.Id;
			if ((domainPropertyId == FactTypeDerivationRule.DerivationCompletenessDomainPropertyId ||
				domainPropertyId == FactTypeDerivationRule.ExternalDerivationDomainPropertyId) &&
				null != (factType = ((FactTypeDerivationRule)e.ModelElement).FactType))
			{
				DelayValidateInternalUniquenessConstraintsForNMinusOneRule(factType);
			}
		}
		/// <summary>
		/// DeleteRule: typeof(FactTypeHasDerivationRule)
		/// </summary>
		private static void NMinusOneFactTypeDerivationRuleDeletedRule(ElementDeletedEventArgs e)
		{
			FactTypeHasDerivationRule link = (FactTypeHasDerivationRule)e.ModelElement;
			FactTypeDerivationRule derivationRule = link.DerivationRule;
			if (derivationRule.DerivationCompleteness == DerivationCompleteness.FullyDerived &&
				!derivationRule.ExternalDerivation)
			{
				DelayValidateInternalUniquenessConstraintsForNMinusOneRule(link.FactType);
			}
		}
		#endregion // NMinusOneError Validation
		#region Internal constraint handling
		private static PropertyAssignment[] myInitialInternalAttributes;
		/// <summary>
		/// Create a UniquenessConstraint with an initial 'IsInternal' property set to true
		/// </summary>
		/// <param name="store">The containing store</param>
		/// <returns>The newly created constraint</returns>
		public static UniquenessConstraint CreateInternalUniquenessConstraint(Store store)
		{
			PropertyAssignment[] attributes = myInitialInternalAttributes;
			if (attributes == null)
			{
				attributes = myInitialInternalAttributes =
					new PropertyAssignment[] { new PropertyAssignment(IsInternalDomainPropertyId, true) };
			}
			return new UniquenessConstraint(store, attributes);
		}
		/// <summary>
		/// Create a UniquenessConstraint with an initial 'IsInternal' property set to true
		/// and attach it to the given factType
		/// </summary>
		/// <param name="factType">The parent factType for the new constraint</param>
		/// <returns>The newly created constraint</returns>
		public static UniquenessConstraint CreateInternalUniquenessConstraint(FactType factType)
		{
			UniquenessConstraint uc = CreateInternalUniquenessConstraint(factType.Store);
			uc.FactTypeCollection.Add(factType);
			return uc;
		}
		#endregion // Internal constraint handling
		#region UniquenessConstraintChangeRule
		/// <summary>
		/// ChangeRule: typeof(UniquenessConstraint)
		/// Handle property changes for <see cref="UniquenessConstraint"/>
		/// </summary>
		private static void UniquenessConstraintChangeRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeId = e.DomainProperty.Id;
			if (attributeId == UniquenessConstraint.IsPreferredDomainPropertyId)
			{
				UniquenessConstraint constraint = e.ModelElement as UniquenessConstraint;
				if ((bool)e.NewValue)
				{
					// The preconditions for all of this are verified in the UI, and
					// are verified again in the PreferredIdentifierAddedRule. If any
					// of this throws it is because the preconditions are violated,
					// but this will be such a rare condition that I don't go
					// out of my way to validate it. Calling code can always use
					// the TestAllowPreferred method to get a cleaner exception.
					// We only use TestAllowPreferred here for role proxy cases
					// to determine whether we should be preferred for the objectified
					// type or an opposite role player.
					Role constraintRole = constraint.RoleCollection[0];
					RoleBase role = constraintRole.Proxy;
					ObjectType objectifyingType;
					ObjectType targetRolePlayer = null;
					if (role == null)
					{
						role = constraintRole;
					}
					else if ((null != (objectifyingType = constraintRole.FactType.NestingType)) &&
						constraint.TestAllowPreferred(objectifyingType, false))
					{
						targetRolePlayer = objectifyingType;
					}
					else
					{
						role = constraintRole;
					}
					if (targetRolePlayer == null)
					{
						LinkedElementCollection<RoleBase> factRoles = role.FactType.RoleCollection;
						int roleCount = factRoles.Count;
						for (int i = 0; i < roleCount; ++i)
						{
							RoleBase testRole = factRoles[i];
							if (role != testRole)
							{
								targetRolePlayer = ((Role)testRole).RolePlayer;
								break;
							}
						}
					}

					// Let the PreferredIdentifierAddedRule do the bulk of the work
					constraint.PreferredIdentifierFor = targetRolePlayer;
				}
				else
				{
					constraint.PreferredIdentifierFor = null;
				}
			}
			else if (attributeId == UniquenessConstraint.ModalityDomainPropertyId)
			{
				FrameworkDomainModel.DelayValidateElement(e.ModelElement as UniquenessConstraint, DelayValidateNMinusOneError);
			}
			else if (attributeId == UniquenessConstraint.IsInternalDomainPropertyId)
			{
				// UNDONE: Support toggling IsInternal property after the object has been created
				throw new InvalidOperationException("UniquenessConstraint.IsInternal cannot be changed");
			}
		}
		#endregion // UniquenessConstraintChangeRule
	}
	#endregion // UniquenessConstraint class
	#region MandatoryConstraint class
	public partial class MandatoryConstraint : IModelErrorOwner, IRedirectVerbalization, IHasIndirectModelErrorOwner
	{
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Implements IModelErrorOwner.GetErrorCollection
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			if (filter == 0)
			{
				filter = (ModelErrorUses)(-1);
			}
			foreach (ModelErrorUsage baseError in base.GetErrorCollection(filter))
			{
				yield return baseError;
			}
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
			{
				NotWellModeledSubsetAndMandatoryError notWellModeled = NotWellModeledSubsetAndMandatoryError;
				if (notWellModeled != null)
				{
					yield return notWellModeled;
				}
				ExclusionContradictsMandatoryError exclusionConstradictsMandatory = ExclusionContradictsMandatoryError;
				if (exclusionConstradictsMandatory != null)
				{
					yield return exclusionConstradictsMandatory;
				}
				foreach (PopulationMandatoryError populationMandatory in PopulationMandatoryErrorCollection)
				{
					yield return populationMandatory;
				}
			}
		}
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			return GetErrorCollection(filter);
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
			base.DelayValidateErrors();
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion // IModelErrorOwner Implementation
		#region IHasIndirectModelErrorOwner Implementation
		private static Guid[] myIndirectModelErrorOwnerLinkRoles;
		/// <summary>
		/// Implements <see cref="IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles"/>
		/// </summary>
		protected Guid[] GetIndirectModelErrorOwnerLinkRoles()
		{
			// Note that the roles are only indirect error owners when
			// the PopulationMandatoryErrorCollection is not empty. However,
			// we need to navigate this link in deleted scenarios as well
			// when there is no way to check this population.
			// Creating a static readonly guid array is causing static field initialization
			// ordering issues with the partial classes. Defer initialization.
			Guid[] linkRoles = myIndirectModelErrorOwnerLinkRoles;
			if (linkRoles == null)
			{
				myIndirectModelErrorOwnerLinkRoles = linkRoles = new Guid[] { ConstraintRoleSequenceHasRole.ConstraintRoleSequenceDomainRoleId };
			}
			return linkRoles;
		}
		Guid[] IHasIndirectModelErrorOwner.GetIndirectModelErrorOwnerLinkRoles()
		{
			return GetIndirectModelErrorOwnerLinkRoles();
		}
		#endregion // IHasIndirectModelErrorOwner Implementation
		#region Simple mandatory constraint handling
		private static PropertyAssignment[] myInitialInternalAttributes;
		/// <summary>
		/// Create a MandatoryConstraint with an initial 'IsSimple' property set set true
		/// </summary>
		/// <param name="store">The containing store</param>
		/// <returns>The newly created constraint</returns>
		public static MandatoryConstraint CreateSimpleMandatoryConstraint(Store store)
		{
			PropertyAssignment[] attributes = myInitialInternalAttributes;
			if (attributes == null)
			{
				attributes = myInitialInternalAttributes = new PropertyAssignment[] { new PropertyAssignment(IsSimpleDomainPropertyId, true) };
			}
			return new MandatoryConstraint(store, attributes);
		}
		/// <summary>
		/// Create a MandatoryConstraint with an initial 'IsSimple' property set to true
		/// and attach it to the given role
		/// </summary>
		/// <param name="role">The role for the new simple mandatory constraint</param>
		/// <returns>The newly created constraint</returns>
		public static MandatoryConstraint CreateSimpleMandatoryConstraint(Role role)
		{
			MandatoryConstraint mc = CreateSimpleMandatoryConstraint(role.Store);
			mc.RoleCollection.Add(role);
			return mc;
		}
		#region MandatoryConstraintChangeRule
		/// <summary>
		/// ChangeRule: typeof(MandatoryConstraint)
		/// Block changes IsSimple, IsImplied, and Modality (when IsImplied is true) properties
		/// </summary>
		private static void MandatoryConstraintChangeRule(ElementPropertyChangedEventArgs e)
		{
			// UNDONE: Localize error messages in this routine
			Guid attributeId = e.DomainProperty.Id;
			if (attributeId == MandatoryConstraint.IsSimpleDomainPropertyId)
			{
				// UNDONE: Support toggling IsSimple property after the object has been created.
				throw new InvalidOperationException("MandatoryConstraint.IsSimple cannot be changed.");
			}
			else if (attributeId == MandatoryConstraint.IsImpliedDomainPropertyId)
			{
				throw new InvalidOperationException("MandatoryConstraint.IsImplied cannot be changed.");
			}
			else if (attributeId == MandatoryConstraint.ModalityDomainPropertyId)
			{
				if (((MandatoryConstraint)e.ModelElement).IsImplied)
				{
					throw new InvalidOperationException("MandatoryConstraint.Modality cannot be changed is MandatoryConstraint.IsImplied is true.");
				}
			}
		}
		#endregion // MandatoryConstraintChangeRule
		#endregion // Simple mandatory constraint handling
		#region Implied mandatory constraint handling
		private static PropertyAssignment[] myInitialImpliedAttributes;
		/// <summary>
		/// Create a MandatoryConstraint with an initial 'IsImplied' property set set true for the specified object type
		/// </summary>
		/// <param name="objectType">The <see cref="ObjectType"/> to create an implied mandatory constraint for</param>
		/// <returns>The newly created constraint</returns>
		public static MandatoryConstraint CreateImpliedMandatoryConstraint(ObjectType objectType)
		{
			PropertyAssignment[] attributes = myInitialImpliedAttributes;
			Store store = objectType.Store;
			if (attributes == null)
			{
				attributes = myInitialImpliedAttributes = new PropertyAssignment[] { new PropertyAssignment(IsImpliedDomainPropertyId, true) };
			}
			MandatoryConstraint retVal = new MandatoryConstraint(store, attributes);
			retVal.Model = objectType.Model;
			new ObjectTypeImpliesMandatoryConstraint(objectType, retVal);
			return retVal;
		}
		#endregion // Implied mandatory constraint handling
		#region IRedirectVerbalization Implementation
		/// <summary>
		/// Redirect exclusive or verbalization to the exclusion constraint
		/// </summary>
		IVerbalize IRedirectVerbalization.SurrogateVerbalizer
		{
			get
			{
				ExclusionConstraint coupledExclusionConstraint = ExclusiveOrExclusionConstraint;
				return (coupledExclusionConstraint != null) ? coupledExclusionConstraint : null;
			}
		}
		#endregion // IRedirectVerbalization Implementation
	}
	#endregion // MandatoryConstraint class
	#region ExlusiveOrConstraintCoupler class
	public partial class ExclusiveOrConstraintCoupler
	{
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// verifies the pattern between coupled constraints.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new ExclusiveOrPatternFixupListener();
			}
		}
		/// <summary>
		/// Fixup listener implementation. Ensures compatibility between linked
		/// exclusion and mandatory constraints
		/// </summary>
		private sealed class ExclusiveOrPatternFixupListener : DeserializationFixupListener<ExclusiveOrConstraintCoupler>
		{
			/// <summary>
			/// ExclusiveOrPatternFixupListener constructor
			/// </summary>
			public ExclusiveOrPatternFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateImplicitStoredElements)
			{
			}
			/// <summary>
			/// Process ExclusiveOrConstraintCoupler elements
			/// </summary>
			/// <param name="element">An ExclusiveOrConstraintCoupler element</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(ExclusiveOrConstraintCoupler element, Store store, INotifyElementAdded notifyAdded)
			{
				if (!element.IsDeleted)
				{
					if (!element.SynchronizeCoupledRoles(false, false))
					{
						element.Delete();
					}
				}
			}
		}
		#endregion // Deserialization Fixup
		#region Pattern verification helper methods
		/// <summary>
		/// Determine if an ExclusiveOrConstraintCoupler can be successfully created
		/// between the two provided constraints.
		/// </summary>
		/// <param name="mandatoryConstraint">A candidate <see cref="MandatoryConstraint"/></param>
		/// <param name="exclusionConstraint">A candidate <see cref="ExclusionConstraint"/></param>
		/// <returns>true if the constraints can be coupled</returns>
		public static bool CanCoupleConstraints(MandatoryConstraint mandatoryConstraint, ExclusionConstraint exclusionConstraint)
		{
			if (mandatoryConstraint.Constraint.ConstraintType != ConstraintType.DisjunctiveMandatory ||
				mandatoryConstraint.ExclusiveOrExclusionConstraint != null ||
				exclusionConstraint.ExclusiveOrMandatoryConstraint != null)
			{
				return false;
			}
			LinkedElementCollection<Role> mandatoryRoles;
			LinkedElementCollection<SetComparisonConstraintRoleSequence> exclusionSequences;
			int sequenceCount;
			bool retVal = false;
			if (mandatoryConstraint.Modality == exclusionConstraint.Modality &&
				(mandatoryRoles = mandatoryConstraint.RoleCollection).Count == (sequenceCount = (exclusionSequences = exclusionConstraint.RoleSequenceCollection).Count))
			{
				retVal = true; // Prove otherwise
				IList<Role> testRoles = null; // A temporary array so we can null out roles that are already matched
				for (int i = 0; i < sequenceCount; ++i)
				{
					SetComparisonConstraintRoleSequence exclusionSequence = exclusionSequences[i];
					LinkedElementCollection<Role> exclusionRoles = exclusionSequence.RoleCollection;
					if (exclusionRoles.Count != 1)
					{
						retVal = false;
						break;
					}
					if (testRoles == null)
					{
						if (exclusionRoles[0] == mandatoryRoles[i])
						{
							// If the roles are already in the same order then
							// we don't need to work as hard
							continue;
						}
						else if (i < (sequenceCount - 1))
						{
							Role[] roles = new Role[sequenceCount];
							mandatoryRoles.CopyTo(roles, 0);
							for (int j = 0; j < i; ++j)
							{
								// These have already been matched
								roles[j] = null;
							}
							testRoles = roles;
						}
						else
						{
							retVal = false;
							break;
						}
					}
					int matchIndex = testRoles.IndexOf(exclusionRoles[0]);
					if (matchIndex >= 0)
					{
						testRoles[matchIndex] = null;
					}
					else
					{
						retVal = false;
						break;
					}
				}
			}
			return retVal;
		}
		/// <summary>
		/// Verify and synchronize the roles of the MandatoryConstraint with
		/// those of the ExclusionConstraint.
		/// </summary>
		/// <param name="throwOnFailure">true to raise an exception on failure</param>
		/// <param name="rulesEnabled">Rules are enabled. Make sure all enforcing rules are off.</param>
		/// <returns>true if the synchronization succeeded.</returns>
		private bool SynchronizeCoupledRoles(bool throwOnFailure, bool rulesEnabled)
		{
			MandatoryConstraint mandatoryConstraint = MandatoryConstraint;
			ExclusionConstraint exclusionConstraint = ExclusionConstraint;
			LinkedElementCollection<Role> mandatoryRoles;
			LinkedElementCollection<SetComparisonConstraintRoleSequence> exclusionSequences;
			int sequenceCount;
			bool invalidConfiguration = true;
			if (mandatoryConstraint.Modality == exclusionConstraint.Modality &&
				(mandatoryRoles = mandatoryConstraint.RoleCollection).Count == (sequenceCount = (exclusionSequences = exclusionConstraint.RoleSequenceCollection).Count))
			{
				invalidConfiguration = false; // Prove otherwise
				Type ruleType = null;
				try
				{
					int sequenceIndex = 0;
					// n is small here (more than 5 is rare), so I'm not too
					// concerned about the efficiency of the algorithm.
					while (sequenceIndex < sequenceCount)
					{
						SetComparisonConstraintRoleSequence exclusionSequence = exclusionSequences[sequenceIndex];
						LinkedElementCollection<Role> exclusionRoles = exclusionSequence.RoleCollection;
						if (exclusionRoles.Count != 1)
						{
							invalidConfiguration = true;
							break;
						}
						int mandatoryIndex = mandatoryRoles.IndexOf(exclusionRoles[0]);
						if (mandatoryIndex == sequenceIndex)
						{
							++sequenceIndex;
						}
						else if (mandatoryIndex < sequenceIndex)
						{
							// Duplicate role in the exclusion constraint, we've already
							// see this one.
							invalidConfiguration = true;
							break;
						}
						else
						{
							if (rulesEnabled && ruleType == null)
							{
								ruleType = typeof(RoleSequencePositionChangeRuleClass);
								Store.RuleManager.DisableRule(ruleType);
							}
							// Move the sequence. We'll reverify its position later.
							exclusionSequences.Move(sequenceIndex, mandatoryIndex);
						}
					}
				}
				finally
				{
					if (ruleType != null)
					{
						Store.RuleManager.EnableRule(ruleType);
					}
				}
			}
			if (invalidConfiguration && throwOnFailure)
			{
				throw new InvalidOperationException(ResourceStrings.ModelExceptionExclusiveOrConstraintCouplerInconsistentConstraints);
			}
			return !invalidConfiguration;
		}
		private static void ThrowDirectExclusionConstraintEditException()
		{
			throw new InvalidOperationException(ResourceStrings.ModelExceptionExclusiveOrConstraintCouplerDirectExclusionConstraintEdit);
		}
		#endregion // Pattern verification helper methods
		#region CouplerDeleteRule
		/// <summary>
		/// DeleteRule: typeof(ExclusiveOrConstraintCoupler), FireTime=LocalCommit, Priority=ORMCoreDomainModel.BeforeDelayValidateRulePriority;
		/// Give the ExclusiveOrConstraintCoupler bidirection propagate delete
		/// behavior, but only if one end is already deleted and the other is not
		/// </summary>
		private static void CouplerDeleteRule(ElementDeletedEventArgs e)
		{
			ExclusiveOrConstraintCoupler link = e.ModelElement as ExclusiveOrConstraintCoupler;
			MandatoryConstraint mandatory = link.MandatoryConstraint;
			ExclusionConstraint exclusion = link.ExclusionConstraint;
			if (mandatory.IsDeleted && !exclusion.IsDeleted)
			{
				exclusion.Delete();
			}
			else if (exclusion.IsDeleted && !mandatory.IsDeleted)
			{
				mandatory.Delete();
			}
		}
		#endregion // CouplerDeleteRule
		#region CouplerAddRule
		/// <summary>
		/// AddRule: typeof(ExclusiveOrConstraintCoupler)
		/// Verify that the exclusion constraint has role settings compatible
		/// with the mandatory constraint, reordering constraints as needed
		/// </summary>
		private static void CouplerAddRule(ElementAddedEventArgs e)
		{
			((ExclusiveOrConstraintCoupler)e.ModelElement).SynchronizeCoupledRoles(true, true);
		}
		#endregion // CouplerAddRule
		#region RoleAddRule
		/// <summary>
		/// AddRule: typeof(ConstraintRoleSequenceHasRole)
		/// Enforce that any role added to a MandatoryConstraint is also
		/// added to a coupled ExclusionConstraint.
		/// </summary>
		private static void RoleAddRule(ElementAddedEventArgs e)
		{
			ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
			ConstraintRoleSequence sequence = link.ConstraintRoleSequence;
			MandatoryConstraint mandatoryConstraint;
			ExclusionConstraint exclusionConstraint;
			SetComparisonConstraintRoleSequence comparisonConstraintSequence;
			if (null != (mandatoryConstraint = sequence as MandatoryConstraint))
			{
				if (null != (exclusionConstraint = mandatoryConstraint.ExclusiveOrExclusionConstraint))
				{
					// We need to add a new sequence with a single role in it to the corresponding exclusion constraint.
					// Add rules can fire due to either add or insert commands, so we need to find the
					// correct position in the sequence.
					Store store = link.Store;
					comparisonConstraintSequence = new SetComparisonConstraintRoleSequence(store, null);
					comparisonConstraintSequence.RoleCollection.Add(link.Role);
					ReadOnlyCollection<ConstraintRoleSequenceHasRole> orderedLinks = ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(sequence);
					int insertionIndex = orderedLinks.IndexOf(link);
					RuleManager ruleManager = store.RuleManager;
					ruleManager.DisableRule(typeof(RoleSequenceAddRuleClass));
					try
					{
						if (insertionIndex == (orderedLinks.Count - 1))
						{
							exclusionConstraint.RoleSequenceCollection.Add(comparisonConstraintSequence);
						}
						else
						{
							exclusionConstraint.RoleSequenceCollection.Insert(insertionIndex, comparisonConstraintSequence);
						}
					}
					finally
					{
						ruleManager.EnableRule(typeof(RoleSequenceAddRuleClass));
					}
				}
			}
			else if (null != (comparisonConstraintSequence = sequence as SetComparisonConstraintRoleSequence) &&
				null != (exclusionConstraint = comparisonConstraintSequence.ExternalConstraint as ExclusionConstraint) &&
				null != exclusionConstraint.ExclusiveOrMandatoryConstraint)
			{
				// Note that this will not fire from the code earlier in this routine because
				// the sequence is populated before it is added to the constraint, so we can
				// always throw here.
				ThrowDirectExclusionConstraintEditException();
			}
		}
		#endregion // RoleAddRule
		#region RolePositionChangeRule
		/// <summary>
		/// RolePlayerPositionChangeRule: typeof(ConstraintRoleSequenceHasRole)
		/// Enforce that any role moved in a MandatoryConstraint is also
		/// moved in a coupled ExclusionConstraint.
		/// </summary>
		private static void RolePositionChangeRule(RolePlayerOrderChangedEventArgs e)
		{
			// Note that we will never get a position change for a role in a coupled
			// exclusion constraint because each sequence has exactly one role
			MandatoryConstraint mandatoryConstraint;
			ExclusionConstraint exclusionConstraint;
			if (e.SourceDomainRole.Id == ConstraintRoleSequenceHasRole.ConstraintRoleSequenceDomainRoleId &&
				null != (mandatoryConstraint = e.SourceElement as MandatoryConstraint) &&
				null != (exclusionConstraint = mandatoryConstraint.ExclusiveOrExclusionConstraint))
			{
				RuleManager ruleManager = exclusionConstraint.Store.RuleManager;
				ruleManager.DisableRule(typeof(RoleSequencePositionChangeRuleClass));
				try
				{
					exclusionConstraint.RoleSequenceCollection.Move(e.OldOrdinal, e.NewOrdinal);
				}
				finally
				{
					ruleManager.EnableRule(typeof(RoleSequencePositionChangeRuleClass));
				}
			}
		}
		#endregion // RolePositionChangeRule
		#region RoleSequencePositionChangeRule
		/// <summary>
		/// RolePlayerPositionChangeRule: typeof(SetComparisonConstraintHasRoleSequence)
		/// Disallow direct edits to coupled exclusion constraints
		/// </summary>
		private static void RoleSequencePositionChangeRule(RolePlayerOrderChangedEventArgs e)
		{
			ExclusionConstraint exclusionConstraint;
			if (e.SourceDomainRole.Id == SetComparisonConstraintHasRoleSequence.ExternalConstraintDomainRoleId &&
				null != (exclusionConstraint = e.SourceElement as ExclusionConstraint) &&
				null != exclusionConstraint.ExclusiveOrMandatoryConstraint)
			{
				ThrowDirectExclusionConstraintEditException();
			}
		}
		#endregion // RoleSequencePositionChangeRule
		#region RoleSequenceAddRule
		/// <summary>
		/// AddRule: typeof(SetComparisonConstraintHasRoleSequence)
		/// Disallow direct edits to coupled exclusion constraints
		/// </summary>
		private static void RoleSequenceAddRule(ElementAddedEventArgs e)
		{
			ExclusionConstraint exclusionConstraint = ((SetComparisonConstraintHasRoleSequence)e.ModelElement).ExternalConstraint as ExclusionConstraint;
			if (null != exclusionConstraint && null != exclusionConstraint.ExclusiveOrMandatoryConstraint)
			{
				ThrowDirectExclusionConstraintEditException();
			}
		}
		#endregion // RoleSequenceAddRule
		#region RoleDeleteRule
		partial class RoleDeletingRuleClass
		{
			private bool myAllowEdit;
			/// <summary>
			/// DeletingRule: typeof(ConstraintRoleSequenceHasRole)
			/// Enforce that any role deleted from a MandatoryConstraint is also
			/// deleted from a coupled ExclusionConstraint.
			/// </summary>
			private void RoleDeletingRule(ElementDeletingEventArgs e)
			{
				if (myAllowEdit)
				{
					return;
				}
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				if (link.Role.IsDeleting)
				{
					return;
				}
				ConstraintRoleSequence sequence = link.ConstraintRoleSequence;
				MandatoryConstraint mandatoryConstraint;
				ExclusionConstraint exclusionConstraint;
				SetComparisonConstraintRoleSequence comparisonConstraintSequence;
				ExclusiveOrConstraintCoupler coupler;
				if (null != (comparisonConstraintSequence = sequence as SetComparisonConstraintRoleSequence))
				{
					if (null != (exclusionConstraint = comparisonConstraintSequence.ExternalConstraint as ExclusionConstraint) &&
						!exclusionConstraint.IsDeleting &&
						null != (coupler = ExclusiveOrConstraintCoupler.GetLinkToExclusiveOrMandatoryConstraint(exclusionConstraint)) &&
						!coupler.IsDeleting)
					{
						// This will handle the exception for the comparison constrant as well, which will
						// immediately delete these thinks.
						ThrowDirectExclusionConstraintEditException();
					}
				}
				else if (null != (mandatoryConstraint = sequence as MandatoryConstraint))
				{
					if (!mandatoryConstraint.IsDeleting &&
						null != (coupler = ExclusiveOrConstraintCoupler.GetLinkToExclusiveOrExclusionConstraint(mandatoryConstraint)) &&
						!coupler.IsDeleting)
					{
						myAllowEdit = true;
						try
						{
							// We need to add a new sequence with a single role in it to the corresponding exclusion constraint.
							// Add rules can fire due to either add or insert commands, so we need to find the
							// correct position in the sequence.
							coupler.ExclusionConstraint.RoleSequenceCollection.RemoveAt(ConstraintRoleSequenceHasRole.GetLinksToRoleCollection(sequence).IndexOf(link));
						}
						finally
						{
							myAllowEdit = false;
						}
					}
				}
			}
		}
		#endregion // RoleDeleteRule
		#region MandatoryConstraintChangeRule
		/// <summary>
		/// ChangeRule: typeof(MandatoryConstraint)
		/// </summary>
		private static void MandatoryConstraintChangeRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == MandatoryConstraint.ModalityDomainPropertyId)
			{
				MandatoryConstraint mandatoryConstraint = e.ModelElement as MandatoryConstraint;
				if (!mandatoryConstraint.IsDeleted)
				{
					ExclusionConstraint exclusionConstraint = mandatoryConstraint.ExclusiveOrExclusionConstraint;
					if (exclusionConstraint != null)
					{
						exclusionConstraint.Modality = mandatoryConstraint.Modality;
					}
				}
			}
		}
		#endregion // MandatoryConstraintChangeRule
		#region ExclusionConstraintChangeRule
		/// <summary>
		/// ChangeRule: typeof(ExclusionConstraint)
		/// Synchronize Modality between coupled exclusion and mandatory constraints
		/// </summary>
		private static void ExclusionConstraintChangeRule(ElementPropertyChangedEventArgs e)
		{
			if (e.DomainProperty.Id == ExclusionConstraint.ModalityDomainPropertyId)
			{
				ExclusionConstraint exclusionConstraint = e.ModelElement as ExclusionConstraint;
				if (!exclusionConstraint.IsDeleted)
				{
					MandatoryConstraint mandatoryConstraint = exclusionConstraint.ExclusiveOrMandatoryConstraint;
					if (mandatoryConstraint != null)
					{
						mandatoryConstraint.Modality = exclusionConstraint.Modality;
					}
				}
			}
		}
		#endregion // ExclusionConstraintChangeRule
	}
	#endregion // ExlusiveOrConstraintCoupler class
	#region FrequencyConstraint class
	public partial class FrequencyConstraint : IModelErrorOwner
	{
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Returns the error associated with the constraint.
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			if (filter == 0)
			{
				filter = (ModelErrorUses)(-1);
			}
			foreach (ModelErrorUsage baseError in base.GetErrorCollection(filter))
			{
				yield return baseError;
			}
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
			{
				FrequencyConstraintMinMaxError minMaxError;
				FrequencyConstraintExactlyOneError exactlyOneError;
				FrequencyConstraintNonRestrictiveRangeError nonRestrictiveRangeError;
				if (null != (minMaxError = FrequencyConstraintMinMaxError))
				{
					yield return minMaxError;
				}
				else if (null != (exactlyOneError = FrequencyConstraintExactlyOneError))
				{
					yield return exactlyOneError;
				}
				else if (null != (nonRestrictiveRangeError = FrequencyConstraintNonRestrictiveRangeError))
				{
					yield return nonRestrictiveRangeError;
				}
				FrequencyConstraintViolatedByUniquenessConstraintError uniquenessViolationError = FrequencyConstraintViolatedByUniquenessConstraintError;
				if (uniquenessViolationError != null)
				{
					yield return uniquenessViolationError;
				}
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
			base.ValidateErrors(notifyAdded);
			VerifyMinMaxError(notifyAdded);
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
			base.DelayValidateErrors();
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateFrequencyConstraintMinMaxError);
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion //IModelErrorOwner Implementation
		#region MinMaxError Validation
		/// <summary>
		/// Validator callback for FrequencyConstraintMinMaxError
		/// </summary>
		private static void DelayValidateFrequencyConstraintMinMaxError(ModelElement element)
		{
			(element as FrequencyConstraint).VerifyMinMaxError(null);
		}
		/// <summary>
		/// Verify that the Min and Max values are consistent
		/// </summary>
		/// <param name="notifyAdded">If not null, this is being called during
		/// load when rules are not in place. Any elements that are added
		/// must be notified back to the caller.</param>
		private void VerifyMinMaxError(INotifyElementAdded notifyAdded)
		{
			if (IsDeleted)
			{
				return;
			}

			FrequencyConstraintMinMaxError minMaxError = FrequencyConstraintMinMaxError;
			FrequencyConstraintExactlyOneError exactlyOneError = FrequencyConstraintExactlyOneError;
			FrequencyConstraintNonRestrictiveRangeError nonRestrictiveError = FrequencyConstraintNonRestrictiveRangeError; 
			int min = MinFrequency;
			int max = MaxFrequency;
			if (max > 0 && min > max)
			{
				//Adding the Error to the model
				if (minMaxError == null)
				{
					minMaxError = new FrequencyConstraintMinMaxError(Store);
					minMaxError.FrequencyConstraint = this;
					minMaxError.Model = Model;
					minMaxError.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(minMaxError, true);
					}
				}
				if (exactlyOneError != null)
				{
					exactlyOneError.Delete();
				}
				if (nonRestrictiveError != null)
				{
					nonRestrictiveError.Delete();
				}
			}
			else 
			{
				if (minMaxError != null)
				{
					minMaxError.Delete();
				}
				if (min == 1 && max <= 1)
				{
					if (max == 1)
					{
						if (exactlyOneError == null)
						{
							exactlyOneError = new FrequencyConstraintExactlyOneError(Store);
							exactlyOneError.FrequencyConstraint = this;
							exactlyOneError.Model = Model;
							exactlyOneError.GenerateErrorText();
							if (notifyAdded != null)
							{
								notifyAdded.ElementAdded(exactlyOneError, true);
							}
						}
						if (nonRestrictiveError != null)
						{
							nonRestrictiveError.Delete();
						}
					}
					else if (max == 0)
					{
						if (nonRestrictiveError == null)
						{
							nonRestrictiveError = new FrequencyConstraintNonRestrictiveRangeError(Store);
							nonRestrictiveError.FrequencyConstraint = this;
							nonRestrictiveError.Model = Model;
							nonRestrictiveError.GenerateErrorText();
							if (notifyAdded != null)
							{
								notifyAdded.ElementAdded(nonRestrictiveError, true);
							}
						}
						if (exactlyOneError != null)
						{
							exactlyOneError.Delete();
						}
					}
				}
				else
				{
					if (exactlyOneError != null)
					{
						exactlyOneError.Delete();
					}
					if (nonRestrictiveError != null)
					{
						nonRestrictiveError.Delete();
					}
				}
			}
		}
		#endregion // MinMaxError Validation
		#region FrequencyConstraintMinMaxRule
		/// <summary>
		/// ChangeRule: typeof(FrequencyConstraint)
		/// </summary>
		private static void FrequencyConstraintMinMaxRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeId = e.DomainProperty.Id;
			if (attributeId == FrequencyConstraint.MinFrequencyDomainPropertyId ||
				attributeId == FrequencyConstraint.MaxFrequencyDomainPropertyId)
			{
				FrequencyConstraint fc = e.ModelElement as FrequencyConstraint;
				if (!fc.IsDeleted)
				{
					FrameworkDomainModel.DelayValidateElement(fc, DelayValidateFrequencyConstraintMinMaxError);
				}
			}
		}
		#endregion // FrequencyConstraintMinMaxRule
		#region ConvertToUniquenessConstraint method
		/// <summary>
		/// The key for a value placed in the top level transaction context info
		/// to indicate that a frequency constraint is being converted to a uniqueness
		/// constraint. Used with <see cref="ConvertingToUniquenessConstraintKey"/>
		/// to facilitate shape conversion.
		/// </summary>
		public static readonly object ConvertingFromFrequencyConstraintKey = new object();
		/// <summary>
		/// The key for a value placed in the top level transaction context info
		/// to indicate the uniquqness constraint that is being created from a frequency
		/// constraint conversion. Used with <see cref="ConvertingFromFrequencyConstraintKey"/>
		/// to facilitate shape conversion.
		/// </summary>
		public static readonly object ConvertingToUniquenessConstraintKey = new object();
		/// <summary>
		/// Convert this <see cref="FrequencyConstraint"/> to an equivalent <see cref="UniquenessConstraint"/>
		/// Should be called only when the <see cref="MinFrequency"/> and <see cref="MaxFrequency"/> are both one.
		/// </summary>
		/// <returns><see langword="true"/> if conversion successful</returns>
		public bool ConvertToUniquenessConstraint()
		{
			int min = MinFrequency;
			int max = MaxFrequency;
			Debug.Assert(min == 1 && max == 1, "Only convert frequency constraints with a min/max of exactly one to uniqueness");
			bool retVal = false;
			if (min == 1 && max == 1)
			{
				Store store = Store;
				using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.ConvertFrequencyToUniquenessTransactionName))
				{
					UniquenessConstraint uniqueness = (FactTypeCollection.Count == 1) ?
						UniquenessConstraint.CreateInternalUniquenessConstraint(store) :
						new UniquenessConstraint(store);
					uniqueness.Modality = Modality;
					uniqueness.Model = Model;
					// UNDONE: MULTISHAPE I should be able to do the following line before
					// deleting the frequency constraint, but adding a shape with fixups 
					// such as done by the FrequencyConstraintShape to place a new external
					// uniqueness shape) is having nasty side-effects with later validation
					// by the multishape code and ends up leaving new links dangling
					// and eventually deleting the new shape. The workaround is to add no
					// roles and facttypes (hence not passing facttypes[0] to CreateInternalUniquenessConstraint)
					//uniqueness.RoleCollection.AddRange(RoleCollection);
					Role[] roles = RoleCollection.ToArray();
					IDictionary<object, object> contextInfo = t.TopLevelTransaction.Context.ContextInfo;
					contextInfo[ConvertingFromFrequencyConstraintKey] = this;
					contextInfo[ConvertingToUniquenessConstraintKey] = uniqueness;
					Delete();
					contextInfo.Remove(ConvertingFromFrequencyConstraintKey);
					contextInfo.Remove(ConvertingToUniquenessConstraintKey);
					uniqueness.RoleCollection.AddRange(roles);
					t.Commit();
					retVal = true;
				}
			}
			return retVal;
		}
		#endregion // ConvertToUniquenessConstraint method
	}
	#endregion // FrequencyConstraint class
	#region Ring Constraint class
	public partial class RingConstraint : IModelErrorOwner
	{
		#region IModelErrorOwner Members
		/// <summary>
		/// Return errors associated with the constraint
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			if (filter == 0)
			{
				filter = (ModelErrorUses)(-1);
			}
			foreach (ModelErrorUsage baseError in base.GetErrorCollection(filter))
			{
				yield return baseError;
			}
			if (0 != (filter & (ModelErrorUses.BlockVerbalization | ModelErrorUses.DisplayPrimary)))
			{
				RingConstraintTypeNotSpecifiedError notSpecified = this.RingConstraintTypeNotSpecifiedError;
				if (notSpecified != null)
				{
					yield return new ModelErrorUsage(notSpecified, ModelErrorUses.BlockVerbalization);
				}
			}
		}
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			return GetErrorCollection(filter);
		}
		/// <summary>
		/// Implements IModelErrorOwner.ValidateErrors
		/// </summary>
		/// <param name="notifyAdded">INotifyElementAdded</param>
		protected new void ValidateErrors(INotifyElementAdded notifyAdded)
		{
			base.ValidateErrors(notifyAdded);
			VerifyTypeNotSpecifiedRule(notifyAdded);
		}
		void IModelErrorOwner.ValidateErrors(INotifyElementAdded notifyAdded)
		{
			this.ValidateErrors(notifyAdded);
		}
		/// <summary>
		/// Implements IModelErrorOwner.DelayValidateErrors
		/// </summary>
		protected new void DelayValidateErrors()
		{
			base.DelayValidateErrors();
			FrameworkDomainModel.DelayValidateElement(this, DelayValidateRingConstraintTypeNotSpecifiedError);
		}
		void IModelErrorOwner.DelayValidateErrors()
		{
			DelayValidateErrors();
		}
		#endregion//Ring Constraint class
		#region RingConstraintTypeNotSpecifiedError Rule
		/// <summary>
		/// Validator callback for RingConstraintTypeNotSpecifiedError
		/// </summary>
		private static void DelayValidateRingConstraintTypeNotSpecifiedError(ModelElement element)
		{
			(element as RingConstraint).VerifyTypeNotSpecifiedRule(null);
		}
		/// <summary>
		/// Add, remove, and otherwise validate RingConstraintTypeNotSpecified error
		/// </summary>
		/// <param name="notifyAdded">If not null, this is being called during
		/// load when rules are not in place. Any elements that are added
		/// must be notified back to the caller.</param>
		private void VerifyTypeNotSpecifiedRule(INotifyElementAdded notifyAdded)
		{
			if (this.IsDeleted)
			{
				return;
			}

			RingConstraintTypeNotSpecifiedError notSpecified = this.RingConstraintTypeNotSpecifiedError;
			//error should only appear if ring constraint type is not definded
			if (this.RingType == RingConstraintType.Undefined)
			{
				if (notSpecified == null)
				{
					notSpecified = new RingConstraintTypeNotSpecifiedError(this.Store);
					notSpecified.RingConstraint = this;
					notSpecified.Model = this.Model;
					notSpecified.GenerateErrorText();
					if (notifyAdded != null)
					{
						notifyAdded.ElementAdded(notSpecified);
					}
				}
			}
			else if (notSpecified != null)
			{
				notSpecified.Delete();
			}
		}
		#endregion //Type Not Specified Error Rule
		#region RingConstraintTypeChangeRule
		/// <summary>
		/// ChangeRule: typeof(RingConstraint)
		/// </summary>
		private static void RingConstraintTypeChangeRule(ElementPropertyChangedEventArgs e)
		{
			Guid attributeId = e.DomainProperty.Id;
			if (attributeId == RingConstraint.RingTypeDomainPropertyId)
			{
				RingConstraint rc = e.ModelElement as RingConstraint;
				if (!rc.IsDeleted)
				{
					FrameworkDomainModel.DelayValidateElement(rc, DelayValidateRingConstraintTypeNotSpecifiedError);
				}
			}
		}
		#endregion // RingConstraintTypeChangeRule
		#region Verbalization
		/// <summary>
		/// Helper function for defering ring verbalization to its constituent parts
		/// </summary>
		private void VerbalizeByRingType(TextWriter writer, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizationContext verbalizationContext, VerbalizationSign sign)
		{
			RingVerbalizerBase verbalizeFirst = null;
			RingVerbalizerBase verbalizeSecond = null;
			switch (RingType)
			{
				case RingConstraintType.Acyclic:
					verbalizeFirst = AcyclicRingVerbalizer.GetVerbalizer();
					break;
				case RingConstraintType.AcyclicIntransitive:
					verbalizeFirst = AcyclicRingVerbalizer.GetVerbalizer();
					verbalizeSecond = IntransitiveRingVerbalizer.GetVerbalizer();
					break;
				case RingConstraintType.Antisymmetric:
					verbalizeFirst = AntisymmetricRingVerbalizer.GetVerbalizer();
					break;
				case RingConstraintType.Asymmetric:
					verbalizeFirst = AsymmetricRingVerbalizer.GetVerbalizer();
					break;
				case RingConstraintType.AsymmetricIntransitive:
					verbalizeFirst = AsymmetricRingVerbalizer.GetVerbalizer();
					verbalizeSecond = IntransitiveRingVerbalizer.GetVerbalizer();
					break;
				case RingConstraintType.Intransitive:
					verbalizeFirst = IntransitiveRingVerbalizer.GetVerbalizer();
					break;
				case RingConstraintType.Irreflexive:
					verbalizeFirst = IrreflexiveRingVerbalizer.GetVerbalizer();
					break;
				case RingConstraintType.Symmetric:
					verbalizeFirst = SymmetricRingVerbalizer.GetVerbalizer();
					break;
				case RingConstraintType.SymmetricIntransitive:
					verbalizeFirst = SymmetricRingVerbalizer.GetVerbalizer();
					verbalizeSecond = IntransitiveRingVerbalizer.GetVerbalizer();
					break;
				case RingConstraintType.SymmetricIrreflexive:
					verbalizeFirst = SymmetricRingVerbalizer.GetVerbalizer();
					verbalizeSecond = IrreflexiveRingVerbalizer.GetVerbalizer();
					break;
			}
			if (verbalizeFirst != null)
			{
				using ((IDisposable)verbalizeFirst)
				{
					verbalizeFirst.Initialize(this);
					verbalizationContext.DeferVerbalization(verbalizeFirst, DeferVerbalizationOptions.None, null);
				}
			}
			if (verbalizeSecond != null)
			{
				using ((IDisposable)verbalizeSecond)
				{
					verbalizeSecond.Initialize(this);
					verbalizationContext.DeferVerbalization(verbalizeSecond, DeferVerbalizationOptions.AlwaysWriteLine, null);
				}
			}
		}
		#region RingVerbalizerBase class and partials for each standalone ring type
		/// <summary>
		/// Base class for child helper ring verbalizations.
		/// Provides properties expected by the generated verbalization code.
		/// </summary>
		private abstract class RingVerbalizerBase
		{
			#region Member Variables
			private RingConstraint myConstraint;
			#endregion // Member Variables
			#region Cache Methods
			/// <summary>
			/// Attach this instance to a context <see cref="RingConstraint"/>
			/// </summary>
			public void Initialize(RingConstraint constraint)
			{
				myConstraint = constraint;
			}
			protected void DisposeHelper()
			{
				myConstraint = null;
			}
			#endregion // Cache Methods
			#region Passthrough properties
			protected ConstraintModality Modality
			{
				get
				{
					return myConstraint.Modality;
				}
			}
			protected LinkedElementCollection<Role> RoleCollection
			{
				get
				{
					return myConstraint.RoleCollection;
				}
			}
			protected LinkedElementCollection<FactType> FactTypeCollection
			{
				get
				{
					return myConstraint.FactTypeCollection;
				}
			}
			#endregion // Passthrough properties
		}
		private partial class AcyclicRingVerbalizer : RingVerbalizerBase { }
		private partial class AntisymmetricRingVerbalizer : RingVerbalizerBase { }
		private partial class AsymmetricRingVerbalizer : RingVerbalizerBase { }
		private partial class IntransitiveRingVerbalizer : RingVerbalizerBase { }
		private partial class IrreflexiveRingVerbalizer : RingVerbalizerBase { }
		private partial class SymmetricRingVerbalizer : RingVerbalizerBase { }
		#endregion // RingVerbalizerBase class and partials for each standalone ring type
		#endregion // Verbalization
	}
	#endregion //Ring Constraint class
	#region PreferredIdentifierFor implementation
	public partial class UniquenessConstraint
	{
		ObjectType IConstraint.PreferredIdentifierFor
		{
			get
			{
				return PreferredIdentifierFor;
			}
			set
			{
				PreferredIdentifierFor = value;
			}
		}
	}
	#endregion // PreferredIdentifierFor implementation
	#region ModelError classes
	#region TooManyRoleSequencesError class
	[ModelErrorDisplayFilter(typeof(ConstraintStructureErrorCategory))]
	public partial class TooManyRoleSequencesError
	{
		#region Base overrides
		/// <summary>
		/// Generate and set text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			ORMNamedElement parent = SetComparisonConstraint;
			if (parent == null)
			{
				parent = SetConstraint;
				Debug.Assert(parent != null);
			}
			string parentName = (parent != null) ? parent.Name : string.Empty;
			ErrorText = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorConstraintHasTooManyRoleSequencesText, parentName);
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
	}
	#endregion // TooManyRoleSequencesError class
	#region TooFewRoleSequencesError class
	[ModelErrorDisplayFilter(typeof(ConstraintStructureErrorCategory))]
	public partial class TooFewRoleSequencesError
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			ORMNamedElement parent = SetComparisonConstraint;
			if (parent == null)
			{
				parent = SetConstraint;
				Debug.Assert(parent != null);
			}
			string parentName = (parent != null) ? parent.Name : "";
			ErrorText = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorConstraintHasTooFewRoleSequencesText, parentName);
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
	}
	#endregion // TooFewRoleSequencesError class
	#region ExternalConstraintRoleSequenceArityMismatchError class
	[ModelErrorDisplayFilter(typeof(ConstraintStructureErrorCategory))]
	public partial class ExternalConstraintRoleSequenceArityMismatchError
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			SetComparisonConstraint parent = this.Constraint;
			string parentName = (parent != null) ? parent.Name : "";
			ErrorText = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorConstraintExternalConstraintArityMismatch, parentName);
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
	}
	#endregion // ExternalConstraintRoleSequenceArityMismatchError class
	#region CompatibleRolePlayerTypeError class
	[ModelErrorDisplayFilter(typeof(ConstraintStructureErrorCategory))]
	public partial class CompatibleRolePlayerTypeError
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			SetComparisonConstraint multiColumnParent = SetComparisonConstraint;
			ORMNamedElement namedParent;
			bool useColumn;
			if (multiColumnParent != null)
			{
				namedParent = multiColumnParent;
				useColumn = multiColumnParent.RoleSequenceCollection[0].RoleCollection.Count > 1;
			}
			else
			{
				namedParent = SetConstraint;
				useColumn = false;
			}
			Debug.Assert(namedParent != null, "Parent must be single column or multi column");
			string parentName = (namedParent != null) ? namedParent.Name : "";
			string modelName = this.Model.Name;
			ErrorText = useColumn ?
				string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorSetComparisonConstraintCompatibleRolePlayerTypeError, parentName, modelName, (Column + 1).ToString(CultureInfo.InvariantCulture)) :
				string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorSetConstraintCompatibleRolePlayerTypeError, parentName, modelName);
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
		#region Accessor Properties
		/// <summary>
		/// Return either the single column or multi column
		/// constraint associated with this error.
		/// </summary>
		public ORMNamedElement ParentConstraint
		{
			get
			{
				return (ORMNamedElement)SetComparisonConstraint ?? SetConstraint;
			}
		}
		#endregion // Accessor Properties
	}
	#endregion // CompatibleRolePlayerTypeError class
	#region JoinPathRequiredError class
	[ModelErrorDisplayFilter(typeof(ConstraintStructureErrorCategory))]
	public partial class JoinPathRequiredError
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			IModelErrorDisplayContext displayContext = (IModelErrorDisplayContext)RoleSequence;
			ErrorText = Utility.UpperCaseFirstLetter(string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorConstraintRoleSequenceJoinPathRequiredError, displayContext.ErrorDisplayContext));
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
	}
	#endregion // JoinPathRequiredError class
	#region FrequencyConstraintMinMaxError class
	[ModelErrorDisplayFilter(typeof(ConstraintStructureErrorCategory))]
	public partial class FrequencyConstraintMinMaxError
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			FrequencyConstraint parent = this.FrequencyConstraint;
			ErrorText = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorFrequencyConstraintMinMaxError, (parent != null) ? parent.Name : "", Model.Name);
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
	}
	#endregion // FrequencyConstraintMinMaxError class
	#region FrequencyConstraintExactlyOneError class
	[ModelErrorDisplayFilter(typeof(ConstraintStructureErrorCategory))]
	public partial class FrequencyConstraintExactlyOneError
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			FrequencyConstraint parent = this.FrequencyConstraint;
			ErrorText = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorFrequencyConstraintExactlyOneError, (parent != null) ? parent.Name : "", Model.Name);
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
	}
	#endregion // FrequencyConstraintExactlyOneError class
	#region FrequencyConstraintNonRestrictiveRangeError class
	[ModelErrorDisplayFilter(typeof(ConstraintStructureErrorCategory))]
	public partial class FrequencyConstraintNonRestrictiveRangeError
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			FrequencyConstraint parent = this.FrequencyConstraint;
			ErrorText = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorFrequencyConstraintNonRestrictiveRangeError, (parent != null) ? parent.Name : "", Model.Name);
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
	}
	#endregion // FrequencyConstraintNonRestrictiveRangeError class
	#region FrequencyConstraintViolatedByUniquenessConstraintError class
	[ModelErrorDisplayFilter(typeof(ConstraintImplicationAndContradictionErrorCategory))]
	public partial class FrequencyConstraintViolatedByUniquenessConstraintError : IRepresentModelElements
	{
		#region Base Overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			FrequencyConstraint parent = FrequencyConstraint;
			ORMModel model = Model;
			string parentName = (parent != null) ? parent.Name : "";
			string modelName = (model != null) ? model.Name : "";
			ErrorText = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorFrequencyConstraintViolatedByUniquenessConstraintText, parentName, modelName);
			string currentText = ErrorText;
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
		#endregion// Base overrides
		#region IRepresentModelElements Implementation
		/// <summary>
		/// Implements <see cref="IRepresentModelElements.GetRepresentedElements"/>
		/// </summary>
		protected new ModelElement[] GetRepresentedElements()
		{
			// The base implementation returns the same set of elements, but
			// order is not guaranteed.
			return new ModelElement[] { FrequencyConstraint };
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion // IRepresentModelElements Implementation
	}
	#endregion // FrequencyConstraintViolatedByUniquenessConstraintError class
	#region ImpliedInternalUniquenessConstraintError class
	[ModelErrorDisplayFilter(typeof(FactTypeDefinitionErrorCategory))]
	public partial class ImpliedInternalUniquenessConstraintError
	{
		#region Base Overrides
		/// <summary>
		/// Generate the text for the error 
		/// </summary>
		public override void GenerateErrorText()
		{
			FactType parent = FactType;
			string modelName = "";
			string parentName = "";
			if (parent != null)
			{
				parentName = parent.Name;
				ORMModel model = parent.Model;
				if (model != null)
				{
					modelName = model.Name;
				}
			}
			ErrorText = string.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorImpliedInternalUniquenessConstraintError, parentName, modelName);
		}
		/// <summary>
		/// Regenerates the error text when the Fact type changes
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.OwnerNameChange | RegenerateErrorTextEvents.ModelNameChange;
			}
		}
		#endregion
	}
	#endregion // ImpliedInternalUniquenessConstraintError class
	#region RingConstraintTypeNotSpecifiedError class
	[ModelErrorDisplayFilter(typeof(ConstraintStructureErrorCategory))]
	public partial class RingConstraintTypeNotSpecifiedError
	{
		#region Base overrides
		/// <summary>
		/// Get Text to display for the RingConstraintTypeNotSpecified error
		/// </summary>
		public override void GenerateErrorText()
		{
			RingConstraint parent = this.RingConstraint;
			string parentName = (parent != null) ? parent.Name : "";
			string modelName = this.Model.Name;
			ErrorText = string.Format(CultureInfo.InvariantCulture, ResourceStrings.RingConstraintTypeNotSpecifiedError, parentName, modelName);
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
		#endregion //Base overrides
	}
	#endregion // RingConstraintTypeNotSpecifiedError class
	#region ImplicationError
	[ModelErrorDisplayFilter(typeof(ConstraintImplicationAndContradictionErrorCategory))]
	public partial class ImplicationError
	{
		#region Base Overrides
		/// <summary>
		/// Generates the text for the error to be displayed.
		/// </summary>
		public override void GenerateErrorText()
		{
			string errorName;
			//Do not know whether the underlying constraint is Set or SetComparison
			//The error has property for each
			if (this.SetComparisonConstraint != null)
			{
				errorName = SetComparisonConstraint.Name;
				Debug.Assert(SetConstraint == null);
			}
			else
			{
				errorName = SetConstraint.Name;
			}

			ErrorText = String.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorConstraintImplication, errorName, Model.Name);
		}
		/// <summary>
		/// Regenerate error text when the constraint name or model name changes
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.OwnerNameChange | RegenerateErrorTextEvents.ModelNameChange;
			}
		}
		#endregion //Base Overrides
	}
	[ModelErrorDisplayFilter(typeof(ConstraintImplicationAndContradictionErrorCategory))]
	public partial class EqualityOrSubsetImpliedByMandatoryError
	{
		#region Base Overrides
		/// <summary>
		/// Generates the text for the error to be displayed.
		/// </summary>
		public override void GenerateErrorText()
		{
			SetComparisonConstraint constraint = EqualityOrSubsetConstraint;
			string errorName = constraint.Name;

			ErrorText = String.Format(CultureInfo.InvariantCulture, ((IConstraint)constraint).ConstraintType == ConstraintType.Subset ? ResourceStrings.ModelErrorConstraintImplicationSubsetMandatory : ResourceStrings.ModelErrorConstraintImplicationEqualityMandatory, errorName, Model.Name);
		}
		/// <summary>
		/// Regenerate error text when the constraint name or model name changes
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.OwnerNameChange | RegenerateErrorTextEvents.ModelNameChange;
			}
		}
		/// <summary>
		/// Redirect the SetComparisonConstraint to the EqualityOrSubsetConstraint property
		/// </summary>
		public override SetComparisonConstraint SetComparisonConstraint
		{
			get
			{
				return EqualityOrSubsetConstraint;
			}
			set
			{
				EqualityOrSubsetConstraint = value;
			}
		}
		#endregion //Base Overrides
	}
	#endregion
	#region ContradictionError
	[ModelErrorDisplayFilter(typeof(ConstraintImplicationAndContradictionErrorCategory))]
	public partial class ContradictionError
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			StringBuilder sb = new StringBuilder();
			ReadOnlyLinkedElementCollection<SetComparisonConstraint> constraints = SetComparisonConstraintCollection;
			int numOfConstraints = constraints.Count;

			for (int i = 0; i < numOfConstraints; ++i)
			{
				// UNDONE: Localize name delimiter format string
				sb.AppendFormat("'{0}'", constraints[i].Name);

				if (i == numOfConstraints - 2)
				{
					// UNDONE: Localize final list separator
					sb.Append(" and ");
				}
				else if (i != numOfConstraints - 1)
				{
					sb.Append(CultureInfo.CurrentCulture.TextInfo.ListSeparator);
					sb.Append(" ");
				}
			}
			string errorConstraints = sb.ToString();

			ErrorText = String.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorConstraintContradiction, errorConstraints, Model.Name);
		}
		/// <summary>
		/// Regenerate the error text when the model name changes
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange;
			}
		}
		#endregion // Base overrides
	}
	[ModelErrorDisplayFilter(typeof(ConstraintImplicationAndContradictionErrorCategory))]
	public partial class ExclusionContradictsMandatoryError : IRepresentModelElements
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			StringBuilder sb = new StringBuilder();

			int numOfExclusionConstraints = ExclusionConstraint.Count;

			for (int i = 0; i < numOfExclusionConstraints; ++i)
			{
				// UNDONE: Localize name delimiter format string
				sb.AppendFormat("'{0}'", ExclusionConstraint[i].Name);

				if (i == numOfExclusionConstraints - 1)
				{
					// UNDONE: Localize final list separator
					sb.Append(" and ");
				}
				else
				{
					sb.Append(CultureInfo.CurrentCulture.TextInfo.ListSeparator);
					sb.Append(" ");
				}
			}

			//The lists of exclusion constraints and mandatory constraints will be separated by 'and'

			int numOfMandatoryConstraints = MandatoryConstraint.Count;

			for (int i = 0; i < numOfMandatoryConstraints; ++i)
			{
				// UNDONE: Localize name delimiter format string
				sb.AppendFormat("'{0}'", MandatoryConstraint[i].Name);

				if (i != numOfMandatoryConstraints - 1)
				{
					sb.Append(CultureInfo.CurrentCulture.TextInfo.ListSeparator);
					sb.Append(" ");
				}
			}
			string errorConstraints = sb.ToString();
			ErrorText = String.Format(CultureInfo.InvariantCulture, ResourceStrings.ModelErrorConstraintContradiction, errorConstraints, Model.Name);
		}
		/// <summary>
		/// Regenerate the error text when the model name changes
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
			// Reimplement to ensure order. Put the ExclusionConstraints first.
			LinkedElementCollection<ExclusionConstraint> exclusionConstraints = ExclusionConstraint;
			int exclusionCount = exclusionConstraints.Count;
			LinkedElementCollection<MandatoryConstraint> mandatoryConstraints = MandatoryConstraint;
			int mandatoryCount = mandatoryConstraints.Count;
			ModelElement[] retVal = new ModelElement[exclusionCount + mandatoryCount];
			for (int i = 0; i < exclusionCount; ++i)
			{
				retVal[i] = exclusionConstraints[i];
			}
			for (int i = 0; i < mandatoryCount; ++i)
			{
				retVal[i + exclusionCount] = mandatoryConstraints[i];
			}
			return retVal;
		}
		ModelElement[] IRepresentModelElements.GetRepresentedElements()
		{
			return GetRepresentedElements();
		}
		#endregion // IRepresentModelElements Implementation
	}
	#endregion

	[ModelErrorDisplayFilter(typeof(ConstraintImplicationAndContradictionErrorCategory))]
	public partial class NotWellModeledSubsetAndMandatoryError
	{
		#region Base overrides
		/// <summary>
		/// Generate text for the error
		/// </summary>
		public override void GenerateErrorText()
		{
			ErrorText = String.Format(CultureInfo.InvariantCulture,
				ResourceStrings.ModelErrorNotWellModeledSubsetAndMandatoryError,
				SubsetConstraint.Name, MandatoryConstraint.Name,
				Model.Name);
		}
		/// <summary>
		/// Regenerate the error text when the model name changes
		/// </summary>
		public override RegenerateErrorTextEvents RegenerateEvents
		{
			get
			{
				return RegenerateErrorTextEvents.ModelNameChange;
			}
		}
		#endregion // Base overrides
	}
	#endregion // ModelError classes
	#region ExclusionType enum
	/// <summary>
	/// Represents the current setting on an exclusion constraint
	/// </summary>
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
	public enum RoleSequenceStyles
	{
		/// <summary>
		/// Constraint uses a single role sequence
		/// </summary>
		OneRoleSequence = 1,
		/// <summary>
		/// Constraint uses one or more role sequences
		/// </summary>
		OneOrMoreRoleSequences = 2,
		/// <summary>
		/// Constraint uses exactly two role sequences
		/// </summary>
		TwoRoleSequences = 4,
		/// <summary>
		/// Constraint uses >=2 role sequences
		/// </summary>
		MultipleRowSequences = 8,
		/// <summary>
		/// A mask to extract the sequence multiplicity values
		/// </summary>
		SequenceMultiplicityMask = OneRoleSequence | OneOrMoreRoleSequences | TwoRoleSequences | MultipleRowSequences,
		/// <summary>
		/// Each role sequence contains exactly one role
		/// </summary>
		OneRolePerSequence = 0x10,
		/// <summary>
		/// Each role sequence contains exactly two roles
		/// </summary>
		TwoRolesPerSequence = 0x20,
		/// <summary>
		/// Each role sequence can contain >=1 roles
		/// </summary>
		MultipleRolesPerSequence = 0x40,
		/// <summary>
		/// The role sequence must contain n or n-1 roles. Applicable
		/// to OneRoleSequence constraints only
		/// </summary>
		AtLeastCountMinusOneRolesPerSequence = 0x80,
		/// <summary>
		/// A mask to extract the row multiplicity values
		/// </summary>
		RoleMultiplicityMask = OneRolePerSequence | TwoRolesPerSequence | MultipleRolesPerSequence | AtLeastCountMinusOneRolesPerSequence,
		/// <summary>
		/// The order of the role sequences is significant
		/// </summary>
		OrderedRoleSequences = 0x100,
		/// <summary>
		/// Each of the columns must be type compatible
		/// </summary>
		CompatibleColumns = 0x200,
	}
	#endregion // RoleSequenceStyles enum
	#region ConstraintType enum
	/// <summary>
	/// A list of constraint types.
	/// </summary>
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
		/// An implied mandatory constraint. Applied to one or more roles played
		/// by the same object type.
		/// </summary>
		ImpliedMandatory,
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
	public enum ConstraintStorageStyle
	{
		/// <summary>
		/// The constraint is stored as a single role sequence. Depending on the
		/// constraint, the roles in the sequence may be treated conceptually as
		/// single roles in multiple sequences.
		/// </summary>
		SetConstraint,
		/// <summary>
		/// The contraint is stored as a sequence of role sequences.  Each role
		/// in a given role sequence must be compatible with roles in the same
		/// position of all other role sequences.
		/// </summary>
		SetComparisonConstraint,
	}
	#endregion // ConstraintStorageStyle enum
	#region ConstraintType and RoleSequenceStyles implementation for all constraints
	public partial class FrequencyConstraint : IConstraint
	{
		#region IConstraint Implementation
		private static readonly IntersectingConstraintValidation[] myIntersectingValidationInfo = new IntersectingConstraintValidation[]
		{
				new IntersectingConstraintValidation(
				    IntersectingConstraintPattern.SetConstraintSameSet,
					IntersectingConstraintPatternOptions.IntersectingConstraintModalityNotWeaker,
					FrequencyConstraintHasFrequencyConstraintViolatedByUniquenessConstraintError.FrequencyConstraintDomainRoleId,
					null,
				    ConstraintType.InternalUniqueness,
				    ConstraintType.ExternalUniqueness),
		};
		/// <summary>
		/// Implements <see cref="IConstraint.GetIntersectingConstraintValidationInfo"/>
		/// </summary>
		/// <returns></returns>
		protected new static IList<IntersectingConstraintValidation> GetIntersectingConstraintValidationInfo()
		{
			return myIntersectingValidationInfo;
		}
		IList<IntersectingConstraintValidation> IConstraint.GetIntersectingConstraintValidationInfo()
		{
			return GetIntersectingConstraintValidationInfo();
		}
		/// <summary>
		/// Implements IConstraint.ConstraintType. Returns ConstraintType.Frequency.
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
		/// Implements IConstraint.RoleSequenceStyles. Returns {OneOrMoreRoleSequences, OneRolePerSequence}.
		/// </summary>
		protected static RoleSequenceStyles RoleSequenceStyles
		{
			get
			{
				return RoleSequenceStyles.OneOrMoreRoleSequences | RoleSequenceStyles.OneRolePerSequence;
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
		/// Implements IConstraint.ConstraintType. Returns ConstraintType.Ring.
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
	public partial class UniquenessConstraint : IConstraint
	{
		#region IConstraint Implementation
		private static readonly IntersectingConstraintValidation[] myIntersectingValidationInfo = new IntersectingConstraintValidation[]
		{
				new IntersectingConstraintValidation(
					IntersectingConstraintPattern.SetConstraintSubset,
					IntersectingConstraintPatternOptions.IntersectingConstraintModalityNotWeaker,
					SetConstraintHasImplicationError.SetConstraintDomainRoleId,
					null,
					ConstraintType.InternalUniqueness,
					ConstraintType.ExternalUniqueness),
		};
		/// <summary>
		/// Implements <see cref="IConstraint.GetIntersectingConstraintValidationInfo"/>
		/// </summary>
		/// <returns></returns>
		protected new static IList<IntersectingConstraintValidation> GetIntersectingConstraintValidationInfo()
		{
			return myIntersectingValidationInfo;
		}
		IList<IntersectingConstraintValidation> IConstraint.GetIntersectingConstraintValidationInfo()
		{
			return GetIntersectingConstraintValidationInfo();
		}
		/// <summary>
		/// Implements IConstraint.ConstraintType. Returns ConstraintType.InternalUniqueness or ConstraintType.ExternalUniqueness.
		/// </summary>
		protected ConstraintType ConstraintType
		{
			get
			{
				return IsInternal ? ConstraintType.InternalUniqueness : ConstraintType.ExternalUniqueness;
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
		/// Implements IConstraint.ConstraintIsInternal
		/// </summary>
		protected new bool ConstraintIsInternal
		{
			get
			{
				return IsInternal;
			}
		}
		bool IConstraint.ConstraintIsInternal
		{
			get
			{
				return ConstraintIsInternal;
			}
		}
		/// <summary>
		/// Implements IConstraint.RoleSequenceStyles.
		/// Returns {MultipleRowSequences, OneRolePerSequence} for external constraint.
		/// Returns {OneRoleSequence, AtLeastCountMinusOneRolesPerSequence} for internal constraint.
		/// </summary>
		protected RoleSequenceStyles RoleSequenceStyles
		{
			get
			{
				return IsInternal ?
					RoleSequenceStyles.OneRoleSequence | RoleSequenceStyles.AtLeastCountMinusOneRolesPerSequence :
					RoleSequenceStyles.MultipleRowSequences | RoleSequenceStyles.OneRolePerSequence;
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
		private static readonly IntersectingConstraintValidation[] myIntersectingValidationInfo = new IntersectingConstraintValidation[]
			{
				//Implication
				new IntersectingConstraintValidation(
					IntersectingConstraintPattern.SetComparisonConstraintSubset,
					IntersectingConstraintPatternOptions.None,
					SetComparisonConstraintHasImplicationError.SetComparisonConstraintDomainRoleId,
					null, // UNDONE: CONSTRAINTINTERSECTION: Examine error implication direction for equality/exclusion/subset
					ConstraintType.Equality),

				//Implication
				new IntersectingConstraintValidation(
					IntersectingConstraintPattern.SetComparisonConstraintSuperset,
					IntersectingConstraintPatternOptions.None,
					SetComparisonConstraintHasImplicationError.SetComparisonConstraintDomainRoleId,
					null, // UNDONE: CONSTRAINTINTERSECTION: Examine error implication direction for equality/exclusion/subset
					ConstraintType.Equality,
					ConstraintType.Subset),
				
				//Contradiction
				new IntersectingConstraintValidation(
					IntersectingConstraintPattern.SetComparisonConstraintSuperset,
					IntersectingConstraintPatternOptions.IntersectingConstraintModalityIgnored,
					SetComparisonConstraintHasExclusionContradictsEqualityError.SetComparisonConstraintDomainRoleId,
					SetComparisonConstraintHasExclusionContradictsEqualityError.SetComparisonConstraintDomainRoleId,
					ConstraintType.Exclusion),

				//TODO: handle disjunctive mandatory too!!!
				//Implication
				new IntersectingConstraintValidation(
					IntersectingConstraintPattern.EqualityImpliedByMandatory,
					IntersectingConstraintPatternOptions.None,
					SetComparisonConstraintHasEqualityOrSubsetImpliedByMandatoryError.SetComparisonConstraintDomainRoleId,
					null,
					ConstraintType.SimpleMandatory),

				//TODO: These cases are not currently implemented
				////Bad ORM
				//new IntersectingConstraintValidation(
				//    IntersectingConstraintPattern.SetComparisonConstraintSubset,
				//    EqualityIsSubsetOfSubsetError.EqualityConstraintDomainRoleId,
				//    ConstraintType.Subset),

				////Bad ORM
				//new IntersectingConstraintValidation(
				//    IntersectingConstraintPattern.SetComparisonConstraintSubset,
				//    EqualityIsSubsetOfExclusionError.EqualityConstraintDomainRoleId,
				//    ConstraintType.Exclusion)
			};
		/// <summary>
		/// Implements <see cref="IConstraint.GetIntersectingConstraintValidationInfo"/>
		/// </summary>
		/// <returns></returns>
		protected new static IList<IntersectingConstraintValidation> GetIntersectingConstraintValidationInfo()
		{
			return myIntersectingValidationInfo;
		}
		IList<IntersectingConstraintValidation> IConstraint.GetIntersectingConstraintValidationInfo()
		{
			return GetIntersectingConstraintValidationInfo();
		}



		/// <summary>
		/// Implements IConstraint.ConstraintType. Returns ConstraintType.Equality.
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
	public partial class ExclusionConstraint : IConstraint, IModelErrorOwner
	{
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Implements IModelErrorOwner.GetErrorCollection
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			if (filter == 0)
			{
				filter = (ModelErrorUses)(-1);
			}
			foreach (ModelErrorUsage baseError in base.GetErrorCollection(filter))
			{
				yield return baseError;
			}
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
			{
				ExclusionContradictsMandatoryError exclusionContradictsMandatory = ExclusionContradictsMandatoryError;
				if (exclusionContradictsMandatory != null)
				{
					yield return exclusionContradictsMandatory;
				}
			}
			MandatoryConstraint coupledMandatory = ExclusiveOrMandatoryConstraint;
			if (coupledMandatory != null)
			{
				foreach (ModelErrorUsage usage in ((IModelErrorOwner)coupledMandatory).GetErrorCollection(filter))
				{
					yield return usage;
				}
			}
		}
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			return GetErrorCollection(filter);
		}
		#endregion

		#region IConstraint Implementation
		private static readonly IntersectingConstraintValidation[] myIntersectingValidationInfo = new IntersectingConstraintValidation[]
			{
				//Implication
				new IntersectingConstraintValidation(
					IntersectingConstraintPattern.SetComparisonConstraintSubset,
					IntersectingConstraintPatternOptions.IntersectingConstraintModalityNotWeaker,
					SetComparisonConstraintHasImplicationError.SetComparisonConstraintDomainRoleId,
					null,
					ConstraintType.Exclusion),

				//Implication
				new IntersectingConstraintValidation(
					IntersectingConstraintPattern.SetComparisonConstraintSuperset,
					IntersectingConstraintPatternOptions.IntersectingConstraintModalityNotStronger,
					SetComparisonConstraintHasImplicationError.SetComparisonConstraintDomainRoleId,
					null,
					ConstraintType.Exclusion),
				
				//Contradiction
				new IntersectingConstraintValidation(
					IntersectingConstraintPattern.SetComparisonConstraintSubset,
					IntersectingConstraintPatternOptions.IntersectingConstraintModalityIgnored,
					SetComparisonConstraintHasExclusionContradictsSubsetError.SetComparisonConstraintDomainRoleId,
					SetComparisonConstraintHasExclusionContradictsSubsetError.SetComparisonConstraintDomainRoleId,
					ConstraintType.Subset),
				
				//Contradiction
				new IntersectingConstraintValidation(
					IntersectingConstraintPattern.SetComparisonConstraintSubset,
					IntersectingConstraintPatternOptions.IntersectingConstraintModalityIgnored,
					SetComparisonConstraintHasExclusionContradictsEqualityError.SetComparisonConstraintDomainRoleId,
					SetComparisonConstraintHasExclusionContradictsEqualityError.SetComparisonConstraintDomainRoleId,
					ConstraintType.Equality),
				
				//TODO: handle disjunctive mandatory too!!!
				//Contradiction
				new IntersectingConstraintValidation(
					IntersectingConstraintPattern.ExclusionContradictsMandatory,
					IntersectingConstraintPatternOptions.IntersectingConstraintModalityIgnored,
					ExclusionConstraintHasExclusionContradictsMandatoryError.ExclusionConstraintDomainRoleId,
					MandatoryConstraintHasExclusionContradictsMandatoryError.MandatoryConstraintDomainRoleId,
					ConstraintType.SimpleMandatory),

				//TODO: this case is not currently handled
				////Bad ORM
				//new IntersectingConstraintValidation(
				//    IntersectingConstraintPattern.SetComparisonConstraintSuperset,
				//    EqualityIsSubsetOfExclusionError.ExclusionConstraintDomainRoleId,
				//    ConstraintType.Equality)
			};
		/// <summary>
		/// Implements <see cref="IConstraint.GetIntersectingConstraintValidationInfo"/>
		/// </summary>
		/// <returns></returns>
		protected new static IList<IntersectingConstraintValidation> GetIntersectingConstraintValidationInfo()
		{
			return myIntersectingValidationInfo;
		}
		IList<IntersectingConstraintValidation> IConstraint.GetIntersectingConstraintValidationInfo()
		{
			return GetIntersectingConstraintValidationInfo();
		}



		/// <summary>
		/// Implements IConstraint.ConstraintType. Returns ConstraintType.Exclusion.
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
	public partial class MandatoryConstraint : IConstraint
	{
		#region IConstraint Implementation
		private static readonly IntersectingConstraintValidation[] myIntersectingValidationInfo = new IntersectingConstraintValidation[]
			{
				//Implication
				new IntersectingConstraintValidation(
					IntersectingConstraintPattern.SetConstraintSubset,
					IntersectingConstraintPatternOptions.IntersectingConstraintModalityNotWeaker,
					SetConstraintHasImplicationError.SetConstraintDomainRoleId,
					null,
					ConstraintType.SimpleMandatory,
					ConstraintType.DisjunctiveMandatory),
				
				//Implication
				new IntersectingConstraintValidation(
					IntersectingConstraintPattern.SubsetImpliedByMandatory,
					IntersectingConstraintPatternOptions.None,
					null,
					SetComparisonConstraintHasEqualityOrSubsetImpliedByMandatoryError.SetComparisonConstraintDomainRoleId,
					ConstraintType.Subset),

				//Implication
				new IntersectingConstraintValidation(
					IntersectingConstraintPattern.EqualityImpliedByMandatory,
					IntersectingConstraintPatternOptions.None,
					null,
					SetComparisonConstraintHasEqualityOrSubsetImpliedByMandatoryError.SetComparisonConstraintDomainRoleId,
					ConstraintType.Equality),

				//Bad ORM
				new IntersectingConstraintValidation(
					IntersectingConstraintPattern.SubsetContradictsMandatory,
					IntersectingConstraintPatternOptions.None,
					MandatoryConstraintHasNotWellModeledSubsetAndMandatoryError.MandatoryConstraintDomainRoleId, // UNDONE: CONSTRAINTINTERSECTION: This should be a many relationship
					SubsetConstraintHasNotWellModeledSubsetAndMandatoryError.SubsetConstraintDomainRoleId,
					ConstraintType.Subset),

				//Contradiction
				new IntersectingConstraintValidation(
					IntersectingConstraintPattern.ExclusionContradictsMandatory,
					IntersectingConstraintPatternOptions.IntersectingConstraintModalityIgnored,
					MandatoryConstraintHasExclusionContradictsMandatoryError.MandatoryConstraintDomainRoleId, // UNDONE: CONSTRAINTINTERSECTION: Reverse many on this relationship, 1 error per contradiction
					ExclusionConstraintHasExclusionContradictsMandatoryError.ExclusionConstraintDomainRoleId, // UNDONE: CONSTRAINTINTERSECTION: Reverse many on this relationship, 1 error per contradiction
					ConstraintType.Exclusion),

				//TODO: Create error object for this case
				////Bad ORM
				//new IntersectingConstraintValidation(
				//    IntersectingConstraintPattern.NotWellModeledEqualityAndMandatory,
				//    SetComparisonConstraintHasEqualityOrSubsetImpliedByMandatoryError.SetComparisonConstraintDomainRoleId,
				//    ConstraintType.Equality)
			};
		/// <summary>
		/// Implements <see cref="IConstraint.GetIntersectingConstraintValidationInfo"/>
		/// </summary>
		/// <returns></returns>
		protected new static IList<IntersectingConstraintValidation> GetIntersectingConstraintValidationInfo()
		{
			return myIntersectingValidationInfo;
		}
		IList<IntersectingConstraintValidation> IConstraint.GetIntersectingConstraintValidationInfo()
		{
			return GetIntersectingConstraintValidationInfo();
		}
		/// <summary>
		/// Implements IConstraint.ConstraintType. Returns ConstraintType.SimpleMandatory or ConstraintType.DisjunctiveMandatory.
		/// </summary>
		protected ConstraintType ConstraintType
		{
			get
			{
				return IsSimple ? ConstraintType.SimpleMandatory : (IsImplied ? ConstraintType.ImpliedMandatory : ConstraintType.DisjunctiveMandatory);
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
		/// Implements IConstraint.ConstraintIsInternal
		/// </summary>
		protected new bool ConstraintIsInternal
		{
			get
			{
				return IsSimple;
			}
		}
		bool IConstraint.ConstraintIsInternal
		{
			get
			{
				return ConstraintIsInternal;
			}
		}
		/// <summary>
		/// Implements IConstraint.ConstraintIsImplied
		/// </summary>
		protected new bool ConstraintIsImplied
		{
			get
			{
				return IsImplied;
			}
		}
		bool IConstraint.ConstraintIsImplied
		{
			get
			{
				return ConstraintIsImplied;
			}
		}
		/// <summary>
		/// Implements IConstraint.RoleSequenceStyles.
		/// Returns {MultipleRowSequences, OneRolePerSequence, CompatibleColumns} for external constraint.
		/// Returns {OneRoleSequence, OneRolePerSequence} for internal constraint.
		/// </summary>
		protected RoleSequenceStyles RoleSequenceStyles
		{
			get
			{
				return IsSimple ?
					RoleSequenceStyles.OneRoleSequence | RoleSequenceStyles.OneRolePerSequence :
					IsImplied ?
						RoleSequenceStyles.OneOrMoreRoleSequences | RoleSequenceStyles.OneRolePerSequence : // Note that columns are compatible, but there isn't much use testing this separately on implied mandatories
						RoleSequenceStyles.MultipleRowSequences | RoleSequenceStyles.OneRolePerSequence | RoleSequenceStyles.CompatibleColumns;
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
	public partial class SubsetConstraint : IConstraint, IModelErrorOwner
	{
		#region IModelErrorOwner Implementation
		/// <summary>
		/// Implements IModelErrorOwner.GetErrorCollection
		/// </summary>
		protected new IEnumerable<ModelErrorUsage> GetErrorCollection(ModelErrorUses filter)
		{
			if (filter == 0)
			{
				filter = (ModelErrorUses)(-1);
			}
			foreach (ModelErrorUsage baseError in base.GetErrorCollection(filter))
			{
				yield return baseError;
			}
			if (0 != (filter & (ModelErrorUses.Verbalize | ModelErrorUses.DisplayPrimary)))
			{
				NotWellModeledSubsetAndMandatoryError notWellModeled = NotWellModeledSubsetAndMandatoryError;
				if (notWellModeled != null)
				{
					yield return notWellModeled;
				}
			}
		}
		IEnumerable<ModelErrorUsage> IModelErrorOwner.GetErrorCollection(ModelErrorUses filter)
		{
			return GetErrorCollection(filter);
		}
		#endregion

		#region IConstraint Implementation
		private static readonly IntersectingConstraintValidation[] myIntersectingValidationInfo = new IntersectingConstraintValidation[]
			{
				//TODO: these cases are not currently handled
				//Implication - if they are in the same direction (it will be bad ORM if they are in different
				//direction and should be handled differently)
				//new IntersectingConstraintValidation(
				//    IntersectingConstraintPattern.SetComparisonConstraintSubset,
				//    SetComparisonConstraintHasImplicationError.SetComparisonConstraintDomainRoleId,
				//    ConstraintType.Subset),

				////Implication - if they are in the same direction (it will be bad ORM if they are in different
				////direction and should be handled differently)
				//new IntersectingConstraintValidation(
				//    IntersectingConstraintPattern.SetComparisonConstraintSuperset,
				//    SetComparisonConstraintHasImplicationError.SetComparisonConstraintDomainRoleId,
				//    ConstraintType.Subset),
				
				//Contradiction
				new IntersectingConstraintValidation(
					IntersectingConstraintPattern.SetComparisonConstraintSuperset,
					IntersectingConstraintPatternOptions.None,
					SetComparisonConstraintHasExclusionContradictsSubsetError.SetComparisonConstraintDomainRoleId,
					SetComparisonConstraintHasExclusionContradictsSubsetError.SetComparisonConstraintDomainRoleId,
					ConstraintType.Exclusion),

				//TODO: handle disjunctive mandatory too!!!
				//Implication
				new IntersectingConstraintValidation(
					IntersectingConstraintPattern.SubsetImpliedByMandatory,
					IntersectingConstraintPatternOptions.None,
					SetComparisonConstraintHasEqualityOrSubsetImpliedByMandatoryError.SetComparisonConstraintDomainRoleId,
					null,
					ConstraintType.SimpleMandatory),

				//Bad ORM
				new IntersectingConstraintValidation(
					IntersectingConstraintPattern.SubsetContradictsMandatory,
					IntersectingConstraintPatternOptions.IntersectingConstraintModalityIgnored,
					SubsetConstraintHasNotWellModeledSubsetAndMandatoryError.SubsetConstraintDomainRoleId,
					MandatoryConstraintHasNotWellModeledSubsetAndMandatoryError.MandatoryConstraintDomainRoleId, // UNDONE: CONSTRAINTINTERSECTION: This should be a many relationship
					ConstraintType.SimpleMandatory),

				//This case is not currently implemented
				////Bad ORM
				//new IntersectingConstraintValidation(
				//    IntersectingConstraintPattern.SetComparisonConstraintSuperset,
				//    EqualityIsSubsetOfSubsetError.EqualityConstraintDomainRoleId,
				//    ConstraintType.Equality)
			};
		/// <summary>
		/// Implements <see cref="IConstraint.GetIntersectingConstraintValidationInfo"/>
		/// </summary>
		/// <returns></returns>
		protected new static IList<IntersectingConstraintValidation> GetIntersectingConstraintValidationInfo()
		{
			return myIntersectingValidationInfo;
		}
		IList<IntersectingConstraintValidation> IConstraint.GetIntersectingConstraintValidationInfo()
		{
			return GetIntersectingConstraintValidationInfo();
		}




		/// <summary>
		/// Implements IConstraint.ConstraintType. Returns ConstraintType.Subset.
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
