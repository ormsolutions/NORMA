<?xml version="1.0" encoding="utf-8"?>
<!--
	Natural Object-Role Modeling Architect for Visual Studio

	Copyright © Neumont University. All rights reserved.

	The use and distribution terms for this software are covered by the
	Common Public License 1.0 (http://opensource.org/licenses/cpl) which
	can be found in the file CPL.txt at the root of this distribution.
	By using this software in any fashion, you are agreeing to be bound by
	the terms of this license.

	You must not remove this notice, or any other, from this software.
-->
<Dsl
	xmlns="http://schemas.microsoft.com/VisualStudio/2005/DslTools/DslDefinitionModel"
	xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
	PackageGuid="EFDDC549-1646-4451-8A51-E5A5E94D647C"
	Id="F7BC82F4-83D1-408C-BA42-607E90B23BEA"
	Namespace="ORMSolutions.ORMArchitect.ORMAbstraction"
	Name="Abstraction"
	DisplayName="ORM Abstraction Model"
	Description="Intermediate Attribute-centric View of ORM Model"
	CompanyName="ORM Solutions, LLC"
	ProductName="Natural Object-Role Modeling Architect for Visual Studio"
	MajorVersion="1" MinorVersion="0" Build="0" Revision="0">

	<Classes>
		<DomainClass Name="AbstractionModel" InheritanceModifier="Sealed" Namespace="ORMSolutions.ORMArchitect.ORMAbstraction" Id="9EF66BE3-C128-4642-9767-063244DE2CEF" DisplayName="AbstractionModel" Description="">
			<Properties>
				<DomainProperty Name="Name" DisplayName="Name" IsElementName="true" Id="49990808-B97E-4A72-8B26-8A8165CF4DF5">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
			<ElementMergeDirectives>
				<ElementMergeDirective UsesCustomAccept="false" UsesCustomMerge="false">
					<Index>
						<DomainClassMoniker Name="ConceptType"/>
					</Index>
					<LinkCreationPaths>
						<DomainPath>AbstractionModelHasConceptType.ConceptTypeCollection</DomainPath>
					</LinkCreationPaths>
				</ElementMergeDirective>
				<ElementMergeDirective UsesCustomAccept="false" UsesCustomMerge="false">
					<Index>
						<DomainClassMoniker Name="InformationTypeFormat"/>
					</Index>
					<LinkCreationPaths>
						<DomainPath>AbstractionModelHasInformationTypeFormat.InformationTypeFormatCollection</DomainPath>
					</LinkCreationPaths>
				</ElementMergeDirective>
			</ElementMergeDirectives>
		</DomainClass>

		<DomainClass Name="ConceptType" InheritanceModifier="Sealed" Namespace="ORMSolutions.ORMArchitect.ORMAbstraction" Id="A75DF497-8D38-4841-AAE3-341FD4ED234B" DisplayName="ConceptType" Description="">
			<Properties>
				<DomainProperty Name="Name" DisplayName="Name" IsElementName="true" Id="1193B616-AC5A-4179-862F-88E9243310A4">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<!-- TODO: This is going to need an odt:dataType property. -->
		<DomainClass Name="InformationTypeFormat" Namespace="ORMSolutions.ORMArchitect.ORMAbstraction" Id="7EB62327-99A6-4543-BE0D-8D4CED8C4F0E" DisplayName="InformationTypeFormat" Description="">
			<Properties>
				<DomainProperty Name="Name" DisplayName="Name" IsElementName="true" Id="E84BE6BF-9799-4C58-B58B-88DA7DC72FA0">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="Uniqueness" InheritanceModifier="Sealed" Namespace="ORMSolutions.ORMArchitect.ORMAbstraction" Id="0AF67F1F-66D6-4C2B-B85C-D556894AC300" DisplayName="Uniqueness" Description="">
			<Properties>
				<DomainProperty Name="Name" DisplayName="Name" IsElementName="true" Id="82013A44-5BAE-43AF-8472-A53473ADAC7E">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="IsPreferred" DefaultValue="false" DisplayName="IsPreferred" Id="FEEF8B5E-13BC-4C0B-8BD4-FFFC4A3764C0">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
	</Classes>

	<Relationships>
		<DomainRelationship Name="AbstractionModelHasConceptType" InheritanceModifier="Sealed" Namespace="ORMSolutions.ORMArchitect.ORMAbstraction" IsEmbedding="true" Id="35E02E9E-24DB-4DEf-B1CC-C8051F0E44A7">
			<Source>
				<DomainRole Name="Model" PropertyName="ConceptTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="1BC6D220-DDDD-4C8F-BFBD-944A84AF311F">
					<RolePlayer>
						<DomainClassMoniker Name="AbstractionModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ConceptType" PropertyName="Model" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ConceptType" Id="3502B0A1-AB2B-4C6C-8D2E-C284E4F4792A">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="AbstractionModelHasInformationTypeFormat" InheritanceModifier="Sealed" Namespace="ORMSolutions.ORMArchitect.ORMAbstraction" IsEmbedding="true" Id="2CDBA90A-2059-48A7-B272-21CEE9119C55">
			<Source>
				<DomainRole Name="Model" PropertyName="InformationTypeFormatCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="0E93BB32-8DDE-4AC6-A17A-259F42260FED">
					<RolePlayer>
						<DomainClassMoniker Name="AbstractionModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="InformationTypeFormat" PropertyName="Model" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="InformationTypeFormat" Id="4BE4BE54-FE23-4445-9DE2-15588D86B5E6">
					<RolePlayer>
						<DomainClassMoniker Name="InformationTypeFormat"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConceptTypeHasUniqueness" InheritanceModifier="Sealed" Namespace="ORMSolutions.ORMArchitect.ORMAbstraction" IsEmbedding="true" Id="356BCEA9-13E0-406D-BC0C-404909856A8F">
			<Source>
				<DomainRole Name="ConceptType" PropertyName="UniquenessCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ConceptType" Id="B425BC77-4990-40F6-8C6F-B670B134AD88">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Uniqueness" PropertyName="ConceptType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Uniqueness" Id="F10EF991-A973-4AD4-A79C-8C52E600181B">
					<RolePlayer>
						<DomainClassMoniker Name="Uniqueness"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConceptTypeChild" AllowsDuplicates="true" Namespace="ORMSolutions.ORMArchitect.ORMAbstraction" InheritanceModifier="Abstract" Id="96AFB439-E314-4D49-9011-18362F78D724">
			<Properties>
				<DomainProperty Name="IsMandatory" DefaultValue="false" DisplayName="IsMandatory" Id="72DE4B58-8890-4D57-A5A1-FBEE314A011F">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="Name"  DisplayName="Name" Id="DE8F10C3-DAC2-434E-8DBA-664A00676D94" IsElementName="true">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
			<Source>
				<DomainRole Name="Parent" PropertyName="TargetCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="Parent" Id="05B3A784-8BC2-43B0-8A07-FF5EEE0B5024">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Target" PropertyName="Parent" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="Child" Id="B00473B0-6E23-481D-9AEE-3785EEC35786">
					<RolePlayer>
						<DomainClassMoniker Name="/Microsoft.VisualStudio.Modeling/ModelElement"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConceptTypeHasChildAsPartOfAssociation" InheritanceModifier="Sealed" Namespace="ORMSolutions.ORMArchitect.ORMAbstraction" Id="0B5B8D09-E72B-484C-9A57-D4068FED9D65">
			<Source>
				<DomainRole Name="Parent" PropertyName="TargetCollection" Multiplicity="ZeroMany" PropagatesDelete="true" IsPropertyGenerator="false" DisplayName="Parent" Id="B6F725E8-2FE3-43B4-AA79-C88B1CE0B571">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Target" PropertyName="Parent" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="AssociationChild" Id="7CE7D4D7-D68C-464A-BED8-3B4796523E80">
					<RolePlayer>
						<DomainRelationshipMoniker Name="ConceptTypeChild"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="InformationType" InheritanceModifier="Sealed" AllowsDuplicates="true" Namespace="ORMSolutions.ORMArchitect.ORMAbstraction" Id="10DBC480-9DD5-47FB-8533-982C27985EE5">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ConceptTypeChild" />
			</BaseRelationship>
			<Source>
				<DomainRole Name="ConceptType" PropertyName="InformationTypeFormatCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ConceptType" Id="88ECA698-B81F-49A5-9945-E3A36697177B">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="InformationTypeFormat" PropertyName="ConceptTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="InformationTypeFormat" Id="62728BCA-C004-461F-ACC5-85213EE059DD">
					<RolePlayer>
						<DomainClassMoniker Name="InformationTypeFormat"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConceptTypeReferencesConceptType" AllowsDuplicates="true" InheritanceModifier="Abstract" Namespace="ORMSolutions.ORMArchitect.ORMAbstraction" Id="F003A8DA-4CD5-4208-B3D3-A51E98C5B962">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ConceptTypeChild" />
			</BaseRelationship>
			<Properties>
				<DomainProperty Name="OppositeName" DisplayName="OppositeName" Id="C83405D5-C96C-4B05-8A62-502FF5A82F10">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
			<Source>
				<DomainRole Name="ReferencingConceptType" PropertyName="ReferencedConceptTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ReferencingConceptType" Id="9F3E40BE-1EFA-4161-9627-E07D8603F34A">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ReferencedConceptType" PropertyName="ReferencingConceptTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ReferencedConceptType" Id="0541CDBD-87F7-4774-A4ED-DA61E06063AD">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConceptTypeRelatesToConceptType" InheritanceModifier="Sealed" AllowsDuplicates="true" Namespace="ORMSolutions.ORMArchitect.ORMAbstraction" Id="D8F2CBCD-1EB4-420A-9D18-5759D46B8AF3">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ConceptTypeReferencesConceptType"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="RelatingConceptType" PropertyName="RelatedConceptTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="RelatingConceptType" Id="A6B9687E-87BB-4696-BC23-AF7CB4046994">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="RelatedConceptType" PropertyName="RelatingConceptTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="RelatedConceptType" Id="CFEDC9CB-C50B-41D6-ACE8-432E67BD4B95">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConceptTypeAssimilatesConceptType" InheritanceModifier="Sealed" AllowsDuplicates="false" Namespace="ORMSolutions.ORMArchitect.ORMAbstraction" Id="A342DF5A-7426-4A9C-8263-3E24CBA2CF60">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ConceptTypeReferencesConceptType" />
			</BaseRelationship>
			<Properties>
				<DomainProperty Name="RefersToSubtype" DisplayName="RefersToSubtype" Id="34102AE8-807A-4A5A-8941-61C49ABDBCE8">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="IsPreferredForTarget" DisplayName="IsPreferredForTarget" Id="88FB045A-7458-440D-8B4C-64056E9352D0">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="IsPreferredForParent" DisplayName="IsPreferredForParent" Id="14F831E4-DA73-426B-9B5F-B026BE235505">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
			<Source>
				<DomainRole Name="AssimilatorConceptType" PropertyName="AssimilatedConceptTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="AssimilatorConceptType" Id="0985DA17-C1F4-4C51-A520-5D8D9D1C6695">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="AssimilatedConceptType" PropertyName="AssimilatorConceptTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="AssimilatedConceptType" Id="556C4DD3-245A-4283-A818-1F98BDA4630D">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<!-- TODO: Target should be restricted to ConceptTypeRelatesToConceptType and InformationType. -->
		<DomainRelationship Name="UniquenessIncludesConceptTypeChild" InheritanceModifier="Sealed" Namespace="ORMSolutions.ORMArchitect.ORMAbstraction" Id="C06E9C7D-89D3-433F-8B8A-FC80E3878DD5">
			<Source>
				<DomainRole Name="Uniqueness" PropertyName="ConceptTypeChildCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Uniqueness" Id="8596E430-AAFB-47CF-9AA2-FE6ABE50468D">
					<RolePlayer>
						<DomainClassMoniker Name="Uniqueness"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ConceptTypeChild" PropertyName="UniquenessCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ConceptTypeChild" Id="2A135454-E3B4-44B1-BD75-A5FFB5AAFF7A">
					<RolePlayer>
						<DomainRelationshipMoniker Name="ConceptTypeChild"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
	</Relationships>

	<XmlSerializationBehavior Name="AbstractionDomainModelSerializationBehavior" Namespace="ORMSolutions.ORMArchitect.ORMAbstraction"/>

</Dsl>