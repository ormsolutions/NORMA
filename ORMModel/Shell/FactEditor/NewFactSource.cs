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
using System.Diagnostics;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio;
#endregion

namespace Neumont.Tools.ORM.Shell.FactEditor
{
	/// <summary>
	/// Represents a source file in the language service and controls
	/// parsing information on that source.
	/// </summary>
	[CLSCompliant(false)]
	public sealed class NewFactSource : Source
	{
		private IVsTextLines m_TextLines;

		/// <summary>
		/// Initializes a new instance of the <see cref="NewFactSource"/> class.
		/// </summary>
		/// <param name="languageService">The language service.</param>
		/// <param name="textLines">The text lines.</param>
		/// <param name="colorizer">The colorizer.</param>
		public NewFactSource(LanguageService languageService, IVsTextLines textLines, Colorizer colorizer)
			: base(languageService, textLines, colorizer)
		{
			m_TextLines = textLines;
		}

		/// <summary>
		/// Gets information about the token at the specified position.
		/// </summary>
		/// <param name="line">The number of the line containing the token to examine.</param>
		/// <param name="col">The character offset in the line to the token to examine.</param>
		/// <returns>
		/// A <see cref="T:Microsoft.VisualStudio.Package.TokenInfo"></see> object containing information about the current token.
		/// </returns>
		public override TokenInfo GetTokenInfo(Int32 line, Int32 col)
		{
			return base.GetTokenInfo(line, col);
		}
	}
}
