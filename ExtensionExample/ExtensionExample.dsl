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
	Id="9F620B5A-9A99-45A4-A022-C9ED95CE85D6"
	Namespace="ExtensionExample"
	Name="ExtensionDomainModel"
	DisplayName="My Custom Extension"
	Description="The extension created for testing purposes"
	CompanyName="Neumont University"
	ProductName="Neumont ORM Architect for Visual Studio"
	MajorVersion="1" MinorVersion="0" Build="0" Revision="0">

	<Attributes>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="3EAE649F-E654-4D04-8289-C25D2C0322D8"/>
			</Parameters>
		</ClrAttribute>
	</Attributes>
	
	<Classes>
		<DomainClass Name="MyCustomExtensionElement" Namespace="ExtensionExample" Id="14DB7E59-72E3-441F-9993-88FB3E3C01B3" DisplayName="MyCustomExtensionElement">
			<Properties>
				<DomainProperty Name="TestProperty" DisplayName="TestProperty" Id="6825C613-7E2A-4D14-8277-0DB3B86B1210" DefaultValue="Default value">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ExtensionExample.TestElementPicker)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="CustomEnum" DisplayName="CustomEnum" Id="26AAF88D-2051-4D87-B863-BF330D7123BB" DefaultValue="Zero">
					<Type>
						<DomainEnumerationMoniker Name="TestEnumeration"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
		<DomainClass Name="ObjectTypeRequiresMeaningfulNameError" Namespace="ExtensionExample" Id="B9448302-BEB4-451E-BFD8-CB824201784C" DisplayName="ObjectTypeRequiresMeaningfulNameError">
			<BaseClass>
				<DomainClassMoniker Name="/Microsoft.VisualStudio.Modeling/ModelElement"/>
				<!--<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ModelError"/>-->
			</BaseClass>
		</DomainClass>
	</Classes>

	<Relationships/>

	<Types>
		<ExternalType Namespace="System" Name="String"/>
		<ExternalType Namespace="Neumont.Tools.ORM.ObjectModel" Name="ModelError"/>
		<DomainEnumeration Namespace="ExtensionExample" Name="TestEnumeration" Description="Provides test values for our enum sample dropdown.">
			<Literals>
				<EnumerationLiteral Name="Zero" Value="0"/>
				<EnumerationLiteral Name="One" Value="1"/>
				<EnumerationLiteral Name="Two" Value="2"/>
				<EnumerationLiteral Name="Three" Value="3"/>
			</Literals>
		</DomainEnumeration>
	</Types>

	<Shapes/>

	<Connectors/>

	<XmlSerializationBehavior Name="ExtensionDomainModelSerializationBehavior" Namespace="ExtensionExample"/>

	<ExplorerBehavior Name="ExtensionDomainModelExplorer"/>

	<ConnectionBuilders/>

</Dsl>