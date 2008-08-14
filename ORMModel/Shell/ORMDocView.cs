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
using System.Drawing;
using System.Drawing.Imaging;
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
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.Modeling.Shell;
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
	public partial class ORMDesignerDocView : MultiDiagramDocView, IORMSelectionContainer
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
			ORMDesignerCommands.DisplayOrientation |
			ORMDesignerCommands.DisplayConstraintsPosition |
			ORMDesignerCommands.ExclusiveOrDecoupler |
			ORMDesignerCommands.SelectAll |
			ORMDesignerCommands.AlignShapes |
			ORMDesignerCommands.AutoLayout |
			ORMDesignerCommands.AddInternalUniqueness |
			ORMDesignerCommands.ToggleSimpleMandatory |
			ORMDesignerCommands.ReportGeneratorList |
			ORMDesignerCommands.DeleteAny |
			ORMDesignerCommands.DeleteAnyShape |
			ORMDesignerCommands.DeleteShape |
			(ORMDesignerCommands.Delete & ~ORMDesignerCommands.DeleteRole); // We don't allow deletion of the final role. Don't bother with sorting out the multiselect problems here
		/// <summary>
		/// The filter for multi selection when the elements are of different types. This should always be a subset of the simple command filter
		/// </summary>
		private const ORMDesignerCommands EnabledComplexMultiSelectCommandFilter =
			EnabledSimpleMultiSelectCommandFilter |
			ORMDesignerCommands.ExclusiveOrCoupler;
		/// <summary>
		/// A filter to turn off commands for a single selection
		/// </summary>
		private const ORMDesignerCommands DisabledSingleSelectCommandFilter =
			ORMDesignerCommands.AutoLayout |
			ORMDesignerCommands.AlignShapes |
			ORMDesignerCommands.ExclusiveOrCoupler;
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
				foreach (System.Reflection.Assembly assembly in assemblies)
				{
					if (assembly.FullName.StartsWith("Neumont.Tools.ORM.Views.RelationalView"))
					{
						relationalDiagramType = assembly.GetType("Neumont.Tools.ORM.Views.RelationalView.RelationalDiagram");
						break;
					}
				}
				Debug.Assert(relationalDiagramType != null);
				Diagram diagram = (Diagram)Activator.CreateInstance(relationalDiagramType, store);
				diagram.Associate((ModelElement)models[0]);
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
			// myVisibleCommands and myEnabledCommands will be set when the selection is changed
			if (0 != (myVisibleCommands & myEnabledCommands))
			{
				base.OnContextMenuRequested(mouseArgs);
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
					visibleCommands = enabledCommands = checkableCommands = checkedCommands = (EnabledSimpleMultiSelectCommandFilter | EnabledComplexMultiSelectCommandFilter);
					Type firstType = null;
					bool isComplex = false;
					NodeShape primaryShape = PrimarySelectedShape;
					foreach (ModelElement melIter in SelectedElements)
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
					foreach (ModelElement melIter in SelectedElements)
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
			ReadingOrder readingOrder = null;
			Role role;
			ObjectType objectType;
			NodeShape nodeShape;
			SetConstraint setConstraint;
			SetComparisonConstraint setComparisonConstraint = null;
			bool otherShape = false;
			if (null != (factType = element as FactType) ||
				(null != (readingOrder = element as ReadingOrder) && null != (factType = readingOrder.FactType)))
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DeleteFactType | ORMDesignerCommands.DeleteAny | ORMDesignerCommands.DisplayReadingsWindow;
				Objectification objectification = factType.Objectification;
				if (objectification == null || objectification.IsImplied)
				{
					visibleCommands |= ORMDesignerCommands.ObjectifyFactType;
					enabledCommands |= ORMDesignerCommands.ObjectifyFactType;
				}
				else
				{
					visibleCommands |= ORMDesignerCommands.UnobjectifyFactType;
					enabledCommands |= ORMDesignerCommands.UnobjectifyFactType;
				}
				if (presentationElement is FactTypeShape ||
					(readingOrder != null && presentationElement is ReadingShape))
				{
					visibleCommands |= ORMDesignerCommands.DeleteFactShape | ORMDesignerCommands.DeleteAnyShape | ORMDesignerCommands.DisplayOrientation | ORMDesignerCommands.DisplayConstraintsPosition;
					enabledCommands |= ORMDesignerCommands.DeleteFactShape | ORMDesignerCommands.DeleteAnyShape | ORMDesignerCommands.DisplayOrientation | ORMDesignerCommands.DisplayConstraintsPosition;
					if (readingOrder == null)
					{
						visibleCommands |= ORMDesignerCommands.AutoLayout | ORMDesignerCommands.AlignShapes;
						enabledCommands |= ORMDesignerCommands.AutoLayout | ORMDesignerCommands.AlignShapes;
					}
					// Don't flag the DisplayOrientation or DisplayConstraintsPosition commands as checkable, multiselect check state mismatches handled dynamically in OnStatusCommand
					if (factType.RoleCollection.Count > 1)
					{
						visibleCommands |= ORMDesignerCommands.DisplayReverseRoleOrder;
						enabledCommands |= ORMDesignerCommands.DisplayReverseRoleOrder;
					}
				}
				else if (null != presentationElement)
				{
					otherShape = true;
				}
			}
			else if (null != (objectType = element as ObjectType))
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DeleteObjectType | ORMDesignerCommands.DeleteAny;
				Objectification objectification = objectType.Objectification;
				if (objectification != null && !objectification.IsImplied)
				{
					visibleCommands |= ORMDesignerCommands.UnobjectifyFactType;
					enabledCommands |= ORMDesignerCommands.UnobjectifyFactType;
				}
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
			else if ((null != setConstraint) || (null != (setComparisonConstraint = element as SetComparisonConstraint)))
			{
				visibleCommands = enabledCommands = ORMDesignerCommands.DeleteConstraint | ORMDesignerCommands.DeleteAny | ORMDesignerCommands.EditExternalConstraint;
				ExclusionConstraint exclusionConstraint;
				if (setConstraint != null)
				{
					if (setConstraint.Constraint.ConstraintType == ConstraintType.DisjunctiveMandatory)
					{
						if (((MandatoryConstraint)setConstraint).ExclusiveOrExclusionConstraint != null)
						{
							visibleCommands |= ORMDesignerCommands.ExclusiveOrDecoupler;
							enabledCommands |= ORMDesignerCommands.ExclusiveOrDecoupler;
						}
						else
						{
							// We'll do deeper processing of this command in OnStatusCommand
							visibleCommands |= ORMDesignerCommands.ExclusiveOrCoupler;
							enabledCommands |= ORMDesignerCommands.ExclusiveOrCoupler;
						}
					}
				}
				else if (null != (exclusionConstraint = setComparisonConstraint as ExclusionConstraint))
				{
					if (exclusionConstraint.ExclusiveOrMandatoryConstraint == null)
					{
						// We'll do deeper processing of this command in OnStatusCommand
						visibleCommands |= ORMDesignerCommands.ExclusiveOrCoupler;
						enabledCommands |= ORMDesignerCommands.ExclusiveOrCoupler;
					}
				}
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
			else if (null != (role = element as Role))
			{
				FactType fact = role.FactType;
				if (fact == null)
				{
					// This is happening during teardown scenarios
					return;
				}
				Role unaryRole = fact.UnaryRole;

				visibleCommands = enabledCommands = ORMDesignerCommands.DisplayReadingsWindow | ORMDesignerCommands.InsertRole | ORMDesignerCommands.DeleteRole | ORMDesignerCommands.ToggleSimpleMandatory | ORMDesignerCommands.AddInternalUniqueness;
				checkableCommands = ORMDesignerCommands.ToggleSimpleMandatory;
				toleratedCommands |= ORMDesignerCommands.DeleteShape | ORMDesignerCommands.DeleteAnyShape | ORMDesignerCommands.AutoLayout;
				if (role.IsMandatory)
				{
					checkedCommands = ORMDesignerCommands.ToggleSimpleMandatory;
				}

				// Disable role deletion if the FactType is a unary
				visibleCommands |= ORMDesignerCommands.DeleteRole;
				if (unaryRole != null && (presentationElement == null || !(presentationElement is RoleNameShape)))
				{
					enabledCommands &= ~ORMDesignerCommands.DeleteRole;
				}

				Objectification objectification = fact.Objectification;
				if (objectification == null || objectification.IsImplied)
				{
					visibleCommands |= ORMDesignerCommands.ObjectifyFactType;
					enabledCommands |= ORMDesignerCommands.ObjectifyFactType;
				}
				else
				{
					visibleCommands |= ORMDesignerCommands.UnobjectifyFactType;
					enabledCommands |= ORMDesignerCommands.UnobjectifyFactType;
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
						switch (constraint.ConstraintStorageStyle)
						{
							case ConstraintStorageStyle.SetConstraint:
								LinkedElementCollection<Role> constraintRoles = ((SetConstraint)constraint).RoleCollection;
								if (constraintRoles.Contains(role) ||
									(constraint.ConstraintType == ConstraintType.ExternalUniqueness &&
									role.Role == unaryRole &&
									constraintRoles.Contains(role.OppositeRole as Role)))
								{
									visibleCommands |= ORMDesignerCommands.ActivateRoleSequence;
									enabledCommands |= ORMDesignerCommands.ActivateRoleSequence;
								}
								break;
							case ConstraintStorageStyle.SetComparisonConstraint:
								LinkedElementCollection<SetComparisonConstraintRoleSequence> roleSequences = (constraint as SetComparisonConstraint).RoleSequenceCollection;
								int sequenceCount = roleSequences.Count;
								for (int i = 0; i < sequenceCount; ++i)
								{
									if (roleSequences[i].RoleCollection.Contains(role))
									{
										visibleCommands |= ORMDesignerCommands.RoleSequenceActions | ORMDesignerCommands.ActivateRoleSequence;
										enabledCommands |= ORMDesignerCommands.RoleSequenceActions | ORMDesignerCommands.ActivateRoleSequence;
										if (i == 0)
										{
											enabledCommands &= ~ORMDesignerCommands.MoveRoleSequenceUp;
										}
										else if (i == (sequenceCount - 1))
										{
											enabledCommands &= ~ORMDesignerCommands.MoveRoleSequenceDown;
										}
										break;
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
			ORMBaseShape shape;
			if (null != (shape = (presentationElement as ORMBaseShape)) &&
				shape.DisplaysMultiplePresentations &&
				ORMBaseShape.ElementHasMultiplePresentations(shape))
			{
				visibleCommands |= ORMDesignerCommands.DiagramList;
				enabledCommands |= ORMDesignerCommands.DiagramList;
			}
			// Turn on standard commands for all selections
			visibleCommands |= ORMDesignerCommands.DisplayStandardWindows | ORMDesignerCommands.CopyImage | ORMDesignerCommands.SelectAll | ORMDesignerCommands.ExtensionManager | ORMDesignerCommands.ErrorList | ORMDesignerCommands.ReportGeneratorList;
			enabledCommands |= ORMDesignerCommands.DisplayStandardWindows | ORMDesignerCommands.CopyImage | ORMDesignerCommands.SelectAll | ORMDesignerCommands.ExtensionManager | ORMDesignerCommands.ErrorList | ORMDesignerCommands.ReportGeneratorList;
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
				IMonitorSelectionService monitorService = docView.MonitorSelection;
				if (monitorService != null && monitorService.CurrentSelectionContainer != docView)
				{
					ORMDesignerCommands activeFilter = ORMDesignerCommands.DisplayStandardWindows;
					commandFlag &= activeFilter;
				}
				bool isVisible;
				bool isEnabled;
				ORMDesignerCommands allEnabledCommands;
				command.Visible = isVisible = (0 != (commandFlag & docView.myVisibleCommands));
				command.Enabled = isEnabled = (0 != (commandFlag & (allEnabledCommands = docView.myEnabledCommands)));
				command.Checked = 0 != (commandFlag & docView.myCheckedCommands);
				if (!isVisible && !isEnabled)
				{
					return;
				}
				if (0 != (commandFlag & (ORMDesignerCommands.Delete | ORMDesignerCommands.DeleteAny)))
				{
					docView.SetDeleteElementCommandText((OleMenuCommand)command);
				}
				else if (0 != (commandFlag & (ORMDesignerCommands.DeleteShape | ORMDesignerCommands.DeleteAnyShape)))
				{
					docView.SetDeleteShapeCommandText((OleMenuCommand)command);
				}
				else if (commandFlag == ORMDesignerCommands.ToggleSimpleMandatory)
				{
					if (isEnabled)
					{
						foreach (ModelElement mel in docView.SelectedElements)
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
				}
				else if (0 != (commandFlag & ORMDesignerCommands.ExclusiveOrCoupler))
				{
					if (isEnabled)
					{
						// Given the strict and unusual requirements on this command,
						// most of the processing for this command is deferred to this point.
						// If it is visible but not enabled, then we have done this processing
						// once already.
						bool disable = false;
						bool hide = false;
						if (0 != (allEnabledCommands & ORMDesignerCommands.ExclusiveOrDecoupler) ||
							2 != docView.SelectedElements.Count)
						{
							disable = hide = true;
						}
						else
						{
							MandatoryConstraint mandatory = null;
							ExclusionConstraint exclusion = null;
							foreach (ModelElement mel in docView.SelectedElements)
							{
								PresentationElement pel = mel as PresentationElement;
								IConstraint testConstraint = (pel != null) ? pel.Subject as IConstraint : mel as IConstraint;
								if (testConstraint == null)
								{
									break;
								}
								switch (testConstraint.ConstraintType)
								{
									case ConstraintType.DisjunctiveMandatory:
										if (mandatory == null)
										{
											mandatory = (MandatoryConstraint)testConstraint;
										}
										break;
									case ConstraintType.Exclusion:
										if (exclusion == null)
										{
											exclusion = (ExclusionConstraint)testConstraint;
										}
										break;
								}
							}
							if (null == mandatory && null == exclusion)
							{
								disable = hide = true;
							}
							else
							{
								if (!ExclusiveOrConstraintCoupler.CanCoupleConstraints(mandatory, exclusion))
								{
									disable = true;
								}
							}
						}
						if (disable)
						{
							docView.myEnabledCommands &= ~ORMDesignerCommands.ExclusiveOrCoupler;
							command.Enabled = false;
						}
						if (hide)
						{
							docView.myVisibleCommands &= ~ORMDesignerCommands.ExclusiveOrCoupler;
							command.Visible = false;
						}
					}
				}
				else if (0 != (commandFlag & ORMDesignerCommands.DisplayOrientation))
				{
					if (isEnabled)
					{
						// This can change between status checks, check the selected items
						DisplayOrientation expectedOrientation = DisplayOrientation.Horizontal;
						switch (commandFlag & ORMDesignerCommands.DisplayOrientation)
						{
							case ORMDesignerCommands.DisplayOrientationRotatedLeft:
								expectedOrientation = DisplayOrientation.VerticalRotatedLeft;
								break;
							case ORMDesignerCommands.DisplayOrientationRotatedRight:
								expectedOrientation = DisplayOrientation.VerticalRotatedRight;
								break;
						}
						bool isChecked = true;
						foreach (ModelElement mel in docView.SelectedElements)
						{
							FactTypeShape factTypeShape;
							ReadingShape readingShape;
							if (null != (factTypeShape = mel as FactTypeShape) ||
								(null != (readingShape = mel as ReadingShape) &&
								null != (factTypeShape = readingShape.ParentShape as FactTypeShape)))
							{
								// The command is checked when all selected values match the expected orientation
								if (factTypeShape.DisplayOrientation != expectedOrientation)
								{
									isChecked = false;
									break;
								}
							}
						}
						command.Checked = isChecked;
					}
				}
				else if (0 != (commandFlag & ORMDesignerCommands.DisplayConstraintsPosition))
				{
					if (isEnabled)
					{
						// This can change between status checks, check the selected items
						ConstraintDisplayPosition expectedPosition = ConstraintDisplayPosition.Top;
						switch (commandFlag & ORMDesignerCommands.DisplayConstraintsPosition)
						{
							case ORMDesignerCommands.DisplayConstraintsOnBottom:
								expectedPosition = ConstraintDisplayPosition.Bottom;
								break;
						}
						bool isChecked = true;
						foreach (ModelElement mel in docView.SelectedElements)
						{
							FactTypeShape factTypeShape;
							ReadingShape readingShape;
							if (null != (factTypeShape = mel as FactTypeShape) ||
								(null != (readingShape = mel as ReadingShape) &&
								null != (factTypeShape = readingShape.ParentShape as FactTypeShape)))
							{
								// The command is checked when all selected values match the expected orientation
								if (factTypeShape.ConstraintDisplayPosition != expectedPosition)
								{
									isChecked = false;
									break;
								}
							}
						}
						command.Checked = isChecked;
					}
				}
				else if (0 != (commandFlag & (ORMDesignerCommands.MoveRoleLeft | ORMDesignerCommands.MoveRoleRight)))
				{
					foreach (ModelElement mel in docView.SelectedElements)
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
				else if (commandFlag == ORMDesignerCommands.ErrorList)
				{
					if (isEnabled)
					{
						OleMenuCommand cmd = sender as OleMenuCommand;
						string errorText = null;
						int errorIndex = cmd.MatchedCommandId;
						foreach (ModelElement mel in docView.SelectedElements)
						{
							IModelErrorOwner errorOwner = EditorUtility.ResolveContextInstance(mel, false) as IModelErrorOwner;
							if (errorOwner != null)
							{
								ModelErrorDisplayFilter displayFilter = null;
								ORMDiagram diagram;
								ORMModel model;
								if (null != (diagram = docView.CurrentDiagram as ORMDiagram) &&
									null != (model = diagram.ModelElement as ORMModel))
								{
									displayFilter = model.ModelErrorDisplayFilter;
								}
								foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.DisplayPrimary))
								{
									if (displayFilter == null || displayFilter.ShouldDisplay(error))
									{
										if (errorIndex == 0)
										{
											errorText = error.ErrorText;
											break;
										}
										--errorIndex;
									}
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
				}
				else if (commandFlag == ORMDesignerCommands.DiagramList)
				{
					if (isEnabled)
					{
						OleMenuCommand cmd = sender as OleMenuCommand;
						int diagramIndex = cmd.MatchedCommandId;
						bool thisDiagram = (diagramIndex == 0);

						ShapeElement sel = docView.GetShapeForDiagramList(diagramIndex);
						if (sel != null)
						{
							cmd.Enabled = true;
							cmd.Visible = true;
							cmd.Supported = true;
							if (thisDiagram)
							{
								cmd.Text = ResourceStrings.CommandNextOnThisDiagramText;
							}
							else
							{
								cmd.Text = sel.Diagram.Name;
							}
						}
						else if (!thisDiagram)
						{
							cmd.Supported = false;
						}
					}
				}
				else if (commandFlag == ORMDesignerCommands.ReportGeneratorList)
				{
					if (isEnabled)
					{
						OleMenuCommand cmd = sender as OleMenuCommand;
						string reportGeneratorText = null;
						int reportGeneratorIndex = cmd.MatchedCommandId;
						foreach (VerbalizationTargetData targetData in ((IORMToolServices)docView.Store).VerbalizationTargets.Values)
						{
							if (targetData.CanReport)
							{
								if (reportGeneratorIndex == 0)
								{
									reportGeneratorText = targetData.ReportCommandName;
									break;
								}
								--reportGeneratorIndex;
							}
						}
						if (reportGeneratorText != null)
						{
							cmd.Enabled = true;
							cmd.Visible = true;
							cmd.Supported = true;
							cmd.Text = reportGeneratorText;
						}
						else
						{
							cmd.Supported = false;
						}
					}
				}

				else if (commandFlag == ORMDesignerCommands.AddInternalUniqueness)
				{
					if (isEnabled)
					{
						// Determine if a unique internal uniqueness constraint can
						// be added at this point.

						// Delay processing for this one until this point. There
						// is no need to run it whenever the selection changes to include
						// a role, given than it is only used when the context menu is opened.
						bool disable = false;
						bool hide = false;
						int selCount = docView.SelectionCount;
						if (selCount > 0)
						{
							bool encounteredNonRole = false;
							Role[] roles = new Role[selCount];
							FactType fact = null;
							int roleCount = 0;
							foreach (ModelElement mel in docView.SelectedElements)
							{
								Role role = mel as Role;
								if (role == null)
								{
									encounteredNonRole = true;
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

								if (Array.IndexOf<Role>(roles, role) < 0)
								{
									roles[roleCount++] = role;
								}
							}
							if (fact != null && !encounteredNonRole)
							{
								foreach (UniquenessConstraint iuc in fact.GetInternalConstraints<UniquenessConstraint>())
								{
									LinkedElementCollection<Role> factRoles = iuc.RoleCollection;
									if (factRoles.Count == roleCount)
									{
										int i = 0;
										for (; i < roleCount; ++i)
										{
											if (!factRoles.Contains(roles[i]))
											{
												break;
											}
										}
										if (i == roleCount)
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
		/// Returns the <see cref="IMonitorSelectionService"/> instance for this <see cref="ORMDesignerDocView"/>.
		/// </summary>
		protected IMonitorSelectionService MonitorSelection
		{
			get
			{
				return myMonitorSelection ?? (myMonitorSelection = myCtorServiceProvider.GetService(typeof(IMonitorSelectionService)) as IMonitorSelectionService);
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
				Store store = Store;
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
				using (Transaction t = store.TransactionManager.BeginTransaction(commandText.Replace("&", string.Empty)))
				{
					Dictionary<object, object> contextInfo = t.TopLevelTransaction.Context.ContextInfo;

					IList selectedElements = SelectedElements;
					// account for multiple selection
					for (int i = selectedElements.Count - 1; i >= 0; i--)
					{
						object selectedObject = selectedElements[i];
						ShapeElement pel; // just the shape
						ModelElement mel;
						bool deleteReferenceModeValueTypeInContext = false;
						if (null != (pel = selectedObject as ShapeElement))
						{
							// Note that if the ORMDiagram.SelectionRules property is overridden
							// or any shape overrides the AllowChildrenInSelection property, then
							// the delete propagation on child shapes can force the pel to be deleted
							// without deleting the underlying mel. This would require resolving all
							// model elements before any pels are deleted because the pel could
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
							if (mel != null && !mel.IsDeleted)
							{
								Role role;
								if (null != (role = mel as Role) && pel is RoleNameShape)
								{
									role.Name = "";
								}
								else
								{
									ReadingOrder readingOrder = mel as ReadingOrder;
									if (readingOrder != null)
									{
										mel = readingOrder.FactType;
									}
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
						}
						else if (null != (mel = selectedObject as ModelElement) && !mel.IsDeleted)
						{

							// Remove the item
							mel.Delete();
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
		/// Execute the delete shape command
		/// </summary>
		/// <param name="commandText">The text from the command</param>
		protected virtual void OnMenuDeleteShape(string commandText)
		{
			int pelCount = SelectionCount;
			if (pelCount > 0)
			{
				Store store = Store;
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
				using (Transaction t = store.TransactionManager.BeginTransaction(commandText.Replace("&", string.Empty)))
				{
					FinalShapeDeleteBehavior finalDeleteBehavior = OptionsPage.CurrentFinalShapeDeleteBehavior;
					bool testMelDeletion = finalDeleteBehavior != FinalShapeDeleteBehavior.DeleteShapeOnly;
					IList selectedElements = SelectedElements;
					for (int i = selectedElements.Count - 1; i >= 0; i--)
					{
						ModelElement mel = selectedElements[i] as ModelElement;
						PresentationElement pel = mel as ShapeElement;
						ObjectType backingObjectifiedType = null;
						if (pel != null && !pel.IsDeleted)
						{
							ObjectifiedFactTypeNameShape objectifiedObjectShape;
							ReadingShape readingShape;
							if (pel is ValueConstraintShape)
							{
								// ValueConstraintShape tolerates deletion, but the
								// shapes cannot be deleted individually
								continue;
							}
							else if (null != (readingShape = pel as ReadingShape))
							{
								pel = readingShape.ParentShape;
								if (pel == null)
								{
									continue;
								}
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
							if (backingMel != null && !backingMel.IsDeleted)
							{
								LinkedElementCollection<PresentationElement> remainingPels = PresentationViewsSubject.GetPresentation(backingMel);
								int newPelCount = remainingPels.Count;
								Partition partition = store.DefaultPartition;
								for (int j = newPelCount - 1; j >= 0; --j)
								{
									if (remainingPels[j].Partition != partition)
									{
										--newPelCount;
									}
								}

								if (newPelCount == 0)
								{
									if (finalDeleteBehavior == FinalShapeDeleteBehavior.Prompt &&
										(int)DialogResult.No == VsShellUtilities.ShowMessageBox(ServiceProvider,
											string.Format(CultureInfo.CurrentCulture, ResourceStrings.FinalShapeDeletionMessage, TypeDescriptor.GetClassName(backingMel), TypeDescriptor.GetComponentName(backingMel)),
											string.Empty, OLEMSGICON.OLEMSGICON_QUERY, OLEMSGBUTTON.OLEMSGBUTTON_YESNO, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_SECOND))
									{
										continue;
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
		/// Run the selected report generator
		/// </summary>
		protected virtual void OnMenuGenerateReport(int reportGeneratorIndex)
		{
			foreach (VerbalizationTargetData targetData in ((IORMToolServices)Store).VerbalizationTargets.Values)
			{
				if (targetData.CanReport)
				{
					if (reportGeneratorIndex == 0)
					{
						targetData.ReportCallback(Store);
						break;
					}
					--reportGeneratorIndex;
				}
			}
		}
		/// <summary>
		/// Execute the AutoLayout menu command
		/// </summary>
		protected virtual void OnMenuAutoLayout()
		{
			ORMDesignerDocView.AutoLayoutDiagram(this.CurrentDiagram, this.SelectedElements, false);
		}
		/// <summary>
		/// Automatically lays out the <see cref="ShapeElement"/>s contained in <paramref name="shapeElementCollection"/> on
		/// the <see cref="Diagram"/> specified by <paramref name="diagram"/>.
		/// </summary>
		/// <remarks>
		/// <see cref="Diagram.AutoLayoutShapeElements(ICollection,VGRoutingStyle,PlacementValueStyle,Boolean)"/> is used to perform the
		/// actual layout. The value returned by <see cref="Diagram.RoutingStyle"/> is passed for the <c>routingStyle</c> parameter.
		/// <see langword="true"/> is passed for the <c>route</c> parameter if and only if <see cref="Diagram.RoutingStyle"/> is any
		/// value other than <see cref="VGRoutingStyle.VGRouteNone"/>.
		/// </remarks>
		public static void AutoLayoutDiagram(Diagram diagram, ICollection shapeElementCollection, bool ignoreExistingShapes)
		{
			ORMDesignerDocView.AutoLayoutDiagram(diagram, shapeElementCollection, ignoreExistingShapes, PlacementValueStyle.VGPlaceWideSSW);
		}
		/// <summary>
		/// Automatically lays out the <see cref="ShapeElement"/>s contained in <paramref name="shapeElementCollection"/> on
		/// the <see cref="Diagram"/> specified by <paramref name="diagram"/>.
		/// </summary>
		/// <remarks>
		/// <see cref="Diagram.AutoLayoutShapeElements(ICollection,VGRoutingStyle,PlacementValueStyle,Boolean)"/> is used to perform the
		/// actual layout. The value returned by <see cref="Diagram.RoutingStyle"/> is passed for the <c>routingStyle</c> parameter.
		/// <see langword="true"/> is passed for the <c>route</c> parameter if and only if <see cref="Diagram.RoutingStyle"/> is any
		/// value other than <see cref="VGRoutingStyle.VGRouteNone"/>.
		/// </remarks>
		public static void AutoLayoutDiagram(Diagram diagram, ICollection shapeElementCollection, bool ignoreExistingShapes, PlacementValueStyle placementStyle)
		{
			if (diagram != null)
			{
				using (Transaction t = diagram.Store.TransactionManager.BeginTransaction(ResourceStrings.AutoLayoutTransactionName))
				{
					LayoutManager bl = new LayoutManager(diagram as ORMDiagram, (diagram.Store as IORMToolServices).GetLayoutEngine(typeof(ORMRadialLayoutEngine)));
					foreach (ShapeElement shape in shapeElementCollection)
					{
						bl.AddShape(shape, false);
					}
					bl.Layout(ignoreExistingShapes);

					if (t.HasPendingChanges)
					{
						t.Commit();
					}
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
			IList components;
			int selectionCount;
			NodeShape matchShape = PrimarySelectedShape;
			Diagram diagram;
			if (null != matchShape &&
				null != (diagram = matchShape.ParentShape as Diagram) &&
				null != (components = SelectedElements) &&
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
				double xAdjust = 0d;
				double yAdjust = 0d;
				bool boundsAdjustment = false;
				BoundsRules boundsRules = diagram.BoundsRules;
				if (boundsRules != null)
				{
					// Do this as a two pass algorithm. The first time through the shapes
					// determines if moving the shape will pull it outside of the accepted
					// diagram bounds. If this is the case, we need to adjust the location
					// of all of the shapes (including the 'matchShape') by sufficient distance
					// to get all of the shapes in bounds.
					for (int i = components.Count - 1; i >= 0; i--)
					{
						NodeShape shape = components[i] as NodeShape;
						if (shape != null &&
							shape != matchShape &&
							shape.ParentShape == diagram)
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
							bounds.Location = newLocation;
							PointD adjustedLocation = boundsRules.GetCompliantBounds(shape, bounds).Location;
							if (adjustedLocation != newLocation)
							{
								boundsAdjustment = true;
								double testAdjust = adjustedLocation.X - newLocation.X;
								if (Math.Abs(xAdjust) < Math.Abs(testAdjust))
								{
									xAdjust = testAdjust;
								}
								testAdjust = adjustedLocation.Y - newLocation.Y;
								if (Math.Abs(yAdjust) < Math.Abs(testAdjust))
								{
									yAdjust = testAdjust;
								}
							}
						}
					}
				}
				using (Transaction t = matchShape.Store.TransactionManager.BeginTransaction(ResourceStrings.AlignShapesTransactionName))
				{
					for (int i = components.Count - 1; i >= 0; i--)
					{
						NodeShape shape = components[i] as NodeShape;
						if (shape != null &&
							shape != matchShape &&
							shape.ParentShape == diagram)
						{
							RectangleD bounds = shape.AbsoluteBounds;
							PointD newLocation = bounds.Location;
							switch (commandId)
							{
								case 1: // AlignBottom
									newLocation = new PointD(bounds.Left, alignTo + yAdjust - bounds.Height);
									break;
								case 2: // AlignHorizontalCenters
									newLocation = new PointD(bounds.Left, alignTo + yAdjust - ((null == (factShape = shape as FactTypeShape)) ? (bounds.Height / 2) : factShape.RolesCenter.Y));
									break;
								case 3: // AlignLeft
									newLocation = new PointD(alignTo + xAdjust, bounds.Top);
									break;
								case 4: // AlignRight
									newLocation = new PointD(alignTo + xAdjust - bounds.Width, bounds.Top);
									break;
								case 6: // AlignTop
									newLocation = new PointD(bounds.Left, alignTo + yAdjust);
									break;
								case 7: // AlignVerticalCenters
									newLocation = new PointD(alignTo + xAdjust - ((null == (factShape = shape as FactTypeShape)) ? (bounds.Width / 2) : factShape.RolesCenter.X), bounds.Top);
									break;
							}
							shape.Location = newLocation;
						}
					}
					if (boundsAdjustment)
					{
						PointD newLocation = matchShape.Location;
						newLocation.Offset(xAdjust, yAdjust);
						matchShape.Location = newLocation;
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
			IList components = SelectedElements;
			if (components.Count == 1)
			{
				RoleBase role = components[0] as RoleBase;
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
							contextInfo[FactType.InsertAfterRoleKey] = role;
						}
						else
						{
							contextInfo[FactType.InsertBeforeRoleKey] = role;
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
			Store store = Store;
			if (store == null)
			{
				return;
			}

			DomainPropertyInfo isMandatoryPropertyInfo = store.DomainDataDirectory.FindDomainProperty(Role.IsMandatoryDomainPropertyId);
			Debug.Assert(isMandatoryPropertyInfo != null);

			using (Transaction t = store.TransactionManager.BeginTransaction(
				Microsoft.VisualStudio.Modeling.Design.ElementPropertyDescriptor.GetSetValueTransactionName(isMandatoryPropertyInfo.DisplayName)))
			{
				bool hasNewIsMandatoryValue = false;
				bool newIsMandatoryValue = false;
				IList selectedElements = SelectedElements;
				for (int i = selectedElements.Count - 1; i >= 0; i--)
				{
					Role role = selectedElements[i] as Role;
					if (role != null)
					{
						if (!hasNewIsMandatoryValue)
						{
							newIsMandatoryValue = !role.IsMandatory;
							hasNewIsMandatoryValue = true;
						}
						role.IsMandatory = newIsMandatoryValue;
					}
				}
				if (t.HasPendingChanges)
				{
					t.Commit();
				}
			}
		}
		/// <summary>
		/// Execute the Add Internal Uniqueness menu command
		/// </summary>
		protected virtual void OnMenuAddInternalUniqueness()
		{
			Store store = Store;
			if (store != null)
			{
				using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.AddInternalConstraintTransactionName))
				{
					FactType parentFact = null;
					LinkedElementCollection<Role> constraintRoles = null;
					bool abort = false;
					IList selectedElements = SelectedElements;
					int selectedElementCount = selectedElements.Count;
					for (int i = 0; i < selectedElementCount; ++i)
					{
						Role role = selectedElements[i] as Role;
						if (role != null)
						{
							FactType testFact = role.FactType;
							if (parentFact == null)
							{
								parentFact = testFact;
								UniquenessConstraint iuc = UniquenessConstraint.CreateInternalUniquenessConstraint(parentFact);
								constraintRoles = iuc.RoleCollection;
								constraintRoles.Add(role);
							}
							else if (testFact != parentFact)
							{
								abort = true; // Transaction will rollback when it disposes if we don't commit
								break;
							}
							else if (!constraintRoles.Contains(role))
							{
								constraintRoles.Add(role);
							}
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
			IList selectedElements = SelectedElements;
			for (int i = selectedElements.Count - 1; i >= 0; i++)
			{
				IModelErrorOwner errorOwner = EditorUtility.ResolveContextInstance(selectedElements[i], false) as IModelErrorOwner;
				if (errorOwner != null)
				{
					ModelErrorDisplayFilter displayFilter = null;
					ORMDiagram diagram;
					ORMModel model;
					if (null != (diagram = this.CurrentDiagram as ORMDiagram) &&
						null != (model = diagram.ModelElement as ORMModel))
					{
						displayFilter = model.ModelErrorDisplayFilter;
					}
					foreach (ModelError error in errorOwner.GetErrorCollection(ModelErrorUses.DisplayPrimary))
					{
						if (displayFilter == null || displayFilter.ShouldDisplay(error))
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
					}
					break;
				}
			}
		}
		/// <summary>
		/// Expandable submenu to display associated diagrams
		/// </summary>
		/// <param name="diagramIndex">Index of the diagram in the list of diagrams</param>
		protected virtual void OnMenuDiagramList(int diagramIndex)
		{
			ShapeElement activate = GetShapeForDiagramList(diagramIndex);
			if (activate != null)
			{
				(DocData as IORMToolServices).ActivateShape(activate);
			}
		}

		/// <summary>
		/// Gets the shape associated with the specified diagram index in the diagram list
		/// </summary>
		/// <param name="diagramIndex">The diagram index for which to get the associated shape</param>
		/// <returns>The shape, if any</returns>
		protected ShapeElement GetShapeForDiagramList(int diagramIndex)
		{
			ShapeElement retVal = null;

			IList selectedElements = SelectedElements;
			if (selectedElements.Count >= 1) // Single-select command
			{
				ORMBaseShape shape = selectedElements[0] as ORMBaseShape;
				if (shape != null)
				{
					Diagram shapeDiagram = shape.Diagram;
					// for the first diagram, find the next shape on the same diagram
					// The first command is recognized, it is just hidden if there are no
					// other shapes on the first diagram
					if (diagramIndex == 0)
					{
						ShapeElement firstShape = null;
						bool seenCurrent = false;
						ORMBaseShape.VisitAssociatedShapes(null, shape, false,
							delegate(ShapeElement testShape)
							{
								if (testShape == shape)
								{
									seenCurrent = true;
								}
								else if (testShape.Diagram == shapeDiagram)
								{
									if (seenCurrent)
									{
										retVal = testShape;
										return false;
									}
									else if (firstShape == null)
									{
										firstShape = testShape;
									}
								}
								return true;
							}
						);
						if (retVal == null && seenCurrent)
						{
							retVal = firstShape;
						}
					}
					else
					{
						--diagramIndex;

						ORMBaseShape.VisitAssociatedShapes(null, shape, true,
							delegate(ShapeElement testShape)
							{
								if (testShape.Diagram != shapeDiagram)
								{
									if (diagramIndex == 0)
									{
										retVal = testShape;
										return false;
									}
									--diagramIndex;
								}
								return true;
							}
						);
					}
				}
			}

			return retVal;
		}
		#region OnMenuCopyImage method and support
		#region UnsafeNativeMethods
		[System.Security.SuppressUnmanagedCodeSecurity]
		private static partial class UnsafeNativeMethods
		{
			[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			private static extern bool OpenClipboard([In] HandleRef hWndNewOwner);

			[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			private static extern bool EmptyClipboard();

			[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			private static extern bool CloseClipboard();

			[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
			private static extern IntPtr SetClipboardData([In] uint uFormat, [In] IntPtr hMem);

			[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			private static extern IntPtr CopyEnhMetaFile([In] IntPtr hemfSrc, [In] [MarshalAs(UnmanagedType.LPTStr)] string lpszFile);

			[DllImport("gdi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			private static extern bool DeleteEnhMetaFile([In] IntPtr hemfSrc);

			[System.Security.Permissions.UIPermission(System.Security.Permissions.SecurityAction.Demand, Clipboard = System.Security.Permissions.UIPermissionClipboard.OwnClipboard)]
			public static void CopyImagesToClipboard(HandleRef hWndNewOwner, Bitmap bitmap, Metafile metafile)
			{
				const uint CF_BITMAP = 2;
				const uint CF_ENHMETAFILE = 14;

				// These are the retry values that System.Windows.Forms.Clipboard uses by default,
				// so they should also work well for us.
				const int RetryTimes = 10;
				const int RetryDelay = 100;

				bool clipboardOpen = false;
				IntPtr hEnhmetafile = IntPtr.Zero;
				try
				{
					int retriesLeft = RetryTimes;
					// If something else already has the clipboard open, we won't be able to open it.
					// Hence the loop here, since we may need to try multiple times.
					while (true)
					{
						// Open and empty the clipboard
						if (clipboardOpen = OpenClipboard(hWndNewOwner) && EmptyClipboard())
						{
							// Copy the metafile to the clipboard
							hEnhmetafile = metafile.GetHenhmetafile();
							IntPtr hEnhmetafileCopy = CopyEnhMetaFile(hEnhmetafile, null);
							if (hEnhmetafileCopy == IntPtr.Zero || SetClipboardData(CF_ENHMETAFILE, hEnhmetafileCopy) == IntPtr.Zero)
							{
								throw new Win32Exception();
							}

							// Copy the bitmap to the clipboard
							// Yes, we're passing null for the 'this' parameter, but the method doesn't use it anyway
							IntPtr hCompatibleBitmap = GetCompatibleBitmap(null, bitmap);
							if (hCompatibleBitmap == IntPtr.Zero || SetClipboardData(CF_BITMAP, hCompatibleBitmap) == IntPtr.Zero)
							{
								throw new Win32Exception();
							}
							break;
						}
						else
						{
							// We couldn't open the clipboard this time.
							// If we haven't tried more than RetryTimes already, wait RetryDelay and then try again
							if (retriesLeft-- > 0)
							{
								System.Threading.Thread.Sleep(RetryDelay);
							}
							else
							{
								throw new Win32Exception();
							}
						}
					}
				}
				finally
				{
					// Close the clipboard and delete the original metafile
					if ((clipboardOpen && !CloseClipboard()) || ((hEnhmetafile != IntPtr.Zero) && !DeleteEnhMetaFile(hEnhmetafile)))
					{
						throw new Win32Exception();
					}
				}
			}
		}
		#endregion // UnsafeNativeMethods

		/// <summary>
		/// Copies the selected elements as an image.
		/// </summary>
		protected virtual void OnMenuCopyImage()
		{
			Diagram diagram = this.CurrentDiagram;
			if (diagram == null)
			{
				return;
			}
			ICollection selectedElements = GetSelectedShapesForImage(diagram);
			if (selectedElements == null)
			{
				return;
			}
			CopyImageToClipboard(diagram, selectedElements);
		}

		private static void CopyImageToClipboard(Diagram diagram, ICollection selectedElements)
		{
			GetShapesToDrawDelegate getShapesToDraw = GetShapesToDraw;
			if (getShapesToDraw != null && GetCompatibleBitmap != null)
			{
				RectangleD boundingBoxForShapes;
				ArrayList shapesToDraw = getShapesToDraw(diagram, selectedElements, out boundingBoxForShapes);
				if (shapesToDraw.Count <= 0)
				{
					return;
				}
				boundingBoxForShapes.Inflate(0.1, 0.1);
				PointD sourceLocation = boundingBoxForShapes.Location;
				DiagramView diagramView = diagram.ActiveDiagramView;
				Rectangle clipRectangle;
				Metafile metafile = null;
				try
				{
					// Calculate the size taking into acount the current DPI, and create the Metafile
					using (Graphics graphics = Graphics.FromHwndInternal(diagramView.Handle))
					{
						float dpiX = graphics.DpiX;
						float dpiY = graphics.DpiY;
						clipRectangle = new Rectangle(
							(int)(boundingBoxForShapes.X * dpiX),
							(int)(boundingBoxForShapes.Y * dpiY),
							(int)Math.Ceiling(boundingBoxForShapes.Width * dpiX),
							(int)Math.Ceiling(boundingBoxForShapes.Height * dpiY));
						IntPtr referenceHdc = IntPtr.Zero;
						try
						{
							referenceHdc = graphics.GetHdc();
							metafile = new Metafile(referenceHdc, EmfType.EmfPlusDual);
						}
						finally
						{
							if (referenceHdc != IntPtr.Zero)
							{
								graphics.ReleaseHdc(referenceHdc);
							}
						}
					}

					// Draw the Metafile
					DrawDiagramShapesForImage(metafile, shapesToDraw, sourceLocation, clipRectangle);

					// Create the Bitmap
					using (Bitmap bitmap = new Bitmap(clipRectangle.Width, clipRectangle.Height, PixelFormat.Format32bppArgb))
					{
						// Draw the Bitmap
						DrawDiagramShapesForImage(bitmap, shapesToDraw, sourceLocation, clipRectangle);

						// Copy the Bitmap and Metafile to the clipboard
						UnsafeNativeMethods.CopyImagesToClipboard(new HandleRef(diagramView, diagramView.Handle), bitmap, metafile);
					}
				}
				finally
				{
					if (metafile != null)
					{
						metafile.Dispose();
					}
				}

				return;
			}
			// Fall back to the built-in support if we can't copy to the clipboard ourselves.
			diagram.CopyImageToClipboard(selectedElements);
		}

		#region Reflected delegates
		private delegate ArrayList GetShapesToDrawDelegate(Diagram @this, ICollection topLevelShapes, out RectangleD boundingBoxForShapes);
		private static readonly GetShapesToDrawDelegate GetShapesToDraw = InitializeGetShapesToDraw();
		private static GetShapesToDrawDelegate InitializeGetShapesToDraw()
		{
			const System.Reflection.BindingFlags bindingFlags =
				System.Reflection.BindingFlags.DeclaredOnly |
				System.Reflection.BindingFlags.ExactBinding |
				System.Reflection.BindingFlags.Instance |
				System.Reflection.BindingFlags.NonPublic;
			System.Reflection.MethodInfo getShapesToDrawMethodInfo = typeof(Diagram).GetMethod("GetShapesToDraw", bindingFlags, null, new Type[] { typeof(ICollection), typeof(RectangleD).MakeByRefType() }, null);
			return getShapesToDrawMethodInfo != null ? (GetShapesToDrawDelegate)Delegate.CreateDelegate(typeof(GetShapesToDrawDelegate), getShapesToDrawMethodInfo, false) : null;
		}

		private delegate IntPtr GetCompatibleBitmapDelegate(Diagram @this, Bitmap bitmap);
		private static readonly GetCompatibleBitmapDelegate GetCompatibleBitmap = InitializeGetCompatibleBitmap();
		private static GetCompatibleBitmapDelegate InitializeGetCompatibleBitmap()
		{
			const System.Reflection.BindingFlags bindingFlags =
				System.Reflection.BindingFlags.DeclaredOnly |
				System.Reflection.BindingFlags.ExactBinding |
				System.Reflection.BindingFlags.Instance |
				System.Reflection.BindingFlags.NonPublic;
			System.Reflection.MethodInfo getCompatibleBitmapMethodInfo = typeof(Diagram).GetMethod("GetCompatibleBitmap", bindingFlags, null, new Type[] { typeof(Bitmap) }, null);
			return getCompatibleBitmapMethodInfo != null ? (GetCompatibleBitmapDelegate)Delegate.CreateDelegate(typeof(GetCompatibleBitmapDelegate), getCompatibleBitmapMethodInfo, false) : null;
		}
		#endregion // Reflected delegates

		private static void DrawDiagramShapesForImage(Image image, ArrayList shapesToDraw, PointD sourceLocation, Rectangle clipRectangle)
		{
			using (Graphics graphics = Graphics.FromImage(image))
			{
				// Configure graphics
				if (sourceLocation.X != 0 || sourceLocation.Y != 0)
				{
					graphics.TranslateTransform((float)-sourceLocation.X, (float)-sourceLocation.Y);
				}
				graphics.PageUnit = GraphicsUnit.Inch;
				graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
				graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
				graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
				graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
				// Respect the user's preference for text rendering
				graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;

				// UNDONE: HACK: Draw a white background for the Bitmap since the transparency gets lost after the call to
				// GetHbitmap() (which is the first step of GetCompatibleBitmap()). We need to find a way to get the bitmap
				// on the clipboard without it losing its transparency.
				if (image is Bitmap)
				{
					graphics.Clear(Color.White);
				}

				// Draw the items
				DiagramPaintEventArgs e = new DiagramPaintEventArgs(graphics, clipRectangle, null, false);
				int shapesToDrawCount = shapesToDraw.Count;
				for (int i = 0; i < shapesToDrawCount; i++)
				{
					ShapeElement shapeElement = (ShapeElement)shapesToDraw[i];
					if (!shapeElement.IsDeleted)
					{
						shapeElement.OnPaintShape(e);
					}
				}
			}
		}

		#region GetSelectedShapesForImage method and support
		/// <summary>
		/// Obtains an <see cref="ICollection"/> of <see cref="ShapeElement"/>s that is appropriate for rendering as an image
		/// based on the current selection.
		/// </summary>
		/// <param name="diagram">The <see cref="Diagram"/> containing the current selection.</param>
		private static ICollection GetSelectedShapesForImage(Diagram diagram)
		{
			DiagramView diagramView = diagram.ActiveDiagramView;
			if (diagramView == null)
			{
				return null;
			}

			SelectedShapesCollection selection = diagramView.Selection;
			IEnumerator enumerator = selection.GetEnumerator();
			if (!enumerator.MoveNext())
			{
				return null;
			}
			// Note that we ignore any field/subfield portions of the DiagramItems.
			// These are automatically copied as part of the shape.
			ShapeElement shape = ((DiagramItem)enumerator.Current).Shape;
			if (shape == diagram)
			{
				// In this case, we want to copy the nested children of the diagram,
				// not the diagram itself. Copying the diagram gets all of the extra
				// whitespace in the diagram itself. Diagrams don't have relative
				// children, so we just need to get the nested children.
				return diagram.NestedChildShapes;
			}

			HashSet<ShapeElement, ShapeElement> shapesSet = new HashSet<ShapeElement, ShapeElement>(selection.Count * 3, KeyProvider<ShapeElement, ShapeElement>.Default);
			shape = ResolveTopLevelShape(shape, diagram);
			shapesSet[shape] = shape;

			while (enumerator.MoveNext())
			{
				shape = ((DiagramItem)enumerator.Current).Shape;
				// Check whether the current shape is the diagram. The diagram will usually be
				// the first item in the selection if it is part of it at all, but just in case...
				if (shape == diagram)
				{
					return diagram.NestedChildShapes;
				}
				shape = ResolveTopLevelShape(shape, diagram);
				shapesSet[shape] = shape;
			}
			// We have the top-level shapes, now go on a link walk to
			// find all links not currently selected that are linked
			// directly or indirectly at both ends to a selected shape.
			AddSharedLinkShapes(shapesSet, shapesSet.ToArray(), diagram);
			return shapesSet;
		}
		/// <summary>
		/// Helper function for <see cref="GetSelectedShapesForImage"/>. Recursively adds <see cref="LinkShape"/>s
		/// to the set of <see cref="ShapeElement"/>s to draw (<paramref name="shapesSet"/>).
		/// </summary>
		/// <param name="shapesSet">The <see cref="ShapeElement"/>s currently being drawn.</param>
		/// <param name="shapes">The set of <see cref="ShapeElement"/>s to walk and attach <see cref="LinkShape"/>s to.</param>
		/// <param name="diagram">The current <see cref="Diagram"/>.</param>
		private static void AddSharedLinkShapes(HashSet<ShapeElement, ShapeElement> shapesSet, IList<ShapeElement> shapes, Diagram diagram)
		{
			int shapesCount = shapes.Count;
			for (int i = 0; i < shapesCount; ++i)
			{
				ShapeElement shape = shapes[i];
				NodeShape nodeShape = shape as NodeShape;
				if (null != nodeShape)
				{
					foreach (LinkShape currentLinkShape in LinkConnectsToNode.GetLink(nodeShape))
					{
						if (shapesSet.Contains(currentLinkShape))
						{
							continue;
						}
						foreach (NodeShape currentNode in currentLinkShape.Nodes)
						{
							if (currentNode != nodeShape)
							{
								ShapeElement resolvedShape = ResolveTopLevelShape(currentNode, diagram);
								LinkShape resolvedLinkShape;
								if (shapesSet.Contains(resolvedShape))
								{
									shapesSet[currentLinkShape] = currentLinkShape;
								}
								else if (null != (resolvedLinkShape = resolvedShape as LinkShape))
								{
									// Check independently if the link can be added. This removes
									// selection order dependencies for links connected to links.
									LinkedElementCollection<NodeShape> secondaryNodes = resolvedLinkShape.Nodes;
									int secondaryNodesCount = secondaryNodes.Count;
									int drawnNodesCount = 0;
									for (; drawnNodesCount < secondaryNodesCount; ++drawnNodesCount)
									{
										if (!shapesSet.Contains(ResolveTopLevelShape(secondaryNodes[drawnNodesCount], diagram)))
										{
											break;
										}
									}
									if (drawnNodesCount == secondaryNodesCount)
									{
										shapesSet[resolvedLinkShape] = resolvedLinkShape;
										shapesSet[currentLinkShape] = currentLinkShape;
									}
								}
							}
						}
					}
				}
				AddSharedLinkShapes(shapesSet, shape.NestedChildShapes, diagram);
				AddSharedLinkShapes(shapesSet, shape.RelativeChildShapes, diagram);
			}
		}
		/// <summary>
		/// Helper function for <see cref="GetSelectedShapesForImage"/>. Given a <see cref="ShapeElement"/>
		/// specified by <paramref name="shape"/> and a <see cref="Diagram"/> specified by <paramref name="diagram"/>,
		/// finds the <see cref="ShapeElement"/> that is an ancestor (or self) of <paramref name="shape"/> and a
		/// direct child of the <paramref name="diagram"/>.
		/// </summary>
		/// <param name="shape">A <see cref="ShapeElement"/>.</param>
		/// <param name="diagram">The containing <see cref="Diagram"/>.</param>
		/// <returns>
		/// The top level <see cref="ShapeElement"/>, or the starting <see cref="ShapeElement"/> if resolution fails
		/// (very unlikely).
		/// </returns>
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
		#endregion // GetSelectedShapesForImage method and support
		#endregion // OnMenuCopyImage method and support

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
				Role oppositeRole;
				ObjectType oppositeRolePlayer;
				if (constraint.ConstraintType == ConstraintType.ExternalUniqueness &&
					null != (oppositeRole = role.OppositeRole as Role) &&
					null != (oppositeRolePlayer = oppositeRole.RolePlayer) &&
					oppositeRolePlayer.IsImplicitBooleanValue)
				{
					role = oppositeRole;
				}
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
			IList selectedElements = SelectedElements;
			if (selectedElements.Count == 1)
			{
				Role role;
				ORMDiagram ormDiagram;
				ExternalConstraintShape ecs;
				SetComparisonConstraint mcec;
				if (null != (role = selectedElements[0] as Role)
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
							// TODO:  Re-initializing the StickyObject is probably inefficient.  Implementing a rule on
							// MCECs whenever their constraint collection is changed would probably be more effective.
							// This is especially true when role sequences are just being moved up and down.  No insertions
							// or deletions, it's just touched.
							//ormDiagram.StickyObject.StickyInitialize();
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
			IList selectedElements = SelectedElements;
			for (int i = selectedElements.Count - 1; i >= 0; i--)
			{
				Role role = selectedElements[i] as Role;
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
			IList selectedElements = SelectedElements;
			for (int i = selectedElements.Count - 1; i >= 0; i--)
			{
				Role role = selectedElements[i] as Role;
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
		/// Set the selected FactTypeShape to the selected display orientation
		/// </summary>
		/// <param name="orientation">New orientation</param>
		protected virtual void OnMenuDisplayOrientation(DisplayOrientation orientation)
		{
			Store store = Store;
			if (store == null)
			{
				return;
			}

			DomainPropertyInfo displayOrientationPropertyInfo = store.DomainDataDirectory.FindDomainProperty(FactTypeShape.DisplayOrientationDomainPropertyId);
			Debug.Assert(displayOrientationPropertyInfo != null);

			using (Transaction t = store.TransactionManager.BeginTransaction(
				Microsoft.VisualStudio.Modeling.Design.ElementPropertyDescriptor.GetSetValueTransactionName(displayOrientationPropertyInfo.DisplayName)))
			{
				IList selectedElements = SelectedElements;
				for (int i = selectedElements.Count - 1; i >= 0; i--)
				{
					ModelElement element = selectedElements[i] as ModelElement;
					FactTypeShape factTypeShape;
					ReadingShape readingShape;
					if (null != (factTypeShape = element as FactTypeShape) ||
						(null != (readingShape = element as ReadingShape) &&
						null != (factTypeShape = readingShape.ParentShape as FactTypeShape)))
					{
						factTypeShape.DisplayOrientation = orientation;
					}
				}
				if (t.HasPendingChanges)
				{
					t.Commit();
				}
			}
		}
		/// <summary>
		/// Set the selected FactTypeShape to the selected constraint display position
		/// </summary>
		/// <param name="position">New position</param>
		protected virtual void OnMenuDisplayConstraintPosition(ConstraintDisplayPosition position)
		{
			Store store = Store;
			if (store == null)
			{
				return;
			}

			DomainPropertyInfo constraintDisplayPositionPropertyInfo = store.DomainDataDirectory.FindDomainProperty(FactTypeShape.ConstraintDisplayPositionDomainPropertyId);
			Debug.Assert(constraintDisplayPositionPropertyInfo != null);

			using (Transaction t = store.TransactionManager.BeginTransaction(
				Microsoft.VisualStudio.Modeling.Design.ElementPropertyDescriptor.GetSetValueTransactionName(constraintDisplayPositionPropertyInfo.DisplayName)))
			{
				IList selectedElements = SelectedElements;
				for (int i = selectedElements.Count - 1; i >= 0; i--)
				{
					ModelElement element = selectedElements[i] as ModelElement;
					FactTypeShape factTypeShape;
					ReadingShape readingShape;
					if (null != (factTypeShape = element as FactTypeShape) ||
						(null != (readingShape = element as ReadingShape) &&
						null != (factTypeShape = readingShape.ParentShape as FactTypeShape)))
					{
						factTypeShape.ConstraintDisplayPosition = position;
					}
				}
				if (t.HasPendingChanges)
				{
					t.Commit();
				}
			}
		}
		/// <summary>
		/// Reverse the role order for the selected fact type shape
		/// </summary>
		protected virtual void OnMenuDisplayReverseRoleOrder()
		{
			IList selectedElements = SelectedElements;
			for (int i = selectedElements.Count - 1; i >= 0; i--)
			{
				ModelElement element = selectedElements[i] as ModelElement;
				FactTypeShape factTypeShape;
				ReadingShape readingShape;
				if (null != (factTypeShape =  element as FactTypeShape) ||
					(null != (readingShape = element as ReadingShape) &&
					null != (factTypeShape = readingShape.ParentShape as FactTypeShape)))
				{
					factTypeShape.ReverseDisplayedRoleOrder();
					break;
				}
			}
		}
		/// <summary>
		/// Couple selected disjunctive mandatory and exclusion constraints
		/// </summary>
		protected virtual void OnMenuExclusiveOrCoupler()
		{
			IList selectedElements = SelectedElements;
			if (selectedElements.Count == 2)
			{
				MandatoryConstraint mandatory = null;
				ExclusionConstraint exclusion = null;
				for (int i = 1; i >= 0; i--)
				{
					ModelElement mel = selectedElements[i] as ModelElement;
					PresentationElement pel = mel as PresentationElement;
					IConstraint testConstraint = (pel != null) ? pel.Subject as IConstraint : mel as IConstraint;
					Debug.Assert(testConstraint != null);
					switch (testConstraint.ConstraintType)
					{
						case ConstraintType.DisjunctiveMandatory:
							mandatory = (MandatoryConstraint)testConstraint;
							break;
						case ConstraintType.Exclusion:
							exclusion = (ExclusionConstraint)testConstraint;
							break;
					}
				}
				if (null != mandatory && null != exclusion)
				{
					using (Transaction t = mandatory.Store.TransactionManager.BeginTransaction(ResourceStrings.ExclusiveOrCouplerTransactionName))
					{
						new ExclusiveOrConstraintCoupler(mandatory, exclusion);
						t.Commit();
					}
				}
			}
		}
		/// <summary>
		/// Decouple selected ExclusiveOr constraint
		/// </summary>
		protected virtual void OnMenuExclusiveOrDecoupler()
		{
			// Cache the selected components collection, these actions are modifying it indirectly
			IList selectedComponentsCollection = SelectedElements;
			int selectedComponentsCount = selectedComponentsCollection.Count;
			if (selectedComponentsCount > 0)
			{
				object[] selectedComponents = new object[selectedComponentsCount];
				selectedComponentsCollection.CopyTo(selectedComponents, 0);
				for (int i = 0; i < selectedComponents.Length; ++i)
				{
					object mel = selectedComponents[i];
					PresentationElement pel = mel as PresentationElement;
					MandatoryConstraint constraint = (pel != null) ? pel.Subject as MandatoryConstraint : mel as MandatoryConstraint;
					if (constraint != null)
					{
						using (Transaction t = constraint.Store.TransactionManager.BeginTransaction(ResourceStrings.ExclusiveOrDecouplerTransactionName))
						{
							constraint.ExclusiveOrExclusionConstraint = null;
							if (t.HasPendingChanges)
							{
								t.Commit();
							}
						}
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
				IList selectedElements = SelectedElements;
				int selectedElementsCount = selectedElements.Count;
				for (int i = 0; i < selectedElementsCount; i++)
				{
					FactType factType = ORMEditorUtility.ResolveContextFactType(selectedElements[i]);
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
								ObjectType nestingType = objectification.NestingType;
								if (nestingType.IsIndependent)
								{
									nestingType.IsIndependent = false;
								}
								else
								{
									objectification.IsImplied = false;
								}
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
								myEnabledCommands |= ORMDesignerCommands.UnobjectifyFactType;
								myVisibleCommands |= ORMDesignerCommands.UnobjectifyFactType;
							}
						}
						// Once we've objectified a fact type, we're done
						break;
					}
				}
			}
		}
		/// <summary>
		/// Unobjectifies the selected fact type or object type.
		/// </summary>
		protected virtual void OnMenuUnobjectifyFactType()
		{
			ORMDiagram diagram = CurrentDiagram as ORMDiagram;
			if (diagram != null)
			{
				IList selectedElements = SelectedElements;
				int selectedElementsCount = selectedElements.Count;
				for (int i = 0; i < selectedElementsCount; i++)
				{
					// ResolveContextFactType will resolve an ObjectType to the FactType that it nests,
					// so we don't need to worry about doing that ourselves.
					FactType factType = ORMEditorUtility.ResolveContextFactType(selectedElements[i]);
					if (factType != null)
					{
						Store store = factType.Store;
						using (Transaction t = store.TransactionManager.BeginTransaction(ResourceStrings.UnobjectifyFactTypeTransactionName))
						{
							Objectification objectification = factType.Objectification;
							Debug.Assert(objectification != null && !objectification.IsImplied);

							Objectification.RemoveExplicitObjectification(objectification);

							if (t.HasPendingChanges)
							{
								t.Commit();
								myEnabledCommands &= ~ORMDesignerCommands.UnobjectifyFactType;
								myVisibleCommands &= ~ORMDesignerCommands.UnobjectifyFactType;
								myEnabledCommands |= ORMDesignerCommands.ObjectifyFactType;
								myVisibleCommands |= ORMDesignerCommands.ObjectifyFactType;
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

					// Activate the same diagram that we currently have selected
					object pDocView = null;
					ErrorHandler.ThrowOnFailure(pNewWindowFrame.GetProperty((int)__VSFPROPID.VSFPROPID_DocView, out pDocView));
					ORMDesignerDocView newView;
					Diagram selectDiagram;
					if (null != (newView = pDocView as ORMDesignerDocView) &&
						null != (selectDiagram = CurrentDiagram))
					{
						newView.SelectDiagram(selectDiagram);
					}
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
	}
}
