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
	partial class MainList
	{
		private sealed partial class ListGrouper : IBranch
		{
			#region SubBranchMetaData struct
			private struct SubBranchMetaData : IComparable<SubBranchMetaData>
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
				public int CompareTo(SubBranchMetaData other)
				{
					if (!IsSet)
					{
						return other.IsSet ? 1 : 0;
					}
					return other.Count - this.Count;
				}
			}
			#endregion //SubBranchMetaData struct

			#region row helper struct, enum, and method
			private enum RowType { Neutral, SubBranch }
			private struct RowHelper
			{
				public readonly RowType Type;
				public readonly int Offset;
				public RowHelper(RowType type, int offset)
				{
					this.Type = type;
					this.Offset = offset;
				}
			}
			private RowHelper BuildRowHelper(int row)
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
			private readonly IBranch myBaseBranch;
			private readonly SurveyQuestion myQuestion;
			private readonly Survey myQuestionList;
			private readonly int myStartIndex;
			private readonly int myEndIndex;
			private readonly bool myNeutralOnTop;
			private readonly SubBranchMetaData[] mySubBranches;
			private int myVisibleSubBranchCount;
			private IBranch myNeutralBranch;
			private IBranch myNonNeutralBranch;
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
				BuildMetaData(myStartIndex);
			}
			#endregion //constructor and variable declaration

			#region tree creation
			/// <summary>
			/// helper method to call handling neutral and building the meta data in order based on whether
			/// the list was sorted with neutral on top
			/// </summary>
			/// <param name="startIndex"></param>
			private void BuildMetaData(int startIndex)
			{
				if (myNeutralOnTop)
				{
					CreateSubBranchMetaData(HandleNeutral(startIndex));
				}
				else
				{
					HandleNeutral(CreateSubBranchMetaData(startIndex));
				}
			}
			/// <summary>
			/// Find index range for neutral answers to myQuestion and place them in either a SimpleListShifter or ListGrouper depending on whether 
			/// or not myQuestion is the last question in the survey.
			/// </summary>
			/// <param name="startIndex"></param>
			/// <returns>either startIndex that was passed if no neutrals were found or the index after the last neutral node</returns>
			private int HandleNeutral(int startIndex)
			{
				int options = 0;
				int index;
				int startNeutralIndex = index = startIndex;
				int neutralAnswer = myQuestion.Mask >> myQuestion.Shift;
				int currentAnswer = -1;
				SampleDataElementNode currentNode;
				for (; index < myEndIndex + 1; ++index)
				{
					currentNode = (SampleDataElementNode)myBaseBranch.GetObject(index, 0, ObjectStyle.TrackingObject, ref options);
					currentAnswer = myQuestion.ExtractAnswer(currentNode.NodeData);
					if (currentAnswer != neutralAnswer)
					{
						break;
					}
				}
				if (--index >= startNeutralIndex)
				{
					int questionIndex = myQuestionList.GetIndex(myQuestion.Question.QuestionType);
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
			private int CreateSubBranchMetaData(int startIndex)
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
			private IBranch CreateSubBranch(SubBranchMetaData branchData)
			{
				IBranch subBranch;
				int questionIndex = myQuestionList.GetIndex(myQuestion.Question.QuestionType);
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

			public VirtualTreeLabelEditData BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				return VirtualTreeLabelEditData.Invalid;
			}
			public LabelEditResult CommitLabelEdit(int row, int column, string newText)
			{
				return LabelEditResult.CancelEdit;
			}
			public BranchFeatures Features
			{
				get
				{
					return BranchFeatures.Expansions | BranchFeatures.BranchRelocation | BranchFeatures.PositionTracking;
				}
			}
			public VirtualTreeAccessibilityData GetAccessibilityData(int row, int column)
			{
				return VirtualTreeAccessibilityData.Empty;
			}
			public VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				return VirtualTreeDisplayData.Empty;
			}
			/// <summary>
			/// called when a node of the current branch is expanded to display the other branches or nodes it contains.
			/// </summary>
			public object GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				RowHelper rh = BuildRowHelper(row);
				switch (rh.Type)
				{
					case RowType.Neutral:
						return myNeutralBranch.GetObject(row - rh.Offset, column, style, ref options);
					case RowType.SubBranch:
						return CreateSubBranch(mySubBranches[row - rh.Offset]);
					default:
						return null;
				}
			}
			public string GetText(int row, int column)
			{
				RowHelper rh = BuildRowHelper(row);
				switch (rh.Type)
				{
					case RowType.Neutral:
						return myNeutralBranch.GetText(row - rh.Offset, column);
					case RowType.SubBranch:
						return mySubBranches[row - rh.Offset].Header;
					default:
						return string.Empty;
				}
			}
			public string GetTipText(int row, int column, ToolTipType tipType)
			{
				return string.Empty;
			}
			public bool IsExpandable(int row, int column)
			{
				RowHelper rh = BuildRowHelper(row);
				switch (rh.Type)
				{
					case RowType.Neutral:
						return myNeutralBranch.IsExpandable(row - rh.Offset, column);
					case RowType.SubBranch:
						return mySubBranches[row - rh.Offset].Count > 0;
					default:
						return false;
				}
			}
			public LocateObjectData LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				//TODO: ask matt about locating objects when I'm not storing references to my subbranches that are created
				return myBaseBranch.LocateObject(obj, style, locateOptions);
			}
			public event BranchModificationEventHandler OnBranchModification
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
			public void OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
			{
			}
			public void OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
			{
			}
			public void OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
			{
			}
			public VirtualTreeStartDragData OnStartDrag(object sender, int row, int column, DragReason reason)
			{
				return VirtualTreeStartDragData.Empty;
			}
			public StateRefreshChanges SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
			{
				return StateRefreshChanges.None;
			}
			public StateRefreshChanges ToggleState(int row, int column)
			{
				return StateRefreshChanges.None;
			}

			public int UpdateCounter
			{
				get
				{
					return 0;
				}
			}

			public int VisibleItemCount
			{
				get
				{
					IBranch neutral = myNeutralBranch;
					return ((neutral != null) ? neutral.VisibleItemCount : 0) + myVisibleSubBranchCount;
				}
			}
			#endregion
		}
	}
}
