using System;
using Microsoft.VisualStudio.Modeling;
using ORMSolutions.ORMArchitect.Framework;

// Common Public License Copyright Notice
// /**************************************************************************\
// * Natural Object-Role Modeling Architect for Visual Studio                 *
// *                                                                          *
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
	#region Customize Copy Closure for ORMCoreDomainModel
	partial class ORMCoreDomainModel : ICopyClosureProvider
	{
		private void AddCopyClosureDirectives(ICopyClosureManager closureManager)
		{
			#region Automatic top-level embedding relationships
			closureManager.AddRootEmbeddingRelationship(ModelHasObjectType.DomainClassId);
			closureManager.AddRootEmbeddingRelationship(ModelHasFactType.DomainClassId);
			closureManager.AddRootEmbeddingRelationship(ModelHasError.DomainClassId);
			closureManager.AddRootEmbeddingRelationship(ModelHasReferenceModeKind.DomainClassId);
			closureManager.AddRootEmbeddingRelationship(ModelHasReferenceMode.DomainClassId);
			closureManager.AddRootEmbeddingRelationship(ModelHasSetConstraint.DomainClassId);
			closureManager.AddRootEmbeddingRelationship(ModelHasSetComparisonConstraint.DomainClassId);
			closureManager.AddRootEmbeddingRelationship(ModelHasDataType.DomainClassId);
			closureManager.AddRootEmbeddingRelationship(GenerationStateHasGenerationSetting.DomainClassId);
			closureManager.AddRootEmbeddingRelationship(ModelContainsRecognizedPhrase.DomainClassId);
			closureManager.AddRootEmbeddingRelationship(ModelHasDefinition.DomainClassId);
			closureManager.AddRootEmbeddingRelationship(ModelHasPrimaryNote.DomainClassId);
			closureManager.AddRootEmbeddingRelationship(ModelHasModelNote.DomainClassId);
			closureManager.AddRootEmbeddingRelationship(ModelHasModelErrorDisplayFilter.DomainClassId);
			closureManager.AddRootEmbeddingRelationship(ElementGroupingSetRelatesToORMModel.DomainClassId);
			closureManager.AddRootEmbeddingRelationship(ElementGroupingSetContainsElementGrouping.DomainClassId);
			closureManager.AddRootEmbeddingRelationship(ModelDefinesFunction.DomainClassId);
			#endregion // Automatic top-level embedding relationships
			#region Closures for standard embedding relationships
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(CalculatedPathValueHasInput.InputDomainRoleId), new DomainRoleClosureRestriction(CalculatedPathValueHasInput.CalculatedValueDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(CalculatedPathValueHasInput.CalculatedValueDomainRoleId), new DomainRoleClosureRestriction(CalculatedPathValueHasInput.InputDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(CalculatedPathValueInputBindsToPathConstant.SourceDomainRoleId), new DomainRoleClosureRestriction(CalculatedPathValueInputBindsToPathConstant.InputDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(CalculatedPathValueInputBindsToPathConstant.InputDomainRoleId), new DomainRoleClosureRestriction(CalculatedPathValueInputBindsToPathConstant.SourceDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(CardinalityConstraintHasDefinition.DefinitionDomainRoleId), new DomainRoleClosureRestriction(CardinalityConstraintHasDefinition.CardinalityConstraintDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(CardinalityConstraintHasDefinition.CardinalityConstraintDomainRoleId), new DomainRoleClosureRestriction(CardinalityConstraintHasDefinition.DefinitionDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(CardinalityConstraintHasNote.NoteDomainRoleId), new DomainRoleClosureRestriction(CardinalityConstraintHasNote.CardinalityConstraintDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(CardinalityConstraintHasNote.CardinalityConstraintDomainRoleId), new DomainRoleClosureRestriction(CardinalityConstraintHasNote.NoteDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(CardinalityConstraintHasRange.RangeDomainRoleId), new DomainRoleClosureRestriction(CardinalityConstraintHasRange.ConstraintDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(CardinalityConstraintHasRange.ConstraintDomainRoleId), new DomainRoleClosureRestriction(CardinalityConstraintHasRange.RangeDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ConstraintRoleProjectedFromPathConstant.SourceDomainRoleId), new DomainRoleClosureRestriction(ConstraintRoleProjectedFromPathConstant.ConstraintRoleProjectionDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ConstraintRoleProjectedFromPathConstant.ConstraintRoleProjectionDomainRoleId), new DomainRoleClosureRestriction(ConstraintRoleProjectedFromPathConstant.SourceDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ConstraintRoleSequenceHasJoinPath.JoinPathDomainRoleId), new DomainRoleClosureRestriction(ConstraintRoleSequenceHasJoinPath.RoleSequenceDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ConstraintRoleSequenceHasJoinPath.RoleSequenceDomainRoleId), new DomainRoleClosureRestriction(ConstraintRoleSequenceHasJoinPath.JoinPathDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(DerivedRoleProjectedFromPathConstant.SourceDomainRoleId), new DomainRoleClosureRestriction(DerivedRoleProjectedFromPathConstant.RoleProjectionDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(DerivedRoleProjectedFromPathConstant.RoleProjectionDomainRoleId), new DomainRoleClosureRestriction(DerivedRoleProjectedFromPathConstant.SourceDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ElementGroupingHasDefinition.DefinitionDomainRoleId), new DomainRoleClosureRestriction(ElementGroupingHasDefinition.GroupingDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ElementGroupingHasDefinition.GroupingDomainRoleId), new DomainRoleClosureRestriction(ElementGroupingHasDefinition.DefinitionDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ElementGroupingHasNote.NoteDomainRoleId), new DomainRoleClosureRestriction(ElementGroupingHasNote.GroupingDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ElementGroupingHasNote.GroupingDomainRoleId), new DomainRoleClosureRestriction(ElementGroupingHasNote.NoteDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ElementGroupingIsOfElementGroupingType.GroupingTypeDomainRoleId), new DomainRoleClosureRestriction(ElementGroupingIsOfElementGroupingType.GroupingDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ElementGroupingIsOfElementGroupingType.GroupingDomainRoleId), new DomainRoleClosureRestriction(ElementGroupingIsOfElementGroupingType.GroupingTypeDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(EntityTypeHasEntityTypeInstance.EntityTypeInstanceDomainRoleId), new DomainRoleClosureRestriction(EntityTypeHasEntityTypeInstance.EntityTypeDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(EntityTypeHasEntityTypeInstance.EntityTypeDomainRoleId), new DomainRoleClosureRestriction(EntityTypeHasEntityTypeInstance.EntityTypeInstanceDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(EntityTypeSubtypeHasEntityTypeSubtypeInstance.EntityTypeSubtypeInstanceDomainRoleId), new DomainRoleClosureRestriction(EntityTypeSubtypeHasEntityTypeSubtypeInstance.EntityTypeSubtypeDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(EntityTypeSubtypeHasEntityTypeSubtypeInstance.EntityTypeSubtypeDomainRoleId), new DomainRoleClosureRestriction(EntityTypeSubtypeHasEntityTypeSubtypeInstance.EntityTypeSubtypeInstanceDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(FactTypeDerivationRuleHasDerivationNote.DerivationNoteDomainRoleId), new DomainRoleClosureRestriction(FactTypeDerivationRuleHasDerivationNote.DerivationRuleDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(FactTypeDerivationRuleHasDerivationNote.DerivationRuleDomainRoleId), new DomainRoleClosureRestriction(FactTypeDerivationRuleHasDerivationNote.DerivationNoteDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(FactTypeHasDefinition.DefinitionDomainRoleId), new DomainRoleClosureRestriction(FactTypeHasDefinition.FactTypeDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(FactTypeHasDefinition.FactTypeDomainRoleId), new DomainRoleClosureRestriction(FactTypeHasDefinition.DefinitionDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(FactTypeHasDerivationRule.DerivationRuleDomainRoleId), new DomainRoleClosureRestriction(FactTypeHasDerivationRule.FactTypeDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(FactTypeHasDerivationRule.FactTypeDomainRoleId), new DomainRoleClosureRestriction(FactTypeHasDerivationRule.DerivationRuleDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(FactTypeHasFactTypeInstance.FactTypeInstanceDomainRoleId), new DomainRoleClosureRestriction(FactTypeHasFactTypeInstance.FactTypeDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(FactTypeHasFactTypeInstance.FactTypeDomainRoleId), new DomainRoleClosureRestriction(FactTypeHasFactTypeInstance.FactTypeInstanceDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(FactTypeHasNote.NoteDomainRoleId), new DomainRoleClosureRestriction(FactTypeHasNote.FactTypeDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(FactTypeHasNote.FactTypeDomainRoleId), new DomainRoleClosureRestriction(FactTypeHasNote.NoteDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(FactTypeHasReadingOrder.ReadingOrderDomainRoleId), new DomainRoleClosureRestriction(FactTypeHasReadingOrder.FactTypeDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(FactTypeHasReadingOrder.FactTypeDomainRoleId), new DomainRoleClosureRestriction(FactTypeHasReadingOrder.ReadingOrderDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(FactTypeHasRole.RoleDomainRoleId), new DomainRoleClosureRestriction(FactTypeHasRole.FactTypeDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(FactTypeHasRole.FactTypeDomainRoleId), new DomainRoleClosureRestriction(FactTypeHasRole.RoleDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(FunctionOperatesOnParameter.ParameterDomainRoleId), new DomainRoleClosureRestriction(FunctionOperatesOnParameter.FunctionDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(FunctionOperatesOnParameter.FunctionDomainRoleId), new DomainRoleClosureRestriction(FunctionOperatesOnParameter.ParameterDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(LeadRolePathCalculatesCalculatedPathValue.CalculatedValueDomainRoleId), new DomainRoleClosureRestriction(LeadRolePathCalculatesCalculatedPathValue.LeadRolePathDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(LeadRolePathCalculatesCalculatedPathValue.LeadRolePathDomainRoleId), new DomainRoleClosureRestriction(LeadRolePathCalculatesCalculatedPathValue.CalculatedValueDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(LeadRolePathHasNote.NoteDomainRoleId), new DomainRoleClosureRestriction(LeadRolePathHasNote.LeadRolePathDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(LeadRolePathHasNote.LeadRolePathDomainRoleId), new DomainRoleClosureRestriction(LeadRolePathHasNote.NoteDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(LeadRolePathHasObjectUnifier.ObjectUnifierDomainRoleId), new DomainRoleClosureRestriction(LeadRolePathHasObjectUnifier.LeadRolePathDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(LeadRolePathHasObjectUnifier.LeadRolePathDomainRoleId), new DomainRoleClosureRestriction(LeadRolePathHasObjectUnifier.ObjectUnifierDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(NameGeneratorRefinesNameGenerator.RefinementDomainRoleId), new DomainRoleClosureRestriction(NameGeneratorRefinesNameGenerator.ParentDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(NameGeneratorRefinesNameGenerator.ParentDomainRoleId), new DomainRoleClosureRestriction(NameGeneratorRefinesNameGenerator.RefinementDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ObjectTypeHasAbbreviation.AbbreviationDomainRoleId), new DomainRoleClosureRestriction(ObjectTypeHasAbbreviation.ObjectTypeDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ObjectTypeHasAbbreviation.ObjectTypeDomainRoleId), new DomainRoleClosureRestriction(ObjectTypeHasAbbreviation.AbbreviationDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ObjectTypeHasCardinalityConstraint.CardinalityConstraintDomainRoleId), new DomainRoleClosureRestriction(ObjectTypeHasCardinalityConstraint.ObjectTypeDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ObjectTypeHasCardinalityConstraint.ObjectTypeDomainRoleId), new DomainRoleClosureRestriction(ObjectTypeHasCardinalityConstraint.CardinalityConstraintDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ObjectTypeHasDefinition.DefinitionDomainRoleId), new DomainRoleClosureRestriction(ObjectTypeHasDefinition.ObjectTypeDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ObjectTypeHasDefinition.ObjectTypeDomainRoleId), new DomainRoleClosureRestriction(ObjectTypeHasDefinition.DefinitionDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ObjectTypeHasNote.NoteDomainRoleId), new DomainRoleClosureRestriction(ObjectTypeHasNote.ObjectTypeDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ObjectTypeHasNote.ObjectTypeDomainRoleId), new DomainRoleClosureRestriction(ObjectTypeHasNote.NoteDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(PathedRoleHasValueConstraint.ValueConstraintDomainRoleId), new DomainRoleClosureRestriction(PathedRoleHasValueConstraint.PathedRoleDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(PathedRoleHasValueConstraint.PathedRoleDomainRoleId), new DomainRoleClosureRestriction(PathedRoleHasValueConstraint.ValueConstraintDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(QueryDefinesParameter.ParameterDomainRoleId), new DomainRoleClosureRestriction(QueryDefinesParameter.QueryDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(QueryDefinesParameter.QueryDomainRoleId), new DomainRoleClosureRestriction(QueryDefinesParameter.ParameterDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ReadingOrderHasReading.ReadingDomainRoleId), new DomainRoleClosureRestriction(ReadingOrderHasReading.ReadingOrderDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ReadingOrderHasReading.ReadingOrderDomainRoleId), new DomainRoleClosureRestriction(ReadingOrderHasReading.ReadingDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(RecognizedPhraseHasAbbreviation.AbbreviationDomainRoleId), new DomainRoleClosureRestriction(RecognizedPhraseHasAbbreviation.RecognizedPhraseDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(RecognizedPhraseHasAbbreviation.RecognizedPhraseDomainRoleId), new DomainRoleClosureRestriction(RecognizedPhraseHasAbbreviation.AbbreviationDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(RoleHasValueConstraint.ValueConstraintDomainRoleId), new DomainRoleClosureRestriction(RoleHasValueConstraint.RoleDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(RoleHasValueConstraint.RoleDomainRoleId), new DomainRoleClosureRestriction(RoleHasValueConstraint.ValueConstraintDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(RolePathOwnerOwnsLeadRolePath.RolePathDomainRoleId), new DomainRoleClosureRestriction(RolePathOwnerOwnsLeadRolePath.PathOwnerDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(RolePathOwnerOwnsLeadRolePath.PathOwnerDomainRoleId), new DomainRoleClosureRestriction(RolePathOwnerOwnsLeadRolePath.RolePathDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(RolePathOwnerOwnsSubquery.SubqueryDomainRoleId), new DomainRoleClosureRestriction(RolePathOwnerOwnsSubquery.PathOwnerDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(RolePathOwnerOwnsSubquery.PathOwnerDomainRoleId), new DomainRoleClosureRestriction(RolePathOwnerOwnsSubquery.SubqueryDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(RolePathRootHasValueConstraint.ValueConstraintDomainRoleId), new DomainRoleClosureRestriction(RolePathRootHasValueConstraint.PathRootDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(RolePathRootHasValueConstraint.PathRootDomainRoleId), new DomainRoleClosureRestriction(RolePathRootHasValueConstraint.ValueConstraintDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(RoleSubPathIsContinuationOfRolePath.SubPathDomainRoleId), new DomainRoleClosureRestriction(RoleSubPathIsContinuationOfRolePath.ParentRolePathDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(RoleSubPathIsContinuationOfRolePath.ParentRolePathDomainRoleId), new DomainRoleClosureRestriction(RoleSubPathIsContinuationOfRolePath.SubPathDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(SetComparisonConstraintHasDefinition.DefinitionDomainRoleId), new DomainRoleClosureRestriction(SetComparisonConstraintHasDefinition.SetComparisonConstraintDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(SetComparisonConstraintHasDefinition.SetComparisonConstraintDomainRoleId), new DomainRoleClosureRestriction(SetComparisonConstraintHasDefinition.DefinitionDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(SetComparisonConstraintHasNote.NoteDomainRoleId), new DomainRoleClosureRestriction(SetComparisonConstraintHasNote.SetComparisonConstraintDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(SetComparisonConstraintHasNote.SetComparisonConstraintDomainRoleId), new DomainRoleClosureRestriction(SetComparisonConstraintHasNote.NoteDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(SetComparisonConstraintHasRoleSequence.RoleSequenceDomainRoleId), new DomainRoleClosureRestriction(SetComparisonConstraintHasRoleSequence.ExternalConstraintDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(SetComparisonConstraintHasRoleSequence.ExternalConstraintDomainRoleId), new DomainRoleClosureRestriction(SetComparisonConstraintHasRoleSequence.RoleSequenceDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(SetConstraintHasDefinition.DefinitionDomainRoleId), new DomainRoleClosureRestriction(SetConstraintHasDefinition.SetConstraintDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(SetConstraintHasDefinition.SetConstraintDomainRoleId), new DomainRoleClosureRestriction(SetConstraintHasDefinition.DefinitionDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(SetConstraintHasNote.NoteDomainRoleId), new DomainRoleClosureRestriction(SetConstraintHasNote.SetConstraintDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(SetConstraintHasNote.SetConstraintDomainRoleId), new DomainRoleClosureRestriction(SetConstraintHasNote.NoteDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(SubqueryParameterInputFromPathConstant.SourceDomainRoleId), new DomainRoleClosureRestriction(SubqueryParameterInputFromPathConstant.ParameterInputDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(SubqueryParameterInputFromPathConstant.ParameterInputDomainRoleId), new DomainRoleClosureRestriction(SubqueryParameterInputFromPathConstant.SourceDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(SubtypeDerivationRuleHasDerivationNote.DerivationNoteDomainRoleId), new DomainRoleClosureRestriction(SubtypeDerivationRuleHasDerivationNote.DerivationRuleDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(SubtypeDerivationRuleHasDerivationNote.DerivationRuleDomainRoleId), new DomainRoleClosureRestriction(SubtypeDerivationRuleHasDerivationNote.DerivationNoteDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(SubtypeHasDerivationRule.DerivationRuleDomainRoleId), new DomainRoleClosureRestriction(SubtypeHasDerivationRule.SubtypeDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(SubtypeHasDerivationRule.SubtypeDomainRoleId), new DomainRoleClosureRestriction(SubtypeHasDerivationRule.DerivationRuleDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(UnaryRoleHasCardinalityConstraint.CardinalityConstraintDomainRoleId), new DomainRoleClosureRestriction(UnaryRoleHasCardinalityConstraint.UnaryRoleDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(UnaryRoleHasCardinalityConstraint.UnaryRoleDomainRoleId), new DomainRoleClosureRestriction(UnaryRoleHasCardinalityConstraint.CardinalityConstraintDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ValueConstraintHasDefinition.DefinitionDomainRoleId), new DomainRoleClosureRestriction(ValueConstraintHasDefinition.ValueConstraintDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ValueConstraintHasDefinition.ValueConstraintDomainRoleId), new DomainRoleClosureRestriction(ValueConstraintHasDefinition.DefinitionDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ValueConstraintHasNote.NoteDomainRoleId), new DomainRoleClosureRestriction(ValueConstraintHasNote.ValueConstraintDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ValueConstraintHasNote.ValueConstraintDomainRoleId), new DomainRoleClosureRestriction(ValueConstraintHasNote.NoteDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ValueConstraintHasValueRange.ValueRangeDomainRoleId), new DomainRoleClosureRestriction(ValueConstraintHasValueRange.ValueConstraintDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ValueConstraintHasValueRange.ValueConstraintDomainRoleId), new DomainRoleClosureRestriction(ValueConstraintHasValueRange.ValueRangeDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ValueTypeHasValueConstraint.ValueConstraintDomainRoleId), new DomainRoleClosureRestriction(ValueTypeHasValueConstraint.ValueTypeDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ValueTypeHasValueConstraint.ValueTypeDomainRoleId), new DomainRoleClosureRestriction(ValueTypeHasValueConstraint.ValueConstraintDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ContainedPart);
			#endregion // Closures for standard embedding relationships
			#region Embedded relationship ordering
			closureManager.AddOrderedRole(CalculatedPathValueHasInput.CalculatedValueDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddOrderedRole(CardinalityConstraintHasRange.ConstraintDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddOrderedRole(EntityTypeHasEntityTypeInstance.EntityTypeDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddOrderedRole(EntityTypeSubtypeHasEntityTypeSubtypeInstance.EntityTypeSubtypeDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddOrderedRole(FactTypeHasFactTypeInstance.FactTypeDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddOrderedRole(FactTypeHasReadingOrder.FactTypeDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddOrderedRole(FactTypeHasRole.FactTypeDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddOrderedRole(FunctionOperatesOnParameter.FunctionDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddOrderedRole(LeadRolePathCalculatesCalculatedPathValue.LeadRolePathDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddOrderedRole(LeadRolePathHasObjectUnifier.LeadRolePathDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddOrderedRole(NameGeneratorRefinesNameGenerator.ParentDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddOrderedRole(QueryDefinesParameter.QueryDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddOrderedRole(ReadingOrderHasReading.ReadingOrderDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddOrderedRole(RolePathOwnerOwnsLeadRolePath.PathOwnerDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddOrderedRole(RolePathOwnerOwnsSubquery.PathOwnerDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddOrderedRole(RoleSubPathIsContinuationOfRolePath.ParentRolePathDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddOrderedRole(SetComparisonConstraintHasRoleSequence.ExternalConstraintDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddOrderedRole(ValueConstraintHasValueRange.ValueConstraintDomainRoleId, MergeIntegrationOrder.AfterLeading);
			#endregion // Embedded relationship ordering
			#region Closures for explicit relationships
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(CalculatedPathValueAggregationContextIncludesPathedRole.CalculatedValueDomainRoleId), new DomainRoleClosureRestriction(CalculatedPathValueAggregationContextIncludesPathedRole.PathedRoleDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.InternalReferencedPart);
			closureManager.AddOrderedRole(CalculatedPathValueAggregationContextIncludesPathedRole.CalculatedValueDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(CalculatedPathValueAggregationContextIncludesRolePathRoot.CalculatedValueDomainRoleId), new DomainRoleClosureRestriction(CalculatedPathValueAggregationContextIncludesRolePathRoot.PathRootDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.InternalReferencedPart);
			closureManager.AddOrderedRole(CalculatedPathValueAggregationContextIncludesRolePathRoot.CalculatedValueDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(CalculatedPathValueInputBindsToCalculatedPathValue.InputDomainRoleId), new DomainRoleClosureRestriction(CalculatedPathValueInputBindsToCalculatedPathValue.SourceDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.InternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(CalculatedPathValueInputBindsToPathedRole.InputDomainRoleId), new DomainRoleClosureRestriction(CalculatedPathValueInputBindsToPathedRole.SourceDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.InternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(CalculatedPathValueInputBindsToRolePathRoot.InputDomainRoleId), new DomainRoleClosureRestriction(CalculatedPathValueInputBindsToRolePathRoot.SourceDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.InternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(CalculatedPathValueInputCorrespondsToFunctionParameter.InputDomainRoleId), new DomainRoleClosureRestriction(CalculatedPathValueInputCorrespondsToFunctionParameter.ParameterDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(CalculatedPathValueIsCalculatedWithFunction.CalculatedValueDomainRoleId), new DomainRoleClosureRestriction(CalculatedPathValueIsCalculatedWithFunction.FunctionDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ConstraintRoleProjectedFromCalculatedPathValue.ConstraintRoleProjectionDomainRoleId), new DomainRoleClosureRestriction(ConstraintRoleProjectedFromCalculatedPathValue.SourceDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ConstraintRoleProjectedFromPathedRole.ConstraintRoleProjectionDomainRoleId), new DomainRoleClosureRestriction(ConstraintRoleProjectedFromPathedRole.SourceDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ConstraintRoleProjectedFromRolePathRoot.ConstraintRoleProjectionDomainRoleId), new DomainRoleClosureRestriction(ConstraintRoleProjectedFromRolePathRoot.SourceDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ConstraintRoleProjection.JoinPathProjectionDomainRoleId), new DomainRoleClosureRestriction(ConstraintRoleProjection.ProjectedConstraintRoleDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.InternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ConstraintRoleSequenceHasRole.RoleDomainRoleId), new DomainRoleClosureRestriction(ConstraintRoleSequenceHasRole.ConstraintRoleSequenceDomainRoleId), CopyClosureDirectiveOptions.None, delegate(ElementLink link)
				{
					IConstraint constraint = ((ConstraintRoleSequenceHasRole)link).ConstraintRoleSequence as IConstraint;
					if (constraint != null && constraint.ConstraintIsInternal)
					{
						return CopyClosureBehavior.ExternalReferencedPart;
					}
					return CopyClosureBehavior.Ignored;
				});
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ConstraintRoleSequenceHasRole.ConstraintRoleSequenceDomainRoleId), new DomainRoleClosureRestriction(ConstraintRoleSequenceHasRole.RoleDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddOrderedRole(ConstraintRoleSequenceHasRole.ConstraintRoleSequenceDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ConstraintRoleSequenceJoinPathProjection.JoinPathDomainRoleId), new DomainRoleClosureRestriction(ConstraintRoleSequenceJoinPathProjection.RolePathDomainRoleId), CopyClosureDirectiveOptions.None, delegate(ElementLink link)
				{
					ConstraintRoleSequenceJoinPathProjection projection = (ConstraintRoleSequenceJoinPathProjection)link;
					if (projection.JoinPath == projection.RolePath.PathOwner)
					{
						return CopyClosureBehavior.InternalReferencedPart;
					}
					return CopyClosureBehavior.ExternalReferencedPart;
				});
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(DerivedRoleProjectedFromCalculatedPathValue.RoleProjectionDomainRoleId), new DomainRoleClosureRestriction(DerivedRoleProjectedFromCalculatedPathValue.SourceDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(DerivedRoleProjectedFromPathedRole.RoleProjectionDomainRoleId), new DomainRoleClosureRestriction(DerivedRoleProjectedFromPathedRole.SourceDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(DerivedRoleProjectedFromRolePathRoot.RoleProjectionDomainRoleId), new DomainRoleClosureRestriction(DerivedRoleProjectedFromRolePathRoot.SourceDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(DerivedRoleProjection.DerivationProjectionDomainRoleId), new DomainRoleClosureRestriction(DerivedRoleProjection.ProjectedRoleDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.InternalReferencedPart);
			// Make the preferred identifier a composite part instead of an external reference to enforce the 1-1 nature of the relationship.
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(EntityTypeHasPreferredIdentifier.PreferredIdentifierForDomainRoleId), new DomainRoleClosureRestriction(EntityTypeHasPreferredIdentifier.PreferredIdentifierDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalCompositePart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(EntityTypeInstanceHasRoleInstance.EntityTypeInstanceDomainRoleId), new DomainRoleClosureRestriction(EntityTypeInstanceHasRoleInstance.RoleInstanceDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(EntityTypeRoleInstance.RoleDomainRoleId), new DomainRoleClosureRestriction(EntityTypeRoleInstance.ObjectTypeInstanceDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(EntityTypeSubtypeInstanceHasSupertypeInstance.EntityTypeSubtypeInstanceDomainRoleId), new DomainRoleClosureRestriction(EntityTypeSubtypeInstanceHasSupertypeInstance.SupertypeInstanceDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ExclusiveOrConstraintCoupler.MandatoryConstraintDomainRoleId), new DomainRoleClosureRestriction(ExclusiveOrConstraintCoupler.ExclusionConstraintDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalCompositePart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ExclusiveOrConstraintCoupler.ExclusionConstraintDomainRoleId), new DomainRoleClosureRestriction(ExclusiveOrConstraintCoupler.MandatoryConstraintDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalCompositePart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(FactTypeInstanceHasRoleInstance.FactTypeInstanceDomainRoleId), new DomainRoleClosureRestriction(FactTypeInstanceHasRoleInstance.RoleInstanceDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.InternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(FactTypeRoleInstance.RoleDomainRoleId), new DomainRoleClosureRestriction(FactTypeRoleInstance.ObjectTypeInstanceDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(GroupingElementRelationship.GroupingDomainRoleId), new DomainRoleClosureRestriction(GroupingElementRelationship.ElementDomainRoleId), CopyClosureDirectiveOptions.RootElementOnly, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(GroupingElementRelationship.ElementDomainRoleId), new DomainRoleClosureRestriction(GroupingElementRelationship.GroupingDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(LeadRolePathSatisfiesCalculatedCondition.LeadRolePathDomainRoleId), new DomainRoleClosureRestriction(LeadRolePathSatisfiesCalculatedCondition.CalculatedConditionDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.InternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(LeadRolePathSatisfiesCalculatedCondition.LeadRolePathDomainRoleId), new DomainRoleClosureRestriction(LeadRolePathSatisfiesCalculatedCondition.CalculatedConditionDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.InternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ModelNoteReferencesModelElement.ElementDomainRoleId), new DomainRoleClosureRestriction(ModelNoteReferencesModelElement.NoteDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(Objectification.NestingTypeDomainRoleId), new DomainRoleClosureRestriction(Objectification.NestedFactTypeDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalCompositePart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(Objectification.NestedFactTypeDomainRoleId), new DomainRoleClosureRestriction(Objectification.NestingTypeDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalCompositePart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ObjectificationImpliesFactType.ImpliedFactTypeDomainRoleId), new DomainRoleClosureRestriction(ObjectificationImpliesFactType.ImpliedByObjectificationDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalCompositePart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ObjectificationImpliesFactType.ImpliedByObjectificationDomainRoleId), new DomainRoleClosureRestriction(ObjectificationImpliesFactType.ImpliedFactTypeDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalCompositePart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ObjectifiedUnaryRoleHasRole.ObjectifiedUnaryRoleDomainRoleId), new DomainRoleClosureRestriction(ObjectifiedUnaryRoleHasRole.TargetRoleDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalCompositePart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ObjectTypePlaysRole.RolePlayerDomainRoleId), new DomainRoleClosureRestriction(ObjectTypePlaysRole.PlayedRoleDomainRoleId, SubtypeMetaRole.DomainClassId, false), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ObjectTypePlaysRole.PlayedRoleDomainRoleId), new DomainRoleClosureRestriction(ObjectTypePlaysRole.RolePlayerDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(PathedRole.RolePathDomainRoleId), new DomainRoleClosureRestriction(PathedRole.RoleDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddOrderedRole(PathedRole.RolePathDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(PathObjectUnifierUnifiesPathedRole.ObjectUnifierDomainRoleId), new DomainRoleClosureRestriction(PathObjectUnifierUnifiesPathedRole.PathedRoleDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.InternalReferencedPart);
			closureManager.AddOrderedRole(PathObjectUnifierUnifiesPathedRole.ObjectUnifierDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(PathObjectUnifierUnifiesRolePathRoot.ObjectUnifierDomainRoleId), new DomainRoleClosureRestriction(PathObjectUnifierUnifiesRolePathRoot.PathRootDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.InternalReferencedPart);
			closureManager.AddOrderedRole(PathObjectUnifierUnifiesRolePathRoot.ObjectUnifierDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ReadingOrderHasRole.ReadingOrderDomainRoleId), new DomainRoleClosureRestriction(ReadingOrderHasRole.RoleDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.InternalReferencedPart);
			closureManager.AddOrderedRole(ReadingOrderHasRole.ReadingOrderDomainRoleId, MergeIntegrationOrder.AfterLeading);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ReferenceModeHasReferenceModeKind.ReferenceModeDomainRoleId), new DomainRoleClosureRestriction(ReferenceModeHasReferenceModeKind.KindDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(RolePathObjectTypeRoot.RolePathDomainRoleId), new DomainRoleClosureRestriction(RolePathObjectTypeRoot.RootObjectTypeDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(RolePathOwnerUsesSharedLeadRolePath.PathOwnerDomainRoleId), new DomainRoleClosureRestriction(RolePathOwnerUsesSharedLeadRolePath.RolePathDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(RolePathOwnerUsesSharedSubquery.PathOwnerDomainRoleId), new DomainRoleClosureRestriction(RolePathOwnerUsesSharedSubquery.SubqueryDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(RoleProxyHasRole.ProxyDomainRoleId), new DomainRoleClosureRestriction(RoleProxyHasRole.TargetRoleDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalCompositePart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(RoleSetDerivationProjection.DerivationRuleDomainRoleId), new DomainRoleClosureRestriction(RoleSetDerivationProjection.RolePathDomainRoleId), CopyClosureDirectiveOptions.None, delegate(ElementLink link)
				{
					RoleSetDerivationProjection projection = (RoleSetDerivationProjection)link;
					if (projection.DerivationRule == projection.RolePath.PathOwner)
					{
						return CopyClosureBehavior.InternalReferencedPart;
					}
					return CopyClosureBehavior.ExternalReferencedPart;
				});
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ValueTypeHasDataType.ValueTypeDomainRoleId), new DomainRoleClosureRestriction(ValueTypeHasDataType.DataTypeDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.ExternalReferencedPart);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ValueTypeHasValueTypeInstance.ValueTypeInstanceDomainRoleId), new DomainRoleClosureRestriction(ValueTypeHasValueTypeInstance.ValueTypeDomainRoleId), CopyClosureDirectiveOptions.None, CopyClosureBehavior.Container);
			closureManager.AddCopyClosureDirective(new DomainRoleClosureRestriction(ValueTypeHasValueTypeInstance.ValueTypeDomainRoleId), new DomainRoleClosureRestriction(ValueTypeHasValueTypeInstance.ValueTypeInstanceDomainRoleId), CopyClosureDirectiveOptions.RootElementOnly, CopyClosureBehavior.ContainedPart);
			closureManager.AddOrderedRole(ValueTypeHasValueTypeInstance.ValueTypeDomainRoleId, MergeIntegrationOrder.AfterLeading);
			#endregion // Closures for explicit relationships
			#region Implied reference callbacks
			closureManager.AddImpliedReference(ObjectType.DomainClassId, true, delegate(ModelElement element, Action<ModelElement> notifyImpliedReference)
				{
					CustomReferenceMode referenceMode = ((ObjectType)element).ReferenceMode as CustomReferenceMode;
					if (referenceMode != null)
					{
						notifyImpliedReference(referenceMode);
					}
				});
			#endregion // Implied reference callbacks
			#region Register ignored properties
			closureManager.AddIgnoredProperty(CardinalityConstraint.DefinitionTextDomainPropertyId);
			closureManager.AddIgnoredProperty(CardinalityConstraint.NoteTextDomainPropertyId);
			closureManager.AddIgnoredProperty(CardinalityConstraint.TextDomainPropertyId);
			closureManager.AddIgnoredProperty(CardinalityConstraint.TextChangedDomainPropertyId);
			closureManager.AddIgnoredProperty(ElementGrouping.DefinitionTextDomainPropertyId);
			closureManager.AddIgnoredProperty(ElementGrouping.NoteTextDomainPropertyId);
			closureManager.AddIgnoredProperty(FactType.DefinitionTextDomainPropertyId);
			closureManager.AddIgnoredProperty(FactType.DerivationNoteDisplayDomainPropertyId);
			closureManager.AddIgnoredProperty(FactType.DerivationStorageDisplayDomainPropertyId);
			closureManager.AddIgnoredProperty(FactType.GeneratedNameDomainPropertyId);
			closureManager.AddIgnoredProperty(FactType.NameDomainPropertyId);
			closureManager.AddIgnoredProperty(FactType.NameChangedDomainPropertyId);
			closureManager.AddIgnoredProperty(FactType.NoteTextDomainPropertyId);
			closureManager.AddIgnoredProperty(FactTypeInstance.NameDomainPropertyId);
			closureManager.AddIgnoredProperty(FactTypeInstance.NameDomainPropertyId);
			closureManager.AddIgnoredProperty(FactTypeInstance.NameChangedDomainPropertyId);
			closureManager.AddIgnoredProperty(FactTypeInstance.NameChangedDomainPropertyId);
			closureManager.AddIgnoredProperty(LeadRolePath.NoteTextDomainPropertyId);
			closureManager.AddIgnoredProperty(ObjectType.DataTypeLengthDomainPropertyId);
			closureManager.AddIgnoredProperty(ObjectType.DataTypeLengthDomainPropertyId);
			closureManager.AddIgnoredProperty(ObjectType.DataTypeScaleDomainPropertyId);
			closureManager.AddIgnoredProperty(ObjectType.DataTypeScaleDomainPropertyId);
			closureManager.AddIgnoredProperty(ObjectType.DefinitionTextDomainPropertyId);
			closureManager.AddIgnoredProperty(ObjectType.DerivationNoteDisplayDomainPropertyId);
			closureManager.AddIgnoredProperty(ObjectType.DerivationStorageDisplayDomainPropertyId);
			closureManager.AddIgnoredProperty(ObjectType.IsSupertypePersonalDomainPropertyId);
			closureManager.AddIgnoredProperty(ObjectType.IsValueTypeDomainPropertyId);
			closureManager.AddIgnoredProperty(ObjectType.NoteTextDomainPropertyId);
			closureManager.AddIgnoredProperty(ObjectType.ReferenceModeDecoratedStringDomainPropertyId);
			closureManager.AddIgnoredProperty(ObjectType.ReferenceModeDisplayDomainPropertyId);
			closureManager.AddIgnoredProperty(ObjectType.ReferenceModeDisplayDomainPropertyId);
			closureManager.AddIgnoredProperty(ObjectType.ReferenceModeStringDomainPropertyId);
			closureManager.AddIgnoredProperty(ObjectType.TreatAsPersonalDomainPropertyId);
			closureManager.AddIgnoredProperty(ObjectType.ValueRangeTextDomainPropertyId);
			closureManager.AddIgnoredProperty(ObjectType.ValueTypeValueRangeTextDomainPropertyId);
			closureManager.AddIgnoredProperty(ObjectTypeInstance.IdentifierNameDomainPropertyId);
			closureManager.AddIgnoredProperty(ObjectTypeInstance.IdentifierNameDomainPropertyId);
			closureManager.AddIgnoredProperty(ObjectTypeInstance.NameDomainPropertyId);
			closureManager.AddIgnoredProperty(ObjectTypeInstance.NameDomainPropertyId);
			closureManager.AddIgnoredProperty(ObjectTypeInstance.NameChangedDomainPropertyId);
			closureManager.AddIgnoredProperty(ObjectTypeInstance.NameChangedDomainPropertyId);
			closureManager.AddIgnoredProperty(ORMModel.DefinitionTextDomainPropertyId);
			closureManager.AddIgnoredProperty(ORMModel.NoteTextDomainPropertyId);
			closureManager.AddIgnoredProperty(Reading.IsPrimaryForFactTypeDomainPropertyId);
			closureManager.AddIgnoredProperty(Reading.IsPrimaryForReadingOrderDomainPropertyId);
			closureManager.AddIgnoredProperty(Reading.SignatureDomainPropertyId);
			closureManager.AddIgnoredProperty(ReadingOrder.ReadingTextDomainPropertyId);
			closureManager.AddIgnoredProperty(Role.IsMandatoryDomainPropertyId);
			closureManager.AddIgnoredProperty(Role.MandatoryConstraintModalityDomainPropertyId);
			closureManager.AddIgnoredProperty(Role.MandatoryConstraintNameDomainPropertyId);
			closureManager.AddIgnoredProperty(Role.MultiplicityDomainPropertyId);
			closureManager.AddIgnoredProperty(Role.ObjectificationOppositeRoleNameDomainPropertyId);
			closureManager.AddIgnoredProperty(Role.ValueRangeTextDomainPropertyId);
			closureManager.AddIgnoredProperty(SetComparisonConstraint.DefinitionTextDomainPropertyId);
			closureManager.AddIgnoredProperty(SetComparisonConstraint.NoteTextDomainPropertyId);
			closureManager.AddIgnoredProperty(SetConstraint.DefinitionTextDomainPropertyId);
			closureManager.AddIgnoredProperty(SetConstraint.NoteTextDomainPropertyId);
			closureManager.AddIgnoredProperty(UniquenessConstraint.IsPreferredDomainPropertyId);
			closureManager.AddIgnoredProperty(ValueConstraint.DefinitionTextDomainPropertyId);
			closureManager.AddIgnoredProperty(ValueConstraint.NoteTextDomainPropertyId);
			closureManager.AddIgnoredProperty(ValueConstraint.TextDomainPropertyId);
			closureManager.AddIgnoredProperty(ValueConstraint.TextChangedDomainPropertyId);
			#endregion // Register ignored properties
			#region Register conditional properties
			closureManager.AddConditionalProperty(ObjectType.IsIndependentDomainPropertyId, delegate(ModelElement sourceElement, ModelElement targetElement)
				{
					ObjectType sourceObjectType = (ObjectType)sourceElement;
					ObjectType targetObjectType;
					if (sourceObjectType.IsIndependent && null != (targetObjectType = (ObjectType)targetElement) && !targetObjectType.AllowIsIndependent())
					{
						return false;
					}
					return true;
				});
			closureManager.AddConditionalProperty(ORMNamedElement.NameDomainPropertyId, delegate(ModelElement sourceElement, ModelElement targetElement)
				{
					IDefaultNamePattern defaultName;
					string pattern;
					if ((defaultName = sourceElement as IDefaultNamePattern) != null && defaultName.DefaultNameResettable && Utility.IsNumberDecoratedName(((ORMNamedElement)sourceElement).Name, string.IsNullOrEmpty(pattern = defaultName.DefaultNamePattern) ? System.ComponentModel.TypeDescriptor.GetClassName(sourceElement) : pattern))
					{
						return false;
					}
					else if (targetElement != null)
					{
						string targetName = ((ORMNamedElement)targetElement).Name;
						if ((defaultName = sourceElement as IDefaultNamePattern) != null && defaultName.DefaultNameResettable && Utility.IsNumberDecoratedName(targetName, string.IsNullOrEmpty(pattern = defaultName.DefaultNamePattern) ? System.ComponentModel.TypeDescriptor.GetClassName(targetElement) : pattern))
						{
							// Use a non-generated source name over a generated target name.
							return true;
						}
						// Override an empty target name with a non-empty source name.
						return !string.IsNullOrEmpty(targetName);
					}
					return true;
				});
			#endregion // Register conditional properties
		}
		void ICopyClosureProvider.AddCopyClosureDirectives(ICopyClosureManager closureManager)
		{
			this.AddCopyClosureDirectives(closureManager);
		}
	}
	#endregion // Customize Copy Closure for ORMCoreDomainModel
	#region One-to-One Embedded Element Equivalence
	#region ConstraintRoleSequenceJoinPath Element Equivalence
	partial class ConstraintRoleSequenceJoinPath : IElementEquivalence
	{
		/// <summary>Implements <cref name="IElementEquivalence.MapEquivalentElements"/></summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			ConstraintRoleSequence parentConstraintRoleSequence;
			if (null != (parentConstraintRoleSequence = this.RoleSequence))
			{
				// Embedded through the ConstraintRoleSequenceHasJoinPath relationship
				ConstraintRoleSequence otherParentConstraintRoleSequence;
				ConstraintRoleSequenceJoinPath otherConstraintRoleSequenceJoinPath;
				if (null != (otherParentConstraintRoleSequence = CopyMergeUtility.GetEquivalentElement(parentConstraintRoleSequence, foreignStore, elementTracker)) && null != (otherConstraintRoleSequenceJoinPath = otherParentConstraintRoleSequence.JoinPath))
				{
					elementTracker.AddEquivalentElement(this, otherConstraintRoleSequenceJoinPath);
					return true;
				}
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return this.MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // ConstraintRoleSequenceJoinPath Element Equivalence
	#region Definition Element Equivalence
	partial class Definition : IElementEquivalence
	{
		/// <summary>Implements <cref name="IElementEquivalence.MapEquivalentElements"/></summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			CardinalityConstraint parentCardinalityConstraint;
			ElementGrouping parentElementGrouping;
			FactType parentFactType;
			ObjectType parentObjectType;
			ORMModel parentORMModel;
			SetComparisonConstraint parentSetComparisonConstraint;
			SetConstraint parentSetConstraint;
			ValueConstraint parentValueConstraint;
			if (null != (parentCardinalityConstraint = this.CardinalityConstraint))
			{
				// Embedded through the CardinalityConstraintHasDefinition relationship
				CardinalityConstraint otherParentCardinalityConstraint;
				Definition otherDefinition;
				if (null != (otherParentCardinalityConstraint = CopyMergeUtility.GetEquivalentElement(parentCardinalityConstraint, foreignStore, elementTracker)) && null != (otherDefinition = otherParentCardinalityConstraint.Definition))
				{
					elementTracker.AddEquivalentElement(this, otherDefinition);
					return true;
				}
			}
			else if (null != (parentElementGrouping = this.Grouping))
			{
				// Embedded through the ElementGroupingHasDefinition relationship
				ElementGrouping otherParentElementGrouping;
				Definition otherDefinition;
				if (null != (otherParentElementGrouping = CopyMergeUtility.GetEquivalentElement(parentElementGrouping, foreignStore, elementTracker)) && null != (otherDefinition = otherParentElementGrouping.Definition))
				{
					elementTracker.AddEquivalentElement(this, otherDefinition);
					return true;
				}
			}
			else if (null != (parentFactType = this.FactType))
			{
				// Embedded through the FactTypeHasDefinition relationship
				FactType otherParentFactType;
				Definition otherDefinition;
				if (null != (otherParentFactType = CopyMergeUtility.GetEquivalentElement(parentFactType, foreignStore, elementTracker)) && null != (otherDefinition = otherParentFactType.Definition))
				{
					elementTracker.AddEquivalentElement(this, otherDefinition);
					return true;
				}
			}
			else if (null != (parentObjectType = this.ObjectType))
			{
				// Embedded through the ObjectTypeHasDefinition relationship
				ObjectType otherParentObjectType;
				Definition otherDefinition;
				if (null != (otherParentObjectType = CopyMergeUtility.GetEquivalentElement(parentObjectType, foreignStore, elementTracker)) && null != (otherDefinition = otherParentObjectType.Definition))
				{
					elementTracker.AddEquivalentElement(this, otherDefinition);
					return true;
				}
			}
			else if (null != (parentORMModel = this.Model))
			{
				// Embedded through the ModelHasDefinition relationship
				ORMModel otherParentORMModel;
				Definition otherDefinition;
				if (null != (otherParentORMModel = CopyMergeUtility.GetEquivalentElement(parentORMModel, foreignStore, elementTracker)) && null != (otherDefinition = otherParentORMModel.Definition))
				{
					elementTracker.AddEquivalentElement(this, otherDefinition);
					return true;
				}
			}
			else if (null != (parentSetComparisonConstraint = this.SetComparisonConstraint))
			{
				// Embedded through the SetComparisonConstraintHasDefinition relationship
				SetComparisonConstraint otherParentSetComparisonConstraint;
				Definition otherDefinition;
				if (null != (otherParentSetComparisonConstraint = CopyMergeUtility.GetEquivalentElement(parentSetComparisonConstraint, foreignStore, elementTracker)) && null != (otherDefinition = otherParentSetComparisonConstraint.Definition))
				{
					elementTracker.AddEquivalentElement(this, otherDefinition);
					return true;
				}
			}
			else if (null != (parentSetConstraint = this.SetConstraint))
			{
				// Embedded through the SetConstraintHasDefinition relationship
				SetConstraint otherParentSetConstraint;
				Definition otherDefinition;
				if (null != (otherParentSetConstraint = CopyMergeUtility.GetEquivalentElement(parentSetConstraint, foreignStore, elementTracker)) && null != (otherDefinition = otherParentSetConstraint.Definition))
				{
					elementTracker.AddEquivalentElement(this, otherDefinition);
					return true;
				}
			}
			else if (null != (parentValueConstraint = this.ValueConstraint))
			{
				// Embedded through the ValueConstraintHasDefinition relationship
				ValueConstraint otherParentValueConstraint;
				Definition otherDefinition;
				if (null != (otherParentValueConstraint = CopyMergeUtility.GetEquivalentElement(parentValueConstraint, foreignStore, elementTracker)) && null != (otherDefinition = otherParentValueConstraint.Definition))
				{
					elementTracker.AddEquivalentElement(this, otherDefinition);
					return true;
				}
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return this.MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // Definition Element Equivalence
	#region DerivationNote Element Equivalence
	partial class DerivationNote : IElementEquivalence
	{
		/// <summary>Implements <cref name="IElementEquivalence.MapEquivalentElements"/></summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			FactTypeDerivationRule parentFactTypeDerivationRule;
			SubtypeDerivationRule parentSubtypeDerivationRule;
			if (null != (parentFactTypeDerivationRule = this.FactTypeDerivationRule))
			{
				// Embedded through the FactTypeDerivationRuleHasDerivationNote relationship
				FactTypeDerivationRule otherParentFactTypeDerivationRule;
				DerivationNote otherDerivationNote;
				if (null != (otherParentFactTypeDerivationRule = CopyMergeUtility.GetEquivalentElement(parentFactTypeDerivationRule, foreignStore, elementTracker)) && null != (otherDerivationNote = otherParentFactTypeDerivationRule.DerivationNote))
				{
					elementTracker.AddEquivalentElement(this, otherDerivationNote);
					return true;
				}
			}
			else if (null != (parentSubtypeDerivationRule = this.SubtypeDerivationRule))
			{
				// Embedded through the SubtypeDerivationRuleHasDerivationNote relationship
				SubtypeDerivationRule otherParentSubtypeDerivationRule;
				DerivationNote otherDerivationNote;
				if (null != (otherParentSubtypeDerivationRule = CopyMergeUtility.GetEquivalentElement(parentSubtypeDerivationRule, foreignStore, elementTracker)) && null != (otherDerivationNote = otherParentSubtypeDerivationRule.DerivationNote))
				{
					elementTracker.AddEquivalentElement(this, otherDerivationNote);
					return true;
				}
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return this.MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // DerivationNote Element Equivalence
	#region ModelErrorDisplayFilter Element Equivalence
	partial class ModelErrorDisplayFilter : IElementEquivalence
	{
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			ORMModel parentORMModel;
			if (null != (parentORMModel = this.Model))
			{
				// Embedded through the ModelHasModelErrorDisplayFilter relationship
				ORMModel otherParentORMModel;
				ModelErrorDisplayFilter otherModelErrorDisplayFilter;
				if (null != (otherParentORMModel = CopyMergeUtility.GetEquivalentElement(parentORMModel, foreignStore, elementTracker)) && null != (otherModelErrorDisplayFilter = otherParentORMModel.ModelErrorDisplayFilter))
				{
					elementTracker.AddEquivalentElement(this, otherModelErrorDisplayFilter);
					return true;
				}
			}
			return false;
		}
	}
	#endregion // ModelErrorDisplayFilter Element Equivalence
	#region Note Element Equivalence
	partial class Note : IElementEquivalence
	{
		/// <summary>Implements <cref name="IElementEquivalence.MapEquivalentElements"/></summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			CardinalityConstraint parentCardinalityConstraint;
			ElementGrouping parentElementGrouping;
			FactType parentFactType;
			LeadRolePath parentLeadRolePath;
			ObjectType parentObjectType;
			ORMModel parentORMModel;
			SetComparisonConstraint parentSetComparisonConstraint;
			SetConstraint parentSetConstraint;
			ValueConstraint parentValueConstraint;
			if (null != (parentCardinalityConstraint = this.CardinalityConstraint))
			{
				// Embedded through the CardinalityConstraintHasNote relationship
				CardinalityConstraint otherParentCardinalityConstraint;
				Note otherNote;
				if (null != (otherParentCardinalityConstraint = CopyMergeUtility.GetEquivalentElement(parentCardinalityConstraint, foreignStore, elementTracker)) && null != (otherNote = otherParentCardinalityConstraint.Note))
				{
					elementTracker.AddEquivalentElement(this, otherNote);
					return true;
				}
			}
			else if (null != (parentElementGrouping = this.Grouping))
			{
				// Embedded through the ElementGroupingHasNote relationship
				ElementGrouping otherParentElementGrouping;
				Note otherNote;
				if (null != (otherParentElementGrouping = CopyMergeUtility.GetEquivalentElement(parentElementGrouping, foreignStore, elementTracker)) && null != (otherNote = otherParentElementGrouping.Note))
				{
					elementTracker.AddEquivalentElement(this, otherNote);
					return true;
				}
			}
			else if (null != (parentFactType = this.FactType))
			{
				// Embedded through the FactTypeHasNote relationship
				FactType otherParentFactType;
				Note otherNote;
				if (null != (otherParentFactType = CopyMergeUtility.GetEquivalentElement(parentFactType, foreignStore, elementTracker)) && null != (otherNote = otherParentFactType.Note))
				{
					elementTracker.AddEquivalentElement(this, otherNote);
					return true;
				}
			}
			else if (null != (parentLeadRolePath = this.RolePath))
			{
				// Embedded through the LeadRolePathHasNote relationship
				LeadRolePath otherParentLeadRolePath;
				Note otherNote;
				if (null != (otherParentLeadRolePath = CopyMergeUtility.GetEquivalentElement(parentLeadRolePath, foreignStore, elementTracker)) && null != (otherNote = otherParentLeadRolePath.Note))
				{
					elementTracker.AddEquivalentElement(this, otherNote);
					return true;
				}
			}
			else if (null != (parentObjectType = this.ObjectType))
			{
				// Embedded through the ObjectTypeHasNote relationship
				ObjectType otherParentObjectType;
				Note otherNote;
				if (null != (otherParentObjectType = CopyMergeUtility.GetEquivalentElement(parentObjectType, foreignStore, elementTracker)) && null != (otherNote = otherParentObjectType.Note))
				{
					elementTracker.AddEquivalentElement(this, otherNote);
					return true;
				}
			}
			else if (null != (parentORMModel = this.PrimaryForModel))
			{
				// Embedded through the ModelHasPrimaryNote relationship
				ORMModel otherParentORMModel;
				Note otherNote;
				if (null != (otherParentORMModel = CopyMergeUtility.GetEquivalentElement(parentORMModel, foreignStore, elementTracker)) && null != (otherNote = otherParentORMModel.Note))
				{
					elementTracker.AddEquivalentElement(this, otherNote);
					return true;
				}
			}
			else if (null != (parentSetComparisonConstraint = this.SetComparisonConstraint))
			{
				// Embedded through the SetComparisonConstraintHasNote relationship
				SetComparisonConstraint otherParentSetComparisonConstraint;
				Note otherNote;
				if (null != (otherParentSetComparisonConstraint = CopyMergeUtility.GetEquivalentElement(parentSetComparisonConstraint, foreignStore, elementTracker)) && null != (otherNote = otherParentSetComparisonConstraint.Note))
				{
					elementTracker.AddEquivalentElement(this, otherNote);
					return true;
				}
			}
			else if (null != (parentSetConstraint = this.SetConstraint))
			{
				// Embedded through the SetConstraintHasNote relationship
				SetConstraint otherParentSetConstraint;
				Note otherNote;
				if (null != (otherParentSetConstraint = CopyMergeUtility.GetEquivalentElement(parentSetConstraint, foreignStore, elementTracker)) && null != (otherNote = otherParentSetConstraint.Note))
				{
					elementTracker.AddEquivalentElement(this, otherNote);
					return true;
				}
			}
			else if (null != (parentValueConstraint = this.ValueConstraint))
			{
				// Embedded through the ValueConstraintHasNote relationship
				ValueConstraint otherParentValueConstraint;
				Note otherNote;
				if (null != (otherParentValueConstraint = CopyMergeUtility.GetEquivalentElement(parentValueConstraint, foreignStore, elementTracker)) && null != (otherNote = otherParentValueConstraint.Note))
				{
					elementTracker.AddEquivalentElement(this, otherNote);
					return true;
				}
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return this.MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // Note Element Equivalence
	#region ObjectTypeCardinalityConstraint Element Equivalence
	partial class ObjectTypeCardinalityConstraint : IElementEquivalence
	{
		/// <summary>Implements <cref name="IElementEquivalence.MapEquivalentElements"/></summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			ObjectType parentObjectType;
			if (null != (parentObjectType = this.ObjectType))
			{
				// Embedded through the ObjectTypeHasCardinalityConstraint relationship
				ObjectType otherParentObjectType;
				ObjectTypeCardinalityConstraint otherObjectTypeCardinalityConstraint;
				if (null != (otherParentObjectType = CopyMergeUtility.GetEquivalentElement(parentObjectType, foreignStore, elementTracker)) && null != (otherObjectTypeCardinalityConstraint = otherParentObjectType.Cardinality))
				{
					elementTracker.AddEquivalentElement(this, otherObjectTypeCardinalityConstraint);
					this.MatchRanges(otherObjectTypeCardinalityConstraint, elementTracker);
					return true;
				}
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return this.MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // ObjectTypeCardinalityConstraint Element Equivalence
	#region PathConditionRoleValueConstraint Element Equivalence
	partial class PathConditionRoleValueConstraint : IElementEquivalence
	{
		/// <summary>Implements <cref name="IElementEquivalence.MapEquivalentElements"/></summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			PathedRole parentPathedRole;
			if (null != (parentPathedRole = this.PathedRole))
			{
				// Embedded through the PathedRoleHasValueConstraint relationship
				PathedRole otherParentPathedRole;
				PathConditionRoleValueConstraint otherPathConditionRoleValueConstraint;
				if (null != (otherParentPathedRole = CopyMergeUtility.GetEquivalentElement(parentPathedRole, foreignStore, elementTracker)) && null != (otherPathConditionRoleValueConstraint = otherParentPathedRole.ValueConstraint))
				{
					elementTracker.AddEquivalentElement(this, otherPathConditionRoleValueConstraint);
					this.MatchValueRanges(otherPathConditionRoleValueConstraint, elementTracker);
					return true;
				}
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return this.MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // PathConditionRoleValueConstraint Element Equivalence
	#region PathConditionRootValueConstraint Element Equivalence
	partial class PathConditionRootValueConstraint : IElementEquivalence
	{
		/// <summary>Implements <cref name="IElementEquivalence.MapEquivalentElements"/></summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			RolePathObjectTypeRoot parentRolePathObjectTypeRoot;
			if (null != (parentRolePathObjectTypeRoot = this.PathRoot))
			{
				// Embedded through the RolePathRootHasValueConstraint relationship
				RolePathObjectTypeRoot otherParentRolePathObjectTypeRoot;
				PathConditionRootValueConstraint otherPathConditionRootValueConstraint;
				if (null != (otherParentRolePathObjectTypeRoot = CopyMergeUtility.GetEquivalentElement(parentRolePathObjectTypeRoot, foreignStore, elementTracker)) && null != (otherPathConditionRootValueConstraint = otherParentRolePathObjectTypeRoot.ValueConstraint))
				{
					elementTracker.AddEquivalentElement(this, otherPathConditionRootValueConstraint);
					this.MatchValueRanges(otherPathConditionRootValueConstraint, elementTracker);
					return true;
				}
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return this.MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // PathConditionRootValueConstraint Element Equivalence
	#region PathConstant Element Equivalence
	partial class PathConstant : IElementEquivalence
	{
		/// <summary>Implements <cref name="IElementEquivalence.MapEquivalentElements"/></summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			CalculatedPathValueInput parentCalculatedPathValueInput;
			ConstraintRoleProjection parentConstraintRoleProjection;
			DerivedRoleProjection parentDerivedRoleProjection;
			SubqueryParameterInput parentSubqueryParameterInput;
			if (null != (parentCalculatedPathValueInput = this.BoundInput))
			{
				// Embedded through the CalculatedPathValueInputBindsToPathConstant relationship
				CalculatedPathValueInput otherParentCalculatedPathValueInput;
				PathConstant otherPathConstant;
				if (null != (otherParentCalculatedPathValueInput = CopyMergeUtility.GetEquivalentElement(parentCalculatedPathValueInput, foreignStore, elementTracker)) && null != (otherPathConstant = otherParentCalculatedPathValueInput.SourceConstant))
				{
					elementTracker.AddEquivalentElement(this, otherPathConstant);
					return true;
				}
			}
			else if (null != (parentConstraintRoleProjection = this.ConstraintRoleProjection))
			{
				// Embedded through the ConstraintRoleProjectedFromPathConstant relationship
				ConstraintRoleProjection otherParentConstraintRoleProjection;
				PathConstant otherPathConstant;
				if (null != (otherParentConstraintRoleProjection = CopyMergeUtility.GetEquivalentElement(parentConstraintRoleProjection, foreignStore, elementTracker)) && null != (otherPathConstant = otherParentConstraintRoleProjection.ProjectedFromConstant))
				{
					elementTracker.AddEquivalentElement(this, otherPathConstant);
					return true;
				}
			}
			else if (null != (parentDerivedRoleProjection = this.DerivedRoleProjection))
			{
				// Embedded through the DerivedRoleProjectedFromPathConstant relationship
				DerivedRoleProjection otherParentDerivedRoleProjection;
				PathConstant otherPathConstant;
				if (null != (otherParentDerivedRoleProjection = CopyMergeUtility.GetEquivalentElement(parentDerivedRoleProjection, foreignStore, elementTracker)) && null != (otherPathConstant = otherParentDerivedRoleProjection.ProjectedFromConstant))
				{
					elementTracker.AddEquivalentElement(this, otherPathConstant);
					return true;
				}
			}
			else if (null != (parentSubqueryParameterInput = this.SubqueryParameterInput))
			{
				// Embedded through the SubqueryParameterInputFromPathConstant relationship
				SubqueryParameterInput otherParentSubqueryParameterInput;
				PathConstant otherPathConstant;
				if (null != (otherParentSubqueryParameterInput = CopyMergeUtility.GetEquivalentElement(parentSubqueryParameterInput, foreignStore, elementTracker)) && null != (otherPathConstant = otherParentSubqueryParameterInput.InputFromConstant))
				{
					elementTracker.AddEquivalentElement(this, otherPathConstant);
					return true;
				}
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return this.MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // PathConstant Element Equivalence
	#region RoleProjectedDerivationRule Element Equivalence
	partial class RoleProjectedDerivationRule : IElementEquivalence
	{
		/// <summary>Implements <cref name="IElementEquivalence.MapEquivalentElements"/></summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			FactType parentFactType;
			if (null != (parentFactType = this.FactType))
			{
				// Embedded through the FactTypeHasDerivationRule relationship
				FactType otherParentFactType;
				RoleProjectedDerivationRule otherRoleProjectedDerivationRule;
				if (null != (otherParentFactType = CopyMergeUtility.GetEquivalentElement(parentFactType, foreignStore, elementTracker)) && null != (otherRoleProjectedDerivationRule = otherParentFactType.DerivationRule))
				{
					elementTracker.AddEquivalentElement(this, otherRoleProjectedDerivationRule);
					return true;
				}
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return this.MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // RoleProjectedDerivationRule Element Equivalence
	#region RoleValueConstraint Element Equivalence
	partial class RoleValueConstraint : IElementEquivalence
	{
		/// <summary>Implements <cref name="IElementEquivalence.MapEquivalentElements"/></summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			Role parentRole;
			if (null != (parentRole = this.Role))
			{
				// Embedded through the RoleHasValueConstraint relationship
				Role otherParentRole;
				RoleValueConstraint otherRoleValueConstraint;
				if (null != (otherParentRole = CopyMergeUtility.GetEquivalentElement(parentRole, foreignStore, elementTracker)) && null != (otherRoleValueConstraint = otherParentRole.ValueConstraint))
				{
					elementTracker.AddEquivalentElement(this, otherRoleValueConstraint);
					this.MatchValueRanges(otherRoleValueConstraint, elementTracker);
					return true;
				}
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return this.MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // RoleValueConstraint Element Equivalence
	#region SubtypeDerivationRule Element Equivalence
	partial class SubtypeDerivationRule : IElementEquivalence
	{
		/// <summary>Implements <cref name="IElementEquivalence.MapEquivalentElements"/></summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			ObjectType parentObjectType;
			if (null != (parentObjectType = this.Subtype))
			{
				// Embedded through the SubtypeHasDerivationRule relationship
				ObjectType otherParentObjectType;
				SubtypeDerivationRule otherSubtypeDerivationRule;
				if (null != (otherParentObjectType = CopyMergeUtility.GetEquivalentElement(parentObjectType, foreignStore, elementTracker)) && null != (otherSubtypeDerivationRule = otherParentObjectType.DerivationRule))
				{
					elementTracker.AddEquivalentElement(this, otherSubtypeDerivationRule);
					return true;
				}
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return this.MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // SubtypeDerivationRule Element Equivalence
	#region UnaryRoleCardinalityConstraint Element Equivalence
	partial class UnaryRoleCardinalityConstraint : IElementEquivalence
	{
		/// <summary>Implements <cref name="IElementEquivalence.MapEquivalentElements"/></summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			Role parentRole;
			if (null != (parentRole = this.UnaryRole))
			{
				// Embedded through the UnaryRoleHasCardinalityConstraint relationship
				Role otherParentRole;
				UnaryRoleCardinalityConstraint otherUnaryRoleCardinalityConstraint;
				if (null != (otherParentRole = CopyMergeUtility.GetEquivalentElement(parentRole, foreignStore, elementTracker)) && null != (otherUnaryRoleCardinalityConstraint = otherParentRole.Cardinality))
				{
					elementTracker.AddEquivalentElement(this, otherUnaryRoleCardinalityConstraint);
					this.MatchRanges(otherUnaryRoleCardinalityConstraint, elementTracker);
					return true;
				}
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return this.MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // UnaryRoleCardinalityConstraint Element Equivalence
	#region ValueTypeValueConstraint Element Equivalence
	partial class ValueTypeValueConstraint : IElementEquivalence
	{
		/// <summary>Implements <cref name="IElementEquivalence.MapEquivalentElements"/></summary>
		protected bool MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			ObjectType parentObjectType;
			if (null != (parentObjectType = this.ValueType))
			{
				// Embedded through the ValueTypeHasValueConstraint relationship
				ObjectType otherParentObjectType;
				ValueTypeValueConstraint otherValueTypeValueConstraint;
				if (null != (otherParentObjectType = CopyMergeUtility.GetEquivalentElement(parentObjectType, foreignStore, elementTracker)) && null != (otherValueTypeValueConstraint = otherParentObjectType.ValueConstraint))
				{
					elementTracker.AddEquivalentElement(this, otherValueTypeValueConstraint);
					this.MatchValueRanges(otherValueTypeValueConstraint, elementTracker);
					return true;
				}
			}
			return false;
		}
		bool IElementEquivalence.MapEquivalentElements(Store foreignStore, IEquivalentElementTracker elementTracker)
		{
			return this.MapEquivalentElements(foreignStore, elementTracker);
		}
	}
	#endregion // ValueTypeValueConstraint Element Equivalence
	#endregion // One-to-One Embedded Element Equivalence
}
