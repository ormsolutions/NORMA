using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.Modeling.Shell;

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
	[CustomSerializedXmlNamespaces("http://schemas.neumont.edu/ORM/ExtensionExample")]
	partial class ExtensionDomainModel : ICustomSerializedDomainModel
	{
		/// <summary>The default XmlNamespace associated with the 'ExtensionDomainModel' extension model</summary>
		public static readonly string XmlNamespace = "http://schemas.neumont.edu/ORM/ExtensionExample";
		/// <summary>Implements ICustomSerializedDomainModel.DefaultElementPrefix</summary>
		protected static string DefaultElementPrefix
		{
			get
			{
				return "ormExtension";
			}
		}
		string ICustomSerializedDomainModel.DefaultElementPrefix
		{
			get
			{
				return DefaultElementPrefix;
			}
		}
		/// <summary>Implements ICustomSerializedDomainModel.GetCustomElementNamespaces</summary>
		protected static string[,] GetCustomElementNamespaces()
		{
			string[,] ret = new string[1, 3];
			ret[0, 0] = "ormExtension";
			ret[0, 1] = "http://schemas.neumont.edu/ORM/ExtensionExample";
			ret[0, 2] = "ExtensionDomainModelTest.xsd";
			return ret;
		}
		string[,] ICustomSerializedDomainModel.GetCustomElementNamespaces()
		{
			return GetCustomElementNamespaces();
		}
		private static Dictionary<string, Guid> myClassNameMap;
		private static Collection<string> myValidNamespaces;
		/// <summary>Implements ICustomSerializedDomainModel.ShouldSerializeDomainClass</summary>
		protected bool ShouldSerializeDomainClass(Store store, DomainClassInfo classInfo)
		{
			return true;
		}
		bool ICustomSerializedDomainModel.ShouldSerializeDomainClass(Store store, DomainClassInfo classInfo)
		{
			return this.ShouldSerializeDomainClass(store, classInfo);
		}
		/// <summary>Implements ICustomSerializedDomainModel.GetRootElementClasses</summary>
		protected static Guid[] GetRootElementClasses()
		{
			return new Guid[0];
		}
		Guid[] ICustomSerializedDomainModel.GetRootElementClasses()
		{
			return GetRootElementClasses();
		}
		/// <summary>Implements ICustomSerializedDomainModel.ShouldSerializeRootElement</summary>
		protected static bool ShouldSerializeRootElement(ModelElement element)
		{
			return true;
		}
		bool ICustomSerializedDomainModel.ShouldSerializeRootElement(ModelElement element)
		{
			return ShouldSerializeRootElement(element);
		}
		/// <summary>Implements ICustomSerializedDomainModel.GetRootRelationshipContainers</summary>
		protected static CustomSerializedRootRelationshipContainer[] GetRootRelationshipContainers()
		{
			return null;
		}
		CustomSerializedRootRelationshipContainer[] ICustomSerializedDomainModel.GetRootRelationshipContainers()
		{
			return GetRootRelationshipContainers();
		}
		/// <summary>Implements ICustomSerializedDomainModel.MapRootElement</summary>
		protected static Guid MapRootElement(string xmlNamespace, string elementName)
		{
			return default(Guid);
		}
		Guid ICustomSerializedDomainModel.MapRootElement(string xmlNamespace, string elementName)
		{
			return MapRootElement(xmlNamespace, elementName);
		}
		/// <summary>Implements ICustomSerializedDomainModel.MapClassName</summary>
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
		Guid ICustomSerializedDomainModel.MapClassName(string xmlNamespace, string elementName)
		{
			return MapClassName(xmlNamespace, elementName);
		}
	}
	#endregion // ExtensionDomainModel model serialization
	#region MyCustomExtensionElement serialization
	partial class MyCustomExtensionElement : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.ElementInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedChildElementInfo</summary>
		protected CustomSerializedContainerElementInfo[] GetCustomSerializedChildElementInfo()
		{
			throw new NotSupportedException();
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		protected CustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new CustomSerializedElementInfo(null, "SampleElement", null, CustomSerializedElementWriteStyle.Element, null);
			}
		}
		CustomSerializedElementInfo ICustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return this.CustomSerializedElementInfo;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		protected CustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			throw new NotSupportedException();
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			throw new NotSupportedException();
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		protected IComparer<DomainRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				return null;
			}
		}
		IComparer<DomainRoleInfo> ICustomSerializedElement.CustomSerializedChildRoleComparer
		{
			get
			{
				return this.CustomSerializedChildRoleComparer;
			}
		}
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return default(CustomSerializedElementMatch);
		}
		CustomSerializedElementMatch ICustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
		}
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		protected Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			return default(Guid);
		}
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
		/// <summary>Implements ICustomSerializedElement.ShouldSerialize</summary>
		protected bool ShouldSerialize()
		{
			return TestEnumeration.Zero != this.CustomEnum || "Default value" != this.TestProperty;
		}
		bool ICustomSerializedElement.ShouldSerialize()
		{
			return this.ShouldSerialize();
		}
	}
	#endregion // MyCustomExtensionElement serialization
	#region ObjectTypeRequiresMeaningfulNameError serialization
	partial class ObjectTypeRequiresMeaningfulNameError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == Neumont.Tools.ORM.ObjectModel.ORMModelElementHasExtensionModelError.ExtendedElementDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ObjectType", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ObjectTypeRequiresMeaningfulNameError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(Neumont.Tools.ORM.ObjectModel.ORMModelElementHasExtensionModelError.ExtendedElementDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/ExtensionExample|ObjectType", match);
				ObjectTypeRequiresMeaningfulNameError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
			}
			return rVal;
		}
		CustomSerializedElementMatch ICustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
		}
	}
	#endregion // ObjectTypeRequiresMeaningfulNameError serialization
}
