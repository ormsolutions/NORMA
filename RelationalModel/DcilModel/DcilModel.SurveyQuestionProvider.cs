using System;
using System.Windows.Forms;
using System.Collections.Generic;
using ORMSolutions.ORMArchitect.Framework.Shell;
using ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid;
namespace ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase
{
	partial class ConceptualDatabaseDomainModel : ISurveyQuestionProvider<Microsoft.VisualStudio.Modeling.Store>
	{
		private static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[] mySurveyQuestionTypeInfo1 = new ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[]{
			ProvideSurveyQuestionForSurveySchemaType.Instance};
		private static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[] mySurveyQuestionTypeInfo2 = new ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[]{
			ProvideSurveyQuestionForSurveySchemaChildType.Instance};
		private static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[] mySurveyQuestionTypeInfo3 = new ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[]{
			ProvideSurveyQuestionForSurveyTableChildType.Instance,
			ProvideSurveyQuestionForSurveyTableChildGlyphType.Instance,
			ProvideSurveyQuestionForSurveyColumnClassificationType.Instance};
		private static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[] mySurveyQuestionTypeInfo4 = new ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[]{
			ProvideSurveyQuestionForSurveyReferenceConstraintChildType.Instance};
		private static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[] mySurveyQuestionTypeInfo5 = new ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[]{
			ProvideSurveyQuestionForSurveyColumnReferenceChildType.Instance};
		private static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[] mySurveyQuestionTypeInfo6 = new ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[]{
			ProvideSurveyQuestionForSurveyUniquenessConstraintChildType.Instance};
		/// <summary>Implements <see cref="ISurveyQuestionProvider{Object}.GetSurveyQuestions"/></summary>
		protected static IEnumerable<ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>> GetSurveyQuestions(Microsoft.VisualStudio.Modeling.Store surveyContext, object expansionKey)
		{
			if (expansionKey == null)
			{
				return mySurveyQuestionTypeInfo1;
			}
			else if (expansionKey == Schema.SurveyExpansionKey)
			{
				return mySurveyQuestionTypeInfo2;
			}
			else if (expansionKey == Table.SurveyExpansionKey)
			{
				return mySurveyQuestionTypeInfo3;
			}
			else if (expansionKey == ReferenceConstraint.SurveyExpansionKey)
			{
				return mySurveyQuestionTypeInfo4;
			}
			else if (expansionKey == ColumnReference.SurveyExpansionKey)
			{
				return mySurveyQuestionTypeInfo5;
			}
			else if (expansionKey == UniquenessConstraint.SurveyExpansionKey)
			{
				return mySurveyQuestionTypeInfo6;
			}
			return null;
		}
		IEnumerable<ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>> ISurveyQuestionProvider<Microsoft.VisualStudio.Modeling.Store>.GetSurveyQuestions(Microsoft.VisualStudio.Modeling.Store surveyContext, object expansionKey)
		{
			return GetSurveyQuestions(surveyContext, expansionKey);
		}
		/// <summary>Implements <see cref="ISurveyQuestionProvider{Object}.GetSurveyQuestionImageLists"/></summary>
		protected ImageList[] GetSurveyQuestionImageLists(Microsoft.VisualStudio.Modeling.Store surveyContext)
		{
			return new ImageList[]{
				Resources.SurveyTreeImageList};
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
		private sealed class ProvideSurveyQuestionForSurveySchemaType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveySchemaType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveySchemaType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveySchemaType);
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
				IAnswerSurveyQuestion<SurveySchemaType> typedData = data as IAnswerSurveyQuestion<SurveySchemaType>;
				if (typedData != null)
				{
					return typedData.AskQuestion(contextElement);
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
					return SurveyQuestionUISupport.Grouping | SurveyQuestionUISupport.Sorting | SurveyQuestionUISupport.Glyph;
				}
			}
			public static int QuestionPriority
			{
				get
				{
					return 200;
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
		private sealed class ProvideSurveyQuestionForSurveySchemaChildType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveySchemaChildType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveySchemaChildType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveySchemaChildType);
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
				IAnswerSurveyQuestion<SurveySchemaChildType> typedData = data as IAnswerSurveyQuestion<SurveySchemaChildType>;
				if (typedData != null)
				{
					return typedData.AskQuestion(contextElement);
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				return (int)SurveySchemaType.Last + 1 + answer;
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
					return SurveyQuestionUISupport.Grouping | SurveyQuestionUISupport.Sorting | SurveyQuestionUISupport.Glyph;
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
		private sealed class ProvideSurveyQuestionForSurveyTableChildType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyTableChildType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyTableChildType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyTableChildType);
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
				IAnswerSurveyQuestion<SurveyTableChildType> typedData = data as IAnswerSurveyQuestion<SurveyTableChildType>;
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
		private sealed class ProvideSurveyQuestionForSurveyTableChildGlyphType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyTableChildGlyphType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyTableChildGlyphType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyTableChildGlyphType);
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
				IAnswerSurveyQuestion<SurveyTableChildGlyphType> typedData = data as IAnswerSurveyQuestion<SurveyTableChildGlyphType>;
				if (typedData != null)
				{
					return typedData.AskQuestion(contextElement);
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				return (int)SurveySchemaType.Last + 1 + (int)SurveySchemaChildType.Last + 1 + answer;
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
		private sealed class ProvideSurveyQuestionForSurveyColumnClassificationType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyColumnClassificationType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyColumnClassificationType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyColumnClassificationType);
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
				IAnswerSurveyQuestion<SurveyColumnClassificationType> typedData = data as IAnswerSurveyQuestion<SurveyColumnClassificationType>;
				if (typedData != null)
				{
					return typedData.AskQuestion(contextElement);
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				int retVal;
				switch ((SurveyColumnClassificationType)answer)
				{
					case SurveyColumnClassificationType.PrimaryRequired:
					case SurveyColumnClassificationType.PrimaryNullable:
						retVal = (int)SurveySchemaType.Last + 1 + (int)SurveySchemaChildType.Last + 1 + (int)SurveyTableChildGlyphType.Last + 1 + (int)SurveyReferenceConstraintChildType.Last + 1 + (int)SurveyUniquenessConstraintChildType.Last + 1 + 0;
						break;
					default:
						retVal = -1;
						break;
				}
				return retVal;
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
				SurveyColumnClassificationType typedAnswer = (SurveyColumnClassificationType)answer;
				switch (typedAnswer)
				{
					case SurveyColumnClassificationType.Required:
					case SurveyColumnClassificationType.PrimaryRequired:
						return new SurveyQuestionDisplayData(true, false);
				}
				return SurveyQuestionDisplayData.Default;
			}
			public SurveyQuestionUISupport UISupport
			{
				get
				{
					return SurveyQuestionUISupport.Overlay | SurveyQuestionUISupport.DisplayData;
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
		private sealed class ProvideSurveyQuestionForSurveyReferenceConstraintChildType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyReferenceConstraintChildType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyReferenceConstraintChildType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyReferenceConstraintChildType);
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
				IAnswerSurveyQuestion<SurveyReferenceConstraintChildType> typedData = data as IAnswerSurveyQuestion<SurveyReferenceConstraintChildType>;
				if (typedData != null)
				{
					return typedData.AskQuestion(contextElement);
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				return (int)SurveySchemaType.Last + 1 + (int)SurveySchemaChildType.Last + 1 + (int)SurveyTableChildGlyphType.Last + 1 + answer;
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
					return SurveyQuestionUISupport.Sorting | SurveyQuestionUISupport.Glyph;
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
		private sealed class ProvideSurveyQuestionForSurveyColumnReferenceChildType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyColumnReferenceChildType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyColumnReferenceChildType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyColumnReferenceChildType);
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
				IAnswerSurveyQuestion<SurveyColumnReferenceChildType> typedData = data as IAnswerSurveyQuestion<SurveyColumnReferenceChildType>;
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
		private sealed class ProvideSurveyQuestionForSurveyUniquenessConstraintChildType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyUniquenessConstraintChildType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyUniquenessConstraintChildType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyUniquenessConstraintChildType);
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
				IAnswerSurveyQuestion<SurveyUniquenessConstraintChildType> typedData = data as IAnswerSurveyQuestion<SurveyUniquenessConstraintChildType>;
				if (typedData != null)
				{
					return typedData.AskQuestion(contextElement);
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				return (int)SurveySchemaType.Last + 1 + (int)SurveySchemaChildType.Last + 1 + (int)SurveyTableChildGlyphType.Last + 1 + (int)SurveyReferenceConstraintChildType.Last + 1 + answer;
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
					return SurveyQuestionUISupport.Sorting | SurveyQuestionUISupport.Glyph;
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
