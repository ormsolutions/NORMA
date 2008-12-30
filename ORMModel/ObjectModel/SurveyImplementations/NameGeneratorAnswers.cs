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
	public partial class NameGenerator : IAnswerSurveyQuestion<SurveyElementType>, IAnswerSurveyQuestion<SurveyNameGeneratorRefinementType>, ISurveyNode, ISurveyNodeContext
	{
		/// <summary>
		/// The key used to retrieve expansion details for the model browser
		/// </summary>
		public static readonly object SurveyExpansionKey = new object();
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
		protected static bool IsSurveyNameEditable
		{
			get
			{
				return false;
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
				return DomainTypeDescriptor.GetDisplayName(NameUsageType ?? this.GetType());
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
		protected static string EditableSurveyName
		{
			get
			{
				return null;
			}
			set
			{
			}
		}
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyNodeDataObject"/>
		/// </summary>
		protected object SurveyNodeDataObject
		{
			get
			{
				return this;
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
		protected object SurveyNodeExpansionKey
		{
			get
			{
				return (NameUsageType == null && (NameUsageTypes.Length != 0 || GetDomainClass().LocalDescendants.Count != 0)) ? SurveyExpansionKey : null;
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
		#region IAnswerSurveyQuestion<SurveyElementType> Implementation
		int IAnswerSurveyQuestion<SurveyElementType>.AskQuestion(object contextElement)
		{
			return AskElementTypeDetailQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyElementType}.AskQuestion"/>
		/// </summary>
		protected static int AskElementTypeDetailQuestion(object contextElement)
		{
			return (int)SurveyElementType.NameGenerator;
		}
		#endregion // IAnswerSurveyQuestion<SurveyElementType> Implementation
		#region IAnswerSurveyQuestion<SurveyNameGeneratorRefinementType> Implementation
		int IAnswerSurveyQuestion<SurveyNameGeneratorRefinementType>.AskQuestion(object contextElement)
		{
			return AskRefinementTypeDetailQuestion(contextElement);
		}
		/// <summary>
		/// Implements <see cref="IAnswerSurveyQuestion{SurveyNameGeneratorRefinementType}.AskQuestion"/>
		/// </summary>
		protected int AskRefinementTypeDetailQuestion(object contextElement)
		{
			return (NameUsageType != null) ? (int)SurveyNameGeneratorRefinementType.UsageRefinement : (int)SurveyNameGeneratorRefinementType.TypeRefinement;
		}
		#endregion // IAnswerSurveyQuestion<SurveyNameGeneratorRefinementType> Implementation
		#region ISurveyNodeContext Implementation
		/// <summary>
		/// The survey node context for a <see cref="Role"/> is
		/// its parent <see cref="FactType"/>
		/// </summary>
		protected object SurveyNodeContext
		{
			get
			{
				return RefinesGenerator;
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
