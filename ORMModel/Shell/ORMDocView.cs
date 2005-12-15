using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Shell;
using Neumont.Tools.ORM;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;
namespace Neumont.Tools.ORM.Shell
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
		/// Support the CopyImage command
		/// </summary>
		CopyImage = 0x2000,
		/// <summary>
		/// Display the verbalization browser toolwindow
		/// </summary>
		DisplayVerbalizationWindow = 0x2000,
		/// <summary>
		/// Select all top level selectable elements on the current diagram
		/// </summary>
		SelectAll = 0x4000,
		/// <summary>
		/// Special command used in addition to the specific Delete elements.
		/// DeleteAny will survive most complex multi-select cases whereas the Delete
		/// will not. This is handled specially for the delete case.
		/// </summary>
		DeleteAny = 0x8000,
		/// <summary>
		/// Apply an auto-layout algorithm to the selection. Applies to top-level objects.
		/// </summary>
		AutoLayout = 0x10000,
		/// <summary>
		/// Mask field representing individual delete commands
		/// </summary>
		Delete = DeleteObjectType | DeleteFactType | DeleteConstraint | DeleteRole,
		/// <summary>
		/// Mask field representing individual RoleSeqeuence edit commands
		/// </summary>
		RoleSequenceActions = ActivateRoleSequence | DeleteRoleSequence | MoveRoleSequenceUp | MoveRoleSequenceDown,
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
		/// <summary>
		/// The filter for multi selection when the elements are all of the same type.
		/// </summary>
		private const ORMDesignerCommands EnabledSimpleMultiSelectCommandFilter = ORMDesignerCommands.DisplayVerbalizationWindow | ORMDesignerCommands.SelectAll | ORMDesignerCommands.AutoLayout | ORMDesignerCommands.DeleteAny | (ORMDesignerCommands.Delete & ~ORMDesignerCommands.DeleteRole); // We don't allow deletion of the final role. Don't bother with sorting out the multiselect problems here
		/// <summary>
		/// The filter for multi selection when the elements are of different types. This should always be a subset of the simple command filter
		/// </summary>
		private const ORMDesignerCommands EnabledComplexMultiSelectCommandFilter = EnabledSimpleMultiSelectCommandFilter;
		/// <summary>
		/// A filter to turn off commands for a single selection
		/// </summary>
		private const ORMDesignerCommands DisabledSingleSelectCommandFilter = ORMDesignerCommands.AutoLayout;
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

					ORMDesignerCommands currentVisible;
					ORMDesignerCommands currentEnabled;
					visibleCommands = enabledCommands = EnabledSimpleMultiSelectCommandFilter;
					Type firstType = null;
					bool isComplex = false;
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
							if (!isComplex)
							{
								Type currentType = mel.GetType();
								if (firstType == null)
								{
									firstType = currentType;
								}
								else if (object.ReferenceEquals(firstType, currentType))
								{
									isComplex = true;
									enabledCommands &= EnabledComplexMultiSelectCommandFilter;
									visibleCommands &= EnabledComplexMultiSelectCommandFilter;
								}
							}
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
					foreach (ModelElement melIter in GetSelectedComponents())
					{
						ModelElement mel = melIter;
						PresentationElement pel = mel as PresentationElement;

						// Checking for StickyObjects.  This needs to be done out here because when a role box is selected
						// the pel will be null.
						ORMDiagram ormDiagram;
						IStickyObject stickyObject;

						// There is a sticky object on this diagram
						if (null != (ormDiagram = CurrentDiagram as ORMDiagram))
						{
							if (null != (stickyObject = pel as IStickyObject))
							{
								ormDiagram.StickyObject = stickyObject;
							}
							// The currently selected item is not selection-compatible with the StickyObject.
							else if (null != (stickyObject = ormDiagram.StickyObject)
								&& !stickyObject.StickySelectable(mel))
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
							visibleCommands &= ~DisabledSingleSelectCommandFilter;
							enabledCommands &= ~DisabledSingleSelectCommandFilter;
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
			ObjectType objectType;
			if (element is FactType)
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DeleteFactType | ORMDesignerCommands.DeleteAny | ORMDesignerCommands.DisplayReadingsWindow | ORMDesignerCommands.DisplayFactEditorWindow | ORMDesignerCommands.AutoLayout;
			}
			else if (null != (objectType = element as ObjectType))
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DeleteObjectType | ORMDesignerCommands.DeleteAny;
				if (objectType.NestedFactType == null)
				{
					visibleCommands |= ORMDesignerCommands.AutoLayout;
					enabledCommands |= ORMDesignerCommands.AutoLayout;
				}
			}
			else if (element is MultiColumnExternalConstraint || element is SingleColumnExternalConstraint)
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DeleteConstraint | ORMDesignerCommands.DeleteAny | ORMDesignerCommands.EditExternalConstraint | ORMDesignerCommands.AutoLayout;
			}
			else if (element is InternalConstraint)
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DeleteConstraint | ORMDesignerCommands.DeleteAny;
			}
			else if (element is ORMModel)
			{
				visibleCommands = ORMDesignerCommands.Delete | ORMDesignerCommands.DisplayCustomReferenceModeWindow | ORMDesignerCommands.DisplayFactEditorWindow | ORMDesignerCommands.CopyImage;
				enabledCommands = ORMDesignerCommands.DisplayCustomReferenceModeWindow | ORMDesignerCommands.DisplayFactEditorWindow | ORMDesignerCommands.CopyImage;
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
				}
			}
			// Turn on the verbalization window command for all selections
			visibleCommands |= ORMDesignerCommands.DisplayVerbalizationWindow | ORMDesignerCommands.SelectAll;
			enabledCommands |= ORMDesignerCommands.DisplayVerbalizationWindow | ORMDesignerCommands.SelectAll;
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
				if (0 != (commandFlag & (ORMDesignerCommands.Delete | ORMDesignerCommands.DeleteAny)))
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
			if (commandText == null && 0 != (myVisibleCommands & ORMDesignerCommands.DeleteAny))
			{
				commandText = ResourceStrings.CommandDeleteMultipleText;
			}
			// Setting command.Text to null will pick up
			// the default command text
			command.Text = commandText;
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

				ORMDesignerCommands enabledCommands = myEnabledCommands;

				// There are a number of things to watch out for in a complex selection.
				// 1) The type of object needs to be redetermined for each selected object
				// 2) Deletions may have side effects on other objects, so selected items
				//    may be deleted already by the time we get to them
				// 3) The queued selection can have removed elements in it and needs to be cleaned
				//    up before committing.
				bool complexSelection = 0 == (enabledCommands & ORMDesignerCommands.Delete);

				Diagram d = null;
				// Use the localized text from the command for our transaction name
				using (Transaction t = store.TransactionManager.BeginTransaction(commandText.Replace("&", "")))
				{
					IDictionary contextInfo = t.TopLevelTransaction.Context.ContextInfo;
					IList queuedSelection = docData.QueuedSelection as IList;
					// account for multiple selection
					foreach (object selectedObject in GetSelectedComponents())
					{
						ShapeElement pel; // just the shape
						ModelElement mel;
						bool deleteReferenceModeValueTypeInContext = false;
						if (null != (pel = selectedObject as ShapeElement))
						{
							if (pel.IsRemoved)
							{
								continue;
							}
							if (d == null)
							{
								d = pel.Diagram;
							}

							// Get the actual object inside the pel before
							// removing the pel.
							mel = pel.ModelElement;

							// Remove the actual object in the model
							if (mel != null && !mel.IsRemoved)
							{
								// Check if the object shape was in expanded mode
								bool testRefModeCollapse = complexSelection || 0 != (enabledCommands & ORMDesignerCommands.DeleteObjectType);
								ObjectTypeShape objectShape;
								if (testRefModeCollapse &&
									null != (objectShape = pel as ObjectTypeShape) &&
									!objectShape.ExpandRefMode
									)
								{
									if (!deleteReferenceModeValueTypeInContext)
									{
										contextInfo[ObjectType.DeleteReferenceModeValueType] = null;
										deleteReferenceModeValueTypeInContext = true;
									}
								}
								else if (deleteReferenceModeValueTypeInContext)
								{
									deleteReferenceModeValueTypeInContext = false;
									contextInfo.Remove(ObjectType.DeleteReferenceModeValueType);
								}

								// get rid of all visual shapes corresponding to this
								// model element. pel removal is done in the PresentationLinkRemoved rule
								mel.PresentationRolePlayers.Clear();

								// Get rid of the model element
								mel.Remove();
							}
						}
						else if (null != (mel = selectedObject as ModelElement) && !mel.IsRemoved)
						{
							// The object was selected directly (through a shape field or sub field element)
							ModelElement shapeAssociatedMel = null;
							if (complexSelection)
							{
								InternalConstraint ic;
								Role role;
								if (null != (ic = selectedObject as InternalConstraint))
								{
									shapeAssociatedMel = ic.FactType;
								}
								else if (null != (role = selectedObject as Role))
								{
									shapeAssociatedMel = role.FactType;
								}
							}
							else
							{
								switch (enabledCommands & ORMDesignerCommands.Delete)
								{
									case ORMDesignerCommands.DeleteRole:
										shapeAssociatedMel = (selectedObject as Role).FactType;
										break;
									case ORMDesignerCommands.DeleteConstraint:
										shapeAssociatedMel = (selectedObject as InternalConstraint).FactType;
										break;
								}
							}

							// Add the parent shape into the queued selection
							if (shapeAssociatedMel != null)
							{
								pel = (CurrentDiagram as ORMDiagram).FindShapeForElement(shapeAssociatedMel);
								if (pel != null && !pel.IsRemoved)
								{
									queuedSelection.Add(pel);
								}
							}

							// Remove the item
							mel.Remove();
						}
					}

					if (t.HasPendingChanges)
					{
						if (complexSelection)
						{
							for (int i = queuedSelection.Count - 1; i >= 0; --i)
							{
								if (((ModelElement)queuedSelection[i]).IsRemoved)
								{
									queuedSelection.RemoveAt(i);
								}
							}
						}
						if (queuedSelection.Count == 0 && d != null)
						{
							queuedSelection.Add(d);
						}
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
		/// Execute the SelectAll menu command
		/// </summary>
		protected virtual void OnMenuSelectAll()
		{
			Diagram diagram;
			DiagramView designer;
			ShapeElementMoveableCollection nestedShapes;
			int shapeCount;

			if (null != (diagram = CurrentDiagram) &&
				null != (nestedShapes = diagram.NestedChildShapes) &&
				null != (designer = CurrentDesigner) &&
				0 != (shapeCount = nestedShapes.Count))
			{
				SelectedShapesCollection shapes = designer.Selection;
				bool firstItem = true;
				for (int i = 0; i < shapeCount; ++i)
				{
					// Use deferred selection modification here so that
					// we don't fire a selection change for each add.
					// Getting into n(n-1) change events is very
					// expensive, especially for verbalization
					ShapeElement currentShape = nestedShapes[i];
					if (currentShape.CanSelect)
					{
						DiagramItem newItem = new DiagramItem(currentShape);
						if (firstItem)
						{
							firstItem = false;
							//spahes.Clear();
							shapes.DeferredClearBeforeAdditions();
							//shapes.Set(newItem);
							shapes.DeferredAdd(newItem);
							shapes.DeferredPrimaryItem(newItem);
						}
						else
						{
							//shapes.Add(newItem);
							shapes.DeferredAdd(newItem);
						}
					}
				}
				if (!firstItem)
				{
					// UNDONE: MSBUG shapes.SetDeferredSelection should not
					// be internal. This is a hack workaround to call something
					// public that calls it.
					designer.DiagramClientView.OnElementEventsEnded(null);
				}
			}
		}
		/// <summary>
		/// Execute the AutoLayout menu command
		/// </summary>
		protected virtual void OnMenuAutoLayout()
		{
			Diagram diagram;

			if (null != (diagram = CurrentDiagram))
			{
				using (Transaction t = diagram.Store.TransactionManager.BeginTransaction(ResourceStrings.AutoLayoutTransactionName))
				{
					// ORM diagrams don't do line routing, so there is no reason to attempt routing here
					diagram.AutoLayoutShapeElements(GetSelectedComponents(), VGRoutingStyle.VGRouteNone, PlacementValueStyle.VGPlaceWideSSW, false);
					t.Commit();
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
					// We've just added a role, so we have more than 1 and
					// can go ahead and enable delete
					myVisibleCommands |= ORMDesignerCommands.DeleteRole;
					myEnabledCommands |= ORMDesignerCommands.DeleteRole;
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
				IStickyObject sticky = ormDiagram.StickyObject;
				if (sticky == null)
				{
					ormDiagram.StickyObject = ecs;
				}
				else
				{
					IConstraint constraint = ecs.AssociatedConstraint;
					ExternalConstraintConnectAction connectAction = ormDiagram.ExternalConstraintConnectAction;
					SingleColumnExternalConstraint scec;
					//MultiColumnExternalConstraint mcec;
					if (null != (scec = constraint as SingleColumnExternalConstraint))
					{
						connectAction.ConstraintRoleSequenceToEdit = scec;
					}
					//else if (null != (mcec = constraint as MultiColumnExternalConstraint))
					//{
					//}
					if (!connectAction.IsActive)
					{
						connectAction.ChainMouseAction(ecs, (DiagramClientView)ormDiagram.ClientViews[0]);
					}
				}
			}
		}

		#region OnMenuCopyImage
#if CUSTOM_COPY_IMAGE
		#region NativeMethods
		[System.Security.SuppressUnmanagedCodeSecurity]
		private static partial class NativeMethods
		{
#if !CUSTOM_COPY_IMAGE_VIA_MAKE_TRANSPARENT
			#region GetNewMetafile
			[System.Runtime.InteropServices.DllImport("user32.dll", CharSet=System.Runtime.InteropServices.CharSet.Auto, ExactSpelling=true)]
			private static extern IntPtr GetDesktopWindow();

			/// <summary>This supports <see cref="OnMenuCopyImage"/>, and should NOT be used by other methods.</summary>
			internal static System.Drawing.Imaging.Metafile GetNewMetafile(System.Drawing.Imaging.EmfType emfType)
			{
				System.Drawing.Graphics graphics = null;
				System.Drawing.Imaging.Metafile metafile = null;
				IntPtr hdc = IntPtr.Zero;
				try
				{
					graphics = System.Drawing.Graphics.FromHwnd(NativeMethods.GetDesktopWindow());
					hdc = graphics.GetHdc();
					metafile = new System.Drawing.Imaging.Metafile(hdc, emfType);
				}
				finally
				{
					if (graphics != null)
					{
						if (hdc != IntPtr.Zero)
						{
							graphics.ReleaseHdc(hdc);
						}
						graphics.Dispose();
					}
				}
				return metafile;
			}
			#endregion
#endif

			#region CopyMetafileToClipboard
			[System.Runtime.InteropServices.DllImport("user32.dll", CharSet=System.Runtime.InteropServices.CharSet.Auto, ExactSpelling=true)]
			private static extern bool CloseClipboard();
			[System.Runtime.InteropServices.DllImport("user32.dll", CharSet=System.Runtime.InteropServices.CharSet.Auto, ExactSpelling=true)]
			private static extern bool EmptyClipboard();
			[System.Runtime.InteropServices.DllImport("user32.dll", CharSet=System.Runtime.InteropServices.CharSet.Auto, ExactSpelling=true)]
			private static extern bool OpenClipboard(IntPtr hWndNewOwner);
			[System.Runtime.InteropServices.DllImport("user32.dll", CharSet=System.Runtime.InteropServices.CharSet.Auto, ExactSpelling=true)]
			private static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);
			[System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet=System.Runtime.InteropServices.CharSet.Auto)]
			private static extern IntPtr CopyEnhMetaFile(IntPtr hemfSrc, IntPtr lpszFile);
			[System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet=System.Runtime.InteropServices.CharSet.Auto)]
			private static extern bool DeleteEnhMetaFile(IntPtr hemfSrc);

			/// <summary>This supports <see cref="OnMenuCopyImage"/>, and should NOT be used by other methods.</summary>
			internal static void CopyMetafileToClipboard(IntPtr hWndNewOwner, System.Drawing.Imaging.Metafile metafile)
			{
				const uint CF_ENHMETAFILE = 14;

				bool clipboardOpen = false;
				IntPtr hEnhmetafile = IntPtr.Zero;
				try
				{
					if (clipboardOpen = OpenClipboard(hWndNewOwner) && EmptyClipboard())
					{
						hEnhmetafile = metafile.GetHenhmetafile();
						SetClipboardData(CF_ENHMETAFILE, CopyEnhMetaFile(hEnhmetafile, IntPtr.Zero));
					}
				}
				finally
				{
					if (clipboardOpen)
					{
						CloseClipboard();
					}
					if (hEnhmetafile != IntPtr.Zero)
					{
						DeleteEnhMetaFile(hEnhmetafile);
					}
				}
			}
			#endregion

#if CUSTOM_COPY_IMAGE_VIA_MAKE_TRANSPARENT
			#region MakeBackgroundTransparent
			[System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet=System.Runtime.InteropServices.CharSet.Auto)]
			private static extern bool ExtFloodFill(IntPtr hdc, int nXStart, int nYStart, uint crColor, uint fuFillType);

			/// <summary>This supports <see cref="OnMenuCopyImage"/>, and should NOT be used by other methods.</summary>
			internal static void MakeBackgroundTransparent(System.Drawing.Imaging.Metafile metafile)
			{
				const uint FLOODFILLSURFACE = 1;

				System.Drawing.Graphics graphics = null;
				IntPtr hdc = IntPtr.Zero;
				try
				{
					graphics = System.Drawing.Graphics.FromImage(metafile);
					hdc = graphics.GetHdc();
					ExtFloodFill(hdc, 0, metafile.Height, 0xFFFFFFFF, FLOODFILLSURFACE);
				}
				finally
				{
					if (graphics != null)
					{
						if (hdc != IntPtr.Zero)
						{
							graphics.ReleaseHdc(hdc);
						}
						graphics.Dispose();
					}
				}
			}
			#endregion
#endif
		}
		#endregion
#endif

		/// <summary>
		/// Copies the selected elements as an image.
		/// </summary>
		protected virtual void OnMenuCopyImage()
		{
			if (this.CurrentDiagram != null && this.CurrentDiagram.ActiveDiagramView != null)
			{
#if !CUSTOM_COPY_IMAGE
				this.CurrentDiagram.CopyImageToClipboard(this.CurrentDiagram.NestedChildShapes);
#else
#if CUSTOM_COPY_IMAGE_VIA_MAKE_TRANSPARENT
				System.Drawing.Imaging.Metafile createdMetafile = this.CurrentDiagram.CreateMetafile(this.CurrentDiagram.NestedChildShapes);
				
				NativeMethods.MakeBackgroundTransparent(createdMetafile);
				NativeMethods.CopyMetafileToClipboard(this.CurrentDiagram.ActiveDiagramView.Handle, createdMetafile);
#else
				System.Reflection.BindingFlags bindingFlags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
				
				Type diagramType = typeof(Microsoft.VisualStudio.Modeling.Diagrams.Diagram);
				System.Reflection.MethodInfo getShapesToDraw = diagramType.GetMethod("GetShapesToDraw", bindingFlags);
				if (getShapesToDraw == null)
				{
					throw new MissingMethodException(diagramType.FullName, "GetShapesToDraw");
				}
								
				RectangleD rect = default(RectangleD);
				object[] parameters = new object[] { this.CurrentDiagram.NestedChildShapes, rect };

				ArrayList shapesToDraw = getShapesToDraw.Invoke(this.CurrentDiagram, parameters) as ArrayList;
				rect = (RectangleD)parameters[1];

				const double imageMargin = 0.1;
				rect.Inflate(imageMargin, imageMargin);

				System.Drawing.Imaging.Metafile metafile = NativeMethods.GetNewMetafile(System.Drawing.Imaging.EmfType.EmfPlusDual);

				using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(metafile))
				{
					if (rect.Location.X != 0 || rect.Location.Y != 0)
					{
						graphics.TranslateTransform((float)(-rect.Location.X), (float)(-rect.Location.Y));
					}
					graphics.PageUnit = System.Drawing.GraphicsUnit.Inch;
					graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
					graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
					graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
					graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

					System.Drawing.Rectangle clipRectangle = new System.Drawing.Rectangle(0, 0, (int)Math.Ceiling(rect.Width * graphics.DpiX), (int)Math.Ceiling(rect.Height * graphics.DpiY));
					DiagramPaintEventArgs diagramPaintEventArgs = new DiagramPaintEventArgs(graphics, clipRectangle, null, true);

					foreach (ShapeElement shapeElement in shapesToDraw)
					{
						if (!shapeElement.IsRemoved)
						{
							shapeElement.OnPaintShape(diagramPaintEventArgs);
						}
					}
				}
				
				NativeMethods.CopyMetafileToClipboard(this.CurrentDiagram.ActiveDiagramView.Handle, metafile);
#endif
#endif
			}
		}
		#endregion

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
