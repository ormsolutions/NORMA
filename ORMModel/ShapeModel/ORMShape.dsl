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
	Id="C52FB9A5-6BF4-4267-8716-71D74C7AA89C"
	Namespace="Neumont.Tools.ORM.ShapeModel"
	PackageNamespace="Neumont.Tools.ORM.Shell"
	Name="ORMShape"
	DisplayName="ORM Shape Domain Model"
	CompanyName="Neumont University"
	ProductName="Neumont ORM Architect for Visual Studio"
	MajorVersion="1" MinorVersion="0" Build="0" Revision="0">

	<Attributes>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;F60BC3F1-C38E-4C7D-9EE5-9211DB26CB45&quot;/*Neumont.Tools.Modeling.FrameworkDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;3EAE649F-E654-4D04-8289-C25D2C0322D8&quot;/*Neumont.Tools.ORM.ObjectModel.ORMCoreDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
	</Attributes>

	<Relationships>
		<DomainRelationship Name="FactTypeShapeHasRoleDisplayOrder" Namespace="Neumont.Tools.ORM.ShapeModel" Id="94B3AEEF-4C8D-4D1A-A7CC-42F7EBDC68A2">
			<Source>
				<DomainRole Name="FactTypeShape" PropertyName="RoleDisplayOrderCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactTypeShape" Id="30C6B725-2D74-47F7-852A-D02C644A447B">
					<RolePlayer>
						<GeometryShapeMoniker Name="FactTypeShape"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="RoleDisplayOrder" PropertyName="FactTypeShapeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="RoleDisplayOrder" Id="4CA45C6E-0400-4976-AF8C-0CAD7C7BC2EE">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/RoleBase"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
	</Relationships>

	<Types>
		<DomainEnumeration Namespace="Neumont.Tools.ORM.ShapeModel" Name="ConstraintDisplayPosition" Description="Determines where internal constraints are drawn on FactTypeShapes.">
			<Literals>
				<EnumerationLiteral Name="Top" Value="0" Description="Draw the constraints above the top of the role boxes."/>
				<EnumerationLiteral Name="Bottom" Value="1" Description="Draw the constraints below the bottom of the role boxes."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;ConstraintDisplayPosition, global::Neumont.Tools.ORM.ShapeModel.ORMDiagram&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Namespace="Neumont.Tools.ORM.ShapeModel" Name="DisplayRoleNames" Description="Determines whether RoleNameShapes will be drawn for the Roles in the FactType represented by the FactTypeShape using this enumeration, overriding the global setting.">
			<Literals>
				<EnumerationLiteral Name="UserDefault" Value="0" Description="Use the global setting."/>
				<EnumerationLiteral Name="On" Value="1" Description="Always draw the RoleNameShapes."/>
				<EnumerationLiteral Name="Off" Value="2" Description="Never draw the RoleNameShapes."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;DisplayRoleNames, global::Neumont.Tools.ORM.ShapeModel.ORMDiagram&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Namespace="Neumont.Tools.ORM.ShapeModel" Name="DisplayOrientation" Description="Determines whether a FactTypeShape is drawn horizontally or vertically.">
			<Literals>
				<EnumerationLiteral Name="Horizontal" Value="0" Description="The fact type is drawn with a horizontal orientation."/>
				<EnumerationLiteral Name="VerticalRotatedRight" Value="1" Description="The fact type is drawn with a vertical orientation rotated to the right."/>
				<EnumerationLiteral Name="VerticalRotatedLeft" Value="2" Description="The fact type is drawn with a vertical orientation rotated to the left."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;DisplayOrientation, global::Neumont.Tools.ORM.ShapeModel.ORMDiagram&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
	</Types>

	<Shapes>
		<GeometryShape Name="ORMBaseShape" Namespace="Neumont.Tools.ORM.ShapeModel" InheritanceModifier="Abstract" Id="55131F4B-0F9A-408D-BED0-79451BA7F4F0" FillGradientMode="None">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Diagrams.Design.PresentationElementTypeDescriptionProvider&lt;ORMBaseShape, global::Neumont.Tools.ORM.ObjectModel.ORMModelElement, Design.ORMBaseShapeTypeDescriptor&lt;ORMBaseShape, global::Neumont.Tools.ORM.ObjectModel.ORMModelElement&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<Properties>
				<DomainProperty Name="UpdateCounter" Id="85E23BA2-451A-4CD3-B233-64973E6133F6" GetterAccessModifier="Private" SetterAccessModifier="Private" Kind="CustomStorage" IsBrowsable="false">
					<Type>
						<ExternalTypeMoniker Name="/System/Int64"/>
					</Type>
				</DomainProperty>
			</Properties>
		</GeometryShape>
		<GeometryShape Name="ObjectTypeShape" Namespace="Neumont.Tools.ORM.ShapeModel" Id="00C1F246-D8F1-4EEA-AC88-39BA238143A8" FillGradientMode="None" InitialWidth=".7" InitialHeight=".35">
			<BaseGeometryShape>
				<GeometryShapeMoniker Name="ORMBaseShape"/>
			</BaseGeometryShape>
			<Properties>
				<DomainProperty Name="ExpandRefMode" DisplayName="ExpandRefMode" Id="B2415BB1-1C83-4F0B-B2C3-58B67BC620DD" DefaultValue="false">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
		</GeometryShape>
		<GeometryShape Name="FactTypeShape" Namespace="Neumont.Tools.ORM.ShapeModel" Id="8E440A3B-275E-42F7-868B-D5D473158ACD" FillGradientMode="None" InitialWidth=".7" InitialHeight=".35">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Diagrams.Design.PresentationElementTypeDescriptionProvider&lt;FactTypeShape, global::Neumont.Tools.ORM.ObjectModel.FactType, Design.FactTypeShapeTypeDescriptor&lt;FactTypeShape, global::Neumont.Tools.ORM.ObjectModel.FactType&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseGeometryShape>
				<GeometryShapeMoniker Name="ORMBaseShape"/>
			</BaseGeometryShape>
			<Properties>
				<DomainProperty Name="ConstraintDisplayPosition" DisplayName="ConstraintDisplayPosition" Id="802767FD-DE7D-4541-B42B-90B613DFE22D" DefaultValue="Top" Description="Determines where internal constraints are drawn on this FactTypeShape.">
					<Type>
						<DomainEnumerationMoniker Name="ConstraintDisplayPosition"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="DisplayRoleNames" DisplayName="DisplayRoleNames" Id="9607AF0F-2E12-4215-B8A5-91B67C1A9F08" DefaultValue="UserDefault" Description="Determines whether RoleNameShapes will be drawn for the Roles in the FactType represented by this FactTypeShape, overriding the global setting." >
					<Type>
						<DomainEnumerationMoniker Name="DisplayRoleNames"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="DisplayOrientation" DisplayName="DisplayOrientation" Id="31A8F13E-97C6-421F-820D-001CD8E774F3" DefaultValue="Horizontal" Description="Determines if the fact type is shown horizontally or vertically.">
					<Type>
						<DomainEnumerationMoniker Name="DisplayOrientation"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="RolesPosition" Id="89244439-FBB1-4DEB-BFF3-69D47CB90A6B" DefaultValue="0" IsBrowsable="false" GetterAccessModifier="Private" SetterAccessModifier="Private">
					<Type>
						<ExternalTypeMoniker Name="/System/Double"/>
					</Type>
				</DomainProperty>
			</Properties>
		</GeometryShape>
		<GeometryShape Name="SubtypeLink" Namespace="Neumont.Tools.ORM.ShapeModel" Id="87DDAEDA-1FD8-4433-BB1E-7482C7F471A7" FillGradientMode="None">
			<!--<BaseGeometryShape>
				<ConnectorMoniker Name="ORMBaseBinaryLinkShape"/>
			</BaseGeometryShape>-->
		</GeometryShape>
		<GeometryShape Name="ExternalConstraintShape" Namespace="Neumont.Tools.ORM.ShapeModel" Id="00A08F56-73BA-4C8F-8FA1-AE61B8FC1CAE" FillGradientMode="None" InitialWidth=".16" InitialHeight=".16">
			<BaseGeometryShape>
				<GeometryShapeMoniker Name="ORMBaseShape"/>
			</BaseGeometryShape>
		</GeometryShape>
		<GeometryShape Name="FrequencyConstraintShape" Namespace="Neumont.Tools.ORM.ShapeModel" Id="EC47CD7D-023B-4971-8B5B-1242DBC7356F" FillGradientMode="None" InitialWidth=".16" InitialHeight=".16">
			<BaseGeometryShape>
				<GeometryShapeMoniker Name="ExternalConstraintShape"/>
			</BaseGeometryShape>
		</GeometryShape>
		<GeometryShape Name="RingConstraintShape" Namespace="Neumont.Tools.ORM.ShapeModel" Id="61B334C5-D37F-4A74-90E9-AC793D35BDF1" FillGradientMode="None" InitialWidth=".16" InitialHeight=".16">
			<BaseGeometryShape>
				<GeometryShapeMoniker Name="ExternalConstraintShape"/>
			</BaseGeometryShape>
		</GeometryShape>
		<GeometryShape Name="FloatingTextShape" Namespace="Neumont.Tools.ORM.ShapeModel" InheritanceModifier="Abstract" Id="0904999F-D9C5-4C4E-A08F-F8DD4B2F29A3" FillGradientMode="None">
			<BaseGeometryShape>
				<GeometryShapeMoniker Name="ORMBaseShape"/>
			</BaseGeometryShape>
		</GeometryShape>
		<GeometryShape Name="ObjectifiedFactTypeNameShape" Namespace="Neumont.Tools.ORM.ShapeModel" Id="7FD5183A-8BC2-43BB-8474-A0A2D558D90A" FillGradientMode="None">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Diagrams.Design.PresentationElementTypeDescriptionProvider&lt;ObjectifiedFactTypeNameShape, global::Neumont.Tools.ORM.ObjectModel.ObjectType, Design.ObjectifiedFactTypeNameShapeTypeDescriptor&lt;ObjectifiedFactTypeNameShape, global::Neumont.Tools.ORM.ObjectModel.ObjectType&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseGeometryShape>
				<GeometryShapeMoniker Name="FloatingTextShape"/>
			</BaseGeometryShape>
			<Properties>
				<DomainProperty Name="ExpandRefMode" DisplayName="ExpandRefMode" Id="5BDAFE8C-AFA7-4B78-ADC6-CAE876AB2140" DefaultValue="false">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
		</GeometryShape>
		<GeometryShape Name="ReadingShape" Namespace="Neumont.Tools.ORM.ShapeModel" Id="C567ED6D-D0A6-4FD8-A974-C567AA309D5E" FillGradientMode="None">
			<BaseGeometryShape>
				<GeometryShapeMoniker Name="FloatingTextShape"/>
			</BaseGeometryShape>
		</GeometryShape>
		<GeometryShape Name="ValueConstraintShape" Namespace="Neumont.Tools.ORM.ShapeModel" Id="B65F916A-06A5-4EFE-BBF9-8D8E55B5C7EB" FillGradientMode="None">
			<BaseGeometryShape>
				<GeometryShapeMoniker Name="FloatingTextShape"/>
			</BaseGeometryShape>
		</GeometryShape>
		<GeometryShape Name="RoleNameShape" Namespace="Neumont.Tools.ORM.ShapeModel" Id="2CB7393C-4502-4C59-979D-94D6C89B4080" FillGradientMode="None">
			<BaseGeometryShape>
				<GeometryShapeMoniker Name="FloatingTextShape"/>
			</BaseGeometryShape>
		</GeometryShape>
		<GeometryShape Name="ModelNoteShape" Namespace="Neumont.Tools.ORM.ShapeModel" Id="8252E1D1-3C59-4595-8C32-9FB79E84924E" FillGradientMode="None" InitialWidth=".312" InitialHeight=".132">
			<BaseGeometryShape>
				<GeometryShapeMoniker Name="FloatingTextShape"/>
			</BaseGeometryShape>
		</GeometryShape>
		<GeometryShape Name="LinkConnectorShape" Namespace="Neumont.Tools.ORM.ShapeModel" Id="51770447-28E0-4BFF-977F-2D2625F7978D" FillGradientMode="None" InitialHeight="0" InitialWidth="0" Description="Zero-size relative shape used as a proxy connection point for other shapes and links."/>
		<GeometryShape Name="FactTypeLinkConnectorShape" Namespace="Neumont.Tools.ORM.ShapeModel" Id="6A50CBAF-5EA0-4963-9FE6-A288B180A5B8" FillGradientMode="None" InitialWidth="0" InitialHeight="0" Description="Zero-size relative shape used to disambiguate multiple duplicate links between FactTypeShape and other shape types.">
			<BaseGeometryShape>
				<GeometryShapeMoniker Name="LinkConnectorShape"/>
			</BaseGeometryShape>
		</GeometryShape>
	</Shapes>

	<Connectors>
		<Connector Name="ORMBaseBinaryLinkShape" Namespace="Neumont.Tools.ORM.ShapeModel" InheritanceModifier="Abstract" Id="CEFF4339-48D0-4FFE-B052-2F9DA167B1DB">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Diagrams.Design.PresentationElementTypeDescriptionProvider&lt;ORMBaseBinaryLinkShape, DslModeling::ModelElement, Design.ORMBaseBinaryLinkShapeTypeDescriptor&lt;ORMBaseBinaryLinkShape, DslModeling::ModelElement&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<Properties>
				<DomainProperty Name="UpdateCounter" Id="BFD06581-3606-4A9F-9DD1-8BA3014BD5BC" GetterAccessModifier="Private" SetterAccessModifier="Private" Kind="CustomStorage" IsBrowsable="false">
					<Type>
						<ExternalTypeMoniker Name="/System/Int64"/>
					</Type>
				</DomainProperty>
			</Properties>
		</Connector>
		<Connector Name="RolePlayerLink" Namespace="Neumont.Tools.ORM.ShapeModel" Id="2B3F0AAE-B1B1-4727-8862-5C34B494B499">
			<BaseConnector>
				<ConnectorMoniker Name="ORMBaseBinaryLinkShape"/>
			</BaseConnector>
		</Connector>
		<Connector Name="ExternalConstraintLink" Namespace="Neumont.Tools.ORM.ShapeModel" Id="8815E6D8-238B-422C-A4B3-29FDC8DE9EA5">
			<BaseConnector>
				<ConnectorMoniker Name="ORMBaseBinaryLinkShape"/>
			</BaseConnector>
		</Connector>
		<Connector Name="ValueRangeLink" Namespace="Neumont.Tools.ORM.ShapeModel" Id="374E43C3-C294-49C4-8A61-3C3CA5FC86E8">
			<BaseConnector>
				<ConnectorMoniker Name="ORMBaseBinaryLinkShape"/>
			</BaseConnector>
		</Connector>
		<Connector Name="ModelNoteLink" Namespace="Neumont.Tools.ORM.ShapeModel" Id="21E7C585-BC80-446F-8517-BC4FD465971F" DisplayName="ModelNoteReference">
			<BaseConnector>
				<ConnectorMoniker Name="ORMBaseBinaryLinkShape"/>
			</BaseConnector>
		</Connector>
	</Connectors>

	<XmlSerializationBehavior Name="ORMShapeDomainModelSerializationBehavior" Namespace="Neumont.Tools.ORM.ShapeModel"/>

	<!-- Diagram is double-derived so that we can override ShouldAddShapeForElement and OnChildConfiguring. -->
	<!-- Diagram has custom constructor so that we can turn off snap-to-grid and set the initial name. -->
	<Diagram Name="ORMDiagram" DisplayName="ORMDiagram" Namespace="Neumont.Tools.ORM.ShapeModel" Id="948F992D-C9B8-46F9-BE3C-B48347F8AB0B" GeneratesDoubleDerived="true" HasCustomConstructor="true">
		<Attributes>
			<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
				<Parameters>
					<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Diagrams.Design.PresentationElementTypeDescriptionProvider&lt;ORMDiagram, global::Neumont.Tools.ORM.ObjectModel.ORMModel, global::Neumont.Tools.Modeling.Diagrams.Design.DiagramTypeDescriptor&lt;ORMDiagram, global::Neumont.Tools.ORM.ObjectModel.ORMModel&gt;&gt;)"/>
				</Parameters>
			</ClrAttribute>
		</Attributes>
		<Properties>
			<DomainProperty Name="AutoPopulateShapes" Id="D3F7A171-CE39-4944-BE80-D55127423C83" DefaultValue="false" IsBrowsable="false">
				<Type>
					<ExternalTypeMoniker Name="/System/Boolean"/>
				</Type>
			</DomainProperty>
		</Properties>
		<Class>
			<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ORMModel"/>
		</Class>
		<ShapeMaps>
			<ShapeMap>
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ObjectType"/>
				<ParentElementPath>
					<DomainPath>Neumont.Tools.ORM.ObjectModel.ModelHasObjectType.Model/!Model</DomainPath>
				</ParentElementPath>
				<GeometryShapeMoniker Name="ObjectTypeShape"/>
			</ShapeMap>
			<ShapeMap>
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/FactType"/>
				<ParentElementPath>
					<DomainPath>Neumont.Tools.ORM.ObjectModel.ModelHasFactType.Model/!Model</DomainPath>
				</ParentElementPath>
				<GeometryShapeMoniker Name="FactTypeShape"/>
			</ShapeMap>
			<ShapeMap>
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/SubtypeFact"/>
				<ParentElementPath>
					<DomainPath>Neumont.Tools.ORM.ObjectModel.ModelHasFactType.Model/!Model</DomainPath>
				</ParentElementPath>
				<GeometryShapeMoniker Name="SubtypeLink"/>
			</ShapeMap>
			<ShapeMap>
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/SetComparisonConstraint"/>
				<ParentElementPath>
					<DomainPath>Neumont.Tools.ORM.ObjectModel.ModelHasSetComparisonConstraint.Model/!Model</DomainPath>
				</ParentElementPath>
				<GeometryShapeMoniker Name="ExternalConstraintShape"/>
			</ShapeMap>
			<ShapeMap>
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/SetConstraint"/>
				<ParentElementPath>
					<DomainPath>Neumont.Tools.ORM.ObjectModel.ModelHasSetConstraint.Model/!Model</DomainPath>
				</ParentElementPath>
				<GeometryShapeMoniker Name="ExternalConstraintShape"/>
			</ShapeMap>
			<ShapeMap>
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/FrequencyConstraint"/>
				<ParentElementPath>
					<DomainPath>Neumont.Tools.ORM.ObjectModel.ModelHasSetConstraint.Model/!Model</DomainPath>
				</ParentElementPath>
				<GeometryShapeMoniker Name="FrequencyConstraintShape"/>
			</ShapeMap>
			<ShapeMap>
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/RingConstraint"/>
				<ParentElementPath>
					<DomainPath>Neumont.Tools.ORM.ObjectModel.ModelHasSetConstraint.Model/!Model</DomainPath>
				</ParentElementPath>
				<GeometryShapeMoniker Name="RingConstraintShape"/>
			</ShapeMap>
			<ShapeMap>
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ObjectType"/>
				<ParentElementPath>
					<DomainPath>Neumont.Tools.ORM.ObjectModel.ModelHasObjectType.Model/!Model</DomainPath>
				</ParentElementPath>
				<GeometryShapeMoniker Name="ObjectifiedFactTypeNameShape"/>
			</ShapeMap>
			<ShapeMap>
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/RoleValueConstraint"/>
				<ParentElementPath>
					<DomainPath>Neumont.Tools.ORM.ObjectModel.RoleHasValueConstraint.Role/!Role/Neumont.Tools.ORM.ObjectModel.FactTypeHasRole.FactType/!FactType/Neumont.Tools.ORM.ObjectModel.ModelHasFactType.Model/!Model</DomainPath>
				</ParentElementPath>
				<GeometryShapeMoniker Name="ValueConstraintShape"/>
			</ShapeMap>
			<ShapeMap>
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ValueTypeValueConstraint"/>
				<ParentElementPath>
					<DomainPath>Neumont.Tools.ORM.ObjectModel.ValueTypeHasValueConstraint.ValueType/!ValueType/Neumont.Tools.ORM.ObjectModel.ModelHasObjectType.Model/!Model</DomainPath>
				</ParentElementPath>
				<GeometryShapeMoniker Name="ValueConstraintShape"/>
			</ShapeMap>
			<ShapeMap>
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ReadingOrder"/>
				<ParentElementPath>
					<DomainPath>Neumont.Tools.ORM.ObjectModel.FactTypeHasReadingOrder.FactType/!FactType/Neumont.Tools.ORM.ObjectModel.ModelHasFactType.Model/!Model</DomainPath>
				</ParentElementPath>
				<GeometryShapeMoniker Name="ReadingShape"/>
			</ShapeMap>
			<ShapeMap>
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/Role"/>
				<ParentElementPath>
					<DomainPath>Neumont.Tools.ORM.ObjectModel.FactTypeHasRole.FactType/!FactType/Neumont.Tools.ORM.ObjectModel.ModelHasFactType.Model/!Model</DomainPath>
				</ParentElementPath>
				<GeometryShapeMoniker Name="RoleNameShape"/>
			</ShapeMap>
			<ShapeMap>
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ModelNote"/>
				<ParentElementPath>
					<DomainPath>Neumont.Tools.ORM.ObjectModel.ModelHasModelNote.Model/!Model</DomainPath>
				</ParentElementPath>
				<GeometryShapeMoniker Name="ModelNoteShape"/>
			</ShapeMap>
		</ShapeMaps>
		<ConnectorMaps>
			<ConnectorMap>
				<ConnectorMoniker Name="RolePlayerLink"/>
				<DomainRelationshipMoniker Name="/Neumont.Tools.ORM.ObjectModel/ObjectTypePlaysRole"/>
			</ConnectorMap>
			<ConnectorMap>
				<ConnectorMoniker Name="ExternalConstraintLink"/>
				<DomainRelationshipMoniker Name="/Neumont.Tools.ORM.ObjectModel/FactConstraint"/>
			</ConnectorMap>
			<ConnectorMap>
				<ConnectorMoniker Name="ValueRangeLink"/>
				<DomainRelationshipMoniker Name="/Neumont.Tools.ORM.ObjectModel/RoleHasValueConstraint"/>
			</ConnectorMap>
			<ConnectorMap>
				<ConnectorMoniker Name="ModelNoteLink"/>
				<DomainRelationshipMoniker Name="/Neumont.Tools.ORM.ObjectModel/ModelNoteReferencesFactType"/>
			</ConnectorMap>
			<ConnectorMap>
				<ConnectorMoniker Name="ModelNoteLink"/>
				<DomainRelationshipMoniker Name="/Neumont.Tools.ORM.ObjectModel/ModelNoteReferencesObjectType"/>
			</ConnectorMap>
			<ConnectorMap>
				<ConnectorMoniker Name="ModelNoteLink"/>
				<DomainRelationshipMoniker Name="/Neumont.Tools.ORM.ObjectModel/ModelNoteReferencesSetConstraint"/>
			</ConnectorMap>
			<ConnectorMap>
				<ConnectorMoniker Name="ModelNoteLink"/>
				<DomainRelationshipMoniker Name="/Neumont.Tools.ORM.ObjectModel/ModelNoteReferencesSetComparisonConstraint"/>
			</ConnectorMap>
		</ConnectorMaps>
	</Diagram>

	<Designer EditorGuid="EDA9E282-8FC6-4AE4-AF2C-C224FD3AE49B">
		<RootClass>
			<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ORMModel"/>
		</RootClass>
		<XmlSerializationDefinition>
			<XmlSerializationBehaviorMoniker Name="/Neumont.Tools.ORM.ObjectModel/ORMCoreDomainModelSerializationBehavior"/>
		</XmlSerializationDefinition>
		<ToolboxTab TabText="ORM Designer">
			<ElementTool Name="EntityType" ToolboxIcon="../Resources/Toolbox.EntityType.Bitmap.Id.bmp" Caption="Entity Type" Tooltip="New Entity Type">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ObjectType"/>
			</ElementTool>
			<ElementTool Name="ValueType" ToolboxIcon="../Resources/Toolbox.ValueType.Bitmap.Id.bmp" Caption="Value Type" Tooltip="New Value Type">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ObjectType"/>
			</ElementTool>
			<ElementTool Name="ObjectifiedFactType" ToolboxIcon="../Resources/Toolbox.ObjectifiedFactType.Bitmap.Id.bmp" Caption="Objectified Fact Type" Tooltip="New Objectified Fact Type">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ObjectType"/>
			</ElementTool>
			<ElementTool Name="UnaryFactType" ToolboxIcon="../Resources/Toolbox.UnaryFactType.Bitmap.Id.bmp" Caption="Unary Fact Type" Tooltip="New Unary Fact Type">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/FactType"/>
			</ElementTool>
			<ElementTool Name="BinaryFactType" ToolboxIcon="../Resources/Toolbox.BinaryFactType.Bitmap.Id.bmp" Caption="Binary Fact Type" Tooltip="New Binary Fact Type">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/FactType"/>
			</ElementTool>
			<ElementTool Name="TernaryFactType" ToolboxIcon="../Resources/Toolbox.TernaryFactType.Bitmap.Id.bmp" Caption="Ternary Fact Type" Tooltip="New Ternary Fact Type">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/FactType"/>
			</ElementTool>
			<ConnectionTool Name="RoleConnector" ToolboxIcon="../Resources/Toolbox.RoleConnector.Bitmap.Id.bmp" Caption="Role Connector" Tooltip="Role Connector Tool"/>
			<ConnectionTool Name="SubtypeConnector" ToolboxIcon="../Resources/Toolbox.SubtypeConnector.Bitmap.Id.bmp" Caption="Subtype Connector" Tooltip="Subtype Connector Tool"/>
			<ElementTool Name="InternalUniquenessConstraint" ToolboxIcon="../Resources/Toolbox.InternalUniquenessConstraint.Bitmap.Id.bmp" Caption="Internal Uniqueness Constraint" Tooltip="New Internal Uniqueness Constraint">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/UniquenessConstraint"/>
			</ElementTool>
			<ElementTool Name="ExternalUniquenessConstraint" ToolboxIcon="../Resources/Toolbox.ExternalUniquenessConstraint.Bitmap.Id.bmp" Caption="External Uniqueness Constraint" Tooltip="New External Uniqueness Constraint">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/UniquenessConstraint"/>
			</ElementTool>
			<ElementTool Name="EqualityConstraint" ToolboxIcon="../Resources/Toolbox.EqualityConstraint.Bitmap.Id.bmp" Caption="Equality Constraint" Tooltip="New Equality Constraint">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/EqualityConstraint"/>
			</ElementTool>
			<ElementTool Name="ExclusionConstraint" ToolboxIcon="../Resources/Toolbox.ExclusionConstraint.Bitmap.Id.bmp" Caption="Exclusion Constraint" Tooltip="New Exclusion Constraint">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ExclusionConstraint"/>
			</ElementTool>
			<ElementTool Name="InclusiveOrConstraint" ToolboxIcon="../Resources/Toolbox.InclusiveOrConstraint.Bitmap.Id.bmp" Caption="Inclusive Or Constraint" Tooltip="New Inclusive Or Constraint">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/MandatoryConstraint"/>
			</ElementTool>
			<ElementTool Name="ExclusiveOrConstraint" ToolboxIcon="../Resources/Toolbox.ExclusiveOrConstraint.Bitmap.Id.bmp" Caption="Exclusive Or Constraint" Tooltip="New Exclusive Or Constraint">
				<DomainRelationshipMoniker Name="/Neumont.Tools.ORM.ObjectModel/ExclusiveOrConstraintCoupler"/>
			</ElementTool>
			<ElementTool Name="SubsetConstraint" ToolboxIcon="../Resources/Toolbox.SubsetConstraint.Bitmap.Id.bmp" Caption="Subset Constraint" Tooltip="New Subset Constraint">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/SubsetConstraint"/>
			</ElementTool>
			<ElementTool Name="FrequencyConstraint" ToolboxIcon="../Resources/Toolbox.FrequencyConstraint.Bitmap.Id.bmp" Caption="Frequency Constraint" Tooltip="New Frequency Constraint">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/FrequencyConstraint"/>
			</ElementTool>
			<ElementTool Name="RingConstraint" ToolboxIcon="../Resources/Toolbox.RingConstraint.Bitmap.Id.bmp" Caption="Ring Constraint" Tooltip="New Ring Constraint">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/RingConstraint"/>
			</ElementTool>
			<ConnectionTool Name="ExternalConstraintConnector" ToolboxIcon="../Resources/Toolbox.ExternalConstraintConnector.Bitmap.Id.bmp" Caption="Constraint Connector" Tooltip="Constraint Connector Tool"/>
			<ElementTool Name="ModelNote" ToolboxIcon="../Resources/Toolbox.ModelNote.Bitmap.Id.bmp" Caption="Model Note" Tooltip="New Model Note">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ModelNote"/>
			</ElementTool>
			<ConnectionTool Name="ModelNoteConnector" ToolboxIcon="../Resources/Toolbox.ModelNoteConnector.Bitmap.Id.bmp" Caption="Model Note Connector" Tooltip="Model Note Connector Tool"/>
		</ToolboxTab>
		<DiagramMoniker Name="/Neumont.Tools.ORM.ShapeModel/ORMDiagram"/>
	</Designer>

</Dsl>