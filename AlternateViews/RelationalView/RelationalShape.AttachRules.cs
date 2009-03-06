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

namespace ORMSolutions.ORMArchitect.Views.RelationalView
{
	#region Attach rules to RelationalShapeDomainModel model
	partial class RelationalShapeDomainModel : ORMSolutions.ORMArchitect.Framework.Shell.IDomainModelEnablesRulesAfterDeserialization
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
						typeof(RelationalDiagram).GetNestedType("ConceptTypeDetachingFromTableRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RelationalDiagram).GetNestedType("ConceptTypeDetachingFromObjectTypeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RelationalDiagram).GetNestedType("DataTypeChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RelationalDiagram).GetNestedType("DataTypeFacetChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RelationalDiagram).GetNestedType("DisplayColumnPropertyChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RelationalDiagram).GetNestedType("DisplayTablePropertyChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RelationalDiagram).GetNestedType("NameChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RelationalDiagram).GetNestedType("ReferenceConstraintAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
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
						customDomainModelTypes[2],
						customDomainModelTypes[3],
						customDomainModelTypes[4],
						customDomainModelTypes[5],
						customDomainModelTypes[6]};
					RelationalShapeDomainModel.myInitiallyDisabledRuleTypes = retVal;
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
			for (int i = 0; i < 7; ++i)
			{
				ruleManager.EnableRule(disabledRuleTypes[i]);
			}
		}
		void ORMSolutions.ORMArchitect.Framework.Shell.IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.Store store)
		{
			this.EnableRulesAfterDeserialization(store);
		}
	}
	#endregion // Attach rules to RelationalShapeDomainModel model
	#region Auto-rule classes
	#region Rule classes for RelationalDiagram
	partial class RelationalDiagram
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.TableIsPrimarilyForConceptType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConceptTypeDetachingFromTableRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConceptTypeDetachingFromTableRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram
			/// /// <summary>
			/// /// DeletingRule: typeof(ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.TableIsPrimarilyForConceptType)
			/// /// </summary>
			/// private static void ConceptTypeDetachingFromTableRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram.ConceptTypeDetachingFromTableRule");
				RelationalDiagram.ConceptTypeDetachingFromTableRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram.ConceptTypeDetachingFromTableRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.ConceptTypeIsForObjectType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConceptTypeDetachingFromObjectTypeRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConceptTypeDetachingFromObjectTypeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram
			/// /// <summary>
			/// /// DeletingRule: typeof(ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.ConceptTypeIsForObjectType)
			/// /// </summary>
			/// private static void ConceptTypeDetachingFromObjectTypeRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram.ConceptTypeDetachingFromObjectTypeRule");
				RelationalDiagram.ConceptTypeDetachingFromObjectTypeRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram.ConceptTypeDetachingFromObjectTypeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasDataType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DataTypeChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DataTypeChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram
			/// /// <summary>
			/// /// RolePlayerChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasDataType)
			/// /// </summary>
			/// private static void DataTypeChangedRule(RolePlayerChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram.DataTypeChangedRule");
				RelationalDiagram.DataTypeChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram.DataTypeChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasDataType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DataTypeFacetChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DataTypeFacetChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram
			/// /// <summary>
			/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ValueTypeHasDataType)
			/// /// </summary>
			/// private static void DataTypeFacetChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram.DataTypeFacetChangedRule");
				RelationalDiagram.DataTypeFacetChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram.DataTypeFacetChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.Column), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DisplayColumnPropertyChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DisplayColumnPropertyChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram
			/// /// <summary>
			/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.Column)
			/// /// </summary>
			/// private static void DisplayColumnPropertyChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram.DisplayColumnPropertyChangedRule");
				RelationalDiagram.DisplayColumnPropertyChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram.DisplayColumnPropertyChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.Table), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DisplayTablePropertyChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DisplayTablePropertyChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram
			/// /// <summary>
			/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.Table)
			/// /// </summary>
			/// private static void DisplayTablePropertyChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram.DisplayTablePropertyChangedRule");
				RelationalDiagram.DisplayTablePropertyChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram.DisplayTablePropertyChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(RelationalDiagram), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class NameChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public NameChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram.NameChangedRule");
				RelationalDiagram.NameChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram.NameChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.ReferenceConstraintTargetsTable), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class ReferenceConstraintAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.ReferenceConstraintTargetsTable), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void ReferenceConstraintAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram.ReferenceConstraintAddedRule");
				RelationalDiagram.ReferenceConstraintAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.Views.RelationalView.RelationalDiagram.ReferenceConstraintAddedRule");
			}
		}
	}
	#endregion // Rule classes for RelationalDiagram
	#endregion // Auto-rule classes
}
