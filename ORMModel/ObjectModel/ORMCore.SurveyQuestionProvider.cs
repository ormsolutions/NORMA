using System;
using Neumont.Tools.ORM.Framework.DynamicSurveyTreeGrid;
namespace Neumont.Tools.ORM.ObjectModel
{
	public partial class ORMMetaModel : ISurveyQuestionProvider
	{
		/// <summary>
		/// Returns an array of ISurveyQuestionTypeInfo representing the questions that can be	asked of objects in this MetaModel
		/// </summary>
		protected ISurveyQuestionTypeInfo[] GetSurveyQuestionTypeInfo()
		{
			return new ISurveyQuestionTypeInfo[]{
				new ProvideSurveyQuestionForElementType(),
				new ProvideSurveyQuestionForErrorState()};
		}
		ISurveyQuestionTypeInfo[] ISurveyQuestionProvider.GetSurveyQuestionTypeInfo()
		{
			return this.GetSurveyQuestionTypeInfo();
		}
		private class ProvideSurveyQuestionForElementType : ISurveyQuestionTypeInfo
		{
			protected Type QuestionType
			{
				get
				{
					return typeof(ElementType);
				}
			}
			Type ISurveyQuestionTypeInfo.QuestionType
			{
				get
				{
					return this.QuestionType;
				}
			}
			protected int AskQuestion(object data)
			{
				IAnswerSurveyQuestion<ElementType> typedData = data as IAnswerSurveyQuestion<ElementType>;
				if (typedData != null)
				{
					return (int)typedData.AskQuestion();
				}
				else
				{
					return -1;
				}
			}
			int ISurveyQuestionTypeInfo.AskQuestion(object data)
			{
				return this.AskQuestion(data);
			}
		}
		private class ProvideSurveyQuestionForErrorState : ISurveyQuestionTypeInfo
		{
			protected Type QuestionType
			{
				get
				{
					return typeof(ErrorState);
				}
			}
			Type ISurveyQuestionTypeInfo.QuestionType
			{
				get
				{
					return this.QuestionType;
				}
			}
			protected int AskQuestion(object data)
			{
				IAnswerSurveyQuestion<ErrorState> typedData = data as IAnswerSurveyQuestion<ErrorState>;
				if (typedData != null)
				{
					return (int)typedData.AskQuestion();
				}
				else
				{
					return -1;
				}
			}
			int ISurveyQuestionTypeInfo.AskQuestion(object data)
			{
				return this.AskQuestion(data);
			}
		}
	}
}
