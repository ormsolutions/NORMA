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
			List<SampleDataElementNode> nodes = myNodes = new List<SampleDataElementNode>();
			foreach(ISurveyNodeProvider nodeProvider in nodeProviderList)
			{
				int count = 0;
				foreach (SampleDataElementNode elementNode in nodeProvider.GetSurveyNodes())
				{
					SampleDataElementNode tempElementNode = elementNode;
					tempElementNode.Index = nodes.Count + count;
					nodes.Add(tempElementNode);
					++count;
				}
			}
			Survey survey = mySurvey = new Survey(questionProviderList);
			List<SurveyQuestionDisplay> currentDisplays = myCurrentDisplays = new List<SurveyQuestionDisplay>();
			int surveyCount = survey.Count;
			for (int i = 0; i < surveyCount; ++i)
			{
				currentDisplays.Add(new SurveyQuestionDisplay(survey[i]));
			}
			survey.ProcessNodes(nodes);
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
					return this;
				}
				else
				{
					return new ListGrouper(this, mySurvey[0], 0, myNodes.Count - 1, myCurrentDisplays[0].NeutralOnTop);
				}
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
		/// <summary>
		/// This tells you which groups are used for sorting and grouping. The order
		/// here gives you the sort order (which question is primary, secondary, etc).
		/// </summary>
		public void SortRespondents()
		{
			myNodes.Sort(
				delegate (SampleDataElementNode node1, SampleDataElementNode node2)
				{
					int displayCount = myCurrentDisplays.Count;
					int retVal = 0;
					int answers1 = node1.NodeData;
					int answers2 = node2.NodeData;
					for (int i = 0; i < displayCount && retVal == 0; ++i)
					{
						SurveyQuestionDisplay display = myCurrentDisplays[i];
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
							#region currently not working CONTROL SORT ORDER
							//else if (order != null)
							//{
							//    if (order[answer1] < order[answer2])
							//    {
							//        retVal = -1;
							//    }
							//    else
							//    {
							//        retVal = 1;
							//    }
							//}
							#endregion
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
					return retVal;
				});
			for (int i = 0; i < myNodes.Count; ++i)
			{
				SampleDataElementNode node = myNodes[i];
				node.Index = i;
				myNodes[i] = node;
			}
		}
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
		protected const BranchFeatures FeaturesResult = BranchFeatures.PositionTracking;
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
			return (style == ObjectStyle.TrackingObject) ? (object)myNodes[row] : null;
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
			// Do nothing. We never raise this event, so we don't need to keep track of who is subscribed to it.
			add
			{
			}
			remove
			{
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
			return OnStartDragResult;
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
		}
		/// <summary><see cref="MainList"/>'s implementation of this does nothing.</summary>
		void INotifySurveyElementChanged.ElementChanged(object sender, ISurveyQuestionTypeInfo[] questions)
		{
		}
		/// <summary><see cref="MainList"/>'s implementation of this does nothing.</summary>
		void INotifySurveyElementChanged.ElementDeleted(object sender)
		{
		}
		/// <summary><see cref="MainList"/>'s implementation of this does nothing.</summary>
		void INotifySurveyElementChanged.ElementRenamed(object sender)
		{
		}
		#endregion //INotifySurveyElementChanged Members
	}
}
