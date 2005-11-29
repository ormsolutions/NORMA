using System;
using System.Collections;
using System.Diagnostics;
using Microsoft.VisualStudio.EnterpriseTools.Shell;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Modeling.Shell;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;

namespace Neumont.Tools.ORM.Shell
{
	public partial class ORMDesignerDocData : IORMToolServices
	{
		#region Store services passthrough
		/// <summary>
		/// Create a store that implements IORMToolServices by deferring
		/// all methods to this document.
		/// </summary>
		/// <param name="storeKey">The store to create. Expect PrimaryStoreKey</param>
		/// <returns></returns>
		protected override Store CreateStore(object storeKey)
		{
			Debug.Assert(storeKey == ModelingDocData.PrimaryStoreKey);
			// UNDONE: (MCurland) The base implementation of this method was supposed
			// to do nothing but create the store, so that derived documents could
			// extend it without breaking code. However, the current implementation
			// does other stuff. This needs to be fixed on the framework side.
			return new ORMStore(this, ServiceProvider);
		}
		/// <summary>
		/// A store implementation that defers all services to the
		/// owning document.
		/// </summary>
		protected class ORMStore : Store, IORMToolServices
		{
			#region Member Variables
			private IORMToolServices myServices;
			#endregion // Member Variables
			#region Constructors
			/// <summary>
			/// Create a new store
			/// </summary>
			/// <param name="services">IORMToolServices to defer to</param>
			/// <param name="serviceProvider">Global service provider</param>
			public ORMStore(IORMToolServices services, IServiceProvider serviceProvider) : base(serviceProvider)
			{
				myServices = services;
			}
			#endregion // Constructors
			#region IORMToolServices Implementation
			/// <summary>
			/// Defer to TaskProvider on the document. Implements
			/// IORMToolServices.TaskProvider
			/// </summary>
			protected IORMToolTaskProvider TaskProvider
			{
				get
				{
					return myServices.TaskProvider;
				}
			}
			IORMToolTaskProvider IORMToolServices.TaskProvider
			{
				get
				{
					return TaskProvider;
				}
			}
			/// <summary>
			/// Defer to ColorService on the document. Implements
			/// IORMToolServices.ColorService
			/// </summary>
			protected IORMFontAndColorService FontAndColorService
			{
				get
				{
					return myServices.FontAndColorService;
				}
			}
			IORMFontAndColorService IORMToolServices.FontAndColorService
			{
				get
				{
					return FontAndColorService;
				}
			}
			/// <summary>
			/// Defer to ServiceProvider on the document. Implements
			/// IORMToolServices.ServiceProvider
			/// </summary>
			protected IServiceProvider ServiceProvider
			{
				get
				{
					return myServices.ServiceProvider;
				}
			}
			IServiceProvider IORMToolServices.ServiceProvider
			{
				get
				{
					return ServiceProvider;
				}
			}
			#endregion // IORMToolServices Implementation
		}
		#endregion // Store services passthrough
		#region IORMToolServices Implementation
		private IORMToolTaskProvider myTaskProvider;
		/// <summary>
		/// Retrieve the task provider for this document. Created
		/// on demand using the CreateTaskProvider method. Implements
		/// IORMToolServices.TaskProvider.
		/// </summary>
		protected IORMToolTaskProvider TaskProvider
		{
			get
			{
				IORMToolTaskProvider provider = myTaskProvider;
				if (provider == null)
				{
					myTaskProvider = provider = CreateTaskProvider();
				}
				return provider;
			}
		}
		IORMToolTaskProvider IORMToolServices.TaskProvider
		{
			get
			{
				return TaskProvider;
			}
		}
		/// <summary>
		/// Get the color service for this document Defers to
		/// the packages color service. Implements
		/// IORMToolServices.ColorService
		/// </summary>
		protected static IORMFontAndColorService FontAndColorService
		{
			get
			{
				return ORMDesignerPackage.FontAndColorService;
			}
		}
		IORMFontAndColorService IORMToolServices.FontAndColorService
		{
			get
			{
				return FontAndColorService;
			}
		}
		IServiceProvider IORMToolServices.ServiceProvider
		{
			get
			{
				return ServiceProvider;
			}
		}
		#endregion // IORMToolServices Implementation
		#region TaskProvider implementation
		/// <summary>
		/// Default implementation of a task provider
		/// </summary>
		protected class ORMTaskProvider : TaskProvider, IORMToolTaskProvider
		{
			#region Member Variables
			ORMDesignerDocData myDocument;
			#endregion //Member Variables
			#region Constructors
			/// <summary>
			/// Create a task provider for this document
			/// </summary>
			/// <param name="document"></param>
			public ORMTaskProvider(ORMDesignerDocData document) : base(document.ServiceProvider)
			{
				Debug.Assert(document.myTaskProvider == null); // Only need one
				myDocument = document;
			}
			#endregion // Constructors
			#region IORMToolTaskProvider Implementation
			/// <summary>
			/// Implements IORMToolTaskProvider.CreateTask
			/// </summary>
			/// <returns>IORMToolTaskItem</returns>
			protected IORMToolTaskItem CreateTask()
			{
				ORMTaskItem task = new ORMTaskItem(this);
				task.CanDelete = false;
				task.Checked = false;
				task.IsCheckedEditable = false;
				task.IsPriorityEditable = false;
				task.IsTextEditable = false;
				task.Line = -1;
				task.SubcategoryIndex = 0;
				// Give this a high priority so it shows up as an error.
				// Normal priority appears as a warning.
				task.Priority = TaskPriority.High;
				// Make this show in the Error list using this category
				task.Category = TaskCategory.BuildCompile;
				task.Document = myDocument.FileName;
				return task;
			}
			IORMToolTaskItem IORMToolTaskProvider.CreateTask()
			{
				return CreateTask();
			}

			/// <summary>
			/// Implements IORMToolTaskProvider.AddTask
			/// </summary>
			/// <param name="task">IORMToolTaskItem created by CreateTask</param>
			protected void AddTask(IORMToolTaskItem task)
			{
				Tasks.Add((Task)task);
			}
			void IORMToolTaskProvider.AddTask(IORMToolTaskItem task)
			{
				AddTask(task);
			}

			/// <summary>
			/// Implements IORMToolTaskProvider.RemoveTask
			/// </summary>
			/// <param name="task">IORMToolTaskItem previously added by AddTask</param>
			protected void RemoveTask(IORMToolTaskItem task)
			{
				Tasks.Remove((Task)task);
			}
			void IORMToolTaskProvider.RemoveTask(IORMToolTaskItem task)
			{
				RemoveTask(task);
			}

			/// <summary>
			/// Implements IORMToolTaskProvider.RemoveAllTasks
			/// </summary>
			protected void RemoveAllTasks()
			{
				Tasks.Clear();
			}
			void IORMToolTaskProvider.RemoveAllTasks()
			{
				RemoveAllTasks();
			}
			/// <summary>
			/// Implements IORMToolTaskProvider.NavigateTo;
			/// </summary>
			/// <param name="task"></param>
			/// <returns></returns>
			protected static bool NavigateTo(IORMToolTaskItem task)
			{
				ModelElementLocator elementLocator = ORMDesignerDocView.ElementLocator;
				if (elementLocator == null)
				{
					return false;
				}
				ModelElement element = null;
				IRepresentModelElements locator = task.ElementLocator;
				if (locator != null)
				{
					ModelElement[] elements = locator.GetRepresentedElements();
					if (elements != null && elements.Length != 0)
					{
						// UNDONE: Support selection of multiple elements
						element = elements[0];
					}
				}
				ModelElement startElement = element;
				IProxyDisplayProvider proxyProvider = null;
				bool useProxy = false;
				// UNDONE: Move navigation code from here down into 
				// docdata and docview classes so we can use it elsewhere
				while (element != null)
				{
					ModelElement selectElement = element;
					if (useProxy)
					{
						// Second pass, we were unable to find a suitable shape for the first
						selectElement = proxyProvider.ElementDisplayedAs(element);
						if (selectElement != null && object.ReferenceEquals(selectElement, element))
						{
							selectElement = null;
						}
					}

					// UNDONE: We should potentially be creating a shape
					// so we can jump to any error
					if (selectElement != null)
					{
						bool continueNow = false;
						foreach (PresentationElement pel in selectElement.AssociatedPresentationElements)
						{
							ShapeElement shape = pel as ShapeElement;
							if (shape != null)
							{
								if (proxyProvider == null)
								{
									proxyProvider = shape as IProxyDisplayProvider;
								}
								if (element is ORMModel && !useProxy && !object.ReferenceEquals(element, startElement))
								{
									if (proxyProvider != null)
									{
										useProxy = true;
										element = startElement;
										continueNow = true;
										break;
									}
								}
								// Select the shape
								bool retVal = elementLocator.NavigateTo(Guid.Empty, shape);
								if (retVal)
								{
									ModelError error;
									IModelErrorActivation activator;
									if (null != (error = locator as ModelError) &
										null != (activator = shape as IModelErrorActivation))
									{
										activator.ActivateModelError(error);
									}
									return true;
								}
							}
						}
						if (continueNow)
						{
							continue;
						}
					}
					ModelElement nextElement = element;
					element = null;

					// If we could not select the current element, then go up the aggregation chain
					foreach (MetaRoleInfo role in nextElement.MetaClass.AllMetaRolesPlayed)
					{
						MetaRoleInfo oppositeRole = role.OppositeMetaRole;
						if (oppositeRole.IsAggregate)
						{
							IList parents = nextElement.GetCounterpartRolePlayers(role, oppositeRole);
							if (parents.Count > 0)
							{
								Debug.Assert(parents.Count == 1); // The aggregating side of a relationship should have multiplicity==1
								element = (ModelElement)parents[0];
								break;
							}
						}
					}
				}
				return false;
			}
			bool IORMToolTaskProvider.NavigateTo(IORMToolTaskItem task)
			{
				return NavigateTo(task);
			}
			#endregion // IORMToolTaskProvider Implementation
		}
		/// <summary>
		/// Default implementation of a task item
		/// </summary>
		protected class ORMTaskItem : Task, IORMToolTaskItem
		{
			#region Member Variables
			IRepresentModelElements myElementLocator;
			IORMToolTaskProvider myOwner;
			#endregion //Member Variables
			#region Constructors
			private ORMTaskItem()
			{
			}
			/// <summary>
			/// Create a task item for the specified owning provider
			/// </summary>
			/// <param name="owner">IORMToolTaskProvider</param>
			public ORMTaskItem(IORMToolTaskProvider owner)
			{
				myOwner = owner;
			}
			#endregion // Constructors
			#region IORMToolTaskItem Implementation
			/// <summary>
			/// Implements IORMToolTaskItem.ElementLocator property
			/// </summary>
			protected IRepresentModelElements ElementLocator
			{
				get { return myElementLocator; }
				set { myElementLocator = value; }
			}
			IRepresentModelElements IORMToolTaskItem.ElementLocator
			{
				get	{ return ElementLocator; }
				set { ElementLocator = value; }
			}
			/// <summary>
			/// Implements IORMToolTaskItem.Text property
			/// </summary>
			protected new string Text
			{
				get { return base.Text; }
				set
				{
					// Don't trigger task list change unless needed
					string oldText = base.Text;
					if (oldText != value)
					{
						base.Text = value;
					}
				}
			}
			string IORMToolTaskItem.Text
			{
				get { return Text; }
				set { Text = value; }
			}
			#endregion // IORMToolTaskItem Implementation
			#region Base overrides
			/// <summary>
			/// Navigate to the item associate with this task
			/// </summary>
			/// <param name="e">EventArgs</param>
			protected override void OnNavigate(EventArgs e)
			{
				if (!myOwner.NavigateTo(this))
				{
					base.OnNavigate(e);
				}
			}
			#endregion // Base overrides
		}
		/// <summary>
		/// Create a new task provider. Called once the first time the TaskProvider
		/// property is accessed.
		/// </summary>
		protected virtual IORMToolTaskProvider CreateTaskProvider()
		{
			Debug.Assert(myTaskProvider == null);
			return new ORMTaskProvider(this);
		}
		#endregion // TaskProvider implementation
	}
}