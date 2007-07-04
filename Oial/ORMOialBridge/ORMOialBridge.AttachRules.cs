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
	partial class ORMToORMAbstractionBridgeDomainModel : Neumont.Tools.ORM.ObjectModel.IDomainModelEnablesRulesAfterDeserialization
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
						typeof(AbstractionModelIsForORMModel).GetNestedType("ConstraintAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ConstraintChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ConstraintDeleteRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ConstraintRoleAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ConstraintRoleDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ValueTypeDataTypeAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ValueTypeDataTypeDeletingRuleClass", BindingFlags.Public | BindingFlags.NonPublic)};
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
			for (int i = 0; i < 7; ++i)
			{
				ruleManager.EnableRule(disabledRuleTypes[i]);
			}
		}
		void Neumont.Tools.ORM.ObjectModel.IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.Store store)
		{
			this.EnableRulesAfterDeserialization(store);
		}
	}
	#endregion // Attach rules to ORMToORMAbstractionBridgeDomainModel model
	#region Auto-rule classes
	#region Rule classes for AbstractionModelIsForORMModel
	partial class AbstractionModelIsForORMModel
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ModelHasSetConstraint))]
		private sealed class ConstraintAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public ConstraintAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ModelHasSetConstraint)
			/// /// </summary>
			/// private static void ConstraintAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ConstraintAddRule");
				AbstractionModelIsForORMModel.ConstraintAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ConstraintAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.SetConstraint))]
		private sealed class ConstraintChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			public ConstraintChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel
			/// /// <summary>
			/// /// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.SetConstraint)
			/// /// </summary>
			/// private static void ConstraintChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ConstraintChangeRule");
				AbstractionModelIsForORMModel.ConstraintChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ConstraintChangeRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ModelHasSetConstraint))]
		private sealed class ConstraintDeleteRuleClass : Microsoft.VisualStudio.Modeling.DeleteRule
		{
			public ConstraintDeleteRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel
			/// /// <summary>
			/// /// DeleteRule: typeof(Neumont.Tools.ORM.ObjectModel.ModelHasSetConstraint)
			/// /// </summary>
			/// private static void ConstraintDeleteRule(ElementDeletedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementDeleted(Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ConstraintDeleteRule");
				AbstractionModelIsForORMModel.ConstraintDeleteRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ConstraintDeleteRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole))]
		private sealed class ConstraintRoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public ConstraintRoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void ConstraintRoleAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ConstraintRoleAddRule");
				AbstractionModelIsForORMModel.ConstraintRoleAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ConstraintRoleAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole))]
		private sealed class ConstraintRoleDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			public ConstraintRoleDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel
			/// /// <summary>
			/// /// DeletingRule: typeof(Neumont.Tools.ORM.ObjectModel.ConstraintRoleSequenceHasRole)
			/// /// </summary>
			/// private static void ConstraintRoleDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ConstraintRoleDeletingRule");
				AbstractionModelIsForORMModel.ConstraintRoleDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ConstraintRoleDeletingRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ValueTypeHasDataType))]
		private sealed class ValueTypeDataTypeAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			public ValueTypeDataTypeAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ValueTypeHasDataType)
			/// /// </summary>
			/// private static void ValueTypeDataTypeAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ValueTypeDataTypeAddRule");
				AbstractionModelIsForORMModel.ValueTypeDataTypeAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ValueTypeDataTypeAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ValueTypeHasDataType))]
		private sealed class ValueTypeDataTypeDeletingRuleClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			public ValueTypeDataTypeDeletingRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel
			/// /// <summary>
			/// /// DeletingRule: typeof(Neumont.Tools.ORM.ObjectModel.ValueTypeHasDataType)
			/// /// </summary>
			/// private static void ValueTypeDataTypeDeletingRule(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ValueTypeDataTypeDeletingRule");
				AbstractionModelIsForORMModel.ValueTypeDataTypeDeletingRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORMToORMAbstractionBridge.AbstractionModelIsForORMModel.ValueTypeDataTypeDeletingRule");
			}
		}
	}
	#endregion // Rule classes for AbstractionModelIsForORMModel
	#endregion // Auto-rule classes
}
