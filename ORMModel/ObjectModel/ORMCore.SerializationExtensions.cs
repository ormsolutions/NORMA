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
namespace Neumont.Tools.ORM.ObjectModel
{
	#region ORMMetaModel model serialization
	public partial class ORMMetaModel : IORMCustomSerializedMetaModel
	{
		/// <summary>
		/// The default XmlNamespace associated with the 'ORMMetaModel' extension model
		/// </summary>
		public const string XmlNamespace = "http://schemas.neumont.edu/ORM/2006-04/ORMCore";
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
			ret[0, 1] = "http://schemas.neumont.edu/ORM/2006-04/ORMCore";
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
			retVal[dataDir.FindMetaRelationship(FactSetComparisonConstraint.MetaRelationshipGuid)] = null;
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
			if ((elementName == "ORMModel") && (xmlNamespace == "http://schemas.neumont.edu/ORM/2006-04/ORMCore"))
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
				validNamespaces.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore");
				ORMMetaModel.myValidNamespaces = validNamespaces;
			}
			if (classNameMap == null)
			{
				classNameMap = new Dictionary<string, Guid>();
				classNameMap.Add("ORMNamedElement", ORMNamedElement.MetaClassGuid);
				classNameMap.Add("ORMModelElement", ORMModelElement.MetaClassGuid);
				classNameMap.Add("ORMModel", ORMModel.MetaClassGuid);
				classNameMap.Add("Note", Note.MetaClassGuid);
				classNameMap.Add("EntityType", ObjectType.MetaClassGuid);
				classNameMap.Add("ValueType", ObjectType.MetaClassGuid);
				classNameMap.Add("ObjectifiedType", ObjectType.MetaClassGuid);
				classNameMap.Add("CustomReferenceMode", CustomReferenceMode.MetaClassGuid);
				classNameMap.Add("ValueTypeHasDataType", ValueTypeHasDataType.MetaClassGuid);
				classNameMap.Add("DataType", DataType.MetaClassGuid);
				classNameMap.Add("ReferenceModeKind", ReferenceModeKind.MetaClassGuid);
				classNameMap.Add("BaseValueConstraint", ValueConstraint.MetaClassGuid);
				classNameMap.Add("ValueConstraint", ValueTypeValueConstraint.MetaClassGuid);
				classNameMap.Add("RoleValueConstraint", RoleValueConstraint.MetaClassGuid);
				classNameMap.Add("ValueRange", ValueRange.MetaClassGuid);
				classNameMap.Add("Fact", FactType.MetaClassGuid);
				classNameMap.Add("ImpliedFact", FactType.MetaClassGuid);
				classNameMap.Add("Expression", Expression.MetaClassGuid);
				classNameMap.Add("DerivationExpression", FactTypeDerivationExpression.MetaClassGuid);
				classNameMap.Add("SubtypeFact", SubtypeFact.MetaClassGuid);
				classNameMap.Add("Objectification", Objectification.MetaClassGuid);
				classNameMap.Add("ReadingOrder", ReadingOrder.MetaClassGuid);
				classNameMap.Add("Reading", Reading.MetaClassGuid);
				classNameMap.Add("Role", Role.MetaClassGuid);
				classNameMap.Add("RoleProxy", RoleProxy.MetaClassGuid);
				classNameMap.Add("SetComparisonConstraint", SetComparisonConstraint.MetaClassGuid);
				classNameMap.Add("RoleSequence", SetComparisonConstraintRoleSequence.MetaClassGuid);
				classNameMap.Add("SetConstraint", SetConstraint.MetaClassGuid);
				classNameMap.Add("MandatoryConstraint", MandatoryConstraint.MetaClassGuid);
				classNameMap.Add("FrequencyConstraint", FrequencyConstraint.MetaClassGuid);
				classNameMap.Add("UniquenessConstraint", UniquenessConstraint.MetaClassGuid);
				classNameMap.Add("EqualityConstraint", EqualityConstraint.MetaClassGuid);
				classNameMap.Add("RingConstraint", RingConstraint.MetaClassGuid);
				classNameMap.Add("ConstraintDuplicateNameError", ConstraintDuplicateNameError.MetaClassGuid);
				classNameMap.Add("ObjectTypeDuplicateNameError", ObjectTypeDuplicateNameError.MetaClassGuid);
				classNameMap.Add("EntityTypeRequiresReferenceSchemeError", EntityTypeRequiresReferenceSchemeError.MetaClassGuid);
				classNameMap.Add("ExternalConstraintRoleSequenceArityMismatchError", ExternalConstraintRoleSequenceArityMismatchError.MetaClassGuid);
				classNameMap.Add("ImpliedInternalUniquenessConstraintError", ImpliedInternalUniquenessConstraintError.MetaClassGuid);
				classNameMap.Add("FactTypeRequiresInternalUniquenessConstraintError", FactTypeRequiresInternalUniquenessConstraintError.MetaClassGuid);
				classNameMap.Add("FactTypeRequiresReadingError", FactTypeRequiresReadingError.MetaClassGuid);
				classNameMap.Add("FrequencyConstraintMinMaxError", FrequencyConstraintMinMaxError.MetaClassGuid);
				classNameMap.Add("MinValueMismatchError", MinValueMismatchError.MetaClassGuid);
				classNameMap.Add("MaxValueMismatchError", MaxValueMismatchError.MetaClassGuid);
				classNameMap.Add("ValueRangeOverlapError", ValueRangeOverlapError.MetaClassGuid);
				classNameMap.Add("RingConstraintTypeNotSpecifiedError", RingConstraintTypeNotSpecifiedError.MetaClassGuid);
				classNameMap.Add("TooFewReadingRolesError", TooFewReadingRolesError.MetaClassGuid);
				classNameMap.Add("TooFewRoleSequencesError", TooFewRoleSequencesError.MetaClassGuid);
				classNameMap.Add("TooManyReadingRolesError", TooManyReadingRolesError.MetaClassGuid);
				classNameMap.Add("TooManyRoleSequencesError", TooManyRoleSequencesError.MetaClassGuid);
				classNameMap.Add("DataTypeNotSpecifiedError", DataTypeNotSpecifiedError.MetaClassGuid);
				classNameMap.Add("EqualityImpliedByMandatoryError", EqualityImpliedByMandatoryError.MetaClassGuid);
				classNameMap.Add("NMinusOneError", NMinusOneError.MetaClassGuid);
				classNameMap.Add("MandatoryImpliedByMandatoryError", MandatoryImpliedByMandatoryError.MetaClassGuid);
				classNameMap.Add("ObjectTypeRequiresPrimarySupertypeError", ObjectTypeRequiresPrimarySupertypeError.MetaClassGuid);
				classNameMap.Add("PreferredIdentifierRequiresMandatoryError", PreferredIdentifierRequiresMandatoryError.MetaClassGuid);
				classNameMap.Add("CompatibleSupertypesError", CompatibleSupertypesError.MetaClassGuid);
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
		protected ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			throw new NotSupportedException();
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
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ORMNamedElement.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ORMNamedElementHasExtensionElement.ExtensionCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|Extensions||", match);
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
		protected ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			throw new NotSupportedException();
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
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ORMModelElement.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ORMModelElementHasExtensionElement.ExtensionCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|Extensions||", match);
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
				if (baseInfoCount != 0)
				{
					baseInfo.CopyTo(ret, 7);
				}
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "Objects", null, ORMCustomSerializedElementWriteStyle.Element, null, ModelHasObjectType.ObjectTypeCollectionMetaRoleGuid);
				ret[1] = new ORMCustomSerializedChildElementInfo(null, "Facts", null, ORMCustomSerializedElementWriteStyle.Element, null, ModelHasFactType.FactTypeCollectionMetaRoleGuid);
				ret[2] = new ORMCustomSerializedChildElementInfo(null, "Constraints", null, ORMCustomSerializedElementWriteStyle.Element, null, ModelHasSetComparisonConstraint.SetComparisonConstraintCollectionMetaRoleGuid, ModelHasSetConstraint.SetConstraintCollectionMetaRoleGuid);
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
				metaRole = metaDataDir.FindMetaRole(ModelHasSetComparisonConstraint.SetComparisonConstraintCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 2;
				metaRole = metaDataDir.FindMetaRole(ModelHasSetConstraint.SetConstraintCollectionMetaRoleGuid);
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
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|Objects||", match);
				match.InitializeRoles(ModelHasFactType.FactTypeCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|Facts||", match);
				match.InitializeRoles(ModelHasSetComparisonConstraint.SetComparisonConstraintCollectionMetaRoleGuid, ModelHasSetConstraint.SetConstraintCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|Constraints||", match);
				match.InitializeRoles(ModelHasDataType.DataTypeCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|DataTypes||", match);
				match.InitializeRoles(ModelHasReferenceMode.ReferenceModeCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|CustomReferenceModes||", match);
				match.InitializeRoles(ModelHasError.ErrorCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|ModelErrors||", match);
				match.InitializeRoles(ModelHasReferenceModeKind.ReferenceModeKindCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|ReferenceModeKinds||", match);
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
	#region Note serialization
	public partial class Note : IORMCustomSerializedElement
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
			if (attributeInfo.Id == Note.TextMetaAttributeGuid)
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
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = Note.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeAttribute(Note.TextMetaAttributeGuid, null);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Text", match);
				Note.myChildElementMappings = childElementMappings;
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
	#endregion // Note serialization
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
				ret = new ORMCustomSerializedChildElementInfo[baseInfoCount + 3];
				if (baseInfoCount != 0)
				{
					baseInfo.CopyTo(ret, 3);
				}
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "Notes", null, ORMCustomSerializedElementWriteStyle.Element, null, ObjectTypeHasNote.NoteMetaRoleGuid);
				ret[1] = new ORMCustomSerializedChildElementInfo(null, "PlayedRoles", null, ORMCustomSerializedElementWriteStyle.Element, null, ObjectTypePlaysRole.PlayedRoleCollectionMetaRoleGuid);
				ret[2] = new ORMCustomSerializedChildElementInfo(null, "ValueRestriction", null, ORMCustomSerializedElementWriteStyle.Element, null, ValueTypeHasValueConstraint.ValueConstraintMetaRoleGuid);
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
				return new ORMCustomSerializedAttributeInfo(null, "_ReferenceMode", null, true, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (attributeInfo.Id == ObjectType.IsIndependentMetaAttributeGuid)
			{
				if (!(this.IsIndependent))
				{
					return new ORMCustomSerializedAttributeInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new ORMCustomSerializedAttributeInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (attributeInfo.Id == ObjectType.IsExternalMetaAttributeGuid)
			{
				if (!(this.IsExternal))
				{
					return new ORMCustomSerializedAttributeInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new ORMCustomSerializedAttributeInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.Attribute, null);
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ValueTypeHasDataType.DataTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ConceptualDataType", null, ORMCustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == ObjectTypePlaysRole.PlayedRoleCollectionMetaRoleGuid)
			{
				string name = "Role";
				if (elementLink.GetRolePlayer(ObjectTypePlaysRole.PlayedRoleCollectionMetaRoleGuid) is SubtypeMetaRole)
				{
					name = "SubtypeMetaRole";
				}
				else if (elementLink.GetRolePlayer(ObjectTypePlaysRole.PlayedRoleCollectionMetaRoleGuid) is SupertypeMetaRole)
				{
					name = "SupertypeMetaRole";
				}
				return new ORMCustomSerializedElementInfo(null, name, null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == EntityTypeHasPreferredIdentifier.PreferredIdentifierMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "PreferredIdentifier", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == Objectification.NestedFactTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "NestedPredicate", null, ORMCustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == ObjectTypeHasEntityTypeRequiresReferenceSchemeError.ReferenceSchemeErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ObjectTypeHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ObjectTypeHasObjectTypeRequiresPrimarySupertypeError.ObjectTypeRequiresPrimarySupertypeErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ObjectTypeHasPreferredIdentifierRequiresMandatoryError.PreferredIdentifierRequiresMandatoryErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ObjectTypeHasCompatibleSupertypesError.CompatibleSupertypesErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
			private IComparer<MetaRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<MetaRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				MetaDataDirectory metaDataDir = store.MetaDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				MetaRoleInfo metaRole;
				metaRole = metaDataDir.FindMetaRole(ObjectTypeHasNote.NoteMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 0;
				metaRole = metaDataDir.FindMetaRole(ObjectTypePlaysRole.PlayedRoleCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 1;
				metaRole = metaDataDir.FindMetaRole(ValueTypeHasDataType.DataTypeMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 2;
				metaRole = metaDataDir.FindMetaRole(ValueTypeHasValueConstraint.ValueConstraintMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 3;
				metaRole = metaDataDir.FindMetaRole(EntityTypeHasPreferredIdentifier.PreferredIdentifierMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 4;
				metaRole = metaDataDir.FindMetaRole(Objectification.NestedFactTypeMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 5;
				metaRole = metaDataDir.FindMetaRole(ObjectTypeHasEntityTypeRequiresReferenceSchemeError.ReferenceSchemeErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 6;
				metaRole = metaDataDir.FindMetaRole(ObjectTypeHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 7;
				metaRole = metaDataDir.FindMetaRole(ObjectTypeHasObjectTypeRequiresPrimarySupertypeError.ObjectTypeRequiresPrimarySupertypeErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 8;
				metaRole = metaDataDir.FindMetaRole(ObjectTypeHasPreferredIdentifierRequiresMandatoryError.PreferredIdentifierRequiresMandatoryErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 9;
				metaRole = metaDataDir.FindMetaRole(ObjectTypeHasCompatibleSupertypesError.CompatibleSupertypesErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 10;
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
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ConceptualDataType", match);
				match.InitializeRoles(EntityTypeHasPreferredIdentifier.PreferredIdentifierMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|PreferredIdentifier", match);
				match.InitializeRoles(Objectification.NestedFactTypeMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|NestedPredicate", match);
				match.InitializeRoles(ObjectTypePlaysRole.PlayedRoleCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|PlayedRoles|http://schemas.neumont.edu/ORM/2006-04/ORMCore|Role", match);
				match.InitializeRoles(ObjectTypeHasNote.NoteMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|Notes||", match);
				match.InitializeRoles(ValueTypeHasValueConstraint.ValueConstraintMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|ValueRestriction||", match);
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
				customSerializedAttributes.Add("IsIndependent", ObjectType.IsIndependentMetaAttributeGuid);
				customSerializedAttributes.Add("IsExternal", ObjectType.IsExternalMetaAttributeGuid);
				customSerializedAttributes.Add("IsPersonal", ObjectType.IsPersonalMetaAttributeGuid);
				ObjectType.myCustomSerializedAttributes = customSerializedAttributes;
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ReferenceModeHasReferenceModeKind.KindMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Kind", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Kind", match);
				match.InitializeAttribute(CustomReferenceMode.CustomFormatStringMetaAttributeGuid, null);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|CustomFormatString", match);
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
		protected ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ValueTypeHasUnspecifiedDataTypeError.DataTypeNotSpecifiedErrorMetaRoleGuid)
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ValueTypeHasDataType.ValueTypeCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ReferenceModeHasReferenceModeKind.ReferenceModeCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				if (baseInfoCount != 0)
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
				return new ORMCustomSerializedElementInfo(null, "BaseValueConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ValueConstraintHasDuplicateNameError.DuplicateNameErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ValueConstraintHasValueRangeOverlapError.ValueRangeOverlapErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|ValueRanges||", match);
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
				return new ORMCustomSerializedElementInfo(null, "ValueConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
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
				return new ORMCustomSerializedElementInfo(null, "RoleValueConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ValueRangeHasMinValueMismatchError.MinValueMismatchErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ValueRangeHasMaxValueMismatchError.MaxValueMismatchErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				ret = new ORMCustomSerializedChildElementInfo[baseInfoCount + 5];
				if (baseInfoCount != 0)
				{
					baseInfo.CopyTo(ret, 5);
				}
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "Notes", null, ORMCustomSerializedElementWriteStyle.Element, null, FactTypeHasNote.NoteMetaRoleGuid);
				ret[1] = new ORMCustomSerializedChildElementInfo(null, "FactRoles", null, ORMCustomSerializedElementWriteStyle.Element, null, FactTypeHasRole.RoleCollectionMetaRoleGuid);
				ret[2] = new ORMCustomSerializedChildElementInfo(null, "ReadingOrders", null, ORMCustomSerializedElementWriteStyle.Element, null, FactTypeHasReadingOrder.ReadingOrderCollectionMetaRoleGuid);
				ret[3] = new ORMCustomSerializedChildElementInfo(null, "InternalConstraints", null, ORMCustomSerializedElementWriteStyle.Element, null, FactSetConstraint.SetConstraintCollectionMetaRoleGuid);
				ret[4] = new ORMCustomSerializedChildElementInfo(null, "DerivationRule", null, ORMCustomSerializedElementWriteStyle.Element, null, FactTypeHasDerivationExpression.DerivationRuleMetaRoleGuid);
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedAttributeInfo
		/// </summary>
		protected new ORMCustomSerializedAttributeInfo GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			if (attributeInfo.Id == FactType.IsExternalMetaAttributeGuid)
			{
				if (!(this.IsExternal))
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == FactSetConstraint.SetConstraintCollectionMetaRoleGuid)
			{
				string name = null;
				ORMCustomSerializedElementWriteStyle writeStyle = ORMCustomSerializedElementWriteStyle.NotWritten;
				if (ConstraintType.InternalUniqueness == ((IConstraint)elementLink.GetRolePlayer(FactSetConstraint.SetConstraintCollectionMetaRoleGuid)).ConstraintType)
				{
					name = "UniquenessConstraint";
					writeStyle = ORMCustomSerializedElementWriteStyle.Element;
				}
				else if (ConstraintType.SimpleMandatory == ((IConstraint)elementLink.GetRolePlayer(FactSetConstraint.SetConstraintCollectionMetaRoleGuid)).ConstraintType)
				{
					name = "MandatoryConstraint";
					writeStyle = ORMCustomSerializedElementWriteStyle.Element;
				}
				return new ORMCustomSerializedElementInfo(null, name, null, writeStyle, null);
			}
			if (roleId == ObjectificationImpliesFactType.ImpliedByObjectificationMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ImpliedByObjectification", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == Objectification.NestingTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FactTypeHasFactTypeRequiresInternalUniquenessConstraintError.InternalUniquenessConstraintRequiredErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FactTypeHasFactTypeRequiresReadingError.ReadingRequiredErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FactTypeHasImpliedInternalUniquenessConstraintError.ImpliedInternalUniquenessConstraintErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FactTypeHasFrequencyConstraintContradictsInternalUniquenessConstraintError.FrequencyConstraintContradictsInternalUniquenessConstraintErrorCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
			private IComparer<MetaRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<MetaRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				MetaDataDirectory metaDataDir = store.MetaDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				MetaRoleInfo metaRole;
				metaRole = metaDataDir.FindMetaRole(FactTypeHasNote.NoteMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 0;
				metaRole = metaDataDir.FindMetaRole(FactTypeHasRole.RoleCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 1;
				metaRole = metaDataDir.FindMetaRole(FactTypeHasReadingOrder.ReadingOrderCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 2;
				metaRole = metaDataDir.FindMetaRole(FactSetConstraint.SetConstraintCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 3;
				metaRole = metaDataDir.FindMetaRole(FactTypeHasDerivationExpression.DerivationRuleMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 4;
				metaRole = metaDataDir.FindMetaRole(ObjectificationImpliesFactType.ImpliedByObjectificationMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 5;
				metaRole = metaDataDir.FindMetaRole(Objectification.NestingTypeMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 6;
				metaRole = metaDataDir.FindMetaRole(FactTypeHasFactTypeRequiresInternalUniquenessConstraintError.InternalUniquenessConstraintRequiredErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 7;
				metaRole = metaDataDir.FindMetaRole(FactTypeHasFactTypeRequiresReadingError.ReadingRequiredErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 8;
				metaRole = metaDataDir.FindMetaRole(FactTypeHasImpliedInternalUniquenessConstraintError.ImpliedInternalUniquenessConstraintErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 9;
				metaRole = metaDataDir.FindMetaRole(FactTypeHasFrequencyConstraintContradictsInternalUniquenessConstraintError.FrequencyConstraintContradictsInternalUniquenessConstraintErrorCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 10;
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
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ImpliedByObjectification", match);
				match.InitializeRoles(FactTypeHasNote.NoteMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|Notes||", match);
				match.InitializeRoles(FactTypeHasRole.RoleCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|FactRoles||", match);
				match.InitializeRoles(FactTypeHasReadingOrder.ReadingOrderCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|ReadingOrders||", match);
				match.InitializeRoles(FactSetConstraint.SetConstraintCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|InternalConstraints||", match);
				match.InitializeRoles(FactTypeHasDerivationExpression.DerivationRuleMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|DerivationRule||", match);
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
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapAttribute
		/// </summary>
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = FactType.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("IsExternal", FactType.IsExternalMetaAttributeGuid);
				FactType.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // FactType serialization
	#region Expression serialization
	public partial class Expression : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.AttributeInfo | ORMCustomSerializedElementSupportedOperations.MixedTypedAttributes);
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
			if (attributeInfo.Id == Expression.LanguageMetaAttributeGuid)
			{
				return new ORMCustomSerializedAttributeInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
			}
			if (attributeInfo.Id == Expression.BodyMetaAttributeGuid)
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
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = Expression.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeAttribute(Expression.BodyMetaAttributeGuid, null);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Body", match);
				Expression.myChildElementMappings = childElementMappings;
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
	#endregion // Expression serialization
	#region FactTypeDerivationExpression serialization
	public partial class FactTypeDerivationExpression : IORMCustomSerializedElement
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
				return new ORMCustomSerializedElementInfo(null, "DerivationExpression", null, ORMCustomSerializedElementWriteStyle.Element, null);
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
	#endregion // FactTypeDerivationExpression serialization
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
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.ElementInfo | ORMCustomSerializedElementSupportedOperations.AttributeInfo);
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
		/// <summary>
		/// Implements IORMCustomSerializedElement.GetCustomSerializedAttributeInfo
		/// </summary>
		protected new ORMCustomSerializedAttributeInfo GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			if (attributeInfo.Id == SubtypeFact.IsPrimaryMetaAttributeGuid)
			{
				if (!(this.IsPrimary))
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
			Dictionary<string, Guid> customSerializedAttributes = SubtypeFact.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("IsPrimary", SubtypeFact.IsPrimaryMetaAttributeGuid);
				SubtypeFact.myCustomSerializedAttributes = customSerializedAttributes;
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
			if (attributeInfo.Id == Objectification.IsImpliedMetaAttributeGuid)
			{
				if (!(this.IsImplied))
				{
					return new ORMCustomSerializedAttributeInfo(null, null, null, false, ORMCustomSerializedAttributeWriteStyle.NotWritten, null);
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
		protected ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ObjectificationImpliesFactType.ImpliedFactTypeCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ImpliedByObjectification", null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
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
			Dictionary<string, Guid> customSerializedAttributes = Objectification.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("IsImplied", Objectification.IsImpliedMetaAttributeGuid);
				Objectification.myCustomSerializedAttributes = customSerializedAttributes;
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
				if (baseInfoCount != 0)
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ReadingOrderHasRole.RoleCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Role", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|RoleSequence|http://schemas.neumont.edu/ORM/2006-04/ORMCore|Role", match);
				match.InitializeRoles(ReadingOrderHasReading.ReadingCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|Readings||", match);
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ReadingHasTooManyRolesError.TooManyRolesErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ReadingHasTooFewRolesError.TooFewRolesErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Data", match);
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
				if (baseInfoCount != 0)
				{
					baseInfo.CopyTo(ret, 1);
				}
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "ValueRestriction", null, ORMCustomSerializedElementWriteStyle.Element, null, RoleHasValueConstraint.ValueConstraintMetaRoleGuid);
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ObjectTypePlaysRole.RolePlayerMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "RolePlayer", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == RoleProxyHasRole.ProxyMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConstraintRoleSequenceHasRole.ConstraintRoleSequenceCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ReadingOrderHasRole.ReadingOrderMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == RoleHasRolePlayerRequiredError.RolePlayerRequiredErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				metaRole = metaDataDir.FindMetaRole(RoleProxyHasRole.ProxyMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 2;
				metaRole = metaDataDir.FindMetaRole(ConstraintRoleSequenceHasRole.ConstraintRoleSequenceCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 3;
				metaRole = metaDataDir.FindMetaRole(ReadingOrderHasRole.ReadingOrderMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 4;
				metaRole = metaDataDir.FindMetaRole(RoleHasRolePlayerRequiredError.RolePlayerRequiredErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 5;
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
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|RolePlayer", match);
				match.InitializeRoles(RoleHasValueConstraint.ValueConstraintMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|ValueRestriction||", match);
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
	#endregion // Role serialization
	#region RoleProxy serialization
	public partial class RoleProxy : IORMCustomSerializedElement
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == RoleProxyHasRole.TargetRoleMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Role", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == ReadingOrderHasRole.ReadingOrderMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = RoleProxy.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(RoleProxyHasRole.TargetRoleMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Role", match);
				RoleProxy.myChildElementMappings = childElementMappings;
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
	#endregion // RoleProxy serialization
	#region SetComparisonConstraint serialization
	public partial class SetComparisonConstraint : IORMCustomSerializedElement
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
			ORMCustomSerializedChildElementInfo[] ret = SetComparisonConstraint.myCustomSerializedChildElementInfo;
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
				if (baseInfoCount != 0)
				{
					baseInfo.CopyTo(ret, 1);
				}
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "RoleSequences", null, ORMCustomSerializedElementWriteStyle.Element, null, SetComparisonConstraintHasRoleSequence.RoleSequenceCollectionMetaRoleGuid);
				SetComparisonConstraint.myCustomSerializedChildElementInfo = ret;
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
			if (attributeInfo.Id == SetComparisonConstraint.ModalityMetaAttributeGuid)
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == SetComparisonConstraintHasCompatibleRolePlayerTypeError.CompatibleRolePlayerTypeErrorCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == SetComparisonConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == SetComparisonConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == SetComparisonConstraintHasExternalConstraintRoleSequenceArityMismatchError.ArityMismatchErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
			private IComparer<MetaRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<MetaRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				MetaDataDirectory metaDataDir = store.MetaDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				MetaRoleInfo metaRole;
				metaRole = metaDataDir.FindMetaRole(SetComparisonConstraintHasRoleSequence.RoleSequenceCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 0;
				metaRole = metaDataDir.FindMetaRole(SetComparisonConstraintHasCompatibleRolePlayerTypeError.CompatibleRolePlayerTypeErrorCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 1;
				metaRole = metaDataDir.FindMetaRole(SetComparisonConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 2;
				metaRole = metaDataDir.FindMetaRole(SetComparisonConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 3;
				metaRole = metaDataDir.FindMetaRole(SetComparisonConstraintHasExternalConstraintRoleSequenceArityMismatchError.ArityMismatchErrorMetaRoleGuid);
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
		protected new IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<MetaRoleInfo> retVal = SetComparisonConstraint.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<MetaRoleInfo> baseComparer = null;
					if (0 != (ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					SetComparisonConstraint.myCustomSortChildComparer = retVal;
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
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = SetComparisonConstraint.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(SetComparisonConstraintHasRoleSequence.RoleSequenceCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|RoleSequences||", match);
				SetComparisonConstraint.myChildElementMappings = childElementMappings;
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
			Dictionary<string, Guid> customSerializedAttributes = SetComparisonConstraint.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("Modality", SetComparisonConstraint.ModalityMetaAttributeGuid);
				SetComparisonConstraint.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // SetComparisonConstraint serialization
	#region SetComparisonConstraintRoleSequence serialization
	public partial class SetComparisonConstraintRoleSequence : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.ElementInfo | (ORMCustomSerializedElementSupportedOperations.AttributeInfo | (ORMCustomSerializedElementSupportedOperations.LinkInfo | ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles)));
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
		/// Implements IORMCustomSerializedElement.GetCustomSerializedAttributeInfo
		/// </summary>
		protected new ORMCustomSerializedAttributeInfo GetCustomSerializedAttributeInfo(MetaAttributeInfo attributeInfo, MetaRoleInfo rolePlayedInfo)
		{
			if (attributeInfo.Id == SetComparisonConstraintRoleSequence.NameMetaAttributeGuid)
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ConstraintRoleSequenceHasRole.RoleCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Role", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
		protected new IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<MetaRoleInfo> retVal = SetComparisonConstraintRoleSequence.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<MetaRoleInfo> baseComparer = null;
					if (0 != (ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					SetComparisonConstraintRoleSequence.myCustomSortChildComparer = retVal;
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
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = SetComparisonConstraintRoleSequence.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ConstraintRoleSequenceHasRole.RoleCollectionMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Role", match);
				SetComparisonConstraintRoleSequence.myChildElementMappings = childElementMappings;
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
	#endregion // SetComparisonConstraintRoleSequence serialization
	#region SetConstraint serialization
	public partial class SetConstraint : IORMCustomSerializedElement
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
			ORMCustomSerializedChildElementInfo[] ret = SetConstraint.myCustomSerializedChildElementInfo;
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
				if (baseInfoCount != 0)
				{
					baseInfo.CopyTo(ret, 1);
				}
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "RoleSequence", null, ORMCustomSerializedElementWriteStyle.Element, null, ConstraintRoleSequenceHasRole.RoleCollectionMetaRoleGuid);
				SetConstraint.myCustomSerializedChildElementInfo = ret;
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
			if (attributeInfo.Id == SetConstraint.ModalityMetaAttributeGuid)
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ConstraintRoleSequenceHasRole.RoleCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Role", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == SetConstraintHasCompatibleRolePlayerTypeError.CompatibleRolePlayerTypeErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == SetConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == SetConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FactSetConstraint.FactTypeCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
			private IComparer<MetaRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<MetaRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				MetaDataDirectory metaDataDir = store.MetaDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				MetaRoleInfo metaRole;
				metaRole = metaDataDir.FindMetaRole(ConstraintRoleSequenceHasRole.RoleCollectionMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 0;
				metaRole = metaDataDir.FindMetaRole(SetConstraintHasCompatibleRolePlayerTypeError.CompatibleRolePlayerTypeErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 1;
				metaRole = metaDataDir.FindMetaRole(SetConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 2;
				metaRole = metaDataDir.FindMetaRole(SetConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesErrorMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 3;
				metaRole = metaDataDir.FindMetaRole(FactSetConstraint.FactTypeCollectionMetaRoleGuid);
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
		protected new IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<MetaRoleInfo> retVal = SetConstraint.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<MetaRoleInfo> baseComparer = null;
					if (0 != (ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					SetConstraint.myCustomSortChildComparer = retVal;
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
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = SetConstraint.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ConstraintRoleSequenceHasRole.RoleCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|RoleSequence|http://schemas.neumont.edu/ORM/2006-04/ORMCore|Role", match);
				SetConstraint.myChildElementMappings = childElementMappings;
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
			Dictionary<string, Guid> customSerializedAttributes = SetConstraint.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("Modality", SetConstraint.ModalityMetaAttributeGuid);
				SetConstraint.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // SetConstraint serialization
	#region MandatoryConstraint serialization
	public partial class MandatoryConstraint : IORMCustomSerializedElement
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
			if (attributeInfo.Id == MandatoryConstraint.IsSimpleMetaAttributeGuid)
			{
				if (!(this.IsSimple))
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == MandatoryConstraintHasMandatoryImpliedByMandatoryError.ImpliedByMandatoryErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapAttribute
		/// </summary>
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = MandatoryConstraint.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("IsSimple", MandatoryConstraint.IsSimpleMetaAttributeGuid);
				MandatoryConstraint.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // MandatoryConstraint serialization
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == FrequencyConstraintHasFrequencyConstraintMinMaxError.FrequencyConstraintMinMaxErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FrequencyConstraintHasFrequencyConstraintInvalidatedByInternalUniquenessConstraintError.FrequencyConstraintContradictsInternalUniquenessConstraintErrorCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
	}
	#endregion // FrequencyConstraint serialization
	#region UniquenessConstraint serialization
	public partial class UniquenessConstraint : IORMCustomSerializedElement
	{
		/// <summary>
		/// Implements IORMCustomSerializedElement.SupportedCustomSerializedOperations
		/// </summary>
		protected new ORMCustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | (ORMCustomSerializedElementSupportedOperations.AttributeInfo | (ORMCustomSerializedElementSupportedOperations.LinkInfo | ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles));
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
			if (attributeInfo.Id == UniquenessConstraint.IsInternalMetaAttributeGuid)
			{
				if (!(this.IsInternal))
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == EntityTypeHasPreferredIdentifier.PreferredIdentifierForMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "PreferredIdentifierFor", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == UniquenessConstraintHasNMinusOneError.NMinusOneErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
			private IComparer<MetaRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<MetaRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				MetaDataDirectory metaDataDir = store.MetaDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				MetaRoleInfo metaRole;
				metaRole = metaDataDir.FindMetaRole(EntityTypeHasPreferredIdentifier.PreferredIdentifierForMetaRoleGuid);
				roleOrderDictionary[metaRole.OppositeMetaRole.FullName] = 0;
				metaRole = metaDataDir.FindMetaRole(UniquenessConstraintHasNMinusOneError.NMinusOneErrorMetaRoleGuid);
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
		protected new IComparer<MetaRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<MetaRoleInfo> retVal = UniquenessConstraint.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<MetaRoleInfo> baseComparer = null;
					if (0 != (ORMCustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					UniquenessConstraint.myCustomSortChildComparer = retVal;
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
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = UniquenessConstraint.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(EntityTypeHasPreferredIdentifier.PreferredIdentifierForMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|PreferredIdentifierFor", match);
				UniquenessConstraint.myChildElementMappings = childElementMappings;
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
			Dictionary<string, Guid> customSerializedAttributes = UniquenessConstraint.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("IsInternal", UniquenessConstraint.IsInternalMetaAttributeGuid);
				UniquenessConstraint.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // UniquenessConstraint serialization
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == EqualityConstraintHasEqualityImpliedByMandatoryError.EqualityImpliedByMandatoryErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
			private IComparer<MetaRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<MetaRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				MetaDataDirectory metaDataDir = store.MetaDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				MetaRoleInfo metaRole;
				metaRole = metaDataDir.FindMetaRole(EqualityConstraintHasEqualityImpliedByMandatoryError.EqualityImpliedByMandatoryErrorMetaRoleGuid);
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == RingConstraintHasRingConstraintTypeNotSpecifiedError.RingConstraintTypeNotSpecifiedErrorMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, null, null, ORMCustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				if (baseInfoCount != 0)
				{
					baseInfo.CopyTo(ret, 1);
				}
				ret[0] = new ORMCustomSerializedChildElementInfo(null, "Constraints", null, ORMCustomSerializedElementWriteStyle.Element, null, SetComparisonConstraintHasDuplicateNameError.SetComparisonConstraintCollectionMetaRoleGuid, SetConstraintHasDuplicateNameError.SetConstraintCollectionMetaRoleGuid, ValueConstraintHasDuplicateNameError.ValueConstraintCollectionMetaRoleGuid);
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == SetComparisonConstraintHasDuplicateNameError.SetComparisonConstraintCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "SetComparisonConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == SetConstraintHasDuplicateNameError.SetConstraintCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "SetConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == ValueConstraintHasDuplicateNameError.ValueConstraintCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ValueConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				match.InitializeRoles(SetComparisonConstraintHasDuplicateNameError.SetComparisonConstraintCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|Constraints|http://schemas.neumont.edu/ORM/2006-04/ORMCore|SetComparisonConstraint", match);
				match.InitializeRoles(SetConstraintHasDuplicateNameError.SetConstraintCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|Constraints|http://schemas.neumont.edu/ORM/2006-04/ORMCore|SetConstraint", match);
				match.InitializeRoles(ValueConstraintHasDuplicateNameError.ValueConstraintCollectionMetaRoleGuid);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|Constraints|http://schemas.neumont.edu/ORM/2006-04/ORMCore|ValueConstraint", match);
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
				if (baseInfoCount != 0)
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ObjectTypeHasDuplicateNameError.ObjectTypeCollectionMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Object", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|Objects|http://schemas.neumont.edu/ORM/2006-04/ORMCore|Object", match);
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ObjectTypeHasEntityTypeRequiresReferenceSchemeError.ObjectTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "EntityType", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|EntityType", match);
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == SetComparisonConstraintHasExternalConstraintRoleSequenceArityMismatchError.ConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Constraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				match.InitializeRoles(SetComparisonConstraintHasExternalConstraintRoleSequenceArityMismatchError.ConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Constraint", match);
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == FactTypeHasImpliedInternalUniquenessConstraintError.FactTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Fact", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Fact", match);
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == FactTypeHasFactTypeRequiresInternalUniquenessConstraintError.FactTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Fact", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Fact", match);
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == FactTypeHasFactTypeRequiresReadingError.FactTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Fact", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Fact", match);
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == FrequencyConstraintHasFrequencyConstraintMinMaxError.FrequencyConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "FrequencyConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|FrequencyConstraint", match);
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ValueRangeHasMinValueMismatchError.ValueRangeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ValueRange", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ValueRange", match);
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ValueRangeHasMaxValueMismatchError.ValueRangeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ValueRange", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ValueRange", match);
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
	#region ValueRangeOverlapError serialization
	public partial class ValueRangeOverlapError : IORMCustomSerializedElement
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ValueConstraintHasValueRangeOverlapError.ValueConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ValueConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ValueRangeOverlapError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ValueConstraintHasValueRangeOverlapError.ValueConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ValueConstraint", match);
				ValueRangeOverlapError.myChildElementMappings = childElementMappings;
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
	#endregion // ValueRangeOverlapError serialization
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == RingConstraintHasRingConstraintTypeNotSpecifiedError.RingConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "RingConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|RingConstraint", match);
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ReadingHasTooFewRolesError.ReadingMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Reading", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Reading", match);
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == SetComparisonConstraintHasTooFewRoleSequencesError.SetComparisonConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "SetComparisonConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == SetConstraintHasTooFewRoleSequencesError.SetConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "SetConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				match.InitializeRoles(SetComparisonConstraintHasTooFewRoleSequencesError.SetComparisonConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|SetComparisonConstraint", match);
				match.InitializeRoles(SetConstraintHasTooFewRoleSequencesError.SetConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|SetConstraint", match);
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ReadingHasTooManyRolesError.ReadingMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Reading", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Reading", match);
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == SetComparisonConstraintHasTooManyRoleSequencesError.SetComparisonConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "SetComparisonConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == SetConstraintHasTooManyRoleSequencesError.SetConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "SetConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				match.InitializeRoles(SetComparisonConstraintHasTooManyRoleSequencesError.SetComparisonConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|SetComparisonConstraint", match);
				match.InitializeRoles(SetConstraintHasTooManyRoleSequencesError.SetConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|SetConstraint", match);
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ValueTypeHasUnspecifiedDataTypeError.ValueTypeHasDataTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ConceptualDataType", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ConceptualDataType", match);
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
	#region EqualityImpliedByMandatoryError serialization
	public partial class EqualityImpliedByMandatoryError : IORMCustomSerializedElement
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == EqualityConstraintHasEqualityImpliedByMandatoryError.EqualityConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "EqualityConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = EqualityImpliedByMandatoryError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(EqualityConstraintHasEqualityImpliedByMandatoryError.EqualityConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|EqualityConstraint", match);
				EqualityImpliedByMandatoryError.myChildElementMappings = childElementMappings;
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
	#endregion // EqualityImpliedByMandatoryError serialization
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == UniquenessConstraintHasNMinusOneError.ConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "UniquenessConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				match.InitializeRoles(UniquenessConstraintHasNMinusOneError.ConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|UniquenessConstraint", match);
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
	#region MandatoryImpliedByMandatoryError serialization
	public partial class MandatoryImpliedByMandatoryError : IORMCustomSerializedElement
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == MandatoryConstraintHasMandatoryImpliedByMandatoryError.MandatoryConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "MandatoryConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = MandatoryImpliedByMandatoryError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(MandatoryConstraintHasMandatoryImpliedByMandatoryError.MandatoryConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|MandatoryConstraint", match);
				MandatoryImpliedByMandatoryError.myChildElementMappings = childElementMappings;
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
	#endregion // MandatoryImpliedByMandatoryError serialization
	#region ObjectTypeRequiresPrimarySupertypeError serialization
	public partial class ObjectTypeRequiresPrimarySupertypeError : IORMCustomSerializedElement
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ObjectTypeHasObjectTypeRequiresPrimarySupertypeError.ObjectTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ObjectType", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = ObjectTypeRequiresPrimarySupertypeError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ObjectTypeHasObjectTypeRequiresPrimarySupertypeError.ObjectTypeMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ObjectType", match);
				ObjectTypeRequiresPrimarySupertypeError.myChildElementMappings = childElementMappings;
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
	#endregion // ObjectTypeRequiresPrimarySupertypeError serialization
	#region PreferredIdentifierRequiresMandatoryError serialization
	public partial class PreferredIdentifierRequiresMandatoryError : IORMCustomSerializedElement
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ObjectTypeHasPreferredIdentifierRequiresMandatoryError.ObjectTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ObjectType", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = PreferredIdentifierRequiresMandatoryError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ObjectTypeHasPreferredIdentifierRequiresMandatoryError.ObjectTypeMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ObjectType", match);
				PreferredIdentifierRequiresMandatoryError.myChildElementMappings = childElementMappings;
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
	#endregion // PreferredIdentifierRequiresMandatoryError serialization
	#region CompatibleSupertypesError serialization
	public partial class CompatibleSupertypesError : IORMCustomSerializedElement
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ObjectTypeHasCompatibleSupertypesError.ObjectTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "ObjectType", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, ORMCustomSerializedElementMatch> myChildElementMappings;
		/// <summary>
		/// Implements IORMCustomSerializedElement.MapChildElement
		/// </summary>
		protected new ORMCustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName)
		{
			Dictionary<string, ORMCustomSerializedElementMatch> childElementMappings = CompatibleSupertypesError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, ORMCustomSerializedElementMatch>();
				ORMCustomSerializedElementMatch match = new ORMCustomSerializedElementMatch();
				match.InitializeRoles(ObjectTypeHasCompatibleSupertypesError.ObjectTypeMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ObjectType", match);
				CompatibleSupertypesError.myChildElementMappings = childElementMappings;
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
	#endregion // CompatibleSupertypesError serialization
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
				if (this.SetConstraint != null)
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == SetComparisonConstraintHasCompatibleRolePlayerTypeError.SetComparisonConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "SetComparisonConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == SetConstraintHasCompatibleRolePlayerTypeError.SetConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "SetConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				match.InitializeRoles(SetComparisonConstraintHasCompatibleRolePlayerTypeError.SetComparisonConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|SetComparisonConstraint", match);
				match.InitializeRoles(SetConstraintHasCompatibleRolePlayerTypeError.SetConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|SetConstraint", match);
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == RoleHasRolePlayerRequiredError.RoleMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Role", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Role", match);
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
		protected new ORMCustomSerializedElementInfo GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == FactTypeHasFrequencyConstraintContradictsInternalUniquenessConstraintError.FactTypeMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "Fact", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == FrequencyConstraintHasFrequencyConstraintInvalidatedByInternalUniquenessConstraintError.FrequencyConstraintMetaRoleGuid)
			{
				return new ORMCustomSerializedElementInfo(null, "FrequencyConstraint", null, ORMCustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (ORMCustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return ORMCustomSerializedElementInfo.Default;
		}
		ORMCustomSerializedElementInfo IORMCustomSerializedElement.GetCustomSerializedLinkInfo(MetaRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Fact", match);
				match.InitializeRoles(FrequencyConstraintHasFrequencyConstraintInvalidatedByInternalUniquenessConstraintError.FrequencyConstraintMetaRoleGuid);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|FrequencyConstraint", match);
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
