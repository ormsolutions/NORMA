
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

namespace Neumont.Tools.ORM.CustomProperties
{
	#region ResourceStrings class
	/// <summary>A helper class to insulate the rest of the code from direct resource manipulation.</summary>
	internal partial class ResourceStrings
	{
		/// <summary>The transaction name used for committing the custom properties editor. The text appears in the undo dropdown in the VS IDE.</summary>
		public static string CustomPropertiesManagerTransactionName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomProperties, "CustomPropertiesManager.TransactionName");
			}
		}
		/// <summary>The description for the custom properties verbalization snippets associated with the custom properties model.</summary>
		public static string CustomPropertiesSnippetsTypeDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomProperties, "Verbalization.CustomPropertiesSnippetsTypeDescription");
			}
		}
		/// <summary>The description for the default custom properties verbalization snippets for the custom properties model.</summary>
		public static string CustomPropertiesSnippetsDefaultDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomProperties, "Verbalization.CustomPropertiesSnippetsDefaultDescription");
			}
		}
		/// <summary>The category for the custom properties editor. Displays as a grouping category in the Properties Window.</summary>
		public static string PropertiesEditorCategory
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomProperties, "PropertiesEditor.Category");
			}
		}
		/// <summary>The description for the custom properties editor. Displays as a detailed description in the Properties Window.</summary>
		public static string PropertiesEditorDescription
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomProperties, "PropertiesEditor.Description");
			}
		}
		/// <summary>The display name for the custom properties editor. Displays as the name of a property in the Properties Window.</summary>
		public static string PropertiesEditorDisplayName
		{
			get
			{
				return ResourceStrings.GetString(ResourceManagers.CustomProperties, "PropertiesEditor.DisplayName");
			}
		}
	}
	#endregion // ResourceStrings class
}
