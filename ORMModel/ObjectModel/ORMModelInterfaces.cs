#region Common Public License Copyright Notice
/**************************************************************************\
* Neumont Object-Role Modeling Architect for Visual Studio                 *
*                                                                          *
* Copyright © Neumont University. All rights reserved.                     *
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
using Neumont.Tools.ORM.Shell;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Design;
using Neumont.Tools.Modeling.Diagrams;

namespace Neumont.Tools.ORM.ObjectModel
{
	#region VerbalizationTarget enum
	/// <summary>
	/// Determines what target the Verbalization Set applies to
	/// </summary>
	[TypeConverter(typeof(EnumConverter<VerbalizationTarget, ORMModel>))]
	public enum VerbalizationTarget
	{
		/// <summary>
		/// Specifies the default verbalization snippet set. If a specific set is
		/// not available for any of the other values, then we use the default set.
		/// </summary>
		Default,
		/// <summary>
		/// Specifies the verbalization snippet set to be used with the ORM Verbalization Browser
		/// </summary>
		VerbalizationBrowser,
		/// <summary>
		/// Specifies the verbalization snippet set to be used with the Verbalization Reports
		/// </summary>
		Report,
	}
	#endregion // VerbalizationTarget enum
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
		/// Retrieve the service for registering and unregistering <see cref="ORMPropertyProvisioning"/>s.
		/// </summary>
		IORMPropertyProviderService PropertyProviderService { get;}
		/// <summary>
		/// Retrieve the context service provider. Can be null in some situations,
		/// such as when the model is being loaded outside the Visual Studio environment.
		/// </summary>
		IServiceProvider ServiceProvider { get;}
		/// <summary>
		/// Retrieve the VerbalizationSnippets dictionary for this store
		/// </summary>
		/// <param name="target">The type of dictionary to retrieve.</param>
		IDictionary<Type, IVerbalizationSets> GetVerbalizationSnippetsDictionary(VerbalizationTarget target);
		/// <summary>
		/// Retrieve the LayoutEngines dictionary for this store
		/// </summary>
		LayoutEngine GetLayoutEngine(Type engineType);
		/// <summary>
		/// Retrieve the INotifySurveyElmentChanged interface for this store
		/// </summary>
		INotifySurveyElementChanged NotifySurveyElementChanged { get;}
		/// <summary>
		/// Return true if a new transaction can be added at this time.
		/// This will return false if the store is currently in UndoRedoOrRollback,
		/// or if an external source sets this to false. For example, an UndoUnit implementation
		/// may set this property to false to signal other events that they should not
		/// begin a new transaction at this time.
		/// </summary>
		bool CanAddTransaction { get; set;}
		/// <summary>
		/// Activate the specified shape on the most appropriate view
		/// </summary>
		/// <param name="shape">A <see cref="ShapeElement"/> to activate.</param>
		/// <returns>Returns <see langword="true"/> if shape activation succeeded.</returns>
		bool ActivateShape(ShapeElement shape);
	}
	#endregion // IORMToolServices interface
	#region ORMPropertyProvisioning delegate
	/// <summary>
	/// Adds extension <see cref="PropertyDescriptor"/>s for the <see cref="IORMExtendableElement"/> specified
	/// by <paramref name="extendableElement"/> to the <see cref="PropertyDescriptorCollection"/> specified by
	/// <paramref name="properties"/>.
	/// </summary>
	public delegate void ORMPropertyProvisioning(IORMExtendableElement extendableElement, PropertyDescriptorCollection properties);
	#endregion // ORMPropertyProvisioning delegate
	#region IORMPropertyProviderService interface
	/// <summary>
	/// Provides methods for registrating and unregistrating <see cref="ORMPropertyProvisioning"/>s for
	/// <see cref="IORMExtendableElement"/>s.
	/// </summary>
	public interface IORMPropertyProviderService
	{
		/// <summary>
		/// Registers the <see cref="ORMPropertyProvisioning"/> specified by <paramref name="propertyProvisioning"/> for the
		/// type specified by <typeparamref name="TExtendableElement"/>.
		/// </summary>
		/// <typeparam name="TExtendableElement">
		/// The type for which the <see cref="ORMPropertyProvisioning"/> should be added. This type specified must
		/// by a subtype of <see cref="ModelElement"/> and implement <see cref="IORMExtendableElement"/>.
		/// </typeparam>
		/// <param name="propertyProvisioning">
		/// The <see cref="ORMPropertyProvisioning"/> being registered.
		/// </param>
		/// <param name="includeSubtypes">
		/// Specifies whether the <see cref="ORMPropertyProvisioning"/> should also be registered for subtypes of
		/// <typeparamref name="TExtendableElement"/>.
		/// </param>
		void RegisterPropertyProvider<TExtendableElement>(ORMPropertyProvisioning propertyProvisioning, bool includeSubtypes)
			where TExtendableElement : ModelElement, IORMExtendableElement;

		/// <summary>
		/// Unregisters the <see cref="ORMPropertyProvisioning"/> specified by <paramref name="propertyProvisioning"/> for the
		/// type specified by <typeparamref name="TExtendableElement"/>.
		/// </summary>
		/// <typeparam name="TExtendableElement">
		/// The type for which the <see cref="ORMPropertyProvisioning"/> should be removed. This type specified must
		/// by a subtype of <see cref="ModelElement"/> and implement <see cref="IORMExtendableElement"/>.
		/// </typeparam>
		/// <param name="propertyProvisioning">
		/// The <see cref="ORMPropertyProvisioning"/> being unregistered.
		/// </param>
		/// <param name="includeSubtypes">
		/// Specifies whether the <see cref="ORMPropertyProvisioning"/> should also be unregistered for subtypes of
		/// <typeparamref name="TExtendableElement"/>.
		/// </param>
		void UnregisterPropertyProvider<TExtendableElement>(ORMPropertyProvisioning propertyProvisioning, bool includeSubtypes)
			where TExtendableElement : ModelElement, IORMExtendableElement;

		/// <summary>
		/// Adds extension <see cref="PropertyDescriptor"/>s for the <see cref="IORMExtendableElement"/> specified
		/// by <paramref name="extendableElement"/> to the <see cref="PropertyDescriptorCollection"/> specified by
		/// <paramref name="properties"/>.
		/// </summary>
		void GetProvidedProperties(IORMExtendableElement extendableElement, PropertyDescriptorCollection properties);
	}
	#endregion // IORMPropertyProviderService interface
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
