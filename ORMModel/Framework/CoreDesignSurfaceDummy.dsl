<?xml version="1.0" encoding="utf-8"?>
<Dsl
	xmlns="http://schemas.microsoft.com/VisualStudio/2005/DslTools/DslDefinitionModel"
	xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
	Id="91D59B16-E488-4A28-8D51-59273AD5BF2E" 
	Namespace="Microsoft.VisualStudio.Modeling.Diagrams"
	Name="CoreDesignSurface"
	DisplayName="Core Design Surface Domain Model"
	CompanyName="Microsoft"
	ProductName="Domain Specific Language Tools"
	MajorVersion="1" MinorVersion="0" Build="0" Revision="0">
	<!-- This is a dummy DSL file we can reference to force the generators to recognize
	Diagram-related elements as standard domain elements. It is not complete, extend as needed.-->

	<Classes>
		<DomainClass Name="PresentationElement" Namespace="Microsoft.VisualStudio.Modeling.Diagrams" Id="9321E0D1-1221-458D-834F-2DC0769CE683" DisplayName="PresentationElement" Description=""/>
		<DomainClass Name="ShapeElement" Namespace="Microsoft.VisualStudio.Modeling.Diagrams" Id="FFB3D9F5-7A47-4E12-8501-0055BD018825" DisplayName="ShapeElement" Description="">
			<BaseClass>
				<DomainClassMoniker Name="PresentationElement"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="NodeShape" Namespace="Microsoft.VisualStudio.Modeling.Diagrams" Id="D2CD161B-6BAA-4ED6-BD22-478B365502DC" DisplayName="NodeShape" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ShapeElement"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="Diagram" Namespace="Microsoft.VisualStudio.Modeling.Diagrams" Id="3102E0D1-1221-458D-834F-2DC0769CE683" DisplayName="Diagram" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NodeShape"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="Compartment" Namespace="Microsoft.VisualStudio.Modeling.Diagrams" Id="CF908511-03D6-4D10-B53A-DDDC44D66A9A" DisplayName="Compartment" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NodeShape"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="ListCompartment" Namespace="Microsoft.VisualStudio.Modeling.Diagrams" Id="F869CFDF-04A5-4D22-A97E-7EC5BCA2096E" DisplayName="ListCompartment" Description="">
			<BaseClass>
				<DomainClassMoniker Name="Compartment"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="ElementListCompartment" Namespace="Microsoft.VisualStudio.Modeling.Diagrams" Id="AFF8951D-DCD5-4E68-A3A1-DB963399CBF7" DisplayName="ElementListCompartment" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ListCompartment"/>
			</BaseClass>
		</DomainClass>
	</Classes>

	<XmlSerializationBehavior Name="CoreDesignSurfaceDomainModelSerializationBehavior" Namespace=""/>

</Dsl>
