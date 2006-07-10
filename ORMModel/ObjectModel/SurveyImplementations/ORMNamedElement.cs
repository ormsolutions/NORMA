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
				// UNDONE: 2006-06 DSL Tools port: This seemed to be returning the same value as IsReadOnly, rather than its opposite,
				// which seemed to be rather backwards. For now, I've changed it to return !IsReadOnly...
				return !Design.ORMTypeDescriptor.CreateNamePropertyDescriptor(this).IsReadOnly;
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
