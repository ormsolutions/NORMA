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
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("FactTypeErrorAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("FactTypeErrorDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ObjectTypeAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ObjectTypeErrorAddedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ORMElementGateway", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ObjectTypeErrorDeletedRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
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
						typeof(AbstractionModelIsForORMModel).GetNestedType("ModificationTracker", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("SetConstraintChangedRuleClass", BindingFlags.Public | BindingFlags.NonPublic)};
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
			for (int i = 0; i < 18; ++i)
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ExcludedORMModelElement))]
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ModelHasFactType))]
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasImpliedInternalUniquenessConstraintError))]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasFactTypeRequiresInternalUniquenessConstraintError))]
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasImpliedInternalUniquenessConstraintError))]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasFactTypeRequiresInternalUniquenessConstraintError))]
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ModelHasObjectType))]
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasEntityTypeRequiresReferenceSchemeError))]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasCompatibleSupertypesError))]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasPreferredIdentifierRequiresMandatoryError))]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasObjectTypeRequiresPrimarySupertypeError))]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ValueTypeHasUnspecifiedDataTypeError))]
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasEntityTypeRequiresReferenceSchemeError))]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasCompatibleSupertypesError))]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasPreferredIdentifierRequiresMandatoryError))]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypeHasObjectTypeRequiresPrimarySupertypeError))]
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ValueTypeHasUnspecifiedDataTypeError))]
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole))]
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole))]
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectTypePlaysRole))]
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConceptTypeIsForObjectType))]
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(InformationTypeFormatIsForValueType))]
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ConceptTypeChildHasPathFactType))]
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole))]
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole))]
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectType))]
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ORMModel))]
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
			[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.SetConstraint))]
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
		}
	}
	#endregion // Rule classes for AbstractionModelIsForORMModel.ModificationTracker
	#endregion // Auto-rule classes
}
