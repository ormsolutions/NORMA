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
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.Modeling;

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// Local settings for the verbalization tool window. These are not stored
	/// with the window because they are required to initialize the window itself.
	/// </summary>
	public class ORMVerbalizationToolWindowSettings
	{
		#region Member variables
		private bool myShowNegativeVerbalizations;
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
					ORMDesignerPackage.VerbalizationWindow.WindowSettingsChanged();
				}
			}
		}
		#endregion // Accessor Properties
	}
	/// <summary>
	/// ToolWindow for hosting a web browser for verbalizations
	/// </summary>
	[Guid("C9AA5E71-9193-46C9-971A-CB6365ACA338")]
	[CLSCompliant(false)]
	public class ORMVerbalizationToolWindow : ORMToolWindow
	{
		#region Member variables
		private WebBrowser myWebBrowser;
		private StringWriter myStringWriter;
		private static string[] myDocumentHeaderReplacementFields;
		private Dictionary<IVerbalize, IVerbalize> myAlreadyVerbalized;
		/// <summary>
		/// An enum to determine callback handling during verbalization
		/// </summary>
		private enum VerbalizationResult
		{
			/// <summary>
			/// The element was successfully verbalized
			/// </summary>
			Verbalized,
			/// <summary>
			/// The element was previously verbalized
			/// </summary>
			AlreadyVerbalized,
			/// <summary>
			/// The element was not verbalized
			/// </summary>
			NotVerbalized,
		}
		/// <summary>
		/// Callback for child verbalizations
		/// </summary>
		private delegate VerbalizationResult VerbalizationHandler(IVerbalize verbalizer, int indentationLevel);
		#endregion // Member variables
		#region Construction
		/// <summary>
		/// Construct a verbalization window with a monitor selection service
		/// </summary>
		/// <param name="serviceProvider">Service provider</param>
		public ORMVerbalizationToolWindow(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
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
			myStringWriter = new StringWriter(builder, CultureInfo.CurrentCulture);
			UpdateVerbalization();
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
		public void GlobalSettingsChanged()
		{
			myDocumentHeaderReplacementFields = null;
			UpdateVerbalization();
		}
		/// <summary>
		/// Called when changes are made to window options (Negative/Positive etc)
		/// </summary>
		public void WindowSettingsChanged()
		{
			UpdateVerbalization();
		}
		#endregion // Selection monitor event handlers and helpers
		#region ToolWindow Overrides
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
		/// See <see cref="ToolWindow.BitmapIndex"/>.
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
					browser.DocumentText = (writer != null) ? writer.ToString() : string.Empty;
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
					myWebBrowser.Dispose();
					myWebBrowser = null;
				}
				if (myStringWriter != null)
				{
					myStringWriter.Dispose();
					myStringWriter = null;
				}
			}
		}
		#endregion // ToolWindow Overrides
		#region Verbalization Implementation
		/// <summary>
		/// Get the 8 document header replacement fields from the current font and color settings
		/// </summary>
		private static string[] GetDocumentHeaderReplacementFields(ModelElement element, IVerbalizationSets<CoreVerbalizationSnippetType> snippets)
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
				string boldWeight = snippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerFontWeightBold);
				string normalWeight = snippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerFontWeightNormal);
				retVal = new string[] { "Tahoma", "8", "darkgreen", normalWeight, "purple", normalWeight, "mediumblue", boldWeight, "brown", normalWeight, "darkgray", normalWeight };
				using (Font font = colorService.GetFont(ORMDesignerColorCategory.Verbalizer))
				{
					retVal[0] = font.FontFamily.Name;
					retVal[1] = (font.Size * 72f).ToString(CultureInfo.InvariantCulture);
					retVal[2] = ColorTranslator.ToHtml(colorService.GetForeColor(ORMDesignerColor.VerbalizerPredicateText));
					retVal[3] = (0 != (colorService.GetFontStyle(ORMDesignerColor.VerbalizerPredicateText) & FontStyle.Bold)) ? boldWeight : normalWeight;
					retVal[4] = ColorTranslator.ToHtml(colorService.GetForeColor(ORMDesignerColor.VerbalizerObjectName));
					retVal[5] = (0 != (colorService.GetFontStyle(ORMDesignerColor.VerbalizerObjectName) & FontStyle.Bold)) ? boldWeight : normalWeight;
					retVal[6] = ColorTranslator.ToHtml(colorService.GetForeColor(ORMDesignerColor.VerbalizerFormalItem));
					retVal[7] = (0 != (colorService.GetFontStyle(ORMDesignerColor.VerbalizerFormalItem) & FontStyle.Bold)) ? boldWeight : normalWeight;
					retVal[8] = ColorTranslator.ToHtml(colorService.GetForeColor(ORMDesignerColor.VerbalizerNotesItem));
					retVal[9] = (0 != (colorService.GetFontStyle(ORMDesignerColor.VerbalizerNotesItem) & FontStyle.Bold)) ? boldWeight : normalWeight;
					retVal[10] = ColorTranslator.ToHtml(colorService.GetForeColor(ORMDesignerColor.VerbalizerRefMode));
					retVal[11] = (0 != (colorService.GetFontStyle(ORMDesignerColor.VerbalizerRefMode) & FontStyle.Bold)) ? boldWeight : normalWeight;
				}
				myDocumentHeaderReplacementFields = retVal;
			}
			return myDocumentHeaderReplacementFields;
		}
		private void UpdateVerbalization()
		{
			if (CurrentORMSelectionContainer == null)
			{
				return;
			}
			if (myStringWriter != null)
			{
				myStringWriter.GetStringBuilder().Length = 0;

				ICollection selectedObjects = base.GetSelectedComponents();
				IDictionary<Type, IVerbalizationSets> snippetsDictionary = null;
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = null;
				bool showNegative = ORMDesignerPackage.VerbalizationWindowSettings.ShowNegativeVerbalizations;
				bool firstCallPending = true;
				Dictionary<IVerbalize, IVerbalize> verbalized = myAlreadyVerbalized;
				if (verbalized == null)
				{
					verbalized = new Dictionary<IVerbalize, IVerbalize>();
					myAlreadyVerbalized = verbalized;
				}
				else
				{
					verbalized.Clear();
				}
				foreach (ModelElement melIter in selectedObjects)
				{
					ModelElement mel = melIter;
					PresentationElement pel = mel as PresentationElement;
					if (pel != null)
					{
						mel = pel.ModelElement;
					}
					if (mel != null && !mel.IsDeleted)
					{
						if (snippetsDictionary == null)
						{
							snippetsDictionary = (mel.Store as IORMToolServices).VerbalizationSnippetsDictionary;
							snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
							myStringWriter.NewLine = snippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerNewLine);
						}
						VerbalizeElement(mel, snippetsDictionary, verbalized, showNegative, myStringWriter, ref firstCallPending);
					}
				}
				if (!firstCallPending)
				{
					// Write footer
					myStringWriter.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerDocumentFooter));
					// Clear cache
					verbalized.Clear();
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
		/// <param name="alreadyVerbalized">A dictionary of top-level (indentationLevel == 0) elements that have already been verbalized.</param>
		/// <param name="isNegative">Use the negative form of the reading</param>
		/// <param name="writer">The TextWriter for verbalization output</param>
		/// <param name="firstCallPending"></param>
		private static void VerbalizeElement(ModelElement element, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IDictionary<IVerbalize, IVerbalize> alreadyVerbalized, bool isNegative, TextWriter writer, ref bool firstCallPending)
		{
			int lastLevel = 0;
			bool firstWrite = true;
			bool localFirstCallPending = firstCallPending;
			IVerbalizationSets<CoreVerbalizationSnippetType> snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
			VerbalizeElement(
				element,
				snippetsDictionary,
				null,
				delegate(IVerbalize verbalizer, int indentationLevel)
				{
					if (indentationLevel == 0)
					{
						if (alreadyVerbalized.ContainsKey(verbalizer))
						{
							return VerbalizationResult.AlreadyVerbalized;
						}
					}
					bool retVal = verbalizer.GetVerbalization(
						writer,
						snippetsDictionary,
						delegate(VerbalizationContent content)
						{
							// Prepare for verbalization on this element. Everything
							// is delayed to this point in case the verbalization implementation
							// does not callback to the text writer.
							if (firstWrite)
							{
								if (localFirstCallPending)
								{
									localFirstCallPending = false;
									// Write the HTML header to the buffer
									writer.Write(string.Format(CultureInfo.InvariantCulture, snippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerDocumentHeader), GetDocumentHeaderReplacementFields(element, snippets)));
								}

								// write open tag for new verbalization
								writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerOpenVerbalization));
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
									writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerIncreaseIndent));
									++lastLevel;
								} while (lastLevel != indentationLevel);
							}
							else if (lastLevel > indentationLevel)
							{
								do
								{
									writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerDecreaseIndent));
									--lastLevel;
								} while (lastLevel != indentationLevel);
							}
						},
						isNegative);
					if (retVal)
					{
						if (indentationLevel == 0)
						{
							alreadyVerbalized.Add(verbalizer, verbalizer);
						}
						return VerbalizationResult.Verbalized;
					}
					else
					{
						return VerbalizationResult.NotVerbalized;
					}
				},
				isNegative,
				0);
			while (lastLevel > 0)
			{
				writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerDecreaseIndent));
				--lastLevel;
			}
			// close the opening tag for the new verbalization
			if (!firstWrite)
			{
				writer.Write(snippets.GetSnippet(CoreVerbalizationSnippetType.VerbalizerCloseVerbalization));
			}
			firstCallPending = localFirstCallPending;
		}
		/// <summary>
		/// Verbalize the passed in element and all its children
		/// </summary>
		private static void VerbalizeElement(ModelElement element, IDictionary<Type, IVerbalizationSets> snippetsDictionary, IVerbalizeFilterChildren filter, VerbalizationHandler callback, bool isNegative, int indentLevel)
		{
			IVerbalize parentVerbalize = null;
			IRedirectVerbalization surrogateRedirect;
			if (indentLevel == 0 &&
				null != (surrogateRedirect = element as IRedirectVerbalization) &&
				null != (parentVerbalize = surrogateRedirect.SurrogateVerbalizer))
			{
				element = parentVerbalize as ModelElement;
			}
			else
			{
				parentVerbalize = element as IVerbalize;
			}
			bool disposeVerbalizer = false;
			if (filter != null && parentVerbalize != null)
			{
				CustomChildVerbalizer filterResult = filter.FilterChildVerbalizer(parentVerbalize, isNegative);
				parentVerbalize = filterResult.Instance;
				disposeVerbalizer = filterResult.Options;
			}
			try
			{
				VerbalizationResult result = (parentVerbalize != null) ? callback(parentVerbalize, indentLevel) : VerbalizationResult.NotVerbalized;
				if (result == VerbalizationResult.AlreadyVerbalized)
				{
					return;
				}
				bool parentVerbalizeOK = result == VerbalizationResult.Verbalized;
				bool verbalizeChildren = parentVerbalizeOK ? (element != null) : (element is IVerbalizeChildren);
				if (verbalizeChildren)
				{
					if (parentVerbalizeOK)
					{
						++indentLevel;
					}
					filter = parentVerbalize as IVerbalizeFilterChildren;
					ReadOnlyCollection<DomainRoleInfo> aggregatingList = element.GetDomainClass().AllDomainRolesPlayed;
					int aggregatingCount = aggregatingList.Count;
					for (int i = 0; i < aggregatingCount; ++i)
					{
						DomainRoleInfo roleInfo = aggregatingList[i];
						if (roleInfo.IsEmbedding)
						{
							LinkedElementCollection<ModelElement> children = roleInfo.GetLinkedElements(element);
							int childCount = children.Count;
							for (int j = 0; j < childCount; ++j)
							{
								VerbalizeElement(children[j], snippetsDictionary, filter, callback, isNegative, indentLevel);
							}
						}
					}
					// TODO: Need BeforeNaturalChildren/AfterNaturalChildren/SkipNaturalChildren settings for IVerbalizeCustomChildren
					IVerbalizeCustomChildren customChildren = parentVerbalize as IVerbalizeCustomChildren;
					if (customChildren != null)
					{
						foreach (CustomChildVerbalizer customChild in customChildren.GetCustomChildVerbalizations(isNegative))
						{
							IVerbalize childVerbalize = customChild.Instance;
							if (childVerbalize != null)
							{
								try
								{
									callback(childVerbalize, indentLevel);
								}
								finally
								{
									if (customChild.Options)
									{
										IDisposable dispose = childVerbalize as IDisposable;
										if (dispose != null)
										{
											dispose.Dispose();
										}
									}
								}
							}
						}
					}
				}
			}
			finally
			{
				if (disposeVerbalizer)
				{
					IDisposable dispose = parentVerbalize as IDisposable;
					if (dispose != null)
					{
						dispose.Dispose();
					}
				}
			}
		}
		#endregion // Verbalization Implementation
		#region ORMToolWindow Implementation
		/// <summary>
		/// Update verbalization when the selection changes
		/// </summary>
		protected override void OnORMSelectionContainerChanged()
		{
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
		/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> so that the <see cref="ORMVerbalizationToolWindow"/>
		/// contents can be updated to reflect any model changes.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="eventManager">The <see cref="ModelingEventManager"/> used to manage the <see cref="EventHandler{TEventArgs}"/>s.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		protected override void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{
			if (store != null && !store.Disposed)
			{
				eventManager.AddOrRemoveHandler(new EventHandler<ElementEventsEndedEventArgs>(ModelStateChangedEvent), action);
			}
		}
		#endregion // ORMToolWindow Implementation
	}
}
