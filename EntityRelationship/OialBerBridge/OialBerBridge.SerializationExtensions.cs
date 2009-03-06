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

namespace ORMSolutions.ORMArchitect.ORMAbstractionToBarkerERBridge
{
	#region ORMAbstractionToBarkerERBridgeDomainModel model serialization
	[CustomSerializedXmlNamespaces("http://schemas.neumont.edu/ORM/Bridge/2008-05/ORMAbstractionToBarkerER")]
	partial class ORMAbstractionToBarkerERBridgeDomainModel : ICustomSerializedDomainModel
	{
		/// <summary>The default XmlNamespace associated with the 'ORMAbstractionToBarkerERBridgeDomainModel' extension model</summary>
		public static readonly string XmlNamespace = "http://schemas.neumont.edu/ORM/Bridge/2008-05/ORMAbstractionToBarkerER";
		/// <summary>Implements ICustomSerializedDomainModel.DefaultElementPrefix</summary>
		protected static string DefaultElementPrefix
		{
			get
			{
				return "oialtober";
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
			ret[0, 0] = "oialtober";
			ret[0, 1] = "http://schemas.neumont.edu/ORM/Bridge/2008-05/ORMAbstractionToBarkerER";
			ret[0, 2] = "ORMAbstractionToBarkerERBridge.xsd";
			return ret;
		}
		string[,] ICustomSerializedDomainModel.GetCustomElementNamespaces()
		{
			return GetCustomElementNamespaces();
		}
		private static Dictionary<string, Guid> myClassNameMap;
		private static Collection<string> myValidNamespaces;
		private static CustomSerializedRootRelationshipContainer[] myRootRelationshipContainers;
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
				MappingBarkerModel.DomainClassId};
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
			CustomSerializedRootRelationshipContainer[] retVal = ORMAbstractionToBarkerERBridgeDomainModel.myRootRelationshipContainers;
			if (retVal == null)
			{
				retVal = new CustomSerializedRootRelationshipContainer[]{
					new CustomSerializedRootRelationshipContainer("oialtober", "Bridge", "http://schemas.neumont.edu/ORM/Bridge/2008-05/ORMAbstractionToBarkerER", new CustomSerializedStandaloneRelationship[]{
						new CustomSerializedStandaloneRelationship(BarkerErModelIsForAbstractionModel.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("BarkerErModel", BarkerErModelIsForAbstractionModel.BarkerErModelDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("AbstractionModel", BarkerErModelIsForAbstractionModel.AbstractionModelDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(AttributeHasConceptTypeChild.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("Attribute", AttributeHasConceptTypeChild.AttributeDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("ConceptTypeChild", AttributeHasConceptTypeChild.ConceptTypeChildDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(EntityTypeIsPrimarilyForConceptType.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("EntityType", EntityTypeIsPrimarilyForConceptType.EntityTypeDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("ConceptType", EntityTypeIsPrimarilyForConceptType.ConceptTypeDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(BinaryAssociationHasConceptTypeChild.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("BinaryAssociation", BinaryAssociationHasConceptTypeChild.BinaryAssociationDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("ConceptTypeChild", BinaryAssociationHasConceptTypeChild.ConceptTypeChildDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(BinaryAssociationHasConceptType.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("BinaryAssociation", BinaryAssociationHasConceptType.BinaryAssociationDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("ConceptType", BinaryAssociationHasConceptType.ConceptTypeDomainRoleId)}, null, null, null)})};
				ORMAbstractionToBarkerERBridgeDomainModel.myRootRelationshipContainers = retVal;
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
			if (elementName == "MappingBarker" && xmlNamespace == "http://schemas.neumont.edu/ORM/Bridge/2008-05/ORMAbstractionToBarkerER")
			{
				return MappingBarkerModel.DomainClassId;
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
			Collection<string> validNamespaces = ORMAbstractionToBarkerERBridgeDomainModel.myValidNamespaces;
			Dictionary<string, Guid> classNameMap = ORMAbstractionToBarkerERBridgeDomainModel.myClassNameMap;
			if (validNamespaces == null)
			{
				validNamespaces = new Collection<string>();
				validNamespaces.Add("http://schemas.neumont.edu/ORM/Bridge/2008-05/ORMAbstractionToBarkerER");
				ORMAbstractionToBarkerERBridgeDomainModel.myValidNamespaces = validNamespaces;
			}
			if (classNameMap == null)
			{
				classNameMap = new Dictionary<string, Guid>();
				classNameMap.Add("BarkerERModelGenerationSetting", BarkerERModelGenerationSetting.DomainClassId);
				ORMAbstractionToBarkerERBridgeDomainModel.myClassNameMap = classNameMap;
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
	#endregion // ORMAbstractionToBarkerERBridgeDomainModel model serialization
	#region BarkerERModelGenerationSetting serialization
	partial class BarkerERModelGenerationSetting : ICustomSerializedElement
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
			if (roleId == GenerationSettingTargetsBarkerERModel.GeneratedBarkerERModelDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "BarkerErModel", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = BarkerERModelGenerationSetting.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(GenerationSettingTargetsBarkerERModel.GeneratedBarkerERModelDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/Bridge/2008-05/ORMAbstractionToBarkerER|BarkerErModel", match);
				BarkerERModelGenerationSetting.myChildElementMappings = childElementMappings;
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
	#endregion // BarkerERModelGenerationSetting serialization
}
