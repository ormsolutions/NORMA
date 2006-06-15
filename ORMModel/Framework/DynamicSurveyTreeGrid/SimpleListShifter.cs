using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.VirtualTreeGrid;
using System.Diagnostics;

namespace Neumont.Tools.ORM.Framework.DynamicSurveyTreeGrid
{
	class SimpleListShifter : IBranch
	{
		#region Variables
		IBranch myBaseBranch;
		int myFirstItem;
		int myCount;
		#endregion

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
				myCount = myBaseBranch.VisibleItemCount - myFirstItem;
		}

		#region IBranch Members

		protected VirtualTreeLabelEditData BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
		{
			return myBaseBranch.BeginLabelEdit(row + myFirstItem, column, activationStyle);
		}
		VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
		{
			return BeginLabelEdit(row, column, activationStyle);
		}
		protected LabelEditResult CommitLabelEdit(int row, int column, string newText)
		{
			return myBaseBranch.CommitLabelEdit(row + myFirstItem, column, newText);
		}
		LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
		{
			return CommitLabelEdit(row, column, newText);
		}
		protected BranchFeatures Features
		{
			get { return myBaseBranch.Features; }
		}
		BranchFeatures IBranch.Features
		{
			get { return Features; }
		}
		protected VirtualTreeAccessibilityData GetAccessibilityData(int row, int column)
		{
			return myBaseBranch.GetAccessibilityData(row + myFirstItem, column);
		}
		VirtualTreeAccessibilityData IBranch.GetAccessibilityData(int row, int column)
		{
			return GetAccessibilityData(row, column);
		}
		protected VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
		{
			return myBaseBranch.GetDisplayData(row + myFirstItem, column, requiredData);
		}
		VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
		{
			return GetDisplayData(row, column, requiredData);
		}
		protected object GetObject(int row, int column, ObjectStyle style, ref int options)
		{
			return myBaseBranch.GetObject(row + myFirstItem, column, style, ref options);
		}
		object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
		{
			return GetObject(row, column, style, ref options);
		}
		protected string GetText(int row, int column)
		{
			return myBaseBranch.GetText(row + myFirstItem, column);
		}
		string IBranch.GetText(int row, int column)
		{
			return GetText(row, column);
		}
		protected string GetTipText(int row, int column, ToolTipType tipType)
		{
			return myBaseBranch.GetTipText(row + myFirstItem, column, tipType);
		}
		string IBranch.GetTipText(int row, int column, ToolTipType tipType)
		{
			return GetTipText(row, column, tipType);
		}
		protected static bool IsExpandable(int row, int column)
		{
			return false;
		}
		bool IBranch.IsExpandable(int row, int column)
		{
			return IsExpandable(row, column) ;
		}
		protected LocateObjectData LocateObject(object obj, ObjectStyle style, int locateOptions)
		{
			LocateObjectData data = myBaseBranch.LocateObject(obj, style, locateOptions);
			data.Row -= myFirstItem;
			return data;
		}
		LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
		{
			return LocateObject(obj, style, locateOptions);
		}
		protected event BranchModificationEventHandler OnBranchModification
		{
			add
			{
				myBaseBranch.OnBranchModification += value;
			}
			remove
			{
				myBaseBranch.OnBranchModification -= value;
			}
		}
		event BranchModificationEventHandler IBranch.OnBranchModification
		{
			add { OnBranchModification += value; }
			remove { OnBranchModification -= value; }
		}
		protected void OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
		{
			myBaseBranch.OnDragEvent(sender, row + myFirstItem, column, eventType, args);
		}
		void IBranch.OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
		{
			OnDragEvent(sender, row, column, eventType, args);
		}
		protected void OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
		{
			myBaseBranch.OnGiveFeedback(args, row + myFirstItem, column);
		}
		void IBranch.OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
		{
			OnGiveFeedback(args, row, column);
		}
		protected void OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
		{
			myBaseBranch.OnQueryContinueDrag(args, row + myFirstItem, column);
		}
		void IBranch.OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
		{
			OnQueryContinueDrag(args, row, column);
		}
		protected VirtualTreeStartDragData OnStartDrag(object sender, int row, int column, DragReason reason)
		{
			return myBaseBranch.OnStartDrag(sender, row + myFirstItem, column, reason);
		}
		VirtualTreeStartDragData IBranch.OnStartDrag(object sender, int row, int column, DragReason reason)
		{
			return OnStartDrag(sender, row, column, reason);
		}
		protected StateRefreshChanges SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
		{
			return myBaseBranch.SynchronizeState(row, column, matchBranch, matchRow + myFirstItem, matchColumn);
		}
		StateRefreshChanges IBranch.SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
		{
			return SynchronizeState(row, column, matchBranch, matchRow, matchColumn);
		}
		protected StateRefreshChanges ToggleState(int row, int column)
		{
			return myBaseBranch.ToggleState(row + myFirstItem, column);
		}
		StateRefreshChanges IBranch.ToggleState(int row, int column)
		{
			return ToggleState(row, column);
		}
		protected int UpdateCounter
		{
			get { return myBaseBranch.UpdateCounter; }
		}
		int IBranch.UpdateCounter
		{
			get { return UpdateCounter; }
		}
		protected int VisibleItemCount
		{
			get { return myCount; }
		}
		int IBranch.VisibleItemCount
		{
			get { return VisibleItemCount; }
		}
		#endregion
	}
}
