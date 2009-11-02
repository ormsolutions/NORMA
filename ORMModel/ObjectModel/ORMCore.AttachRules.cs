using System;
using System.Reflection;

// Common Public License Copyright Notice
// /**************************************************************************\
// * Natural Object-Role Modeling Architect for Visual Studio                 *
// *                                                                          *
// * Copyright © Neumont University. All rights reserved.                     *
// * Copyright © ORM Solutions, LLC. All rights reserved.                     *
// *                                                                          *
// * The use and distribution terms for this software are covered by the      *
// * Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
// * can be found in the file CPL.txt at the root of this distribution.       *
// * By using this software in any fashion, you are agreeing to be bound by   *
// * the terms of this license.                                               *
// *                                                                          *
// * You must not remove this notice, or any other, from this software.       *
// \**************************************************************************/

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region Attach rules to ORMCoreDomainModel model
	partial class ORMCoreDomainModel : ORMSolutions.ORMArchitect.Framework.Shell.IDomainModelEnablesRulesAfterDeserialization
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
						typeof(CalculatedPathValue).GetNestedType("FunctionChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(CalculatedPathValue).GetNestedType("FunctionDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(CalculatedPathValue).GetNestedType("InputBoundToCalculatedValueRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(CalculatedPathValue).GetNestedType("InputBoundToConstantRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(CalculatedPathValue).GetNestedType("InputBoundToPathedRoleRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("AutomaticJoinPathChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("BlockRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("ConstraintRoleProjectedFromCalculatedValueRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("ConstraintRoleProjectedFromConstantRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("ConstraintRoleProjectedFromPathedRoleRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("ConstraintRoleSequenceHasRoleAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("ConstraintRoleSequenceHasRoleDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("JoinPathAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("JoinPathDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("ModalityChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("SetComparisonConstraintHasRoleDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("SetConstraintDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("RolePlayerAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("RolePlayerDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("RolePlayerRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("RoleAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("RoleDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequence).GetNestedType("ConstraintRolePositionChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
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
						typeof(EntityTypeInstance).GetNestedType("EntityTypeInstanceAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("EntityTypeInstanceDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("EntityTypeRoleInstanceAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("EntityTypeRoleInstanceDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("ObjectificationAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("ObjectificationDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("ObjectificationRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("PreferredIdentifierAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("PreferredIdentifierDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("PreferredIdentifierRoleAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("PreferredIdentifierRoleDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(EntityTypeInstance).GetNestedType("PreferredIdentifierRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
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
						typeof(ElementGrouping).GetNestedType("GroupingChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ElementGrouping).GetNestedType("GroupingTypeAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ElementGrouping).GetNestedType("GroupingTypeDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ElementGrouping).GetNestedType("GroupingExclusionAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ElementGrouping).GetNestedType("GroupingExclusionDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ElementGrouping).GetNestedType("StandardNamedElementNameChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ElementGrouping).GetNestedType("FactTypeNameChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ElementGrouping).GetNestedType("ModelNoteTextChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ElementGroupingSet).GetNestedType("DuplicateGroupingNameGroupingDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ElementGroupingSet).GetNestedType("GroupingDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ElementGroupingSet).GetNestedType("GroupingSetAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("BlockRoleMigrationRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("FactTypeAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("FactTypeChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactType).GetNestedType("FactTypeDerivationExpressionChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
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
						typeof(FactTypeDerivationExpression).GetNestedType("DerivationExpressionAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeDerivationExpression).GetNestedType("DerivationExpressionChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeDerivationExpression).GetNestedType("DerivationRuleAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeDerivationExpression).GetNestedType("DerivationRuleChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeInstance).GetNestedType("FactTypeInstanceAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeInstance).GetNestedType("FactTypeRoleAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeInstance).GetNestedType("FactTypeRoleDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeInstance).GetNestedType("FactTypeRoleInstanceAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeInstance).GetNestedType("FactTypeNameChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeInstance).GetNestedType("FactTypeRoleInstanceDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeInstance).GetNestedType("FactTypeInstanceHasRoleInstanceRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeInstance).GetNestedType("FactTypeRoleInstanceRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeInstance).GetNestedType("ImpliedBooleanRolePlayerDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeInstance).GetNestedType("ObjectTypeInstanceNameChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FrequencyConstraint).GetNestedType("FrequencyConstraintMinMaxRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(MandatoryConstraint).GetNestedType("MandatoryConstraintChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModelError).GetNestedType("SynchronizeErrorTextForModelRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModelError).GetNestedType("SynchronizeErrorTextForOwnerRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ORMSolutions.ORMArchitect.Framework.NamedElementDictionary).GetNestedType("ElementLinkAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ORMSolutions.ORMArchitect.Framework.NamedElementDictionary).GetNestedType("ElementLinkDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ORMSolutions.ORMArchitect.Framework.NamedElementDictionary).GetNestedType("NamedElementChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
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
						typeof(ObjectificationInstance).GetNestedType("ObjectificationInstanceAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectificationInstance).GetNestedType("ObjectificationInstanceDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectificationInstance).GetNestedType("ObjectificationInstanceRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectificationInstance).GetNestedType("RoleInstanceRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
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
						typeof(ObjectType).GetNestedType("SubtypeDerivationExpressionChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
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
						typeof(ObjectTypeInstance).GetNestedType("EntityTypeInstanceHasRoleInstanceRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("EntityTypeSupertypeInstanceAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("EntityTypeSubtypeInstanceDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("EntityTypeSubtypeInstanceAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("EntityTypeSupertypeInstanceDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("EntityTypeSupertypeInstanceRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("FactTypeInstanceNameChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("FactTypeNameChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("MandatoryConstraintChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("ObjectificationAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("ObjectificationDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("ObjectificationInstanceAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("ObjectificationInstanceDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("ObjectificationInstanceRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("ObjectificationRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("ObjectTypeInstanceAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("ObjectTypeInstanceRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("RoleInstanceAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("RoleInstanceDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("RoleInstanceRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("RolePlayerAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("RolePlayerDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("RolePlayerRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeInstance).GetNestedType("PreferredSupertypeChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ORMModel).GetNestedType("DuplicateConstraintNameConstraintDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ORMModel).GetNestedType("DuplicateObjectTypeNameObjectTypeDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ORMModel).GetNestedType("DuplicateRecognizedPhraseDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ORMModel).GetNestedType("DuplicateFunctionNameDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
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
						typeof(Role).GetNestedType("FactTypeDerivationRuleDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RoleChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RoleInstanceAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RoleInstanceDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RoleDerivesFromCalculatedValueRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RoleDerivesFromConstantRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RoleDerivedFromPathedRoleRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RoleInstanceRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RolePlayerRequiredAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RolePlayerRequiredDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("RolePlayerRequiredForNewRoleAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("UpdatedRolePlayerRequiredErrorsDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(Role).GetNestedType("UniquenessConstraintChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RolePath).GetNestedType("PathedRoleDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RolePath).GetNestedType("PathedRoleRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RolePath).GetNestedType("SubPathDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RolePath).GetNestedType("SubPathRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RolePathComponent).GetNestedType("RolePathCompositorHasPathComponentAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RolePathComponent).GetNestedType("RolePathCompositorHasPathComponentDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RolePathComponent).GetNestedType("RolePathCompositorHasPathComponentRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RolePathComponent).GetNestedType("RolePathOwnerHasPathComponentAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RolePathComponent).GetNestedType("RolePathOwnerHasPathComponentDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RolePathComponent).GetNestedType("RolePathOwnerHasPathComponentRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
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
			if (ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InitializingToolboxItems)
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
			for (int i = 0; i < 307; ++i)
			{
				ruleManager.EnableRule(disabledRuleTypes[i]);
			}
			ORMCoreDomainModel.EnableCustomRuleNotifications(store);
		}
		void ORMSolutions.ORMArchitect.Framework.Shell.IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.Store store)
		{
			this.EnableRulesAfterDeserialization(store);
		}
	}
	#endregion // Attach rules to ORMCoreDomainModel model
	#region Auto-rule classes
	#region Rule classes for CalculatedPathValue
	partial class CalculatedPathValue
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(CalculatedPathValueIsCalculatedWithFunction), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FunctionChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FunctionChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.CalculatedPathValue
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(CalculatedPathValueIsCalculatedWithFunction)
			/// /// </summary>
			/// private static void FunctionChangedRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.CalculatedPathValue.FunctionChangedRule");
				CalculatedPathValue.FunctionChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.CalculatedPathValue.FunctionChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(CalculatedPathValueIsCalculatedWithFunction), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FunctionDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FunctionDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.CalculatedPathValue
			/// /// <summary>
			/// /// DeleteRule: typeof(CalculatedPathValueIsCalculatedWithFunction)
			/// /// </summary>
			/// private static void FunctionDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.CalculatedPathValue.FunctionDeletedRule");
				CalculatedPathValue.FunctionDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.CalculatedPathValue.FunctionDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(CalculatedPathValueInputBindsToCalculatedPathValue), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class InputBoundToCalculatedValueRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public InputBoundToCalculatedValueRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.CalculatedPathValue
			/// /// <summary>
			/// /// AddRule: typeof(CalculatedPathValueInputBindsToCalculatedPathValue)
			/// /// </summary>
			/// private static void InputBoundToCalculatedValueRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.CalculatedPathValue.InputBoundToCalculatedValueRule");
				CalculatedPathValue.InputBoundToCalculatedValueRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.CalculatedPathValue.InputBoundToCalculatedValueRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(CalculatedPathValueInputBindsToPathConstant), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class InputBoundToConstantRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public InputBoundToConstantRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.CalculatedPathValue
			/// /// <summary>
			/// /// AddRule: typeof(CalculatedPathValueInputBindsToPathConstant)
			/// /// </summary>
			/// private static void InputBoundToConstantRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.CalculatedPathValue.InputBoundToConstantRule");
				CalculatedPathValue.InputBoundToConstantRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.CalculatedPathValue.InputBoundToConstantRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(CalculatedPathValueInputBindsToPathedRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class InputBoundToPathedRoleRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public InputBoundToPathedRoleRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.CalculatedPathValue
			/// /// <summary>
			/// /// AddRule: typeof(CalculatedPathValueInputBindsToPathedRole)
			/// /// </summary>
			/// private static void InputBoundToPathedRoleRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.CalculatedPathValue.InputBoundToPathedRoleRule");
				CalculatedPathValue.InputBoundToPathedRoleRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.CalculatedPathValue.InputBoundToPathedRoleRule");
			}
		}
	}
	#endregion // Rule classes for CalculatedPathValue
	#region Rule classes for ConstraintRoleSequence
	partial class ConstraintRoleSequence
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceJoinPath), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class AutomaticJoinPathChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public AutomaticJoinPathChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence
			/// /// <summary>
			/// /// ChangeRule: typeof(ConstraintRoleSequenceJoinPath)
			/// /// </summary>
			/// private static void AutomaticJoinPathChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.AutomaticJoinPathChangeRule");
				ConstraintRoleSequence.AutomaticJoinPathChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.AutomaticJoinPathChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class BlockRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public BlockRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.BlockRolePlayerChangeRule");
				ConstraintRoleSequence.BlockRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.BlockRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleProjectedFromCalculatedPathValue), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRoleProjectedFromCalculatedValueRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleProjectedFromCalculatedValueRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence
			/// /// <summary>
			/// /// AddRule: typeof(ConstraintRoleProjectedFromCalculatedPathValue)
			/// /// </summary>
			/// private static void ConstraintRoleProjectedFromCalculatedValueRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.ConstraintRoleProjectedFromCalculatedValueRule");
				ConstraintRoleSequence.ConstraintRoleProjectedFromCalculatedValueRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.ConstraintRoleProjectedFromCalculatedValueRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleProjectedFromPathConstant), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRoleProjectedFromConstantRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleProjectedFromConstantRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence
			/// /// <summary>
			/// /// AddRule: typeof(ConstraintRoleProjectedFromPathConstant)
			/// /// </summary>
			/// private static void ConstraintRoleProjectedFromConstantRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.ConstraintRoleProjectedFromConstantRule");
				ConstraintRoleSequence.ConstraintRoleProjectedFromConstantRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.ConstraintRoleProjectedFromConstantRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleProjectedFromPathedRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRoleProjectedFromPathedRoleRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleProjectedFromPathedRoleRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence
			/// /// <summary>
			/// /// AddRule: typeof(ConstraintRoleProjectedFromPathedRole)
			/// /// </summary>
			/// private static void ConstraintRoleProjectedFromPathedRoleRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.ConstraintRoleProjectedFromPathedRoleRule");
				ConstraintRoleSequence.ConstraintRoleProjectedFromPathedRoleRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.ConstraintRoleProjectedFromPathedRoleRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRoleSequenceHasRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleSequenceHasRoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.ConstraintRoleSequenceHasRoleAddedRule");
				ConstraintRoleSequence.ConstraintRoleSequenceHasRoleAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.ConstraintRoleSequenceHasRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRoleSequenceHasRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleSequenceHasRoleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.ConstraintRoleSequenceHasRoleDeletedRule");
				ConstraintRoleSequence.ConstraintRoleSequenceHasRoleDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.ConstraintRoleSequenceHasRoleDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasJoinPath), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class JoinPathAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public JoinPathAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence
			/// /// <summary>
			/// /// AddRule: typeof(ConstraintRoleSequenceHasJoinPath)
			/// /// </summary>
			/// private static void JoinPathAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.JoinPathAddedRule");
				ConstraintRoleSequence.JoinPathAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.JoinPathAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasJoinPath), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class JoinPathDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public JoinPathDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence
			/// /// <summary>
			/// /// DeleteRule: typeof(ConstraintRoleSequenceHasJoinPath)
			/// /// </summary>
			/// private static void JoinPathDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.JoinPathDeletedRule");
				ConstraintRoleSequence.JoinPathDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.JoinPathDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetComparisonConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ModalityChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ModalityChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.ModalityChangeRule");
				ConstraintRoleSequence.ModalityChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.ModalityChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequence), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class SetComparisonConstraintHasRoleDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SetComparisonConstraintHasRoleDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.SetComparisonConstraintHasRoleDeletingRule");
				ConstraintRoleSequence.SetComparisonConstraintHasRoleDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.SetComparisonConstraintHasRoleDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class SetConstraintDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SetConstraintDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.SetConstraintDeletingRule");
				ConstraintRoleSequence.SetConstraintDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.SetConstraintDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.RolePlayerAddedRule");
				ConstraintRoleSequence.RolePlayerAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.RolePlayerAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.RolePlayerDeletedRule");
				ConstraintRoleSequence.RolePlayerDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.RolePlayerDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerRolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.RolePlayerRolePlayerChangedRule");
				ConstraintRoleSequence.RolePlayerRolePlayerChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.RolePlayerRolePlayerChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence
			/// /// <summary>
			/// /// AddRule: typeof(FactTypeHasRole)
			/// /// </summary>
			/// private static void RoleAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.RoleAddedRule");
				ConstraintRoleSequence.RoleAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.RoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence
			/// /// <summary>
			/// /// DeleteRule: typeof(FactTypeHasRole)
			/// /// </summary>
			/// private static void RoleDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.RoleDeletedRule");
				ConstraintRoleSequence.RoleDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.RoleDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRolePositionChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRolePositionChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence
			/// /// <summary>
			/// /// RolePlayerPositionChangeRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void ConstraintRolePositionChangedRule(RolePlayerOrderChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerPositionChanged(Microsoft.VisualStudio.Modeling.RolePlayerOrderChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.ConstraintRolePositionChangedRule");
				ConstraintRoleSequence.ConstraintRolePositionChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequence.ConstraintRolePositionChangedRule");
			}
		}
	}
	#endregion // Rule classes for ConstraintRoleSequence
	#region Rule classes for ConstraintUtility
	partial class ConstraintUtility
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class ConstraintRoleSequenceHasRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleSequenceHasRoleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintUtility
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintUtility.ConstraintRoleSequenceHasRoleDeletedRule");
				ConstraintUtility.ConstraintRoleSequenceHasRoleDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintUtility.ConstraintRoleSequenceHasRoleDeletedRule");
			}
		}
	}
	#endregion // Rule classes for ConstraintUtility
	#region Rule classes for EntityTypeHasPreferredIdentifier
	partial class EntityTypeHasPreferredIdentifier
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ModalityChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ModalityChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier.ModalityChangeRule");
				EntityTypeHasPreferredIdentifier.ModalityChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier.ModalityChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
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
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierAddedRule");
				EntityTypeHasPreferredIdentifier.PreferredIdentifierAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class PreferredIdentifierRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierRolePlayerChangeRule");
				EntityTypeHasPreferredIdentifier.PreferredIdentifierRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier.PreferredIdentifierRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class TestRemovePreferredIdentifierConstraintRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public TestRemovePreferredIdentifierConstraintRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierConstraintRoleAddRule");
				EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierConstraintRoleAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierConstraintRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class TestRemovePreferredIdentifierDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public TestRemovePreferredIdentifierDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierDeletingRule");
				EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class TestRemovePreferredIdentifierObjectificationAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public TestRemovePreferredIdentifierObjectificationAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierObjectificationAddRule");
				EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierObjectificationAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierObjectificationAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class TestRemovePreferredIdentifierObjectificationRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public TestRemovePreferredIdentifierObjectificationRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierObjectificationRolePlayerChangeRule");
				EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierObjectificationRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierObjectificationRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class TestRemovePreferredIdentifierRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public TestRemovePreferredIdentifierRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierRoleAddRule");
				EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierRoleAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class TestRemovePreferredIdentifierRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public TestRemovePreferredIdentifierRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierRolePlayerChangeRule");
				EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier.TestRemovePreferredIdentifierRolePlayerChangeRule");
			}
		}
	}
	#endregion // Rule classes for EntityTypeHasPreferredIdentifier
	#region Rule classes for EntityTypeInstance
	partial class EntityTypeInstance
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasEntityTypeInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EntityTypeInstanceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EntityTypeInstanceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(EntityTypeHasEntityTypeInstance)
			/// /// </summary>
			/// private static void EntityTypeInstanceAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.EntityTypeInstanceAddedRule");
				EntityTypeInstance.EntityTypeInstanceAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.EntityTypeInstanceAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EntityTypeInstanceDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EntityTypeInstanceDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.EntityTypeInstanceDeletingRule");
				EntityTypeInstance.EntityTypeInstanceDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.EntityTypeInstanceDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeInstanceHasRoleInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EntityTypeRoleInstanceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EntityTypeRoleInstanceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(EntityTypeInstanceHasRoleInstance)
			/// /// </summary>
			/// private static void EntityTypeRoleInstanceAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.EntityTypeRoleInstanceAddedRule");
				EntityTypeInstance.EntityTypeRoleInstanceAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.EntityTypeRoleInstanceAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeInstanceHasRoleInstance), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class EntityTypeRoleInstanceDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EntityTypeRoleInstanceDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance
			/// /// <summary>
			/// /// DeleteRule: typeof(EntityTypeInstanceHasRoleInstance), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void EntityTypeRoleInstanceDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.EntityTypeRoleInstanceDeletedRule");
				EntityTypeInstance.EntityTypeRoleInstanceDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.EntityTypeRoleInstanceDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectificationAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectificationAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(Objectification)
			/// /// </summary>
			/// private static void ObjectificationAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.ObjectificationAddedRule");
				EntityTypeInstance.ObjectificationAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.ObjectificationAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=(1 + ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority))]
		private sealed class ObjectificationDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectificationDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance
			/// /// <summary>
			/// /// DeleteRule: typeof(Objectification), Priority=1;
			/// /// </summary>
			/// private static void ObjectificationDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.ObjectificationDeletedRule");
				EntityTypeInstance.ObjectificationDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.ObjectificationDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=(1 + ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority))]
		private sealed class ObjectificationRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectificationRolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(Objectification), Priority=1;
			/// /// </summary>
			/// private static void ObjectificationRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.ObjectificationRolePlayerChangedRule");
				EntityTypeInstance.ObjectificationRolePlayerChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.ObjectificationRolePlayerChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class PreferredIdentifierAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.PreferredIdentifierAddedRule");
				EntityTypeInstance.PreferredIdentifierAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.PreferredIdentifierAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class PreferredIdentifierDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance
			/// /// <summary>
			/// /// DeleteRule: typeof(EntityTypeHasPreferredIdentifier)
			/// /// </summary>
			/// private static void PreferredIdentifierDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.PreferredIdentifierDeletedRule");
				EntityTypeInstance.PreferredIdentifierDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.PreferredIdentifierDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class PreferredIdentifierRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierRoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void PreferredIdentifierRoleAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.PreferredIdentifierRoleAddedRule");
				EntityTypeInstance.PreferredIdentifierRoleAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.PreferredIdentifierRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class PreferredIdentifierRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierRoleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance
			/// /// <summary>
			/// /// DeleteRule: typeof(ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void PreferredIdentifierRoleDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.PreferredIdentifierRoleDeletedRule");
				EntityTypeInstance.PreferredIdentifierRoleDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.PreferredIdentifierRoleDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class PreferredIdentifierRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierRolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(EntityTypeHasPreferredIdentifier)
			/// /// </summary>
			/// private static void PreferredIdentifierRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.PreferredIdentifierRolePlayerChangedRule");
				EntityTypeInstance.PreferredIdentifierRolePlayerChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.PreferredIdentifierRolePlayerChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RoleInstanceHasPopulationUniquenessError), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleInstanceHasPopulationUniquenessErrorDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleInstanceHasPopulationUniquenessErrorDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.RoleInstanceHasPopulationUniquenessErrorDeletedRule");
				EntityTypeInstance.RoleInstanceHasPopulationUniquenessErrorDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeInstance.RoleInstanceHasPopulationUniquenessErrorDeletedRule");
			}
		}
	}
	#endregion // Rule classes for EntityTypeInstance
	#region Rule classes for ExclusiveOrConstraintCoupler
	partial class ExclusiveOrConstraintCoupler
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ExclusiveOrConstraintCoupler), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class CouplerAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public CouplerAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler.CouplerAddRule");
				ExclusiveOrConstraintCoupler.CouplerAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler.CouplerAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ExclusiveOrConstraintCoupler), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class CouplerDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public CouplerDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler.CouplerDeleteRule");
				ExclusiveOrConstraintCoupler.CouplerDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler.CouplerDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ExclusionConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ExclusionConstraintChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ExclusionConstraintChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler.ExclusionConstraintChangeRule");
				ExclusiveOrConstraintCoupler.ExclusionConstraintChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler.ExclusionConstraintChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(MandatoryConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class MandatoryConstraintChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public MandatoryConstraintChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler.MandatoryConstraintChangeRule");
				ExclusiveOrConstraintCoupler.MandatoryConstraintChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler.MandatoryConstraintChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler.RoleAddRule");
				ExclusiveOrConstraintCoupler.RoleAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler.RoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed partial class RoleDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler.RoleDeletingRule");
				this.RoleDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler.RoleDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePositionChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePositionChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler.RolePositionChangeRule");
				ExclusiveOrConstraintCoupler.RolePositionChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler.RolePositionChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetComparisonConstraintHasRoleSequence), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleSequenceAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleSequenceAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler.RoleSequenceAddRule");
				ExclusiveOrConstraintCoupler.RoleSequenceAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler.RoleSequenceAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetComparisonConstraintHasRoleSequence), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleSequencePositionChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleSequencePositionChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler.RoleSequencePositionChangeRule");
				ExclusiveOrConstraintCoupler.RoleSequencePositionChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler.RoleSequencePositionChangeRule");
			}
		}
	}
	#endregion // Rule classes for ExclusiveOrConstraintCoupler
	#region Rule classes for ElementGrouping
	partial class ElementGrouping
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ElementGrouping), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class GroupingChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public GroupingChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping
			/// /// <summary>
			/// /// ChangeRule: typeof(ElementGrouping)
			/// /// </summary>
			/// private static void GroupingChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping.GroupingChangedRule");
				ElementGrouping.GroupingChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping.GroupingChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ElementGroupingIsOfElementGroupingType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class GroupingTypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public GroupingTypeAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping
			/// /// <summary>
			/// /// AddRule: typeof(ElementGroupingIsOfElementGroupingType)
			/// /// </summary>
			/// private static void GroupingTypeAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping.GroupingTypeAddedRule");
				ElementGrouping.GroupingTypeAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping.GroupingTypeAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ElementGroupingIsOfElementGroupingType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class GroupingTypeDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public GroupingTypeDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping
			/// /// <summary>
			/// /// DeleteRule: typeof(ElementGroupingIsOfElementGroupingType)
			/// /// </summary>
			/// private static void GroupingTypeDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping.GroupingTypeDeletedRule");
				ElementGrouping.GroupingTypeDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping.GroupingTypeDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(GroupingElementExclusion), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class GroupingExclusionAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public GroupingExclusionAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping
			/// /// <summary>
			/// /// AddRule: typeof(GroupingElementExclusion)
			/// /// </summary>
			/// private static void GroupingExclusionAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping.GroupingExclusionAddedRule");
				ElementGrouping.GroupingExclusionAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping.GroupingExclusionAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(GroupingElementExclusion), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class GroupingExclusionDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public GroupingExclusionDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping
			/// /// <summary>
			/// /// DeleteRule: typeof(GroupingElementExclusion)
			/// /// </summary>
			/// private static void GroupingExclusionDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping.GroupingExclusionDeletedRule");
				ElementGrouping.GroupingExclusionDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping.GroupingExclusionDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMNamedElement), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class StandardNamedElementNameChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public StandardNamedElementNameChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping
			/// /// <summary>
			/// /// ChangeRule: typeof(ORMNamedElement)
			/// /// </summary>
			/// private static void StandardNamedElementNameChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping.StandardNamedElementNameChangedRule");
				ElementGrouping.StandardNamedElementNameChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping.StandardNamedElementNameChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeNameChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeNameChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping.FactTypeNameChangedRule");
				ElementGrouping.FactTypeNameChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping.FactTypeNameChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ModelNote), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ModelNoteTextChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ModelNoteTextChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping
			/// /// <summary>
			/// /// ChangeRule: typeof(ModelNote)
			/// /// </summary>
			/// private static void ModelNoteTextChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping.ModelNoteTextChangedRule");
				ElementGrouping.ModelNoteTextChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGrouping.ModelNoteTextChangedRule");
			}
		}
	}
	#endregion // Rule classes for ElementGrouping
	#region Rule classes for ElementGroupingSet
	partial class ElementGroupingSet
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ElementGroupingHasDuplicateNameError), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DuplicateGroupingNameGroupingDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DuplicateGroupingNameGroupingDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGroupingSet
			/// /// <summary>
			/// /// DeleteRule: typeof(ElementGroupingHasDuplicateNameError)
			/// /// </summary>
			/// private static void DuplicateGroupingNameGroupingDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGroupingSet.DuplicateGroupingNameGroupingDeletedRule");
				ElementGroupingSet.DuplicateGroupingNameGroupingDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGroupingSet.DuplicateGroupingNameGroupingDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ElementGroupingSetContainsElementGrouping), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class GroupingDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public GroupingDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGroupingSet
			/// /// <summary>
			/// /// DeleteRule: typeof(ElementGroupingSetContainsElementGrouping), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void GroupingDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGroupingSet.GroupingDeletedRule");
				ElementGroupingSet.GroupingDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGroupingSet.GroupingDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ElementGroupingSet), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class GroupingSetAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public GroupingSetAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGroupingSet
			/// /// <summary>
			/// /// AddRule: typeof(ElementGroupingSet)
			/// /// </summary>
			/// private static void GroupingSetAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGroupingSet.GroupingSetAddedRule");
				ElementGroupingSet.GroupingSetAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ElementGroupingSet.GroupingSetAddedRule");
			}
		}
	}
	#endregion // Rule classes for ElementGroupingSet
	#region Rule classes for FactType
	partial class FactType
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class BlockRoleMigrationRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public BlockRoleMigrationRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.BlockRoleMigrationRule");
				FactType.BlockRoleMigrationRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.BlockRoleMigrationRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ModelHasFactType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.FactTypeAddedRule");
				FactType.FactTypeAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.FactTypeAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.FactTypeChangeRule");
				FactType.FactTypeChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.FactTypeChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeDerivationExpression), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeDerivationExpressionChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeDerivationExpressionChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.FactTypeDerivationExpressionChangeRule");
				FactType.FactTypeDerivationExpressionChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.FactTypeDerivationExpressionChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasReadingOrder), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeHasReadingOrderAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeHasReadingOrderAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.FactTypeHasReadingOrderAddRule");
				FactType.FactTypeHasReadingOrderAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.FactTypeHasReadingOrderAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasReadingOrder), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeHasReadingOrderDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeHasReadingOrderDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.FactTypeHasReadingOrderDeleteRule");
				FactType.FactTypeHasReadingOrderDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.FactTypeHasReadingOrderDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeHasRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeHasRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.FactTypeHasRoleAddRule");
				FactType.FactTypeHasRoleAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.FactTypeHasRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeHasRoleDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeHasRoleDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.FactTypeHasRoleDeleteRule");
				FactType.FactTypeHasRoleDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.FactTypeHasRoleDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactSetConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class InternalConstraintAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public InternalConstraintAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.InternalConstraintAddRule");
				FactType.InternalConstraintAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.InternalConstraintAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactSetConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class InternalConstraintDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public InternalConstraintDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.InternalConstraintDeleteRule");
				FactType.InternalConstraintDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.InternalConstraintDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class InternalConstraintCollectionHasConstraintAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public InternalConstraintCollectionHasConstraintAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.InternalConstraintCollectionHasConstraintAddedRule");
				FactType.InternalConstraintCollectionHasConstraintAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.InternalConstraintCollectionHasConstraintAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class InternalConstraintCollectionHasConstraintDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public InternalConstraintCollectionHasConstraintDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.InternalConstraintCollectionHasConstraintDeleteRule");
				FactType.InternalConstraintCollectionHasConstraintDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.InternalConstraintCollectionHasConstraintDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(UniquenessConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class InternalUniquenessConstraintChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public InternalUniquenessConstraintChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.InternalUniquenessConstraintChangeRule");
				FactType.InternalUniquenessConstraintChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.InternalUniquenessConstraintChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReadingOrderHasReading), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ReadingOrderHasReadingAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReadingOrderHasReadingAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ReadingOrderHasReadingAddRule");
				FactType.ReadingOrderHasReadingAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ReadingOrderHasReadingAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReadingOrderHasReading), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ReadingOrderHasReadingDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReadingOrderHasReadingDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ReadingOrderHasReadingDeleteRule");
				FactType.ReadingOrderHasReadingDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ReadingOrderHasReadingDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=(1 + ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority))]
		private sealed class ValidateFactTypeNameForObjectificationAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValidateFactTypeNameForObjectificationAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ValidateFactTypeNameForObjectificationAddedRule");
				FactType.ValidateFactTypeNameForObjectificationAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ValidateFactTypeNameForObjectificationAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=(1 + ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority))]
		private sealed class ValidateFactTypeNameForObjectificationDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValidateFactTypeNameForObjectificationDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ValidateFactTypeNameForObjectificationDeleteRule");
				FactType.ValidateFactTypeNameForObjectificationDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ValidateFactTypeNameForObjectificationDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=(1 + ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority))]
		private sealed class ValidateFactTypeNameForObjectificationRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValidateFactTypeNameForObjectificationRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ValidateFactTypeNameForObjectificationRolePlayerChangeRule");
				FactType.ValidateFactTypeNameForObjectificationRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ValidateFactTypeNameForObjectificationRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValidateFactTypeNameForObjectTypeNameChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValidateFactTypeNameForObjectTypeNameChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ValidateFactTypeNameForObjectTypeNameChangeRule");
				FactType.ValidateFactTypeNameForObjectTypeNameChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ValidateFactTypeNameForObjectTypeNameChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Reading), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValidateFactTypeNameForReadingChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValidateFactTypeNameForReadingChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ValidateFactTypeNameForReadingChangeRule");
				FactType.ValidateFactTypeNameForReadingChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ValidateFactTypeNameForReadingChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasReadingOrder), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValidateFactTypeNameForReadingOrderReorderRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValidateFactTypeNameForReadingOrderReorderRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ValidateFactTypeNameForReadingOrderReorderRule");
				FactType.ValidateFactTypeNameForReadingOrderReorderRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ValidateFactTypeNameForReadingOrderReorderRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReadingOrderHasReading), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValidateFactTypeNameForReadingReorderRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValidateFactTypeNameForReadingReorderRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ValidateFactTypeNameForReadingReorderRule");
				FactType.ValidateFactTypeNameForReadingReorderRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ValidateFactTypeNameForReadingReorderRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValidateFactTypeNameForRolePlayerAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValidateFactTypeNameForRolePlayerAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ValidateFactTypeNameForRolePlayerAddedRule");
				FactType.ValidateFactTypeNameForRolePlayerAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ValidateFactTypeNameForRolePlayerAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValidateFactTypeNameForRolePlayerDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValidateFactTypeNameForRolePlayerDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ValidateFactTypeNameForRolePlayerDeleteRule");
				FactType.ValidateFactTypeNameForRolePlayerDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ValidateFactTypeNameForRolePlayerDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValidateFactTypeNameForRolePlayerRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValidateFactTypeNameForRolePlayerRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ValidateFactTypeNameForRolePlayerRolePlayerChangeRule");
				FactType.ValidateFactTypeNameForRolePlayerRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.ValidateFactTypeNameForRolePlayerRolePlayerChangeRule");
			}
		}
	}
	#endregion // Rule classes for FactType
	#region Rule classes for FactType.UnaryBinarizationUtility
	partial class FactType
	{
		partial class UnaryBinarizationUtility
		{
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ConstraintRoleSequenceHasRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ConstraintRoleSequenceHasRoleAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility
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
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility.ConstraintRoleSequenceHasRoleAddedRule");
					UnaryBinarizationUtility.ConstraintRoleSequenceHasRoleAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility.ConstraintRoleSequenceHasRoleAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ConstraintRoleSequenceHasRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ConstraintRoleSequenceHasRoleDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility
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
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility.ConstraintRoleSequenceHasRoleDeletedRule");
					UnaryBinarizationUtility.ConstraintRoleSequenceHasRoleDeletedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility.ConstraintRoleSequenceHasRoleDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeHasRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeHasRoleAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility
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
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility.FactTypeHasRoleAddedRule");
					UnaryBinarizationUtility.FactTypeHasRoleAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility.FactTypeHasRoleAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeHasRoleDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeHasRoleDeletingRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility
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
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility.FactTypeHasRoleDeletingRule");
					UnaryBinarizationUtility.FactTypeHasRoleDeletingRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility.FactTypeHasRoleDeletingRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeNameChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeNameChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility
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
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility.FactTypeNameChangedRule");
					UnaryBinarizationUtility.FactTypeNameChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility.FactTypeNameChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ObjectTypePlaysRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ObjectTypePlaysRoleAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility
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
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility.ObjectTypePlaysRoleAddedRule");
					UnaryBinarizationUtility.ObjectTypePlaysRoleAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility.ObjectTypePlaysRoleAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ObjectTypePlaysRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ObjectTypePlaysRoleDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility
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
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility.ObjectTypePlaysRoleDeletedRule");
					UnaryBinarizationUtility.ObjectTypePlaysRoleDeletedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility.ObjectTypePlaysRoleDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ObjectTypePlaysRoleRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ObjectTypePlaysRoleRolePlayerChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility
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
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility.ObjectTypePlaysRoleRolePlayerChangedRule");
					UnaryBinarizationUtility.ObjectTypePlaysRoleRolePlayerChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility.ObjectTypePlaysRoleRolePlayerChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Role), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class RoleNameChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public RoleNameChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility
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
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility.RoleNameChangedRule");
					UnaryBinarizationUtility.RoleNameChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactType.UnaryBinarizationUtility.RoleNameChangedRule");
				}
			}
		}
	}
	#endregion // Rule classes for FactType.UnaryBinarizationUtility
	#region Rule classes for FactTypeDerivationExpression
	partial class FactTypeDerivationExpression
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasDerivationExpression), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DerivationExpressionAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DerivationExpressionAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeDerivationExpression
			/// /// <summary>
			/// /// AddRule: typeof(FactTypeHasDerivationExpression)
			/// /// </summary>
			/// private static void DerivationExpressionAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeDerivationExpression.DerivationExpressionAddedRule");
				FactTypeDerivationExpression.DerivationExpressionAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeDerivationExpression.DerivationExpressionAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeDerivationExpression), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DerivationExpressionChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DerivationExpressionChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeDerivationExpression
			/// /// <summary>
			/// /// ChangeRule: typeof(FactTypeDerivationExpression)
			/// /// </summary>
			/// private static void DerivationExpressionChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeDerivationExpression.DerivationExpressionChangedRule");
				FactTypeDerivationExpression.DerivationExpressionChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeDerivationExpression.DerivationExpressionChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasDerivationRule), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DerivationRuleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DerivationRuleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeDerivationExpression
			/// /// <summary>
			/// /// AddRule: typeof(FactTypeHasDerivationRule)
			/// /// </summary>
			/// private static void DerivationRuleAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeDerivationExpression.DerivationRuleAddedRule");
				FactTypeDerivationExpression.DerivationRuleAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeDerivationExpression.DerivationRuleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeDerivationRule), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DerivationRuleChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DerivationRuleChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeDerivationExpression
			/// /// <summary>
			/// /// ChangeRule: typeof(FactTypeDerivationRule)
			/// /// </summary>
			/// private static void DerivationRuleChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeDerivationExpression.DerivationRuleChangedRule");
				FactTypeDerivationExpression.DerivationRuleChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeDerivationExpression.DerivationRuleChangedRule");
			}
		}
	}
	#endregion // Rule classes for FactTypeDerivationExpression
	#region Rule classes for FactTypeInstance
	partial class FactTypeInstance
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasFactTypeInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeInstanceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeInstanceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(FactTypeHasFactTypeInstance)
			/// /// </summary>
			/// private static void FactTypeInstanceAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance.FactTypeInstanceAddedRule");
				FactTypeInstance.FactTypeInstanceAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance.FactTypeInstanceAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeRoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(FactTypeHasRole)
			/// /// </summary>
			/// private static void FactTypeRoleAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance.FactTypeRoleAddedRule");
				FactTypeInstance.FactTypeRoleAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance.FactTypeRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeRoleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance
			/// /// <summary>
			/// /// DeleteRule: typeof(FactTypeHasRole)
			/// /// </summary>
			/// private static void FactTypeRoleDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance.FactTypeRoleDeletedRule");
				FactTypeInstance.FactTypeRoleDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance.FactTypeRoleDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeInstanceHasRoleInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeRoleInstanceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeRoleInstanceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(FactTypeInstanceHasRoleInstance)
			/// /// </summary>
			/// private static void FactTypeRoleInstanceAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance.FactTypeRoleInstanceAddedRule");
				FactTypeInstance.FactTypeRoleInstanceAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance.FactTypeRoleInstanceAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeNameChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeNameChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance.FactTypeNameChangedRule");
				FactTypeInstance.FactTypeNameChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance.FactTypeNameChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeInstanceHasRoleInstance), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class FactTypeRoleInstanceDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeRoleInstanceDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance
			/// /// <summary>
			/// /// DeleteRule: typeof(FactTypeInstanceHasRoleInstance), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void FactTypeRoleInstanceDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance.FactTypeRoleInstanceDeletedRule");
				FactTypeInstance.FactTypeRoleInstanceDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance.FactTypeRoleInstanceDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeInstanceHasRoleInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeInstanceHasRoleInstanceRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeInstanceHasRoleInstanceRolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(FactTypeInstanceHasRoleInstance)
			/// /// </summary>
			/// private static void FactTypeInstanceHasRoleInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance.FactTypeInstanceHasRoleInstanceRolePlayerChangedRule");
				FactTypeInstance.FactTypeInstanceHasRoleInstanceRolePlayerChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance.FactTypeInstanceHasRoleInstanceRolePlayerChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeRoleInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeRoleInstanceRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeRoleInstanceRolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(FactTypeRoleInstance)
			/// /// </summary>
			/// private static void FactTypeRoleInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance.FactTypeRoleInstanceRolePlayerChangedRule");
				FactTypeInstance.FactTypeRoleInstanceRolePlayerChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance.FactTypeRoleInstanceRolePlayerChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ImpliedBooleanRolePlayerDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ImpliedBooleanRolePlayerDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance.ImpliedBooleanRolePlayerDeletedRule");
				FactTypeInstance.ImpliedBooleanRolePlayerDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance.ImpliedBooleanRolePlayerDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypeInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectTypeInstanceNameChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectTypeInstanceNameChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance
			/// /// <summary>
			/// /// ChangeRule: typeof(ObjectTypeInstance)
			/// /// </summary>
			/// private static void ObjectTypeInstanceNameChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance.ObjectTypeInstanceNameChangedRule");
				FactTypeInstance.ObjectTypeInstanceNameChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeInstance.ObjectTypeInstanceNameChangedRule");
			}
		}
	}
	#endregion // Rule classes for FactTypeInstance
	#region Rule classes for FrequencyConstraint
	partial class FrequencyConstraint
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FrequencyConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FrequencyConstraintMinMaxRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FrequencyConstraintMinMaxRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.FrequencyConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FrequencyConstraint.FrequencyConstraintMinMaxRule");
				FrequencyConstraint.FrequencyConstraintMinMaxRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.FrequencyConstraint.FrequencyConstraintMinMaxRule");
			}
		}
	}
	#endregion // Rule classes for FrequencyConstraint
	#region Rule classes for MandatoryConstraint
	partial class MandatoryConstraint
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(MandatoryConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class MandatoryConstraintChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public MandatoryConstraintChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.MandatoryConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.MandatoryConstraint.MandatoryConstraintChangeRule");
				MandatoryConstraint.MandatoryConstraintChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.MandatoryConstraint.MandatoryConstraintChangeRule");
			}
		}
	}
	#endregion // Rule classes for MandatoryConstraint
	#region Rule classes for ModelError
	partial class ModelError
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMModel), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class SynchronizeErrorTextForModelRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SynchronizeErrorTextForModelRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ModelError
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ModelError.SynchronizeErrorTextForModelRule");
				ModelError.SynchronizeErrorTextForModelRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ModelError.SynchronizeErrorTextForModelRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMNamedElement), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class SynchronizeErrorTextForOwnerRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SynchronizeErrorTextForOwnerRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ModelError
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ModelError.SynchronizeErrorTextForOwnerRule");
				ModelError.SynchronizeErrorTextForOwnerRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ModelError.SynchronizeErrorTextForOwnerRule");
			}
		}
	}
	#endregion // Rule classes for ModelError
	#region Rule classes for NameGenerator
	partial class NameGenerator
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(NameGenerator), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed partial class SynchronizedRefinementsPropertyChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SynchronizedRefinementsPropertyChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.NameGenerator
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.NameGenerator.SynchronizedRefinementsPropertyChangedRule");
				this.SynchronizedRefinementsPropertyChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.NameGenerator.SynchronizedRefinementsPropertyChangedRule");
			}
		}
	}
	#endregion // Rule classes for NameGenerator
	#region Rule classes for Note
	partial class Note
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Note), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class NoteChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public NoteChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Note
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Note.NoteChangeRule");
				Note.NoteChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Note.NoteChangeRule");
			}
		}
	}
	#endregion // Rule classes for Note
	#region Rule classes for Objectification
	partial class Objectification
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectificationImpliesFactType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ImpliedFactTypeAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ImpliedFactTypeAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ImpliedFactTypeAddRule");
				Objectification.ImpliedFactTypeAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ImpliedFactTypeAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ImpliedObjectificationConstraintRoleSequenceHasRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ImpliedObjectificationConstraintRoleSequenceHasRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ImpliedObjectificationConstraintRoleSequenceHasRoleAddRule");
				Objectification.ImpliedObjectificationConstraintRoleSequenceHasRoleAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ImpliedObjectificationConstraintRoleSequenceHasRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ImpliedObjectificationConstraintRoleSequenceHasRoleDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ImpliedObjectificationConstraintRoleSequenceHasRoleDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ImpliedObjectificationConstraintRoleSequenceHasRoleDeletingRule");
				Objectification.ImpliedObjectificationConstraintRoleSequenceHasRoleDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ImpliedObjectificationConstraintRoleSequenceHasRoleDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ImpliedObjectificationFactTypeHasRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ImpliedObjectificationFactTypeHasRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ImpliedObjectificationFactTypeHasRoleAddRule");
				Objectification.ImpliedObjectificationFactTypeHasRoleAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ImpliedObjectificationFactTypeHasRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ImpliedObjectificationFactTypeHasRoleDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ImpliedObjectificationFactTypeHasRoleDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ImpliedObjectificationFactTypeHasRoleDeletingRule");
				Objectification.ImpliedObjectificationFactTypeHasRoleDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ImpliedObjectificationFactTypeHasRoleDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ImpliedObjectificationIsImpliedChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ImpliedObjectificationIsImpliedChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ImpliedObjectificationIsImpliedChangeRule");
				Objectification.ImpliedObjectificationIsImpliedChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ImpliedObjectificationIsImpliedChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ImpliedObjectificationObjectifyingTypeIsIndependentChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ImpliedObjectificationObjectifyingTypeIsIndependentChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ImpliedObjectificationObjectifyingTypeIsIndependentChangeRule");
				Objectification.ImpliedObjectificationObjectifyingTypeIsIndependentChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ImpliedObjectificationObjectifyingTypeIsIndependentChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ImpliedObjectificationObjectifyingTypePlaysRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ImpliedObjectificationObjectifyingTypePlaysRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ImpliedObjectificationObjectifyingTypePlaysRoleAddRule");
				Objectification.ImpliedObjectificationObjectifyingTypePlaysRoleAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ImpliedObjectificationObjectifyingTypePlaysRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(UniquenessConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ImpliedObjectificationUniquenessConstraintChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ImpliedObjectificationUniquenessConstraintChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ImpliedObjectificationUniquenessConstraintChangeRule");
				Objectification.ImpliedObjectificationUniquenessConstraintChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ImpliedObjectificationUniquenessConstraintChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class InternalConstraintChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public InternalConstraintChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.InternalConstraintChangeRule");
				Objectification.InternalConstraintChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.InternalConstraintChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectificationAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectificationAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ObjectificationAddRule");
				Objectification.ObjectificationAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ObjectificationAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectificationDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectificationDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ObjectificationDeletingRule");
				Objectification.ObjectificationDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ObjectificationDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectificationRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectificationRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ObjectificationRolePlayerChangeRule");
				Objectification.ObjectificationRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.ObjectificationRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class PreferredIdentifierDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.PreferredIdentifierDeletingRule");
				Objectification.PreferredIdentifierDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.PreferredIdentifierDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class PreferredIdentifierRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.PreferredIdentifierRolePlayerChangeRule");
				Objectification.PreferredIdentifierRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.PreferredIdentifierRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactSetConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.RoleAddRule");
				Objectification.RoleAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.RoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed partial class RoleDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.RoleDeletingRule");
				this.RoleDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.RoleDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.RolePlayerAddRule");
				Objectification.RolePlayerAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.RolePlayerAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.RolePlayerDeletingRule");
				Objectification.RolePlayerDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.RolePlayerDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.RolePlayerRolePlayerChangeRule");
				Objectification.RolePlayerRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.RolePlayerRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ModelHasSetConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class UniquenessConstraintAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public UniquenessConstraintAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.UniquenessConstraintAddRule");
				Objectification.UniquenessConstraintAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.UniquenessConstraintAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ModelHasSetConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class UniquenessConstraintDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public UniquenessConstraintDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.UniquenessConstraintDeletingRule");
				Objectification.UniquenessConstraintDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification.UniquenessConstraintDeletingRule");
			}
		}
	}
	#endregion // Rule classes for Objectification
	#region Rule classes for ObjectificationInstance
	partial class ObjectificationInstance
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectificationInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectificationInstanceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectificationInstanceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectificationInstance
			/// /// <summary>
			/// /// AddRule: typeof(ObjectificationInstance)
			/// /// </summary>
			/// private static void ObjectificationInstanceAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectificationInstance.ObjectificationInstanceAddedRule");
				ObjectificationInstance.ObjectificationInstanceAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectificationInstance.ObjectificationInstanceAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectificationInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectificationInstanceDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectificationInstanceDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectificationInstance
			/// /// <summary>
			/// /// DeletingRule: typeof(ObjectificationInstance)
			/// /// </summary>
			/// private static void ObjectificationInstanceDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectificationInstance.ObjectificationInstanceDeletingRule");
				ObjectificationInstance.ObjectificationInstanceDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectificationInstance.ObjectificationInstanceDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectificationInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectificationInstanceRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectificationInstanceRolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectificationInstance
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ObjectificationInstance)
			/// /// </summary>
			/// private static void ObjectificationInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectificationInstance.ObjectificationInstanceRolePlayerChangedRule");
				ObjectificationInstance.ObjectificationInstanceRolePlayerChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectificationInstance.ObjectificationInstanceRolePlayerChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RoleInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed partial class RoleInstanceRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleInstanceRolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectificationInstance
			/// partial class RoleInstanceRolePlayerChangedRuleClass
			/// {
			/// 	/// <summary>
			/// 	/// RolePlayerChangeRule: typeof(RoleInstance)
			/// 	/// </summary>
			/// 	private void RoleInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			/// 	{
			/// 	}
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectificationInstance.RoleInstanceRolePlayerChangedRule");
				this.RoleInstanceRolePlayerChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectificationInstance.RoleInstanceRolePlayerChangedRule");
			}
		}
	}
	#endregion // Rule classes for ObjectificationInstance
	#region Rule classes for ObjectType
	partial class ObjectType
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
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
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.CheckForIncompatibleRelationshipAddRule");
				ObjectType.CheckForIncompatibleRelationshipAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.CheckForIncompatibleRelationshipAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
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
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.CheckForIncompatibleRelationshipRolePlayerChangeRule");
				ObjectType.CheckForIncompatibleRelationshipRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.CheckForIncompatibleRelationshipRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class CheckIsIndependentRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public CheckIsIndependentRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.CheckIsIndependentRolePlayerChangeRule");
				ObjectType.CheckIsIndependentRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.CheckIsIndependentRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.ConstraintRoleAddedRule");
				ObjectType.ConstraintRoleAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.ConstraintRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRoleDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.ConstraintRoleDeletingRule");
				ObjectType.ConstraintRoleDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.ConstraintRoleDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ExclusionConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ExclusionModalityChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ExclusionModalityChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.ExclusionModalityChangeRule");
				ObjectType.ExclusionModalityChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.ExclusionModalityChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetComparisonConstraintHasRoleSequence), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ExclusionSequenceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ExclusionSequenceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.ExclusionSequenceAddedRule");
				ObjectType.ExclusionSequenceAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.ExclusionSequenceAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(MandatoryConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class MandatoryModalityChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public MandatoryModalityChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.MandatoryModalityChangeRule");
				ObjectType.MandatoryModalityChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.MandatoryModalityChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ModelHasObjectType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectTypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectTypeAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.ObjectTypeAddedRule");
				ObjectType.ObjectTypeAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.ObjectTypeAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectTypeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectTypeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.ObjectTypeChangeRule");
				ObjectType.ObjectTypeChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.ObjectTypeChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectTypeDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectTypeDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.ObjectTypeDeletingRule");
				ObjectType.ObjectTypeDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.ObjectTypeDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SubtypeDerivationExpression), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class SubtypeDerivationExpressionChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SubtypeDerivationExpressionChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType
			/// /// <summary>
			/// /// ChangeRule: typeof(SubtypeDerivationExpression)
			/// /// </summary>
			/// private static void SubtypeDerivationExpressionChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.SubtypeDerivationExpressionChangeRule");
				ObjectType.SubtypeDerivationExpressionChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.SubtypeDerivationExpressionChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SubtypeFact), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed partial class SubtypeFactChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SubtypeFactChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.SubtypeFactChangeRule");
				this.SubtypeFactChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.SubtypeFactChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class SupertypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SupertypeAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.SupertypeAddedRule");
				ObjectType.SupertypeAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.SupertypeAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class SupertypeDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SupertypeDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.SupertypeDeleteRule");
				ObjectType.SupertypeDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.SupertypeDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class SupertypeDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SupertypeDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.SupertypeDeletingRule");
				ObjectType.SupertypeDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.SupertypeDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class UnspecifiedDataTypeAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public UnspecifiedDataTypeAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.UnspecifiedDataTypeAddRule");
				ObjectType.UnspecifiedDataTypeAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.UnspecifiedDataTypeAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class UnspecifiedDataTypeRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public UnspecifiedDataTypeRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.UnspecifiedDataTypeRolePlayerChangeRule");
				ObjectType.UnspecifiedDataTypeRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.UnspecifiedDataTypeRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class VerifyReferenceSchemeAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public VerifyReferenceSchemeAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.VerifyReferenceSchemeAddRule");
				ObjectType.VerifyReferenceSchemeAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.VerifyReferenceSchemeAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class VerifyReferenceSchemeDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public VerifyReferenceSchemeDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.VerifyReferenceSchemeDeleteRule");
				ObjectType.VerifyReferenceSchemeDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.VerifyReferenceSchemeDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class VerifyReferenceSchemeRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public VerifyReferenceSchemeRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.VerifyReferenceSchemeRolePlayerChangeRule");
				ObjectType.VerifyReferenceSchemeRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.VerifyReferenceSchemeRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class VerifyValueTypeHasDataTypeAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public VerifyValueTypeHasDataTypeAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.VerifyValueTypeHasDataTypeAddRule");
				ObjectType.VerifyValueTypeHasDataTypeAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.VerifyValueTypeHasDataTypeAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class VerifyValueTypeHasDataTypeDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public VerifyValueTypeHasDataTypeDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.VerifyValueTypeHasDataTypeDeleteRule");
				ObjectType.VerifyValueTypeHasDataTypeDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType.VerifyValueTypeHasDataTypeDeleteRule");
			}
		}
	}
	#endregion // Rule classes for ObjectType
	#region Rule classes for ObjectTypeInstance
	partial class ObjectTypeInstance
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRoleSequenceHasRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleSequenceHasRoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.ConstraintRoleSequenceHasRoleAddedRule");
				ObjectTypeInstance.ConstraintRoleSequenceHasRoleAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.ConstraintRoleSequenceHasRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRoleSequenceHasRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleSequenceHasRoleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.ConstraintRoleSequenceHasRoleDeletedRule");
				ObjectTypeInstance.ConstraintRoleSequenceHasRoleDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.ConstraintRoleSequenceHasRoleDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeInstanceHasRoleInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EntityTypeInstanceHasRoleInstanceRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EntityTypeInstanceHasRoleInstanceRolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.EntityTypeInstanceHasRoleInstanceRolePlayerChangedRule");
				ObjectTypeInstance.EntityTypeInstanceHasRoleInstanceRolePlayerChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.EntityTypeInstanceHasRoleInstanceRolePlayerChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeSubtypeInstanceHasSupertypeInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EntityTypeSupertypeInstanceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EntityTypeSupertypeInstanceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(EntityTypeSubtypeInstanceHasSupertypeInstance)
			/// /// </summary>
			/// private static void EntityTypeSupertypeInstanceAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.EntityTypeSupertypeInstanceAddedRule");
				ObjectTypeInstance.EntityTypeSupertypeInstanceAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.EntityTypeSupertypeInstanceAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeSubtypeInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed partial class EntityTypeSubtypeInstanceDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EntityTypeSubtypeInstanceDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
			/// partial class EntityTypeSubtypeInstanceDeletingRuleClass
			/// {
			/// 	/// <summary>
			/// 	/// DeletingRule: typeof(EntityTypeSubtypeInstance)
			/// 	/// </summary>
			/// 	private void EntityTypeSubtypeInstanceDeletingRule(ElementDeletingEventArgs e)
			/// 	{
			/// 	}
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.EntityTypeSubtypeInstanceDeletingRule");
				this.EntityTypeSubtypeInstanceDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.EntityTypeSubtypeInstanceDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeSubtypeHasEntityTypeSubtypeInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EntityTypeSubtypeInstanceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EntityTypeSubtypeInstanceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(EntityTypeSubtypeHasEntityTypeSubtypeInstance)
			/// /// </summary>
			/// private static void EntityTypeSubtypeInstanceAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.EntityTypeSubtypeInstanceAddedRule");
				ObjectTypeInstance.EntityTypeSubtypeInstanceAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.EntityTypeSubtypeInstanceAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeSubtypeInstanceHasSupertypeInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EntityTypeSupertypeInstanceDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EntityTypeSupertypeInstanceDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// DeleteRule: typeof(EntityTypeSubtypeInstanceHasSupertypeInstance)
			/// /// </summary>
			/// private static void EntityTypeSupertypeInstanceDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.EntityTypeSupertypeInstanceDeletedRule");
				ObjectTypeInstance.EntityTypeSupertypeInstanceDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.EntityTypeSupertypeInstanceDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeSubtypeInstanceHasSupertypeInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EntityTypeSupertypeInstanceRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EntityTypeSupertypeInstanceRolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(EntityTypeSubtypeInstanceHasSupertypeInstance)
			/// /// </summary>
			/// private static void EntityTypeSupertypeInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.EntityTypeSupertypeInstanceRolePlayerChangedRule");
				ObjectTypeInstance.EntityTypeSupertypeInstanceRolePlayerChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.EntityTypeSupertypeInstanceRolePlayerChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeInstanceNameChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeInstanceNameChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// ChangeRule: typeof(FactTypeInstance)
			/// /// </summary>
			/// private static void FactTypeInstanceNameChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.FactTypeInstanceNameChangedRule");
				ObjectTypeInstance.FactTypeInstanceNameChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.FactTypeInstanceNameChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeNameChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeNameChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.FactTypeNameChangedRule");
				ObjectTypeInstance.FactTypeNameChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.FactTypeNameChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(MandatoryConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class MandatoryConstraintChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public MandatoryConstraintChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.MandatoryConstraintChangedRule");
				ObjectTypeInstance.MandatoryConstraintChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.MandatoryConstraintChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectificationAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectificationAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(Objectification)
			/// /// </summary>
			/// private static void ObjectificationAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.ObjectificationAddedRule");
				ObjectTypeInstance.ObjectificationAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.ObjectificationAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectificationDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectificationDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// DeleteRule: typeof(Objectification)
			/// /// </summary>
			/// private static void ObjectificationDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.ObjectificationDeletedRule");
				ObjectTypeInstance.ObjectificationDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.ObjectificationDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectificationInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectificationInstanceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectificationInstanceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(ObjectificationInstance)
			/// /// </summary>
			/// private static void ObjectificationInstanceAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.ObjectificationInstanceAddedRule");
				ObjectTypeInstance.ObjectificationInstanceAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.ObjectificationInstanceAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectificationInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectificationInstanceDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectificationInstanceDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// DeleteRule: typeof(ObjectificationInstance)
			/// /// </summary>
			/// private static void ObjectificationInstanceDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.ObjectificationInstanceDeletedRule");
				ObjectTypeInstance.ObjectificationInstanceDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.ObjectificationInstanceDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectificationInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectificationInstanceRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectificationInstanceRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ObjectificationInstance)
			/// /// </summary>
			/// private static void ObjectificationInstanceRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.ObjectificationInstanceRolePlayerChangeRule");
				ObjectTypeInstance.ObjectificationInstanceRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.ObjectificationInstanceRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Objectification), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectificationRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectificationRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.ObjectificationRolePlayerChangeRule");
				ObjectTypeInstance.ObjectificationRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.ObjectificationRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypeHasObjectTypeInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectTypeInstanceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectTypeInstanceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// AddRule: typeof(ObjectTypeHasObjectTypeInstance)
			/// /// </summary>
			/// private static void ObjectTypeInstanceAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.ObjectTypeInstanceAddedRule");
				ObjectTypeInstance.ObjectTypeInstanceAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.ObjectTypeInstanceAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypeHasObjectTypeInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectTypeInstanceRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectTypeInstanceRolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ObjectTypeHasObjectTypeInstance)
			/// /// </summary>
			/// private static void ObjectTypeInstanceRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.ObjectTypeInstanceRolePlayerChangedRule");
				ObjectTypeInstance.ObjectTypeInstanceRolePlayerChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.ObjectTypeInstanceRolePlayerChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RoleInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleInstanceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleInstanceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.RoleInstanceAddedRule");
				ObjectTypeInstance.RoleInstanceAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.RoleInstanceAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RoleInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleInstanceDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleInstanceDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.RoleInstanceDeletedRule");
				ObjectTypeInstance.RoleInstanceDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.RoleInstanceDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RoleInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleInstanceRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleInstanceRolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.RoleInstanceRolePlayerChangedRule");
				ObjectTypeInstance.RoleInstanceRolePlayerChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.RoleInstanceRolePlayerChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.RolePlayerAddedRule");
				ObjectTypeInstance.RolePlayerAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.RolePlayerAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.RolePlayerDeletedRule");
				ObjectTypeInstance.RolePlayerDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.RolePlayerDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerRolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.RolePlayerRolePlayerChangedRule");
				ObjectTypeInstance.RolePlayerRolePlayerChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.RolePlayerRolePlayerChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SubtypeFact), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class PreferredSupertypeChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredSupertypeChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance
			/// /// <summary>
			/// /// ChangeRule: typeof(SubtypeFact)
			/// /// </summary>
			/// private static void PreferredSupertypeChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.PreferredSupertypeChangedRule");
				ObjectTypeInstance.PreferredSupertypeChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeInstance.PreferredSupertypeChangedRule");
			}
		}
	}
	#endregion // Rule classes for ObjectTypeInstance
	#region Rule classes for ORMModel
	partial class ORMModel
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetComparisonConstraintHasDuplicateNameError), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetConstraintHasDuplicateNameError), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueConstraintHasDuplicateNameError), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DuplicateConstraintNameConstraintDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DuplicateConstraintNameConstraintDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel
			/// /// <summary>
			/// /// DeleteRule: typeof(SetComparisonConstraintHasDuplicateNameError)
			/// /// DeleteRule: typeof(SetConstraintHasDuplicateNameError)
			/// /// DeleteRule: typeof(ValueConstraintHasDuplicateNameError)
			/// /// </summary>
			/// private static void DuplicateConstraintNameConstraintDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel.DuplicateConstraintNameConstraintDeletedRule");
				ORMModel.DuplicateConstraintNameConstraintDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel.DuplicateConstraintNameConstraintDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypeHasDuplicateNameError), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DuplicateObjectTypeNameObjectTypeDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DuplicateObjectTypeNameObjectTypeDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel
			/// /// <summary>
			/// /// DeleteRule: typeof(ObjectTypeHasDuplicateNameError)
			/// /// </summary>
			/// private static void DuplicateObjectTypeNameObjectTypeDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel.DuplicateObjectTypeNameObjectTypeDeletedRule");
				ORMModel.DuplicateObjectTypeNameObjectTypeDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel.DuplicateObjectTypeNameObjectTypeDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RecognizedPhraseHasDuplicateNameError), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DuplicateRecognizedPhraseDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DuplicateRecognizedPhraseDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel
			/// /// <summary>
			/// /// DeleteRule: typeof(RecognizedPhraseHasDuplicateNameError)
			/// /// </summary>
			/// private static void DuplicateRecognizedPhraseDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel.DuplicateRecognizedPhraseDeletedRule");
				ORMModel.DuplicateRecognizedPhraseDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel.DuplicateRecognizedPhraseDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FunctionHasDuplicateNameError), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DuplicateFunctionNameDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DuplicateFunctionNameDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel
			/// /// <summary>
			/// /// DeleteRule: typeof(FunctionHasDuplicateNameError)
			/// /// </summary>
			/// private static void DuplicateFunctionNameDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel.DuplicateFunctionNameDeletedRule");
				ORMModel.DuplicateFunctionNameDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel.DuplicateFunctionNameDeletedRule");
			}
		}
	}
	#endregion // Rule classes for ORMModel
	#region Rule classes for Reading
	partial class Reading
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReadingOrderHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ReadingOrderHasRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReadingOrderHasRoleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Reading
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Reading.ReadingOrderHasRoleDeletedRule");
				Reading.ReadingOrderHasRoleDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Reading.ReadingOrderHasRoleDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Reading), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ReadingPropertiesChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReadingPropertiesChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Reading
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Reading.ReadingPropertiesChangedRule");
				Reading.ReadingPropertiesChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Reading.ReadingPropertiesChangedRule");
			}
		}
	}
	#endregion // Rule classes for Reading
	#region Rule classes for RecognizedPhrase
	partial class RecognizedPhrase
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RecognizedPhraseHasAbbreviation), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class RecognizedPhraseHasAbbreviationDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RecognizedPhraseHasAbbreviationDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.RecognizedPhrase
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RecognizedPhrase.RecognizedPhraseHasAbbreviationDeletedRule");
				RecognizedPhrase.RecognizedPhraseHasAbbreviationDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RecognizedPhrase.RecognizedPhraseHasAbbreviationDeletedRule");
			}
		}
	}
	#endregion // Rule classes for RecognizedPhrase
	#region Rule classes for ReadingOrder
	partial class ReadingOrder
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReadingOrderHasReading), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class EnforceNoEmptyReadingOrderDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceNoEmptyReadingOrderDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrder
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrder.EnforceNoEmptyReadingOrderDeleteRule");
				ReadingOrder.EnforceNoEmptyReadingOrderDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrder.EnforceNoEmptyReadingOrderDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReadingOrderHasReading), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class EnforceNoEmptyReadingOrderRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceNoEmptyReadingOrderRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrder
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrder.EnforceNoEmptyReadingOrderRolePlayerChangeRule");
				ReadingOrder.EnforceNoEmptyReadingOrderRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrder.EnforceNoEmptyReadingOrderRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeHasRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeHasRoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrder
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrder.FactTypeHasRoleAddedRule");
				ReadingOrder.FactTypeHasRoleAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrder.FactTypeHasRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReadingOrderHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ReadingOrderHasRoleDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReadingOrderHasRoleDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrder
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrder.ReadingOrderHasRoleDeletingRule");
				ReadingOrder.ReadingOrderHasRoleDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrder.ReadingOrderHasRoleDeletingRule");
			}
		}
	}
	#endregion // Rule classes for ReadingOrder
	#region Rule classes for ReferenceMode
	partial class ReferenceMode
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(CustomReferenceMode), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class CustomReferenceModeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public CustomReferenceModeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ReferenceMode
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ReferenceMode.CustomReferenceModeChangeRule");
				ReferenceMode.CustomReferenceModeChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ReferenceMode.CustomReferenceModeChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ModelHasReferenceMode), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class ReferenceModeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReferenceModeAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ReferenceMode
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ReferenceMode.ReferenceModeAddedRule");
				ReferenceMode.ReferenceModeAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ReferenceMode.ReferenceModeAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReferenceModeKind), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ReferenceModeKindChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReferenceModeKindChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ReferenceMode
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ReferenceMode.ReferenceModeKindChangeRule");
				ReferenceMode.ReferenceModeKindChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ReferenceMode.ReferenceModeKindChangeRule");
			}
		}
	}
	#endregion // Rule classes for ReferenceMode
	#region Rule classes for ReferenceModeHasReferenceModeKind
	partial class ReferenceModeHasReferenceModeKind
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReferenceModeHasReferenceModeKind), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ReferenceModeHasReferenceModeKindDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReferenceModeHasReferenceModeKindDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ReferenceModeHasReferenceModeKind
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ReferenceModeHasReferenceModeKind.ReferenceModeHasReferenceModeKindDeletingRule");
				ReferenceModeHasReferenceModeKind.ReferenceModeHasReferenceModeKindDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ReferenceModeHasReferenceModeKind.ReferenceModeHasReferenceModeKindDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReferenceModeHasReferenceModeKind), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ReferenceModeHasReferenceModeKindRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReferenceModeHasReferenceModeKindRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ReferenceModeHasReferenceModeKind
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ReferenceModeHasReferenceModeKind.ReferenceModeHasReferenceModeKindRolePlayerChangeRule");
				ReferenceModeHasReferenceModeKind.ReferenceModeHasReferenceModeKindRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ReferenceModeHasReferenceModeKind.ReferenceModeHasReferenceModeKindRolePlayerChangeRule");
			}
		}
	}
	#endregion // Rule classes for ReferenceModeHasReferenceModeKind
	#region Rule classes for RingConstraint
	partial class RingConstraint
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RingConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RingConstraintTypeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RingConstraintTypeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.RingConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RingConstraint.RingConstraintTypeChangeRule");
				RingConstraint.RingConstraintTypeChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RingConstraint.RingConstraintTypeChangeRule");
			}
		}
	}
	#endregion // Rule classes for RingConstraint
	#region Rule classes for Role
	partial class Role
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRoleSequenceHasRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleSequenceHasRoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Role
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.ConstraintRoleSequenceHasRoleAddedRule");
				Role.ConstraintRoleSequenceHasRoleAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.ConstraintRoleSequenceHasRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintRoleSequenceHasRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleSequenceHasRoleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Role
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.ConstraintRoleSequenceHasRoleDeletedRule");
				Role.ConstraintRoleSequenceHasRoleDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.ConstraintRoleSequenceHasRoleDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasDerivationRule), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeDerivationRuleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeDerivationRuleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Role
			/// /// <summary>
			/// /// DeleteRule: typeof(FactTypeHasDerivationRule)
			/// /// </summary>
			/// private static void FactTypeDerivationRuleDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.FactTypeDerivationRuleDeletedRule");
				Role.FactTypeDerivationRuleDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.FactTypeDerivationRuleDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Role), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Role
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.RoleChangeRule");
				Role.RoleChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.RoleChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RoleInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleInstanceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleInstanceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Role
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.RoleInstanceAddedRule");
				Role.RoleInstanceAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.RoleInstanceAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RoleInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleInstanceDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleInstanceDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Role
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.RoleInstanceDeletedRule");
				Role.RoleInstanceDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.RoleInstanceDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RoleDerivesFromCalculatedPathValue), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleDerivesFromCalculatedValueRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleDerivesFromCalculatedValueRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Role
			/// /// <summary>
			/// /// AddRule: typeof(RoleDerivesFromCalculatedPathValue)
			/// /// </summary>
			/// private static void RoleDerivesFromCalculatedValueRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.RoleDerivesFromCalculatedValueRule");
				Role.RoleDerivesFromCalculatedValueRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.RoleDerivesFromCalculatedValueRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RoleDerivesFromPathConstant), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleDerivesFromConstantRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleDerivesFromConstantRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Role
			/// /// <summary>
			/// /// AddRule: typeof(RoleDerivesFromPathConstant)
			/// /// </summary>
			/// private static void RoleDerivesFromConstantRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.RoleDerivesFromConstantRule");
				Role.RoleDerivesFromConstantRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.RoleDerivesFromConstantRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RoleDerivesFromPathedRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleDerivedFromPathedRoleRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleDerivedFromPathedRoleRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Role
			/// /// <summary>
			/// /// AddRule: typeof(RoleDerivesFromPathedRole)
			/// /// </summary>
			/// private static void RoleDerivedFromPathedRoleRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.RoleDerivedFromPathedRoleRule");
				Role.RoleDerivedFromPathedRoleRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.RoleDerivedFromPathedRoleRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RoleInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleInstanceRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleInstanceRolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Role
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.RoleInstanceRolePlayerChangedRule");
				Role.RoleInstanceRolePlayerChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.RoleInstanceRolePlayerChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class RolePlayerRequiredAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerRequiredAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Role
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.RolePlayerRequiredAddRule");
				Role.RolePlayerRequiredAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.RolePlayerRequiredAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerRequiredDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerRequiredDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Role
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.RolePlayerRequiredDeleteRule");
				Role.RolePlayerRequiredDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.RolePlayerRequiredDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerRequiredForNewRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerRequiredForNewRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Role
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.RolePlayerRequiredForNewRoleAddRule");
				Role.RolePlayerRequiredForNewRoleAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.RolePlayerRequiredForNewRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class UpdatedRolePlayerRequiredErrorsDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public UpdatedRolePlayerRequiredErrorsDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Role
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.UpdatedRolePlayerRequiredErrorsDeleteRule");
				Role.UpdatedRolePlayerRequiredErrorsDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.UpdatedRolePlayerRequiredErrorsDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(UniquenessConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class UniquenessConstraintChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public UniquenessConstraintChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.Role
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.UniquenessConstraintChangedRule");
				Role.UniquenessConstraintChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.Role.UniquenessConstraintChangedRule");
			}
		}
	}
	#endregion // Rule classes for Role
	#region Rule classes for RolePath
	partial class RolePath
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(PathedRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class PathedRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PathedRoleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.RolePath
			/// /// <summary>
			/// /// DeleteRule: typeof(PathedRole), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void PathedRoleDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RolePath.PathedRoleDeletedRule");
				RolePath.PathedRoleDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RolePath.PathedRoleDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(PathedRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class PathedRoleRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PathedRoleRolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.RolePath
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(PathedRole), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void PathedRoleRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RolePath.PathedRoleRolePlayerChangedRule");
				RolePath.PathedRoleRolePlayerChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RolePath.PathedRoleRolePlayerChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RoleSubPathIsContinuationOfRolePath), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class SubPathDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SubPathDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.RolePath
			/// /// <summary>
			/// /// DeleteRule: typeof(RoleSubPathIsContinuationOfRolePath), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void SubPathDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RolePath.SubPathDeletedRule");
				RolePath.SubPathDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RolePath.SubPathDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RoleSubPathIsContinuationOfRolePath), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class SubPathRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SubPathRolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.RolePath
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(RoleSubPathIsContinuationOfRolePath), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void SubPathRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RolePath.SubPathRolePlayerChangedRule");
				RolePath.SubPathRolePlayerChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RolePath.SubPathRolePlayerChangedRule");
			}
		}
	}
	#endregion // Rule classes for RolePath
	#region Rule classes for RolePathComponent
	partial class RolePathComponent
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RolePathCompositorHasPathComponent), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePathCompositorHasPathComponentAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePathCompositorHasPathComponentAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.RolePathComponent
			/// /// <summary>
			/// /// AddRule: typeof(RolePathCompositorHasPathComponent)
			/// /// </summary>
			/// private static void RolePathCompositorHasPathComponentAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RolePathComponent.RolePathCompositorHasPathComponentAddedRule");
				RolePathComponent.RolePathCompositorHasPathComponentAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RolePathComponent.RolePathCompositorHasPathComponentAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RolePathCompositorHasPathComponent), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePathCompositorHasPathComponentDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePathCompositorHasPathComponentDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.RolePathComponent
			/// /// <summary>
			/// /// DeleteRule: typeof(RolePathCompositorHasPathComponent)
			/// /// </summary>
			/// private static void RolePathCompositorHasPathComponentDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RolePathComponent.RolePathCompositorHasPathComponentDeletedRule");
				RolePathComponent.RolePathCompositorHasPathComponentDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RolePathComponent.RolePathCompositorHasPathComponentDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RolePathCompositorHasPathComponent), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePathCompositorHasPathComponentRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePathCompositorHasPathComponentRolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.RolePathComponent
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(RolePathCompositorHasPathComponent)
			/// /// </summary>
			/// private static void RolePathCompositorHasPathComponentRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RolePathComponent.RolePathCompositorHasPathComponentRolePlayerChangedRule");
				RolePathComponent.RolePathCompositorHasPathComponentRolePlayerChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RolePathComponent.RolePathCompositorHasPathComponentRolePlayerChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RolePathOwnerHasPathComponent), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePathOwnerHasPathComponentAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePathOwnerHasPathComponentAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.RolePathComponent
			/// /// <summary>
			/// /// AddRule: typeof(RolePathOwnerHasPathComponent)
			/// /// </summary>
			/// private static void RolePathOwnerHasPathComponentAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RolePathComponent.RolePathOwnerHasPathComponentAddedRule");
				RolePathComponent.RolePathOwnerHasPathComponentAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RolePathComponent.RolePathOwnerHasPathComponentAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RolePathOwnerHasPathComponent), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePathOwnerHasPathComponentDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePathOwnerHasPathComponentDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.RolePathComponent
			/// /// <summary>
			/// /// DeleteRule: typeof(RolePathOwnerHasPathComponent)
			/// /// </summary>
			/// private static void RolePathOwnerHasPathComponentDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RolePathComponent.RolePathOwnerHasPathComponentDeletedRule");
				RolePathComponent.RolePathOwnerHasPathComponentDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RolePathComponent.RolePathOwnerHasPathComponentDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RolePathOwnerHasPathComponent), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePathOwnerHasPathComponentRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePathOwnerHasPathComponentRolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.RolePathComponent
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(RolePathOwnerHasPathComponent)
			/// /// </summary>
			/// private static void RolePathOwnerHasPathComponentRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RolePathComponent.RolePathOwnerHasPathComponentRolePlayerChangedRule");
				RolePathComponent.RolePathOwnerHasPathComponentRolePlayerChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.RolePathComponent.RolePathOwnerHasPathComponentRolePlayerChangedRule");
			}
		}
	}
	#endregion // Rule classes for RolePathComponent
	#region Rule classes for SetComparisonConstraint
	partial class SetComparisonConstraint
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetComparisonConstraintHasRoleSequence), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
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
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.ConstraintHasRoleSequenceAddedRule");
				SetComparisonConstraint.ConstraintHasRoleSequenceAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.ConstraintHasRoleSequenceAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
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
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.ConstraintRoleSequenceHasRoleAddedRule");
				SetComparisonConstraint.ConstraintRoleSequenceHasRoleAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.ConstraintRoleSequenceHasRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ModelHasSetComparisonConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceCardinalityForConstraintAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceCardinalityForConstraintAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceCardinalityForConstraintAddRule");
				SetComparisonConstraint.EnforceRoleSequenceCardinalityForConstraintAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceCardinalityForConstraintAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetComparisonConstraintHasRoleSequence), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceCardinalityForSequenceAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceCardinalityForSequenceAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceCardinalityForSequenceAddRule");
				SetComparisonConstraint.EnforceRoleSequenceCardinalityForSequenceAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceCardinalityForSequenceAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetComparisonConstraintHasRoleSequence), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceCardinalityForSequenceDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceCardinalityForSequenceDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceCardinalityForSequenceDeleteRule");
				SetComparisonConstraint.EnforceRoleSequenceCardinalityForSequenceDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceCardinalityForSequenceDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceValidityForRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceValidityForRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRoleAddRule");
				SetComparisonConstraint.EnforceRoleSequenceValidityForRoleAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceValidityForRoleDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceValidityForRoleDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRoleDeleteRule");
				SetComparisonConstraint.EnforceRoleSequenceValidityForRoleDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRoleDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceValidityForRoleReorderRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceValidityForRoleReorderRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRoleReorderRule");
				SetComparisonConstraint.EnforceRoleSequenceValidityForRoleReorderRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRoleReorderRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceValidityForRolePlayerAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceValidityForRolePlayerAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRolePlayerAddRule");
				SetComparisonConstraint.EnforceRoleSequenceValidityForRolePlayerAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRolePlayerAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceValidityForRolePlayerDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceValidityForRolePlayerDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRolePlayerDeleteRule");
				SetComparisonConstraint.EnforceRoleSequenceValidityForRolePlayerDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRolePlayerDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRule");
				SetComparisonConstraint.EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ExternalRoleConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class ExternalRoleConstraintDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ExternalRoleConstraintDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.ExternalRoleConstraintDeletedRule");
				SetComparisonConstraint.ExternalRoleConstraintDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.ExternalRoleConstraintDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ModelHasFactType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.FactTypeAddedRule");
				SetComparisonConstraint.FactTypeAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.FactTypeAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactSetComparisonConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactSetComparisonConstraintAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactSetComparisonConstraintAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.FactSetComparisonConstraintAddedRule");
				SetComparisonConstraint.FactSetComparisonConstraintAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.FactSetComparisonConstraintAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetComparisonConstraintHasRoleSequence), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class SetComparisonConstraintRoleSequenceDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SetComparisonConstraintRoleSequenceDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.SetComparisonConstraintRoleSequenceDeletedRule");
				SetComparisonConstraint.SetComparisonConstraintRoleSequenceDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetComparisonConstraint.SetComparisonConstraintRoleSequenceDeletedRule");
			}
		}
	}
	#endregion // Rule classes for SetComparisonConstraint
	#region Rule classes for SetConstraint
	partial class SetConstraint
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ModelHasSetConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConstraintAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.ConstraintAddedRule");
				SetConstraint.ConstraintAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.ConstraintAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
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
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.ConstraintRoleSequenceHasRoleAddedRule");
				SetConstraint.ConstraintRoleSequenceHasRoleAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.ConstraintRoleSequenceHasRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceValidityForRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceValidityForRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.EnforceRoleSequenceValidityForRoleAddRule");
				SetConstraint.EnforceRoleSequenceValidityForRoleAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.EnforceRoleSequenceValidityForRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceValidityForRoleDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceValidityForRoleDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.EnforceRoleSequenceValidityForRoleDeleteRule");
				SetConstraint.EnforceRoleSequenceValidityForRoleDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.EnforceRoleSequenceValidityForRoleDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceValidityForRolePlayerAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceValidityForRolePlayerAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.EnforceRoleSequenceValidityForRolePlayerAddRule");
				SetConstraint.EnforceRoleSequenceValidityForRolePlayerAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.EnforceRoleSequenceValidityForRolePlayerAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceValidityForRolePlayerDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceValidityForRolePlayerDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.EnforceRoleSequenceValidityForRolePlayerDeleteRule");
				SetConstraint.EnforceRoleSequenceValidityForRolePlayerDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.EnforceRoleSequenceValidityForRolePlayerDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRule");
				SetConstraint.EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.EnforceRoleSequenceValidityForRolePlayerRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactSetConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactSetConstraintAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactSetConstraintAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.FactSetConstraintAddedRule");
				SetConstraint.FactSetConstraintAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.FactSetConstraintAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactSetConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactSetConstraintDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactSetConstraintDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.FactSetConstraintDeletedRule");
				SetConstraint.FactSetConstraintDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.FactSetConstraintDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ModelHasFactType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.FactTypeAddedRule");
				SetConstraint.FactTypeAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.FactTypeAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ModalityChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ModalityChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.ModalityChangeRule");
				SetConstraint.ModalityChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.ModalityChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class SetConstraintRoleSequenceHasRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SetConstraintRoleSequenceHasRoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.SetConstraintRoleSequenceHasRoleAddedRule");
				SetConstraint.SetConstraintRoleSequenceHasRoleAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.SetConstraintRoleSequenceHasRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class SetConstraintRoleSequenceHasRoleDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SetConstraintRoleSequenceHasRoleDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.SetConstraintRoleSequenceHasRoleDeletingRule");
				SetConstraint.SetConstraintRoleSequenceHasRoleDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint.SetConstraintRoleSequenceHasRoleDeletingRule");
			}
		}
	}
	#endregion // Rule classes for SetConstraint
	#region Rule classes for SubtypeFact
	partial class SubtypeFact
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class DeleteSubtypeWhenRolePlayerDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DeleteSubtypeWhenRolePlayerDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.DeleteSubtypeWhenRolePlayerDeletedRule");
				SubtypeFact.DeleteSubtypeWhenRolePlayerDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.DeleteSubtypeWhenRolePlayerDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnsureConsistentDataTypesAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnsureConsistentDataTypesAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.EnsureConsistentDataTypesAddRule");
				SubtypeFact.EnsureConsistentDataTypesAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.EnsureConsistentDataTypesAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnsureConsistentDataTypesDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnsureConsistentDataTypesDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.EnsureConsistentDataTypesDeleteRule");
				SubtypeFact.EnsureConsistentDataTypesDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.EnsureConsistentDataTypesDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class EnsureConsistentRolePlayerTypesAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public EnsureConsistentRolePlayerTypesAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.EnsureConsistentRolePlayerTypesAddRule");
				SubtypeFact.EnsureConsistentRolePlayerTypesAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.EnsureConsistentRolePlayerTypesAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SubtypeFact), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class InitializeSubtypeAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public InitializeSubtypeAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.InitializeSubtypeAddRule");
				SubtypeFact.InitializeSubtypeAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.InitializeSubtypeAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class LimitSubtypeConstraintChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public LimitSubtypeConstraintChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.LimitSubtypeConstraintChangeRule");
				SubtypeFact.LimitSubtypeConstraintChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.LimitSubtypeConstraintChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class LimitSubtypeConstraintRolesAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public LimitSubtypeConstraintRolesAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.LimitSubtypeConstraintRolesAddRule");
				SubtypeFact.LimitSubtypeConstraintRolesAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.LimitSubtypeConstraintRolesAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class LimitSubtypeConstraintRolesDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public LimitSubtypeConstraintRolesDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.LimitSubtypeConstraintRolesDeleteRule");
				SubtypeFact.LimitSubtypeConstraintRolesDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.LimitSubtypeConstraintRolesDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactSetConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class LimitSubtypeConstraintsAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public LimitSubtypeConstraintsAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.LimitSubtypeConstraintsAddRule");
				SubtypeFact.LimitSubtypeConstraintsAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.LimitSubtypeConstraintsAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactSetConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class LimitSubtypeConstraintsDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public LimitSubtypeConstraintsDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.LimitSubtypeConstraintsDeleteRule");
				SubtypeFact.LimitSubtypeConstraintsDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.LimitSubtypeConstraintsDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class LimitSubtypeRolesAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public LimitSubtypeRolesAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.LimitSubtypeRolesAddRule");
				SubtypeFact.LimitSubtypeRolesAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.LimitSubtypeRolesAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class LimitSubtypeRolesDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public LimitSubtypeRolesDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.LimitSubtypeRolesDeleteRule");
				SubtypeFact.LimitSubtypeRolesDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.LimitSubtypeRolesDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SetComparisonConstraintHasRoleSequence), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class LimitSubtypeSetComparisonConstraintSequenceAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public LimitSubtypeSetComparisonConstraintSequenceAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.LimitSubtypeSetComparisonConstraintSequenceAddRule");
				SubtypeFact.LimitSubtypeSetComparisonConstraintSequenceAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact.LimitSubtypeSetComparisonConstraintSequenceAddRule");
			}
		}
	}
	#endregion // Rule classes for SubtypeFact
	#region Rule classes for UniquenessConstraint
	partial class UniquenessConstraint
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactSetConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class NMinusOneConstraintAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public NMinusOneConstraintAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.UniquenessConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.UniquenessConstraint.NMinusOneConstraintAddRule");
				UniquenessConstraint.NMinusOneConstraintAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.UniquenessConstraint.NMinusOneConstraintAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class NMinusOneConstraintRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public NMinusOneConstraintRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.UniquenessConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.UniquenessConstraint.NMinusOneConstraintRoleAddRule");
				UniquenessConstraint.NMinusOneConstraintRoleAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.UniquenessConstraint.NMinusOneConstraintRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class NMinusOneConstraintRoleDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public NMinusOneConstraintRoleDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.UniquenessConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.UniquenessConstraint.NMinusOneConstraintRoleDeleteRule");
				UniquenessConstraint.NMinusOneConstraintRoleDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.UniquenessConstraint.NMinusOneConstraintRoleDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class NMinusOneFactTypeRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public NMinusOneFactTypeRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.UniquenessConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.UniquenessConstraint.NMinusOneFactTypeRoleAddRule");
				UniquenessConstraint.NMinusOneFactTypeRoleAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.UniquenessConstraint.NMinusOneFactTypeRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class NMinusOneFactTypeRoleDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public NMinusOneFactTypeRoleDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.UniquenessConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.UniquenessConstraint.NMinusOneFactTypeRoleDeleteRule");
				UniquenessConstraint.NMinusOneFactTypeRoleDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.UniquenessConstraint.NMinusOneFactTypeRoleDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(UniquenessConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class UniquenessConstraintChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public UniquenessConstraintChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.UniquenessConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.UniquenessConstraint.UniquenessConstraintChangeRule");
				UniquenessConstraint.UniquenessConstraintChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.UniquenessConstraint.UniquenessConstraintChangeRule");
			}
		}
	}
	#endregion // Rule classes for UniquenessConstraint
	#region Rule classes for ValueConstraint
	partial class ValueConstraint
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DataTypeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DataTypeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.DataTypeChangeRule");
				ValueConstraint.DataTypeChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.DataTypeChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DataTypeDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DataTypeDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.DataTypeDeletingRule");
				ValueConstraint.DataTypeDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.DataTypeDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DataTypeRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DataTypeRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.DataTypeRolePlayerChangeRule");
				ValueConstraint.DataTypeRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.DataTypeRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ObjectTypeRoleAddedClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectTypeRoleAddedClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.ObjectTypeRoleAdded");
				ValueConstraint.ObjectTypeRoleAdded(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.ObjectTypeRoleAdded");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class PreferredIdentifierDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.PreferredIdentifierDeletingRule");
				ValueConstraint.PreferredIdentifierDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.PreferredIdentifierDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(EntityTypeHasPreferredIdentifier), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class PreferredIdentifierRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.PreferredIdentifierRolePlayerChangeRule");
				ValueConstraint.PreferredIdentifierRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.PreferredIdentifierRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConstraintRoleSequenceHasRole), Priority=(1 + ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority))]
		private sealed class PreferredIdentifierRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.PreferredIdentifierRoleAddRule");
				ValueConstraint.PreferredIdentifierRoleAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.PreferredIdentifierRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.RolePlayerDeletingRule");
				ValueConstraint.RolePlayerDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.RolePlayerDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.RolePlayerRolePlayerChangeRule");
				ValueConstraint.RolePlayerRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.RolePlayerRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RoleHasValueConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleValueConstraintAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleValueConstraintAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.RoleValueConstraintAddedRule");
				ValueConstraint.RoleValueConstraintAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.RoleValueConstraintAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasValueConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValueConstraintAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueConstraintAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.ValueConstraintAddRule");
				ValueConstraint.ValueConstraintAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.ValueConstraintAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValueConstraintChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueConstraintChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.ValueConstraintChangeRule");
				ValueConstraint.ValueConstraintChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.ValueConstraintChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueConstraintHasValueRange), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValueRangeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueRangeAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.ValueRangeAddedRule");
				ValueConstraint.ValueRangeAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.ValueRangeAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueRange), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValueRangeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueRangeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.ValueRangeChangeRule");
				ValueConstraint.ValueRangeChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint.ValueRangeChangeRule");
			}
		}
	}
	#endregion // Rule classes for ValueConstraint
	#region Rule classes for ValueRange
	partial class ValueRange
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueRange), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValueRangeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueRangeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ValueRange
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueRange.ValueRangeChangeRule");
				ValueRange.ValueRangeChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueRange.ValueRangeChangeRule");
			}
		}
	}
	#endregion // Rule classes for ValueRange
	#region Rule classes for ValueTypeInstance
	partial class ValueTypeInstance
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValueTypeHasDataTypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueTypeHasDataTypeAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeInstance
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeInstance.ValueTypeHasDataTypeAddedRule");
				ValueTypeInstance.ValueTypeHasDataTypeAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeInstance.ValueTypeHasDataTypeAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValueTypeHasDataTypeDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueTypeHasDataTypeDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeInstance
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeInstance.ValueTypeHasDataTypeDeletedRule");
				ValueTypeInstance.ValueTypeHasDataTypeDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeInstance.ValueTypeHasDataTypeDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasDataType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValueTypeHasDataTypeRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueTypeHasDataTypeRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeInstance
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeInstance.ValueTypeHasDataTypeRolePlayerChangeRule");
				ValueTypeInstance.ValueTypeHasDataTypeRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeInstance.ValueTypeHasDataTypeRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValueTypeInstanceValueChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueTypeInstanceValueChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeInstance
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeInstance.ValueTypeInstanceValueChangedRule");
				ValueTypeInstance.ValueTypeInstanceValueChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeInstance.ValueTypeInstanceValueChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueTypeHasValueTypeInstance), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ValueTypeHasValueTypeInstanceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueTypeHasValueTypeInstanceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeInstance
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeInstance.ValueTypeHasValueTypeInstanceAddedRule");
				ValueTypeInstance.ValueTypeHasValueTypeInstanceAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeInstance.ValueTypeHasValueTypeInstanceAddedRule");
			}
		}
	}
	#endregion // Rule classes for ValueTypeInstance
	#endregion // Auto-rule classes
}
namespace ORMSolutions.ORMArchitect.Framework
{
	#region Auto-rule classes
	#region Rule classes for NamedElementDictionary
	partial class NamedElementDictionary
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Microsoft.VisualStudio.Modeling.ElementLink), Priority=(NamedElementDictionary.RulePriority + ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority))]
		private sealed class ElementLinkAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ElementLinkAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Framework.NamedElementDictionary
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.NamedElementDictionary.ElementLinkAddedRule");
				NamedElementDictionary.ElementLinkAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.NamedElementDictionary.ElementLinkAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Microsoft.VisualStudio.Modeling.ElementLink), Priority=(NamedElementDictionary.RulePriority + ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority))]
		private sealed class ElementLinkDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ElementLinkDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Framework.NamedElementDictionary
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.NamedElementDictionary.ElementLinkDeletingRule");
				NamedElementDictionary.ElementLinkDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.NamedElementDictionary.ElementLinkDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Microsoft.VisualStudio.Modeling.ModelElement), Priority=(NamedElementDictionary.RulePriority + ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority))]
		private sealed class NamedElementChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public NamedElementChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Framework.NamedElementDictionary
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.NamedElementDictionary.NamedElementChangedRule");
				NamedElementDictionary.NamedElementChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ObjectModel.NamedElementDictionary.NamedElementChangedRule");
			}
		}
	}
	#endregion // Rule classes for NamedElementDictionary
	#endregion // Auto-rule classes
}
