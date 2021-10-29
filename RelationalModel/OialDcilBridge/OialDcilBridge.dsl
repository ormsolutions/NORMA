﻿<?xml version="1.0" encoding="utf-8"?>
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
	Id="005CBD56-3BA5-4947-9F46-5608BD563CED"
	Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge"
	Name="ORMAbstractionToConceptualDatabaseBridge"
	DisplayName="Map to Relational Model"
	Description="Bridge ORM Abstraction Model and Relational Database Model"
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
				<AttributeParameter Value="&quot;CEDE46B1-9CA1-4C55-BC88-3DACFADD70EA&quot;/*ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.ConceptualDatabaseDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;1F394F03-8A41-48BC-BDED-2268E131B4A3&quot;/*ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.ORMToORMAbstractionBridgeDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
	</Attributes>

	<Classes>
		<DomainClass Name="MappingCustomizationModel" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge" Id="D027CCE5-B22A-44C0-A580-630658605DA5" DisplayName="MappingCustomizationModel" Description=""/>
		<DomainClass Name="AssimilationMapping" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge" Id="2F631FD8-6B87-42E6-961F-750A566FB7C1" DisplayName="AssimilationMapping" Description="">
			<Properties>
				<DomainProperty Name="AbsorptionChoice" DefaultValue="Absorb" DisplayName="AbsorptionChoice" Id="62B69840-B95F-467B-8FFF-670552139E1D">
					<Type>
						<DomainEnumerationMoniker Name="AssimilationAbsorptionChoice"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="RelationalReferenceModeNaming" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge" Id="8153232D-54BD-4CFE-9B2F-9E20FACEB688" DisplayName="ReferenceModeNaming" InheritanceModifier="Sealed" Description="">
			<BaseClass>
				<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.Core.ObjectModel/ReferenceModeNaming"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="RelationalDefaultReferenceModeNaming" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge" Id="953A051C-B190-4BCA-9EB9-6BDCE8EC5D7C" DisplayName="DefaultReferenceModeNaming" InheritanceModifier="Sealed" Description="">
			<BaseClass>
				<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.Core.ObjectModel/DefaultReferenceModeNaming"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="RelationalNameGenerator" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge" Id="A2D24E49-1B2F-42C0-B1CE-1F7F3B193E26" DisplayName="Relational Names" Description="">
			<Attributes>
				<ClrAttribute Name="ORMSolutions.ORMArchitect.Core.ObjectModel.NameConsumerIdentifier">
					<Parameters>
						<AttributeParameter Value="&quot;Relational&quot;"/>
					</Parameters>
				</ClrAttribute>
				<ClrAttribute Name="ORMSolutions.ORMArchitect.Core.ObjectModel.NameUsage">
					<Parameters>
						<AttributeParameter Value="typeof(ColumnNameUsage)"/>
					</Parameters>
				</ClrAttribute>
				<ClrAttribute Name="ORMSolutions.ORMArchitect.Core.ObjectModel.NameUsage">
					<Parameters>
						<AttributeParameter Value="typeof(TableNameUsage)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.Core.ObjectModel/NameGenerator"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ColumnNameUsage" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge" InheritanceModifier="Abstract" Id="61362E35-E677-4A3E-B5CC-A05B7C6EA6E9" DisplayName="Column Specific" Description="">
			<Attributes>
				<ClrAttribute Name="ORMSolutions.ORMArchitect.Core.ObjectModel.NameUsageIdentifier">
					<Parameters>
						<AttributeParameter Value="&quot;RelationalColumn&quot;"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.Core.ObjectModel/NameUsage"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TableNameUsage" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge" InheritanceModifier="Abstract" Id="E8CE0734-1FB3-4D0B-9DA0-56FDDC502AC0" DisplayName="Table Specific" Description="">
			<Attributes>
				<ClrAttribute Name="ORMSolutions.ORMArchitect.Core.ObjectModel.NameUsageIdentifier">
					<Parameters>
						<AttributeParameter Value="&quot;RelationalTable&quot;"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.Core.ObjectModel/NameUsage"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="SchemaGenerationSetting" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge" Id="94918D9C-0935-4B98-86F2-FF8C98861E0D" DisplayName="SchemaGenerationSetting" Description="">
			<BaseClass>
				<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.Core.ObjectModel/GenerationSetting"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="CoreAlgorithmVersion" DisplayName="Depth" Id="5570F2C2-0F9E-43A7-8A2E-AFC3DFB3F7A3">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="NameAlgorithmVersion" DisplayName="Depth" Id="01203AF6-93D5-4555-9BD1-545CF5907022">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
	</Classes>
	
	<Relationships>
		<DomainRelationship Id="C997059D-5F08-43DB-A225-B698EA7BADFB" Name="AssimilationMappingCustomizesFactType" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge">
			<Source>
				<DomainRole Id="CDF964C3-4A74-479A-86DB-5D5ABB23DCEA" Description="" Name="AssimilationMapping" PropertyName="FactType" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true">
					<RolePlayer>
						<DomainClassMoniker Name="AssimilationMapping"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="41BD276D-D55E-4972-AFD9-757EBE824F39" Description="" Name="FactType" PropertyName="AssimilationMapping" Multiplicity="One" IsPropertyGenerator="false">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.Core.ObjectModel/FactType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="61DEB15B-308B-4266-8766-C1E4348250D8" Name="MappingCustomizationModelHasAssimilationMapping" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge" IsEmbedding="true">
			<Source>
				<DomainRole Id="2122D3FD-134D-41DB-8B12-8FD2FD60727E" Description="" Name="Model" PropertyName="AssimilationMappingCollection" Multiplicity="ZeroMany" IsPropertyGenerator="true">
					<RolePlayer>
						<DomainClassMoniker Name="MappingCustomizationModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="DA82C400-912E-45B2-87C4-56AF21A7D481" Description="" Name="AssimilationMapping" PropertyName="Model" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true">
					<RolePlayer>
						<DomainClassMoniker Name="AssimilationMapping"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="F96C317D-4820-4604-AA3E-7F8A97541B7E" Name="ReferenceModeNamingCustomizesObjectType" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge">
			<Source>
				<DomainRole Id="D83F7C4D-F955-4EC8-BDBC-0E7CDC480A79" Description="" Name="ReferenceModeNaming" PropertyName="ObjectType" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true">
					<RolePlayer>
						<DomainClassMoniker Name="RelationalReferenceModeNaming"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="67C2B3CD-F276-411B-980D-13D94970D604" Description="" Name="ObjectType" PropertyName="ReferenceModeNaming" Multiplicity="ZeroOne" IsPropertyGenerator="false">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.Core.ObjectModel/ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="D412080D-5555-4134-8106-9F9452A7D452" Name="MappingCustomizationModelHasReferenceModeNaming" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge" IsEmbedding="true">
			<Source>
				<DomainRole Id="F10B9590-AF11-4FD9-BBA9-D277A7A50FA0" Description="" Name="Model" PropertyName="ReferenceModeNamingCollection" Multiplicity="ZeroMany" IsPropertyGenerator="true">
					<RolePlayer>
						<DomainClassMoniker Name="MappingCustomizationModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="219A1852-9896-4303-BF8F-2696BAA25962" Description="" Name="ReferenceModeNaming" PropertyName="Model" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true">
					<RolePlayer>
						<DomainClassMoniker Name="RelationalReferenceModeNaming"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="0B75DB0C-3196-4C80-884B-2ADDA04DE8B0" Name="DefaultReferenceModeNamingCustomizesORMModel" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge">
			<Source>
				<DomainRole Id="341C7C8A-D168-4D09-B02D-6A79BC3F34C8" Description="" Name="ORMModel" PropertyName="DefaultReferenceModeNamingCollection" Multiplicity="ZeroMany" IsPropertyGenerator="false">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.Core.ObjectModel/ORMModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="5C4580AD-BC05-4691-A9CE-E54952DB1EF9" Description="" Name="DefaultReferenceModeNaming" PropertyName="ORMModel" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true">
					<RolePlayer>
						<DomainClassMoniker Name="RelationalDefaultReferenceModeNaming"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="93737680-42D1-4453-BD79-D143406648CE" Name="MappingCustomizationModelHasDefaultReferenceModeNaming" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge" IsEmbedding="true">
			<Source>
				<DomainRole Id="652081EB-9586-407C-8D3F-A6491B975C27" Description="" Name="Model" PropertyName="DefaultReferenceModeNamingCollection" Multiplicity="ZeroMany" IsPropertyGenerator="true">
					<RolePlayer>
						<DomainClassMoniker Name="MappingCustomizationModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="32C09A1C-8139-489A-8A4E-2E35A5C31D7C" Description="" Name="DefaultReferenceModeNaming" PropertyName="Model" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true">
					<RolePlayer>
						<DomainClassMoniker Name="RelationalDefaultReferenceModeNaming"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="9CA44CEF-1787-41BC-A0AC-5AC79753DABB" Name="SchemaIsForAbstractionModel" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge">
			<Source>
				<DomainRole Id="6ED59846-E0D8-4980-81AC-B3541AD9D7DD" Description="" Name="Schema" PropertyName="AbstractionModel" Multiplicity="One">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase/Schema"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="3C18D01E-2687-42A1-9536-B23F91A247D0" Description="" Name="AbstractionModel" PropertyName="Schema" Multiplicity="One">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.ORMAbstraction/AbstractionModel"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="3598C529-7D90-47BF-92AE-F77575B8BFD9" Name="TableIsPrimarilyForConceptType" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge">
			<Source>
				<DomainRole Id="DAEB8DB4-C3A6-497D-BA4C-74D1AF6CCDC0" Description="" Name="Table" PropertyName="ConceptType" Multiplicity="One">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase/Table"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="64F5EC66-1AF4-4368-A1A2-5913681DE491" Description="" Name="ConceptType" PropertyName="Table" Multiplicity="ZeroOne">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.ORMAbstraction/ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="058E26E9-85E8-4DA4-8979-BFA0455D41CC" Name="TableIsAlsoForConceptType" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge">
			<Source>
				<DomainRole Id="E2F7DD3B-8E40-45A5-AC39-863ABFA35020" Description="" Name="Table" PropertyName="ConceptType" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase/Table"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="51D93721-0758-4405-B36E-E807309F8CA8" Description="" Name="ConceptType" PropertyName="Table" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.ORMAbstraction/ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="1772C296-EC3E-4FE3-88D5-4ABC85E74849" Name="TableIsAlsoForConceptTypeHasAssimilationPath" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge">
			<Source>
				<DomainRole Id="BDA4BD0B-F86E-481A-921F-7F76EDBE81A7" Name="TableIsAlsoForConceptType" PropertyName="AssimilationPath" Multiplicity="OneMany">
					<RolePlayer>
						<DomainRelationshipMoniker Name="TableIsAlsoForConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="C14AED06-E158-4082-96E3-62C2A7E302F1" Name="Assimilation" PropertyName="TableIsAlsoForConceptType" IsPropertyGenerator="false" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainRelationshipMoniker Name="/ORMSolutions.ORMArchitect.ORMAbstraction/ConceptTypeAssimilatesConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="CCBCAB63-ADE4-43FA-8E29-8A598B0969F5" Name="ColumnHasConceptTypeChild" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge" AllowsDuplicates="true">
			<Source>
				<DomainRole Id="BC7EA8A8-8772-4CA4-B914-B78B4B583338" Description="" Name="Column" PropertyName="ConceptTypeChildPath" Multiplicity="OneMany">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase/Column"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="B162A279-A4C1-4271-AD37-9CDDFC421722" Description="" Name="ConceptTypeChild" PropertyName="Column" Multiplicity="OneMany">
					<RolePlayer>
						<DomainRelationshipMoniker Name="/ORMSolutions.ORMArchitect.ORMAbstraction/ConceptTypeChild"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="5DA169BB-5439-4F61-926F-6B789503511E" Name="UniquenessConstraintIsForUniqueness" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge">
			<Source>
				<DomainRole Id="2C58F935-E736-401F-B22F-E38F71AB33E5" Description="" Name="UniquenessConstraint" PropertyName="Uniqueness" Multiplicity="ZeroOne">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase/UniquenessConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="78AB7C46-D141-4136-B1DA-2F5A64E820DC" Description="" Name="Uniqueness" PropertyName="UniquenessConstraint" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.ORMAbstraction/Uniqueness"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="A9F82D26-1D87-4808-B618-37FF179466FC" Name="DomainIsForInformationTypeFormat" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge">
			<Source>
				<DomainRole Id="EEC22052-39CD-4F93-AC66-6634DD6423B4" Description="" Name="Domain" PropertyName="InformationTypeFormat" Multiplicity="One">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase/Domain"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="64E89755-37F7-43D6-A6C5-654B1C315D1B" Description="" Name="InformationTypeFormat" PropertyName="Domain" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.ORMAbstraction/InformationTypeFormat"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="GenerationSettingTargetsSchema" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge" Id="D7C8E3C8-7A84-4C28-8362-8C8D38C35A07">
			<Source>
				<DomainRole Name="GenerationSetting" PropertyName="GeneratedSchema" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="GenerationSetting" Id="654A46A6-2F7A-4C60-AEC9-AD4B71F58082">
					<RolePlayer>
						<DomainClassMoniker Name="SchemaGenerationSetting"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="GeneratedSchema" PropertyName="GenerationSetting" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="GeneratedSchema" Id="4CD40629-C999-4E6B-A8B6-429046D3F554">
					<RolePlayer>
						<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase/Schema"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
	</Relationships>

	<Types>
		<DomainEnumeration Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge" Name="AssimilationAbsorptionChoice" Description="Determines the mechanism used to map assimilation relationships as table or column elements.">
			<Literals>
				<EnumerationLiteral Name="Absorb" Value="0" Description="All assimilations are pulled into the same table as the supertype."/>
				<EnumerationLiteral Name="Partition" Value="1" Description="Each subtype is given its own table. Data from the supertype is duplicated."/>
				<EnumerationLiteral Name="Separate" Value="2" Description="Each subtype is given its own table, data from the supertype is stored in a separate referenced table."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.EnumConverter&lt;AssimilationAbsorptionChoice, global::ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.MappingCustomizationModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
	</Types>

	<XmlSerializationBehavior Name="ORMAbstractionToConceptualDatabaseBridgeSerializationBehavior" Namespace="ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge"/>

	<DslLibraryImports>
		<DslLibraryImport FilePath="..\DcilModel\DcilModel.dsl"/>
		<DslLibraryImport FilePath="..\..\Oial\OialModel\OialModel.dsl"/>
		<DslLibraryImport FilePath="..\..\Oial\ORMOialBridge\ORMOialBridge.dsl"/>
		<DslLibraryImport FilePath="..\..\ORMModel\Framework\Framework.dsl"/>
		<DslLibraryImport FilePath="..\..\ORMModel\Framework\SystemCore.dsl"/>
		<DslLibraryImport FilePath="..\..\ORMModel\ObjectModel\ORMCore.dsl"/>
	</DslLibraryImports>

</Dsl>