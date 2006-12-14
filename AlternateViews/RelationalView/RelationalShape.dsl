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
	PackageGuid="EFDDC549-1646-4451-8A51-E5A5E94D647C"
	Id="0144A831-92D5-4C42-B7C5-99A5FA9D79DF"
	Namespace="Neumont.Tools.ORM.Views.RelationalView"
	Name="RelationalShape"
	DisplayName="Relational View"
	Description="Relational View of ORM Model"
	CompanyName="Neumont University"
	ProductName="Neumont ORM Architect for Visual Studio"
	MajorVersion="1" MinorVersion="0" Build="0" Revision="0"
	AccessModifier="Assembly">
	<Attributes>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;3EAE649F-E654-4D04-8289-C25D2C0322D8&quot;/*Neumont.Tools.ORM.ObjectModel.ORMCoreDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;CD96AA55-FCBC-47D0-93F8-30D3DACC5FF7&quot;/*Neumont.Tools.ORM.OIALModel.OIALMetaModel*/"/>
			</Parameters>
		</ClrAttribute>
	</Attributes>

	<Classes>
		<DomainClass Name="RelationalNamedElement" Namespace="Neumont.Tools.ORM.Views.RelationalView" Id="FC88D6A8-FFC5-4F1B-B424-A5C46843F3F1" DisplayName="RelationalNamedElement" InheritanceModifier="Abstract" AccessModifier="Assembly" Description="">
			<Properties>
				<DomainProperty Name="Name" DefaultValue="" DisplayName="Name" IsElementName="true" Id="EF72AAC6-D628-49F6-86F0-52D72765FE68" IsBrowsable="false">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="RelationalModel" Namespace="Neumont.Tools.ORM.Views.RelationalView" Id="7FAEDEEC-0A27-4417-B74B-422A67A67F50" DisplayName="OIALModel" AccessModifier="Assembly" Description="">
			<BaseClass>
				<DomainClassMoniker Name="RelationalNamedElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Id="7AD94A1D-9A17-408B-83D4-28C1B8270CD5" Description="Specifies whether data types should be shown or not." DefaultValue="true" Name="DisplayDataTypes" DisplayName="DisplayDataTypes">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
			<!--<ElementMergeDirectives>
				<ElementMergeDirective UsesCustomAccept="false" UsesCustomMerge="false">
					<Index>
						<DomainClassMoniker Name="ConceptType"/>
					</Index>
					<LinkCreationPaths>
						<DomainPath>OIALModelHasConceptType.ConceptTypeCollection</DomainPath>
					</LinkCreationPaths>
				</ElementMergeDirective>
				<ElementMergeDirective UsesCustomAccept="false" UsesCustomMerge="false">
					<Index>
						<DomainClassMoniker Name="InformationTypeFormat"/>
					</Index>
					<LinkCreationPaths>
						<DomainPath>OIALHasInformationTypeFormat.InformationTypeFormatCollection</DomainPath>
					</LinkCreationPaths>
				</ElementMergeDirective>
				<ElementMergeDirective UsesCustomAccept="false" UsesCustomMerge="false">
					<Index>
						<DomainClassMoniker Name="ChildSequenceConstraint"/>
					</Index>
					<LinkCreationPaths>
						<DomainPath>OIALModelHasChildSequenceConstraint.ChildSequenceConstraintCollection</DomainPath>
					</LinkCreationPaths>
				</ElementMergeDirective>
			</ElementMergeDirectives>-->
		</DomainClass>

		<DomainClass Name="Table" Namespace="Neumont.Tools.ORM.Views.RelationalView" Id="9A790BEF-9541-4EAC-A0FC-8BE790958D88" DisplayName="RelationalTable" AccessModifier="Assembly" Description="">
			<BaseClass>
				<DomainClassMoniker Name="RelationalNamedElement"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="Column" Namespace="Neumont.Tools.ORM.Views.RelationalView" Id="71EC2CA3-0D43-4ACC-A302-063DBE8681A4" DisplayName="RelationalColumn" AccessModifier="Assembly" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.Diagnostics.DebuggerDisplay">
					<Parameters>
						<AttributeParameter Value="&quot;Name={Name}, Mandatory = {IsMandatory}, Data Type = {DataType}&quot;" />
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="RelationalNamedElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="IsMandatory" DefaultValue="" DisplayName="IsMandatory" IsElementName="true" Id="A0C66481-CCC1-4FAC-AAA5-31AFC48027C1">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="DataType" DefaultValue="" DisplayName="DataType" IsElementName="true" Id="C47B93EB-3E02-4177-A606-0C1FE611338C">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="Constraint" Namespace="Neumont.Tools.ORM.Views.RelationalView" Id="B425375C-0C0D-46DF-810D-3384D6B4E62B" DisplayName="RelationalConstraint" AccessModifier="Assembly" Description="" InheritanceModifier="Abstract">
			<Attributes>
				<ClrAttribute Name="global::System.Diagnostics.DebuggerDisplay">
					<Parameters>
						<AttributeParameter Value="&quot;Name={Name}&quot;" />
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="RelationalNamedElement"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ForeignKey" Namespace="Neumont.Tools.ORM.Views.RelationalView" Id="16611F65-CEB1-403D-A890-602518898C80" DisplayName="RelationalConstraint" AccessModifier="Assembly" Description="">
			<Attributes>
				<ClrAttribute Name="System.Diagnostics.DebuggerDisplay">
					<Parameters>
						<AttributeParameter Value="&quot;Name={Name}&quot;" />
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="Constraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="UniquenessConstraint" Namespace="Neumont.Tools.ORM.Views.RelationalView" Id="BE998864-7664-410B-8072-E4D7065A17D9" DisplayName="RelationalConstraint" AccessModifier="Assembly" Description="">
			<Attributes>
				<ClrAttribute Name="System.Diagnostics.DebuggerDisplay">
					<Parameters>
						<AttributeParameter Value="&quot;Name={Name}, Primary = {IsPreferred}&quot;" />
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="Constraint"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="IsPreferred" DefaultValue="" DisplayName="IsPreferred" IsElementName="true" Id="5C1EFC1E-21B8-46C8-ADC7-02EB60012187">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
	</Classes>

	<Relationships>
		<DomainRelationship IsEmbedding="true" Name="RelationalModelHasTable" Namespace="Neumont.Tools.ORM.Views.RelationalView" Id="99F4EFF8-1552-46DA-8B6C-8CC7EB9409E3" AccessModifier="Assembly">

			<Source>
				<DomainRole Name="RelationalModel" PropertyName="TableCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="RelationalModel" Id="9B228D99-4CAD-4727-A96C-8CE6B9B9F9BB">
					<RolePlayer>
						<DomainClassMoniker Name="RelationalModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Table" PropertyName="RelationalModel" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Table" Id="C4451D63-DC31-4181-AD15-1A8655736350">
					<RolePlayer>
						<DomainClassMoniker Name="Table"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship IsEmbedding="true" Name="TableHasColumn" Namespace="Neumont.Tools.ORM.Views.RelationalView" Id="4350E15F-9256-45D2-9C53-2165C954B779" AccessModifier="Assembly">

			<Source>
				<DomainRole Name="Table" PropertyName="ColumnCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Table" Id="29D486E0-43CF-4229-94C3-72015C789A23">
					<RolePlayer>
						<DomainClassMoniker Name="Table"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Column" PropertyName="Table" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Column" Id="950F1A01-C0F2-472F-9420-14EB642EA607">
					<RolePlayer>
						<DomainClassMoniker Name="Column"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		
		<DomainRelationship IsEmbedding="true" Name="TableHasConstraint" Namespace="Neumont.Tools.ORM.Views.RelationalView" Id="DE27A531-45D4-40F5-A8EA-389C18F7CC65" AccessModifier="Assembly">

			<Source>
				<DomainRole Name="Table" PropertyName="ConstraintCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Table" Id="6457DC9B-37EE-44A6-9E7B-6F8558F2A35B">
					<RolePlayer>
						<DomainClassMoniker Name="Table"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Constraint" PropertyName="Table" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Constraint" Id="85FF9E28-159B-4D6A-A213-38A7F4AF1CD6">
					<RolePlayer>
						<DomainClassMoniker Name="Constraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConstraintReferencesColumn" AllowsDuplicates="true" Namespace="Neumont.Tools.ORM.Views.RelationalView" Id="8C312418-C27C-47D0-B56E-994972B98444" AccessModifier="Assembly">

			<Source>
				<DomainRole Name="Constraint" PropertyName="ColumnCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Constraint" Id="4970B4EE-E2F2-41D6-9A5F-6D60B5194CCC">
					<RolePlayer>
						<DomainClassMoniker Name="Constraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Column" PropertyName="ConstraintCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Column" Id="7986BD40-2615-4CA7-97EF-4E511C2CB850">
					<RolePlayer>
						<DomainClassMoniker Name="Column"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="TableReferencesTable" AllowsDuplicates="true" Namespace="Neumont.Tools.ORM.Views.RelationalView" Id="9974A38A-AB9A-4F05-B257-C3C753718E6C" AccessModifier="Assembly">

			<Source>
				<DomainRole Name="Table" PropertyName="ReferencedTableCollection" Multiplicity="ZeroMany" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Constraint" Id="F2F61EDB-05B8-4367-9BF2-E44E6ED20FAD">
					<RolePlayer>
						<DomainClassMoniker Name="Table"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ReferencedTable" PropertyName="ReferencingTableCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="Table" Id="3E98D5E9-48AD-476E-AC5D-005FE123C392">
					<RolePlayer>
						<DomainClassMoniker Name="Table"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="TableReferenceHasForeignKey" Namespace="Neumont.Tools.ORM.Views.RelationalView" Id="71DBF367-E57D-4279-9330-9FC7CF24A8F4" AccessModifier="Assembly">

			<Source>
				<DomainRole Name="TableReferencesTable" PropertyName="ForeignKey" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="TableReferencesTable" Id="F430CC5E-AE32-4ED3-965A-3DD67B37E38F">
					<RolePlayer>
						<DomainRelationshipMoniker Name="TableReferencesTable"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ForeignKey" PropertyName="TableReference" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ForeignKey" Id="83163AF7-5DC2-4B38-89DD-2C8221406C84">
					<RolePlayer>
						<DomainClassMoniker Name="ForeignKey"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="TableReferencesConceptType" Namespace="Neumont.Tools.ORM.Views.RelationalView" Id="B286333F-1047-44D1-9459-5735F428913D" AccessModifier="Assembly">

			<Source>
				<DomainRole Name="Table" PropertyName="ConceptType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Table" Id="1697C628-0BA4-4A2E-9A7A-3BEA6A08FAC1" IsPropertyBrowsable="false">
					<RolePlayer>
						<DomainClassMoniker Name="Table"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ConceptType" PropertyName="Table" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ConceptType" Id="A1840588-8A18-4673-A4D9-A35C81E8500D">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORM.OIALModel/ConceptType" />
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="RelationalModelHasOIALModel" Namespace="Neumont.Tools.ORM.Views.RelationalView" Id="1D3A4CB0-B28D-4AE8-BCD5-723EC645EAE9" AccessModifier="Assembly">

			<Source>
				<DomainRole Name="RelationalModel" PropertyName="OIALModel" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="RelationalModel" Id="02FCACDA-1000-49E1-AC8E-5046AC1DE79F" IsPropertyBrowsable="false">
					<RolePlayer>
						<DomainClassMoniker Name="RelationalModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="OIALModel" PropertyName="RelationalModel" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="OIALModel" Id="DA1EB30B-830D-4C65-9C72-73DD1C9603CC">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORM.OIALModel/OIALModel" />
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
	</Relationships>

	<Shapes>
		<CompartmentShape Name="TableShape" Id="50DABFCD-909C-418A-8895-172AADAAD4FB" Namespace="Neumont.Tools.ORM.Views.RelationalView" GeneratesDoubleDerived="true" HasCustomConstructor="true" InitialHeight="0.3" InitialWidth="1" OutlineThickness="0.015625" AccessModifier="Assembly">
			<ShapeHasDecorators Position="InnerTopCenter">
				<TextDecorator FontSize="10" FontStyle="Bold" Name="TableNameDecorator"/>
			</ShapeHasDecorators>
			<Compartment Name="ColumnsCompartment" Title="Columns"/>
		</CompartmentShape>
	</Shapes>

	<Connectors>
		<Connector Id="42F16D0D-8D7D-4452-8146-6A543F683C11" Name="ForeignKeyConnector" Namespace="Neumont.Tools.ORM.Views.RelationalView" DisplayName="ForeignKeyConnector" Thickness="0.0234375" Color="Purple" TargetEndStyle="FilledArrow" FixedTooltipText="ForeignKey Connector" AccessModifier="Assembly"/>
	</Connectors>
	
	<XmlSerializationBehavior Name="RelationalShapeModelSerializationBehavior" Namespace="Neumont.Tools.ORM.Views.RelationalView"/>

	<Diagram Name="RelationalDiagram" Namespace="Neumont.Tools.ORM.Views.RelationalView" Id="9DD5AFCE-2B3C-4854-AE9F-8FF5D5B7BF08" AccessModifier="Assembly" HasCustomConstructor="true">
		<Class>
			<DomainClassMoniker Name="/Neumont.Tools.ORM.Views.RelationalView/RelationalModel"/>
		</Class>
		<ShapeMaps>
			<CompartmentShapeMap>
				<DomainClassMoniker Name="/Neumont.Tools.ORM.Views.RelationalView/Table"/>
				<ParentElementPath>
					<DomainPath>Neumont.Tools.ORM.Views.RelationalView.RelationalModelHasTable.RelationalModel/!RelationalModel</DomainPath>
				</ParentElementPath>
				<DecoratorMap>
					<TextDecoratorMoniker Name="TableShape/TableNameDecorator"/>
					<PropertyDisplayed>
						<PropertyPath>
							<DomainPropertyMoniker Name="/Neumont.Tools.ORM.Views.RelationalView/RelationalNamedElement/Name"/>
						</PropertyPath>
					</PropertyDisplayed>
				</DecoratorMap>
				<CompartmentShapeMoniker Name="TableShape"/>
				<CompartmentMap>
					<CompartmentMoniker Name="TableShape/ColumnsCompartment" />
					<ElementsDisplayed>
						<DomainPath>Neumont.Tools.ORM.Views.RelationalView.TableHasColumn.ColumnCollection/!Column</DomainPath>
					</ElementsDisplayed>
					<PropertyDisplayed>
						<PropertyPath>
							<DomainPropertyMoniker Name="/Neumont.Tools.ORM.Views.RelationalView/RelationalNamedElement/Name" />
						</PropertyPath>
					</PropertyDisplayed>
				</CompartmentMap>
			</CompartmentShapeMap>
		</ShapeMaps>
		<ConnectorMaps>
			<ConnectorMap>
				<ConnectorMoniker Name="ForeignKeyConnector"/>
				<DomainRelationshipMoniker Name="TableReferencesTable"/>
			</ConnectorMap>
		</ConnectorMaps>
	</Diagram>

	<Designer EditorGuid="81DF6430-142D-4BCC-9E09-F873DED5BA0E">
		<RootClass>
			<DomainClassMoniker Name="/Neumont.Tools.ORM.Views.RelationalView/RelationalModel"/>
		</RootClass>
		<XmlSerializationDefinition>
			<XmlSerializationBehaviorMoniker Name="/Neumont.Tools.ORM.Views.RelationalView/RelationalShapeModelSerializationBehavior"/>
		</XmlSerializationDefinition>
		<DiagramMoniker Name="RelationalDiagram"/>
	</Designer>

</Dsl>