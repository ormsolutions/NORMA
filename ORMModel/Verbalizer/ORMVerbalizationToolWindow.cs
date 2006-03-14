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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;
using System.ComponentModel.Design;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// ToolWindow for hosting a web browser for verbalizations
	/// </summary>
	[Guid("C9AA5E71-9193-46c9-971A-CB6365ACA338")]
	[CLSCompliant(false)]
	public class ORMVerbalizationToolWindow : ORMToolWindow
	{
		#region Member variables
		private WebBrowser myWebBrowser;
		private bool myShowNegativeVerbalizations;
		private StringWriter myStringWriter;
		private static string[] myDocumentHeaderReplacementFields;
		private IDictionary<Type, IVerbalizationSets> mySnippetsDictionary;
		/// <summary>
		/// Callback for child verbalizations
		/// </summary>
		private delegate bool VerbalizationHandler(IVerbalize verbalizer, int indentationLevel);
		#endregion // Member variables
		#region Accessor Properties
		/// <summary>
		/// Show negative verbalizations if available
		/// </summary>
		public bool ShowNegativeVerbalizations
		{
			get
			{
				return myShowNegativeVerbalizations;
			}
			set
			{
				if (myShowNegativeVerbalizations != value)
				{
					myShowNegativeVerbalizations = value;
					UpdateVerbalization();
				}
			}
		}
		#endregion // Accessor Properties
		#region Construction
		/// <summary>
		/// Construct a verbalization window with a monitor selection service
		/// </summary>
		/// <param name="serviceProvider">Service provider</param>
		public ORMVerbalizationToolWindow(IServiceProvider serviceProvider) : base(serviceProvider) { }
		/// <summary>
		/// Initialize here after we have the frame so we can grab the toolbar host
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();
			IVsToolWindowToolbarHost host = ToolBarHost;
			Debug.Assert(host != null); // Should be set with HasToolbar true
			if (host != null)
			{
				CommandID command = ORMDesignerDocView.ORMDesignerCommandIds.VerbalizationToolBar;
				Guid commandGuid = command.Guid;
				host.AddToolbar(VSTWT_LOCATION.VSTWT_LEFT, ref commandGuid, (uint)command.ID);
			}
			// create the string writer to hold the html
			StringBuilder builder = new StringBuilder();
			myStringWriter = new StringWriter(builder, CultureInfo.CurrentUICulture);
		}
		/// <summary>
		/// Make sure the toolbar flag gets set
		/// </summary>
		protected override bool HasToolBar
		{
			get
			{
				return true;
			}
		}
		#endregion // Construction
		#region Selection monitor event handlers and helpers
		private void ModelStateChangedEvent(object sender, ElementEventsEndedEventArgs e)
		{
			UpdateVerbalization();
		}
		/// <summary>
		/// Called when the options dialog settings have changed
		/// </summary>
		public void SettingsChanged()
		{
			myDocumentHeaderReplacementFields = null;
			UpdateVerbalization();
		}
		#endregion // Selection monitor event handlers and helpers
		#region Overrides
		/// <summary>
		/// See <see cref="ToolWindow.BitmapResource"/>.
		/// </summary>
		protected override int BitmapResource
		{
			get
			{
				return 125;
			}
		}
		/// <summary>
		/// See <see cref="ToolWindow.BitmapResource"/>.
		/// </summary>
		protected override int BitmapIndex
		{
			get
			{
				return 0;
			}
		}
		/// <summary>
		/// Gets the web browser control hosted in the tool window
		/// </summary>
		public override IWin32Window Window
		{
			get
			{
				WebBrowser browser = myWebBrowser;
				if (browser == null)
				{
					myWebBrowser = browser = new WebBrowser();
					browser.Dock = DockStyle.Fill;
					StringWriter writer = myStringWriter;
					browser.DocumentText = (writer != null) ? writer.ToString() : "";
					// The container magically provides resize support, we don't have
					// to go all the way to a form
					ContainerControl container = new ContainerControl();
					container.Controls.Add(browser);
				}
				return browser.Parent;
			}
		}
		/// <summary>
		/// Clean up any existing objects
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				// TODO: Remove event handlers for monitor selection
				if (myWebBrowser != null)
				{
					(myWebBrowser as IDisposable).Dispose();
					myWebBrowser = null;
				}
				if (myStringWriter != null)
				{
					(myStringWriter as IDisposable).Dispose();
					myStringWriter = null;
				}
			}
		}
		#endregion // Overrides
		#region Verbalization Implementation
		/// <summary>
		/// Get the 8 document header replacement fields from the current font and color settings
		/// </summary>
		private static string[] GetDocumentHeaderReplacementFields(ModelElement element, VerbalizationSets<CoreVerbalizationTextSnippetType> snippets)
		{
			string[] retVal = myDocumentHeaderReplacementFields;
			if (retVal == null)
			{
				// The replacement fields, pulled from VerbalizationGenerator.xsd
				//{0} font-family
				//{1} font-size
				//{2} predicate text color
				//{3} predicate text bold
				//{4} object name color
				//{5} object name bold
				//{6} formal item color
				//{7} formal item bold
				//{8} notes item color
				//{9} notes item bold
				//{10} refmode item color
				//{11} refmode item bold
				IORMFontAndColorService colorService = ((IORMToolServices)element.Store).FontAndColorService;
				string boldWeight = snippets.GetSnippet(CoreVerbalizationTextSnippetType.VerbalizerFontWeightBold);
				string normalWeight = snippets.GetSnippet(CoreVerbalizationTextSnippetType.VerbalizerFontWeightNormal);
				retVal = new string[] { "Tahoma", "8", "darkgreen", normalWeight, "purple", normalWeight, "mediumblue", boldWeight, "brown", normalWeight, "darkgray", normalWeight };
				using (Font font = colorService.GetFont(ORMDesignerColorCategory.Verbalizer))
				{
					retVal[0] = font.FontFamily.Name;
					retVal[1] = (font.Size * 72f).ToString(CultureInfo.InvariantCulture);
					retVal[2] = ColorTranslator.ToHtml(colorService.GetForeColor(ORMDesignerColor.VerbalizerPredicateText));
					retVal[3] = (0 != (colorService.GetFontFlags(ORMDesignerColor.VerbalizerPredicateText) & FONTFLAGS.FF_BOLD)) ? boldWeight : normalWeight;
					retVal[4] = ColorTranslator.ToHtml(colorService.GetForeColor(ORMDesignerColor.VerbalizerObjectName));
					retVal[5] = (0 != (colorService.GetFontFlags(ORMDesignerColor.VerbalizerObjectName) & FONTFLAGS.FF_BOLD)) ? boldWeight : normalWeight;
					retVal[6] = ColorTranslator.ToHtml(colorService.GetForeColor(ORMDesignerColor.VerbalizerFormalItem));
					retVal[7] = (0 != (colorService.GetFontFlags(ORMDesignerColor.VerbalizerFormalItem) & FONTFLAGS.FF_BOLD)) ? boldWeight : normalWeight;
					retVal[8] = ColorTranslator.ToHtml(colorService.GetForeColor(ORMDesignerColor.VerbalizerNotesItem));
					retVal[9] = (0 != (colorService.GetFontFlags(ORMDesignerColor.VerbalizerNotesItem) & FONTFLAGS.FF_BOLD)) ? boldWeight : normalWeight;
					retVal[10] = ColorTranslator.ToHtml(colorService.GetForeColor(ORMDesignerColor.VerbalizerRefMode));
					retVal[11] = (0 != (colorService.GetFontFlags(ORMDesignerColor.VerbalizerRefMode) & FONTFLAGS.FF_BOLD)) ? boldWeight : normalWeight;
				}
				myDocumentHeaderReplacementFields = retVal;
			}
			return myDocumentHeaderReplacementFields;
		}
		#region UNDONE: Overridable snippet sets
		#if FALSE
		// UNDONE: This is a sample overridable VerbalizationSets. Use as the basis
		// for loading snippet overrides from an Xml file
		public abstract class PassthroughVerbalizationSets<EnumType> : VerbalizationSets<EnumType> where EnumType : struct
		{
			private VerbalizationSets<EnumType> myDeferTo;
			public PassthroughVerbalizationSets(VerbalizationSets<EnumType> deferTo)
			{
				myDeferTo = deferTo;
			}
			public override string GetSnippet(EnumType snippetType, bool isDeontic, bool isNegative)
			{
				string retVal = base.GetSnippet(snippetType, isDeontic, isNegative);
				return (retVal == null) ? myDeferTo.GetSnippet(snippetType, isDeontic, isNegative) : retVal;
			}
		}
		private class DefaultSetOverride : PassthroughVerbalizationSets<CoreVerbalizationTextSnippetType>
		{
			public DefaultSetOverride() : base(CoreVerbalizationSets.Default) { }
			protected override void PopulateVerbalizationSets(VerbalizationSets<CoreVerbalizationTextSnippetType>.VerbalizationSet[] sets)
			{
				DictionaryVerbalizationSet set = new DictionaryVerbalizationSet();
				sets[GetSetIndex(false, false)] = set;
				IDictionary<CoreVerbalizationTextSnippetType, string> dictionary = set.Dictionary;
				dictionary.Add(CoreVerbalizationTextSnippetType.ObjectType, @"<span class=""objectType"" style=""text-decoration:underline"">OBJECTYPE:{0}</span>");
			}
			/// <summary>
			/// Converts enum value of CoreVerbalizationTextSnippetType to an integer index value.
			/// </summary>
			protected override int ValueToIndex(CoreVerbalizationTextSnippetType enumValue)
			{
				return (int)enumValue;
			}
		}
		#endif // FALSE
		#endregion // UNDONE: Overridable snippet sets
		/// <summary>
		/// Get the snippet sets dictionary
		/// </summary>
		private IDictionary<Type, IVerbalizationSets> SnippetsDictionary
		{
			get
			{
				// UNDONE: Support loading from somewhere other than default and
				// support extension snippet sets
				IDictionary<Type, IVerbalizationSets> retVal = mySnippetsDictionary;
				if (retVal == null)
				{
					retVal = new Dictionary<Type, IVerbalizationSets>();
					retVal.Add(typeof(CoreVerbalizationTextSnippetType), CoreVerbalizationSets.Default);
					mySnippetsDictionary = retVal;
				}
				return retVal;
			}
		}
		private void UpdateVerbalization()
		{
			if (myCurrentORMSelectionContainer == null)
			{
				return;
			}
			if (myStringWriter != null)
			{
				myStringWriter.GetStringBuilder().Length = 0;

				ICollection selectedObjects = base.GetSelectedComponents();
				IDictionary<Type, IVerbalizationSets> snippetsDictionary = SnippetsDictionary;
				VerbalizationSets<CoreVerbalizationTextSnippetType> snippets = (VerbalizationSets<CoreVerbalizationTextSnippetType>)snippetsDictionary[typeof(CoreVerbalizationTextSnippetType)];
				myStringWriter.NewLine = snippets.GetSnippet(CoreVerbalizationTextSnippetType.VerbalizerNewLine);
				bool showNegative = myShowNegativeVerbalizations;
				bool firstCallPending = true;
				foreach (ModelElement melIter in selectedObjects)
				{
					ModelElement mel = melIter;
					PresentationElement pel = mel as PresentationElement;
					if (pel != null)
					{
						mel = pel.ModelElement;
					}
					if (mel != null)
					{
						VerbalizeElement(mel, snippetsDictionary, showNegative, myStringWriter, ref firstCallPending);
					}
				}
				if (!firstCallPending)
				{
					// Write footer
					myStringWriter.Write(snippets.GetSnippet(CoreVerbalizationTextSnippetType.VerbalizerDocumentFooter));
				}
				else
				{
					// Nothing happened, put in text for nothing happened
				}
				WebBrowser browser = myWebBrowser;
				if (browser != null)
				{
					browser.DocumentText = myStringWriter.ToString();
				}
			}
		}
		/// <summary>
		/// Determine the indentation level for verbalizing a ModelElement, and fire
		/// the delegate for verbalization
		/// </summary>
		/// <param name="element">The element to verbalize</param>
		/// <param name="snippetsDictionary">The default or loaded verbalization sets. Passed through all verbalization calls.</param>
		/// <param name="isNegative">Use the negative form of the reading</param>
		/// <param name="writer">The TextWriter for verbalization output</param>
		/// <param name="firstCallPending"></param>
		private static void VerbalizeElement(ModelElement element, IDictionary<Type, IVerbalizationSets> snippetsDictionary, bool isNegative, TextWriter writer, ref bool firstCallPending)
		{
			int lastLevel = 0;
			bool firstWrite = true;
			bool localFirstCallPending = firstCallPending;
			VerbalizationSets<CoreVerbalizationTextSnippetType> snippets = (VerbalizationSets<CoreVerbalizationTextSnippetType>)snippetsDictionary[typeof(CoreVerbalizationTextSnippetType)];
			VerbalizeElement(
				element,
				snippetsDictionary,
				delegate(IVerbalize verbalizer, int indentationLevel)
				{
					bool openedErrorReport = false;
					bool retVal = verbalizer.GetVerbalization(
						writer,
						snippetsDictionary,
						delegate(VerbalizationContent content)
						{
							if (content == VerbalizationContent.ErrorReport)
							{
								// spit opening tag for text denoting an error
								openedErrorReport = true;
								writer.Write(snippets.GetSnippet(CoreVerbalizationTextSnippetType.VerbalizerOpenError));
							}

							// Prepare for verbalization on this element. Everything
							// is delayed to this point in case the verbalization implementation
							// does not callback to the text writer.
							if (firstWrite)
							{
								if (localFirstCallPending)
								{
									localFirstCallPending = false;
									// Write the HTML header to the buffer
									writer.Write(string.Format(CultureInfo.InvariantCulture, snippets.GetSnippet(CoreVerbalizationTextSnippetType.VerbalizerDocumentHeader), GetDocumentHeaderReplacementFields(element, snippets)));
								}

								// write open tag for new verbalization
								writer.Write(snippets.GetSnippet(CoreVerbalizationTextSnippetType.VerbalizerOpenVerbalization));

								firstWrite = false;
							}
							else
							{
								writer.WriteLine();
							}


							// Write indentation tags as needed
							if (indentationLevel > lastLevel)
							{
								do
								{
									writer.Write(snippets.GetSnippet(CoreVerbalizationTextSnippetType.VerbalizerIncreaseIndent));
									++lastLevel;
								} while (lastLevel != indentationLevel);
							}
							else if (lastLevel > indentationLevel)
							{
								do
								{
									writer.Write(snippets.GetSnippet(CoreVerbalizationTextSnippetType.VerbalizerDecreaseIndent));
									--lastLevel;
								} while (lastLevel != indentationLevel);
							}
						},
						isNegative);
					if (openedErrorReport)
					{
						// Close error report tag
						writer.Write(snippets.GetSnippet(CoreVerbalizationTextSnippetType.VerbalizerCloseError));
					}
					return retVal;
				},
				0);
			while (lastLevel > 0)
			{
				writer.Write(snippets.GetSnippet(CoreVerbalizationTextSnippetType.VerbalizerDecreaseIndent));
				--lastLevel;
			}
			// close the opening tag for the new verbalization
			if (!firstWrite)
			{
				writer.Write(snippets.GetSnippet(CoreVerbalizationTextSnippetType.VerbalizerCloseVerbalization));
			}
			firstCallPending = localFirstCallPending;
		}
		/// <summary>
		/// Verbalize the passed in element and all its children
		/// </summary>
		private static void VerbalizeElement(ModelElement element, IDictionary<Type, IVerbalizationSets> snippetsDictionary, VerbalizationHandler callback, int indentLevel)
		{
			IVerbalize parentVerbalize = element as IVerbalize;
			if (parentVerbalize == null && indentLevel == 0)
			{
				IRedirectVerbalization surrogateRedirect = element as IRedirectVerbalization;
				if (surrogateRedirect != null)
				{
					parentVerbalize = surrogateRedirect.SurrogateVerbalizer;
					if (parentVerbalize != null)
					{
						element = parentVerbalize as ModelElement;
					}
				}
			}
			bool parentVerbalizeOK = (parentVerbalize != null) ? callback(parentVerbalize, indentLevel) : false;
			bool verbalizeChildren = parentVerbalizeOK ? (element != null) : (element is IVerbalizeChildren);
			if (verbalizeChildren)
			{
				if (parentVerbalizeOK)
				{
					++indentLevel;
				}
				MetaClassInfo currentMetaClass = element.MetaClass;
				while (currentMetaClass != null)
				{
					IList aggregateList = currentMetaClass.AggregatedRoles;
					int aggregateCount = aggregateList.Count;
					for (int i = 0; i < aggregateCount; ++i)
					{
						MetaRoleInfo roleInfo = (MetaRoleInfo)aggregateList[i];
						IList children = element.GetCounterpartRolePlayers(roleInfo.OppositeMetaRole, roleInfo, false);
						int childCount = children.Count;
						for (int j = 0; j < childCount; ++j)
						{
							VerbalizeElement((ModelElement)children[j], snippetsDictionary, callback, indentLevel);
						}
					}
					currentMetaClass = currentMetaClass.BaseMetaClass;
				}
				// TODO: Custom child verbalization goes here. Need BeforeNaturalChildren/AfterNaturalChildren/SkipNaturalChildren setting
				// on IVerbalizeCustomChildren
			}
		}
		#endregion // Verbalization Implementation
		#region ORMToolWindow Implementation
		/// <summary>
		/// Calls the base selection changed functionality first, and then calls UpdateVerbalization().
		/// </summary>
		protected override void MonitorSelectionChanged(object sender, MonitorSelectionEventArgs e)
		{
			base.MonitorSelectionChanged(sender, e);
			UpdateVerbalization();
		}
		/// <summary>
		/// Gets the title that will be displayed on the tool window.
		/// </summary>
		public override string WindowTitle
		{
			get
			{
				return ResourceStrings.ModelVerbalizationWindowTitle;
			}
		}
		/// <summary>
		/// Wires event handlers to the store.
		/// </summary>
		protected override void AttachEventHandlers(Store store)
		{
			if (store != null && !store.Disposed)
			{
				store.EventManagerDirectory.ElementEventsEnded.Add(new ElementEventsEndedEventHandler(ModelStateChangedEvent));
			}
		}
		/// <summary>
		/// Unwires event handlers from the store.
		/// </summary>
		protected override void DetachEventHandlers(Store store)
		{
			if (store != null && !store.Disposed)
			{
				store.EventManagerDirectory.ElementEventsEnded.Remove(new ElementEventsEndedEventHandler(ModelStateChangedEvent));
			}
		}
		#endregion // ORMToolWindow Implementation
	}
}
