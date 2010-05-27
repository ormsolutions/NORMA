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
// * Copyright © ORM Solutions, LLC. All rights reserved.                     *
// *                                                                          *
// * The use and distribution terms for this software are covered by the      *
// * Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *
// * can be found in the file CPL.txt at the root of this distribution.       *
// * By using this software in any fashion, you are agreeing to be bound by   *
// * the terms of this license.                                               *
// *                                                                          *
// * You must not remove this notice, or any other, from this software.       *
// \**************************************************************************/

namespace ORMSolutions.ORMArchitect.Core.ObjectModel
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
			retVal[dataDir.FindDomainRelationship(ModelHasModelErrorDisplayFilter.DomainClassId)] = null;
			retVal[dataDir.FindDomainRelationship(ElementAssociatedWithModelError.DomainClassId)] = null;
			retVal[dataDir.FindDomainRelationship(ElementHasAlias.DomainClassId)] = null;
			retVal[dataDir.FindDomainRelationship(ObjectTypeHasObjectTypeInstance.DomainClassId)] = null;
			retVal[dataDir.FindDomainRelationship(RolePathOwnerHasSingleLeadRolePath.DomainClassId)] = null;
			retVal[dataDir.FindDomainRelationship(RolePathOwnerHasSingleOwnedLeadRolePath.DomainClassId)] = null;
			retVal[dataDir.FindDomainRelationship(RolePathOwnerHasLeadRolePath.DomainClassId)] = null;
			retVal[dataDir.FindDomainClass(IntrinsicReferenceMode.DomainClassId)] = null;
			retVal[dataDir.FindDomainRelationship(GroupingElementRelationship.DomainClassId)] = null;
			retVal[dataDir.FindDomainRelationship(ElementGroupingContainsElementGrouping.DomainClassId)] = null;
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
				GenerationState.DomainClassId,
				ElementGroupingSet.DomainClassId};
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
			if (elementName == "Grouping" && xmlNamespace == "http://schemas.neumont.edu/ORM/2006-04/ORMCore")
			{
				return ElementGroupingSet.DomainClassId;
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
				classNameMap.Add("EntityTypeSubtypeInstance", EntityTypeSubtypeInstance.DomainClassId);
				classNameMap.Add("EntityTypeRoleInstance", EntityTypeRoleInstance.DomainClassId);
				classNameMap.Add("CustomReferenceMode", CustomReferenceMode.DomainClassId);
				classNameMap.Add("DataType", DataType.DomainClassId);
				classNameMap.Add("ReferenceModeKind", ReferenceModeKind.DomainClassId);
				classNameMap.Add("BaseValueConstraint", ValueConstraint.DomainClassId);
				classNameMap.Add("ValueConstraint", ValueTypeValueConstraint.DomainClassId);
				classNameMap.Add("RoleValueConstraint", RoleValueConstraint.DomainClassId);
				classNameMap.Add("PathedRoleConditionValueConstraint", PathConditionRoleValueConstraint.DomainClassId);
				classNameMap.Add("ValueRange", ValueRange.DomainClassId);
				classNameMap.Add("Fact", FactType.DomainClassId);
				classNameMap.Add("ImpliedFact", FactType.DomainClassId);
				classNameMap.Add("FactTypeInstance", FactTypeInstance.DomainClassId);
				classNameMap.Add("FactTypeRoleInstance", FactTypeRoleInstance.DomainClassId);
				classNameMap.Add("Expression", Expression.DomainClassId);
				classNameMap.Add("DerivationExpression", FactTypeDerivationExpression.DomainClassId);
				classNameMap.Add("FactTypeDerivationPath", FactTypeDerivationRule.DomainClassId);
				classNameMap.Add("FactTypeDerivationProjection", FactTypeDerivationProjection.DomainClassId);
				classNameMap.Add("FactTypeRoleProjection", FactTypeRoleProjection.DomainClassId);
				classNameMap.Add("SubtypeDerivationPath", SubtypeDerivationRule.DomainClassId);
				classNameMap.Add("SubtypeFact", SubtypeFact.DomainClassId);
				classNameMap.Add("ReadingOrder", ReadingOrder.DomainClassId);
				classNameMap.Add("Reading", Reading.DomainClassId);
				classNameMap.Add("Role", Role.DomainClassId);
				classNameMap.Add("RoleProxy", RoleProxy.DomainClassId);
				classNameMap.Add("ObjectifiedUnaryRole", ObjectifiedUnaryRole.DomainClassId);
				classNameMap.Add("SetComparisonConstraint", SetComparisonConstraint.DomainClassId);
				classNameMap.Add("RoleSequence", SetComparisonConstraintRoleSequence.DomainClassId);
				classNameMap.Add("SetConstraint", SetConstraint.DomainClassId);
				classNameMap.Add("JoinPath", ConstraintRoleSequenceJoinPath.DomainClassId);
				classNameMap.Add("ConstraintRoleSequenceJoinPathProjection", ConstraintRoleSequenceJoinPathProjection.DomainClassId);
				classNameMap.Add("ConstraintRoleProjection", ConstraintRoleProjection.DomainClassId);
				classNameMap.Add("MandatoryConstraint", MandatoryConstraint.DomainClassId);
				classNameMap.Add("FrequencyConstraint", FrequencyConstraint.DomainClassId);
				classNameMap.Add("UniquenessConstraint", UniquenessConstraint.DomainClassId);
				classNameMap.Add("EqualityConstraint", EqualityConstraint.DomainClassId);
				classNameMap.Add("ExclusionConstraint", ExclusionConstraint.DomainClassId);
				classNameMap.Add("SubsetConstraint", SubsetConstraint.DomainClassId);
				classNameMap.Add("RingConstraint", RingConstraint.DomainClassId);
				classNameMap.Add("Function", Function.DomainClassId);
				classNameMap.Add("Parameter", FunctionParameter.DomainClassId);
				classNameMap.Add("RolePathOwner", RolePathOwner.DomainClassId);
				classNameMap.Add("RolePath", LeadRolePath.DomainClassId);
				classNameMap.Add("CalculatedValue", CalculatedPathValue.DomainClassId);
				classNameMap.Add("Input", CalculatedPathValueInput.DomainClassId);
				classNameMap.Add("Constant", PathConstant.DomainClassId);
				classNameMap.Add("RootObjectType", RolePathObjectTypeRoot.DomainClassId);
				classNameMap.Add("SubPath", RoleSubPath.DomainClassId);
				classNameMap.Add("PathedRole", PathedRole.DomainClassId);
				classNameMap.Add("ObjectUnifier", PathObjectUnifier.DomainClassId);
				classNameMap.Add("ConstraintDuplicateNameError", ConstraintDuplicateNameError.DomainClassId);
				classNameMap.Add("ObjectTypeDuplicateNameError", ObjectTypeDuplicateNameError.DomainClassId);
				classNameMap.Add("RecognizedPhraseDuplicateNameError", RecognizedPhraseDuplicateNameError.DomainClassId);
				classNameMap.Add("FunctionDuplicateNameError", FunctionDuplicateNameError.DomainClassId);
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
				classNameMap.Add("ObjectifiedInstanceRequiredError", ObjectifiedInstanceRequiredError.DomainClassId);
				classNameMap.Add("ObjectifyingInstanceRequiredError", ObjectifyingInstanceRequiredError.DomainClassId);
				classNameMap.Add("PopulationUniquenessError", PopulationUniquenessError.DomainClassId);
				classNameMap.Add("TooFewReadingRolesError", TooFewReadingRolesError.DomainClassId);
				classNameMap.Add("ReadingRequiresUserModificationError", ReadingRequiresUserModificationError.DomainClassId);
				classNameMap.Add("TooFewRoleSequencesError", TooFewRoleSequencesError.DomainClassId);
				classNameMap.Add("TooManyReadingRolesError", TooManyReadingRolesError.DomainClassId);
				classNameMap.Add("TooManyRoleSequencesError", TooManyRoleSequencesError.DomainClassId);
				classNameMap.Add("DataTypeNotSpecifiedError", DataTypeNotSpecifiedError.DomainClassId);
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
				classNameMap.Add("JoinPathRequiredError", JoinPathRequiredError.DomainClassId);
				classNameMap.Add("RolePlayerRequiredError", RolePlayerRequiredError.DomainClassId);
				classNameMap.Add("FrequencyConstraintViolatedByUniquenessConstraintError", FrequencyConstraintViolatedByUniquenessConstraintError.DomainClassId);
				classNameMap.Add("EqualityConstraintImpliedByMandatoryConstraintsError", EqualityOrSubsetImpliedByMandatoryError.DomainClassId);
				classNameMap.Add("SubsetConstraintImpliedByMandatoryConstraintsError", EqualityOrSubsetImpliedByMandatoryError.DomainClassId);
				classNameMap.Add("PathRequiresRootObjectTypeError", PathRequiresRootObjectTypeError.DomainClassId);
				classNameMap.Add("JoinedPathRoleRequiresCompatibleRolePlayerError", JoinedPathRoleRequiresCompatibleRolePlayerError.DomainClassId);
				classNameMap.Add("ObjectUnifierRequiresCompatibleObjectTypesError", PathObjectUnifierRequiresCompatibleObjectTypesError.DomainClassId);
				classNameMap.Add("PathSameFactTypeRoleFollowsJoinError", PathSameFactTypeRoleFollowsJoinError.DomainClassId);
				classNameMap.Add("PathOuterJoinRequiresOptionalRoleError", PathOuterJoinRequiresOptionalRoleError.DomainClassId);
				classNameMap.Add("CalculatedPathValueRequiresFunctionError", CalculatedPathValueRequiresFunctionError.DomainClassId);
				classNameMap.Add("CalculatedPathValueMustBeConsumedError", CalculatedPathValueMustBeConsumedError.DomainClassId);
				classNameMap.Add("CalculatedPathValueParameterBindingError", CalculatedPathValueParameterBindingError.DomainClassId);
				classNameMap.Add("CalculatedPathValueHasUnboundParameterError", CalculatedPathValueHasUnboundParameterError.DomainClassId);
				classNameMap.Add("FactTypeDerivationRequiresProjectionError", FactTypeDerivationRequiresProjectionError.DomainClassId);
				classNameMap.Add("PartialFactTypeDerivationProjectionError", PartialFactTypeDerivationProjectionError.DomainClassId);
				classNameMap.Add("JoinPathRequiresProjectionError", ConstraintRoleSequenceJoinPathRequiresProjectionError.DomainClassId);
				classNameMap.Add("PartialJoinPathProjectionError", PartialConstraintRoleSequenceJoinPathProjectionError.DomainClassId);
				classNameMap.Add("Grouping", ElementGroupingSet.DomainClassId);
				classNameMap.Add("Group", ElementGrouping.DomainClassId);
				classNameMap.Add("GroupDuplicateNameError", ElementGroupingDuplicateNameError.DomainClassId);
				classNameMap.Add("GroupMembershipContradictionError", ElementGroupingMembershipContradictionError.DomainClassId);
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
			CustomSerializedContainerElementInfo[] ret = ORMModelElement.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new CustomSerializedContainerElementInfo[1];
				ret[0] = new CustomSerializedContainerElementInfo("orm", "Extensions", null, CustomSerializedElementWriteStyle.Element, null, ORMModelElementHasExtensionElement.ExtensionDomainRoleId);
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
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == GroupingMembershipContradictionErrorIsForElement.GroupingMembershipContradictionErrorRelationshipDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == GroupingElementInclusion.GroupingDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == GroupingElementExclusion.GroupingDomainRoleId)
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
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
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
				ret = new CustomSerializedContainerElementInfo[baseInfoCount + 12];
				if (baseInfoCount != 0)
				{
					baseInfo.CopyTo(ret, 12);
				}
				ret[0] = new CustomSerializedContainerElementInfo(null, "Definitions", null, CustomSerializedElementWriteStyle.Element, null, ModelHasDefinition.DefinitionDomainRoleId);
				ret[1] = new CustomSerializedContainerElementInfo(null, "Notes", null, CustomSerializedElementWriteStyle.Element, null, ModelHasPrimaryNote.NoteDomainRoleId);
				ret[2] = new CustomSerializedContainerElementInfo(null, "Objects", null, CustomSerializedElementWriteStyle.Element, null, ModelHasObjectType.ObjectTypeDomainRoleId);
				ret[3] = new CustomSerializedContainerElementInfo(null, "Facts", null, CustomSerializedElementWriteStyle.Element, null, ModelHasFactType.FactTypeDomainRoleId);
				ret[4] = new CustomSerializedContainerElementInfo(null, "Constraints", null, CustomSerializedElementWriteStyle.Element, null, ModelHasSetConstraint.SetConstraintDomainRoleId, ModelHasSetComparisonConstraint.SetComparisonConstraintDomainRoleId);
				ret[5] = new CustomSerializedContainerElementInfo(null, "DataTypes", null, CustomSerializedElementWriteStyle.Element, null, ModelHasDataType.DataTypeDomainRoleId);
				ret[6] = new CustomSerializedContainerElementInfo(null, "Functions", null, CustomSerializedElementWriteStyle.Element, null, ModelDefinesFunction.FunctionDomainRoleId);
				ret[7] = new CustomSerializedContainerElementInfo(null, "CustomReferenceModes", null, CustomSerializedElementWriteStyle.Element, null, ModelHasReferenceMode.ReferenceModeDomainRoleId);
				ret[8] = new CustomSerializedContainerElementInfo(null, "ModelNotes", null, CustomSerializedElementWriteStyle.Element, null, ModelHasModelNote.NoteDomainRoleId);
				ret[9] = new CustomSerializedContainerElementInfo(null, "ModelErrors", null, CustomSerializedElementWriteStyle.Element, null, ModelHasError.ErrorDomainRoleId);
				ret[10] = new CustomSerializedContainerElementInfo(null, "ReferenceModeKinds", null, CustomSerializedElementWriteStyle.Element, null, ModelHasReferenceModeKind.ReferenceModeKindDomainRoleId);
				ret[11] = new CustomSerializedContainerElementInfo(null, "RecognizedPhrases", null, CustomSerializedElementWriteStyle.Element, null, ModelContainsRecognizedPhrase.RecognizedPhraseDomainRoleId);
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
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		protected new CustomSerializedElementInfo GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ElementGroupingSetRelatesToORMModel.GroupingSetDomainRoleId)
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
				domainRole = domainDataDirectory.FindDomainRole(ModelHasDefinition.DefinitionDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(ModelHasPrimaryNote.NoteDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(ModelHasObjectType.ObjectTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(ModelHasFactType.FactTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 3;
				domainRole = domainDataDirectory.FindDomainRole(ModelHasSetConstraint.SetConstraintDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 4;
				domainRole = domainDataDirectory.FindDomainRole(ModelHasSetComparisonConstraint.SetComparisonConstraintDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 5;
				domainRole = domainDataDirectory.FindDomainRole(ModelHasDataType.DataTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 6;
				domainRole = domainDataDirectory.FindDomainRole(ModelDefinesFunction.FunctionDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 7;
				domainRole = domainDataDirectory.FindDomainRole(ModelHasReferenceMode.ReferenceModeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 8;
				domainRole = domainDataDirectory.FindDomainRole(ModelHasModelNote.NoteDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 9;
				domainRole = domainDataDirectory.FindDomainRole(ModelHasError.ErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 10;
				domainRole = domainDataDirectory.FindDomainRole(ModelHasReferenceModeKind.ReferenceModeKindDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 11;
				domainRole = domainDataDirectory.FindDomainRole(ModelContainsRecognizedPhrase.RecognizedPhraseDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 12;
				domainRole = domainDataDirectory.FindDomainRole(ElementGroupingSetRelatesToORMModel.GroupingSetDomainRoleId).OppositeDomainRole;
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
				match.InitializeRoles(ModelHasDefinition.DefinitionDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Definitions||", match);
				match.InitializeRoles(ModelHasPrimaryNote.NoteDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Notes||", match);
				match.InitializeRoles(ModelHasObjectType.ObjectTypeDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Objects||", match);
				match.InitializeRoles(ModelHasFactType.FactTypeDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Facts||", match);
				match.InitializeRoles(ModelHasSetConstraint.SetConstraintDomainRoleId, ModelHasSetComparisonConstraint.SetComparisonConstraintDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Constraints||", match);
				match.InitializeRoles(ModelHasDataType.DataTypeDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|DataTypes||", match);
				match.InitializeRoles(ModelDefinesFunction.FunctionDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Functions||", match);
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
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
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
			CustomSerializedContainerElementInfo[] ret = NameGenerator.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo("orm", "Refinements", null, CustomSerializedElementWriteStyle.Element, null, NameGeneratorRefinesNameGenerator.RefinementDomainRoleId);
				NameGenerator.myCustomSerializedChildElementInfo = ret;
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
				domainRole = domainDataDirectory.FindDomainRole(NameGeneratorRefinesNameGenerator.RefinementDomainRoleId).OppositeDomainRole;
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
					xPos = int.MinValue;
				}
				int yPos;
				if (!this.myRoleOrderDictionary.TryGetValue(string.Concat(y.DomainRelationship.ImplementationClass.FullName, ".", y.Name), out yPos))
				{
					yPos = int.MinValue;
				}
				return xPos.CompareTo(yPos);
			}
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedChildRoleComparer</summary>
		protected new IComparer<DomainRoleInfo> CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<DomainRoleInfo> retVal = NameGenerator.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
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
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
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
		/// <summary>Implements ICustomSerializedElement.ShouldSerialize</summary>
		protected new bool ShouldSerialize()
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
				ret = new CustomSerializedContainerElementInfo[baseInfoCount + 7];
				if (baseInfoCount != 0)
				{
					baseInfo.CopyTo(ret, 7);
				}
				ret[0] = new CustomSerializedContainerElementInfo(null, "Definitions", null, CustomSerializedElementWriteStyle.Element, null, ObjectTypeHasDefinition.DefinitionDomainRoleId);
				ret[1] = new CustomSerializedContainerElementInfo(null, "Notes", null, CustomSerializedElementWriteStyle.Element, null, ObjectTypeHasNote.NoteDomainRoleId);
				ret[2] = new CustomSerializedContainerElementInfo(null, "Abbreviations", null, CustomSerializedElementWriteStyle.Element, null, ObjectTypeHasAbbreviation.AbbreviationDomainRoleId);
				ret[3] = new CustomSerializedContainerElementInfo(null, "PlayedRoles", null, CustomSerializedElementWriteStyle.Element, null, ObjectTypePlaysRole.PlayedRoleDomainRoleId);
				ret[4] = new CustomSerializedContainerElementInfo(null, "SubtypeDerivationRule", null, CustomSerializedElementWriteStyle.Element, null, SubtypeHasDerivationExpression.DerivationRuleDomainRoleId, SubtypeHasDerivationRule.DerivationRuleDomainRoleId);
				ret[5] = new CustomSerializedContainerElementInfo(null, "ValueRestriction", null, CustomSerializedElementWriteStyle.Element, null, ValueTypeHasValueConstraint.ValueConstraintDomainRoleId);
				ret[6] = new CustomSerializedContainerElementInfo(null, "Instances", null, CustomSerializedElementWriteStyle.Element, null, ValueTypeHasValueTypeInstance.ValueTypeInstanceDomainRoleId, EntityTypeHasEntityTypeInstance.EntityTypeInstanceDomainRoleId, EntityTypeSubtypeHasEntityTypeSubtypeInstance.EntityTypeSubtypeInstanceDomainRoleId);
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
			if (roleId == RolePathObjectTypeRoot.RolePathDomainRoleId)
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
				domainRole = domainDataDirectory.FindDomainRole(SubtypeHasDerivationExpression.DerivationRuleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 4;
				domainRole = domainDataDirectory.FindDomainRole(SubtypeHasDerivationRule.DerivationRuleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 5;
				domainRole = domainDataDirectory.FindDomainRole(ValueTypeHasDataType.DataTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 6;
				domainRole = domainDataDirectory.FindDomainRole(ValueTypeHasValueConstraint.ValueConstraintDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 7;
				domainRole = domainDataDirectory.FindDomainRole(EntityTypeHasPreferredIdentifier.PreferredIdentifierDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 8;
				domainRole = domainDataDirectory.FindDomainRole(Objectification.NestedFactTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 9;
				domainRole = domainDataDirectory.FindDomainRole(ValueTypeHasValueTypeInstance.ValueTypeInstanceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 10;
				domainRole = domainDataDirectory.FindDomainRole(EntityTypeHasEntityTypeInstance.EntityTypeInstanceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 10;
				domainRole = domainDataDirectory.FindDomainRole(EntityTypeSubtypeHasEntityTypeSubtypeInstance.EntityTypeSubtypeInstanceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 10;
				domainRole = domainDataDirectory.FindDomainRole(ObjectTypeImpliesMandatoryConstraint.MandatoryConstraintDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 11;
				domainRole = domainDataDirectory.FindDomainRole(ObjectTypeHasEntityTypeRequiresReferenceSchemeError.ReferenceSchemeErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 12;
				domainRole = domainDataDirectory.FindDomainRole(ObjectTypeHasDuplicateNameError.DuplicateNameErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 13;
				domainRole = domainDataDirectory.FindDomainRole(ObjectTypeHasPreferredIdentifierRequiresMandatoryError.PreferredIdentifierRequiresMandatoryErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 14;
				domainRole = domainDataDirectory.FindDomainRole(ObjectTypeHasCompatibleSupertypesError.CompatibleSupertypesErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 15;
				domainRole = domainDataDirectory.FindDomainRole(ModelNoteReferencesObjectType.NoteDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 16;
				domainRole = domainDataDirectory.FindDomainRole(RolePathObjectTypeRoot.RolePathDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 17;
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
				match.InitializeRoles(SubtypeHasDerivationExpression.DerivationRuleDomainRoleId, SubtypeHasDerivationRule.DerivationRuleDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|SubtypeDerivationRule||", match);
				match.InitializeRoles(ValueTypeHasValueConstraint.ValueConstraintDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ValueRestriction||", match);
				match.InitializeRoles(ValueTypeHasValueTypeInstance.ValueTypeInstanceDomainRoleId, EntityTypeHasEntityTypeInstance.EntityTypeInstanceDomainRoleId, EntityTypeSubtypeHasEntityTypeSubtypeInstance.EntityTypeSubtypeInstanceDomainRoleId);
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
			if (domainPropertyInfo.Id == ValueTypeInstance.InvariantValueDomainPropertyId)
			{
				if (string.IsNullOrEmpty(this.InvariantValue))
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
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
				match.InitializeAttribute(ValueTypeInstance.InvariantValueDomainPropertyId, null);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|InvariantValue", match);
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
		/// <summary>Implements ICustomSerializedElement.ShouldSerialize</summary>
		protected new bool ShouldSerialize()
		{
			return !this.ValueType.IsImplicitBooleanValue || EntityTypeRoleInstance.GetLinksToRoleCollection(this).Count != 0;
		}
		bool ICustomSerializedElement.ShouldSerialize()
		{
			return this.ShouldSerialize();
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
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
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
			if (roleId == ObjectificationInstance.ObjectifiedInstanceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ObjectifiedInstance", null, CustomSerializedElementWriteStyle.Element, null);
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
			if (roleId == EntityTypeSubtypeInstanceHasSupertypeInstance.EntityTypeSubtypeInstanceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ObjectifyingInstanceHasObjectifiedInstanceRequiredError.ObjectifiedInstanceRequiredErrorDomainRoleId)
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
				domainRole = domainDataDirectory.FindDomainRole(EntityTypeInstanceHasRoleInstance.RoleInstanceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(ObjectificationInstance.ObjectifiedInstanceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(EntityTypeRoleInstance.RoleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(FactTypeRoleInstance.RoleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 3;
				domainRole = domainDataDirectory.FindDomainRole(EntityTypeInstanceHasTooFewEntityTypeRoleInstancesError.TooFewEntityTypeRoleInstancesErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 4;
				domainRole = domainDataDirectory.FindDomainRole(ObjectTypeInstanceHasPopulationMandatoryError.PopulationMandatoryErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 5;
				domainRole = domainDataDirectory.FindDomainRole(EntityTypeSubtypeInstanceHasSupertypeInstance.EntityTypeSubtypeInstanceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 6;
				domainRole = domainDataDirectory.FindDomainRole(ObjectifyingInstanceHasObjectifiedInstanceRequiredError.ObjectifiedInstanceRequiredErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 7;
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
				IComparer<DomainRoleInfo> retVal = EntityTypeInstance.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					EntityTypeInstance.myCustomSortChildComparer = retVal;
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = EntityTypeInstance.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ObjectificationInstance.ObjectifiedInstanceDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ObjectifiedInstance", match);
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
	#region EntityTypeSubtypeInstance serialization
	partial class EntityTypeSubtypeInstance : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
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
				return new CustomSerializedElementInfo(null, "EntityTypeSubtypeInstance", null, CustomSerializedElementWriteStyle.Element, null);
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
			if (roleId == EntityTypeSubtypeInstanceHasSupertypeInstance.SupertypeInstanceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "SupertypeInstance", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == ObjectificationInstance.ObjectifiedInstanceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ObjectifiedInstance", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == EntityTypeRoleInstance.RoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FactTypeRoleInstance.RoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ObjectTypeInstanceHasPopulationMandatoryError.PopulationMandatoryErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ObjectifyingInstanceHasObjectifiedInstanceRequiredError.ObjectifiedInstanceRequiredErrorDomainRoleId)
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
				domainRole = domainDataDirectory.FindDomainRole(EntityTypeSubtypeInstanceHasSupertypeInstance.SupertypeInstanceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(ObjectificationInstance.ObjectifiedInstanceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(EntityTypeRoleInstance.RoleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(FactTypeRoleInstance.RoleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 3;
				domainRole = domainDataDirectory.FindDomainRole(ObjectTypeInstanceHasPopulationMandatoryError.PopulationMandatoryErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 4;
				domainRole = domainDataDirectory.FindDomainRole(ObjectifyingInstanceHasObjectifiedInstanceRequiredError.ObjectifiedInstanceRequiredErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 5;
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
				IComparer<DomainRoleInfo> retVal = EntityTypeSubtypeInstance.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					EntityTypeSubtypeInstance.myCustomSortChildComparer = retVal;
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = EntityTypeSubtypeInstance.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(EntityTypeSubtypeInstanceHasSupertypeInstance.SupertypeInstanceDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|SupertypeInstance", match);
				match.InitializeRoles(ObjectificationInstance.ObjectifiedInstanceDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ObjectifiedInstance", match);
				EntityTypeSubtypeInstance.myChildElementMappings = childElementMappings;
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
	#endregion // EntityTypeSubtypeInstance serialization
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
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
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
				ret = new CustomSerializedContainerElementInfo[baseInfoCount + 3];
				if (baseInfoCount != 0)
				{
					baseInfo.CopyTo(ret, 3);
				}
				ret[0] = new CustomSerializedContainerElementInfo(null, "Definitions", null, CustomSerializedElementWriteStyle.Element, null, ValueConstraintHasDefinition.DefinitionDomainRoleId);
				ret[1] = new CustomSerializedContainerElementInfo(null, "Notes", null, CustomSerializedElementWriteStyle.Element, null, ValueConstraintHasNote.NoteDomainRoleId);
				ret[2] = new CustomSerializedContainerElementInfo(null, "ValueRanges", null, CustomSerializedElementWriteStyle.Element, null, ValueConstraintHasValueRange.ValueRangeDomainRoleId);
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
				domainRole = domainDataDirectory.FindDomainRole(ValueConstraintHasDefinition.DefinitionDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(ValueConstraintHasNote.NoteDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(ValueConstraintHasValueRange.ValueRangeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(ValueConstraintHasDuplicateNameError.DuplicateNameErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 3;
				domainRole = domainDataDirectory.FindDomainRole(ValueConstraintHasValueRangeOverlapError.ValueRangeOverlapErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 4;
				domainRole = domainDataDirectory.FindDomainRole(ValueConstraintHasValueTypeDetachedError.ValueTypeDetachedErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 5;
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
				IComparer<DomainRoleInfo> retVal = ValueConstraint.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					ValueConstraint.myCustomSortChildComparer = retVal;
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ValueConstraint.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ValueConstraintHasDefinition.DefinitionDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Definitions||", match);
				match.InitializeRoles(ValueConstraintHasNote.NoteDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Notes||", match);
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
	#region PathConditionRoleValueConstraint serialization
	partial class PathConditionRoleValueConstraint : ICustomSerializedElement
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
				return new CustomSerializedElementInfo(null, "PathedRoleConditionValueConstraint", null, CustomSerializedElementWriteStyle.Element, null);
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
			if (domainPropertyInfo.Id == PathConditionRoleValueConstraint.NameDomainPropertyId)
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
	}
	#endregion // PathConditionRoleValueConstraint serialization
	#region ValueRange serialization
	partial class ValueRange : ICustomSerializedElement
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
			if (domainPropertyInfo.Id == ValueRange.InvariantMaxValueDomainPropertyId)
			{
				if (string.IsNullOrEmpty(this.InvariantMaxValue))
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == ValueRange.InvariantMinValueDomainPropertyId)
			{
				if (string.IsNullOrEmpty(this.InvariantMinValue))
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
		private static Dictionary<string, Guid> myCustomSerializedAttributes;
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		protected new Guid MapAttribute(string xmlNamespace, string attributeName)
		{
			Dictionary<string, Guid> customSerializedAttributes = ValueRange.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("InvariantMaxValue", ValueRange.InvariantMaxValueDomainPropertyId);
				customSerializedAttributes.Add("InvariantMinValue", ValueRange.InvariantMinValueDomainPropertyId);
				ValueRange.myCustomSerializedAttributes = customSerializedAttributes;
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
				ret[5] = new CustomSerializedContainerElementInfo(null, "DerivationRule", null, CustomSerializedElementWriteStyle.Element, null, FactTypeHasDerivationExpression.DerivationRuleDomainRoleId, FactTypeHasDerivationRule.DerivationRuleDomainRoleId);
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
				domainRole = domainDataDirectory.FindDomainRole(FactTypeHasDerivationRule.DerivationRuleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 6;
				domainRole = domainDataDirectory.FindDomainRole(FactTypeHasFactTypeInstance.FactTypeInstanceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 7;
				domainRole = domainDataDirectory.FindDomainRole(ObjectificationImpliesFactType.ImpliedByObjectificationDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 8;
				domainRole = domainDataDirectory.FindDomainRole(Objectification.NestingTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 9;
				domainRole = domainDataDirectory.FindDomainRole(FactTypeHasFactTypeRequiresInternalUniquenessConstraintError.InternalUniquenessConstraintRequiredErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 10;
				domainRole = domainDataDirectory.FindDomainRole(FactTypeHasFactTypeRequiresReadingError.ReadingRequiredErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 11;
				domainRole = domainDataDirectory.FindDomainRole(FactTypeHasImpliedInternalUniquenessConstraintError.ImpliedInternalUniquenessConstraintErrorDomainRoleId).OppositeDomainRole;
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
				match.InitializeRoles(FactTypeHasDerivationExpression.DerivationRuleDomainRoleId, FactTypeHasDerivationRule.DerivationRuleDomainRoleId);
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
			if (roleId == ObjectificationInstance.ObjectifyingInstanceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ObjectifiedInstanceHasObjectifyingInstanceRequiredError.ObjectifyingInstanceRequiredErrorDomainRoleId)
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
		protected bool ShouldSerialize()
		{
			return !this.Role.RolePlayer.IsImplicitBooleanValue;
		}
		bool ICustomSerializedElement.ShouldSerialize()
		{
			return this.ShouldSerialize();
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
	#region FactTypeDerivationRule serialization
	partial class FactTypeDerivationRule : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
			CustomSerializedContainerElementInfo[] ret = FactTypeDerivationRule.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "DerivationProjections", null, CustomSerializedElementWriteStyle.Element, null, FactTypeDerivationProjection.RolePathDomainRoleId);
				ret[1] = new CustomSerializedContainerElementInfo(null, "InformalRule", null, CustomSerializedElementWriteStyle.Element, null, FactTypeDerivationRuleHasDerivationNote.DerivationNoteDomainRoleId);
				FactTypeDerivationRule.myCustomSerializedChildElementInfo = ret;
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
				return new CustomSerializedElementInfo(null, "FactTypeDerivationPath", null, CustomSerializedElementWriteStyle.Element, null);
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
			if (domainPropertyInfo.Id == FactTypeDerivationRule.DerivationCompletenessDomainPropertyId)
			{
				if (DerivationCompleteness.FullyDerived == this.DerivationCompleteness)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == FactTypeDerivationRule.DerivationStorageDomainPropertyId)
			{
				if (DerivationStorage.NotStored == this.DerivationStorage)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == FactTypeDerivationRule.SetProjectionDomainPropertyId)
			{
				if (!this.SetProjection)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == FactTypeDerivationRule.NameDomainPropertyId)
			{
				if (this.DerivationCompleteness != DerivationCompleteness.FullyDerived)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, "Name", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == FactTypeDerivationRule.ExternalDerivationDomainPropertyId)
			{
				if (!this.ExternalDerivation || this.LeadRolePathCollection.Count != 0)
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
			if (roleId == FactTypeDerivationProjection.RolePathDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "DerivationProjection", null, CustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == FactTypeDerivationRuleHasProjectionRequiredError.ProjectionRequiredErrorDomainRoleId)
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = FactTypeDerivationRule.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(FactTypeDerivationProjection.RolePathDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|DerivationProjections||DerivationProjection", match);
				match.InitializeRoles(FactTypeDerivationRuleHasDerivationNote.DerivationNoteDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|InformalRule||", match);
				FactTypeDerivationRule.myChildElementMappings = childElementMappings;
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
			Dictionary<string, Guid> customSerializedAttributes = FactTypeDerivationRule.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("DerivationCompleteness", FactTypeDerivationRule.DerivationCompletenessDomainPropertyId);
				customSerializedAttributes.Add("DerivationStorage", FactTypeDerivationRule.DerivationStorageDomainPropertyId);
				customSerializedAttributes.Add("SetProjection", FactTypeDerivationRule.SetProjectionDomainPropertyId);
				customSerializedAttributes.Add("Name", FactTypeDerivationRule.NameDomainPropertyId);
				customSerializedAttributes.Add("ExternalDerivation", FactTypeDerivationRule.ExternalDerivationDomainPropertyId);
				FactTypeDerivationRule.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // FactTypeDerivationRule serialization
	#region FactTypeDerivationProjection serialization
	partial class FactTypeDerivationProjection : ICustomSerializedElement
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
			if (roleId == FactTypeRoleProjection.ProjectedRoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "RoleProjection", null, CustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == FactTypeDerivationProjectionHasPartialProjectionError.PartialProjectionErrorDomainRoleId)
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = FactTypeDerivationProjection.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(FactTypeRoleProjection.ProjectedRoleDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|RoleProjection", match);
				FactTypeDerivationProjection.myChildElementMappings = childElementMappings;
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
	#endregion // FactTypeDerivationProjection serialization
	#region FactTypeRoleProjection serialization
	partial class FactTypeRoleProjection : ICustomSerializedElement
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
			CustomSerializedContainerElementInfo[] ret = FactTypeRoleProjection.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new CustomSerializedContainerElementInfo[1];
				ret[0] = new CustomSerializedContainerElementInfo(null, "DerivationSource", null, CustomSerializedElementWriteStyle.Element, null, FactTypeRoleProjectedFromRolePathRoot.SourceDomainRoleId, FactTypeRoleProjectedFromPathedRole.SourceDomainRoleId, FactTypeRoleProjectedFromCalculatedPathValue.SourceDomainRoleId, FactTypeRoleProjectedFromPathConstant.SourceDomainRoleId);
				FactTypeRoleProjection.myCustomSerializedChildElementInfo = ret;
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
			if (roleId == FactTypeRoleProjectedFromRolePathRoot.SourceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "PathRoot", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == FactTypeRoleProjectedFromPathedRole.SourceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "PathedRole", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == FactTypeRoleProjectedFromCalculatedPathValue.SourceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "CalculatedValue", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = FactTypeRoleProjection.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(FactTypeRoleProjectedFromRolePathRoot.SourceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|DerivationSource||PathRoot", match);
				match.InitializeRoles(FactTypeRoleProjectedFromPathedRole.SourceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|DerivationSource||PathedRole", match);
				match.InitializeRoles(FactTypeRoleProjectedFromCalculatedPathValue.SourceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|DerivationSource||CalculatedValue", match);
				match.InitializeRoles(FactTypeRoleProjectedFromPathConstant.SourceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|DerivationSource||", match);
				FactTypeRoleProjection.myChildElementMappings = childElementMappings;
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
	#endregion // FactTypeRoleProjection serialization
	#region SubtypeDerivationRule serialization
	partial class SubtypeDerivationRule : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo;
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
			CustomSerializedContainerElementInfo[] ret = SubtypeDerivationRule.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "InformalRule", null, CustomSerializedElementWriteStyle.Element, null, SubtypeDerivationRuleHasDerivationNote.DerivationNoteDomainRoleId);
				SubtypeDerivationRule.myCustomSerializedChildElementInfo = ret;
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
				return new CustomSerializedElementInfo(null, "SubtypeDerivationPath", null, CustomSerializedElementWriteStyle.Element, null);
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
			if (domainPropertyInfo.Id == SubtypeDerivationRule.ExternalDerivationDomainPropertyId)
			{
				if (!this.ExternalDerivation || this.LeadRolePathCollection.Count != 0)
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
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		protected new CustomSerializedElementMatch MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = SubtypeDerivationRule.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(SubtypeDerivationRuleHasDerivationNote.DerivationNoteDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|InformalRule||", match);
				SubtypeDerivationRule.myChildElementMappings = childElementMappings;
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
			Dictionary<string, Guid> customSerializedAttributes = SubtypeDerivationRule.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("ExternalDerivation", SubtypeDerivationRule.ExternalDerivationDomainPropertyId);
				SubtypeDerivationRule.myCustomSerializedAttributes = customSerializedAttributes;
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
		/// <summary>Implements ICustomSerializedElement.ShouldSerialize</summary>
		protected new bool ShouldSerialize()
		{
			// We leave this in memory so that a user can easily switch subtypes, but do not serialize it unless we have a current subtype.
			return this.Subtype.IsSubtype;
		}
		bool ICustomSerializedElement.ShouldSerialize()
		{
			return this.ShouldSerialize();
		}
	}
	#endregion // SubtypeDerivationRule serialization
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
				ret = new CustomSerializedContainerElementInfo[baseInfoCount + 3];
				if (baseInfoCount != 0)
				{
					baseInfo.CopyTo(ret, 3);
				}
				ret[0] = new CustomSerializedContainerElementInfo(null, "ValueRestriction", null, CustomSerializedElementWriteStyle.Element, null, RoleHasValueConstraint.ValueConstraintDomainRoleId);
				ret[1] = new CustomSerializedContainerElementInfo(null, "DerivationSource", null, CustomSerializedElementWriteStyle.Element, null, RoleDerivesFromPathedRole_Deprecated.SourceDomainRoleId, RoleDerivesFromCalculatedPathValue_Deprecated.SourceDomainRoleId, RoleDerivesFromPathConstant_Deprecated.SourceDomainRoleId);
				ret[2] = new CustomSerializedContainerElementInfo(null, "RoleInstances", null, CustomSerializedElementWriteStyle.Element, null, EntityTypeRoleInstance.ObjectTypeInstanceDomainRoleId, FactTypeRoleInstance.ObjectTypeInstanceDomainRoleId);
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
			if (roleId == RoleDerivesFromPathedRole_Deprecated.SourceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "PathedRole", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == RoleDerivesFromCalculatedPathValue_Deprecated.SourceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "CalculatedValue", null, CustomSerializedElementWriteStyle.Element, null);
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
			if (roleId == ObjectifiedUnaryRoleHasRole.ObjectifiedUnaryRoleDomainRoleId)
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
			if (roleId == PathedRole.RolePathDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FactTypeRoleProjection.DerivationProjectionDomainRoleId)
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
				domainRole = domainDataDirectory.FindDomainRole(RoleDerivesFromPathedRole_Deprecated.SourceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(RoleDerivesFromCalculatedPathValue_Deprecated.SourceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(RoleDerivesFromPathConstant_Deprecated.SourceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(EntityTypeRoleInstance.ObjectTypeInstanceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 3;
				domainRole = domainDataDirectory.FindDomainRole(FactTypeRoleInstance.ObjectTypeInstanceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 3;
				domainRole = domainDataDirectory.FindDomainRole(RoleProxyHasRole.ProxyDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 4;
				domainRole = domainDataDirectory.FindDomainRole(ObjectifiedUnaryRoleHasRole.ObjectifiedUnaryRoleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 5;
				domainRole = domainDataDirectory.FindDomainRole(ConstraintRoleSequenceHasRole.ConstraintRoleSequenceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 6;
				domainRole = domainDataDirectory.FindDomainRole(ReadingOrderHasRole.ReadingOrderDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 7;
				domainRole = domainDataDirectory.FindDomainRole(RoleHasRolePlayerRequiredError.RolePlayerRequiredErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 8;
				domainRole = domainDataDirectory.FindDomainRole(PathedRole.RolePathDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 9;
				domainRole = domainDataDirectory.FindDomainRole(FactTypeRoleProjection.DerivationProjectionDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 10;
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
				match.InitializeRoles(RoleDerivesFromPathedRole_Deprecated.SourceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|DerivationSource||PathedRole", match);
				match.InitializeRoles(RoleDerivesFromCalculatedPathValue_Deprecated.SourceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|DerivationSource||CalculatedValue", match);
				match.InitializeRoles(true, EntityTypeRoleInstance.ObjectTypeInstanceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|RoleInstances||EntityTypeRoleInstance", match);
				match.InitializeRoles(true, FactTypeRoleInstance.ObjectTypeInstanceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|RoleInstances||FactTypeRoleInstance", match);
				match.InitializeRoles(RoleHasValueConstraint.ValueConstraintDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ValueRestriction||", match);
				match.InitializeRoles(RoleDerivesFromPathConstant_Deprecated.SourceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|DerivationSource||", match);
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
	#region ObjectifiedUnaryRole serialization
	partial class ObjectifiedUnaryRole : ICustomSerializedElement
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
			if (roleId == ObjectifiedUnaryRoleHasRole.TargetRoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "UnaryRole", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ObjectifiedUnaryRole.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ObjectifiedUnaryRoleHasRole.TargetRoleDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|UnaryRole", match);
				ObjectifiedUnaryRole.myChildElementMappings = childElementMappings;
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
	#endregion // ObjectifiedUnaryRole serialization
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
				ret = new CustomSerializedContainerElementInfo[baseInfoCount + 3];
				if (baseInfoCount != 0)
				{
					baseInfo.CopyTo(ret, 3);
				}
				ret[0] = new CustomSerializedContainerElementInfo(null, "Definitions", null, CustomSerializedElementWriteStyle.Element, null, SetComparisonConstraintHasDefinition.DefinitionDomainRoleId);
				ret[1] = new CustomSerializedContainerElementInfo(null, "Notes", null, CustomSerializedElementWriteStyle.Element, null, SetComparisonConstraintHasNote.NoteDomainRoleId);
				ret[2] = new CustomSerializedContainerElementInfo(null, "RoleSequences", null, CustomSerializedElementWriteStyle.Element, null, SetComparisonConstraintHasRoleSequence.RoleSequenceDomainRoleId);
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
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasDefinition.DefinitionDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasNote.NoteDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasRoleSequence.RoleSequenceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasCompatibleRolePlayerTypeError.CompatibleRolePlayerTypeErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 3;
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 4;
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 5;
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasExternalConstraintRoleSequenceArityMismatchError.ArityMismatchErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 6;
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasImplicationError.ImplicationErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 7;
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasContradictionError.ContradictionErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 8;
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasDuplicateNameError.DuplicateNameErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 9;
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasEqualityOrSubsetImpliedByMandatoryError.EqualityOrSubsetImpliedByMandatoryErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 10;
				domainRole = domainDataDirectory.FindDomainRole(ModelNoteReferencesSetComparisonConstraint.NoteDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 11;
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
				match.InitializeRoles(SetComparisonConstraintHasDefinition.DefinitionDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Definitions||", match);
				match.InitializeRoles(SetComparisonConstraintHasNote.NoteDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Notes||", match);
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
	#region ConstraintRoleSequence serialization
	partial class ConstraintRoleSequence : ICustomSerializedElement
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
			if (roleId == ConstraintRoleSequenceHasJoinPathRequiredError.JoinPathRequiredErrorDomainRoleId)
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
	#endregion // ConstraintRoleSequence serialization
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "JoinRule", null, CustomSerializedElementWriteStyle.Element, null, ConstraintRoleSequenceHasJoinPath.JoinPathDomainRoleId);
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
				domainRole = domainDataDirectory.FindDomainRole(ConstraintRoleSequenceHasJoinPath.JoinPathDomainRoleId).OppositeDomainRole;
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
				match.InitializeRoles(ConstraintRoleSequenceHasJoinPath.JoinPathDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|JoinRule||", match);
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
				ret = new CustomSerializedContainerElementInfo[baseInfoCount + 4];
				if (baseInfoCount != 0)
				{
					baseInfo.CopyTo(ret, 4);
				}
				ret[0] = new CustomSerializedContainerElementInfo(null, "Definitions", null, CustomSerializedElementWriteStyle.Element, null, SetConstraintHasDefinition.DefinitionDomainRoleId);
				ret[1] = new CustomSerializedContainerElementInfo(null, "Notes", null, CustomSerializedElementWriteStyle.Element, null, SetConstraintHasNote.NoteDomainRoleId);
				ret[2] = new CustomSerializedContainerElementInfo(null, "RoleSequence", null, CustomSerializedElementWriteStyle.Element, null, ConstraintRoleSequenceHasRole.RoleDomainRoleId);
				ret[3] = new CustomSerializedInnerContainerElementInfo(null, "JoinRule", null, CustomSerializedElementWriteStyle.Element, null, ret[2], ConstraintRoleSequenceHasJoinPath.JoinPathDomainRoleId);
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
				domainRole = domainDataDirectory.FindDomainRole(SetConstraintHasDefinition.DefinitionDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(SetConstraintHasNote.NoteDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(ConstraintRoleSequenceHasRole.RoleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(ConstraintRoleSequenceHasJoinPath.JoinPathDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 3;
				domainRole = domainDataDirectory.FindDomainRole(SetConstraintHasCompatibleRolePlayerTypeError.CompatibleRolePlayerTypeErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 4;
				domainRole = domainDataDirectory.FindDomainRole(SetConstraintHasTooFewRoleSequencesError.TooFewRoleSequencesErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 5;
				domainRole = domainDataDirectory.FindDomainRole(SetConstraintHasTooManyRoleSequencesError.TooManyRoleSequencesErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 6;
				domainRole = domainDataDirectory.FindDomainRole(FactSetConstraint.FactTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 7;
				domainRole = domainDataDirectory.FindDomainRole(SetConstraintHasImplicationError.ImplicationErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 8;
				domainRole = domainDataDirectory.FindDomainRole(SetConstraintHasDuplicateNameError.DuplicateNameErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 9;
				domainRole = domainDataDirectory.FindDomainRole(ModelNoteReferencesSetConstraint.NoteDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 10;
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
				match.InitializeRoles(SetConstraintHasDefinition.DefinitionDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Definitions||", match);
				match.InitializeRoles(SetConstraintHasNote.NoteDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Notes||", match);
				match.InitializeRoles(ConstraintRoleSequenceHasJoinPath.JoinPathDomainRoleId);
				childElementMappings.Add("http://schemas.neumont.edu/ORM/2006-04/ORMCore|RoleSequence||JoinRule||", match);
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
				return CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
			CustomSerializedContainerElementInfo[] ret = ConstraintRoleSequenceHasRole.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new CustomSerializedContainerElementInfo[1];
				ret[0] = new CustomSerializedContainerElementInfo(null, "ProjectedFrom", null, CustomSerializedElementWriteStyle.Element, null, ConstraintRoleProjectedFromPathedRole_Deprecated.SourceDomainRoleId, ConstraintRoleProjectedFromCalculatedPathValue_Deprecated.SourceDomainRoleId, ConstraintRoleProjectedFromPathConstant_Deprecated.SourceDomainRoleId);
				ConstraintRoleSequenceHasRole.myCustomSerializedChildElementInfo = ret;
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
			if (roleId == ConstraintRoleProjectedFromPathedRole_Deprecated.SourceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "PathedRole", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == ConstraintRoleProjectedFromCalculatedPathValue_Deprecated.SourceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "CalculatedValue", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == ConstraintRoleProjection.JoinPathProjectionDomainRoleId)
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ConstraintRoleSequenceHasRole.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ConstraintRoleProjectedFromPathedRole_Deprecated.SourceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ProjectedFrom||PathedRole", match);
				match.InitializeRoles(ConstraintRoleProjectedFromCalculatedPathValue_Deprecated.SourceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ProjectedFrom||CalculatedValue", match);
				match.InitializeRoles(ConstraintRoleProjectedFromPathConstant_Deprecated.SourceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ProjectedFrom||", match);
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
	#region ConstraintRoleSequenceJoinPath serialization
	partial class ConstraintRoleSequenceJoinPath : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
			CustomSerializedContainerElementInfo[] ret = ConstraintRoleSequenceJoinPath.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "JoinPathProjections", null, CustomSerializedElementWriteStyle.Element, null, ConstraintRoleSequenceJoinPathProjection.RolePathDomainRoleId);
				ConstraintRoleSequenceJoinPath.myCustomSerializedChildElementInfo = ret;
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
				return new CustomSerializedElementInfo(null, "JoinPath", null, CustomSerializedElementWriteStyle.Element, null);
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
			if (domainPropertyInfo.Id == ConstraintRoleSequenceJoinPath.IsAutomaticDomainPropertyId)
			{
				if (!this.IsAutomatic)
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
			if (roleId == ConstraintRoleSequenceJoinPathProjection.RolePathDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "JoinPathProjection", null, CustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == ConstraintRoleSequenceJoinPathHasProjectionRequiredError.ProjectionRequiredErrorDomainRoleId)
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ConstraintRoleSequenceJoinPath.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ConstraintRoleSequenceJoinPathProjection.RolePathDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|JoinPathProjections||JoinPathProjection", match);
				ConstraintRoleSequenceJoinPath.myChildElementMappings = childElementMappings;
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
			Dictionary<string, Guid> customSerializedAttributes = ConstraintRoleSequenceJoinPath.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("IsAutomatic", ConstraintRoleSequenceJoinPath.IsAutomaticDomainPropertyId);
				ConstraintRoleSequenceJoinPath.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // ConstraintRoleSequenceJoinPath serialization
	#region ConstraintRoleSequenceJoinPathProjection serialization
	partial class ConstraintRoleSequenceJoinPathProjection : ICustomSerializedElement
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
			if (roleId == ConstraintRoleProjection.ProjectedConstraintRoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ConstraintRoleProjection", null, CustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == ConstraintRoleSequenceProjectionHasPartialProjectionError.PartialProjectionErrorDomainRoleId)
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ConstraintRoleSequenceJoinPathProjection.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ConstraintRoleProjection.ProjectedConstraintRoleDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ConstraintRoleProjection", match);
				ConstraintRoleSequenceJoinPathProjection.myChildElementMappings = childElementMappings;
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
	#endregion // ConstraintRoleSequenceJoinPathProjection serialization
	#region ConstraintRoleProjection serialization
	partial class ConstraintRoleProjection : ICustomSerializedElement
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
			CustomSerializedContainerElementInfo[] ret = ConstraintRoleProjection.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new CustomSerializedContainerElementInfo[1];
				ret[0] = new CustomSerializedContainerElementInfo(null, "ProjectedFrom", null, CustomSerializedElementWriteStyle.Element, null, ConstraintRoleProjectedFromRolePathRoot.SourceDomainRoleId, ConstraintRoleProjectedFromPathedRole.SourceDomainRoleId, ConstraintRoleProjectedFromCalculatedPathValue.SourceDomainRoleId, ConstraintRoleProjectedFromPathConstant.SourceDomainRoleId);
				ConstraintRoleProjection.myCustomSerializedChildElementInfo = ret;
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
			if (roleId == ConstraintRoleProjectedFromRolePathRoot.SourceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "PathRoot", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == ConstraintRoleProjectedFromPathedRole.SourceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "PathedRole", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == ConstraintRoleProjectedFromCalculatedPathValue.SourceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "CalculatedValue", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ConstraintRoleProjection.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ConstraintRoleProjectedFromRolePathRoot.SourceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ProjectedFrom||PathRoot", match);
				match.InitializeRoles(ConstraintRoleProjectedFromPathedRole.SourceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ProjectedFrom||PathedRole", match);
				match.InitializeRoles(ConstraintRoleProjectedFromCalculatedPathValue.SourceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ProjectedFrom||CalculatedValue", match);
				match.InitializeRoles(ConstraintRoleProjectedFromPathConstant.SourceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ProjectedFrom||", match);
				ConstraintRoleProjection.myChildElementMappings = childElementMappings;
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
	#endregion // ConstraintRoleProjection serialization
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
			if (roleId == FrequencyConstraintHasFrequencyConstraintViolatedByUniquenessConstraintError.FrequencyConstraintViolatedByUniquenessConstraintErrorDomainRoleId)
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
				domainRole = domainDataDirectory.FindDomainRole(SetComparisonConstraintHasExclusionContradictsEqualityError.ExclusionContradictsEqualityErrorDomainRoleId).OppositeDomainRole;
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
	#region Function serialization
	partial class Function : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
			CustomSerializedContainerElementInfo[] ret = Function.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "Parameters", null, CustomSerializedElementWriteStyle.Element, null, FunctionOperatesOnParameter.ParameterDomainRoleId);
				Function.myCustomSerializedChildElementInfo = ret;
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
			if (domainPropertyInfo.Id == Function.IsBooleanDomainPropertyId)
			{
				if (!this.IsBoolean)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == Function.OperatorSymbolDomainPropertyId)
			{
				if (string.IsNullOrEmpty(this.OperatorSymbol))
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
			if (roleId == CalculatedPathValueIsCalculatedWithFunction.CalculatedValueDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FunctionHasDuplicateNameError.DuplicateNameErrorDomainRoleId)
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = Function.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(FunctionOperatesOnParameter.ParameterDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Parameters||", match);
				Function.myChildElementMappings = childElementMappings;
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
			Dictionary<string, Guid> customSerializedAttributes = Function.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("IsBoolean", Function.IsBooleanDomainPropertyId);
				customSerializedAttributes.Add("OperatorSymbol", Function.OperatorSymbolDomainPropertyId);
				Function.myCustomSerializedAttributes = customSerializedAttributes;
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
		/// <summary>Implements ICustomSerializedElement.ShouldSerialize</summary>
		protected new bool ShouldSerialize()
		{
			return this.CalculatedValueCollection.Count != 0;
		}
		bool ICustomSerializedElement.ShouldSerialize()
		{
			return this.ShouldSerialize();
		}
	}
	#endregion // Function serialization
	#region FunctionParameter serialization
	partial class FunctionParameter : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
				return new CustomSerializedElementInfo(null, "Parameter", null, CustomSerializedElementWriteStyle.Element, null);
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
			if (domainPropertyInfo.Id == FunctionParameter.BagInputDomainPropertyId)
			{
				if (!this.BagInput)
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
			if (roleId == CalculatedPathValueInputCorrespondsToFunctionParameter.InputDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == CalculatedPathValueParameterBindingErrorTargetsFunctionParameter.ParameterBindingErrorDomainRoleId)
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
			Dictionary<string, Guid> customSerializedAttributes = FunctionParameter.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("BagInput", FunctionParameter.BagInputDomainPropertyId);
				FunctionParameter.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // FunctionParameter serialization
	#region RolePathOwner serialization
	partial class RolePathOwner : ICustomSerializedElement
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
			CustomSerializedContainerElementInfo[] ret = RolePathOwner.myCustomSerializedChildElementInfo;
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
				ret = new CustomSerializedContainerElementInfo[baseInfoCount + 3];
				if (baseInfoCount != 0)
				{
					baseInfo.CopyTo(ret, 3);
				}
				ret[0] = new CustomSerializedContainerElementInfo(null, "PathComponent", null, CustomSerializedElementWriteStyle.Element, null, RolePathOwnerHasPathComponent_Deprecated.RolePathDomainRoleId);
				ret[1] = new CustomSerializedContainerElementInfo(null, "PathComponents", null, CustomSerializedElementWriteStyle.Element, null, RolePathOwnerOwnsLeadRolePath.RolePathDomainRoleId, RolePathOwnerUsesSharedLeadRolePath.RolePathDomainRoleId);
				ret[2] = new CustomSerializedContainerElementInfo(null, "CalculatedValues", null, CustomSerializedElementWriteStyle.Element, null, RolePathOwnerCalculatesCalculatedPathValue_Deprecated.CalculatedValueDomainRoleId);
				RolePathOwner.myCustomSerializedChildElementInfo = ret;
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
			if (roleId == RolePathOwnerUsesSharedLeadRolePath.RolePathDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "SharedRolePath", null, CustomSerializedElementWriteStyle.Element, null);
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
				domainRole = domainDataDirectory.FindDomainRole(RolePathOwnerHasPathComponent_Deprecated.RolePathDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(RolePathOwnerOwnsLeadRolePath.RolePathDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(RolePathOwnerUsesSharedLeadRolePath.RolePathDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(RolePathOwnerCalculatesCalculatedPathValue_Deprecated.CalculatedValueDomainRoleId).OppositeDomainRole;
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
				IComparer<DomainRoleInfo> retVal = RolePathOwner.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					RolePathOwner.myCustomSortChildComparer = retVal;
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = RolePathOwner.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(RolePathOwnerUsesSharedLeadRolePath.RolePathDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|PathComponents||SharedRolePath", match);
				match.InitializeRoles(RolePathOwnerHasPathComponent_Deprecated.RolePathDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|PathComponent||", match);
				match.InitializeRoles(RolePathOwnerOwnsLeadRolePath.RolePathDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|PathComponents||", match);
				match.InitializeRoles(RolePathOwnerCalculatesCalculatedPathValue_Deprecated.CalculatedValueDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|CalculatedValues||", match);
				RolePathOwner.myChildElementMappings = childElementMappings;
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
	#endregion // RolePathOwner serialization
	#region LeadRolePath serialization
	partial class LeadRolePath : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
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
			CustomSerializedContainerElementInfo[] ret = LeadRolePath.myCustomSerializedChildElementInfo;
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
				ret = new CustomSerializedContainerElementInfo[baseInfoCount + 3];
				if (baseInfoCount != 0)
				{
					baseInfo.CopyTo(ret, 3);
				}
				ret[0] = new CustomSerializedContainerElementInfo(null, "ObjectUnifiers", null, CustomSerializedElementWriteStyle.Element, null, LeadRolePathHasObjectUnifier.ObjectUnifierDomainRoleId);
				ret[1] = new CustomSerializedContainerElementInfo(null, "CalculatedValues", null, CustomSerializedElementWriteStyle.Element, null, LeadRolePathCalculatesCalculatedPathValue.CalculatedValueDomainRoleId);
				ret[2] = new CustomSerializedContainerElementInfo(null, "Conditions", null, CustomSerializedElementWriteStyle.Element, null, LeadRolePathSatisfiesCalculatedCondition.CalculatedConditionDomainRoleId);
				LeadRolePath.myCustomSerializedChildElementInfo = ret;
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
				return new CustomSerializedElementInfo(null, "RolePath", null, CustomSerializedElementWriteStyle.Element, null);
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
			if (roleId == LeadRolePathSatisfiesCalculatedCondition.CalculatedConditionDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "CalculatedCondition", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == FactTypeDerivationProjection.DerivationRuleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConstraintRoleSequenceJoinPathProjection.JoinPathDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == RolePathOwnerUsesSharedLeadRolePath.PathOwnerDomainRoleId)
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
				domainRole = domainDataDirectory.FindDomainRole(LeadRolePathHasObjectUnifier.ObjectUnifierDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(LeadRolePathCalculatesCalculatedPathValue.CalculatedValueDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(LeadRolePathSatisfiesCalculatedCondition.CalculatedConditionDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(FactTypeDerivationProjection.DerivationRuleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 3;
				domainRole = domainDataDirectory.FindDomainRole(ConstraintRoleSequenceJoinPathProjection.JoinPathDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 4;
				domainRole = domainDataDirectory.FindDomainRole(RolePathOwnerUsesSharedLeadRolePath.PathOwnerDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 5;
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
				IComparer<DomainRoleInfo> retVal = LeadRolePath.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					LeadRolePath.myCustomSortChildComparer = retVal;
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = LeadRolePath.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(LeadRolePathSatisfiesCalculatedCondition.CalculatedConditionDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Conditions||CalculatedCondition", match);
				match.InitializeRoles(LeadRolePathHasObjectUnifier.ObjectUnifierDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ObjectUnifiers||", match);
				match.InitializeRoles(LeadRolePathCalculatesCalculatedPathValue.CalculatedValueDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|CalculatedValues||", match);
				LeadRolePath.myChildElementMappings = childElementMappings;
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
	#endregion // LeadRolePath serialization
	#region CalculatedPathValue serialization
	partial class CalculatedPathValue : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
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
			CustomSerializedContainerElementInfo[] ret = CalculatedPathValue.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "Inputs", null, CustomSerializedElementWriteStyle.Element, null, CalculatedPathValueHasInput.InputDomainRoleId);
				CalculatedPathValue.myCustomSerializedChildElementInfo = ret;
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
				return new CustomSerializedElementInfo(null, "CalculatedValue", null, CustomSerializedElementWriteStyle.Element, null);
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
			if (roleId == CalculatedPathValueIsCalculatedWithFunction.FunctionDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Function", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == CalculatedPathValueScopedWithPathedRole.ScopeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Scope", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == LeadRolePathSatisfiesCalculatedCondition.LeadRolePathDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == CalculatedPathValueInputBindsToCalculatedPathValue.InputDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FactTypeRoleProjectedFromCalculatedPathValue.RoleProjectionDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == RoleDerivesFromCalculatedPathValue_Deprecated.RoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConstraintRoleProjectedFromCalculatedPathValue.ConstraintRoleProjectionDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConstraintRoleProjectedFromCalculatedPathValue_Deprecated.ConstraintRoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == CalculatedPathValueHasFunctionRequiredError.FunctionRequiredErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == CalculatedPathValueHasUnboundParameterError.ParameterBindingErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == CalculatedPathValueHasConsumptionRequiredError.ConsumptionRequiredErrorDomainRoleId)
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
				domainRole = domainDataDirectory.FindDomainRole(CalculatedPathValueIsCalculatedWithFunction.FunctionDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(CalculatedPathValueScopedWithPathedRole.ScopeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(CalculatedPathValueHasInput.InputDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(LeadRolePathSatisfiesCalculatedCondition.LeadRolePathDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 3;
				domainRole = domainDataDirectory.FindDomainRole(CalculatedPathValueInputBindsToCalculatedPathValue.InputDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 4;
				domainRole = domainDataDirectory.FindDomainRole(FactTypeRoleProjectedFromCalculatedPathValue.RoleProjectionDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 5;
				domainRole = domainDataDirectory.FindDomainRole(RoleDerivesFromCalculatedPathValue_Deprecated.RoleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 6;
				domainRole = domainDataDirectory.FindDomainRole(ConstraintRoleProjectedFromCalculatedPathValue.ConstraintRoleProjectionDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 7;
				domainRole = domainDataDirectory.FindDomainRole(ConstraintRoleProjectedFromCalculatedPathValue_Deprecated.ConstraintRoleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 8;
				domainRole = domainDataDirectory.FindDomainRole(CalculatedPathValueHasFunctionRequiredError.FunctionRequiredErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 9;
				domainRole = domainDataDirectory.FindDomainRole(CalculatedPathValueHasUnboundParameterError.ParameterBindingErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 10;
				domainRole = domainDataDirectory.FindDomainRole(CalculatedPathValueHasConsumptionRequiredError.ConsumptionRequiredErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 11;
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
				IComparer<DomainRoleInfo> retVal = CalculatedPathValue.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					CalculatedPathValue.myCustomSortChildComparer = retVal;
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = CalculatedPathValue.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(CalculatedPathValueIsCalculatedWithFunction.FunctionDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Function", match);
				match.InitializeRoles(CalculatedPathValueScopedWithPathedRole.ScopeDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Scope", match);
				match.InitializeRoles(CalculatedPathValueHasInput.InputDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Inputs||", match);
				CalculatedPathValue.myChildElementMappings = childElementMappings;
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
	#endregion // CalculatedPathValue serialization
	#region CalculatedPathValueInput serialization
	partial class CalculatedPathValueInput : ICustomSerializedElement
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
			CustomSerializedContainerElementInfo[] ret = CalculatedPathValueInput.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "Source", null, CustomSerializedElementWriteStyle.Element, null, CalculatedPathValueInputBindsToRolePathRoot.SourceDomainRoleId, CalculatedPathValueInputBindsToPathedRole.SourceDomainRoleId, CalculatedPathValueInputBindsToCalculatedPathValue.SourceDomainRoleId, CalculatedPathValueInputBindsToPathConstant.SourceDomainRoleId);
				CalculatedPathValueInput.myCustomSerializedChildElementInfo = ret;
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
				return new CustomSerializedElementInfo(null, "Input", null, CustomSerializedElementWriteStyle.Element, null);
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
			if (domainPropertyInfo.Id == CalculatedPathValueInput.DistinctValuesDomainPropertyId)
			{
				FunctionParameter parameter;
				if (!this.DistinctValues || null == (parameter = this.Parameter) || !parameter.BagInput)
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
			if (roleId == CalculatedPathValueInputCorrespondsToFunctionParameter.ParameterDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Parameter", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == CalculatedPathValueInputBindsToRolePathRoot.SourceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "PathRoot", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == CalculatedPathValueInputBindsToPathedRole.SourceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "PathedRole", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == CalculatedPathValueInputBindsToCalculatedPathValue.SourceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "CalculatedValue", null, CustomSerializedElementWriteStyle.Element, null);
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
				domainRole = domainDataDirectory.FindDomainRole(CalculatedPathValueInputCorrespondsToFunctionParameter.ParameterDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(CalculatedPathValueInputBindsToRolePathRoot.SourceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(CalculatedPathValueInputBindsToPathedRole.SourceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(CalculatedPathValueInputBindsToCalculatedPathValue.SourceDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(CalculatedPathValueInputBindsToPathConstant.SourceDomainRoleId).OppositeDomainRole;
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
				IComparer<DomainRoleInfo> retVal = CalculatedPathValueInput.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					CalculatedPathValueInput.myCustomSortChildComparer = retVal;
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = CalculatedPathValueInput.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(CalculatedPathValueInputCorrespondsToFunctionParameter.ParameterDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Parameter", match);
				match.InitializeRoles(CalculatedPathValueInputBindsToRolePathRoot.SourceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Source||PathRoot", match);
				match.InitializeRoles(CalculatedPathValueInputBindsToPathedRole.SourceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Source||PathedRole", match);
				match.InitializeRoles(CalculatedPathValueInputBindsToCalculatedPathValue.SourceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Source||CalculatedValue", match);
				match.InitializeRoles(CalculatedPathValueInputBindsToPathConstant.SourceDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Source||", match);
				CalculatedPathValueInput.myChildElementMappings = childElementMappings;
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
			Dictionary<string, Guid> customSerializedAttributes = CalculatedPathValueInput.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("DistinctValues", CalculatedPathValueInput.DistinctValuesDomainPropertyId);
				CalculatedPathValueInput.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // CalculatedPathValueInput serialization
	#region PathConstant serialization
	partial class PathConstant : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.MixedTypedAttributes;
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
				return new CustomSerializedElementInfo(null, "Constant", null, CustomSerializedElementWriteStyle.Element, null);
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
			if (domainPropertyInfo.Id == PathConstant.LexicalValueDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, "Value", null, false, CustomSerializedAttributeWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = PathConstant.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeAttribute(PathConstant.LexicalValueDomainPropertyId, null);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Value", match);
				PathConstant.myChildElementMappings = childElementMappings;
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
	#endregion // PathConstant serialization
	#region RolePath serialization
	partial class RolePath : ICustomSerializedElement
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
			CustomSerializedContainerElementInfo[] ret = RolePath.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "PathedRoles", null, CustomSerializedElementWriteStyle.Element, null, PathedRole.RoleDomainRoleId);
				ret[1] = new CustomSerializedContainerElementInfo(null, "SubPaths", null, CustomSerializedElementWriteStyle.Element, null, RoleSubPathIsContinuationOfRolePath.SubPathDomainRoleId);
				RolePath.myCustomSerializedChildElementInfo = ret;
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
			if (domainPropertyInfo.Id == RolePath.SplitIsNegatedDomainPropertyId)
			{
				if (!this.SplitIsNegated || this.SubPathCollection.Count <= 1)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == RolePath.SplitCombinationOperatorDomainPropertyId)
			{
				if (this.SubPathCollection.Count <= 1)
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
			if (roleId == RolePathObjectTypeRoot.RootObjectTypeDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "RootObjectType", null, CustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == PathedRole.RoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "PathedRole", null, CustomSerializedElementWriteStyle.PrimaryLinkElement, null);
			}
			if (roleId == RolePathHasRootObjectTypeError.RootObjectTypeRequiredErrorDomainRoleId)
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
				domainRole = domainDataDirectory.FindDomainRole(RolePathObjectTypeRoot.RootObjectTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(PathedRole.RoleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(RoleSubPathIsContinuationOfRolePath.SubPathDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(RolePathHasRootObjectTypeError.RootObjectTypeRequiredErrorDomainRoleId).OppositeDomainRole;
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
				IComparer<DomainRoleInfo> retVal = RolePath.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					RolePath.myCustomSortChildComparer = retVal;
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = RolePath.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(RolePathObjectTypeRoot.RootObjectTypeDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|RootObjectType", match);
				match.InitializeRoles(true, PathedRole.RoleDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|PathedRoles||PathedRole", match);
				match.InitializeRoles(RoleSubPathIsContinuationOfRolePath.SubPathDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|SubPaths||", match);
				RolePath.myChildElementMappings = childElementMappings;
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
			Dictionary<string, Guid> customSerializedAttributes = RolePath.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("SplitIsNegated", RolePath.SplitIsNegatedDomainPropertyId);
				customSerializedAttributes.Add("SplitCombinationOperator", RolePath.SplitCombinationOperatorDomainPropertyId);
				RolePath.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // RolePath serialization
	#region RolePathObjectTypeRoot serialization
	partial class RolePathObjectTypeRoot : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo;
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
				return new CustomSerializedElementInfo(null, "RootObjectType", null, CustomSerializedElementWriteStyle.Element, null);
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
			if (domainPropertyInfo.Id == RolePathObjectTypeRoot.IsNegatedDomainPropertyId)
			{
				if (!this.IsNegated)
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
			if (roleId == CalculatedPathValueInputBindsToRolePathRoot.InputDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConstraintRoleProjectedFromRolePathRoot.ConstraintRoleProjectionDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FactTypeRoleProjectedFromRolePathRoot.RoleProjectionDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == PathObjectUnifierUnifiesRolePathRoot.ObjectUnifierDomainRoleId)
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
			Dictionary<string, Guid> customSerializedAttributes = RolePathObjectTypeRoot.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("IsNegated", RolePathObjectTypeRoot.IsNegatedDomainPropertyId);
				RolePathObjectTypeRoot.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // RolePathObjectTypeRoot serialization
	#region RoleSubPath serialization
	partial class RoleSubPath : ICustomSerializedElement
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
				return new CustomSerializedElementInfo(null, "SubPath", null, CustomSerializedElementWriteStyle.Element, null);
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
	#endregion // RoleSubPath serialization
	#region PathedRole serialization
	partial class PathedRole : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.PropertyInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
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
			CustomSerializedContainerElementInfo[] ret = PathedRole.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new CustomSerializedContainerElementInfo[1];
				ret[0] = new CustomSerializedContainerElementInfo(null, "ValueRestriction", null, CustomSerializedElementWriteStyle.Element, null, PathedRoleHasValueConstraint.ValueConstraintDomainRoleId);
				PathedRole.myCustomSerializedChildElementInfo = ret;
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
			if (domainPropertyInfo.Id == PathedRole.IsNegatedDomainPropertyId)
			{
				if (!this.IsNegated)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == PathedRole.PathedRolePurposeDomainPropertyId)
			{
				return new CustomSerializedPropertyInfo(null, "Purpose", null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
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
			if (roleId == PathedRoleIsRemotelyCorrelatedWithPathedRole_Deprecated.CorrelatingParentDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "CorrelatedWith", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == PathedRoleIsRemotelyCorrelatedWithPathedRole_Deprecated.CorrelatedChildDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == CalculatedPathValueScopedWithPathedRole.CalculatedValueDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == CalculatedPathValueInputBindsToPathedRole.InputDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == FactTypeRoleProjectedFromPathedRole.RoleProjectionDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == RoleDerivesFromPathedRole_Deprecated.RoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConstraintRoleProjectedFromPathedRole.ConstraintRoleProjectionDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ConstraintRoleProjectedFromPathedRole_Deprecated.ConstraintRoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == PathedRoleHasCompatibleJoinRolePlayerError.JoinCompatibilityErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == PathedRoleHasSameFactTypeFollowsJoinError.SameFactTypeRoleFollowsJoinErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == PathedRoleHasMandatoryOuterJoinError.MandatoryOuterJoinErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == PathObjectUnifierUnifiesPathedRole.ObjectUnifierDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
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
				domainRole = domainDataDirectory.FindDomainRole(PathedRoleHasValueConstraint.ValueConstraintDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(PathedRoleIsRemotelyCorrelatedWithPathedRole_Deprecated.CorrelatingParentDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(PathedRoleIsRemotelyCorrelatedWithPathedRole_Deprecated.CorrelatedChildDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(CalculatedPathValueScopedWithPathedRole.CalculatedValueDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 3;
				domainRole = domainDataDirectory.FindDomainRole(CalculatedPathValueInputBindsToPathedRole.InputDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 4;
				domainRole = domainDataDirectory.FindDomainRole(FactTypeRoleProjectedFromPathedRole.RoleProjectionDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 5;
				domainRole = domainDataDirectory.FindDomainRole(RoleDerivesFromPathedRole_Deprecated.RoleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 6;
				domainRole = domainDataDirectory.FindDomainRole(ConstraintRoleProjectedFromPathedRole.ConstraintRoleProjectionDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 7;
				domainRole = domainDataDirectory.FindDomainRole(ConstraintRoleProjectedFromPathedRole_Deprecated.ConstraintRoleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 8;
				domainRole = domainDataDirectory.FindDomainRole(PathedRoleHasCompatibleJoinRolePlayerError.JoinCompatibilityErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 9;
				domainRole = domainDataDirectory.FindDomainRole(PathedRoleHasSameFactTypeFollowsJoinError.SameFactTypeRoleFollowsJoinErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 10;
				domainRole = domainDataDirectory.FindDomainRole(PathedRoleHasMandatoryOuterJoinError.MandatoryOuterJoinErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 11;
				domainRole = domainDataDirectory.FindDomainRole(PathObjectUnifierUnifiesPathedRole.ObjectUnifierDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 12;
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
				IComparer<DomainRoleInfo> retVal = PathedRole.myCustomSortChildComparer;
				if (null == retVal)
				{
					retVal = new CustomSortChildComparer(this.Store);
					PathedRole.myCustomSortChildComparer = retVal;
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = PathedRole.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(PathedRoleIsRemotelyCorrelatedWithPathedRole_Deprecated.CorrelatingParentDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|CorrelatedWith", match);
				match.InitializeRoles(PathedRoleHasValueConstraint.ValueConstraintDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ValueRestriction||", match);
				PathedRole.myChildElementMappings = childElementMappings;
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
			Dictionary<string, Guid> customSerializedAttributes = PathedRole.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("IsNegated", PathedRole.IsNegatedDomainPropertyId);
				customSerializedAttributes.Add("Purpose", PathedRole.PathedRolePurposeDomainPropertyId);
				PathedRole.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // PathedRole serialization
	#region PathObjectUnifier serialization
	partial class PathObjectUnifier : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
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
				return new CustomSerializedElementInfo(null, "ObjectUnifier", null, CustomSerializedElementWriteStyle.Element, null);
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
			if (roleId == PathObjectUnifierUnifiesRolePathRoot.PathRootDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "PathRoot", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == PathObjectUnifierUnifiesPathedRole.PathedRoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "PathedRole", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == PathObjectUnifierHasCompatibleObjectTypesError.ObjectUnifierDomainRoleId)
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
				domainRole = domainDataDirectory.FindDomainRole(PathObjectUnifierUnifiesRolePathRoot.PathRootDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(PathObjectUnifierUnifiesPathedRole.PathedRoleDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(PathObjectUnifierHasCompatibleObjectTypesError.ObjectUnifierDomainRoleId).OppositeDomainRole;
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
				IComparer<DomainRoleInfo> retVal = PathObjectUnifier.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					PathObjectUnifier.myCustomSortChildComparer = retVal;
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = PathObjectUnifier.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(PathObjectUnifierUnifiesRolePathRoot.PathRootDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|PathRoot", match);
				match.InitializeRoles(PathObjectUnifierUnifiesPathedRole.PathedRoleDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|PathedRole", match);
				PathObjectUnifier.myChildElementMappings = childElementMappings;
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
	#endregion // PathObjectUnifier serialization
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
	#region FunctionDuplicateNameError serialization
	partial class FunctionDuplicateNameError : ICustomSerializedElement
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
			CustomSerializedContainerElementInfo[] ret = FunctionDuplicateNameError.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "Functions", null, CustomSerializedElementWriteStyle.Element, null, FunctionHasDuplicateNameError.FunctionDomainRoleId);
				FunctionDuplicateNameError.myCustomSerializedChildElementInfo = ret;
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
			if (roleId == FunctionHasDuplicateNameError.FunctionDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Function", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = FunctionDuplicateNameError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(FunctionHasDuplicateNameError.FunctionDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Functions||Function", match);
				FunctionDuplicateNameError.myChildElementMappings = childElementMappings;
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
	#endregion // FunctionDuplicateNameError serialization
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
		/// <summary>Implements ICustomSerializedElement.ShouldSerialize</summary>
		protected new bool ShouldSerialize()
		{
			return ((ICustomSerializedElement)this.ValueTypeInstance).ShouldSerialize();
		}
		bool ICustomSerializedElement.ShouldSerialize()
		{
			return this.ShouldSerialize();
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
		/// <summary>Implements ICustomSerializedElement.ShouldSerialize</summary>
		protected new bool ShouldSerialize()
		{
			return ((ICustomSerializedElement)this.ObjectTypeInstance).ShouldSerialize();
		}
		bool ICustomSerializedElement.ShouldSerialize()
		{
			return this.ShouldSerialize();
		}
	}
	#endregion // PopulationMandatoryError serialization
	#region ObjectifiedInstanceRequiredError serialization
	partial class ObjectifiedInstanceRequiredError : ICustomSerializedElement
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
			if (roleId == ObjectifyingInstanceHasObjectifiedInstanceRequiredError.ObjectTypeInstanceDomainRoleId)
			{
				string name = "EntityTypeInstance";
				if (this.ObjectTypeInstance is EntityTypeSubtypeInstance)
				{
					name = "EntityTypeSubtypeInstance";
				}
				return new CustomSerializedElementInfo(null, name, null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ObjectifiedInstanceRequiredError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ObjectifyingInstanceHasObjectifiedInstanceRequiredError.ObjectTypeInstanceDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|EntityTypeInstance", match);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|EntityTypeSubtypeInstance", match);
				ObjectifiedInstanceRequiredError.myChildElementMappings = childElementMappings;
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
		/// <summary>Implements ICustomSerializedElement.ShouldSerialize</summary>
		protected new bool ShouldSerialize()
		{
			return ((ICustomSerializedElement)this.ObjectTypeInstance).ShouldSerialize();
		}
		bool ICustomSerializedElement.ShouldSerialize()
		{
			return this.ShouldSerialize();
		}
	}
	#endregion // ObjectifiedInstanceRequiredError serialization
	#region ObjectifyingInstanceRequiredError serialization
	partial class ObjectifyingInstanceRequiredError : ICustomSerializedElement
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
			if (roleId == ObjectifiedInstanceHasObjectifyingInstanceRequiredError.FactTypeInstanceDomainRoleId)
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ObjectifyingInstanceRequiredError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ObjectifiedInstanceHasObjectifyingInstanceRequiredError.FactTypeInstanceDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|FactTypeInstance", match);
				ObjectifyingInstanceRequiredError.myChildElementMappings = childElementMappings;
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
	#endregion // ObjectifyingInstanceRequiredError serialization
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
	#region JoinPathRequiredError serialization
	partial class JoinPathRequiredError : ICustomSerializedElement
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
			if (roleId == ConstraintRoleSequenceHasJoinPathRequiredError.RoleSequenceDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ConstraintRoleSequence", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = JoinPathRequiredError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ConstraintRoleSequenceHasJoinPathRequiredError.RoleSequenceDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ConstraintRoleSequence", match);
				JoinPathRequiredError.myChildElementMappings = childElementMappings;
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
	#endregion // JoinPathRequiredError serialization
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
	#region FrequencyConstraintViolatedByUniquenessConstraintError serialization
	partial class FrequencyConstraintViolatedByUniquenessConstraintError : ICustomSerializedElement
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
			if (roleId == FrequencyConstraintHasFrequencyConstraintViolatedByUniquenessConstraintError.FrequencyConstraintDomainRoleId)
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = FrequencyConstraintViolatedByUniquenessConstraintError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(FrequencyConstraintHasFrequencyConstraintViolatedByUniquenessConstraintError.FrequencyConstraintDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|FrequencyConstraint", match);
				FrequencyConstraintViolatedByUniquenessConstraintError.myChildElementMappings = childElementMappings;
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
	#endregion // FrequencyConstraintViolatedByUniquenessConstraintError serialization
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
	#region PathRequiresRootObjectTypeError serialization
	partial class PathRequiresRootObjectTypeError : ICustomSerializedElement
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
			if (roleId == RolePathHasRootObjectTypeError.RolePathDomainRoleId)
			{
				string name = "RolePath";
				if (this.RolePath is RoleSubPath)
				{
					name = "SubPath";
				}
				return new CustomSerializedElementInfo(null, name, null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = PathRequiresRootObjectTypeError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(RolePathHasRootObjectTypeError.RolePathDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|RolePath", match);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|SubPath", match);
				PathRequiresRootObjectTypeError.myChildElementMappings = childElementMappings;
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
	#endregion // PathRequiresRootObjectTypeError serialization
	#region JoinedPathRoleRequiresCompatibleRolePlayerError serialization
	partial class JoinedPathRoleRequiresCompatibleRolePlayerError : ICustomSerializedElement
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
			if (roleId == PathedRoleHasCompatibleJoinRolePlayerError.PathedRoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "PathedRole", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = JoinedPathRoleRequiresCompatibleRolePlayerError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(PathedRoleHasCompatibleJoinRolePlayerError.PathedRoleDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|PathedRole", match);
				JoinedPathRoleRequiresCompatibleRolePlayerError.myChildElementMappings = childElementMappings;
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
	#endregion // JoinedPathRoleRequiresCompatibleRolePlayerError serialization
	#region PathObjectUnifierRequiresCompatibleObjectTypesError serialization
	partial class PathObjectUnifierRequiresCompatibleObjectTypesError : ICustomSerializedElement
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
				return new CustomSerializedElementInfo(null, "ObjectUnifierRequiresCompatibleObjectTypesError", null, CustomSerializedElementWriteStyle.Element, null);
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
			if (roleId == PathObjectUnifierHasCompatibleObjectTypesError.ObjectUnifierDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ObjectUnifier", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = PathObjectUnifierRequiresCompatibleObjectTypesError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(PathObjectUnifierHasCompatibleObjectTypesError.ObjectUnifierDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ObjectUnifier", match);
				PathObjectUnifierRequiresCompatibleObjectTypesError.myChildElementMappings = childElementMappings;
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
	#endregion // PathObjectUnifierRequiresCompatibleObjectTypesError serialization
	#region PathSameFactTypeRoleFollowsJoinError serialization
	partial class PathSameFactTypeRoleFollowsJoinError : ICustomSerializedElement
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
			if (roleId == PathedRoleHasSameFactTypeFollowsJoinError.PathedRoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "PathedRole", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = PathSameFactTypeRoleFollowsJoinError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(PathedRoleHasSameFactTypeFollowsJoinError.PathedRoleDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|PathedRole", match);
				PathSameFactTypeRoleFollowsJoinError.myChildElementMappings = childElementMappings;
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
	#endregion // PathSameFactTypeRoleFollowsJoinError serialization
	#region PathOuterJoinRequiresOptionalRoleError serialization
	partial class PathOuterJoinRequiresOptionalRoleError : ICustomSerializedElement
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
			if (roleId == PathedRoleHasMandatoryOuterJoinError.PathedRoleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "PathedRole", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = PathOuterJoinRequiresOptionalRoleError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(PathedRoleHasMandatoryOuterJoinError.PathedRoleDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|PathedRole", match);
				PathOuterJoinRequiresOptionalRoleError.myChildElementMappings = childElementMappings;
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
	#endregion // PathOuterJoinRequiresOptionalRoleError serialization
	#region CalculatedPathValueRequiresFunctionError serialization
	partial class CalculatedPathValueRequiresFunctionError : ICustomSerializedElement
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
			if (roleId == CalculatedPathValueHasFunctionRequiredError.CalculatedPathValueDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "CalculatedValue", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = CalculatedPathValueRequiresFunctionError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(CalculatedPathValueHasFunctionRequiredError.CalculatedPathValueDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|CalculatedValue", match);
				CalculatedPathValueRequiresFunctionError.myChildElementMappings = childElementMappings;
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
	#endregion // CalculatedPathValueRequiresFunctionError serialization
	#region CalculatedPathValueMustBeConsumedError serialization
	partial class CalculatedPathValueMustBeConsumedError : ICustomSerializedElement
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
			if (roleId == CalculatedPathValueHasConsumptionRequiredError.CalculatedPathValueDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "CalculatedValue", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = CalculatedPathValueMustBeConsumedError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(CalculatedPathValueHasConsumptionRequiredError.CalculatedPathValueDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|CalculatedValue", match);
				CalculatedPathValueMustBeConsumedError.myChildElementMappings = childElementMappings;
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
	#endregion // CalculatedPathValueMustBeConsumedError serialization
	#region CalculatedPathValueParameterBindingError serialization
	partial class CalculatedPathValueParameterBindingError : ICustomSerializedElement
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
			if (roleId == CalculatedPathValueHasUnboundParameterError.CalculatedPathValueDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "CalculatedValue", null, CustomSerializedElementWriteStyle.PrimaryLinkElement, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = CalculatedPathValueParameterBindingError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(CalculatedPathValueHasUnboundParameterError.CalculatedPathValueDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|CalculatedValue", match);
				CalculatedPathValueParameterBindingError.myChildElementMappings = childElementMappings;
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
	#endregion // CalculatedPathValueParameterBindingError serialization
	#region CalculatedPathValueHasUnboundParameterError serialization
	partial class CalculatedPathValueHasUnboundParameterError : ICustomSerializedElement
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
			if (roleId == CalculatedPathValueParameterBindingErrorTargetsFunctionParameter.ParameterDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Parameter", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = CalculatedPathValueHasUnboundParameterError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(CalculatedPathValueParameterBindingErrorTargetsFunctionParameter.ParameterDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Parameter", match);
				CalculatedPathValueHasUnboundParameterError.myChildElementMappings = childElementMappings;
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
	#endregion // CalculatedPathValueHasUnboundParameterError serialization
	#region FactTypeDerivationRequiresProjectionError serialization
	partial class FactTypeDerivationRequiresProjectionError : ICustomSerializedElement
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
			if (roleId == FactTypeDerivationRuleHasProjectionRequiredError.DerivationRuleDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "FactTypeDerivationPath", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = FactTypeDerivationRequiresProjectionError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(FactTypeDerivationRuleHasProjectionRequiredError.DerivationRuleDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|FactTypeDerivationPath", match);
				FactTypeDerivationRequiresProjectionError.myChildElementMappings = childElementMappings;
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
	#endregion // FactTypeDerivationRequiresProjectionError serialization
	#region PartialFactTypeDerivationProjectionError serialization
	partial class PartialFactTypeDerivationProjectionError : ICustomSerializedElement
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
			if (roleId == FactTypeDerivationProjectionHasPartialProjectionError.DerivationProjectionDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "DerivationProjection", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = PartialFactTypeDerivationProjectionError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(FactTypeDerivationProjectionHasPartialProjectionError.DerivationProjectionDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|DerivationProjection", match);
				PartialFactTypeDerivationProjectionError.myChildElementMappings = childElementMappings;
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
	#endregion // PartialFactTypeDerivationProjectionError serialization
	#region ConstraintRoleSequenceJoinPathRequiresProjectionError serialization
	partial class ConstraintRoleSequenceJoinPathRequiresProjectionError : ICustomSerializedElement
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
				return new CustomSerializedElementInfo(null, "JoinPathRequiresProjectionError", null, CustomSerializedElementWriteStyle.Element, null);
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
			if (roleId == ConstraintRoleSequenceJoinPathHasProjectionRequiredError.JoinPathDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "JoinPath", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ConstraintRoleSequenceJoinPathRequiresProjectionError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ConstraintRoleSequenceJoinPathHasProjectionRequiredError.JoinPathDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|JoinPath", match);
				ConstraintRoleSequenceJoinPathRequiresProjectionError.myChildElementMappings = childElementMappings;
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
	#endregion // ConstraintRoleSequenceJoinPathRequiresProjectionError serialization
	#region PartialConstraintRoleSequenceJoinPathProjectionError serialization
	partial class PartialConstraintRoleSequenceJoinPathProjectionError : ICustomSerializedElement
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
				return new CustomSerializedElementInfo(null, "PartialJoinPathProjectionError", null, CustomSerializedElementWriteStyle.Element, null);
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
			if (roleId == ConstraintRoleSequenceProjectionHasPartialProjectionError.JoinPathProjectionDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "JoinPathProjection", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = PartialConstraintRoleSequenceJoinPathProjectionError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ConstraintRoleSequenceProjectionHasPartialProjectionError.JoinPathProjectionDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|JoinPathProjection", match);
				PartialConstraintRoleSequenceJoinPathProjectionError.myChildElementMappings = childElementMappings;
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
	#endregion // PartialConstraintRoleSequenceJoinPathProjectionError serialization
	#region ElementGroupingSet serialization
	sealed partial class ElementGroupingSet : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		CustomSerializedElementSupportedOperations ICustomSerializedElement.SupportedCustomSerializedOperations
		{
			get
			{
				return CustomSerializedElementSupportedOperations.ChildElementInfo | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
			}
		}
		private static CustomSerializedContainerElementInfo[] myCustomSerializedChildElementInfo;
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedChildElementInfo</summary>
		CustomSerializedContainerElementInfo[] ICustomSerializedElement.GetCustomSerializedChildElementInfo()
		{
			CustomSerializedContainerElementInfo[] ret = ElementGroupingSet.myCustomSerializedChildElementInfo;
			if (ret == null)
			{
				ret = new CustomSerializedContainerElementInfo[1];
				ret[0] = new CustomSerializedContainerElementInfo(null, "Groups", null, CustomSerializedElementWriteStyle.Element, null, ElementGroupingSetContainsElementGrouping.GroupingDomainRoleId);
				ElementGroupingSet.myCustomSerializedChildElementInfo = ret;
			}
			return ret;
		}
		/// <summary>Implements ICustomSerializedElement.CustomSerializedElementInfo</summary>
		CustomSerializedElementInfo ICustomSerializedElement.CustomSerializedElementInfo
		{
			get
			{
				return new CustomSerializedElementInfo(null, "Grouping", null, CustomSerializedElementWriteStyle.Element, null);
			}
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedPropertyInfo</summary>
		CustomSerializedPropertyInfo ICustomSerializedElement.GetCustomSerializedPropertyInfo(DomainPropertyInfo domainPropertyInfo, DomainRoleInfo rolePlayedInfo)
		{
			throw new NotSupportedException();
		}
		/// <summary>Implements ICustomSerializedElement.GetCustomSerializedLinkInfo</summary>
		CustomSerializedElementInfo ICustomSerializedElement.GetCustomSerializedLinkInfo(DomainRoleInfo rolePlayedInfo, ElementLink elementLink)
		{
			Guid roleId = rolePlayedInfo.Id;
			if (roleId == ElementGroupingSetRelatesToORMModel.ModelDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ORMModel", null, CustomSerializedElementWriteStyle.Element, null);
			}
			return CustomSerializedElementInfo.Default;
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
				domainRole = domainDataDirectory.FindDomainRole(ElementGroupingSetRelatesToORMModel.ModelDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(ElementGroupingSetContainsElementGrouping.GroupingDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
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
		IComparer<DomainRoleInfo> ICustomSerializedElement.CustomSerializedChildRoleComparer
		{
			get
			{
				IComparer<DomainRoleInfo> retVal = ElementGroupingSet.myCustomSortChildComparer;
				if (null == retVal)
				{
					retVal = new CustomSortChildComparer(this.Store);
					ElementGroupingSet.myCustomSortChildComparer = retVal;
				}
				return retVal;
			}
		}
		private static Dictionary<string, CustomSerializedElementMatch> myChildElementMappings;
		/// <summary>Implements ICustomSerializedElement.MapChildElement</summary>
		CustomSerializedElementMatch ICustomSerializedElement.MapChildElement(string elementNamespace, string elementName, string containerNamespace, string containerName, string outerContainerNamespace, string outerContainerName)
		{
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ElementGroupingSet.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ElementGroupingSetRelatesToORMModel.ModelDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|ORMModel", match);
				match.InitializeRoles(ElementGroupingSetContainsElementGrouping.GroupingDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Groups||", match);
				ElementGroupingSet.myChildElementMappings = childElementMappings;
			}
			CustomSerializedElementMatch rVal;
			childElementMappings.TryGetValue(string.Concat(outerContainerNamespace, "|", outerContainerName, "|", (object)containerNamespace != (object)outerContainerNamespace ? containerNamespace : null, "|", containerName, "|", (object)elementNamespace != (object)containerNamespace ? elementNamespace : null, "|", elementName), out rVal);
			return rVal;
		}
		/// <summary>Implements ICustomSerializedElement.MapAttribute</summary>
		Guid ICustomSerializedElement.MapAttribute(string xmlNamespace, string attributeName)
		{
			return default(Guid);
		}
		/// <summary>Implements ICustomSerializedElement.ShouldSerialize</summary>
		private static bool ShouldSerialize()
		{
			return true;
		}
		bool ICustomSerializedElement.ShouldSerialize()
		{
			return ShouldSerialize();
		}
	}
	#endregion // ElementGroupingSet serialization
	#region ElementGrouping serialization
	partial class ElementGrouping : ICustomSerializedElement
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
			CustomSerializedContainerElementInfo[] ret = ElementGrouping.myCustomSerializedChildElementInfo;
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
				ret = new CustomSerializedContainerElementInfo[baseInfoCount + 5];
				if (baseInfoCount != 0)
				{
					baseInfo.CopyTo(ret, 5);
				}
				ret[0] = new CustomSerializedContainerElementInfo(null, "Definitions", null, CustomSerializedElementWriteStyle.Element, null, ElementGroupingHasDefinition.DefinitionDomainRoleId);
				ret[1] = new CustomSerializedContainerElementInfo(null, "Notes", null, CustomSerializedElementWriteStyle.Element, null, ElementGroupingHasNote.NoteDomainRoleId);
				ret[2] = new CustomSerializedContainerElementInfo(null, "GroupTypes", null, CustomSerializedElementWriteStyle.Element, null, ElementGroupingIsOfElementGroupingType.GroupingTypeDomainRoleId);
				ret[3] = new CustomSerializedContainerElementInfo(null, "Elements", null, CustomSerializedElementWriteStyle.Element, null, GroupingElementInclusion.IncludedElementDomainRoleId, GroupingElementExclusion.ExcludedElementDomainRoleId);
				ret[4] = new CustomSerializedContainerElementInfo(null, "NestedGroups", null, CustomSerializedElementWriteStyle.Element, null, ElementGroupingIncludesElementGrouping.IncludedChildGroupingDomainRoleId, ElementGroupingExcludesElementGrouping.ExcludedChildGroupingDomainRoleId);
				ElementGrouping.myCustomSerializedChildElementInfo = ret;
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
				return new CustomSerializedElementInfo(null, "Group", null, CustomSerializedElementWriteStyle.Element, null);
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
			if (domainPropertyInfo.Id == ElementGrouping.TypeComplianceDomainPropertyId)
			{
				if (this.TypeCompliance == GroupingMembershipTypeCompliance.NotExcluded)
				{
					return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.NotWritten, null);
				}
				return new CustomSerializedPropertyInfo(null, null, null, false, CustomSerializedAttributeWriteStyle.Attribute, null);
			}
			if (domainPropertyInfo.Id == ElementGrouping.PriorityDomainPropertyId)
			{
				if (this.Priority == 0)
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
			if (roleId == GroupingElementInclusion.IncludedElementDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "IncludedElement", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == GroupingElementExclusion.ExcludedElementDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ExcludedElement", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == ElementGroupingIncludesElementGrouping.IncludedChildGroupingDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "IncludedGroup", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == ElementGroupingExcludesElementGrouping.ExcludedChildGroupingDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "ExcludedGroup", null, CustomSerializedElementWriteStyle.Element, null);
			}
			if (roleId == ElementGroupingHasDuplicateNameError.DuplicateNameErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ElementGroupingHasMembershipContradictionError.MembershipContradictionErrorDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ElementGroupingIncludesElementGrouping.ParentGroupingDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, null, null, CustomSerializedElementWriteStyle.NotWritten, null);
			}
			if (roleId == ElementGroupingExcludesElementGrouping.ParentGroupingDomainRoleId)
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
				domainRole = domainDataDirectory.FindDomainRole(ElementGroupingHasDefinition.DefinitionDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 0;
				domainRole = domainDataDirectory.FindDomainRole(ElementGroupingHasNote.NoteDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 1;
				domainRole = domainDataDirectory.FindDomainRole(ElementGroupingIsOfElementGroupingType.GroupingTypeDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 2;
				domainRole = domainDataDirectory.FindDomainRole(GroupingElementInclusion.IncludedElementDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 3;
				domainRole = domainDataDirectory.FindDomainRole(GroupingElementExclusion.ExcludedElementDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 4;
				domainRole = domainDataDirectory.FindDomainRole(ElementGroupingIncludesElementGrouping.IncludedChildGroupingDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 5;
				domainRole = domainDataDirectory.FindDomainRole(ElementGroupingExcludesElementGrouping.ExcludedChildGroupingDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 6;
				domainRole = domainDataDirectory.FindDomainRole(ElementGroupingHasDuplicateNameError.DuplicateNameErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 7;
				domainRole = domainDataDirectory.FindDomainRole(ElementGroupingHasMembershipContradictionError.MembershipContradictionErrorDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 8;
				domainRole = domainDataDirectory.FindDomainRole(ElementGroupingIncludesElementGrouping.ParentGroupingDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 9;
				domainRole = domainDataDirectory.FindDomainRole(ElementGroupingExcludesElementGrouping.ParentGroupingDomainRoleId).OppositeDomainRole;
				roleOrderDictionary[string.Concat(domainRole.DomainRelationship.ImplementationClass.FullName, ".", domainRole.Name)] = 10;
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
				IComparer<DomainRoleInfo> retVal = ElementGrouping.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					ElementGrouping.myCustomSortChildComparer = retVal;
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ElementGrouping.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(GroupingElementInclusion.IncludedElementDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Elements||IncludedElement", match);
				match.InitializeRoles(GroupingElementExclusion.ExcludedElementDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Elements||ExcludedElement", match);
				match.InitializeRoles(ElementGroupingIncludesElementGrouping.IncludedChildGroupingDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|NestedGroups||IncludedGroup", match);
				match.InitializeRoles(ElementGroupingExcludesElementGrouping.ExcludedChildGroupingDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|NestedGroups||ExcludedGroup", match);
				match.InitializeRoles(ElementGroupingHasDefinition.DefinitionDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Definitions||", match);
				match.InitializeRoles(ElementGroupingHasNote.NoteDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Notes||", match);
				match.InitializeRoles(ElementGroupingIsOfElementGroupingType.GroupingTypeDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|GroupTypes||", match);
				ElementGrouping.myChildElementMappings = childElementMappings;
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
			Dictionary<string, Guid> customSerializedAttributes = ElementGrouping.myCustomSerializedAttributes;
			if (customSerializedAttributes == null)
			{
				customSerializedAttributes = new Dictionary<string, Guid>();
				customSerializedAttributes.Add("TypeCompliance", ElementGrouping.TypeComplianceDomainPropertyId);
				customSerializedAttributes.Add("Priority", ElementGrouping.PriorityDomainPropertyId);
				ElementGrouping.myCustomSerializedAttributes = customSerializedAttributes;
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
	#endregion // ElementGrouping serialization
	#region ElementGroupingDuplicateNameError serialization
	partial class ElementGroupingDuplicateNameError : ICustomSerializedElement
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
			CustomSerializedContainerElementInfo[] ret = ElementGroupingDuplicateNameError.myCustomSerializedChildElementInfo;
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
				ret[0] = new CustomSerializedContainerElementInfo(null, "Groups", null, CustomSerializedElementWriteStyle.Element, null, ElementGroupingHasDuplicateNameError.GroupingDomainRoleId);
				ElementGroupingDuplicateNameError.myCustomSerializedChildElementInfo = ret;
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
				return new CustomSerializedElementInfo(null, "GroupDuplicateNameError", null, CustomSerializedElementWriteStyle.Element, null);
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
			if (roleId == ElementGroupingHasDuplicateNameError.GroupingDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Group", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ElementGroupingDuplicateNameError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ElementGroupingHasDuplicateNameError.GroupingDomainRoleId);
				childElementMappings.Add("||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Groups||Group", match);
				ElementGroupingDuplicateNameError.myChildElementMappings = childElementMappings;
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
	#endregion // ElementGroupingDuplicateNameError serialization
	#region ElementGroupingMembershipContradictionError serialization
	partial class ElementGroupingMembershipContradictionError : ICustomSerializedElement
	{
		/// <summary>Implements ICustomSerializedElement.SupportedCustomSerializedOperations</summary>
		protected new CustomSerializedElementSupportedOperations SupportedCustomSerializedOperations
		{
			get
			{
				return base.SupportedCustomSerializedOperations | CustomSerializedElementSupportedOperations.ElementInfo | CustomSerializedElementSupportedOperations.LinkInfo | CustomSerializedElementSupportedOperations.CustomSortChildRoles;
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
				return new CustomSerializedElementInfo(null, "GroupMembershipContradictionError", null, CustomSerializedElementWriteStyle.Element, null);
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
			if (roleId == ElementGroupingHasMembershipContradictionError.GroupingDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Group", null, CustomSerializedElementWriteStyle.PrimaryLinkElement, null);
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
				domainRole = domainDataDirectory.FindDomainRole(ElementGroupingHasMembershipContradictionError.GroupingDomainRoleId).OppositeDomainRole;
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
				IComparer<DomainRoleInfo> retVal = ElementGroupingMembershipContradictionError.myCustomSortChildComparer;
				if (null == retVal)
				{
					IComparer<DomainRoleInfo> baseComparer = null;
					if (0 != (CustomSerializedElementSupportedOperations.CustomSortChildRoles & base.SupportedCustomSerializedOperations))
					{
						baseComparer = base.CustomSerializedChildRoleComparer;
					}
					retVal = new CustomSortChildComparer(this.Store, baseComparer);
					ElementGroupingMembershipContradictionError.myCustomSortChildComparer = retVal;
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ElementGroupingMembershipContradictionError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(ElementGroupingHasMembershipContradictionError.GroupingDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Group", match);
				ElementGroupingMembershipContradictionError.myChildElementMappings = childElementMappings;
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
	#endregion // ElementGroupingMembershipContradictionError serialization
	#region ElementGroupingHasMembershipContradictionError serialization
	partial class ElementGroupingHasMembershipContradictionError : ICustomSerializedElement
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
			if (roleId == GroupingMembershipContradictionErrorIsForElement.ElementDomainRoleId)
			{
				return new CustomSerializedElementInfo(null, "Element", null, CustomSerializedElementWriteStyle.Element, null);
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
			Dictionary<string, CustomSerializedElementMatch> childElementMappings = ElementGroupingHasMembershipContradictionError.myChildElementMappings;
			if (childElementMappings == null)
			{
				childElementMappings = new Dictionary<string, CustomSerializedElementMatch>();
				CustomSerializedElementMatch match = new CustomSerializedElementMatch();
				match.InitializeRoles(GroupingMembershipContradictionErrorIsForElement.ElementDomainRoleId);
				childElementMappings.Add("||||http://schemas.neumont.edu/ORM/2006-04/ORMCore|Element", match);
				ElementGroupingHasMembershipContradictionError.myChildElementMappings = childElementMappings;
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
	#endregion // ElementGroupingHasMembershipContradictionError serialization
}
