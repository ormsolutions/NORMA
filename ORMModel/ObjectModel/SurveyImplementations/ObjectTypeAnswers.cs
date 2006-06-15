using System;
using System.Collections.Generic;
using System.Text;
using Neumont.Tools.ORM.Framework.DynamicSurveyTreeGrid;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORM.ObjectModel
{
	public partial class ObjectType : IAnswerSurveyQuestion<ErrorState>, IAnswerSurveyQuestion<ElementType>
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
	}
}
