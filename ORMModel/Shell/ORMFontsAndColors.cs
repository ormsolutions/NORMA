using System;
using System.Diagnostics;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio;
using System.Runtime.InteropServices;
namespace Northface.Tools.ORM.Shell
{
	/// <summary>
	/// Indexed color values in the 'ORM Designer' color category
	/// </summary>
	public enum ORMDesignerColor
	{
		/// <summary>
		/// The color used to draw all constraints
		/// </summary>
		Constraint,
		/// <summary>
		/// The color used to draw selected constraints
		/// during a role picker mouse action
		/// </summary>
		RolePicker,
		// Items here must be in the same order as myDefaultColorSettings
		// defined in the ORMDesignerFontsAndColors class
	}
	/// <summary>
	/// The class to create the ORM Designer category in the tools/options/environment/fonts and colors page
	/// </summary>
	[Guid("C5AA80F8-F730-4809-AAB1-8D925E36F9F5")]
	public partial class ORMDesignerFontsAndColors : IVsFontAndColorDefaultsProvider, IVsFontAndColorDefaults
	{
		#region Constant definitions
		/// <summary>
		/// Bitwise or this value with a value from the ColorIndex array
		/// </summary>
		private const uint StandardPaletteBit = 0x01000000;
		/// <summary>
		/// The guid for the font and color category used for this language service
		/// </summary>
		public static readonly Guid FontAndColorCategory = new Guid("663DE24F-8E3A-4C0F-A307-53053ED6C59B");
		/// <summary>
		/// The unlocalized name for the constraint display item
		/// </summary>
		public const string ConstraintColorName = "ORM Constraint";
		/// <summary>
		/// The unlocalized name for the role highlight display item
		/// </summary>
		public const string RolePickerColorName = "ORM Role Picker";
		#endregion // Constant definitions
		#region Default Settings
		/// <summary>
		/// A helper structure for store default settings
		/// </summary>
		private struct DefaultColorSetting
		{
			public DefaultColorSetting(
				string name,
				string localizedNameId,
				uint foregroundColor,
				uint backgroundColor,
				__FCITEMFLAGS itemFlags,
				bool defaultBold)
			{
				Name = name;
				LocalizedNameId = localizedNameId;
				ForegroundColor = foregroundColor;
				BackgroundColor = backgroundColor;
				ItemFlags = itemFlags;
				DefaultBold = defaultBold;
			}
			public string Name;
			public string LocalizedNameId;
			public uint ForegroundColor;
			public uint BackgroundColor;
			public __FCITEMFLAGS ItemFlags;
			public bool DefaultBold;
		}
		/// <summary>
		/// Default setting definition. Must be in the same order as
		/// the ORMDesignerColor enum
		/// </summary>
		private static readonly DefaultColorSetting[] myDefaultColorSettings = {
			new DefaultColorSetting(
			ConstraintColorName,
			ResourceStrings.FontsAndColorsConstraintColorId,
			(int)COLORINDEX.CI_MAGENTA | StandardPaletteBit,
			(int)COLORINDEX.CI_SYSPLAINTEXT_BK | StandardPaletteBit,
			__FCITEMFLAGS.FCIF_ALLOWFGCHANGE | __FCITEMFLAGS.FCIF_ALLOWCUSTOMCOLORS,
			false)
			,new DefaultColorSetting(
			RolePickerColorName,
			ResourceStrings.FontsAndColorsRolePickerColorId,
			(uint)COLORINDEX.CI_SYSPLAINTEXT_FG | StandardPaletteBit,
			(uint)COLORINDEX.CI_YELLOW | StandardPaletteBit,
			__FCITEMFLAGS.FCIF_ALLOWBGCHANGE | __FCITEMFLAGS.FCIF_ALLOWFGCHANGE | __FCITEMFLAGS.FCIF_ALLOWCUSTOMCOLORS,
			false)
		};
		#endregion // Default Settings
		#region IVsFontAndColorDefaultsProvider Implementation
		/// <summary>
		/// Implements IVsFontAndColorDefaultsProvider.GetObject
		/// </summary>
		/// <param name="rguidCategory"></param>
		/// <param name="ppObj"></param>
		/// <returns></returns>
		protected int GetObject(ref Guid rguidCategory, out object ppObj)
		{
			if (rguidCategory == FontAndColorCategory)
			{
				ppObj = this as IVsFontAndColorDefaults;
				return NativeMethods.S_OK;
			}
			ppObj = null;
			return NativeMethods.E_NOINTERFACE;
		}
		int IVsFontAndColorDefaultsProvider.GetObject(ref Guid rguidCategory, out object ppObj)
		{
			return GetObject(ref rguidCategory, out ppObj);
		}
		#endregion // IVsFontAndColorDefaultsProvider Implementation
		#region IVsFontAndColorDefaults Implementation
		/// <summary>
		/// Implements IVsFontAndColorDefaults.GetBaseCategory
		/// </summary>
		/// <param name="pguidBase"></param>
		/// <returns></returns>
		protected int GetBaseCategory(out Guid pguidBase)
		{
			pguidBase = Guid.Empty;
			return NativeMethods.E_NOTIMPL;
		}
		int IVsFontAndColorDefaults.GetBaseCategory(out Guid pguidBase)
		{
			return GetBaseCategory(out pguidBase);
		}
		/// <summary>
		/// Implements  IVsFontAndColorDefaults.GetCategoryName
		/// </summary>
		/// <param name="pbstrName"></param>
		/// <returns></returns>
		protected int GetCategoryName(out string pbstrName)
		{
			pbstrName = ResourceStrings.GetColorNameString(ResourceStrings.FontsAndColorsCategoryNameId);
			return NativeMethods.S_OK;
		}
		int IVsFontAndColorDefaults.GetCategoryName(out string pbstrName)
		{
			return GetCategoryName(out pbstrName);
		}
		/// <summary>
		/// Implements IVsFontAndColorDefaults.GetFlags
		/// </summary>
		/// <param name="dwFlags"></param>
		/// <returns></returns>
		protected int GetFlags(out uint dwFlags)
		{
			// Pull values from the __FONTCOLORFLAGS enum
			dwFlags = (uint)(__FONTCOLORFLAGS.FCF_ONLYNEWINSTANCES);
			return NativeMethods.S_OK;
		}
		int IVsFontAndColorDefaults.GetFlags(out uint dwFlags)
		{
			return GetFlags(out dwFlags);
		}
		/// <summary>
		/// Implements IVsFontAndColorDefaults.GetFont
		/// </summary>
		/// <param name="pInfo"></param>
		/// <returns></returns>
		protected int GetFont(FontInfo[] pInfo)
		{
			FontInfo info = new FontInfo();
			info.bstrFaceName = "Tahoma";
			info.bFaceNameValid = 1;
			info.wPointSize = 7;
			info.bPointSizeValid = 1;
			info.iCharSet = (byte)EnvDTE.vsFontCharSet.vsFontCharSetDefault;
			info.bCharSetValid = 1;
			pInfo[0] = info;
			return NativeMethods.S_OK;
		}
		int IVsFontAndColorDefaults.GetFont(FontInfo[] pInfo)
		{
			return GetFont(pInfo);
		}
		/// <summary>
		/// Implements IVsFontAndColorDefaults.GetItem
		/// </summary>
		/// <param name="iItem"></param>
		/// <param name="pInfo"></param>
		/// <returns></returns>
		protected int GetItem(int iItem, AllColorableItemInfo[] pInfo)
		{
			AllColorableItemInfo allInfo = new AllColorableItemInfo();
			DefaultColorSetting setting = myDefaultColorSettings[iItem];
			allInfo.fFlags = (uint)setting.ItemFlags;
			allInfo.bFlagsValid = 1;
			allInfo.bstrName = setting.Name;
			allInfo.bNameValid = 1;
			allInfo.bstrLocalizedName = ResourceStrings.GetColorNameString(setting.LocalizedNameId);
			allInfo.bLocalizedNameValid = 1;
			allInfo.Info.crForeground = allInfo.crAutoForeground = setting.ForegroundColor;
			allInfo.Info.bForegroundValid = allInfo.bAutoForegroundValid = 1;
			allInfo.Info.crBackground = allInfo.crAutoBackground = setting.BackgroundColor;
			allInfo.Info.bBackgroundValid = allInfo.bAutoBackgroundValid = 1;
			allInfo.Info.dwFontFlags = (setting.DefaultBold) ? (uint)FONTFLAGS.FF_BOLD : 0;
			allInfo.Info.bFontFlagsValid = 1;
			pInfo[0] = allInfo;
			return NativeMethods.S_OK;
		}
		int IVsFontAndColorDefaults.GetItem(int iItem, AllColorableItemInfo[] pInfo)
		{
			return GetItem(iItem, pInfo);
		}
		/// <summary>
		/// Implements IVsFontAndColorDefaults.GetItemByName
		/// </summary>
		/// <param name="szItem"></param>
		/// <param name="pInfo"></param>
		/// <returns></returns>
		protected int GetItemByName(string szItem, AllColorableItemInfo[] pInfo)
		{
			DefaultColorSetting[] settings = myDefaultColorSettings;
			int settingsCount = settings.Length;
			for (int i = 0; i < settingsCount; ++i)
			{
				if (settings[i].Name == szItem)
				{
					return GetItem(i, pInfo);
				}
			}
			return NativeMethods.E_INVALIDARG;
		}
		int IVsFontAndColorDefaults.GetItemByName(string szItem, AllColorableItemInfo[] pInfo)
		{
			return GetItemByName(szItem, pInfo);
		}
		/// <summary>
		/// Implements IVsFontAndColorDefaults.GetItemCount
		/// </summary>
		/// <param name="pcItems"></param>
		/// <returns></returns>
		protected int GetItemCount(out int pcItems)
		{
			pcItems = myDefaultColorSettings.Length;
			return NativeMethods.S_OK;
		}
		int IVsFontAndColorDefaults.GetItemCount(out int pcItems)
		{
			return GetItemCount(out pcItems);
		}
		/// <summary>
		/// Implements IVsFontAndColorDefaults.GetPriority
		/// </summary>
		/// <param name="pPriority"></param>
		/// <returns></returns>
		protected int GetPriority(out ushort pPriority)
		{
			pPriority = (ushort)__FCPRIORITY.FCP_CLIENTS;
			return NativeMethods.S_OK;
		}
		int IVsFontAndColorDefaults.GetPriority(out ushort pPriority)
		{
			return GetPriority(out pPriority);
		}
		#endregion // IVsFontAndColorDefaults Implementation
	}
}