using System;
using System.Reflection;

// Common Public License Copyright Notice
// /**************************************************************************\
// * Neumont Object-Role Modeling Architect for Visual Studio                 *
// *                                                                          *
// * Copyright © Neumont University. All rights reserved.                     *
// *                                                                          *
// * The use and distribution terms for this software are covered by the      *
// * Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
// * can be found in the file CPL.txt at the root of this distribution.       *
// * By using this software in any fashion, you are agreeing to be bound by   *
// * the terms of this license.                                               *
// *                                                                          *
// * You must not remove this notice, or any other, from this software.       *
// \**************************************************************************/

namespace Neumont.Tools.ORM.ObjectModel
{
	#region Attach rules to ORMCoreDomainModel model
	public partial class ORMCoreDomainModel : Neumont.Tools.ORM.ObjectModel.IDomainModelEnablesRulesAfterDeserialization
	{
		private static Type[] myCustomDomainModelTypes;
		private static Type[] CustomDomainModelTypes
		{
			get
			{
				Type[] retVal = ORMCoreDomainModel.myCustomDomainModelTypes;
				if (retVal == null)
				{
					// No synchronization is needed here.
					// If accessed concurrently, the worst that will happen is the array of Types being created multiple times.
					// This would have a slightly negative impact on performance, but the result would still be correct.
					// Given the low likelihood of this ever happening, the extra overhead of synchronization would outweigh any possible gain from it.
					retVal = new Type[]{
						typeof(ConstraintUtility).GetNestedType("ConstraintRoleSequenceHasRoleDeleted", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(CustomReferenceMode).GetNestedType("CustomReferenceModeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("PreferredIdentifierAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("TestRemovePreferredIdentifierConstraintRoleAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("TestRemovePreferredIdentifierObjectificationAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("TestRemovePreferredIdentifierObjectificationRolePlayerChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("TestRemovePreferredIdentifierRoleAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("TestRemovePreferredIdentifierDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("TestRemovePreferredIdentifierRolePlayerChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("ConstraintRoleSequenceHasRoleAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("ConstraintRoleSequenceHasRoleDeleted", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("EntityTypeHasEntityTypeInstanceAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("EntityTypeInstanceHasRoleInstanceAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("EntityTypeInstanceHasRoleInstanceDeleted", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EqualityConstraint).GetNestedType("ConstraintRoleSequenceHasRoleAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EqualityConstraint).GetNestedType("ConstraintRoleSequenceHasRoleDeleting", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("FactTypeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("FactTypeHasReadingOrderAddRuleModelValidation", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("FactTypeHasReadingOrderDeleteRuleModelValidation", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("FactTypeHasRoleAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("FactTypeHasRoleDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("InternalConstraintCollectionHasConstraintAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("InternalConstraintCollectionHasConstraintDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("InternalUniquenessConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ModelHasFactTypeAddRuleModelValidation", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ModelHasInternalConstraintAddRuleModelValidation", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ModelHasInternalConstraintDeleteRuleModelValidation", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ReadingOrderHasReadingAddRuleModelValidation", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ReadingOrderHasReadingDeleteRuleModelValidation", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ValidateFactNameForObjectTypeNameChange", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ValidateFactNameForReadingChange", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ValidateFactNameForReadingOrderReorder", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ValidateFactNameForReadingReorder", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ValidateFactNameForRolePlayerAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ValidateFactNameForRolePlayerDelete", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ValidateFactNameForRolePlayerRolePlayerChange", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeDerivationExpression).GetNestedType("FactTypeDerivationExpressionChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeInstance).GetNestedType("FactTypeHasRoleAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeInstance).GetNestedType("FactTypeHasRoleDeleted", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeInstance).GetNestedType("FactTypeHasFactTypeInstanceAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeInstance).GetNestedType("FactTypeInstanceHasRoleInstanceAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeInstance).GetNestedType("FactTypeInstanceHasRoleInstanceDeleted", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FrequencyConstraint).GetNestedType("FrequencyConstraintMinMaxRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FrequencyConstraint).GetNestedType("RemoveContradictionErrorsWithFactTypeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(MandatoryConstraint).GetNestedType("MandatoryConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(MandatoryConstraint).GetNestedType("VerifyImpliedMandatoryRoleAdd", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(MandatoryConstraint).GetNestedType("VerifyImpliedMandatoryRoleDeleting", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModelError).GetNestedType("SynchronizeErrorForOwnerRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModelError).GetNestedType("SynchronizeErrorTextForModelRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Neumont.Tools.Modeling.NamedElementDictionary).GetNestedType("ElementLinkAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Neumont.Tools.Modeling.NamedElementDictionary).GetNestedType("ElementLinkDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Neumont.Tools.Modeling.NamedElementDictionary).GetNestedType("NamedElementChangedRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Note).GetNestedType("NoteChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ImpliedFactTypeAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ImpliedObjectificationConstraintRoleSequenceHasRoleAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ImpliedObjectificationConstraintRoleSequenceHasRoleDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ImpliedObjectificationFactTypeHasRoleAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ImpliedObjectificationFactTypeHasRoleDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ImpliedObjectificationIsImpliedChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ImpliedObjectificationObjectifyingTypeIsIndependentChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ImpliedObjectificationObjectifyingTypePlaysRoleAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ImpliedObjectificationUniquenessConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("InternalConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ObjectificationAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ObjectificationDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ObjectificationRolePlayerChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("PreferredIdentifierDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("RoleAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("RolePlayerAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("RoleDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("RolePlayerDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("UniquenessConstraintAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("UniquenessConstraintDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("CheckForIncompatibleRelationshipRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("CheckForIncompatibleRelationshipRolePlayerChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("MandatoryRoleAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("MandatoryRoleDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("ModelHasObjectTypeAddRuleModelValidation", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("ObjectTypeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("ObjectTypeDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("SubtypeFactChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("SupertypeAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("SupertypeDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("SupertypeDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("UnspecifiedDataTypeAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("UnspecifiedDataRoleRolePlayerChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("VerifyReferenceSchemeAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("VerifyReferenceSchemeDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("VerifyValueTypeHasDataTypeAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("VerifyValueTypeHasDataTypeDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(DelayValidateElements),
						typeof(ORMModel).GetNestedType("RemoveDuplicateConstraintNameErrorRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ORMModel).GetNestedType("RemoveDuplicateObjectTypeNameErrorRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Reading).GetNestedType("ReadingOrderHasRoleDeleted", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Reading).GetNestedType("ReadingPropertiesChanged", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingOrder).GetNestedType("EnforceNoEmptyReadingOrder", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingOrder).GetNestedType("FactTypeHasRoleAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingOrder).GetNestedType("ReadingOrderHasRoleDeleting", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReferenceMode).GetNestedType("ReferenceModeAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReferenceMode).GetNestedType("ReferenceModeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReferenceModeHasReferenceModeKind).GetNestedType("ReferenceModeHasReferenceModeKindChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReferenceModeHasReferenceModeKind).GetNestedType("ReferenceModeHasReferenceModeKindDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReferenceModeKind).GetNestedType("ReferenceModeKindChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RingConstraint).GetNestedType("RingConstraintTypeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RoleChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RolePlayerRequiredAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RolePlayerRequiredForNewRoleAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RolePlayerRequiredDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("UpdatedRolePlayerRequiredErrorsDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("ConstraintHasRoleSequenceAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("ConstraintRoleSequenceHasRoleAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("EnforceRoleSequenceCardinalityForAdd", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("EnforceRoleSequenceCardinalityForConstraintAdd", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("EnforceRoleSequenceCardinalityForDelete", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("EnforceRoleSequenceValidityForAdd", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("EnforceRoleSequenceValidityForDelete", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("EnforceRoleSequenceValidityForReorder", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("EnforceRoleSequenceValidityForRolePlayerAdd", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("EnforceRoleSequenceValidityForRolePlayerDelete", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("EnforceRoleSequenceValidityForRolePlayerRolePlayerChange", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("ExternalRoleConstraintDeleted", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("FactAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("FactSetComparisonConstraintAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("ConstraintAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("ConstraintRoleSequenceHasRoleAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("EnforceRoleSequenceValidityForAdd", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("EnforceRoleSequenceValidityForDelete", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("EnforceRoleSequenceValidityForRolePlayerAdd", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("EnforceRoleSequenceValidityForRolePlayerDelete", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("EnforceRoleSequenceValidityForRolePlayerRolePlayerChange", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("FactAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("FactSetConstraintAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("FactSetConstraintDeleted", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("EnsureConsistentDataTypesAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("EnsureConsistentDataTypesDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("EnsureConsistentRolePlayerTypesAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("InitializeSubtypeAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("LimitSubtypeConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("LimitSubtypeConstraintRolesAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("LimitSubtypeConstraintRolesDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("LimitSubtypeConstraintsAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("LimitSubtypeConstraintsDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("LimitSubtypeRolesAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("LimitSubtypeRolesDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("DeleteSubtypeWhenRolePlayerDeleted", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(UniquenessConstraint).GetNestedType("NMinusOneAddRuleModelConstraintAddValidation", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(UniquenessConstraint).GetNestedType("NMinusOneAddRuleModelFactAddValidation", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(UniquenessConstraint).GetNestedType("NMinusOneAddRuleModelValidation", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(UniquenessConstraint).GetNestedType("NMinusOneDeleteRuleModelConstraintDeleteValidation", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(UniquenessConstraint).GetNestedType("NMinusOneDeleteRuleModelFactDeleteValidation", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(UniquenessConstraint).GetNestedType("UniquenessConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("DataTypeAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("DataTypeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("DataTypeRolePlayerChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("ObjectTypeRoleAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("RoleValueConstraintAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("ValueConstraintAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("ValueConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("ValueRangeAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("ValueRangeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueRange).GetNestedType("ValueRangeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueTypeInstance).GetNestedType("ValueTypeHasDataTypeAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueTypeInstance).GetNestedType("ValueTypeHasDataTypeRolePlayerChange", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueTypeInstance).GetNestedType("ValueTypeInstanceValueChanged", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueTypeInstance).GetNestedType("ValueTypeHasValueTypeInstanceAdded", BindingFlags.Public | BindingFlags.NonPublic)};
					ORMCoreDomainModel.myCustomDomainModelTypes = retVal;
					System.Diagnostics.Debug.Assert(Array.IndexOf<Type>(retVal, null) < 0, "One or more rule types failed to resolve. The file and/or package will fail to load.");
				}
				return retVal;
			}
		}
		/// <summary>
		/// Generated code to attach <see cref="Microsoft.VisualStudio.Modeling.Rule"/>s to the <see cref="Microsoft.VisualStudio.Modeling.Store"/>.
		/// </summary>
		/// <seealso cref="Microsoft.VisualStudio.Modeling.DomainModel.GetCustomDomainModelTypes">
		/// 
		/// </seealso>
		protected override Type[] GetCustomDomainModelTypes()
		{
			if (Neumont.Tools.ORM.ObjectModel.ORMCoreDomainModel.InitializingToolboxItems)
			{
				return Type.EmptyTypes;
			}
			Type[] retVal = base.GetCustomDomainModelTypes();
			int baseLength = retVal.Length;
			Type[] customDomainModelTypes = ORMCoreDomainModel.CustomDomainModelTypes;
			if (baseLength <= 0)
			{
				return customDomainModelTypes;
			}
			else
			{
				Array.Resize<Type>(ref retVal, baseLength + customDomainModelTypes.Length);
				customDomainModelTypes.CopyTo(retVal, baseLength);
				return retVal;
			}
		}
		/// <summary>
		/// Implements IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization
		/// </summary>
		protected void EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.RuleManager ruleManager)
		{
			Type[] disabledRuleTypes = ORMCoreDomainModel.CustomDomainModelTypes;
			int count = disabledRuleTypes.Length;
			for (int i = 0; i < count; ++i)
			{
				ruleManager.EnableRule(disabledRuleTypes[i]);
			}
		}
		void Neumont.Tools.ORM.ObjectModel.IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.RuleManager ruleManager)
		{
			this.EnableRulesAfterDeserialization(ruleManager);
		}
	}
	#endregion // Attach rules to ORMCoreDomainModel model
	#region Initially disable rules
	public partial class ConstraintUtility
	{
		private partial class ConstraintRoleSequenceHasRoleDeleted
		{
			public ConstraintRoleSequenceHasRoleDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class CustomReferenceMode
	{
		private partial class CustomReferenceModeChangeRule
		{
			public CustomReferenceModeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class EntityTypeHasPreferredIdentifier
	{
		private partial class PreferredIdentifierAddedRule
		{
			public PreferredIdentifierAddedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class EntityTypeHasPreferredIdentifier
	{
		private partial class TestRemovePreferredIdentifierConstraintRoleAddRule
		{
			public TestRemovePreferredIdentifierConstraintRoleAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class EntityTypeHasPreferredIdentifier
	{
		private partial class TestRemovePreferredIdentifierObjectificationAddRule
		{
			public TestRemovePreferredIdentifierObjectificationAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class EntityTypeHasPreferredIdentifier
	{
		private partial class TestRemovePreferredIdentifierObjectificationRolePlayerChangeRule
		{
			public TestRemovePreferredIdentifierObjectificationRolePlayerChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class EntityTypeHasPreferredIdentifier
	{
		private partial class TestRemovePreferredIdentifierRoleAddRule
		{
			public TestRemovePreferredIdentifierRoleAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class EntityTypeHasPreferredIdentifier
	{
		private partial class TestRemovePreferredIdentifierDeletingRule
		{
			public TestRemovePreferredIdentifierDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class EntityTypeHasPreferredIdentifier
	{
		private partial class TestRemovePreferredIdentifierRolePlayerChangeRule
		{
			public TestRemovePreferredIdentifierRolePlayerChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class EntityTypeInstance
	{
		private partial class ConstraintRoleSequenceHasRoleAdded
		{
			public ConstraintRoleSequenceHasRoleAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class EntityTypeInstance
	{
		private partial class ConstraintRoleSequenceHasRoleDeleted
		{
			public ConstraintRoleSequenceHasRoleDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class EntityTypeInstance
	{
		private partial class EntityTypeHasEntityTypeInstanceAdded
		{
			public EntityTypeHasEntityTypeInstanceAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class EntityTypeInstance
	{
		private partial class EntityTypeInstanceHasRoleInstanceAdded
		{
			public EntityTypeInstanceHasRoleInstanceAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class EntityTypeInstance
	{
		private partial class EntityTypeInstanceHasRoleInstanceDeleted
		{
			public EntityTypeInstanceHasRoleInstanceDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class EqualityConstraint
	{
		private partial class ConstraintRoleSequenceHasRoleAdded
		{
			public ConstraintRoleSequenceHasRoleAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class EqualityConstraint
	{
		private partial class ConstraintRoleSequenceHasRoleDeleting
		{
			public ConstraintRoleSequenceHasRoleDeleting()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactType
	{
		private partial class FactTypeChangeRule
		{
			public FactTypeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactType
	{
		private partial class FactTypeHasReadingOrderAddRuleModelValidation
		{
			public FactTypeHasReadingOrderAddRuleModelValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactType
	{
		private partial class FactTypeHasReadingOrderDeleteRuleModelValidation
		{
			public FactTypeHasReadingOrderDeleteRuleModelValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactType
	{
		private partial class FactTypeHasRoleAddRule
		{
			public FactTypeHasRoleAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactType
	{
		private partial class FactTypeHasRoleDeleteRule
		{
			public FactTypeHasRoleDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactType
	{
		private partial class InternalConstraintCollectionHasConstraintAddedRule
		{
			public InternalConstraintCollectionHasConstraintAddedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactType
	{
		private partial class InternalConstraintCollectionHasConstraintDeleteRule
		{
			public InternalConstraintCollectionHasConstraintDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactType
	{
		private partial class InternalUniquenessConstraintChangeRule
		{
			public InternalUniquenessConstraintChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactType
	{
		private partial class ModelHasFactTypeAddRuleModelValidation
		{
			public ModelHasFactTypeAddRuleModelValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactType
	{
		private partial class ModelHasInternalConstraintAddRuleModelValidation
		{
			public ModelHasInternalConstraintAddRuleModelValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactType
	{
		private partial class ModelHasInternalConstraintDeleteRuleModelValidation
		{
			public ModelHasInternalConstraintDeleteRuleModelValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactType
	{
		private partial class ReadingOrderHasReadingAddRuleModelValidation
		{
			public ReadingOrderHasReadingAddRuleModelValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactType
	{
		private partial class ReadingOrderHasReadingDeleteRuleModelValidation
		{
			public ReadingOrderHasReadingDeleteRuleModelValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactType
	{
		private partial class ValidateFactNameForObjectTypeNameChange
		{
			public ValidateFactNameForObjectTypeNameChange()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactType
	{
		private partial class ValidateFactNameForReadingChange
		{
			public ValidateFactNameForReadingChange()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactType
	{
		private partial class ValidateFactNameForReadingOrderReorder
		{
			public ValidateFactNameForReadingOrderReorder()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactType
	{
		private partial class ValidateFactNameForReadingReorder
		{
			public ValidateFactNameForReadingReorder()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactType
	{
		private partial class ValidateFactNameForRolePlayerAdded
		{
			public ValidateFactNameForRolePlayerAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactType
	{
		private partial class ValidateFactNameForRolePlayerDelete
		{
			public ValidateFactNameForRolePlayerDelete()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactType
	{
		private partial class ValidateFactNameForRolePlayerRolePlayerChange
		{
			public ValidateFactNameForRolePlayerRolePlayerChange()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactTypeDerivationExpression
	{
		private partial class FactTypeDerivationExpressionChangeRule
		{
			public FactTypeDerivationExpressionChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactTypeInstance
	{
		private partial class FactTypeHasRoleAdded
		{
			public FactTypeHasRoleAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactTypeInstance
	{
		private partial class FactTypeHasRoleDeleted
		{
			public FactTypeHasRoleDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactTypeInstance
	{
		private partial class FactTypeHasFactTypeInstanceAdded
		{
			public FactTypeHasFactTypeInstanceAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactTypeInstance
	{
		private partial class FactTypeInstanceHasRoleInstanceAdded
		{
			public FactTypeInstanceHasRoleInstanceAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactTypeInstance
	{
		private partial class FactTypeInstanceHasRoleInstanceDeleted
		{
			public FactTypeInstanceHasRoleInstanceDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FrequencyConstraint
	{
		private partial class FrequencyConstraintMinMaxRule
		{
			public FrequencyConstraintMinMaxRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FrequencyConstraint
	{
		private partial class RemoveContradictionErrorsWithFactTypeRule
		{
			public RemoveContradictionErrorsWithFactTypeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class MandatoryConstraint
	{
		private partial class MandatoryConstraintChangeRule
		{
			public MandatoryConstraintChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class MandatoryConstraint
	{
		private partial class VerifyImpliedMandatoryRoleAdd
		{
			public VerifyImpliedMandatoryRoleAdd()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class MandatoryConstraint
	{
		private partial class VerifyImpliedMandatoryRoleDeleting
		{
			public VerifyImpliedMandatoryRoleDeleting()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ModelError
	{
		private partial class SynchronizeErrorForOwnerRule
		{
			public SynchronizeErrorForOwnerRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ModelError
	{
		private partial class SynchronizeErrorTextForModelRule
		{
			public SynchronizeErrorTextForModelRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Note
	{
		private partial class NoteChangeRule
		{
			public NoteChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Objectification
	{
		private partial class ImpliedFactTypeAddRule
		{
			public ImpliedFactTypeAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Objectification
	{
		private partial class ImpliedObjectificationConstraintRoleSequenceHasRoleAddRule
		{
			public ImpliedObjectificationConstraintRoleSequenceHasRoleAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Objectification
	{
		private partial class ImpliedObjectificationConstraintRoleSequenceHasRoleDeletingRule
		{
			public ImpliedObjectificationConstraintRoleSequenceHasRoleDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Objectification
	{
		private partial class ImpliedObjectificationFactTypeHasRoleAddRule
		{
			public ImpliedObjectificationFactTypeHasRoleAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Objectification
	{
		private partial class ImpliedObjectificationFactTypeHasRoleDeletingRule
		{
			public ImpliedObjectificationFactTypeHasRoleDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Objectification
	{
		private partial class ImpliedObjectificationIsImpliedChangeRule
		{
			public ImpliedObjectificationIsImpliedChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Objectification
	{
		private partial class ImpliedObjectificationObjectifyingTypeIsIndependentChangeRule
		{
			public ImpliedObjectificationObjectifyingTypeIsIndependentChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Objectification
	{
		private partial class ImpliedObjectificationObjectifyingTypePlaysRoleAddRule
		{
			public ImpliedObjectificationObjectifyingTypePlaysRoleAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Objectification
	{
		private partial class ImpliedObjectificationUniquenessConstraintChangeRule
		{
			public ImpliedObjectificationUniquenessConstraintChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Objectification
	{
		private partial class InternalConstraintChangeRule
		{
			public InternalConstraintChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Objectification
	{
		private partial class ObjectificationAddRule
		{
			public ObjectificationAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Objectification
	{
		private partial class ObjectificationDeleteRule
		{
			public ObjectificationDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Objectification
	{
		private partial class ObjectificationRolePlayerChangeRule
		{
			public ObjectificationRolePlayerChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Objectification
	{
		private partial class PreferredIdentifierDeletingRule
		{
			public PreferredIdentifierDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Objectification
	{
		private partial class RoleAddRule
		{
			public RoleAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Objectification
	{
		private partial class RolePlayerAddRule
		{
			public RolePlayerAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Objectification
	{
		private partial class RoleDeletingRule
		{
			public RoleDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Objectification
	{
		private partial class RolePlayerDeletingRule
		{
			public RolePlayerDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Objectification
	{
		private partial class UniquenessConstraintAddRule
		{
			public UniquenessConstraintAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Objectification
	{
		private partial class UniquenessConstraintDeletingRule
		{
			public UniquenessConstraintDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectType
	{
		private partial class CheckForIncompatibleRelationshipRule
		{
			public CheckForIncompatibleRelationshipRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectType
	{
		private partial class CheckForIncompatibleRelationshipRolePlayerChangeRule
		{
			public CheckForIncompatibleRelationshipRolePlayerChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectType
	{
		private partial class MandatoryRoleAddedRule
		{
			public MandatoryRoleAddedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectType
	{
		private partial class MandatoryRoleDeletingRule
		{
			public MandatoryRoleDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectType
	{
		private partial class ModelHasObjectTypeAddRuleModelValidation
		{
			public ModelHasObjectTypeAddRuleModelValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectType
	{
		private partial class ObjectTypeChangeRule
		{
			public ObjectTypeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectType
	{
		private partial class ObjectTypeDeleteRule
		{
			public ObjectTypeDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectType
	{
		private partial class SubtypeFactChangeRule
		{
			public SubtypeFactChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectType
	{
		private partial class SupertypeAddedRule
		{
			public SupertypeAddedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectType
	{
		private partial class SupertypeDeleteRule
		{
			public SupertypeDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectType
	{
		private partial class SupertypeDeletingRule
		{
			public SupertypeDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectType
	{
		private partial class UnspecifiedDataTypeAddRule
		{
			public UnspecifiedDataTypeAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectType
	{
		private partial class UnspecifiedDataRoleRolePlayerChangeRule
		{
			public UnspecifiedDataRoleRolePlayerChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectType
	{
		private partial class VerifyReferenceSchemeAddRule
		{
			public VerifyReferenceSchemeAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectType
	{
		private partial class VerifyReferenceSchemeDeleteRule
		{
			public VerifyReferenceSchemeDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectType
	{
		private partial class VerifyValueTypeHasDataTypeAddRule
		{
			public VerifyValueTypeHasDataTypeAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectType
	{
		private partial class VerifyValueTypeHasDataTypeDeleteRule
		{
			public VerifyValueTypeHasDataTypeDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMCoreDomainModel
	{
		private partial class DelayValidateElements
		{
			public DelayValidateElements()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMModel
	{
		private partial class RemoveDuplicateConstraintNameErrorRule
		{
			public RemoveDuplicateConstraintNameErrorRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMModel
	{
		private partial class RemoveDuplicateObjectTypeNameErrorRule
		{
			public RemoveDuplicateObjectTypeNameErrorRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Reading
	{
		private partial class ReadingOrderHasRoleDeleted
		{
			public ReadingOrderHasRoleDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Reading
	{
		private partial class ReadingPropertiesChanged
		{
			public ReadingPropertiesChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ReadingOrder
	{
		private partial class EnforceNoEmptyReadingOrder
		{
			public EnforceNoEmptyReadingOrder()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ReadingOrder
	{
		private partial class FactTypeHasRoleAddedRule
		{
			public FactTypeHasRoleAddedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ReadingOrder
	{
		private partial class ReadingOrderHasRoleDeleting
		{
			public ReadingOrderHasRoleDeleting()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ReferenceMode
	{
		private partial class ReferenceModeAddedRule
		{
			public ReferenceModeAddedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ReferenceMode
	{
		private partial class ReferenceModeChangeRule
		{
			public ReferenceModeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ReferenceModeHasReferenceModeKind
	{
		private partial class ReferenceModeHasReferenceModeKindChangeRule
		{
			public ReferenceModeHasReferenceModeKindChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ReferenceModeHasReferenceModeKind
	{
		private partial class ReferenceModeHasReferenceModeKindDeletingRule
		{
			public ReferenceModeHasReferenceModeKindDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ReferenceModeKind
	{
		private partial class ReferenceModeKindChangeRule
		{
			public ReferenceModeKindChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class RingConstraint
	{
		private partial class RingConstraintTypeChangeRule
		{
			public RingConstraintTypeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Role
	{
		private partial class RoleChangeRule
		{
			public RoleChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Role
	{
		private partial class RolePlayerRequiredAddRule
		{
			public RolePlayerRequiredAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Role
	{
		private partial class RolePlayerRequiredForNewRoleAddRule
		{
			public RolePlayerRequiredForNewRoleAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Role
	{
		private partial class RolePlayerRequiredDeleteRule
		{
			public RolePlayerRequiredDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class Role
	{
		private partial class UpdatedRolePlayerRequiredErrorsDeleteRule
		{
			public UpdatedRolePlayerRequiredErrorsDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetComparisonConstraint
	{
		private partial class ConstraintHasRoleSequenceAdded
		{
			public ConstraintHasRoleSequenceAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetComparisonConstraint
	{
		private partial class ConstraintRoleSequenceHasRoleAdded
		{
			public ConstraintRoleSequenceHasRoleAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetComparisonConstraint
	{
		private partial class EnforceRoleSequenceCardinalityForAdd
		{
			public EnforceRoleSequenceCardinalityForAdd()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetComparisonConstraint
	{
		private partial class EnforceRoleSequenceCardinalityForConstraintAdd
		{
			public EnforceRoleSequenceCardinalityForConstraintAdd()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetComparisonConstraint
	{
		private partial class EnforceRoleSequenceCardinalityForDelete
		{
			public EnforceRoleSequenceCardinalityForDelete()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetComparisonConstraint
	{
		private partial class EnforceRoleSequenceValidityForAdd
		{
			public EnforceRoleSequenceValidityForAdd()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetComparisonConstraint
	{
		private partial class EnforceRoleSequenceValidityForDelete
		{
			public EnforceRoleSequenceValidityForDelete()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetComparisonConstraint
	{
		private partial class EnforceRoleSequenceValidityForReorder
		{
			public EnforceRoleSequenceValidityForReorder()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetComparisonConstraint
	{
		private partial class EnforceRoleSequenceValidityForRolePlayerAdd
		{
			public EnforceRoleSequenceValidityForRolePlayerAdd()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetComparisonConstraint
	{
		private partial class EnforceRoleSequenceValidityForRolePlayerDelete
		{
			public EnforceRoleSequenceValidityForRolePlayerDelete()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetComparisonConstraint
	{
		private partial class EnforceRoleSequenceValidityForRolePlayerRolePlayerChange
		{
			public EnforceRoleSequenceValidityForRolePlayerRolePlayerChange()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetComparisonConstraint
	{
		private partial class ExternalRoleConstraintDeleted
		{
			public ExternalRoleConstraintDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetComparisonConstraint
	{
		private partial class FactAdded
		{
			public FactAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetComparisonConstraint
	{
		private partial class FactSetComparisonConstraintAdded
		{
			public FactSetComparisonConstraintAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetConstraint
	{
		private partial class ConstraintAdded
		{
			public ConstraintAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetConstraint
	{
		private partial class ConstraintRoleSequenceHasRoleAdded
		{
			public ConstraintRoleSequenceHasRoleAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetConstraint
	{
		private partial class EnforceRoleSequenceValidityForAdd
		{
			public EnforceRoleSequenceValidityForAdd()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetConstraint
	{
		private partial class EnforceRoleSequenceValidityForDelete
		{
			public EnforceRoleSequenceValidityForDelete()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetConstraint
	{
		private partial class EnforceRoleSequenceValidityForRolePlayerAdd
		{
			public EnforceRoleSequenceValidityForRolePlayerAdd()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetConstraint
	{
		private partial class EnforceRoleSequenceValidityForRolePlayerDelete
		{
			public EnforceRoleSequenceValidityForRolePlayerDelete()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetConstraint
	{
		private partial class EnforceRoleSequenceValidityForRolePlayerRolePlayerChange
		{
			public EnforceRoleSequenceValidityForRolePlayerRolePlayerChange()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetConstraint
	{
		private partial class FactAdded
		{
			public FactAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetConstraint
	{
		private partial class FactSetConstraintAdded
		{
			public FactSetConstraintAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SetConstraint
	{
		private partial class FactSetConstraintDeleted
		{
			public FactSetConstraintDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SubtypeFact
	{
		private partial class EnsureConsistentDataTypesAddRule
		{
			public EnsureConsistentDataTypesAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SubtypeFact
	{
		private partial class EnsureConsistentDataTypesDeleteRule
		{
			public EnsureConsistentDataTypesDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SubtypeFact
	{
		private partial class EnsureConsistentRolePlayerTypesAddRule
		{
			public EnsureConsistentRolePlayerTypesAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SubtypeFact
	{
		private partial class InitializeSubtypeAddRule
		{
			public InitializeSubtypeAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SubtypeFact
	{
		private partial class LimitSubtypeConstraintChangeRule
		{
			public LimitSubtypeConstraintChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SubtypeFact
	{
		private partial class LimitSubtypeConstraintRolesAddRule
		{
			public LimitSubtypeConstraintRolesAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SubtypeFact
	{
		private partial class LimitSubtypeConstraintRolesDeleteRule
		{
			public LimitSubtypeConstraintRolesDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SubtypeFact
	{
		private partial class LimitSubtypeConstraintsAddRule
		{
			public LimitSubtypeConstraintsAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SubtypeFact
	{
		private partial class LimitSubtypeConstraintsDeleteRule
		{
			public LimitSubtypeConstraintsDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SubtypeFact
	{
		private partial class LimitSubtypeRolesAddRule
		{
			public LimitSubtypeRolesAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SubtypeFact
	{
		private partial class LimitSubtypeRolesDeleteRule
		{
			public LimitSubtypeRolesDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class SubtypeFact
	{
		private partial class DeleteSubtypeWhenRolePlayerDeleted
		{
			public DeleteSubtypeWhenRolePlayerDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class UniquenessConstraint
	{
		private partial class NMinusOneAddRuleModelConstraintAddValidation
		{
			public NMinusOneAddRuleModelConstraintAddValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class UniquenessConstraint
	{
		private partial class NMinusOneAddRuleModelFactAddValidation
		{
			public NMinusOneAddRuleModelFactAddValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class UniquenessConstraint
	{
		private partial class NMinusOneAddRuleModelValidation
		{
			public NMinusOneAddRuleModelValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class UniquenessConstraint
	{
		private partial class NMinusOneDeleteRuleModelConstraintDeleteValidation
		{
			public NMinusOneDeleteRuleModelConstraintDeleteValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class UniquenessConstraint
	{
		private partial class NMinusOneDeleteRuleModelFactDeleteValidation
		{
			public NMinusOneDeleteRuleModelFactDeleteValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class UniquenessConstraint
	{
		private partial class UniquenessConstraintChangeRule
		{
			public UniquenessConstraintChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ValueConstraint
	{
		private partial class DataTypeAddRule
		{
			public DataTypeAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ValueConstraint
	{
		private partial class DataTypeChangeRule
		{
			public DataTypeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ValueConstraint
	{
		private partial class DataTypeRolePlayerChangeRule
		{
			public DataTypeRolePlayerChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ValueConstraint
	{
		private partial class ObjectTypeRoleAdded
		{
			public ObjectTypeRoleAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ValueConstraint
	{
		private partial class RoleValueConstraintAdded
		{
			public RoleValueConstraintAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ValueConstraint
	{
		private partial class ValueConstraintAddRule
		{
			public ValueConstraintAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ValueConstraint
	{
		private partial class ValueConstraintChangeRule
		{
			public ValueConstraintChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ValueConstraint
	{
		private partial class ValueRangeAdded
		{
			public ValueRangeAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ValueConstraint
	{
		private partial class ValueRangeChangeRule
		{
			public ValueRangeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ValueRange
	{
		private partial class ValueRangeChangeRule
		{
			public ValueRangeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ValueTypeInstance
	{
		private partial class ValueTypeHasDataTypeAdded
		{
			public ValueTypeHasDataTypeAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ValueTypeInstance
	{
		private partial class ValueTypeHasDataTypeRolePlayerChange
		{
			public ValueTypeHasDataTypeRolePlayerChange()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ValueTypeInstance
	{
		private partial class ValueTypeInstanceValueChanged
		{
			public ValueTypeInstanceValueChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ValueTypeInstance
	{
		private partial class ValueTypeHasValueTypeInstanceAdded
		{
			public ValueTypeHasValueTypeInstanceAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	#endregion // Initially disable rules
}
namespace Neumont.Tools.Modeling
{
	#region Initially disable rules
	public partial class NamedElementDictionary
	{
		private partial class ElementLinkAddedRule
		{
			public ElementLinkAddedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class NamedElementDictionary
	{
		private partial class ElementLinkDeleteRule
		{
			public ElementLinkDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class NamedElementDictionary
	{
		private partial class NamedElementChangedRule
		{
			public NamedElementChangedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	#endregion // Initially disable rules
}
