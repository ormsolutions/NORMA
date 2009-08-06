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
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualStudio.VirtualTreeGrid;
using System.Drawing;

namespace ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid
{
	/// <summary>
	/// SurveyTree class. Used to create a tree that can be used with
	/// a <see cref="SurveyTreeContainer"/>
	/// </summary>
	public partial class SurveyTree<SurveyContextType> : INotifySurveyElementChanged
		where SurveyContextType : class
	{
		#region NodeLocation structure
		/// <summary>
		/// Store the location of a primary element, keyed off that element.
		/// If a reference is created to a node before the node is displayed
		/// in its own list, then this 'location' simply caches the <see cref="Survey"/>
		/// information for the node.
		/// </summary>
		private struct NodeLocation
		{
			private object myContext;
			/// <summary>
			/// The context node
			/// </summary>
			public SampleDataElementNode ElementNode;
			/// <summary>
			/// Place a node in an expanded list
			/// </summary>
			/// <param name="list">The <see cref="MainList"/> that is the primary location for this node</param>
			/// <param name="node">The node itself</param>
			public NodeLocation(MainList list, SampleDataElementNode node)
			{
				ElementNode = node;
				myContext = list;
			}
			/// <summary>
			/// Record a node without placing it in an explicit list
			/// </summary>
			/// <param name="survey">The <see cref="Survey"/> to interpret the source data</param>
			/// <param name="node">The node itself</param>
			public NodeLocation(Survey survey, SampleDataElementNode node)
			{
				ElementNode = node;
				myContext = survey;
			}
			/// <summary>
			/// The list where this element is located. Can be null if the element is a reference that is not yet
			/// included in an expanded branch.
			/// </summary>
			public MainList MainList
			{
				get
				{
					return myContext as MainList;
				}
			}
			/// <summary>
			/// The <see cref="Survey"/> used to interpet the element data.
			/// </summary>
			public Survey Survey
			{
				get
				{
					return myContext as Survey ?? ((MainList)myContext).QuestionList;
				}
			}
		}
		#endregion // NodeLocation structure
		#region LinkedNode class
		/// <summary>
		/// A simple linked list node class. LinkedList{} is too hard to modify during iteration,
		/// and a LinkedListNode{} requires a LinkedList.
		/// </summary>
		private class LinkedNode<T>
		{
			private T myValue;
			private LinkedNode<T> myNext;
			private LinkedNode<T> myPrev;
			public LinkedNode(T value)
			{
				myValue = value;
			}
			/// <summary>
			/// Set the next element
			/// </summary>
			/// <param name="next">Next element. If next has a previous element, then the head of the next element is inserted.</param>
			/// <param name="head">Reference to head node</param>
			public void SetNext(LinkedNode<T> next, ref LinkedNode<T> head)
			{
				Debug.Assert(next != null);
				if (next.myPrev != null)
				{
					next.myPrev.SetNext(GetHead(), ref head);
					return;
				}
				if (myNext != null)
				{
					myNext.myPrev = next.GetTail();
				}
				if (myPrev == null)
				{
					head = this;
				}
				myNext = next;
				next.myPrev = this;
			}
			/// <summary>
			/// The value passed to the constructor or set directly
			/// </summary>
			public T Value
			{
				get
				{
					return myValue;
				}
				set
				{
					myValue = value;
				}
			}
			/// <summary>
			/// Get the next node
			/// </summary>
			public LinkedNode<T> Next
			{
				get
				{
					return myNext;
				}
			}
			/// <summary>
			/// Get the previous node
			/// </summary>
			public LinkedNode<T> Previous
			{
				get
				{
					return myPrev;
				}
			}
			/// <summary>
			/// Get the head element in the linked list
			/// </summary>
			public LinkedNode<T> GetHead()
			{
				LinkedNode<T> retVal = this;
				LinkedNode<T> prev;
				while (null != (prev = retVal.myPrev))
				{
					retVal = prev;
				}
				return retVal;
			}
			/// <summary>
			/// Get the tail element in the linked list
			/// </summary>
			public LinkedNode<T> GetTail()
			{
				LinkedNode<T> retVal = this;
				LinkedNode<T> next;
				while (null != (next = retVal.myNext))
				{
					retVal = next;
				}
				return retVal;
			}
			/// <summary>
			/// Detach the current node
			/// </summary>
			/// <param name="headNode"></param>
			public void Detach(ref LinkedNode<T> headNode)
			{
				if (myPrev == null)
				{
					headNode = myNext;
				}
				else
				{
					myPrev.myNext = myNext;
				}
				if (myNext != null)
				{
					myNext.myPrev = myPrev;
				}
				myNext = null;
				myPrev = null;
			}
		}
		#endregion // LinkedNode class
		#region SurveyNodeReference struct
		/// <summary>
		/// A structure representing a reference to a primary element
		/// </summary>
		private struct SurveyNodeReference
		{
			private SampleDataElementNode myNode;
			private object myContextElement;
			/// <summary>
			/// Create a new <see cref="SurveyNodeReference"/>
			/// </summary>
			/// <param name="node">The node corresponding to this reference element</param>
			/// <param name="contextElement">The context element that includes this instance</param>
			public SurveyNodeReference(SampleDataElementNode node, object contextElement)
			{
				myNode = node;
				myContextElement = contextElement;
			}
			/// <summary>
			/// Retrieve the <see cref="SampleDataElementNode"/> within the expansion for the current <see cref="ContextElement"/>
			/// </summary>
			public SampleDataElementNode Node
			{
				get
				{
					return myNode;
				}
			}
			/// <summary>
			/// The referenced element
			/// </summary>
			public object ReferencedElement
			{
				get
				{
					return ((ISurveyNodeReference)myNode.Element).ReferencedElement;
				}
			}
			/// <summary>
			/// The reason for the node reference
			/// </summary>
			public object ReferenceReason
			{
				get
				{
					return ((ISurveyNodeReference)myNode.Element).SurveyNodeReferenceReason;
				}
			}
			/// <summary>
			/// The container element that owns the reference
			/// </summary>
			public object ContextElement
			{
				get
				{
					return myContextElement;
				}
			}
		}
		#endregion // SurveyNodeReference struct
		#region Member Variables
		/// <summary>
		/// An expansionKey to use instead of the public <see langword="null"/> representation of
		/// the top level element grouping.
		/// </summary>
		private static readonly object TopLevelExpansionKey = new object();
		/// <summary>
		/// The main context object for the survey tree
		/// </summary>
		private SurveyContextType mySurveyContext;
		/// <summary>
		/// The location of the standard link overlay image in the image list
		/// </summary>
		private const int LinkOverlayImageIndex = 0;
		/// <summary>
		/// The node providers, used to retrieve elements for initial set population
		/// </summary>
		private readonly ISurveyNodeProvider[] myNodeProviders;
		/// <summary>
		/// The question providers, used to determine which questions to ask and how they should be used
		/// </summary>
		private readonly ISurveyQuestionProvider<SurveyContextType>[] myQuestionProviders;
		/// <summary>
		/// Map primary elements to the expanded lists associated with those elements
		/// </summary>
		private readonly Dictionary<object, MainList> myMainListDictionary;
		/// <summary>
		/// Map primary elements to the list they're contained in
		/// </summary>
		private readonly Dictionary<object, NodeLocation> myNodeDictionary;
		/// <summary>
		/// Map expansion keys to cached survey information
		/// </summary>
		private readonly Dictionary<object, Survey> mySurveyDictionary;
		/// <summary>
		/// Track the links referencing a primary element
		/// </summary>
		private readonly Dictionary<object, LinkedNode<SurveyNodeReference>> myReferenceDictionary;
		/// <summary>
		/// The composite image list for all question providers
		/// </summary>
		private readonly ImageList myImageList;
		/// <summary>
		/// The provider offsets into the consolidated image list
		/// </summary>
		private readonly int[] myQuestionProviderImageOffsets;
		/// <summary>
		/// A set of all question types that are used for error display
		/// </summary>
		private Type[] myErrorDisplayTypes;
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// Public constructor
		/// </summary>
		public SurveyTree(SurveyContextType surveyContext, ISurveyNodeProvider[] nodeProviders, ISurveyQuestionProvider<SurveyContextType>[] questionProviders)
		{
			mySurveyContext = surveyContext;
			myNodeProviders = nodeProviders ?? new ISurveyNodeProvider[0];
			myQuestionProviders = questionProviders ?? new ISurveyQuestionProvider<SurveyContextType>[0];
			myMainListDictionary = new Dictionary<object, MainList>();
			myNodeDictionary = new Dictionary<object, NodeLocation>();
			mySurveyDictionary = new Dictionary<object, Survey>();
			myReferenceDictionary = new Dictionary<object, LinkedNode<SurveyNodeReference>>();
			int questionProviderCount;
			ImageList compositeImageList = new ImageList();
			compositeImageList.ColorDepth = ColorDepth.Depth32Bit;
			ImageList.ImageCollection compositeImages = compositeImageList.Images;
			Type resourceType = typeof(SurveyTree<>);
			Image overlayImage = Image.FromStream(resourceType.Assembly.GetManifestResourceStream(resourceType, "LinkOverlay.png"), true, true);
			compositeImages.Add(overlayImage); // Note that the link overlay position corresponds to LinkOverlayImageIndex
			if (questionProviders != null &&
				0 != (questionProviderCount = questionProviders.Length))
			{
				int imageOffset = 1; // Standard overlay image is first
				int[] providerImageOffsets = new int[questionProviderCount];
				for (int i = 0; i < questionProviderCount; ++i)
				{
					ImageList[] currentImageLists;
					if (null != (currentImageLists = questionProviders[i].GetSurveyQuestionImageLists(surveyContext)))
					{
						bool haveProviderOffset = false;
						foreach (ImageList currentImageList in currentImageLists)
						{
							ImageList.ImageCollection currentImages;
							int currentCount;
							if (0 != (currentCount = (currentImages = currentImageList.Images).Count))
							{
								for (int j = 0; j < currentCount; ++j)
								{
									compositeImages.Add(currentImages[j]);
								}
								if (!haveProviderOffset)
								{
									haveProviderOffset = true;
									providerImageOffsets[i] = imageOffset;
								}
								imageOffset += currentCount;
							}
						}
						if (!haveProviderOffset)
						{
							providerImageOffsets[i] = -1;
						}
					}
				}
				myQuestionProviderImageOffsets = providerImageOffsets;
			}
			else
			{
				myQuestionProviderImageOffsets = new int[0];
			}
			myImageList = compositeImageList;
		}
		#endregion // Constructor
		#region Accessor Properties
		/// <summary>
		/// Provides the RootBranch for a <see cref="SurveyTree{SurveyContextType}"/>
		/// </summary>
		public IBranch CreateRootBranch()
		{
			MainList mainList;
			if (!myMainListDictionary.TryGetValue(TopLevelExpansionKey, out mainList))
			{
				myMainListDictionary.Add(TopLevelExpansionKey, mainList = new MainList(this, null, null));
			}
			return mainList.GetRootBranch(true);
		}
		/// <summary>
		/// Update the error display for the specified element.
		/// </summary>
		public void UpdateErrorDisplay(object element)
		{
			this.ElementChanged(element, ErrorDisplayTypes);
		}
		private Type[] ErrorDisplayTypes
		{
			get
			{
				Type[] retVal = myErrorDisplayTypes;
				if (retVal == null)
				{
					List<Type> displayTypes = new List<Type>();
					foreach (ISurveyQuestionProvider<SurveyContextType> questionProvider in myQuestionProviders)
					{
						IEnumerable<Type> providerDisplayTypes = questionProvider.GetErrorDisplayTypes();
						if (providerDisplayTypes != null)
						{
							displayTypes.AddRange(providerDisplayTypes);
						}
					}
					myErrorDisplayTypes = retVal = displayTypes.ToArray();
				}
				return retVal;
			}
		}
		#endregion // Accessor Properties
		#region Methods
		/// <summary>
		/// Retrieve or create the survey associated with the provided <paramref name="expansionKey"/>
		/// </summary>
		private Survey GetSurvey(object expansionKey)
		{
			Survey retVal;
			Dictionary<object, Survey> dictionary = mySurveyDictionary;
			object key = expansionKey ?? TopLevelExpansionKey;
			if (!dictionary.TryGetValue(key, out retVal))
			{
				retVal = new Survey(mySurveyContext, myQuestionProviders, myQuestionProviderImageOffsets, expansionKey);
				dictionary[key] = retVal;
			}
			return retVal;
		}
		/// <summary>
		/// Determine if an element is expandable for a given expansion key
		/// </summary>
		private bool GetExpandable(object element, object expansionKey)
		{
			if (expansionKey == null)
			{
				ISurveyNode surveyNode;
				if (null == (surveyNode = element as ISurveyNode) ||
					null == (expansionKey = surveyNode.SurveyNodeExpansionKey))
				{
					return false;
				}
			}
			foreach (ISurveyNodeProvider nodeProvider in myNodeProviders)
			{
				if (nodeProvider.IsSurveyNodeExpandable(element, expansionKey))
				{
					return true;
				}
			}
			return false;
		}
		#endregion // Methods
		#region INotifySurveyElementChanged Implementation
		/// <summary>
		/// Implements <see cref="INotifySurveyElementChanged.ElementAdded"/>
		/// </summary>
		protected void ElementAdded(object element, object contextElement)
		{
			MainList list;
			if (myMainListDictionary.TryGetValue((contextElement == null) ? TopLevelExpansionKey : contextElement, out list))
			{
				list.ElementAdded(element);
			}
		}
		void INotifySurveyElementChanged.ElementAdded(object element, object contextElement)
		{
			ElementAdded(element, contextElement);
		}
		/// <summary>
		/// Implements <see cref="INotifySurveyElementChanged.ElementChanged"/>
		/// </summary>
		protected void ElementChanged(object element, params Type[] questionTypes)
		{
			NodeLocation location;
			if (myNodeDictionary.TryGetValue(element, out location))
			{
				MainList notifyList = location.MainList;
				if (notifyList != null)
				{
					notifyList.NodeChanged(location.ElementNode, null, false, questionTypes);
				}
				else
				{
					SampleDataElementNode node = location.ElementNode;
					Survey survey = location.Survey;
					ISurveyNodeContext contextElement = element as ISurveyNodeContext;
					node.Update(questionTypes, contextElement != null ? contextElement.SurveyNodeContext : null, survey);
					myNodeDictionary[element] = new NodeLocation(survey, node);
				}
				LinkedNode<SurveyNodeReference> linkNode;
				if (myReferenceDictionary.TryGetValue(element, out linkNode))
				{
					while (linkNode != null)
					{
						SurveyNodeReference link = linkNode.Value;
						if (myMainListDictionary.TryGetValue(link.ContextElement ?? TopLevelExpansionKey, out notifyList))
						{
							notifyList.NodeChanged(linkNode.Value.Node, linkNode, true, questionTypes);
						}
						linkNode = linkNode.Next;
					}
				}
			}
		}
		void INotifySurveyElementChanged.ElementChanged(object element, params Type[] questionTypes)
		{
			ElementChanged(element, questionTypes);
		}
		/// <summary>
		/// Implements <see cref="INotifySurveyElementChanged.ElementExpandabilityChanged"/>
		/// </summary>
		protected void ElementExpandibilityChanged(object element)
		{
			NodeLocation location;
			MainList existingExpansionList;
			ISurveyNode surveyNode;
			object expansionKey;
			// If we have a list for this node and the list still has elements, then
			// ignore this notification. We will automatically check the expandability
			// setting when the last item is removed from the list.
			if ((!myMainListDictionary.TryGetValue(element, out existingExpansionList) ||
				((IBranch)existingExpansionList).VisibleItemCount == 0) &&
				myNodeDictionary.TryGetValue(element, out location) &&
				null != (surveyNode = element as ISurveyNode) &&
				null != (expansionKey = surveyNode.SurveyNodeExpansionKey))
			{
				bool expandable = false;
				bool haveExpandable = false;

				MainList notifyList = location.MainList;
				if (notifyList != null)
				{
					if (!haveExpandable)
					{
						haveExpandable = true;
						expandable = GetExpandable(element, expansionKey);
					}
					notifyList.NodeExpandabilityChanged(location.ElementNode, existingExpansionList, expandable);
				}
				if (existingExpansionList != null)
				{
					myMainListDictionary.Remove(element);
				}
				else
				{
					LinkedNode<SurveyNodeReference> linkNode;
					if (myReferenceDictionary.TryGetValue(element, out linkNode))
					{
						while (linkNode != null)
						{
							SurveyNodeReference link = linkNode.Value;
							if (myMainListDictionary.TryGetValue(link.ContextElement ?? TopLevelExpansionKey, out notifyList))
							{
								SampleDataElementNode referenceNode = linkNode.Value.Node;
								ISurveyNodeReference reference;
								if (null != (reference = referenceNode.Element as ISurveyNodeReference) &&
									0 != (reference.SurveyNodeReferenceOptions & SurveyNodeReferenceOptions.InlineExpansion))
								{
									if (!haveExpandable)
									{
										haveExpandable = true;
										expandable = GetExpandable(expansionKey, element);
									}
									notifyList.NodeExpandabilityChanged(referenceNode, existingExpansionList, expandable);
								}
							}
							linkNode = linkNode.Next;
						}
					}
				}
			}
		}
		void INotifySurveyElementChanged.ElementExpandabilityChanged(object element)
		{
			ElementExpandibilityChanged(element);
		}
		/// <summary>
		/// Implements <see cref="INotifySurveyElementChanged.ElementReferenceChanged"/>
		/// </summary>
		void ElementReferenceChanged(object element, object referenceReason, object contextElement, params Type[] questionTypes)
		{
			LinkedNode<SurveyNodeReference> linkNode;
			MainList notifyList;
			if (myMainListDictionary.TryGetValue(contextElement ?? TopLevelExpansionKey, out notifyList) &&
				myReferenceDictionary.TryGetValue(element, out linkNode))
			{
				while (linkNode != null)
				{
					SurveyNodeReference link = linkNode.Value;
					if (link.ContextElement == contextElement &&
						referenceReason == link.ReferenceReason)
					{
						notifyList.NodeChanged(link.Node, linkNode, false, questionTypes);
						break;
					}
					linkNode = linkNode.Next;
				}
			}
		}
		void INotifySurveyElementChanged.ElementReferenceChanged(object element, object referenceReason, object contextElement, params Type[] questionTypes)
		{
			ElementReferenceChanged(element, referenceReason, contextElement, questionTypes);
		}
		/// <summary>
		/// Implements <see cref="INotifySurveyElementChanged.ElementDeleted(object, bool)"/>
		/// </summary>
		protected void ElementDeleted(object element, bool preserveReferences)
		{
			NodeLocation value;
			if (myNodeDictionary.TryGetValue(element, out value))
			{
				// Remove items from the primary display location
				ISurveyNodeReference reference = element as ISurveyNodeReference;
				if (reference != null && 0 == (reference.SurveyNodeReferenceOptions & SurveyNodeReferenceOptions.TrackReferenceInstance))
				{
					reference = null;
				}
				MainList notifyList;
				if (null != (notifyList = value.MainList))
				{
					if (reference != null)
					{
						// Delete the node as a reference
						ElementReferenceDeleted(reference.ReferencedElement, reference.SurveyNodeReferenceReason, notifyList.ContextElement);
					}
					else
					{
						// Delete the node directly
						notifyList.NodeDeleted(value.ElementNode);
					}
				}

				if (preserveReferences)
				{
					// We have all of the information from inclusion of the element in
					// a list, we just need a different node type.
					myNodeDictionary[element] = new NodeLocation(value.Survey, value.ElementNode);
				}
				else
				{
					// Remove items from all secondary display locations
					LinkedNode<SurveyNodeReference> linkNode;
					if (myReferenceDictionary.TryGetValue(element, out linkNode))
					{
						while (linkNode != null)
						{
							SurveyNodeReference link = linkNode.Value;
							if (myMainListDictionary.TryGetValue(link.ContextElement ?? TopLevelExpansionKey, out notifyList))
							{
								notifyList.NodeDeleted(linkNode.Value.Node);
							}
							linkNode = linkNode.Next;
						}
						myReferenceDictionary.Remove(element);
					}

					// Remove the tracking entry for this element
					myNodeDictionary.Remove(element);

					// Remove tracking for an expansion of this element
					if (myMainListDictionary.ContainsKey(element))
					{
						myMainListDictionary.Remove(element);
					}
				}
			}
		}
		void INotifySurveyElementChanged.ElementDeleted(object element, bool preserveReferences)
		{
			ElementDeleted(element, preserveReferences);
		}
		/// <summary>
		/// Implements <see cref="INotifySurveyElementChanged.ElementDeleted(object)"/>
		/// </summary>
		protected void ElementDeleted(object element)
		{
			ElementDeleted(element, false);
		}
		void INotifySurveyElementChanged.ElementDeleted(object element)
		{
			ElementDeleted(element);
		}
		/// <summary>
		/// Implements <see cref="INotifySurveyElementChanged.ElementReferenceDeleted(Object,Object,Object)"/>
		/// </summary>
		protected void ElementReferenceDeleted(object element, object referenceReason, object contextElement)
		{
			LinkedNode<SurveyNodeReference> headLinkNode;
			if (myReferenceDictionary.TryGetValue(element, out headLinkNode))
			{
				MainList notifyList;
				myMainListDictionary.TryGetValue(contextElement ?? TopLevelExpansionKey, out notifyList);
				LinkedNode<SurveyNodeReference> linkNode = headLinkNode;
				LinkedNode<SurveyNodeReference> startHeadLinkNode = headLinkNode;
				while (linkNode != null)
				{
					SurveyNodeReference link = linkNode.Value;
					if (link.ContextElement == contextElement &&
						referenceReason == link.ReferenceReason)
					{
						if (notifyList != null)
						{
							notifyList.NodeDeleted(link.Node);
						}
						linkNode.Detach(ref headLinkNode);
						break;
					}
					linkNode = linkNode.Next;
				}
				if (headLinkNode == null)
				{
					myReferenceDictionary.Remove(element);
					if (element is ISurveyFloatingNode)
					{
						// Verify that no one actually included this in a list, then remove it
						NodeLocation location;
						if (myNodeDictionary.TryGetValue(element, out location) &&
							location.MainList == null)
						{
							myNodeDictionary.Remove(element);
						}
					}
				}
				else if (startHeadLinkNode != headLinkNode)
				{
					myReferenceDictionary[element] = headLinkNode;
				}
			}
		}
		void INotifySurveyElementChanged.ElementReferenceDeleted(object element, object referenceReason, object contextElement)
		{
			ElementReferenceDeleted(element, referenceReason, contextElement);
		}
		/// <summary>
		/// Implements <see cref="INotifySurveyElementChanged.ElementRenamed"/>
		/// </summary>
		protected void ElementRenamed(object element)
		{
			NodeLocation location;
			if (myNodeDictionary.TryGetValue(element, out location))
			{
				MainList notifyList = location.MainList;
				if (notifyList != null)
				{
					notifyList.NodeRenamed(location.ElementNode, null);
				}
				LinkedNode<SurveyNodeReference> linkNode;
				if (myReferenceDictionary.TryGetValue(element, out linkNode))
				{
					while (linkNode != null)
					{
						SurveyNodeReference link = linkNode.Value;
						if (myMainListDictionary.TryGetValue(link.ContextElement ?? TopLevelExpansionKey, out notifyList))
						{
							notifyList.NodeRenamed(linkNode.Value.Node, linkNode);
						}
						linkNode = linkNode.Next;
					}
				}
			}
		}
		void INotifySurveyElementChanged.ElementRenamed(object element)
		{
			ElementRenamed(element);
		}
		/// <summary>
		/// Implements <see cref="INotifySurveyElementChanged.ElementReferenceRenamed"/>
		/// </summary>
		protected void ElementReferenceRenamed(object element, object referenceReason, object contextElement)
		{
			LinkedNode<SurveyNodeReference> linkNode;
			MainList notifyList;
			if (myMainListDictionary.TryGetValue(contextElement ?? TopLevelExpansionKey, out notifyList) &&
				myReferenceDictionary.TryGetValue(element, out linkNode))
			{
				while (linkNode != null)
				{
					SurveyNodeReference link = linkNode.Value;
					if (link.ContextElement == contextElement &&
						referenceReason == link.ReferenceReason)
					{
						notifyList.NodeRenamed(link.Node, linkNode);
						break;
					}
					linkNode = linkNode.Next;
				}
			}
		}
		void INotifySurveyElementChanged.ElementReferenceRenamed(object element, object referenceReason, object contextElement)
		{
			ElementReferenceRenamed(element, referenceReason, contextElement);
		}
		/// <summary>
		/// Implements <see cref="INotifySurveyElementChanged.ElementCustomSortChanged"/>
		/// </summary>
		protected void ElementCustomSortChanged(object element)
		{
			NodeLocation location;
			if (myNodeDictionary.TryGetValue(element, out location))
			{
				MainList notifyList = location.MainList;
				if (notifyList != null)
				{
					notifyList.NodeCustomSortChanged(location.ElementNode, null);
				}
				// The custom sort on the element is not used by references, do not notify
			}
		}
		void INotifySurveyElementChanged.ElementCustomSortChanged(object element)
		{
			ElementCustomSortChanged(element);
		}
		/// <summary>
		/// Implements <see cref="INotifySurveyElementChanged.ElementReferenceCustomSortChanged"/>
		/// </summary>
		protected void ElementReferenceCustomSortChanged(object element, object referenceReason, object contextElement)
		{
			LinkedNode<SurveyNodeReference> linkNode;
			MainList notifyList;
			if (myMainListDictionary.TryGetValue(contextElement ?? TopLevelExpansionKey, out notifyList) &&
				myReferenceDictionary.TryGetValue(element, out linkNode))
			{
				while (linkNode != null)
				{
					SurveyNodeReference link = linkNode.Value;
					if (link.ContextElement == contextElement &&
						referenceReason == link.ReferenceReason)
					{
						notifyList.NodeCustomSortChanged(link.Node, linkNode);
						break;
					}
					linkNode = linkNode.Next;
				}
			}
		}
		void INotifySurveyElementChanged.ElementReferenceCustomSortChanged(object element, object referenceReason, object contextElement)
		{
			ElementReferenceCustomSortChanged(element, referenceReason, contextElement);
		}
		#endregion //INotifySurveyElementChanged Implementation
		#region Survey class
		/// <summary>
		/// question list of all questions returned by the doc data
		/// </summary>
		private class Survey
		{
			#region Member Variables
			private readonly List<SurveyQuestion> myQuestions;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// public constructor
			/// </summary>
			/// <param name="surveyContext">The <typeparamref name="SurveyNodeContext"/> thisis created in. This value is not store with the new Survey.</param>
			/// <param name="questionProviders">Array of <see cref="ISurveyQuestionProvider{SurveyContextType}"/> instances</param>
			/// <param name="providerImageOffsets">Array of offsets into the global image list with indices corresponding to each provider</param>
			/// <param name="expansionKey">Key to identify the set of questions being retrieved from the <paramref name="questionProviderList"/></param>
			public Survey(SurveyContextType surveyContext, ISurveyQuestionProvider<SurveyContextType>[] questionProviders, int[] providerImageOffsets, object expansionKey)
			{
				int providerCount = questionProviders.Length;
				List<SurveyQuestion> surveyQuestions = new List<SurveyQuestion>();
				int totalShift = 0;
				for (int i = 0; i < providerCount; ++i)
				{
					IEnumerable<ISurveyQuestionTypeInfo<SurveyContextType>> questions = questionProviders[i].GetSurveyQuestions(surveyContext, expansionKey);
					if (questions == null)
					{
						continue;
					}
					foreach (ISurveyQuestionTypeInfo<SurveyContextType> currentQuestionTypeInfo in questions)
					{
						SurveyQuestion currentQuestion = new SurveyQuestion(currentQuestionTypeInfo, providerImageOffsets[i]);
						currentQuestion.Shift = totalShift;
						currentQuestion.Mask = GenerateMask(currentQuestion.BitCount, currentQuestion.Shift);
						currentQuestion.QuestionList = this;
						totalShift += currentQuestion.BitCount;
						surveyQuestions.Add(currentQuestion);
					}
				}
				// Sort by priority, but within reason. Grouped elements are given priority over non-group elements
				// to make it easier to find these important groups
				surveyQuestions.Sort(
					delegate(SurveyQuestion leftQuestion, SurveyQuestion rightQuestion)
					{
						if (leftQuestion == rightQuestion)
						{
							return 0;
						}
						ISurveyQuestionTypeInfo<SurveyContextType> leftQuestionInfo = leftQuestion.Question;
						ISurveyQuestionTypeInfo<SurveyContextType> rightQuestionInfo = rightQuestion.Question;
						if (0 != (leftQuestionInfo.UISupport & SurveyQuestionUISupport.Grouping))
						{
							if (0 == (rightQuestionInfo.UISupport & SurveyQuestionUISupport.Grouping))
							{
								return -1;
							}
						}
						else if (0 != (rightQuestionInfo.UISupport & SurveyQuestionUISupport.Grouping))
						{
							return 1;
						}
						int leftPriority = leftQuestionInfo.QuestionPriority;
						int rightPriority = rightQuestionInfo.QuestionPriority;
						if (leftPriority < rightPriority)
						{
							return -1;
						}
						else if (leftPriority == rightPriority)
						{
							return 0;
						}
						return 1;
					});
				myQuestions = surveyQuestions;
			}
			#endregion // Constructor
			#region Indexers
			/// <summary>
			/// Indexer for question list
			/// </summary>
			/// <param name="i">Index of questions to be returned</param>
			/// <returns>survey question at the location</returns>
			public SurveyQuestion this[int i]
			{
				get
				{
					return myQuestions[i];
				}
			}
			/// <summary>
			/// Indexer for question list
			/// </summary>
			/// <param name="question">type of question to retrieve</param>
			/// <returns>survey question of matching type</returns>
			public SurveyQuestion this[Type question]
			{
				get
				{
					List<SurveyQuestion> questions = myQuestions;
					int questionCount = questions.Count;
					for (int i = 0; i < questionCount; ++i)
					{
						SurveyQuestion currentQuestion = questions[i];
						if (currentQuestion.Question.QuestionType == question)
						{
							return currentQuestion;
						}
					}
					return null;
				}
			}
			#endregion // Indexers
			#region Accessor Properties
			/// <summary>
			/// number of questions in the survey
			/// </summary>
			public int Count
			{
				get
				{
					return myQuestions.Count;
				}
			}
			#endregion // Accessor Properties
			#region Methods
			/// <summary>
			/// returns index of question type passed in
			/// </summary>
			public int GetIndex(Type question)
			{
				List<SurveyQuestion> questions = myQuestions;
				int questionCount = questions.Count;
				for (int i = 0; i < questionCount; ++i)
				{
					if (question == questions[i].Question.QuestionType)
					{
						return i;
					}
				}
				return -1;
			}
			private static int GenerateMask(int bitCount, int shift)
			{
				return ((1 << bitCount) - 1) << shift;
			}
			#endregion // Methods
		}
		#endregion // Survey class
		#region SurveyQuestion class
		/// <summary>
		/// Wrapper class for <see cref="ISurveyQuestionTypeInfo{SurveyContextType}"/> that coordinates information
		/// for storing answers to this questions with storage information for other questions in
		/// a <see cref="Survey"/>
		/// </summary>
		private sealed class SurveyQuestion
		{
			private readonly ISurveyQuestionTypeInfo<SurveyContextType> myQuestion;
			private readonly string[] myHeaders;
			public const int NeutralAnswer = -1;
			#region MetaData and local members
			private int myShift;
			/// <summary>
			/// the total bitcount shift that this answer should have in all SampleDataElementNodes node data
			/// </summary>
			public int Shift
			{
				get
				{
					return myShift;
				}
				set
				{
					myShift = value;
				}
			}
			private int myMask;
			/// <summary>
			/// integer mask, the not applicable answer to this question shifted by the total shift for the survey this is in
			/// </summary>
			public int Mask
			{
				get
				{
					return myMask;
				}
				set
				{
					myMask = value;
				}
			}
			private Survey myQuestionList;
			/// <summary>
			/// the survey that this question is held in
			/// </summary>
			public Survey QuestionList
			{
				get
				{
					return this.myQuestionList;
				}
				set
				{
					myQuestionList = value;
				}
			}
			#endregion //MetaData and local members
			private int myProviderImageListOffset;
			/// <summary>
			/// Cached offset for image list associated with the provider for
			/// this question. Image indices are remapped to a composite image list
			/// to enable overlay support across question providers.
			/// </summary>
			public int ProviderImageListOffset
			{
				get
				{

					return myProviderImageListOffset;
				}
				set
				{
					myProviderImageListOffset = value;
				}
			}


			/// <summary>
			/// returns the wrapped ISurveyQuestionTypeInfo
			/// </summary>
			public ISurveyQuestionTypeInfo<SurveyContextType> Question
			{
				get
				{
					return myQuestion;
				}
			}
			/// <summary>
			/// Gets the UI support.
			/// </summary>
			/// <value>The UI support.</value>
			public SurveyQuestionUISupport UISupport
			{
				get
				{
					return myQuestion.UISupport;
				}
			}
			/// <summary>
			/// Number of answers to this question, not including the 'not applicable' answer
			/// </summary>
			public int CategoryCount
			{
				get
				{
					return myHeaders.Length;
				}
			}
			/// <summary>
			/// Number of bits needed to store all answers to this question, not including the 'not applicable' answer
			/// </summary>
			public int BitCount
			{
				get
				{
					int itemCount = myHeaders.Length + 1;
					int bitCount = 0;
					while (itemCount > 0)
					{
						itemCount >>= 1;
						bitCount++;
					}
					return bitCount;
				}
			}

			/// <summary>
			/// Returns the header for this answer
			/// </summary>
			public string CategoryHeader(int answer)
			{
				if (answer < 0 || answer > myHeaders.Length - 1)
				{
					return String.Empty;
				}
				return myHeaders[answer];
			}
			/// <summary>
			/// returns the answer value to this question in the integer node data passed in
			/// </summary>
			/// <param name="answerData">node data containing an answer to this question</param>
			/// <returns>the integer value of the answer to this question, or <see cref="NeutralAnswer"/> if the answer is neutral.</returns>
			public int ExtractAnswer(int answerData)
			{
				int mask = myMask;
				answerData &= mask;
				return (answerData == mask) ? NeutralAnswer : (answerData >> myShift);
			}
			/// <summary>
			/// public constructor
			/// </summary>
			/// <param name="question">the qeustion to be wrapped by this class</param>
			/// <param name="providerImageListOffset">ImageList offset for the provider</param>
			public SurveyQuestion(ISurveyQuestionTypeInfo<SurveyContextType> question, int providerImageListOffset)
			{
				if (question == null)
				{
					throw new ArgumentNullException("question");
				}
				myQuestion = question;
				ISurveyDynamicValues dynamicValues = question.DynamicQuestionValues;
				string[] headers;
				if (dynamicValues != null)
				{
					int valueCount = dynamicValues.ValueCount;
					headers = new string[valueCount];
					for (int i = 0; i < valueCount; ++i)
					{
						headers[i] = dynamicValues.GetValueName(i);
					}
				}
				else
				{
					headers = Utility.GetLocalizedEnumNames(question.QuestionType, true);
				}
				myHeaders = headers;
				myProviderImageListOffset = providerImageListOffset;
			}

		}
		#endregion // SurveyQuestion class
	}
}
