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
		protected void EnableRulesAfterDeserialization(Microsoft.VisualStudio.Modeling.RuleManager ruleManager)
		{
			Type[] disabledRuleTypes = ORMShapeDomainModel.CustomDomainModelTypes;
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
	#endregion // Attach rules to ORMShapeDomainModel model
	#region Initially disable rules
	public partial class ExternalConstraintLink
	{
		private partial class DeleteDanglingConstraintShapeRule
		{
			public DeleteDanglingConstraintShapeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ExternalConstraintShape
	{
		private partial class ExclusiveOrCouplerAdded
		{
			public ExclusiveOrCouplerAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ExternalConstraintShape
	{
		private partial class ExclusiveOrCouplerDeleted
		{
			public ExclusiveOrCouplerDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactTypeShape
	{
		private partial class ConstraintDisplayPositionChangeRule
		{
			public ConstraintDisplayPositionChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactTypeShape
	{
		private partial class DerivationRuleChanged
		{
			public DerivationRuleChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactTypeShape
	{
		private partial class DerivationRuleAdd
		{
			public DerivationRuleAdd()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactTypeShape
	{
		private partial class DerivationRuleDelete
		{
			public DerivationRuleDelete()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactTypeShape
	{
		private partial class ExternalConstraintShapeChangeRule
		{
			public ExternalConstraintShapeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactTypeShape
	{
		private partial class FactTypeShapeChangeRule
		{
			public FactTypeShapeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactTypeShape
	{
		private partial class ObjectificationIsImpliedChangeRule
		{
			public ObjectificationIsImpliedChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactTypeShape
	{
		private partial class ObjectificationRolePlayerChangeRule
		{
			public ObjectificationRolePlayerChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactTypeShape
	{
		private partial class RoleDisplayOrderChanged
		{
			public RoleDisplayOrderChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactTypeShape
	{
		private partial class SwitchFromNestedFact
		{
			public SwitchFromNestedFact()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FactTypeShape
	{
		private partial class SwitchToNestedFact
		{
			public SwitchToNestedFact()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class FrequencyConstraintShape
	{
		private partial class FrequencyConstraintPropertyChangeRule
		{
			public FrequencyConstraintPropertyChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ModelNoteShape
	{
		private partial class NoteChangeRule
		{
			public NoteChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectTypeShape
	{
		private partial class DataTypeAddedRule
		{
			public DataTypeAddedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectTypeShape
	{
		private partial class DataTypeDeleteRule
		{
			public DataTypeDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectTypeShape
	{
		private partial class ObjectTypeShapeChangeRule
		{
			public ObjectTypeShapeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectTypeShape
	{
		private partial class PreferredIdentifierDeleteRule
		{
			public PreferredIdentifierDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectTypeShape
	{
		private partial class PreferredIdentifierAddedRule
		{
			public PreferredIdentifierAddedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectTypeShape
	{
		private partial class PreferredIdentifierLengthened
		{
			public PreferredIdentifierLengthened()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectTypeShape
	{
		private partial class PreferredIdentifierShortened
		{
			public PreferredIdentifierShortened()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectTypeShape
	{
		private partial class RolePlayerAddedRule
		{
			public RolePlayerAddedRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectTypeShape
	{
		private partial class RolePlayerDeleteRule
		{
			public RolePlayerDeleteRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ObjectTypeShape
	{
		private partial class ShapeChangeRule
		{
			public ShapeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMBaseBinaryLinkShape
	{
		private partial class LinkChangeRule
		{
			public LinkChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMBaseShape
	{
		private partial class ModelErrorAdded
		{
			public ModelErrorAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMBaseShape
	{
		private partial class ModelErrorDeleting
		{
			public ModelErrorDeleting()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMShapeDomainModel
	{
		private partial class ConstraintRoleSequenceRoleAdded
		{
			public ConstraintRoleSequenceRoleAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMShapeDomainModel
	{
		private partial class ConstraintRoleSequenceRoleDeleted
		{
			public ConstraintRoleSequenceRoleDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMShapeDomainModel
	{
		private partial class FactConstraintAdded
		{
			public FactConstraintAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMShapeDomainModel
	{
		private partial class FactConstraintDeleted
		{
			public FactConstraintDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMShapeDomainModel
	{
		private partial class ExternalRoleConstraintDeleted
		{
			public ExternalRoleConstraintDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMShapeDomainModel
	{
		private partial class FactTypedAdded
		{
			public FactTypedAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMShapeDomainModel
	{
		private partial class FactTypeShapeChanged
		{
			public FactTypeShapeChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMShapeDomainModel
	{
		private partial class ModelNoteAdded
		{
			public ModelNoteAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMShapeDomainModel
	{
		private partial class ModelNoteReferenceAdded
		{
			public ModelNoteReferenceAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMShapeDomainModel
	{
		private partial class ObjectTypedAdded
		{
			public ObjectTypedAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMShapeDomainModel
	{
		private partial class ObjectTypePlaysRoleAdded
		{
			public ObjectTypePlaysRoleAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMShapeDomainModel
	{
		private partial class ObjectTypePlaysRoleRolePlayerChange
		{
			public ObjectTypePlaysRoleRolePlayerChange()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMShapeDomainModel
	{
		private partial class ObjectTypeShapeChangeRule
		{
			public ObjectTypeShapeChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMShapeDomainModel
	{
		private partial class ReadingOrderAdded
		{
			public ReadingOrderAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMShapeDomainModel
	{
		private partial class RoleAdded
		{
			public RoleAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMShapeDomainModel
	{
		private partial class RoleChange
		{
			public RoleChange()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMShapeDomainModel
	{
		private partial class RoleDeleted
		{
			public RoleDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMShapeDomainModel
	{
		private partial class RoleValueConstraintAdded
		{
			public RoleValueConstraintAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMShapeDomainModel
	{
		private partial class SetComparisonConstraintAdded
		{
			public SetComparisonConstraintAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMShapeDomainModel
	{
		private partial class SetConstraintAdded
		{
			public SetConstraintAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ORMShapeDomainModel
	{
		private partial class ValueTypeValueConstraintAdded
		{
			public ValueTypeValueConstraintAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ReadingShape
	{
		private partial class DisplayOrientationChanged
		{
			public DisplayOrientationChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ReadingShape
	{
		private partial class ReadingOrderDeleted
		{
			public ReadingOrderDeleted()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ReadingShape
	{
		private partial class ReadingPositionChanged
		{
			public ReadingPositionChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ReadingShape
	{
		private partial class ReadingTextChanged
		{
			public ReadingTextChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ReadingShape
	{
		private partial class RoleDisplayOrderAdded
		{
			public RoleDisplayOrderAdded()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ReadingShape
	{
		private partial class RoleDisplayOrderPositionChanged
		{
			public RoleDisplayOrderPositionChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class RingConstraintShape
	{
		private partial class RingConstraintPropertyChangeRule
		{
			public RingConstraintPropertyChangeRule()
			{
				base.IsEnabled = false;
			}
		}
	}
	public partial class ValueConstraintShape
	{
		private partial class ValueConstraintTextChanged
		{
			public ValueConstraintTextChanged()
			{
				base.IsEnabled = false;
			}
		}
	}
	#endregion // Initially disable rules
}
