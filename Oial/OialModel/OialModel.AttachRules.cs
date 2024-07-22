﻿using System;
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

namespace ORMSolutions.ORMArchitect.ORMAbstraction
{
	#region Attach rules to AbstractionDomainModel model
	partial class AbstractionDomainModel
	{
		private static Type[] myCustomDomainModelTypes;
		private static Type[] CustomDomainModelTypes
		{
			get
			{
				Type[] retVal = AbstractionDomainModel.myCustomDomainModelTypes;
				if (retVal == null)
				{
					// No synchronization is needed here.
					// If accessed concurrently, the worst that will happen is the array of Types being created multiple times.
					// This would have a slightly negative impact on performance, but the result would still be correct.
					// Given the low likelihood of this ever happening, the extra overhead of synchronization would outweigh any possible gain from it.
					retVal = new Type[]{
						typeof(EnsureUnaryFormatClass)};
					AbstractionDomainModel.myCustomDomainModelTypes = retVal;
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
			Type[] customDomainModelTypes = AbstractionDomainModel.CustomDomainModelTypes;
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
	#endregion // Attach rules to AbstractionDomainModel model
	#region Auto-rule classes
	#region Rule classes for AbstractionDomainModel
	partial class AbstractionDomainModel
	{
		[Microsoft.VisualStudio.Modeling.RuleOn(typeof(AbstractionModelHasInformationTypeFormat), Priority=ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel.InlineRulePriority)]
		private sealed class EnsureUnaryFormatClass : Microsoft.VisualStudio.Modeling.AddRule
		{
			/// <summary>
			/// Provide the following method in class: 
			/// ORMSolutions.ORMArchitect.ORMAbstraction.AbstractionDomainModel
			/// /// <summary>
			/// /// AddRule: typeof(AbstractionModelHasInformationTypeFormat)
			/// /// </summary>
			/// private static void EnsureUnaryFormat(ElementAddedEventArgs e)
			/// {
			/// }
			/// </summary>
			[System.Diagnostics.DebuggerStepThrough()]
			public override void ElementAdded(Microsoft.VisualStudio.Modeling.ElementAddedEventArgs e)
			{
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleStart(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstraction.AbstractionDomainModel.EnsureUnaryFormat");
				AbstractionDomainModel.EnsureUnaryFormat(e);
				ORMSolutions.ORMArchitect.Framework.Diagnostics.TraceUtility.TraceRuleEnd(e.ModelElement.Store, "ORMSolutions.ORMArchitect.ORMAbstraction.AbstractionDomainModel.EnsureUnaryFormat");
			}
		}
	}
	#endregion // Rule classes for AbstractionDomainModel
	#endregion // Auto-rule classes
}
