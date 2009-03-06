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
	Id="B1430D25-AF34-47e3-BFFF-561DEEF0A2B1"
	Namespace="ORMSolutions.ORMArchitect.CustomProperties"
	Name="CustomProperties"
	AccessModifier="Assembly"
	DisplayName="Custom Properties"
	Description="Add custom properties to ORM model elements"
	CompanyName="ORM Solutions, LLC	"
	ProductName="Natural Object-Role Modeling Architect for Visual Studio"
	MajorVersion="1" MinorVersion="0" Build="0" Revision="0">

	<Attributes>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;3EAE649F-E654-4D04-8289-C25D2C0322D8&quot;/*ORMSolutions.ORMArchitect.Core.ObjectModel.ORMCoreDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
	</Attributes>
	
	<Classes>
		<DomainClass Name="CustomPropertyGroup" Namespace="ORMSolutions.ORMArchitect.CustomProperties" Id="D0938429-DBA7-42f1-829A-090FC2C75AD3" DisplayName="CustomPropertyGroup" AccessModifier="Assembly" InheritanceModifier="Sealed">
			<Properties>
				<DomainProperty Name="Name" DisplayName="Name" Id="35B17292-2DDC-4089-BD0F-F127A01D23C7">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<!--<DomainProperty Name="Namespace" DisplayName="Namespace" Id="9F1C96F8-8DA9-4FB9-8633-16DDA3795F11">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="Prefix" DisplayName="Prefix" Id="635EF40B-A5F0-4F7D-8F43-5C25FA4F90EC">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>-->
				<DomainProperty Name="IsDefault" DisplayName="IsDefault" Id="69F71D2C-1668-4818-89DB-01E616A215A8" DefaultValue="false">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="Description" DisplayName="Description" Id="7E619D46-D1D2-4B40-8A8D-7FA878CF4626">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
		<DomainClass Name="CustomPropertyDefinition" Namespace="ORMSolutions.ORMArchitect.CustomProperties" Id="169EA615-15F2-47FF-BAA4-6D8CD1D5DE4A" DisplayName="CustomPropertyDefinition" AccessModifier="Assembly" InheritanceModifier="Sealed">
			<Properties>
				<DomainProperty Name="Name" DisplayName="Name" Id="E5B9518B-B741-4002-A6A0-4E11D6D461DF">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="Description" DisplayName="Description" Id="090F63DA-24D4-4D28-B703-D868616134C4">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="Category" DisplayName="Category" Id="A600653C-13CF-4DD1-B849-EB61B44FB785" DefaultValue="Default">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="DataType" DisplayName="DataType" Id="B032D072-B353-4E14-8B47-DB41108F0DDA">
					<Type>
						<DomainEnumerationMoniker Name="CustomPropertyDataType"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="DefaultValue" DisplayName="Default" Id="A3023163-70D8-4E90-ACA9-315CFDDC7F44">
					<Type>
						<ExternalTypeMoniker Name="/System/Object"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="ORMTypes" DisplayName="ORMTypes" Id="C1D2D7A1-787C-4BF8-9EA3-3C0FFA6E39B0">
					<Type>
						<DomainEnumerationMoniker Name="ORMTypes"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="CustomEnumValue" DisplayName="CustomEnumValue" Id="86A3A085-CA1D-4757-9DB7-6A633771F79B">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
		<DomainClass Name="CustomProperty" Namespace="ORMSolutions.ORMArchitect.CustomProperties" Id="D9BD32B6-73CC-49f9-BD71-F39E738320BA" DisplayName="CustomProperty" AccessModifier="Assembly" InheritanceModifier="Sealed">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptionProvider&lt;CustomProperty, CustomPropertyTypeDescriptor&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<Properties>
				<DomainProperty Name="Value" DisplayName="Value" Id="5B638692-C232-44b4-A168-24EA2C899ED2">
					<Type>
						<ExternalTypeMoniker Name="/System/Object"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
	</Classes>

	<Relationships>
		<DomainRelationship Name="CustomPropertyHasCustomPropertyDefinition" Namespace="ORMSolutions.ORMArchitect.CustomProperties" Id="1A95E98A-4185-48F4-8FD0-E5AF676D26E7" AccessModifier="Assembly" InheritanceModifier="Sealed">
			<Source>
				<DomainRole Name="CustomProperty" Multiplicity="One" Id="B8BF7986-4312-42E2-B5C0-A460BB381666" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="CustomProperty" PropertyName="CustomPropertyDefinition">
					<RolePlayer>
						<DomainClassMoniker Name="CustomProperty"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="CustomPropertyDefinition" Multiplicity="ZeroMany" Id="AF59CD4C-9CB9-4FDA-A5F2-8362CEC9A94E" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="CustomPropertyDefinition" PropertyName="CustomPropertyCollection">
					<RolePlayer>
						<DomainClassMoniker Name="CustomPropertyDefinition"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<!--CustomPropertyGroupContainsCustomPropertyDefinition-->
		<!--<DomainRelationship Name="CustomProperties" Namespace="ORMSolutions.ORMArchitect.CustomProperties" Id="157C325F-561F-430D-95BB-1AE421FC3EDA" IsEmbedding="true" AccessModifier="Assembly">-->
		<DomainRelationship Name="CustomPropertyGroupContainsCustomPropertyDefinition" Namespace="ORMSolutions.ORMArchitect.CustomProperties" Id="157C325F-561F-430D-95BB-1AE421FC3EDA" IsEmbedding="true" AccessModifier="Assembly" InheritanceModifier="Sealed">
			<Source>
				<DomainRole Name="CustomPropertyGroup" Multiplicity="ZeroMany" Id="CC4A4E71-9945-4B47-A010-33E2F78646FA" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="CustomPropertyGroup" PropertyName="CustomPropertyDefinitionCollection">
					<RolePlayer>
						<DomainClassMoniker Name="CustomPropertyGroup"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="CustomPropertyDefinition" Multiplicity="One" Id="6B330D54-4307-4117-83E2-6C91242675F1" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="CustomPropertyDefinition" PropertyName="CustomPropertyGroup">
					<RolePlayer>
						<DomainClassMoniker Name="CustomPropertyDefinition"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
	</Relationships>

	<Types>
		<DomainEnumeration Name="ORMTypes" Namespace="ORMSolutions.ORMArchitect.CustomProperties" IsFlags="true" AccessModifier="Assembly">
			<Literals>
				<EnumerationLiteral Name="None" Value="0"/>
				
				<EnumerationLiteral Name="EntityType" Value="1"/>
				<EnumerationLiteral Name="ValueType" Value="2"/>
				<EnumerationLiteral Name="FactType" Value="4"/>
				<EnumerationLiteral Name="SubtypeFact" Value="8"/>
				<EnumerationLiteral Name="Role" Value="16"/>
				
				<EnumerationLiteral Name="FrequencyConstraint" Value="32"/>
				<EnumerationLiteral Name="MandatoryConstraint" Value="64"/>
				<EnumerationLiteral Name="RingConstraint" Value="128"/>
				<EnumerationLiteral Name="UniquenessConstraint" Value="256"/>
				<EnumerationLiteral Name="EqualityConstraint" Value="512"/>
				<EnumerationLiteral Name="ExclusionConstraint" Value="1024"/>
				<EnumerationLiteral Name="SubsetConstraint" Value="2048"/>
				<EnumerationLiteral Name="ValueConstraint" Value="4096"/>
				<EnumerationLiteral Name="AllConstraints" Value="7904"/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.EnumConverter&lt;ORMTypes, global::ORMSolutions.ORMArchitect.CustomProperties.CustomPropertiesDomainModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Name="CustomPropertyDataType" Namespace="ORMSolutions.ORMArchitect.CustomProperties" AccessModifier="Assembly">
			<Literals>
				<EnumerationLiteral Name="String" Value="0"/>
				<EnumerationLiteral Name="Integer" Value="1"/>
				<EnumerationLiteral Name="Decimal" Value="2"/>
				<EnumerationLiteral Name="DateTime" Value="3"/>
				<EnumerationLiteral Name="CustomEnumeration" Value="4"/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.EnumConverter&lt;CustomPropertyDataType, global::ORMSolutions.ORMArchitect.CustomProperties.CustomPropertiesDomainModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
	</Types>

	<XmlSerializationBehavior Name="CustomPropertyDomainModelSerializationBehavior" Namespace="ORMSolutions.ORMArchitect.CustomProperties"/>

</Dsl>