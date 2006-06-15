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
namespace Neumont.Tools.ORM.OIALModel
{
	#region OIALMetaModel model serialization
	[MetaModelDisplayName(typeof(OIALMetaModel), "DisplayName")]
	[MetaModelDescription(typeof(OIALMetaModel), "Description")]
	public partial class OIALMetaModel : IORMCustomSerializedMetaModel
	{
		/// <summary>
		/// The default XmlNamespace associated with the 'OIALMetaModel' extension model
		/// </summary>
		public const string XmlNamespace = "http://schemas.neumont.edu/ORM/2006-01/OIALModel";
		/// <summary>
		/// Implements IORMCustomSerializedMetaModel.DefaultElementPrefix
		/// </summary>
		protected static string DefaultElementPrefix
		{
			get
			{
				return "oial";
			}
		}
		string IORMCustomSerializedMetaModel.DefaultElementPrefix
		{
			get
			{
				return DefaultElementPrefix;
			}
		}
		/// <summary>
		/// Implements IORMCustomSerializedMetaModel.GetCustomElementNamespaces
		/// </summary>
		protected static string[,] GetCustomElementNamespaces()
		{
			string[,] ret = new string[1, 3];
			ret[0, 0] = "oial";
			ret[0, 1] = "http://schemas.neumont.edu/ORM/2006-01/OIALModel";
			ret[0, 2] = "OIALModel.xsd";
			return ret;
		}
		string[,] IORMCustomSerializedMetaModel.GetCustomElementNamespaces()
		{
			return GetCustomElementNamespaces();
		}
		private static Dictionary<string, Guid> myClassNameMap;
		private static Collection<string> myValidNamespaces;
		/// <summary>
		/// Implements IORMCustomSerializedMetaModel.ShouldSerializeMetaClass
		/// </summary>
		protected bool ShouldSerializeMetaClass(Store store, MetaClassInfo classInfo)
		{
			return true;
		}
		bool IORMCustomSerializedMetaModel.ShouldSerializeMetaClass(Store store, MetaClassInfo classInfo)
		{
			return this.ShouldSerializeMetaClass(store, classInfo);
		}
		/// <summary>
		/// Implements IORMCustomSerializedMetaModel.GetRootElementClasses
		/// </summary>
		protected static Guid[] GetRootElementClasses()
		{
			return new Guid[]{
				OIALModel.MetaClassGuid};
		}
		Guid[] IORMCustomSerializedMetaModel.GetRootElementClasses()
		{
			return GetRootElementClasses();
		}
		/// <summary>
		/// Implements IORMCustomSerializedMetaModel.MapRootElement
		/// </summary>
		protected static Guid MapRootElement(string xmlNamespace, string elementName)
		{
			if ((elementName == "Model") && (xmlNamespace == "http://schemas.neumont.edu/ORM/2006-01/OIALModel"))
			{
				return OIALModel.MetaClassGuid;
			}
			return default(Guid);
		}
		Guid IORMCustomSerializedMetaModel.MapRootElement(string xmlNamespace, string elementName)
		{
			return MapRootElement(xmlNamespace, elementName);
		}
		/// <summary>
		/// Implements IORMCustomSerializedMetaModel.MapClassName
		/// </summary>
		protected static Guid MapClassName(string xmlNamespace, string elementName)
		{
			Collection<string> validNamespaces = OIALMetaModel.myValidNamespaces;
			Dictionary<string, Guid> classNameMap = OIALMetaModel.myClassNameMap;
			if (validNamespaces == null)
			{
				validNamespaces = new Collection<string>();
				validNamespaces.Add("http://schemas.neumont.edu/ORM/2006-01/OIALModel");
				OIALMetaModel.myValidNamespaces = validNamespaces;
			}
			if (classNameMap == null)
			{
				classNameMap = new Dictionary<string, Guid>();
				classNameMap.Add("Model", OIALModel.MetaClassGuid);
				classNameMap.Add("ChildSequenceConstraint", ChildSequenceConstraint.MetaClassGuid);
				classNameMap.Add("ChildSequence", ChildSequence.MetaClassGuid);
				classNameMap.Add("InformationTypeFormat", InformationTypeFormat.MetaClassGuid);
				classNameMap.Add("ConceptType", ConceptType.MetaClassGuid);
				classNameMap.Add("InformationType", InformationType.MetaClassGuid);
				classNameMap.Add("ConceptTypeHasChild", ConceptTypeHasChild.MetaClassGuid);
				OIALMetaModel.myClassNameMap = classNameMap;
			}
			if (validNamespaces.Contains(xmlNamespace) && classNameMap.ContainsKey(elementName))
			{
				return classNameMap[elementName];
			}
			return default(Guid);
		}
		Guid IORMCustomSerializedMetaModel.MapClassName(string xmlNamespace, string elementName)
		{
			return MapClassName(xmlNamespace, elementName);
		}
	}
	#endregion // OIALMetaModel model serialization
	#region OIALModel serialization
	public partial class OIALModel : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.ChildElementInfo | (ORMCustomSerializedElementSupportedOperations.ElementInfo | (ORMCustomSerializedElementSupportedOperations.LinkInfo | ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles));
			}
		}
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		private static ORMCustomSerializedChildElementInfo[] myCustomSerializedChildElementInfo;
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo
		/// </summary>
		protected ORMCustomSerializedChildElementInfo[] GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedChildElementInfo[] ret = OIALModel.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new ORMCustomSerializedChildElementInfo[3];
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "ChildSequenceConstraints", null, ORMCustomSerializedElementWriteStyle.Element, null, OIALModelHasChildSequenceConstraint.ChildSequenceConstraintCollectionMetaRoleGuid);
				ret[1] = new ORMCustomSerializedChildElementInfo(null, "InformationTypeFormats", null, ORMCustomSerializedElementWriteStyle.Element, null, OIALHasInformationTypeFormat.InformationTypeFormatCollectionMetaRoleGuid);
				ret[2] = new ORMCustomSerializedChildElementInfo(null, "ConceptTypes", null, ORMCustomSerializedElementWriteStyle.Element, null, OIALModelHasConceptType.ConceptTypeCollectionMetaRoleGuid);
				OIALModel.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		ORMCustomSerializedChildElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.CustomSerializedElementInfo
		/// </summary>
		protected ORMCustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new ORMCustomSerializedElementInfo(null, "Model", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return this.CustomSerializedElementInfo;
			}
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedAttributeInfo
		/// </summary>
		protected ORMCustomSerializedAttributeInfo GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			throw new NotSupportedException();
		}
		ORMCustomSerializedAttributeInfo IORMCustomSerializedElement.GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedAttributeInfo(attributeInfo, rolePlayedInfo);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == OIALModelHasORMModel.ORMModelMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ORMModel", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static IComparer<MetaRoleInfo> myCustomSortChildComparer;
		private class CustomSortChildComparer : IComparer<MetaRoleInfo>
		{
			private Dictionary<string, int> myRoleOrderDictionary;
			public CustomSortChildComparer(Store store)
			{
				MetaDataDirectory metaDataDir = store.MetaDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				MetaRoleInfo metaRole;
				metaRole = metaDataDir.FindMetaRole(OIALModelHasORMModel.ORMModelMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 0;
				metaRole = metaDataDir.FindMetaRole(OIALModelHasChildSequenceConstraint.ChildSequenceConstraintCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 1;
				metaRole = metaDataDir.FindMetaRole(OIALHasInformationTypeFormat.InformationTypeFormatCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 2;
				metaRole = metaDataDir.FindMetaRole(OIALModelHasConceptType.ConceptTypeCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 3;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<MetaRoleInfo>.Compare(MetaRoleInfo x, MetaRoleInfo y)
			{
				int xPos;
				if (!(this.myRoleOrderDictionary.TryGetValue(x.FullName, out xPos)))
				{
					xPos = int.MaxValue;
				}
				int yPos;
				if (!(this.myRoleOrderDictionary.TryGetValue(y.FullName, out yPos)))
				{
					yPos = int.MaxValue;
				}
				if (xPos == yPos)
				{
					return 0;
				}
				else if (xPos < yPos)
				{
					return -1;
				}
				return 1;
			}
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		/// </summary>
		protected IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<MetaRoleInfo> retVal = OIALModel.myCustomSortChildComparer;
				if (null == retVal)
				{
					retVal = new CustomSortChildComparer(this.Store);
					OIALModel.myCustomSortChildComparer = retVal;
				}
				return retVal;
			}
		}
		IComparer<MetaRoleInfo> IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		{
			get
			{
				return this.CustomSerializedChildRoleComparer;
			}
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = OIALModel.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(OIALModelHasORMModel.ORMModelMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-01/OIALModel|ORMModel", match);
				match.InitializeRoles(OIALModelHasChildSequenceConstraint.ChildSequenceConstraintCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-01/OIALModel|ChildSequenceConstraints||", match);
				match.InitializeRoles(OIALHasInformationTypeFormat.InformationTypeFormatCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-01/OIALModel|InformationTypeFormats||", match);
				match.InitializeRoles(OIALModelHasConceptType.ConceptTypeCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-01/OIALModel|ConceptTypes||", match);
				OIALModel.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal);
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapAttribute
		/// </summary>
		protected Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			return default(Guid);
		}
		Guid IORMCustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.ShouldSerialize
		/// </summary>
		protected static bool ShouldSerialize()
		{
			return true;
		}
		bool IORMCustomSerializedElement.ShouldSerialize()
		{
			return ShouldSerialize();
		}
	}
	#endregion // OIALModel serialization
	#region ChildSequenceConstraint serialization
	public partial class ChildSequenceConstraint : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.ChildElementInfo | ORMCustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		private static ORMCustomSerializedChildElementInfo[] myCustomSerializedChildElementInfo;
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo
		/// </summary>
		protected ORMCustomSerializedChildElementInfo[] GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedChildElementInfo[] ret = ChildSequenceConstraint.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new ORMCustomSerializedChildElementInfo[1];
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "ChildSequence", null, ORMCustomSerializedElementWriteStyle.Element, null, SingleChildSequenceConstraintHasMinTwoChildrenChildSequence.ChildSequenceMetaRoleGuid);
				ChildSequenceConstraint.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		ORMCustomSerializedChildElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.CustomSerializedElementInfo
		/// </summary>
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
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedAttributeInfo
		/// </summary>
		protected ORMCustomSerializedAttributeInfo GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			throw new NotSupportedException();
		}
		ORMCustomSerializedAttributeInfo IORMCustomSerializedElement.GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedAttributeInfo(attributeInfo, rolePlayedInfo);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ChildHasSingleChildConstraint.ConceptTypeHasChildMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		/// </summary>
		protected IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				return null;
			}
		}
		IComparer<MetaRoleInfo> IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		{
			get
			{
				return this.CustomSerializedChildRoleComparer;
			}
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ChildSequenceConstraint.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(SingleChildSequenceConstraintHasMinTwoChildrenChildSequence.ChildSequenceMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-01/OIALModel|ChildSequence||", match);
				ChildSequenceConstraint.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal);
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapAttribute
		/// </summary>
		protected Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			return default(Guid);
		}
		Guid IORMCustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.ShouldSerialize
		/// </summary>
		protected static bool ShouldSerialize()
		{
			return true;
		}
		bool IORMCustomSerializedElement.ShouldSerialize()
		{
			return ShouldSerialize();
		}
	}
	#endregion // ChildSequenceConstraint serialization
	#region ChildSequence serialization
	public partial class ChildSequence : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo
		/// </summary>
		protected ORMCustomSerializedChildElementInfo[] GetCustomSerializedChildElementInfo()
		{
			throw new NotSupportedException();
		}
		ORMCustomSerializedChildElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.CustomSerializedElementInfo
		/// </summary>
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
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedAttributeInfo
		/// </summary>
		protected ORMCustomSerializedAttributeInfo GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			throw new NotSupportedException();
		}
		ORMCustomSerializedAttributeInfo IORMCustomSerializedElement.GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedAttributeInfo(attributeInfo, rolePlayedInfo);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ChildSequenceHasConceptTypeHasChild.ConceptTypeHasChildCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ConceptTypeHasChild", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		/// </summary>
		protected IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				return null;
			}
		}
		IComparer<MetaRoleInfo> IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		{
			get
			{
				return this.CustomSerializedChildRoleComparer;
			}
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ChildSequence.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ChildSequenceHasConceptTypeHasChild.ConceptTypeHasChildCollectionMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-01/OIALModel|ConceptTypeHasChild", match);
				ChildSequence.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal);
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapAttribute
		/// </summary>
		protected Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			return default(Guid);
		}
		Guid IORMCustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.ShouldSerialize
		/// </summary>
		protected static bool ShouldSerialize()
		{
			return true;
		}
		bool IORMCustomSerializedElement.ShouldSerialize()
		{
			return ShouldSerialize();
		}
	}
	#endregion // ChildSequence serialization
	#region InformationTypeFormat serialization
	public partial class InformationTypeFormat : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo
		/// </summary>
		protected ORMCustomSerializedChildElementInfo[] GetCustomSerializedChildElementInfo()
		{
			throw new NotSupportedException();
		}
		ORMCustomSerializedChildElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.CustomSerializedElementInfo
		/// </summary>
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
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedAttributeInfo
		/// </summary>
		protected ORMCustomSerializedAttributeInfo GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			throw new NotSupportedException();
		}
		ORMCustomSerializedAttributeInfo IORMCustomSerializedElement.GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedAttributeInfo(attributeInfo, rolePlayedInfo);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == InformationTypeHasInformationTypeFormat.InformationTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == InformationTypeFormatHasObjectType.ValueTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ValueType", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		/// </summary>
		protected IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				return null;
			}
		}
		IComparer<MetaRoleInfo> IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		{
			get
			{
				return this.CustomSerializedChildRoleComparer;
			}
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = InformationTypeFormat.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(InformationTypeFormatHasObjectType.ValueTypeMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-01/OIALModel|ValueType", match);
				InformationTypeFormat.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal);
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapAttribute
		/// </summary>
		protected Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			return default(Guid);
		}
		Guid IORMCustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.ShouldSerialize
		/// </summary>
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
	#region ConceptType serialization
	public partial class ConceptType : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.ChildElementInfo | (ORMCustomSerializedElementSupportedOperations.LinkInfo | (ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles | ORMCustomSerializedElementSupportedOperations.AggregatingLinkInfo));
			}
		}
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		private static ORMCustomSerializedChildElementInfo[] myCustomSerializedChildElementInfo;
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo
		/// </summary>
		protected ORMCustomSerializedChildElementInfo[] GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedChildElementInfo[] ret = ConceptType.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new ORMCustomSerializedChildElementInfo[1];
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "ContainedTypes", null, ORMCustomSerializedElementWriteStyle.Element, null, ConceptTypeHasInformationType.InformationTypeCollectionMetaRoleGuid, ConceptTypeAbsorbedConceptType.AbsorbedConceptTypeCollectionMetaRoleGuid, ConceptTypeRef.ReferencedConceptTypeCollectionMetaRoleGuid);
				ConceptType.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		ORMCustomSerializedChildElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.CustomSerializedElementInfo
		/// </summary>
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
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedAttributeInfo
		/// </summary>
		protected ORMCustomSerializedAttributeInfo GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			throw new NotSupportedException();
		}
		ORMCustomSerializedAttributeInfo IORMCustomSerializedElement.GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedAttributeInfo(attributeInfo, rolePlayedInfo);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ConceptTypeHasObjectType.ObjectTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ObjectType", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == ConceptTypeAbsorbedConceptType.AbsorbingConceptTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ConceptTypeLink", null, ORMCustomSerializedElementWriteStyle.AggregatingLinkElement, null);
			}
			if (roleId == ConceptTypeRef.ReferencedConceptTypeCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ReferencedConceptType", null, ORMCustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == ConceptTypeRef.ReferencingConceptTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static IComparer<MetaRoleInfo> myCustomSortChildComparer;
		private class CustomSortChildComparer : IComparer<MetaRoleInfo>
		{
			private Dictionary<string, int> myRoleOrderDictionary;
			public CustomSortChildComparer(Store store)
			{
				MetaDataDirectory metaDataDir = store.MetaDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				MetaRoleInfo metaRole;
				metaRole = metaDataDir.FindMetaRole(ConceptTypeHasObjectType.ObjectTypeMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 0;
				metaRole = metaDataDir.FindMetaRole(ConceptTypeHasInformationType.InformationTypeCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 1;
				metaRole = metaDataDir.FindMetaRole(ConceptTypeAbsorbedConceptType.AbsorbedConceptTypeCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 1;
				metaRole = metaDataDir.FindMetaRole(ConceptTypeRef.ReferencedConceptTypeCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 1;
				metaRole = metaDataDir.FindMetaRole(ConceptTypeAbsorbedConceptType.AbsorbingConceptTypeMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 2;
				metaRole = metaDataDir.FindMetaRole(ConceptTypeRef.ReferencingConceptTypeMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 3;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<MetaRoleInfo>.Compare(MetaRoleInfo x, MetaRoleInfo y)
			{
				int xPos;
				if (!(this.myRoleOrderDictionary.TryGetValue(x.FullName, out xPos)))
				{
					xPos = int.MaxValue;
				}
				int yPos;
				if (!(this.myRoleOrderDictionary.TryGetValue(y.FullName, out yPos)))
				{
					yPos = int.MaxValue;
				}
				if (xPos == yPos)
				{
					return 0;
				}
				else if (xPos < yPos)
				{
					return -1;
				}
				return 1;
			}
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		/// </summary>
		protected IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<MetaRoleInfo> retVal = ConceptType.myCustomSortChildComparer;
				if (null == retVal)
				{
					retVal = new CustomSortChildComparer(this.Store);
					ConceptType.myCustomSortChildComparer = retVal;
				}
				return retVal;
			}
		}
		IComparer<MetaRoleInfo> IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		{
			get
			{
				return this.CustomSerializedChildRoleComparer;
			}
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ConceptType.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ConceptTypeHasObjectType.ObjectTypeMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-01/OIALModel|ObjectType", match);
				match.InitializeRoles(ConceptTypeAbsorbedConceptType.AbsorbingConceptTypeMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-01/OIALModel|ConceptTypeLink", match);
				match.InitializeRoles(ConceptTypeRef.ReferencedConceptTypeCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-01/OIALModel|ContainedTypes|http://schemas.neumont.edu/ORM/2006-01/OIALModel|ReferencedConceptType", match);
				match.InitializeRoles(ConceptTypeHasInformationType.InformationTypeCollectionMetaRoleGuid, ConceptTypeAbsorbedConceptType.AbsorbedConceptTypeCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-01/OIALModel|ContainedTypes||", match);
				ConceptType.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal);
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapAttribute
		/// </summary>
		protected Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			return default(Guid);
		}
		Guid IORMCustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.ShouldSerialize
		/// </summary>
		protected static bool ShouldSerialize()
		{
			return true;
		}
		bool IORMCustomSerializedElement.ShouldSerialize()
		{
			return ShouldSerialize();
		}
	}
	#endregion // ConceptType serialization
	#region InformationType serialization
	public partial class InformationType : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.LinkInfo | ORMCustomSerializedElementSupportedOperations.AggregatingLinkInfo;
			}
		}
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo
		/// </summary>
		protected ORMCustomSerializedChildElementInfo[] GetCustomSerializedChildElementInfo()
		{
			throw new NotSupportedException();
		}
		ORMCustomSerializedChildElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.CustomSerializedElementInfo
		/// </summary>
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
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedAttributeInfo
		/// </summary>
		protected ORMCustomSerializedAttributeInfo GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			throw new NotSupportedException();
		}
		ORMCustomSerializedAttributeInfo IORMCustomSerializedElement.GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedAttributeInfo(attributeInfo, rolePlayedInfo);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == InformationTypeHasInformationTypeFormat.InformationTypeFormatMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "InformationTypeFormat", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == ConceptTypeHasInformationType.ConceptTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "InformationTypeLink", null, ORMCustomSerializedElementWriteStyle.AggregatingLinkElement, null);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		/// </summary>
		protected IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				return null;
			}
		}
		IComparer<MetaRoleInfo> IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		{
			get
			{
				return this.CustomSerializedChildRoleComparer;
			}
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = InformationType.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(InformationTypeHasInformationTypeFormat.InformationTypeFormatMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-01/OIALModel|InformationTypeFormat", match);
				match.InitializeRoles(ConceptTypeHasInformationType.ConceptTypeMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-01/OIALModel|InformationTypeLink", match);
				InformationType.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal);
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapAttribute
		/// </summary>
		protected Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			return default(Guid);
		}
		Guid IORMCustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.ShouldSerialize
		/// </summary>
		protected static bool ShouldSerialize()
		{
			return true;
		}
		bool IORMCustomSerializedElement.ShouldSerialize()
		{
			return ShouldSerialize();
		}
	}
	#endregion // InformationType serialization
	#region ConceptTypeHasChild serialization
	public partial class ConceptTypeHasChild : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.ChildElementInfo | (ORMCustomSerializedElementSupportedOperations.AttributeInfo | ORMCustomSerializedElementSupportedOperations.LinkInfo);
			}
		}
		ORMCustomSerializedElementSupportedOperations IORMCustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		private static ORMCustomSerializedChildElementInfo[] myCustomSerializedChildElementInfo;
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo
		/// </summary>
		protected ORMCustomSerializedChildElementInfo[] GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedChildElementInfo[] ret = ConceptTypeHasChild.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new ORMCustomSerializedChildElementInfo[2];
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "SingleChildConstraints", null, ORMCustomSerializedElementWriteStyle.Element, null, ChildHasSingleChildConstraint.SingleChildConstraintCollectionMetaRoleGuid);
				ret[1] = new ORMCustomSerializedChildElementInfo(null, "PathRoles", null, ORMCustomSerializedElementWriteStyle.Element, null, ConceptTypeHasChildHasPathRole.PathRoleCollectionMetaRoleGuid);
				ConceptTypeHasChild.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		ORMCustomSerializedChildElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.CustomSerializedElementInfo
		/// </summary>
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
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedAttributeInfo
		/// </summary>
		protected ORMCustomSerializedAttributeInfo GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			if (attributeInfo.Id == ConceptTypeHasChild.MandatoryMetaAttributeGuid)
			{
				if (this.Mandatory == MandatoryConstraintModality.NotMandatory)
				{
					return new ORMCustomSerializedAttributeInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new ORMCustomSerializedAttributeInfo(null, "Mandatory", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			return ORMCustomSerializedAttributeInfo.Default;
		}
		ORMCustomSerializedAttributeInfo IORMCustomSerializedElement.GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedAttributeInfo(attributeInfo, rolePlayedInfo);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ChildSequenceHasConceptTypeHasChild.ChildSequenceCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConceptTypeHasChildHasPathRole.PathRoleCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo("orm", "Role", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		/// </summary>
		protected IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				return null;
			}
		}
		IComparer<MetaRoleInfo> IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		{
			get
			{
				return this.CustomSerializedChildRoleComparer;
			}
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ConceptTypeHasChild.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ConceptTypeHasChildHasPathRole.PathRoleCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-01/OIALModel|PathRoles|http://schemas.neumont.edu/ORM/2006-01/OIALModel|Role", match);
				match.InitializeRoles(ChildHasSingleChildConstraint.SingleChildConstraintCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-01/OIALModel|SingleChildConstraints||", match);
				ConceptTypeHasChild.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal);
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapAttribute
		/// </summary>
		protected Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = ConceptTypeHasChild.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("Mandatory", ConceptTypeHasChild.MandatoryMetaAttributeGuid);
				ConceptTypeHasChild.myCustomSerializedAttributes = customSerializedAttributes;
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
		/// <summary>
		/// Implements IORMCustomSerializedElement.ShouldSerialize
		/// </summary>
		protected static bool ShouldSerialize()
		{
			return true;
		}
		bool IORMCustomSerializedElement.ShouldSerialize()
		{
			return ShouldSerialize();
		}
	}
	#endregion // ConceptTypeHasChild serialization
}
