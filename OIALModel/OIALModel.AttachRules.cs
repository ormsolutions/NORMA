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
	#region Attach rules to OIALMetaModel model
	public partial class OIALMetaModel
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
				typeof(OIALModel).GetNestedType("ModelHasObjectTypeAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(OIALModel).GetNestedType("ModelHasObjectTypeRemovingRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(OIALModel).GetNestedType("ObjectTypeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(OIALModel).GetNestedType("ObjectTypePlaysRoleAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(OIALModel).GetNestedType("ObjectTypePlaysRoleRemovingRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(OIALModel).GetNestedType("ModelHasFactTypeAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(OIALModel).GetNestedType("ModelHasFactTypeRemovingRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(OIALModel).GetNestedType("FactTypeChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(OIALModel).GetNestedType("ModelHasSetConstraintAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(OIALModel).GetNestedType("ModelHasSetConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(OIALModel).GetNestedType("ModelHasSetConstraintRemovingRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(OIALModel).GetNestedType("ConstraintRoleSequenceHasRoleAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(OIALModel).GetNestedType("ConstraintRoleSequenceHasRoleRemovingRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(OIALModel).GetNestedType("UniquenessConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(OIALModel).GetNestedType("MandatoryConstraintChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(OIALModel).GetNestedType("RoleBaseChangeRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(OIALModel).GetNestedType("CheckConceptTypeParentExclusiveMandatory", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("OIALModelHasConceptTypeAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(OIALModel).GetNestedType("CheckConceptTypeParentExclusiveMandatory", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("OIALModelHasConceptTypeRemoveRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(OIALModel).GetNestedType("CheckConceptTypeParentExclusiveMandatory", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ConceptTypeAbsorbedConceptTypeAddRule", BindingFlags.Public | BindingFlags.NonPublic),
				typeof(OIALModel).GetNestedType("CheckConceptTypeParentExclusiveMandatory", BindingFlags.Public | BindingFlags.NonPublic).GetNestedType("ConceptTypeAbsorbedConceptTypeRemoveRule", BindingFlags.Public | BindingFlags.NonPublic)};
			System.Diagnostics.Debug.Assert(!(((System.Collections.IList)retVal).Contains(null)), "One or more rule types failed to resolve. The file and/or package will fail to load.");
			return retVal;
		}
	}
	#endregion // Attach rules to OIALMetaModel model
}
