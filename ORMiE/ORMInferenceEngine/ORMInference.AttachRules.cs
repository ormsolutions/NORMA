using System;
using System.Reflection;

// Copyright Notice
// /**************************************************************************\
// * Free University Bozen-Bolzano                                            *
// \**************************************************************************/

namespace unibz.ORMInferenceEngine
{
	#region Attach rules to ORMInferenceEngineDomainModel model
	partial class ORMInferenceEngineDomainModel
	{
		private static Type[] myCustomDomainModelTypes;
		private static Type[] CustomDomainModelTypes
		{
			get
			{
				Type[] retVal = ORMInferenceEngineDomainModel.myCustomDomainModelTypes;
				if (retVal == null)
				{
					// No synchronization is needed here.
					// If accessed concurrently, the worst that will happen is the array of Types being created multiple times.
					// This would have a slightly negative impact on performance, but the result would still be correct.
					// Given the low likelihood of this ever happening, the extra overhead of synchronization would outweigh any possible gain from it.
					retVal = new Type[]{
						typeof(InferredSubtypeFact).GetNestedType("InitializeInferredSubtypeFactClass", BindingFlags.Public | BindingFlags.NonPublic)};
					ORMInferenceEngineDomainModel.myCustomDomainModelTypes = retVal;
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
			Type[] customDomainModelTypes = ORMInferenceEngineDomainModel.CustomDomainModelTypes;
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
	}
	#endregion // Attach rules to ORMInferenceEngineDomainModel model
	#region Auto-rule classes
	#region Rule classes for InferredSubtypeFact
	partial class InferredSubtypeFact
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(SubtypeFactIsInferred), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class InitializeInferredSubtypeFactClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			/// <summary>
			/// Provide the following method in class: 
			/// unibz.ORMInferenceEngine.InferredSubtypeFact
			/// /// <summary>
			/// /// AddRule: typeof(SubtypeFactIsInferred)
			/// /// </summary>
			/// private static void InitializeInferredSubtypeFact(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "unibz.ORMInferenceEngine.InferredSubtypeFact.InitializeInferredSubtypeFact");
				InferredSubtypeFact.InitializeInferredSubtypeFact(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "unibz.ORMInferenceEngine.InferredSubtypeFact.InitializeInferredSubtypeFact");
			}
		}
	}
	#endregion // Rule classes for InferredSubtypeFact
	#endregion // Auto-rule classes
}
