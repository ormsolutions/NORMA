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
	public partial class FactType : IAnswerSurveyQuestion<ElementType>, IAnswerSurveyQuestion<ErrorState>, ISurveyNode
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
			return (int)ElementType.FactType;
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
		#endregion
	}
}
