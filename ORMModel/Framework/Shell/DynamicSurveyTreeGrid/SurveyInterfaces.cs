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
	/// the associated <see cref="SurveyQuestion"/>
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
		/// <returns><see cref="IEnumerable{ISurveyQuestionTypeInfo}"/> representing questions supported by this provider</returns>
		IEnumerable<ISurveyQuestionTypeInfo> GetSurveyQuestions();
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
	#region IAnswerSurveyQuestion<T>
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
	#endregion
	#region ISurveyNode
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
	}
	#endregion //ISurveyNode
	#region ISurveyNodeProvider
	/// <summary>
	/// Interface for a <see cref="DomainModel"/> to provide a list of objects for the <see cref="SurveyTreeContainer"/>.
	/// </summary>
	public interface ISurveyNodeProvider
	{
		/// <summary>
		/// Retrieve survey elements for this <see cref="DomainModel"/>.
		/// </summary>
		IEnumerable<object> GetSurveyNodes();
	}
	#endregion //ISurveyNodeProvider
	#region INotifySurveyElementChanged
	/// <summary>
	///defines behavior for a container in the survey tree to recieve events from it's contained elements
	/// </summary>
	public interface INotifySurveyElementChanged
	{
		/// <summary>
		/// called if an element is added to the containers node provider
		/// </summary>
		/// <param name="element">the object that has been added</param>
		void ElementAdded(object element);
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
	#endregion //INotifySurveyElementChanged
}
