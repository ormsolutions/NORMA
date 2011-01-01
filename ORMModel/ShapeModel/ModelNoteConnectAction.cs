#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ShapeModel;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	#region ModelNoteConnectAction class
	/// <summary>
	/// A connect action for attaching a base type to its derived type
	/// </summary>
	public class ModelNoteConnectAction : ConnectAction
	{
		#region ModelNoteConnectionType class
		/// <summary>
		/// The ConnectionType used with this ConnectAction. The type
		/// is a singleton, holds all of the context-independent logic,
		/// and operates directly on shape elements.
		/// </summary>
		protected class ModelNoteConnectionType : ConnectionType
		{
			/// <summary>
			/// The singleton ModelNoteConnectionType instance
			/// </summary>
			public static new readonly ModelNoteConnectionType Instance = new ModelNoteConnectionType();
			/// <summary>
			/// An array of one element containing the singleton ModelNoteConnectionType instance
			/// </summary>
			public static readonly ConnectionType[] InstanceArray = { Instance };
			/// <summary>
			/// Called as the pointer is moved over potential targets after a source is selected
			/// So should be pretty quick
			/// Gets called with a target of null when the cursor leaves the view.
			/// Can't find a use for this at present.
			/// </summary>
			/// <param name="sourceShapeElement">ShapeElement</param>
			/// <param name="targetShapeElement">ShapeElement</param>
			public override bool IsOfInterest(ShapeElement sourceShapeElement, ShapeElement targetShapeElement)
			{
				return false; // Always or'd with IsValidSourceAndTarget
			}
			/// <summary>
			/// Called as the pointer is moved over potential targets after a source is selected
			/// So should be pretty quick
			/// </summary>
			/// <remarks>
			/// The cursor can change dependant on CanCreateConnection when this returns true
			/// </remarks>
			/// <param name="sourceShapeElement">ShapeElement</param>
			/// <param name="targetShapeElement">ShapeElement</param>
			/// <returns></returns>
			public override bool IsValidSourceAndTarget(ShapeElement sourceShapeElement, ShapeElement targetShapeElement)
			{
				ModelNoteConnectAction action = (sourceShapeElement.Diagram as ORMDiagram).ModelNoteConnectAction;
				ORMModelElement sourceElement;
				ORMModelElement targetElement;
				if (null != (sourceElement = action.mySourceElement) &&
					null != (targetElement = ElementFromShape<ORMModelElement>(targetShapeElement)))
				{
					ModelNote note = null;
					if (null == (note = sourceElement as ModelNote))
					{
						if (null != (note = targetElement as ModelNote))
						{
							// Switch the source and target
							targetElement = sourceElement;
						}
					}
					if (note != null)
					{
						FactType factType;
						ObjectType objectType;
						SetConstraint setConstraint;
						SetComparisonConstraint setComparisonConstraint;
						if (null != (factType = targetElement as FactType))
						{
							return !note.FactTypeCollection.Contains(factType);
						}
						else if (null != (objectType = targetElement as ObjectType))
						{
							return !note.ObjectTypeCollection.Contains(objectType);
						}
						else if (null != (setConstraint = targetElement as SetConstraint))
						{
							return !setConstraint.Constraint.ConstraintIsInternal && !note.SetConstraintCollection.Contains(setConstraint);
						}
						else if (null != (setComparisonConstraint = targetElement as SetComparisonConstraint))
						{
							return !note.SetComparisonConstraintCollection.Contains(setComparisonConstraint);
						}
					}
				}
				return false;
			}
			/// <summary>
			/// Called after IsValidSourceAndTarget allows the shapes through. Used for
			/// more in-depth checking before ConnectionType.CreateConnection is called, and
			/// to display warning messages on the design surface.
			/// </summary>
			/// <param name="sourceShapeElement">The source of the requested connection</param>
			/// <param name="targetShapeElement">The target of the requested connection</param>
			/// <param name="connectionWarning">A location to write the warning string</param>
			/// <returns>true if the connection can proceed</returns>
			public override bool CanCreateConnection(ShapeElement sourceShapeElement, ShapeElement targetShapeElement, ref string connectionWarning)
			{
				// Everything is handled in IsValidSourceAndTarget.
				return true;
			}
			/// <summary>
			/// Create a connection between an ModelNoteShape and a FactType or ObjectType.
			/// </summary>
			/// <param name="sourceShapeElement">The source of the requested connection</param>
			/// <param name="targetShapeElement">The target of the requested connection</param>
			/// <param name="paintFeedbackArgs">PaintFeedbackArgs</param>
			public override void CreateConnection(ShapeElement sourceShapeElement, ShapeElement targetShapeElement, PaintFeedbackArgs paintFeedbackArgs)
			{
				ModelNoteConnectAction action = (sourceShapeElement.Diagram as ORMDiagram).ModelNoteConnectAction;
				ORMModelElement sourceElement;
				ORMModelElement targetElement;
				if (null != (sourceElement = action.mySourceElement) &&
					null != (targetElement = ElementFromShape<ORMModelElement>(targetShapeElement)))
				{
					ModelNote note = null;
					if (null == (note = sourceElement as ModelNote))
					{
						if (null != (note = targetElement as ModelNote))
						{
							// Switch the source and target
							targetElement = sourceElement;
						}
					}
					if (note != null)
					{
						FactType factType;
						ObjectType objectType;
						SetConstraint setConstraint;
						SetComparisonConstraint setComparisonConstraint;
						if (null != (factType = targetElement as FactType))
						{
							new ModelNoteReferencesFactType(note, factType);
						}
						else if (null != (objectType = targetElement as ObjectType))
						{
							new ModelNoteReferencesObjectType(note, objectType);
						}
						else if (null != (setConstraint = targetElement as SetConstraint))
						{
							new ModelNoteReferencesSetConstraint(note, setConstraint);
						}
						else if (null != (setComparisonConstraint = targetElement as SetComparisonConstraint))
						{
							new ModelNoteReferencesSetComparisonConstraint(note, setComparisonConstraint);
						}
					}
				}
			}
			/// <summary>
			/// Provide the transaction name. The name is displayed in the undo and redo lists.
			/// </summary>
			public override string GetConnectTransactionName(ShapeElement sourceShape, ShapeElement targetShape)
			{
				return ResourceStrings.ModelNoteConnectActionTransactionName;
			}
		}
		#endregion // ModelNoteConnectionType class
		#region Member variables
		// The following cursors are built as embedded resources. Pick them up by their file name.
		private static Cursor myAllowedCursor = new Cursor(typeof(ModelNoteConnectAction), "ConnectModelNoteAllowed.cur");
		private static Cursor mySearchingCursor = new Cursor(typeof(ModelNoteConnectAction), "ConnectModelNoteSearching.cur");
		private static readonly ConnectionType[] EmptyConnectionTypes = {};
		private ORMModelElement mySourceElement;
		private bool myEmulateDrag;
		#endregion // Member variables
		#region Constructors
		/// <summary>
		/// Create a model note connector action for the given diagram. One
		/// action per diagram should be sufficient.
		/// </summary>
		/// <param name="diagram">The hosting diagram</param>
		public ModelNoteConnectAction(Diagram diagram)
			: base(diagram, true)
		{
			Reset();
		}
		#endregion // Constructors
		#region Base overrides
		/// <summary>
		/// Retrieve all connect types associated with this connect action.
		/// Returns an empty array unless the sourceShapeElement is an ModelNoteShape
		/// </summary>
		/// <param name="sourceShapeElement">The source element</param>
		/// <param name="targetShapeElement">The target element. Currently ignored.</param>
		/// <returns></returns>
		protected override ConnectionType[] GetConnectionTypes(ShapeElement sourceShapeElement, ShapeElement targetShapeElement)
		{
			return ModelNoteConnectionType.InstanceArray;
		}
		/// <summary>
		/// Reset the member variables
		/// </summary>
		/// <param name="e">DiagramEventArgs</param>
		protected override void OnMouseActionDeactivated(DiagramEventArgs e)
		{
			base.OnMouseActionDeactivated(e);

			// Set the selection back to pointer after connect action
			DiagramView activeView;
			IToolboxService toolbox;
			if ((null != (activeView = Diagram.ActiveDiagramView)) &&
				(null != (toolbox = activeView.Toolbox)))
			{
				toolbox.SelectedToolboxItemUsed();
			}

			Reset();
		}
		/// <summary>
		/// Get the source object type or role after the
		/// base class sets the source shape
		/// </summary>
		/// <param name="e">MouseActionEventArgs</param>
		protected override void OnClicked(MouseActionEventArgs e)
		{
			base.OnClicked(e);
			if (mySourceElement == null)
			{
				mySourceElement = ElementFromShape<ORMModelElement>(MouseDownHitShape);
			}
		}
		/// <summary>
		/// Allow dragging the source element onto the target
		/// </summary>
		protected override void OnDraggingBegun(MouseActionEventArgs e)
		{
			if (mySourceElement == null)
			{
				mySourceElement = ElementFromShape<ORMModelElement>(MouseDownHitShape);
			}
			base.OnDraggingBegun(e);
		}		/// <summary>
		/// If we're emulating a drag (occurs when we're chained from RoleDragPendingAction),
		/// then complete the action on mouse up
		/// </summary>
		/// <param name="e">DiagramMouseEventArgs</param>
		protected override void OnMouseUp(DiagramMouseEventArgs e)
		{
			if (myEmulateDrag && mySourceElement != null)
			{
				myEmulateDrag = false;
				DiagramClientView clientView = e.DiagramClientView;
				if (CanComplete(clientView))
				{
					// Establish the mouse down destinations
					MouseDown(e);
					Complete(clientView);
				}
			}
			base.OnMouseUp(e);
		}
		/// <summary>
		/// Get a cursor from the cursor type. Returns the searching cursor for
		/// everything except the allowed action.
		/// </summary>
		/// <param name="connectActionCursor">The requrested cursor styl</param>
		/// <returns></returns>
		protected override Cursor GetCursorFromCursorType(ConnectActionCursor connectActionCursor)
		{
			Cursor cursor;
			switch (connectActionCursor)
			{
				case ConnectActionCursor.Allowed:
					cursor = myAllowedCursor;
					break;
				//case ConnectActionCursor.Searching:
				//case ConnectActionCursor.Disallowed:
				//case ConnectActionCursor.Warning:
				default:
					cursor = mySearchingCursor;
					break;
			}
			return cursor;
		}
		#endregion // Base overrides
		#region ModelNoteConnectAction specific
		private void Reset()
		{
			mySourceElement = null;
			myEmulateDrag = false;
		}
		/// <summary>
		/// Set this mouse action as the active action on the
		/// diagram of the given shape, and activate its drag line
		/// centered on the shape.
		/// </summary>
		/// <param name="chainFromPoint">The point to begin the mouse action</param>
		/// <param name="clientView">The active DiagramClientView</param>
		/// <param name="emulateDrag">true if this should emulate a drag, meaning
		/// that the mouse up acts like a click.</param>
		public void ChainMouseAction(PointD chainFromPoint, DiagramClientView clientView, bool emulateDrag)
		{
			DiagramView activeView = Diagram.ActiveDiagramView;
			if (activeView != null)
			{
				// Move on to the selection action
				clientView.ActiveMouseAction = this;

				// Now emulate a mouse click in the middle of the added note shape. The click
				// actions provide a starting point for the connect action, so a mouse move
				// provides a drag line.
				Point emulateClickPoint = clientView.WorldToDevice(chainFromPoint);
				DiagramMouseEventArgs mouseEventArgs = new DiagramMouseEventArgs(new MouseEventArgs(MouseButtons.Left, 1, emulateClickPoint.X, emulateClickPoint.Y, 0), clientView);
				MouseDown(mouseEventArgs);
				Click(new DiagramPointEventArgs(emulateClickPoint.X, emulateClickPoint.Y, PointRelativeTo.Client, clientView));
				MouseUp(mouseEventArgs);

				// An extra move lets us chain when the mouse is not on the design surface,
				// such as when we are being activated via the task list.
				MouseMove(mouseEventArgs);

				myEmulateDrag = emulateDrag;
				ORMDiagram.SelectToolboxItem(activeView, ResourceStrings.ToolboxModelNoteConnectorItemId);
			}
		}
		/// <summary>
		/// Get the underlying element for a presentation element
		/// </summary>
		/// <typeparam name="ElementType">The type of the element to retrieve</typeparam>
		/// <param name="shape">A presentation element.</param>
		/// <returns>An ElementType element, or null</returns>
		protected static ElementType ElementFromShape<ElementType>(PresentationElement shape) where ElementType : ModelElement
		{
			return ((shape != null) ? shape.ModelElement : null) as ElementType;
		}
		#endregion // ModelNoteConnectAction specific
	}
	#endregion // ModelNoteConnectAction class
	#region ModelNoteAction class
	/// <summary>
	/// A toolbox action to add a model note and activate
	/// the new note shape for editing when the item is first added
	/// </summary>
	public class ModelNoteAction : ToolboxAction
	{
		#region Member variables
		/// <summary>
		/// An event that fires after the standard MouseActionDeactivated event. The
		/// standard event clears the toolbox, this one can be used to chain this
		/// action to a new standard action.
		/// </summary>
		public event MouseActionDeactivatedEventHandler AfterMouseActionDeactivated;
		private ModelNoteShape myAddedNoteShape;
		#endregion // Member variables
		#region Constructors
		/// <summary>
		/// Construct a new model note action for a specific diagram. The
		/// same action can be reused multiple times by activating and
		/// deactivating it.
		/// </summary>
		/// <param name="diagram">The owning diagram</param>
		public ModelNoteAction(Diagram diagram)
			: base(diagram)
		{
			Reset();
		}
		#endregion // Constructors
		#region ModelNoteAction specific
		/// <summary>
		/// Central function to return member variables to a clean state.
		/// Called by the constructor and the deactivation sequence.
		/// </summary>
		private void Reset()
		{
			myAddedNoteShape = null;
		}
		/// <summary>
		/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> during connect action activation and deactivation.
		/// The default implementation watches for new <see cref="ModelNoteShape"/>s added to the <see cref="Diagram"/>.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		protected virtual void ManageStoreEvents(Store store, EventHandlerAction action)
		{
			if (store == null || store.Disposed)
			{
				return; // bail out
			}
			ModelingEventManager.GetModelingEventManager(store).AddOrRemoveHandler(store.DomainDataDirectory.FindDomainClass(ModelNoteShape.DomainClassId), new EventHandler<ElementAddedEventArgs>(ModelNoteShapeAddedEvent), action);
		}
		/// <summary>
		/// An IMS event to track the shape element added to the associated
		/// diagram during this connect action.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ModelNoteShapeAddedEvent(object sender, ElementAddedEventArgs e)
		{
			if (myAddedNoteShape == null)
			{
				ModelNoteShape candidate = e.ModelElement as ModelNoteShape;
				// Make sure the shape was added to the diagram associated with this
				// connect action
				if (candidate != null && candidate.Diagram == Diagram)
				{
					myAddedNoteShape = candidate;
				}
			}
		}
		/// <summary>
		/// The note shape added as a result of a completed mouse action
		/// </summary>
		public ModelNoteShape AddedNoteShape
		{
			get
			{
				return myAddedNoteShape;
			}
		}
		/// <summary>
		/// Was this action completed successfully?
		/// </summary>
		public bool ActionCompleted
		{
			get
			{
				return myAddedNoteShape != null;
			}
		}
		#endregion // ModelNoteAction specific
		#region Base overrides
		/// <summary>
		/// Adds an <see cref="EventHandler{TEventArgs}"/> on the <see cref="Store"/> so we can track the <see cref="ModelNoteShape"/>
		/// added during the <see cref="Transaction"/> resulting from completion of the <see cref="MouseAction"/>.
		/// </summary>
		protected override void OnMouseActionActivated(DiagramEventArgs e)
		{
			ManageStoreEvents(Diagram.Store, EventHandlerAction.Add);
		}
		/// <summary>
		/// Deactivates the <see cref="MouseAction"/> by removing the listening <see cref="EventHandler{TEventArgs}"/>s,
		/// calling the base, and then raising the <see cref="AfterMouseActionDeactivated"/> event.
		/// </summary>
		protected override void OnMouseActionDeactivated(DiagramEventArgs e)
		{
			if (myInOnClicked)
			{
				myDeactivatedDuringOnClick = true;
				return;
			}
			ManageStoreEvents(e.DiagramClientView.Diagram.Store, EventHandlerAction.Remove);
			base.OnMouseActionDeactivated(e);
			MouseActionDeactivatedEventHandler handler = AfterMouseActionDeactivated;
			if (handler != null)
			{
				handler(this, e);
			}
			Reset();
		}
		#region Deactivation order HACK
		private bool myInOnClicked;
		private bool myDeactivatedDuringOnClick;
		/// <summary>
		/// Hack override to handle MSBUG where the transaction triggered
		/// by the mouse action is committed after the mouse action is deactivated.
		/// We don't want this because we end up tossing our state prematurely and
		/// cannot commit our mouse action.
		/// </summary>
		protected override void OnClicked(MouseActionEventArgs e)
		{
			try
			{
				myInOnClicked = true;
				myDeactivatedDuringOnClick = false;
				base.OnClicked(e);
#if VISUALSTUDIO_10_0
				if (IsActive)
				{
					Cancel(e.DiagramClientView);
				}
#endif
				myInOnClicked = false;
				if (myDeactivatedDuringOnClick)
				{
					OnMouseActionDeactivated(e);
				}
			}
			finally
			{
				myInOnClicked = false;
			}
		}
		#endregion // Deactivation order HACK
		#endregion // Base overrides
	}
	#endregion // ModelNoteAction class
}
