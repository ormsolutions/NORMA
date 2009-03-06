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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	/// <summary>
	/// A ConnectAction to add role sequences to an Internal constraint
	/// </summary>
	public class InternalUniquenessConstraintConnectAction : ConnectAction
	{
		#region InternalUniquenessConstraintConnectionType class
		/// <summary>
		/// The ConnectionType used with this ConnectAction. The type
		/// is a singleton, holds all of the context-independent logic,
		/// and operates directly on shape elements.
		/// </summary>
		protected class InternalUniquenessConstraintConnectionType : ConnectionType
		{
			/// <summary>
			/// The singleton InternalConstraintConnectionType instance
			/// </summary>
			public static new readonly InternalUniquenessConstraintConnectionType Instance = new InternalUniquenessConstraintConnectionType();
			/// <summary>
			/// An array of one element containing the singleton InternalUniquenessConstraintConnectionType instance
			/// </summary>
			public static readonly ConnectionType[] InstanceArray = { Instance };
			/// <summary>
			/// Enforce use static properties to get singleton.
			/// </summary>
			protected InternalUniquenessConstraintConnectionType() { }
			/// <summary>
			/// Called as the pointer is moved over potential targets after a source is selected
			/// So should be pretty quick
			/// </summary>
			/// <remarks>
			/// The cursor can change dependant on CanCreateConnection when this returns true
			/// When this returns false, the control falls back to
			/// InternalUniquenessConstraintConectAction.
			/// </remarks>
			/// <param name="sourceShapeElement">ShapeElement</param>
			/// <param name="targetShapeElement">ShapeElement</param>
			/// <returns></returns>
			public override bool IsValidSourceAndTarget(ShapeElement sourceShapeElement, ShapeElement targetShapeElement)
			{
				// The source and target shapes are allowed here so we can display instructions in CanCreateConnection
				return (targetShapeElement == sourceShapeElement.Diagram || targetShapeElement == sourceShapeElement);
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
				if (sourceShapeElement is FactTypeShape)
				{
					if (sourceShapeElement == targetShapeElement)
					{
						// UNDONE: Constrain this, this is overly generous
						retVal = true;
					}
					else
					{
						Debug.Assert(IsValidSourceAndTarget(sourceShapeElement, targetShapeElement)); // The condition that got us here
						if (targetShapeElement == sourceShapeElement.Diagram)
						{
							connectionWarning = ResourceStrings.InternalUniquenessConstraintConnectActionInstructions;
						}
					}
				}
				return retVal;
			}
			/// <summary>
			/// Create a connection between a FactTypeShape and a FactType. Roles
			/// used in the connection are stored with the currently active connect action.
			/// </summary>
			/// <param name="sourceShapeElement">The source of the requested connection</param>
			/// <param name="targetShapeElement">The target of the requested connection</param>
			/// <param name="paintFeedbackArgs">PaintFeedbackArgs</param>
			public override void CreateConnection(ShapeElement sourceShapeElement, ShapeElement targetShapeElement, PaintFeedbackArgs paintFeedbackArgs)
			{
				IConstraint constraint;
				InternalUniquenessConstraintConnectAction action;
				IList<Role> selectedRoles;
				int rolesCount;
				if ((null != (action = (sourceShapeElement.Diagram as ORMDiagram).InternalUniquenessConstraintConnectAction)) &&
					(null != (constraint = action.ActiveConstraint)) &&
					(null != (selectedRoles = action.SelectedRoleCollection)) &&
					(0 != (rolesCount = selectedRoles.Count)))
				{
					UniquenessConstraint iuConstraint;
					if (null != (iuConstraint = constraint as UniquenessConstraint) &&
						iuConstraint.IsInternal)
					{
						// Keep the collection ordered, this ends up as constraint order on objectified FactTypes
						LinkedElementCollection<Role> roles = iuConstraint.RoleCollection;
						int existingRolesCount = roles.Count;
						for (int i = existingRolesCount - 1; i >= 0; --i)
						{
							Role testRole = roles[i];
							if (!selectedRoles.Contains(testRole))
							{
								roles.Remove(testRole);
								--existingRolesCount;
							}
						}
						for (int i = 0; i < rolesCount; ++i)
						{
							Role selectedRole = selectedRoles[i];
							int existingIndex = roles.IndexOf(selectedRole);
							if (existingIndex == -1)
							{
								if (i < existingRolesCount)
								{
									roles.Insert(i, selectedRole);
								}
								else if (!roles.Contains(selectedRole))
								{
									roles.Add(selectedRole);
								}
								++existingRolesCount;
							}
							else if (existingIndex != i)
							{
								roles.Move(existingIndex, i);
							}
						}
					}
				}
			}
			/// <summary>
			/// Provide the transaction name. The name is displayed in the undo and redo lists.
			/// </summary>
			public override string GetConnectTransactionName(ShapeElement sourceShape, ShapeElement targetShape)
			{
				return ResourceStrings.InternalUniquenessConstraintConnectActionTransactionName;
			}
			/// <summary>
			/// Controls the PaintFeedbackArgs for while building an internal uniqueness constraint.
			/// </summary>
			/// <param name="sourceShapeElement">The shape that the internal uniqueness constraint is being added to.</param>
			/// <param name="sourcePoint">The point where the internal uniqueness constraint was added.</param>
			/// <param name="targetShapeElement">The shape that the mouse is currently over.</param>
			/// <param name="targetPoint">The point that the mouse is currently over.</param>
			/// <param name="paintFeedbackArgs">The current PaintFeedbackArgs.</param>
			/// <returns>The modified PaintFeedbackArgs</returns>
			public override PaintFeedbackArgs UpdatePaintFeedbackParameters(ShapeElement sourceShapeElement, PointD sourcePoint, ShapeElement targetShapeElement, PointD targetPoint, PaintFeedbackArgs paintFeedbackArgs)
			{
				PaintFeedbackArgs args = base.UpdatePaintFeedbackParameters(sourceShapeElement, sourcePoint, targetShapeElement, targetPoint, paintFeedbackArgs);
				args.DisplaySourceAndTargetFeedback = false;
				args.TargetConnectionPoint = args.SourceConnectionPoint;
				return args;
			}
		}
		#endregion // InternalUniquenessConstraintConnectionType class
		#region Member variables
		// The following cursors are built as embedded resources. Pick them up by their file name.
		private static Cursor myAllowedCursor = new Cursor(typeof(InternalUniquenessConstraintConnectAction), "ConnectInternalConstraintAllowed.cur");
		private static Cursor mySearchingCursor = new Cursor(typeof(InternalUniquenessConstraintConnectAction), "ConnectInternalConstraintSearching.cur");
		private IList<Role> mySelectedRoles;
		private FactTypeShape mySourceShape;
		private UniquenessConstraint myIUC;
		private DiagramItem myLastMouseMoveItem;
		private static readonly ConnectionType[] EmptyConnectionTypes = {};
		private enum OnClickedAction
		{
			Normal,
			CheckForCommit,
			Commit,
			Complete, // Not necessarily a set with the others, but we don't care about other values after completion
		}

		/// <summary>
		/// Gets the FactTypeShape this internal uniqueness constraint is on.
		/// </summary>
		public FactTypeShape SourceShape
		{
			get
			{
				return mySourceShape;
			}
		}
		private OnClickedAction myPendingOnClickedAction;
		#endregion // Member variables
		#region Constructors
		/// <summary>
		/// Create an Internal constraint action for the given diagram. One
		/// action per diagram should be sufficient.
		/// </summary>
		/// <param name="diagram">The hosting diagram</param>
		public InternalUniquenessConstraintConnectAction(Diagram diagram) : base(diagram, true)
		{
			Reset();
		}
		/// <summary>
		/// Retrieve all connect types associated with this connect action.
		/// Returns an empty array unless the sourceShapeElement is a FactTypeShape
		/// </summary>
		/// <param name="sourceShapeElement">The source element</param>
		/// <param name="targetShapeElement">The target element. Currently ignored.</param>
		/// <returns></returns>
		protected override ConnectionType[] GetConnectionTypes(ShapeElement sourceShapeElement, ShapeElement targetShapeElement)
		{
			if (sourceShapeElement is FactTypeShape)
			{
				return InternalUniquenessConstraintConnectionType.InstanceArray;
			}
			return EmptyConnectionTypes;
		}
		/// <summary>
		/// Get a cursor from the cursor type. Returns the searching cursor for
		/// everything except the allowed action.
		/// </summary>
		/// <param name="connectActionCursor">The requested cursor style</param>
		/// <returns></returns>
		protected override Cursor GetCursorFromCursorType(ConnectActionCursor connectActionCursor)
		{
			Cursor cursor = null;
			switch (connectActionCursor)
			{
				case ConnectActionCursor.Allowed:
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
					break;
				case ConnectActionCursor.Searching:
				case ConnectActionCursor.Warning:
					cursor = Cursors.No;
					break;
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
			switch (myPendingOnClickedAction)
			{
				case OnClickedAction.Commit:
					myPendingOnClickedAction = OnClickedAction.Normal;
					// Letting the click through to the base ConnectAction
					// at this point (a constraint is selected and a role has been
					// double-clicked) will force the connect action to finish.
					base.OnClicked(e);
					return;
				case OnClickedAction.CheckForCommit:
					myPendingOnClickedAction = OnClickedAction.Normal;
					break;
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
				UniquenessConstraint internalUniquenessConstraint;
				Role role;
				if (null != (internalUniquenessConstraint = currentElement as UniquenessConstraint) &&
					internalUniquenessConstraint.IsInternal)
				{
					if (mySourceShape == null)
					{
						// Let the click through to the base to officially begin the drag action
						base.OnClicked(e);
						mySourceShape = item.Shape as FactTypeShape;
						myIUC = internalUniquenessConstraint;
					}
				}
				else if (mySourceShape != null)
				{
					if (null != (role = currentElement as Role))
					{
						if (role.FactType == mySourceShape.AssociatedFactType)
						{
							// Add or remove the role
							IList<Role> roles = SelectedRoleCollection;
							int roleIndex = roles.IndexOf(role);
							bool forceRedraw = false;
							if (roleIndex >= 0)
							{
								// Only remove a role when the control key is down. Otherwise,
								// there is no way to double-click on a previously selected
								// role without turning it off, and this is a natural gesture.
								// Add shift key as well for discoverability.
								if (0 != (0xff00 & GetKeyState(Keys.ControlKey)) ||
									0 != (0xff00 & GetKeyState(Keys.ShiftKey)))
								{
									forceRedraw = true;
									roles.RemoveAt(roleIndex);
								}
							}
							else
							{
								forceRedraw = true;
								roles.Add(role);
							}
							if (mySourceShape != null)
							{
								myPendingOnClickedAction = OnClickedAction.CheckForCommit;
							}

							if (forceRedraw)
							{
								// Force the shape to redraw
								Debug.Assert(mySourceShape != null); //source shape should have been set
								mySourceShape.Invalidate(true);
							}
						}
					}
					else if (currentElement is ORMDiagram)
					{
						base.OnClicked(e); // Let through to allow a cancel
					}
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
			if (mySourceShape != null)
			{
				mySourceShape.Invalidate(true);
			}

			// Set the selection back to pointer after connect action
			DiagramView activeView;
			IToolboxService toolbox;
			if ((null != (activeView = Diagram.ActiveDiagramView)) &&
				(null != (toolbox = activeView.Toolbox)))
			{
				toolbox.SelectedToolboxItemUsed();
			}

			// Do all requisite cleanup before a potential reactivation.
			Reset();
		}
		/// <summary>
		/// Cancel if the last hit shape is the Diagram by not forwarding
		/// to the base class. Otherwise, complete the action.
		/// </summary>
		/// <param name="e">DiagramEventArgs</param>
		protected override void OnMouseActionCompleted(DiagramEventArgs e)
		{
			base.OnMouseActionCompleted(e);
			myPendingOnClickedAction = OnClickedAction.Complete;
		}
		/// <summary>
		/// Override DoPaintFeedback in order to stop connect line from drawing when mouse
		/// is over invalid object. We won't do anything.
		/// </summary>
		/// <param name="e"></param>
		public override void DoPaintFeedback(DiagramPaintEventArgs e)
		{
		}
		#endregion // Base overrides
		#region InternalUniquenessConstraintConnectAction specific
		private void Reset()
		{
			mySelectedRoles = null;
			mySourceShape = null;
			myIUC = null;
			myLastMouseMoveItem = null;
			myPendingOnClickedAction = OnClickedAction.Normal;
			FactTypeShape.ActiveInternalUniquenessConstraintConnectAction = null;
		}
		/// <summary>
		/// Gets and sets the selected roles.
		/// </summary>
		/// <value></value>
		public IList<Role> SelectedRoleCollection
		{
			get
			{
				if (mySelectedRoles == null)
				{
					mySelectedRoles = new List<Role>();
					FactTypeShape.ActiveInternalUniquenessConstraintConnectAction = this;
				}
				return mySelectedRoles;
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
		/// The constraint that acts as the Source object for this mouse action
		/// </summary>
		public UniquenessConstraint ActiveConstraint
		{
			get
			{
				return myIUC;
			}
		}
		/// <summary>
		/// Set this mouse action as the active action on the
		/// diagram of the given shape.
		/// </summary>
		/// <param name="attachToShape">The shape the constraint is being attached to.</param>
		/// <param name="constraint">The constraint being connected.</param>
		/// <param name="clientView">The active DiagramClientView</param>
		public void ChainMouseAction(FactTypeShape attachToShape, UniquenessConstraint constraint, DiagramClientView clientView)
		{
			DiagramView activeView = Diagram.ActiveDiagramView;
			if (activeView != null)
			{
				// Move on to the selection action
				clientView.ActiveMouseAction = this;

				// Now emulate a mouse click in the middle of the added constraint. The click
				// actions provide a starting point for the connect action, so a mouse move
				// provides a drag line.
				Point emulateClickPoint = clientView.WorldToDevice(attachToShape.GetAbsoluteConstraintAttachPoint(constraint));
				DiagramMouseEventArgs mouseEventArgs = new DiagramMouseEventArgs(new MouseEventArgs(MouseButtons.Left, 1, emulateClickPoint.X, emulateClickPoint.Y, 0), clientView);
				MouseDown(mouseEventArgs);
				Click(new DiagramPointEventArgs(emulateClickPoint.X, emulateClickPoint.Y, PointRelativeTo.Client, clientView));
				MouseUp(mouseEventArgs);
				attachToShape.Invalidate(true);

				// An extra move lets us chain when the mouse is not on the design surface,
				// such as when we are being activated via the task list.
				MouseMove(mouseEventArgs);

				ORMDiagram.SelectToolboxItem(activeView, ResourceStrings.ToolboxInternalUniquenessConstraintItemId);
				FactTypeShape.ActiveInternalUniquenessConstraintConnectAction = this;
			}
		}
		#endregion // InternalUniquenessConstraintConnectAction specific
	}
	/// <summary>
	/// A toolbox action to add an Internal constraint
	/// </summary>
	public class InternalUniquenessConstraintAction : ToolboxAction
	{
		#region Member variables
		/// <summary>
		/// An event that fires after the standard MouseActionDeactivated event. The
		/// standard event clears the toolbox, this one can be used to chain this
		/// action to a new standard action.
		/// </summary>
		public event MouseActionDeactivatedEventHandler AfterMouseActionDeactivated;
		private UniquenessConstraint myAddedConstraint;
		private FactTypeShape myDropTargetShape;
		#endregion // Member variables
		#region Constructors
		/// <summary>
		/// Construct a new constraint action for a specific diagram. The
		/// same connect action can be reused multiple times by activating and
		/// deactivating it.
		/// </summary>
		/// <param name="diagram">The owning diagram</param>
		public InternalUniquenessConstraintAction(Diagram diagram) : base(diagram)
		{
			Reset();
		}
		#endregion // Constructors
		#region InternalConstraintAction specific
		/// <summary>
		/// Central function to return member variables to a clean state.
		/// Called by the constructor and the deactivation sequence.
		/// </summary>
		private void Reset()
		{
			myAddedConstraint = null;
			myDropTargetShape = null;
		}
		/// <summary>
		/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> during connect action activation and deactivation.
		/// The default implementation watches for new <see cref="UniquenessConstraint"/>s added to the <see cref="ORMModel"/>.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		protected virtual void ManageStoreEvents(Store store, EventHandlerAction action)
		{
			if (store == null || store.Disposed)
			{
				return; // bail out
			}
			ModelingEventManager.GetModelingEventManager(store).AddOrRemoveHandler(store.DomainDataDirectory.FindDomainClass(UniquenessConstraint.DomainClassId), new EventHandler<ElementAddedEventArgs>(InternalConstraintAddedEvent), action);
		}
		/// <summary>
		/// An IMS event to track the shape element added to the associated
		/// diagram during this connect action.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void InternalConstraintAddedEvent(object sender, ElementAddedEventArgs e)
		{
			if (myAddedConstraint == null)
			{
				UniquenessConstraint candidate = e.ModelElement as UniquenessConstraint;
				if (candidate != null && candidate.IsInternal)
				{
					ORMDiagram d = Diagram as ORMDiagram;
					if (d != null)
					{
						// Find the shape associated with the fact type we added to
						LinkedElementCollection<FactType> candidateFacts = candidate.FactTypeCollection;
						if (candidateFacts.Count != 0)
						{
							FactTypeShape shape = d.FindShapeForElement(candidateFacts[0]) as FactTypeShape;
							if (shape != null)
							{
								myDropTargetShape = shape;
								myAddedConstraint = candidate;
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// The fact type shape added to as a result of a completed mouse action
		/// </summary>
		public FactTypeShape DropTargetShape
		{
			get
			{
				return myDropTargetShape;
			}
		}
		/// <summary>
		/// The internal uniqueness constraint added as a result of a completed mouse action
		/// </summary>
		public UniquenessConstraint AddedConstraint
		{
			get
			{
				return myAddedConstraint;
			}
		}
		/// <summary>
		/// Was this action completed successfully?
		/// </summary>
		public bool ActionCompleted
		{
			get
			{
				return myAddedConstraint != null;
			}
		}
		#endregion // InternalConstraintAction specific
		#region Base overrides
		/// <summary>
		/// Adds an <see cref="EventHandler{TEventArgs}"/> on the <see cref="Store"/> so we can track the shape for the <see cref="UniquenessConstraint"/>
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
}
