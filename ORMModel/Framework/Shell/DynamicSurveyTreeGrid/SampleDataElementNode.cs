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
using Microsoft.VisualStudio.Modeling;
using System.Diagnostics;

namespace Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid
{
	public partial class MainList
	{
		/// <summary>
		/// wrapper for objects to be dispalyed in the Survey Tree
		/// </summary>
		[DebuggerDisplay("{myDisplayText} {(myElement != null) ? myElement.GetType().Name : @\"\"\"\"}")]
		public struct SampleDataElementNode : IEquatable<SampleDataElementNode>
		{
			private readonly object myElement;
			private int myNodeData;
			private string myDisplayText; // Cache this so it is fast and stable over time
			/// <summary>
			/// public constructor
			/// </summary>
			public SampleDataElementNode(object element)
				: this(element, 0)
			{
			}
			/// <summary>
			/// public constructor
			/// </summary>
			public SampleDataElementNode(object element, int nodeData)
			{
				if (element == null)
				{
					throw new ArgumentNullException("element");
				}
				myElement = element;
				myNodeData = nodeData;
				ISurveyNode node = element as ISurveyNode;
				myDisplayText = (node != null) ? node.SurveyName : element.ToString();
			}
			/// <summary>
			/// returns object wrapped by this node
			/// </summary>
			public object Element
			{
				get
				{
					return myElement;
				}
			}
			/// <summary>
			/// gets or sets the integer that holds the answers to all questions in myNodeCachedQuestions
			/// </summary>
			public int NodeData
			{
				get
				{
					return myNodeData;
				}
				private set
				{
					myNodeData = value;
				}
			}
			#region ISurveyNode property wrappers
			/// <summary>
			/// returns name of the wrapped element
			/// </summary>
			public string SurveyName
			{
				get
				{
					return myDisplayText;
				}
			}
			/// <summary>
			/// for edit mode returns the name the user can change
			/// </summary>
			public string EditableSurveyName
			{
				get
				{
					ISurveyNode node = myElement as ISurveyNode;
					string retVal = (node != null) ? node.EditableSurveyName : null;
					return (retVal != null) ? retVal : myDisplayText;
				}
				set
				{
					ISurveyNode node = myElement as ISurveyNode;
					if (node != null)
					{
						node.EditableSurveyName = value;
					}
				}
			}
			/// <summary>
			/// returns whether or not this object can be edited.
			/// </summary>
			public bool IsSurveyNameEditable
			{
				get
				{
					ISurveyNode node = myElement as ISurveyNode;
					return node != null && node.IsSurveyNameEditable;
				}
			}
			/// <summary>
			/// Returns the data object for this node
			/// </summary>
			public object SurveyNodeDataObject
			{
				get
				{
					ISurveyNode node = myElement as ISurveyNode;
					return (node != null) ? node.SurveyNodeDataObject : null;
				}
			}
			#endregion

			#region Infrastructure Methods

			/// <summary>See <see cref="Object.GetHashCode"/>.</summary>
			public override int GetHashCode()
			{
				return ((this.myElement != null) ? this.myElement.GetHashCode() : 0) ^ this.myNodeData;
			}
			/// <summary>See <see cref="Object.Equals(Object)"/>.</summary>
			public override bool Equals(object obj)
			{
				return obj is SampleDataElementNode && this.Equals((SampleDataElementNode)obj);
			}
			/// <summary>See <see cref="IEquatable{SampleDataElementNode}.Equals"/>.</summary>
			public bool Equals(SampleDataElementNode other)
			{
				return this.myElement == other.myElement && this.myNodeData == other.myNodeData;
			}
			/// <summary>
			/// Returns whether <param name="left"/> is equal to <param name="right"/>, based on the
			/// <see cref="Equals(SampleDataElementNode)"/> method.
			/// </summary>
			public static bool operator ==(SampleDataElementNode left, SampleDataElementNode right)
			{
				return left.Equals(right);
			}
			/// <summary>
			/// Returns whether <param name="left"/> is not equal to <param name="right"/>, based on the
			/// <see cref="Equals(SampleDataElementNode)"/> method.
			/// </summary>
			public static bool operator !=(SampleDataElementNode left, SampleDataElementNode right)
			{
				return !left.Equals(right);
			}

			#endregion // Infrastructure Methods
			#region InitializeNodes
			/// <summary>
			/// cycles through all SampleDataElementNodes in the list and creates their node data based on the answer to this survey's questions
			/// </summary>
			/// <param name="nodeList">List of elements to initialize</param>
			/// <param name="survey">The <see cref="Survey"/>s to initialize the node for</param>
			/// <returns></returns>
			public static void InitializeNodes(IList<SampleDataElementNode> nodeList, Survey survey)
			{
				int questionCount = survey.Count;
				int nodeCount = nodeList.Count;
				for (int i = 0; i < nodeCount; ++i)
				{
					SampleDataElementNode currentNode = nodeList[i];
					int data = 0;
					object nodeElement = currentNode.Element;
					for (int j = 0; j < questionCount; ++j)
					{
						SurveyQuestion currentQuestion = survey[j];
							int currentAnswer = currentQuestion.Question.AskQuestion(nodeElement);
							data |= (currentAnswer << currentQuestion.Shift) & currentQuestion.Mask;
						
					}
					currentNode.NodeData = data;
					nodeList[i] = currentNode;
				}
			}
			/// <summary>
			/// Processes the current node
			/// </summary>
			/// <param name="survey">The <see cref="Survey"/>s to initialize the node for</param>
			public void Initialize(Survey survey)
			{
				int data = 0;
				int questionCount = survey.Count;
				object nodeElement = Element;
				for (int i = 0; i < questionCount; ++i)
				{
					SurveyQuestion currentQuestion = survey[i];
					int currentAnswer = currentQuestion.Question.AskQuestion(nodeElement);
					data |= (currentAnswer << currentQuestion.Shift) & currentQuestion.Mask;
				}
				NodeData = data;
			}
			/// <summary>
			/// Updates the current node data
			/// </summary>
			/// <param name="questionTypes">types of the questions that are affected by  the change</param>
			/// <param name="survey">survey</param>
			public void Update(Type[] questionTypes, Survey survey)
			{
				int data = myNodeData;
				int questionCount = questionTypes.Length;
				object nodeElement = Element;
				for (int i = 0; i < questionCount; ++i)
				{
					SurveyQuestion currentQuestion = survey[questionTypes[i]];
					int currentAnswer = currentQuestion.Question.AskQuestion(nodeElement);
					data &= ~currentQuestion.Mask;
					data |= (currentAnswer << currentQuestion.Shift) & currentQuestion.Mask;
				}
				NodeData = data;
			}
			#endregion // InitializeNodes
		}
	}
}