#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Neumont.Tools.ORM.ObjectModel;
using Neumont.Tools.ORM.ShapeModel;
using Neumont.Tools.ORM.Shell;
using System.Xml;

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
			#endregion // IORMToolServices Implementation
		}
	}
}
