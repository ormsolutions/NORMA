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
	Id="B2CAED8E-4155-4317-9405-55006FDE280E"
	Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker"
	PackageNamespace="ORMSolutions.ORMArchitect.EntityRelationshipModels"
	Name="Barker"
	DisplayName="Barker ER"
	Description="Barker Entity Relationship View of ORM Model"
	CompanyName="ORM Solutions, LLC"
	ProductName="Natural Object-Role Modeling Architect for Visual Studio"
	MajorVersion="1" MinorVersion="0" Build="0" Revision="0">

	<Attributes>
		<!-- UNDONE: This is a temporary hack to get a reasonable order in the .orm file. The NORMA framework needs to add a
		LoadPriority attribute to enable these to be explicitly controlled. -->
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;1F394F03-8A41-48BC-BDED-2268E131B4A3&quot;/*ORMSolutions.ORMArchitect.ORMToORMAbstractionBridge.ORMToORMAbstractionBridgeDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
	</Attributes>

	<Classes>
		<DomainClass Id="F8F5677E-5632-4A48-BF24-18F4D32DB589" Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker" Name="BarkerModelElement" InheritanceModifier="Abstract" Description="Base class for ConceptualData &lt;see cref='DslModeling::ModelElement'/>s.">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptionProvider&lt;BarkerModelElement, Design.BarkerModelElementTypeDescriptor&lt;BarkerModelElement&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainClass>
		<DomainClass Id="4AA67E55-0269-49B9-A580-62AE22EDDDED" Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker" Name="EntityType">
			<BaseClass>
				<DomainClassMoniker Name="BarkerModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Id="BAE233D4-8EA9-4B39-A402-B4AEF480E68E" Name="Name" IsElementName="true" Description="The name of the entity type.">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;name&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
		<DomainClass Id="2A1C3209-1E17-4E6F-8B2D-2BD051610A29" Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker" Name="Attribute">
			<BaseClass>
				<DomainClassMoniker Name="BarkerModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Id="6B20E133-FD6E-4034-AC73-1450ABD115FC" Name="Name" IsElementName="true" Description="The name of the attribute.">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;name&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>

				<DomainProperty Id="73BF0465-613A-4B97-8293-827448E0DBC6" Name="IsMandatory" DisplayName="IsMandatory" Description="Is this attribute required for its entity type?">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;isMandatory&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				
				<DomainProperty Id="7F5E4C3E-7020-4664-B4B6-FD2EBBDCCDC5" Name="IsPrimaryIdComponent" DisplayName="IsPrimaryIdComponent" Description="Is this attribute part of primary identifier for its entity type?">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;isPrimaryIdComponent&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
		<DomainClass Id="B34CDD9E-F23C-4190-9194-4260F4DD9A59" Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker" Name="Value">
			<BaseClass>
				<DomainClassMoniker Name="BarkerModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Id="7BBD6637-C6C2-489A-B18E-E77C92FAA9BC" Name="Val" Description="represents the actual value.">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;val&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
		<DomainClass Id="6E5B3828-8A31-4210-B288-6FF1D858686E" Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker" Name="Role">
			<BaseClass>
				<DomainClassMoniker Name="BarkerModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Id="0FA61AF6-3123-4FF8-9232-2F1CE0F94060" Name="Number" Description="The number of the role.">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;number&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
				<DomainProperty Id="736301C6-F806-4AEC-91B7-FECE4D7DA10B" Name="PredicateText" Description="The predicate text of the role.">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;predicateText&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Id="D4F77973-EB66-4940-8D70-5F58EDAD45E6" Name="IsNonTransferable" DisplayName="IsNonTransferable" Description="Is this role non-transferable?">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;isNonTransferable&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Id="BB635854-08AA-4555-9917-1BBA75599B14" Name="IsMandatory" DisplayName="IsMandatory" Description="Is this a required role?">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;isMandatory&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Id="A5843D76-17F3-4A8D-9A8C-331ACEEDCC60" Name="IsPrimaryIdComponent" DisplayName="IsPrimaryIdComponent" Description="Is this role a part of primary identifier for an entity type?">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;isPrimaryIdComponent&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Id="D7959BF6-7F67-4F37-9914-52D76D1388D6" Name="IsMultiValued" DisplayName="IsMultiValued" Description="Does this role have cardinality specified?">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;isMultiValued&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
		<DomainClass Id="80EF7F9F-B60A-4929-8D99-6BD734F91056" Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker" Name="CardinalityQualifier">
			<BaseClass>
				<DomainClassMoniker Name="BarkerModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Id="0B246974-931A-47E1-B7CD-810D6EC1353D" Name="Number" Description="The number of times something can occur.">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;number&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
				<DomainProperty Id="2BF2033D-E5BA-4F6E-A02E-54E6E4E32A89" Name="Operator" Description="The comparison operator that goes with the cardinality number.">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;operator&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<DomainEnumerationMoniker Name="/ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker/OperatorSymbol"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
		<DomainClass Id="958EDC63-93F3-40B2-818D-DC2C764E4DFB" Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker" Name="OptionalRole">
			<BaseClass>
				<DomainClassMoniker Name="Role"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Id="63311551-B395-492E-B59F-CAA6F521494C" Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker" Name="ExclusiveArc">
			<BaseClass>
				<DomainClassMoniker Name="BarkerModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Id="377D74D9-E695-4DBD-919E-B970F75D6339" Name="Number" Description="The number of the exclusive arc.">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;number&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
				<DomainProperty Id="8C44FB8C-0953-44FF-997D-41882BA6B18F" Name="IsMandatory" DisplayName="IsMandatory" Description="Is one of the roles under exclusive arc required?">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;isMandatory&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Id="ED4DE2FC-330E-44DA-8E57-63CF8614A8B6" Name="IsPrimaryIdComponent" DisplayName="IsPrimaryIdComponent" Description="Is one of the roles under exclusive arc a part of primary identifier for an entity type?">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;isPrimaryIdComponent&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
		<DomainClass Id="AB570194-BCC8-4448-A022-BB07021D5EE5" Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker" Name="BinaryAssociation">
			<BaseClass>
				<DomainClassMoniker Name="BarkerModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Id="D0058027-F844-4A3A-850F-3E15D10F147A" Name="Number" Description="The number of the binary association.">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;number&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
		<!--not sure if we need this class - i created it only to be the root in serialization extensions-->
		<DomainClass Id="A96FCCEA-0FCC-4207-AC77-F69C0BF2C0CB" Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker" Name="BarkerErModel">
			<BaseClass>
				<DomainClassMoniker Name="BarkerModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Id="5FE133BF-1CB7-48D7-9BF2-9C4AA9223FD3" Name="Name" IsElementName="true" Description="The name of the model.">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;name&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
	</Classes>

	<Relationships>
		<DomainRelationship Id="FF7D1A21-8F7F-4392-8BC4-C75E2B44464B" Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker" Name="EntityTypeHasAttribute" IsEmbedding="true">
			<Source>
				<DomainRole Id="60179D80-596A-4646-A240-7ED97AAEDCE2" Name="EntityType" PropertyName="AttributeCollection" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="EntityType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="9F98B07B-EA08-4C6B-AE5D-6EA871155650" Name="Attribute" PropertyName="EntityType" Multiplicity="One" PropagatesDelete="true">
					<RolePlayer>
						<DomainClassMoniker Name="Attribute"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<!--
		<DomainRelationship Id="2D69BA1A-06C0-4491-B85A-2D1ED959DE68" Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker" Name="AttributeIsBasedOnDomain">
			<Source>
				<DomainRole Id="63E6C1D2-DCF0-490A-90E0-BCBA2BE02FFF" Name="Attribute" PropertyName="Domain" Multiplicity="ZeroOne">
					<RolePlayer>
						<DomainClassMoniker Name="Attribute"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="521CAA45-6BC2-404A-9332-7B3F26351C38" Name="Domain" PropertyName="AttributeCollection" Multiplicity="ZeroMany"  PropagatesDelete="true">
					<RolePlayer>
						<DomainClassMoniker Name="Domain"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		-->
		<DomainRelationship Id="D1BFE73F-E05D-48D2-895B-B4DAAA60908A" Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker" Name="AttributeHasPossibleValue" IsEmbedding="true">
			<Source>
				<DomainRole Id="2C5B6EF2-CDF7-4A42-A90F-A08BF4C36C9E" Name="Attribute" PropertyName="PossibleValuesCollection" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="Attribute"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="CFFA86F5-0271-4416-AD85-D52934459D56" Name="Value" PropertyName="Attribute" Multiplicity="One" >
					<RolePlayer>
						<DomainClassMoniker Name="Value"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<!--
		<DomainRelationship Id="13980B73-2D59-4EF9-B167-985942E211F5" Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker" Name="DomainHasPossibleValue">
			<Source>
				<DomainRole Id="00ED7745-BF37-408F-880A-3BB9607428FF" Name="Domain" PropertyName="PossibleValuesCollection" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="Domain"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="52603D09-964B-4E97-966B-D3DEED324BCA" Name="Value" PropertyName="DomainCollection" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="Value"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		-->
		<DomainRelationship Id="0BDBB7E8-8D3C-4C09-A7FC-9086B04A494E" Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker" Name="EntityTypeIsSubtypeOfEntityType">
			<Source>
				<DomainRole Id="87A158DB-AE78-4282-811C-8BCBC82F24E7" Name="Subtype" PropertyName="Supertype" Multiplicity="ZeroOne">
					<RolePlayer>
						<DomainClassMoniker Name="EntityType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="C19B7670-8950-425F-96C6-E4EBC546F279" Name="Supertype" PropertyName="SubtypesCollection" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="EntityType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="A7C02288-3E60-4AA5-A509-4F900D42CE46" Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker" Name="EntityTypePlaysRole">
			<Source>
				<DomainRole Id="C88332B5-35EB-40FC-9FA6-A0D74F0D6337" Name="EntityType" PropertyName="RoleCollection" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="EntityType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="FE8EB808-17FA-4F28-AA8F-19A240F9791D" Name="Role" PropertyName="EntityType" Multiplicity="One" PropagatesDelete="true">
					<RolePlayer>
						<DomainClassMoniker Name="Role"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="8BD9C7C4-41C6-42D1-B3F6-E124110C9F20" Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker" Name="RoleHasCardinalityQualifier" IsEmbedding="true">
			<Source>
				<DomainRole Id="44710950-5BA1-492D-826D-6D59FB659E56" Name="Role" PropertyName="CardinalityQualifier" Multiplicity="ZeroOne">
					<RolePlayer>
						<DomainClassMoniker Name="Role"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="C910C3D7-0E8E-4657-BB3B-56B3F84FF7D8" Name="CardinalityQualifier" PropertyName="Role" Multiplicity="One">
					<RolePlayer>
						<DomainClassMoniker Name="CardinalityQualifier"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="0CAEDC0D-9450-4CE1-B61A-DF55DF75FBEA" Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker" Name="ExclusiveArcSpansOptionalRole">
			<Source>
				<DomainRole Id="5F2B9A73-0D8B-4CB8-BBE0-E09317086E20" Name="ExclusiveArc" PropertyName="RoleCollection" Multiplicity="OneMany">
					<RolePlayer>
						<DomainClassMoniker Name="ExclusiveArc"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="7890570B-8556-46BF-9AD1-7F11B6A18906" Name="ConstrainedRole" PropertyName="ExclusiveArc" Multiplicity="ZeroOne">
					<RolePlayer>
						<DomainClassMoniker Name="Role"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="BCAC40EF-38D3-4548-9BBC-804A11E57E08" Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker" Name="BinaryAssociationContainsRole" IsEmbedding="true">
			<Source>
				<DomainRole Id="C9B8667E-A51D-47DC-A195-4F4D4CDC2BDA" Name="BinaryAssociation" PropertyName="RoleCollection" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="BinaryAssociation"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="A70F9695-2F99-4483-9AC7-9314CE08D14B" Name="Role" PropertyName="BinaryAssociation" Multiplicity="One">
					<RolePlayer>
						<DomainClassMoniker Name="Role"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<!--not sure if we need these relationships - i created them only to be containers in the root in serialization extensions-->
		<DomainRelationship Id="2CCB5738-115C-4470-BDBB-DA4280ACD994" Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker" Name="BarkerErModelContainsEntityType" IsEmbedding="true">
			<Source>
				<DomainRole Id="B9AA4A9F-0414-44F1-84A7-80E9D6984A12" Name="BarkerErModel" PropertyName="EntityTypeCollection" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="BarkerErModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="8EA97980-B6E3-45A5-8447-6CEEBF5D62DE" Name="EntityType" PropertyName="BarkerErModel" Multiplicity="One" PropagatesDelete="true">
					<RolePlayer>
						<DomainClassMoniker Name="EntityType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="734A6758-65E4-40F9-B4EC-8B99DEC314AF" Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker" Name="BarkerErModelContainsBinaryAssociation" IsEmbedding="true">
			<Source>
				<DomainRole Id="400580D3-EB5B-4230-BD4F-E69C33C72751" Name="BarkerErModel" PropertyName="BinaryAssociationCollection" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="BarkerErModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="7BD4B726-1526-47EE-9E4C-A887A52A7E27" Name="BinaryAssociation" PropertyName="BarkerErModel" Multiplicity="One" PropagatesDelete="true">
					<RolePlayer>
						<DomainClassMoniker Name="BinaryAssociation"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="B3EC67E1-4E16-47BD-A93A-F798F1765292" Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker" Name="BarkerErModelContainsExclusiveArc" IsEmbedding="true">
			<Source>
				<DomainRole Id="ACF7DAB7-7550-4B64-81E3-2F94F1BFAF25" Name="BarkerErModel" PropertyName="ExclusiveArcCollection" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="BarkerErModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="153A27E8-B3DE-4C63-B7B1-918C09A95645" Name="ExclusiveArc" PropertyName="BarkerErModel" Multiplicity="One" PropagatesDelete="true">
					<RolePlayer>
						<DomainClassMoniker Name="ExclusiveArc"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
	</Relationships>

	<Types>
		<DomainEnumeration Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker" Name="OperatorSymbol" Description="Valid values for comparison operators.">
			<Literals>
				<EnumerationLiteral Name="Equal" Value="0">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;EQUAL&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="LessThan" Value="1">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;LESS_THAN&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="GreaterThan" Value="2">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;GREATER_THAN&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="LessThanOrEqualTo" Value="3">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;LESS_THAN_OR_EQUAL_TO&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="GreaterThanOrEqualTo" Value="4">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;GREATER_THAN_OR_EQUAL_TO&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.Serializable"/>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.EnumConverter&lt;OperatorSymbol, global::ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker.BarkerErModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
	</Types>

	<XmlSerializationBehavior Name="BerDomainModelSerializationBehavior" Namespace="ORMSolutions.ORMArchitect.EntityRelationshipModels.Barker"/>

	<DslLibraryImports>
		<DslLibraryImport FilePath="..\..\ORMModel\Framework\SystemCore.dsl"/>
		<!-- UNDONE: This is a temporary hack to get a reasonable order in the .orm file. The NORMA framework needs to add a
		LoadPriority attribute to enable these to be explicitly controlled. -->
		<DslLibraryImport FilePath="..\..\Oial\ORMOialBridge\ORMOialBridge.dsl"/>
	</DslLibraryImports>
</Dsl>