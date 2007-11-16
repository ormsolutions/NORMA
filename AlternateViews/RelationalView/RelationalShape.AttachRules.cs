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

namespace Neumont.Tools.ORM.Views.RelationalView
{
	#region Attach rules to RelationalShapeDomainModel model
	partial class RelationalShapeDomainModel : Neumont.Tools.Modeling.Shell.IDomainModelEnablesRulesAfterDeserialization
	{
		private static Type[] myCustomDomainModelTypes;
		private static Type[] CustomDomainModelTypes
		{
			get
			{
				Type[] retVal = RelationalShapeDomainModel.myCustomDomainModelTypes;
				if (retVal == null)
				{
					// No synchronization is needed here.
					// If accessed concurrently, the worst that will happen is the array of Types being created multiple times.
					// This would have a slightly negative impact on performance, but the result would still be correct.
					// Given the low likelihood of this ever happening, the extra overhead of synchronization would outweigh any possible gain from it.
					retVal = new Type[]{
						typeof(RelationalDiagram).GetNestedType("IsNullableChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RelationalDiagram).GetNestedType("NameChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RelationalDiagram).GetNestedType("ReferenceConstraintAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RelationalDiagram).GetNestedType("ConceptTypeDetachingFromTableRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RelationalDiagram).GetNestedType("ConceptTypeDetachingFromObjectTypeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ColumnElementListCompartment)};
					RelationalShapeDomainModel.myCustomDomainModelTypes = retVal;
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
				Type[] retVal = RelationalShapeDomainModel.myInitiallyDisabledRuleTypes;
				if (retVal == null)
				{
					Type[] customDomainModelTypes = RelationalShapeDomainModel.CustomDomainModelTypes;
					retVal = new Type[]{
						customDomainModelTypes[0],
						customDomainModelTypes[1],
						customDomainModelTypes[3],
						customDomainModelTypes[4]};
					RelationalShapeDomainModel.myInitiallyDisabledRuleTypes = retVal;
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
			Type[] customDomainModelTypes = RelationalShapeDomainModel.CustomDomainModelTypes;
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
			RelationalShapeDomainModel.EnableDiagramRules(store);
			Microsoft.VisualStudio.Modeling.RuleManager ruleManager = store.RuleManager;
			Type[] disabledRuleTypes = RelationalShapeDomainModel.InitiallyDisabledRuleTypes;
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
	#endregion // Attach rules to RelationalShapeDomainModel model
	#region Auto-rule classes
	#region Rule classes for RelationalDiagram
	partial class RelationalDiagram
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.RelationalModels.ConceptualDatabase.Column), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class IsNullableChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public IsNullableChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.Views.RelationalView.RelationalDiagram
			/// /// <summary>
			/// /// ChangeRule: typeof(Neumont.Tools.RelationalModels.ConceptualDatabase.Column)
			/// /// </summary>
			/// private static void IsNullableChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.Views.RelationalView.RelationalDiagram.IsNullableChangedRule");
				RelationalDiagram.IsNullableChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.Views.RelationalView.RelationalDiagram.IsNullableChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RelationalDiagram), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class NameChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public NameChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.Views.RelationalView.RelationalDiagram
			/// /// <summary>
			/// /// ChangeRule: typeof(RelationalDiagram)
			/// /// </summary>
			/// private static void NameChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.Views.RelationalView.RelationalDiagram.NameChangedRule");
				RelationalDiagram.NameChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.Views.RelationalView.RelationalDiagram.NameChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.RelationalModels.ConceptualDatabase.ReferenceConstraintTargetsTable), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class ReferenceConstraintAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.Views.RelationalView.RelationalDiagram
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.RelationalModels.ConceptualDatabase.ReferenceConstraintTargetsTable), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void ReferenceConstraintAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.Views.RelationalView.RelationalDiagram.ReferenceConstraintAddedRule");
				RelationalDiagram.ReferenceConstraintAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.Views.RelationalView.RelationalDiagram.ReferenceConstraintAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.TableIsPrimarilyForConceptType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConceptTypeDetachingFromTableRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConceptTypeDetachingFromTableRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.Views.RelationalView.RelationalDiagram
			/// /// <summary>
			/// /// DeletingRule: typeof(Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.TableIsPrimarilyForConceptType)
			/// /// </summary>
			/// private static void ConceptTypeDetachingFromTableRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.Views.RelationalView.RelationalDiagram.ConceptTypeDetachingFromTableRule");
				RelationalDiagram.ConceptTypeDetachingFromTableRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.Views.RelationalView.RelationalDiagram.ConceptTypeDetachingFromTableRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORMToORMAbstractionBridge.ConceptTypeIsForObjectType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConceptTypeDetachingFromObjectTypeRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConceptTypeDetachingFromObjectTypeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.Views.RelationalView.RelationalDiagram
			/// /// <summary>
			/// /// DeletingRule: typeof(Neumont.Tools.ORMToORMAbstractionBridge.ConceptTypeIsForObjectType)
			/// /// </summary>
			/// private static void ConceptTypeDetachingFromObjectTypeRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.Views.RelationalView.RelationalDiagram.ConceptTypeDetachingFromObjectTypeRule");
				RelationalDiagram.ConceptTypeDetachingFromObjectTypeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.Views.RelationalView.RelationalDiagram.ConceptTypeDetachingFromObjectTypeRule");
			}
		}
	}
	#endregion // Rule classes for RelationalDiagram
	#endregion // Auto-rule classes
}
