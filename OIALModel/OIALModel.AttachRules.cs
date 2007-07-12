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

namespace Neumont.Tools.ORM.OIALModel
{
	#region Attach rules to OIALDomainModel model
	partial class OIALDomainModel : Neumont.Tools.Modeling.Shell.IDomainModelEnablesRulesAfterDeserialization
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
						typeof(OIALModel).GetNestedType("ModelHasObjectTypeAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(OIALModel).GetNestedType("ModelHasObjectTypeDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(OIALModel).GetNestedType("ObjectTypeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(OIALModel).GetNestedType("ObjectTypePlaysRoleAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(OIALModel).GetNestedType("ObjectTypePlaysRoleDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(OIALModel).GetNestedType("ModelHasFactTypeAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(OIALModel).GetNestedType("ModelHasFactTypeDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(OIALModel).GetNestedType("ModelHasSetConstraintAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(OIALModel).GetNestedType("ModelHasSetConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(OIALModel).GetNestedType("ModelHasSetConstraintDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(OIALModel).GetNestedType("ConstraintRoleSequenceHasRoleAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(OIALModel).GetNestedType("ConstraintRoleSequenceHasRoleDeletingRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(OIALModel).GetNestedType("UniquenessConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(OIALModel).GetNestedType("MandatoryConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(OIALModel).GetNestedType("RoleBaseChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(OIALModel).GetNestedType("CheckConceptTypeParentExclusiveMandatory", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("OIALModelHasConceptTypeAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(OIALModel).GetNestedType("CheckConceptTypeParentExclusiveMandatory", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("OIALModelHasConceptTypeDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(OIALModel).GetNestedType("CheckConceptTypeParentExclusiveMandatory", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ConceptTypeAbsorbedConceptTypeAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(OIALModel).GetNestedType("CheckConceptTypeParentExclusiveMandatory", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ConceptTypeAbsorbedConceptTypeDeleteRule", BindingFlags.Public | BindingFlags.NonPublic)};
					OIALDomainModel.myCustomDomainModelTypes = retVal;
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
		/// <summary>Implements IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization</summary>
		protected void EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.Store store)
		{
			Microsoft.VisualStudio.Modeling.RuleManager ruleManager = store.RuleManager;
			Type[] disabledRuleTypes = OIALDomainModel.CustomDomainModelTypes;
			for (int i = 0; i < 19; ++i)
			{
				ruleManager.EnableRule(disabledRuleTypes[i]);
			}
		}
		void Neumont.Tools.Modeling.Shell.IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.Store store)
		{
			this.EnableRulesAfterDeserialization(store);
		}
	}
	#endregion // Attach rules to OIALDomainModel model
	#region Initially disable rules
	partial class OIALModel
	{
		partial class ModelHasObjectTypeAddRule
		{
			public ModelHasObjectTypeAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class OIALModel
	{
		partial class ModelHasObjectTypeDeletingRule
		{
			public ModelHasObjectTypeDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class OIALModel
	{
		partial class ObjectTypeChangeRule
		{
			public ObjectTypeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class OIALModel
	{
		partial class ObjectTypePlaysRoleAddRule
		{
			public ObjectTypePlaysRoleAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class OIALModel
	{
		partial class ObjectTypePlaysRoleDeletingRule
		{
			public ObjectTypePlaysRoleDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class OIALModel
	{
		partial class ModelHasFactTypeAddRule
		{
			public ModelHasFactTypeAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class OIALModel
	{
		partial class ModelHasFactTypeDeletingRule
		{
			public ModelHasFactTypeDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class OIALModel
	{
		partial class ModelHasSetConstraintAddRule
		{
			public ModelHasSetConstraintAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class OIALModel
	{
		partial class ModelHasSetConstraintChangeRule
		{
			public ModelHasSetConstraintChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class OIALModel
	{
		partial class ModelHasSetConstraintDeletingRule
		{
			public ModelHasSetConstraintDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class OIALModel
	{
		partial class ConstraintRoleSequenceHasRoleAddRule
		{
			public ConstraintRoleSequenceHasRoleAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class OIALModel
	{
		partial class ConstraintRoleSequenceHasRoleDeletingRule
		{
			public ConstraintRoleSequenceHasRoleDeletingRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class OIALModel
	{
		partial class UniquenessConstraintChangeRule
		{
			public UniquenessConstraintChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class OIALModel
	{
		partial class MandatoryConstraintChangeRule
		{
			public MandatoryConstraintChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class OIALModel
	{
		partial class RoleBaseChangeRule
		{
			public RoleBaseChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class OIALModel
	{
		partial class CheckConceptTypeParentExclusiveMandatory
		{
			partial class OIALModelHasConceptTypeAddRule
			{
				public OIALModelHasConceptTypeAddRule()
				{
					base.IsEnabled = false;
				}
			}
		}
	}
	partial class OIALModel
	{
		partial class CheckConceptTypeParentExclusiveMandatory
		{
			partial class OIALModelHasConceptTypeDeleteRule
			{
				public OIALModelHasConceptTypeDeleteRule()
				{
					base.IsEnabled = false;
				}
			}
		}
	}
	partial class OIALModel
	{
		partial class CheckConceptTypeParentExclusiveMandatory
		{
			partial class ConceptTypeAbsorbedConceptTypeAddRule
			{
				public ConceptTypeAbsorbedConceptTypeAddRule()
				{
					base.IsEnabled = false;
				}
			}
		}
	}
	partial class OIALModel
	{
		partial class CheckConceptTypeParentExclusiveMandatory
		{
			partial class ConceptTypeAbsorbedConceptTypeDeleteRule
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
