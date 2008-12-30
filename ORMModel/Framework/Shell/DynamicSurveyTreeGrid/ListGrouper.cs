#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
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
					public IBranch EnsureBranch(ListGrouper grouper)
					{
						IBranch branch = myBranch;
						if (branch == null)
						{
							//Get parent QuestionList
							Survey questionList = grouper.mySurvey;
							//getIndex
							int questionIndex = questionList.GetIndex(grouper.myQuestion.Question.QuestionType);
							if (questionIndex < questionList.Count - 1)
							{
								SurveyQuestion nextQuestion = questionList[questionIndex + 1];
								if ((0 != (nextQuestion.UISupport & (SurveyQuestionUISupport.Grouping))))
								{
									branch = new ListGrouper(grouper.myBaseBranch, nextQuestion, Start, End, grouper.myNeutralOnTop);
								}
								else
								{
									branch = new SimpleListShifter(grouper.myBaseBranch, Start, Count);
								}
							}
							else
							{
								branch = new SimpleListShifter(grouper.myBaseBranch, Start, Count);
							}
							myBranch = branch;
						}
						return branch;
					}
					/// <summary>
					/// Returns true if the index is in range for this subbranch
					/// </summary>
					public bool IsInRange(int index)
					{
						return Start <= index && End >= index;
					}
					#region Adjust Methods
					#region AdjustDelete method
					/// <summary>
					/// Adjusts the delete.
					/// </summary>
					/// <param name="index">The index.</param>
					/// <param name="adjustIndex">Index of the adjust.</param>
					/// <param name="modificationEvents">The modification events.</param>
					/// <returns></returns>
					public bool AdjustDelete(int index, int adjustIndex, BranchModificationEventHandler modificationEvents)
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
												HandleSubranch(index, adjustIndex, modificationEvents, notifyListShifter);
											}
										}
										else
										{
											notifyListGrouper.ElementDeletedAt(startIndex, modificationEvents, null, 0);
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
											notifyListGrouper.ElementDeletedAt(startIndex, modificationEvents, null, 0);
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
					#endregion // AdjustDelete method
					#region AdjustAdd method
					/// <summary>
					/// Return true if the calling grouper should show a new header item
					/// </summary>
					public bool AdjustAdd(bool isChanged, bool afterChanged, int index, BranchModificationEventHandler modificationEvents)
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
											HandleSubranch(index, +1, modificationEvents, notifyListShifter);
										}
									}
									else
									{
										notifyListGrouper.ElementAddedAt(startIndex, true, modificationEvents, null, 0);
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
										notifyListGrouper.ElementAddedAt(index, false, null, null, 0);
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
					#region AdjustModified method
					public void AdjustModified(int fromIndex, int toIndex, bool displayChanged, BranchModificationEventHandler modificationEvents)
					{
						if (modificationEvents != null)
						{
							if (Start <= fromIndex && End >= fromIndex)
							{
								Debug.Assert(Start <= toIndex && End >= toIndex, "Renaming a survey node should never cause it to switch groups");
								IBranch branch = myBranch;
								if (branch != null)
								{
									SimpleListShifter shifter;
									ListGrouper grouper;
									if (null != (shifter = branch as SimpleListShifter))
									{
										fromIndex -= Start;
										toIndex -= Start;
										if (fromIndex != toIndex)
										{
											modificationEvents(shifter, BranchModificationEventArgs.MoveItem(shifter, fromIndex, toIndex));
										}
										else if (displayChanged)
										{
											modificationEvents(shifter, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.Text, shifter, fromIndex, 0, 1)));
										}
									}
									else if (null != (grouper = branch as ListGrouper))
									{
										grouper.ElementModifiedAt(fromIndex, toIndex, displayChanged, modificationEvents, null, 0);
									}
								}

							}
						}
					}
					#endregion // AdjustModified method
					#endregion // Adjust Methods
					#region  Helper Methods
					private static void HandleSubranch(int index, int adjustIndex, BranchModificationEventHandler modificationEvents, SimpleListShifter notifyListShifter)
					{
						if (adjustIndex == -1)
						{
							modificationEvents(notifyListShifter, BranchModificationEventArgs.DeleteItems(notifyListShifter, index, 1));

						}
						else if (adjustIndex == 1)
						{
							modificationEvents(notifyListShifter, BranchModificationEventArgs.InsertItems(notifyListShifter, index - 1, 1));

						}
					}
					#endregion
					#region AdjustChange
					public void AdjustChange(int index, BranchModificationEventHandler modificationEvents)
					{
						if (Start <= index && End >= index)
						{
							index -= Start;
							IBranch branch = myBranch;
							if (branch != null)
							{
								SimpleListShifter shifter;
								ListGrouper grouper;
								if (null != (shifter = branch as SimpleListShifter))
								{
									modificationEvents(shifter, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.VisibleElements, shifter, index, 0, 1)));
								}
								else if (null != (grouper = branch as ListGrouper))
								{
									grouper.ElementChangedAt(index, modificationEvents, null, 0);
								}
							}
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
				private readonly Survey mySurvey;
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
					mySurvey = myQuestion.QuestionList;
					Debug.Assert(mySurvey != null); //questions should only be accessible through a survey in which case their question list must be set
					myVisibleSubBranchCount = 0;
					myNeutralOnTop = neutralOnTop;
					int categoryCount = question.CategoryCount;
					mySubBranches = new SubBranchMetaData[categoryCount];
					mySubBranchOrder = new int[categoryCount];
					BuildMetaData(startIndex, endIndex);
				}
				#endregion //constructor and variable declaration
				#region tree creation
				/// <summary>
				/// helper method to call handling neutral and building the meta data in order based on whether
				/// the list was sorted with neutral on top
				/// </summary>
				private void BuildMetaData(int startIndex, int endIndex)
				{
					if (myNeutralOnTop)
					{
						CreateSubBranchMetaData(CreateNeutralMetaData(startIndex, endIndex), endIndex);
					}
					else
					{
						CreateNeutralMetaData(CreateSubBranchMetaData(startIndex, endIndex), endIndex);
					}
				}
				/// <summary>
				/// Find index range for neutral answers to myQuestion and place them in either a SimpleListShifter or ListGrouper depending on whether 
				/// or not myQuestion is the last question in the survey.
				/// </summary>
				/// <param name="startIndex"></param>
				/// <param name="endIndex"></param>
				/// <returns>either startIndex that was passed if no neutrals were found or the index after the last neutral node</returns>
				private int CreateNeutralMetaData(int startIndex, int endIndex)
				{
					List<SampleDataElementNode> nodes = ((MainList)myBaseBranch).myNodes;
					int index;
					int startNeutralIndex = index = startIndex;
					int currentAnswer = -1;
					SampleDataElementNode currentNode;
					int indexBound = endIndex + 1;
					for (; index < indexBound; ++index)
					{
						currentNode = nodes[index];
						currentAnswer = myQuestion.ExtractAnswer(currentNode.NodeData);
						if (currentAnswer != SurveyQuestion.NeutralAnswer)
						{
							break;
						}
					}
					if (--index >= startNeutralIndex)
					{
						SurveyQuestion nextQuestion = NextGroupableQuestion;
						myNeutralBranch = (nextQuestion != null) ?
							//TODO: use neutralOnTop bool stored with question instead of passing one in
							(IBranch)new ListGrouper(myBaseBranch, nextQuestion, startNeutralIndex, index, myNeutralOnTop) :
							new SimpleListShifter(myBaseBranch, startNeutralIndex, index - startNeutralIndex + 1);
						return ++index;
					}
					else
					{
						return startNeutralIndex;
					}
				}
				/// <summary>
				/// Return the first groupable <see cref="SurveyQuestion"/> after
				/// the current question in the context <see cref="Survey"/>.
				/// </summary>
				private SurveyQuestion NextGroupableQuestion
				{
					get
					{
						Survey survey = mySurvey;
						int contextQuestionIndex = survey.GetIndex(myQuestion.Question.QuestionType);
						int questionCount = survey.Count;
						for (int i = contextQuestionIndex + 1; i < questionCount; ++i)
						{
							// UNDONE: This should come from the display information, not the question
							// itself.
							if (0 != (survey[i].UISupport & SurveyQuestionUISupport.Grouping))
							{
								return survey[i];
							}
						}
						return null;
					}
				}
				/// <summary>
				/// build meta data for sub branches
				/// </summary>
				/// <param name="startIndex"></param>
				/// <param name="endIndex"></param>
				/// <returns>index that the method started with if there are no sub branches, otherwise the next index after the one the method completed on</returns>
				private int CreateSubBranchMetaData(int startIndex, int endIndex)
				{
					int index = startIndex;
					SurveyQuestion question = myQuestion;
					int questionCount = question.CategoryCount;
					int lastAnswer;
					int currentAnswer = lastAnswer = -2;
					SubBranchMetaData[] subBranchData = mySubBranches;
					SubBranchMetaData currentSubBranch = default(SubBranchMetaData);
					int itemCount = endIndex + 1;
					int dataIndex;
					List<SampleDataElementNode> nodes = ((MainList)myBaseBranch).myNodes;
					for (; index < itemCount; ++index)
					{
						currentAnswer = question.ExtractAnswer(nodes[index].NodeData);
						if (currentAnswer == SurveyQuestion.NeutralAnswer)
						{
							currentAnswer = lastAnswer;
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

					return (index < endIndex || index == 0) ? index : endIndex;
				}
				#endregion //tree creation
				#region IBranch Members
				/// <summary>
				/// Implements <see cref="IBranch.BeginLabelEdit"/>
				/// </summary>
				public VirtualTreeLabelEditData BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
				{
					switch (TranslateRow(ref row))
					{
						case RowType.Neutral:
							return myNeutralBranch.BeginLabelEdit(row, column, activationStyle);
						default:
							return VirtualTreeLabelEditData.Invalid;
					}
				}
				/// <summary>
				/// Implements <see cref="IBranch.CommitLabelEdit"/>
				/// </summary>
				public LabelEditResult CommitLabelEdit(int row, int column, string newText)
				{
					switch (TranslateRow(ref row))
					{
						case RowType.Neutral:
							return myNeutralBranch.CommitLabelEdit(row, column, newText);
						default:
							return LabelEditResult.CancelEdit;
					}
				}
				/// <summary>
				/// Return the features supported by this branch.
				/// </summary>
				/// <value></value>
				public BranchFeatures Features
				{
					get
					{
						return BranchFeatures.Expansions | BranchFeatures.BranchRelocation | BranchFeatures.InsertsAndDeletes | BranchFeatures.PositionTracking | BranchFeatures.ExplicitLabelEdits;
					}
				}
				/// <summary>
				/// Implements <see cref="IBranch.GetAccessibilityData"/>
				/// </summary>
				public VirtualTreeAccessibilityData GetAccessibilityData(int row, int column)
				{
					switch (TranslateRow(ref row))
					{
						case RowType.Neutral:
							return myNeutralBranch.GetAccessibilityData(row, column);
						default:
							return VirtualTreeAccessibilityData.Empty;
					}
				}
				/// <summary>
				/// Implements <see cref="IBranch.BeginLabelEdit"/>
				/// </summary>
				public VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
				{
					switch (TranslateRow(ref row))
					{
						case RowType.Neutral:
							return myNeutralBranch.GetDisplayData(row, column, requiredData);
						default:
							return VirtualTreeDisplayData.Empty;
					}
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
				/// <summary>
				/// Implements <see cref="IBranch.GetText"/>
				/// </summary>
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
				/// <summary>
				/// Implements <see cref="IBranch.GetTipText"/>
				/// </summary>
				public string GetTipText(int row, int column, ToolTipType tipType)
				{
					switch (TranslateRow(ref row))
					{
						case RowType.Neutral:
							return myNeutralBranch.GetTipText(row, column, tipType);
						default:
							return null;
					}
				}
				/// <summary>
				/// Implements <see cref="IBranch.IsExpandable"/>
				/// </summary>
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
				/// <summary>
				/// Implements <see cref="IBranch.LocateObject"/>
				/// </summary>
				public LocateObjectData LocateObject(object obj, ObjectStyle style, int locateOptions)
				{
					if (style == ObjectStyle.TrackingObject)
					{
						LocateObjectData baseData = myBaseBranch.LocateObject(obj, style, locateOptions);
						int row = baseData.Row;
						if (row != VirtualTreeConstant.NullIndex)
						{
							// See if this item is contained in one of our subbranches. If it
							// is in a subbranch, then we return control to the calling tree to
							// manage branch expansion. Otherwise, we need to forward the request
							// to the neutral branch directly.
							SubBranchMetaData[] branches = mySubBranches;
							for (int i = 0; i < branches.Length; ++i)
							{
								if (branches[i].IsInRange(row))
								{
									int adjustedIndex = Array.IndexOf<int>(mySubBranchOrder, i);
									if (myNeutralOnTop)
									{
										IBranch neutral = myNeutralBranch;
										adjustedIndex += (neutral != null) ? neutral.VisibleItemCount : 0;
									}
									return new LocateObjectData(adjustedIndex, 0, (int)TrackingObjectAction.NextLevel);
								}
							}
							IBranch neutralBranch = myNeutralBranch;
							if (neutralBranch != null && neutralBranch.VisibleItemCount != 0)
							{
								// If we get here, we either have no match or the match is in the neutral branch
								LocateObjectData neutralData = neutralBranch.LocateObject(obj, style, locateOptions);
								if (neutralData.Row != VirtualTreeConstant.NullIndex)
								{
									if (!myNeutralOnTop)
									{
										neutralData.Row += myVisibleSubBranchCount;
									}
									if (baseData.Options == (int)TrackingObjectAction.NextLevel &&
										neutralData.Options == (int)TrackingObjectAction.ThisLevel)
									{
										neutralData.Options = (int)TrackingObjectAction.NextLevel;
									}
									return neutralData;
								}
							}
						}
					}
					return new LocateObjectData(VirtualTreeConstant.NullIndex, VirtualTreeConstant.NullIndex, 0);
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
				/// <summary>
				/// Implements <see cref="IBranch.OnDragEvent"/>
				/// </summary>
				public void OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
				{
					switch (TranslateRow(ref row))
					{
						case RowType.Neutral:
							myNeutralBranch.OnDragEvent(sender, row, column, eventType, args);
							break;
					}
				}
				/// <summary>
				/// Implements <see cref="IBranch.OnGiveFeedback"/>
				/// </summary>
				public void OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
				{
					switch (TranslateRow(ref row))
					{
						case RowType.Neutral:
							myNeutralBranch.OnGiveFeedback(args, row, column);
							break;
					}
				}
				/// <summary>
				/// Implements <see cref="IBranch.OnQueryContinueDrag"/>
				/// </summary>
				public void OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
				{
					switch (TranslateRow(ref row))
					{
						case RowType.Neutral:
							myNeutralBranch.OnQueryContinueDrag(args, row, column);
							break;
					}
				}
				/// <summary>
				/// Implements <see cref="IBranch.OnStartDrag"/>
				/// </summary>
				public VirtualTreeStartDragData OnStartDrag(object sender, int row, int column, DragReason reason)
				{
					switch (TranslateRow(ref row))
					{
						case RowType.Neutral:
							return myNeutralBranch.OnStartDrag(sender, row, column, reason);
					}
					return VirtualTreeStartDragData.Empty;
				}
				/// <summary>
				/// Implements <see cref="IBranch.SynchronizeState"/>
				/// </summary>
				public StateRefreshChanges SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
				{
					switch (TranslateRow(ref row))
					{
						case RowType.Neutral:
							return myNeutralBranch.SynchronizeState(row, column, matchBranch, matchRow, matchColumn);
					}
					return StateRefreshChanges.None;
				}
				/// <summary>
				/// Implements <see cref="IBranch.ToggleState"/>
				/// </summary>
				public StateRefreshChanges ToggleState(int row, int column)
				{
					switch (TranslateRow(ref row))
					{
						case RowType.Neutral:
							return myNeutralBranch.ToggleState(row, column);
					}
					return StateRefreshChanges.None;
				}
				/// <summary>
				/// Implements <see cref="IBranch.UpdateCounter"/>
				/// </summary>
				public int UpdateCounter
				{
					get
					{
						return 0;
					}
				}
				/// <summary>
				/// Implements <see cref="IBranch.VisibleItemCount"/>
				/// </summary>
				public int VisibleItemCount
				{
					get
					{
						IBranch neutral = myNeutralBranch;
						return ((neutral != null) ? neutral.VisibleItemCount : 0) + myVisibleSubBranchCount;
					}
				}
				#endregion
				#region ElementDeletedAt
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
					for (int i = 0; i < this.mySubBranches.Length; i++)
					{
						if (mySubBranches[i].AdjustDelete(index, -1, modificationEvents))
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
											modificationEvents(notifyThrough, BranchModificationEventArgs.DeleteItems(notifyThrough, notifyThroughOffset + deleteAt, 1));
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

					// Handle neutral branches
					IBranch neutralBranch = myNeutralBranch;
					if (neutralBranch != null)
					{
						Debug.Assert(notifyThroughOffset == 0 || notifyThrough != null);
						int offsetAdjustment = notifyThroughOffset + (myNeutralOnTop ? 0 : myVisibleSubBranchCount);
						IBranch notifyBranch = notifyThrough ?? this;
						SimpleListShifter shifter;
						if (null == (shifter = (neutralBranch as SimpleListShifter)))
						{
							((ListGrouper)neutralBranch).ElementDeletedAt(index, modificationEvents, notifyBranch, offsetAdjustment);
						}
						// Simple shifter cases
						else if (shifter.FirstItem > index)
						{
							shifter.FirstItem -= 1;
						}
						else if (shifter.LastItem >= index)
						{
							shifter.Count -= 1;
							if (modificationEvents != null)
							{
								modificationEvents(notifyBranch, BranchModificationEventArgs.DeleteItems(notifyBranch, offsetAdjustment + index - shifter.FirstItem, 1));
							}
						}
						if (neutralBranch.VisibleItemCount == 0)
						{
							// This can be recreated, there is no reason to keep an empty passthrough branch
							myNeutralBranch = null;
						}
					}
				}
				#endregion // ElementDeletedAt
				#region ElementAddedAt
				/// <summary>
				/// Adds a node at given the index and adjusts indices
				/// </summary>
				/// <param name="index">The index of the newly added element</param>
				/// <param name="modificationEvents">The event handler to notify the tree with</param>
				public void ElementAddedAt(int index, BranchModificationEventHandler modificationEvents)
				{
					ElementAddedAt(index, false, modificationEvents, null, 0);
				}
				/// <summary>
				/// Adds a node at given the index and adjusts indices
				/// </summary>
				/// <param name="index">The index of the newly added element</param>
				/// <param name="neutralChanged">Forwarded from an outer grouping where the neutral contents have changed.</param>
				/// <param name="modificationEvents">The event handler to notify the tree with</param>
				/// <param name="notifyThrough">A wrapper branch. Notify the event handler with this branch, not the current branch</param>
				/// <param name="notifyThroughOffset">Used if notifyThrough is not null. The starting offset of this branch in the outer branch.</param>
				private void ElementAddedAt(int index, bool neutralChanged, BranchModificationEventHandler modificationEvents, IBranch notifyThrough, int notifyThroughOffset)
				{
					int currentAnswer = myQuestion.ExtractAnswer(((MainList)myBaseBranch).myNodes[index].NodeData);
					bool neutralOnTop = myNeutralOnTop;
					int testCurrentAnswer = (currentAnswer == SurveyQuestion.NeutralAnswer && !neutralOnTop) ? int.MaxValue : currentAnswer;
					SubBranchMetaData[] subBranches = mySubBranches;
					for (int i = 0; i < subBranches.Length; i++)
					{
						if (subBranches[i].AdjustAdd(i == testCurrentAnswer, i > testCurrentAnswer, index, modificationEvents))
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
											modificationEvents(notifyThrough, BranchModificationEventArgs.InsertItems(notifyThrough, notifyThroughOffset + insertAt - 1, 1));
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
					IBranch neutralBranch = myNeutralBranch;
					if (neutralBranch != null)
					{
						Debug.Assert(notifyThroughOffset == 0 || notifyThrough != null);
						int offsetAdjustment = notifyThroughOffset + (neutralOnTop ? 0 : myVisibleSubBranchCount);
						IBranch notifyBranch = notifyThrough ?? this;
						SimpleListShifter shifter;
						if (null == (shifter = (neutralBranch as SimpleListShifter)))
						{
							((ListGrouper)neutralBranch).ElementAddedAt(index, neutralChanged && currentAnswer == SurveyQuestion.NeutralAnswer, modificationEvents, notifyBranch, offsetAdjustment);
						}
						// Simple shifter cases
						else if (shifter.FirstItem > (index - (neutralChanged ? 0 : 1)))
						{
							shifter.FirstItem += 1;	
						}
						else if (shifter.LastItem >= index ||
							(currentAnswer == SurveyQuestion.NeutralAnswer && index == (shifter.LastItem + 1)))
						{
							shifter.Count += 1;
							if (modificationEvents != null)
							{
								modificationEvents(notifyBranch, BranchModificationEventArgs.InsertItems(notifyBranch, offsetAdjustment + index - shifter.FirstItem - 1, 1));
							}
						}
					}
					else if (currentAnswer == SurveyQuestion.NeutralAnswer &&
						index == (neutralOnTop ? subBranches[0].Start - 1 : (subBranches[subBranches.Length - 1].End + 1)))
					{
						// Dynamically create the neutral branch
						SurveyQuestion nextQuestion = NextGroupableQuestion;
						myNeutralBranch = neutralBranch = (nextQuestion != null) ?
							//TODO: use neutralOnTop bool stored with question instead of passing one in
							(IBranch)new ListGrouper(myBaseBranch, nextQuestion, index, index, myNeutralOnTop) :
							new SimpleListShifter(myBaseBranch, index, 1);
						if (modificationEvents != null)
						{
							Debug.Assert(notifyThroughOffset == 0 || notifyThrough != null);
							IBranch notifyBranch = notifyThrough ?? this;
							modificationEvents(notifyBranch, BranchModificationEventArgs.InsertItems(notifyBranch, notifyThroughOffset + (myNeutralOnTop ? 0 : myVisibleSubBranchCount) - 1, 1));
						}
					}
				}
				#endregion // ElementAddedAt
				#region ElementModifiedAt
				/// <summary>
				/// Renames the node at given index and redraws the tree
				/// </summary>
				/// <param name="fromIndex">Original index of the element</param>
				/// <param name="toIndex">New index of the element. Can be the same as fromIndex</param>
				/// <param name="displayChanged">Set if the display should be changed even when the element have not moved.</param>
				/// <param name="modificationEvents">The event handler to notify the tree with</param>
				public void ElementModifiedAt(int fromIndex, int toIndex, bool displayChanged, BranchModificationEventHandler modificationEvents)
				{
					ElementModifiedAt(fromIndex, toIndex, displayChanged, modificationEvents, null, 0);
				}
				/// <summary>
				/// Renames the node at given index and redraws the tree
				/// </summary>
				/// <param name="fromIndex">Original index of the element</param>
				/// <param name="toIndex">New index of the element. Can be the same as fromIndex</param>
				/// <param name="displayChanged">Set if the display should be changed even when the element have not moved.</param>
				/// <param name="modificationEvents">The event handler to notify the tree with</param>
				/// <param name="notifyThrough">A wrapper branch. Notify the event handler with this branch, not the current branch</param>
				/// <param name="notifyThroughOffset">Used if notifyThrough is not null. The starting offset of this branch in the outer branch.</param>
				public void ElementModifiedAt(int fromIndex, int toIndex, bool displayChanged, BranchModificationEventHandler modificationEvents, IBranch notifyThrough, int notifyThroughOffset)
				{
					if (modificationEvents != null)
					{
						SubBranchMetaData[] subBranches = mySubBranches;
						for (int i = 0; i < subBranches.Length; i++)
						{
							subBranches[i].AdjustModified(fromIndex, toIndex, displayChanged, modificationEvents);
						}
						// Handle any nested neutral branches
						IBranch neutralBranch = myNeutralBranch;
						if (neutralBranch != null)
						{
							Debug.Assert(notifyThroughOffset == 0 || notifyThrough != null);
							int offsetAdjustment = notifyThroughOffset + (myNeutralOnTop ? 0 : myVisibleSubBranchCount);
							IBranch notifyBranch = (notifyThrough != null) ? notifyThrough : this;
							SimpleListShifter shifter;
							if (null == (shifter = (neutralBranch as SimpleListShifter)))
							{
								((ListGrouper)neutralBranch).ElementModifiedAt(fromIndex, toIndex, displayChanged, modificationEvents, notifyBranch, offsetAdjustment);
							}
							else if (shifter.FirstItem <= fromIndex && shifter.LastItem >= fromIndex)
							{
								if (fromIndex != toIndex)
								{
									modificationEvents(notifyBranch, BranchModificationEventArgs.MoveItem(notifyBranch, fromIndex - shifter.FirstItem + offsetAdjustment, toIndex - shifter.FirstItem + offsetAdjustment));
								}
								else if (displayChanged)
								{
									modificationEvents(notifyBranch, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.Text, notifyBranch, fromIndex - shifter.FirstItem + offsetAdjustment, 0, 1)));
								}
							}
						}
					}
				}
				#endregion // ElementModifiedAt
				#region ElementChangedAt
				/// <summary>
				/// Modifies display of the node at the given index and redraws the tree
				/// </summary>
				/// <param name="index">Index of the display change for the item</param>
				/// <param name="modificationEvents">The event handler to notify the tree with</param>
				public void ElementChangedAt(int index, BranchModificationEventHandler modificationEvents)
				{
					ElementChangedAt(index, modificationEvents, null, 0);
				}
				/// <summary>
				/// Modifies display of the node at the given index and redraws the tree
				/// </summary>
				/// <param name="index">Index of the display change for the item</param>
				/// <param name="modificationEvents">The event handler to notify the tree with</param>
				/// <param name="notifyThrough">A wrapper branch. Notify the event handler with this branch, not the current branch</param>
				/// <param name="notifyThroughOffset">Used if notifyThrough is not null. The starting offset of this branch in the outer branch.</param>
				public void ElementChangedAt(int index, BranchModificationEventHandler modificationEvents, IBranch notifyThrough, int notifyThroughOffset)
				{
					if (modificationEvents != null)
					{
						// Handle the main branches
						SubBranchMetaData[] subBranches = mySubBranches;
						for (int i = 0; i < subBranches.Length; i++)
						{
							subBranches[i].AdjustChange(index, modificationEvents);
						}

						// Handle any nested neutral branches
						IBranch neutralBranch = myNeutralBranch;
						if (neutralBranch != null)
						{
							Debug.Assert(notifyThroughOffset == 0 || notifyThrough != null);
							int offsetAdjustment = notifyThroughOffset + (myNeutralOnTop ? 0 : myVisibleSubBranchCount);
							IBranch notifyBranch = notifyThrough ?? this;
							SimpleListShifter shifter;
							if (null == (shifter = (neutralBranch as SimpleListShifter)))
							{
								((ListGrouper)neutralBranch).ElementChangedAt(index, modificationEvents, notifyBranch, offsetAdjustment);
							}
							else if (shifter.FirstItem <= index && shifter.LastItem >= index)
							{
								modificationEvents(notifyBranch, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.VisibleElements, notifyBranch, index - shifter.FirstItem + offsetAdjustment, 0, 1)));
							}
						}
					}
				}
				#endregion // ElementChangedAt
			}
		}
	}
}
