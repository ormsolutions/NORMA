using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.VirtualTreeGrid;
using System.Diagnostics;

namespace Neumont.Tools.ORM.Framework.DynamicSurveyTreeGrid
{
	class ListGrouper :	IBranch
	{
		#region SubBranchMetaData 
		private struct SubBranchMetaData : IComparable
		{
			public int Start;
			public int End;
			public bool IsSet;
			public string Header;
			public int Count 
			{ 
				get 
				{
					if (!IsSet)
					{
						return 0;
					}
					return End - Start + 1; 
				} 
			}
			#region IComparable Members
			public int CompareTo(object obj)
			{
				Debug.Assert(obj is SubBranchMetaData);
				SubBranchMetaData compare = (SubBranchMetaData)obj;
				if (!IsSet)
				{
					return compare.IsSet ? 1 : 0;
				}
				return compare.Count - this.Count;
			}
			#endregion
		}
		#endregion //SubBranchMetaData
		#region row helper struct, enum, and method
		private enum RowType { Neutral, SubBranch }
		private struct RowHelper
		{
			public RowType Type;
			public int Offset;
			public RowHelper(RowType type, int offset)
			{
				this.Type = type;
				this.Offset = offset;
			}
		}
		private RowHelper buildRowHelper(int row)
		{
			if (myNeutralOnTop)
			{
				IBranch neutral = myNeutralBranch;
				int neutralCount = (neutral != null) ? neutral.VisibleItemCount : 0;
				if (row < neutralCount)
				{
					return new RowHelper(RowType.Neutral, 0);
				}
				else
				{
					return new RowHelper(RowType.SubBranch, neutralCount);
				}
			}
			if (row < myVisibleSubBranchCount)
			{
				return new RowHelper(RowType.SubBranch, 0);
			}
			return new RowHelper(RowType.Neutral, myVisibleSubBranchCount);
		}
		#endregion //row helper struct, enum, and method
		#region constructor and variable declaration
		IBranch myBaseBranch;
		SurveyQuestion myQuestion;
		Survey myQuestionList;
		int myStartIndex;
		int myEndIndex;
		int myVisibleSubBranchCount;
		bool myNeutralOnTop;
		IBranch myNeutralBranch;
		IBranch myNonNeutralBranch;
		SubBranchMetaData[] mySubBranches;
		/// <summary>
		/// constructor for list grouper, neither parameter can be null
		/// </summary>
		/// <param name="baseBranch">the Ibranch this grouper is going to sit on top of</param>
		/// <param name="question">question to be applied by this grouper</param>
		/// <param name="startIndex"></param>
		/// <param name="endIndex"></param>
		/// <param name="neutralOnTop"></param>
		public ListGrouper(IBranch baseBranch, SurveyQuestion question, int startIndex, int endIndex, bool neutralOnTop)
		{
			Debug.Assert(baseBranch != null);
			myBaseBranch = baseBranch;
			myQuestion = question;
			myQuestionList = myQuestion.QuestionList;
			Debug.Assert(myQuestionList != null); //questions should only be accessible through a survey in which case their question list must be set
			myStartIndex = startIndex;
			myEndIndex = endIndex;
			myVisibleSubBranchCount = 0;
			myNeutralOnTop = neutralOnTop;
			mySubBranches = new SubBranchMetaData[myQuestion.CategoryCount];
			buildMetaData(myStartIndex);
		}		
		/// <summary>
		/// setter method is currently used to determine the subbranches of each node and how to group
		/// the nodes of the underlying main list
		/// </summary>
		public IBranch BaseBranch
		{
			get { return myBaseBranch; }
		}
		#endregion //constructor and variable declaration
		#region tree creation
		/// <summary>
		/// helper method to call handling neutral and building the meta data in order based on whether
		/// the list was sorted with neutral on top
		/// </summary>
		/// <param name="startIndex"></param>
		private void buildMetaData(int startIndex)
		{
			if (myNeutralOnTop)
			{
				createSubBranchMetaData(handleNeutral(startIndex));
			}
			else
			{
				handleNeutral(createSubBranchMetaData(startIndex));
			}
		}
		/// <summary>
		/// Find index range for neutral answers to myQuestion and place them in either a SimpleListShifter or ListGrouper depending on whether 
		/// or not myQuestion is the last question in the survey.
		/// </summary>
		/// <param name="startIndex"></param>
		/// <returns>either startIndex that was passed if no neutrals were found or the index after the last neutral node</returns>
		private int handleNeutral(int startIndex)
		{
			int options = 0;
			int index;
			int startNeutralIndex = index = startIndex;
			int neutralAnswer = myQuestion.Mask >> myQuestion.Shift;
			int currentAnswer = -1;
			SampleDataElementNode currentNode;
			for(; index < myEndIndex + 1; ++index)
			{
				currentNode = (SampleDataElementNode)myBaseBranch.GetObject(index, 0, ObjectStyle.TrackingObject, ref options);
				currentAnswer = myQuestion.ExtractAnswer(currentNode.NodeData);
				if(currentAnswer != neutralAnswer)
				{
					break;
				}
			}
			if(--index >= startNeutralIndex)
			{
				int questionIndex = myQuestionList.getIndex(myQuestion.Question.QuestionType);
				if (questionIndex == myQuestionList.Count - 1)
				{
					myNeutralBranch = new SimpleListShifter(myBaseBranch, startNeutralIndex, index - startNeutralIndex + 1);
				}
				else
				{
					//TODO: use neutralOnTop bool stored with question instead of passing one in
					myNeutralBranch = new ListGrouper(myBaseBranch, myQuestionList[questionIndex + 1], startNeutralIndex, index, myNeutralOnTop);
				}
				return ++index;
			}
			else
			{
				return startNeutralIndex;
			}
		}
		/// <summary>
		/// build meta data for sub branches
		/// </summary>
		/// <param name="startIndex"></param>
		/// <returns>index that the method started with if there are no sub branches, otherwise the next index after the one the method completed on</returns>
		private int createSubBranchMetaData(int startIndex)
		{
			int options = 0;
			int index = startIndex;
			int neutralAnswer = myQuestion.Mask >> myQuestion.Shift;
			int lastAnswer;
			int currentAnswer = lastAnswer = -2;
			SampleDataElementNode currentNode;
			SubBranchMetaData currentMetaData;
			for (; index < myEndIndex + 1; ++index)
			{
				currentNode = (SampleDataElementNode)myBaseBranch.GetObject(index, 0, ObjectStyle.TrackingObject, ref options);
				currentAnswer = myQuestion.ExtractAnswer(currentNode.NodeData);
				if (currentAnswer == neutralAnswer)
				{
					break;
				}
				currentMetaData = mySubBranches[currentAnswer];
				if (currentAnswer != lastAnswer)
				{
					lastAnswer = currentAnswer;
					currentMetaData.Start = index;
					currentMetaData.Header = myQuestion.CategoryHeader(currentAnswer);
				}
				currentMetaData.End = index;
				currentMetaData.IsSet = true;
				mySubBranches[currentAnswer] = currentMetaData;
			}
			Array.Sort(mySubBranches);
			myVisibleSubBranchCount = 0;
			for (int i = 0; i < mySubBranches.Length; ++i)
			{
				if (mySubBranches[i].Count > 0)
				{
					++myVisibleSubBranchCount;
				}
			}
			return index < myEndIndex ? index : myEndIndex;
		}
		private IBranch createSubBranch(SubBranchMetaData branchData)
		{
			IBranch subBranch;
			int questionIndex = myQuestionList.getIndex(myQuestion.Question.QuestionType);
			if (questionIndex < myQuestionList.Count - 1)
			{
				subBranch = new ListGrouper(myBaseBranch, myQuestionList[questionIndex + 1], branchData.Start, branchData.End, myNeutralOnTop);
			}
			else
			{
				subBranch = new SimpleListShifter(myBaseBranch, branchData.Start, branchData.Count);
			}
			myNonNeutralBranch = subBranch;
			return subBranch;
		}
		#endregion //tree creation
		#region IBranch Members

		protected static VirtualTreeLabelEditData BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
		{
			return VirtualTreeLabelEditData.Invalid;
		}
		VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
		{
			return BeginLabelEdit(row, column, activationStyle);
		}
		protected static LabelEditResult CommitLabelEdit(int row, int column, string newText)
		{
			return LabelEditResult.CancelEdit;
		}
		LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
		{
			return CommitLabelEdit(row, column, newText);
		}
		protected static BranchFeatures Features
		{ 
			get { return BranchFeatures.Expansions | BranchFeatures.BranchRelocation | BranchFeatures.PositionTracking; }
		}
		BranchFeatures IBranch.Features
		{
			get { return Features; }
		}
		protected static VirtualTreeAccessibilityData GetAccessibilityData(int row, int column)
		{
			return VirtualTreeAccessibilityData.Empty;
		}
		VirtualTreeAccessibilityData IBranch.GetAccessibilityData(int row, int column)
		{
			return GetAccessibilityData(row, column);
		}
		protected static VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
		{
			return VirtualTreeDisplayData.Empty;
		}
		VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
		{
			return VirtualTreeDisplayData.Empty;
		}
		/// <summary>
		/// called when a node of the current branch is expanded to display the other branches or nodes it contains.
		/// </summary>
		protected object GetObject(int row, int column, ObjectStyle style, ref int options)
		{
			RowHelper rh = buildRowHelper(row);
			switch (rh.Type)
			{
				case RowType.Neutral: return myNeutralBranch.GetObject(row - rh.Offset, column, style, ref options);
				case RowType.SubBranch: return createSubBranch(mySubBranches[row - rh.Offset]);
				default: return null;
			}
		}
		object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
		{
			return GetObject(row, column, style, ref options);
		}
		protected string GetText(int row, int column)
		{
			RowHelper rh = buildRowHelper(row);
			switch (rh.Type)
			{
				case RowType.Neutral: return myNeutralBranch.GetText(row - rh.Offset, column);
				case RowType.SubBranch: return mySubBranches[row - rh.Offset].Header;
				default: return "";
			}
		}
		string IBranch.GetText(int row, int column)
		{
			return GetText(row, column);
		}
		protected static string GetTipText(int row, int column, ToolTipType tipType)
		{
			return "";
		}
		string IBranch.GetTipText(int row, int column, ToolTipType tipType)
		{
			return GetTipText(row, column, tipType);
		}
		protected bool IsExpandable(int row, int column)
		{
			RowHelper rh = buildRowHelper(row);
			switch (rh.Type)
			{
				case RowType.Neutral: return myNeutralBranch.IsExpandable(row - rh.Offset, column);
				case RowType.SubBranch: return mySubBranches[row - rh.Offset].Count > 0;
				default: return false;
			}
		}
		bool IBranch.IsExpandable(int row, int column)
		{
			return IsExpandable(row, column);
		}
		protected LocateObjectData LocateObject(object obj, ObjectStyle style, int locateOptions)
		{
			//TODO: ask matt about locating objects when I'm not storing references to my subbranches that are created
			return myBaseBranch.LocateObject(obj, style, locateOptions);
		}
		LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
		{
			return LocateObject(obj, style, locateOptions);
		}
		protected event BranchModificationEventHandler OnBranchModification
		{
			add { myBaseBranch.OnBranchModification += value; }
			remove { myBaseBranch.OnBranchModification -= value; }
		}
		event BranchModificationEventHandler IBranch.OnBranchModification
		{
			add { OnBranchModification += value; }
			remove { OnBranchModification -= value; }
		}
		protected static void OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
		{		}
		void IBranch.OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
		{
			OnDragEvent(sender, row, column, eventType, args);
		}
		protected static void OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
		{ }
		void IBranch.OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
		{
			OnGiveFeedback(args, row, column);
		}
		protected static void OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
		{}
		void IBranch.OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
		{
			OnQueryContinueDrag(args, row, column);
		}
		protected static VirtualTreeStartDragData OnStartDrag(object sender, int row, int column, DragReason reason)
		{
			return VirtualTreeStartDragData.Empty;
		}
		VirtualTreeStartDragData IBranch.OnStartDrag(object sender, int row, int column, DragReason reason)
		{
			return OnStartDrag(sender, row, column, reason);
		}

		protected static StateRefreshChanges SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
		{
			return StateRefreshChanges.None;
		}
		StateRefreshChanges IBranch.SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
		{
			return SynchronizeState(row, column, matchBranch, matchRow, matchColumn);
		}
		protected static StateRefreshChanges ToggleState(int row, int column)
		{
			return StateRefreshChanges.None;
		}
		StateRefreshChanges IBranch.ToggleState(int row, int column)
		{
			return ToggleState(row, column);
		}

		protected static int UpdateCounter
		{
			get { return 0; }
		}
		int IBranch.UpdateCounter
		{
			get { return UpdateCounter; }
		}

		protected int VisibleItemCount
		{	
			get 
			{
				IBranch neutral = myNeutralBranch;
				return ((neutral != null) ? neutral.VisibleItemCount : 0) + myVisibleSubBranchCount;
			}
		}
		int IBranch.VisibleItemCount
		{
			get
			{
				return VisibleItemCount;
			}
		}
		#endregion
	}
}
