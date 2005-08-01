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

namespace ORM2CommandLineTest
{
	public class ORMStore : Store, IORMToolServices
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
}
