#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using Northface.Tools.ORM.Shell;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;

#endregion

namespace Northface.Tools.ORM.FactEditor
{
	/// <summary>
	/// Filter the text view and look for specific package commands
	/// </summary>
	public class FactTextViewFilter : IOleCommandTarget, IVsTextViewEvents, IVsTextViewFilter
	{
//		private bool myInit = false;
		private IOleCommandTarget myNextCmdTarget;
		private ORMDesignerPackage myPackage;
		private IVsTextView myTextView;
		private IVsTextLines myTextLines;
		private IFactParser myParser;
//		private IVsMethodTipWindow myMethodTipWindow;
//		private bool myTipWinDisplayed = false;
		private const int MAX_FIG_MARKS = 100;


		// other private members from CFigTextViewFilter (used for auto-complete and red squiggly lines)
//		CComPtr<CFigStatementCompletion> m_srpCompletionSet;
//		CFigMethodData* m_pMethodData;
//		CComPtr<IFigTaskManager> m_srpTaskManager;
//		DWORD m_dwTextViewEventsCookie;	// for IVsTextViewEvents advise sink

		/// <summary>
		/// Create a text view on the editor
		/// </summary>
		/// <param name="package">The package we are attached to</param>
		/// <param name="view">The view to filter</param>
		public FactTextViewFilter(ORMDesignerPackage package, IVsTextView view)
		{
			myPackage = package;
			myTextView = view;
		}

		#region Init
		/// <summary>
		/// Initalize the filter by adding the next command target on the text view,
		/// then create a parser
		/// </summary>
		/// <returns>HRESULT</returns>
		public int Init()
		{
			// keep a ptr to the text buffer
			int hr = NativeMethods.S_OK;
			hr = myTextView.GetBuffer(out myTextLines);
			if (hr != NativeMethods.S_OK)
				return hr;
			
			myTextView.AddCommandFilter(this, out myNextCmdTarget);

			// Get an object to tie to the IDE
			myParser = new FactParser();

			// UNDONE: create a method tip window here
//			hr = srpLocalRegistry->CreateInstance(
//						CLSID_VsMethodTipWindow, NULL, IID_IVsMethodTipWindow, CLSCTX_INPROC_SERVER,
//						(LPVOID*)&m_srpMethodTipWindow);
//			if (FAILED(hr))
//				return hr;
//
//			// give the method tip window object the method data object it will use
//			// when update is called.
//			hr = m_srpMethodTipWindow->SetMethodData(
//							static_cast<IVsMethodData*>(m_pMethodData));
//			if (FAILED(hr))
//				return hr;
//
//			// allocate an array of tokens for the parser
//			m_pMks = new FIGMARK[MAX_FIG_MARKS];
//			if (NULL == m_pMks)
//				return E_OUTOFMEMORY;
//
//			// hook ourselves into the command target chain
//			hr = m_srpTextView->AddCommandFilter(this, &m_srpNextCmdTarg);
//			if (FAILED(hr))
//				return hr;
//
//			// setup an IVsTextViewEvents advise on the view, intf is impl on this view filter obj.
//			CComPtr<IConnectionPointContainer> srpConPtCont;
//			hr = m_srpTextView->QueryInterface(IID_IConnectionPointContainer,
//													(void**)&srpConPtCont);
//			if (FAILED(hr))
//				return hr;
//			CComPtr<IConnectionPoint> srpConPt;
//			hr = srpConPtCont->FindConnectionPoint(IID_IVsTextViewEvents,
//													&srpConPt);

			// Remember to unadvise
//			if (FAILED(hr))
//				return hr;
//			hr = srpConPt->Advise(static_cast<IVsTextViewEvents*>(this),
//									&m_dwTextViewEventsCookie);
//			if (FAILED(hr))
//				return hr;
//
//
//			if (m_fInit)
//				return E_FAIL;
//			m_fInit = TRUE;

			return hr;
		}

		#endregion

		#region IOleCommandTarget Members

		int IOleCommandTarget.Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
		{
			return Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
		}
		/// <summary>
		/// Implements IOleCommandTarget.Exec
		/// </summary>
		/// <param name="pguidCmdGroup"></param>
		/// <param name="nCmdID"></param>
		/// <param name="nCmdexecopt"></param>
		/// <param name="pvaIn"></param>
		/// <param name="pvaOut"></param>
		/// <returns></returns>
		protected int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
		{
			// NOTE: pass on the commands we don't use to m_srpNextCmdTarg
			// otherwise they wont show up in the view.  Cmd Targs are CHAINED.
			int hr = 0;
			bool fHandled = true;

			// CMDSETID_StandardCommandSet2K is in stdidcmd.h and vsshlids.h in VSIP sdk
			if (typeof(ORMDesignerPackage).GUID == pguidCmdGroup)
			{
				// UNDONE: use this condition to look for recognized tokens for our package, e.g. quantifiers and constraints
//				int nLine;
//				int nCol, nCols;
//				if (myTextView.GetCaretPos(out nLine, out nCol) >= 0 &&
//					myTextLines.GetLengthOfLine(nLine, out nCols) >= 0)
//				{
//					string bstrCode = "";
//
//					if (0 != nCol) // Appending to end of a line?
//						bstrCode = "\n";
//					switch (nCmdID)
//					{
//						case 0x0101:
//							bstrCode += "draw arrow 0 0 0 0 'Arrow";
//							break;
//						case 0x0102:
//							bstrCode += "draw circle 0 0 0 'Circle";
//							break;
//						case 0x0103:
//							bstrCode += "'";
//							break;
//						case 0x0104:
//							bstrCode += "draw rectangle 0 0 0 0 'Rectangle";
//							break;
//						default:
//							return 0; // we don't support this command from our custom menu
//					}
//					// handle the special case of inserting to last line in the buffer:
//					int nLines;
//					hr = myTextLines.GetLineCount(out nLines);
//					if (hr >= 0)
//					{
//						Debug.Assert(nLines > 0, "Line count is not > 0");
//						if (0 == nCol   // Not appending to last line?
//							&& (nLine + 1 < nLines || 0 < nCols))
//							bstrCode += "\n";
//
//						// create a pointer to the BSTR
//						IntPtr pBstr = IntPtr.Zero;
//						pBstr = Marshal.StringToBSTR(bstrCode);
//						GCHandle h = GCHandle.Alloc(bstrCode, GCHandleType.Pinned);
//						string test = Marshal.PtrToStringUni(h.AddrOfPinnedObject());
//
//						// Inserts the specified span of text
//						TextSpan[] ts = new TextSpan[MAX_FIG_MARKS];
//						hr = myTextLines.ReplaceLines(
//							nLine,              // starting line
//							nCol,               // starting character index within the line (must be <= length of line)
//							nLine,              // ending line
//							nCol,               // ending character index within the line (must be <= length of line)
//							pBstr,           // new line text to insert
//							bstrCode.Length,  // # of chars to insert, if any
//							ts);              // range of characters changed
//						h.Free();
//						Marshal.Release(pBstr);
//					}
//				}
				
				// Display any error message to the user and then reset the returned "hr"
				// to NOERROR because the error has been handled.
//				if (FAILED(hr))
//					m_pCVsPkg->ReportErrorInfo(hr);
				return 0;
			}
			else if (FactGuidList.StandardCommandSet2K == pguidCmdGroup)
			{
//				bool fCompleteWord = false;

				// Resource IDs for command targets
//				public static const int ECMD_COMPLETEWORD = 107;
//				public static const int ECMD_SHOWMEMBERLIST = 108;
//				public static const int ECMD_TYPECHAR = 1;
//				public static const int ECMD_BACKSPACE = 2;
//				public static const int ECMD_RETURN = 3;
//				public static const int ECMD_TAB = 4;
//				public static const int ECMD_BACKTAB = 5;
//				public static const int ECMD_DELETE = 6;
//				public static const int ECMD_LEFT = 7;
//				public static const int ECMD_LEFT_EXT = 8;
//				public static const int ECMD_RIGHT = 9;
//				public static const int ECMD_RIGHT_EXT = 10;
//				public static const int ECMD_UP = 11;
//				public static const int ECMD_UP_EXT = 12;
//				public static const int ECMD_DOWN = 13;
//				public static const int ECMD_DOWN_EXT = 14;
//				public static const int ECMD_HOME = 15;
//				public static const int ECMD_HOME_EXT = 16;
//				public static const int ECMD_END = 17;
//				public static const int ECMD_END_EXT = 18;

				switch ((int)nCmdID)
				{
					case 140:
//						object testObj = Marshal.GetObjectForNativeVariant(pvaIn);						
						goto default;
//					case 107: //FactGuidList.ECMD_COMPLETEWORD:     // Ctrl-<space> completes a word (arrow,circle,rectangle)
//						fCompleteWord = true;
					// fall through
					case 108: //FactGuidList.ECMD_SHOWMEMBERLIST:   // Ctrl-j       drops the statement completion box           
						bool fShowDraw = true;
						bool fShowFigures = false;
						string cbstrText;

						hr = getCurrentLineText(out cbstrText);
//						if (hr > 0)
//						{
//							int nTokens = 0;
//							hr = getTokens(cbstrText, out nTokens);
//							if (hr > 0 && nTokens > 0)
//							{
//								// show 'draw' unless we have it already
//								if (FactTokenType.Object == myMarks[0].TokenType)
//								{
//									fShowDraw = false;
//
//									// show 'type' unless we have it already
//									if ((nTokens < 2) || (eType != m_pMks[1].eTokenType))
//										fShowFigures = true;
//								}
//							}
//						}

						if (!fShowDraw && !fShowFigures)
							break;

						// TODO: implement classes for AutoComplete/Intellisense
//						int dwFlags = 0;
//						m_srpCompletionSet->Reset(
//							/* [in]   BOOL   fDraw = FALSE   */ fShowDraw,
//							/* [out]  DWORD* pdwFlags = NULL */ &dwFlags);
//						if (fCompleteWord)
//							dwFlags = dwFlags | UCS_COMPLETEWORD;
//						hr = m_srpTextView->UpdateCompletionStatus(m_srpCompletionSet, dwFlags);
						break;

					case 3: //FactGuidList.ECMD_RETURN:           // dismiss method tip window if it's displayed
					case 2: //FactGuidList.ECMD_BACKSPACE:        // dismiss method tip window if it's displayed
//						if (myTipWinDisplayed)
//						{
//							hr = myTextView.UpdateTipWindow(
//											m_srpMethodTipWindow, UTW_DISMISS);
//							m_fTipWinDisplayed = FALSE;
//						}
						// pass this character 'cmd' on to next in chain (the view so it will show up)
						fHandled = false;
						hr = myNextCmdTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
						break;

					case 1: //FactGuidList.ECMD_TYPECHAR:         // any character
						fHandled = false;
						// pass this character 'cmd' on to next in chain (the view so it will show up)
						hr = myNextCmdTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);

						// UNDONE: I don't know what to do with pvaIn (for IntelliSense)
//							if (pvaIn[0].vt != VT_UI2)
//								break;
//							// did the user type a space after "draw" or "draw <type>" or is the 
//							// tip window still displayed ?
//							if (pvaIn[0].uiVal != ' ' && !m_fTipWinDisplayed)
//								break;

						int hrLocal = NativeMethods.S_OK;

						// get the current line where the cursor is
						int nLine = 0;
						int vc;
						hrLocal = myTextView.GetCaretPos(out nLine, out vc);
						if (hrLocal < 0)
							break;

						// get the number of chars on the line
						int nLineLength;
						hrLocal = myTextLines.GetLengthOfLine(nLine, out nLineLength);
						if (hrLocal < 0 || nLineLength == 0)
							break;

						// see if current line contains "draw" (starting at the beggining of the line)
						string bstrLine;
						hrLocal = myTextLines.GetLineText(
							nLine,          // starting line
							0,              // starting character index within the line (must be <= length of line)
							nLine,          // ending line
							nLineLength,    // ending character index within the line (must be <= length of line)
							out bstrLine);    // line text, if any
						if (hrLocal < 0 || bstrLine.Length == 0)
							break;

						// call the Line method on the parse object
						FactLine factLine = new FactLine(bstrLine);
						hrLocal = myParser.Line(ref factLine);

						if (hrLocal < 0)
						{
							// parse call failed
							break;
						}

						// TODO: for IntelliSense implementation example, see Figures example
						break;

					default:
						// pass this cmd on to next in chain (the view so it will show up)
						fHandled = false;
						hr = myNextCmdTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
						break;
				}

				// TODO: Parse the line to show red squigglies if no objects are on the line
//				int nLine = 0;
//				int vc;
//				hr = myTextView.GetCaretPos(out nLine, out vc);
//				hr = myTaskManager.CreateTask(nLine, myTextView);

				// if we handled the command return OK - we have reported any error
				// otherwise passed the result returned from the next command target
				if (fHandled)
					return 0;
				else
					return hr;
			}

			return myNextCmdTarget.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
		}

		int IOleCommandTarget.QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
		{
			return QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
		}
		/// <summary>
		/// Implements IOleCommandTarget.QueryStatus
		/// </summary>
		/// <param name="pguidCmdGroup"></param>
		/// <param name="cCmds"></param>
		/// <param name="prgCmds"></param>
		/// <param name="pCmdText"></param>
		/// <returns></returns>
		protected int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
		{
			if (typeof(ORMDesignerPackage).GUID == pguidCmdGroup)
			{
				// enable/disable only commands which differ for us from the default
				prgCmds[0].cmdf = (uint)(OLECMDF.OLECMDF_DEFHIDEONCTXTMENU | OLECMDF.OLECMDF_SUPPORTED | OLECMDF.OLECMDF_INVISIBLE);
				return NativeMethods.S_OK;
			}
			else if (FactGuidList.StandardCommandSet2K == pguidCmdGroup)
			{
				prgCmds[0].cmdf = (uint)(OLECMDF.OLECMDF_SUPPORTED | OLECMDF.OLECMDF_ENABLED);
				return NativeMethods.S_OK;
			}

			// pass this cmd on to next in chain (the view so it will show up)
			return myNextCmdTarget.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
		}

		#endregion

		#region IVsTextViewEvents Members

		void IVsTextViewEvents.OnChangeCaretLine(IVsTextView pView, int iNewLine, int iOldLine)
		{
			throw new NotImplementedException();
		}

		void IVsTextViewEvents.OnChangeScrollInfo(IVsTextView pView, int iBar, int iMinUnit, int iMaxUnits, int iVisibleUnits, int iFirstVisibleUnit)
		{
			throw new NotImplementedException();
		}

		void IVsTextViewEvents.OnKillFocus(IVsTextView pView)
		{
			throw new NotImplementedException();
		}

		void IVsTextViewEvents.OnSetBuffer(IVsTextView pView, IVsTextLines pBuffer)
		{
			throw new NotImplementedException();
		}

		void IVsTextViewEvents.OnSetFocus(IVsTextView pView)
		{
			throw new NotImplementedException();
		}

#endregion

		#region IVsTextViewFilter Members

		int IVsTextViewFilter.GetDataTipText(TextSpan[] pSpan, out string pbstrText)
		{
			return GetDataTipText(pSpan, out pbstrText);
		}
		/// <summary>
		/// Implements IVsTextViewFilter.GetDataTipText
		/// </summary>
		/// <param name="pSpan"></param>
		/// <param name="pbstrText"></param>
		/// <returns></returns>
		protected int GetDataTipText(TextSpan[] pSpan, out string pbstrText)
		{
			pbstrText = null;
			return NativeMethods.E_NOTIMPL;
		}

		int IVsTextViewFilter.GetPairExtents(int iLine, int iIndex, TextSpan[] pSpan)
		{
			return GetPairExtents(iLine, iIndex, pSpan);
		}
		/// <summary>
		/// Implements IVsTextViewFilter.GetPairExtents
		/// </summary>
		/// <param name="iLine"></param>
		/// <param name="iIndex"></param>
		/// <param name="pSpan"></param>
		/// <returns></returns>
		protected int GetPairExtents(int iLine, int iIndex, TextSpan[] pSpan)
		{
			return NativeMethods.E_NOTIMPL;
		}

		int IVsTextViewFilter.GetWordExtent(int iLine, int iIndex, uint dwFlags, TextSpan[] pSpan)
		{
			return GetWordExtent(iLine, iIndex, dwFlags, pSpan);
		}
		/// <summary>
		/// Implements IVsTextViewFilter.GetWordExtent
		/// </summary>
		/// <param name="iLine"></param>
		/// <param name="iIndex"></param>
		/// <param name="dwFlags"></param>
		/// <param name="pSpan"></param>
		/// <returns></returns>
		protected int GetWordExtent(int iLine, int iIndex, uint dwFlags, TextSpan[] pSpan)
		{
			return NativeMethods.E_NOTIMPL;
		}

#endregion

		#region Private Methods

		private int getCurrentLineText(out string pbstrLineText)
		{
			pbstrLineText = "";

			// get the current line where the cursor is
			int nLine = 0;
			int vc;
			int hr = myTextView.GetCaretPos(out nLine, out vc);
			if (hr < 0)
				return hr;

			// get the number of chars on the line
			int nLineLength;
			hr = myTextLines.GetLengthOfLine(nLine, out nLineLength);
			if (hr < 0 || nLineLength == 0)
				return hr;

			// see if current line contains "draw" (starting at the beggining of the line)
			hr = myTextLines.GetLineText(
				nLine,          // starting line
				0,              // starting character index within the line (must be <= length of line)
				nLine,          // ending line
				nLineLength,    // ending character index within the line (must be <= length of line)
				out pbstrLineText);    // line text, if any  
			return hr;
		}

		private int getTokens(string bstrLine, out int pnTokens)
		{
			pnTokens = 0;
			int nLineLength = bstrLine.Length;

			// call the Line method on the parse object
			FactLine factLine = new FactLine(bstrLine);
			int hr = myParser.Line(ref factLine);

			if (hr < 0)
				return hr;

			pnTokens = factLine.Marks.Count;

			return NativeMethods.S_OK;
		}

#endregion

		#region Properties

		/// <summary>
		/// Expse the text view so the window manager can remove command filters
		/// </summary>
		/// <value></value>
		public IVsTextView TextView
		{
			get { return myTextView; }
		}

		#endregion
	}
}
