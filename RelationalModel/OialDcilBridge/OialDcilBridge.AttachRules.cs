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
						typeof(ModificationTracker).GetNestedType("ConceptTypeAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("ConceptTypeDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("InformationTypeFormatAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("InformationTypeFormatDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModificationTracker).GetNestedType("UniquenessDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic)};
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
			for (int i = 0; i < 6; ++i)
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORMAbstraction.AbstractionModel))]
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORMAbstraction.AbstractionModelHasConceptType))]
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORMAbstraction.AbstractionModelHasConceptType))]
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORMAbstraction.AbstractionModelHasInformationTypeFormat))]
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORMAbstraction.AbstractionModelHasInformationTypeFormat))]
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(UniquenessConstraintIsForUniqueness))]
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
	#endregion // Auto-rule classes
}
