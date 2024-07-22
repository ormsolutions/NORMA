#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright � Neumont University. All rights reserved.                     *
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

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	public partial class FactType : IAnswerSurveyQuestion<SurveyElementType>, IAnswerSurveyQuestion<SurveyErrorState>, IAnswerSurveyQuestion<SurveyQuestionGlyph>, IAnswerSurveyQuestion<SurveyFactTypeDetailType>, IAnswerSurveyQuestion<SurveyDerivationType>, ISurveyNode, ISurveyNodeContext
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
		#region IAnswerSurveyQuestion<ElementType> Implementation
		int IAnswerSurveyQuestion<SurveyElementType>.AskQuestion(object contextElement)
		{
			return AskElementQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyElementType}.AskQuestion"/>
		/// </summary>
		protected int AskElementQuestion(object contextElement)
		{
			return (int)SurveyElementType.FactType;
		}
		#endregion // IAnswerSurveyQuestion<ElementType> Implementation
		#region ISurveyNode Implementation
		bool ISurveyNode.IsSurveyNameEditable
		{
			get
			{
				return IsSurveyNameEditable;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.IsSurveyNameEditable"/>
		/// </summary>
		protected bool IsSurveyNameEditable
		{
			get
			{
				return !DomainTypeDescriptor.CreatePropertyDescriptor(this, FactType.NameDomainPropertyId).IsReadOnly;
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
		/// Implements <see cref="ISurveyNode.SurveyName"/>
		/// </summary>
		protected string SurveyName //TODO: this might be updated to show the more informative element names (componentName)?
		{
			get
			{
				return Objectification != null ? GenerateName(false) : Name;
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
		/// <summary>
		/// Implements <see cref="ISurveyNode.EditableSurveyName"/>
		/// </summary>
		protected string EditableSurveyName
		{
			get
			{
				return this.Name;
			}
			set
			{
				DomainTypeDescriptor.CreatePropertyDescriptor(this, FactType.NameDomainPropertyId).SetValue(this, value);
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeDataObject"/>
		/// </summary>
		protected object SurveyNodeDataObject
		{
			get
			{
				FactType resolvedFactType = this;
				Objectification objectification;
				if (null != (objectification = ImpliedByObjectification) &&
					objectification.IsImplied)
				{
					// Don't drag out implied fact types unless there
					// is an objectifying shape to attach to.
					//resolvedFactType = objectification.NestedFactType;
				}
				DataObject retVal = new DataObject();
				retVal.SetData(typeof(FactType), resolvedFactType);
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
		/// The key used to retrieve <see cref="FactType"/> expansion details for the model browser
		/// </summary>
		public static readonly object SurveyExpansionKey = new object();
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeExpansionKey"/>
		/// </summary>		
		protected static object SurveyNodeExpansionKey
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
		/// The survey node context for an implied <see cref="FactType"/> is
		/// the FactType associated with the objectification.
		/// </summary>
		protected object SurveyNodeContext
		{
			get
			{
				FactType contextFactType = null;
				Objectification objectification = ImpliedByObjectification;
				if (objectification != null)
				{
					contextFactType = objectification.NestedFactType;
				}
				else if (UnaryPattern == UnaryValuePattern.Negation)
				{
					contextFactType = PositiveUnaryFactType;
				}
				return contextFactType;
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
			Objectification objectification = Objectification;
			if (objectification != null && !objectification.IsImplied)
			{
				return (int)SurveyQuestionGlyph.ObjectifiedFactType;
			}
			else
			{
				LinkedElementCollection<RoleBase> roles = RoleCollection;
				switch (roles.Count)
				{
					case 1:
						return (int)SurveyQuestionGlyph.UnaryFactType;
					case 2:
						return (int)SurveyQuestionGlyph.BinaryFactType;
					case 3:
						return (int)SurveyQuestionGlyph.TernaryFactType;
					default:
						return (int)SurveyQuestionGlyph.NaryFactType;

				}
			}
		}
		#endregion // IAnswerSurveyQuestion<SurveyQuestionGlyph> Implementation
		#region IAnswerSurveyQuestion<SurveyFactTypeDetailType> Implementation
		int IAnswerSurveyQuestion<SurveyFactTypeDetailType>.AskQuestion(object contextElement)
		{
			return AskFactTypeDetailQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyFactTypeDetailType}.AskQuestion"/>
		/// </summary>
		protected int AskFactTypeDetailQuestion(object contextElement)
		{
			int answer = -1;
			if (this.UnaryPattern == UnaryValuePattern.Negation)
			{
				answer = (int)SurveyFactTypeDetailType.UnaryNegationFactType;
			}
			else if (this.ImpliedByObjectification != null)
			{
				// This is the only other possibility if this is being asked.
				answer = (int)SurveyFactTypeDetailType.ImpliedFactType;
			}
			return answer;
		}
		#endregion // IAnswerSurveyQuestion<SurveyFactTypeDetailType> Implementation
		#region IAnswerSurveyQuestion<SurveyDerivationType> Implementation
		int IAnswerSurveyQuestion<SurveyDerivationType>.AskQuestion(object contextElement)
		{
			return AnswerDerivationTypeQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyDerivationType}.AskQuestion"/>
		/// </summary>
		protected int AnswerDerivationTypeQuestion(object contextElement)
		{
			return DerivationRule != null ? ((this is QueryBase) ? (int)SurveyDerivationType.Query : (int)SurveyDerivationType.Derived) : -1;
		}
		#endregion // IAnswerSurveyQuestion<SurveyDerivationType> Implementation
	}
	partial class QueryParameter : IAnswerSurveyQuestion<SurveyQuestionGlyph>, IAnswerSurveyQuestion<SurveyQueryParameterType>, ISurveyFloatingNode
	{
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
			ObjectType parameterType = ParameterType;
			if (parameterType != null)
			{
				return ((IAnswerSurveyQuestion<SurveyQuestionGlyph>)parameterType).AskQuestion(null);
			}
			return (int)SurveyQuestionGlyph.ObjectTypeNotSet;
		}
		#endregion // IAnswerSurveyQuestion<SurveyQuestionGlyph> Implementation
		#region IAnswerSurveyQuestion<SurveyQueryParameterType> Implementation
		int IAnswerSurveyQuestion<SurveyQueryParameterType>.AskQuestion(object contextElement)
		{
			return AskParameterTypeQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyQueryParameterType}.AskQuestion"/>
		/// </summary>
		protected int AskParameterTypeQuestion(object contextElement)
		{
			return (int)SurveyQueryParameterType.Input;
		}
		#endregion // IAnswerSurveyQuestion<SurveyQueryParameterType> Implementation
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
}
