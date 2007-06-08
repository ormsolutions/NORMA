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
	Id="C52FB9A5-6BF4-4267-8716-71D74C7AA89C"
	Namespace="Neumont.Tools.OialDcilBridge"
	PackageNamespace="Neumont.Tools.ORM.Shell"
	Name="OialDcilBridge"
	DisplayName="OIAL/DCIL Bridge"
	CompanyName="Neumont University"
	ProductName="Neumont ORM Architect for Visual Studio"
	MajorVersion="1" MinorVersion="0" Build="0" Revision="0">

	<Attributes>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;F7BC82F4-83D1-408C-BA42-607E90B23BEA&quot;/*Neumont.Tools.Oial.OialDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;CEDE46B1-9CA1-4C55-BC88-3DACFADD70EA&quot;/*Neumont.Tools.Dil.Dcil.DcilDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
	</Attributes>
	
	<Relationships>
		<DomainRelationship Id="9CA44CEF-1787-41BC-A0AC-5AC79753DABB" Name="SchemaIsForOialModel" Namespace="Neumont.Tools.OialDcilBridge">
			<Source>
				<DomainRole Id="6ED59846-E0D8-4980-81AC-B3541AD9D7DD" Description="" Name="Schema" PropertyName="OialModel" Multiplicity="One">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.Dil.Dcil/Schema"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="3C18D01E-2687-42A1-9536-B23F91A247D0" Description="" Name="OialModel" PropertyName="Schema" Multiplicity="One">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.Oial/OialModel"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="3598C529-7D90-47BF-92AE-F77575B8BFD9" Name="TableIsPrimarilyForConceptType" Namespace="Neumont.Tools.OialDcilBridge">
			<Source>
				<DomainRole Id="DAEB8DB4-C3A6-497D-BA4C-74D1AF6CCDC0" Description="" Name="Table" PropertyName="ConceptType" Multiplicity="One">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.Dil.Dcil/Table"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="64F5EC66-1AF4-4368-A1A2-5913681DE491" Description="" Name="ConceptType" PropertyName="Table" Multiplicity="ZeroOne">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.Oial/ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="058E26E9-85E8-4DA4-8979-BFA0455D41CC" Name="TableIsAlsoForConceptType" Namespace="Neumont.Tools.OialDcilBridge">
			<Source>
				<DomainRole Id="E2F7DD3B-8E40-45A5-AC39-863ABFA35020" Description="" Name="Table" PropertyName="ConceptType" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.Dil.Dcil/Table"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="51D93721-0758-4405-B36E-E807309F8CA8" Description="" Name="ConceptType" PropertyName="Table" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.Oial/ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="CCBCAB63-ADE4-43FA-8E29-8A598B0969F5" Name="ColumnHasConceptTypeChild" Namespace="Neumont.Tools.OialDcilBridge">
			<Source>
				<DomainRole Id="BC7EA8A8-8772-4CA4-B914-B78B4B583338" Description="" Name="Column" PropertyName="ConceptTypeChildPath" Multiplicity="OneMany">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.Dil.Dcil/Column"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="B162A279-A4C1-4271-AD37-9CDDFC421722" Description="" Name="ConceptTypeChild" PropertyName="Column" Multiplicity="OneMany">
					<RolePlayer>
						<DomainRelationshipMoniker Name="/Neumont.Tools.Oial/ConceptTypeChild"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="5DA169BB-5439-4F61-926F-6B789503511E" Name="UniquenessConstraintIsForUniqueness" Namespace="Neumont.Tools.OialDcilBridge">
			<Source>
				<DomainRole Id="2C58F935-E736-401F-B22F-E38F71AB33E5" Description="" Name="UniquenessConstraint" PropertyName="Uniqueness" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.Dil.Dcil/UniquenessConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="78AB7C46-D141-4136-B1DA-2F5A64E820DC" Description="" Name="ConceptTypeChild" PropertyName="Column" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainRelationshipMoniker Name="/Neumont.Tools.Oial/ConceptTypeChild"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="A9F82D26-1D87-4808-B618-37FF179466FC" Name="DomainIsForInformationTypeFormat" Namespace="Neumont.Tools.OialDcilBridge">
			<Source>
				<DomainRole Id="EEC22052-39CD-4F93-AC66-6634DD6423B4" Description="" Name="Domain" PropertyName="InformationTypeFormat" Multiplicity="One">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.Dil.Dcil/Domain"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="64E89755-37F7-43D6-A6C5-654B1C315D1B" Description="" Name="InformationTypeFormat" PropertyName="Domain" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.Oial/InformationTypeFormat"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
	</Relationships>

	<XmlSerializationBehavior Name="OialDcilBridgeSerializationBehavior" Namespace="Neumont.Tools.OialDcilBridge"/>	
</Dsl>