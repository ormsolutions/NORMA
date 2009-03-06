#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
*                                                                          *
* The use and distribution terms for this software are covered by the      *
* Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
* can be found in the file CPL.txt at the root of this distribution.       *
* By using this software in any fashion, you are agreeing to be bound by   *
* the terms of this license.                                               *
*                                                                          *
* You must not remove this notice, or any other, from this software.       *
\**************************************************************************/
#endregion

using System;
using System.Diagnostics;
using System.Resources;
using System.Windows.Forms;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ShapeModel;
using System.Drawing;
using System.IO;

namespace ORMSolutions.ORMArchitect.CustomProperties
{
	internal partial class ResourceStrings
	{
		/// <summary>
		/// The Custom Properties Resource Manager.
		/// </summary>
		public enum ResourceManagers
		{
			/// <summary>
			/// Standalone resource file for the custom properties model
			/// </summary>
			CustomProperties,
		}
		private static ResourceManager GetResourceManager(ResourceManagers manager)
		{
			switch (manager)
			{
				case ResourceManagers.CustomProperties:
					// Note that this maps to the CustomPropertyProvidersResources.resx file because
					// of a custom LogicalName element in the project file. You can only access this
					// attribute if you edit the xml form of the project file.
					return ResourceAccessor<CustomProperty>.ResourceManager;
			}
			return null;
		}
		private static string GetString(ResourceManagers manager, string resourceName)
		{
			string retVal = null;
			ResourceManager resMgr = GetResourceManager(manager);
			if (resMgr != null)
			{
				retVal = resMgr.GetString(resourceName);
				Debug.Assert(!String.IsNullOrEmpty(retVal), "Unrecognized resource string: " + resourceName);
			}
			return retVal ?? String.Empty;
		}
	}
}
