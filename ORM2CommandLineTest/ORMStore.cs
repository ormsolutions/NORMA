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


#endregion

namespace Neumont.Tools.ORM.SDK.TestEngine
{
	public partial struct Suite
	{
		private class ORMStore : Store, IORMToolServices, ISafeEventManagerProvider
		{
			#region Member Variables
			private readonly IORMToolServices myServices;
			private readonly SafeEventManager mySafeEventManager;
			#endregion // Member Variables
			#region CreateableSafeEventManager class
			private class CreateableSafeEventManager : SafeEventManager
			{
				public CreateableSafeEventManager(Store store) : base(store) { }
				protected override void DisplayException(Exception ex)
				{
					// UNDONE: Report any exception coming through here
				}
			}
			#endregion // CreateableSafeEventManager class
			#region Constructors
			/// <summary>
			/// Create a new store
			/// </summary>
			/// <param name="services">IORMToolServices to defer to</param>
			public ORMStore(IORMToolServices services)
			{
				myServices = services;
				mySafeEventManager = new CreateableSafeEventManager(this);
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
			IDictionary<Type, IVerbalizationSets> IORMToolServices.VerbalizationSnippetsDictionary
			{
				get
				{
					return myServices.VerbalizationSnippetsDictionary;
				}
			}
			//TODO: anything implementing IORMToolServices needs this property filled in with correct code
			INotifySurveyElementChanged IORMToolServices.NotifySurveyElementChanged
			{
				get
				{
					return null;
				}
			}
			#endregion // IORMToolServices Implementation
			#region ISafeEventManagerProvider Implementation
			SafeEventManager ISafeEventManagerProvider.SafeEventManager
			{
				get
				{
					return mySafeEventManager;
				}
			}
			#endregion // ISafeEventManagerProvider Implementation
		}
	}
}
