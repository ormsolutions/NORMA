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
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using IOleServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections;
using Neumont.Tools.Modeling.Shell;
using Microsoft.VisualStudio.Modeling.Shell;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ObjectModel.Design;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ShapeModel;
using System.Globalization;
using System.Text;
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Design;
#endregion

namespace Neumont.Tools.ORM.Shell
{
	#region Placeholder editor factory and tool window classes
	/// <summary>
	/// Editor factory class used to enable custom key bindings for the
	/// Fact Editor. This factory is a place holder to enable key bindings.
	/// It is never created.
	/// </summary>
	[Guid("EE99A954-82C4-4098-A2BD-5C75E3D138A7")]
	public static class ORMFactEditorEditorFactory { }
	/// <summary>
	/// Public face of the fact editor tool window. This class exists to
	/// get a guid off of it. It is never created.
	/// </summary>
	[Guid("63B6F84D-DF09-4E65-86EA-6BC1B856837B")]
	public static class FactEditorToolWindow { }
	/// <summary>
	/// Provides a language service for a Managed fact editor.
	/// </summary>
	#endregion // Placeholder editor factory and tool window classes
	[Guid(FactEditorLanguageService.GuidString)]
	[CLSCompliant(false)]
	public sealed partial class FactEditorLanguageService : LanguageService
	{
		#region FactEditorCodeWindowManager class
		/// <summary>
		/// Attach cutom key bindings to a standard <see cref="CodeWindowManager"/> implementation
		/// </summary>
		private class FactEditorCodeWindowManager : CodeWindowManager
		{
			public FactEditorCodeWindowManager(LanguageService service, IVsCodeWindow codeWindow, Source source)
				: base(service, codeWindow, source)
			{
			}
			public override int AddAdornments()
			{
				IOleServiceProvider sp = (IOleServiceProvider)CodeWindow;
				Guid tempGuid = typeof(IVsWindowFrame).GUID;
				IntPtr pFrame = IntPtr.Zero;
				int hr = sp.QueryService(ref tempGuid, ref tempGuid, out pFrame);
				if (hr == VSConstants.S_OK)
				{
					try
					{
						IVsWindowFrame frame = (IVsWindowFrame)Marshal.GetObjectForIUnknown(pFrame);
						tempGuid = typeof(ORMFactEditorEditorFactory).GUID;
						hr = frame.SetGuidProperty((int)__VSFPROPID.VSFPROPID_InheritKeyBindings, ref tempGuid);
					}
					finally
					{
						if (pFrame != IntPtr.Zero)
						{
							Marshal.Release(pFrame);
						}
					}
				}
				return base.AddAdornments();
			}
		}
		#endregion // FactEditorCodeWindowManager class
		#region FactEditorViewFilter class
		/// <summary>
		/// Handle fact editor commands via  <see cref="ViewFilter"/> on the
		/// framework-provided <see cref="IVsTextView"/> implementation
		/// </summary>
		private class FactEditorViewFilter : ViewFilter
		{
			private const uint cmdIdFactEditorCommitLine = 0x292b; // Keep in sync with PkgCmd.vsct

			public FactEditorViewFilter(CodeWindowManager mgr, IVsTextView view)
				: base(mgr, view)
			{
			}
			protected override int QueryCommandStatus(ref Guid guidCmdGroup, uint nCmdId)
			{
				if (nCmdId == cmdIdFactEditorCommitLine && guidCmdGroup == typeof(ORMDesignerDocView.ORMDesignerCommandIds).GUID)
				{
					Source source = Source;
					// Supported=1, Enabled=2, Invisible=0x10
					return (source == null || !source.IsCompletorActive) ? 3 : 2;
				}
				return base.QueryCommandStatus(ref guidCmdGroup, nCmdId);
			}
			public override bool HandlePreExec(ref Guid guidCmdGroup, uint nCmdId, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
			{
				if (nCmdId == cmdIdFactEditorCommitLine && guidCmdGroup == typeof(ORMDesignerDocView.ORMDesignerCommandIds).GUID)
				{
					Source source = this.Source;
					FactEditorLanguageService languageService = (FactEditorLanguageService)source.LanguageService;
					ActivationManager activationContext = languageService.ActiveContext;
					IVsTextView textView = this.TextView;

					// get the current line where the cursor is
					int line = 0;
					int column;
					ErrorHandler.ThrowOnFailure(textView.GetCaretPos(out line, out column));

					// get the text on the selected line
					string text = source.GetLine(line);

					ParsedFactType parsedFactType = ParsedFactType.ParseLine(text);
					if (parsedFactType.RolePlayers.Count != 0)
					{
						// Cache and clear FactType portions of activation context
						// to avoid event interactions as the selected FactType is modified.
						FactType activeFactType = activationContext.CurrentFactType;
						ReadingOrder activeReadingOrder = activationContext.CurrentReadingOrder;
						IList<RoleBase> activeRoleOrder = activationContext.CurrentSelectedRoleOrder;
						activationContext.ClearFactTypeSelection();
						ReadingOrder readingOrder = FactSaver.IntegrateParsedFactType(
							activationContext.CurrentDocument,
							activationContext.CurrentDocumentView as ORMDesignerDocView,
							activeFactType,
							activeReadingOrder,
							activeRoleOrder,
							parsedFactType);
						source.SetText((readingOrder != null) ? HeadReading(GetHeadText(readingOrder.ReadingText), readingOrder) : "");
					}
				}
				else if (guidCmdGroup == typeof(VSConstants.VSStd2KCmdID).GUID)
				{
					switch ((VSConstants.VSStd2KCmdID)nCmdId)
					{
						case VSConstants.VSStd2KCmdID.OPENLINEABOVE:
						case VSConstants.VSStd2KCmdID.OPENLINEBELOW:
						case VSConstants.VSStd2KCmdID.RETURN:
							return true;
					}
				}
				return base.HandlePreExec(ref guidCmdGroup, nCmdId, nCmdexecopt, pvaIn, pvaOut);
			}
			private static string HeadReading(string headText, ReadingOrder readingOrder)
			{
				return Reading.ReplaceFields(
					headText,
					delegate(int replaceIndex)
					{
						if (replaceIndex == 0)
						{
							IList<RoleBase> roles = readingOrder.RoleCollection;
							// Note that sending the defaultRoleOrder based on the
							// reading order means that the isUnaryRole parameter is
							// meaningless.
							return FormatReplacementField(roles[0], roles, false);
						}
						return "";
					});
			}

			private static string GetHeadText(string reading)
			{
				int replacePosition = reading.IndexOf("{0} ");
				if (replacePosition >= 0 && replacePosition == reading.IndexOf('{'))
				{
					return reading.Substring(0, replacePosition + 4);
				}
				return "";
			}
		}
		#endregion // FactEditorViewFilter class
		#region Constants
		/// <summary>
		/// The official language name for this language service
		/// </summary>
		public const string LanguageName = "ORM Fact Editor";
		#endregion // Constraints
		#region Private Members
		private FactEditorLineScanner m_FactScanner;
		private LanguagePreferences m_LanguagePreferences;
		private ImageList m_ImageList;
		private IVsWindowFrame m_ToolWindow;
		private ActivationManager m_ActivationManager;
		#endregion // Private Members
		#region Guid Fields
		/// <summary>
		/// Guid string for the language service.
		/// </summary>
		public static readonly Guid Guid = new Guid(GuidString);
		/// <summary>
		/// Guid string for the language service.
		/// </summary>
		public const string GuidString = "C50CD300-9D1E-4AB0-B494-73FA23D14D2B";
		#endregion // Guid Fields
		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="FactEditorLanguageService"/> class.
		/// </summary>
		public FactEditorLanguageService()
		{
			m_ImageList = new ImageList();
			m_ImageList.ImageSize = new Size(16, 16);
			m_ImageList.ColorDepth = ColorDepth.Depth24Bit;
			m_ImageList.TransparentColor = System.Drawing.Color.Lime;
			m_ImageList.ImageStream = ResourceStrings.FactEditorIntellisenseImageList;
		}
		#endregion // Constructor
		#region Overridden Properties
		/// <summary>
		/// Returns the name of the language (for example, "C++" or "HTML").
		/// </summary>
		/// <returns>
		/// Returns a string containing the name of the language. This must return the same
		/// name the language service was registered with in Visual Studio.
		/// </returns>
		public override string Name
		{
			get { return LanguageName; }
		}
		#endregion // Overridden Properties
		#region Overridden Methods
		/// <summary>Returns a list of file extension filters suitable for a Save As dialog box.</summary>
		/// <returns>If successful, returns a string containing the file extension filters; otherwise, returns an empty string.</returns>
		public override string GetFormatFilterList()
		{
			return string.Empty;
		}

		/// <summary>
		/// Create a CodeWindowManager with appropriate key bindings and view filters
		/// </summary>
		public override CodeWindowManager CreateCodeWindowManager(IVsCodeWindow codeWindow, Source source)
		{
			return new FactEditorCodeWindowManager(this, codeWindow, source);
		}
		/// <summary>
		/// Create a view filter that handles additional commands
		/// </summary>
		public override ViewFilter CreateViewFilter(CodeWindowManager mgr, IVsTextView newView)
		{
			return new FactEditorViewFilter(mgr, newView);
		}
		/// <summary>
		/// Creates a IVsTextLines and forwards the call to the overloaded GetOrCreateSource method.
		/// </summary>
		/// <param name="view">The IVsTextView to get or create a source from.</param>
		public Source GetOrCreateSource(IVsTextView view)
		{
			if (view == null)
			{
				return null;
			}

			IVsTextLines ppBuffer;
			ErrorHandler.ThrowOnFailure(view.GetBuffer(out ppBuffer));

			return GetOrCreateSource(ppBuffer);
		}

#if !VISUALSTUDIO_9_0
		/// <summary>
		/// Calls GetSource with the given IVsTextLines.  If the source does not exist
		/// it is created and returned.
		/// </summary>
		/// <param name="lines">The IVsTextLines of the source you want to get or create.</param>
		public Source GetOrCreateSource(IVsTextLines lines)
		{
			Source s = base.GetSource(lines);
			if (s != null)
			{
				return s;
			}
			else
			{
				s = CreateSource(lines);
				((IList)GetSources()).Add(s);
			}
			return s;
		}
#endif //!VISUALSTUDIO_9_0
		/// <summary>
		/// Returns a <see cref="T:Microsoft.VisualStudio.Package.LanguagePreferences"></see>
		/// object for this language service.
		/// </summary>
		/// <returns>
		/// If successful, returns a <see cref="T:Microsoft.VisualStudio.Package.LanguagePreferences"></see> object;
		/// otherwise, returns a null value.
		/// </returns>
		public override LanguagePreferences GetLanguagePreferences()
		{
			LanguagePreferences retVal = m_LanguagePreferences;
			if (retVal == null)
			{
				retVal = new LanguagePreferences(this.Site, typeof(FactEditorLanguageService).GUID, this.Name);

				retVal.Init();  // Must do this first!

				retVal.AutoListMembers = true;
				retVal.EnableCodeSense = true;
				retVal.EnableMatchBraces = true;
				retVal.EnableQuickInfo = true;
				retVal.EnableMatchBracesAtCaret = true;
				retVal.EnableShowMatchingBrace = true;
				retVal.MaxErrorMessages = 10;

				System.Threading.Interlocked.CompareExchange<LanguagePreferences>(ref m_LanguagePreferences, retVal, null);
				retVal = m_LanguagePreferences;
			}
			return retVal;
		}

		/// <summary>
		/// Returns a single instantiation of a parser.
		/// </summary>
		/// <param name="buffer">[in] An <see cref="T:Microsoft.VisualStudio.TextManager.Interop.IVsTextLines"></see>
		/// representing the lines of source to parse.</param>
		/// <returns>
		/// If successful, returns an <see cref="T:Microsoft.VisualStudio.Package.IScanner"></see> object;
		/// otherwise, returns a null value.
		/// </returns>
		public override IScanner GetScanner(IVsTextLines buffer)
		{
			FactEditorLineScanner retVal = m_FactScanner;
			if (retVal == null)
			{
				m_FactScanner = retVal = new FactEditorLineScanner(this, buffer);
			}
			return retVal;
		}

		/// <summary>
		/// Parses the source based on the specified <see cref="T:Microsoft.VisualStudio.Package.ParseRequest"></see> object.
		/// </summary>
		/// <param name="req">[in] The <see cref="T:Microsoft.VisualStudio.Package.ParseRequest"></see>
		/// describing how to parse the source file.</param>
		/// <returns>
		/// If successful, returns an <see cref="T:Microsoft.VisualStudio.Package.AuthoringScope"></see> object;
		/// otherwise, returns a null value.
		/// </returns>
		public override AuthoringScope ParseSource(ParseRequest req)
		{
			//Trace.WriteLine(String.Format("\nParse Request: {0}\n", req.Reason.ToString()));

			Source s = this.GetSource(req.View);

			return new FactEditorAuthoringScope(this, req);
		}

		/// <summary>
		/// Returns an image list containing glyphs associated with the language service.
		/// </summary>
		/// <returns>
		/// If successful, returns an <see cref="T:System.Windows.Forms.ImageList"></see> object; otherwise, returns a null value.
		/// </returns>
		public override System.Windows.Forms.ImageList GetImageList()
		{
			return m_ImageList;
		}
		/// <summary>
		/// Close the tool window if it has been opened
		/// </summary>
		public override void Dispose()
		{
			if (m_ToolWindow != null)
			{
				m_ToolWindow.CloseFrame(0);
			}
			base.Dispose();
		}
		#endregion // Overridden Methods
		#region Helper Methods
		private static string FormatReplacementField(RoleBase role, IList<RoleBase> defaultRoleOrder, bool isUnaryRole)
		{
			string retVal = null;
			ObjectType player = role.Role.RolePlayer;
			if (player != null)
			{
				string refModeString = player.ReferenceModeDecoratedString;
				string playerName = player.Name;
				bool needsBrackets = !string.IsNullOrEmpty(playerName) && (Char.IsLower(playerName[0]) || playerName.IndexOf(' ') >= 0);
				if (string.IsNullOrEmpty(refModeString) && !player.IsValueType)
				{
					retVal = string.Format(CultureInfo.InvariantCulture, ResourceStrings.FactEditorUnqualifiedRolePlayerFormatString, playerName);
				}
				else
				{
					retVal = string.Format(CultureInfo.InvariantCulture, ResourceStrings.FactEditorQualifiedRolePlayerFormatString, playerName, refModeString);
				}
				if (needsBrackets)
				{
					retVal = "[" + retVal + "]";
				}
			}
			else
			{
				retVal = string.Format(CultureInfo.InvariantCulture, ResourceStrings.FactEditorMissingRolePlayerFormatString, isUnaryRole ? 1 : (defaultRoleOrder.IndexOf(role) + 1));
			}
			return retVal;
		}
		/// <summary>
		/// <see cref="StringBuilder"/> based version of <see cref="FormatReplacementField(RoleBase, IList{RoleBase}, bool)"/>
		/// </summary>
		private static void FormatReplacementField(StringBuilder builder, RoleBase role, IList<RoleBase> defaultRoleOrder, bool isUnaryRole)
		{
			ObjectType player = role.Role.RolePlayer;
			if (player != null)
			{
				FormatObjectType(builder, player);
			}
			else
			{
				builder.AppendFormat(CultureInfo.InvariantCulture, ResourceStrings.FactEditorMissingRolePlayerFormatString, isUnaryRole ? 1 : (defaultRoleOrder.IndexOf(role) + 1));
			}
		}
		/// <summary>
		/// Format a single <see cref="ObjectType"/> as a valid role replacement field
		/// </summary>
		private static void FormatObjectType(StringBuilder builder, ObjectType objectType)
		{
			string refModeString = objectType.ReferenceModeDecoratedString;
			string objectTypeName = objectType.Name;
			bool needsBrackets = !string.IsNullOrEmpty(objectTypeName) && (Char.IsLower(objectTypeName[0]) || objectTypeName.IndexOf(' ') >= 0);
			if (needsBrackets)
			{
				builder.Append('[');
			}
			if (string.IsNullOrEmpty(refModeString) && !objectType.IsValueType)
			{
				builder.AppendFormat(CultureInfo.InvariantCulture, ResourceStrings.FactEditorUnqualifiedRolePlayerFormatString, objectTypeName);
			}
			else
			{
				builder.AppendFormat(CultureInfo.InvariantCulture, ResourceStrings.FactEditorQualifiedRolePlayerFormatString, objectTypeName, refModeString);
			}
			if (needsBrackets)
			{
				builder.Append(']');
			}
		}
		#endregion // Helper Methods
		#region ToolWindow Creation and Activation
		#region ActivationManager class
		/// <summary>
		/// A helper class used to track selection changes and notify other
		/// language service components.
		/// </summary>
		private sealed class ActivationManager : INotifyToolWindowActivation<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer>
		{
			#region Private Fields
			private ToolWindowActivationHelper<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer> myActivator;
			/// <summary>
			/// Context <see cref="FactEditorLanguageService"/>
			/// </summary>
			private FactEditorLanguageService myLanguageService;
			/// <summary>
			/// Source associated with the text buffer on this window
			/// </summary>
			private Source mySource;
			/// <summary>
			/// <see cref="IVsTextView"/> associated with this window
			/// </summary>
			private IVsTextView myTextView;
			/// <summary>
			/// The currently selected <see cref="FactType"/>
			/// </summary>
			private FactType mySelectedFactType;
			/// <summary>
			/// The currently selected existing <see cref="ReadingOrder"/>
			/// </summary>
			private ReadingOrder mySelectedReadingOrder;
			/// <summary>
			/// The <see cref="ReadingOrder.ReadingText"/> value of the currently selected <see cref="ReadingOrder"/>
			/// </summary>
			private string mySelectedReadingOrderText;
			/// <summary>
			/// The currently selected existing reverse <see cref="ReadingOrder"/>
			/// </summary>
			private ReadingOrder mySelectedReverseReadingOrder;
			/// <summary>
			/// The <see cref="ReadingOrder.ReadingText"/> value of the currently selected reverse <see cref="ReadingOrder"/>
			/// </summary>
			private string mySelectedReverseReadingOrderText;
			/// <summary>
			/// The selected role order. Set if a specific reading
			/// order is not available for the current FactType selection.
			/// </summary>
			private IList<RoleBase> mySelectedRoleOrder;
			/// <summary>
			/// The default role order for the currently selected FactType,
			/// regardless of the selected role order.
			/// </summary>
			private IList<RoleBase> myDefaultRoleOrder;
			/// <summary>
			/// Cached objects written to the Fact Editor window
			/// </summary>
			private IList<ObjectType> mySelectedObjectTypes;
			/// <summary>
			/// The number of elements from the <see cref="mySelectedObjectTypes"/> field
			/// that should be used.
			/// </summary>
			private int mySelectedObjectTypeCount;
			#endregion // Private Fields
			#region Constructor
			/// <summary>
			/// Create a new <see cref="ActivationManager"/>
			/// </summary>
			public ActivationManager(FactEditorLanguageService languageService, IVsWindowFrame toolWindowFrame, Source source, IVsTextView textView)
			{
				myLanguageService = languageService;
				mySource = source;
				myTextView = textView;
				myActivator = new ToolWindowActivationHelper<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer>(
					languageService.Site,
					toolWindowFrame,
					CoveredFrameContentActions.ClearContentsOnDocumentChanged | CoveredFrameContentActions.ClearContentsOnSelectionChanged,
					this);
			}
			#endregion // Constructor
			#region Public Properties
			/// <summary>
			/// Get the current document
			/// </summary>
			public ORMDesignerDocData CurrentDocument
			{
				get
				{
					return myActivator.CurrentDocument;
				}
			}
			/// <summary>
			/// Get the current document
			/// </summary>
			public DiagramDocView CurrentDocumentView
			{
				get
				{
					return myActivator.CurrentDocumentView;
				}
			}
			/// <summary>
			/// The fact type currently being edited
			/// </summary>
			public FactType CurrentFactType
			{
				get
				{
					return mySelectedFactType;
				}
			}
			/// <summary>
			/// The currently selected reading order
			/// </summary>
			public ReadingOrder CurrentReadingOrder
			{
				get
				{
					return mySelectedReadingOrder;
				}
			}
			/// <summary>
			/// The currently selected role order for the <see cref="CurrentFactType"/>.
			/// This will only be set if <see cref="CurrentReadingOrder"/> is <see langword="null"/>
			/// and CurrentFactType is not <see langword="null"/>
			/// </summary>
			public IList<RoleBase> CurrentSelectedRoleOrder
			{
				get
				{
					return mySelectedRoleOrder;
				}
			}
			#endregion // Public Properties
			#region INotifyToolWindowActivation<ORMDesignerDocData,DiagramDocView,IORMSelectionContainer> Implementation
			void INotifyToolWindowActivation<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer>.ActivatorSelectionContainerChanged(ToolWindowActivationHelper<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer> activator)
			{
				UpdateSelection();
			}
			void INotifyToolWindowActivation<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer>.ActivatorDocumentChanged(ToolWindowActivationHelper<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer> activator)
			{
			}
			void INotifyToolWindowActivation<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer>.ActivatorDocumentViewChanged(ToolWindowActivationHelper<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer> activator)
			{
			}
			void INotifyToolWindowActivation<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer>.ActivatorAttachEventHandlers(ORMDesignerDocData docData)
			{
				Store store = docData.Store;
				if (store != null)
				{
					ManageEvents(docData.Store, EventHandlerAction.Add);
				}
			}
			void INotifyToolWindowActivation<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer>.ActivatorDetachEventHandlers(ORMDesignerDocData docData)
			{
				Store store = docData.Store;
				if (store != null)
				{
					ManageEvents(docData.Store, EventHandlerAction.Remove);
				}
			}
			private void ManageEvents(Store store, EventHandlerAction action)
			{
				ModelingEventManager eventManager = ((IModelingEventManagerProvider)store).ModelingEventManager;
				DomainDataDirectory directory = store.DomainDataDirectory;

				// Events begun/ended
				eventManager.AddOrRemoveHandler(new EventHandler<ElementEventsBegunEventArgs>(EventsBeginning), action);
				eventManager.AddOrRemoveHandler(new EventHandler<ElementEventsEndedEventArgs>(EventsEnding), action);

				// Role add/delete
				DomainClassInfo classInfo = directory.FindDomainRelationship(FactTypeHasRole.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(RoleAdded), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(RoleDeleted), action);

				// ReadingOrder add/delete/reorder
				classInfo = directory.FindDomainRelationship(FactTypeHasReadingOrder.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ReadingOrderAdded), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ReadingOrderDeleted), action);
				DomainRoleInfo domainRoleInfo = directory.FindDomainRole(FactTypeHasReadingOrder.ReadingOrderDomainRoleId);
				eventManager.AddOrRemoveHandler(domainRoleInfo, new EventHandler<RolePlayerOrderChangedEventArgs>(ReadingOrderPositionChanged), action);

				// Reading add/delete/reorder/change
				classInfo = directory.FindDomainRelationship(ReadingOrderHasReading.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ReadingAdded), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ReadingDeleted), action);
				domainRoleInfo = directory.FindDomainRole(ReadingOrderHasReading.ReadingDomainRoleId);
				eventManager.AddOrRemoveHandler(domainRoleInfo, new EventHandler<RolePlayerOrderChangedEventArgs>(ReadingPositionChanged), action);
				DomainPropertyInfo propertyInfo = directory.FindDomainProperty(Reading.TextDomainPropertyId);
				eventManager.AddOrRemoveHandler(propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ReadingTextChanged), action);

				// DisplayOrder added/delete/reorder
				classInfo = directory.GetDomainRelationship(FactTypeShapeHasRoleDisplayOrder.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(RoleDisplayOrderAdded), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(RoleDisplayOrderDeleted), action);
				domainRoleInfo = directory.FindDomainRole(FactTypeShapeHasRoleDisplayOrder.RoleDisplayOrderDomainRoleId);
				eventManager.AddOrRemoveHandler(domainRoleInfo, new EventHandler<RolePlayerOrderChangedEventArgs>(RoleDisplayOrderPositionChanged), action);

				// RolePlayer add/delete/change
				classInfo = directory.GetDomainRelationship(ObjectTypePlaysRole.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(RolePlayerAdded), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(RolePlayerDeleted), action);
				domainRoleInfo = directory.FindDomainRole(ObjectTypePlaysRole.RolePlayerDomainRoleId);
				eventManager.AddOrRemoveHandler(domainRoleInfo, new EventHandler<RolePlayerChangedEventArgs>(RolePlayerChanged), action);

				// ObjectType add/delete/change
				classInfo = directory.GetDomainClass(ObjectType.DomainClassId);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(ObjectTypeAdded), action);
				eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(ObjectTypeDeleted), action);
				propertyInfo = directory.FindDomainProperty(ObjectType.NameDomainPropertyId);
				eventManager.AddOrRemoveHandler(classInfo, propertyInfo, new EventHandler<ElementPropertyChangedEventArgs>(ObjectTypeNameChanged), action);
			}
			#endregion // INotifyToolWindowActivation<ORMDesignerDocData,DiagramDocView,IORMSelectionContainer> Implementation
			#region Selection Handling Methods
			private bool myRequireUpdate;
			private bool myForceUpdate;
			private bool myInEvents;
			private void EventsBeginning(object sender, ElementEventsBegunEventArgs e)
			{
				myRequireUpdate = false;
				myForceUpdate = false;
				myInEvents = true;
			}
			private void EventsEnding(object sender, ElementEventsEndedEventArgs e)
			{
				myInEvents = false;
				if (myRequireUpdate)
				{
					UpdateSelection(myForceUpdate);
				}
			}
			private void UpdateSelection()
			{
				bool forceUpdate = myForceUpdate;
				myForceUpdate = false;
				UpdateSelection(forceUpdate);
			}
			private void UpdateSelection(bool forceUpdate)
			{
				if (myInEvents)
				{
					myRequireUpdate = true;
					myForceUpdate |= forceUpdate;
					return;
				}
				myRequireUpdate = false;
				IORMSelectionContainer selectionContainer = myActivator.CurrentSelectionContainer;
				FactType currentFactType = null;
				ObjectType firstObjectType = null;
				const int MaxSelectedObjectTypes = 10;
				IList<ObjectType> selectedObjectTypes = null;
				int selectedObjectTypeCount = 0;
				ReadingOrder currentReadingOrder = null;
				IList<RoleBase> currentReadingOrderRoles = null;
				ReadingOrder reverseReadingOrder = null;
				RoleBase leadRole = null;
				RoleBase[] roleOrder = null;
				RoleBase unaryRole = null;
				IList<RoleBase> roleOrderList = null;
				string forwardReadingText = null;
				string reverseReadingText = null;

				IList<RoleBase> defaultOrder = null;
				int openRoleIndex = 0;
				if (selectionContainer != null)
				{
					ICollection selectedObjects = selectionContainer.GetSelectedComponents();
					if (selectedObjects != null)
					{
						LinkedElementCollection<RoleBase> factRoles = null;
						int fullOrderRoleCount = 0;
						foreach (object element in selectedObjects)
						{
							if (selectedObjectTypes != null)
							{
								// We're not trying to get a FactType anymore, just an
								// ordered sequence of object types
								ObjectType currentObjectType = EditorUtility.ResolveContextInstance(element, false) as ObjectType;
								if (currentObjectType == null)
								{
									FactType testFactType = ORMEditorUtility.ResolveContextFactType(element);
									if (testFactType != null)
									{
										currentObjectType = testFactType.NestingType;
									}
								}
								bool abandonObjectTypeSelection = false;
								if (currentObjectType == null)
								{
									abandonObjectTypeSelection = true;
								}
								else if (!selectedObjectTypes.Contains(currentObjectType))
								{
									if (selectedObjectTypeCount == MaxSelectedObjectTypes)
									{
										abandonObjectTypeSelection = true;
									}
									else
									{
										selectedObjectTypes[selectedObjectTypeCount] = currentObjectType;
										++selectedObjectTypeCount;
									}
								}
								if (abandonObjectTypeSelection)
								{
									selectedObjectTypes = null;
									selectedObjectTypeCount = 0;
									break;
								}
							}
							else
							{
								FactType testFactType = ORMEditorUtility.ResolveContextFactType(element);
								if (testFactType is SubtypeFact)
								{
									// Readings on subtypes are not directly edited
									testFactType = null;
								}
								// Handle selection of multiple elements as long as
								// they all resolve to the same facttype
								if (testFactType == null)
								{
									ObjectType currentObjectType = EditorUtility.ResolveContextInstance(element, false) as ObjectType;
									if (currentObjectType != null)
									{
										selectedObjectTypes = new ObjectType[MaxSelectedObjectTypes];
										if (firstObjectType != null)
										{
											selectedObjectTypes[0] = firstObjectType;
											selectedObjectTypeCount = 1;
										}
										if (firstObjectType != currentObjectType)
										{
											selectedObjectTypes[selectedObjectTypeCount] = currentObjectType;
											++selectedObjectTypeCount;
										}
									}
									currentFactType = null;
									roleOrder = null;
									roleOrderList = null;
								}
								else if (currentFactType == null)
								{
									currentFactType = testFactType;
									// Track in case we find that we want to use this
									// as the ObjectType part of this, not the 
									firstObjectType = testFactType.NestingType;
								}
								else if (testFactType != currentFactType)
								{
									currentFactType = null;
									roleOrder = null;
									roleOrderList = null;
									break;
								}
								if (testFactType != null)
								{
									if (unaryRole == null)
									{
										if (factRoles == null)
										{
											factRoles = testFactType.RoleCollection;
											fullOrderRoleCount = factRoles.Count;
											int? unaryRoleIndex = FactType.GetUnaryRoleIndex(factRoles);
											if (unaryRoleIndex.HasValue)
											{
												unaryRole = factRoles[unaryRoleIndex.Value];
												fullOrderRoleCount -= 1;
												leadRole = unaryRole;
												continue;
											}
										}
										RoleBase currentRole = element as RoleBase;
										if (currentRole != null)
										{
											if (leadRole == null)
											{
												leadRole = currentRole;
											}
											else if (roleOrder == null)
											{
												if (currentRole != leadRole)
												{
													roleOrder = new RoleBase[fullOrderRoleCount];
													roleOrderList = roleOrder;
													roleOrder[0] = leadRole;
													roleOrder[1] = currentRole;
													openRoleIndex = 2;
												}
											}
											else if (!roleOrderList.Contains(currentRole))
											{
												roleOrder[openRoleIndex] = currentRole;
												++openRoleIndex;
											}
										}
									}
								}
							}
						}

						if (currentFactType != null)
						{
							LinkedElementCollection<ReadingOrder> readingOrders = currentFactType.ReadingOrderCollection;
							// First base the order on the role selection
							if (leadRole != null)
							{
								if (roleOrder == null)
								{
									roleOrder = new RoleBase[fullOrderRoleCount];
									roleOrderList = roleOrder;
									roleOrder[0] = leadRole;
									openRoleIndex = 1;
								}
								currentReadingOrder = FactType.FindMatchingReadingOrder(readingOrders, roleOrder);
							}

							// Get the default display order based sole on the current FactType selection
							// Try to get preferred order from a FactTypeShape
							bool mergeDefaultOrder = false;
							ModelingWindowPane pane = selectionContainer as ModelingWindowPane;
							if (pane != null)
							{
								ShapeElement pel = pane.PrimarySelection as ShapeElement;
								while (pel != null)
								{
									FactTypeShape factTypeShape = pel as FactTypeShape;
									if (factTypeShape != null)
									{
										defaultOrder = factTypeShape.DisplayedRoleOrder;
										if (currentReadingOrder == null)
										{
											currentReadingOrder = FactType.FindMatchingReadingOrder(readingOrders, defaultOrder);
											mergeDefaultOrder = true;
										}
										break;
									}
									pel = pel.ParentShape;
								}
							}

							// Fallback on default order from the FactType
							if (defaultOrder == null)
							{
								foreach (ReadingOrder order in readingOrders)
								{
									defaultOrder = order.RoleCollection;
									if (currentReadingOrder == null)
									{
										currentReadingOrder = order;
										currentReadingOrderRoles = defaultOrder;
										mergeDefaultOrder = true;
									}
									break;
								}
								if (defaultOrder == null && unaryRole == null)
								{
									defaultOrder = factRoles;
								}
							}

							if (currentReadingOrder == null || mergeDefaultOrder)
							{
								// Merge the default order with the selected roles
								if (roleOrder != null)
								{
									if (openRoleIndex < fullOrderRoleCount)
									{
										Debug.Assert(defaultOrder != null && unaryRole == null && defaultOrder.Count == fullOrderRoleCount);
										if (leadRole == defaultOrder[fullOrderRoleCount - 1])
										{
											// Fill in backwards on the default order
											for (int i = fullOrderRoleCount - 2; i >= 0; --i)
											{
												RoleBase testRole = defaultOrder[i];
												if (!roleOrderList.Contains(testRole))
												{
													roleOrder[openRoleIndex] = testRole;
													if (++openRoleIndex == fullOrderRoleCount)
													{
														break;
													}
												}
											}
										}
										else
										{
											for (int i = 0; i < fullOrderRoleCount; ++i)
											{
												RoleBase testRole = defaultOrder[i];
												if (!roleOrderList.Contains(testRole))
												{
													roleOrder[openRoleIndex] = testRole;
													if (++openRoleIndex == fullOrderRoleCount)
													{
														break;
													}
												}
											}
										}
									}
									currentReadingOrder = null;
								}
								else if (currentReadingOrder == null)
								{
									roleOrder = new RoleBase[fullOrderRoleCount];
									defaultOrder.CopyTo(roleOrder, 0);
								}
							}
							else
							{
								roleOrder = null;
							}

							// Check reverse order for binary display. Combining displays requires
							// that both readings begin and end with the replacement fields
							if (currentReadingOrder != null && fullOrderRoleCount == 2)
							{
								forwardReadingText = currentReadingOrder.ReadingText;
								if (forwardReadingText.StartsWith("{0}") && forwardReadingText.EndsWith("{1}"))
								{
									if (currentReadingOrderRoles == null)
									{
										currentReadingOrderRoles = currentReadingOrder.RoleCollection;
									}
									reverseReadingOrder = FactType.FindMatchingReadingOrder(readingOrders, new RoleBase[] { currentReadingOrderRoles[1], currentReadingOrderRoles[0] });
									if (reverseReadingOrder != null)
									{
										reverseReadingText = reverseReadingOrder.ReadingText;
										if (!(reverseReadingText.StartsWith("{0}") && reverseReadingText.EndsWith("{1}")))
										{
											reverseReadingText = null;
											reverseReadingOrder = null;
										}
									}
								}
							}
						}
					}
				}
				int newCaretPosition = -1;
				string newSourceText = null;
				string currentReadingOrderText = currentReadingOrder != null ? currentReadingOrder.ReadingText : null;
				string reverseReadingOrderText = reverseReadingOrder != null ? reverseReadingOrder.ReadingText : null;
				if (forceUpdate ||
					(mySelectedFactType != currentFactType ||
					mySelectedReadingOrder != currentReadingOrder ||
					mySelectedReadingOrderText != currentReadingOrderText ||
					mySelectedReverseReadingOrder != reverseReadingOrder ||
					mySelectedReverseReadingOrderText != reverseReadingOrderText ||
					!ListEquals(mySelectedRoleOrder, roleOrder) ||
					!ListEquals(myDefaultRoleOrder, defaultOrder) ||
					mySelectedObjectTypeCount != selectedObjectTypeCount ||
					!ListEquals(mySelectedObjectTypes, selectedObjectTypes)))
				{
					mySelectedRoleOrder = (roleOrder == null) ? null : Array.AsReadOnly<RoleBase>(roleOrder);
					myDefaultRoleOrder = defaultOrder;
					mySelectedReadingOrder = currentReadingOrder;
					mySelectedReadingOrderText = currentReadingOrderText;
					mySelectedReverseReadingOrder = reverseReadingOrder;
					mySelectedReverseReadingOrderText = reverseReadingOrderText;
					mySelectedFactType = currentFactType;
					mySelectedObjectTypes = selectedObjectTypes;
					mySelectedObjectTypeCount = selectedObjectTypeCount;

					newSourceText = "";
					if (currentFactType != null)
					{
						if (currentReadingOrder != null)
						{
							if (currentReadingOrderRoles == null)
							{
								currentReadingOrderRoles = currentReadingOrder.RoleCollection;
							}
							int replaceRoleCount = currentReadingOrderRoles.Count;
							if (forwardReadingText == null)
							{
								forwardReadingText = currentReadingOrder.ReadingText;
							}
							else if (reverseReadingText != null)
							{
								reverseReadingText = reverseReadingText.Substring(3, reverseReadingText.Length - 6).Trim();
								int insertAfter = forwardReadingText.Length - 4;
								while (char.IsWhiteSpace(forwardReadingText[insertAfter]))
								{
									--insertAfter;
								}
								forwardReadingText = forwardReadingText.Insert(insertAfter + 1, "/" + reverseReadingText);
							}
							newSourceText = Reading.ReplaceFields(
								forwardReadingText,
								delegate(int replaceIndex)
								{
									if (replaceIndex < replaceRoleCount)
									{
										return FormatReplacementField(currentReadingOrderRoles[replaceIndex], defaultOrder, unaryRole != null);
									}
									return null;
								});
						}
						else if (roleOrder != null)
						{
							// List all of the replacement fields for the
							// selected role order
							StringBuilder sb = new StringBuilder();
							int i = 0;
							for (; i < roleOrder.Length; ++i)
							{
								if (i != 0)
								{
									if (i == 1)
									{
										newCaretPosition = sb.Length + 1;
									}
									sb.Append(' ', 2);
								}
								FormatReplacementField(sb, roleOrder[i], defaultOrder, unaryRole != null);
							}
							if (i == 1)
							{
								sb.Append(' ');
							}
							newSourceText = sb.ToString();
						}
					}
				}
				if (selectedObjectTypes != null)
				{
					StringBuilder sb = new StringBuilder();
					int i = 0;
					for (; i < selectedObjectTypeCount; ++i)
					{
						if (i != 0)
						{
							if (i == 1)
							{
								newCaretPosition = sb.Length + 1;
							}
							sb.Append(' ', 2);
						}
						FormatObjectType(sb, selectedObjectTypes[i]);
					}
					if (i == 1)
					{
						sb.Append(' ');
					}
					newSourceText = sb.ToString();
				}
				if (newSourceText != null)
				{
					mySource.SetText(newSourceText);
				}
				if (newCaretPosition > 0)
				{
					myTextView.SetCaretPos(0, newCaretPosition);
				}
			}
			private static bool ListEquals<T>(IList<T> order1, IList<T> order2) where T : class
			{
				if (order1 == null)
				{
					return order2 == null;
				}
				else if (order2 == null)
				{
					return false;
				}
				int order1Count = order1.Count;
				if (order2.Count != order1.Count)
				{
					return false;
				}
				for (int i = 0; i < order1Count; ++i)
				{
					if (order1[i] != order2[i])
					{
						return false;
					}
				}
				return true;
			}
			/// <summary>
			/// Clear any cached selection properties relating to the currently selected FactType
			/// </summary>
			public void ClearFactTypeSelection()
			{
				mySelectedFactType = null;
				mySelectedReadingOrder = null;
				mySelectedReadingOrderText = null;
				mySelectedReverseReadingOrder = null;
				mySelectedReverseReadingOrderText = null;
				mySelectedRoleOrder = null;
				myDefaultRoleOrder = null;
			}
			#endregion // Selection Handling Methods
			#region Event Handlers
			private void RoleAdded(object sender, ElementAddedEventArgs e)
			{
				FactType currentFactType = mySelectedFactType;
				if (currentFactType != null && ((FactTypeHasRole)e.ModelElement).FactType == currentFactType)
				{
					UpdateSelection();
				}
			}
			private void RoleDeleted(object sender, ElementDeletedEventArgs e)
			{
				FactType currentFactType = mySelectedFactType;
				if (currentFactType != null && ((FactTypeHasRole)e.ModelElement).FactType == currentFactType)
				{
					UpdateSelection();
				}
			}
			private void ReadingOrderAdded(object sender, ElementAddedEventArgs e)
			{
				FactType currentFactType = mySelectedFactType;
				if (currentFactType != null && ((FactTypeHasReadingOrder)e.ModelElement).FactType == currentFactType)
				{
					UpdateSelection();
				}
			}
			private void ReadingOrderDeleted(object sender, ElementDeletedEventArgs e)
			{
				FactType currentFactType = mySelectedFactType;
				if (currentFactType != null && ((FactTypeHasReadingOrder)e.ModelElement).FactType == currentFactType)
				{
					UpdateSelection();
				}
			}
			private void ReadingOrderPositionChanged(object sender, RolePlayerOrderChangedEventArgs e)
			{
				FactType currentFactType = mySelectedFactType;
				if (currentFactType != null && (FactType)e.SourceElement == currentFactType)
				{
					UpdateSelection();
				}
			}
			private void ReadingPositionChanged(object sender, RolePlayerOrderChangedEventArgs e)
			{
				ReadingOrder currentReadingOrder = mySelectedReadingOrder;
				if (currentReadingOrder != null &&
					(e.NewOrdinal == 0 || e.OldOrdinal == 0))
				{
					ReadingOrder order = (ReadingOrder)e.SourceElement;
					if (order == currentReadingOrder || order == mySelectedReverseReadingOrder)
					{
						UpdateSelection();
					}
				}
			}
			private void ReadingAdded(object sender, ElementAddedEventArgs e)
			{
				ReadingOrder currentReadingOrder = mySelectedReadingOrder;
				if (currentReadingOrder != null)
				{
					ReadingOrder order = ((ReadingOrderHasReading)e.ModelElement).ReadingOrder;
					if (order == currentReadingOrder || order == mySelectedReverseReadingOrder)
					{
						UpdateSelection();
					}
				}
			}
			private void ReadingDeleted(object sender, ElementDeletedEventArgs e)
			{
				ReadingOrder currentReadingOrder = mySelectedReadingOrder;
				if (currentReadingOrder != null)
				{
					ReadingOrder order = ((ReadingOrderHasReading)e.ModelElement).ReadingOrder;
					if (!order.IsDeleted &&
						(order == currentReadingOrder || order == mySelectedReverseReadingOrder))
					{
						UpdateSelection();
					}
				}
			}
			private void ReadingTextChanged(object sender, ElementPropertyChangedEventArgs e)
			{
				ReadingOrder currentReadingOrder = mySelectedReadingOrder;
				if (currentReadingOrder != null)
				{
					Reading reading = (Reading)e.ModelElement;
					ReadingOrder order;
					if (!reading.IsDeleted &&
						null != (order = reading.ReadingOrder) &&
						(order == currentReadingOrder || order== mySelectedReverseReadingOrder))
					{
						UpdateSelection();
					}
				}
			}
			private void RoleDisplayOrderAdded(object sender, ElementAddedEventArgs e)
			{
				FactType currentFactType = mySelectedFactType;
				if (currentFactType != null &&
					((FactTypeShapeHasRoleDisplayOrder)e.ModelElement).FactTypeShape.ModelElement == currentFactType)
				{
					UpdateSelection();
				}
			}
			private void RoleDisplayOrderDeleted(object sender, ElementDeletedEventArgs e)
			{
				FactType currentFactType = mySelectedFactType;
				FactTypeShape shape;
				if (currentFactType != null &&
					!(shape = ((FactTypeShapeHasRoleDisplayOrder)e.ModelElement).FactTypeShape).IsDeleted &&
					shape.ModelElement == currentFactType)
				{
					UpdateSelection();
				}
			}
			private void RoleDisplayOrderPositionChanged(object sender, RolePlayerOrderChangedEventArgs e)
			{
				FactType currentFactType = mySelectedFactType;
				if (currentFactType != null && ((FactTypeShape)e.SourceElement).ModelElement == currentFactType)
				{
					UpdateSelection();
				}
			}
			private void RolePlayerAdded(object sender, ElementAddedEventArgs e)
			{
				FactType currentFactType = mySelectedFactType;
				Role role;
				if (currentFactType != null &&
					!(role = ((ObjectTypePlaysRole)e.ModelElement).PlayedRole).IsDeleted &&
					role.FactType == currentFactType)
				{
					UpdateSelection(true);
				}
			}
			private void RolePlayerDeleted(object sender, ElementDeletedEventArgs e)
			{
				FactType currentFactType = mySelectedFactType;
				Role role;
				if (currentFactType != null &&
					!(role = ((ObjectTypePlaysRole)e.ModelElement).PlayedRole).IsDeleted &&
					role.FactType == currentFactType)
				{
					UpdateSelection(true);
				}
			}
			private void RolePlayerChanged(object sender, RolePlayerChangedEventArgs e)
			{
				FactType currentFactType = mySelectedFactType;
				Role role;
				if (currentFactType != null &&
					!(role = ((ObjectTypePlaysRole)e.ElementLink).PlayedRole).IsDeleted &&
					role.FactType == currentFactType)
				{
					UpdateSelection(true);
				}
			}
			private void ObjectTypeAdded(object sender, ElementAddedEventArgs e)
			{
				UpdateForObjectType(e.ModelElement);
			}
			private bool UpdateForObjectType(ModelElement element)
			{
				int selectedObjectTypeCount = mySelectedObjectTypeCount;
				if (selectedObjectTypeCount != 0)
				{
					ObjectType objectType = (ObjectType)element;
					IList<ObjectType> selectedObjectTypes = mySelectedObjectTypes;
					for (int i = 0; i < selectedObjectTypeCount; ++i)
					{
						if (selectedObjectTypes[i] == objectType)
						{
							UpdateSelection();
							return true;
						}
					}
				}
				return false;
			}
			private void ObjectTypeDeleted(object sender, ElementDeletedEventArgs e)
			{
				UpdateForObjectType(e.ModelElement);
			}
			private void ObjectTypeNameChanged(object sender, ElementPropertyChangedEventArgs e)
			{
				if (UpdateForObjectType(e.ModelElement))
				{
					return;
				}
				// Note that additional tests are done with the FactTypeNameChanged watch
				FactType currentFactType = mySelectedFactType;
				ObjectType objectType;
				if (currentFactType != null && !currentFactType.IsDeleted && !(objectType = (ObjectType)e.ModelElement).IsDeleted)
				{
					ReadingOrder order;
					IList<RoleBase> roleOrder;
					if (null != (order = mySelectedReadingOrder))
					{
						LinkedElementCollection<Role> playedRoles = objectType.PlayedRoleCollection;
						if (playedRoles.Count != 0)
						{
							foreach (RoleBase testRole in order.RoleCollection)
							{
								if (playedRoles.Contains(testRole.Role))
								{
									UpdateSelection(true);
									return;
								}
							}
						}
					}
					else if (null != (roleOrder = mySelectedRoleOrder))
					{
						LinkedElementCollection<Role> playedRoles = objectType.PlayedRoleCollection;
						if (playedRoles.Count != 0)
						{
							foreach (RoleBase testRole in roleOrder)
							{
								if (playedRoles.Contains(testRole.Role))
								{
									// We need to force update here because we're not caching the object type names
									UpdateSelection(true);
									return;
								}
							}
						}
					}
				}
			}
			#endregion // Event Handlers
		}
		private ActivationManager ActiveContext
		{
			get
			{
				return m_ActivationManager;
			}
		}
		#endregion // ActivationManager class
		#region ToolWindow creation
		/// <summary>
		/// Get the toolwindow associated with this language service
		/// </summary>
		public IVsWindowFrame FactEditorToolWindow
		{
			get
			{
				IVsWindowFrame windowFrame = m_ToolWindow;
				if (windowFrame == null)
				{
					ILocalRegistry3 locReg = (ILocalRegistry3)this.GetService(typeof(ILocalRegistry));
					IntPtr pBuf = IntPtr.Zero;
					Guid iid = typeof(IVsTextLines).GUID;
					ErrorHandler.ThrowOnFailure(locReg.CreateInstance(
						typeof(VsTextBufferClass).GUID,
						null,
						ref iid,
						(uint)CLSCTX.CLSCTX_INPROC_SERVER,
						out pBuf));

					IVsTextLines lines = null;
					IObjectWithSite objectWithSite = null;
					try
					{
						// Get an object to tie to the IDE
						lines = (IVsTextLines)Marshal.GetObjectForIUnknown(pBuf);
						objectWithSite = lines as IObjectWithSite;
						objectWithSite.SetSite(this);
					}
					finally
					{
						if (pBuf != IntPtr.Zero)
						{
							Marshal.Release(pBuf);
						}
					}

					// assign our language service to the buffer
					Guid langService = typeof(FactEditorLanguageService).GUID;
					ErrorHandler.ThrowOnFailure(lines.SetLanguageServiceID(ref langService));

					// Create a std code view (text)
					IntPtr srpCodeWin = IntPtr.Zero;
					iid = typeof(IVsCodeWindow).GUID;

					// create code view (does CoCreateInstance if not in shell's registry)
					ErrorHandler.ThrowOnFailure(locReg.CreateInstance(
						typeof(VsCodeWindowClass).GUID,
						null,
						ref iid,
						(uint)CLSCTX.CLSCTX_INPROC_SERVER,
						out srpCodeWin));

					IVsCodeWindow codeWindow = null;
					try
					{
						// Get an object to tie to the IDE
						codeWindow = (IVsCodeWindow)Marshal.GetObjectForIUnknown(srpCodeWin);
					}
					finally
					{
						if (srpCodeWin != IntPtr.Zero)
						{
							Marshal.Release(srpCodeWin);
						}
					}

					INITVIEW initView = new INITVIEW();
					initView.fSelectionMargin = 0;
					initView.IndentStyle = vsIndentStyle.vsIndentStyleNone;
					initView.fWidgetMargin = 0;
					initView.fVirtualSpace = 0;
					initView.fDragDropMove = 1;
					initView.fVisibleWhitespace = 0;
					initView.fHotURLs = 0;

					IVsCodeWindowEx codeWindowEx = codeWindow as IVsCodeWindowEx;
					int hr = codeWindowEx.Initialize(
						(uint)(_codewindowbehaviorflags.CWB_DISABLEDROPDOWNBAR | _codewindowbehaviorflags.CWB_DISABLESPLITTER),
						0,
						null,
						null,
						(uint)(TextViewInitFlags.VIF_SET_WIDGET_MARGIN |
							TextViewInitFlags.VIF_SET_SELECTION_MARGIN) |
						(uint)(TextViewInitFlags2.VIF_SUPPRESSTRACKGOBACK |
							TextViewInitFlags2.VIF_SUPPRESSBORDER |
							TextViewInitFlags2.VIF_SUPPRESS_STATUS_BAR_UPDATE |
							TextViewInitFlags2.VIF_SUPPRESSTRACKCHANGES),
						new INITVIEW[] { initView });
					ErrorHandler.ThrowOnFailure(codeWindow.SetBuffer(lines));

					IVsUIShell shell = (IVsUIShell)GetService(typeof(IVsUIShell));
					Guid emptyGuid = new Guid();
					Guid factEditorToolWindowGuid = typeof(FactEditorToolWindow).GUID;
					// CreateToolWindow ARGS
					// 0 - toolwindow.flags (initnew)
					// 1 - 0 (the tool window ID)
					// 2- IVsWindowPane
					// 3- guid null
					// 4- persistent slot (same nr as the guid attr on tool window class)
					// 5- guid null
					// 6- ole service provider (null)
					// 7- tool window.windowTitle
					// 8- int[] for position (empty array)
					// 9- out IVsWindowFrame
					ErrorHandler.ThrowOnFailure(shell.CreateToolWindow(
						(uint)__VSCREATETOOLWIN.CTW_fInitNew, // tool window flags, default to init new
						0,
						(IVsWindowPane)codeWindow,
						ref emptyGuid,
						ref factEditorToolWindowGuid,
						ref emptyGuid,
						null,
						ResourceStrings.FactEditorToolWindowCaption,
						null,
						out windowFrame));

					ErrorHandler.ThrowOnFailure(windowFrame.SetProperty((int)__VSFPROPID.VSFPROPID_BitmapResource, 125));
					ErrorHandler.ThrowOnFailure(windowFrame.SetProperty((int)__VSFPROPID.VSFPROPID_BitmapIndex, 3));
					IVsTextView textView;
					ErrorHandler.ThrowOnFailure(codeWindow.GetPrimaryView(out textView));
					m_ToolWindow = windowFrame;
					m_ActivationManager = new ActivationManager(this, windowFrame, this.GetOrCreateSource(lines), textView);
				}
				return windowFrame;
			}
		}
		#endregion // ToolWindow creation
		#endregion // ToolWindow Creation and Activation
	}
}
