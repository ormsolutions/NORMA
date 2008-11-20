#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Neumont.Tools.Modeling.Shell;
using Neumont.Tools.Modeling;

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// Tool window to contain a single diagram that is used
	/// to step through other diagrams without changing the main selection.
	/// </summary>
	[Guid("19A5C15D-14D4-4A88-9891-A3294077BE56")]
	[CLSCompliant(false)]
	public class ORMDiagramSpyWindow : ORMToolWindow, IORMSelectionContainer, IProvideFrameVisibility, IORMDesignerView
	{
		#region Member Variables
		private ToolWindowDiagramView myDiagramView;
		private ORMDesignerCommandManager myCommandManager;
		private LinkLabel myWatermarkLabel;
		private bool myDiagramSetChanged;
		private bool myDisassociating;
		private Store myStore;
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// public constructor
		/// </summary>
		/// <param name="serviceProvider"></param>
		public ORMDiagramSpyWindow(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
		#endregion // Constructor
		#region Public activation methods
		/// <summary>
		/// Activate the specified <paramref name="diagram"/>
		/// </summary>
		/// <param name="diagram">The <see cref="Diagram"/> to activate</param>
		/// <returns>true if the diagram is successfully activated</returns>
		public bool ActivateDiagram(Diagram diagram)
		{
			if (diagram != null) // Sanity check
			{
				bool calledShow = false;
				Store spyOnStore = myStore;
				Store testStore = diagram.Store;
				if (spyOnStore == null)
				{
					// Either the window has never been shown, or it is not currently
					// active. Pre-check the store against the current selection to
					// see if showing the window will actually initialize to the correct store.
					IMonitorSelectionService selectionService;
					ModelingDocData docData;
					if (null != (selectionService = (IMonitorSelectionService)ExternalServiceProvider.GetService(typeof(IMonitorSelectionService))) &&
						null != (docData = selectionService.CurrentDocument as ModelingDocData) &&
						testStore == (spyOnStore = docData.Store))
					{
						calledShow = true;
						Show();
						spyOnStore = myStore; // Sanity check in case something went wrong
					}
					else
					{
						return false;
					}
				}
				if (testStore == spyOnStore)
				{
					DiagramView diagramView = myDiagramView;
					Diagram oldDiagram = diagramView.Diagram;
					bool reselectOldDiagram = false;
					if (oldDiagram != null)
					{
						reselectOldDiagram = oldDiagram == diagram;
						if (!reselectOldDiagram)
						{
							myDisassociating = true;
							try
							{
								oldDiagram.Disassociate(diagramView);
							}
							finally
							{
								myDisassociating = false;
							}
						}
					}
					if (!reselectOldDiagram)
					{
						diagram.Associate(diagramView);
					}
					AdjustVisibility(true, false);
					if (!calledShow)
					{
						Show();
					}
					if (reselectOldDiagram && diagramView.Selection.PrimaryItem != null)
					{
						OnSelectionChanging(EventArgs.Empty);
						OnSelectionChanged(EventArgs.Empty);
					}
					else
					{
						SetSelectedComponents(new object[] { diagram });
					}
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Get the currently displayed diagram
		/// </summary>
		public Diagram ActiveDiagram
		{
			get
			{
				return myDiagramView.Diagram;
			}
		}
		/// <summary>
		/// Select the specified <paramref name="shape"/> in the spy window
		/// </summary>
		/// <param name="shape">The <see cref="ShapeElement"/> to activate and show</param>
		/// <returns>true if activation was successful</returns>
		public bool ActivateShape(ShapeElement shape)
		{
			Diagram diagram = shape as Diagram;
			if (ActivateDiagram(diagram ?? shape.Diagram))
			{
				if (diagram == null)
				{
					// Do not select the rectangle for a diagram, the view zooms out to fit it in the window
					myDiagramView.DiagramClientView.EnsureVisible(GetShapeBoundingBox(shape), DiagramClientView.EnsureVisiblePreferences.ScrollIntoViewCenter);
					// We could select a diagram, but this leaves it with the old selection state instead
					// if this is the currently selected spy diagram. If the user really wants the diagram,
					// they are only one click away after this point.
					myDiagramView.Selection.Set(new DiagramItem(shape));
				}
				return true;
			}
			return false;
		}
		/// <summary>
		/// Select the specified <paramref name="diagramItem"/> in the spy window
		/// </summary>
		/// <param name="diagramItem">The <see cref="DiagramItem"/> to activate and show</param>
		/// <returns>true if activation was successful</returns>
		public bool ActivateDiagramItem(DiagramItem diagramItem)
		{
			if (diagramItem != null && ActivateDiagram(diagramItem.Diagram))
			{
				if (!diagramItem.IsDiagram)
				{
					myDiagramView.DiagramClientView.EnsureVisible(GetShapeBoundingBox(diagramItem.Shape), DiagramClientView.EnsureVisiblePreferences.ScrollIntoViewCenter);
					myDiagramView.Selection.Set(diagramItem);
				}
				return true;
			}
			return false;
		}
		/// <summary>
		/// Get an accurate bounding box for a shape, which we'll define as
		/// the bounding box for the top-most non-diagram shape in the parent hierarchy.
		/// </summary>
		private RectangleD GetShapeBoundingBox(ShapeElement shape)
		{
			ShapeElement parentShape = shape.ParentShape;
			while (parentShape != null && !(parentShape is Diagram))
			{
				shape = parentShape;
				parentShape = shape.ParentShape;
			}
			return shape.BoundingBox;
		}
		#endregion // Public activation methods
		#region ORMToolWindow overrides
		/// <summary>
		/// Required override. Attach handlers for tracking the current diagram.
		/// </summary>
		protected override void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{
			DomainDataDirectory dataDirectory = store.DomainDataDirectory;
			DomainClassInfo classInfo = dataDirectory.FindDomainClass(typeof(Diagram));
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementAddedEventArgs>(DiagramAddedEvent), action);
			eventManager.AddOrRemoveHandler(classInfo, new EventHandler<ElementDeletedEventArgs>(DiagramRemovedEvent), action);
			eventManager.AddOrRemoveHandler(dataDirectory.FindDomainProperty(Diagram.NameDomainPropertyId), new EventHandler<ElementPropertyChangedEventArgs>(DiagramNameChangedEvent), action);
			eventManager.AddOrRemoveHandler(new EventHandler<ElementEventsEndedEventArgs>(ElementEventsEnded), action);
			myStore = (action == EventHandlerAction.Add) ? store : null;
			myDiagramSetChanged = true;
			AdjustVisibility(false, false);
		}
		private void DiagramAddedEvent(object sender, ElementAddedEventArgs e)
		{
			ModelElement element = e.ModelElement;
			if (element.Store.DefaultPartition == element.Partition)
			{
				myDiagramSetChanged = true;
			}
		}
		private void DiagramRemovedEvent(object sender, ElementDeletedEventArgs e)
		{
			ModelElement element = e.ModelElement;
			if (element.Store.DefaultPartition == element.Partition)
			{
				myDiagramSetChanged = true;
				if (element == myDiagramView.Diagram)
				{
					// Note that this is unlikely, the diagram will be disassociatin firts
					AdjustVisibility(false, true);
				}
			}
		}
		private void Disassociate()
		{
			DiagramView diagramView = myDiagramView;
			Diagram diagram = diagramView.Diagram;
			if (diagram != null)
			{
				myDisassociating = true;
				try
				{
					diagram.Disassociate(diagramView);
				}
				finally
				{
					myDisassociating = false;
				}
			}
		}
		private void DiagramDisassociatingEvent(object sender, EventArgs e)
		{
			if (!myDisassociating)
			{
				// Handle this if it wasn't initiated by this window
				myDiagramSetChanged = true;
				AdjustVisibility(false, false);
			}
		}
		private void DiagramNameChangedEvent(object sender, ElementPropertyChangedEventArgs e)
		{
			ModelElement element = e.ModelElement;
			if (element.Store.DefaultPartition == element.Partition)
			{
				myDiagramSetChanged = true;
			}
		}
		private void ElementEventsEnded(object sender, ElementEventsEndedEventArgs e)
		{
			if (myDiagramView.Diagram == null)
			{
				RebuildWatermark();
			}
			else
			{
				CommandManager.UpdateCommandStatus();
			}
		}
		private void AdjustVisibility(bool diagramVisible, bool deferRebuildWatermark)
		{
			if (!diagramVisible)
			{
				DiagramView view = myDiagramView;
				Diagram diagram = view.Diagram;
				if (diagram != null)
				{
					myDisassociating = true;
					try
					{
						diagram.Disassociate(view);
					}
					finally
					{
						myDisassociating = false;
					}
					SetSelectedComponents(null);
				}

				if (!deferRebuildWatermark)
				{
					RebuildWatermark();
				}
			}
			myDiagramView.Visible = diagramVisible;
			myWatermarkLabel.Visible = !diagramVisible;
		}
		private void RebuildWatermark()
		{
			if (myDiagramSetChanged)
			{
				myDiagramSetChanged = false;
				LinkLabel watermarkLabel = myWatermarkLabel;
				Store store = myStore;
				ReadOnlyCollection<Diagram> diagrams;
				int diagramCount;
				LinkLabel.LinkCollection links = watermarkLabel.Links;
				if (store == null ||
					store.Disposed ||
					0 == (diagramCount = (diagrams = store.ElementDirectory.FindElements<Diagram>(true)).Count))
				{
					watermarkLabel.Text = ResourceStrings.DiagramSpyNoSelection;
					links.Clear();
				}
				else
				{
					Diagram[] diagramArray = new Diagram[diagramCount];
					diagrams.CopyTo(diagramArray, 0);
					Partition targetPartition = store.DefaultPartition;
					Array.Sort<Diagram>(
						diagramArray,
						delegate(Diagram left, Diagram right)
						{
							// Filter diagrams, such as the context window, that are not in the default partition
							if (left.Partition != targetPartition)
							{
								if (right.Partition == targetPartition)
								{
									return 1;
								}
							}
							else if (right.Partition != targetPartition)
							{
								return -1;
							}
							return string.Compare(left.Name, right.Name, StringComparison.CurrentCultureIgnoreCase);
						});
					for (int i = diagramCount - 1; i >= 0; --i)
					{
						if (diagramArray[i].Partition == targetPartition)
						{
							break;
						}
						--diagramCount;
					}
					if (diagramCount == 0)
					{
						watermarkLabel.Text = ResourceStrings.DiagramSpyNoSelection;
						links.Clear();
					}
					else
					{
						StringBuilder builder = new StringBuilder(ResourceStrings.DiagramSpyDiagramListStart);
						string listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator + " ";
						int separatorLength = listSeparator.Length;
						int offset = builder.Length;
						links.Clear();
						for (int i = 0; i < diagramCount; ++i)
						{
							Diagram diagram = diagramArray[i];
							string diagramName = diagram.Name;
							int nameLength = diagramName.Length;
							if (i != 0)
							{
								offset += separatorLength;
								builder.Append(listSeparator);
							}
							builder.Append(diagramName);
							links.Add(offset, nameLength, diagram);
							offset += nameLength;
						}
						watermarkLabel.Text = builder.ToString();
					}
				}
			}
		}
		/// <summary>
		/// called when document current selected document changes
		/// </summary>
		protected override void OnCurrentDocumentChanged()
		{
			base.OnCurrentDocumentChanged();
			LoadWindow();
		}
		/// <summary>
		/// returns string to be displayed as window title
		/// </summary>
		public override string WindowTitle
		{
			get
			{
				return ResourceStrings.DiagramSpyWindowTitle;
			}
		}
		/// <summary>
		/// See <see cref="ToolWindow.BitmapResource"/>.
		/// </summary>
		protected override int BitmapResource
		{
			get
			{
				return PackageResources.Id.ToolWindowIcons;
			}
		}
		/// <summary>
		/// See <see cref="ToolWindow.BitmapIndex"/>.
		/// </summary>
		protected override int BitmapIndex
		{
			get
			{
				return PackageResources.ToolWindowIconIndex.DiagramSpy;
			}
		}
		/// <summary>
		/// retuns the SurveyTreeControl this window contains
		/// </summary>
		public override IWin32Window Window
		{
			get
			{
				DiagramView diagramView = myDiagramView;
				if (diagramView == null)
				{
					LoadWindow();
					diagramView = myDiagramView;
				}
				return diagramView.Parent;
			}
		}
		/// <summary>
		/// Update commands when the selection changes
		/// </summary>
		protected override void OnSelectionChanged(EventArgs e)
		{
			CommandManager.UpdateCommandStatus();
		}
		#endregion //ORMToolWindow overrides
		#region IORMDesignerView Implementation
		/// <summary>
		/// Implements <see cref="IORMDesignerView.CurrentDesigner"/>
		/// </summary>
		protected DiagramView CurrentDesigner
		{
			get
			{
				DiagramView view = myDiagramView;
				return (view.Diagram != null) ? view : null;
			}
		}
		DiagramView IORMDesignerView.CurrentDesigner
		{
			get
			{
				return CurrentDesigner;
			}
		}
		/// <summary>
		/// Implements <see cref="IORMDesignerView.CurrentDiagram"/>
		/// </summary>
		protected Diagram CurrentDiagram
		{
			get
			{
				return myDiagramView.Diagram;
			}
		}
		Diagram IORMDesignerView.CurrentDiagram
		{
			get
			{
				return CurrentDiagram;
			}
		}
		/// <summary>
		/// Implements <see cref="IORMDesignerView.Store"/>
		/// </summary>
		protected Store Store
		{
			get
			{
				Diagram diagram = myDiagramView.Diagram;
				Store retVal = null;
				if (diagram != null)
				{
					retVal = diagram.Store;
					if (retVal.ShuttingDown || retVal.Disposed)
					{
						retVal = null;
					}
				}
				return retVal;
			}
		}
		Store IORMDesignerView.Store
		{
			get
			{
				return Store;
			}
		}
		ModelingDocData IORMDesignerView.DocData
		{
			get
			{
				return DocData as ModelingDocData;
			}
		}
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
				return ExternalServiceProvider;
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
		#region IProvideFrameVisibility Implementation
		FrameVisibility IProvideFrameVisibility.CurrentFrameVisibility
		{
			get
			{
				return CurrentFrameVisibility;
			}
		}
		#endregion // IProvideFrameVisibility Implementation
		#region ORMDiagramSpyToolWindow specific
		/// <summary>
		/// Loads the SurveyTreeControl from the current document
		/// </summary>
		protected void LoadWindow()
		{
			ToolWindowDiagramView diagramView = myDiagramView;
			LinkLabel watermarkLabel = myWatermarkLabel;
			if (diagramView == null)
			{
				ContainerControl container = new ContainerControl();
				myDiagramView = diagramView = new ToolWindowDiagramView(this);
				diagramView.DiagramClientView.DiagramDisassociating += new EventHandler(DiagramDisassociatingEvent);
				myWatermarkLabel = watermarkLabel = new LinkLabel();
				watermarkLabel.Dock = DockStyle.Fill;
				watermarkLabel.Site = diagramView.Site;
				watermarkLabel.TextAlign = ContentAlignment.MiddleCenter;
				watermarkLabel.BackColor = SystemColors.ControlLight;
				watermarkLabel.ForeColor = SystemColors.ControlText;
				watermarkLabel.LinkClicked += new LinkLabelLinkClickedEventHandler(WatermarkLinkClicked);
				container.Site = diagramView.Site;
				diagramView.BackColor = SystemColors.Window;
				diagramView.Dock = DockStyle.Fill;
				diagramView.ContextMenuRequestedEvent += new EventHandler<DiagramMouseEventArgs>(this.DesignerContextMenuRequested);
				diagramView.Visible = false;
				DiagramClientView clientView = diagramView.DiagramClientView;
				clientView.ZoomChanged += new ZoomChangedEventHandler(this.DesignerZoomChanged);
				clientView.TakeFocusOnDragOver = true;
				container.SuspendLayout();
				container.Controls.Add(diagramView);
				container.Controls.Add(watermarkLabel);
				container.ResumeLayout();
				Guid commandSetId = typeof(ORMDesignerEditorFactory).GUID;
				Frame.SetGuidProperty((int)__VSFPROPID.VSFPROPID_InheritKeyBindings, ref commandSetId);
			}
			ORMDesignerDocData currentDocument = this.CurrentDocument;
			Store newStore = (currentDocument != null) ? currentDocument.Store : null;
			Store oldStore = myStore;
			if (oldStore != newStore)
			{
				myDiagramSetChanged = true;
				myStore = newStore;
				AdjustVisibility(false, false);
			}
		}
		private void WatermarkLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			ActivateDiagram(e.Link.LinkData as Diagram);
		}
		#endregion // ORMDiagramSpyToolWindow specific
		#region ContextMenu
		// The context menu handlers are on the DiagramDocView, but we took
		// the ToolWindow derivation path from ModelingWindowPane instead of
		// the ModelingDocView/DiagramDocView, so we need to duplicate the
		// context menu support from the DiagramDocView.

		private void DesignerContextMenuRequested(object sender, DiagramMouseEventArgs e)
		{
			OnContextMenuRequested(e);
		}
		/// <summary>
		/// A context menu has been requested at the specified position. Translate the
		/// position to a device coordinates and show the context menu
		/// </summary>
		protected virtual void OnContextMenuRequested(DiagramMouseEventArgs mouseArgs)
		{
			DiagramClientView clientView = mouseArgs.DiagramClientView;
			if (clientView != null)
			{
				Point pt = clientView.PointToScreen(clientView.WorldToDevice(mouseArgs.MousePosition));
				this.ShowContextMenu(ContextMenuId, pt);
			}
		}
		/// <summary>
		/// Update commands when the zoom level changes
		/// </summary>
		private void DesignerZoomChanged(object sender, DiagramEventArgs e)
		{
			IVsUIShell shell = (IVsUIShell)GetService(typeof(SVsUIShell));
			if (shell != null)
			{
				ErrorHandler.ThrowOnFailure(shell.UpdateCommandUI(0));
			}
		}
		/// <summary>
		/// Get the <see cref="CommandID"/> for the context menu to show
		/// </summary>
		protected virtual CommandID ContextMenuId
		{
			get
			{
				return ORMDesignerDocView.ORMDesignerCommandIds.ViewContextMenu;
			}
		}
		/// <summary>
		/// Show the context menu at the specified point
		/// </summary>
		protected void ShowContextMenu(CommandID contextMenuId, Point pt)
		{
			this.MenuService.ShowContextMenu(contextMenuId, pt.X, pt.Y);
		}
		#endregion // ContextMenu
	}
}
