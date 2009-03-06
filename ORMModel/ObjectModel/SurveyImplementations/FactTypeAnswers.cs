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

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	public partial class FactType : IAnswerSurveyQuestion<SurveyElementType>, IAnswerSurveyQuestion<SurveyErrorState>, IAnswerSurveyQuestion<SurveyQuestionGlyph>, IAnswerSurveyQuestion<SurveyFactTypeDetailType>, ISurveyNode, ISurveyNodeContext
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
				string retVal;
				if (Objectification != null)
				{
					retVal = this.GenerateName();
				}
				else
				{
					retVal = this.Name;
					if (string.IsNullOrEmpty(retVal))
					{
						// UNDONE: MattCurland: We're getting this during redo scenarios on objectified facts.
						// This is a bug in the Name propery implementation, which should handle
						// this transparently. The sequencing here is very tricky because it involves
						// synchronizing the FactType and ObjectType names. I don't want to destabilize
						// that scenario for this case, so I'm just regenerating the name.
						retVal = GenerateName();
					}
				}
				return retVal;
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
				FactType resolvedFact = this;
				Objectification objectification;
				if (null != (objectification = ImpliedByObjectification))
				{
					resolvedFact = objectification.NestedFactType;
				}
				DataObject retVal = new DataObject();
				retVal.SetData(typeof(FactType), resolvedFact);
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
				Objectification objectification = ImpliedByObjectification;
				return (objectification != null) ? objectification.NestedFactType : null;
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
						// This case should not get hit with unary binarization, but it isn't hurting anything
						return (int)SurveyQuestionGlyph.UnaryFactType;
					case 2:
						return GetUnaryRoleIndex(roles).HasValue ? (int)SurveyQuestionGlyph.UnaryFactType : (int)SurveyQuestionGlyph.BinaryFactType;
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
			// If this question is being asked, then we must be an implicit fact type
			Debug.Assert(this.ImpliedByObjectification != null);
			return (int)SurveyFactTypeDetailType.ImpliedFactType;
		}
		#endregion // IAnswerSurveyQuestion<SurveyFactTypeDetailType> Implementation
	}
}
