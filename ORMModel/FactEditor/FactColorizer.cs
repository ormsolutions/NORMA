#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;

#endregion

namespace Northface.Tools.ORM.FactEditor
{
	/// <summary>
	/// A class which handles applying color to marked tokens
	/// </summary>
	public class FactColorizer : IVsColorizer
	{
		private FactParser myParser;
		private IVsTextLines myTextLines;


		/// <summary>
		/// Create a new colorizer object
		/// </summary>
		/// <param name="parser">The parser that knows how to parse the lines</param>
		/// <param name="textLines">The data source of lines to parse</param>
		public FactColorizer(FactParser parser, IVsTextLines textLines)
		{
			myParser = parser;
			myTextLines = textLines;
		}

		#region IVsColorizer Members

		void IVsColorizer.CloseColorizer()
		{
			CloseColorizer();
		}
		/// <summary>
		/// Implements IVsColorizer.CloseColorizer
		/// </summary>
		protected void CloseColorizer()
		{

		}

		// Colorize the given text.  For each character in the line of text
		// given, a matching index into the colorizer's syntax item array (+1) should
		// be placed in the color index array provided.  The colorizer should
		// start in the state provided, and return its current state (which will
		// be stored and provided as the start state when the following line is
		// colorized).  The index array given is gauranteed to be ONE ELEMENT
		// longer than the number of characters in the line.  This element is
		// used to determine the (background) color of the space to the right of
		// the last character on the line.
		// Color indexes are 1-BASED -- use 0 to specify default text color.
		// pText is NOT null-terminated.  iLength is the length of the line
		// MINUS the end-of-line marker (CR, LF, CRLF pair, or 0 (EOF)), which
		// will be present.
		//
		// NOTES:
		// The color of each character in the line is specified by setting a 'value'
		// in the provided pAttributes array.
		// The 'value' is really an index into an array of possible ColorableItems.
		// We are using values from the DEFAULTITEMS enum in textmgr.idl.
		// Our sample language uses the stock set of color items (which can be changed
		// in the options dialog). 
		// It is possible to invent your own unique set of colors but you would need
		// to implement IVsProvideColorableItems and IVsColorableItem. 
		// 
		int IVsColorizer.ColorizeLine(int iLine, int iLength, IntPtr pszText, int iState, uint[] pAttributes)
		{
			return ColorizeLine(iLine, iLength, pszText, iState, pAttributes);
		}
		/// <summary>
		/// Implements IVsColorizer.ColorizeLine
		/// </summary>
		/// <param name="iLine"></param>
		/// <param name="iLength"></param>
		/// <param name="pszText"></param>
		/// <param name="iState"></param>
		/// <param name="pAttributes"></param>
		/// <returns></returns>
		protected int ColorizeLine(int iLine, int iLength, IntPtr pszText, int iState, uint[] pAttributes)
		{
			if (null == pAttributes)
			{
				return NativeMethods.E_INVALIDARG;
			}

			// set all colors to default or shell crashes
			int len = pAttributes.Length;
			for (int i = 0; i < len; ++i)
			{
				pAttributes[i] = (uint)DEFAULTITEMS.COLITEM_TEXT;
			}

			if (pszText == IntPtr.Zero)
			{
				return (int)DEFAULTITEMS.COLITEM_TEXT;
			}
			if (iLength <= 0)
			{
				return (int)DEFAULTITEMS.COLITEM_TEXT;
			}
			if (myParser == null)
			{
				return (int)DEFAULTITEMS.COLITEM_TEXT;
			}
			
			// Create a string from the "const" IntPtr param
			// call the Line method on the parse object
			string s = Marshal.PtrToStringUni(pszText);

			FactLine factLine = new FactLine(s);
			(myParser as IFactParser).Line(ref factLine);

			ulong type; 
			int totalMarks = factLine.Marks.Count;
			for (int m = 0; m < totalMarks; ++m)
			{
				// TODO: Implement our own coloring provider (currently using DEFAULTITEMS)
				// We color one of the following 6 ways, as defined in textmgr.idl
				//COLITEM_TEXT = 0,           // Default
				//COLITEM_KEYWORD,            // Keyword
				//COLITEM_COMMENT,            // Comment
				//COLITEM_IDENTIFIER,         // Identifier
				//COLITEM_STRING,             // String
				//COLITEM_NUMBER              // Number
				switch (factLine.Marks[m].TokenType)
				{
					case FactTokenType.Object: type = (ulong)DEFAULTITEMS.COLITEM_KEYWORD; break;
					case FactTokenType.ReferenceMode: type = (ulong)DEFAULTITEMS.COLITEM_NUMBER; break;
					case FactTokenType.Parenthesis: type = (ulong)DEFAULTITEMS.COLITEM_TEXT; break;
					case FactTokenType.Predicate:
					default: type = (ulong)DEFAULTITEMS.COLITEM_STRING; break;
				}

				// color the token
				long iStart = (long)factLine.Marks[m].nStart;
				long iEnd = (long)factLine.Marks[m].nEnd;
				for (long i = iStart; i <= iEnd; i++)
					pAttributes[i] = (uint)type;
			}

			pAttributes[iLength] = pAttributes[iLength - 1];
			return (int) DEFAULTITEMS.COLITEM_TEXT; // return the current state
		}

		int IVsColorizer.GetStartState(out int piStartState)
		{
			return GetStartState(out piStartState);
		}
		/// <summary>
		/// Implements IVsColorizer.GetStartState
		/// </summary>
		/// <param name="piStartState"></param>
		/// <returns></returns>
		protected int GetStartState(out int piStartState)
		{
			piStartState = 0;
			return NativeMethods.S_OK;
		}

		int IVsColorizer.GetStateAtEndOfLine(int iLine, int iLength, IntPtr pText, int iState)
		{
			return GetStateAtEndOfLine(iLine, iLength, pText, iState);
		}
		/// <summary>
		/// Implements IVsColorizer.GetStateAtEndOfLine
		/// </summary>
		/// <param name="iLine"></param>
		/// <param name="iLength"></param>
		/// <param name="pText"></param>
		/// <param name="iState"></param>
		/// <returns></returns>
		protected int GetStateAtEndOfLine(int iLine, int iLength, IntPtr pText, int iState)
		{
			return 0;
		}

		int IVsColorizer.GetStateMaintenanceFlag(out int pfFlag)
		{
			return GetStateMaintenanceFlag(out pfFlag);
		}
		/// <summary>
		/// Implements IVsColorizer.GetStateMaintenanceFlag
		/// </summary>
		/// <param name="pfFlag"></param>
		/// <returns></returns>
		protected int GetStateMaintenanceFlag(out int pfFlag)
		{
			pfFlag = 0;
			return NativeMethods.S_OK;
		}

		#endregion
	}
}
