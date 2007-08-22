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

namespace Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge
{
	#region ORMAbstractionToConceptualDatabaseBridgeDomainModel model serialization
	[CustomSerializedXmlNamespaces("http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase")]
	partial class ORMAbstractionToConceptualDatabaseBridgeDomainModel : ICustomSerializedDomainModel
	{
		/// <summary>The default XmlNamespace associated with the 'ORMAbstractionToConceptualDatabaseBridgeDomainModel' extension model</summary>
		public static readonly string XmlNamespace = "http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase";
		/// <summary>Implements ICustomSerializedDomainModel.DefaultElementPrefix</summary>
		protected static string DefaultElementPrefix
		{
			get
			{
				return "oialtocdb";
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
			ret[0, 0] = "oialtocdb";
			ret[0, 1] = "http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase";
			ret[0, 2] = "ORMAbstractionToConceptualDatabase.xsd";
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
			retVal[dataDir.FindDomainRelationship(AssimilationMappingKeepAlive.DomainClassId)] = null;
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
				omissions = ORMAbstractionToConceptualDatabaseBridgeDomainModel.BuildCustomSerializationOmissions(store);
				this.myCustomSerializationOmissions = omissions;
			}
			return !(omissions.ContainsKey(classInfo));
		}
		bool ICustomSerializedDomainModel.ShouldSerializeDomainClass(Store store, DomainClassInfo classInfo)
		{
			return this.ShouldSerializeDomainClass(store, classInfo);
		}
		/// <summary>Implements ICustomSerializedDomainModel.GetRootElementClasses</summary>
		protected static Guid[] GetRootElementClasses()
		{
			return new Guid[]{
				MappingCustomizationModel.DomainClassId};
		}
		Guid[] ICustomSerializedDomainModel.GetRootElementClasses()
		{
			return GetRootElementClasses();
		}
		/// <summary>Implements ICustomSerializedDomainModel.GetRootRelationshipContainers</summary>
		protected static CustomSerializedRootRelationshipContainer[] GetRootRelationshipContainers()
		{
			CustomSerializedRootRelationshipContainer[] retVal = ORMAbstractionToConceptualDatabaseBridgeDomainModel.myRootRelationshipContainers;
			if (retVal == null)
			{
				retVal = new CustomSerializedRootRelationshipContainer[]{
					new CustomSerializedRootRelationshipContainer("oialtocdb", "Bridge", "http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase", new CustomSerializedStandaloneRelationship[]{
						new CustomSerializedStandaloneRelationship(SchemaIsForAbstractionModel.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("Schema", SchemaIsForAbstractionModel.SchemaDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("AbstractionModel", SchemaIsForAbstractionModel.AbstractionModelDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(ColumnHasConceptTypeChild.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("Column", ColumnHasConceptTypeChild.ColumnDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("ConceptTypeChild", ColumnHasConceptTypeChild.ConceptTypeChildDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(DomainIsForInformationTypeFormat.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("Domain", DomainIsForInformationTypeFormat.DomainDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("InformationTypeFormat", DomainIsForInformationTypeFormat.InformationTypeFormatDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(TableIsPrimarilyForConceptType.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("Table", TableIsPrimarilyForConceptType.TableDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("ConceptType", TableIsPrimarilyForConceptType.ConceptTypeDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(TableIsAlsoForConceptType.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("Table", TableIsAlsoForConceptType.TableDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("ConceptType", TableIsAlsoForConceptType.ConceptTypeDomainRoleId)}, null, null, null),
						new CustomSerializedStandaloneRelationship(UniquenessConstraintIsForUniqueness.DomainClassId, new CustomSerializedStandaloneRelationshipRole[]{
							new CustomSerializedStandaloneRelationshipRole("UniquenessConstraint", UniquenessConstraintIsForUniqueness.UniquenessConstraintDomainRoleId),
							new CustomSerializedStandaloneRelationshipRole("Uniqueness", UniquenessConstraintIsForUniqueness.UniquenessDomainRoleId)}, null, null, null)})};
				ORMAbstractionToConceptualDatabaseBridgeDomainModel.myRootRelationshipContainers = retVal;
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
			if ((elementName == "MappingCustomization") && (xmlNamespace == "http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase"))
			{
				return MappingCustomizationModel.DomainClassId;
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
			Collection<string> validNamespaces = ORMAbstractionToConceptualDatabaseBridgeDomainModel.myValidNamespaces;
			Dictionary<string, Guid> classNameMap = ORMAbstractionToConceptualDatabaseBridgeDomainModel.myClassNameMap;
			if (validNamespaces == null)
			{
				validNamespaces = new Collection<string>();
				validNamespaces.Add("http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase");
				ORMAbstractionToConceptualDatabaseBridgeDomainModel.myValidNamespaces = validNamespaces;
			}
			if (classNameMap == null)
			{
				classNameMap = new Dictionary<string, Guid>();
				classNameMap.Add("MappingCustomization", MappingCustomizationModel.DomainClassId);
				classNameMap.Add("AssimilationMapping", AssimilationMapping.DomainClassId);
				ORMAbstractionToConceptualDatabaseBridgeDomainModel.myClassNameMap = classNameMap;
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
	#endregion // ORMAbstractionToConceptualDatabaseBridgeDomainModel model serialization
	#region MappingCustomizationModel serialization
	partial class MappingCustomizationModel : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.ElementInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		private static CustomSerializedContainerElementInfo[] myCustomSerializedChildElementInfo;
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedChildElementInfo</summary>
		protected CustomSerializedContainerElementInfo[] GetCustomSerializedChildElementInfo()
		{
			CustomSerializedContainerElementInfo[] ret = MappingCustomizationModel.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new CustomSerializedContainerElementInfo[1];
				ret[0] = new CustomSerializedContainerElementInfo(null, "AssimilationMappings", null, CustomSerializedElementWriteStyle.Element, null, MappingCustomizationModelHasAssimilationMapping.AssimilationMappingDomainRoleId);
				MappingCustomizationModel.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
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
				return new CustomSerializedElementInfo(null, "MappingCustomization", null, CustomSerializedElementWriteStyle.Element, null);
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
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = MappingCustomizationModel.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(MappingCustomizationModelHasAssimilationMapping.AssimilationMappingDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase|AssimilationMappings||", match);
				MappingCustomizationModel.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", ((object)containerNamespace != (object)outerContainerNamespace) ? containerNamespace : null, "|", containerName, "|", ((object)elementNamespace != (object)containerNamespace) ? elementNamespace : null, "|", elementName), out rVal);
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
		protected bool ShouldSerialize()
		{
			foreach (AssimilationMapping assimilationMapping in this.AssimilationMappingCollection)
			{
				if ((assimilationMapping.AbsorptionChoice != AssimilationAbsorptionChoice.Absorb) && (assimilationMapping.Assimilation != null))
				{
					return true;
				}
			}
			return false;
		}
		bool ICustomSerializedElement.ShouldSerialize()
		{
			return this.ShouldSerialize();
		}
	}
	#endregion // MappingCustomizationModel serialization
	#region AssimilationMapping serialization
	partial class AssimilationMapping : ICustomSerializedElement
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
			if (roleId == AssimilationMappingCustomizesAssimilation.AssimilationDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Assimilation", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = AssimilationMapping.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(AssimilationMappingCustomizesAssimilation.AssimilationDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase|Assimilation", match);
				AssimilationMapping.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", ((object)containerNamespace != (object)outerContainerNamespace) ? containerNamespace : null, "|", containerName, "|", ((object)elementNamespace != (object)containerNamespace) ? elementNamespace : null, "|", elementName), out rVal);
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
		protected bool ShouldSerialize()
		{
			return (this.AbsorptionChoice != AssimilationAbsorptionChoice.Absorb) && (this.Assimilation != null);
		}
		bool ICustomSerializedElement.ShouldSerialize()
		{
			return this.ShouldSerialize();
		}
	}
	#endregion // AssimilationMapping serialization
}
