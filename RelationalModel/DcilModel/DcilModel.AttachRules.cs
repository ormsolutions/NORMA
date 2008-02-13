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

namespace Neumont.Tools.RelationalModels.ConceptualDatabase
{
	#region Attach rules to ConceptualDatabaseDomainModel model
	partial class ConceptualDatabaseDomainModel : Neumont.Tools.Modeling.Shell.IDomainModelEnablesRulesAfterDeserialization
	{
		private static Type[] myCustomDomainModelTypes;
		private static Type[] CustomDomainModelTypes
		{
			get
			{
				Type[] retVal = ConceptualDatabaseDomainModel.myCustomDomainModelTypes;
				if (retVal == null)
				{
					// No synchronization is needed here.
					// If accessed concurrently, the worst that will happen is the array of Types being created multiple times.
					// This would have a slightly negative impact on performance, but the result would still be correct.
					// Given the low likelihood of this ever happening, the extra overhead of synchronization would outweigh any possible gain from it.
					retVal = new Type[]{
						typeof(ReferenceConstraint).GetNestedType("ReferenceConstraintTargetAddedClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReferenceConstraint).GetNestedType("UniquenessConstraintColumnAddedClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReferenceConstraint).GetNestedType("UniquenessConstraintColumnDeletingClass", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReferenceConstraint).GetNestedType("UniquenessConstraintColumnPositionChangedClass", BindingFlags.Public | BindingFlags.NonPublic)};
					ConceptualDatabaseDomainModel.myCustomDomainModelTypes = retVal;
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
			Type[] customDomainModelTypes = ConceptualDatabaseDomainModel.CustomDomainModelTypes;
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
			Type[] disabledRuleTypes = ConceptualDatabaseDomainModel.CustomDomainModelTypes;
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
	#endregion // Attach rules to ConceptualDatabaseDomainModel model
	#region Auto-rule classes
	#region Rule classes for ReferenceConstraint
	partial class ReferenceConstraint
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(ReferenceConstraintTargetsTable), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class ReferenceConstraintTargetAddedClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public ReferenceConstraintTargetAddedClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.RelationalModels.ConceptualDatabase.ReferenceConstraint
			/// /// <summary>
			/// /// AddRule: typeof(ReferenceConstraintTargetsTable)
			/// /// </summary>
			/// private static void ReferenceConstraintTargetAdded(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.RelationalModels.ConceptualDatabase.ReferenceConstraint.ReferenceConstraintTargetAdded");
				ReferenceConstraint.ReferenceConstraintTargetAdded(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.RelationalModels.ConceptualDatabase.ReferenceConstraint.ReferenceConstraintTargetAdded");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(UniquenessConstraintIncludesColumn), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class UniquenessConstraintColumnAddedClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public UniquenessConstraintColumnAddedClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.RelationalModels.ConceptualDatabase.ReferenceConstraint
			/// /// <summary>
			/// /// AddRule: typeof(UniquenessConstraintIncludesColumn)
			/// /// </summary>
			/// private static void UniquenessConstraintColumnAdded(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.RelationalModels.ConceptualDatabase.ReferenceConstraint.UniquenessConstraintColumnAdded");
				ReferenceConstraint.UniquenessConstraintColumnAdded(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.RelationalModels.ConceptualDatabase.ReferenceConstraint.UniquenessConstraintColumnAdded");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(UniquenessConstraintIncludesColumn), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class UniquenessConstraintColumnDeletingClass : Microsoft.VisualStudio.Modeling.DeletingRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public UniquenessConstraintColumnDeletingClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.RelationalModels.ConceptualDatabase.ReferenceConstraint
			/// /// <summary>
			/// /// DeletingRule: typeof(UniquenessConstraintIncludesColumn)
			/// /// </summary>
			/// private static void UniquenessConstraintColumnDeleting(ElementDeletingEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementDeleting(Microsoft.VisualStudio.Modeling.ElementDeletingEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "Neumont.Tools.RelationalModels.ConceptualDatabase.ReferenceConstraint.UniquenessConstraintColumnDeleting");
				ReferenceConstraint.UniquenessConstraintColumnDeleting(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "Neumont.Tools.RelationalModels.ConceptualDatabase.ReferenceConstraint.UniquenessConstraintColumnDeleting");
			}
		}
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(UniquenessConstraintIncludesColumn), Priority=Neumont.Tools.Modeling.FrameworkDomainModel.InlineRulePriority)]
		private sealed class UniquenessConstraintColumnPositionChangedClass : Microsoft.VisualStudio.Modeling.RolePlayerPositionChangeRule
		{
			[System.Diagnostics.DebuggerStepThrough()]
			public UniquenessConstraintColumnPositionChangedClass()
			{
				base.IsEnabled = false;
			}
			/// <summary>
			/// Provide the following method in class: 
			/// Neumont.Tools.RelationalModels.ConceptualDatabase.ReferenceConstraint
			/// /// <summary>
			/// /// RolePlayerPositionChangeRule: typeof(UniquenessConstraintIncludesColumn)
			/// /// </summary>
			/// private static void UniquenessConstraintColumnPositionChanged(RolePlayerOrderChangedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void RolePlayerPositionChanged(Microsoft.VisualStudio.Modeling.RolePlayerOrderChangedEventArgs e)
			{
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleStart(e.SourceElement.Store, "Neumont.Tools.RelationalModels.ConceptualDatabase.ReferenceConstraint.UniquenessConstraintColumnPositionChanged");
				ReferenceConstraint.UniquenessConstraintColumnPositionChanged(e);
				Neumont.Tools.Modeling.Diagnostics.TraceUtility.TraceRuleEnd(e.SourceElement.Store, "Neumont.Tools.RelationalModels.ConceptualDatabase.ReferenceConstraint.UniquenessConstraintColumnPositionChanged");
			}
		}
	}
	#endregion // Rule classes for ReferenceConstraint
	#endregion // Auto-rule classes
}
