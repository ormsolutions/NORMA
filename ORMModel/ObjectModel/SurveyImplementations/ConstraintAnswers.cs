#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
*                                                                          *
* The use and distribution terms for this software are covered by the      *
* Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
* can be found in the file CPL.txt at the root of this distribution.       *
* By using this software in any fashion, you are agreeing to be bound by   *
* the terms of this license.                                               *
*                                                                          *
* You must not remove this notice, or any other, from this software.       *
\**************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;
using System.Windows.Forms;
using System.Diagnostics;

namespace Neumont.Tools.ORM.ObjectModel
{
	public partial class SetConstraint : IAnswerSurveyQuestion<SurveyElementType>, IAnswerSurveyQuestion<SurveyErrorState>, IAnswerSurveyQuestion<SurveyQuestionGlyph>, IAnswerSurveyQuestion<SurveyFactTypeDetailType>, ISurveyNode, ISurveyNodeContext
	{
		#region IAnswerSurveyQuestion<SurveyErrorState> Implementation
		int IAnswerSurveyQuestion<SurveyErrorState>.AskQuestion(object contextElement)
		{
			return AskErrorQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyErrorState}.AskQuestion"/>
		/// </summary>
		protected int AskErrorQuestion(object contextElement)
		{
			ORMModel model = Model;
			return (model == null) ?
				-1 :
				(int)(ModelError.HasErrors(this, ModelErrorUses.DisplayPrimary, model.ModelErrorDisplayFilter) ? SurveyErrorState.HasError : SurveyErrorState.NoError);
		}
		#endregion // IAnswerSurveyQuestion<SurveyErrorState> Implementation
		#region IAnswerSurveyQuestion<SurveyElementType> Implementation
		int IAnswerSurveyQuestion<SurveyElementType>.AskQuestion(object contextElement)
		{
			return AskElementQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyElementType}.AskQuestion"/>
		/// </summary>
		protected static int AskElementQuestion(object contextElement)
		{
			return (int)SurveyElementType.ExternalConstraint;
		}
		#endregion // IAnswerSurveyQuestion<SurveyElementType> Implementation
		#region IAnswerSurveyQuestion<SurveyFactTypeDetailType> Implementation
		int IAnswerSurveyQuestion<SurveyFactTypeDetailType>.AskQuestion(object contextElement)
		{
			return AskFactTypeDetailQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyFactTypeDetailType}.AskQuestion"/>
		/// </summary>
		protected static int AskFactTypeDetailQuestion(object contextElement)
		{
			return (int)SurveyFactTypeDetailType.InternalConstraint;
		}
		#endregion // IAnswerSurveyQuestion<SurveyFactTypeDetailType> Implementation
		#region ISurveyNode Implementation
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeDataObject"/>
		/// </summary>
		protected new object SurveyNodeDataObject
		{
			get
			{
				if (Constraint.ConstraintIsInternal)
				{
					LinkedElementCollection<FactType> facts = FactTypeCollection;
					return (facts.Count == 1) ? ((ISurveyNode)facts[0]).SurveyNodeDataObject : null;
				}
				else
				{
					DataObject retVal = new DataObject();
					retVal.SetData(typeof(SetConstraint), this);
					return retVal;
				}
			}
		}
		object ISurveyNode.SurveyNodeDataObject
		{
			get
			{
				return SurveyNodeDataObject;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeExpansionKey"/>
		/// </summary>
		protected new object SurveyNodeExpansionKey
		{
			get
			{
				IConstraint constraint = Constraint;
				if (!constraint.ConstraintIsInternal && constraint.ConstraintType != ConstraintType.ImpliedMandatory)
				{
					return FactConstraint.SurveyConstraintExpansionKey;
				}
				return null;
			}
		}
			object ISurveyNode.SurveyNodeExpansionKey
			{
				get
				{
					return SurveyNodeExpansionKey;
				}
			}
		#endregion // ISurveyNode Implementation
		#region ISurveyNodeContext Implementation
		/// <summary>
		/// The survey node context for an internal <see cref="SetConstraint"/> is
		/// the associated <see cref="FactType"/>
		/// </summary>
		protected object SurveyNodeContext
		{
			get
			{
				LinkedElementCollection<FactType> factTypes;
				if (Constraint.ConstraintIsInternal &&
					(factTypes = FactTypeCollection).Count == 1)
				{
					return factTypes[0];
				}
				return null;
			}
		}
		object ISurveyNodeContext.SurveyNodeContext
		{
			get
			{
				return SurveyNodeContext;
			}
		}
		#endregion // ISurveyNodeContext Implementation
		#region IAnswerSurveyQuestion<SurveyQuestionGlyph> Implementation
		int IAnswerSurveyQuestion<SurveyQuestionGlyph>.AskQuestion(object contextElement)
		{
			return AskGlyphQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyQuestionGlyph}.AskQuestion"/>
		/// </summary>
		protected int AskGlyphQuestion(object contextElement)
		{
			IConstraint constraint = this.Constraint;
			if (constraint.Modality == ConstraintModality.Alethic)
			{
				if (constraint.ConstraintType == ConstraintType.Ring)
				{
					switch (((RingConstraint)this).RingType)
					{
						case RingConstraintType.Undefined:
							return (int)SurveyQuestionGlyph.RingUndefined;
						case RingConstraintType.PurelyReflexive:
							return (int)SurveyQuestionGlyph.RingPurelyReflexive;
						case RingConstraintType.Irreflexive:
							return (int)SurveyQuestionGlyph.RingIrreflexive;
						case RingConstraintType.Symmetric:
							return (int)SurveyQuestionGlyph.RingSymmetric;
						case RingConstraintType.Asymmetric:
							return (int)SurveyQuestionGlyph.RingAsymmetric;
						case RingConstraintType.Antisymmetric:
							return (int)SurveyQuestionGlyph.RingAntisymmetric;
						case RingConstraintType.Intransitive:
							return (int)SurveyQuestionGlyph.RingIntransitive;
						case RingConstraintType.Acyclic:
							return (int)SurveyQuestionGlyph.RingAcyclic;
						case RingConstraintType.AcyclicIntransitive:
							return (int)SurveyQuestionGlyph.RingAcyclicIntransitive;
						case RingConstraintType.AsymmetricIntransitive:
							return (int)SurveyQuestionGlyph.RingAsymmetricIntransitive;
						case RingConstraintType.SymmetricIntransitive:
							return (int)SurveyQuestionGlyph.RingSymmetricIntransitive;
						case RingConstraintType.SymmetricIrreflexive:
							return (int)SurveyQuestionGlyph.RingSymmetricIrreflexive;
						default:
							return (int)SurveyQuestionGlyph.RingUndefined;
					}
				}
				else
				{
					switch (constraint.ConstraintType)
					{
						case ConstraintType.ExternalUniqueness:
							return ((UniquenessConstraint)this).IsPreferred ?
								(int)SurveyQuestionGlyph.ExternalUniquenessConstraintIsPreferred :
								(int)SurveyQuestionGlyph.ExternalUniquenessConstraint;
						case ConstraintType.SimpleMandatory:
							return (int)SurveyQuestionGlyph.SimpleMandatoryConstraint;
						case ConstraintType.InternalUniqueness:
							return (int)SurveyQuestionGlyph.InternalUniquenessConstraint;
						case ConstraintType.Frequency:
							return (int)SurveyQuestionGlyph.FrequencyConstraint;
						case ConstraintType.DisjunctiveMandatory:
							return (null != ((MandatoryConstraint)this).ExclusiveOrExclusionConstraint) ?
								(int)SurveyQuestionGlyph.ExclusiveOrConstraint :
								(int)SurveyQuestionGlyph.DisjunctiveMandatoryConstraint;
						default:
							Debug.Fail("Constraint has to be one of the above!");
							return -1;
					}
				}
			}
			else
			{
				if (constraint.ConstraintType == ConstraintType.Ring)
				{
					switch (((RingConstraint)this).RingType)
					{
						case RingConstraintType.Undefined:
							return (int)SurveyQuestionGlyph.RingUndefinedDeontic;
						case RingConstraintType.PurelyReflexive:
							return (int)SurveyQuestionGlyph.RingPurelyReflexiveDeontic;
						case RingConstraintType.Irreflexive:
							return (int)SurveyQuestionGlyph.RingIrreflexiveDeontic;
						case RingConstraintType.Symmetric:
							return (int)SurveyQuestionGlyph.RingSymmetricDeontic;
						case RingConstraintType.Asymmetric:
							return (int)SurveyQuestionGlyph.RingAsymmetricDeontic;
						case RingConstraintType.Antisymmetric:
							return (int)SurveyQuestionGlyph.RingAntisymmetricDeontic;
						case RingConstraintType.Intransitive:
							return (int)SurveyQuestionGlyph.RingIntransitiveDeontic;
						case RingConstraintType.Acyclic:
							return (int)SurveyQuestionGlyph.RingAcyclicDeontic;
						case RingConstraintType.AcyclicIntransitive:
							return (int)SurveyQuestionGlyph.RingAcyclicIntransitiveDeontic;
						case RingConstraintType.AsymmetricIntransitive:
							return (int)SurveyQuestionGlyph.RingAsymmetricIntransitiveDeontic;
						case RingConstraintType.SymmetricIntransitive:
							return (int)SurveyQuestionGlyph.RingSymmetricIntransitiveDeontic;
						case RingConstraintType.SymmetricIrreflexive:
							return (int)SurveyQuestionGlyph.RingSymmetricIrreflexiveDeontic;
						default:
							return (int)SurveyQuestionGlyph.RingUndefinedDeontic;
					}
				}
				else
				{
					switch (constraint.ConstraintType)
					{
						case ConstraintType.ExternalUniqueness:
							return ((UniquenessConstraint)this).IsPreferred ?
								(int)SurveyQuestionGlyph.ExternalUniquenessConstraintIsPreferredDeontic :
								(int)SurveyQuestionGlyph.ExternalUniquenessConstraintDeontic;
						case ConstraintType.SimpleMandatory:
							return (int)SurveyQuestionGlyph.SimpleMandatoryConstraintDeontic;
						case ConstraintType.InternalUniqueness:
							return (int)SurveyQuestionGlyph.InternalUniquenessConstraintDeontic;
						case ConstraintType.Frequency:
							return (int)SurveyQuestionGlyph.FrequencyConstraintDeontic;
						case ConstraintType.DisjunctiveMandatory:
							return (null != ((MandatoryConstraint)this).ExclusiveOrExclusionConstraint) ?
								(int)SurveyQuestionGlyph.ExclusiveOrConstraintDeontic :
								(int)SurveyQuestionGlyph.DisjunctiveMandatoryConstraintDeontic;
						default:
							Debug.Fail("Constraint has to be one of the above!");
							return -1;
					}
				}
			}

		}
		#endregion // IAnswerSurveyQuestion<SurveyQuestionGlyph> Implementation
	}
	public partial class SetComparisonConstraint : IAnswerSurveyQuestion<SurveyElementType>, IAnswerSurveyQuestion<SurveyErrorState>, IAnswerSurveyQuestion<SurveyQuestionGlyph>, ISurveyNode
	{
		#region IAnswerSurveyQuestion<SurveyErrorState> Implementation
		int IAnswerSurveyQuestion<SurveyErrorState>.AskQuestion(object contextElement)
		{
			return AskErrorQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyErrorState}.AskQuestion"/>
		/// </summary>
		protected int AskErrorQuestion(object contextElement)
		{
			ORMModel model = Model;
			return (model == null) ?
				-1 :
				(int)(ModelError.HasErrors(this, ModelErrorUses.DisplayPrimary, model.ModelErrorDisplayFilter) ? SurveyErrorState.HasError : SurveyErrorState.NoError);
		}
		#endregion // IAnswerSurveyQuestion<SurveyErrorState> Implementation
		#region IAnswerSurveyQuestion<SurveyElementType> Implementation
		int IAnswerSurveyQuestion<SurveyElementType>.AskQuestion(object contextElement)
		{
			return AskElementQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyElementType}.AskQuestion"/>
		/// </summary>
		protected static int AskElementQuestion(object contextElement)
		{
			return (int)SurveyElementType.ExternalConstraint;
		}
		#endregion // IAnswerSurveyQuestion<SurveyElementType> Implementation
		#region IAnswerSurveyQuestion<SurveyQuestionGlyph> Implementation
		int IAnswerSurveyQuestion<SurveyQuestionGlyph>.AskQuestion(object contextElement)
		{
			return AskGlyphQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyQuestionGlyph}.AskQuestion"/>
		/// </summary>
		protected int AskGlyphQuestion(object contextElement)
		{
			IConstraint constraint = this as IConstraint;
			if (constraint.Modality == ConstraintModality.Alethic)
			{
				switch (constraint.ConstraintType)
				{
					case ConstraintType.Equality:
						return (int)SurveyQuestionGlyph.EqualityConstraint;
					case ConstraintType.Exclusion:
						if (null != (constraint as ExclusionConstraint).ExclusiveOrMandatoryConstraint)
						{
							return (int)SurveyQuestionGlyph.ExclusiveOrConstraint;
						}
						else
						{
							return (int)SurveyQuestionGlyph.ExclusionConstraint;
						}
					case ConstraintType.Subset:
						return (int)SurveyQuestionGlyph.SubsetConstraint;
					default:
						Debug.Fail("Constraint has to be one of the above!");
						return -1;
				}
			}
			else
			{
				switch (constraint.ConstraintType)
				{
					case ConstraintType.Equality:
						return (int)SurveyQuestionGlyph.EqualityConstraintDeontic;
					case ConstraintType.Exclusion:
						if (null != (constraint as ExclusionConstraint).ExclusiveOrMandatoryConstraint)
						{
							return (int)SurveyQuestionGlyph.ExclusiveOrConstraintDeontic;
						}
						else
						{
							return (int)SurveyQuestionGlyph.ExclusionConstraintDeontic;
						}
					case ConstraintType.Subset:
						return (int)SurveyQuestionGlyph.SubsetConstraintDeontic;
					default:
						Debug.Fail("Constraint has to be one of the above!");
						return -1;

				}
			}
		}
		#endregion // IAnswerSurveyQuestion<SurveyQuestionGlyph> Implementation
		#region ISurveyNode Implementation
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeDataObject"/>
		/// </summary>
		protected new object SurveyNodeDataObject
		{
			get
			{
				DataObject retVal = new DataObject();
				retVal.SetData(typeof(SetComparisonConstraint), this);
				return retVal;
			}
		}
		object ISurveyNode.SurveyNodeDataObject
		{
			get
			{
				return SurveyNodeDataObject;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeExpansionKey"/>
		/// </summary>
		protected new static object SurveyNodeExpansionKey
		{
			get
			{
				return FactConstraint.SurveyConstraintExpansionKey;
			}
		}
		object ISurveyNode.SurveyNodeExpansionKey
		{
			get
			{
				return SurveyNodeExpansionKey;
			}
		}
		#endregion // ISurveyNode Implementation
	}
	public partial class ExclusionConstraint : ISurveyNode
	{
		#region ISurveyNode Members
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeDataObject"/>
		/// </summary>
		protected new object SurveyNodeDataObject
		{
			get
			{
				MandatoryConstraint mandatoryConstraint = ExclusiveOrMandatoryConstraint;
				if (mandatoryConstraint != null)
				{
					DataObject retVal = new DataObject();
					retVal.SetData(typeof(SetConstraint), mandatoryConstraint);
					return retVal;
				}
				return base.SurveyNodeDataObject;
			}
		}
		object ISurveyNode.SurveyNodeDataObject
		{
			get
			{
				return SurveyNodeDataObject;
			}
		}
		#endregion
	}
	partial class FactConstraint : ISurveyNodeReference
	{
		/// <summary>
		/// The key used to retrieve external constraint expansion details
		/// </summary>
		public static readonly object SurveyConstraintExpansionKey = new object();
		#region ISurveyNodeReference Implementation
		/// <summary>
		/// Implements <see cref="ISurveyNodeReference.ReferencedSurveyNode"/>
		/// </summary>
		protected object ReferencedSurveyNode
		{
			get
			{
				return FactType;
			}
		}
		object ISurveyNodeReference.ReferencedSurveyNode
		{
			get
			{
				return ReferencedSurveyNode;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNodeReference.SurveyNodeReferenceReason"/>
		/// </summary>
		protected object SurveyNodeReferenceReason
		{
			get
			{
				return this;
			}
		}
		object ISurveyNodeReference.SurveyNodeReferenceReason
		{
			get
			{
				return SurveyNodeReferenceReason;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNodeReference.SurveyNodeReferenceOptions"/>
		/// </summary>
		protected static SurveyNodeReferenceOptions SurveyNodeReferenceOptions
		{
			get
			{
				return SurveyNodeReferenceOptions.FilterReferencedAnswers;
			}
		}
		SurveyNodeReferenceOptions ISurveyNodeReference.SurveyNodeReferenceOptions
		{
			get
			{
				return SurveyNodeReferenceOptions;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNodeReference.UseSurveyNodeReferenceAnswer"/>
		/// </summary>
		protected static bool UseSurveyNodeReferenceAnswer(Type questionType, ISurveyDynamicValues dynamicValues, int answer)
		{
			return questionType != typeof(SurveyErrorState);
		}
		bool ISurveyNodeReference.UseSurveyNodeReferenceAnswer(Type questionType, ISurveyDynamicValues dynamicValues, int answer)
		{
			return UseSurveyNodeReferenceAnswer(questionType, dynamicValues, answer);
		}
		#endregion // ISurveyNodeReference Implementation
	}
}
