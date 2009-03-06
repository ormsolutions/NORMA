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
	PackageGuid="EFDDC549-1646-4451-8A51-E5A5E94D647C"
	Id="77C1024F-D688-4AEE-AF16-29C2E791A9E7"
	Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToBarkerERBridge"
	Name="ORMAbstractionToBarkerERBridge"
	DisplayName="Map to Barker ER Model"
	Description="(Preliminary) Bridge ORM Abstraction Model and Barker ER Model"
	CompanyName="ORM Solutions, LLC"
	ProductName="Natural Object-Role Modeling Architect for Visual Studio"
	MajorVersion="1" MinorVersion="0" Build="0" Revision="0">

	<Attributes>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;F60BC3F1-C38E-4C7D-9EE5-9211DB26CB45&quot;/*ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;3EAE649F-E654-4D04-8289-C25D2C0322D8&quot;/*ORMSolutions.ORMArchitect.Core.ObjectModel.ORMCoreDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;F7BC82F4-83D1-408C-BA42-607E90B23BEA&quot;/*ORMSolutions.ORMArchitect.ORMAbstraction.AbstractionDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;B2CAED8E-4155-4317-9405-55006FDE280E&quot;/*ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker.BarkerDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;1F394F03-8A41-48BC-BDED-2268E131B4A3&quot;/*ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.ORMToORMAbstractionBridgeDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
	</Attributes>

	<Classes>
		<DomainClass Name="MappingBarkerModel" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToBarkerERBridge" Id="1EAA1877-F749-465C-AB97-9C64F9582BF8" DisplayName="MappingBarkerModel" Description=""/>

		<DomainClass Name="BarkerERModelGenerationSetting" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToBarkerERBridge" Id="77B26A43-49CF-47CE-BDCC-44FD78F9096F" DisplayName="BarkerERModelGenerationSetting" Description="">
			<BaseClass>
				<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.Core.ObjectModel/GenerationSetting"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="CoreAlgorithmVersion" DisplayName="Depth" Id="DE3D9138-4E38-427B-BA66-DF7B3CCDC4FA">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="NameAlgorithmVersion" DisplayName="Depth" Id="803570CD-A276-4F30-9BC6-70AA923B7F97">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
	</Classes>
	
	<Relationships>
		<DomainRelationship Id="42C5224A-EA1F-4C06-9516-D44B4FDBC30D" Name="BarkerErModelIsForAbstractionModel" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToBarkerERBridge">
			<Source>
				<DomainRole Id="D2D2C257-DC11-4EB2-86A9-A1CC31CD551D" Description="" Name="BarkerErModel" PropertyName="AbstractionModel" Multiplicity="One">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker/BarkerErModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="F861386B-1881-4AC7-9988-F5F5EFC27721" Description="" Name="AbstractionModel" PropertyName="BarkerErModel" Multiplicity="One">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.ORMAbstraction/AbstractionModel"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="281345A7-81EA-4BF8-A3C6-77607C83EA9B" Name="EntityTypeIsPrimarilyForConceptType" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToBarkerERBridge">
			<Source>
				<DomainRole Id="A848A9DB-04DE-4607-9205-35C2B8A4AC4E" Description="" Name="EntityType" PropertyName="ConceptType" Multiplicity="One">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker/EntityType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="79FE9518-573B-4D6E-B87D-E829076A0D96" Description="" Name="ConceptType" PropertyName="EntityType" Multiplicity="ZeroOne">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.ORMAbstraction/ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="GenerationSettingTargetsBarkerERModel" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToBarkerERBridge" Id="E44E5810-1166-4BB9-A6AB-4D8176DD3D46">
			<Source>
				<DomainRole Name="GenerationSetting" PropertyName="GeneratedBarkerERModel" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="GenerationSetting" Id="0A1DEAE3-DE66-4D36-9597-C9B095E8A5C4">
					<RolePlayer>
						<DomainClassMoniker Name="BarkerERModelGenerationSetting"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="GeneratedBarkerERModel" PropertyName="GenerationSetting" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="GeneratedBarkerERModel" Id="B993B0D6-D3AE-4A31-9094-A8696A2297EC">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker/BarkerErModel"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Id="E068B80F-D598-4C4C-A6E9-729602A6C564" Name="AttributeHasConceptTypeChild" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToBarkerERBridge" AllowsDuplicates="true">
			<Source>
				<DomainRole Id="C18D335D-5B83-4B06-BD07-FD04F941FA57" Description="" Name="Attribute" PropertyName="ConceptTypeChildPath" Multiplicity="OneMany">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker/Attribute"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="31A83BC9-8635-49CF-945F-21E01E54B228" Description="" Name="ConceptTypeChild" PropertyName="Attribute" Multiplicity="OneMany">
					<RolePlayer>
						<DomainRelationshipMoniker Name="/ORMSolutions.ORMArchitect.ORMAbstraction/ConceptTypeChild"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="011E06F6-6B28-4212-B57D-9C97403BCE0B" Name="BinaryAssociationHasConceptTypeChild" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToBarkerERBridge" AllowsDuplicates="true">
			<Source>
				<DomainRole Id="EC36D9A6-531C-4F56-8679-97BD23077F84" Description="" Name="BinaryAssociation" PropertyName="ConceptTypeChildPath" Multiplicity="OneMany">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker/BinaryAssociation"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="3BFD33CE-BD21-4773-863E-4C3BD82279C4" Description="" Name="ConceptTypeChild" PropertyName="BinaryAssociation" Multiplicity="OneMany">
					<RolePlayer>
						<DomainRelationshipMoniker Name="/ORMSolutions.ORMArchitect.ORMAbstraction/ConceptTypeChild"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="E9923CB9-22EF-4866-9E54-3799C8311DAE" Name="BinaryAssociationHasConceptType" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToBarkerERBridge" AllowsDuplicates="true">
			<Source>
				<DomainRole Id="B56F9740-E940-4021-81D6-3D49F6EA911B" Description="" Name="BinaryAssociation" PropertyName="ConceptType" Multiplicity="OneMany">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker/BinaryAssociation"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="453529F7-8950-48FD-BC2A-DC56D8A32672" Description="" Name="ConceptType" PropertyName="BinaryAssociation" Multiplicity="OneMany">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.ORMAbstraction/ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
	</Relationships>

	<Types>
	</Types>

	<XmlSerializationBehavior Name="ORMAbstractionToBarkerERBridgeSerializationBehavior" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToBarkerERBridge"/>	
</Dsl>