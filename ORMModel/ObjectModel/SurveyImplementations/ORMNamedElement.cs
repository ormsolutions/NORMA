using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.Framework.DynamicSurveyTreeGrid;

namespace Neumont.Tools.ORM.ObjectModel
{
	public partial class ORMNamedElement : ISurveyName
	{
		#region ISurveyName Members
		/// <summary>
		/// whether or not this object is editable in the survey tree
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
				MetaAttributeInfo info = this.Store.MetaDataDirectory.FindMetaAttribute(NamedElement.NameMetaAttributeGuid);
				return this.IsPropertyDescriptorReadOnly(this.CreatePropertyDescriptor(info, this));
			}
		}

		/// <summary>
		/// the display name to be used in the survey tree
		/// </summary>
		string ISurveyName.SurveyName
		{
			get
			{
				return this.Name;
			}
		}
		/// <summary>
		/// implementation of SurveyName from ISurveyName
		/// </summary>
		protected string SurveyName //TODO: this may need to be updated to return the more descriptive element name (componentName)?
		{
			get
			{
				return this.Name;
			}
		}
		/// <summary>
		/// editable name to be displayed in the survey tree
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
		/// implementatin of EditableSurveyName from ISurveyName
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
