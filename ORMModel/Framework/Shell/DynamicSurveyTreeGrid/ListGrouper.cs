#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
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
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.VirtualTreeGrid;
namespace ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid
{
	/// <summary>
	/// A <see cref="TypeDescriptionProvider"/> used by the internal grouping
	/// mechanisms in the <see cref="SurveyTree{Object}"/> implementation. This
	/// cannot be a nested type because it is used in an attribute.
	/// </summary>
	public sealed class SurveyGroupHeaderTypeDescriptionProvider : TypeDescriptionProvider
	{
		private class SurveyGroupHeaderTypeDescriptor : CustomTypeDescriptor
		{
			private string myComponentName;
			public SurveyGroupHeaderTypeDescriptor(ISurveyGroupHeader groupHeader)
			{
				ISurveyQuestionTypeInfo question = groupHeader.QuestionTypeInfo;
				ISurveyDynamicValues dynamicValues = question.DynamicQuestionValues;
				if (dynamicValues != null)
				{
					myComponentName = dynamicValues.GetValueName(groupHeader.Answer);
				}
				else
				{
					myComponentName = Utility.GetLocalizedEnumName(question.QuestionType, groupHeader.Answer);
				}
			}
			public override string GetClassName()
			{
				return FrameworkResourceStrings.DynamicSurveyHeaderClassName;
			}
			public override string GetComponentName()
			{
				return myComponentName;
			}
		}
		/// <summary>
		/// Standard override. Provides a custom descriptor for instances of the
		/// <see cref="ISurveyGroupHeader"/> interface.
		/// </summary>
		public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
		{
			ISurveyGroupHeader typedInstance = instance as ISurveyGroupHeader;
			if (typedInstance != null)
			{
				return new SurveyGroupHeaderTypeDescriptor(typedInstance);
			}
			return base.GetTypeDescriptor(objectType, instance);
		}
	}
	partial class SurveyTree<SurveyContextType>
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
									branch = new ListGrouper(grouper.myBaseBranch, nextQuestion, Start, End, grouper.myNeutralOnTop, false);
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
					/// <param name="headerAlwaysVisible">The header for sub branch is always visible</param>
					/// <param name="modificationEvents">The modification events.</param>
					/// <returns><see langword="true"/> to fully remove the node</returns>
					public bool AdjustDelete(int index, bool headerAlwaysVisible, BranchModificationEventHandler modificationEvents)
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
									--End;
									if (End < Start && !headerAlwaysVisible)
									{
										myBranch = null;
										return true;
									}
									else if (branch != null)
									{
										if (notifyListShifter != null)
										{
											notifyListShifter.Count -= 1;
											if (modificationEvents != null)
											{
												HandleSubranch(index, -1, modificationEvents, notifyListShifter);
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
									--Start;
									if (branch != null)
									{
										if (notifyListShifter != null)
										{
											notifyListShifter.FirstItem -= 1;
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
								--Start;
								--End;
								if (branch != null)
								{
									if (notifyListShifter != null)
									{
										notifyListShifter.FirstItem -= 1;
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
					public bool AdjustAdd(bool isChanged, bool afterChanged, bool headerAlwaysVisible, int index, BranchModificationEventHandler modificationEvents)
					{
						if (End < Start)
						{
							if (isChanged)
							{
								// We're adding the first item to a branch. We know
								// all of the values based on the provided index, so
								// there is no reason to calculate current state based
								// on the old data, which we also do not bother to synchronize
								// for empty branches. If there is an existing branch, then
								// it exists to show an empty header node only and
								// index has no meaning except to signal there is no data.
								// Make sure we update the base information before notifying
								// because this branch is transitioning into coresponding to
								// real data and needs to be correctly positioned.
								IBranch branch = myBranch;
								End = index;
								Start = index;
								if (branch != null)
								{
									SimpleListShifter notifyListShifter;
									if (null != (notifyListShifter = branch as SimpleListShifter))
									{
										notifyListShifter.FirstItem = index;
										notifyListShifter.Count += 1;
										if (modificationEvents != null)
										{
											HandleSubranch(0, +1, modificationEvents, notifyListShifter);
										}
									}
									else
									{
										ListGrouper notifyListGrouper = (ListGrouper)branch;
										notifyListGrouper.RebaseHeaderOnlyBranch(index);
										notifyListGrouper.ElementAddedAt(index, true, modificationEvents, null, 0);
									}
								}
								else
								{
									return !headerAlwaysVisible;
								}
							}
						}
						else if (index <= End || (isChanged && (index == End + 1)))
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
									Debug.Assert(branch == null, "You should not have a branch with no children unless it is shown for empty headers, which should not hit this code path");
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
					/// <summary>
					/// Reset the base information for items and branches that correspond to
					/// no node data. These may have visible branches that are shown for
					/// empty headers, and the indices in these branches are not tracked beyond
					/// the assertion that the end is less than the start.
					/// </summary>
					/// <param name="index">The location of the first piece of real data that is being added to this branch.</param>
					public void Rebase(int index)
					{
						Debug.Assert(End < Start); // Should not be called otherwise, this happens before the notifications to add the first real element
						Start = index;
						End = index - 1;
						IBranch branch = myBranch;
						if (branch != null)
						{
							SimpleListShifter listShifter = branch as SimpleListShifter;
							if (listShifter != null)
							{
								listShifter.FirstItem = index;
							}
							else
							{
								((ListGrouper)branch).RebaseHeaderOnlyBranch(index);
							}
						}
					}
					#endregion
					#region AdjustChange
					public void AdjustChange(int index, VirtualTreeDisplayDataChanges changes, BranchModificationEventHandler modificationEvents)
					{
						if (Start <= index && End >= index)
						{
							int startIndex = index;
							index -= Start;
							IBranch branch = myBranch;
							if (branch != null)
							{
								SimpleListShifter shifter;
								ListGrouper grouper;
								if (null != (shifter = branch as SimpleListShifter))
								{
									modificationEvents(shifter, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(changes, shifter, index, 0, 1)));
								}
								else if (null != (grouper = branch as ListGrouper))
								{
									grouper.ElementChangedAt(startIndex, changes, modificationEvents, null, 0);
								}
							}
						}
					}
					#endregion
				}
				#endregion //SubBranchMetaData struct
				#region GroupHeaderTrackingObject class
				[TypeDescriptionProvider(typeof(SurveyGroupHeaderTypeDescriptionProvider))]
				private sealed class GroupHeaderTrackingObject : ISurveyGroupHeader, IFreeFormCommandProvider<SurveyContextType>
				{
					#region Member Variables
					private ISurveyQuestionTypeInfo<SurveyContextType> myQuestionTypeInfo;
					private int myAnswer;
					private object myContextElement;
					#endregion // Member Variables
					#region Constructor
					/// <summary>
					/// Create a <see cref="GroupHeaderTrackingObject"/> for a header node.
					/// </summary>
					/// <param name="questionTypeInfo">The header's <see cref="ISurveyQuestionTypeInfo"/></param>
					/// <param name="answer">The answer to the question, interpreted based on the provided <paramref name="questionTypeInfo"/></param>
					/// <param name="contextElement">The context element</param>
					public GroupHeaderTrackingObject(ISurveyQuestionTypeInfo<SurveyContextType> questionTypeInfo, int answer, object contextElement)
					{
						myQuestionTypeInfo = questionTypeInfo;
						myAnswer = answer;
						myContextElement = contextElement;
					}
					#endregion // Constructor
					#region ISurveyGroupHeader Implementation
					ISurveyQuestionTypeInfo ISurveyGroupHeader.QuestionTypeInfo
					{
						get
						{
							return myQuestionTypeInfo;
						}
					}
					int ISurveyGroupHeader.Answer
					{
						get
						{
							return myAnswer;
						}
					}
					object ISurveyGroupHeader.ContextElement
					{
						get
						{
							return myContextElement;
						}
					}
					#endregion // ISurveyGroupHeader Implementation
					#region Equals and GetHashCode
					/// <summary>
					/// Determine equality based on contents
					/// </summary>
					public override bool Equals(object obj)
					{
						ISurveyGroupHeader otherHeader = obj as ISurveyGroupHeader;
						return (otherHeader != null) ? Equals(otherHeader) : false;
					}
					/// <summary>
					/// Required override required with <see cref="Equals(object)"/>
					/// </summary>
					public override int GetHashCode()
					{
						object contextElement = myContextElement;
						return (contextElement != null) ?
							Utility.GetCombinedHashCode(myQuestionTypeInfo.GetHashCode(), myAnswer.GetHashCode(), contextElement.GetHashCode()) :
							Utility.GetCombinedHashCode(myQuestionTypeInfo.GetHashCode(), myAnswer.GetHashCode());
					}
					/// <summary>
					/// Implements <see cref="IEquatable{ISurveyGroupHeader}"/>
					/// </summary>
					public bool Equals(ISurveyGroupHeader other)
					{
						return myQuestionTypeInfo == other.QuestionTypeInfo &&
							myAnswer == other.Answer &&
							myContextElement == other.ContextElement;
					}
					/// <summary>
					/// Corresponds to Equals implementation
					/// </summary>
					public static bool operator ==(GroupHeaderTrackingObject left, ISurveyGroupHeader right)
					{
						return left.Equals(right);
					}
					/// <summary>
					/// Corresponds to Equals implementation
					/// </summary>
					public static bool operator !=(GroupHeaderTrackingObject left, ISurveyGroupHeader right)
					{
						return !left.Equals(right);
					}
					#endregion // Equals and GetHashCode
					#region IFreeFormCommandProvider Implementation
					int IFreeFormCommandProvider<SurveyContextType>.GetFreeFormCommandCount(SurveyContextType context, object targetElement)
					{
						IFreeFormCommandProvider<SurveyContextType> passTo = myQuestionTypeInfo.GetFreeFormCommands(context, myAnswer);
						if (passTo != null)
						{
							return passTo.GetFreeFormCommandCount(context, targetElement);
						}
						return 0;
					}
					void IFreeFormCommandProvider<SurveyContextType>.OnFreeFormCommandStatus(SurveyContextType context, object targetElement, MenuCommand command, int commandIndex)
					{
						IFreeFormCommandProvider<SurveyContextType> passTo = myQuestionTypeInfo.GetFreeFormCommands(context, myAnswer);
						if (passTo != null)
						{
							passTo.OnFreeFormCommandStatus(context, targetElement, command, commandIndex);
						}
					}
					void IFreeFormCommandProvider<SurveyContextType>.OnFreeFormCommandExecute(SurveyContextType context, object targetElement, int commandIndex)
					{
						IFreeFormCommandProvider<SurveyContextType> passTo = myQuestionTypeInfo.GetFreeFormCommands(context, myAnswer);
						if (passTo != null)
						{
							passTo.OnFreeFormCommandExecute(context, targetElement, commandIndex);
						}
					}
					#endregion // IFreeFormCommandProvider Implementation
				}
				#endregion // GroupHeaderTrackingObject class
				#region row helper struct, enum, and method
				private enum RowType { Neutral, SubBranch, Invalid }
				private RowType TranslateRow(ref int row)
				{
					IBranch neutral = myNeutralBranch;
					int neutralCount = (neutral != null) ? neutral.VisibleItemCount : 0;
					int subBranchCount = myVisibleSubBranchCount;
					if (row >= (neutralCount + subBranchCount) || row < 0)
					{
						// Occurs during delete notifications with multiple branches
						// visible. See comments in MainList.OutOfRange.
						return RowType.Invalid;
					}
					if (myNeutralOnTop)
					{
						if (row < neutralCount)
						{
							return RowType.Neutral;
						}
						row -= neutralCount;
					}
					if (row < subBranchCount)
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
				private readonly bool myNeutralAncestry;
				private readonly SubBranchMetaData[] mySubBranches;
				private readonly int[] mySubBranchOrder;
				private int myVisibleSubBranchCount;
				private IBranch myNeutralBranch;

				/// <summary>
				/// constructor for list grouper, neither parameter can be null
				/// </summary>
				/// <param name="baseBranch">The <see cref="IBranch"/> this grouper built on.
				/// This is always a <see cref="MainList"/>, but the interface is used more than the list.</param>
				/// <param name="question"><see cref="SurveyQuestion"/> to be applied by this grouper</param>
				/// <param name="startIndex">The starting index of the range of nodes in this branch
				/// in the full set of nodes owned by th base branch.</param>
				/// <param name="endIndex">The end index of the range of nodes in this branch
				/// in the full set of nodes owned by th base branch. This may be less than the
				/// startIndex if there are no nodes.</param>
				/// <param name="neutralOnTop">Display neutral items before item headers</param>
				/// <param name="neutralAncestry">All ancestors containing this branch are neutral.
				/// Empty grouping nodes are shown only for branches with pure neutral ancestry.</param>
				public ListGrouper(IBranch baseBranch, SurveyQuestion question, int startIndex, int endIndex, bool neutralOnTop, bool neutralAncestry)
				{
					Debug.Assert(baseBranch != null);
					myBaseBranch = baseBranch;
					myQuestion = question;
					mySurvey = myQuestion.QuestionList;
					Debug.Assert(mySurvey != null); //questions should only be accessible through a survey in which case their question list must be set
					myVisibleSubBranchCount = 0;
					myNeutralOnTop = neutralOnTop;
					myNeutralAncestry = neutralAncestry;
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
							(IBranch)new ListGrouper(myBaseBranch, nextQuestion, startNeutralIndex, index, myNeutralOnTop, myNeutralAncestry) :
							new SimpleListShifter(myBaseBranch, startNeutralIndex, index - startNeutralIndex + 1);
						return ++index;
					}
					else if (HasTrailingEmptyHeaderGroups)
					{
						// No items, but we need the branch for the header
						myNeutralBranch = new ListGrouper(myBaseBranch, NextGroupableQuestion, startNeutralIndex, index, myNeutralOnTop, myNeutralAncestry);
						return startNeutralIndex;
					}
					else
					{
						return startNeutralIndex;
					}
				}
				/// <summary>
				/// A helper method to rebase the data in the branch. This is in place
				/// so that we don't need to track indices in branches that correspond
				/// to no data.
				/// </summary>
				/// <param name="index">The location of the new data</param>
				private void RebaseHeaderOnlyBranch(int index)
				{
					SubBranchMetaData[] subBranches = mySubBranches;
					for (int i = 0; i < subBranches.Length; ++i)
					{
						subBranches[i].Rebase(index);
					}
					IBranch neutral = myNeutralBranch;
					if (neutral != null)
					{
						SimpleListShifter listShifter = neutral as SimpleListShifter;
						if (listShifter != null)
						{
							listShifter.FirstItem = index;
						}
						else
						{
							((ListGrouper)neutral).RebaseHeaderOnlyBranch(index);
						}
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
				/// Return true if any trailing grouping questions after the
				/// current group support empty header nodes.
				/// </summary>
				private bool HasTrailingEmptyHeaderGroups
				{
					get
					{
						if (!myNeutralAncestry)
						{
							return false;
						}
						Survey survey = mySurvey;
						int contextQuestionIndex = survey.GetIndex(myQuestion.Question.QuestionType);
						int questionCount = survey.Count;
						for (int i = contextQuestionIndex + 1; i < questionCount; ++i)
						{
							// UNDONE: This should come from the display information, not the question
							// itself.
							// Note that the generators only spit EmptyGroups if Grouping is set, but check
							// both is equally efficient and more robust.
							if ((SurveyQuestionUISupport.Grouping | SurveyQuestionUISupport.EmptyGroups) == (survey[i].UISupport & (SurveyQuestionUISupport.Grouping | SurveyQuestionUISupport.EmptyGroups)))
							{
								return true;
							}
						}
						return false;
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
					int answerCount = question.CategoryCount;
					int lastAnswer;
					int currentAnswer = lastAnswer = -2;
					SubBranchMetaData[] subBranchData = mySubBranches;
					SubBranchMetaData currentSubBranch = default(SubBranchMetaData);
					int indexBound = endIndex + 1;
					int dataIndex;
					MainList mainList = (MainList)myBaseBranch;
					List<SampleDataElementNode> nodes = mainList.myNodes;
					SurveyContextType surveyContext = mainList.mySurveyTree.mySurveyContext;
					for (; index < indexBound; ++index)
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
					for (dataIndex = currentAnswer + 1; dataIndex < answerCount; ++dataIndex)
					{
						currentSubBranch = subBranchData[dataIndex];
						currentSubBranch.Start = index;
						currentSubBranch.End = index - 1;
						currentSubBranch.Header = question.CategoryHeader(dataIndex);
						subBranchData[dataIndex] = currentSubBranch;
					}

					int[] orderArray = mySubBranchOrder;
					ISurveyQuestionTypeInfo<SurveyContextType> questionInfo = question.Question;
					for (int i = 0; i < answerCount; ++i)
					{
						orderArray[i] = i;
					}
					bool ignoreEmptyGroups = !myNeutralAncestry || 0 == (questionInfo.UISupport & SurveyQuestionUISupport.EmptyGroups);
					if (answerCount > 1)
					{
						Array.Sort<int>(
							orderArray,
							delegate(int left, int right)
							{
								if (left == right)
								{
									return 0;
								}
								int retVal = 0;
								if (subBranchData[left].Count == 0 && (ignoreEmptyGroups || !questionInfo.ShowEmptyGroup(surveyContext, left)))
								{
									if (subBranchData[right].Count != 0 || (!ignoreEmptyGroups && questionInfo.ShowEmptyGroup(surveyContext, right)))
									{
										retVal = 1;
									}
								}
								else if (subBranchData[right].Count == 0 && (ignoreEmptyGroups || !questionInfo.ShowEmptyGroup(surveyContext, right)))
								{
									retVal = -1;
								}
								if (retVal == 0)
								{
									retVal = (left < right) ? -1 : 1;
								}
								return retVal;
							});
					}

					myVisibleSubBranchCount = 0;
					for (int i = 0; i < answerCount; ++i)
					{
						if (subBranchData[i].Count != 0 || (!ignoreEmptyGroups && questionInfo.ShowEmptyGroup(surveyContext, i)))
						{
							++myVisibleSubBranchCount;
						}
					}

					return (endIndex < startIndex) ?
						startIndex :
						((index < endIndex || index == 0) ? index : endIndex);
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
						return BranchFeatures.Expansions | BranchFeatures.BranchRelocation | BranchFeatures.InsertsAndDeletes | BranchFeatures.PositionTracking | BranchFeatures.ExplicitLabelEdits | BranchFeatures.DelayedLabelEdits | BranchFeatures.ImmediateMouseLabelEdits | BranchFeatures.ImmediateSelectionLabelEdits;
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
							switch (style)
							{
								case ObjectStyle.ExpandedBranch:
									return mySubBranches[row].EnsureBranch(this);
								case ObjectStyle.TrackingObject:
									return new GroupHeaderTrackingObject(myQuestion.Question, row, ((MainList)myBaseBranch).myContextElement);
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
							return mySubBranches[row].Count > 0 || (myNeutralAncestry && myQuestion.Question.ShowEmptyGroup(((MainList)myBaseBranch).mySurveyTree.mySurveyContext, row));
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
					SubBranchMetaData[] subBranches = mySubBranches;
					ISurveyQuestionTypeInfo<SurveyContextType> questionInfo = myQuestion.Question;
					bool checkForEmptyGroups = myNeutralAncestry && 0 != (questionInfo.UISupport & SurveyQuestionUISupport.EmptyGroups);
					SurveyContextType surveyContext = ((MainList)myBaseBranch).mySurveyTree.mySurveyContext;
					for (int i = 0; i < subBranches.Length; ++i)
					{
						if (subBranches[i].AdjustDelete(index, checkForEmptyGroups && questionInfo.ShowEmptyGroup(surveyContext, i), modificationEvents))
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
					// An element may have been added to a header that is visible only
					// because it is always shown, not because it has visible children.
					// These need to be specially accounted for so that the data can be
					// fixed up without doing extra work to reshow the node.
					int groupAlwaysShownFor = (currentAnswer != SurveyQuestion.NeutralAnswer && myNeutralAncestry && myQuestion.Question.ShowEmptyGroup(((MainList)myBaseBranch).mySurveyTree.mySurveyContext, currentAnswer)) ? currentAnswer : int.MaxValue;
					SubBranchMetaData[] subBranches = mySubBranches;
					for (int i = 0; i < subBranches.Length; ++i)
					{
						if (subBranches[i].AdjustAdd(i == testCurrentAnswer, i > testCurrentAnswer, groupAlwaysShownFor == i, index, modificationEvents))
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
					else if (currentAnswer == SurveyQuestion.NeutralAnswer)
					{
						// If all of the metadata Start/End information were kept in sync all the time, then
						// we could check if we need a new neutral branch with the following code
						// index == (neutralOnTop ? subBranches[0].Start - 1 : (subBranches[subBranches.Length - 1].End + 1))
						// However, when empty groups are showing, these fields are updated only when data is
						// actually displayed in that section of the branch. Therefore, we need to look closer
						// at the data (besides the Start of the first and the End of the last) to get reliable
						// information. Only trust branch information that has data in it.
						bool addNeutral = false;
						if (neutralOnTop)
						{
							int i = 0;
							for (; i < subBranches.Length; ++i)
							{
								if (subBranches[i].Count > 0)
								{
									addNeutral = index == (subBranches[i].Start - 1);
									break;
								}
							}
							if (i == subBranches.Length)
							{
								// We have an empty neutral branch and will only be here if we are properly
								// rebased. All of the data is the same, test the first.
								addNeutral = index == (subBranches[0]).Start - 1;
							}
						}
						else
						{
							int i = subBranches.Length - 1;
							for (; i >= 0; --i)
							{
								if (subBranches[i].Count > 0)
								{
									addNeutral = index == (subBranches[i].End + 1);
									break;
								}
							}
							if (i == -1)
							{
								// Same comments as above. This is a rebased branch with header nodes only.
								addNeutral = index == (subBranches[0].End + 1);
							}
						}
						if (addNeutral)
						{
							// Dynamically create the neutral branch
							SurveyQuestion nextQuestion = NextGroupableQuestion;
							myNeutralBranch = neutralBranch = (nextQuestion != null) ?
								//TODO: use neutralOnTop bool stored with question instead of passing one in
								(IBranch)new ListGrouper(myBaseBranch, nextQuestion, index, index, myNeutralOnTop, myNeutralAncestry) :
								new SimpleListShifter(myBaseBranch, index, 1);
							if (modificationEvents != null)
							{
								Debug.Assert(notifyThroughOffset == 0 || notifyThrough != null);
								IBranch notifyBranch = notifyThrough ?? this;
								modificationEvents(notifyBranch, BranchModificationEventArgs.InsertItems(notifyBranch, notifyThroughOffset + (myNeutralOnTop ? 0 : myVisibleSubBranchCount) - 1, 1));
							}
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
						for (int i = 0; i < subBranches.Length; ++i)
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
				/// <param name="displayChanges">Notify which parts of the item need to be updated</param>
				/// <param name="modificationEvents">The event handler to notify the tree with</param>
				public void ElementChangedAt(int index, VirtualTreeDisplayDataChanges displayChanges, BranchModificationEventHandler modificationEvents)
				{
					ElementChangedAt(index, displayChanges, modificationEvents, null, 0);
				}
				/// <summary>
				/// Modifies display of the node at the given index and redraws the tree
				/// </summary>
				/// <param name="index">Index of the display change for the item</param>
				/// <param name="displayChanges">Notify which parts of the item need to be updated</param>
				/// <param name="modificationEvents">The event handler to notify the tree with</param>
				/// <param name="notifyThrough">A wrapper branch. Notify the event handler with this branch, not the current branch</param>
				/// <param name="notifyThroughOffset">Used if notifyThrough is not null. The starting offset of this branch in the outer branch.</param>
				public void ElementChangedAt(int index, VirtualTreeDisplayDataChanges displayChanges, BranchModificationEventHandler modificationEvents, IBranch notifyThrough, int notifyThroughOffset)
				{
					if (modificationEvents != null)
					{
						// Handle the main branches
						SubBranchMetaData[] subBranches = mySubBranches;
						for (int i = 0; i < subBranches.Length; ++i)
						{
							subBranches[i].AdjustChange(index, displayChanges, modificationEvents);
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
								((ListGrouper)neutralBranch).ElementChangedAt(index, displayChanges, modificationEvents, notifyBranch, offsetAdjustment);
							}
							else if (shifter.FirstItem <= index && shifter.LastItem >= index)
							{
								modificationEvents(notifyBranch, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(displayChanges, notifyBranch, index - shifter.FirstItem + offsetAdjustment, 0, 1)));
							}
						}
					}
				}
				#endregion // ElementChangedAt
			}
		}
	}
}
