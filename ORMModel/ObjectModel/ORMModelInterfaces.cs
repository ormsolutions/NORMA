using System;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Northface.Tools.ORM.Shell;

namespace Northface.Tools.ORM.ObjectModel
{
	#region IORMToolServices interface
	/// <summary>
	/// An interface that should be implemented by any
	/// store that hosts ORM-derived object models. This
	/// can be implemented via pass-through to the host document,
	/// or directly on a store for non-VS loading and validation
	/// of the object model.
	/// </summary>
	public interface IORMToolServices
	{
		/// <summary>
		/// Retrieve the service for adding and removing tasks
		/// </summary>
		IORMToolTaskProvider TaskProvider { get;}
		/// <summary>
		/// Retrieve the service for getting current font and color information
		/// </summary>
		IORMFontAndColorService FontAndColorService { get;}
		/// <summary>
		/// Retrieve the context service provider. Can be null in some situations,
		/// such as when the model is being loaded outside the Visual Studio environment.
		/// </summary>
		IServiceProvider ServiceProvider { get;}
	}
	#endregion // IORMToolServices interface
	#region IRepresentedModelElements interface
	/// <summary>
	/// Retrieve the ModelElement associated with object. Implemented
	/// on objects that are directly associated with task items.
	/// </summary>
	public interface IRepresentModelElements
	{
		/// <summary>
		/// Retrieve the model element associated with this item.
		/// The retrieved item should have a shape associated with
		/// it that can be selected on an ORM diagram. If a shape is
		/// not available, then aggregating parent chain is used to
		/// find a shape that is available.
		/// </summary>
		ModelElement[] GetRepresentedElements();
	}
	#endregion // IRepresentedModelElements interface
	#region IORMToolTaskItem interface
	/// <summary>
	/// An item that can be added to a task provider. At design time,
	/// this item will appear in the VS task list. When the model is
	/// loaded outside the shell, task items will be routed through a
	/// different mechanism, such as build-time error output. An object
	/// implementing this interface can be retrieved via the CreateTask
	/// method on the IORMToolTaskProvider interface.
	/// </summary>
	public interface IORMToolTaskItem
	{
		/// <summary>
		/// Set this property to support jumping to an
		/// element that is associated with an error object.
		/// Generally, task list items are linked to objects that
		/// are related to the actual elements with the reported
		/// anomaly, so custom code is need to navigate from the
		/// error object to the object with the actual problem.
		/// </summary>
		IRepresentModelElements ElementLocator { get; set; }
		/// <summary>
		/// The text for the displayed task item
		/// </summary>
		string Text { get; set; }
		// UNDONE: This interface should be extended to allow
		// more options.
	}
	#endregion // IORMToolTaskItem interface
	#region IORMToolTaskProvider interface
	/// <summary>
	/// A service interface used for creating, adding, and removing
	/// tasks.
	/// </summary>
	public interface IORMToolTaskProvider
	{
		/// <summary>
		/// Create a new task. The task will not be added until
		/// AddTask is called explicitly. This gives the user the
		/// chance to initialize tasks before they are displayed
		/// </summary>
		/// <returns></returns>
		IORMToolTaskItem CreateTask();
		/// <summary>
		/// Add a task to the task list. Added tasks must be created
		/// with the CreateTask method.
		/// </summary>
		/// <param name="task">A task created with CreateTask</param>
		void AddTask(IORMToolTaskItem task);
		/// <summary>
		/// Remove a task. The task should have been previously
		/// added with the AddTask method.
		/// </summary>
		/// <param name="task">A previously added task</param>
		void RemoveTask(IORMToolTaskItem task);
		/// <summary>
		/// Remove all task items
		/// </summary>
		void RemoveAllTasks();
		/// <summary>
		/// Navigate to the given task. Navigable tasks
		/// must have been created by CreateTask.
		/// </summary>
		/// <param name="task">The task to activate and locate</param>
		/// <returns>true if navigation successful</returns>
		bool NavigateTo(IORMToolTaskItem task);
	}
	#endregion // IORMToolTaskProvider interface
}