using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ORMSolutions.ORMArchitect.Core.ObjectModel;

namespace ORMSolutions.ORMArchitectSDK.TestEngine
{
	public partial struct Suite
	{
		private class ORMTaskProvider : IORMToolTaskProvider
		{
			#region TaskItems collection
			private Collection<IORMToolTaskItem> myTaskItems = new Collection<IORMToolTaskItem>();
			public Collection<IORMToolTaskItem> TaskItems
			{
				get { return myTaskItems; }
			}
			#endregion // TaskItems collection
			#region IORMToolTaskProvider Implementation
			void IORMToolTaskProvider.AddTask(IORMToolTaskItem task)
			{
				myTaskItems.Add(task);
			}
			IORMToolTaskItem IORMToolTaskProvider.CreateTask()
			{
				return new ORMTaskItem(this);
			}
			bool IORMToolTaskProvider.NavigateTo(IORMToolTaskItem task)
			{
				return false;
			}
			void IORMToolTaskProvider.RemoveAllTasks()
			{
				myTaskItems.Clear();
			}
			void IORMToolTaskProvider.RemoveTask(IORMToolTaskItem task)
			{
				myTaskItems.Remove(task);
			}
			#endregion //IORMToolTaskProvider Implementation
		}
	}
}
