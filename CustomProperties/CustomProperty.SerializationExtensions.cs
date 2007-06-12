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

namespace Neumont.Tools.ORM.CustomProperties
{
	#region CustomPropertiesDomainModel model serialization
	sealed partial class CustomPropertiesDomainModel : IORMCustomSerializedDomainModel
	{
		/// <summary>The default XmlNamespace associated with the 'CustomPropertiesDomainModel' extension model</summary>
		public static readonly string XmlNamespace = "http://schemas.neumont.edu/ORM/Preview/CustomProperties";
		/// <summary>Implements IORMCustomSerializedDomainModel.DefaultElementPrefix</summary>
		private static string DefaultElementPrefix
		{
			get
			{
				return "cp";
			}
		}
		string IORMCustomSerializedDomainModel.DefaultElementPrefix
		{
			get
			{
				return DefaultElementPrefix;
			}
		}
		/// <summary>Implements IORMCustomSerializedDomainModel.GetCustomElementNamespaces</summary>
		private static string[,] GetCustomElementNamespaces()
		{
			string[,] ret = new string[1, 3];
			ret[0, 0] = "cp";
			ret[0, 1] = "http://schemas.neumont.edu/ORM/Preview/CustomProperties";
			ret[0, 2] = "CustomProperties.xsd";
			return ret;
		}
		string[,] IORMCustomSerializedDomainModel.GetCustomElementNamespaces()
		{
			return GetCustomElementNamespaces();
		}
		private static Dictionary<string, Guid> myClassNameMap;
		private static Collection<string> myValidNamespaces;
		/// <summary>Implements IORMCustomSerializedDomainModel.ShouldSerializeDomainClass</summary>
		bool IORMCustomSerializedDomainModel.ShouldSerializeDomainClass(Store store, DomainClassInfo classInfo)
		{
			return true;
		}
		/// <summary>Implements IORMCustomSerializedDomainModel.GetRootElementClasses</summary>
		private static Guid[] GetRootElementClasses()
		{
			return new Guid[]{
				CustomPropertyGroup.DomainClassId};
		}
		Guid[] IORMCustomSerializedDomainModel.GetRootElementClasses()
		{
			return GetRootElementClasses();
		}
		/// <summary>Implements IORMCustomSerializedDomainModel.GetRootRelationshipContainers</summary>
		private static ORMRootRelationshipContainer[] GetRootRelationshipContainers()
		{
			return new ORMRootRelationshipContainer[0];
		}
		ORMRootRelationshipContainer[] IORMCustomSerializedDomainModel.GetRootRelationshipContainers()
		{
			return GetRootRelationshipContainers();
		}
		/// <summary>Implements IORMCustomSerializedDomainModel.MapRootElement</summary>
		private static Guid MapRootElement(string xmlNamespace, string elementName)
		{
			if ((elementName == "CustomPropertyGroup") && (xmlNamespace == "http://schemas.neumont.edu/ORM/Preview/CustomProperties"))
			{
				return CustomPropertyGroup.DomainClassId;
			}
			return default(Guid);
		}
		Guid IORMCustomSerializedDomainModel.MapRootElement(string xmlNamespace, string elementName)
		{
			return MapRootElement(xmlNamespace, elementName);
		}
		/// <summary>Implements IORMCustomSerializedDomainModel.MapClassName</summary>
		private static Guid MapClassName(string xmlNamespace, string elementName)
		{
			Collection<string> validNamespaces = CustomPropertiesDomainModel.myValidNamespaces;
			Dictionary<string, Guid> classNameMap = CustomPropertiesDomainModel.myClassNameMap;
			if (validNamespaces == null)
			{
				validNamespaces = new Collection<string>();
				validNamespaces.Add("http://schemas.neumont.edu/ORM/Preview/CustomProperties");
				CustomPropertiesDomainModel.myValidNamespaces = validNamespaces;
			}
			if (classNameMap == null)
			{
				classNameMap = new Dictionary<string, Guid>();
				classNameMap.Add("CustomPropertyGroup", CustomPropertyGroup.DomainClassId);
				classNameMap.Add("CustomPropertyDefinition", CustomPropertyDefinition.DomainClassId);
				classNameMap.Add("CustomProperty", CustomProperty.DomainClassId);
				CustomPropertiesDomainModel.myClassNameMap = classNameMap;
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
	#endregion // CustomPropertiesDomainModel model serialization
	#region CustomPropertyGroup serialization
	sealed partial class CustomPropertyGroup : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.ChildElementInfo | ORMCustomSerializedElementSupportedOperations.PropertyInfo;
			}
		}
		private static ORMCustomSerializedContainerElementInfo[] myCustomSerializedChildElementInfo;
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo</summary>
		ORMCustomSerializedContainerElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedContainerElementInfo[] ret = CustomPropertyGroup.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new ORMCustomSerializedContainerElementInfo[1];
				ret[0] = new ORMCustomSerializedContainerElementInfo(null, "CustomPropertyDefinitions", null, ORMCustomSerializedElementWriteStyle.Element, null, CustomPropertyGroupContainsCustomPropertyDefinition.CustomPropertyDefinitionDomainRoleId);
				CustomPropertyGroup.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		/// <summary>Implements IORMCustomSerializedElement.CustomSerializedElementInfo</summary>
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				throw new NotSupportedException();
			}
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		ORMCustomSerializedPropertyInfo IORMCustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == CustomPropertyGroup.NameDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, "name", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == CustomPropertyGroup.DescriptionDomainPropertyId)
			{
				if (string.IsNullOrEmpty(this.Description))
				{
					return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new ORMCustomSerializedPropertyInfo(null, "description", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == CustomPropertyGroup.IsDefaultDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, "isDefault", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			return ORMCustomSerializedPropertyInfo.Default;
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			throw new NotSupportedException();
		}
		/// <summary>Implements IORMCustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		IComparer<DomainRoleInfo> IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		{
			get
			{
				return null;
			}
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements IORMCustomSerializedElement.MapChildElement</summary>
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = CustomPropertyGroup.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(CustomPropertyGroupContainsCustomPropertyDefinition.CustomPropertyDefinitionDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/Preview/CustomProperties|CustomPropertyDefinitions||", match);
				CustomPropertyGroup.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", ((object)containerNamespace != (object)outerContainerNamespace) ? containerNamespace : null, "|", containerName, "|", ((object)elementNamespace != (object)containerNamespace) ? elementNamespace : null, "|", elementName), out rVal);
			return rVal;
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements IORMCustomSerializedElement.MapAttribute</summary>
		Guid IORMCustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = CustomPropertyGroup.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("name", CustomPropertyGroup.NameDomainPropertyId);
				customSerializedAttributes.Add("description", CustomPropertyGroup.DescriptionDomainPropertyId);
				customSerializedAttributes.Add("isDefault", CustomPropertyGroup.IsDefaultDomainPropertyId);
				CustomPropertyGroup.myCustomSerializedAttributes = customSerializedAttributes;
			}
			Guid rVal;
			string key = attributeName;
			if (xmlNamespace.Length != 0)
			{
				key = string.Concat(xmlNamespace, "|", attributeName);
			}
			customSerializedAttributes.TryGetValue(key, out rVal);
			return rVal;
		}
		/// <summary>Implements IORMCustomSerializedElement.ShouldSerialize</summary>
		private static bool ShouldSerialize()
		{
			return true;
		}
		bool IORMCustomSerializedElement.ShouldSerialize()
		{
			return ShouldSerialize();
		}
	}
	#endregion // CustomPropertyGroup serialization
	#region CustomPropertyDefinition serialization
	sealed partial class CustomPropertyDefinition : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.PropertyInfo | ORMCustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo</summary>
		ORMCustomSerializedContainerElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			throw new NotSupportedException();
		}
		/// <summary>Implements IORMCustomSerializedElement.CustomSerializedElementInfo</summary>
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				throw new NotSupportedException();
			}
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		ORMCustomSerializedPropertyInfo IORMCustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == CustomPropertyDefinition.NameDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, "name", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == CustomPropertyDefinition.DescriptionDomainPropertyId)
			{
				if (string.IsNullOrEmpty(this.Description))
				{
					return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new ORMCustomSerializedPropertyInfo(null, "description", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == CustomPropertyDefinition.CategoryDomainPropertyId)
			{
				if (string.IsNullOrEmpty(this.Category) || (this.Category == "Default"))
				{
					return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new ORMCustomSerializedPropertyInfo(null, "category", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == CustomPropertyDefinition.DataTypeDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, "dataType", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == CustomPropertyDefinition.DefaultValueDomainPropertyId)
			{
				if (this.DefaultValue == null)
				{
					return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new ORMCustomSerializedPropertyInfo(null, "defaultValue", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == CustomPropertyDefinition.CustomEnumValueDomainPropertyId)
			{
				if (string.IsNullOrEmpty(this.CustomEnumValue))
				{
					return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new ORMCustomSerializedPropertyInfo(null, "customEnumValues", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == CustomPropertyDefinition.ORMTypesDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, "ORMTypes", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			return ORMCustomSerializedPropertyInfo.Default;
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == CustomPropertyHasCustomPropertyDefinition.CustomPropertyDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, "CustomProperty", null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		/// <summary>Implements IORMCustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		IComparer<DomainRoleInfo> IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		{
			get
			{
				return null;
			}
		}
		/// <summary>Implements IORMCustomSerializedElement.MapChildElement</summary>
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return default(ORMCustomSerializedElementMatch);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements IORMCustomSerializedElement.MapAttribute</summary>
		Guid IORMCustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = CustomPropertyDefinition.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("name", CustomPropertyDefinition.NameDomainPropertyId);
				customSerializedAttributes.Add("description", CustomPropertyDefinition.DescriptionDomainPropertyId);
				customSerializedAttributes.Add("category", CustomPropertyDefinition.CategoryDomainPropertyId);
				customSerializedAttributes.Add("dataType", CustomPropertyDefinition.DataTypeDomainPropertyId);
				customSerializedAttributes.Add("defaultValue", CustomPropertyDefinition.DefaultValueDomainPropertyId);
				customSerializedAttributes.Add("customEnumValues", CustomPropertyDefinition.CustomEnumValueDomainPropertyId);
				customSerializedAttributes.Add("ORMTypes", CustomPropertyDefinition.ORMTypesDomainPropertyId);
				CustomPropertyDefinition.myCustomSerializedAttributes = customSerializedAttributes;
			}
			Guid rVal;
			string key = attributeName;
			if (xmlNamespace.Length != 0)
			{
				key = string.Concat(xmlNamespace, "|", attributeName);
			}
			customSerializedAttributes.TryGetValue(key, out rVal);
			return rVal;
		}
		/// <summary>Implements IORMCustomSerializedElement.ShouldSerialize</summary>
		private static bool ShouldSerialize()
		{
			return true;
		}
		bool IORMCustomSerializedElement.ShouldSerialize()
		{
			return ShouldSerialize();
		}
	}
	#endregion // CustomPropertyDefinition serialization
	#region CustomProperty serialization
	sealed partial class CustomProperty : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.PropertyInfo | ORMCustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo</summary>
		ORMCustomSerializedContainerElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			throw new NotSupportedException();
		}
		/// <summary>Implements IORMCustomSerializedElement.CustomSerializedElementInfo</summary>
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				throw new NotSupportedException();
			}
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		ORMCustomSerializedPropertyInfo IORMCustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == CustomProperty.ValueDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, "value", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			return ORMCustomSerializedPropertyInfo.Default;
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == CustomPropertyHasCustomPropertyDefinition.CustomPropertyDefinitionDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, "CustomPropertyDefinition", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		/// <summary>Implements IORMCustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		IComparer<DomainRoleInfo> IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		{
			get
			{
				return null;
			}
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements IORMCustomSerializedElement.MapChildElement</summary>
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = CustomProperty.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(CustomPropertyHasCustomPropertyDefinition.CustomPropertyDefinitionDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/Preview/CustomProperties|CustomPropertyDefinition", match);
				CustomProperty.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", ((object)containerNamespace != (object)outerContainerNamespace) ? containerNamespace : null, "|", containerName, "|", ((object)elementNamespace != (object)containerNamespace) ? elementNamespace : null, "|", elementName), out rVal);
			return rVal;
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements IORMCustomSerializedElement.MapAttribute</summary>
		Guid IORMCustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = CustomProperty.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("value", CustomProperty.ValueDomainPropertyId);
				CustomProperty.myCustomSerializedAttributes = customSerializedAttributes;
			}
			Guid rVal;
			string key = attributeName;
			if (xmlNamespace.Length != 0)
			{
				key = string.Concat(xmlNamespace, "|", attributeName);
			}
			customSerializedAttributes.TryGetValue(key, out rVal);
			return rVal;
		}
		/// <summary>Implements IORMCustomSerializedElement.ShouldSerialize</summary>
		private static bool ShouldSerialize()
		{
			return true;
		}
		bool IORMCustomSerializedElement.ShouldSerialize()
		{
			return ShouldSerialize();
		}
	}
	#endregion // CustomProperty serialization
}
