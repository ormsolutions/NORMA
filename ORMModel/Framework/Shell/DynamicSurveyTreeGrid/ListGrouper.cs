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
			private struct SubBranchMetaData
			{
				public int Start;
				public int End;
				public string Header;
				private IBranch myBranch;
				public int Count
				{
					get
					{
						return End - Start + 1;
					}
				}
				public IBranch Branch
				{
					get
					{
						return myBranch;
					}
				}
				public IBranch EnsureBranch(ListGrouper parent)
				{
					IBranch branch = myBranch;
					if (branch == null)
					{
						//Get parent QuestionList
						Survey questionList = parent.myQuestionList;
						//getIndex
						int questionIndex = questionList.GetIndex(parent.myQuestion.Question.QuestionType);
						if (questionIndex < questionList.Count - 1)
						{
							branch = new ListGrouper(parent.myBaseBranch, questionList[questionIndex + 1], Start, End, parent.myNeutralOnTop);
						}
						else
						{
							branch = new SimpleListShifter(parent.myBaseBranch, Start, Count);
						}
						myBranch = branch;

					}
					return branch;
				}
				#region Adjust Methods
				#region Delete
				/// <summary>
				/// Adjusts the delete.
				/// </summary>
				/// <param name="index">The index.</param>
				/// <param name="adjustIndex">Index of the adjust.</param>
				/// <param name="modificationEvents">The modification events.</param>
				/// <param name="notifyThrough">The notify through.</param>
				/// <param name="notifyThroughOffset">The notify through offset.</param>
				/// <returns></returns>
				public bool AdjustDelete(int index, int adjustIndex, BranchModificationEventHandler modificationEvents, IBranch notifyThrough, int notifyThroughOffset)
				{
					if (index <= End)
					{
						IBranch branch = myBranch;
						SimpleListShifter notifyListShifter = null;
						ListGrouper notifyListGrouper = null;
						if (branch != null)
						{
							if (null == (notifyListShifter = branch as SimpleListShifter))
							{
								notifyListGrouper = (ListGrouper)branch;
							}
						}
						if (index >= Start && index <= End)
						{    
							int startIndex = index;
							index -= Start;
							if (index < Count)
							{
								End += adjustIndex;

								if (End < Start)
								{
									myBranch = null;
									return true;
								}
								else if (branch != null)
								{
									if (notifyListShifter != null)
									{
										notifyListShifter.Count += adjustIndex;
										if (modificationEvents != null)
										{
											if (notifyThrough != null)
											{
												HandleNeutral(index, adjustIndex, modificationEvents, notifyThrough, notifyThroughOffset);
											}
											else
											{
												HandleSubranch(index, adjustIndex, modificationEvents, notifyListShifter);
											}
										}
									}
									else
									{
										notifyListGrouper.ElementDeletedAt(startIndex, modificationEvents, notifyThrough, notifyThroughOffset);
									}
								}
							}
							else
							{
								Start += adjustIndex;
								if (branch != null)
								{
									if (notifyListShifter != null)
									{
										notifyListShifter.FirstItem += adjustIndex;
									}
									else
									{
										notifyListGrouper.ElementDeletedAt(startIndex, modificationEvents, notifyThrough, notifyThroughOffset);
									}
								}
							}
						}
						else
						{
							Start += adjustIndex;
							End += adjustIndex;
							if (branch != null)
							{
								if (notifyListShifter != null)
								{
									notifyListShifter.FirstItem += adjustIndex;
								}
								else
								{
									notifyListGrouper.ElementDeletedAt(index, null, null, 0);
								}
							}
						}
					}

					return false;

				}
				#endregion
				#region AdjustAdd method

				/// <summary>
				/// Return true if the calling grouper should show a new header item
				/// </summary>
				public bool AdjustAdd(bool isChanged, bool afterChanged, int index, BranchModificationEventHandler modificationEvents, IBranch notifyThrough, int notifyThroughOffset)
				{
					if (index <= End || (isChanged && (index == End + 1)))
					{
						IBranch branch = myBranch;
						SimpleListShifter notifyListShifter = null;
						ListGrouper notifyListGrouper = null;
						if (branch != null)
						{
							if (null == (notifyListShifter = branch as SimpleListShifter))
							{
								notifyListGrouper = (ListGrouper)branch;
							}
						}
						if (isChanged)
						{
							int startIndex = index;
							index -= Start;
							End += 1;
							if (Start == End)
							{
								Debug.Assert(branch == null, "You should not have a branch with no children, remove in AdjustDelete");
								return true;
							}
							else if (branch != null)
							{
								if (notifyListShifter != null)
								{
									notifyListShifter.Count += 1;
									if (modificationEvents != null)
									{
										if (notifyThrough != null)
										{
											HandleNeutral(index, +1, modificationEvents, notifyThrough, notifyThroughOffset);
										}
										else
										{
											HandleSubranch(index, +1, modificationEvents, notifyListShifter);
										}
									}
								}
								else
								{
									notifyListGrouper.ElementAdded(startIndex, modificationEvents, notifyThrough, notifyThroughOffset);
								}
							}
						}
						else
						{
							Start += 1;
							End += 1;
							if (branch != null)
							{
								if (notifyListShifter != null)
								{
									notifyListShifter.FirstItem += 1;
								}
								else
								{
									notifyListGrouper.ElementAdded(index, null, null, 0);
								}
							}
						}
					}
					else if (afterChanged && index == (End + 1) && End < Start)
					{
						++Start;
						++End;
					}
					return false;
				}
				#endregion // AdjustAdd method
				#endregion
				#region  Helper Methods
				private static void HandleSubranch(int index, int adjustIndex, BranchModificationEventHandler modificationEvents, SimpleListShifter notifyListShifter)
				{
					if (adjustIndex == -1)
					{
						modificationEvents(notifyListShifter, BranchModificationEventArgs.DeleteItems(notifyListShifter, index, 1));

					}
					if (adjustIndex == 1)
					{
						modificationEvents(notifyListShifter, BranchModificationEventArgs.InsertItems(notifyListShifter, index - 1, 1));

					}
				}

				private static void HandleNeutral(int index, int adjustIndex, BranchModificationEventHandler modificationEvents, IBranch notifyThrough, int notifyThroughOffset)
				{
					if (adjustIndex == -1)
					{
						modificationEvents(notifyThrough, BranchModificationEventArgs.DeleteItems(notifyThrough, notifyThroughOffset + index, 1));
					}
					if (adjustIndex == 1)
					{
						modificationEvents(notifyThrough, BranchModificationEventArgs.DeleteItems(notifyThrough, notifyThroughOffset + index, 1));
					}
				}
				#endregion
			}
			#endregion //SubBranchMetaData struct
			#region row helper struct, enum, and method
			private enum RowType { Neutral, SubBranch }
			private RowType TranslateRow(ref int row)
			{
				if (myNeutralOnTop)
				{
					IBranch neutral = myNeutralBranch;
					int neutralCount = (neutral != null) ? neutral.VisibleItemCount : 0;
					if (row < neutralCount)
					{
						return RowType.Neutral;
					}
					row -= neutralCount;
				}
				int subBranchCount;
				if (row < (subBranchCount = myVisibleSubBranchCount))
				{
					row = mySubBranchOrder[row];
					return RowType.SubBranch;
				}
				Debug.Assert(!myNeutralOnTop);
				row -= subBranchCount;
				return RowType.Neutral;
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
			private readonly int[] mySubBranchOrder;
			private int myVisibleSubBranchCount;
			private IBranch myNeutralBranch;

			/// <summary>
			/// constructor for list grouper, neither parameter can be null
			/// </summary>
			/// <param name="baseBranch">the IBranch this grouper is going to sit on top of</param>
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
				int categoryCount = question.CategoryCount;
				mySubBranches = new SubBranchMetaData[categoryCount];
				mySubBranchOrder = new int[categoryCount];
				BuildMetaData(startIndex);
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
				List<SampleDataElementNode> nodes = ((MainList)myBaseBranch).myNodes;
				int index;
				int startNeutralIndex = index = startIndex;
				int neutralAnswer = myQuestion.Mask >> myQuestion.Shift;
				int currentAnswer = -1;
				SampleDataElementNode currentNode;
				int indexBound = myEndIndex + 1;
				for (; index < indexBound; ++index)
				{
					currentNode = nodes[index];
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
				int index = startIndex;
				SurveyQuestion question = myQuestion;
				int questionCount = question.CategoryCount;
				int neutralAnswer = myQuestion.Mask >> myQuestion.Shift;
				int lastAnswer;
				int currentAnswer = lastAnswer = -2;
				SubBranchMetaData[] subBranchData = mySubBranches;
				SubBranchMetaData currentSubBranch = default(SubBranchMetaData);
				int itemCount = myEndIndex + 1;
				int dataIndex;
				List<SampleDataElementNode> nodes = ((MainList)myBaseBranch).myNodes;
				for (; index < itemCount; ++index)
				{
					currentAnswer = question.ExtractAnswer(nodes[index].NodeData);
					if (currentAnswer == neutralAnswer)
					{
						break;
					}
					if (currentAnswer != lastAnswer)
					{
						if (lastAnswer < 0)
						{
							lastAnswer = -1;
						}
						else
						{
							currentSubBranch.End = index - 1;
							subBranchData[lastAnswer] = currentSubBranch;
						}
						for (dataIndex = lastAnswer + 1; dataIndex < currentAnswer; ++dataIndex)
						{
							currentSubBranch = subBranchData[dataIndex];
							currentSubBranch.Start = index;
							currentSubBranch.End = index - 1;
							currentSubBranch.Header = question.CategoryHeader(dataIndex);
							subBranchData[dataIndex] = currentSubBranch;
						}
						lastAnswer = currentAnswer;
						currentSubBranch = subBranchData[currentAnswer];
						currentSubBranch.Start = index;
						currentSubBranch.Header = question.CategoryHeader(currentAnswer);
					}
				}
				if (currentAnswer < 0)
				{
					currentAnswer = -1;
				}
				else
				{
					currentSubBranch.End = index - 1;
					subBranchData[currentAnswer] = currentSubBranch;
				}
				for (dataIndex = currentAnswer + 1; dataIndex < questionCount; ++dataIndex)
				{
					currentSubBranch = subBranchData[dataIndex];
					currentSubBranch.Start = index;
					currentSubBranch.End = index - 1;
					currentSubBranch.Header = question.CategoryHeader(dataIndex);
					subBranchData[dataIndex] = currentSubBranch;
				}

				int[] orderArray = mySubBranchOrder;
				for (int i = 0; i < questionCount; ++i)
				{
					orderArray[i] = i;
				}
				Array.Sort<int>(
					orderArray,
					delegate(int left, int right)
					{
						if (left == right)
						{
							return 0;
						}
						int retVal = 0;
						if (subBranchData[left].Count == 0)
						{
							if (subBranchData[right].Count != 0)
							{
								retVal = 1;
							}
						}
						else if (subBranchData[right].Count == 0)
						{
							retVal = -1;
						}
						if (retVal == 0)
						{
							retVal = (left < right) ? -1 : 1;
						}
						return retVal;
					});

				myVisibleSubBranchCount = 0;
				for (int i = 0; i < questionCount; ++i)
				{
					if (subBranchData[i].Count != 0)
					{
						++myVisibleSubBranchCount;
					}
				}
				return index < myEndIndex ? index : myEndIndex;
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
					return BranchFeatures.Expansions | BranchFeatures.BranchRelocation | BranchFeatures.InsertsAndDeletes | BranchFeatures.PositionTracking;
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
			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				switch (TranslateRow(ref row))
				{
					case RowType.Neutral:
						return myNeutralBranch.GetObject(row, column, style, ref options);
					case RowType.SubBranch:
						if (style == ObjectStyle.ExpandedBranch)
						{
							return mySubBranches[row].EnsureBranch(this);
						}
						break;
				}
				return null;
			}

			public string GetText(int row, int column)
			{
				switch (TranslateRow(ref row))
				{
					case RowType.Neutral:
						return myNeutralBranch.GetText(row, column);
					case RowType.SubBranch:
						return mySubBranches[row].Header;
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
				switch (TranslateRow(ref row))
				{
					case RowType.Neutral:
						return myNeutralBranch.IsExpandable(row, column);
					case RowType.SubBranch:
						return mySubBranches[row].Count > 0;
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
			#region Element Deleted At
			/// <summary>
			/// Deletes node at given index and adjusts indices
			/// </summary>
			/// <param name="index">index of the node that has been deleted</param>
			/// <param name="modificationEvents">The event handler to notify the tree with</param>
			public void ElementDeletedAt(int index, BranchModificationEventHandler modificationEvents)
			{
				ElementDeletedAt(index, modificationEvents, null, 0);
			}
			/// <summary>
			/// Deletes node at given index and adjusts indices
			/// </summary>
			/// <param name="index">index of the node that has been deleted</param>
			/// <param name="modificationEvents">The event handler to notify the tree with</param>
			/// <param name="notifyThrough">A wrapper branch. Notify the event handler with this branch, not the current branch</param>
			/// <param name="notifyThroughOffset">Used if notifyThrough is not null. The starting offset of this branch in the outer branch.</param>
			private void ElementDeletedAt(int index, BranchModificationEventHandler modificationEvents, IBranch notifyThrough, int notifyThroughOffset)
			{
				MainList baseBranch = ((MainList)this.myBaseBranch);
				if (myNeutralOnTop)
				{
					Debug.Assert(myNeutralBranch == null, "Neutral adjustment not handled");
					#region currently not implemented Neutral handling
					//ElementDeletedAtNeutral();
					//IBranch neutralBranch = myNeutralBranch;
					//if (neutralBranch != null)
					//{
					//    SimpleListShifter neutralShifter;
					//    ListGrouper neutralGrouper;
					//    if (null != (neutralShifter = myNeutralBranch as SimpleListShifter))
					//    {
					//    }
					//    else if (null != (neutralGrouper = myNeutralBranch as ListGrouper))
					//    {
					//        neutralGrouper.ElementDeletedAt
					//    }
					//} 
					#endregion
				}
				for (int i = 0; i < this.mySubBranches.Length; i++)
				{
					if (mySubBranches[i].AdjustDelete(index, -1, modificationEvents, notifyThrough, notifyThroughOffset))
					{
						int currentHeaderCount = myVisibleSubBranchCount;
						int[] orderArray = mySubBranchOrder;
						for (int deleteAt = 0; deleteAt < currentHeaderCount; ++deleteAt)
						{
							if (orderArray[deleteAt] == i)
							{
								for (int j = deleteAt; j < currentHeaderCount - 1; ++j)
								{
									orderArray[j] = orderArray[j + 1];
								}
								orderArray[currentHeaderCount - 1] = i;
								--myVisibleSubBranchCount;
								if (modificationEvents != null)
								{
									if (notifyThrough != null)
									{
										modificationEvents(this, BranchModificationEventArgs.DeleteItems(notifyThrough, notifyThroughOffset + deleteAt, 1));
									}
									else
									{
										modificationEvents(this, BranchModificationEventArgs.DeleteItems(this, deleteAt, 1));
									}
								}
								break;
							}
						}
					}
				}
				if (!myNeutralOnTop)
				{
					Debug.Assert(myNeutralBranch == null, "Neutral adjustment not handled");
					// UNDONE: Handle trailing neutral
				}
			}
			#endregion
			#region ElementAdded
			public void ElementAdded(int index, BranchModificationEventHandler ModificationEvents)
			{
				ElementAdded(index, ModificationEvents, null, 0);
			}
			private void ElementAdded(int index, BranchModificationEventHandler modificationEvents, IBranch notifyThrough, int notifyThroughOffset)
			{
				int currentAnswer = myQuestion.ExtractAnswer(((MainList)myBaseBranch).myNodes[index].NodeData);
				if (myNeutralOnTop)
				{
					Debug.Assert(myNeutralBranch == null, "Neutral adjustment not handled");
					#region handle neutral, currently not implemented
					//ElementDeletedAtNeutral();
					//IBranch neutralBranch = myNeutralBranch;
					//if (neutralBranch != null)
					//{
					//    SimpleListShifter neutralShifter;
					//    ListGrouper neutralGrouper;
					//    if (null != (neutralShifter = myNeutralBranch as SimpleListShifter))
					//    {
					//    }
					//    else if (null != (neutralGrouper = myNeutralBranch as ListGrouper))
					//    {
					//        neutralGrouper.ElementDeletedAt
					//    }
					//} 
					#endregion
				}
				for (int i = 0; i < this.mySubBranches.Length; i++)
				{
					if (mySubBranches[i].AdjustAdd(i == currentAnswer, i > currentAnswer, index, modificationEvents, notifyThrough, notifyThroughOffset))
					{
						// We need to add the header row
						int currentHeaderCount = myVisibleSubBranchCount;
						// UNDONE: This will get more complicated when the order array has
						// a more complicated sort
						int[] orderArray = mySubBranchOrder;
						for (int j = currentHeaderCount; j < orderArray.Length; ++j)
						{
							if (orderArray[j] == i)
							{
								int insertAt = 0;
								for (int k = 0; k < currentHeaderCount; ++k)
								{
									if ((k == 0 && i < orderArray[k]) ||
										(i < orderArray[k] && i > orderArray[k - 1]))
									{
										break;
									}
									++insertAt;
								}
								for (int k = j - 1; k >= insertAt; --k)
								{
									orderArray[k + 1] = orderArray[k];
								}
								orderArray[insertAt] = i;
								++myVisibleSubBranchCount;
								if (modificationEvents != null)
								{
									if (notifyThrough != null)
									{
										modificationEvents(this, BranchModificationEventArgs.InsertItems(notifyThrough, notifyThroughOffset + insertAt - 1, 1));
									}
									else
									{
										modificationEvents(this, BranchModificationEventArgs.InsertItems(this, insertAt - 1, 1));
									}
								}
								break;
							}
						}
					}
				}
				if (!myNeutralOnTop)
				{
					Debug.Assert(myNeutralBranch == null, "Neutral adjustment not handled");
					// UNDONE: Handle trailing neutral
				}
			}
			#endregion
			#region ElemetRenamedAt
			/// <summary>
			/// Renames the node at given index and redraws the tree
			/// </summary>
			/// <param name="from">From.</param>
			/// <param name="to">To.</param>
			/// <param name="myModificationEvents">My modification events.</param>
			public void ElementRenamedAt(int from, int to, BranchModificationEventHandler myModificationEvents)
			{
				SimpleListShifter shifter = (SimpleListShifter)GetModifiedBranch(from, this);
					if (from != to)
					{
						if (shifter != null)
						{
							myModificationEvents(this, BranchModificationEventArgs.MoveItem(shifter, from, to));
						}
					}
					else
					{
						if (shifter != null)
						{
							myModificationEvents(this, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(shifter)));
						}
					}
				
			}
			private IBranch GetModifiedBranch(int index, IBranch branch)
			{
				SimpleListShifter shifter;
				ListGrouper grouper;
				if (null != (grouper = branch as ListGrouper))
				{
					for (int i = 0; i < grouper.mySubBranches.Length; i++)
					{
						if (grouper.mySubBranches[i].Start <= index && grouper.mySubBranches[i].End >= index)
						{
							IBranch subranch = mySubBranches[i].Branch;
							return GetModifiedBranch(index, subranch);
						}
					}
				}
				if (null != (shifter = branch as SimpleListShifter))
				{
					return shifter;
				}
				return null;
			}
			#endregion
		}

	}
}
