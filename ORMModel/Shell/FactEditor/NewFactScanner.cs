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

#region Using Directives
using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Modeling.Shell;
using Neumont.Tools.ORM.ObjectModel;
#endregion

namespace Neumont.Tools.ORM.Shell.FactEditor
{
	/// <summary>
	/// Parses the string of text set as it's source and provides tokens.
	/// </summary>
	public sealed class NewFactScanner : IScanner
	{
		#region Private Members
		private String m_Braces = "()[]{}";
		private Boolean m_InBrace = false;
		private String m_Line;
		private Int32 m_Offset;
		private LanguageService m_LanguageService;
		private IVsTextLines m_TextLines;
		private ORMDesignerDocView m_View;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="T:NewFactScanner"/> class.
		/// </summary>
		[CLSCompliant(false)]
		public NewFactScanner(LanguageService service, IVsTextLines textLines)
		{
			m_LanguageService = service;
			m_TextLines = textLines;

			IMonitorSelectionService monitor = m_LanguageService.GetService(typeof(IMonitorSelectionService)) as IMonitorSelectionService;
			monitor.DocumentWindowChanged += new EventHandler<MonitorSelectionEventArgs>(DocumentWindowChanged);
			m_View = monitor.CurrentDocumentView as ORMDesignerDocView;
		}
		#endregion

		#region Document Changed Events
		private void DocumentWindowChanged(Object sender, MonitorSelectionEventArgs e)
		{
			this.CurrentDocumentView = ((IMonitorSelectionService)sender).CurrentDocumentView as ORMDesignerDocView;
		}
		#endregion

		#region IScanner Members
		Boolean IScanner.ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref Int32 state)
		{
			return ScanTokenAndProvideInfoAboutIt(tokenInfo, ref state);
		}

		void IScanner.SetSource(String source, Int32 offset)
		{
			SetSource(source, offset);
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Used to (re)initialize the scanner before scanning a small portion of text, such as single source line for syntax coloring purposes.
		/// </summary>
		/// <param name="source">The source text portion to be scanned.</param>
		/// <param name="offset">The index of the first character to be scanned.</param>
		private void SetSource(String source, Int32 offset)
		{
			m_Line = source;
			m_Offset = offset;
		}

		/// <summary>
		/// Scan the next token and fills in syntax coloring details about it in tokenInfo.
		/// </summary>
		/// <param name="tokenInfo">Keeps information about the token.</param>
		/// <param name="state">Keeps track of scanner state. In: state after last token. Out: state after current token.</param>
		/// <returns></returns>
		private Boolean ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref Int32 state)
		{
			Boolean foundToken = GetNextToken(m_Offset, tokenInfo, ref state);
			if (foundToken)
			{
				m_Offset = tokenInfo.EndIndex + 1;

				if (tokenInfo is FactTokenInfo)
				{
					FactTokenInfo factToken = tokenInfo as FactTokenInfo;
					factToken.Value = m_Line.Substring(factToken.StartIndex, factToken.Length);
					factToken.ExistsOnModel = this.ObjectTypeExists(factToken.Value);
				}
			}
			return foundToken;
		}

		private Boolean GetNextToken(Int32 startIndex, TokenInfo tokenInfo, ref Int32 state)
		{
			Boolean bFoundToken = false;      // Assume we are done with this line.
			Int32 index = startIndex;

			if (index < m_Line.Length)
			{
				bFoundToken = true;            // We are not done with this line.
				tokenInfo.StartIndex = index;
				Char c = m_Line[index];

				if (Char.IsPunctuation(c))
				{
					tokenInfo.Type = TokenType.Operator;
					tokenInfo.Color = TokenColor.Keyword;
					tokenInfo.EndIndex = index;

					if (m_Braces.IndexOf(c) != -1)
					{
						if (c == '(')
						{
							m_InBrace = true;
							tokenInfo.Trigger = TokenTriggers.MatchBraces | TokenTriggers.ParameterStart;
						}
						else if (c == ')')
						{
							m_InBrace = false;
							tokenInfo.Trigger = TokenTriggers.MatchBraces | TokenTriggers.ParameterEnd;
						}
						else
						{
							tokenInfo.Trigger = TokenTriggers.MatchBraces;
						}
					}
				}
				else if (Char.IsWhiteSpace(c))
				{
					do
					{
						++index;
					}
					while (index < m_Line.Length &&
						  Char.IsWhiteSpace(m_Line[index]));
					tokenInfo.Type = TokenType.WhiteSpace;
					tokenInfo.Color = TokenColor.Text;
					tokenInfo.EndIndex = index - 1;
				}
				else if (Char.IsLower(c))
				{
					do
					{
						++index;
					}
					while (index < m_Line.Length &&
						  !Char.IsPunctuation(m_Line[index]) &&
						  !Char.IsWhiteSpace(m_Line[index]));
					tokenInfo.Type = TokenType.Identifier;
					if (m_InBrace)
					{
						tokenInfo.Color = TokenColor.Identifier;
					}
					else
					{
						tokenInfo.Color = TokenColor.Comment;
					}
					tokenInfo.EndIndex = index - 1;
				}
				else if (Char.IsUpper(c))
				{
					do
					{
						++index;
					}
					while (index < m_Line.Length &&
						  !Char.IsPunctuation(m_Line[index]) &&
						  !Char.IsWhiteSpace(m_Line[index]));
					tokenInfo.Type = TokenType.Identifier;
					tokenInfo.Color = TokenColor.String;
					tokenInfo.EndIndex = index - 1;
				}
				else
				{
					do
					{
						++index;
					}
					while (index < m_Line.Length &&
							  !Char.IsPunctuation(m_Line[index]) &&
							  !Char.IsWhiteSpace(m_Line[index]));
					tokenInfo.Type = TokenType.Unknown;
					tokenInfo.Color = TokenColor.Identifier;
					tokenInfo.EndIndex = index - 1;
					tokenInfo.Trigger = TokenTriggers.None;
				}
			}
			return bFoundToken;
		}
		#endregion

		#region Model Helper Methods
		private Boolean ObjectTypeExists(String name)
		{
			ORMDesignerDocView currentDocView = m_View;
			if (currentDocView != null)
			{
				List<ObjectType> temp = new List<ObjectType>();
				temp.AddRange(currentDocView.DocData.Store.ElementDirectory.FindElements<ObjectType>());
				//temp.Sort(Modeling.NamedElementComparer<ObjectType>.CurrentCulture);

				foreach (ObjectType o in temp)
				{
					if (o.Name == name)
						return true;
				}
			}

			return false;
		}
		#endregion

		#region Private Properties
		/// <summary>
		/// Gets or sets the current document view.
		/// </summary>
		/// <value>The current document view.</value>
		private ORMDesignerDocView CurrentDocumentView
		{
			get
			{
				return m_View;
			}
			set
			{
				ORMDesignerDocView oldView = m_View;
				if (oldView != null)
				{
					if (value != null)
					{
						if (oldView == value)
						{
							return;
						}
						else if (oldView.DocData == value.DocData)
						{
							m_View = value;
							return;
						}
					}
					ModelingDocData docData = oldView.DocData;
					if (docData != null)
					{
						//ManageEventHandlers(docData.Store, EventHandlerAction.Remove);
					}
				}
				m_View = value;
				//ReloadModelElements();
				if (value != null)
				{
					//ManageEventHandlers(value.DocData.Store, EventHandlerAction.Add);
				}
			}
		}
		#endregion
	}
}
