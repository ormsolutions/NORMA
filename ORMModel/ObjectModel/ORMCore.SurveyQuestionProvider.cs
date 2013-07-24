using System;
using System.Windows.Forms;
using System.Collections.Generic;
using ORMSolutions.ORMArchitect.Framework.Shell;
using ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid;
namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	partial class ORMCoreDomainModel : ISurveyQuestionProvider<Microsoft.VisualStudio.Modeling.Store>
	{
		private ProvideSurveyQuestionForSurveyGroupingTypeGlyph myDynamicSurveyGroupingTypeGlyphQuestionInstance;
		private static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[] mySurveyQuestionTypeInfo1 = new ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[]{
			ProvideSurveyQuestionForSurveyElementType.Instance,
			ProvideSurveyQuestionForSurveyErrorState.Instance,
			ProvideSurveyQuestionForSurveyQuestionGlyph.Instance,
			ProvideSurveyQuestionForSurveyDerivationType.Instance};
		private static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[] mySurveyQuestionTypeInfo2 = new ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[]{
			ProvideSurveyQuestionForSurveyFactTypeDetailType.Instance,
			ProvideSurveyQuestionForSurveyErrorState.Instance,
			ProvideSurveyQuestionForSurveyQuestionGlyph.Instance,
			ProvideSurveyQuestionForSurveyRoleType.Instance};
		private static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[] mySurveyQuestionTypeInfo3 = new ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[]{
			ProvideSurveyQuestionForSurveyNameGeneratorRefinementType.Instance};
		private ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[] mySurveyQuestionTypeInfo4;
		private ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[] EnsureSurveyQuestionTypeInfo4(Microsoft.VisualStudio.Modeling.Store surveyContext)
		{
			return this.mySurveyQuestionTypeInfo4 ?? (this.mySurveyQuestionTypeInfo4 = new ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[]{
				ProvideSurveyQuestionForSurveyGroupingChildType.Instance,
				this.myDynamicSurveyGroupingTypeGlyphQuestionInstance ?? (this.myDynamicSurveyGroupingTypeGlyphQuestionInstance = new ProvideSurveyQuestionForSurveyGroupingTypeGlyph(surveyContext)),
				ProvideSurveyQuestionForSurveyGroupingReferenceType.Instance});
		}
		private static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[] mySurveyQuestionTypeInfo5 = new ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[]{
			ProvideSurveyQuestionForSurveyQuestionGlyph.Instance,
			ProvideSurveyQuestionForSurveyQueryParameterType.Instance};
		private static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[] mySurveyQuestionTypeInfo6 = new ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[]{
			ProvideSurveyQuestionForSurveyConstraintDetailType.Instance};
		private static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[] mySurveyQuestionTypeInfo7 = new ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>[]{
			ProvideSurveyQuestionForSurveyConstraintDetailType.Instance};
		/// <summary>Implements <see cref="ISurveyQuestionProvider{Object}.GetSurveyQuestions"/></summary>
		protected IEnumerable<ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>> GetSurveyQuestions(Microsoft.VisualStudio.Modeling.Store surveyContext, object expansionKey)
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
			else if (expansionKey == ElementGrouping.SurveyExpansionKey)
			{
				return this.EnsureSurveyQuestionTypeInfo4(surveyContext);
			}
			else if (expansionKey == ORMCoreDomainModel.SurveyFloatingExpansionKey)
			{
				return mySurveyQuestionTypeInfo5;
			}
			else if (expansionKey == SetConstraint.SurveyExpansionKey)
			{
				return mySurveyQuestionTypeInfo6;
			}
			else if (expansionKey == SetComparisonConstraint.SurveyExpansionKey)
			{
				return mySurveyQuestionTypeInfo7;
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
			this.EnsureSurveyQuestionTypeInfo4(surveyContext);
			return new ImageList[]{
				ResourceStrings.SurveyTreeImageList,
				((SurveyGroupingTypeGlyph)this.myDynamicSurveyGroupingTypeGlyphQuestionInstance.DynamicQuestionValues).GroupingTypeImages};
		}
		ImageList[] ISurveyQuestionProvider<Microsoft.VisualStudio.Modeling.Store>.GetSurveyQuestionImageLists(Microsoft.VisualStudio.Modeling.Store surveyContext)
		{
			return this.GetSurveyQuestionImageLists(surveyContext);
		}
		/// <summary>Implements <see cref="ISurveyQuestionProvider{Object}.GetErrorDisplayTypes"/></summary>
		protected static IEnumerable<Type> GetErrorDisplayTypes()
		{
			return new Type[]{
				typeof(SurveyErrorState)};
		}
		IEnumerable<Type> ISurveyQuestionProvider<Microsoft.VisualStudio.Modeling.Store>.GetErrorDisplayTypes()
		{
			return GetErrorDisplayTypes();
		}
		private sealed class ProvideSurveyQuestionForSurveyElementType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyElementType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyElementType();
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
			public int AskQuestion(object data, object contextElement)
			{
				IAnswerSurveyQuestion<SurveyElementType> typedData = data as IAnswerSurveyQuestion<SurveyElementType>;
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
				switch ((SurveyElementType)answer)
				{
					case SurveyElementType.Grouping:
						return FreeFormElementGroupingCommands;
				}
				return null;
			}
			public bool ShowEmptyGroup(Microsoft.VisualStudio.Modeling.Store surveyContext, int answer)
			{
				switch ((SurveyElementType)answer)
				{
					case SurveyElementType.Grouping:
						return true;
					default:
						return false;
				}
			}
			public SurveyQuestionDisplayData GetDisplayData(int answer)
			{
				return SurveyQuestionDisplayData.Default;
			}
			public SurveyQuestionUISupport UISupport
			{
				get
				{
					return SurveyQuestionUISupport.Grouping | SurveyQuestionUISupport.Sorting | SurveyQuestionUISupport.EmptyGroups;
				}
			}
			public static int QuestionPriority
			{
				get
				{
					return -500;
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
		private sealed class ProvideSurveyQuestionForSurveyConstraintDetailType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyConstraintDetailType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyConstraintDetailType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyConstraintDetailType);
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
				IAnswerSurveyQuestion<SurveyConstraintDetailType> typedData = data as IAnswerSurveyQuestion<SurveyConstraintDetailType>;
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
					return 1000;
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
		private sealed class ProvideSurveyQuestionForSurveyErrorState : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyErrorState()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyErrorState();
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
			public int AskQuestion(object data, object contextElement)
			{
				IAnswerSurveyQuestion<SurveyErrorState> typedData = data as IAnswerSurveyQuestion<SurveyErrorState>;
				if (typedData != null)
				{
					return typedData.AskQuestion(contextElement);
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				int retVal;
				switch ((SurveyErrorState)answer)
				{
					case SurveyErrorState.HasError:
						retVal = (int)SurveyQuestionGlyph.Last + 1 + 0;
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
		private sealed class ProvideSurveyQuestionForSurveyQuestionGlyph : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyQuestionGlyph()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyQuestionGlyph();
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
			public int AskQuestion(object data, object contextElement)
			{
				IAnswerSurveyQuestion<SurveyQuestionGlyph> typedData = data as IAnswerSurveyQuestion<SurveyQuestionGlyph>;
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
		private sealed class ProvideSurveyQuestionForSurveyFactTypeDetailType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyFactTypeDetailType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyFactTypeDetailType();
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
			public int AskQuestion(object data, object contextElement)
			{
				IAnswerSurveyQuestion<SurveyFactTypeDetailType> typedData = data as IAnswerSurveyQuestion<SurveyFactTypeDetailType>;
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
		private sealed class ProvideSurveyQuestionForSurveyRoleType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyRoleType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyRoleType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyRoleType);
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
				IAnswerSurveyQuestion<SurveyRoleType> typedData = data as IAnswerSurveyQuestion<SurveyRoleType>;
				if (typedData != null)
				{
					return typedData.AskQuestion(contextElement);
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				return (int)SurveyQuestionGlyph.Last + 1 + 1 + answer;
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
		private sealed class ProvideSurveyQuestionForSurveyNameGeneratorRefinementType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyNameGeneratorRefinementType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyNameGeneratorRefinementType();
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
			public int AskQuestion(object data, object contextElement)
			{
				IAnswerSurveyQuestion<SurveyNameGeneratorRefinementType> typedData = data as IAnswerSurveyQuestion<SurveyNameGeneratorRefinementType>;
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
		private sealed class ProvideSurveyQuestionForSurveyGroupingChildType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyGroupingChildType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyGroupingChildType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyGroupingChildType);
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
				IAnswerSurveyQuestion<SurveyGroupingChildType> typedData = data as IAnswerSurveyQuestion<SurveyGroupingChildType>;
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
		private sealed class ProvideSurveyQuestionForSurveyGroupingTypeGlyph : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private SurveyGroupingTypeGlyph myDynamicValues;
			public ProvideSurveyQuestionForSurveyGroupingTypeGlyph(Microsoft.VisualStudio.Modeling.Store surveyContext)
			{
				this.myDynamicValues = new SurveyGroupingTypeGlyph(surveyContext);
			}
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyGroupingTypeGlyph);
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
				IAnswerSurveyDynamicQuestion<SurveyGroupingTypeGlyph> typedData = data as IAnswerSurveyDynamicQuestion<SurveyGroupingTypeGlyph>;
				if (typedData != null)
				{
					return typedData.AskQuestion(this.myDynamicValues, contextElement);
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				return (int)SurveyQuestionGlyph.Last + 1 + 1 + (int)SurveyRoleType.Supertype + 1 + 2 + (int)SurveyDerivationType.Query + 1 + (int)SurveyQueryParameterType.Input + 1 + answer;
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
		private sealed class ProvideSurveyQuestionForSurveyGroupingReferenceType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyGroupingReferenceType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyGroupingReferenceType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyGroupingReferenceType);
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
				IAnswerSurveyQuestion<SurveyGroupingReferenceType> typedData = data as IAnswerSurveyQuestion<SurveyGroupingReferenceType>;
				if (typedData != null)
				{
					return typedData.AskQuestion(contextElement);
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				int retVal;
				switch ((SurveyGroupingReferenceType)answer)
				{
					case SurveyGroupingReferenceType.Exclusion:
						retVal = (int)SurveyQuestionGlyph.Last + 1 + 1 + (int)SurveyRoleType.Supertype + 1 + 0;
						break;
					case SurveyGroupingReferenceType.Contradiction:
						retVal = (int)SurveyQuestionGlyph.Last + 1 + 1 + (int)SurveyRoleType.Supertype + 1 + 1;
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
				SurveyGroupingReferenceType typedAnswer = (SurveyGroupingReferenceType)answer;
				switch (typedAnswer)
				{
					case SurveyGroupingReferenceType.Exclusion:
					case SurveyGroupingReferenceType.Contradiction:
						return new SurveyQuestionDisplayData(false, true);
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
		private sealed class ProvideSurveyQuestionForSurveyDerivationType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyDerivationType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyDerivationType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyDerivationType);
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
				IAnswerSurveyQuestion<SurveyDerivationType> typedData = data as IAnswerSurveyQuestion<SurveyDerivationType>;
				if (typedData != null)
				{
					return typedData.AskQuestion(contextElement);
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				return (int)SurveyQuestionGlyph.Last + 1 + 1 + (int)SurveyRoleType.Supertype + 1 + 2 + answer;
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
		private sealed class ProvideSurveyQuestionForSurveyQueryParameterType : ISurveyQuestionTypeInfo, ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store>
		{
			private ProvideSurveyQuestionForSurveyQueryParameterType()
			{
			}
			public static readonly ISurveyQuestionTypeInfo<Microsoft.VisualStudio.Modeling.Store> Instance = new ProvideSurveyQuestionForSurveyQueryParameterType();
			public Type QuestionType
			{
				get
				{
					return typeof(SurveyQueryParameterType);
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
				IAnswerSurveyQuestion<SurveyQueryParameterType> typedData = data as IAnswerSurveyQuestion<SurveyQueryParameterType>;
				if (typedData != null)
				{
					return typedData.AskQuestion(contextElement);
				}
				return -1;
			}
			public int MapAnswerToImageIndex(int answer)
			{
				return (int)SurveyQuestionGlyph.Last + 1 + 1 + (int)SurveyRoleType.Supertype + 1 + 2 + (int)SurveyDerivationType.Query + 1 + answer;
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
	}
}
