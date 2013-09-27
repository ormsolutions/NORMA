#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
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
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid;
using System.Windows.Forms;
using System.Diagnostics;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
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
			ORMModel model = ResolvedModel;
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
		/// The key used to an expansion details for a <see cref="SetConstraint"/>
		/// </summary>
		public static readonly object SurveyExpansionKey = new object();
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
				return SurveyExpansionKey;
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
					case ConstraintType.Ring:
						switch (((RingConstraint)this).RingType)
						{
							// Use as default
							//case RingConstraintType.Undefined:
							//    return (int)SurveyQuestionGlyph.RingUndefined;
							case RingConstraintType.Reflexive:
								return (int)SurveyQuestionGlyph.RingReflexive;
							case RingConstraintType.Irreflexive:
								return (int)SurveyQuestionGlyph.RingIrreflexive;
							case RingConstraintType.Symmetric:
								return (int)SurveyQuestionGlyph.RingSymmetric;
							case RingConstraintType.Antisymmetric:
								return (int)SurveyQuestionGlyph.RingAntisymmetric;
							case RingConstraintType.Asymmetric:
								return (int)SurveyQuestionGlyph.RingAsymmetric;
							case RingConstraintType.Transitive:
								return (int)SurveyQuestionGlyph.RingTransitive;
							case RingConstraintType.Intransitive:
								return (int)SurveyQuestionGlyph.RingIntransitive;
							case RingConstraintType.StronglyIntransitive:
								return (int)SurveyQuestionGlyph.RingStronglyIntransitive;
							case RingConstraintType.Acyclic:
								return (int)SurveyQuestionGlyph.RingAcyclic;
							case RingConstraintType.AcyclicTransitive:
								return (int)SurveyQuestionGlyph.RingAcyclicTransitive;
							case RingConstraintType.AcyclicIntransitive:
								return (int)SurveyQuestionGlyph.RingAcyclicIntransitive;
							case RingConstraintType.AcyclicStronglyIntransitive:
								return (int)SurveyQuestionGlyph.RingAcyclicStronglyIntransitive;
							case RingConstraintType.ReflexiveSymmetric:
								return (int)SurveyQuestionGlyph.RingReflexiveSymmetric;
							case RingConstraintType.ReflexiveAntisymmetric:
								return (int)SurveyQuestionGlyph.RingReflexiveAntisymmetric;
							case RingConstraintType.ReflexiveTransitive:
								return (int)SurveyQuestionGlyph.RingReflexiveTransitive;
							case RingConstraintType.ReflexiveTransitiveAntisymmetric:
								return (int)SurveyQuestionGlyph.RingReflexiveTransitiveAntisymmetric;
							case RingConstraintType.SymmetricTransitive:
								return (int)SurveyQuestionGlyph.RingSymmetricTransitive;
							case RingConstraintType.SymmetricIrreflexive:
								return (int)SurveyQuestionGlyph.RingSymmetricIrreflexive;
							case RingConstraintType.SymmetricIntransitive:
								return (int)SurveyQuestionGlyph.RingSymmetricIntransitive;
							case RingConstraintType.SymmetricStronglyIntransitive:
								return (int)SurveyQuestionGlyph.RingSymmetricStronglyIntransitive;
							case RingConstraintType.AsymmetricIntransitive:
								return (int)SurveyQuestionGlyph.RingAsymmetricIntransitive;
							case RingConstraintType.AsymmetricStronglyIntransitive:
								return (int)SurveyQuestionGlyph.RingAsymmetricStronglyIntransitive;
							case RingConstraintType.TransitiveIrreflexive:
								return (int)SurveyQuestionGlyph.RingTransitiveIrreflexive;
							case RingConstraintType.TransitiveAntisymmetric:
								return (int)SurveyQuestionGlyph.RingTransitiveAntisymmetric;
							case RingConstraintType.TransitiveAsymmetric:
								return (int)SurveyQuestionGlyph.RingTransitiveAsymmetric;
							case RingConstraintType.PurelyReflexive:
								return (int)SurveyQuestionGlyph.RingPurelyReflexive;
							default:
								return (int)SurveyQuestionGlyph.RingUndefined;
						}
					case ConstraintType.ValueComparison:
						switch (((ValueComparisonConstraint)this).Operator)
						{
							// Use as default
							//case ValueComparisonOperator.Undefined:
							//    return (int)SurveyQuestionGlyph.ValueComparisonUndefined;
							case ValueComparisonOperator.Equal:
								return (int)SurveyQuestionGlyph.ValueComparisonEqual;
							case ValueComparisonOperator.NotEqual:
								return (int)SurveyQuestionGlyph.ValueComparisonNotEqual;
							case ValueComparisonOperator.LessThan:
								return (int)SurveyQuestionGlyph.ValueComparisonLessThan;
							case ValueComparisonOperator.GreaterThan:
								return (int)SurveyQuestionGlyph.ValueComparisonGreaterThan;
							case ValueComparisonOperator.LessThanOrEqual:
								return (int)SurveyQuestionGlyph.ValueComparisonLessThanOrEqual;
							case ValueComparisonOperator.GreaterThanOrEqual:
								return (int)SurveyQuestionGlyph.ValueComparisonGreaterThanOrEqual;
							default:
								return (int)SurveyQuestionGlyph.ValueComparisonUndefined;
						}
					default:
						Debug.Fail("Constraint has to be one of the above!");
						return -1;
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
					case ConstraintType.Ring:
						switch (((RingConstraint)this).RingType)
						{
							// Use as default
							//case RingConstraintType.Undefined:
							//    return (int)SurveyQuestionGlyph.RingUndefinedDeontic;
							case RingConstraintType.Reflexive:
								return (int)SurveyQuestionGlyph.RingReflexiveDeontic;
							case RingConstraintType.Irreflexive:
								return (int)SurveyQuestionGlyph.RingIrreflexiveDeontic;
							case RingConstraintType.Symmetric:
								return (int)SurveyQuestionGlyph.RingSymmetricDeontic;
							case RingConstraintType.Antisymmetric:
								return (int)SurveyQuestionGlyph.RingAntisymmetricDeontic;
							case RingConstraintType.Asymmetric:
								return (int)SurveyQuestionGlyph.RingAsymmetricDeontic;
							case RingConstraintType.Transitive:
								return (int)SurveyQuestionGlyph.RingTransitiveDeontic;
							case RingConstraintType.Intransitive:
								return (int)SurveyQuestionGlyph.RingIntransitiveDeontic;
							case RingConstraintType.StronglyIntransitive:
								return (int)SurveyQuestionGlyph.RingStronglyIntransitiveDeontic;
							case RingConstraintType.Acyclic:
								return (int)SurveyQuestionGlyph.RingAcyclicDeontic;
							case RingConstraintType.AcyclicTransitive:
								return (int)SurveyQuestionGlyph.RingAcyclicTransitiveDeontic;
							case RingConstraintType.AcyclicIntransitive:
								return (int)SurveyQuestionGlyph.RingAcyclicIntransitiveDeontic;
							case RingConstraintType.AcyclicStronglyIntransitive:
								return (int)SurveyQuestionGlyph.RingAcyclicStronglyIntransitiveDeontic;
							case RingConstraintType.ReflexiveSymmetric:
								return (int)SurveyQuestionGlyph.RingReflexiveSymmetricDeontic;
							case RingConstraintType.ReflexiveAntisymmetric:
								return (int)SurveyQuestionGlyph.RingReflexiveAntisymmetricDeontic;
							case RingConstraintType.ReflexiveTransitive:
								return (int)SurveyQuestionGlyph.RingReflexiveTransitiveDeontic;
							case RingConstraintType.ReflexiveTransitiveAntisymmetric:
								return (int)SurveyQuestionGlyph.RingReflexiveTransitiveAntisymmetricDeontic;
							case RingConstraintType.SymmetricTransitive:
								return (int)SurveyQuestionGlyph.RingSymmetricTransitiveDeontic;
							case RingConstraintType.SymmetricIrreflexive:
								return (int)SurveyQuestionGlyph.RingSymmetricIrreflexiveDeontic;
							case RingConstraintType.SymmetricIntransitive:
								return (int)SurveyQuestionGlyph.RingSymmetricIntransitiveDeontic;
							case RingConstraintType.SymmetricStronglyIntransitive:
								return (int)SurveyQuestionGlyph.RingSymmetricStronglyIntransitiveDeontic;
							case RingConstraintType.AsymmetricIntransitive:
								return (int)SurveyQuestionGlyph.RingAsymmetricIntransitiveDeontic;
							case RingConstraintType.AsymmetricStronglyIntransitive:
								return (int)SurveyQuestionGlyph.RingAsymmetricStronglyIntransitiveDeontic;
							case RingConstraintType.TransitiveIrreflexive:
								return (int)SurveyQuestionGlyph.RingTransitiveIrreflexiveDeontic;
							case RingConstraintType.TransitiveAntisymmetric:
								return (int)SurveyQuestionGlyph.RingTransitiveAntisymmetricDeontic;
							case RingConstraintType.TransitiveAsymmetric:
								return (int)SurveyQuestionGlyph.RingTransitiveAsymmetricDeontic;
							case RingConstraintType.PurelyReflexive:
								return (int)SurveyQuestionGlyph.RingPurelyReflexiveDeontic;
							default:
								return (int)SurveyQuestionGlyph.RingUndefinedDeontic;
						}
					case ConstraintType.ValueComparison:
						switch (((ValueComparisonConstraint)this).Operator)
						{
							// Use as default
							//case ValueComparisonOperator.Undefined:
							//    return (int)SurveyQuestionGlyph.ValueComparisonUndefinedDeontic;
							case ValueComparisonOperator.Equal:
								return (int)SurveyQuestionGlyph.ValueComparisonEqualDeontic;
							case ValueComparisonOperator.NotEqual:
								return (int)SurveyQuestionGlyph.ValueComparisonNotEqualDeontic;
							case ValueComparisonOperator.LessThan:
								return (int)SurveyQuestionGlyph.ValueComparisonLessThanDeontic;
							case ValueComparisonOperator.GreaterThan:
								return (int)SurveyQuestionGlyph.ValueComparisonGreaterThanDeontic;
							case ValueComparisonOperator.LessThanOrEqual:
								return (int)SurveyQuestionGlyph.ValueComparisonLessThanOrEqualDeontic;
							case ValueComparisonOperator.GreaterThanOrEqual:
								return (int)SurveyQuestionGlyph.ValueComparisonGreaterThanOrEqualDeontic;
							default:
								return (int)SurveyQuestionGlyph.ValueComparisonUndefinedDeontic;
						}
					default:
						Debug.Fail("Constraint has to be one of the above!");
						return -1;
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
			ORMModel model = ResolvedModel;
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
		/// The key used to an expansion details for a <see cref="SetConstraint"/>
		/// </summary>
		public static readonly object SurveyExpansionKey = new object();
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
				return SurveyExpansionKey;
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
	public partial class MandatoryConstraint : ISurveyNode
	{
		#region ISurveyNode Implementation
		// For an exclusive or constraint, we show the mandatory constraint but
		// use the name for the exclusion constraint

		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyName"/>
		/// Redirect ExclusiveOr constraint to use name of the exclusion constraint.
		/// </summary>
		protected new string SurveyName
		{
			get
			{
				ISurveyNode exclusionNode = ExclusiveOrExclusionConstraint;
				return exclusionNode != null ? exclusionNode.SurveyName : base.Name;
			}
		}
		string ISurveyNode.SurveyName
		{
			get
			{
				return SurveyName;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.EditableSurveyName"/>
		/// Redirect ExclusiveOr constraint to use name of the exclusion constraint.
		/// </summary>
		protected new string EditableSurveyName
		{
			get
			{
				ISurveyNode exclusionNode = ExclusiveOrExclusionConstraint;
				return exclusionNode != null ? exclusionNode.EditableSurveyName : base.EditableSurveyName;
			}
			set
			{
				ISurveyNode exclusionNode = ExclusiveOrExclusionConstraint;
				if (exclusionNode != null)
				{
					exclusionNode.EditableSurveyName = value;


				}
				else
				{
					base.EditableSurveyName = value;
				}
			}
		}
		string ISurveyNode.EditableSurveyName
		{
			get
			{
				return EditableSurveyName;
			}
			set
			{
				EditableSurveyName = value;
			}
		}
		#endregion // ISurveyNode Implementation
	}
	partial class FactConstraint : ISurveyNodeReference, IAnswerSurveyQuestion<SurveyConstraintDetailType>
	{
		#region ISurveyNodeReference Implementation
		/// <summary>
		/// Implements <see cref="IElementReference.ReferencedElement"/>
		/// </summary>
		protected object ReferencedElement
		{
			get
			{
				return FactType;
			}
		}
		object IElementReference.ReferencedElement
		{
			get
			{
				return ReferencedElement;
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
		#region IAnswerSurveyQuestion<SurveyConstraintDetailType> Implementation
		int IAnswerSurveyQuestion<SurveyConstraintDetailType>.AskQuestion(object contextElement)
		{
			return AskErrorQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyConstraintDetailType}.AskQuestion"/>
		/// </summary>
		protected int AskErrorQuestion(object contextElement)
		{
			return (int)SurveyConstraintDetailType.ConsistuentFactType;
		}
		#endregion // IAnswerSurveyQuestion<SurveyConstraintDetailType> Implementation
	}
}
