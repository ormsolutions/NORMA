#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;
using Neumont.Tools.ORM.Shell;
using System.Xml;
using Neumont.Tools.Modeling.Shell.DynamicSurveyTreeGrid;
using Neumont.Tools.Modeling;
using Neumont.Tools.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams;


#endregion

namespace Neumont.Tools.ORM.SDK.TestEngine
{
	public partial struct Suite
	{
		private class ORMStore : Store, IORMToolServices, IModelingEventManagerProvider
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
			#region IORMToolServices Implementation
			IORMPropertyProviderService IORMToolServices.PropertyProviderService
			{
				get
				{
					return myServices.PropertyProviderService;
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
			IDictionary<Type, IVerbalizationSets> IORMToolServices.GetVerbalizationSnippetsDictionary(VerbalizationTarget target)
			{
				return myServices.GetVerbalizationSnippetsDictionary(target);
			}
			INotifySurveyElementChanged IORMToolServices.NotifySurveyElementChanged
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
			bool IORMToolServices.ActivateShape(ShapeElement shape)
			{
				return myServices.ActivateShape(shape);
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
		}
	}
}
