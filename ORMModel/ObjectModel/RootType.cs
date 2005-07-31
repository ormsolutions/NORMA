using System;
using System.Collections;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;

namespace Neumont.Tools.ORM.ObjectModel
{
	public partial class RootType
	{
		/// <summary>
		/// Return a simple name instead of a name decorated with the type (the
		/// default for a ModelElement). This is the easiest way to display
		/// clean names in the property grid when we reference properties.
		/// </summary>
		public override string ToString()
		{
			return Name;
		}
	}
}
