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

namespace Neumont.Tools.ORM.ExtensionExample
{
	#region Attach rules to ExtensionDomainModel model
	partial class ExtensionDomainModel : Neumont.Tools.Modeling.Shell.IDomainModelEnablesRulesAfterDeserialization
	{
		private static Type[] myCustomDomainModelTypes;
		private static Type[] CustomDomainModelTypes
		{
			get
			{
				Type[] retVal = ExtensionDomainModel.myCustomDomainModelTypes;
				if (retVal == null)
				{
					// No synchronization is needed here.
					// If accessed concurrently, the worst that will happen is the array of Types being created multiple times.
					// This would have a slightly negative impact on performance, but the result would still be correct.
					// Given the low likelihood of this ever happening, the extra overhead of synchronization would outweigh any possible gain from it.
					retVal = new Type[]{
						typeof(MyCustomExtensionElement).GetNestedType("RoleAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeRequiresMeaningfulNameError).GetNestedType("ExtensionObjectTypeAddRuleClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeRequiresMeaningfulNameError).GetNestedType("ExtensionObjectTypeChangeRuleClass", BindingFlags.Public | BindingFlags.NonPublic)};
					ExtensionDomainModel.myCustomDomainModelTypes = retVal;
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
			Type[] customDomainModelTypes = ExtensionDomainModel.CustomDomainModelTypes;
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
			Type[] disabledRuleTypes = ExtensionDomainModel.CustomDomainModelTypes;
			for (int i = 0; i < 3; ++i)
			{
				ruleManager.EnableRule(disabledRuleTypes[i]);
			}
		}
		void Neumont.Tools.Modeling.Shell.IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.Store store)
		{
			this.EnableRulesAfterDeserialization(store);
		}
	}
	#endregion // Attach rules to ExtensionDomainModel model
	#region Auto-rule classes
	#region Rule classes for MyCustomExtensionElement
	partial class MyCustomExtensionElement
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasRole), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class RoleAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public RoleAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ExtensionExample.MyCustomExtensionElement
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.FactTypeHasRole)
			/// /// </summary>
			/// private static void RoleAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ExtensionExample.MyCustomExtensionElement.RoleAddRule");
				MyCustomExtensionElement.RoleAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ExtensionExample.MyCustomExtensionElement.RoleAddRule");
			}
		}
	}
	#endregion // Rule classes for MyCustomExtensionElement
	#region Rule classes for ObjectTypeRequiresMeaningfulNameError
	partial class ObjectTypeRequiresMeaningfulNameError
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ModelHasObjectType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ExtensionObjectTypeAddRuleClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ExtensionObjectTypeAddRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ExtensionExample.ObjectTypeRequiresMeaningfulNameError
			/// /// <summary>
			/// /// AddRule: typeof(Neumont.Tools.ORM.ObjectModel.ModelHasObjectType)
			/// /// </summary>
			/// private static void ExtensionObjectTypeAddRule(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ExtensionExample.ObjectTypeRequiresMeaningfulNameError.ExtensionObjectTypeAddRule");
				ObjectTypeRequiresMeaningfulNameError.ExtensionObjectTypeAddRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ExtensionExample.ObjectTypeRequiresMeaningfulNameError.ExtensionObjectTypeAddRule");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(Neumont.Tools.ORM.ObjectModel.ObjectType), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ExtensionObjectTypeChangeRuleClass : Microsoft.VisualStudio.Modeling.ChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ExtensionObjectTypeChangeRuleClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.ORM.ExtensionExample.ObjectTypeRequiresMeaningfulNameError
			/// /// <summary>
			/// /// ChangeRule: typeof(Neumont.Tools.ORM.ObjectModel.ObjectType)
			/// /// </summary>
			/// private static void ExtensionObjectTypeChangeRule(ElementPropertyChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementPropertyChanged(Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.ORM.ExtensionExample.ObjectTypeRequiresMeaningfulNameError.ExtensionObjectTypeChangeRule");
				ObjectTypeRequiresMeaningfulNameError.ExtensionObjectTypeChangeRule(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.ORM.ExtensionExample.ObjectTypeRequiresMeaningfulNameError.ExtensionObjectTypeChangeRule");
			}
		}
	}
	#endregion // Rule classes for ObjectTypeRequiresMeaningfulNameError
	#endregion // Auto-rule classes
}
