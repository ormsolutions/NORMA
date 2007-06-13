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
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.Modeling;
using System.Collections.ObjectModel;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;

namespace Neumont.Tools.ORM.ObjectModel
{
	public partial class SubtypeFact : IVerbalizeCustomChildren, IAnswerSurveyQuestion<SurveyQuestionGlyph>
	{
		#region Create functions
		/// <summary>
		/// Create a subtyping relationship between two objects
		/// </summary>
		/// <param name="subtype">The object type to use as the subtype</param>
		/// <param name="supertype">The object type to use as the supertype</param>
		/// <returns>Subtype object</returns>
		public static SubtypeFact Create(ObjectType subtype, ObjectType supertype)
		{
			Debug.Assert(subtype != null && supertype != null);
			SubtypeFact retVal = new SubtypeFact(subtype.Store);
			retVal.Model = subtype.Model;
			retVal.Subtype = subtype;
			retVal.Supertype = supertype;
			return retVal;
		}

		#endregion // Create functions
		#region Accessor functions
		/// <summary>
		/// Get the subtype for this relationship
		/// </summary>
		public ObjectType Subtype
		{
			get
			{
				Role role = SubtypeRole;
				return (role != null) ? role.RolePlayer : null;
			}
			set
			{
				SubtypeRole.RolePlayer = value;
			}
		}
		/// <summary>
		/// Get the super type for this relationship
		/// </summary>
		public ObjectType Supertype
		{
			get
			{
				Role role = SupertypeRole;
				return (role != null) ? role.RolePlayer : null;
			}
			set
			{
				SupertypeRole.RolePlayer = value;
			}
		}
		/// <summary>
		/// Get the Role attached to the subtype object
		/// </summary>
		public SubtypeMetaRole SubtypeRole
		{
			get
			{
				LinkedElementCollection<RoleBase> roles = RoleCollection;
				SubtypeMetaRole retVal = null;
				if (roles.Count == 2)
				{
					retVal = roles[0] as SubtypeMetaRole;
					if (retVal == null)
					{
						retVal = roles[1] as SubtypeMetaRole;
						Debug.Assert(retVal != null); // One of them better be a subtype
					}
				}
				return retVal;
			}
		}
		/// <summary>
		/// Get the Role attached to the supertype object
		/// </summary>
		public SupertypeMetaRole SupertypeRole
		{
			get
			{
				LinkedElementCollection<RoleBase> roles = RoleCollection;
				// Start with checking role 1, not 0. This corresponds
				// to the indices we set in the InitializeSubtypeAddRule.
				// This is not guaranteed (the user can switch them in the xml),
				// but will be the most common case, so we check it first.
				SupertypeMetaRole retVal = null;
				if (roles.Count == 2)
				{
					retVal = roles[1] as SupertypeMetaRole;
					if (retVal == null)
					{
						retVal = roles[0] as SupertypeMetaRole;
						Debug.Assert(retVal != null); // One of them better be a supertype
					}
				}
				return retVal;
			}
		}
		#endregion // Accessor functions
		#region Implicit Reading Support
		/// <summary>
		/// Indicate that a SubtypeFact always has implicit readings
		/// </summary>
		public override bool HasImplicitReadings
		{
			get
			{
				return true;
			}
		}
		/// <summary>
		/// Provide a generated element name for a subtype fact
		/// </summary>
		protected override string GenerateImplicitName()
		{
			return string.Format(CultureInfo.CurrentCulture, ResourceStrings.SubtypeFactElementNameFormat, Subtype.Name, Supertype.Name);
		}
		/// <summary>
		/// Provide an implicit reading for a subtype fact
		/// </summary>
		/// <param name="leadRole">The role that should begin the reading</param>
		/// <returns>An appropriate reading</returns>
		protected override IReading GetImplicitReading(RoleBase leadRole)
		{
			LinkedElementCollection<RoleBase> roles = RoleCollection;
			if (roles.Count == 2) // Sanity check
			{
				IList<RoleBase> order = null;
				if (roles[0] == leadRole)
				{
					order = roles;
				}
				else if (roles[1] == leadRole)
				{
					order = new RoleBase[] { leadRole, roles[0] };
				}
				if (order != null)
				{
					return new ImplicitReading(leadRole is SubtypeMetaRole ? ResourceStrings.SubtypeFactPredicateReading : ResourceStrings.SubtypeFactPredicateInverseReading, order);
				}
			}
			return null;
		}
		#endregion // Implicit Reading Support
		#region Initialize pattern rules
		/// <summary>
		/// A rule to create a subtype-style FactType with all
		/// of the required roles and constraints.
		/// </summary>
		[RuleOn(typeof(SubtypeFact))] // AddRule
		private sealed partial class InitializeSubtypeAddRule : AddRule
		{
			/// <summary>
			/// Make sure a Subtype is a 1-1 fact with a mandatory role
			/// on the base type (role 0)
			/// </summary>
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				FactType fact = e.ModelElement as FactType;
				Store store = fact.Store;
			
				// Establish role collecton
				LinkedElementCollection<RoleBase> roles = fact.RoleCollection;
				SubtypeMetaRole subTypeMetaRole = new SubtypeMetaRole(store);
				SupertypeMetaRole superTypeMetaRole = new SupertypeMetaRole(store);
				roles.Add(subTypeMetaRole);
				roles.Add(superTypeMetaRole);
			
				// Add injection constraints
				superTypeMetaRole.Multiplicity = RoleMultiplicity.ExactlyOne;
				subTypeMetaRole.Multiplicity = RoleMultiplicity.ZeroToOne;
			}
		}
		#endregion Initialize pattern rules
		#region Role and constraint pattern locking rules
		private static void ThrowPatternModifiedException()
		{
			throw new InvalidOperationException(ResourceStrings.ModelExceptionSubtypeConstraintAndRolePatternFixed);
		}
		private static void ThrowInvalidDisjunctiveMandatorySubtypeConstraint()
		{
			throw new InvalidOperationException(ResourceStrings.ModelExceptionSupertypeMetaRoleDisjunctiveMandatoryMustContainOnlySupertypeMetaRoles);
		}
		private static void ThrowInvalidExclusionSubtypeConstraint()
		{
			throw new InvalidOperationException(ResourceStrings.ModelExceptionSupertypeMetaRoleExclusionMustBeSingleColumnAndContainOnlySupertypeMetaRoles);
		}
		private static void ThrowInvalidSubtypeMetaRoleConstraint()
		{
			throw new InvalidOperationException(ResourceStrings.ModelExceptionSubtypeMetaRoleOnlyAllowsImplicitConstraints);
		}
		private static void ThrowInvalidSupertypeMetaRoleConstraint()
		{
			throw new InvalidOperationException(ResourceStrings.ModelExceptionSupertypeMetaRoleOnlyAllowsImplicitDisjunctiveMandatoryAndExclusionConstraints);
		}
		/// <summary>
		/// Block internal constraints from being added to a subtype
		/// after it is included in a model.
		/// </summary>
		[RuleOn(typeof(FactSetConstraint))] // AddRule
		private sealed partial class LimitSubtypeConstraintsAddRule : AddRule
		{
			/// <summary>
			/// Block internal constraint modification on subtypes
			/// </summary>
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				FactSetConstraint link = e.ModelElement as FactSetConstraint;
				if (link.SetConstraint.Constraint.ConstraintIsInternal)
				{
					SubtypeFact subtypeFact = link.FactType as SubtypeFact;
					if (subtypeFact != null)
					{
						if (subtypeFact.Model != null)
						{
							// Allow before adding to model, not afterwards
							ThrowPatternModifiedException();
						}
					}
				}
			}
		}
		/// <summary>
		/// Block internal constraints from being removed from a subtype
		/// after it is included in a model.
		/// </summary>
		[RuleOn(typeof(FactSetConstraint), FireTime = TimeToFire.LocalCommit, Priority = ORMCoreDomainModel.BeforeDelayValidateRulePriority)] // DeleteRule
		private sealed partial class LimitSubtypeConstraintsDeleteRule : DeleteRule
		{
			/// <summary>
			/// Block internal constraint modification on subtypes
			/// </summary>
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				FactSetConstraint link = e.ModelElement as FactSetConstraint;
				if (link.SetConstraint.Constraint.ConstraintIsInternal)
				{
					SubtypeFact subtypeFact = link.FactType as SubtypeFact;
					if (subtypeFact != null && !subtypeFact.IsDeleted)
					{
						if (subtypeFact.Model != null)
						{
							// Allow before adding to model, not afterwards
							ThrowPatternModifiedException();
						}
					}
				}
			}
		}
		/// <summary>
		/// Block roles from being added to a subtype
		/// after it is included in a model.
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole))] // AddRule
		private sealed partial class LimitSubtypeRolesAddRule : AddRule
		{
			/// <summary>
			/// Block internal constraint modification on subtypes
			/// </summary>
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
				SubtypeFact subtypeFact = link.FactType as SubtypeFact;
				if (subtypeFact != null)
				{
					if (subtypeFact.Model != null)
					{
						// Allow before adding to model, not afterwards
						ThrowPatternModifiedException();
					}
				}
				else
				{
					RoleBase role = link.Role;
					if (role is SubtypeMetaRole || role is SupertypeMetaRole)
					{
						throw new InvalidOperationException(ResourceStrings.ModelExceptionSubtypeFactMustBeParentOfMetaRole);
					}
				}
			}
		}
		/// <summary>
		/// Block roles from being removed from a subtype
		/// after it is included in a model.
		/// </summary>
		[RuleOn(typeof(FactTypeHasRole), FireTime = TimeToFire.LocalCommit, Priority = ORMCoreDomainModel.BeforeDelayValidateRulePriority)] // DeleteRule
		private sealed partial class LimitSubtypeRolesDeleteRule : DeleteRule
		{
			/// <summary>
			/// Block internal role modification on subtypes
			/// </summary>
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				FactTypeHasRole link = e.ModelElement as FactTypeHasRole;
				SubtypeFact subtypeFact = link.FactType as SubtypeFact;
				if (subtypeFact != null && !subtypeFact.IsDeleted)
				{
					if (subtypeFact.Model != null)
					{
						// Allow before adding to model, not afterwards
						ThrowPatternModifiedException();
					}
				}
			}
		}
		/// <summary>
		/// Block internal constraints from being modified on a subtype, block
		/// external constraints from being added to the subtype role, and
		/// limit external constraints being added to the supertype role
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole))] // AddRule
		private sealed partial class LimitSubtypeConstraintRolesAddRule : AddRule
		{
			/// <summary>
			/// Block internal constraint modification on subtypes
			/// </summary>
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				Role untypedRole = link.Role;
				SupertypeMetaRole supertypeRole = untypedRole as SupertypeMetaRole;
				SubtypeMetaRole subtypeRole = (supertypeRole == null) ? untypedRole as SubtypeMetaRole : null;
				ConstraintRoleSequence sequence = link.ConstraintRoleSequence;
				IConstraint constraint;
				if (supertypeRole != null || subtypeRole != null)
				{
					SetConstraint ic;
					SetComparisonConstraintRoleSequence externalSequence;
					bool invalidConstraintOnSubtypeRole = false;
					bool invalidConstraintOnSupertypeRole = false;
					if (null != (ic = sequence as SetConstraint))
					{
						constraint = ic.Constraint;
						if (constraint.ConstraintIsInternal)
						{
							SubtypeFact subtypeFact = untypedRole.FactType as SubtypeFact;
							if (subtypeFact != null && subtypeFact.Model != null)
							{
								// Allow before adding to model, not afterwards
								ThrowPatternModifiedException();
							}
						}
						else if (constraint.ConstraintType == ConstraintType.ImpliedMandatory)
						{
							// Nothing to do
						}
						else if (subtypeRole != null)
						{
							invalidConstraintOnSubtypeRole = true;
						}
						else if (constraint.ConstraintType == ConstraintType.DisjunctiveMandatory)
						{
							ORMCoreDomainModel.DelayValidateElement(ic, DelayValidateDisjunctiveMandatorySupertypeOnly);
						}
						else
						{
							invalidConstraintOnSupertypeRole = true;
						}
					}
					else if (subtypeRole != null)
					{
						invalidConstraintOnSubtypeRole = true;
					}
					else if (null != (externalSequence = sequence as SetComparisonConstraintRoleSequence))
					{
						constraint = externalSequence.Constraint;
						if (constraint != null)
						{
							if (constraint.ConstraintType == ConstraintType.Exclusion)
							{
								ORMCoreDomainModel.DelayValidateElement((ModelElement)constraint, DelayValidateExclusionSupertypeOnly);
							}
							else
							{
								invalidConstraintOnSupertypeRole = true;
							}
						}
					}
					if (invalidConstraintOnSupertypeRole)
					{
						ThrowInvalidSupertypeMetaRoleConstraint();
					}
					else if (invalidConstraintOnSubtypeRole)
					{
						ThrowInvalidSubtypeMetaRoleConstraint();
					}
				}
				else if (null != (constraint = sequence.Constraint))
				{
					switch (constraint.ConstraintType)
					{
						case ConstraintType.DisjunctiveMandatory:
							ORMCoreDomainModel.DelayValidateElement((ModelElement)constraint, DelayValidateDisjunctiveMandatorySupertypeOnly);
							break;
						case ConstraintType.Exclusion:
							ORMCoreDomainModel.DelayValidateElement((ModelElement)constraint, DelayValidateExclusionSupertypeOnly);
							break;
					}
				}
			}
		}
		/// <summary>
		/// Validator callback for mixed disjunctive mandatory constraint type exception
		/// </summary>
		private static void DelayValidateDisjunctiveMandatorySupertypeOnly(ModelElement element)
		{
			MandatoryConstraint constraint = (MandatoryConstraint)element;
			if (!constraint.IsDeleted)
			{
				LinkedElementCollection<Role> roles = constraint.RoleCollection;
				bool seenSupertypeMetaRole = false;
				int roleCount = roles.Count;
				for (int i = 0; i < roleCount; ++i)
				{
					if (roles[i] is SupertypeMetaRole)
					{
						if (i > 0 && !seenSupertypeMetaRole)
						{
							ThrowInvalidDisjunctiveMandatorySubtypeConstraint();
						}
						seenSupertypeMetaRole = true;
					}
					else if (seenSupertypeMetaRole)
					{
						ThrowInvalidDisjunctiveMandatorySubtypeConstraint();
					}
				}
			}
		}
		/// <summary>
		/// Validator callback for mixed exclusion constraint type exception
		/// </summary>
		private static void DelayValidateExclusionSupertypeOnly(ModelElement element)
		{
			ExclusionConstraint constraint = (ExclusionConstraint)element;
			if (!constraint.IsDeleted)
			{
				ReadOnlyCollection<FactConstraint> factConstraints = constraint.FactConstraintCollection;
				int factConstraintCount = factConstraints.Count;
				bool seenSupertypeMetaRole = false;
				for (int i = 0; i < factConstraintCount; ++i)
				{
					FactConstraint factConstraint = factConstraints[i];
					LinkedElementCollection<ConstraintRoleSequenceHasRole> roleLinks = factConstraint.ConstrainedRoleCollection;
					int roleLinkCount = roleLinks.Count;
					for (int j = 0; j < roleLinkCount; ++j)
					{
						if (roleLinks[j].Role is SupertypeMetaRole)
						{
							if (!seenSupertypeMetaRole && (i > 0 || j > 0))
							{
								ThrowInvalidExclusionSubtypeConstraint();
							}
							seenSupertypeMetaRole = true;
						}
						else if (seenSupertypeMetaRole)
						{
							ThrowInvalidExclusionSubtypeConstraint();
						}
					}
				}
				// Final check to enforce single column uniqueness only
				if (seenSupertypeMetaRole && factConstraintCount != constraint.RoleSequenceCollection.Count)
				{
					ThrowInvalidExclusionSubtypeConstraint();
				}
			}
		}
		/// <summary>
		/// Validate subtype external constraint patterns when a role sequence
		/// is added with preexisting roles
		/// </summary>
		[RuleOn(typeof(SetComparisonConstraintHasRoleSequence))] // AddRule
		private sealed partial class LimitSubtypeSetComparisonConstraintSequenceAddRule : AddRule
		{
			public override void ElementAdded(ElementAddedEventArgs e)
			{
				SetComparisonConstraintHasRoleSequence link = e.ModelElement as SetComparisonConstraintHasRoleSequence;
				SetComparisonConstraintRoleSequence sequence = link.RoleSequence;
				LinkedElementCollection<Role> roles = sequence.RoleCollection;
				int roleCount = roles.Count;
				if (roleCount != 0)
				{
					SetComparisonConstraint constraint = link.ExternalConstraint;
					bool isExclusion = constraint is ExclusionConstraint;
					bool addDelayValidate = false;
					for (int i = 0; i < roleCount; ++i)
					{
						Role untypedRole = roles[i];
						if (untypedRole is SubtypeMetaRole)
						{
							ThrowInvalidSubtypeMetaRoleConstraint();
						}
						else if (isExclusion)
						{
							addDelayValidate = true;
						}
						else if (untypedRole is SupertypeMetaRole)
						{
							ThrowInvalidSupertypeMetaRoleConstraint();
						}
					}
					if (addDelayValidate)
					{
						ORMCoreDomainModel.DelayValidateElement(constraint, DelayValidateExclusionSupertypeOnly);
					}
				}
			}
		}
		/// <summary>
		/// Block roles from being removed from subtype constraints
		/// after it is included in a model.
		/// </summary>
		[RuleOn(typeof(ConstraintRoleSequenceHasRole), FireTime = TimeToFire.LocalCommit, Priority = ORMCoreDomainModel.BeforeDelayValidateRulePriority)] // DeleteRule
		private sealed partial class LimitSubtypeConstraintRolesDeleteRule : DeleteRule
		{
			/// <summary>
			/// Block internal role modification on subtypes
			/// </summary>
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				ConstraintRoleSequenceHasRole link = e.ModelElement as ConstraintRoleSequenceHasRole;
				SetConstraint ic = link.ConstraintRoleSequence as SetConstraint;
				LinkedElementCollection<FactType> facts;
				if (ic != null &&
					!ic.IsDeleted &&
					ic.Constraint.ConstraintIsInternal &&
					1 == (facts = ic.FactTypeCollection).Count)
				{
					SubtypeFact subtypeFact = facts[0] as SubtypeFact;
					if (subtypeFact != null && !subtypeFact.IsDeleted)
					{
						if (subtypeFact.Model != null)
						{
							// Allow before adding to model, not afterwards
							ThrowPatternModifiedException();
						}
					}
				}
			}
		}
		/// <summary>
		/// Block the modality from being changed on internal constraints
		/// on subtype facts
		/// </summary>
		[RuleOn(typeof(SetConstraint))] // ChangeRule
		private sealed partial class LimitSubtypeConstraintChangeRule : ChangeRule
		{
			/// <summary>
			/// Block internal property modification on implicit subtype constraints
			/// </summary>
			public sealed override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
			{
				Guid attributeId = e.DomainProperty.Id;
				SetConstraint constraint = e.ModelElement as SetConstraint;
				if (!constraint.IsDeleted)
				{
					LinkedElementCollection<FactType> testFacts = null;
					if (attributeId == UniquenessConstraint.IsInternalDomainPropertyId ||
						attributeId == MandatoryConstraint.IsSimpleDomainPropertyId)
					{
						testFacts = constraint.FactTypeCollection;
					}
					else if (attributeId == SetConstraint.ModalityDomainPropertyId)
					{
						if (constraint.Constraint.ConstraintIsInternal)
						{
							testFacts = constraint.FactTypeCollection;
						}
					}
					if (testFacts != null)
					{
						int testFactsCount = testFacts.Count;
						for (int i = 0; i < testFactsCount; ++i)
						{
							if (testFacts[i] is SubtypeFact)
							{
								// We never do this internally, so block any modification,
								// not just those after the subtype fact is added to the model
								ThrowPatternModifiedException();
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// Ensure that a role player deletion on a subtype results in a deletion
		/// of the subtype itself.
		/// </summary>
		[RuleOn(typeof(ObjectTypePlaysRole), FireTime = TimeToFire.LocalCommit, Priority = ORMCoreDomainModel.BeforeDelayValidateRulePriority)] // DeleteRule
		private sealed partial class DeleteSubtypeWhenRolePlayerDeleted : DeleteRule
		{
			/// <summary>
			/// Remove the full SubtypeFact when a role player is removed
			/// </summary>
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				Role role = link.PlayedRole;
				if (role != null && !role.IsDeleted)
				{
					SubtypeFact subtypeFact = role.FactType as SubtypeFact;
					if (subtypeFact != null && !subtypeFact.IsDeleted)
					{
						subtypeFact.Delete();
					}
				}
			}
		}
		#endregion // Role and constraint pattern locking rules
		#region Mixed role player types rules
		private static void ThrowMixedRolePlayerTypesException()
		{
			throw new InvalidOperationException(ResourceStrings.ModelExceptionSubtypeRolePlayerTypesCannotBeMixed);
		}
		/// <summary>
		/// Ensure consistent types (EntityType or ValueType) for role
		/// players in a subtyping relationship
		/// </summary>
		[RuleOn(typeof(ObjectTypePlaysRole), FireTime = TimeToFire.LocalCommit, Priority = ORMCoreDomainModel.BeforeDelayValidateRulePriority)] // AddRule
		private sealed partial class EnsureConsistentRolePlayerTypesAddRule : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ObjectTypePlaysRole link = e.ModelElement as ObjectTypePlaysRole;
				SubtypeMetaRole subtypeRole;
				SubtypeFact subtypeFact;
				if (null != (subtypeRole = link.PlayedRole as SubtypeMetaRole) &&
					null != (subtypeFact = subtypeRole.FactType as SubtypeFact))
				{
					ObjectType superType = subtypeFact.Supertype;
					if (null == superType ||
						((superType.DataType == null) != (link.RolePlayer.DataType == null)))
					{
						ThrowMixedRolePlayerTypesException();
					}
				}
			}
		}
		/// <summary>
		/// Stop the ValueTypeHasDataType relationship from being
		/// added if an ObjectType participates in a subtyping relationship
		/// </summary>
		[RuleOn(typeof(ValueTypeHasDataType))] // AddRule
		private sealed partial class EnsureConsistentDataTypesAddRule : AddRule
		{
			public sealed override void ElementAdded(ElementAddedEventArgs e)
			{
				ValueTypeHasDataType link = e.ModelElement as ValueTypeHasDataType;
				ObjectType objectType = link.ValueType;
				LinkedElementCollection<Role> playedRoles = objectType.PlayedRoleCollection;
				int playedRoleCount = playedRoles.Count;
				for (int i = 0; i < playedRoleCount; ++i)
				{
					Role testRole = playedRoles[i];
					if (testRole is SubtypeMetaRole ||
						testRole is SupertypeMetaRole)
					{
						if (null != testRole.FactType)
						{
							ThrowMixedRolePlayerTypesException();
						}
					}
				}
			}
		}
		/// <summary>
		/// Stop the ValueTypeHasDataType relationship from being
		/// removed if an ObjectType participates in a subtyping relationship
		/// </summary>
		[RuleOn(typeof(ValueTypeHasDataType))] // DeleteRule
		private sealed partial class EnsureConsistentDataTypesDeleteRule : DeleteRule
		{
			public sealed override void ElementDeleted(ElementDeletedEventArgs e)
			{
				ValueTypeHasDataType link = e.ModelElement as ValueTypeHasDataType;
				ObjectType objectType = link.ValueType;
				if (!objectType.IsDeleted)
				{
					LinkedElementCollection<Role> playedRoles = objectType.PlayedRoleCollection;
					int playedRoleCount = playedRoles.Count;
					for (int i = 0; i < playedRoleCount; ++i)
					{
						Role testRole = playedRoles[i];
						if (testRole is SubtypeMetaRole ||
							testRole is SupertypeMetaRole)
						{
							if (null != testRole.FactType)
							{
								ThrowMixedRolePlayerTypesException();
							}
						}
					}
				}
			}
		}
		#endregion // Mixed role player types rules
		#region Deserialization Fixup
		/// <summary>
		/// Return a deserialization fixup listener. The listener
		/// validates all model errors and adds errors to the task provider.
		/// </summary>
		public static IDeserializationFixupListener FixupListener
		{
			get
			{
				return new SubtypeFactFixupListener();
			}
		}
		/// <summary>
		/// A listener class to enforce valid subtype facts on load.
		/// Invalid subtype patterns will either be fixed up or completely
		/// removed.
		/// </summary>
		private sealed class SubtypeFactFixupListener : DeserializationFixupListener<SubtypeFact>
		{
			/// <summary>
			/// Create a new SubtypeFactFixupListener
			/// </summary>
			public SubtypeFactFixupListener()
				: base((int)ORMDeserializationFixupPhase.ValidateImplicitStoredElements)
			{
			}
			/// <summary>
			/// Make sure the subtype fact constraint pattern
			/// and object types are appropriate.
			/// </summary>
			/// <param name="element">An SubtypeFact instance</param>
			/// <param name="store">The context store</param>
			/// <param name="notifyAdded">The listener to notify if elements are added during fixup</param>
			protected sealed override void ProcessElement(SubtypeFact element, Store store, INotifyElementAdded notifyAdded)
			{
				// Note that the arity and types of the subtype/supertype roles are
				// enforced by the schema.
				Role superTypeMetaRole;
				Role subTypeMetaRole;
				ObjectType superType;
				ObjectType subType;
				if (null == (superTypeMetaRole = element.SupertypeRole) ||
					null == (subTypeMetaRole = element.SubtypeRole) ||
					null == (superType = superTypeMetaRole.RolePlayer) ||
					null == (subType = subTypeMetaRole.RolePlayer) ||
					// They must both be value types or object types, but can't switch
					((superType.DataType == null) != (subType.DataType == null)))
				{
					RemoveFact(element);
				}
				else
				{
					// UNDONE: We can still save explicit readings, but we won't
					// be able to load these in later file formats. Get rid of any serialized readings.
					element.ReadingOrderCollection.Clear();

					// Note that rules aren't on, so we can read the Multiplicity properties,
					// but we can't set them. All changes must be made explicitly.
					if (superTypeMetaRole.Multiplicity != RoleMultiplicity.ExactlyOne)
					{
						EnsureSingleColumnUniqueAndMandatory(store, element.Model, subTypeMetaRole, true, notifyAdded);
					}
					if (subTypeMetaRole.Multiplicity != RoleMultiplicity.ZeroToOne)
					{
						EnsureSingleColumnUniqueAndMandatory(store, element.Model, superTypeMetaRole, false, notifyAdded);
					}
				}
			}
			/// <summary>
			/// Internal constraints are not fully connected at this point (FactSetConstraint instances
			/// are not implicitly constructed until a later phase), so we need to work a little harder
			/// to remove them.
			/// </summary>
			/// <param name="fact">The fact to clear of external constraints</param>
			private static void RemoveFact(FactType fact)
			{
				LinkedElementCollection<RoleBase> factRoles = fact.RoleCollection;
				int roleCount = factRoles.Count;
				for (int i = 0; i < roleCount; ++i)
				{
					Role role = factRoles[i].Role;
					LinkedElementCollection<ConstraintRoleSequence> sequences = role.ConstraintRoleSequenceCollection;
					int sequenceCount = sequences.Count;
					for (int j = sequenceCount - 1; j >= 0; --j)
					{
						SetConstraint ic = sequences[j] as SetConstraint;
						if (ic != null && ic.Constraint.ConstraintIsInternal)
						{
							ic.Delete();
						}
					}
				}
				fact.Delete();
			}
			private static void EnsureSingleColumnUniqueAndMandatory(Store store, ORMModel model, Role role, bool requireMandatory, INotifyElementAdded notifyAdded)
			{
				LinkedElementCollection<ConstraintRoleSequence> sequences = role.ConstraintRoleSequenceCollection;
				int sequenceCount = sequences.Count;
				bool haveUniqueness = false;
				bool haveMandatory = !requireMandatory;
				SetConstraint ic;
				for (int i = sequenceCount - 1; i >= 0; --i)
				{
					ic = sequences[i] as SetConstraint;
					if (ic != null && ic.Constraint.ConstraintIsInternal)
					{
						if (ic.RoleCollection.Count == 1 && ic.Modality == ConstraintModality.Alethic)
						{
							switch (ic.Constraint.ConstraintType)
							{
								case ConstraintType.InternalUniqueness:
									if (haveUniqueness)
									{
										ic.Delete();
									}
									else
									{
										haveUniqueness = true;
									}
									break;
								case ConstraintType.SimpleMandatory:
									if (haveMandatory)
									{
										ic.Delete();
									}
									else
									{
										haveMandatory = true;
									}
									break;
							}
						}
						else
						{
							ic.Delete();
						}
					}
				}
				if (!haveUniqueness)
				{
					ic = UniquenessConstraint.CreateInternalUniquenessConstraint(store);
					ic.RoleCollection.Add(role);
					ic.Model = model;
					notifyAdded.ElementAdded(ic, true);
				}
				if (!haveMandatory)
				{
					ic = MandatoryConstraint.CreateSimpleMandatoryConstraint(store);
					ic.RoleCollection.Add(role);
					ic.Model = model;
					notifyAdded.ElementAdded(ic, true);
				}
			}
		}
		#endregion Deserialization Fixup
		#region IVerbalizeCustomChildren Implementation
		/// <summary>
		/// Implements IVerbalizeCustomChildren.GetCustomChildVerbalizations. Hides
		/// implementation in <see cref="FactType"/>
		/// </summary>
		protected static new IEnumerable<CustomChildVerbalizer> GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, bool isNegative)
		{
			yield break;
		}
		IEnumerable<CustomChildVerbalizer> IVerbalizeCustomChildren.GetCustomChildVerbalizations(IVerbalizeFilterChildren filter, bool isNegative)
		{
			return GetCustomChildVerbalizations(filter, isNegative);
		}
		#endregion // IVerbalizeCustomChildren Implementation
		#region IAnswerSurveyQuestion<SurveyQuestionGlyph> implementation
		int IAnswerSurveyQuestion<SurveyQuestionGlyph>.AskQuestion()
		{
			return AskGlyphQuestion();
		}
		/// <summary>
		/// returns answer to IAnswerSurveyQuestion for glyphs
		/// </summary>
		/// <returns></returns>
		protected new int AskGlyphQuestion()
		{
			if (this.IsPrimary)
			{
				return (int)SurveyQuestionGlyph.PrimarySubtypeRelationship;
			}
			else
			{
				return (int)SurveyQuestionGlyph.SecondarySubtypeRelationship;
			}
		}
		#endregion // IAnswerSurveyQuestion<SurveyQuestionGlyph> implementation
	}
}
