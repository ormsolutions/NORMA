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
using System.ComponentModel;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;
using Neumont.Tools.ORM.Design;

namespace Neumont.Tools.ORM.ObjectModel
{
	public partial class FactType : IAnswerSurveyQuestion<ElementType>, IAnswerSurveyQuestion<ErrorState>, ISurveyName
	{
		#region IAnswerSurveyQuestion<ErrorState> Members

		int IAnswerSurveyQuestion<ErrorState>.AskQuestion()
		{
			return AskErrorQuestion();
		}
		/// <summary>
		/// returns answer to IAnswerSurveyQuetion for errors
		/// </summary>
		/// <returns></returns>
		protected int AskErrorQuestion()
		{
			return (int)(ModelError.HasErrors(this, ModelErrorUses.None) ? ErrorState.HasError : ErrorState.NoError);
		}

		#endregion
		#region IAnswerSurveyQuestion<ElementType> Members
		int IAnswerSurveyQuestion<ElementType>.AskQuestion()
		{
			return AskElementQuestion();
		}
		/// <summary>
		/// implementation of AskQuestion method from IAnswerSurveyQuestion
		/// </summary>
		/// <returns></returns>
		protected int AskElementQuestion()
		{
			return (int)ElementType.FactType;
		}

		#endregion
		#region ISurveyName Members
		/// <summary>
		/// returns whether or not this object is editable in a survey tree
		/// </summary>
		bool ISurveyName.IsEditable
		{
			get
			{
				return IsEditable;
			}
		}
		/// <summary>
		/// implementation of IsEditable from ISurveyName
		/// </summary>
		protected bool IsEditable
		{
			get
			{
				// UNDONE: 2006-06 DSL Tools port: This seemed to be returning the same value as IsReadOnly, rather than its opposite,
				// which seemed to be rather backwards. For now, I've changed it to return !IsReadOnly...
				return !ORMTypeDescriptor.CreateNamePropertyDescriptor(this).IsReadOnly;
			}
		}
		/// <summary>
		/// the display name for a survey tree
		/// </summary>
		string ISurveyName.SurveyName 
		{
			get
			{
				return SurveyName;
			}
		}
		/// <summary>
		/// implementation of SurveyName from ISurveyName
		/// </summary>
		protected string SurveyName //TODO: this might be updated to show the more informative element names (componentName)?
		{
			get 
			{
				return this.Name;
			}
		}
		/// <summary>
		/// the editable name for a survey tree
		/// </summary>
		string ISurveyName.EditableSurveyName
		{
			get
			{
				return EditableSurveyName;
			}
			set
			{
				EditableSurveyName = value;
			}
		}
		/// <summary>
		/// implementation of EditableSurveyName from ISurveyName
		/// </summary>
		protected string EditableSurveyName
		{
			get
			{
				return this.Name;
			}
			set
			{
				this.Name = value;
			}
		}

		#endregion
	}
}
