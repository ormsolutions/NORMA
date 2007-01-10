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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.Modeling;

namespace Neumont.Tools.ORM.ShapeModel
{
	/// <summary>
	/// A ConnectAction to add role sequences to an external constraint
	/// </summary>
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
					// The source and target shapes are allowed here so we can display instructions in CanCreateConnection
					retVal = targetShapeElement is FactTypeShape ||
						targetShapeElement is SubtypeLink ||
						sourceShapeElement == targetShapeElement;
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
					bool isFactTypeShape;
					if ((isFactTypeShape = targetShapeElement is FactTypeShape) || targetShapeElement is SubtypeLink)
					{
						ExternalConstraintConnectAction action = (sourceShapeElement.Diagram as ORMDiagram).ExternalConstraintConnectAction;
						if (action != null)
						{
							if (action.mySubtypeConnection)
							{
								retVal = !isFactTypeShape;
							}
							else if (isFactTypeShape || (action.myAllowSubtypeConnection && action.SelectedRoleCollection.Count == 0))
							{
								retVal = true;
							}
						}
					}
					else
					{
						Debug.Assert(IsValidSourceAndTarget(sourceShapeElement, targetShapeElement)); // The condition that got us here
						connectionWarning = ResourceStrings.ExternalConstraintConnectActionInstructions;
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
					SetComparisonConstraint mcConstraint;
					SetConstraint scConstraint;
					ConstraintRoleSequence modifyRoleSequence = null;
					if (null != (mcConstraint = constraint as SetComparisonConstraint))
					{
						ConstraintRoleSequence constraintRoleSequenceBeingEdited = action.ConstraintRoleSequenceToEdit;
						// Add a new role set
						if (null == constraintRoleSequenceBeingEdited)
						{
							LinkedElementCollection<SetComparisonConstraintRoleSequence> roleSequences = mcConstraint.RoleSequenceCollection;
							if (action.mySubtypeConnection)
							{
								// All editing is done as a single column, add role sequences to the constraint
								// instead of roles to the sequence
								Store store = mcConstraint.Store;
								for (int i = 0; i < rolesCount; ++i)
								{
									SetComparisonConstraintRoleSequence roleSequence = new SetComparisonConstraintRoleSequence(store);
									roleSequence.RoleCollection.Add(selectedRoles[i]);
									roleSequences.Add(roleSequence);
								}
							}
							else
							{
								SetComparisonConstraintRoleSequence roleSequence = new SetComparisonConstraintRoleSequence(mcConstraint.Store);
								LinkedElementCollection<Role> roles = roleSequence.RoleCollection;
								for (int i = 0; i < rolesCount; ++i)
								{
									roles.Add(selectedRoles[i]);
								}
								roleSequences.Add(roleSequence);
							}
						}
						else if (action.mySubtypeConnection)
						{
							LinkedElementCollection<SetComparisonConstraintRoleSequence> existingSequences = mcConstraint.RoleSequenceCollection;
							int existingSequenceCount = existingSequences.Count;
							// Pull out removed ones first
							for (int i = existingSequenceCount - 1; i >= 0; --i)
							{
								SetComparisonConstraintRoleSequence existingSequence = existingSequences[i];
								LinkedElementCollection<Role> sequenceRoles = existingSequence.RoleCollection;
								if (sequenceRoles.Count != 1 || !selectedRoles.Contains(sequenceRoles[0]))
								{
									existingSequences.Remove(existingSequence);
									--existingSequenceCount;
								}
							}
							Store store = mcConstraint.Store;
							for (int i = 0; i < rolesCount; ++i)
							{
								Role selectedRole = selectedRoles[i];
								int existingIndex = -1;
								for (int j = 0; j < existingSequenceCount; ++j)
								{
									if (existingSequences[j].RoleCollection[0] == selectedRole)
									{
										existingIndex = j;
										break;
									}
								}
								if (existingIndex == -1)
								{
									SetComparisonConstraintRoleSequence roleSequence = new SetComparisonConstraintRoleSequence(store);
									roleSequence.RoleCollection.Add(selectedRoles[i]);
									if (i < existingSequenceCount)
									{
										existingSequences.Insert(i, roleSequence);
									}
									else
									{
										existingSequences.Add(roleSequence);
									}
									++existingSequenceCount;
								}
								else if (existingIndex != i)
								{
									existingSequences.Move(existingIndex, i);
								}
							}
						}
						// Edit the existing role set.
						else
						{
							modifyRoleSequence = constraintRoleSequenceBeingEdited;
						}
					}
					else if (null != (scConstraint = constraint as SetConstraint))
					{
						// The single-column constraint is its own role set, just add the roles.
						modifyRoleSequence = scConstraint;
					}
					if (modifyRoleSequence != null)
					{
						// Note that we don't just blow away the collection here, there are too
						// many side effects (such as removing the preferred identifier when a compatible
						// link is added)
						LinkedElementCollection<Role> roles = modifyRoleSequence.RoleCollection;
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
								else
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
				return ResourceStrings.ExternalConstraintConnectActionTransactionName;
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
		private bool mySubtypeConnection;
		private bool myAllowSubtypeConnection;
		private IList<Role> myInitialSelectedRoles;
		private IList<Role> mySelectedRoles;
		private ExternalConstraintShape mySourceShape;
		private IConstraint myActiveConstraint;
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
						if (element is Role || element is SubtypeLink)
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
				bool isSupertypeRole = false;
				ORMDiagram ormDiagram;
				foreach (ModelElement elem in item.RepresentedElements)
				{
					currentElement = elem;
					SubtypeLink subtypeLink = currentElement as SubtypeLink;
					if (subtypeLink != null)
					{
						isSupertypeRole = true;
						currentElement = subtypeLink.AssociatedSubtypeFact.SupertypeRole;
					}
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
						IConstraint activeConstraint = constraintShape.AssociatedConstraint;
						myActiveConstraint = activeConstraint;
						switch (activeConstraint.ConstraintType)
						{
							case ConstraintType.DisjunctiveMandatory:
								// This setting is refined later
								myAllowSubtypeConnection = true;
								break;
							case ConstraintType.Exclusion:
								ExclusionConstraint exclusion = (ExclusionConstraint)activeConstraint;
								// If the exclusion constraint is currently attached to any fact types
								// that are not subtype facts, then we cannot allow a subtype connection.
								LinkedElementCollection<FactType> exclusionFactTypes = exclusion.FactTypeCollection;
								myAllowSubtypeConnection = (exclusionFactTypes.Count == 0) ? true : exclusionFactTypes[0] is SubtypeFact;
								break;
							default:
								myAllowSubtypeConnection = false;
								break;
						}
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
					ExternalConstraintShape sourceShape = mySourceShape;
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
							redrawIndexBound = roles.Count;
							if (roles.Count == 0 && mySubtypeConnection && InitialRoles.Count == 0)
							{
								mySubtypeConnection = false;
							}
						}
					}
					else
					{
						bool allowAdd = false;
						if (mySubtypeConnection)
						{
							allowAdd = isSupertypeRole;
						}
						else if (isSupertypeRole)
						{
							if (roles.Count == 0 && InitialRoles.Count == 0 && myAllowSubtypeConnection)
							{
								mySubtypeConnection = true;
								allowAdd = true;
							}
						}
						else
						{
							allowAdd = true;
						}
						if (allowAdd)
						{
							forceRedraw = true;
							roles.Add(role);
						}
						else
						{
							sourceShape = null;
						}
					}
					if (sourceShape != null)
					{
						myPendingOnClickedAction = OnClickedAction.CheckForCommit;
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
				else if (mySourceShape != null && null != (ormDiagram = currentElement as ORMDiagram))
				{
					base.OnClicked(e); // Let through to allow a cancel
				}
			}
		}
		private static void RedrawOwningFactType(Role role)
		{
			FactType factType = role.FactType;
			if (factType != null)
			{
				LinkedElementCollection<PresentationElement> pels = PresentationViewsSubject.GetPresentation(factType);
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
		}
		private static void RedrawOwningFactTypes(IList<Role> roles)
		{
			if (roles != null)
			{
				int roleCount = roles.Count;
				for (int i = 0; i < roleCount; ++i)
				{
					RedrawOwningFactType(roles[i]);
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
			RedrawOwningFactTypes(mySelectedRoles);
			RedrawOwningFactTypes(myInitialSelectedRoles);

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
			ExternalConstraintShape chainOnShape = (myPendingOnClickedAction == OnClickedAction.Complete && !(myInitialSelectedRoles != null && myInitialSelectedRoles.Count != 0)) ? mySourceShape : null;
			bool editingSubtypeFact = mySubtypeConnection;
			Reset();
			if (chainOnShape != null)
			{
				Shell.ORMDesignerDocView.RefreshCommandStatus(e.DiagramClientView);
				// Depending on the type of constraint you're editing, and possibly depending on the 
				// state of the model at the moment, you may want to activate the ExternalConstraintConnectAction
				// again.  If you want to do any fun tricks after a user's committed the action,
				// this would be the place to do it.
				IConstraint constraint = chainOnShape.AssociatedConstraint;
				switch (constraint.ConstraintStorageStyle)
				{
					case ConstraintStorageStyle.SetComparisonConstraint:
						if (!editingSubtypeFact)
						{
							IConstraint editConstraint = chainOnShape.AssociatedConstraint;
							int maximum = ConstraintUtility.RoleSequenceCountMaximum(editConstraint);
							if (maximum < 0 || ((SetComparisonConstraint)editConstraint).RoleSequenceCollection.Count < maximum)
							{
								ChainMouseAction(chainOnShape, e.DiagramClientView);
							}
						}
						break;
				}
			}
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
		#endregion // Base overrides
		#region ExternalConstraintConnectAction specific
		/// <summary>
		/// The constraint that acts as the Source object for this mouse action
		/// </summary>
		public IConstraint ActiveConstraint
		{
			get
			{
				return myActiveConstraint;
			}
		}
		private void Reset()
		{
			myConstraintRoleSequence = null;
			myInitialSelectedRoles = null;
			mySelectedRoles = null;
			mySourceShape = null;
			myActiveConstraint = null;
			myPendingOnClickedAction = OnClickedAction.Normal;
			mySubtypeConnection = false;
			myAllowSubtypeConnection = false;
			FactTypeShape.ActiveExternalConstraintConnectAction = null;
		}
		/// <summary>
		/// Get the sequence of currently selected roles.
		/// </summary>
		/// <value>IList{Role}</value>
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
				LinkedElementCollection<Role> roleCollection = value.RoleCollection;
				IList<Role> selectedRoleCollection = SelectedRoleCollection;
				IList<Role> initialRoles = InitialRoles;
				bool firstRole = true;
				foreach (Role r in roleCollection)
				{
					if (firstRole && r is SupertypeMetaRole)
					{
						firstRole = false;
						mySubtypeConnection = true;
						SetComparisonConstraintRoleSequence comparisonSequence = value as SetComparisonConstraintRoleSequence;
						if (comparisonSequence != null)
						{
							SetComparisonConstraint constraint = comparisonSequence.ExternalConstraint;
							LinkedElementCollection<SetComparisonConstraintRoleSequence> sequences = constraint.RoleSequenceCollection;
							int sequenceCount = sequences.Count;
							for (int i = 0; i < sequenceCount; ++i)
							{
								LinkedElementCollection<Role> roles = sequences[i].RoleCollection;
								if (roles.Count > 0)
								{
									Role r1 = roles[0];
									selectedRoleCollection.Add(r1);
									initialRoles.Add(r1);
								}
							}
							break;
						}
					}
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
		/// Manages <see cref="EventHandler{TEventArgs}"/>s in the <see cref="Store"/> during connect action activation and deactivation.
		/// The default implementation watches for new <see cref="ExternalConstraintShape"/>s added to the <see cref="Diagram"/>.
		/// </summary>
		/// <param name="store">The <see cref="Store"/> for which the <see cref="EventHandler{TEventArgs}"/>s should be managed.</param>
		/// <param name="action">The <see cref="EventHandlerAction"/> that should be taken for the <see cref="EventHandler{TEventArgs}"/>s.</param>
		protected virtual void ManageStoreEvents(Store store, EventHandlerAction action)
		{
			if (store == null || store.Disposed)
			{
				return; // bail out
			}
			ModelingEventManager.GetModelingEventManager(store).AddOrRemoveHandler(store.DomainDataDirectory.FindDomainClass(ExternalConstraintShape.DomainClassId), new EventHandler<ElementAddedEventArgs>(ExternalConstraintShapeAddedEvent), action);
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
		/// Adds an <see cref="EventHandler{TEventArgs}"/> on the <see cref="Store"/> so we can track the <see cref="ExternalConstraintShape"/>
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
