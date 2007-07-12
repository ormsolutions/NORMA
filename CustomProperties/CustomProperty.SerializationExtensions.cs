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

namespace Neumont.Tools.ORM.CustomProperties
{
	#region CustomPropertiesDomainModel model serialization
	[CustomSerializedXmlNamespaces("http://schemas.neumont.edu/ORM/Preview/CustomProperties")]
	sealed partial class CustomPropertiesDomainModel : ICustomSerializedDomainModel
	{
		/// <summary>The default XmlNamespace associated with the 'CustomPropertiesDomainModel' extension model</summary>
		public static readonly string XmlNamespace = "http://schemas.neumont.edu/ORM/Preview/CustomProperties";
		/// <summary>Implements ICustomSerializedDomainModel.DefaultElementPrefix</summary>
		private static string DefaultElementPrefix
		{
			get
			{
				return "cp";
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
		private static string[,] GetCustomElementNamespaces()
		{
			string[,] ret = new string[1, 3];
			ret[0, 0] = "cp";
			ret[0, 1] = "http://schemas.neumont.edu/ORM/Preview/CustomProperties";
			ret[0, 2] = "CustomProperties.xsd";
			return ret;
		}
		string[,] ICustomSerializedDomainModel.GetCustomElementNamespaces()
		{
			return GetCustomElementNamespaces();
		}
		private static Dictionary<string, Guid> myClassNameMap;
		private static Collection<string> myValidNamespaces;
		/// <summary>Implements ICustomSerializedDomainModel.ShouldSerializeDomainClass</summary>
		bool ICustomSerializedDomainModel.ShouldSerializeDomainClass(Store store, DomainClassInfo classInfo)
		{
			return true;
		}
		/// <summary>Implements ICustomSerializedDomainModel.GetRootElementClasses</summary>
		private static Guid[] GetRootElementClasses()
		{
			return new Guid[]{
				CustomPropertyGroup.DomainClassId};
		}
		Guid[] ICustomSerializedDomainModel.GetRootElementClasses()
		{
			return GetRootElementClasses();
		}
		/// <summary>Implements ICustomSerializedDomainModel.GetRootRelationshipContainers</summary>
		private static CustomSerializedRootRelationshipContainer[] GetRootRelationshipContainers()
		{
			return null;
		}
		CustomSerializedRootRelationshipContainer[] ICustomSerializedDomainModel.GetRootRelationshipContainers()
		{
			return GetRootRelationshipContainers();
		}
		/// <summary>Implements ICustomSerializedDomainModel.MapRootElement</summary>
		private static Guid MapRootElement(string xmlNamespace, string elementName)
		{
			if ((elementName == "CustomPropertyGroup") && (xmlNamespace == "http://schemas.neumont.edu/ORM/Preview/CustomProperties"))
			{
				return CustomPropertyGroup.DomainClassId;
			}
			return default(Guid);
		}
		Guid ICustomSerializedDomainModel.MapRootElement(string xmlNamespace, string elementName)
		{
			return MapRootElement(xmlNamespace, elementName);
		}
		/// <summary>Implements ICustomSerializedDomainModel.MapClassName</summary>
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
		Guid ICustomSerializedDomainModel.MapClassName(string xmlNamespace, string elementName)
		{
			return MapClassName(xmlNamespace, elementName);
		}
	}
	#endregion // CustomPropertiesDomainModel model serialization
	#region CustomPropertyGroup serialization
	sealed partial class CustomPropertyGroup : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo;
			}
		}
		private static CustomSerializedContainerElementInfo[] myCustomSerializedChildElementInfo;
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedChildElementInfo</summary>
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			CustomSerializedContainerElementInfo[] ret = CustomPropertyGroup.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new CustomSerializedContainerElementInfo[1];
				ret[0] = new CustomSerializedContainerElementInfo(null, "CustomPropertyDefinitions", null, CustomSerializedElementWriteStyle.Element, null, CustomPropertyGroupContainsCustomPropertyDefinition.CustomPropertyDefinitionDomainRoleId);
				CustomPropertyGroup.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		CustomSerializedElementInfo ICustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				throw new NotSupportedException();
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == CustomPropertyGroup.NameDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, "name", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == CustomPropertyGroup.DescriptionDomainPropertyId)
			{
				if (string.IsNullOrEmpty(this.Description))
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, "description", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == CustomPropertyGroup.IsDefaultDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, "isDefault", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			throw new NotSupportedException();
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		IComparer<DomainRoleInfo> ICustomSerializedElement.CustomSerializedChildRoleComparer
		{
			get
			{
				return null;
			}
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		CustomSerializedElementMatch ICustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = CustomPropertyGroup.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(CustomPropertyGroupContainsCustomPropertyDefinition.CustomPropertyDefinitionDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/Preview/CustomProperties|CustomPropertyDefinitions||", match);
				CustomPropertyGroup.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", ((object)containerNamespace != (object)outerContainerNamespace) ? containerNamespace : null, "|", containerName, "|", ((object)elementNamespace != (object)containerNamespace) ? elementNamespace : null, "|", elementName), out rVal);
			return rVal;
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
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
		/// <summary>Implements ICustomSerializedElement.ShouldSerialize</summary>
		private static bool ShouldSerialize()
		{
			return true;
		}
		bool ICustomSerializedElement.ShouldSerialize()
		{
			return ShouldSerialize();
		}
	}
	#endregion // CustomPropertyGroup serialization
	#region CustomPropertyDefinition serialization
	sealed partial class CustomPropertyDefinition : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedChildElementInfo</summary>
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			throw new NotSupportedException();
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		CustomSerializedElementInfo ICustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				throw new NotSupportedException();
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == CustomPropertyDefinition.NameDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, "name", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == CustomPropertyDefinition.DescriptionDomainPropertyId)
			{
				if (string.IsNullOrEmpty(this.Description))
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, "description", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == CustomPropertyDefinition.CategoryDomainPropertyId)
			{
				if (string.IsNullOrEmpty(this.Category) || (this.Category == "Default"))
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, "category", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == CustomPropertyDefinition.DataTypeDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, "dataType", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == CustomPropertyDefinition.DefaultValueDomainPropertyId)
			{
				if (this.DefaultValue == null)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, "defaultValue", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == CustomPropertyDefinition.CustomEnumValueDomainPropertyId)
			{
				if (string.IsNullOrEmpty(this.CustomEnumValue))
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, "customEnumValues", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == CustomPropertyDefinition.ORMTypesDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, "ORMTypes", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == CustomPropertyHasCustomPropertyDefinition.CustomPropertyDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "CustomProperty", null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			return CustomSerializedElementInfo.Default;
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		IComparer<DomainRoleInfo> ICustomSerializedElement.CustomSerializedChildRoleComparer
		{
			get
			{
				return null;
			}
		}
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		CustomSerializedElementMatch ICustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return default(CustomSerializedElementMatch);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
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
		/// <summary>Implements ICustomSerializedElement.ShouldSerialize</summary>
		private static bool ShouldSerialize()
		{
			return true;
		}
		bool ICustomSerializedElement.ShouldSerialize()
		{
			return ShouldSerialize();
		}
	}
	#endregion // CustomPropertyDefinition serialization
	#region CustomProperty serialization
	sealed partial class CustomProperty : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedChildElementInfo</summary>
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			throw new NotSupportedException();
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		CustomSerializedElementInfo ICustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				throw new NotSupportedException();
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == CustomProperty.ValueDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, "value", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == CustomPropertyHasCustomPropertyDefinition.CustomPropertyDefinitionDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "CustomPropertyDefinition", null, CustomSerializedElementWriteStyle.Element, null);
			}
			return CustomSerializedElementInfo.Default;
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		IComparer<DomainRoleInfo> ICustomSerializedElement.CustomSerializedChildRoleComparer
		{
			get
			{
				return null;
			}
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		CustomSerializedElementMatch ICustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = CustomProperty.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(CustomPropertyHasCustomPropertyDefinition.CustomPropertyDefinitionDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/Preview/CustomProperties|CustomPropertyDefinition", match);
				CustomProperty.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", ((object)containerNamespace != (object)outerContainerNamespace) ? containerNamespace : null, "|", containerName, "|", ((object)elementNamespace != (object)containerNamespace) ? elementNamespace : null, "|", elementName), out rVal);
			return rVal;
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
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
		/// <summary>Implements ICustomSerializedElement.ShouldSerialize</summary>
		private static bool ShouldSerialize()
		{
			return true;
		}
		bool ICustomSerializedElement.ShouldSerialize()
		{
			return ShouldSerialize();
		}
	}
	#endregion // CustomProperty serialization
}
