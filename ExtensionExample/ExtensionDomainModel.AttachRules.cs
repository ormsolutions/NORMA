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
namespace ExtensionExample
{
	#region Attach rules to ExtensionDomainModel model
	public partial class ExtensionDomainModel
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
				typeof(ExtensionAddRule)};
			System.Diagnostics.Debug.Assert(!(((System.Collections.IList)retVal).Contains(null)), "One or more rule types failed to resolve. The file and/or package will fail to load.");
			return retVal;
		}
	}
	#endregion // Attach rules to ExtensionDomainModel model
}
