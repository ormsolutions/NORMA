using System;
using System.Windows.Forms;
using System.Collections.Generic;
using ORMSolutions.ORMArchitect.Framework.Shell;
using ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid;
namespace ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker
{
	partial class BarkerDomainModel : ISurveyQuestionProvider<Microsoft.VisualStudio.Modeling.Store>
	{
		private static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[] mySurveyQuestionTypeInfo1 = new ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[]{
			ProvideSurveyQuestionForSurveyBarkerModelType.Instance};
		private static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[] mySurveyQuestionTypeInfo2 = new ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[]{
			ProvideSurveyQuestionForSurveyBarkerElementType.Instance};
		private static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[] mySurveyQuestionTypeInfo3 = new ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[]{
			ProvideSurveyQuestionForSurveyEntityChildType.Instance};
		private static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[] mySurveyQuestionTypeInfo4 = new ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[]{
			ProvideSurveyQuestionForSurveyBinaryAssociationChildType.Instance};
		private static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[] mySurveyQuestionTypeInfo5 = new ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[]{
			ProvideSurveyQuestionForSurveyAttributeChildType.Instance};
		private static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[] mySurveyQuestionTypeInfo6 = new ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[]{
			ProvideSurveyQuestionForSurveyRoleChildType.Instance};
		private static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[] mySurveyQuestionTypeInfo7 = new ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[]{
			ProvideSurveyQuestionForSurveyExclusiveArcChildType.Instance};
		/// <summary>Implements <see cref="ISurveyQuestionProvider{Object}.GetSurveyQuestions"/></summary>
		protected static IEnumerable<ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>> GetSurveyQuestions(Microsoft.VisualStudio.Modeling.Store surveyContext, object expansionKey)
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
		IEnumerable<ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>> ISurveyQuestionProvider<Microsoft.VisualStudio.Modeling.Store>.GetSurveyQuestions(Microsoft.VisualStudio.Modeling.Store surveyContext, object expansionKey)
		{
			return GetSurveyQuestions(surveyContext, expansionKey);
		}
		/// <summary>Implements <see cref="ISurveyQuestionProvider{Object}.GetSurveyQuestionImageLists"/></summary>
		protected ImageList[] GetSurveyQuestionImageLists(Microsoft.VisualStudio.Modeling.Store surveyContext)
		{
			return new ImageList[]{
				ResourceStrings.SurveyTreeImageList};
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
		private sealed class ProvideSurveyQuestionForSurveyBarkerModelType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyBarkerModelType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyBarkerModelType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyBarkerModelType);
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
				IAnswerSurveyQuestion<SurveyBarkerModelType> typedData = data as IAnswerSurveyQuestion<SurveyBarkerModelType>;
				if (typedData != null)
				{
					return typedData.AskQuestion(contextElement);
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
					return 400;
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
		private sealed class ProvideSurveyQuestionForSurveyBarkerElementType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyBarkerElementType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyBarkerElementType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyBarkerElementType);
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
				IAnswerSurveyQuestion<SurveyBarkerElementType> typedData = data as IAnswerSurveyQuestion<SurveyBarkerElementType>;
				if (typedData != null)
				{
					return typedData.AskQuestion(contextElement);
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
		private sealed class ProvideSurveyQuestionForSurveyEntityChildType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyEntityChildType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyEntityChildType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyEntityChildType);
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
				IAnswerSurveyQuestion<SurveyEntityChildType> typedData = data as IAnswerSurveyQuestion<SurveyEntityChildType>;
				if (typedData != null)
				{
					return typedData.AskQuestion(contextElement);
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
		private sealed class ProvideSurveyQuestionForSurveyBinaryAssociationChildType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyBinaryAssociationChildType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyBinaryAssociationChildType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyBinaryAssociationChildType);
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
				IAnswerSurveyQuestion<SurveyBinaryAssociationChildType> typedData = data as IAnswerSurveyQuestion<SurveyBinaryAssociationChildType>;
				if (typedData != null)
				{
					return typedData.AskQuestion(contextElement);
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
		private sealed class ProvideSurveyQuestionForSurveyAttributeChildType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyAttributeChildType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyAttributeChildType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyAttributeChildType);
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
				IAnswerSurveyQuestion<SurveyAttributeChildType> typedData = data as IAnswerSurveyQuestion<SurveyAttributeChildType>;
				if (typedData != null)
				{
					return typedData.AskQuestion(contextElement);
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
		private sealed class ProvideSurveyQuestionForSurveyRoleChildType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyRoleChildType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyRoleChildType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyRoleChildType);
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
				IAnswerSurveyQuestion<SurveyRoleChildType> typedData = data as IAnswerSurveyQuestion<SurveyRoleChildType>;
				if (typedData != null)
				{
					return typedData.AskQuestion(contextElement);
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
		private sealed class ProvideSurveyQuestionForSurveyExclusiveArcChildType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyExclusiveArcChildType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyExclusiveArcChildType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyExclusiveArcChildType);
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
				IAnswerSurveyQuestion<SurveyExclusiveArcChildType> typedData = data as IAnswerSurveyQuestion<SurveyExclusiveArcChildType>;
				if (typedData != null)
				{
					return typedData.AskQuestion(contextElement);
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
	}
}
