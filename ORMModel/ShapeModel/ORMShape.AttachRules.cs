using System;
using System.Reflection;

// Common Public License Copyright Notice
// /**************************************************************************\
// * Natural Object-Role Modeling Architect for Visual Studio                 *
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

namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	#region Attach rules to ORMShapeDomainModel model
	partial class ORMShapeDomainModel : ORMSolutions.ORMArchitect.Framework.Shell.IDomainModelEnablesRulesAfterDeserialization
	{
		private static Type[] myCustomDomainModelTypes;
		private static Type[] CustomDomainModelTypes
		{
			get
			{
				Type[] retVal = ORMShapeDomainModel.myCustomDomainModelTypes;
				if (retVal == null)
				{
					// No synchronization is needed here.
					// If accessed concurrently, the worst that will happen is the array of Types being created multiple times.
					// This would have a slightly negative impact on performance, but the result would still be correct.
					// Given the low likelihood of this ever happening, the extra overhead of synchronization would outweigh any possible gain from it.
					retVal = new Type[]{
						typeof(ExternalConstraintLink).GetNestedType("DeleteDanglingConstraintShapeAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExternalConstraintLink).GetNestedType("DeleteDanglingConstraintShapeDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExternalConstraintLink).GetNestedType("DeleteDanglingConstraintShapeRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExternalConstraintShape).GetNestedType("ExclusiveOrCouplerAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExternalConstraintShape).GetNestedType("ExclusiveOrCouplerDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExternalConstraintShape).GetNestedType("PreferredIdentifierAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExternalConstraintShape).GetNestedType("PreferredIdentifierDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExternalConstraintShape).GetNestedType("PreferredIdentifierRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("ConnectionPropertyChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("ExternalConstraintShapeChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("FactTypeShapeChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("ImplicitBooleanValueTypeDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("ObjectificationIsImpliedChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("ObjectificationRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("RoleChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("RoleDisplayOrderChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("SwitchFromNestedFactTypeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("SwitchToNestedFactTypeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FrequencyConstraintShape).GetNestedType("FrequencyConstraintPropertyChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FrequencyConstraintShape).GetNestedType("FrequencyConstraintConversionDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModelNoteShape).GetNestedType("NoteChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("ConnectionPropertyChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("DataTypeAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("DataTypeDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("ObjectTypeChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("ObjectTypeShapeChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("PreferredIdentifierAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("PreferredIdentifierDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("PreferredIdentifierLengthenedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("PreferredIdentifierRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("PreferredIdentifierRolePlayerChangeRuleForResizeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("PreferredIdentifierShortenedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("RolePlayerAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("RolePlayerDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("RolePlayerRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("SubtypeDerivationRuleAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("SubtypeDerivationRuleDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ORMBaseBinaryLinkShape).GetNestedType("LinkChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ORMBaseShape).GetNestedType("AbsoluteBoundsChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ORMBaseShape).GetNestedType("ModelErrorAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ORMBaseShape).GetNestedType("ModelErrorDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ClearInvalidateCacheOnCommittingRuleClass),
						typeof(ConstraintRoleSequenceRoleAddedRuleClass),
						typeof(ConstraintRoleSequenceRoleDeletedRuleClass),
						typeof(ConstraintRoleSequencePositionChangedRuleClass),
						typeof(FactConstraintAddedRuleClass),
						typeof(FactConstraintDeletedRuleClass),
						typeof(ExternalRoleConstraintDeletedRuleClass),
						typeof(FactTypedAddedRuleClass),
						typeof(FactTypeShapeChangedRuleClass),
						typeof(ForceClearViewFixupDataListRuleClass),
						typeof(ModelNoteAddedRuleClass),
						typeof(ModelNoteReferenceAddedRuleClass),
						typeof(ObjectTypedAddedRuleClass),
						typeof(ObjectTypePlaysRoleAddedRuleClass),
						typeof(ObjectTypePlaysRoleRolePlayerChangeRuleClass),
						typeof(ObjectTypeShapeChangeRuleClass),
						typeof(ReadingOrderAddedRuleClass),
						typeof(RoleAddedRuleClass),
						typeof(RoleAddedRuleInlineClass),
						typeof(RoleDeletedRuleClass),
						typeof(RoleValueConstraintAddedRuleClass),
						typeof(SetComparisonConstraintAddedRuleClass),
						typeof(SetConstraintAddedRuleClass),
						typeof(ValueTypeValueConstraintAddedRuleClass),
						typeof(ReadingShape).GetNestedType("DerivationRuleAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("DerivationRuleChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("DerivationRuleDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("DisplayOrientationChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("ReadingAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("ReadingDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("ReadingOrderDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("ReadingPositionChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("ReadingTextChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("RoleDisplayOrderAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("RoleDisplayOrderPositionChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("RolePlayerAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("RolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("RolePlayerDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("RolePlayerRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RingConstraintShape).GetNestedType("RingConstraintPropertyChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraintShape).GetNestedType("ValueConstraintTextChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraintShape).GetNestedType("ValueConstraintShapeDisplayChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic)};
					ORMShapeDomainModel.myCustomDomainModelTypes = retVal;
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
			Type[] customDomainModelTypes = ORMShapeDomainModel.CustomDomainModelTypes;
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
			Type[] disabledRuleTypes = ORMShapeDomainModel.CustomDomainModelTypes;
			for (int i = 0; i < 83; ++i)
			{
				ruleManager.EnableRule(disabledRuleTypes[i]);
			}
		}
		void ORMSolutions.ORMArchitect.Framework.Shell.IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.Store store)
		{
			this.EnableRulesAfterDeserialization(store);
		}
	}
	#endregion // Attach rules to ORMShapeDomainModel model
	#region Auto-rule classes
	#region Rule classes for ExternalConstraintLink
	partial class ExternalConstraintLink
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ExternalConstraintShape), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=(Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority + 1))]
		private sealed class DeleteDanglingConstraintShapeAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DeleteDanglingConstraintShapeAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintLink
			/// /// <summary>
			/// /// AddRule: typeof(ExternalConstraintShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority + 1;
			/// /// </summary>
			/// private static void DeleteDanglingConstraintShapeAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintLink.DeleteDanglingConstraintShapeAddRule");
				ExternalConstraintLink.DeleteDanglingConstraintShapeAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintLink.DeleteDanglingConstraintShapeAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Microsoft.VisualStudio.Modeling.Diagrams.LinkConnectsToNode), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DeleteDanglingConstraintShapeDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DeleteDanglingConstraintShapeDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintLink
			/// /// <summary>
			/// /// DeletingRule: typeof(Microsoft.VisualStudio.Modeling.Diagrams.LinkConnectsToNode)
			/// /// </summary>
			/// private static void DeleteDanglingConstraintShapeDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintLink.DeleteDanglingConstraintShapeDeletingRule");
				ExternalConstraintLink.DeleteDanglingConstraintShapeDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintLink.DeleteDanglingConstraintShapeDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Microsoft.VisualStudio.Modeling.Diagrams.LinkConnectsToNode), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DeleteDanglingConstraintShapeRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DeleteDanglingConstraintShapeRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintLink
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(Microsoft.VisualStudio.Modeling.Diagrams.LinkConnectsToNode)
			/// /// </summary>
			/// private static void DeleteDanglingConstraintShapeRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintLink.DeleteDanglingConstraintShapeRolePlayerChangeRule");
				ExternalConstraintLink.DeleteDanglingConstraintShapeRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintLink.DeleteDanglingConstraintShapeRolePlayerChangeRule");
			}
		}
	}
	#endregion // Rule classes for ExternalConstraintLink
	#region Rule classes for ExternalConstraintShape
	partial class ExternalConstraintShape
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class ExclusiveOrCouplerAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ExclusiveOrCouplerAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintShape
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void ExclusiveOrCouplerAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintShape.ExclusiveOrCouplerAddedRule");
				ExternalConstraintShape.ExclusiveOrCouplerAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintShape.ExclusiveOrCouplerAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class ExclusiveOrCouplerDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ExclusiveOrCouplerDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintShape
			/// /// <summary>
			/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void ExclusiveOrCouplerDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintShape.ExclusiveOrCouplerDeletedRule");
				ExternalConstraintShape.ExclusiveOrCouplerDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintShape.ExclusiveOrCouplerDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class PreferredIdentifierAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintShape
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void PreferredIdentifierAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintShape.PreferredIdentifierAddRule");
				ExternalConstraintShape.PreferredIdentifierAddRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintShape.PreferredIdentifierAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class PreferredIdentifierDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintShape
			/// /// <summary>
			/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void PreferredIdentifierDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintShape.PreferredIdentifierDeleteRule");
				ExternalConstraintShape.PreferredIdentifierDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintShape.PreferredIdentifierDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class PreferredIdentifierRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintShape
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void PreferredIdentifierRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintShape.PreferredIdentifierRolePlayerChangeRule");
				ExternalConstraintShape.PreferredIdentifierRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ExternalConstraintShape.PreferredIdentifierRolePlayerChangeRule");
			}
		}
	}
	#endregion // Rule classes for ExternalConstraintShape
	#region Rule classes for FactTypeShape
	partial class FactTypeShape
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeShape), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class ConnectionPropertyChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConnectionPropertyChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape
			/// /// <summary>
			/// /// ChangeRule: typeof(FactTypeShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void ConnectionPropertyChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape.ConnectionPropertyChangeRule");
				FactTypeShape.ConnectionPropertyChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape.ConnectionPropertyChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ExternalConstraintShape), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class ExternalConstraintShapeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ExternalConstraintShapeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape
			/// /// <summary>
			/// /// ChangeRule: typeof(ExternalConstraintShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void ExternalConstraintShapeChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape.ExternalConstraintShapeChangeRule");
				FactTypeShape.ExternalConstraintShapeChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape.ExternalConstraintShapeChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeShape), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class FactTypeShapeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeShapeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape
			/// /// <summary>
			/// /// ChangeRule: typeof(FactTypeShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void FactTypeShapeChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape.FactTypeShapeChangeRule");
				FactTypeShape.FactTypeShapeChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape.FactTypeShapeChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class ImplicitBooleanValueTypeDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ImplicitBooleanValueTypeDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape
			/// /// <summary>
			/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void ImplicitBooleanValueTypeDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape.ImplicitBooleanValueTypeDeletedRule");
				FactTypeShape.ImplicitBooleanValueTypeDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape.ImplicitBooleanValueTypeDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class ObjectificationIsImpliedChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectificationIsImpliedChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape
			/// /// <summary>
			/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void ObjectificationIsImpliedChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape.ObjectificationIsImpliedChangeRule");
				FactTypeShape.ObjectificationIsImpliedChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape.ObjectificationIsImpliedChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class ObjectificationRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectificationRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void ObjectificationRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape.ObjectificationRolePlayerChangeRule");
				FactTypeShape.ObjectificationRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape.ObjectificationRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Role), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class RoleChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape
			/// /// <summary>
			/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Role), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void RoleChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape.RoleChangedRule");
				FactTypeShape.RoleChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape.RoleChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeShapeHasRoleDisplayOrder), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class RoleDisplayOrderChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleDisplayOrderChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape
			/// /// <summary>
			/// /// RolePlayerPositionChangeRule: typeof(FactTypeShapeHasRoleDisplayOrder), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void RoleDisplayOrderChangedRule(RolePlayerOrderChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerPositionChanged(Microsoft.VisualStudio.Modeling.RolePlayerOrderChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape.RoleDisplayOrderChangedRule");
				FactTypeShape.RoleDisplayOrderChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape.RoleDisplayOrderChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class SwitchFromNestedFactTypeRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SwitchFromNestedFactTypeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape
			/// /// <summary>
			/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void SwitchFromNestedFactTypeRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape.SwitchFromNestedFactTypeRule");
				FactTypeShape.SwitchFromNestedFactTypeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape.SwitchFromNestedFactTypeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class SwitchToNestedFactTypeRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SwitchToNestedFactTypeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void SwitchToNestedFactTypeRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape.SwitchToNestedFactTypeRule");
				FactTypeShape.SwitchToNestedFactTypeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FactTypeShape.SwitchToNestedFactTypeRule");
			}
		}
	}
	#endregion // Rule classes for FactTypeShape
	#region Rule classes for FrequencyConstraintShape
	partial class FrequencyConstraintShape
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FrequencyConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class FrequencyConstraintPropertyChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FrequencyConstraintPropertyChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.FrequencyConstraintShape
			/// /// <summary>
			/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FrequencyConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void FrequencyConstraintPropertyChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FrequencyConstraintShape.FrequencyConstraintPropertyChangeRule");
				FrequencyConstraintShape.FrequencyConstraintPropertyChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FrequencyConstraintShape.FrequencyConstraintPropertyChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FrequencyConstraintShape), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FrequencyConstraintConversionDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FrequencyConstraintConversionDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.FrequencyConstraintShape
			/// /// <summary>
			/// /// DeletingRule: typeof(FrequencyConstraintShape)
			/// /// </summary>
			/// private static void FrequencyConstraintConversionDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FrequencyConstraintShape.FrequencyConstraintConversionDeletingRule");
				FrequencyConstraintShape.FrequencyConstraintConversionDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.FrequencyConstraintShape.FrequencyConstraintConversionDeletingRule");
			}
		}
	}
	#endregion // Rule classes for FrequencyConstraintShape
	#region Rule classes for ModelNoteShape
	partial class ModelNoteShape
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Note), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class NoteChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public NoteChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ModelNoteShape
			/// /// <summary>
			/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Note), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void NoteChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ModelNoteShape.NoteChangeRule");
				ModelNoteShape.NoteChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ModelNoteShape.NoteChangeRule");
			}
		}
	}
	#endregion // Rule classes for ModelNoteShape
	#region Rule classes for ObjectTypeShape
	partial class ObjectTypeShape
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypeShape), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class ConnectionPropertyChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConnectionPropertyChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// ChangeRule: typeof(ObjectTypeShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void ConnectionPropertyChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.ConnectionPropertyChangeRule");
				ObjectTypeShape.ConnectionPropertyChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.ConnectionPropertyChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasDataType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DataTypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DataTypeAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasDataType)
			/// /// </summary>
			/// private static void DataTypeAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.DataTypeAddedRule");
				ObjectTypeShape.DataTypeAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.DataTypeAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasDataType), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class DataTypeDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DataTypeDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasDataType), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void DataTypeDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.DataTypeDeleteRule");
				ObjectTypeShape.DataTypeDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.DataTypeDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class ObjectTypeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectTypeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void ObjectTypeChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.ObjectTypeChangeRule");
				ObjectTypeShape.ObjectTypeChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.ObjectTypeChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypeShape), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class ObjectTypeShapeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectTypeShapeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// ChangeRule: typeof(ObjectTypeShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void ObjectTypeShapeChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.ObjectTypeShapeChangeRule");
				ObjectTypeShape.ObjectTypeShapeChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.ObjectTypeShapeChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class PreferredIdentifierAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier)
			/// /// </summary>
			/// private static void PreferredIdentifierAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.PreferredIdentifierAddedRule");
				ObjectTypeShape.PreferredIdentifierAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.PreferredIdentifierAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class PreferredIdentifierDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void PreferredIdentifierDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.PreferredIdentifierDeleteRule");
				ObjectTypeShape.PreferredIdentifierDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.PreferredIdentifierDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class PreferredIdentifierLengthenedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierLengthenedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void PreferredIdentifierLengthenedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.PreferredIdentifierLengthenedRule");
				ObjectTypeShape.PreferredIdentifierLengthenedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.PreferredIdentifierLengthenedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class PreferredIdentifierRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier)
			/// /// </summary>
			/// private static void PreferredIdentifierRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.PreferredIdentifierRolePlayerChangeRule");
				ObjectTypeShape.PreferredIdentifierRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.PreferredIdentifierRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class PreferredIdentifierRolePlayerChangeRuleForResizeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierRolePlayerChangeRuleForResizeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void PreferredIdentifierRolePlayerChangeRuleForResizeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.PreferredIdentifierRolePlayerChangeRuleForResizeRule");
				ObjectTypeShape.PreferredIdentifierRolePlayerChangeRuleForResizeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.PreferredIdentifierRolePlayerChangeRuleForResizeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class PreferredIdentifierShortenedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PreferredIdentifierShortenedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void PreferredIdentifierShortenedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.PreferredIdentifierShortenedRule");
				ObjectTypeShape.PreferredIdentifierShortenedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.PreferredIdentifierShortenedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void RolePlayerAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.RolePlayerAddedRule");
				ObjectTypeShape.RolePlayerAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.RolePlayerAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class RolePlayerDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void RolePlayerDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.RolePlayerDeleteRule");
				ObjectTypeShape.RolePlayerDeleteRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.RolePlayerDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RolePlayerRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerRolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void RolePlayerRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.RolePlayerRolePlayerChangedRule");
				ObjectTypeShape.RolePlayerRolePlayerChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.RolePlayerRolePlayerChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeHasDerivationRule), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AutoLayoutShapesRulePriority)]
		private sealed class SubtypeDerivationRuleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SubtypeDerivationRuleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeHasDerivationRule), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AutoLayoutShapesRulePriority;
			/// /// </summary>
			/// private static void SubtypeDerivationRuleAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.SubtypeDerivationRuleAddedRule");
				ObjectTypeShape.SubtypeDerivationRuleAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.SubtypeDerivationRuleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeHasDerivationRule), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AutoLayoutShapesRulePriority)]
		private sealed class SubtypeDerivationRuleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SubtypeDerivationRuleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeHasDerivationRule), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AutoLayoutShapesRulePriority;
			/// /// </summary>
			/// private static void SubtypeDerivationRuleDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.SubtypeDerivationRuleDeletedRule");
				ObjectTypeShape.SubtypeDerivationRuleDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ObjectTypeShape.SubtypeDerivationRuleDeletedRule");
			}
		}
	}
	#endregion // Rule classes for ObjectTypeShape
	#region Rule classes for ORMBaseBinaryLinkShape
	partial class ORMBaseBinaryLinkShape
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMBaseBinaryLinkShape), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AutoLayoutShapesRulePriority)]
		private sealed class LinkChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public LinkChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMBaseBinaryLinkShape
			/// /// <summary>
			/// /// ChangeRule: typeof(ORMBaseBinaryLinkShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AutoLayoutShapesRulePriority;
			/// /// </summary>
			/// private static void LinkChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMBaseBinaryLinkShape.LinkChangeRule");
				ORMBaseBinaryLinkShape.LinkChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMBaseBinaryLinkShape.LinkChangeRule");
			}
		}
	}
	#endregion // Rule classes for ORMBaseBinaryLinkShape
	#region Rule classes for ORMBaseShape
	partial class ORMBaseShape
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Microsoft.VisualStudio.Modeling.Diagrams.NodeShape), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class AbsoluteBoundsChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public AbsoluteBoundsChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMBaseShape
			/// /// <summary>
			/// /// ChangeRule: typeof(Microsoft.VisualStudio.Modeling.Diagrams.NodeShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void AbsoluteBoundsChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMBaseShape.AbsoluteBoundsChangedRule");
				ORMBaseShape.AbsoluteBoundsChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMBaseShape.AbsoluteBoundsChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasError), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ModelErrorAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ModelErrorAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMBaseShape
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasError)
			/// /// </summary>
			/// private static void ModelErrorAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMBaseShape.ModelErrorAddedRule");
				ORMBaseShape.ModelErrorAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMBaseShape.ModelErrorAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasError), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ModelErrorDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ModelErrorDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMBaseShape
			/// /// <summary>
			/// /// DeletingRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasError)
			/// /// </summary>
			/// private static void ModelErrorDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMBaseShape.ModelErrorDeletingRule");
				ORMBaseShape.ModelErrorDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMBaseShape.ModelErrorDeletingRule");
			}
		}
	}
	#endregion // Rule classes for ORMBaseShape
	#region Rule classes for ORMShapeDomainModel
	partial class ORMShapeDomainModel
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMShapeDomainModel))]
		private sealed class ClearInvalidateCacheOnCommittingRuleClass : Microsoft.VisualStudio.Modeling.TransactionCommittingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ClearInvalidateCacheOnCommittingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// TransactionCommittingRule: typeof(ORMShapeDomainModel)
			/// /// </summary>
			/// private static void ClearInvalidateCacheOnCommittingRule(TransactionCommitEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void TransactionCommitting(Microsoft.VisualStudio.Modeling.TransactionCommitEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.Transaction.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ClearInvalidateCacheOnCommittingRule");
				ORMShapeDomainModel.ClearInvalidateCacheOnCommittingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.Transaction.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ClearInvalidateCacheOnCommittingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class ConstraintRoleSequenceRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleSequenceRoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void ConstraintRoleSequenceRoleAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ConstraintRoleSequenceRoleAddedRule");
				ORMShapeDomainModel.ConstraintRoleSequenceRoleAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ConstraintRoleSequenceRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class ConstraintRoleSequenceRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleSequenceRoleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void ConstraintRoleSequenceRoleDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ConstraintRoleSequenceRoleDeletedRule");
				ORMShapeDomainModel.ConstraintRoleSequenceRoleDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ConstraintRoleSequenceRoleDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class ConstraintRoleSequencePositionChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConstraintRoleSequencePositionChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// RolePlayerPositionChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void ConstraintRoleSequencePositionChangedRule(RolePlayerOrderChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerPositionChanged(Microsoft.VisualStudio.Modeling.RolePlayerOrderChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ConstraintRoleSequencePositionChangedRule");
				ORMShapeDomainModel.ConstraintRoleSequencePositionChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ConstraintRoleSequencePositionChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class FactConstraintAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactConstraintAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void FactConstraintAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.FactConstraintAddedRule");
				ORMShapeDomainModel.FactConstraintAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.FactConstraintAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class FactConstraintDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactConstraintDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void FactConstraintDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.FactConstraintDeletedRule");
				ORMShapeDomainModel.FactConstraintDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.FactConstraintDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ExternalRoleConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class ExternalRoleConstraintDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ExternalRoleConstraintDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ExternalRoleConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void ExternalRoleConstraintDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ExternalRoleConstraintDeletedRule");
				ORMShapeDomainModel.ExternalRoleConstraintDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ExternalRoleConstraintDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasFactType), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class FactTypedAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypedAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasFactType), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void FactTypedAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.FactTypedAddedRule");
				ORMShapeDomainModel.FactTypedAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.FactTypedAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeShape), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class FactTypeShapeChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeShapeChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// ChangeRule: typeof(FactTypeShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void FactTypeShapeChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.FactTypeShapeChangedRule");
				ORMShapeDomainModel.FactTypeShapeChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.FactTypeShapeChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Microsoft.VisualStudio.Modeling.Diagrams.Diagram), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=(Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority + 1))]
		private sealed partial class ForceClearViewFixupDataListRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ForceClearViewFixupDataListRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// partial class ForceClearViewFixupDataListRuleClass
			/// {
			/// 	/// <summary>
			/// 	/// ChangeRule: typeof(Microsoft.VisualStudio.Modeling.Diagrams.Diagram), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority + 1;
			/// 	/// </summary>
			/// 	private void ForceClearViewFixupDataListRule(ElementPropertyChangedEventArgs e)
			/// 	{
			/// 	}
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ForceClearViewFixupDataListRule");
				this.ForceClearViewFixupDataListRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ForceClearViewFixupDataListRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasModelNote), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class ModelNoteAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ModelNoteAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasModelNote), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void ModelNoteAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ModelNoteAddedRule");
				ORMShapeDomainModel.ModelNoteAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ModelNoteAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelNoteReferencesModelElement), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class ModelNoteReferenceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ModelNoteReferenceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelNoteReferencesModelElement), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void ModelNoteReferenceAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ModelNoteReferenceAddedRule");
				ORMShapeDomainModel.ModelNoteReferenceAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ModelNoteReferenceAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasObjectType), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class ObjectTypedAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectTypedAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasObjectType), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void ObjectTypedAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ObjectTypedAddedRule");
				ORMShapeDomainModel.ObjectTypedAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ObjectTypedAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class ObjectTypePlaysRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectTypePlaysRoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void ObjectTypePlaysRoleAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ObjectTypePlaysRoleAddedRule");
				ORMShapeDomainModel.ObjectTypePlaysRoleAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ObjectTypePlaysRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class ObjectTypePlaysRoleRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectTypePlaysRoleRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void ObjectTypePlaysRoleRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ObjectTypePlaysRoleRolePlayerChangeRule");
				ORMShapeDomainModel.ObjectTypePlaysRoleRolePlayerChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ObjectTypePlaysRoleRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypeShape), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectifiedFactTypeNameShape), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class ObjectTypeShapeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ObjectTypeShapeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// ChangeRule: typeof(ObjectTypeShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// ChangeRule: typeof(ObjectifiedFactTypeNameShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void ObjectTypeShapeChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ObjectTypeShapeChangeRule");
				ORMShapeDomainModel.ObjectTypeShapeChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ObjectTypeShapeChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasReadingOrder), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class ReadingOrderAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReadingOrderAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasReadingOrder), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void ReadingOrderAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ReadingOrderAddedRule");
				ORMShapeDomainModel.ReadingOrderAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ReadingOrderAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class RoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void RoleAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.RoleAddedRule");
				ORMShapeDomainModel.RoleAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.RoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasRole), Priority=(ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.BeforeDelayValidateRulePriority + ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority))]
		private sealed class RoleAddedRuleInlineClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleAddedRuleInlineClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasRole), Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void RoleAddedRuleInline(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.RoleAddedRuleInline");
				ORMShapeDomainModel.RoleAddedRuleInline(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.RoleAddedRuleInline");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class RoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void RoleDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.RoleDeletedRule");
				ORMShapeDomainModel.RoleDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.RoleDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.RoleHasValueConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class RoleValueConstraintAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleValueConstraintAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.RoleHasValueConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void RoleValueConstraintAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.RoleValueConstraintAddedRule");
				ORMShapeDomainModel.RoleValueConstraintAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.RoleValueConstraintAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasSetComparisonConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class SetComparisonConstraintAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SetComparisonConstraintAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasSetComparisonConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void SetComparisonConstraintAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.SetComparisonConstraintAddedRule");
				ORMShapeDomainModel.SetComparisonConstraintAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.SetComparisonConstraintAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasSetConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class SetConstraintAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public SetConstraintAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasSetConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void SetConstraintAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.SetConstraintAddedRule");
				ORMShapeDomainModel.SetConstraintAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.SetConstraintAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasValueConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class ValueTypeValueConstraintAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueTypeValueConstraintAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasValueConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void ValueTypeValueConstraintAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ValueTypeValueConstraintAddedRule");
				ORMShapeDomainModel.ValueTypeValueConstraintAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ORMShapeDomainModel.ValueTypeValueConstraintAddedRule");
			}
		}
	}
	#endregion // Rule classes for ORMShapeDomainModel
	#region Rule classes for ReadingShape
	partial class ReadingShape
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasDerivationRule), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AutoLayoutShapesRulePriority)]
		private sealed class DerivationRuleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DerivationRuleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasDerivationRule), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AutoLayoutShapesRulePriority;
			/// /// </summary>
			/// private static void DerivationRuleAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.DerivationRuleAddedRule");
				ReadingShape.DerivationRuleAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.DerivationRuleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeDerivationRule), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AutoLayoutShapesRulePriority)]
		private sealed class DerivationRuleChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DerivationRuleChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape
			/// /// <summary>
			/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeDerivationRule), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AutoLayoutShapesRulePriority;
			/// /// </summary>
			/// private static void DerivationRuleChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.DerivationRuleChangedRule");
				ReadingShape.DerivationRuleChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.DerivationRuleChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasDerivationRule), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AutoLayoutShapesRulePriority)]
		private sealed class DerivationRuleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DerivationRuleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape
			/// /// <summary>
			/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasDerivationRule), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AutoLayoutShapesRulePriority;
			/// /// </summary>
			/// private static void DerivationRuleDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.DerivationRuleDeletedRule");
				ReadingShape.DerivationRuleDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.DerivationRuleDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeShape), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class DisplayOrientationChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DisplayOrientationChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape
			/// /// <summary>
			/// /// ChangeRule: typeof(FactTypeShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void DisplayOrientationChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.DisplayOrientationChangedRule");
				ReadingShape.DisplayOrientationChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.DisplayOrientationChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrderHasReading), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class ReadingAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReadingAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrderHasReading), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void ReadingAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.ReadingAddedRule");
				ReadingShape.ReadingAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.ReadingAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrderHasReading), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class ReadingDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReadingDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape
			/// /// <summary>
			/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrderHasReading), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void ReadingDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.ReadingDeletedRule");
				ReadingShape.ReadingDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.ReadingDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasReadingOrder), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class ReadingOrderDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReadingOrderDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape
			/// /// <summary>
			/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasReadingOrder), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void ReadingOrderDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.ReadingOrderDeletedRule");
				ReadingShape.ReadingOrderDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.ReadingOrderDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrderHasReading), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class ReadingPositionChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReadingPositionChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape
			/// /// <summary>
			/// /// RolePlayerPositionChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrderHasReading), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void ReadingPositionChangedRule(RolePlayerOrderChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerPositionChanged(Microsoft.VisualStudio.Modeling.RolePlayerOrderChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.ReadingPositionChangedRule");
				ReadingShape.ReadingPositionChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.ReadingPositionChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Reading), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class ReadingTextChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReadingTextChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape
			/// /// <summary>
			/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Reading), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void ReadingTextChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.ReadingTextChangedRule");
				ReadingShape.ReadingTextChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.ReadingTextChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeShapeHasRoleDisplayOrder), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class RoleDisplayOrderAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleDisplayOrderAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape
			/// /// <summary>
			/// /// AddRule: typeof(FactTypeShapeHasRoleDisplayOrder), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void RoleDisplayOrderAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.RoleDisplayOrderAddedRule");
				ReadingShape.RoleDisplayOrderAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.RoleDisplayOrderAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeShapeHasRoleDisplayOrder), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class RoleDisplayOrderPositionChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleDisplayOrderPositionChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape
			/// /// <summary>
			/// /// RolePlayerPositionChangeRule: typeof(FactTypeShapeHasRoleDisplayOrder), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void RoleDisplayOrderPositionChangedRule(RolePlayerOrderChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerPositionChanged(Microsoft.VisualStudio.Modeling.RolePlayerOrderChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.RoleDisplayOrderPositionChangedRule");
				ReadingShape.RoleDisplayOrderPositionChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.RoleDisplayOrderPositionChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class RolePlayerAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void RolePlayerAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.RolePlayerAddedRule");
				ReadingShape.RolePlayerAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.RolePlayerAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class RolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape
			/// /// <summary>
			/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void RolePlayerChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.RolePlayerChangedRule");
				ReadingShape.RolePlayerChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.RolePlayerChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class RolePlayerDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape
			/// /// <summary>
			/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void RolePlayerDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.RolePlayerDeletedRule");
				ReadingShape.RolePlayerDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.RolePlayerDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class RolePlayerRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RolePlayerRolePlayerChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void RolePlayerRolePlayerChangedRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.RolePlayerRolePlayerChangedRule");
				ReadingShape.RolePlayerRolePlayerChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ReadingShape.RolePlayerRolePlayerChangedRule");
			}
		}
	}
	#endregion // Rule classes for ReadingShape
	#region Rule classes for RingConstraintShape
	partial class RingConstraintShape
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.RingConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class RingConstraintPropertyChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RingConstraintPropertyChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.RingConstraintShape
			/// /// <summary>
			/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.RingConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void RingConstraintPropertyChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.RingConstraintShape.RingConstraintPropertyChangeRule");
				RingConstraintShape.RingConstraintPropertyChangeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.RingConstraintShape.RingConstraintPropertyChangeRule");
			}
		}
	}
	#endregion // Rule classes for RingConstraintShape
	#region Rule classes for ValueConstraintShape
	partial class ValueConstraintShape
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class ValueConstraintTextChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueConstraintTextChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ValueConstraintShape
			/// /// <summary>
			/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void ValueConstraintTextChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ValueConstraintShape.ValueConstraintTextChangedRule");
				ValueConstraintShape.ValueConstraintTextChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ValueConstraintShape.ValueConstraintTextChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ValueConstraintShape), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class ValueConstraintShapeDisplayChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ValueConstraintShapeDisplayChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Core.ShapeModel.ValueConstraintShape
			/// /// <summary>
			/// /// ChangeRule: typeof(ValueConstraintShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void ValueConstraintShapeDisplayChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ValueConstraintShape.ValueConstraintShapeDisplayChangedRule");
				ValueConstraintShape.ValueConstraintShapeDisplayChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Core.ShapeModel.ValueConstraintShape.ValueConstraintShapeDisplayChangedRule");
			}
		}
	}
	#endregion // Rule classes for ValueConstraintShape
	#endregion // Auto-rule classes
}
