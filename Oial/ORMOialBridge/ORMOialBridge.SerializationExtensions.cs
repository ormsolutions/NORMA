using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using ORMSolutions.ORMArchitect.Framework.Shell;

// Common Public License Copyright Notice
// /**************************************************************************\
// * Natural Object-Role Modeling Architect for Visual Studio                 *
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

namespace ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge
{
	#region ORMToORMAbstractionBridgeDomainModel model serialization
	[CustomSerializedXmlNamespaces("http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction")]
	partial class ORMToORMAbstractionBridgeDomainModel : ICustomSerializedDomainModel
	{
		/// <summary>The default XmlNamespace associated with the 'ORMToORMAbstractionBridgeDomainModel' extension model</summary>
		public static readonly string XmlNamespace = "http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction";
		/// <summary>Implements ICustomSerializedDomainModel.DefaultElementPrefix</summary>
		protected static string DefaultElementPrefix
		{
			get
			{
				return "ormtooial";
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
			ret[0, 0] = "ormtooial";
			ret[0, 1] = "http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction";
			ret[0, 2] = "ORMToORMAbstraction.xsd";
			return ret;
		}
		string[,] ICustomSerializedDomainModel.GetCustomElementNamespaces()
		{
			return GetCustomElementNamespaces();
		}
		private Dictionary<DomainClassInfo, object> myCustomSerializationOmissions;
		private static Dictionary<DomainClassInfo, object> BuildCustomSerializationOmissions(Store store)
		{
			Dictionary<DomainClassInfo, object> retVal = new Dictionary<DomainClassInfo, object>();
			DomainDataDirectory dataDir = store.DomainDataDirectory;
			retVal[dataDir.FindDomainRelationship(ExcludedORMModelElement.DomainClassId)] = null;
			return retVal;
		}
		private static Dictionary<string, Guid> myClassNameMap;
		private static Collection<string> myValidNamespaces;
		private static CustomSerializedRootRelationshipContainer[] myRootRelationshipContainers;
		/// <summary>Implements ICustomSerializedDomainModel.ShouldSerializeDomainClass</summary>
		protected bool ShouldSerializeDomainClass(Store store, DomainClassInfo classInfo)
		{
			Dictionary<DomainClassInfo, object> omissions = this.myCustomSerializationOmissions;
			if (omissions == null)
			{
				omissions = ORMToORMAbstractionBridgeDomainModel.BuildCustomSerializationOmissions(store);
				this.myCustomSerializationOmissions = omissions;
			}
			return !omissions.ContainsKey(classInfo);
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
			CustomSerializedRootRelationshipContainer[] retVal = ORMToORMAbstractionBridgeDomainModel.myRootRelationshipContainers;
			if (retVal == null)
			{
				retVal = new CustomSerializedRootRelationshipContainer[]{
					new CustomSerializedRootRelationshipContainer("ormtooial", "Bridge", "http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction", new CustomSerializedStandaloneRelationship[]{
						new CustomSerializedStandaloneRelationship(AbstractionModelIsForORMModel.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("AbstractionModel", AbstractionModelIsForORMModel.AbstractionModelDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("ORMModel", AbstractionModelIsForORMModel.ORMModelDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(FactTypeMapsTowardsRole.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("FactType", FactTypeMapsTowardsRole.FactTypeDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("TowardsRole", FactTypeMapsTowardsRole.TowardsRoleDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(ConceptTypeIsForObjectType.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("ConceptType", ConceptTypeIsForObjectType.ConceptTypeDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("ObjectType", ConceptTypeIsForObjectType.ObjectTypeDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(ConceptTypeChildHasPathFactType.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("ConceptTypeChild", ConceptTypeChildHasPathFactType.ConceptTypeChildDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("PathFactType", ConceptTypeChildHasPathFactType.PathFactTypeDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(InformationTypeFormatIsForValueType.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("InformationTypeFormat", InformationTypeFormatIsForValueType.InformationTypeFormatDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("ValueType", InformationTypeFormatIsForValueType.ValueTypeDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(UniquenessIsForUniquenessConstraint.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("AbstractionUniquenessConstraint", UniquenessIsForUniquenessConstraint.UniquenessDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("ORMUniquenessConstraint", UniquenessIsForUniquenessConstraint.UniquenessConstraintDomainRoleId)}, null, null, null)})};
				ORMToORMAbstractionBridgeDomainModel.myRootRelationshipContainers = retVal;
			}
			return retVal;
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
			Collection<string> validNamespaces = ORMToORMAbstractionBridgeDomainModel.myValidNamespaces;
			Dictionary<string, Guid> classNameMap = ORMToORMAbstractionBridgeDomainModel.myClassNameMap;
			if (validNamespaces == null)
			{
				validNamespaces = new Collection<string>();
				validNamespaces.Add("http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction");
				ORMToORMAbstractionBridgeDomainModel.myValidNamespaces = validNamespaces;
			}
			if (classNameMap == null)
			{
				classNameMap = new Dictionary<string, Guid>();
				classNameMap.Add("FactTypeMapsTowardsRole", FactTypeMapsTowardsRole.DomainClassId);
				classNameMap.Add("AbstractionModelGenerationSetting", AbstractionModelGenerationSetting.DomainClassId);
				ORMToORMAbstractionBridgeDomainModel.myClassNameMap = classNameMap;
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
	#endregion // ORMToORMAbstractionBridgeDomainModel model serialization
	#region FactTypeMapsTowardsRole serialization
	partial class FactTypeMapsTowardsRole : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.PropertyInfo;
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
			if (domainPropertyInfo.Id == FactTypeMapsTowardsRole.MandatoryPatternDomainPropertyId)
			{
				if (MappingMandatoryPattern.None == this.MandatoryPattern)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == FactTypeMapsTowardsRole.UniquenessPatternDomainPropertyId)
			{
				if (MappingUniquenessPattern.None == this.UniquenessPattern)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
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
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		protected Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = FactTypeMapsTowardsRole.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("MandatoryPattern", FactTypeMapsTowardsRole.MandatoryPatternDomainPropertyId);
				customSerializedAttributes.Add("UniquenessPattern", FactTypeMapsTowardsRole.UniquenessPatternDomainPropertyId);
				FactTypeMapsTowardsRole.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // FactTypeMapsTowardsRole serialization
	#region AbstractionModelGenerationSetting serialization
	partial class AbstractionModelGenerationSetting : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.LinkInfo;
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
			throw new NotSupportedException();
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == GenerationSettingTargetsAbstractionModel.GeneratedAbstractionModelDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "AbstractionModel", null, CustomSerializedElementWriteStyle.Element, null);
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
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = AbstractionModelGenerationSetting.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(GenerationSettingTargetsAbstractionModel.GeneratedAbstractionModelDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction|AbstractionModel", match);
				AbstractionModelGenerationSetting.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal);
			return rVal;
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
		protected static bool ShouldSerialize()
		{
			return true;
		}
		bool ICustomSerializedElement.ShouldSerialize()
		{
			return ShouldSerialize();
		}
	}
	#endregion // AbstractionModelGenerationSetting serialization
}
