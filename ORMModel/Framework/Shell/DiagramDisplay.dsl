<?xml version="1.0" encoding="utf-8"?>
<!--
	Natural Object-Role Modeling Architect for Visual Studio

	Copyright © ORM Solutions, LLC. All rights reserved.

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
	Id="B5149D2B-5B64-49D1-ACA0-F8DDE50E2C24"
	Namespace="ORMSolutions.ORMArchitect.Framework.Shell"
	Name="DiagramDisplay"
	DisplayName="Diagram Management"
	Description="Reorder diagrams and cache diagram positions"
	CompanyName="ORM Solutions, LLC"
	ProductName="Natural ORM Architect for Visual Studio"
	MajorVersion="1" MinorVersion="0" Build="0" Revision="0">

	<Attributes>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;91D59B16-E488-4A28-8D51-59273AD5BF2E&quot;/*Microsoft.VisualStudio.Modeling.Diagrams.CoreDesignSurfaceDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
	</Attributes>

	<Classes>
		<DomainClass Name="DiagramDisplay" Namespace="ORMSolutions.ORMArchitect.Framework.Shell" Id="14A6B724-7849-4D7D-A5C2-29910FFBB516" DisplayName="DiagramDisplay" InheritanceModifier="Sealed" Description="">
			<Properties>
				<DomainProperty Name="SaveDiagramPosition" DisplayName="SaveDiagramPositions" IsBrowsable="true" DefaultValue="true" Id="17AA2B64-3328-4420-8C68-34157D10DB77" Description="Save the most recent position and zoom information for each diagram in addition to diagram order.">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
		<DomainClass Name="DiagramPlaceHolder" Namespace="ORMSolutions.ORMArchitect.Framework.Shell" Id="8936A358-233B-4322-A20D-92F0D61F8378" DisplayName="DiagramPlaceHolder" InheritanceModifier="Sealed" Description="Diagram placeholder type used during deserialization to load forward references to diagrams without creating a diagram instance of a random type. Creating random types will cause the primary collection to be reordered, resulting in a significant file change for a diagram reorder.">
			<BaseClass>
				<DomainClassMoniker Name="/Microsoft.VisualStudio.Modeling.Diagrams/Diagram"/>
			</BaseClass>
		</DomainClass>
	</Classes>

	<Relationships>
		<DomainRelationship Name="DiagramDisplayHasDiagramOrder" Namespace="ORMSolutions.ORMArchitect.Framework.Shell" Id="6CCD00CE-62D2-420D-805F-791019A2C127">
			<Properties>
				<DomainProperty Name="CenterPoint" DisplayName="CenterPoint" IsBrowsable="false" Id="DF1FFEE4-AC41-4210-9A9A-5B4708B725B6" Description="" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/Microsoft.VisualStudio.Modeling.Diagrams/PointD"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="ZoomFactor" DisplayName="ZoomFactor" IsBrowsable="false" Id="9182EB46-20F6-4445-8FDA-7052BAE54F45" Description="" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/Single"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="IsActiveDiagram" DisplayName="IsActive" IsBrowsable="false" DefaultValue="false" Id="0327999B-60E2-4119-8FAC-A34708851D0A" Description="" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
			<Source>
				<DomainRole Name="DiagramDisplay" PropertyName="OrderedDiagramCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="DiagramDisplay" Id="8B27865A-F62A-4583-998E-94259315B5BE">
					<RolePlayer>
						<DomainClassMoniker Name="DiagramDisplay"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Diagram" PropertyName="DiagramDisplay" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="Diagram" Id="A22A0CFA-632C-4AAA-8A88-E813DAE7CBB2">
					<RolePlayer>
						<DomainClassMoniker Name="/Microsoft.VisualStudio.Modeling.Diagrams/Diagram"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
	</Relationships>
	<Types>
		<ExternalType Name="PointD" Namespace="Microsoft.VisualStudio.Modeling.Diagrams"/>
	</Types>

	<XmlSerializationBehavior Name="DiagramDisplayModelSerializationBehavior" Namespace=""/>

</Dsl>
