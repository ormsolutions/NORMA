using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using Neumont.Tools.Modeling.Design;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace Neumont.Tools.Dil.Dcil
{
	internal static class Resources
	{
		#region Supported Resource Managers
		/// <summary>
		/// Recognized resource managers
		/// </summary>
		private enum ResourceManagers
		{
			/// <summary>
			/// DSL-managed resource file for the core object model
			/// </summary>
			DomainModel,
			/// <summary>
			/// Standalone resource file for the root element of the model
			/// </summary>
			Catalog,
		}
		#endregion // Supported Resource Managers
		#region Non-DSL ResourceManagers
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

		private static ResourceManager CatalogResourceManager
		{
			get
			{
				return ResourceAccessor<Catalog>.ResourceManager;
			}
		}
		#endregion // Non-DSL ResourceManagers
		#region Helper functions
		private static ResourceManager GetResourceManager(ResourceManagers manager)
		{
			switch (manager)
			{
				case ResourceManagers.DomainModel:
					return DcilDomainModel.SingletonResourceManager;
				case ResourceManagers.Catalog:
					return CatalogResourceManager;
				default:
					return null;
			}
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
				Bitmap image = GetObject(ResourceManagers.Catalog, SurveyTreeImageList_Id) as Bitmap;
				list.Images.AddStrip(image);
				list.ColorDepth = ColorDepth.Depth32Bit;
				list.TransparentColor = Color.Transparent;
				return list;
			}
		}
		#endregion // Public accessor properties
	}
}
