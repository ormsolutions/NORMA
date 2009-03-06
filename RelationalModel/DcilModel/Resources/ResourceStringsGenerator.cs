
// Common Public License Copyright Notice
// /**************************************************************************\
// * Natural Object-Role Modeling Architect for Visual Studio                 *
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

namespace ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase
{
	#region ResourceStrings class
	/// <summary>A helper class to insulate the rest of the code from direct resource manipulation.</summary>
	internal partial class ResourceStrings
	{
		/// <summary>The format string used to display a column reference in the ORM Model Browser. {0}=name of the source column, {1}=name of the target column</summary>
		public static string ColumnReferenceDisplayFormatString
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CatalogModel, "ColumnReference.DisplayFormatString");
			}
		}
	}
	#endregion // ResourceStrings class
}
