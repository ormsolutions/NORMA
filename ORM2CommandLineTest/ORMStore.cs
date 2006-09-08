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


#endregion

namespace Neumont.Tools.ORM.SDK.TestEngine
{
	public partial struct Suite
	{
		private class ORMStore : Store, IORMToolServices
		{
			#region Member Variables
			private IORMToolServices myServices;
			#endregion // Member Variables
			#region Constructors
			/// <summary>
			/// Create a new store
			/// </summary>
			/// <param name="services">IORMToolServices to defer to</param>
			public ORMStore(IORMToolServices services)
			{
				myServices = services;
			}
			#endregion // Constructors
			#region IORMToolServices Implementation
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
			//TODO: anything implmenting IORMToolServices needs this property filled in with correct code
			INotifySurveyElementChanged IORMToolServices.NotifySurveyElementChanged
			{
				get
				{
					return null;
				}
			}


			#endregion // IORMToolServices Implementation
		}
	}
}
