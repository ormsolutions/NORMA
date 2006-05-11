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
	#region Attach rules to ORMShapeModel model
	public partial class ORMShapeModel
	{
		/// <summary>
		/// Generated code to attach rules to the store.
		/// </summary>
		protected override Type[] AllMetaModelTypes()
		{
			if (!(Neumont.Tools.ORM.ObjectModel.ORMMetaModel.InitializingToolboxItems))
			{
				return Type.EmptyTypes;
			}
			Type[] retVal = new Type[]{
				typeof(ExternalConstraintLink).GetNestedType("RemoveDanglingConstraintShapeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactTypeShape).GetNestedType("ConstraintDisplayPositionChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactTypeShape).GetNestedType("DerivationRuleChanged", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactTypeShape).GetNestedType("DerivationRuleAdd", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactTypeShape).GetNestedType("DerivationRuleRemove", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactTypeShape).GetNestedType("ExternalConstraintShapeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactTypeShape).GetNestedType("FactTypeShapeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactTypeShape).GetNestedType("ObjectificationIsImpliedChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactTypeShape).GetNestedType("RoleDisplayOrderChanged", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactTypeShape).GetNestedType("SwitchFromNestedFact", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactTypeShape).GetNestedType("SwitchToNestedFact", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FrequencyConstraintShape).GetNestedType("FrequencyConstraintAttributeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectTypeShape).GetNestedType("DataTypeAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectTypeShape).GetNestedType("PreferredIdentifierRemovedRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectTypeShape).GetNestedType("PreferredIdentifierAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectTypeShape).GetNestedType("RolePlayerAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectTypeShape).GetNestedType("RolePlayerRemovedRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectTypeShape).GetNestedType("ShapeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ORMBaseBinaryLinkShape).GetNestedType("LinkChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ORMBaseShape).GetNestedType("ModelErrorAdded", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ORMBaseShape).GetNestedType("ModelErrorRemoving", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ConstraintRoleSequenceRoleAdded),
				typeof(ConstraintRoleSequenceRoleRemoved),
				typeof(FactConstraintAdded),
				typeof(FactConstraintRemoved),
				typeof(ExternalRoleConstraintRemoved),
				typeof(FactTypedAdded),
				typeof(FactTypeShapeChanged),
				typeof(LinkConnectsToNodeRemoved),
				typeof(SetComparisonConstraintAdded),
				typeof(ObjectTypedAdded),
				typeof(ObjectTypePlaysRoleRemoved),
				typeof(ObjectTypeShapeChangeRule),
				typeof(ParentShapeRemoved),
				typeof(PresentationLinkRemoved),
				typeof(ReadingOrderAdded),
				typeof(RoleAdded),
				typeof(RoleChange),
				typeof(ObjectTypePlaysRoleAdded),
				typeof(RelativeParentShapeRemoved),
				typeof(RoleRemoved),
				typeof(RoleValueConstraintAdded),
				typeof(RoleValueConstraintRemoved),
				typeof(SetConstraintAdded),
				typeof(ValueTypeValueConstraintAdded),
				typeof(ValueTypeValueConstraintRemoved),
				typeof(ReadingShape).GetNestedType("ReadingOrderReadingTextChanged", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ReadingShape).GetNestedType("ReadingOrderRemoved", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ReadingShape).GetNestedType("ReadingTextChanged", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ReadingShape).GetNestedType("RoleDisplayOrderPositionChanged", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ReadingShape).GetNestedType("RoleDisplayOrderAdded", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(RingConstraintShape).GetNestedType("RingConstraintAttributeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(RolePlayerLink).GetNestedType("RolePlayerRemoving", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ValueConstraintShape).GetNestedType("ValueRangeChanged", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ValueConstraintShape).GetNestedType("ValueConstraintAdded", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ValueConstraintShape).GetNestedType("ValueTypeHasDataTypeAdded", BindingFlags.Public | BindingFlags.NonPublic)};
			System.Diagnostics.Debug.Assert(!(((System.Collections.IList)retVal).Contains(null)), "One or more rule types failed to resolve. The file and/or package will fail to load.");
			return retVal;
		}
	}
	#endregion // Attach rules to ORMShapeModel model
}
