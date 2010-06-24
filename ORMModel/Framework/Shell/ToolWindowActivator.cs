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
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using ORMSolutions.ORMArchitect.Framework;
using Microsoft.VisualStudio;
using System.Diagnostics;
using System.ComponentModel.Design;

namespace ORMSolutions.ORMArchitect.Framework.Shell
{
	#region FrameVisibility enum
	/// <summary>
	/// Indicate the current visibility state for an <see cref="T:ToolWindowActivationHelper"/>.
	/// The values here reduce the myriad settings allowed by the <see cref="__FRAMESHOW"/> and
	/// <see cref="__FRAMESHOW2"/> enumerations into three easily actionable values and transitions.
	/// </summary>
	public enum FrameVisibility
	{
		/// <summary>
		/// The frame is not currently visible
		/// </summary>
		Hidden,
		/// <summary>
		/// The frame is currently visible and not fully covered by any other frame.
		/// </summary>
		Visible,
		/// <summary>
		/// A representation of the frame (a tab or icon) is shown, but the frame
		/// contents itself are not visible. A derived window can explicitly change
		/// a window from Covered to Hidden in response to requested change events,
		/// effectively deferring updates until the frame again transitions to Visible.
		/// </summary>
		Covered,
	}
	#endregion // FrameVisibility enum
	#region CoveredFrameContentActions enum
	/// <summary>
	/// Determine which actions to take automatically when selection changes
	/// are made while the <see cref="P:ToolWindowActivationHelper.CurrentFrameVisibility"/> property
	/// has a value of <see cref="FrameVisibility.Covered"/>. This value is returned
	/// from the virtual <see cref="P:ToolWindowActivationHelper.CoveredFrameContentActions"/> property,
	/// which is used by the <see cref="M:ToolWindowActivationHelper.CurrentSelectionContainerChanging"/>
	/// and <see cref="M:ToolWindowActivationHelper.CurrentDocumentChanging"/> methods. Most derived tool windows
	/// can customize behavior by overriding the property, but finer grained control is also available
	/// via the other overrides. Derived tool windows should explicitly call <see cref="M:ToolWindowActivationHelper.ClearContents"/>
	/// to transition from a covered to a lightweight hidden state.
	/// </summary>
	[Flags]
	public enum CoveredFrameContentActions
	{
		/// <summary>
		/// Do not take any action when selection changes while a toolwindow is completely
		/// covered by another Visual Studio window.
		/// </summary>
		None = 0,
		/// <summary>
		/// Automatically clear the contents of a tool window when the current
		/// document changes while a toolwindow is completely covered by another Visual Studio window.
		/// </summary>
		ClearContentsOnDocumentChanged,
		/// <summary>
		/// Automatically clear the contents of a tool window when the current
		/// selection changes while a toolwindow is completely covered by another Visual Studio window.
		/// </summary>
		ClearContentsOnSelectionChanged,
	}
	#endregion // CoveredFrameContentActions enum
	#region INotifyToolWindowActivation interface
	/// <summary>
	/// Callback interface provided by consumers of the <see cref="T:ToolWindowActivationHelper"/> class.
	/// </summary>
	[CLSCompliant(false)]
	public interface INotifyToolWindowActivation<DocDataType, DocViewType, SelectionContainerType>
		where DocDataType : ModelingDocData
		where DocViewType : class, IVsWindowPane
		where SelectionContainerType : class, ISelectionContainer, ISelectionService
	{
		/// <summary>
		/// The <see cref="T:ToolWindowActivationHelper"/> has detected a selection change
		/// </summary>
		/// <param name="activator">The corresponding <see cref="T:ToolWindowActivationHelper"/></param>
		void ActivatorSelectionContainerChanged(ToolWindowActivationHelper<DocDataType, DocViewType, SelectionContainerType> activator);
		/// <summary>
		/// The <see cref="T:ToolWindowActivationHelper"/> has detected a change in the active document
		/// </summary>
		/// <param name="activator">The corresponding <see cref="T:ToolWindowActivationHelper"/></param>
		void ActivatorDocumentChanged(ToolWindowActivationHelper<DocDataType, DocViewType, SelectionContainerType> activator);
		/// <summary>
		/// The <see cref="T:ToolWindowActivationHelper"/> has detected a change in the active document view
		/// </summary>
		/// <param name="activator">The corresponding <see cref="T:ToolWindowActivationHelper"/></param>
		void ActivatorDocumentViewChanged(ToolWindowActivationHelper<DocDataType, DocViewType, SelectionContainerType> activator);
		/// <summary>
		/// The <see cref="T:ToolWindowActivationHelper"/> has detected that events need to be
		/// attached to listen to changes in the <paramref name="docData"/>
		/// </summary>
		/// <param name="docData">The document to attach to</param>
		void ActivatorAttachEventHandlers(DocDataType docData);
		/// <summary>
		/// The <see cref="T:ToolWindowActivationHelper"/> has detected that events need to be
		/// detached to stop listening to changes in the <paramref name="docData"/>
		/// </summary>
		/// <param name="docData">The document to detach from</param>
		void ActivatorDetachEventHandlers(DocDataType docData);
		/// <summary>
		/// The <see cref="T:ToolWindowActivationHelper"/> has been hidden with notifications disabled
		/// and is now transitioning to a visible state.
		/// </summary>
		/// <param name="activator">The corresponding <see cref="T:ToolWindowActivationHelper"/></param>
		void ActivatorVisibleWindowSessionBeginning(ToolWindowActivationHelper<DocDataType, DocViewType, SelectionContainerType> activator);
	}
	#endregion // INotifyToolWindowActivation interface
	#region ICurrentFrameVisibility interface
	/// <summary>
	/// Provide the current frame visibility. Implement this interface
	/// on a selection container tool window to enable maintaining the tool
	/// window as a selection container even when it is hidden.
	/// </summary>
	public interface IProvideFrameVisibility
	{
		/// <summary>
		/// The current <see cref="FrameVisibility"/> for this container
		/// </summary>
		FrameVisibility CurrentFrameVisibility { get;}
	}
	#endregion // ICurrentFrameVisibility interface
	#region ToolWindowActivationHelper class
	/// <summary>
	/// Helper class to enable tool window implementations to track changes selection
	/// changes in documents of a specific type. Used with the <see cref="INotifyToolWindowActivation{DocDataType,DocViewType,SelectionContainerType}"/>
	/// interface.
	/// </summary>
	/// <typeparam name="DocDataType">The type of the <see cref="ModelingDocData"/> loaded by the designer
	/// tracked by this <see cref="T:ToolWindowActivationHelper"/>.</typeparam>
	/// <typeparam name="DocViewType">The type of the <see cref="IVsWindowPane"/> view displayed
	/// by the designer tracked by this <see cref="T:ToolWindowActivationHelper"/></typeparam>
	/// <typeparam name="SelectionContainerType">An interface indicating that a window that may or
	/// may not be the current document view provides selection for the <typeparamref name="DocDataType"/>.
	/// This type must derive from both <see cref="ISelectionContainer"/> and <see cref="ISelectionService"/></typeparam>
	[CLSCompliant(false)]
	public class ToolWindowActivationHelper<DocDataType, DocViewType, SelectionContainerType> : IVsWindowFrameNotify3
		where DocDataType : ModelingDocData
		where DocViewType : class, IVsWindowPane
		where SelectionContainerType : class, ISelectionContainer, ISelectionService
	{
		#region FrameVisibilityFlags enum
		/// <summary>
		/// Frame visibility flags, includes values from public FrameVisibility enum plus
		/// additional transitional data
		/// </summary>
		private enum FrameVisibilityFlags
		{
			Hidden = FrameVisibility.Hidden,
			Visible = FrameVisibility.Visible,
			Covered = FrameVisibility.Covered,
			FrameVisibilityMask = Hidden | Visible | Covered,
			PendingHiddenMeansCovered = 1 << 2,
			PendingHiddenMeansClosed = 1 << 3,
			// The rest of this is a hack because OnShow is getting called with a TabDeactivated/WinHidden
			// when a tab is dragged to a new docking location, but is not called again to reshow the
			// window when the drag is completed
			HasBeenVisible = 1 << 4,
			PersistentFlagsMask = HasBeenVisible,
		}
		#endregion // FrameVisibilityFlags enum
		#region Member Variables
		/// <summary>
		/// The most recently selected SelectionContainer that contains selectable elements
		/// </summary>
		private SelectionContainerType myCurrentSelectionContainer;
		/// <summary>
		/// The current document.
		/// </summary>
		private DocDataType myCurrentDocument;
		/// <summary>
		/// The current diagram view.
		/// </summary>
		private DocViewType myCurrentDocumentView;
		/// <summary>
		/// The service to monitor the selection
		/// </summary>
		private readonly IMonitorSelectionService myMonitorSelectionService;
		/// <summary>
		/// Current frame visibility state
		/// </summary>
		private FrameVisibilityFlags myFrameVisibility;
		/// <summary>
		/// Track the last frame mode. Hack because OnShow is not called enough.
		/// </summary>
		private VSFRAMEMODE myLastFrameMode;
		/// <summary>
		/// The frame provided to the constructor
		/// </summary>
		private IVsWindowFrame myFrame;
		/// <summary>
		/// The default covered frame actions, provided to the constructor
		/// </summary>
		private CoveredFrameContentActions myCoveredFrameActions;
		/// <summary>
		/// The notification callback, provided to the contructor
		/// </summary>
		private INotifyToolWindowActivation<DocDataType, DocViewType, SelectionContainerType> myNotifyCallback;
		#endregion // Member Variables
		#region Constructors
		/// <summary>
		/// Create a new <see cref="T:ToolWindowActivationHelper"/>
		/// </summary>
		/// <param name="unfilteredServiceProvider">An <see cref="IServiceProvider"/> that is not filtered by
		/// the containing toolwindow implementation.</param>
		/// <param name="frame">The <see cref="IVsWindowFrame"/> to provide notifications for</param>
		/// <param name="coveredFrameActions">The default <see cref="CoveredFrameContentActions"/></param>
		/// <param name="notifyCallback">Thee <see cref="T:INotifyToolWindowActivation"/> callback used to communicate
		/// with the consuming class</param>
		public ToolWindowActivationHelper(IServiceProvider unfilteredServiceProvider, IVsWindowFrame frame, CoveredFrameContentActions coveredFrameActions, INotifyToolWindowActivation<DocDataType, DocViewType, SelectionContainerType> notifyCallback)
		{
			if (unfilteredServiceProvider == null)
			{
				throw new ArgumentNullException("unfilteredServiceProvider");
			}
			if (frame == null)
			{
				throw new ArgumentNullException("frame");
			}
			if (notifyCallback == null)
			{
				throw new ArgumentNullException("notifyCallback");
			}
			myLastFrameMode = (VSFRAMEMODE)(-1);
			myMonitorSelectionService = (IMonitorSelectionService)unfilteredServiceProvider.GetService(typeof(IMonitorSelectionService));
			myFrame = frame;
			myCoveredFrameActions = coveredFrameActions;
			myNotifyCallback = notifyCallback;
			frame.SetProperty((int)__VSFPROPID.VSFPROPID_ViewHelper, this);
		}
		#endregion // Constructors
		#region Public methods
		/// <summary>
		/// Clear a covered tool window. Used to cover a window without clearing it,
		/// then clear it later if updates are not required.
		/// </summary>
		public void ClearIfCovered()
		{
			if (CurrentFrameVisibility == FrameVisibility.Covered)
			{
				ClearContents();
			}
		}
		#endregion // Public methods
		#region Properties for CurrentDocument and CurrentSelectionContainer
		/// <summary>
		/// Get the current <typeparamref name="DocDataType"/>
		/// </summary>
		public DocDataType CurrentDocument
		{
			get
			{
				return myCurrentDocument;
			}
		}
		/// <summary>
		/// Sets the current <typeparamref name="DocDataType"/> and <typeparamref name="DocViewType"/>.
		/// </summary>
		/// <param name="docData">The doc data.</param>
		/// <param name="docView">The doc view.</param>
		private void SetCurrentDocument(DocDataType docData, DocViewType docView)
		{
			DocDataType oldDocData = myCurrentDocument;
			if (oldDocData == docData)
			{
				if (myCurrentDocumentView != docView)
				{
					myCurrentDocumentView = docView;
					myNotifyCallback.ActivatorDocumentViewChanged(this);
				}
				return;
			}
			if (oldDocData != null)	// If the current document is not null
			{
				// If we get to this point, we know that the document window
				// has really changed, so we need to unwire the event handlers
				// from the model store.
				oldDocData.DocumentReloading -= new EventHandler(DocumentReloading);
				myNotifyCallback.ActivatorDetachEventHandlers(oldDocData);
			}
			myCurrentDocument = docData;
			myCurrentDocumentView = docView;
			if (docData != null)	// If the new DocData is actually a DocDataType,
			{
				Store newStore = docData.Store;
				if (newStore != null && !newStore.Disposed)
				{
					docData.DocumentReloading += new EventHandler(DocumentReloading);
					myNotifyCallback.ActivatorAttachEventHandlers(docData);	// wire the event handlers to the model store.
				}
				else
				{
					myCurrentDocumentView = null;
					myCurrentDocument = null;
				}
			}
			else
			{
				SelectionContainerType container = myCurrentSelectionContainer;
				if (container != null)
				{
					container.SelectionChanged -= new EventHandler(IntraContainerSelectionChanged);
				}
				myCurrentSelectionContainer = null;
				myCurrentDocumentView = null;
				myNotifyCallback.ActivatorSelectionContainerChanged(this);
			}
			myNotifyCallback.ActivatorDocumentChanged(this);
		}
		private void DocumentReloading(object sender, EventArgs e)
		{
			DocDataType docData;
			if (null != (docData = sender as DocDataType))
			{
				docData.DocumentReloading -= new EventHandler(DocumentReloading);
				myNotifyCallback.ActivatorDetachEventHandlers(docData);
				docData.DocumentReloaded += new EventHandler(DocumentReloaded);
			}
		}
		private void DocumentReloaded(object sender, EventArgs e)
		{
			DocDataType docData;
			if (null != (docData = sender as DocDataType))
			{
				docData.DocumentReloaded -= new EventHandler(DocumentReloaded);
				docData.DocumentReloading += new EventHandler(DocumentReloading);
				myNotifyCallback.ActivatorAttachEventHandlers(docData);
			}
		}
		/// <summary>
		/// Get the current <typeparamref name="DocViewType"/>. This will be null if CurrentDocument is null.
		/// </summary>
		public DocViewType CurrentDocumentView
		{
			get
			{
				return myCurrentDocumentView;
			}
		}
		/// <summary>
		/// Get the current <typeparamref name="SelectionContainerType"/>. Tracking this separate
		/// from CurrentDocument allows us to switch between multiple
		/// different tool windows without changing the current document.
		/// </summary>
		public SelectionContainerType CurrentSelectionContainer
		{
			get
			{
				SelectionContainerType cachedContainer = myCurrentSelectionContainer;
				SelectionContainerType currentContainer;
				if (cachedContainer != null &&
					null != (currentContainer = myMonitorSelectionService.CurrentSelectionContainer as SelectionContainerType))
				{
					// We do not trust the cached value for the current selection container because
					// it can cause the IORMSelectionContainer mechanism to go into an infinite loop
					// when the selection is switching between two containers. The cached value
					// will eventually change to match when the notification is received by this window.
					return currentContainer;
				}
				return cachedContainer;
			}
			private set
			{
				if (value != null)
				{
					if (!CurrentSelectionContainerChanging(value))
					{
						SelectionContainerType previousSelectionContainer = myCurrentSelectionContainer;
						if (previousSelectionContainer != value)
						{
							if (previousSelectionContainer != null)
							{
								previousSelectionContainer.SelectionChanged -= IntraContainerSelectionChanged;
							}
							myCurrentSelectionContainer = value;
							myNotifyCallback.ActivatorSelectionContainerChanged(this);
							value.SelectionChanged += new EventHandler(IntraContainerSelectionChanged);
						}
					}
				}
			}
		}
		private void IntraContainerSelectionChanged(object sender, EventArgs e)
		{
			if (sender == myCurrentSelectionContainer)
			{
				if (!CurrentSelectionContainerChanging(myCurrentSelectionContainer))
				{
					myNotifyCallback.ActivatorSelectionContainerChanged(this);
				}
			}
		}
		/// <summary>
		/// Called when the selection container is changed. Returning
		/// <see langword="true"/> from this method will block the <see cref="M:INotifyToolWindowActivation.ActivatorSelectionContainerChanged"/>
		/// notification. The default behavior is to clear the window contents if the
		/// tool windows <see cref="M:FrameVisibility"/>  property is currently <see cref="F:FrameVisibility.Covered"/>
		/// </summary>
		/// <returns><see langword="false"/> to continue with selection change, <see langword="true"/> to block.</returns>
		protected virtual bool CurrentSelectionContainerChanging(SelectionContainerType newContainer)
		{
			if (CurrentFrameVisibility != FrameVisibility.Visible &&
				0 != (CoveredFrameContentActions & CoveredFrameContentActions.ClearContentsOnSelectionChanged))
			{
				ClearContents();
				return true;
			}
			return false;
		}
		#endregion // Properties for CurrentDocument and CurrentSelectionContainer
		#region Window state notification changes
		/// <summary>
		/// Return the actions to take when selection changes while
		/// a frame is covered. Default action is <see cref="F:CoveredFrameContentActions.ClearContentsOnDocumentChanged"/>
		/// </summary>
		protected virtual CoveredFrameContentActions CoveredFrameContentActions
		{
			get
			{
				return myCoveredFrameActions;
			}
		}
		/// <summary>
		/// The current <see cref="T:FrameVisibility"/>
		/// </summary>
		public FrameVisibility CurrentFrameVisibility
		{
			get
			{
				return (FrameVisibility)(myFrameVisibility & FrameVisibilityFlags.FrameVisibilityMask);
			}
		}
		/// <summary>
		/// Clear the contents of the tool window associated with this <see cref="T:ToolWindowActivationHelper"/>
		/// </summary>
		protected virtual void ClearContents()
		{
			FrameVisibilityFlags flags = myFrameVisibility;
			switch (flags & FrameVisibilityFlags.FrameVisibilityMask)
			{
				case FrameVisibilityFlags.Covered:
				case FrameVisibilityFlags.Visible:
					IMonitorSelectionService monitor = myMonitorSelectionService;
					monitor.SelectionChanged -= new EventHandler<MonitorSelectionEventArgs>(MonitorSelectionChanged);
					monitor.DocumentWindowChanged -= new EventHandler<MonitorSelectionEventArgs>(DocumentWindowChanged);
					myFrameVisibility = FrameVisibilityFlags.Hidden | (flags & FrameVisibilityFlags.PersistentFlagsMask);
					SetCurrentDocument(null, null);
					break;
			}
		}
		private void ShowContents()
		{
			FrameVisibilityFlags flags = myFrameVisibility;
			switch (flags & FrameVisibilityFlags.FrameVisibilityMask)
			{
				case FrameVisibilityFlags.Covered:
					myFrameVisibility = FrameVisibilityFlags.Visible | (flags & FrameVisibilityFlags.PersistentFlagsMask) | FrameVisibilityFlags.HasBeenVisible;
					break;
				case FrameVisibilityFlags.Hidden:
					IMonitorSelectionService monitor = myMonitorSelectionService;
					monitor.SelectionChanged += new EventHandler<MonitorSelectionEventArgs>(MonitorSelectionChanged);
					monitor.DocumentWindowChanged += new EventHandler<MonitorSelectionEventArgs>(DocumentWindowChanged);
					myNotifyCallback.ActivatorVisibleWindowSessionBeginning(this);
					SetCurrentDocument(SafeGetCurrentDocument(monitor) as DocDataType, monitor.CurrentDocumentView as DocViewType);
					myFrameVisibility = FrameVisibilityFlags.Visible | (flags & FrameVisibilityFlags.PersistentFlagsMask) | FrameVisibilityFlags.HasBeenVisible;
					CurrentSelectionContainer = monitor.CurrentSelectionContainer as SelectionContainerType ?? monitor.CurrentDocumentView as SelectionContainerType;
					break;
			}
		}
		/// <summary>
		/// Helper method to prevent the debugger from breaking when a common
		/// exception is through retrieving the CurrentDocument from a <see cref="IMonitorSelectionService"/>
		/// </summary>
		[DebuggerStepThrough]
		private static object SafeGetCurrentDocument(IMonitorSelectionService monitor)
		{
			object retVal = null;
			try
			{
				retVal = monitor.CurrentDocument;
			}
			catch (System.Runtime.InteropServices.COMException)
			{
				// Swallow, this will occasionally be initialized when the document is shutting down
			}
			return retVal;
		}
		#region Other notifications, check for mode change
		/// <summary>
		/// Implements <see cref="IVsWindowFrameNotify3.OnDockableChange"/>
		/// </summary>
		protected int OnDockableChange(int fDockable, int x, int y, int w, int h)
		{
			HandlePossibleFrameModeChange();
			return VSConstants.S_OK;
		}
		int IVsWindowFrameNotify3.OnDockableChange(int fDockable, int x, int y, int w, int h)
		{
			return OnDockableChange(fDockable, x, y, w, h);
		}
		/// <summary>
		/// Implements <see cref="IVsWindowFrameNotify3.OnMove"/>
		/// </summary>
		protected int OnMove(int x, int y, int w, int h)
		{
			HandlePossibleFrameModeChange();
			return VSConstants.S_OK;
		}
		private void HandlePossibleFrameModeChange()
		{
			object frameModeObj;
			VSFRAMEMODE frameMode;
			if (VSConstants.S_OK == myFrame.GetProperty((int)__VSFPROPID.VSFPROPID_FrameMode, out frameModeObj) &&
				myLastFrameMode != (frameMode = (VSFRAMEMODE)frameModeObj))
			{
				myLastFrameMode = frameMode;
				FrameVisibilityFlags flags = myFrameVisibility;
				if ((flags & FrameVisibilityFlags.FrameVisibilityMask) == FrameVisibilityFlags.Covered &&
					0 != (flags & FrameVisibilityFlags.HasBeenVisible))
				{
					OnShow((int)__FRAMESHOW.FRAMESHOW_WinShown);
				}
			}
		}
		int IVsWindowFrameNotify3.OnMove(int x, int y, int w, int h)
		{
			return OnMove(x, y, w, h);
		}
		/// <summary>
		/// Implements <see cref="IVsWindowFrameNotify3.OnSize"/>
		/// </summary>
		protected static int OnSize(int x, int y, int w, int h)
		{
			return VSConstants.S_OK;
		}
		int IVsWindowFrameNotify3.OnSize(int x, int y, int w, int h)
		{
			return OnSize(x, y, w, h);
		}
		#endregion // Other notifications, check for mode change
		/// <summary>
		/// Implements <see cref="IVsWindowFrameNotify3.OnClose"/>
		/// </summary>
		protected int OnFrameClose(ref uint pgrfSaveOptions)
		{
			ClearContents();
			return VSConstants.S_OK;
		}
		int IVsWindowFrameNotify3.OnClose(ref uint pgrfSaveOptions)
		{
			return OnFrameClose(ref pgrfSaveOptions);
		}
		/// <summary>
		/// Implements <see cref="IVsWindowFrameNotify3.OnShow"/>
		/// </summary>
		protected int OnShow(int fShow)
		{
			FrameVisibilityFlags flags = myFrameVisibility;
			FrameVisibilityFlags startFlags = flags & ~(FrameVisibilityFlags.FrameVisibilityMask | FrameVisibilityFlags.PersistentFlagsMask);
			bool coverPending = 0 != (flags & FrameVisibilityFlags.PendingHiddenMeansCovered);
			bool closePending = !coverPending && 0 != (flags & FrameVisibilityFlags.PendingHiddenMeansCovered);
			myFrameVisibility &= FrameVisibilityFlags.FrameVisibilityMask | FrameVisibilityFlags.PersistentFlagsMask;
			switch ((__FRAMESHOW)fShow)
			{
				case (__FRAMESHOW)__FRAMESHOW2.FRAMESHOW_BeforeWinHidden:
					myFrameVisibility |= FrameVisibilityFlags.PendingHiddenMeansClosed;
					break;
				case __FRAMESHOW.FRAMESHOW_WinMinimized:
				case __FRAMESHOW.FRAMESHOW_TabDeactivated:
					myFrameVisibility |= FrameVisibilityFlags.PendingHiddenMeansCovered;
					break;
				case __FRAMESHOW.FRAMESHOW_DestroyMultInst:
				case __FRAMESHOW.FRAMESHOW_WinClosed:
					ClearContents();
					break;
				case __FRAMESHOW.FRAMESHOW_WinHidden:
					bool cover = false;
					object frameModeObj;
					VSFRAMEMODE frameMode = (VSFRAMEMODE)(-1);
					IVsWindowFrame frame = myFrame;
					if (frame != null &&
						VSConstants.S_OK == frame.GetProperty((int)__VSFPROPID.VSFPROPID_FrameMode, out frameModeObj))
					{
						// VS is changing the framemode during a hide request without telling us, always check and reset
						// at this point so that a move on a hidden window does not reshow it.
						myLastFrameMode = frameMode = (VSFRAMEMODE)frameModeObj;
					}
					if (0 != (startFlags & FrameVisibilityFlags.PendingHiddenMeansCovered))
					{
						cover = true;
					}
					else if (0 == (startFlags & FrameVisibilityFlags.PendingHiddenMeansClosed))
					{
						cover = frameMode == VSFRAMEMODE.VSFM_MdiChild;
					}
					if (cover)
					{
						myFrameVisibility = FrameVisibilityFlags.Covered | (flags & FrameVisibilityFlags.PersistentFlagsMask);
					}
					else
					{
						ClearContents();
					}
					break;
				case __FRAMESHOW.FRAMESHOW_AutoHideSlideBegin:
				case __FRAMESHOW.FRAMESHOW_WinMaximized:
				case __FRAMESHOW.FRAMESHOW_WinRestored:
				case __FRAMESHOW.FRAMESHOW_WinShown:
					ShowContents();
					break;
			}
			return VSConstants.S_OK;
		}
		int IVsWindowFrameNotify3.OnShow(int fShow)
		{
			return OnShow(fShow);
		}
		#endregion // Window state notification changes
		#region IMonitorSelectionService Event Handlers
		/// <summary>
		/// Handles the SelectionChanged event on the IMonitorSelectionService
		/// </summary>
		private void MonitorSelectionChanged(object sender, MonitorSelectionEventArgs e)
		{
			IMonitorSelectionService monitor = (IMonitorSelectionService)sender;
			SelectionContainerType newContainer = monitor.CurrentSelectionContainer as SelectionContainerType;
			if (newContainer == null)
			{
				IProvideFrameVisibility visibility = myCurrentSelectionContainer as IProvideFrameVisibility;
				if (visibility == null || visibility.CurrentFrameVisibility == FrameVisibility.Hidden)
				{
					newContainer = monitor.CurrentDocumentView as SelectionContainerType;
				}
			}
			CurrentSelectionContainer = newContainer;
		}
		/// <summary>
		/// Handles the DocumentWindowChanged event on the IMonitorSelectionService
		/// </summary>
		private void DocumentWindowChanged(object sender, MonitorSelectionEventArgs e)
		{
			IMonitorSelectionService monitor = (IMonitorSelectionService)sender;
			DocDataType docData = monitor.CurrentDocument as DocDataType;
			DocViewType docView = monitor.CurrentDocumentView as DocViewType;
			if (!CurrentDocumentChanging(docData, docView))
			{
				SetCurrentDocument(docData, docView);
			}
		}
		/// <summary>
		/// Called when the current document is changed. Returning
		/// <see langword="true"/> will force the current document to be cleared.
		/// The default behavior is to clear the window contents if the
		/// tool windows <see cref="M:FrameVisibility"/>  property is currently <see cref="F:FrameVisibility.Covered"/>
		/// </summary>
		/// <returns><see langword="false"/> to continue with selection change, <see langword="true"/> to block.</returns>
		protected virtual bool CurrentDocumentChanging(DocDataType docData, DocViewType docView)
		{
			if (CurrentFrameVisibility != FrameVisibility.Visible &&
				0 != (CoveredFrameContentActions & CoveredFrameContentActions.ClearContentsOnDocumentChanged))
			{
				ClearContents();
				return true;
			}
			return false;
		}
		#endregion // IMonitorSelectionService Event Handlers
	}
	#endregion // ToolWindowActivationHelper class
}
