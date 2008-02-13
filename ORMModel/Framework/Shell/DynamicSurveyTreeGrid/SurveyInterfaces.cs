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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid
{
	#region SurveyQuestionUISupport enum
	/// <summary>
	/// Survey question ui support. Indicates how answers to this
	/// questions may be used when presenting elements that answer
	/// the associated <see cref="ISurveyQuestionTypeInfo"/>
	/// </summary>
	[Flags]
	public enum SurveyQuestionUISupport
	{
		/// <summary>
		/// If nothing is supported by question
		/// </summary>
		None = 0,
		/// <summary>
		/// If sorting is supported by question
		/// </summary>
		Sorting = 1,
		/// <summary>
		/// If grouping is supported, implies that sorting is also supported
		/// </summary>
		Grouping = 2,
		/// <summary>
		/// Answers to the question can be resolved to an image index that is
		/// used for a primary glyph. Index resolution is performed with the
		/// <see cref="ISurveyQuestionTypeInfo.MapAnswerToImageIndex"/> method.
		/// </summary>
		Glyph = 4,
		/// <summary>
		/// Answers to the question can be resolved to an image index that is
		/// used as an overlay glyph. Index resolution is performed with the
		/// <see cref="ISurveyQuestionTypeInfo.MapAnswerToImageIndex"/> method.
		/// </summary>
		Overlay = 8,
		/// <summary>
		/// Question supports additional display settings with the
		/// <see cref="ISurveyQuestionTypeInfo.GetDisplayData"/> method.
		/// </summary>
		DisplayData = 0x10,
	}
	#endregion // SurveyQuestionUISupport enum
	#region ISurveyQuestionProvider interface
	/// <summary>
	/// An ISurveyQuestionProvider can return an array of ISurveyQuestionTypeInfo
	/// </summary>
	public interface ISurveyQuestionProvider
	{
		/// <summary>
		/// Retrieve the supported <see cref="ISurveyQuestionTypeInfo"/> instances
		/// </summary>
		/// <param name="expansionKey">The expansion key indicating the type of expansion
		/// data to retrieve for the provided context. Expansion keys are also used by
		/// <see cref="ISurveyNode.SurveyNodeExpansionKey"/> and <see cref="ISurveyNodeProvider.GetSurveyNodes"/></param>
		/// <returns><see cref="IEnumerable{ISurveyQuestionTypeInfo}"/> representing questions supported by this provider</returns>
		IEnumerable<ISurveyQuestionTypeInfo> GetSurveyQuestions(object expansionKey);
		/// <summary>
		/// The <see cref="ImageList"/> associated with answers to all supported questions
		/// </summary>
		ImageList SurveyQuestionImageList { get;}
        /// <summary>
        /// Get the list of types for this survey provider that correspond to
        /// error state display changes. Each returned type should correspond to one
        /// of the <see cref="ISurveyQuestionTypeInfo.QuestionType"/> types returned
        /// by the <see cref="GetSurveyQuestions"/>
        /// </summary>
        /// <returns><see cref="IEnumerable{Type}"/> or null</returns>
        IEnumerable<Type> GetErrorDisplayTypes();
	}
	#endregion // ISurveyQuestionProvider interface
	#region SurveyQuestionDisplayData structure
	/// <summary>
	/// Associate additional display information with this question.
	/// Used by the <see cref="ISurveyQuestionTypeInfo.GetDisplayData"/> method.
	/// </summary>
	public struct SurveyQuestionDisplayData
	{
		private bool myIsBold;
		private bool myIsGrayText;
		/// <summary>
		/// Default display settings
		/// </summary>
		public static readonly SurveyQuestionDisplayData Default = new SurveyQuestionDisplayData();
		/// <summary>
		/// Create a new <see cref="SurveyQuestionDisplayData"/>
		/// </summary>
		/// <param name="isBold">Display text as bold</param>
		/// <param name="isGrayText">Display text as gray text</param>
		public SurveyQuestionDisplayData(bool isBold, bool isGrayText)
		{
			myIsBold = isBold;
			myIsGrayText = isGrayText;
		}
		/// <summary>
		/// Return <see langword="true"/> if these are the default settings
		/// </summary>
		public bool IsDefault
		{
			get
			{
				return !(myIsBold || myIsGrayText);
			}
		}
		/// <summary>
		/// Should the text display bold?
		/// </summary>
		public bool Bold
		{
			get
			{
				return myIsBold;
			}
		}
		/// <summary>
		/// Should the text display gray?
		/// </summary>
		public bool GrayText
		{
			get
			{
				return myIsGrayText;
			}
		}
	}
	#endregion // SurveyQuestionDisplayData structure
	#region ISurveyQuestionTypeInfo interface
	/// <summary>
	/// Holds a reference to a specific type of question and method for asking question of objects
	/// </summary>
	public interface ISurveyQuestionTypeInfo
	{
		/// <summary>
		/// The type of question that this ISurveyQuestionTypeInfo represents.
		/// The return type must be an <see cref="Enum"/>
		/// </summary>
		Type QuestionType { get; }
		/// <summary>
		/// Retrieve the answer of any object to my question
		/// </summary>
		/// <param name="data">The data object to query for an answer to this question.</param>
		/// <returns>Answer to question, or -1 if the provided <paramref name="data"/>
		/// object does not implement <see cref="IAnswerSurveyQuestion{T}"/> or returns
		/// a not applicable answer for the <see cref="QuestionType"/> associated with
		/// this implementation of <see cref="ISurveyQuestionTypeInfo"/></returns>
		int AskQuestion(object data);
		/// <summary>
		/// Maps the index of the answer to an image in the <see cref="ImageList"/> provided
		/// by the <see cref="ISurveyQuestionProvider.SurveyQuestionImageList"/> property.
		/// </summary>
		/// <param name="answer">A value from the enum type returned by the <see cref="QuestionType"/> property.</param>
		/// <returns>0-based index into the image list.</returns>
		int MapAnswerToImageIndex(int answer);
		/// <summary>
		/// Retrieves additional display information for a given answer. Used only if
		/// <see cref="UISupport"/> indicates support for <see cref="SurveyQuestionUISupport.DisplayData"/>
		/// </summary>
		/// <param name="answer">A value from the enum type returned by the <see cref="QuestionType"/> property.</param>
		/// <returns><see cref="SurveyQuestionDisplayData"/> settings</returns>
		SurveyQuestionDisplayData GetDisplayData(int answer);
		/// <summary>
		/// UISupport for Question
		/// </summary>
		SurveyQuestionUISupport UISupport { get;}
	}
	#endregion // ISurveyQuestionTypeInfo interface
	#region IAnswerSurveyQuestion<T> interface
	/// <summary>
	/// Any object which is going to be displayed in the survey tree must implement this interface
	/// </summary>
	/// <typeparam name="TAnswerEnum">an enum representing the potential answers to this question</typeparam>
	public interface IAnswerSurveyQuestion<TAnswerEnum>
		where TAnswerEnum : struct, IFormattable, IComparable
	{
		/// <summary>
		/// called by survey tree to create node data of the implementing object's answers
		/// </summary>
		/// <returns>int representing the answer to the enum question</returns>
		int AskQuestion();
	}
	#endregion // IAnswerSurveyQuestion<T> interface
	#region ISurveyNode interface
	/// <summary>
	/// must be implemented on objects to be displayed on survey tree, used to get their displayable and editable names
	/// </summary>
	public interface ISurveyNode
	{
		/// <summary>
		/// whether or not this objects name is editable
		/// </summary>
		bool IsSurveyNameEditable { get; }
		/// <summary>
		/// the display name for the survey tree
		/// </summary>
		string SurveyName { get; }
		/// <summary>
		/// the name that will be displayed in edit mode, may be more complex than display name, is settable
		/// </summary>
		string EditableSurveyName { get; set; }
		/// <summary>
		/// Get a data object representing the survey node. Used for drag-drop operations.
		/// </summary>
		object SurveyNodeDataObject { get;}
		/// <summary>
		/// Return an object to use as the identifier for elements used
		/// as the expansion for this element. Elements returned as part
		/// of this expansion should implement <see cref="ISurveyNodeContext"/>
		/// to be able to find this element.
		/// </summary>
		object SurveyNodeExpansionKey { get;}
	}
	#endregion // ISurveyNode interface
	#region ISurveyNodeContext interface
	/// <summary>
	/// Implement on elements that are displayed as expansions in
	/// the survey tree. Used to locate context elements when an
	/// item has not yet been expanded and needs to be located in the tree.
	/// </summary>
	public interface ISurveyNodeContext
	{
		/// <summary>
		/// Return the survey context object for this object.
		/// A context object will return this element from
		/// <see cref="ISurveyNodeProvider.GetSurveyNodes"/>.
		/// </summary>
		object SurveyNodeContext { get;}
	}
	#endregion // ISurveyNodeContext interface
	#region ICustomComparableSurveyNode interface
	/// <summary>
	/// Provide a custom comparison to do before the sort falls back
	/// on comparing the survey names of the objects. A class that implements
	/// the <see cref="ISurveyNode"/> interface can also implement ICustomComparableSurveyNode
	/// to support custom sorting. Any grouping semantics will already have been supplied before
	/// this comparison is called.
	/// </summary>
	public interface ICustomComparableSurveyNode
	{
		/// <summary>
		/// Determine the ordering for this <see cref="ISurveyNode"/> object
		/// as compared to another ISurveyNode.
		/// </summary>
		/// <param name="other">The opposite object to compare to</param>
		/// <param name="customSortData">Data returned by the <see cref="ResetCustomSortData"/>
		/// method representing a snapshot of the data used to sort an element.</param>
		/// <param name="otherCustomSortData">Sort data returned by the other
		/// element's <see cref="ResetCustomSortData"/> method.</param>
		/// <returns>Normal comparison semantics apply, except that a 0
		/// return here applies no information, not equality. Survey nodes
		/// must have a fully deterministic order.</returns>
		int CompareToSurveyNode(object other, object customSortData, object otherCustomSortData);
		/// <summary>
		/// Custom sorting nodes requires data that cannot be represented by
		/// the survey categorization and string representations of the element.
		/// Generally, this data is based on the current node state. However,
		/// the old data is required to find an element in a custom sorted
		/// state at a time when the old custom sort information is no longer
		/// available. The data returned here (potentially null if the custom sort
		/// is based on state that does not change over time) is passed to the
		/// <see cref="CompareToSurveyNode"/> method, which can interpret the
		/// data to compare to elements.
		/// </summary>
		/// <param name="customSortData">A reference to existing data. If this
		/// is not null, then the data should only be recreated if it has changed.</param>
		/// <returns><see langword="true"/> if the data has changed</returns>
		bool ResetCustomSortData(ref object customSortData);
	}
	#endregion // ICustomComparableSurveyNode interface
	#region ISurveyNodeProvider interface
	/// <summary>
	/// Interface for a <see cref="DomainModel"/> to provide a list of objects for the <see cref="SurveyTreeContainer"/>.
	/// </summary>
	public interface ISurveyNodeProvider
	{
		/// <summary>
		/// Retrieve survey elements for this <see cref="DomainModel"/>. A <paramref name="context"/>
		/// object and <paramref name="expansionKey"/> are provided to provide detail information for
		/// each type of object. Requesting expansion nodes from the original provider as opposed to
		/// the context node enables providers that are unknown to the context node to provide
		/// expansion information.
		/// </summary>
		/// <param name="context">The parent object. If the context is <see langword="null"/>, then
		/// the nodes are being requested for the top-level list.</param>
		/// <param name="expansionKey">The expansion key indicating the type of expansion
		/// data to retrieve for the provided context. Expansion keys are also used by
		/// <see cref="ISurveyNode.SurveyNodeExpansionKey"/> and <see cref="ISurveyQuestionProvider.GetSurveyQuestions"/></param>
		/// <returns><see cref="IEnumerable{Object}"/> for all nodes returned by the provider</returns>
		IEnumerable<object> GetSurveyNodes(object context, object expansionKey);
	}
	#endregion //ISurveyNodeProvider interface
	#region INotifySurveyElementChanged interface
	/// <summary>
	///defines behavior for a container in the survey tree to recieve events from it's contained elements
	/// </summary>
	public interface INotifySurveyElementChanged
	{
		/// <summary>
		/// Called when an element is added to the container's node provider
		/// </summary>
		/// <param name="element">The object that has been added</param>
		/// <param name="contextElement">If the object is added as part of a detail expansion, the context element
		/// is the element's parent object.</param>
		void ElementAdded(object element, object contextElement);
		/// <summary>
		/// called if an element in the container's node provider has been changed
		/// </summary>
		/// <param name="element">the object that has been changed</param>
		/// <param name="questionTypes">The question types.</param>
		void ElementChanged(object element, params Type[] questionTypes);
		/// <summary>
		/// called if element is removed from the container's node provider
		/// </summary>
		/// <param name="element">the object that was removed from the node provider</param>
		void ElementDeleted(object element);
		/// <summary>
		/// called if an element in the container's node provider has been renamed
		/// </summary>
		/// <param name="element">the object that has been renamed</param>
		void ElementRenamed(object element);
		/// <summary>
		/// called if the custom sort data has changed for an element in the container's node provider
		/// </summary>
		/// <param name="element">the object that has been modified</param>
		void ElementCustomSortChanged(object element);
	}
	#endregion // INotifySurveyElementChanged interface
}
