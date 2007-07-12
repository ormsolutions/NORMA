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

namespace Neumont.Tools.ORM.OIALModel
{
	#region OIALDomainModel model serialization
	partial class OIALDomainModel : ICustomSerializedDomainModel
	{
		/// <summary>The default XmlNamespace associated with the 'OIALDomainModel' extension model</summary>
		public static readonly string XmlNamespace = "http://schemas.neumont.edu/ORM/2006-01/OIALModel";
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
			string[,] ret = new string[1, 3];
			ret[0, 0] = "oial";
			ret[0, 1] = "http://schemas.neumont.edu/ORM/2006-01/OIALModel";
			ret[0, 2] = "OIALModel.xsd";
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
			retVal[dataDir.FindDomainRelationship(ChildSequenceConstraintHasChildSequence.DomainClassId)] = null;
			return retVal;
		}
		private static Dictionary<string, Guid> myClassNameMap;
		private static Collection<string> myValidNamespaces;
		/// <summary>Implements ICustomSerializedDomainModel.ShouldSerializeDomainClass</summary>
		protected bool ShouldSerializeDomainClass(Store store, DomainClassInfo classInfo)
		{
			Dictionary<DomainClassInfo, object> omissions = this.myCustomSerializationOmissions;
			if (omissions == null)
			{
				omissions = OIALDomainModel.BuildCustomSerializationOmissions(store);
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
				OIALModel.DomainClassId};
		}
		Guid[] ICustomSerializedDomainModel.GetRootElementClasses()
		{
			return GetRootElementClasses();
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
			if ((elementName == "Model") && (xmlNamespace == "http://schemas.neumont.edu/ORM/2006-01/OIALModel"))
			{
				return OIALModel.DomainClassId;
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
			Collection<string> validNamespaces = OIALDomainModel.myValidNamespaces;
			Dictionary<string, Guid> classNameMap = OIALDomainModel.myClassNameMap;
			if (validNamespaces == null)
			{
				validNamespaces = new Collection<string>();
				validNamespaces.Add("http://schemas.neumont.edu/ORM/2006-01/OIALModel");
				OIALDomainModel.myValidNamespaces = validNamespaces;
			}
			if (classNameMap == null)
			{
				classNameMap = new Dictionary<string, Guid>();
				classNameMap.Add("Model", OIALModel.DomainClassId);
				classNameMap.Add("ChildSequenceConstraint", ChildSequenceConstraint.DomainClassId);
				classNameMap.Add("ChildSequence", ChildSequence.DomainClassId);
				classNameMap.Add("ExclusionConstraint", ExclusionConstraint.DomainClassId);
				classNameMap.Add("SubsetConstraint", SubsetConstraint.DomainClassId);
				classNameMap.Add("EqualityConstraint", EqualityConstraint.DomainClassId);
				classNameMap.Add("InformationTypeFormat", InformationTypeFormat.DomainClassId);
				classNameMap.Add("ConceptType", ConceptType.DomainClassId);
				classNameMap.Add("ConceptTypeChild", ConceptTypeChild.DomainClassId);
				OIALDomainModel.myClassNameMap = classNameMap;
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
	#endregion // OIALDomainModel model serialization
	#region OIALModel serialization
	partial class OIALModel : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.ChildElementInfo | (CustomSerializedElementSupportedOperations.ElementInfo | (CustomSerializedElementSupportedOperations.PropertyInfo | (CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles)));
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
			CustomSerializedContainerElementInfo[] ret = OIALModel.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new CustomSerializedContainerElementInfo[3];
				ret[0] = new CustomSerializedContainerElementInfo(null, "InformationTypeFormats", null, CustomSerializedElementWriteStyle.NotWritten, null, OIALHasInformationTypeFormat.InformationTypeFormatDomainRoleId);
				ret[1] = new CustomSerializedContainerElementInfo(null, "ConceptTypes", null, CustomSerializedElementWriteStyle.NotWritten, null, OIALModelHasConceptType.ConceptTypeDomainRoleId);
				ret[2] = new CustomSerializedContainerElementInfo(null, "ChildSequenceConstraints", null, CustomSerializedElementWriteStyle.NotWritten, null, OIALModelHasChildSequenceConstraint.ChildSequenceConstraintDomainRoleId);
				OIALModel.myCustomSerializedChildElementInfo = ret;
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
				return new CustomSerializedElementInfo(null, "Model", null, CustomSerializedElementWriteStyle.Element, null);
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
			if (domainPropertyInfo.Id == OIALModel.NameDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
			}
			if (domainPropertyInfo.Id == OIALModel.RegeneratingDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
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
			if (roleId == OIALModelHasORMModel.ORMModelDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ORMModel", null, CustomSerializedElementWriteStyle.Element, null);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
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
				domainRole = domainDataDirectory.FindDomainRole(OIALModelHasORMModel.ORMModelDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(OIALHasInformationTypeFormat.InformationTypeFormatDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(OIALModelHasConceptType.ConceptTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(OIALModelHasChildSequenceConstraint.ChildSequenceConstraintDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 3;
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
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		protected IComparer<DomainRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<DomainRoleInfo> retVal = OIALModel.myCustomSortChildComparer;
				if (null == retVal)
				{
					retVal = new CustomSortChildComparer(this.Store);
					OIALModel.myCustomSortChildComparer = retVal;
				}
				return retVal;
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = OIALModel.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(OIALModelHasORMModel.ORMModelDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-01/OIALModel|ORMModel", match);
				match.InitializeRoles(OIALHasInformationTypeFormat.InformationTypeFormatDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-01/OIALModel|InformationTypeFormats||", match);
				match.InitializeRoles(OIALModelHasConceptType.ConceptTypeDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-01/OIALModel|ConceptTypes||", match);
				match.InitializeRoles(OIALModelHasChildSequenceConstraint.ChildSequenceConstraintDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-01/OIALModel|ChildSequenceConstraints||", match);
				OIALModel.myChildElementMappings = childElementMappings;
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
		protected static bool ShouldSerialize()
		{
			return true;
		}
		bool ICustomSerializedElement.ShouldSerialize()
		{
			return ShouldSerialize();
		}
	}
	#endregion // OIALModel serialization
	#region ChildSequenceConstraint serialization
	partial class ChildSequenceConstraint : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
			CustomSerializedContainerElementInfo[] ret = ChildSequenceConstraint.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new CustomSerializedContainerElementInfo[1];
				ret[0] = new CustomSerializedContainerElementInfo(null, "ChildSequence", null, CustomSerializedElementWriteStyle.Element, null, SingleChildSequenceConstraintHasMinTwoChildrenChildSequence.ChildSequenceDomainRoleId);
				ChildSequenceConstraint.myCustomSerializedChildElementInfo = ret;
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
			if (roleId == ChildHasSingleChildConstraint.ConceptTypeChildDomainRoleId)
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
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ChildSequenceConstraint.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(SingleChildSequenceConstraintHasMinTwoChildrenChildSequence.ChildSequenceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-01/OIALModel|ChildSequence||", match);
				ChildSequenceConstraint.myChildElementMappings = childElementMappings;
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
		protected static bool ShouldSerialize()
		{
			return true;
		}
		bool ICustomSerializedElement.ShouldSerialize()
		{
			return ShouldSerialize();
		}
	}
	#endregion // ChildSequenceConstraint serialization
	#region ChildSequence serialization
	partial class ChildSequence : ICustomSerializedElement
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
			if (roleId == ChildSequenceHasConceptTypeChild.ConceptTypeChildDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ConceptTypeChild", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ChildSequence.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ChildSequenceHasConceptTypeChild.ConceptTypeChildDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-01/OIALModel|ConceptTypeChild", match);
				ChildSequence.myChildElementMappings = childElementMappings;
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
		protected static bool ShouldSerialize()
		{
			return true;
		}
		bool ICustomSerializedElement.ShouldSerialize()
		{
			return ShouldSerialize();
		}
	}
	#endregion // ChildSequence serialization
	#region ExclusionConstraint serialization
	partial class ExclusionConstraint : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo;
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
		protected new CustomSerializedContainerElementInfo[] GetCustomSerializedChildElementInfo()
		{
			CustomSerializedContainerElementInfo[] ret = ExclusionConstraint.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				CustomSerializedContainerElementInfo[] baseInfo = null;
				int baseInfoCount = 0;
				if (0 != (CustomSerializedElementSupportedOperations.ChildElementInfo & base.SupportedCustomSerializedOperations))
				{
					baseInfo = base.GetCustomSerializedChildElementInfo();
					if (baseInfo != null)
					{
						baseInfoCount = baseInfo.Length;
					}
				}
				ret = new CustomSerializedContainerElementInfo[baseInfoCount + 1];
				if (baseInfoCount != 0)
				{
					baseInfo.CopyTo(ret, 1);
				}
				ret[0] = new CustomSerializedContainerElementInfo(null, "ChildSequence", null, CustomSerializedElementWriteStyle.Element, null, TwoOrMoreChildSequenceConstraintHasChildSequence.ChildSequenceDomainRoleId);
				ExclusionConstraint.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ExclusionConstraint.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(TwoOrMoreChildSequenceConstraintHasChildSequence.ChildSequenceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-01/OIALModel|ChildSequence||", match);
				ExclusionConstraint.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", ((object)containerNamespace != (object)outerContainerNamespace) ? containerNamespace : null, "|", containerName, "|", ((object)elementNamespace != (object)containerNamespace) ? elementNamespace : null, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
			}
			return rVal;
		}
		CustomSerializedElementMatch ICustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
		}
	}
	#endregion // ExclusionConstraint serialization
	#region SubsetConstraint serialization
	partial class SubsetConstraint : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo;
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
		protected new CustomSerializedContainerElementInfo[] GetCustomSerializedChildElementInfo()
		{
			CustomSerializedContainerElementInfo[] ret = SubsetConstraint.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				CustomSerializedContainerElementInfo[] baseInfo = null;
				int baseInfoCount = 0;
				if (0 != (CustomSerializedElementSupportedOperations.ChildElementInfo & base.SupportedCustomSerializedOperations))
				{
					baseInfo = base.GetCustomSerializedChildElementInfo();
					if (baseInfo != null)
					{
						baseInfoCount = baseInfo.Length;
					}
				}
				ret = new CustomSerializedContainerElementInfo[baseInfoCount + 2];
				if (baseInfoCount != 0)
				{
					baseInfo.CopyTo(ret, 2);
				}
				ret[0] = new CustomSerializedContainerElementInfo(null, "SubChildSequence", null, CustomSerializedElementWriteStyle.Element, null, SubsetConstraintHasSubChildSequence.SubChildSequenceDomainRoleId);
				ret[1] = new CustomSerializedContainerElementInfo(null, "SuperChildSequence", null, CustomSerializedElementWriteStyle.Element, null, SubsetConstraintHasSuperChildSequence.SuperChildSequenceDomainRoleId);
				SubsetConstraint.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = SubsetConstraint.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(SubsetConstraintHasSubChildSequence.SubChildSequenceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-01/OIALModel|SubChildSequence||", match);
				match.InitializeRoles(SubsetConstraintHasSuperChildSequence.SuperChildSequenceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-01/OIALModel|SuperChildSequence||", match);
				SubsetConstraint.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", ((object)containerNamespace != (object)outerContainerNamespace) ? containerNamespace : null, "|", containerName, "|", ((object)elementNamespace != (object)containerNamespace) ? elementNamespace : null, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
			}
			return rVal;
		}
		CustomSerializedElementMatch ICustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
		}
	}
	#endregion // SubsetConstraint serialization
	#region EqualityConstraint serialization
	partial class EqualityConstraint : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo;
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
		protected new CustomSerializedContainerElementInfo[] GetCustomSerializedChildElementInfo()
		{
			CustomSerializedContainerElementInfo[] ret = EqualityConstraint.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				CustomSerializedContainerElementInfo[] baseInfo = null;
				int baseInfoCount = 0;
				if (0 != (CustomSerializedElementSupportedOperations.ChildElementInfo & base.SupportedCustomSerializedOperations))
				{
					baseInfo = base.GetCustomSerializedChildElementInfo();
					if (baseInfo != null)
					{
						baseInfoCount = baseInfo.Length;
					}
				}
				ret = new CustomSerializedContainerElementInfo[baseInfoCount + 1];
				if (baseInfoCount != 0)
				{
					baseInfo.CopyTo(ret, 1);
				}
				ret[0] = new CustomSerializedContainerElementInfo(null, "ChildSequence", null, CustomSerializedElementWriteStyle.Element, null, TwoOrMoreChildSequenceConstraintHasChildSequence.ChildSequenceDomainRoleId);
				EqualityConstraint.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = EqualityConstraint.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(TwoOrMoreChildSequenceConstraintHasChildSequence.ChildSequenceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-01/OIALModel|ChildSequence||", match);
				EqualityConstraint.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", ((object)containerNamespace != (object)outerContainerNamespace) ? containerNamespace : null, "|", containerName, "|", ((object)elementNamespace != (object)containerNamespace) ? elementNamespace : null, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
			}
			return rVal;
		}
		CustomSerializedElementMatch ICustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
		}
	}
	#endregion // EqualityConstraint serialization
	#region InformationTypeFormat serialization
	partial class InformationTypeFormat : ICustomSerializedElement
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
			if (roleId == InformationTypeFormatHasObjectType.ValueTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ValueType", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == InformationType.ConceptTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ConceptType", null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConceptTypeChild.ParentDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ConceptTypeChild", null, CustomSerializedElementWriteStyle.NotWritten, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = InformationTypeFormat.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(InformationTypeFormatHasObjectType.ValueTypeDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-01/OIALModel|ValueType", match);
				InformationTypeFormat.myChildElementMappings = childElementMappings;
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
	#region ConceptType serialization
	partial class ConceptType : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.ChildElementInfo | (CustomSerializedElementSupportedOperations.LinkInfo | (CustomSerializedElementSupportedOperations.CustomSortChildRoles | CustomSerializedElementSupportedOperations.EmbeddingLinkInfo));
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
			CustomSerializedContainerElementInfo[] ret = ConceptType.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new CustomSerializedContainerElementInfo[1];
				ret[0] = new CustomSerializedContainerElementInfo(null, "ContainedTypes", null, CustomSerializedElementWriteStyle.Element, null, InformationType.InformationTypeFormatDomainRoleId, ConceptTypeAbsorbedConceptType.AbsorbedConceptTypeDomainRoleId, ConceptTypeRef.ReferencedConceptTypeDomainRoleId);
				ConceptType.myCustomSerializedChildElementInfo = ret;
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
			if (roleId == ConceptTypeHasObjectType.ObjectTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ObjectType", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == ConceptTypeAbsorbedConceptType.AbsorbingConceptTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ConceptTypeLink", null, CustomSerializedElementWriteStyle.EmbeddingLinkElement, null);
			}
			if (roleId == ConceptTypeRef.ReferencedConceptTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ReferencedConceptType", null, CustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == InformationType.InformationTypeFormatDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "InformationType", null, CustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == ConceptTypeRef.ReferencingConceptTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConceptTypeChild.ParentDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Parent", null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConceptTypeChild.TargetDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Target", null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
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
				domainRole = domainDataDirectory.FindDomainRole(ConceptTypeHasObjectType.ObjectTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(InformationType.InformationTypeFormatDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(ConceptTypeAbsorbedConceptType.AbsorbedConceptTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(ConceptTypeRef.ReferencedConceptTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(ConceptTypeAbsorbedConceptType.AbsorbingConceptTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(ConceptTypeRef.ReferencingConceptTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 3;
				domainRole = domainDataDirectory.FindDomainRole(ConceptTypeChild.ParentDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 4;
				domainRole = domainDataDirectory.FindDomainRole(ConceptTypeChild.TargetDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 5;
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
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		protected IComparer<DomainRoleInfo> CustomSerializedChildRoleComparer
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ConceptType.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ConceptTypeHasObjectType.ObjectTypeDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-01/OIALModel|ObjectType", match);
				match.InitializeRoles(ConceptTypeAbsorbedConceptType.AbsorbingConceptTypeDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-01/OIALModel|ConceptTypeLink", match);
				match.InitializeRoles(InformationType.InformationTypeFormatDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-01/OIALModel|ContainedTypes||InformationType", match);
				match.InitializeRoles(ConceptTypeRef.ReferencedConceptTypeDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-01/OIALModel|ContainedTypes||ReferencedConceptType", match);
				match.InitializeRoles(ConceptTypeAbsorbedConceptType.AbsorbedConceptTypeDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-01/OIALModel|ContainedTypes||", match);
				ConceptType.myChildElementMappings = childElementMappings;
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
		protected static bool ShouldSerialize()
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
				return CustomSerializedElementSupportedOperations.ChildElementInfo | (CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo);
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
			CustomSerializedContainerElementInfo[] ret = ConceptTypeChild.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new CustomSerializedContainerElementInfo[2];
				ret[0] = new CustomSerializedContainerElementInfo(null, "SingleChildConstraints", null, CustomSerializedElementWriteStyle.Element, null, ChildHasSingleChildConstraint.SingleChildConstraintDomainRoleId);
				ret[1] = new CustomSerializedContainerElementInfo(null, "PathRoles", null, CustomSerializedElementWriteStyle.Element, null, ConceptTypeChildHasPathRole.PathRoleDomainRoleId);
				ConceptTypeChild.myCustomSerializedChildElementInfo = ret;
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
			if (domainPropertyInfo.Id == ConceptTypeChild.MandatoryDomainPropertyId)
			{
				if (this.Mandatory == MandatoryConstraintModality.NotMandatory)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, "Mandatory", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
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
			if (roleId == ChildSequenceHasConceptTypeChild.ChildSequenceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConceptTypeChildHasPathRole.PathRoleDomainRoleId)
			{
				return new CustomSerializedElementInfo("orm", "Role", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ConceptTypeChild.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ConceptTypeChildHasPathRole.PathRoleDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-01/OIALModel|PathRoles||Role", match);
				match.InitializeRoles(ChildHasSingleChildConstraint.SingleChildConstraintDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-01/OIALModel|SingleChildConstraints||", match);
				ConceptTypeChild.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", ((object)containerNamespace != (object)outerContainerNamespace) ? containerNamespace : null, "|", containerName, "|", ((object)elementNamespace != (object)containerNamespace) ? elementNamespace : null, "|", elementName), out rVal);
			return rVal;
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
				customSerializedAttributes.Add("Mandatory", ConceptTypeChild.MandatoryDomainPropertyId);
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
}
