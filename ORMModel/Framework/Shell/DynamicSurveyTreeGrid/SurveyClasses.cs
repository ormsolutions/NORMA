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
using System.Windows.Forms;
using Microsoft.VisualStudio.VirtualTreeGrid;

namespace Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid
{
	/// <summary>
	/// SurveyTree class. Used to create a tree that can be used with
	/// a <see cref="SurveyTreeContainer"/>
	/// </summary>
	public partial class SurveyTree : INotifySurveyElementChanged
	{
		#region NodeLocation structure
		private struct NodeLocation
		{
			public MainList MainList;
			public SampleDataElementNode ElementNode;
			public NodeLocation(MainList list, SampleDataElementNode node)
			{
				ElementNode = node;
				MainList = list;
			}
		}
		#endregion // NodeLocation structure
		#region Member Variables
		/// <summary>
		/// An expansionKey to use instead of the public <see langword="null"/> representation of
		/// the top level element grouping.
		/// </summary>
		private static readonly object TopLevelExpansionKey = new object();
		private IEnumerable<ISurveyNodeProvider> myNodeProviderList;
		private IEnumerable<ISurveyQuestionProvider> myQuestionProviderList;
		private Dictionary<object, MainList> myMainListDictionary;
		private Dictionary<object, NodeLocation> myNodeDictionary;
		private Dictionary<object, Survey> mySurveyDictionary;
		private Type[] myErrorDisplayTypes;
		#endregion // Member Variables
		#region Constructor
		/// <summary>
		/// Public constructor
		/// </summary>
		public SurveyTree(IEnumerable<ISurveyNodeProvider> nodeProviderList, IEnumerable<ISurveyQuestionProvider> questionProviderList)
		{
			if (nodeProviderList == null)
			{
				throw new ArgumentNullException("nodeProviderList");
			}
			if (questionProviderList == null)
			{
				throw new ArgumentNullException("questionProviderList");
			}
			myNodeProviderList = nodeProviderList;
			myQuestionProviderList = questionProviderList;
			myMainListDictionary = new Dictionary<object, MainList>();
			myNodeDictionary = new Dictionary<object, NodeLocation>();
			mySurveyDictionary = new Dictionary<object, Survey>();
		}
		#endregion // Constructor
		#region Accessor Properties
		/// <summary>
		/// Provides the RootBranch for a <see cref="SurveyTree"/>
		/// </summary>
		public IBranch CreateRootBranch()
		{
			MainList mainList;
			if (!myMainListDictionary.TryGetValue(TopLevelExpansionKey, out mainList))
			{
				myMainListDictionary.Add(TopLevelExpansionKey, mainList = new MainList(this, null, null));
			}
			return mainList.CreateRootBranch();
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
					foreach (ISurveyQuestionProvider questionProvider in myQuestionProviderList)
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
			if (!dictionary.TryGetValue((expansionKey == null) ? TopLevelExpansionKey : expansionKey, out retVal))
			{
				retVal = new Survey(myQuestionProviderList, expansionKey);
			}
			return retVal;
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
			NodeLocation value;
			if (myNodeDictionary.TryGetValue(element, out value))
			{
				value.MainList.NodeChanged(value.ElementNode, questionTypes);
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
			NodeLocation value;
			if (myNodeDictionary.TryGetValue(element, out value))
			{
				value.MainList.NodeDeleted(value.ElementNode);
				myNodeDictionary.Remove(element);
				if (myMainListDictionary.ContainsKey(element))
				{
					myMainListDictionary.Remove(element);
				}
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
			NodeLocation value;
			if (myNodeDictionary.TryGetValue(element, out value))
			{
				value.MainList.NodeRenamed(value.ElementNode);
			}
		}
		void INotifySurveyElementChanged.ElementRenamed(object element)
		{
			ElementRenamed(element);
		}
		/// <summary>
		/// Implements <see cref="INotifySurveyElementChanged.ElementCustomSortChanged"/>
		/// </summary>
		protected void ElementCustomSortChanged(object element)
		{
			NodeLocation value;
			if (myNodeDictionary.TryGetValue(element, out value))
			{
				value.MainList.NodeCustomSortChanged(value.ElementNode);
			}
		}
		void INotifySurveyElementChanged.ElementCustomSortChanged(object element)
		{
			ElementCustomSortChanged(element);
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
			private int totalShift;
			ImageList myMainImageList;
			#endregion // Member Variables
			#region Constructor
			/// <summary>
			/// public constructor
			/// </summary>
			/// <param name="questionProviderList">enumerable object of ISurveyQuestion providers</param>
			/// <param name="expansionKey">Key to identify the set of questions being retrieved from the <paramref name="questionProviderList"/></param>
			public Survey(IEnumerable<ISurveyQuestionProvider> questionProviderList, object expansionKey)
			{
				if (questionProviderList == null)
				{
					throw new ArgumentNullException("questionProviderList");
				}
				myQuestions = new List<SurveyQuestion>();
				myMainImageList = null;
				bool combinedImageList = false;
				int imageOffset = 0;
				foreach (ISurveyQuestionProvider provider in questionProviderList)
				{
					IEnumerable<ISurveyQuestionTypeInfo> questions = provider.GetSurveyQuestions(expansionKey);
					if (questions == null)
					{
						continue;
					}
					foreach (ISurveyQuestionTypeInfo currentQuestionTypeInfo in questions)
					{
						SurveyQuestion currentQuestion = new SurveyQuestion(currentQuestionTypeInfo, imageOffset);
						currentQuestion.Shift = totalShift;
						currentQuestion.Mask = GenerateMask(currentQuestion.BitCount, currentQuestion.Shift);
						currentQuestion.QuestionList = this;
						totalShift += currentQuestion.BitCount;
						myQuestions.Add(currentQuestion);
					}
					// Sort by priority, but within reason. Grouped elements are given priority over non-group elements
					// to make it easier to find these important groups
					myQuestions.Sort(
						delegate(SurveyQuestion leftQuestion, SurveyQuestion rightQuestion)
						{
							if (leftQuestion == rightQuestion)
							{
								return 0;
							}
							ISurveyQuestionTypeInfo leftQuestionInfo = leftQuestion.Question;
							ISurveyQuestionTypeInfo rightQuestionInfo = rightQuestion.Question;
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
					ImageList currentImageList = provider.SurveyQuestionImageList;
					if (currentImageList != null)
					{
						ImageList.ImageCollection currentImages = currentImageList.Images;
						int currentCount = currentImages.Count;
						if (currentCount != 0)
						{
							if (myMainImageList == null)
							{
								myMainImageList = currentImageList;
							}
							else
							{
								if (!combinedImageList)
								{
									combinedImageList = true;
									ImageList oldList = myMainImageList;
									myMainImageList = new ImageList();
									ImageList.ImageCollection oldImages = oldList.Images;
									for (int i = 0; i < imageOffset; ++i)
									{
										myMainImageList.Images.Add(oldImages[i]);
									}
								}
								for (int i = 0; i < currentCount; ++i)
								{
									myMainImageList.Images.Add(currentImages[i]);
								}
							}
							imageOffset += currentCount;
						}
					}
				}
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
					for (int i = 0; i < myQuestions.Count; ++i)
					{
						SurveyQuestion currentQuestion = myQuestions[i];
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

			/// <summary>
			/// Gets the main image list.
			/// </summary>
			/// <value>The main image list.</value>
			public ImageList MainImageList
			{

				get
				{
					return myMainImageList;
				}
			}
			#endregion // Accessor Properties
			#region Methods
			/// <summary>
			/// returns index of question type passed in
			/// </summary>
			public int GetIndex(Type question)
			{
				for (int i = 0; i < myQuestions.Count; ++i)
				{
					if (question == myQuestions[i].Question.QuestionType)
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
		/// wrapper for ISurveyQeustionTypeInfo, holds some metadata about the question
		/// </summary>
		private sealed class SurveyQuestion
		{
			private readonly ISurveyQuestionTypeInfo myQuestion;
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
			public ISurveyQuestionTypeInfo Question
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
			public SurveyQuestion(ISurveyQuestionTypeInfo question, int providerImageListOffset)
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
