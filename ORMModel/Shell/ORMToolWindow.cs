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
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Neumont.Tools.Modeling;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio;
using System.Diagnostics;

namespace Neumont.Tools.ORM.Shell
{
	#region FrameVisibility enum
	/// <summary>
	/// Indicate the current visibility state for an <see cref="ORMToolWindow"/>.
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
	/// are made while the <see cref="ORMToolWindow.FrameVisibility"/> property
	/// has a value of <see cref="FrameVisibility.Covered"/>. This value is returned
	/// from the virtual <see cref="ORMToolWindow.CoveredFrameContentActions"/> property,
	/// which is used by the <see cref="ORMToolWindow.CurrentORMSelectionContainerChanging"/>
	/// and <see cref="ORMToolWindow.CurrentDocumentChanging"/> methods. Most derived tool windows
	/// can customize behavior by overriding the property, but finer grained control is also available
	/// via the other overrides. Derived tool windows should explicitly call <see cref="ORMToolWindow.ClearContents"/>
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
	/// <summary>
	/// Provides common functionality for all ORM tool windows.  Implements ToolWindow for WindowTitle functionality,
	/// declares abstract methods for attaching and detaching event handlers, and handles the logic for switching
	/// between ORM documents and non-ORM documents, different tool windows, etc.
	/// </summary>
	[CLSCompliant(false)]
	public abstract class ORMToolWindow : ToolWindow, IVsWindowFrameNotify3
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
		/// The most recently selected SelectionContainer that contains selectable
		/// ORM ModelElements.
		/// </summary>
		private IORMSelectionContainer myCurrentORMSelectionContainer;
		/// <summary>
		/// The current ORM document.
		/// </summary>
		private ORMDesignerDocData myCurrentDocument;
		/// <summary>
		/// The current diagram docview.
		/// </summary>
		private DiagramDocView myCurrentDocumentView;
		/// <summary>
		/// The service provider passed to the constructor. The base class messes with this.
		/// </summary>
		private readonly IServiceProvider myCtorServiceProvider;
		/// <summary>
		/// Current frame visibility state
		/// </summary>
		private FrameVisibilityFlags myFrameVisibility;
		/// <summary>
		/// Track the last frame mode. Hack because OnShow is not called enough.
		/// </summary>
		private VSFRAMEMODE myLastFrameMode;
		#endregion // Member Variables
		#region Properties for CurrentDocument and CurrentORMSelectionContainer
		/// <summary>
		/// Get the current ORMDesignerDocData
		/// </summary>
		protected ORMDesignerDocData CurrentDocument
		{
			get
			{
				return myCurrentDocument;
			}
		}
		/// <summary>
		/// Sets the current ORMDesignerDocData and DiagramDocView.
		/// </summary>
		/// <param name="docData">The doc data.</param>
		/// <param name="docView">The doc view.</param>
		private void SetCurrentDocument(ORMDesignerDocData docData, DiagramDocView docView)
		{
			ORMDesignerDocData oldDocData = myCurrentDocument;
			if (oldDocData == docData)
			{
				if (myCurrentDocumentView != docView)
				{
					myCurrentDocumentView = docView;
					OnCurrentDocumentViewChanged();
				}
				return;
			}
			if (oldDocData != null)	// If the current document is not null
			{
				// If we get to this point, we know that the document window
				// has really changed, so we need to unwire the event handlers
				// from the model store.
				DetachEventHandlers(oldDocData);
			}
			myCurrentDocument = docData;
			myCurrentDocumentView = docView;
			if (docData != null)	// If the new DocData is actually an ORMDesignerDocData,
			{
				Store newStore = docData.Store;
				if (newStore != null && !newStore.Disposed)
				{
					AttachEventHandlers(docData);	// wire the event handlers to the model store.
				}
				else
				{
					myCurrentDocumentView = null;
					myCurrentDocument = null;
				}
			}
			else
			{
				myCurrentORMSelectionContainer = null;
				myCurrentDocumentView = null;
				OnORMSelectionContainerChanged();
			}
			OnCurrentDocumentChanged();
		}
		private void DocumentReloading(object sender, EventArgs e)
		{
			ORMDesignerDocData docData;
			if (null != (docData  = sender as ORMDesignerDocData))
			{
				DetachEventHandlers(docData);
				docData.DocumentReloaded += new EventHandler(DocumentReloaded);
			}
		}
		private void DocumentReloaded(object sender, EventArgs e)
		{
			ORMDesignerDocData docData;
			if (null != (docData = sender as ORMDesignerDocData))
			{
				docData.DocumentReloaded -= new EventHandler(DocumentReloaded);
				AttachEventHandlers(docData);
			}
		}
		/// <summary>
		/// Get the current DiagramDocView. This will be null if CurrentDocument is null.
		/// </summary>
		protected DiagramDocView CurrentDocumentView
		{
			get
			{
				return myCurrentDocumentView;
			}
		}
		/// <summary>
		/// Provide a notification when the selection container has been modified. The
		/// default implemention is empty.
		/// </summary>
		protected virtual void OnCurrentDocumentChanged() { }
		/// <summary>
		/// Provide a notification when the current document view has been modified.
		/// This will be called only if the current document is not modified at the same time.
		/// </summary>
		protected virtual void OnCurrentDocumentViewChanged() { }
		/// <summary>
		/// Get the current IORMSelectionContainer. Tracking this separate
		/// from CurrentDocument allows us to switch between multiple
		/// different ORM tool windows without changing the current ORM document.
		/// </summary>
		protected IORMSelectionContainer CurrentORMSelectionContainer
		{
			get
			{
				return myCurrentORMSelectionContainer;
			}
			private set
			{
				if (value != null)
				{
					if (!CurrentORMSelectionContainerChanging(value))
					{
						myCurrentORMSelectionContainer = value;
						OnORMSelectionContainerChanged();
					}
				}
			}
		}
		/// <summary>
		/// Called when the selection container is changed. Returning
		/// <see langword="true"/> from this method will block the <see cref="OnORMSelectionContainerChanged"/>
		/// notification. The default behavior is to clear the window contents if the
		/// tool windows <see cref="M:FrameVisibility"/>  property is currently <see cref="F:FrameVisibility.Covered"/>
		/// </summary>
		/// <returns><see langword="false"/> to continue with selection change, <see langword="true"/> to block.</returns>
		protected virtual bool CurrentORMSelectionContainerChanging(IORMSelectionContainer newContainer)
		{
			if (FrameVisibility == FrameVisibility.Covered &&
				0 != (CoveredFrameContentActions & CoveredFrameContentActions.ClearContentsOnSelectionChanged))
			{
				ClearContents();
				return true;
			}
			return false;
		}
		/// <summary>
		/// Provide a notification when the selection container has been modified. The
		/// default implemention is empty.
		/// </summary>
		protected virtual void OnORMSelectionContainerChanged() { }
		#endregion // Properties for CurrentDocument and CurrentORMSelectionContainer
		#region ORMToolWindow Constructor
		/// <summary>
		/// Constructs a new ORM tool window. Initialization is performed in the Initialize method.
		/// </summary>
		protected ORMToolWindow(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			myLastFrameMode = (VSFRAMEMODE)(-1);
			myCtorServiceProvider = serviceProvider;
		}
		/// <summary>
		/// The <see cref="IServiceProvider"/> instance passed to the constructor.
		/// Provides services without filtering by the tool window.
		/// </summary>
		protected IServiceProvider ExternalServiceProvider
		{
			get
			{
				return myCtorServiceProvider;
			}
		}
		/// <summary>
		/// Constructs a new ORM tool window, wires selection and document changed events,
		/// and initializes the CurrentModelElementSelectionContainer to the current DocView.
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();
			Frame.SetProperty((int)__VSFPROPID.VSFPROPID_ViewHelper, this);
		}
		#endregion // ORMToolWindow Constructor
		#region Window state notification changes
		/// <summary>
		/// Return the actions to take when selection changes while
		/// a frame is covered. Default action is <see cref="F:CoveredFrameContentActions.ClearContentsOnDocumentChanged"/>
		/// </summary>
		protected virtual CoveredFrameContentActions CoveredFrameContentActions
		{
			get
			{
				return CoveredFrameContentActions.ClearContentsOnDocumentChanged;
			}
		}
		/// <summary>
		/// The current <see cref="T:FrameVisibility"/>
		/// </summary>
		public FrameVisibility FrameVisibility
		{
			get
			{
				return (FrameVisibility)(myFrameVisibility & FrameVisibilityFlags.FrameVisibilityMask);
			}
		}
		/// <summary>
		/// Clear the contents of this <see cref="ORMToolWindow"/>
		/// </summary>
		protected virtual void ClearContents()
		{
			FrameVisibilityFlags flags = myFrameVisibility;
			switch (flags & FrameVisibilityFlags.FrameVisibilityMask)
			{
				case FrameVisibilityFlags.Covered:
				case FrameVisibilityFlags.Visible:
					IMonitorSelectionService monitor = (IMonitorSelectionService)myCtorServiceProvider.GetService(typeof(IMonitorSelectionService));
					monitor.SelectionChanged -= new EventHandler<MonitorSelectionEventArgs>(MonitorSelectionChanged);
					monitor.DocumentWindowChanged -= new EventHandler<MonitorSelectionEventArgs>(DocumentWindowChanged);
					myFrameVisibility = FrameVisibilityFlags.Hidden | (flags & FrameVisibilityFlags.PersistentFlagsMask);
					SetCurrentDocument(null, null);
					CurrentORMSelectionContainer = null;
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
					IMonitorSelectionService monitor = (IMonitorSelectionService)myCtorServiceProvider.GetService(typeof(IMonitorSelectionService));
					monitor.SelectionChanged += new EventHandler<MonitorSelectionEventArgs>(MonitorSelectionChanged);
					monitor.DocumentWindowChanged += new EventHandler<MonitorSelectionEventArgs>(DocumentWindowChanged);
					SetCurrentDocument(SafeGetCurrentDocument(monitor) as ORMDesignerDocData, monitor.CurrentDocumentView as DiagramDocView);
					CurrentORMSelectionContainer = monitor.CurrentSelectionContainer as IORMSelectionContainer;
					myFrameVisibility = FrameVisibilityFlags.Visible | (flags & FrameVisibilityFlags.PersistentFlagsMask) | FrameVisibilityFlags.HasBeenVisible;
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
		#region Other notifications we don't care about
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
			if (VSConstants.S_OK == Frame.GetProperty((int)__VSFPROPID.VSFPROPID_FrameMode, out frameModeObj) &&
				myLastFrameMode != (frameMode = (VSFRAMEMODE)frameModeObj))
			{
				myLastFrameMode = frameMode;
				FrameVisibilityFlags flags = myFrameVisibility;
				if ((flags & FrameVisibilityFlags.FrameVisibilityMask) != FrameVisibilityFlags.Visible &&
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
		#endregion // Other notifications we don't care about
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
					if (0 != (startFlags & FrameVisibilityFlags.PendingHiddenMeansCovered))
					{
						cover = true;
					}
					else if (0 == (startFlags & FrameVisibilityFlags.PendingHiddenMeansClosed))
					{
						object frameMode;
						IVsWindowFrame frame = Frame;
						if (frame != null &&
							VSConstants.S_OK == frame.GetProperty((int)__VSFPROPID.VSFPROPID_FrameMode, out frameMode))
						{
							cover = (VSFRAMEMODE)frameMode == VSFRAMEMODE.VSFM_MdiChild;
						}
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
			CurrentORMSelectionContainer = ((IMonitorSelectionService)sender).CurrentSelectionContainer as IORMSelectionContainer;
		}
		/// <summary>
		/// Handles the DocumentWindowChanged event on the IMonitorSelectionService
		/// </summary>
		private void DocumentWindowChanged(object sender, MonitorSelectionEventArgs e)
		{
			IMonitorSelectionService monitor = (IMonitorSelectionService)sender;
			ORMDesignerDocData docData = monitor.CurrentDocument as ORMDesignerDocData;
			DiagramDocView docView = monitor.CurrentDocumentView as DiagramDocView;
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
		protected virtual bool CurrentDocumentChanging(ORMDesignerDocData docData, DiagramDocView docView)
		{
			if (FrameVisibility == FrameVisibility.Covered &&
				0 != (CoveredFrameContentActions & CoveredFrameContentActions.ClearContentsOnDocumentChanged))
			{
				ClearContents();
				return true;
			}
			return false;
		}
		#endregion // IMonitorSelectionService Event Handlers
		#region Abstract Methods and Properties
		/// <summary>
		/// Attaches custom <see cref="EventHandler{TEventArgs}"/>s to the <see cref="Store"/>.  This method must be overridden.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="eventManager">The <see cref="ModelingEventManager"/> used to manage the <see cref="EventHandler{TEventArgs}"/>s.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		protected abstract void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action);
		/// <summary>
		/// Gets the string that should be displayed in the title bar of the tool window.
		/// </summary>
		public abstract override string WindowTitle { get; }
		/// <summary>
		/// See <see cref="ToolWindow.BitmapResource"/>.
		/// </summary>
		/// <remarks>
		/// Force subclasses to implement <see cref="ToolWindow.BitmapResource"/>.
		/// </remarks>
		protected abstract override int BitmapResource { get; }
		/// <summary>
		/// See <see cref="ToolWindow.BitmapIndex"/>.
		/// </summary>
		/// <remarks>
		/// Force subclasses to implement <see cref="ToolWindow.BitmapIndex"/>.
		/// </remarks>
		protected abstract override int BitmapIndex { get; }
		#endregion // Abstract Methods and Properties
		#region ISelectionContainer overrides
		/// <summary>
		/// Counts the number of elements in the current selection.
		/// Defers to <see cref="myCurrentORMSelectionContainer"/>.
		/// </summary>
		/// <remarks>
		/// See <see cref="ModelingWindowPane.CountSelectedObjects"/> and <see cref="ISelectionContainer.CountObjects"/>.
		/// </remarks>
		protected override uint CountSelectedObjects()
		{
			uint retVal = 0;
			IORMSelectionContainer container = myCurrentORMSelectionContainer;
			if (container != null)
			{
				if (container == this)
				{
					retVal = base.CountSelectedObjects();
				}
				else
				{
					container.CountObjects((uint)Constants.GETOBJS_SELECTED, out retVal);
				}
			}
			return retVal;
		}
		/// <summary>
		/// Counts the number of elements in the <see cref="ModelingWindowPane"/>.
		/// Defers to <see cref="myCurrentORMSelectionContainer"/>.
		/// </summary>
		/// <remarks>
		/// See <see cref="ModelingWindowPane.CountAllObjects"/> and <see cref="ISelectionContainer.CountObjects"/>.
		/// </remarks>
		protected override uint CountAllObjects()
		{
			uint retVal = 0;
			IORMSelectionContainer container = myCurrentORMSelectionContainer;
			if (container != null)
			{
				if (container == this)
				{
					retVal = base.CountAllObjects();
				}
				else
				{
					container.CountObjects((uint)Constants.GETOBJS_ALL, out retVal);
				}
			}
			return retVal;
		}
		/// <summary>
		/// Gets a read-only collection of currently selected elements in the <see cref="ModelingWindowPane"/>.
		/// Defers to <see cref="myCurrentORMSelectionContainer"/>.
		/// </summary>
		/// <remarks>
		/// See <see cref="ModelingWindowPane.GetSelectedComponents"/> and
		/// <see cref="System.ComponentModel.Design.ISelectionService.GetSelectedComponents"/>.
		/// </remarks>
		public override ICollection GetSelectedComponents()
		{
			IORMSelectionContainer container = myCurrentORMSelectionContainer;
			if (container != null)
			{
				return (container == this) ? base.GetSelectedComponents() : container.GetSelectedComponents();
			}
			return null;
		}
		/// <summary>
		/// Returns the elements that are currently selected in the <see cref="ModelingWindowPane"/>.
		/// Defers to <see cref="myCurrentORMSelectionContainer"/>.
		/// </summary>
		/// <remarks>
		/// See <see cref="ModelingWindowPane.GetSelectedObjects"/> and <see cref="ISelectionContainer.GetObjects"/>.
		/// </remarks>
		protected override void GetSelectedObjects(uint count, object[] objects)
		{
			IORMSelectionContainer container = myCurrentORMSelectionContainer;
			if (container != null)
			{
				if (container == this)
				{
					base.GetSelectedObjects(count, objects);
				}
				else
				{
					container.GetObjects((uint)Constants.GETOBJS_SELECTED, count, objects);
				}
			}
		}
		/// <summary>
		/// Gets all elements in the <see cref="ModelingWindowPane"/>.
		/// Defers to <see cref="myCurrentORMSelectionContainer"/>.
		/// </summary>
		/// <remarks>
		/// See <see cref="ModelingWindowPane.GetAllObjects"/> and <see cref="ISelectionContainer.GetObjects"/>.
		/// </remarks>
		protected override void GetAllObjects(uint count, object[] objects)
		{
			IORMSelectionContainer container = myCurrentORMSelectionContainer;
			if (container != null)
			{
				if (container == this)
				{
					base.GetAllObjects(count, objects);
				}
				else
				{
					container.GetObjects((uint)Constants.GETOBJS_ALL, count, objects);
				}
			}
		}
		/// <summary>
		/// Selects elements in the <see cref="ModelingWindowPane"/>.
		/// Defers to <see cref="myCurrentORMSelectionContainer"/>.
		/// </summary>
		/// <remarks>
		/// See <see cref="ModelingWindowPane.DoSelectObjects"/> and <see cref="ISelectionContainer.SelectObjects"/>.
		/// </remarks>
		protected override void DoSelectObjects(uint count, object[] objects, uint flags)
		{
			IORMSelectionContainer container = myCurrentORMSelectionContainer;
			if (container != null)
			{
				if (container == this)
				{
					// UNDONE: The base implementation is empty. Get an easily
					// supported way of requested model selection.
					base.DoSelectObjects(count, objects, flags);
				}
				else
				{
					container.SelectObjects(count, objects, flags);
				}
			}
		}
		#endregion // ISelectionContainer overrides
		#region ORMToolWindow specific
		/// <summary>
		/// Attach <see cref="EventHandler{TEventArgs}"/>s to the <see cref="Store"/> associated with the <see cref="ORMDesignerDocData"/>. Defers to <see cref="ManageEventHandlers"/>.
		/// </summary>
		protected void AttachEventHandlers(ORMDesignerDocData docData)
		{
			docData.DocumentReloading += new EventHandler(DocumentReloading);
			Store store = docData.Store;
			if (null != store && !store.Disposed)
			{
				ManageEventHandlers(store, ModelingEventManager.GetModelingEventManager(store), EventHandlerAction.Add);
			}
		}
		/// <summary>
		/// Detach <see cref="EventHandler{TEventArgs}"/>s from the <see cref="Store"/> associated with the <see cref="ORMDesignerDocData"/>. Defers to <see cref="ManageEventHandlers"/>.
		/// </summary>
		protected void DetachEventHandlers(ORMDesignerDocData docData)
		{
			docData.DocumentReloading -= new EventHandler(DocumentReloading);
			Store store = docData.Store;
			if (store != null && !store.Disposed)
			{
				ManageEventHandlers(store, ModelingEventManager.GetModelingEventManager(store), EventHandlerAction.Remove);
			}
		}
		#endregion // ORMToolWindow specific
	}
}
