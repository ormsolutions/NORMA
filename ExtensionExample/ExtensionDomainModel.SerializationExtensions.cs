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

namespace Neumont.Tools.ORM.ExtensionExample
{
	#region ExtensionDomainModel model serialization
	public partial class ExtensionDomainModel : IORMCustomSerializedDomainModel
	{
		/// <summary>
		/// The default XmlNamespace associated with the 'ExtensionDomainModel' extension model
		/// </summary>
		public static readonly string XmlNamespace = "http://schemas.neumont.edu/ORM/ExtensionExample";
		/// <summary>
		/// Implements IORMCustomSerializedDomainModel.DefaultElementPrefix
		/// </summary>
		protected static string DefaultElementPrefix
		{
			get
			{
				return "ormExtension";
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
			ret[0, 0] = "ormExtension";
			ret[0, 1] = "http://schemas.neumont.edu/ORM/ExtensionExample";
			ret[0, 2] = "ExtensionDomainModelTest.xsd";
			return ret;
		}
		string[,] IORMCustomSerializedDomainModel.GetCustomElementNamespaces()
		{
			return GetCustomElementNamespaces();
		}
		private static Dictionary<string, Guid> myClassNameMap;
		private static Collection<string> myValidNamespaces;
		/// <summary>
		/// Implements IORMCustomSerializedDomainModel.ShouldSerializeDomainClass
		/// </summary>
		protected bool ShouldSerializeDomainClass(Store store, DomainClassInfo classInfo)
		{
			return true;
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
			Collection<string> validNamespaces = ExtensionDomainModel.myValidNamespaces;
			Dictionary<string, Guid> classNameMap = ExtensionDomainModel.myClassNameMap;
			if (validNamespaces == null)
			{
				validNamespaces = new Collection<string>();
				validNamespaces.Add("http://schemas.neumont.edu/ORM/ExtensionExample");
				ExtensionDomainModel.myValidNamespaces = validNamespaces;
			}
			if (classNameMap == null)
			{
				classNameMap = new Dictionary<string, Guid>();
				classNameMap.Add("SampleElement", MyCustomExtensionElement.DomainClassId);
				classNameMap.Add("ObjectTypeRequiresMeaningfulNameError", ObjectTypeRequiresMeaningfulNameError.DomainClassId);
				ExtensionDomainModel.myClassNameMap = classNameMap;
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
	#endregion // ExtensionDomainModel model serialization
	#region MyCustomExtensionElement serialization
	public partial class MyCustomExtensionElement : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.ElementInfo;
			}
		}
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo
		/// </summary>
		protected ORMCustomSerializedChildElementInfo[] GetCustomSerializedChildElementInfo()
		{
			throw new NotSupportedException();
		}
		ORMCustomSerializedChildElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.CustomSerializedElementInfo
		/// </summary>
		protected ORMCustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new ORMCustomSerializedElementInfo(null, "SampleElement", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return this.CustomSerializedElementInfo;
			}
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedPropertyInfo
		/// </summary>
		protected ORMCustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			throw new NotSupportedException();
		}
		ORMCustomSerializedPropertyInfo IORMCustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			throw new NotSupportedException();
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		/// </summary>
		protected IComparer<DomainRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				return null;
			}
		}
		IComparer<DomainRoleInfo> IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		{
			get
			{
				return this.CustomSerializedChildRoleComparer;
			}
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return default(ORMCustomSerializedElementMatch);
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapAttribute
		/// </summary>
		protected Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			return default(Guid);
		}
		Guid IORMCustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.ShouldSerialize
		/// </summary>
		protected bool ShouldSerialize()
		{
			return (TestEnumeration.Zero != this.CustomEnum) || ("Default value" != this.TestProperty);
		}
		bool IORMCustomSerializedElement.ShouldSerialize()
		{
			return this.ShouldSerialize();
		}
	}
	#endregion // MyCustomExtensionElement serialization
	#region ObjectTypeRequiresMeaningfulNameError serialization
	public partial class ObjectTypeRequiresMeaningfulNameError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ORMModelElementHasExtensionModelError.ExtendedElementDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, "ObjectType", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ObjectTypeRequiresMeaningfulNameError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ORMModelElementHasExtensionModelError.ExtendedElementDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ExtensionExample|ObjectType", match);
				ObjectTypeRequiresMeaningfulNameError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // ObjectTypeRequiresMeaningfulNameError serialization
}
