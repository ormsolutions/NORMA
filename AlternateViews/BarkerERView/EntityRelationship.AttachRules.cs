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

namespace Neumont.Tools.ORM.Views.BarkerERView
{
	#region Attach rules to BarkerERShapeDomainModel model
	partial class BarkerERShapeDomainModel : Neumont.Tools.Modeling.Shell.IDomainModelEnablesRulesAfterDeserialization
	{
		private static Type[] myCustomDomainModelTypes;
		private static Type[] CustomDomainModelTypes
		{
			get
			{
				Type[] retVal = BarkerERShapeDomainModel.myCustomDomainModelTypes;
				if (retVal == null)
				{
					// No synchronization is needed here.
					// If accessed concurrently, the worst that will happen is the array of Types being created multiple times.
					// This would have a slightly negative impact on performance, but the result would still be correct.
					// Given the low likelihood of this ever happening, the extra overhead of synchronization would outweigh any possible gain from it.
					retVal = new Type[]{
						typeof(BarkerERDiagram).GetNestedType("ConceptTypeDetachingFromEntityTypeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(BarkerERDiagram).GetNestedType("ConceptTypeDetachingFromObjectTypeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(BarkerERDiagram).GetNestedType("DisplayAttributePropertyChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(BarkerERDiagram).GetNestedType("DisplayEntityTypePropertyChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(BarkerERDiagram).GetNestedType("NameChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(BarkerERDiagram).GetNestedType("BinaryAssociationAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AttributeElementListCompartment)};
					BarkerERShapeDomainModel.myCustomDomainModelTypes = retVal;
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
				Type[] retVal = BarkerERShapeDomainModel.myInitiallyDisabledRuleTypes;
				if (retVal == null)
				{
					Type[] customDomainModelTypes = BarkerERShapeDomainModel.CustomDomainModelTypes;
					retVal = new Type[]{
						customDomainModelTypes[0],
						customDomainModelTypes[1],
						customDomainModelTypes[2],
						customDomainModelTypes[3],
						customDomainModelTypes[4]};
					BarkerERShapeDomainModel.myInitiallyDisabledRuleTypes = retVal;
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
			Type[] customDomainModelTypes = BarkerERShapeDomainModel.CustomDomainModelTypes;
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
			BarkerERShapeDomainModel.EnableDiagramRules(store);
			Microsoft.VisualStudio.Modeling.RuleManager ruleManager = store.RuleManager;
			Type[] disabledRuleTypes = BarkerERShapeDomainModel.InitiallyDisabledRuleTypes;
			for (int i = 0; i < 5; ++i)
			{
				ruleManager.EnableRule(disabledRuleTypes[i]);
			}
		}
		void Neumont.Tools.Modeling.Shell.IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.Store store)
		{
			this.EnableRulesAfterDeserialization(store);
		}
	}
	#endregion // Attach rules to BarkerERShapeDomainModel model
	#region Auto-rule classes
	#region Rule classes for BarkerERDiagram
	partial class BarkerERDiagram
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORMAbstractionToBarkerERBridge.EntityTypeIsPrimarilyForConceptType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ConceptTypeDetachingFromEntityTypeRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ConceptTypeDetachingFromEntityTypeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.Views.BarkerERView.BarkerERDiagram
			/// /// <summary>
			/// /// DeletingRule: typeof(Neumont.Tools.ORMAbstractionToBarkerERBridge.EntityTypeIsPrimarilyForConceptType)
			/// /// </summary>
			/// private static void ConceptTypeDetachingFromEntityTypeRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.Views.BarkerERView.BarkerERDiagram.ConceptTypeDetachingFromEntityTypeRule");
				BarkerERDiagram.ConceptTypeDetachingFromEntityTypeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.Views.BarkerERView.BarkerERDiagram.ConceptTypeDetachingFromEntityTypeRule");
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
			/// Neumont.Tools.ORM.Views.BarkerERView.BarkerERDiagram
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
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.Views.BarkerERView.BarkerERDiagram.ConceptTypeDetachingFromObjectTypeRule");
				BarkerERDiagram.ConceptTypeDetachingFromObjectTypeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.Views.BarkerERView.BarkerERDiagram.ConceptTypeDetachingFromObjectTypeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.EntityRelationshipModels.Barker.Attribute), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DisplayAttributePropertyChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DisplayAttributePropertyChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.Views.BarkerERView.BarkerERDiagram
			/// /// <summary>
			/// /// ChangeRule: typeof(Neumont.Tools.EntityRelationshipModels.Barker.Attribute)
			/// /// </summary>
			/// private static void DisplayAttributePropertyChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.Views.BarkerERView.BarkerERDiagram.DisplayAttributePropertyChangedRule");
				BarkerERDiagram.DisplayAttributePropertyChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.Views.BarkerERView.BarkerERDiagram.DisplayAttributePropertyChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.EntityRelationshipModels.Barker.EntityType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DisplayEntityTypePropertyChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DisplayEntityTypePropertyChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.Views.BarkerERView.BarkerERDiagram
			/// /// <summary>
			/// /// ChangeRule: typeof(Neumont.Tools.EntityRelationshipModels.Barker.EntityType)
			/// /// </summary>
			/// private static void DisplayEntityTypePropertyChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.Views.BarkerERView.BarkerERDiagram.DisplayEntityTypePropertyChangedRule");
				BarkerERDiagram.DisplayEntityTypePropertyChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.Views.BarkerERView.BarkerERDiagram.DisplayEntityTypePropertyChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(BarkerERDiagram), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class NameChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public NameChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.Views.BarkerERView.BarkerERDiagram
			/// /// <summary>
			/// /// ChangeRule: typeof(BarkerERDiagram)
			/// /// </summary>
			/// private static void NameChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.Views.BarkerERView.BarkerERDiagram.NameChangedRule");
				BarkerERDiagram.NameChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.Views.BarkerERView.BarkerERDiagram.NameChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.EntityRelationshipModels.Barker.BarkerErModelContainsBinaryAssociation), FireTime=Microsoft.VisualStudio.Modeling.TimeToFire.TopLevelCommit, Priority=Microsoft.VisualStudio.Modeling.Diagrams.DiagramFixupConstants.AddConnectionRulePriority)]
		private sealed class BinaryAssociationAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.Views.BarkerERView.BarkerERDiagram
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.EntityRelationshipModels.Barker.BarkerErModelContainsBinaryAssociation), FireTime=TopLevelCommit, Priority=DiagramFixupConstants.AddConnectionRulePriority;
			/// /// </summary>
			/// private static void BinaryAssociationAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.Views.BarkerERView.BarkerERDiagram.BinaryAssociationAddedRule");
				BarkerERDiagram.BinaryAssociationAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.Views.BarkerERView.BarkerERDiagram.BinaryAssociationAddedRule");
			}
		}
	}
	#endregion // Rule classes for BarkerERDiagram
	#endregion // Auto-rule classes
}
