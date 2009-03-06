
// Common Public License Copyright Notice
// /**************************************************************************\
// * Natural Object-Role Modeling Architect for Visual Studio                 *
// *                                                                          *
// * Copyright © ORM Solutions, LLC. All rights reserved.                        *
// *                                                                          *
// * The use and distribution terms for this software are covered by the      *
// * Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
// * can be found in the file CPL.txt at the root of this distribution.       *
// * By using this software in any fashion, you are agreeing to be bound by   *
// * the terms of this license.                                               *
// *                                                                          *
// * You must not remove this notice, or any other, from this software.       *
// \**************************************************************************/

namespace ORMSolutions.ORMArchitect.Framework
{
	#region FrameworkResourceStrings class
	/// <summary>A helper class to insulate the rest of the code from direct resource manipulation.</summary>
	internal partial class FrameworkResourceStrings
	{
		/// <summary>The transaction name for reordering diagrams</summary>
		public static string DiagramDisplayReorderDiagramsTransactionName
		{
			get
			{
				return FrameworkResourceStrings.GetString(ResourceManagers.Framework, "DiagramDisplay.ReorderDiagrams.TransactionName");
			}
		}
		/// <summary>The text shown in the Properties Window for the class name of a selected header. The component name corresponds to the localized enum name shown in the tree.</summary>
		public static string DynamicSurveyHeaderClassName
		{
			get
			{
				return FrameworkResourceStrings.GetString(ResourceManagers.Framework, "DynamicSurvey.HeaderClassName");
			}
		}
	}
	#endregion // FrameworkResourceStrings class
}
