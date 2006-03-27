using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections;

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
		/// The service provider passed to the constructor. The base class messes with this.
		/// </summary>
		private IServiceProvider myCtorServiceProvider;
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
			private set
			{
				if (object.ReferenceEquals(myCurrentDocument, value))
				{
					return;
				}
				if (myCurrentDocument != null && myCurrentDocument.Store != null)	// If the current document is not null
				{
					// If we get to this point, we know that the document window
					// has really changed, so we need to unwire the event handlers
					// from the model store.
					DetachEventHandlers(myCurrentDocument.Store);
				}
				myCurrentDocument = value;
				if (value != null)	// If the new DocData is actually an ORMDesignerDocData,
				{
					Store newStore = value.Store;
					if (newStore != null)
					{
						AttachEventHandlers(newStore);	// wire the event handlers to the model store.
					}
					else
					{
						myCurrentDocument = null;
					}
				}
				OnCurrentDocumentChanged();
			}
		}
		/// <summary>
		/// Provide a notification when the selection container has been modified. The
		/// default implemention is empty.
		/// </summary>
		protected virtual void OnCurrentDocumentChanged() { }
		/// <summary>
		/// Get the current IORMSelectionContainer. Tracking this separate
		/// from CurrentDocument allows us to switch between multiple
		/// different ORM tool windows without changing the current ORM documtn.
		/// </summary>
		protected virtual IORMSelectionContainer CurrentORMSelectionContainer
		{
			get
			{
				return myCurrentORMSelectionContainer;
			}
			private set
			{
				myCurrentORMSelectionContainer = value;
				OnORMSelectionContainerChanged();
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
		public ORMToolWindow(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			myCtorServiceProvider = serviceProvider;
		}
		/// <summary>
		/// Constructs a new ORM tool window, wires selection and document changed events,
		/// and initializes the CurrentModelElementSelectionContainer to the current DocView.
		/// </summary>
		protected override void Initialize()
		{
			IMonitorSelectionService monitor = (IMonitorSelectionService)myCtorServiceProvider.GetService(typeof(IMonitorSelectionService));
			monitor.SelectionChanged += new EventHandler<MonitorSelectionEventArgs>(MonitorSelectionChanged);
			monitor.DocumentWindowChanged += new EventHandler<MonitorSelectionEventArgs>(DocumentWindowChanged);
			CurrentDocument = monitor.CurrentDocument as ORMDesignerDocData;
			CurrentORMSelectionContainer = monitor.CurrentDocumentView as IORMSelectionContainer;
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
			CurrentDocument = ((IMonitorSelectionService)sender).CurrentDocument as ORMDesignerDocData;
		}
		#endregion // IMonitorSelectionService Event Handlers
		#region Abstract Methods and Properties
		/// <summary>
		/// Attaches custom event handlers to the store.  This method must be overridden.
		/// </summary>
		/// <param name="store">The store to which event handlers should be attached.</param>
		protected abstract void AttachEventHandlers(Store store);
		/// <summary>
		/// Detaches custom event handlers from the store.  This method must be overridden.
		/// </summary>
		/// <param name="store">The store from which event handlers should be detached.</param>
		protected abstract void DetachEventHandlers(Store store);
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
		/// Passes the count request through to myCurrentORMSelectionContainer.
		/// </summary>
		/// <returns>The number of objects selected in myCurrentORMSelectionContainer.</returns>
		protected override uint CountSelectedObjects()
		{
			uint retVal = 0;
			if (myCurrentORMSelectionContainer != null)
			{
				myCurrentORMSelectionContainer.CountObjects((uint)Constants.GETOBJS_SELECTED, out retVal);
			}
			return retVal;
		}
		/// <summary>
		/// Passes the count request through to myCurrentORMSelectionContainer.
		/// </summary>
		/// <returns>The total number of objects in myCurrentORMSelectionContainer.</returns>
		protected override uint CountAllObjects()
		{
			uint retVal = 0;
			if (myCurrentORMSelectionContainer != null)
			{
				myCurrentORMSelectionContainer.CountObjects((uint)Constants.GETOBJS_ALL, out retVal);
			}
			return retVal;
		}
		/// <summary>
		/// Passes GetSelectedComponents through to myCurrentORMSelectionContainer casted as a ModelingWindowPane.
		/// </summary>
		/// <returns></returns>
		public override ICollection GetSelectedComponents()
		{
			ICollection retVal = null;
			ModelingWindowPane pane = myCurrentORMSelectionContainer as ModelingWindowPane;
			if (pane != null)
			{
				retVal = pane.GetSelectedComponents();
			}
			return retVal;
		}
		/// <summary>
		/// Passes the get request through to myCurrentORMSelectionContainer.
		/// </summary>
		/// <param name="count">The number of objects to get.</param>
		/// <param name="objects">An object array to store the objects retrieved from myCurrentORMSelectionContainer.</param>
		protected override void GetSelectedObjects(uint count, object[] objects)
		{
			if (myCurrentORMSelectionContainer != null)
			{
				myCurrentORMSelectionContainer.GetObjects((uint)Constants.GETOBJS_SELECTED, count, objects);
			}
		}
		/// <summary>
		/// Passes the get request through to myCurrentORMSelectionContainer.
		/// </summary>
		/// <param name="count">The number of objects to get.</param>
		/// <param name="objects">An object array to store the objects retrieved from myCurrentORMSelectionContainer.</param>
		protected override void GetAllObjects(uint count, object[] objects)
		{
			if (myCurrentORMSelectionContainer != null)
			{
				myCurrentORMSelectionContainer.GetObjects((uint)Constants.GETOBJS_ALL, count, objects);
			}
		}
		/// <summary>
		/// Passes the select request through to myCurrentORMSelectionContainer.
		/// </summary>
		/// <param name="count">The number of objects to select.</param>
		/// <param name="objects">An array of objects which should be selected.</param>
		/// <param name="flags">Unknown</param>
		protected override void DoSelectObjects(uint count, object[] objects, uint flags)
		{
			if (myCurrentORMSelectionContainer != null)
			{
				myCurrentORMSelectionContainer.SelectObjects(count, objects, flags);
			}
		}
		#endregion // ISelectionContainer overrides
	}
}
