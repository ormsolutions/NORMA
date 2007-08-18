using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;
namespace Neumont.Tools.RelationalModels.ConceptualDatabase
{
	partial class ConceptualDatabaseDomainModel : ISurveyQuestionProvider
	{
		private static readonly ISurveyQuestionTypeInfo[] mySurveyQuestionTypeInfo1 = new ISurveyQuestionTypeInfo[]{
			ProvideSurveyQuestionForSurveySchemaType.Instance};
		private static readonly ISurveyQuestionTypeInfo[] mySurveyQuestionTypeInfo2 = new ISurveyQuestionTypeInfo[]{
			ProvideSurveyQuestionForSurveySchemaChildType.Instance};
		private static readonly ISurveyQuestionTypeInfo[] mySurveyQuestionTypeInfo3 = new ISurveyQuestionTypeInfo[]{
			ProvideSurveyQuestionForSurveyTableChildType.Instance,
			ProvideSurveyQuestionForSurveyTableChildGlyphType.Instance,
			ProvideSurveyQuestionForSurveyColumnClassificationType.Instance};
		private static readonly ISurveyQuestionTypeInfo[] mySurveyQuestionTypeInfo4 = new ISurveyQuestionTypeInfo[]{
			ProvideSurveyQuestionForSurveyReferenceConstraintChildType.Instance};
		private static readonly ISurveyQuestionTypeInfo[] mySurveyQuestionTypeInfo5 = new ISurveyQuestionTypeInfo[]{
			ProvideSurveyQuestionForSurveyUniquenessConstraintChildType.Instance};
		/// <summary>Implements <see cref="ISurveyQuestionProvider.GetSurveyQuestions"/></summary>
		protected static IEnumerable<ISurveyQuestionTypeInfo> GetSurveyQuestions(object expansionKey)
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
			else if (expansionKey == UniquenessConstraint.SurveyExpansionKey)
			{
				return mySurveyQuestionTypeInfo5;
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
				return Resources.SurveyTreeImageList;
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
		private sealed class ProvideSurveyQuestionForSurveySchemaType : ISurveyQuestionTypeInfo
		{
			private ProvideSurveyQuestionForSurveySchemaType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo Instance = new ProvideSurveyQuestionForSurveySchemaType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveySchemaType);
				}
			}
			public int AskQuestion(object data)
			{
				IAnswerSurveyQuestion<SurveySchemaType> typedData = data as IAnswerSurveyQuestion<SurveySchemaType>;
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
					return SurveyQuestionUISupport.Grouping | (SurveyQuestionUISupport.Sorting | SurveyQuestionUISupport.Glyph);
				}
			}
		}
		private sealed class ProvideSurveyQuestionForSurveySchemaChildType : ISurveyQuestionTypeInfo
		{
			private ProvideSurveyQuestionForSurveySchemaChildType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo Instance = new ProvideSurveyQuestionForSurveySchemaChildType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveySchemaChildType);
				}
			}
			public int AskQuestion(object data)
			{
				IAnswerSurveyQuestion<SurveySchemaChildType> typedData = data as IAnswerSurveyQuestion<SurveySchemaChildType>;
				if (typedData != null)
				{
					return typedData.AskQuestion();
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				return ((int)SurveySchemaType.Last + 1) + answer;
			}
			public SurveyQuestionDisplayData GetDisplayData(int answer)
			{
				return SurveyQuestionDisplayData.Default;
			}
			public SurveyQuestionUISupport UISupport
			{
				get
				{
					return SurveyQuestionUISupport.Grouping | (SurveyQuestionUISupport.Sorting | SurveyQuestionUISupport.Glyph);
				}
			}
		}
		private sealed class ProvideSurveyQuestionForSurveyTableChildType : ISurveyQuestionTypeInfo
		{
			private ProvideSurveyQuestionForSurveyTableChildType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo Instance = new ProvideSurveyQuestionForSurveyTableChildType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyTableChildType);
				}
			}
			public int AskQuestion(object data)
			{
				IAnswerSurveyQuestion<SurveyTableChildType> typedData = data as IAnswerSurveyQuestion<SurveyTableChildType>;
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
		}
		private sealed class ProvideSurveyQuestionForSurveyTableChildGlyphType : ISurveyQuestionTypeInfo
		{
			private ProvideSurveyQuestionForSurveyTableChildGlyphType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo Instance = new ProvideSurveyQuestionForSurveyTableChildGlyphType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyTableChildGlyphType);
				}
			}
			public int AskQuestion(object data)
			{
				IAnswerSurveyQuestion<SurveyTableChildGlyphType> typedData = data as IAnswerSurveyQuestion<SurveyTableChildGlyphType>;
				if (typedData != null)
				{
					return typedData.AskQuestion();
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				return (((int)SurveySchemaType.Last + 1) + ((int)SurveySchemaChildType.Last + 1)) + answer;
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
		}
		private sealed class ProvideSurveyQuestionForSurveyColumnClassificationType : ISurveyQuestionTypeInfo
		{
			private ProvideSurveyQuestionForSurveyColumnClassificationType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo Instance = new ProvideSurveyQuestionForSurveyColumnClassificationType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyColumnClassificationType);
				}
			}
			public int AskQuestion(object data)
			{
				IAnswerSurveyQuestion<SurveyColumnClassificationType> typedData = data as IAnswerSurveyQuestion<SurveyColumnClassificationType>;
				if (typedData != null)
				{
					return typedData.AskQuestion();
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
						retVal = ((((((int)SurveySchemaType.Last + 1) + ((int)SurveySchemaChildType.Last + 1)) + ((int)SurveyTableChildGlyphType.Last + 1)) + ((int)SurveyReferenceConstraintChildType.Last + 1)) + ((int)SurveyUniquenessConstraintChildType.Last + 1)) + 0;
						break;
					default:
						retVal = -1;
						break;
				}
				return retVal;
			}
			public SurveyQuestionDisplayData GetDisplayData(int answer)
			{
				switch ((SurveyColumnClassificationType)answer)
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
		}
		private sealed class ProvideSurveyQuestionForSurveyReferenceConstraintChildType : ISurveyQuestionTypeInfo
		{
			private ProvideSurveyQuestionForSurveyReferenceConstraintChildType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo Instance = new ProvideSurveyQuestionForSurveyReferenceConstraintChildType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyReferenceConstraintChildType);
				}
			}
			public int AskQuestion(object data)
			{
				IAnswerSurveyQuestion<SurveyReferenceConstraintChildType> typedData = data as IAnswerSurveyQuestion<SurveyReferenceConstraintChildType>;
				if (typedData != null)
				{
					return typedData.AskQuestion();
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				return ((((int)SurveySchemaType.Last + 1) + ((int)SurveySchemaChildType.Last + 1)) + ((int)SurveyTableChildGlyphType.Last + 1)) + answer;
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
		}
		private sealed class ProvideSurveyQuestionForSurveyUniquenessConstraintChildType : ISurveyQuestionTypeInfo
		{
			private ProvideSurveyQuestionForSurveyUniquenessConstraintChildType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo Instance = new ProvideSurveyQuestionForSurveyUniquenessConstraintChildType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyUniquenessConstraintChildType);
				}
			}
			public int AskQuestion(object data)
			{
				IAnswerSurveyQuestion<SurveyUniquenessConstraintChildType> typedData = data as IAnswerSurveyQuestion<SurveyUniquenessConstraintChildType>;
				if (typedData != null)
				{
					return typedData.AskQuestion();
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				return (((((int)SurveySchemaType.Last + 1) + ((int)SurveySchemaChildType.Last + 1)) + ((int)SurveyTableChildGlyphType.Last + 1)) + ((int)SurveyReferenceConstraintChildType.Last + 1)) + answer;
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
		}
	}
}
