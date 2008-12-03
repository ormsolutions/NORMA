using System;
using System.Reflection;

// Common Public License Copyright Notice
// /**************************************************************************\
// * Natural Object-Role Modeling Architect for Visual Studio                 *
// *                                                                          *
// * Copyright © Matthew Curland. All rights reserved.                        *
// *                                                                          *
// * The use and distribution terms for this software are covered by the      *
// * Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
// * can be found in the file CPL.txt at the root of this distribution.       *
// * By using this software in any fashion, you are agreeing to be bound by   *
// * the terms of this license.                                               *
// *                                                                          *
// * You must not remove this notice, or any other, from this software.       *
// \**************************************************************************/

namespace Neumont.Tools.Modeling.Shell
{
	#region Attach rules to DiagramDisplayDomainModel model
	partial class DiagramDisplayDomainModel : Neumont.Tools.Modeling.Shell.IDomainModelEnablesRulesAfterDeserialization
	{
		private static Type[] myCustomDomainModelTypes;
		private static Type[] CustomDomainModelTypes
		{
			get
			{
				Type[] retVal = DiagramDisplayDomainModel.myCustomDomainModelTypes;
				if (retVal == null)
				{
					// No synchronization is needed here.
					// If accessed concurrently, the worst that will happen is the array of Types being created multiple times.
					// This would have a slightly negative impact on performance, but the result would still be correct.
					// Given the low likelihood of this ever happening, the extra overhead of synchronization would outweigh any possible gain from it.
					retVal = new Type[]{
						typeof(DiagramDisplay).GetNestedType("DiagramAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(DiagramDisplay).GetNestedType("DiagramOrderDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic)};
					DiagramDisplayDomainModel.myCustomDomainModelTypes = retVal;
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
			Type[] customDomainModelTypes = DiagramDisplayDomainModel.CustomDomainModelTypes;
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
			Type[] disabledRuleTypes = DiagramDisplayDomainModel.CustomDomainModelTypes;
			for (int i = 0; i < 2; ++i)
			{
				ruleManager.EnableRule(disabledRuleTypes[i]);
			}
		}
		void Neumont.Tools.Modeling.Shell.IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.Store store)
		{
			this.EnableRulesAfterDeserialization(store);
		}
	}
	#endregion // Attach rules to DiagramDisplayDomainModel model
	#region Auto-rule classes
	#region Rule classes for DiagramDisplay
	partial class DiagramDisplay
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Microsoft.VisualStudio.Modeling.Diagrams.Diagram), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=Neumont.Tools.Modeling.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class DiagramAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DiagramAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.Modeling.Shell.DiagramDisplay
			/// /// <summary>
			/// /// AddRule: typeof(Microsoft.VisualStudio.Modeling.Diagrams.Diagram), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void DiagramAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.Modeling.Shell.DiagramDisplay.DiagramAddedRule");
				DiagramDisplay.DiagramAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.Modeling.Shell.DiagramDisplay.DiagramAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(DiagramDisplayHasDiagramOrder), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.LocalCommit, Priority=Neumont.Tools.Modeling.FrameworkDomainModel.BeforeDelayValidateRulePriority)]
		private sealed class DiagramOrderDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DiagramOrderDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.Modeling.Shell.DiagramDisplay
			/// /// <summary>
			/// /// DeleteRule: typeof(DiagramDisplayHasDiagramOrder), FireTime=LocalCommit, Priority=FrameworkDomainModel.BeforeDelayValidateRulePriority;
			/// /// </summary>
			/// private static void DiagramOrderDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.Modeling.Shell.DiagramDisplay.DiagramOrderDeletedRule");
				DiagramDisplay.DiagramOrderDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.Modeling.Shell.DiagramDisplay.DiagramOrderDeletedRule");
			}
		}
	}
	#endregion // Rule classes for DiagramDisplay
	#endregion // Auto-rule classes
}
