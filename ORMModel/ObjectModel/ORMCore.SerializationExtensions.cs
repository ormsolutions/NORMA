using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Neumont.Tools.ORM.Shell;
namespace Neumont.Tools.ORM.ObjectModel
{
	#region ORMMetaModel model serialization
	public partial class ORMMetaModel : IORMCustomSerializedMetaModel
	{
		/// <summary>
		/// Implements IORMCustomSerializedMetaModel.DefaultElementPrefix
		/// </summary>
		protected static string DefaultElementPrefix
		{
			get
			{
				return "orm";
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
			ret[0, 0] = "orm";
			ret[0, 1] = "http://schemas.neumont.edu/ORM/ORMCore";
			ret[0, 2] = "ORM2Core.xsd";
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
			retVal[dataDir.FindMetaRelationship(MultiColumnExternalFactConstraint.MetaRelationshipGuid)] = null;
			retVal[dataDir.FindMetaRelationship(SingleColumnExternalFactConstraint.MetaRelationshipGuid)] = null;
			retVal[dataDir.FindMetaRelationship(ExternalRoleConstraint.MetaRelationshipGuid)] = null;
			retVal[dataDir.FindMetaClass(IntrinsicReferenceMode.MetaClassGuid)] = null;
			retVal[dataDir.FindMetaRelationship(Microsoft.VisualStudio.Modeling.SubjectHasPresentation.MetaRelationshipGuid)] = null;
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
				omissions = ORMMetaModel.BuildCustomSerializationOmissions(store);
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
				ORMModel.MetaClassGuid};
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
			if ((elementName == "ORMModel") && (xmlNamespace == "http://schemas.neumont.edu/ORM/ORMCore"))
			{
				return ORMModel.MetaClassGuid;
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
			Collection<string> validNamespaces = ORMMetaModel.myValidNamespaces;
			Dictionary<string, Guid> classNameMap = ORMMetaModel.myClassNameMap;
			if (validNamespaces == null)
			{
				validNamespaces = new Collection<string>();
				validNamespaces.Add("http://schemas.neumont.edu/ORM/ORMCore");
				ORMMetaModel.myValidNamespaces = validNamespaces;
			}
			if (classNameMap == null)
			{
				classNameMap = new Dictionary<string, Guid>();
				classNameMap.Add("ORMNamedElement", ORMNamedElement.MetaClassGuid);
				classNameMap.Add("ORMModelElement", ORMModelElement.MetaClassGuid);
				classNameMap.Add("ORMModel", ORMModel.MetaClassGuid);
				classNameMap.Add("EntityType", ObjectType.MetaClassGuid);
				classNameMap.Add("ValueType", ObjectType.MetaClassGuid);
				classNameMap.Add("ObjectifiedType", ObjectType.MetaClassGuid);
				classNameMap.Add("CustomReferenceMode", CustomReferenceMode.MetaClassGuid);
				classNameMap.Add("ValueTypeHasDataType", ValueTypeHasDataType.MetaClassGuid);
				classNameMap.Add("DataType", DataType.MetaClassGuid);
				classNameMap.Add("ReferenceModeKind", ReferenceModeKind.MetaClassGuid);
				classNameMap.Add("BaseValueRangeDefinition", ValueConstraint.MetaClassGuid);
				classNameMap.Add("ValueRangeDefinition", ValueTypeValueConstraint.MetaClassGuid);
				classNameMap.Add("RoleValueRangeDefinition", RoleValueConstraint.MetaClassGuid);
				classNameMap.Add("ValueRange", ValueRange.MetaClassGuid);
				classNameMap.Add("Fact", FactType.MetaClassGuid);
				classNameMap.Add("ImpliedFact", FactType.MetaClassGuid);
				classNameMap.Add("SubtypeFact", SubtypeFact.MetaClassGuid);
				classNameMap.Add("Objectification", Objectification.MetaClassGuid);
				classNameMap.Add("ReadingOrder", ReadingOrder.MetaClassGuid);
				classNameMap.Add("Reading", Reading.MetaClassGuid);
				classNameMap.Add("Role", Role.MetaClassGuid);
				classNameMap.Add("MultiColumnExternalConstraint", MultiColumnExternalConstraint.MetaClassGuid);
				classNameMap.Add("RoleSequence", MultiColumnExternalConstraintRoleSequence.MetaClassGuid);
				classNameMap.Add("SingleColumnExternalConstraint", SingleColumnExternalConstraint.MetaClassGuid);
				classNameMap.Add("DisjunctiveMandatoryConstraint", DisjunctiveMandatoryConstraint.MetaClassGuid);
				classNameMap.Add("FrequencyConstraint", FrequencyConstraint.MetaClassGuid);
				classNameMap.Add("ExternalUniquenessConstraint", ExternalUniquenessConstraint.MetaClassGuid);
				classNameMap.Add("ImpliedExternalUniquenessConstraint", ExternalUniquenessConstraint.MetaClassGuid);
				classNameMap.Add("InternalConstraint", InternalConstraint.MetaClassGuid);
				classNameMap.Add("InternalUniquenessConstraint", InternalUniquenessConstraint.MetaClassGuid);
				classNameMap.Add("EqualityConstraint", EqualityConstraint.MetaClassGuid);
				classNameMap.Add("ImpliedEqualityConstraint", EqualityConstraint.MetaClassGuid);
				classNameMap.Add("RingConstraint", RingConstraint.MetaClassGuid);
				classNameMap.Add("ConstraintDuplicateNameError", ConstraintDuplicateNameError.MetaClassGuid);
				classNameMap.Add("FactTypeDuplicateNameError", FactTypeDuplicateNameError.MetaClassGuid);
				classNameMap.Add("ObjectTypeDuplicateNameError", ObjectTypeDuplicateNameError.MetaClassGuid);
				classNameMap.Add("EntityTypeRequiresReferenceSchemeError", EntityTypeRequiresReferenceSchemeError.MetaClassGuid);
				classNameMap.Add("ExternalConstraintRoleSequenceArityMismatchError", ExternalConstraintRoleSequenceArityMismatchError.MetaClassGuid);
				classNameMap.Add("ImpliedInternalUniquenessConstraintError", ImpliedInternalUniquenessConstraintError.MetaClassGuid);
				classNameMap.Add("FactTypeRequiresInternalUniquenessConstraintError", FactTypeRequiresInternalUniquenessConstraintError.MetaClassGuid);
				classNameMap.Add("FactTypeRequiresReadingError", FactTypeRequiresReadingError.MetaClassGuid);
				classNameMap.Add("FrequencyConstraintMinMaxError", FrequencyConstraintMinMaxError.MetaClassGuid);
				classNameMap.Add("MinValueMismatchError", MinValueMismatchError.MetaClassGuid);
				classNameMap.Add("MaxValueMismatchError", MaxValueMismatchError.MetaClassGuid);
				classNameMap.Add("RingConstraintTypeNotSpecifiedError", RingConstraintTypeNotSpecifiedError.MetaClassGuid);
				classNameMap.Add("TooFewReadingRolesError", TooFewReadingRolesError.MetaClassGuid);
				classNameMap.Add("TooFewRoleSequencesError", TooFewRoleSequencesError.MetaClassGuid);
				classNameMap.Add("TooManyReadingRolesError", TooManyReadingRolesError.MetaClassGuid);
				classNameMap.Add("TooManyRoleSequencesError", TooManyRoleSequencesError.MetaClassGuid);
				classNameMap.Add("DataTypeNotSpecifiedError", DataTypeNotSpecifiedError.MetaClassGuid);
				classNameMap.Add("EqualityIsImpliedByMandatoryError", EqualityIsImpliedByMandatoryError.MetaClassGuid);
				classNameMap.Add("NMinusOneError", NMinusOneError.MetaClassGuid);
				classNameMap.Add("SimpleMandatoryImpliesDisjunctiveMandatoryError", SimpleMandatoryImpliesDisjunctiveMandatoryError.MetaClassGuid);
				classNameMap.Add("CompatibleRolePlayerTypeError", CompatibleRolePlayerTypeError.MetaClassGuid);
				classNameMap.Add("RolePlayerRequiredError", RolePlayerRequiredError.MetaClassGuid);
				classNameMap.Add("FrequencyConstraintContradictsInternalUniquenessConstraintError", FrequencyConstraintContradictsInternalUniquenessConstraintError.MetaClassGuid);
				ORMMetaModel.myClassNameMap = classNameMap;
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
	#endregion // ORMMetaModel model serialization
	#region ORMNamedElement serialization
	public partial class ORMNamedElement : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
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
		private static ORMCustomSerializedChildElementInfo[] myCustomSerializedChildElementInfo;
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo
		/// </summary>
		protected ORMCustomSerializedChildElementInfo[] GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedChildElementInfo[] ret = ORMNamedElement.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new ORMCustomSerializedChildElementInfo[1];
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "Extensions", null, ORMCustomSerializedElementWriteStyle.Element, null, ORMNamedElementHasExtensionElement.ExtensionCollectionMetaRoleGuid);
				ORMNamedElement.myCustomSerializedChildElementInfo = ret;
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
		protected ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			throw new NotSupportedException();
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		/// </summary>
		[CLSCompliant(false)]
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
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ORMNamedElement.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ORMNamedElementHasExtensionElement.ExtensionCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|Extensions||", match);
				ORMNamedElement.myChildElementMappings = childElementMappings;
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
	#endregion // ORMNamedElement serialization
	#region ORMModelElement serialization
	public partial class ORMModelElement : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
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
		private static ORMCustomSerializedChildElementInfo[] myCustomSerializedChildElementInfo;
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedChildElementInfo
		/// </summary>
		protected ORMCustomSerializedChildElementInfo[] GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedChildElementInfo[] ret = ORMModelElement.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new ORMCustomSerializedChildElementInfo[1];
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "Extensions", null, ORMCustomSerializedElementWriteStyle.Element, null, ORMModelElementHasExtensionElement.ExtensionCollectionMetaRoleGuid);
				ORMModelElement.myCustomSerializedChildElementInfo = ret;
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
		protected ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			throw new NotSupportedException();
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		/// </summary>
		[CLSCompliant(false)]
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
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ORMModelElement.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ORMModelElementHasExtensionElement.ExtensionCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|Extensions||", match);
				ORMModelElement.myChildElementMappings = childElementMappings;
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
	#endregion // ORMModelElement serialization
	#region ORMModel serialization
	public partial class ORMModel : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.ChildElementInfo | (ORMCustomSerializedElementSupportedOperations.ElementInfo | ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles));
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
		protected new ORMCustomSerializedChildElementInfo[] GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedChildElementInfo[] ret = ORMModel.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ORMCustomSerializedChildElementInfo[] baseInfo = null;
				int baseInfoCount = 0;
				if (0 != (ORMCustomSerializedElementSupportedOperations.ChildElementInfo & base.SupportedCustomSerializedOperations))
				{
					baseInfo = base.GetCustomSerializedChildElementInfo();
					if (baseInfo != null)
					{
						baseInfoCount = baseInfo.Length;
					}
				}
				ret = new ORMCustomSerializedChildElementInfo[baseInfoCount + 7];
				if (!(baseInfoCount == 0))
				{
					baseInfo.CopyTo(ret, 7);
				}
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "Objects", null, ORMCustomSerializedElementWriteStyle.Element, null, ModelHasObjectType.ObjectTypeCollectionMetaRoleGuid);
				ret[1] = new ORMCustomSerializedChildElementInfo(null, "Facts", null, ORMCustomSerializedElementWriteStyle.Element, null, ModelHasFactType.FactTypeCollectionMetaRoleGuid);
				ret[2] = new ORMCustomSerializedChildElementInfo(null, "ExternalConstraints", null, ORMCustomSerializedElementWriteStyle.Element, null, ModelHasMultiColumnExternalConstraint.MultiColumnExternalConstraintCollectionMetaRoleGuid, ModelHasSingleColumnExternalConstraint.SingleColumnExternalConstraintCollectionMetaRoleGuid);
				ret[3] = new ORMCustomSerializedChildElementInfo(null, "DataTypes", null, ORMCustomSerializedElementWriteStyle.Element, null, ModelHasDataType.DataTypeCollectionMetaRoleGuid);
				ret[4] = new ORMCustomSerializedChildElementInfo(null, "CustomReferenceModes", null, ORMCustomSerializedElementWriteStyle.Element, null, ModelHasReferenceMode.ReferenceModeCollectionMetaRoleGuid);
				ret[5] = new ORMCustomSerializedChildElementInfo(null, "ModelErrors", null, ORMCustomSerializedElementWriteStyle.Element, null, ModelHasError.ErrorCollectionMetaRoleGuid);
				ret[6] = new ORMCustomSerializedChildElementInfo(null, "ReferenceModeKinds", null, ORMCustomSerializedElementWriteStyle.Element, null, ModelHasReferenceModeKind.ReferenceModeKindCollectionMetaRoleGuid);
				ORMModel.myCustomSerializedChildElementInfo = ret;
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
		protected new ORMCustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new ORMCustomSerializedElementInfo(null, "ORMModel", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return this.CustomSerializedElementInfo;
			}
		}
		private static IComparer<MetaRoleInfo> myCustomSortChildComparer;
		private class CustomSortChildComparer : IComparer<MetaRoleInfo>
		{
			private Dictionary<string, int> myRoleOrderDictionary;
			private IComparer<MetaRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<MetaRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				MetaDataDirectory metaDataDir = store.MetaDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				MetaRoleInfo metaRole;
				metaRole = metaDataDir.FindMetaRole(ModelHasObjectType.ObjectTypeCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 0;
				metaRole = metaDataDir.FindMetaRole(ModelHasFactType.FactTypeCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 1;
				metaRole = metaDataDir.FindMetaRole(ModelHasMultiColumnExternalConstraint.MultiColumnExternalConstraintCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 2;
				metaRole = metaDataDir.FindMetaRole(ModelHasSingleColumnExternalConstraint.SingleColumnExternalConstraintCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 2;
				metaRole = metaDataDir.FindMetaRole(ModelHasDataType.DataTypeCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 3;
				metaRole = metaDataDir.FindMetaRole(ModelHasReferenceMode.ReferenceModeCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 4;
				metaRole = metaDataDir.FindMetaRole(ModelHasError.ErrorCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 5;
				metaRole = metaDataDir.FindMetaRole(ModelHasReferenceModeKind.ReferenceModeKindCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 6;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<MetaRoleInfo>.Compare(MetaRoleInfo x, MetaRoleInfo y)
			{
				if (this.myBaseComparer != null)
				{
					int baseOpinion = this.myBaseComparer.Compare(x, y);
					if (0 != baseOpinion)
					{
						return baseOpinion;
					}
				}
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
		protected new IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<MetaRoleInfo> retVal = ORMModel.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<MetaRoleInfo> baseComparer = null;
					if (0 != (ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					ORMModel.myCustomSortChildComparer = retVal;
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
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ORMModel.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ModelHasObjectType.ObjectTypeCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|Objects||", match);
				match.InitializeRoles(ModelHasFactType.FactTypeCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|Facts||", match);
				match.InitializeRoles(ModelHasMultiColumnExternalConstraint.MultiColumnExternalConstraintCollectionMetaRoleGuid, ModelHasSingleColumnExternalConstraint.SingleColumnExternalConstraintCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|ExternalConstraints||", match);
				match.InitializeRoles(ModelHasDataType.DataTypeCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|DataTypes||", match);
				match.InitializeRoles(ModelHasReferenceMode.ReferenceModeCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|CustomReferenceModes||", match);
				match.InitializeRoles(ModelHasError.ErrorCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|ModelErrors||", match);
				match.InitializeRoles(ModelHasReferenceModeKind.ReferenceModeKindCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|ReferenceModeKinds||", match);
				ORMModel.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // ORMModel serialization
	#region ObjectType serialization
	public partial class ObjectType : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.ChildElementInfo | (ORMCustomSerializedElementSupportedOperations.ElementInfo | (ORMCustomSerializedElementSupportedOperations.AttributeInfo | (ORMCustomSerializedElementSupportedOperations.LinkInfo | ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles))));
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
		protected new ORMCustomSerializedChildElementInfo[] GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedChildElementInfo[] ret = ObjectType.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ORMCustomSerializedChildElementInfo[] baseInfo = null;
				int baseInfoCount = 0;
				if (0 != (ORMCustomSerializedElementSupportedOperations.ChildElementInfo & base.SupportedCustomSerializedOperations))
				{
					baseInfo = base.GetCustomSerializedChildElementInfo();
					if (baseInfo != null)
					{
						baseInfoCount = baseInfo.Length;
					}
				}
				ret = new ORMCustomSerializedChildElementInfo[baseInfoCount + 2];
				if (!(baseInfoCount == 0))
				{
					baseInfo.CopyTo(ret, 2);
				}
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "PlayedRoles", null, ORMCustomSerializedElementWriteStyle.Element, null, ObjectTypePlaysRole.PlayedRoleCollectionMetaRoleGuid);
				ret[1] = new ORMCustomSerializedChildElementInfo(null, "ValueConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null, ValueTypeHasValueConstraint.ValueConstraintMetaRoleGuid);
				ObjectType.myCustomSerializedChildElementInfo = ret;
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
		protected new ORMCustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				string name = "EntityType";
				if (this.IsValueType)
				{
					name = "ValueType";
				}
				else if (this.NestedFactType != null)
				{
					name = "ObjectifiedType";
				}
				return new ORMCustomSerializedElementInfo(null, name, null, ORMCustomSerializedElementWriteStyle.Element, null);
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
		protected new ORMCustomSerializedAttributeInfo GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			if (attributeInfo.Id == ObjectType.ReferenceModeStringMetaAttributeGuid)
			{
				if (this.IsValueType)
				{
					return new ORMCustomSerializedAttributeInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				if (this.NestedFactType != null)
				{
					return new ORMCustomSerializedAttributeInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new ORMCustomSerializedAttributeInfo(null, "_ReferenceMode", null, true, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (attributeInfo.Id == ObjectType.IsPersonalMetaAttributeGuid)
			{
				if (!(this.IsPersonal))
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
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == ValueTypeHasDataType.DataTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ConceptualDataType", null, ORMCustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (rolePlayedInfo.Id == ObjectTypePlaysRole.PlayedRoleCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Role", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (rolePlayedInfo.Id == Objectification.NestedFactTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "NestedPredicate", null, ORMCustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (rolePlayedInfo.Id == EntityTypeHasPreferredIdentifier.PreferredIdentifierMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "PreferredIdentifier", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (rolePlayedInfo.Id == ObjectTypeHasEntityTypeRequiresReferenceSchemeError.ReferenceSchemeErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (rolePlayedInfo.Id == ObjectTypeHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static IComparer<MetaRoleInfo> myCustomSortChildComparer;
		private class CustomSortChildComparer : IComparer<MetaRoleInfo>
		{
			private Dictionary<string, int> myRoleOrderDictionary;
			private IComparer<MetaRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<MetaRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				MetaDataDirectory metaDataDir = store.MetaDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				MetaRoleInfo metaRole;
				metaRole = metaDataDir.FindMetaRole(ObjectTypePlaysRole.PlayedRoleCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 0;
				metaRole = metaDataDir.FindMetaRole(ValueTypeHasDataType.DataTypeMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 1;
				metaRole = metaDataDir.FindMetaRole(ValueTypeHasValueConstraint.ValueConstraintMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 2;
				metaRole = metaDataDir.FindMetaRole(Objectification.NestedFactTypeMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 3;
				metaRole = metaDataDir.FindMetaRole(EntityTypeHasPreferredIdentifier.PreferredIdentifierMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 4;
				metaRole = metaDataDir.FindMetaRole(ObjectTypeHasEntityTypeRequiresReferenceSchemeError.ReferenceSchemeErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 5;
				metaRole = metaDataDir.FindMetaRole(ObjectTypeHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 6;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<MetaRoleInfo>.Compare(MetaRoleInfo x, MetaRoleInfo y)
			{
				if (this.myBaseComparer != null)
				{
					int baseOpinion = this.myBaseComparer.Compare(x, y);
					if (0 != baseOpinion)
					{
						return baseOpinion;
					}
				}
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
		protected new IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<MetaRoleInfo> retVal = ObjectType.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<MetaRoleInfo> baseComparer = null;
					if (0 != (ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					ObjectType.myCustomSortChildComparer = retVal;
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
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ObjectType.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ValueTypeHasDataType.DataTypeMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|ConceptualDataType", match);
				match.InitializeRoles(Objectification.NestedFactTypeMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|NestedPredicate", match);
				match.InitializeRoles(EntityTypeHasPreferredIdentifier.PreferredIdentifierMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|PreferredIdentifier", match);
				match.InitializeRoles(ObjectTypePlaysRole.PlayedRoleCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|PlayedRoles|http://schemas.neumont.edu/ORM/ORMCore|Role", match);
				match.InitializeRoles(ValueTypeHasValueConstraint.ValueConstraintMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|ValueConstraint||", match);
				ObjectType.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
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
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = ObjectType.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("_ReferenceMode", ObjectType.ReferenceModeStringMetaAttributeGuid);
				customSerializedAttributes.Add("IsPersonal", ObjectType.IsPersonalMetaAttributeGuid);
				ObjectType.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // ObjectType serialization
	#region CustomReferenceMode serialization
	public partial class CustomReferenceMode : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.AttributeInfo | (ORMCustomSerializedElementSupportedOperations.LinkInfo | (ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles | ORMCustomSerializedElementSupportedOperations.MixedTypedAttributes)));
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
			if (attributeInfo.Id == CustomReferenceMode.CustomFormatStringMetaAttributeGuid)
			{
				return new ORMCustomSerializedAttributeInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.Element, null);
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
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == ReferenceModeHasReferenceModeKind.KindMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Kind", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static IComparer<MetaRoleInfo> myCustomSortChildComparer;
		private class CustomSortChildComparer : IComparer<MetaRoleInfo>
		{
			private Dictionary<string, int> myRoleOrderDictionary;
			private IComparer<MetaRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<MetaRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				MetaDataDirectory metaDataDir = store.MetaDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				MetaRoleInfo metaRole;
				metaRole = metaDataDir.FindMetaRole(ReferenceModeHasReferenceModeKind.KindMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 0;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<MetaRoleInfo>.Compare(MetaRoleInfo x, MetaRoleInfo y)
			{
				if (this.myBaseComparer != null)
				{
					int baseOpinion = this.myBaseComparer.Compare(x, y);
					if (0 != baseOpinion)
					{
						return baseOpinion;
					}
				}
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
		protected new IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<MetaRoleInfo> retVal = CustomReferenceMode.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<MetaRoleInfo> baseComparer = null;
					if (0 != (ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					CustomReferenceMode.myCustomSortChildComparer = retVal;
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
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = CustomReferenceMode.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ReferenceModeHasReferenceModeKind.KindMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|Kind", match);
				match.InitializeAttribute(CustomReferenceMode.CustomFormatStringMetaAttributeGuid, null);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|CustomFormatString", match);
				CustomReferenceMode.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // CustomReferenceMode serialization
	#region ValueTypeHasDataType serialization
	public partial class ValueTypeHasDataType : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return ORMCustomSerializedElementSupportedOperations.AttributeInfo | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
			if (attributeInfo.Id == ValueTypeHasDataType.ScaleMetaAttributeGuid)
			{
				if (rolePlayedInfo.Id == ValueTypeHasDataType.ValueTypeCollectionMetaRoleGuid)
				{
					return new ORMCustomSerializedAttributeInfo(null, "Scale", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
				}
				return new ORMCustomSerializedAttributeInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
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
		protected ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == ValueTypeHasUnspecifiedDataTypeError.DataTypeNotSpecifiedErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		/// </summary>
		[CLSCompliant(false)]
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
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return default(ORMCustomSerializedElementMatch);
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
			Dictionary<string, Guid> customSerializedAttributes = ValueTypeHasDataType.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("Scale", ValueTypeHasDataType.ScaleMetaAttributeGuid);
				ValueTypeHasDataType.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // ValueTypeHasDataType serialization
	#region DataType serialization
	public partial class DataType : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == ValueTypeHasDataType.ValueTypeCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.ShouldSerialize
		/// </summary>
		protected new bool ShouldSerialize()
		{
			return this.GetElementLinks().Count > 1;
		}
		bool IORMCustomSerializedElement.ShouldSerialize()
		{
			return this.ShouldSerialize();
		}
	}
	#endregion // DataType serialization
	#region ReferenceModeKind serialization
	public partial class ReferenceModeKind : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.ElementInfo | (ORMCustomSerializedElementSupportedOperations.AttributeInfo | ORMCustomSerializedElementSupportedOperations.LinkInfo));
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
		/// Implements IORMCustomSerializedElement.CustomSerializedElementInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new ORMCustomSerializedElementInfo(null, "ReferenceModeKind", null, ORMCustomSerializedElementWriteStyle.Element, null);
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
		protected new ORMCustomSerializedAttributeInfo GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			if (attributeInfo.Id == ReferenceModeKind.NameMetaAttributeGuid)
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
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == ReferenceModeHasReferenceModeKind.ReferenceModeCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
	}
	#endregion // ReferenceModeKind serialization
	#region ValueConstraint serialization
	public partial class ValueConstraint : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.ChildElementInfo | (ORMCustomSerializedElementSupportedOperations.ElementInfo | ORMCustomSerializedElementSupportedOperations.LinkInfo));
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
		protected new ORMCustomSerializedChildElementInfo[] GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedChildElementInfo[] ret = ValueConstraint.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ORMCustomSerializedChildElementInfo[] baseInfo = null;
				int baseInfoCount = 0;
				if (0 != (ORMCustomSerializedElementSupportedOperations.ChildElementInfo & base.SupportedCustomSerializedOperations))
				{
					baseInfo = base.GetCustomSerializedChildElementInfo();
					if (baseInfo != null)
					{
						baseInfoCount = baseInfo.Length;
					}
				}
				ret = new ORMCustomSerializedChildElementInfo[baseInfoCount + 1];
				if (!(baseInfoCount == 0))
				{
					baseInfo.CopyTo(ret, 1);
				}
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "ValueRanges", null, ORMCustomSerializedElementWriteStyle.Element, null, ValueConstraintHasValueRange.ValueRangeCollectionMetaRoleGuid);
				ValueConstraint.myCustomSerializedChildElementInfo = ret;
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
		protected new ORMCustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new ORMCustomSerializedElementInfo(null, "BaseValueRangeDefinition", null, ORMCustomSerializedElementWriteStyle.Element, null);
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == ValueConstraintHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ValueConstraint.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ValueConstraintHasValueRange.ValueRangeCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|ValueRanges||", match);
				ValueConstraint.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // ValueConstraint serialization
	#region ValueTypeValueConstraint serialization
	public partial class ValueTypeValueConstraint : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.ElementInfo;
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
		/// Implements IORMCustomSerializedElement.CustomSerializedElementInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new ORMCustomSerializedElementInfo(null, "ValueRangeDefinition", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return this.CustomSerializedElementInfo;
			}
		}
	}
	#endregion // ValueTypeValueConstraint serialization
	#region RoleValueConstraint serialization
	public partial class RoleValueConstraint : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.ElementInfo;
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
		/// Implements IORMCustomSerializedElement.CustomSerializedElementInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new ORMCustomSerializedElementInfo(null, "RoleValueRangeDefinition", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return this.CustomSerializedElementInfo;
			}
		}
	}
	#endregion // RoleValueConstraint serialization
	#region ValueRange serialization
	public partial class ValueRange : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == ValueRangeHasMinValueMismatchError.MinValueMismatchErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (rolePlayedInfo.Id == ValueRangeHasMaxValueMismatchError.MaxValueMismatchErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
	}
	#endregion // ValueRange serialization
	#region FactType serialization
	public partial class FactType : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.ChildElementInfo | (ORMCustomSerializedElementSupportedOperations.ElementInfo | (ORMCustomSerializedElementSupportedOperations.LinkInfo | ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles)));
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
		protected new ORMCustomSerializedChildElementInfo[] GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedChildElementInfo[] ret = FactType.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ORMCustomSerializedChildElementInfo[] baseInfo = null;
				int baseInfoCount = 0;
				if (0 != (ORMCustomSerializedElementSupportedOperations.ChildElementInfo & base.SupportedCustomSerializedOperations))
				{
					baseInfo = base.GetCustomSerializedChildElementInfo();
					if (baseInfo != null)
					{
						baseInfoCount = baseInfo.Length;
					}
				}
				ret = new ORMCustomSerializedChildElementInfo[baseInfoCount + 3];
				if (!(baseInfoCount == 0))
				{
					baseInfo.CopyTo(ret, 3);
				}
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "FactRoles", null, ORMCustomSerializedElementWriteStyle.Element, null, FactTypeHasRole.RoleCollectionMetaRoleGuid);
				ret[1] = new ORMCustomSerializedChildElementInfo(null, "ReadingOrders", null, ORMCustomSerializedElementWriteStyle.Element, null, FactTypeHasReadingOrder.ReadingOrderCollectionMetaRoleGuid);
				ret[2] = new ORMCustomSerializedChildElementInfo(null, "InternalConstraints", null, ORMCustomSerializedElementWriteStyle.Element, null, FactTypeHasInternalConstraint.InternalConstraintCollectionMetaRoleGuid);
				FactType.myCustomSerializedChildElementInfo = ret;
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
		protected new ORMCustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				string name = "Fact";
				if (this.ImpliedByObjectification != null)
				{
					name = "ImpliedFact";
				}
				return new ORMCustomSerializedElementInfo(null, name, null, ORMCustomSerializedElementWriteStyle.Element, null);
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == ObjectificationImpliesFactType.ImpliedByObjectificationMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ImpliedByObjectification", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (rolePlayedInfo.Id == Objectification.NestingTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (rolePlayedInfo.Id == FactTypeHasFactTypeRequiresInternalUniquenessConstraintError.InternalUniquenessConstraintRequiredErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (rolePlayedInfo.Id == FactTypeHasFactTypeRequiresReadingError.ReadingRequiredErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (rolePlayedInfo.Id == FactTypeHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (rolePlayedInfo.Id == FactTypeHasImpliedInternalUniquenessConstraintError.ImpliedInternalUniquenessConstraintErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (rolePlayedInfo.Id == FactTypeHasFrequencyConstraintContradictsInternalUniquenessConstraintError.FrequencyConstraintContradictsInternalUniquenessConstraintErrorCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static IComparer<MetaRoleInfo> myCustomSortChildComparer;
		private class CustomSortChildComparer : IComparer<MetaRoleInfo>
		{
			private Dictionary<string, int> myRoleOrderDictionary;
			private IComparer<MetaRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<MetaRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				MetaDataDirectory metaDataDir = store.MetaDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				MetaRoleInfo metaRole;
				metaRole = metaDataDir.FindMetaRole(FactTypeHasRole.RoleCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 0;
				metaRole = metaDataDir.FindMetaRole(FactTypeHasReadingOrder.ReadingOrderCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 1;
				metaRole = metaDataDir.FindMetaRole(FactTypeHasInternalConstraint.InternalConstraintCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 2;
				metaRole = metaDataDir.FindMetaRole(ObjectificationImpliesFactType.ImpliedByObjectificationMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 3;
				metaRole = metaDataDir.FindMetaRole(Objectification.NestingTypeMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 4;
				metaRole = metaDataDir.FindMetaRole(FactTypeHasFactTypeRequiresInternalUniquenessConstraintError.InternalUniquenessConstraintRequiredErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 5;
				metaRole = metaDataDir.FindMetaRole(FactTypeHasFactTypeRequiresReadingError.ReadingRequiredErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 6;
				metaRole = metaDataDir.FindMetaRole(FactTypeHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 7;
				metaRole = metaDataDir.FindMetaRole(FactTypeHasImpliedInternalUniquenessConstraintError.ImpliedInternalUniquenessConstraintErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 8;
				metaRole = metaDataDir.FindMetaRole(FactTypeHasFrequencyConstraintContradictsInternalUniquenessConstraintError.FrequencyConstraintContradictsInternalUniquenessConstraintErrorCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 9;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<MetaRoleInfo>.Compare(MetaRoleInfo x, MetaRoleInfo y)
			{
				if (this.myBaseComparer != null)
				{
					int baseOpinion = this.myBaseComparer.Compare(x, y);
					if (0 != baseOpinion)
					{
						return baseOpinion;
					}
				}
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
		protected new IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<MetaRoleInfo> retVal = FactType.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<MetaRoleInfo> baseComparer = null;
					if (0 != (ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					FactType.myCustomSortChildComparer = retVal;
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
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = FactType.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ObjectificationImpliesFactType.ImpliedByObjectificationMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|ImpliedByObjectification", match);
				match.InitializeRoles(FactTypeHasRole.RoleCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|FactRoles||", match);
				match.InitializeRoles(FactTypeHasReadingOrder.ReadingOrderCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|ReadingOrders||", match);
				match.InitializeRoles(FactTypeHasInternalConstraint.InternalConstraintCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|InternalConstraints||", match);
				FactType.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // FactType serialization
	#region SubtypeFact serialization
	public partial class SubtypeFact : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.ElementInfo;
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
		/// Implements IORMCustomSerializedElement.CustomSerializedElementInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new ORMCustomSerializedElementInfo(null, "SubtypeFact", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return this.CustomSerializedElementInfo;
			}
		}
	}
	#endregion // SubtypeFact serialization
	#region Objectification serialization
	public partial class Objectification : IORMCustomSerializedElement
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
		protected ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == ObjectificationImpliesFactType.ImpliedFactTypeCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ImpliedByObjectification", null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (rolePlayedInfo.Id == ObjectificationImpliesEqualityConstraint.ImpliedEqualityConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ImpliedByObjectification", null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (rolePlayedInfo.Id == ObjectificationImpliesExternalUniquenessConstraint.ImpliedExternalUniquenessConstraintCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ImpliedByObjectification", null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.CustomSerializedChildRoleComparer
		/// </summary>
		[CLSCompliant(false)]
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
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return default(ORMCustomSerializedElementMatch);
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
	#endregion // Objectification serialization
	#region ReadingOrder serialization
	public partial class ReadingOrder : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.ChildElementInfo | (ORMCustomSerializedElementSupportedOperations.LinkInfo | ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles));
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
		protected new ORMCustomSerializedChildElementInfo[] GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedChildElementInfo[] ret = ReadingOrder.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ORMCustomSerializedChildElementInfo[] baseInfo = null;
				int baseInfoCount = 0;
				if (0 != (ORMCustomSerializedElementSupportedOperations.ChildElementInfo & base.SupportedCustomSerializedOperations))
				{
					baseInfo = base.GetCustomSerializedChildElementInfo();
					if (baseInfo != null)
					{
						baseInfoCount = baseInfo.Length;
					}
				}
				ret = new ORMCustomSerializedChildElementInfo[baseInfoCount + 2];
				if (!(baseInfoCount == 0))
				{
					baseInfo.CopyTo(ret, 2);
				}
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "Readings", null, ORMCustomSerializedElementWriteStyle.Element, null, ReadingOrderHasReading.ReadingCollectionMetaRoleGuid);
				ret[1] = new ORMCustomSerializedChildElementInfo(null, "RoleSequence", null, ORMCustomSerializedElementWriteStyle.Element, null, ReadingOrderHasRole.RoleCollectionMetaRoleGuid);
				ReadingOrder.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		ORMCustomSerializedChildElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == ReadingOrderHasRole.RoleCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Role", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static IComparer<MetaRoleInfo> myCustomSortChildComparer;
		private class CustomSortChildComparer : IComparer<MetaRoleInfo>
		{
			private Dictionary<string, int> myRoleOrderDictionary;
			private IComparer<MetaRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<MetaRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				MetaDataDirectory metaDataDir = store.MetaDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				MetaRoleInfo metaRole;
				metaRole = metaDataDir.FindMetaRole(ReadingOrderHasReading.ReadingCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 0;
				metaRole = metaDataDir.FindMetaRole(ReadingOrderHasRole.RoleCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 1;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<MetaRoleInfo>.Compare(MetaRoleInfo x, MetaRoleInfo y)
			{
				if (this.myBaseComparer != null)
				{
					int baseOpinion = this.myBaseComparer.Compare(x, y);
					if (0 != baseOpinion)
					{
						return baseOpinion;
					}
				}
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
		protected new IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<MetaRoleInfo> retVal = ReadingOrder.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<MetaRoleInfo> baseComparer = null;
					if (0 != (ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					ReadingOrder.myCustomSortChildComparer = retVal;
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
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ReadingOrder.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ReadingOrderHasRole.RoleCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|RoleSequence|http://schemas.neumont.edu/ORM/ORMCore|Role", match);
				match.InitializeRoles(ReadingOrderHasReading.ReadingCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|Readings||", match);
				ReadingOrder.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // ReadingOrder serialization
	#region Reading serialization
	public partial class Reading : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.AttributeInfo | (ORMCustomSerializedElementSupportedOperations.LinkInfo | ORMCustomSerializedElementSupportedOperations.MixedTypedAttributes));
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
			if (attributeInfo.Id == Reading.TextMetaAttributeGuid)
			{
				return new ORMCustomSerializedAttributeInfo(null, "Data", null, false, ORMCustomSerializedAttributeWriteStyle.Element, null);
			}
			if (attributeInfo.Id == Reading.LanguageMetaAttributeGuid)
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
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == ReadingHasTooManyRolesError.TooManyRolesErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (rolePlayedInfo.Id == ReadingHasTooFewRolesError.TooFewRolesErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = Reading.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeAttribute(Reading.TextMetaAttributeGuid, null);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|Data", match);
				Reading.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // Reading serialization
	#region Role serialization
	public partial class Role : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.ChildElementInfo | (ORMCustomSerializedElementSupportedOperations.AttributeInfo | (ORMCustomSerializedElementSupportedOperations.LinkInfo | ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles)));
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
		protected new ORMCustomSerializedChildElementInfo[] GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedChildElementInfo[] ret = Role.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ORMCustomSerializedChildElementInfo[] baseInfo = null;
				int baseInfoCount = 0;
				if (0 != (ORMCustomSerializedElementSupportedOperations.ChildElementInfo & base.SupportedCustomSerializedOperations))
				{
					baseInfo = base.GetCustomSerializedChildElementInfo();
					if (baseInfo != null)
					{
						baseInfoCount = baseInfo.Length;
					}
				}
				ret = new ORMCustomSerializedChildElementInfo[baseInfoCount + 1];
				if (!(baseInfoCount == 0))
				{
					baseInfo.CopyTo(ret, 1);
				}
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "ValueConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null, RoleHasValueConstraint.ValueConstraintMetaRoleGuid);
				Role.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		ORMCustomSerializedChildElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedAttributeInfo
		/// </summary>
		protected new ORMCustomSerializedAttributeInfo GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			if (attributeInfo.Id == Role.IsMandatoryMetaAttributeGuid)
			{
				return new ORMCustomSerializedAttributeInfo(null, "_IsMandatory", null, true, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (attributeInfo.Id == Role.MultiplicityMetaAttributeGuid)
			{
				return new ORMCustomSerializedAttributeInfo(null, "_Multiplicity", null, true, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
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
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == ObjectTypePlaysRole.RolePlayerMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "RolePlayer", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (rolePlayedInfo.Id == ConstraintRoleSequenceHasRole.ConstraintRoleSequenceCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (rolePlayedInfo.Id == ReadingOrderHasRole.ReadingOrderMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (rolePlayedInfo.Id == RoleHasRolePlayerRequiredError.RolePlayerRequiredErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static IComparer<MetaRoleInfo> myCustomSortChildComparer;
		private class CustomSortChildComparer : IComparer<MetaRoleInfo>
		{
			private Dictionary<string, int> myRoleOrderDictionary;
			private IComparer<MetaRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<MetaRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				MetaDataDirectory metaDataDir = store.MetaDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				MetaRoleInfo metaRole;
				metaRole = metaDataDir.FindMetaRole(ObjectTypePlaysRole.RolePlayerMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 0;
				metaRole = metaDataDir.FindMetaRole(RoleHasValueConstraint.ValueConstraintMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 1;
				metaRole = metaDataDir.FindMetaRole(ConstraintRoleSequenceHasRole.ConstraintRoleSequenceCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 2;
				metaRole = metaDataDir.FindMetaRole(ReadingOrderHasRole.ReadingOrderMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 3;
				metaRole = metaDataDir.FindMetaRole(RoleHasRolePlayerRequiredError.RolePlayerRequiredErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 4;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<MetaRoleInfo>.Compare(MetaRoleInfo x, MetaRoleInfo y)
			{
				if (this.myBaseComparer != null)
				{
					int baseOpinion = this.myBaseComparer.Compare(x, y);
					if (0 != baseOpinion)
					{
						return baseOpinion;
					}
				}
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
		protected new IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<MetaRoleInfo> retVal = Role.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<MetaRoleInfo> baseComparer = null;
					if (0 != (ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					Role.myCustomSortChildComparer = retVal;
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
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = Role.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ObjectTypePlaysRole.RolePlayerMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|RolePlayer", match);
				match.InitializeRoles(RoleHasValueConstraint.ValueConstraintMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|ValueConstraint||", match);
				Role.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
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
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = Role.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("_IsMandatory", Role.IsMandatoryMetaAttributeGuid);
				customSerializedAttributes.Add("_Multiplicity", Role.MultiplicityMetaAttributeGuid);
				Role.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // Role serialization
	#region MultiColumnExternalConstraint serialization
	public partial class MultiColumnExternalConstraint : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.ChildElementInfo | (ORMCustomSerializedElementSupportedOperations.AttributeInfo | (ORMCustomSerializedElementSupportedOperations.LinkInfo | ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles)));
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
		protected new ORMCustomSerializedChildElementInfo[] GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedChildElementInfo[] ret = MultiColumnExternalConstraint.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ORMCustomSerializedChildElementInfo[] baseInfo = null;
				int baseInfoCount = 0;
				if (0 != (ORMCustomSerializedElementSupportedOperations.ChildElementInfo & base.SupportedCustomSerializedOperations))
				{
					baseInfo = base.GetCustomSerializedChildElementInfo();
					if (baseInfo != null)
					{
						baseInfoCount = baseInfo.Length;
					}
				}
				ret = new ORMCustomSerializedChildElementInfo[baseInfoCount + 1];
				if (!(baseInfoCount == 0))
				{
					baseInfo.CopyTo(ret, 1);
				}
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "RoleSequences", null, ORMCustomSerializedElementWriteStyle.Element, null, MultiColumnExternalConstraintHasRoleSequence.RoleSequenceCollectionMetaRoleGuid);
				MultiColumnExternalConstraint.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		ORMCustomSerializedChildElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedAttributeInfo
		/// </summary>
		protected new ORMCustomSerializedAttributeInfo GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			if (attributeInfo.Id == MultiColumnExternalConstraint.ModalityMetaAttributeGuid)
			{
				if (ConstraintModality.Alethic == this.Modality)
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
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == MultiColumnExternalConstraintHasCompatibleRolePlayerTypeError.CompatibleRolePlayerTypeErrorCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (rolePlayedInfo.Id == MultiColumnExternalConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (rolePlayedInfo.Id == MultiColumnExternalConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (rolePlayedInfo.Id == MultiColumnExternalConstraintHasExternalConstraintRoleSequenceArityMismatchError.ArityMismatchErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static IComparer<MetaRoleInfo> myCustomSortChildComparer;
		private class CustomSortChildComparer : IComparer<MetaRoleInfo>
		{
			private Dictionary<string, int> myRoleOrderDictionary;
			private IComparer<MetaRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<MetaRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				MetaDataDirectory metaDataDir = store.MetaDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				MetaRoleInfo metaRole;
				metaRole = metaDataDir.FindMetaRole(MultiColumnExternalConstraintHasRoleSequence.RoleSequenceCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 0;
				metaRole = metaDataDir.FindMetaRole(MultiColumnExternalConstraintHasCompatibleRolePlayerTypeError.CompatibleRolePlayerTypeErrorCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 1;
				metaRole = metaDataDir.FindMetaRole(MultiColumnExternalConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 2;
				metaRole = metaDataDir.FindMetaRole(MultiColumnExternalConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 3;
				metaRole = metaDataDir.FindMetaRole(MultiColumnExternalConstraintHasExternalConstraintRoleSequenceArityMismatchError.ArityMismatchErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 4;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<MetaRoleInfo>.Compare(MetaRoleInfo x, MetaRoleInfo y)
			{
				if (this.myBaseComparer != null)
				{
					int baseOpinion = this.myBaseComparer.Compare(x, y);
					if (0 != baseOpinion)
					{
						return baseOpinion;
					}
				}
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
		protected new IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<MetaRoleInfo> retVal = MultiColumnExternalConstraint.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<MetaRoleInfo> baseComparer = null;
					if (0 != (ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					MultiColumnExternalConstraint.myCustomSortChildComparer = retVal;
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
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = MultiColumnExternalConstraint.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(MultiColumnExternalConstraintHasRoleSequence.RoleSequenceCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|RoleSequences||", match);
				MultiColumnExternalConstraint.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
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
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = MultiColumnExternalConstraint.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("Modality", MultiColumnExternalConstraint.ModalityMetaAttributeGuid);
				MultiColumnExternalConstraint.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // MultiColumnExternalConstraint serialization
	#region MultiColumnExternalConstraintRoleSequence serialization
	public partial class MultiColumnExternalConstraintRoleSequence : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.ElementInfo | (ORMCustomSerializedElementSupportedOperations.LinkInfo | ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles));
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
		/// Implements IORMCustomSerializedElement.CustomSerializedElementInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new ORMCustomSerializedElementInfo(null, "RoleSequence", null, ORMCustomSerializedElementWriteStyle.Element, null);
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == ConstraintRoleSequenceHasRole.RoleCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Role", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static IComparer<MetaRoleInfo> myCustomSortChildComparer;
		private class CustomSortChildComparer : IComparer<MetaRoleInfo>
		{
			private Dictionary<string, int> myRoleOrderDictionary;
			private IComparer<MetaRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<MetaRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				MetaDataDirectory metaDataDir = store.MetaDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				MetaRoleInfo metaRole;
				metaRole = metaDataDir.FindMetaRole(ConstraintRoleSequenceHasRole.RoleCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 0;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<MetaRoleInfo>.Compare(MetaRoleInfo x, MetaRoleInfo y)
			{
				if (this.myBaseComparer != null)
				{
					int baseOpinion = this.myBaseComparer.Compare(x, y);
					if (0 != baseOpinion)
					{
						return baseOpinion;
					}
				}
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
		protected new IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<MetaRoleInfo> retVal = MultiColumnExternalConstraintRoleSequence.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<MetaRoleInfo> baseComparer = null;
					if (0 != (ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					MultiColumnExternalConstraintRoleSequence.myCustomSortChildComparer = retVal;
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
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = MultiColumnExternalConstraintRoleSequence.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ConstraintRoleSequenceHasRole.RoleCollectionMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|Role", match);
				MultiColumnExternalConstraintRoleSequence.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // MultiColumnExternalConstraintRoleSequence serialization
	#region SingleColumnExternalConstraint serialization
	public partial class SingleColumnExternalConstraint : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.ChildElementInfo | (ORMCustomSerializedElementSupportedOperations.AttributeInfo | (ORMCustomSerializedElementSupportedOperations.LinkInfo | ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles)));
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
		protected new ORMCustomSerializedChildElementInfo[] GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedChildElementInfo[] ret = SingleColumnExternalConstraint.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ORMCustomSerializedChildElementInfo[] baseInfo = null;
				int baseInfoCount = 0;
				if (0 != (ORMCustomSerializedElementSupportedOperations.ChildElementInfo & base.SupportedCustomSerializedOperations))
				{
					baseInfo = base.GetCustomSerializedChildElementInfo();
					if (baseInfo != null)
					{
						baseInfoCount = baseInfo.Length;
					}
				}
				ret = new ORMCustomSerializedChildElementInfo[baseInfoCount + 1];
				if (!(baseInfoCount == 0))
				{
					baseInfo.CopyTo(ret, 1);
				}
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "RoleSequence", null, ORMCustomSerializedElementWriteStyle.Element, null, ConstraintRoleSequenceHasRole.RoleCollectionMetaRoleGuid);
				SingleColumnExternalConstraint.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		ORMCustomSerializedChildElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedAttributeInfo
		/// </summary>
		protected new ORMCustomSerializedAttributeInfo GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			if (attributeInfo.Id == SingleColumnExternalConstraint.ModalityMetaAttributeGuid)
			{
				if (ConstraintModality.Alethic == this.Modality)
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
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == ConstraintRoleSequenceHasRole.RoleCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Role", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (rolePlayedInfo.Id == SingleColumnExternalConstraintHasCompatibleRolePlayerTypeError.CompatibleRolePlayerTypeErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (rolePlayedInfo.Id == SingleColumnExternalConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (rolePlayedInfo.Id == SingleColumnExternalConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static IComparer<MetaRoleInfo> myCustomSortChildComparer;
		private class CustomSortChildComparer : IComparer<MetaRoleInfo>
		{
			private Dictionary<string, int> myRoleOrderDictionary;
			private IComparer<MetaRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<MetaRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				MetaDataDirectory metaDataDir = store.MetaDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				MetaRoleInfo metaRole;
				metaRole = metaDataDir.FindMetaRole(ConstraintRoleSequenceHasRole.RoleCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 0;
				metaRole = metaDataDir.FindMetaRole(SingleColumnExternalConstraintHasCompatibleRolePlayerTypeError.CompatibleRolePlayerTypeErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 1;
				metaRole = metaDataDir.FindMetaRole(SingleColumnExternalConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 2;
				metaRole = metaDataDir.FindMetaRole(SingleColumnExternalConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 3;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<MetaRoleInfo>.Compare(MetaRoleInfo x, MetaRoleInfo y)
			{
				if (this.myBaseComparer != null)
				{
					int baseOpinion = this.myBaseComparer.Compare(x, y);
					if (0 != baseOpinion)
					{
						return baseOpinion;
					}
				}
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
		protected new IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<MetaRoleInfo> retVal = SingleColumnExternalConstraint.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<MetaRoleInfo> baseComparer = null;
					if (0 != (ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					SingleColumnExternalConstraint.myCustomSortChildComparer = retVal;
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
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = SingleColumnExternalConstraint.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ConstraintRoleSequenceHasRole.RoleCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|RoleSequence|http://schemas.neumont.edu/ORM/ORMCore|Role", match);
				SingleColumnExternalConstraint.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
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
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = SingleColumnExternalConstraint.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("Modality", SingleColumnExternalConstraint.ModalityMetaAttributeGuid);
				SingleColumnExternalConstraint.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // SingleColumnExternalConstraint serialization
	#region DisjunctiveMandatoryConstraint serialization
	public partial class DisjunctiveMandatoryConstraint : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == DisjunctiveMandatoryConstraintHasSimpleMandatoryImpliesDisjunctiveMandatoryError.ImpliedBySimpleMandatoryErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
	}
	#endregion // DisjunctiveMandatoryConstraint serialization
	#region FrequencyConstraint serialization
	public partial class FrequencyConstraint : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == FrequencyConstraintHasFrequencyConstraintMinMaxError.FrequencyConstraintMinMaxErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (rolePlayedInfo.Id == FrequencyConstraintHasFrequencyConstraintInvalidatedByInternalUniquenessConstraintError.FrequencyConstraintContradictsInternalUniquenessConstraintErrorCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
	}
	#endregion // FrequencyConstraint serialization
	#region ExternalUniquenessConstraint serialization
	public partial class ExternalUniquenessConstraint : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.ElementInfo | (ORMCustomSerializedElementSupportedOperations.LinkInfo | ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles));
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
		/// Implements IORMCustomSerializedElement.CustomSerializedElementInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				string name = null;
				if (this.ImpliedByObjectification != null)
				{
					name = "ImpliedExternalUniquenessConstraint";
				}
				return new ORMCustomSerializedElementInfo(null, name, null, ORMCustomSerializedElementWriteStyle.Element, null);
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == EntityTypeHasPreferredIdentifier.PreferredIdentifierForMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "PreferredIdentifierFor", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (rolePlayedInfo.Id == ObjectificationImpliesExternalUniquenessConstraint.ImpliedByObjectificationMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ImpliedByObjectification", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static IComparer<MetaRoleInfo> myCustomSortChildComparer;
		private class CustomSortChildComparer : IComparer<MetaRoleInfo>
		{
			private Dictionary<string, int> myRoleOrderDictionary;
			private IComparer<MetaRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<MetaRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				MetaDataDirectory metaDataDir = store.MetaDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				MetaRoleInfo metaRole;
				metaRole = metaDataDir.FindMetaRole(EntityTypeHasPreferredIdentifier.PreferredIdentifierForMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 0;
				metaRole = metaDataDir.FindMetaRole(ObjectificationImpliesExternalUniquenessConstraint.ImpliedByObjectificationMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 1;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<MetaRoleInfo>.Compare(MetaRoleInfo x, MetaRoleInfo y)
			{
				if (this.myBaseComparer != null)
				{
					int baseOpinion = this.myBaseComparer.Compare(x, y);
					if (0 != baseOpinion)
					{
						return baseOpinion;
					}
				}
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
		protected new IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<MetaRoleInfo> retVal = ExternalUniquenessConstraint.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<MetaRoleInfo> baseComparer = null;
					if (0 != (ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					ExternalUniquenessConstraint.myCustomSortChildComparer = retVal;
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
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ExternalUniquenessConstraint.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(EntityTypeHasPreferredIdentifier.PreferredIdentifierForMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|PreferredIdentifierFor", match);
				match.InitializeRoles(ObjectificationImpliesExternalUniquenessConstraint.ImpliedByObjectificationMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|ImpliedByObjectification", match);
				ExternalUniquenessConstraint.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // ExternalUniquenessConstraint serialization
	#region InternalConstraint serialization
	public partial class InternalConstraint : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.ChildElementInfo | (ORMCustomSerializedElementSupportedOperations.AttributeInfo | (ORMCustomSerializedElementSupportedOperations.LinkInfo | ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles)));
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
		protected new ORMCustomSerializedChildElementInfo[] GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedChildElementInfo[] ret = InternalConstraint.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ORMCustomSerializedChildElementInfo[] baseInfo = null;
				int baseInfoCount = 0;
				if (0 != (ORMCustomSerializedElementSupportedOperations.ChildElementInfo & base.SupportedCustomSerializedOperations))
				{
					baseInfo = base.GetCustomSerializedChildElementInfo();
					if (baseInfo != null)
					{
						baseInfoCount = baseInfo.Length;
					}
				}
				ret = new ORMCustomSerializedChildElementInfo[baseInfoCount + 1];
				if (!(baseInfoCount == 0))
				{
					baseInfo.CopyTo(ret, 1);
				}
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "RoleSequence", null, ORMCustomSerializedElementWriteStyle.Element, null, ConstraintRoleSequenceHasRole.RoleCollectionMetaRoleGuid);
				InternalConstraint.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		ORMCustomSerializedChildElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedAttributeInfo
		/// </summary>
		protected new ORMCustomSerializedAttributeInfo GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			if (attributeInfo.Id == InternalConstraint.ModalityMetaAttributeGuid)
			{
				if (ConstraintModality.Alethic == this.Modality)
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
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == ConstraintRoleSequenceHasRole.RoleCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Role", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (rolePlayedInfo.Id == InternalConstraintHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static IComparer<MetaRoleInfo> myCustomSortChildComparer;
		private class CustomSortChildComparer : IComparer<MetaRoleInfo>
		{
			private Dictionary<string, int> myRoleOrderDictionary;
			private IComparer<MetaRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<MetaRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				MetaDataDirectory metaDataDir = store.MetaDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				MetaRoleInfo metaRole;
				metaRole = metaDataDir.FindMetaRole(ConstraintRoleSequenceHasRole.RoleCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 0;
				metaRole = metaDataDir.FindMetaRole(InternalConstraintHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 1;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<MetaRoleInfo>.Compare(MetaRoleInfo x, MetaRoleInfo y)
			{
				if (this.myBaseComparer != null)
				{
					int baseOpinion = this.myBaseComparer.Compare(x, y);
					if (0 != baseOpinion)
					{
						return baseOpinion;
					}
				}
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
		protected new IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<MetaRoleInfo> retVal = InternalConstraint.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<MetaRoleInfo> baseComparer = null;
					if (0 != (ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					InternalConstraint.myCustomSortChildComparer = retVal;
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
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = InternalConstraint.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ConstraintRoleSequenceHasRole.RoleCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|RoleSequence|http://schemas.neumont.edu/ORM/ORMCore|Role", match);
				InternalConstraint.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
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
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = InternalConstraint.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("Modality", InternalConstraint.ModalityMetaAttributeGuid);
				InternalConstraint.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // InternalConstraint serialization
	#region InternalUniquenessConstraint serialization
	public partial class InternalUniquenessConstraint : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.LinkInfo | ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles);
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == EntityTypeHasPreferredIdentifier.PreferredIdentifierForMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "PreferredIdentifierFor", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (rolePlayedInfo.Id == InternalUniquenessConstraintHasNMinusOneError.NMinusOneErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static IComparer<MetaRoleInfo> myCustomSortChildComparer;
		private class CustomSortChildComparer : IComparer<MetaRoleInfo>
		{
			private Dictionary<string, int> myRoleOrderDictionary;
			private IComparer<MetaRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<MetaRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				MetaDataDirectory metaDataDir = store.MetaDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				MetaRoleInfo metaRole;
				metaRole = metaDataDir.FindMetaRole(EntityTypeHasPreferredIdentifier.PreferredIdentifierForMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 0;
				metaRole = metaDataDir.FindMetaRole(InternalUniquenessConstraintHasNMinusOneError.NMinusOneErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 1;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<MetaRoleInfo>.Compare(MetaRoleInfo x, MetaRoleInfo y)
			{
				if (this.myBaseComparer != null)
				{
					int baseOpinion = this.myBaseComparer.Compare(x, y);
					if (0 != baseOpinion)
					{
						return baseOpinion;
					}
				}
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
		protected new IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<MetaRoleInfo> retVal = InternalUniquenessConstraint.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<MetaRoleInfo> baseComparer = null;
					if (0 != (ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					InternalUniquenessConstraint.myCustomSortChildComparer = retVal;
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
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = InternalUniquenessConstraint.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(EntityTypeHasPreferredIdentifier.PreferredIdentifierForMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|PreferredIdentifierFor", match);
				InternalUniquenessConstraint.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // InternalUniquenessConstraint serialization
	#region EqualityConstraint serialization
	public partial class EqualityConstraint : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.ElementInfo | (ORMCustomSerializedElementSupportedOperations.LinkInfo | ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles));
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
		/// Implements IORMCustomSerializedElement.CustomSerializedElementInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				string name = null;
				if (this.ImpliedByObjectification != null)
				{
					name = "ImpliedEqualityConstraint";
				}
				return new ORMCustomSerializedElementInfo(null, name, null, ORMCustomSerializedElementWriteStyle.Element, null);
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == ObjectificationImpliesEqualityConstraint.ImpliedByObjectificationMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ImpliedByObjectification", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (rolePlayedInfo.Id == EqualityConstraintHasEqualityIsImpliedByMandatoryError.EqualityIsImpliedByMandatoryErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static IComparer<MetaRoleInfo> myCustomSortChildComparer;
		private class CustomSortChildComparer : IComparer<MetaRoleInfo>
		{
			private Dictionary<string, int> myRoleOrderDictionary;
			private IComparer<MetaRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<MetaRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				MetaDataDirectory metaDataDir = store.MetaDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				MetaRoleInfo metaRole;
				metaRole = metaDataDir.FindMetaRole(ObjectificationImpliesEqualityConstraint.ImpliedByObjectificationMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 0;
				metaRole = metaDataDir.FindMetaRole(EqualityConstraintHasEqualityIsImpliedByMandatoryError.EqualityIsImpliedByMandatoryErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 1;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<MetaRoleInfo>.Compare(MetaRoleInfo x, MetaRoleInfo y)
			{
				if (this.myBaseComparer != null)
				{
					int baseOpinion = this.myBaseComparer.Compare(x, y);
					if (0 != baseOpinion)
					{
						return baseOpinion;
					}
				}
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
		protected new IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<MetaRoleInfo> retVal = EqualityConstraint.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<MetaRoleInfo> baseComparer = null;
					if (0 != (ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					EqualityConstraint.myCustomSortChildComparer = retVal;
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
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = EqualityConstraint.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ObjectificationImpliesEqualityConstraint.ImpliedByObjectificationMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|ImpliedByObjectification", match);
				EqualityConstraint.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // EqualityConstraint serialization
	#region RingConstraint serialization
	public partial class RingConstraint : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.AttributeInfo | ORMCustomSerializedElementSupportedOperations.LinkInfo);
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
			if (attributeInfo.Id == RingConstraint.RingTypeMetaAttributeGuid)
			{
				return new ORMCustomSerializedAttributeInfo(null, "Type", null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
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
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == RingConstraintHasRingConstraintTypeNotSpecifiedError.RingConstraintTypeNotSpecifiedErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapAttribute
		/// </summary>
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = RingConstraint.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("Type", RingConstraint.RingTypeMetaAttributeGuid);
				RingConstraint.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // RingConstraint serialization
	#region ConstraintDuplicateNameError serialization
	public partial class ConstraintDuplicateNameError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.ChildElementInfo | ORMCustomSerializedElementSupportedOperations.LinkInfo);
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
		protected new ORMCustomSerializedChildElementInfo[] GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedChildElementInfo[] ret = ConstraintDuplicateNameError.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ORMCustomSerializedChildElementInfo[] baseInfo = null;
				int baseInfoCount = 0;
				if (0 != (ORMCustomSerializedElementSupportedOperations.ChildElementInfo & base.SupportedCustomSerializedOperations))
				{
					baseInfo = base.GetCustomSerializedChildElementInfo();
					if (baseInfo != null)
					{
						baseInfoCount = baseInfo.Length;
					}
				}
				ret = new ORMCustomSerializedChildElementInfo[baseInfoCount + 1];
				if (!(baseInfoCount == 0))
				{
					baseInfo.CopyTo(ret, 1);
				}
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "Constraints", null, ORMCustomSerializedElementWriteStyle.Element, null, MultiColumnExternalConstraintHasDuplicateNameError.MultiColumnExternalConstraintCollectionMetaRoleGuid, SingleColumnExternalConstraintHasDuplicateNameError.SingleColumnExternalConstraintCollectionMetaRoleGuid, InternalConstraintHasDuplicateNameError.InternalConstraintCollectionMetaRoleGuid, ValueConstraintHasDuplicateNameError.ValueConstraintCollectionMetaRoleGuid);
				ConstraintDuplicateNameError.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		ORMCustomSerializedChildElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == MultiColumnExternalConstraintHasDuplicateNameError.MultiColumnExternalConstraintCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "MultiColumnConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (rolePlayedInfo.Id == SingleColumnExternalConstraintHasDuplicateNameError.SingleColumnExternalConstraintCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "SingleColumnConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (rolePlayedInfo.Id == InternalConstraintHasDuplicateNameError.InternalConstraintCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "InternalConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (rolePlayedInfo.Id == ValueConstraintHasDuplicateNameError.ValueConstraintCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ValueConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ConstraintDuplicateNameError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(MultiColumnExternalConstraintHasDuplicateNameError.MultiColumnExternalConstraintCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|Constraints|http://schemas.neumont.edu/ORM/ORMCore|MultiColumnConstraint", match);
				match.InitializeRoles(SingleColumnExternalConstraintHasDuplicateNameError.SingleColumnExternalConstraintCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|Constraints|http://schemas.neumont.edu/ORM/ORMCore|SingleColumnConstraint", match);
				match.InitializeRoles(InternalConstraintHasDuplicateNameError.InternalConstraintCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|Constraints|http://schemas.neumont.edu/ORM/ORMCore|InternalConstraint", match);
				match.InitializeRoles(ValueConstraintHasDuplicateNameError.ValueConstraintCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|Constraints|http://schemas.neumont.edu/ORM/ORMCore|ValueConstraint", match);
				ConstraintDuplicateNameError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // ConstraintDuplicateNameError serialization
	#region FactTypeDuplicateNameError serialization
	public partial class FactTypeDuplicateNameError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.ChildElementInfo | ORMCustomSerializedElementSupportedOperations.LinkInfo);
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
		protected new ORMCustomSerializedChildElementInfo[] GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedChildElementInfo[] ret = FactTypeDuplicateNameError.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ORMCustomSerializedChildElementInfo[] baseInfo = null;
				int baseInfoCount = 0;
				if (0 != (ORMCustomSerializedElementSupportedOperations.ChildElementInfo & base.SupportedCustomSerializedOperations))
				{
					baseInfo = base.GetCustomSerializedChildElementInfo();
					if (baseInfo != null)
					{
						baseInfoCount = baseInfo.Length;
					}
				}
				ret = new ORMCustomSerializedChildElementInfo[baseInfoCount + 1];
				if (!(baseInfoCount == 0))
				{
					baseInfo.CopyTo(ret, 1);
				}
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "Facts", null, ORMCustomSerializedElementWriteStyle.Element, null, FactTypeHasDuplicateNameError.FactTypeCollectionMetaRoleGuid);
				FactTypeDuplicateNameError.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		ORMCustomSerializedChildElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == FactTypeHasDuplicateNameError.FactTypeCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Fact", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = FactTypeDuplicateNameError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(FactTypeHasDuplicateNameError.FactTypeCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|Facts|http://schemas.neumont.edu/ORM/ORMCore|Fact", match);
				FactTypeDuplicateNameError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // FactTypeDuplicateNameError serialization
	#region ObjectTypeDuplicateNameError serialization
	public partial class ObjectTypeDuplicateNameError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.ChildElementInfo | ORMCustomSerializedElementSupportedOperations.LinkInfo);
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
		protected new ORMCustomSerializedChildElementInfo[] GetCustomSerializedChildElementInfo()
		{
			ORMCustomSerializedChildElementInfo[] ret = ObjectTypeDuplicateNameError.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ORMCustomSerializedChildElementInfo[] baseInfo = null;
				int baseInfoCount = 0;
				if (0 != (ORMCustomSerializedElementSupportedOperations.ChildElementInfo & base.SupportedCustomSerializedOperations))
				{
					baseInfo = base.GetCustomSerializedChildElementInfo();
					if (baseInfo != null)
					{
						baseInfoCount = baseInfo.Length;
					}
				}
				ret = new ORMCustomSerializedChildElementInfo[baseInfoCount + 1];
				if (!(baseInfoCount == 0))
				{
					baseInfo.CopyTo(ret, 1);
				}
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "Objects", null, ORMCustomSerializedElementWriteStyle.Element, null, ObjectTypeHasDuplicateNameError.ObjectTypeCollectionMetaRoleGuid);
				ObjectTypeDuplicateNameError.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		ORMCustomSerializedChildElementInfo[] IORMCustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == ObjectTypeHasDuplicateNameError.ObjectTypeCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Object", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ObjectTypeDuplicateNameError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ObjectTypeHasDuplicateNameError.ObjectTypeCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/ORMCore|Objects|http://schemas.neumont.edu/ORM/ORMCore|Object", match);
				ObjectTypeDuplicateNameError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // ObjectTypeDuplicateNameError serialization
	#region EntityTypeRequiresReferenceSchemeError serialization
	public partial class EntityTypeRequiresReferenceSchemeError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == ObjectTypeHasEntityTypeRequiresReferenceSchemeError.ObjectTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "EntityType", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = EntityTypeRequiresReferenceSchemeError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ObjectTypeHasEntityTypeRequiresReferenceSchemeError.ObjectTypeMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|EntityType", match);
				EntityTypeRequiresReferenceSchemeError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // EntityTypeRequiresReferenceSchemeError serialization
	#region ExternalConstraintRoleSequenceArityMismatchError serialization
	public partial class ExternalConstraintRoleSequenceArityMismatchError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == MultiColumnExternalConstraintHasExternalConstraintRoleSequenceArityMismatchError.ConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Constraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ExternalConstraintRoleSequenceArityMismatchError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(MultiColumnExternalConstraintHasExternalConstraintRoleSequenceArityMismatchError.ConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|Constraint", match);
				ExternalConstraintRoleSequenceArityMismatchError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // ExternalConstraintRoleSequenceArityMismatchError serialization
	#region ImpliedInternalUniquenessConstraintError serialization
	public partial class ImpliedInternalUniquenessConstraintError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == FactTypeHasImpliedInternalUniquenessConstraintError.FactTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Fact", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ImpliedInternalUniquenessConstraintError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(FactTypeHasImpliedInternalUniquenessConstraintError.FactTypeMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|Fact", match);
				ImpliedInternalUniquenessConstraintError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // ImpliedInternalUniquenessConstraintError serialization
	#region FactTypeRequiresInternalUniquenessConstraintError serialization
	public partial class FactTypeRequiresInternalUniquenessConstraintError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == FactTypeHasFactTypeRequiresInternalUniquenessConstraintError.FactTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Fact", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = FactTypeRequiresInternalUniquenessConstraintError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(FactTypeHasFactTypeRequiresInternalUniquenessConstraintError.FactTypeMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|Fact", match);
				FactTypeRequiresInternalUniquenessConstraintError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // FactTypeRequiresInternalUniquenessConstraintError serialization
	#region FactTypeRequiresReadingError serialization
	public partial class FactTypeRequiresReadingError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == FactTypeHasFactTypeRequiresReadingError.FactTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Fact", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = FactTypeRequiresReadingError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(FactTypeHasFactTypeRequiresReadingError.FactTypeMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|Fact", match);
				FactTypeRequiresReadingError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // FactTypeRequiresReadingError serialization
	#region FrequencyConstraintMinMaxError serialization
	public partial class FrequencyConstraintMinMaxError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == FrequencyConstraintHasFrequencyConstraintMinMaxError.FrequencyConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "FrequencyConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = FrequencyConstraintMinMaxError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(FrequencyConstraintHasFrequencyConstraintMinMaxError.FrequencyConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|FrequencyConstraint", match);
				FrequencyConstraintMinMaxError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // FrequencyConstraintMinMaxError serialization
	#region MinValueMismatchError serialization
	public partial class MinValueMismatchError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == ValueRangeHasMinValueMismatchError.ValueRangeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ValueRange", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = MinValueMismatchError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ValueRangeHasMinValueMismatchError.ValueRangeMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|ValueRange", match);
				MinValueMismatchError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // MinValueMismatchError serialization
	#region MaxValueMismatchError serialization
	public partial class MaxValueMismatchError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == ValueRangeHasMaxValueMismatchError.ValueRangeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ValueRange", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = MaxValueMismatchError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ValueRangeHasMaxValueMismatchError.ValueRangeMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|ValueRange", match);
				MaxValueMismatchError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // MaxValueMismatchError serialization
	#region RingConstraintTypeNotSpecifiedError serialization
	public partial class RingConstraintTypeNotSpecifiedError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == RingConstraintHasRingConstraintTypeNotSpecifiedError.RingConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "RingConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = RingConstraintTypeNotSpecifiedError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(RingConstraintHasRingConstraintTypeNotSpecifiedError.RingConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|RingConstraint", match);
				RingConstraintTypeNotSpecifiedError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // RingConstraintTypeNotSpecifiedError serialization
	#region TooFewReadingRolesError serialization
	public partial class TooFewReadingRolesError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == ReadingHasTooFewRolesError.ReadingMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Reading", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = TooFewReadingRolesError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ReadingHasTooFewRolesError.ReadingMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|Reading", match);
				TooFewReadingRolesError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // TooFewReadingRolesError serialization
	#region TooFewRoleSequencesError serialization
	public partial class TooFewRoleSequencesError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == MultiColumnExternalConstraintHasTooFewRoleSequencesError.MultiColumnConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "MultiColumnConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (rolePlayedInfo.Id == SingleColumnExternalConstraintHasTooFewRoleSequencesError.SingleColumnConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "SingleColumnConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = TooFewRoleSequencesError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(MultiColumnExternalConstraintHasTooFewRoleSequencesError.MultiColumnConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|MultiColumnConstraint", match);
				match.InitializeRoles(SingleColumnExternalConstraintHasTooFewRoleSequencesError.SingleColumnConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|SingleColumnConstraint", match);
				TooFewRoleSequencesError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // TooFewRoleSequencesError serialization
	#region TooManyReadingRolesError serialization
	public partial class TooManyReadingRolesError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == ReadingHasTooManyRolesError.ReadingMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Reading", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = TooManyReadingRolesError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ReadingHasTooManyRolesError.ReadingMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|Reading", match);
				TooManyReadingRolesError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // TooManyReadingRolesError serialization
	#region TooManyRoleSequencesError serialization
	public partial class TooManyRoleSequencesError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == MultiColumnExternalConstraintHasTooManyRoleSequencesError.MultiColumnConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "MultiColumnConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (rolePlayedInfo.Id == SingleColumnExternalConstraintHasTooManyRoleSequencesError.SingleColumnConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "SingleColumnConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = TooManyRoleSequencesError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(MultiColumnExternalConstraintHasTooManyRoleSequencesError.MultiColumnConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|MultiColumnConstraint", match);
				match.InitializeRoles(SingleColumnExternalConstraintHasTooManyRoleSequencesError.SingleColumnConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|SingleColumnConstraint", match);
				TooManyRoleSequencesError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // TooManyRoleSequencesError serialization
	#region DataTypeNotSpecifiedError serialization
	public partial class DataTypeNotSpecifiedError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == ValueTypeHasUnspecifiedDataTypeError.ValueTypeHasDataTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ConceptualDataType", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = DataTypeNotSpecifiedError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ValueTypeHasUnspecifiedDataTypeError.ValueTypeHasDataTypeMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|ConceptualDataType", match);
				DataTypeNotSpecifiedError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // DataTypeNotSpecifiedError serialization
	#region EqualityIsImpliedByMandatoryError serialization
	public partial class EqualityIsImpliedByMandatoryError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == EqualityConstraintHasEqualityIsImpliedByMandatoryError.EqualityConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "EqualityConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = EqualityIsImpliedByMandatoryError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(EqualityConstraintHasEqualityIsImpliedByMandatoryError.EqualityConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|EqualityConstraint", match);
				EqualityIsImpliedByMandatoryError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // EqualityIsImpliedByMandatoryError serialization
	#region NMinusOneError serialization
	public partial class NMinusOneError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == InternalUniquenessConstraintHasNMinusOneError.ConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "InternalUniquenessConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = NMinusOneError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(InternalUniquenessConstraintHasNMinusOneError.ConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|InternalUniquenessConstraint", match);
				NMinusOneError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // NMinusOneError serialization
	#region SimpleMandatoryImpliesDisjunctiveMandatoryError serialization
	public partial class SimpleMandatoryImpliesDisjunctiveMandatoryError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == DisjunctiveMandatoryConstraintHasSimpleMandatoryImpliesDisjunctiveMandatoryError.DisjunctiveMandatoryConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "DisjunctiveMandatoryConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = SimpleMandatoryImpliesDisjunctiveMandatoryError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(DisjunctiveMandatoryConstraintHasSimpleMandatoryImpliesDisjunctiveMandatoryError.DisjunctiveMandatoryConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|DisjunctiveMandatoryConstraint", match);
				SimpleMandatoryImpliesDisjunctiveMandatoryError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // SimpleMandatoryImpliesDisjunctiveMandatoryError serialization
	#region CompatibleRolePlayerTypeError serialization
	public partial class CompatibleRolePlayerTypeError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.AttributeInfo | ORMCustomSerializedElementSupportedOperations.LinkInfo);
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
			if (attributeInfo.Id == CompatibleRolePlayerTypeError.ColumnMetaAttributeGuid)
			{
				if (this.SingleColumnExternalConstraint != null)
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
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == MultiColumnExternalConstraintHasCompatibleRolePlayerTypeError.MultiColumnExternalConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "MultiColumnConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (rolePlayedInfo.Id == SingleColumnExternalConstraintHasCompatibleRolePlayerTypeError.SingleColumnExternalConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "SingleColumnConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = CompatibleRolePlayerTypeError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(MultiColumnExternalConstraintHasCompatibleRolePlayerTypeError.MultiColumnExternalConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|MultiColumnConstraint", match);
				match.InitializeRoles(SingleColumnExternalConstraintHasCompatibleRolePlayerTypeError.SingleColumnExternalConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|SingleColumnConstraint", match);
				CompatibleRolePlayerTypeError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
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
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = CompatibleRolePlayerTypeError.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("Column", CompatibleRolePlayerTypeError.ColumnMetaAttributeGuid);
				CompatibleRolePlayerTypeError.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // CompatibleRolePlayerTypeError serialization
	#region RolePlayerRequiredError serialization
	public partial class RolePlayerRequiredError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == RoleHasRolePlayerRequiredError.RoleMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Role", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = RolePlayerRequiredError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(RoleHasRolePlayerRequiredError.RoleMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|Role", match);
				RolePlayerRequiredError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // RolePlayerRequiredError serialization
	#region FrequencyConstraintContradictsInternalUniquenessConstraintError serialization
	public partial class FrequencyConstraintContradictsInternalUniquenessConstraintError : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | ORMCustomSerializedElementSupportedOperations.LinkInfo;
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedLinkInfo
		/// </summary>
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			if (rolePlayedInfo.Id == FactTypeHasFrequencyConstraintContradictsInternalUniquenessConstraintError.FactTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Fact", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (rolePlayedInfo.Id == FrequencyConstraintHasFrequencyConstraintInvalidatedByInternalUniquenessConstraintError.FrequencyConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "FrequencyConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = FrequencyConstraintContradictsInternalUniquenessConstraintError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(FactTypeHasFrequencyConstraintContradictsInternalUniquenessConstraintError.FactTypeMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|Fact", match);
				match.InitializeRoles(FrequencyConstraintHasFrequencyConstraintInvalidatedByInternalUniquenessConstraintError.FrequencyConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/ORMCore|FrequencyConstraint", match);
				FrequencyConstraintContradictsInternalUniquenessConstraintError.myChildElementMappings = childElementMappings;
			}
			ORMCustomSerializedElementMatch rVal;
			if (!(childElementMappings.TryGetValue(string.Concat(containerNamespace, "|", containerName, "|", elementNamespace, "|", elementName), out rVal)))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
			}
			return rVal;
		}
		ORMCustomSerializedElementMatch IORMCustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName);
		}
	}
	#endregion // FrequencyConstraintContradictsInternalUniquenessConstraintError serialization
}
