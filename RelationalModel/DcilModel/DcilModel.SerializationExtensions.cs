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

namespace Neumont.Tools.RelationalModels.ConceptualDatabase
{
	#region ConceptualDatabaseDomainModel model serialization
	partial class ConceptualDatabaseDomainModel : IORMCustomSerializedDomainModel
	{
		/// <summary>The default XmlNamespace associated with the 'ConceptualDatabaseDomainModel' extension model</summary>
		public static readonly string XmlNamespace = "http://schemas.neumont.edu/ORM/Relational/2007-06/ConceptualDatabase";
		/// <summary>Implements IORMCustomSerializedDomainModel.DefaultElementPrefix</summary>
		protected static string DefaultElementPrefix
		{
			get
			{
				return "rcd";
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
			ret[0, 0] = "rcd";
			ret[0, 1] = "http://schemas.neumont.edu/ORM/Relational/2007-06/ConceptualDatabase";
			ret[0, 2] = "ConceptualDatabase.xsd";
			ret[1, 0] = "ddt";
			ret[1, 1] = "http://schemas.orm.net/DIL/DILDT";
			ret[1, 2] = "DILDT.xsd";
			return ret;
		}
		string[,] IORMCustomSerializedDomainModel.GetCustomElementNamespaces()
		{
			return GetCustomElementNamespaces();
		}
		private Dictionary<DomainClassInfo, object> myCustomSerializationOmissions;
		private static Dictionary<DomainClassInfo, object> BuildCustomSerializationOmissions(Store store)
		{
			Dictionary<DomainClassInfo, object> retVal = new Dictionary<DomainClassInfo, object>();
			DomainDataDirectory dataDir = store.DomainDataDirectory;
			retVal[dataDir.FindDomainRelationship(SchemaContainsContent.DomainClassId)] = null;
			retVal[dataDir.FindDomainRelationship(TableContainsConstraint.DomainClassId)] = null;
			return retVal;
		}
		private static Dictionary<string, Guid> myClassNameMap;
		private static Collection<string> myValidNamespaces;
		/// <summary>Implements IORMCustomSerializedDomainModel.ShouldSerializeDomainClass</summary>
		protected bool ShouldSerializeDomainClass(Store store, DomainClassInfo classInfo)
		{
			Dictionary<DomainClassInfo, object> omissions = this.myCustomSerializationOmissions;
			if (omissions == null)
			{
				omissions = ConceptualDatabaseDomainModel.BuildCustomSerializationOmissions(store);
				this.myCustomSerializationOmissions = omissions;
			}
			return !(omissions.ContainsKey(classInfo));
		}
		bool IORMCustomSerializedDomainModel.ShouldSerializeDomainClass(Store store, DomainClassInfo classInfo)
		{
			return this.ShouldSerializeDomainClass(store, classInfo);
		}
		/// <summary>Implements IORMCustomSerializedDomainModel.GetRootElementClasses</summary>
		protected static Guid[] GetRootElementClasses()
		{
			return new Guid[]{
				Catalog.DomainClassId};
		}
		Guid[] IORMCustomSerializedDomainModel.GetRootElementClasses()
		{
			return GetRootElementClasses();
		}
		/// <summary>Implements IORMCustomSerializedDomainModel.GetRootRelationshipContainers</summary>
		protected static ORMRootRelationshipContainer[] GetRootRelationshipContainers()
		{
			return new ORMRootRelationshipContainer[0];
		}
		ORMRootRelationshipContainer[] IORMCustomSerializedDomainModel.GetRootRelationshipContainers()
		{
			return GetRootRelationshipContainers();
		}
		/// <summary>Implements IORMCustomSerializedDomainModel.MapRootElement</summary>
		protected static Guid MapRootElement(string xmlNamespace, string elementName)
		{
			if ((elementName == "Catalog") && (xmlNamespace == "http://schemas.neumont.edu/ORM/Relational/2007-06/ConceptualDatabase"))
			{
				return Catalog.DomainClassId;
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
			Collection<string> validNamespaces = ConceptualDatabaseDomainModel.myValidNamespaces;
			Dictionary<string, Guid> classNameMap = ConceptualDatabaseDomainModel.myClassNameMap;
			if (validNamespaces == null)
			{
				validNamespaces = new Collection<string>();
				validNamespaces.Add("http://schemas.neumont.edu/ORM/Relational/2007-06/ConceptualDatabase");
				validNamespaces.Add("http://schemas.orm.net/DIL/DILDT");
				ConceptualDatabaseDomainModel.myValidNamespaces = validNamespaces;
			}
			if (classNameMap == null)
			{
				classNameMap = new Dictionary<string, Guid>();
				classNameMap.Add("Catalog", Catalog.DomainClassId);
				classNameMap.Add("Schema", Schema.DomainClassId);
				classNameMap.Add("DomainDataType", Domain.DomainClassId);
				classNameMap.Add("Table", Table.DomainClassId);
				classNameMap.Add("Column", Column.DomainClassId);
				classNameMap.Add("UniquenessConstraint", UniquenessConstraint.DomainClassId);
				ConceptualDatabaseDomainModel.myClassNameMap = classNameMap;
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
	#endregion // ConceptualDatabaseDomainModel model serialization
	#region Catalog serialization
	partial class Catalog : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.ChildElementInfo;
			}
		}
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		private static ORMCustomSerializedContainerElementInfo[] myCustomSerializedChildElementInfo;
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo</summary>
		protected ORMCustomSerializedContainerElementInfo[] GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedContainerElementInfo[] ret = Catalog.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new ORMCustomSerializedContainerElementInfo[1];
				ret[0] = new ORMCustomSerializedContainerElementInfo(null, "Schemas", null, ORMCustomSerializedElementWriteStyle.Element, null, CatalogContainsSchema.SchemaDomainRoleId);
				Catalog.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
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
			throw new NotSupportedException();
		}
		ORMCustomSerializedPropertyInfo IORMCustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			throw new NotSupportedException();
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
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements IORMCustomSerializedElement.MapChildElement</summary>
		protected ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = Catalog.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(CatalogContainsSchema.SchemaDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/Relational/2007-06/ConceptualDatabase|Schemas||", match);
				Catalog.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", ((object)containerNamespace != (object)outerContainerNamespace) ? containerNamespace : null, "|", containerName, "|", ((object)elementNamespace != (object)containerNamespace) ? elementNamespace : null, "|", elementName), out rVal);
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
		}
		/// <summary>Implements IORMCustomSerializedElement.MapAttribute</summary>
		protected Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			return default(Guid);
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
	#endregion // Catalog serialization
	#region Schema serialization
	partial class Schema : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.ChildElementInfo;
			}
		}
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		private static ORMCustomSerializedContainerElementInfo[] myCustomSerializedChildElementInfo;
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo</summary>
		protected ORMCustomSerializedContainerElementInfo[] GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedContainerElementInfo[] ret = Schema.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new ORMCustomSerializedContainerElementInfo[2];
				ret[0] = new ORMCustomSerializedContainerElementInfo(null, "Tables", null, ORMCustomSerializedElementWriteStyle.Element, null, SchemaContainsTable.TableDomainRoleId);
				ret[1] = new ORMCustomSerializedContainerElementInfo(null, "DomainDataTypes", null, ORMCustomSerializedElementWriteStyle.Element, null, SchemaContainsDomain.DomainDomainRoleId);
				Schema.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
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
			throw new NotSupportedException();
		}
		ORMCustomSerializedPropertyInfo IORMCustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			throw new NotSupportedException();
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
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements IORMCustomSerializedElement.MapChildElement</summary>
		protected ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = Schema.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(SchemaContainsTable.TableDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/Relational/2007-06/ConceptualDatabase|Tables||", match);
				match.InitializeRoles(SchemaContainsDomain.DomainDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/Relational/2007-06/ConceptualDatabase|DomainDataTypes||", match);
				Schema.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", ((object)containerNamespace != (object)outerContainerNamespace) ? containerNamespace : null, "|", containerName, "|", ((object)elementNamespace != (object)containerNamespace) ? elementNamespace : null, "|", elementName), out rVal);
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
		}
		/// <summary>Implements IORMCustomSerializedElement.MapAttribute</summary>
		protected Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			return default(Guid);
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
	#endregion // Schema serialization
	#region Domain serialization
	partial class Domain : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
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
				return new ORMCustomSerializedElementInfo(null, "DomainDataType", null, ORMCustomSerializedElementWriteStyle.Element, null);
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
			throw new NotSupportedException();
		}
		ORMCustomSerializedPropertyInfo IORMCustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			throw new NotSupportedException();
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
		/// <summary>Implements IORMCustomSerializedElement.MapAttribute</summary>
		protected Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			return default(Guid);
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
	#endregion // Domain serialization
	#region Table serialization
	partial class Table : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.ChildElementInfo | ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles;
			}
		}
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		private static ORMCustomSerializedContainerElementInfo[] myCustomSerializedChildElementInfo;
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo</summary>
		protected ORMCustomSerializedContainerElementInfo[] GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedContainerElementInfo[] ret = Table.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new ORMCustomSerializedContainerElementInfo[2];
				ret[0] = new ORMCustomSerializedContainerElementInfo(null, "Columns", null, ORMCustomSerializedElementWriteStyle.Element, null, TableContainsColumn.ColumnDomainRoleId);
				ret[1] = new ORMCustomSerializedContainerElementInfo(null, "Constraints", null, ORMCustomSerializedElementWriteStyle.Element, null, TableContainsCheckConstraint.CheckConstraintDomainRoleId, TableContainsUniquenessConstraint.UniquenessConstraintDomainRoleId, TableContainsReferenceConstraint.ReferenceConstraintDomainRoleId);
				Table.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
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
			throw new NotSupportedException();
		}
		ORMCustomSerializedPropertyInfo IORMCustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			throw new NotSupportedException();
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				domainRole = domainDataDirectory.FindDomainRole(TableContainsColumn.ColumnDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(TableContainsCheckConstraint.CheckConstraintDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(TableContainsUniquenessConstraint.UniquenessConstraintDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(TableContainsReferenceConstraint.ReferenceConstraintDomainRoleId).OppositeDomainRole;
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
		protected IComparer<DomainRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<DomainRoleInfo> retVal = Table.myCustomSortChildComparer;
				if (null == retVal)
				{
					retVal = new CustomSortChildComparer(this.Store);
					Table.myCustomSortChildComparer = retVal;
				}
				return retVal;
			}
		}
		IComparer<DomainRoleInfo> IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		{
			get
			{
				return this.CustomSerializedChildRoleComparer;
			}
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements IORMCustomSerializedElement.MapChildElement</summary>
		protected ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = Table.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(TableContainsColumn.ColumnDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/Relational/2007-06/ConceptualDatabase|Columns||", match);
				match.InitializeRoles(TableContainsCheckConstraint.CheckConstraintDomainRoleId, TableContainsUniquenessConstraint.UniquenessConstraintDomainRoleId, TableContainsReferenceConstraint.ReferenceConstraintDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/Relational/2007-06/ConceptualDatabase|Constraints||", match);
				Table.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", ((object)containerNamespace != (object)outerContainerNamespace) ? containerNamespace : null, "|", containerName, "|", ((object)elementNamespace != (object)containerNamespace) ? elementNamespace : null, "|", elementName), out rVal);
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
		}
		/// <summary>Implements IORMCustomSerializedElement.MapAttribute</summary>
		protected Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			return default(Guid);
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
	#endregion // Table serialization
	#region Column serialization
	partial class Column : IORMCustomSerializedElement
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
			if (domainPropertyInfo.Id == Column.IsIdentityDomainPropertyId)
			{
				if (!(this.IsIdentity))
				{
					return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == Column.IsNullableDomainPropertyId)
			{
				if (!(this.IsNullable))
				{
					return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
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
			if (roleId == UniquenessConstraintIncludesColumn.UniquenessConstraintDomainRoleId)
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
			Dictionary<string, Guid> customSerializedAttributes = Column.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("IsIdentity", Column.IsIdentityDomainPropertyId);
				customSerializedAttributes.Add("IsNullable", Column.IsNullableDomainPropertyId);
				Column.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // Column serialization
	#region UniquenessConstraint serialization
	partial class UniquenessConstraint : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.ChildElementInfo | (ORMCustomSerializedElementSupportedOperations.PropertyInfo | ORMCustomSerializedElementSupportedOperations.LinkInfo);
			}
		}
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		private static ORMCustomSerializedContainerElementInfo[] myCustomSerializedChildElementInfo;
		/// <summary>Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo</summary>
		protected ORMCustomSerializedContainerElementInfo[] GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedContainerElementInfo[] ret = UniquenessConstraint.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new ORMCustomSerializedContainerElementInfo[1];
				ret[0] = new ORMCustomSerializedContainerElementInfo(null, "Columns", null, ORMCustomSerializedElementWriteStyle.Element, null, UniquenessConstraintIncludesColumn.ColumnDomainRoleId);
				UniquenessConstraint.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
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
			if (domainPropertyInfo.Id == UniquenessConstraint.IsPrimaryDomainPropertyId)
			{
				if (!(this.IsPrimary))
				{
					return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
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
			if (roleId == UniquenessConstraintIncludesColumn.ColumnDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, "Column", null, ORMCustomSerializedElementWriteStyle.Element, null);
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
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements IORMCustomSerializedElement.MapChildElement</summary>
		protected ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = UniquenessConstraint.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(UniquenessConstraintIncludesColumn.ColumnDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/Relational/2007-06/ConceptualDatabase|Columns||Column", match);
				UniquenessConstraint.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", ((object)containerNamespace != (object)outerContainerNamespace) ? containerNamespace : null, "|", containerName, "|", ((object)elementNamespace != (object)containerNamespace) ? elementNamespace : null, "|", elementName), out rVal);
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements IORMCustomSerializedElement.MapAttribute</summary>
		protected Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = UniquenessConstraint.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("IsPrimary", UniquenessConstraint.IsPrimaryDomainPropertyId);
				UniquenessConstraint.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // UniquenessConstraint serialization
}
