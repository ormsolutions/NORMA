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
using System.Globalization;
using ORMSolutions.ORMArchitect.Framework;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	partial class RoleBase : IAnswerSurveyQuestion<SurveyFactTypeDetailType>, IAnswerSurveyQuestion<SurveyErrorState>, ISurveyNodeContext, ICustomComparableSurveyNode
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
			FactType factType;
			ORMModel model;
			return (null == (factType = FactType) || null == (model = factType.Model)) ?
				-1 :
				(int)(ModelError.HasErrors(this, ModelErrorUses.DisplayPrimary, model.ModelErrorDisplayFilter) ? SurveyErrorState.HasError : SurveyErrorState.NoError);
		}
		#endregion // IAnswerSurveyQuestion<SurveyErrorState> Implementation
		#region IAnswerSurveyQuestion<SurveyFactTypeDetailType> Implementation
		int IAnswerSurveyQuestion<SurveyFactTypeDetailType>.AskQuestion(object contextElement)
		{
			return AskFactTypeDetailQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyFactTypeDetailType}.AskQuestion"/>
		/// </summary>
		protected static int AskFactTypeDetailQuestion(object contextElement)
		{
			return (int)SurveyFactTypeDetailType.Role;
		}
		#endregion // IAnswerSurveyQuestion<SurveyFactTypeDetailType> Implementation
		#region ICustomComparableSurveyNode Implementation
		int ICustomComparableSurveyNode.CompareToSurveyNode(object contextElement, object other, object customSortData, object otherCustomSortData)
		{
			return CompareToSurveyNode(contextElement, other, customSortData, otherCustomSortData);
		}
		/// <summary>
		/// Implements <see cref="ICustomComparableSurveyNode.CompareToSurveyNode"/>. Roles
		/// compare based on order in the FactType.RoleCollection. 0 (no information) is
		/// returned for a comparison to all other element types.
		/// </summary>
		protected int CompareToSurveyNode(object contextElement, object other, object customSortData, object otherCustomSortData)
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
		bool ICustomComparableSurveyNode.ResetCustomSortData(object contextElement, ref object customSortData)
		{
			return ResetCustomSortData(contextElement, ref customSortData);
		}
		/// <summary>
		/// Implements <see cref="ICustomComparableSurveyNode.CompareToSurveyNode"/>. Returns
		/// the current position in the RoleCollection of the parent <see cref="FactType"/>
		/// </summary>
		protected bool ResetCustomSortData(object contextElement, ref object customSortData)
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
		#endregion // ICustomComparableSurveyNode Implementation
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
	partial class Role : IAnswerSurveyQuestion<SurveyQuestionGlyph>, ISurveyNode
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
				return !DomainTypeDescriptor.CreatePropertyDescriptor(this, Role.NameDomainPropertyId).IsReadOnly;
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
				ObjectType rolePlayer = RolePlayer;
				string rolePlayerName = (rolePlayer != null) ? rolePlayer.Name : ResourceStrings.RoleSurveyNameMissingRolePlayer;
				string roleName = Name;
				if (!string.IsNullOrEmpty(roleName))
				{
					return string.Format(CultureInfo.CurrentCulture, ResourceStrings.RoleSurveyNameFormat, rolePlayerName, roleName);
				}
				return rolePlayerName;
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
				return Name;
			}
			set
			{
				DomainTypeDescriptor.CreatePropertyDescriptor(this, Role.NameDomainPropertyId).SetValue(this, value);
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeDataObject"/>
		/// </summary>
		protected object SurveyNodeDataObject
		{
			get
			{
				FactType resolvedFactType = FactType;
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
			ObjectType rolePlayer = RolePlayer;
			if (rolePlayer != null)
			{
				return ((IAnswerSurveyQuestion<SurveyQuestionGlyph>)rolePlayer).AskQuestion(null);
			}
			return (int)SurveyQuestionGlyph.ObjectTypeNotSet;
		}
		#endregion // IAnswerSurveyQuestion<SurveyQuestionGlyph> Implementation
	}
	partial class RoleProxy : ISurveyNodeReference
	{
		#region ISurveyNodeReference Implementation
		/// <summary>
		/// Implements <see cref="IElementReference.ReferencedElement"/>
		/// </summary>
		protected object ReferencedElement
		{
			get
			{
				return Role;
			}
		}
		object IElementReference.ReferencedElement
		{
			get
			{
				return ReferencedElement;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNodeReference.SurveyNodeReferenceReason"/>
		/// </summary>
		protected object SurveyNodeReferenceReason
		{
			get
			{
				return this;
			}
		}
		object ISurveyNodeReference.SurveyNodeReferenceReason
		{
			get
			{
				return SurveyNodeReferenceReason;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNodeReference.SurveyNodeReferenceOptions"/>
		/// </summary>
		protected static SurveyNodeReferenceOptions SurveyNodeReferenceOptions
		{
			get
			{
				return SurveyNodeReferenceOptions.None;
			}
		}
		SurveyNodeReferenceOptions ISurveyNodeReference.SurveyNodeReferenceOptions
		{
			get
			{
				return SurveyNodeReferenceOptions;
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNodeReference.UseSurveyNodeReferenceAnswer"/>
		/// </summary>
		protected static bool UseSurveyNodeReferenceAnswer(Type questionType, ISurveyDynamicValues dynamicValues, int answer)
		{
			return true;
		}
		bool ISurveyNodeReference.UseSurveyNodeReferenceAnswer(Type questionType, ISurveyDynamicValues dynamicValues, int answer)
		{
			return UseSurveyNodeReferenceAnswer(questionType, dynamicValues, answer);
		}
		#endregion // ISurveyNodeReference Implementation
	}
	partial class SubtypeMetaRole : ICustomComparableSurveyNode, IAnswerSurveyQuestion<SurveyRoleType>
	{
		#region ICustomComparableSurveyNode Implementation
		bool ICustomComparableSurveyNode.ResetCustomSortData(object contextElement, ref object customSortData)
		{
			return ResetCustomSortData(contextElement, ref customSortData);
		}
		/// <summary>
		/// Implements <see cref="ICustomComparableSurveyNode.CompareToSurveyNode"/>.
		/// Returns 1 to place the subtype role after the supertype role.
		/// </summary>
		protected new bool ResetCustomSortData(object contextElement, ref object customSortData)
		{
			if (null == customSortData || (int)customSortData != 1)
			{
				customSortData = 1;
				return true;
			}
			return false;
		}
		#endregion // ICustomComparableSurveyNode Implementation
		#region IAnswerSurveyQuestion<SurveyRoleType> Implementation
		int IAnswerSurveyQuestion<SurveyRoleType>.AskQuestion(object contextElement)
		{
			return AskRoleTypeQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyRoleType}.AskQuestion"/>
		/// </summary>
		protected static int AskRoleTypeQuestion(object contextElement)
		{
			return (int)SurveyRoleType.Subtype;
		}
		#endregion // IAnswerSurveyQuestion<SurveyRoleType> Implementation
	}
	partial class SupertypeMetaRole : ICustomComparableSurveyNode, IAnswerSurveyQuestion<SurveyRoleType>
	{
		#region ICustomComparableSurveyNode Implementation
		bool ICustomComparableSurveyNode.ResetCustomSortData(object contextElement, ref object customSortData)
		{
			return ResetCustomSortData(contextElement, ref customSortData);
		}
		/// <summary>
		/// Implements <see cref="ICustomComparableSurveyNode.CompareToSurveyNode"/>.
		/// Returns 0 to place the subtype role after the supertype role.
		/// </summary>
		protected new bool ResetCustomSortData(object contextElement, ref object customSortData)
		{
			if (null == customSortData || (int)customSortData != 0)
			{
				customSortData = 0;
				return true;
			}
			return false;
		}
		#endregion // ICustomComparableSurveyNode Implementation
		#region IAnswerSurveyQuestion<SurveyRoleType> Implementation
		int IAnswerSurveyQuestion<SurveyRoleType>.AskQuestion(object contextElement)
		{
			return AskRoleTypeQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyRoleType}.AskQuestion"/>
		/// </summary>
		protected static int AskRoleTypeQuestion(object contextElement)
		{
			return (int)SurveyRoleType.Supertype;
		}
		#endregion // IAnswerSurveyQuestion<SurveyRoleType> Implementation
	}
}
