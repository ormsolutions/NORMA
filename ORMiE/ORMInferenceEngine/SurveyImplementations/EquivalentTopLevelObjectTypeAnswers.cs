using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.VirtualTreeGrid;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid;
using ORMSolutions.ORMArchitect.Framework;

namespace unibz.ORMInferenceEngine
{
	partial class EquivalentTopLevelObjectType : ISurveyNodeReference, IAnswerSurveyQuestion<SurveyRootElementType>, ISurveyNode
	{
		#region IAnswerSurveyQuestion<SurveyRootElementType> Implementation
		int IAnswerSurveyQuestion<SurveyRootElementType>.AskQuestion(object contextElement)
		{
			return (int)SurveyRootElementType.EquivalentTopLevelObjectType;
		}
		#endregion // IAnswerSurveyQuestion<SurveyRootElementType> Implementation
		#region ISurveyNodeReference Implementation
		SurveyNodeReferenceOptions ISurveyNodeReference.SurveyNodeReferenceOptions
		{
			get { return SurveyNodeReferenceOptions.BlockLinkDisplay | SurveyNodeReferenceOptions.FilterReferencedAnswers | SurveyNodeReferenceOptions.BlockTargetNavigation | SurveyNodeReferenceOptions.TrackReferenceInstance /*| SurveyNodeReferenceOptions.SelectSelf*/; }
		}
		object ISurveyNodeReference.SurveyNodeReferenceReason
		{
			get { return this; }
		}
		bool ISurveyNodeReference.UseSurveyNodeReferenceAnswer(Type questionType, ISurveyDynamicValues dynamicValues, int answer)
		{
			if (questionType == typeof(SurveyErrorState))
			{
				return false;
			}
			return true;
		}
		object IElementReference.ReferencedElement
		{
			get { return ObjectType; }
		}
		#endregion // ISurveyNodeReference Implementation
		#region ISurveyNode Implementation
		string ISurveyNode.EditableSurveyName
		{
			get
			{
				return null;
			}
			set
			{
				// Not editable, not called
			}
		}

		bool ISurveyNode.IsSurveyNameEditable
		{
			get { return false; }
		}

		string ISurveyNode.SurveyName
		{
			get { return null; }
		}

		object ISurveyNode.SurveyNodeDataObject
		{
			get { return null; }
		}
		/// <summary>
		/// The key used to retrieve expansions from the model browser.
		/// </summary>
		public static readonly object SurveyExpansionKey = new object();
		object ISurveyNode.SurveyNodeExpansionKey
		{
			get { return SurveyExpansionKey; }
		}
		#endregion // ISurveyNode Implementation
	}
}
