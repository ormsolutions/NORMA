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
	partial class ORMCoreDomainModel : Neumont.Tools.Modeling.Shell.IDomainModelEnablesRulesAfterDeserialization
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
						typeof(ConstraintRoleSequence).GetNestedType("BlockRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("ConstraintRoleSequenceHasRoleAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("ConstraintRoleSequenceHasRoleDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("ModalityChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("SetComparisonConstraintHasRoleDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("SetConstraintDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintUtility).GetNestedType("ConstraintRoleSequenceHasRoleDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("ModalityChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("PreferredIdentifierAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("PreferredIdentifierRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("TestRemovePreferredIdentifierConstraintRoleAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("TestRemovePreferredIdentifierDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("TestRemovePreferredIdentifierObjectificationAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("TestRemovePreferredIdentifierObjectificationRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("TestRemovePreferredIdentifierRoleAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeHasPreferredIdentifier).GetNestedType("TestRemovePreferredIdentifierRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("ConstraintRoleSequenceHasRoleAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("ConstraintRoleSequenceHasRoleDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("EntityTypeHasEntityTypeInstanceAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("EntityTypeHasPreferredIdentifierAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("EntityTypeHasPreferredIdentifierDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("EntityTypeHasPreferredIdentifierRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("EntityTypeInstanceDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("EntityTypeInstanceHasRoleInstanceAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("EntityTypeInstanceHasRoleInstanceDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("RoleInstanceHasPopulationUniquenessErrorDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExclusiveOrConstraintCoupler).GetNestedType("CouplerAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExclusiveOrConstraintCoupler).GetNestedType("CouplerDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExclusiveOrConstraintCoupler).GetNestedType("ExclusionConstraintChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExclusiveOrConstraintCoupler).GetNestedType("MandatoryConstraintChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExclusiveOrConstraintCoupler).GetNestedType("RoleAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExclusiveOrConstraintCoupler).GetNestedType("RoleDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExclusiveOrConstraintCoupler).GetNestedType("RolePositionChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExclusiveOrConstraintCoupler).GetNestedType("RoleSequenceAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExclusiveOrConstraintCoupler).GetNestedType("RoleSequencePositionChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("BlockRoleMigrationRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("FactTypeAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("FactTypeChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("FactTypeHasReadingOrderAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("FactTypeHasReadingOrderDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("FactTypeHasRoleAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("FactTypeHasRoleDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("InternalConstraintAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("InternalConstraintDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("InternalConstraintCollectionHasConstraintAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("InternalConstraintCollectionHasConstraintDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("InternalUniquenessConstraintChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ReadingOrderHasReadingAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ReadingOrderHasReadingDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ValidateFactTypeNameForObjectificationAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ValidateFactTypeNameForObjectificationDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ValidateFactTypeNameForObjectificationRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ValidateFactTypeNameForObjectTypeNameChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ValidateFactTypeNameForReadingChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ValidateFactTypeNameForReadingOrderReorderRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ValidateFactTypeNameForReadingReorderRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ValidateFactTypeNameForRolePlayerAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ValidateFactTypeNameForRolePlayerDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("ValidateFactTypeNameForRolePlayerRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("UnaryBinarizationUtility", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ConstraintRoleSequenceHasRoleAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("UnaryBinarizationUtility", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ConstraintRoleSequenceHasRoleDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("UnaryBinarizationUtility", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("FactTypeHasRoleAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("UnaryBinarizationUtility", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("FactTypeHasRoleDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("UnaryBinarizationUtility", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("FactTypeNameChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("UnaryBinarizationUtility", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ObjectTypePlaysRoleAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("UnaryBinarizationUtility", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ObjectTypePlaysRoleDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("UnaryBinarizationUtility", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ObjectTypePlaysRoleRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("UnaryBinarizationUtility", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("RoleNameChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeDerivationExpression).GetNestedType("FactTypeDerivationExpressionChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeInstance).GetNestedType("FactTypeHasFactTypeInstanceAddedClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeInstance).GetNestedType("FactTypeHasRoleAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeInstance).GetNestedType("FactTypeHasRoleDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeInstance).GetNestedType("FactTypeInstanceHasRoleInstanceAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeInstance).GetNestedType("FactTypeInstanceHasRoleInstanceDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeInstance).GetNestedType("ImpliedBooleanRolePlayerDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FrequencyConstraint).GetNestedType("FrequencyConstraintMinMaxRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FrequencyConstraint).GetNestedType("RemoveContradictionErrorsWithFactTypeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(MandatoryConstraint).GetNestedType("MandatoryConstraintChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModelError).GetNestedType("SynchronizeErrorTextForModelRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModelError).GetNestedType("SynchronizeErrorTextForOwnerRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Neumont.Tools.Modeling.NamedElementDictionary).GetNestedType("ElementLinkAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Neumont.Tools.Modeling.NamedElementDictionary).GetNestedType("ElementLinkDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Neumont.Tools.Modeling.NamedElementDictionary).GetNestedType("NamedElementChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(NameGenerator).GetNestedType("SynchronizedRefinementsPropertyChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Note).GetNestedType("NoteChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ImpliedFactTypeAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ImpliedObjectificationConstraintRoleSequenceHasRoleAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ImpliedObjectificationConstraintRoleSequenceHasRoleDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ImpliedObjectificationFactTypeHasRoleAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ImpliedObjectificationFactTypeHasRoleDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ImpliedObjectificationIsImpliedChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ImpliedObjectificationObjectifyingTypeIsIndependentChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ImpliedObjectificationObjectifyingTypePlaysRoleAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ImpliedObjectificationUniquenessConstraintChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("InternalConstraintChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ObjectificationAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ObjectificationDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("ObjectificationRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("PreferredIdentifierDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("PreferredIdentifierRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("RoleAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("RoleDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("RolePlayerAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("RolePlayerDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("RolePlayerRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("UniquenessConstraintAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Objectification).GetNestedType("UniquenessConstraintDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("CheckForIncompatibleRelationshipAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("CheckForIncompatibleRelationshipRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("CheckIsIndependentRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("ConstraintRoleAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("ConstraintRoleDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("ExclusionModalityChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("ExclusionSequenceAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("MandatoryModalityChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("ObjectTypeAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("ObjectTypeChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("ObjectTypeDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("SubtypeFactChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("SupertypeAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("SupertypeDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("SupertypeDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("UnspecifiedDataTypeAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("UnspecifiedDataTypeRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("VerifyReferenceSchemeAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("VerifyReferenceSchemeDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("VerifyReferenceSchemeRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("VerifyValueTypeHasDataTypeAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectType).GetNestedType("VerifyValueTypeHasDataTypeDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("ConstraintRoleSequenceHasRoleAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("ConstraintRoleSequenceHasRoleDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("EntityTypeInstanceHasRoleInstanceAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("EntityTypeInstanceHasRoleInstanceDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("EntityTypeInstanceHasRoleInstanceRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("MandatoryConstraintChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("RoleInstanceAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("RoleInstanceDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("RolePlayerAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("RolePlayerDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("RolePlayerRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("ValueTypeInstanceValueChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ORMModel).GetNestedType("DuplicateConstraintNameConstraintDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ORMModel).GetNestedType("DuplicateObjectTypeNameObjectTypeDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ORMModel).GetNestedType("DuplicateRecognizedPhraseDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Reading).GetNestedType("ReadingOrderHasRoleDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Reading).GetNestedType("ReadingPropertiesChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RecognizedPhrase).GetNestedType("RecognizedPhraseHasAbbreviationDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingOrder).GetNestedType("EnforceNoEmptyReadingOrderDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingOrder).GetNestedType("EnforceNoEmptyReadingOrderRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingOrder).GetNestedType("FactTypeHasRoleAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingOrder).GetNestedType("ReadingOrderHasRoleDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReferenceMode).GetNestedType("CustomReferenceModeChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReferenceMode).GetNestedType("ReferenceModeAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReferenceMode).GetNestedType("ReferenceModeKindChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReferenceModeHasReferenceModeKind).GetNestedType("ReferenceModeHasReferenceModeKindDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReferenceModeHasReferenceModeKind).GetNestedType("ReferenceModeHasReferenceModeKindRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RingConstraint).GetNestedType("RingConstraintTypeChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("ConstraintRoleSequenceHasRoleAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("ConstraintRoleSequenceHasRoleDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RoleChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RoleInstanceAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RoleInstanceDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RoleInstanceRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RolePlayerRequiredAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RolePlayerRequiredDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RolePlayerRequiredForNewRoleAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("UpdatedRolePlayerRequiredErrorsDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("UniquenessConstraintChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("ConstraintHasRoleSequenceAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("ConstraintRoleSequenceHasRoleAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("EnforceRoleSequenceCardinalityForConstraintAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("EnforceRoleSequenceCardinalityForSequenceAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("EnforceRoleSequenceCardinalityForSequenceDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("EnforceRoleSequenceValidityForRoleAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("EnforceRoleSequenceValidityForRoleDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("EnforceRoleSequenceValidityForRoleReorderRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("EnforceRoleSequenceValidityForRolePlayerAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("EnforceRoleSequenceValidityForRolePlayerDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("ExternalRoleConstraintDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("FactTypeAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("FactSetComparisonConstraintAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetComparisonConstraint).GetNestedType("SetComparisonConstraintRoleSequenceDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("ConstraintAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("ConstraintRoleSequenceHasRoleAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("EnforceRoleSequenceValidityForRoleAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("EnforceRoleSequenceValidityForRoleDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("EnforceRoleSequenceValidityForRolePlayerAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("EnforceRoleSequenceValidityForRolePlayerDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("FactSetConstraintAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("FactSetConstraintDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("FactTypeAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("ModalityChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("SetConstraintRoleSequenceHasRoleAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SetConstraint).GetNestedType("SetConstraintRoleSequenceHasRoleDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("DeleteSubtypeWhenRolePlayerDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("EnsureConsistentDataTypesAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("EnsureConsistentDataTypesDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("EnsureConsistentRolePlayerTypesAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("InitializeSubtypeAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("LimitSubtypeConstraintChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("LimitSubtypeConstraintRolesAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("LimitSubtypeConstraintRolesDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("LimitSubtypeConstraintsAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("LimitSubtypeConstraintsDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("LimitSubtypeRolesAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("LimitSubtypeRolesDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(SubtypeFact).GetNestedType("LimitSubtypeSetComparisonConstraintSequenceAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(UniquenessConstraint).GetNestedType("NMinusOneConstraintAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(UniquenessConstraint).GetNestedType("NMinusOneConstraintRoleAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(UniquenessConstraint).GetNestedType("NMinusOneConstraintRoleDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(UniquenessConstraint).GetNestedType("NMinusOneFactTypeRoleAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(UniquenessConstraint).GetNestedType("NMinusOneFactTypeRoleDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(UniquenessConstraint).GetNestedType("UniquenessConstraintChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("DataTypeChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("DataTypeDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("DataTypeRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("ObjectTypeRoleAddedClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("PreferredIdentifierDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("PreferredIdentifierRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("PreferredIdentifierRoleAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("RolePlayerDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("RolePlayerRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("RoleValueConstraintAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("ValueConstraintAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("ValueConstraintChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("ValueRangeAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraint).GetNestedType("ValueRangeChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueRange).GetNestedType("ValueRangeChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueTypeInstance).GetNestedType("ValueTypeHasDataTypeAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueTypeInstance).GetNestedType("ValueTypeHasDataTypeDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueTypeInstance).GetNestedType("ValueTypeHasDataTypeRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueTypeInstance).GetNestedType("ValueTypeInstanceValueChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueTypeInstance).GetNestedType("ValueTypeHasValueTypeInstanceAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic)};
					ORMCoreDomainModel.myCustomDomainModelTypes = retVal;
					System.Diagnostics.Debug.Assert(Array.IndexOf<Type>(retVal, null) < 0, "One or more rule types failed to resolve. The file and/or package will fail to load.");
				}
				return retVal;
			}
		}
		/// <summary>Generated code to attach <see cref="Microsoft.VisualStudio.Modeling.Rule"/>s to the <see cref="Microsoft.VisualStudio.Modeling.Store"/>.</summary>
		/// <seealso cref="Microsoft.VisualStudio.Modeling.DomainModel.GetCustomDomainModelTypes"/>
		protected override Type[] GetCustomDomainModelTypes()
		{
			if (Neumont.Tools.Modeling.FrameworkDomainModel.InitializingToolboxItems)
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
			Type[] disabledRuleTypes = ORMCoreDomainModel.CustomDomainModelTypes;
			for (int i = 0; i < 235; ++i)
			{
				ruleManager.EnableRule(disabledRuleTypes[i]);
			}
		}
		void Neumont.Tools.Modeling.Shell.IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.Store store)
		{
			this.EnableRulesAfterDeserialization(store);
		}
	}
	#endregion // Attach rules to ORMCoreDomainModel model
	#region Auto-rule classes
	#region Rule classes for ConstraintRoleSequence
	partial class ConstraintRoleSequence
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class BlockRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public BlockRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequence
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void BlockRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequence.BlockRolePlayerChangeRule");
				ConstraintRoleSequence.BlockRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequence.BlockRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRoleSequenceHasRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleSequenceHasRoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequence
			/// /// <summary>
			/// /// AddRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void ConstraintRoleSequenceHasRoleAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequence.ConstraintRoleSequenceHasRoleAddedRule");
				ConstraintRoleSequence.ConstraintRoleSequenceHasRoleAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequence.ConstraintRoleSequenceHasRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRoleSequenceHasRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleSequenceHasRoleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequence
			/// /// <summary>
			/// /// DeleteRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void ConstraintRoleSequenceHasRoleDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequence.ConstraintRoleSequenceHasRoleDeletedRule");
				ConstraintRoleSequence.ConstraintRoleSequenceHasRoleDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequence.ConstraintRoleSequenceHasRoleDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetComparisonConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ModalityChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ModalityChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequence
			/// /// <summary>
			/// /// ChangeRule: typeof(SetComparisonConstraint)
			/// /// </summary>
			/// private static void ModalityChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequence.ModalityChangeRule");
				ConstraintRoleSequence.ModalityChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequence.ModalityChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequence), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class SetComparisonConstraintHasRoleDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SetComparisonConstraintHasRoleDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequence
			/// /// <summary>
			/// /// DeletingRule: typeof(ConstraintRoleSequence)
			/// /// </summary>
			/// private static void SetComparisonConstraintHasRoleDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequence.SetComparisonConstraintHasRoleDeletingRule");
				ConstraintRoleSequence.SetComparisonConstraintHasRoleDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequence.SetComparisonConstraintHasRoleDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class SetConstraintDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SetConstraintDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequence
			/// /// <summary>
			/// /// DeletingRule: typeof(SetConstraint)
			/// /// </summary>
			/// private static void SetConstraintDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequence.SetConstraintDeletingRule");
				ConstraintRoleSequence.SetConstraintDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequence.SetConstraintDeletingRule");
			}
		}
	}
	#endregion // Rule classes for ConstraintRoleSequence
	#region Rule classes for ConstraintUtility
	partial class ConstraintUtility
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=Neumont.Tools.Modeling.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class ConstraintRoleSequenceHasRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleSequenceHasRoleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ConstraintUtility
			/// /// <summary>
			/// /// DeleteRule: typeof(ConstraintRoleSequenceHasRole), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void ConstraintRoleSequenceHasRoleDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ConstraintUtility.ConstraintRoleSequenceHasRoleDeletedRule");
				ConstraintUtility.ConstraintRoleSequenceHasRoleDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ConstraintUtility.ConstraintRoleSequenceHasRoleDeletedRule");
			}
		}
	}
	#endregion // Rule classes for ConstraintUtility
	#region Rule classes for EntityTypeHasPreferredIdentifier
	partial class EntityTypeHasPreferredIdentifier
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ModalityChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ModalityChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier
			/// /// <summary>
			/// /// ChangeRule: typeof(SetConstraint)
			/// /// </summary>
			/// private static void ModalityChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.ModalityChangeRule");
				EntityTypeHasPreferredIdentifier.ModalityChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.ModalityChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class PreferredIdentifierAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			public override bool FireBefore
			{
				[System.Diagnostics.DebuggerStepThrough()]
				get
				{
					return true;
				}
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier
			/// /// <summary>
			/// /// AddRule: typeof(EntityTypeHasPreferredIdentifier)
			/// /// </summary>
			/// private static void PreferredIdentifierAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierAddedRule");
				EntityTypeHasPreferredIdentifier.PreferredIdentifierAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class PreferredIdentifierRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(EntityTypeHasPreferredIdentifier)
			/// /// </summary>
			/// private static void PreferredIdentifierRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierRolePlayerChangeRule");
				EntityTypeHasPreferredIdentifier.PreferredIdentifierRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class TestRemovePreferredIdentifierConstraintRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public TestRemovePreferredIdentifierConstraintRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier
			/// /// <summary>
			/// /// AddRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void TestRemovePreferredIdentifierConstraintRoleAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierConstraintRoleAddRule");
				EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierConstraintRoleAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierConstraintRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class TestRemovePreferredIdentifierDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public TestRemovePreferredIdentifierDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier
			/// /// <summary>
			/// /// DeletingRule: typeof(ObjectTypePlaysRole)
			/// /// DeletingRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void TestRemovePreferredIdentifierDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierDeletingRule");
				EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class TestRemovePreferredIdentifierObjectificationAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public TestRemovePreferredIdentifierObjectificationAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier
			/// /// <summary>
			/// /// AddRule: typeof(Objectification)
			/// /// </summary>
			/// private static void TestRemovePreferredIdentifierObjectificationAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierObjectificationAddRule");
				EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierObjectificationAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierObjectificationAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class TestRemovePreferredIdentifierObjectificationRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public TestRemovePreferredIdentifierObjectificationRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(Objectification)
			/// /// </summary>
			/// private static void TestRemovePreferredIdentifierObjectificationRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierObjectificationRolePlayerChangeRule");
				EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierObjectificationRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierObjectificationRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class TestRemovePreferredIdentifierRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public TestRemovePreferredIdentifierRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier
			/// /// <summary>
			/// /// AddRule: typeof(FactTypeHasRole)
			/// /// </summary>
			/// private static void TestRemovePreferredIdentifierRoleAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierRoleAddRule");
				EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierRoleAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class TestRemovePreferredIdentifierRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public TestRemovePreferredIdentifierRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void TestRemovePreferredIdentifierRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierRolePlayerChangeRule");
				EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierRolePlayerChangeRule");
			}
		}
	}
	#endregion // Rule classes for EntityTypeHasPreferredIdentifier
	#region Rule classes for EntityTypeInstance
	partial class EntityTypeInstance
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRoleSequenceHasRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleSequenceHasRoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.EntityTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void ConstraintRoleSequenceHasRoleAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeInstance.ConstraintRoleSequenceHasRoleAddedRule");
				EntityTypeInstance.ConstraintRoleSequenceHasRoleAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeInstance.ConstraintRoleSequenceHasRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRoleSequenceHasRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleSequenceHasRoleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.EntityTypeInstance
			/// /// <summary>
			/// /// DeleteRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void ConstraintRoleSequenceHasRoleDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeInstance.ConstraintRoleSequenceHasRoleDeletedRule");
				EntityTypeInstance.ConstraintRoleSequenceHasRoleDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeInstance.ConstraintRoleSequenceHasRoleDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasEntityTypeInstance), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EntityTypeHasEntityTypeInstanceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EntityTypeHasEntityTypeInstanceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.EntityTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(EntityTypeHasEntityTypeInstance)
			/// /// </summary>
			/// private static void EntityTypeHasEntityTypeInstanceAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeInstance.EntityTypeHasEntityTypeInstanceAddedRule");
				EntityTypeInstance.EntityTypeHasEntityTypeInstanceAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeInstance.EntityTypeHasEntityTypeInstanceAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EntityTypeHasPreferredIdentifierAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EntityTypeHasPreferredIdentifierAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.EntityTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(EntityTypeHasPreferredIdentifier)
			/// /// </summary>
			/// private static void EntityTypeHasPreferredIdentifierAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeInstance.EntityTypeHasPreferredIdentifierAddedRule");
				EntityTypeInstance.EntityTypeHasPreferredIdentifierAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeInstance.EntityTypeHasPreferredIdentifierAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EntityTypeHasPreferredIdentifierDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EntityTypeHasPreferredIdentifierDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.EntityTypeInstance
			/// /// <summary>
			/// /// DeleteRule: typeof(EntityTypeHasPreferredIdentifier)
			/// /// </summary>
			/// private static void EntityTypeHasPreferredIdentifierDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeInstance.EntityTypeHasPreferredIdentifierDeletedRule");
				EntityTypeInstance.EntityTypeHasPreferredIdentifierDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeInstance.EntityTypeHasPreferredIdentifierDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EntityTypeHasPreferredIdentifierRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EntityTypeHasPreferredIdentifierRolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.EntityTypeInstance
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(EntityTypeHasPreferredIdentifier)
			/// /// </summary>
			/// private static void EntityTypeHasPreferredIdentifierRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeInstance.EntityTypeHasPreferredIdentifierRolePlayerChangedRule");
				EntityTypeInstance.EntityTypeHasPreferredIdentifierRolePlayerChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeInstance.EntityTypeHasPreferredIdentifierRolePlayerChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeInstance), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EntityTypeInstanceDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EntityTypeInstanceDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.EntityTypeInstance
			/// /// <summary>
			/// /// DeletingRule: typeof(EntityTypeInstance)
			/// /// </summary>
			/// private static void EntityTypeInstanceDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeInstance.EntityTypeInstanceDeletingRule");
				EntityTypeInstance.EntityTypeInstanceDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeInstance.EntityTypeInstanceDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeInstanceHasRoleInstance), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EntityTypeInstanceHasRoleInstanceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EntityTypeInstanceHasRoleInstanceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.EntityTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(EntityTypeInstanceHasRoleInstance)
			/// /// </summary>
			/// private static void EntityTypeInstanceHasRoleInstanceAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeInstance.EntityTypeInstanceHasRoleInstanceAddedRule");
				EntityTypeInstance.EntityTypeInstanceHasRoleInstanceAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeInstance.EntityTypeInstanceHasRoleInstanceAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeInstanceHasRoleInstance), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EntityTypeInstanceHasRoleInstanceDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EntityTypeInstanceHasRoleInstanceDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.EntityTypeInstance
			/// /// <summary>
			/// /// DeletingRule: typeof(EntityTypeInstanceHasRoleInstance)
			/// /// </summary>
			/// private static void EntityTypeInstanceHasRoleInstanceDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeInstance.EntityTypeInstanceHasRoleInstanceDeletingRule");
				EntityTypeInstance.EntityTypeInstanceHasRoleInstanceDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeInstance.EntityTypeInstanceHasRoleInstanceDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RoleInstanceHasPopulationUniquenessError), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleInstanceHasPopulationUniquenessErrorDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleInstanceHasPopulationUniquenessErrorDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.EntityTypeInstance
			/// /// <summary>
			/// /// DeleteRule: typeof(RoleInstanceHasPopulationUniquenessError)
			/// /// </summary>
			/// private static void RoleInstanceHasPopulationUniquenessErrorDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeInstance.RoleInstanceHasPopulationUniquenessErrorDeletedRule");
				EntityTypeInstance.RoleInstanceHasPopulationUniquenessErrorDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.EntityTypeInstance.RoleInstanceHasPopulationUniquenessErrorDeletedRule");
			}
		}
	}
	#endregion // Rule classes for EntityTypeInstance
	#region Rule classes for ExclusiveOrConstraintCoupler
	partial class ExclusiveOrConstraintCoupler
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ExclusiveOrConstraintCoupler), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class CouplerAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public CouplerAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler
			/// /// <summary>
			/// /// AddRule: typeof(ExclusiveOrConstraintCoupler)
			/// /// </summary>
			/// private static void CouplerAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler.CouplerAddRule");
				ExclusiveOrConstraintCoupler.CouplerAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler.CouplerAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ExclusiveOrConstraintCoupler), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=Neumont.Tools.Modeling.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class CouplerDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public CouplerDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler
			/// /// <summary>
			/// /// DeleteRule: typeof(ExclusiveOrConstraintCoupler), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void CouplerDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler.CouplerDeleteRule");
				ExclusiveOrConstraintCoupler.CouplerDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler.CouplerDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ExclusionConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ExclusionConstraintChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ExclusionConstraintChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler
			/// /// <summary>
			/// /// ChangeRule: typeof(ExclusionConstraint)
			/// /// </summary>
			/// private static void ExclusionConstraintChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler.ExclusionConstraintChangeRule");
				ExclusiveOrConstraintCoupler.ExclusionConstraintChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler.ExclusionConstraintChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(MandatoryConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class MandatoryConstraintChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public MandatoryConstraintChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler
			/// /// <summary>
			/// /// ChangeRule: typeof(MandatoryConstraint)
			/// /// </summary>
			/// private static void MandatoryConstraintChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler.MandatoryConstraintChangeRule");
				ExclusiveOrConstraintCoupler.MandatoryConstraintChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler.MandatoryConstraintChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler
			/// /// <summary>
			/// /// AddRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void RoleAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler.RoleAddRule");
				ExclusiveOrConstraintCoupler.RoleAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler.RoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed partial class RoleDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler
			/// partial class RoleDeletingRuleClass
			/// {
			/// 	/// <summary>
			/// 	/// DeletingRule: typeof(ConstraintRoleSequenceHasRole)
			/// 	/// </summary>
			/// 	private void RoleDeletingRule(ElementDeletingEventArgs e)
			/// 	{
			/// 	}
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler.RoleDeletingRule");
				this.RoleDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler.RoleDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePositionChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePositionChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler
			/// /// <summary>
			/// /// RolePlayerPositionChangeRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void RolePositionChangeRule(RolePlayerOrderChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerPositionChanged(Microsoft.VisualStudio.Modeling.RolePlayerOrderChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler.RolePositionChangeRule");
				ExclusiveOrConstraintCoupler.RolePositionChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler.RolePositionChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetComparisonConstraintHasRoleSequence), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleSequenceAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleSequenceAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler
			/// /// <summary>
			/// /// AddRule: typeof(SetComparisonConstraintHasRoleSequence)
			/// /// </summary>
			/// private static void RoleSequenceAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler.RoleSequenceAddRule");
				ExclusiveOrConstraintCoupler.RoleSequenceAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler.RoleSequenceAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetComparisonConstraintHasRoleSequence), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleSequencePositionChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleSequencePositionChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler
			/// /// <summary>
			/// /// RolePlayerPositionChangeRule: typeof(SetComparisonConstraintHasRoleSequence)
			/// /// </summary>
			/// private static void RoleSequencePositionChangeRule(RolePlayerOrderChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerPositionChanged(Microsoft.VisualStudio.Modeling.RolePlayerOrderChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler.RoleSequencePositionChangeRule");
				ExclusiveOrConstraintCoupler.RoleSequencePositionChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler.RoleSequencePositionChangeRule");
			}
		}
	}
	#endregion // Rule classes for ExclusiveOrConstraintCoupler
	#region Rule classes for FactType
	partial class FactType
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class BlockRoleMigrationRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public BlockRoleMigrationRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(FactTypeHasRole)
			/// /// </summary>
			/// private static void BlockRoleMigrationRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.FactType.BlockRoleMigrationRule");
				FactType.BlockRoleMigrationRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.FactType.BlockRoleMigrationRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ModelHasFactType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// AddRule: typeof(ModelHasFactType)
			/// /// </summary>
			/// private static void FactTypeAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.FactTypeAddedRule");
				FactType.FactTypeAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.FactTypeAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// ChangeRule: typeof(FactType)
			/// /// </summary>
			/// private static void FactTypeChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.FactTypeChangeRule");
				FactType.FactTypeChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.FactTypeChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasReadingOrder), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeHasReadingOrderAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeHasReadingOrderAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// AddRule: typeof(FactTypeHasReadingOrder)
			/// /// </summary>
			/// private static void FactTypeHasReadingOrderAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.FactTypeHasReadingOrderAddRule");
				FactType.FactTypeHasReadingOrderAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.FactTypeHasReadingOrderAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasReadingOrder), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeHasReadingOrderDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeHasReadingOrderDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// DeleteRule: typeof(FactTypeHasReadingOrder)
			/// /// </summary>
			/// private static void FactTypeHasReadingOrderDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.FactTypeHasReadingOrderDeleteRule");
				FactType.FactTypeHasReadingOrderDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.FactTypeHasReadingOrderDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeHasRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeHasRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// AddRule: typeof(FactTypeHasRole)
			/// /// </summary>
			/// private static void FactTypeHasRoleAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.FactTypeHasRoleAddRule");
				FactType.FactTypeHasRoleAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.FactTypeHasRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeHasRoleDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeHasRoleDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// DeleteRule: typeof(FactTypeHasRole)
			/// /// </summary>
			/// private static void FactTypeHasRoleDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.FactTypeHasRoleDeleteRule");
				FactType.FactTypeHasRoleDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.FactTypeHasRoleDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactSetConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class InternalConstraintAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public InternalConstraintAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// AddRule: typeof(FactSetConstraint)
			/// /// </summary>
			/// private static void InternalConstraintAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.InternalConstraintAddRule");
				FactType.InternalConstraintAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.InternalConstraintAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactSetConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class InternalConstraintDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public InternalConstraintDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// DeleteRule: typeof(FactSetConstraint)
			/// /// </summary>
			/// private static void InternalConstraintDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.InternalConstraintDeleteRule");
				FactType.InternalConstraintDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.InternalConstraintDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class InternalConstraintCollectionHasConstraintAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public InternalConstraintCollectionHasConstraintAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// AddRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void InternalConstraintCollectionHasConstraintAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.InternalConstraintCollectionHasConstraintAddedRule");
				FactType.InternalConstraintCollectionHasConstraintAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.InternalConstraintCollectionHasConstraintAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class InternalConstraintCollectionHasConstraintDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public InternalConstraintCollectionHasConstraintDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// DeleteRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void InternalConstraintCollectionHasConstraintDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.InternalConstraintCollectionHasConstraintDeleteRule");
				FactType.InternalConstraintCollectionHasConstraintDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.InternalConstraintCollectionHasConstraintDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(UniquenessConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class InternalUniquenessConstraintChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public InternalUniquenessConstraintChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// ChangeRule: typeof(UniquenessConstraint)
			/// /// </summary>
			/// private static void InternalUniquenessConstraintChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.InternalUniquenessConstraintChangeRule");
				FactType.InternalUniquenessConstraintChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.InternalUniquenessConstraintChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReadingOrderHasReading), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ReadingOrderHasReadingAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReadingOrderHasReadingAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// AddRule: typeof(ReadingOrderHasReading)
			/// /// </summary>
			/// private static void ReadingOrderHasReadingAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ReadingOrderHasReadingAddRule");
				FactType.ReadingOrderHasReadingAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ReadingOrderHasReadingAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReadingOrderHasReading), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ReadingOrderHasReadingDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReadingOrderHasReadingDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// DeleteRule: typeof(ReadingOrderHasReading)
			/// /// </summary>
			/// private static void ReadingOrderHasReadingDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ReadingOrderHasReadingDeleteRule");
				FactType.ReadingOrderHasReadingDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ReadingOrderHasReadingDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=(1 + Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority))]
		private sealed class ValidateFactTypeNameForObjectificationAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValidateFactTypeNameForObjectificationAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// AddRule: typeof(Objectification), Priority=1;
			/// /// </summary>
			/// private static void ValidateFactTypeNameForObjectificationAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ValidateFactTypeNameForObjectificationAddedRule");
				FactType.ValidateFactTypeNameForObjectificationAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ValidateFactTypeNameForObjectificationAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=(1 + Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority))]
		private sealed class ValidateFactTypeNameForObjectificationDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValidateFactTypeNameForObjectificationDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// DeleteRule: typeof(Objectification), Priority=1;
			/// /// </summary>
			/// private static void ValidateFactTypeNameForObjectificationDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ValidateFactTypeNameForObjectificationDeleteRule");
				FactType.ValidateFactTypeNameForObjectificationDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ValidateFactTypeNameForObjectificationDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=(1 + Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority))]
		private sealed class ValidateFactTypeNameForObjectificationRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValidateFactTypeNameForObjectificationRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(Objectification), Priority=1;
			/// /// </summary>
			/// private static void ValidateFactTypeNameForObjectificationRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ValidateFactTypeNameForObjectificationRolePlayerChangeRule");
				FactType.ValidateFactTypeNameForObjectificationRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ValidateFactTypeNameForObjectificationRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValidateFactTypeNameForObjectTypeNameChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValidateFactTypeNameForObjectTypeNameChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// ChangeRule: typeof(ObjectType)
			/// /// </summary>
			/// private static void ValidateFactTypeNameForObjectTypeNameChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ValidateFactTypeNameForObjectTypeNameChangeRule");
				FactType.ValidateFactTypeNameForObjectTypeNameChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ValidateFactTypeNameForObjectTypeNameChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Reading), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValidateFactTypeNameForReadingChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValidateFactTypeNameForReadingChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// ChangeRule: typeof(Reading)
			/// /// </summary>
			/// private static void ValidateFactTypeNameForReadingChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ValidateFactTypeNameForReadingChangeRule");
				FactType.ValidateFactTypeNameForReadingChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ValidateFactTypeNameForReadingChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasReadingOrder), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValidateFactTypeNameForReadingOrderReorderRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValidateFactTypeNameForReadingOrderReorderRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// RolePlayerPositionChangeRule: typeof(FactTypeHasReadingOrder)
			/// /// </summary>
			/// private static void ValidateFactTypeNameForReadingOrderReorderRule(RolePlayerOrderChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerPositionChanged(Microsoft.VisualStudio.Modeling.RolePlayerOrderChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ValidateFactTypeNameForReadingOrderReorderRule");
				FactType.ValidateFactTypeNameForReadingOrderReorderRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ValidateFactTypeNameForReadingOrderReorderRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReadingOrderHasReading), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValidateFactTypeNameForReadingReorderRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValidateFactTypeNameForReadingReorderRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// RolePlayerPositionChangeRule: typeof(ReadingOrderHasReading)
			/// /// </summary>
			/// private static void ValidateFactTypeNameForReadingReorderRule(RolePlayerOrderChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerPositionChanged(Microsoft.VisualStudio.Modeling.RolePlayerOrderChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ValidateFactTypeNameForReadingReorderRule");
				FactType.ValidateFactTypeNameForReadingReorderRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ValidateFactTypeNameForReadingReorderRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValidateFactTypeNameForRolePlayerAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValidateFactTypeNameForRolePlayerAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// AddRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void ValidateFactTypeNameForRolePlayerAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ValidateFactTypeNameForRolePlayerAddedRule");
				FactType.ValidateFactTypeNameForRolePlayerAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ValidateFactTypeNameForRolePlayerAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValidateFactTypeNameForRolePlayerDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValidateFactTypeNameForRolePlayerDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// DeleteRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void ValidateFactTypeNameForRolePlayerDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ValidateFactTypeNameForRolePlayerDeleteRule");
				FactType.ValidateFactTypeNameForRolePlayerDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ValidateFactTypeNameForRolePlayerDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValidateFactTypeNameForRolePlayerRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValidateFactTypeNameForRolePlayerRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactType
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void ValidateFactTypeNameForRolePlayerRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ValidateFactTypeNameForRolePlayerRolePlayerChangeRule");
				FactType.ValidateFactTypeNameForRolePlayerRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.FactType.ValidateFactTypeNameForRolePlayerRolePlayerChangeRule");
			}
		}
	}
	#endregion // Rule classes for FactType
	#region Rule classes for FactType.UnaryBinarizationUtility
	partial class FactType
	{
		partial class UnaryBinarizationUtility
		{
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ConstraintRoleSequenceHasRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ConstraintRoleSequenceHasRoleAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility
				/// /// <summary>
				/// /// AddRule: typeof(ConstraintRoleSequenceHasRole)
				/// /// </summary>
				/// private static void ConstraintRoleSequenceHasRoleAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility.ConstraintRoleSequenceHasRoleAddedRule");
					UnaryBinarizationUtility.ConstraintRoleSequenceHasRoleAddedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility.ConstraintRoleSequenceHasRoleAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ConstraintRoleSequenceHasRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ConstraintRoleSequenceHasRoleDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility
				/// /// <summary>
				/// /// DeleteRule: typeof(ConstraintRoleSequenceHasRole)
				/// /// </summary>
				/// private static void ConstraintRoleSequenceHasRoleDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility.ConstraintRoleSequenceHasRoleDeletedRule");
					UnaryBinarizationUtility.ConstraintRoleSequenceHasRoleDeletedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility.ConstraintRoleSequenceHasRoleDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeHasRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeHasRoleAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility
				/// /// <summary>
				/// /// AddRule: typeof(FactTypeHasRole)
				/// /// </summary>
				/// private static void FactTypeHasRoleAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility.FactTypeHasRoleAddedRule");
					UnaryBinarizationUtility.FactTypeHasRoleAddedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility.FactTypeHasRoleAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeHasRoleDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeHasRoleDeletingRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility
				/// /// <summary>
				/// /// DeletingRule: typeof(FactTypeHasRole)
				/// /// </summary>
				/// private static void FactTypeHasRoleDeletingRule(ElementDeletingEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility.FactTypeHasRoleDeletingRule");
					UnaryBinarizationUtility.FactTypeHasRoleDeletingRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility.FactTypeHasRoleDeletingRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeNameChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeNameChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility
				/// /// <summary>
				/// /// ChangeRule: typeof(FactType)
				/// /// </summary>
				/// private static void FactTypeNameChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility.FactTypeNameChangedRule");
					UnaryBinarizationUtility.FactTypeNameChangedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility.FactTypeNameChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ObjectTypePlaysRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ObjectTypePlaysRoleAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility
				/// /// <summary>
				/// /// AddRule: typeof(ObjectTypePlaysRole)
				/// /// </summary>
				/// private static void ObjectTypePlaysRoleAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility.ObjectTypePlaysRoleAddedRule");
					UnaryBinarizationUtility.ObjectTypePlaysRoleAddedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility.ObjectTypePlaysRoleAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ObjectTypePlaysRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ObjectTypePlaysRoleDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility
				/// /// <summary>
				/// /// DeleteRule: typeof(ObjectTypePlaysRole)
				/// /// </summary>
				/// private static void ObjectTypePlaysRoleDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility.ObjectTypePlaysRoleDeletedRule");
					UnaryBinarizationUtility.ObjectTypePlaysRoleDeletedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility.ObjectTypePlaysRoleDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ObjectTypePlaysRoleRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ObjectTypePlaysRoleRolePlayerChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility
				/// /// <summary>
				/// /// RolePlayerChangeRule: typeof(ObjectTypePlaysRole)
				/// /// </summary>
				/// private static void ObjectTypePlaysRoleRolePlayerChangedRule(RolePlayerChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility.ObjectTypePlaysRoleRolePlayerChangedRule");
					UnaryBinarizationUtility.ObjectTypePlaysRoleRolePlayerChangedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility.ObjectTypePlaysRoleRolePlayerChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Role), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class RoleNameChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public RoleNameChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility
				/// /// <summary>
				/// /// ChangeRule: typeof(Role)
				/// /// </summary>
				/// private static void RoleNameChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility.RoleNameChangedRule");
					UnaryBinarizationUtility.RoleNameChangedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactType.UnaryBinarizationUtility.RoleNameChangedRule");
				}
			}
		}
	}
	#endregion // Rule classes for FactType.UnaryBinarizationUtility
	#region Rule classes for FactTypeDerivationExpression
	partial class FactTypeDerivationExpression
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeDerivationExpression), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeDerivationExpressionChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeDerivationExpressionChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactTypeDerivationExpression
			/// /// <summary>
			/// /// ChangeRule: typeof(FactTypeDerivationExpression)
			/// /// </summary>
			/// private static void FactTypeDerivationExpressionChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactTypeDerivationExpression.FactTypeDerivationExpressionChangeRule");
				FactTypeDerivationExpression.FactTypeDerivationExpressionChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactTypeDerivationExpression.FactTypeDerivationExpressionChangeRule");
			}
		}
	}
	#endregion // Rule classes for FactTypeDerivationExpression
	#region Rule classes for FactTypeInstance
	partial class FactTypeInstance
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasFactTypeInstance), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeHasFactTypeInstanceAddedClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeHasFactTypeInstanceAddedClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(FactTypeHasFactTypeInstance)
			/// /// </summary>
			/// private static void FactTypeHasFactTypeInstanceAdded(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactTypeInstance.FactTypeHasFactTypeInstanceAdded");
				FactTypeInstance.FactTypeHasFactTypeInstanceAdded(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactTypeInstance.FactTypeHasFactTypeInstanceAdded");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeHasRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeHasRoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(FactTypeHasRole)
			/// /// </summary>
			/// private static void FactTypeHasRoleAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactTypeInstance.FactTypeHasRoleAddedRule");
				FactTypeInstance.FactTypeHasRoleAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactTypeInstance.FactTypeHasRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeHasRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeHasRoleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactTypeInstance
			/// /// <summary>
			/// /// DeleteRule: typeof(FactTypeHasRole)
			/// /// </summary>
			/// private static void FactTypeHasRoleDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactTypeInstance.FactTypeHasRoleDeletedRule");
				FactTypeInstance.FactTypeHasRoleDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactTypeInstance.FactTypeHasRoleDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeInstanceHasRoleInstance), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeInstanceHasRoleInstanceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeInstanceHasRoleInstanceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(FactTypeInstanceHasRoleInstance)
			/// /// </summary>
			/// private static void FactTypeInstanceHasRoleInstanceAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactTypeInstance.FactTypeInstanceHasRoleInstanceAddedRule");
				FactTypeInstance.FactTypeInstanceHasRoleInstanceAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactTypeInstance.FactTypeInstanceHasRoleInstanceAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeInstanceHasRoleInstance), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=Neumont.Tools.Modeling.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class FactTypeInstanceHasRoleInstanceDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeInstanceHasRoleInstanceDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactTypeInstance
			/// /// <summary>
			/// /// DeleteRule: typeof(FactTypeInstanceHasRoleInstance), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void FactTypeInstanceHasRoleInstanceDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactTypeInstance.FactTypeInstanceHasRoleInstanceDeletedRule");
				FactTypeInstance.FactTypeInstanceHasRoleInstanceDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactTypeInstance.FactTypeInstanceHasRoleInstanceDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ImpliedBooleanRolePlayerDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ImpliedBooleanRolePlayerDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FactTypeInstance
			/// /// <summary>
			/// /// DeleteRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void ImpliedBooleanRolePlayerDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactTypeInstance.ImpliedBooleanRolePlayerDeletedRule");
				FactTypeInstance.ImpliedBooleanRolePlayerDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FactTypeInstance.ImpliedBooleanRolePlayerDeletedRule");
			}
		}
	}
	#endregion // Rule classes for FactTypeInstance
	#region Rule classes for FrequencyConstraint
	partial class FrequencyConstraint
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FrequencyConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FrequencyConstraintMinMaxRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FrequencyConstraintMinMaxRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FrequencyConstraint
			/// /// <summary>
			/// /// ChangeRule: typeof(FrequencyConstraint)
			/// /// </summary>
			/// private static void FrequencyConstraintMinMaxRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FrequencyConstraint.FrequencyConstraintMinMaxRule");
				FrequencyConstraint.FrequencyConstraintMinMaxRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FrequencyConstraint.FrequencyConstraintMinMaxRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactSetConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RemoveContradictionErrorsWithFactTypeRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RemoveContradictionErrorsWithFactTypeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.FrequencyConstraint
			/// /// <summary>
			/// /// DeleteRule: typeof(FactSetConstraint)
			/// /// </summary>
			/// private static void RemoveContradictionErrorsWithFactTypeRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FrequencyConstraint.RemoveContradictionErrorsWithFactTypeRule");
				FrequencyConstraint.RemoveContradictionErrorsWithFactTypeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.FrequencyConstraint.RemoveContradictionErrorsWithFactTypeRule");
			}
		}
	}
	#endregion // Rule classes for FrequencyConstraint
	#region Rule classes for MandatoryConstraint
	partial class MandatoryConstraint
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(MandatoryConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class MandatoryConstraintChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public MandatoryConstraintChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.MandatoryConstraint
			/// /// <summary>
			/// /// ChangeRule: typeof(MandatoryConstraint)
			/// /// </summary>
			/// private static void MandatoryConstraintChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.MandatoryConstraint.MandatoryConstraintChangeRule");
				MandatoryConstraint.MandatoryConstraintChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.MandatoryConstraint.MandatoryConstraintChangeRule");
			}
		}
	}
	#endregion // Rule classes for MandatoryConstraint
	#region Rule classes for ModelError
	partial class ModelError
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMModel), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class SynchronizeErrorTextForModelRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SynchronizeErrorTextForModelRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ModelError
			/// /// <summary>
			/// /// ChangeRule: typeof(ORMModel)
			/// /// </summary>
			/// private static void SynchronizeErrorTextForModelRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ModelError.SynchronizeErrorTextForModelRule");
				ModelError.SynchronizeErrorTextForModelRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ModelError.SynchronizeErrorTextForModelRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMNamedElement), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class SynchronizeErrorTextForOwnerRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SynchronizeErrorTextForOwnerRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ModelError
			/// /// <summary>
			/// /// ChangeRule: typeof(ORMNamedElement)
			/// /// </summary>
			/// private static void SynchronizeErrorTextForOwnerRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ModelError.SynchronizeErrorTextForOwnerRule");
				ModelError.SynchronizeErrorTextForOwnerRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ModelError.SynchronizeErrorTextForOwnerRule");
			}
		}
	}
	#endregion // Rule classes for ModelError
	#region Rule classes for NameGenerator
	partial class NameGenerator
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(NameGenerator), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed partial class SynchronizedRefinementsPropertyChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SynchronizedRefinementsPropertyChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.NameGenerator
			/// partial class SynchronizedRefinementsPropertyChangedRuleClass
			/// {
			/// 	/// <summary>
			/// 	/// ChangeRule: typeof(NameGenerator)
			/// 	/// </summary>
			/// 	private void SynchronizedRefinementsPropertyChangedRule(ElementPropertyChangedEventArgs e)
			/// 	{
			/// 	}
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.NameGenerator.SynchronizedRefinementsPropertyChangedRule");
				this.SynchronizedRefinementsPropertyChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.NameGenerator.SynchronizedRefinementsPropertyChangedRule");
			}
		}
	}
	#endregion // Rule classes for NameGenerator
	#region Rule classes for Note
	partial class Note
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Note), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class NoteChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public NoteChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Note
			/// /// <summary>
			/// /// ChangeRule: typeof(Note)
			/// /// </summary>
			/// private static void NoteChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Note.NoteChangeRule");
				Note.NoteChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Note.NoteChangeRule");
			}
		}
	}
	#endregion // Rule classes for Note
	#region Rule classes for Objectification
	partial class Objectification
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectificationImpliesFactType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ImpliedFactTypeAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ImpliedFactTypeAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Objectification
			/// /// <summary>
			/// /// AddRule: typeof(ObjectificationImpliesFactType)
			/// /// </summary>
			/// private static void ImpliedFactTypeAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ImpliedFactTypeAddRule");
				Objectification.ImpliedFactTypeAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ImpliedFactTypeAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ImpliedObjectificationConstraintRoleSequenceHasRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ImpliedObjectificationConstraintRoleSequenceHasRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Objectification
			/// /// <summary>
			/// /// AddRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void ImpliedObjectificationConstraintRoleSequenceHasRoleAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ImpliedObjectificationConstraintRoleSequenceHasRoleAddRule");
				Objectification.ImpliedObjectificationConstraintRoleSequenceHasRoleAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ImpliedObjectificationConstraintRoleSequenceHasRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ImpliedObjectificationConstraintRoleSequenceHasRoleDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ImpliedObjectificationConstraintRoleSequenceHasRoleDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Objectification
			/// /// <summary>
			/// /// DeletingRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void ImpliedObjectificationConstraintRoleSequenceHasRoleDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ImpliedObjectificationConstraintRoleSequenceHasRoleDeletingRule");
				Objectification.ImpliedObjectificationConstraintRoleSequenceHasRoleDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ImpliedObjectificationConstraintRoleSequenceHasRoleDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ImpliedObjectificationFactTypeHasRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ImpliedObjectificationFactTypeHasRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Objectification
			/// /// <summary>
			/// /// AddRule: typeof(FactTypeHasRole)
			/// /// </summary>
			/// private static void ImpliedObjectificationFactTypeHasRoleAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ImpliedObjectificationFactTypeHasRoleAddRule");
				Objectification.ImpliedObjectificationFactTypeHasRoleAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ImpliedObjectificationFactTypeHasRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ImpliedObjectificationFactTypeHasRoleDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ImpliedObjectificationFactTypeHasRoleDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Objectification
			/// /// <summary>
			/// /// DeletingRule: typeof(FactTypeHasRole)
			/// /// </summary>
			/// private static void ImpliedObjectificationFactTypeHasRoleDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ImpliedObjectificationFactTypeHasRoleDeletingRule");
				Objectification.ImpliedObjectificationFactTypeHasRoleDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ImpliedObjectificationFactTypeHasRoleDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ImpliedObjectificationIsImpliedChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ImpliedObjectificationIsImpliedChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Objectification
			/// /// <summary>
			/// /// ChangeRule: typeof(Objectification)
			/// /// </summary>
			/// private static void ImpliedObjectificationIsImpliedChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ImpliedObjectificationIsImpliedChangeRule");
				Objectification.ImpliedObjectificationIsImpliedChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ImpliedObjectificationIsImpliedChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ImpliedObjectificationObjectifyingTypeIsIndependentChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ImpliedObjectificationObjectifyingTypeIsIndependentChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Objectification
			/// /// <summary>
			/// /// ChangeRule: typeof(ObjectType)
			/// /// </summary>
			/// private static void ImpliedObjectificationObjectifyingTypeIsIndependentChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ImpliedObjectificationObjectifyingTypeIsIndependentChangeRule");
				Objectification.ImpliedObjectificationObjectifyingTypeIsIndependentChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ImpliedObjectificationObjectifyingTypeIsIndependentChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ImpliedObjectificationObjectifyingTypePlaysRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ImpliedObjectificationObjectifyingTypePlaysRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Objectification
			/// /// <summary>
			/// /// AddRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void ImpliedObjectificationObjectifyingTypePlaysRoleAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ImpliedObjectificationObjectifyingTypePlaysRoleAddRule");
				Objectification.ImpliedObjectificationObjectifyingTypePlaysRoleAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ImpliedObjectificationObjectifyingTypePlaysRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(UniquenessConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ImpliedObjectificationUniquenessConstraintChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ImpliedObjectificationUniquenessConstraintChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Objectification
			/// /// <summary>
			/// /// ChangeRule: typeof(UniquenessConstraint)
			/// /// </summary>
			/// private static void ImpliedObjectificationUniquenessConstraintChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ImpliedObjectificationUniquenessConstraintChangeRule");
				Objectification.ImpliedObjectificationUniquenessConstraintChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ImpliedObjectificationUniquenessConstraintChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class InternalConstraintChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public InternalConstraintChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Objectification
			/// /// <summary>
			/// /// ChangeRule: typeof(SetConstraint)
			/// /// </summary>
			/// private static void InternalConstraintChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.InternalConstraintChangeRule");
				Objectification.InternalConstraintChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.InternalConstraintChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectificationAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectificationAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Objectification
			/// /// <summary>
			/// /// AddRule: typeof(Objectification)
			/// /// </summary>
			/// private static void ObjectificationAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ObjectificationAddRule");
				Objectification.ObjectificationAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ObjectificationAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectificationDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectificationDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Objectification
			/// /// <summary>
			/// /// DeletingRule: typeof(Objectification)
			/// /// </summary>
			/// private static void ObjectificationDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ObjectificationDeletingRule");
				Objectification.ObjectificationDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ObjectificationDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectificationRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectificationRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Objectification
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(Objectification)
			/// /// </summary>
			/// private static void ObjectificationRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ObjectificationRolePlayerChangeRule");
				Objectification.ObjectificationRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.ObjectificationRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class PreferredIdentifierDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Objectification
			/// /// <summary>
			/// /// DeletingRule: typeof(EntityTypeHasPreferredIdentifier)
			/// /// </summary>
			/// private static void PreferredIdentifierDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.PreferredIdentifierDeletingRule");
				Objectification.PreferredIdentifierDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.PreferredIdentifierDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class PreferredIdentifierRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Objectification
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(EntityTypeHasPreferredIdentifier)
			/// /// </summary>
			/// private static void PreferredIdentifierRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.PreferredIdentifierRolePlayerChangeRule");
				Objectification.PreferredIdentifierRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.PreferredIdentifierRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactSetConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Objectification
			/// /// <summary>
			/// /// AddRule: typeof(FactTypeHasRole)
			/// /// AddRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// AddRule: typeof(FactSetConstraint)
			/// /// </summary>
			/// private static void RoleAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.RoleAddRule");
				Objectification.RoleAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.RoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed partial class RoleDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Objectification
			/// partial class RoleDeletingRuleClass
			/// {
			/// 	/// <summary>
			/// 	/// DeletingRule: typeof(FactTypeHasRole)
			/// 	/// DeletingRule: typeof(ConstraintRoleSequenceHasRole)
			/// 	/// </summary>
			/// 	private void RoleDeletingRule(ElementDeletingEventArgs e)
			/// 	{
			/// 	}
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.RoleDeletingRule");
				this.RoleDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.RoleDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Objectification
			/// /// <summary>
			/// /// AddRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void RolePlayerAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.RolePlayerAddRule");
				Objectification.RolePlayerAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.RolePlayerAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Objectification
			/// /// <summary>
			/// /// DeletingRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void RolePlayerDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.RolePlayerDeletingRule");
				Objectification.RolePlayerDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.RolePlayerDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Objectification
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void RolePlayerRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.RolePlayerRolePlayerChangeRule");
				Objectification.RolePlayerRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.RolePlayerRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ModelHasSetConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class UniquenessConstraintAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public UniquenessConstraintAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Objectification
			/// /// <summary>
			/// /// AddRule: typeof(ModelHasSetConstraint)
			/// /// </summary>
			/// private static void UniquenessConstraintAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.UniquenessConstraintAddRule");
				Objectification.UniquenessConstraintAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.UniquenessConstraintAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ModelHasSetConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class UniquenessConstraintDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public UniquenessConstraintDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Objectification
			/// /// <summary>
			/// /// DeletingRule: typeof(ModelHasSetConstraint)
			/// /// </summary>
			/// private static void UniquenessConstraintDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.UniquenessConstraintDeletingRule");
				Objectification.UniquenessConstraintDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Objectification.UniquenessConstraintDeletingRule");
			}
		}
	}
	#endregion // Rule classes for Objectification
	#region Rule classes for ObjectType
	partial class ObjectType
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class CheckForIncompatibleRelationshipAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public CheckForIncompatibleRelationshipAddRuleClass()
			{
				base.IsEnabled = false;
			}
			public override bool FireBefore
			{
				[System.Diagnostics.DebuggerStepThrough()]
				get
				{
					return true;
				}
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectType
			/// /// <summary>
			/// /// AddRule: typeof(Objectification)
			/// /// AddRule: typeof(ValueTypeHasDataType)
			/// /// AddRule: typeof(ObjectTypePlaysRole)
			/// /// AddRule: typeof(FactTypeHasRole)
			/// /// </summary>
			/// private static void CheckForIncompatibleRelationshipAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.CheckForIncompatibleRelationshipAddRule");
				ObjectType.CheckForIncompatibleRelationshipAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.CheckForIncompatibleRelationshipAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class CheckForIncompatibleRelationshipRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public CheckForIncompatibleRelationshipRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			public override bool FireBefore
			{
				[System.Diagnostics.DebuggerStepThrough()]
				get
				{
					return true;
				}
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectType
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(Objectification)
			/// /// RolePlayerChangeRule: typeof(ValueTypeHasDataType)
			/// /// RolePlayerChangeRule: typeof(ObjectTypePlaysRole)
			/// /// RolePlayerChangeRule: typeof(FactTypeHasRole)
			/// /// </summary>
			/// private static void CheckForIncompatibleRelationshipRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.CheckForIncompatibleRelationshipRolePlayerChangeRule");
				ObjectType.CheckForIncompatibleRelationshipRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.CheckForIncompatibleRelationshipRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class CheckIsIndependentRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public CheckIsIndependentRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectType
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void CheckIsIndependentRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.CheckIsIndependentRolePlayerChangeRule");
				ObjectType.CheckIsIndependentRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.CheckIsIndependentRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectType
			/// /// <summary>
			/// /// AddRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void ConstraintRoleAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.ConstraintRoleAddedRule");
				ObjectType.ConstraintRoleAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.ConstraintRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRoleDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectType
			/// /// <summary>
			/// /// DeletingRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void ConstraintRoleDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.ConstraintRoleDeletingRule");
				ObjectType.ConstraintRoleDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.ConstraintRoleDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ExclusionConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ExclusionModalityChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ExclusionModalityChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectType
			/// /// <summary>
			/// /// ChangeRule: typeof(ExclusionConstraint)
			/// /// </summary>
			/// private static void ExclusionModalityChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.ExclusionModalityChangeRule");
				ObjectType.ExclusionModalityChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.ExclusionModalityChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetComparisonConstraintHasRoleSequence), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ExclusionSequenceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ExclusionSequenceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectType
			/// /// <summary>
			/// /// AddRule: typeof(SetComparisonConstraintHasRoleSequence)
			/// /// </summary>
			/// private static void ExclusionSequenceAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.ExclusionSequenceAddedRule");
				ObjectType.ExclusionSequenceAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.ExclusionSequenceAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(MandatoryConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class MandatoryModalityChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public MandatoryModalityChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectType
			/// /// <summary>
			/// /// ChangeRule: typeof(MandatoryConstraint)
			/// /// </summary>
			/// private static void MandatoryModalityChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.MandatoryModalityChangeRule");
				ObjectType.MandatoryModalityChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.MandatoryModalityChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ModelHasObjectType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectTypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectTypeAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectType
			/// /// <summary>
			/// /// AddRule: typeof(ModelHasObjectType)
			/// /// </summary>
			/// private static void ObjectTypeAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.ObjectTypeAddedRule");
				ObjectType.ObjectTypeAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.ObjectTypeAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectTypeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectTypeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectType
			/// /// <summary>
			/// /// ChangeRule: typeof(ObjectType)
			/// /// </summary>
			/// private static void ObjectTypeChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.ObjectTypeChangeRule");
				ObjectType.ObjectTypeChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.ObjectTypeChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectTypeDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectTypeDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectType
			/// /// <summary>
			/// /// DeletingRule: typeof(ObjectType)
			/// /// </summary>
			/// private static void ObjectTypeDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.ObjectTypeDeletingRule");
				ObjectType.ObjectTypeDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.ObjectTypeDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SubtypeFact), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed partial class SubtypeFactChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SubtypeFactChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectType
			/// partial class SubtypeFactChangeRuleClass
			/// {
			/// 	/// <summary>
			/// 	/// ChangeRule: typeof(SubtypeFact)
			/// 	/// </summary>
			/// 	private void SubtypeFactChangeRule(ElementPropertyChangedEventArgs e)
			/// 	{
			/// 	}
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.SubtypeFactChangeRule");
				this.SubtypeFactChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.SubtypeFactChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class SupertypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SupertypeAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectType
			/// /// <summary>
			/// /// AddRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void SupertypeAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.SupertypeAddedRule");
				ObjectType.SupertypeAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.SupertypeAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class SupertypeDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SupertypeDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectType
			/// /// <summary>
			/// /// DeleteRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void SupertypeDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.SupertypeDeleteRule");
				ObjectType.SupertypeDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.SupertypeDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class SupertypeDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SupertypeDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectType
			/// /// <summary>
			/// /// DeletingRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void SupertypeDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.SupertypeDeletingRule");
				ObjectType.SupertypeDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.SupertypeDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class UnspecifiedDataTypeAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public UnspecifiedDataTypeAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectType
			/// /// <summary>
			/// /// AddRule: typeof(ValueTypeHasDataType)
			/// /// </summary>
			/// private static void UnspecifiedDataTypeAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.UnspecifiedDataTypeAddRule");
				ObjectType.UnspecifiedDataTypeAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.UnspecifiedDataTypeAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class UnspecifiedDataTypeRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public UnspecifiedDataTypeRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectType
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ValueTypeHasDataType)
			/// /// </summary>
			/// private static void UnspecifiedDataTypeRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.UnspecifiedDataTypeRolePlayerChangeRule");
				ObjectType.UnspecifiedDataTypeRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.UnspecifiedDataTypeRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class VerifyReferenceSchemeAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public VerifyReferenceSchemeAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectType
			/// /// <summary>
			/// /// AddRule: typeof(EntityTypeHasPreferredIdentifier)
			/// /// </summary>
			/// private static void VerifyReferenceSchemeAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.VerifyReferenceSchemeAddRule");
				ObjectType.VerifyReferenceSchemeAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.VerifyReferenceSchemeAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class VerifyReferenceSchemeDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public VerifyReferenceSchemeDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectType
			/// /// <summary>
			/// /// DeleteRule: typeof(EntityTypeHasPreferredIdentifier)
			/// /// </summary>
			/// private static void VerifyReferenceSchemeDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.VerifyReferenceSchemeDeleteRule");
				ObjectType.VerifyReferenceSchemeDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.VerifyReferenceSchemeDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class VerifyReferenceSchemeRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public VerifyReferenceSchemeRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectType
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(EntityTypeHasPreferredIdentifier)
			/// /// </summary>
			/// private static void VerifyReferenceSchemeRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.VerifyReferenceSchemeRolePlayerChangeRule");
				ObjectType.VerifyReferenceSchemeRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.VerifyReferenceSchemeRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class VerifyValueTypeHasDataTypeAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public VerifyValueTypeHasDataTypeAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectType
			/// /// <summary>
			/// /// AddRule: typeof(ValueTypeHasDataType)
			/// /// </summary>
			/// private static void VerifyValueTypeHasDataTypeAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.VerifyValueTypeHasDataTypeAddRule");
				ObjectType.VerifyValueTypeHasDataTypeAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.VerifyValueTypeHasDataTypeAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class VerifyValueTypeHasDataTypeDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public VerifyValueTypeHasDataTypeDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectType
			/// /// <summary>
			/// /// DeleteRule: typeof(ValueTypeHasDataType)
			/// /// </summary>
			/// private static void VerifyValueTypeHasDataTypeDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.VerifyValueTypeHasDataTypeDeleteRule");
				ObjectType.VerifyValueTypeHasDataTypeDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectType.VerifyValueTypeHasDataTypeDeleteRule");
			}
		}
	}
	#endregion // Rule classes for ObjectType
	#region Rule classes for ObjectTypeInstance
	partial class ObjectTypeInstance
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRoleSequenceHasRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleSequenceHasRoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void ConstraintRoleSequenceHasRoleAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.ConstraintRoleSequenceHasRoleAddedRule");
				ObjectTypeInstance.ConstraintRoleSequenceHasRoleAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.ConstraintRoleSequenceHasRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRoleSequenceHasRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleSequenceHasRoleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// DeleteRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void ConstraintRoleSequenceHasRoleDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.ConstraintRoleSequenceHasRoleDeletedRule");
				ObjectTypeInstance.ConstraintRoleSequenceHasRoleDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.ConstraintRoleSequenceHasRoleDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeInstanceHasRoleInstance), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EntityTypeInstanceHasRoleInstanceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EntityTypeInstanceHasRoleInstanceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(EntityTypeInstanceHasRoleInstance)
			/// /// </summary>
			/// private static void EntityTypeInstanceHasRoleInstanceAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.EntityTypeInstanceHasRoleInstanceAddedRule");
				ObjectTypeInstance.EntityTypeInstanceHasRoleInstanceAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.EntityTypeInstanceHasRoleInstanceAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeInstanceHasRoleInstance), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EntityTypeInstanceHasRoleInstanceDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EntityTypeInstanceHasRoleInstanceDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// DeleteRule: typeof(EntityTypeInstanceHasRoleInstance)
			/// /// </summary>
			/// private static void EntityTypeInstanceHasRoleInstanceDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.EntityTypeInstanceHasRoleInstanceDeletedRule");
				ObjectTypeInstance.EntityTypeInstanceHasRoleInstanceDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.EntityTypeInstanceHasRoleInstanceDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeInstanceHasRoleInstance), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EntityTypeInstanceHasRoleInstanceRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EntityTypeInstanceHasRoleInstanceRolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(EntityTypeInstanceHasRoleInstance)
			/// /// </summary>
			/// private static void EntityTypeInstanceHasRoleInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.EntityTypeInstanceHasRoleInstanceRolePlayerChangedRule");
				ObjectTypeInstance.EntityTypeInstanceHasRoleInstanceRolePlayerChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.EntityTypeInstanceHasRoleInstanceRolePlayerChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(MandatoryConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class MandatoryConstraintChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public MandatoryConstraintChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// ChangeRule: typeof(MandatoryConstraint)
			/// /// </summary>
			/// private static void MandatoryConstraintChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.MandatoryConstraintChangedRule");
				ObjectTypeInstance.MandatoryConstraintChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.MandatoryConstraintChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RoleInstance), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleInstanceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleInstanceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(RoleInstance)
			/// /// </summary>
			/// private static void RoleInstanceAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.RoleInstanceAddedRule");
				ObjectTypeInstance.RoleInstanceAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.RoleInstanceAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RoleInstance), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleInstanceDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleInstanceDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// DeleteRule: typeof(RoleInstance)
			/// /// </summary>
			/// private static void RoleInstanceDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.RoleInstanceDeletedRule");
				ObjectTypeInstance.RoleInstanceDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.RoleInstanceDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void RolePlayerAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.RolePlayerAddedRule");
				ObjectTypeInstance.RolePlayerAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.RolePlayerAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// DeleteRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void RolePlayerDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.RolePlayerDeletedRule");
				ObjectTypeInstance.RolePlayerDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.RolePlayerDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerRolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void RolePlayerRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.RolePlayerRolePlayerChangedRule");
				ObjectTypeInstance.RolePlayerRolePlayerChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.RolePlayerRolePlayerChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeInstance), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValueTypeInstanceValueChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueTypeInstanceValueChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// ChangeRule: typeof(ValueTypeInstance)
			/// /// </summary>
			/// private static void ValueTypeInstanceValueChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.ValueTypeInstanceValueChangedRule");
				ObjectTypeInstance.ValueTypeInstanceValueChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ObjectTypeInstance.ValueTypeInstanceValueChangedRule");
			}
		}
	}
	#endregion // Rule classes for ObjectTypeInstance
	#region Rule classes for ORMModel
	partial class ORMModel
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetComparisonConstraintHasDuplicateNameError), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetConstraintHasDuplicateNameError), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueConstraintHasDuplicateNameError), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DuplicateConstraintNameConstraintDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DuplicateConstraintNameConstraintDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ORMModel
			/// /// <summary>
			/// /// DeleteRule: typeof(SetComparisonConstraintHasDuplicateNameError)
			/// /// DeleteRule: typeof(SetConstraintHasDuplicateNameError)
			/// /// DeleteRule: typeof(ValueConstraintHasDuplicateNameError)
			/// /// </summary>
			/// private static void DuplicateConstraintNameConstraintDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ORMModel.DuplicateConstraintNameConstraintDeleteRule");
				ORMModel.DuplicateConstraintNameConstraintDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ORMModel.DuplicateConstraintNameConstraintDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypeHasDuplicateNameError), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DuplicateObjectTypeNameObjectTypeDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DuplicateObjectTypeNameObjectTypeDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ORMModel
			/// /// <summary>
			/// /// DeleteRule: typeof(ObjectTypeHasDuplicateNameError)
			/// /// </summary>
			/// private static void DuplicateObjectTypeNameObjectTypeDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ORMModel.DuplicateObjectTypeNameObjectTypeDeleteRule");
				ORMModel.DuplicateObjectTypeNameObjectTypeDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ORMModel.DuplicateObjectTypeNameObjectTypeDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RecognizedPhraseHasDuplicateNameError), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DuplicateRecognizedPhraseDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DuplicateRecognizedPhraseDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ORMModel
			/// /// <summary>
			/// /// DeleteRule: typeof(RecognizedPhraseHasDuplicateNameError)
			/// /// </summary>
			/// private static void DuplicateRecognizedPhraseDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ORMModel.DuplicateRecognizedPhraseDeleteRule");
				ORMModel.DuplicateRecognizedPhraseDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ORMModel.DuplicateRecognizedPhraseDeleteRule");
			}
		}
	}
	#endregion // Rule classes for ORMModel
	#region Rule classes for Reading
	partial class Reading
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReadingOrderHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ReadingOrderHasRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReadingOrderHasRoleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Reading
			/// /// <summary>
			/// /// DeleteRule: typeof(ReadingOrderHasRole)
			/// /// </summary>
			/// private static void ReadingOrderHasRoleDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Reading.ReadingOrderHasRoleDeletedRule");
				Reading.ReadingOrderHasRoleDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Reading.ReadingOrderHasRoleDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Reading), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ReadingPropertiesChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReadingPropertiesChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Reading
			/// /// <summary>
			/// /// ChangeRule: typeof(Reading)
			/// /// </summary>
			/// private static void ReadingPropertiesChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Reading.ReadingPropertiesChangedRule");
				Reading.ReadingPropertiesChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Reading.ReadingPropertiesChangedRule");
			}
		}
	}
	#endregion // Rule classes for Reading
	#region Rule classes for RecognizedPhrase
	partial class RecognizedPhrase
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RecognizedPhraseHasAbbreviation), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=Neumont.Tools.Modeling.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class RecognizedPhraseHasAbbreviationDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RecognizedPhraseHasAbbreviationDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.RecognizedPhrase
			/// /// <summary>
			/// /// DeleteRule: typeof(RecognizedPhraseHasAbbreviation), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void RecognizedPhraseHasAbbreviationDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.RecognizedPhrase.RecognizedPhraseHasAbbreviationDeletedRule");
				RecognizedPhrase.RecognizedPhraseHasAbbreviationDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.RecognizedPhrase.RecognizedPhraseHasAbbreviationDeletedRule");
			}
		}
	}
	#endregion // Rule classes for RecognizedPhrase
	#region Rule classes for ReadingOrder
	partial class ReadingOrder
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReadingOrderHasReading), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=Neumont.Tools.Modeling.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class EnforceNoEmptyReadingOrderDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceNoEmptyReadingOrderDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ReadingOrder
			/// /// <summary>
			/// /// DeleteRule: typeof(ReadingOrderHasReading), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void EnforceNoEmptyReadingOrderDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ReadingOrder.EnforceNoEmptyReadingOrderDeleteRule");
				ReadingOrder.EnforceNoEmptyReadingOrderDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ReadingOrder.EnforceNoEmptyReadingOrderDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReadingOrderHasReading), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=Neumont.Tools.Modeling.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class EnforceNoEmptyReadingOrderRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceNoEmptyReadingOrderRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ReadingOrder
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ReadingOrderHasReading), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void EnforceNoEmptyReadingOrderRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ReadingOrder.EnforceNoEmptyReadingOrderRolePlayerChangeRule");
				ReadingOrder.EnforceNoEmptyReadingOrderRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ReadingOrder.EnforceNoEmptyReadingOrderRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeHasRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeHasRoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ReadingOrder
			/// /// <summary>
			/// /// AddRule: typeof(FactTypeHasRole)
			/// /// </summary>
			/// private static void FactTypeHasRoleAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ReadingOrder.FactTypeHasRoleAddedRule");
				ReadingOrder.FactTypeHasRoleAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ReadingOrder.FactTypeHasRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReadingOrderHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ReadingOrderHasRoleDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReadingOrderHasRoleDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ReadingOrder
			/// /// <summary>
			/// /// DeletingRule: typeof(ReadingOrderHasRole)
			/// /// </summary>
			/// private static void ReadingOrderHasRoleDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ReadingOrder.ReadingOrderHasRoleDeletingRule");
				ReadingOrder.ReadingOrderHasRoleDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ReadingOrder.ReadingOrderHasRoleDeletingRule");
			}
		}
	}
	#endregion // Rule classes for ReadingOrder
	#region Rule classes for ReferenceMode
	partial class ReferenceMode
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(CustomReferenceMode), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class CustomReferenceModeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public CustomReferenceModeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ReferenceMode
			/// /// <summary>
			/// /// ChangeRule: typeof(CustomReferenceMode)
			/// /// </summary>
			/// private static void CustomReferenceModeChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ReferenceMode.CustomReferenceModeChangeRule");
				ReferenceMode.CustomReferenceModeChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ReferenceMode.CustomReferenceModeChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ModelHasReferenceMode), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=Neumont.Tools.Modeling.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class ReferenceModeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReferenceModeAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ReferenceMode
			/// /// <summary>
			/// /// AddRule: typeof(ModelHasReferenceMode), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void ReferenceModeAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ReferenceMode.ReferenceModeAddedRule");
				ReferenceMode.ReferenceModeAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ReferenceMode.ReferenceModeAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReferenceModeKind), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ReferenceModeKindChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReferenceModeKindChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ReferenceMode
			/// /// <summary>
			/// /// ChangeRule: typeof(ReferenceModeKind)
			/// /// </summary>
			/// private static void ReferenceModeKindChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ReferenceMode.ReferenceModeKindChangeRule");
				ReferenceMode.ReferenceModeKindChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ReferenceMode.ReferenceModeKindChangeRule");
			}
		}
	}
	#endregion // Rule classes for ReferenceMode
	#region Rule classes for ReferenceModeHasReferenceModeKind
	partial class ReferenceModeHasReferenceModeKind
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReferenceModeHasReferenceModeKind), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ReferenceModeHasReferenceModeKindDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReferenceModeHasReferenceModeKindDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ReferenceModeHasReferenceModeKind
			/// /// <summary>
			/// /// DeletingRule: typeof(ReferenceModeHasReferenceModeKind)
			/// /// </summary>
			/// private static void ReferenceModeHasReferenceModeKindDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ReferenceModeHasReferenceModeKind.ReferenceModeHasReferenceModeKindDeletingRule");
				ReferenceModeHasReferenceModeKind.ReferenceModeHasReferenceModeKindDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ReferenceModeHasReferenceModeKind.ReferenceModeHasReferenceModeKindDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReferenceModeHasReferenceModeKind), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ReferenceModeHasReferenceModeKindRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReferenceModeHasReferenceModeKindRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ReferenceModeHasReferenceModeKind
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ReferenceModeHasReferenceModeKind)
			/// /// </summary>
			/// private static void ReferenceModeHasReferenceModeKindRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ReferenceModeHasReferenceModeKind.ReferenceModeHasReferenceModeKindRolePlayerChangeRule");
				ReferenceModeHasReferenceModeKind.ReferenceModeHasReferenceModeKindRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ReferenceModeHasReferenceModeKind.ReferenceModeHasReferenceModeKindRolePlayerChangeRule");
			}
		}
	}
	#endregion // Rule classes for ReferenceModeHasReferenceModeKind
	#region Rule classes for RingConstraint
	partial class RingConstraint
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RingConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RingConstraintTypeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RingConstraintTypeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.RingConstraint
			/// /// <summary>
			/// /// ChangeRule: typeof(RingConstraint)
			/// /// </summary>
			/// private static void RingConstraintTypeChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.RingConstraint.RingConstraintTypeChangeRule");
				RingConstraint.RingConstraintTypeChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.RingConstraint.RingConstraintTypeChangeRule");
			}
		}
	}
	#endregion // Rule classes for RingConstraint
	#region Rule classes for Role
	partial class Role
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRoleSequenceHasRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleSequenceHasRoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Role
			/// /// <summary>
			/// /// AddRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void ConstraintRoleSequenceHasRoleAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Role.ConstraintRoleSequenceHasRoleAddedRule");
				Role.ConstraintRoleSequenceHasRoleAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Role.ConstraintRoleSequenceHasRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRoleSequenceHasRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleSequenceHasRoleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Role
			/// /// <summary>
			/// /// DeleteRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void ConstraintRoleSequenceHasRoleDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Role.ConstraintRoleSequenceHasRoleDeletedRule");
				Role.ConstraintRoleSequenceHasRoleDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Role.ConstraintRoleSequenceHasRoleDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Role), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Role
			/// /// <summary>
			/// /// ChangeRule: typeof(Role)
			/// /// </summary>
			/// private static void RoleChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Role.RoleChangeRule");
				Role.RoleChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Role.RoleChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RoleInstance), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleInstanceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleInstanceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Role
			/// /// <summary>
			/// /// AddRule: typeof(RoleInstance)
			/// /// </summary>
			/// private static void RoleInstanceAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Role.RoleInstanceAddedRule");
				Role.RoleInstanceAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Role.RoleInstanceAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RoleInstance), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleInstanceDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleInstanceDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Role
			/// /// <summary>
			/// /// DeleteRule: typeof(RoleInstance)
			/// /// </summary>
			/// private static void RoleInstanceDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Role.RoleInstanceDeletedRule");
				Role.RoleInstanceDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Role.RoleInstanceDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RoleInstance), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleInstanceRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleInstanceRolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Role
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(RoleInstance)
			/// /// </summary>
			/// private static void RoleInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.Role.RoleInstanceRolePlayerChangedRule");
				Role.RoleInstanceRolePlayerChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.Role.RoleInstanceRolePlayerChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=Neumont.Tools.Modeling.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class RolePlayerRequiredAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerRequiredAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Role
			/// /// <summary>
			/// /// AddRule: typeof(ObjectTypePlaysRole), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void RolePlayerRequiredAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Role.RolePlayerRequiredAddRule");
				Role.RolePlayerRequiredAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Role.RolePlayerRequiredAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerRequiredDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerRequiredDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Role
			/// /// <summary>
			/// /// DeleteRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void RolePlayerRequiredDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Role.RolePlayerRequiredDeleteRule");
				Role.RolePlayerRequiredDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Role.RolePlayerRequiredDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerRequiredForNewRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerRequiredForNewRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Role
			/// /// <summary>
			/// /// AddRule: typeof(FactTypeHasRole)
			/// /// </summary>
			/// private static void RolePlayerRequiredForNewRoleAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Role.RolePlayerRequiredForNewRoleAddRule");
				Role.RolePlayerRequiredForNewRoleAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Role.RolePlayerRequiredForNewRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class UpdatedRolePlayerRequiredErrorsDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public UpdatedRolePlayerRequiredErrorsDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Role
			/// /// <summary>
			/// /// DeleteRule: typeof(FactTypeHasRole)
			/// /// </summary>
			/// private static void UpdatedRolePlayerRequiredErrorsDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Role.UpdatedRolePlayerRequiredErrorsDeleteRule");
				Role.UpdatedRolePlayerRequiredErrorsDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Role.UpdatedRolePlayerRequiredErrorsDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(UniquenessConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class UniquenessConstraintChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public UniquenessConstraintChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.Role
			/// /// <summary>
			/// /// ChangeRule: typeof(UniquenessConstraint)
			/// /// </summary>
			/// private static void UniquenessConstraintChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Role.UniquenessConstraintChangedRule");
				Role.UniquenessConstraintChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.Role.UniquenessConstraintChangedRule");
			}
		}
	}
	#endregion // Rule classes for Role
	#region Rule classes for SetComparisonConstraint
	partial class SetComparisonConstraint
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetComparisonConstraintHasRoleSequence), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintHasRoleSequenceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintHasRoleSequenceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			public override bool FireBefore
			{
				[System.Diagnostics.DebuggerStepThrough()]
				get
				{
					return true;
				}
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint
			/// /// <summary>
			/// /// AddRule: typeof(SetComparisonConstraintHasRoleSequence)
			/// /// </summary>
			/// private static void ConstraintHasRoleSequenceAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.ConstraintHasRoleSequenceAddedRule");
				SetComparisonConstraint.ConstraintHasRoleSequenceAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.ConstraintHasRoleSequenceAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRoleSequenceHasRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleSequenceHasRoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			public override bool FireBefore
			{
				[System.Diagnostics.DebuggerStepThrough()]
				get
				{
					return true;
				}
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint
			/// /// <summary>
			/// /// AddRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void ConstraintRoleSequenceHasRoleAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.ConstraintRoleSequenceHasRoleAddedRule");
				SetComparisonConstraint.ConstraintRoleSequenceHasRoleAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.ConstraintRoleSequenceHasRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ModelHasSetComparisonConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceCardinalityForConstraintAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceCardinalityForConstraintAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint
			/// /// <summary>
			/// /// AddRule: typeof(ModelHasSetComparisonConstraint)
			/// /// </summary>
			/// private static void EnforceRoleSequenceCardinalityForConstraintAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceCardinalityForConstraintAddRule");
				SetComparisonConstraint.EnforceRoleSequenceCardinalityForConstraintAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceCardinalityForConstraintAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetComparisonConstraintHasRoleSequence), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceCardinalityForSequenceAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceCardinalityForSequenceAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint
			/// /// <summary>
			/// /// AddRule: typeof(SetComparisonConstraintHasRoleSequence)
			/// /// </summary>
			/// private static void EnforceRoleSequenceCardinalityForSequenceAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceCardinalityForSequenceAddRule");
				SetComparisonConstraint.EnforceRoleSequenceCardinalityForSequenceAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceCardinalityForSequenceAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetComparisonConstraintHasRoleSequence), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceCardinalityForSequenceDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceCardinalityForSequenceDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint
			/// /// <summary>
			/// /// DeleteRule: typeof(SetComparisonConstraintHasRoleSequence)
			/// /// </summary>
			/// private static void EnforceRoleSequenceCardinalityForSequenceDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceCardinalityForSequenceDeleteRule");
				SetComparisonConstraint.EnforceRoleSequenceCardinalityForSequenceDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceCardinalityForSequenceDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceValidityForRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceValidityForRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint
			/// /// <summary>
			/// /// AddRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void EnforceRoleSequenceValidityForRoleAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRoleAddRule");
				SetComparisonConstraint.EnforceRoleSequenceValidityForRoleAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceValidityForRoleDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceValidityForRoleDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint
			/// /// <summary>
			/// /// DeleteRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void EnforceRoleSequenceValidityForRoleDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRoleDeleteRule");
				SetComparisonConstraint.EnforceRoleSequenceValidityForRoleDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRoleDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceValidityForRoleReorderRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceValidityForRoleReorderRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint
			/// /// <summary>
			/// /// RolePlayerPositionChangeRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void EnforceRoleSequenceValidityForRoleReorderRule(RolePlayerOrderChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerPositionChanged(Microsoft.VisualStudio.Modeling.RolePlayerOrderChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRoleReorderRule");
				SetComparisonConstraint.EnforceRoleSequenceValidityForRoleReorderRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRoleReorderRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceValidityForRolePlayerAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceValidityForRolePlayerAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint
			/// /// <summary>
			/// /// AddRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void EnforceRoleSequenceValidityForRolePlayerAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRolePlayerAddRule");
				SetComparisonConstraint.EnforceRoleSequenceValidityForRolePlayerAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRolePlayerAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceValidityForRolePlayerDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceValidityForRolePlayerDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint
			/// /// <summary>
			/// /// DeleteRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void EnforceRoleSequenceValidityForRolePlayerDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRolePlayerDeleteRule");
				SetComparisonConstraint.EnforceRoleSequenceValidityForRolePlayerDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRolePlayerDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRule");
				SetComparisonConstraint.EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ExternalRoleConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=Neumont.Tools.Modeling.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class ExternalRoleConstraintDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ExternalRoleConstraintDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint
			/// /// <summary>
			/// /// DeleteRule: typeof(ExternalRoleConstraint), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void ExternalRoleConstraintDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.ExternalRoleConstraintDeletedRule");
				SetComparisonConstraint.ExternalRoleConstraintDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.ExternalRoleConstraintDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ModelHasFactType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint
			/// /// <summary>
			/// /// AddRule: typeof(ModelHasFactType)
			/// /// </summary>
			/// private static void FactTypeAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.FactTypeAddedRule");
				SetComparisonConstraint.FactTypeAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.FactTypeAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactSetComparisonConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactSetComparisonConstraintAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactSetComparisonConstraintAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint
			/// /// <summary>
			/// /// AddRule: typeof(FactSetComparisonConstraint)
			/// /// </summary>
			/// private static void FactSetComparisonConstraintAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.FactSetComparisonConstraintAddedRule");
				SetComparisonConstraint.FactSetComparisonConstraintAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.FactSetComparisonConstraintAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetComparisonConstraintHasRoleSequence), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=Neumont.Tools.Modeling.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class SetComparisonConstraintRoleSequenceDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SetComparisonConstraintRoleSequenceDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint
			/// /// <summary>
			/// /// DeleteRule: typeof(SetComparisonConstraintHasRoleSequence), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void SetComparisonConstraintRoleSequenceDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.SetComparisonConstraintRoleSequenceDeletedRule");
				SetComparisonConstraint.SetComparisonConstraintRoleSequenceDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetComparisonConstraint.SetComparisonConstraintRoleSequenceDeletedRule");
			}
		}
	}
	#endregion // Rule classes for SetComparisonConstraint
	#region Rule classes for SetConstraint
	partial class SetConstraint
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ModelHasSetConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetConstraint
			/// /// <summary>
			/// /// AddRule: typeof(ModelHasSetConstraint)
			/// /// </summary>
			/// private static void ConstraintAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.ConstraintAddedRule");
				SetConstraint.ConstraintAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.ConstraintAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRoleSequenceHasRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleSequenceHasRoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			public override bool FireBefore
			{
				[System.Diagnostics.DebuggerStepThrough()]
				get
				{
					return true;
				}
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetConstraint
			/// /// <summary>
			/// /// AddRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void ConstraintRoleSequenceHasRoleAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.ConstraintRoleSequenceHasRoleAddedRule");
				SetConstraint.ConstraintRoleSequenceHasRoleAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.ConstraintRoleSequenceHasRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceValidityForRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceValidityForRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetConstraint
			/// /// <summary>
			/// /// AddRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void EnforceRoleSequenceValidityForRoleAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.EnforceRoleSequenceValidityForRoleAddRule");
				SetConstraint.EnforceRoleSequenceValidityForRoleAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.EnforceRoleSequenceValidityForRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceValidityForRoleDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceValidityForRoleDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetConstraint
			/// /// <summary>
			/// /// DeleteRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void EnforceRoleSequenceValidityForRoleDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.EnforceRoleSequenceValidityForRoleDeleteRule");
				SetConstraint.EnforceRoleSequenceValidityForRoleDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.EnforceRoleSequenceValidityForRoleDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceValidityForRolePlayerAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceValidityForRolePlayerAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetConstraint
			/// /// <summary>
			/// /// AddRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void EnforceRoleSequenceValidityForRolePlayerAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.EnforceRoleSequenceValidityForRolePlayerAddRule");
				SetConstraint.EnforceRoleSequenceValidityForRolePlayerAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.EnforceRoleSequenceValidityForRolePlayerAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceValidityForRolePlayerDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceValidityForRolePlayerDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetConstraint
			/// /// <summary>
			/// /// DeleteRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void EnforceRoleSequenceValidityForRolePlayerDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.EnforceRoleSequenceValidityForRolePlayerDeleteRule");
				SetConstraint.EnforceRoleSequenceValidityForRolePlayerDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.EnforceRoleSequenceValidityForRolePlayerDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetConstraint
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRule");
				SetConstraint.EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactSetConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactSetConstraintAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactSetConstraintAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetConstraint
			/// /// <summary>
			/// /// AddRule: typeof(FactSetConstraint)
			/// /// </summary>
			/// private static void FactSetConstraintAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.FactSetConstraintAddedRule");
				SetConstraint.FactSetConstraintAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.FactSetConstraintAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactSetConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactSetConstraintDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactSetConstraintDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetConstraint
			/// /// <summary>
			/// /// DeleteRule: typeof(FactSetConstraint)
			/// /// </summary>
			/// private static void FactSetConstraintDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.FactSetConstraintDeletedRule");
				SetConstraint.FactSetConstraintDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.FactSetConstraintDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ModelHasFactType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetConstraint
			/// /// <summary>
			/// /// AddRule: typeof(ModelHasFactType)
			/// /// </summary>
			/// private static void FactTypeAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.FactTypeAddedRule");
				SetConstraint.FactTypeAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.FactTypeAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ModalityChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ModalityChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetConstraint
			/// /// <summary>
			/// /// ChangeRule: typeof(SetConstraint)
			/// /// </summary>
			/// private static void ModalityChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.ModalityChangeRule");
				SetConstraint.ModalityChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.ModalityChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class SetConstraintRoleSequenceHasRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SetConstraintRoleSequenceHasRoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetConstraint
			/// /// <summary>
			/// /// AddRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void SetConstraintRoleSequenceHasRoleAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.SetConstraintRoleSequenceHasRoleAddedRule");
				SetConstraint.SetConstraintRoleSequenceHasRoleAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.SetConstraintRoleSequenceHasRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class SetConstraintRoleSequenceHasRoleDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SetConstraintRoleSequenceHasRoleDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SetConstraint
			/// /// <summary>
			/// /// DeletingRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void SetConstraintRoleSequenceHasRoleDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.SetConstraintRoleSequenceHasRoleDeletingRule");
				SetConstraint.SetConstraintRoleSequenceHasRoleDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SetConstraint.SetConstraintRoleSequenceHasRoleDeletingRule");
			}
		}
	}
	#endregion // Rule classes for SetConstraint
	#region Rule classes for SubtypeFact
	partial class SubtypeFact
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=Neumont.Tools.Modeling.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class DeleteSubtypeWhenRolePlayerDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DeleteSubtypeWhenRolePlayerDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SubtypeFact
			/// /// <summary>
			/// /// DeleteRule: typeof(ObjectTypePlaysRole), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void DeleteSubtypeWhenRolePlayerDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.DeleteSubtypeWhenRolePlayerDeletedRule");
				SubtypeFact.DeleteSubtypeWhenRolePlayerDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.DeleteSubtypeWhenRolePlayerDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnsureConsistentDataTypesAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnsureConsistentDataTypesAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SubtypeFact
			/// /// <summary>
			/// /// AddRule: typeof(ValueTypeHasDataType)
			/// /// </summary>
			/// private static void EnsureConsistentDataTypesAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.EnsureConsistentDataTypesAddRule");
				SubtypeFact.EnsureConsistentDataTypesAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.EnsureConsistentDataTypesAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnsureConsistentDataTypesDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnsureConsistentDataTypesDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SubtypeFact
			/// /// <summary>
			/// /// DeleteRule: typeof(ValueTypeHasDataType)
			/// /// </summary>
			/// private static void EnsureConsistentDataTypesDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.EnsureConsistentDataTypesDeleteRule");
				SubtypeFact.EnsureConsistentDataTypesDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.EnsureConsistentDataTypesDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=Neumont.Tools.Modeling.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class EnsureConsistentRolePlayerTypesAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnsureConsistentRolePlayerTypesAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SubtypeFact
			/// /// <summary>
			/// /// AddRule: typeof(ObjectTypePlaysRole), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void EnsureConsistentRolePlayerTypesAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.EnsureConsistentRolePlayerTypesAddRule");
				SubtypeFact.EnsureConsistentRolePlayerTypesAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.EnsureConsistentRolePlayerTypesAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SubtypeFact), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class InitializeSubtypeAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public InitializeSubtypeAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SubtypeFact
			/// /// <summary>
			/// /// AddRule: typeof(SubtypeFact)
			/// /// </summary>
			/// private static void InitializeSubtypeAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.InitializeSubtypeAddRule");
				SubtypeFact.InitializeSubtypeAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.InitializeSubtypeAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class LimitSubtypeConstraintChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public LimitSubtypeConstraintChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SubtypeFact
			/// /// <summary>
			/// /// ChangeRule: typeof(SetConstraint)
			/// /// </summary>
			/// private static void LimitSubtypeConstraintChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.LimitSubtypeConstraintChangeRule");
				SubtypeFact.LimitSubtypeConstraintChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.LimitSubtypeConstraintChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class LimitSubtypeConstraintRolesAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public LimitSubtypeConstraintRolesAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SubtypeFact
			/// /// <summary>
			/// /// AddRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void LimitSubtypeConstraintRolesAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.LimitSubtypeConstraintRolesAddRule");
				SubtypeFact.LimitSubtypeConstraintRolesAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.LimitSubtypeConstraintRolesAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=Neumont.Tools.Modeling.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class LimitSubtypeConstraintRolesDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public LimitSubtypeConstraintRolesDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SubtypeFact
			/// /// <summary>
			/// /// DeleteRule: typeof(ConstraintRoleSequenceHasRole), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void LimitSubtypeConstraintRolesDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.LimitSubtypeConstraintRolesDeleteRule");
				SubtypeFact.LimitSubtypeConstraintRolesDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.LimitSubtypeConstraintRolesDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactSetConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class LimitSubtypeConstraintsAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public LimitSubtypeConstraintsAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SubtypeFact
			/// /// <summary>
			/// /// AddRule: typeof(FactSetConstraint)
			/// /// </summary>
			/// private static void LimitSubtypeConstraintsAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.LimitSubtypeConstraintsAddRule");
				SubtypeFact.LimitSubtypeConstraintsAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.LimitSubtypeConstraintsAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactSetConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=Neumont.Tools.Modeling.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class LimitSubtypeConstraintsDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public LimitSubtypeConstraintsDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SubtypeFact
			/// /// <summary>
			/// /// DeleteRule: typeof(FactSetConstraint), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void LimitSubtypeConstraintsDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.LimitSubtypeConstraintsDeleteRule");
				SubtypeFact.LimitSubtypeConstraintsDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.LimitSubtypeConstraintsDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class LimitSubtypeRolesAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public LimitSubtypeRolesAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SubtypeFact
			/// /// <summary>
			/// /// AddRule: typeof(FactTypeHasRole)
			/// /// </summary>
			/// private static void LimitSubtypeRolesAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.LimitSubtypeRolesAddRule");
				SubtypeFact.LimitSubtypeRolesAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.LimitSubtypeRolesAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=Neumont.Tools.Modeling.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class LimitSubtypeRolesDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public LimitSubtypeRolesDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SubtypeFact
			/// /// <summary>
			/// /// DeleteRule: typeof(FactTypeHasRole), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void LimitSubtypeRolesDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.LimitSubtypeRolesDeleteRule");
				SubtypeFact.LimitSubtypeRolesDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.LimitSubtypeRolesDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetComparisonConstraintHasRoleSequence), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class LimitSubtypeSetComparisonConstraintSequenceAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public LimitSubtypeSetComparisonConstraintSequenceAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.SubtypeFact
			/// /// <summary>
			/// /// AddRule: typeof(SetComparisonConstraintHasRoleSequence)
			/// /// </summary>
			/// private static void LimitSubtypeSetComparisonConstraintSequenceAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.LimitSubtypeSetComparisonConstraintSequenceAddRule");
				SubtypeFact.LimitSubtypeSetComparisonConstraintSequenceAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.SubtypeFact.LimitSubtypeSetComparisonConstraintSequenceAddRule");
			}
		}
	}
	#endregion // Rule classes for SubtypeFact
	#region Rule classes for UniquenessConstraint
	partial class UniquenessConstraint
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactSetConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class NMinusOneConstraintAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public NMinusOneConstraintAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.UniquenessConstraint
			/// /// <summary>
			/// /// AddRule: typeof(FactSetConstraint)
			/// /// </summary>
			/// private static void NMinusOneConstraintAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.UniquenessConstraint.NMinusOneConstraintAddRule");
				UniquenessConstraint.NMinusOneConstraintAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.UniquenessConstraint.NMinusOneConstraintAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class NMinusOneConstraintRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public NMinusOneConstraintRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.UniquenessConstraint
			/// /// <summary>
			/// /// AddRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void NMinusOneConstraintRoleAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.UniquenessConstraint.NMinusOneConstraintRoleAddRule");
				UniquenessConstraint.NMinusOneConstraintRoleAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.UniquenessConstraint.NMinusOneConstraintRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class NMinusOneConstraintRoleDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public NMinusOneConstraintRoleDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.UniquenessConstraint
			/// /// <summary>
			/// /// DeleteRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void NMinusOneConstraintRoleDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.UniquenessConstraint.NMinusOneConstraintRoleDeleteRule");
				UniquenessConstraint.NMinusOneConstraintRoleDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.UniquenessConstraint.NMinusOneConstraintRoleDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class NMinusOneFactTypeRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public NMinusOneFactTypeRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.UniquenessConstraint
			/// /// <summary>
			/// /// AddRule: typeof(FactTypeHasRole)
			/// /// </summary>
			/// private static void NMinusOneFactTypeRoleAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.UniquenessConstraint.NMinusOneFactTypeRoleAddRule");
				UniquenessConstraint.NMinusOneFactTypeRoleAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.UniquenessConstraint.NMinusOneFactTypeRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class NMinusOneFactTypeRoleDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public NMinusOneFactTypeRoleDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.UniquenessConstraint
			/// /// <summary>
			/// /// DeleteRule: typeof(FactTypeHasRole)
			/// /// </summary>
			/// private static void NMinusOneFactTypeRoleDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.UniquenessConstraint.NMinusOneFactTypeRoleDeleteRule");
				UniquenessConstraint.NMinusOneFactTypeRoleDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.UniquenessConstraint.NMinusOneFactTypeRoleDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(UniquenessConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class UniquenessConstraintChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public UniquenessConstraintChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.UniquenessConstraint
			/// /// <summary>
			/// /// ChangeRule: typeof(UniquenessConstraint)
			/// /// </summary>
			/// private static void UniquenessConstraintChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.UniquenessConstraint.UniquenessConstraintChangeRule");
				UniquenessConstraint.UniquenessConstraintChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.UniquenessConstraint.UniquenessConstraintChangeRule");
			}
		}
	}
	#endregion // Rule classes for UniquenessConstraint
	#region Rule classes for ValueConstraint
	partial class ValueConstraint
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DataTypeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DataTypeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ValueConstraint
			/// /// <summary>
			/// /// ChangeRule: typeof(ValueTypeHasDataType)
			/// /// </summary>
			/// private static void DataTypeChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.DataTypeChangeRule");
				ValueConstraint.DataTypeChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.DataTypeChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DataTypeDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DataTypeDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ValueConstraint
			/// /// <summary>
			/// /// DeletingRule: typeof(ValueTypeHasDataType)
			/// /// </summary>
			/// private static void DataTypeDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.DataTypeDeletingRule");
				ValueConstraint.DataTypeDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.DataTypeDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DataTypeRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DataTypeRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ValueConstraint
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ValueTypeHasDataType)
			/// /// </summary>
			/// private static void DataTypeRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.DataTypeRolePlayerChangeRule");
				ValueConstraint.DataTypeRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.DataTypeRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectTypeRoleAddedClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectTypeRoleAddedClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ValueConstraint
			/// /// <summary>
			/// /// AddRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void ObjectTypeRoleAdded(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.ObjectTypeRoleAdded");
				ValueConstraint.ObjectTypeRoleAdded(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.ObjectTypeRoleAdded");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class PreferredIdentifierDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ValueConstraint
			/// /// <summary>
			/// /// DeletingRule: typeof(EntityTypeHasPreferredIdentifier)
			/// /// </summary>
			/// private static void PreferredIdentifierDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.PreferredIdentifierDeletingRule");
				ValueConstraint.PreferredIdentifierDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.PreferredIdentifierDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class PreferredIdentifierRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ValueConstraint
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(EntityTypeHasPreferredIdentifier)
			/// /// </summary>
			/// private static void PreferredIdentifierRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.PreferredIdentifierRolePlayerChangeRule");
				ValueConstraint.PreferredIdentifierRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.PreferredIdentifierRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=(1 + Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority))]
		private sealed class PreferredIdentifierRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ValueConstraint
			/// /// <summary>
			/// /// AddRule: typeof(ConstraintRoleSequenceHasRole), Priority=1;
			/// /// </summary>
			/// private static void PreferredIdentifierRoleAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.PreferredIdentifierRoleAddRule");
				ValueConstraint.PreferredIdentifierRoleAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.PreferredIdentifierRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ValueConstraint
			/// /// <summary>
			/// /// DeletingRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void RolePlayerDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.RolePlayerDeletingRule");
				ValueConstraint.RolePlayerDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.RolePlayerDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ValueConstraint
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void RolePlayerRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.RolePlayerRolePlayerChangeRule");
				ValueConstraint.RolePlayerRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.RolePlayerRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RoleHasValueConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleValueConstraintAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleValueConstraintAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ValueConstraint
			/// /// <summary>
			/// /// AddRule: typeof(RoleHasValueConstraint)
			/// /// </summary>
			/// private static void RoleValueConstraintAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.RoleValueConstraintAddedRule");
				ValueConstraint.RoleValueConstraintAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.RoleValueConstraintAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasValueConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValueConstraintAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueConstraintAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ValueConstraint
			/// /// <summary>
			/// /// AddRule: typeof(ValueTypeHasValueConstraint)
			/// /// </summary>
			/// private static void ValueConstraintAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.ValueConstraintAddRule");
				ValueConstraint.ValueConstraintAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.ValueConstraintAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValueConstraintChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueConstraintChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ValueConstraint
			/// /// <summary>
			/// /// ChangeRule: typeof(ValueConstraint)
			/// /// </summary>
			/// private static void ValueConstraintChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.ValueConstraintChangeRule");
				ValueConstraint.ValueConstraintChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.ValueConstraintChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueConstraintHasValueRange), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValueRangeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueRangeAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ValueConstraint
			/// /// <summary>
			/// /// AddRule: typeof(ValueConstraintHasValueRange)
			/// /// </summary>
			/// private static void ValueRangeAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.ValueRangeAddedRule");
				ValueConstraint.ValueRangeAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.ValueRangeAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueRange), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValueRangeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueRangeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ValueConstraint
			/// /// <summary>
			/// /// ChangeRule: typeof(ValueRange)
			/// /// </summary>
			/// private static void ValueRangeChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.ValueRangeChangeRule");
				ValueConstraint.ValueRangeChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueConstraint.ValueRangeChangeRule");
			}
		}
	}
	#endregion // Rule classes for ValueConstraint
	#region Rule classes for ValueRange
	partial class ValueRange
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueRange), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValueRangeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueRangeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ValueRange
			/// /// <summary>
			/// /// ChangeRule: typeof(ValueRange)
			/// /// </summary>
			/// private static void ValueRangeChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueRange.ValueRangeChangeRule");
				ValueRange.ValueRangeChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueRange.ValueRangeChangeRule");
			}
		}
	}
	#endregion // Rule classes for ValueRange
	#region Rule classes for ValueTypeInstance
	partial class ValueTypeInstance
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValueTypeHasDataTypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueTypeHasDataTypeAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ValueTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(ValueTypeHasDataType)
			/// /// </summary>
			/// private static void ValueTypeHasDataTypeAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueTypeInstance.ValueTypeHasDataTypeAddedRule");
				ValueTypeInstance.ValueTypeHasDataTypeAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueTypeInstance.ValueTypeHasDataTypeAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValueTypeHasDataTypeDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueTypeHasDataTypeDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ValueTypeInstance
			/// /// <summary>
			/// /// DeleteRule: typeof(ValueTypeHasDataType)
			/// /// </summary>
			/// private static void ValueTypeHasDataTypeDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueTypeInstance.ValueTypeHasDataTypeDeletedRule");
				ValueTypeInstance.ValueTypeHasDataTypeDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueTypeInstance.ValueTypeHasDataTypeDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValueTypeHasDataTypeRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueTypeHasDataTypeRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ValueTypeInstance
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ValueTypeHasDataType)
			/// /// </summary>
			/// private static void ValueTypeHasDataTypeRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ValueTypeInstance.ValueTypeHasDataTypeRolePlayerChangeRule");
				ValueTypeInstance.ValueTypeHasDataTypeRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ObjectModel.ValueTypeInstance.ValueTypeHasDataTypeRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeInstance), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValueTypeInstanceValueChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueTypeInstanceValueChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ValueTypeInstance
			/// /// <summary>
			/// /// ChangeRule: typeof(ValueTypeInstance)
			/// /// </summary>
			/// private static void ValueTypeInstanceValueChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueTypeInstance.ValueTypeInstanceValueChangedRule");
				ValueTypeInstance.ValueTypeInstanceValueChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueTypeInstance.ValueTypeInstanceValueChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasValueTypeInstance), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValueTypeHasValueTypeInstanceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueTypeHasValueTypeInstanceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ObjectModel.ValueTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(ValueTypeHasValueTypeInstance)
			/// /// </summary>
			/// private static void ValueTypeHasValueTypeInstanceAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueTypeInstance.ValueTypeHasValueTypeInstanceAddedRule");
				ValueTypeInstance.ValueTypeHasValueTypeInstanceAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.ValueTypeInstance.ValueTypeHasValueTypeInstanceAddedRule");
			}
		}
	}
	#endregion // Rule classes for ValueTypeInstance
	#endregion // Auto-rule classes
}
namespace Neumont.Tools.Modeling
{
	#region Auto-rule classes
	#region Rule classes for NamedElementDictionary
	partial class NamedElementDictionary
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Microsoft.VisualStudio.Modeling.ElementLink), Priority=(NamedElementDictionary.RulePriority + Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority))]
		private sealed class ElementLinkAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ElementLinkAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.Modeling.NamedElementDictionary
			/// /// <summary>
			/// /// AddRule: typeof(Microsoft.VisualStudio.Modeling.ElementLink), Priority=NamedElementDictionary.RulePriority;
			/// /// </summary>
			/// private static void ElementLinkAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.NamedElementDictionary.ElementLinkAddedRule");
				NamedElementDictionary.ElementLinkAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.NamedElementDictionary.ElementLinkAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Microsoft.VisualStudio.Modeling.ElementLink), Priority=(NamedElementDictionary.RulePriority + Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority))]
		private sealed class ElementLinkDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ElementLinkDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.Modeling.NamedElementDictionary
			/// /// <summary>
			/// /// DeletingRule: typeof(Microsoft.VisualStudio.Modeling.ElementLink), Priority=NamedElementDictionary.RulePriority;
			/// /// </summary>
			/// private static void ElementLinkDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.NamedElementDictionary.ElementLinkDeletingRule");
				NamedElementDictionary.ElementLinkDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.NamedElementDictionary.ElementLinkDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Microsoft.VisualStudio.Modeling.ModelElement), Priority=(NamedElementDictionary.RulePriority + Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority))]
		private sealed class NamedElementChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public NamedElementChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.Modeling.NamedElementDictionary
			/// /// <summary>
			/// /// ChangeRule: typeof(Microsoft.VisualStudio.Modeling.ModelElement), Priority=NamedElementDictionary.RulePriority;
			/// /// </summary>
			/// private static void NamedElementChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.NamedElementDictionary.NamedElementChangedRule");
				NamedElementDictionary.NamedElementChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ObjectModel.NamedElementDictionary.NamedElementChangedRule");
			}
		}
	}
	#endregion // Rule classes for NamedElementDictionary
	#endregion // Auto-rule classes
}
