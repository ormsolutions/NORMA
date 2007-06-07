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
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;
using Neumont.Tools.ORM.ObjectModel;
using System.Windows.Forms;

namespace Neumont.Tools.ORM.ObjectModel
{
	public partial class ObjectType : IAnswerSurveyQuestion<SurveyErrorState>, IAnswerSurveyQuestion<SurveyElementType>, IAnswerSurveyQuestion<SurveyQuestionGlyph>, ISurveyNode
	{
		#region IAnswerSurveyQuestion<ErrorState> Implementation
		int IAnswerSurveyQuestion<SurveyErrorState>.AskQuestion()
		{
			return AskErrorQuestion();
		}
		/// <summary>
		/// implmentation of AskQuestion from IAnswerSurveyQuestion
		/// </summary>
		/// <returns></returns>
		protected int AskErrorQuestion()
		{
			if (Model == null)
				return -1;
			return (int)(ModelError.HasErrors(this, ModelErrorUses.DisplayPrimary, Model.ModelErrorDisplayFilter) ? SurveyErrorState.HasError : SurveyErrorState.NoError);
		}
		#endregion // IAnswerSurveyQuestion<ErrorState> Implementation
		#region IAnswerSurveyQuestion<ElementType> Members

		int IAnswerSurveyQuestion<SurveyElementType>.AskQuestion()
		{
			return AskElementQuestion();
		}
		/// <summary>
		/// implementation of AskQuestion from IAnswerSurveyQuestion
		/// </summary>
		/// <returns></returns>
		protected int AskElementQuestion()
		{
			return (int)SurveyElementType.ObjectType;
		}

		#endregion
		#region IAnswerSurveyQuestion<SurveyQuestionGlyph> Members

		int IAnswerSurveyQuestion<SurveyQuestionGlyph>.AskQuestion()
		{
			return AnswerGlyphQuestion();
		}

		/// <summary>
		/// Answers the glyph question.
		/// </summary>
		/// <returns></returns>
		protected int AnswerGlyphQuestion()
		{
			if (this.IsValueType)
			{
				return (int)SurveyQuestionGlyph.ValueType;
			}
			else if (this.Objectification != null && !this.Objectification.IsImplied)
			{
				return (int)SurveyQuestionGlyph.ObjectifiedFactType;
			}
			else
			{
				return (int)SurveyQuestionGlyph.EntityType;
			}
		}

		#endregion
		#region ISurveyNode Members
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeDataObject"/>
		/// </summary>
		protected new object SurveyNodeDataObject
		{
			get
			{

				DataObject retVal = new DataObject();
				Objectification objectification;
				if (null != (objectification = Objectification))
				{
					retVal.SetData(typeof(FactType), objectification.NestedFactType);
				}
				else
				{
					retVal.SetData(typeof(ObjectType), this);
				}
				return retVal;
			}
		}
		object ISurveyNode.SurveyNodeDataObject
		{
			get
			{
				return SurveyNodeDataObject;
			}
		}
		#endregion


	}

}
