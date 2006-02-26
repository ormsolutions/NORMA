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

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// A collection of static <see cref="Guid"/>s used in the <see cref="Microsoft.VisualStudio.OLE.Interop.IOleCommandTarget.Exec"/>
	/// <seealso cref="Neumont.Tools.ORM.FactEditor.FactTextViewFilter"/>
	/// </summary>
	public static class FactGuidList
	{
		/// <summary>
		/// The Standard command set.
		/// </summary>
		public static readonly Guid StandardCommandSet2K = Microsoft.VisualStudio.VSConstants.VSStd2K;
		
		/// <summary>
		/// The shell's text editor commands.
		/// </summary>
		public static readonly Guid CmdUIGuidTextEditor = new Guid(0x8B382828, 0x6202, 0x11d1, 0x88, 0x70, 0x00, 0x00, 0xF8, 0x75, 0x79, 0xD2);

		/// <summary>
		/// The <see cref="Guid"/> for the FactEditor tool window.
		/// </summary>
		public static readonly Guid FactEditorToolWindowGuid = new Guid(FactEditorToolWindowGuidString);
		/// <summary>
		/// The <see cref="String"/> form of the <see cref="FactEditorToolWindowGuid"/>.
		/// </summary>
		public const string FactEditorToolWindowGuidString = "63B6F84D-DF09-4E65-86EA-6BC1B856837B";
	}
}
