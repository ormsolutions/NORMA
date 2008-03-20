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

namespace Neumont.Tools.ORM.ObjectModel
{
	#region ORMCoreDomainModel model serialization
	[CustomSerializedXmlNamespaces("http://schemas.neumont.edu/ORM/2006-04/ORMCore")]
	partial class ORMCoreDomainModel : ICustomSerializedDomainModel
	{
		/// <summary>The default XmlNamespace associated with the 'ORMCoreDomainModel' extension model</summary>
		public static readonly string XmlNamespace = "http://schemas.neumont.edu/ORM/2006-04/ORMCore";
		/// <summary>Implements ICustomSerializedDomainModel.DefaultElementPrefix</summary>
		protected static string DefaultElementPrefix
		{
			get
			{
				return "orm";
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
			ret[0, 0] = "orm";
			ret[0, 1] = "http://schemas.neumont.edu/ORM/2006-04/ORMCore";
			ret[0, 2] = "ORM2Core.xsd";
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
			retVal[dataDir.FindDomainRelationship(FactConstraint.DomainClassId)] = null;
			retVal[dataDir.FindDomainRelationship(FactSetComparisonConstraint.DomainClassId)] = null;
			retVal[dataDir.FindDomainRelationship(ExternalRoleConstraint.DomainClassId)] = null;
			retVal[dataDir.FindDomainRelationship(RoleInstance.DomainClassId)] = null;
			retVal[dataDir.FindDomainRelationship(ModelNoteReferencesModelElement.DomainClassId)] = null;
			retVal[dataDir.FindDomainRelationship(JoinHasRole.DomainClassId)] = null;
			retVal[dataDir.FindDomainRelationship(ORMModelElementHasJoinPath.DomainClassId)] = null;
			retVal[dataDir.FindDomainRelationship(ModelHasModelErrorDisplayFilter.DomainClassId)] = null;
			retVal[dataDir.FindDomainRelationship(ElementAssociatedWithModelError.DomainClassId)] = null;
			retVal[dataDir.FindDomainRelationship(ElementHasAlias.DomainClassId)] = null;
			retVal[dataDir.FindDomainRelationship(ObjectTypeHasObjectTypeInstance.DomainClassId)] = null;
			retVal[dataDir.FindDomainClass(IntrinsicReferenceMode.DomainClassId)] = null;
			retVal[dataDir.FindDomainRelationship(Microsoft.VisualStudio.Modeling.Diagrams.PresentationViewsSubject.DomainClassId)] = null;
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
				omissions = ORMCoreDomainModel.BuildCustomSerializationOmissions(store);
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
			return new Guid[]{
				ORMModel.DomainClassId,
				ModelErrorDisplayFilter.DomainClassId,
				NameGenerator.DomainClassId,
				GenerationState.DomainClassId};
		}
		Guid[] ICustomSerializedDomainModel.GetRootElementClasses()
		{
			return GetRootElementClasses();
		}
		/// <summary>Implements ICustomSerializedDomainModel.ShouldSerializeRootElement</summary>
		protected static bool ShouldSerializeRootElement(ModelElement element)
		{
			DomainClassInfo elementDomainClass = element.GetDomainClass();
			if (elementDomainClass.IsDerivedFrom(NameGenerator.DomainClassId))
			{
				// Only serialize if this is not contained in a refinement relationship
				return ((NameGenerator)element).RefinesGenerator == null;
			}
			else if (elementDomainClass.IsDerivedFrom(GenerationState.DomainClassId))
			{
				// Only serialize if there are generated elements in this model.
				return ((GenerationState)element).GenerationSettingCollection.Count != 0;
			}
			return true;
		}
		bool ICustomSerializedDomainModel.ShouldSerializeRootElement(ModelElement element)
		{
			return ShouldSerializeRootElement(element);
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
			if (elementName == "ORMModel" && xmlNamespace == "http://schemas.neumont.edu/ORM/2006-04/ORMCore")
			{
				return ORMModel.DomainClassId;
			}
			if (elementName == "ModelErrorDisplayFilter" && xmlNamespace == "http://schemas.neumont.edu/ORM/2006-04/ORMCore")
			{
				return ModelErrorDisplayFilter.DomainClassId;
			}
			if (elementName == "NameGenerator" && xmlNamespace == "http://schemas.neumont.edu/ORM/2006-04/ORMCore")
			{
				return NameGenerator.DomainClassId;
			}
			if (elementName == "GenerationState" && xmlNamespace == "http://schemas.neumont.edu/ORM/2006-04/ORMCore")
			{
				return GenerationState.DomainClassId;
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
			Collection<string> validNamespaces = ORMCoreDomainModel.myValidNamespaces;
			Dictionary<string, Guid> classNameMap = ORMCoreDomainModel.myClassNameMap;
			if (validNamespaces == null)
			{
				validNamespaces = new Collection<string>();
				validNamespaces.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore");
				ORMCoreDomainModel.myValidNamespaces = validNamespaces;
			}
			if (classNameMap == null)
			{
				classNameMap = new Dictionary<string, Guid>();
				classNameMap.Add("ORMModelElement", ORMModelElement.DomainClassId);
				classNameMap.Add("ORMNamedElement", ORMNamedElement.DomainClassId);
				classNameMap.Add("ModelError", ModelError.DomainClassId);
				classNameMap.Add("ORMModel", ORMModel.DomainClassId);
				classNameMap.Add("Definition", Definition.DomainClassId);
				classNameMap.Add("Note", Note.DomainClassId);
				classNameMap.Add("ModelNote", ModelNote.DomainClassId);
				classNameMap.Add("GenerationState", GenerationState.DomainClassId);
				classNameMap.Add("Alias", NameAlias.DomainClassId);
				classNameMap.Add("NameGenerator", NameGenerator.DomainClassId);
				classNameMap.Add("EntityType", ObjectType.DomainClassId);
				classNameMap.Add("ValueType", ObjectType.DomainClassId);
				classNameMap.Add("ObjectifiedType", ObjectType.DomainClassId);
				classNameMap.Add("RecognizedPhrase", RecognizedPhrase.DomainClassId);
				classNameMap.Add("ValueTypeInstance", ValueTypeInstance.DomainClassId);
				classNameMap.Add("EntityTypeInstance", EntityTypeInstance.DomainClassId);
				classNameMap.Add("EntityTypeRoleInstance", EntityTypeRoleInstance.DomainClassId);
				classNameMap.Add("CustomReferenceMode", CustomReferenceMode.DomainClassId);
				classNameMap.Add("DataType", DataType.DomainClassId);
				classNameMap.Add("ReferenceModeKind", ReferenceModeKind.DomainClassId);
				classNameMap.Add("BaseValueConstraint", ValueConstraint.DomainClassId);
				classNameMap.Add("ValueConstraint", ValueTypeValueConstraint.DomainClassId);
				classNameMap.Add("RoleValueConstraint", RoleValueConstraint.DomainClassId);
				classNameMap.Add("ValueRange", ValueRange.DomainClassId);
				classNameMap.Add("Fact", FactType.DomainClassId);
				classNameMap.Add("ImpliedFact", FactType.DomainClassId);
				classNameMap.Add("FactTypeInstance", FactTypeInstance.DomainClassId);
				classNameMap.Add("FactTypeRoleInstance", FactTypeRoleInstance.DomainClassId);
				classNameMap.Add("Expression", Expression.DomainClassId);
				classNameMap.Add("DerivationExpression", FactTypeDerivationExpression.DomainClassId);
				classNameMap.Add("SubtypeFact", SubtypeFact.DomainClassId);
				classNameMap.Add("ReadingOrder", ReadingOrder.DomainClassId);
				classNameMap.Add("Reading", Reading.DomainClassId);
				classNameMap.Add("Role", Role.DomainClassId);
				classNameMap.Add("RoleProxy", RoleProxy.DomainClassId);
				classNameMap.Add("SetComparisonConstraint", SetComparisonConstraint.DomainClassId);
				classNameMap.Add("RoleSequence", SetComparisonConstraintRoleSequence.DomainClassId);
				classNameMap.Add("SetConstraint", SetConstraint.DomainClassId);
				classNameMap.Add("MandatoryConstraint", MandatoryConstraint.DomainClassId);
				classNameMap.Add("FrequencyConstraint", FrequencyConstraint.DomainClassId);
				classNameMap.Add("UniquenessConstraint", UniquenessConstraint.DomainClassId);
				classNameMap.Add("EqualityConstraint", EqualityConstraint.DomainClassId);
				classNameMap.Add("ExclusionConstraint", ExclusionConstraint.DomainClassId);
				classNameMap.Add("SubsetConstraint", SubsetConstraint.DomainClassId);
				classNameMap.Add("RingConstraint", RingConstraint.DomainClassId);
				classNameMap.Add("Join", Join.DomainClassId);
				classNameMap.Add("ConstraintDuplicateNameError", ConstraintDuplicateNameError.DomainClassId);
				classNameMap.Add("ObjectTypeDuplicateNameError", ObjectTypeDuplicateNameError.DomainClassId);
				classNameMap.Add("RecognizedPhraseDuplicateNameError", RecognizedPhraseDuplicateNameError.DomainClassId);
				classNameMap.Add("EntityTypeRequiresReferenceSchemeError", EntityTypeRequiresReferenceSchemeError.DomainClassId);
				classNameMap.Add("ExternalConstraintRoleSequenceArityMismatchError", ExternalConstraintRoleSequenceArityMismatchError.DomainClassId);
				classNameMap.Add("ImpliedInternalUniquenessConstraintError", ImpliedInternalUniquenessConstraintError.DomainClassId);
				classNameMap.Add("FactTypeRequiresInternalUniquenessConstraintError", FactTypeRequiresInternalUniquenessConstraintError.DomainClassId);
				classNameMap.Add("FactTypeRequiresReadingError", FactTypeRequiresReadingError.DomainClassId);
				classNameMap.Add("FrequencyConstraintExactlyOneError", FrequencyConstraintExactlyOneError.DomainClassId);
				classNameMap.Add("FrequencyConstraintMinMaxError", FrequencyConstraintMinMaxError.DomainClassId);
				classNameMap.Add("MinValueMismatchError", MinValueMismatchError.DomainClassId);
				classNameMap.Add("MaxValueMismatchError", MaxValueMismatchError.DomainClassId);
				classNameMap.Add("ValueRangeOverlapError", ValueRangeOverlapError.DomainClassId);
				classNameMap.Add("ValueTypeDetachedError", ValueConstraintValueTypeDetachedError.DomainClassId);
				classNameMap.Add("RingConstraintTypeNotSpecifiedError", RingConstraintTypeNotSpecifiedError.DomainClassId);
				classNameMap.Add("CompatibleValueTypeInstanceValueError", CompatibleValueTypeInstanceValueError.DomainClassId);
				classNameMap.Add("TooFewEntityTypeRoleInstancesError", TooFewEntityTypeRoleInstancesError.DomainClassId);
				classNameMap.Add("TooFewFactTypeRoleInstancesError", TooFewFactTypeRoleInstancesError.DomainClassId);
				classNameMap.Add("PopulationMandatoryError", PopulationMandatoryError.DomainClassId);
				classNameMap.Add("PopulationUniquenessError", PopulationUniquenessError.DomainClassId);
				classNameMap.Add("TooFewReadingRolesError", TooFewReadingRolesError.DomainClassId);
				classNameMap.Add("ReadingRequiresUserModificationError", ReadingRequiresUserModificationError.DomainClassId);
				classNameMap.Add("TooFewRoleSequencesError", TooFewRoleSequencesError.DomainClassId);
				classNameMap.Add("TooManyReadingRolesError", TooManyReadingRolesError.DomainClassId);
				classNameMap.Add("TooManyRoleSequencesError", TooManyRoleSequencesError.DomainClassId);
				classNameMap.Add("DataTypeNotSpecifiedError", DataTypeNotSpecifiedError.DomainClassId);
				classNameMap.Add("EqualityImpliedByMandatoryError", EqualityImpliedByMandatoryError.DomainClassId);
				classNameMap.Add("NMinusOneError", NMinusOneError.DomainClassId);
				classNameMap.Add("ImplicationError", ImplicationError.DomainClassId);
				classNameMap.Add("ContradictionError", ContradictionError.DomainClassId);
				classNameMap.Add("ExclusionContradictsEqualityError", ExclusionContradictsEqualityError.DomainClassId);
				classNameMap.Add("ExclusionContradictsSubsetError", ExclusionContradictsSubsetError.DomainClassId);
				classNameMap.Add("ExclusionContradictsMandatoryError", ExclusionContradictsMandatoryError.DomainClassId);
				classNameMap.Add("NotWellModeledSubsetAndMandatoryError", NotWellModeledSubsetAndMandatoryError.DomainClassId);
				classNameMap.Add("PreferredIdentifierRequiresMandatoryError", PreferredIdentifierRequiresMandatoryError.DomainClassId);
				classNameMap.Add("CompatibleSupertypesError", CompatibleSupertypesError.DomainClassId);
				classNameMap.Add("CompatibleRolePlayerTypeError", CompatibleRolePlayerTypeError.DomainClassId);
				classNameMap.Add("RolePlayerRequiredError", RolePlayerRequiredError.DomainClassId);
				classNameMap.Add("FrequencyConstraintContradictsInternalUniquenessConstraintError", FrequencyConstraintContradictsInternalUniquenessConstraintError.DomainClassId);
				classNameMap.Add("EqualityConstraintImpliedByMandatoryConstraintsError", EqualityOrSubsetImpliedByMandatoryError.DomainClassId);
				classNameMap.Add("SubsetConstraintImpliedByMandatoryConstraintsError", EqualityOrSubsetImpliedByMandatoryError.DomainClassId);
				ORMCoreDomainModel.myClassNameMap = classNameMap;
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
	#endregion // ORMCoreDomainModel model serialization
	#region ORMModelElement serialization
	partial class ORMModelElement : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.ChildElementInfo;
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
			CustomSerializedContainerElementInfo[] ret = ORMModelElement.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new CustomSerializedContainerElementInfo[1];
				ret[0] = new CustomSerializedContainerElementInfo(null, "Extensions", null, CustomSerializedElementWriteStyle.Element, null, ORMModelElementHasExtensionElement.ExtensionDomainRoleId);
				ORMModelElement.myCustomSerializedChildElementInfo = ret;
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ORMModelElement.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ORMModelElementHasExtensionElement.ExtensionDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Extensions||", match);
				ORMModelElement.myChildElementMappings = childElementMappings;
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
	#endregion // ORMModelElement serialization
	#region ORMNamedElement serialization
	partial class ORMNamedElement : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.None;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
	}
	#endregion // ORMNamedElement serialization
	#region ModelError serialization
	partial class ModelError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.PropertyInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		protected new CustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == ModelError.ErrorTextDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, "Name", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = ModelError.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("Name", ModelError.ErrorTextDomainPropertyId);
				ModelError.myCustomSerializedAttributes = customSerializedAttributes;
			}
			Guid rVal;
			string key = attributeName;
			if (xmlNamespace.Length != 0)
			{
				key = string.Concat(xmlNamespace, "|", attributeName);
			}
			if (!customSerializedAttributes.TryGetValue(key, out rVal))
			{
				rVal = base.MapAttribute(xmlNamespace, attributeName);
			}
			return rVal;
		}
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
	}
	#endregion // ModelError serialization
	#region ORMModel serialization
	partial class ORMModel : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
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
			CustomSerializedContainerElementInfo[] ret = ORMModel.myCustomSerializedChildElementInfo;
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
				ret = new CustomSerializedContainerElementInfo[baseInfoCount + 9];
				if (baseInfoCount != 0)
				{
					baseInfo.CopyTo(ret, 9);
				}
				ret[0] = new CustomSerializedContainerElementInfo(null, "Objects", null, CustomSerializedElementWriteStyle.Element, null, ModelHasObjectType.ObjectTypeDomainRoleId);
				ret[1] = new CustomSerializedContainerElementInfo(null, "Facts", null, CustomSerializedElementWriteStyle.Element, null, ModelHasFactType.FactTypeDomainRoleId);
				ret[2] = new CustomSerializedContainerElementInfo(null, "Constraints", null, CustomSerializedElementWriteStyle.Element, null, ModelHasSetComparisonConstraint.SetComparisonConstraintDomainRoleId, ModelHasSetConstraint.SetConstraintDomainRoleId);
				ret[3] = new CustomSerializedContainerElementInfo(null, "DataTypes", null, CustomSerializedElementWriteStyle.Element, null, ModelHasDataType.DataTypeDomainRoleId);
				ret[4] = new CustomSerializedContainerElementInfo(null, "CustomReferenceModes", null, CustomSerializedElementWriteStyle.Element, null, ModelHasReferenceMode.ReferenceModeDomainRoleId);
				ret[5] = new CustomSerializedContainerElementInfo(null, "ModelNotes", null, CustomSerializedElementWriteStyle.Element, null, ModelHasModelNote.NoteDomainRoleId);
				ret[6] = new CustomSerializedContainerElementInfo(null, "ModelErrors", null, CustomSerializedElementWriteStyle.Element, null, ModelHasError.ErrorDomainRoleId);
				ret[7] = new CustomSerializedContainerElementInfo(null, "ReferenceModeKinds", null, CustomSerializedElementWriteStyle.Element, null, ModelHasReferenceModeKind.ReferenceModeKindDomainRoleId);
				ret[8] = new CustomSerializedContainerElementInfo(null, "RecognizedPhrases", null, CustomSerializedElementWriteStyle.Element, null, ModelContainsRecognizedPhrase.RecognizedPhraseDomainRoleId);
				ORMModel.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		protected new CustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new CustomSerializedElementInfo(null, "ORMModel", null, CustomSerializedElementWriteStyle.Element, null);
			}
		}
		CustomSerializedElementInfo ICustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return this.CustomSerializedElementInfo;
			}
		}
		private static IComparer<DomainRoleInfo> myCustomSortChildComparer;
		private sealed class CustomSortChildComparer : IComparer<DomainRoleInfo>
		{
			private readonly Dictionary<string, int> myRoleOrderDictionary;
			private IComparer<DomainRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<DomainRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				DomainDataDirectory domainDataDirectory = store.DomainDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				DomainRoleInfo domainRole;
				domainRole = domainDataDirectory.FindDomainRole(ModelHasObjectType.ObjectTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(ModelHasFactType.FactTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(ModelHasSetComparisonConstraint.SetComparisonConstraintDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(ModelHasSetConstraint.SetConstraintDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(ModelHasDataType.DataTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 3;
				domainRole = domainDataDirectory.FindDomainRole(ModelHasReferenceMode.ReferenceModeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 4;
				domainRole = domainDataDirectory.FindDomainRole(ModelHasModelNote.NoteDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 5;
				domainRole = domainDataDirectory.FindDomainRole(ModelHasError.ErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 6;
				domainRole = domainDataDirectory.FindDomainRole(ModelHasReferenceModeKind.ReferenceModeKindDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 7;
				domainRole = domainDataDirectory.FindDomainRole(ModelContainsRecognizedPhrase.RecognizedPhraseDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 8;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<DomainRoleInfo>.Compare(DomainRoleInfo x, DomainRoleInfo y)
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
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(x.DomainRelationship.ImplementationClass.FullName, ".", x.Name), out xPos))
				{
					xPos = int.MaxValue;
				}
				int yPos;
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(y.DomainRelationship.ImplementationClass.FullName, ".", y.Name), out yPos))
				{
					yPos = int.MaxValue;
				}
				return xPos.CompareTo(yPos);
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		protected new IComparer<DomainRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<DomainRoleInfo> retVal = ORMModel.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					ORMModel.myCustomSortChildComparer = retVal;
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
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ORMModel.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ModelHasObjectType.ObjectTypeDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Objects||", match);
				match.InitializeRoles(ModelHasFactType.FactTypeDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Facts||", match);
				match.InitializeRoles(ModelHasSetComparisonConstraint.SetComparisonConstraintDomainRoleId, ModelHasSetConstraint.SetConstraintDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Constraints||", match);
				match.InitializeRoles(ModelHasDataType.DataTypeDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|DataTypes||", match);
				match.InitializeRoles(ModelHasReferenceMode.ReferenceModeDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|CustomReferenceModes||", match);
				match.InitializeRoles(ModelHasModelNote.NoteDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ModelNotes||", match);
				match.InitializeRoles(ModelHasError.ErrorDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ModelErrors||", match);
				match.InitializeRoles(ModelHasReferenceModeKind.ReferenceModeKindDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ReferenceModeKinds||", match);
				match.InitializeRoles(ModelContainsRecognizedPhrase.RecognizedPhraseDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|RecognizedPhrases||", match);
				ORMModel.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // ORMModel serialization
	#region Definition serialization
	partial class Definition : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.MixedTypedAttributes;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		protected new CustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == Definition.TextDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = Definition.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeAttribute(Definition.TextDomainPropertyId, null);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Text", match);
				Definition.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // Definition serialization
	#region Note serialization
	partial class Note : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.MixedTypedAttributes;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		protected new CustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == Note.TextDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = Note.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeAttribute(Note.TextDomainPropertyId, null);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Text", match);
				Note.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // Note serialization
	#region ModelNote serialization
	partial class ModelNote : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
			CustomSerializedContainerElementInfo[] ret = ModelNote.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "ReferencedBy", null, CustomSerializedElementWriteStyle.Element, null, ModelNoteReferencesFactType.ElementDomainRoleId, ModelNoteReferencesObjectType.ElementDomainRoleId, ModelNoteReferencesSetConstraint.ElementDomainRoleId, ModelNoteReferencesSetComparisonConstraint.ElementDomainRoleId);
				ModelNote.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ModelNoteReferencesFactType.ElementDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "FactType", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == ModelNoteReferencesObjectType.ElementDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ObjectType", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == ModelNoteReferencesSetConstraint.ElementDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "SetConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == ModelNoteReferencesSetComparisonConstraint.ElementDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "SetComparisonConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ModelNote.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ModelNoteReferencesFactType.ElementDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ReferencedBy||FactType", match);
				match.InitializeRoles(ModelNoteReferencesObjectType.ElementDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ReferencedBy||ObjectType", match);
				match.InitializeRoles(ModelNoteReferencesSetConstraint.ElementDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ReferencedBy||SetConstraint", match);
				match.InitializeRoles(ModelNoteReferencesSetComparisonConstraint.ElementDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ReferencedBy||SetComparisonConstraint", match);
				ModelNote.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // ModelNote serialization
	#region GenerationState serialization
	partial class GenerationState : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.ChildElementInfo;
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
			CustomSerializedContainerElementInfo[] ret = GenerationState.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new CustomSerializedContainerElementInfo[1];
				ret[0] = new CustomSerializedContainerElementInfo(null, "GenerationSettings", null, CustomSerializedElementWriteStyle.Element, null, GenerationStateHasGenerationSetting.GenerationSettingDomainRoleId);
				GenerationState.myCustomSerializedChildElementInfo = ret;
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = GenerationState.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(GenerationStateHasGenerationSetting.GenerationSettingDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|GenerationSettings||", match);
				GenerationState.myChildElementMappings = childElementMappings;
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
	#endregion // GenerationState serialization
	#region NameAlias serialization
	partial class NameAlias : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		protected new CustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new CustomSerializedElementInfo(null, "Alias", null, CustomSerializedElementWriteStyle.Element, null);
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
		protected new CustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == NameAlias.NameConsumerDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, null, null, true, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == NameAlias.NameUsageDomainPropertyId)
			{
				if (this.NameUsageType == null)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, true, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = NameAlias.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("NameConsumer", NameAlias.NameConsumerDomainPropertyId);
				customSerializedAttributes.Add("NameUsage", NameAlias.NameUsageDomainPropertyId);
				NameAlias.myCustomSerializedAttributes = customSerializedAttributes;
			}
			Guid rVal;
			string key = attributeName;
			if (xmlNamespace.Length != 0)
			{
				key = string.Concat(xmlNamespace, "|", attributeName);
			}
			if (!customSerializedAttributes.TryGetValue(key, out rVal))
			{
				rVal = base.MapAttribute(xmlNamespace, attributeName);
			}
			return rVal;
		}
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
	}
	#endregion // NameAlias serialization
	#region NameGenerator serialization
	partial class NameGenerator : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
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
			CustomSerializedContainerElementInfo[] ret = NameGenerator.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new CustomSerializedContainerElementInfo[1];
				ret[0] = new CustomSerializedContainerElementInfo("orm", "Refinements", null, CustomSerializedElementWriteStyle.Element, null, NameGeneratorRefinesNameGenerator.RefinementDomainRoleId);
				NameGenerator.myCustomSerializedChildElementInfo = ret;
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
			if (domainPropertyInfo.Id == NameGenerator.SpacingFormatDomainPropertyId)
			{
				if (this.SpacingFormat == NameGeneratorSpacingFormat.Retain)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == NameGenerator.CasingOptionDomainPropertyId)
			{
				if (this.CasingOption == NameGeneratorCasingOption.None)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == NameGenerator.SpacingReplacementDomainPropertyId)
			{
				if (this.SpacingFormat != NameGeneratorSpacingFormat.ReplaceWith)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == NameGenerator.NameUsageDomainPropertyId)
			{
				if (this.NameUsageType == null)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, true, CustomSerializedAttributeWriteStyle.Attribute, null);
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
		private static IComparer<DomainRoleInfo> myCustomSortChildComparer;
		private sealed class CustomSortChildComparer : IComparer<DomainRoleInfo>
		{
			private readonly Dictionary<string, int> myRoleOrderDictionary;
			public CustomSortChildComparer(Store store)
			{
				DomainDataDirectory domainDataDirectory = store.DomainDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				DomainRoleInfo domainRole;
				domainRole = domainDataDirectory.FindDomainRole(NameGeneratorRefinesNameGenerator.RefinementDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<DomainRoleInfo>.Compare(DomainRoleInfo x, DomainRoleInfo y)
			{
				int xPos;
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(x.DomainRelationship.ImplementationClass.FullName, ".", x.Name), out xPos))
				{
					xPos = int.MaxValue;
				}
				int yPos;
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(y.DomainRelationship.ImplementationClass.FullName, ".", y.Name), out yPos))
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
				IComparer<DomainRoleInfo> retVal = NameGenerator.myCustomSortChildComparer;
				if (null == retVal)
				{
					retVal = new CustomSortChildComparer(this.Store);
					NameGenerator.myCustomSortChildComparer = retVal;
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = NameGenerator.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(NameGeneratorRefinesNameGenerator.RefinementDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Refinements||", match);
				NameGenerator.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal);
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
			Dictionary<string, Guid> customSerializedAttributes = NameGenerator.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("SpacingFormat", NameGenerator.SpacingFormatDomainPropertyId);
				customSerializedAttributes.Add("CasingOption", NameGenerator.CasingOptionDomainPropertyId);
				customSerializedAttributes.Add("SpacingReplacement", NameGenerator.SpacingReplacementDomainPropertyId);
				customSerializedAttributes.Add("NameUsage", NameGenerator.NameUsageDomainPropertyId);
				NameGenerator.myCustomSerializedAttributes = customSerializedAttributes;
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
		protected bool ShouldSerialize()
		{
			return this.RequiresSerialization();
		}
		bool ICustomSerializedElement.ShouldSerialize()
		{
			return this.ShouldSerialize();
		}
	}
	#endregion // NameGenerator serialization
	#region ObjectType serialization
	partial class ObjectType : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
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
			CustomSerializedContainerElementInfo[] ret = ObjectType.myCustomSerializedChildElementInfo;
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
				ret = new CustomSerializedContainerElementInfo[baseInfoCount + 6];
				if (baseInfoCount != 0)
				{
					baseInfo.CopyTo(ret, 6);
				}
				ret[0] = new CustomSerializedContainerElementInfo(null, "Definitions", null, CustomSerializedElementWriteStyle.Element, null, ObjectTypeHasDefinition.DefinitionDomainRoleId);
				ret[1] = new CustomSerializedContainerElementInfo(null, "Notes", null, CustomSerializedElementWriteStyle.Element, null, ObjectTypeHasNote.NoteDomainRoleId);
				ret[2] = new CustomSerializedContainerElementInfo(null, "Abbreviations", null, CustomSerializedElementWriteStyle.Element, null, ObjectTypeHasAbbreviation.AbbreviationDomainRoleId);
				ret[3] = new CustomSerializedContainerElementInfo(null, "PlayedRoles", null, CustomSerializedElementWriteStyle.Element, null, ObjectTypePlaysRole.PlayedRoleDomainRoleId);
				ret[4] = new CustomSerializedContainerElementInfo(null, "ValueRestriction", null, CustomSerializedElementWriteStyle.Element, null, ValueTypeHasValueConstraint.ValueConstraintDomainRoleId);
				ret[5] = new CustomSerializedContainerElementInfo(null, "Instances", null, CustomSerializedElementWriteStyle.Element, null, ValueTypeHasValueTypeInstance.ValueTypeInstanceDomainRoleId, EntityTypeHasEntityTypeInstance.EntityTypeInstanceDomainRoleId);
				ObjectType.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		protected new CustomSerializedElementInfo CustomSerializedElementInfo
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
				return new CustomSerializedElementInfo(null, name, null, CustomSerializedElementWriteStyle.Element, null);
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
		protected new CustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == ObjectType.ReferenceModeStringDomainPropertyId)
			{
				if (this.IsValueType)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, "_ReferenceMode", null, true, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == ObjectType.IsIndependentDomainPropertyId)
			{
				if (!this.IsIndependent)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == ObjectType.IsExternalDomainPropertyId)
			{
				if (!this.IsExternal)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == ObjectType.IsPersonalDomainPropertyId)
			{
				if (!this.IsPersonal)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == ObjectType.IsImplicitBooleanValueDomainPropertyId)
			{
				if (!this.IsImplicitBooleanValue)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ValueTypeHasDataType.DataTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ConceptualDataType", null, CustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == ObjectTypePlaysRole.PlayedRoleDomainRoleId)
			{
				string name = "Role";
				if (DomainRoleInfo.GetRolePlayer(elementLink, ObjectTypePlaysRole.PlayedRoleDomainRoleId) is SubtypeMetaRole)
				{
					name = "SubtypeMetaRole";
				}
				else if (DomainRoleInfo.GetRolePlayer(elementLink, ObjectTypePlaysRole.PlayedRoleDomainRoleId) is SupertypeMetaRole)
				{
					name = "SupertypeMetaRole";
				}
				return new CustomSerializedElementInfo(null, name, null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == EntityTypeHasPreferredIdentifier.PreferredIdentifierDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "PreferredIdentifier", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == Objectification.NestedFactTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "NestedPredicate", null, CustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == ObjectTypeImpliesMandatoryConstraint.MandatoryConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ObjectTypeHasEntityTypeRequiresReferenceSchemeError.ReferenceSchemeErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ObjectTypeHasDuplicateNameError.DuplicateNameErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ObjectTypeHasPreferredIdentifierRequiresMandatoryError.PreferredIdentifierRequiresMandatoryErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ObjectTypeHasCompatibleSupertypesError.CompatibleSupertypesErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ModelNoteReferencesObjectType.NoteDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
			private IComparer<DomainRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<DomainRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				DomainDataDirectory domainDataDirectory = store.DomainDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				DomainRoleInfo domainRole;
				domainRole = domainDataDirectory.FindDomainRole(ObjectTypeHasDefinition.DefinitionDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(ObjectTypeHasNote.NoteDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(ObjectTypeHasAbbreviation.AbbreviationDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(ObjectTypePlaysRole.PlayedRoleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 3;
				domainRole = domainDataDirectory.FindDomainRole(ValueTypeHasDataType.DataTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 4;
				domainRole = domainDataDirectory.FindDomainRole(ValueTypeHasValueConstraint.ValueConstraintDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 5;
				domainRole = domainDataDirectory.FindDomainRole(EntityTypeHasPreferredIdentifier.PreferredIdentifierDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 6;
				domainRole = domainDataDirectory.FindDomainRole(Objectification.NestedFactTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 7;
				domainRole = domainDataDirectory.FindDomainRole(ValueTypeHasValueTypeInstance.ValueTypeInstanceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 8;
				domainRole = domainDataDirectory.FindDomainRole(EntityTypeHasEntityTypeInstance.EntityTypeInstanceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 8;
				domainRole = domainDataDirectory.FindDomainRole(ObjectTypeImpliesMandatoryConstraint.MandatoryConstraintDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 9;
				domainRole = domainDataDirectory.FindDomainRole(ObjectTypeHasEntityTypeRequiresReferenceSchemeError.ReferenceSchemeErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 10;
				domainRole = domainDataDirectory.FindDomainRole(ObjectTypeHasDuplicateNameError.DuplicateNameErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 11;
				domainRole = domainDataDirectory.FindDomainRole(ObjectTypeHasPreferredIdentifierRequiresMandatoryError.PreferredIdentifierRequiresMandatoryErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 12;
				domainRole = domainDataDirectory.FindDomainRole(ObjectTypeHasCompatibleSupertypesError.CompatibleSupertypesErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 13;
				domainRole = domainDataDirectory.FindDomainRole(ModelNoteReferencesObjectType.NoteDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 14;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<DomainRoleInfo>.Compare(DomainRoleInfo x, DomainRoleInfo y)
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
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(x.DomainRelationship.ImplementationClass.FullName, ".", x.Name), out xPos))
				{
					xPos = int.MaxValue;
				}
				int yPos;
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(y.DomainRelationship.ImplementationClass.FullName, ".", y.Name), out yPos))
				{
					yPos = int.MaxValue;
				}
				return xPos.CompareTo(yPos);
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		protected new IComparer<DomainRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<DomainRoleInfo> retVal = ObjectType.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					ObjectType.myCustomSortChildComparer = retVal;
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
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ObjectType.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ValueTypeHasDataType.DataTypeDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ConceptualDataType", match);
				match.InitializeRoles(EntityTypeHasPreferredIdentifier.PreferredIdentifierDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|PreferredIdentifier", match);
				match.InitializeRoles(Objectification.NestedFactTypeDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|NestedPredicate", match);
				match.InitializeRoles(ObjectTypePlaysRole.PlayedRoleDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|PlayedRoles||Role", match);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|PlayedRoles||SubtypeMetaRole", match);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|PlayedRoles||SupertypeMetaRole", match);
				match.InitializeRoles(ObjectTypeHasDefinition.DefinitionDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Definitions||", match);
				match.InitializeRoles(ObjectTypeHasNote.NoteDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Notes||", match);
				match.InitializeRoles(ObjectTypeHasAbbreviation.AbbreviationDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Abbreviations||", match);
				match.InitializeRoles(ValueTypeHasValueConstraint.ValueConstraintDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ValueRestriction||", match);
				match.InitializeRoles(ValueTypeHasValueTypeInstance.ValueTypeInstanceDomainRoleId, EntityTypeHasEntityTypeInstance.EntityTypeInstanceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Instances||", match);
				ObjectType.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
			}
			return rVal;
		}
		CustomSerializedElementMatch ICustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = ObjectType.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("_ReferenceMode", ObjectType.ReferenceModeStringDomainPropertyId);
				customSerializedAttributes.Add("IsIndependent", ObjectType.IsIndependentDomainPropertyId);
				customSerializedAttributes.Add("IsExternal", ObjectType.IsExternalDomainPropertyId);
				customSerializedAttributes.Add("IsPersonal", ObjectType.IsPersonalDomainPropertyId);
				customSerializedAttributes.Add("IsImplicitBooleanValue", ObjectType.IsImplicitBooleanValueDomainPropertyId);
				ObjectType.myCustomSerializedAttributes = customSerializedAttributes;
			}
			Guid rVal;
			string key = attributeName;
			if (xmlNamespace.Length != 0)
			{
				key = string.Concat(xmlNamespace, "|", attributeName);
			}
			if (!customSerializedAttributes.TryGetValue(key, out rVal))
			{
				rVal = base.MapAttribute(xmlNamespace, attributeName);
			}
			return rVal;
		}
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
	}
	#endregion // ObjectType serialization
	#region RecognizedPhrase serialization
	partial class RecognizedPhrase : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
			CustomSerializedContainerElementInfo[] ret = RecognizedPhrase.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "Abbreviations", null, CustomSerializedElementWriteStyle.Element, null, RecognizedPhraseHasAbbreviation.AbbreviationDomainRoleId);
				RecognizedPhrase.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == RecognizedPhraseHasDuplicateNameError.DuplicateNameErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = RecognizedPhrase.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(RecognizedPhraseHasAbbreviation.AbbreviationDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Abbreviations||", match);
				RecognizedPhrase.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // RecognizedPhrase serialization
	#region ValueTypeInstance serialization
	partial class ValueTypeInstance : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.MixedTypedAttributes;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		protected new CustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new CustomSerializedElementInfo(null, "ValueTypeInstance", null, CustomSerializedElementWriteStyle.Element, null);
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
		protected new CustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == ValueTypeInstance.ValueDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == EntityTypeRoleInstance.RoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FactTypeRoleInstance.RoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ValueTypeInstanceHasCompatibleValueTypeInstanceValueError.CompatibleValueTypeInstanceValueErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ObjectTypeInstanceHasPopulationMandatoryError.PopulationMandatoryErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ValueTypeInstance.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeAttribute(ValueTypeInstance.ValueDomainPropertyId, null);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Value", match);
				ValueTypeInstance.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // ValueTypeInstance serialization
	#region EntityTypeInstance serialization
	partial class EntityTypeInstance : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
			CustomSerializedContainerElementInfo[] ret = EntityTypeInstance.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "RoleInstances", null, CustomSerializedElementWriteStyle.Element, null, EntityTypeInstanceHasRoleInstance.RoleInstanceDomainRoleId);
				EntityTypeInstance.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		protected new CustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new CustomSerializedElementInfo(null, "EntityTypeInstance", null, CustomSerializedElementWriteStyle.Element, null);
			}
		}
		CustomSerializedElementInfo ICustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return this.CustomSerializedElementInfo;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == EntityTypeInstanceHasRoleInstance.RoleInstanceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "EntityTypeRoleInstance", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == EntityTypeRoleInstance.RoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FactTypeRoleInstance.RoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == EntityTypeInstanceHasTooFewEntityTypeRoleInstancesError.TooFewEntityTypeRoleInstancesErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ObjectTypeInstanceHasPopulationMandatoryError.PopulationMandatoryErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = EntityTypeInstance.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(EntityTypeInstanceHasRoleInstance.RoleInstanceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|RoleInstances||EntityTypeRoleInstance", match);
				EntityTypeInstance.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // EntityTypeInstance serialization
	#region EntityTypeRoleInstance serialization
	partial class EntityTypeRoleInstance : ICustomSerializedElement
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
			if (roleId == EntityTypeInstanceHasRoleInstance.EntityTypeInstanceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == RoleInstanceHasPopulationUniquenessError.PopulationUniquenessErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == EntityTypeRoleInstanceHasPopulationUniquenessError.PopulationUniquenessErrorDomainRoleId)
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
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return default(CustomSerializedElementMatch);
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
	#endregion // EntityTypeRoleInstance serialization
	#region CustomReferenceMode serialization
	partial class CustomReferenceMode : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles | CustomSerializedElementSupportedOperations.MixedTypedAttributes;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		protected new CustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == CustomReferenceMode.CustomFormatStringDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ReferenceModeHasReferenceModeKind.KindDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Kind", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
			private IComparer<DomainRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<DomainRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				DomainDataDirectory domainDataDirectory = store.DomainDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				DomainRoleInfo domainRole;
				domainRole = domainDataDirectory.FindDomainRole(ReferenceModeHasReferenceModeKind.KindDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<DomainRoleInfo>.Compare(DomainRoleInfo x, DomainRoleInfo y)
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
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(x.DomainRelationship.ImplementationClass.FullName, ".", x.Name), out xPos))
				{
					xPos = int.MaxValue;
				}
				int yPos;
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(y.DomainRelationship.ImplementationClass.FullName, ".", y.Name), out yPos))
				{
					yPos = int.MaxValue;
				}
				return xPos.CompareTo(yPos);
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		protected new IComparer<DomainRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<DomainRoleInfo> retVal = CustomReferenceMode.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					CustomReferenceMode.myCustomSortChildComparer = retVal;
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
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = CustomReferenceMode.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ReferenceModeHasReferenceModeKind.KindDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Kind", match);
				match.InitializeAttribute(CustomReferenceMode.CustomFormatStringDomainPropertyId, null);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|CustomFormatString", match);
				CustomReferenceMode.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // CustomReferenceMode serialization
	#region ValueTypeHasDataType serialization
	partial class ValueTypeHasDataType : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
			if (domainPropertyInfo.Id == ValueTypeHasDataType.ScaleDomainPropertyId)
			{
				if (rolePlayedInfo.Id == ValueTypeHasDataType.ValueTypeDomainRoleId)
				{
					return new CustomSerializedPropertyInfo(null, "Scale", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
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
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ValueTypeHasUnspecifiedDataTypeError.DataTypeNotSpecifiedErrorDomainRoleId)
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
			Dictionary<string, Guid> customSerializedAttributes = ValueTypeHasDataType.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("Scale", ValueTypeHasDataType.ScaleDomainPropertyId);
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
	#endregion // ValueTypeHasDataType serialization
	#region DataType serialization
	partial class DataType : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ValueTypeHasDataType.ValueTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		/// <summary>Implements ICustomSerializedElement.ShouldSerialize</summary>
		protected new bool ShouldSerialize()
		{
			return DomainRoleInfo.GetAllElementLinks(this).Count > 1;
		}
		bool ICustomSerializedElement.ShouldSerialize()
		{
			return this.ShouldSerialize();
		}
	}
	#endregion // DataType serialization
	#region ReferenceModeKind serialization
	partial class ReferenceModeKind : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		protected new CustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new CustomSerializedElementInfo(null, "ReferenceModeKind", null, CustomSerializedElementWriteStyle.Element, null);
			}
		}
		CustomSerializedElementInfo ICustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return this.CustomSerializedElementInfo;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ReferenceModeHasReferenceModeKind.ReferenceModeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
	}
	#endregion // ReferenceModeKind serialization
	#region ValueConstraint serialization
	partial class ValueConstraint : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
			CustomSerializedContainerElementInfo[] ret = ValueConstraint.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "ValueRanges", null, CustomSerializedElementWriteStyle.Element, null, ValueConstraintHasValueRange.ValueRangeDomainRoleId);
				ValueConstraint.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		protected new CustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new CustomSerializedElementInfo(null, "BaseValueConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
		}
		CustomSerializedElementInfo ICustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return this.CustomSerializedElementInfo;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ValueConstraintHasDuplicateNameError.DuplicateNameErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ValueConstraintHasValueRangeOverlapError.ValueRangeOverlapErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ValueConstraintHasValueTypeDetachedError.ValueTypeDetachedErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ValueConstraint.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ValueConstraintHasValueRange.ValueRangeDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ValueRanges||", match);
				ValueConstraint.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // ValueConstraint serialization
	#region ValueTypeValueConstraint serialization
	partial class ValueTypeValueConstraint : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ElementInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		protected new CustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new CustomSerializedElementInfo(null, "ValueConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
		}
		CustomSerializedElementInfo ICustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return this.CustomSerializedElementInfo;
			}
		}
	}
	#endregion // ValueTypeValueConstraint serialization
	#region RoleValueConstraint serialization
	partial class RoleValueConstraint : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ElementInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		protected new CustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new CustomSerializedElementInfo(null, "RoleValueConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
		}
		CustomSerializedElementInfo ICustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return this.CustomSerializedElementInfo;
			}
		}
	}
	#endregion // RoleValueConstraint serialization
	#region ValueRange serialization
	partial class ValueRange : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ValueRangeHasMinValueMismatchError.MinValueMismatchErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ValueRangeHasMaxValueMismatchError.MaxValueMismatchErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
	}
	#endregion // ValueRange serialization
	#region FactType serialization
	partial class FactType : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
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
			CustomSerializedContainerElementInfo[] ret = FactType.myCustomSerializedChildElementInfo;
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
				ret = new CustomSerializedContainerElementInfo[baseInfoCount + 7];
				if (baseInfoCount != 0)
				{
					baseInfo.CopyTo(ret, 7);
				}
				ret[0] = new CustomSerializedContainerElementInfo(null, "Definitions", null, CustomSerializedElementWriteStyle.Element, null, FactTypeHasDefinition.DefinitionDomainRoleId);
				ret[1] = new CustomSerializedContainerElementInfo(null, "Notes", null, CustomSerializedElementWriteStyle.Element, null, FactTypeHasNote.NoteDomainRoleId);
				ret[2] = new CustomSerializedContainerElementInfo(null, "FactRoles", null, CustomSerializedElementWriteStyle.Element, null, FactTypeHasRole.RoleDomainRoleId);
				ret[3] = new CustomSerializedContainerElementInfo(null, "ReadingOrders", null, CustomSerializedElementWriteStyle.Element, null, FactTypeHasReadingOrder.ReadingOrderDomainRoleId);
				ret[4] = new CustomSerializedContainerElementInfo(null, "InternalConstraints", null, CustomSerializedElementWriteStyle.Element, null, FactSetConstraint.SetConstraintDomainRoleId);
				ret[5] = new CustomSerializedContainerElementInfo(null, "DerivationRule", null, CustomSerializedElementWriteStyle.Element, null, FactTypeHasDerivationExpression.DerivationRuleDomainRoleId);
				ret[6] = new CustomSerializedContainerElementInfo(null, "Instances", null, CustomSerializedElementWriteStyle.Element, null, FactTypeHasFactTypeInstance.FactTypeInstanceDomainRoleId);
				FactType.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		protected new CustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				string name = "Fact";
				if (this.ImpliedByObjectification != null)
				{
					name = "ImpliedFact";
				}
				return new CustomSerializedElementInfo(null, name, null, CustomSerializedElementWriteStyle.Element, null);
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
		protected new CustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == FactType.NameDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, "_Name", null, true, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == FactType.IsExternalDomainPropertyId)
			{
				if (!this.IsExternal)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == FactSetConstraint.SetConstraintDomainRoleId)
			{
				string name = null;
				CustomSerializedElementWriteStyle writeStyle = CustomSerializedElementWriteStyle.NotWritten;
				if (ConstraintType.InternalUniqueness == ((IConstraint)DomainRoleInfo.GetRolePlayer(elementLink, FactSetConstraint.SetConstraintDomainRoleId)).ConstraintType)
				{
					name = "UniquenessConstraint";
					writeStyle = CustomSerializedElementWriteStyle.Element;
				}
				else if (ConstraintType.SimpleMandatory == ((IConstraint)DomainRoleInfo.GetRolePlayer(elementLink, FactSetConstraint.SetConstraintDomainRoleId)).ConstraintType)
				{
					name = "MandatoryConstraint";
					writeStyle = CustomSerializedElementWriteStyle.Element;
				}
				return new CustomSerializedElementInfo(null, name, null, writeStyle, null);
			}
			if (roleId == ObjectificationImpliesFactType.ImpliedByObjectificationDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ImpliedByObjectification", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == Objectification.NestingTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FactTypeHasFactTypeRequiresInternalUniquenessConstraintError.InternalUniquenessConstraintRequiredErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FactTypeHasFactTypeRequiresReadingError.ReadingRequiredErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FactTypeHasImpliedInternalUniquenessConstraintError.ImpliedInternalUniquenessConstraintErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FactTypeHasFrequencyConstraintContradictsInternalUniquenessConstraintError.FrequencyConstraintContradictsInternalUniquenessConstraintErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ModelNoteReferencesFactType.NoteDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
			private IComparer<DomainRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<DomainRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				DomainDataDirectory domainDataDirectory = store.DomainDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				DomainRoleInfo domainRole;
				domainRole = domainDataDirectory.FindDomainRole(FactTypeHasDefinition.DefinitionDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(FactTypeHasNote.NoteDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(FactTypeHasRole.RoleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(FactTypeHasReadingOrder.ReadingOrderDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 3;
				domainRole = domainDataDirectory.FindDomainRole(FactSetConstraint.SetConstraintDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 4;
				domainRole = domainDataDirectory.FindDomainRole(FactTypeHasDerivationExpression.DerivationRuleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 5;
				domainRole = domainDataDirectory.FindDomainRole(FactTypeHasFactTypeInstance.FactTypeInstanceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 6;
				domainRole = domainDataDirectory.FindDomainRole(ObjectificationImpliesFactType.ImpliedByObjectificationDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 7;
				domainRole = domainDataDirectory.FindDomainRole(Objectification.NestingTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 8;
				domainRole = domainDataDirectory.FindDomainRole(FactTypeHasFactTypeRequiresInternalUniquenessConstraintError.InternalUniquenessConstraintRequiredErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 9;
				domainRole = domainDataDirectory.FindDomainRole(FactTypeHasFactTypeRequiresReadingError.ReadingRequiredErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 10;
				domainRole = domainDataDirectory.FindDomainRole(FactTypeHasImpliedInternalUniquenessConstraintError.ImpliedInternalUniquenessConstraintErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 11;
				domainRole = domainDataDirectory.FindDomainRole(FactTypeHasFrequencyConstraintContradictsInternalUniquenessConstraintError.FrequencyConstraintContradictsInternalUniquenessConstraintErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 12;
				domainRole = domainDataDirectory.FindDomainRole(ModelNoteReferencesFactType.NoteDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 13;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<DomainRoleInfo>.Compare(DomainRoleInfo x, DomainRoleInfo y)
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
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(x.DomainRelationship.ImplementationClass.FullName, ".", x.Name), out xPos))
				{
					xPos = int.MaxValue;
				}
				int yPos;
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(y.DomainRelationship.ImplementationClass.FullName, ".", y.Name), out yPos))
				{
					yPos = int.MaxValue;
				}
				return xPos.CompareTo(yPos);
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		protected new IComparer<DomainRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<DomainRoleInfo> retVal = FactType.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					FactType.myCustomSortChildComparer = retVal;
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
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = FactType.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ObjectificationImpliesFactType.ImpliedByObjectificationDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ImpliedByObjectification", match);
				match.InitializeRoles(FactSetConstraint.SetConstraintDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|InternalConstraints||UniquenessConstraint", match);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|InternalConstraints||MandatoryConstraint", match);
				match.InitializeRoles(FactTypeHasDefinition.DefinitionDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Definitions||", match);
				match.InitializeRoles(FactTypeHasNote.NoteDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Notes||", match);
				match.InitializeRoles(FactTypeHasRole.RoleDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|FactRoles||", match);
				match.InitializeRoles(FactTypeHasReadingOrder.ReadingOrderDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ReadingOrders||", match);
				match.InitializeRoles(FactTypeHasDerivationExpression.DerivationRuleDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|DerivationRule||", match);
				match.InitializeRoles(FactTypeHasFactTypeInstance.FactTypeInstanceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Instances||", match);
				FactType.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
			}
			return rVal;
		}
		CustomSerializedElementMatch ICustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = FactType.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("_Name", FactType.NameDomainPropertyId);
				customSerializedAttributes.Add("IsExternal", FactType.IsExternalDomainPropertyId);
				FactType.myCustomSerializedAttributes = customSerializedAttributes;
			}
			Guid rVal;
			string key = attributeName;
			if (xmlNamespace.Length != 0)
			{
				key = string.Concat(xmlNamespace, "|", attributeName);
			}
			if (!customSerializedAttributes.TryGetValue(key, out rVal))
			{
				rVal = base.MapAttribute(xmlNamespace, attributeName);
			}
			return rVal;
		}
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
	}
	#endregion // FactType serialization
	#region FactTypeInstance serialization
	partial class FactTypeInstance : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
			CustomSerializedContainerElementInfo[] ret = FactTypeInstance.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "RoleInstances", null, CustomSerializedElementWriteStyle.Element, null, FactTypeInstanceHasRoleInstance.RoleInstanceDomainRoleId);
				FactTypeInstance.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		protected new CustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new CustomSerializedElementInfo(null, "FactTypeInstance", null, CustomSerializedElementWriteStyle.Element, null);
			}
		}
		CustomSerializedElementInfo ICustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return this.CustomSerializedElementInfo;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == FactTypeInstanceHasRoleInstance.RoleInstanceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "FactTypeRoleInstance", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == EntityTypeRoleInstance.RoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FactTypeRoleInstance.RoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FactTypeInstanceHasTooFewFactTypeRoleInstancesError.TooFewFactTypeRoleInstancesErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = FactTypeInstance.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(FactTypeInstanceHasRoleInstance.RoleInstanceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|RoleInstances||FactTypeRoleInstance", match);
				FactTypeInstance.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // FactTypeInstance serialization
	#region FactTypeRoleInstance serialization
	partial class FactTypeRoleInstance : ICustomSerializedElement
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
			if (roleId == FactTypeInstanceHasRoleInstance.FactTypeInstanceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == RoleInstanceHasPopulationUniquenessError.PopulationUniquenessErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FactTypeRoleInstanceHasPopulationUniquenessError.PopulationUniquenessErrorDomainRoleId)
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
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return default(CustomSerializedElementMatch);
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
	#endregion // FactTypeRoleInstance serialization
	#region Expression serialization
	partial class Expression : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.MixedTypedAttributes;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		protected new CustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == Expression.LanguageDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
			}
			if (domainPropertyInfo.Id == Expression.BodyDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = Expression.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeAttribute(Expression.BodyDomainPropertyId, null);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Body", match);
				Expression.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // Expression serialization
	#region FactTypeDerivationExpression serialization
	partial class FactTypeDerivationExpression : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ElementInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		protected new CustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new CustomSerializedElementInfo(null, "DerivationExpression", null, CustomSerializedElementWriteStyle.Element, null);
			}
		}
		CustomSerializedElementInfo ICustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return this.CustomSerializedElementInfo;
			}
		}
	}
	#endregion // FactTypeDerivationExpression serialization
	#region SubtypeFact serialization
	partial class SubtypeFact : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		protected new CustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new CustomSerializedElementInfo(null, "SubtypeFact", null, CustomSerializedElementWriteStyle.Element, null);
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
		protected new CustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == SubtypeFact.IsPrimaryDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
			}
			if (domainPropertyInfo.Id == SubtypeFact.ProvidesPreferredIdentifierDomainPropertyId)
			{
				if (!this.ProvidesPreferredIdentifier)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, "PreferredIdentificationPath", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = SubtypeFact.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("IsPrimary", SubtypeFact.IsPrimaryDomainPropertyId);
				customSerializedAttributes.Add("PreferredIdentificationPath", SubtypeFact.ProvidesPreferredIdentifierDomainPropertyId);
				SubtypeFact.myCustomSerializedAttributes = customSerializedAttributes;
			}
			Guid rVal;
			string key = attributeName;
			if (xmlNamespace.Length != 0)
			{
				key = string.Concat(xmlNamespace, "|", attributeName);
			}
			if (!customSerializedAttributes.TryGetValue(key, out rVal))
			{
				rVal = base.MapAttribute(xmlNamespace, attributeName);
			}
			return rVal;
		}
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
	}
	#endregion // SubtypeFact serialization
	#region Objectification serialization
	partial class Objectification : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
			if (domainPropertyInfo.Id == Objectification.IsImpliedDomainPropertyId)
			{
				if (!this.IsImplied)
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
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ObjectificationImpliesFactType.ImpliedFactTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ImpliedByObjectification", null, CustomSerializedElementWriteStyle.NotWritten, null);
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
			Dictionary<string, Guid> customSerializedAttributes = Objectification.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("IsImplied", Objectification.IsImpliedDomainPropertyId);
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
	#endregion // Objectification serialization
	#region ReadingOrder serialization
	partial class ReadingOrder : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
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
			CustomSerializedContainerElementInfo[] ret = ReadingOrder.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "Readings", null, CustomSerializedElementWriteStyle.Element, null, ReadingOrderHasReading.ReadingDomainRoleId);
				ret[1] = new CustomSerializedContainerElementInfo(null, "RoleSequence", null, CustomSerializedElementWriteStyle.Element, null, ReadingOrderHasRole.RoleDomainRoleId);
				ReadingOrder.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ReadingOrderHasRole.RoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Role", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
			private IComparer<DomainRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<DomainRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				DomainDataDirectory domainDataDirectory = store.DomainDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				DomainRoleInfo domainRole;
				domainRole = domainDataDirectory.FindDomainRole(ReadingOrderHasReading.ReadingDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(ReadingOrderHasRole.RoleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<DomainRoleInfo>.Compare(DomainRoleInfo x, DomainRoleInfo y)
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
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(x.DomainRelationship.ImplementationClass.FullName, ".", x.Name), out xPos))
				{
					xPos = int.MaxValue;
				}
				int yPos;
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(y.DomainRelationship.ImplementationClass.FullName, ".", y.Name), out yPos))
				{
					yPos = int.MaxValue;
				}
				return xPos.CompareTo(yPos);
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		protected new IComparer<DomainRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<DomainRoleInfo> retVal = ReadingOrder.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					ReadingOrder.myCustomSortChildComparer = retVal;
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
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ReadingOrder.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ReadingOrderHasRole.RoleDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|RoleSequence||Role", match);
				match.InitializeRoles(ReadingOrderHasReading.ReadingDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Readings||", match);
				ReadingOrder.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // ReadingOrder serialization
	#region Reading serialization
	partial class Reading : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.MixedTypedAttributes;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		protected new CustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == Reading.TextDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, "Data", null, false, CustomSerializedAttributeWriteStyle.Element, null);
			}
			if (domainPropertyInfo.Id == Reading.LanguageDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ReadingHasTooManyRolesError.TooManyRolesErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ReadingHasTooFewRolesError.TooFewRolesErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ReadingHasReadingRequiresUserModificationError.RequiresUserModificationErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = Reading.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeAttribute(Reading.TextDomainPropertyId, null);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Data", match);
				Reading.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // Reading serialization
	#region Role serialization
	partial class Role : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
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
			CustomSerializedContainerElementInfo[] ret = Role.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "ValueRestriction", null, CustomSerializedElementWriteStyle.Element, null, RoleHasValueConstraint.ValueConstraintDomainRoleId);
				ret[1] = new CustomSerializedContainerElementInfo(null, "RoleInstances", null, CustomSerializedElementWriteStyle.Element, null, EntityTypeRoleInstance.ObjectTypeInstanceDomainRoleId, FactTypeRoleInstance.ObjectTypeInstanceDomainRoleId);
				Role.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		protected new CustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == Role.IsMandatoryDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, "_IsMandatory", null, true, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == Role.MultiplicityDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, "_Multiplicity", null, true, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ObjectTypePlaysRole.RolePlayerDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "RolePlayer", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == EntityTypeRoleInstance.ObjectTypeInstanceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "EntityTypeRoleInstance", null, CustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == FactTypeRoleInstance.ObjectTypeInstanceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "FactTypeRoleInstance", null, CustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == RoleProxyHasRole.ProxyDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConstraintRoleSequenceHasRole.ConstraintRoleSequenceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ReadingOrderHasRole.ReadingOrderDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == RoleHasRolePlayerRequiredError.RolePlayerRequiredErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == JoinHasInputRole.JoinDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == JoinHasOutputRole.JoinDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
			private IComparer<DomainRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<DomainRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				DomainDataDirectory domainDataDirectory = store.DomainDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				DomainRoleInfo domainRole;
				domainRole = domainDataDirectory.FindDomainRole(ObjectTypePlaysRole.RolePlayerDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(RoleHasValueConstraint.ValueConstraintDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(EntityTypeRoleInstance.ObjectTypeInstanceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(FactTypeRoleInstance.ObjectTypeInstanceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(RoleProxyHasRole.ProxyDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 3;
				domainRole = domainDataDirectory.FindDomainRole(ConstraintRoleSequenceHasRole.ConstraintRoleSequenceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 4;
				domainRole = domainDataDirectory.FindDomainRole(ReadingOrderHasRole.ReadingOrderDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 5;
				domainRole = domainDataDirectory.FindDomainRole(RoleHasRolePlayerRequiredError.RolePlayerRequiredErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 6;
				domainRole = domainDataDirectory.FindDomainRole(JoinHasInputRole.JoinDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 7;
				domainRole = domainDataDirectory.FindDomainRole(JoinHasOutputRole.JoinDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 8;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<DomainRoleInfo>.Compare(DomainRoleInfo x, DomainRoleInfo y)
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
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(x.DomainRelationship.ImplementationClass.FullName, ".", x.Name), out xPos))
				{
					xPos = int.MaxValue;
				}
				int yPos;
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(y.DomainRelationship.ImplementationClass.FullName, ".", y.Name), out yPos))
				{
					yPos = int.MaxValue;
				}
				return xPos.CompareTo(yPos);
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		protected new IComparer<DomainRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<DomainRoleInfo> retVal = Role.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					Role.myCustomSortChildComparer = retVal;
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
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = Role.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ObjectTypePlaysRole.RolePlayerDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|RolePlayer", match);
				match.InitializeRoles(true, EntityTypeRoleInstance.ObjectTypeInstanceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|RoleInstances||EntityTypeRoleInstance", match);
				match.InitializeRoles(true, FactTypeRoleInstance.ObjectTypeInstanceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|RoleInstances||FactTypeRoleInstance", match);
				match.InitializeRoles(RoleHasValueConstraint.ValueConstraintDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ValueRestriction||", match);
				Role.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
			}
			return rVal;
		}
		CustomSerializedElementMatch ICustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = Role.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("_IsMandatory", Role.IsMandatoryDomainPropertyId);
				customSerializedAttributes.Add("_Multiplicity", Role.MultiplicityDomainPropertyId);
				Role.myCustomSerializedAttributes = customSerializedAttributes;
			}
			Guid rVal;
			string key = attributeName;
			if (xmlNamespace.Length != 0)
			{
				key = string.Concat(xmlNamespace, "|", attributeName);
			}
			if (!customSerializedAttributes.TryGetValue(key, out rVal))
			{
				rVal = base.MapAttribute(xmlNamespace, attributeName);
			}
			return rVal;
		}
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
	}
	#endregion // Role serialization
	#region RoleProxy serialization
	partial class RoleProxy : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == RoleProxyHasRole.TargetRoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Role", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == ReadingOrderHasRole.ReadingOrderDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = RoleProxy.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(RoleProxyHasRole.TargetRoleDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Role", match);
				RoleProxy.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // RoleProxy serialization
	#region SetComparisonConstraint serialization
	partial class SetComparisonConstraint : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
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
			CustomSerializedContainerElementInfo[] ret = SetComparisonConstraint.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "RoleSequences", null, CustomSerializedElementWriteStyle.Element, null, SetComparisonConstraintHasRoleSequence.RoleSequenceDomainRoleId);
				SetComparisonConstraint.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		protected new CustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == SetComparisonConstraint.ModalityDomainPropertyId)
			{
				if (ConstraintModality.Alethic == this.Modality)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == SetComparisonConstraintHasCompatibleRolePlayerTypeError.CompatibleRolePlayerTypeErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == SetComparisonConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == SetComparisonConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == SetComparisonConstraintHasExternalConstraintRoleSequenceArityMismatchError.ArityMismatchErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == SetComparisonConstraintHasImplicationError.ImplicationErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == SetComparisonConstraintHasContradictionError.ContradictionErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == SetComparisonConstraintHasDuplicateNameError.DuplicateNameErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == SetComparisonConstraintHasEqualityOrSubsetImpliedByMandatoryError.EqualityOrSubsetImpliedByMandatoryErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ModelNoteReferencesSetComparisonConstraint.NoteDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
			private IComparer<DomainRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<DomainRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				DomainDataDirectory domainDataDirectory = store.DomainDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				DomainRoleInfo domainRole;
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasRoleSequence.RoleSequenceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasCompatibleRolePlayerTypeError.CompatibleRolePlayerTypeErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 3;
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasExternalConstraintRoleSequenceArityMismatchError.ArityMismatchErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 4;
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasImplicationError.ImplicationErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 5;
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasContradictionError.ContradictionErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 6;
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasDuplicateNameError.DuplicateNameErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 7;
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasEqualityOrSubsetImpliedByMandatoryError.EqualityOrSubsetImpliedByMandatoryErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 8;
				domainRole = domainDataDirectory.FindDomainRole(ModelNoteReferencesSetComparisonConstraint.NoteDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 9;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<DomainRoleInfo>.Compare(DomainRoleInfo x, DomainRoleInfo y)
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
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(x.DomainRelationship.ImplementationClass.FullName, ".", x.Name), out xPos))
				{
					xPos = int.MaxValue;
				}
				int yPos;
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(y.DomainRelationship.ImplementationClass.FullName, ".", y.Name), out yPos))
				{
					yPos = int.MaxValue;
				}
				return xPos.CompareTo(yPos);
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		protected new IComparer<DomainRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<DomainRoleInfo> retVal = SetComparisonConstraint.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					SetComparisonConstraint.myCustomSortChildComparer = retVal;
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
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = SetComparisonConstraint.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(SetComparisonConstraintHasRoleSequence.RoleSequenceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|RoleSequences||", match);
				SetComparisonConstraint.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
			}
			return rVal;
		}
		CustomSerializedElementMatch ICustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = SetComparisonConstraint.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("Modality", SetComparisonConstraint.ModalityDomainPropertyId);
				SetComparisonConstraint.myCustomSerializedAttributes = customSerializedAttributes;
			}
			Guid rVal;
			string key = attributeName;
			if (xmlNamespace.Length != 0)
			{
				key = string.Concat(xmlNamespace, "|", attributeName);
			}
			if (!customSerializedAttributes.TryGetValue(key, out rVal))
			{
				rVal = base.MapAttribute(xmlNamespace, attributeName);
			}
			return rVal;
		}
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
	}
	#endregion // SetComparisonConstraint serialization
	#region SetComparisonConstraintRoleSequence serialization
	partial class SetComparisonConstraintRoleSequence : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
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
			CustomSerializedContainerElementInfo[] ret = SetComparisonConstraintRoleSequence.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "JoinPath", null, CustomSerializedElementWriteStyle.Element, null, ConstraintRoleSequenceHasJoinPath.JoinDomainRoleId);
				SetComparisonConstraintRoleSequence.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		protected new CustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new CustomSerializedElementInfo(null, "RoleSequence", null, CustomSerializedElementWriteStyle.Element, null);
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
		protected new CustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == SetComparisonConstraintRoleSequence.NameDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ConstraintRoleSequenceHasRole.RoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Role", null, CustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
			private IComparer<DomainRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<DomainRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				DomainDataDirectory domainDataDirectory = store.DomainDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				DomainRoleInfo domainRole;
				domainRole = domainDataDirectory.FindDomainRole(ConstraintRoleSequenceHasRole.RoleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(ConstraintRoleSequenceHasJoinPath.JoinDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<DomainRoleInfo>.Compare(DomainRoleInfo x, DomainRoleInfo y)
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
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(x.DomainRelationship.ImplementationClass.FullName, ".", x.Name), out xPos))
				{
					xPos = int.MaxValue;
				}
				int yPos;
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(y.DomainRelationship.ImplementationClass.FullName, ".", y.Name), out yPos))
				{
					yPos = int.MaxValue;
				}
				return xPos.CompareTo(yPos);
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		protected new IComparer<DomainRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<DomainRoleInfo> retVal = SetComparisonConstraintRoleSequence.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					SetComparisonConstraintRoleSequence.myCustomSortChildComparer = retVal;
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
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = SetComparisonConstraintRoleSequence.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ConstraintRoleSequenceHasRole.RoleDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Role", match);
				match.InitializeRoles(ConstraintRoleSequenceHasJoinPath.JoinDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|JoinPath||", match);
				SetComparisonConstraintRoleSequence.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // SetComparisonConstraintRoleSequence serialization
	#region SetConstraint serialization
	partial class SetConstraint : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
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
			CustomSerializedContainerElementInfo[] ret = SetConstraint.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "RoleSequence", null, CustomSerializedElementWriteStyle.Element, null, ConstraintRoleSequenceHasRole.RoleDomainRoleId);
				ret[1] = new CustomSerializedInnerContainerElementInfo(null, "JoinPath", null, CustomSerializedElementWriteStyle.Element, null, ret[0], ConstraintRoleSequenceHasJoinPath.JoinDomainRoleId);
				SetConstraint.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		protected new CustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == SetConstraint.ModalityDomainPropertyId)
			{
				if (ConstraintModality.Alethic == this.Modality)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ConstraintRoleSequenceHasRole.RoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Role", null, CustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == SetConstraintHasCompatibleRolePlayerTypeError.CompatibleRolePlayerTypeErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == SetConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == SetConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FactSetConstraint.FactTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == SetConstraintHasImplicationError.ImplicationErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == SetConstraintHasDuplicateNameError.DuplicateNameErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ModelNoteReferencesSetConstraint.NoteDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
			private IComparer<DomainRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<DomainRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				DomainDataDirectory domainDataDirectory = store.DomainDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				DomainRoleInfo domainRole;
				domainRole = domainDataDirectory.FindDomainRole(ConstraintRoleSequenceHasRole.RoleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(ConstraintRoleSequenceHasJoinPath.JoinDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(SetConstraintHasCompatibleRolePlayerTypeError.CompatibleRolePlayerTypeErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(SetConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 3;
				domainRole = domainDataDirectory.FindDomainRole(SetConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 4;
				domainRole = domainDataDirectory.FindDomainRole(FactSetConstraint.FactTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 5;
				domainRole = domainDataDirectory.FindDomainRole(SetConstraintHasImplicationError.ImplicationErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 6;
				domainRole = domainDataDirectory.FindDomainRole(SetConstraintHasDuplicateNameError.DuplicateNameErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 7;
				domainRole = domainDataDirectory.FindDomainRole(ModelNoteReferencesSetConstraint.NoteDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 8;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<DomainRoleInfo>.Compare(DomainRoleInfo x, DomainRoleInfo y)
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
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(x.DomainRelationship.ImplementationClass.FullName, ".", x.Name), out xPos))
				{
					xPos = int.MaxValue;
				}
				int yPos;
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(y.DomainRelationship.ImplementationClass.FullName, ".", y.Name), out yPos))
				{
					yPos = int.MaxValue;
				}
				return xPos.CompareTo(yPos);
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		protected new IComparer<DomainRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<DomainRoleInfo> retVal = SetConstraint.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					SetConstraint.myCustomSortChildComparer = retVal;
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
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = SetConstraint.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ConstraintRoleSequenceHasRole.RoleDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|RoleSequence||Role", match);
				match.InitializeRoles(ConstraintRoleSequenceHasJoinPath.JoinDomainRoleId);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|RoleSequence||JoinPath||", match);
				SetConstraint.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
			}
			return rVal;
		}
		CustomSerializedElementMatch ICustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = SetConstraint.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("Modality", SetConstraint.ModalityDomainPropertyId);
				SetConstraint.myCustomSerializedAttributes = customSerializedAttributes;
			}
			Guid rVal;
			string key = attributeName;
			if (xmlNamespace.Length != 0)
			{
				key = string.Concat(xmlNamespace, "|", attributeName);
			}
			if (!customSerializedAttributes.TryGetValue(key, out rVal))
			{
				rVal = base.MapAttribute(xmlNamespace, attributeName);
			}
			return rVal;
		}
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
	}
	#endregion // SetConstraint serialization
	#region ConstraintRoleSequenceHasRole serialization
	partial class ConstraintRoleSequenceHasRole : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
				return new CustomSerializedElementInfo(null, "Role", null, CustomSerializedElementWriteStyle.Element, null);
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
			if (roleId == ConstraintRoleSequenceHasRoleHasProjectionJoin.ProjectionJoinDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ProjectionJoin", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ConstraintRoleSequenceHasRole.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ConstraintRoleSequenceHasRoleHasProjectionJoin.ProjectionJoinDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ProjectionJoin", match);
				ConstraintRoleSequenceHasRole.myChildElementMappings = childElementMappings;
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
	#endregion // ConstraintRoleSequenceHasRole serialization
	#region MandatoryConstraint serialization
	partial class MandatoryConstraint : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		protected new CustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == MandatoryConstraint.IsSimpleDomainPropertyId)
			{
				if (!this.IsSimple)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == MandatoryConstraint.IsImpliedDomainPropertyId)
			{
				if (!this.IsImplied)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ObjectTypeImpliesMandatoryConstraint.ObjectTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ImpliedByObjectType", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == ExclusiveOrConstraintCoupler.ExclusionConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ExclusiveOrExclusionConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == MandatoryConstraintHasPopulationMandatoryError.PopulationMandatoryErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == MandatoryConstraintHasExclusionContradictsMandatoryError.ExclusionContradictsMandatoryErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == MandatoryConstraintHasNotWellModeledSubsetAndMandatoryError.NotWellModeledSubsetAndMandatoryErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
			private IComparer<DomainRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<DomainRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				DomainDataDirectory domainDataDirectory = store.DomainDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				DomainRoleInfo domainRole;
				domainRole = domainDataDirectory.FindDomainRole(ObjectTypeImpliesMandatoryConstraint.ObjectTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(ExclusiveOrConstraintCoupler.ExclusionConstraintDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(MandatoryConstraintHasPopulationMandatoryError.PopulationMandatoryErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(MandatoryConstraintHasExclusionContradictsMandatoryError.ExclusionContradictsMandatoryErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 3;
				domainRole = domainDataDirectory.FindDomainRole(MandatoryConstraintHasNotWellModeledSubsetAndMandatoryError.NotWellModeledSubsetAndMandatoryErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 4;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<DomainRoleInfo>.Compare(DomainRoleInfo x, DomainRoleInfo y)
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
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(x.DomainRelationship.ImplementationClass.FullName, ".", x.Name), out xPos))
				{
					xPos = int.MaxValue;
				}
				int yPos;
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(y.DomainRelationship.ImplementationClass.FullName, ".", y.Name), out yPos))
				{
					yPos = int.MaxValue;
				}
				return xPos.CompareTo(yPos);
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		protected new IComparer<DomainRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<DomainRoleInfo> retVal = MandatoryConstraint.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					MandatoryConstraint.myCustomSortChildComparer = retVal;
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
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = MandatoryConstraint.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ObjectTypeImpliesMandatoryConstraint.ObjectTypeDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ImpliedByObjectType", match);
				match.InitializeRoles(ExclusiveOrConstraintCoupler.ExclusionConstraintDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ExclusiveOrExclusionConstraint", match);
				MandatoryConstraint.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
			}
			return rVal;
		}
		CustomSerializedElementMatch ICustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = MandatoryConstraint.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("IsSimple", MandatoryConstraint.IsSimpleDomainPropertyId);
				customSerializedAttributes.Add("IsImplied", MandatoryConstraint.IsImpliedDomainPropertyId);
				MandatoryConstraint.myCustomSerializedAttributes = customSerializedAttributes;
			}
			Guid rVal;
			string key = attributeName;
			if (xmlNamespace.Length != 0)
			{
				key = string.Concat(xmlNamespace, "|", attributeName);
			}
			if (!customSerializedAttributes.TryGetValue(key, out rVal))
			{
				rVal = base.MapAttribute(xmlNamespace, attributeName);
			}
			return rVal;
		}
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
	}
	#endregion // MandatoryConstraint serialization
	#region FrequencyConstraint serialization
	partial class FrequencyConstraint : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == FrequencyConstraintHasFrequencyConstraintExactlyOneError.FrequencyConstraintExactlyOneErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FrequencyConstraintHasFrequencyConstraintMinMaxError.FrequencyConstraintMinMaxErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FrequencyConstraintHasFrequencyConstraintInvalidatedByInternalUniquenessConstraintError.FrequencyConstraintContradictsInternalUniquenessConstraintErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
	}
	#endregion // FrequencyConstraint serialization
	#region UniquenessConstraint serialization
	partial class UniquenessConstraint : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		protected new CustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == UniquenessConstraint.IsInternalDomainPropertyId)
			{
				if (!this.IsInternal)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == EntityTypeHasPreferredIdentifier.PreferredIdentifierForDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "PreferredIdentifierFor", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == UniquenessConstraintHasNMinusOneError.NMinusOneErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
			private IComparer<DomainRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<DomainRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				DomainDataDirectory domainDataDirectory = store.DomainDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				DomainRoleInfo domainRole;
				domainRole = domainDataDirectory.FindDomainRole(EntityTypeHasPreferredIdentifier.PreferredIdentifierForDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(UniquenessConstraintHasNMinusOneError.NMinusOneErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<DomainRoleInfo>.Compare(DomainRoleInfo x, DomainRoleInfo y)
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
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(x.DomainRelationship.ImplementationClass.FullName, ".", x.Name), out xPos))
				{
					xPos = int.MaxValue;
				}
				int yPos;
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(y.DomainRelationship.ImplementationClass.FullName, ".", y.Name), out yPos))
				{
					yPos = int.MaxValue;
				}
				return xPos.CompareTo(yPos);
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		protected new IComparer<DomainRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<DomainRoleInfo> retVal = UniquenessConstraint.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					UniquenessConstraint.myCustomSortChildComparer = retVal;
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
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = UniquenessConstraint.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(EntityTypeHasPreferredIdentifier.PreferredIdentifierForDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|PreferredIdentifierFor", match);
				UniquenessConstraint.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
			}
			return rVal;
		}
		CustomSerializedElementMatch ICustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = UniquenessConstraint.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("IsInternal", UniquenessConstraint.IsInternalDomainPropertyId);
				UniquenessConstraint.myCustomSerializedAttributes = customSerializedAttributes;
			}
			Guid rVal;
			string key = attributeName;
			if (xmlNamespace.Length != 0)
			{
				key = string.Concat(xmlNamespace, "|", attributeName);
			}
			if (!customSerializedAttributes.TryGetValue(key, out rVal))
			{
				rVal = base.MapAttribute(xmlNamespace, attributeName);
			}
			return rVal;
		}
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
	}
	#endregion // UniquenessConstraint serialization
	#region EqualityConstraint serialization
	partial class EqualityConstraint : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == EqualityConstraintHasEqualityImpliedByMandatoryError.EqualityImpliedByMandatoryErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == SetComparisonConstraintHasExclusionContradictsEqualityError.ExclusionContradictsEqualityErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
			private IComparer<DomainRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<DomainRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				DomainDataDirectory domainDataDirectory = store.DomainDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				DomainRoleInfo domainRole;
				domainRole = domainDataDirectory.FindDomainRole(EqualityConstraintHasEqualityImpliedByMandatoryError.EqualityImpliedByMandatoryErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasExclusionContradictsEqualityError.ExclusionContradictsEqualityErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<DomainRoleInfo>.Compare(DomainRoleInfo x, DomainRoleInfo y)
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
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(x.DomainRelationship.ImplementationClass.FullName, ".", x.Name), out xPos))
				{
					xPos = int.MaxValue;
				}
				int yPos;
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(y.DomainRelationship.ImplementationClass.FullName, ".", y.Name), out yPos))
				{
					yPos = int.MaxValue;
				}
				return xPos.CompareTo(yPos);
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		protected new IComparer<DomainRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<DomainRoleInfo> retVal = EqualityConstraint.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					EqualityConstraint.myCustomSortChildComparer = retVal;
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
	}
	#endregion // EqualityConstraint serialization
	#region ExclusionConstraint serialization
	partial class ExclusionConstraint : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ExclusiveOrConstraintCoupler.MandatoryConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ExclusiveOrMandatoryConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == SetComparisonConstraintHasExclusionContradictsEqualityError.ExclusionContradictsEqualityErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == SetComparisonConstraintHasExclusionContradictsSubsetError.ExclusionContradictsSubsetErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ExclusionConstraintHasExclusionContradictsMandatoryError.ExclusionContradictsMandatoryErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
			private IComparer<DomainRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<DomainRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				DomainDataDirectory domainDataDirectory = store.DomainDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				DomainRoleInfo domainRole;
				domainRole = domainDataDirectory.FindDomainRole(ExclusiveOrConstraintCoupler.MandatoryConstraintDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasExclusionContradictsEqualityError.ExclusionContradictsEqualityErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasExclusionContradictsSubsetError.ExclusionContradictsSubsetErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(ExclusionConstraintHasExclusionContradictsMandatoryError.ExclusionContradictsMandatoryErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 3;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<DomainRoleInfo>.Compare(DomainRoleInfo x, DomainRoleInfo y)
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
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(x.DomainRelationship.ImplementationClass.FullName, ".", x.Name), out xPos))
				{
					xPos = int.MaxValue;
				}
				int yPos;
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(y.DomainRelationship.ImplementationClass.FullName, ".", y.Name), out yPos))
				{
					yPos = int.MaxValue;
				}
				return xPos.CompareTo(yPos);
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		protected new IComparer<DomainRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<DomainRoleInfo> retVal = ExclusionConstraint.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					ExclusionConstraint.myCustomSortChildComparer = retVal;
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
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ExclusionConstraint.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ExclusiveOrConstraintCoupler.MandatoryConstraintDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ExclusiveOrMandatoryConstraint", match);
				ExclusionConstraint.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == SetComparisonConstraintHasExclusionContradictsSubsetError.ExclusionContradictsSubsetErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == SubsetConstraintHasNotWellModeledSubsetAndMandatoryError.NotWellModeledSubsetAndMandatoryErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
			private IComparer<DomainRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<DomainRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				DomainDataDirectory domainDataDirectory = store.DomainDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				DomainRoleInfo domainRole;
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasExclusionContradictsSubsetError.ExclusionContradictsSubsetErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(SubsetConstraintHasNotWellModeledSubsetAndMandatoryError.NotWellModeledSubsetAndMandatoryErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<DomainRoleInfo>.Compare(DomainRoleInfo x, DomainRoleInfo y)
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
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(x.DomainRelationship.ImplementationClass.FullName, ".", x.Name), out xPos))
				{
					xPos = int.MaxValue;
				}
				int yPos;
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(y.DomainRelationship.ImplementationClass.FullName, ".", y.Name), out yPos))
				{
					yPos = int.MaxValue;
				}
				return xPos.CompareTo(yPos);
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		protected new IComparer<DomainRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<DomainRoleInfo> retVal = SubsetConstraint.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					SubsetConstraint.myCustomSortChildComparer = retVal;
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
	}
	#endregion // SubsetConstraint serialization
	#region RingConstraint serialization
	partial class RingConstraint : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		protected new CustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == RingConstraint.RingTypeDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, "Type", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == RingConstraintHasRingConstraintTypeNotSpecifiedError.RingConstraintTypeNotSpecifiedErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = RingConstraint.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("Type", RingConstraint.RingTypeDomainPropertyId);
				RingConstraint.myCustomSerializedAttributes = customSerializedAttributes;
			}
			Guid rVal;
			string key = attributeName;
			if (xmlNamespace.Length != 0)
			{
				key = string.Concat(xmlNamespace, "|", attributeName);
			}
			if (!customSerializedAttributes.TryGetValue(key, out rVal))
			{
				rVal = base.MapAttribute(xmlNamespace, attributeName);
			}
			return rVal;
		}
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
	}
	#endregion // RingConstraint serialization
	#region Join serialization
	partial class Join : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		protected new CustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == Join.JoinTypeDomainPropertyId)
			{
				if (this.JoinType == JoinType.Inner)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, "Type", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == JoinHasInputRole.RoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "InputRole", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == JoinHasOutputRole.RoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "OutputRole", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == ConstraintRoleSequenceHasRoleHasProjectionJoin.ConstraintRoleSequenceHasRoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
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
			private IComparer<DomainRoleInfo> myBaseComparer;
			public CustomSortChildComparer(Store store, IComparer<DomainRoleInfo> baseComparer)
			{
				this.myBaseComparer = baseComparer;
				DomainDataDirectory domainDataDirectory = store.DomainDataDirectory;
				Dictionary<string, int> roleOrderDictionary = new Dictionary<string, int>();
				DomainRoleInfo domainRole;
				domainRole = domainDataDirectory.FindDomainRole(JoinHasInputRole.RoleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(JoinHasOutputRole.RoleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(ConstraintRoleSequenceHasRoleHasProjectionJoin.ConstraintRoleSequenceHasRoleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				this.myRoleOrderDictionary = roleOrderDictionary;
			}
			int IComparer<DomainRoleInfo>.Compare(DomainRoleInfo x, DomainRoleInfo y)
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
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(x.DomainRelationship.ImplementationClass.FullName, ".", x.Name), out xPos))
				{
					xPos = int.MaxValue;
				}
				int yPos;
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(y.DomainRelationship.ImplementationClass.FullName, ".", y.Name), out yPos))
				{
					yPos = int.MaxValue;
				}
				return xPos.CompareTo(yPos);
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		protected new IComparer<DomainRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<DomainRoleInfo> retVal = Join.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					Join.myCustomSortChildComparer = retVal;
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
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = Join.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(JoinHasInputRole.RoleDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|InputRole", match);
				match.InitializeRoles(JoinHasOutputRole.RoleDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|OutputRole", match);
				Join.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
			}
			return rVal;
		}
		CustomSerializedElementMatch ICustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = Join.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("Type", Join.JoinTypeDomainPropertyId);
				Join.myCustomSerializedAttributes = customSerializedAttributes;
			}
			Guid rVal;
			string key = attributeName;
			if (xmlNamespace.Length != 0)
			{
				key = string.Concat(xmlNamespace, "|", attributeName);
			}
			if (!customSerializedAttributes.TryGetValue(key, out rVal))
			{
				rVal = base.MapAttribute(xmlNamespace, attributeName);
			}
			return rVal;
		}
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
	}
	#endregion // Join serialization
	#region ConstraintDuplicateNameError serialization
	partial class ConstraintDuplicateNameError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
			CustomSerializedContainerElementInfo[] ret = ConstraintDuplicateNameError.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "Constraints", null, CustomSerializedElementWriteStyle.Element, null, SetComparisonConstraintHasDuplicateNameError.SetComparisonConstraintDomainRoleId, SetConstraintHasDuplicateNameError.SetConstraintDomainRoleId, ValueConstraintHasDuplicateNameError.ValueConstraintDomainRoleId);
				ConstraintDuplicateNameError.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == SetComparisonConstraintHasDuplicateNameError.SetComparisonConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "SetComparisonConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == SetConstraintHasDuplicateNameError.SetConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "SetConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == ValueConstraintHasDuplicateNameError.ValueConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ValueConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ConstraintDuplicateNameError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(SetComparisonConstraintHasDuplicateNameError.SetComparisonConstraintDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Constraints||SetComparisonConstraint", match);
				match.InitializeRoles(SetConstraintHasDuplicateNameError.SetConstraintDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Constraints||SetConstraint", match);
				match.InitializeRoles(ValueConstraintHasDuplicateNameError.ValueConstraintDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Constraints||ValueConstraint", match);
				ConstraintDuplicateNameError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // ConstraintDuplicateNameError serialization
	#region ObjectTypeDuplicateNameError serialization
	partial class ObjectTypeDuplicateNameError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
			CustomSerializedContainerElementInfo[] ret = ObjectTypeDuplicateNameError.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "Objects", null, CustomSerializedElementWriteStyle.Element, null, ObjectTypeHasDuplicateNameError.ObjectTypeDomainRoleId);
				ObjectTypeDuplicateNameError.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ObjectTypeHasDuplicateNameError.ObjectTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Object", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ObjectTypeDuplicateNameError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ObjectTypeHasDuplicateNameError.ObjectTypeDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Objects||Object", match);
				ObjectTypeDuplicateNameError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // ObjectTypeDuplicateNameError serialization
	#region RecognizedPhraseDuplicateNameError serialization
	partial class RecognizedPhraseDuplicateNameError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
			CustomSerializedContainerElementInfo[] ret = RecognizedPhraseDuplicateNameError.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "Objects", null, CustomSerializedElementWriteStyle.Element, null, RecognizedPhraseHasDuplicateNameError.RecognizedPhraseDomainRoleId);
				RecognizedPhraseDuplicateNameError.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == RecognizedPhraseHasDuplicateNameError.RecognizedPhraseDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "RecognizedPhrase", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = RecognizedPhraseDuplicateNameError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(RecognizedPhraseHasDuplicateNameError.RecognizedPhraseDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Objects||RecognizedPhrase", match);
				RecognizedPhraseDuplicateNameError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // RecognizedPhraseDuplicateNameError serialization
	#region EntityTypeRequiresReferenceSchemeError serialization
	partial class EntityTypeRequiresReferenceSchemeError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ObjectTypeHasEntityTypeRequiresReferenceSchemeError.ObjectTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "EntityType", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = EntityTypeRequiresReferenceSchemeError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ObjectTypeHasEntityTypeRequiresReferenceSchemeError.ObjectTypeDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|EntityType", match);
				EntityTypeRequiresReferenceSchemeError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // EntityTypeRequiresReferenceSchemeError serialization
	#region ExternalConstraintRoleSequenceArityMismatchError serialization
	partial class ExternalConstraintRoleSequenceArityMismatchError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == SetComparisonConstraintHasExternalConstraintRoleSequenceArityMismatchError.ConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Constraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ExternalConstraintRoleSequenceArityMismatchError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(SetComparisonConstraintHasExternalConstraintRoleSequenceArityMismatchError.ConstraintDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Constraint", match);
				ExternalConstraintRoleSequenceArityMismatchError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // ExternalConstraintRoleSequenceArityMismatchError serialization
	#region ImpliedInternalUniquenessConstraintError serialization
	partial class ImpliedInternalUniquenessConstraintError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == FactTypeHasImpliedInternalUniquenessConstraintError.FactTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Fact", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ImpliedInternalUniquenessConstraintError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(FactTypeHasImpliedInternalUniquenessConstraintError.FactTypeDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Fact", match);
				ImpliedInternalUniquenessConstraintError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // ImpliedInternalUniquenessConstraintError serialization
	#region FactTypeRequiresInternalUniquenessConstraintError serialization
	partial class FactTypeRequiresInternalUniquenessConstraintError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == FactTypeHasFactTypeRequiresInternalUniquenessConstraintError.FactTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Fact", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = FactTypeRequiresInternalUniquenessConstraintError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(FactTypeHasFactTypeRequiresInternalUniquenessConstraintError.FactTypeDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Fact", match);
				FactTypeRequiresInternalUniquenessConstraintError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // FactTypeRequiresInternalUniquenessConstraintError serialization
	#region FactTypeRequiresReadingError serialization
	partial class FactTypeRequiresReadingError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == FactTypeHasFactTypeRequiresReadingError.FactTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Fact", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = FactTypeRequiresReadingError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(FactTypeHasFactTypeRequiresReadingError.FactTypeDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Fact", match);
				FactTypeRequiresReadingError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // FactTypeRequiresReadingError serialization
	#region FrequencyConstraintExactlyOneError serialization
	partial class FrequencyConstraintExactlyOneError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == FrequencyConstraintHasFrequencyConstraintExactlyOneError.FrequencyConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "FrequencyConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = FrequencyConstraintExactlyOneError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(FrequencyConstraintHasFrequencyConstraintExactlyOneError.FrequencyConstraintDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|FrequencyConstraint", match);
				FrequencyConstraintExactlyOneError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // FrequencyConstraintExactlyOneError serialization
	#region FrequencyConstraintMinMaxError serialization
	partial class FrequencyConstraintMinMaxError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == FrequencyConstraintHasFrequencyConstraintMinMaxError.FrequencyConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "FrequencyConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = FrequencyConstraintMinMaxError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(FrequencyConstraintHasFrequencyConstraintMinMaxError.FrequencyConstraintDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|FrequencyConstraint", match);
				FrequencyConstraintMinMaxError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // FrequencyConstraintMinMaxError serialization
	#region MinValueMismatchError serialization
	partial class MinValueMismatchError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ValueRangeHasMinValueMismatchError.ValueRangeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ValueRange", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = MinValueMismatchError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ValueRangeHasMinValueMismatchError.ValueRangeDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ValueRange", match);
				MinValueMismatchError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // MinValueMismatchError serialization
	#region MaxValueMismatchError serialization
	partial class MaxValueMismatchError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ValueRangeHasMaxValueMismatchError.ValueRangeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ValueRange", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = MaxValueMismatchError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ValueRangeHasMaxValueMismatchError.ValueRangeDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ValueRange", match);
				MaxValueMismatchError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // MaxValueMismatchError serialization
	#region ValueRangeOverlapError serialization
	partial class ValueRangeOverlapError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ValueConstraintHasValueRangeOverlapError.ValueConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ValueConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ValueRangeOverlapError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ValueConstraintHasValueRangeOverlapError.ValueConstraintDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ValueConstraint", match);
				ValueRangeOverlapError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // ValueRangeOverlapError serialization
	#region ValueConstraintValueTypeDetachedError serialization
	partial class ValueConstraintValueTypeDetachedError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		protected new CustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				return new CustomSerializedElementInfo(null, "ValueTypeDetachedError", null, CustomSerializedElementWriteStyle.Element, null);
			}
		}
		CustomSerializedElementInfo ICustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return this.CustomSerializedElementInfo;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ValueConstraintHasValueTypeDetachedError.ValueConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ValueConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ValueConstraintValueTypeDetachedError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ValueConstraintHasValueTypeDetachedError.ValueConstraintDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ValueConstraint", match);
				ValueConstraintValueTypeDetachedError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // ValueConstraintValueTypeDetachedError serialization
	#region RingConstraintTypeNotSpecifiedError serialization
	partial class RingConstraintTypeNotSpecifiedError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == RingConstraintHasRingConstraintTypeNotSpecifiedError.RingConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "RingConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = RingConstraintTypeNotSpecifiedError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(RingConstraintHasRingConstraintTypeNotSpecifiedError.RingConstraintDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|RingConstraint", match);
				RingConstraintTypeNotSpecifiedError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // RingConstraintTypeNotSpecifiedError serialization
	#region CompatibleValueTypeInstanceValueError serialization
	partial class CompatibleValueTypeInstanceValueError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ValueTypeInstanceHasCompatibleValueTypeInstanceValueError.ValueTypeInstanceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ValueTypeInstance", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = CompatibleValueTypeInstanceValueError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ValueTypeInstanceHasCompatibleValueTypeInstanceValueError.ValueTypeInstanceDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ValueTypeInstance", match);
				CompatibleValueTypeInstanceValueError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // CompatibleValueTypeInstanceValueError serialization
	#region TooFewEntityTypeRoleInstancesError serialization
	partial class TooFewEntityTypeRoleInstancesError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == EntityTypeInstanceHasTooFewEntityTypeRoleInstancesError.EntityTypeInstanceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "EntityTypeInstance", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = TooFewEntityTypeRoleInstancesError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(EntityTypeInstanceHasTooFewEntityTypeRoleInstancesError.EntityTypeInstanceDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|EntityTypeInstance", match);
				TooFewEntityTypeRoleInstancesError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // TooFewEntityTypeRoleInstancesError serialization
	#region TooFewFactTypeRoleInstancesError serialization
	partial class TooFewFactTypeRoleInstancesError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == FactTypeInstanceHasTooFewFactTypeRoleInstancesError.FactTypeInstanceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "FactTypeInstance", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = TooFewFactTypeRoleInstancesError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(FactTypeInstanceHasTooFewFactTypeRoleInstancesError.FactTypeInstanceDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|FactTypeInstance", match);
				TooFewFactTypeRoleInstancesError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // TooFewFactTypeRoleInstancesError serialization
	#region PopulationMandatoryError serialization
	partial class PopulationMandatoryError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ObjectTypeInstanceHasPopulationMandatoryError.ObjectTypeInstanceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ObjectTypeInstance", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == MandatoryConstraintHasPopulationMandatoryError.MandatoryConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "MandatoryConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = PopulationMandatoryError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ObjectTypeInstanceHasPopulationMandatoryError.ObjectTypeInstanceDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ObjectTypeInstance", match);
				match.InitializeRoles(MandatoryConstraintHasPopulationMandatoryError.MandatoryConstraintDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|MandatoryConstraint", match);
				PopulationMandatoryError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // PopulationMandatoryError serialization
	#region PopulationUniquenessError serialization
	partial class PopulationUniquenessError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
			CustomSerializedContainerElementInfo[] ret = PopulationUniquenessError.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "RoleInstances", null, CustomSerializedElementWriteStyle.Element, null, FactTypeRoleInstanceHasPopulationUniquenessError.RoleInstanceDomainRoleId, EntityTypeRoleInstanceHasPopulationUniquenessError.RoleInstanceDomainRoleId);
				PopulationUniquenessError.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == FactTypeRoleInstanceHasPopulationUniquenessError.RoleInstanceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "FactTypeRoleInstance", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == EntityTypeRoleInstanceHasPopulationUniquenessError.RoleInstanceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "EntityTypeRoleInstance", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = PopulationUniquenessError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(FactTypeRoleInstanceHasPopulationUniquenessError.RoleInstanceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|RoleInstances||FactTypeRoleInstance", match);
				match.InitializeRoles(EntityTypeRoleInstanceHasPopulationUniquenessError.RoleInstanceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|RoleInstances||EntityTypeRoleInstance", match);
				PopulationUniquenessError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // PopulationUniquenessError serialization
	#region TooFewReadingRolesError serialization
	partial class TooFewReadingRolesError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ReadingHasTooFewRolesError.ReadingDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Reading", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = TooFewReadingRolesError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ReadingHasTooFewRolesError.ReadingDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Reading", match);
				TooFewReadingRolesError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // TooFewReadingRolesError serialization
	#region ReadingRequiresUserModificationError serialization
	partial class ReadingRequiresUserModificationError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ReadingHasReadingRequiresUserModificationError.ReadingDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Reading", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ReadingRequiresUserModificationError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ReadingHasReadingRequiresUserModificationError.ReadingDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Reading", match);
				ReadingRequiresUserModificationError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // ReadingRequiresUserModificationError serialization
	#region TooFewRoleSequencesError serialization
	partial class TooFewRoleSequencesError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == SetComparisonConstraintHasTooFewRoleSequencesError.SetComparisonConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "SetComparisonConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == SetConstraintHasTooFewRoleSequencesError.SetConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "SetConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = TooFewRoleSequencesError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(SetComparisonConstraintHasTooFewRoleSequencesError.SetComparisonConstraintDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|SetComparisonConstraint", match);
				match.InitializeRoles(SetConstraintHasTooFewRoleSequencesError.SetConstraintDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|SetConstraint", match);
				TooFewRoleSequencesError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // TooFewRoleSequencesError serialization
	#region TooManyReadingRolesError serialization
	partial class TooManyReadingRolesError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ReadingHasTooManyRolesError.ReadingDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Reading", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = TooManyReadingRolesError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ReadingHasTooManyRolesError.ReadingDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Reading", match);
				TooManyReadingRolesError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // TooManyReadingRolesError serialization
	#region TooManyRoleSequencesError serialization
	partial class TooManyRoleSequencesError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == SetComparisonConstraintHasTooManyRoleSequencesError.SetComparisonConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "SetComparisonConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == SetConstraintHasTooManyRoleSequencesError.SetConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "SetConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = TooManyRoleSequencesError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(SetComparisonConstraintHasTooManyRoleSequencesError.SetComparisonConstraintDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|SetComparisonConstraint", match);
				match.InitializeRoles(SetConstraintHasTooManyRoleSequencesError.SetConstraintDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|SetConstraint", match);
				TooManyRoleSequencesError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // TooManyRoleSequencesError serialization
	#region DataTypeNotSpecifiedError serialization
	partial class DataTypeNotSpecifiedError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ValueTypeHasUnspecifiedDataTypeError.ValueTypeHasDataTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ConceptualDataType", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = DataTypeNotSpecifiedError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ValueTypeHasUnspecifiedDataTypeError.ValueTypeHasDataTypeDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ConceptualDataType", match);
				DataTypeNotSpecifiedError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // DataTypeNotSpecifiedError serialization
	#region EqualityImpliedByMandatoryError serialization
	partial class EqualityImpliedByMandatoryError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == EqualityConstraintHasEqualityImpliedByMandatoryError.EqualityConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "EqualityConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = EqualityImpliedByMandatoryError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(EqualityConstraintHasEqualityImpliedByMandatoryError.EqualityConstraintDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|EqualityConstraint", match);
				EqualityImpliedByMandatoryError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // EqualityImpliedByMandatoryError serialization
	#region NMinusOneError serialization
	partial class NMinusOneError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == UniquenessConstraintHasNMinusOneError.ConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "UniquenessConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = NMinusOneError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(UniquenessConstraintHasNMinusOneError.ConstraintDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|UniquenessConstraint", match);
				NMinusOneError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // NMinusOneError serialization
	#region ImplicationError serialization
	partial class ImplicationError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == SetComparisonConstraintHasImplicationError.SetComparisonConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "SetComparisonConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == SetConstraintHasImplicationError.SetConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "SetConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ImplicationError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(SetComparisonConstraintHasImplicationError.SetComparisonConstraintDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|SetComparisonConstraint", match);
				match.InitializeRoles(SetConstraintHasImplicationError.SetConstraintDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|SetConstraint", match);
				ImplicationError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // ImplicationError serialization
	#region ContradictionError serialization
	partial class ContradictionError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
			CustomSerializedContainerElementInfo[] ret = ContradictionError.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "Constraints", null, CustomSerializedElementWriteStyle.Element, null, SetComparisonConstraintHasContradictionError.SetComparisonConstraintDomainRoleId, SetComparisonConstraintHasContradictionError.SetComparisonConstraintDomainRoleId);
				ContradictionError.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == SetComparisonConstraintHasContradictionError.SetComparisonConstraintDomainRoleId)
			{
				Guid elementLinkDomainId = elementLink.GetDomainRelationship().Id;
				if (elementLinkDomainId == SetComparisonConstraintHasExclusionContradictsSubsetError.DomainClassId)
				{
					string name = "SubsetConstraint";
					if (((SetComparisonConstraintHasContradictionError)elementLink).SetComparisonConstraint is ExclusionConstraint)
					{
						name = "ExclusionConstraintThatContradictsWithSubset";
					}
					return new CustomSerializedElementInfo(null, name, null, CustomSerializedElementWriteStyle.Element, null);
				}
				else if (elementLinkDomainId == SetComparisonConstraintHasExclusionContradictsEqualityError.DomainClassId)
				{
					string name = "EqualityConstraint";
					if (((SetComparisonConstraintHasContradictionError)elementLink).SetComparisonConstraint is ExclusionConstraint)
					{
						name = "ExclusionConstraintThatContradictsWithEquality";
					}
					return new CustomSerializedElementInfo(null, name, null, CustomSerializedElementWriteStyle.Element, null);
				}
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ContradictionError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRolesWithExplicitRelationship(SetComparisonConstraintHasExclusionContradictsSubsetError.DomainClassId, SetComparisonConstraintHasContradictionError.SetComparisonConstraintDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Constraints||SubsetConstraint", match);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Constraints||ExclusionConstraintThatContradictsWithSubset", match);
				match.InitializeRolesWithExplicitRelationship(SetComparisonConstraintHasExclusionContradictsEqualityError.DomainClassId, SetComparisonConstraintHasContradictionError.SetComparisonConstraintDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Constraints||EqualityConstraint", match);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Constraints||ExclusionConstraintThatContradictsWithEquality", match);
				ContradictionError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // ContradictionError serialization
	#region ExclusionContradictsEqualityError serialization
	partial class ExclusionContradictsEqualityError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == SetComparisonConstraintHasExclusionContradictsEqualityError.SetComparisonConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
	}
	#endregion // ExclusionContradictsEqualityError serialization
	#region ExclusionContradictsSubsetError serialization
	partial class ExclusionContradictsSubsetError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == SetComparisonConstraintHasExclusionContradictsSubsetError.SetComparisonConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
	}
	#endregion // ExclusionContradictsSubsetError serialization
	#region ExclusionContradictsMandatoryError serialization
	partial class ExclusionContradictsMandatoryError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
			CustomSerializedContainerElementInfo[] ret = ExclusionContradictsMandatoryError.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "Constraints", null, CustomSerializedElementWriteStyle.Element, null, MandatoryConstraintHasExclusionContradictsMandatoryError.MandatoryConstraintDomainRoleId, ExclusionConstraintHasExclusionContradictsMandatoryError.ExclusionConstraintDomainRoleId);
				ExclusionContradictsMandatoryError.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == MandatoryConstraintHasExclusionContradictsMandatoryError.MandatoryConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "MandatoryConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == ExclusionConstraintHasExclusionContradictsMandatoryError.ExclusionConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ExclusionConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ExclusionContradictsMandatoryError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(MandatoryConstraintHasExclusionContradictsMandatoryError.MandatoryConstraintDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Constraints||MandatoryConstraint", match);
				match.InitializeRoles(ExclusionConstraintHasExclusionContradictsMandatoryError.ExclusionConstraintDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Constraints||ExclusionConstraint", match);
				ExclusionContradictsMandatoryError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // ExclusionContradictsMandatoryError serialization
	#region NotWellModeledSubsetAndMandatoryError serialization
	partial class NotWellModeledSubsetAndMandatoryError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
			CustomSerializedContainerElementInfo[] ret = NotWellModeledSubsetAndMandatoryError.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "Constraints", null, CustomSerializedElementWriteStyle.Element, null, MandatoryConstraintHasNotWellModeledSubsetAndMandatoryError.MandatoryConstraintDomainRoleId, SubsetConstraintHasNotWellModeledSubsetAndMandatoryError.SubsetConstraintDomainRoleId);
				NotWellModeledSubsetAndMandatoryError.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			return this.GetCustomSerializedChildElementInfo();
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == SubsetConstraintHasNotWellModeledSubsetAndMandatoryError.SubsetConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "SubsetConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == MandatoryConstraintHasNotWellModeledSubsetAndMandatoryError.MandatoryConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "MandatoryConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = NotWellModeledSubsetAndMandatoryError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(MandatoryConstraintHasNotWellModeledSubsetAndMandatoryError.MandatoryConstraintDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Constraints||MandatoryConstraint", match);
				match.InitializeRoles(SubsetConstraintHasNotWellModeledSubsetAndMandatoryError.SubsetConstraintDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Constraints||SubsetConstraint", match);
				NotWellModeledSubsetAndMandatoryError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // NotWellModeledSubsetAndMandatoryError serialization
	#region PreferredIdentifierRequiresMandatoryError serialization
	partial class PreferredIdentifierRequiresMandatoryError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ObjectTypeHasPreferredIdentifierRequiresMandatoryError.ObjectTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ObjectType", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = PreferredIdentifierRequiresMandatoryError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ObjectTypeHasPreferredIdentifierRequiresMandatoryError.ObjectTypeDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ObjectType", match);
				PreferredIdentifierRequiresMandatoryError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // PreferredIdentifierRequiresMandatoryError serialization
	#region CompatibleSupertypesError serialization
	partial class CompatibleSupertypesError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ObjectTypeHasCompatibleSupertypesError.ObjectTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ObjectType", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = CompatibleSupertypesError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ObjectTypeHasCompatibleSupertypesError.ObjectTypeDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ObjectType", match);
				CompatibleSupertypesError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // CompatibleSupertypesError serialization
	#region CompatibleRolePlayerTypeError serialization
	partial class CompatibleRolePlayerTypeError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		protected new CustomSerializedPropertyInfo GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			if (domainPropertyInfo.Id == CompatibleRolePlayerTypeError.ColumnDomainPropertyId)
			{
				if (this.SetConstraint != null)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.PropertyInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
			}
			return CustomSerializedPropertyInfo.Default;
		}
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			return this.GetCustomSerializedPropertyInfo(domainPropertyInfo, rolePlayedInfo);
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == SetComparisonConstraintHasCompatibleRolePlayerTypeError.SetComparisonConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "SetComparisonConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == SetConstraintHasCompatibleRolePlayerTypeError.SetConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "SetConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = CompatibleRolePlayerTypeError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(SetComparisonConstraintHasCompatibleRolePlayerTypeError.SetComparisonConstraintDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|SetComparisonConstraint", match);
				match.InitializeRoles(SetConstraintHasCompatibleRolePlayerTypeError.SetConstraintDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|SetConstraint", match);
				CompatibleRolePlayerTypeError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
			{
				rVal = base.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
			}
			return rVal;
		}
		CustomSerializedElementMatch ICustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			return this.MapChildElement(elementNamespace, elementName, containerNamespace, containerName, outerContainerNamespace, outerContainerName);
		}
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = CompatibleRolePlayerTypeError.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("Column", CompatibleRolePlayerTypeError.ColumnDomainPropertyId);
				CompatibleRolePlayerTypeError.myCustomSerializedAttributes = customSerializedAttributes;
			}
			Guid rVal;
			string key = attributeName;
			if (xmlNamespace.Length != 0)
			{
				key = string.Concat(xmlNamespace, "|", attributeName);
			}
			if (!customSerializedAttributes.TryGetValue(key, out rVal))
			{
				rVal = base.MapAttribute(xmlNamespace, attributeName);
			}
			return rVal;
		}
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return this.MapAttribute(xmlNamespace, attributeName);
		}
	}
	#endregion // CompatibleRolePlayerTypeError serialization
	#region RolePlayerRequiredError serialization
	partial class RolePlayerRequiredError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == RoleHasRolePlayerRequiredError.RoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Role", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = RolePlayerRequiredError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(RoleHasRolePlayerRequiredError.RoleDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Role", match);
				RolePlayerRequiredError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // RolePlayerRequiredError serialization
	#region FrequencyConstraintContradictsInternalUniquenessConstraintError serialization
	partial class FrequencyConstraintContradictsInternalUniquenessConstraintError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == FactTypeHasFrequencyConstraintContradictsInternalUniquenessConstraintError.FactTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Fact", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == FrequencyConstraintHasFrequencyConstraintInvalidatedByInternalUniquenessConstraintError.FrequencyConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "FrequencyConstraint", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = FrequencyConstraintContradictsInternalUniquenessConstraintError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(FactTypeHasFrequencyConstraintContradictsInternalUniquenessConstraintError.FactTypeDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Fact", match);
				match.InitializeRoles(FrequencyConstraintHasFrequencyConstraintInvalidatedByInternalUniquenessConstraintError.FrequencyConstraintDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|FrequencyConstraint", match);
				FrequencyConstraintContradictsInternalUniquenessConstraintError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // FrequencyConstraintContradictsInternalUniquenessConstraintError serialization
	#region EqualityOrSubsetImpliedByMandatoryError serialization
	partial class EqualityOrSubsetImpliedByMandatoryError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.LinkInfo;
			}
		}
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return this.SupportedCustomSerializedOperations;
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		protected new CustomSerializedElementInfo CustomSerializedElementInfo
		{
			get
			{
				string name = "EqualityConstraintImpliedByMandatoryConstraintsError";
				if (this.EqualityOrSubsetConstraint is SubsetConstraint)
				{
					name = "SubsetConstraintImpliedByMandatoryConstraintsError";
				}
				return new CustomSerializedElementInfo(null, name, null, CustomSerializedElementWriteStyle.Element, null);
			}
		}
		CustomSerializedElementInfo ICustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return this.CustomSerializedElementInfo;
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == SetComparisonConstraintHasEqualityOrSubsetImpliedByMandatoryError.SetComparisonConstraintDomainRoleId)
			{
				string name = "EqualityConstraint";
				if (((SetComparisonConstraintHasEqualityOrSubsetImpliedByMandatoryError)elementLink).SetComparisonConstraint is SubsetConstraint)
				{
					name = "SubsetConstraint";
				}
				return new CustomSerializedElementInfo(null, name, null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == SetComparisonConstraintHasImplicationError.SetComparisonConstraintDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (0 != (CustomSerializedElementSupportedOperations.LinkInfo & base.SupportedCustomSerializedOperations))
			{
				return base.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
			}
			return CustomSerializedElementInfo.Default;
		}
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			return this.GetCustomSerializedLinkInfo(rolePlayedInfo, elementLink);
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = EqualityOrSubsetImpliedByMandatoryError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(SetComparisonConstraintHasEqualityOrSubsetImpliedByMandatoryError.SetComparisonConstraintDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|EqualityConstraint", match);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|SubsetConstraint", match);
				EqualityOrSubsetImpliedByMandatoryError.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			if (!childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal))
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
	#endregion // EqualityOrSubsetImpliedByMandatoryError serialization
}
