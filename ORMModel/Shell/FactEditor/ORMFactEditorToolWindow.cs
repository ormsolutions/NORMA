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

#region Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;

using Neumont.Tools.Modeling;

using IServiceProvider = System.IServiceProvider;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ObjectModel.Design;
using System.Collections;
using System.Text.RegularExpressions;
using System.Globalization;
#endregion

namespace Neumont.Tools.ORM.Shell.FactEditor
{
	/// <summary>
	/// The toolwindow responsible for editing facts.
	/// </summary>
	[CLSCompliant(false)]
	public class ORMFactEditorToolWindow : ORMToolWindow, IVsWindowPane
	{
		#region Private Members
		private IVsTextLines m_TextLines;
		private IVsWindowPane m_TextView;
		private NewFactSource m_Source;
		private IMenuCommandService m_CommandService;
		private IWin32Window m_Win32Window;
		private IntPtr m_WindowHandle;
		private Reading m_readingToEdit;

		private FactType m_SelectedFactType;
		private String m_FactEditorText;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="ORMFactEditorToolWindow"/> class.
		/// </summary>
		public ORMFactEditorToolWindow(System.IServiceProvider provider)
			: base(provider)
		{
			if (null == provider)
			{
				throw new ArgumentNullException("provider");
			}


			// Create the text buffer.
			m_TextLines = CreateInstance<VsTextBufferClass, IVsTextLines>();

			// The text buffer must be sited with the global service provider.
			((IObjectWithSite)m_TextLines).SetSite(this.GetService(typeof(IOleServiceProvider)));

			// Set the GUID of the language service that will handle this text buffer
			Guid languageGuid = typeof(NewFactLanguageService).GUID;
			ErrorHandler.ThrowOnFailure(m_TextLines.SetLanguageServiceID(ref languageGuid));
		}
		#endregion

		#region Select Changed Events
		/// <summary>
		/// Handle selection changes
		/// </summary>
		protected override void OnORMSelectionContainerChanged()
		{
			IORMSelectionContainer selectionContainer = CurrentORMSelectionContainer;
			if (selectionContainer != null)
			{
				ICollection selectedObjects = selectionContainer.GetSelectedComponents();
				FactType currentFactType = null;
				if (selectedObjects != null)
				{
					foreach (object element in selectedObjects)
					{
						FactType testFactType = ORMEditorUtility.ResolveContextFactType(element);
						// Handle selection of multiple elements as long as
						// they all resolve to the same fact
						if (currentFactType == null)
						{
							currentFactType = testFactType;
						}
						else if (testFactType != currentFactType)
						{
							currentFactType = null;
							break;
						}
					}
				}
				m_SelectedFactType = currentFactType;
				if (currentFactType != null)
				{
					//if the fact is selected off of the Model Browser window
					if (selectionContainer != null && selectionContainer.GetType() == typeof(Neumont.Tools.ORM.Shell.ORMModelBrowserToolWindow))
					{
						m_FactEditorText = GetPrimaryReadingText();
					}
					else
					{
						m_FactEditorText = GetDisplayReadingText();
					}
				}
				else
				{
					m_FactEditorText = String.Empty;
				}
			}
			else
			{
				m_FactEditorText = String.Empty;
			}
			m_Source.SetText(m_FactEditorText);
		}
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

		private string GetDisplayReadingText()
		{
			FactType factType = m_SelectedFactType;
			string sourceText = String.Empty;
			ReadingOrder readingOrder;
			LinkedElementCollection<ReadingOrder> orders = factType.ReadingOrderCollection;
			int orderCount = orders.Count;
			if (orderCount == 0)
			{
				m_readingToEdit = null;
				return String.Empty;
			}
			readingOrder = factType.FindMatchingReadingOrder(factType.RoleCollection);
			if (readingOrder == null && orderCount != 0)
			{
				readingOrder = orders[0];
			}
			m_readingToEdit = readingOrder.PrimaryReading;
			if (readingOrder != null)
			{
				//if it is a binary with 2 readings
				if (readingOrder.RoleCollection.Count == 2 && orderCount == 2)
				{
					if (orders[0] == readingOrder)
					{
						return GetBinaryReadingText(orders[0].ReadingCollection[0], orders[1].ReadingCollection[0]);
					}
					else
					{
						return GetBinaryReadingText(orders[1].ReadingCollection[0], orders[0].ReadingCollection[0]);
					}
				}
				sourceText = ORMFactEditorToolWindow.FullReading(readingOrder.PrimaryReading, readingOrder, sourceText);
			}
			else
			{
				return GetPrimaryReadingText();
			}
			return sourceText;
		}

		/// <summary>
		/// Set the reading being edited
		/// </summary>
		public Reading ReadingToEdit
		{
			get { return m_readingToEdit; }
			set { m_readingToEdit = value; }
		}

		private string GetPrimaryReadingText()
		{
			FactType factType = m_SelectedFactType;
			LinkedElementCollection<ReadingOrder> readingOrders = factType.ReadingOrderCollection;
			int orderCount = readingOrders.Count;
			if (orderCount == 0)
			{
				m_readingToEdit = null;
				return string.Empty;
			}
			m_readingToEdit = readingOrders[0].PrimaryReading;
			string sourceText = m_readingToEdit.Text;

			if (factType.RoleCollection.Count == 2 && orderCount == 2)
			{
				sourceText = GetBinaryReadingText(readingOrders[0].ReadingCollection[0], readingOrders[1].ReadingCollection[0]);
			}
			else
			{
				sourceText = regCountPlaces.Replace(sourceText, ReadingMatch(readingOrders[0].RoleCollection, readingOrders[0].RoleCollection.Count));
			}
			return sourceText;
		}

		private string GetBinaryReadingText(Reading forward, Reading reverse)
		{
			String[] forwardReading = forward.Text.Split('}');
			String[] reverseReading = reverse.Text.Split('}');
			string sourceText = "{0} " + forwardReading[1].Remove(forwardReading[1].Length - 2).Trim() + "/" + reverseReading[1].Remove(reverseReading[1].Length - 2).Trim() + " {1}";
			sourceText = regCountPlaces.Replace(sourceText, ReadingMatch(forward.ReadingOrder.RoleCollection, forward.ReadingOrder.RoleCollection.Count));
			return sourceText;
		}
		#endregion

		#region Implemented Abstract Properties
		/// <summary>
		/// Gets the window associated with this window pane.
		/// </summary>
		public override IWin32Window Window
		{
			get
			{
				return this.m_Win32Window ?? (this.m_Win32Window = new FactEditorWin32Window(this));
			}
		}

		/// <summary>
		/// Gets the string that should be displayed in the title bar of the tool window.
		/// </summary>
		public override String WindowTitle
		{
			get
			{
				return "New Fact Editor";
			}
		}

		/// <summary>
		/// See <see cref="ToolWindow.BitmapIndex"/>.
		/// </summary>
		protected override int BitmapIndex
		{
			get { return 3; }
		}

		/// <summary>
		/// See <see cref="ToolWindow.BitmapResource"/>.
		/// </summary>
		protected override int BitmapResource
		{
			get { return 125; }
		}

		/// <summary>
		/// Attaches custom <see cref="EventHandler{TEventArgs}"/>s to the <see cref="Store"/>.  This method must be overridden.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="eventManager">The <see cref="ModelingEventManager"/> used to manage the <see cref="EventHandler{TEventArgs}"/>s.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		protected override void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{

		}
		#endregion

		#region Overriden Methods
		/// <summary>
		/// Return the service of the given type.
		/// This override is needed to be able to use a different command service from the one
		/// implemented in the base class.
		/// </summary>
		protected override object GetService(Type serviceType)
		{
			if ((typeof(IOleCommandTarget) == serviceType) ||
				(typeof(System.ComponentModel.Design.IMenuCommandService) == serviceType))
			{
				if (null != m_CommandService)
				{
					return m_CommandService;
				}
			}
			return base.GetService(serviceType);
		}

		/// <summary>
		/// Constructs a new ORM tool window, wires selection and document changed events,
		/// and initializes the CurrentModelElementSelectionContainer to the current DocView.
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			// Register this object as command filter for the text view so that it will
			// be possible to intercept some command.
			IOleCommandTarget originalFilter;
			ErrorHandler.ThrowOnFailure(((IVsTextView)TextView).AddCommandFilter(this, out originalFilter));
			// Create a command service that will use the previous command target
			// as parent target and will route to it the commands that it can not handle.
			if (null == originalFilter)
			{
				m_CommandService = new MyMenuService(this);
			}
			else
			{
				m_CommandService = new MyMenuService(this, originalFilter);
			}

			// Add the command handler for CTRL-RETURN.
			CommandID ctrlReturnCommandID = new CommandID(
								typeof(VSConstants.VSStd2KCmdID).GUID,
								(Int32)VSConstants.VSStd2KCmdID.OPENLINEABOVE);
			OleMenuCommand ctrlReturnCommand = new OleMenuCommand(new EventHandler(OnReturn), ctrlReturnCommandID);
			ctrlReturnCommand.BeforeQueryStatus += new EventHandler(UnsupportedOnCompletion);
			m_CommandService.AddCommand(ctrlReturnCommand);

			// override any action for the RETURN key
			CommandID returnCommandID = new CommandID(
								typeof(VSConstants.VSStd2KCmdID).GUID,
								(Int32)VSConstants.VSStd2KCmdID.RETURN);
			OleMenuCommand returnCommand = new OleMenuCommand(new EventHandler(OnNoAction), returnCommandID);
			m_CommandService.AddCommand(returnCommand);

			// override any action for the SHIFT-RETURN key
			CommandID shiftReturnCommandID = new CommandID(
								typeof(VSConstants.VSStd2KCmdID).GUID,
								(Int32)VSConstants.VSStd2KCmdID.OPENLINEBELOW);
			OleMenuCommand shiftReturnCommand = new OleMenuCommand(new EventHandler(OnNoAction), shiftReturnCommandID);
			m_CommandService.AddCommand(shiftReturnCommand);

			// Now we set the key binding for this frame to the same value as the text editor
			// so that there will be the same mapping for the commands.
			Guid commandUiGuid = VSConstants.GUID_TextEditorFactory;
			Frame.SetGuidProperty((int)__VSFPROPID.VSFPROPID_InheritKeyBindings, ref commandUiGuid);
		}
		#endregion

		#region Command Event Handlers
		/// <summary>
		/// Empty command handler used to overwrite some standard command with an empty action.
		/// </summary>
		private void OnNoAction(Object sender, EventArgs e)
		{
			// Do Nothing.
		}

		private void OnReturn(Object sender, EventArgs e)
		{
			NewFactLanguageService service = (NewFactLanguageService)((IServiceProvider)ORMDesignerPackage.Singleton).GetService(typeof(NewFactLanguageService));

			IVsTextView textView = (IVsTextView)this.TextView;

			// get the current line where the cursor is
			Int32 line = 0;
			Int32 column;
			ErrorHandler.ThrowOnFailure(textView.GetCaretPos(out line, out column));

			// get the text lines
			IVsTextLines textLines;
			ErrorHandler.ThrowOnFailure(textView.GetBuffer(out textLines));

			// get the number of chars on the line
			Int32 lineLength;
			ErrorHandler.ThrowOnFailure(textLines.GetLengthOfLine(line, out lineLength));

			// see if current line contains "draw" (starting at the beggining of the line)
			String text;
			ErrorHandler.ThrowOnFailure(textLines.GetLineText(
				line,          // starting line
				0,              // starting character index within the line (must be <= length of line)
				line,          // ending line
				lineLength,    // ending character index within the line (must be <= length of line)
				out text)   // line text, if any
			);

			//Trace.WriteLine("Number of Sources: " + ((ArrayList)(service.GetSources())).Count);
			IFactParser parser = new FactParser();
			ParsedFact fact = parser.ParseLine(text);
			FactType selectedFact = this.SelectedFactType;
			if (selectedFact != null && fact.ReverseReadingText != null)
				FactParser.SetReadings(selectedFact, fact);
			fact.ReadingToEdit = this.ReadingToEdit;
			selectedFact = FactSaver.AddFact(
				base.CurrentDocument,
				base.CurrentDocumentView as ORMDesignerDocView,
				fact, selectedFact);

			ReadingOrder readingOrder = selectedFact.FindMatchingReadingOrder(selectedFact.RoleCollection);
			this.SelectedFactType = null;
			if (readingOrder != null)
			{
				string headText = ORMFactEditorToolWindow.GetHeadText(readingOrder.PrimaryReading.Text);
				string headReading = ORMFactEditorToolWindow.HeadReading(headText, readingOrder);
				this.MySource.SetText(headReading);
			}
			else this.MySource.SetText(String.Empty);
		}

		/// <summary>
		/// Set the status of the command to Unsupported when the completion window is visible.
		/// </summary>
		private void UnsupportedOnCompletion(Object sender, EventArgs args)
		{
			MenuCommand command = sender as MenuCommand;
			if (null == command)
			{
				return;
			}
			command.Supported = (null == m_Source) || (!m_Source.IsCompletorActive);
		}
		#endregion

		#region Private Methods
		private TInterface CreateInstance<TClass, TInterface>()
			where TInterface : class
			where TClass : class
		{
			Guid classGuid = typeof(TClass).GUID;
			Guid interfaceGuid = typeof(TInterface).GUID;
			Package package = this.GetService(typeof(Package)) as Package ?? Package.GetGlobalService(typeof(Package)) as Package;

			if (package != null)
			{
				return package.CreateInstance(ref classGuid, ref interfaceGuid, typeof(TClass)) as TInterface;
			}

			return null;
		}
		#endregion

		#region Private Properties
		/// <summary>
		/// Gets the text view (as an IVsWindowPane) that is used by the fact editor.
		/// </summary>
		private IVsWindowPane TextView
		{
			get
			{
				// Avoid to create the object more than once.
				if (null == m_TextView)
				{

					// Create the text view object.
					IVsTextView textViewLocal = CreateInstance<VsTextViewClass, IVsTextView>();
					//this.textView = textViewLocal;

					// Now we have to site the text view using the global service provider.
					// Note that this service provider will be used only before the text view will be
					// used inside CreateToolWindow, because inside this call it will be sited again
					// using a different provider.
					((IObjectWithSite)textViewLocal).SetSite(this.GetService(typeof(IOleServiceProvider)));

					// Now it is possible to initalize the view using the buffer.
					ErrorHandler.ThrowOnFailure(
						textViewLocal.Initialize(m_TextLines, IntPtr.Zero, (uint)TextViewInitFlags.VIF_VSCROLL | (uint)TextViewInitFlags.VIF_HSCROLL, null));

					// Get the language service.
					NewFactLanguageService language = this.GetService(typeof(NewFactLanguageService)) as NewFactLanguageService;
					if (null != language)
					{
						// In order to enable intellisense we have to create a Source object and create
						// a CodeWindowManager on top of it; this way the window manager will install a
						// special filter to the text view that will intercept some key stroke, call the
						// language service to get the list of the methods and show the completion window.
						// In general you don't need to do all this work because for code windows it is
						// done by the framework, but in this case this window is not a code window, so
						// we have to do it manually.

						// Create a new source object.
						m_Source = language.CreateSource(m_TextLines) as NewFactSource;

						// Now we can create a CodeWindowManager using the source.
						CodeWindowManager windowManager = language.CreateCodeWindowManager(null, m_Source);
						// Add the window manager to the language service.
						language.AddCodeWindowManager(windowManager);
						// Add the text view to the window manager.
						windowManager.OnNewView(textViewLocal);
					}

					this.m_TextView = (IVsWindowPane)textViewLocal;
				}
				return this.m_TextView;
			}
		}

		private FactType SelectedFactType
		{
			get
			{
				return m_SelectedFactType;
			}
			set
			{
				m_SelectedFactType = value;
			}
		}

		private NewFactSource MySource { get { return m_Source; } set { m_Source = value; } }
		#endregion

		#region IVsWindowPane Members
		Int32 IVsWindowPane.ClosePane()
		{
			int hr = VSConstants.S_OK;
			if (null != m_TextView)
			{
				// Call the implementation provided by the text view.
				hr = m_TextView.ClosePane();
			}
			Dispose(true);
			return hr;
		}

		Int32 IVsWindowPane.CreatePaneWindow(IntPtr hwndParent, int x, int y, int cx, int cy, out IntPtr hwnd)
		{
			Int32 hr = ((IVsWindowPane)TextView).CreatePaneWindow(hwndParent, x, y, cx, cy, out hwnd);

			this.m_WindowHandle = hwnd;

			return hr;
		}

		Int32 IVsWindowPane.GetDefaultSize(SIZE[] pSize)
		{
			return TextView.GetDefaultSize(pSize);
		}

		Int32 IVsWindowPane.LoadViewState(IStream pStream)
		{
			return TextView.LoadViewState(pStream);
		}

		Int32 IVsWindowPane.SaveViewState(IStream pStream)
		{
			return TextView.SaveViewState(pStream);
		}

		Int32 IVsWindowPane.TranslateAccelerator(MSG[] lpmsg)
		{
			return TextView.TranslateAccelerator(lpmsg);
		}

		#endregion

		#region Helper Methods

		private static readonly Regex regCountPlaces = new Regex(@"{(?<placeHolderNr>\d+)}", RegexOptions.Compiled);

		private static string FullReading(Reading reading, ReadingOrder readingOrder, string fullReading)
		{
			LinkedElementCollection<RoleBase> roles = readingOrder.RoleCollection;
			int roleCount = roles.Count;
			fullReading = regCountPlaces.Replace(
				reading.Text,
				ReadingMatch(roles, roleCount));
			return fullReading;
		}

		private static MatchEvaluator ReadingMatch(LinkedElementCollection<RoleBase> roles, int roleCount)
		{
			return delegate(Match m)
							{
								string retval = null;
								string matchText = m.Value;
								int rolePosition = int.Parse(matchText.Substring(1, matchText.Length - 2), CultureInfo.InvariantCulture);
								if (roleCount > rolePosition)
								{
									ObjectType player = roles[rolePosition].Role.RolePlayer;
									if (player != null)
									{
										string refModeString = player.ReferenceModeString;
										if ((refModeString == null || refModeString.Length == 0) && !player.IsValueType)
										{
											retval = string.Format(CultureInfo.InvariantCulture, "{0}", player.Name);
										}
										else
										{
											retval = string.Format(CultureInfo.InvariantCulture, "{0}({1})", player.Name, refModeString);
										}
										if (!string.IsNullOrEmpty(retval) && Char.IsLower(retval[0]))
										{
											retval = "[" + retval + "]";
										}
									}
									else
									{
										retval = ResourceStrings.ModelReadingEditorMissingRolePlayerText;
									}
								}
								else
								{
									retval = ResourceStrings.ModelReadingEditorMissingRolePlayerText;
								}
								return retval;
							};
		}

		private static string HeadReading(string headText, ReadingOrder readingOrder)
		{
			LinkedElementCollection<RoleBase> roles = readingOrder.RoleCollection;
			return regCountPlaces.Replace(
				headText,
				ReadingMatch(roles, 1));
		}

		private static string GetHeadText(string reading)
		{
			char[] delim = new char[1];
			delim[0] = '}';
			string[] splitReading = reading.Split(delim);
			return splitReading[0] + "} ";
		}
		#endregion

		private sealed class FactEditorWin32Window : IWin32Window
		{
			#region Private Members
			private readonly ORMFactEditorToolWindow toolWindow;
			#endregion

			#region Constructor
			public FactEditorWin32Window(ORMFactEditorToolWindow toolWindow)
			{
				this.toolWindow = toolWindow;
			}
			#endregion

			#region IWin32Window Members
			public IntPtr Handle
			{
				get
				{
					return this.toolWindow.m_WindowHandle;
				}
			}
			#endregion
		}

		private sealed class MyMenuService : IMenuCommandService, IOleCommandTarget
		{
			#region Private Members
			private IMenuCommandService m_CommandService;
			#endregion

			#region Constructors
			/// <summary>
			/// Initializes a new instance of the <see cref="MyMenuService"/> class.
			/// </summary>
			/// <param name="proivder">The proivder.</param>
			public MyMenuService(IServiceProvider proivder)
				: this(proivder, null)
			{ }

			/// <summary>
			/// Initializes a new instance of the <see cref="MyMenuService"/> class.
			/// </summary>
			/// <param name="provider">The provider.</param>
			/// <param name="target">The target.</param>
			public MyMenuService(IServiceProvider provider, IOleCommandTarget target)
			{
				m_CommandService = new OleMenuCommandService(provider, target);
			}
			#endregion

			#region IOleCommandTarget Members

			int IOleCommandTarget.Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
			{
				return ((IOleCommandTarget)m_CommandService).Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
			}

			int IOleCommandTarget.QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
			{
				return ((IOleCommandTarget)m_CommandService).QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
			}
			#endregion

			#region IMenuCommandService Members

			void IMenuCommandService.AddCommand(MenuCommand command)
			{
				m_CommandService.AddCommand(command);
			}

			void IMenuCommandService.AddVerb(DesignerVerb verb)
			{
				m_CommandService.AddVerb(verb);
			}

			MenuCommand IMenuCommandService.FindCommand(CommandID commandID)
			{
				return m_CommandService.FindCommand(commandID);
			}

			bool IMenuCommandService.GlobalInvoke(CommandID commandID)
			{
				return m_CommandService.GlobalInvoke(commandID);
			}

			void IMenuCommandService.RemoveCommand(MenuCommand command)
			{
				m_CommandService.RemoveCommand(command);
			}

			void IMenuCommandService.RemoveVerb(DesignerVerb verb)
			{
				m_CommandService.RemoveVerb(verb);
			}

			void IMenuCommandService.ShowContextMenu(CommandID menuID, int x, int y)
			{
				m_CommandService.ShowContextMenu(menuID, x, y);
			}

			DesignerVerbCollection IMenuCommandService.Verbs
			{
				get { return m_CommandService.Verbs; }
			}

			#endregion
		}
	}
}