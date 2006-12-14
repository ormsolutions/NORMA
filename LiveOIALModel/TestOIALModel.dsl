<?xml version="1.0" encoding="utf-8"?>
<!--
	Neumont Object-Role Modeling Architect for Visual Studio

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
	Id="8C422562-FC92-4A83-9A6B-FE92D099FF06"
	Namespace="Neumont.Tools.ORM.TestOIALModel"
	Name="OIAL"
	DisplayName="LIVE ORM Intermediate Abstraction Language"
	Description="Extension rules and elements used to perform dynamic object absorption. Used by more compact views on the ORM model."
	CompanyName="Neumont University"
	ProductName="Neumont ORM Architect for Visual Studio"
	MajorVersion="1" MinorVersion="0" Build="0" Revision="0">

	<Attributes>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;3EAE649F-E654-4D04-8289-C25D2C0322D8&quot;/*Neumont.Tools.ORM.ObjectModel.ORMCoreDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
	</Attributes>
	
	<Classes>

		<DomainClass Name="OIALNamedElement" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="800B9524-E41C-4360-B06A-51005417283C" DisplayName="OIALNamedElement" InheritanceModifier="Abstract" Description="">
			<Properties>
				<DomainProperty Name="Name" DefaultValue="" DisplayName="Name" IsElementName="true" Id="34B63104-1E6D-457B-8029-609EBB9DF6A5">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="LiveOIALModel" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="0122B3E1-ECC2-4063-88D8-8E3FC25B01B9" DisplayName="OIALModel" Description="">
			<BaseClass>
				<DomainClassMoniker Name="OIALNamedElement"/>
			</BaseClass>
			<ElementMergeDirectives>
				<ElementMergeDirective UsesCustomAccept="false" UsesCustomMerge="false">
					<Index>
						<DomainClassMoniker Name="ConceptType"/>
					</Index>
					<LinkCreationPaths>
						<DomainPath>OIALModelHasConceptType.ConceptTypeCollection</DomainPath>
					</LinkCreationPaths>
				</ElementMergeDirective>
				<ElementMergeDirective UsesCustomAccept="false" UsesCustomMerge="false">
					<Index>
						<DomainClassMoniker Name="InformationTypeFormat"/>
					</Index>
					<LinkCreationPaths>
						<DomainPath>OIALHasInformationTypeFormat.InformationTypeFormatCollection</DomainPath>
					</LinkCreationPaths>
				</ElementMergeDirective>
				<ElementMergeDirective UsesCustomAccept="false" UsesCustomMerge="false">
					<Index>
						<DomainClassMoniker Name="ChildSequenceConstraint"/>
					</Index>
					<LinkCreationPaths>
						<DomainPath>OIALModelHasChildSequenceConstraint.ChildSequenceConstraintCollection</DomainPath>
					</LinkCreationPaths>
				</ElementMergeDirective>
			</ElementMergeDirectives>
		</DomainClass>

		<!--<DomainClass Name="ParentableType" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="5B25EF27-C3E0-4F71-8230-D155D8279926" DisplayName="ParentableType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="OIALNamedElement"/>
			</BaseClass>
		</DomainClass>-->
		
		<DomainClass Name="ConceptType" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="B3AD2FA0-CD80-4BCE-89D7-ADBE6D5089E0" DisplayName="ConceptType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="OIALNamedElement"/>
			</BaseClass>
		</DomainClass>

		<!--<DomainClass Name="InformationType" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="08BE24C4-C237-4CAC-B7F0-04C4F5E27758" DisplayName="InformationType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ParentableType"/>
			</BaseClass>
		</DomainClass>-->

		<DomainClass Name="InformationTypeFormat" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="6686689D-3D40-4E16-9CB4-0F58036A51B0" DisplayName="InformationTypeFormat" Description="">
			<BaseClass>
				<DomainClassMoniker Name="OIALNamedElement"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ChildSequence" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="F03F850B-9F09-46D8-B6C5-BCFA00FD0CD3" DisplayName="ChildSequence" Description="">
			<BaseClass>
				<DomainClassMoniker Name="OIALNamedElement"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="Constraint" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="E5D9E3E5-5931-4A1D-B138-615D2FD906CE" DisplayName="Constraint" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="OIALNamedElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Modality" DefaultValue="" DisplayName="Modality" Id="8EA5357B-66FA-4D61-A6C6-BD9ED091D131">
					<Type>
						<DomainEnumerationMoniker Name="/Neumont.Tools.ORM.ObjectModel/ConstraintModality"/>
					</Type>
				</DomainProperty>
			</Properties>

		</DomainClass>

		<DomainClass Name="SingleChildConstraint" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="67DB8A48-40A6-42BF-863D-78BAC1446AD3" DisplayName="SingleChildConstraint" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="Constraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ChildSequenceConstraint" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="E02AF95C-B70E-40DA-A827-4F8E16BA7B60" DisplayName="ChildSequenceConstraint" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="Constraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="SingleChildUniquenessConstraint" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="DCC774B9-CC66-437E-956E-6AB792F96089" DisplayName="SingleChildUniquenessConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SingleChildConstraint"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="IsPreferred" DefaultValue="False" DisplayName="IsPreferred" Id="30840715-E330-4FB5-ACA8-657155ED00E1">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>

		</DomainClass>

		<DomainClass Name="SingleChildSequenceConstraint" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="526A34FD-1C91-4184-B35D-A7DCF1DA42DE" DisplayName="SingleChildSequenceConstraint" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ChildSequenceConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="MultiChildSequenceConstraint" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="1D88E008-F00A-4055-9299-E80A0E460408" DisplayName="MultiChildSequenceConstraint" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ChildSequenceConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="SingleChildFrequencyConstraint" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="09B6B866-61F7-405D-9F3E-3F44A9BA2BA1" DisplayName="SingleChildFrequencyConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SingleChildConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ValueConstraint" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="4CE5276C-45F6-4D72-BEA4-9D0A6C37930C" DisplayName="ValueConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SingleChildConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ChildSequenceFrequencyConstraint" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="A7A3F837-4FF9-4423-A59A-F789E817250E" DisplayName="ChildSequenceFrequencyConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SingleChildSequenceConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="RingConstraint" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="50AFA697-8336-49DE-9F2C-4EF50D2DD560" DisplayName="RingConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SingleChildSequenceConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="DisjunctiveMandatoryConstraint" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="75D57A27-56F8-49EA-B509-3300B174FA4D" DisplayName="DisjunctiveMandatoryConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SingleChildSequenceConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ChildSequenceUniquenessConstraint" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="FBF2F9AF-5A37-4202-8412-A8D33780D615" DisplayName="ChildSequenceUniquenessConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SingleChildSequenceConstraint"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="ShouldIgnore" DefaultValue="False" DisplayName="ShouldIgnore" Id="66D9FE4B-AE01-4693-881B-30BAB3BB6082" Description="Describes whether views should ignore generating primary key notation for this constraint even though the &lt;see cref=&quot;P:Neumont.Tools.ORM.TestOIALModel.ChildSequenceUniquenessConstraint.IsPreferred&quot; /&gt; property may be true.">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="IsPreferred" DefaultValue="False" DisplayName="IsPreferred" Id="95371269-C2E4-417C-B567-58DA3C747CA0">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>

		</DomainClass>

		<DomainClass Name="MinTwoChildrenChildSequence" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="745E0167-6373-4D1D-A4E6-7C815CCEDAF1" DisplayName="MinTwoChildrenChildSequence" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ChildSequence"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="SubsetConstraint" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="1009F73D-4428-4365-87EA-81DF77095531" DisplayName="SubsetConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="MultiChildSequenceConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TwoOrMoreChildSequenceConstraint" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="4A149949-60DD-4165-AF58-F8C9A607C73D" DisplayName="TwoOrMoreChildSequenceConstraint" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="MultiChildSequenceConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ExclusionConstraint" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="C17C4731-EBFB-43E1-ABDD-E06D6FA81FB3" DisplayName="ExclusionConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="TwoOrMoreChildSequenceConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="EqualityConstraint" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="A6A27EE0-B4F0-46AD-AA46-4545F726BDF5" DisplayName="EqualityConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="TwoOrMoreChildSequenceConstraint"/>
			</BaseClass>
		</DomainClass>
		
	</Classes>

	<Relationships>
		<DomainRelationship Name="OIALModelHasORMModel" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="CE29D602-48DD-43F5-B5C3-7E1FE8FFB59F">

			<Source>
				<DomainRole Name="OIALModel" PropertyName="ORMModel" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="OIALModel" Id="528F3575-90C9-4AB1-805F-BD93852121DD">
					<RolePlayer>
						<DomainClassMoniker Name="LiveOIALModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ORMModel" PropertyName="OIALModel" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ORMModel" Id="A81B9158-0571-40F0-BB07-748F4076EC92">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ORMModel"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="OIALModelHasConceptType" Namespace="Neumont.Tools.ORM.TestOIALModel" IsEmbedding="true" Id="2BAD2B26-9947-4946-AF72-36388C98E2BD">

			<Source>
				<DomainRole Name="Model" PropertyName="ConceptTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="CEF05E31-BDF9-4AA4-ABCA-1E31D48F58DB">
					<RolePlayer>
						<DomainClassMoniker Name="LiveOIALModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ConceptType" PropertyName="Model" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ConceptType" Id="A904FCA0-DD58-4C5D-9725-878270924698">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConceptTypeChild" AllowsDuplicates="true" Namespace="Neumont.Tools.ORM.TestOIALModel" InheritanceModifier="Abstract" Id="0476605B-54F3-4051-8B68-E57480EAB0ED">
			<Properties>
				<DomainProperty Name="Mandatory" DefaultValue="NotMandatory" DisplayName="Mandatory" Id="2BE7593A-1E76-44F6-BC9F-5297096BCF4E">
					<Type>
						<DomainEnumerationMoniker Name="MandatoryConstraintModality"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="Name" DisplayName="Name" Id="16CC1655-0665-4D2D-A9B9-C8B0B47E2914" IsElementName="true">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
			<Source>
				<DomainRole Name="Parent" PropertyName="TargetCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="Parent" Id="18221BF0-A9BD-4F58-A91A-71D4C91561D0">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Target" PropertyName="Parent" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="Child" Id="4FB28AD5-BFB9-45A3-BE65-5887CF8E7026">
					<RolePlayer>
						<DomainClassMoniker Name="OIALNamedElement"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConceptTypeAbsorbedConceptType" Namespace="Neumont.Tools.ORM.TestOIALModel" IsEmbedding="true" Id="CD53823E-C08E-4858-B2C8-EF8DFB61A80C">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ConceptTypeChild" />
			</BaseRelationship>
			<Source>
				<DomainRole Name="AbsorbingConceptType" PropertyName="AbsorbedConceptTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="AbsorbingConceptType" Id="BF7F84BA-392D-4F72-A89C-34F0FF4588F1">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="AbsorbedConceptType" PropertyName="AbsorbingConceptType" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="AbsorbedConceptType" Id="FBB9EFB8-79F5-46AD-B9F0-2FAA406A5529">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="InformationType" AllowsDuplicates="true" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="94C404B9-8089-46F1-9B40-6C28A5B18E77">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ConceptTypeChild" />
			</BaseRelationship>
			<Source>
				<DomainRole Name="ConceptType" PropertyName="InformationTypeFormatCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ConceptType" Id="9C2FABE7-3993-4CF4-825D-EC05209DBE82">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="InformationTypeFormat" PropertyName="ConceptTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="InformationTypeFormat" Id="B5A1560B-40AD-4F0D-A749-61E9A00FCB87">
					<RolePlayer>
						<DomainClassMoniker Name="InformationTypeFormat"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="OIALHasInformationTypeFormat" Namespace="Neumont.Tools.ORM.TestOIALModel" IsEmbedding="true" Id="8AD40E7F-0688-4FA9-8787-4250E211B204">
			<Source>
				<DomainRole Name="Model" PropertyName="InformationTypeFormatCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="F9458392-7802-4CAF-9867-F9C2E17B1104">
					<RolePlayer>
						<DomainClassMoniker Name="LiveOIALModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="InformationTypeFormat" PropertyName="Model" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="InformationTypeFormat" Id="6D0CF1DC-11D7-402C-9A51-6CF96ED28398">
					<RolePlayer>
						<DomainClassMoniker Name="InformationTypeFormat"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConceptTypeRef" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="26728038-C906-4187-8EE4-F1CC14897FA0" AllowsDuplicates="true">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ConceptTypeChild" />
			</BaseRelationship>
			<Properties>
				<DomainProperty Name="OppositeName" DefaultValue="" DisplayName="OppositeName" Id="E5C68A11-4A68-44DF-A821-383A995A1A8E">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
			<Source>
				<DomainRole Name="ReferencingConceptType" PropertyName="ReferencedConceptTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ReferencingConceptType" Id="81B301E1-6B88-4F99-835C-58CC101A7A15">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ReferencedConceptType" PropertyName="ReferencingConceptType" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ReferencedConceptType" Id="E3EDD051-4E78-43DE-95BA-10206FB8AC2C">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="InformationTypeFormatHasObjectType" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="6256286A-B9A3-45AC-A4DD-6A6C063AE117">
			<Source>
				<DomainRole Name="InformationTypeFormat" PropertyName="ValueType" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="InformationTypeFormat" Id="DF9EA42F-A030-42A5-AC14-822C62AC946E">
					<RolePlayer>
						<DomainClassMoniker Name="InformationTypeFormat"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ValueType" PropertyName="InformationTypeFormat" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ValueType" Id="FF695482-1CBB-4FB8-87E1-B6571E1678D2">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConceptTypeHasObjectType" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="05367D79-7984-4861-98CA-DFB14D86C9EC">
			<Source>
				<DomainRole Name="ConceptType" PropertyName="ObjectType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ConceptType" Id="4238840E-E5A0-4477-8B04-402E72DF4369">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ObjectType" PropertyName="ConceptType" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ObjectType" Id="8414A2D9-9845-4588-A944-B7B8C60FDCFD">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ChildSequenceConstraintHasChildSequence" Namespace="Neumont.Tools.ORM.TestOIALModel" IsEmbedding="true" InheritanceModifier="Abstract" Id="7B7EC740-89BA-4278-B8C7-011F49D6A5A6">
			<Source>
				<DomainRole Name="ChildSequenceConstraint" PropertyName="ChildSequence" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="SingleChildSequenceConstraint" Id="A5088A66-A31C-4A79-A7E6-0114A9188C4B">
					<RolePlayer>
						<DomainClassMoniker Name="ChildSequenceConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ChildSequence" PropertyName="ChildSequenceConstraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="false" DisplayName="ChildSequence" Id="5CFF3A81-8ED1-46E3-B3FE-D8778BC4E67D">
					<RolePlayer>
						<DomainClassMoniker Name="ChildSequence"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SingleChildSequenceConstraintHasMinTwoChildrenChildSequence" Namespace="Neumont.Tools.ORM.TestOIALModel" IsEmbedding="true" Id="18EDCF00-CE25-493D-905E-6B9CC6A55501">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ChildSequenceConstraintHasChildSequence"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="SingleChildSequenceConstraint" PropertyName="ChildSequence" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SingleChildSequenceConstraint" Id="66C387E1-62F1-4E13-A683-345EFC9C1986">
					<RolePlayer>
						<DomainClassMoniker Name="SingleChildSequenceConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ChildSequence" PropertyName="SingleChildSequenceConstraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="false" DisplayName="ChildSequence" Id="AC96B1FF-286C-4D9B-BC81-5937D2F0655F">
					<RolePlayer>
						<DomainClassMoniker Name="MinTwoChildrenChildSequence"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SubsetConstraintHasSubChildSequence" Namespace="Neumont.Tools.ORM.TestOIALModel" IsEmbedding="true" Id="057B55A0-C0C6-483C-BA83-048767DE1320">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ChildSequenceConstraintHasChildSequence"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="SubsetConstraint" PropertyName="SubChildSequence" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SubsetConstraint" Id="17280110-2FFF-4A35-9E94-1EB24299DCA4">
					<RolePlayer>
						<DomainClassMoniker Name="SubsetConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="SubChildSequence" PropertyName="SubsetConstraint" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="false" DisplayName="SubChildSequence" Id="5EB18AAB-7A85-4B02-A168-2DBE427A666C">
					<RolePlayer>
						<DomainClassMoniker Name="ChildSequence"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SubsetConstraintHasSuperChildSequence" Namespace="Neumont.Tools.ORM.TestOIALModel" IsEmbedding="true" Id="9EF8CD6D-A013-43EA-A9EE-60515141F071">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ChildSequenceConstraintHasChildSequence"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="SubsetConstraint" PropertyName="SuperChildSequence" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SubsetConstraint" Id="EA2C4D8C-9CE5-4C2F-B2AB-395743728154">
					<RolePlayer>
						<DomainClassMoniker Name="SubsetConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="SuperChildSequence" PropertyName="SubsetConstraint" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="false" DisplayName="SuperChildSequence" Id="7D190DE2-1F60-4551-9B97-A3C03D30B032">
					<RolePlayer>
						<DomainClassMoniker Name="ChildSequence"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="TwoOrMoreChildSequenceConstraintHasChildSequence" Namespace="Neumont.Tools.ORM.TestOIALModel" IsEmbedding="true" Id="BFF06C7C-8460-436E-A357-D77C86452BD3">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ChildSequenceConstraintHasChildSequence"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="TwoOrMoreChildSequenceConstraint" PropertyName="ChildSequence" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="TwoOrMoreChildSequenceConstraint" Id="78EC7624-5480-4CBA-A93C-89B6851ACC09">
					<RolePlayer>
						<DomainClassMoniker Name="TwoOrMoreChildSequenceConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ChildSequence" PropertyName="TwoOrMoreChildSequenceConstraint" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="false" DisplayName="ChildSequence" Id="BEDB8959-2FB6-4C0A-A445-5057AD95485C">
					<RolePlayer>
						<DomainClassMoniker Name="ChildSequence"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ChildHasSingleChildConstraint" Namespace="Neumont.Tools.ORM.TestOIALModel" IsEmbedding="true" Id="22DAF21B-05D1-4916-942D-5B01C9493B9D">
			<Source>
				<DomainRole Name="ConceptTypeChild" PropertyName="SingleChildConstraintCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ConceptTypeChild" Id="7E9DDE0C-0710-4365-868B-866B383F12FF">
					<RolePlayer>
						<DomainRelationshipMoniker Name="ConceptTypeChild"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="SingleChildConstraint" PropertyName="ConceptTypeChild" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="SingleChildConstraint" Id="4EB1BCA8-2232-458D-8A4A-6A2132275FD1">
					<RolePlayer>
						<DomainClassMoniker Name="SingleChildConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="OIALModelHasChildSequenceConstraint" Namespace="Neumont.Tools.ORM.TestOIALModel" IsEmbedding="true" Id="DE228FEF-A466-41CF-B905-BE34264311F4">
			<Source>
				<DomainRole Name="OIALModel" PropertyName="ChildSequenceConstraintCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="OIALModel" Id="BB8A13B9-D197-4DBF-A95D-96204C8FA6EC">
					<RolePlayer>
						<DomainClassMoniker Name="LiveOIALModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ChildSequenceConstraint" PropertyName="OIALModel" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ChildSequenceConstraint" Id="EB00AFE6-6958-4F6C-B8EF-873A59D70106">
					<RolePlayer>
						<DomainClassMoniker Name="ChildSequenceConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConceptTypeChildHasPathRole" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="C2D63745-176D-48D7-8778-685E0B879F4A">
			<Source>
				<DomainRole Name="ConceptTypeChild" PropertyName="PathRoleCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ConceptTypeChild" Id="05234D98-EFE6-4097-B7B3-45DF5C75C533">
					<RolePlayer>
						<DomainRelationshipMoniker Name="ConceptTypeChild"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="PathRole" PropertyName="ConceptTypeChild" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="PathRole" Id="4F61716A-2DA2-43FB-83A4-98C71A1DCA3F">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/RoleBase"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ChildSequenceHasConceptTypeChild" Namespace="Neumont.Tools.ORM.TestOIALModel" Id="8718DCF7-02B9-4589-8FE0-9946EFE74B1E">
			<Source>
				<DomainRole Name="ChildSequence" PropertyName="ConceptTypeChildCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ChildSequence" Id="DFCAD5D0-B350-468F-819F-5BDFE9B628DF">
					<RolePlayer>
						<DomainClassMoniker Name="ChildSequence"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ConceptTypeChild" PropertyName="ChildSequenceCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ConceptTypeChild" Id="AF0F09AE-8382-442C-811F-C1B620B2020F">
					<RolePlayer>
						<DomainRelationshipMoniker Name="ConceptTypeChild"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

	</Relationships>

	<Types>
		<DomainEnumeration Namespace="Neumont.Tools.ORM.TestOIALModel" Name="MandatoryConstraintModality" Description="A list of constraint modalities for simple mandatory role constraints used in &lt;see cref=&quot;ConceptTypeChild&quot;/&gt; relationships.">
			<Literals>
				<EnumerationLiteral Name ="NotMandatory" Value="0" Description="See &lt;see langword=&quot;null&quot;/&gt;."/>
				<EnumerationLiteral Name ="Alethic" Value="1" Description="See &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.ConstraintModality.Alethic&quot;/&gt;."/>
				<EnumerationLiteral Name ="Deontic" Value="2" Description="See &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.ConstraintModality.Deontic&quot;/&gt;."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;MandatoryConstraintModality, global::Neumont.Tools.ORM.TestOIALModel.OIALDomainModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
	</Types>

	<XmlSerializationBehavior Name="OIALMetaModelSerializationBehavior" Namespace="Neumont.Tools.ORM.TestOIALModel"/>
	
</Dsl>
