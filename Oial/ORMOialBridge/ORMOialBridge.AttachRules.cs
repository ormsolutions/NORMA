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
						typeof(AbstractionModelIsForORMModel).GetNestedType("ConstraintRule", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ConstraintAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ConstraintRule", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ConstraintRule", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ConstraintDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ObjectTypeRule", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ValueTypeDataTypeAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(AbstractionModelIsForORMModel).GetNestedType("ObjectTypeRule", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ValueTypeDataTypeDeleteRule", BindingFlags.Public | BindingFlags.NonPublic)};
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
			if (Neumont.Tools.ORM.ObjectModel.ORMCoreDomainModel.InitializingToolboxItems)
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
			for (int i = 0; i < 5; ++i)
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
	#region Initially disable rules
	partial class AbstractionModelIsForORMModel
	{
		partial class ConstraintRule
		{
			partial class ConstraintAddRule
			{
				public ConstraintAddRule()
				{
					base.IsEnabled = false;
				}
			}
		}
	}
	partial class AbstractionModelIsForORMModel
	{
		partial class ConstraintRule
		{
			partial class ConstraintChangeRule
			{
				public ConstraintChangeRule()
				{
					base.IsEnabled = false;
				}
			}
		}
	}
	partial class AbstractionModelIsForORMModel
	{
		partial class ConstraintRule
		{
			partial class ConstraintDeleteRule
			{
				public ConstraintDeleteRule()
				{
					base.IsEnabled = false;
				}
			}
		}
	}
	partial class AbstractionModelIsForORMModel
	{
		partial class ObjectTypeRule
		{
			partial class ValueTypeDataTypeAddRule
			{
				public ValueTypeDataTypeAddRule()
				{
					base.IsEnabled = false;
				}
			}
		}
	}
	partial class AbstractionModelIsForORMModel
	{
		partial class ObjectTypeRule
		{
			partial class ValueTypeDataTypeDeleteRule
			{
				public ValueTypeDataTypeDeleteRule()
				{
					base.IsEnabled = false;
				}
			}
		}
	}
	#endregion // Initially disable rules
}
