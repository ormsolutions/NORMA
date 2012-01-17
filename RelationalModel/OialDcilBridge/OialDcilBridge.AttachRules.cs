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

namespace ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge
{
	#region Attach rules to ORMAbstractionToConceptualDatabaseBridgeDomainModel model
	partial class ORMAbstractionToConceptualDatabaseBridgeDomainModel : ORMSolutions.ORMArchitect.Framework.Shell.IDomainModelEnablesRulesAfterDeserialization
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
						typeof(ModificationTracker).GetNestedType("AbbreviationAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("AbbreviationAddedRuleClass2", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("AbbreviationChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("AbbreviationDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("AbbreviationDeletedRuleClass2", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("AbstractionModelChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("AssimilationMappingAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("AssimilationMappingChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("ColumnChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("ColumnOrderChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("ConceptTypeAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("ConceptTypeChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("ConceptTypeChildChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("ConceptTypeDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("DefaultReferenceModeNamingCustomizesORMModelAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("DefaultReferenceModeNamingChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("FactTypeNameChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("InformationTypeFormatAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("InformationTypeFormatDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("NameGeneratorSettingsChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("RecognizedPhraseDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("ReferenceModeNamingCustomizesObjectTypeAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("ReferenceModeNamingChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("RoleNameChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("SchemaChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("TableChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("UniquenessDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("UniquenessConstraintRoleDeletingClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("UniquenessConstraintRoleOrderChangedClass", BindingFlags.Public | BindingFlags.NonPublic),
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
			if (ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InitializingToolboxItems)
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
			for (int i = 0; i < 35; ++i)
			{
				ruleManager.EnableRule(disabledRuleTypes[i]);
			}
		}
		void ORMSolutions.ORMArchitect.Framework.Shell.IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.Store store)
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasAbbreviation), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class AbbreviationAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public AbbreviationAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasAbbreviation)
				/// /// </summary>
				/// private static void AbbreviationAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.AbbreviationAddedRule");
					ModificationTracker.AbbreviationAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.AbbreviationAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.RecognizedPhraseHasAbbreviation), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class AbbreviationAddedRuleClass2 : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public AbbreviationAddedRuleClass2()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// AddRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.RecognizedPhraseHasAbbreviation)
				/// /// </summary>
				/// private static void AbbreviationAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.AbbreviationAddedRuleClass2");
					ModificationTracker.AbbreviationAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.AbbreviationAddedRuleClass2");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.NameAlias), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class AbbreviationChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public AbbreviationChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.NameAlias)
				/// /// </summary>
				/// private static void AbbreviationChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.AbbreviationChangedRule");
					ModificationTracker.AbbreviationChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.AbbreviationChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasAbbreviation), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class AbbreviationDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public AbbreviationDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ObjectTypeHasAbbreviation)
				/// /// </summary>
				/// private static void AbbreviationDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.AbbreviationDeletedRule");
					ModificationTracker.AbbreviationDeletedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.AbbreviationDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.RecognizedPhraseHasAbbreviation), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class AbbreviationDeletedRuleClass2 : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public AbbreviationDeletedRuleClass2()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.RecognizedPhraseHasAbbreviation)
				/// /// </summary>
				/// private static void AbbreviationDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.AbbreviationDeletedRuleClass2");
					ModificationTracker.AbbreviationDeletedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.AbbreviationDeletedRuleClass2");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.ORMAbstraction.AbstractionModel), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class AbstractionModelChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public AbstractionModelChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.ORMAbstraction.AbstractionModel)
				/// /// </summary>
				/// private static void AbstractionModelChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.AbstractionModelChangedRule");
					ModificationTracker.AbstractionModelChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.AbstractionModelChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(AssimilationMappingCustomizesFactType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class AssimilationMappingAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public AssimilationMappingAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
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
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.AssimilationMappingAddedRule");
					ModificationTracker.AssimilationMappingAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.AssimilationMappingAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(AssimilationMapping), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class AssimilationMappingChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public AssimilationMappingChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
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
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.AssimilationMappingChangedRule");
					ModificationTracker.AssimilationMappingChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.AssimilationMappingChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.Column), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ColumnChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ColumnChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.Column)
				/// /// </summary>
				/// private static void ColumnChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ColumnChangedRule");
					ModificationTracker.ColumnChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ColumnChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.TableContainsColumn), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ColumnOrderChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ColumnOrderChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// RolePlayerPositionChangeRule: typeof(ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.TableContainsColumn)
				/// /// </summary>
				/// private static void ColumnOrderChangedRule(RolePlayerOrderChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void RolePlayerPositionChanged(Microsoft.VisualStudio.Modeling.RolePlayerOrderChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ColumnOrderChangedRule");
					ModificationTracker.ColumnOrderChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ColumnOrderChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.ORMAbstraction.AbstractionModelHasConceptType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ConceptTypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ConceptTypeAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// AddRule: typeof(ORMSolutions.ORMArchitect.ORMAbstraction.AbstractionModelHasConceptType)
				/// /// </summary>
				/// private static void ConceptTypeAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ConceptTypeAddedRule");
					ModificationTracker.ConceptTypeAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ConceptTypeAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.ORMAbstraction.ConceptType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ConceptTypeChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ConceptTypeChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.ORMAbstraction.ConceptType)
				/// /// </summary>
				/// private static void ConceptTypeChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ConceptTypeChangedRule");
					ModificationTracker.ConceptTypeChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ConceptTypeChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.ORMAbstraction.ConceptTypeChild), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ConceptTypeChildChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ConceptTypeChildChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.ORMAbstraction.ConceptTypeChild)
				/// /// </summary>
				/// private static void ConceptTypeChildChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ConceptTypeChildChangedRule");
					ModificationTracker.ConceptTypeChildChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ConceptTypeChildChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.ORMAbstraction.AbstractionModelHasConceptType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ConceptTypeDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ConceptTypeDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.ORMAbstraction.AbstractionModelHasConceptType)
				/// /// </summary>
				/// private static void ConceptTypeDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ConceptTypeDeletedRule");
					ModificationTracker.ConceptTypeDeletedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ConceptTypeDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(DefaultReferenceModeNamingCustomizesORMModel), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class DefaultReferenceModeNamingCustomizesORMModelAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public DefaultReferenceModeNamingCustomizesORMModelAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
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
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.DefaultReferenceModeNamingCustomizesORMModelAddedRule");
					ModificationTracker.DefaultReferenceModeNamingCustomizesORMModelAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.DefaultReferenceModeNamingCustomizesORMModelAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(DefaultReferenceModeNaming), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class DefaultReferenceModeNamingChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public DefaultReferenceModeNamingChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
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
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.DefaultReferenceModeNamingChangedRule");
					ModificationTracker.DefaultReferenceModeNamingChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.DefaultReferenceModeNamingChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeNameChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeNameChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactType)
				/// /// </summary>
				/// private static void FactTypeNameChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.FactTypeNameChangedRule");
					ModificationTracker.FactTypeNameChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.FactTypeNameChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.ORMAbstraction.AbstractionModelHasInformationTypeFormat), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class InformationTypeFormatAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public InformationTypeFormatAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// AddRule: typeof(ORMSolutions.ORMArchitect.ORMAbstraction.AbstractionModelHasInformationTypeFormat)
				/// /// </summary>
				/// private static void InformationTypeFormatAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.InformationTypeFormatAddedRule");
					ModificationTracker.InformationTypeFormatAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.InformationTypeFormatAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.ORMAbstraction.AbstractionModelHasInformationTypeFormat), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class InformationTypeFormatDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public InformationTypeFormatDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.ORMAbstraction.AbstractionModelHasInformationTypeFormat)
				/// /// </summary>
				/// private static void InformationTypeFormatDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.InformationTypeFormatDeletedRule");
					ModificationTracker.InformationTypeFormatDeletedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.InformationTypeFormatDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.NameGenerator), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class NameGeneratorSettingsChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public NameGeneratorSettingsChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.NameGenerator)
				/// /// </summary>
				/// private static void NameGeneratorSettingsChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.NameGeneratorSettingsChangedRule");
					ModificationTracker.NameGeneratorSettingsChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.NameGeneratorSettingsChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelContainsRecognizedPhrase), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class RecognizedPhraseDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public RecognizedPhraseDeletingRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// DeletingRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ModelContainsRecognizedPhrase)
				/// /// </summary>
				/// private static void RecognizedPhraseDeletingRule(ElementDeletingEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.RecognizedPhraseDeletingRule");
					ModificationTracker.RecognizedPhraseDeletingRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.RecognizedPhraseDeletingRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReferenceModeNamingCustomizesObjectType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ReferenceModeNamingCustomizesObjectTypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ReferenceModeNamingCustomizesObjectTypeAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
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
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ReferenceModeNamingCustomizesObjectTypeAddedRule");
					ModificationTracker.ReferenceModeNamingCustomizesObjectTypeAddedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ReferenceModeNamingCustomizesObjectTypeAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReferenceModeNaming), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ReferenceModeNamingChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ReferenceModeNamingChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
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
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ReferenceModeNamingChangedRule");
					ModificationTracker.ReferenceModeNamingChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.ReferenceModeNamingChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Role), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class RoleNameChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public RoleNameChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.Role)
				/// /// </summary>
				/// private static void RoleNameChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.RoleNameChangedRule");
					ModificationTracker.RoleNameChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.RoleNameChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.Schema), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class SchemaChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public SchemaChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.Schema)
				/// /// </summary>
				/// private static void SchemaChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.SchemaChangedRule");
					ModificationTracker.SchemaChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.SchemaChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.Table), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class TableChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public TableChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.Table)
				/// /// </summary>
				/// private static void TableChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.TableChangedRule");
					ModificationTracker.TableChangedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.TableChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(UniquenessConstraintIsForUniqueness), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class UniquenessDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public UniquenessDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
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
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.UniquenessDeletedRule");
					ModificationTracker.UniquenessDeletedRule(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.UniquenessDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.ORMAbstraction.UniquenessIncludesConceptTypeChild), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class UniquenessConstraintRoleDeletingClass : Microsoft.VisualStudio.Modeling.DeletingRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public UniquenessConstraintRoleDeletingClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// DeletingRule: typeof(ORMSolutions.ORMArchitect.ORMAbstraction.UniquenessIncludesConceptTypeChild)
				/// /// </summary>
				/// private static void UniquenessConstraintRoleDeleting(ElementDeletingEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.UniquenessConstraintRoleDeleting");
					ModificationTracker.UniquenessConstraintRoleDeleting(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.UniquenessConstraintRoleDeleting");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.ORMAbstraction.UniquenessIncludesConceptTypeChild), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
			private sealed class UniquenessConstraintRoleOrderChangedClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public UniquenessConstraintRoleOrderChangedClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
				/// /// <summary>
				/// /// RolePlayerPositionChangeRule: typeof(ORMSolutions.ORMArchitect.ORMAbstraction.UniquenessIncludesConceptTypeChild)
				/// /// </summary>
				/// private static void UniquenessConstraintRoleOrderChanged(RolePlayerOrderChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void RolePlayerPositionChanged(Microsoft.VisualStudio.Modeling.RolePlayerOrderChangedEventArgs e)
				{
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.UniquenessConstraintRoleOrderChanged");
					ModificationTracker.UniquenessConstraintRoleOrderChanged(e);
					ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker.UniquenessConstraintRoleOrderChanged");
				}
			}
		}
	}
	#endregion // Rule classes for ORMAbstractionToConceptualDatabaseBridgeDomainModel.ModificationTracker
	#region Rule classes for AssimilationMapping
	partial class AssimilationMapping
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(AssimilationMappingCustomizesFactType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class AssimilationMappingAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public AssimilationMappingAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.AssimilationMappingAddedRule");
				AssimilationMapping.AssimilationMappingAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.AssimilationMappingAddedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(AssimilationMapping), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class AssimilationMappingChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public AssimilationMappingChangedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping
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
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.AssimilationMappingChangedRule");
				AssimilationMapping.AssimilationMappingChangedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.AssimilationMappingChangedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DisjunctiveMandatoryCouplerDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DisjunctiveMandatoryCouplerDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping
			/// /// <summary>
			/// /// DeletingRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ExclusiveOrConstraintCoupler)
			/// /// </summary>
			/// private static void DisjunctiveMandatoryCouplerDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.DisjunctiveMandatoryCouplerDeletingRule");
				AssimilationMapping.DisjunctiveMandatoryCouplerDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.DisjunctiveMandatoryCouplerDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class DisjunctiveMandatoryRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public DisjunctiveMandatoryRoleDeletedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping
			/// /// <summary>
			/// /// DeleteRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void DisjunctiveMandatoryRoleDeletedRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.DisjunctiveMandatoryRoleDeletedRule");
				AssimilationMapping.DisjunctiveMandatoryRoleDeletedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.DisjunctiveMandatoryRoleDeletedRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class FactTypeDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public FactTypeDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping
			/// /// <summary>
			/// /// DeletingRule: typeof(ORMSolutions.ORMArchitect.Core.ObjectModel.FactType)
			/// /// </summary>
			/// private static void FactTypeDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.FactTypeDeletingRule");
				AssimilationMapping.FactTypeDeletingRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.FactTypeDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.ConceptTypeChildHasPathFactType), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class PathFactTypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public PathFactTypeAddedRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping
			/// /// <summary>
			/// /// AddRule: typeof(ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.ConceptTypeChildHasPathFactType)
			/// /// </summary>
			/// private static void PathFactTypeAddedRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.PathFactTypeAddedRule");
				AssimilationMapping.PathFactTypeAddedRule(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.AssimilationMapping.PathFactTypeAddedRule");
			}
		}
	}
	#endregion // Rule classes for AssimilationMapping
	#endregion // Auto-rule classes
}
