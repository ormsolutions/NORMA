using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Neumont.Tools.ORM.ObjectModel;

namespace ORM2CommandLineTest
{
	public class ORMTaskProvider : IORMToolTaskProvider
	{
		Collection<IORMToolTaskItem> taskItems = new Collection<IORMToolTaskItem>();

		public Collection<IORMToolTaskItem> TaskItems
		{
			get { return taskItems; }
		}

		public ORMTaskProvider()
		{

		}
		#region IORMToolTaskProvider Members

		/// <summary>
		/// Implements IORMToolTaskProvider.AddTask
		/// </summary>
		/// <param name="task">IORMToolTaskItem created by CreateTask</param>
		protected void AddTask(IORMToolTaskItem task)
		{
			taskItems.Add(task);
		}
		void IORMToolTaskProvider.AddTask(IORMToolTaskItem task)
		{
			AddTask(task);
		}

		/// <summary>
		/// Implements IORMToolTaskProvider.CreateTask
		/// </summary>
		/// <returns>IORMToolTaskItem</returns>
		protected IORMToolTaskItem CreateTask()
		{
			ORMTaskItem task = new ORMTaskItem(this);
			return task;
		}
		IORMToolTaskItem IORMToolTaskProvider.CreateTask()
		{
			return CreateTask();
		}

		/// <summary>
		/// Implements IORMToolTaskProvider.NavigateTo;
		/// </summary>
		/// <param name="task"></param>
		/// <returns></returns>
		protected bool NavigateTo(IORMToolTaskItem task)
		{
			return false;		
		}
		bool IORMToolTaskProvider.NavigateTo(IORMToolTaskItem task)
		{
			return NavigateTo(task);
		}

		/// <summary>
		/// Implements IORMToolTaskProvider.RemoveAllTasks
		/// </summary>
		protected void RemoveAllTasks()
		{
			taskItems.Clear();
		}
		void IORMToolTaskProvider.RemoveAllTasks()
		{
			RemoveAllTasks();
		}

		/// <summary>
		/// Implements IORMToolTaskProvider.RemoveTask
		/// </summary>
		/// <param name="task">IORMToolTaskItem previously added by AddTask</param>
		protected void RemoveTask(IORMToolTaskItem task)
		{
			taskItems.Remove(task);
		}
		void IORMToolTaskProvider.RemoveTask(IORMToolTaskItem task)
		{
			RemoveTask(task);
		}

		#endregion //IORMToolTaskProvider Members
	}

}
