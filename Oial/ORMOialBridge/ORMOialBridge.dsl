<?xml version="1.0" encoding="utf-8"?>
<!--
	Neumont Object-Role Modeling Architect for Visual Studio

	Copyright Â© Neumont University. All rights reserved.

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
	Id="1F394F03-8A41-48BC-BDED-2268E131B4A3"
	Namespace="Neumont.Tools.ORMOialBridge"
	Name="ORMOialBridge"
	DisplayName="ORM/OIAL Bridge"
	Description="Bridges ORM and OIAL together."
	CompanyName="Neumont University"
	ProductName="Neumont ORM Architect for Visual Studio"
	MajorVersion="1" MinorVersion="0" Build="0" Revision="0">

	<Attributes>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;3EAE649F-E654-4D04-8289-C25D2C0322D8&quot;/*Neumont.Tools.ORM.ObjectModel.ORMCoreDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;F7BC82F4-83D1-408C-BA42-607E90B23BEA&quot;/*Neumont.Tools.Oial.OialDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
	</Attributes>

	<Relationships>
		<DomainRelationship Name="FactTypeMapsTowardsRole" Namespace="Neumont.Tools.ORMOialBridge" Id="98ABB729-F2F0-4629-BFA7-801B6615137D">
			<Properties>
				<DomainProperty Name="Depth" DisplayName="Depth" Id="720B9285-CC59-48E2-8B33-D9944A9ED400">
					<Type>
						<DomainEnumerationMoniker Name="MappingDepth"/>
					</Type>
				</DomainProperty>
			</Properties>
			<Source>
				<DomainRole Name="FactType" PropertyName="TowardsRole" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="FactType" Id="05ABB37C-0363-4BD5-BE02-F688BFDB55A5">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/FactType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="TowardsRole" PropertyName="FactType" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="TowardsRole" Id="89EBC0D2-380D-4434-9E1E-F5E9C6B7EB16">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/RoleBase"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="OialModelIsForORMModel" Namespace="Neumont.Tools.ORMOialBridge" Id="02969205-DC37-4D83-ACF2-506A6A3FE02C">
			<Source>
				<DomainRole Name="OialModel" PropertyName="ORMModel" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="OialModel" Id="FDB675B5-BFE3-4C5C-8B22-C5CFB0333811">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.Oial/OialModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ORMModel" PropertyName="OialModel" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ORMModel" Id="5021FD0A-C82F-4319-AC52-F99BDF7B9882">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ORMModel"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConceptTypeIsForObjectType" Namespace="Neumont.Tools.ORMOialBridge" Id="494EE309-435B-4DD1-B2DD-C7E794F768DB">
			<Source>
				<DomainRole Name="ConceptType" PropertyName="ObjectType" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ConceptType" Id="EE5F768C-B308-480E-A444-F86F81B02F46">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.Oial/ConceptType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ObjectType" PropertyName="ConceptType" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ObjectType" Id="02C44E70-F770-42A3-80E8-C2339A49687C">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConceptTypeChildHasPathFactType" Namespace="Neumont.Tools.ORMOialBridge" Id="A7D4FF78-1217-41C4-9C63-559FFBF2AF4B">
			<Source>
				<DomainRole Name="ConceptTypeChild" PropertyName="PathFactTypeCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ConceptTypeChild" Id="309ECB8E-8840-48FB-9591-EF74CC1B805C">
					<RolePlayer>
						<DomainRelationshipMoniker Name="/Neumont.Tools.Oial/ConceptTypeChild"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="PathFactType" PropertyName="ConceptTypeChild" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="PathFactType" Id="A8EAD929-E0A5-4DC3-8284-0813173BE908">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/FactType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="InformationTypeFormatIsForValueType" Namespace="Neumont.Tools.ORMOialBridge" Id="FB9FCDA4-030C-4F2F-8201-5287F79C25AF">
			<Source>
				<DomainRole Name="InformationTypeFormat" PropertyName="ValueType" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="InformationTypeFormat" Id="E7453C11-962F-4D37-9E69-F01E83810CC8">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.Oial/InformationTypeFormat"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ValueType" PropertyName="InformationTypeFormat" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ValueType" Id="EE9CF583-CD1B-4DF1-BAFB-C4C220B6D685">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="UniquenessIsForUniquenessConstraint" Namespace="Neumont.Tools.ORMOialBridge" Id="E61822C1-04EE-4AE3-A28E-F45879C8FE41">
			<Source>
				<DomainRole Name="Uniqueness" PropertyName="UniquenessConstraint" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="false" DisplayName="UniquenessConstraint" Id="4C49F284-81C1-41E8-8A23-137DEF1D229F">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.Oial/Uniqueness"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="UniquenessConstraint" PropertyName="Uniqueness" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="Uniqueness" Id="6D7DD635-6B8D-4EA7-A2A5-003ADA32AFD0">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/UniquenessConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
	</Relationships>

	<Types>
		<DomainEnumeration Namespace="Neumont.Tools.ORMOialBridge" Name="MappingDepth">
			<Literals>
				<EnumerationLiteral Name="Shallow" Value="0" Description="Only the &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.FactType&quot;/&gt; referenced is mapped to the destination &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.ObjectType&quot;/&gt;."/>
				<EnumerationLiteral Name="Deep" Value="1" Description="The &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.FactType&quot;/&gt; referenced is mapped to the destination &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.ObjectType&quot;/&gt;, and the &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.ObjectType&quot;/&gt; playing the opposite role is absorbed into the destination &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.ObjectType&quot;/&gt;."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;MappingDepth, global::Neumont.Tools.ORMOialBridge.ORMOialBridgeDomainModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
	</Types>

	<XmlSerializationBehavior Name="ORMOialBridgeDomainModelSerializationBehavior" Namespace="Neumont.Tools.ORMOialBridge"/>

</Dsl>