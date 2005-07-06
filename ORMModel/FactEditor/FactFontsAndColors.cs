using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio;
namespace Northface.Tools.ORM.FactEditor
{
	#region FactEditorColorizableItem Enum
	/// <summary>
	/// An indexed list of supported color items
	/// for the fact editor. Note that these elements
	/// start at 1. 0 is reserved for plain text and
	/// is never requested.
	/// </summary>
	public enum FactEditorColorizableItem
	{
		/// <summary>
		/// The PredicateText color category
		/// </summary>
		PredicateText = 1,
		/// <summary>
		/// The ObjectName color category
		/// </summary>
		ObjectName,
		/// <summary>
		/// The ReferenceModeName color category
		/// </summary>
		ReferenceModeName,
		/// <summary>
		/// The Delimiter color category
		/// </summary>
		Delimiter,
		/// <summary>
		/// The Quantifier color category
		/// </summary>
		Quantifier,
		// Any order change here needs to be reflected in the myDefaultColorSettings array below
	}
	#endregion // FactEditorColorizableItem Enum
	public partial class FactLanguageService : IVsProvideColorableItems
	{
		#region Constant definitions
		/// <summary>
		/// Bitwise or this value with a value from the ColorIndex array
		/// </summary>
		private const uint StandardColorPaletteBit = 0x01000000;
		#endregion // Constant definitions
		#region Default Settings
		/// <summary>
		/// A helper structure for store default settings
		/// </summary>
		private struct DefaultColorSetting
		{
			public DefaultColorSetting(
				string localizedNameId,
				uint foregroundColor,
				uint backgroundColor,
				bool defaultBold)
			{
				LocalizedNameId = localizedNameId;
				ForegroundColor = foregroundColor;
				BackgroundColor = backgroundColor;
				DefaultBold = defaultBold;
			}
			public string LocalizedNameId;
			public uint ForegroundColor;
			public uint BackgroundColor;
			public bool DefaultBold;
		}
		/// <summary>
		/// Default setting definition. Must be in the same order as
		/// the FactEditorColorizableItem enum
		/// </summary>
		private static readonly DefaultColorSetting[] myDefaultColorSettings = {
			new DefaultColorSetting(
			ResourceStrings.FactEditorColorsPredicateTextId,
			(uint)COLORINDEX.CI_BLUE | StandardColorPaletteBit,
			(uint)COLORINDEX.CI_SYSPLAINTEXT_BK | StandardColorPaletteBit,
			false)
			,new DefaultColorSetting(
			ResourceStrings.FactEditorColorsObjectNameId,
			(int)COLORINDEX.CI_RED | StandardColorPaletteBit,
			(int)COLORINDEX.CI_SYSPLAINTEXT_BK | StandardColorPaletteBit,
			true)
			,new DefaultColorSetting(
			ResourceStrings.FactEditorColorsReferenceModeNameId,
			(uint)COLORINDEX.CI_PURPLE | StandardColorPaletteBit,
			(uint)COLORINDEX.CI_SYSPLAINTEXT_BK | StandardColorPaletteBit,
			true)
			,new DefaultColorSetting(
			ResourceStrings.FactEditorColorsDelimiterId,
			(uint)COLORINDEX.CI_BLACK | StandardColorPaletteBit,
			(uint)COLORINDEX.CI_SYSPLAINTEXT_BK | StandardColorPaletteBit,
			true)
			,new DefaultColorSetting(
			ResourceStrings.FactEditorColorsQuantifierId,
			(uint)COLORINDEX.CI_DARKGREEN | StandardColorPaletteBit,
			(uint)COLORINDEX.CI_SYSPLAINTEXT_BK | StandardColorPaletteBit,
			true)
		};
		#endregion // Default Settings
		#region IVsColorableItem Implementation Class
		private class ColorableItemImpl : IVsColorableItem
		{
			DefaultColorSetting mySetting;
			public ColorableItemImpl(DefaultColorSetting setting)
			{
				mySetting = setting;
			}
			#region IVsColorableItem Implementation
			int IVsColorableItem.GetDefaultColors(COLORINDEX[] piForeground, COLORINDEX[] piBackground)
			{
				piForeground[0] = (COLORINDEX)mySetting.ForegroundColor;
				piBackground[0] = (COLORINDEX)mySetting.BackgroundColor;
				return VSConstants.S_OK;
			}
			int IVsColorableItem.GetDefaultFontFlags(out uint pdwFontFlags)
			{
				pdwFontFlags = (mySetting.DefaultBold) ? (uint)FONTFLAGS.FF_BOLD : 0;
				return VSConstants.S_OK;
			}
			int IVsColorableItem.GetDisplayName(out string pbstrName)
			{
				pbstrName = ResourceStrings.GetColorNameString(mySetting.LocalizedNameId);
				return VSConstants.S_OK;
			}
			#endregion // IVsColorableItem Implementation
		}
		#endregion // IVsColorableItem Implementation Class
		#region IVsProvideColorableItems Implementation
		/// <summary>
		/// Implements IVsProvideColorableItems.GetColorableItem
		/// </summary>
		/// <param name="iIndex"></param>
		/// <param name="ppItem"></param>
		/// <returns></returns>
		protected static int GetColorableItem(int iIndex, out IVsColorableItem ppItem)
		{
			Debug.Assert(iIndex > 0); // Appears to make all calls 1-based
			ppItem = new ColorableItemImpl(myDefaultColorSettings[iIndex - 1]);
			return VSConstants.S_OK;
		}
		int IVsProvideColorableItems.GetColorableItem(int iIndex, out IVsColorableItem ppItem)
		{
			return GetColorableItem(iIndex, out ppItem);
		}
		/// <summary>
		/// Implements IVsProvideColorableItems.GetItemCount
		/// </summary>
		/// <param name="piCount"></param>
		/// <returns></returns>
		protected static int GetItemCount(out int piCount)
		{
			piCount = myDefaultColorSettings.Length;
			return VSConstants.S_OK;
		}
		int IVsProvideColorableItems.GetItemCount(out int piCount)
		{
			return GetItemCount(out piCount);
		}
		#endregion // IVsProvideColorableItems Implementation
	}
}
