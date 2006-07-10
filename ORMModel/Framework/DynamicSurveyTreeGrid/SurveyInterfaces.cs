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

namespace Neumont.Tools.ORM.Framework.DynamicSurveyTreeGrid
{
	#region ISurveyQuestionProvider
	/// <summary>
	/// An ISurveyQuestionProvider can return an array of ISurveyQeustionTypeInfo
	/// </summary>
	public interface ISurveyQuestionProvider
	{
		/// <summary>
		/// Get an array of ISurveyQuestionTypeInfo
		/// </summary>
		/// <returns>Array of interfaces representing all available questions in the system</returns>
		ISurveyQuestionTypeInfo[] GetSurveyQuestionTypeInfo();
	}
	#endregion
	#region ISurveyQuestionTypeInfo
	/// <summary>
	/// Holds a reference to a specific type of question and method for asking question of objects
	/// </summary>
	public interface ISurveyQuestionTypeInfo
	{
		/// <summary>
		/// The type of question that this ISurveyQuestionTypeInfo represents
		/// </summary>
		Type QuestionType { get;}
		/// <summary>
		/// Retrieve the answer of any object to my question
		/// </summary>
		/// <param name="data">if object does not implement IAnswerSurveyQuestion return is not applicable</param>
		/// <returns>integer answer to question, -1 if not applicable</returns>
		int AskQuestion(object data);
	}
	#endregion
	#region IAnswerSurveyQuestion<T>
	/// <summary>
	/// Any object which is going to be displayed in the survey tree must implement this interface
	/// </summary>
	/// <typeparam name="T">an enum representing the potential answers to this question</typeparam>
	public interface IAnswerSurveyQuestion<T>
	{
		/// <summary>
		/// called by survey tree to create node data of the implementing object's answers
		/// </summary>
		/// <returns>int representing the answer to the enum question</returns>
		int AskQuestion();
	}
	#endregion
	#region ISurveyName
	/// <summary>
	/// must be implemented on objects to be displayed on survey tree, used to get their displayable and editable names
	/// </summary>
	public interface ISurveyName
	{
		/// <summary>
		/// whether or not this objects name is editable
		/// </summary>
		bool IsEditable { get;}
		/// <summary>
		/// the display name for the survey tree
		/// </summary>
		string SurveyName { get;}
		/// <summary>
		/// the name that will be displayed in edit mode, may be more complex than display name, is settable
		/// </summary>
		string EditableSurveyName { get; set;}
	}
	#endregion //ISurveyName
	#region ISurveyNodeProvider
	/// <summary>
	/// Interface for a MetaModel to provide an enumeration of SampleDataElementNodes for the SurveyTree
	/// </summary>
	public interface ISurveyNodeProvider
	{
		/// <summary>
		/// retrieve an enumeration of SampleDataElementNdoes from this MetaModel
		/// </summary>
		/// <returns>IEnumerable of SampleDataElementNodes</returns>
		IEnumerable<SampleDataElementNode> GetSurveyNodes();
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
		/// <param name="sender">the object that has been added</param>
		void ElementAdded(object sender);
		/// <summary>
		/// called if an element in the containers node provider has been changed
		/// </summary>
		/// <param name="sender">the object that has been changed</param>
		/// <param name="questions">array of ISurveyQuestionTypeInfo that could be effected by this change</param>
		void ElementChanged(object sender, ISurveyQuestionTypeInfo[] questions);
		/// <summary>
		/// called if element is removed from the containers node provider
		/// </summary>
		/// <param name="sender">the object that was removed from the node provider</param>
		void ElementDeleted(object sender);
		/// <summary>
		/// called if an element in the containers node provider has been renamed
		/// </summary>
		/// <param name="sender">the object that has been renamed</param>
		void ElementRenamed(object sender);
	}
	#endregion //INotifySurveyElementChanged
}
