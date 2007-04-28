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
	}
	#endregion // ISurveyQuestionProvider interface
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
		/// as the expansion for this element.
		/// </summary>
		object SurveyNodeExpansionKey { get;}
	}
	#endregion // ISurveyNode interface
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
		/// <returns>Normal comparison semantics apply, except that a 0
		/// return here applies no information, not equality. Survey nodes
		/// must have a fully deterministic order.</returns>
		int CompareToSurveyNode(object other);
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
		/// Called when an element is added to the containers node provider
		/// </summary>
		/// <param name="element">The object that has been added</param>
		/// <param name="contextElement">If the object is added as part of a detail expansion, the context element
		/// is the element's parent object.</param>
		void ElementAdded(object element, object contextElement);
		/// <summary>
		/// called if an element in the containers node provider has been changed
		/// </summary>
		/// <param name="element">the object that has been changed</param>
		/// <param name="questionTypes">The question types.</param>
		void ElementChanged(object element, params Type[] questionTypes);
		/// <summary>
		/// called if element is removed from the containers node provider
		/// </summary>
		/// <param name="element">the object that was removed from the node provider</param>
		void ElementDeleted(object element);
		/// <summary>
		/// called if an element in the containers node provider has been renamed
		/// </summary>
		/// <param name="element">the object that has been renamed</param>
		void ElementRenamed(object element);
	}
	#endregion // INotifySurveyElementChanged interface
}
