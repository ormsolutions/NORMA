using System;
using System.Diagnostics;
using System.Drawing;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio;
using MSOle = Microsoft.VisualStudio.OLE.Interop;
using System.Runtime.InteropServices;
namespace Neumont.Tools.ORM.Shell
{
	#region ORMDesignerColor Enum
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
		/// The colors used to draw an active constraint
		/// and its associated roles.
		/// </summary>
		ActiveConstraint,
		/// <summary>
		/// The color used to display error conditions
		/// on internal constraints
		/// </summary>
		ConstraintError,
		/// <summary>
		/// The colors used to draw selected constraints
		/// during a role picker mouse action
		/// </summary>
		RolePicker,
		// Items here must be in the same order as myDefaultColorSettings
		// defined in the ORMDesignerFontsAndColors class. Also need to
		// update NameFromItemIndex when adding/removing items here.
	}
	#endregion // ORMDesignerColor Enum
	#region IORMFontAndColorService
	/// <summary>
	/// Abstract 
	/// </summary>
	public interface IORMFontAndColorService
	{
		/// <summary>
		/// Retrieve forecolor information for the specified index
		/// </summary>
		/// <param name="colorIndex">Item to retrieve</param>
		/// <returns>Color</returns>
		Color GetForeColor(ORMDesignerColor colorIndex);
		/// <summary>
		/// Retrieve background color information for the specified index
		/// </summary>
		/// <param name="colorIndex">Item to retrieve</param>
		/// <returns>Color</returns>
		Color GetBackColor(ORMDesignerColor colorIndex);
		/// <summary>
		/// Retrieve font information. A new Font object is generated
		/// on each call and must be disposed of properly by the caller.
		/// </summary>
		/// <returns>A new font object</returns>
		Font GetFont();
		/// <summary>
		/// Retrieve font flag information for the specified index
		/// </summary>
		/// <param name="colorIndex">Item to retrieve</param>
		/// <returns>FONTFLAGS</returns>
		FONTFLAGS GetFontFlags(ORMDesignerColor colorIndex);
	}
	#endregion // IORMFontAndColorService interface
	/// <summary>
	/// The class to create the ORM Designer category in the tools/options/environment/fonts and colors page
	/// </summary>
	[Guid("C5AA80F8-F730-4809-AAB1-8D925E36F9F5")]
	public partial class ORMDesignerFontsAndColors : IVsFontAndColorDefaultsProvider, IVsFontAndColorDefaults, IVsFontAndColorEvents, IORMFontAndColorService
	{
		#region Constant definitions
		/// <summary>
		/// Bitwise or this value with a value from the COLORINDEX enum
		/// </summary>
		private const uint StandardPaletteBit = 0x01000000;
		/// <summary>
		/// Bitwise or this value with a system color value. Retrieve with
		/// the GetSysColor function.
		/// </summary>
		private const uint SystemColorBit = 0x10000000;
		/// <summary>
		/// Bitwise or this value with a system color value. Retrieve with
		/// the  IVsUIShell::GetVSSysColor function.
		/// </summary>
		private const uint VsColorBit = 0x20000000;
		/// <summary>
		/// The color refers to the forecolor of another indexed color in the same set
		/// </summary>
		private const uint TrackForegroundColorBit = 0x04000000;
		/// <summary>
		/// The color refers to the back color of another indexed color in the same set
		/// </summary>
		private const uint TrackBackgroundColorBit = 0x08000000;
		/// <summary>
		/// A mask value for all special bits
		/// </summary>
		private const uint AllSpecialColorBits = StandardPaletteBit | SystemColorBit | VsColorBit | TrackForegroundColorBit | TrackBackgroundColorBit;
		/// <summary>
		/// The guid for the font and color category used for this language service
		/// </summary>
		public static readonly Guid FontAndColorCategory = new Guid("663DE24F-8E3A-4C0F-A307-53053ED6C59B");
		/// <summary>
		/// The guid for the TextEditor Category
		/// </summary>
		private static readonly Guid TextEditorCategory = new Guid(0xa27b4e24, 0xa735, 0x4d1d, 0xb8, 0xe7,  0x97,  0x16,  0xe1,  0xe3,  0xd8,  0xe0);
		/// <summary>
		/// The unlocalized name for the constraint display item
		/// </summary>
		public const string ConstraintColorName = "ORM Constraint";
		/// <summary>
		/// The unlocalized name for the constraint error display item
		/// </summary>
		public const string ConstraintErrorColorName = "ORM Constraint (Error)";
		/// <summary>
		/// The unlocalized name for the active constraint display item
		/// </summary>
		public const string ActiveConstraintColorName = "ORM Constraint (Active)";
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
			(uint)ColorTranslator.ToWin32(Color.Purple),
			(int)COLORINDEX.CI_SYSPLAINTEXT_BK | StandardPaletteBit,
			__FCITEMFLAGS.FCIF_ALLOWFGCHANGE | __FCITEMFLAGS.FCIF_ALLOWCUSTOMCOLORS,
			false)
			,new DefaultColorSetting(
			ActiveConstraintColorName,
			ResourceStrings.FontsAndColorsActiveConstraintColorId,
			(uint)COLORINDEX.CI_SYSSEL_FG | StandardPaletteBit,
			(uint)COLORINDEX.CI_SYSSEL_BK | StandardPaletteBit,
			__FCITEMFLAGS.FCIF_ALLOWBGCHANGE | __FCITEMFLAGS.FCIF_ALLOWFGCHANGE | __FCITEMFLAGS.FCIF_ALLOWCUSTOMCOLORS,
			false)
			,new DefaultColorSetting(
			ConstraintErrorColorName,
			ResourceStrings.FontsAndColorsConstraintErrorColorId,
			(uint)COLORINDEX.CI_RED | StandardPaletteBit,
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
		#region Constructor
		private IServiceProvider myServiceProvider;
		/// <summary>
		/// Create a new ORMDesignerFontsAndColors
		/// </summary>
		/// <param name="serviceProvider">Service provider</param>
		public ORMDesignerFontsAndColors(IServiceProvider serviceProvider)
		{
			myServiceProvider = serviceProvider;
		}
		#endregion // Constructor
		#region Settings Cache
		/// <summary>
		/// Struct for caching values
		/// </summary>
		private struct ColorItem
		{
			public Color ForeColor;
			public Color BackColor;
			public FONTFLAGS FontFlags;
		}
		/// <summary>
		/// Helper structure to retrieve color and font values
		/// </summary>
		private struct ColorRetriever
		{
			/// <summary>
			/// A delegate to translate an item index into a name
			/// </summary>
			/// <param name="itemIndex">The item index</param>
			/// <returns>Item name string</returns>
			public delegate string NameFromIndex(int itemIndex);
			/// <summary>
			/// Initialize a ColorRetriever structure
			/// </summary>
			/// <param name="serviceProvider"></param>
			/// <param name="categoryGuid"></param>
			/// <param name="indexConverter">Delegate to convert a name into an index</param>
			public ColorRetriever(IServiceProvider serviceProvider, Guid categoryGuid, NameFromIndex indexConverter)
			{
				myServiceProvider = serviceProvider;
				myShell = null;
				myStorage = null;
				myStorageOpen = false;
				myUserForeColor = Color.Empty;
				myUserBackColor = Color.Empty;
				myCategoryGuid = categoryGuid;
				myGetItemParam = new ColorableItemInfo[1];
				myIndexConverter = indexConverter;
			}
			private bool myStorageOpen;
			private IServiceProvider myServiceProvider;
			private IVsUIShell2 myShell;
			private IVsFontAndColorStorage myStorage;
			private Color myUserForeColor;
			private Color myUserBackColor;
			private Guid myCategoryGuid;
			private ColorableItemInfo[] myGetItemParam;
			private NameFromIndex myIndexConverter;
			/// <summary>
			/// Call from a finally to clean up correctly
			/// </summary>
			public void Close()
			{
				if (myStorage != null && myStorageOpen)
				{
					myStorage.CloseCategory(); // Ignore hresult return
				}
			}
			/// <summary>
			/// Get the color information for a specific item
			/// </summary>
			/// <param name="itemIndex">The index of an item in the category
			/// used to create the ColorRetriever</param>
			/// <returns>ColorItem structure</returns>
			public ColorItem GetColorItem(int itemIndex)
			{
				ColorItem retVal = new ColorItem();
				EnsureStorage();
				ColorableItemInfo item = new ColorableItemInfo();
				myGetItemParam[0] = item;
				ErrorHandler.ThrowOnFailure(myStorage.GetItem(myIndexConverter(itemIndex), myGetItemParam));
				item = myGetItemParam[0];
				retVal.FontFlags = (item.bFontFlagsValid != 0) ? (FONTFLAGS)item.dwFontFlags : FONTFLAGS.FF_DEFAULT;
				retVal.ForeColor = (item.bForegroundValid != 0) ? TranslateColorValue(item.crForeground) : Color.Empty;
				retVal.BackColor = (item.bBackgroundValid != 0) ? TranslateColorValue(item.crBackground) : Color.Empty;
				return retVal;
			}
			/// <summary>
			/// Retrieve font information for the current category
			/// </summary>
			/// <param name="logFont">LOGFONTW structure (out)</param>
			/// <param name="fontInfo">FontInfo structure (out)</param>
			public void GetFont(out LOGFONTW logFont, out FontInfo fontInfo)
			{
				EnsureStorage();
				LOGFONTW[] logFontParam = new LOGFONTW[1];
				FontInfo[] fontInfoParam = new FontInfo[1];
				ErrorHandler.ThrowOnFailure(myStorage.GetFont(logFontParam, fontInfoParam));
				logFont = logFontParam[0];
				fontInfo = fontInfoParam[0];
			}
			private void EnsureStorage()
			{
				if (!myStorageOpen)
				{
					if (myStorage == null)
					{
						myStorage = (IVsFontAndColorStorage)myServiceProvider.GetService(typeof(IVsFontAndColorStorage));
					}
					ErrorHandler.ThrowOnFailure(myStorage.OpenCategory(ref myCategoryGuid, (uint)(__FCSTORAGEFLAGS.FCSF_READONLY | __FCSTORAGEFLAGS.FCSF_LOADDEFAULTS)));
					myStorageOpen = true;
				}
			}
			private void EnsureUserTextColors()
			{
				if (myUserForeColor.IsEmpty)
				{
					if (myStorageOpen)
					{
						myStorageOpen = false;
						myStorage.CloseCategory(); // Ignore return
					}
					Debug.Assert(myStorage != null); // All paths to this lead through EnsureStore
					Guid textCategory = TextEditorCategory;
					ErrorHandler.ThrowOnFailure(myStorage.OpenCategory(ref textCategory, (uint)(__FCSTORAGEFLAGS.FCSF_READONLY | __FCSTORAGEFLAGS.FCSF_LOADDEFAULTS)));
					try
					{
						ColorableItemInfo item = new ColorableItemInfo();
						myGetItemParam[0] = item;
						myStorage.GetItem("Plain Text", myGetItemParam);
						item = myGetItemParam[0];
						myUserForeColor = TranslateColorValue(item.crForeground);
						myUserBackColor = TranslateColorValue(item.crBackground);
					}
					finally
					{
						myStorage.CloseCategory(); // Ignore return
					}
				}
			}
			[DllImport("user32.dll", ExactSpelling = true)]
			private static extern uint GetSysColor(int nIndex);
			private Color TranslateColorValue(uint colorValue)
			{
				if (0 != (colorValue & AllSpecialColorBits))
				{
					uint coreColorValue = colorValue & ~AllSpecialColorBits;
					switch (colorValue & AllSpecialColorBits)
					{
						case StandardPaletteBit:
							Color knownColor = Color.Empty;
							switch ((COLORINDEX)coreColorValue)
							{
								case COLORINDEX.CI_USERTEXT_FG:
									EnsureUserTextColors();
									knownColor = myUserForeColor;
									break;
								case COLORINDEX.CI_USERTEXT_BK:
									EnsureUserTextColors();
									knownColor = myUserBackColor;
									break;

								case COLORINDEX.CI_BLACK:
									knownColor = Color.Black;
									break;
								case COLORINDEX.CI_WHITE:
									knownColor = Color.White;
									break;
								case COLORINDEX.CI_MAROON:
									knownColor = Color.Maroon;
									break;
								case COLORINDEX.CI_DARKGREEN:
									knownColor = Color.DarkGreen;
									break;
								case COLORINDEX.CI_BROWN:
									knownColor = Color.Brown;
									break;
								case COLORINDEX.CI_DARKBLUE:
									knownColor = Color.DarkBlue;
									break;
								case COLORINDEX.CI_PURPLE:
									knownColor = Color.Purple;
									break;
								case COLORINDEX.CI_AQUAMARINE:
									knownColor = Color.Aquamarine;
									break;
								case COLORINDEX.CI_LIGHTGRAY:
									knownColor = Color.LightGray;
									break;
								case COLORINDEX.CI_DARKGRAY:
									knownColor = Color.DarkGray;
									break;
								case COLORINDEX.CI_RED:
									knownColor = Color.Red;
									break;
								case COLORINDEX.CI_GREEN:
									knownColor = Color.Green;
									break;
								case COLORINDEX.CI_YELLOW:
									knownColor = Color.Yellow;
									break;
								case COLORINDEX.CI_BLUE:
									knownColor = Color.Blue;
									break;
								case COLORINDEX.CI_MAGENTA:
									knownColor = Color.Magenta;
									break;
								case COLORINDEX.CI_CYAN:
									knownColor = Color.Cyan;
									break;

								case COLORINDEX.CI_SYSSEL_FG:
									knownColor = SystemColors.HighlightText;
									break;
								case COLORINDEX.CI_SYSSEL_BK:
									knownColor = SystemColors.Highlight;
									break;
								case COLORINDEX.CI_SYSINACTSEL_FG:
									knownColor = SystemColors.InactiveCaptionText;
									break;
								case COLORINDEX.CI_SYSINACTSEL_BK:
									knownColor = SystemColors.InactiveCaption;
									break;
								case COLORINDEX.CI_SYSWIDGETMGN_BK:
									knownColor = SystemColors.ButtonFace;
									break;
								case COLORINDEX.CI_SYSPLAINTEXT_FG:
									knownColor = SystemColors.WindowText;
									break;
								case COLORINDEX.CI_SYSPLAINTEXT_BK:
									knownColor = SystemColors.Window;
									break;
							}
							return knownColor;
						case SystemColorBit:
							colorValue = GetSysColor((int)coreColorValue);
							break;
						case VsColorBit:
							if (myShell == null)
							{
								myShell = (IVsUIShell2)myServiceProvider.GetService(typeof(IVsUIShell));
							}
							ErrorHandler.ThrowOnFailure(myShell.GetVSSysColorEx((int)coreColorValue, out colorValue));
							break;
						case TrackForegroundColorBit:
							{
								ColorableItemInfo[] myGetItemParam = new ColorableItemInfo[1];
								ErrorHandler.ThrowOnFailure(myStorage.GetItem(myIndexConverter((int)coreColorValue), myGetItemParam));
								return TranslateColorValue(myGetItemParam[0].crForeground);
							}
						case TrackBackgroundColorBit:
							{
								ColorableItemInfo[] myGetItemParam = new ColorableItemInfo[1];
								ErrorHandler.ThrowOnFailure(myStorage.GetItem(myIndexConverter((int)coreColorValue), myGetItemParam));
								return TranslateColorValue(myGetItemParam[0].crBackground);
							}
					}
				}
				return ColorTranslator.FromWin32((int)colorValue);
			}
		}
		private ColorItem[] myColors;
		private LOGFONTW myLogFont = new LOGFONTW();
		private FontInfo myFontInfo = new FontInfo();
		private bool mySettingsChangePending; // Set to true on events, no effect until OnApply fires
		#region IORMFontAndColorService implementation
		/// <summary>
		/// Retrieve font information. A new Font object is generated
		/// on each call and must be disposed of properly by the caller.
		/// Implements IORMFontAndColorService.GetFont
		/// </summary>
		/// <returns>A new font object</returns>
		protected Font GetFont()
		{
			EnsureCache();
			FontInfo fontInfo = myFontInfo;
			Debug.Assert(fontInfo.bFaceNameValid != 0 && fontInfo.bCharSetValid != 0 && fontInfo.bPointSizeValid != 0);
			return new Font(fontInfo.bstrFaceName, fontInfo.wPointSize / 72.0f, FontStyle.Regular, GraphicsUnit.World, fontInfo.iCharSet);
		}
		Font IORMFontAndColorService.GetFont()
		{
			return GetFont();
		}
		/// <summary>
		/// Retrieve forecolor information for the specified index.
		/// Implements IORMFontAndColorService.GetForeColor
		/// </summary>
		/// <param name="colorIndex">Item to retrieve</param>
		/// <returns>Color</returns>
		protected Color GetForeColor(ORMDesignerColor colorIndex)
		{
			EnsureCache();
			return myColors[(int)colorIndex].ForeColor;
		}
		Color IORMFontAndColorService.GetForeColor(ORMDesignerColor colorIndex)
		{
			return GetForeColor(colorIndex);
		}
		/// <summary>
		/// Retrieve background color information for the specified index.
		/// Implements IORMFontAndColorService.GetBackColor
		/// </summary>
		/// <param name="colorIndex">Item to retrieve</param>
		/// <returns>Color</returns>
		protected Color GetBackColor(ORMDesignerColor colorIndex)
		{
			EnsureCache();
			return myColors[(int)colorIndex].BackColor;
		}
		Color IORMFontAndColorService.GetBackColor(ORMDesignerColor colorIndex)
		{
			return GetBackColor(colorIndex);
		}
		/// <summary>
		/// Retrieve font flag information for the specified index.
		/// Implements IORMFontAndColorService.GetFontFlags
		/// </summary>
		/// <param name="colorIndex">Item to retrieve</param>
		/// <returns>FONTFLAGS</returns>
		protected FONTFLAGS GetFontFlags(ORMDesignerColor colorIndex)
		{
			EnsureCache();
			return myColors[(int)colorIndex].FontFlags;
		}
		FONTFLAGS IORMFontAndColorService.GetFontFlags(ORMDesignerColor colorIndex)
		{
			return GetFontFlags(colorIndex);
		}
		#endregion // IORMFontAndColorService implementation
		private void EnsureCache()
		{
			if (myColors == null)
			{
				FillCache();
			}
		}
		private void ClearCache()
		{
			if (myColors != null)
			{
				myColors = null;
				myLogFont = new LOGFONTW();
				myFontInfo = new FontInfo();
			}
		}
		private void FillCache()
		{
			Debug.Assert(myColors == null);
			ColorRetriever retriever = new ColorRetriever(myServiceProvider, FontAndColorCategory, new ColorRetriever.NameFromIndex(NameFromItemIndex));
			try
			{
				retriever.GetFont(out myLogFont, out myFontInfo);
				int itemCount = myDefaultColorSettings.Length;
				myColors = new ColorItem[itemCount];
				for (int i = 0; i < itemCount; ++i)
				{
					myColors[i] = retriever.GetColorItem(i);
				}
			}
			finally
			{
				retriever.Close();
			}
		}
		private static string NameFromItemIndex(int itemIndex)
		{
			return NameFromItemIndex((ORMDesignerColor)itemIndex);
		}
		/// <summary>
		/// Get the name of an item from its index
		/// </summary>
		/// <param name="itemIndex">A valid item index</param>
		/// <returns>A name. Throws on an invalid index</returns>
		private static string NameFromItemIndex(ORMDesignerColor itemIndex)
		{
			string retVal;
			switch (itemIndex)
			{
				case ORMDesignerColor.Constraint:
					retVal = ConstraintColorName;
					break;
				case ORMDesignerColor.ConstraintError:
					retVal = ConstraintErrorColorName;
					break;
				case ORMDesignerColor.RolePicker:
					retVal = RolePickerColorName;
					break;
				case ORMDesignerColor.ActiveConstraint:
					retVal = ActiveConstraintColorName;
					break;
				default:
					Debug.Assert(false); // The cases may not match all of the ORMDesignerColor enums.
					throw new ArgumentOutOfRangeException();
			}
			return retVal;
		}
		#endregion // Read Cache
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
				return VSConstants.S_OK;
			}
			ppObj = null;
			return VSConstants.E_NOINTERFACE;
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
		protected static int GetBaseCategory(out Guid pguidBase)
		{
			pguidBase = Guid.Empty;
			return VSConstants.E_NOTIMPL;
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
		protected static int GetCategoryName(out string pbstrName)
		{
			pbstrName = ResourceStrings.GetColorNameString(ResourceStrings.FontsAndColorsCategoryNameId);
			return VSConstants.S_OK;
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
		protected static int GetFlags(out uint dwFlags)
		{
			// Pull values from the __FONTCOLORFLAGS enum
			dwFlags = (uint)(__FONTCOLORFLAGS.FCF_MUSTRESTART);
			return VSConstants.S_OK;
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
		protected static int GetFont(FontInfo[] pInfo)
		{
			FontInfo info = new FontInfo();
			info.bstrFaceName = "Tahoma";
			info.bFaceNameValid = 1;
			info.wPointSize = 7;
			info.bPointSizeValid = 1;
			info.iCharSet = (byte)EnvDTE.vsFontCharSet.vsFontCharSetDefault;
			info.bCharSetValid = 1;
			pInfo[0] = info;
			return VSConstants.S_OK;
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
		protected static int GetItem(int iItem, AllColorableItemInfo[] pInfo)
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
			return VSConstants.S_OK;
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
		protected static int GetItemByName(string szItem, AllColorableItemInfo[] pInfo)
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
			return VSConstants.E_INVALIDARG;
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
		protected static int GetItemCount(out int pcItems)
		{
			pcItems = myDefaultColorSettings.Length;
			return VSConstants.S_OK;
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
		protected static int GetPriority(out ushort pPriority)
		{
			pPriority = (ushort)__FCPRIORITY.FCP_CLIENTS;
			return VSConstants.S_OK;
		}
		int IVsFontAndColorDefaults.GetPriority(out ushort pPriority)
		{
			return GetPriority(out pPriority);
		}
		#endregion // IVsFontAndColorDefaults Implementation
		#region IVsFontAndColorEvents Implementation
		private void OnChange(ref Guid rguidCategory)
		{
			if (rguidCategory == FontAndColorCategory)
			{
				mySettingsChangePending = true;
			}
		}
		/// <summary>
		/// Implements IVsFontAndColorEvents.OnApply
		/// </summary>
		protected int OnApply()
		{
			// This is a really unfortunate ommission on the
			// part of VS because there is an OnApply, but no
			// OnCancel. All of the new information is provided
			// in the On*Changed events, but we can't use it
			// because we don't no when to toss it. Therefore,
			// we have to go all the way back and reset all
			// values on any change.
			// We don't even know if mySettingsChangePending
			// was set by a previously canceled foray into
			// the font and color settings!
			if (mySettingsChangePending)
			{
				mySettingsChangePending = false;
				ClearCache();
				OptionsPage.NotifySettingsChange(myServiceProvider, ChangeDocumentFontAndColors);
			}
			return VSConstants.S_OK;
		}
		int IVsFontAndColorEvents.OnApply()
		{
			return OnApply();
		}
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
		/// <summary>
		/// Callback function for modifying a document when font
		/// and color changes are applied.
		/// </summary>
		/// <param name="docData">Currently running docdata</param>
		protected static void ChangeDocumentFontAndColors(ORMDesignerDocData docData)
		{
			foreach (ORMDesignerDocView docView in docData.DocViews)
			{
				// UNDONE: This doesn't actually do anything right now. It triggers
				// RefreshResources on all of the loaded style sets, but this does
				// not re-retrieve the resources. The FCF_MUSTRESTART flags can be
				// removed when we find a way to actually trigger a change here.
				// 0x15 == WM_SYSCOLORCHANGE
				SendMessage(docView.CurrentDesigner.DiagramClientView.Handle, 0x15, 0, 0);
			}
		}
		/// <summary>
		/// Implements IVsFontAndColorEvents.OnFontChanged
		/// </summary>
		protected int OnFontChanged(ref Guid rguidCategory, FontInfo[] pInfo, LOGFONTW[] pLOGFONT, uint HFONT)
		{
			OnChange(ref rguidCategory);
			return VSConstants.S_OK;
		}
		int IVsFontAndColorEvents.OnFontChanged(ref Guid rguidCategory, FontInfo[] pInfo, LOGFONTW[] pLOGFONT, uint HFONT)
		{
			return OnFontChanged(ref rguidCategory, pInfo, pLOGFONT, HFONT);
		}
		/// <summary>
		/// Implements IVsFontAndColorEvents.OnItemChanged
		/// </summary>
		protected int OnItemChanged(ref Guid rguidCategory, string szItem, int iItem, ColorableItemInfo[] pInfo, uint crLiteralForeground, uint crLiteralBackground)
		{
			OnChange(ref rguidCategory);
			return VSConstants.S_OK;
		}
		int IVsFontAndColorEvents.OnItemChanged(ref Guid rguidCategory, string szItem, int iItem, ColorableItemInfo[] pInfo, uint crLiteralForeground, uint crLiteralBackground)
		{
			OnChange(ref rguidCategory);
			return OnItemChanged(ref rguidCategory, szItem, iItem, pInfo, crLiteralForeground, crLiteralBackground);
		}
		/// <summary>
		/// Implements IVsFontAndColorEvents.OnReset
		/// </summary>
		protected int OnReset(ref Guid rguidCategory)
		{
			OnChange(ref rguidCategory);
			return VSConstants.S_OK;
		}
		int IVsFontAndColorEvents.OnReset(ref Guid rguidCategory)
		{
			return OnReset(ref rguidCategory);
		}
		/// <summary>
		/// Implements IVsFontAndColorEvents.OnResetToBaseCategory
		/// </summary>
		protected int OnResetToBaseCategory(ref Guid rguidCategory)
		{
			OnChange(ref rguidCategory);
			return VSConstants.S_OK;
		}
		int IVsFontAndColorEvents.OnResetToBaseCategory(ref Guid rguidCategory)
		{
			return OnResetToBaseCategory(ref rguidCategory);
		}
		#endregion // IVsFontAndColorEvents Implementation
	}
}
