using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.VirtualTreeGrid;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
using ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid;
using ORMSolutions.ORMArchitect.Framework;

namespace unibz.ORMInferenceEngine
{
	partial class DisjointObjectTypeContainment : ISurveyNodeReference
	{
		#region ISurveyNodeReference Implementation
		SurveyNodeReferenceOptions ISurveyNodeReference.SurveyNodeReferenceOptions
		{
			get { return SurveyNodeReferenceOptions.BlockLinkDisplay | SurveyNodeReferenceOptions.InlineExpansion | SurveyNodeReferenceOptions.BlockTargetNavigation | SurveyNodeReferenceOptions.TrackReferenceInstance; }
		}
		object ISurveyNodeReference.SurveyNodeReferenceReason
		{
			get { return typeof(ObjectTypeContainment); }
		}
		bool ISurveyNodeReference.UseSurveyNodeReferenceAnswer(Type questionType, ISurveyDynamicValues dynamicValues, int answer)
		{
			return true;
		}
		object IElementReference.ReferencedElement
		{
			get
			{
                return DisjointTopLevelObjectType.GetLinkToInferredHierarchy(Child);
			}
		}
		#endregion // ISurveyNodeReference Implementation
	}
}
