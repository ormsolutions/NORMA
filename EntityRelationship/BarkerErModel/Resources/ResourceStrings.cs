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
using System.Drawing;

namespace Neumont.Tools.EntityRelationshipModels.Barker
{
	internal partial class ResourceStrings
	{
		/// <summary>
		/// The abstraction/conceptual database bridge resource.
		/// </summary>
		public enum ResourceManagers
		{
			/// <summary>
			/// Standalone resource file for the BarkerErModel
			/// </summary>
			Model,
			/// <summary>
			/// Generated resource file for the BarkerErDomainModel
			/// </summary>
			DomainModel,
		}
		#region Helper functions
		private static ResourceManager GetResourceManager(ResourceManagers manager)
		{
			switch (manager)
			{
				case ResourceManagers.Model:
					return ResourceAccessor<BarkerErModel>.ResourceManager;
				case ResourceManagers.DomainModel:
					return BarkerDomainModel.SingletonResourceManager;
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
		private static object GetObject(ResourceManagers manager, string resourceName)
		{
			object retVal = null;
			ResourceManager resMgr = GetResourceManager(manager);
			if (resMgr != null)
			{
				retVal = resMgr.GetObject(resourceName);
				Debug.Assert(retVal != null, "Unrecognized resource string: " + resourceName);
			}
			return retVal;
		}
		#endregion // Helper functions
		#region Private resource ids
		private const string SurveyTreeImageList_Id = "SurveyTree.ImageStrip";
		#endregion // Private resource ids
		#region Public accessor properties
		/// <summary>
		/// The images used in the model browser for the relational model
		/// </summary>
		public static ImageList SurveyTreeImageList
		{
			get
			{
				ImageList list = new ImageList();
				Bitmap image = GetObject(ResourceManagers.Model, SurveyTreeImageList_Id) as Bitmap;
				list.Images.AddStrip(image);
				list.ColorDepth = ColorDepth.Depth32Bit;
				list.TransparentColor = Color.Transparent;
				return list;
			}
		}
		#endregion // Public accessor properties
	}
}
