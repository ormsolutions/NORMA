using System;

namespace Northface.Tools.ORM.ObjectModel
{
	public abstract partial class ModelError
	{
		private object myTaskData;
		/// <summary>
		/// Non-IMS managed slot for storing error reporting
		/// data. Allows items to be easily removed from the task list.
		/// </summary>
		public object TaskData
		{
			get { return myTaskData; }
			set { myTaskData = value; }
		}
		/// <summary>
		/// Called to set the name of the error.
		/// </summary>
		public abstract void GenerateErrorText();
	}
}