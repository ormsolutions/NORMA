#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
* Copyright © Matthew Curland. All rights reserved.                        *
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
using Neumont.Tools.Modeling.Shell;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;
using Neumont.Tools.Modeling;

namespace Neumont.Tools.ORM.Shell
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
		/// Display the readings toolwindow
		/// </summary>
		DisplayReadingsWindow = 1L << 3,

		// IL << 4 is available

		/// <summary>
		/// Insert a role before or after the current role
		/// </summary>
		InsertRole = 1L << 5,
		/// <summary>
		/// Delete the current role
		/// </summary>
		DeleteRole = 1L << 6,

		// IL << 7 is available

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
		/// Activate editing for the ExternalConstraint
		/// </summary>
		EditExternalConstraint = 1L << 12,
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
	public partial class ORMDesignerDocView : MultiDiagramDocView, IORMSelectionContainer, IORMDesignerView, IModelingEventSubscriber
	{
		#region Member variables
		private IServiceProvider myCtorServiceProvider;
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
			//ContextMenuStrip contextMenu = sender as ContextMenuStrip;
			//bool haveSelectedTab = base.GetDesignerAtPoint(contextMenu.Location) != null;
			//foreach (ToolStripItem item in contextMenu.Items)
			//{
			//    if (item.Tag == ContextMenuItemNeedsSelectedTab)
			//    {
			//        item.Enabled = haveSelectedTab;
			//    }
			//}

			ContextMenuStrip contextMenu = sender as ContextMenuStrip;
			DiagramView designer = base.GetDesignerAtPoint(contextMenu.Location);
			Partition partition = this.DocData.Store.DefaultPartition;

			//Disable/Enable New Diagram Tabs stuff wow
			ToolStripDropDownItem newPageMenuItem = (ToolStripDropDownItem)contextMenu.Items[ResourceStrings.DiagramCommandNewPage];
			ToolStripItemCollection items = newPageMenuItem.DropDownItems;
			int itemCount = items.Count;

			for (int i = 0; i < itemCount; i++)
			{
				DomainClassInfo diagramInfo = (DomainClassInfo)items[i].Tag;
				ReadOnlyCollection<ModelElement> modelElements = partition.ElementDirectory.FindElements(diagramInfo);

				object[] attributes = diagramInfo.ImplementationClass.GetCustomAttributes(typeof(DiagramMenuDisplayAttribute), true);
				if (attributes.Length > 0)
				{
					DiagramMenuDisplayAttribute attribute = (DiagramMenuDisplayAttribute)attributes[0];
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
			}

			//If a diagram tab is selected
			if (designer != null)
			{
				Diagram diagram = designer.Diagram;
				//Retrieve all existing diagrams of the same type as the one selected
				ReadOnlyCollection<ModelElement> modelElements = partition.ElementDirectory.FindElements(diagram.GetDomainClass(), true);
				DomainClassInfo diagramInfo = diagram.GetDomainClass();

				//Grab the attribute of the diagram type selected
				object[] attributes = diagramInfo.ImplementationClass.GetCustomAttributes(typeof(DiagramMenuDisplayAttribute), true);
				if (attributes.Length > 0)
				{
					DiagramMenuDisplayAttribute attribute = (DiagramMenuDisplayAttribute)attributes[0];
					//If required but you only have 1 then disable delete
					if ((attribute.DiagramOption & DiagramMenuDisplayOptions.Required) != 0 && modelElements.Count <= 1)
					{
						//DISABLE DELETE
						ToolStripMenuItem deletePageMenuItem = (ToolStripMenuItem)contextMenu.Items[ResourceStrings.DiagramCommandDeletePage];
						deletePageMenuItem.Enabled = false;
					}
					else
					{
						//ALLOW DELETE
						ToolStripMenuItem deletePageMenuItem = (ToolStripMenuItem)contextMenu.Items[ResourceStrings.DiagramCommandDeletePage];
						deletePageMenuItem.Enabled = true;
					}

					if ((attribute.DiagramOption & DiagramMenuDisplayOptions.BlockRename) != 0)
					{
						//DISABLE RENAME
						ToolStripMenuItem renamePageMenuItem = (ToolStripMenuItem)contextMenu.Items[ResourceStrings.DiagramCommandRenamePage];
						renamePageMenuItem.Enabled = false;
					}
					else
					{
						//ALLOW RENAME
						ToolStripMenuItem renamePageMenuItem = (ToolStripMenuItem)contextMenu.Items[ResourceStrings.DiagramCommandRenamePage];
						renamePageMenuItem.Enabled = true;
					}
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
		private void ContextMenuNewPageClick(object sender, EventArgs e)
		{
			ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
			if (menuItem != null)
			{
				Store store = base.DocData.Store;
				using (Transaction t = store.TransactionManager.BeginTransaction(menuItem.Text.Replace("&", string.Empty)))
				{
					DomainClassInfo diagramInfo = menuItem.Tag as DomainClassInfo;
					if (diagramInfo != null)
					{
						store.ElementFactory.CreateElement(diagramInfo);
						t.Commit();
					}
				}
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
					object[] attributes = diagramInfo.ImplementationClass.GetCustomAttributes(typeof(DiagramMenuDisplayAttribute), true);
					if (attributes.Length > 0)
					{
						DiagramMenuDisplayAttribute attribute = (DiagramMenuDisplayAttribute)attributes[0];
						Image image = attribute.Image;
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
						newDiagramMenuItem.Tag = diagramInfo;
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

				contextMenu.Items.AddRange(new ToolStripItem[] { newPageMenuItem, new ToolStripSeparator(), deletePageMenuItem, renamePageMenuItem });
				#endregion // Setup context menu

				Store store = document.Store;
				// Add our existing diagrams, or make a new one if we don't already have one
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
			CommandManager.UpdateCommandStatus();
		}
		#endregion // Base overrides
		#region IModelingEventSubscriber Implementation
		/// <summary>
		/// Implements <see cref="IModelingEventSubscriber.ManageModelingEventHandlers"/>.
		/// </summary>
		protected void ManageModelingEventHandlers(ModelingEventManager eventManager, EventSubscriberReasons reasons, EventHandlerAction action)
		{
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
			CommandManager.UpdateCommandStatus();
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
	}
}
