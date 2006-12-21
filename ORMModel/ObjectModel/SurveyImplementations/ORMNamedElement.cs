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
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;

namespace Neumont.Tools.ORM.ObjectModel
{
	public partial class ORMNamedElement : ISurveyNode
	{
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
				return !DomainTypeDescriptor.CreatePropertyDescriptor(this, ORMNamedElement.NameDomainPropertyId).IsReadOnly;
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
		protected string SurveyName //TODO: this may need to be updated to return the more descriptive element name (componentName)?
		{
			get
			{
				return this.Name;
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
				DomainTypeDescriptor.CreatePropertyDescriptor(this, ORMNamedElement.NameDomainPropertyId).SetValue(this, value);
			}
		}

		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeDataObject"/>
		/// </summary>
		protected static object SurveyNodeDataObject
		{
			get
			{
				return null;
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
	}
}
