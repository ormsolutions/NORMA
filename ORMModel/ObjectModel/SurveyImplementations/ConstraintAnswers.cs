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

namespace Neumont.Tools.ORM.ObjectModel
{
	public partial class SetConstraint : IAnswerSurveyQuestion<ElementType>, IAnswerSurveyQuestion<ErrorState>, ISurveyNode
	{
		#region IAnswerSurveyQuestion<ErrorState> Members

		int IAnswerSurveyQuestion<ErrorState>.AskQuestion()
		{
			return AskErrorQuestion();
		}
		/// <summary>
		/// returns answer to IAnswerSurveyQuetion for errors
		/// </summary>
		/// <returns></returns>
		protected int AskErrorQuestion()
		{
			return (int)(ModelError.HasErrors(this, ModelErrorUses.None) ? ErrorState.HasError : ErrorState.NoError);
		}

		#endregion
		#region IAnswerSurveyQuestion<ElementType> Members
		int IAnswerSurveyQuestion<ElementType>.AskQuestion()
		{
			return AskElementQuestion();
		}
		/// <summary>
		/// implementation of AskQuestion method from IAnswerSurveyQuestion
		/// </summary>
		/// <returns></returns>
		protected int AskElementQuestion()
		{
			return (int)ElementType.Constraint;
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
	}
	public partial class SetComparisonConstraint : IAnswerSurveyQuestion<ElementType>, IAnswerSurveyQuestion<ErrorState>, ISurveyNode
	{
		#region IAnswerSurveyQuestion<ErrorState> Members

		int IAnswerSurveyQuestion<ErrorState>.AskQuestion()
		{
			return AskErrorQuestion();
		}
		/// <summary>
		/// returns answer to IAnswerSurveyQuetion for errors
		/// </summary>
		/// <returns></returns>
		protected int AskErrorQuestion()
		{
			return (int)(ModelError.HasErrors(this, ModelErrorUses.None) ? ErrorState.HasError : ErrorState.NoError);
		}

		#endregion
		#region IAnswerSurveyQuestion<ElementType> Members
		int IAnswerSurveyQuestion<ElementType>.AskQuestion()
		{
			return AskElementQuestion();
		}
		/// <summary>
		/// implementation of AskQuestion method from IAnswerSurveyQuestion
		/// </summary>
		/// <returns></returns>
		protected int AskElementQuestion()
		{
			return (int)ElementType.Constraint;
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
