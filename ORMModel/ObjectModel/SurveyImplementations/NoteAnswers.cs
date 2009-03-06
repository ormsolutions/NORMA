#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © ORM Solutions, LLC. All rights reserved.                        *
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
using System.Diagnostics;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using ORMSolutions.ORMArchitect.Framework.Shell.DynamicSurveyTreeGrid;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region ModelNote class
	partial class ModelNote : IAnswerSurveyQuestion<SurveyQuestionGlyph>, ISurveyNode, ISurveyFloatingNode
	{
		#region ISurveyNode Implementation
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
		bool ISurveyNode.IsSurveyNameEditable
		{
			get
			{
				return IsSurveyNameEditable;
			}
		}
		private static Regex myNormalizeSpaceRegex;
		/// <summary>
		/// A regular expression to normalize spaces
		/// </summary>
		private static Regex NormalizeSpaceRegex
		{
			get
			{
				Regex retVal = myNormalizeSpaceRegex;
				if (retVal == null)
				{
					System.Threading.Interlocked.CompareExchange<Regex>(
						ref myNormalizeSpaceRegex,
						new Regex(
							@"\s+",
							RegexOptions.Compiled),
						null);
					retVal = myNormalizeSpaceRegex;
				}
				return retVal;
			}
		}
		private const int SurveyNameMaxLength = 40;
		/// <summary>
		/// Implements <see cref="ISurveyNode.SurveyName"/>
		/// </summary>
		protected string SurveyName
		{
			get
			{
				string retVal = Text;
				retVal = NormalizeSpaceRegex.Replace(retVal, " ");
				if (retVal.Length > SurveyNameMaxLength)
				{
					int spaceIndex = retVal.LastIndexOf(' ', SurveyNameMaxLength);
					if (spaceIndex == -1)
					{
						spaceIndex = SurveyNameMaxLength;
					}
					retVal = retVal.Substring(0, spaceIndex) + "…";
				}
				return retVal;
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
		/// Implements <see cref="ISurveyNode.SurveyNodeDataObject"/>
		/// </summary>
		protected object SurveyNodeDataObject
		{
			get
			{
				DataObject retVal = new DataObject();
				retVal.SetData(typeof(ModelNote), this);
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
		#endregion // ISurveyNode Implementation
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
			return (int)SurveyQuestionGlyph.Note;
		}
		#endregion // IAnswerSurveyQuestion<SurveyQuestionGlyph> Implementation
		#region ISurveyFloatingNode Implementation
		/// <summary>
		/// Implements <see cref="ISurveyFloatingNode.FloatingSurveyNodeQuestionKey"/>
		/// </summary>
		protected object FloatingSurveyNodeQuestionKey
		{
			get
			{
				return ORMCoreDomainModel.SurveyFloatingExpansionKey;
			}
		}
		object ISurveyFloatingNode.FloatingSurveyNodeQuestionKey
		{
			get
			{
				return FloatingSurveyNodeQuestionKey;
			}
		}
		#endregion // ISurveyFloatingNode Implementation
	}
	#endregion // ModelNote class
}
