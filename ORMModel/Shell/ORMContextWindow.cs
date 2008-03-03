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
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.Modeling.Shell;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// The ToolWindow which is responsible for displaying and temporary,
	/// auto-generated diagram corresponding to the current selection.
	/// </summary>
	[Guid("2B93A7CC-1F28-4347-8A22-644FB7B92090")]
	[CLSCompliant(false)]
	public class ORMContextWindow : ORMToolWindow
	{
		#region Private data members
		private DiagramView myDiagramView;
		private ORMDiagram myDiagram;
		private IHierarchyContextEnabled myCurrentlySelectedObject;
		private int myGenerations = 1;
		private Control myPanel;
		private NumericUpDown myUpDownGenerations;

		#endregion // Private data members
		#region Construction
		/// <summary>
		/// Returns the ORM Context Window.
		/// </summary>
		public ORMContextWindow(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
		/// <summary>
		/// Standard override
		/// </summary>
		public override string ToString()
		{
			// Return a shorter name. This object is used
			// as the Id for the alternate partition, which
			// can be displayed, so we give it a shorter name.
			return GetType().Name;
		}
		/// <summary>
		/// Create or retrieve the diagram view
		/// </summary>
		public override IWin32Window Window
		{
			get
			{
				if (myPanel != null)
				{
					return myPanel;
				}

				myPanel = new System.Windows.Forms.ContainerControl();
				myPanel.SuspendLayout();
				Control pnlOption = new System.Windows.Forms.ContainerControl();
				Label lblGenerations = new System.Windows.Forms.Label();
				DiagramView viewControl = myDiagramView;
				if (viewControl == null)
				{
					myDiagramView = viewControl = new DiagramView();
				}
				myUpDownGenerations = new System.Windows.Forms.NumericUpDown();
				// 
				// pnlOption
				// 
				pnlOption.Controls.Add(myUpDownGenerations);
				pnlOption.Controls.Add(lblGenerations);
				//pnlOption.Dock = System.Windows.Forms.DockStyle.Top;
				pnlOption.Location = new System.Drawing.Point(0, 0);
				pnlOption.Name = "Options";
				pnlOption.Size = new System.Drawing.Size(316, 24);
				pnlOption.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
				//pnlOption.TabIndex = 0;
				// 
				// lblGenerations
				// 
				lblGenerations.AutoSize = true;
				lblGenerations.Location = new System.Drawing.Point(4, 4);
				lblGenerations.Name = "lblGenerations";
				lblGenerations.Size = new System.Drawing.Size(64, 13);
				lblGenerations.TabIndex = 0;
				lblGenerations.Text = "Generations";
				// 
				// myUpDownGenerations
				// 
				myUpDownGenerations.Location = new System.Drawing.Point(73, 1);
				myUpDownGenerations.Name = "numericUpDown1";
				myUpDownGenerations.Size = new System.Drawing.Size(50, 20);
				myUpDownGenerations.TabIndex = 1;
				myUpDownGenerations.Minimum = 0;
				myUpDownGenerations.Maximum = 3;
				myUpDownGenerations.ValueChanged += new EventHandler(upDownGenerations_ValueChanged);
				myUpDownGenerations.Value = myGenerations;
				// 
				// viewControl
				// 
				viewControl.Size = new Size(myPanel.ClientSize.Width, myPanel.ClientSize.Height - 24);
				viewControl.Location = new Point(0, 24);
				viewControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;
				//
				// myPanel
				//
				myPanel.Controls.Add(viewControl);
				myPanel.Controls.Add(pnlOption);
				myPanel.TabIndex = 0;
				myPanel.ResumeLayout(true);
				return myPanel;
				//return viewControl;
			}
		}
		#region TempControls
		//ComboBox cboRouting;
		//ComboBox cboPlacement;
		//public Control TempControls()
		//{
		//  Control panel = new Control();
		//  cboRouting = new ComboBox();
		//  cboPlacement = new ComboBox();
		//  Button btn = new Button();

		//  btn.Click += new EventHandler(btn_Click);
		//  btn.Location = new System.Drawing.Point(0, 0);

		//  cboRouting.Items.AddRange(new object[] {
		//    VGRoutingStyle.VGRouteCenterToCenter
		//    ,VGRoutingStyle.VGRouteFlowchartNS
		//    ,VGRoutingStyle.VGRouteFlowchartWE
		//    ,VGRoutingStyle.VGRouteNetwork
		//    ,VGRoutingStyle.VGRouteNone
		//    ,VGRoutingStyle.VGRouteOrgChartNS
		//    ,VGRoutingStyle.VGRouteOrgChartWE
		//    ,VGRoutingStyle.VGRouteRightAngle
		//    ,VGRoutingStyle.VGRouteSimpleHH
		//    ,VGRoutingStyle.VGRouteSimpleHV
		//    ,VGRoutingStyle.VGRouteSimpleVH
		//    ,VGRoutingStyle.VGRouteSimpleVV
		//    ,VGRoutingStyle.VGRouteStraight
		//    ,VGRoutingStyle.VGRouteTreeNS
		//    ,VGRoutingStyle.VGRouteTreeWE});
		//  cboRouting.Items.AddRange(new object[]{
		//    PlacementValueStyle.VGPlaceCircular
		//    ,PlacementValueStyle.VGPlaceEW
		//    ,PlacementValueStyle.VGPlaceNone
		//    ,PlacementValueStyle.VGPlaceNS
		//    ,PlacementValueStyle.VGPlaceSN
		//    ,PlacementValueStyle.VGPlaceUndirected
		//    ,PlacementValueStyle.VGPlaceWE
		//    ,PlacementValueStyle.VGPlaceWideENE
		//    ,PlacementValueStyle.VGPlaceWideESE
		//    ,PlacementValueStyle.VGPlaceWideNNE
		//    ,PlacementValueStyle.VGPlaceWideNNW
		//    ,PlacementValueStyle.VGPlaceWideSSE
		//    ,PlacementValueStyle.VGPlaceWideSSW
		//    ,PlacementValueStyle.VGPlaceWideWNW
		//    ,PlacementValueStyle.VGPlaceWideWSW});

		//  panel.Dock = DockStyle.Right;
		//  return panel;
		//}
		//void btn_Click(object sender, EventArgs e)
		//{
		//  using (Transaction t = myDiagram.Store.TransactionManager.BeginTransaction("Layout Context Diagram"))
		//  {
		//    myDiagram.AutoLayoutShapeElements(myDiagram.NestedChildShapes, VGRoutingStyle.VGRouteNetwork, PlacementValueStyle.VGPlaceCircular, true);
		//    if (t.HasPendingChanges)
		//    {
		//      t.Commit();
		//    }
		//  }
		//}
		#endregion

		#endregion // Construction
		#region properties
		/// <summary>
		/// Gets or sets the number of generations out to display from the selected element.
		/// </summary>
		/// <value>The generations.</value>
		public int Generations
		{
			get { return myGenerations; }
			set
			{
				if (value == myGenerations)
				{
					return;
				}
				myGenerations = value;
				this.DrawDiagram(true);
			}
		}
		#endregion
		#region Implementation
		/// <summary>
		/// Draws the diagram.
		/// </summary>
		private void DrawDiagram()
		{
			DrawDiagram(false);
		}
		/// <summary>
		/// Draws the diagram.
		/// </summary>
		/// <param name="refresh">if set to <c>true</c> the diagram will redraw.</param>
		private void DrawDiagram(bool refresh)
		{
			if (this.myDiagramView == null)
			{
				return;
			}
			IHierarchyContextEnabled hierarchyElement = null;
			ModelElement element = null;
			object[] selectedElements = GetSelectedElements();
			foreach (object obj in selectedElements)
			{
				if (null != (element = EditorUtility.ResolveContextInstance(obj, false) as ModelElement) &&
					element.Store != null &&
					!element.IsDeleted &&
					null != (hierarchyElement = element as IHierarchyContextEnabled))
				{
					break;
				}
			}
			DrawDiagram(refresh, element, hierarchyElement);
		}
		private void DrawDiagram(Store store)
		{
			if (this.myDiagramView == null)
			{
				if (Window == null)
				{
					return;
				}
			}
			foreach (ORMModel model in store.ElementDirectory.FindElements<ORMModel>(true))
			{
				DrawDiagram(false, model, null);
				break;
			}
		}
		private void DrawDiagram(bool refresh, ModelElement element, IHierarchyContextEnabled hierarchyElement)
		{
			bool storeChange = false;
			if (hierarchyElement == null && (myDiagram == null || (element != null && (storeChange = (element.Store != myDiagram.Store)))))
			{
				ModelElement selectedElement = element;
				element = myCurrentlySelectedObject as ModelElement;
				if (element != null && ((element.IsDeleted || element.Store == null) || storeChange))
				{
					myCurrentlySelectedObject = null;
					RemoveDiagram();
				}
				hierarchyElement = myCurrentlySelectedObject;
				if (hierarchyElement == null && selectedElement != null && !selectedElement.IsDeleted && selectedElement.Store != null)
				{
					ORMModel attachToModel = selectedElement as ORMModel;
					if (attachToModel != null)
					{
						EnsureDiagram(attachToModel);
					}
				}
			}
			else if (hierarchyElement == myCurrentlySelectedObject && !refresh && (myDiagram != null && myDiagram.HasChildren))
			{
				return;
			}
			if (hierarchyElement == null)
			{
				return;
			}
			myCurrentlySelectedObject = hierarchyElement;
			ORMModel model = hierarchyElement.Model;
			if (model == null)
			{
				return;
			}
			this.EnsureDiagram(model);
			if (myDiagram == null)
			{
				return;
			}
			Store store = element.Store;
			Microsoft.VisualStudio.Modeling.UndoManager undoManager = store.CurrentContext.UndoManager;
			using (Transaction t = store.TransactionManager.BeginTransaction("Draw Context Diagram"))
			{
				myDiagram.NestedChildShapes.Clear();
				myDiagram.AutoPopulateShapes = true;

				PlaceObject(hierarchyElement);
				LinkedElementCollection<ShapeElement> collection = myDiagram.NestedChildShapes;
				LayoutManager bl = new LayoutManager(myDiagram, (myDiagram.Store as IORMToolServices).GetLayoutEngine(typeof(ORMRadialLayoutEngine)));
				foreach (ShapeElement shape in collection)
				{
					bl.AddShape(shape, false);
				}
				bl.Layout();

				myDiagram.AutoPopulateShapes = false;
				if (t.HasPendingChanges)
				{
					t.Commit();
				}
			}
			return;
		}
		/// <summary>
		/// Places the object on the diagram.
		/// </summary>
		/// <param name="element">The element.</param>
		private void PlaceObject(IHierarchyContextEnabled element)
		{
			SortedList<IHierarchyContextEnabled, int> elementsToPlace = GetRelatedContextableElements(element, myGenerations);
			IList<IHierarchyContextEnabled> elements = elementsToPlace.Keys;
			foreach (IHierarchyContextEnabled elem in elements)
			{
				if (elem.ForwardHierarchyContextTo != null)
				{
					continue;
				}
				myDiagram.PlaceORMElementOnDiagram(null, (ModelElement)elem, PointD.Empty, ORMPlacementOption.None);
			}
		}
		/// <summary>
		/// Gets the related contextable elements for the defined number of generations from specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="generations">The numeber of generations out to go.</param>
		/// <returns></returns>
		private static SortedList<IHierarchyContextEnabled, int> GetRelatedContextableElements(IHierarchyContextEnabled element, int generations)
		{
			SortedList<IHierarchyContextEnabled, int> relatedElementsCollection = new SortedList<IHierarchyContextEnabled, int>(HierarchyContextPlacePrioritySortComparer.Instance);
			GetRelatedContextableElementsHelper(element, ref relatedElementsCollection, generations);
			IList<IHierarchyContextEnabled> keys = relatedElementsCollection.Keys;
			int relatedElementsCount = keys.Count;
			for (int i = 0; i < relatedElementsCount; ++i)
			{
				IEnumerable<IHierarchyContextEnabled> forcedContextElements = keys[i].ForcedHierarchyContextElementCollection;
				if (forcedContextElements != null)
				{
					foreach (IHierarchyContextEnabled dependantContextableElement in forcedContextElements)
					{
						GetRelatedContextableElementsHelper(dependantContextableElement, ref relatedElementsCollection, 0);
					}
				}
			}
			return relatedElementsCollection;
		}
		/// <summary>
		/// Dont use this method. Use GetRelatedContextableElements instead.
		/// helper function for GetRelatedContextableElements.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="relatedElementsCollection">The related elements collection.</param>
		/// <param name="generations">The generations.</param>
		private static void GetRelatedContextableElementsHelper(IHierarchyContextEnabled element, ref SortedList<IHierarchyContextEnabled, int> relatedElementsCollection, int generations)
		{
			if (element == null)
			{
				return;
			}
			IHierarchyContextEnabled contextableElement = EditorUtility.ResolveContextInstance(element, false) as IHierarchyContextEnabled;
			if (contextableElement == null)
			{
				return;
			}
			if (!relatedElementsCollection.ContainsKey(contextableElement))
			{
				relatedElementsCollection.Add(contextableElement, generations);
			}
			else
			{
				if (relatedElementsCollection[contextableElement] >= generations)
				{
					return;
				}
				else
				{
					relatedElementsCollection[contextableElement] = generations;
				}
			}
			if (contextableElement.ForwardHierarchyContextTo != null)
			{
				GetRelatedContextableElementsHelper(contextableElement.ForwardHierarchyContextTo, ref relatedElementsCollection, generations);
			}
			if (generations > 0 && (relatedElementsCollection.Count == 1 || contextableElement.ContinueWalkingHierarchyContext))
			{
				GetLinkedElements(contextableElement, ref relatedElementsCollection, generations);
			}
		}
		/// <summary>
		/// Gets the linked elements for the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="relatedElementsCollection">The related elements collection.</param>
		/// <param name="generations">The generations.</param>
		private static void GetLinkedElements(IHierarchyContextEnabled element, ref SortedList<IHierarchyContextEnabled, int> relatedElementsCollection, int generations)
		{
			ReadOnlyCollection<ElementLink> col = DomainRoleInfo.GetAllElementLinks((ModelElement)element);
			foreach (ElementLink link in col)
			{
				foreach (ModelElement modelElement in link.LinkedElements)
				{
					IHierarchyContextEnabled contextableElement = modelElement as IHierarchyContextEnabled;
					if (contextableElement == null || contextableElement == element)
					{
						continue;
					}
					int decrement = contextableElement.HierarchyContextDecrementCount;
					if (relatedElementsCollection.Count == 1 && contextableElement.ForwardHierarchyContextTo != null)
					{
						decrement = 0;
					}
					GetRelatedContextableElementsHelper(contextableElement, ref relatedElementsCollection, generations - decrement);
				}
			}
		}
		/// <summary>
		/// Gets the selected objects from main diagram.
		/// </summary>
		private object[] GetSelectedElements()
		{
			uint count = this.CountSelectedObjects();
			object[] selectedShapes = new object[count];
			this.GetSelectedObjects(count, selectedShapes);
			return selectedShapes;
		}
		#endregion
		#region Diagram Management Methods
		/// <summary>
		/// Ensures that the diagram exists and that it is using the correct store.
		/// </summary>
		private void EnsureDiagram(ModelElement model)
		{
			if (myDiagram == null || myDiagram.IsDeleted || myDiagram.Store != model.Store)
			{
				ResetDiagram(model);
			}
		}
		/// <summary>
		/// Resets the diagram.
		/// </summary>
		private void ResetDiagram(ModelElement model)
		{
			this.RemoveDiagram();
			this.CreateDiagram(model);
		}
		/// <summary>
		/// Creates the diagram.
		/// </summary>
		private void CreateDiagram(ModelElement model)
		{
			Store store = model.Store;
			if (store == null || store.ShuttingDown)
			{
				return;
			}
			Partition partition = Partition.FindByAlternateId(store, this);
			ORMDiagram diagram = null;
			if (partition == null)
			{
				partition = new Partition(store);
				partition.AlternateId = this;
				store.StoreDisposing += new EventHandler(Store_StoreDisposing);
			}
			else
			{
				ReadOnlyCollection<ORMDiagram> diagrams = partition.ElementDirectory.FindElements<ORMDiagram>();
				if (diagrams.Count != 0)
				{
					diagram = diagrams[0];
				}
			}
			if (diagram == null)
			{
				// We can get a partition with no diagram with an undo operation. The
				// diagram will be removed, but the partition will remain.
				Microsoft.VisualStudio.Modeling.UndoManager undoManager = store.CurrentContext.UndoManager;
				using (Transaction t = store.TransactionManager.BeginTransaction("Create Context Diagram"))
				{
					diagram = new ORMDiagram(partition);
					diagram.Associate(myDiagramView);
					myDiagramView.HasWatermark = false;
					diagram.ModelElement = model;
					diagram.AutoPopulateShapes = false;
					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				}
			}
			myDiagram = diagram;
			DiagramClientView clientView = myDiagramView.DiagramClientView;
			if (clientView.Diagram != diagram)
			{
				clientView.Diagram = diagram;
			}
		}
		/// <summary>
		/// Handles the StoreDisposing event of the Store control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		void Store_StoreDisposing(object sender, EventArgs e)
		{
			Store store = (Store)sender;
			Partition partition = Partition.FindByAlternateId(store, this);
			if (partition != null)
			{
				ReadOnlyCollection<ORMDiagram> diagrams = partition.ElementDirectory.FindElements<ORMDiagram>();
				if (diagrams.Count != 0)
				{
					DiagramClientView clientView = myDiagramView.DiagramClientView;
					if (clientView.Diagram == diagrams[0])
					{
						// This needs to be cleared before the store is gone to
						// avoid a crash when the next diagram is associated
						clientView.Diagram = null;
					}
				}
				store.Partitions.Remove(partition.Id);
			}
		}
		/// <summary>
		/// Handles the ValueChanged event of the upDownGenerations control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		void upDownGenerations_ValueChanged(object sender, EventArgs e)
		{
			myUpDownGenerations.Enabled = false;
			Application.DoEvents();
			try
			{
				Generations = (int)((NumericUpDown)sender).Value;
			}
			finally
			{
				myUpDownGenerations.Enabled = true;
			}
		}
		/// <summary>
		/// Removes the diagram.
		/// </summary>
		private void RemoveDiagram()
		{
			myDiagram = null;
			myDiagramView.Diagram = null;
			myCurrentlySelectedObject = null;
		}
		#endregion
		#region ORMToolWindow Implementation
		/// <summary>
		/// Clear a covered window when the document changes and when the selection changes.
		/// </summary>
		protected override CoveredFrameContentActions CoveredFrameContentActions
		{
			get
			{
				return CoveredFrameContentActions.ClearContentsOnSelectionChanged | CoveredFrameContentActions.ClearContentsOnDocumentChanged;
			}
		}
		/// <summary>
		/// Provide a notification when the selection container has been modified. The
		/// default implemention is empty.
		/// </summary>
		protected override void OnORMSelectionContainerChanged()
		{
			if (this.myDiagram != null)
			{
				if (myDiagram.Store.ShuttingDown)
				{
					this.RemoveDiagram();
				}
			}
			DrawDiagram();
			base.OnORMSelectionContainerChanged();
		}
		/// <summary>
		/// Manages event handlers in the store.
		/// </summary>
		protected override void ManageEventHandlers(Store store, ModelingEventManager eventManager, EventHandlerAction action)
		{
		}
		#region ToolWindowProperties
		/// <summary>
		/// Returns the title of the window.
		/// </summary>
		public override string WindowTitle
		{
			get
			{
				return ResourceStrings.ModelContextWindowTitle;
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
				return PackageResources.ToolWindowIconIndex.ContextWindow;
			}
		}
		#endregion
		#endregion // ORMToolWindow Implementation
		#region HierarchyContextPlacePrioritySortComparer class
		private sealed class HierarchyContextPlacePrioritySortComparer : IComparer<IHierarchyContextEnabled>
		{
			private HierarchyContextPlacePrioritySortComparer() { }
			/// <summary>
			/// Singleton IComparer&lt;IHierarchyContextEnabled&gt; implementation
			/// </summary>
			public static readonly IComparer<IHierarchyContextEnabled> Instance = new HierarchyContextPlacePrioritySortComparer();
			int IComparer<IHierarchyContextEnabled>.Compare(IHierarchyContextEnabled x, IHierarchyContextEnabled y)
			{
				int priorityDifference = y.HierarchyContextPlacementPriority - x.HierarchyContextPlacementPriority;
				return (priorityDifference == 0) ? x.Id.CompareTo(y.Id) : priorityDifference;
			}
		}
		#endregion // HierarchyContextPlacePrioritySortComparer class
	}
}
