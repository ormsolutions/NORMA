#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                        *
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
//using System.Drawing;
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
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Shell;
using ORMSolutions.ORMArchitect.Core.ObjectModel.Verbalization;

namespace ORMSolutions.ORMArchitect.Core.Shell
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
		/// <summary>
		/// Store the state of the diagram spy hyperlink toggle
		/// </summary>
		public bool HyperlinkToDiagramSpy
		{
			get
			{
				return OptionsPage.CurrentVerbalizationHyperlinkTarget == HyperlinkTargetWindow.DiagramSpyWindow;
			}
			set
			{
				if (HyperlinkToDiagramSpy != value)
				{
					OptionsPage page = new OptionsPage();
					page.Initialize();
					page.VerbalizationHyperlinkTarget = value ? HyperlinkTargetWindow.DiagramSpyWindow : HyperlinkTargetWindow.DocumentWindow;
					page.ApplyChanges();
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
		private Dictionary<object, object> myLocallyVerbalized;
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
			if (((IORMToolServices)sender).ProcessingVisibleTransactionItemEvents)
			{
				UpdateVerbalization();
			}
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
				return PackageResources.Id.ToolWindowIcons;
			}
		}
		/// <summary>
		/// See <see cref="ToolWindow.BitmapIndex"/>.
		/// </summary>
		protected override int BitmapIndex
		{
			get
			{
				return PackageResources.ToolWindowIconIndex.VerbalizationBrowser;
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
					browser.Navigating += new WebBrowserNavigatingEventHandler(Browser_Navigating);
					browser.Dock = DockStyle.Fill;
					StringWriter writer = myStringWriter;
					browser.DocumentText = (writer != null) ? writer.ToString() : string.Empty;
					browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(Browser_DocumentCompleted);
					// The container magically provides resize support, we don't have
					// to go all the way to a form
					ContainerControl container = new ContainerControl();
					container.Controls.Add(browser);
					Guid commandSetId = typeof(ORMDesignerEditorFactory).GUID;
					Frame.SetGuidProperty((int)__VSFPROPID.VSFPROPID_InheritKeyBindings, ref commandSetId);
				}
				return browser.Parent;
			}
		}
		void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			StringWriter writer = myStringWriter;
			WebBrowser browser;
			if (writer != null &&
				writer.GetStringBuilder().Length != 0 &&
				string.IsNullOrEmpty((browser = myWebBrowser).DocumentTitle))
			{
				// The document is resetting on load after we set the document. This
				// only appears to happen on the initial load, so we don't need to check
				// the condition after we've caught this one time.
				browser.DocumentText = writer.ToString();
				browser.DocumentCompleted -= new WebBrowserDocumentCompletedEventHandler(Browser_DocumentCompleted);
			}
		}
		private void Browser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
		{
			Uri uri = e.Url;
			if (uri.Scheme == "elementid")
			{
				ModelingDocData docData;
				IORMToolServices services;
				ModelElement element;
				if (null != (docData = this.DocData as ModelingDocData) &&
					null != (element = docData.Store.ElementDirectory.FindElement(new Guid(uri.LocalPath))) &&
					null != (services = docData as IORMToolServices))
				{
					services.NavigateTo(element, (!(element is ModelError) && ORMDesignerPackage.VerbalizationWindowSettings.HyperlinkToDiagramSpy) ? NavigateToWindow.DiagramSpy : NavigateToWindow.Document);
				}
				e.Cancel = true;
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
				myDocumentHeaderReplacementFields = retVal = VerbalizationHelper.GetDocumentHeaderReplacementFields(element.Store, snippets);
			}
			return retVal;
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
				IDictionary<string, object> options = null;
				IVerbalizationSets<CoreVerbalizationSnippetType> snippets = null;
				IExtensionVerbalizerService extensionVerbalizer = null;
				VerbalizationCallbackWriter callbackWriter = null;
				bool showNegative = ORMDesignerPackage.VerbalizationWindowSettings.ShowNegativeVerbalizations;
				bool firstCallPending = true;
				Dictionary<IVerbalize, IVerbalize> alreadyVerbalized = myAlreadyVerbalized;
				if (alreadyVerbalized == null)
				{
					alreadyVerbalized = new Dictionary<IVerbalize, IVerbalize>();
					myAlreadyVerbalized = alreadyVerbalized;
				}
				else
				{
					alreadyVerbalized.Clear();
				}
				Dictionary<object, object> locallyVerbalized = myLocallyVerbalized;
				if (locallyVerbalized == null)
				{
					locallyVerbalized = new Dictionary<object, object>();
					myLocallyVerbalized = locallyVerbalized;
				}
				if (selectedObjects != null)
				{
					foreach (object melIter in selectedObjects)
					{
						ModelElement mel = melIter as ModelElement;
						PresentationElement pel = mel as PresentationElement;
						if (pel != null)
						{
							IRedirectVerbalization shapeRedirect = pel as IRedirectVerbalization;
							if (null == (shapeRedirect = pel as IRedirectVerbalization) ||
								null == (mel = shapeRedirect.SurrogateVerbalizer as ModelElement))
							{
								mel = pel.ModelElement;
							}
						}
						if (mel != null && !mel.IsDeleted)
						{
							if (snippetsDictionary == null)
							{
								Store store = Utility.ValidateStore(mel.Store);
								if (store == null)
								{
									break;
								}
								IORMToolServices toolServices = (IORMToolServices)store;
								extensionVerbalizer = toolServices.ExtensionVerbalizerService;
								options = toolServices.VerbalizationOptions;
								snippetsDictionary = toolServices.GetVerbalizationSnippetsDictionary(ORMCoreDomainModel.VerbalizationTargetName);
								snippets = (IVerbalizationSets<CoreVerbalizationSnippetType>)snippetsDictionary[typeof(CoreVerbalizationSnippetType)];
								callbackWriter = new VerbalizationCallbackWriter(snippets, myStringWriter, GetDocumentHeaderReplacementFields(mel, snippets));
							}
							locallyVerbalized.Clear();
							VerbalizationHelper.VerbalizeElement(
								mel,
								snippetsDictionary,
								extensionVerbalizer,
								options,
								ORMCoreDomainModel.VerbalizationTargetName,
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
		#endregion // Verbalization Implementation
		#region ORMToolWindow Implementation
		/// <summary>
		/// Clear a covered window when the document changes and when the selection changes.
		/// </summary>
		protected override CoveredFrameContentActions CoveredFrameContentActions
		{
			get
			{
				return CoveredFrameContentActions.ClearContentsOnSelectionChanged | CoveredFrameContentActions.ClearContentsOnDocumentChanged;
			}
		}
		/// <summary>
		/// Update verbalization when the selection changes
		/// </summary>
		protected override void OnORMSelectionContainerChanged()
		{
			UpdateVerbalization();
		}
		/// <summary>
		/// Update verbalization when the document changes
		/// </summary>
		protected override void OnCurrentDocumentChanged()
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
			if (Utility.ValidateStore(store) != null)
			{
				eventManager.AddOrRemoveHandler(new EventHandler<ElementEventsEndedEventArgs>(ModelStateChangedEvent), action);
			}
		}
		#endregion // ORMToolWindow Implementation
	}
}
