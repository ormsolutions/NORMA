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
using Neumont.Tools.Modeling.Shell;
using Neumont.Tools.ORM.ObjectModel;
using Microsoft.VisualStudio;
using System.Diagnostics;

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// Provides common functionality for all ORM tool windows.  Implements ToolWindow for WindowTitle functionality,
	/// declares abstract methods for attaching and detaching event handlers, and handles the logic for switching
	/// between ORM documents and non-ORM documents, different tool windows, etc.
	/// </summary>
	[CLSCompliant(false)]
	public abstract class ORMToolWindow : ToolWindow, INotifyToolWindowActivation<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer>
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
		/// The service provider passed to the constructor. The base class messes with this.
		/// </summary>
		private readonly IServiceProvider myCtorServiceProvider;
		private ToolWindowActivationHelper<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer> myActivationHelper;
		#endregion // Member Variables
		#region Properties for CurrentDocument and CurrentORMSelectionContainer
		/// <summary>
		/// Get the current ORMDesignerDocData
		/// </summary>
		protected ORMDesignerDocData CurrentDocument
		{
			get
			{
				return myActivationHelper.CurrentDocument;
			}
		}
		/// <summary>
		/// Get the current DiagramDocView. This will be null if CurrentDocument is null.
		/// </summary>
		protected DiagramDocView CurrentDocumentView
		{
			get
			{
				return myActivationHelper.CurrentDocumentView;
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
				return myActivationHelper.CurrentSelectionContainer;
			}
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
			myActivationHelper = new ToolWindowActivationHelper<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer>(myCtorServiceProvider, Frame, CoveredFrameContentActions, this);
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
		protected FrameVisibility CurrentFrameVisibility
		{
			get
			{
				return myActivationHelper.CurrentFrameVisibility;
			}
		}
		/// <summary>
		/// Clear the tool window if it is covered but not detached.
		/// </summary>
		protected void ClearIfCovered()
		{
			myActivationHelper.ClearIfCovered();
		}
		#endregion // Window state notification changes
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
		/// </summary>
		/// <remarks>
		/// See <see cref="ModelingWindowPane.CountSelectedObjects"/> and <see cref="ISelectionContainer.CountObjects"/>.
		/// </remarks>
		protected override uint CountSelectedObjects()
		{
			uint retVal = 0;
			IORMSelectionContainer container = myActivationHelper.CurrentSelectionContainer;
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
		/// </summary>
		/// <remarks>
		/// See <see cref="ModelingWindowPane.CountAllObjects"/> and <see cref="ISelectionContainer.CountObjects"/>.
		/// </remarks>
		protected override uint CountAllObjects()
		{
			uint retVal = 0;
			IORMSelectionContainer container = myActivationHelper.CurrentSelectionContainer;
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
		/// </summary>
		/// <remarks>
		/// See <see cref="ModelingWindowPane.GetSelectedComponents"/> and
		/// <see cref="System.ComponentModel.Design.ISelectionService.GetSelectedComponents"/>.
		/// </remarks>
		public override ICollection GetSelectedComponents()
		{
			IORMSelectionContainer container = myActivationHelper.CurrentSelectionContainer;
			if (container != null)
			{
				return (container == this) ? base.GetSelectedComponents() : container.GetSelectedComponents();
			}
			return null;
		}
		/// <summary>
		/// Returns the elements that are currently selected in the <see cref="ModelingWindowPane"/>.
		/// </summary>
		/// <remarks>
		/// See <see cref="ModelingWindowPane.GetSelectedObjects"/> and <see cref="ISelectionContainer.GetObjects"/>.
		/// </remarks>
		protected override void GetSelectedObjects(uint count, object[] objects)
		{
			IORMSelectionContainer container = myActivationHelper.CurrentSelectionContainer;
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
		/// </summary>
		/// <remarks>
		/// See <see cref="ModelingWindowPane.GetAllObjects"/> and <see cref="ISelectionContainer.GetObjects"/>.
		/// </remarks>
		protected override void GetAllObjects(uint count, object[] objects)
		{
			IORMSelectionContainer container = myActivationHelper.CurrentSelectionContainer;
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
		/// </summary>
		/// <remarks>
		/// See <see cref="ModelingWindowPane.DoSelectObjects"/> and <see cref="ISelectionContainer.SelectObjects"/>.
		/// </remarks>
		protected override void DoSelectObjects(uint count, object[] objects, uint flags)
		{
			IORMSelectionContainer container = myActivationHelper.CurrentSelectionContainer;
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
			Store store = docData.Store;
			if (store != null && !store.Disposed)
			{
				ManageEventHandlers(store, ModelingEventManager.GetModelingEventManager(store), EventHandlerAction.Remove);
			}
		}
		#endregion // ORMToolWindow specific
		#region INotifyToolWindowActivation<ORMDesignerDocData,DiagramDocView,IORMSelectionContainer> Implementation
		/// <summary>
		/// Implements <see cref="INotifyToolWindowActivation{ORMDesignerDocData, DiagramDocView, IORMSelectionContainer}.ActivatorSelectionContainerChanged"/>
		/// </summary>
		protected void ActivatorSelectionContainerChanged(ToolWindowActivationHelper<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer> activator)
		{
			OnORMSelectionContainerChanged();
		}
		void INotifyToolWindowActivation<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer>.ActivatorSelectionContainerChanged(ToolWindowActivationHelper<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer> activator)
		{
			ActivatorSelectionContainerChanged(activator);
		}
		/// <summary>
		/// Implements <see cref="INotifyToolWindowActivation{ORMDesignerDocData, DiagramDocView, IORMSelectionContainer}.ActivatorDocumentChanged"/>
		/// </summary>
		protected void ActivatorDocumentChanged(ToolWindowActivationHelper<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer> activator)
		{
			OnCurrentDocumentChanged();
		}
		void INotifyToolWindowActivation<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer>.ActivatorDocumentChanged(ToolWindowActivationHelper<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer> activator)
		{
			ActivatorDocumentChanged(activator);
		}
		/// <summary>
		/// Implements <see cref="INotifyToolWindowActivation{ORMDesignerDocData, DiagramDocView, IORMSelectionContainer}.ActivatorDocumentViewChanged"/>
		/// </summary>
		protected void ActivatorDocumentViewChanged(ToolWindowActivationHelper<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer> activator)
		{
			OnCurrentDocumentViewChanged();
		}
		void INotifyToolWindowActivation<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer>.ActivatorDocumentViewChanged(ToolWindowActivationHelper<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer> activator)
		{
			ActivatorDocumentViewChanged(activator);
		}
		/// <summary>
		/// Implements <see cref="INotifyToolWindowActivation{ORMDesignerDocData, DiagramDocView, IORMSelectionContainer}.ActivatorAttachEventHandlers"/>
		/// </summary>
		protected void ActivatorAttachEventHandlers(ORMDesignerDocData docData)
		{
			AttachEventHandlers(docData);
		}
		void INotifyToolWindowActivation<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer>.ActivatorAttachEventHandlers(ORMDesignerDocData docData)
		{
			ActivatorAttachEventHandlers(docData);
		}
		/// <summary>
		/// Implements <see cref="INotifyToolWindowActivation{ORMDesignerDocData, DiagramDocView, IORMSelectionContainer}.ActivatorDetachEventHandlers"/>
		/// </summary>
		protected void ActivatorDetachEventHandlers(ORMDesignerDocData docData)
		{
			DetachEventHandlers(docData);
		}
		void INotifyToolWindowActivation<ORMDesignerDocData, DiagramDocView, IORMSelectionContainer>.ActivatorDetachEventHandlers(ORMDesignerDocData docData)
		{
			ActivatorDetachEventHandlers(docData);
		}
		#endregion // INotifyToolWindowActivation<ORMDesignerDocData,DiagramDocView,IORMSelectionContainer> Implementation
	}
}
