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
	Id="CEDE46B1-9CA1-4C55-BC88-3DACFADD70EA"
	Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase"
	PackageNamespace="Neumont.Tools.RelationalModels"
	Name="ConceptualDatabase"
	DisplayName="Conceptual Database"
	CompanyName="Neumont University"
	ProductName="Neumont ORM Architect for Visual Studio"
	Description="Relational Database View of ORM Model"
	MajorVersion="1" MinorVersion="0" Build="0" Revision="0">

	<Classes>
		<DomainClass Id="35796255-F8FB-4D5E-A2CE-B3D48911EBEB" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="ConceptualDatabaseModelElement" InheritanceModifier="Abstract" Description="Base class for ConceptualData &lt;see cref='DslModeling::ModelElement'/>s.">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.ElementTypeDescriptionProvider&lt;ConceptualDatabaseModelElement, Design.ConceptualDatabaseElementTypeDescriptor&lt;ConceptualDatabaseModelElement&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainClass>
		<DomainClass Id="0A5DCA22-AF17-4C53-9BAF-B7DA1650119C" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="Catalog" Description="A named collection of schemas (commonly referred to as a database). Equivalent to a 'CATALOG' in the SQL Standard.">
			<BaseClass>
				<DomainClassMoniker Name="ConceptualDatabaseModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Id="AEDFA9D8-02DE-47EC-ABD0-B78399C7F9EB" Name="Name" IsElementName="true" Description="The name of the catalog.">
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
		<DomainClass Id="DDBACED7-C013-419B-A305-9937379038D0" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="Schema" Description="Equivalent to a 'SCHEMA' in the SQL Standard.">
			<BaseClass>
				<DomainClassMoniker Name="ConceptualDatabaseModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Id="DDEE5918-35B9-476C-BB21-31E9E132FA6F" Name="Name" IsElementName="true" Description="The name of the schema.">
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
		<DomainClass Id="99E0B931-A6B9-4248-B6DE-5AFD95BBB21A" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="Table" Description="Equivalent to a 'TABLE' in the SQL Standard.">
			<BaseClass>
				<DomainClassMoniker Name="ConceptualDatabaseModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Id="0A14B5D9-1988-4736-A243-D7147DCC74E9" Name="Name" IsElementName="true" Description="The name of the table.">
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
		<DomainClass Id="16628BD3-D761-4C6A-816E-C98AEFBADC41" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="Column" Description="Equivalent to a 'COLUMN' in the SQL Standard.">
			<BaseClass>
				<DomainClassMoniker Name="ConceptualDatabaseModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Id="3F8B881E-244C-4B4E-96E9-05147D4C6471" Name="Name" IsElementName="true" Description="The name of the column.">
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
				<DomainProperty Id="2FD3C751-BD3D-44EA-94E1-6F318FE25A07" Name="IsNullable" Description="Is NULL a valid value for this column?">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;isNullable&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Id="BEAD460A-E2BA-417D-B36E-182833217F9A" Name="IsIdentity" Description="Is this an IDENTITY column?">
					<Notes>If 'true' is specified for this attribute, the generationCode element must not be present.</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;isIdentity&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<!-- UNDONE: How are we going to represent the formula used to compute a column? (For now, we're not...) -->
				<!--<DomainProperty Id="35765C03-7DCC-4FBB-85D0-D4A876B9E846" Name="generationCode" Description="The code used in the generation / computation of this column.">
					<Notes>This element may only be present if @isIdentity is false.</Notes>
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>-->
			</Properties>
		</DomainClass>
		<DomainClass Id="A966AAA8-E770-4696-8865-A1396B7871BD" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="Constraint" InheritanceModifier="Abstract" Description="Base class for ConceptualData constraints.">
			<BaseClass>
				<DomainClassMoniker Name="ConceptualDatabaseModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Id="E5B7177F-C2C6-4777-B917-7847930E34EC" Name="Name" IsElementName="true" Description="The name of the constraint.">
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
		<DomainClass Id="18DC9CAA-3F7B-49E2-8DB3-71898C66423A" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="UniquenessConstraint" Description="A constraint on the uniqueness of a set of columns. Equivalent to a 'UNIQUE' or 'PRIMARY KEY' in the SQL Standard.">
			<BaseClass>
				<DomainClassMoniker Name="Constraint"/>
			</BaseClass>
			<Properties>
				<DomainProperty Id="F09AC57C-454B-48D7-BE68-53A5CE64B8F9" Name="IsPrimary" Description="Is this uniqueness constraint a PRIMARY KEY?">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;isPrimary&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
		<DomainClass Id="17F929CE-D332-40F6-BCE3-6A7901790FE3" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="ReferenceConstraint" Description="A reference to a &lt;see cref='UniquenessConstraint'/> in another table. Equivalent to a 'FOREIGN KEY' in the SQL Standard.">
			<BaseClass>
				<DomainClassMoniker Name="Constraint"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Id="A8F18E46-FC02-4DC1-AF0A-47FA4C5D8DDC" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="CheckConstraint" Description="Equivalent to a table-level or domain 'CHECK' clause in the SQL Standard.">
			<BaseClass>
				<DomainClassMoniker Name="Constraint"/>
			</BaseClass>
			<Properties>
				<!-- UNDONE: How are we going to represent the formula of the check constraint? (For now, we're not...) -->
				<!--<DomainProperty Name="Code">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>-->
			</Properties>
		</DomainClass>
		<DomainClass Id="C06C8520-E087-4A6A-8E41-24A195773EDC" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="Domain" Description="A user-defined data domain, which can have custom restrictions on it.">
			<BaseClass>
				<DomainClassMoniker Name="ConceptualDatabaseModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Id="B0681A83-3DBE-4520-BF28-E039927BC184" Name="Name" IsElementName="true" Description="The name of the data domain.">
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
		<DomainClass Id="14B0A424-4646-4B01-80AA-F67D118F46CA" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="PredefinedDataType">
			<BaseClass>
				<DomainClassMoniker Name="ConceptualDatabaseModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Id="9F52CE66-BB82-42F3-811F-0ECAFAB205B5" Name="Name" Description="The name of the predefined type.">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;name&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<DomainEnumerationMoniker Name="/Neumont.Tools.RelationalModels.DatabaseDefinition/PredefinedType"/>
					</Type>
				</DomainProperty>
				<DomainProperty Id="C1982D15-4BFF-4075-8AB3-6BA723A88915" Name="Length" Description="The number of characters in a CHARACTER, CHARACTER VARYING, or CHARACTER LARGE OBJECT, or the number of bytes in a BINARY LARGE OBJECT.">
					<Notes>
						If this is any other data type, a value for this attribute must not be specified.
					</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;length&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
				<DomainProperty Id="FD15650F-454E-464F-8CE0-DFFE5551EED5" Name="Precision" Description="The maximum number of decimal digits in a NUMERIC or DECIMAL, or the maximum number of binary digits in the significand (mantissa) of a FLOAT.">
					<Notes>
						If this is any other data type, a value for this attribute must not be specified.
					</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;precision&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
				<DomainProperty Id="989B8033-13D5-455D-87BA-6807DE8579B2" Name="Scale" Description="The maximum number of decimal digits after the decimal point in a NUMERIC or DECIMAL.">
					<Notes>
						If this is any other data type, a value for this attribute must not be specified.
					</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;scale&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
		<!--<DomainClass Id="22200B9F-5E51-4CE7-86E4-0A94A0EE2A46" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="Trigger" Description="Equivalent to a 'TRIGGER' in the SQL Standard.">
			<BaseClass>
				<DomainClassMoniker Name="ConceptualDatabaseModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Id="35E9F3CC-97EA-41E7-A43B-D20BBC7E61F1" Name="Name" IsElementName="true" Description="The name of the trigger.">
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
				<DomainProperty Name="referencing" Description="Equivalent to the 'REFERENCING' clause in the SQL Standard.">
					<Type>
						<DomainEnumerationMoniker Name="TriggerTarget"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="when" Description="Equivalent to a search condition in the SQL Standard.">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="atomicBlock" Description="">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="targetTable" Description="The name of the table being referenced.">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="actionTime" Description="When the trigger fires.">
					<Type>
						<DomainEnumerationMoniker Name="TriggerActionTime"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="event" Description="The event that causes the trigger to fire.">
					<Type>
						<DomainEnumerationMoniker Name="TriggerEvent"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="forEach" Description="The level fo the trigger">
					<Type>
						<DomainEnumerationMoniker Name="TriggerForEach"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>-->
		<!--<DomainClass Id="1D1E4A80-F62A-401A-9096-3191B8FD90DC" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="Procedure" Description="Equivalent to a 'PROCEDURE' in the SQL standard.">
			<BaseClass>
				<DomainClassMoniker Name="ConceptualDatabaseModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Id="EECE03DD-D3BF-41B0-850E-34B916E463FA" Name="Name" IsElementName="true" Description="The name of the procedure.">
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
				<DomainProperty Id="857A2E6F-AB9C-4CB5-8F48-F1991E477F2C" Name="SqlDataAccessIndication">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlAttribute">
							<Parameters>
								<AttributeParameter Value="&quot;sqlDataAccessIndication&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<DomainEnumerationMoniker Name="/Neumont.Tools.RelationalModels.DatabaseDefinition/SqlDataAccessIndication"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>-->
	</Classes>

	<Relationships>
		<DomainRelationship Id="7CBDB2CD-2E18-4F16-BAE8-C81D0F72F90D" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="CatalogContainsSchema" IsEmbedding="true">
			<Source>
				<DomainRole Id="F7B037D9-D101-463A-8BDE-62EEBD5430F3" Name="Catalog" PropertyName="SchemaCollection" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="Catalog"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<!-- UNDONE: Strictly speaking this role should have a multiplicity of One rather than ZeroOne, but we haven't yet decided how / if we are going to represent catalogs. -->
				<DomainRole Id="0C3B8302-B281-4DC4-B5B7-D57F7E025E01" Name="Schema" PropertyName="Catalog" Multiplicity="ZeroOne" PropagatesDelete="true">
					<RolePlayer>
						<DomainClassMoniker Name="Schema"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Id="CE3DCF73-DA08-4116-A95A-BD9BC2A9D418" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="SchemaContainsContent" IsEmbedding="true" InheritanceModifier="Abstract">
			<Source>
				<DomainRole Id="6D64020B-CF32-4AC6-A74B-59E2F6421B65" Name="Schema" PropertyName="SchemaContent" IsPropertyGenerator="false" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="Schema"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="6A22B218-D701-47D9-8529-C1CFA7826D30" Name="SchemaContent" PropertyName="Schema" IsPropertyGenerator="false" Multiplicity="ZeroOne" PropagatesDelete="true">
					<RolePlayer>
						<DomainClassMoniker Name="ConceptualDatabaseModelElement"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="3AC7CBD0-A619-4F31-8C8C-F0AA586D2C69" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="SchemaContainsTable" IsEmbedding="true">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="SchemaContainsContent"/>
			</BaseRelationship>
			<Source>
				<DomainRole Id="CC145E8C-EBC7-4BBF-9E70-24F76C4B69A9" Name="Schema" PropertyName="TableCollection" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="Schema"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="DF1F7F50-B459-4E7B-8958-BAA88E685268" Name="Table" PropertyName="Schema" Multiplicity="One" PropagatesDelete="true">
					<RolePlayer>
						<DomainClassMoniker Name="Table"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="4E787A64-2A01-4AF0-A28B-35052693B1AB" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="SchemaContainsDomain" IsEmbedding="true">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="SchemaContainsContent"/>
			</BaseRelationship>
			<Source>
				<DomainRole Id="BF0668B8-0CB5-49D0-A1B7-4637A16E2625" Name="Schema" PropertyName="DomainCollection" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="Schema"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="591B91CC-8E0B-498D-A1DE-C2CA8440EBEE" Name="Domain" PropertyName="Schema" Multiplicity="One" PropagatesDelete="true">
					<RolePlayer>
						<DomainClassMoniker Name="Domain"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<!--<DomainRelationship Id="6BB09D05-C03F-4785-B26F-73DC97255734" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="SchemaContainsTrigger" IsEmbedding="true">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="SchemaContainsContent"/>
			</BaseRelationship>
			<Source>
				<DomainRole Id="B7B5FAE0-9C48-408B-B4AD-03B65CEE1B78" Name="Schema" PropertyName="TriggerCollection" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="Schema"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="93C3C74D-576A-40F1-9E5A-B38818E0E2F5" Name="Trigger" PropertyName="Schema" Multiplicity="One" PropagatesDelete="true">
					<RolePlayer>
						<DomainClassMoniker Name="Trigger"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>-->
		<!--<DomainRelationship Id="E918781A-0363-48CA-A215-A6BFFCB62726" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="SchemaContainsProcedure" IsEmbedding="true">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="SchemaContainsContent"/>
			</BaseRelationship>
			<Source>
				<DomainRole Id="9946FB8B-7F94-4731-BD9B-A255335EDF09" Name="Schema" PropertyName="ProcedureCollection" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="Schema"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="8DBD22F4-CF4F-4DAE-9B9B-83E585D581B5" Name="Procedure" PropertyName="Schema" Multiplicity="One" PropagatesDelete="true">
					<RolePlayer>
						<DomainClassMoniker Name="Procedure"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>-->

		<DomainRelationship Id="FB5B6B20-8A4E-4BA3-BC8B-FBEAADBF9C33" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="TableContainsColumn" IsEmbedding="true">
			<Source>
				<DomainRole Id="C3A52815-E9B9-4757-9985-717E54F884ED" Name="Table" PropertyName="ColumnCollection" Multiplicity="OneMany">
					<RolePlayer>
						<DomainClassMoniker Name="Table"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="EA68512E-C944-44BC-9F54-D58C347DCDD7" Name="Column" PropertyName="Table" Multiplicity="One" PropagatesDelete="true">
					<RolePlayer>
						<DomainClassMoniker Name="Column"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="DAD57818-93B6-4584-A94E-71D31E4F959C" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="TableContainsConstraint" IsEmbedding="true" InheritanceModifier="Abstract">
			<Source>
				<DomainRole Id="E64C339F-8AC5-494D-A913-D22D5034E961" Name="Table" PropertyName="ConstraintCollection" IsPropertyGenerator="false" Multiplicity="OneMany">
					<RolePlayer>
						<DomainClassMoniker Name="Table"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="66919910-6ACA-4A0D-BCFD-25791A641A16" Name="Constraint" PropertyName="Table" IsPropertyGenerator="false" Multiplicity="ZeroOne" PropagatesDelete="true">
					<RolePlayer>
						<DomainClassMoniker Name="Constraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="8F420258-5C3D-4383-A657-13C0085500C5" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="TableContainsUniquenessConstraint" IsEmbedding="true">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="TableContainsConstraint"/>
			</BaseRelationship>
			<Source>
				<DomainRole Id="B9790914-E889-4F00-A456-ED1D1817C23B" Name="Table" PropertyName="UniquenessConstraintCollection" Multiplicity="OneMany">
					<RolePlayer>
						<DomainClassMoniker Name="Table"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="132009F4-76FF-46FB-AC95-A2419575D20C" Name="UniquenessConstraint" PropertyName="Table" Multiplicity="One" PropagatesDelete="true">
					<RolePlayer>
						<DomainClassMoniker Name="UniquenessConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="8E95A446-7557-4E60-B011-79F28F2F0773" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="TableContainsReferenceConstraint" IsEmbedding="true">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="TableContainsConstraint"/>
			</BaseRelationship>
			<Source>
				<DomainRole Id="8D0549B5-9A2F-4FD2-A733-BCC779A89BAF" Name="Table" PropertyName="ReferenceConstraintCollection" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="Table"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="6DAA73B4-E37E-4BE1-BB3C-DAFBCFD51343" Name="ReferenceConstraint" PropertyName="SourceTable" Multiplicity="One">
					<RolePlayer>
						<DomainClassMoniker Name="ReferenceConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="C129490D-491C-426F-B04B-7C11D461F608" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="TableContainsCheckConstraint" IsEmbedding="true">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="TableContainsConstraint"/>
			</BaseRelationship>
			<Source>
				<DomainRole Id="3370D577-580D-4153-A03B-429ACE90C798" Name="Table" PropertyName="CheckConstraintCollection" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="Table"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="C6559E00-462F-4D95-A8BA-C5D18A25601A" Name="CheckConstraint" PropertyName="Table" Multiplicity="ZeroOne" PropagatesDelete="true">
					<RolePlayer>
						<DomainClassMoniker Name="CheckConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Id="25D67B3B-A209-480A-B708-684F1A4B887C" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="DomainContainsCheckConstraint" IsEmbedding="true">
			<Source>
				<DomainRole Id="4B1BC8FC-29CA-4004-9395-2AE5BF2015A1" Name="Domain" PropertyName="CheckConstraint" Multiplicity="One">
					<RolePlayer>
						<DomainClassMoniker Name="Domain"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="7194E4E3-B028-40F7-B99E-175BCAE13B50" Name="CheckConstraint" PropertyName="Domain" Multiplicity="ZeroOne" PropagatesDelete="true">
					<RolePlayer>
						<DomainClassMoniker Name="CheckConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Id="AB00E63E-3F80-443F-A2C1-DF38B3B3CC4F" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="ColumnHasDomain">
			<Source>
				<DomainRole Id="DB3A68AF-C144-41A1-89CC-5410AA813D4F" Name="Column" PropertyName="Domain" Multiplicity="ZeroOne">
					<RolePlayer>
						<DomainClassMoniker Name="Column"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="8049EFC1-7E4F-4965-82E7-D0B376424C57" Name="Domain" IsPropertyGenerator="false" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="Domain"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="7DF7C60C-BB01-4D69-A884-11B62EB0DF4C" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="ColumnHasPredefinedDataType" IsEmbedding="true">
			<Source>
				<DomainRole Id="213FD4BF-DD0C-4281-88B3-B19FF0F930C0" Name="Column" PropertyName="PredefinedDataType" Multiplicity="ZeroOne">
					<RolePlayer>
						<DomainClassMoniker Name="Column"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="E72D55D8-EB10-4EEB-B40E-FCCF3B111BDC" Name="PredefinedDataType" PropertyName="Column" Multiplicity="ZeroOne" PropagatesDelete="true">
					<RolePlayer>
						<DomainClassMoniker Name="PredefinedDataType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="D889DCCC-682C-4588-ABD7-EA0F89F16EC3" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="DomainHasPredefinedDataType" IsEmbedding="true">
			<Source>
				<DomainRole Id="90609E27-B82A-4B38-9CD9-A1852E4AE8BC" Name="Domain" PropertyName="PredefinedDataType" Multiplicity="One">
					<RolePlayer>
						<DomainClassMoniker Name="Domain"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="A8838633-1B28-4538-9B43-11556D0DD343" Name="PredefinedDataType" PropertyName="Domain" Multiplicity="ZeroOne" PropagatesCopy="true">
					<RolePlayer>
						<DomainClassMoniker Name="PredefinedDataType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Id="0A87B269-0D2C-4D7D-80A7-6DC1B5E0C5F6" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="UniquenessConstraintIncludesColumn">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.ElementTypeDescriptionProvider&lt;UniquenessConstraintIncludesColumn, Design.ConceptualDatabaseElementTypeDescriptor&lt;UniquenessConstraintIncludesColumn&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<Source>
				<DomainRole Id="2AC76280-67CA-4583-A203-C0EDA2D15FE4" Name="UniquenessConstraint" PropertyName="ColumnCollection" Multiplicity="OneMany">
					<RolePlayer>
						<DomainClassMoniker Name="UniquenessConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="32200848-28E3-4D35-9A61-F592903B52BF" Name="Column" IsPropertyGenerator="false" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="Column"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Id="27A4368B-C175-4F67-803F-AD902B5E7753" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="ReferenceConstraintTargetsTable">
			<Source>
				<DomainRole Id="22F748A4-8086-46C4-A323-E93574438D16" Name="ReferenceConstraint" PropertyName="TargetTable" Multiplicity="One">
					<RolePlayer>
						<DomainClassMoniker Name="ReferenceConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="3EFFDDB4-4B20-4EB4-8A79-271240574413" Name="TargetTable" IsPropertyGenerator="false" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="Table"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="2CA33A35-0FD3-4B68-9222-F2851A909C2F" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="ColumnReference" AllowsDuplicates="true">
			<Source>
				<DomainRole Id="7BE6B7CC-EE99-4667-B096-79E2B4403561" Name="SourceColumn" PropertyName="TargetColumnCollection" IsPropertyGenerator="false" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="Column"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="D7569B79-22DB-4423-9B31-AD8C5FA96AC2" Name="TargetColumn" PropertyName="SourceColumnCollection" IsPropertyGenerator="false" Multiplicity="ZeroMany">
					<RolePlayer>
						<DomainClassMoniker Name="Column"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Id="59F535BC-8DAF-43A0-8CFC-519086A5A9DE" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="ReferenceConstraintContainsColumnReference">
			<Source>
				<DomainRole Id="FA2DFF9B-4E4B-4BDE-BDDD-090D6B6A6893" Name="ReferenceConstraint" PropertyName="ColumnReferenceCollection" Multiplicity="OneMany">
					<RolePlayer>
						<DomainClassMoniker Name="ReferenceConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Id="4FEBD832-AE5D-446A-A2CE-3F425E0E659E" Name="ColumnReference" PropertyName="ReferenceConstraint" Multiplicity="One" PropagatesDelete="true">
					<RolePlayer>
						<DomainRelationshipMoniker Name="ColumnReference"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
	</Relationships>

	<Types>
		<DomainEnumeration Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="TriggerTarget" Description="The valid values for the target attribute of a trigger.">
			<Literals>
				<EnumerationLiteral Name="OldRow" Value="0">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;OLD ROW&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="NewRow" Value="1">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;NEW ROW&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="OldTable" Value="2">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;OLD TABLE&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="NewTable" Value="3">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;NEW TABLE&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.Serializable"/>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;TriggerTarget, global::Neumont.Tools.RelationalModels.ConceptualDatabase.Catalog&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="TriggerActionTime" Description="The valid values for the actionTime attribute of a trigger.">
			<Literals>
				<EnumerationLiteral Name="Before" Value="0">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;BEFORE&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="After" Value="1">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;AFTER&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.Serializable"/>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;TriggerActionTime, global::Neumont.Tools.RelationalModels.ConceptualDatabase.Catalog&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="TriggerEvent" Description="The valid values for the event attribute of a trigger.">
			<Literals>
				<EnumerationLiteral Name="Insert" Value="0">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;INSERT&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="Delete" Value="1">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;DELETE&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="Update" Value="2">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;UPDATE&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.Serializable"/>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;TriggerEvent, global::Neumont.Tools.RelationalModels.ConceptualDatabase.Catalog&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase" Name="TriggerForEach" Description="The valid values for the event attribute of a trigger.">
			<Literals>
				<EnumerationLiteral Name="Statement" Value="0">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;STATEMENT&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="Row" Value="1">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;ROW&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.Serializable"/>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;TriggerForEach, global::Neumont.Tools.RelationalModels.ConceptualDatabase.Catalog&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Namespace="Neumont.Tools.RelationalModels.DatabaseDefinition" Name="SqlDataAccessIndication" Description="Used to indicate the type of access to SQL-data for a SQL-invoked routine.">
			<Literals>
				<EnumerationLiteral Name="NoSql" Value="0" Description="Indicates that the SQL-invoked routine does not possibly contain SQL.">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;NO SQL&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="ContainsSql" Value="1" Description="Indicates that the SQL-invoked routine possibly contains SQL.">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;CONTAINS SQL&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="ReadsSqlData" Value="2" Description="Indicates that the SQL-invoked routine possibly reads SQL-data.">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;READS SQL DATA&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="ModifiesSqlData" Value="3" Description="Indicates that the SQL-invoked routine possibly modified SQL-data.">
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;MODIFIES SQL DATA&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.Serializable"/>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;SqlDataAccessIndication, global::Neumont.Tools.RelationalModels.ConceptualDatabase.Catalog&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Namespace="Neumont.Tools.RelationalModels.DatabaseDefinition" Name="PredefinedType">
			<Literals>
				<EnumerationLiteral Name="StringMask" Value="1140850688">
					<Notes>CharacterStringMask | BinaryStringMask</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Browsable">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.EditorBrowsable">
							<Parameters>
								<AttributeParameter Value="global::System.ComponentModel.EditorBrowsableState.Advanced"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="CharacterStringMask" Value="67108864">
					<Notes>1 &lt;&lt; 26</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Browsable">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.EditorBrowsable">
							<Parameters>
								<AttributeParameter Value="global::System.ComponentModel.EditorBrowsableState.Advanced"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="BinaryStringMask" Value="1073741824">
					<Notes>1 &lt;&lt; 30</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Browsable">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.EditorBrowsable">
							<Parameters>
								<AttributeParameter Value="global::System.ComponentModel.EditorBrowsableState.Advanced"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="LargeObjectStringMask" Value="536870912">
					<Notes>1 &lt;&lt; 29</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Browsable">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.EditorBrowsable">
							<Parameters>
								<AttributeParameter Value="global::System.ComponentModel.EditorBrowsableState.Advanced"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="NumericMask" Value="402653184">
					<Notes>ExactNumericMask | ApproximateNumericMask</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Browsable">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.EditorBrowsable">
							<Parameters>
								<AttributeParameter Value="global::System.ComponentModel.EditorBrowsableState.Advanced"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="ExactNumericMask" Value="268435456">
					<Notes>1 &lt;&lt; 28</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Browsable">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.EditorBrowsable">
							<Parameters>
								<AttributeParameter Value="global::System.ComponentModel.EditorBrowsableState.Advanced"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="ApproximateNumericMask" Value="134217728">
					<Notes>1 &lt;&lt; 27</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Browsable">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.EditorBrowsable">
							<Parameters>
								<AttributeParameter Value="global::System.ComponentModel.EditorBrowsableState.Advanced"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>

				<EnumerationLiteral Name="Character" Value="67108865">
					<Notes>1 | CharacterStringMask</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;CHARACTER&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="CharacterVarying" Value="67108866">
					<Notes>2 | CharacterStringMask</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;CHARACTER VARYING&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="CharacterLargeObject" Value="603979779">
					<Notes>3 | CharacterStringMask | LargeObjectStringMask</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;CHARACTER LARGE OBJECT&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="BinaryLargeObject" Value="1610612740">
					<Notes>4 | BinaryStringMask | LargeObjectStringMask</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;BINARY LARGE OBJECT&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="Numeric" Value="268435461">
					<Notes>5 | ExactNumericMask</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;NUMERIC&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="Decimal" Value="268435462">
					<Notes>6 | ExactNumericMask</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;DECIMAL&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="SmallInt" Value="268435463">
					<Notes>7 | ExactNumericMask</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;SMALLINT&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="Integer" Value="268435464">
					<Notes>8 | ExactNumericMask</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;INTEGER&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="BigInt" Value="268435465">
					<Notes>9 | ExactNumericMask</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;BIGINT&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="Float" Value="134217738">
					<Notes>10 | ApproximateNumericMask</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;FLOAT&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="Real" Value="134217739">
					<Notes>11 | ApproximateNumericMask</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;REAL&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="DoublePrecision" Value="134217740">
					<Notes>12 | ApproximateNumericMask</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;DOUBLE PRECISION&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="Boolean" Value="13">
					<Notes>13</Notes>
					<Attributes>
						<ClrAttribute Name="global::System.Xml.Serialization.XmlEnum">
							<Parameters>
								<AttributeParameter Value="&quot;BOOLEAN&quot;"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.Serializable"/>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;PredefinedType, global::Neumont.Tools.RelationalModels.ConceptualDatabase.Catalog&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
	</Types>

	<XmlSerializationBehavior Name="DcilDomainModelSerializationBehavior" Namespace="Neumont.Tools.RelationalModels.ConceptualDatabase"/>

</Dsl>