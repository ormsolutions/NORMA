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
	PackageGuid="EFDDC549-1646-4451-8A51-E5A5E94D647C"
	Id="005CBD56-3BA5-4947-9F46-5608BD563CED"
	Namespace="Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge"
	Name="ORMAbstractionToConceptualDatabaseBridge"
	DisplayName="(Preliminary) ORMAbstraction/ConceptualDatabase Bridge"
	CompanyName="Neumont University"
	ProductName="Neumont ORM Architect for Visual Studio"
	MajorVersion="1" MinorVersion="0" Build="0" Revision="0">

	<Attributes>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;F7BC82F4-83D1-408C-BA42-607E90B23BEA&quot;/*Neumont.Tools.ORMAbstraction.AbstractionDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;CEDE46B1-9CA1-4C55-BC88-3DACFADD70EA&quot;/*Neumont.Tools.RelationalModels.ConceptualDatabase.ConceptualDatabaseDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
	</Attributes>

	<Classes>
		<DomainClass Name="MappingCustomizationModel" Namespace="Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge" Id="D027CCE5-B22A-44C0-A580-630658605DA5" DisplayName="MappingCustomizationModel" Description=""/>
		<DomainClass Name="AssimilationMapping" Namespace="Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge" Id="2F631FD8-6B87-42E6-961F-750A566FB7C1" DisplayName="AssimilationMapping" Description="">
			<Properties>
				<DomainProperty Name="AbsorptionChoice" DefaultValue="Absorb" DisplayName="AbsorptionChoice" Id="62B69840-B95F-467B-8FFF-670552139E1D">
					<Type>
						<DomainEnumerationMoniker Name="AssimilationAbsorptionChoice"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
	</Classes>
	
	<Relationships>
		<DomainRelationship Id="C997059D-5F08-43DB-A225-B698EA7BADFB" Name="AssimilationMappingCustomizesAssimilation" Namespace="Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge">
			<Source>
				<DomainRole Id="CDF964C3-4A74-479A-86DB-5D5ABB23DCEA" Description="" Name="AssimilationMapping" PropertyName="Assimilation" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true">
					<RolePlayer>
						<DomainClassMoniker Name="AssimilationMapping"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="41BD276D-D55E-4972-AFD9-757EBE824F39" Description="" Name="Assimilation" PropertyName="AssimilationMapping" Multiplicity="One" IsPropertyGenerator="false">
					<RolePlayer>
						<DomainRelationshipMoniker Name="/Neumont.Tools.ORMAbstraction/ConceptTypeAssimilatesConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="61DEB15B-308B-4266-8766-C1E4348250D8" Name="MappingCustomizationModelHasAssimilationMapping" Namespace="Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge" IsEmbedding="true">
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
		<DomainRelationship Id="9CA44CEF-1787-41BC-A0AC-5AC79753DABB" Name="SchemaIsForAbstractionModel" Namespace="Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge">
			<Source>
				<DomainRole Id="6ED59846-E0D8-4980-81AC-B3541AD9D7DD" Description="" Name="Schema" PropertyName="AbstractionModel" Multiplicity="One">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.RelationalModels.ConceptualDatabase/Schema"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="3C18D01E-2687-42A1-9536-B23F91A247D0" Description="" Name="AbstractionModel" PropertyName="Schema" Multiplicity="One">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORMAbstraction/AbstractionModel"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="3598C529-7D90-47BF-92AE-F77575B8BFD9" Name="TableIsPrimarilyForConceptType" Namespace="Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge">
			<Source>
				<DomainRole Id="DAEB8DB4-C3A6-497D-BA4C-74D1AF6CCDC0" Description="" Name="Table" PropertyName="ConceptType" Multiplicity="One">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.RelationalModels.ConceptualDatabase/Table"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="64F5EC66-1AF4-4368-A1A2-5913681DE491" Description="" Name="ConceptType" PropertyName="Table" Multiplicity="ZeroOne">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORMAbstraction/ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="058E26E9-85E8-4DA4-8979-BFA0455D41CC" Name="TableIsAlsoForConceptType" Namespace="Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge">
			<Source>
				<DomainRole Id="E2F7DD3B-8E40-45A5-AC39-863ABFA35020" Description="" Name="Table" PropertyName="ConceptType" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.RelationalModels.ConceptualDatabase/Table"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="51D93721-0758-4405-B36E-E807309F8CA8" Description="" Name="ConceptType" PropertyName="Table" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORMAbstraction/ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="CCBCAB63-ADE4-43FA-8E29-8A598B0969F5" Name="ColumnHasConceptTypeChild" Namespace="Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge">
			<Source>
				<DomainRole Id="BC7EA8A8-8772-4CA4-B914-B78B4B583338" Description="" Name="Column" PropertyName="ConceptTypeChildPath" Multiplicity="OneMany">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.RelationalModels.ConceptualDatabase/Column"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="B162A279-A4C1-4271-AD37-9CDDFC421722" Description="" Name="ConceptTypeChild" PropertyName="Column" Multiplicity="OneMany">
					<RolePlayer>
						<DomainRelationshipMoniker Name="/Neumont.Tools.ORMAbstraction/ConceptTypeChild"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="5DA169BB-5439-4F61-926F-6B789503511E" Name="UniquenessConstraintIsForUniqueness" Namespace="Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge">
			<Source>
				<DomainRole Id="2C58F935-E736-401F-B22F-E38F71AB33E5" Description="" Name="UniquenessConstraint" PropertyName="Uniqueness" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.RelationalModels.ConceptualDatabase/UniquenessConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="78AB7C46-D141-4136-B1DA-2F5A64E820DC" Description="" Name="ConceptTypeChild" PropertyName="Column" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainRelationshipMoniker Name="/Neumont.Tools.ORMAbstraction/ConceptTypeChild"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="A9F82D26-1D87-4808-B618-37FF179466FC" Name="DomainIsForInformationTypeFormat" Namespace="Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge">
			<Source>
				<DomainRole Id="EEC22052-39CD-4F93-AC66-6634DD6423B4" Description="" Name="Domain" PropertyName="InformationTypeFormat" Multiplicity="One">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.RelationalModels.ConceptualDatabase/Domain"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="64E89755-37F7-43D6-A6C5-654B1C315D1B" Description="" Name="InformationTypeFormat" PropertyName="Domain" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORMAbstraction/InformationTypeFormat"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
	</Relationships>

	<Types>
		<DomainEnumeration Namespace="Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge" Name="AssimilationAbsorptionChoice" Description="Determines the mechanism used to map assimilation relationships as table or column elements.">
			<Literals>
				<EnumerationLiteral Name="Absorb" Value="0" Description="All assimilations are pulled into the same table as the supertype."/>
				<EnumerationLiteral Name="Partition" Value="1" Description="Each subtype is given its own table. Data from the supertype is duplicated."/>
				<EnumerationLiteral Name="Separate" Value="2" Description="Each subtype is given its own table, data from the supertype is stored in a separate referenced table."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;AssimilationAbsorptionChoice, global::Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge.MappingCustomizationModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
	</Types>

	<XmlSerializationBehavior Name="ORMAbstractionToConceptualDatabaseBridgeSerializationBehavior" Namespace="Neumont.Tools.ORMAbstractionToConceptualDatabaseBridge"/>	
</Dsl>