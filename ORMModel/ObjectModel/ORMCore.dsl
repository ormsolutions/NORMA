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
	Id="3EAE649F-E654-4D04-8289-C25D2C0322D8"
	Namespace="Neumont.Tools.ORM.ObjectModel"
	PackageNamespace="Neumont.Tools.ORM.Shell"
	Name="ORMCore"
	DisplayName="ORM Core Domain Model"
	CompanyName="Neumont University"
	ProductName="Neumont ORM Architect for Visual Studio"
	MajorVersion="1" MinorVersion="0" Build="0" Revision="0">

	<Attributes>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;F60BC3F1-C38E-4C7D-9EE5-9211DB26CB45&quot;/*Neumont.Tools.Modeling.FrameworkDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
	</Attributes>

	<Classes>
		<DomainClass Name="NameConsumer" Namespace="Neumont.Tools.ORM.ObjectModel" Id="491389AA-B7DB-4461-B3CE-8064F8DE4072" DisplayName="NameConsumer" Description=""/>

		<DomainClass Name="OmittedWord" Namespace="Neumont.Tools.ORM.ObjectModel" Id="DF462D31-C2E4-47A5-AF48-7FFC55DE4B2A" DisplayName="OmittedWord" Description="">
			<Properties>
				<DomainProperty Name="Word" DisplayName="Word" Id="80822CFE-225B-4618-B0F1-C83C60D45881" Description="">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="NameGenerator" Namespace="Neumont.Tools.ORM.ObjectModel" Id="E032727F-440A-431A-82E7-2454BE939C82" DisplayName="Name Generation Defaults" Description="">
      <Attributes>
        <ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
          <Parameters>
            <AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.ElementTypeDescriptionProvider&lt;NameGenerator, Design.NameGeneratorTypeDescriptor&lt;NameGenerator&gt;&gt;)"/>
          </Parameters>
        </ClrAttribute>
      </Attributes>
      <BaseClass>
				<DomainClassMoniker Name="NameConsumer"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="NameUsage" DisplayName="NameUsage" IsBrowsable="false" Id="B92D3173-900E-4F35-BAC2-32A607E744FA" Description="" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="CasingOption" DefaultValue="None" DisplayName="CasingOption" Id="30950747-68E7-4A64-8ED7-BACEDAAFD4A2" Description="">
					<Type>
						<DomainEnumerationMoniker Name="NameGeneratorCasingOption"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="SpacingFormat" DefaultValue="Retain" DisplayName="SpacingFormat" Id="CFD3E74C-DE51-4FD9-ADDB-01B0F5414BFD" Description="">
					<Type>
						<DomainEnumerationMoniker Name="NameGeneratorSpacingFormat"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="SpacingReplacement" DefaultValue="" DisplayName="SpacingReplacement" Id="0E203ACB-3611-4180-9324-7FD7D30A5AE4" Description="">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="AutomaticallyShortenNames" DefaultValue="true" DisplayName="AutomaticallyShortenNames" Id="6A3526D9-AFB3-417E-A988-A44644AA094E" Description="">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="UseTargetDefaultMaximum" DefaultValue="true" DisplayName="UseTargetDefaultMaximum" Id="67B83B76-394F-4702-A984-6009DC51D224" Description="">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="UserDefinedMaximum" DefaultValue="128" DisplayName="UserDefinedMaximum" Id="FC154AD5-AB52-4AC8-856C-28B00395ABF4" Description="">
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
				<!--<DomainProperty Name="SubjectArea" DefaultValue="None" DisplayName="SubjectArea" Id="0C59CC43-FC54-44EE-9DBA-EE33648EC50C" Description="">
					<Type>
						<DomainEnumerationMoniker Name="NameGeneratorSubjectArea"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="SubjectAreaText" DefaultValue="" DisplayName="SubjectAreaText" Id="69D57F4B-BDF0-44BD-A62B-0FF49E5D1D31" Description="">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>-->
			</Properties>
		</DomainClass>

		<DomainClass Name="NameUsage" Namespace="Neumont.Tools.ORM.ObjectModel" Id="5B10B7DC-2018-41D9-AEF6-E12104614CA0" DisplayName="NameUsage" InheritanceModifier="Abstract" Description="">
			<Attributes>
				<ClrAttribute Name="Neumont.Tools.ORM.ObjectModel.NameUsageIdentifier">
					<Parameters>
						<AttributeParameter Value="&quot;NameUsage&quot;"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainClass>

		<DomainClass Name="GenerationState" Namespace="Neumont.Tools.ORM.ObjectModel" Id="CD0749E6-DDB0-4890-A559-EB70D3F698E0" DisplayName="GenerationState" Description=""/>
		<DomainClass Name="GenerationSetting" Namespace="Neumont.Tools.ORM.ObjectModel" Id="B707A1D2-87D1-43EA-93B0-92ED9308A0A5" InheritanceModifier="Abstract" DisplayName="GenerationSetting" Description=""/>

		<DomainClass Name="ModelErrorCategory" Namespace="Neumont.Tools.ORM.ObjectModel" Id="C9730E21-67A1-47E1-A065-B08C2B3815CE" DisplayName="ModelErrorCategory" InheritanceModifier="Abstract" Description=""/>
		<DomainClass Name="ModelErrorDisplayFilter" Namespace="Neumont.Tools.ORM.ObjectModel" Id="67CDCE7B-3D28-4A92-B9EB-00418152A13F" DisplayName="ModelErrorDisplayFilter" InheritanceModifier="Sealed" Description="">
			<Properties>
				<DomainProperty Name="ExcludedCategories" DefaultValue="" DisplayName="ExcludedCategories" Id="46F355F4-001C-4A3F-8A0F-56BEC4EACDEB" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="IncludedErrors" DefaultValue="" DisplayName="IncludedErrors" Id="D83D0737-79B4-415D-9C93-73442F3C606F" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="ExcludedErrors" DefaultValue="" DisplayName="ExcludedErrors" Id="593D1E1A-C01D-48E8-8385-1507C7F95A25" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
		<DomainClass Name="PopulationErrorCategory" Namespace="Neumont.Tools.ORM.ObjectModel" Id="18C1AE31-7241-453E-9DCB-9409ACA41896" DisplayName="Sample Population Errors" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelErrorCategory"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="DataTypeAndValueErrorCategory" Namespace="Neumont.Tools.ORM.ObjectModel" Id="D98CE8A4-2CB0-423E-80AC-4E4E7A963EC3" DisplayName="DataType and Value Errors" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelErrorCategory"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="ConstraintImplicationAndContradictionErrorCategory" Namespace="Neumont.Tools.ORM.ObjectModel" Id="A8846FAF-A765-4E39-A876-CFA09A1FFB3A" DisplayName="Constraint Implication and Contradiction Errors" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelErrorCategory"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="ConstraintStructureErrorCategory" Namespace="Neumont.Tools.ORM.ObjectModel" Id="FEABDE83-E7B0-44C1-B6C8-3F0EF3E09589" DisplayName="Constraint Structure Errors" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelErrorCategory"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="FactTypeDefinitionErrorCategory" Namespace="Neumont.Tools.ORM.ObjectModel" Id="1360B437-C64F-4A30-956B-47D4F1C7E85B" DisplayName="Fact Type Definition Errors" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelErrorCategory"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="ReferenceSchemeErrorCategory" Namespace="Neumont.Tools.ORM.ObjectModel" Id="2F515685-36D3-4631-A6C4-572BD9644FD7" DisplayName="Reference Scheme Errors" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelErrorCategory"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="NameErrorCategory" Namespace="Neumont.Tools.ORM.ObjectModel" Id="57AE5888-89E0-4449-B0C8-80802DEB014C" DisplayName="Naming Errors" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelErrorCategory"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ORMModelElement" Namespace="Neumont.Tools.ORM.ObjectModel" Id="BFBBEE5E-C691-4299-B958-77AC1B701F28" DisplayName="ORMModelElement" InheritanceModifier="Abstract" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.ElementTypeDescriptionProvider&lt;ORMModelElement, Design.ORMModelElementTypeDescriptor&lt;ORMModelElement&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainClass>

		<DomainClass Name="ORMNamedElement" Namespace="Neumont.Tools.ORM.ObjectModel" Id="C2BE18BA-BC16-4764-BAA1-18E721435BCE" DisplayName="ORMNamedElement" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Name" DefaultValue="" DisplayName="Name" IsElementName="true" Id="4A557C1E-0A89-49B7-B4BD-FA095F6267D7">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.MergableProperty">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="ORMModel" Namespace="Neumont.Tools.ORM.ObjectModel" Id="73E1F528-9E60-4198-AAC2-F8D6CCF62EB3" DisplayName="ORMModel" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMNamedElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="ModelErrorDisplayFilterDisplay" DefaultValue="" DisplayName="ErrorDisplay" Id="C5A66492-0FFA-46F9-A64B-361E62D696B0" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.ObjectModel.Design.ModelErrorDisplayFilterEditor)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/Neumont.Tools.ORM.ObjectModel/ModelErrorDisplayFilter"/>
					</Type>
				</DomainProperty>
			</Properties>
			<ElementMergeDirectives>
				<ElementMergeDirective UsesCustomAccept="false" UsesCustomMerge="true">
					<Index>
						<DomainClassMoniker Name="ObjectType"/>
					</Index>
					<LinkCreationPaths>
						<DomainPath>ModelHasObjectType.ObjectTypeCollection</DomainPath>
					</LinkCreationPaths>
				</ElementMergeDirective>
				<ElementMergeDirective UsesCustomAccept="false" UsesCustomMerge="false">
					<Index>
						<DomainClassMoniker Name="FactType"/>
					</Index>
					<LinkCreationPaths>
						<DomainPath>ModelHasFactType.FactTypeCollection</DomainPath>
					</LinkCreationPaths>
				</ElementMergeDirective>
				<ElementMergeDirective UsesCustomAccept="true" UsesCustomMerge="false">
					<Index>
						<DomainClassMoniker Name="SetConstraint"/>
					</Index>
					<LinkCreationPaths>
						<DomainPath>ModelHasSetConstraint.SetConstraintCollection</DomainPath>
					</LinkCreationPaths>
				</ElementMergeDirective>
				<ElementMergeDirective UsesCustomAccept="false" UsesCustomMerge="false">
					<Index>
						<DomainClassMoniker Name="SetComparisonConstraint"/>
					</Index>
					<LinkCreationPaths>
						<DomainPath>ModelHasSetComparisonConstraint.SetComparisonConstraintCollection</DomainPath>
					</LinkCreationPaths>
				</ElementMergeDirective>
				<ElementMergeDirective UsesCustomAccept="false" UsesCustomMerge="false">
					<Index>
						<DomainClassMoniker Name="ModelNote"/>
					</Index>
					<LinkCreationPaths>
						<DomainPath>ModelHasModelNote.NoteCollection</DomainPath>
					</LinkCreationPaths>
				</ElementMergeDirective>
			</ElementMergeDirectives>
		</DomainClass>


		<DomainClass Name="ObjectType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="2FED415E-1786-4FBF-8556-A507F2F765FD" DisplayName="ObjectType" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.ElementTypeDescriptionProvider&lt;ObjectType, Design.ObjectTypeTypeDescriptor&lt;ObjectType&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="ORMNamedElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="IsExternal" DefaultValue="false" DisplayName="IsExternal" Id="D03828FD-1DA7-4804-A16B-CC27F2046F57">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="DefinitionText" DefaultValue="" DisplayName="Definition" Description="A definition of this ObjectType. To insert new lines, use Control-Enter in the dropdown editor, or open the 'ORM Notes Editor' toolwindow." Id="431A8A8F-E8EC-4014-B1A1-843E55751A55" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.MultilineTextEditor&lt;global::Neumont.Tools.ORM.ObjectModel.Definition&gt;)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.MergableProperty">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="NoteText" DefaultValue="" DisplayName="Note" Description="A note to associate with this ObjectType. To insert new lines, use Control-Enter in the dropdown editor, or open the 'ORM Notes Editor' toolwindow." Id="17C4E23D-CA49-4329-982F-48F4EFCA23BD" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.MultilineTextEditor&lt;global::Neumont.Tools.ORM.ObjectModel.Note&gt;)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.MergableProperty">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="IsIndependent" DefaultValue="false" DisplayName="IsIndependent" Id="D52257EF-D76A-404D-AAC5-7450BA5CC790">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="IsValueType" DefaultValue="false" DisplayName="IsValueType" Id="F63ACB94-8526-432E-964C-3B4441195754" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.RefreshProperties">
							<Parameters>
								<AttributeParameter Value="global::System.ComponentModel.RefreshProperties.All"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="Scale" DefaultValue="0" DisplayName="DataTypeScale" Id="BD2D708A-7687-4218-94BC-05834AFAC869" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="Length" DefaultValue="0" DisplayName="DataTypeLength" Id="C9B01797-2CA1-4FF8-865A-FDA0DDF33F8D" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="ReferenceModeDisplay" DefaultValue="" DisplayName="RefMode" Id="2E56D25A-BD96-4478-A55C-9F17A15C94B6" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.ObjectModel.Design.ReferenceModePicker)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.ObjectModel.Design.ReferenceModeConverter)"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Object"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="ReferenceModeString" DefaultValue="" DisplayName="ReferenceModeString" Id="CE61C7AD-B177-4C56-8843-149C01439D25" IsBrowsable="false" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="ReferenceModeDecoratedString" DefaultValue="" DisplayName="ReferenceModeDecoratedString" Id="E139F1E3-DC42-498F-BB70-890C3B1FDD13" IsBrowsable="false" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="ReferenceMode" DefaultValue="" DisplayName="ReferenceMode" Id="E2049BA3-F50D-4E1C-9ABD-8A7EBECFEDF5" IsBrowsable="false" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/Neumont.Tools.ORM.ObjectModel/ReferenceMode"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="DataTypeDisplay" DefaultValue="" DisplayName="DataType" Id="3E8893A7-5985-4200-A595-CB1E9EC9ADA7" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.ObjectModel.Design.DataTypePicker)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/Neumont.Tools.ORM.ObjectModel/DataType"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="ValueRangeText" DefaultValue="" DisplayName="ValueRange" Id="F0662C59-700B-435C-B57B-93E5FD84B71F" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="ValueTypeValueRangeText" DefaultValue="" DisplayName="ValueTypeValueRange" Id="6EBE45BB-1054-4785-8C9D-905A41599EF9" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="IsPersonal" DefaultValue="false" DisplayName="IsPersonal" Id="EF9AE461-4327-46DC-8FE0-D1388F061B30">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="IsImplicitBooleanValue" DefaultValue="false" DisplayName="IsImplicitBooleanValue" Id="AE665FCF-B90A-41BD-B3E9-8611B42E668C" IsBrowsable="false">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="FactType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="723A2B28-6CDA-4185-B597-87866E257265" DisplayName="FactType" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.ElementTypeDescriptionProvider&lt;FactType, Design.FactTypeTypeDescriptor&lt;FactType&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="IsExternal" DefaultValue="false" DisplayName="IsExternal" Id="67EA8C95-FD9A-473B-8AA2-E35FCDD68361">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="DefinitionText" DefaultValue="" DisplayName="Definition" Description="A definition of this FactType. To insert new lines, use Control-Enter in the dropdown editor, or open the 'ORM Notes Editor' toolwindow." Id="3F58E4D1-4562-478A-A3FE-08715E455CD8" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.MultilineTextEditor&lt;global::Neumont.Tools.ORM.ObjectModel.Definition&gt;)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.MergableProperty">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="NoteText" DefaultValue="" DisplayName="Note" Description="A note to associate with this FactType. To insert new lines, use Control-Enter in the dropdown editor, or open the 'ORM Notes Editor' toolwindow." Id="AF6200B1-068D-434A-98D3-44E872B921BD" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.MultilineTextEditor&lt;global::Neumont.Tools.ORM.ObjectModel.Note&gt;)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.MergableProperty">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="Name" DefaultValue="" DisplayName="Name" IsElementName="true" Id="B17F5E42-A0FA-4B88-9D24-D148CEEE7DB0" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.MergableProperty">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="GeneratedName" DefaultValue="" DisplayName="GeneratedName" IsElementName="false" Id="F6FC3149-2ED8-458D-A29C-FD640A810A79" IsBrowsable="false" Kind="CustomStorage" GetterAccessModifier="Private" SetterAccessModifier="Private">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="NameChanged" DefaultValue="" DisplayName="NameChanged" Id="20A75B4B-69D4-4D1B-BEB5-9B0D66FDB1F3" IsBrowsable="false" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/Int64"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="DerivationRuleDisplay" DefaultValue="" DisplayName="DerivationRule" Id="7AF5C436-C28A-49BA-B8E0-05C409B67358" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.ObjectModel.Design.DerivationRuleEditor)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.MergableProperty">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="DerivationStorageDisplay" DefaultValue="" DisplayName="DerivationStorage" Id="307C9629-ACE8-43E1-ABF3-33E8BB7146B7" Kind="CustomStorage">
					<Type>
						<DomainEnumerationMoniker Name="DerivationStorageType"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="SubtypeFact" Namespace="Neumont.Tools.ORM.ObjectModel" Id="7A957450-AD7E-4C29-AF59-A10F8C8052CC" DisplayName="SubtypeRelationship">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.ElementTypeDescriptionProvider&lt;SubtypeFact, Design.SubtypeFactTypeDescriptor&lt;SubtypeFact&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="FactType"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="IsPrimary" DefaultValue="false" IsBrowsable="false" DisplayName="IsPrimary" Id="9A2A6585-7CAA-41F9-8117-9F357A6C3626">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="ProvidesPreferredIdentifier" DefaultValue="false" DisplayName="IdentificationPath" Id="E4E9E28D-1A60-4321-857E-018F39AA3EE3" Description="The preferred identification scheme for the subtype is provided by a supertype reached through this path.">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.MergableProperty">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="RoleBase" Namespace="Neumont.Tools.ORM.ObjectModel" Id="62293718-2F14-4A4C-88EB-0BA3AA6B7B91" DisplayName="RoleBase" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="RoleProxy" Namespace="Neumont.Tools.ORM.ObjectModel" Id="FF552152-BD43-4731-8EDA-675E68D6C5DB" DisplayName="RoleProxy" Description="">
			<BaseClass>
				<DomainClassMoniker Name="RoleBase"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="Role" Namespace="Neumont.Tools.ORM.ObjectModel" Id="291FEB71-371A-4B23-9DDC-61154A10A3D7" DisplayName="Role" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.ElementTypeDescriptionProvider&lt;Role, Design.RoleTypeDescriptor&lt;Role&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="RoleBase"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="RolePlayerDisplay" DefaultValue="" DisplayName="RolePlayer" Id="B66FCA99-E6EC-46C9-B445-D549F6D7ABE1" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.ObjectModel.Design.RolePlayerPicker)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/Neumont.Tools.ORM.ObjectModel/ObjectType"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="IsMandatory" DefaultValue="false" DisplayName="IsMandatory" Id="0F5EED7E-7584-413A-9250-BD4624DC164E" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="Multiplicity" DefaultValue="Unspecified" DisplayName="Multiplicity" Id="ADA46024-61B8-4E1D-BB28-2FF2C71B83CD" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.MergableProperty">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<DomainEnumerationMoniker Name="RoleMultiplicity"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="ValueRangeText" DefaultValue="" DisplayName="ValueRange" Id="3882C0AC-6F4A-4CF1-B856-E57A2DD4650C" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="MandatoryConstraintName" DefaultValue="" DisplayName="MandatoryConstraintName" Id="A6680C0F-84B1-499C-8B58-1E1C5D09570C" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="MandatoryConstraintModality" DefaultValue="" DisplayName="MandatoryConstraintModality" Id="29B14765-434B-4CCF-9C93-BEE8BB7E2697" Kind="CustomStorage">
					<Type>
						<DomainEnumerationMoniker Name="ConstraintModality"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="Name" DefaultValue="" DisplayName="Name" Id="F173D0FA-8F94-479D-8794-2572B8CD8D9A">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.MergableProperty">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="ObjectificationOppositeRoleName" DefaultValue="" DisplayName="ImpliedRoleName" Id="4719AAC4-E0E7-467A-B261-CDB8AE9826ED" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.MergableProperty">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="NameAlias" Namespace="Neumont.Tools.ORM.ObjectModel" Id="A0AD1270-E3D1-4851-A5AB-D87E5942F9AE" DisplayName="NameAlias" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMNamedElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="NameConsumer" DisplayName="NameConsumer" Id="BE9EDEB2-C60E-4446-BAC0-73CCD61716EA" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="NameUsage" DisplayName="NameUsage" Id="18DBB768-B471-4926-B678-5B2245760333" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="EqualityConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" Id="E4F8E935-C07C-4269-81E3-978110F6DC68" DisplayName="EqualityConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SetComparisonConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ExclusionConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" Id="7766C350-ADFC-464C-B200-E4473F551E03" DisplayName="ExclusionConstraint" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.ElementTypeDescriptionProvider&lt;ExclusionConstraint, Design.ExclusionConstraintTypeDescriptor&lt;ExclusionConstraint&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="SetComparisonConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="SubsetConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" Id="9B5982E3-A852-4071-A973-9719F87546F0" DisplayName="SubsetConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SetComparisonConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="SetComparisonConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" Id="85074B82-ED14-4D70-B95C-0B29F2D64210" DisplayName="SetComparisonConstraint" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMNamedElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Modality" DefaultValue="Alethic" DisplayName="Modality" Id="C0AEF802-D9E9-4938-B44B-DE9A6A530D9B">
					<Type>
						<DomainEnumerationMoniker Name="ConstraintModality"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="Expression" Namespace="Neumont.Tools.ORM.ObjectModel" Id="1B62AF68-86A9-4A14-8B32-8988041BBCCF" DisplayName="Expression" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Body" DefaultValue="" DisplayName="Body" Id="9760D258-0126-4749-A370-D7CC5A04F138">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="Language" DefaultValue="" DisplayName="Language" Id="53D116FA-E39C-47C5-A4D6-41E42786EEDB">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>

		</DomainClass>

		<DomainClass Name="SetComparisonConstraintRoleSequence" Namespace="Neumont.Tools.ORM.ObjectModel" Id="9E59F946-8745-4936-A4AA-74552664790E" DisplayName="SetComparisonConstraintRoleSequence" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ConstraintRoleSequence"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="RingConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" Id="31792DBE-49EB-4544-9FB4-3A692AAC39C9" DisplayName="RingConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SetConstraint"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="RingType" DefaultValue="Undefined" DisplayName="RingType" Id="54D182E1-6650-4393-8BD8-9D9E42BB8CE7">
					<Type>
						<DomainEnumerationMoniker Name="RingConstraintType"/>
					</Type>
				</DomainProperty>
			</Properties>

		</DomainClass>

		<DomainClass Name="FrequencyConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" Id="A6D76D01-FDC3-43A2-8AAF-56C2E0BD0465" DisplayName="FrequencyConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SetConstraint"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="MinFrequency" DefaultValue="1" DisplayName="MinFrequency" Id="2D48D3CA-564D-459E-A701-4209A12C4783">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.ObjectModel.Design.FrequencyConstraintMinConverter)"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="MaxFrequency" DefaultValue="2" DisplayName="MaxFrequency" Id="F46D9200-3602-435C-B852-C53BE10D99C6">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.ObjectModel.Design.FrequencyConstraintMaxPicker)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.ObjectModel.Design.FrequencyConstraintMaxConverter)"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="UniquenessConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" Id="49C7E3CE-C4F9-417D-B49C-27EA4016371E" DisplayName="UniquenessConstraint" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.ElementTypeDescriptionProvider&lt;UniquenessConstraint, Design.UniquenessConstraintTypeDescriptor&lt;UniquenessConstraint&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="SetConstraint"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="IsPreferred" DefaultValue="false" DisplayName="IsPreferredIdentifier" Id="585DE7A0-8E09-43F3-8463-F20609A16790" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="IsInternal" DefaultValue="false" DisplayName="IsInternal" Id="55B187ED-3869-4F83-B6A7-8661D61B1C62" IsBrowsable="false">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="MandatoryConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" Id="F054BE4D-BFCA-4CD3-A0D8-97F61C165753" DisplayName="InclusiveOrConstraint" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.ElementTypeDescriptionProvider&lt;MandatoryConstraint, Design.MandatoryConstraintTypeDescriptor&lt;MandatoryConstraint&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="SetConstraint"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="IsSimple" DefaultValue="false" DisplayName="IsSimple" Id="BBDA6BD3-B3AC-4E26-ABC3-3FE1DAFA0165" IsBrowsable="false">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="IsImplied" DefaultValue="false" DisplayName="IsImplied" Id="EBF58507-1C28-4C7B-8C1B-ED4C319C9C3C" IsBrowsable="false">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="SetConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" Id="1B85E4BE-0C95-45BD-A76F-2087456F891B" DisplayName="SetConstraint" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ConstraintRoleSequence"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Modality" DefaultValue="Alethic" DisplayName="Modality" Id="B4F1902A-7EB9-464F-A0F8-F816658C1BD8">
					<Type>
						<DomainEnumerationMoniker Name="ConstraintModality"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="ConstraintRoleSequence" Namespace="Neumont.Tools.ORM.ObjectModel" Id="E279C66B-E89C-4E02-9DE2-64791C8A4511" DisplayName="ConstraintRoleSequence" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMNamedElement"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="Join" Namespace="Neumont.Tools.ORM.ObjectModel" Id="EFDE476D-C440-4524-97DA-42697FA92CE9" DisplayName="Join" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.ElementTypeDescriptionProvider&lt;Join, Design.JoinTypeDescriptor&lt;Join&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="JoinType" DefaultValue="Inner" DisplayName="JoinType" Id="59049038-A13E-4AD1-A86B-8EC3493DFDC9">
					<Type>
						<DomainEnumerationMoniker Name="JoinType"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="TooFewRoleSequencesError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="686A4B07-0ED9-4143-8225-5524C4D6C001" DisplayName="Too Few Role Sequences" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TooManyRoleSequencesError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="1ADACF12-94F5-430D-9E14-6A3B0334139E" DisplayName="Too Many Role Sequences" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ObjectTypeDuplicateNameError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="798D4CC7-1AD8-4A83-AFD5-5730AC342DC2" DisplayName="Duplicate ObjectType Names" Description="">
			<BaseClass>
				<DomainClassMoniker Name="DuplicateNameError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="PopulationUniquenessError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="BA0A8F9E-91E1-4D56-8A44-9F49432C63C5" DisplayName="Population Violates Uniqueness Constraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ContradictionError" InheritanceModifier="Abstract" Namespace="Neumont.Tools.ORM.ObjectModel" Id="B42B88E4-CA87-4DFA-90BE-00606E4BE23B" DisplayName="ContradictionError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ExclusionContradictsMandatoryError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="5A57EA68-918D-4AE3-AF7F-D9F7CDB5AB34" DisplayName="Contradicting Exclusion and Mandatory Constraints" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ContradictionError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ExclusionContradictsEqualityError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="D8714F74-60B7-48F5-BF7D-88D8736CB22A" DisplayName="Contradicting Exclusion and Equality Constraints" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ContradictionError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ExclusionContradictsSubsetError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="F671FE6D-BA8A-4BF2-AFB6-BE5827996C50" DisplayName="Contradicting Exclusion and Subset Constraints" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ContradictionError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="NotWellModeledSubsetAndMandatoryError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="2DECDC39-E109-4D59-8BF3-046E2CD8584C" DisplayName="Contradicting Subset and Mandatory Constraints" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="PopulationMandatoryError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="5B76CB18-90B2-4656-BB0D-0788460FDB70" DisplayName="Missing Mandatory Sample Population" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ConstraintDuplicateNameError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="AA63E81B-6978-49A2-A4AC-86022A172EDD" DisplayName="Duplicate Constraint Names" Description="">
			<BaseClass>
				<DomainClassMoniker Name="DuplicateNameError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="DuplicateNameError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="9E29C624-4559-4020-9163-7B5846C94C0C" DisplayName="DuplicateNameError" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TooFewReadingRolesError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="1D2B23EF-456E-4E80-91D8-FB384F779A54" DisplayName="FactType has Fewer Roles than Reading Text" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TooManyReadingRolesError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="50C98172-412C-40C0-ADD3-82809C3D82F7" DisplayName="FactType has More Roles than Reading Text" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ExternalConstraintRoleSequenceArityMismatchError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="3DA5385A-D9DE-4F3D-9D2E-CA79F10AB542" DisplayName="Constraint Role Sequences with Different Numbers of Roles" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="FactTypeRequiresReadingError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="3ECA7E92-45B2-45BD-BAD3-6AF0C4B40E70" DisplayName="FactType Requires Reading" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="FactTypeRequiresInternalUniquenessConstraintError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="295D4B3D-1351-431D-B72F-28661D744B58" DisplayName="FactType Requires Internal Uniqueness Constraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="DataTypeNotSpecifiedError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="8AFA102F-529C-4896-AEB3-9D714E28FC61" DisplayName="DataType not Specified for ValueType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="NMinusOneError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="497754B3-5176-4712-BC46-2E4377354C8B" DisplayName="Insufficient Roles for Internal Uniqueness Constraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="CompatibleRolePlayerTypeError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="5C8D3150-2604-44FC-A468-B678F9B4206E" DisplayName="Incompatible Constrained Role Players" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Column" DefaultValue="0" DisplayName="Column" Id="222DCF1C-83FB-43F1-A8BE-3D05B8CF1693">
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
			</Properties>

		</DomainClass>

		<DomainClass Name="RolePlayerRequiredError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="59A21FDE-D979-4B18-9088-707B79FCE19E" DisplayName="Role Player Required" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="EqualityImpliedByMandatoryError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="0809316F-AE25-4D6A-8FF2-8CE8A685D32D" DisplayName="Equality Constraint Implied By Mandatory Constraint(s)" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="EntityTypeRequiresReferenceSchemeError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="A9177733-169B-418A-A843-3E3777DC9982" DisplayName="EntityType Requires Reference Scheme" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="FrequencyConstraintMinMaxError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="5586C408-1A46-4CA7-8B0D-0462CD904009" DisplayName="Inconsistent Frequency Constraint Minimum and Maximum Values" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="FrequencyConstraintExactlyOneError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="9BBFF3C2-329B-4956-8FFA-1C6F305CF601" DisplayName="Represent Frequency Constraint of Exactly One as Uniqueneness" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ReadingRequiresUserModificationError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="56D0B016-EAF3-4E4F-B17A-7F7987EBC0CB" DisplayName="Reading Text Automatically Modified" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ModelError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="16DF5C5E-83EF-4EDC-B54A-56D58D62D982" DisplayName="ModelError" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="ErrorText" DefaultValue="" DisplayName="ErrorText" IsElementName="true" Id="6A6023E7-AC27-4D86-AFE4-6428659A048E">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.MergableProperty">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="ReferenceModeKind" Namespace="Neumont.Tools.ORM.ObjectModel" Id="7EC5E835-5EEB-4FB1-AA09-9BD6ABA531E1" DisplayName="ReferenceModeKind" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="FormatString" DefaultValue="" DisplayName="FormatString" Id="3D1B9C67-FF56-4345-B445-30F1F3367613">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="ReferenceModeType" DefaultValue="General" DisplayName="ReferenceModeType" Id="3543E2CB-037D-4D6E-A76A-10CBDFB05146">
					<Type>
						<DomainEnumerationMoniker Name="ReferenceModeType"/>
					</Type>
				</DomainProperty>
			</Properties>

		</DomainClass>

		<DomainClass Name="IntrinsicReferenceMode" Namespace="Neumont.Tools.ORM.ObjectModel" Id="F34A46FD-D7EA-4423-B40F-90A6662CADB9" DisplayName="IntrinsicReferenceMode" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ReferenceMode"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="CustomReferenceMode" Namespace="Neumont.Tools.ORM.ObjectModel" Id="BB33470D-3C98-4B2E-9134-9347C8008861" DisplayName="CustomReferenceMode" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ReferenceMode"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="CustomFormatString" DefaultValue="" DisplayName="CustomFormatString" Id="4A7202FF-1D4F-4770-953A-D63ADA849CB3">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>

		</DomainClass>

		<DomainClass Name="ReferenceMode" Namespace="Neumont.Tools.ORM.ObjectModel" Id="5123D945-262C-42B7-838D-1B7F4E5A911C" DisplayName="ReferenceMode" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMNamedElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="KindDisplay" DefaultValue="" DisplayName="Kind" Id="BBC452CA-0454-4047-9143-B11E065556FB" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.ObjectModel.Design.ReferenceModeKindPicker)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/Neumont.Tools.ORM.ObjectModel/ReferenceModeKind"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="UnspecifiedDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="B7DDA0A4-C18A-4E85-8259-F529FC45F72E" DisplayName="UnspecifiedDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="DataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="FixedLengthTextDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="2B525C4C-9B55-4C8D-98BB-63739E9D7C3D" DisplayName="FixedLengthTextDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="TextDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="VariableLengthTextDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="6F30DE79-85BE-4194-B362-A39023A0E200" DisplayName="VariableLengthTextDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="TextDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="LargeLengthTextDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="27CBCB76-FAC5-436A-950A-CC428FEC9EED" DisplayName="LargeLengthTextDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="TextDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TextDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="E1BE56BD-6663-4F5C-AF6A-39E03DFB2BFA" DisplayName="TextDataType" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="DataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="SignedIntegerNumericDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="F4962B12-8C72-4FEF-9C24-D23A5872A403" DisplayName="SignedIntegerNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="SignedSmallIntegerNumericDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="67825482-E490-4A16-B47D-53E72F4EBEE3" DisplayName="SignedSmallIntegerNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="SignedLargeIntegerNumericDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="AF56003A-52F1-4C02-B203-EF15B4CB2AE1" DisplayName="SignedLargeIntegerNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="UnsignedIntegerNumericDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="9D76D09D-10F6-4DB0-8890-1077A95FB364" DisplayName="UnsignedIntegerNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="UnsignedTinyIntegerNumericDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="A14599A2-2B27-4E76-894F-A1814723EFE9" DisplayName="UnsignedTinyIntegerNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="UnsignedSmallIntegerNumericDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="0EDC60E7-0548-48C3-BC6B-219AF6E50A31" DisplayName="UnsignedSmallIntegerNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="UnsignedLargeIntegerNumericDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="0ED930F6-73C7-4E04-8898-E5F30EF3A641" DisplayName="UnsignedLargeIntegerNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="AutoCounterNumericDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="E2F2CD9B-5C9D-439D-AEAC-A2F093ED04FE" DisplayName="AutoCounterNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="FloatingPointNumericDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="C82CD420-BB13-4F63-9EA7-850512E5B7DD" DisplayName="FloatingPointNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="SinglePrecisionFloatingPointNumericDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="53399895-20E9-49E0-BC73-E00461387680" DisplayName="SinglePrecisionFloatingPointNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="DoublePrecisionFloatingPointNumericDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="F5A7B8A3-2EF5-4143-BDEE-1AA762CB6E02" DisplayName="DoublePrecisionFloatingPointNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="DecimalNumericDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="B86FAADD-E5CF-4745-A796-FABD0310A4A8" DisplayName="DecimalNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="MoneyNumericDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="520A70DA-ACC3-47B2-B8EF-00AF2FF6D170" DisplayName="MoneyNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="NumericDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="CCCFB38B-956F-4E71-8CDC-7A9CD7D6052C" DisplayName="NumericDataType" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="DataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="FixedLengthRawDataDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="1AA62E47-0EB5-45B7-B1FA-AC17EF24E009" DisplayName="FixedLengthRawDataDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="RawDataDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="VariableLengthRawDataDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="01A2EA3B-BC60-4E62-8819-26E81B8D871F" DisplayName="VariableLengthRawDataDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="RawDataDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="LargeLengthRawDataDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="FF35CE6D-2BB6-4DF4-A98C-D303A5698AD2" DisplayName="LargeLengthRawDataDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="RawDataDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="PictureRawDataDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="D33DACE5-3D70-4678-9325-058C1CCFD81F" DisplayName="PictureRawDataDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="RawDataDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="OleObjectRawDataDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="0B79F4F9-09B6-408A-88A2-F8B1051C2B05" DisplayName="OleObjectRawDataDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="RawDataDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="RawDataDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="C5683946-DF1B-42AF-947A-006DD6875CCF" DisplayName="RawDataDataType" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="DataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="AutoTimestampTemporalDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="5553662C-93B7-4C7B-8723-FF56963AE644" DisplayName="AutoTimestampTemporalDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="TemporalDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TimeTemporalDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="13138B79-3CB9-479E-AC5B-569A755085C4" DisplayName="TimeTemporalDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="TemporalDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="DateTemporalDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="ABC122E6-894B-446E-8CD4-EAD7D61FCC46" DisplayName="DateTemporalDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="TemporalDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="DateAndTimeTemporalDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="A5B3A699-DFB5-4522-B024-F55BDE90AC6A" DisplayName="DateAndTimeTemporalDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="TemporalDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TemporalDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="BFBEF833-DD04-4DB3-A167-D1314273B2C6" DisplayName="TemporalDataType" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="DataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TrueOrFalseLogicalDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="689EA7B7-31A8-4800-A98E-99CCD21E112C" DisplayName="TrueOrFalseLogicalDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="LogicalDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="YesOrNoLogicalDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="7E694D96-8444-4007-BFEB-C1B0BD3F96DE" DisplayName="YesOrNoLogicalDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="LogicalDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="LogicalDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="A7D4D492-2702-4B87-BD9E-0D7D7D85943A" DisplayName="LogicalDataType" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="DataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="RowIdOtherDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="42A558F7-1A61-49A6-A207-A706FAF94DD8" DisplayName="RowIdOtherDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="OtherDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ObjectIdOtherDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="48B82DAB-A7E2-4DAB-8D53-9840CF7A15DD" DisplayName="ObjectIdOtherDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="OtherDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="OtherDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="0B801E67-9D48-49F7-AA13-9C7BD8153624" DisplayName="OtherDataType" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="DataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="DataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="9D0C6367-617F-4A8C-A0E5-5DA23828ED61" DisplayName="DataType" InheritanceModifier="Abstract" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.ElementTypeDescriptionProvider&lt;DataType, Design.DataTypeTypeDescriptor&lt;DataType&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="Reading" Namespace="Neumont.Tools.ORM.ObjectModel" Id="7544854F-A4A7-4429-8859-F1D3B0E52B03" DisplayName="Reading" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.ElementTypeDescriptionProvider&lt;Reading, Design.ReadingTypeDescriptor&lt;Reading&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Text" DefaultValue="" DisplayName="Text" Id="A6239359-0AC5-4934-B38A-011AA1F935A6">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="IsPrimaryForReadingOrder" DefaultValue="false" DisplayName="IsPrimaryForReadingOrder" Id="1A989428-C41C-498A-BD90-1B92A703AA27" IsBrowsable="false" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="Language" DefaultValue="" DisplayName="Language" Id="34C42F00-5D21-4731-8E38-9A03271F045A">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="IsPrimaryForFactType" DefaultValue="false" DisplayName="IsPrimaryForFactType" Id="1C5A6551-972D-42A6-B43D-AEC6D7301977" IsBrowsable="false" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="ReadingOrder" Namespace="Neumont.Tools.ORM.ObjectModel" Id="7CB4A39B-D11F-48FC-BFED-B80F5D3FC54E" DisplayName="ReadingOrder" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="ReadingText" DefaultValue="" DisplayName="ReadingText" IsUIReadOnly="true" Id="4E75AD63-A42B-4571-85CE-81A4C5E02C23" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.ObjectModel.Design.ReadingTextEditor)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="ValueRange" Namespace="Neumont.Tools.ORM.ObjectModel" Id="8987ECEA-6C2A-4825-8C9F-465005272CE8" DisplayName="ValueRange" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="MinValue" DefaultValue="" DisplayName="MinValue" Id="59B141FD-47ED-43FF-837E-858F140FAD57">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="MaxValue" DefaultValue="" DisplayName="MaxValue" Id="08199824-9DDC-4878-8E04-E0F432069726">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="Text" DefaultValue="" DisplayName="Text" Id="1FB8C126-4481-41D0-B41C-5A30BC7245DE" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="MinInclusion" DefaultValue="NotSet" DisplayName="MinInclusion" Id="CDE9FC53-BE51-4C27-9E6C-675CDB580F3A">
					<Type>
						<DomainEnumerationMoniker Name="RangeInclusion"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="MaxInclusion" DefaultValue="NotSet" DisplayName="MaxInclusion" Id="EB018230-2726-4206-AE2E-1C911B606FC1">
					<Type>
						<DomainEnumerationMoniker Name="RangeInclusion"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="ValueTypeValueConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" Id="E46B0A2E-460E-4FF7-B447-C9C09597B500" DisplayName="ValueTypeValueConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ValueConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="RoleValueConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" Id="6C223B62-6239-4514-81C5-AAD6A10D3A2D" DisplayName="RoleValueConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ValueConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ValueConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" Id="EF2EFEAD-A124-413C-8F86-C95E2B47160C" DisplayName="ValueConstraint" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMNamedElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Text" DefaultValue="" DisplayName="Text" Id="410FCE34-DACB-4F59-94A6-FF7E42108E74" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="TextChanged" DefaultValue="" DisplayName="TextChanged" Id="ACB49806-A830-431D-959F-20C7DD9C1D4D" IsBrowsable="false" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/Int64"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="ValueMismatchError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="A18FA855-E7CA-4716-8E8D-1606C09B090A" DisplayName="Value Constraint Value Invalid for DataType" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="MinValueMismatchError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="7E0D53CF-D374-4EDA-B6A6-04D381AA0DC5" DisplayName="Minimum Bound of Value Range Invalid for DataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ValueMismatchError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="MaxValueMismatchError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="CCE42465-23A0-4726-8881-3ADB48E2CC67" DisplayName="Maximum Bound of Value Range Invalid for DataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ValueMismatchError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ImpliedInternalUniquenessConstraintError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="B7381F8B-C95E-408D-9747-4B6BB35C1171" DisplayName="FactType has Implied Internal Uniqueness Constraint(s)" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="FrequencyConstraintContradictsInternalUniquenessConstraintError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="A080C2B2-F666-4689-A63E-BD97CB0491E2" DisplayName="Contradicting Frequency and Internal Uniqueness Constraints" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="RingConstraintTypeNotSpecifiedError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="15026270-DFD6-470D-A997-233173E644DC" DisplayName="Ring Constraint Type Not Specified" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="SubtypeMetaRole" Namespace="Neumont.Tools.ORM.ObjectModel" Id="4AD109E1-3AB4-4F8A-A862-1694AEE06289" DisplayName="SubtypeMetaRole" Description="">
			<BaseClass>
				<DomainClassMoniker Name="Role"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="SupertypeMetaRole" Namespace="Neumont.Tools.ORM.ObjectModel" Id="E559A725-BBA4-4068-B247-DC8C4B1628D7" DisplayName="SupertypeMetaRole" Description="">
			<BaseClass>
				<DomainClassMoniker Name="Role"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="Definition" Namespace="Neumont.Tools.ORM.ObjectModel" Id="25D3235C-76E2-4095-8EFD-847057937A00" DisplayName="Definition" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Text" DefaultValue="" Description="The definition contents. To insert new lines, use Control-Enter in the dropdown editor, or open the 'ORM Notes Editor' toolwindow." DisplayName="Definition" Id="B68867A8-4B52-4DE1-8B39-7EEE5ECB60A4">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.MultilineTextEditor&lt;global::Neumont.Tools.ORM.ObjectModel.Definition&gt;)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.MergableProperty">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="Note" Namespace="Neumont.Tools.ORM.ObjectModel" Id="C3DE6C8C-2215-49B0-BD70-70D2C3630C33" DisplayName="Note" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Text" DefaultValue="" Description="The note contents. To insert new lines, use Control-Enter in the dropdown editor, or open the 'ORM Notes Editor' toolwindow." DisplayName="Note" Id="0EF3BC12-45FF-46A8-B325-CDFCC105A1E1">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.MultilineTextEditor&lt;global::Neumont.Tools.ORM.ObjectModel.Note&gt;)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.MergableProperty">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="ModelNote" Namespace="Neumont.Tools.ORM.ObjectModel" Id="41D610C9-BACC-473D-BFE6-7034E6FF0B11" DisplayName="ModelNote" Description="">
			<BaseClass>
				<DomainClassMoniker Name="Note"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="CompatibleSupertypesError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="70A9ED25-7A0E-4DEC-B39D-83BB1A6294B8" DisplayName="Incompatible or Transitive Supertypes" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ImplicationError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="78026AEA-19EB-497A-A596-25C929F67AA8" DisplayName="Constraint Implied by Intersecting Constraints" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="EqualityOrSubsetImpliedByMandatoryError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="80B1F784-858E-483B-91A5-E55CFEBA44B9" DisplayName="Mandatory Constraint Implies Equality or Subset Constraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ImplicationError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="PreferredIdentifierRequiresMandatoryError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="129CCE68-7CE9-4A97-BAD3-C36B4D372A77" DisplayName="EntityType with Compound Preferred Identifier Requires Mandatory Constraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ValueRangeOverlapError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="2CF1EE1A-1737-4868-9B5C-95B2C0F9488B" DisplayName="Value Ranges Overlap" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ValueConstraintValueTypeDetachedError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="92C7060E-A912-4986-984E-E9915B1321AD" DisplayName="Path to Identifying ValueType Detached" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="FactTypeDerivationExpression" Namespace="Neumont.Tools.ORM.ObjectModel" Id="2A29F892-B69B-4EEB-BF50-A0E59B6E64C2" DisplayName="FactTypeDerivationExpression" Description="">
			<BaseClass>
				<DomainClassMoniker Name="Expression"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="DerivationStorage" DefaultValue="Derived" DisplayName="DerivationStorage" Id="6B011B44-9854-436A-ADED-7BBC635A7C1F">
					<Type>
						<DomainEnumerationMoniker Name="DerivationStorageType"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="ObjectTypeInstance" Namespace="Neumont.Tools.ORM.ObjectModel" Id="870F5EE8-0859-4710-A526-66635F4EFD14" DisplayName="ObjectTypeInstance" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Name" DefaultValue="" DisplayName="Name" IsElementName="true" Id="553DEB12-8FE0-4FE4-B94E-52F1CD5DCF0A" Kind="Calculated">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.MergableProperty">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="NameChanged" DefaultValue="" DisplayName="NameChanged" Id="F1E0BB68-F047-464B-B17B-6BA865144BB4" IsBrowsable="false" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/Int64"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="EntityTypeInstance" Namespace="Neumont.Tools.ORM.ObjectModel" Id="5F8B6A1C-3020-41C9-97B4-E54A3E98B368" DisplayName="EntityTypeInstance" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ObjectTypeInstance"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ValueTypeInstance" Namespace="Neumont.Tools.ORM.ObjectModel" Id="BCC1483D-CBB8-4E4F-903B-16224768F6F5" DisplayName="ValueTypeInstance" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ObjectTypeInstance"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Value" DefaultValue="" DisplayName="Value" Id="1D0232BA-A92F-4B81-99BF-2A2A44821030">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>

		</DomainClass>

		<DomainClass Name="FactTypeInstance" Namespace="Neumont.Tools.ORM.ObjectModel" Id="78458A27-FDB1-4B6E-9D0A-D42DD8D5AEAD" DisplayName="FactTypeInstance" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TooFewEntityTypeRoleInstancesError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="39F447EA-8EA4-483D-B791-848AD27544E2" DisplayName="Incomplete Sample Population to Identify EntityType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TooFewFactTypeRoleInstancesError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="BE44DD74-2569-421E-8E1B-ABCDC7810C92" DisplayName="Incomplete FactType Sample Population" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="CompatibleValueTypeInstanceValueError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="D5B21137-31E8-444D-BCD2-58BBF442B4C0" DisplayName="Sample Population Value Invalid for DataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

	</Classes>

	<Relationships>
		<!--<DomainRelationship Name="ORMElementLink" Namespace="Neumont.Tools.ORM.ObjectModel" AllowsDuplicates="false" InheritanceModifier="Abstract" Id="D6DC3311-4298-4EE5-9DA2-B1378AB09BF1">
			<Source>
				<DomainRole Name="SourceRole" PropertyName="TargetRoleCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="SourceRole" Id="97F9C6E3-18C8-49F8-89C2-50563AAC4ECE">
					<RolePlayer>
						<DomainClassMoniker Name="/Microsoft.VisualStudio.Modeling/ModelElement"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="TargetRole" PropertyName="SourceRoleCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="TargetRole"  Id="635F5B0D-F4CA-4321-89A7-FA0ACE028562">
					<RolePlayer>
						<DomainClassMoniker Name="/Microsoft.VisualStudio.Modeling/ModelElement"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>-->

		<DomainRelationship Name="FactConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" AllowsDuplicates="false" InheritanceModifier="Abstract" Id="BCF635F2-F2C6-4690-956D-2A44C48A9DA9">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="FactType" PropertyName="ConstraintCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="FactType" Id="D60CB2BF-7DE7-4CED-A00F-BF7C3A2E5248">
					<RolePlayer>
						<DomainClassMoniker Name="FactType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Constraint" PropertyName="FactTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="Constraint" Id="9B305629-1EFA-404F-AE8E-475117B287AE">
					<RolePlayer>
						<DomainClassMoniker Name="ORMNamedElement"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FactSetComparisonConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" AllowsDuplicates="false" Id="FF8F65AD-248A-4EF8-9172-515204C9A44C">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="FactConstraint"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="FactType" PropertyName="SetComparisonConstraintCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="F7733FAF-1029-480E-8FEA-96FDD65AB212">
					<RolePlayer>
						<DomainClassMoniker Name="FactType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="SetComparisonConstraint" PropertyName="FactTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetComparisonConstraint" Id="575F65E4-682E-427A-B273-3D30D909A816">
					<RolePlayer>
						<DomainClassMoniker Name="SetComparisonConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FactSetConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" AllowsDuplicates="false" Id="771EC962-8086-4B21-BFB2-830F30E52861">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="FactConstraint"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="FactType" PropertyName="SetConstraintCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="DE9A381F-5346-4C95-9D48-E468B8CF8A29">
					<RolePlayer>
						<DomainClassMoniker Name="FactType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="SetConstraint" PropertyName="FactTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetConstraint" Id="7789FD46-6E28-4AB7-AFC5-7F17B95AC4D9">
					<RolePlayer>
						<DomainClassMoniker Name="SetConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ORMModelElementHasExtensionElement" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="FF867109-FE3A-42C4-9770-2D735555016A">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ExtendedElement" PropertyName="ExtensionCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ExtendedElement" Id="9105A491-7FC1-408E-8E07-F8E79CA0BFA4">
					<RolePlayer>
						<DomainClassMoniker Name="ORMModelElement"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Extension" PropertyName="ExtendedElement" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="false" DisplayName="Extension" Id="0A7BBD8D-0D86-4FB4-991B-365302D1ED63">
					<RolePlayer>
						<DomainClassMoniker Name="/Microsoft.VisualStudio.Modeling/ModelElement"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ORMModelElementHasExtensionModelError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="7A4D2B10-43F3-475F-AA0A-8F880B9A1E4B">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="ExtendedElement" PropertyName="ExtensionModelErrorCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ExtendedElement" Id="415C3EF5-7524-45A9-9307-3D8B53BD88D6">
					<RolePlayer>
						<DomainClassMoniker Name="ORMModelElement"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ExtensionModelError" PropertyName="ExtendedElement" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="false" DisplayName="ExtensionModelError" Id="1A7A14EF-01FC-4ED8-A1EA-3533511D1750">
					<RolePlayer>
						<DomainClassMoniker Name="ModelError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ValueTypeHasDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="E4BBC988-E920-4ACB-8071-552AEEBA7FA9">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Properties>
				<DomainProperty Name="Scale" DefaultValue="0" DisplayName="Scale" Id="F21936E2-E7E6-4AFC-B96F-43E9C76F8A9B">
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="Length" DefaultValue="0" DisplayName="Length" Id="60D1471D-23C9-4D4D-91AF-6AA5E9BA7B8B">
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
			</Properties>
			<Source>
				<DomainRole Name="ValueType" PropertyName="DataType" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ValueType" Id="3F6D8D0A-CEC5-47EF-8F81-EF25F59593E0">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="DataType" PropertyName="ValueTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="DataType" Id="0414C824-F797-4F95-8F25-7D275FD632B8">
					<RolePlayer>
						<DomainClassMoniker Name="DataType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="Objectification" Namespace="Neumont.Tools.ORM.ObjectModel" Id="935DC968-DDD1-4C57-9D43-9F367BE78C6D">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Properties>
				<DomainProperty Name="IsImplied" DefaultValue="false" DisplayName="IsImplied" Id="7D34DD15-B4D2-4144-AC1C-0FFFD54DA865" IsBrowsable="false">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
			<Source>
				<DomainRole Name="NestingType" PropertyName="NestedFactType" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="NestingType" PropertyDisplayName="ObjectifiedFactType" Id="2660CF3E-2A56-496D-98CD-BFFAC5E73198">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.ObjectModel.Design.NestedFactTypePicker)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.MergableProperty">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.TypeConverter"/>
					</Attributes>
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="NestedFactType" PropertyName="NestingType" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="NestedFactType" PropertyDisplayName="ObjectifyingEntityType" Id="69F805CC-874F-4E03-8364-0A0445168B26">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.ObjectModel.Design.NestingTypePicker)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.MergableProperty">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.TypeConverter"/>
					</Attributes>
					<RolePlayer>
						<DomainClassMoniker Name="FactType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ObjectTypePlaysRole" Namespace="Neumont.Tools.ORM.ObjectModel" Id="0AB8D25E-45D4-4696-B6EE-6F108FEE97A7">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="PlayedRole" PropertyName="RolePlayer" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="PlayedRole" Id="A87B6EEB-1753-4AD3-A00D-431E34B05AC2">
					<RolePlayer>
						<DomainClassMoniker Name="Role"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="RolePlayer" PropertyName="PlayedRoleCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="RolePlayer" Id="8EC5C761-2E7C-422C-B5E7-354788A18F59">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ModelHasObjectType" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="F060C714-EF07-481F-AB4B-BA02B9908025">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Model" PropertyName="ObjectTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="E3FA6F15-EF49-4B72-B02F-EC7C2BA718EC">
					<RolePlayer>
						<DomainClassMoniker Name="ORMModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ObjectType" PropertyName="Model" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ObjectType" Id="F827BD9B-9EDA-41C6-BAE9-ACFD8A19BA08">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ModelHasFactType" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="DF97B102-8500-4EA1-9059-356BC49E7066">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Model" PropertyName="FactTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="CC174187-4E88-4230-ADBD-B468F58AB58D">
					<RolePlayer>
						<DomainClassMoniker Name="ORMModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="FactType" PropertyName="Model" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="FactType" Id="972619DE-83C9-4A7B-A2C5-A626F02D192B">
					<RolePlayer>
						<DomainClassMoniker Name="FactType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ModelHasError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="20CFE989-A6AF-4D97-A552-AE5DD7684971">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Model" PropertyName="ErrorCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="Model" Id="8F57FA08-7038-4CDB-900A-450A9A9DD8DC">
					<RolePlayer>
						<DomainClassMoniker Name="ORMModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Error" PropertyName="Model" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Error" Id="48331657-5405-4A04-B772-23A9413788A4">
					<RolePlayer>
						<DomainClassMoniker Name="ModelError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ModelHasReferenceModeKind" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="1B280979-E9F5-4774-847F-3A1078DB1943">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Model" PropertyName="ReferenceModeKindCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="966465E7-6BAD-4100-A082-B4AA20511A7D">
					<RolePlayer>
						<DomainClassMoniker Name="ORMModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ReferenceModeKind" PropertyName="Model" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ReferenceModeKind" Id="234CDD35-B21E-48D2-A430-5D9EC306FA67">
					<RolePlayer>
						<DomainClassMoniker Name="ReferenceModeKind"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ModelHasReferenceMode" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="F6647D65-926B-4E66-81BC-F6293A44093E">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Model" PropertyName="ReferenceModeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="88B428C5-E93F-4739-82E5-440E6B13921A">
					<RolePlayer>
						<DomainClassMoniker Name="ORMModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ReferenceMode" PropertyName="Model" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ReferenceMode" Id="4EBAAA54-37E3-43D5-A1CD-C8AC5DA274E4">
					<RolePlayer>
						<DomainClassMoniker Name="ReferenceMode"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ModelHasSetConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="C0104439-3B39-41E7-9B68-61F31F17A066">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Model" PropertyName="SetConstraintCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="54B0D8A2-91B9-41A3-8571-103DCB7BECCD">
					<RolePlayer>
						<DomainClassMoniker Name="ORMModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="SetConstraint" PropertyName="Model" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="SetConstraint" Id="550F7793-1381-4D37-A5E2-78C48D0F1331">
					<RolePlayer>
						<DomainClassMoniker Name="SetConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ObjectTypeImpliesMandatoryConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" Id="9A43A30E-6F18-46FF-9B8D-5313F6E93807">
			<Source>
				<DomainRole Name="ObjectType" PropertyName="ImpliedMandatoryConstraint" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ImpliedByObjectType" Id="929DFC02-4C1E-4D43-90D7-1112C3CF757B">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="MandatoryConstraint" PropertyName="ImpliedByObjectType" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ImpliedMandatoryConstraint" Id="C4E7DFA7-13B4-463F-BF84-4032DE22DA88">
					<RolePlayer>
						<DomainClassMoniker Name="MandatoryConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ModelHasSetComparisonConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="37FBE5B6-4E18-43E2-B34B-DAB0EF69DDE4">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Model" PropertyName="SetComparisonConstraintCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="0D5738FB-77EF-41C2-82EE-A98E5484B11B">
					<RolePlayer>
						<DomainClassMoniker Name="ORMModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="SetComparisonConstraint" PropertyName="Model" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="SetComparisonConstraint" Id="C176DA76-1A98-4369-B25F-BC1DED5B388C">
					<RolePlayer>
						<DomainClassMoniker Name="SetComparisonConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ModelHasDataType" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="33611A97-9270-469E-AB75-B53A24699A2D">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Model" PropertyName="DataTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="41F9A40E-DEDD-4BBA-9C79-8548ACFBCB9A">
					<RolePlayer>
						<DomainClassMoniker Name="ORMModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="DataType" PropertyName="Model" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="DataType" Id="FF17FEFB-0E20-4326-8637-4C3984B8D20B">
					<RolePlayer>
						<DomainClassMoniker Name="DataType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="GenerationStateHasGenerationSetting" IsEmbedding="true"  Namespace="Neumont.Tools.ORM.ObjectModel" Id="475FF8F0-0E0D-4CF1-8110-C132F815E2E6">
			<Source>
				<DomainRole Name="GenerationState" PropertyName="GenerationSettingCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="GenerationState" Id="F9949793-300A-4CE6-B969-CFBE5A2A1982">
					<RolePlayer>
						<DomainClassMoniker Name="GenerationState"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="GenerationSetting" PropertyName="GenerationState" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="GenerationSetting" Id="AFE2FC08-6B47-40E6-9CC3-B943900A95B2">
					<RolePlayer>
						<DomainClassMoniker Name="GenerationSetting"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="NameGeneratorRefinesNameGenerator" IsEmbedding="true" Namespace="Neumont.Tools.ORM.ObjectModel" Id="6224BA97-F59F-4360-A159-7CD5DDB6493F">
			<Source>
				<DomainRole Name="Parent" PropertyName="RefinedByGeneratorCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="RefinedByGeneratorCollection" Id="A6585A27-D26A-49B0-BBED-CA133CC1E261">
					<RolePlayer>
						<DomainClassMoniker Name="NameGenerator"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Refinement" PropertyName="RefinesGenerator" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Refinement" Id="05748872-B408-46EB-AB0C-2BD06E819887">
					<RolePlayer>
						<DomainClassMoniker Name="NameGenerator"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ElementHasAlias" InheritanceModifier="Abstract" Namespace="Neumont.Tools.ORM.ObjectModel" Id="94F76133-EACC-40D1-B61E-1EBD32C0F81F">
			<Source>
				<DomainRole Name="Element" PropertyName="AliasCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="Element" Id="4CCF4DEB-AB2F-402B-AAF5-55D51AC0F6DB">
					<RolePlayer>
						<DomainClassMoniker Name="/Microsoft.VisualStudio.Modeling/ModelElement"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Alias" PropertyName="Element" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Alias" Id="DAE4D333-A8B3-448F-94C2-56286FB60A1F">
					<RolePlayer>
						<DomainClassMoniker Name="NameAlias"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ObjectTypeHasAbbreviation" Namespace="Neumont.Tools.ORM.ObjectModel" DisplayName="ObjectType Abbreviations" IsEmbedding="true" Id="6A85513C-747F-4A8C-B45A-B5CFF88314E5">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementHasAlias"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="ObjectType" PropertyName="AbbreviationCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ObjectType" Id="2C863159-2675-4F32-A30D-83C573A207C7">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Abbreviation" PropertyName="Element" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="false" DisplayName="Alias" Id="832E3923-BBB7-4A29-A60C-66B03E0A92DA">
					<RolePlayer>
						<DomainClassMoniker Name="NameAlias"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="NameGeneratorContainsOmittedWord" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="3DE59564-603C-48ED-8EA4-D51AE8E761F1">
			<Source>
				<DomainRole Name="NameGenerator" PropertyName="OmittedWordCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="NameGenerator" Id="965FF527-A2D6-4468-94DE-464489E332E2">
					<RolePlayer>
						<DomainClassMoniker Name="NameGenerator"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="OmittedWord" PropertyName="NameGenerator" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="OmittedWord" Id="6D4F2B86-2C27-4F82-84CE-8AA23DCC0EF8">
					<RolePlayer>
						<DomainClassMoniker Name="OmittedWord"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>



		<DomainRelationship Name="ExternalRoleConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" AllowsDuplicates="false" Id="9692D61F-13AE-4FEE-9F76-8E0D9A5FF976">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ConstrainedRole" PropertyName="FactConstraintCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ConstrainedRole" Id="F87A3EC5-C735-4E45-A9EE-DADE8E2CFD37">
					<RolePlayer>
						<DomainRelationshipMoniker Name="ConstraintRoleSequenceHasRole"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="FactConstraint" PropertyName="ConstrainedRoleCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactConstraint" Id="8E11E9E0-DEC5-405D-8757-E34582339384">
					<RolePlayer>
						<DomainRelationshipMoniker Name="FactConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ExclusiveOrConstraintCoupler" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="false" Id="F2244A4C-BBE0-463B-9E8B-6A768C5C1469">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="MandatoryConstraint" PropertyName="ExclusiveOrExclusionConstraint" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ExclusiveOrExclusionConstraint" Id="5CB84560-B945-4D22-BD70-523D502FBB95">
					<RolePlayer>
						<DomainClassMoniker Name="MandatoryConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ExclusionConstraint" PropertyName="ExclusiveOrMandatoryConstraint" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ExclusiveOrMandatoryConstraint" Id="6413EE7E-A13F-4330-A45E-79727EA49A30">
					<RolePlayer>
						<DomainClassMoniker Name="ExclusionConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SetComparisonConstraintHasRoleSequence" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="84B13BEA-FC8C-446C-B643-9688B99AF1B6">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ExternalConstraint" PropertyName="RoleSequenceCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ExternalConstraint" Id="1D11FC93-6110-44F7-BFE1-38FC7DC81170">
					<RolePlayer>
						<DomainClassMoniker Name="SetComparisonConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="RoleSequence" PropertyName="ExternalConstraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="RoleSequence" Id="603112CD-EEF4-4659-84F2-F210C8B234D5">
					<RolePlayer>
						<DomainClassMoniker Name="SetComparisonConstraintRoleSequence"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConstraintRoleSequenceHasRole" Namespace="Neumont.Tools.ORM.ObjectModel" AllowsDuplicates="false" Id="BD1A0274-1152-4A54-B4A5-58BD023CE710">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Role" PropertyName="ConstraintRoleSequenceCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Role" Id="1A5A347E-1D5D-4045-9EA0-13B2338FC898">
					<RolePlayer>
						<DomainClassMoniker Name="Role"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ConstraintRoleSequence" PropertyName="RoleCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ConstraintRoleSequence" Id="9AD53904-280A-4329-A6F0-20E2C44F5607">
					<RolePlayer>
						<DomainClassMoniker Name="ConstraintRoleSequence"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SetComparisonConstraintHasTooFewRoleSequencesError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="3167F5D3-C234-46E3-AAC2-4CEB791DFB9C">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="SetComparisonConstraint" PropertyName="TooFewRoleSequencesError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetComparisonConstraint" Id="0178C877-8906-4BDC-B3F8-0322A578741D">
					<RolePlayer>
						<DomainClassMoniker Name="SetComparisonConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="TooFewRoleSequencesError" PropertyName="SetComparisonConstraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="TooFewRoleSequencesError" Id="1BA9841F-59A2-475C-BB3F-7497B7F6315E">
					<RolePlayer>
						<DomainClassMoniker Name="TooFewRoleSequencesError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SetComparisonConstraintHasTooManyRoleSequencesError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="E7C33130-2D1F-4F95-B988-BD7608CF2D1C">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="SetComparisonConstraint" PropertyName="TooManyRoleSequencesError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetComparisonConstraint" Id="76082669-3E03-4837-8824-526BB25DACB8">
					<RolePlayer>
						<DomainClassMoniker Name="SetComparisonConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="TooManyRoleSequencesError" PropertyName="SetComparisonConstraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="TooManyRoleSequencesError" Id="C4C9E95C-B71C-4EEC-A101-6CA827169545">
					<RolePlayer>
						<DomainClassMoniker Name="TooManyRoleSequencesError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ObjectTypeHasDuplicateNameError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="BC1031EB-8590-4A14-ABBD-F12A18622855">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="ObjectType" PropertyName="DuplicateNameError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ObjectType" Id="A2252380-7CAC-4D36-8857-2426AE558C08">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="DuplicateNameError" PropertyName="ObjectTypeCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="DuplicateNameError" Id="40422AA2-B5FD-4056-ABBF-D393358BC01A">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectTypeDuplicateNameError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="RoleInstanceHasPopulationUniquenessError" InheritanceModifier="Abstract" Namespace="Neumont.Tools.ORM.ObjectModel" Id="5DBE50CD-A939-484D-9B96-700CB6CC7813">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="RoleInstance" PropertyName="PopulationUniquenessError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="RoleInstance" Id="527FFAEB-8F4A-4DFF-B02F-49822FCE2F3D">
					<RolePlayer>
						<DomainRelationshipMoniker Name="RoleInstance"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="PopulationUniquenessError" PropertyName="RoleInstanceCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="PopulationUniquenessError" Id="3AE1D857-C7E7-4053-A461-3EB965666F2C">
					<RolePlayer>
						<DomainClassMoniker Name="PopulationUniquenessError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="EntityTypeRoleInstanceHasPopulationUniquenessError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="04312BEF-EA3E-4525-9A1A-903497EFDAF7">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="RoleInstanceHasPopulationUniquenessError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="RoleInstance" PropertyName="PopulationUniquenessError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="RoleInstance" Id="F3EF0D4D-5F76-4DAB-BB6E-B475E7DDA70D">
					<RolePlayer>
						<DomainRelationshipMoniker Name="EntityTypeRoleInstance"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="PopulationUniquenessError" PropertyName="EntityTypeRoleInstanceCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="PopulationUniquenessError" Id="0F8036BA-33AA-48BC-B058-1BD990A6E264">
					<RolePlayer>
						<DomainClassMoniker Name="PopulationUniquenessError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FactTypeRoleInstanceHasPopulationUniquenessError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="A483FDFE-53EF-4352-8D97-986BF2C0E8E7">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="RoleInstanceHasPopulationUniquenessError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="RoleInstance" PropertyName="PopulationUniquenessError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="RoleInstance" Id="14BF8CDB-9685-49AC-A7B9-696A72D6D97C">
					<RolePlayer>
						<DomainRelationshipMoniker Name="FactTypeRoleInstance"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="PopulationUniquenessError" PropertyName="FactTypeRoleInstanceCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="PopulationUniquenessError" Id="F00366B7-A23F-48D7-8977-0078EB2CD7B6">
					<RolePlayer>
						<DomainClassMoniker Name="PopulationUniquenessError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SetComparisonConstraintHasContradictionError" InheritanceModifier="Abstract" Namespace="Neumont.Tools.ORM.ObjectModel" Id="A1D4A389-9D19-4921-BD0D-D965B53897E3">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="SetComparisonConstraint" PropertyName="ContradictionError" Multiplicity="ZeroMany"  PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetComparisonConstraint" Id="61F38936-0F1D-4D08-BF95-75429D108D6E">
					<RolePlayer>
						<DomainClassMoniker Name="SetComparisonConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ContradictionError" PropertyName="SetComparisonConstraintCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ContradictionError" Id="0D23D71A-EE1C-42EB-A54B-B6CC0091EF58">
					<RolePlayer>
						<DomainClassMoniker Name="ContradictionError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SetComparisonConstraintHasExclusionContradictsEqualityError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="E7E85549-6312-4E65-AD48-4DDF51E8139C">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="SetComparisonConstraintHasContradictionError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="SetComparisonConstraint" PropertyName="ExclusionContradictsEqualityError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetComparisonConstraint" Id="A1CAE6F2-1FF6-455C-B345-FE1EABA54CC9">
					<RolePlayer>
						<DomainClassMoniker Name="SetComparisonConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ExclusionContradictsEqualityError" PropertyName="SetComparisonConstraintCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ExclusionContradictsEqualityError" Id="550FE83E-6DD2-49FE-B125-CA26DA12897F">
					<RolePlayer>
						<DomainClassMoniker Name="ExclusionContradictsEqualityError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SetComparisonConstraintHasExclusionContradictsSubsetError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="B4BBA5AF-05AC-4FEC-8288-8D80DC0AF16E">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="SetComparisonConstraintHasContradictionError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="SetComparisonConstraint" PropertyName="ExclusionContradictsSubsetError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetComparisonConstraint" Id="054006AC-2B5A-4084-ADFB-DFFCCEB18B0D">
					<RolePlayer>
						<DomainClassMoniker Name="SetComparisonConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ExclusionContradictsSubsetError" PropertyName="SetComparisonConstraintCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ExclusionContradictsSubsetError" Id="94EFA870-8061-41FB-8D99-FCDDBAE7FF54">
					<RolePlayer>
						<DomainClassMoniker Name="ExclusionContradictsSubsetError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ExclusionConstraintHasExclusionContradictsMandatoryError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="E638E328-24A9-42C0-BBB1-F1EBC4B6E218">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="ExclusionConstraint" PropertyName="ExclusionContradictsMandatoryError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ExclusionConstraint" Id="DAC3FEEB-4C79-46BC-876F-69CEA0DA8E7C">
					<RolePlayer>
						<DomainClassMoniker Name="ExclusionConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ExclusionContradictsMandatoryError" PropertyName="ExclusionConstraint" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ExclusionContradictsMandatoryError" Id="1D5F486E-469D-42B0-8876-5512CD22E808">
					<RolePlayer>
						<DomainClassMoniker Name="ExclusionContradictsMandatoryError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="MandatoryConstraintHasExclusionContradictsMandatoryError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="6CDDA5A5-C6FE-4E9B-9248-17512F9C891A">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="MandatoryConstraint" PropertyName="ExclusionContradictsMandatoryError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="MandatoryConstraint" Id="7687A52F-3510-4D26-861D-589AE3429790">
					<RolePlayer>
						<DomainClassMoniker Name="MandatoryConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ExclusionContradictsMandatoryError" PropertyName="MandatoryConstraint" Multiplicity="OneMany"  PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ExclusionContradictsMandatoryError" Id="65AEAC10-914E-46A1-9C0B-1653F49A1AF3">
					<RolePlayer>
						<DomainClassMoniker Name="ExclusionContradictsMandatoryError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SubsetConstraintHasNotWellModeledSubsetAndMandatoryError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="4BA72B40-A736-49D4-9FDE-8B07EE4A61A6">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="SubsetConstraint" PropertyName="NotWellModeledSubsetAndMandatoryError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SubsetConstraint" Id="9DC05203-7E4E-485D-9DD8-35E87A7B367A">
					<RolePlayer>
						<DomainClassMoniker Name="SubsetConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="NotWellModeledSubsetAndMandatoryError" PropertyName="SubsetConstraint" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="NotWellModeledSubsetAndMandatoryError" Id="D48F7913-B297-4D19-833C-811194FAE9FA">
					<RolePlayer>
						<DomainClassMoniker Name="NotWellModeledSubsetAndMandatoryError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="MandatoryConstraintHasNotWellModeledSubsetAndMandatoryError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="475557A9-6E11-4D1C-A5A0-9D06DAED3EE5">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="MandatoryConstraint" PropertyName="NotWellModeledSubsetAndMandatoryError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="MandatoryConstraint" Id="7D6BCCE4-5F44-473A-AA5D-3ED9115CD6C6">
					<RolePlayer>
						<DomainClassMoniker Name="MandatoryConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="NotWellModeledSubsetAndMandatoryError" PropertyName="MandatoryConstraint" Multiplicity="One"  PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="NotWellModeledSubsetAndMandatoryError" Id="1D06C86D-F4D2-4D52-94CB-05B0625B5AA0">
					<RolePlayer>
						<DomainClassMoniker Name="NotWellModeledSubsetAndMandatoryError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="MandatoryConstraintHasPopulationMandatoryError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="9A93DD53-8683-47F4-9EE7-4F1F244A218E">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="MandatoryConstraint" PropertyName="PopulationMandatoryErrorCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="MandatoryConstraint" Id="AF065878-27B3-456A-9CD4-E1B81DFFAD2D">
					<RolePlayer>
						<DomainClassMoniker Name="MandatoryConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="PopulationMandatoryError" PropertyName="MandatoryConstraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="PopulationMandatoryError" Id="35EC4E97-2248-4AA0-85AF-0891EDB7803B">
					<RolePlayer>
						<DomainClassMoniker Name="PopulationMandatoryError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ObjectTypeInstanceHasPopulationMandatoryError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="E0F0750E-47CB-44C6-B348-A9A1101475A7">
			<Source>
				<DomainRole Name="ObjectTypeInstance" PropertyName="PopulationMandatoryErrorCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ObjectTypeInstance" Id="4A0B3B52-B579-4E07-972C-59F4F98BEAC3">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectTypeInstance"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="PopulationMandatoryError" PropertyName="ObjectTypeInstance" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="PopulationMandatoryError" Id="078CF514-9F4C-44C2-9173-3A5F3EDFAFCB">
					<RolePlayer>
						<DomainClassMoniker Name="PopulationMandatoryError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ReadingOrderHasReading" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="F945750F-2F77-43F4-8314-E5B351913902">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ReadingOrder" PropertyName="ReadingCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ReadingOrder" Id="02C96E32-5A2A-4CFC-AB29-DF81B40FF0CE">
					<RolePlayer>
						<DomainClassMoniker Name="ReadingOrder"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Reading" PropertyName="ReadingOrder" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Reading" Id="DB4ABBCB-FCF0-43C9-94E5-AE458BA6EE03">
					<RolePlayer>
						<DomainClassMoniker Name="Reading"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FactTypeHasReadingOrder" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="14C7D6CB-0C30-4326-A877-D3AEE7A9FADF">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="FactType" PropertyName="ReadingOrderCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="D77A6514-B8F0-4C0C-B856-EE74DBBC1C41">
					<RolePlayer>
						<DomainClassMoniker Name="FactType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ReadingOrder" PropertyName="FactType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ReadingOrder" Id="53267E0C-C487-4CB6-B2FD-980CA30FFE99">
					<RolePlayer>
						<DomainClassMoniker Name="ReadingOrder"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ReferenceModeHasReferenceModeKind" Namespace="Neumont.Tools.ORM.ObjectModel" Id="8B022051-E094-435E-B985-688FFC89DC6D">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ReferenceMode" PropertyName="Kind" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ReferenceMode" Id="67F35299-D67F-4AE2-9159-E5EFF1FF8544">
					<RolePlayer>
						<DomainClassMoniker Name="ReferenceMode"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Kind" PropertyName="ReferenceModeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Kind" Id="CD7ED96F-DDB9-4242-94CF-B10255822F66">
					<RolePlayer>
						<DomainClassMoniker Name="ReferenceModeKind"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SetConstraintHasDuplicateNameError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="CB5DF90F-3917-4BD1-9807-A24F6D7C52F9">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="SetConstraint" PropertyName="DuplicateNameError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetConstraint" Id="B4623963-690D-4687-A95E-0FC998AA59EC">
					<RolePlayer>
						<DomainClassMoniker Name="SetConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="DuplicateNameError" PropertyName="SetConstraintCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="DuplicateNameError" Id="250ACF2E-45F0-48B9-8429-21859DCB701C">
					<RolePlayer>
						<DomainClassMoniker Name="ConstraintDuplicateNameError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SetComparisonConstraintHasDuplicateNameError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="CF7AF531-F3D3-42E4-A9F7-D44536DA9E53">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="SetComparisonConstraint" PropertyName="DuplicateNameError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetComparisonConstraint" Id="86A9CB44-0050-4E7C-9DF5-692F980F96EC">
					<RolePlayer>
						<DomainClassMoniker Name="SetComparisonConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="DuplicateNameError" PropertyName="SetComparisonConstraintCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="DuplicateNameError" Id="1711998F-52C3-4BC4-9F95-02589C1B301E">
					<RolePlayer>
						<DomainClassMoniker Name="ConstraintDuplicateNameError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="EntityTypeHasPreferredIdentifier" Namespace="Neumont.Tools.ORM.ObjectModel" Id="8FF87866-8213-4A03-85A8-B0275A265793">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="PreferredIdentifierFor" PropertyName="PreferredIdentifier" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="PreferredIdentifierFor" Id="04D998EE-030E-4A81-88BC-666CE4EFB3ED">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="PreferredIdentifier" PropertyName="PreferredIdentifierFor" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="PreferredIdentifier" Id="6195CE84-7CA8-4E13-B8C8-24438E2CF300">
					<RolePlayer>
						<DomainClassMoniker Name="UniquenessConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ReadingHasTooManyRolesError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="D2116BC7-25A8-455E-9347-414BD03B7546">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="Reading" PropertyName="TooManyRolesError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Reading" Id="AA517583-0A1B-4129-905E-A9EE3F59EE17">
					<RolePlayer>
						<DomainClassMoniker Name="Reading"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="TooManyRolesError" PropertyName="Reading" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="TooManyRolesError" Id="50D39649-4BF7-411B-B6B5-102377344379">
					<RolePlayer>
						<DomainClassMoniker Name="TooManyReadingRolesError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ReadingHasTooFewRolesError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="FC3E0A3C-40CE-4DED-8A6B-241C7B51C099">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="Reading" PropertyName="TooFewRolesError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Reading" Id="3A9889AA-6152-4E1E-A1EC-B100AD24A60A">
					<RolePlayer>
						<DomainClassMoniker Name="Reading"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="TooFewRolesError" PropertyName="Reading" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="TooFewRolesError" Id="19BFCFE6-EC81-48DC-9B4E-D026F1040AE1">
					<RolePlayer>
						<DomainClassMoniker Name="TooFewReadingRolesError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ReadingHasReadingRequiresUserModificationError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="3B7E4BBF-06B6-489E-BDF5-72EDEE5B87F4">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="Reading" PropertyName="RequiresUserModificationError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Reading" Id="1258A048-1355-406C-A9F4-49BCF704927F">
					<RolePlayer>
						<DomainClassMoniker Name="Reading"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="RequiresUserModificationError" PropertyName="Reading" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="RequiresUserModificationError" Id="31EE8A7B-0A15-40E8-ACEB-30E3A2138F93">
					<RolePlayer>
						<DomainClassMoniker Name="ReadingRequiresUserModificationError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SetComparisonConstraintHasExternalConstraintRoleSequenceArityMismatchError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="C5A25732-F5A7-409E-B56A-6419A951FB13">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="Constraint" PropertyName="ArityMismatchError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Constraint" Id="E75C8B14-01C7-4CEF-879D-BE6A1D922AA4">
					<RolePlayer>
						<DomainClassMoniker Name="SetComparisonConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ArityMismatchError" PropertyName="Constraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ArityMismatchError" Id="0F5D4090-69D4-4E6E-AA87-2BDC7792D75B">
					<RolePlayer>
						<DomainClassMoniker Name="ExternalConstraintRoleSequenceArityMismatchError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ElementAssociatedWithModelError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="5E4032B3-EF22-447B-9732-F25CACA1E613">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="AssociatedElement" PropertyName="AssociatedModelErrorCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="AssociatedElement" Id="58FBB087-5A43-482B-9565-5B8451F9AC8F">
					<RolePlayer>
						<DomainClassMoniker Name="/Microsoft.VisualStudio.Modeling/ModelElement"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ModelError" PropertyName="AssociatedElementCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ModelError" Id="9B58A0FF-46E2-487B-8032-1A9922858D2F">
					<RolePlayer>
						<DomainClassMoniker Name="ModelError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FactTypeHasFactTypeRequiresReadingError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="EEC8EB82-5B15-4B61-8737-DA1A54199A13">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="FactType" PropertyName="ReadingRequiredError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="C79D68BD-DBED-4487-A448-70B9EDC5E4D9">
					<RolePlayer>
						<DomainClassMoniker Name="FactType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ReadingRequiredError" PropertyName="FactType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ReadingRequiredError" Id="E2DF0D63-6D67-475C-8470-8913E24265E9">
					<RolePlayer>
						<DomainClassMoniker Name="FactTypeRequiresReadingError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FactTypeHasFactTypeRequiresInternalUniquenessConstraintError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="DD5FF7F8-7169-489B-9B8A-EDE3772F52BE">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="FactType" PropertyName="InternalUniquenessConstraintRequiredError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="42AED551-7F1A-4F16-AA39-682C9DBB8607">
					<RolePlayer>
						<DomainClassMoniker Name="FactType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="InternalUniquenessConstraintRequiredError" PropertyName="FactType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="InternalUniquenessConstraintRequiredError" Id="36FC676E-7C82-4756-BEA5-C4A690B41AB0">
					<RolePlayer>
						<DomainClassMoniker Name="FactTypeRequiresInternalUniquenessConstraintError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ValueTypeHasValueConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="3DD5CC0F-891E-4A88-A8B2-AEB28A4795E3">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ValueType" PropertyName="ValueConstraint" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ValueType" Id="2F42A8FD-AB49-4E0F-AF3A-1098BA77A4C1">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ValueConstraint" PropertyName="ValueType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ValueConstraint" Id="4A9B0738-AA8E-4BAE-B76A-0027FC06685D">
					<RolePlayer>
						<DomainClassMoniker Name="ValueTypeValueConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="RoleHasValueConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="BFB9DA2A-0EA6-46AB-B608-41440BDD0D84">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Role" PropertyName="ValueConstraint" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Role" Id="AF99B941-1811-4DFB-BD26-8F4148D3F1D9">
					<RolePlayer>
						<DomainClassMoniker Name="Role"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ValueConstraint" PropertyName="Role" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ValueConstraint" Id="464DFFCF-6633-45CD-8671-2C5E92AE89D2">
					<RolePlayer>
						<DomainClassMoniker Name="RoleValueConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ValueConstraintHasValueRange" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="53B596BA-0506-4533-80B0-391891C61C9A">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ValueConstraint" PropertyName="ValueRangeCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ValueConstraint" Id="D3706490-D843-45C0-8948-C6CC6A3D804C">
					<RolePlayer>
						<DomainClassMoniker Name="ValueConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ValueRange" PropertyName="ValueConstraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ValueRange" Id="42AFA13D-E73D-47CA-8750-6605CB820138">
					<RolePlayer>
						<DomainClassMoniker Name="ValueRange"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ValueTypeHasUnspecifiedDataTypeError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="F2A79E36-A317-4C36-81DA-D562D2AFBF09">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="ValueTypeHasDataType" PropertyName="DataTypeNotSpecifiedError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ValueTypeHasDataType" Id="E8122190-AE46-40D8-8040-118D577735A6">
					<RolePlayer>
						<DomainRelationshipMoniker Name="ValueTypeHasDataType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="DataTypeNotSpecifiedError" PropertyName="ValueTypeHasDataType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="DataTypeNotSpecifiedError" Id="90DA103D-40D2-421D-8BCE-88F657A8A996">
					<RolePlayer>
						<DomainClassMoniker Name="DataTypeNotSpecifiedError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SetComparisonConstraintHasCompatibleRolePlayerTypeError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="13410C4F-FFED-4B0F-AD0B-BD48D09B4310">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="SetComparisonConstraint" PropertyName="CompatibleRolePlayerTypeErrorCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetComparisonConstraint" Id="B1660D4F-F77A-4FE2-9DDC-DFBFAB545B92">
					<RolePlayer>
						<DomainClassMoniker Name="SetComparisonConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="CompatibleRolePlayerTypeError" PropertyName="SetComparisonConstraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="CompatibleRolePlayerTypeError" Id="159EB571-D8E0-495E-9F51-85EABCF95F0C">
					<RolePlayer>
						<DomainClassMoniker Name="CompatibleRolePlayerTypeError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SetConstraintHasCompatibleRolePlayerTypeError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="91CDE095-28D9-4852-B171-430FE5A29429">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="SetConstraint" PropertyName="CompatibleRolePlayerTypeError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetConstraint" Id="171D82AC-46AE-486D-B602-62A0F49CEBC0">
					<RolePlayer>
						<DomainClassMoniker Name="SetConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="CompatibleRolePlayerTypeError" PropertyName="SetConstraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="CompatibleRolePlayerTypeError" Id="60C1D366-C379-41DB-ADB6-213802DA7DD4">
					<RolePlayer>
						<DomainClassMoniker Name="CompatibleRolePlayerTypeError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="UniquenessConstraintHasNMinusOneError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="429F7144-1227-4D0E-B4F8-59AD6FFC7EB3">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="Constraint" PropertyName="NMinusOneError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Constraint" Id="6165AD47-FB70-4F43-936E-E162D0E8E917">
					<RolePlayer>
						<DomainClassMoniker Name="UniquenessConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="NMinusOneError" PropertyName="Constraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="NMinusOneError" Id="5C3EDC3B-E19D-4841-AA8A-47009692802F">
					<RolePlayer>
						<DomainClassMoniker Name="NMinusOneError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="RoleHasRolePlayerRequiredError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="09E6AC31-2CA1-4126-8C95-BFC571088B2D">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="Role" PropertyName="RolePlayerRequiredError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Role" Id="529B2BED-F8F3-4A9B-95B7-55D9A1ED5B44">
					<RolePlayer>
						<DomainClassMoniker Name="Role"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="RolePlayerRequiredError" PropertyName="Role" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="RolePlayerRequiredError" Id="AAD1B942-E191-4878-9421-33A7F2D201F7">
					<RolePlayer>
						<DomainClassMoniker Name="RolePlayerRequiredError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="EqualityConstraintHasEqualityImpliedByMandatoryError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="0D352E42-06D6-4AD1-AACE-3EA5AACDE302">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="EqualityConstraint" PropertyName="EqualityImpliedByMandatoryError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="EqualityConstraint" Id="FBF0117A-A2B8-4EF9-B263-24288F40CB4E">
					<RolePlayer>
						<DomainClassMoniker Name="EqualityConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="EqualityImpliedByMandatoryError" PropertyName="EqualityConstraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="EqualityImpliedByMandatoryError" Id="6C8C0E5F-9376-490B-98F7-67607F791480">
					<RolePlayer>
						<DomainClassMoniker Name="EqualityImpliedByMandatoryError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ObjectTypeHasEntityTypeRequiresReferenceSchemeError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="1B6CBB8C-D1A6-4949-AC4D-596DC1CE147F">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="ObjectType" PropertyName="ReferenceSchemeError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ObjectType" Id="7CFF7155-B46F-4492-A054-8028772D7529">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ReferenceSchemeError" PropertyName="ObjectType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ReferenceSchemeError" Id="81D1EBC1-8270-4CBC-9812-31E241339118">
					<RolePlayer>
						<DomainClassMoniker Name="EntityTypeRequiresReferenceSchemeError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FrequencyConstraintHasFrequencyConstraintMinMaxError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="2E851B91-FCB9-4B3C-9276-2C2E3A1972C9">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="FrequencyConstraint" PropertyName="FrequencyConstraintMinMaxError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FrequencyConstraint" Id="80542FE3-3450-42EE-9C22-B17E868B7695">
					<RolePlayer>
						<DomainClassMoniker Name="FrequencyConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="FrequencyConstraintMinMaxError" PropertyName="FrequencyConstraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="FrequencyConstraintMinMaxError" Id="E25CA8CB-1265-4F7E-AF04-36DEC1D314E1">
					<RolePlayer>
						<DomainClassMoniker Name="FrequencyConstraintMinMaxError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FrequencyConstraintHasFrequencyConstraintExactlyOneError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="5A820704-A594-48C8-9C56-AF2567C92D91">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="FrequencyConstraint" PropertyName="FrequencyConstraintExactlyOneError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FrequencyConstraint" Id="4F690891-F7A0-4C47-B890-F0F6121EBA3F">
					<RolePlayer>
						<DomainClassMoniker Name="FrequencyConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="FrequencyConstraintExactlyOneError" PropertyName="FrequencyConstraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="FrequencyConstraintExactlyOneError" Id="05D223BC-A180-44EF-9E87-E4BB3C3F4B03">
					<RolePlayer>
						<DomainClassMoniker Name="FrequencyConstraintExactlyOneError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ObjectificationImpliesFactType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="D2706F81-78CC-493E-90C9-D54A10D33FA0">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ImpliedByObjectification" PropertyName="ImpliedFactTypeCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ImpliedByObjectification" Id="FA1A0D65-0A3C-4300-A217-EC5A23CA3AD9">
					<RolePlayer>
						<DomainRelationshipMoniker Name="Objectification"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ImpliedFactType" PropertyName="ImpliedByObjectification" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ImpliedFactType" Id="9B3FB1D7-01AF-4F66-BA48-59A2DED5BC6B">
					<RolePlayer>
						<DomainClassMoniker Name="FactType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ValueRangeHasMaxValueMismatchError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="1D2620BE-40AC-4F10-B420-5CD52687DD49">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="ValueRange" PropertyName="MaxValueMismatchError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ValueRange" Id="A5F8E444-85DA-40D0-97E2-91EB8E36A6B0">
					<RolePlayer>
						<DomainClassMoniker Name="ValueRange"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="MaxValueMismatchError" PropertyName="ValueRange" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="MaxValueMismatchError" Id="673115E6-A4C7-4CD4-B7F2-AF9D2E649ACC">
					<RolePlayer>
						<DomainClassMoniker Name="MaxValueMismatchError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ValueRangeHasMinValueMismatchError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="0E8BE672-BCBE-412B-9589-76BFA88FDE38">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="ValueRange" PropertyName="MinValueMismatchError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ValueRange" Id="CFAFDA20-C375-431D-89DF-CDBF14419773">
					<RolePlayer>
						<DomainClassMoniker Name="ValueRange"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="MinValueMismatchError" PropertyName="ValueRange" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="MinValueMismatchError" Id="C56B78D7-91B1-49E3-ACB8-291383274884">
					<RolePlayer>
						<DomainClassMoniker Name="MinValueMismatchError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FactTypeHasImpliedInternalUniquenessConstraintError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="32D5A7E1-5A80-44AB-BC2E-96A15A4D92CB">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="FactType" PropertyName="ImpliedInternalUniquenessConstraintError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="87737B64-9709-4DDE-8D77-290A2CCEED1C">
					<RolePlayer>
						<DomainClassMoniker Name="FactType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ImpliedInternalUniquenessConstraintError" PropertyName="FactType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ImpliedInternalUniquenessConstraintError" Id="67D87B16-4D08-4D96-ADFD-DED8EB84E786">
					<RolePlayer>
						<DomainClassMoniker Name="ImpliedInternalUniquenessConstraintError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SetConstraintHasTooFewRoleSequencesError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="6409DBE5-5C44-42AF-B0C6-FB1EE7E3AF2A">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="SetConstraint" PropertyName="TooFewRoleSequencesError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetConstraint" Id="1DF0F9D7-63BD-4577-8C8F-FCAB97FFF98C">
					<RolePlayer>
						<DomainClassMoniker Name="SetConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="TooFewRoleSequencesError" PropertyName="SetConstraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="TooFewRoleSequencesError" Id="504B2551-4DB1-4411-B278-DBC6A5233BF1">
					<RolePlayer>
						<DomainClassMoniker Name="TooFewRoleSequencesError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SetConstraintHasTooManyRoleSequencesError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="D54EB064-7FC6-4BCD-AF30-C73E2D586FC4">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="SetConstraint" PropertyName="TooManyRoleSequencesError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetConstraint" Id="F7EEDD61-28A3-48CC-B83D-C18ECEE2D582">
					<RolePlayer>
						<DomainClassMoniker Name="SetConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="TooManyRoleSequencesError" PropertyName="SetConstraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="TooManyRoleSequencesError" Id="CEE80FF1-0811-4B22-9CA2-B773F20BC7E1">
					<RolePlayer>
						<DomainClassMoniker Name="TooManyRoleSequencesError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FrequencyConstraintHasFrequencyConstraintInvalidatedByInternalUniquenessConstraintError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="78716CE5-DB71-4367-A912-9B622A3C480B">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="FrequencyConstraint" PropertyName="FrequencyConstraintContradictsInternalUniquenessConstraintErrorCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FrequencyConstraint" Id="4A87A658-BE43-4337-A7E1-DB66219CB52C">
					<RolePlayer>
						<DomainClassMoniker Name="FrequencyConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="FrequencyConstraintContradictsInternalUniquenessConstraintError" PropertyName="FrequencyConstraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="FrequencyConstraintContradictsInternalUniquenessConstraintError" Id="56817FD9-B572-46EA-A2B5-CBD06F09C64A">
					<RolePlayer>
						<DomainClassMoniker Name="FrequencyConstraintContradictsInternalUniquenessConstraintError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FactTypeHasFrequencyConstraintContradictsInternalUniquenessConstraintError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="57656C65-6812-4E80-AB3C-199DEB82B3EF">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="FactType" PropertyName="FrequencyConstraintContradictsInternalUniquenessConstraintErrorCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="D2203F28-3CB7-4474-892C-25EE95AB22A6">
					<RolePlayer>
						<DomainClassMoniker Name="FactType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="FrequencyConstraintContradictsInternalUniquenessConstraintError" PropertyName="FactType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="FrequencyConstraintContradictsInternalUniquenessConstraintError" Id="2B477358-D3E1-4F15-979D-C2D486BB3A1B">
					<RolePlayer>
						<DomainClassMoniker Name="FrequencyConstraintContradictsInternalUniquenessConstraintError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="RingConstraintHasRingConstraintTypeNotSpecifiedError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="62E65E16-EFA7-43D0-9759-8715D0C8B914">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="RingConstraint" PropertyName="RingConstraintTypeNotSpecifiedError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="RingConstraint" Id="4E7DEA3B-ACF3-4E71-A4F2-5C08FB8077D2">
					<RolePlayer>
						<DomainClassMoniker Name="RingConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="RingConstraintTypeNotSpecifiedError" PropertyName="RingConstraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="RingConstraintTypeNotSpecifiedError" Id="46F7EF6E-7A37-44A3-A221-26D0D99AE4BD">
					<RolePlayer>
						<DomainClassMoniker Name="RingConstraintTypeNotSpecifiedError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ValueConstraintHasDuplicateNameError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="3D69F8DE-6075-432B-8843-E8BABC677457">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="ValueConstraint" PropertyName="DuplicateNameError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ValueConstraint" Id="8D9CDE68-22D7-48FF-ABE2-1617B5D2BB92">
					<RolePlayer>
						<DomainClassMoniker Name="ValueConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="DuplicateNameError" PropertyName="ValueConstraintCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="DuplicateNameError" Id="FA64A9FE-8209-4FC7-BDA6-7DF2C734573A">
					<RolePlayer>
						<DomainClassMoniker Name="ConstraintDuplicateNameError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ObjectTypeHasDefinition" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="CC5801E2-DC99-4927-8924-F3E451F61E60">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ObjectType" PropertyName="Definition" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ObjectType" Id="0D5CA68C-3576-4E90-A998-E3745CD245B6">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Definition" PropertyName="ObjectType" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Definition" Id="3A176F2B-D5AD-485F-BEC3-BE6192FD6D65">
					<RolePlayer>
						<DomainClassMoniker Name="Definition"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FactTypeHasDefinition" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="A2B92B06-BA59-4659-905E-D1A68B5F7865">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="FactType" PropertyName="Definition" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="A28E8432-212E-4723-AEA8-5BBD203B9BC0">
					<RolePlayer>
						<DomainClassMoniker Name="FactType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Definition" PropertyName="FactType" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Definition" Id="4EEBED3E-8B32-4FD7-AA5C-009AE061EC71">
					<RolePlayer>
						<DomainClassMoniker Name="Definition"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FactTypeHasNote" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="B41C4D61-2A9F-4C91-B948-52E53A8E525F">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="FactType" PropertyName="Note" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="BEDCB56B-CAD7-45FD-B781-2A437AEF5141">
					<RolePlayer>
						<DomainClassMoniker Name="FactType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Note" PropertyName="FactType" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Note" Id="0EE3D4E9-714C-404F-AE40-5C140A335F42">
					<RolePlayer>
						<DomainClassMoniker Name="Note"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ObjectTypeHasNote" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="8357F61D-E61E-40F5-B98A-782B02A85B1A">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ObjectType" PropertyName="Note" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ObjectType" Id="A75B240C-06CC-4F48-B787-8536C58E4CD8">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Note" PropertyName="ObjectType" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Note" Id="B2BC6ECC-430A-48AE-A862-D4D876748130">
					<RolePlayer>
						<DomainClassMoniker Name="Note"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ModelHasModelNote" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="783EA177-E965-4C01-9D4A-A89C016203B6">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Model" PropertyName="NoteCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="197F0666-B426-44A9-BCA8-833FFE54135D">
					<RolePlayer>
						<DomainClassMoniker Name="ORMModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Note" PropertyName="Model" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Note" Id="A07BE562-6AC3-4C86-9612-894048C94E5D">
					<RolePlayer>
						<DomainClassMoniker Name="ModelNote"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ModelNoteReferencesModelElement" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="false" Id="57A1D17B-DB15-418A-8D82-3D44B3D1169F" AllowsDuplicates="false" InheritanceModifier="Abstract">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Note" PropertyName="ElementCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Note" Id="9B8920D2-BE41-4E4A-B39F-50394050A019">
					<RolePlayer>
						<DomainClassMoniker Name="ModelNote"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Element" PropertyName="ModelNoteCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="Element" Id="9D0C7587-F135-4619-86FA-33C80C4EB769">
					<RolePlayer>
						<DomainClassMoniker Name="/Microsoft.VisualStudio.Modeling/ModelElement"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ModelNoteReferencesFactType" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="false" Id="A6F1EB10-F929-4389-B584-38DFE11A85C2">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ModelNoteReferencesModelElement"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="Note" PropertyName="FactTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Note" Id="B2D991E2-3F04-4471-93F5-72D4E4DDF087">
					<RolePlayer>
						<DomainClassMoniker Name="ModelNote"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Element" PropertyName="ModelNoteCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Element" Id="C176868F-94E3-4F5E-A855-A33CA7D6544B">
					<RolePlayer>
						<DomainClassMoniker Name="FactType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ModelNoteReferencesObjectType" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="false" Id="CB83FD24-7819-4C34-AF59-B4E14AE3BE8F">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ModelNoteReferencesModelElement"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="Note" PropertyName="ObjectTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Note" Id="C1350ACB-4CB5-444E-8E97-76E525006233">
					<RolePlayer>
						<DomainClassMoniker Name="ModelNote"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Element" PropertyName="ModelNoteCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Element" Id="FE3BF94E-B5B1-4152-8F73-2FBC16C08BE6">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ModelNoteReferencesSetConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="false" Id="F5582A97-F2AE-45FA-A3B8-A00D62020519">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ModelNoteReferencesModelElement"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="Note" PropertyName="SetConstraintCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Note" Id="1D6BD804-5F9B-4558-97F7-CAD1E0D4F135">
					<RolePlayer>
						<DomainClassMoniker Name="ModelNote"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Element" PropertyName="ModelNoteCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Element" Id="D5ECAFC4-1E5C-471F-9E8D-54AF59FC44BB">
					<RolePlayer>
						<DomainClassMoniker Name="SetConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ModelNoteReferencesSetComparisonConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="false" Id="57A31B5C-E265-4C91-8C6C-151101258E28">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ModelNoteReferencesModelElement"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="Note" PropertyName="SetComparisonConstraintCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Note" Id="C8AC78EC-FC49-4163-807B-C6A50E905354">
					<RolePlayer>
						<DomainClassMoniker Name="ModelNote"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Element" PropertyName="ModelNoteCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Element" Id="DF805505-E31F-42E8-9C65-7004E3F3D9BD">
					<RolePlayer>
						<DomainClassMoniker Name="SetComparisonConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ObjectTypeHasCompatibleSupertypesError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="4A739F80-00FA-4F02-BD81-ED60C79DEFC3">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="ObjectType" PropertyName="CompatibleSupertypesError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ObjectType" Id="FF50F039-B38B-41A2-9D06-0AEFBE62C6A9">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="CompatibleSupertypesError" PropertyName="ObjectType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="CompatibleSupertypesError" Id="368139C3-B56F-4483-83AE-9C8E68BCA8B0">
					<RolePlayer>
						<DomainClassMoniker Name="CompatibleSupertypesError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SetConstraintHasImplicationError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="7DEA2631-58EF-46A6-B9E1-A8EDA2948AE3">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="SetConstraint" PropertyName="ImplicationError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetConstraint" Id="11E75E00-142E-4F4F-B9B1-AB5511218440">
					<RolePlayer>
						<DomainClassMoniker Name="SetConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ImplicationError" PropertyName="SetConstraint" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ImplicationError" Id="2FF88D36-6636-4E33-B11D-DB6013FB7BA2">
					<RolePlayer>
						<DomainClassMoniker Name="ImplicationError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SetComparisonConstraintHasImplicationError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="6F9E9E77-3DA6-4B01-B3AB-F46FB4C43CA8">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="SetComparisonConstraint" PropertyName="ImplicationError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetComparisonConstraint" Id="BCE096EE-2383-42C8-B52C-C543CD252264">
					<RolePlayer>
						<DomainClassMoniker Name="SetComparisonConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ImplicationError" PropertyName="SetComparisonConstraint" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ImplicationError" Id="DA7F46C4-A975-4DCB-B1DE-1FC779452865">
					<RolePlayer>
						<DomainClassMoniker Name="ImplicationError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SetComparisonConstraintHasEqualityOrSubsetImpliedByMandatoryError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="A7CA6438-CACE-4FCC-B96C-03E1DDCD3152">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="SetComparisonConstraint" PropertyName="EqualityOrSubsetImpliedByMandatoryError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetComparisonConstraint" Id="5606E453-D52C-4465-AE97-EF3D75E97245">
					<RolePlayer>
						<DomainClassMoniker Name="SetComparisonConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="EqualityOrSubsetImpliedByMandatoryError" PropertyName="EqualityOrSubsetConstraint" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" PropertyGetterAccessModifier="Private" PropertySetterAccessModifier="Private" DisplayName="EqualityOrSubsetImpliedByMandatoryError" Id="6DDF4667-3DD7-4661-9CF2-AA6E3EF782E3">
					<RolePlayer>
						<DomainClassMoniker Name="EqualityOrSubsetImpliedByMandatoryError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ObjectTypeHasPreferredIdentifierRequiresMandatoryError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="31A1BFF6-47DC-4F00-955B-1935082A3F25">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="ObjectType" PropertyName="PreferredIdentifierRequiresMandatoryError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ObjectType" Id="E7851C84-D822-4EA5-AB3F-2648538158C5">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="PreferredIdentifierRequiresMandatoryError" PropertyName="ObjectType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="PreferredIdentifierRequiresMandatoryError" Id="835D831B-67A2-4F84-890A-7D73BC5E6DBC">
					<RolePlayer>
						<DomainClassMoniker Name="PreferredIdentifierRequiresMandatoryError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ValueConstraintHasValueRangeOverlapError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="9044BE08-D88B-4BCA-B261-0841E1C73B5D">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="ValueConstraint" PropertyName="ValueRangeOverlapError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ValueConstraint" Id="6011898A-4CF2-429A-986E-F0ED8D938064">
					<RolePlayer>
						<DomainClassMoniker Name="ValueConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ValueRangeOverlapError" PropertyName="ValueConstraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ValueRangeOverlapError" Id="8D12358B-19F0-4FC5-82FB-3E512FECD499">
					<RolePlayer>
						<DomainClassMoniker Name="ValueRangeOverlapError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ValueConstraintHasValueTypeDetachedError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="706B0A12-F0E1-4048-8CEB-EEB5D7BC5CB3">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="ValueConstraint" PropertyName="ValueTypeDetachedError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ValueConstraint" Id="32F093E3-A3FD-42D4-9723-38794D103F4A">
					<RolePlayer>
						<DomainClassMoniker Name="ValueConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ValueTypeDetachedError" PropertyName="ValueConstraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ValueTypeDetachedError" Id="469A903B-45CC-4B7D-B2EF-F666C5B5D87B">
					<RolePlayer>
						<DomainClassMoniker Name="ValueConstraintValueTypeDetachedError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FactTypeHasRole" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="40F02204-F32A-4424-9FD5-5B6B943C603A">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="FactType" PropertyName="RoleCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="B40AB1F6-50F6-42D6-9928-102320DDCFF2">
					<RolePlayer>
						<DomainClassMoniker Name="FactType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Role" PropertyName="FactType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Role" Id="9199D2CB-4524-4AFB-A647-DD4DBF34CB13">
					<RolePlayer>
						<DomainClassMoniker Name="RoleBase"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ReadingOrderHasRole" Namespace="Neumont.Tools.ORM.ObjectModel" AllowsDuplicates="false" Id="F4D3824F-5764-434B-9ABD-FD847D4B7570">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ReadingOrder" PropertyName="RoleCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ReadingOrder" Id="B7E469EC-836D-447F-A0A5-155F928BCE83">
					<RolePlayer>
						<DomainClassMoniker Name="ReadingOrder"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Role" PropertyName="ReadingOrder" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="Role" Id="DAA60744-9E62-4277-A1F0-A459D38D95C1">
					<RolePlayer>
						<DomainClassMoniker Name="RoleBase"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="RoleProxyHasRole" Namespace="Neumont.Tools.ORM.ObjectModel" Id="5A3809EF-42F1-4965-8490-52FEA5DA30A2">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Proxy" PropertyName="TargetRole" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Proxy" Id="F9024EAC-69AF-438A-85AD-393B55ABF91F">
					<RolePlayer>
						<DomainClassMoniker Name="RoleProxy"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="TargetRole" PropertyName="Proxy" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="TargetRole" Id="AE004027-BE74-4E53-99D7-D3E894F4124D">
					<RolePlayer>
						<DomainClassMoniker Name="Role"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FactTypeHasDerivationExpression" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="27127A53-8E17-420F-9E87-9812F7C76CD8">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="FactType" PropertyName="DerivationRule" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="73B1A9D8-42A4-44E0-B906-AEF10E346DB6">
					<RolePlayer>
						<DomainClassMoniker Name="FactType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="DerivationRule" PropertyName="FactType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="DerivationRule" Id="F0EBCC89-95A8-45E8-9865-616A9AC858F9">
					<RolePlayer>
						<DomainClassMoniker Name="FactTypeDerivationExpression"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ObjectTypeHasObjectTypeInstance" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="F4343CDE-A3C7-402C-AF81-CBDC8F092C9E">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ObjectType" PropertyName="ObjectTypeInstanceCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ObjectType" Id="4BA85C9E-99D0-4F84-864B-B8E8C305B2C4">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ObjectTypeInstance" PropertyName="ObjectType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ObjectTypeInstance" Id="9DFF9164-DC48-46B8-A244-7B9FAB8598DB">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectTypeInstance"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="EntityTypeHasEntityTypeInstance" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="0F9CDA9D-88CE-47DD-B202-93B1455E08C3">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ObjectTypeHasObjectTypeInstance"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="EntityType" PropertyName="EntityTypeInstanceCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="EntityType" Id="5A293722-12D6-4B42-A336-3281A404E785">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="EntityTypeInstance" PropertyName="EntityType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="EntityTypeInstance" Id="DCB0CB77-842D-4B69-950D-26C3C4509B6D">
					<RolePlayer>
						<DomainClassMoniker Name="EntityTypeInstance"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ValueTypeHasValueTypeInstance" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="E01B8EC6-F3BF-4963-92DB-7E352501C04D">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ObjectTypeHasObjectTypeInstance"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="ValueType" PropertyName="ValueTypeInstanceCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ValueType" Id="9558751B-6AE9-424D-8B62-66B71F01A207">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ValueTypeInstance" PropertyName="ValueType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ValueTypeInstance" Id="17DA5F43-028E-4F9B-8E91-105DBF10AEE3">
					<RolePlayer>
						<DomainClassMoniker Name="ValueTypeInstance"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="RoleInstance" Namespace="Neumont.Tools.ORM.ObjectModel" AllowsDuplicates="true" InheritanceModifier="Abstract" Id="D3162C67-DE52-4B0D-802F-824E6ED5B74B">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Role" PropertyName="ObjectTypeInstanceCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Role" Id="C927C1AA-2E2D-41CF-9D87-0A69A63F3E99">
					<RolePlayer>
						<DomainClassMoniker Name="Role"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ObjectTypeInstance" PropertyName="RoleCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ObjectTypeInstance" Id="7E4356D2-05D5-4194-BDBC-F5B96D22E419">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectTypeInstance"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="EntityTypeRoleInstance" Namespace="Neumont.Tools.ORM.ObjectModel" AllowsDuplicates="true" Id="5DB3A2C1-C5DE-4C4A-97C2-E09CE11537D3">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="RoleInstance"/>
			</BaseRelationship>
			<!-- The Source and Target elements here should be redundant.
				 Tracking issue at http://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=164144
				 If this issue is fixed up, go back to using CreateAsRelationshipName tags in the ORMCore.SerializationExtensions.xml
				 file corresponding to this relationship (reverse changes for changeset 706). -->
			<Source>
				<DomainRole Name="Role" PropertyName="ObjectTypeInstanceCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="Role" Id="26D1BF78-FC2B-4D86-BDB3-C185FE443DAC">
					<RolePlayer>
						<DomainClassMoniker Name="Role"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ObjectTypeInstance" PropertyName="RoleCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ObjectTypeInstance" Id="B24B068F-BF3C-4D4A-9569-0305F9B5AA7E">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectTypeInstance"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FactTypeRoleInstance" Namespace="Neumont.Tools.ORM.ObjectModel" AllowsDuplicates="true" Id="FC7C9715-6886-46C2-A7A0-3BFD95CD0766">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="RoleInstance"/>
			</BaseRelationship>
			<!-- The Source and Target elements here should be redundant.
				 Tracking issue at http://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=164144
				 If this issue is fixed up, go back to using CreateAsRelationshipName tags in the ORMCore.SerializationExtensions.xml
				 file corresponding to this relationship (reverse changes for changeset 706). -->
			<Source>
				<DomainRole Name="Role" PropertyName="ObjectTypeInstanceCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="Role" Id="BDEB47FC-DD0A-4509-9269-2EA5C196F68F">
					<RolePlayer>
						<DomainClassMoniker Name="Role"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ObjectTypeInstance" PropertyName="RoleCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ObjectTypeInstance" Id="2CDE9B25-54C8-42B6-A54F-61210345E9A0">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectTypeInstance"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="EntityTypeInstanceHasRoleInstance" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="false" Id="05C64570-96FE-42C4-B9A6-F88D3BDC7C1F">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="EntityTypeInstance" PropertyName="RoleInstanceCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="EntityTypeInstance" Id="9997C315-3B87-4533-A45F-C62AFA68647C">
					<RolePlayer>
						<DomainClassMoniker Name="EntityTypeInstance"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="RoleInstance" PropertyName="EntityTypeInstance" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="RoleInstance" Id="C85848F4-9E45-44E7-AAAF-5E632CAB6D09">
					<RolePlayer>
						<DomainRelationshipMoniker Name="EntityTypeRoleInstance"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FactTypeHasFactTypeInstance" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="5283F53B-0DA8-4E4C-8A31-BDE51057E7EF">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="FactType" PropertyName="FactTypeInstanceCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="4690EAFF-667B-4AA6-93ED-9487E109C7BC">
					<RolePlayer>
						<DomainClassMoniker Name="FactType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="FactTypeInstance" PropertyName="FactType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="FactTypeInstance" Id="E2961A99-6EB7-45D5-9EAF-EC1C39FD026B">
					<RolePlayer>
						<DomainClassMoniker Name="FactTypeInstance"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FactTypeInstanceHasRoleInstance" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="false" Id="F92B6EC1-8055-4502-BD5D-763D1F5B6849">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="FactTypeInstance" PropertyName="RoleInstanceCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactTypeInstance" Id="82D54796-2A6C-48C4-8B9C-36CE1914D3BC">
					<RolePlayer>
						<DomainClassMoniker Name="FactTypeInstance"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="RoleInstance" PropertyName="FactTypeInstance" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="RoleInstance" Id="AF7B2192-02B6-49A6-A6D7-67A608124BB6">
					<RolePlayer>
						<DomainRelationshipMoniker Name="FactTypeRoleInstance"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="EntityTypeInstanceHasTooFewEntityTypeRoleInstancesError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="922E0A74-9384-4D25-9C38-E0AB709FEE8F">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="EntityTypeInstance" PropertyName="TooFewEntityTypeRoleInstancesError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="EntityTypeInstance" Id="715ED45E-2C85-491E-A577-3A6985347687">
					<RolePlayer>
						<DomainClassMoniker Name="EntityTypeInstance"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="TooFewEntityTypeRoleInstancesError" PropertyName="EntityTypeInstance" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="TooFewEntityTypeRoleInstancesError" Id="61D2D2DF-749C-4D93-8EF4-5CCD92E03154">
					<RolePlayer>
						<DomainClassMoniker Name="TooFewEntityTypeRoleInstancesError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FactTypeInstanceHasTooFewFactTypeRoleInstancesError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="6AC86DD8-1766-472E-B70F-B788C04ED688">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="FactTypeInstance" PropertyName="TooFewFactTypeRoleInstancesError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactTypeInstance" Id="AC1F89F9-1DBC-4826-8035-6EA0C1D35EB1">
					<RolePlayer>
						<DomainClassMoniker Name="FactTypeInstance"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="TooFewFactTypeRoleInstancesError" PropertyName="FactTypeInstance" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="TooFewFactTypeRoleInstancesError" Id="8D72911A-53E9-4E0D-8BE7-79FC16057ED5">
					<RolePlayer>
						<DomainClassMoniker Name="TooFewFactTypeRoleInstancesError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ValueTypeInstanceHasCompatibleValueTypeInstanceValueError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="A8AF2A8F-CDD0-41CB-B8CD-60CF28277288">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="ValueTypeInstance" PropertyName="CompatibleValueTypeInstanceValueError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ValueTypeInstance" Id="5C173594-AA0C-45BA-92F1-8D80A74E1300">
					<RolePlayer>
						<DomainClassMoniker Name="ValueTypeInstance"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="CompatibleValueTypeInstanceValueError" PropertyName="ValueTypeInstance" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="CompatibleValueTypeInstanceValueError" Id="52774478-7F8B-4974-A266-75463EA1808F">
					<RolePlayer>
						<DomainClassMoniker Name="CompatibleValueTypeInstanceValueError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="JoinHasRole" Namespace="Neumont.Tools.ORM.ObjectModel" InheritanceModifier="Abstract" Id="100DCE21-D23A-4ED5-8919-A6FA9DAA4F8B">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Join" PropertyName="Role" Multiplicity="OneMany" PropagatesDelete="true" IsPropertyGenerator="false" DisplayName="Join" Id="0D213B9C-DC79-4AE9-8E50-69AB7BB0095F">
					<RolePlayer>
						<DomainClassMoniker Name="Join"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Role" PropertyName="JoinCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Role" Id="2B0EEA3A-5901-48D8-A327-AE3DE2E0E42F">
					<RolePlayer>
						<DomainClassMoniker Name="Role"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="JoinHasInputRole" Namespace="Neumont.Tools.ORM.ObjectModel" Id="7F393586-4D6C-43F6-97D0-53E23B0A569C">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="JoinHasRole"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="Join" PropertyName="InputRole" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Join" Id="38D76A89-DDE7-4F93-9D0B-00E862DAE9C6">
					<RolePlayer>
						<DomainClassMoniker Name="Join"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Role" PropertyName="JoinCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="InputRole" Id="170F3E22-5670-4D4F-80B7-77F2532F9289">
					<RolePlayer>
						<DomainClassMoniker Name="Role"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="JoinHasOutputRole" Namespace="Neumont.Tools.ORM.ObjectModel" Id="78519CFE-11F8-45B2-89A0-7B1DCD4ABA30">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="JoinHasRole"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="Join" PropertyName="OutputRole" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Join" Id="E39A57EC-8F97-4D1A-A6E2-4AE613FECC41">
					<RolePlayer>
						<DomainClassMoniker Name="Join"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Role" PropertyName="JoinCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="OutputRole" Id="2E0CC7EF-2937-4236-B5D5-CC61167F4AE7">
					<RolePlayer>
						<DomainClassMoniker Name="Role"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ORMModelElementHasJoinPath" Namespace="Neumont.Tools.ORM.ObjectModel" InheritanceModifier="Abstract" IsEmbedding="true" Id="744C121F-CB6C-4B1F-9AC0-867EBC2F3AD7">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="JoinPathOwner" PropertyName="JoinPath" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="JoinPathOwner" Id="E8BED1B1-F17A-471E-811D-E6A9D5916CDA">
					<RolePlayer>
						<DomainClassMoniker Name="ORMModelElement"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Join" PropertyName="JoinPathOwner" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Join" Id="B5F5B7C2-C5A1-46BB-8969-264F8DC50BB7">
					<RolePlayer>
						<DomainClassMoniker Name="Join"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConstraintRoleSequenceHasJoinPath" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="62FC7AC1-EB51-4887-81D4-15007D5FACBD">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMModelElementHasJoinPath"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="JoinPathOwner" PropertyName="JoinPath" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="JoinPathOwner" Id="D204614A-A424-426B-9216-FAB21CD3BA9C">
					<RolePlayer>
						<DomainClassMoniker Name="ConstraintRoleSequence"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Join" PropertyName="JoinPathOwner" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="false" DisplayName="Join" Id="B8D5101B-7904-41B6-B14C-5BE667B2A6BA">
					<RolePlayer>
						<DomainClassMoniker Name="Join"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ConstraintRoleSequenceHasRoleHasProjectionJoin" Namespace="Neumont.Tools.ORM.ObjectModel" Id="314434D1-1E15-4E2F-A2A7-2D1D72343B06">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ConstraintRoleSequenceHasRole" PropertyName="ProjectionJoin" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ConstraintRoleSequenceHasRole" Id="20E93902-F83A-4273-BFFF-945E44D172AE">
					<RolePlayer>
						<DomainRelationshipMoniker Name="ConstraintRoleSequenceHasRole"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ProjectionJoin" PropertyName="ConstraintRoleSequenceHasRole" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ProjectionJoin" Id="98D276DB-7213-46F9-BBEE-2DC6D326C5CA">
					<RolePlayer>
						<DomainClassMoniker Name="Join"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ModelHasModelErrorDisplayFilter" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="A8E175FA-A727-4909-8944-423EF0748E3D">
			<Source>
				<DomainRole Name="Model" PropertyName="ModelErrorDisplayFilter" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ModelErrorDisplayFilter" Id="69E39A5B-B394-4270-9C43-894E4516B177">
					<RolePlayer>
						<DomainClassMoniker Name="ORMModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ModelErrorDisplayFilter" PropertyName="Model" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Model" Id="BE9E525D-C456-40D9-8420-A0745FBED25A">
					<RolePlayer>
						<DomainClassMoniker Name="ModelErrorDisplayFilter"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
	</Relationships>

	<Types>
		<ExternalType Namespace="Neumont.Tools.ORM.ObjectModel" Name="FactType"/>
		<ExternalType Namespace="Neumont.Tools.ORM.ObjectModel" Name="ReferenceMode"/>
		<ExternalType Namespace="Neumont.Tools.ORM.ObjectModel" Name="DataType"/>
		<ExternalType Namespace="Neumont.Tools.ORM.ObjectModel" Name="ModelErrorDisplayFilter"/>
		<ExternalType Namespace="Neumont.Tools.ORM.ObjectModel" Name="ObjectType"/>
		<ExternalType Namespace="Neumont.Tools.ORM.ObjectModel" Name="ReferenceModeKind"/>
		<DomainEnumeration Namespace="Neumont.Tools.ORM.ObjectModel" Name="DerivationStorageType" Description="Used to specify how/whether the contents of the fact should be stored by generated systems.">
			<Literals>
				<EnumerationLiteral Name="Derived" Value="0" Description="Fact is derived but should not be stored."/>
				<EnumerationLiteral Name="DerivedAndStored" Value="1" Description="Fact is derived and should be stored."/>
				<EnumerationLiteral Name="PartiallyDerived" Value="2" Description="Fact is partially derived and should be stored."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;DerivationStorageType, global::Neumont.Tools.ORM.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Namespace="Neumont.Tools.ORM.ObjectModel" Name="ConstraintModality">
			<Literals>
				<EnumerationLiteral Name="Alethic" Value="0" Description="The constraint must hold."/>
				<EnumerationLiteral Name="Deontic" Value="1" Description="The constraint should hold."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;ConstraintModality, global::Neumont.Tools.ORM.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Namespace="Neumont.Tools.ORM.ObjectModel" Name="RoleMultiplicity" Description="Defines the multiplicity for the roles. The role multiplicity is currently displayed only on roles associated with binary fact types and is calculated based on the existing mandatory and internal uniqueness constraints associated with the fact.">
			<Literals>
				<EnumerationLiteral Name="Unspecified" Value="0" Description="Insufficient constraints are present to determine the user intention.">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Browsable">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="Indeterminate" Value="1" Description="Too many constraints are present to determine the user intention.">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Browsable">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="ZeroToOne" Value="2" Description="0..1"/>
				<EnumerationLiteral Name="ZeroToMany" Value="3" Description="0..*"/>
				<EnumerationLiteral Name="ExactlyOne" Value="4" Description="1"/>
				<EnumerationLiteral Name="OneToMany" Value="5" Description="1..*"/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;RoleMultiplicity, global::Neumont.Tools.ORM.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Namespace="Neumont.Tools.ORM.ObjectModel" Name="RingConstraintType">
			<Literals>
				<EnumerationLiteral Name="Undefined" Value="0" Description="">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Browsable">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="PurelyReflexive" Value="1" Description=""/>
				<EnumerationLiteral Name="Irreflexive" Value="2" Description=""/>
				<EnumerationLiteral Name="Symmetric" Value="3" Description=""/>
				<EnumerationLiteral Name="Asymmetric" Value="4" Description=""/>
				<EnumerationLiteral Name="Antisymmetric" Value="5" Description=""/>
				<EnumerationLiteral Name="Intransitive" Value="6" Description=""/>
				<EnumerationLiteral Name="Acyclic" Value="7" Description=""/>
				<EnumerationLiteral Name="AcyclicIntransitive" Value="8" Description=""/>
				<EnumerationLiteral Name="AsymmetricIntransitive" Value="9" Description=""/>
				<EnumerationLiteral Name="SymmetricIntransitive" Value="10" Description=""/>
				<EnumerationLiteral Name="SymmetricIrreflexive" Value="11" Description=""/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;RingConstraintType, global::Neumont.Tools.ORM.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Namespace="Neumont.Tools.ORM.ObjectModel" Name="ReferenceModeType">
			<Literals>
				<EnumerationLiteral Name="General" Value="0" Description="That other reference mode type."/>
				<EnumerationLiteral Name="Popular" Value="1" Description="The 'in' and 'fashionable' reference mode type."/>
				<EnumerationLiteral Name="UnitBased" Value="2" Description="The reference mode type based on units."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;ReferenceModeType, global::Neumont.Tools.ORM.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Namespace="Neumont.Tools.ORM.ObjectModel" Name="RangeInclusion">
			<Literals>
				<EnumerationLiteral Name="NotSet" Value="0">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Browsable">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
				</EnumerationLiteral>
				<EnumerationLiteral Name="Open" Value="1" Description="Indicates the specific value is not included in the range."/>
				<EnumerationLiteral Name="Closed" Value="2" Description="Indicates the specific value is included in the range."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;RangeInclusion, global::Neumont.Tools.ORM.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Namespace="Neumont.Tools.ORM.ObjectModel" Name="JoinType">
			<Literals>
				<EnumerationLiteral Name="Inner" Value="0" Description="Inner join."/>
				<EnumerationLiteral Name="Outer" Value="1" Description="Outer join."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;JoinType, global::Neumont.Tools.ORM.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>


		<DomainEnumeration Namespace="Neumont.Tools.ORM.ObjectModel" Name="NameGeneratorCasingOption">
			<Literals>
				<EnumerationLiteral Name="None" Value="0"/>
				<EnumerationLiteral Name="Camel" Value="1" Description="Indicates the casing of the string is Camel"/>
				<EnumerationLiteral Name="Pascal" Value="2" Description="Indicates the casing of the string is Pascal."/>
				<EnumerationLiteral Name="Upper" Value="3" Description="Indicates the casing of the string is Upper."/>
				<EnumerationLiteral Name="Lower" Value="4" Description="Indicates the casing of the string is Lower."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;NameGeneratorCasingOption, global::Neumont.Tools.ORM.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>

		<DomainEnumeration Namespace="Neumont.Tools.ORM.ObjectModel" Name="NameGeneratorSpacingFormat">
			<Literals>
				<EnumerationLiteral Name="Retain" Value="0"/>
				<EnumerationLiteral Name="Remove" Value="1" Description="Indicates that spaces are Removed"/>
				<EnumerationLiteral Name="ReplaceWith" Value="2" Description="Indicates that spaces are ReplacedWith a different string."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;NameGeneratorSpacingFormat, global::Neumont.Tools.ORM.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>

		<DomainEnumeration Namespace="Neumont.Tools.ORM.ObjectModel" Name="NameGeneratorSubjectArea">
			<Literals>
				<EnumerationLiteral Name="None" Value="0"/>
				<EnumerationLiteral Name="Prefix" Value="1" Description="Indicates that the chosen subject will be prepended to the generated name."/>
				<EnumerationLiteral Name="Suffix" Value="2" Description="Indicates that the chosen subject will be appended to the generated name."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.Modeling.Design.EnumConverter&lt;NameGeneratorSubjectArea, global::Neumont.Tools.ORM.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>

	</Types>

	<XmlSerializationBehavior Name="ORMCoreDomainModelSerializationBehavior" Namespace="Neumont.Tools.ORM.ObjectModel"/>

</Dsl>
