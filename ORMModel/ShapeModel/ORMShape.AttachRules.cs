using System;
using System.Reflection;
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
			if (!(Neumont.Tools.ORM.ObjectModel.ORMMetaModel.ReflectRules))
			{
				return Type.EmptyTypes;
			}
			Type[] retVal = new Type[]{
				typeof(FactTypeShape).GetNestedType("ConstraintDisplayPositionChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactTypeShape).GetNestedType("ExternalConstraintShapeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactTypeShape).GetNestedType("FactTypeShapeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactTypeShape).GetNestedType("SwitchFromNestedFact", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FactTypeShape).GetNestedType("SwitchToNestedFact", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(FrequencyConstraintShape).GetNestedType("FrequencyConstraintAttributeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectTypeShape).GetNestedType("PreferredIdentifierRemovedRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectTypeShape).GetNestedType("PreferredIdentifierAddedRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ObjectTypeShape).GetNestedType("ShapeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ORMBaseBinaryLinkShape).GetNestedType("LinkChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ConstraintRoleSequenceRoleAdded),
				typeof(ConstraintRoleSequenceRoleRemoved),
				typeof(ExternalFactConstraintAdded),
				typeof(ExternalFactConstraintRemoved),
				typeof(FactTypedAdded),
				typeof(FactTypeShapeChanged),
				typeof(FactTypeHasInternalConstraintAdded),
				typeof(FactTypeHasInternalConstraintRemoved),
				typeof(MultiColumnExternalConstraintAdded),
				typeof(ObjectTypedAdded),
				typeof(ObjectTypePlaysRoleRemoved),
				typeof(ObjectTypeShapeChangeRule),
				typeof(ParentShapeRemoved),
				typeof(PresentationLinkRemoved),
				typeof(ReadingOrderAdded),
				typeof(RoleAdded),
				typeof(RoleChange),
				typeof(ObjectTypePlaysRoleAdded),
				typeof(RoleRemoved),
				typeof(RoleValueConstraintAdded),
				typeof(RoleValueConstraintRemoved),
				typeof(SingleColumnExternalConstraintAdded),
				typeof(ValueTypeValueConstraintAdded),
				typeof(ValueTypeValueConstraintRemoved),
				typeof(ReadingShape).GetNestedType("ReadingOrderReadingTextChanged", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ReadingShape).GetNestedType("ReadingOrderRemoved", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(ReadingShape).GetNestedType("ReadingTextChanged", BindingFlags.Public | BindingFlags.NonPublic),
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
