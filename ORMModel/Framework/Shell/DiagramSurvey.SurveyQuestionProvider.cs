using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;
namespace Neumont.Tools.Modeling.Shell
{
	partial class DiagramSurvey : ISurveyQuestionProvider
	{
		private ProvideSurveyQuestionForDiagramGlyphSurveyType myDynamicDiagramGlyphSurveyTypeQuestionInstance;
		private ISurveyQuestionTypeInfo[] mySurveyQuestionTypeInfo1;
		private ISurveyQuestionTypeInfo[] EnsureSurveyQuestionTypeInfo1()
		{
			return this.mySurveyQuestionTypeInfo1 ?? (this.mySurveyQuestionTypeInfo1 = new ISurveyQuestionTypeInfo[]{
				this.myDynamicDiagramGlyphSurveyTypeQuestionInstance ?? (this.myDynamicDiagramGlyphSurveyTypeQuestionInstance = new ProvideSurveyQuestionForDiagramGlyphSurveyType(this.Store)),
				ProvideSurveyQuestionForDiagramSurveyType.Instance});
		}
		/// <summary>Implements <see cref="ISurveyQuestionProvider.GetSurveyQuestions"/></summary>
		protected IEnumerable<ISurveyQuestionTypeInfo> GetSurveyQuestions(object expansionKey)
		{
			if (expansionKey == null)
			{
				return this.EnsureSurveyQuestionTypeInfo1();
			}
			return null;
		}
		IEnumerable<ISurveyQuestionTypeInfo> ISurveyQuestionProvider.GetSurveyQuestions(object expansionKey)
		{
			return this.GetSurveyQuestions(expansionKey);
		}
		/// <summary>Implements <see cref="ISurveyQuestionProvider.SurveyQuestionImageList"/></summary>
		protected ImageList SurveyQuestionImageList
		{
			get
			{
				// Note that this relies heavily on the current structure of the generated code
				this.EnsureSurveyQuestionTypeInfo1();
				return ((DiagramSurvey.DiagramGlyphSurveyType)this.myDynamicDiagramGlyphSurveyTypeQuestionInstance.DynamicQuestionValues).DiagramImages;
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
			return null;
		}
		IEnumerable<Type> ISurveyQuestionProvider.GetErrorDisplayTypes()
		{
			return GetErrorDisplayTypes();
		}
		private sealed class ProvideSurveyQuestionForDiagramSurveyType : ISurveyQuestionTypeInfo
		{
			private ProvideSurveyQuestionForDiagramSurveyType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo Instance = new ProvideSurveyQuestionForDiagramSurveyType();
			public Type QuestionType
			{
				get
				{
					return typeof(DiagramSurveyType);
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
				IAnswerSurveyQuestion<DiagramSurveyType> typedData = data as IAnswerSurveyQuestion<DiagramSurveyType>;
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
					return -1000;
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
		private sealed class ProvideSurveyQuestionForDiagramGlyphSurveyType : ISurveyQuestionTypeInfo
		{
			private DiagramGlyphSurveyType myDynamicValues;
			public ProvideSurveyQuestionForDiagramGlyphSurveyType(Store store)
			{
				this.myDynamicValues = new DiagramGlyphSurveyType(store);
			}
			public Type QuestionType
			{
				get
				{
					return typeof(DiagramGlyphSurveyType);
				}
			}
			public ISurveyDynamicValues DynamicQuestionValues
			{
				get
				{
					return this.myDynamicValues;
				}
			}
			public int AskQuestion(object data)
			{
				IAnswerSurveyDynamicQuestion<DiagramGlyphSurveyType> typedData = data as IAnswerSurveyDynamicQuestion<DiagramGlyphSurveyType>;
				if (typedData != null)
				{
					return typedData.AskQuestion(this.myDynamicValues);
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
	}
}
