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

namespace Neumont.Tools.ORM.ShapeModel
{
	#region Attach rules to ORMShapeDomainModel model
	partial class ORMShapeDomainModel : Neumont.Tools.ORM.ObjectModel.IDomainModelEnablesRulesAfterDeserialization
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
						typeof(FactTypeShape).GetNestedType("ConstraintDisplayPositionChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("DerivationAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("DerivationChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("DerivationDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("ExternalConstraintShapeChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("FactTypeShapeChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("ImplicitBooleanValueChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("ObjectificationIsImpliedChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("ObjectificationRolePlayerChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("RoleDisplayOrderChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("SwitchFromNestedFactTypeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("SwitchToNestedFactTypeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FrequencyConstraintShape).GetNestedType("FrequencyConstraintPropertyChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModelNoteShape).GetNestedType("NoteChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
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
						typeof(ORMBaseBinaryLinkShape).GetNestedType("LinkChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ORMBaseShape).GetNestedType("AbsoluteBoundsChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ORMBaseShape).GetNestedType("ModelErrorAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ORMBaseShape).GetNestedType("ModelErrorDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequenceRoleAddedRuleClass),
						typeof(ConstraintRoleSequenceRoleDeletedRuleClass),
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
						typeof(RoleChangedRuleClass),
						typeof(RoleDeletedRuleClass),
						typeof(RoleValueConstraintAddedRuleClass),
						typeof(SetComparisonConstraintAddedRuleClass),
						typeof(SetConstraintAddedRuleClass),
						typeof(ValueTypeValueConstraintAddedRuleClass),
						typeof(ReadingShape).GetNestedType("DisplayOrientationChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("ReadingAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("ReadingOrderDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("ReadingPositionChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("ReadingTextChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("RoleDisplayOrderAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("RoleDisplayOrderPositionChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RingConstraintShape).GetNestedType("RingConstraintPropertyChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraintShape).GetNestedType("ValueConstraintTextChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic)};
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
			if (Neumont.Tools.Modeling.FrameworkDomainModel.InitializingToolboxItems)
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
			for (int i = 0; i < 69; ++i)
			{
				ruleManager.EnableRule(disabledRuleTypes[i]);
			}
		}
		void Neumont.Tools.ORM.ObjectModel.IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.Store store)
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
			public DeleteDanglingConstraintShapeAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ExternalConstraintLink
			/// /// <summary>
			/// /// AddRule: typeof(ExternalConstraintShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority + 1;
			/// /// </summary>
			/// private static void DeleteDanglingConstraintShapeAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ExternalConstraintLink.DeleteDanglingConstraintShapeAddRule");
				ExternalConstraintLink.DeleteDanglingConstraintShapeAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ExternalConstraintLink.DeleteDanglingConstraintShapeAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Microsoft.VisualStudio.Modeling.Diagrams.LinkConnectsToNode))]
		private sealed class DeleteDanglingConstraintShapeDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			public DeleteDanglingConstraintShapeDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ExternalConstraintLink
			/// /// <summary>
			/// /// DeletingRule: typeof(Microsoft.VisualStudio.Modeling.Diagrams.LinkConnectsToNode)
			/// /// </summary>
			/// private static void DeleteDanglingConstraintShapeDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ExternalConstraintLink.DeleteDanglingConstraintShapeDeletingRule");
				ExternalConstraintLink.DeleteDanglingConstraintShapeDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ExternalConstraintLink.DeleteDanglingConstraintShapeDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Microsoft.VisualStudio.Modeling.Diagrams.LinkConnectsToNode))]
		private sealed class DeleteDanglingConstraintShapeRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			public DeleteDanglingConstraintShapeRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ExternalConstraintLink
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(Microsoft.VisualStudio.Modeling.Diagrams.LinkConnectsToNode)
			/// /// </summary>
			/// private static void DeleteDanglingConstraintShapeRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ShapeModel.ExternalConstraintLink.DeleteDanglingConstraintShapeRolePlayerChangeRule");
				ExternalConstraintLink.DeleteDanglingConstraintShapeRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ShapeModel.ExternalConstraintLink.DeleteDanglingConstraintShapeRolePlayerChangeRule");
			}
		}
	}
	#endregion // Rule classes for ExternalConstraintLink
	#region Rule classes for ExternalConstraintShape
	partial class ExternalConstraintShape
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class ExclusiveOrCouplerAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public ExclusiveOrCouplerAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ExternalConstraintShape
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void ExclusiveOrCouplerAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ExternalConstraintShape.ExclusiveOrCouplerAddedRule");
				ExternalConstraintShape.ExclusiveOrCouplerAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ExternalConstraintShape.ExclusiveOrCouplerAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class ExclusiveOrCouplerDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			public ExclusiveOrCouplerDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ExternalConstraintShape
			/// /// <summary>
			/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void ExclusiveOrCouplerDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ExternalConstraintShape.ExclusiveOrCouplerDeletedRule");
				ExternalConstraintShape.ExclusiveOrCouplerDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ExternalConstraintShape.ExclusiveOrCouplerDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class PreferredIdentifierAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public PreferredIdentifierAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ExternalConstraintShape
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void PreferredIdentifierAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ExternalConstraintShape.PreferredIdentifierAddRule");
				ExternalConstraintShape.PreferredIdentifierAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ExternalConstraintShape.PreferredIdentifierAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class PreferredIdentifierDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			public PreferredIdentifierDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ExternalConstraintShape
			/// /// <summary>
			/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void PreferredIdentifierDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ExternalConstraintShape.PreferredIdentifierDeleteRule");
				ExternalConstraintShape.PreferredIdentifierDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ExternalConstraintShape.PreferredIdentifierDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class PreferredIdentifierRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			public PreferredIdentifierRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ExternalConstraintShape
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void PreferredIdentifierRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ShapeModel.ExternalConstraintShape.PreferredIdentifierRolePlayerChangeRule");
				ExternalConstraintShape.PreferredIdentifierRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ShapeModel.ExternalConstraintShape.PreferredIdentifierRolePlayerChangeRule");
			}
		}
	}
	#endregion // Rule classes for ExternalConstraintShape
	#region Rule classes for FactTypeShape
	partial class FactTypeShape
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeShape), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class ConstraintDisplayPositionChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			public ConstraintDisplayPositionChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.FactTypeShape
			/// /// <summary>
			/// /// ChangeRule: typeof(FactTypeShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void ConstraintDisplayPositionChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.ConstraintDisplayPositionChangeRule");
				FactTypeShape.ConstraintDisplayPositionChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.ConstraintDisplayPositionChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasDerivationExpression), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AutoLayoutShapesRulePriority)]
		private sealed class DerivationAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public DerivationAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.FactTypeShape
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasDerivationExpression), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AutoLayoutShapesRulePriority;
			/// /// </summary>
			/// private static void DerivationAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.DerivationAddedRule");
				FactTypeShape.DerivationAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.DerivationAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasDerivationExpression), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AutoLayoutShapesRulePriority)]
		private sealed class DerivationChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			public DerivationChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.FactTypeShape
			/// /// <summary>
			/// /// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasDerivationExpression), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AutoLayoutShapesRulePriority;
			/// /// </summary>
			/// private static void DerivationChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.DerivationChangedRule");
				FactTypeShape.DerivationChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.DerivationChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasDerivationExpression), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AutoLayoutShapesRulePriority)]
		private sealed class DerivationDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			public DerivationDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.FactTypeShape
			/// /// <summary>
			/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasDerivationExpression), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AutoLayoutShapesRulePriority;
			/// /// </summary>
			/// private static void DerivationDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.DerivationDeletedRule");
				FactTypeShape.DerivationDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.DerivationDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ExternalConstraintShape), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class ExternalConstraintShapeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			public ExternalConstraintShapeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.FactTypeShape
			/// /// <summary>
			/// /// ChangeRule: typeof(ExternalConstraintShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void ExternalConstraintShapeChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.ExternalConstraintShapeChangeRule");
				FactTypeShape.ExternalConstraintShapeChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.ExternalConstraintShapeChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeShape), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class FactTypeShapeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			public FactTypeShapeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.FactTypeShape
			/// /// <summary>
			/// /// ChangeRule: typeof(FactTypeShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void FactTypeShapeChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.FactTypeShapeChangeRule");
				FactTypeShape.FactTypeShapeChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.FactTypeShapeChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectType))]
		private sealed class ImplicitBooleanValueChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			public ImplicitBooleanValueChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.FactTypeShape
			/// /// <summary>
			/// /// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectType)
			/// /// </summary>
			/// private static void ImplicitBooleanValueChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.ImplicitBooleanValueChangeRule");
				FactTypeShape.ImplicitBooleanValueChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.ImplicitBooleanValueChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.Objectification), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class ObjectificationIsImpliedChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			public ObjectificationIsImpliedChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.FactTypeShape
			/// /// <summary>
			/// /// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.Objectification), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void ObjectificationIsImpliedChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.ObjectificationIsImpliedChangeRule");
				FactTypeShape.ObjectificationIsImpliedChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.ObjectificationIsImpliedChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.Objectification), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class ObjectificationRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			public ObjectificationRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.FactTypeShape
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.Objectification), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void ObjectificationRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.ObjectificationRolePlayerChangeRule");
				FactTypeShape.ObjectificationRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.ObjectificationRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeShapeHasRoleDisplayOrder), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class RoleDisplayOrderChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
		{
			public RoleDisplayOrderChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.FactTypeShape
			/// /// <summary>
			/// /// RolePlayerPositionChangeRule: typeof(FactTypeShapeHasRoleDisplayOrder), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void RoleDisplayOrderChangedRule(RolePlayerOrderChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void RolePlayerPositionChanged(Microsoft.VisualStudio.Modeling.RolePlayerOrderChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.RoleDisplayOrderChangedRule");
				FactTypeShape.RoleDisplayOrderChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.RoleDisplayOrderChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.Objectification), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class SwitchFromNestedFactTypeRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			public SwitchFromNestedFactTypeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.FactTypeShape
			/// /// <summary>
			/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.Objectification), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void SwitchFromNestedFactTypeRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.SwitchFromNestedFactTypeRule");
				FactTypeShape.SwitchFromNestedFactTypeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.SwitchFromNestedFactTypeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.Objectification), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class SwitchToNestedFactTypeRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public SwitchToNestedFactTypeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.FactTypeShape
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.Objectification), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void SwitchToNestedFactTypeRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.SwitchToNestedFactTypeRule");
				FactTypeShape.SwitchToNestedFactTypeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.FactTypeShape.SwitchToNestedFactTypeRule");
			}
		}
	}
	#endregion // Rule classes for FactTypeShape
	#region Rule classes for FrequencyConstraintShape
	partial class FrequencyConstraintShape
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FrequencyConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class FrequencyConstraintPropertyChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			public FrequencyConstraintPropertyChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.FrequencyConstraintShape
			/// /// <summary>
			/// /// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.FrequencyConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void FrequencyConstraintPropertyChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.FrequencyConstraintShape.FrequencyConstraintPropertyChangeRule");
				FrequencyConstraintShape.FrequencyConstraintPropertyChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.FrequencyConstraintShape.FrequencyConstraintPropertyChangeRule");
			}
		}
	}
	#endregion // Rule classes for FrequencyConstraintShape
	#region Rule classes for ModelNoteShape
	partial class ModelNoteShape
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.Note), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class NoteChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			public NoteChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ModelNoteShape
			/// /// <summary>
			/// /// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.Note), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void NoteChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ModelNoteShape.NoteChangeRule");
				ModelNoteShape.NoteChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ModelNoteShape.NoteChangeRule");
			}
		}
	}
	#endregion // Rule classes for ModelNoteShape
	#region Rule classes for ObjectTypeShape
	partial class ObjectTypeShape
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ValueTypeHasDataType))]
		private sealed class DataTypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public DataTypeAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ValueTypeHasDataType)
			/// /// </summary>
			/// private static void DataTypeAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.DataTypeAddedRule");
				ObjectTypeShape.DataTypeAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.DataTypeAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ValueTypeHasDataType), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class DataTypeDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			public DataTypeDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ValueTypeHasDataType), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void DataTypeDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.DataTypeDeleteRule");
				ObjectTypeShape.DataTypeDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.DataTypeDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectType), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class ObjectTypeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			public ObjectTypeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectType), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void ObjectTypeChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.ObjectTypeChangeRule");
				ObjectTypeShape.ObjectTypeChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.ObjectTypeChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypeShape), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class ObjectTypeShapeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			public ObjectTypeShapeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// ChangeRule: typeof(ObjectTypeShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void ObjectTypeShapeChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.ObjectTypeShapeChangeRule");
				ObjectTypeShape.ObjectTypeShapeChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.ObjectTypeShapeChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier))]
		private sealed class PreferredIdentifierAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public PreferredIdentifierAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier)
			/// /// </summary>
			/// private static void PreferredIdentifierAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.PreferredIdentifierAddedRule");
				ObjectTypeShape.PreferredIdentifierAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.PreferredIdentifierAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class PreferredIdentifierDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			public PreferredIdentifierDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void PreferredIdentifierDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.PreferredIdentifierDeleteRule");
				ObjectTypeShape.PreferredIdentifierDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.PreferredIdentifierDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class PreferredIdentifierLengthenedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public PreferredIdentifierLengthenedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void PreferredIdentifierLengthenedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.PreferredIdentifierLengthenedRule");
				ObjectTypeShape.PreferredIdentifierLengthenedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.PreferredIdentifierLengthenedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier))]
		private sealed class PreferredIdentifierRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			public PreferredIdentifierRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier)
			/// /// </summary>
			/// private static void PreferredIdentifierRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.PreferredIdentifierRolePlayerChangeRule");
				ObjectTypeShape.PreferredIdentifierRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.PreferredIdentifierRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class PreferredIdentifierRolePlayerChangeRuleForResizeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			public PreferredIdentifierRolePlayerChangeRuleForResizeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void PreferredIdentifierRolePlayerChangeRuleForResizeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.PreferredIdentifierRolePlayerChangeRuleForResizeRule");
				ObjectTypeShape.PreferredIdentifierRolePlayerChangeRuleForResizeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.PreferredIdentifierRolePlayerChangeRuleForResizeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class PreferredIdentifierShortenedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			public PreferredIdentifierShortenedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void PreferredIdentifierShortenedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.PreferredIdentifierShortenedRule");
				ObjectTypeShape.PreferredIdentifierShortenedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.PreferredIdentifierShortenedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole))]
		private sealed class RolePlayerAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public RolePlayerAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole)
			/// /// </summary>
			/// private static void RolePlayerAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.RolePlayerAddedRule");
				ObjectTypeShape.RolePlayerAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.RolePlayerAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class RolePlayerDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			public RolePlayerDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ObjectTypeShape
			/// /// <summary>
			/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void RolePlayerDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.RolePlayerDeleteRule");
				ObjectTypeShape.RolePlayerDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ObjectTypeShape.RolePlayerDeleteRule");
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
			public LinkChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMBaseBinaryLinkShape
			/// /// <summary>
			/// /// ChangeRule: typeof(ORMBaseBinaryLinkShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AutoLayoutShapesRulePriority;
			/// /// </summary>
			/// private static void LinkChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMBaseBinaryLinkShape.LinkChangeRule");
				ORMBaseBinaryLinkShape.LinkChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMBaseBinaryLinkShape.LinkChangeRule");
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
			public AbsoluteBoundsChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMBaseShape
			/// /// <summary>
			/// /// ChangeRule: typeof(Microsoft.VisualStudio.Modeling.Diagrams.NodeShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void AbsoluteBoundsChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMBaseShape.AbsoluteBoundsChangedRule");
				ORMBaseShape.AbsoluteBoundsChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMBaseShape.AbsoluteBoundsChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ModelHasError))]
		private sealed class ModelErrorAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public ModelErrorAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMBaseShape
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ModelHasError)
			/// /// </summary>
			/// private static void ModelErrorAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMBaseShape.ModelErrorAddedRule");
				ORMBaseShape.ModelErrorAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMBaseShape.ModelErrorAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ModelHasError))]
		private sealed class ModelErrorDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			public ModelErrorDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMBaseShape
			/// /// <summary>
			/// /// DeletingRule: typeof(Neumont.Tools.ORM.ObjectModel.ModelHasError)
			/// /// </summary>
			/// private static void ModelErrorDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMBaseShape.ModelErrorDeletingRule");
				ORMBaseShape.ModelErrorDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMBaseShape.ModelErrorDeletingRule");
			}
		}
	}
	#endregion // Rule classes for ORMBaseShape
	#region Rule classes for ORMShapeDomainModel
	partial class ORMShapeDomainModel
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class ConstraintRoleSequenceRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public ConstraintRoleSequenceRoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void ConstraintRoleSequenceRoleAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ConstraintRoleSequenceRoleAddedRule");
				ORMShapeDomainModel.ConstraintRoleSequenceRoleAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ConstraintRoleSequenceRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class ConstraintRoleSequenceRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			public ConstraintRoleSequenceRoleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void ConstraintRoleSequenceRoleDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ConstraintRoleSequenceRoleDeletedRule");
				ORMShapeDomainModel.ConstraintRoleSequenceRoleDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ConstraintRoleSequenceRoleDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FactConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class FactConstraintAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public FactConstraintAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.FactConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void FactConstraintAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.FactConstraintAddedRule");
				ORMShapeDomainModel.FactConstraintAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.FactConstraintAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FactConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class FactConstraintDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			public FactConstraintDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.FactConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void FactConstraintDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.FactConstraintDeletedRule");
				ORMShapeDomainModel.FactConstraintDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.FactConstraintDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ExternalRoleConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class ExternalRoleConstraintDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			public ExternalRoleConstraintDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ExternalRoleConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void ExternalRoleConstraintDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ExternalRoleConstraintDeletedRule");
				ORMShapeDomainModel.ExternalRoleConstraintDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ExternalRoleConstraintDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ModelHasFactType), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class FactTypedAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public FactTypedAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ModelHasFactType), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void FactTypedAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.FactTypedAddedRule");
				ORMShapeDomainModel.FactTypedAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.FactTypedAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeShape), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class FactTypeShapeChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			public FactTypeShapeChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// ChangeRule: typeof(FactTypeShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void FactTypeShapeChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.FactTypeShapeChangedRule");
				ORMShapeDomainModel.FactTypeShapeChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.FactTypeShapeChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Microsoft.VisualStudio.Modeling.Diagrams.Diagram), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=(Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority + 1))]
		private sealed partial class ForceClearViewFixupDataListRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			public ForceClearViewFixupDataListRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel
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
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ForceClearViewFixupDataListRule");
				this.ForceClearViewFixupDataListRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ForceClearViewFixupDataListRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ModelHasModelNote), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class ModelNoteAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public ModelNoteAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ModelHasModelNote), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void ModelNoteAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ModelNoteAddedRule");
				ORMShapeDomainModel.ModelNoteAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ModelNoteAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ModelNoteReferencesModelElement), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class ModelNoteReferenceAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public ModelNoteReferenceAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ModelNoteReferencesModelElement), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void ModelNoteReferenceAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ModelNoteReferenceAddedRule");
				ORMShapeDomainModel.ModelNoteReferenceAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ModelNoteReferenceAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ModelHasObjectType), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class ObjectTypedAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public ObjectTypedAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ModelHasObjectType), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void ObjectTypedAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ObjectTypedAddedRule");
				ORMShapeDomainModel.ObjectTypedAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ObjectTypedAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class ObjectTypePlaysRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public ObjectTypePlaysRoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void ObjectTypePlaysRoleAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ObjectTypePlaysRoleAddedRule");
				ORMShapeDomainModel.ObjectTypePlaysRoleAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ObjectTypePlaysRoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class ObjectTypePlaysRoleRolePlayerChangeRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			public ObjectTypePlaysRoleRolePlayerChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void ObjectTypePlaysRoleRolePlayerChangeRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ObjectTypePlaysRoleRolePlayerChangeRule");
				ORMShapeDomainModel.ObjectTypePlaysRoleRolePlayerChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ObjectTypePlaysRoleRolePlayerChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectTypeShape), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ObjectifiedFactTypeNameShape), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class ObjectTypeShapeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			public ObjectTypeShapeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// ChangeRule: typeof(ObjectTypeShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// ChangeRule: typeof(ObjectifiedFactTypeNameShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void ObjectTypeShapeChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ObjectTypeShapeChangeRule");
				ORMShapeDomainModel.ObjectTypeShapeChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ObjectTypeShapeChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasReadingOrder), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class ReadingOrderAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public ReadingOrderAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasReadingOrder), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void ReadingOrderAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ReadingOrderAddedRule");
				ORMShapeDomainModel.ReadingOrderAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ReadingOrderAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class RoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public RoleAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void RoleAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.RoleAddedRule");
				ORMShapeDomainModel.RoleAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.RoleAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.Role), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class RoleChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			public RoleChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.Role), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void RoleChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.RoleChangedRule");
				ORMShapeDomainModel.RoleChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.RoleChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasRole), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class RoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			public RoleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasRole), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void RoleDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.RoleDeletedRule");
				ORMShapeDomainModel.RoleDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.RoleDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.RoleHasValueConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class RoleValueConstraintAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public RoleValueConstraintAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.RoleHasValueConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void RoleValueConstraintAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.RoleValueConstraintAddedRule");
				ORMShapeDomainModel.RoleValueConstraintAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.RoleValueConstraintAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ModelHasSetConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class SetComparisonConstraintAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public SetComparisonConstraintAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ModelHasSetConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void SetComparisonConstraintAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.SetComparisonConstraintAddedRule");
				ORMShapeDomainModel.SetComparisonConstraintAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.SetComparisonConstraintAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ModelHasSetComparisonConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class SetConstraintAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public SetConstraintAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ModelHasSetComparisonConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void SetConstraintAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.SetConstraintAddedRule");
				ORMShapeDomainModel.SetConstraintAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.SetConstraintAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ValueTypeHasValueConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class ValueTypeValueConstraintAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public ValueTypeValueConstraintAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ValueTypeHasValueConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void ValueTypeValueConstraintAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ValueTypeValueConstraintAddedRule");
				ORMShapeDomainModel.ValueTypeValueConstraintAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ORMShapeDomainModel.ValueTypeValueConstraintAddedRule");
			}
		}
	}
	#endregion // Rule classes for ORMShapeDomainModel
	#region Rule classes for ReadingShape
	partial class ReadingShape
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeShape), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class DisplayOrientationChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			public DisplayOrientationChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ReadingShape
			/// /// <summary>
			/// /// ChangeRule: typeof(FactTypeShape), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void DisplayOrientationChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ReadingShape.DisplayOrientationChangedRule");
				ReadingShape.DisplayOrientationChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ReadingShape.DisplayOrientationChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ReadingOrderHasReading), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class ReadingAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public ReadingAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ReadingShape
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ReadingOrderHasReading), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void ReadingAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ReadingShape.ReadingAddedRule");
				ReadingShape.ReadingAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ReadingShape.ReadingAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasReadingOrder), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class ReadingOrderDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			public ReadingOrderDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ReadingShape
			/// /// <summary>
			/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasReadingOrder), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void ReadingOrderDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ReadingShape.ReadingOrderDeletedRule");
				ReadingShape.ReadingOrderDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ReadingShape.ReadingOrderDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ReadingOrderHasReading), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class ReadingPositionChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
		{
			public ReadingPositionChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ReadingShape
			/// /// <summary>
			/// /// RolePlayerPositionChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.ReadingOrderHasReading), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void ReadingPositionChangedRule(RolePlayerOrderChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void RolePlayerPositionChanged(Microsoft.VisualStudio.Modeling.RolePlayerOrderChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "Neumont.Tools.ORM.ShapeModel.ReadingShape.ReadingPositionChangedRule");
				ReadingShape.ReadingPositionChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "Neumont.Tools.ORM.ShapeModel.ReadingShape.ReadingPositionChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.Reading), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddShapeRulePriority)]
		private sealed class ReadingTextChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			public ReadingTextChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ReadingShape
			/// /// <summary>
			/// /// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.Reading), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddShapeRulePriority;
			/// /// </summary>
			/// private static void ReadingTextChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ReadingShape.ReadingTextChangedRule");
				ReadingShape.ReadingTextChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ReadingShape.ReadingTextChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeShapeHasRoleDisplayOrder), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class RoleDisplayOrderAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public RoleDisplayOrderAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ReadingShape
			/// /// <summary>
			/// /// AddRule: typeof(FactTypeShapeHasRoleDisplayOrder), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void RoleDisplayOrderAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ReadingShape.RoleDisplayOrderAddedRule");
				ReadingShape.RoleDisplayOrderAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ReadingShape.RoleDisplayOrderAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(FactTypeShapeHasRoleDisplayOrder), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class RoleDisplayOrderPositionChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
		{
			public RoleDisplayOrderPositionChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ReadingShape
			/// /// <summary>
			/// /// RolePlayerPositionChangeRule: typeof(FactTypeShapeHasRoleDisplayOrder), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void RoleDisplayOrderPositionChangedRule(RolePlayerOrderChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void RolePlayerPositionChanged(Microsoft.VisualStudio.Modeling.RolePlayerOrderChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "Neumont.Tools.ORM.ShapeModel.ReadingShape.RoleDisplayOrderPositionChangedRule");
				ReadingShape.RoleDisplayOrderPositionChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "Neumont.Tools.ORM.ShapeModel.ReadingShape.RoleDisplayOrderPositionChangedRule");
			}
		}
	}
	#endregion // Rule classes for ReadingShape
	#region Rule classes for RingConstraintShape
	partial class RingConstraintShape
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.RingConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class RingConstraintPropertyChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			public RingConstraintPropertyChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.RingConstraintShape
			/// /// <summary>
			/// /// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.RingConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void RingConstraintPropertyChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.RingConstraintShape.RingConstraintPropertyChangeRule");
				RingConstraintShape.RingConstraintPropertyChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.RingConstraintShape.RingConstraintPropertyChangeRule");
			}
		}
	}
	#endregion // Rule classes for RingConstraintShape
	#region Rule classes for ValueConstraintShape
	partial class ValueConstraintShape
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ValueConstraint), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.ResizeParentRulePriority)]
		private sealed class ValueConstraintTextChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			public ValueConstraintTextChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ShapeModel.ValueConstraintShape
			/// /// <summary>
			/// /// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.ValueConstraint), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.ResizeParentRulePriority;
			/// /// </summary>
			/// private static void ValueConstraintTextChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ValueConstraintShape.ValueConstraintTextChangedRule");
				ValueConstraintShape.ValueConstraintTextChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ShapeModel.ValueConstraintShape.ValueConstraintTextChangedRule");
			}
		}
	}
	#endregion // Rule classes for ValueConstraintShape
	#endregion // Auto-rule classes
}
