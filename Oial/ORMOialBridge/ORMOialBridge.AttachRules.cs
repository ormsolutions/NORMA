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

namespace Neumont.Tools.ORMToORMAbstractionBridge
{
	#region Attach rules to ORMToORMAbstractionBridgeDomainModel model
	partial class ORMToORMAbstractionBridgeDomainModel : Neumont.Tools.Modeling.Shell.IDomainModelEnablesRulesAfterDeserialization
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
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("FactTypeDerivationAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("FactTypeDerivationChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("FactTypeDerivationDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("FactTypeErrorAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("FactTypeErrorDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ObjectTypeAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ObjectTypeErrorAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ObjectTypeErrorDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("PreferredIdentifierAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("RolePlayerAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("RolePlayerDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("RolePlayerRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ConceptTypeBridgeDetachedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("InformationTypeFormatBridgeDetachedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ConceptTypeChildPathBridgeDetachedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ConstraintRoleAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ConstraintRoleDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ObjectTypeChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ORMModelChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("PreferredIdentifierDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("PreferredIdentifierRolePlayerChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
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
			if (Neumont.Tools.Modeling.FrameworkDomainModel.InitializingToolboxItems)
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
			for (int i = 0; i < 29; ++i)
			{
				ruleManager.EnableRule(disabledRuleTypes[i]);
			}
		}
		void Neumont.Tools.Modeling.Shell.IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.Store store)
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ExcludedORMModelElement), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ElementExclusionAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ElementExclusionAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
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
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.ElementExclusionAddedRule");
					ORMElementGateway.ElementExclusionAddedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.ElementExclusionAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ModelHasFactType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ModelHasFactType)
				/// /// </summary>
				/// private static void FactTypeAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeAddedRule");
					ORMElementGateway.FactTypeAddedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasDerivationExpression), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeDerivationAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeDerivationAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasDerivationExpression)
				/// /// </summary>
				/// private static void FactTypeDerivationAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeDerivationAddedRule");
					ORMElementGateway.FactTypeDerivationAddedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeDerivationAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FactTypeDerivationExpression), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeDerivationChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeDerivationChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.FactTypeDerivationExpression)
				/// /// </summary>
				/// private static void FactTypeDerivationChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeDerivationChangedRule");
					ORMElementGateway.FactTypeDerivationChangedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeDerivationChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasDerivationExpression), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeDerivationDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeDerivationDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasDerivationExpression)
				/// /// </summary>
				/// private static void FactTypeDerivationDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeDerivationDeletedRule");
					ORMElementGateway.FactTypeDerivationDeletedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeDerivationDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasImpliedInternalUniquenessConstraintError), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasFactTypeRequiresInternalUniquenessConstraintError), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeErrorAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeErrorAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasImpliedInternalUniquenessConstraintError)
				/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasFactTypeRequiresInternalUniquenessConstraintError)
				/// /// </summary>
				/// private static void FactTypeErrorAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeErrorAddedRule");
					ORMElementGateway.FactTypeErrorAddedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeErrorAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasImpliedInternalUniquenessConstraintError), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasFactTypeRequiresInternalUniquenessConstraintError), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class FactTypeErrorDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public FactTypeErrorDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasImpliedInternalUniquenessConstraintError)
				/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasFactTypeRequiresInternalUniquenessConstraintError)
				/// /// </summary>
				/// private static void FactTypeErrorDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeErrorDeletedRule");
					ORMElementGateway.FactTypeErrorDeletedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.FactTypeErrorDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ModelHasObjectType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ObjectTypeAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ObjectTypeAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ModelHasObjectType)
				/// /// </summary>
				/// private static void ObjectTypeAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.ObjectTypeAddedRule");
					ORMElementGateway.ObjectTypeAddedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.ObjectTypeAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasEntityTypeRequiresReferenceSchemeError), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasCompatibleSupertypesError), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasPreferredIdentifierRequiresMandatoryError), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasObjectTypeRequiresPrimarySupertypeError), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ValueTypeHasUnspecifiedDataTypeError), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ObjectTypeErrorAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ObjectTypeErrorAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasEntityTypeRequiresReferenceSchemeError)
				/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasCompatibleSupertypesError)
				/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasPreferredIdentifierRequiresMandatoryError)
				/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasObjectTypeRequiresPrimarySupertypeError)
				/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ValueTypeHasUnspecifiedDataTypeError)
				/// /// </summary>
				/// private static void ObjectTypeErrorAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.ObjectTypeErrorAddedRule");
					ORMElementGateway.ObjectTypeErrorAddedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.ObjectTypeErrorAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasEntityTypeRequiresReferenceSchemeError), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasCompatibleSupertypesError), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasPreferredIdentifierRequiresMandatoryError), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasObjectTypeRequiresPrimarySupertypeError), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ValueTypeHasUnspecifiedDataTypeError), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ObjectTypeErrorDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ObjectTypeErrorDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasEntityTypeRequiresReferenceSchemeError)
				/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasCompatibleSupertypesError)
				/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasPreferredIdentifierRequiresMandatoryError)
				/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasObjectTypeRequiresPrimarySupertypeError)
				/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ValueTypeHasUnspecifiedDataTypeError)
				/// /// </summary>
				/// private static void ObjectTypeErrorDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.ObjectTypeErrorDeletedRule");
					ORMElementGateway.ObjectTypeErrorDeletedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.ObjectTypeErrorDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class PreferredIdentifierAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public PreferredIdentifierAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier)
				/// /// </summary>
				/// private static void PreferredIdentifierAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.PreferredIdentifierAddedRule");
					ORMElementGateway.PreferredIdentifierAddedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.PreferredIdentifierAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class RolePlayerAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public RolePlayerAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole)
				/// /// </summary>
				/// private static void RolePlayerAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.RolePlayerAddedRule");
					ORMElementGateway.RolePlayerAddedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.RolePlayerAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class RolePlayerDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public RolePlayerDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole)
				/// /// </summary>
				/// private static void RolePlayerDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.RolePlayerDeletedRule");
					ORMElementGateway.RolePlayerDeletedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.RolePlayerDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class RolePlayerRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public RolePlayerRolePlayerChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway
				/// /// <summary>
				/// /// RolePlayerChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole)
				/// /// </summary>
				/// private static void RolePlayerRolePlayerChangedRule(RolePlayerChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.RolePlayerRolePlayerChangedRule");
					ORMElementGateway.RolePlayerRolePlayerChangedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ORMElementGateway.RolePlayerRolePlayerChangedRule");
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConceptTypeIsForObjectType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ConceptTypeBridgeDetachedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ConceptTypeBridgeDetachedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
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
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ConceptTypeBridgeDetachedRule");
					ModificationTracker.ConceptTypeBridgeDetachedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ConceptTypeBridgeDetachedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(InformationTypeFormatIsForValueType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class InformationTypeFormatBridgeDetachedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public InformationTypeFormatBridgeDetachedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
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
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.InformationTypeFormatBridgeDetachedRule");
					ModificationTracker.InformationTypeFormatBridgeDetachedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.InformationTypeFormatBridgeDetachedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConceptTypeChildHasPathFactType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ConceptTypeChildPathBridgeDetachedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ConceptTypeChildPathBridgeDetachedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
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
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ConceptTypeChildPathBridgeDetachedRule");
					ModificationTracker.ConceptTypeChildPathBridgeDetachedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ConceptTypeChildPathBridgeDetachedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ConstraintRoleAddedRuleClass : Microsoft.VisualStudio.Modeling.AddRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ConstraintRoleAddedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole)
				/// /// </summary>
				/// private static void ConstraintRoleAddedRule(ElementAddedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ConstraintRoleAddedRule");
					ModificationTracker.ConstraintRoleAddedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ConstraintRoleAddedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ConstraintRoleDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ConstraintRoleDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole)
				/// /// </summary>
				/// private static void ConstraintRoleDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ConstraintRoleDeletedRule");
					ModificationTracker.ConstraintRoleDeletedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ConstraintRoleDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ObjectTypeChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ObjectTypeChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectType)
				/// /// </summary>
				/// private static void ObjectTypeChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ObjectTypeChangedRule");
					ModificationTracker.ObjectTypeChangedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ObjectTypeChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ORMModel), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class ORMModelChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public ORMModelChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.ORMModel)
				/// /// </summary>
				/// private static void ORMModelChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ORMModelChangedRule");
					ModificationTracker.ORMModelChangedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.ORMModelChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class PreferredIdentifierDeletedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public PreferredIdentifierDeletedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier)
				/// /// </summary>
				/// private static void PreferredIdentifierDeletedRule(ElementDeletedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.PreferredIdentifierDeletedRule");
					ModificationTracker.PreferredIdentifierDeletedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.PreferredIdentifierDeletedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class PreferredIdentifierRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public PreferredIdentifierRolePlayerChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// RolePlayerChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.EntityTypeHasPreferredIdentifier)
				/// /// </summary>
				/// private static void PreferredIdentifierRolePlayerChangedRule(RolePlayerChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.PreferredIdentifierRolePlayerChangedRule");
					ModificationTracker.PreferredIdentifierRolePlayerChangedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.PreferredIdentifierRolePlayerChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class RolePlayerRolePlayerChangedRuleClass : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public RolePlayerRolePlayerChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// RolePlayerChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole)
				/// /// </summary>
				/// private static void RolePlayerRolePlayerChangedRule(RolePlayerChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void RolePlayerChanged(Microsoft.VisualStudio.Modeling.RolePlayerChangedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ElementLink.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.RolePlayerRolePlayerChangedRule");
					ModificationTracker.RolePlayerRolePlayerChangedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ElementLink.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.RolePlayerRolePlayerChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.SetConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class SetConstraintChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public SetConstraintChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.SetConstraint)
				/// /// </summary>
				/// private static void SetConstraintChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.SetConstraintChangedRule");
					ModificationTracker.SetConstraintChangedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.SetConstraintChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.SubtypeFact), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class SubtypeFactChangedRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public SubtypeFactChangedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.SubtypeFact)
				/// /// </summary>
				/// private static void SubtypeFactChangedRule(ElementPropertyChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.SubtypeFactChangedRule");
					ModificationTracker.SubtypeFactChangedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.SubtypeFactChangedRule");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class UniquenessConstraintRoleOrderChangedClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public UniquenessConstraintRoleOrderChangedClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// RolePlayerPositionChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole)
				/// /// </summary>
				/// private static void UniquenessConstraintRoleOrderChanged(RolePlayerOrderChangedEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void RolePlayerPositionChanged(Microsoft.VisualStudio.Modeling.RolePlayerOrderChangedEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.UniquenessConstraintRoleOrderChanged");
					ModificationTracker.UniquenessConstraintRoleOrderChanged(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.UniquenessConstraintRoleOrderChanged");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class UniquenessConstraintRoleDeletingClass : Microsoft.VisualStudio.Modeling.DeletingRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public UniquenessConstraintRoleDeletingClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
				/// /// <summary>
				/// /// DeletingRule: typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole)
				/// /// </summary>
				/// private static void UniquenessConstraintRoleDeleting(ElementDeletingEventArgs e)
				/// {
				/// }
				/// </summary>
				[System.Diagnostics.DebuggerStepThrough()]
				public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
				{
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.UniquenessConstraintRoleDeleting");
					ModificationTracker.UniquenessConstraintRoleDeleting(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.UniquenessConstraintRoleDeleting");
				}
			}
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(UniquenessIsForUniquenessConstraint), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
			private sealed class UniquenessBridgeDetachedRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
			{
				[System.Diagnostics.DebuggerStepThrough()]
				public UniquenessBridgeDetachedRuleClass()
				{
					base.IsEnabled = false;
				}
				/// <summary>
				/// Provide the following method in class: 
				/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker
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
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.UniquenessBridgeDetachedRule");
					ModificationTracker.UniquenessBridgeDetachedRule(e);
					Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ModificationTracker.UniquenessBridgeDetachedRule");
				}
			}
		}
	}
	#endregion // Rule classes for AbstractionModelIsForORMModel.ModificationTracker
	#endregion // Auto-rule classes
}
