using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;
namespace Neumont.Tools.ORM.ObjectModel
{
	partial class ORMCoreDomainModel : ISurveyQuestionProvider
	{
		private static readonly ISurveyQuestionTypeInfo[] mySurveyQuestionTypeInfo1 = new ISurveyQuestionTypeInfo[]{
			ProvideSurveyQuestionForSurveyElementType.Instance,
			ProvideSurveyQuestionForSurveyErrorState.Instance,
			ProvideSurveyQuestionForSurveyQuestionGlyph.Instance};
		private static readonly ISurveyQuestionTypeInfo[] mySurveyQuestionTypeInfo2 = new ISurveyQuestionTypeInfo[]{
			ProvideSurveyQuestionForSurveyFactTypeDetailType.Instance,
			ProvideSurveyQuestionForSurveyErrorState.Instance,
			ProvideSurveyQuestionForSurveyQuestionGlyph.Instance};
		/// <summary>Implements <see cref="ISurveyQuestionProvider.GetSurveyQuestions"/></summary>
		protected static IEnumerable<ISurveyQuestionTypeInfo> GetSurveyQuestions(object expansionKey)
		{
			if (expansionKey == null)
			{
				return mySurveyQuestionTypeInfo1;
			}
			else if (expansionKey == FactType.SurveyExpansionKey)
			{
				return mySurveyQuestionTypeInfo2;
			}
			return null;
		}
		IEnumerable<ISurveyQuestionTypeInfo> ISurveyQuestionProvider.GetSurveyQuestions(object expansionKey)
		{
			return GetSurveyQuestions(expansionKey);
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
		private sealed class ProvideSurveyQuestionForSurveyElementType : ISurveyQuestionTypeInfo
		{
			private ProvideSurveyQuestionForSurveyElementType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo Instance = new ProvideSurveyQuestionForSurveyElementType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyElementType);
				}
			}
			public int AskQuestion(object data)
			{
				IAnswerSurveyQuestion<SurveyElementType> typedData = data as IAnswerSurveyQuestion<SurveyElementType>;
				if (typedData != null)
				{
					return typedData.AskQuestion();
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				return -1;
			}
			public SurveyQuestionUISupport UISupport
			{
				get
				{
					return SurveyQuestionUISupport.Grouping | SurveyQuestionUISupport.Sorting;
				}
			}
		}
		private sealed class ProvideSurveyQuestionForSurveyErrorState : ISurveyQuestionTypeInfo
		{
			private ProvideSurveyQuestionForSurveyErrorState()
			{
			}
			public static readonly ISurveyQuestionTypeInfo Instance = new ProvideSurveyQuestionForSurveyErrorState();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyErrorState);
				}
			}
			public int AskQuestion(object data)
			{
				IAnswerSurveyQuestion<SurveyErrorState> typedData = data as IAnswerSurveyQuestion<SurveyErrorState>;
				if (typedData != null)
				{
					return typedData.AskQuestion();
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				return -1;
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
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				return answer;
			}
			public SurveyQuestionUISupport UISupport
			{
				get
				{
					return SurveyQuestionUISupport.Glyph;
				}
			}
		}
		private sealed class ProvideSurveyQuestionForSurveyFactTypeDetailType : ISurveyQuestionTypeInfo
		{
			private ProvideSurveyQuestionForSurveyFactTypeDetailType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo Instance = new ProvideSurveyQuestionForSurveyFactTypeDetailType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyFactTypeDetailType);
				}
			}
			public int AskQuestion(object data)
			{
				IAnswerSurveyQuestion<SurveyFactTypeDetailType> typedData = data as IAnswerSurveyQuestion<SurveyFactTypeDetailType>;
				if (typedData != null)
				{
					return typedData.AskQuestion();
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				return -1;
			}
			public SurveyQuestionUISupport UISupport
			{
				get
				{
					return SurveyQuestionUISupport.Grouping | SurveyQuestionUISupport.Sorting;
				}
			}
		}
	}
}
