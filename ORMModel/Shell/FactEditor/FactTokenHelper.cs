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

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
#endregion

namespace Neumont.Tools.ORM.Shell.FactEditor
{
	/// <summary>
	/// A helper class to easily retrieve tokens from a given IVsTextView.
	/// </summary>
	internal static class FactTokenHelper
	{
		/// <summary>
		/// Gets the specific token that resides at the given index.
		/// </summary>
		/// <param name="index">The index of the token to get.</param>
		/// <param name="view">The IVsTextView of the lines of source to parse.</param>
		/// <param name="service">The language service.</param>
		/// <returns>The <see cref="T:Microsoft.VisualStudio.Package.TokenInfo"/> at the index.</returns>
		public static FactTokenInfo GetTokenAtIndex(Int32 index, IVsTextView view, NewFactLanguageService service)
		{
			IVsTextLines lines;

			ErrorHandler.ThrowOnFailure(view.GetBuffer(out lines));

			return GetTokenAtIndex(index, lines, service);
		}

		/// <summary>
		/// Gets the specific token that resides at the given index.
		/// </summary>
		/// <param name="index">The index of the token to get.</param>
		/// <param name="lines">The IVsTextLines of the lines of source to parse.</param>
		/// <param name="service">The language service.</param>
		/// <returns>The <see cref="T:Microsoft.VisualStudio.Package.TokenInfo"/> at the index.</returns>
		public static FactTokenInfo GetTokenAtIndex(Int32 index, IVsTextLines lines, NewFactLanguageService service)
		{
			List<FactTokenInfo> tokens = GetTokens(lines, service);

			foreach (FactTokenInfo token in tokens)
			{
				if (index >= token.StartIndex & index <= token.EndIndex)
				{
					return token;
				}
			}

			return null;
		}

		/// <summary>
		/// Gets a list of tokens from the IVsTextView.
		/// </summary>
		/// <param name="view">The IVsTextLines of the lines of source to parse.</param>
		/// <param name="service">The language service.</param>
		/// <returns>A list of tokens.</returns>
		public static List<FactTokenInfo> GetTokens(IVsTextView view, NewFactLanguageService service)
		{
			IVsTextLines lines;

			ErrorHandler.ThrowOnFailure(view.GetBuffer(out lines));

			return GetTokens(lines, service);
		}

		/// <summary>
		/// Gets a list of tokens from the IVsTextLines.
		/// </summary>
		/// <param name="lines">The IVsTextLines of the lines of source to parse.</param>
		/// <param name="service">The language service.</param>
		/// <returns>A list of tokens.</returns>
		public static List<FactTokenInfo> GetTokens(IVsTextLines lines, NewFactLanguageService service)
		{
			Source source = service.GetOrCreateSource(lines);
			String lineText = source.GetLine(0);

			// Get the scanner from the language service.
			IScanner scanner = service.GetScanner(lines);
			scanner.SetSource(lineText, 0);

			// Now use the scanner to parse the first line and build the list of the tokens.
			List<FactTokenInfo> tokens = new List<FactTokenInfo>();
			FactTokenInfo lastToken = null;
			FactTokenInfo currentToken = new FactTokenInfo();
			int state = 0;
			while (scanner.ScanTokenAndProvideInfoAboutIt(currentToken, ref state))
			{
				if ((null != lastToken) && (currentToken.StartIndex > lastToken.EndIndex + 1))
				{
					tokens.Clear();
				}
				tokens.Add(currentToken);
				lastToken = currentToken;
				currentToken = new FactTokenInfo();
			}

			return tokens;
		}
	}
}
