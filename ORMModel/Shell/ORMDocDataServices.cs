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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Shell;
using Neumont.Tools.ORM.ShapeModel;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.Framework.DynamicSurveyTreeGrid;
using Neumont.Tools.ORM.Framework;
using Microsoft.VisualStudio.VirtualTreeGrid;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System.Runtime.InteropServices;

namespace Neumont.Tools.ORM.Shell
{
	public partial class ORMDesignerDocData : IORMToolServices
	{
		#region SurveyTreeSetup
		private ITree mySurveyTree = null;
		private MainList myRootBranch;
		/// <summary>
		/// property to return the survey tree associated with this DocData
		/// </summary>
		public ITree SurveyTree
		{
			get
			{
				if (mySurveyTree == null)
				{
					mySurveyTree = new VirtualTree();
					IList<ISurveyNodeProvider> nodeProviderList = new List<ISurveyNodeProvider>();
					IList<ISurveyQuestionProvider> questionProviderList = new List<ISurveyQuestionProvider>();
					ICollection<DomainModel> domainModels = this.Store.DomainModels;
					foreach(DomainModel domainModel in domainModels)
					{
						ISurveyNodeProvider nodeProvider = domainModel as ISurveyNodeProvider;
						if (nodeProvider != null)
						{
							nodeProviderList.Add(nodeProvider);	
						}
						ISurveyQuestionProvider questionProvider = domainModel as ISurveyQuestionProvider;
						if (questionProvider != null)
						{
							questionProviderList.Add(questionProvider);
						}
						IORMModelEventSubscriber eventSubscriber = domainModel as IORMModelEventSubscriber;
						if (eventSubscriber != null)
						{
							eventSubscriber.SurveyQuestionLoad();
						}
					}
					myRootBranch = new MainList(nodeProviderList, questionProviderList);
					mySurveyTree.Root = myRootBranch.RootBranch;
				}
				return mySurveyTree;
			}
		}

		#endregion //SurveyTreeSetup
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
			/// <summary>
			/// Defer to VerbalizationSnippetsDictionary on the document. Implements
			/// IORMToolServices.VerbalizationSnippetsDictionary
			/// </summary>
			protected IDictionary<Type, IVerbalizationSets> VerbalizationSnippetsDictionary
			{
				get
				{
					return myServices.VerbalizationSnippetsDictionary;
				}
			}
			IDictionary<Type, IVerbalizationSets> IORMToolServices.VerbalizationSnippetsDictionary
			{
				get
				{
					return VerbalizationSnippetsDictionary;
				}
			}
			/// <summary>
			/// currently unimplemented as this should only be returned from ORMDocDataServices
			/// </summary>
			protected INotifySurveyElementChanged NotifySurveyElementChanged
			{
				get
				{
					return myServices.NotifySurveyElementChanged;
				}
			}
			INotifySurveyElementChanged IORMToolServices.NotifySurveyElementChanged
			{
				get
				{
					return NotifySurveyElementChanged;
				}
			}

			#endregion // IORMToolServices Implementation
		}
		#endregion // Store services passthrough
		#region IORMToolServices Implementation
		private IORMToolTaskProvider myTaskProvider;
		private string myLastVerbalizationSnippetsOptions;
		private IDictionary<Type, IVerbalizationSets> myVerbalizationSnippets;
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
		/// <summary>
		/// Implements IORMToolServices.VerbalizationSnippetsDictionary
		/// </summary>
		protected IDictionary<Type, IVerbalizationSets> VerbalizationSnippetsDictionary
		{
			get
			{
				IDictionary<Type, IVerbalizationSets> retVal = myVerbalizationSnippets;
				string currentSnippetsOptions = myLastVerbalizationSnippetsOptions;
				string verbalizationOptions = OptionsPage.CurrentCustomVerbalizationSnippets;
				if (verbalizationOptions == null)
				{
					verbalizationOptions = "";
				}
				if (retVal == null || (currentSnippetsOptions == null || currentSnippetsOptions != verbalizationOptions))
				{
					currentSnippetsOptions = verbalizationOptions;
					// UNDONE: Directory should be configurable
					retVal = VerbalizationSnippetSetsManager.LoadSnippetsDictionary(
						Store,
						ORMDesignerPackage.VerbalizationDirectory,
						VerbalizationSnippetsIdentifier.ParseIdentifiers(verbalizationOptions));
					myVerbalizationSnippets = retVal;
					myLastVerbalizationSnippetsOptions = currentSnippetsOptions;
				}
				return retVal;
			}
		}
		IDictionary<Type, IVerbalizationSets> IORMToolServices.VerbalizationSnippetsDictionary
		{
			get
			{
				return VerbalizationSnippetsDictionary;
			}
		}
		/// <summary>
		/// Get the INotifySurveyElementChanged for the SurveyTree of this DocData
		/// may be null
		/// </summary>
		protected INotifySurveyElementChanged NotifySurveyElementChanged
		{
			get
			{
				return this.myRootBranch as INotifySurveyElementChanged;
			}
		}
		INotifySurveyElementChanged IORMToolServices.NotifySurveyElementChanged
		{
			get
			{
				return NotifySurveyElementChanged;
			}
		}
		#endregion // IORMToolServices Implementation
		#region TaskProvider implementation
		/// <summary>
		/// Default implementation of a task provider
		/// </summary>
		[CLSCompliant(false)]
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
			protected bool NavigateTo(IORMToolTaskItem task)
			{
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
				bool haveCurrentDesigner = false;
				DiagramDocView currentDocView = null;
				VSDiagramView currentDesigner = null;

				// The shape priority is:
				// 1) The first shape on a diagram which is the ActiveDiagramView for the current designer
				// 2) The first shape with an active diagram view
				// 3) The first shape.
				// We'll walk through the collection first to pick up the different elements
				const int ActiveViewOnActiveDocView = 0;
				const int ActiveView = 1;
				const int FirstShape = 2;
				const int SelectShapesSize = 3;
				ShapeElement[] selectShapes = new ShapeElement[SelectShapesSize];
				bool selectShapesInitialized = true;
				ORMDesignerDocData targetDocData = myDocument;
				// UNDONE: Move navigation code from here down into 
				// docdata and docview classes so we can use it elsewhere
				while (element != null)
				{
					ModelElement selectElement = element;
					if (useProxy)
					{
						// Second pass, we were unable to find a suitable shape for the first
						selectElement = proxyProvider.ElementDisplayedAs(element);
						if (selectElement != null && selectElement == element)
						{
							selectElement = null;
						}
					}

					// UNDONE: We should potentially be creating a shape
					// so we can jump to any error
					if (selectElement != null)
					{
						bool continueNow = false;

						// Grab the shapes in priority order
						if (!selectShapesInitialized)
						{
							selectShapes.Initialize();
							selectShapesInitialized = true;
						}
						foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(selectElement))
						{
							ShapeElement shape = pel as ShapeElement;
							if (shape != null)
							{
								// Get the current active designer
								if (!haveCurrentDesigner)
								{
									haveCurrentDesigner = true;
									IServiceProvider serviceProvider;
									IMonitorSelectionService selectionService;
									DiagramDocView currentView;
									if (null != (serviceProvider = targetDocData.ServiceProvider) &&
										null != (selectionService = (IMonitorSelectionService)serviceProvider.GetService(typeof(IMonitorSelectionService))) &&
										null != (currentView = selectionService.CurrentDocumentView as DiagramDocView) &&
										currentView.DocData == targetDocData)
									{
										currentDocView = currentView;
										currentDesigner = currentDocView.CurrentDesigner;
									}
								}

								// Get the shapes in priority
								Diagram diagram = shape.Diagram;
								if (diagram != null)
								{
									selectShapesInitialized = false;
									if (selectShapes[FirstShape] == null)
									{
										selectShapes[FirstShape] = shape;
									}
									VSDiagramView diagramView = diagram.ActiveDiagramView as VSDiagramView;
									if (diagramView != null)
									{
										if (selectShapes[ActiveView] == null)
										{
											selectShapes[ActiveView] = shape;
										}
										if (diagramView == currentDesigner)
										{
											if (selectShapes[ActiveViewOnActiveDocView] == null)
											{
												selectShapes[ActiveViewOnActiveDocView] = shape;
											}
										}
									}
								}
							}
						}

						// Walk the shapes in priority order and try to select them
						if (!selectShapesInitialized)
						{
							for (int i = 0; i < SelectShapesSize; ++i)
							{
								ShapeElement shape = selectShapes[i];
								if (shape != null)
								{
									if (proxyProvider == null)
									{
										proxyProvider = shape as IProxyDisplayProvider;
									}
									if (!useProxy && element is ORMModel && element != startElement)
									{
										if (proxyProvider != null)
										{
											useProxy = true;
											element = startElement;
											continueNow = true;
											break;
										}
									}

									// Select the correct document, activate it, select the correct tab,
									// then select the shape.
									Diagram diagram = shape.Diagram;
									Debug.Assert(diagram != null); // Checked in first pass through shape elements
									VSDiagramView diagramView = diagram.ActiveDiagramView as VSDiagramView;
									MultiDiagramDocView docView;
									VSDiagramView selectOnView = null;
									if (diagramView != null)
									{
										if (currentDesigner != null && diagramView == currentDesigner)
										{
											selectOnView = diagramView;
										}
										else if (null != (docView = diagramView.DocView as MultiDiagramDocView))
										{
											selectOnView = diagramView;
											docView.Show();
										}
									}
									else if (null != (docView = currentDocView as MultiDiagramDocView))
									{
										if (docView.ActivateDiagram(diagram))
										{
											selectOnView = docView.CurrentDesigner;
										}
									}
									else
									{
										// Walk all the documents and invalidate ORM diagrams if the options have changed
										#region Walk RunningDocumentTable
										IVsRunningDocumentTable docTable = (IVsRunningDocumentTable)targetDocData.ServiceProvider.GetService(typeof(IVsRunningDocumentTable));
										IEnumRunningDocuments docIter;
										ErrorHandler.ThrowOnFailure(docTable.GetRunningDocumentsEnum(out docIter));
										int hrIter;
										uint[] currentDocs = new uint[1];
										uint fetched = 0;
										do
										{
											ErrorHandler.ThrowOnFailure(hrIter = docIter.Next(1, currentDocs, out fetched));
											if (hrIter == 0)
											{
												uint grfRDTFlags;
												uint dwReadLocks;
												uint dwEditLocks;
												string bstrMkDocument;
												IVsHierarchy pHier;
												uint itemId;
												IntPtr punkDocData = IntPtr.Zero;
												ErrorHandler.ThrowOnFailure(docTable.GetDocumentInfo(
													currentDocs[0],
													out grfRDTFlags,
													out dwReadLocks,
													out dwEditLocks,
													out bstrMkDocument,
													out pHier,
													out itemId,
													out punkDocData));
												try
												{
													ORMDesignerDocData docData = Marshal.GetObjectForIUnknown(punkDocData) as ORMDesignerDocData;
													if (targetDocData == docData)
													{
														IList<ModelingDocView> views = docData.DocViews;
														int viewCount = views.Count;
														for (int j = 0; j < viewCount; ++j)
														{
															docView = views[j] as MultiDiagramDocView;
															if (docView != null && docView.ActivateDiagram(diagram))
															{
																docView.Show();
																selectOnView = docView.CurrentDesigner;
																break;
															}
														}
													}
												}
												finally
												{
													if (punkDocData != IntPtr.Zero)
													{
														Marshal.Release(punkDocData);
													}
												}
											}
										} while (fetched != 0 && selectOnView == null);
										#endregion // Walk RunningDocumentTable
									}

									if (selectOnView != null)
									{
										selectOnView.Selection.Set(new DiagramItem(shape));
										selectOnView.DiagramClientView.EnsureVisible(new ShapeElement[]{shape});
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
						}
						if (continueNow)
						{
							continue;
						}
					}
					ModelElement nextElement = element;
					element = null;

					// If we could not select the current element, then go up the aggregation chain
					foreach (DomainRoleInfo role in nextElement.GetDomainClass().AllDomainRolesPlayed)
					{
						DomainRoleInfo oppositeRole = role.OppositeDomainRole;
						if (oppositeRole.IsEmbedding)
						{
							LinkedElementCollection<ModelElement> parents = role.GetLinkedElements(nextElement);
							if (parents.Count > 0)
							{
								Debug.Assert(parents.Count == 1); // The aggregating side of a relationship should have multiplicity==1
								element = parents[0];
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
		[CLSCompliant(false)]
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
