#region Using directives

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TextManager.Interop;
using Northface.Tools.ORM.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using System.Runtime.InteropServices;

#endregion

namespace Northface.Tools.ORM.FactEditor
{
	#region FactLineType enum

	// UNDONE: Determine if the FactLineType is necessary
	/// <summary>
	/// The type of fact. Currently this is unused. It could be used to determine
	/// if there is an error on the line, e.g. No objects exist in the fact. Another
	/// possible type could indicate if the fact is objectified
	/// </summary>
	public enum FactLineType
	{
		/// <summary>
		/// The fact contains no object types
		/// </summary>
		ContainsObject = 0,
		/// <summary>
		/// The fact is in an invalid state
		/// </summary>
		Invalid
	}

	#endregion

	#region FactTokenType enum

	/// <summary>
	/// Valid token types for a fact
	/// </summary>
	public enum FactTokenType
	{
		/// <summary>
		/// Unknown token
		/// </summary>
		Unknown = 1,
		/// <summary>
		///  Errors
		/// </summary>
		Error,
		/// <summary>
		/// Object type
		/// </summary>
		EntityType,
		/// <summary>
		/// Object type
		/// </summary>
		ValueType,
		/// <summary>
		/// Predicate reading
		/// </summary>
		Predicate,
		/// <summary>
		/// Reference Mode
		/// </summary>
		ReferenceMode,
		/// <summary>
		/// Parenthesis
		/// </summary>
		Parenthesis,
	}
	

	#endregion

	#region FactTokenMark struct

	/// <summary>
	/// a token to be marked
	/// </summary>
	public struct FactTokenMark
	{
		/// <summary>
		/// What type of token it is, e.g. Object, Predicate
		/// </summary>
		public FactTokenType TokenType;
		/// <summary>
		/// The starting index in the string for this token
		/// </summary>
		public int nStart;
		/// <summary>
		/// The ending index in the string for this token
		/// </summary>
		public int nEnd;
	}


	#endregion

	/// <summary>
	/// Language service to provide colorization
	/// </summary>
	[Guid("C50CD300-9D1E-4AB0-B494-73FA23D14D2B")]
	public partial class FactLanguageService : IVsLanguageInfo
	{
		private ORMDesignerPackage myPackage;
		private System.IServiceProvider vsIServiceProvider;
		private FactParser myParser;

		/// <summary>
		/// Construct a language service for the ORM package
		/// </summary>
		/// <param name="package"></param>
		public FactLanguageService(ORMDesignerPackage package)
		{
			myPackage = package;
			vsIServiceProvider = package;
		}

		void InitFactParser()
		{
			if (myParser == null)
			{
				myParser = new FactParser();
			}
		}

		#region IVsLanguageInfo Members

		int IVsLanguageInfo.GetCodeWindowManager(IVsCodeWindow pCodeWin, out IVsCodeWindowManager ppCodeWinMgr)
		{
			return GetCodeWindowManager(pCodeWin, out ppCodeWinMgr);
		}
		/// <summary>
		/// Implements IVsLanguageInfo.GetCodeWindowManager
		/// </summary>
		/// <param name="pCodeWin"></param>
		/// <param name="ppCodeWinMgr"></param>
		/// <returns></returns>
		protected int GetCodeWindowManager(IVsCodeWindow pCodeWin, out IVsCodeWindowManager ppCodeWinMgr)
		{
			ppCodeWinMgr = new FactCodeWindowManager(myPackage, pCodeWin);
			return NativeMethods.S_OK;
		}

		int IVsLanguageInfo.GetColorizer(IVsTextLines pBuffer, out IVsColorizer ppColorizer)
		{
			return GetColorizer(pBuffer, out ppColorizer);
		}
		/// <summary>
		/// Implements IVsLanguageInfo.GetColorizer
		/// </summary>
		/// <param name="pBuffer"></param>
		/// <param name="ppColorizer"></param>
		/// <returns></returns>
		protected int GetColorizer(IVsTextLines pBuffer, out IVsColorizer ppColorizer)
		{
			InitFactParser();
			ppColorizer = new FactColorizer(myParser, pBuffer);
			return NativeMethods.S_OK;
		}

		int IVsLanguageInfo.GetFileExtensions(out string pbstrExtensions)
		{
			return GetFileExtensions(out pbstrExtensions);
		}
		/// <summary>
		/// Implements IVsLanguageInfo.GetFileExtensions
		/// </summary>
		/// <param name="pbstrExtensions"></param>
		/// <returns></returns>
		protected int GetFileExtensions(out string pbstrExtensions)
		{
			// TODO: change hard-coded file extension
			pbstrExtensions = ".fct";
			return NativeMethods.S_OK;
		}

		int IVsLanguageInfo.GetLanguageName(out string bstrName)
		{
			return GetLanguageName(out bstrName);
		}
		/// <summary>
		/// Implements IVsLanguageInfo.GetLanguageName
		/// </summary>
		/// <param name="bstrName"></param>
		/// <returns></returns>
		protected int GetLanguageName(out string bstrName)
		{
			// TODO: get a localized string for the language name
			bstrName = "ORM Fact Editor";
			return NativeMethods.S_OK;
		}

		#endregion
	}
}
