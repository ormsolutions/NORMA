#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © ORM Solutions, LLC. All rights reserved.                        *
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
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using ORMSolutions.ORMArchitect.Framework.Shell;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Core.ShapeModel;
using ORMSolutions.ORMArchitect.Framework;
using Microsoft.VisualStudio.Shell.Interop;

namespace ORMSolutions.ORMArchitect.Core.Shell
{
	#region ORMDesignerCommands enum
	/// <summary>
	/// Valid commands
	/// </summary>
	[Flags]
	[Serializable]
	public enum ORMDesignerCommands : long
	{
		/// <summary>
		/// Commands not set
		/// </summary>
		None = 0L,
		/// <summary>
		/// Deletion of one or more object types is enabled
		/// </summary>
		DeleteObjectType = 1L << 0,
		/// <summary>
		/// Deletion of one or more fact types is enabled
		/// </summary>
		DeleteFactType = 1L << 1,
		/// <summary>
		/// Deletion of one or more constraints is enabled
		/// </summary>
		DeleteConstraint = 1L << 2,
		/// <summary>
		/// Support all zoom commands
		/// </summary>
		Zoom = 1L << 3,
		/// <summary>
		/// Check the selected object for the <see cref="IFreeFormCommandProvider{Store}"/> interface
		/// </summary>
		FreeFormCommandList = 1L << 4,
		/// <summary>
		/// Insert a role before or after the current role
		/// </summary>
		InsertRole = 1L << 5,
		/// <summary>
		/// Delete the current role
		/// </summary>
		DeleteRole = 1L << 6,

		// 1L << 7 is available

		/// <summary>
		/// Activate editing for the RoleSequence
		/// </summary>
		ActivateRoleSequence = 1L << 8,
		/// <summary>
		/// Delete the RoleSequence
		/// </summary>
		DeleteRoleSequence = 1L << 9,
		/// <summary>
		/// Roll the RoleSequence up (lower number) in the active Constraint's RoleSequenceCollection
		/// </summary>
		MoveRoleSequenceUp = 1L << 10,
		/// <summary>
		/// Roll the RoleSequence down (higher number) in the active Constraint's RoleSequenceCollection
		/// </summary>
		MoveRoleSequenceDown = 1L << 11,
		/// <summary>
		/// Activate editing for an external constraint or an internal uniqueness constraint
		/// </summary>
		EditRoleSequenceConstraint = 1L << 12,
		/// <summary>
		/// Display standard toolwindows that we never disable.
		/// This currently maps to the Verbalization and Model Browser windows
		/// </summary>
		DisplayStandardWindows = 1L << 13,
		/// <summary>
		/// Select all top level selectable elements on the current diagram
		/// </summary>
		SelectAll = 1L << 14,
		/// <summary>
		/// Special command used in addition to the specific Delete elements.
		/// DeleteAny will survive most complex multi-select cases whereas the Delete
		/// will not. This is handled specially for the delete case.
		/// </summary>
		DeleteAny = 1L << 15,
		/// <summary>
		/// Apply an auto-layout algorithm to the selection. Applies to top-level objects.
		/// </summary>
		AutoLayout = 1L << 16,
		/// <summary>
		/// Toggle the IsMandatory property on the selected role. Applies to a single role.
		/// </summary>
		ToggleSimpleMandatory = 1L << 17,
		/// <summary>
		/// Add an internal uniqueness constraint for the selected roles.
		/// Applies to one or more roles from the same fact type.
		/// </summary>
		AddInternalUniqueness = 1L << 18,
		/// <summary>
		/// Display the ExtensionManager dialog
		/// </summary>
		ExtensionManager = 1L << 19,
		/// <summary>
		/// Support the CopyImage command
		/// </summary>
		CopyImage = 1L << 20,
		/// <summary>
		/// Delete an object shape
		/// </summary>
		DeleteObjectShape = 1L << 21,
		/// <summary>
		/// Delete a fact shape
		/// </summary>
		DeleteFactShape = 1L << 22,
		/// <summary>
		/// Delete a constraint shape
		/// </summary>
		DeleteConstraintShape = 1L << 23,
		/// <summary>
		/// Special command used in addition to the specific Delete*Shape elements.
		/// DeleteAnyShape will survive most complex multi-select cases whereas the Delete*Shape
		/// will not. This is handled specially for the delete case.
		/// </summary>
		DeleteAnyShape = 1L << 24,
		/// <summary>
		/// Align top level shape elements. Applies to all of the standard Format.Align commands.
		/// </summary>
		AlignShapes = 1L << 25,
		/// <summary>
		/// Move a role's order to the left within the fact type.
		/// </summary>
		MoveRoleLeft = 1L << 26,
		/// <summary>
		/// Move a role's order to the right within the fact type.
		/// </summary>
		MoveRoleRight = 1L << 27,
		/// <summary>
		/// Expand the error list for the selected object
		/// </summary>
		ErrorList = 1L << 28,
		/// <summary>
		/// Objectifies the fact type.
		/// </summary>
		ObjectifyFactType = 1L << 29,
		/// <summary>
		/// Delete Model Note
		/// </summary>
		DeleteModelNote = 1L << 30,
		/// <summary>
		/// Delete Model Note Shape
		/// </summary>
		DeleteModelNoteShape = 1L << 31,
		/// <summary>
		/// Delete Model Note Reference
		/// </summary>
		DeleteModelNoteReference = 1L << 32,
		/// <summary>
		/// Rotate a fact type shape to a horizontal orientation
		/// </summary>
		DisplayOrientationHorizontal = 1L << 33,
		/// <summary>
		/// Rotate a fact type shape to a left vertical orientation
		/// </summary>
		DisplayOrientationRotatedLeft = 1L << 34,
		/// <summary>
		/// Rotate a fact type shape to a right vertical orientation
		/// </summary>
		DisplayOrientationRotatedRight = 1L << 35,
		/// <summary>
		/// Display constraints on the top of the fact type shape
		/// </summary>
		DisplayConstraintsOnTop = 1L << 36,
		/// <summary>
		/// Display constraints on the bottom of the fact type shape
		/// </summary>
		DisplayConstraintsOnBottom = 1L << 37,
		/// <summary>
		/// Reverse the role order on the fact type shape
		/// </summary>
		DisplayReverseRoleOrder = 1L << 38,
		/// <summary>
		/// Couple a MandatoryConstraint and an ExclusionConstraint into an ExclusiveOr constraint
		/// </summary>
		ExclusiveOrCoupler = 1L << 39,
		/// <summary>
		/// Separate an ExclusiveOr constraint coupling
		/// </summary>
		ExclusiveOrDecoupler = 1L << 40,
		/// <summary>
		/// Support label editing
		/// </summary>
		EditLabel = 1L << 41,
		/// <summary>
		/// Expand the diagram list for the selected shape
		/// </summary>
		DiagramList = 1L << 42,
		/// <summary>
		/// Run the report generator
		/// </summary>
		ReportGeneratorList = 1L << 43,
		/// <summary>
		/// Unobjectifies the fact type.
		/// </summary>
		UnobjectifyFactType = 1L << 44,
		/// <summary>
		/// Select a shape in the diagram spy window
		/// </summary>
		SelectInDiagramSpy = 1L << 45,
		/// <summary>
		/// Select the current shape in the diagram view window
		/// </summary>
		SelectInDocumentWindow = 1L << 46,
		/// <summary>
		/// Delete a group
		/// </summary>
		DeleteGroup = 1L << 47,
		/// <summary>
		/// Remove a referenced element from a group. This
		/// may result in either deletion of exclusion, depending
		/// on whether or not the element is explicitly included
		/// by one or more associated group types.
		/// </summary>
		RemoveFromGroup = 1L << 48,
		/// <summary>
		/// Include an explicitly excluded element in a group.
		/// </summary>
		IncludeInGroup = 1L << 49,
		/// <summary>
		/// Include selected elements in a new group and select the group.
		/// </summary>
		IncludeInNewGroup = 1L << 50,
		/// <summary>
		/// Get a list of available groups to include selected elements in
		/// </summary>
		IncludeInGroupList = 1L << 51,
		/// <summary>
		/// Get a list of groups the element is directly included in
		/// </summary>
		DeleteFromGroupList = 1L << 52,
		/// <summary>
		/// Activate the current selection in the model browser window
		/// </summary>
		SelectInModelBrowser = 1L << 53,
		/// <summary>
		/// Activate the next shape in the same diagram corresponding the current backing element
		/// </summary>
		SelectNextInCurrentDiagram = 1L << 54,
		/// <summary>
		/// Show errors that are disabled for this item
		/// </summary>
		DisabledErrorList = 1L << 55,
		/// <summary>
		/// Get a list of groups the element is directly included in
		/// </summary>
		SelectInGroupList = 1L << 56,
		/// <summary>
		/// Mask field representing individual delete commands
		/// </summary>
		Delete = DeleteObjectType | DeleteFactType | DeleteConstraint | DeleteRole | DeleteModelNote | DeleteModelNoteReference | DeleteGroup | RemoveFromGroup,
		/// <summary>
		/// Mask field representing individual shape delete commands
		/// </summary>
		DeleteShape = DeleteObjectShape | DeleteFactShape | DeleteConstraintShape | DeleteModelNoteShape,
		/// <summary>
		/// Mask field representing individual RoleSeqeuence edit commands
		/// </summary>
		RoleSequenceActions = ActivateRoleSequence | DeleteRoleSequence | MoveRoleSequenceUp | MoveRoleSequenceDown,
		/// <summary>
		/// Mask field representing individual DisplayOrientation edit commands
		/// </summary>
		DisplayOrientation = DisplayOrientationHorizontal | DisplayOrientationRotatedLeft | DisplayOrientationRotatedRight,
		/// <summary>
		/// Mask field representing individual DisplayConstraintsOn edit commands
		/// </summary>
		DisplayConstraintsPosition = DisplayConstraintsOnTop | DisplayConstraintsOnBottom,
		// Update the multiselect command filter constants in ORMDesignerDocView
		// when new commands are added
	}
	#endregion // ORMDesignerCommands enum
	/// <summary>
	/// <see cref="DiagramDocView"/> designed to contain multiple <see cref="ORMDiagram"/>s.
	/// </summary>
	[CLSCompliant(false)]
	public partial class ORMDesignerDocView : MultiDiagramDocView, IORMSelectionContainer, IORMDesignerView, IModelingEventSubscriber, IVsToolboxUser
	{
		#region Member variables
		private IServiceProvider myCtorServiceProvider;
		private IMonitorSelectionService myMonitorSelectionService;
		private ORMDesignerCommandManager myCommandManager;
		#endregion // Member variables
		#region Construction/destruction
		/// <summary>
		/// Standard DocView constructor, called by the editor factory
		/// </summary>
		/// <param name="docData">DocData</param>
		/// <param name="serviceProvider">IServiceProvider</param>
		public ORMDesignerDocView(ModelingDocData docData, IServiceProvider serviceProvider)
			: base(docData, serviceProvider)
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
			MultiDiagramContextMenuStrip contextMenu = ContextMenuStrip;
			ToolStripItemCollection menuItems = contextMenu.Items;
			Diagram diagram = contextMenu.SelectedDiagram;
			bool haveSelectedTab = diagram != null;
			foreach (ToolStripItem item in menuItems)
			{
				if (item.Tag == ContextMenuItemNeedsSelectedTab)
				{
					item.Enabled = haveSelectedTab;
				}
			}

			Partition partition = this.DocData.Store.DefaultPartition;

			//Disable/Enable New Diagram Tabs stuff wow
			ToolStripDropDownItem newPageMenuItem = (ToolStripDropDownItem)menuItems[ResourceStrings.DiagramCommandNewPage];
			ToolStripItemCollection items = newPageMenuItem.DropDownItems;
			int itemCount = items.Count;

			for (int i = 0; i < itemCount; i++)
			{
				object[] itemInfo = (object[])items[i].Tag;
				DomainClassInfo diagramInfo = (DomainClassInfo)itemInfo[1];
				ReadOnlyCollection<ModelElement> modelElements = partition.ElementDirectory.FindElements(diagramInfo);
				DiagramMenuDisplayAttribute attribute = (DiagramMenuDisplayAttribute)itemInfo[0];
				if (modelElements.Count > 0 & (attribute.DiagramOption & DiagramMenuDisplayOptions.AllowMultiple) == 0)
				{
					//DISABLE NEW
					items[i].Enabled = false;
				}
				else
				{
					//ENABLE NEW
					items[i].Enabled = true;
				}
			}

			// Disable page reordering if there is only one page
			string reorderKey = ResourceStrings.DiagramCommandReorderPages;
			if (menuItems.ContainsKey(reorderKey))
			{
				menuItems[reorderKey].Enabled = HasMultiplePages;
			}


			//If a diagram tab is selected
			if (diagram != null)
			{
				//Retrieve all existing diagrams of the same type as the one selected
				ReadOnlyCollection<ModelElement> modelElements = partition.ElementDirectory.FindElements(diagram.GetDomainClass(), true);
				DomainClassInfo diagramInfo = diagram.GetDomainClass();

				//Grab the attribute of the diagram type selected
				object[] attributes = diagramInfo.ImplementationClass.GetCustomAttributes(typeof(DiagramMenuDisplayAttribute), false);
				if (attributes.Length > 0)
				{
					DiagramMenuDisplayAttribute attribute = (DiagramMenuDisplayAttribute)attributes[0];
					//If required but you only have 1 then disable delete
					if ((attribute.DiagramOption & DiagramMenuDisplayOptions.Required) != 0 && modelElements.Count <= 1)
					{
						//DISABLE DELETE
						ToolStripMenuItem deletePageMenuItem = (ToolStripMenuItem)menuItems[ResourceStrings.DiagramCommandDeletePage];
						deletePageMenuItem.Enabled = false;
					}
					else
					{
						//ALLOW DELETE
						ToolStripMenuItem deletePageMenuItem = (ToolStripMenuItem)menuItems[ResourceStrings.DiagramCommandDeletePage];
						deletePageMenuItem.Enabled = true;
					}

					if ((attribute.DiagramOption & DiagramMenuDisplayOptions.BlockRename) != 0)
					{
						//DISABLE RENAME
						ToolStripMenuItem renamePageMenuItem = (ToolStripMenuItem)menuItems[ResourceStrings.DiagramCommandRenamePage];
						renamePageMenuItem.Enabled = false;
					}
					else
					{
						//ALLOW RENAME
						ToolStripMenuItem renamePageMenuItem = (ToolStripMenuItem)menuItems[ResourceStrings.DiagramCommandRenamePage];
						renamePageMenuItem.Enabled = true;
					}
				}
			}
		}
		private void ContextMenuNewPageClick(object sender, EventArgs e)
		{
			ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
			if (menuItem != null)
			{
				Store store = base.DocData.Store;
				using (Transaction t = store.TransactionManager.BeginTransaction(menuItem.Text.Replace("&", string.Empty)))
				{
					object[] itemInfo;
					if (null != (itemInfo = menuItem.Tag as object[]))
					{
						DomainClassInfo diagramInfo = (DomainClassInfo)itemInfo[1];
						DiagramMenuDisplayAttribute attribute = (DiagramMenuDisplayAttribute)itemInfo[0];
						IDiagramInitialization initializer = attribute.CreateInitializer(diagramInfo.ImplementationClass);
						Diagram newDiagram = (Diagram)store.ElementFactory.CreateElement(diagramInfo);
						if (initializer != null)
						{
							initializer.InitializeDiagram(newDiagram);
						}
						Diagram selectedDiagram;
						DiagramDisplay displayContainer;
						LinkedElementCollection<Diagram> diagramOrder;
						int selectedDiagramIndex;
						if (null != (selectedDiagram = ContextMenuStrip.SelectedDiagram) &&
							null != store.FindDomainModel(DiagramDisplayDomainModel.DomainModelId) &&
							null != (displayContainer = DiagramDisplayHasDiagramOrder.GetDiagramDisplay(selectedDiagram)) &&
							(selectedDiagramIndex = (diagramOrder = displayContainer.OrderedDiagramCollection).IndexOf(selectedDiagram)) < (diagramOrder.Count - 1))
						{
							// Add the page immediately after the selected diagram, unless we're already at the end, in which
							// case we leave it to the rules to add in the correct place
							diagramOrder.Insert(selectedDiagramIndex + 1, newDiagram);
						}
						t.Commit();
					}
				}
			}
		}
		private void ContextMenuDeletePageClick(object sender, EventArgs e)
		{
			Diagram diagram = ContextMenuStrip.SelectedDiagram;
			if (diagram != null)
			{
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
			base.RenameDiagram(ContextMenuStrip.SelectedDiagram);
		}
		private void ContextMenuPageOrderClick(object sender, EventArgs e)
		{
			base.ReorderDiagrams(ContextMenuStrip.SelectedDiagram);
		}
		#endregion // Context Menu

		/// <summary>
		/// See <see cref="ModelingDocView.LoadView"/>.
		/// </summary>
		protected override bool LoadView()
		{
			if (base.LoadView())
			{
				ORMDesignerDocData document = (ORMDesignerDocData)this.DocData;
				#region Setup context menu
				ContextMenuStrip contextMenu = base.ContextMenuStrip = new MultiDiagramContextMenuStrip();
				contextMenu.ShowImageMargin = false;
				contextMenu.Opening += ContextMenuOpening;
				ToolStripMenuItem newPageMenuItem = new ToolStripMenuItem(ResourceStrings.DiagramCommandNewPage);
				ToolStripMenuItem deletePageMenuItem = new ToolStripMenuItem(ResourceStrings.DiagramCommandDeletePage);
				ToolStripMenuItem renamePageMenuItem = new ToolStripMenuItem(ResourceStrings.DiagramCommandRenamePage);
				newPageMenuItem.DropDown = new ToolStripDropDown();
				ToolStripItemCollection items = newPageMenuItem.DropDownItems;

				ReadOnlyCollection<DomainClassInfo> diagrams = document.Store.DomainDataDirectory.FindDomainClass(Diagram.DomainClassId).AllDescendants;
				int diagramCount = diagrams.Count;
				for (int i = 0; i < diagramCount; ++i)
				{
					DomainClassInfo diagramInfo = diagrams[i];
					object[] attributes = diagramInfo.ImplementationClass.GetCustomAttributes(typeof(DiagramMenuDisplayAttribute), false);
					if (attributes.Length > 0)
					{
						DiagramMenuDisplayAttribute attribute = (DiagramMenuDisplayAttribute)attributes[0];
						Image image = attribute.TabImage;
						string name = attribute.DisplayName;
						if (string.IsNullOrEmpty(name))
						{
							name = diagramInfo.DisplayName;
						}
						if (image != null)
						{
							base.RegisterImageForDiagramType(diagramInfo.ImplementationClass, image);
						}
						ToolStripMenuItem newDiagramMenuItem = new ToolStripMenuItem(name, image, ContextMenuNewPageClick);
						newDiagramMenuItem.Tag = new object[] { attribute, diagramInfo };
						items.Add(newDiagramMenuItem);
					}
				}
				newPageMenuItem.Name = ResourceStrings.DiagramCommandNewPage;
				newPageMenuItem.DropDown.ImageScalingSize = DiagramImageSize;

				deletePageMenuItem.Name = ResourceStrings.DiagramCommandDeletePage;
				deletePageMenuItem.Click += ContextMenuDeletePageClick;
				deletePageMenuItem.Tag = ContextMenuItemNeedsSelectedTab;

				renamePageMenuItem.Name = ResourceStrings.DiagramCommandRenamePage;
				renamePageMenuItem.Click += ContextMenuRenamePageClick;
				renamePageMenuItem.Tag = ContextMenuItemNeedsSelectedTab;

				if (null != Store.FindDomainModel(DiagramDisplayDomainModel.DomainModelId))
				{
					ToolStripMenuItem reorderPagesMenuItem = new ToolStripMenuItem(ResourceStrings.DiagramCommandReorderPages);
					reorderPagesMenuItem.Name = ResourceStrings.DiagramCommandReorderPages;
					reorderPagesMenuItem.Click += ContextMenuPageOrderClick;

					contextMenu.Items.AddRange(new ToolStripItem[] { newPageMenuItem, reorderPagesMenuItem, new ToolStripSeparator(), deletePageMenuItem, renamePageMenuItem });
				}
				else
				{
					contextMenu.Items.AddRange(new ToolStripItem[] { newPageMenuItem, new ToolStripSeparator(), deletePageMenuItem, renamePageMenuItem });
				}
				#endregion // Setup context menu

				Store store = document.Store;
				// Add our existing diagrams. If we have the diagram display model extension turned on
				// for this designer, then we will always have an ordered set of diagrams at this point.
				if (null != (store.FindDomainModel(DiagramDisplayDomainModel.DomainModelId)))
				{
					ReadOnlyCollection<DiagramDisplay> diagramDisplayElements = store.ElementDirectory.FindElements<DiagramDisplay>(false);
					if (diagramDisplayElements.Count != 0)
					{
						IList<DiagramDisplayHasDiagramOrder> existingDiagramLinks = DiagramDisplayHasDiagramOrder.GetLinksToOrderedDiagramCollection(diagramDisplayElements[0]);
						int existingDiagramsCount = existingDiagramLinks.Count;
						if (existingDiagramsCount != 0)
						{
							Partition defaultPartition = store.DefaultPartition;
							for (int i = 0; i < existingDiagramsCount; ++i)
							{
								DiagramDisplayHasDiagramOrder link = existingDiagramLinks[i];
								Diagram existingDiagram = link.Diagram;
								if (existingDiagram.Partition == defaultPartition)
								{
									// The fixup listeners guarantee that there is exactly one active diagram,
									// so we can safely trust the property to get an active diagram
									base.AddDiagram(existingDiagram, link.IsActiveDiagram);
								}
							}
						}
					}
				}
				else
				{
					// Add our existing diagrams
					ReadOnlyCollection<Diagram> existingDiagrams = store.ElementDirectory.FindElements<Diagram>(true);
					int existingDiagramsCount = existingDiagrams.Count;
					if (existingDiagramsCount != 0)
					{
						bool seenDiagram = false;
						Partition defaultPartition = store.DefaultPartition;
						for (int i = 0; i < existingDiagramsCount; ++i)
						{
							Diagram existingDiagram = existingDiagrams[i];
							if (existingDiagram.Partition == defaultPartition)
							{
								// Make the first diagram be selected
								base.AddDiagram(existingDiagram, !seenDiagram);
								seenDiagram = true;
							}
						}
					}
				}

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
				return ORMDesignerCommandIds.ViewContextMenu;
			}
		}

		/// <summary>
		/// String indicating the toolbox tab name that should be selected when this view gets focus.
		/// </summary>
		protected override string DefaultToolboxTabName
		{
			get
			{
				return ORMShapeToolboxHelper.DefaultToolboxTabName;
			}
		}

		/// <summary>
		/// See <see cref="ModelingDocView.DefaultToolboxTabToolboxItemsCount"/>.
		/// </summary>
		protected override int DefaultToolboxTabToolboxItemsCount
		{
			get
			{
				return ORMShapeToolboxHelper.DefaultToolboxTabToolboxItemsCount;
			}
		}

		/// <summary>
		/// Handle right-clicks on the diagram
		/// </summary>
		/// <param name="mouseArgs"></param>
		protected override void OnContextMenuRequested(DiagramPointEventArgs mouseArgs)
		{
			// Command status is set when the selection is changed
			if (CommandManager.HasEnabledCommands)
			{
				base.OnContextMenuRequested(mouseArgs);
			}
			else
			{
				mouseArgs.Handled = true;
			}
		}
		/// <summary>
		/// Enable menu commands when the selection changes
		/// </summary>
		protected override void OnSelectionChanged(EventArgs e)
		{
			if (Store == null)
			{
				// This gets occasional calls during shutdown, especially if VS
				// has been sitting for a while. Do a sanity check before proceeding.
				return;
			}
			base.OnSelectionChanged(e);
			CommandManager.UpdateCommandStatus();
		}
#if VISUALSTUDIO_11_0
		/// <summary>
		/// Required AllDesigners override
		/// </summary>
		public override IEnumerable<VSDiagramView> AllDesigners
		{
			get
			{
				// I have no idea what this does, I can't get VS to call it.
				return new VSDiagramView[] { };
			}
		}
#endif // VISUALSTUDIO_11_0
		#endregion // Base overrides
		#region IModelingEventSubscriber Implementation
		/// <summary>
		/// Implements <see cref="IModelingEventSubscriber.ManageModelingEventHandlers"/>.
		/// </summary>
		protected new void ManageModelingEventHandlers(ModelingEventManager eventManager, EventSubscriberReasons reasons, EventHandlerAction action)
		{
			base.ManageModelingEventHandlers(eventManager, reasons, action);
			if ((EventSubscriberReasons.DocumentLoaded | EventSubscriberReasons.UserInterfaceEvents) == (reasons & (EventSubscriberReasons.DocumentLoaded | EventSubscriberReasons.UserInterfaceEvents)))
			{
				eventManager.AddOrRemoveHandler(new EventHandler<ElementEventsEndedEventArgs>(OnElementEventsEnded), action);
			}
		}
		void IModelingEventSubscriber.ManageModelingEventHandlers(ModelingEventManager eventManager, EventSubscriberReasons reasons, EventHandlerAction action)
		{
			ManageModelingEventHandlers(eventManager, reasons, action);
		}
		/// <summary>
		/// Update cached menu status when a transaction is completed
		/// </summary>
		private void OnElementEventsEnded(object sender, ElementEventsEndedEventArgs e)
		{
			if (((IORMToolServices)sender).ProcessingVisibleTransactionItemEvents)
			{
				CommandManager.UpdateCommandStatus();
			}
		}
		#endregion // IModelingEventSubscriber Implementation
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
		/// Refresh all visible diagrams
		/// </summary>
		/// <param name="serviceProvider">Required <see cref="IServiceProvider"/></param>
		public static void InvalidateAllDiagrams(IServiceProvider serviceProvider)
		{
			InvalidateAllDiagrams(serviceProvider, null);
		}
		/// <summary>
		/// Refresh all visible diagrams
		/// </summary>
		/// <param name="serviceProvider">Required <see cref="IServiceProvider"/></param>
		/// <param name="docData">A specific <see cref="ORMDesignerDocData"/></param>
		public static void InvalidateAllDiagrams(IServiceProvider serviceProvider, ORMDesignerDocData docData)
		{
			OptionsPage.NotifySettingsChange(
				serviceProvider,
				delegate(ORMDesignerDocData docData2)
				{
					if (docData == docData2)
					{
						foreach (ORMDesignerDocView docView in docData2.DocViews)
						{
							VSDiagramView view;
							if (null != (view = docView.CurrentDesigner) &&
								null != view.DiagramClientView)
							{
								view.Invalidate(true);
							}
						}
					}
				});
		}
		/// <summary>
		/// Get the selection service for this designer
		/// </summary>
		protected IMonitorSelectionService MonitorSelection
		{
			get
			{
				return myMonitorSelectionService ?? (myMonitorSelectionService = (IMonitorSelectionService)myCtorServiceProvider.GetService(typeof(IMonitorSelectionService)));
			}
		}
		#endregion // ORMDesignerDocView Specific
		#region IORMDesignerView Implementation
		/// <summary>
		/// Implements <see cref="IORMDesignerView.CommandManager"/>
		/// </summary>
		protected ORMDesignerCommandManager CommandManager
		{
			get
			{
				return myCommandManager ?? (myCommandManager = new ORMDesignerCommandManager(this));
			}
		}
		ORMDesignerCommandManager IORMDesignerView.CommandManager
		{
			get
			{
				return CommandManager;
			}
		}
		IServiceProvider IORMDesignerView.ServiceProvider
		{
			get
			{
				return myCtorServiceProvider;
			}
		}
		DiagramView IORMDesignerView.CurrentDesigner
		{
			get
			{
				return CurrentDesigner;
			}
		}
		IList IORMDesignerView.SelectedElements
		{
			get
			{
				return SelectedElements;
			}
		}
		#endregion // IORMDesignerView Implementation
		#region IVsToolboxUser Toolwindow Redirection
		int IVsToolboxUser.IsSupported(Microsoft.VisualStudio.OLE.Interop.IDataObject pDO)
		{
			// Redirect toolbox queries to supporting tool windows if the document is not the
			// active selection container.
			object selectionContainer = MonitorSelection.CurrentSelectionContainer;
			IVsToolboxUser redirectUser;
			IORMDesignerView designerView;
			if (selectionContainer != this &&
				null != (redirectUser = selectionContainer as IVsToolboxUser) &&
				null != (designerView = selectionContainer as IORMDesignerView) &&
				null != designerView.CurrentDiagram)
			{
				return redirectUser.IsSupported(pDO);
			}
			return IsSupported(pDO);
		}
		int IVsToolboxUser.ItemPicked(Microsoft.VisualStudio.OLE.Interop.IDataObject pDO)
		{
			// Redirect toolbox queries to supporting tool windows if the document is not the
			// active selection container.
			object selectionContainer = MonitorSelection.CurrentSelectionContainer;
			IVsToolboxUser redirectUser;
			IORMDesignerView designerView;
			if (selectionContainer != this &&
				null != (redirectUser = selectionContainer as IVsToolboxUser) &&
				null != (designerView = selectionContainer as IORMDesignerView) &&
				null != designerView.CurrentDiagram)
			{
				return redirectUser.ItemPicked(pDO);
			}
			return ItemPicked(pDO);
		}
		#endregion // IVsToolboxUser Toolwindow Redirection
	}
}
