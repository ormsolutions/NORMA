#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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
using ORMSolutions.ORMArchitect.Framework.Diagrams;

namespace ORMSolutions.ORMArchitect.Core.ShapeModel
{
	#region RoleDragPendingAction Class
	/// <summary>
	/// A class similar to the DragDropPendingAction class,
	/// except we don't touch the cursor, and we chain to
	/// the RoleConnectAction when dragging starts.
	/// </summary>
	public class RoleDragPendingAction : SelectAction
	{
		/// <summary>
		/// Create a new RoleDragDropPendingAction. Should be
		/// called once per diagram
		/// </summary>
		/// <param name="diagram">The owning diagram</param>
		public RoleDragPendingAction(Diagram diagram) : base(diagram)
		{
		}
		/// <summary>
		/// If we've reached the dragging state, then
		/// chaing to the toolbox action.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(DiagramMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && CurrentMouseActionState == DraggingState)
			{
				DiagramClientView clientView = e.DiagramClientView;
				// Stop this mouse action
				Complete(clientView);

				// Chain to other action
				ORMDiagram diagram = (ORMDiagram)clientView.Diagram;
				diagram.RoleConnectAction.ChainMouseAction(this.MouseDownPoint, clientView, true);
			}
		}
		private bool myAllowDoubleClick;
		/// <summary>
		/// Track information for OnDoubleClick
		/// </summary>
		protected override void OnMouseDown(DiagramMouseEventArgs e)
		{
			myAllowDoubleClick = e.Clicks == 2 && e.Button == MouseButtons.Left;
			base.OnMouseDown(e);
		}
		/// <summary>
		/// If the role object is part of a sticky external constraint shape
		/// object then activate it on a double click. Equivalent to the
		/// 'ActiveRoleSequence' diagram command.
		/// </summary>
		protected override void OnDoubleClick(DiagramPointEventArgs e)
		{
			// Note this looks like a strange place to put this, but this
			// is the mouse action that is active when a role is clicked on.
			if (myAllowDoubleClick)
			{
				ORMDiagram ormDiagram = Diagram as ORMDiagram;
				IStickyObject sticky;
				ExternalConstraintShape constraintShape;
				if (null != (sticky = ormDiagram.StickyObject) &&
					null != (constraintShape = sticky as ExternalConstraintShape))
				{
					foreach (Role role in e.DiagramHitTestInfo.HitDiagramItem.RepresentedElements)
					{
						if (sticky.StickySelectable(role))
						{
							IConstraint constraint = constraintShape.AssociatedConstraint;
							Role constraintRole = role;
							Role oppositeRole;
							ObjectType oppositeRolePlayer;
							if (constraint.ConstraintType == ConstraintType.ExternalUniqueness &&
								null != (oppositeRole = role.OppositeRole as Role) &&
								null != (oppositeRolePlayer = oppositeRole.RolePlayer) &&
								oppositeRolePlayer.IsImplicitBooleanValue)
							{
								constraintRole = oppositeRole;
							}
							foreach (ConstraintRoleSequence sequence in constraintRole.ConstraintRoleSequenceCollection)
							{
								if (constraint == sequence.Constraint)
								{
									ExternalConstraintConnectAction connectAction = ormDiagram.ExternalConstraintConnectAction;
									connectAction.ConstraintRoleSequenceToEdit = sequence;
									connectAction.ChainMouseAction(constraintShape, e.DiagramClientView);
									e.Handled = true;
									return;
								}
							}
						}
						break;
					}
				}
			}
			base.OnDoubleClick(e);
		}
	}
	#endregion // RoleDragPendingAction Class
	#region RoleConnectAction class
	/// <summary>
	/// A connect action for attaching a role to
	/// its role player.
	/// </summary>
	public class RoleConnectAction : ConnectAction
	{
		#region ExternalConstraintConnectionType class
		/// <summary>
		/// The ConnectionType used with this ConnectAction. The type
		/// is a singleton, holds all of the context-independent logic,
		/// and operates directly on shape elements.
		/// </summary>
		protected class RoleConnectionType : ConnectionType
		{
			/// <summary>
			/// The singleton RoleConnectionType instance
			/// </summary>
			public static new readonly RoleConnectionType Instance = new RoleConnectionType();
			/// <summary>
			/// An array of one element containing the singleton RoleConnectionType instance
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
				ORMDiagram diagram = (ORMDiagram)sourceShapeElement.Diagram;
				RoleConnectAction action = diagram.RoleConnectAction;
				action.myRoleReorderConnector = false;
				ObjectType objectType;
				Role sourceRole = action.mySourceRole;
				Role role;
				Role targetRole;
				Objectification objectification;
				// Reorder roles in the same shape
				if (sourceRole != null &&
					sourceShapeElement == targetShapeElement &&
					null != (targetRole = action.myLastMouseMoveRole) &&
					targetRole != sourceRole)
				{
					action.myRoleReorderConnector = true;
					return true;
				}
				else if ((null != (role = sourceRole) && null != (objectType = ObjectTypeFromShape(targetShapeElement)) &&
					(null == (objectification = objectType.Objectification) || !objectification.IsImplied)) ||
					(null != (objectType = action.mySourceObjectType) && null != (role = action.myLastMouseMoveRole)))
				{
					return role.FactType != objectType.NestedFactType;
				}
				// Allow the user to drag out an existing role player from a
				// role on a shape that is not connected.
				else if (targetShapeElement == diagram &&
					action.mySourceRoleMissingConnector)
				{
					return true;
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
				// Everything is handled in IsValidSourceAndTarget. A warning
				// about connecting a role to its nesting object type would be
				// annoying because it would be hit dragging an objectified role
				// out of its own fact type. This condition is checked in IsValidSourceAndTarget
				return true;
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
				ORMDiagram diagram = (ORMDiagram)sourceShapeElement.Diagram;
				RoleConnectAction action = diagram.RoleConnectAction;
				ObjectType objectType;
				Role sourceRole = action.mySourceRole;
				Role role;
				Role targetRole;
				FactTypeShape factTypeShape;
				if (sourceRole != null &&
					sourceShapeElement == targetShapeElement &&
					null != (targetRole = action.myLastMouseMoveRole) &&
					targetRole != sourceRole &&
					null != (factTypeShape = sourceShapeElement as FactTypeShape))
				{
					LinkedElementCollection<RoleBase> displayedRoles = factTypeShape.GetEditableDisplayRoleOrder();
					displayedRoles.Move(displayedRoles.IndexOf(sourceRole), displayedRoles.IndexOf(targetRole));
				}
				else if ((null != (role = sourceRole) && null != (objectType = ObjectTypeFromShape(targetShapeElement))) ||
					(null != (objectType = action.mySourceObjectType) && null != (role = action.myLastMouseMoveRole)))
				{
					// Don't trigger a change if none is indicated. Turn this into a noop
					if (role.RolePlayer != objectType)
					{
						role.RolePlayer = objectType;
					}
				}
				else if (targetShapeElement == diagram &&
					action.mySourceRoleMissingConnector)
				{
					diagram.PlaceORMElementOnDiagram(null, role.RolePlayer, paintFeedbackArgs.TargetFeedbackBounds.Location, ORMPlacementOption.AllowMultipleShapes, null, null);
				}
			}
			/// <summary>
			/// Provide the transaction name. The name is displayed in the undo and redo lists.
			/// </summary>
			public override string GetConnectTransactionName(ShapeElement sourceShape, ShapeElement targetShape)
			{
				return (targetShape is ORMDiagram) ? ResourceStrings.DropShapeTransactionName : ResourceStrings.RoleConnectActionTransactionName;
			}
		}
		#endregion // ExternalConstraintConnectionType class
		#region Member variables
		// The following cursors are built as embedded resources. Pick them up by their file name.
		private static Cursor myAllowedCursor = new Cursor(typeof(RoleConnectAction), "ConnectRoleAllowed.cur");
		private static Cursor myAllowedReorderCursor = new Cursor(typeof(RoleConnectAction), "ConnectRoleReorderAllowed.cur");
		private static Cursor mySearchingCursor = new Cursor(typeof(RoleConnectAction), "ConnectRoleSearching.cur");
		private static readonly ConnectionType[] EmptyConnectionTypes = {};
		private Role myLastMouseDownRole;
		private Role myLastMouseMoveRole;
		private Role mySourceRole;
		private bool mySourceRoleMissingConnector;
		private bool myRoleReorderConnector;
		private ObjectType mySourceObjectType;
		private bool myEmulateDrag;
		#endregion // Member variables
		#region Constructors
		/// <summary>
		/// Create a role connector action for the given diagram. One
		/// action per diagram should be sufficient.
		/// </summary>
		/// <param name="diagram">The hosting diagram</param>
		public RoleConnectAction(Diagram diagram) : base(diagram, true)
		{
			Reset();
		}
		#endregion // Constructors
		#region Base overrides
		/// <summary>
		/// Retrieve all connect types associated with this connect action.
		/// Returns an empty array unless the sourceShapeElement is an ExternalConstraintShape
		/// </summary>
		/// <param name="sourceShapeElement">The source element</param>
		/// <param name="targetShapeElement">The target element. Currently ignored.</param>
		/// <returns></returns>
		protected override ConnectionType[] GetConnectionTypes(ShapeElement sourceShapeElement, ShapeElement targetShapeElement)
		{
			return RoleConnectionType.InstanceArray;
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
			if (mySourceObjectType == null && mySourceRole == null)
			{
				Role role = myLastMouseDownRole;
				if (role != null)
				{
					mySourceRole = role;
					FactTypeShape factShape;
					if (null != role.RolePlayer &&
						null != (factShape = MouseDownHitShape as FactTypeShape))
					{
						bool haveConnectorForRole = false;
						foreach (RolePlayerLink connector in MultiShapeUtility.GetEffectiveAttachedLinkShapesFrom<RolePlayerLink>(factShape))
						{
							if (connector.AssociatedRolePlayerLink.PlayedRole == role)
							{
								haveConnectorForRole = true;
								break;
							}
						}
						mySourceRoleMissingConnector = !haveConnectorForRole;
					}
				}
				else
				{
					mySourceObjectType = ObjectTypeFromShape(MouseDownHitShape);
				}
			}
		}
		/// <summary>
		/// If the current item is a role, then cache it as the
		/// last mouse down hit role
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(DiagramMouseEventArgs e)
		{
			myLastMouseDownRole = HitRole(e);
			base.OnMouseDown(e);
		}
		/// <summary>
		/// If the current item is a role, then cache it as the
		/// last mouse mouse hit role
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(DiagramMouseEventArgs e)
		{
			myLastMouseMoveRole = HitRole(e);
			base.OnMouseMove(e);
		}
		/// <summary>
		/// If we're emulating a drag (occurs when we're chained from RoleDragPendingAction),
		/// then complete the action on mouse up
		/// </summary>
		/// <param name="e">DiagramMouseEventArgs</param>
		protected override void OnMouseUp(DiagramMouseEventArgs e)
		{
			if (myEmulateDrag && (mySourceObjectType != null || mySourceRole != null))
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
					cursor = myRoleReorderConnector ? myAllowedReorderCursor : myAllowedCursor;
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
		#region RoleConnectAction specific
		private void Reset()
		{
			mySourceRole = null;
			mySourceRoleMissingConnector = false;
			myRoleReorderConnector = false;
			mySourceObjectType = null;
			myLastMouseDownRole = null;
			myLastMouseMoveRole = null;
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

				// Now emulate a mouse click in the middle of the added constraint. The click
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
				ORMDiagram.SelectToolboxItem(activeView, ResourceStrings.ToolboxRoleConnectorItemId);
			}
		}
		/// <summary>
		/// Helper function to find the role represented
		/// at the given diagram position.
		/// </summary>
		/// <param name="e"></param>
		/// <returns>Hit role, or null</returns>
		private static Role HitRole(DiagramMouseEventArgs e)
		{
			Role retVal = null;
			DiagramItem item = e.DiagramHitTestInfo.HitDiagramItem;
			if (item != null)
			{
				foreach (ModelElement element in item.RepresentedElements)
				{
					retVal = element as Role;
				}
			}
			return retVal;
		}
		/// <summary>
		/// Get the underlying object type for a ShapeElement
		/// </summary>
		/// <param name="shape">A shape element.</param>
		/// <returns>An ObjectType, or null</returns>
		protected static ObjectType ObjectTypeFromShape(ShapeElement shape)
		{
			ObjectType objectType;
			if (shape == null)
			{
				// Protect against breaking into the debugger, should
				// not be hit without a debugger active.
				return null;
			}
			ModelElement backingElement = shape.ModelElement;
			FactType factType;
			if (null == (objectType = backingElement as ObjectType))
			{
				if (null != (factType = backingElement as FactType))
				{
					objectType = factType.NestingType;
				}
			}
			return objectType;
		}
		#endregion // RoleConnectAction specific
	}
	#endregion // RoleConnectAction class
}  
