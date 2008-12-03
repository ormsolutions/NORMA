#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ObjectModel.Design;
using Neumont.Tools.ORM.ShapeModel;
using Neumont.Tools.ORM.Shell;
using System.Xml;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.Modeling.Shell;
using Neumont.Tools.Modeling.Design;
#endregion

namespace Neumont.Tools.ORM.SDK.TestEngine
{
	public partial struct Suite
	{
		private class ORMStore : Store, IORMToolServices, IFrameworkServices, IModelingEventManagerProvider, ISerializationContextHost
		{
			#region Member Variables
			private readonly IORMToolServices myServices;
			private readonly ModelingEventManager myModelingEventManager;
			#endregion // Member Variables
			#region Constructors
			/// <summary>
			/// Create a new store
			/// </summary>
			/// <param name="services">IORMToolServices to defer to</param>
			public ORMStore(IORMToolServices services)
			{
				myServices = services;
				myModelingEventManager = new ModelingEventManagerImpl(this, (IORMToolTestServices)services);
			}
			#endregion // Constructors
			#region ORMModelErrorActivationService class
			private sealed class ORMModelErrorActivationService : IORMModelErrorActivationService
			{
				#region Member variables and constructor
				private Store myStore;
				private Dictionary<Type, ORMModelErrorActivator> myActivators;
				public ORMModelErrorActivationService(Store store)
				{
					myStore = store;
					myActivators = new Dictionary<Type, ORMModelErrorActivator>();
				}
				#endregion // Member variables and constructor
				#region IORMModelErrorActivationService Implementation
				private bool ActivateError(ModelElement selectedElement, ModelError error, DomainClassInfo domainClass)
				{
					ORMModelErrorActivator activator;
					if (myActivators.TryGetValue(domainClass.ImplementationClass, out activator))
					{
						if (activator((IORMToolServices)myStore, selectedElement, error))
						{
							return true;
						}
					}
					// See if anything on a base type can handle it. This maximizes the chances of finding a handler.
					// UNDONE: Do we want both the 'registerDerivedTypes' parameter on RegisterErrorActivator and this recursion?
					domainClass = domainClass.BaseDomainClass;
					if (domainClass != null)
					{
						return ActivateError(selectedElement, error, domainClass);
					}
					return false;
				}
				bool IORMModelErrorActivationService.ActivateError(ModelElement selectedElement, ModelError error)
				{
					return ActivateError(selectedElement, error, selectedElement.GetDomainClass());
				}
				/// <summary>
				/// Recursively register the given <paramref name="activator"/> for the <paramref name="domainClass"/>
				/// </summary>
				/// <param name="domainClass">The <see cref="DomainClassInfo"/> for the type to register</param>
				/// <param name="activator">A delegate callback for when an element of this type is selected</param>
				private void RegisterErrorActivator(DomainClassInfo domainClass, ORMModelErrorActivator activator)
				{
					myActivators[domainClass.ImplementationClass] = activator;
					foreach (DomainClassInfo derivedClassInfo in domainClass.AllDescendants)
					{
						RegisterErrorActivator(derivedClassInfo, activator);
					}
				}
				void IORMModelErrorActivationService.RegisterErrorActivator(Type elementType, bool registerDerivedTypes, ORMModelErrorActivator activator)
				{
					if (registerDerivedTypes)
					{
						DomainDataDirectory dataDirectory = myStore.DomainDataDirectory;
						RegisterErrorActivator(elementType.IsSubclassOf(typeof(ElementLink)) ? dataDirectory.GetDomainRelationship(elementType) : dataDirectory.GetDomainClass(elementType), activator);
					}
					else
					{
						myActivators[elementType] = activator;
					}
				}
				#endregion // IORMModelErrorActivationService Implementation
			}
			#endregion // ORMModelErrorActivationService class
			#region IORMToolServices Implementation
			IPropertyProviderService IFrameworkServices.PropertyProviderService
			{
				get
				{
					return myServices.PropertyProviderService;
				}
			}
			private ORMModelErrorActivationService myActivationService;
			IORMModelErrorActivationService IORMToolServices.ModelErrorActivationService
			{
				get
				{
					ORMModelErrorActivationService retVal = myActivationService;
					if (retVal == null)
					{
						myActivationService = retVal = new ORMModelErrorActivationService(this);
						ORMEditorUtility.RegisterModelErrorActivators(this);
					}
					return retVal;
				}
			}
			IORMToolTaskProvider IORMToolServices.TaskProvider
			{
				get
				{
					return myServices.TaskProvider;
				}
			}
			bool IORMToolServices.CanAddTransaction
			{
				get
				{
					return true;
				}
				set
				{
				}
			}
			IORMFontAndColorService IORMToolServices.FontAndColorService
			{
				get
				{
					return myServices.FontAndColorService;
				}
			}
			IServiceProvider IORMToolServices.ServiceProvider
			{
				get
				{
					return myServices.ServiceProvider;
				}
			}
			IDictionary<string, VerbalizationTargetData> IORMToolServices.VerbalizationTargets
			{
				get
				{
					return myServices.VerbalizationTargets;
				}
			}
			IDictionary<Type, IVerbalizationSets> IORMToolServices.GetVerbalizationSnippetsDictionary(string target)
			{
				return myServices.GetVerbalizationSnippetsDictionary(target);
			}
			INotifySurveyElementChanged IFrameworkServices.NotifySurveyElementChanged
			{
				get
				{
					return myServices.NotifySurveyElementChanged;
				}
			}
			private IDictionary<Type, LayoutEngineData> myLayoutEngines;
			LayoutEngine IORMToolServices.GetLayoutEngine(Type engineType)
			{
				IDictionary<Type, LayoutEngineData> retVal = myLayoutEngines;
				if (retVal == null)
				{
					retVal = LayoutEngineData.CreateLayoutEngineDictionary(this);
					myLayoutEngines = retVal;
				}
				return retVal[engineType].Instance;
			}
			bool IORMToolServices.ActivateShape(ShapeElement shape, NavigateToWindow window)
			{
				return myServices.ActivateShape(shape, window);
			}
			bool IORMToolServices.NavigateTo(object element, NavigateToWindow window)
			{
				return myServices.NavigateTo(element, window);
			}
			#endregion // IORMToolServices Implementation
			#region IModelingEventManagerProvider Implementation
			ModelingEventManager IModelingEventManagerProvider.ModelingEventManager
			{
				get
				{
					return myModelingEventManager;
				}
			}
			#endregion // IModelingEventManagerProvider Implementation
			#region ModelingEventManagerImpl class
			/// <summary>  
			/// A class to display an exception message without  
			/// breaking an event loop.  
			/// </summary>  
			private class ModelingEventManagerImpl : ModelingEventManager
			{
				private IORMToolTestServices myTestServices;
				/// <summary>  
				/// Create a new UISafeEventManager  
				/// </summary>  
				public ModelingEventManagerImpl(Store store, IORMToolTestServices testServices)
					: base(store)
				{
					myTestServices = testServices;
				}
				/// <summary>
				/// Record exception
				/// </summary>  
				/// <param name="ex">The exception to display.</param>  
				protected override void DisplayException(Exception ex)
				{
					myTestServices.LogException(ex);
				}
			}
			#endregion // ModelingEventManagerImpl class
			#region ISerializationContextHost Implementation
			private ISerializationContext mySerializationContext;
			/// <summary>
			/// Implements <see cref="ISerializationContextHost.SerializationContext"/>
			/// </summary>
			protected ISerializationContext SerializationContext
			{
				get
				{
					return mySerializationContext;
				}
				set
				{
					mySerializationContext = value;
				}
			}
			ISerializationContext ISerializationContextHost.SerializationContext
			{
				get
				{
					return SerializationContext;
				}
				set
				{
					SerializationContext = value;
				}
			}
			#endregion // ISerializationContextHost Implementation
		}
	}
}
