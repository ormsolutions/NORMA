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
	public partial class SetConstraint : IAnswerSurveyQuestion<SurveyElementType>, IAnswerSurveyQuestion<SurveyErrorState>, IAnswerSurveyQuestion<SurveyQuestionGlyph>, IAnswerSurveyQuestion<SurveyFactTypeDetailType>, ISurveyNode
	{
		#region IAnswerSurveyQuestion<ErrorState> Members

		int IAnswerSurveyQuestion<SurveyErrorState>.AskQuestion()
		{
			return AskErrorQuestion();
		}
		/// <summary>
		/// returns answer to IAnswerSurveyQuestion for errors
		/// </summary>
		/// <returns></returns>
		protected int AskErrorQuestion()
		{
			return (int)(ModelError.HasErrors(this, ModelErrorUses.None) ? SurveyErrorState.HasError : SurveyErrorState.NoError);
		}

		#endregion
		#region IAnswerSurveyQuestion<ElementType> Members
		int IAnswerSurveyQuestion<SurveyElementType>.AskQuestion()
		{
			return AskElementQuestion();
		}
		/// <summary>
		/// implementation of AskQuestion method from IAnswerSurveyQuestion
		/// </summary>
		/// <returns></returns>
		protected int AskElementQuestion()
		{
			return (int)SurveyElementType.ExternalConstraint;
		}

		#endregion
		#region IAnswerSurveyQuestion<SurveyFactTypeDetailType> Members
		int IAnswerSurveyQuestion<SurveyFactTypeDetailType>.AskQuestion()
		{
			return AskFactTypeDetailQuestion();
		}
		/// <summary>
		/// returns answer to IAnswerSurveyQuestion for fact type details
		/// </summary>
		protected int AskFactTypeDetailQuestion()
		{
			return (int)SurveyFactTypeDetailType.InternalConstraint;
		}
		#endregion
		#region ISurveyNode Members
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
		#endregion
		#region IAnswerSurveyQuestion<SurveyQuestionGlyph> Members

		int IAnswerSurveyQuestion<SurveyQuestionGlyph>.AskQuestion()
		{
			return AskGlyphQuestion();
		}

		#region AskGlyphQuestion
		/// <summary>
		/// Answers the glyph question.
		/// </summary>
		/// <returns></returns>
		protected int AskGlyphQuestion()
		{
			if (this.Constraint.Modality == ConstraintModality.Alethic)
			{
				if (this.Constraint.ConstraintType == ConstraintType.Ring)
				{
					switch ((this as RingConstraint).RingType)
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

					switch (this.Constraint.ConstraintType)
					{
						case ConstraintType.ExternalUniqueness:
							if ((this as UniquenessConstraint).IsPreferred)
							{
								return (int)SurveyQuestionGlyph.ExternalUniquenessConstraintIsPreferred;
							}
							else
							{
								return (int)SurveyQuestionGlyph.ExternalUniquenessConstraint;
							}
						case ConstraintType.SimpleMandatory:
							return (int)SurveyQuestionGlyph.SimpleMandatoryConstraint;
						case ConstraintType.InternalUniqueness:
							return (int)SurveyQuestionGlyph.InternalUniquenessConstraint;
						case ConstraintType.Frequency:
							return (int)SurveyQuestionGlyph.FrequencyConstraint;
						case ConstraintType.DisjunctiveMandatory:
							if (null != (this as MandatoryConstraint).ExclusiveOrExclusionConstraint)
							{
								return (int)SurveyQuestionGlyph.ExclusiveOrConstraint;
							}
							else
							{
								return (int)SurveyQuestionGlyph.DisjunctiveMandatoryConstraint;
							}
						default:
							Debug.Fail("Constraint has to be one of the above!");
							return -1;
					}
				}
			}
			else
			{
				if (this.Constraint.ConstraintType == ConstraintType.Ring)
				{

					switch ((this as RingConstraint).RingType)
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

					switch (this.Constraint.ConstraintType)
					{
						case ConstraintType.ExternalUniqueness:
							if ((this as UniquenessConstraint).IsPreferred)
							{
								return (int)SurveyQuestionGlyph.ExternalUniquenessConstraintIsPreferredDeontic;
							}
							else
							{
								return (int)SurveyQuestionGlyph.ExternalUniquenessConstraintDeontic;
							}
						case ConstraintType.SimpleMandatory:
							return (int)SurveyQuestionGlyph.SimpleMandatoryConstraintDeontic;
						case ConstraintType.InternalUniqueness:
							return (int)SurveyQuestionGlyph.InternalUniquenessConstraintDeontic;
						case ConstraintType.Frequency:
							return (int)SurveyQuestionGlyph.FrequencyConstraintDeontic;
						case ConstraintType.DisjunctiveMandatory:
							if (null != (this as MandatoryConstraint).ExclusiveOrExclusionConstraint)
							{
								return (int)SurveyQuestionGlyph.ExclusiveOrConstraintDeontic;
							}
							else
							{
								return (int)SurveyQuestionGlyph.DisjunctiveMandatoryConstraintDeontic;
							}
						default:
							Debug.Fail("Constraint has to be one of the above!");
							return -1;
					}
				}
			}

		} 
		#endregion

		#endregion
	}
	public partial class SetComparisonConstraint : IAnswerSurveyQuestion<SurveyElementType>, IAnswerSurveyQuestion<SurveyErrorState>, IAnswerSurveyQuestion<SurveyQuestionGlyph>, ISurveyNode
	{
		#region IAnswerSurveyQuestion<ErrorState> Members

		int IAnswerSurveyQuestion<SurveyErrorState>.AskQuestion()
		{
			return AskErrorQuestion();
		}
		/// <summary>
		/// returns answer to IAnswerSurveyQuestion for errors
		/// </summary>
		/// <returns></returns>
		protected int AskErrorQuestion()
		{
			return (int)(ModelError.HasErrors(this, ModelErrorUses.None) ? SurveyErrorState.HasError : SurveyErrorState.NoError);
		}

		#endregion
		#region IAnswerSurveyQuestion<ElementType> Members
		int IAnswerSurveyQuestion<SurveyElementType>.AskQuestion()
		{
			return AskElementQuestion();
		}
		/// <summary>
		/// implementation of AskQuestion method from IAnswerSurveyQuestion
		/// </summary>
		/// <returns></returns>
		protected int AskElementQuestion()
		{

			return (int)SurveyElementType.ExternalConstraint;
		}

		#endregion
		#region IAnswerSurveyQuestion<SurveyQuestionGlyph> Members

		int IAnswerSurveyQuestion<SurveyQuestionGlyph>.AskQuestion()
		{
			return AskGlyphQuestion();
		}
		#region AskGlyphQuestion
		/// <summary>
		/// implementation of AskQuestion method from IAnswerSurveyQuestion for Glyph types
		/// </summary>
		/// <returns></returns>
		protected int AskGlyphQuestion()
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
		#endregion
		#endregion
		#region ISurveyNode Members
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
		#endregion
	}
	public partial class ExclusionConstraint : ISurveyNode
	{
		#region ISurveyNode Members
		/// <summary>
		/// Implements <see cref="ISurveyNode"/>.<see cref="SurveyNodeDataObject"/>
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
}
