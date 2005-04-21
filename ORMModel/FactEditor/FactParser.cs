#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;

#endregion

namespace Northface.Tools.ORM.FactEditor
{
	/// <summary>
	/// The FactLine class is used to organize markings within a line for
	/// colorization. We look for object types, predicates, and reference modes
	/// </summary>
	public sealed class FactLine
	{
		private string myLineText;
		private Collection<FactTokenMark> myMarks;
		private string myError = "";

		/// <summary>
		/// Create a new FactLine
		/// </summary>
		/// <param name="line">The source line text</param>
		public FactLine(string line) 
		{
			myLineText = line;
			myMarks = new Collection<FactTokenMark>();
		}

		/// <summary>
		/// The source text for this line
		/// </summary>
		/// <value></value>
		public string LineText
		{
			get { return myLineText; }
		}

		/// <summary>
		/// The markings on the line to indicate where different types of tokens are located
		/// </summary>
		/// <value></value>
		public Collection<FactTokenMark> Marks
		{
			get { return myMarks; }
			set { myMarks = value; }
		}

		/// <summary>
		/// An error that may have been found in setting the marks
		/// </summary>
		/// <value></value>
		public string Error
		{
			get { return myError; }
			set { myError = value; }
		}

		/// <summary>
		/// Indicates whether the current FactLine state has an error
		/// </summary>
		/// <value></value>
		public bool HasError
		{
			get { return myError.Length > 0; }
		}
	}

	/// <summary>
	/// A parser which breaks down a string into marks representing object types, predicates, and reference modes
	/// </summary>
	public class FactParser : IFactParser
	{
		private static Regex ReferenceModeRegEx = new Regex(@"\((?<refmode>[^\(\)]+)\)", RegexOptions.Singleline | RegexOptions.Compiled);

		/// <summary>
		/// Default constructor
		/// </summary>
		public FactParser() { }

		#region IFactParser Members

		int IFactParser.Line(ref FactLine factLine)
		{
			return Line(ref factLine);
		}

		/// <summary>
		/// Examines the line text of a FactLine object and marks object types, predicates, and reference modes
		/// </summary>
		/// <param name="factLine">The source of the line text to examine</param>
		/// <returns>HRESULT</returns>
		protected int Line(ref FactLine factLine)
		{
			int hResult = NativeMethods.S_OK;
			try
			{
				// break the string into lines and words for processing - just get the first line
				string tmpLine = factLine.LineText.Replace("\r", "");
				string[] lines = tmpLine.Split('\n');
				tmpLine = lines[0];
				
				// remove whitespace at right
				string trimR = tmpLine.TrimEnd(new char[] { ' ', '\t' });
				// find first index of a letter
				int runningStartPos = 0;
				for (int ws = 0; ws < trimR.Length; ++ws)
				{
					if (char.IsLetter(trimR[ws]))
					{
						runningStartPos = ws;
						break;
					}
				}

				string[] words = trimR.TrimStart(new char[] { ' ', '\t' }).Split(' ');
				string curWord = "";
				int curLen = 0;
				int nrObjects = 0;

				// examine each word on this line for recognized tokens
				factLine.Marks.Clear();
				for (int i = 0; i < words.Length; i++)
				{
					FactTokenMark iterMark;
					curWord = words[i];
					curLen = curWord.Length;

					// If we are dealing with an Object type.
					if (curLen > 0 && char.IsUpper(curWord[0]))
					{
						// add a mark to make the whole word an object type
						FactTokenMark iterMarkObj = new FactTokenMark();

						Match refMatch = ReferenceModeRegEx.Match(curWord);
						if (refMatch.Groups["refmode"].Value.Length != 0)
						{
							iterMarkObj.TokenType = FactTokenType.EntityType;
						}
						else
						{
							iterMarkObj.TokenType = FactTokenType.ValueType;
						}
						iterMarkObj.nStart = runningStartPos;
						iterMarkObj.nEnd = runningStartPos + curWord.Length - 1;
						factLine.Marks.Add(iterMarkObj);

						// now, go through and add marks (some will overlap previous marks for reference modes
						for (Match m = ReferenceModeRegEx.Match(curWord); m.Success; m = m.NextMatch())
						{
							// mark the left paren
							iterMark = new FactTokenMark();
							iterMark.TokenType = FactTokenType.Parenthesis;
							iterMark.nStart = runningStartPos + m.Index;
							iterMark.nEnd = iterMark.nStart;
							factLine.Marks.Add(iterMark);

							// mark the reference mode, if there is one
							Group refModeGroup = m.Groups["refmode"];
							int tmpLen = refModeGroup.Length;
							if (tmpLen > 0)
							{
								int refModeIndex = refModeGroup.Index;
								iterMark = new FactTokenMark();
								iterMark.TokenType = FactTokenType.ReferenceMode;
								iterMark.nStart = runningStartPos + refModeIndex;
								iterMark.nEnd = iterMark.nStart + tmpLen - 1;
								factLine.Marks.Add(iterMark);

								// mark the right paren
								iterMark = new FactTokenMark();
								iterMark.TokenType = FactTokenType.Parenthesis;
								iterMark.nStart = runningStartPos + refModeIndex + tmpLen;
								iterMark.nEnd = runningStartPos + refModeIndex + tmpLen;
								factLine.Marks.Add(iterMark);
							}
							else
							{
								// mark the right paren as one char after the left paren
								iterMark = new FactTokenMark();
								iterMark.TokenType = FactTokenType.Parenthesis;
								iterMark.nStart = runningStartPos + m.Index + 1;
								iterMark.nEnd = runningStartPos + m.Index + 1;
								factLine.Marks.Add(iterMark);
							}
						}
						++nrObjects;
					}
					else
					{
						iterMark = new FactTokenMark();
						iterMark.TokenType = FactTokenType.Predicate;
						iterMark.nStart = runningStartPos;
						iterMark.nEnd = iterMark.nStart + curLen - 1;
						factLine.Marks.Add(iterMark);
					}

					// add the length of the string to the running start position
					runningStartPos += curLen + 1; // add 1 for the width of the separator
				}
				
				// TODO: localize error in squiggly lines (error message is currently unused)
				if (nrObjects < 1)
				{
					factLine.Error = "Fact must contain at least one object.";
				}
			}
			catch (Exception)
			{
				hResult = NativeMethods.E_FAIL;
			}
			return hResult;
		}

		int IFactParser.ParseLine(Microsoft.VisualStudio.TextManager.Interop.IVsTextLines textLines, int lineNr, out FactLineType lineType, out string comment)
		{
			return ParseLine(textLines, lineNr, out lineType, out comment);
		}

		// UNDONE: ParseLine is not being called from anywhere currently, code is here for future use
		/// <summary>
		/// Parse a line and determine what type of line it is. Currently this method is not being used
		/// </summary>
		/// <param name="textLines"></param>
		/// <param name="lineNr"></param>
		/// <param name="lineType"></param>
		/// <param name="comment"></param>
		/// <returns>HRESULT</returns>
		protected int ParseLine(Microsoft.VisualStudio.TextManager.Interop.IVsTextLines textLines, int lineNr, out FactLineType lineType, out string comment)
		{
			lineType = FactLineType.Invalid;
			comment = "";
			int hResult = NativeMethods.S_OK;

			const int   ciMacFigMarks = 100;
			int			nLineLength;
			string		cbstrLine;
			FactLineType flt = FactLineType.Invalid;
			string		cbstrError = "";
			string		wszLine = "";

			textLines.GetLengthOfLine(lineNr, out nLineLength);
			if (nLineLength == 0)
				return NativeMethods.S_OK;
    
			// Puts the specified span of text into a BSTR; it is the caller's responsibility to free the BSTR.
			textLines.GetLineText(
				lineNr,				// starting line
				0,					// starting character index within the line (must be <= length of line)
				lineNr,				// ending line
				nLineLength,		// ending character index within the line (must be <= length of line)
				out cbstrLine);		// line text, if any
			
			if(cbstrLine.Length == 0)
				return NativeMethods.S_OK;

			// parse the line
			int ncMarks  = ciMacFigMarks;
			FactTokenMark[] pMks = new FactTokenMark[ncMarks];
			for(int i = 0;i<ncMarks; ++i)
			{
				pMks[i] = new FactTokenMark();
			}

			// call the Line method on the parse object
			FactLine factLine = new FactLine(wszLine);
			hResult = Line(ref factLine);
			ncMarks = factLine.Marks.Count;
			cbstrError = factLine.Error;

			// what type of line ?   base on the parsed tokens	
			if (hResult != NativeMethods.S_OK || cbstrError.Length != 0)
			{
				// parse failed, so make comment the line text
				lineType = FactLineType.Invalid;
				comment = wszLine;  // return comment
				return NativeMethods.S_OK;
			}

			// make sure we have at least 1 object type
			for(int i = 0; i < ncMarks; ++i)
			{
				switch (pMks[i].TokenType)
				{
					case FactTokenType.EntityType:
					case FactTokenType.ValueType:
						flt = FactLineType.ContainsObject;
						break;
				}
			}
			lineType = flt;

			// did we get a valid line type ?
			if( flt == FactLineType.Invalid ) 
			{
				// invalid line, so make comment the line text
				comment = wszLine;
			}

			return NativeMethods.S_OK;
		}

		#endregion
	}
}
