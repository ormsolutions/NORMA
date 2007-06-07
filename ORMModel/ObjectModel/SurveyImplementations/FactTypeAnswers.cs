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
	public partial class FactType : IAnswerSurveyQuestion<SurveyElementType>, IAnswerSurveyQuestion<SurveyErrorState>, IAnswerSurveyQuestion<SurveyQuestionGlyph>, IAnswerSurveyQuestion<SurveyFactTypeDetailType>, ISurveyNode
	{
		#region IAnswerSurveyQuestion<ErrorState> Implementation
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
			if (Model == null)
				return -1;
            return (int)(ModelError.HasErrors(this, ModelErrorUses.DisplayPrimary, Model.ModelErrorDisplayFilter) ? SurveyErrorState.HasError : SurveyErrorState.NoError);
        }
        #endregion // IAnswerSurveyQuestion<ErrorState> Implementation
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
			return (int)SurveyElementType.FactType;
		}

		#endregion
		#region ISurveyNode Members
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
				string retVal = this.Name;
				if (string.IsNullOrEmpty(retVal))
				{
					// UNDONE: MattCurland: We're getting this during redo scenarios on objectified facts.
					// This is a bug in the Name propery implementation, which should handle
					// this transparently. The sequencing here is very tricky because it involves
					// synchronizing the FactType and ObjectType namds. I don't want to destabilize
					// that scenario for this case, so I'm just regenerating the name.
					retVal = GenerateName();
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
		/// The key used to retrieve expansion details for the model browser
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
		#endregion
		#region IAnswerSurveyQuestion<SurveyQuestionGlyph> Members

		int IAnswerSurveyQuestion<SurveyQuestionGlyph>.AskQuestion()
		{
			return AskGlyphQuestion();
		}
		/// <summary>
		/// returns answer to IAnswerSurveyQuestion for glyphs
		/// </summary>
		/// <returns></returns>
		protected int AskGlyphQuestion()
		{
			if (this.Objectification != null && !this.Objectification.IsImplied)
			{
				return (int)SurveyQuestionGlyph.ObjectifiedFactType;
			}
			else
			{
				switch (RoleCollection.Count)
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
			// If this question is being asked, then we must be an implicit fact type
			Debug.Assert(this.ImpliedByObjectification != null);
			return (int)SurveyFactTypeDetailType.ImpliedFactType;
		}
		#endregion
	}
}
