using System;
using System.Windows.Forms;
using System.Collections.Generic;
using ORMSolutions.ORMArchitect.Framework.Shell;
using ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid;
namespace ORMSolutions.ORMArchitect.Framework.Shell
{
	partial class DiagramSurvey : ISurveyQuestionProvider<Microsoft.VisualStudio.Modeling.Store>
	{
		private ProvideSurveyQuestionForDiagramGlyphSurveyType myDynamicDiagramGlyphSurveyTypeQuestionInstance;
		private ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[] mySurveyQuestionTypeInfo1;
		private ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[] EnsureSurveyQuestionTypeInfo1(Microsoft.VisualStudio.Modeling.Store surveyContext)
		{
			return this.mySurveyQuestionTypeInfo1 ?? (this.mySurveyQuestionTypeInfo1 = new ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[]{
				this.myDynamicDiagramGlyphSurveyTypeQuestionInstance ?? (this.myDynamicDiagramGlyphSurveyTypeQuestionInstance = new ProvideSurveyQuestionForDiagramGlyphSurveyType(surveyContext)),
				ProvideSurveyQuestionForDiagramSurveyType.Instance});
		}
		/// <summary>Implements <see cref="ISurveyQuestionProvider{Object}.GetSurveyQuestions"/></summary>
		protected IEnumerable<ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>> GetSurveyQuestions(Microsoft.VisualStudio.Modeling.Store surveyContext, object expansionKey)
		{
			if (expansionKey == null)
			{
				return this.EnsureSurveyQuestionTypeInfo1(surveyContext);
			}
			return null;
		}
		IEnumerable<ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>> ISurveyQuestionProvider<Microsoft.VisualStudio.Modeling.Store>.GetSurveyQuestions(Microsoft.VisualStudio.Modeling.Store surveyContext, object expansionKey)
		{
			return this.GetSurveyQuestions(surveyContext, expansionKey);
		}
		/// <summary>Implements <see cref="ISurveyQuestionProvider{Object}.GetSurveyQuestionImageLists"/></summary>
		protected ImageList[] GetSurveyQuestionImageLists(Microsoft.VisualStudio.Modeling.Store surveyContext)
		{
			// Note that this relies heavily on the current structure of the generated code
			this.EnsureSurveyQuestionTypeInfo1(surveyContext);
			return new ImageList[]{
				((DiagramSurvey.DiagramGlyphSurveyType)this.myDynamicDiagramGlyphSurveyTypeQuestionInstance.DynamicQuestionValues).DiagramImages};
		}
		ImageList[] ISurveyQuestionProvider<Microsoft.VisualStudio.Modeling.Store>.GetSurveyQuestionImageLists(Microsoft.VisualStudio.Modeling.Store surveyContext)
		{
			return this.GetSurveyQuestionImageLists(surveyContext);
		}
		/// <summary>Implements <see cref="ISurveyQuestionProvider{Object}.GetErrorDisplayTypes"/></summary>
		protected static IEnumerable<Type> GetErrorDisplayTypes()
		{
			return null;
		}
		IEnumerable<Type> ISurveyQuestionProvider<Microsoft.VisualStudio.Modeling.Store>.GetErrorDisplayTypes()
		{
			return GetErrorDisplayTypes();
		}
		private sealed class ProvideSurveyQuestionForDiagramSurveyType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForDiagramSurveyType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForDiagramSurveyType();
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
			public int AskQuestion(object data, object contextElement)
			{
				IAnswerSurveyQuestion<DiagramSurveyType> typedData = data as IAnswerSurveyQuestion<DiagramSurveyType>;
				if (typedData != null)
				{
					return typedData.AskQuestion(contextElement);
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				return -1;
			}
			public IFreeFormCommandProvider<Microsoft.VisualStudio.Modeling.Store> GetFreeFormCommands(Microsoft.VisualStudio.Modeling.Store surveyContext, int answer)
			{
				return null;
			}
			public bool ShowEmptyGroup(Microsoft.VisualStudio.Modeling.Store surveyContext, int answer)
			{
				return false;
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
		private sealed class ProvideSurveyQuestionForDiagramGlyphSurveyType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private DiagramGlyphSurveyType myDynamicValues;
			public ProvideSurveyQuestionForDiagramGlyphSurveyType(Microsoft.VisualStudio.Modeling.Store surveyContext)
			{
				this.myDynamicValues = new DiagramGlyphSurveyType(surveyContext);
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
			public int AskQuestion(object data, object contextElement)
			{
				IAnswerSurveyDynamicQuestion<DiagramGlyphSurveyType> typedData = data as IAnswerSurveyDynamicQuestion<DiagramGlyphSurveyType>;
				if (typedData != null)
				{
					return typedData.AskQuestion(this.myDynamicValues, contextElement);
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				return answer;
			}
			public IFreeFormCommandProvider<Microsoft.VisualStudio.Modeling.Store> GetFreeFormCommands(Microsoft.VisualStudio.Modeling.Store surveyContext, int answer)
			{
				return null;
			}
			public bool ShowEmptyGroup(Microsoft.VisualStudio.Modeling.Store surveyContext, int answer)
			{
				return false;
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
