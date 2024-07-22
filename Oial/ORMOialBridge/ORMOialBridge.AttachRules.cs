﻿using System;
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

namespace ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge
{
	#region Attach rules to ORMToORMAbstractionBridgeDomainModel model
	partial class ORMToORMAbstractionBridgeDomainModel : ORMSolutions.ORMArchitect.Framework.Shell.IDomainModelEnablesRulesAfterDeserialization
	{
		private static Type[] myCustomDomainModelTypes;
		private static Type[] CustomDomainModelTypes
		{
			get
			{
				Type[] retVal = ORMToORMAbstractionBridgeDomainModel.myCustomDomainModelTypes;
				if (retVal == null)
				{
					// No synchronization is needed here.
					// If accessed concurrently, the worst that will happen is the array of Types being created multiple times.
					// This would have a slightly negative impact on performance, but the result would still be correct.
					// Given the low likelihood of this ever happening, the extra overhead of synchronization would outweigh any possible gain from it.
					retVal = new Type[]{
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ElementExclusionAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("FactTypeAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("FactTypeDerivationExpressionAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("FactTypeDerivationExpressionChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("FactTypeDerivationExpressionDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("FactTypeDerivationRuleAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("FactTypeDerivationRuleChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("FactTypeDerivationRuleDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("FactTypeErrorAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("FactTypeErrorDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ObjectTypeAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ObjectTypeErrorAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ObjectTypeErrorDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ObjectificationAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ObjectificationDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ObjectificationRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("PreferredIdentifierAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("RolePlayerAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("RolePlayerDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("RolePlayerRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ConceptTypeBridgeDetachedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("InformationTypeFormatBridgeDetachedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ConceptTypeChildPathBridgeDetachedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ConstraintRoleAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ConstraintRoleDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("DataTypeAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("DataTypeDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("DataTypeChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("DataTypeRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("FactTypeChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ObjectTypeChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ORMModelChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("PreferredIdentifierDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("PreferredIdentifierRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ReadingOrderAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ReadingOrderDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ReadingOrderReorderedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ReadingAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ReadingChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ReadingDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ReadingReorderedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("RoleChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("RolePlayerRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("SetConstraintChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("SubtypeFactChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("UniquenessConstraintRoleOrderChangedClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("UniquenessConstraintRoleDeletingClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("UniquenessBridgeDetachedRuleClass", BindingFlags.Public | BindingFlags.NonPublic)};
					ORMToORMAbstractionBridgeDomainModel.myCustomDomainModelTypes = retVal;
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
			Type[] customDomainModelTypes = ORMToORMAbstractionBridgeDomainModel.CustomDomainModelTypes;
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
			Type[] disabledRuleTypes = ORMToORMAbstractionBridgeDomainModel.CustomDomainModelTypes;
			for (int i = 0; i < 48; ++i)
			{
				ruleManager.EnableRule(disabledRuleTypes[i]);
			}
		}
		void ORMSolutions.ORMArchitect.Framework.Shell.IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.Store store)
		{
			this.EnableRulesAfterDeserialization(store);
		}
	}
	#endregion // Attach rules to ORMToORMAbstractionBridgeDomainModel model
	#region Auto-rule classes
	#region Rule classes for AbstractionModelIsForORMModel.ORMElementGateway
	partial class AbstractionModelIsForORMModel
	{
		partial class ORMElementGateway
		{
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ExcludedORMModelElement), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ElementExclusionAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ElementExclusionAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// AddRule: typeof(ExcludedORMModelElement)
				/// /// </summary>
				/// private static void ElementExclusionAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.ElementExclusionAddedRule");
					ORMElementGateway.ElementExclusionAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.ElementExclusionAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasFactType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasFactType)
				/// /// </summary>
				/// private static void FactTypeAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeAddedRule");
					ORMElementGateway.FactTypeAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasDerivationExpression), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeDerivationExpressionAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeDerivationExpressionAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasDerivationExpression)
				/// /// </summary>
				/// private static void FactTypeDerivationExpressionAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeDerivationExpressionAddedRule");
					ORMElementGateway.FactTypeDerivationExpressionAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeDerivationExpressionAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeDerivationExpression), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeDerivationExpressionChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeDerivationExpressionChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeDerivationExpression)
				/// /// </summary>
				/// private static void FactTypeDerivationExpressionChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeDerivationExpressionChangedRule");
					ORMElementGateway.FactTypeDerivationExpressionChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeDerivationExpressionChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasDerivationExpression), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeDerivationExpressionDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeDerivationExpressionDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasDerivationExpression)
				/// /// </summary>
				/// private static void FactTypeDerivationExpressionDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeDerivationExpressionDeletedRule");
					ORMElementGateway.FactTypeDerivationExpressionDeletedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeDerivationExpressionDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasDerivationRule), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeDerivationRuleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeDerivationRuleAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasDerivationRule)
				/// /// </summary>
				/// private static void FactTypeDerivationRuleAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeDerivationRuleAddedRule");
					ORMElementGateway.FactTypeDerivationRuleAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeDerivationRuleAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeDerivationRule), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeDerivationRuleChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeDerivationRuleChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeDerivationRule)
				/// /// </summary>
				/// private static void FactTypeDerivationRuleChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeDerivationRuleChangedRule");
					ORMElementGateway.FactTypeDerivationRuleChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeDerivationRuleChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasDerivationRule), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeDerivationRuleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeDerivationRuleDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasDerivationRule)
				/// /// </summary>
				/// private static void FactTypeDerivationRuleDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeDerivationRuleDeletedRule");
					ORMElementGateway.FactTypeDerivationRuleDeletedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeDerivationRuleDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasImpliedInternalUniquenessConstraintError), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasFactTypeRequiresInternalUniquenessConstraintError), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeErrorAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeErrorAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasImpliedInternalUniquenessConstraintError)
				/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasFactTypeRequiresInternalUniquenessConstraintError)
				/// /// </summary>
				/// private static void FactTypeErrorAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeErrorAddedRule");
					ORMElementGateway.FactTypeErrorAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeErrorAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasImpliedInternalUniquenessConstraintError), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasFactTypeRequiresInternalUniquenessConstraintError), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeErrorDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeErrorDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasImpliedInternalUniquenessConstraintError)
				/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasFactTypeRequiresInternalUniquenessConstraintError)
				/// /// </summary>
				/// private static void FactTypeErrorDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeErrorDeletedRule");
					ORMElementGateway.FactTypeErrorDeletedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeErrorDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasObjectType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ObjectTypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ObjectTypeAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelHasObjectType)
				/// /// </summary>
				/// private static void ObjectTypeAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.ObjectTypeAddedRule");
					ORMElementGateway.ObjectTypeAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.ObjectTypeAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasEntityTypeRequiresReferenceSchemeError), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasCompatibleSupertypesError), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasPreferredIdentifierRequiresMandatoryError), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasUnspecifiedDataTypeError), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ObjectTypeErrorAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ObjectTypeErrorAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasEntityTypeRequiresReferenceSchemeError)
				/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasCompatibleSupertypesError)
				/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasPreferredIdentifierRequiresMandatoryError)
				/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasUnspecifiedDataTypeError)
				/// /// </summary>
				/// private static void ObjectTypeErrorAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.ObjectTypeErrorAddedRule");
					ORMElementGateway.ObjectTypeErrorAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.ObjectTypeErrorAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasEntityTypeRequiresReferenceSchemeError), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasCompatibleSupertypesError), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasPreferredIdentifierRequiresMandatoryError), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasUnspecifiedDataTypeError), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ObjectTypeErrorDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ObjectTypeErrorDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasEntityTypeRequiresReferenceSchemeError)
				/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasCompatibleSupertypesError)
				/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasPreferredIdentifierRequiresMandatoryError)
				/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasUnspecifiedDataTypeError)
				/// /// </summary>
				/// private static void ObjectTypeErrorDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.ObjectTypeErrorDeletedRule");
					ORMElementGateway.ObjectTypeErrorDeletedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.ObjectTypeErrorDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ObjectificationAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ObjectificationAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification)
				/// /// </summary>
				/// private static void ObjectificationAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.ObjectificationAddedRule");
					ORMElementGateway.ObjectificationAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.ObjectificationAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ObjectificationDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ObjectificationDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification)
				/// /// </summary>
				/// private static void ObjectificationDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.ObjectificationDeletedRule");
					ORMElementGateway.ObjectificationDeletedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.ObjectificationDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ObjectificationRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ObjectificationRolePlayerChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// RolePlayerChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Objectification)
				/// /// </summary>
				/// private static void ObjectificationRolePlayerChangedRule(RolePlayerChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.ObjectificationRolePlayerChangedRule");
					ORMElementGateway.ObjectificationRolePlayerChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.ObjectificationRolePlayerChangedRule");
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
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
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
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.PreferredIdentifierAddedRule");
					ORMElementGateway.PreferredIdentifierAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.PreferredIdentifierAddedRule");
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
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
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
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.RolePlayerAddedRule");
					ORMElementGateway.RolePlayerAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.RolePlayerAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class RolePlayerDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public RolePlayerDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypePlaysRole)
				/// /// </summary>
				/// private static void RolePlayerDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.RolePlayerDeletedRule");
					ORMElementGateway.RolePlayerDeletedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.RolePlayerDeletedRule");
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
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
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
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.RolePlayerRolePlayerChangedRule");
					ORMElementGateway.RolePlayerRolePlayerChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.RolePlayerRolePlayerChangedRule");
				}
			}
		}
	}
	#endregion // Rule classes for AbstractionModelIsForORMModel.ORMElementGateway
	#region Rule classes for AbstractionModelIsForORMModel.ModificationTracker
	partial class AbstractionModelIsForORMModel
	{
		partial class ModificationTracker
		{
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConceptTypeIsForObjectType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ConceptTypeBridgeDetachedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ConceptTypeBridgeDetachedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// DeleteRule: typeof(ConceptTypeIsForObjectType)
				/// /// </summary>
				/// private static void ConceptTypeBridgeDetachedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ConceptTypeBridgeDetachedRule");
					ModificationTracker.ConceptTypeBridgeDetachedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ConceptTypeBridgeDetachedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(InformationTypeFormatIsForValueType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class InformationTypeFormatBridgeDetachedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public InformationTypeFormatBridgeDetachedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// DeleteRule: typeof(InformationTypeFormatIsForValueType)
				/// /// </summary>
				/// private static void InformationTypeFormatBridgeDetachedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.InformationTypeFormatBridgeDetachedRule");
					ModificationTracker.InformationTypeFormatBridgeDetachedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.InformationTypeFormatBridgeDetachedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConceptTypeChildHasPathFactType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ConceptTypeChildPathBridgeDetachedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ConceptTypeChildPathBridgeDetachedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// DeleteRule: typeof(ConceptTypeChildHasPathFactType)
				/// /// </summary>
				/// private static void ConceptTypeChildPathBridgeDetachedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ConceptTypeChildPathBridgeDetachedRule");
					ModificationTracker.ConceptTypeChildPathBridgeDetachedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ConceptTypeChildPathBridgeDetachedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ConstraintRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ConstraintRoleAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole)
				/// /// </summary>
				/// private static void ConstraintRoleAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ConstraintRoleAddedRule");
					ModificationTracker.ConstraintRoleAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ConstraintRoleAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ConstraintRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ConstraintRoleDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole)
				/// /// </summary>
				/// private static void ConstraintRoleDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ConstraintRoleDeletedRule");
					ModificationTracker.ConstraintRoleDeletedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ConstraintRoleDeletedRule");
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
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
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
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.DataTypeAddedRule");
					ModificationTracker.DataTypeAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.DataTypeAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasDataType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class DataTypeDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public DataTypeDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasDataType)
				/// /// </summary>
				/// private static void DataTypeDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.DataTypeDeletedRule");
					ModificationTracker.DataTypeDeletedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.DataTypeDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasDataType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class DataTypeChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public DataTypeChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasDataType)
				/// /// </summary>
				/// private static void DataTypeChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.DataTypeChangedRule");
					ModificationTracker.DataTypeChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.DataTypeChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasDataType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class DataTypeRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public DataTypeRolePlayerChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// RolePlayerChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasDataType)
				/// /// </summary>
				/// private static void DataTypeRolePlayerChangedRule(RolePlayerChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.DataTypeRolePlayerChangedRule");
					ModificationTracker.DataTypeRolePlayerChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.DataTypeRolePlayerChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactType)
				/// /// </summary>
				/// private static void FactTypeChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.FactTypeChangedRule");
					ModificationTracker.FactTypeChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.FactTypeChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ObjectTypeChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ObjectTypeChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectType)
				/// /// </summary>
				/// private static void ObjectTypeChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ObjectTypeChangedRule");
					ModificationTracker.ObjectTypeChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ObjectTypeChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ORMModelChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ORMModelChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel)
				/// /// </summary>
				/// private static void ORMModelChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ORMModelChangedRule");
					ModificationTracker.ORMModelChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ORMModelChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class PreferredIdentifierDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public PreferredIdentifierDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier)
				/// /// </summary>
				/// private static void PreferredIdentifierDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.PreferredIdentifierDeletedRule");
					ModificationTracker.PreferredIdentifierDeletedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.PreferredIdentifierDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class PreferredIdentifierRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public PreferredIdentifierRolePlayerChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// RolePlayerChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.EntityTypeHasPreferredIdentifier)
				/// /// </summary>
				/// private static void PreferredIdentifierRolePlayerChangedRule(RolePlayerChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.PreferredIdentifierRolePlayerChangedRule");
					ModificationTracker.PreferredIdentifierRolePlayerChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.PreferredIdentifierRolePlayerChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasReadingOrder), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ReadingOrderAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ReadingOrderAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasReadingOrder)
				/// /// </summary>
				/// private static void ReadingOrderAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ReadingOrderAddedRule");
					ModificationTracker.ReadingOrderAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ReadingOrderAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasReadingOrder), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ReadingOrderDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ReadingOrderDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasReadingOrder)
				/// /// </summary>
				/// private static void ReadingOrderDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ReadingOrderDeletedRule");
					ModificationTracker.ReadingOrderDeletedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ReadingOrderDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasReadingOrder), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ReadingOrderReorderedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ReadingOrderReorderedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// RolePlayerPositionChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactTypeHasReadingOrder)
				/// /// </summary>
				/// private static void ReadingOrderReorderedRule(RolePlayerOrderChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void RolePlayerPositionChanged(Microsoft.VisualStudio.Modeling.RolePlayerOrderChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ReadingOrderReorderedRule");
					ModificationTracker.ReadingOrderReorderedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ReadingOrderReorderedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrderHasReading), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ReadingAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ReadingAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrderHasReading)
				/// /// </summary>
				/// private static void ReadingAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ReadingAddedRule");
					ModificationTracker.ReadingAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ReadingAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Reading), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ReadingChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ReadingChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Reading)
				/// /// </summary>
				/// private static void ReadingChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ReadingChangedRule");
					ModificationTracker.ReadingChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ReadingChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrderHasReading), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ReadingDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ReadingDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrderHasReading)
				/// /// </summary>
				/// private static void ReadingDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ReadingDeletedRule");
					ModificationTracker.ReadingDeletedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ReadingDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrderHasReading), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ReadingReorderedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ReadingReorderedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// RolePlayerPositionChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ReadingOrderHasReading)
				/// /// </summary>
				/// private static void ReadingReorderedRule(RolePlayerOrderChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void RolePlayerPositionChanged(Microsoft.VisualStudio.Modeling.RolePlayerOrderChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ReadingReorderedRule");
					ModificationTracker.ReadingReorderedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ReadingReorderedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Role), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class RoleChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public RoleChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Role)
				/// /// </summary>
				/// private static void RoleChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.RoleChangedRule");
					ModificationTracker.RoleChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.RoleChangedRule");
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
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
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
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.RolePlayerRolePlayerChangedRule");
					ModificationTracker.RolePlayerRolePlayerChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.RolePlayerRolePlayerChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class SetConstraintChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public SetConstraintChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.SetConstraint)
				/// /// </summary>
				/// private static void SetConstraintChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.SetConstraintChangedRule");
					ModificationTracker.SetConstraintChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.SetConstraintChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class SubtypeFactChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public SubtypeFactChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.SubtypeFact)
				/// /// </summary>
				/// private static void SubtypeFactChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.SubtypeFactChangedRule");
					ModificationTracker.SubtypeFactChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.SubtypeFactChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class UniquenessConstraintRoleOrderChangedClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public UniquenessConstraintRoleOrderChangedClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// RolePlayerPositionChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole)
				/// /// </summary>
				/// private static void UniquenessConstraintRoleOrderChanged(RolePlayerOrderChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void RolePlayerPositionChanged(Microsoft.VisualStudio.Modeling.RolePlayerOrderChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.UniquenessConstraintRoleOrderChanged");
					ModificationTracker.UniquenessConstraintRoleOrderChanged(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.UniquenessConstraintRoleOrderChanged");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class UniquenessConstraintRoleDeletingClass : Microsoft.VisualStudio.Modeling.DeletingRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public UniquenessConstraintRoleDeletingClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// DeletingRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole)
				/// /// </summary>
				/// private static void UniquenessConstraintRoleDeleting(ElementDeletingEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.UniquenessConstraintRoleDeleting");
					ModificationTracker.UniquenessConstraintRoleDeleting(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.UniquenessConstraintRoleDeleting");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(UniquenessIsForUniquenessConstraint), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class UniquenessBridgeDetachedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public UniquenessBridgeDetachedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// DeleteRule: typeof(UniquenessIsForUniquenessConstraint)
				/// /// </summary>
				/// private static void UniquenessBridgeDetachedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.UniquenessBridgeDetachedRule");
					ModificationTracker.UniquenessBridgeDetachedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.UniquenessBridgeDetachedRule");
				}
			}
		}
	}
	#endregion // Rule classes for AbstractionModelIsForORMModel.ModificationTracker
	#endregion // Auto-rule classes
}
