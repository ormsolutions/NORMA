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
	/// This class provides the ability to show tooltips and
	/// provide an intellisense list (Declarations).
	/// </summary>
	[CLSCompliant(false)]
	public sealed class NewFactAuthoringScope : AuthoringScope
	{
		#region Private Members
		private NewFactLanguageService m_LanguageService;
		private ParseRequest m_ParseRequest;
		#endregion

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="NewFactAuthoringScope"/> class.
		/// </summary>
		/// <param name="languageService">The language service.</param>
		/// <param name="parseRequest">The parse request.</param>
		public NewFactAuthoringScope(NewFactLanguageService languageService, ParseRequest parseRequest)
		{
			m_LanguageService = languageService;
			m_ParseRequest = parseRequest;
		}
		#endregion

		#region Overriden Methods
		/// <summary>
		/// Returns a string to be used for a tool tip based on the specified location.
		/// </summary>
		/// <param name="line">[in] The line in the source to look at for a tool tip.</param>
		/// <param name="col">[in] An offset within the line to look at for a tool tip.</param>
		/// <param name="span">A <see cref="T:Microsoft.VisualStudio.TextManager.Interop.TextSpan"></see>
		/// that describes the area over which the cursor can hover before the tool tip is dismissed from view.</param>
		/// <returns>
		/// If successful, returns a string containing the text for the tool tip; otherwise, returns a null value.
		/// </returns>
		[CLSCompliant(false)]
		public override String GetDataTipText(Int32 line, Int32 col, out TextSpan span)
		{
			// new up the span
			span = new TextSpan();

			// if the line isn't the first, just return null
			if (line > 0)
			{
				return null;
			}

			// get the view of the parse request
			IVsTextView view = m_ParseRequest.View;

			// get the token at the given column index
			FactTokenInfo token = FactTokenHelper.GetTokenAtIndex(col, view, m_LanguageService);

			if (token == null)
			{
				return null;
			}

			// populate the span information
			span.iStartLine = line;
			span.iStartIndex = token.StartIndex;
			span.iEndIndex = token.EndIndex + 1;

			// if the token does not exist on the model create a document task
			// (that is the squiggly line)
			if (!token.ExistsOnModel)
			{
				//Source s = m_LanguageService.GetOrCreateSource(view);

				//if (s != null)
				//{
				//    DocumentTask task = s.CreateErrorTaskItem(span, MARKERTYPE.MARKER_SYNTAXERROR, String.Empty);
				//    task.CanDelete = true;
				//}
			}

			// return the string to show as the tooltip
			return String.Format("{0} {1} exist on the model.", token.Value, (token.ExistsOnModel == true) ? "does" : "does not");
		}

		/// <summary>
		/// Returns a list of declarations based on the specified reason for parsing.
		/// </summary>
		/// <param name="view">[in] An <see cref="T:Microsoft.VisualStudio.TextManager.Interop.IVsTextView"></see> object that can be used to access the source.</param>
		/// <param name="line">[in] The line number where the parse operation started.</param>
		/// <param name="col">[in] The offset into the line where the parse operation started.</param>
		/// <param name="info">[in] A <see cref="T:Microsoft.VisualStudio.Package.TokenInfo"></see> structure containing information about the token at the specified position.</param>
		/// <param name="reason">[in] The <see cref="T:Microsoft.VisualStudio.Package.ParseReason"></see> value describing what kind of parse operation was completed.</param>
		/// <returns>
		/// If successful returns a <see cref="T:Microsoft.VisualStudio.Package.Declarations"></see> object; otherwise, returns a null value.
		/// </returns>
		[CLSCompliant(false)]
		public override Declarations GetDeclarations(IVsTextView view, Int32 line, Int32 col, TokenInfo info, ParseReason reason)
		{
			return new NewFactDeclarations(m_LanguageService);
		}

		/// <summary>
		/// Returns a list of overloaded method signatures for a specified method name.
		/// </summary>
		/// <param name="line">[in] The line number where the parse for method signatures started.</param>
		/// <param name="col">[in] The offset into the line where the parse for method signatures started.</param>
		/// <param name="name">[in] The name of the method for which to get permutations.</param>
		/// <returns>
		/// If successful, returns a <see cref="T:Microsoft.VisualStudio.Package.Methods"></see> object; otherwise, returns a null value.
		/// </returns>
		[CLSCompliant(false)]
		public override Methods GetMethods(Int32 line, Int32 col, String name)
		{
			return null;
		}

		/// <summary>
		/// Returns a URI (Universal Resource Identifier) based on the current location in the source and the specified command.
		/// </summary>
		/// <param name="cmd">[in] A value from the <see cref="T:Microsoft.VisualStudio.VSConstants.VSStd97CmdID"></see> enumeration that determines what kind of destination URI must be returned. This is the command the user entered, typically from a context menu.</param>
		/// <param name="textView">[in] The <see cref="T:Microsoft.VisualStudio.TextManager.Interop.IVsTextView"></see> object containing the text under the cursor.</param>
		/// <param name="line">[in] The line number containing the text under the cursor.</param>
		/// <param name="col">[in] The offset into the line containing the text under the cursor.</param>
		/// <param name="span">[out] A <see cref="T:Microsoft.VisualStudio.TextManager.Interop.TextSpan"></see> object marking the selected text area for which the URI is determined.</param>
		/// <returns>
		/// If successful, returns a string containing the URI; otherwise, returns a null value.
		/// </returns>
		[CLSCompliant(false)]
		public override String Goto(VSConstants.VSStd97CmdID cmd, IVsTextView textView, Int32 line, Int32 col, out TextSpan span)
		{
			span = new TextSpan();
			return null;
		}
		#endregion
	}
}
