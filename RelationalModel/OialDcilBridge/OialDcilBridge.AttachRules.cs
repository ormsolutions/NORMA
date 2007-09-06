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

namespace Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge
{
	#region Attach rules to ORMAbstractionToConceptualDatabaseBridgeDomainModel model
	partial class ORMAbstractionToConceptualDatabaseBridgeDomainModel : Neumont.Tools.Modeling.Shell.IDomainModelEnablesRulesAfterDeserialization
	{
		private static Type[] myCustomDomainModelTypes;
		private static Type[] CustomDomainModelTypes
		{
			get
			{
				Type[] retVal = ORMAbstractionToConceptualDatabaseBridgeDomainModel.myCustomDomainModelTypes;
				if (retVal == null)
				{
					// No synchronization is needed here.
					// If accessed concurrently, the worst that will happen is the array of Types being created multiple times.
					// This would have a slightly negative impact on performance, but the result would still be correct.
					// Given the low likelihood of this ever happening, the extra overhead of synchronization would outweigh any possible gain from it.
					retVal = new Type[]{
						typeof(ModificationTracker).GetNestedType("AbstractionModelChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("AssimilationMappingAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("AssimilationMappingChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("ConceptTypeAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("ConceptTypeChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("ConceptTypeChildChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("ConceptTypeDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("DefaultReferenceModeNamingCustomizesORMModelAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("DefaultReferenceModeNamingChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("FactTypeNameChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("InformationTypeFormatAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("InformationTypeFormatDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("ReferenceModeNamingCustomizesObjectTypeAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("ReferenceModeNamingChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("RoleNameChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("UniquenessDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AssimilationMapping).GetNestedType("AssimilationMappingAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AssimilationMapping).GetNestedType("AssimilationMappingChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AssimilationMapping).GetNestedType("DisjunctiveMandatoryCouplerDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AssimilationMapping).GetNestedType("DisjunctiveMandatoryRoleDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AssimilationMapping).GetNestedType("FactTypeDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AssimilationMapping).GetNestedType("PathFactTypeAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic)};
					ORMAbstractionToConceptualDatabaseBridgeDomainModel.myCustomDomainModelTypes = retVal;
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
			Type[] customDomainModelTypes = ORMAbstractionToConceptualDatabaseBridgeDomainModel.CustomDomainModelTypes;
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
			Type[] disabledRuleTypes = ORMAbstractionToConceptualDatabaseBridgeDomainModel.CustomDomainModelTypes;
			for (int i = 0; i < 22; ++i)
			{
				ruleManager.EnableRule(disabledRuleTypes[i]);
			}
		}
		void Neumont.Tools.Modeling.Shell.IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.Store store)
		{
			this.EnableRulesAfterDeserialization(store);
		}
	}
	#endregion // Attach rules to ORMAbstractionToConceptualDatabaseBridgeDomainModel model
	#region Auto-rule classes
	#region Rule classes for ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
	partial class ORMAbstractionToConceptualDatabaseBridgeDomainModel
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
				/// Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
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
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.AbstractionModelChangedRule");
					ModificationTracker.AbstractionModelChangedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.AbstractionModelChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(AssimilationMappingCustomizesFactType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class AssimilationMappingAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public AssimilationMappingAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// AddRule: typeof(AssimilationMappingCustomizesFactType)
				/// /// </summary>
				/// private static void AssimilationMappingAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.AssimilationMappingAddedRule");
					ModificationTracker.AssimilationMappingAddedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.AssimilationMappingAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(AssimilationMapping), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class AssimilationMappingChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public AssimilationMappingChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(AssimilationMapping)
				/// /// </summary>
				/// private static void AssimilationMappingChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.AssimilationMappingChangedRule");
					ModificationTracker.AssimilationMappingChangedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.AssimilationMappingChangedRule");
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
				/// Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
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
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ConceptTypeAddedRule");
					ModificationTracker.ConceptTypeAddedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ConceptTypeAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORMAbstraction.ConceptType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ConceptTypeChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ConceptTypeChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(Neumont.Tools.ORMAbstraction.ConceptType)
				/// /// </summary>
				/// private static void ConceptTypeChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ConceptTypeChangedRule");
					ModificationTracker.ConceptTypeChangedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ConceptTypeChangedRule");
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
				/// Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
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
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ConceptTypeChildChangedRule");
					ModificationTracker.ConceptTypeChildChangedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ConceptTypeChildChangedRule");
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
				/// Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
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
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ConceptTypeDeletedRule");
					ModificationTracker.ConceptTypeDeletedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ConceptTypeDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(DefaultReferenceModeNamingCustomizesORMModel), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class DefaultReferenceModeNamingCustomizesORMModelAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public DefaultReferenceModeNamingCustomizesORMModelAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// AddRule: typeof(DefaultReferenceModeNamingCustomizesORMModel)
				/// /// </summary>
				/// private static void DefaultReferenceModeNamingCustomizesORMModelAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.DefaultReferenceModeNamingCustomizesORMModelAddedRule");
					ModificationTracker.DefaultReferenceModeNamingCustomizesORMModelAddedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.DefaultReferenceModeNamingCustomizesORMModelAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(DefaultReferenceModeNaming), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class DefaultReferenceModeNamingChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public DefaultReferenceModeNamingChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(DefaultReferenceModeNaming)
				/// /// </summary>
				/// private static void DefaultReferenceModeNamingChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.DefaultReferenceModeNamingChangedRule");
					ModificationTracker.DefaultReferenceModeNamingChangedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.DefaultReferenceModeNamingChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FactType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeNameChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeNameChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.FactType)
				/// /// </summary>
				/// private static void FactTypeNameChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.FactTypeNameChangedRule");
					ModificationTracker.FactTypeNameChangedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.FactTypeNameChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORMAbstraction.AbstractionModelHasInformationTypeFormat), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class InformationTypeFormatAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public InformationTypeFormatAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// AddRule: typeof(Neumont.Tools.ORMAbstraction.AbstractionModelHasInformationTypeFormat)
				/// /// </summary>
				/// private static void InformationTypeFormatAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.InformationTypeFormatAddedRule");
					ModificationTracker.InformationTypeFormatAddedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.InformationTypeFormatAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORMAbstraction.AbstractionModelHasInformationTypeFormat), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class InformationTypeFormatDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public InformationTypeFormatDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// DeleteRule: typeof(Neumont.Tools.ORMAbstraction.AbstractionModelHasInformationTypeFormat)
				/// /// </summary>
				/// private static void InformationTypeFormatDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.InformationTypeFormatDeletedRule");
					ModificationTracker.InformationTypeFormatDeletedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.InformationTypeFormatDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReferenceModeNamingCustomizesObjectType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ReferenceModeNamingCustomizesObjectTypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ReferenceModeNamingCustomizesObjectTypeAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// AddRule: typeof(ReferenceModeNamingCustomizesObjectType)
				/// /// </summary>
				/// private static void ReferenceModeNamingCustomizesObjectTypeAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ReferenceModeNamingCustomizesObjectTypeAddedRule");
					ModificationTracker.ReferenceModeNamingCustomizesObjectTypeAddedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ReferenceModeNamingCustomizesObjectTypeAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReferenceModeNaming), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ReferenceModeNamingChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ReferenceModeNamingChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(ReferenceModeNaming)
				/// /// </summary>
				/// private static void ReferenceModeNamingChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ReferenceModeNamingChangedRule");
					ModificationTracker.ReferenceModeNamingChangedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ReferenceModeNamingChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.Role), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class RoleNameChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public RoleNameChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.Role)
				/// /// </summary>
				/// private static void RoleNameChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.RoleNameChangedRule");
					ModificationTracker.RoleNameChangedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.RoleNameChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(UniquenessConstraintIsForUniqueness), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class UniquenessDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public UniquenessDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// DeleteRule: typeof(UniquenessConstraintIsForUniqueness)
				/// /// </summary>
				/// private static void UniquenessDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.UniquenessDeletedRule");
					ModificationTracker.UniquenessDeletedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.UniquenessDeletedRule");
				}
			}
		}
	}
	#endregion // Rule classes for ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
	#region Rule classes for AssimilationMapping
	partial class AssimilationMapping
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(AssimilationMappingCustomizesFactType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class AssimilationMappingAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public AssimilationMappingAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping
			/// /// <summary>
			/// /// AddRule: typeof(AssimilationMappingCustomizesFactType)
			/// /// </summary>
			/// private static void AssimilationMappingAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.AssimilationMappingAddedRule");
				AssimilationMapping.AssimilationMappingAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.AssimilationMappingAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(AssimilationMapping), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class AssimilationMappingChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public AssimilationMappingChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping
			/// /// <summary>
			/// /// ChangeRule: typeof(AssimilationMapping)
			/// /// </summary>
			/// private static void AssimilationMappingChangedRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.AssimilationMappingChangedRule");
				AssimilationMapping.AssimilationMappingChangedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.AssimilationMappingChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DisjunctiveMandatoryCouplerDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DisjunctiveMandatoryCouplerDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping
			/// /// <summary>
			/// /// DeletingRule: typeof(Neumont.Tools.ORM.ObjectModel.ExclusiveOrConstraintCoupler)
			/// /// </summary>
			/// private static void DisjunctiveMandatoryCouplerDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.DisjunctiveMandatoryCouplerDeletingRule");
				AssimilationMapping.DisjunctiveMandatoryCouplerDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.DisjunctiveMandatoryCouplerDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DisjunctiveMandatoryRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DisjunctiveMandatoryRoleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping
			/// /// <summary>
			/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void DisjunctiveMandatoryRoleDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.DisjunctiveMandatoryRoleDeletedRule");
				AssimilationMapping.DisjunctiveMandatoryRoleDeletedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.DisjunctiveMandatoryRoleDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FactType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping
			/// /// <summary>
			/// /// DeletingRule: typeof(Neumont.Tools.ORM.ObjectModel.FactType)
			/// /// </summary>
			/// private static void FactTypeDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.FactTypeDeletingRule");
				AssimilationMapping.FactTypeDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.FactTypeDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORMToORMAbstractionBridge.ConceptTypeChildHasPathFactType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class PathFactTypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PathFactTypeAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORMToORMAbstractionBridge.ConceptTypeChildHasPathFactType)
			/// /// </summary>
			/// private static void PathFactTypeAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.PathFactTypeAddedRule");
				AssimilationMapping.PathFactTypeAddedRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.PathFactTypeAddedRule");
			}
		}
	}
	#endregion // Rule classes for AssimilationMapping
	#endregion // Auto-rule classes
}
