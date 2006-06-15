using System;
using System.Collections.Generic;
using System.Text;
using Neumont.Tools.ORM.Framework.DynamicSurveyTreeGrid;
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling;

namespace Neumont.Tools.ORM.ObjectModel
{
	public partial class FactType : IAnswerSurveyQuestion<ElementType>, IAnswerSurveyQuestion<ErrorState>, ISurveyName
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
		#region ISurveyName Members
		/// <summary>
		/// returns whether or not this object is editable in a survey tree
		/// </summary>
		bool ISurveyName.IsEditable
		{
			get
			{
				return IsEditable;
			}
		}
		/// <summary>
		/// implementation of IsEditable from ISurveyName
		/// </summary>
		protected bool IsEditable
		{
			get
			{
				MetaAttributeInfo info = this.Store.MetaDataDirectory.FindMetaAttribute(FactType.NameMetaAttributeGuid);
				return this.IsPropertyDescriptorReadOnly(this.CreatePropertyDescriptor(info, this));
			}
		}
		/// <summary>
		/// the display name for a survey tree
		/// </summary>
		string ISurveyName.SurveyName 
		{
			get
			{
				return SurveyName;
			}
		}
		/// <summary>
		/// implementation of SurveyName from ISurveyName
		/// </summary>
		protected string SurveyName //TODO: this might be updated to show the more informative element names (componentName)?
		{
			get 
			{
				return this.Name;
			}
		}
		/// <summary>
		/// the editable name for a survey tree
		/// </summary>
		string ISurveyName.EditableSurveyName
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
		/// implementation of EditableSurveyName from ISurveyName
		/// </summary>
		protected string EditableSurveyName
		{
			get
			{
				return this.Name;
			}
			set
			{
				this.Name = value;
			}
		}

		#endregion
	}
}
