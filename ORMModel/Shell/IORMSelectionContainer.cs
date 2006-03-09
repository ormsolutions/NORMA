using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Shell.Interop;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// An interface used to mark a window as being a selection container for
	/// elements in an ORM Model. It is expected that these classes will already
	/// inherit from ISelectionContainer, so there should be no additional implementation
	/// required to use this interface.
	/// </summary>
	[CLSCompliant(false)]
	public interface IORMSelectionContainer : ISelectionContainer { }
}
