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
	Id="0144A831-92D5-4C42-B7C5-99A5FA9D79DF"
	Namespace="ORMSolutions.ORMArchitect.Views.RelationalView"
	Name="RelationalShape"
	DisplayName="Relational View"
	Description="Graphical View of Relational Model (slow and temporary)"
	CompanyName="ORM Solutions, LLC"
	ProductName="Natural Object-Role Modeling Architect for Visual Studio"
	MajorVersion="1" MinorVersion="0" Build="0" Revision="0"
	AccessModifier="Assembly">
	<Attributes>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;005CBD56-3BA5-4947-9F46-5608BD563CED&quot;/*ORMSolutions.ORMArchitect.ORMAbstractionToConceptualDatabaseBridge.ORMAbstractionToConceptualDatabaseBridgeDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
	</Attributes>

	<Shapes>
		<CompartmentShape Name="TableShape" Id="50DABFCD-909C-418A-8895-172AADAAD4FB" Namespace="ORMSolutions.ORMArchitect.Views.RelationalView" GeneratesDoubleDerived="true" InitialHeight="0.3" InitialWidth="1" OutlineThickness="0.015625" AccessModifier="Assembly">
			<Properties>
				<DomainProperty Name="UpdateCounter" Id="9D49FFBB-3BB3-4711-B88E-9DBBD984C63D" GetterAccessModifier="Private" SetterAccessModifier="Private" Kind="CustomStorage" IsBrowsable="false">
					<Type>
						<ExternalTypeMoniker Name="/System/Int64"/>
					</Type>
				</DomainProperty>
			</Properties>
			<ShapeHasDecorators Position="InnerTopCenter">
				<TextDecorator FontSize="10" FontStyle="Bold" Name="TableNameDecorator"/>
			</ShapeHasDecorators>
			<Compartment Name="ColumnsCompartment" Title="Columns"/>
		</CompartmentShape>
	</Shapes>

	<Connectors>
		<Connector Id="42F16D0D-8D7D-4452-8146-6A543F683C11" Name="ForeignKeyConnector" Namespace="ORMSolutions.ORMArchitect.Views.RelationalView" DisplayName="ForeignKeyConnector" Thickness="0.0234375" Color="Purple" TargetEndStyle="FilledArrow" AccessModifier="Assembly"/>
	</Connectors>
	
	<XmlSerializationBehavior Name="RelationalShapeModelSerializationBehavior" Namespace="ORMSolutions.ORMArchitect.Views.RelationalView"/>

	<!-- Diagram is double-derived so that we can override CreateChildShape. -->
	<Diagram Name="RelationalDiagram" Namespace="ORMSolutions.ORMArchitect.Views.RelationalView" Id="9DD5AFCE-2B3C-4854-AE9F-8FF5D5B7BF08" AccessModifier="Assembly" GeneratesDoubleDerived="true" HasCustomConstructor="true">
		<Attributes>
			<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
				<Parameters>
					<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptionProvider&lt;RelationalDiagram, global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptor&lt;RelationalDiagram&gt;&gt;)"/>
				</Parameters>
			</ClrAttribute>
		</Attributes>
		<Properties>
			<DomainProperty Id="7AD94A1D-9A17-408B-83D4-28C1B8270CD5" Description="Specifies whether data types should be shown or not." DefaultValue="true" Name="DisplayDataTypes" DisplayName="DisplayDataTypes">
				<Type>
					<ExternalTypeMoniker Name="/System/Boolean"/>
				</Type>
			</DomainProperty>
		</Properties>
		<Class>
			<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase/Catalog"/>
		</Class>
		<ShapeMaps>
			<CompartmentShapeMap>
				<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase/Table"/>
				<ParentElementPath>
					<DomainPath>ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.SchemaContainsTable.Schema/!Schema/ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.CatalogContainsSchema.Catalog/!Catalog</DomainPath>
				</ParentElementPath>
				<DecoratorMap>
					<TextDecoratorMoniker Name="TableShape/TableNameDecorator"/>
					<PropertyDisplayed>
						<PropertyPath>
							<DomainPropertyMoniker Name="/ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase/Table/Name"/>
						</PropertyPath>
					</PropertyDisplayed>
				</DecoratorMap>
				<CompartmentShapeMoniker Name="TableShape"/>
				<CompartmentMap>
					<CompartmentMoniker Name="TableShape/ColumnsCompartment" />
					<ElementsDisplayed>
						<DomainPath>ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase.TableContainsColumn.ColumnCollection/!Column</DomainPath>
					</ElementsDisplayed>
					<PropertyDisplayed>
						<PropertyPath>
							<DomainPropertyMoniker Name="/ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase/Column/Name" />
						</PropertyPath>
					</PropertyDisplayed>
				</CompartmentMap>
			</CompartmentShapeMap>
		</ShapeMaps>
		<ConnectorMaps>
			<!-- This one needs custom code, there is an extra step (TableContainsReferenceConstraint) required to
			find the source shape. -->
			<!-- <ConnectorMap>
				<ConnectorMoniker Name="ForeignKeyConnector"/>
				<DomainRelationshipMoniker Name="/ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase/ReferenceConstraintTargetsTable"/>
			</ConnectorMap> -->
		</ConnectorMaps>
	</Diagram>

	<Designer EditorGuid="81DF6430-142D-4BCC-9E09-F873DED5BA0E">
		<RootClass>
			<DomainClassMoniker Name="/ORMSolutions.ORMArchitect.RelationalModels.ConceptualDatabase/Catalog"/>
		</RootClass>
		<XmlSerializationDefinition>
			<XmlSerializationBehaviorMoniker Name="/ORMSolutions.ORMArchitect.Views.RelationalView/RelationalShapeModelSerializationBehavior"/>
		</XmlSerializationDefinition>
		<DiagramMoniker Name="RelationalDiagram"/>
	</Designer>

</Dsl>