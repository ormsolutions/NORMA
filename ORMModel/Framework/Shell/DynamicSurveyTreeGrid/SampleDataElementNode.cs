#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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

namespace ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid
{
	partial class SurveyTree<SurveyContextType>
	{
		/// <summary>
		/// wrapper for objects to be displayed in the Survey Tree
		/// </summary>
		[DebuggerDisplay("{myDisplayText} {(myElement != null) ? myElement.GetType().Name : @\"\"\"\"}")]
		private struct SampleDataElementNode : IEquatable<SampleDataElementNode>
		{
			#region Member Variables
			private readonly object myElement;
			private readonly object myCustomSortData; // Cache this so we can do a custom sort based on a snapshot, not current state
			private readonly string myDisplayText; // Cache this so it is fast and stable over time
			private int myNodeData;
			#endregion // Member Variables
			#region Public Create and Update Methods
			/// <summary>
			/// Create a new <see cref="SampleDataElementNode"/>
			/// </summary>
			/// <param name="surveyTree">The owning <see cref="SurveyTree{SurveyContextType}"/> container</param>
			/// <param name="survey">The <see cref="Survey"/> this node is associated with</param>
			/// <param name="contextElement">The parent element, or null for a root expansion context</param>
			/// <param name="element">The element to create a node for</param>
			/// <param name="verifyReferences">Set to <see langword="true"/> to verify link information.</param>
			/// <returns>A fully initialized <see cref="SampleDataElementNode"/>.</returns>
			public static SampleDataElementNode Create(SurveyTree<SurveyContextType> surveyTree, Survey survey, object contextElement, object element, bool verifyReferences)
			{
				return new SampleDataElementNode(surveyTree, contextElement, element, InitializeNodeData(element, contextElement, survey), null, null, verifyReferences);
			}
			/// <summary>
			/// Refetch the current settings of the <see cref="SurveyName"/> property
			/// </summary>
			/// <param name="surveyTree">The owning <see cref="SurveyTree{SurveyContextType}"/> container</param>
			/// <param name="contextElement">The parent element, or null for a root expansion context</param>
			/// <returns>A new <see cref="SampleDataElementNode"/>. This node is not modified.</returns>
			public SampleDataElementNode UpdateSurveyName(SurveyTree<SurveyContextType> surveyTree, object contextElement)
			{
				return new SampleDataElementNode(surveyTree, contextElement, myElement, myNodeData, null, myCustomSortData, false);
			}
			/// <summary>
			/// Refetch the current settings of the <see cref="SurveyName"/> property
			/// </summary>
			/// <param name="surveyTree">The owning <see cref="SurveyTree{SurveyContextType}"/> container</param>
			/// <param name="contextElement">The parent element, or null for a root expansion context</param>
			/// <param name="customSortData">Updated sort data</param>
			/// <returns>A new <see cref="SampleDataElementNode"/>. This node is not modified.</returns>
			public SampleDataElementNode UpdateCustomSortData(SurveyTree<SurveyContextType> surveyTree, object contextElement, object customSortData)
			{
				return new SampleDataElementNode(surveyTree, contextElement, myElement, myNodeData, myDisplayText, customSortData, false);
			}
			#endregion // Public and Update Methods
			#region Constructors
			/// <summary>
			/// Private constructor
			/// </summary>
			private SampleDataElementNode(SurveyTree<SurveyContextType> surveyTree, object contextElement, object element, int nodeData, string displayText, object customSortData, bool verifyReferences)
			{
				if (element == null)
				{
					throw new ArgumentNullException("element");
				}
				myElement = element;
				myNodeData = nodeData;
				ISurveyNodeReference reference = element as ISurveyNodeReference;
				object referencedElement = (reference == null) ? null : reference.ReferencedElement;
				if (verifyReferences && referencedElement != null)
				{
					surveyTree.VerifyReferenceTarget(referencedElement);
				}
				if (displayText == null)
				{
					object textElement = element;
					ISurveyNode node = textElement as ISurveyNode;
					if (node == null ||
						null == (displayText = node.SurveyName))
					{
						if (null == referencedElement ||
							null == (node = (textElement = referencedElement) as ISurveyNode) ||
							null == (displayText = node.SurveyName))
						{
							displayText = textElement.ToString();
						}
					}
				}
				myDisplayText = displayText;
				myCustomSortData = customSortData;
				if (null == customSortData)
				{
					ICustomComparableSurveyNode customCompare = element as ICustomComparableSurveyNode;
					if (null != customCompare)
					{
						customCompare.ResetCustomSortData(contextElement, ref myCustomSortData);
					}
				}

				if (verifyReferences && referencedElement != null)
				{
					// Add a link relating the target element to this node. Note that we're fully constructed at this point
					Dictionary<object, LinkedNode<SurveyNodeReference>> links = surveyTree.myReferenceDictionary;
					LinkedNode<SurveyNodeReference> newLink = new LinkedNode<SurveyNodeReference>(new SurveyNodeReference(this, contextElement));
					LinkedNode<SurveyNodeReference> existingLink;
					if (links.TryGetValue(referencedElement, out existingLink))
					{
						newLink.SetNext(existingLink, ref existingLink);
					}
					links[referencedElement] = newLink;
				}
			}
			#endregion // Constructors
			#region Accessor Properties
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
			/// <summary>
			/// Get any custom sort data associated with this node
			/// </summary>
			public object CustomSortData
			{
				get
				{
					return myCustomSortData;
				}
			}
			/// <summary>
			/// Return the directly implemented <see cref="ISurveyNode"/> with
			/// the <see cref="ISurveyNode"/> implemented by a <see cref="ISurveyNodeReference"/>
			/// target element as a fallback.
			/// </summary>
			private ISurveyNode ResolvedSurveyNode
			{
				get
				{
					object element = myElement;
					ISurveyNode node;
					ISurveyNodeReference reference;
					if (null == (node = element as ISurveyNode) &&
						null != (reference = element as ISurveyNodeReference))
					{
						node = reference.ReferencedElement as ISurveyNode;
					}
					return node;
				}
			}
			#endregion // Accessor Properties
			#region ISurveyNode property wrappers
			/// <summary>
			/// The cached text for the displayed element
			/// </summary>
			public string SurveyName
			{
				get
				{
					return myDisplayText;
				}
			}
			/// <summary>
			/// An undecorated, editable name
			/// </summary>
			public string EditableSurveyName
			{
				get
				{
					ISurveyNode node = ResolvedSurveyNode;
					return (node != null) ?	(node.EditableSurveyName ?? myDisplayText) : myDisplayText;
				}
				set
				{
					ISurveyNode node = ResolvedSurveyNode;
					if (node != null)
					{
						node.EditableSurveyName = value;
					}
				}
			}
			/// <summary>
			/// True if the element supports name editing
			/// </summary>
			public bool IsSurveyNameEditable
			{
				get
				{
					ISurveyNode node = ResolvedSurveyNode;
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
					ISurveyNode node = ResolvedSurveyNode;
					return (node != null) ? node.SurveyNodeDataObject : null;
				}
			}
			/// <summary>
			/// Returns the expansion key for this node
			/// </summary>
			public object SurveyNodeExpansionKey
			{
				get
				{
					// Note that this does not defer to a node reference. However,
					// if this is a node reference and returns an expansion key, then
					// the reference's target/reason pair is treated as a primary element
					// and must be unique.
					ISurveyNode node = myElement as ISurveyNode;
					return (node != null) ? node.SurveyNodeExpansionKey : null;
				}
			}
			#endregion
			#region Infrastructure Methods
			/// <summary>See <see cref="Object.GetHashCode"/>.</summary>
			public override int GetHashCode()
			{
				object element = myElement;
				ISurveyNodeReference reference;
				if (element == null)
				{
					return myNodeData;
				}
				else if (null != (reference = element as ISurveyNodeReference))
				{
					// For node references, we do not care about full equality of the
					// instances, only about a match for the target, reason, and nodedata.
					// Do not defer to the full GetHashCode method on reference elements.
					return Utility.GetCombinedHashCode(reference.ReferencedElement.GetHashCode(), reference.SurveyNodeReferenceReason.GetHashCode(), myNodeData);
				}
				return Utility.GetCombinedHashCode(element.GetHashCode(), myNodeData);
			}
			/// <summary>See <see cref="Object.Equals(Object)"/>.</summary>
			public override bool Equals(object obj)
			{
				return obj is SampleDataElementNode && this.Equals((SampleDataElementNode)obj);
			}
			/// <summary>See <see cref="IEquatable{SampleDataElementNode}.Equals"/>.</summary>
			public bool Equals(SampleDataElementNode other)
			{
				object element = myElement;
				object otherElement = other.Element;
				ISurveyNodeReference reference;
				ISurveyNodeReference otherReference;
				if (null != (reference = element as ISurveyNodeReference))
				{
					if (null != (otherReference = otherElement as ISurveyNodeReference))
					{
						return reference.ReferencedElement == otherReference.ReferencedElement &&
							reference.SurveyNodeReferenceReason == otherReference.SurveyNodeReferenceReason &&
							myNodeData == other.myNodeData &&
							myDisplayText == other.myDisplayText &&
							object.Equals(myCustomSortData, other.myCustomSortData);
					}
					return false;
				}
				else if (null != (otherReference = otherElement as ISurveyNodeReference))
				{
					return false;
				}
				return element == otherElement &&
					myNodeData == other.myNodeData &&
					myDisplayText == other.myDisplayText &&
					object.Equals(myCustomSortData, other.myCustomSortData);
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
			#region NodeData Population
			/// <summary>
			/// Initialize the node answer cache
			/// </summary>
			/// <param name="element">The element to query</param>
			/// <param name="contextElement">The context element for this node</param>
			/// <param name="survey">The <see cref="Survey"/>s to initialize the node for</param>
			/// <returns>Packed bitfield integer containing all current answers</returns>
			private static int InitializeNodeData(object element, object contextElement, Survey survey)
			{
				int data = 0;
				int questionCount = survey.Count;
				for (int i = 0; i < questionCount; ++i)
				{
					SurveyQuestion currentQuestion = survey[i];
					int currentAnswer = currentQuestion.Question.AskQuestion(element, contextElement);
					data |= (currentAnswer << currentQuestion.Shift) & currentQuestion.Mask;
				}
				return data;
			}
			/// <summary>
			/// Updates the current node data
			/// </summary>
			/// <param name="questionTypes">types of the questions that are affected by  the change</param>
			/// <param name="contextElement">The context element for this node</param>
			/// <param name="survey">survey</param>
			public void Update(Type[] questionTypes, object contextElement, Survey survey)
			{
				int data = myNodeData;
				int questionCount = questionTypes.Length;
				object nodeElement = Element;
				for (int i = 0; i < questionCount; ++i)
				{
					SurveyQuestion currentQuestion = survey[questionTypes[i]];
					if (currentQuestion != null)
					{
						int currentAnswer = currentQuestion.Question.AskQuestion(nodeElement, contextElement);
						data &= ~currentQuestion.Mask;
						data |= (currentAnswer << currentQuestion.Shift) & currentQuestion.Mask;
					}
				}
				NodeData = data;
			}
			#endregion // NodeData Population
		}
	}
}