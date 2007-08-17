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
	Namespace="Neumont.Tools.ORMToORMAbstractionBridge"
	Name="ORMToORMAbstractionBridge"
	DisplayName="(Preview) ORM/ORMAbstraction Bridge"
	Description="Bridges ORM and and ORM Intermediate Abstraction Language."
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
				<AttributeParameter Value="&quot;F7BC82F4-83D1-408C-BA42-607E90B23BEA&quot;/*Neumont.Tools.ORMAbstraction.AbstractionDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
	</Attributes>

	<Relationships>
		<DomainRelationship Name="FactTypeMapsTowardsRole" Namespace="Neumont.Tools.ORMToORMAbstractionBridge" Id="98ABB729-F2F0-4629-BFA7-801B6615137D" HasCustomConstructor="true">
			<Properties>
				<DomainProperty Name="Depth" DisplayName="Depth" Id="720B9285-CC59-48E2-8B33-D9944A9ED400">
					<Type>
						<DomainEnumerationMoniker Name="MappingDepth"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="UniquenessPattern" DefaultValue="None" DisplayName="UniquenessPattern" Id="8789B1D0-9BE1-4C7A-B63B-D8D431AF5A47">
					<Type>
						<DomainEnumerationMoniker Name="MappingUniquenessPattern"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="MandatoryPattern" DefaultValue="None" DisplayName="MandatoryPattern" Id="1BEE0F25-5E38-4C51-8744-1BCB5FEB20CC">
					<Type>
						<DomainEnumerationMoniker Name="MappingMandatoryPattern"/>
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

		<DomainRelationship Name="AbstractionModelIsForORMModel" Namespace="Neumont.Tools.ORMToORMAbstractionBridge" Id="02969205-DC37-4D83-ACF2-506A6A3FE02C">
			<Source>
				<DomainRole Name="AbstractionModel" PropertyName="ORMModel" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="AbstractionModel" Id="FDB675B5-BFE3-4C5C-8B22-C5CFB0333811">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORMAbstraction/AbstractionModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ORMModel" PropertyName="AbstractionModel" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ORMModel" Id="5021FD0A-C82F-4319-AC52-F99BDF7B9882">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ORMModel"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConceptTypeIsForObjectType" Namespace="Neumont.Tools.ORMToORMAbstractionBridge" Id="494EE309-435B-4DD1-B2DD-C7E794F768DB">
			<Source>
				<DomainRole Name="ConceptType" PropertyName="ObjectType" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ConceptType" Id="EE5F768C-B308-480E-A444-F86F81B02F46">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORMAbstraction/ConceptType"/>
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

		<DomainRelationship Name="ConceptTypeChildHasPathFactType" Namespace="Neumont.Tools.ORMToORMAbstractionBridge" Id="A7D4FF78-1217-41C4-9C63-559FFBF2AF4B">
			<Source>
				<DomainRole Name="ConceptTypeChild" PropertyName="PathFactTypeCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ConceptTypeChild" Id="309ECB8E-8840-48FB-9591-EF74CC1B805C">
					<RolePlayer>
						<DomainRelationshipMoniker Name="/Neumont.Tools.ORMAbstraction/ConceptTypeChild"/>
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

		<DomainRelationship Name="InformationTypeFormatIsForValueType" Namespace="Neumont.Tools.ORMToORMAbstractionBridge" Id="FB9FCDA4-030C-4F2F-8201-5287F79C25AF">
			<Source>
				<DomainRole Name="InformationTypeFormat" PropertyName="ValueType" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="InformationTypeFormat" Id="E7453C11-962F-4D37-9E69-F01E83810CC8">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORMAbstraction/InformationTypeFormat"/>
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

		<DomainRelationship Name="UniquenessIsForUniquenessConstraint" Namespace="Neumont.Tools.ORMToORMAbstractionBridge" Id="E61822C1-04EE-4AE3-A28E-F45879C8FE41">
			<Source>
				<DomainRole Name="Uniqueness" PropertyName="UniquenessConstraint" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="false" DisplayName="UniquenessConstraint" Id="4C49F284-81C1-41E8-8A23-137DEF1D229F">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORMAbstraction/Uniqueness"/>
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

		<DomainRelationship Name="ExcludedORMModelElement" Namespace="Neumont.Tools.ORMToORMAbstractionBridge" Id="6A9A8EFB-4DB5-49A2-8818-D66BED21D590">
			<Source>
				<DomainRole Name="ExcludedElement" PropertyName="AbstractionModel" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ExcludedElement" Id="7DCCCA8C-9283-4167-9CC2-79CAEA5DA008">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ORMModelElement"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="AbstractionModel" PropertyName="ExcludedElement" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="AbstractionModel" Id="AFBD0D81-5EF6-4D0E-A4CB-0145F8EA939B">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORMAbstraction/AbstractionModel"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
	</Relationships>

	<Types>
		<DomainEnumeration Namespace="Neumont.Tools.ORMToORMAbstractionBridge" Name="MappingDepth" Description="Specify whether a mapping is shallow (absorbs just the &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.FactType&quot;/&gt; only) or deep (absorbs the &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.FactType&quot;/&gt; and the opposite &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.ObjectType&quot;&gt;role player&lt;/see&gt;.">
			<Literals>
				<EnumerationLiteral Name="Shallow" Value="0" Description="Only the &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.FactType&quot;/&gt; referenced is mapped to the destination &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.ObjectType&quot;/&gt;."/>
				<EnumerationLiteral Name="Deep" Value="1" Description="The &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.FactType&quot;/&gt; referenced is mapped to the destination &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.ObjectType&quot;/&gt;, and the &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.ObjectType&quot;/&gt; playing the opposite role is absorbed into the destination &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.ObjectType&quot;/&gt;."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;MappingDepth, global::Neumont.Tools.ORMToORMAbstractionBridge.ORMToORMAbstractionBridgeDomainModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
				<ClrAttribute Name="global::System.Serializable"/>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Namespace="Neumont.Tools.ORMToORMAbstractionBridge" Name="MappingUniquenessPattern" Description="Specifies the uniqueness pattern present on the &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.FactType&quot;/&gt; at the time the &lt;see cref=&quot;FactTypeMapsTowardsRole&quot;/&gt; relationship was last updated.">
			<Literals>
				<EnumerationLiteral Name="None" Value="0" Description="Uniqueness pattern not specified"/>
				<EnumerationLiteral Name="OneToMany" Value="1" Description="The mapping is based on a one-to-many &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.FactType&quot;/&gt; with a &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.UniquenessConstraint&quot;/&gt; on the &lt;see cref=&quot;FactTypeMapsTowardsRole.TowardsRole&quot;/&gt;."/>
				<EnumerationLiteral Name="ManyToOne" Value="2" Description="The mapping is based on a many-to-one &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.FactType&quot;/&gt; with a &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.UniquenessConstraint&quot;/&gt; on the role opposite &lt;see cref=&quot;FactTypeMapsTowardsRole.TowardsRole&quot;/&gt;. Note that this value is included for completeness and will not appear in actual &lt;see cref=&quot;FactTypeMapsTowardsRole&quot;/&gt; instances."/>
				<EnumerationLiteral Name="OneToOne" Value="3" Description="The mapping is based on a one-to-one &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.FactType&quot;/&gt;."/>
				<EnumerationLiteral Name="Subtype" Value="4" Description="The mapping is based on a &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.SubtypeFact&quot;/&gt; with the &lt;see cref=&quot;FactTypeMapsTowardsRole.TowardsRole&quot;/&gt; acting as the superttype."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;MappingUniquenessPattern, global::Neumont.Tools.ORMToORMAbstractionBridge.ORMToORMAbstractionBridgeDomainModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
				<ClrAttribute Name="global::System.Serializable"/>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Namespace="Neumont.Tools.ORMToORMAbstractionBridge" Name="MappingMandatoryPattern" Description="Specifies the mandatory pattern present on the &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.FactType&quot;/&gt; at the time the &lt;see cref=&quot;FactTypeMapsTowardsRole&quot;/&gt; relationship was last updated. Indicated mandatory relationships include single-role implied mandatory constraints.">
			<Literals>
				<EnumerationLiteral Name="None" Value="0" Description="Mandatory pattern not specified"/>
				<EnumerationLiteral Name="NotMandatory" Value="1" Description="The &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.FactType&quot;/&gt; has no mandatory roles."/>
				<EnumerationLiteral Name="TowardsRoleMandatory" Value="2" Description="The &lt;see cref=&quot;FactTypeMapsTowardsRole.TowardsRole&quot;/&gt; of the &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.FactType&quot;/&gt; is mandatory."/>
				<EnumerationLiteral Name="OppositeRoleMandatory" Value="3" Description="The role opposite &lt;see cref=&quot;FactTypeMapsTowardsRole.TowardsRole&quot;/&gt; of the &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.FactType&quot;/&gt; is mandatory."/>
				<EnumerationLiteral Name="BothRolesMandatory" Value="4" Description="Both the &lt;see cref=&quot;FactTypeMapsTowardsRole.TowardsRole&quot;/&gt; and opposite role of the &lt;see cref=&quot;Neumont.Tools.ORM.ObjectModel.FactType&quot;/&gt; are mandatory."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;MappingMandatoryPattern, global::Neumont.Tools.ORMToORMAbstractionBridge.ORMToORMAbstractionBridgeDomainModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
				<ClrAttribute Name="global::System.Serializable"/>
			</Attributes>
		</DomainEnumeration>
	</Types>

	<XmlSerializationBehavior Name="ORMToORMAbstractionBridgeDomainModelSerializationBehavior" Namespace="Neumont.Tools.ORMToORMAbstractionBridge"/>

</Dsl>