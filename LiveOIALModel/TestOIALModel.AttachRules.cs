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

namespace Neumont.Tools.ORM.TestOIALModel
{
	#region Attach rules to OIALDomainModel model
	partial class OIALDomainModel : Neumont.Tools.ORM.ObjectModel.IDomainModelEnablesRulesAfterDeserialization
	{
		private static Type[] myCustomDomainModelTypes;
		private static Type[] CustomDomainModelTypes
		{
			get
			{
				Type[] retVal = OIALDomainModel.myCustomDomainModelTypes;
				if (retVal == null)
				{
					// No synchronization is needed here.
					// If accessed concurrently, the worst that will happen is the array of Types being created multiple times.
					// This would have a slightly negative impact on performance, but the result would still be correct.
					// Given the low likelihood of this ever happening, the extra overhead of synchronization would outweigh any possible gain from it.
					retVal = new Type[]{
						typeof(LiveOIALModel).GetNestedType("MandatoryConstraintRule", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("MandatoryConstraintAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(LiveOIALModel).GetNestedType("MandatoryConstraintRule", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("MandatoryConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(LiveOIALModel).GetNestedType("MandatoryConstraintRule", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("MandatoryConstraintDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(LiveOIALModel).GetNestedType("CheckConceptTypeParentExclusiveMandatory", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("OIALModelHasConceptTypeAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(LiveOIALModel).GetNestedType("CheckConceptTypeParentExclusiveMandatory", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("OIALModelHasConceptTypeDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(LiveOIALModel).GetNestedType("CheckConceptTypeParentExclusiveMandatory", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ConceptTypeAbsorbedConceptTypeAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(LiveOIALModel).GetNestedType("CheckConceptTypeParentExclusiveMandatory", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ConceptTypeAbsorbedConceptTypeDeleteRule", BindingFlags.Public | BindingFlags.NonPublic)};
					OIALDomainModel.myCustomDomainModelTypes = retVal;
					System.Diagnostics.Debug.Assert(Array.IndexOf<Type>(retVal, null) < 0, "One or more rule types failed to resolve. The file and/or package will fail to load.");
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
			Type[] customDomainModelTypes = OIALDomainModel.CustomDomainModelTypes;
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
			Type[] disabledRuleTypes = OIALDomainModel.CustomDomainModelTypes;
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
	#endregion // Attach rules to OIALDomainModel model
	#region Initially disable rules
	public partial class LiveOIALModel
	{
		private partial class MandatoryConstraintRule
		{
			private partial class MandatoryConstraintAddRule
			{
				public MandatoryConstraintAddRule()
				{
					base.IsEnabled = false;
				}
			}
		}
	}
	public partial class LiveOIALModel
	{
		private partial class MandatoryConstraintRule
		{
			private partial class MandatoryConstraintChangeRule
			{
				public MandatoryConstraintChangeRule()
				{
					base.IsEnabled = false;
				}
			}
		}
	}
	public partial class LiveOIALModel
	{
		private partial class MandatoryConstraintRule
		{
			private partial class MandatoryConstraintDeletingRule
			{
				public MandatoryConstraintDeletingRule()
				{
					base.IsEnabled = false;
				}
			}
		}
	}
	public partial class LiveOIALModel
	{
		private partial class CheckConceptTypeParentExclusiveMandatory
		{
			private partial class OIALModelHasConceptTypeAddRule
			{
				public OIALModelHasConceptTypeAddRule()
				{
					base.IsEnabled = false;
				}
			}
		}
	}
	public partial class LiveOIALModel
	{
		private partial class CheckConceptTypeParentExclusiveMandatory
		{
			private partial class OIALModelHasConceptTypeDeleteRule
			{
				public OIALModelHasConceptTypeDeleteRule()
				{
					base.IsEnabled = false;
				}
			}
		}
	}
	public partial class LiveOIALModel
	{
		private partial class CheckConceptTypeParentExclusiveMandatory
		{
			private partial class ConceptTypeAbsorbedConceptTypeAddRule
			{
				public ConceptTypeAbsorbedConceptTypeAddRule()
				{
					base.IsEnabled = false;
				}
			}
		}
	}
	public partial class LiveOIALModel
	{
		private partial class CheckConceptTypeParentExclusiveMandatory
		{
			private partial class ConceptTypeAbsorbedConceptTypeDeleteRule
			{
				public ConceptTypeAbsorbedConceptTypeDeleteRule()
				{
					base.IsEnabled = false;
				}
			}
		}
	}
	#endregion // Initially disable rules
}
