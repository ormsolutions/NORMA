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
		private static readonly ISurveyQuestionTypeInfo[] mySurveyQuestionTypeInfo3 = new ISurveyQuestionTypeInfo[]{
			ProvideSurveyQuestionForSurveyNameGeneratorRefinementType.Instance};
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
			else if (expansionKey == NameGenerator.SurveyExpansionKey)
			{
				return mySurveyQuestionTypeInfo3;
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
		/// <summary>Implements <see cref="ISurveyQuestionProvider.GetErrorDisplayTypes"/></summary>
		protected static IEnumerable<Type> GetErrorDisplayTypes()
		{
			return new Type[]{
				typeof(SurveyErrorState)};
		}
		IEnumerable<Type> ISurveyQuestionProvider.GetErrorDisplayTypes()
		{
			return GetErrorDisplayTypes();
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
			public ISurveyDynamicValues DynamicQuestionValues
			{
				get
				{
					return null;
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
			public SurveyQuestionDisplayData GetDisplayData(int answer)
			{
				return SurveyQuestionDisplayData.Default;
			}
			public SurveyQuestionUISupport UISupport
			{
				get
				{
					return SurveyQuestionUISupport.Grouping | SurveyQuestionUISupport.Sorting;
				}
			}
			public static int QuestionPriority
			{
				get
				{
					return 0;
				}
			}
			int ISurveyQuestionTypeInfo.QuestionPriority
			{
				get
				{
					return QuestionPriority;
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
			public ISurveyDynamicValues DynamicQuestionValues
			{
				get
				{
					return null;
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
				int retVal;
				switch ((SurveyErrorState)answer)
				{
					case SurveyErrorState.HasError:
						retVal = (int)SurveyQuestionGlyph.Last + 1;
						break;
					default:
						retVal = -1;
						break;
				}
				return retVal;
			}
			public SurveyQuestionDisplayData GetDisplayData(int answer)
			{
				return SurveyQuestionDisplayData.Default;
			}
			public SurveyQuestionUISupport UISupport
			{
				get
				{
					return SurveyQuestionUISupport.Overlay;
				}
			}
			public static int QuestionPriority
			{
				get
				{
					return 0;
				}
			}
			int ISurveyQuestionTypeInfo.QuestionPriority
			{
				get
				{
					return QuestionPriority;
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
			public ISurveyDynamicValues DynamicQuestionValues
			{
				get
				{
					return null;
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
			public SurveyQuestionDisplayData GetDisplayData(int answer)
			{
				return SurveyQuestionDisplayData.Default;
			}
			public SurveyQuestionUISupport UISupport
			{
				get
				{
					return SurveyQuestionUISupport.Glyph;
				}
			}
			public static int QuestionPriority
			{
				get
				{
					return 0;
				}
			}
			int ISurveyQuestionTypeInfo.QuestionPriority
			{
				get
				{
					return QuestionPriority;
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
			public ISurveyDynamicValues DynamicQuestionValues
			{
				get
				{
					return null;
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
			public SurveyQuestionDisplayData GetDisplayData(int answer)
			{
				return SurveyQuestionDisplayData.Default;
			}
			public SurveyQuestionUISupport UISupport
			{
				get
				{
					return SurveyQuestionUISupport.Grouping | SurveyQuestionUISupport.Sorting;
				}
			}
			public static int QuestionPriority
			{
				get
				{
					return 0;
				}
			}
			int ISurveyQuestionTypeInfo.QuestionPriority
			{
				get
				{
					return QuestionPriority;
				}
			}
		}
		private sealed class ProvideSurveyQuestionForSurveyNameGeneratorRefinementType : ISurveyQuestionTypeInfo
		{
			private ProvideSurveyQuestionForSurveyNameGeneratorRefinementType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo Instance = new ProvideSurveyQuestionForSurveyNameGeneratorRefinementType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyNameGeneratorRefinementType);
				}
			}
			public ISurveyDynamicValues DynamicQuestionValues
			{
				get
				{
					return null;
				}
			}
			public int AskQuestion(object data)
			{
				IAnswerSurveyQuestion<SurveyNameGeneratorRefinementType> typedData = data as IAnswerSurveyQuestion<SurveyNameGeneratorRefinementType>;
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
			public SurveyQuestionDisplayData GetDisplayData(int answer)
			{
				return SurveyQuestionDisplayData.Default;
			}
			public SurveyQuestionUISupport UISupport
			{
				get
				{
					return SurveyQuestionUISupport.Sorting;
				}
			}
			public static int QuestionPriority
			{
				get
				{
					return 0;
				}
			}
			int ISurveyQuestionTypeInfo.QuestionPriority
			{
				get
				{
					return QuestionPriority;
				}
			}
		}
	}
}
