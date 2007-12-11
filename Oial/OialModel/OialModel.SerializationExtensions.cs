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

namespace Neumont.Tools.ORMAbstraction
{
	#region AbstractionDomainModel model serialization
	[CustomSerializedXmlNamespaces("http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core", "http://schemas.neumont.edu/ORM/Abstraction/2007-06/DataTypes/Core")]
	partial class AbstractionDomainModel : ICustomSerializedDomainModel
	{
		/// <summary>The default XmlNamespace associated with the 'AbstractionDomainModel' extension model</summary>
		public static readonly string XmlNamespace = "http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core";
		/// <summary>Implements ICustomSerializedDomainModel.DefaultElementPrefix</summary>
		protected static string DefaultElementPrefix
		{
			get
			{
				return "oial";
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
			string[,] ret = new string[2, 3];
			ret[0, 0] = "oial";
			ret[0, 1] = "http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core";
			ret[0, 2] = "OIAL.xsd";
			ret[1, 0] = "odt";
			ret[1, 1] = "http://schemas.neumont.edu/ORM/Abstraction/2007-06/DataTypes/Core";
			ret[1, 2] = "OIALDatatypes.xsd";
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
			return new Guid[]{
				AbstractionModel.DomainClassId};
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
			if (elementName == "model" && xmlNamespace == "http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core")
			{
				return AbstractionModel.DomainClassId;
			}
			return default(Guid);
		}
		Guid ICustomSerializedDomainModel.MapRootElement(string xmlNamespace, string elementName)
		{
			return MapRootElement(xmlNamespace, elementName);
		}
		/// <summary>Implements ICustomSerializedDomainModel.MapClassName</summary>
		protected static Guid MapClassName(string xmlNamespace, string elementName)
		{
			Collection<string> validNamespaces = AbstractionDomainModel.myValidNamespaces;
			Dictionary<string, Guid> classNameMap = AbstractionDomainModel.myClassNameMap;
			if (validNamespaces == null)
			{
				validNamespaces = new Collection<string>();
				validNamespaces.Add("http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core");
				validNamespaces.Add("http://schemas.neumont.edu/ORM/Abstraction/2007-06/DataTypes/Core");
				AbstractionDomainModel.myValidNamespaces = validNamespaces;
			}
			if (classNameMap == null)
			{
				classNameMap = new Dictionary<string, Guid>();
				classNameMap.Add("model", AbstractionModel.DomainClassId);
				classNameMap.Add("dataType", InformationTypeFormat.DomainClassId);
				classNameMap.Add("uniquenessConstraint", Uniqueness.DomainClassId);
				classNameMap.Add("conceptType", ConceptType.DomainClassId);
				AbstractionDomainModel.myClassNameMap = classNameMap;
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
	#endregion // AbstractionDomainModel model serialization
	#region AbstractionModel serialization
	sealed partial class AbstractionModel : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
			}
		}
		private static CustomSerializedContainerElementInfo[] myCustomSerializedChildElementInfo;
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedChildElementInfo</summary>
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			CustomSerializedContainerElementInfo[] ret = AbstractionModel.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new CustomSerializedContainerElementInfo[2];
				ret[0] = new CustomSerializedContainerElementInfo(null, "informationTypeFormats", null, CustomSerializedElementWriteStyle.Element, null, AbstractionModelHasInformationTypeFormat.InformationTypeFormatDomainRoleId);
				ret[1] = new CustomSerializedContainerElementInfo(null, "conceptTypes", null, CustomSerializedElementWriteStyle.Element, null, AbstractionModelHasConceptType.ConceptTypeDomainRoleId);
				AbstractionModel.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		CustomSerializedElementInfo ICustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return new CustomSerializedElementInfo(null, "model", null, CustomSerializedElementWriteStyle.Element, null);
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == AbstractionModel.NameDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, "name", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			throw new NotSupportedException();
		}
		private static IComparer<DomainRoleInfo> myCustomSortChildComparer;
		private sealed class CustomSortChildComparer : IComparer<DomainRoleInfo>
		{
			private readonly Dictionary<string, int> myRoleOrderDictionary;
			public CustomSortChildComparer(Store store)
			{
				DomainDataDirectory domainDataDirectory = store.DomainDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				DomainRoleInfo domainRole;
				domainRole = domainDataDirectory.FindDomainRole(AbstractionModelHasInformationTypeFormat.InformationTypeFormatDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(AbstractionModelHasConceptType.ConceptTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<DomainRoleInfo>.Compare(DomainRoleInfo x, DomainRoleInfo y)
			{
				int xPos;
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(x.DomainRelationship.ImplementationClass.FullName, ".", x.Name), out xPos))
				{
					xPos = int.MaxValue;
				}
				int yPos;
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(y.DomainRelationship.ImplementationClass.FullName, ".", y.Name), out yPos))
				{
					yPos = int.MaxValue;
				}
				return xPos.CompareTo(yPos);
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		IComparer<DomainRoleInfo> ICustomSerializedElement.CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<DomainRoleInfo> retVal = AbstractionModel.myCustomSortChildComparer;
				if (null == retVal)
				{
					retVal = new CustomSortChildComparer(this.Store);
					AbstractionModel.myCustomSortChildComparer = retVal;
				}
				return retVal;
			}
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		CustomSerializedElementMatch ICustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = AbstractionModel.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(AbstractionModelHasInformationTypeFormat.InformationTypeFormatDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core|informationTypeFormats||", match);
				match.InitializeRoles(AbstractionModelHasConceptType.ConceptTypeDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core|conceptTypes||", match);
				AbstractionModel.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal);
			return rVal;
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = AbstractionModel.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("name", AbstractionModel.NameDomainPropertyId);
				AbstractionModel.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // AbstractionModel serialization
	#region InformationTypeFormat serialization
	partial class InformationTypeFormat : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
				return new CustomSerializedElementInfo("odt", "dataType", null, CustomSerializedElementWriteStyle.Element, null);
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
			if (domainPropertyInfo.Id == InformationTypeFormat.NameDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, "name", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == InformationType.ConceptTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ConceptType", null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConceptTypeChild.ParentDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Parent", null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			return CustomSerializedElementInfo.Default;
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
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		protected Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = InformationTypeFormat.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("name", InformationTypeFormat.NameDomainPropertyId);
				InformationTypeFormat.myCustomSerializedAttributes = customSerializedAttributes;
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
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
		/// <summary>Implements ICustomSerializedElement.ShouldSerialize</summary>
		protected static bool ShouldSerialize()
		{
			return true;
		}
		bool ICustomSerializedElement.ShouldSerialize()
		{
			return ShouldSerialize();
		}
	}
	#endregion // InformationTypeFormat serialization
	#region Uniqueness serialization
	sealed partial class Uniqueness : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
				return new CustomSerializedElementInfo(null, "uniquenessConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == Uniqueness.IsPreferredDomainPropertyId)
			{
				if (!this.IsPreferred)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, "isPreferred", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == Uniqueness.NameDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, "name", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == UniquenessIncludesConceptTypeChild.ConceptTypeChildDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "uniquenessChild", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = Uniqueness.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(UniquenessIncludesConceptTypeChild.ConceptTypeChildDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core|uniquenessChild", match);
				Uniqueness.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal);
			return rVal;
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = Uniqueness.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("isPreferred", Uniqueness.IsPreferredDomainPropertyId);
				customSerializedAttributes.Add("name", Uniqueness.NameDomainPropertyId);
				Uniqueness.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // Uniqueness serialization
	#region ConceptType serialization
	sealed partial class ConceptType : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
			}
		}
		private static CustomSerializedContainerElementInfo[] myCustomSerializedChildElementInfo;
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedChildElementInfo</summary>
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			CustomSerializedContainerElementInfo[] ret = ConceptType.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new CustomSerializedContainerElementInfo[3];
				ret[0] = new CustomSerializedContainerElementInfo(null, "children", null, CustomSerializedElementWriteStyle.Element, null, InformationType.InformationTypeFormatDomainRoleId, ConceptTypeRelatesToConceptType.RelatedConceptTypeDomainRoleId, ConceptTypeAssimilatesConceptType.AssimilatedConceptTypeDomainRoleId);
				ret[1] = new CustomSerializedContainerElementInfo(null, "uniquenessConstraints", null, CustomSerializedElementWriteStyle.Element, null, ConceptTypeHasUniqueness.UniquenessDomainRoleId);
				ret[2] = new CustomSerializedContainerElementInfo(null, "association", null, CustomSerializedElementWriteStyle.Element, null, ConceptTypeHasChildAsPartOfAssociation.TargetDomainRoleId);
				ConceptType.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		CustomSerializedElementInfo ICustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return new CustomSerializedElementInfo(null, "conceptType", null, CustomSerializedElementWriteStyle.Element, null);
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == ConceptType.NameDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, "name", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == InformationType.InformationTypeFormatDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "informationType", null, CustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == ConceptTypeRelatesToConceptType.RelatedConceptTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "relatedConceptType", null, CustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == ConceptTypeAssimilatesConceptType.AssimilatedConceptTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "assimilatedConceptType", null, CustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == ConceptTypeRelatesToConceptType.RelatingConceptTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConceptTypeAssimilatesConceptType.AssimilatorConceptTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConceptTypeHasChildAsPartOfAssociation.TargetDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "associationChild", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == ConceptTypeChild.ParentDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConceptTypeChild.TargetDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConceptTypeReferencesConceptType.ReferencedConceptTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConceptTypeReferencesConceptType.ReferencingConceptTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			return CustomSerializedElementInfo.Default;
		}
		private static IComparer<DomainRoleInfo> myCustomSortChildComparer;
		private sealed class CustomSortChildComparer : IComparer<DomainRoleInfo>
		{
			private readonly Dictionary<string, int> myRoleOrderDictionary;
			public CustomSortChildComparer(Store store)
			{
				DomainDataDirectory domainDataDirectory = store.DomainDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				DomainRoleInfo domainRole;
				domainRole = domainDataDirectory.FindDomainRole(InformationType.InformationTypeFormatDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(ConceptTypeRelatesToConceptType.RelatedConceptTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(ConceptTypeAssimilatesConceptType.AssimilatedConceptTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(ConceptTypeHasUniqueness.UniquenessDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(ConceptTypeHasChildAsPartOfAssociation.TargetDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(ConceptTypeRelatesToConceptType.RelatingConceptTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 3;
				domainRole = domainDataDirectory.FindDomainRole(ConceptTypeAssimilatesConceptType.AssimilatorConceptTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 4;
				domainRole = domainDataDirectory.FindDomainRole(ConceptTypeChild.ParentDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 5;
				domainRole = domainDataDirectory.FindDomainRole(ConceptTypeChild.TargetDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 6;
				domainRole = domainDataDirectory.FindDomainRole(ConceptTypeReferencesConceptType.ReferencedConceptTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 7;
				domainRole = domainDataDirectory.FindDomainRole(ConceptTypeReferencesConceptType.ReferencingConceptTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 8;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<DomainRoleInfo>.Compare(DomainRoleInfo x, DomainRoleInfo y)
			{
				int xPos;
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(x.DomainRelationship.ImplementationClass.FullName, ".", x.Name), out xPos))
				{
					xPos = int.MaxValue;
				}
				int yPos;
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(y.DomainRelationship.ImplementationClass.FullName, ".", y.Name), out yPos))
				{
					yPos = int.MaxValue;
				}
				return xPos.CompareTo(yPos);
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		IComparer<DomainRoleInfo> ICustomSerializedElement.CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<DomainRoleInfo> retVal = ConceptType.myCustomSortChildComparer;
				if (null == retVal)
				{
					retVal = new CustomSortChildComparer(this.Store);
					ConceptType.myCustomSortChildComparer = retVal;
				}
				return retVal;
			}
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		CustomSerializedElementMatch ICustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ConceptType.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(true, InformationType.InformationTypeFormatDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core|children||informationType", match);
				match.InitializeRoles(true, ConceptTypeRelatesToConceptType.RelatedConceptTypeDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core|children||relatedConceptType", match);
				match.InitializeRoles(ConceptTypeAssimilatesConceptType.AssimilatedConceptTypeDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core|children||assimilatedConceptType", match);
				match.InitializeRoles(ConceptTypeHasChildAsPartOfAssociation.TargetDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core|association||associationChild", match);
				match.InitializeRoles(ConceptTypeHasUniqueness.UniquenessDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core|uniquenessConstraints||", match);
				ConceptType.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal);
			return rVal;
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = ConceptType.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("name", ConceptType.NameDomainPropertyId);
				ConceptType.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // ConceptType serialization
	#region ConceptTypeChild serialization
	partial class ConceptTypeChild : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
				throw new NotSupportedException();
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
			if (domainPropertyInfo.Id == ConceptTypeChild.IsMandatoryDomainPropertyId)
			{
				if (!this.IsMandatory)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, "isMandatory", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == ConceptTypeChild.NameDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, "name", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == UniquenessIncludesConceptTypeChild.UniquenessDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConceptTypeHasChildAsPartOfAssociation.ParentDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			return CustomSerializedElementInfo.Default;
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
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		protected Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = ConceptTypeChild.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("isMandatory", ConceptTypeChild.IsMandatoryDomainPropertyId);
				customSerializedAttributes.Add("name", ConceptTypeChild.NameDomainPropertyId);
				ConceptTypeChild.myCustomSerializedAttributes = customSerializedAttributes;
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
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
		/// <summary>Implements ICustomSerializedElement.ShouldSerialize</summary>
		protected static bool ShouldSerialize()
		{
			return true;
		}
		bool ICustomSerializedElement.ShouldSerialize()
		{
			return ShouldSerialize();
		}
	}
	#endregion // ConceptTypeChild serialization
	#region InformationType serialization
	sealed partial class InformationType : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ElementInfo;
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		CustomSerializedElementInfo ICustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return new CustomSerializedElementInfo(null, "informationType", null, CustomSerializedElementWriteStyle.Element, null);
			}
		}
	}
	#endregion // InformationType serialization
	#region ConceptTypeReferencesConceptType serialization
	partial class ConceptTypeReferencesConceptType : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.PropertyInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		protected new CustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == ConceptTypeReferencesConceptType.OppositeNameDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, "oppositeName", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = ConceptTypeReferencesConceptType.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("oppositeName", ConceptTypeReferencesConceptType.OppositeNameDomainPropertyId);
				ConceptTypeReferencesConceptType.myCustomSerializedAttributes = customSerializedAttributes;
			}
			Guid rVal;
			string key = attributeName;
			if (xmlNamespace.Length != 0)
			{
				key = string.Concat(xmlNamespace, "|", attributeName);
			}
			if (!customSerializedAttributes.TryGetValue(key, out rVal))
			{
				rVal = base.MapAttribute(xmlNamespace, attributeName);
			}
			return rVal;
		}
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
	}
	#endregion // ConceptTypeReferencesConceptType serialization
	#region ConceptTypeRelatesToConceptType serialization
	sealed partial class ConceptTypeRelatesToConceptType : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ElementInfo;
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		CustomSerializedElementInfo ICustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return new CustomSerializedElementInfo(null, "relatedConceptType", null, CustomSerializedElementWriteStyle.Element, null);
			}
		}
	}
	#endregion // ConceptTypeRelatesToConceptType serialization
	#region ConceptTypeAssimilatesConceptType serialization
	sealed partial class ConceptTypeAssimilatesConceptType : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo;
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		CustomSerializedElementInfo ICustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return new CustomSerializedElementInfo(null, "assimiliatedConceptType", null, CustomSerializedElementWriteStyle.Element, null);
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == ConceptTypeAssimilatesConceptType.RefersToSubtypeDomainPropertyId)
			{
				if (!this.RefersToSubtype)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, "refersToSubtype", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == ConceptTypeAssimilatesConceptType.IsPreferredForTargetDomainPropertyId)
			{
				if (!this.IsPreferredForTarget)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, "isPreferredForTarget", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == ConceptTypeAssimilatesConceptType.IsPreferredForParentDomainPropertyId)
			{
				if (!this.IsPreferredForParent)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, "isPreferredForParent", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = ConceptTypeAssimilatesConceptType.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("refersToSubtype", ConceptTypeAssimilatesConceptType.RefersToSubtypeDomainPropertyId);
				customSerializedAttributes.Add("isPreferredForTarget", ConceptTypeAssimilatesConceptType.IsPreferredForTargetDomainPropertyId);
				customSerializedAttributes.Add("isPreferredForParent", ConceptTypeAssimilatesConceptType.IsPreferredForParentDomainPropertyId);
				ConceptTypeAssimilatesConceptType.myCustomSerializedAttributes = customSerializedAttributes;
			}
			Guid rVal;
			string key = attributeName;
			if (xmlNamespace.Length != 0)
			{
				key = string.Concat(xmlNamespace, "|", attributeName);
			}
			if (!customSerializedAttributes.TryGetValue(key, out rVal))
			{
				rVal = base.MapAttribute(xmlNamespace, attributeName);
			}
			return rVal;
		}
	}
	#endregion // ConceptTypeAssimilatesConceptType serialization
}
