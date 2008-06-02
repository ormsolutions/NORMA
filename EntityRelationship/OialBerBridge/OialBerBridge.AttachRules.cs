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

namespace Neumont.Tools.ORMAbstractionToBarkerERBridge
{
	#region Attach rules to ORMAbstractionToBarkerERBridgeDomainModel model
	partial class ORMAbstractionToBarkerERBridgeDomainModel : Neumont.Tools.Modeling.Shell.IDomainModelEnablesRulesAfterDeserialization
	{
		private static Type[] myCustomDomainModelTypes;
		private static Type[] CustomDomainModelTypes
		{
			get
			{
				Type[] retVal = ORMAbstractionToBarkerERBridgeDomainModel.myCustomDomainModelTypes;
				if (retVal == null)
				{
					// No synchronization is needed here.
					// If accessed concurrently, the worst that will happen is the array of Types being created multiple times.
					// This would have a slightly negative impact on performance, but the result would still be correct.
					// Given the low likelihood of this ever happening, the extra overhead of synchronization would outweigh any possible gain from it.
					retVal = new Type[]{
						typeof(ModificationTracker).GetNestedType("AbstractionModelChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("ConceptTypeAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("ConceptTypeChildChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("ConceptTypeDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic)};
					ORMAbstractionToBarkerERBridgeDomainModel.myCustomDomainModelTypes = retVal;
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
			Type[] customDomainModelTypes = ORMAbstractionToBarkerERBridgeDomainModel.CustomDomainModelTypes;
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
			Type[] disabledRuleTypes = ORMAbstractionToBarkerERBridgeDomainModel.CustomDomainModelTypes;
			for (int i = 0; i < 4; ++i)
			{
				ruleManager.EnableRule(disabledRuleTypes[i]);
			}
		}
		void Neumont.Tools.Modeling.Shell.IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.Store store)
		{
			this.EnableRulesAfterDeserialization(store);
		}
	}
	#endregion // Attach rules to ORMAbstractionToBarkerERBridgeDomainModel model
	#region Auto-rule classes
	#region Rule classes for ORMAbstractionToBarkerERBridgeDomainModel.ModificationTracker
	partial class ORMAbstractionToBarkerERBridgeDomainModel
	{
		partial class ModificationTracker
		{
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORMAbstraction.AbstractionModel), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class AbstractionModelChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public AbstractionModelChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMAbstractionToBarkerERBridge.ORMAbstractionToBarkerERBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(Neumont.Tools.ORMAbstraction.AbstractionModel)
				/// /// </summary>
				/// private static void AbstractionModelChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToBarkerERBridge.ORMAbstractionToBarkerERBridgeDomainModel.ModificationTracker.AbstractionModelChangedRule");
					ModificationTracker.AbstractionModelChangedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToBarkerERBridge.ORMAbstractionToBarkerERBridgeDomainModel.ModificationTracker.AbstractionModelChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORMAbstraction.AbstractionModelHasConceptType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ConceptTypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ConceptTypeAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMAbstractionToBarkerERBridge.ORMAbstractionToBarkerERBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// AddRule: typeof(Neumont.Tools.ORMAbstraction.AbstractionModelHasConceptType)
				/// /// </summary>
				/// private static void ConceptTypeAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToBarkerERBridge.ORMAbstractionToBarkerERBridgeDomainModel.ModificationTracker.ConceptTypeAddedRule");
					ModificationTracker.ConceptTypeAddedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToBarkerERBridge.ORMAbstractionToBarkerERBridgeDomainModel.ModificationTracker.ConceptTypeAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORMAbstraction.ConceptTypeChild), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ConceptTypeChildChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ConceptTypeChildChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMAbstractionToBarkerERBridge.ORMAbstractionToBarkerERBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(Neumont.Tools.ORMAbstraction.ConceptTypeChild)
				/// /// </summary>
				/// private static void ConceptTypeChildChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToBarkerERBridge.ORMAbstractionToBarkerERBridgeDomainModel.ModificationTracker.ConceptTypeChildChangedRule");
					ModificationTracker.ConceptTypeChildChangedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToBarkerERBridge.ORMAbstractionToBarkerERBridgeDomainModel.ModificationTracker.ConceptTypeChildChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORMAbstraction.AbstractionModelHasConceptType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ConceptTypeDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ConceptTypeDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMAbstractionToBarkerERBridge.ORMAbstractionToBarkerERBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// DeleteRule: typeof(Neumont.Tools.ORMAbstraction.AbstractionModelHasConceptType)
				/// /// </summary>
				/// private static void ConceptTypeDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToBarkerERBridge.ORMAbstractionToBarkerERBridgeDomainModel.ModificationTracker.ConceptTypeDeletedRule");
					ModificationTracker.ConceptTypeDeletedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToBarkerERBridge.ORMAbstractionToBarkerERBridgeDomainModel.ModificationTracker.ConceptTypeDeletedRule");
				}
			}
		}
	}
	#endregion // Rule classes for ORMAbstractionToBarkerERBridgeDomainModel.ModificationTracker
	#endregion // Auto-rule classes
}
