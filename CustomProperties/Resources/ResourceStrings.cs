#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
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
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;
using System.Drawing;
using System.IO;

namespace Neumont.Tools.ORM.CustomProperties
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
			CustomProperties
		}
		private static readonly object LockObject = new object();
		private static void LoadResourceManagerForType(ref ResourceManager resMgr, Type type)
		{
			if (resMgr == null)
			{
				lock (LockObject)
				{
					if (resMgr == null)
					{
						resMgr = new ResourceManager(type.FullName, type.Assembly);
					}
				}
			}
		}
		private static ResourceManager CustomPropertiesResourceManager
		{
			get
			{
				return ResourceAccessor<CustomPropertyProvidersResources>.ResourceManager;
			}
		}
		private static ResourceManager myCustomPropertiesModelResourceManager;
		private static ResourceManager CustomPropertiesModelResourceManager
		{
			get
			{
				if (myCustomPropertiesModelResourceManager == null)
				{
					LoadResourceManagerForType(ref myCustomPropertiesModelResourceManager, typeof(CustomPropertyProvidersResources));
				}
				return myCustomPropertiesModelResourceManager;
			}
		}
		private static ResourceManager GetResourceManager(ResourceManagers manager)
		{
			return CustomPropertiesModelResourceManager;
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
