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

namespace Neumont.Tools.ORM.ShapeModel
{
	#region Attach rules to ORMShapeDomainModel model
	partial class ORMShapeDomainModel : Neumont.Tools.ORM.ObjectModel.IDomainModelEnablesRulesAfterDeserialization
	{
		private static Type[] myCustomDomainModelTypes;
		private static Type[] CustomDomainModelTypes
		{
			get
			{
				Type[] retVal = ORMShapeDomainModel.myCustomDomainModelTypes;
				if (retVal == null)
				{
					// No synchronization is needed here.
					// If accessed concurrently, the worst that will happen is the array of Types being created multiple times.
					// This would have a slightly negative impact on performance, but the result would still be correct.
					// Given the low likelihood of this ever happening, the extra overhead of synchronization would outweigh any possible gain from it.
					retVal = new Type[]{
						typeof(ExternalConstraintLink).GetNestedType("DeleteDanglingConstraintShapeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExternalConstraintShape).GetNestedType("ExclusiveOrCouplerAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExternalConstraintShape).GetNestedType("ExclusiveOrCouplerDeleted", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExternalConstraintShape).GetNestedType("PreferredIdentifierAddRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExternalConstraintShape).GetNestedType("PreferredIdentifierDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ExternalConstraintShape).GetNestedType("PreferredIdentifierRolePlayerChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("ConstraintDisplayPositionChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("DerivationRuleChanged", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("DerivationRuleAdd", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("DerivationRuleDelete", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("ExternalConstraintShapeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("FactTypeShapeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("ObjectificationIsImpliedChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("ObjectificationRolePlayerChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("RoleDisplayOrderChanged", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("SwitchFromNestedFact", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FactTypeShape).GetNestedType("SwitchToNestedFact", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(FrequencyConstraintShape).GetNestedType("FrequencyConstraintPropertyChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ModelNoteShape).GetNestedType("NoteChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("DataTypeAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("DataTypeDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("ObjectTypeShapeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("PreferredIdentifierDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("PreferredIdentifierAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("PreferredIdentifierRolePlayerChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("PreferredIdentifierRolePlayerChangeRuleForResize", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("PreferredIdentifierLengthened", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("PreferredIdentifierShortened", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("RolePlayerAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("RolePlayerDeleteRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ObjectTypeShape).GetNestedType("ShapeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ORMBaseBinaryLinkShape).GetNestedType("LinkChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ORMBaseShape).GetNestedType("ModelErrorAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ORMBaseShape).GetNestedType("ModelErrorDeleting", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ConstraintRoleSequenceRoleAdded),
						typeof(ConstraintRoleSequenceRoleDeleted),
						typeof(FactConstraintAdded),
						typeof(FactConstraintDeleted),
						typeof(ExternalRoleConstraintDeleted),
						typeof(FactTypedAdded),
						typeof(FactTypeShapeChanged),
						typeof(ModelNoteAdded),
						typeof(ModelNoteReferenceAdded),
						typeof(ObjectTypedAdded),
						typeof(ObjectTypePlaysRoleAdded),
						typeof(ObjectTypePlaysRoleRolePlayerChange),
						typeof(ObjectTypeShapeChangeRule),
						typeof(ReadingOrderAdded),
						typeof(RoleAdded),
						typeof(RoleChange),
						typeof(RoleDeleted),
						typeof(RoleValueConstraintAdded),
						typeof(SetComparisonConstraintAdded),
						typeof(SetConstraintAdded),
						typeof(ValueTypeValueConstraintAdded),
						typeof(ReadingShape).GetNestedType("DisplayOrientationChanged", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("ReadingOrderDeleted", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("ReadingPositionChanged", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("ReadingTextChanged", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("RoleDisplayOrderAdded", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ReadingShape).GetNestedType("RoleDisplayOrderPositionChanged", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(RingConstraintShape).GetNestedType("RingConstraintPropertyChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
						typeof(ValueConstraintShape).GetNestedType("ValueConstraintTextChanged", BindingFlags.Public | BindingFlags.NonPublic)};
					ORMShapeDomainModel.myCustomDomainModelTypes = retVal;
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
			Type[] customDomainModelTypes = ORMShapeDomainModel.CustomDomainModelTypes;
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
			Type[] disabledRuleTypes = ORMShapeDomainModel.CustomDomainModelTypes;
			for (int i = 0; i < 63; ++i)
			{
				ruleManager.EnableRule(disabledRuleTypes[i]);
			}
		}
		void Neumont.Tools.ORM.ObjectModel.IDomainModelEnablesRulesAfterDeserialization.EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.Store store)
		{
			this.EnableRulesAfterDeserialization(store);
		}
	}
	#endregion // Attach rules to ORMShapeDomainModel model
	#region Initially disable rules
	partial class ExternalConstraintLink
	{
		partial class DeleteDanglingConstraintShapeRule
		{
			public DeleteDanglingConstraintShapeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ExternalConstraintShape
	{
		partial class ExclusiveOrCouplerAdded
		{
			public ExclusiveOrCouplerAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ExternalConstraintShape
	{
		partial class ExclusiveOrCouplerDeleted
		{
			public ExclusiveOrCouplerDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ExternalConstraintShape
	{
		partial class PreferredIdentifierAddRule
		{
			public PreferredIdentifierAddRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ExternalConstraintShape
	{
		partial class PreferredIdentifierDeleteRule
		{
			public PreferredIdentifierDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ExternalConstraintShape
	{
		partial class PreferredIdentifierRolePlayerChangeRule
		{
			public PreferredIdentifierRolePlayerChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactTypeShape
	{
		partial class ConstraintDisplayPositionChangeRule
		{
			public ConstraintDisplayPositionChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactTypeShape
	{
		partial class DerivationRuleChanged
		{
			public DerivationRuleChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactTypeShape
	{
		partial class DerivationRuleAdd
		{
			public DerivationRuleAdd()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactTypeShape
	{
		partial class DerivationRuleDelete
		{
			public DerivationRuleDelete()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactTypeShape
	{
		partial class ExternalConstraintShapeChangeRule
		{
			public ExternalConstraintShapeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactTypeShape
	{
		partial class FactTypeShapeChangeRule
		{
			public FactTypeShapeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactTypeShape
	{
		partial class ObjectificationIsImpliedChangeRule
		{
			public ObjectificationIsImpliedChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactTypeShape
	{
		partial class ObjectificationRolePlayerChangeRule
		{
			public ObjectificationRolePlayerChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactTypeShape
	{
		partial class RoleDisplayOrderChanged
		{
			public RoleDisplayOrderChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactTypeShape
	{
		partial class SwitchFromNestedFact
		{
			public SwitchFromNestedFact()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FactTypeShape
	{
		partial class SwitchToNestedFact
		{
			public SwitchToNestedFact()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class FrequencyConstraintShape
	{
		partial class FrequencyConstraintPropertyChangeRule
		{
			public FrequencyConstraintPropertyChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ModelNoteShape
	{
		partial class NoteChangeRule
		{
			public NoteChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectTypeShape
	{
		partial class DataTypeAddedRule
		{
			public DataTypeAddedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectTypeShape
	{
		partial class DataTypeDeleteRule
		{
			public DataTypeDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectTypeShape
	{
		partial class ObjectTypeShapeChangeRule
		{
			public ObjectTypeShapeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectTypeShape
	{
		partial class PreferredIdentifierDeleteRule
		{
			public PreferredIdentifierDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectTypeShape
	{
		partial class PreferredIdentifierAddedRule
		{
			public PreferredIdentifierAddedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectTypeShape
	{
		partial class PreferredIdentifierRolePlayerChangeRule
		{
			public PreferredIdentifierRolePlayerChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectTypeShape
	{
		partial class PreferredIdentifierRolePlayerChangeRuleForResize
		{
			public PreferredIdentifierRolePlayerChangeRuleForResize()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectTypeShape
	{
		partial class PreferredIdentifierLengthened
		{
			public PreferredIdentifierLengthened()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectTypeShape
	{
		partial class PreferredIdentifierShortened
		{
			public PreferredIdentifierShortened()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectTypeShape
	{
		partial class RolePlayerAddedRule
		{
			public RolePlayerAddedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectTypeShape
	{
		partial class RolePlayerDeleteRule
		{
			public RolePlayerDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ObjectTypeShape
	{
		partial class ShapeChangeRule
		{
			public ShapeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMBaseBinaryLinkShape
	{
		partial class LinkChangeRule
		{
			public LinkChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMBaseShape
	{
		partial class ModelErrorAdded
		{
			public ModelErrorAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMBaseShape
	{
		partial class ModelErrorDeleting
		{
			public ModelErrorDeleting()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMShapeDomainModel
	{
		partial class ConstraintRoleSequenceRoleAdded
		{
			public ConstraintRoleSequenceRoleAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMShapeDomainModel
	{
		partial class ConstraintRoleSequenceRoleDeleted
		{
			public ConstraintRoleSequenceRoleDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMShapeDomainModel
	{
		partial class FactConstraintAdded
		{
			public FactConstraintAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMShapeDomainModel
	{
		partial class FactConstraintDeleted
		{
			public FactConstraintDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMShapeDomainModel
	{
		partial class ExternalRoleConstraintDeleted
		{
			public ExternalRoleConstraintDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMShapeDomainModel
	{
		partial class FactTypedAdded
		{
			public FactTypedAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMShapeDomainModel
	{
		partial class FactTypeShapeChanged
		{
			public FactTypeShapeChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMShapeDomainModel
	{
		partial class ModelNoteAdded
		{
			public ModelNoteAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMShapeDomainModel
	{
		partial class ModelNoteReferenceAdded
		{
			public ModelNoteReferenceAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMShapeDomainModel
	{
		partial class ObjectTypedAdded
		{
			public ObjectTypedAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMShapeDomainModel
	{
		partial class ObjectTypePlaysRoleAdded
		{
			public ObjectTypePlaysRoleAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMShapeDomainModel
	{
		partial class ObjectTypePlaysRoleRolePlayerChange
		{
			public ObjectTypePlaysRoleRolePlayerChange()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMShapeDomainModel
	{
		partial class ObjectTypeShapeChangeRule
		{
			public ObjectTypeShapeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMShapeDomainModel
	{
		partial class ReadingOrderAdded
		{
			public ReadingOrderAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMShapeDomainModel
	{
		partial class RoleAdded
		{
			public RoleAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMShapeDomainModel
	{
		partial class RoleChange
		{
			public RoleChange()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMShapeDomainModel
	{
		partial class RoleDeleted
		{
			public RoleDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMShapeDomainModel
	{
		partial class RoleValueConstraintAdded
		{
			public RoleValueConstraintAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMShapeDomainModel
	{
		partial class SetComparisonConstraintAdded
		{
			public SetComparisonConstraintAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMShapeDomainModel
	{
		partial class SetConstraintAdded
		{
			public SetConstraintAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ORMShapeDomainModel
	{
		partial class ValueTypeValueConstraintAdded
		{
			public ValueTypeValueConstraintAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ReadingShape
	{
		partial class DisplayOrientationChanged
		{
			public DisplayOrientationChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ReadingShape
	{
		partial class ReadingOrderDeleted
		{
			public ReadingOrderDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ReadingShape
	{
		partial class ReadingPositionChanged
		{
			public ReadingPositionChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ReadingShape
	{
		partial class ReadingTextChanged
		{
			public ReadingTextChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ReadingShape
	{
		partial class RoleDisplayOrderAdded
		{
			public RoleDisplayOrderAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ReadingShape
	{
		partial class RoleDisplayOrderPositionChanged
		{
			public RoleDisplayOrderPositionChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class RingConstraintShape
	{
		partial class RingConstraintPropertyChangeRule
		{
			public RingConstraintPropertyChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	partial class ValueConstraintShape
	{
		partial class ValueConstraintTextChanged
		{
			public ValueConstraintTextChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	#endregion // Initially disable rules
}
