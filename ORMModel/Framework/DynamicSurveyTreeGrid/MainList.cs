using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Microsoft.VisualStudio.VirtualTreeGrid;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORM.Framework.DynamicSurveyTreeGrid
{
	/// <summary>
	/// main branch provider for a SurveyTree, implements IBranch, main branch can be retrieved from RootBranch
	/// </summary>
	public class MainList : IBranch, INotifySurveyElementChanged
	{
		#region survey question display struct
		//TODO: ask Matt, probably want to move this inside of the survey along with myCurrentDisplays, and have the survey manage it all
		private struct SurveyQuestionDisplay
		{
			public SurveyQuestion Question;
			/// <summary>
			/// Limited to Sorting | Grouping
			/// </summary>
			public SurveyQuestionUISupport CurrentGrouping;
			/// <summary>
			/// Allows reordering of answers in the display
			/// </summary>
			public int[] AnswerOrder;
			public bool NeutralOnTop;
			public SurveyQuestionDisplay(SurveyQuestion question)
			{
				Question = question;
				CurrentGrouping = new SurveyQuestionUISupport(); // Choose Sorting or Grouping
				AnswerOrder = new int[Question.CategoryCount];
				NeutralOnTop = true;
			}
		}
		#endregion //survey question display struct
		#region .ctor and instance variables
		List<SampleDataElementNode> myNodes;
		private Survey mySurvey;
		private List<SurveyQuestionDisplay> myCurrentDisplays;
		/// <summary>
		/// public constructor
		/// </summary>
		/// <param name="nodeProviderList">enumeration of ISurveyNodeProviders</param>
		/// <param name="questionProviderList">enumeration of ISurveyQuestionProvider</param>
		public MainList(IEnumerable<ISurveyNodeProvider> nodeProviderList, IEnumerable<ISurveyQuestionProvider> questionProviderList)
		{
			this.myNodes = new List<SampleDataElementNode>();
			foreach(ISurveyNodeProvider nodeProvider in nodeProviderList)
			{
				int count = 0;
				IEnumerable<SampleDataElementNode> nodeEnumeration = nodeProvider.GetSurveyNodes();
				foreach (SampleDataElementNode elementNode in nodeEnumeration)
				{
					SampleDataElementNode tempElement = elementNode;
					tempElement.Index = myNodes.Count + count;
					myNodes.Add(tempElement);
					++count;
				}
			}
			mySurvey = new Survey(questionProviderList);
			myCurrentDisplays = new List<SurveyQuestionDisplay>();
			for (int i = 0; i < mySurvey.Count; ++i)
			{
				SurveyQuestionDisplay display = new SurveyQuestionDisplay(mySurvey[i]);
				myCurrentDisplays.Add(display);
			}
			mySurvey.ProcessNodes(myNodes);
		}
		#endregion //.ctor and instance variables.
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
					return new ListGrouper(this, mySurvey[0], 0, myNodes.Count - 1,myCurrentDisplays[0].NeutralOnTop);
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
		/// Implements IBranch.BeginLabelEdit
		/// </summary>
		/// <returns>An Invalid Edit Data</returns>
		protected static VirtualTreeLabelEditData BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
		{
			return VirtualTreeLabelEditData.Invalid;
		}
		VirtualTreeLabelEditData IBranch.BeginLabelEdit(int row, int column, VirtualTreeLabelEditActivationStyles activationStyle)
		{
			return BeginLabelEdit(row, column, activationStyle);
		}

		/// <summary>
		/// Implements IBranch.CommitLabelEdit
		/// </summary>
		/// <returns>Cancels The Edit</returns>
		protected static LabelEditResult CommitLabelEdit(int row, int column, string newText)
		{
			return LabelEditResult.CancelEdit;
		}
		LabelEditResult IBranch.CommitLabelEdit(int row, int column, string newText)
		{
			return CommitLabelEdit(row, column, newText);
		}
		/// <summary>
		/// Implements IBranch.Features
		/// </summary>
		protected static BranchFeatures Features
		{
			get { return BranchFeatures.PositionTracking; }
		}
		BranchFeatures IBranch.Features
		{
			get { return Features; }
		}
		/// <summary>
		/// Implements IBranch.GetAccessiblityData
		/// </summary>
		/// <returns>Empty</returns>
		protected static VirtualTreeAccessibilityData GetAccessibilityData(int row, int column)
		{
			return VirtualTreeAccessibilityData.Empty;
		}
		VirtualTreeAccessibilityData IBranch.GetAccessibilityData(int row, int column)
		{
			return GetAccessibilityData(row, column);
		}
		/// <summary>
		/// Implements IBranch.GetDisplayData
		/// </summary>
		/// <returns>Empty</returns>
		protected static VirtualTreeDisplayData GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
		{
			return VirtualTreeDisplayData.Empty;
		}
		VirtualTreeDisplayData IBranch.GetDisplayData(int row, int column, VirtualTreeDisplayDataMasks requiredData)
		{
			return GetDisplayData(row,column,requiredData);
		}

		/// <summary>
		/// Implements IBranch.GetObject
		/// </summary>
		/// <param name="row">Index of The Object</param>
		/// <param name="column"></param>
		/// <param name="options"></param>
		/// <param name="style"></param>
		/// <returns>Object</returns>
		protected object GetObject(int row, int column, ObjectStyle style, ref int options)
		{
			if(style == ObjectStyle.TrackingObject)
				return myNodes[row];
			return null;
		}
		object IBranch.GetObject(int row, int column, ObjectStyle style, ref int options)
		{
			return GetObject(row, column, style, ref options);
		}
		/// <summary>
		/// Implements IBranch.GetText
		/// </summary>
		/// <param name="row">index of the Node</param>
		/// <param name="column">not currently used, 0</param>
		/// <returns>The Display Name Of The Node</returns>
		protected string GetText(int row, int column)
		{
			return myNodes[row].ElementName;
		}
		string IBranch.GetText(int row, int column)
		{
			return GetText(row, column);
		}
		/// <summary>
		/// Implements IBranch.GetTipText
		/// </summary>
		/// <param name="row">index Of The Node</param>
		/// <param name="column">not currently used, 0</param>
		/// <param name="tipType"></param>
		/// <returns>Returns The Display Text Of The Node</returns>
		protected string GetTipText(int row, int column, ToolTipType tipType)
		{
			return myNodes[row].ElementName;
		}
		string IBranch.GetTipText(int row, int column, ToolTipType tipType)
		{
			return GetTipText(row, column, tipType);
		}
		/// <summary>
		/// Implements IBranch.IsExpandable
		/// </summary>
		/// <returns>False</returns>
		protected static bool IsExpandable(int row, int column)
		{
			return false;
		}
		bool IBranch.IsExpandable(int row, int column)
		{
			return IsExpandable(row, column);
		}
		/// <summary>
		/// Implementing IBranch.LocateObject
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="style"></param>
		/// <param name="locateOptions"></param>
		/// <returns>New LocateObjectData</returns>
		protected LocateObjectData LocateObject(object obj, ObjectStyle style, int locateOptions)
		{
			if (style == ObjectStyle.TrackingObject)
			{
				for (int i = 0; i < myNodes.Count; ++i)
				{
					if (((SampleDataElementNode)obj).Equals(myNodes[i]))
					{
						return new LocateObjectData(i, 0, 0);
					}
				}
			}
			return new LocateObjectData(-1, 0, locateOptions);
		}
		LocateObjectData IBranch.LocateObject(object obj, ObjectStyle style, int locateOptions)
		{
			return LocateObject(obj, style, locateOptions);
		}
		///<summary>
		/// Implementation of IBranch.OnBranchModification
		/// </summary>
		protected event BranchModificationEventHandler OnBranchModification;
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
		/// Implementation of IBranch.OnDragEvent
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="row"></param>
		/// <param name="column"></param>
		/// <param name="eventType"></param>
		/// <param name="args"></param>
		protected static void OnDragEvent(object sender, int row, int column, DragEventType eventType, System.Windows.Forms.DragEventArgs args)
		{

		}
		void IBranch.OnDragEvent(object sender, int row, int column, DragEventType eventType, System.Windows.Forms.DragEventArgs args)
		{
			OnDragEvent(sender, row, column, eventType, args);
		}
		/// <summary>
		/// Implements IBranch.OnGiveFeedback
		/// </summary>
		/// <param name="args"></param>
		/// <param name="row"></param>
		/// <param name="column"></param>
		protected static void OnGiveFeedback(System.Windows.Forms.GiveFeedbackEventArgs args, int row, int column)
		{
		}
		void IBranch.OnGiveFeedback(System.Windows.Forms.GiveFeedbackEventArgs args, int row, int column)
		{
			OnGiveFeedback(args,row, column);
		}
		/// <summary>
		/// Implements IBranch.OnQueryContinueDrag
		/// </summary>
		/// <param name="args"></param>
		/// <param name="row"></param>
		/// <param name="column"></param>
		protected static void OnQueryContinueDrag(System.Windows.Forms.QueryContinueDragEventArgs args, int row, int column)
		{
		}
		void IBranch.OnQueryContinueDrag(System.Windows.Forms.QueryContinueDragEventArgs args, int row, int column)
		{
			OnQueryContinueDrag(args, row, column);
		}
		/// <summary>
		/// Implements IBranch.OnStartDrag
		/// </summary>
		/// <returns>Empty</returns>
		protected static VirtualTreeStartDragData OnStartDrag(object sender, int row, int column, DragReason reason)
		{
			return VirtualTreeStartDragData.Empty;
		}
		VirtualTreeStartDragData IBranch.OnStartDrag(object sender, int row, int column, DragReason reason)
		{
			return OnStartDrag(sender, row, column, reason);
		}
		/// <summary>
		/// Implements IBranch.SynchronizeState
		/// </summary>
		/// <returns>None</returns>
		protected static StateRefreshChanges SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
		{
			return StateRefreshChanges.None;
		}
		StateRefreshChanges IBranch.SynchronizeState(int row, int column, IBranch matchBranch, int matchRow, int matchColumn)
		{
			return SynchronizeState(row, column, matchBranch, matchRow, matchColumn);
		}
		/// <summary>
		/// Implements IBranch.ToggleState
		/// </summary>
		/// <returns>None</returns>
		protected static StateRefreshChanges ToggleState(int row, int column)
		{
			return StateRefreshChanges.None;
		}
		StateRefreshChanges IBranch.ToggleState(int row, int column)
		{
			return ToggleState(row, column);
		}
		/// <summary>
		/// Implements IBranch.UpdateCounter
		/// </summary>
		protected static int UpdateCounter
		{
			get { return 0; }
		}
		int IBranch.UpdateCounter
		{
			get { return UpdateCounter; }
		}
		/// <summary>
		/// Implements IBranch.VisibleItemCount
		/// </summary>
		protected int VisibleItemCount
		{
			get { return myNodes.Count; }
		}
		int IBranch.VisibleItemCount
		{
			get { return VisibleItemCount; }
		}

		#endregion

		#region INotifySurveyElementChanged Members
		/// <summary>
		/// called when an element is added to a node provider
		/// </summary>
		/// <param name="sender">object that was added</param>
		protected static void ElementAdded(object sender)
		{
		}
		void INotifySurveyElementChanged.ElementAdded(object sender)
		{
			ElementAdded(sender);
		}
		/// <summary>
		/// called when an element in a node provider is changed
		/// </summary>
		/// <param name="sender">object that was changed</param>
		/// <param name="questions">array of questions that need to be reasked of this node</param>
		protected static void ElementChanged(object sender, ISurveyQuestionTypeInfo[] questions)
		{
		}
		void INotifySurveyElementChanged.ElementChanged(object sender, ISurveyQuestionTypeInfo[] questions)
		{
			ElementChanged(sender,questions);
		}
		/// <summary>
		/// called when an element in a node provider is removed
		/// </summary>
		/// <param name="sender">object that was removed</param>
		protected static void ElementRemoved(object sender)
		{
		}
		void INotifySurveyElementChanged.ElementRemoved(object sender)
		{
			ElementRemoved(sender);
		}
		/// <summary>
		/// called when an element in a node provider is renamed
		/// </summary>
		/// <param name="sender">object that was renamed</param>
		protected static void ElementRenamed(object sender)
		{
		}
		void INotifySurveyElementChanged.ElementRenamed(object sender)
		{
			ElementRenamed(sender);
		}

		#endregion //INotifySurveyElementChanged Members
	}
}
