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
	public partial class RoleBase : IAnswerSurveyQuestion<SurveyFactTypeDetailType>, ISurveyNode, ISurveyNodeContext, ICustomComparableSurveyNode
	{
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
		protected string SurveyName
		{
			get
			{
				Role role = Role;
				string retVal = role.Name;
				if (string.IsNullOrEmpty(retVal))
				{
					// UNDONE: Use a better name here
					retVal = TypeDescriptor.GetClassName(role);
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
				return Role.Name;
			}
			set
			{
				Role role = Role;
				DomainTypeDescriptor.CreatePropertyDescriptor(role, Role.NameDomainPropertyId).SetValue(role, value);
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeDataObject"/>
		/// </summary>
		protected object SurveyNodeDataObject
		{
			get
			{
				Role role = Role;
				FactType resolvedFactType = role.FactType;
				Objectification objectification;
				if (null != (objectification = resolvedFactType.ImpliedByObjectification))
				{
					resolvedFactType = objectification.NestedFactType;
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
		/// Implements <see cref="ISurveyNode.SurveyNodeExpansionKey"/>
		/// </summary>		
		protected static object SurveyNodeExpansionKey
		{
			get
			{
				return null;
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
			return (int)SurveyFactTypeDetailType.Role;
		}
		#endregion
		#region ICustomComparableSurveyNode Members
		int ICustomComparableSurveyNode.CompareToSurveyNode(object other, object customSortData, object otherCustomSortData)
		{
			return CompareToSurveyNode(other, customSortData, otherCustomSortData);
		}
		/// <summary>
		/// Implements <see cref="ICustomComparableSurveyNode.CompareToSurveyNode"/>. Roles
		/// compare based on order in the FactType.RoleCollection. 0 (no information) is
		/// returned for a comparison to all other element types.
		/// </summary>
		protected int CompareToSurveyNode(object other, object customSortData, object otherCustomSortData)
		{
			if (other is RoleBase)
			{
				int thisIndex = (int)customSortData;
				int otherIndex = (int)otherCustomSortData;
				if (thisIndex < otherIndex)
				{
					return -1;
				}
				else if (thisIndex != otherIndex)
				{
					return 1;
				}
			}
			// For this comparison, this implies no information is available
			return 0;
		}
		bool ICustomComparableSurveyNode.ResetCustomSortData(ref object customSortData)
		{
			return ResetCustomSortData(ref customSortData);
		}
		/// <summary>
		/// Implements <see cref="ICustomComparableSurveyNode.CompareToSurveyNode"/>. Returns
		/// the current position in the RoleCollection of the parent <see cref="FactType"/>
		/// </summary>
		protected bool ResetCustomSortData(ref object customSortData)
		{
			int retVal = -1;
			FactType factType;
			if (null != (factType = FactType))
			{
				retVal = factType.RoleCollection.IndexOf(this);
			}
			if (null == customSortData || (int)customSortData != retVal)
			{
				customSortData = retVal;
				return true;
			}
			return false;
		}
		#endregion
		#region ISurveyNodeContext Implementation
		/// <summary>
		/// The survey node context for a <see cref="Role"/> is
		/// its parent <see cref="FactType"/>
		/// </summary>
		protected object SurveyNodeContext
		{
			get
			{
				return FactType;
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
	}
}
