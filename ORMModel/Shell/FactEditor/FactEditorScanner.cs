#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
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

#region Using Directives
using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Modeling.Shell;
using ORMSolutions.ORMArchitect.Core.ObjectModel;
#endregion

namespace ORMSolutions.ORMArchitect.Core.Shell
{
	partial class FactEditorLanguageService
	{
		private const TokenTriggers OpenParenthesisTokenTrigger = (TokenTriggers)0x20000000;
		private const TokenTriggers OpenSquareBracketTokenTrigger = (TokenTriggers)0x40000000;
		/// <summary>
		/// Parses the string of text set as it's source and provides tokens.
		/// </summary>
		private sealed class FactEditorLineScanner : IScanner
		{
			#region Private Members
			private const string m_Braces = "()[]{}";
			private int m_ParenCount;
			private int m_SquareBracketCount;
			private string m_Line;
			private int m_Offset;
			private LanguageService m_LanguageService;
			private IVsTextLines m_TextLines;
			private IORMDesignerView m_View;
			#endregion // Private Members
			#region Constructors
			/// <summary>
			/// Initializes a new instance of the <see cref="T:NewFactScanner"/> class.
			/// </summary>
			public FactEditorLineScanner(LanguageService service, IVsTextLines textLines)
			{
				m_LanguageService = service;
				m_TextLines = textLines;

				IMonitorSelectionService monitor = m_LanguageService.GetService(typeof(IMonitorSelectionService)) as IMonitorSelectionService;
				EventHandler<MonitorSelectionEventArgs> windowChange = new EventHandler<MonitorSelectionEventArgs>(DocumentWindowChanged);
				monitor.DocumentWindowChanged += windowChange;
				monitor.WindowChanged += windowChange;
				IORMDesignerView testView = monitor.CurrentSelectionContainer as IORMDesignerView;
				m_View = (testView != null && testView.CurrentDesigner != null) ? testView : monitor.CurrentDocumentView as IORMDesignerView;
			}
			#endregion // Constructors
			#region Document Changed Events
			private void DocumentWindowChanged(Object sender, MonitorSelectionEventArgs e)
			{
				IMonitorSelectionService monitor = (IMonitorSelectionService)sender;
				IORMDesignerView testView = monitor.CurrentSelectionContainer as IORMDesignerView;
				this.CurrentDesignerView = (testView != null && testView.CurrentDesigner != null) ? testView : monitor.CurrentDocumentView as IORMDesignerView;
			}
			#endregion // Document Changed Events
			#region IScanner Implementation
			/// <summary>
			/// Scan the next token and fills in syntax coloring details about it in tokenInfo.
			/// Implements <see cref="IScanner.ScanTokenAndProvideInfoAboutIt"/>
			/// </summary>
			/// <param name="tokenInfo">Keeps information about the token.</param>
			/// <param name="state">Keeps track of scanner state. In: state after last token. Out: state after current token.</param>
			/// <returns></returns>
			private bool ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
			{
				bool foundToken = GetNextToken(m_Offset, tokenInfo, ref state);
				if (foundToken)
				{
					m_Offset = tokenInfo.EndIndex + 1;

					if (tokenInfo is FactTokenInfo)
					{
						FactTokenInfo factToken = tokenInfo as FactTokenInfo;
						factToken.Value = m_Line.Substring(factToken.StartIndex, factToken.Length);
#if FACTEDITOR_TIPTEXT
						factToken.ExistsOnModel = this.ObjectTypeExists(factToken.Value);
#endif // FACTEDITOR_TIPTEXT
					}
				}
				return foundToken;
			}
			bool IScanner.ScanTokenAndProvideInfoAboutIt(TokenInfo tokenInfo, ref int state)
			{
				return ScanTokenAndProvideInfoAboutIt(tokenInfo, ref state);
			}
			/// <summary>
			/// Used to (re)initialize the scanner before scanning a small portion of text, such as single source line for syntax coloring purposes.
			/// Implements <see cref="IScanner.SetSource"/>
			/// </summary>
			/// <param name="source">The source text portion to be scanned.</param>
			/// <param name="offset">The index of the first character to be scanned.</param>
			private void SetSource(string source, int offset)
			{
				m_Line = source;
				m_Offset = offset;
				m_ParenCount = 0;
				m_SquareBracketCount = 0;
			}
			void IScanner.SetSource(string source, int offset)
			{
				SetSource(source, offset);
			}
			#endregion // IScanner Implementation
			#region Private Methods
			private bool GetNextToken(int startIndex, TokenInfo tokenInfo, ref int state)
			{
				bool bFoundToken = false;      // Assume we are done with this line.
				int index = startIndex;

				if (index < m_Line.Length)
				{
					bFoundToken = true;            // We are not done with this line.
					tokenInfo.StartIndex = index;
					Char c = m_Line[index];

					if (Char.IsPunctuation(c))
					{
						tokenInfo.Type = TokenType.Delimiter;
						tokenInfo.Color = (TokenColor)FactEditorColorizableItem.Delimiter;
						tokenInfo.EndIndex = index;

						if (m_Braces.IndexOf(c) != -1)
						{
							if (c == '(')
							{
								++m_ParenCount;
								tokenInfo.Trigger = TokenTriggers.MemberSelect | OpenParenthesisTokenTrigger;
							}
							else if (c == ')')
							{
								if (m_ParenCount > 0)
								{
									--m_ParenCount;
								}
							}
							else if (c == '[')
							{
								++m_SquareBracketCount;
								tokenInfo.Trigger = TokenTriggers.MemberSelect | OpenSquareBracketTokenTrigger;
							}
							else if (c == ']')
							{
								if (m_SquareBracketCount > 0)
								{
									--m_SquareBracketCount;
								}
							}
							tokenInfo.Trigger |= TokenTriggers.MatchBraces;
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
						if (m_ParenCount != 0)
						{
							tokenInfo.Color = (TokenColor)FactEditorColorizableItem.ReferenceModeName;
						}
						else if (m_SquareBracketCount != 0)
						{
							tokenInfo.Color = (TokenColor)FactEditorColorizableItem.ObjectName;
						}
						else
						{
							tokenInfo.Color = (TokenColor)FactEditorColorizableItem.PredicateText;
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
						if (m_ParenCount != 0)
						{
							tokenInfo.Color = (TokenColor)FactEditorColorizableItem.ReferenceModeName;
						}
						else
						{
							tokenInfo.Color = (TokenColor)FactEditorColorizableItem.ObjectName;
						}
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
						tokenInfo.Color = (TokenColor)FactEditorColorizableItem.PredicateText;
						tokenInfo.EndIndex = index - 1;
						tokenInfo.Trigger = TokenTriggers.None;
					}
				}
				return bFoundToken;
			}
			#endregion // Private Methods
			#region Model Helper Methods
#if FACTEDITOR_TIPTEXT
			private bool ObjectTypeExists(string name)
			{
				IORMDesignerView currentDesignerView = m_View;
				if (currentDesignerView != null)
				{
					// UNDONE: This is painfully slow. Use ObjectTypesDictionary on the current model
					List<ObjectType> temp = new List<ObjectType>();
					temp.AddRange(currentDesignerView.DocData.Store.ElementDirectory.FindElements<ObjectType>());
					//temp.Sort(Modeling.NamedElementComparer<ObjectType>.CurrentCulture);

					foreach (ObjectType o in temp)
					{
						if (o.Name == name)
							return true;
					}
				}

				return false;
			}
#endif // FACTEDITOR_TIPTEXT
			#endregion // Model Helper Methods
			#region Private Properties
			/// <summary>
			/// Gets or sets the current designer view.
			/// </summary>
			private IORMDesignerView CurrentDesignerView
			{
				get
				{
					return m_View;
				}
				set
				{
					IORMDesignerView oldView = m_View;
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
			#endregion // Private Properties
		}
	}
}
