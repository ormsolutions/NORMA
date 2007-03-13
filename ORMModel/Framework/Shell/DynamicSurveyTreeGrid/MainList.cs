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
	/// <summary>
	/// main branch provider for a SurveyTree, implements IBranch, main branch can be retrieved from RootBranch
	/// </summary>
	public partial class MainList : IBranch, INotifySurveyElementChanged
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
			/// <summary>
			/// Allows reordering of answers in the display
			/// </summary>
			public readonly int[] AnswerOrder;
			public readonly bool NeutralOnTop;
			public SurveyQuestionDisplay(SurveyQuestion question)
			{
				Question = question;
				CurrentGrouping = new SurveyQuestionUISupport(); // Choose Sorting or Grouping

				AnswerOrder = new int[Question.CategoryCount];
				NeutralOnTop = true;
			}
		}
		#endregion //survey question display struct

		#region Constructor and instance fields
		private readonly List<SampleDataElementNode> myNodes;
		private readonly Survey mySurvey;
		private readonly List<SurveyQuestionDisplay> myCurrentDisplays;
		private BranchModificationEventHandler myModificationEvents;
		private int myAttachedEventCount;
		private Dictionary<object, SampleDataElementNode> myDictionary;
		private ListGrouper myRootGrouper;
		private ImageList myImageList;
		/// <summary>
		/// Public constructor
		/// </summary>
		public MainList(IEnumerable<ISurveyNodeProvider> nodeProviderList, IEnumerable<ISurveyQuestionProvider> questionProviderList)
		{
			if (nodeProviderList == null)
			{
				throw new ArgumentNullException("nodeProviderList");
			}

			if (questionProviderList == null)
			{
				throw new ArgumentNullException("questionProviderList");
			}
			myNodeComparer = new NodeComparerImpl(this);
			List<SampleDataElementNode> nodes = new List<SampleDataElementNode>();
			myNodes = nodes;
			foreach (ISurveyNodeProvider nodeProvider in nodeProviderList)
			{
				foreach (object elementNode in nodeProvider.GetSurveyNodes())
				{
					nodes.Add(new SampleDataElementNode(elementNode));
				}
			}
			Survey survey = new Survey(questionProviderList);
			mySurvey = survey;
			myImageList = mySurvey.MainImageList;
			List<SurveyQuestionDisplay> currentDisplays = new List<SurveyQuestionDisplay>();
			myCurrentDisplays = currentDisplays;
			int surveyCount = survey.Count;
			for (int i = 0; i < surveyCount; ++i)
			{
				currentDisplays.Add(new SurveyQuestionDisplay(survey[i]));
			}
			SampleDataElementNode.InitializeNodes(nodes, survey);
			nodes.Sort(myNodeComparer);
			myDictionary = new Dictionary<object, SampleDataElementNode>();
			foreach (SampleDataElementNode node in myNodes)
			{
				myDictionary.Add(node.Element, node);
			}
		}
		#endregion // Constructor and instance fields

		#region root branch and survey properties
		/// <summary>
		/// provides the RootBranch for a SurveyTree
		/// </summary>
		public IBranch RootBranch
		{
			get
			{
				if (mySurvey.Count == 0)
				{
					myRootGrouper = null;
					return this;
				}
				else
				{
					return myRootGrouper = new ListGrouper(this, mySurvey[0], 0, myNodes.Count - 1, myCurrentDisplays[0].NeutralOnTop);
				}
			}
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
						int[] order = display.AnswerOrder;
						SurveyQuestion question = display.Question;
						int neutralAnswer = question.Mask >> question.Shift;
						int answer1 = question.ExtractAnswer(answers1);
						int answer2 = question.ExtractAnswer(answers2);
						if (answer1 != answer2)
						{
							if (answer1 == neutralAnswer)
							{
								retVal = display.NeutralOnTop ? -1 : 1;
							}
							else if (answer2 == neutralAnswer)
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
					retVal = String.CompareOrdinal(node1.SurveyName, node2.SurveyName);
					if (retVal == 0 && node1.Element != node2.Element)
					{
						object element1 = node1.Element;
						object element2 = node2.Element;
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
			VirtualTreeDisplayData returnData = new VirtualTreeDisplayData();
			returnData.ImageList = myImageList;
			int nodeData = myNodes[row].NodeData;
			int questionCount = mySurvey.Count;
			short image = 0;
			for (int i = 0; i < questionCount; i++)
			{
				SurveyQuestion question = mySurvey[i];
				if (0 != (question.Question.UISupport & SurveyQuestionUISupport.Glyph))
				{
					int answer = myCurrentDisplays[i].Question.ExtractAnswer(nodeData);
					image = (short)question.Question.MapAnswerToImageIndex(answer);
					image = (short)(image - question.ProviderImageListOffset);
				}

			}
			returnData.Image = image;
			returnData.SelectedImage = image;
			returnData.OverlayIndex = -1; //TODO: ask Matt about overlays.
			return returnData;
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
			return (style == ObjectStyle.TrackingObject) ? myNodes[row].Element : null;
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
		protected static bool IsExpandable(int row, int column)
		{
			return false;
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
				SampleDataElementNode node;
				int nodeIndex;
				if (myDictionary.TryGetValue(obj, out node) &&
					0 <= (nodeIndex = myNodes.BinarySearch(node, myNodeComparer)))
				{
					return new LocateObjectData(nodeIndex, 0, (int)TrackingObjectAction.ThisLevel);
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

		#region INotifySurveyElementChanged Implementation
		/// <summary>
		/// Implements <see cref="INotifySurveyElementChanged.ElementAdded"/>
		/// </summary>
		protected void ElementAdded(object element)
		{
			SampleDataElementNode newNode = new SampleDataElementNode(element);
			newNode.Initialize(mySurvey);
			int index;
			if (myDictionary.ContainsKey(element) ||
				0 <= (index = myNodes.BinarySearch(newNode, myNodeComparer)))
			{
				// Don't add a node we already have or one we can't uniquely track
				return;
			}
			index = ~index;
			myNodes.Insert(index, newNode);
			myDictionary[element] = newNode;
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
		void INotifySurveyElementChanged.ElementAdded(object element)
		{
			ElementAdded(element);
		}
		/// <summary>
		/// Implements <see cref="INotifySurveyElementChanged.ElementChanged"/>
		/// </summary>
		/// <param name="element">the object that has been changed</param>
		/// <param name="questionTypes">The question types.</param>
		protected void ElementChanged(object element, params Type[] questionTypes)
		{
			SampleDataElementNode node;
			int index;
			if (!myDictionary.TryGetValue(element, out node) || 0 > (index = myNodes.BinarySearch(node, myNodeComparer)))
			{
				return;
			}
			node.Update(questionTypes, mySurvey);
			myNodes[index] = node;
			myDictionary[element] = node;

			BranchModificationEventHandler modificationEvents = myModificationEvents;
			if (myRootGrouper != null)
			{
				myRootGrouper.ElementChangedAt(modificationEvents, index);
			}
			else if (modificationEvents != null)
			{
				modificationEvents(this, BranchModificationEventArgs.DisplayDataChanged(new DisplayDataChangedData(VirtualTreeDisplayDataChanges.Image, this, index, 0, 1)));
			}
		
		}
		void INotifySurveyElementChanged.ElementChanged(object element, params Type[] questionTypes)
		{
			ElementChanged(element, questionTypes);
		}
		/// <summary>
		/// Implements <see cref="INotifySurveyElementChanged.ElementDeleted"/>
		/// </summary>
		protected void ElementDeleted(object element)
		{
			SampleDataElementNode node;
			int index;
			if (!myDictionary.TryGetValue(element, out node) ||
				0 > (index = myNodes.BinarySearch(node, myNodeComparer)))
			{
				return;
			}
			myDictionary.Remove(element);
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
		void INotifySurveyElementChanged.ElementDeleted(object element)
		{
			ElementDeleted(element);
		}
		/// <summary>
		/// Implements <see cref="INotifySurveyElementChanged.ElementRenamed"/>
		/// </summary>
		protected void ElementRenamed(object element)
		{
			SampleDataElementNode node;
			int fromIndex;
			if (!myDictionary.TryGetValue(element, out node) ||
				0 > (fromIndex = myNodes.BinarySearch(node, myNodeComparer)))
			{
				return;
			}
			SampleDataElementNode newNode = new SampleDataElementNode(element, node.NodeData);
			int toIndex = myNodes.BinarySearch(newNode, myNodeComparer);
			int inverseToIndex = ~toIndex;
			BranchModificationEventHandler modificationEvents = myModificationEvents;
			if (fromIndex == toIndex || (inverseToIndex >= 0 && (inverseToIndex == fromIndex || (inverseToIndex - fromIndex) == 1)))
			{
				myNodes[fromIndex] = newNode;
				myDictionary[element] = newNode;
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
				myDictionary[element] = newNode;
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
		void INotifySurveyElementChanged.ElementRenamed(object element)
		{
			ElementRenamed(element);
		}
		#endregion //INotifySurveyElementChanged Implementation
	}
}

