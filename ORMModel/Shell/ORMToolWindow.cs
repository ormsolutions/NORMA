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

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// Provides common functionality for all ORM tool windows.  Implements ToolWindow for WindowTitle functionality,
	/// declares abstract methods for attaching and detaching event handlers, and handles the logic for switching
	/// between ORM documents and non-ORM documents, different tool windows, etc.
	/// </summary>
	[CLSCompliant(false)]
	public abstract class ORMToolWindow : ToolWindow
	{
		#region Local Data Members
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
		#endregion // Local Data Members
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
			if (myCurrentDocument == docData)
			{
				if (myCurrentDocumentView != docView)
				{
					myCurrentDocumentView = docView;
					OnCurrentDocumentViewChanged();
				}
				return;
			}
			if (myCurrentDocument != null && myCurrentDocument.Store != null)	// If the current document is not null
			{
				// If we get to this point, we know that the document window
				// has really changed, so we need to unwire the event handlers
				// from the model store.
				DetachEventHandlers(myCurrentDocument.Store);
			}
			myCurrentDocument = docData;
			myCurrentDocumentView = docView;
			if (docData != null)	// If the new DocData is actually an ORMDesignerDocData,
			{
				Store newStore = docData.Store;
				if (newStore != null)
				{
					AttachEventHandlers(newStore);	// wire the event handlers to the model store.
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
		protected virtual IORMSelectionContainer CurrentORMSelectionContainer
		{
			get
			{
				return myCurrentORMSelectionContainer;
			}
			private set
			{
				if (value != null)
				{
					myCurrentORMSelectionContainer = value;
					OnORMSelectionContainerChanged();
				}
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
		/// The <see cref="IServiceProvider"/> instance passed to the constraint.
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
			IMonitorSelectionService monitor = (IMonitorSelectionService)myCtorServiceProvider.GetService(typeof(IMonitorSelectionService));
			monitor.SelectionChanged += new EventHandler<MonitorSelectionEventArgs>(MonitorSelectionChanged);
			monitor.DocumentWindowChanged += new EventHandler<MonitorSelectionEventArgs>(DocumentWindowChanged);
			try
			{
				//CurrentDocument = monitor.CurrentDocument as ORMDesignerDocData;
				SetCurrentDocument(monitor.CurrentDocument as ORMDesignerDocData, monitor.CurrentDocumentView as DiagramDocView);
				CurrentORMSelectionContainer = monitor.CurrentSelectionContainer as IORMSelectionContainer;
			}
			catch (System.Runtime.InteropServices.COMException)
			{
				// Swallow, this will occasionally be initialized when the document is shutting down
			}
		}
		#endregion // ORMToolWindow Constructor
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
			SetCurrentDocument(monitor.CurrentDocument as ORMDesignerDocData, monitor.CurrentDocumentView as DiagramDocView);
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
		/// Attach <see cref="EventHandler{TEventArgs}"/>s to the <see cref="Store"/>. Defers to <see cref="ManageEventHandlers"/>.
		/// </summary>
		protected void AttachEventHandlers(Store store)
		{
			ManageEventHandlers(store, ModelingEventManager.GetModelingEventManager(store), EventHandlerAction.Add);
		}
		/// <summary>
		/// Detach <see cref="EventHandler{TEventArgs}"/>s from the <see cref="Store"/>. Defers to <see cref="ManageEventHandlers"/>.
		/// </summary>
		protected void DetachEventHandlers(Store store)
		{
			ManageEventHandlers(store, ModelingEventManager.GetModelingEventManager(store), EventHandlerAction.Remove);
		}
		#endregion // ORMToolWindow specific
	}
}
