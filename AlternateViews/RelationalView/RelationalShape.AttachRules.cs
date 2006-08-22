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

namespace Neumont.Tools.ORM.Views.RelationalView
{
	#region Attach rules to RelationalShapeDomainModel model
	internal partial class RelationalShapeDomainModel : Neumont.Tools.ORM.ObjectModel.IDomainModelEnablesRulesAfterDeserialization
	{
		private static Type[] myCustomDomainModelTypes;
		private static Type[] CustomDomainModelTypes
		{
			get
			{
				Type[] retVal = RelationalShapeDomainModel.myCustomDomainModelTypes;
				if (retVal == null)
				{
					// No synchronization is needed here.
					// If accessed concurrently, the worst that will happen is the array of Types being created multiple times.
					// This would have a slightly negative impact on performance, but the result would still be correct.
					// Given the low likelihood of this ever happening, the extra overhead of synchronization would outweigh any possible gain from it.
					retVal = new Type[]{
						typeof(RelationalModel).GetNestedType("DelayedFixUpDiagram", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RelationalModel).GetNestedType("DelayedCompartmentItemAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RelationalModel).GetNestedType("DelayedOIALModelAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RelationalModel).GetNestedType("DelayedConceptTypeAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RelationalModel).GetNestedType("DelayedForeignKeyItemAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RelationalModel).GetNestedType("DelayedRelationalDiagramAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RelationalModel).GetNestedType("RelationalDiagramDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ColumnElementListCompartment)};
					RelationalShapeDomainModel.myCustomDomainModelTypes = retVal;
					System.Diagnostics.Debug.Assert(Array.IndexOf<Type>(retVal, null) < 0, "One or more rule types failed to resolve. The file and/or package will fail to load.");
				}
				return retVal;
			}
		}
		private static Type[] myInitiallyDisabledRuleTypes;
		private static Type[] InitiallyDisabledRuleTypes
		{
			get
			{
				Type[] retVal = RelationalShapeDomainModel.myInitiallyDisabledRuleTypes;
				if (retVal == null)
				{
					Type[] customDomainModelTypes = RelationalShapeDomainModel.CustomDomainModelTypes;
					retVal = new Type[]{
						customDomainModelTypes[0],
						customDomainModelTypes[1],
						customDomainModelTypes[2],
						customDomainModelTypes[3],
						customDomainModelTypes[4],
						customDomainModelTypes[5],
						customDomainModelTypes[6],
						customDomainModelTypes[7]};
					RelationalShapeDomainModel.myInitiallyDisabledRuleTypes = retVal;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Generated code to attach <see cref="Microsoft.VisualStudio.Modeling.Rule"/>s to the <see cref="Microsoft.VisualStudio.Modeling.Store"/>.
		/// </summary>
		/// <seealso cref="Microsoft.VisualStudio.Modeling.DomainModel.GetCustomDomainModelTypes">
		/// 
		/// </seealso>
		protected override Type[] GetCustomDomainModelTypes()
		{
			if (Neumont.Tools.ORM.ObjectModel.ORMCoreDomainModel.InitializingToolboxItems)
			{
				return Type.EmptyTypes;
			}
			Type[] retVal = base.GetCustomDomainModelTypes();
			int baseLength = retVal.Length;
			Type[] customDomainModelTypes = RelationalShapeDomainModel.CustomDomainModelTypes;
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
		/// <summary>
		/// Implements IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization
		/// </summary>
		protected void EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.RuleManager ruleManager)
		{
			Type[] disabledRuleTypes = RelationalShapeDomainModel.InitiallyDisabledRuleTypes;
			int count = disabledRuleTypes.Length;
			for (int i = 0; i < count; ++i)
			{
				ruleManager.EnableRule(disabledRuleTypes[i]);
			}
		}
		void Neumont.Tools.ORM.ObjectModel.IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.RuleManager ruleManager)
		{
			this.EnableRulesAfterDeserialization(ruleManager);
		}
	}
	#endregion // Attach rules to RelationalShapeDomainModel model
	#region Initially disable rules
	internal partial class RelationalModel
	{
		private partial class DelayedFixUpDiagram
		{
			public DelayedFixUpDiagram()
			{
				base.IsEnabled = false;
			}
		}
	}
	internal partial class RelationalModel
	{
		private partial class DelayedCompartmentItemAddRule
		{
			public DelayedCompartmentItemAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	internal partial class RelationalModel
	{
		private partial class DelayedOIALModelAddedRule
		{
			public DelayedOIALModelAddedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	internal partial class RelationalModel
	{
		private partial class DelayedConceptTypeAddedRule
		{
			public DelayedConceptTypeAddedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	internal partial class RelationalModel
	{
		private partial class DelayedForeignKeyItemAddRule
		{
			public DelayedForeignKeyItemAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	internal partial class RelationalModel
	{
		private partial class DelayedRelationalDiagramAddRule
		{
			public DelayedRelationalDiagramAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	internal partial class RelationalModel
	{
		private partial class RelationalDiagramDeleteRule
		{
			public RelationalDiagramDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	#endregion // Initially disable rules
}
