using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.Shell;
namespace Neumont.Tools.ORM.ShapeModel
{
	#region ORMShapeModel model serialization
	public partial class ORMShapeModel : IORMCustomSerializedMetaModel
	{
		/// <summary>
		/// Implements IORMCustomSerializedMetaModel.DefaultElementPrefix
		/// </summary>
		protected static string DefaultElementPrefix
		{
			get
			{
				return "ormDiagram";
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
			ret[0, 0] = "ormDiagram";
			ret[0, 1] = "http://schemas.neumont.edu/ORM/2006-01/ORMDiagram";
			ret[0, 2] = "ORM2Diagram.xsd";
			return ret;
		}
		string[,] IORMCustomSerializedMetaModel.GetCustomElementNamespaces()
		{
			return GetCustomElementNamespaces();
		}
		private Dictionary<MetaClassInfo, object> myCustomSerializationOmissions;
		private static Dictionary<MetaClassInfo, object> BuildCustomSerializationOmissions(Store store)
		{
			Dictionary<MetaClassInfo, object> retVal = new Dictionary<MetaClassInfo, object>();
			MetaDataDirectory dataDir = store.MetaDataDirectory;
			retVal[dataDir.FindMetaClass(ExternalConstraintLink.MetaClassGuid)] = null;
			retVal[dataDir.FindMetaClass(ValueRangeLink.MetaClassGuid)] = null;
			retVal[dataDir.FindMetaClass(RolePlayerLink.MetaClassGuid)] = null;
			retVal[dataDir.FindMetaClass(SubtypeLink.MetaClassGuid)] = null;
			retVal[dataDir.FindMetaClass(LinkConnectorShape.MetaClassGuid)] = null;
			retVal[dataDir.FindMetaRelationship(Microsoft.VisualStudio.Modeling.Diagrams.LinkConnectsToNode.MetaRelationshipGuid)] = null;
			return retVal;
		}
		private static Dictionary<string, Guid> myClassNameMap;
		private static Collection<string> myValidNamespaces;
		/// <summary>
		/// Implements IORMCustomSerializedMetaModel.ShouldSerializeMetaClass
		/// </summary>
		protected bool ShouldSerializeMetaClass(Store store, MetaClassInfo classInfo)
		{
			Dictionary<MetaClassInfo, object> omissions = this.myCustomSerializationOmissions;
			if (omissions == null)
			{
				omissions = ORMShapeModel.BuildCustomSerializationOmissions(store);
				this.myCustomSerializationOmissions = omissions;
			}
			return !(omissions.ContainsKey(classInfo));
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
				ORMDiagram.MetaClassGuid};
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
			if ((elementName == "ORMDiagram") && (xmlNamespace == "http://schemas.neumont.edu/ORM/2006-01/ORMDiagram"))
			{
				return ORMDiagram.MetaClassGuid;
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
			Collection<string> validNamespaces = ORMShapeModel.myValidNamespaces;
			Dictionary<string, Guid> classNameMap = ORMShapeModel.myClassNameMap;
			if (validNamespaces == null)
			{
				validNamespaces = new Collection<string>();
				validNamespaces.Add("http://schemas.neumont.edu/ORM/2006-01/ORMDiagram");
				ORMShapeModel.myValidNamespaces = validNamespaces;
			}
			if (classNameMap == null)
			{
				classNameMap = new Dictionary<string, Guid>();
				classNameMap.Add("ORMDiagram", ORMDiagram.MetaClassGuid);
				classNameMap.Add("ORMBaseShape", ORMBaseShape.MetaClassGuid);
				classNameMap.Add("ObjectTypeShape", ObjectTypeShape.MetaClassGuid);
				classNameMap.Add("ObjectifiedFactTypeNameShape", ObjectifiedFactTypeNameShape.MetaClassGuid);
				classNameMap.Add("ReadingShape", ReadingShape.MetaClassGuid);
				classNameMap.Add("ValueConstraintShape", ValueConstraintShape.MetaClassGuid);
				classNameMap.Add("RoleNameShape", RoleNameShape.MetaClassGuid);
				classNameMap.Add("FactTypeShape", FactTypeShape.MetaClassGuid);
				ORMShapeModel.myClassNameMap = classNameMap;
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
	#endregion // ORMShapeModel model serialization
	#region ORMDiagram serialization
	public partial class ORMDiagram : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.ChildElementInfo | (ORMCustomSerializedElementSupportedOperations.AttributeInfo | (ORMCustomSerializedElementSupportedOperations.LinkInfo | ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles));
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
			ORMCustomSerializedChildElementInfo[] ret = ORMDiagram.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new ORMCustomSerializedChildElementInfo[1];
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "Shapes", null, ORMCustomSerializedElementWriteStyle.Element, null, ParentShapeContainsNestedChildShapes.NestedChildShapesMetaRoleGuid);
				ORMDiagram.myCustomSerializedChildElementInfo = ret;
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
			if (attributeInfo.Id == ORMDiagram.DiagramIdMetaAttributeGuid)
			{
				return new ORMCustomSerializedAttributeInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
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
			if (rolePlayedInfo.Id == SubjectHasPresentation.SubjectMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Subject", null, ORMCustomSerializedElementWriteStyle.Element, null);
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
				metaRole = metaDataDir.FindMetaRole(ParentShapeContainsNestedChildShapes.NestedChildShapesMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 0;
				metaRole = metaDataDir.FindMetaRole(SubjectHasPresentation.SubjectMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 1;
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
		[CLSCompliant(false)]
		protected IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<MetaRoleInfo> retVal = ORMDiagram.myCustomSortChildComparer;
				if (null == retVal)
				{
					retVal = new CustomSortChildComparer(this.Store);
					ORMDiagram.myCustomSortChildComparer = retVal;
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
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ORMDiagram.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(SubjectHasPresentation.SubjectMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-01/ORMDiagram|Subject", match);
				match.InitializeRoles(ParentShapeContainsNestedChildShapes.NestedChildShapesMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-01/ORMDiagram|Shapes||", match);
				ORMDiagram.myChildElementMappings = childElementMappings;
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
	#endregion // ORMDiagram serialization
	#region ORMBaseShape serialization
	public partial class ORMBaseShape : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.ChildElementInfo | (ORMCustomSerializedElementSupportedOperations.AttributeInfo | (ORMCustomSerializedElementSupportedOperations.LinkInfo | ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles));
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
			ORMCustomSerializedChildElementInfo[] ret = ORMBaseShape.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new ORMCustomSerializedChildElementInfo[2];
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "RelativeShapes", null, ORMCustomSerializedElementWriteStyle.Element, null, ParentShapeHasRelativeChildShapes.RelativeChildShapesMetaRoleGuid);
				ret[1] = new ORMCustomSerializedChildElementInfo(null, "NestedShapes", null, ORMCustomSerializedElementWriteStyle.Element, null, ParentShapeContainsNestedChildShapes.NestedChildShapesMetaRoleGuid);
				ORMBaseShape.myCustomSerializedChildElementInfo = ret;
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
			if (attributeInfo.Id == ORMBaseShape.IsExpandedMetaAttributeGuid)
			{
				return new ORMCustomSerializedAttributeInfo(null, null, null, true, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (attributeInfo.Id == ORMBaseShape.AbsoluteBoundsMetaAttributeGuid)
			{
				return new ORMCustomSerializedAttributeInfo(null, null, null, true, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
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
			if (rolePlayedInfo.Id == SubjectHasPresentation.SubjectMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Subject", null, ORMCustomSerializedElementWriteStyle.Element, null);
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
				metaRole = metaDataDir.FindMetaRole(ParentShapeHasRelativeChildShapes.RelativeChildShapesMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 0;
				metaRole = metaDataDir.FindMetaRole(ParentShapeContainsNestedChildShapes.NestedChildShapesMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 1;
				metaRole = metaDataDir.FindMetaRole(SubjectHasPresentation.SubjectMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 2;
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
		[CLSCompliant(false)]
		protected IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<MetaRoleInfo> retVal = ORMBaseShape.myCustomSortChildComparer;
				if (null == retVal)
				{
					retVal = new CustomSortChildComparer(this.Store);
					ORMBaseShape.myCustomSortChildComparer = retVal;
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
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ORMBaseShape.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(SubjectHasPresentation.SubjectMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-01/ORMDiagram|Subject", match);
				match.InitializeRoles(ParentShapeHasRelativeChildShapes.RelativeChildShapesMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-01/ORMDiagram|RelativeShapes||", match);
				match.InitializeRoles(ParentShapeContainsNestedChildShapes.NestedChildShapesMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-01/ORMDiagram|NestedShapes||", match);
				ORMBaseShape.myChildElementMappings = childElementMappings;
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
			Dictionary<string, Guid> customSerializedAttributes = ORMBaseShape.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("IsExpanded", ORMBaseShape.IsExpandedMetaAttributeGuid);
				customSerializedAttributes.Add("AbsoluteBounds", ORMBaseShape.AbsoluteBoundsMetaAttributeGuid);
				ORMBaseShape.myCustomSerializedAttributes = customSerializedAttributes;
			}
			Guid rVal;
			string key = attributeName;
			if (!(xmlNamespace.Length == 0))
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
	#endregion // ORMBaseShape serialization
	#region ObjectTypeShape serialization
	public partial class ObjectTypeShape : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.AttributeInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedAttributeInfo
		/// </summary>
		protected new ORMCustomSerializedAttributeInfo GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			if (attributeInfo.Id == ObjectTypeShape.ShapeNameMetaAttributeGuid)
			{
				return new ORMCustomSerializedAttributeInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
			}
			if (attributeInfo.Id == ObjectTypeShape.ReferenceModeNameMetaAttributeGuid)
			{
				return new ORMCustomSerializedAttributeInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.AttributeInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedAttributeInfo(attributeInfo, rolePlayedInfo);
			}
			return ORMCustomSerializedAttributeInfo.Default;
		}
		ORMCustomSerializedAttributeInfo IORMCustomSerializedElement.GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedAttributeInfo(attributeInfo, rolePlayedInfo);
		}
	}
	#endregion // ObjectTypeShape serialization
	#region ObjectifiedFactTypeNameShape serialization
	public partial class ObjectifiedFactTypeNameShape : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.AttributeInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedAttributeInfo
		/// </summary>
		protected new ORMCustomSerializedAttributeInfo GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			if (attributeInfo.Id == ObjectifiedFactTypeNameShape.ObjectTypeNameMetaAttributeGuid)
			{
				return new ORMCustomSerializedAttributeInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.AttributeInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedAttributeInfo(attributeInfo, rolePlayedInfo);
			}
			return ORMCustomSerializedAttributeInfo.Default;
		}
		ORMCustomSerializedAttributeInfo IORMCustomSerializedElement.GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedAttributeInfo(attributeInfo, rolePlayedInfo);
		}
	}
	#endregion // ObjectifiedFactTypeNameShape serialization
	#region ReadingShape serialization
	public partial class ReadingShape : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.AttributeInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedAttributeInfo
		/// </summary>
		protected new ORMCustomSerializedAttributeInfo GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			if (attributeInfo.Id == ReadingShape.ReadingTextMetaAttributeGuid)
			{
				return new ORMCustomSerializedAttributeInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.AttributeInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedAttributeInfo(attributeInfo, rolePlayedInfo);
			}
			return ORMCustomSerializedAttributeInfo.Default;
		}
		ORMCustomSerializedAttributeInfo IORMCustomSerializedElement.GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedAttributeInfo(attributeInfo, rolePlayedInfo);
		}
	}
	#endregion // ReadingShape serialization
	#region ValueConstraintShape serialization
	public partial class ValueConstraintShape : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.AttributeInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedAttributeInfo
		/// </summary>
		protected new ORMCustomSerializedAttributeInfo GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			if (attributeInfo.Id == ValueConstraintShape.ValueRangeTextMetaAttributeGuid)
			{
				return new ORMCustomSerializedAttributeInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.AttributeInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedAttributeInfo(attributeInfo, rolePlayedInfo);
			}
			return ORMCustomSerializedAttributeInfo.Default;
		}
		ORMCustomSerializedAttributeInfo IORMCustomSerializedElement.GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedAttributeInfo(attributeInfo, rolePlayedInfo);
		}
	}
	#endregion // ValueConstraintShape serialization
	#region RoleNameShape serialization
	public partial class RoleNameShape : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.AttributeInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedAttributeInfo
		/// </summary>
		protected new ORMCustomSerializedAttributeInfo GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			if (attributeInfo.Id == RoleNameShape.RoleNameMetaAttributeGuid)
			{
				return new ORMCustomSerializedAttributeInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.AttributeInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedAttributeInfo(attributeInfo, rolePlayedInfo);
			}
			return ORMCustomSerializedAttributeInfo.Default;
		}
		ORMCustomSerializedAttributeInfo IORMCustomSerializedElement.GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedAttributeInfo(attributeInfo, rolePlayedInfo);
		}
	}
	#endregion // RoleNameShape serialization
	#region FactTypeShape serialization
	public partial class FactTypeShape : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.AttributeInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedAttributeInfo
		/// </summary>
		protected new ORMCustomSerializedAttributeInfo GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			if (attributeInfo.Id == FactTypeShape.DisplayRoleNamesMetaAttributeGuid)
			{
				if (this.DisplayRoleNames == DisplayRoleNames.UserDefault)
				{
					return new ORMCustomSerializedAttributeInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new ORMCustomSerializedAttributeInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.AttributeInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedAttributeInfo(attributeInfo, rolePlayedInfo);
			}
			return ORMCustomSerializedAttributeInfo.Default;
		}
		ORMCustomSerializedAttributeInfo IORMCustomSerializedElement.GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedAttributeInfo(attributeInfo, rolePlayedInfo);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapAttribute
		/// </summary>
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = FactTypeShape.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("DisplayRoleNames", FactTypeShape.DisplayRoleNamesMetaAttributeGuid);
				FactTypeShape.myCustomSerializedAttributes = customSerializedAttributes;
			}
			Guid rVal;
			string key = attributeName;
			if (!(xmlNamespace.Length == 0))
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
	#endregion // FactTypeShape serialization
}
