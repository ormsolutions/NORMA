using System;
using System.Collections.Generic;
using System.Text;

namespace ORMSolutions.ORMArchitect.CustomProperties
{
	/// <summary>
	/// Arguments for when a name has changed for either a definition or a group.
	/// </summary>
	public class NameChangedEventArgs : EventArgs
	{
		#region Fields
		private string _newName;

		#endregion
		#region Constructors
		public NameChangedEventArgs(string newName)
		{
			_newName = newName;
		}
		#endregion
		#region Properties
		/// <summary>
		/// The name after the change has occured.
		/// </summary>
		public string NewName
		{
			get
			{
				return _newName;
			}
		}
		#endregion
	}
}
