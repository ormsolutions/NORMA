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

namespace Neumont.Tools.ORM.Views.RelationalView
{
	#region RelationalShapeDomainModel model serialization
	partial class RelationalShapeDomainModel : IORMCustomSerializedDomainModel
	{
		/// <summary>The default XmlNamespace associated with the 'RelationalShapeDomainModel' extension model</summary>
		public static readonly string XmlNamespace = "http://schemas.neumont.edu/ORM/Views/RelationalView";
		/// <summary>Implements IORMCustomSerializedDomainModel.DefaultElementPrefix</summary>
		protected static string DefaultElementPrefix
		{
			get
			{
				return "rvw";
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
			string[,] ret = new string[1, 3];
			ret[0, 0] = "rvw";
			ret[0, 1] = "http://schemas.neumont.edu/ORM/Views/RelationalView";
			ret[0, 2] = "RelationalShape.xsd";
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
			retVal[dataDir.FindDomainRelationship(RelationalModelHasTable.DomainClassId)] = null;
			retVal[dataDir.FindDomainRelationship(ParentShapeContainsNestedChildShapes.DomainClassId)] = null;
			retVal[dataDir.FindDomainRelationship(PresentationViewsSubject.DomainClassId)] = null;
			retVal[dataDir.FindDomainClass(Neumont.Tools.ORM.Views.RelationalView.TableShape.DomainClassId)] = null;
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
				omissions = RelationalShapeDomainModel.BuildCustomSerializationOmissions(store);
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
				RelationalModel.DomainClassId,
				RelationalDiagram.DomainClassId};
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
			if ((elementName == "RelationalModel") && (xmlNamespace == "http://schemas.neumont.edu/ORM/Views/RelationalView"))
			{
				return RelationalModel.DomainClassId;
			}
			if ((elementName == "RelationalDiagram") && (xmlNamespace == "http://schemas.neumont.edu/ORM/Views/RelationalView"))
			{
				return RelationalDiagram.DomainClassId;
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
			Collection<string> validNamespaces = RelationalShapeDomainModel.myValidNamespaces;
			Dictionary<string, Guid> classNameMap = RelationalShapeDomainModel.myClassNameMap;
			if (validNamespaces == null)
			{
				validNamespaces = new Collection<string>();
				validNamespaces.Add("http://schemas.neumont.edu/ORM/Views/RelationalView");
				RelationalShapeDomainModel.myValidNamespaces = validNamespaces;
			}
			if (classNameMap == null)
			{
				classNameMap = new Dictionary<string, Guid>();
				classNameMap.Add("RelationalDiagram", RelationalDiagram.DomainClassId);
				classNameMap.Add("RelationalModel", RelationalModel.DomainClassId);
				RelationalShapeDomainModel.myClassNameMap = classNameMap;
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
	#endregion // RelationalShapeDomainModel model serialization
	#region RelationalDiagram serialization
	partial class RelationalDiagram : IORMCustomSerializedElement
	{
		/// <summary>Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.PropertyInfo;
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
			if (domainPropertyInfo.Id == RelationalDiagram.IsCompleteViewDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
			}
			if (domainPropertyInfo.Id == RelationalDiagram.NameDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
			}
			if (domainPropertyInfo.Id == RelationalDiagram.BaseFontNameDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
			}
			if (domainPropertyInfo.Id == RelationalDiagram.BaseFontSizeDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
			}
			if (domainPropertyInfo.Id == RelationalDiagram.DiagramIdDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
			}
			if (domainPropertyInfo.Id == RelationalDiagram.DoLineRoutingDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
			}
			if (domainPropertyInfo.Id == RelationalDiagram.DoResizeParentDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
			}
			if (domainPropertyInfo.Id == RelationalDiagram.DoShapeAnchoringDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
			}
			if (domainPropertyInfo.Id == RelationalDiagram.DoViewFixupDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
			}
			if (domainPropertyInfo.Id == RelationalDiagram.PlaceUnplacedShapesDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
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
	#endregion // RelationalDiagram serialization
	#region RelationalModel serialization
	partial class RelationalModel : IORMCustomSerializedElement
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
			if (domainPropertyInfo.Id == RelationalModel.NameDomainPropertyId)
			{
				return new ORMCustomSerializedPropertyInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
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
			if (roleId == RelationalModelHasOIALModel.OIALModelDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, "OIALModel", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == RelationalModelHasTable.TableDomainRoleId)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == PresentationViewsSubject.PresentationDomainRoleId)
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
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements IORMCustomSerializedElement.MapChildElement</summary>
		protected ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = RelationalModel.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(RelationalModelHasOIALModel.OIALModelDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/Views/RelationalView|OIALModel", match);
				RelationalModel.myChildElementMappings = childElementMappings;
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
	#endregion // RelationalModel serialization
}
