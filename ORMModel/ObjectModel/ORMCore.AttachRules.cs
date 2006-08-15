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
	public partial class ORMCoreDomainModel
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
					// Given the low likelihood of this even happening, the extra overhead of synchronization would outweigh any possible gain from it.
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
		/// Generated code to attach s to the .
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
	}
	#endregion // Attach rules to ORMCoreDomainModel model
}
