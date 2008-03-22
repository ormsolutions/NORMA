using System;
using System.Windows.Forms;
using System.Collections.Generic;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;
namespace Neumont.Tools.EntityRelationshipModels.Barker
{
	partial class BarkerDomainModel : ISurveyQuestionProvider
	{
		private static readonly ISurveyQuestionTypeInfo[] mySurveyQuestionTypeInfo1 = new ISurveyQuestionTypeInfo[]{
			ProvideSurveyQuestionForSurveyBarkerModelType.Instance};
		private static readonly ISurveyQuestionTypeInfo[] mySurveyQuestionTypeInfo2 = new ISurveyQuestionTypeInfo[]{
			ProvideSurveyQuestionForSurveyBarkerElementType.Instance};
		private static readonly ISurveyQuestionTypeInfo[] mySurveyQuestionTypeInfo3 = new ISurveyQuestionTypeInfo[]{
			ProvideSurveyQuestionForSurveyEntityChildType.Instance};
		private static readonly ISurveyQuestionTypeInfo[] mySurveyQuestionTypeInfo4 = new ISurveyQuestionTypeInfo[]{
			ProvideSurveyQuestionForSurveyBinaryAssociationChildType.Instance};
		private static readonly ISurveyQuestionTypeInfo[] mySurveyQuestionTypeInfo5 = new ISurveyQuestionTypeInfo[]{
			ProvideSurveyQuestionForSurveyAttributeChildType.Instance};
		private static readonly ISurveyQuestionTypeInfo[] mySurveyQuestionTypeInfo6 = new ISurveyQuestionTypeInfo[]{
			ProvideSurveyQuestionForSurveyRoleChildType.Instance};
		private static readonly ISurveyQuestionTypeInfo[] mySurveyQuestionTypeInfo7 = new ISurveyQuestionTypeInfo[]{
			ProvideSurveyQuestionForSurveyExclusiveArcChildType.Instance};
		/// <summary>Implements <see cref="ISurveyQuestionProvider.GetSurveyQuestions"/></summary>
		protected static IEnumerable<ISurveyQuestionTypeInfo> GetSurveyQuestions(object expansionKey)
		{
			if (expansionKey == null)
			{
				return mySurveyQuestionTypeInfo1;
			}
			else if (expansionKey == BarkerErModel.SurveyExpansionKey)
			{
				return mySurveyQuestionTypeInfo2;
			}
			else if (expansionKey == EntityType.SurveyExpansionKey)
			{
				return mySurveyQuestionTypeInfo3;
			}
			else if (expansionKey == BinaryAssociation.SurveyExpansionKey)
			{
				return mySurveyQuestionTypeInfo4;
			}
			else if (expansionKey == Attribute.SurveyExpansionKey)
			{
				return mySurveyQuestionTypeInfo5;
			}
			else if (expansionKey == Role.SurveyExpansionKey)
			{
				return mySurveyQuestionTypeInfo6;
			}
			else if (expansionKey == ExclusiveArc.SurveyExpansionKey)
			{
				return mySurveyQuestionTypeInfo7;
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
			return null;
		}
		IEnumerable<Type> ISurveyQuestionProvider.GetErrorDisplayTypes()
		{
			return GetErrorDisplayTypes();
		}
		private sealed class ProvideSurveyQuestionForSurveyBarkerModelType : ISurveyQuestionTypeInfo
		{
			private ProvideSurveyQuestionForSurveyBarkerModelType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo Instance = new ProvideSurveyQuestionForSurveyBarkerModelType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyBarkerModelType);
				}
			}
			public int AskQuestion(object data)
			{
				IAnswerSurveyQuestion<SurveyBarkerModelType> typedData = data as IAnswerSurveyQuestion<SurveyBarkerModelType>;
				if (typedData != null)
				{
					return typedData.AskQuestion();
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				int retVal;
				switch ((SurveyBarkerModelType)answer)
				{
					case SurveyBarkerModelType.Model:
						retVal = 0;
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
					return SurveyQuestionUISupport.Grouping | SurveyQuestionUISupport.Sorting | SurveyQuestionUISupport.Glyph;
				}
			}
		}
		private sealed class ProvideSurveyQuestionForSurveyBarkerElementType : ISurveyQuestionTypeInfo
		{
			private ProvideSurveyQuestionForSurveyBarkerElementType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo Instance = new ProvideSurveyQuestionForSurveyBarkerElementType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyBarkerElementType);
				}
			}
			public int AskQuestion(object data)
			{
				IAnswerSurveyQuestion<SurveyBarkerElementType> typedData = data as IAnswerSurveyQuestion<SurveyBarkerElementType>;
				if (typedData != null)
				{
					return typedData.AskQuestion();
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				int retVal;
				switch ((SurveyBarkerElementType)answer)
				{
					case SurveyBarkerElementType.EntityType:
						retVal = 1;
						break;
					case SurveyBarkerElementType.BinaryAssociation:
						retVal = 4;
						break;
					case SurveyBarkerElementType.Constraint:
						retVal = 10;
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
					return SurveyQuestionUISupport.Grouping | SurveyQuestionUISupport.Sorting | SurveyQuestionUISupport.Glyph;
				}
			}
		}
		private sealed class ProvideSurveyQuestionForSurveyEntityChildType : ISurveyQuestionTypeInfo
		{
			private ProvideSurveyQuestionForSurveyEntityChildType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo Instance = new ProvideSurveyQuestionForSurveyEntityChildType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyEntityChildType);
				}
			}
			public int AskQuestion(object data)
			{
				IAnswerSurveyQuestion<SurveyEntityChildType> typedData = data as IAnswerSurveyQuestion<SurveyEntityChildType>;
				if (typedData != null)
				{
					return typedData.AskQuestion();
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				int retVal;
				switch ((SurveyEntityChildType)answer)
				{
					case SurveyEntityChildType.Attribute:
						retVal = 2;
						break;
					case SurveyEntityChildType.Subtype:
						retVal = 13;
						break;
					case SurveyEntityChildType.RoleRef:
						retVal = 14;
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
					return SurveyQuestionUISupport.Grouping | SurveyQuestionUISupport.Sorting | SurveyQuestionUISupport.Glyph;
				}
			}
		}
		private sealed class ProvideSurveyQuestionForSurveyBinaryAssociationChildType : ISurveyQuestionTypeInfo
		{
			private ProvideSurveyQuestionForSurveyBinaryAssociationChildType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo Instance = new ProvideSurveyQuestionForSurveyBinaryAssociationChildType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyBinaryAssociationChildType);
				}
			}
			public int AskQuestion(object data)
			{
				IAnswerSurveyQuestion<SurveyBinaryAssociationChildType> typedData = data as IAnswerSurveyQuestion<SurveyBinaryAssociationChildType>;
				if (typedData != null)
				{
					return typedData.AskQuestion();
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				int retVal;
				switch ((SurveyBinaryAssociationChildType)answer)
				{
					case SurveyBinaryAssociationChildType.Role_OptionalOne:
						retVal = 6;
						break;
					case SurveyBinaryAssociationChildType.Role_MandatoryOne:
						retVal = 8;
						break;
					case SurveyBinaryAssociationChildType.Role_MandatoryMany:
						retVal = 7;
						break;
					case SurveyBinaryAssociationChildType.Role_OptionalMany:
						retVal = 5;
						break;
					case SurveyBinaryAssociationChildType.Role_MandatoryOnePrimary:
						retVal = 12;
						break;
					case SurveyBinaryAssociationChildType.Role_MandatoryManyPrimary:
						retVal = 11;
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
					return SurveyQuestionUISupport.Glyph;
				}
			}
		}
		private sealed class ProvideSurveyQuestionForSurveyAttributeChildType : ISurveyQuestionTypeInfo
		{
			private ProvideSurveyQuestionForSurveyAttributeChildType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo Instance = new ProvideSurveyQuestionForSurveyAttributeChildType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyAttributeChildType);
				}
			}
			public int AskQuestion(object data)
			{
				IAnswerSurveyQuestion<SurveyAttributeChildType> typedData = data as IAnswerSurveyQuestion<SurveyAttributeChildType>;
				if (typedData != null)
				{
					return typedData.AskQuestion();
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				int retVal;
				switch ((SurveyAttributeChildType)answer)
				{
					case SurveyAttributeChildType.PossibleValue:
						retVal = 3;
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
					return SurveyQuestionUISupport.Grouping | SurveyQuestionUISupport.Sorting | SurveyQuestionUISupport.Glyph;
				}
			}
		}
		private sealed class ProvideSurveyQuestionForSurveyRoleChildType : ISurveyQuestionTypeInfo
		{
			private ProvideSurveyQuestionForSurveyRoleChildType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo Instance = new ProvideSurveyQuestionForSurveyRoleChildType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyRoleChildType);
				}
			}
			public int AskQuestion(object data)
			{
				IAnswerSurveyQuestion<SurveyRoleChildType> typedData = data as IAnswerSurveyQuestion<SurveyRoleChildType>;
				if (typedData != null)
				{
					return typedData.AskQuestion();
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				int retVal;
				switch ((SurveyRoleChildType)answer)
				{
					case SurveyRoleChildType.CardinalityQualifier:
						retVal = 9;
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
					return SurveyQuestionUISupport.Grouping | SurveyQuestionUISupport.Sorting | SurveyQuestionUISupport.Glyph;
				}
			}
		}
		private sealed class ProvideSurveyQuestionForSurveyExclusiveArcChildType : ISurveyQuestionTypeInfo
		{
			private ProvideSurveyQuestionForSurveyExclusiveArcChildType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo Instance = new ProvideSurveyQuestionForSurveyExclusiveArcChildType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyExclusiveArcChildType);
				}
			}
			public int AskQuestion(object data)
			{
				IAnswerSurveyQuestion<SurveyExclusiveArcChildType> typedData = data as IAnswerSurveyQuestion<SurveyExclusiveArcChildType>;
				if (typedData != null)
				{
					return typedData.AskQuestion();
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				int retVal;
				switch ((SurveyExclusiveArcChildType)answer)
				{
					case SurveyExclusiveArcChildType.RoleRef:
						retVal = 14;
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
					return SurveyQuestionUISupport.Grouping | SurveyQuestionUISupport.Sorting | SurveyQuestionUISupport.Glyph;
				}
			}
		}
	}
}
