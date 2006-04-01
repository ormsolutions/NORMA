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
using Neumont.Tools.ORM.ObjectModel.Editors;
using Microsoft.VisualStudio.Shell.Interop;

using System.Windows.Forms;
using System.Globalization;
namespace Neumont.Tools.ORM.Shell
{
	#region ORMDesignerCommands enum
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
		/// Display standard toolwindows that we never disable.
		/// This currently maps to the Verbalization and Model Browser windows
		/// </summary>
		DisplayStandardWindows = 0x2000,
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
		/// Toggle the IsMandatory property on the selected role. Applies to a single role.
		/// </summary>
		ToggleSimpleMandatory = 0x20000,
		/// <summary>
		/// Add an internal uniqueness constraint for the selected roles.
		/// Applies to one or more roles from the same fact type.
		/// </summary>
		AddInternalUniqueness = 0x40000,
		/// <summary>
		/// Display the ExtensionManager dialog
		/// </summary>
		ExtensionManager = 0x80000,
		/// <summary>
		/// Support the CopyImage command
		/// </summary>
		CopyImage = 0x100000,
		/// <summary>
		/// Delete an object shape
		/// </summary>
		DeleteObjectShape = 0x200000,
		/// <summary>
		/// Delete a fact shape
		/// </summary>
		DeleteFactShape = 0x400000,
		/// <summary>
		/// Delete a constraint shape
		/// </summary>
		DeleteConstraintShape = 0x800000,
		/// <summary>
		/// Special command used in addition to the specific Delete*Shape elements.
		/// DeleteAnyShape will survive most complex multi-select cases whereas the Delete*Shape
		/// will not. This is handled specially for the delete case.
		/// </summary>
		DeleteAnyShape = 0x1000000,
		/// <summary>
		/// Expand the error list for the selected object
		/// </summary>
		ErrorList = 0x10000000,
		/// <summary>
		/// Align top level shape elements. Applies to all of the standard Format.Align commands.
		/// </summary>
		AlignShapes = 0x2000000,
		/// <summary>
		/// Move a role's order to the left within the fact type.
		/// </summary>
		MoveRoleLeft = 0x4000000, 
		/// <summary>
		/// Move a role's order to the right within the fact type.
		/// </summary>
		MoveRoleRight = 0x8000000, 
		/// <summary>
		/// Mask field representing individual delete commands
		/// </summary>
		Delete = DeleteObjectType | DeleteFactType | DeleteConstraint | DeleteRole,
		/// <summary>
		/// Mask field representing individual shape delete commands
		/// </summary>
		DeleteShape = DeleteObjectShape | DeleteFactShape | DeleteConstraintShape,
		/// <summary>
		/// Mask field representing individual RoleSeqeuence edit commands
		/// </summary>
		RoleSequenceActions = ActivateRoleSequence | DeleteRoleSequence | MoveRoleSequenceUp | MoveRoleSequenceDown,
		// Update the multiselect command filter constants in ORMDesignerDocView
		// when new commands are added
	}
	#endregion // ORMDesignerCommands enum
	/// <summary>
	/// <see cref="DiagramDocView"/> designed to contain multiple <see cref="ORMDiagram"/>s.
	/// </summary>
	[CLSCompliant(false)]
	public partial class ORMDesignerDocView : TabbedDiagramDocView, IORMSelectionContainer
	{
		#region Member variables
		private ORMDesignerCommands myEnabledCommands;
		private ORMDesignerCommands myVisibleCommands;
		private ORMDesignerCommands myCheckedCommands;
		private IServiceProvider myCtorServiceProvider;
		private IMonitorSelectionService myMonitorSelection;
		/// <summary>
		/// The filter for multi selection when the elements are all of the same type.
		/// </summary>
		private const ORMDesignerCommands EnabledSimpleMultiSelectCommandFilter =
			ORMDesignerCommands.DisplayStandardWindows |
			ORMDesignerCommands.CopyImage |
			ORMDesignerCommands.SelectAll |
			ORMDesignerCommands.AlignShapes |
			ORMDesignerCommands.AutoLayout |
			ORMDesignerCommands.AddInternalUniqueness |
			ORMDesignerCommands.ToggleSimpleMandatory |
			ORMDesignerCommands.DeleteAny |
			ORMDesignerCommands.DeleteAnyShape |
			ORMDesignerCommands.DeleteShape |
			(ORMDesignerCommands.Delete & ~ORMDesignerCommands.DeleteRole); // We don't allow deletion of the final role. Don't bother with sorting out the multiselect problems here
		/// <summary>
		/// The filter for multi selection when the elements are of different types. This should always be a subset of the simple command filter
		/// </summary>
		private const ORMDesignerCommands EnabledComplexMultiSelectCommandFilter = EnabledSimpleMultiSelectCommandFilter;
		/// <summary>
		/// A filter to turn off commands for a single selection
		/// </summary>
		private const ORMDesignerCommands DisabledSingleSelectCommandFilter =
			ORMDesignerCommands.AutoLayout |
			ORMDesignerCommands.AlignShapes;
		#endregion // Member variables
		#region Construction/destruction
		/// <summary>
		/// Standard DocView constructor, called by the editor factory
		/// </summary>
		/// <param name="docData">DocData</param>
		/// <param name="serviceProvider">IServiceProvider</param>
		public ORMDesignerDocView(DocData docData, IServiceProvider serviceProvider) : base(docData, serviceProvider)
		{
			myCtorServiceProvider = serviceProvider;
		}
		#endregion // Construction/destruction
		#region Base overrides
		/// <summary>
		/// See <see cref="TabbedDiagramDocView.CreateDiagram"/>.
		/// </summary>
		protected override Diagram CreateDiagram(Store store)
		{
			Diagram diagram = ORMDiagram.CreateORMDiagram(store);
			if (diagram.ModelElement == null)
			{
				// Make sure the diagram element is correctly attached to the model, and
				// create a model if we don't have one yet.
				IList elements = store.ElementDirectory.GetElements(ORMModel.MetaClassGuid, true);
				if (elements.Count == 0)
				{
					diagram.Associate(ORMModel.CreateORMModel(store));
				}
				else
				{
					Debug.Assert(elements.Count == 1);
					diagram.Associate((ModelElement)elements[0]);
				}
			}
			base.Diagrams.Add(diagram, true);
			return diagram;
		}
		/// <summary>
		/// See <see cref="TabbedDiagramDocView.LoadView"/>.
		/// </summary>
		protected override bool LoadView()
		{
			if (base.LoadView())
			{
				ORMDesignerDocData document = (ORMDesignerDocData)this.DocData;
				// Try to replace the default tab image with our tab image.
				// HACK: MSBUG: The TabStrip property on TabbedDiagramDocView is private, so we have to use reflection to get it
				System.Reflection.PropertyInfo tabStripPropertyInfo = typeof(TabbedDiagramDocView).GetProperty("TabStrip", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
				if (tabStripPropertyInfo != null)
				{
					TabStrip tabStrip = tabStripPropertyInfo.GetValue(this, null) as TabStrip;
					if (tabStrip != null)
					{
						System.Drawing.Bitmap tabImage = ResourceStrings.DiagramTabImage;
						tabStrip.TabImageList.Images.Clear();
						tabStrip.TabImageList.ImageSize = tabImage.Size;
						tabStrip.TabImageList.Images.Add(tabImage);
					}
				}

				// Add our existing diagrams, or make a new one if we don't already have one
				IList existingDiagrams = document.Store.ElementDirectory.GetElements(ORMDiagram.MetaClassGuid, true);
				if (existingDiagrams.Count > 0)
				{
					for (int i = 0; i < existingDiagrams.Count; i++)
					{
						// Make the first diagram be selected
						base.Diagrams.Add((Diagram)existingDiagrams[i], i == 0);
					}
				}
				else
				{
					base.AddDiagram();
				}

				// TODO: We don't know where this call should go, because we're not even sure what it does...
				// Make sure all of the shapes are set up correctly
				base.CurrentDiagram.PerformShapeAnchoringRule();

				// Make sure we get a closing notification so we can clear the
				// selected components
				document.DocumentClosing += new EventHandler(DocumentClosing);

				return true;
			}
			return false;
		}

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
		/// This is required when actions may update the currently
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
			ORMDesignerCommands checkableCommands = ORMDesignerCommands.None;
			ORMDesignerCommands checkedCommands = ORMDesignerCommands.None;
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

					// Running filters to ensure that tolerated commands don't indicate
					// a multi-select state when none is there.
					ORMDesignerCommands seenVisible = 0;
					ORMDesignerCommands seenVisibleOnce = 0;
					ORMDesignerCommands seenEnabled = 0;
					ORMDesignerCommands seenEnabledOnce = 0;
					ORMDesignerCommands seenTolerated = 0;

					ORMDesignerCommands currentVisible;
					ORMDesignerCommands currentEnabled;
					ORMDesignerCommands currentCheckable;
					ORMDesignerCommands currentChecked;
					ORMDesignerCommands currentTolerated;
					visibleCommands = enabledCommands = checkableCommands = checkedCommands = EnabledSimpleMultiSelectCommandFilter;
					Type firstType = null;
					bool isComplex = false;
					NodeShape primaryShape = PrimarySelectedShape;
					foreach (ModelElement melIter in GetSelectedComponents())
					{
						bool isPrimarySelection = false;
						ModelElement mel = melIter;
						PresentationElement pel = mel as PresentationElement;
						if (pel != null)
						{
							isPrimarySelection = primaryShape != null && object.ReferenceEquals(primaryShape, pel);
							mel = pel.ModelElement;
						}
						if (mel != null)
						{
							SetCommandStatus(mel, pel, isPrimarySelection, out currentVisible, out currentEnabled, out currentCheckable, out currentChecked, out currentTolerated);
							Debug.Assert(0 == (currentEnabled & ~currentVisible)); // Everthing enabled should be visible
							Debug.Assert(0 == (currentChecked & ~currentCheckable)); // Everything checked should be checkable

							if (firstType != null)
							{
								ORMDesignerCommands checkedConflict = checkedCommands & (currentCheckable & ~currentChecked);
								if (checkedConflict != 0)
								{
									// A single menu item has different checked states for different selected items
									ORMDesignerCommands turnOff = ~checkedConflict;
									enabledCommands &= turnOff;
									visibleCommands &= turnOff;
									checkableCommands &= turnOff;
									checkedCommands &= turnOff;
								}
							}
							if (!isComplex)
							{
								Type currentType = mel.GetType();
								if (firstType == null)
								{
									firstType = currentType;
								}
								else if (!object.ReferenceEquals(firstType, currentType))
								{
									isComplex = true;
									enabledCommands &= EnabledComplexMultiSelectCommandFilter;
									visibleCommands &= EnabledComplexMultiSelectCommandFilter;
									checkedCommands &= EnabledComplexMultiSelectCommandFilter;
									checkableCommands &= EnabledComplexMultiSelectCommandFilter;
								}
							}
							// Don't turn off tolerated commands, but don't turn them on either.
							enabledCommands &= currentEnabled | (enabledCommands & currentTolerated);
							visibleCommands &= currentVisible | (visibleCommands & currentTolerated);
							checkableCommands &= currentCheckable | (checkableCommands & currentTolerated);
							checkedCommands &= currentChecked | (checkedCommands & currentTolerated);
							if (enabledCommands == 0 && visibleCommands == 0)
							{
								break;
							}

							// With tolerated commands, it is possible that a multi-select command will have
							// a single selection plus other elements that tolerate it. These commands need
							// to be filtered out.
							seenTolerated |= currentTolerated;
							ORMDesignerCommands newCommands = currentEnabled & ~seenEnabled;
							ORMDesignerCommands oldCommands = currentEnabled & seenEnabled;
							seenEnabledOnce |= newCommands;
							seenEnabledOnce &= ~oldCommands;
							seenEnabled |= newCommands;

							// Repeat for visible
							newCommands = currentVisible & ~seenVisible;
							oldCommands = currentVisible & seenVisible;
							seenVisibleOnce |= newCommands;
							seenVisibleOnce &= ~oldCommands;
							seenVisible |= newCommands;
						}
					}
					enabledCommands &= ~(seenTolerated & ~seenEnabled);
					enabledCommands &= ~(seenEnabledOnce & DisabledSingleSelectCommandFilter);
					visibleCommands &= ~(seenTolerated & ~seenVisible);
					visibleCommands &= ~(seenVisibleOnce & DisabledSingleSelectCommandFilter);
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
							ORMDesignerCommands toleratedCommandsDummy;
							SetCommandStatus(mel, pel, true, out visibleCommands, out enabledCommands, out checkableCommands, out checkedCommands, out toleratedCommandsDummy);
							Debug.Assert(0 == (enabledCommands & ~visibleCommands)); // Everthing enabled should be visible
							Debug.Assert(0 == (checkedCommands & ~checkableCommands)); // Everything checked should be checkable
							visibleCommands &= ~DisabledSingleSelectCommandFilter;
							enabledCommands &= ~DisabledSingleSelectCommandFilter;
						}
					}
				}
			}
			myVisibleCommands = visibleCommands;
			myEnabledCommands = enabledCommands;
			myCheckedCommands = checkedCommands & visibleCommands;
		}
		/// <summary>
		/// Determine which commands are visible and enabled for the
		/// current state of an individual given element.
		/// </summary>
		/// <param name="element">A single model element. Should be a backing object, not a presentation element.</param>
		/// <param name="presentationElement">The selected presentation element representing the element. Can be null.</param>
		/// <param name="primarySelection">true if the presentation element is the primary selection</param>
		/// <param name="visibleCommands">(output) The set of visible commands</param>
		/// <param name="enabledCommands">(output) The set of enabled commands</param>
		/// <param name="checkableCommands">(output) The set of commands that are checked in some circumstances</param>
		/// <param name="checkedCommands">(output) The set of checked commands</param>
		/// <param name="toleratedCommands">(output) The set of commands allowed on other multi-selected elements that should not be turned off because this is included in the selection.</param>
		public virtual void SetCommandStatus(ModelElement element, PresentationElement presentationElement, bool primarySelection, out ORMDesignerCommands visibleCommands, out ORMDesignerCommands enabledCommands, out ORMDesignerCommands checkableCommands, out ORMDesignerCommands checkedCommands, out ORMDesignerCommands toleratedCommands)
		{
			enabledCommands = ORMDesignerCommands.None;
			visibleCommands = ORMDesignerCommands.None;
			checkableCommands = ORMDesignerCommands.None;
			checkedCommands = ORMDesignerCommands.None;
			toleratedCommands = ORMDesignerCommands.None;
			Role role;
			ObjectType objectType;
			NodeShape nodeShape;
			bool otherShape = false;
			if (element is FactType)
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DeleteFactType | ORMDesignerCommands.DeleteAny | ORMDesignerCommands.DisplayReadingsWindow | ORMDesignerCommands.DisplayFactEditorWindow;
				if (presentationElement is FactTypeShape)
				{
					visibleCommands |= ORMDesignerCommands.DeleteFactShape | ORMDesignerCommands.DeleteAnyShape | ORMDesignerCommands.AutoLayout | ORMDesignerCommands.AlignShapes | ORMDesignerCommands.CopyImage;
					enabledCommands |= ORMDesignerCommands.DeleteFactShape | ORMDesignerCommands.DeleteAnyShape | ORMDesignerCommands.AutoLayout | ORMDesignerCommands.AlignShapes | ORMDesignerCommands.CopyImage;
				}
				else if (null != presentationElement)
				{
					otherShape = true;
				}
			}
			else if (null != (objectType = element as ObjectType))
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DeleteObjectType | ORMDesignerCommands.DeleteAny;
				if (presentationElement is ObjectTypeShape)
				{
					visibleCommands |= ORMDesignerCommands.AutoLayout | ORMDesignerCommands.DeleteObjectShape | ORMDesignerCommands.DeleteAnyShape | ORMDesignerCommands.AlignShapes | ORMDesignerCommands.CopyImage;
					enabledCommands |= ORMDesignerCommands.AutoLayout | ORMDesignerCommands.DeleteObjectShape | ORMDesignerCommands.DeleteAnyShape | ORMDesignerCommands.AlignShapes | ORMDesignerCommands.CopyImage;
				}
				else if (presentationElement is ObjectifiedFactTypeNameShape)
				{
					// Treat deletion of ObjectifiedFactTypeNameShape is the same as deleting the associated FactShape
					visibleCommands |= ORMDesignerCommands.DeleteFactShape | ORMDesignerCommands.DeleteAnyShape;
					enabledCommands |= ORMDesignerCommands.DeleteFactShape | ORMDesignerCommands.DeleteAnyShape;
					toleratedCommands |= ORMDesignerCommands.AutoLayout | ORMDesignerCommands.CopyImage;
					if (!primarySelection)
					{
						toleratedCommands |= ORMDesignerCommands.AlignShapes;
					}
				}
				else if (null != presentationElement)
				{
					otherShape = true;
				}
			}
			else if (element is MultiColumnExternalConstraint || element is SingleColumnExternalConstraint)
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DeleteConstraint | ORMDesignerCommands.DeleteAny | ORMDesignerCommands.EditExternalConstraint;
				if (presentationElement is ExternalConstraintShape)
				{
					visibleCommands |= ORMDesignerCommands.DeleteConstraintShape | ORMDesignerCommands.DeleteAnyShape | ORMDesignerCommands.AlignShapes | ORMDesignerCommands.CopyImage | ORMDesignerCommands.AutoLayout;
					enabledCommands |= ORMDesignerCommands.DeleteConstraintShape | ORMDesignerCommands.DeleteAnyShape | ORMDesignerCommands.AlignShapes | ORMDesignerCommands.CopyImage | ORMDesignerCommands.AutoLayout;
				}
				else if (null != presentationElement)
				{
					otherShape = true;
				}
			}
			else if (element is InternalConstraint)
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DeleteConstraint | ORMDesignerCommands.DeleteAny;
				if (presentationElement != null)
				{
					toleratedCommands |= ORMDesignerCommands.DeleteShape | ORMDesignerCommands.DeleteAnyShape | ORMDesignerCommands.AutoLayout | ORMDesignerCommands.CopyImage;
				}
			}
			else if (element is ValueConstraint)
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DeleteConstraint | ORMDesignerCommands.DeleteAny;
				if (presentationElement != null)
				{
					toleratedCommands |= ORMDesignerCommands.DeleteShape | ORMDesignerCommands.DeleteAnyShape | ORMDesignerCommands.AutoLayout | ORMDesignerCommands.CopyImage;
					if (!primarySelection)
					{
						toleratedCommands |= ORMDesignerCommands.AlignShapes;
					}
				}
			}
			else if (element is ORMModel)
			{
				visibleCommands = ORMDesignerCommands.DisplayCustomReferenceModeWindow | ORMDesignerCommands.DisplayFactEditorWindow | ORMDesignerCommands.CopyImage;
				enabledCommands = ORMDesignerCommands.DisplayCustomReferenceModeWindow | ORMDesignerCommands.DisplayFactEditorWindow | ORMDesignerCommands.CopyImage;
			}
			else if (null != (role = element as Role))
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DisplayReadingsWindow | ORMDesignerCommands.InsertRole | ORMDesignerCommands.DeleteRole | ORMDesignerCommands.DisplayFactEditorWindow | ORMDesignerCommands.ToggleSimpleMandatory | ORMDesignerCommands.AddInternalUniqueness;
				checkableCommands = ORMDesignerCommands.ToggleSimpleMandatory;
				toleratedCommands |= ORMDesignerCommands.DeleteShape | ORMDesignerCommands.DeleteAnyShape | ORMDesignerCommands.CopyImage | ORMDesignerCommands.AutoLayout;
				if (role.IsMandatory)
				{
					checkedCommands = ORMDesignerCommands.ToggleSimpleMandatory;
				}
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

				if (null != (ormDiagram = CurrentDiagram as ORMDiagram))
				{
					FactTypeShape factShape;
					FactType fact;
					if (null != (fact = role.FactType) &&
						null != (factShape = ormDiagram.FindShapeForElement<FactTypeShape>(fact)))
					{
						UpdateMoveRoleCommandStatus(factShape, role, ref visibleCommands, ref enabledCommands);
					}

					if (null != (constraintShape = ormDiagram.StickyObject as ExternalConstraintShape)
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
			}
			else if ((null != (nodeShape = presentationElement as NodeShape)) &&
					!(nodeShape.ParentShape is Diagram))
			{
				otherShape = true;
			}
			if (otherShape)
			{
				toleratedCommands |=
					ORMDesignerCommands.AutoLayout |
					ORMDesignerCommands.CopyImage |
					ORMDesignerCommands.Delete |
					ORMDesignerCommands.DeleteAny |
					ORMDesignerCommands.DeleteShape |
					ORMDesignerCommands.DeleteAnyShape;
				if (!primarySelection)
				{
					toleratedCommands |= ORMDesignerCommands.AlignShapes;
				}
			}
			// Turn on the verbalization window command for all selections
			visibleCommands |= ORMDesignerCommands.DisplayStandardWindows | ORMDesignerCommands.SelectAll | ORMDesignerCommands.ExtensionManager | ORMDesignerCommands.ErrorList;
			enabledCommands |= ORMDesignerCommands.DisplayStandardWindows | ORMDesignerCommands.SelectAll | ORMDesignerCommands.ExtensionManager | ORMDesignerCommands.ErrorList;
		}
		private static void UpdateMoveRoleCommandStatus(FactTypeShape factShape, Role role, ref ORMDesignerCommands visibleCommands, ref ORMDesignerCommands enabledCommands)
		{
			RoleMoveableCollection roles = factShape.DisplayedRoleOrder;
			enabledCommands &= ~(ORMDesignerCommands.MoveRoleRight | ORMDesignerCommands.MoveRoleLeft);
			visibleCommands |= ORMDesignerCommands.MoveRoleLeft | ORMDesignerCommands.MoveRoleRight;
			int roleIndex = roles.IndexOf(role);
			int rolesCount = roles.Count;
			if (roleIndex != 0)
			{
				enabledCommands |= ORMDesignerCommands.MoveRoleLeft;
				if (rolesCount == 2)
				{
					visibleCommands &= ~ORMDesignerCommands.MoveRoleRight;
				}
			}
			if (roleIndex < (rolesCount - 1))
			{
				enabledCommands |= ORMDesignerCommands.MoveRoleRight;
				if (rolesCount == 2)
				{
					visibleCommands &= ~ORMDesignerCommands.MoveRoleLeft;
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
				IMonitorSelectionService monitorService = docView.MonitorSelectionService;
				if (monitorService != null && !object.ReferenceEquals(monitorService.CurrentSelectionContainer, docView))
				{
					ORMDesignerCommands activeFilter = ORMDesignerCommands.DisplayStandardWindows;
					commandFlag &= activeFilter;
				}
				command.Visible = 0 != (commandFlag & docView.myVisibleCommands);
				command.Enabled = 0 != (commandFlag & docView.myEnabledCommands);
				command.Checked = 0 != (commandFlag & docView.myCheckedCommands);
				if (0 != (commandFlag & (ORMDesignerCommands.Delete | ORMDesignerCommands.DeleteAny)))
				{
					docView.SetDeleteElementCommandText((OleMenuCommand)command);
				}
				else if (0 != (commandFlag & (ORMDesignerCommands.DeleteShape | ORMDesignerCommands.DeleteAnyShape)))
				{
					docView.SetDeleteShapeCommandText((OleMenuCommand)command);
				}
				else if (commandFlag == ORMDesignerCommands.ToggleSimpleMandatory && command.Enabled)
				{
					foreach (ModelElement mel in docView.GetSelectedComponents())
					{
						Role role = mel as Role;
						if (role != null)
						{
							// The command is only enabled when all selected roles have
							// the same mandatory state. A quick check will let us know when
							// the state has been changed.
							command.Checked = role.IsMandatory;
							break;
						}
					}
				}
				else if (0 != (commandFlag & (ORMDesignerCommands.MoveRoleLeft | ORMDesignerCommands.MoveRoleRight)))
				{
					foreach (ModelElement mel in docView.GetSelectedComponents())
					{
						Role role = mel as Role;
						if (role != null)
						{
							FactType fact;
							if (null != (fact = role.FactType))
							{
								((OleMenuCommand)sender).Text = (fact.RoleCollection.Count == 2) ? ResourceStrings.CommandSwapRoleOrderText : null;
							}
						}
					}
				}
				else if (commandFlag == ORMDesignerCommands.ErrorList && command.Enabled)
				{
					OleMenuCommand cmd = sender as OleMenuCommand;
					string errorText = null;
					int errorIndex = cmd.MatchedCommandId;
					foreach (ModelElement mel in docView.GetSelectedComponents())
					{
						IModelErrorOwner errorOwner = EditorUtility.ResolveContextInstance(mel, false) as IModelErrorOwner;
						if (errorOwner != null)
						{
							// UNDONE: ModelErrorUses filter
							foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.None))
							{
								if (errorIndex == 0)
								{
									errorText = error.Name;
									break;
								}
								--errorIndex;
							}
						}
					}
					if (errorText != null)
					{
						cmd.Enabled = true;
						cmd.Visible = true;
						cmd.Supported = true;
						cmd.Text = errorText;
					}
					else
					{
						cmd.Supported = false;
					}
				}
				else if (commandFlag == ORMDesignerCommands.AddInternalUniqueness && command.Enabled)
				{
					// Determine if a unique internal uniqueness constraint can
					// be added at this point.

					// Delay processing for this one until this point. There
					// is no need to run it whenever the selection changes to include
					// a role, given than it is only used when the context menu is opened.
					bool disable = false;
					bool hide = false;
					int selCount = docView.SelectionCount;
					if (selCount != 0)
					{
						Role[] roles = new Role[selCount];
						FactType fact = null;
						int currentRoleIndex = 0;
						foreach (ModelElement mel in docView.GetSelectedComponents())
						{
							Role role = mel as Role;
							if (role == null)
							{
								break;
							}
							FactType testFact = role.FactType;
							if (fact == null)
							{
								fact = testFact;
							}
							else if (!object.ReferenceEquals(fact, testFact))
							{
								fact = null;
								break;
							}
							roles[currentRoleIndex] = role;
							++currentRoleIndex;
						}
						if (currentRoleIndex == selCount && fact != null)
						{
							foreach (InternalUniquenessConstraint iuc in fact.GetInternalConstraints<InternalUniquenessConstraint>())
							{
								RoleMoveableCollection factRoles = iuc.RoleCollection;
								if (factRoles.Count == selCount)
								{
									int i = 0;
									for (; i < selCount; ++i)
									{
										if (!factRoles.Contains(roles[i]))
										{
											break;
										}
									}
									if (i == selCount)
									{
										disable = true;
										break;
									}
								}
							}
						}
						else
						{
							hide = true;
							disable = true;
						}
					}
					if (disable)
					{
						docView.myEnabledCommands &= ~ORMDesignerCommands.AddInternalUniqueness;
						command.Enabled = false;
						if (hide)
						{
							docView.myVisibleCommands &= ~ORMDesignerCommands.AddInternalUniqueness;
							command.Visible = false;
						}
					}
				}
			}
		}

		/// <summary>
		/// Set the menu's text for the delete element command
		/// </summary>
		/// <param name="command">OleMenuCommand</param>
		protected virtual void SetDeleteElementCommandText(OleMenuCommand command)
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
				commandText = ResourceStrings.CommandDeleteMultipleElementsText;
			}
			// Setting command.Text to null will pick up
			// the default command text
			command.Text = commandText;
		}
		/// <summary>
		/// Set the menu's text for the delete element command
		/// </summary>
		/// <param name="command">OleMenuCommand</param>
		protected virtual void SetDeleteShapeCommandText(OleMenuCommand command)
		{
			Debug.Assert(command != null);
			string commandText;
			switch (myVisibleCommands & ORMDesignerCommands.DeleteShape)
			{
				case ORMDesignerCommands.DeleteObjectShape:
					commandText = ResourceStrings.CommandDeleteObjectTypeShapeText;
					break;
				case ORMDesignerCommands.DeleteFactShape:
					commandText = ResourceStrings.CommandDeleteFactTypeShapeText;
					break;
				case ORMDesignerCommands.DeleteConstraintShape:
					commandText = ResourceStrings.CommandDeleteConstraintShapeText;
					break;
				default:
					commandText = null;
					break;
			}
			if (commandText == null && 0 != (myVisibleCommands & ORMDesignerCommands.DeleteAnyShape))
			{
				commandText = ResourceStrings.CommandDeleteMultipleShapesText;
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
		/// <param name="document">ORMDesignerDocData</param>
		public void InitializeView(ORMDesignerDocData document)
		{
			
		}
		private void DocumentClosing(object sender, EventArgs e)
		{
			(sender as DocData).DocumentClosing -= new EventHandler(DocumentClosing);
			SetSelectedComponents(null);
		}
		/// <summary>
		/// returns the monitor service
		/// </summary>
		protected IMonitorSelectionService MonitorSelectionService
		{
			get
			{
				IMonitorSelectionService monitorSelect = myMonitorSelection;
				if (monitorSelect == null)
				{
					myMonitorSelection = monitorSelect = (IMonitorSelectionService)myCtorServiceProvider.GetService(typeof(IMonitorSelectionService));
				}
				return monitorSelect;
			}
		}
		/// <summary>
		/// Execute the delete element command
		/// </summary>
		/// <param name="commandText">The text from the command</param>
		protected virtual void OnMenuDeleteElement(string commandText)
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
							if (mel != null && !mel.IsRemoved && !(mel is ReadingOrder)) // Reading orders tolerate delete, but are not deleted directly
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
		/// Execute the delete shape command
		/// </summary>
		/// <param name="commandText">The text from the command</param>
		protected virtual void OnMenuDeleteShape(string commandText)
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
				bool complexSelection = 0 == (enabledCommands & ORMDesignerCommands.DeleteShape);

				// Use the localized text from the command for our transaction name
				using (Transaction t = store.TransactionManager.BeginTransaction(commandText.Replace("&", "")))
				{
					// Note that we don't deal with QueuedSelection here like
					// we do in OnMenuDeleteElement because we only run this
					// command for top-level shape elements, so there is no
					// chance that we will have a parent other than the diagram
					// to select.
					FinalShapeDeleteBehavior finalDeleteBehavior = OptionsPage.CurrentFinalShapeDeleteBehavior;
					bool testMelDeletion = finalDeleteBehavior != FinalShapeDeleteBehavior.DeleteShapeOnly;
					foreach (ModelElement mel in GetSelectedComponents())
					{
						PresentationElement pel = mel as ShapeElement;
						ObjectType backingObjectifiedType = null;
						// ReadingShape and ValueConstraintShape tolerate deletion, but the
						// shapes cannot be deleted individually
						if (pel != null && !pel.IsRemoved)
						{
							ObjectifiedFactTypeNameShape objectifiedObjectShape;
							if (pel is ReadingShape || pel is ValueConstraintShape)
							{
								continue;
							}
							else if (null != (objectifiedObjectShape = pel as ObjectifiedFactTypeNameShape))
							{
								// The two parts of an objectification should always appear together,
								// pretend we're removing the fact shape
								pel = objectifiedObjectShape.ParentShape;
								if (pel == null)
								{
									continue;
								}
							}
							ModelElement backingMel = null;
							if (testMelDeletion)
							{
								backingMel = pel.ModelElement;
								FactType fact = backingMel as FactType;
								if (fact != null)
								{
									backingObjectifiedType = fact.NestingType;
								}
							}
							pel.Remove();
							if (backingMel != null && !backingMel.IsRemoved && backingMel.PresentationRolePlayers.Count == 0)
							{
								if (finalDeleteBehavior == FinalShapeDeleteBehavior.Prompt)
								{
									IVsUIShell shell;
									if (null != (shell = (IVsUIShell)ServiceProvider.GetService(typeof(IVsUIShell))))
									{
										Guid g = new Guid();
										int pnResult;
										shell.ShowMessageBox(0, ref g, ResourceStrings.PackageOfficialName,
											string.Format(CultureInfo.CurrentCulture, ResourceStrings.FinalShapeDeletionMessage, backingMel.GetClassName(), backingMel.GetComponentName()),
											"", 0, OLEMSGBUTTON.OLEMSGBUTTON_YESNO,
											OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_SECOND, OLEMSGICON.OLEMSGICON_QUERY, 0, out pnResult);
										if (pnResult == (int)DialogResult.No)
										{
											continue;
										}
									}
								}
								backingMel.Remove();
								if (backingObjectifiedType != null && !backingObjectifiedType.IsRemoved && backingObjectifiedType.PresentationRolePlayers.Count <= 1)
								{
									// Remove the corresponding backing objectified type. Removing the facttype shape pel
									// added a new shape for the object, so we expect 1 presentation role player here.
									backingObjectifiedType.Remove();
								}
							}
						}
					}
					if (t.HasPendingChanges)
					{
						t.Commit();

						// Clearing the selection selects the diagram
						CurrentDesigner.Selection.Clear();
					}
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
		/// Get the shape to use as the primary selection
		/// </summary>
		private NodeShape PrimarySelectedShape
		{
			get
			{
				NodeShape retVal = null;
				DiagramItem primaryItem = CurrentDesigner.Selection.PrimaryItem;
				if (primaryItem != null)
				{
					retVal = primaryItem.Shape as NodeShape;
				}
				if (retVal == null)
				{
					retVal = PrimarySelection as NodeShape;
				}
				return retVal;
			}
		}
		/// <summary>
		/// Execute the Align menu commands
		/// </summary>
		/// <param name="commandId">Standard command id. Expecting one of AlignBottom,
		/// AlignBottom, AlignHorizontalCenters, AlignLeft, AlignRight, AlignTop, AlignVerticalCenters
		/// </param>
		protected virtual void OnMenuAlignShapes(int commandId)
		{
			ICollection components;
			int selectionCount;
			NodeShape matchShape = PrimarySelectedShape;
			if (null != matchShape &&
				null != (components = GetSelectedComponents()) &&
				(selectionCount = components.Count) > 1)
			{
				FactTypeShape factShape;
				double alignTo;
				RectangleD matchBounds = matchShape.AbsoluteBoundingBox;
				switch (commandId)
				{
					case 1: // AlignBottom
						Debug.Assert(commandId == StandardCommands.AlignBottom.ID);
						alignTo = matchBounds.Bottom;
						break;
					case 2: // AlignHorizontalCenters
						Debug.Assert(commandId == StandardCommands.AlignHorizontalCenters.ID);
						alignTo = (null == (factShape = matchShape as FactTypeShape)) ? matchBounds.Center.Y : matchBounds.Top + factShape.RolesCenter.Y;
						break;
					case 3: // AlignLeft
						Debug.Assert(commandId == StandardCommands.AlignLeft.ID);
						alignTo = matchBounds.Left;
						break;
					case 4: // AlignRight
						Debug.Assert(commandId == StandardCommands.AlignRight.ID);
						alignTo = matchBounds.Right;
						break;
					case 6: // AlignTop
						Debug.Assert(commandId == StandardCommands.AlignTop.ID);
						alignTo = matchBounds.Top;
						break;
					case 7: // AlignVerticalCenters
						Debug.Assert(commandId == StandardCommands.AlignVerticalCenters.ID);
						alignTo = (null == (factShape = matchShape as FactTypeShape)) ? matchBounds.Center.X : matchBounds.Left + factShape.RolesCenter.X;
						break;
					default:
						return;
				}
				using (Transaction t = matchShape.Store.TransactionManager.BeginTransaction(ResourceStrings.AlignShapesTransactionName))
				{
					foreach (object component in components)
					{
						NodeShape shape = component as NodeShape;
						if (shape != null &&
							!object.ReferenceEquals(shape, matchShape) &&
							shape.ParentShape is Diagram)
						{
							RectangleD bounds = shape.AbsoluteBoundingBox;
							PointD newLocation = bounds.Location;
							switch (commandId)
							{
								case 1: // AlignBottom
									newLocation = new PointD(bounds.Left, alignTo - bounds.Height);
									break;
								case 2: // AlignHorizontalCenters
									newLocation = new PointD(bounds.Left, alignTo - ((null == (factShape = shape as FactTypeShape)) ? (bounds.Height / 2) : factShape.RolesCenter.Y));
									break;
								case 3: // AlignLeft
									newLocation = new PointD(alignTo, bounds.Top);
									break;
								case 4: // AlignRight
									newLocation = new PointD(alignTo - bounds.Width, bounds.Top);
									break;
								case 6: // AlignTop
									newLocation = new PointD(bounds.Left, alignTo);
									break;
								case 7: // AlignVerticalCenters
									newLocation = new PointD(alignTo - ((null == (factShape = shape as FactTypeShape)) ? (bounds.Width / 2) : factShape.RolesCenter.X), bounds.Top);
									break;
							}
							shape.Location = newLocation;
						}
					}
					if (t.HasPendingChanges)
					{
						t.Commit();
					}
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
					Store store = factType.Store;
					using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.InsertRoleTransactionName))
					{
						IDictionary contextInfo = t.TopLevelTransaction.Context.ContextInfo;
						if (insertAfter)
						{
							++insertIndex;
							contextInfo[FactTypeShape.InsertAfterRoleKey] = role;
						}
						else
						{
							contextInfo[FactTypeShape.InsertBeforeRoleKey] = role;
						}
						//bool aggressivelyKillValueType = store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.Contains(DeleteReferenceModeValueType);

						Role newRole = Role.CreateRole(store);
						roles.Insert(insertIndex, newRole);
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
		/// Execute the Toggle Simple Mandatory menu command
		/// </summary>
		protected virtual void OnMenuToggleSimpleMandatory()
		{
			ICollection components = GetSelectedComponents();
			foreach (object component in components)
			{
				Role role = component as Role;
				if (role != null)
				{
					// Use the standard property descriptor to pick up the
					// same transaction name, etc. This emulates toggling the
					// property in the properties window.
					role.CreatePropertyDescriptor(role.Store.MetaDataDirectory.FindMetaAttribute(Role.IsMandatoryMetaAttributeGuid), role).SetValue(role, !role.IsMandatory);
				}
			}
		}
		/// <summary>
		/// Execute the Add Internal Uniqueness menu command
		/// </summary>
		protected virtual void OnMenuAddInternalUniqueness()
		{
			ORMDiagram diagram;
			ORMModel model;
			if ((null != (diagram = CurrentDiagram as ORMDiagram)) &&
				(null != (model = diagram.ModelElement as ORMModel)))
			{
				Store store = model.Store;
				using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.AddInternalConstraintTransactionName))
				{
					FactType parentFact = null;
					RoleMoveableCollection constraintRoles = null;
					bool abort = false;
					foreach (ModelElement mel in GetSelectedComponents())
					{
						Role role = mel as Role;
						if (role != null)
						{
							FactType testFact = role.FactType;
							if (parentFact == null)
							{
								parentFact = testFact;
								InternalUniquenessConstraint iuc = InternalUniquenessConstraint.CreateInternalUniquenessConstraint(store);
								iuc.FactType = parentFact;
								constraintRoles = iuc.RoleCollection;
							}
							else if (!object.ReferenceEquals(testFact, parentFact))
							{
								abort = true; // Transaction will rollback when it disposes if we don't commit
								break;
							}
							constraintRoles.Add(role);
						}
					}
					if (!abort && t.HasPendingChanges)
					{
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
		/// <summary>
		/// Display the extension manager dialog for the current model
		/// </summary>
		protected virtual void OnMenuExtensionManager()
		{
			ExtensionManager.ShowDialog(ServiceProvider, this.DocData as ORMDesignerDocData);
		}
        /// <summary>
        /// Expand the context menu to display local errors
        /// </summary>
		/// <param name="errorIndex">Index of the error in the error collection</param>
        protected virtual void OnMenuErrorList(int errorIndex)
        {
			foreach (ModelElement mel in GetSelectedComponents())
			{
				IModelErrorOwner errorOwner = EditorUtility.ResolveContextInstance(mel, false) as IModelErrorOwner;
				if (errorOwner != null)
				{
					// UNDONE: ModelErrorUses filter
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.None))
					{
						if (errorIndex == 0)
						{
							IORMToolTaskItem task;
							IORMToolServices services;
							IORMToolTaskProvider provider;
							if (null != (task = error.TaskData as IORMToolTaskItem) &&
								null != (services = error.Store as IORMToolServices) &&
								null != (provider = services.TaskProvider))
							{
								provider.NavigateTo(task);
							}
							break;
						}
						--errorIndex;
					}
					break;
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
				// Get the links for which both endpoints are in our selection
				ArrayList selectedElements = this.SelectedElements.Clone() as ArrayList;
				for (int i = 0; i < selectedElements.Count; i++)
				{
					ShapeElement element = selectedElements[i] as ShapeElement;
					if (element != null)
					{
						foreach (ElementLink link in element.ModelElement.GetElementLinks())
						{
							ModelElement element1;
							ModelElement element2;
							element1 = link.GetRolePlayer(0);
							Role role1 = element1 as Role;
							if (role1 != null)
							{
								element1 = role1.FactType;
							}
							element2 = link.GetRolePlayer(1);
							Role role2 = element2 as Role;
							if (role2 != null)
							{
								element2 = role2.FactType;
							}

							foreach (PresentationElement presentationElement1 in element1.PresentationRolePlayers)
							{
								if (selectedElements.Contains(presentationElement1))
								{
									foreach (PresentationElement presentationElement2 in element2.PresentationRolePlayers)
									{
										if (selectedElements.Contains(presentationElement2))
										{
											selectedElements.AddRange(link.PresentationRolePlayers);
											break;
										}
									}
									break;
								}
							}
						}
					}
				}
#if !CUSTOM_COPY_IMAGE
				this.CurrentDiagram.CopyImageToClipboard(selectedElements);
#else
#if CUSTOM_COPY_IMAGE_VIA_MAKE_TRANSPARENT
				System.Drawing.Imaging.Metafile createdMetafile = this.CurrentDiagram.CreateMetafile(selectedElements);
				
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
				object[] parameters = new object[] { selectedElements, rect };

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
		/// Move the selected role to the left.
		/// </summary>
		protected virtual void OnMenuMoveRoleLeft(ORMDesignerDocView docView)
		{
			ORMDiagram diagram = (ORMDiagram)CurrentDiagram;
			foreach (ModelElement mel in docView.GetSelectedComponents())
			{
				Role role = mel as Role;
				if (role != null)
				{
					FactTypeShape factShape = diagram.FindShapeForElement<FactTypeShape>(role.FactType);
					if (null != factShape)
					{
						if (factShape.MoveRoleLeft(role))
						{
							UpdateMoveRoleCommandStatus(factShape, role, ref myVisibleCommands, ref myEnabledCommands);
						}
						return;
					}
				}
			}
		}
		/// <summary>
		/// Move the selected role to the right.
		/// </summary>
		protected virtual void OnMenuMoveRoleRight(ORMDesignerDocView docView)
		{
			ORMDiagram diagram = (ORMDiagram)CurrentDiagram;
			foreach (ModelElement mel in docView.GetSelectedComponents())
			{
				Role role = mel as Role;
				if (role != null)
				{
					FactTypeShape factShape = diagram.FindShapeForElement<FactTypeShape>(role.FactType);
					if (null != factShape)
					{
						if (factShape.MoveRoleRight(role))
						{
							UpdateMoveRoleCommandStatus(factShape, role, ref myVisibleCommands, ref myEnabledCommands);
						}
						return;
					}
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
