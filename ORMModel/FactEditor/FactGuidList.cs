#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Northface.Tools.ORM.Shell
{
	/// <summary>
	/// A collection of static GUIDs used in the IOleCommandTarget.Exec
	/// see FactTextViewFilter
	/// </summary>
	public class FactGuidList
	{
		/// <summary>
		/// The Standard command set
		/// </summary>
		public static readonly Guid StandardCommandSet2K = new Guid(0x1496A755, 0x94DE, 0x11D0, 0x8C, 0x3F, 0x00, 0xC0, 0x4F, 0xC2, 0xAA, 0xE2);
		
		/// <summary>
		/// The shell's text editor commands
		/// </summary>
		public static readonly Guid CmdUIGuidTextEditor = new Guid(0x8B382828, 0x6202, 0x11d1, 0x88, 0x70, 0x00, 0x00, 0xF8, 0x75, 0x79, 0xD2);
	}
}
