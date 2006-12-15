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
using System.Threading;
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
			Survey survey = mySurvey = new Survey(questionProviderList);
			List<SurveyQuestionDisplay> currentDisplays = myCurrentDisplays = new List<SurveyQuestionDisplay>();
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
			if (myModificationEvents != null)
			{
				myModificationEvents(this, e);
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
				if (retVal == 0)
				{
					retVal = String.CompareOrdinal(node1.SurveyName, node2.SurveyName);
					if (retVal == 0)
					{
						retVal = unchecked(node1.Element.GetHashCode() - node2.Element.GetHashCode());
					}
				}
				return retVal;
			}
		}
		#endregion
		#endregion //end sort list methods

		#region IBranch Members
		/// <summary>
		/// Returned by IBranch.BeginLabelEdit
		/// </summary>
		protected static readonly VirtualTreeLabelEditData BeginLabelEditResult = VirtualTreeLabelEditData.Invalid;
		VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
		{
			return BeginLabelEditResult;
		}

		/// <summary>
		/// Returned by IBranch.CommitLabelEdit
		/// </summary>
		protected const LabelEditResult CommitLabelEditResult = LabelEditResult.CancelEdit;
		LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
		{
			return CommitLabelEditResult;
		}
		/// <summary>
		/// Returned by IBranch.Features
		/// </summary>
		protected const BranchFeatures FeaturesResult = BranchFeatures.PositionTracking | BranchFeatures.InsertsAndDeletes | BranchFeatures.Expansions;
		BranchFeatures IBranch.Features
		{
			get
			{
				return FeaturesResult;
			}
		}
		/// <summary>
		/// Returned by IBranch.GetAccessiblityData
		/// </summary>
		protected static readonly VirtualTreeAccessibilityData GetAccessibilityDataResult = VirtualTreeAccessibilityData.Empty;
		VirtualTreeAccessibilityData IBranch.GetAccessibilityData(int row, int column)
		{
			return GetAccessibilityDataResult;
		}
		/// <summary>
		/// Returned by IBranch.GetDisplayData
		/// </summary>
		protected static readonly VirtualTreeDisplayData GetDisplayDataResult = VirtualTreeDisplayData.Empty;
		VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
		{
			return GetDisplayDataResult;
		}

		/// <summary>
		/// Implements IBranch.GetObject
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
		/// Implements IBranch.GetText
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
		/// Implements IBranch.GetTipText
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
		/// Returned by IBranch.IsExpandable
		/// </summary>
		protected const bool IsExpandableResult = false;
		bool IBranch.IsExpandable(int row, int column)
		{
			return IsExpandableResult;
		}
		/// <summary>
		/// Implements IBranch.LocateObject
		/// </summary>
		protected LocateObjectData LocateObject(object obj, ObjectStyle style, int locateOptions)
		{
			if (style == ObjectStyle.TrackingObject)
			{
				int nodeIndex = myNodes.IndexOf((SampleDataElementNode)obj);
				if (nodeIndex >= 0)
				{
					return new LocateObjectData(nodeIndex, 0, locateOptions);
				}
			}
			return new LocateObjectData(VirtualTreeConstant.NullIndex, VirtualTreeConstant.NullIndex, locateOptions);
		}
		LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
		{
			return LocateObject(obj, style, locateOptions);
		}

		/// <summary><see cref="MainList"/>'s implementation of this does nothing.</summary>
		event BranchModificationEventHandler IBranch.OnBranchModification
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
		/// <summary><see cref="MainList"/>'s implementation of this does nothing.</summary>
		void IBranch.OnDragEvent(object sender, int row, int column, DragEventType eventType, System.Windows.Forms.DragEventArgs args)
		{
		}
		/// <summary><see cref="MainList"/>'s implementation of this does nothing.</summary>
		void IBranch.OnGiveFeedback(System.Windows.Forms.GiveFeedbackEventArgs args, int row, int column)
		{
		}
		/// <summary><see cref="MainList"/>'s implementation of this does nothing.</summary>
		void IBranch.OnQueryContinueDrag(System.Windows.Forms.QueryContinueDragEventArgs args, int row, int column)
		{
		}
		/// <summary>
		/// Returned by IBranch.OnStartDrag
		/// </summary>
		protected static readonly VirtualTreeStartDragData OnStartDragResult = VirtualTreeStartDragData.Empty;
		VirtualTreeStartDragData IBranch.OnStartDrag(object sender, int row, int column, DragReason reason)
		{
			object dataObject = myNodes[row].SurveyNodeDataObject;
			return (dataObject != null) ? new VirtualTreeStartDragData(dataObject, DragDropEffects.All) : VirtualTreeStartDragData.Empty;
		}
		/// <summary>
		/// Returned by IBranch.SynchronizeState
		/// </summary>
		protected const StateRefreshChanges SynchronizeStateResult = StateRefreshChanges.None;
		StateRefreshChanges IBranch.SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
		{
			return SynchronizeStateResult;
		}
		/// <summary>
		/// Returned by IBranch.ToggleState
		/// </summary>
		protected const StateRefreshChanges ToggleStateResult = StateRefreshChanges.None;
		StateRefreshChanges IBranch.ToggleState(int row, int column)
		{
			return ToggleStateResult;
		}
		/// <summary>
		/// Returned by IBranch.UpdateCounter
		/// </summary>
		protected const int UpdateCounterResult = 0;
		int IBranch.UpdateCounter
		{
			get
			{
				return UpdateCounterResult;
			}
		}
		/// <summary>
		/// Implements IBranch.VisibleItemCount
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

		#region INotifySurveyElementChanged Members
		/// <summary><see cref="MainList"/>'s implementation of this does nothing.</summary>
		void INotifySurveyElementChanged.ElementAdded(object sender)
		{

			SampleDataElementNode newNode = new SampleDataElementNode(sender);
			if (myModificationEvents != null)
			{
				newNode.Initialize(mySurvey);
				if (!myDictionary.ContainsKey(newNode.Element))
				{
					myDictionary.Add(newNode.Element, newNode);
				}
				else
				{
					return;
				}
				int index = myNodes.BinarySearch(newNode, myNodeComparer);
				index = ~index;
				myNodes.Insert(index, newNode);
				if (myRootGrouper == null)
				{
					RaiseBranchEvent(BranchModificationEventArgs.InsertItems(this, index - 1, 1));
				}
				else
				{
					BranchModificationEventHandler forwardToHandler = myModificationEvents;
					LinkedList<BranchModificationEventArgs> forwardEvents = null;
					myRootGrouper.ElementAdded(
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
			}
		}
		/// <summary><see cref="MainList"/>'s implementation of this does nothing.</summary>
		void INotifySurveyElementChanged.ElementChanged(object sender, ISurveyQuestionTypeInfo[] questions)
		{
		}
		/// <summary><see cref="MainList"/>'s implementation of this does nothing.</summary>
		void INotifySurveyElementChanged.ElementDeleted(object sender)
		{
			SampleDataElementNode node;
			if (myModificationEvents != null)
			{
				if (myDictionary.ContainsKey(sender))
				{
					node = myDictionary[sender];
				}
				else
				{
					return;
				}
				int index = myNodes.BinarySearch(node, myNodeComparer);
				if (index >= 0)
				{
					myDictionary.Remove(sender);
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
			}
		}
		void INotifySurveyElementChanged.ElementRenamed(object sender)
		{
			SampleDataElementNode node;
			int from = 0;
			int to = 0;

			if (myModificationEvents != null)
			{
				if (myDictionary.ContainsKey(sender))
				{
					node = myDictionary[sender];
				}
				else
				{
					return;
				}
				from = myNodes.IndexOf(node);
				myNodes.Remove(node);
				to = myNodes.BinarySearch(node, myNodeComparer);
				to = ~to;
				myNodes.Insert(to, node);
				if (from >= 0)
				{
					if (myRootGrouper != null)
					{
						myRootGrouper.ElementRenamedAt(from, to, myModificationEvents);
					}
				}
			}
		}

	}
		#endregion //INotifySurveyElementChanged Members
}

