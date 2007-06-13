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
	partial class ORMCoreDomainModel : Neumont.Tools.ORM.ObjectModel.IDomainModelEnablesRulesAfterDeserialization
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
						typeof(ConstraintRoleSequence).GetNestedType("BlockRolePlayerChange", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("ConstraintRoleSequenceHasRoleAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("ConstraintRoleSequenceHasRoleDeleted", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("ConstraintRoleSequenceHasRoleRolePlayerChanged", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("ModalityChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("SetComparisonConstraintHasRoleDeleting", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("SetConstraintDeleting", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintUtility).GetNestedType("ConstraintRoleSequenceHasRoleDeleted", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(CustomReferenceMode).GetNestedType("CustomReferenceModeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("ModalityChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("PreferredIdentifierAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("PreferredIdentifierRolePlayerChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("TestRemovePreferredIdentifierConstraintRoleAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("TestRemovePreferredIdentifierObjectificationAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("TestRemovePreferredIdentifierObjectificationRolePlayerChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("TestRemovePreferredIdentifierRoleAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("TestRemovePreferredIdentifierDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("TestRemovePreferredIdentifierRolePlayerChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("EntityTypeInstanceDeleting", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("ConstraintRoleSequenceHasRoleAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("ConstraintRoleSequenceHasRoleDeleted", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("EntityTypeHasEntityTypeInstanceAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("EntityTypeHasPreferredIdentifierAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("EntityTypeHasPreferredIdentifierDeleted", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("EntityTypeHasPreferredIdentifierRolePlayerChanged", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("EntityTypeInstanceHasRoleInstanceAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("EntityTypeInstanceHasRoleInstanceDeleted", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("RoleInstanceHasPopulationUniquenessErrorDeleted", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EqualityConstraint).GetNestedType("ConstraintRoleSequenceHasRoleAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EqualityConstraint).GetNestedType("ConstraintRoleSequenceHasRoleDeleting", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExclusiveOrConstraintCoupler).GetNestedType("CouplerAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExclusiveOrConstraintCoupler).GetNestedType("CouplerDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExclusiveOrConstraintCoupler).GetNestedType("ExclusionConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExclusiveOrConstraintCoupler).GetNestedType("MandatoryConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExclusiveOrConstraintCoupler).GetNestedType("RoleAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExclusiveOrConstraintCoupler).GetNestedType("RoleDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExclusiveOrConstraintCoupler).GetNestedType("RolePositionChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExclusiveOrConstraintCoupler).GetNestedType("RoleSequenceAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExclusiveOrConstraintCoupler).GetNestedType("RoleSequencePositionChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
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
						typeof(FactType).GetNestedType("UnaryBinarizationUtility", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ConstraintRoleSequenceHasRoleAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("UnaryBinarizationUtility", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ConstraintRoleSequenceHasRoleDeleted", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("UnaryBinarizationUtility", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("FactTypeHasRoleAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("UnaryBinarizationUtility", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("FactTypeHasRoleDeleting", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("UnaryBinarizationUtility", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("FactTypeNameChanged", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("UnaryBinarizationUtility", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ObjectTypePlaysRoleAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("UnaryBinarizationUtility", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ObjectTypePlaysRoleDeleted", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("UnaryBinarizationUtility", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ObjectTypePlaysRoleRolePlayerChanged", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ValidateFactNameForObjectificationAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ValidateFactNameForObjectificationDelete", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ValidateFactNameForObjectificationRolePlayerChange", BindingFlags.Public | BindingFlags.NonPublic),
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
						typeof(Objectification).GetNestedType("ObjectificationDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ObjectificationRolePlayerChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("PreferredIdentifierDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("PreferredIdentifierRolePlayerChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("RoleAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("RolePlayerAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("RoleDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("RolePlayerDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("UniquenessConstraintAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("UniquenessConstraintDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("CheckForIncompatibleRelationshipRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("CheckForIncompatibleRelationshipRolePlayerChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("CheckIsIndependentRolePlayerChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("MandatoryModalityChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
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
						typeof(ObjectType).GetNestedType("VerifyReferenceSchemeRolePlayerChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("VerifyValueTypeHasDataTypeAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("VerifyValueTypeHasDataTypeDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("EntityTypeInstanceHasRoleInstanceAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("EntityTypeInstanceHasRoleInstanceDeleted", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("EntityTypeInstanceHasRoleInstanceRolePlayerChanged", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("ValueTypeInstanceValueChanged", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(DelayValidateElements),
						typeof(TransactionRulesFixupHack),
						typeof(ORMModel).GetNestedType("RemoveDuplicateConstraintNameErrorRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ORMModel).GetNestedType("RemoveDuplicateObjectTypeNameErrorRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Reading).GetNestedType("ReadingOrderHasRoleDeleted", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Reading).GetNestedType("ReadingPropertiesChanged", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingOrder).GetNestedType("EnforceNoEmptyReadingOrderDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingOrder).GetNestedType("EnforceNoEmptyReadingOrderRolePlayerChange", BindingFlags.Public | BindingFlags.NonPublic),
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
						typeof(Role).GetNestedType("ConstraintRoleSequenceHasRoleAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("ConstraintRoleSequenceHasRoleDeleted", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("ConstraintRoleSequenceHasRoleRolePlayerChanged", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RoleInstanceAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RoleInstanceDeleted", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RoleInstanceRolePlayerChanged", BindingFlags.Public | BindingFlags.NonPublic),
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
						typeof(SetComparisonConstraint).GetNestedType("SetComparisonConstraintRoleSequenceDeleted", BindingFlags.Public | BindingFlags.NonPublic),
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
						typeof(SetConstraint).GetNestedType("ModalityChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("SetConstraintRoleSequenceHasRoleAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("SetConstraintRoleSequenceHasRoleDeleting", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("DeleteSubtypeWhenRolePlayerDeleted", BindingFlags.Public | BindingFlags.NonPublic),
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
						typeof(SubtypeFact).GetNestedType("LimitSubtypeSetComparisonConstraintSequenceAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(UniquenessConstraint).GetNestedType("NMinusOneAddRuleModelConstraintAddValidation", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(UniquenessConstraint).GetNestedType("NMinusOneAddRuleModelFactAddValidation", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(UniquenessConstraint).GetNestedType("NMinusOneAddRuleModelValidation", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(UniquenessConstraint).GetNestedType("NMinusOneDeleteRuleModelConstraintDeleteValidation", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(UniquenessConstraint).GetNestedType("NMinusOneDeleteRuleModelFactDeleteValidation", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(UniquenessConstraint).GetNestedType("UniquenessConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("DataTypeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("DataTypeDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("DataTypeRolePlayerChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("ObjectTypeRoleAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("PreferredIdentifierDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("PreferredIdentifierRolePlayerChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("PreferredIdentifierRoleAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("RolePlayerDeleting", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("RolePlayerRolePlayerChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
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
		private static Type[] myInitiallyDisabledRuleTypes;
		private static Type[] InitiallyDisabledRuleTypes
		{
			get
			{
				Type[] retVal = ORMCoreDomainModel.myInitiallyDisabledRuleTypes;
				if (retVal == null)
				{
					Type[] customDomainModelTypes = ORMCoreDomainModel.CustomDomainModelTypes;
					retVal = new Type[]{
						customDomainModelTypes[0],
						customDomainModelTypes[1],
						customDomainModelTypes[2],
						customDomainModelTypes[3],
						customDomainModelTypes[4],
						customDomainModelTypes[5],
						customDomainModelTypes[6],
						customDomainModelTypes[7],
						customDomainModelTypes[8],
						customDomainModelTypes[9],
						customDomainModelTypes[10],
						customDomainModelTypes[11],
						customDomainModelTypes[12],
						customDomainModelTypes[13],
						customDomainModelTypes[14],
						customDomainModelTypes[15],
						customDomainModelTypes[16],
						customDomainModelTypes[17],
						customDomainModelTypes[18],
						customDomainModelTypes[19],
						customDomainModelTypes[20],
						customDomainModelTypes[21],
						customDomainModelTypes[22],
						customDomainModelTypes[23],
						customDomainModelTypes[24],
						customDomainModelTypes[25],
						customDomainModelTypes[26],
						customDomainModelTypes[27],
						customDomainModelTypes[28],
						customDomainModelTypes[29],
						customDomainModelTypes[30],
						customDomainModelTypes[31],
						customDomainModelTypes[32],
						customDomainModelTypes[33],
						customDomainModelTypes[34],
						customDomainModelTypes[35],
						customDomainModelTypes[36],
						customDomainModelTypes[37],
						customDomainModelTypes[38],
						customDomainModelTypes[39],
						customDomainModelTypes[40],
						customDomainModelTypes[41],
						customDomainModelTypes[42],
						customDomainModelTypes[43],
						customDomainModelTypes[44],
						customDomainModelTypes[45],
						customDomainModelTypes[46],
						customDomainModelTypes[47],
						customDomainModelTypes[48],
						customDomainModelTypes[49],
						customDomainModelTypes[50],
						customDomainModelTypes[51],
						customDomainModelTypes[52],
						customDomainModelTypes[53],
						customDomainModelTypes[54],
						customDomainModelTypes[55],
						customDomainModelTypes[56],
						customDomainModelTypes[57],
						customDomainModelTypes[58],
						customDomainModelTypes[59],
						customDomainModelTypes[60],
						customDomainModelTypes[61],
						customDomainModelTypes[62],
						customDomainModelTypes[63],
						customDomainModelTypes[64],
						customDomainModelTypes[65],
						customDomainModelTypes[66],
						customDomainModelTypes[67],
						customDomainModelTypes[68],
						customDomainModelTypes[69],
						customDomainModelTypes[70],
						customDomainModelTypes[71],
						customDomainModelTypes[72],
						customDomainModelTypes[73],
						customDomainModelTypes[74],
						customDomainModelTypes[75],
						customDomainModelTypes[76],
						customDomainModelTypes[77],
						customDomainModelTypes[78],
						customDomainModelTypes[79],
						customDomainModelTypes[80],
						customDomainModelTypes[81],
						customDomainModelTypes[82],
						customDomainModelTypes[83],
						customDomainModelTypes[84],
						customDomainModelTypes[85],
						customDomainModelTypes[86],
						customDomainModelTypes[87],
						customDomainModelTypes[88],
						customDomainModelTypes[89],
						customDomainModelTypes[90],
						customDomainModelTypes[91],
						customDomainModelTypes[92],
						customDomainModelTypes[93],
						customDomainModelTypes[94],
						customDomainModelTypes[95],
						customDomainModelTypes[96],
						customDomainModelTypes[97],
						customDomainModelTypes[98],
						customDomainModelTypes[99],
						customDomainModelTypes[100],
						customDomainModelTypes[101],
						customDomainModelTypes[102],
						customDomainModelTypes[103],
						customDomainModelTypes[104],
						customDomainModelTypes[105],
						customDomainModelTypes[106],
						customDomainModelTypes[107],
						customDomainModelTypes[108],
						customDomainModelTypes[109],
						customDomainModelTypes[110],
						customDomainModelTypes[111],
						customDomainModelTypes[112],
						customDomainModelTypes[113],
						customDomainModelTypes[114],
						customDomainModelTypes[115],
						customDomainModelTypes[116],
						customDomainModelTypes[117],
						customDomainModelTypes[118],
						customDomainModelTypes[119],
						customDomainModelTypes[120],
						customDomainModelTypes[121],
						customDomainModelTypes[122],
						customDomainModelTypes[123],
						customDomainModelTypes[124],
						customDomainModelTypes[125],
						customDomainModelTypes[126],
						customDomainModelTypes[127],
						customDomainModelTypes[128],
						customDomainModelTypes[129],
						customDomainModelTypes[132],
						customDomainModelTypes[133],
						customDomainModelTypes[134],
						customDomainModelTypes[135],
						customDomainModelTypes[136],
						customDomainModelTypes[137],
						customDomainModelTypes[138],
						customDomainModelTypes[139],
						customDomainModelTypes[140],
						customDomainModelTypes[141],
						customDomainModelTypes[142],
						customDomainModelTypes[143],
						customDomainModelTypes[144],
						customDomainModelTypes[145],
						customDomainModelTypes[146],
						customDomainModelTypes[147],
						customDomainModelTypes[148],
						customDomainModelTypes[149],
						customDomainModelTypes[150],
						customDomainModelTypes[151],
						customDomainModelTypes[152],
						customDomainModelTypes[153],
						customDomainModelTypes[154],
						customDomainModelTypes[155],
						customDomainModelTypes[156],
						customDomainModelTypes[157],
						customDomainModelTypes[158],
						customDomainModelTypes[159],
						customDomainModelTypes[160],
						customDomainModelTypes[161],
						customDomainModelTypes[162],
						customDomainModelTypes[163],
						customDomainModelTypes[164],
						customDomainModelTypes[165],
						customDomainModelTypes[166],
						customDomainModelTypes[167],
						customDomainModelTypes[168],
						customDomainModelTypes[169],
						customDomainModelTypes[170],
						customDomainModelTypes[171],
						customDomainModelTypes[172],
						customDomainModelTypes[173],
						customDomainModelTypes[174],
						customDomainModelTypes[175],
						customDomainModelTypes[176],
						customDomainModelTypes[177],
						customDomainModelTypes[178],
						customDomainModelTypes[179],
						customDomainModelTypes[180],
						customDomainModelTypes[181],
						customDomainModelTypes[182],
						customDomainModelTypes[183],
						customDomainModelTypes[184],
						customDomainModelTypes[185],
						customDomainModelTypes[186],
						customDomainModelTypes[187],
						customDomainModelTypes[188],
						customDomainModelTypes[189],
						customDomainModelTypes[190],
						customDomainModelTypes[191],
						customDomainModelTypes[192],
						customDomainModelTypes[193],
						customDomainModelTypes[194],
						customDomainModelTypes[195],
						customDomainModelTypes[196],
						customDomainModelTypes[197],
						customDomainModelTypes[198],
						customDomainModelTypes[199],
						customDomainModelTypes[200],
						customDomainModelTypes[201],
						customDomainModelTypes[202],
						customDomainModelTypes[203],
						customDomainModelTypes[204],
						customDomainModelTypes[205],
						customDomainModelTypes[206],
						customDomainModelTypes[207],
						customDomainModelTypes[208],
						customDomainModelTypes[209],
						customDomainModelTypes[210],
						customDomainModelTypes[211],
						customDomainModelTypes[212],
						customDomainModelTypes[213],
						customDomainModelTypes[214],
						customDomainModelTypes[215],
						customDomainModelTypes[216],
						customDomainModelTypes[217],
						customDomainModelTypes[218],
						customDomainModelTypes[219],
						customDomainModelTypes[220],
						customDomainModelTypes[221],
						customDomainModelTypes[222]};
					ORMCoreDomainModel.myInitiallyDisabledRuleTypes = retVal;
				}
				return retVal;
			}
		}
		/// <summary>Generated code to attach <see cref="Microsoft.VisualStudio.Modeling.Rule"/>s to the <see cref="Microsoft.VisualStudio.Modeling.Store"/>.</summary>
		/// <seealso cref="Microsoft.VisualStudio.Modeling.DomainModel.GetCustomDomainModelTypes"/>
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
		/// <summary>Implements IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization</summary>
		protected void EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.Store store)
		{
			Microsoft.VisualStudio.Modeling.RuleManager ruleManager = store.RuleManager;
			Type[] disabledRuleTypes = ORMCoreDomainModel.InitiallyDisabledRuleTypes;
			for (int i = 0; i < 221; ++i)
			{
				ruleManager.EnableRule(disabledRuleTypes[i]);
			}
		}
		void Neumont.Tools.ORM.ObjectModel.IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.Store store)
		{
			this.EnableRulesAfterDeserialization(store);
		}
	}
	#endregion // Attach rules to ORMCoreDomainModel model
	#region Initially disable rules
	partial class ConstraintRoleSequence
	{
		partial class BlockRolePlayerChange
		{
			public BlockRolePlayerChange()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ConstraintRoleSequence
	{
		partial class ConstraintRoleSequenceHasRoleAdded
		{
			public ConstraintRoleSequenceHasRoleAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ConstraintRoleSequence
	{
		partial class ConstraintRoleSequenceHasRoleDeleted
		{
			public ConstraintRoleSequenceHasRoleDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ConstraintRoleSequence
	{
		partial class ConstraintRoleSequenceHasRoleRolePlayerChanged
		{
			public ConstraintRoleSequenceHasRoleRolePlayerChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ConstraintRoleSequence
	{
		partial class ModalityChangeRule
		{
			public ModalityChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ConstraintRoleSequence
	{
		partial class SetComparisonConstraintHasRoleDeleting
		{
			public SetComparisonConstraintHasRoleDeleting()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ConstraintRoleSequence
	{
		partial class SetConstraintDeleting
		{
			public SetConstraintDeleting()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ConstraintUtility
	{
		partial class ConstraintRoleSequenceHasRoleDeleted
		{
			public ConstraintRoleSequenceHasRoleDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class CustomReferenceMode
	{
		partial class CustomReferenceModeChangeRule
		{
			public CustomReferenceModeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class EntityTypeHasPreferredIdentifier
	{
		partial class ModalityChangeRule
		{
			public ModalityChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class EntityTypeHasPreferredIdentifier
	{
		partial class PreferredIdentifierAddedRule
		{
			public PreferredIdentifierAddedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class EntityTypeHasPreferredIdentifier
	{
		partial class PreferredIdentifierRolePlayerChangeRule
		{
			public PreferredIdentifierRolePlayerChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class EntityTypeHasPreferredIdentifier
	{
		partial class TestRemovePreferredIdentifierConstraintRoleAddRule
		{
			public TestRemovePreferredIdentifierConstraintRoleAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class EntityTypeHasPreferredIdentifier
	{
		partial class TestRemovePreferredIdentifierObjectificationAddRule
		{
			public TestRemovePreferredIdentifierObjectificationAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class EntityTypeHasPreferredIdentifier
	{
		partial class TestRemovePreferredIdentifierObjectificationRolePlayerChangeRule
		{
			public TestRemovePreferredIdentifierObjectificationRolePlayerChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class EntityTypeHasPreferredIdentifier
	{
		partial class TestRemovePreferredIdentifierRoleAddRule
		{
			public TestRemovePreferredIdentifierRoleAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class EntityTypeHasPreferredIdentifier
	{
		partial class TestRemovePreferredIdentifierDeletingRule
		{
			public TestRemovePreferredIdentifierDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class EntityTypeHasPreferredIdentifier
	{
		partial class TestRemovePreferredIdentifierRolePlayerChangeRule
		{
			public TestRemovePreferredIdentifierRolePlayerChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class EntityTypeInstance
	{
		partial class EntityTypeInstanceDeleting
		{
			public EntityTypeInstanceDeleting()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class EntityTypeInstance
	{
		partial class ConstraintRoleSequenceHasRoleAdded
		{
			public ConstraintRoleSequenceHasRoleAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class EntityTypeInstance
	{
		partial class ConstraintRoleSequenceHasRoleDeleted
		{
			public ConstraintRoleSequenceHasRoleDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class EntityTypeInstance
	{
		partial class EntityTypeHasEntityTypeInstanceAdded
		{
			public EntityTypeHasEntityTypeInstanceAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class EntityTypeInstance
	{
		partial class EntityTypeHasPreferredIdentifierAdded
		{
			public EntityTypeHasPreferredIdentifierAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class EntityTypeInstance
	{
		partial class EntityTypeHasPreferredIdentifierDeleted
		{
			public EntityTypeHasPreferredIdentifierDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class EntityTypeInstance
	{
		partial class EntityTypeHasPreferredIdentifierRolePlayerChanged
		{
			public EntityTypeHasPreferredIdentifierRolePlayerChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class EntityTypeInstance
	{
		partial class EntityTypeInstanceHasRoleInstanceAdded
		{
			public EntityTypeInstanceHasRoleInstanceAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class EntityTypeInstance
	{
		partial class EntityTypeInstanceHasRoleInstanceDeleted
		{
			public EntityTypeInstanceHasRoleInstanceDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class EntityTypeInstance
	{
		partial class RoleInstanceHasPopulationUniquenessErrorDeleted
		{
			public RoleInstanceHasPopulationUniquenessErrorDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class EqualityConstraint
	{
		partial class ConstraintRoleSequenceHasRoleAdded
		{
			public ConstraintRoleSequenceHasRoleAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class EqualityConstraint
	{
		partial class ConstraintRoleSequenceHasRoleDeleting
		{
			public ConstraintRoleSequenceHasRoleDeleting()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ExclusiveOrConstraintCoupler
	{
		partial class CouplerAddRule
		{
			public CouplerAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ExclusiveOrConstraintCoupler
	{
		partial class CouplerDeleteRule
		{
			public CouplerDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ExclusiveOrConstraintCoupler
	{
		partial class ExclusionConstraintChangeRule
		{
			public ExclusionConstraintChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ExclusiveOrConstraintCoupler
	{
		partial class MandatoryConstraintChangeRule
		{
			public MandatoryConstraintChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ExclusiveOrConstraintCoupler
	{
		partial class RoleAddRule
		{
			public RoleAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ExclusiveOrConstraintCoupler
	{
		partial class RoleDeletingRule
		{
			public RoleDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ExclusiveOrConstraintCoupler
	{
		partial class RolePositionChangeRule
		{
			public RolePositionChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ExclusiveOrConstraintCoupler
	{
		partial class RoleSequenceAddRule
		{
			public RoleSequenceAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ExclusiveOrConstraintCoupler
	{
		partial class RoleSequencePositionChangeRule
		{
			public RoleSequencePositionChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactType
	{
		partial class FactTypeChangeRule
		{
			public FactTypeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactType
	{
		partial class FactTypeHasReadingOrderAddRuleModelValidation
		{
			public FactTypeHasReadingOrderAddRuleModelValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactType
	{
		partial class FactTypeHasReadingOrderDeleteRuleModelValidation
		{
			public FactTypeHasReadingOrderDeleteRuleModelValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactType
	{
		partial class FactTypeHasRoleAddRule
		{
			public FactTypeHasRoleAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactType
	{
		partial class FactTypeHasRoleDeleteRule
		{
			public FactTypeHasRoleDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactType
	{
		partial class InternalConstraintCollectionHasConstraintAddedRule
		{
			public InternalConstraintCollectionHasConstraintAddedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactType
	{
		partial class InternalConstraintCollectionHasConstraintDeleteRule
		{
			public InternalConstraintCollectionHasConstraintDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactType
	{
		partial class InternalUniquenessConstraintChangeRule
		{
			public InternalUniquenessConstraintChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactType
	{
		partial class ModelHasFactTypeAddRuleModelValidation
		{
			public ModelHasFactTypeAddRuleModelValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactType
	{
		partial class ModelHasInternalConstraintAddRuleModelValidation
		{
			public ModelHasInternalConstraintAddRuleModelValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactType
	{
		partial class ModelHasInternalConstraintDeleteRuleModelValidation
		{
			public ModelHasInternalConstraintDeleteRuleModelValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactType
	{
		partial class ReadingOrderHasReadingAddRuleModelValidation
		{
			public ReadingOrderHasReadingAddRuleModelValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactType
	{
		partial class ReadingOrderHasReadingDeleteRuleModelValidation
		{
			public ReadingOrderHasReadingDeleteRuleModelValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactType
	{
		partial class UnaryBinarizationUtility
		{
			partial class ConstraintRoleSequenceHasRoleAdded
			{
				public ConstraintRoleSequenceHasRoleAdded()
				{
					base.IsEnabled = false;
				}
			}
		}
	}
	partial class FactType
	{
		partial class UnaryBinarizationUtility
		{
			partial class ConstraintRoleSequenceHasRoleDeleted
			{
				public ConstraintRoleSequenceHasRoleDeleted()
				{
					base.IsEnabled = false;
				}
			}
		}
	}
	partial class FactType
	{
		partial class UnaryBinarizationUtility
		{
			partial class FactTypeHasRoleAdded
			{
				public FactTypeHasRoleAdded()
				{
					base.IsEnabled = false;
				}
			}
		}
	}
	partial class FactType
	{
		partial class UnaryBinarizationUtility
		{
			partial class FactTypeHasRoleDeleting
			{
				public FactTypeHasRoleDeleting()
				{
					base.IsEnabled = false;
				}
			}
		}
	}
	partial class FactType
	{
		partial class UnaryBinarizationUtility
		{
			partial class FactTypeNameChanged
			{
				public FactTypeNameChanged()
				{
					base.IsEnabled = false;
				}
			}
		}
	}
	partial class FactType
	{
		partial class UnaryBinarizationUtility
		{
			partial class ObjectTypePlaysRoleAdded
			{
				public ObjectTypePlaysRoleAdded()
				{
					base.IsEnabled = false;
				}
			}
		}
	}
	partial class FactType
	{
		partial class UnaryBinarizationUtility
		{
			partial class ObjectTypePlaysRoleDeleted
			{
				public ObjectTypePlaysRoleDeleted()
				{
					base.IsEnabled = false;
				}
			}
		}
	}
	partial class FactType
	{
		partial class UnaryBinarizationUtility
		{
			partial class ObjectTypePlaysRoleRolePlayerChanged
			{
				public ObjectTypePlaysRoleRolePlayerChanged()
				{
					base.IsEnabled = false;
				}
			}
		}
	}
	partial class FactType
	{
		partial class ValidateFactNameForObjectificationAdded
		{
			public ValidateFactNameForObjectificationAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactType
	{
		partial class ValidateFactNameForObjectificationDelete
		{
			public ValidateFactNameForObjectificationDelete()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactType
	{
		partial class ValidateFactNameForObjectificationRolePlayerChange
		{
			public ValidateFactNameForObjectificationRolePlayerChange()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactType
	{
		partial class ValidateFactNameForObjectTypeNameChange
		{
			public ValidateFactNameForObjectTypeNameChange()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactType
	{
		partial class ValidateFactNameForReadingChange
		{
			public ValidateFactNameForReadingChange()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactType
	{
		partial class ValidateFactNameForReadingOrderReorder
		{
			public ValidateFactNameForReadingOrderReorder()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactType
	{
		partial class ValidateFactNameForReadingReorder
		{
			public ValidateFactNameForReadingReorder()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactType
	{
		partial class ValidateFactNameForRolePlayerAdded
		{
			public ValidateFactNameForRolePlayerAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactType
	{
		partial class ValidateFactNameForRolePlayerDelete
		{
			public ValidateFactNameForRolePlayerDelete()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactType
	{
		partial class ValidateFactNameForRolePlayerRolePlayerChange
		{
			public ValidateFactNameForRolePlayerRolePlayerChange()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactTypeDerivationExpression
	{
		partial class FactTypeDerivationExpressionChangeRule
		{
			public FactTypeDerivationExpressionChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactTypeInstance
	{
		partial class FactTypeHasRoleAdded
		{
			public FactTypeHasRoleAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactTypeInstance
	{
		partial class FactTypeHasRoleDeleted
		{
			public FactTypeHasRoleDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactTypeInstance
	{
		partial class FactTypeHasFactTypeInstanceAdded
		{
			public FactTypeHasFactTypeInstanceAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactTypeInstance
	{
		partial class FactTypeInstanceHasRoleInstanceAdded
		{
			public FactTypeInstanceHasRoleInstanceAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactTypeInstance
	{
		partial class FactTypeInstanceHasRoleInstanceDeleted
		{
			public FactTypeInstanceHasRoleInstanceDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FrequencyConstraint
	{
		partial class FrequencyConstraintMinMaxRule
		{
			public FrequencyConstraintMinMaxRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FrequencyConstraint
	{
		partial class RemoveContradictionErrorsWithFactTypeRule
		{
			public RemoveContradictionErrorsWithFactTypeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class MandatoryConstraint
	{
		partial class MandatoryConstraintChangeRule
		{
			public MandatoryConstraintChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ModelError
	{
		partial class SynchronizeErrorForOwnerRule
		{
			public SynchronizeErrorForOwnerRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ModelError
	{
		partial class SynchronizeErrorTextForModelRule
		{
			public SynchronizeErrorTextForModelRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Note
	{
		partial class NoteChangeRule
		{
			public NoteChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Objectification
	{
		partial class ImpliedFactTypeAddRule
		{
			public ImpliedFactTypeAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Objectification
	{
		partial class ImpliedObjectificationConstraintRoleSequenceHasRoleAddRule
		{
			public ImpliedObjectificationConstraintRoleSequenceHasRoleAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Objectification
	{
		partial class ImpliedObjectificationConstraintRoleSequenceHasRoleDeletingRule
		{
			public ImpliedObjectificationConstraintRoleSequenceHasRoleDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Objectification
	{
		partial class ImpliedObjectificationFactTypeHasRoleAddRule
		{
			public ImpliedObjectificationFactTypeHasRoleAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Objectification
	{
		partial class ImpliedObjectificationFactTypeHasRoleDeletingRule
		{
			public ImpliedObjectificationFactTypeHasRoleDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Objectification
	{
		partial class ImpliedObjectificationIsImpliedChangeRule
		{
			public ImpliedObjectificationIsImpliedChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Objectification
	{
		partial class ImpliedObjectificationObjectifyingTypeIsIndependentChangeRule
		{
			public ImpliedObjectificationObjectifyingTypeIsIndependentChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Objectification
	{
		partial class ImpliedObjectificationObjectifyingTypePlaysRoleAddRule
		{
			public ImpliedObjectificationObjectifyingTypePlaysRoleAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Objectification
	{
		partial class ImpliedObjectificationUniquenessConstraintChangeRule
		{
			public ImpliedObjectificationUniquenessConstraintChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Objectification
	{
		partial class InternalConstraintChangeRule
		{
			public InternalConstraintChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Objectification
	{
		partial class ObjectificationAddRule
		{
			public ObjectificationAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Objectification
	{
		partial class ObjectificationDeletingRule
		{
			public ObjectificationDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Objectification
	{
		partial class ObjectificationRolePlayerChangeRule
		{
			public ObjectificationRolePlayerChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Objectification
	{
		partial class PreferredIdentifierDeletingRule
		{
			public PreferredIdentifierDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Objectification
	{
		partial class PreferredIdentifierRolePlayerChangeRule
		{
			public PreferredIdentifierRolePlayerChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Objectification
	{
		partial class RoleAddRule
		{
			public RoleAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Objectification
	{
		partial class RolePlayerAddRule
		{
			public RolePlayerAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Objectification
	{
		partial class RoleDeletingRule
		{
			public RoleDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Objectification
	{
		partial class RolePlayerDeletingRule
		{
			public RolePlayerDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Objectification
	{
		partial class UniquenessConstraintAddRule
		{
			public UniquenessConstraintAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Objectification
	{
		partial class UniquenessConstraintDeletingRule
		{
			public UniquenessConstraintDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectType
	{
		partial class CheckForIncompatibleRelationshipRule
		{
			public CheckForIncompatibleRelationshipRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectType
	{
		partial class CheckForIncompatibleRelationshipRolePlayerChangeRule
		{
			public CheckForIncompatibleRelationshipRolePlayerChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectType
	{
		partial class CheckIsIndependentRolePlayerChangeRule
		{
			public CheckIsIndependentRolePlayerChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectType
	{
		partial class MandatoryModalityChangeRule
		{
			public MandatoryModalityChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectType
	{
		partial class MandatoryRoleAddedRule
		{
			public MandatoryRoleAddedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectType
	{
		partial class MandatoryRoleDeletingRule
		{
			public MandatoryRoleDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectType
	{
		partial class ModelHasObjectTypeAddRuleModelValidation
		{
			public ModelHasObjectTypeAddRuleModelValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectType
	{
		partial class ObjectTypeChangeRule
		{
			public ObjectTypeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectType
	{
		partial class ObjectTypeDeleteRule
		{
			public ObjectTypeDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectType
	{
		partial class SubtypeFactChangeRule
		{
			public SubtypeFactChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectType
	{
		partial class SupertypeAddedRule
		{
			public SupertypeAddedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectType
	{
		partial class SupertypeDeleteRule
		{
			public SupertypeDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectType
	{
		partial class SupertypeDeletingRule
		{
			public SupertypeDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectType
	{
		partial class UnspecifiedDataTypeAddRule
		{
			public UnspecifiedDataTypeAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectType
	{
		partial class UnspecifiedDataRoleRolePlayerChangeRule
		{
			public UnspecifiedDataRoleRolePlayerChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectType
	{
		partial class VerifyReferenceSchemeAddRule
		{
			public VerifyReferenceSchemeAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectType
	{
		partial class VerifyReferenceSchemeDeleteRule
		{
			public VerifyReferenceSchemeDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectType
	{
		partial class VerifyReferenceSchemeRolePlayerChangeRule
		{
			public VerifyReferenceSchemeRolePlayerChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectType
	{
		partial class VerifyValueTypeHasDataTypeAddRule
		{
			public VerifyValueTypeHasDataTypeAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectType
	{
		partial class VerifyValueTypeHasDataTypeDeleteRule
		{
			public VerifyValueTypeHasDataTypeDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectTypeInstance
	{
		partial class EntityTypeInstanceHasRoleInstanceAdded
		{
			public EntityTypeInstanceHasRoleInstanceAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectTypeInstance
	{
		partial class EntityTypeInstanceHasRoleInstanceDeleted
		{
			public EntityTypeInstanceHasRoleInstanceDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectTypeInstance
	{
		partial class EntityTypeInstanceHasRoleInstanceRolePlayerChanged
		{
			public EntityTypeInstanceHasRoleInstanceRolePlayerChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectTypeInstance
	{
		partial class ValueTypeInstanceValueChanged
		{
			public ValueTypeInstanceValueChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMModel
	{
		partial class RemoveDuplicateConstraintNameErrorRule
		{
			public RemoveDuplicateConstraintNameErrorRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMModel
	{
		partial class RemoveDuplicateObjectTypeNameErrorRule
		{
			public RemoveDuplicateObjectTypeNameErrorRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Reading
	{
		partial class ReadingOrderHasRoleDeleted
		{
			public ReadingOrderHasRoleDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Reading
	{
		partial class ReadingPropertiesChanged
		{
			public ReadingPropertiesChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ReadingOrder
	{
		partial class EnforceNoEmptyReadingOrderDeleteRule
		{
			public EnforceNoEmptyReadingOrderDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ReadingOrder
	{
		partial class EnforceNoEmptyReadingOrderRolePlayerChange
		{
			public EnforceNoEmptyReadingOrderRolePlayerChange()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ReadingOrder
	{
		partial class FactTypeHasRoleAddedRule
		{
			public FactTypeHasRoleAddedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ReadingOrder
	{
		partial class ReadingOrderHasRoleDeleting
		{
			public ReadingOrderHasRoleDeleting()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ReferenceMode
	{
		partial class ReferenceModeAddedRule
		{
			public ReferenceModeAddedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ReferenceMode
	{
		partial class ReferenceModeChangeRule
		{
			public ReferenceModeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ReferenceModeHasReferenceModeKind
	{
		partial class ReferenceModeHasReferenceModeKindChangeRule
		{
			public ReferenceModeHasReferenceModeKindChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ReferenceModeHasReferenceModeKind
	{
		partial class ReferenceModeHasReferenceModeKindDeletingRule
		{
			public ReferenceModeHasReferenceModeKindDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ReferenceModeKind
	{
		partial class ReferenceModeKindChangeRule
		{
			public ReferenceModeKindChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class RingConstraint
	{
		partial class RingConstraintTypeChangeRule
		{
			public RingConstraintTypeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Role
	{
		partial class RoleChangeRule
		{
			public RoleChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Role
	{
		partial class RolePlayerRequiredAddRule
		{
			public RolePlayerRequiredAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Role
	{
		partial class RolePlayerRequiredForNewRoleAddRule
		{
			public RolePlayerRequiredForNewRoleAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Role
	{
		partial class RolePlayerRequiredDeleteRule
		{
			public RolePlayerRequiredDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Role
	{
		partial class UpdatedRolePlayerRequiredErrorsDeleteRule
		{
			public UpdatedRolePlayerRequiredErrorsDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Role
	{
		partial class ConstraintRoleSequenceHasRoleAdded
		{
			public ConstraintRoleSequenceHasRoleAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Role
	{
		partial class ConstraintRoleSequenceHasRoleDeleted
		{
			public ConstraintRoleSequenceHasRoleDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Role
	{
		partial class ConstraintRoleSequenceHasRoleRolePlayerChanged
		{
			public ConstraintRoleSequenceHasRoleRolePlayerChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Role
	{
		partial class RoleInstanceAdded
		{
			public RoleInstanceAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Role
	{
		partial class RoleInstanceDeleted
		{
			public RoleInstanceDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class Role
	{
		partial class RoleInstanceRolePlayerChanged
		{
			public RoleInstanceRolePlayerChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetComparisonConstraint
	{
		partial class ConstraintHasRoleSequenceAdded
		{
			public ConstraintHasRoleSequenceAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetComparisonConstraint
	{
		partial class ConstraintRoleSequenceHasRoleAdded
		{
			public ConstraintRoleSequenceHasRoleAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetComparisonConstraint
	{
		partial class EnforceRoleSequenceCardinalityForAdd
		{
			public EnforceRoleSequenceCardinalityForAdd()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetComparisonConstraint
	{
		partial class EnforceRoleSequenceCardinalityForConstraintAdd
		{
			public EnforceRoleSequenceCardinalityForConstraintAdd()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetComparisonConstraint
	{
		partial class EnforceRoleSequenceCardinalityForDelete
		{
			public EnforceRoleSequenceCardinalityForDelete()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetComparisonConstraint
	{
		partial class EnforceRoleSequenceValidityForAdd
		{
			public EnforceRoleSequenceValidityForAdd()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetComparisonConstraint
	{
		partial class EnforceRoleSequenceValidityForDelete
		{
			public EnforceRoleSequenceValidityForDelete()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetComparisonConstraint
	{
		partial class EnforceRoleSequenceValidityForReorder
		{
			public EnforceRoleSequenceValidityForReorder()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetComparisonConstraint
	{
		partial class EnforceRoleSequenceValidityForRolePlayerAdd
		{
			public EnforceRoleSequenceValidityForRolePlayerAdd()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetComparisonConstraint
	{
		partial class EnforceRoleSequenceValidityForRolePlayerDelete
		{
			public EnforceRoleSequenceValidityForRolePlayerDelete()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetComparisonConstraint
	{
		partial class EnforceRoleSequenceValidityForRolePlayerRolePlayerChange
		{
			public EnforceRoleSequenceValidityForRolePlayerRolePlayerChange()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetComparisonConstraint
	{
		partial class ExternalRoleConstraintDeleted
		{
			public ExternalRoleConstraintDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetComparisonConstraint
	{
		partial class FactAdded
		{
			public FactAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetComparisonConstraint
	{
		partial class FactSetComparisonConstraintAdded
		{
			public FactSetComparisonConstraintAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetComparisonConstraint
	{
		partial class SetComparisonConstraintRoleSequenceDeleted
		{
			public SetComparisonConstraintRoleSequenceDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetConstraint
	{
		partial class ConstraintAdded
		{
			public ConstraintAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetConstraint
	{
		partial class ConstraintRoleSequenceHasRoleAdded
		{
			public ConstraintRoleSequenceHasRoleAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetConstraint
	{
		partial class EnforceRoleSequenceValidityForAdd
		{
			public EnforceRoleSequenceValidityForAdd()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetConstraint
	{
		partial class EnforceRoleSequenceValidityForDelete
		{
			public EnforceRoleSequenceValidityForDelete()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetConstraint
	{
		partial class EnforceRoleSequenceValidityForRolePlayerAdd
		{
			public EnforceRoleSequenceValidityForRolePlayerAdd()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetConstraint
	{
		partial class EnforceRoleSequenceValidityForRolePlayerDelete
		{
			public EnforceRoleSequenceValidityForRolePlayerDelete()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetConstraint
	{
		partial class EnforceRoleSequenceValidityForRolePlayerRolePlayerChange
		{
			public EnforceRoleSequenceValidityForRolePlayerRolePlayerChange()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetConstraint
	{
		partial class FactAdded
		{
			public FactAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetConstraint
	{
		partial class FactSetConstraintAdded
		{
			public FactSetConstraintAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetConstraint
	{
		partial class FactSetConstraintDeleted
		{
			public FactSetConstraintDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetConstraint
	{
		partial class ModalityChangeRule
		{
			public ModalityChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetConstraint
	{
		partial class SetConstraintRoleSequenceHasRoleAdded
		{
			public SetConstraintRoleSequenceHasRoleAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SetConstraint
	{
		partial class SetConstraintRoleSequenceHasRoleDeleting
		{
			public SetConstraintRoleSequenceHasRoleDeleting()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SubtypeFact
	{
		partial class DeleteSubtypeWhenRolePlayerDeleted
		{
			public DeleteSubtypeWhenRolePlayerDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SubtypeFact
	{
		partial class EnsureConsistentDataTypesAddRule
		{
			public EnsureConsistentDataTypesAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SubtypeFact
	{
		partial class EnsureConsistentDataTypesDeleteRule
		{
			public EnsureConsistentDataTypesDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SubtypeFact
	{
		partial class EnsureConsistentRolePlayerTypesAddRule
		{
			public EnsureConsistentRolePlayerTypesAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SubtypeFact
	{
		partial class InitializeSubtypeAddRule
		{
			public InitializeSubtypeAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SubtypeFact
	{
		partial class LimitSubtypeConstraintChangeRule
		{
			public LimitSubtypeConstraintChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SubtypeFact
	{
		partial class LimitSubtypeConstraintRolesAddRule
		{
			public LimitSubtypeConstraintRolesAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SubtypeFact
	{
		partial class LimitSubtypeConstraintRolesDeleteRule
		{
			public LimitSubtypeConstraintRolesDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SubtypeFact
	{
		partial class LimitSubtypeConstraintsAddRule
		{
			public LimitSubtypeConstraintsAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SubtypeFact
	{
		partial class LimitSubtypeConstraintsDeleteRule
		{
			public LimitSubtypeConstraintsDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SubtypeFact
	{
		partial class LimitSubtypeRolesAddRule
		{
			public LimitSubtypeRolesAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SubtypeFact
	{
		partial class LimitSubtypeRolesDeleteRule
		{
			public LimitSubtypeRolesDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class SubtypeFact
	{
		partial class LimitSubtypeSetComparisonConstraintSequenceAddRule
		{
			public LimitSubtypeSetComparisonConstraintSequenceAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class UniquenessConstraint
	{
		partial class NMinusOneAddRuleModelConstraintAddValidation
		{
			public NMinusOneAddRuleModelConstraintAddValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class UniquenessConstraint
	{
		partial class NMinusOneAddRuleModelFactAddValidation
		{
			public NMinusOneAddRuleModelFactAddValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class UniquenessConstraint
	{
		partial class NMinusOneAddRuleModelValidation
		{
			public NMinusOneAddRuleModelValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class UniquenessConstraint
	{
		partial class NMinusOneDeleteRuleModelConstraintDeleteValidation
		{
			public NMinusOneDeleteRuleModelConstraintDeleteValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class UniquenessConstraint
	{
		partial class NMinusOneDeleteRuleModelFactDeleteValidation
		{
			public NMinusOneDeleteRuleModelFactDeleteValidation()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class UniquenessConstraint
	{
		partial class UniquenessConstraintChangeRule
		{
			public UniquenessConstraintChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ValueConstraint
	{
		partial class DataTypeChangeRule
		{
			public DataTypeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ValueConstraint
	{
		partial class DataTypeDeletingRule
		{
			public DataTypeDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ValueConstraint
	{
		partial class DataTypeRolePlayerChangeRule
		{
			public DataTypeRolePlayerChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ValueConstraint
	{
		partial class ObjectTypeRoleAdded
		{
			public ObjectTypeRoleAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ValueConstraint
	{
		partial class PreferredIdentifierDeletingRule
		{
			public PreferredIdentifierDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ValueConstraint
	{
		partial class PreferredIdentifierRolePlayerChangeRule
		{
			public PreferredIdentifierRolePlayerChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ValueConstraint
	{
		partial class PreferredIdentifierRoleAddRule
		{
			public PreferredIdentifierRoleAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ValueConstraint
	{
		partial class RolePlayerDeleting
		{
			public RolePlayerDeleting()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ValueConstraint
	{
		partial class RolePlayerRolePlayerChangeRule
		{
			public RolePlayerRolePlayerChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ValueConstraint
	{
		partial class RoleValueConstraintAdded
		{
			public RoleValueConstraintAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ValueConstraint
	{
		partial class ValueConstraintAddRule
		{
			public ValueConstraintAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ValueConstraint
	{
		partial class ValueConstraintChangeRule
		{
			public ValueConstraintChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ValueConstraint
	{
		partial class ValueRangeAdded
		{
			public ValueRangeAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ValueConstraint
	{
		partial class ValueRangeChangeRule
		{
			public ValueRangeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ValueRange
	{
		partial class ValueRangeChangeRule
		{
			public ValueRangeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ValueTypeInstance
	{
		partial class ValueTypeHasDataTypeAdded
		{
			public ValueTypeHasDataTypeAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ValueTypeInstance
	{
		partial class ValueTypeHasDataTypeRolePlayerChange
		{
			public ValueTypeHasDataTypeRolePlayerChange()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ValueTypeInstance
	{
		partial class ValueTypeInstanceValueChanged
		{
			public ValueTypeInstanceValueChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ValueTypeInstance
	{
		partial class ValueTypeHasValueTypeInstanceAdded
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
	partial class NamedElementDictionary
	{
		partial class ElementLinkAddedRule
		{
			public ElementLinkAddedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class NamedElementDictionary
	{
		partial class ElementLinkDeleteRule
		{
			public ElementLinkDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class NamedElementDictionary
	{
		partial class NamedElementChangedRule
		{
			public NamedElementChangedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	#endregion // Initially disable rules
}
