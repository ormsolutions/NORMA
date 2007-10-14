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
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.VirtualTreeGrid;
using System.Diagnostics;
using System.Collections;

namespace Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid
{
	partial class SurveyTree
	{
		/// <summary>
		/// main branch provider for a SurveyTree, implements IBranch, main branch can be retrieved from RootBranch
		/// </summary>
		private partial class MainList : IBranch
		{
			#region survey question display struct
			//TODO: ask Matt, probably want to move this inside of the survey along with myCurrentDisplays, and have the survey manage it all
			private struct SurveyQuestionDisplay
			{
				public readonly SurveyQuestion Question;
				/// <summary>
				/// Limited to Sorting | Grouping
				/// </summary>
				public readonly SurveyQuestionUISupport CurrentGrouping;
				// UNDONE: AnswerOrder
				///// <summary>
				///// Allows reordering of answers in the display
				///// </summary>
				//public readonly int[] AnswerOrder;
				public readonly bool NeutralOnTop;
				public SurveyQuestionDisplay(SurveyQuestion question)
				{
					Question = question;
					CurrentGrouping = new SurveyQuestionUISupport(); // Choose Sorting or Grouping

					// UNDONE: AnswerOrder
					//AnswerOrder = new int[Question.CategoryCount];
					NeutralOnTop = false;
				}
			}
			#endregion //survey question display struct

			#region Constructor and instance fields
			private readonly List<SampleDataElementNode> myNodes;
			private readonly Survey mySurvey;
			private readonly List<SurveyQuestionDisplay> myCurrentDisplays;
			private BranchModificationEventHandler myModificationEvents;
			private int myAttachedEventCount;
			private ListGrouper myRootGrouper;
			private SurveyTree mySurveyTree;
			private object myContextElement;
			private int[] myOverlayIndices;
			/// <summary>
			/// Public constructor
			/// </summary>
			public MainList(SurveyTree surveyTree, object context, object expansionKey)
			{
				Debug.Assert(surveyTree != null, "SurveyTree required");
				Survey survey = surveyTree.GetSurvey(expansionKey);

				myNodeComparer = new NodeComparerImpl(this);
				mySurveyTree = surveyTree;
				List<SampleDataElementNode> nodes = new List<SampleDataElementNode>();
				myNodes = nodes;
				foreach (ISurveyNodeProvider nodeProvider in surveyTree.myNodeProviderList)
				{
					foreach (object elementNode in nodeProvider.GetSurveyNodes(context, expansionKey))
					{
						nodes.Add(new SampleDataElementNode(elementNode));
					}
				}
				mySurvey = survey;
				myContextElement = context;
				List<SurveyQuestionDisplay> currentDisplays = new List<SurveyQuestionDisplay>();
				myCurrentDisplays = currentDisplays;
				int surveyCount = survey.Count;
				for (int i = 0; i < surveyCount; ++i)
				{
					currentDisplays.Add(new SurveyQuestionDisplay(survey[i]));
				}
				SampleDataElementNode.InitializeNodes(nodes, survey);
				nodes.Sort(myNodeComparer);
				foreach (SampleDataElementNode node in myNodes)
				{
					mySurveyTree.myNodeDictionary.Add(node.Element, new NodeLocation(this, node));
				}
			}
			#endregion // Constructor and instance fields

			#region root branch and survey properties
			/// <summary>
			/// provides the RootBranch for a SurveyTree
			/// </summary>
			public IBranch CreateRootBranch()
			{
				SurveyQuestion question;
				// UNDONE: This is placeholder slop, it needs to depend on the display settings to pick the top-most SurveyQuestion
				if (mySurvey.Count != 0 &&
					(0 != ((question = mySurvey[0]).UISupport & SurveyQuestionUISupport.Grouping)))
				{
					return myRootGrouper = new ListGrouper(this, mySurvey[0], 0, myNodes.Count - 1, myCurrentDisplays[0].NeutralOnTop);
				}
				myRootGrouper = null;
				return this;
			}
			/// <summary>
			/// Raise the specified event for the given branch
			/// </summary>
			/// <param name="e"></param>
			private void RaiseBranchEvent(BranchModificationEventArgs e)
			{
				BranchModificationEventHandler handler = myModificationEvents;
				if (handler != null)
				{
					handler(this, e);
				}
			}
			/// <summary>
			/// provides the survey that this class contains, current applied questions can be accessed through the survey
			/// </summary>
			public Survey QuestionList
			{
				get
				{
					return mySurvey;
				}
			}
			#endregion //root branch and survey properties

			#region sort list methods
			#region IComparer<SampleDataElementNode> Members
			private readonly IComparer<SampleDataElementNode> myNodeComparer;
			private sealed class NodeComparerImpl : IComparer<SampleDataElementNode>
			{
				private MainList myParent;
				public NodeComparerImpl(MainList parent)
				{
					myParent = parent;
				}
				int IComparer<SampleDataElementNode>.Compare(SampleDataElementNode node1, SampleDataElementNode node2)
				{
					if (node1 == node2)
					{
						return 0;
					}
					object element1 = node1.Element;
					object element2 = node2.Element;
					List<SurveyQuestionDisplay> displays = myParent.myCurrentDisplays;
					int displayCount = displays.Count;
					int retVal = 0;
					int answers1 = node1.NodeData;
					int answers2 = node2.NodeData;
					for (int i = 0; i < displayCount && retVal == 0; ++i)
					{
						SurveyQuestionDisplay display = displays[i];
						if (0 != (display.Question.UISupport & (SurveyQuestionUISupport.Sorting)))
						{
							// UNDONE: AnswerOrder
							// int[] order = display.AnswerOrder;
							SurveyQuestion question = display.Question;
							int answer1 = question.ExtractAnswer(answers1);
							int answer2 = question.ExtractAnswer(answers2);
							if (answer1 != answer2)
							{
								if (answer1 == SurveyQuestion.NeutralAnswer)
								{
									retVal = display.NeutralOnTop ? -1 : 1;
								}
								else if (answer2 == SurveyQuestion.NeutralAnswer)
								{
									retVal = display.NeutralOnTop ? 1 : -1;
								}
								else if (answer1 < answer2)
								{
									retVal = -1;
								}
								else
								{
									retVal = 1;
								}
							}
						}
					}
					if (retVal == 0)
					{
						// Categorization left us hanging, fall back on the custom sort
						// followed by the survey name.
						ICustomComparableSurveyNode customCompare = element1 as ICustomComparableSurveyNode;
						if (customCompare != null)
						{
							retVal = customCompare.CompareToSurveyNode(element2);
						}
						if (retVal == 0 && null != (customCompare = element2 as ICustomComparableSurveyNode))
						{
							retVal = -customCompare.CompareToSurveyNode(element1);
						}
						if (retVal == 0)
						{
							retVal = String.CompareOrdinal(node1.SurveyName, node2.SurveyName);
						}
						if (retVal == 0)
						{
							retVal = unchecked(element1.GetHashCode() - element1.GetHashCode());
							if (retVal == 0)
							{
								// A little overkill, but the whole system breaks down if we
								// can't get a unique sort
								Microsoft.VisualStudio.Modeling.ModelElement modelElement1;
								Microsoft.VisualStudio.Modeling.ModelElement modelElement2;
								if (null != (modelElement1 = element1 as Microsoft.VisualStudio.Modeling.ModelElement) &&
									null != (modelElement2 = element2 as Microsoft.VisualStudio.Modeling.ModelElement))
								{
									retVal = modelElement1.Id.CompareTo(modelElement2.Id);
								}
								else
								{
									retVal = -1;
								}
							}
						}
					}
					return retVal;
				}
			}
			#endregion
			#endregion //end sort list methods

			#region IBranch Members
			/// <summary>
			/// Implements <see cref="IBranch.BeginLabelEdit"/>
			/// </summary>
			protected VirtualTreeLabelEditData BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				SampleDataElementNode editNode = myNodes[row];
				if (editNode.IsSurveyNameEditable)
				{
					return new VirtualTreeLabelEditData(editNode.EditableSurveyName);
				}
				return VirtualTreeLabelEditData.Invalid;
			}
			VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
			{
				return BeginLabelEdit(row, column, activationStyle);
			}

			/// <summary>
			/// Implements <see cref="IBranch.CommitLabelEdit"/>
			/// </summary>
			protected LabelEditResult CommitLabelEdit(int row, int column, string newText)
			{
				SampleDataElementNode editNode = myNodes[row];
				editNode.EditableSurveyName = newText;
				return LabelEditResult.AcceptEdit;
			}
			LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
			{
				return CommitLabelEdit(row, column, newText);
			}
			/// <summary>
			/// Implements <see cref="IBranch.Features"/>
			/// </summary>
			protected static BranchFeatures Features
			{
				get
				{
					return BranchFeatures.PositionTracking | BranchFeatures.InsertsAndDeletes | BranchFeatures.Expansions | BranchFeatures.ExplicitLabelEdits;
				}
			}
			BranchFeatures IBranch.Features
			{
				get
				{
					return Features;
				}
			}
			/// <summary>
			/// Implements <see cref="IBranch.GetAccessibilityData"/>
			/// </summary>
			protected static VirtualTreeAccessibilityData GetAccessibilityData(int row, int column)
			{
				return VirtualTreeAccessibilityData.Empty;
			}
			VirtualTreeAccessibilityData IBranch.GetAccessibilityData(int row, int column)
			{
				return GetAccessibilityData(row, column);
			}
			/// <summary>
			/// Implements <see cref="IBranch.GetDisplayData"/>
			/// </summary>
			protected VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				VirtualTreeDisplayData retVal = VirtualTreeDisplayData.Empty;
				int nodeData = myNodes[row].NodeData;
				int questionCount = mySurvey.Count;
				int image = -1;

				int overlayImage = -1;
				int overlayBitField = -1;
				for (int i = 0; i < questionCount; i++)
				{
					SurveyQuestion question = mySurvey[i];
					ISurveyQuestionTypeInfo questionInfo = question.Question;
					SurveyQuestionUISupport support = questionInfo.UISupport;
					if (0 != (support & (SurveyQuestionUISupport.Glyph | SurveyQuestionUISupport.Overlay | SurveyQuestionUISupport.DisplayData)))
					{
						int answer = myCurrentDisplays[i].Question.ExtractAnswer(nodeData);
						if (answer != SurveyQuestion.NeutralAnswer)
						{
							// Extract image and overlay support
							if (0 != (support & SurveyQuestionUISupport.Glyph))
							{
								if (image == -1 &&
									0 <= (image = questionInfo.MapAnswerToImageIndex(answer)))
								{
									image += question.ProviderImageListOffset;
								}
							}
							else if (0 != (support & SurveyQuestionUISupport.Overlay))
							{
								int answerImage = questionInfo.MapAnswerToImageIndex(answer);
								if (0 <= answerImage)
								{
									if (overlayImage == -1)
									{
										overlayImage = answerImage + question.ProviderImageListOffset;
									}
									else
									{
										if (overlayBitField == -1)
										{
											overlayBitField = 0;
											AddToOverlayList(overlayImage, ref overlayBitField);
										}
										AddToOverlayList(answerImage + question.ProviderImageListOffset, ref overlayBitField);
									}
								}
							}

							// Extract other display settings
							if (0 != (support & SurveyQuestionUISupport.DisplayData))
							{
								SurveyQuestionDisplayData displayData = questionInfo.GetDisplayData(answer);
								retVal.Bold = displayData.Bold;
								retVal.GrayText = displayData.GrayText;
							}
						}
					}
				}

				if (0 <= image)
				{
					retVal.ImageList = mySurvey.MainImageList;
					retVal.SelectedImage = retVal.Image = (short)image;
				}

				if (overlayBitField == -1)
				{
					retVal.OverlayIndex = (short)overlayImage;
				}
				else
				{
					retVal.OverlayIndices = myOverlayIndices;
					retVal.OverlayIndex = (short)overlayBitField;
				}

				return retVal;
			}
			private void AddToOverlayList(int answer, ref int overlayBitField)
			{
				int[] overlayIndices = myOverlayIndices;
				if (overlayIndices == null)
				{
					myOverlayIndices = overlayIndices = new int[8];
				}

				int curBit = 1;
				for (int i = 0; i < 8; ++i)
				{
					if ((overlayBitField & curBit) == 0)
					{
						overlayIndices[i] = answer;
						break;
					}

					curBit <<= 1;
				}

				overlayBitField |= curBit;
			}
			VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
			{
				return GetDisplayData(row, column, requiredData);
			}
			/// <summary>
			/// Implements <see cref="IBranch.GetObject"/>
			/// </summary>
			protected object GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				switch (style)
				{
					case ObjectStyle.ExpandedBranch:
						{
							SampleDataElementNode node = myNodes[row];
							object expansionKey;
							if (null != (expansionKey = node.SurveyNodeExpansionKey))
							{
								object element = node.Element;
								SurveyTree parent = mySurveyTree;
								MainList expansion = new MainList(parent, element, expansionKey);
								mySurveyTree.myMainListDictionary[element] = expansion;
								return expansion.CreateRootBranch();
							}
							break;
						}
					case ObjectStyle.TrackingObject:
						return myNodes[row].Element;
				}
				return null;
			}
			object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
			{
				return GetObject(row, column, style, ref options);
			}
			/// <summary>
			/// Implements <see cref="IBranch.GetText"/>
			/// </summary>
			/// <returns>The Display Name Of The Node</returns>
			protected string GetText(int row, int column)
			{
				return myNodes[row].SurveyName;
			}
			string IBranch.GetText(int row, int column)
			{
				return GetText(row, column);
			}
			/// <summary>
			/// Implements <see cref="IBranch.GetTipText"/>
			/// </summary>
			/// <returns>Returns The Display Text Of The Node</returns>
			protected string GetTipText(int row, int column, ToolTipType tipType)
			{
				return myNodes[row].SurveyName;

			}
			string IBranch.GetTipText(int row, int column, ToolTipType tipType)
			{
				return GetTipText(row, column, tipType);
			}
			/// <summary>
			/// Implements <see cref="IBranch.IsExpandable"/>
			/// </summary>
			protected bool IsExpandable(int row, int column)
			{
				return myNodes[row].SurveyNodeExpansionKey != null;
			}
			bool IBranch.IsExpandable(int row, int column)
			{
				return IsExpandable(row, column);
			}
			/// <summary>
			/// Implements <see cref="IBranch.LocateObject"/>
			/// </summary>
			protected LocateObjectData LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				if (style == ObjectStyle.TrackingObject)
				{
					NodeLocation location;
					int nodeIndex;
					ISurveyNodeContext nodeContext;
					object contextElement = null;
					if (mySurveyTree.myNodeDictionary.TryGetValue(obj, out location))
					{
						MainList locatedList = location.MainList;
						if (locatedList == this)
						{
							if (0 <= (nodeIndex = myNodes.BinarySearch(location.ElementNode, myNodeComparer)))
							{
								return new LocateObjectData(nodeIndex, 0, (int)TrackingObjectAction.ThisLevel);
							}
						}
						else
						{
							contextElement = locatedList.myContextElement;
						}
					}
					else if (null != (nodeContext = obj as ISurveyNodeContext))
					{
						contextElement = nodeContext.SurveyNodeContext;
					}
					if (contextElement != null)
					{
						// This item is in an expansion, see if we can find the context element at this level
						LocateObjectData contextData = LocateObject(contextElement, style, locateOptions);
						if (contextData.Row != VirtualTreeConstant.NullIndex)
						{
							contextData.Options = (int)TrackingObjectAction.NextLevel;
							return contextData;
						}
					}
				}
				return new LocateObjectData(VirtualTreeConstant.NullIndex, VirtualTreeConstant.NullIndex, 0);
			}
			LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
			{
				return LocateObject(obj, style, locateOptions);
			}

			/// <summary>
			/// Implements <see cref="IBranch.OnBranchModification"/>
			/// </summary>
			protected event BranchModificationEventHandler OnBranchModification
			{
				add
				{
					// Note: This assumes that the branch is attached to a single tree. If
					// it is attached to multiple trees, then we need to look at the invocation
					// list to see if we already have a listener attached for that tree, and
					// we need to track a count for each attached tree listener. Alternately,
					// only the top-level grouper should attach its events to this branch. All other
					// shifter/grouper implementations should not attach events.
					if (myModificationEvents == null)
					{
						myModificationEvents += value;
					}
					++myAttachedEventCount;
				}
				remove
				{
					if (--myAttachedEventCount == 0)
					{
						myModificationEvents = null;
					}
				}
			}
			event BranchModificationEventHandler IBranch.OnBranchModification
			{
				add
				{
					OnBranchModification += value;
				}
				remove
				{
					OnBranchModification -= value;
				}
			}
			/// <summary>
			/// Implements <see cref="IBranch.OnDragEvent"/>
			/// </summary>
			protected static void OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
			{
			}
			void IBranch.OnDragEvent(object sender, int row, int column, DragEventType eventType, DragEventArgs args)
			{
				OnDragEvent(sender, row, column, eventType, args);
			}
			/// <summary>
			/// Implements <see cref="IBranch.OnGiveFeedback"/>
			/// </summary>
			protected static void OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
			{
			}
			void IBranch.OnGiveFeedback(GiveFeedbackEventArgs args, int row, int column)
			{
				OnGiveFeedback(args, row, column);
			}
			/// <summary>
			/// Implements <see cref="IBranch.OnQueryContinueDrag"/>
			/// </summary>
			protected static void OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
			{
			}
			void IBranch.OnQueryContinueDrag(QueryContinueDragEventArgs args, int row, int column)
			{
				OnQueryContinueDrag(args, row, column);
			}
			/// <summary>
			/// Implements <see cref="IBranch.OnStartDrag"/>
			/// </summary>
			protected VirtualTreeStartDragData OnStartDrag(object sender, int row, int column, DragReason reason)
			{
				if (reason == DragReason.DragDrop)
				{
					object dataObject = myNodes[row].SurveyNodeDataObject;
					return (dataObject != null) ? new VirtualTreeStartDragData(dataObject, DragDropEffects.All) : VirtualTreeStartDragData.Empty;
				}
				return VirtualTreeStartDragData.Empty;
			}
			VirtualTreeStartDragData IBranch.OnStartDrag(object sender, int row, int column, DragReason reason)
			{
				return OnStartDrag(sender, row, column, reason);
			}
			/// <summary>
			/// Implements <see cref="IBranch.SynchronizeState"/>
			/// </summary>
			protected static StateRefreshChanges SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
			{
				return StateRefreshChanges.None;
			}
			StateRefreshChanges IBranch.SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
			{
				return SynchronizeState(row, column, matchBranch, matchRow, matchColumn);
			}
			/// <summary>
			/// Implements <see cref="IBranch.ToggleState"/>
			/// </summary>
			protected static StateRefreshChanges ToggleState(int row, int column)
			{
				return StateRefreshChanges.None;
			}
			StateRefreshChanges IBranch.ToggleState(int row, int column)
			{
				return ToggleState(row, column);
			}
			/// <summary>
			/// Returned by <see cref="IBranch.UpdateCounter"/>
			/// </summary>
			protected static int UpdateCounter
			{
				get
				{
					return 0;
				}
			}
			int IBranch.UpdateCounter
			{
				get
				{
					return UpdateCounter;
				}
			}
			/// <summary>
			/// Implements <see cref="IBranch.VisibleItemCount"/>
			/// </summary>
			protected int VisibleItemCount
			{
				get
				{
					return myNodes.Count;
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

			#region Notification methods, correspond to INotifySurveyElementChanged
			/// <summary>
			/// Forwarded from <see cref="INotifySurveyElementChanged.ElementAdded"/>
			/// </summary>
			public void ElementAdded(object element)
			{
				SampleDataElementNode newNode = new SampleDataElementNode(element);
				newNode.Initialize(mySurvey);
				int index;
				if (0 <= (index = myNodes.BinarySearch(newNode, myNodeComparer)))
				{
					// Don't add a node we already have or one we can't uniquely track
					return;
				}
				index = ~index;
				myNodes.Insert(index, newNode);
				mySurveyTree.myNodeDictionary[element] = new NodeLocation(this, newNode);
				if (myRootGrouper == null)
				{
					RaiseBranchEvent(BranchModificationEventArgs.InsertItems(this, index - 1, 1));
				}
				else
				{
					BranchModificationEventHandler forwardToHandler = myModificationEvents;
					LinkedList<BranchModificationEventArgs> forwardEvents = null;
					myRootGrouper.ElementAddedAt(
						index,
						(forwardToHandler == null) ?
							(BranchModificationEventHandler)null : delegate(object eventSender, BranchModificationEventArgs e)
							{
								if (forwardEvents == null)
								{
									forwardEvents = new LinkedList<BranchModificationEventArgs>();
								}
								forwardEvents.AddLast(e);
							});
					if (forwardEvents != null)
					{
						foreach (BranchModificationEventArgs args in forwardEvents)
						{
							forwardToHandler(this, args);
						}
					}
				}
			}
			/// <summary>
			/// Forwarded from <see cref="INotifySurveyElementChanged.ElementChanged"/>
			/// </summary>
			public void NodeChanged(SampleDataElementNode node, params Type[] questionTypes)
			{
				int index;
				if (0 > (index = myNodes.BinarySearch(node, myNodeComparer)))
				{
					return;
				}
				node.Update(questionTypes, mySurvey);
				myNodes[index] = node;
				mySurveyTree.myNodeDictionary[node.Element] = new NodeLocation(this, node);

				BranchModificationEventHandler modificationEvents = myModificationEvents;
				if (myRootGrouper != null)
				{
					myRootGrouper.ElementChangedAt(index, modificationEvents);
				}
				else if (modificationEvents != null)
				{
					modificationEvents(this, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.Image, this, index, 0, 1)));
				}

			}
			/// <summary>
			/// Forwarded from <see cref="INotifySurveyElementChanged.ElementDeleted"/>
			/// </summary>
			public void NodeDeleted(SampleDataElementNode node)
			{
				int index;
				if (0 > (index = myNodes.BinarySearch(node, myNodeComparer)))
				{
					return;
				}
				// Note that the dictionary is cleaned up in the SurveyTree's calling code
				myNodes.RemoveAt(index);
				if (myRootGrouper != null)
				{
					BranchModificationEventHandler forwardToHandler = myModificationEvents;
					LinkedList<BranchModificationEventArgs> forwardEvents = null;
					myRootGrouper.ElementDeletedAt(
						index,
						(forwardToHandler == null) ?
							(BranchModificationEventHandler)null :
							delegate(object eventSender, BranchModificationEventArgs e)
							{
								if (forwardEvents == null)
								{
									forwardEvents = new LinkedList<BranchModificationEventArgs>();
								}
								forwardEvents.AddLast(e);
							});
					if (forwardEvents != null)
					{
						foreach (BranchModificationEventArgs args in forwardEvents)
						{
							forwardToHandler(this, args);
						}
					}
				}
				else
				{
					RaiseBranchEvent(BranchModificationEventArgs.DeleteItems(this, index, 1));
				}
			}
			/// <summary>
			/// Forwarded from <see cref="INotifySurveyElementChanged.ElementRenamed"/>
			/// </summary>
			public void NodeRenamed(SampleDataElementNode node)
			{
				int fromIndex;
				if (0 > (fromIndex = myNodes.BinarySearch(node, myNodeComparer)))
				{
					return;
				}
				object element = node.Element;
				SampleDataElementNode newNode = new SampleDataElementNode(element, node.NodeData);
				int toIndex = myNodes.BinarySearch(newNode, myNodeComparer);
				int inverseToIndex = ~toIndex;
				BranchModificationEventHandler modificationEvents = myModificationEvents;
				if (fromIndex == toIndex || (inverseToIndex >= 0 && (inverseToIndex == fromIndex || (inverseToIndex - fromIndex) == 1)))
				{
					myNodes[fromIndex] = newNode;
					mySurveyTree.myNodeDictionary[element] = new NodeLocation(this, newNode);
					if (myRootGrouper != null)
					{
						myRootGrouper.ElementRenamedAt(fromIndex, fromIndex, modificationEvents);
					}
					else if (modificationEvents != null)
					{
						modificationEvents(this, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.Text, this, fromIndex, 0, 1)));
					}
				}
				else if (inverseToIndex >= 0)
				{
					myNodes.RemoveAt(fromIndex);
					if (inverseToIndex > fromIndex)
					{
						--inverseToIndex;
					}
					myNodes.Insert(inverseToIndex, newNode);
					mySurveyTree.myNodeDictionary[element] = new NodeLocation(this, newNode);
					if (myRootGrouper != null)
					{
						myRootGrouper.ElementRenamedAt(fromIndex, inverseToIndex, modificationEvents);
					}
					else if (modificationEvents != null)
					{
						modificationEvents(this, BranchModificationEventArgs.MoveItem(this, fromIndex, inverseToIndex));
					}
				}
				else
				{
					Debug.Assert(false);
				}
			}
			#endregion //INotifySurveyElementChanged Implementation
		}
	}
}