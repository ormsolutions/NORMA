using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Northface.Tools.ORM.ObjectModel;

namespace Northface.Tools.ORM.ShapeModel
{
	/// <summary>
	/// A ConnectAction to add role sequences to an external constraint
	/// </summary>
	[CLSCompliant(true)]
	public class ExternalConstraintConnectAction : ConnectAction
	{
		#region ExternalConstraintConnectionType class
		/// <summary>
		/// The ConnectionType used with this ConnectAction. The type
		/// is a singleton, holds all of the context-independent logic,
		/// and operates directly on shape elements.
		/// </summary>
		protected class ExternalConstraintConnectionType : ConnectionType
		{
			/// <summary>
			/// The singleton ExternalConstraintConnectionType instance
			/// </summary>
			public static new readonly ExternalConstraintConnectionType Instance = new ExternalConstraintConnectionType();
			/// <summary>
			/// An array of one element containing the singleton ExternalConstraintConnectionType instance
			/// </summary>
			public static readonly ConnectionType[] InstanceArray = {Instance};
			/// <summary>
			/// Called as the pointer is moved over potential targets after a source is selected
			/// So should be pretty quick
			/// </summary>
			/// <remarks>
			/// The cursor can change dependant on CanCreateConnection when this returns true
			/// When this returns false, the control falls back to
			/// ExternalConstraintConectAction.
			/// </remarks>
			/// <param name="sourceShapeElement">ShapeElement</param>
			/// <param name="targetShapeElement">ShapeElement</param>
			/// <returns></returns>
			public override bool IsValidSourceAndTarget(ShapeElement sourceShapeElement, ShapeElement targetShapeElement)
			{
				// We support attaching from an external constraint shape to a fact type shape. However, to
				// get warning text on the drag action we need to get through to CanCreateConnection, so we
				// are more lenient here.
				bool retVal = false;
				if (sourceShapeElement is ExternalConstraintShape)
				{
					retVal = targetShapeElement is FactTypeShape ||
						object.ReferenceEquals(targetShapeElement, sourceShapeElement.Diagram) ||
						object.ReferenceEquals(sourceShapeElement, targetShapeElement);
				}
				return retVal;
			}
			/// <summary>
			/// Used for more in-depth checking before ConnectionType.CreateConnection is called, and
			/// to display warning messages on the design surface.
			/// </summary>
			/// <param name="sourceShapeElement">The source of the requested connection</param>
			/// <param name="targetShapeElement">The target of the requested connection</param>
			/// <param name="connectionWarning">A location to write the warning string</param>
			/// <returns>true if the connection can proceed</returns>
			public override bool CanCreateConnection(ShapeElement sourceShapeElement, ShapeElement targetShapeElement, ref string connectionWarning)
			{
				bool retVal = false;
				if (sourceShapeElement is ExternalConstraintShape)
				{
					if (targetShapeElement is FactTypeShape)
					{
						// UNDONE: Constrain this, this is overly generous
						retVal = true;
					}
					else
					{
						Debug.Assert(IsValidSourceAndTarget(sourceShapeElement, targetShapeElement)); // The condition that got us here
						connectionWarning = ResourceStrings.ExternalConstraintConnectActionInstructions;
						// Let the click through for the diagram to generate a completion request so we can
						// effect a cancel by ignoring it.
						retVal = object.ReferenceEquals(targetShapeElement, sourceShapeElement.Diagram);
					}
				}
				return retVal;
			}
			/// <summary>
			/// Create a connection between an ExternalConstraintShape and a FactType. Roles
			/// used in the connection are stored with the currently active connect action.
			/// </summary>
			/// <param name="sourceShapeElement">The source of the requested connection</param>
			/// <param name="targetShapeElement">The target of the requested connection</param>
			/// <param name="paintFeedbackArgs">PaintFeedbackArgs</param>
			public override void CreateConnection(ShapeElement sourceShapeElement, ShapeElement targetShapeElement, PaintFeedbackArgs paintFeedbackArgs)
			{
				ExternalConstraintShape constraintShape;
				IConstraint constraint;
				ExternalConstraintConnectAction action;
				IList<Role> selectedRoles;
				int rolesCount;
				if ((null != (constraintShape = sourceShapeElement as ExternalConstraintShape)) &&
					(null != (constraint = constraintShape.AssociatedConstraint)) &&
					(null != (action = (sourceShapeElement.Diagram as ORMDiagram).ExternalConstraintConnectAction)) &&
					(null != (selectedRoles = action.SelectedRoleCollection)) &&
					(0 != (rolesCount = selectedRoles.Count)))
				{
					MultiColumnExternalConstraint mcConstraint;
					SingleColumnExternalConstraint scConstraint;
					if (null != (mcConstraint = constraint as MultiColumnExternalConstraint))
					{
						ConstraintRoleSequence constraintRoleSequenceBeingEdited = action.ConstraintRoleSequenceToEdit;
						// Add a new role set
						if (null == constraintRoleSequenceBeingEdited)
						{
							MultiColumnExternalConstraintRoleSequenceMoveableCollection roleSequences = mcConstraint.RoleSequenceCollection;
							MultiColumnExternalConstraintRoleSequence roleSequence = MultiColumnExternalConstraintRoleSequence.CreateMultiColumnExternalConstraintRoleSequence(mcConstraint.Store);
							RoleMoveableCollection roles = roleSequence.RoleCollection;
							for (int i = 0; i < rolesCount; ++i)
							{
								roles.Add(selectedRoles[i]);
							}
							roleSequences.Add(roleSequence);
						}
						// Edit the existing role set.
						else
						{
							RoleMoveableCollection roles = constraintRoleSequenceBeingEdited.RoleCollection;
							roles.Clear();
							foreach (Role role in action.SelectedRoleCollection)
							{
								roles.Add(role);
							}
						}
					}
					else if (null != (scConstraint = constraint as SingleColumnExternalConstraint))
					{
						// The single-column constraint is its own role set, just add the roles
						RoleMoveableCollection roles = scConstraint.RoleCollection;
						roles.Clear();
						for (int i = 0; i < rolesCount; ++i)
						{
							roles.Add(selectedRoles[i]);
						}
					}
				}
			}
			/// <summary>
			/// Move the feedback dragline to the center
			/// </summary>
			/// <param name="sourceShapeElement"></param>
			/// <param name="sourcePoint"></param>
			/// <param name="targetShapeElement"></param>
			/// <param name="targetPoint"></param>
			/// <param name="paintFeedbackArgs"></param>
			/// <returns></returns>
			public override PaintFeedbackArgs UpdatePaintFeedbackParameters(ShapeElement sourceShapeElement, PointD sourcePoint, ShapeElement targetShapeElement, PointD targetPoint, PaintFeedbackArgs paintFeedbackArgs)
			{
				PaintFeedbackArgs args = base.UpdatePaintFeedbackParameters(sourceShapeElement, sourcePoint, targetShapeElement, targetPoint, paintFeedbackArgs);
				RectangleD bounds = sourceShapeElement.AbsoluteBoundingBox;
				args.SourceConnectionPoint = bounds.Center;
				return args;
			}
		}
		#endregion // ExternalConstraintConnectionType class
		#region Member variables
		// The following cursors are built as embedded resources. Pick them up by their file name.
		private static Cursor myAllowedCursor = new Cursor(typeof(ExternalConstraintConnectAction), "ConnectExternalConstraintAllowed.cur");
		private static Cursor mySearchingCursor = new Cursor(typeof(ExternalConstraintConnectAction), "ConnectExternalConstraintSearching.cur");
		private ConstraintRoleSequence myConstraintRoleSequence;
		private IList<Role> myInitialSelectedRoles;
		private IList<Role> mySelectedRoles;
		private ExternalConstraintShape mySourceShape;
		private DiagramItem myLastMouseMoveItem;
		private static readonly ConnectionType[] EmptyConnectionTypes = {};
		private enum OnClickedAction
		{
			Normal,
			CheckForCommit,
			Commit,
			Complete, // Not necessarily a set with the others, but we don't care about other values after completion
		}
		private OnClickedAction myPendingOnClickedAction;
		#endregion // Member variables
		#region Constructors
		/// <summary>
		/// Create an external constract action for the given diagram. One
		/// action per diagram should be sufficient.
		/// </summary>
		/// <param name="diagram">The hosting diagram</param>
		public ExternalConstraintConnectAction(Diagram diagram) : base(diagram, true)
		{
			Reset();
		}
		/// <summary>
		/// Retrieve all connect types associated with this connect action.
		/// Returns an empty array unless the sourceShapeElement is an ExternalConstraintShape
		/// </summary>
		/// <param name="sourceShapeElement">The source element</param>
		/// <param name="targetShapeElement">The target element. Currently ignored.</param>
		/// <returns></returns>
		protected override ConnectionType[] GetConnectionTypes(ShapeElement sourceShapeElement, ShapeElement targetShapeElement)
		{
			if (sourceShapeElement is ExternalConstraintShape)
			{
				return ExternalConstraintConnectionType.InstanceArray;
			}
			return EmptyConnectionTypes;
		}
		/// <summary>
		/// Get a cursor from the cursor type. Returns the searching cursor for
		/// everything except the allowed action.
		/// </summary>
		/// <param name="connectActionCursor">The requrested cursor styl</param>
		/// <returns></returns>
		protected override Cursor GetCursorFromCursorType(ConnectActionCursor connectActionCursor)
		{
			Cursor cursor = null;
			if (connectActionCursor == ConnectActionCursor.Allowed)
			{
				DiagramItem item;
				if (null != (item = myLastMouseMoveItem))
				{
					foreach (ModelElement element in item.RepresentedElements)
					{
						if (element is Role)
						{
							cursor = myAllowedCursor;
							break;
						}
					}
				}
			}
			return (cursor == null) ? mySearchingCursor : cursor;
		}
		#endregion // Constructors
		#region Base overrides
		/// <summary>
		/// Test for a double click if a commit is expected to signal the
		/// OnClicked to forward the click to the base ConnectAction and
		/// complete the operation.
		/// </summary>
		/// <param name="e">DiagramMouseEventArgs</param>
		protected override void OnMouseDown(DiagramMouseEventArgs e)
		{
			if (myPendingOnClickedAction == OnClickedAction.CheckForCommit && e.Clicks == 2 && e.Button == MouseButtons.Left)
			{
				myPendingOnClickedAction = OnClickedAction.Commit;
			}
		}
		[DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
		private static extern short GetKeyState(Keys nVirtKey);
		/// <summary>
		/// Track the last hit diagram item
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(DiagramMouseEventArgs e)
		{
			myLastMouseMoveItem = (DiagramItem)e.DiagramHitTestInfo.HitDiagramItem.Clone();
			base.OnMouseMove(e);
		}
		/// <summary>
		/// Add a source shape or commit/cancel the action by forwarding the
		/// click to the base class, or modify the current role sequence by handling
		/// the click locally.
		/// </summary>
		/// <param name="e">MouseActionEventArgs</param>
		protected override void OnClicked(MouseActionEventArgs e)
		{
			if (myPendingOnClickedAction == OnClickedAction.Commit)
			{
				myPendingOnClickedAction = OnClickedAction.Normal;
				// Letting the click through to the base ConnectAction
				// at this point (a constraint is selected and a role has been
				// double-clicked) will force the connect action to finish.
				base.OnClicked(e);
				return;
			}
			DiagramMouseEventArgs args = CurrentDiagramArgs as DiagramMouseEventArgs;
			if (args != null)
			{
				DiagramItem item = args.DiagramHitTestInfo.HitDiagramItem;
				ModelElement currentElement = null;
				foreach (ModelElement elem in item.RepresentedElements)
				{
					currentElement = elem;
					break;
				}
				ExternalConstraintShape constraintShape;
				Role role;
				if (null != (constraintShape = currentElement as ExternalConstraintShape))
				{
					if (mySourceShape == null)
					{
						// Let the click through to the base to officially begin the drag action
						base.OnClicked(e);
						mySourceShape = constraintShape;
						ORMDiagram ormDiagram;
						if (null != (ormDiagram = mySourceShape.Diagram as ORMDiagram))
						{
							ormDiagram.StickyObject = constraintShape;
						}
					}
				}
				else if (null != (role = currentElement as Role))
				{
					// Add or remove the role
					IList<Role> roles = SelectedRoleCollection;
					int roleIndex = roles.IndexOf(role);
					bool forceRedraw = false;
					int redrawIndexBound = -1;
					if (roleIndex >= 0)
					{
						// Only remove a role when the control key is down. Otherwise,
						// there is no way to double-click on a previously selected
						// role without turning it off, and this is a natural gesture.
						if (0 != (0xff00 & GetKeyState(Keys.ControlKey)))
						{
							forceRedraw = true;
							roles.RemoveAt(roleIndex);
							redrawIndexBound = roles.Count;
						}
					}
					else
					{
						forceRedraw = true;
						roles.Add(role);
						if (mySourceShape != null)
						{
							myPendingOnClickedAction = OnClickedAction.CheckForCommit;
						}
					}

					if (forceRedraw)
					{
						// Force the shape types to redraw
						RedrawOwningFactType(role);

						// Force anything with a later index to redraw as well.
						// These roles may be on different fact types than the
						// original.
						for (int i = roleIndex; i < redrawIndexBound; ++i)
						{
							RedrawOwningFactType(roles[i]);
						}
					}
				}
				else if (mySourceShape != null && currentElement is ORMDiagram)
				{
					base.OnClicked(e); // Let through to allow a cancel
					((ORMDiagram)currentElement).StickyObject = null;
				}
			}
		}
		private void RedrawOwningFactType(Role role)
		{
			PresentationElementMoveableCollection pels = role.FactType.PresentationRolePlayers;
			int pelsCount = pels.Count;
			for (int i = 0; i < pelsCount; ++i)
			{
				ShapeElement shape = pels[i] as ShapeElement;
				if (shape != null)
				{
					shape.Invalidate(true);
				}
			}
		}
		/// <summary>
		/// Redraw fact types for any selected roles, deselect the toolbox item,
		/// and chain to a new connect action if this one completed successfully.
		/// </summary>
		/// <param name="e">DiagramEventArgs</param>
		protected override void OnMouseActionDeactivated(DiagramEventArgs e)
		{
			base.OnMouseActionDeactivated(e);
			IList<Role> roles = mySelectedRoles;
			if (roles != null)
			{
				int roleCount = roles.Count;
				for (int i = 0; i < roleCount; ++i)
				{
					RedrawOwningFactType(roles[i]);
				}
			}

			// Set the selection back to pointer after connect action
			DiagramView activeView;
			IToolboxService toolbox;
			if ((null != (activeView = Diagram.ActiveDiagramView)) &&
				(null != (toolbox = activeView.Toolbox)))
			{
				toolbox.SelectedToolboxItemUsed();
			}

			// The ChainMouseAction call can reactivate this connect action,
			// so make sure we snapshot the state we need and do all requisite
			// cleanup before a potential reactivation.
			ExternalConstraintShape chainOnShape = (myPendingOnClickedAction == OnClickedAction.Complete) ? mySourceShape : null;
			Reset();
			if (chainOnShape != null)
			{
				// UNDONE: We should only do this if appropriate for the constraint type
				// and the current condition of the constraint
				ChainMouseAction(chainOnShape, e.DiagramClientView);
			}
		}
		/// <summary>
		/// Cancel if the last hit shape is the Diagram by not forwarding
		/// to the base class. Otherwise, complete the action.
		/// </summary>
		/// <param name="e">DiagramEventArgs</param>
		protected override void OnMouseActionCompleted(DiagramEventArgs e)
		{
			if (MouseDownHitShape == Diagram)
			{
				return; // Effect a cancel for a click on the diagram
			}
			using (Transaction t = Diagram.Store.TransactionManager.BeginTransaction(ResourceStrings.ExternalConstraintConnectActionTransactionName))
			{
				base.OnMouseActionCompleted(e);
				if (t.HasPendingChanges)
				{
					t.Commit();
				}
			}
			myPendingOnClickedAction = OnClickedAction.Complete;
		}
		#endregion // Base overrides
		#region ExternalConstraintConnectAction specific
		private void Reset()
		{
			myConstraintRoleSequence = null;
			myInitialSelectedRoles = null;
			mySelectedRoles = null;
			mySourceShape = null;
			myPendingOnClickedAction = OnClickedAction.Normal;
			FactTypeShape.ActiveExternalConstraintConnectAction = null;
		}
		/// <summary>
		/// Get the sequence of currently selected roles.
		/// </summary>
		/// <value>IList&lt;Role&gt;</value>
		[CLSCompliant(false)]
		public IList<Role> SelectedRoleCollection
		{
			get
			{
				if (mySelectedRoles == null)
				{
					mySelectedRoles = new List<Role>();
					FactTypeShape.ActiveExternalConstraintConnectAction = this;
				}
				return mySelectedRoles;
			}
		}
		/// <summary>
		/// Set the sequence of initially selected roles.
		/// </summary>
		/// <value>RoleMovableCollection</value>
		public ConstraintRoleSequence ConstraintRoleSequenceToEdit
		{
			set
			{
				myConstraintRoleSequence = value;
				RoleMoveableCollection roleCollection = myConstraintRoleSequence.RoleCollection;
				IList<Role> selectedRoleCollection = SelectedRoleCollection;
				IList<Role> initialRoles = InitialRoles;
				foreach (Role r in roleCollection)
				{
					selectedRoleCollection.Add(r);
					initialRoles.Add(r);
				}
			}
			get
			{
				return myConstraintRoleSequence;
			}
		}
		/// <summary>
		/// The initial roles that were in the role sequence that this ConnectAction is editing.
		/// </summary>
		/// <value>List&lt;Role&gt;</value>
		[CLSCompliant(false)]
		public IList<Role> InitialRoles
		{
			get
			{
				if (null == myInitialSelectedRoles)
				{
					myInitialSelectedRoles = new List<Role>();
					FactTypeShape.ActiveExternalConstraintConnectAction = this;
				}
				return myInitialSelectedRoles;
			}
		}
		/// <summary>
		/// If a role is actively selected, return the 0-based selection index,
		/// or -1 if the role is not actively selected.
		/// </summary>
		/// <param name="role">A role to test</param>
		/// <returns>0-based role index, or -1 if not selected</returns>
		public int GetActiveRoleIndex(Role role)
		{
			if (IsActive && mySelectedRoles != null)
			{
				return mySelectedRoles.IndexOf(role);
			}
			return -1;
		}
		/// <summary>
		/// Set this mouse action as the active action on the
		/// diagram of the given shape, and activate its drag line
		/// centered on the shape.
		/// </summary>
		/// <param name="attachToShape">The shape for the constraint
		/// being connected.</param>
		/// <param name="clientView">The active DiagramClientView</param>
		public void ChainMouseAction(ExternalConstraintShape attachToShape, DiagramClientView clientView)
		{
			DiagramView activeView = Diagram.ActiveDiagramView;
			if (activeView != null)
			{
				// Move on to the selection action
				clientView.ActiveMouseAction = this;

				// Now emulate a mouse click in the middle of the added constraint. The click
				// actions provide a starting point for the connect action, so a mouse move
				// provides a drag line.
				Point emulateClickPoint = clientView.WorldToDevice(attachToShape.AbsoluteCenter);
				DiagramMouseEventArgs mouseEventArgs = new DiagramMouseEventArgs(new MouseEventArgs(MouseButtons.Left, 1, emulateClickPoint.X, emulateClickPoint.Y, 0), clientView);
				MouseDown(mouseEventArgs);
				Click(new DiagramPointEventArgs(emulateClickPoint.X, emulateClickPoint.Y, PointRelativeTo.Client, clientView));
				MouseUp(mouseEventArgs);

				// An extra move lets us chain when the mouse is not on the design surface,
				// such as when we are being activated via the task list.
				MouseMove(mouseEventArgs);

				ORMDiagram.SelectToolboxItem(activeView, ResourceStrings.ToolboxExternalConstraintConnectorItemId);
			}
		}
		#endregion // ExternalConstraintConnectAction specific
	}
	/// <summary>
	/// A toolbox action to add an external constraint and activate
	/// the external constraint connect action
	/// </summary>
	[CLSCompliant(true)]
	public class ExternalConstraintAction : ToolboxAction
	{
		#region Member variables
		/// <summary>
		/// An event that fires after the standard MouseActionDeactivated event. The
		/// standard event clears the toolbox, this one can be used to chain this
		/// action to a new standard action.
		/// </summary>
		public event MouseActionDeactivatedEventHandler AfterMouseActionDeactivated;
		private ExternalConstraintShape myAddedConstraintShape;
		#endregion // Member variables
		#region Constructors
		/// <summary>
		/// Construct a new constraint action for a specific diagram. The
		/// same connect action can be reused multiple times by activating and
		/// deactivating it.
		/// </summary>
		/// <param name="diagram">The owning diagram</param>
		public ExternalConstraintAction(Diagram diagram) : base(diagram)
		{
			Reset();
		}
		#endregion // Constructors
		#region ExternalConstraintAction specific
		/// <summary>
		/// Central function to return member variables to a clean state.
		/// Called by the constructor and the deactivation sequence.
		/// </summary>
		private void Reset()
		{
			myAddedConstraintShape = null;
		}
		/// <summary>
		/// Add events to the store during connect action
		/// activation. The default implementation watches for
		/// new external constraints added to the diagram.
		/// </summary>
		/// <param name="store">Store</param>
		protected virtual void AddStoreEvents(Store store)
		{
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventManager = store.EventManagerDirectory;

			MetaClassInfo classInfo = dataDirectory.FindMetaClass(ExternalConstraintShape.MetaClassGuid);
			eventManager.ElementAdded.Add(classInfo, new ElementAddedEventHandler(ExternalConstraintShapeAddedEvent));
		}
		/// <summary>
		/// Removed any events added during the AddStoreEvents methods
		/// </summary>
		/// <param name="store">Store</param>
		protected virtual void RemoveStoreEvents(Store store)
		{
			MetaDataDirectory dataDirectory = store.MetaDataDirectory;
			EventManagerDirectory eventManager = store.EventManagerDirectory;

			MetaClassInfo classInfo = dataDirectory.FindMetaClass(ExternalConstraintShape.MetaClassGuid);
			eventManager.ElementAdded.Remove(classInfo, new ElementAddedEventHandler(ExternalConstraintShapeAddedEvent));
		}
		/// <summary>
		/// An IMS event to track the shape element added to the associated
		/// diagram during this connect action.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ExternalConstraintShapeAddedEvent(object sender, ElementAddedEventArgs e)
		{
			if (myAddedConstraintShape == null)
			{
				ExternalConstraintShape candidate = e.ModelElement as ExternalConstraintShape;
				// Make sure the shape was added to the diagram associated with this
				// connect action
				if (candidate != null && candidate.Diagram == Diagram)
				{
					myAddedConstraintShape = candidate;
				}
			}
		}
		/// <summary>
		/// The constraint shape added as a result of a completed mouse action
		/// </summary>
		public ExternalConstraintShape AddedConstraintShape
		{
			get
			{
				return myAddedConstraintShape;
			}
		}
		/// <summary>
		/// Was this action completed successfully?
		/// </summary>
		public bool ActionCompleted
		{
			get
			{
				return myAddedConstraintShape != null;
			}
		}
		#endregion // ExternalConstraintAction specific
		#region Base overrides
		/// <summary>
		/// Add an event on the store so we can track
		/// the shape for an external constraint added during
		/// the transaction resulting from completion of the mouse
		/// action.
		/// </summary>
		/// <param name="e">DiagramEventArgs</param>
		protected override void OnMouseActionActivated(DiagramEventArgs e)
		{
			AddStoreEvents(Diagram.Store);
		}
		/// <summary>
		/// Deactivate the mouse action by removing the listening events,
		/// call the base, then firing off our own AfterMouseActionDeactivated
		/// event, if it is set.
		/// </summary>
		/// <param name="e">DiagramEventArgs</param>
		protected override void OnMouseActionDeactivated(DiagramEventArgs e)
		{
			RemoveStoreEvents(e.DiagramClientView.Diagram.Store);
			base.OnMouseActionDeactivated(e);
			MouseActionDeactivatedEventHandler handler = AfterMouseActionDeactivated;
			if (handler != null)
			{
				handler(this, e);
			}
			Reset();
		}
		#endregion // Base overrides
	}
}
