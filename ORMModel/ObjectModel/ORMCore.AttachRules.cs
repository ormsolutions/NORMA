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
	#region Attach rules to ORMMetaModel model
	public partial class ORMMetaModel
	{
		/// <summary>
		/// Generated code to attach rules to the store.
		/// </summary>
		protected override Type[] AllMetaModelTypes()
		{
			if (!(Neumont.Tools.ORM.ObjectModel.ORMMetaModel.InitializingToolboxItems))
			{
				return Type.EmptyTypes;
			}
			Type[] retVal = new Type[]{
				typeof(ConstraintUtility).GetNestedType("ConstraintRoleSequenceHasRoleRemoved", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(CustomReferenceMode).GetNestedType("CustomReferenceModeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(DataTypeNotSpecifiedError).GetNestedType("UnspecifiedTypeAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(DisjunctiveMandatoryConstraint).GetNestedType("VerifyImpliedDisjunctiveMandatoryRoleAdd", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(DisjunctiveMandatoryConstraint).GetNestedType("VerifyImpliedDisjunctiveMandatoryRoleRemoved", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(EntityTypeHasPreferredIdentifier).GetNestedType("PreferredIdentifierAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(EntityTypeHasPreferredIdentifier).GetNestedType("TestRemovePreferredIdentifierAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(EntityTypeHasPreferredIdentifier).GetNestedType("TestRemovePreferredIdentifierRemovingRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(EqualityConstraint).GetNestedType("ConstraintRoleSequenceHasRoleAdded", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(EqualityConstraint).GetNestedType("ConstraintRoleSequenceHasRoleRemoved", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ExternalUniquenessConstraint).GetNestedType("ExternalUniquenessConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactType).GetNestedType("FactTypeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactType).GetNestedType("FactTypeHasReadingOrderAddRuleModelValidation", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactType).GetNestedType("FactTypeHasReadingOrderRemovedRuleModelValidation", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactType).GetNestedType("FactTypeHasRoleAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactType).GetNestedType("FactTypeHasRoleRemoveRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactType).GetNestedType("InternalConstraintCollectionHasConstraintAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactType).GetNestedType("InternalConstraintCollectionHasConstraintRemovedRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactType).GetNestedType("InternalUniquenessConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactType).GetNestedType("ModelHasFactTypeAddRuleModelValidation", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactType).GetNestedType("ModelHasInternalConstraintAddRuleModelValidation", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactType).GetNestedType("ModelHasInternalConstraintRemoveRuleModelValidation", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactType).GetNestedType("ReadingOrderHasReadingAddRuleModelValidation", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactType).GetNestedType("ReadingOrderHasReadingRemoveRuleModelValidation", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FrequencyConstraint).GetNestedType("FrequencyConstraintMinMaxRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FrequencyConstraint).GetNestedType("RemoveContradictionErrorsWithFactTypeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(InternalConstraint).GetNestedType("ConstraintRoleSequenceHasRoleAdded", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(InternalConstraint).GetNestedType("FactTypeHasInternalConstraintAdded", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(InternalUniquenessConstraint).GetNestedType("InternalUniquenessConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(InternalUniquenessConstraint).GetNestedType("NMinusOneAddRuleModelConstraintAddValidation", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(InternalUniquenessConstraint).GetNestedType("NMinusOneAddRuleModelFactAddValidation", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(InternalUniquenessConstraint).GetNestedType("NMinusOneAddRuleModelValidation", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(InternalUniquenessConstraint).GetNestedType("NMinusOneRemoveRuleModelConstraintRemoveValidation", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(InternalUniquenessConstraint).GetNestedType("NMinusOneRemoveRuleModelFactRemoveValidation", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ModelError).GetNestedType("SynchronizeErrorForOwnerRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ModelError).GetNestedType("SynchronizeErrorTextForModelRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(MultiColumnExternalConstraint).GetNestedType("ConstraintHasRoleSequenceAdded", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(MultiColumnExternalConstraint).GetNestedType("ConstraintRoleSequenceHasRoleAdded", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(MultiColumnExternalConstraint).GetNestedType("EnforceRoleSequenceCardinalityForAdd", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(MultiColumnExternalConstraint).GetNestedType("EnforceRoleSequenceCardinalityForConstraintAdd", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(MultiColumnExternalConstraint).GetNestedType("EnforceRoleSequenceCardinalityForRemove", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(MultiColumnExternalConstraint).GetNestedType("EnforceRoleSequenceValidityForAdd", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(MultiColumnExternalConstraint).GetNestedType("EnforceRoleSequenceValidityForFactTypeAdd", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(MultiColumnExternalConstraint).GetNestedType("EnforceRoleSequenceValidityForFactTypeRemove", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(MultiColumnExternalConstraint).GetNestedType("EnforceRoleSequenceValidityForRemove", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(MultiColumnExternalConstraint).GetNestedType("EnforceRoleSequenceValidityForReorder", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(MultiColumnExternalConstraint).GetNestedType("ExternalRoleConstraintRemoved", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(Neumont.Tools.ORM.Framework.NamedElementDictionary).GetNestedType("ElementLinkAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(Neumont.Tools.ORM.Framework.NamedElementDictionary).GetNestedType("ElementLinkRemovedRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(Neumont.Tools.ORM.Framework.NamedElementDictionary).GetNestedType("NamedElementChangedRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(Objectification).GetNestedType("EqualityConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(Objectification).GetNestedType("ExternalUniquenessConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(Objectification).GetNestedType("ImpliedFactTypeAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(Objectification).GetNestedType("InternalConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(Objectification).GetNestedType("ObjectificationAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(Objectification).GetNestedType("RoleAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(Objectification).GetNestedType("RolePlayerAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(Objectification).GetNestedType("RoleRemovingRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(Objectification).GetNestedType("RolePlayerRemovingRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(Objectification).GetNestedType("UpdateImplicitUniquenessAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(Objectification).GetNestedType("UpdateImplicitUniquenessRemoveRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectType).GetNestedType("CheckForIncompatibleRelationshipRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectType).GetNestedType("MandatoryRoleAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectType).GetNestedType("MandatoryRoleRemovingRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectType).GetNestedType("ModelHasObjectTypeAddRuleModelValidation", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectType).GetNestedType("ObjectTypeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectType).GetNestedType("ObjectTypeRemoveRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectType).GetNestedType("SubtypeFactChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectType).GetNestedType("SupertypeAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectType).GetNestedType("SupertypeRemoveRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectType).GetNestedType("SupertypeRemovingRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectType).GetNestedType("VerifyObjectificationAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectType).GetNestedType("VerifyObjectificationRemoveRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectType).GetNestedType("VerifyReferenceSchemeAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectType).GetNestedType("VerifyReferenceSchemeRemoveRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectType).GetNestedType("VerifyValueTypeHasDataTypeAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectType).GetNestedType("VerifyValueTypeHasDataTypeRemoveRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(DelayValidateElements),
				typeof(ORMModel).GetNestedType("RemoveDuplicateConstraintNameErrorRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ORMModel).GetNestedType("RemoveDuplicateFactTypeNameErrorRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ORMModel).GetNestedType("RemoveDuplicateObjectTypeNameErrorRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(Reading).GetNestedType("ReadingOrderHasRoleRemoved", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(Reading).GetNestedType("ReadingPropertiesChanged", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ReadingOrder).GetNestedType("EnforceNoEmptyReadingOrder", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ReadingOrder).GetNestedType("FactTypeHasRoleAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ReadingOrder).GetNestedType("ReadingOrderHasReadingAdded", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ReadingOrder).GetNestedType("ReadingOrderHasReadingRemoved", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ReadingOrder).GetNestedType("ReadingOrderHasRoleRemoving", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ReferenceMode).GetNestedType("ReferenceModeAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ReferenceMode).GetNestedType("ReferenceModeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ReferenceModeHasReferenceModeKind).GetNestedType("ReferenceModeHasReferenceModeKindChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ReferenceModeHasReferenceModeKind).GetNestedType("ReferenceModeHasReferenceModeKindRemovingRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ReferenceModeKind).GetNestedType("ReferenceModeKindChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(RingConstraint).GetNestedType("RingConstraintTypeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(Role).GetNestedType("RoleChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(Role).GetNestedType("RolePlayerRequiredAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(Role).GetNestedType("RolePlayerRequiredForNewRoleAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(Role).GetNestedType("RolePlayerRequiredRemovedRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(Role).GetNestedType("UpdatedRolePlayerRequiredErrorsRemovedRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(RoleValueConstraint).GetNestedType("RoleValueConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(RootType).GetNestedType("RootTypeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(Note).GetNestedType("NoteChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(SingleColumnExternalConstraint).GetNestedType("ConstraintAdded", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(SingleColumnExternalConstraint).GetNestedType("ConstraintRoleSequenceHasRoleAdded", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(SingleColumnExternalConstraint).GetNestedType("EnforceRoleSequenceValidityForAdd", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(SingleColumnExternalConstraint).GetNestedType("EnforceRoleSequenceValidityForFactTypeAdd", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(SingleColumnExternalConstraint).GetNestedType("EnforceRoleSequenceValidityForFactTypeRemove", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(SingleColumnExternalConstraint).GetNestedType("EnforceRoleSequenceValidityForRemove", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(SubtypeFact).GetNestedType("BlockCircularSubtypesAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(SubtypeFact).GetNestedType("EnsureConsistentDataTypesAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(SubtypeFact).GetNestedType("EnsureConsistentDataTypesRemoveRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(SubtypeFact).GetNestedType("EnsureConsistentRolePlayerTypesAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(SubtypeFact).GetNestedType("InitializeSubtypeAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(SubtypeFact).GetNestedType("LimitSubtypeConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(SubtypeFact).GetNestedType("LimitSubtypeConstraintRolesAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(SubtypeFact).GetNestedType("LimitSubtypeConstraintRolesRemoveRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(SubtypeFact).GetNestedType("LimitSubtypeConstraintsAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(SubtypeFact).GetNestedType("LimitSubtypeConstraintsRemoveRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(SubtypeFact).GetNestedType("LimitSubtypeRolesAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(SubtypeFact).GetNestedType("LimitSubtypeRolesRemoveRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(SubtypeFact).GetNestedType("RemoveSubtypeWhenRolePlayerRemoved", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ValueRange).GetNestedType("DataTypeAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ValueRange).GetNestedType("DataTypeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ValueRange).GetNestedType("ObjectTypeRoleAdded", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ValueRange).GetNestedType("RoleValueConstraintAdded", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ValueRange).GetNestedType("ValueRangeAdded", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ValueRange).GetNestedType("ValueRangeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ValueRange).GetNestedType("ValueConstraintAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ValueTypeValueConstraint).GetNestedType("ValueTypeValueConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic)};
			System.Diagnostics.Debug.Assert(!(((System.Collections.IList)retVal).Contains(null)), "One or more rule types failed to resolve. The file and/or package will fail to load.");
			return retVal;
		}
	}
	#endregion // Attach rules to ORMMetaModel model
}
