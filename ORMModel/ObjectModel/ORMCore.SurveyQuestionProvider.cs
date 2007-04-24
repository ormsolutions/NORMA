using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;
namespace Neumont.Tools.ORM.ObjectModel
{
	partial class ORMCoreDomainModel : ISurveyQuestionProvider
	{
		private static readonly ISurveyQuestionTypeInfo[] mySurveyQuestionTypeInfo = new ISurveyQuestionTypeInfo[]{
			ProvideSurveyQuestionForElementType.Instance,
			ProvideSurveyQuestionForErrorState.Instance,
			ProvideSurveyQuestionForSurveyQuestionGlyph.Instance};
		/// <summary>Implements <see cref="ISurveyQuestionProvider.GetSurveyQuestions"/></summary>
		protected static IEnumerable<ISurveyQuestionTypeInfo> GetSurveyQuestions()
		{
			return mySurveyQuestionTypeInfo;
		}
		IEnumerable<ISurveyQuestionTypeInfo> ISurveyQuestionProvider.GetSurveyQuestions()
		{
			return GetSurveyQuestions();
		}
		/// <summary>Implements <see cref="ISurveyQuestionProvider.SurveyQuestionImageList"/></summary>
		protected ImageList SurveyQuestionImageList
		{
			get
			{
				return ResourceStrings.SurveyTreeImageList;
			}
		}
		ImageList ISurveyQuestionProvider.SurveyQuestionImageList
		{
			get
			{
				return this.SurveyQuestionImageList;
			}
		}
		private sealed class ProvideSurveyQuestionForElementType : ISurveyQuestionTypeInfo
		{
			private ProvideSurveyQuestionForElementType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo Instance = new ProvideSurveyQuestionForElementType();
			public Type QuestionType
			{
				get
				{
					return typeof(ElementType);
				}
			}
			public int AskQuestion(object data)
			{
				IAnswerSurveyQuestion<ElementType> typedData = data as IAnswerSurveyQuestion<ElementType>;
				if (typedData != null)
				{
					return typedData.AskQuestion();
				}
				else
				{
					return -1;
				}
			}
			public int MapAnswerToImageIndex(int answer)
			{
				if ((this.UISupport & SurveyQuestionUISupport.Glyph) != SurveyQuestionUISupport.None)
				{
					SurveyQuestionGlyph t = (SurveyQuestionGlyph)answer;
					return (int)t;
				}
				else
				{
					return -1;
				}
			}
			public SurveyQuestionUISupport UISupport
			{
				get
				{
					return SurveyQuestionUISupport.Grouping | SurveyQuestionUISupport.Sorting;
				}
			}
		}
		private sealed class ProvideSurveyQuestionForErrorState : ISurveyQuestionTypeInfo
		{
			private ProvideSurveyQuestionForErrorState()
			{
			}
			public static readonly ISurveyQuestionTypeInfo Instance = new ProvideSurveyQuestionForErrorState();
			public Type QuestionType
			{
				get
				{
					return typeof(ErrorState);
				}
			}
			public int AskQuestion(object data)
			{
				IAnswerSurveyQuestion<ErrorState> typedData = data as IAnswerSurveyQuestion<ErrorState>;
				if (typedData != null)
				{
					return typedData.AskQuestion();
				}
				else
				{
					return -1;
				}
			}
			public int MapAnswerToImageIndex(int answer)
			{
				if ((this.UISupport & SurveyQuestionUISupport.Glyph) != SurveyQuestionUISupport.None)
				{
					SurveyQuestionGlyph t = (SurveyQuestionGlyph)answer;
					return (int)t;
				}
				else
				{
					return -1;
				}
			}
			public SurveyQuestionUISupport UISupport
			{
				get
				{
					return SurveyQuestionUISupport.Overlay;
				}
			}
		}
		private sealed class ProvideSurveyQuestionForSurveyQuestionGlyph : ISurveyQuestionTypeInfo
		{
			private ProvideSurveyQuestionForSurveyQuestionGlyph()
			{
			}
			public static readonly ISurveyQuestionTypeInfo Instance = new ProvideSurveyQuestionForSurveyQuestionGlyph();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyQuestionGlyph);
				}
			}
			public int AskQuestion(object data)
			{
				IAnswerSurveyQuestion<SurveyQuestionGlyph> typedData = data as IAnswerSurveyQuestion<SurveyQuestionGlyph>;
				if (typedData != null)
				{
					return typedData.AskQuestion();
				}
				else
				{
					return -1;
				}
			}
			public int MapAnswerToImageIndex(int answer)
			{
				if ((this.UISupport & SurveyQuestionUISupport.Glyph) != SurveyQuestionUISupport.None)
				{
					SurveyQuestionGlyph t = (SurveyQuestionGlyph)answer;
					return (int)t;
				}
				else
				{
					return -1;
				}
			}
			public SurveyQuestionUISupport UISupport
			{
				get
				{
					return SurveyQuestionUISupport.Glyph;
				}
			}
		}
	}
}
