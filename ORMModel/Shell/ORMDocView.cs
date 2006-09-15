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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.ORM;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ObjectModel.Design;
using Neumont.Tools.ORM.ShapeModel;

namespace Neumont.Tools.ORM.Shell
{
	#region ORMDesignerCommands enum
	/// <summary>
	/// Valid commands
	/// </summary>
	[Flags]
	public enum ORMDesignerCommands : long
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
		/// Expand the error list for the selected object
		/// </summary>
		ErrorList = 0x10000000,
		/// <summary>
		/// Objectifies the fact type.
		/// </summary>
		ObjectifyFactType = 0x20000000,
		/// <summary>
		/// Delete Model Note
		/// </summary>
		DeleteModelNote = 0x40000000,
		/// <summary>
		/// Delete Model Note Shape
		/// </summary>
		DeleteModelNoteShape = 0x80000000,
		/// <summary>
		/// Delete Model Note Reference
		/// </summary>
		DeleteModelNoteReference = 0x100000000,
		/// <summary>
		/// Mask field representing individual delete commands
		/// </summary>
		Delete = DeleteObjectType | DeleteFactType | DeleteConstraint | DeleteRole | DeleteModelNote | DeleteModelNoteReference,
		/// <summary>
		/// Mask field representing individual shape delete commands
		/// </summary>
		DeleteShape = DeleteObjectShape | DeleteFactShape | DeleteConstraintShape | DeleteModelNoteShape,
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
	public partial class ORMDesignerDocView : Neumont.Tools.Modeling.Shell.MultiDiagramDocView, IORMSelectionContainer
	{
		#region Member variables
		private ORMDesignerCommands myEnabledCommands;
		private ORMDesignerCommands myVisibleCommands;
		private ORMDesignerCommands myCheckedCommands;
		private IServiceProvider myCtorServiceProvider;
		private IMonitorSelectionService myMonitorSelection;
		/// <summary>
		/// The filter for multi selection when the elements are all of the same type.`
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
		public ORMDesignerDocView(ModelingDocData docData, IServiceProvider serviceProvider) : base(docData, serviceProvider)
		{
			myCtorServiceProvider = serviceProvider;
		}
		#endregion // Construction/destruction
		#region Base overrides
		
		#region Context Menu
		/// <summary>
		/// When set as the <see cref="ToolStripItem.Tag"/> for a <see cref="ToolStripItem"/> in <see cref="P:ContextMenuStrip"/>,
		/// that <see cref="ToolStripItem"/> will be disabled if no tab is selected when the <see cref="T:ContextMenuStrip"/> is
		/// opened.
		/// </summary>
		public static readonly object ContextMenuItemNeedsSelectedTab = new object();
		private void ContextMenuOpening(object sender, EventArgs e)
		{
			ContextMenuStrip contextMenu = sender as ContextMenuStrip;
			bool haveSelectedTab = base.GetDesignerAtPoint(contextMenu.Location) != null;
			foreach (ToolStripItem item in contextMenu.Items)
			{
				if (item.Tag == ContextMenuItemNeedsSelectedTab)
				{
					item.Enabled = haveSelectedTab;
				}
			}
		}
		private void ContextMenuNewPageORMClick(object sender, EventArgs e)
		{
			Store store = base.DocData.Store;
			using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.DiagramCommandNewPage.Replace("&", "")))
			{
				ReadOnlyCollection<ORMModel> models = store.ElementDirectory.FindElements<ORMModel>();
				ORMDiagram diagram = new ORMDiagram(store);
				diagram.Associate(models.Count > 0 ? (ModelElement)models[0] : new ORMModel(store));
				t.Commit();
			}
		}
		private void ContextMenuDeletePageClick(object sender, EventArgs e)
		{
			ToolStrip senderOwner = ((ToolStripItem)sender).Owner;
			DiagramView designer = base.GetDesignerAtPoint(senderOwner.Location);
			if (designer != null)
			{
				Diagram diagram = designer.Diagram;
				Store store = diagram.Store;
				using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.DiagramCommandDeletePage.Replace("&", "")))
				{
					// UNDONE: MSBUG This rule should not be doing anything if the parent is deleted.
					// Causes diagram deletion to crash VS
					bool turnedOffLineRoutingRule = false;
					Type ruleType = typeof(Diagram).Assembly.GetType("Microsoft.VisualStudio.Modeling.Diagrams.LineRoutingRule");
					RuleManager ruleManager = store.RuleManager;
					try
					{
						if (ruleType != null)
						{
							ruleManager.DisableRule(ruleType);
							turnedOffLineRoutingRule = true;
						}
						diagram.Delete();
						t.Commit();
					}
					finally
					{
						if (turnedOffLineRoutingRule)
						{
							ruleManager.EnableRule(ruleType);
						}
					}
				}
			}
		}
		private void ContextMenuRenamePageClick(object sender, EventArgs e)
		{
			base.RenameDiagramAtPoint(((ToolStripItem)sender).Owner.Location);
		}
		#endregion // Context Menu
		
		#region HACK: Temporary RelationalView Context Menu stuff
#if !DISABLE_RELATIONAL_VIEW_HACK
		private void ContextMenuNewPageRelationalView(object sender, EventArgs e)
		{
			Store store = base.DocData.Store;
			using (Transaction t = store.TransactionManager.BeginTransaction("Relational View"))
			{
				ReadOnlyCollection<ModelElement> models = store.ElementDirectory.FindElements(new Guid("7FAEDEEC-0A27-4417-B74B-422A67A67F50"));
				if (models.Count == 0)
				{
					return;
				}
				System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				Type relationalDiagramType = null;
				Type relationalShapeModelType = null;
				Type relationalFixUpDiagram = null;
				Type relationalCompartmentItemAddRule = null;
				Type relationalCompartmentItemChangeRule = null;
				foreach (System.Reflection.Assembly assembly in assemblies)
				{
					if (assembly.FullName.StartsWith("Neumont.Tools.ORM.Views.RelationalView"))
					{
						relationalDiagramType = assembly.GetType("Neumont.Tools.ORM.Views.RelationalView.RelationalDiagram");
						relationalShapeModelType = assembly.GetType("Neumont.Tools.ORM.Views.RelationalView.RelationalShapeDomainModel");
						relationalFixUpDiagram = assembly.GetType("Neumont.Tools.ORM.Views.RelationalView.FixUpDiagram");
						relationalCompartmentItemAddRule = assembly.GetType("Neumont.Tools.ORM.Views.RelationalView.CompartmentItemAddRule");
						relationalCompartmentItemChangeRule = assembly.GetType("Neumont.Tools.ORM.Views.RelationalView.CompartmentItemChangeRule");
						break;
					}
				}
				Debug.Assert(relationalDiagramType != null && relationalShapeModelType != null && relationalFixUpDiagram != null && relationalCompartmentItemAddRule != null);
				Diagram diagram = (Diagram)Activator.CreateInstance(relationalDiagramType, store);
				diagram.Associate((ModelElement)models[0]);
				relationalShapeModelType.GetMethod("EnableDiagramRules", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).Invoke(null, new object[] { store });
				store.RuleManager.DisableRule(relationalFixUpDiagram);
				store.RuleManager.DisableRule(relationalCompartmentItemAddRule);
				store.RuleManager.DisableRule(relationalCompartmentItemChangeRule);
				t.Commit();
			}
			((IDisposable)sender).Dispose();
		}
#endif // !DISABLE_RELATIONAL_VIEW_HACK
		#endregion // HACK: Temporary RelationalView Context Menu stuff

		/// <summary>
		/// See <see cref="ModelingDocView.LoadView"/>.
		/// </summary>
		protected override bool LoadView()
		{
			if (base.LoadView())
			{
				ORMDesignerDocData document = (ORMDesignerDocData)this.DocData;

				base.RegisterImageForDiagramType(typeof(ORMDiagram), ResourceStrings.DiagramTabImage);

				#region Setup context menu
				ContextMenuStrip contextMenu = base.ContextMenuStrip = new ContextMenuStrip();
				contextMenu.ShowImageMargin = false;
				contextMenu.Opening += ContextMenuOpening;
				ToolStripMenuItem newPageMenuItem = new ToolStripMenuItem(ResourceStrings.DiagramCommandNewPage);
				ToolStripMenuItem deletePageMenuItem = new ToolStripMenuItem(ResourceStrings.DiagramCommandDeletePage);
				ToolStripMenuItem renamePageMenuItem = new ToolStripMenuItem(ResourceStrings.DiagramCommandRenamePage);
				ToolStripMenuItem ormDiagramMenuItem = new ToolStripMenuItem("&ORM");
				ormDiagramMenuItem.Image = ResourceStrings.DiagramTabImage;
				ormDiagramMenuItem.Click += ContextMenuNewPageORMClick;
				newPageMenuItem.DropDown = new ToolStripDropDown();
				newPageMenuItem.DropDownItems.Add(ormDiagramMenuItem);
				newPageMenuItem.DropDown.ImageScalingSize = DiagramImageSize;
				deletePageMenuItem.Click += ContextMenuDeletePageClick;
				deletePageMenuItem.Tag = ContextMenuItemNeedsSelectedTab;
				renamePageMenuItem.Click += ContextMenuRenamePageClick;
				renamePageMenuItem.Tag = ContextMenuItemNeedsSelectedTab;
				contextMenu.Items.AddRange(new ToolStripItem[] { newPageMenuItem, new ToolStripSeparator(), deletePageMenuItem, renamePageMenuItem });
				#endregion // Setup context menu

				#region HACK: Temporary RelationalView Context Menu stuff
#if !DISABLE_RELATIONAL_VIEW_HACK
				bool haveRelationalShapeDomainModel = false;
				foreach (DomainModel model in document.Store.DomainModels)
				{
					if (model.GetType().FullName == "Neumont.Tools.ORM.Views.RelationalView.RelationalShapeDomainModel")
					{
						haveRelationalShapeDomainModel = true;
						break;
					}
				}
				if (haveRelationalShapeDomainModel)
				{
					bool haveRelationalDiagram = false;
					foreach (Diagram diagram in document.Store.ElementDirectory.FindElements<Diagram>(true))
					{
						if (diagram.GetType().FullName == "Neumont.Tools.ORM.Views.RelationalView.RelationalDiagram")
						{
							haveRelationalDiagram = true;
							break;
						}
					}
					if (!haveRelationalDiagram)
					{
						ToolStripMenuItem relationalViewMenuItem = new ToolStripMenuItem("&Relational View");
						relationalViewMenuItem.Click += ContextMenuNewPageRelationalView;
						newPageMenuItem.DropDownItems.Add(relationalViewMenuItem);
					}
					contextMenu.Opening += delegate(object sender, CancelEventArgs e)
					{
						DiagramView designer = base.GetDesignerAtPoint(((ContextMenuStrip)sender).Location);
						if (designer != null)
						{
							renamePageMenuItem.Enabled = deletePageMenuItem.Enabled = designer.Diagram.GetType().FullName != "Neumont.Tools.ORM.Views.RelationalView.RelationalDiagram";
						}
					};
				}
#endif // !DISABLE_RELATIONAL_VIEW_HACK
				#endregion // HACK: Temporary RelationalView Context Menu stuff

				Store store = document.Store;
				// Add our existing diagrams, or make a new one if we don't already have one
				ReadOnlyCollection<Diagram> existingDiagrams = store.ElementDirectory.FindElements<Diagram>(true);
				int existingDiagramsCount = existingDiagrams.Count;
				bool addDefaultDiagram = existingDiagramsCount == 0;
				if (!addDefaultDiagram)
				{
					addDefaultDiagram = true;
					Partition defaultPartition = store.DefaultPartition;
					for (int i = 0; i < existingDiagramsCount; ++i)
					{
						Diagram existingDiagram = existingDiagrams[i];
						if (existingDiagram.Partition == defaultPartition)
						{
							// Make the first diagram be selected
							base.AddDiagram(existingDiagram, addDefaultDiagram);
							addDefaultDiagram = false;
						}
					}
				}
				if (addDefaultDiagram)
				{
					Diagram diagram = new ORMDiagram(store);
					if (diagram.ModelElement == null)
					{
						// Make sure the diagram element is correctly attached to the model, and
						// create a model if we don't have one yet.
						ReadOnlyCollection<ORMModel> elements = store.ElementDirectory.FindElements<ORMModel>(true);
						if (elements.Count <= 0)
						{
							diagram.Associate(new ORMModel(store));
						}
						else
						{
							Debug.Assert(elements.Count == 1);
							diagram.Associate(elements[0]);
						}
					}
					// The DocData events for adding new Diagrams to the DocView are not yet hooked up, so
					// we need to explicitly add it here.
					base.AddDiagram(diagram, true);
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
		protected override CommandID ContextMenuId
		{
			get
			{
				return default(CommandID);
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
							isPrimarySelection = primaryShape != null && primaryShape == pel;
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
								else if (firstType != currentType)
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
			FactType factType;
			Role role;
			ObjectType objectType;
			NodeShape nodeShape;
			SetConstraint setConstraint;
			bool otherShape = false;
			if (null != (factType = element as FactType))
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DeleteFactType | ORMDesignerCommands.DeleteAny | ORMDesignerCommands.DisplayReadingsWindow | ORMDesignerCommands.DisplayFactEditorWindow;
				Objectification objectification = factType.Objectification;
				if (objectification == null || objectification.IsImplied)
				{
					visibleCommands |= ORMDesignerCommands.ObjectifyFactType;
					enabledCommands |= ORMDesignerCommands.ObjectifyFactType;
				}
				if (presentationElement is FactTypeShape)
				{
					visibleCommands |= ORMDesignerCommands.DeleteFactShape | ORMDesignerCommands.DeleteAnyShape | ORMDesignerCommands.AutoLayout | ORMDesignerCommands.AlignShapes;
					enabledCommands |= ORMDesignerCommands.DeleteFactShape | ORMDesignerCommands.DeleteAnyShape | ORMDesignerCommands.AutoLayout | ORMDesignerCommands.AlignShapes;
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
					visibleCommands |= ORMDesignerCommands.AutoLayout | ORMDesignerCommands.DeleteObjectShape | ORMDesignerCommands.DeleteAnyShape | ORMDesignerCommands.AlignShapes;
					enabledCommands |= ORMDesignerCommands.AutoLayout | ORMDesignerCommands.DeleteObjectShape | ORMDesignerCommands.DeleteAnyShape | ORMDesignerCommands.AlignShapes;
				}
				else if (presentationElement is ObjectifiedFactTypeNameShape)
				{
					// Treat deletion of ObjectifiedFactTypeNameShape is the same as deleting the associated FactShape
					visibleCommands |= ORMDesignerCommands.DeleteFactShape | ORMDesignerCommands.DeleteAnyShape;
					enabledCommands |= ORMDesignerCommands.DeleteFactShape | ORMDesignerCommands.DeleteAnyShape;
					toleratedCommands |= ORMDesignerCommands.AutoLayout;
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
			else if (null != (setConstraint = element as SetConstraint) && setConstraint.Constraint.ConstraintIsInternal)
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DeleteConstraint | ORMDesignerCommands.DeleteAny;
				if (presentationElement != null)
				{
					toleratedCommands |= ORMDesignerCommands.DeleteShape | ORMDesignerCommands.DeleteAnyShape | ORMDesignerCommands.AutoLayout;
				}
			}
			else if (setConstraint != null || element is SetComparisonConstraint)
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DeleteConstraint | ORMDesignerCommands.DeleteAny | ORMDesignerCommands.EditExternalConstraint;
				if (presentationElement is ExternalConstraintShape)
				{
					visibleCommands |= ORMDesignerCommands.DeleteConstraintShape | ORMDesignerCommands.DeleteAnyShape | ORMDesignerCommands.AlignShapes | ORMDesignerCommands.AutoLayout;
					enabledCommands |= ORMDesignerCommands.DeleteConstraintShape | ORMDesignerCommands.DeleteAnyShape | ORMDesignerCommands.AlignShapes | ORMDesignerCommands.AutoLayout;
				}
				else if (null != presentationElement)
				{
					otherShape = true;
				}
			}
			else if (element is ValueConstraint)
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DeleteConstraint | ORMDesignerCommands.DeleteAny;
				if (presentationElement != null)
				{
					toleratedCommands |= ORMDesignerCommands.DeleteShape | ORMDesignerCommands.DeleteAnyShape | ORMDesignerCommands.AutoLayout;
					if (!primarySelection)
					{
						toleratedCommands |= ORMDesignerCommands.AlignShapes;
					}
				}
			}
			else if (element is ORMModel)
			{
				visibleCommands = ORMDesignerCommands.DisplayCustomReferenceModeWindow | ORMDesignerCommands.DisplayFactEditorWindow;
				enabledCommands = ORMDesignerCommands.DisplayCustomReferenceModeWindow | ORMDesignerCommands.DisplayFactEditorWindow;
			}
			else if (null != (role = element as Role))
			{
				FactType fact = role.FactType;
				if (fact == null)
				{
					// This is happening during teardown scenarios
					return;
				}

				visibleCommands = enabledCommands = ORMDesignerCommands.DisplayReadingsWindow | ORMDesignerCommands.InsertRole | ORMDesignerCommands.DeleteRole | ORMDesignerCommands.DisplayFactEditorWindow | ORMDesignerCommands.ToggleSimpleMandatory | ORMDesignerCommands.AddInternalUniqueness;
				checkableCommands = ORMDesignerCommands.ToggleSimpleMandatory;
				toleratedCommands |= ORMDesignerCommands.DeleteShape | ORMDesignerCommands.DeleteAnyShape | ORMDesignerCommands.AutoLayout;
				if (role.IsMandatory)
				{
					checkedCommands = ORMDesignerCommands.ToggleSimpleMandatory;
				}

				// Disable role deletion if the role count == 1
				visibleCommands |= ORMDesignerCommands.DeleteRole;
				if (fact.RoleCollection.Count == 1)
				{
					enabledCommands &= ~ORMDesignerCommands.DeleteRole;
				}

				Objectification objectification = fact.Objectification;
				if (objectification == null || objectification.IsImplied)
				{
					visibleCommands |= ORMDesignerCommands.ObjectifyFactType;
					enabledCommands |= ORMDesignerCommands.ObjectifyFactType;
				}

				// Extra menu commands may be visible if there is a StickyObject active on the diagram.
				ExternalConstraintShape constraintShape;
				IConstraint constraint;
				ORMDiagram ormDiagram;

				if (null != (ormDiagram = CurrentDiagram as ORMDiagram))
				{
					FactTypeShape factShape;
					if (null != (factShape = ormDiagram.FindShapeForElement<FactTypeShape>(fact)))
					{
						UpdateMoveRoleCommandStatus(factShape, role, ref visibleCommands, ref enabledCommands);
					}

					if (null != (constraintShape = ormDiagram.StickyObject as ExternalConstraintShape)
						&& null != (constraint = constraintShape.AssociatedConstraint))
					{
						bool thisRoleInConstraint = false;
						switch (constraint.ConstraintStorageStyle)
						{
							case ConstraintStorageStyle.SetConstraint:
								SetConstraint scec = constraint as SetConstraint;
								if (scec.RoleCollection.IndexOf(role) >= 0)
								{
									thisRoleInConstraint = true;
									visibleCommands |= ORMDesignerCommands.ActivateRoleSequence;
									enabledCommands |= ORMDesignerCommands.ActivateRoleSequence;
								}
								break;
							case ConstraintStorageStyle.SetComparisonConstraint:
								SetComparisonConstraint mcec = constraint as SetComparisonConstraint;
								int indexOfRole = -1;
								LinkedElementCollection<Role> currentRoleSequence = null;
								foreach (SetComparisonConstraintRoleSequence rs in mcec.RoleSequenceCollection)
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
			else if (element is ModelNote)
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DeleteModelNote | ORMDesignerCommands.DeleteAny;
				if (presentationElement is ModelNoteShape)
				{
					visibleCommands |= ORMDesignerCommands.DeleteModelNoteShape | ORMDesignerCommands.DeleteAnyShape | ORMDesignerCommands.AlignShapes | ORMDesignerCommands.AutoLayout;
					enabledCommands |= ORMDesignerCommands.DeleteModelNoteShape | ORMDesignerCommands.DeleteAnyShape | ORMDesignerCommands.AlignShapes | ORMDesignerCommands.AutoLayout;
				}
				else if (null != presentationElement)
				{
					otherShape = true;
				}
			}
			else if (element is ModelNoteReferencesModelElement)
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DeleteModelNoteReference | ORMDesignerCommands.DeleteAny;
				otherShape = true;
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
					ORMDesignerCommands.Delete |
					ORMDesignerCommands.DeleteAny |
					ORMDesignerCommands.DeleteShape |
					ORMDesignerCommands.DeleteAnyShape;
				if (!primarySelection)
				{
					toleratedCommands |= ORMDesignerCommands.AlignShapes;
				}
			}
			// Turn on standard commands for all selections
			visibleCommands |= ORMDesignerCommands.DisplayStandardWindows | ORMDesignerCommands.CopyImage | ORMDesignerCommands.SelectAll | ORMDesignerCommands.ExtensionManager | ORMDesignerCommands.ErrorList;
			enabledCommands |= ORMDesignerCommands.DisplayStandardWindows | ORMDesignerCommands.CopyImage | ORMDesignerCommands.SelectAll | ORMDesignerCommands.ExtensionManager | ORMDesignerCommands.ErrorList;
		}
		private static void UpdateMoveRoleCommandStatus(FactTypeShape factShape, Role role, ref ORMDesignerCommands visibleCommands, ref ORMDesignerCommands enabledCommands)
		{
			LinkedElementCollection<RoleBase> roles = factShape.DisplayedRoleOrder;
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
				if (monitorService != null && monitorService.CurrentSelectionContainer != docView)
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
							foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.DisplayPrimary))
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
							else if (fact != testFact)
							{
								fact = null;
								break;
							}
							roles[currentRoleIndex] = role;
							++currentRoleIndex;
						}
						if (currentRoleIndex == selCount && fact != null)
						{
							foreach (UniquenessConstraint iuc in fact.GetInternalConstraints<UniquenessConstraint>())
							{
								LinkedElementCollection<Role> factRoles = iuc.RoleCollection;
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
				case ORMDesignerCommands.DeleteModelNote:
					commandText = ResourceStrings.CommandDeleteModelNoteText;
					break;
				case ORMDesignerCommands.DeleteModelNoteReference:
					commandText = ResourceStrings.CommandDeleteModelNoteReferenceText;
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
				case ORMDesignerCommands.DeleteModelNoteShape:
					commandText = ResourceStrings.CommandDeleteModelNoteShapeText;
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
		#region SelectionContainer filtering
		/// <summary>
		/// Return the number of objects to display as part of the selection container
		/// </summary>
		protected override uint CountAllObjects()
		{
			ShapeElement startingShape = CurrentDiagram;
			return (startingShape != null) ? CountAllShapes(startingShape) : base.CountAllObjects();
		}
		/// <summary>
		/// Replacement helper function for DiagramDocView.CountShapes
		/// that respects the ISelectionContainerFilter interface
		/// </summary>
		/// <param name="parentShapeElement">The parent shape element.
		/// This is called recursively.</param>
		/// <returns>The number of selectable shapes</returns>
		private static uint CountAllShapes(ShapeElement parentShapeElement)
		{
			uint total = 0;
			if (parentShapeElement != null)
			{
				if (parentShapeElement.CanSelect)
				{
					ISelectionContainerFilter filter;
					if (null == (filter = parentShapeElement as ISelectionContainerFilter) ||
						filter.IncludeInSelectionContainer)
					{
						++total;
					}
				}
				foreach (ShapeElement nestedShape in parentShapeElement.NestedChildShapes)
				{
					total += CountAllShapes(nestedShape);
				}
				foreach (ShapeElement relativeShape in parentShapeElement.RelativeChildShapes)
				{
					total += CountAllShapes(relativeShape);
				}
			}
			return total;
		}
		/// <summary>
		/// Return all objects to the selection container
		/// </summary>
		protected override void GetAllObjects(uint count, object[] objects)
		{
			ShapeElement startingShape = CurrentDiagram;
			if (startingShape != null)
			{
				GetAllShapes(startingShape, 0, count, objects);
			}
			else
			{
				base.GetAllObjects(count, objects);
			}
		}
		/// <summary>
		/// Replacement helper function for DiagramDocView.GrabObjects
		/// that respects the ISelectionContainerFilter interface
		/// </summary>
		/// <param name="parentShapeElement">The parent shape element.
		/// This is called recursively.</param>
		/// <param name="index">The next index to populate</param>
		/// <param name="count">The total number of available slots.
		/// The count value is based on the CountAllObjects implementation.</param>
		/// <param name="shapes">An array of all shape objects to return.</param>
		/// <returns>The number of selectable shapes</returns>
		private static uint GetAllShapes(ShapeElement parentShapeElement, uint index, uint count, object[] shapes)
		{
			if (index < count)
			{
				if (parentShapeElement == null)
				{
					return index;
				}
				if (parentShapeElement.CanSelect)
				{
					ISelectionContainerFilter filter;
					if (null == (filter = parentShapeElement as ISelectionContainerFilter) ||
						filter.IncludeInSelectionContainer)
					{
						shapes[index] = parentShapeElement;
						++index;
					}
				}
				foreach (ShapeElement element1 in parentShapeElement.NestedChildShapes)
				{
					index = GetAllShapes(element1, index, count, shapes);
					if (index >= count)
					{
						return index;
					}
				}
				foreach (ShapeElement element2 in parentShapeElement.RelativeChildShapes)
				{
					index = GetAllShapes(element2, index, count, shapes);
					if (index >= count)
					{
						return index;
					}
				}
			}
			return index;
		}
		#endregion // SelectionContainer filtering
		#region ORMDesignerDocView Specific
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
				return myMonitorSelection ?? (myMonitorSelection = (IMonitorSelectionService)myCtorServiceProvider.GetService(typeof(IMonitorSelectionService)));
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
					Dictionary<object, object> contextInfo = t.TopLevelTransaction.Context.ContextInfo;
					#if QUEUEDSELECTION
					// UNDONE: 2006-06 DSL Tools port: QueuedSelection doesn't seem to exist any more. What do we replace it with?
					IList queuedSelection = docData.QueuedSelection as IList;
					#endif // QUEUEDSELECTION
					// account for multiple selection
					foreach (object selectedObject in GetSelectedComponents())
					{
						ShapeElement pel; // just the shape
						ModelElement mel;
						bool deleteReferenceModeValueTypeInContext = false;
						if (null != (pel = selectedObject as ShapeElement))
						{
							// Note that if the ORMDiagram.SelectionRules property is overridden
							// or any shape overrides the AllowChildrenInSelection property, then
							// the delete propagation on child shapes can force the pel to be deleted
							// without deleting the underlying mel. This would require resolving all
							// model elements before any pels are deleting because the pel could
							// now be deleted without the underlying mel having been touched.
							if (pel.IsDeleted)
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
							if (mel != null && !mel.IsDeleted && !(mel is ReadingOrder)) // Reading orders tolerate delete, but are not deleted directly
							{
								// Check if the object shape was in expanded mode
								bool testRefModeCollapse = complexSelection || 0 != (enabledCommands & ORMDesignerCommands.DeleteObjectType);
								ObjectTypeShape objectShape;
								ObjectifiedFactTypeNameShape objectifiedShape;
								if (testRefModeCollapse &&
									((null != (objectShape = pel as ObjectTypeShape) &&
									!objectShape.ExpandRefMode) ||
									(null != (objectifiedShape = pel as ObjectifiedFactTypeNameShape) &&
									!objectifiedShape.ExpandRefMode))
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

								// Get rid of the model element. Delete propagation on the PresentationViewsSubject
								// relationship will automatically delete the pel.
								mel.Delete();
							}
						}
						else if (null != (mel = selectedObject as ModelElement) && !mel.IsDeleted)
						{
#if QUEUEDSELECTION
							// The object was selected directly (through a shape field or sub field element)
							ModelElement shapeAssociatedMel = null;
							if (complexSelection)
							{
								SetConstraint ic;
								LinkedElementCollection<FactType> facts;
								Role role;
								if (null != (ic = selectedObject as SetConstraint) &&
									ic.Constraint.ConstraintIsInternal &&
									1 == (facts = ic.FactTypeCollection).Count)
								{
									shapeAssociatedMel = facts[0];
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
										{
											SetConstraint setConstraint;
											LinkedElementCollection<FactType> facts;
											if (null != (setConstraint = selectedObject as SetConstraint) &&
												setConstraint.Constraint.ConstraintIsInternal &&
												1 == (facts = setConstraint.FactTypeCollection).Count)
											{
												shapeAssociatedMel = facts[0];
											}
										}
										break;
								}
							}

							// Add the parent shape into the queued selection
							if (shapeAssociatedMel != null)
							{
								pel = (CurrentDiagram as ORMDiagram).FindShapeForElement(shapeAssociatedMel);
								if (pel != null && !pel.IsDeleted)
								{
									queuedSelection.Add(pel);
								}
							}
#endif // QUEUEDSELECTION

							// Remove the item
							mel.Delete();
						}
					}

					if (t.HasPendingChanges)
					{
#if QUEUEDSELECTION
						if (complexSelection)
						{
							for (int i = queuedSelection.Count - 1; i >= 0; --i)
							{
								if (((ModelElement)queuedSelection[i]).IsDeleted)
								{
									queuedSelection.RemoveAt(i);
								}
							}
						}
						if (queuedSelection.Count == 0 && d != null)
						{
							queuedSelection.Add(d);
						}
#endif // QUEUEDSELECTION
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
			int pelCount = SelectionCount;
			if (pelCount > 0)
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
						if (pel != null && !pel.IsDeleted)
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
							pel.Delete();
							int newPelCount;
							LinkedElementCollection<PresentationElement> remainingPels;
							if (backingMel != null && !backingMel.IsDeleted && (newPelCount = (remainingPels = PresentationViewsSubject.GetPresentation(backingMel)).Count) != 0)
							{
								Partition partition = store.DefaultPartition;
								for (int i = newPelCount - 1; i >= 0; --i)
								{
									if (remainingPels[i].Partition != partition)
									{
										--newPelCount;
									}
								}

								if (newPelCount == 0)
								{
									if (finalDeleteBehavior == FinalShapeDeleteBehavior.Prompt)
									{
										IVsUIShell shell;
										if (null != (shell = (IVsUIShell)ServiceProvider.GetService(typeof(IVsUIShell))))
										{
											Guid g = new Guid();
											int pnResult;
											shell.ShowMessageBox(0, ref g, ResourceStrings.PackageOfficialName,
												string.Format(CultureInfo.CurrentCulture, ResourceStrings.FinalShapeDeletionMessage, TypeDescriptor.GetClassName(backingMel), TypeDescriptor.GetComponentName(backingMel)),
												"", 0, OLEMSGBUTTON.OLEMSGBUTTON_YESNO,
												OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_SECOND, OLEMSGICON.OLEMSGICON_QUERY, 0, out pnResult);
											if (pnResult == (int)DialogResult.No)
											{
												continue;
											}
										}
									}
									backingMel.Delete();
									if (backingObjectifiedType != null && !backingObjectifiedType.IsDeleted && PresentationViewsSubject.GetPresentation(backingObjectifiedType).Count <= 1)
									{
										// Remove the corresponding backing objectified type. Removing the facttype shape pel
										// added a new shape for the object, so we expect 1 presentation role player here.
										backingObjectifiedType.Delete();
									}
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
			LinkedElementCollection<ShapeElement> nestedShapes;
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
					// Firing n(n+1)/2 selection change events is very
					// expensive, especially for verbalization.
					ShapeElement currentShape = nestedShapes[i];
					if (currentShape.CanSelect)
					{
						DiagramItem newItem = new DiagramItem(currentShape);
						if (firstItem)
						{
							firstItem = false;
							shapes.DeferredClearBeforeAdditions();
							shapes.DeferredAdd(newItem);
							shapes.DeferredPrimaryItem(newItem);
						}
						else
						{
							shapes.DeferredAdd(newItem);
						}
					}
				}
				if (!firstItem)
				{
					shapes.SetDeferredSelection();
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
				RectangleD matchBounds = matchShape.AbsoluteBounds;
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
							shape != matchShape &&
							shape.ParentShape is Diagram)
						{
							RectangleD bounds = shape.AbsoluteBounds;
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
				RoleBase role = null;
				foreach (object component in components)
				{
					role = component as RoleBase;
					break;
				}
				FactType factType;
				if (role != null &&
					null != (factType = role.FactType))
				{
					LinkedElementCollection<RoleBase> roles = factType.RoleCollection;
					int insertIndex = roles.IndexOf(role);
					Store store = factType.Store;
					using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.InsertRoleTransactionName))
					{
						Dictionary<object, object> contextInfo = t.TopLevelTransaction.Context.ContextInfo;
						if (insertAfter)
						{
							++insertIndex;
							contextInfo[FactTypeShape.InsertAfterRoleKey] = role;
						}
						else
						{
							contextInfo[FactTypeShape.InsertBeforeRoleKey] = role;
						}
						//bool aggressivelyKillValueType = store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.ContainsKey(DeleteReferenceModeValueType);

						Role newRole = new Role(store);
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
			foreach (object component in GetSelectedComponents())
			{
				Role role = component as Role;
				if (role != null)
				{
					// Use the standard property descriptor to pick up the
					// same transaction name, etc. This emulates toggling the
					// property in the properties window.
					DomainTypeDescriptor.CreatePropertyDescriptor(role, Role.IsMandatoryDomainPropertyId).SetValue(role, !role.IsMandatory);
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
					LinkedElementCollection<Role> constraintRoles = null;
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
								UniquenessConstraint iuc = UniquenessConstraint.CreateInternalUniquenessConstraint(parentFact);
								constraintRoles = iuc.RoleCollection;
							}
							else if (testFact != parentFact)
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
					SetConstraint scec;
					//SetComparisonConstraint mcec;
					if (null != (scec = constraint as SetConstraint))
					{
						connectAction.ConstraintRoleSequenceToEdit = scec;
					}
					//else if (null != (mcec = constraint as SetComparisonConstraint))
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
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.DisplayPrimary))
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
			Diagram diagram;
			DiagramView diagramView;
			if (null != (diagram = CurrentDiagram) &&
				null != (diagramView = diagram.ActiveDiagramView))
			{
				SelectedShapesCollection selectedShapes = diagramView.DiagramClientView.Selection;
				Dictionary<ShapeElement, ShapeElement> shapesDictionary = null;
				foreach (DiagramItem diagramItem in selectedShapes)
				{
					// Note that we ignore any field/subfield portions of
					// diagramItem. These are automatically copied as part of the shape.
					ShapeElement shape = diagramItem.Shape;
					if (shape == diagram)
					{
						// In this case, we want to copy the nested children
						// of the diagram, not the diagram itself. Copying the
						// diagram gets all of the extra whitespace in the diagram itself.
						// Diagrams don't have relative children, so just do the nested.
						diagram.CopyImageToClipboard(diagram.NestedChildShapes);
						return;
					}
					shape = ResolveTopLevelShape(shape, diagram);
					if (shapesDictionary == null)
					{
						shapesDictionary = new Dictionary<ShapeElement, ShapeElement>(selectedShapes.Count * 2);
					}
					if (!shapesDictionary.ContainsKey(shape))
					{
						shapesDictionary.Add(shape, shape);
					}
				}
				if (shapesDictionary != null)
				{
					Dictionary<ShapeElement, ShapeElement>.ValueCollection values = shapesDictionary.Values;

					// We have the top-level shapes, now go on a link walk to
					// find all links not currently selected that are linked
					// directly or indirectly at both ends to a selected shape.
					int topLevelShapesCount = values.Count;
					ShapeElement[] topLevelShapes = new ShapeElement[topLevelShapesCount];
					values.CopyTo(topLevelShapes, 0);
					AddSharedLinkShapes(shapesDictionary, topLevelShapes, diagram);
#if !CUSTOM_COPY_IMAGE
					diagram.CopyImageToClipboard(values);
#else
#if CUSTOM_COPY_IMAGE_VIA_MAKE_TRANSPARENT
				System.Drawing.Imaging.Metafile createdMetafile = diagram.CreateMetafile(values);
				
				NativeMethods.MakeBackgroundTransparent(createdMetafile);
				NativeMethods.CopyMetafileToClipboard(diagramView.Handle, createdMetafile);
#else
				System.Reflection.BindingFlags bindingFlags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
				
				Type diagramType = typeof(Microsoft.VisualStudio.Modeling.Diagrams.Diagram);
				System.Reflection.MethodInfo getShapesToDraw = diagramType.GetMethod("GetShapesToDraw", bindingFlags);
				if (getShapesToDraw == null)
				{
					throw new MissingMethodException(diagramType.FullName, "GetShapesToDraw");
				}
								
				RectangleD rect = default(RectangleD);
				object[] parameters = new object[] { values, rect };

				ArrayList shapesToDraw = getShapesToDraw.Invoke(diagram, parameters) as ArrayList;
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
						if (!shapeElement.IsDeleted)
						{
							shapeElement.OnPaintShape(diagramPaintEventArgs);
						}
					}
				}
				
				NativeMethods.CopyMetafileToClipboard(diagramView.Handle, metafile);
#endif
#endif
				}
			}
		}
		/// <summary>
		/// Helper function for OnMenuCopyImage. Recursively add link shapes
		/// to the shapes to draw.
		/// </summary>
		/// <param name="shapesDictionary">A dictionary of shapes currently being drawn</param>
		/// <param name="shapes">The set of shapes to walk and attach links to.</param>
		/// <param name="diagram">The current diagram</param>
		private static void AddSharedLinkShapes(Dictionary<ShapeElement, ShapeElement> shapesDictionary, IList<ShapeElement> shapes, Diagram diagram)
		{
			int shapesCount = shapes.Count;
			for (int i = 0; i < shapesCount; ++i)
			{
				ShapeElement shape = shapes[i];
				NodeShape nodeShape = shape as NodeShape;
				if (null != nodeShape)
				{
					LinkedElementCollection<LinkShape> linkShapes = LinkConnectsToNode.GetLink(nodeShape);
					int linkShapeCount = linkShapes.Count;
					for (int j = 0; j < linkShapeCount; ++j)
					{
						LinkShape currentLinkShape = linkShapes[j];
						if (shapesDictionary.ContainsKey(currentLinkShape))
						{
							continue;
						}
						LinkedElementCollection<NodeShape> nodes = currentLinkShape.Nodes;
						int nodesCount = nodes.Count;
						for (int k = 0; k < nodesCount; ++k)
						{
							NodeShape currentNode = nodes[k];
							if (currentNode != nodeShape)
							{
								ShapeElement resolvedShape = ResolveTopLevelShape(currentNode, diagram);
								LinkShape resolvedLinkShape;
								if (shapesDictionary.ContainsKey(resolvedShape))
								{
									shapesDictionary.Add(currentLinkShape, currentLinkShape);
								}
								else if (null != (resolvedLinkShape = resolvedShape as LinkShape))
								{
									// Check independently if the link can be added. This removes
									// selection order dependencies for links connected to links.
									LinkedElementCollection<NodeShape> secondaryNodes = resolvedLinkShape.Nodes;
									int secondaryNodesCount = secondaryNodes.Count;
									int l = 0;
									for (; l < secondaryNodesCount; ++l)
									{
										if (!shapesDictionary.ContainsKey(ResolveTopLevelShape(secondaryNodes[l], diagram)))
										{
											break;
										}
									}
									if (l == secondaryNodesCount)
									{
										shapesDictionary.Add(resolvedLinkShape, resolvedLinkShape);
										shapesDictionary.Add(currentLinkShape, currentLinkShape);
									}
								}
							}
						}
					}
				}
				AddSharedLinkShapes(shapesDictionary, shape.NestedChildShapes, diagram);
				AddSharedLinkShapes(shapesDictionary, shape.RelativeChildShapes, diagram);
			}
		}
		/// <summary>
		/// Helper function for OnMenuCopyImage. Given a shape and a diagram,
		/// find the shape that is an ancestor (or self) of the shape and a
		/// direct child of the diagram.
		/// </summary>
		/// <param name="shape">A shape element</param>
		/// <param name="diagram">The containing diagram</param>
		/// <returns>The top level shape, or the starting shape if resolution fails (very unlikely)</returns>
		private static ShapeElement ResolveTopLevelShape(ShapeElement shape, Diagram diagram)
		{
			ShapeElement startShape = shape;
			ShapeElement parentShape = shape.ParentShape;
			while (parentShape != diagram)
			{
				if (parentShape == null)
				{
					return startShape;
				}
				shape = parentShape;
				parentShape = shape.ParentShape;
			}
			return shape;
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
					if (constraint == sequence.Constraint)
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
				SetComparisonConstraint mcec;
				if (null != (role = SelectedElements[0] as Role)
					&& null != (ormDiagram = CurrentDiagram as ORMDiagram)
					&& null != (ecs = ormDiagram.StickyObject as ExternalConstraintShape)
					&& null != (mcec = ecs.AssociatedConstraint as SetComparisonConstraint))
				{
					// TODO:  It is theoretically possible to have one role playing a part in multiple
					// RoleSequences for a constraint.  At some point it would probably be nice to
					// decide which RoleSequence is active and blow that one away instead of just
					// walking the RoleSequenceCollection and killing any RoleSequence that has
					// reference to this role.

					LinkedElementCollection<ConstraintRoleSequence> roleConstraints = role.ConstraintRoleSequenceCollection;

					int constraintCount = roleConstraints.Count;
					using (Transaction t = role.Store.TransactionManager.BeginTransaction(ResourceStrings.DeleteRoleSequenceTransactionName))
					{
						for (int i = constraintCount - 1; i >= 0; --i)
						{
							// The current ConstraintRoleSequence is the one associated with the current StickyObject.
							if ((roleConstraints[i]).Constraint == mcec)
							{
								// TODO: Remove the ConstraintRoleSequence from this role.
								roleConstraints[i].Delete();
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
					"Multiselect deletion of role sequences is not implemented. " +
					"If you see this message, decide if what you're doing is really a valid operation. " +
					"If it is, look in Shell\\ORMCommandSet.cs, OnMenuDeleteRowSequence() to implement it.");
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
			SetComparisonConstraint mcec;
			if (null != (role = SelectedElements[0] as Role)
				&& null != (ormDiagram = CurrentDiagram as ORMDiagram)
				&& null != (ecs = ormDiagram.StickyObject as ExternalConstraintShape)
				&& null != (mcec = ecs.AssociatedConstraint as SetComparisonConstraint))
			{
				LinkedElementCollection<SetComparisonConstraintRoleSequence> roleSequences = mcec.RoleSequenceCollection;
				SetComparisonConstraintRoleSequence sequenceToMove = null;
				int sequenceOriginalPosition = 0;
				int sequenceNewPosition = -1;
				int lastPosition = roleSequences.Count - 1;
				foreach (SetComparisonConstraintRoleSequence sequence in roleSequences)
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
			SetComparisonConstraint mcec;
			if (null != (role = SelectedElements[0] as Role)
				&& null != (ormDiagram = CurrentDiagram as ORMDiagram)
				&& null != (ecs = ormDiagram.StickyObject as ExternalConstraintShape)
				&& null != (mcec = ecs.AssociatedConstraint as SetComparisonConstraint))
			{

				LinkedElementCollection<SetComparisonConstraintRoleSequence> roleSequences = mcec.RoleSequenceCollection;
				SetComparisonConstraintRoleSequence sequenceToMove = null;
				int sequenceOriginalPosition = 0;
				int sequenceNewPosition = -1;
				int lastPosition = roleSequences.Count - 1;
				foreach (SetComparisonConstraintRoleSequence sequence in roleSequences)
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
		protected virtual void OnMenuMoveRoleLeft()
		{
			ORMDiagram diagram = (ORMDiagram)CurrentDiagram;
			foreach (ModelElement mel in GetSelectedComponents())
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
		protected virtual void OnMenuMoveRoleRight()
		{
			ORMDiagram diagram = (ORMDiagram)CurrentDiagram;
			foreach (ModelElement mel in GetSelectedComponents())
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
		/// Objectifies the selected fact type.
		/// </summary>
		protected virtual void OnMenuObjectifyFactType()
		{
			ORMDiagram diagram = CurrentDiagram as ORMDiagram;
			if (diagram != null)
			{
				foreach (ModelElement mel in GetSelectedComponents())
				{
					FactType factType = ORMEditorUtility.ResolveContextFactType(mel);
					if (factType != null)
					{
						Store store = factType.Store;
						using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.ObjectifyFactTypeTransactionName))
						{
							Objectification objectification = factType.Objectification;
							if (objectification != null)
							{
								Debug.Assert(objectification.IsImplied);
								// Set the objectifying type to not be independent, which breaks the implication pattern and makes
								// the objectification change to be explicit
								objectification.NestingType.IsIndependent = false;
							}
							else
							{
								Objectification.CreateObjectificationForFactType(factType, false, null);
							}
							if (t.HasPendingChanges)
							{
								t.Commit();
								myEnabledCommands &= ~ORMDesignerCommands.ObjectifyFactType;
								myVisibleCommands &= ~ORMDesignerCommands.ObjectifyFactType;
							}
						}
						// Once we've objectified a fact type, we're done
						break;
					}
				}
			}
		}
		/// <summary>
		/// Open a new window on this document
		/// </summary>
		protected virtual void OnMenuNewWindow()
		{
			IServiceProvider serviceProvider = ServiceProvider;
			IVsUIShellOpenDocument openDoc;
			IVsRunningDocumentTable rdt;
			if (null != (openDoc = (IVsUIShellOpenDocument)serviceProvider.GetService(typeof(IVsUIShellOpenDocument))) &&
				null != (rdt = (IVsRunningDocumentTable)serviceProvider.GetService(typeof(IVsRunningDocumentTable))))
			{
				IntPtr punkDocData = IntPtr.Zero;
				try
				{
					uint grfRDTFlags;
					uint dwReadLocks;
					uint dwEditLocks;
					string bstrMkDocument;
					IVsHierarchy hier;
					uint[] itemId = new uint[1];
					ErrorHandler.ThrowOnFailure(rdt.GetDocumentInfo(
						DocData.Cookie,
						out grfRDTFlags,
						out dwReadLocks,
						out dwEditLocks,
						out bstrMkDocument,
						out hier,
						out itemId[0],
						out punkDocData));
					Guid logicalView = Guid.Empty;
					IVsUIHierarchy uiHier;
					IVsWindowFrame pWindowFrame;
					int fOpen;
					ErrorHandler.ThrowOnFailure(openDoc.IsDocumentOpen(
						hier as IVsUIHierarchy,
						itemId[0],
						bstrMkDocument,
						ref logicalView,
						(uint)__VSIDOFLAGS.IDO_IgnoreLogicalView, // Corresponds to GUID_NULL for logical view
						out uiHier,
						itemId,
						out pWindowFrame,
						out fOpen));
					IVsWindowFrame pNewWindowFrame;
					ErrorHandler.ThrowOnFailure(openDoc.OpenCopyOfStandardEditor(
						pWindowFrame,
						ref logicalView,
						out pNewWindowFrame));
					ErrorHandler.ThrowOnFailure(pNewWindowFrame.Show());
				}
				finally
				{
					if (punkDocData != IntPtr.Zero)
					{
						Marshal.Release(punkDocData);
					}
				}
			}
		}
		#endregion // ORMDesignerDocView Specific
	}
}
