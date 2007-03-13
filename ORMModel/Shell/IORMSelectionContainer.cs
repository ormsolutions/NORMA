using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Shell.Interop;
using Neumont.Tools.ORM.ObjectModel;

namespace Neumont.Tools.ORM.Shell
{
	/// <summary>
	/// An interface used to mark a window as being a selection container for
	/// elements in an <see cref="ORMModel"/>. It is expected that these classes will already
	/// implement <see cref="ISelectionContainer"/> and <see cref="ISelectionService"/>, so
	/// there should be no additional implementation required to use this interface.
	/// </summary>
	[CLSCompliant(false)]
	public interface IORMSelectionContainer : ISelectionContainer, ISelectionService
	{
	}
}
