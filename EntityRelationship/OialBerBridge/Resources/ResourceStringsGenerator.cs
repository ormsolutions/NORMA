
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

namespace Neumont.Tools.ORMAbstractionToBarkerERBridge
{
	#region ResourceStrings class
	/// <summary>A helper class to insulate the rest of the code from direct resource manipulation.</summary>
	internal partial class ResourceStrings
	{
		/// <summary>The text for the exception thrown when an attempt is made to enter a default custom format with no replacement fields.</summary>
		public static string ReferenceModeNamingDefaultCustomFormatInvalidDefaultCustomFormatException
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomizationModel, "ReferenceModeNaming.DefaultCustomFormat.InvalidDefaultCustomFormatException");
			}
		}
	}
	#endregion // ResourceStrings class
}
