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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
#endregion

namespace Neumont.Tools.ORM.Shell.FactEditor
{
	/// <summary>
	/// Provides a language service for a Managed fact editor.
	/// </summary>
	[Guid(NewFactLanguageService.GuidString)]
	[CLSCompliant(false)]
	public sealed class NewFactLanguageService : LanguageService
	{
		#region Private Members
		private string m_Name = "Needs a Name";
		private NewFactScanner m_FactScanner;
		private LanguagePreferences m_LanguagePreferences;
		private ImageList m_ImageList;
		private ColorableItem[] m_ColorableItems;
		private List<Source> m_Sources;
		#endregion

		#region Guid Fields
		/// <summary>
		/// Guid string for the language service.
		/// </summary>
		public static readonly Guid Guid = new Guid(GuidString);

		/// <summary>
		/// Guid string for the language service.
		/// </summary>
		public const string GuidString = "CC5B0775-F625-4779-8917-D8740D727149";
		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="NewFactLanguageService"/> class.
		/// </summary>
		public NewFactLanguageService()
		{
			m_Sources = new List<Source>();

			m_ImageList = new ImageList();
			m_ImageList.ImageSize = new Size(16, 16);
			m_ImageList.TransparentColor = System.Drawing.Color.Lime;
			m_ImageList.ImageStream = ResourceStrings.FactEditorIntellisenseImageList;

			m_ColorableItems = new ColorableItem[]
            {
                new ColorableItem("My Language - Text",
                                  "My Language - Text",
                                  COLORINDEX.CI_SYSPLAINTEXT_FG,
                                  COLORINDEX.CI_SYSPLAINTEXT_BK,
                                  System.Drawing.Color.Empty,
                                  System.Drawing.Color.Empty,
                                  FONTFLAGS.FF_BOLD),
                new ColorableItem("My Language - Keyword",
                                  "My Language - Keyword",
                                  COLORINDEX.CI_MAROON,
                                  COLORINDEX.CI_SYSPLAINTEXT_BK,
                                  System.Drawing.Color.FromArgb(192,32,32),
                                  System.Drawing.Color.Empty,
                                  FONTFLAGS.FF_BOLD),
                new ColorableItem("My Language - Comment",
                                  "My Language - Comment",
                                  COLORINDEX.CI_DARKGREEN,
                                  COLORINDEX.CI_LIGHTGRAY,
                                  System.Drawing.Color.FromArgb(32,128,32),
                                  System.Drawing.Color.Empty,
                                  FONTFLAGS.FF_BOLD)

            };
		}
		#endregion

		#region Overriden Properties
		/// <summary>
		/// Returns the name of the language (for example, "C++" or "HTML").
		/// </summary>
		/// <returns>
		/// Returns a string containing the name of the language. This must return the same
		/// name the language service was registered with in Visual Studio.
		/// </returns>
		public override string Name
		{
			get { return m_Name; }
		}
		#endregion

		#region Overriden Methods
		/// <summary>Returns a list of file extension filters suitable for a Save As dialog box.</summary>
		/// <returns>If successful, returns a string containing the file extension filters; otherwise, returns an empty string.</returns>
		public override string GetFormatFilterList()
		{
			return string.Empty;
		}

		/// <summary>
		/// Instantiates a <see cref="T:Microsoft.VisualStudio.Package.Source"></see> class.
		/// </summary>
		/// <param name="buffer">[in] The <see cref="T:Microsoft.VisualStudio.TextManager.Interop.IVsTextLines"></see>
		/// buffer that the <see cref="T:Microsoft.VisualStudio.Package.Source"></see> object represents.</param>
		/// <returns>
		/// If successful, returns a <see cref="T:Microsoft.VisualStudio.Package.Source"></see>
		/// object; otherwise, returns a null value.
		/// </returns>
		public override Source CreateSource(IVsTextLines buffer)
		{
			NewFactSource source = new NewFactSource(this, buffer, this.GetColorizer(buffer));

			m_Sources.Add(source);

			return source;
		}

		/// <summary>
		/// Creates a IVsTextLines and forwards the call to the overloaded GetOrCreateSource method.
		/// </summary>
		/// <param name="view">The IVsTextView to get or create a source from.</param>
		public Source GetOrCreateSource(IVsTextView view)
		{
			if (view == null)
			{
				return null;
			}

			IVsTextLines ppBuffer;
			ErrorHandler.ThrowOnFailure(view.GetBuffer(out ppBuffer));

			return this.GetOrCreateSource(ppBuffer);
		}

#if !VISUALSTUDIO_9_0
		/// <summary>
		/// Calls GetSource with the given IVsTextLines.  If the source does not exist
		/// it is created and returned.
		/// </summary>
		/// <param name="lines">The IVsTextLines of the source you want to get or create.</param>
		public Source GetOrCreateSource(IVsTextLines lines)
		{
			if (lines == null)
			{
				return null;
			}

			Source s = this.GetSource(lines);

			if (s != null)
			{
				return s;
			}
			else
			{
				return this.CreateSource(lines);
			}
		}
#endif //!VISUALSTUDIO_9_0

		/// <summary>
		/// Returns an existing <see cref="T:Microsoft.VisualStudio.Package.Source"></see>
		/// object that contains the source file shown in the specified text view.
		/// </summary>
		/// <param name="view">[in] An <see cref="T:Microsoft.VisualStudio.TextManager.Interop.IVsTextView"></see>
		/// object that displays the source file for which to get the <see cref="T:Microsoft.VisualStudio.Package.Source"></see> object.</param>
		/// <returns>
		/// If successful, returns a <see cref="T:Microsoft.VisualStudio.Package.Source"></see> object;
		/// otherwise, returns a null value (there is no <see cref="T:Microsoft.VisualStudio.Package.Source"></see>
		/// object in this language service that controls the set of source lines shown in the specified view).
		/// </returns>
		new public Source GetSource(IVsTextView view)
		{
			IVsTextLines ppBuffer;
			if (view == null)
			{
				return null;
			}
			ErrorHandler.ThrowOnFailure(view.GetBuffer(out ppBuffer));
			return this.GetSource(ppBuffer);
		}

		/// <summary>
		/// Returns an existing <see cref="T:Microsoft.VisualStudio.Package.Source"></see>
		/// object that contains the specified buffer of source.
		/// </summary>
		/// <param name="buffer">[in] An <see cref="T:Microsoft.VisualStudio.TextManager.Interop.IVsTextLines"></see>
		/// object containing the lines of source.</param>
		/// <returns>
		/// If successful, returns a <see cref="T:Microsoft.VisualStudio.Package.Source"></see> object;
		/// otherwise, returns a null value (there is no <see cref="T:Microsoft.VisualStudio.Package.Source"></see>
		/// object in this language service that controls that set of source lines).
		/// </returns>
		new public Source GetSource(IVsTextLines buffer)
		{
			Source source = base.GetSource(buffer);

			if (source != null)
				return source;

			foreach (Source s in this.m_Sources)
			{
				if (s.GetTextLines() == buffer)
				{
					return s;
				}
			}

			return null;
		}

		/// <summary>
		/// Returns the requested <see cref="T:Microsoft.VisualStudio.TextManager.Interop.IVsColorableItem"></see> object.
		/// </summary>
		/// <param name="index">[in] A zero-based index into the list of colorable items maintained by the language service.</param>
		/// <param name="item">[out] Returns the <see cref="T:Microsoft.VisualStudio.TextManager.Interop.IVsColorableItem"></see> object.</param>
		/// <returns>
		/// If successful, returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"></see>;
		/// otherwise, returns an error code.
		/// </returns>
		public override Int32 GetColorableItem(Int32 index, out IVsColorableItem item)
		{
			if (index > (m_ColorableItems.Length - 1))
			{
				item = null;
				return -1;
			}

			item = m_ColorableItems[0];

			return VSConstants.S_OK;
		}


		/// <summary>
		/// Returns the number of custom colorable items supported by the language service.
		/// </summary>
		/// <param name="count">[out] The number of custom colorable items available.</param>
		/// <returns>
		/// If successful, returns <see cref="F:Microsoft.VisualStudio.VSConstants.S_OK"></see>;
		/// otherwise, returns an error code.
		/// </returns>
		public override int GetItemCount(out int count)
		{
			count = m_ColorableItems.Length;

			return VSConstants.S_OK;
		}

		/// <summary>
		/// Returns a <see cref="T:Microsoft.VisualStudio.Package.LanguagePreferences"></see>
		/// object for this language service.
		/// </summary>
		/// <returns>
		/// If successful, returns a <see cref="T:Microsoft.VisualStudio.Package.LanguagePreferences"></see> object;
		/// otherwise, returns a null value.
		/// </returns>
		public override LanguagePreferences GetLanguagePreferences()
		{
			if (m_LanguagePreferences == null)
			{
				m_LanguagePreferences = new LanguagePreferences(this.Site, typeof(NewFactLanguageService).GUID, this.Name);

				if (this.m_LanguagePreferences != null)
				{
					this.m_LanguagePreferences.Init();  // Must do this first!

					m_LanguagePreferences.EnableCodeSense = true;
					m_LanguagePreferences.EnableMatchBraces = true;
					m_LanguagePreferences.EnableCommenting = true;
					m_LanguagePreferences.LineNumbers = true;
					m_LanguagePreferences.EnableQuickInfo = true;
					m_LanguagePreferences.EnableMatchBracesAtCaret = true;
					m_LanguagePreferences.EnableShowMatchingBrace = true;
					m_LanguagePreferences.MaxErrorMessages = 10;
				}
			}
			return m_LanguagePreferences;
		}

		/// <summary>
		/// Returns a single instantiation of a parser.
		/// </summary>
		/// <param name="buffer">[in] An <see cref="T:Microsoft.VisualStudio.TextManager.Interop.IVsTextLines"></see>
		/// representing the lines of source to parse.</param>
		/// <returns>
		/// If successful, returns an <see cref="T:Microsoft.VisualStudio.Package.IScanner"></see> object;
		/// otherwise, returns a null value.
		/// </returns>
		public override IScanner GetScanner(IVsTextLines buffer)
		{
			if (m_FactScanner == null)
				m_FactScanner = new NewFactScanner(this, buffer);
			return m_FactScanner;
		}

		/// <summary>
		/// Parses the source based on the specified <see cref="T:Microsoft.VisualStudio.Package.ParseRequest"></see> object.
		/// </summary>
		/// <param name="req">[in] The <see cref="T:Microsoft.VisualStudio.Package.ParseRequest"></see>
		/// describing how to parse the source file.</param>
		/// <returns>
		/// If successful, returns an <see cref="T:Microsoft.VisualStudio.Package.AuthoringScope"></see> object;
		/// otherwise, returns a null value.
		/// </returns>
		public override AuthoringScope ParseSource(ParseRequest req)
		{
			//Trace.WriteLine(String.Format("\nParse Request: {0}\n", req.Reason.ToString()));

			Source s = this.GetSource(req.View);

			return new NewFactAuthoringScope(this, req);
		}

		/// <summary>
		/// Returns an image list containing glyphs associated with the language service.
		/// </summary>
		/// <returns>
		/// If successful, returns an <see cref="T:System.Windows.Forms.ImageList"></see> object; otherwise, returns a null value.
		/// </returns>
		public override System.Windows.Forms.ImageList GetImageList()
		{
			return m_ImageList;
		}
		#endregion
	}
}
