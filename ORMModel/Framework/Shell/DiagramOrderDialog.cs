#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.VirtualTreeGrid;

namespace ORMSolutions.ORMArchitect.Framework.Shell
{
	/// <summary>
	/// Display a diagram order dialog
	/// </summary>
	[CLSCompliant(false)]
	public sealed partial class DiagramOrderDialog : Form
	{
		#region DiagramBranch class
		private class DiagramBranch : IBranch
		{
			private Diagram[] myDiagrams;
			private ImageList myImages;
			private BranchModificationEventHandler myModifications;
			public DiagramBranch(DiagramOrderDialog parent)
			{
				myDiagrams = parent.myDiagramOrder;
				myImages = parent.myImages;
			}
			#region Helper methods
			/// <summary>
			/// Move all items up one
			/// </summary>
			public void MoveUpOne(int[] items)
			{
				Array.Sort<int>(items);
				Diagram[] diagrams = myDiagrams;
				for (int i = 0; i < items.Length; ++i)
				{
					int index = items[i];
					if (index > 0)
					{
						Diagram targetDiagram = diagrams[index];
						diagrams[index] = diagrams[index - 1];
						diagrams[index - 1] = targetDiagram;
						BranchModificationEventHandler handler = myModifications;
						if (handler != null)
						{
							handler(this, BranchModificationEventArgs.MoveItem(this, index, index - 1));
						}
					}
				}
			}
			/// <summary>
			/// Move all items down one
			/// </summary>
			public void MoveDownOne(int[] items)
			{
				Array.Sort<int>(items);
				Diagram[] diagrams = myDiagrams;
				int lastItem = diagrams.Length - 1;
				for (int i = items.Length - 1; i >= 0; --i)
				{
					int index = items[i];
					if (index < lastItem)
					{
						Diagram targetDiagram = diagrams[index];
						diagrams[index] = diagrams[index + 1];
						diagrams[index + 1] = targetDiagram;
						BranchModificationEventHandler handler = myModifications;
						if (handler != null)
						{
							handler(this, BranchModificationEventArgs.MoveItem(this, index, index + 1));
						}
					}
				}
			}
			private void MoveDiagramsTo(Diagram[] diagrams, int targetIndex)
			{
				int moveCount = diagrams.Length;
				if (moveCount != 0)
				{
					int[] diagramIndices = new int[moveCount];
					Diagram[] allDiagrams = myDiagrams;
					for (int i = 0; i < moveCount; ++i)
					{
						Diagram moveDiagram = diagrams[i];
						int currentIndex = Array.IndexOf<Diagram>(allDiagrams, moveDiagram);
						if (currentIndex == -1 || currentIndex == targetIndex)
						{
							return; // Bail out
						}
						diagramIndices[i] = currentIndex;
					}
					MoveDiagramsTo(diagramIndices, targetIndex);
				}
			}
			private void MoveDiagramsTo(int[] diagramIndices, int targetIndex)
			{
				Array.Sort<int>(diagramIndices);
				bool movedDown = false;
				for (int i = 0; i < diagramIndices.Length; ++i)
				{
					int currentIndex = diagramIndices[i];
					MoveDiagramTo(currentIndex, (currentIndex > targetIndex && movedDown) ? (targetIndex + 1) : targetIndex);
					if (!movedDown && currentIndex < targetIndex)
					{
						movedDown = true;
					}
					for (int j = i + 1; j < diagramIndices.Length; ++j)
					{
						int adjustIndex = diagramIndices[j];
						if (targetIndex > adjustIndex)
						{
							diagramIndices[j] = adjustIndex - 1;
						}
					}
					if (currentIndex > targetIndex)
					{
						++targetIndex;
					}
				}
			}
			private void MoveDiagramTo(Diagram diagram, int targetIndex)
			{
				MoveDiagramTo(Array.IndexOf<Diagram>(myDiagrams, diagram), targetIndex);
			}
			private void MoveDiagramTo(int diagramIndex, int targetIndex)
			{
				if (diagramIndex == targetIndex)
				{
					return;
				}
				Diagram[] diagrams = myDiagrams;
				Diagram newTargetDiagram = diagrams[diagramIndex];
				if (diagramIndex > targetIndex)
				{
					// Move items down
					for (int i = diagramIndex; i > targetIndex; --i)
					{
						diagrams[i] = diagrams[i - 1];
					}
				}
				else
				{
					// Move items up
					for (int i = diagramIndex; i < targetIndex; ++i)
					{
						diagrams[i] = diagrams[i + 1];
					}
				}
				diagrams[targetIndex] = newTargetDiagram;
				if (myModifications != null)
				{
					myModifications(this, BranchModificationEventArgs.MoveItem(this, diagramIndex, targetIndex));
				}
			}
			#endregion // Helper methods
			#region IBranch Implementation
			VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				return VirtualTreeLabelEditData.Invalid;
			}
			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
			{
				return LabelEditResult.CancelEdit;
			}
			BranchFeatures IBranch.Features
			{
				get
				{
					return BranchFeatures.InsertsAndDeletes;
				}
			}
			VirtualTreeAccessibilityData IBranch.GetAccessibilityData(int row, int column)
			{
				return VirtualTreeAccessibilityData.Empty;
			}
			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				return new VirtualTreeDisplayData((short)myImages.Images.IndexOfKey(myDiagrams[row].GetType().GUID.ToString("N", null)));
			}
			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				return null;
			}
			string IBranch.GetText(int row, int column)
			{
				return myDiagrams[row].Name;
			}
			string IBranch.GetTipText(int row, int column, ToolTipType tipType)
			{
				return (tipType == ToolTipType.Default) ? null : "";
			}
			bool IBranch.IsExpandable(int row, int column)
			{
				return false;
			}
			LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				return default(LocateObjectData);
			}
			event BranchModificationEventHandler IBranch.OnBranchModification
			{
				add
				{
					myModifications += value;
				}
				remove
				{
					myModifications -= value;
				}
			}
			void IBranch.OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
			{
				switch (eventType)
				{
					case DragEventType.Drop:
					case DragEventType.Enter:
						IDataObject data = args.Data;
						if (data.GetDataPresent(typeof(Diagram)))
						{
							// A single diagram
							Diagram rowDiagram = myDiagrams[row];
							Diagram sourceDiagram = (Diagram)data.GetData(typeof(Diagram));
							if (null != sourceDiagram &&
								rowDiagram != sourceDiagram &&
								rowDiagram.Partition == sourceDiagram.Partition)
							{
								if (eventType == DragEventType.Drop)
								{
									MoveDiagramTo(sourceDiagram, row);
								}
								else
								{
									args.Effect = DragDropEffects.Move;
								}
							}
						}
						else if (data.GetDataPresent(typeof(VirtualTreeStartDragData[])))
						{
							Diagram rowDiagram = myDiagrams[row];
							Partition verifyPartition = rowDiagram.Partition;
							VirtualTreeStartDragData[] sourceDragDataItems = (VirtualTreeStartDragData[])data.GetData(typeof(VirtualTreeStartDragData[]));
							Diagram[] dropDiagrams = null;
							bool doDrop = eventType == DragEventType.Drop;
							for (int i = 0; i < sourceDragDataItems.Length; ++i)
							{
								IDataObject sourceData;
								Diagram sourceDiagram;
								if (null == (sourceData = sourceDragDataItems[i].Data as IDataObject) ||
									!sourceData.GetDataPresent(typeof(Diagram)) ||
									null == (sourceDiagram = (Diagram)sourceData.GetData(typeof(Diagram))) ||
									rowDiagram == sourceDiagram ||
									verifyPartition != sourceDiagram.Partition)
								{
									args.Effect = DragDropEffects.None;
									return;
								}
								else if (doDrop)
								{
									(dropDiagrams ?? (dropDiagrams = new Diagram[sourceDragDataItems.Length]))[i] = sourceDiagram;
								}
							}
							if (doDrop)
							{
								MoveDiagramsTo(dropDiagrams, row);
							}
							else
							{
								args.Effect = DragDropEffects.Move;
							}
						}
						break;
				}
			}
			void IBranch.OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
			{
			}
			void IBranch.OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
			{
			}
			VirtualTreeStartDragData IBranch.OnStartDrag(object sender, int row, int column, DragReason reason)
			{
				if (reason == DragReason.DragDrop)
				{
					DataObject data = new DataObject();
					data.SetData(typeof(Diagram), myDiagrams[row]);
					return new VirtualTreeStartDragData(data, DragDropEffects.Move);
				}
				return VirtualTreeStartDragData.Empty;
			}
			StateRefreshChanges IBranch.SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
			{
				return StateRefreshChanges.None;
			}
			StateRefreshChanges IBranch.ToggleState(int row, int column)
			{
				return StateRefreshChanges.None;
			}
			int IBranch.UpdateCounter
			{
				get
				{
					return 0;
				}
			}
			int IBranch.VisibleItemCount
			{
				get
				{
					return myDiagrams.Length;
				}
			}
			#endregion // IBranch implementation
		}
		#endregion // DiagramBranch class
		private static Size LastFormSize;
		private Diagram[] myDiagramOrder;
		private ImageList myImages;
		private DiagramOrderDialog(IList<Diagram> startingOrder, ImageList images)
		{
			int diagramCount = startingOrder.Count;
			Diagram[] order = startingOrder as Diagram[];
			if (order == null)
			{
				order = new Diagram[diagramCount];
				startingOrder.CopyTo(order, 0);
			}
			myDiagramOrder = order;
			myImages = images;
			InitializeComponent();
			DiagramsList.ImageList = images;
		}
		/// <summary>
		/// Display the diagram order dialog with the specified diagram order and update
		/// the diagram order as specified by the user.
		/// </summary>
		/// <param name="serviceProvider">A <see cref="IServiceProvider"/> used to parent the dialog</param>
		/// <param name="docData">The owning <see cref="ModelingDocData"/> of the diagrams being reordered</param>
		/// <param name="diagrams">A list of <see cref="Diagram"/> elements to reorder</param>
		/// <param name="images">The diagram images to display. Images are keyed off the Guid of the diagram type (format "N")</param>
		public static void ShowDialog(IServiceProvider serviceProvider, ModelingDocData docData, IList<Diagram> diagrams, ImageList images)
		{
			DiagramOrderDialog orderDialog = new DiagramOrderDialog(diagrams, images);
			if (orderDialog.ShowDialog(Utility.GetDialogOwnerWindow(serviceProvider)) == DialogResult.OK)
			{
				DiagramDisplay.UpdateDiagramDisplayOrder(docData.Store, orderDialog.myDiagramOrder);
			}
		}
		private void DiagramOrderDialog_Load(object sender, EventArgs e)
		{
			if (LastFormSize != Size.Empty)
			{
				Size = LastFormSize;
			}
			ITree tree = new VirtualTree();
			tree.Root = new DiagramBranch(this);
			VirtualTreeControl treeControl = DiagramsList;
			treeControl.Tree = tree;
			treeControl.Select();
		}
		private int[] SelectedIndices
		{
			get
			{
				int[] retVal = null;
				VirtualTreeControl treeControl = DiagramsList;
				int itemCount = treeControl.SelectedItemCount;
				if (itemCount != 0)
				{
					retVal = new int[itemCount];
					int lastIndex = -1;
					ColumnItemEnumerator selectionIter = treeControl.CreateSelectedItemEnumerator();
					while (selectionIter.MoveNext())
					{
						retVal[++lastIndex] = selectionIter.RowInTree;
					}
				}
				return retVal;
			}
		}
		private void UpButton_Click(object sender, EventArgs e)
		{
			int[] indices = SelectedIndices;
			if (indices != null)
			{
				((DiagramBranch)DiagramsList.Tree.Root).MoveUpOne(indices);
				DiagramsList_SelectionChanged(null, null);
			}
		}
		private void DownButton_Click(object sender, EventArgs e)
		{
			int[] indices = SelectedIndices;
			if (indices != null)
			{
				((DiagramBranch)DiagramsList.Tree.Root).MoveDownOne(indices);
				DiagramsList_SelectionChanged(null, null);
			}
		}
		private void DiagramsList_SelectionChanged(object sender, EventArgs e)
		{
			bool upEnabled = false;
			bool downEnabled = false;
			VirtualTreeControl treeControl = DiagramsList;
			int lastItem = treeControl.Tree.VisibleItemCount - 1;
			if (treeControl.SelectedItemCount != 0)
			{
				upEnabled = true;
				downEnabled = true;
				ColumnItemEnumerator selectionIter = treeControl.CreateSelectedItemEnumerator();
				while (selectionIter.MoveNext())
				{
					int row = selectionIter.RowInTree;
					if (row == 0)
					{
						upEnabled = false;
					}
					if (row == lastItem)
					{
						downEnabled = false;
					}
				}
			}
			UpButton.Enabled = upEnabled;
			DownButton.Enabled = downEnabled;
		}

		private void DiagramOrderDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			LastFormSize = RestoreBounds.Size;
		}

		private void DiagramOrderDialog_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Modifiers == Keys.Alt)
			{
				if (e.KeyCode == Keys.Up)
				{
					if (UpButton.Enabled)
					{
						UpButton_Click(null, null);
						e.Handled = true;
					}
				}
				else if (e.KeyCode == Keys.Down)
				{
					if (DownButton.Enabled)
					{
						DownButton_Click(null, null);
						e.Handled = true;
					}
				}
			}
		}
	}
}