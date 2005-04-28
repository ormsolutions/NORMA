using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.EnterpriseTools.Designer;
using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Microsoft.VisualStudio.EnterpriseTools.Validation.UI;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Shell;
using Northface.Tools.ORM;
using Northface.Tools.ORM.ObjectModel;
using Northface.Tools.ORM.ShapeModel;
namespace Northface.Tools.ORM.Shell
{
	/// <summary>
	/// Valid commands
	/// </summary>
	[Flags]
	public enum ORMDesignerCommands
	{
		/// <summary>
		/// Commands not set
		/// </summary>
		None = 0,
		/// <summary>
		/// Deletion of one or more object types is enabled
		/// </summary>
		DeleteObjectType = 1,
		/// <summary>
		/// Deletion of one or more fact types is enabled
		/// </summary>
		DeleteFactType = 2,
		/// <summary>
		/// Deletion of one or more constraints is enabled
		/// </summary>
		DeleteConstraint = 4,
		/// <summary>
		/// Display the readings toolwindow
		/// </summary>
		DisplayReadingsWindow = 8,
		/// <summary>
		/// Display the Custom Reference Mode window
		/// </summary>
		DisplayCustomReferenceModeWindow = 0x10,
		/// <summary>
		/// Insert a role before or after the current role
		/// </summary>
		InsertRole = 0x20,
		/// <summary>
		/// Delete the current role
		/// </summary>
		DeleteRole = 0x40,
		/// <summary>
		/// Display the fact editor toolwindow
		/// </summary>
		DisplayFactEditorWindow = 0x80,
		/// <summary>
		/// Mask field representing individual delete commands
		/// </summary>
		Delete = DeleteObjectType | DeleteFactType | DeleteConstraint | DeleteRole,
		#region Constraint editing commands
		/// <summary>
		/// Activate editing for the RoleSequence
		/// </summary>
		ActivateRoleSequence = 0x100,
		/// <summary>
		/// Delete the RoleSequence
		/// </summary>
		DeleteRoleSequence = 0x200,
		/// <summary>
		/// Roll the RoleSequence up (lower number) in the active Constraint's RoleSequenceCollection
		/// </summary>
		MoveRoleSequenceUp = 0x400,
		/// <summary>
		/// Roll the RoleSequence down (higher number) in the active Constraint's RoleSequenceCollection
		/// </summary>
		MoveRoleSequenceDown = 0x800,
		/// <summary>
		/// Activate editing for the ExternalConstraint
		/// </summary>
		EditExternalConstraint = 0x1000,

		/// <summary>
		/// Mask field representing individual RoleSeqeuence edit commands
		/// </summary>
		RoleSequenceActions = ActivateRoleSequence | DeleteRoleSequence | MoveRoleSequenceUp | MoveRoleSequenceDown,
		#endregion //Constraint editing 
		// Update the multiselect command filter constants in ORMDesignerDocView
		// when new commands are added
	}

	/// <summary>
	/// DocView designed to contain a single ORM Diagram
	/// </summary>
	[CLSCompliant(false)]
	public partial class ORMDesignerDocView : SingleDiagramDocView
	{
		#region Member variables
		private ORMDesignerCommands myEnabledCommands;
		private ORMDesignerCommands myVisibleCommands;
		private const ORMDesignerCommands EnabledSimpleMultiSelectCommandFilter = ORMDesignerCommands.Delete & ~ORMDesignerCommands.DeleteRole; // We don't allow deletion of the final role. Don't bother with sorting out the multiselect problems here
		private const ORMDesignerCommands EnabledComplexMultiSelectCommandFilter = ORMDesignerCommands.Delete & ~ORMDesignerCommands.DeleteRole;
		#endregion // Member variables
		#region Construction/destruction
		/// <summary>
		/// Standard DocView constructor, called by the editor factory
		/// </summary>
		/// <param name="docData">DocData</param>
		/// <param name="serviceProvider">IServiceProvider</param>
		public ORMDesignerDocView(DocData docData, IServiceProvider serviceProvider) : base(docData, serviceProvider)
		{
		}
		#endregion // Construction/destruction
		#region Base overrides
		/// <summary>
		/// Get the default context menu for this view
		/// </summary>
		protected override System.ComponentModel.Design.CommandID ContextMenuId
		{
			get
			{
				return default(System.ComponentModel.Design.CommandID);
			}
		}

		/// <summary>
		/// String indicating the toolbox tab name that should be selected when this view gets focus.
		/// </summary>
		protected override string DefaultToolboxTabName
		{
			get
			{
				return ResourceStrings.ToolboxDefaultTabName;
			}
		}


		/// <summary>
		/// Handle right-clicks on the diagram
		/// </summary>
		/// <param name="mouseArgs"></param>
		protected override void OnContextMenuRequested(DiagramPointEventArgs mouseArgs)
		{
			// myVisibleCommands and myEnabledCommands will be set when the selection is changed
			if (0 != (myVisibleCommands & myEnabledCommands))
			{
				DiagramClientView clientView = mouseArgs.DiagramClientView;
				// Get the mouse point (relative to the diagram's position), and convert it to a point on the screen
				System.Drawing.Point emulateClickPoint = clientView.PointToScreen(clientView.WorldToDevice(mouseArgs.MousePosition));
				this.MenuService.ShowContextMenu(ORMDesignerCommandIds.ViewContextMenu, emulateClickPoint.X, emulateClickPoint.Y);
			}
			else
			{
				mouseArgs.Handled = true;
			}
		}
		/// <summary>
		/// Call to refresh the command status for a client view.
		/// This is required when actions my update the currently
		/// enabled commands, but do not change the selection.
		/// </summary>
		/// <param name="clientView">The modified (presumably active) view</param>
		public static void RefreshCommandStatus(DiagramClientView clientView)
		{
			Diagram diagram;
			VSDiagramView diagramView;
			ORMDesignerDocView docView;
			if (null != (diagram = clientView.Diagram) &&
				null != (diagramView = diagram.ActiveDiagramView as VSDiagramView) &&
				null != (docView = diagramView.DocView as ORMDesignerDocView))
			{
				docView.OnSelectionChanged(EventArgs.Empty);
			}
		}
		/// <summary>
		/// Enable menu commands when the selection changes
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSelectionChanged(EventArgs e)
		{
			ORMDesignerCommands visibleCommands = ORMDesignerCommands.None;
			ORMDesignerCommands enabledCommands = ORMDesignerCommands.None;
			int count = SelectionCount;
			if (count != 0)
			{
				if (count > 1)
				{
					// StickyObjects cannot be multi-selected (shift-click).  In other words, if there is an active StickyObject,
					// it will be deactivated if multiple objects are selected.
					ORMDiagram ormDiagram;
					if (null != (ormDiagram = CurrentDiagram as ORMDiagram))
					{
						ormDiagram.StickyObject = null;
					}

					ORMDesignerCommands currentVisible = ORMDesignerCommands.None;
					ORMDesignerCommands currentEnabled = ORMDesignerCommands.None;
					visibleCommands = enabledCommands = EnabledSimpleMultiSelectCommandFilter; // UNDONE: state.IsCoercedSelectionMixed ? EnabledComplexMultiSelectCommandFilter : EnabledSimpleMultiSelectCommandFilter;
					// UNDONE: How do we get the state?
					//foreach (ModelElement mel in state.CoercedSelectionModelElements)
					foreach (ModelElement melIter in GetSelectedComponents())
					{
						ModelElement mel = melIter;
						PresentationElement pel = mel as PresentationElement;
						if (pel != null)
						{
							mel = pel.ModelElement;
						}
						if (mel != null)
						{
							SetCommandStatus(mel, out currentVisible, out currentEnabled);
							enabledCommands &= currentEnabled;
							visibleCommands &= currentVisible;
							if (enabledCommands == 0 && visibleCommands == 0)
							{
								break;
							}
						}
					}
				}
				else
				{
					// UNDONE: How do we get the state?
					//foreach (ModelElement mel in state.CoercedSelectionModelElements)
					foreach (ModelElement melIter in GetSelectedComponents())
					{
						ModelElement mel = melIter;
						PresentationElement pel = mel as PresentationElement;

						// Checking for StickyObjects.  This needs to be done out here because when a role box is selected
						// the pel will be null.
						ORMDiagram ormDiagram;
						ormDiagram = CurrentDiagram as ORMDiagram;
						IStickyObject stickyObject;

						// There is a sticky object on this diagram
						if (null != (stickyObject = ormDiagram.StickyObject))
						{
							// The currently selected item is not selection-compatible with the StickyObject.
							if (!stickyObject.StickySelectable(mel))
							{
								ormDiagram.StickyObject = null;
							}
						}

						if (pel != null)
						{
							mel = pel.ModelElement;
						}

						if (mel != null)
						{
							SetCommandStatus(mel, out visibleCommands, out enabledCommands);
						}
					}
				}
			}
			myVisibleCommands = visibleCommands;
			myEnabledCommands = enabledCommands;
		}
		/// <summary>
		/// Determine which commands are visible and enabled for the
		/// current state of an individual given element.
		/// </summary>
		/// <param name="element">A single model element. Should be a backing object, not a presentation element.</param>
		/// <param name="visibleCommands">(output) The set of visible commands</param>
		/// <param name="enabledCommands">(output) The set of enabled commands</param>
		protected virtual void SetCommandStatus(ModelElement element, out ORMDesignerCommands visibleCommands, out ORMDesignerCommands enabledCommands)
		{
			enabledCommands = ORMDesignerCommands.None;
			visibleCommands = ORMDesignerCommands.None;
			Role role;
			if (element is FactType)
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DeleteFactType | ORMDesignerCommands.DisplayReadingsWindow | ORMDesignerCommands.DisplayFactEditorWindow;
			}
			else if (element is ObjectType)
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DeleteObjectType;
			}
			else if (element is MultiColumnExternalConstraint || element is SingleColumnExternalConstraint)
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DeleteConstraint | ORMDesignerCommands.EditExternalConstraint;
			}
			else if (element is InternalConstraint)
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DeleteConstraint;
			}
			else if (element is ORMModel)
			{
				visibleCommands = ORMDesignerCommands.Delete | ORMDesignerCommands.DisplayCustomReferenceModeWindow | ORMDesignerCommands.DisplayFactEditorWindow;
				enabledCommands = ORMDesignerCommands.DisplayCustomReferenceModeWindow | ORMDesignerCommands.DisplayFactEditorWindow;
			}
			else if (null != (role = element as Role))
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DisplayReadingsWindow | ORMDesignerCommands.InsertRole | ORMDesignerCommands.DeleteRole | ORMDesignerCommands.DisplayFactEditorWindow;
				// Disable role deletion if the role count == 1
				visibleCommands |= ORMDesignerCommands.DeleteRole;
				if (role.FactType.RoleCollection.Count == 1)
				{
					enabledCommands &= ~ORMDesignerCommands.DeleteRole;
				}

				// Extra menu commands may be visible if there is a StickyObject active on the diagram.
				ExternalConstraintShape constraintShape;
				IConstraint constraint;
				ORMDiagram ormDiagram;

				if (null != (ormDiagram = CurrentDiagram as ORMDiagram)
					&& null != (constraintShape = ormDiagram.StickyObject as ExternalConstraintShape)
					&& null != (constraint = constraintShape.AssociatedConstraint))
				{
					bool thisRoleInConstraint = false;
					switch (constraint.ConstraintStorageStyle)
					{
						case ConstraintStorageStyle.SingleColumnExternalConstraint:
							SingleColumnExternalConstraint scec = constraint as SingleColumnExternalConstraint;
							if (scec.RoleCollection.IndexOf(role) >= 0)
							{
								thisRoleInConstraint = true;
								visibleCommands |= ORMDesignerCommands.ActivateRoleSequence;
								enabledCommands |= ORMDesignerCommands.ActivateRoleSequence;
							}
							break;
						case ConstraintStorageStyle.MultiColumnExternalConstraint:
							MultiColumnExternalConstraint mcec = constraint as MultiColumnExternalConstraint;
							int indexOfRole = -1;
							RoleMoveableCollection currentRoleSequence = null;
							foreach (MultiColumnExternalConstraintRoleSequence rs in mcec.RoleSequenceCollection)
							{
								currentRoleSequence = rs.RoleCollection;
								indexOfRole = currentRoleSequence.IndexOf(role);
								if (indexOfRole >= 0)
								{
									thisRoleInConstraint = true;
									indexOfRole = mcec.RoleSequenceCollection.IndexOf(rs);
									break;
								}
							}
							if (thisRoleInConstraint)
							{
								visibleCommands |= ORMDesignerCommands.RoleSequenceActions | ORMDesignerCommands.ActivateRoleSequence;
								enabledCommands |= ORMDesignerCommands.RoleSequenceActions | ORMDesignerCommands.ActivateRoleSequence;
								if (indexOfRole == 0)
								{
									enabledCommands &= ~ORMDesignerCommands.MoveRoleSequenceUp;
								}
								else if (indexOfRole == currentRoleSequence.Count - 1)
								{
									enabledCommands &= ~ORMDesignerCommands.MoveRoleSequenceDown;
								}
							}
							break;
						default:
							break;
					}
					if (!thisRoleInConstraint)
					{
						ormDiagram.StickyObject = null;
					}
				}
			}
		}
		
		/// <summary>
		/// Check the current status of the requested command. This is called frequently, and is
		/// static to enable placing the null check inside this function.
		/// </summary>
		/// <param name="sender">A MenuCommand or OleMenuCommand</param>
		/// <param name="docView">The view to test</param>
		/// <param name="commandFlag">The command to check for enabled</param>
		protected static void OnStatusCommand(object sender, ORMDesignerDocView docView, ORMDesignerCommands commandFlag)
		{
			MenuCommand command = sender as MenuCommand;
			Debug.Assert(command != null);
			if (docView != null)
			{
				command.Visible = 0 != (commandFlag & docView.myVisibleCommands);
				command.Enabled = 0 != (commandFlag & docView.myEnabledCommands);
				if (0 != (commandFlag & ORMDesignerCommands.Delete))
				{
					docView.SetDeleteCommandText((OleMenuCommand)command);
				}
			}
		}

		/// <summary>
		/// Set the menu's text for the delete command
		/// </summary>
		/// <param name="command">OleMenuCommand</param>
		protected virtual void SetDeleteCommandText(OleMenuCommand command)
		{
			Debug.Assert(command != null);
			string commandText;
			switch (myVisibleCommands & ORMDesignerCommands.Delete)
			{
				case ORMDesignerCommands.DeleteObjectType:
					commandText = ResourceStrings.CommandDeleteObjectTypeText;
					break;
				case ORMDesignerCommands.DeleteFactType:
					commandText = ResourceStrings.CommandDeleteFactTypeText;
					break;
				case ORMDesignerCommands.DeleteConstraint:
					commandText = ResourceStrings.CommandDeleteConstraintText;
					break;
				case ORMDesignerCommands.DeleteRole:
					commandText = ResourceStrings.CommandDeleteRoleText;
					break;
				default:
					commandText = null;
					break;
			}
			// Setting command.Text to null will pick up
			// the default command text
			command.Text = commandText;
		}
		/// <summary>
		/// UNDONE: Temporary workaround for DSLTools SDK bug.
		/// OnClose is throwing when ModelingDocStore.CreateVSHost
		/// attempts to cast to the wrong class type.
		/// </summary>
		/// <param name="pgrfSaveOptions"></param>
		/// <returns></returns>
		public override int OnClose(ref uint pgrfSaveOptions)
		{
			return 0;
		}
		#endregion // Base overrides
		#region ORMDesignerDocView Specific
		/// <summary>
		/// Called by ORMDesignerDocData during Load
		/// </summary>
		/// <param name="diagram">The diagram object. Passed to the base class.</param>
		/// <param name="document">ORMDesignerDocData</param>
		public void InitializeView(Diagram diagram, ORMDesignerDocData document)
		{
			// Important to set this value via the Diagram member on SingleDiagramDocView as the 
			// side effects are vital to the diagram being hooked to the view correctly.
			base.Diagram = diagram;

			// Make sure we get a closing notification so we can clear the
			// selected components
			document.DocumentClosing += new EventHandler(DocumentClosing);
		}
		private void DocumentClosing(object sender, EventArgs e)
		{
			(sender as DocData).DocumentClosing -= new EventHandler(DocumentClosing);
			SetSelectedComponents(new object[]{});
		}
		/// <summary>
		/// Execute the delete command
		/// </summary>
		/// <param name="commandText">The text from the command</param>
		protected virtual void OnMenuDelete(string commandText)
		{
			int count = SelectionCount;
			if (count > 0)
			{
				ModelingDocData docData = this.DocData as ModelingDocData;
				Debug.Assert(docData != null);

				Store store = docData.Store;
				Debug.Assert(store != null);

				Diagram d = null;
				// Use the localized text from the command for our transaction name
				using (Transaction t = store.TransactionManager.BeginTransaction(commandText.Replace("&", "")))
				{
					bool testRefModeCollapse = 0 != (myEnabledCommands & ORMDesignerCommands.DeleteObjectType);

					// account for multiple selection
					foreach (object selectedObject in GetSelectedComponents())
					{
						ShapeElement pel; // just the shape
						ModelElement mel;
						if (null != (pel = selectedObject as ShapeElement))
						{
							// Check if the object shape was in expanded mode
							Northface.Tools.ORM.ShapeModel.ObjectTypeShape objectShape;
							if (testRefModeCollapse &&
								null != (objectShape = pel as Northface.Tools.ORM.ShapeModel.ObjectTypeShape) &&
								!objectShape.ExpandRefMode
								)
							{
								t.TopLevelTransaction.Context.ContextInfo[ObjectType.DeleteReferenceModeValueType] = null;
							}
							if (d == null)
							{
								d = pel.Diagram;
								(docData.QueuedSelection as IList).Add(d);
							}

							// Get the actual object inside the pel before
							// removing the pel.
							mel = pel.ModelElement;

							// Remove the actual object in the model
							if (mel != null)
							{
								// get rid of all visual shapes corresponding to this
								// model element. pel removal is done in the PresentationLinkRemoved rule
								mel.PresentationRolePlayers.Clear();

								// Get rid of the model element
								mel.Remove();
							}
						}
						else if (null != (mel = selectedObject as ModelElement))
						{
							// The object was selected directly (through a shape field or sub field element)
							ModelElement shapeAssociatedMel = null;
							switch (myEnabledCommands & ORMDesignerCommands.DeleteRole)
							{
								case ORMDesignerCommands.DeleteRole:
									shapeAssociatedMel = (selectedObject as Role).FactType;
									break;
								case ORMDesignerCommands.DeleteConstraint:
									shapeAssociatedMel = (selectedObject as InternalConstraint).FactType;
									break;
							}

							// Add the parent shape into the queued selection
							if (shapeAssociatedMel != null)
							{
								pel = (CurrentDiagram as ORMDiagram).FindShapeForElement(shapeAssociatedMel);
								if (pel != null)
								{
									(docData.QueuedSelection as IList).Add(pel);
								}
							}

							// Remove the item
							mel.Remove();
						}
					}

					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				}

				if (d != null)
				{
					// Clearing the selection selects the diagram
					CurrentDesigner.Selection.Clear();
				}
			}
		}
		/// <summary>
		/// Execute the Insert Role menu commands
		/// </summary>
		/// <param name="insertAfter">true to insert the role after the
		/// selected role, false to insert it before the selected role</param>
		protected virtual void OnMenuInsertRole(bool insertAfter)
		{
			ICollection components = GetSelectedComponents();
			if (components.Count == 1)
			{
				Role role = null;
				foreach (object component in components)
				{
					role = component as Role;
					break;
				}
				FactType factType;
				if (role != null &&
					null != (factType = role.FactType))
				{
					RoleMoveableCollection roles = factType.RoleCollection;
					int insertIndex = roles.IndexOf(role);
					if (insertAfter)
					{
						++insertIndex;
					}
					Store store = factType.Store;
					using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.InsertRoleTransactionName))
					{
						Role newRole = Role.CreateRole(store);
						if (insertIndex == roles.Count)
						{
							roles.Add(newRole);
						}
						else
						{
							roles.Insert(insertIndex, newRole);
						}
						t.Commit();
					}
				}
			}
		}
		/// <summary>
		/// Select the constraint as the ORDiagram's sticky object for editing.
		/// </summary>
		protected virtual void OnMenuEditExternalConstraint()
		{
			ORMDiagram ormDiagram;
			ExternalConstraintShape ecs;
			if (null != (ormDiagram = CurrentDiagram as ORMDiagram)
				&& null != (ecs = SelectedElements[0] as ExternalConstraintShape))
			{
				ormDiagram.StickyObject = ecs;
			}
		}
		/// <summary>
		/// Activate the RoleSequence for editing.
		/// </summary>
		protected virtual void OnMenuActivateRoleSequence()
		{
			// Get the constraint of the StickyObject.
			ORMDiagram ormDiagram;
			ExternalConstraintShape constraintShape;
			if (null != (ormDiagram = CurrentDiagram as ORMDiagram)
				&& null != (constraintShape = ormDiagram.StickyObject as ExternalConstraintShape))
			{
				IConstraint constraint = constraintShape.AssociatedConstraint;
				ExternalConstraintConnectAction connectAction = ormDiagram.ExternalConstraintConnectAction;

				Role role = SelectedElements[0] as Role;
				ConstraintRoleSequence selectedSequence = null;
				foreach (ConstraintRoleSequence sequence in role.ConstraintRoleSequenceCollection)
				{
					if (object.ReferenceEquals(constraint, sequence.Constraint))
					{
						selectedSequence = sequence;
						break;
					}
				}
				connectAction.ConstraintRoleSequenceToEdit = selectedSequence;
				connectAction.ChainMouseAction(constraintShape, CurrentDesigner.DiagramClientView);
			}
		}
		/// <summary>
		/// Delete the RoleSequence from the ORMDiagram's StickyObject that contains the currently selected role.
		/// </summary>
		protected virtual void OnMenuDeleteRoleSequence()
		{
			if (SelectedElements.Count == 1)
			{
				Role role;
				ORMDiagram ormDiagram;
				ExternalConstraintShape ecs;
				MultiColumnExternalConstraint mcec;
				if (null != (role = SelectedElements[0] as Role)
					&& null != (ormDiagram = CurrentDiagram as ORMDiagram)
					&& null != (ecs = ormDiagram.StickyObject as ExternalConstraintShape)
					&& null != (mcec = ecs.AssociatedConstraint as MultiColumnExternalConstraint))
				{
					// TODO:  It is theoretically possible to have one role playing a part in multiple
					// RoleSequences for a constraint.  At some point it would probably be nice to
					// decide which RoleSequence is active and blow that one away instead of just
					// walking the RoleSequenceCollection and killing any RoleSequence that has
					// reference to this role.

					ConstraintRoleSequenceMoveableCollection roleConstraints = role.ConstraintRoleSequenceCollection;

					int constraintCount = roleConstraints.Count;
					using (Transaction t = role.Store.TransactionManager.BeginTransaction(ResourceStrings.DeleteRoleSequenceTransactionName))
					{
						for (int i = constraintCount - 1; i >= 0; --i)
						{
							// The current ConstraintRoleSequence is the one associated with the current StickyObject.
							if (object.ReferenceEquals((roleConstraints[i]).Constraint, mcec))
							{
								// TODO: Remove the ConstraintRoleSequence from this role.
								roleConstraints[i].Remove();
							}
						}
						if (t.HasPendingChanges)
						{
							t.Commit();
							ormDiagram.StickyObject.StickyRedraw();
//							// TODO:  Re-initializing the StickyObject is probably inefficient.  Implementing a rule on
//							// MCECs whenever their constraint collection is changed would probably be more effective.
//							// This is especially true when role sequences are just being moved up and down.  No insertions
//							// or deletions, it's just touched.
//							ormDiagram.StickyObject.StickyInitialize();
						}
					}
				}
			}
			else
			{
				// Not sure if this should be allowed.  For that matter, since roles are represented as
				// ShapeFields instead of ShapeElements, I don't know that it's even possible to multiselect them.
				throw new NotImplementedException(
					string.Concat("Multiselect deletion of role sequences is not implemented.  ",
					"If you see this message, decide if what you're doing is really a valid operation.  ",
					"If it is, look in Shell\\ORMCommandSet.cs, OnMenuDeleteRowSequence() to implement it."));
			}

		}
		/// <summary>
		/// Move the RoleSequence of the ORMDiagram's StickyObject up in the collection.
		/// </summary>
		protected virtual void OnMenuMoveRoleSequenceUp()
		{
			Role role;
			ORMDiagram ormDiagram;
			ExternalConstraintShape ecs;
			MultiColumnExternalConstraint mcec;
			if (null != (role = SelectedElements[0] as Role)
				&& null != (ormDiagram = CurrentDiagram as ORMDiagram)
				&& null != (ecs = ormDiagram.StickyObject as ExternalConstraintShape)
				&& null != (mcec = ecs.AssociatedConstraint as MultiColumnExternalConstraint))
			{
				MultiColumnExternalConstraintRoleSequenceMoveableCollection roleSequences = mcec.RoleSequenceCollection;
				MultiColumnExternalConstraintRoleSequence sequenceToMove = null;
				int sequenceOriginalPosition = 0;
				int sequenceNewPosition = -1;
				int lastPosition = roleSequences.Count - 1;
				foreach (MultiColumnExternalConstraintRoleSequence sequence in roleSequences)
				{
					if (sequence.RoleCollection.IndexOf(role) >= 0)
					{
						sequenceToMove = sequence;
						break;
					}
					++sequenceOriginalPosition;
				}

				if (sequenceToMove != null)
				{
					using (Transaction trans = role.Store.TransactionManager.BeginTransaction(ResourceStrings.MoveRoleSequenceDownTransactionName))
					{
						if (sequenceOriginalPosition > 0)
						{
							sequenceNewPosition = sequenceOriginalPosition - 1;
							roleSequences.Move(sequenceOriginalPosition, sequenceNewPosition);
						}
						if (trans.HasPendingChanges)
						{
							trans.Commit();

							// We need to reset the enabled commands so that they are immediately available if the same
							// role is right-clicked again.  Otherwise, the diagram's selected item will not have changed
							// and therefore the menu's enabled items will not be refreshed and may not reflect the
							// currently available options.
							if (sequenceOriginalPosition == lastPosition)
							{
								myEnabledCommands |= ORMDesignerCommands.MoveRoleSequenceDown;
							}
							if (sequenceNewPosition == 0)
							{
								myEnabledCommands &= ~ORMDesignerCommands.MoveRoleSequenceUp;
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// Move the RoleSequence of the ORMDiagram's StickyObject down in the collection.
		/// </summary>
		protected virtual void OnMenuMoveRoleSequenceDown()
		{
			Role role;
			ORMDiagram ormDiagram;
			ExternalConstraintShape ecs;
			MultiColumnExternalConstraint mcec;
			if (null != (role = SelectedElements[0] as Role)
				&& null != (ormDiagram = CurrentDiagram as ORMDiagram)
				&& null != (ecs = ormDiagram.StickyObject as ExternalConstraintShape)
				&& null != (mcec = ecs.AssociatedConstraint as MultiColumnExternalConstraint))
			{

				MultiColumnExternalConstraintRoleSequenceMoveableCollection roleSequences = mcec.RoleSequenceCollection;
				MultiColumnExternalConstraintRoleSequence sequenceToMove = null;
				int sequenceOriginalPosition = 0;
				int sequenceNewPosition = -1;
				int lastPosition = roleSequences.Count - 1;
				foreach (MultiColumnExternalConstraintRoleSequence sequence in roleSequences)
				{
					if (sequence.RoleCollection.IndexOf(role) >= 0)
					{
						sequenceToMove = sequence;
						break;
					}
					++sequenceOriginalPosition;
				}

				if (sequenceToMove != null)
				{
					using (Transaction trans = role.Store.TransactionManager.BeginTransaction(ResourceStrings.MoveRoleSequenceUpTransactionName))
					{
						if (sequenceOriginalPosition < lastPosition)
						{
							sequenceNewPosition = sequenceOriginalPosition + 1;
							roleSequences.Move(sequenceOriginalPosition, sequenceNewPosition);
						}
						if (trans.HasPendingChanges)
						{
							trans.Commit();

							// We need to reset the enabled commands so that they are immediately available if the same
							// role is right-clicked again.  Otherwise, the diagram's selected item will not have changed
							// and therefore the menu's enabled items will not be refreshed and may not reflect the
							// currently available options.
							if (sequenceOriginalPosition == 0)
							{
								myEnabledCommands |= ORMDesignerCommands.MoveRoleSequenceUp;
							}
							if (sequenceNewPosition == lastPosition)
							{
								myEnabledCommands &= ~ORMDesignerCommands.MoveRoleSequenceDown;
							}
						}
					}
				}
			}
		}
		/// <summary>
		/// Begin a new RoleSequence on an ExternalConstraint.
		/// </summary>
		protected virtual void OnMenuBeginRoleSequenceOnExternalConstraint()
		{
			// Get the constraint of the StickyObject.
			ORMDiagram ormDiagram = CurrentDiagram as ORMDiagram;
			if (ormDiagram != null)
			{
				ExternalConstraintShape constraintShape;
				if (null != (constraintShape = ormDiagram.StickyObject as ExternalConstraintShape))
				{
					ExternalConstraintConnectAction connectAction = ormDiagram.ExternalConstraintConnectAction;
					connectAction.ChainMouseAction(constraintShape, ormDiagram.ActiveDiagramView.DiagramClientView);
				}
			}
		}
		/// <summary>
		/// Get the element locator associate with this view.
		/// The locator is used to jump to a specific element.
		/// </summary>
		public static ModelElementLocator ElementLocator
		{
			get
			{
				// The element locator available from the command
				// set associate with the current package.
				ORMDesignerCommandSet commandSet = ORMDesignerPackage.CommandSet as ORMDesignerCommandSet;
				return (commandSet != null) ? commandSet.ElementLocator : null;
			}
		}
		#endregion // ORMDesignerDocView Specific
	}
}
