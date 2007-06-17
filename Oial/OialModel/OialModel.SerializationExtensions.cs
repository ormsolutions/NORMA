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

namespace Neumont.Tools.ORMAbstraction
{
	#region AbstractionDomainModel model serialization
	partial class AbstractionDomainModel : IORMCustomSerializedDomainModel
	{
		/// <summary>The default XmlNamespace associated with the 'AbstractionDomainModel' extension model</summary>
		public static readonly string XmlNamespace = "http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core";
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
			ret[0, 1] = "http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core";
			ret[0, 2] = "OIAL.xsd";
			ret[1, 0] = "odt";
			ret[1, 1] = "http://schemas.neumont.edu/ORM/Abstraction/2007-06/DataTypes/Core";
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
				AbstractionModel.DomainClassId};
		}
		Guid[] IORMCustomSerializedDomainModel.GetRootElementClasses()
		{
			return GetRootElementClasses();
		}
		/// <summary>Implements IORMCustomSerializedDomainModel.GetRootRelationshipContainers</summary>
		protected static ORMCustomSerializedRootRelationshipContainer[] GetRootRelationshipContainers()
		{
			return null;
		}
		ORMCustomSerializedRootRelationshipContainer[] IORMCustomSerializedDomainModel.GetRootRelationshipContainers()
		{
			return GetRootRelationshipContainers();
		}
		/// <summary>Implements IORMCustomSerializedDomainModel.MapRootElement</summary>
		protected static Guid MapRootElement(string xmlNamespace, string elementName)
		{
			if ((elementName == "model") && (xmlNamespace == "http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core"))
			{
				return AbstractionModel.DomainClassId;
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
		Guid IORMCustomSerializedDomainModel.MapClassName(string xmlNamespace, string elementName)
		{
			return MapClassName(xmlNamespace, elementName);
		}
	}
	#endregion // AbstractionDomainModel model serialization
	#region AbstractionModel serialization
	sealed partial class AbstractionModel : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.ChildElementInfo | (ORMCustomSerializedElementSupportedOperations.ElementInfo | (ORMCustomSerializedElementSupportedOperations.PropertyInfo | ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles));
			}
		}
		private static ORMCustomSerializedContainerElementInfo[] myCustomSerializedChildElementInfo;
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo</summary>
		ORMCustomSerializedContainerElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedContainerElementInfo[] ret = AbstractionModel.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new ORMCustomSerializedContainerElementInfo[2];
				ret[0] = new ORMCustomSerializedContainerElementInfo(null, "informationTypeFormats", null, ORMCustomSerializedElementWriteStyle.Element, null, AbstractionModelHasInformationTypeFormat.InformationTypeFormatDomainRoleId);
				ret[1] = new ORMCustomSerializedContainerElementInfo(null, "conceptTypes", null, ORMCustomSerializedElementWriteStyle.Element, null, AbstractionModelHasConceptType.ConceptTypeDomainRoleId);
				AbstractionModel.myCustomSerializedChildElementInfo = ret;
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
			if (domainPropertyInfo.Id == AbstractionModel.NameDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, "name", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			return ORMCustomSerializedPropertyInfo.Default;
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
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
				if (!(this.myRoleOrderDictionary.TryGetValue(string.Concat(x.DomainRelationship.ImplementationClass.FullName, ".", x.Name), out xPos)))
				{
					xPos = int.MaxValue;
				}
				int yPos;
				if (!(this.myRoleOrderDictionary.TryGetValue(string.Concat(y.DomainRelationship.ImplementationClass.FullName, ".", y.Name), out yPos)))
				{
					yPos = int.MaxValue;
				}
				return xPos.CompareTo(yPos);
			}
		}
		/// <summary>Implements IORMCustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		IComparer<DomainRoleInfo> IORMCustomSerializedElement.CustomSerializedChildRoleComparer
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
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements IORMCustomSerializedElement.MapChildElement</summary>
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = AbstractionModel.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(AbstractionModelHasInformationTypeFormat.InformationTypeFormatDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core|informationTypeFormats||", match);
				match.InitializeRoles(AbstractionModelHasConceptType.ConceptTypeDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core|conceptTypes||", match);
				AbstractionModel.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", ((object)containerNamespace != (object)outerContainerNamespace) ? containerNamespace : null, "|", containerName, "|", ((object)elementNamespace != (object)containerNamespace) ? elementNamespace : null, "|", elementName), out rVal);
			return rVal;
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements IORMCustomSerializedElement.MapAttribute</summary>
		Guid IORMCustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
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
	#endregion // AbstractionModel serialization
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
				return new ORMCustomSerializedElementInfo("odt", "dataType", null, ORMCustomSerializedElementWriteStyle.Element, null);
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
				return new ORMCustomSerializedElementInfo(null, "uniquenessConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
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
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core|uniquenessChild", match);
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
				return ORMCustomSerializedElementSupportedOperations.ChildElementInfo | (ORMCustomSerializedElementSupportedOperations.ElementInfo | (ORMCustomSerializedElementSupportedOperations.PropertyInfo | (ORMCustomSerializedElementSupportedOperations.LinkInfo | ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles)));
			}
		}
		private static ORMCustomSerializedContainerElementInfo[] myCustomSerializedChildElementInfo;
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo</summary>
		ORMCustomSerializedContainerElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedContainerElementInfo[] ret = ConceptType.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new ORMCustomSerializedContainerElementInfo[3];
				ret[0] = new ORMCustomSerializedContainerElementInfo(null, "children", null, ORMCustomSerializedElementWriteStyle.Element, null, InformationType.InformationTypeFormatDomainRoleId, ConceptTypeRelatesToConceptType.RelatedConceptTypeDomainRoleId, ConceptTypeAssimilatesConceptType.AssimilatedConceptTypeDomainRoleId);
				ret[1] = new ORMCustomSerializedContainerElementInfo(null, "uniquenessConstraints", null, ORMCustomSerializedElementWriteStyle.Element, null, ConceptTypeHasUniqueness.UniquenessDomainRoleId);
				ret[2] = new ORMCustomSerializedContainerElementInfo(null, "association", null, ORMCustomSerializedElementWriteStyle.Element, null, ConceptTypeHasChildAsPartOfAssociation.TargetDomainRoleId);
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
			if (roleId == ConceptTypeRelatesToConceptType.RelatedConceptTypeDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, "relatedConceptType", null, ORMCustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == ConceptTypeAssimilatesConceptType.AssimilatedConceptTypeDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, "assimilatedConceptType", null, ORMCustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == ConceptTypeRelatesToConceptType.RelatingConceptTypeDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConceptTypeAssimilatesConceptType.AssimilatorConceptTypeDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
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
				if (!(this.myRoleOrderDictionary.TryGetValue(string.Concat(x.DomainRelationship.ImplementationClass.FullName, ".", x.Name), out xPos)))
				{
					xPos = int.MaxValue;
				}
				int yPos;
				if (!(this.myRoleOrderDictionary.TryGetValue(string.Concat(y.DomainRelationship.ImplementationClass.FullName, ".", y.Name), out yPos)))
				{
					yPos = int.MaxValue;
				}
				return xPos.CompareTo(yPos);
			}
		}
		/// <summary>Implements IORMCustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		IComparer<DomainRoleInfo> IORMCustomSerializedElement.CustomSerializedChildRoleComparer
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
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements IORMCustomSerializedElement.MapChildElement</summary>
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ConceptType.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
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
}
