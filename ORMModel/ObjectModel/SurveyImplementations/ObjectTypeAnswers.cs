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
		#region IAnswerSurveyQuestion<SurveyErrorState> Implementation
		int IAnswerSurveyQuestion<SurveyErrorState>.AskQuestion(object contextElement)
		{
			return AskErrorQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyErrorState}.AskQuestion"/>
		/// </summary>
		protected int AskErrorQuestion(object contextElement)
		{
			ORMModel model = Model;
			return (Model == null) ?
				-1 :
				(int)(ModelError.HasErrors(this, ModelErrorUses.DisplayPrimary, model.ModelErrorDisplayFilter) ? SurveyErrorState.HasError : SurveyErrorState.NoError);
		}
		#endregion // IAnswerSurveyQuestion<SurveyErrorState> Implementation
		#region IAnswerSurveyQuestion<SurveyElementType> Implementation
		int IAnswerSurveyQuestion<SurveyElementType>.AskQuestion(object contextElement)
		{
			return AskElementQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyElementType}.AskQuestion"/>
		/// </summary>
		protected static int AskElementQuestion(object contextElements)
		{
			return (int)SurveyElementType.ObjectType;
		}
		#endregion // IAnswerSurveyQuestion<SurveyElementType> Implementation
		#region IAnswerSurveyQuestion<SurveyQuestionGlyph> Implementation
		int IAnswerSurveyQuestion<SurveyQuestionGlyph>.AskQuestion(object contextElement)
		{
			return AnswerGlyphQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyQuestionGlyph}.AskQuestion"/>
		/// </summary>
		protected int AnswerGlyphQuestion(object contextElement)
		{
			Objectification objectification;
			if (this.IsValueType)
			{
				return (int)SurveyQuestionGlyph.ValueType;
			}
			else if (null != (objectification = this.Objectification) &&
				!objectification.IsImplied)
			{
				return (int)SurveyQuestionGlyph.ObjectifiedFactType;
			}
			else
			{
				return (int)SurveyQuestionGlyph.EntityType;
			}
		}
		#endregion // IAnswerSurveyQuestion<SurveyQuestionGlyph> Implementation
		#region ISurveyNode Implementation
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
		#endregion // ISurveyNode Implementation
	}

}
