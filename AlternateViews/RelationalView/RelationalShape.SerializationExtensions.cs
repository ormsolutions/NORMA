using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.Shell;
using Neumont.Tools.ORM.ObjectModel;

// Common Public License Copyright Notice
// /**************************************************************************\
// * Neumont Object-Role Modeling Architect for Visual Studio                 *
// *                                                                          *
// * Copyright © Neumont University. All rights reserved.                     *
// *                                                                          *
// * The use and distribution terms for this software are covered by the      *
// * Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
// * can be found in the file CPL.txt at the root of this distribution.       *
// * By using this software in any fashion, you are agreeing to be bound by   *
// * the terms of this license.                                               *
// *                                                                          *
// * You must not remove this notice, or any other, from this software.       *
// \**************************************************************************/

namespace Neumont.Tools.ORM.Views.RelationalView
{
	#region RelationalShapeDomainModel model serialization
	internal partial class RelationalShapeDomainModel : IORMCustomSerializedDomainModel
	{
		/// <summary>
		/// The default XmlNamespace associated with the 'RelationalShapeDomainModel' extension model
		/// </summary>
		public static readonly string XmlNamespace = "http://schemas.neumont.edu/ORM/Views/RelationalView";
		/// <summary>
		/// Implements IORMCustomSerializedDomainModel.DefaultElementPrefix
		/// </summary>
		protected static string DefaultElementPrefix
		{
			get
			{
				return "relationalDiagram";
			}
		}
		string IORMCustomSerializedDomainModel.DefaultElementPrefix
		{
			get
			{
				return DefaultElementPrefix;
			}
		}
		/// <summary>
		/// Implements IORMCustomSerializedDomainModel.GetCustomElementNamespaces
		/// </summary>
		protected static string[,] GetCustomElementNamespaces()
		{
			string[,] ret = new string[1, 3];
			ret[0, 0] = "relationalDiagram";
			ret[0, 1] = "http://schemas.neumont.edu/ORM/Views/RelationalView";
			ret[0, 2] = "RelationalShape.xsd";
			return ret;
		}
		string[,] IORMCustomSerializedDomainModel.GetCustomElementNamespaces()
		{
			return GetCustomElementNamespaces();
		}
		private Dictionary<DomainClassInfo, object> myCustomSerializationOmissions;
		private static Dictionary<DomainClassInfo, object> BuildCustomSerializationOmissions(Store store)
		{
			Dictionary<DomainClassInfo, object> retVal = new Dictionary<DomainClassInfo, object>();
			DomainDataDirectory dataDir = store.DomainDataDirectory;
			retVal[dataDir.FindDomainClass(Neumont.Tools.ORM.Views.RelationalView.TableShape.DomainClassId)] = null;
			return retVal;
		}
		private static Dictionary<string, Guid> myClassNameMap;
		private static Collection<string> myValidNamespaces;
		/// <summary>
		/// Implements IORMCustomSerializedDomainModel.ShouldSerializeDomainClass
		/// </summary>
		protected bool ShouldSerializeDomainClass(Store store, DomainClassInfo classInfo)
		{
			Dictionary<DomainClassInfo, object> omissions = this.myCustomSerializationOmissions;
			if (omissions == null)
			{
				omissions = RelationalShapeDomainModel.BuildCustomSerializationOmissions(store);
				this.myCustomSerializationOmissions = omissions;
			}
			return !(omissions.ContainsKey(classInfo));
		}
		bool IORMCustomSerializedDomainModel.ShouldSerializeDomainClass(Store store, DomainClassInfo classInfo)
		{
			return this.ShouldSerializeDomainClass(store, classInfo);
		}
		/// <summary>
		/// Implements IORMCustomSerializedDomainModel.GetRootElementClasses
		/// </summary>
		protected static Guid[] GetRootElementClasses()
		{
			return new Guid[0];
		}
		Guid[] IORMCustomSerializedDomainModel.GetRootElementClasses()
		{
			return GetRootElementClasses();
		}
		/// <summary>
		/// Implements IORMCustomSerializedDomainModel.MapRootElement
		/// </summary>
		protected static Guid MapRootElement(string xmlNamespace, string elementName)
		{
			return default(Guid);
		}
		Guid IORMCustomSerializedDomainModel.MapRootElement(string xmlNamespace, string elementName)
		{
			return MapRootElement(xmlNamespace, elementName);
		}
		/// <summary>
		/// Implements IORMCustomSerializedDomainModel.MapClassName
		/// </summary>
		protected static Guid MapClassName(string xmlNamespace, string elementName)
		{
			Collection<string> validNamespaces = RelationalShapeDomainModel.myValidNamespaces;
			Dictionary<string, Guid> classNameMap = RelationalShapeDomainModel.myClassNameMap;
			if (validNamespaces == null)
			{
				validNamespaces = new Collection<string>();
				validNamespaces.Add("http://schemas.neumont.edu/ORM/Views/RelationalView");
				RelationalShapeDomainModel.myValidNamespaces = validNamespaces;
			}
			if (classNameMap == null)
			{
				classNameMap = new Dictionary<string, Guid>();
				RelationalShapeDomainModel.myClassNameMap = classNameMap;
			}
			if (validNamespaces.Contains(xmlNamespace) && classNameMap.ContainsKey(elementName))
			{
				return classNameMap[elementName];
			}
			return default(Guid);
		}
		Guid IORMCustomSerializedDomainModel.MapClassName(string xmlNamespace, string elementName)
		{
			return MapClassName(xmlNamespace, elementName);
		}
	}
	#endregion // RelationalShapeDomainModel model serialization
}
