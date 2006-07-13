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
		private static Type[] myCustomDomainModelTypes;
		private static Type[] CustomDomainModelTypes
		{
			get
			{
				Type[] retVal = OIALMetaModel.myCustomDomainModelTypes;
				if (retVal == null)
				{
					// No synchronization is needed here.
					// If accessed concurrently, the worst that will happen is the array of Types being created multiple times.
					// This would have a slightly negative impact on performance, but the result would still be correct.
					// Given the low likelihood of this even happening, the extra overhead of synchronization would outweigh any possible gain from it.
					retVal = new Type[]{
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
					OIALMetaModel.myCustomDomainModelTypes = retVal;
					System.Diagnostics.Debug.Assert(Array.IndexOf<Type>(retVal, null) < 0, "One or more rule types failed to resolve. The file and/or package will fail to load.");
				}
				return retVal;
			}
		}
		/// <summary>
		/// Generated code to attach s to the .
		/// </summary>
		/// <seealso cref="Microsoft.VisualStudio.Modeling.DomainModel.GetCustomDomainModelTypes">
		/// 
		/// </seealso>
		protected override Type[] GetCustomDomainModelTypes()
		{
			if (!(Neumont.Tools.ORM.ObjectModel.ORMCoreModel.InitializingToolboxItems))
			{
				return Type.EmptyTypes;
			}
			Type[] retVal = base.GetCustomDomainModelTypes();
			int baseLength = retVal.Length;
			Type[] customDomainModelTypes = OIALMetaModel.CustomDomainModelTypes;
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
	#endregion // Attach rules to OIALMetaModel model
}
