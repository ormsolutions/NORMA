#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.Win32;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.Shell; // For ORMDesignerColor enum


namespace ORMSolutions.ORMArchitect.Core.Load
{
	#region VerbalizationManager class
	/// <summary>
	/// A class used with the <see cref="ModelLoader"/> to enable verbalization
	/// of elements in a loaded model.
	/// </summary>
	/// <remarks>Verbalization options of ORM models are tightly coupled with the
	/// extension model. Extension models provide verbalization snippets (static text
	/// with replacement fields), targets (representing the user of the verbalized
	/// output (VerbalizationBrowser, HtmlReport, etc)), and options (miscellaneous
	/// settings controlling verbalization). Verbalization is enabled by providing
	/// a VerbalizationManager to the appropriate ModelLoader constructor.</remarks>
	public class VerbalizationManager
	{
		#region Member Variables
		private string myDirectory;
		private IDictionary<string, object> myOptions;
		private IList<VerbalizationSnippetsIdentifier> mySnippetsIdentifiers;
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// Private constructor, called through public static create methods
		/// </summary>
		/// <param name="snippetsDirectory">The snippet directory location</param>
		private VerbalizationManager(string snippetsDirectory)
		{
			myDirectory = snippetsDirectory;
			myOptions = new Dictionary<string, object>();
			mySnippetsIdentifiers = new List<VerbalizationSnippetsIdentifier>();
		}
		#endregion // Constructor
		#region Static creation methods
		/// <summary>
		/// Create a <see cref="VerbalizationManager"/> with snippets loaded from a registry-
		/// specified directory. The loader checks for information under this key at the given
		/// value in both the local machine (HKEY_LOCAL_MACHINE) and user (HKEY_CURRENT_USER)
		/// registry hives.
		/// </summary>
		/// <param name="keyName">The registry key name from either the application and user roots
		/// or the provided root keys.</param>
		/// <param name="keyValue">The registry value to read the verbalization directory from. A
		/// value of <see langword="null"/> corresponds to the default 'VerbalizationDir' value,
		/// while an empty string corresponds to the default value for the registry key.</param>
		/// <param name="localMachineRootKey">An alternate open key in the local machine hive. The provided
		/// <paramref name="keyName"/> is relative to this key.</param>
		/// <param name="userRootKey">An alternate open key in the current user hive. The provided
		/// <paramref name="keyName"/> is relative to this key.</param>
		/// <returns>A new manager attached to this directory.</returns>
		public static VerbalizationManager LoadFromRegistry(string keyName, string keyValue, RegistryKey localMachineRootKey, RegistryKey userRootKey)
		{
			bool currentUserPass = true; // Check user first to allow override.
			for (; ; )
			{
				RegistryKey rootKey = currentUserPass ? (userRootKey ?? Registry.CurrentUser) : (localMachineRootKey ?? Registry.LocalMachine);
				using (RegistryKey verbalizationKey = rootKey.OpenSubKey(keyName, RegistryKeyPermissionCheck.ReadSubTree))
				{
					if (verbalizationKey != null)
					{
						object valueObj;
						string directory;
						if (null != (valueObj = verbalizationKey.GetValue(keyValue == null ? "VerbalizationDir" : keyValue)) &&
							null != (directory = valueObj as string))
						{
							return LoadFromDirectory(directory);
						}
					}
				}
				if (!currentUserPass)
				{
					break;
				}
				currentUserPass = false;
			}
			return null;
		}
		/// <summary>
		/// Load the verbalization manager using snippets from the given directory
		/// </summary>
		/// <param name="snippetsDirectory">A full directory path</param>
		/// <returns>The <see cref="VerbalizationManager"/> using snippets from this directory.</returns>
		public static VerbalizationManager LoadFromDirectory(string snippetsDirectory)
		{
			return new VerbalizationManager(snippetsDirectory);
		}
		#endregion // Static creation methods
		#region Accessor Properties
		/// <summary>
		/// Get the root directory for snippet files
		/// </summary>
		public string SnippetsDirectory
		{
			get
			{
				return myDirectory;
			}
		}
		/// <summary>
		/// Get the custom options to apply
		/// </summary>
		public IDictionary<string, object> CustomOptions
		{
			get
			{
				return myOptions;
			}
		}
		/// <summary>
		/// Get the custom <see cref="VerbalizationSnippetsIdentifier"/> for this item.
		/// </summary>
		public IList<VerbalizationSnippetsIdentifier> CustomSnippetsIdentifiers
		{
			get
			{
				return mySnippetsIdentifiers;
			}
		}
		#endregion // Accessor Properties
		#region Font and color management
		private string myFontFamilyName;
		private float myFontSize = 8.0F;
		private struct CategoryFontData
		{
			public readonly Color Color;
			public readonly bool IsBold;
			public CategoryFontData(Color color, bool isBold)
			{
				Color = color;
				IsBold = isBold;
			}
			public static CategoryFontData GetDefault(ORMDesignerColor color)
			{
				switch (color)
				{
					case ORMDesignerColor.VerbalizerPredicateText:
						return new CategoryFontData(Color.DarkGreen, false);
					case ORMDesignerColor.VerbalizerObjectName:
						return new CategoryFontData(Color.Purple, false);
					case ORMDesignerColor.VerbalizerFormalItem:
						return new CategoryFontData(Color.MediumBlue, true);
					case ORMDesignerColor.VerbalizerNotesItem:
						return new CategoryFontData(Color.Black, false);
					case ORMDesignerColor.VerbalizerRefMode:
						return new CategoryFontData(Color.Brown, false);
					case ORMDesignerColor.VerbalizerInstanceValue:
						return new CategoryFontData(Color.Brown, false);
				}
				return new CategoryFontData(Color.Black, true);
			}
		}
		private Dictionary<ORMDesignerColor, CategoryFontData> myVerbalizationColors;
		/// <summary>
		/// Manage the font family associated with verbalization. Defaults to "Tahoma".
		/// </summary>
		public string FontFamilyName
		{
			get
			{
				return myFontFamilyName ?? "Tahoma";
			}
			set
			{
				myFontFamilyName = string.IsNullOrEmpty(value) ? null : value;
			}
		}
		/// <summary>
		/// Verbalization font size in points. Defaults to 8 point.
		/// </summary>
		public float FontSize
		{
			get
			{
				return myFontSize;
			}
			set
			{
				myFontSize = value;
			}
		}
		/// <summary>
		/// Get the color for a standard verbalizer color
		/// </summary>
		/// <param name="verbalizerColor">A designer color used for verbalization</param>
		/// <returns>The custom color, or the default color used for this category.</returns>
		public Color GetColor(ORMDesignerColor verbalizerColor)
		{
			Dictionary<ORMDesignerColor, CategoryFontData> dict = myVerbalizationColors;
			CategoryFontData fontData;
			return (dict != null && dict.TryGetValue(verbalizerColor, out fontData)) ? fontData.Color : CategoryFontData.GetDefault(verbalizerColor).Color;
		}
		/// <summary>
		/// Get whether a font is bolded for a verbalizer color category
		/// </summary>
		/// <param name="verbalizerColor">A designer color used for verbalization</param>
		/// <returns>Returns true if the color category is bolded.</returns>
		public bool GetIsBold(ORMDesignerColor verbalizerColor)
		{
			Dictionary<ORMDesignerColor, CategoryFontData> dict = myVerbalizationColors;
			CategoryFontData fontData;
			return (dict != null && dict.TryGetValue(verbalizerColor, out fontData)) ? fontData.IsBold : CategoryFontData.GetDefault(verbalizerColor).IsBold;
		}
		/// <summary>
		/// Set custom color information
		/// </summary>
		/// <param name="verbalizerColor">The color category to set</param>
		/// <param name="color">The color to apply. Use <see langword="null"/>
		/// to revert to the default settings.</param>
		/// <param name="isBold">Whether or not to bold the font. Use <see langword="null"/> to revert to
		/// the default setting.</param>
		public void SetCustomColor(ORMDesignerColor verbalizerColor, Color? color, bool? isBold)
		{
			Dictionary<ORMDesignerColor, CategoryFontData> dict = myVerbalizationColors;
			CategoryFontData defaultData = CategoryFontData.GetDefault(verbalizerColor);
			bool defaultColor = !color.HasValue || defaultData.Color == color.Value;
			bool defaultBold = !isBold.HasValue || defaultData.IsBold == isBold.Value;
			if (defaultColor && defaultBold)
			{
				if (dict != null && dict.ContainsKey(verbalizerColor))
				{
					dict.Remove(verbalizerColor);
					if (dict.Count == 0)
					{
						myVerbalizationColors = null;
					}
				}
			}
			else
			{
				if (dict == null)
				{
					myVerbalizationColors = dict = new Dictionary<ORMDesignerColor, CategoryFontData>();
				}
				dict[verbalizerColor] = new CategoryFontData(defaultColor ? defaultData.Color : color.Value, defaultBold ? defaultData.IsBold : isBold.Value);
			}
		}
		#endregion // Font and color management
		#region Verbalization Methods
		/// <summary>
		/// Using the current verbalization settings, verbalize the specified
		/// elements into the output text writer.
		/// </summary>
		/// <param name="store">The context <see cref="Store"/> returned by <see cref="ModelLoader.Load(Stream)"/>.</param>
		/// <param name="writer">An externally allocated writer to receive the output.</param>
		/// <param name="target">The name of the target to verbalize. The target must match
		/// a known target as registered with the <see cref="VerbalizationTargetProviderAttribute"/> on
		/// a loaded core or extension model.</param>
		/// <param name="elements">One or more elements to verbalize.</param>
		/// <param name="showNegative">Generate a negative verbalization if available.</param>
		public void Verbalize(Store store, TextWriter writer, string target, ICollection elements, bool showNegative)
		{
			IDictionary<Type, IVerbalizationSets> snippetsDictionary = null;
			IDictionary<string, object> options = null;
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = null;
			IExtensionVerbalizerService extensionVerbalizer = null;
			VerbalizationCallbackWriter callbackWriter = null;
			bool firstCallPending = true;
			Dictionary<IVerbalize, IVerbalize> alreadyVerbalized = new Dictionary<IVerbalize, IVerbalize>();
			Dictionary<object, object> locallyVerbalized = new Dictionary<object, object>();
			if (elements != null)
			{
				foreach (object elemIter in elements)
				{
					if (snippetsDictionary == null)
					{
						IORMToolServices toolServices = (IORMToolServices)store;
						extensionVerbalizer = toolServices.ExtensionVerbalizerService;
						options = toolServices.VerbalizationOptions;
						snippetsDictionary = toolServices.GetVerbalizationSnippetsDictionary(target);
						snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
						callbackWriter = new VerbalizationCallbackWriter(snippets, writer, VerbalizationHelper.GetDocumentHeaderReplacementFields(store, snippets));
					}
					object elem = elemIter;
					ModelElement mel = elem as ModelElement;
					PresentationElement pel;
					if ((mel != null) &&
						null != (pel = mel as PresentationElement))
					{
						IRedirectVerbalization shapeRedirect = pel as IRedirectVerbalization;
						if (null == (shapeRedirect = pel as IRedirectVerbalization) ||
							null == (elem = shapeRedirect.SurrogateVerbalizer as ModelElement))
						{
							elem = mel = pel.ModelElement;
						}
					}
					if (elem != null &&
						(mel == null || !mel.IsDeleted))
					{
						locallyVerbalized.Clear();
						VerbalizationHelper.VerbalizeElement(
							elem,
							snippetsDictionary,
							extensionVerbalizer,
							options,
							target,
							alreadyVerbalized,
							locallyVerbalized,
							(showNegative ? VerbalizationSign.Negative : VerbalizationSign.Positive) | VerbalizationSign.AttemptOppositeSign,
							callbackWriter,
							true,
							ref firstCallPending);
					}
				}
			}
			if (!firstCallPending)
			{
				// Write footer
				callbackWriter.WriteDocumentFooter();
				// Clear cache
				alreadyVerbalized.Clear();
			}
		}
		#endregion // Verbalization Methods
	}
	#endregion // VerbalizationManager class
}
