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
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.VirtualTreeGrid;

namespace Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid
{
	partial class SurveyTree
	{
		partial class MainList
		{
			partial class ListGrouper
			{
				private sealed class SimpleListShifter : IBranch
				{
					#region Fields and Constructor
					private readonly IBranch myBaseBranch;
					private int myFirstItem;
					private int myCount;
					//private BranchModificationEventHandler myModificationEvents;

					/// <summary>
					/// Shows all elements of the branch from the certain point you 
					/// designate for a numbered amount that you request
					/// </summary>
					/// <param name="baseBranch">The List You Want To Shift</param>
					/// <param name="firstItemIndex">Where You Want To Start The Shift</param>
					/// <param name="count">Amount That You Want Displayed</param>
					public SimpleListShifter(IBranch baseBranch, int firstItemIndex, int count)
					{

						Debug.Assert(baseBranch != null);
						Debug.Assert(firstItemIndex >= 0);
						Debug.Assert(firstItemIndex < baseBranch.VisibleItemCount);
						myBaseBranch = baseBranch;
						myFirstItem = firstItemIndex;
						myCount = count;
						if (myFirstItem + myCount > myBaseBranch.VisibleItemCount)
						{
							myCount = myBaseBranch.VisibleItemCount - myFirstItem;
						}

					}
					#endregion // Fields and Constructor
					#region Accessor properties
					/// <summary>
					/// Gets or sets the first item of a shifter
					/// </summary>
					public int FirstItem
					{
						get { return myFirstItem; }
						set { myFirstItem = value; }
					}
					public int Count
					{
						get { return myCount; }
						set { myCount = value; }
					}
					/// <summary>
					/// Returns last item's index
					/// </summary>
					public int EndIndex
					{
						get { return myFirstItem + myCount - 1; }
					}

					#endregion // Accessor properties
					#region IBranch Members

					public VirtualTreeLabelEditData BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
					{
						return myBaseBranch.BeginLabelEdit(row + myFirstItem, column, activationStyle);
					}
					public LabelEditResult CommitLabelEdit(int row, int column, string newText)
					{
						return myBaseBranch.CommitLabelEdit(row + myFirstItem, column, newText);
					}
					public BranchFeatures Features
					{
						get
						{
							return myBaseBranch.Features;
						}
					}
					public VirtualTreeAccessibilityData GetAccessibilityData(int row, int column)
					{
						return myBaseBranch.GetAccessibilityData(row + myFirstItem, column);
					}
					public VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
					{
						return myBaseBranch.GetDisplayData(row + myFirstItem, column, requiredData);
					}

					/// <summary>
					/// Retrieve an object associated with this branch. See ObjectStyle
					/// for descriptions of the different object styles that the tree will request.
					/// </summary>
					/// <param name="row">Target row</param>
					/// <param name="column">Target column</param>
					/// <param name="style">Style of object to retrieve</param>
					/// <param name="options">Placeholder for setting/returns options. Contents depend on the style.</param>
					/// <returns>
					/// An object or null, with the type of the object determined by the style parameter.
					/// </returns>
					public object GetObject(int row, int column, ObjectStyle style, ref int options)
					{
						return myBaseBranch.GetObject(row + myFirstItem, column, style, ref options);
					}
					public string GetText(int row, int column)
					{
						return myBaseBranch.GetText(row + myFirstItem, column);
					}
					public string GetTipText(int row, int column, ToolTipType tipType)
					{
						return myBaseBranch.GetTipText(row + myFirstItem, column, tipType);
					}
					public bool IsExpandable(int row, int column)
					{
						return myBaseBranch.IsExpandable(row + myFirstItem, column);
					}
					public LocateObjectData LocateObject(object obj, ObjectStyle style, int locateOptions)
					{
						LocateObjectData data = myBaseBranch.LocateObject(obj, style, locateOptions);
						if (data.Row >= 0)
						{
							data.Row -= myFirstItem;
						}
						return data;
					}
					public event BranchModificationEventHandler OnBranchModification
					{
						add
						{
							// Don't need to do anything here. This will always be
							// a child of a grouper, which will have events attached
							// to the main list.

							//myBaseBranch.OnBranchModification += value;
						}
						remove
						{
							//myBaseBranch.OnBranchModification -= value;
						}
					}
					public void OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
					{
						myBaseBranch.OnDragEvent(sender, row + myFirstItem, column, eventType, args);
					}
					public void OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
					{
						myBaseBranch.OnGiveFeedback(args, row + myFirstItem, column);
					}
					public void OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
					{
						myBaseBranch.OnQueryContinueDrag(args, row + myFirstItem, column);
					}
					public VirtualTreeStartDragData OnStartDrag(object sender, int row, int column, DragReason reason)
					{
						return myBaseBranch.OnStartDrag(sender, row + myFirstItem, column, reason);
					}
					public StateRefreshChanges SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
					{
						return myBaseBranch.SynchronizeState(row, column, matchBranch, matchRow + myFirstItem, matchColumn);
					}
					public StateRefreshChanges ToggleState(int row, int column)
					{
						return myBaseBranch.ToggleState(row + myFirstItem, column);
					}
					public int UpdateCounter
					{
						get
						{
							return myBaseBranch.UpdateCounter;
						}
					}
					public int VisibleItemCount
					{
						get
						{
							return myCount;
						}
					}
					#endregion // IBranch Members
				}
			}
		}
	}
}
