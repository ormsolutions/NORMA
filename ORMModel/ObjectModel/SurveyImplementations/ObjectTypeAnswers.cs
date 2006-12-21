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
	public partial class ObjectType : IAnswerSurveyQuestion<ErrorState>, IAnswerSurveyQuestion<ElementType>, ISurveyNode
	{

		#region IAnswerSurveyQuestion<ErrorState> Members

		int IAnswerSurveyQuestion<ErrorState>.AskQuestion()
		{
			return AskErrorQuestion();
		}
		/// <summary>
		/// implmentation of AskQuestion from IAnswerSurveyQuestion
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
		/// implementation of AskQuestion from IAnswerSurveyQuestion
		/// </summary>
		/// <returns></returns>
		protected int AskElementQuestion()
		{
			return (int)ElementType.ObjectType;
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
