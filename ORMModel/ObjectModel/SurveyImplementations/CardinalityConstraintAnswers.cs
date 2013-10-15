#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © ORM Solutions, LLC. All rights reserved.                     *
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
using System.Diagnostics;
using System.Windows.Forms;
using ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region ObjectTypeCardinalityConstraint class
	partial class ObjectTypeCardinalityConstraint : IAnswerSurveyQuestion<SurveyQuestionGlyph>, ISurveyNode, ISurveyFloatingNode
	{
		#region ISurveyNode Implementation
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeDataObject"/>
		/// </summary>
		protected new object SurveyNodeDataObject
		{
			get
			{
				DataObject retVal = new DataObject();
				retVal.SetData(typeof(ObjectTypeCardinalityConstraint), this);
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
		#endregion // ISurveyNode Implementation
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
			return (int)((Modality == ConstraintModality.Deontic) ? SurveyQuestionGlyph.ObjectTypeCardinalityConstraintDeontic : SurveyQuestionGlyph.ObjectTypeCardinalityConstraint);
		}
		#endregion // IAnswerSurveyQuestion<SurveyQuestionGlyph> Implementation
		#region ISurveyFloatingNode Implementation
		/// <summary>
		/// Implements <see cref="ISurveyFloatingNode.FloatingSurveyNodeQuestionKey"/>
		/// </summary>
		protected object FloatingSurveyNodeQuestionKey
		{
			get
			{
				return ORMCoreDomainModel.SurveyFloatingExpansionKey;
			}
		}
		object ISurveyFloatingNode.FloatingSurveyNodeQuestionKey
		{
			get
			{
				return FloatingSurveyNodeQuestionKey;
			}
		}
		#endregion // ISurveyFloatingNode Implementation
	}
	#endregion // ObjectTypeCardinalityConstraint class
	#region UnaryRoleCardinalityConstraint class
	partial class UnaryRoleCardinalityConstraint : IAnswerSurveyQuestion<SurveyQuestionGlyph>, ISurveyNode, ISurveyFloatingNode
	{
		#region ISurveyNode Implementation
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeDataObject"/>
		/// </summary>
		protected new object SurveyNodeDataObject
		{
			get
			{
				DataObject retVal = new DataObject();
				retVal.SetData(typeof(UnaryRoleCardinalityConstraint), this);
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
		#endregion // ISurveyNode Implementation
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
			return (int)((Modality == ConstraintModality.Deontic)? SurveyQuestionGlyph.UnaryRoleCardinalityConstraintDeontic : SurveyQuestionGlyph.UnaryRoleCardinalityConstraint);
		}
		#endregion // IAnswerSurveyQuestion<SurveyQuestionGlyph> Implementation
		#region ISurveyFloatingNode Implementation
		/// <summary>
		/// Implements <see cref="ISurveyFloatingNode.FloatingSurveyNodeQuestionKey"/>
		/// </summary>
		protected object FloatingSurveyNodeQuestionKey
		{
			get
			{
				return ORMCoreDomainModel.SurveyFloatingExpansionKey;
			}
		}
		object ISurveyFloatingNode.FloatingSurveyNodeQuestionKey
		{
			get
			{
				return FloatingSurveyNodeQuestionKey;
			}
		}
		#endregion // ISurveyFloatingNode Implementation
	}
	#endregion // UnaryRoleCardinalityConstraint class
}
