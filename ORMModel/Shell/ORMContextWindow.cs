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
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using MSOLE = Microsoft.VisualStudio.OLE.Interop;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// The ToolWindow which is responsible for displaying and allowing
	/// the update of notes on elements implementing INoteOwner.
	/// </summary>
	[Guid("2B93A7CC-1F28-4347-8A22-644FB7B92090")]
	[CLSCompliant(false)]
	public class ORMContextWindow : ORMToolWindow, MSOLE.IOleCommandTarget
	{

		#region Private data members
		private DiagramView myDiagramView;
		private Partition myPartition;
		private ORMDiagram myDiagram;
		private Guid myContextGuid;
		private Guid myPartitionGuid;
		private Context myContext;
		private IHierarchyContextEnabled myCurrentlySelectedObject;
		private int myGenerations = 1;
		Control myPanel;
		NumericUpDown myUpDownGenerations;

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
					myDiagramView.HasWatermark = false;
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
				viewControl.HasWatermark = false;
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
					!element.IsDeleted &&
					null != (hierarchyElement = element as IHierarchyContextEnabled))
				{
					break;
				}
			}
			if (hierarchyElement == null && myDiagram == null)
			{
				element = myCurrentlySelectedObject as ModelElement;
				if (element != null && element.IsDeleted)
				{
					myCurrentlySelectedObject = null;
					RemoveDiagram();
				}
				hierarchyElement = myCurrentlySelectedObject;
			}
			else if (hierarchyElement == myCurrentlySelectedObject && refresh == false)
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
			UndoState restoreUndoState = undoManager.UndoState;
			try
			{
				undoManager.UndoState = UndoState.DisabledNoFlush;
				using (Transaction t = store.TransactionManager.BeginTransaction("Draw Context Diagram"))
				{
					myDiagram.NestedChildShapes.Clear();
					myDiagram.AutoPopulateShapes = true;
					PlaceObject(hierarchyElement);
					myDiagram.AutoPopulateShapes = false;
					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				}
			}
			finally
			{
				undoManager.UndoState = restoreUndoState;
			}
			return;
		}
		/// <summary>
		/// Places the object on the diagram.
		/// </summary>
		/// <param name="element">The element.</param>
		private void PlaceObject(IHierarchyContextEnabled element)
		{
			List<IHierarchyContextEnabled> elementsToPlace = GetRelatedContextableElements(element, myGenerations);
			elementsToPlace.Sort(delegate(IHierarchyContextEnabled x, IHierarchyContextEnabled y)
			{
				return y.HierarchyContextPlacementPriority - x.HierarchyContextPlacementPriority;
			});
			foreach (IHierarchyContextEnabled elem in elementsToPlace)
			{
				if (elem.ForwardHierarchyContextTo != null)
				{
					continue;
				}
				myDiagram.PlaceORMElementOnDiagram(null, (ModelElement)elem, PointD.Empty);
			}
		}
		/// <summary>
		/// Gets the related contextable elements for the defined number of generations from specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="generations">The numeber of generations out to go.</param>
		/// <returns></returns>
		private static List<IHierarchyContextEnabled> GetRelatedContextableElements(IHierarchyContextEnabled element, int generations)
		{
			List<IHierarchyContextEnabled> relatedElementsCollection = new List<IHierarchyContextEnabled>();
			GetRelatedContextableElementsHelper(element, ref relatedElementsCollection, generations);
			int relatedElementsCount = relatedElementsCollection.Count;
			for (int i = 0; i < relatedElementsCount; ++i)
			{
				IEnumerable<IHierarchyContextEnabled> forcedContextElements = relatedElementsCollection[i].ForcedHierarchyContextElementCollection;
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
		private static void GetRelatedContextableElementsHelper(IHierarchyContextEnabled element, ref List<IHierarchyContextEnabled> relatedElementsCollection, int generations)
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
			if (!relatedElementsCollection.Contains(contextableElement))
			{
				relatedElementsCollection.Add(contextableElement);
				if (contextableElement.ForwardHierarchyContextTo != null)
				{
					GetRelatedContextableElementsHelper(contextableElement.ForwardHierarchyContextTo, ref relatedElementsCollection, generations);
				}
				if (generations > 0 && (relatedElementsCollection.Count == 1 || contextableElement.ContinueWalkingHierarchyContext))
				{
					GetLinkedElements(contextableElement, ref relatedElementsCollection, generations);
				}
			}
		}
		/// <summary>
		/// Gets the linked elements for the specified element.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="relatedElementsCollection">The related elements collection.</param>
		/// <param name="generations">The generations.</param>
		private static void GetLinkedElements(IHierarchyContextEnabled element, ref List<IHierarchyContextEnabled> relatedElementsCollection, int generations)
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
			ReadOnlyCollection<PresentationViewsSubject> collection1 = DomainRoleInfo.GetElementLinks<PresentationViewsSubject>(model, PresentationViewsSubject.SubjectDomainRoleId);
			using (IEnumerator<PresentationViewsSubject> enumerator1 = collection1.GetEnumerator())
			{
				while (enumerator1.MoveNext())
				{
					ORMDiagram element1 = enumerator1.Current.Presentation as ORMDiagram;
					if (element1 != null)
					{
						if (element1.InDragAndDrop == true)
						{
							if (myDiagram != null && model != myDiagram.ModelElement)
							{
								this.RemoveDiagram();
							}
							return;
						}
					}
				}
			}
			if (myDiagram == null || myDiagram.Store != model.Store)
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
			Partition partition = this.myPartition;
			Context context = this.myContext;
			if (partition == null)
			{
				Context previousContext = store.CurrentContext;
				this.myPartition = partition = new Partition(store);
				this.myContext = context = new Context(store);
				partition.AddContext(context);
				partition.RemoveContext(previousContext);
				store.CurrentContext.RemovePartition(partition);
				context.UndoManager.UndoState = UndoState.Disabled;
			}

			Microsoft.VisualStudio.Modeling.UndoManager undoManager = store.CurrentContext.UndoManager;
			UndoState restoreUndoState = undoManager.UndoState;
			try
			{
				undoManager.UndoState = UndoState.DisabledNoFlush;
				using (Transaction t = store.TransactionManager.BeginTransaction("Create Context Diagram"))
				{
					this.myContextGuid = context.Id;
					this.myPartitionGuid = partition.Id;
					ORMDiagram diagram = new ORMDiagram(partition);
					this.myDiagram = diagram;
					diagram.Associate(myDiagramView);
					diagram.ModelElement = model;
					diagram.AutoPopulateShapes = false;
					diagram.DragDropUndoState = UndoState.DisabledNoFlush;
					if (t.HasPendingChanges)
					{
						t.Commit();
					}
				}
			}
			finally
			{
				undoManager.UndoState = restoreUndoState;
			}
			myDiagram.Store.StoreDisposing += new EventHandler(Store_StoreDisposing);
		}
		/// <summary>
		/// Handles the StoreDisposing event of the Store control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
		void Store_StoreDisposing(object sender, EventArgs e)
		{
			this.RemoveDiagram();
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
			if (myDiagram == null)
			{
				return;
			}
			Store currentStore = myDiagram.Store;
			if (currentStore == null)
			{
				return;
			}
			if (!currentStore.Partitions.ContainsKey(myPartitionGuid))
			{
				return;
			}
			Microsoft.VisualStudio.Modeling.UndoManager undoManager = currentStore.CurrentContext.UndoManager;
			UndoState restoreUndoState = undoManager.UndoState;
			// UNDONE: MSBUG This rule should not be doing anything if the parent is deleted.
			// Causes diagram deletion to crash VS
			bool turnedOffResizeRule = false;
			Type ruleType = typeof(Diagram).Assembly.GetType("Microsoft.VisualStudio.Modeling.Diagrams.ResizeParentRule");
			try
			{
				undoManager.UndoState = UndoState.DisabledNoFlush;
				using (Transaction t = currentStore.TransactionManager.BeginTransaction("Transaction"))
				{
					if (ruleType != null)
					{
						currentStore.RuleManager.DisableRule(ruleType);
						turnedOffResizeRule = true;
					}
					t.Commit();
					if (myPartition != null)
					{
						myPartition.Contexts.Clear();
					}
					if (myContext != null)
					{
						myContext.Delete();
					}
					currentStore.Partitions.Remove(myPartitionGuid);
				}
			}
			finally
			{
				if (turnedOffResizeRule)
				{
					currentStore.RuleManager.EnableRule(ruleType);
				}
				myContext = null;
				myPartition = null;
				myDiagram = null;
				myContext = null;
				myContextGuid = Guid.Empty;
				myPartitionGuid = Guid.Empty;
				undoManager.UndoState = restoreUndoState;
				myDiagramView.Diagram = null;
				myCurrentlySelectedObject = null;
			}
		}
#endregion
		#region ORMToolWindow Implementation
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
		/// Attaches event handlers to the store.
		/// </summary>
		protected override void AttachEventHandlers(Store store)
		{
			// empty
		}
		/// <summary>
		/// Detaches event handlers from the store.
		/// </summary>
		protected override void DetachEventHandlers(Store store)
		{
			// empty
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
				return 125;
			}
		}
		/// <summary>
		/// See <see cref="ToolWindow.BitmapIndex"/>.
		/// </summary>
		protected override int BitmapIndex
		{
			get
			{
				return -1;
			}
		}
		#endregion
		#endregion // ORMToolWindow Implementation
	}
}
