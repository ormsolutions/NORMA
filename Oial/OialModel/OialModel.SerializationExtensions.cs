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

namespace Neumont.Tools.Oial
{
	#region OialDomainModel model serialization
	partial class OialDomainModel : IORMCustomSerializedDomainModel
	{
		/// <summary>The default XmlNamespace associated with the 'OialDomainModel' extension model</summary>
		public static readonly string XmlNamespace = "http://schemas.orm.net/OIAL/Core";
		/// <summary>Implements IORMCustomSerializedDomainModel.DefaultElementPrefix</summary>
		protected static string DefaultElementPrefix
		{
			get
			{
				return "oil";
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
		protected static string[,] GetCustomElementNamespaces()
		{
			string[,] ret = new string[2, 3];
			ret[0, 0] = "oil";
			ret[0, 1] = "http://schemas.orm.net/OIAL/Core";
			ret[0, 2] = "OIAL.xsd";
			ret[1, 0] = "odt";
			ret[1, 1] = "http://schemas.orm.net/OIAL/Datatypes/Core";
			ret[1, 2] = "OIALDatatypes.xsd";
			return ret;
		}
		string[,] IORMCustomSerializedDomainModel.GetCustomElementNamespaces()
		{
			return GetCustomElementNamespaces();
		}
		private static Dictionary<string, Guid> myClassNameMap;
		private static Collection<string> myValidNamespaces;
		/// <summary>Implements IORMCustomSerializedDomainModel.ShouldSerializeDomainClass</summary>
		protected bool ShouldSerializeDomainClass(Store store, DomainClassInfo classInfo)
		{
			return true;
		}
		bool IORMCustomSerializedDomainModel.ShouldSerializeDomainClass(Store store, DomainClassInfo classInfo)
		{
			return this.ShouldSerializeDomainClass(store, classInfo);
		}
		/// <summary>Implements IORMCustomSerializedDomainModel.GetRootElementClasses</summary>
		protected static Guid[] GetRootElementClasses()
		{
			return new Guid[]{
				OialModel.DomainClassId};
		}
		Guid[] IORMCustomSerializedDomainModel.GetRootElementClasses()
		{
			return GetRootElementClasses();
		}
		/// <summary>Implements IORMCustomSerializedDomainModel.MapRootElement</summary>
		protected static Guid MapRootElement(string xmlNamespace, string elementName)
		{
			if ((elementName == "model") && (xmlNamespace == "http://schemas.orm.net/OIAL/Core"))
			{
				return OialModel.DomainClassId;
			}
			return default(Guid);
		}
		Guid IORMCustomSerializedDomainModel.MapRootElement(string xmlNamespace, string elementName)
		{
			return MapRootElement(xmlNamespace, elementName);
		}
		/// <summary>Implements IORMCustomSerializedDomainModel.MapClassName</summary>
		protected static Guid MapClassName(string xmlNamespace, string elementName)
		{
			Collection<string> validNamespaces = OialDomainModel.myValidNamespaces;
			Dictionary<string, Guid> classNameMap = OialDomainModel.myClassNameMap;
			if (validNamespaces == null)
			{
				validNamespaces = new Collection<string>();
				validNamespaces.Add("http://schemas.orm.net/OIAL/Core");
				validNamespaces.Add("http://schemas.orm.net/OIAL/Datatypes/Core");
				OialDomainModel.myValidNamespaces = validNamespaces;
			}
			if (classNameMap == null)
			{
				classNameMap = new Dictionary<string, Guid>();
				classNameMap.Add("model", OialModel.DomainClassId);
				classNameMap.Add("InformationTypeFormat", InformationTypeFormat.DomainClassId);
				classNameMap.Add("uniqueness", Uniqueness.DomainClassId);
				classNameMap.Add("conceptType", ConceptType.DomainClassId);
				classNameMap.Add("ConceptTypeHasChildAsPartOfAssociation", ConceptTypeHasChildAsPartOfAssociation.DomainClassId);
				classNameMap.Add("ConceptTypeHasUniqueness", ConceptTypeHasUniqueness.DomainClassId);
				classNameMap.Add("UniquenessIncludesConceptTypeChild", UniquenessIncludesConceptTypeChild.DomainClassId);
				classNameMap.Add("OialModelHasConceptType", OialModelHasConceptType.DomainClassId);
				classNameMap.Add("OialModelHasInformationTypeFormat", OialModelHasInformationTypeFormat.DomainClassId);
				OialDomainModel.myClassNameMap = classNameMap;
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
	#endregion // OialDomainModel model serialization
	#region OialModel serialization
	sealed partial class OialModel : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.ChildElementInfo | (ORMCustomSerializedElementSupportedOperations.ElementInfo | (ORMCustomSerializedElementSupportedOperations.PropertyInfo | ORMCustomSerializedElementSupportedOperations.LinkInfo));
			}
		}
		private static ORMCustomSerializedContainerElementInfo[] myCustomSerializedChildElementInfo;
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo</summary>
		ORMCustomSerializedContainerElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedContainerElementInfo[] ret = OialModel.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new ORMCustomSerializedContainerElementInfo[1];
				ret[0] = new ORMCustomSerializedContainerElementInfo(null, "informationTypeFormats", null, ORMCustomSerializedElementWriteStyle.Element, null, OialModelHasInformationTypeFormat.InformationTypeFormatDomainRoleId);
				OialModel.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		/// <summary>Implements IORMCustomSerializedElement.CustomSerializedElementInfo</summary>
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return new ORMCustomSerializedElementInfo(null, "model", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		ORMCustomSerializedPropertyInfo IORMCustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == OialModel.NameDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, "name", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			return ORMCustomSerializedPropertyInfo.Default;
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == OialModelHasConceptType.ConceptTypeDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, "conceptType", null, ORMCustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = OialModel.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(OialModelHasConceptType.ConceptTypeDomainRoleId);
				childElementMappings.Add("||||http://schemas.orm.net/OIAL/Core|conceptType", match);
				match.InitializeRoles(OialModelHasInformationTypeFormat.InformationTypeFormatDomainRoleId);
				childElementMappings.Add("||http://schemas.orm.net/OIAL/Core|informationTypeFormats||", match);
				OialModel.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", ((object)containerNamespace != (object)outerContainerNamespace) ? containerNamespace : null, "|", containerName, "|", ((object)elementNamespace != (object)containerNamespace) ? elementNamespace : null, "|", elementName), out rVal);
			return rVal;
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements IORMCustomSerializedElement.MapAttribute</summary>
		Guid IORMCustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = OialModel.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("name", OialModel.NameDomainPropertyId);
				OialModel.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // OialModel serialization
	#region InformationTypeFormat serialization
	partial class InformationTypeFormat : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.ElementInfo | (ORMCustomSerializedElementSupportedOperations.PropertyInfo | ORMCustomSerializedElementSupportedOperations.LinkInfo);
			}
		}
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo</summary>
		protected ORMCustomSerializedContainerElementInfo[] GetCustomSerializedChildElementInfo()
		{
			throw new NotSupportedException();
		}
		ORMCustomSerializedContainerElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>Implements IORMCustomSerializedElement.CustomSerializedElementInfo</summary>
		protected ORMCustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new ORMCustomSerializedElementInfo("odt", null, null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return this.CustomSerializedElementInfo;
			}
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		protected ORMCustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == InformationTypeFormat.NameDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, "name", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			return ORMCustomSerializedPropertyInfo.Default;
		}
		ORMCustomSerializedPropertyInfo IORMCustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == InformationType.ConceptTypeDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, "ConceptType", null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConceptTypeChild.ParentDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, "Parent", null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		/// <summary>Implements IORMCustomSerializedElement.CustomSerializedChildRoleComparer</summary>
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
		/// <summary>Implements IORMCustomSerializedElement.MapChildElement</summary>
		protected ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return default(ORMCustomSerializedElementMatch);
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements IORMCustomSerializedElement.MapAttribute</summary>
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
		Guid IORMCustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
		/// <summary>Implements IORMCustomSerializedElement.ShouldSerialize</summary>
		protected static bool ShouldSerialize()
		{
			return true;
		}
		bool IORMCustomSerializedElement.ShouldSerialize()
		{
			return ShouldSerialize();
		}
	}
	#endregion // InformationTypeFormat serialization
	#region Uniqueness serialization
	sealed partial class Uniqueness : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.ElementInfo | (ORMCustomSerializedElementSupportedOperations.PropertyInfo | ORMCustomSerializedElementSupportedOperations.LinkInfo);
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
				return new ORMCustomSerializedElementInfo(null, "uniqueness", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		ORMCustomSerializedPropertyInfo IORMCustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == Uniqueness.IsPreferredDomainPropertyId)
			{
				if (!(this.IsPreferred))
				{
					return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new ORMCustomSerializedPropertyInfo(null, "isPreferred", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == Uniqueness.NameDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, "name", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			return ORMCustomSerializedPropertyInfo.Default;
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == UniquenessIncludesConceptTypeChild.ConceptTypeChildDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, "uniquenessChild", null, ORMCustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = Uniqueness.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(UniquenessIncludesConceptTypeChild.ConceptTypeChildDomainRoleId);
				childElementMappings.Add("||||http://schemas.orm.net/OIAL/Core|uniquenessChild", match);
				Uniqueness.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", ((object)containerNamespace != (object)outerContainerNamespace) ? containerNamespace : null, "|", containerName, "|", ((object)elementNamespace != (object)containerNamespace) ? elementNamespace : null, "|", elementName), out rVal);
			return rVal;
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements IORMCustomSerializedElement.MapAttribute</summary>
		Guid IORMCustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
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
	#endregion // Uniqueness serialization
	#region ConceptType serialization
	sealed partial class ConceptType : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.ChildElementInfo | (ORMCustomSerializedElementSupportedOperations.ElementInfo | (ORMCustomSerializedElementSupportedOperations.PropertyInfo | ORMCustomSerializedElementSupportedOperations.LinkInfo));
			}
		}
		private static ORMCustomSerializedContainerElementInfo[] myCustomSerializedChildElementInfo;
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo</summary>
		ORMCustomSerializedContainerElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedContainerElementInfo[] ret = ConceptType.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new ORMCustomSerializedContainerElementInfo[1];
				ret[0] = new ORMCustomSerializedContainerElementInfo(null, "association", null, ORMCustomSerializedElementWriteStyle.Element, null, ConceptTypeHasChildAsPartOfAssociation.TargetDomainRoleId);
				ConceptType.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		/// <summary>Implements IORMCustomSerializedElement.CustomSerializedElementInfo</summary>
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return new ORMCustomSerializedElementInfo(null, "conceptType", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		ORMCustomSerializedPropertyInfo IORMCustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == ConceptType.NameDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, "name", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			return ORMCustomSerializedPropertyInfo.Default;
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == InformationType.InformationTypeFormatDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, "informationType", null, ORMCustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == ConceptTypeHasUniqueness.UniquenessDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, "uniqueness", null, ORMCustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == ConceptTypeRelatesToConceptType.RelatedConceptTypeDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, "relatedConceptType", null, ORMCustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == ConceptTypeAssimilatesConceptType.AssimilatedConceptTypeDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, "assimilatedConceptType", null, ORMCustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == ConceptTypeHasChildAsPartOfAssociation.TargetDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, "associationChild", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == ConceptTypeChild.ParentDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConceptTypeChild.TargetDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConceptTypeRelatesToConceptType.RelatingConceptTypeDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConceptTypeAssimilatesConceptType.AssimilatorConceptTypeDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConceptTypeReferencesConceptType.ReferencedConceptTypeDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConceptTypeReferencesConceptType.ReferencingConceptTypeDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
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
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ConceptType.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(InformationType.InformationTypeFormatDomainRoleId);
				childElementMappings.Add("||||http://schemas.orm.net/OIAL/Core|informationType", match);
				match.InitializeRoles(ConceptTypeHasUniqueness.UniquenessDomainRoleId);
				childElementMappings.Add("||||http://schemas.orm.net/OIAL/Core|uniqueness", match);
				match.InitializeRoles(ConceptTypeRelatesToConceptType.RelatedConceptTypeDomainRoleId);
				childElementMappings.Add("||||http://schemas.orm.net/OIAL/Core|relatedConceptType", match);
				match.InitializeRoles(ConceptTypeAssimilatesConceptType.AssimilatedConceptTypeDomainRoleId);
				childElementMappings.Add("||||http://schemas.orm.net/OIAL/Core|assimilatedConceptType", match);
				match.InitializeRoles(ConceptTypeHasChildAsPartOfAssociation.TargetDomainRoleId);
				childElementMappings.Add("||||http://schemas.orm.net/OIAL/Core|associationChild", match);
				ConceptType.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", ((object)containerNamespace != (object)outerContainerNamespace) ? containerNamespace : null, "|", containerName, "|", ((object)elementNamespace != (object)containerNamespace) ? elementNamespace : null, "|", elementName), out rVal);
			return rVal;
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements IORMCustomSerializedElement.MapAttribute</summary>
		Guid IORMCustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
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
	#endregion // ConceptType serialization
	#region ConceptTypeChild serialization
	partial class ConceptTypeChild : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.PropertyInfo | ORMCustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo</summary>
		protected ORMCustomSerializedContainerElementInfo[] GetCustomSerializedChildElementInfo()
		{
			throw new NotSupportedException();
		}
		ORMCustomSerializedContainerElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>Implements IORMCustomSerializedElement.CustomSerializedElementInfo</summary>
		protected ORMCustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				throw new NotSupportedException();
			}
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return this.CustomSerializedElementInfo;
			}
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		protected ORMCustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == ConceptTypeChild.IsMandatoryDomainPropertyId)
			{
				if (!(this.IsMandatory))
				{
					return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new ORMCustomSerializedPropertyInfo(null, "isMandatory", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == ConceptTypeChild.NameDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, "name", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			return ORMCustomSerializedPropertyInfo.Default;
		}
		ORMCustomSerializedPropertyInfo IORMCustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == UniquenessIncludesConceptTypeChild.UniquenessDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConceptTypeHasChildAsPartOfAssociation.ParentDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		/// <summary>Implements IORMCustomSerializedElement.CustomSerializedChildRoleComparer</summary>
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
		/// <summary>Implements IORMCustomSerializedElement.MapChildElement</summary>
		protected ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return default(ORMCustomSerializedElementMatch);
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements IORMCustomSerializedElement.MapAttribute</summary>
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
		Guid IORMCustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
		/// <summary>Implements IORMCustomSerializedElement.ShouldSerialize</summary>
		protected static bool ShouldSerialize()
		{
			return true;
		}
		bool IORMCustomSerializedElement.ShouldSerialize()
		{
			return ShouldSerialize();
		}
	}
	#endregion // ConceptTypeChild serialization
	#region InformationType serialization
	sealed partial class InformationType : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.ElementInfo;
			}
		}
		/// <summary>Implements IORMCustomSerializedElement.CustomSerializedElementInfo</summary>
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return new ORMCustomSerializedElementInfo(null, "informationType", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
		}
	}
	#endregion // InformationType serialization
	#region ConceptTypeReferencesConceptType serialization
	partial class ConceptTypeReferencesConceptType : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.PropertyInfo;
			}
		}
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		protected new ORMCustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == ConceptTypeReferencesConceptType.OppositeNameDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, "oppositeName", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return ORMCustomSerializedPropertyInfo.Default;
		}
		ORMCustomSerializedPropertyInfo IORMCustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements IORMCustomSerializedElement.MapAttribute</summary>
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
			if (!(customSerializedAttributes.TryGetValue(key, out rVal)))
			{
				rVal = base.MapAttribute(xmlNamespace, attributeName);
			}
			return rVal;
		}
		Guid IORMCustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
	}
	#endregion // ConceptTypeReferencesConceptType serialization
	#region ConceptTypeRelatesToConceptType serialization
	sealed partial class ConceptTypeRelatesToConceptType : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.ElementInfo;
			}
		}
		/// <summary>Implements IORMCustomSerializedElement.CustomSerializedElementInfo</summary>
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return new ORMCustomSerializedElementInfo(null, "relatedConceptType", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
		}
	}
	#endregion // ConceptTypeRelatesToConceptType serialization
	#region ConceptTypeAssimilatesConceptType serialization
	sealed partial class ConceptTypeAssimilatesConceptType : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.ElementInfo | ORMCustomSerializedElementSupportedOperations.PropertyInfo);
			}
		}
		/// <summary>Implements IORMCustomSerializedElement.CustomSerializedElementInfo</summary>
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return new ORMCustomSerializedElementInfo(null, "assimiliatedConceptType", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		ORMCustomSerializedPropertyInfo IORMCustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == ConceptTypeAssimilatesConceptType.RefersToSubtypeDomainPropertyId)
			{
				if (!(this.RefersToSubtype))
				{
					return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new ORMCustomSerializedPropertyInfo(null, "refersToSubtype", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == ConceptTypeAssimilatesConceptType.IsPreferredForTargetDomainPropertyId)
			{
				if (!(this.IsPreferredForTarget))
				{
					return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new ORMCustomSerializedPropertyInfo(null, "isPreferredForTarget", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == ConceptTypeAssimilatesConceptType.IsPreferredForParentDomainPropertyId)
			{
				if (!(this.IsPreferredForParent))
				{
					return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new ORMCustomSerializedPropertyInfo(null, "isPreferredForParent", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return ORMCustomSerializedPropertyInfo.Default;
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements IORMCustomSerializedElement.MapAttribute</summary>
		Guid IORMCustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
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
			if (!(customSerializedAttributes.TryGetValue(key, out rVal)))
			{
				rVal = base.MapAttribute(xmlNamespace, attributeName);
			}
			return rVal;
		}
	}
	#endregion // ConceptTypeAssimilatesConceptType serialization
	#region ConceptTypeHasChildAsPartOfAssociation serialization
	sealed partial class ConceptTypeHasChildAsPartOfAssociation : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.None;
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
			throw new NotSupportedException();
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
		/// <summary>Implements IORMCustomSerializedElement.MapChildElement</summary>
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return default(ORMCustomSerializedElementMatch);
		}
		/// <summary>Implements IORMCustomSerializedElement.MapAttribute</summary>
		Guid IORMCustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return default(Guid);
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
	#endregion // ConceptTypeHasChildAsPartOfAssociation serialization
	#region ConceptTypeHasUniqueness serialization
	sealed partial class ConceptTypeHasUniqueness : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.None;
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
			throw new NotSupportedException();
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
		/// <summary>Implements IORMCustomSerializedElement.MapChildElement</summary>
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return default(ORMCustomSerializedElementMatch);
		}
		/// <summary>Implements IORMCustomSerializedElement.MapAttribute</summary>
		Guid IORMCustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return default(Guid);
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
	#endregion // ConceptTypeHasUniqueness serialization
	#region UniquenessIncludesConceptTypeChild serialization
	sealed partial class UniquenessIncludesConceptTypeChild : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.None;
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
			throw new NotSupportedException();
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
		/// <summary>Implements IORMCustomSerializedElement.MapChildElement</summary>
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return default(ORMCustomSerializedElementMatch);
		}
		/// <summary>Implements IORMCustomSerializedElement.MapAttribute</summary>
		Guid IORMCustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return default(Guid);
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
	#endregion // UniquenessIncludesConceptTypeChild serialization
	#region OialModelHasConceptType serialization
	sealed partial class OialModelHasConceptType : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.None;
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
			throw new NotSupportedException();
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
		/// <summary>Implements IORMCustomSerializedElement.MapChildElement</summary>
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return default(ORMCustomSerializedElementMatch);
		}
		/// <summary>Implements IORMCustomSerializedElement.MapAttribute</summary>
		Guid IORMCustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return default(Guid);
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
	#endregion // OialModelHasConceptType serialization
	#region OialModelHasInformationTypeFormat serialization
	sealed partial class OialModelHasInformationTypeFormat : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.None;
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
			throw new NotSupportedException();
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
		/// <summary>Implements IORMCustomSerializedElement.MapChildElement</summary>
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return default(ORMCustomSerializedElementMatch);
		}
		/// <summary>Implements IORMCustomSerializedElement.MapAttribute</summary>
		Guid IORMCustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return default(Guid);
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
	#endregion // OialModelHasInformationTypeFormat serialization
}
