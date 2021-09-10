#region Common Public License Copyright Notice
/**************************************************************************\
* Natural Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright � Neumont University. All rights reserved.                     *
* Copyright � ORM Solutions, LLC. All rights reserved.                        *
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Core.Shell;
using ORMSolutions.ORMArchitect.Framework;
using ORMSolutions.ORMArchitect.Framework.Design;
using ORMSolutions.ORMArchitect.Framework.Diagrams;

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
{
	#region IORMToolServices interface
	/// <summary>
	/// Specify which window <see cref="IORMToolServices.NavigateTo(object, NavigateToWindow)"/>
	/// or <see cref="IORMToolServices.ActivateShape"/> should jump to.
	/// </summary>
	public enum NavigateToWindow
	{
		/// <summary>
		/// Select the shape in the most appropriate view
		/// for the primary document window. If no corresponding
		/// shape is available, then attempt to select the item in
		/// the model browser.
		/// </summary>
		Document,
		/// <summary>
		/// Select the shape in the diagram spy window. If no
		/// corresponding shape is available, then attempt to
		/// select the item in the model browser.
		/// </summary>
		DiagramSpy,
		/// <summary>
		/// Select the specified element in the model browser
		/// </summary>
		ModelBrowser,
	}
	/// <summary>
	/// Specify options for <see cref="IORMToolServices.NavigateTo(object, NavigateToWindow,NavigateToOptions)"/>
	/// </summary>
	[Flags]
	public enum NavigateToOptions
	{
		/// <summary>
		/// Use default behavior
		/// </summary>
		None = 0,
		/// <summary>
		/// Do not resolve <see cref="IRepresentModelElements"/> for the target object.
		/// </summary>
		IgnoreRepresentedElements = 1,
	}
	/// <summary>
	/// An interface that should be implemented by any
	/// store that hosts ORM-derived object models. This
	/// can be implemented via pass-through to the host document,
	/// or directly on a store for non-VS loading and validation
	/// of the object model.
	/// </summary>
	public interface IORMToolServices : IFrameworkServices
	{
		/// <summary>
		/// Retrieve the service for adding and removing tasks
		/// </summary>
		IORMToolTaskProvider TaskProvider { get; }
		/// <summary>
		/// Retrieve the service for getting current font and color information
		/// </summary>
		IORMFontAndColorService FontAndColorService { get; }
		/// <summary>
		/// Retrieve the <see cref="IORMModelErrorActivationService">service</see> for managing model error activation.
		/// </summary>
		IORMModelErrorActivationService ModelErrorActivationService { get; }
		/// <summary>
		/// Return the <see cref="IORMExtendableElementService">service</see> for managing extension elements.
		/// </summary>
		IORMExtendableElementService ExtendableElementService { get; }
		/// <summary>
		/// Retrieve the context service provider. Can be null in some situations,
		/// such as when the model is being loaded outside the Visual Studio environment.
		/// </summary>
		IServiceProvider ServiceProvider { get; }
		/// <summary>
		/// Retrieve the VerbalizationSnippets dictionary for this store
		/// </summary>
		/// <param name="target">The type of dictionary to retrieve.</param>
		IDictionary<Type, IVerbalizationSets> GetVerbalizationSnippetsDictionary(string target);
		/// <summary>
		/// Get the current verbalization targets dictionary, which contains information about all verbalization targets
		/// supported by loaded domain models.
		/// </summary>
		IDictionary<string, VerbalizationTargetData> VerbalizationTargets { get; }
		/// <summary>
		/// Get a service to all extension models to inject verbalized child
		/// elements into the standard verbalization.
		/// </summary>
		IExtensionVerbalizerService ExtensionVerbalizerService { get; }
		/// <summary>
		/// Get a dictionary with named values for verbalization options supported
		/// by loaded domain models.
		/// </summary>
		IDictionary<string, object> VerbalizationOptions { get; }
		/// <summary>
		/// Retrieve the LayoutEngines dictionary for this store
		/// </summary>
		LayoutEngine GetLayoutEngine(Type engineType);
		/// <summary>
		/// Return true if a new transaction can be added at this time.
		/// This will return false if the store is currently in UndoRedoOrRollback,
		/// or if an external source sets this to false. For example, an UndoUnit implementation
		/// may set this property to false to signal other events that they should not
		/// begin a new transaction at this time.
		/// </summary>
		bool CanAddTransaction { get; set; }
		/// <summary>
		/// Return true if events are being processed for a visible transaction item.
		/// A <see cref="TransactionItem"/> is visible if it contains changes to the primary
		/// document <see cref="Store"/>. Visible transaction items can be seen in
		/// the Undo and Redo lists.
		/// </summary>
		/// <remarks>By default, the answer to this question is automatically calculated
		/// based on the currently processing transaction. However, the answer cannot
		/// be accurately calculated based on context information if the <see cref="Store"/>
		/// is not in an Undo or Redo state. To override the default processing, an UndoUnit
		/// implementation must explicitly set the property twice. The first call sets the
		/// expected return value; the second call must be the opposite boolean value.</remarks>
		bool ProcessingVisibleTransactionItemEvents { get; set; }
		/// <summary>
		/// Activate the specified shape on the most appropriate view
		/// </summary>
		/// <param name="shape">A <see cref="ShapeElement"/> to activate.</param>
		/// <param name="window">The type of window to select the shape in.</param>
		/// <returns>Returns <see langword="true"/> if shape activation succeeded.</returns>
		bool ActivateShape(ShapeElement shape, NavigateToWindow window);
		/// <summary>
		/// Find the most convenient activation target for specified element
		/// </summary>
		/// <param name="target">The element to activate. Must either be a 
		/// <see cref="ModelElement"/> or implement <see cref="IRepresentModelElements"/>.</param>
		/// <param name="window">The type of window to select the element in.</param>
		/// <returns>Returns <see langword="true"/> if activate succeeds</returns>
		bool NavigateTo(object target, NavigateToWindow window);
		/// <summary>
		/// Find the most convenient activation target for specified element, with additional options.
		/// </summary>
		/// <param name="target">The element to activate. Must either be a 
		/// <see cref="ModelElement"/> or implement <see cref="IRepresentModelElements"/>.</param>
		/// <param name="window">The type of window to select the element in.</param>
		/// <param name="options">Specify additional options for navigation behavior.</param>
		/// <returns>Returns <see langword="true"/> if activate succeeds</returns>
		bool NavigateTo(object target, NavigateToWindow window, NavigateToOptions options);
	}
	#endregion // IORMToolServices interface
	#region IShapeFreeDataObjectProvider interface
	/// <summary>
	/// Implement on a domain model, returning the types of elements
	/// that do not have shapes but can be dropped on a diagram in a
	/// different model file to merge them into the target model.
	/// Returned types must derive from <see cref="ModelElement"/>.
	/// </summary>
	/// <remarks>A data object that includes a shape-free instance must also
	/// add a value for the type of <see cref="ShapeFreeDataObjectSourceStore"/>
	/// to the data object, with the source store as the data value.</remarks>
	public interface IShapeFreeDataObjectProvider
	{
		/// <summary>
		/// Return the list of types in this model that support shape-free drag drop
		/// into an external model.
		/// </summary>
		Type[] ShapeFreeDataObjectTypes { get; }
	}
	#endregion // IShapeFreeDataObjectProvider interface
	#region ShapeFreeDataObjectSourceStore class
	/// <summary>
	/// The type of this class is used as a signal that a drag of a shape-free
	/// data object is being requested. The registered value should be the
	/// <see cref="ModelElement.Store"/> property for the dragged element. This
	/// allows for an easy check to determine if a shape-free item is available
	/// and will block it from being dropped in the source store, which is meaningless
	/// for a shape-free object.
	/// </summary>
	public static class ShapeFreeDataObjectSourceStore { }
	#endregion // ShapeFreeDataObjectSourceStore class
	#region ORMModelErrorActivation delegate
	/// <summary>
	/// Activate the provided <paramref name="error"/> using the provided <paramref name="services"/>.
	/// Used with the <see cref="IORMModelErrorActivationService"/> interface.
	/// </summary>
	/// <param name="services">The context <see cref="IORMToolServices"/></param>
	/// <param name="selectedElement">The currently selected element</param>
	/// <param name="error">The model error to activate</param>
	/// <returns><see langword="true"/> if the activation succeeded.</returns>
	public delegate bool ORMModelErrorActivator(IORMToolServices services, ModelElement selectedElement, ModelError error);
	#endregion // ORMModelErrorActivation delegate
	#region IORMModelErrorActivationService
	/// <summary>
	/// Manage delegates for model error activation. IORMModelErrorActivateService
	/// provides shared activation for different selections of the same element.
	/// Explicit activation on shape representations should be done by implementing
	/// the <see cref="IModelErrorActivation"/> interface on the shape, not by registering
	/// a <see cref="ORMModelErrorActivator"/> with this interface.
	/// </summary>
	public interface IORMModelErrorActivationService
	{
		/// <summary>
		/// Activate a specific <paramref name="error"/> for a given <paramref name="selectedElement"/>
		/// </summary>
		/// <param name="selectedElement">An element that has already been selected in the UI.</param>
		/// <param name="error">The <see cref="ModelError"/> instance to activate</param>
		/// <returns><see langword="true"/> if the activation succeeded.</returns>
		bool ActivateError(ModelElement selectedElement, ModelError error);
		/// <summary>
		/// Register a handler for a specific type of error. Error activators should be registered
		/// during initial DocumentLoaded call (DocumentReloading is not set) to
		/// <see cref="IModelingEventSubscriber.ManageModelingEventHandlers"/>
		/// </summary>
		/// <param name="elementType">The type of element to handle</param>
		/// <param name="registerDerivedTypes">Specify <see langword="true"/> to register derived types in addition to the specified type.
		/// The final registration for a given type wins, so you can explicitly override some derived types even if this is <see langword="true"/></param>
		/// <param name="activator">The delegate callback used for error activation.</param>
		void RegisterErrorActivator(Type elementType, bool registerDerivedTypes, ORMModelErrorActivator activator);
	}
	#endregion // IORMModelErrorActivationService
	#region ISelectionContainerFilter interface
	/// <summary>
	/// Implement this interface to dynamically stop a selectable
	/// object from appearing in the diagrams selection container
	/// list of all objects. The most prominent place this appears
	/// is in the properties window
	/// </summary>
	public interface ISelectionContainerFilter
	{
		/// <summary>
		/// Return false from IncludeInSelectionContainer to allow
		/// an item to be selectable without appearing in the
		/// list of all available objects
		/// </summary>
		bool IncludeInSelectionContainer { get;}
	}
	#endregion // ISelectionContainerFilter interface
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
