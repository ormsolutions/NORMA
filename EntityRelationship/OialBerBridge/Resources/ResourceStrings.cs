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

namespace ORMSolutions.ORMArchitect.ORMAbstractionToBarkerERBridge
{
	internal partial class ResourceStrings
	{
		/// <summary>
		/// The abstraction/conceptual database bridge resource.
		/// </summary>
		public enum ResourceManagers
		{
			/// <summary>
			/// Generated resource file for the Barker ER model
			/// </summary>
			ObjectModel,
		}
		private static ResourceManager GetResourceManager(ResourceManagers manager)
		{
			switch (manager)
			{
				case ResourceManagers.ObjectModel:
					return ORMAbstractionToBarkerERBridgeDomainModel.SingletonResourceManager;
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
