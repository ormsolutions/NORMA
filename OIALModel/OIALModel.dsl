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
	Id="CD96AA55-FCBC-47D0-93F8-30D3DACC5FF7"
	Namespace="Neumont.Tools.ORM.OIALModel"
	Name="OIAL"
	DisplayName="ORM Intermediate Abstraction Language"
	Description="Extension rules and elements used to perform dynamic object absorption. Used by more compact views on the ORM model."
	CompanyName="Neumont University"
	ProductName="Neumont ORM Architect for Visual Studio"
	MajorVersion="1" MinorVersion="0" Build="0" Revision="0">

	<Attributes>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;3EAE649F-E654-4D04-8289-C25D2C0322D8&quot;/*Neumont.Tools.ORM.ObjectModel.ORMCoreModel*/"/>
			</Parameters>
		</ClrAttribute>
	</Attributes>
	
	<Classes>

		<DomainClass Name="OIALNamedElement" Namespace="Neumont.Tools.ORM.OIALModel" Id="AA4C1802-DD46-43F6-A580-724CDD7ED5EB" DisplayName="OIALNamedElement" InheritanceModifier="Abstract" Description="">
			<Properties>
				<DomainProperty Name="Name" DefaultValue="" DisplayName="Name" IsElementName="true" Id="EC43BA80-66B1-4F5B-8AE4-BA49B91051E3">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="OIALModel" Namespace="Neumont.Tools.ORM.OIALModel" Id="8BDC608E-901D-44F5-B6B6-18E5199E9768" DisplayName="OIALModel" Description="">
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

		<DomainClass Name="ParentableType" Namespace="Neumont.Tools.ORM.OIALModel" Id="5B25EF27-C3E0-4F71-8230-D155D8279926" DisplayName="ParentableType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="OIALNamedElement"/>
			</BaseClass>
		</DomainClass>
		
		<DomainClass Name="ConceptType" Namespace="Neumont.Tools.ORM.OIALModel" Id="AC397C05-9E1A-4BED-BFC8-82D3C1DED36D" DisplayName="ConceptType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ParentableType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="InformationType" Namespace="Neumont.Tools.ORM.OIALModel" Id="08BE24C4-C237-4CAC-B7F0-04C4F5E27758" DisplayName="InformationType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ParentableType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="InformationTypeFormat" Namespace="Neumont.Tools.ORM.OIALModel" Id="8D5E029E-4A1A-4AB2-A222-5D727C32F3F5" DisplayName="InformationTypeFormat" Description="">
			<BaseClass>
				<DomainClassMoniker Name="OIALNamedElement"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ChildSequence" Namespace="Neumont.Tools.ORM.OIALModel" Id="AEFB7B15-F7D4-4D82-A1E2-000526723265" DisplayName="ChildSequence" Description="">
			<BaseClass>
				<DomainClassMoniker Name="OIALNamedElement"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="Constraint" Namespace="Neumont.Tools.ORM.OIALModel" Id="AA42253E-5FF3-48C7-AA9C-866B6155D99A" DisplayName="Constraint" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="OIALNamedElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Modality" DefaultValue="" DisplayName="Modality" Id="5CCE8E32-9FB1-487F-9249-357584F06F7C">
					<Type>
						<DomainEnumerationMoniker Name="/Neumont.Tools.ORM.ObjectModel/ConstraintModality"/>
					</Type>
				</DomainProperty>
			</Properties>

		</DomainClass>

		<DomainClass Name="SingleChildConstraint" Namespace="Neumont.Tools.ORM.OIALModel" Id="48459459-BA21-451D-B433-53C3AB892719" DisplayName="SingleChildConstraint" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="Constraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ChildSequenceConstraint" Namespace="Neumont.Tools.ORM.OIALModel" Id="35FB67C9-7F1C-475D-BE7F-9D55071AF0A8" DisplayName="ChildSequenceConstraint" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="Constraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="SingleChildUniquenessConstraint" Namespace="Neumont.Tools.ORM.OIALModel" Id="D52E7116-46B7-4F80-9071-36F30541CDA5" DisplayName="SingleChildUniquenessConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SingleChildConstraint"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="IsPreferred" DefaultValue="False" DisplayName="IsPreferred" Id="65B13D34-BDD7-4AAB-BF8A-EF9C0AF33687">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>

		</DomainClass>

		<DomainClass Name="SingleChildSequenceConstraint" Namespace="Neumont.Tools.ORM.OIALModel" Id="D6BB5AD8-13D9-4FBF-A62C-A8FCD26AFD2B" DisplayName="SingleChildSequenceConstraint" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ChildSequenceConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="MultiChildSequenceConstraint" Namespace="Neumont.Tools.ORM.OIALModel" Id="C110669A-73E3-457D-A99E-7C6A1322C482" DisplayName="MultiChildSequenceConstraint" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ChildSequenceConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="SingleChildFrequencyConstraint" Namespace="Neumont.Tools.ORM.OIALModel" Id="469CA5B0-3512-4413-819B-56C16A82BD05" DisplayName="SingleChildFrequencyConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SingleChildConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ValueConstraint" Namespace="Neumont.Tools.ORM.OIALModel" Id="7615A579-C490-43D8-B50F-3A5D5E5530C5" DisplayName="ValueConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SingleChildConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ChildSequenceFrequencyConstraint" Namespace="Neumont.Tools.ORM.OIALModel" Id="7CF44EAC-A8F9-4E67-98FD-537988B59BA0" DisplayName="ChildSequenceFrequencyConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SingleChildSequenceConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="RingConstraint" Namespace="Neumont.Tools.ORM.OIALModel" Id="96301EC3-A829-4BB8-A5BB-705801778612" DisplayName="RingConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SingleChildSequenceConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="DisjunctiveMandatoryConstraint" Namespace="Neumont.Tools.ORM.OIALModel" Id="37BE6A6C-12C5-446B-A229-02155A4CA85B" DisplayName="DisjunctiveMandatoryConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SingleChildSequenceConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ChildSequenceUniquenessConstraint" Namespace="Neumont.Tools.ORM.OIALModel" Id="2BF1BCF6-F1AC-48C5-B996-EF72D9DE6B1E" DisplayName="ChildSequenceUniquenessConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SingleChildSequenceConstraint"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="IsPreferred" DefaultValue="False" DisplayName="IsPreferred" Id="0EEBF253-C79B-45D3-A975-F3F59B5603CA">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>

		</DomainClass>

		<DomainClass Name="MinTwoChildrenChildSequence" Namespace="Neumont.Tools.ORM.OIALModel" Id="03805483-888A-448E-82FD-0104034870F5" DisplayName="MinTwoChildrenChildSequence" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ChildSequence"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="SubsetConstraint" Namespace="Neumont.Tools.ORM.OIALModel" Id="BE0D381D-8A53-402C-9D4A-DD2032460EC2" DisplayName="SubsetConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="MultiChildSequenceConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TwoOrMoreChildSequenceConstraint" Namespace="Neumont.Tools.ORM.OIALModel" Id="66DEE528-C04E-46A8-8704-842377F5064A" DisplayName="TwoOrMoreChildSequenceConstraint" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="MultiChildSequenceConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ExclusionConstraint" Namespace="Neumont.Tools.ORM.OIALModel" Id="8A090D5D-D2C4-46A4-9DD4-4193CC17B57A" DisplayName="ExclusionConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="TwoOrMoreChildSequenceConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="EqualityConstraint" Namespace="Neumont.Tools.ORM.OIALModel" Id="4FCDFBBE-7021-4AF1-A5D7-E34B778122F4" DisplayName="EqualityConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="TwoOrMoreChildSequenceConstraint"/>
			</BaseClass>
		</DomainClass>
		
	</Classes>

	<Relationships>
		<DomainRelationship Name="OIALModelHasORMModel" Namespace="Neumont.Tools.ORM.OIALModel" Id="7B04B2C6-4DFE-45E7-A7DF-BEA7ABF36E77">

			<Source>
				<DomainRole Name="OIALModel" PropertyName="ORMModel" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="OIALModel" Id="9069D6BD-0771-463A-B9CB-2C40372C53D7">
					<RolePlayer>
						<DomainClassMoniker Name="OIALModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ORMModel" PropertyName="OIALModel" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ORMModel" Id="CE948D50-A437-4172-B16C-74EB8E59E9B5">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ORMModel"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="OIALModelHasConceptType" Namespace="Neumont.Tools.ORM.OIALModel" IsEmbedding="true" Id="D82D29A8-A4EE-4650-AA33-0AA10DBB5352">

			<Source>
				<DomainRole Name="Model" PropertyName="ConceptTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="362A25D7-B320-47ED-BC01-FE7B2BB7BA83">
					<RolePlayer>
						<DomainClassMoniker Name="OIALModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ConceptType" PropertyName="Model" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ConceptType" Id="D2B0DFD2-EA25-4CBB-9884-8AE4C6FC7410">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConceptTypeHasChild" AllowsDuplicates="true" Namespace="Neumont.Tools.ORM.OIALModel" InheritanceModifier="Abstract" Id="7ACC3B69-DC50-4E97-897F-A25C4DD39E48">
			<Properties>
				<DomainProperty Name="Mandatory" DefaultValue="NotMandatory" DisplayName="Mandatory" Id="15E5A8F5-BDB7-44DB-80E0-F5D45872B8F6">
					<Type>
						<DomainEnumerationMoniker Name="MandatoryConstraintModality"/>
					</Type>
				</DomainProperty>
			</Properties>
			<Source>
				<DomainRole Name="Parent" PropertyName="ChildCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="Parent" Id="186C90D1-9393-4093-AB9F-4A8B979D32ED">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Child" PropertyName="Parent" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="Child" Id="7BE85312-15F4-447B-A53C-BFBBA2ED03BA">
					<RolePlayer>
						<DomainClassMoniker Name="ParentableType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConceptTypeAbsorbedConceptType" Namespace="Neumont.Tools.ORM.OIALModel" IsEmbedding="true" Id="91A27060-1738-419B-9B52-AFD3D924E1CA">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ConceptTypeHasChild" />
			</BaseRelationship>
			<Source>
				<DomainRole Name="AbsorbingConceptType" PropertyName="AbsorbedConceptTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="AbsorbingConceptType" Id="192EF6B2-48A9-4222-9D7E-321B2F446C06">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="AbsorbedConceptType" PropertyName="AbsorbingConceptType" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="AbsorbedConceptType" Id="1B266D6C-99C0-466B-8F34-8FFEEF56DDF7">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConceptTypeHasInformationType" Namespace="Neumont.Tools.ORM.OIALModel" IsEmbedding="true" Id="AFD486DA-57A4-4A81-865A-CC563F86F733">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ConceptTypeHasChild" />
			</BaseRelationship>
			<Source>
				<DomainRole Name="ConceptType" PropertyName="InformationTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ConceptType" Id="FE66EF7B-D877-4633-AE3C-501B682FABDB">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="InformationType" PropertyName="ConceptType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="InformationType" Id="53308CB9-6672-4C19-A5E9-6F07BC449C2F">
					<RolePlayer>
						<DomainClassMoniker Name="InformationType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="OIALHasInformationTypeFormat" Namespace="Neumont.Tools.ORM.OIALModel" IsEmbedding="true" Id="6BCA5356-4932-4490-BC12-ED881BE0C079">
			<Source>
				<DomainRole Name="Model" PropertyName="InformationTypeFormatCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="1D539D42-E9BB-41ED-A388-63DFCD3A3527">
					<RolePlayer>
						<DomainClassMoniker Name="OIALModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="InformationTypeFormat" PropertyName="Model" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="InformationTypeFormat" Id="3735FEB2-CD83-4A71-B4F1-871266976BF2">
					<RolePlayer>
						<DomainClassMoniker Name="InformationTypeFormat"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConceptTypeRef" Namespace="Neumont.Tools.ORM.OIALModel" Id="8EE4CA1C-F47E-49E8-B732-C33DA9E56FC7" AllowsDuplicates="true">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ConceptTypeHasChild" />
			</BaseRelationship>
			<Properties>
				<DomainProperty Name="OppositeName" DefaultValue="" DisplayName="OppositeName" Id="ED2A412D-8D2B-4819-94EB-8F8AA6745674">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
			<Source>
				<DomainRole Name="ReferencingConceptType" PropertyName="ReferencedConceptTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ReferencingConceptType" Id="6A7BF5FE-205A-4114-A69B-F66764BEF6E9">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ReferencedConceptType" PropertyName="ReferencingConceptType" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ReferencedConceptType" Id="FE87C3DB-A84D-414F-BD9F-E2E45A25AABA">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="InformationTypeHasInformationTypeFormat" Namespace="Neumont.Tools.ORM.OIALModel" Id="D70B7396-F7CC-40A2-A062-DE8A6C864B83">
			<Source>
				<DomainRole Name="InformationType" PropertyName="InformationTypeFormat" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="InformationType" Id="07FBDC19-A33C-4377-BD14-7105A671FD7E">
					<RolePlayer>
						<DomainClassMoniker Name="InformationType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="InformationTypeFormat" PropertyName="InformationType" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="InformationTypeFormat" Id="EA0795A4-6811-421D-959F-D09FF082B01A">
					<RolePlayer>
						<DomainClassMoniker Name="InformationTypeFormat"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="InformationTypeFormatHasObjectType" Namespace="Neumont.Tools.ORM.OIALModel" Id="837FB0B5-12B6-4432-9785-DDA720D469D6">
			<Source>
				<DomainRole Name="InformationTypeFormat" PropertyName="ValueType" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="InformationTypeFormat" Id="5ED10E60-BBD1-407A-B845-CC25217C8DC4">
					<RolePlayer>
						<DomainClassMoniker Name="InformationTypeFormat"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ValueType" PropertyName="InformationTypeFormat" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ValueType" Id="4C90D645-49CA-40B7-971E-BB17E2CF79A1">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConceptTypeHasObjectType" Namespace="Neumont.Tools.ORM.OIALModel" Id="473534D7-3779-49E7-AB65-4BCEF9932B06">
			<Source>
				<DomainRole Name="ConceptType" PropertyName="ObjectType" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ConceptType" Id="03B7885F-475C-499A-AFC1-195D9CA5FA7F">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ObjectType" PropertyName="ConceptType" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ObjectType" Id="A2EE78AA-C18A-417F-BB1D-335C9DF8DD0C">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ChildSequenceConstraintHasChildSequence" Namespace="Neumont.Tools.ORM.OIALModel" IsEmbedding="true" InheritanceModifier="Abstract" Id="30E43083-7C59-40F9-8834-7F318DF81802">
			<Source>
				<DomainRole Name="ChildSequenceConstraint" PropertyName="ChildSequence" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="SingleChildSequenceConstraint" Id="84F895EF-5CDA-4716-A738-A1F50A40098F">
					<RolePlayer>
						<DomainClassMoniker Name="ChildSequenceConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ChildSequence" PropertyName="ChildSequenceConstraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="false" DisplayName="ChildSequence" Id="7BCC2A75-DB60-4399-9467-FAED6AEDED82">
					<RolePlayer>
						<DomainClassMoniker Name="ChildSequence"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SingleChildSequenceConstraintHasMinTwoChildrenChildSequence" Namespace="Neumont.Tools.ORM.OIALModel" IsEmbedding="true" Id="10E35E6D-E220-4003-A44E-8011676ED75C">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ChildSequenceConstraintHasChildSequence"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="SingleChildSequenceConstraint" PropertyName="ChildSequence" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SingleChildSequenceConstraint" Id="B057D4E8-4342-43C5-AC8D-87043DB5B0F4">
					<RolePlayer>
						<DomainClassMoniker Name="SingleChildSequenceConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ChildSequence" PropertyName="SingleChildSequenceConstraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="false" DisplayName="ChildSequence" Id="381D7C86-173E-4DB6-919C-AC2A03D28F7B">
					<RolePlayer>
						<DomainClassMoniker Name="MinTwoChildrenChildSequence"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SubsetConstraintHasSubChildSequence" Namespace="Neumont.Tools.ORM.OIALModel" IsEmbedding="true" Id="C18AA773-3480-49EA-A15A-74BF18486518">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ChildSequenceConstraintHasChildSequence"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="SubsetConstraint" PropertyName="SubChildSequence" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SubsetConstraint" Id="6F57A29D-B3D9-4AFD-96F3-CFF4DA8330D4">
					<RolePlayer>
						<DomainClassMoniker Name="SubsetConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="SubChildSequence" PropertyName="SubsetConstraint" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="false" DisplayName="SubChildSequence" Id="9A40CFF0-272E-4441-B2FD-4C2CB2C8EEF4">
					<RolePlayer>
						<DomainClassMoniker Name="ChildSequence"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SubsetConstraintHasSuperChildSequence" Namespace="Neumont.Tools.ORM.OIALModel" IsEmbedding="true" Id="D5E1F509-4802-4FEB-96F8-652DA6FFC9C5">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ChildSequenceConstraintHasChildSequence"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="SubsetConstraint" PropertyName="SuperChildSequence" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SubsetConstraint" Id="DB67F0ED-AAB6-401F-A05F-1F6743CF1373">
					<RolePlayer>
						<DomainClassMoniker Name="SubsetConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="SuperChildSequence" PropertyName="SubsetConstraint" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="false" DisplayName="SuperChildSequence" Id="96A9C720-C183-4073-8759-4041D8524DE7">
					<RolePlayer>
						<DomainClassMoniker Name="ChildSequence"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="TwoOrMoreChildSequenceConstraintHasChildSequence" Namespace="Neumont.Tools.ORM.OIALModel" IsEmbedding="true" Id="34A603F3-4002-410C-8806-D11769CA536B">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ChildSequenceConstraintHasChildSequence"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="TwoOrMoreChildSequenceConstraint" PropertyName="ChildSequence" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="TwoOrMoreChildSequenceConstraint" Id="CD1D2BFD-3ECB-45CC-9539-4C85BE09AD63">
					<RolePlayer>
						<DomainClassMoniker Name="TwoOrMoreChildSequenceConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ChildSequence" PropertyName="TwoOrMoreChildSequenceConstraint" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="false" DisplayName="ChildSequence" Id="2340754C-5B5B-4948-BB37-7DDD4844A0AF">
					<RolePlayer>
						<DomainClassMoniker Name="ChildSequence"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ChildHasSingleChildConstraint" Namespace="Neumont.Tools.ORM.OIALModel" IsEmbedding="true" Id="BBEE96D9-9348-4C65-A668-939844A87E95">
			<Source>
				<DomainRole Name="ConceptTypeHasChild" PropertyName="SingleChildConstraintCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ConceptTypeHasChild" Id="7C128BE8-6C19-427E-95C6-0517E7A3F439">
					<RolePlayer>
						<DomainRelationshipMoniker Name="ConceptTypeHasChild"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="SingleChildConstraint" PropertyName="ConceptTypeHasChild" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="SingleChildConstraint" Id="708FDFD0-1921-464C-ABB0-D3A95F3D0BD7">
					<RolePlayer>
						<DomainClassMoniker Name="SingleChildConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="OIALModelHasChildSequenceConstraint" Namespace="Neumont.Tools.ORM.OIALModel" IsEmbedding="true" Id="C0386A3E-C683-4583-BE34-2FD7A6977CFD">
			<Source>
				<DomainRole Name="OIALModel" PropertyName="ChildSequenceConstraintCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="OIALModel" Id="50DBC977-4080-4472-9394-E1A0671B0DC8">
					<RolePlayer>
						<DomainClassMoniker Name="OIALModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ChildSequenceConstraint" PropertyName="OIALModel" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ChildSequenceConstraint" Id="978B29CC-3E94-464E-A41C-7BB3AA39F645">
					<RolePlayer>
						<DomainClassMoniker Name="ChildSequenceConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConceptTypeHasChildHasPathRole" Namespace="Neumont.Tools.ORM.OIALModel" Id="A32D0A06-1A64-4D53-8DA7-B42BC9CF1E11">
			<Source>
				<DomainRole Name="ConceptTypeHasChild" PropertyName="PathRoleCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ConceptTypeHasChild" Id="888D5C3C-4A27-4FD9-87D4-85EB33CCE90C">
					<RolePlayer>
						<DomainRelationshipMoniker Name="ConceptTypeHasChild"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="PathRole" PropertyName="ConceptTypeHasChild" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="PathRole" Id="8E2CFDB8-F6D4-44E2-AE9C-BB72E5052809">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/RoleBase"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ChildSequenceHasConceptTypeHasChild" Namespace="Neumont.Tools.ORM.OIALModel" Id="37B530D4-8951-4DC7-B77A-CB6045EEF9D3">
			<Source>
				<DomainRole Name="ChildSequence" PropertyName="ConceptTypeHasChildCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ChildSequence" Id="5B867D80-06C5-4B15-8107-0AA297254A1B">
					<RolePlayer>
						<DomainClassMoniker Name="ChildSequence"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ConceptTypeHasChild" PropertyName="ChildSequenceCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ConceptTypeHasChild" Id="8B792457-1A77-4965-A7B7-496007C1C343">
					<RolePlayer>
						<DomainRelationshipMoniker Name="ConceptTypeHasChild"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

	</Relationships>

	<Types>
		<DomainEnumeration Namespace="Neumont.Tools.ORM.OIALModel" Name="MandatoryConstraintModality" Description="A list of constraint modalities for simple mandatory role constraints used in &lt;see cref=&quot;ConceptTypeHasChild&quot;/&gt; relationships.">
			<Literals>
				<EnumerationLiteral Name ="NotMandatory" Value="0" Description="See &lt;see langword=&quot;null&quot;/&gt;."/>
				<EnumerationLiteral Name ="Alethic" Value="1" Description="See &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.ConstraintModality.Alethic&quot;/&gt;."/>
				<EnumerationLiteral Name ="Deontic" Value="2" Description="See &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.ConstraintModality.Deontic&quot;/&gt;."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;MandatoryConstraintModality, global::Neumont.Tools.ORM.OIALModel.OIALDomainModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
	</Types>

	<XmlSerializationBehavior Name="OIALMetaModelSerializationBehavior" Namespace="Neumont.Tools.ORM.OIALModel"/>
	
</Dsl>