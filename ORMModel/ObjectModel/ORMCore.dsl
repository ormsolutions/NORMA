<?xml version="1.0" encoding="utf-8"?>
<!--
	Natural Object-Role Modeling Architect for Visual Studio

	Copyright © Neumont University. All rights reserved.
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
	PackageGuid="EFDDC549-1646-4451-8A51-E5A5E94D647C"
	Id="3EAE649F-E654-4D04-8289-C25D2C0322D8"
	Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel"
	PackageNamespace="ORMSolutions.ORMArchitect.Core.Shell"
	Name="ORMCore"
	DisplayName="ORM Core Domain Model"
	CompanyName="ORM Solutions, LLC"
	ProductName="Natural Object-Role Modeling Architect for Visual Studio"
	MajorVersion="1" MinorVersion="0" Build="0" Revision="0">

	<Attributes>
		<ClrAttribute Name="DslModeling::ExtendsDomainModel">
			<Parameters>
				<AttributeParameter Value="&quot;F60BC3F1-C38E-4C7D-9EE5-9211DB26CB45&quot;/*ORMSolutions.ORMArchitect.Framework.FrameworkDomainModel*/"/>
			</Parameters>
		</ClrAttribute>
	</Attributes>

	<Classes>
		<DomainClass Name="NameConsumer" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="491389AA-B7DB-4461-B3CE-8064F8DE4072" DisplayName="NameConsumer" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="RecognizedPhrase" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="DF462D31-C2E4-47A5-AF48-7FFC55DE4B2A" DisplayName="RecognizedPhrase" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMNamedElement"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="NameGenerator" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="E032727F-440A-431A-82E7-2454BE939C82" DisplayName="Name Generation Defaults" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptionProvider&lt;NameGenerator, Design.NameGeneratorTypeDescriptor&lt;NameGenerator&gt;&gt;)"/>
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
				<DomainProperty Name="CasingOption" DefaultValue="None" DisplayName="CasingOption" Id="30950747-68E7-4A64-8ED7-BACEDAAFD4A2" Description="Specify upper/lower case settings of names generated for this context.">
					<Type>
						<DomainEnumerationMoniker Name="NameGeneratorCasingOption"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="SpacingFormat" DefaultValue="Retain" DisplayName="SpacingFormat" Id="CFD3E74C-DE51-4FD9-ADDB-01B0F5414BFD" Description="Specify if whitespace is preserved, removed, or replaced in names generated for this context.">
					<Type>
						<DomainEnumerationMoniker Name="NameGeneratorSpacingFormat"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="SpacingReplacement" DefaultValue="" DisplayName="SpacingReplacement" Id="0E203ACB-3611-4180-9324-7FD7D30A5AE4" Description="Specify the characters used instead of spaces in names generated for this context.">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="AutomaticallyShortenNames" DefaultValue="true" DisplayName="AutomaticallyShortenNames" Id="6A3526D9-AFB3-417E-A988-A44644AA094E" Description="Specify if names generated for this context should be automatically shortened if they are too long for the generation target.">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="UseTargetDefaultMaximum" DefaultValue="true" DisplayName="UseTargetDefaultMaximum" Id="67B83B76-394F-4702-A984-6009DC51D224" Description="Specify if the default maximum name length for this name generation context should be used when shortening names.">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="UserDefinedMaximum" DefaultValue="128" DisplayName="UserDefinedMaximum" Id="FC154AD5-AB52-4AC8-856C-28B00395ABF4" Description="Specify a custom maximum name length for this name generation context.">
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

		<DomainClass Name="NameUsage" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="5B10B7DC-2018-41D9-AEF6-E12104614CA0" DisplayName="NameUsage" InheritanceModifier="Abstract" Description="">
			<Attributes>
				<ClrAttribute Name="ORMSolutions.ORMArchitect.Core.ObjectModel.NameUsageIdentifier">
					<Parameters>
						<AttributeParameter Value="&quot;NameUsage&quot;"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainClass>

		<DomainClass Name="GenerationState" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="CD0749E6-DDB0-4890-A559-EB70D3F698E0" DisplayName="GenerationState" Description=""/>
		<DomainClass Name="GenerationSetting" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="B707A1D2-87D1-43EA-93B0-92ED9308A0A5" InheritanceModifier="Abstract" DisplayName="GenerationSetting" Description=""/>

		<DomainClass Name="ModelErrorCategory" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="C9730E21-67A1-47E1-A065-B08C2B3815CE" DisplayName="ModelErrorCategory" InheritanceModifier="Abstract" Description=""/>
		<DomainClass Name="ModelErrorDisplayFilter" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="67CDCE7B-3D28-4A92-B9EB-00418152A13F" DisplayName="ModelErrorDisplayFilter" InheritanceModifier="Sealed" Description="">
			<Properties>
				<DomainProperty Name="ExcludedCategories" DefaultValue="" DisplayName="ExcludedCategories" IsBrowsable="false" Id="46F355F4-001C-4A3F-8A0F-56BEC4EACDEB" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="IncludedErrors" DefaultValue="" DisplayName="IncludedErrors" IsBrowsable="false" Id="D83D0737-79B4-415D-9C93-73442F3C606F" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="ExcludedErrors" DefaultValue="" DisplayName="ExcludedErrors" IsBrowsable="false" Id="593D1E1A-C01D-48E8-8385-1507C7F95A25" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
		<DomainClass Name="PopulationErrorCategory" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="18C1AE31-7241-453E-9DCB-9409ACA41896" DisplayName="Sample Population Errors" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelErrorCategory"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="DataTypeAndValueErrorCategory" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="D98CE8A4-2CB0-423E-80AC-4E4E7A963EC3" DisplayName="DataType and Value Errors" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelErrorCategory"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="ConstraintImplicationAndContradictionErrorCategory" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="A8846FAF-A765-4E39-A876-CFA09A1FFB3A" DisplayName="Constraint Implication and Contradiction Errors" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelErrorCategory"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="ConstraintStructureErrorCategory" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="FEABDE83-E7B0-44C1-B6C8-3F0EF3E09589" DisplayName="Constraint Structure Errors" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelErrorCategory"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="FactTypeDefinitionErrorCategory" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="1360B437-C64F-4A30-956B-47D4F1C7E85B" DisplayName="Fact Type Definition Errors" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelErrorCategory"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="ReferenceSchemeErrorCategory" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="2F515685-36D3-4631-A6C4-572BD9644FD7" DisplayName="Reference Scheme Errors" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelErrorCategory"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="NameErrorCategory" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="57AE5888-89E0-4449-B0C8-80802DEB014C" DisplayName="Naming Errors" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelErrorCategory"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ElementGroupingSet" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="C0436CE8-6957-4FB9-A526-D94DC2073C02" DisplayName="Groups" InheritanceModifier="Sealed" Description="A Group owner, allows group containment, order, and naming enforcement."/>
		<DomainClass Name="ElementGrouping" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="B3856187-EFEB-4437-AF4C-8DF5504FB461" DisplayName="Group" Description="A group of elements. A GroupType is associated with the Group to control the group contents.">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptionProvider&lt;ElementGrouping, Design.ElementGroupingTypeDescriptor&lt;ElementGrouping&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="ORMNamedElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="DefinitionText" DefaultValue="" DisplayName="InformalDescription" Category="Group" Description="An informal description of this group.&#xd;&#xa;    To insert new lines, use Control-Enter in the dropdown editor, or open the 'ORM Informal Description Editor' tool window." Id="D1539042-2A67-413B-8B3B-12D00775BB8D" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.MultilineTextEditor&lt;global::ORMSolutions.ORMArchitect.Core.ObjectModel.Definition&gt;)"/>
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
				<DomainProperty Name="NoteText" DefaultValue="" DisplayName="Note" Category="Group" Description="A note to associate with this group.&#xd;&#xa;    To insert new lines, use Control-Enter in the dropdown editor, or open the 'ORM Notes Editor' tool window." Id="39B0228B-8884-4E4E-B595-4F058F192B50" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.MultilineTextEditor&lt;global::ORMSolutions.ORMArchitect.Core.ObjectModel.Note&gt;)"/>
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
				<DomainProperty Name="TypeCompliance" DefaultValue="NotExcluded" DisplayName="GroupTypeCompliance" Category="Group" Id="16E7B546-46CE-4A46-AED5-1437EDB5FA6C" Description="Specify the level of GroupType compliance for elements in this group.&#xd;&#xa;    Not Excluded: Allow elements not explicitly excluded by a selected GroupType.&#xd;&#xa;    Approved by Some Type: Allow elements explicitly approved by at least one GroupType.&#xd;&#xa;    Approved by All Types: Allow elements explicitly approved by all selected GroupTypes.">
					<Type>
						<DomainEnumerationMoniker Name="GroupingMembershipTypeCompliance"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="Priority" DefaultValue="0" DisplayName="GroupPriority" Category="Group" Id="C290CB24-0F2C-4E67-A561-FCD25DDA53E8" Description="Specify a priority relative to other Groups. If an element is included in two groups of the same type, the settings for the Group with the highest GroupPriority are given precedence.">
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
		<DomainClass Name="ElementGroupingType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="0F0515DF-287F-44A2-9EC1-74DBDBA87146" DisplayName="GroupType" InheritanceModifier="Abstract" Description="A type for a group. Each Group is associated with a new instance of each of its GroupTypes, allowing individual settings per group.">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptionProvider&lt;ElementGroupingType, Design.ElementGroupingTypeTypeDescriptor&lt;ElementGroupingType&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainClass>
		<DomainClass Name="ElementGroupingErrorCategory" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="7DA10A75-7D12-41D9-8D11-38675314C654" DisplayName="Grouping Errors" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelErrorCategory"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="ElementGroupingDuplicateNameError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="374625CA-858D-42B7-A9E4-1B33BAE89EFF" DisplayName="Duplicate Group Names" Description="">
			<BaseClass>
				<DomainClassMoniker Name="DuplicateNameError"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="ElementGroupingMembershipContradictionError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="DB58CEA7-1371-4433-B455-2AC77DEE27AD" DisplayName="Element Required and Blocked in one Group" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ORMModelElement" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="BFBBEE5E-C691-4299-B958-77AC1B701F28" DisplayName="ORMModelElement" InheritanceModifier="Abstract" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptionProvider&lt;ORMModelElement, Design.ORMModelElementTypeDescriptor&lt;ORMModelElement&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainClass>

		<DomainClass Name="ORMNamedElement" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="C2BE18BA-BC16-4764-BAA1-18E721435BCE" DisplayName="ORMNamedElement" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Name" DefaultValue="" DisplayName="Name" IsElementName="true" Id="4A557C1E-0A89-49B7-B4BD-FA095F6267D7" Description="A name for this element.">
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

		<DomainClass Name="ORMModel" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="73E1F528-9E60-4198-AAC2-F8D6CCF62EB3" DisplayName="ORMModel" Description="" GeneratesDoubleDerived="true">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptionProvider&lt;ORMModel, Design.ORMModelTypeDescriptor&lt;ORMModel&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="ORMNamedElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="ModelErrorDisplayFilterDisplay" DefaultValue="" DisplayName="ErrorDisplay" Id="C5A66492-0FFA-46F9-A64B-361E62D696B0" Kind="CustomStorage" Description="Validation error display options for this model. Control error display by category and individually.">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Core.ObjectModel.Design.ModelErrorDisplayFilterEditor)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/ORMSolutions.ORMArchitect.Core.ObjectModel/ModelErrorDisplayFilter"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="DefinitionText" DefaultValue="" DisplayName="InformalDescription" Description="An informal description of this Model.&#xd;&#xa;    To insert new lines, use Control-Enter in the dropdown editor, or open the 'ORM Informal Description Editor' tool window." Id="E86A38C9-2F8E-4066-8114-384184C5E3C3" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.MultilineTextEditor&lt;global::ORMSolutions.ORMArchitect.Core.ObjectModel.Definition&gt;)"/>
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
				<DomainProperty Name="NoteText" DefaultValue="" DisplayName="Note" Description="A note to associate with this Model.&#xd;&#xa;    To insert new lines, use Control-Enter in the dropdown editor, or open the 'ORM Notes Editor' tool window." Id="603A1F7B-06A4-4B85-9B0D-E3A85629FF98" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.MultilineTextEditor&lt;global::ORMSolutions.ORMArchitect.Core.ObjectModel.Note&gt;)"/>
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


		<DomainClass Name="ObjectType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="2FED415E-1786-4FBF-8556-A507F2F765FD" DisplayName="ObjectType" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptionProvider&lt;ObjectType, Design.ObjectTypeTypeDescriptor&lt;ObjectType&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="ORMNamedElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="IsExternal" DefaultValue="false" DisplayName="IsExternal" Id="D03828FD-1DA7-4804-A16B-CC27F2046F57" Description="Is this ObjectType defined in an external model?">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="DefinitionText" DefaultValue="" DisplayName="InformalDescription" Description="An informal description of this ObjectType.&#xd;&#xa;    To insert new lines, use Control-Enter in the dropdown editor, or open the 'ORM Informal Description Editor' tool window." Id="431A8A8F-E8EC-4014-B1A1-843E55751A55" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.MultilineTextEditor&lt;global::ORMSolutions.ORMArchitect.Core.ObjectModel.Definition&gt;)"/>
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
				<DomainProperty Name="NoteText" DefaultValue="" DisplayName="Note" Description="A note to associate with this ObjectType.&#xd;&#xa;    To insert new lines, use Control-Enter in the dropdown editor, or open the 'ORM Notes Editor' tool window." Id="17C4E23D-CA49-4329-982F-48F4EFCA23BD" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.MultilineTextEditor&lt;global::ORMSolutions.ORMArchitect.Core.ObjectModel.Note&gt;)"/>
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
				<DomainProperty Name="IsIndependent" DefaultValue="false" DisplayName="IsIndependent" Id="D52257EF-D76A-404D-AAC5-7450BA5CC790" Description="Can an instance of this ObjectType exist if that instance plays no roles?">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="IsValueType" DefaultValue="false" DisplayName="IsValueType" Id="F63ACB94-8526-432E-964C-3B4441195754" Kind="CustomStorage" Description="Is this ObjectType a self-identifying value or an entity?">
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
				<DomainProperty Name="DataTypeScale" DefaultValue="0" DisplayName="DataTypeScale" Id="BD2D708A-7687-4218-94BC-05834AFAC869" Kind="CustomStorage"  Description="The number of digits allowed to the right of the decimal point in a value with this DataType.">
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="DataTypeLength" DefaultValue="0" DisplayName="DataTypeLength" Id="C9B01797-2CA1-4FF8-865A-FDA0DDF33F8D" Kind="CustomStorage" Description="The maximum length of values with this DataType.">
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="ReferenceModeDisplay" DefaultValue="" DisplayName="RefMode" Id="2E56D25A-BD96-4478-A55C-9F17A15C94B6" Kind="CustomStorage" Description="The reference mode pattern for the EntityType.&#xd;&#xa;    If the desired reference mode pattern is not specified in the dropdown, then a new pattern can be entered. The type of a new reference mode pattern is set by prepending a '.' for a popular reference mode, appending a ':' for a unit-based reference mode, and applying no decorators for a general reference mode. Reference mode settings can also be managed with the 'ORM Reference Mode Editor' tool window.">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Core.ObjectModel.Design.ReferenceModePicker)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Core.ObjectModel.Design.ReferenceModeConverter)"/>
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
						<ExternalTypeMoniker Name="/ORMSolutions.ORMArchitect.Core.ObjectModel/ReferenceMode"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="DataTypeDisplay" DefaultValue="" DisplayName="DataType" Id="3E8893A7-5985-4200-A595-CB1E9EC9ADA7" Kind="CustomStorage" Description="The DataType for this ValueType, or the DataType for the identifying ValueType if this is an EntityType.&#xd;&#xa;    This is a portable DataType. The final physical DataType is dependent on the generation target.">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Core.ObjectModel.Design.DataTypePicker)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/ORMSolutions.ORMArchitect.Core.ObjectModel/DataType"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="ValueRangeText" DefaultValue="" DisplayName="ValueRange" Id="F0662C59-700B-435C-B57B-93E5FD84B71F" Kind="CustomStorage" Description="Restrict the range of possible values for instances of this ObjectType.&#xd;&#xa;    To specify a range, use '..' between the range endpoints, square brackets to specify a closed endpoint, and parentheses to specify an open endpoint. Commas are used to entered multiple ranges or discrete values.&#xd;&#xa;    Example: {[10..20), 30} specifies all values between 10 and 20 (but not including 20) and the value 30.">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="ValueTypeValueRangeText" DefaultValue="" DisplayName="ValueTypeValueRange" Id="6EBE45BB-1054-4785-8C9D-905A41599EF9" Kind="CustomStorage" Description="The ValueRange property for the ValueType that identifies this EntityType.&#xd;&#xa;    The ValueRange property of an EntityType is applied to the identifying role, not directly to the identifying ValueType. This allows EntityType ValueRanges to be specified independently for multiple EntityTypes identified with the same unit-based or general reference mode patterns.">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="IsPersonal" DefaultValue="false" DisplayName="IsPersonal" Id="EF9AE461-4327-46DC-8FE0-D1388F061B30" Description="Does this ObjectType represent a person instead of a thing?&#xd;&#xa;    Used as a verbalization directive to render references to this type using a personal pronoun ('who' instead of 'that').">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="IsImplicitBooleanValue" DefaultValue="false" DisplayName="IsImplicitBooleanValue" Id="AE665FCF-B90A-41BD-B3E9-8611B42E668C" IsBrowsable="false">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="DerivationExpressionDisplay" DefaultValue="" DisplayName="DerivationRule" Id="B852BC09-7887-4BA7-A7AA-09D4F4E2AAD2" Kind="CustomStorage" Description="The derivation rule for this subtype. If a rule is not specified, then this is treated as an asserted subtype.">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Core.ObjectModel.Design.DerivationRuleEditor)"/>
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

		<DomainClass Name="FactType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="723A2B28-6CDA-4185-B597-87866E257265" DisplayName="FactType" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptionProvider&lt;FactType, Design.FactTypeTypeDescriptor&lt;FactType&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="IsExternal" DefaultValue="false" DisplayName="IsExternal" Id="67EA8C95-FD9A-473B-8AA2-E35FCDD68361" Description="Is this FactType defined in an external model?">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="DefinitionText" DefaultValue="" DisplayName="InformalDescription" Description="An informal description of this FactType.&#xd;&#xa;    To insert new lines, use Control-Enter in the dropdown editor, or open the 'ORM Informal Description Editor' tool window." Id="3F58E4D1-4562-478A-A3FE-08715E455CD8" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.MultilineTextEditor&lt;global::ORMSolutions.ORMArchitect.Core.ObjectModel.Definition&gt;)"/>
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
				<DomainProperty Name="NoteText" DefaultValue="" DisplayName="Note" Description="A note to associate with this FactType.&#xd;&#xa;    To insert new lines, use Control-Enter in the dropdown editor, or open the 'ORM Notes Editor' tool window." Id="AF6200B1-068D-434A-98D3-44E872B921BD" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.MultilineTextEditor&lt;global::ORMSolutions.ORMArchitect.Core.ObjectModel.Note&gt;)"/>
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
				<DomainProperty Name="Name" DefaultValue="" DisplayName="Name" IsElementName="true" Id="B17F5E42-A0FA-4B88-9D24-D148CEEE7DB0" Kind="CustomStorage" Description="The name for this FactType.&#xd;&#xa;    If the Name property is read-only, then it is a generated name based on primary reading.&#xd;&#xa;    If the Name property is editable, then it is the name of an explicit or implicit objectifying EntityType. The editable name can be reset to match the generated name by clearing the property value.">
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
				<DomainProperty Name="DerivationExpressionDisplay" DefaultValue="" DisplayName="DerivationRule" Id="7AF5C436-C28A-49BA-B8E0-05C409B67358" Kind="CustomStorage" Description="A derivation rule for this FactType.">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Core.ObjectModel.Design.DerivationRuleEditor)"/>
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
				<DomainProperty Name="DerivationStorageDisplay" DefaultValue="" DisplayName="DerivationStorage" Id="307C9629-ACE8-43E1-ABF3-33E8BB7146B7" Kind="CustomStorage" Description="Storage options for a derived FactType.">
					<Type>
						<DomainEnumerationMoniker Name="DerivationExpressionStorageType"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="SubtypeFact" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="7A957450-AD7E-4C29-AF59-A10F8C8052CC" DisplayName="SubtypeRelationship">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptionProvider&lt;SubtypeFact, Design.SubtypeFactTypeDescriptor&lt;SubtypeFact&gt;&gt;)"/>
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

		<DomainClass Name="RoleBase" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="62293718-2F14-4A4C-88EB-0BA3AA6B7B91" DisplayName="RoleBase" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="RoleProxy" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="FF552152-BD43-4731-8EDA-675E68D6C5DB" DisplayName="RoleProxy" Description="">
			<BaseClass>
				<DomainClassMoniker Name="RoleBase"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="Role" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="291FEB71-371A-4B23-9DDC-61154A10A3D7" DisplayName="Role" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptionProvider&lt;Role, Design.RoleTypeDescriptor&lt;Role&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="RoleBase"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="RolePlayerDisplay" DefaultValue="" DisplayName="RolePlayer" Id="B66FCA99-E6EC-46C9-B445-D549F6D7ABE1" Kind="CustomStorage" Description="The ObjectType that plays this Role.">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Core.ObjectModel.Design.RolePlayerPicker)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/ORMSolutions.ORMArchitect.Core.ObjectModel/ObjectType"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="IsMandatory" DefaultValue="false" DisplayName="IsMandatory" Id="0F5EED7E-7584-413A-9250-BD4624DC164E" Kind="CustomStorage" Description="Does this Role have a simple mandatory constraint?">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="Multiplicity" DefaultValue="Unspecified" DisplayName="Multiplicity" Id="ADA46024-61B8-4E1D-BB28-2FF2C71B83CD" Kind="CustomStorage" Description="The multiplicity specification for a Role of a binary FactType. Affects the uniqueness and mandatory constraints on the opposite Role.">
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
				<DomainProperty Name="ValueRangeText" DefaultValue="" DisplayName="ValueRange" Id="3882C0AC-6F4A-4CF1-B856-E57A2DD4650C" Kind="CustomStorage" Description="Restrict the range of possible values for instances of the RolePlayer ObjectType.&#xd;&#xa;    To specify a range, use '..' between the range endpoints, square brackets to specify a closed endpoint, and parentheses to specify an open endpoint. Commas are used to entered multiple ranges or discrete values.&#xd;&#xa;    Example: {[10..20), 30} specifies all values between 10 and 20 (but not including 20) and the value 30.">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="MandatoryConstraintName" DefaultValue="" DisplayName="MandatoryConstraintName" Id="A6680C0F-84B1-499C-8B58-1E1C5D09570C" Kind="CustomStorage" Description="The Name of the simple mandatory constraint on this Role.">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="MandatoryConstraintModality" DefaultValue="" DisplayName="MandatoryConstraintModality" Id="29B14765-434B-4CCF-9C93-BEE8BB7E2697" Kind="CustomStorage" Description="The Modality of the simple mandatory constraint on this Role.&#xd;&#xa;    Alethic modality means the constraint is structurally enforced and data violating the constraint cannot be entered in the system;&#xd;&#xa;    Deontic modality means that data violating the constraint can be recorded.">
					<Type>
						<DomainEnumerationMoniker Name="ConstraintModality"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="Name" DefaultValue="" DisplayName="Name" Id="F173D0FA-8F94-479D-8794-2572B8CD8D9A" Description="The explicit Name for this role.">
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
				<DomainProperty Name="ObjectificationOppositeRoleName" DefaultValue="" DisplayName="ImpliedRoleName" Id="4719AAC4-E0E7-467A-B261-CDB8AE9826ED" Kind="CustomStorage" Description="The Name of the implied Role attached to the objectifying EntityType.&#xd;&#xa;    An implied binary FactType is created relating the objectifying EntityType to each of the role players of an objectified FactType. Binary FactTypes with a spanning internal uniqueness constraint and ternary (or higher arity) FactTypes are automatically objectified.">
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

		<DomainClass Name="ObjectifiedUnaryRole" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="EE1EFD02-7AD8-42FA-9062-1F499941617F" DisplayName="Role" Description="">
			<BaseClass>
				<DomainClassMoniker Name="Role"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="NameAlias" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="A0AD1270-E3D1-4851-A5AB-D87E5942F9AE" DisplayName="NameAlias" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMNamedElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="NameConsumer" DisplayName="NameConsumer" IsBrowsable="false" Id="BE9EDEB2-C60E-4446-BAC0-73CCD61716EA" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="NameUsage" DisplayName="NameUsage" IsBrowsable="false" Id="18DBB768-B471-4926-B678-5B2245760333" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="EqualityConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="E4F8E935-C07C-4269-81E3-978110F6DC68" DisplayName="EqualityConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SetComparisonConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ExclusionConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="7766C350-ADFC-464C-B200-E4473F551E03" DisplayName="ExclusionConstraint" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptionProvider&lt;ExclusionConstraint, Design.ExclusionConstraintTypeDescriptor&lt;ExclusionConstraint&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="SetComparisonConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="SubsetConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="9B5982E3-A852-4071-A973-9719F87546F0" DisplayName="SubsetConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SetComparisonConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="SetComparisonConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="85074B82-ED14-4D70-B95C-0B29F2D64210" DisplayName="SetComparisonConstraint" InheritanceModifier="Abstract" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptionProvider&lt;SetComparisonConstraint, Design.SetComparisonConstraintTypeDescriptor&lt;SetComparisonConstraint&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="ORMNamedElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Modality" DefaultValue="Alethic" DisplayName="Modality" Id="C0AEF802-D9E9-4938-B44B-DE9A6A530D9B" Description="The Modality of the simple mandatory constraint on this Role.&#xd;&#xa;    Alethic modality means the constraint is structurally enforced and data violating the constraint cannot be entered in the system;&#xd;&#xa;    Deontic modality means that data violating the constraint can be recorded.">
					<Type>
						<DomainEnumerationMoniker Name="ConstraintModality"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="DefinitionText" DefaultValue="" DisplayName="InformalDescription" Description="An informal description of this constraint.&#xd;&#xa;    To insert new lines, use Control-Enter in the dropdown editor, or open the 'ORM Informal Description Editor' tool window." Id="57B17D7F-C707-4CEB-8B95-36A19648C059" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.MultilineTextEditor&lt;global::ORMSolutions.ORMArchitect.Core.ObjectModel.Definition&gt;)"/>
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
				<DomainProperty Name="NoteText" DefaultValue="" DisplayName="Note" Description="A note to associate with this constraint.&#xd;&#xa;    To insert new lines, use Control-Enter in the dropdown editor, or open the 'ORM Notes Editor' tool window." Id="00A8BEC9-8FDB-4961-B345-3A7C8FD18CFA" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.MultilineTextEditor&lt;global::ORMSolutions.ORMArchitect.Core.ObjectModel.Note&gt;)"/>
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

		<DomainClass Name="Expression" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="1B62AF68-86A9-4A14-8B32-8988041BBCCF" DisplayName="Expression" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Body" DefaultValue="" DisplayName="Body" IsBrowsable="false" Id="9760D258-0126-4749-A370-D7CC5A04F138">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="Language" DefaultValue="" DisplayName="Language" IsBrowsable="false" Id="53D116FA-E39C-47C5-A4D6-41E42786EEDB">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="SetComparisonConstraintRoleSequence" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="9E59F946-8745-4936-A4AA-74552664790E" DisplayName="SetComparisonConstraintRoleSequence" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ConstraintRoleSequence"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="RingConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="31792DBE-49EB-4544-9FB4-3A692AAC39C9" DisplayName="RingConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SetConstraint"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="RingType" DefaultValue="Undefined" DisplayName="RingType" Id="54D182E1-6650-4393-8BD8-9D9E42BB8CE7" Description="Restriction type of this Ring constraint.">
					<Type>
						<DomainEnumerationMoniker Name="RingConstraintType"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="FrequencyConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="A6D76D01-FDC3-43A2-8AAF-56C2E0BD0465" DisplayName="FrequencyConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SetConstraint"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="MinFrequency" DefaultValue="1" DisplayName="MinFrequency" Id="2D48D3CA-564D-459E-A701-4209A12C4783" Description="The minimum number of occurrences for each instance that plays the restricted roles.">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Core.ObjectModel.Design.FrequencyConstraintMinConverter)"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="MaxFrequency" DefaultValue="2" DisplayName="MaxFrequency" Id="F46D9200-3602-435C-B852-C53BE10D99C6" Description="The maximum number of occurrences for each instance that plays the restricted roles.">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Core.ObjectModel.Design.FrequencyConstraintMaxPicker)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Core.ObjectModel.Design.FrequencyConstraintMaxConverter)"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="UniquenessConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="49C7E3CE-C4F9-417D-B49C-27EA4016371E" DisplayName="UniquenessConstraint" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptionProvider&lt;UniquenessConstraint, Design.UniquenessConstraintTypeDescriptor&lt;UniquenessConstraint&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="SetConstraint"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="IsPreferred" DefaultValue="false" DisplayName="IsPreferredIdentifier" Id="585DE7A0-8E09-43F3-8463-F20609A16790" Kind="CustomStorage" Description="Is this the preferred identifier for the EntityType role player of the opposite role(s)?&#xd;&#xa;    The opposite role player of an internal constraint on an objectified FactType is the objectifying EntityType. Binary FactTypes with a spanning internal uniqueness constraint and ternary (or higher arity) FactTypes are automatically objectified.">
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

		<DomainClass Name="MandatoryConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="F054BE4D-BFCA-4CD3-A0D8-97F61C165753" DisplayName="InclusiveOrConstraint" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptionProvider&lt;MandatoryConstraint, Design.MandatoryConstraintTypeDescriptor&lt;MandatoryConstraint&gt;&gt;)"/>
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

		<DomainClass Name="SetConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="1B85E4BE-0C95-45BD-A76F-2087456F891B" DisplayName="SetConstraint" InheritanceModifier="Abstract" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptionProvider&lt;SetConstraint, Design.SetConstraintTypeDescriptor&lt;SetConstraint&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="ConstraintRoleSequence"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Modality" DefaultValue="Alethic" DisplayName="Modality" Id="B4F1902A-7EB9-464F-A0F8-F816658C1BD8" Description="The Modality of the simple mandatory constraint on this Role.&#xd;&#xa;    Alethic modality means the constraint is structurally enforced and data violating the constraint cannot be entered in the system;&#xd;&#xa;    Deontic modality means that data violating the constraint can be recorded.">
					<Type>
						<DomainEnumerationMoniker Name="ConstraintModality"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="DefinitionText" DefaultValue="" DisplayName="InformalDescription" Description="An informal description of this constraint.&#xd;&#xa;    To insert new lines, use Control-Enter in the dropdown editor, or open the 'ORM Informal Description Editor' tool window." Id="27353884-FF01-4DA8-9C95-A4186E1B569F" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.MultilineTextEditor&lt;global::ORMSolutions.ORMArchitect.Core.ObjectModel.Definition&gt;)"/>
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
				<DomainProperty Name="NoteText" DefaultValue="" DisplayName="Note" Description="A note to associate with this constraint.&#xd;&#xa;    To insert new lines, use Control-Enter in the dropdown editor, or open the 'ORM Notes Editor' tool window." Id="63E2F8FC-6003-47F0-AF9A-AC539907CF11" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.MultilineTextEditor&lt;global::ORMSolutions.ORMArchitect.Core.ObjectModel.Note&gt;)"/>
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

		<DomainClass Name="ConstraintRoleSequence" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="E279C66B-E89C-4E02-9DE2-64791C8A4511" DisplayName="ConstraintRoleSequence" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMNamedElement"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="RolePathOwner" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="444D962D-BAE2-4278-A8A2-40A4605CF5AB" DisplayName="RolePathOwner" InheritanceModifier="Abstract" Description="An abstract owner for one or more path objects.">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="RolePathComponent" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="73315B01-5A3B-40C7-9E0E-A600D72BE751" DisplayName="RolePathComponent" InheritanceModifier="Abstract" Description="Represents an abstract path component that can be recursively combined.">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="RolePathCombination" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="EC942EB6-27BF-4ACF-9237-793E46BC31F6" DisplayName="RolePathCombination" Description="Represents one or more role paths with combined results and additional restrictions.">
			<BaseClass>
				<DomainClassMoniker Name="RolePathComponent"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="RolePathCombinationCorrelation" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="CECB3722-9EF9-4B7A-B5FD-A63C0F34070A" Description="Represents two or more correlated pathed roles from different combined role paths.">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="RolePath" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="37599E0F-212C-4290-88A6-7406B8EF7E57" DisplayName="RolePath" InheritanceModifier="Abstract" Description="An ordered sequence of roles through ORM space with a tail split branching into other subpaths">
			<BaseClass>
				<!-- Note that we make RolePath a RolePathComponent so that we can use a single-inheritance chain to
				make a LeadRolePath both a component and a RolePath. A RoleSubPath is never used as a path component. -->
				<DomainClassMoniker Name="RolePathComponent"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="SplitIsNegated" DefaultValue="False" DisplayName="SplitIsNegated" Id="2670BB76-4732-4C0D-916B-3F07F54F71C7" Description="Indicates if the tail split in its entirety should be treated as a negation.">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="SplitCombinationOperator" DefaultValue="And" DisplayName="SplitCombinationOperator" Id="2E4570B4-163C-4ADD-959D-246CC454409D" Description="Determines the logical operator used to combine split paths.">
					<Type>
						<DomainEnumerationMoniker Name="LogicalCombinationOperator"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
		<DomainClass Name="LeadRolePath" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="404B14CD-3F82-44F3-8DDD-E1E729EBB9FB" DisplayName="LeadRolePath" Description="A top level role path starting at a root object type. Provides a context for subpaths, functions, and constraints specific to this path.">
			<BaseClass>
				<DomainClassMoniker Name="RolePath"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="RoleSubPath" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="AA7B4894-C286-45C3-A95F-09D04E286038" DisplayName="RoleSubPath" Description="A branched path split from the end of another path.">
			<BaseClass>
				<DomainClassMoniker Name="RolePath"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="CalculatedPathValue" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="C4C327B6-F1DE-4F02-AF04-3EA358ED0684" DisplayName="CalculatedValue" Description="A calculated value used in a role path.">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="PathConstant" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="B8F65CC9-2BDE-4688-9671-9F4789A3828A" DisplayName="A constant value used directly in a path.">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="LexicalValue" Id="4E5C5B55-B001-44C6-B057-D5DA26B63246" DisplayName="Value" Description="A lexical constant value interpreted based on context.">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
		<DomainClass Name="Function" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="921037FF-D266-4F31-9256-9A6D4F410109" DisplayName="Function" Description="A function or operator used to represented a calculation algorithm.">
			<BaseClass>
				<DomainClassMoniker Name="ORMNamedElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="IsBoolean" Id="363BE96E-1BD8-46B1-B54B-41DFE6B9D4CF" DisplayName="IsBoolean" DefaultValue="false" Description="Set if this function returns a boolean value that can be evaluated directly as a condition.">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="OperatorSymbol" Id="ECE5DDEC-7AB3-4B65-BDE9-DDD836E92288" DisplayName="OperatorSymbol" DefaultValue="" Description="A string indicating this function should be displayed as an operator instead of using functional notation. Represents infix notation for a binary operator and prefix notation for a unary.">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
		<DomainClass Name="FunctionParameter" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="1C650E60-597E-4B84-A007-ACD640654354" DisplayName="FunctionParameter" Description="A formal function parameter describing expected input to a function.">
			<BaseClass>
				<DomainClassMoniker Name="ORMNamedElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="BagInput" Id="42ACD532-5E46-4200-AC29-C20E926DC73F" DisplayName="BagInput" DefaultValue="false" Description="Set if a bag input is expected for this parameter, such as with an aggregation function.">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
		<DomainClass Name="CalculatedPathValueInput" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="79E5EBEF-2FFA-49E6-8A22-CC90B68A6941" DisplayName="CalculatedPathValueInput" Description="An input value or bag passed to a function parameter calculate a value.">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="DistinctValues" DefaultValue="false" DisplayName="DistinctValues" IsBrowsable="true" Id="8F1B2264-B859-48C2-BCDC-3EE964729C77" Description="Should the bag be limited to distinct values, resulting in a set of values instead of a bag of values?">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
		<DomainClass Name="FactTypeDerivationRule" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="DEDADFCE-C351-4FCB-A455-B19FB91875B8" DisplayName="FactTypeDerivationRule" Description="A role path defining a fact type derivation.">
			<BaseClass>
				<DomainClassMoniker Name="RolePathOwner"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="DerivationCompleteness" DefaultValue="FullyDerived" DisplayName="DerivationCompleteness" IsBrowsable="false" Id="F254F0A7-E37E-4FDA-AC96-DEEEB8828FEC">
					<Type>
						<DomainEnumerationMoniker Name="DerivationCompleteness"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="DerivationStorage" DefaultValue="NotStored" DisplayName="DerivationStorage" IsBrowsable="false" Id="5F83F8C7-D15D-4985-9CCC-099B354BD178">
					<Type>
						<DomainEnumerationMoniker Name="DerivationStorage"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="SetProjection" DefaultValue="false" DisplayName="DistinctFacts" Id="CB6A01D4-8E6C-4320-AFD3-492A9D473B17" Description="The derivation rule results in a set of distinct facts instead of a bag that might contain duplicates.">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>
		<DomainClass Name="SubtypeDerivationRule" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="7B27FBFE-0A5E-447C-89B7-1BA25F9ED880" DisplayName="SubtypeDerivationRule" Description="A role path defining subtype population.">
			<BaseClass>
				<DomainClassMoniker Name="RolePathOwner"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="ConstraintRoleSequenceJoinPath" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="56690364-4793-49F3-94C2-2984ED932D84" DisplayName="ConstraintRoleSequenceJoinPath" Description="A role path defining cross fact type relationships within a constraint role sequence.">
			<BaseClass>
				<DomainClassMoniker Name="RolePathOwner"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="IsAutomatic" Id="E6D3DFAD-F849-4D4E-AA81-46FABEBA7409" DisplayName="AutomaticJoinPath" Description="The join path is automatically created from the constraint sequence.">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="TooFewRoleSequencesError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="686A4B07-0ED9-4143-8225-5524C4D6C001" DisplayName="Too Few Role Sequences" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TooManyRoleSequencesError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="1ADACF12-94F5-430D-9E14-6A3B0334139E" DisplayName="Too Many Role Sequences" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ObjectTypeDuplicateNameError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="798D4CC7-1AD8-4A83-AFD5-5730AC342DC2" DisplayName="Duplicate ObjectType Names" Description="">
			<BaseClass>
				<DomainClassMoniker Name="DuplicateNameError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="RecognizedPhraseDuplicateNameError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="E4F41332-9A69-4CE8-871C-3507326D7CDB" DisplayName="Duplicate Recognized Phrases" Description="">
			<BaseClass>
				<DomainClassMoniker Name="DuplicateNameError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="FunctionDuplicateNameError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="B642B6C4-9C15-44C3-92EC-1B39B91619E4" DisplayName="Duplicate Function Names" Description="">
			<BaseClass>
				<DomainClassMoniker Name="DuplicateNameError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="PopulationUniquenessError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="BA0A8F9E-91E1-4D56-8A44-9F49432C63C5" DisplayName="Population Violates Uniqueness Constraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ContradictionError" InheritanceModifier="Abstract" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="B42B88E4-CA87-4DFA-90BE-00606E4BE23B" DisplayName="ContradictionError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ExclusionContradictsMandatoryError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="5A57EA68-918D-4AE3-AF7F-D9F7CDB5AB34" DisplayName="Contradicting Exclusion and Mandatory Constraints" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ContradictionError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ExclusionContradictsEqualityError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="D8714F74-60B7-48F5-BF7D-88D8736CB22A" DisplayName="Contradicting Exclusion and Equality Constraints" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ContradictionError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ExclusionContradictsSubsetError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="F671FE6D-BA8A-4BF2-AFB6-BE5827996C50" DisplayName="Contradicting Exclusion and Subset Constraints" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ContradictionError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="NotWellModeledSubsetAndMandatoryError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="2DECDC39-E109-4D59-8BF3-046E2CD8584C" DisplayName="Contradicting Subset and Mandatory Constraints" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="PopulationMandatoryError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="5B76CB18-90B2-4656-BB0D-0788460FDB70" DisplayName="Missing Mandatory Sample Population" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ObjectifiedInstanceRequiredError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="0B96D8AA-7EB3-4B6E-B45E-F94E8C63064A" DisplayName="Missing Objectified FactType Instance" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ObjectifyingInstanceRequiredError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="23808265-C2BE-4E03-B555-A4DB84CF053C" DisplayName="Missing Objectifying EntityType Instance" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ConstraintDuplicateNameError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="AA63E81B-6978-49A2-A4AC-86022A172EDD" DisplayName="Duplicate Constraint Names" Description="">
			<BaseClass>
				<DomainClassMoniker Name="DuplicateNameError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="DuplicateNameError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="9E29C624-4559-4020-9163-7B5846C94C0C" DisplayName="DuplicateNameError" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TooFewReadingRolesError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="1D2B23EF-456E-4E80-91D8-FB384F779A54" DisplayName="FactType has Fewer Roles than Reading Text" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TooManyReadingRolesError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="50C98172-412C-40C0-ADD3-82809C3D82F7" DisplayName="FactType has More Roles than Reading Text" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ExternalConstraintRoleSequenceArityMismatchError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="3DA5385A-D9DE-4F3D-9D2E-CA79F10AB542" DisplayName="Constraint Role Sequences with Different Numbers of Roles" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="FactTypeRequiresReadingError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="3ECA7E92-45B2-45BD-BAD3-6AF0C4B40E70" DisplayName="FactType Requires Reading" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="FactTypeRequiresInternalUniquenessConstraintError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="295D4B3D-1351-431D-B72F-28661D744B58" DisplayName="FactType Requires Internal Uniqueness Constraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="DataTypeNotSpecifiedError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="8AFA102F-529C-4896-AEB3-9D714E28FC61" DisplayName="DataType not Specified for ValueType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="NMinusOneError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="497754B3-5176-4712-BC46-2E4377354C8B" DisplayName="Insufficient Roles for Internal Uniqueness Constraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="CompatibleRolePlayerTypeError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="5C8D3150-2604-44FC-A468-B678F9B4206E" DisplayName="Incompatible Constrained Role Players" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Column" DefaultValue="0" DisplayName="Column" IsBrowsable="false" Id="222DCF1C-83FB-43F1-A8BE-3D05B8CF1693">
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="JoinPathRequiredError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="153C3FF6-A7F9-4D82-8B6B-8A61D3F40889" DisplayName="Join Required for Multiple Fact Types" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>
		<DomainClass Name="RolePlayerRequiredError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="59A21FDE-D979-4B18-9088-707B79FCE19E" DisplayName="Role Player Required" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="EntityTypeRequiresReferenceSchemeError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="A9177733-169B-418A-A843-3E3777DC9982" DisplayName="EntityType Requires Reference Scheme" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="FrequencyConstraintMinMaxError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="5586C408-1A46-4CA7-8B0D-0462CD904009" DisplayName="Inconsistent Frequency Constraint Minimum and Maximum Values" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="FrequencyConstraintExactlyOneError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="9BBFF3C2-329B-4956-8FFA-1C6F305CF601" DisplayName="Represent Frequency Constraint of Exactly One as Uniqueneness" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ReadingRequiresUserModificationError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="56D0B016-EAF3-4E4F-B17A-7F7987EBC0CB" DisplayName="Reading Text Automatically Modified" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ModelError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="16DF5C5E-83EF-4EDC-B54A-56D58D62D982" DisplayName="ModelError" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="ErrorText" DefaultValue="" DisplayName="ErrorText" IsBrowsable="false" IsElementName="true" Id="6A6023E7-AC27-4D86-AFE4-6428659A048E">
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

		<DomainClass Name="ReferenceModeKind" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="7EC5E835-5EEB-4FB1-AA09-9BD6ABA531E1" DisplayName="ReferenceModeKind" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="FormatString" DefaultValue="" DisplayName="FormatString" Id="3D1B9C67-FF56-4345-B445-30F1F3367613" Description="Default format string for reference mode patterns with this ReferenceModeKind. Replacement field {0}=EntityTypeName, {1}=ReferenceModeName">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="ReferenceModeType" DefaultValue="General" DisplayName="ReferenceModeType" Id="3543E2CB-037D-4D6E-A76A-10CBDFB05146" Description="One of Popular, UnitBased, or General.">
					<Type>
						<DomainEnumerationMoniker Name="ReferenceModeType"/>
					</Type>
				</DomainProperty>
			</Properties>

		</DomainClass>

		<DomainClass Name="IntrinsicReferenceMode" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="F34A46FD-D7EA-4423-B40F-90A6662CADB9" DisplayName="IntrinsicReferenceMode" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ReferenceMode"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="CustomReferenceMode" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="BB33470D-3C98-4B2E-9134-9347C8008861" DisplayName="CustomReferenceMode" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ReferenceMode"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="CustomFormatString" DefaultValue="" DisplayName="CustomFormatString" Id="4A7202FF-1D4F-4770-953A-D63ADA849CB3" Description="Custom format string for this reference mode pattern. Replacement field {0}=EntityTypeName, {1}=ReferenceModeName">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>

		</DomainClass>

		<DomainClass Name="ReferenceMode" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="5123D945-262C-42B7-838D-1B7F4E5A911C" DisplayName="ReferenceMode" InheritanceModifier="Abstract" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptionProvider&lt;ReferenceMode, Design.ReferenceModeTypeDescriptor&lt;ReferenceMode&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="ORMNamedElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="KindDisplay" DefaultValue="" DisplayName="Kind" Id="BBC452CA-0454-4047-9143-B11E065556FB" Kind="CustomStorage" Description="The kind of the reference mode pattern. One of Popular, UnitBase, or General.">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Core.ObjectModel.Design.ReferenceModeKindPicker)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/ORMSolutions.ORMArchitect.Core.ObjectModel/ReferenceModeKind"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="UnspecifiedDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="B7DDA0A4-C18A-4E85-8259-F529FC45F72E" DisplayName="UnspecifiedDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="DataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="FixedLengthTextDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="2B525C4C-9B55-4C8D-98BB-63739E9D7C3D" DisplayName="FixedLengthTextDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="TextDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="VariableLengthTextDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="6F30DE79-85BE-4194-B362-A39023A0E200" DisplayName="VariableLengthTextDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="TextDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="LargeLengthTextDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="27CBCB76-FAC5-436A-950A-CC428FEC9EED" DisplayName="LargeLengthTextDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="TextDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TextDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="E1BE56BD-6663-4F5C-AF6A-39E03DFB2BFA" DisplayName="TextDataType" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="DataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="SignedIntegerNumericDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="F4962B12-8C72-4FEF-9C24-D23A5872A403" DisplayName="SignedIntegerNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="SignedSmallIntegerNumericDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="67825482-E490-4A16-B47D-53E72F4EBEE3" DisplayName="SignedSmallIntegerNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="SignedLargeIntegerNumericDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="AF56003A-52F1-4C02-B203-EF15B4CB2AE1" DisplayName="SignedLargeIntegerNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="UnsignedIntegerNumericDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="9D76D09D-10F6-4DB0-8890-1077A95FB364" DisplayName="UnsignedIntegerNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="UnsignedTinyIntegerNumericDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="A14599A2-2B27-4E76-894F-A1814723EFE9" DisplayName="UnsignedTinyIntegerNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="UnsignedSmallIntegerNumericDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="0EDC60E7-0548-48C3-BC6B-219AF6E50A31" DisplayName="UnsignedSmallIntegerNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="UnsignedLargeIntegerNumericDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="0ED930F6-73C7-4E04-8898-E5F30EF3A641" DisplayName="UnsignedLargeIntegerNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="AutoCounterNumericDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="E2F2CD9B-5C9D-439D-AEAC-A2F093ED04FE" DisplayName="AutoCounterNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="FloatingPointNumericDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="C82CD420-BB13-4F63-9EA7-850512E5B7DD" DisplayName="FloatingPointNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="SinglePrecisionFloatingPointNumericDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="53399895-20E9-49E0-BC73-E00461387680" DisplayName="SinglePrecisionFloatingPointNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="DoublePrecisionFloatingPointNumericDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="F5A7B8A3-2EF5-4143-BDEE-1AA762CB6E02" DisplayName="DoublePrecisionFloatingPointNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="DecimalNumericDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="B86FAADD-E5CF-4745-A796-FABD0310A4A8" DisplayName="DecimalNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="MoneyNumericDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="520A70DA-ACC3-47B2-B8EF-00AF2FF6D170" DisplayName="MoneyNumericDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="NumericDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="NumericDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="CCCFB38B-956F-4E71-8CDC-7A9CD7D6052C" DisplayName="NumericDataType" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="DataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="FixedLengthRawDataDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="1AA62E47-0EB5-45B7-B1FA-AC17EF24E009" DisplayName="FixedLengthRawDataDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="RawDataDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="VariableLengthRawDataDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="01A2EA3B-BC60-4E62-8819-26E81B8D871F" DisplayName="VariableLengthRawDataDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="RawDataDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="LargeLengthRawDataDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="FF35CE6D-2BB6-4DF4-A98C-D303A5698AD2" DisplayName="LargeLengthRawDataDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="RawDataDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="PictureRawDataDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="D33DACE5-3D70-4678-9325-058C1CCFD81F" DisplayName="PictureRawDataDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="RawDataDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="OleObjectRawDataDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="0B79F4F9-09B6-408A-88A2-F8B1051C2B05" DisplayName="OleObjectRawDataDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="RawDataDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="RawDataDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="C5683946-DF1B-42AF-947A-006DD6875CCF" DisplayName="RawDataDataType" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="DataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="AutoTimestampTemporalDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="5553662C-93B7-4C7B-8723-FF56963AE644" DisplayName="AutoTimestampTemporalDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="TemporalDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TimeTemporalDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="13138B79-3CB9-479E-AC5B-569A755085C4" DisplayName="TimeTemporalDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="TemporalDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="DateTemporalDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="ABC122E6-894B-446E-8CD4-EAD7D61FCC46" DisplayName="DateTemporalDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="TemporalDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="DateAndTimeTemporalDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="A5B3A699-DFB5-4522-B024-F55BDE90AC6A" DisplayName="DateAndTimeTemporalDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="TemporalDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TemporalDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="BFBEF833-DD04-4DB3-A167-D1314273B2C6" DisplayName="TemporalDataType" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="DataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TrueOrFalseLogicalDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="689EA7B7-31A8-4800-A98E-99CCD21E112C" DisplayName="TrueOrFalseLogicalDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="LogicalDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="YesOrNoLogicalDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="7E694D96-8444-4007-BFEB-C1B0BD3F96DE" DisplayName="YesOrNoLogicalDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="LogicalDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="LogicalDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="A7D4D492-2702-4B87-BD9E-0D7D7D85943A" DisplayName="LogicalDataType" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="DataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="RowIdOtherDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="42A558F7-1A61-49A6-A207-A706FAF94DD8" DisplayName="RowIdOtherDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="OtherDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ObjectIdOtherDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="48B82DAB-A7E2-4DAB-8D53-9840CF7A15DD" DisplayName="ObjectIdOtherDataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="OtherDataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="OtherDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="0B801E67-9D48-49F7-AA13-9C7BD8153624" DisplayName="OtherDataType" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="DataType"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="DataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="9D0C6367-617F-4A8C-A0E5-5DA23828ED61" DisplayName="DataType" InheritanceModifier="Abstract" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptionProvider&lt;DataType, Design.DataTypeTypeDescriptor&lt;DataType&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="Reading" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="7544854F-A4A7-4429-8859-F1D3B0E52B03" DisplayName="Reading" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptionProvider&lt;Reading, Design.ReadingTypeDescriptor&lt;Reading&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Text" DefaultValue="" DisplayName="Text" Id="A6239359-0AC5-4934-B38A-011AA1F935A6" Description="The text of this reading. Includes ordered replacement fields corresponding to the parent ReadingOrder.">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="IsPrimaryForReadingOrder" DefaultValue="false" DisplayName="IsPrimaryForReadingOrder" Id="1A989428-C41C-498A-BD90-1B92A703AA27" IsBrowsable="false" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="Language" DefaultValue="" DisplayName="Language" IsBrowsable="false" Id="34C42F00-5D21-4731-8E38-9A03271F045A">
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

		<DomainClass Name="ReadingOrder" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="7CB4A39B-D11F-48FC-BFED-B80F5D3FC54E" DisplayName="ReadingOrder" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="ReadingText" DefaultValue="" DisplayName="ReadingText" IsUIReadOnly="true" Id="4E75AD63-A42B-4571-85CE-81A4C5E02C23" Kind="CustomStorage" Description="The text for the default Reading of this ReadingOrder. Includes ordered replacement fields corresponding to this ReadingOrder.">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Core.ObjectModel.Design.ReadingTextEditor)"/>
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

		<DomainClass Name="ValueRange" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="8987ECEA-6C2A-4825-8C9F-465005272CE8" DisplayName="ValueRange" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="MinValue" DefaultValue="" DisplayName="MinValue" IsBrowsable="false" Id="59B141FD-47ED-43FF-837E-858F140FAD57">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="MaxValue" DefaultValue="" DisplayName="MaxValue" IsBrowsable="false" Id="08199824-9DDC-4878-8E04-E0F432069726">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="Text" DefaultValue="" DisplayName="Text" IsBrowsable="false" Id="1FB8C126-4481-41D0-B41C-5A30BC7245DE" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="MinInclusion" DefaultValue="NotSet" DisplayName="MinInclusion" IsBrowsable="false" Id="CDE9FC53-BE51-4C27-9E6C-675CDB580F3A">
					<Type>
						<DomainEnumerationMoniker Name="RangeInclusion"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="MaxInclusion" DefaultValue="NotSet" DisplayName="MaxInclusion" IsBrowsable="false" Id="EB018230-2726-4206-AE2E-1C911B606FC1">
					<Type>
						<DomainEnumerationMoniker Name="RangeInclusion"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="ValueTypeValueConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="E46B0A2E-460E-4FF7-B447-C9C09597B500" DisplayName="ValueTypeValueConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ValueConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="RoleValueConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="6C223B62-6239-4514-81C5-AAD6A10D3A2D" DisplayName="RoleValueConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ValueConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="PathConditionRoleValueConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="9B47EDFC-4267-446D-B3F1-6C79982AD89D" DisplayName="PathConditionRoleValueConstraint" Description="Value constraint applied to a role in a join path.">
			<BaseClass>
				<DomainClassMoniker Name="ValueConstraint"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ValueConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="EF2EFEAD-A124-413C-8F86-C95E2B47160C" DisplayName="ValueConstraint" InheritanceModifier="Abstract" Description="">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptionProvider&lt;ValueConstraint, Design.ValueConstraintTypeDescriptor&lt;ValueConstraint&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseClass>
				<DomainClassMoniker Name="ORMNamedElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="DefinitionText" DefaultValue="" DisplayName="InformalDescription" Description="An informal description of this constraint.&#xd;&#xa;    To insert new lines, use Control-Enter in the dropdown editor, or open the 'ORM Informal Description Editor' tool window." Id="728C6254-DC03-4FF3-B3F2-9BA8B34F60C1" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.MultilineTextEditor&lt;global::ORMSolutions.ORMArchitect.Core.ObjectModel.Definition&gt;)"/>
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
				<DomainProperty Name="NoteText" DefaultValue="" DisplayName="Note" Description="A note to associate with this constraint.&#xd;&#xa;    To insert new lines, use Control-Enter in the dropdown editor, or open the 'ORM Notes Editor' tool window." Id="49306D45-6A55-4D4F-9C83-FC95D33797D0" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.MultilineTextEditor&lt;global::ORMSolutions.ORMArchitect.Core.ObjectModel.Note&gt;)"/>
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
				<DomainProperty Name="Text" DefaultValue="" DisplayName="Text" Id="410FCE34-DACB-4F59-94A6-FF7E42108E74" Kind="CustomStorage" Description="The range of possible values.&#xd;&#xa;    To specify a range, use '..' between the range endpoints, square brackets to specify a closed endpoint, and parentheses to specify an open endpoint. Commas are used to entered multiple ranges or discrete values.&#xd;&#xa;    Example: {[10..20), 30} specifies all values between 10 and 20 (but not including 20) and the value 30.">
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

		<DomainClass Name="ValueConstraintError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="A18FA855-E7CA-4716-8E8D-1606C09B090A" DisplayName="Value Constraint Value Invalid for DataType" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="MinValueMismatchError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="7E0D53CF-D374-4EDA-B6A6-04D381AA0DC5" DisplayName="Minimum Bound of Value Range Invalid for DataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ValueConstraintError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="MaxValueMismatchError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="CCE42465-23A0-4726-8881-3ADB48E2CC67" DisplayName="Maximum Bound of Value Range Invalid for DataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ValueConstraintError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ImpliedInternalUniquenessConstraintError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="B7381F8B-C95E-408D-9747-4B6BB35C1171" DisplayName="FactType has Implied Internal Uniqueness Constraint(s)" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="FrequencyConstraintViolatedByUniquenessConstraintError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="4A239F5F-2FF9-4E5A-AAA8-50D313ED0193" DisplayName="Frequency Constraint Violated By Uniqueness Constraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="RingConstraintTypeNotSpecifiedError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="15026270-DFD6-470D-A997-233173E644DC" DisplayName="Ring Constraint Type Not Specified" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="SubtypeMetaRole" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="4AD109E1-3AB4-4F8A-A862-1694AEE06289" DisplayName="SubtypeMetaRole" Description="">
			<BaseClass>
				<DomainClassMoniker Name="Role"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="SupertypeMetaRole" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="E559A725-BBA4-4068-B247-DC8C4B1628D7" DisplayName="SupertypeMetaRole" Description="">
			<BaseClass>
				<DomainClassMoniker Name="Role"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="Definition" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="25D3235C-76E2-4095-8EFD-847057937A00" DisplayName="InformalDescription" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Text" DefaultValue="" Description="The description contents.&#xd;&#xa;    To insert new lines, use Control-Enter in the dropdown editor, or open the 'ORM Notes Editor' tool window." DisplayName="Text" Id="B68867A8-4B52-4DE1-8B39-7EEE5ECB60A4">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.MultilineTextEditor&lt;global::ORMSolutions.ORMArchitect.Core.ObjectModel.Definition&gt;)"/>
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

		<DomainClass Name="Note" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="C3DE6C8C-2215-49B0-BD70-70D2C3630C33" DisplayName="Note" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Text" DefaultValue="" Description="The note contents.&#xd;&#xa;    To insert new lines, use Control-Enter in the dropdown editor, or open the 'ORM Notes Editor' tool window." DisplayName="Note" Id="0EF3BC12-45FF-46A8-B325-CDFCC105A1E1">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.MultilineTextEditor&lt;global::ORMSolutions.ORMArchitect.Core.ObjectModel.Note&gt;)"/>
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

		<DomainClass Name="ModelNote" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="41D610C9-BACC-473D-BFE6-7034E6FF0B11" DisplayName="ModelNote" Description="">
			<BaseClass>
				<DomainClassMoniker Name="Note"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="CompatibleSupertypesError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="70A9ED25-7A0E-4DEC-B39D-83BB1A6294B8" DisplayName="Incompatible or Transitive Supertypes" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ImplicationError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="78026AEA-19EB-497A-A596-25C929F67AA8" DisplayName="Constraint Implied by Intersecting Constraints" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="EqualityOrSubsetImpliedByMandatoryError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="80B1F784-858E-483B-91A5-E55CFEBA44B9" DisplayName="Mandatory Constraint Implies Equality or Subset Constraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ImplicationError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="PreferredIdentifierRequiresMandatoryError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="129CCE68-7CE9-4A97-BAD3-C36B4D372A77" DisplayName="EntityType with Compound Preferred Identifier Requires Mandatory Constraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ValueRangeOverlapError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="2CF1EE1A-1737-4868-9B5C-95B2C0F9488B" DisplayName="Value Ranges Overlap" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ValueConstraintError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ValueConstraintValueTypeDetachedError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="92C7060E-A912-4986-984E-E9915B1321AD" DisplayName="Path to Identifying ValueType Detached" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ValueConstraintError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="FactTypeDerivationExpression" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="2A29F892-B69B-4EEB-BF50-A0E59B6E64C2" DisplayName="FactTypeDerivationExpression" Description="">
			<BaseClass>
				<DomainClassMoniker Name="Expression"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="DerivationStorage" DefaultValue="Derived" DisplayName="DerivationStorage" IsBrowsable="false" Id="6B011B44-9854-436A-ADED-7BBC635A7C1F">
					<Type>
						<DomainEnumerationMoniker Name="DerivationExpressionStorageType"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="SubtypeDerivationExpression" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="CCE39440-4C8D-45E2-ACFE-1642989D1107" DisplayName="SubtypeDerivationExpression" Description="">
			<BaseClass>
				<DomainClassMoniker Name="Expression"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ObjectTypeInstance" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="870F5EE8-0859-4710-A526-66635F4EFD14" DisplayName="ObjectTypeInstance" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Name" DefaultValue="" DisplayName="Name" IsElementName="true" Id="553DEB12-8FE0-4FE4-B94E-52F1CD5DCF0A" Kind="CustomStorage"  GetterAccessModifier="Public" SetterAccessModifier="Private" Description="An ordered tuple of values for this instance. If the parent &lt;see cref=&quot;ObjectType&quot;/&gt; objectifies a &lt;see cref=&quot;FactType&quot;/&gt;, then Name returns the FactType population prepended by an external identifier reference.">
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
				<DomainProperty Name="IdentifierName" DefaultValue="" DisplayName="IdentifierName" Id="AC967572-B11E-41C9-A206-CC2424E5E17E" Kind="CustomStorage"  GetterAccessModifier="Public" SetterAccessModifier="Private" Description="An ordered tuple of values for this instance, ignores objectification of the associated &lt;see cref=&quot;ObjectType&quot;/&gt;.">
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

		<DomainClass Name="EntityTypeInstance" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="5F8B6A1C-3020-41C9-97B4-E54A3E98B368" DisplayName="EntityTypeInstance" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ObjectTypeInstance"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="EntityTypeSubtypeInstance" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="19F9A457-D2C6-4D0A-B767-542DF80FEFF3" DisplayName="EntityTypeSubtypeInstance" Description="An instance of an EntityType Subtype that uses the preferred identification scheme of a parent.">
			<BaseClass>
				<DomainClassMoniker Name="ObjectTypeInstance"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ValueTypeInstance" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="BCC1483D-CBB8-4E4F-903B-16224768F6F5" DisplayName="ValueTypeInstance" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ObjectTypeInstance"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Value" DefaultValue="" DisplayName="Value" Id="1D0232BA-A92F-4B81-99BF-2A2A44821030" Description="The instance value.">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>

		</DomainClass>

		<DomainClass Name="FactTypeInstance" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="78458A27-FDB1-4B6E-9D0A-D42DD8D5AEAD" DisplayName="FactTypeInstance" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Name" DefaultValue="" DisplayName="Name" IsBrowsable="false" IsElementName="true" Id="AA6CFB60-9F6A-48AB-AB0F-445BF7112FB9" Kind="CustomStorage"  GetterAccessModifier="Public" SetterAccessModifier="Private">
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
				<DomainProperty Name="NameChanged" DefaultValue="" DisplayName="NameChanged" Id="F510E9DB-71BF-4D70-B36A-4C6BF17D0917" IsBrowsable="false" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/Int64"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="TooFewEntityTypeRoleInstancesError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="39F447EA-8EA4-483D-B791-848AD27544E2" DisplayName="Incomplete Sample Population to Identify EntityType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TooFewFactTypeRoleInstancesError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="BE44DD74-2569-421E-8E1B-ABCDC7810C92" DisplayName="Incomplete FactType Sample Population" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="CompatibleValueTypeInstanceValueError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="D5B21137-31E8-444D-BCD2-58BBF442B4C0" DisplayName="Sample Population Value Invalid for DataType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

	</Classes>

	<Relationships>
		<!--<DomainRelationship Name="ORMElementLink" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" AllowsDuplicates="false" InheritanceModifier="Abstract" Id="D6DC3311-4298-4EE5-9DA2-B1378AB09BF1">
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

		<DomainRelationship Name="FactConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" AllowsDuplicates="false" InheritanceModifier="Abstract" Id="BCF635F2-F2C6-4690-956D-2A44C48A9DA9">
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

		<DomainRelationship Name="FactSetComparisonConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" AllowsDuplicates="false" Id="FF8F65AD-248A-4EF8-9172-515204C9A44C">
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

		<DomainRelationship Name="FactSetConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" AllowsDuplicates="false" Id="771EC962-8086-4B21-BFB2-830F30E52861">
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

		<DomainRelationship Name="ORMModelElementHasExtensionElement" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="FF867109-FE3A-42C4-9770-2D735555016A">
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

		<DomainRelationship Name="ORMModelElementHasExtensionModelError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="7A4D2B10-43F3-475F-AA0A-8F880B9A1E4B">
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

		<DomainRelationship Name="ValueTypeHasDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="E4BBC988-E920-4ACB-8071-552AEEBA7FA9">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Properties>
				<DomainProperty Name="Scale" DefaultValue="0" DisplayName="Scale" IsBrowsable="false" Id="F21936E2-E7E6-4AFC-B96F-43E9C76F8A9B">
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="Length" DefaultValue="0" DisplayName="Length" IsBrowsable="false" Id="60D1471D-23C9-4D4D-91AF-6AA5E9BA7B8B">
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

		<DomainRelationship Name="Objectification" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="935DC968-DDD1-4C57-9D43-9F367BE78C6D">
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
				<DomainRole Name="NestingType" PropertyName="NestedFactType" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="NestingType" PropertyDisplayName="ObjectifiedFactType" Id="2660CF3E-2A56-496D-98CD-BFFAC5E73198" Description="The FactType objectified by this EntityType.">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Core.ObjectModel.Design.NestedFactTypePicker)"/>
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
				<DomainRole Name="NestedFactType" PropertyName="NestingType" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="NestedFactType" PropertyDisplayName="ObjectifyingEntityType" Id="69F805CC-874F-4E03-8364-0A0445168B26" Description="The EntityType that objectifies this FactType.">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Core.ObjectModel.Design.NestingTypePicker)"/>
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

		<DomainRelationship Name="ObjectTypePlaysRole" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="0AB8D25E-45D4-4696-B6EE-6F108FEE97A7">
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

		<DomainRelationship Name="ModelHasObjectType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="F060C714-EF07-481F-AB4B-BA02B9908025">
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

		<DomainRelationship Name="ModelHasFactType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="DF97B102-8500-4EA1-9059-356BC49E7066">
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

		<DomainRelationship Name="ModelHasError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="20CFE989-A6AF-4D97-A552-AE5DD7684971">
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

		<DomainRelationship Name="ModelHasReferenceModeKind" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="1B280979-E9F5-4774-847F-3A1078DB1943">
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

		<DomainRelationship Name="ModelHasReferenceMode" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="F6647D65-926B-4E66-81BC-F6293A44093E">
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

		<DomainRelationship Name="ModelHasSetConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="C0104439-3B39-41E7-9B68-61F31F17A066">
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

		<DomainRelationship Name="ObjectTypeImpliesMandatoryConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="9A43A30E-6F18-46FF-9B8D-5313F6E93807">
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

		<DomainRelationship Name="ModelHasSetComparisonConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="37FBE5B6-4E18-43E2-B34B-DAB0EF69DDE4">
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

		<DomainRelationship Name="ModelHasDataType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="33611A97-9270-469E-AB75-B53A24699A2D">
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

		<DomainRelationship Name="GenerationStateHasGenerationSetting" IsEmbedding="true"  Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="475FF8F0-0E0D-4CF1-8110-C132F815E2E6">
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

		<DomainRelationship Name="NameGeneratorRefinesNameGenerator" IsEmbedding="true" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="6224BA97-F59F-4360-A159-7CD5DDB6493F">
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

		<DomainRelationship Name="ElementHasAlias" InheritanceModifier="Abstract" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="94F76133-EACC-40D1-B61E-1EBD32C0F81F">
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

		<DomainRelationship Name="RecognizedPhraseHasAbbreviation" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" DisplayName="Other Phrase Replacements and Omissions" IsEmbedding="true" Id="83D9D4FB-5F40-42F2-A014-8A5E5052C24F">
			<Attributes>
				<ClrAttribute Name="ORMSolutions.ORMArchitect.Core.ObjectModel.NameAliasOwnerCreationInfoAttribute">
					<Parameters>
						<AttributeParameter Value="true"/>
						<AttributeParameter Value='"6D4F2B86-2C27-4F82-84CE-8AA23DCC0EF8"'/>
						<AttributeParameter Value='"GetExistingRecognizedPhrase"'/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementHasAlias"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="RecognizedPhrase" PropertyName="AbbreviationCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="RecognizedPhrase" Id="64397C80-2261-4C8B-9CF3-B0CF963EBCCF">
					<RolePlayer>
						<DomainClassMoniker Name="RecognizedPhrase"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Abbreviation" PropertyName="Element" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="false" DisplayName="Alias" Id="5FEF356B-E9AC-4522-811F-E062D83FF9D3">
					<RolePlayer>
						<DomainClassMoniker Name="NameAlias"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ObjectTypeHasAbbreviation" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" DisplayName="ObjectType Abbreviations" IsEmbedding="true" Id="6A85513C-747F-4A8C-B45A-B5CFF88314E5">
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

		<DomainRelationship Name="ModelContainsRecognizedPhrase" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="27AC76D9-EEDA-4836-8A93-59A7197122D9">
			<Source>
				<DomainRole Name="Model" PropertyName="RecognizedPhraseCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="NameGenerator" Id="965FF527-A2D6-4468-94DE-464489E332E2">
					<RolePlayer>
						<DomainClassMoniker Name="ORMModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="RecognizedPhrase" PropertyName="Model" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="RecognizedPhrase" Id="6D4F2B86-2C27-4F82-84CE-8AA23DCC0EF8">
					<RolePlayer>
						<DomainClassMoniker Name="RecognizedPhrase"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="RecognizedPhraseHasDuplicateNameError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="6D1ABE6F-A38B-4981-8124-4DFB48F1AA5A">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="RecognizedPhrase" PropertyName="DuplicateNameError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="RecognizedPhrase" Id="41DF5814-E8E1-4B5E-A6D2-C3C9CE8459A9">
					<RolePlayer>
						<DomainClassMoniker Name="RecognizedPhrase"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="DuplicateNameError" PropertyName="RecognizedPhraseCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="DuplicateNameError" Id="DC8DEFE0-274B-42F5-834E-42BFB183FA9E">
					<RolePlayer>
						<DomainClassMoniker Name="RecognizedPhraseDuplicateNameError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FunctionHasDuplicateNameError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="0EC6FED9-6561-43C7-B704-9BC56D7474FE">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="Function" PropertyName="DuplicateNameError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Function" Id="A9A2CADB-325A-416B-AFD6-3215814BF46A">
					<RolePlayer>
						<DomainClassMoniker Name="Function"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="DuplicateNameError" PropertyName="FunctionCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="DuplicateNameError" Id="7C4C1204-9BAC-4E3E-BABC-4D849F4CF30A">
					<RolePlayer>
						<DomainClassMoniker Name="FunctionDuplicateNameError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ExternalRoleConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" AllowsDuplicates="false" Id="9692D61F-13AE-4FEE-9F76-8E0D9A5FF976">
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

		<DomainRelationship Name="ExclusiveOrConstraintCoupler" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="false" Id="F2244A4C-BBE0-463B-9E8B-6A768C5C1469">
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

		<DomainRelationship Name="SetComparisonConstraintHasRoleSequence" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="84B13BEA-FC8C-446C-B643-9688B99AF1B6">
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

		<DomainRelationship Name="ConstraintRoleSequenceHasRole" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" AllowsDuplicates="false" Id="BD1A0274-1152-4A54-B4A5-58BD023CE710">
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

		<DomainRelationship Name="SetComparisonConstraintHasTooFewRoleSequencesError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="3167F5D3-C234-46E3-AAC2-4CEB791DFB9C">
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

		<DomainRelationship Name="SetComparisonConstraintHasTooManyRoleSequencesError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="E7C33130-2D1F-4F95-B988-BD7608CF2D1C">
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

		<DomainRelationship Name="ObjectTypeHasDuplicateNameError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="BC1031EB-8590-4A14-ABBD-F12A18622855">
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

		<DomainRelationship Name="RoleInstanceHasPopulationUniquenessError" InheritanceModifier="Abstract" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="5DBE50CD-A939-484D-9B96-700CB6CC7813">
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

		<DomainRelationship Name="EntityTypeRoleInstanceHasPopulationUniquenessError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="04312BEF-EA3E-4525-9A1A-903497EFDAF7">
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

		<DomainRelationship Name="FactTypeRoleInstanceHasPopulationUniquenessError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="A483FDFE-53EF-4352-8D97-986BF2C0E8E7">
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

		<DomainRelationship Name="SetComparisonConstraintHasContradictionError" InheritanceModifier="Abstract" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="A1D4A389-9D19-4921-BD0D-D965B53897E3">
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

		<DomainRelationship Name="SetComparisonConstraintHasExclusionContradictsEqualityError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="E7E85549-6312-4E65-AD48-4DDF51E8139C">
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

		<DomainRelationship Name="SetComparisonConstraintHasExclusionContradictsSubsetError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="B4BBA5AF-05AC-4FEC-8288-8D80DC0AF16E">
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

		<DomainRelationship Name="ExclusionConstraintHasExclusionContradictsMandatoryError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="E638E328-24A9-42C0-BBB1-F1EBC4B6E218">
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

		<DomainRelationship Name="MandatoryConstraintHasExclusionContradictsMandatoryError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="6CDDA5A5-C6FE-4E9B-9248-17512F9C891A">
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

		<DomainRelationship Name="SubsetConstraintHasNotWellModeledSubsetAndMandatoryError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="4BA72B40-A736-49D4-9FDE-8B07EE4A61A6">
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

		<DomainRelationship Name="MandatoryConstraintHasNotWellModeledSubsetAndMandatoryError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="475557A9-6E11-4D1C-A5A0-9D06DAED3EE5">
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

		<DomainRelationship Name="MandatoryConstraintHasPopulationMandatoryError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="9A93DD53-8683-47F4-9EE7-4F1F244A218E">
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

		<DomainRelationship Name="ObjectTypeInstanceHasPopulationMandatoryError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="E0F0750E-47CB-44C6-B348-A9A1101475A7">
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

		<DomainRelationship Name="ReadingOrderHasReading" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="F945750F-2F77-43F4-8314-E5B351913902">
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

		<DomainRelationship Name="FactTypeHasReadingOrder" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="14C7D6CB-0C30-4326-A877-D3AEE7A9FADF">
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

		<DomainRelationship Name="ReferenceModeHasReferenceModeKind" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="8B022051-E094-435E-B985-688FFC89DC6D">
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

		<DomainRelationship Name="SetConstraintHasDuplicateNameError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="CB5DF90F-3917-4BD1-9807-A24F6D7C52F9">
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

		<DomainRelationship Name="SetComparisonConstraintHasDuplicateNameError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="CF7AF531-F3D3-42E4-A9F7-D44536DA9E53">
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

		<DomainRelationship Name="EntityTypeHasPreferredIdentifier" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="8FF87866-8213-4A03-85A8-B0275A265793">
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

		<DomainRelationship Name="ReadingHasTooManyRolesError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="D2116BC7-25A8-455E-9347-414BD03B7546">
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

		<DomainRelationship Name="ReadingHasTooFewRolesError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="FC3E0A3C-40CE-4DED-8A6B-241C7B51C099">
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

		<DomainRelationship Name="ReadingHasReadingRequiresUserModificationError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="3B7E4BBF-06B6-489E-BDF5-72EDEE5B87F4">
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

		<DomainRelationship Name="SetComparisonConstraintHasExternalConstraintRoleSequenceArityMismatchError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="C5A25732-F5A7-409E-B56A-6419A951FB13">
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

		<DomainRelationship Name="ElementAssociatedWithModelError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="5E4032B3-EF22-447B-9732-F25CACA1E613">
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

		<DomainRelationship Name="FactTypeHasFactTypeRequiresReadingError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="EEC8EB82-5B15-4B61-8737-DA1A54199A13">
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

		<DomainRelationship Name="FactTypeHasFactTypeRequiresInternalUniquenessConstraintError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="DD5FF7F8-7169-489B-9B8A-EDE3772F52BE">
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

		<DomainRelationship Name="ValueTypeHasValueConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="3DD5CC0F-891E-4A88-A8B2-AEB28A4795E3">
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

		<DomainRelationship Name="RoleHasValueConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="BFB9DA2A-0EA6-46AB-B608-41440BDD0D84">
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

		<DomainRelationship Name="PathedRoleHasValueConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="C77DB9EF-B86F-4B08-BBAA-B2BA2DAD64D9">
			<Source>
				<DomainRole Name="PathedRole" PropertyName="ValueConstraintCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="PathedRole" Id="046A095E-696D-4899-8051-58A70ACED299">
					<RolePlayer>
						<DomainRelationshipMoniker Name="PathedRole"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ValueConstraint" PropertyName="PathedRole" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ValueConstraint" Id="D3438173-2268-4F8B-8237-FD563D69A3C6">
					<RolePlayer>
						<DomainClassMoniker Name="PathConditionRoleValueConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="PathConditionRoleValueConstraintAppliesToRolePathCombination" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="EED3B60C-2688-43A7-8E2B-DF14A8B5DFC4">
			<Source>
				<DomainRole Name="ValueConstraint" PropertyName="AppliesToPathCombination" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="AppliesToPathCombination" Id="E3A0CE35-4238-45B5-917F-35107A66A698">
					<RolePlayer>
						<DomainClassMoniker Name="PathConditionRoleValueConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Combination" PropertyName="ValueConstraintCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Combination" Id="B86177CD-20A5-4D1E-977F-C3811D92E302">
					<RolePlayer>
						<DomainClassMoniker Name="RolePathCombination"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ValueConstraintHasValueRange" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="53B596BA-0506-4533-80B0-391891C61C9A">
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

		<DomainRelationship Name="ValueTypeHasUnspecifiedDataTypeError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="F2A79E36-A317-4C36-81DA-D562D2AFBF09">
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

		<DomainRelationship Name="SetComparisonConstraintHasCompatibleRolePlayerTypeError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="13410C4F-FFED-4B0F-AD0B-BD48D09B4310">
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

		<DomainRelationship Name="SetConstraintHasCompatibleRolePlayerTypeError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="91CDE095-28D9-4852-B171-430FE5A29429">
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

		<DomainRelationship Name="ConstraintRoleSequenceHasJoinPathRequiredError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="91C09C68-C29D-42F5-B0BC-6BDDC7BBC745">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="RoleSequence" PropertyName="JoinPathRequiredError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="RoleSequence" Id="0D1381DB-9CA6-465F-8166-0285674EDE13">
					<RolePlayer>
						<DomainClassMoniker Name="ConstraintRoleSequence"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="JoinPathRequiredError" PropertyName="RoleSequence" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="JoinPathRequiredError" Id="D1DD16C2-97CE-4615-B741-47DA5E06D077">
					<RolePlayer>
						<DomainClassMoniker Name="JoinPathRequiredError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="UniquenessConstraintHasNMinusOneError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="429F7144-1227-4D0E-B4F8-59AD6FFC7EB3">
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

		<DomainRelationship Name="RoleHasRolePlayerRequiredError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="09E6AC31-2CA1-4126-8C95-BFC571088B2D">
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

		<DomainRelationship Name="ObjectTypeHasEntityTypeRequiresReferenceSchemeError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="1B6CBB8C-D1A6-4949-AC4D-596DC1CE147F">
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

		<DomainRelationship Name="FrequencyConstraintHasFrequencyConstraintMinMaxError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="2E851B91-FCB9-4B3C-9276-2C2E3A1972C9">
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

		<DomainRelationship Name="FrequencyConstraintHasFrequencyConstraintExactlyOneError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="5A820704-A594-48C8-9C56-AF2567C92D91">
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

		<DomainRelationship Name="ObjectificationImpliesFactType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="D2706F81-78CC-493E-90C9-D54A10D33FA0">
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

		<DomainRelationship Name="ValueRangeHasMaxValueMismatchError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="1D2620BE-40AC-4F10-B420-5CD52687DD49">
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

		<DomainRelationship Name="ValueRangeHasMinValueMismatchError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="0E8BE672-BCBE-412B-9589-76BFA88FDE38">
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

		<DomainRelationship Name="FactTypeHasImpliedInternalUniquenessConstraintError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="32D5A7E1-5A80-44AB-BC2E-96A15A4D92CB">
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

		<DomainRelationship Name="SetConstraintHasTooFewRoleSequencesError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="6409DBE5-5C44-42AF-B0C6-FB1EE7E3AF2A">
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

		<DomainRelationship Name="SetConstraintHasTooManyRoleSequencesError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="D54EB064-7FC6-4BCD-AF30-C73E2D586FC4">
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

		<DomainRelationship Name="FrequencyConstraintHasFrequencyConstraintViolatedByUniquenessConstraintError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="F6B8084D-4406-4AB3-9BA9-C89FB7BBA074">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="FrequencyConstraint" PropertyName="FrequencyConstraintViolatedByUniquenessConstraintError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FrequencyConstraint" Id="D0F67CC7-1A3D-4875-B7C1-267C0CAA2D98">
					<RolePlayer>
						<DomainClassMoniker Name="FrequencyConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="FrequencyConstraintViolatedByUniquenessConstraintError" PropertyName="FrequencyConstraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="FrequencyConstraintViolatedByUniquenessConstraintError" Id="758254C3-BC61-4FD6-BFFB-29BAE506B204">
					<RolePlayer>
						<DomainClassMoniker Name="FrequencyConstraintViolatedByUniquenessConstraintError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="RingConstraintHasRingConstraintTypeNotSpecifiedError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="62E65E16-EFA7-43D0-9759-8715D0C8B914">
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

		<DomainRelationship Name="ValueConstraintHasDuplicateNameError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="3D69F8DE-6075-432B-8843-E8BABC677457">
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

		<DomainRelationship Name="ModelHasDefinition" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="BC513C18-D426-4E5E-907C-1CD0C87732F1">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Model" PropertyName="Definition" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="EE4B3D9B-415E-4105-90BD-BE09D8E135A7">
					<RolePlayer>
						<DomainClassMoniker Name="ORMModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Definition" PropertyName="Model" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Definition" Id="86F015DA-181F-4431-9773-892D82E1C448">
					<RolePlayer>
						<DomainClassMoniker Name="Definition"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ObjectTypeHasDefinition" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="CC5801E2-DC99-4927-8924-F3E451F61E60">
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

		<DomainRelationship Name="FactTypeHasDefinition" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="A2B92B06-BA59-4659-905E-D1A68B5F7865">
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

		<DomainRelationship Name="SetConstraintHasDefinition" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="7CF6A973-7693-41E1-B366-8F225B354818">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="SetConstraint" PropertyName="Definition" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetConstraint" Id="51269942-962E-49DB-84A0-594FF220B278">
					<RolePlayer>
						<DomainClassMoniker Name="SetConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Definition" PropertyName="SetConstraint" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Definition" Id="004F7FEB-02F9-4F4E-B30B-B79E9249F479">
					<RolePlayer>
						<DomainClassMoniker Name="Definition"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SetComparisonConstraintHasDefinition" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="6134C02A-9A2C-4B67-8B61-F31067B39070">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="SetComparisonConstraint" PropertyName="Definition" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetComparisonConstraint" Id="E97BB07F-315B-4038-B89C-A8D38673422A">
					<RolePlayer>
						<DomainClassMoniker Name="SetComparisonConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Definition" PropertyName="SetComparisonConstraint" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Definition" Id="D6E68100-7417-41AF-8C67-82E4B31C6639">
					<RolePlayer>
						<DomainClassMoniker Name="Definition"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ValueConstraintHasDefinition" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="94530978-1A69-47DB-BF78-0F075B8A5FC9">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ValueConstraint" PropertyName="Definition" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ValueConstraint" Id="4402D3B6-2D2F-4A2A-99CC-BF2555461479">
					<RolePlayer>
						<DomainClassMoniker Name="ValueConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Definition" PropertyName="ValueConstraint" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Definition" Id="B41BFF8E-FECF-4A56-AF41-D1F76D9E9DBC">
					<RolePlayer>
						<DomainClassMoniker Name="Definition"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ElementGroupingHasDefinition" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="772A73A6-4362-494A-9F40-36228F0802C9">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Grouping" PropertyName="Definition" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ElementGrouping" Id="1DF300CE-7575-4602-BE59-D37BECC8E9BA">
					<RolePlayer>
						<DomainClassMoniker Name="ElementGrouping"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Definition" PropertyName="Grouping" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Definition" Id="8ACD65F1-0B2C-40F1-B2E8-FAC3D34F8456">
					<RolePlayer>
						<DomainClassMoniker Name="Definition"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FactTypeHasNote" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="B41C4D61-2A9F-4C91-B948-52E53A8E525F">
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

		<DomainRelationship Name="ObjectTypeHasNote" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="8357F61D-E61E-40F5-B98A-782B02A85B1A">
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

		<DomainRelationship Name="SetConstraintHasNote" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="A6D0352F-F155-431A-B798-E89342C8F05F">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="SetConstraint" PropertyName="Note" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetConstraint" Id="4F6A92AC-0643-461F-BF0A-09A9DD3C7E0D">
					<RolePlayer>
						<DomainClassMoniker Name="SetConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Note" PropertyName="SetConstraint" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Note" Id="FD8D1E32-54DE-40F6-A538-CBBA909FE5BB">
					<RolePlayer>
						<DomainClassMoniker Name="Note"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SetComparisonConstraintHasNote" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="4C6469F2-688E-4475-BA7A-3722A052F935">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="SetComparisonConstraint" PropertyName="Note" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetComparisonConstraint" Id="7B0AC9C2-0E88-4F0D-B52C-0F70D0AAD974">
					<RolePlayer>
						<DomainClassMoniker Name="SetComparisonConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Note" PropertyName="SetComparisonConstraint" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Note" Id="F2F2A25D-D4C1-47BE-A567-D95939D7C8B1">
					<RolePlayer>
						<DomainClassMoniker Name="Note"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ValueConstraintHasNote" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="24E5A243-AA38-458F-ACF8-11B71EA3D7D3">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ValueConstraint" PropertyName="Note" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ValueConstraint" Id="E9E038E7-1054-4F14-99CB-C69887454529">
					<RolePlayer>
						<DomainClassMoniker Name="ValueConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Note" PropertyName="ValueConstraint" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Note" Id="168403A0-3BA7-440E-9202-1A7CFAAF12E1">
					<RolePlayer>
						<DomainClassMoniker Name="Note"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ElementGroupingHasNote" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="090BFBF3-1514-41AB-9356-B82D8858BFF6">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Grouping" PropertyName="Note" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Grouping" Id="3DA67E56-0612-4CAC-9513-89AEC5EBEBFA">
					<RolePlayer>
						<DomainClassMoniker Name="ElementGrouping"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Note" PropertyName="Grouping" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Note" Id="24750293-0741-45A9-AF7F-CEA4B6D20E96">
					<RolePlayer>
						<DomainClassMoniker Name="Note"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ModelHasPrimaryNote" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="BEA348B0-FC00-4B2F-8285-882693FE408F">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Model" PropertyName="Note" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="189B06E7-23C1-42C1-9DA0-25A62ACA12E1">
					<RolePlayer>
						<DomainClassMoniker Name="ORMModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Note" PropertyName="PrimaryForModel" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Note" Id="9F33E7CB-A993-451F-A60E-2BE8E21B9AC3">
					<RolePlayer>
						<DomainClassMoniker Name="Note"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ModelHasModelNote" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="783EA177-E965-4C01-9D4A-A89C016203B6">
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

		<DomainRelationship Name="ModelNoteReferencesModelElement" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="false" Id="57A1D17B-DB15-418A-8D82-3D44B3D1169F" AllowsDuplicates="false" InheritanceModifier="Abstract">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptionProvider&lt;ModelNoteReferencesModelElement, global::ORMSolutions.ORMArchitect.Framework.Design.ElementTypeDescriptor&lt;ModelNoteReferencesModelElement&gt;&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
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

		<DomainRelationship Name="ModelNoteReferencesFactType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="false" Id="A6F1EB10-F929-4389-B584-38DFE11A85C2" DisplayName="ModelNoteOnFactType">
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

		<DomainRelationship Name="ModelNoteReferencesObjectType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="false" Id="CB83FD24-7819-4C34-AF59-B4E14AE3BE8F" DisplayName="ModelNoteOnObjectType">
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

		<DomainRelationship Name="ModelNoteReferencesSetConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="false" Id="F5582A97-F2AE-45FA-A3B8-A00D62020519" DisplayName="ModelNoteOnConstraint">
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

		<DomainRelationship Name="ModelNoteReferencesSetComparisonConstraint" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="false" Id="57A31B5C-E265-4C91-8C6C-151101258E28" DisplayName="ModelNoteOnConstraint">
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

		<DomainRelationship Name="ObjectTypeHasCompatibleSupertypesError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="4A739F80-00FA-4F02-BD81-ED60C79DEFC3">
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

		<DomainRelationship Name="SetConstraintHasImplicationError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="7DEA2631-58EF-46A6-B9E1-A8EDA2948AE3">
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

		<DomainRelationship Name="SetComparisonConstraintHasImplicationError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="6F9E9E77-3DA6-4B01-B3AB-F46FB4C43CA8">
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

		<DomainRelationship Name="SetComparisonConstraintHasEqualityOrSubsetImpliedByMandatoryError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="A7CA6438-CACE-4FCC-B96C-03E1DDCD3152">
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

		<DomainRelationship Name="ObjectTypeHasPreferredIdentifierRequiresMandatoryError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="31A1BFF6-47DC-4F00-955B-1935082A3F25">
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

		<DomainRelationship Name="ValueConstraintHasValueRangeOverlapError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="9044BE08-D88B-4BCA-B261-0841E1C73B5D">
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

		<DomainRelationship Name="ValueConstraintHasValueTypeDetachedError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="706B0A12-F0E1-4048-8CEB-EEB5D7BC5CB3">
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

		<DomainRelationship Name="FactTypeHasRole" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="40F02204-F32A-4424-9FD5-5B6B943C603A">
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

		<DomainRelationship Name="ReadingOrderHasRole" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" AllowsDuplicates="false" Id="F4D3824F-5764-434B-9ABD-FD847D4B7570">
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

		<DomainRelationship Name="RoleProxyHasRole" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="5A3809EF-42F1-4965-8490-52FEA5DA30A2">
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

		<DomainRelationship Name="ObjectifiedUnaryRoleHasRole" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="8455A054-C4BF-4E35-99B0-5054602675F1" Description="Links a unary role with the objectified unary role in the implied FactType. Implies a single-column equality constraint between the two roles.">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ObjectifiedUnaryRole" PropertyName="TargetRole" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ObjectifiedUnaryRole" Id="04095428-688B-44D0-9BAE-84CBEA134295">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectifiedUnaryRole"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="TargetRole" PropertyName="ObjectifiedUnaryRole" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="TargetRole" Id="30BBECA0-CCB7-4A34-8AED-82EC95CC1166">
					<RolePlayer>
						<DomainClassMoniker Name="Role"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FactTypeHasDerivationExpression" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="27127A53-8E17-420F-9E87-9812F7C76CD8">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="FactType" PropertyName="DerivationExpression" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="73B1A9D8-42A4-44E0-B906-AEF10E346DB6">
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

		<DomainRelationship Name="SubtypeHasDerivationExpression" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="EFC3B143-5649-4D72-87B1-3FBBC58D9764">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Subtype" PropertyName="DerivationExpression" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Subtype" Id="10EA88C0-446D-4F1C-84E2-726031C14211">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="DerivationRule" PropertyName="Subtype" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="DerivationRule" Id="ACCEFC08-FD9D-48B5-B664-29B38484326B">
					<RolePlayer>
						<DomainClassMoniker Name="SubtypeDerivationExpression"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ObjectTypeHasObjectTypeInstance" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="F4343CDE-A3C7-402C-AF81-CBDC8F092C9E">
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

		<DomainRelationship Name="EntityTypeHasEntityTypeInstance" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="0F9CDA9D-88CE-47DD-B202-93B1455E08C3">
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

		<DomainRelationship Name="EntityTypeSubtypeHasEntityTypeSubtypeInstance" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="7E059BD5-D4A9-48A1-88FA-6459B77D7E23">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ObjectTypeHasObjectTypeInstance"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="EntityTypeSubtype" PropertyName="EntityTypeSubtypeInstanceCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="EntityTypeSubtype" Id="0DADE527-8AD4-4234-9EF4-F78FD7D360C2">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="EntityTypeSubtypeInstance" PropertyName="EntityTypeSubtype" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="EntityTypeSubtypeInstance" Id="9A51223A-5D4D-4E86-BB76-A49F28DEE71D">
					<RolePlayer>
						<DomainClassMoniker Name="EntityTypeSubtypeInstance"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ValueTypeHasValueTypeInstance" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="E01B8EC6-F3BF-4963-92DB-7E352501C04D">
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

		<DomainRelationship Name="RoleInstance" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" AllowsDuplicates="true" InheritanceModifier="Abstract" Id="D3162C67-DE52-4B0D-802F-824E6ED5B74B">
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

		<DomainRelationship Name="EntityTypeRoleInstance" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" AllowsDuplicates="true" Id="5DB3A2C1-C5DE-4C4A-97C2-E09CE11537D3">
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

		<DomainRelationship Name="FactTypeRoleInstance" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" AllowsDuplicates="true" Id="FC7C9715-6886-46C2-A7A0-3BFD95CD0766">
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

		<DomainRelationship Name="EntityTypeInstanceHasRoleInstance" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="false" Id="05C64570-96FE-42C4-B9A6-F88D3BDC7C1F">
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

		<DomainRelationship Name="EntityTypeSubtypeInstanceHasSupertypeInstance" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="F11D087F-8B5B-4AC9-9B67-F967D5A5013E">
			<Source>
				<DomainRole Name="EntityTypeSubtypeInstance" PropertyName="SupertypeInstance" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="EntityTypeSubtypeInstance" Id="23F6CEC5-E016-40AD-A4D2-B684A9CA0231">
					<RolePlayer>
						<DomainClassMoniker Name="EntityTypeSubtypeInstance"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="SupertypeInstance" PropertyName="EntityTypeSubtypeInstanceCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SupertypeInstance" Id="BC2CD94C-D143-4676-8036-D4094A61B970">
					<RolePlayer>
						<DomainClassMoniker Name="EntityTypeInstance"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FactTypeHasFactTypeInstance" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="5283F53B-0DA8-4E4C-8A31-BDE51057E7EF">
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

		<DomainRelationship Name="FactTypeInstanceHasRoleInstance" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="false" Id="F92B6EC1-8055-4502-BD5D-763D1F5B6849">
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

		<DomainRelationship Name="ObjectificationInstance" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="false" Id="943F2CFD-0179-48D8-81D9-3B8277A8D449">
			<Source>
				<DomainRole Name="ObjectifiedInstance" PropertyName="ObjectifyingInstance" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ObjectifiedInstance" Id="819A1D2B-125B-4193-A336-25BE03EA91C3">
					<RolePlayer>
						<DomainClassMoniker Name="FactTypeInstance"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ObjectifyingInstance" PropertyName="ObjectifiedInstance" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ObjectifyingInstance" Id="326E4CF1-6A1F-41AB-B535-3A016CE59C7F">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectTypeInstance"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ObjectifiedInstanceHasObjectifyingInstanceRequiredError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="4C26F55B-D01B-4871-96C7-659FFB8448E9">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="FactTypeInstance" PropertyName="ObjectifyingInstanceRequiredError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactTypeInstance" Id="2D8DD1D8-131E-44E7-9030-E16CC588A0AA">
					<RolePlayer>
						<DomainClassMoniker Name="FactTypeInstance"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ObjectifyingInstanceRequiredError" PropertyName="FactTypeInstance" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ObjectifyingInstanceRequiredError" Id="0B7C29C1-067C-4986-9CB6-E485CD091C28">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectifyingInstanceRequiredError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ObjectifyingInstanceHasObjectifiedInstanceRequiredError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="A66FF864-B788-4287-B774-09BABE9E62B9">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="ObjectTypeInstance" PropertyName="ObjectifiedInstanceRequiredError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ObjectTypeInstance" Id="07CBB8AE-FC88-4F02-905E-2CE7258EBA46">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectTypeInstance"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ObjectifiedInstanceRequiredError" PropertyName="ObjectTypeInstance" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ObjectifiedInstanceRequiredError" Id="B8B818DD-B4F9-4641-992F-C405C45C6F8F">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectifiedInstanceRequiredError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="EntityTypeInstanceHasTooFewEntityTypeRoleInstancesError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="922E0A74-9384-4D25-9C38-E0AB709FEE8F">
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

		<DomainRelationship Name="FactTypeInstanceHasTooFewFactTypeRoleInstancesError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="6AC86DD8-1766-472E-B70F-B788C04ED688">
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

		<DomainRelationship Name="ValueTypeInstanceHasCompatibleValueTypeInstanceValueError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="A8AF2A8F-CDD0-41CB-B8CD-60CF28277288">
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

		<DomainRelationship Name="ConstraintRoleSequenceHasJoinPath" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="62FC7AC1-EB51-4887-81D4-15007D5FACBD">
			<Source>
				<DomainRole Name="RoleSequence" PropertyName="JoinPath" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="RoleSequence" Id="D204614A-A424-426B-9216-FAB21CD3BA9C">
					<RolePlayer>
						<DomainClassMoniker Name="ConstraintRoleSequence"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="JoinPath" PropertyName="RoleSequence" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="JoinPath" Id="B8D5101B-7904-41B6-B14C-5BE667B2A6BA">
					<RolePlayer>
						<DomainClassMoniker Name="ConstraintRoleSequenceJoinPath"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		
		<DomainRelationship Name="ConstraintRoleSequenceJoinPathProjection" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="9B90E59C-590B-4CBC-B7CF-604062E28A77">
			<Source>
				<DomainRole Name="JoinPath" PropertyName="ProjectedPathComponentCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="JoinPath" Id="8771F7B6-2544-46A6-BB63-A1C136B0F529">
					<RolePlayer>
						<DomainClassMoniker Name="ConstraintRoleSequenceJoinPath"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="PathComponent" PropertyName="ConstraintRoleSequenceJoinPathProjection" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="PathComponent" Id="0A6AD07C-880B-460E-AD75-136E3AB612AC">
					<RolePlayer>
						<DomainClassMoniker Name="RolePathComponent"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="ConstraintRoleProjection" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="B4DF8B28-6C80-4FAA-B081-A1F26C2B3BC0">
			<Source>
				<DomainRole Name="JoinPathProjection" PropertyName="ProjectedRoleCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="JoinPathProjection" Id="CAD5C4D6-E038-4EC9-BF02-26C07B918FD6">
					<RolePlayer>
						<DomainRelationshipMoniker Name="ConstraintRoleSequenceJoinPathProjection"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ProjectedConstraintRole" PropertyName="JoinPathProjectionCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ProjectedConstraintRole" Id="D943C94B-4A1F-4219-A33D-BC1DFE0B6AA4">
					<RolePlayer>
						<DomainRelationshipMoniker Name="ConstraintRoleSequenceHasRole"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="ConstraintRoleProjectedFromPathedRole" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="00D571D2-D924-4D40-8D69-299C5E18FC60">
			<Source>
				<DomainRole Name="ConstraintRoleProjection" PropertyName="ProjectedFromPathedRole" Multiplicity="ZeroOne" IsPropertyGenerator="true" DisplayName="ConstraintRoleProjection" Id="AA56EBC2-5647-44A7-8D1F-1CD454B6E6E4" Description="The pathed role in the join path associated with this constraint sequence.">
					<RolePlayer>
						<DomainRelationshipMoniker Name="ConstraintRoleProjection"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<!-- Although it is unusual to project the same data twice, it should not illegal. -->
				<DomainRole Name="Source" PropertyName="ConstraintRoleProjections" Multiplicity="ZeroMany" IsPropertyGenerator="true" DisplayName="Source" Id="688AF0D6-47BF-4049-A9D5-B31988E690D0" Description="The projected constraint role associated with this pathed role.">
					<RolePlayer>
						<DomainRelationshipMoniker Name="PathedRole"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="ConstraintRoleProjectedFromCalculatedPathValue" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="6228FFA8-9F2C-4A09-8938-1C07159C4953">
			<Source>
				<DomainRole Name="ConstraintRoleProjection" PropertyName="ProjectedFromCalculatedValue" Multiplicity="ZeroOne" IsPropertyGenerator="true" DisplayName="ConstraintRoleProjection" Id="4DE79423-24E0-4B1B-A76F-7D37FA39027D" Description="The calculated value in the join path associated with this constraint sequence.">
					<RolePlayer>
						<DomainRelationshipMoniker Name="ConstraintRoleProjection"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<!-- Although it is unusual to project the same data twice, it should not illegal. -->
				<DomainRole Name="Source" PropertyName="ConstraintRoleProjections" Multiplicity="ZeroMany" IsPropertyGenerator="true" DisplayName="Source" Id="D8A1DE32-1473-4DB7-8124-A5CE82FA8E69" Description="The projected constraint role associated with this calculated value.">
					<RolePlayer>
						<DomainClassMoniker Name="CalculatedPathValue"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="ConstraintRoleProjectedFromPathConstant" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="8AA8B309-4AE6-4B78-B38E-B0D9B27FA829">
			<Source>
				<DomainRole Name="ConstraintRoleProjection" PropertyName="ProjectedFromConstant" Multiplicity="ZeroOne" IsPropertyGenerator="true" DisplayName="ConstraintRoleProjection" Id="93FB3FBA-1F2A-4509-8B56-A109F02961E4" Description="The constant value associated with this constraint role.">
					<RolePlayer>
						<DomainRelationshipMoniker Name="ConstraintRoleProjection"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Source" PropertyName="ConstraintRoleProjection" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Source" Id="7B505588-0570-4AED-BBCF-4A8A79AD3ED7" Description="The constraint role that uses this path constant.">
					<RolePlayer>
						<DomainClassMoniker Name="PathConstant"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="ConstraintRoleProjectedFromPathedRole_Deprecated" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="AAF520DF-F858-4837-B070-CE6734BD154B">
			<Source>
				<DomainRole Name="ConstraintRole" PropertyName="ProjectedFromPathedRole" Multiplicity="ZeroOne" IsPropertyGenerator="false" DisplayName="ConstraintRole" Id="3C7C5B31-C245-4656-8BFF-7BF8961D5A33" Description="The pathed role in the join path associated with this constraint sequence.">
					<RolePlayer>
						<DomainRelationshipMoniker Name="ConstraintRoleSequenceHasRole"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<!-- Although it is unusual to project the same data twice, it should not illegal. -->
				<DomainRole Name="Source" PropertyName="ConstraintRoleProjections" Multiplicity="ZeroMany" IsPropertyGenerator="false" DisplayName="Source" Id="1420953D-B972-40B7-984D-3E97743421B3" Description="The projected constraint role associated with this pathed role.">
					<RolePlayer>
						<DomainRelationshipMoniker Name="PathedRole"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="ConstraintRoleProjectedFromCalculatedPathValue_Deprecated" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="06B5F374-2C58-498B-BEA9-B5BDF5861661">
			<Source>
				<DomainRole Name="ConstraintRole" PropertyName="ProjectedFromCalculatedValue" Multiplicity="ZeroOne" IsPropertyGenerator="false" DisplayName="ConstraintRole" Id="A847BBCD-0FAA-47F7-979C-0547613C398E" Description="The calculated value in the join path associated with this constraint sequence.">
					<RolePlayer>
						<DomainRelationshipMoniker Name="ConstraintRoleSequenceHasRole"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<!-- Although it is unusual to project the same data twice, it should not illegal. -->
				<DomainRole Name="Source" PropertyName="ConstraintRoleProjections" Multiplicity="ZeroMany" IsPropertyGenerator="false" DisplayName="Source" Id="56A95CD0-AA18-40B1-8D52-AB02A54D6E4B" Description="The projected constraint role associated with this calculated value.">
					<RolePlayer>
						<DomainClassMoniker Name="CalculatedPathValue"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="ConstraintRoleProjectedFromPathConstant_Deprecated" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="FD92B616-8995-4A10-949F-7E9F8B0E30CD">
			<Source>
				<DomainRole Name="ConstraintRole" PropertyName="ProjectedFromConstant" Multiplicity="ZeroOne" IsPropertyGenerator="false" DisplayName="ConstraintRole" Id="2DE9BCFE-120C-4C60-88DC-65574EFB82A7" Description="The constant value associated with this constraint role.">
					<RolePlayer>
						<DomainRelationshipMoniker Name="ConstraintRoleSequenceHasRole"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Source" PropertyName="ConstraintRoleProjection" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="Source" Id="BFC9BEFB-BE9A-4770-9CCC-165FB412DF17" Description="The constraint role that uses this path constant.">
					<RolePlayer>
						<DomainClassMoniker Name="PathConstant"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ModelHasModelErrorDisplayFilter" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="A8E175FA-A727-4909-8944-423EF0748E3D">
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
		<DomainRelationship Name="ElementGroupingSetRelatesToORMModel" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="ABDBCFC4-8861-4AE6-BCDC-5C5851713A74">
			<Source>
				<DomainRole Name="GroupingSet" PropertyName="Model" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="GroupingSet" Id="9E2BBC38-9220-424F-AAC6-B86D45D94599">
					<RolePlayer>
						<DomainClassMoniker Name="ElementGroupingSet"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Model" PropertyName="GroupingSet" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="Model" Id="A2D63832-F63B-4D4B-AFE8-1DBCC1CD897F">
					<RolePlayer>
						<DomainClassMoniker Name="ORMModel"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="ElementGroupingSetContainsElementGrouping" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" InheritanceModifier="Sealed" Id="A0B26EE8-E099-4E74-8EFF-5E7D2FA55B26">
			<Source>
				<DomainRole Name="GroupingSet" PropertyName="GroupingCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="GroupingSet" Id="02F5B2E6-9A76-43AC-B03D-9337461E0E90">
					<RolePlayer>
						<DomainClassMoniker Name="ElementGroupingSet"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Grouping" PropertyName="GroupingSet" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Grouping" Id="FE740A9A-EAC3-4A80-A52D-D4484003EC4A">
					<RolePlayer>
						<DomainClassMoniker Name="ElementGrouping"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="ElementGroupingIsOfElementGroupingType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="575C1C61-23F5-4B19-AE80-114E380D7E2A">
			<Source>
				<DomainRole Name="Grouping" PropertyName="GroupingTypeCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Grouping" Id="6F1BF1FE-60D8-48C5-BBD2-DEE006B4A045">
					<RolePlayer>
						<DomainClassMoniker Name="ElementGrouping"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="GroupingType" PropertyName="Grouping" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="GroupingType" Id="0BAFFAA0-D7EE-4E6D-98CA-D9AEFEBD4F1C">
					<RolePlayer>
						<DomainClassMoniker Name="ElementGroupingType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="GroupingElementRelationship" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" InheritanceModifier="Abstract" Id="5B5119D5-BD6A-41E5-8D42-17F25B51655D">
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeDescriptionProvider">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Core.ObjectModel.Design.GroupingElementRelationshipTypeDescriptionProvider)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
			<Source>
				<DomainRole Name="Grouping" PropertyName="ElementCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Grouping" Id="DAA7FBC4-68DC-4C03-875D-49AD7C245041">
					<RolePlayer>
						<DomainClassMoniker Name="ElementGrouping"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Element" PropertyName="GroupingCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="Element" Id="AE038BB0-4CE5-49FB-A113-6DE0DA3DC766">
					<RolePlayer>
						<DomainClassMoniker Name="/Microsoft.VisualStudio.Modeling/ModelElement"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="GroupingElementInclusion" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="C66C692D-6AA6-4FB9-901A-7E6C205AA272">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="GroupingElementRelationship"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="Grouping" PropertyName="IncludedElementCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Grouping" Id="80689CAC-F3EE-45C0-BE1E-3CECD0259971">
					<RolePlayer>
						<DomainClassMoniker Name="ElementGrouping"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="IncludedElement" PropertyName="GroupingCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="IncludedElement" Id="B7973813-1ED1-4440-A52B-E081749863EB">
					<RolePlayer>
						<DomainClassMoniker Name="/Microsoft.VisualStudio.Modeling/ModelElement"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="GroupingElementExclusion" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="CD51E687-6D5B-4102-B08C-78E1DA44BA38">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="GroupingElementRelationship"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="Grouping" PropertyName="ExcludedElementCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Grouping" Id="3C61D89A-2944-418A-853C-DAD62B114299">
					<RolePlayer>
						<DomainClassMoniker Name="ElementGrouping"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ExcludedElement" PropertyName="GroupingCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ExcludedElement" Id="531798A4-D4BD-4E95-AE2B-8E97E65A0249">
					<RolePlayer>
						<DomainClassMoniker Name="/Microsoft.VisualStudio.Modeling/ModelElement"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="ElementGroupingContainsElementGrouping" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" InheritanceModifier="Abstract" Id="7B1D7109-3AEA-406A-89E8-989DBB27A469">
			<Source>
				<DomainRole Name="ParentGrouping" PropertyName="ChildGroupingCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ParentGrouping" Id="73F0282C-73E5-4EED-BC79-9693A8215A8B">
					<RolePlayer>
						<DomainClassMoniker Name="ElementGrouping"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ChildGrouping" PropertyName="ParentGroupingCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ChildGrouping" Id="88F5E59F-24A3-4433-8B4F-10CB85807597">
					<RolePlayer>
						<DomainClassMoniker Name="ElementGrouping"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="ElementGroupingIncludesElementGrouping" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="76F5EA7D-2565-44C7-BA47-EB86FAB2A189">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementGroupingContainsElementGrouping"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="ParentGrouping" PropertyName="IncludedChildGroupingCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ParentGrouping" Id="231DA867-2184-43A2-B9CA-E51AFBC60CDF">
					<RolePlayer>
						<DomainClassMoniker Name="ElementGrouping"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="IncludedChildGrouping" PropertyName="IncludedParentGroupingCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ChildGrouping" Id="07E28D3D-B4D3-40BE-9CAA-0AFA622E26C8">
					<RolePlayer>
						<DomainClassMoniker Name="ElementGrouping"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="ElementGroupingExcludesElementGrouping" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="C710924E-194B-45CF-AEC2-B1EC0B86990C">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementGroupingContainsElementGrouping"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="ParentGrouping" PropertyName="ExcludedChildGroupingCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ParentGrouping" Id="40C3B2A3-48A2-45E6-9354-550E606A8E83">
					<RolePlayer>
						<DomainClassMoniker Name="ElementGrouping"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ExcludedChildGrouping" PropertyName="ExcludedParentGroupingCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ChildGrouping" Id="82E5050B-1847-4AD2-A26C-A95540D7C425">
					<RolePlayer>
						<DomainClassMoniker Name="ElementGrouping"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="ElementGroupingHasDuplicateNameError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="C87FAD64-4283-4710-86BA-2E1A11D9A551">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="Grouping" PropertyName="DuplicateNameError" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Grouping" Id="B7A2FAB0-ADFF-4F1D-A813-42CBAD17C40D">
					<RolePlayer>
						<DomainClassMoniker Name="ElementGrouping"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="DuplicateNameError" PropertyName="GroupingCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="DuplicateNameError" Id="D4F63EA1-5D4C-4829-9632-2D7CE502A16F">
					<RolePlayer>
						<DomainClassMoniker Name="ElementGroupingDuplicateNameError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="ElementGroupingHasMembershipContradictionError" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="FFA04DF2-2BCA-4290-8801-FA1947CCDBDA">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="ElementAssociatedWithModelError"/>
			</BaseRelationship>
			<Source>
				<DomainRole Name="Grouping" PropertyName="MembershipContradictionErrorCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Grouping" Id="CF43911D-1FF2-47AD-B451-4CB8F67B7805">
					<RolePlayer>
						<DomainClassMoniker Name="ElementGrouping"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="MembershipContradictionError" PropertyName="Grouping" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="MembershipContradictionError" Id="13DB78A4-0967-485F-B69F-5E3DB2875557">
					<RolePlayer>
						<DomainClassMoniker Name="ElementGroupingMembershipContradictionError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="GroupingMembershipContradictionErrorIsForElement" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="67A68DF7-1472-40B8-825A-1FC426A5E6C1">
			<Source>
				<!-- Node that this is modeled as a link-to-a-link so that we can get both the group and the element for a deleted error in an event situation -->
				<DomainRole Name="GroupingMembershipContradictionErrorRelationship" PropertyName="Element" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="GroupingMembershipContradictionErrorRelationship" Id="A316CBB8-E335-4A11-937A-0B166628EE19">
					<RolePlayer>
						<DomainRelationshipMoniker Name="ElementGroupingHasMembershipContradictionError"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Element" PropertyName="MembershipContradictionErrorCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="Element" Id="AE99D58E-9B14-4266-AFE0-8E7A94DBE3F8">
					<RolePlayer>
						<DomainClassMoniker Name="/Microsoft.VisualStudio.Modeling/ModelElement"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<!-- UNDONE: Format change, remove old role path constructs. -->
		<DomainRelationship Name="RolePathOwnerHasPathComponent_Deprecated" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="E915E71A-B11C-4732-86D7-35C7C1B132A4">
			<Source>
				<DomainRole Name="PathOwner" PropertyName="PathComponent" Multiplicity="ZeroOne" IsPropertyGenerator="false" DisplayName="PathOwner" Id="0A64FD0A-53C9-4E10-9CFE-003ED101107C">
					<RolePlayer>
						<DomainClassMoniker Name="RolePathOwner"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<!-- Leave this without a delete propagation so that we can easily change the component to a non-deprecated relationship -->
				<DomainRole Name="PathComponent" PropertyName="ParentOwner" Multiplicity="ZeroOne" IsPropertyGenerator="false" DisplayName="PathComponent" Id="3F31CFCC-88DA-44F8-ADB3-B4019283AD4A">
					<RolePlayer>
						<DomainClassMoniker Name="RolePathComponent"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="RolePathOwnerHasPathComponent" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="C4D6E714-1489-4BD0-B92B-061D494AB66C">
			<Source>
				<DomainRole Name="PathOwner" PropertyName="PathComponentCollection" Multiplicity="ZeroMany" IsPropertyGenerator="true" DisplayName="PathOwner" Id="7B7881DA-9325-4882-950A-FEC9A9CBA048">
					<RolePlayer>
						<DomainClassMoniker Name="RolePathOwner"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<!-- UNDONE: Format change, remove old role path constructs. Multiplicity here should be one when the deprecated embedding is removed. -->
				<DomainRole Name="PathComponent" PropertyName="PathOwner" Multiplicity="ZeroOne" IsPropertyGenerator="true" PropagatesDelete="true" DisplayName="PathComponent" Id="50D66006-E047-4C5E-B5AE-F1AD19C1BBB8">
					<RolePlayer>
						<DomainClassMoniker Name="RolePathComponent"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="RolePathOwnerHasSingleLeadRolePath" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="C77BEA97-713A-45C2-9EE3-AF6939C06A13" Description="Derived relationship based on RolePathOwnerHasPathComponent to determine path owners with a single root.">
			<Source>
				<DomainRole Name="PathOwner" PropertyName="SingleLeadRolePath" Multiplicity="ZeroOne" IsPropertyGenerator="true" DisplayName="PathOwner" Id="B79117D1-DEE0-4EBD-9D2A-D51A1CAB54D7">
					<RolePlayer>
						<DomainClassMoniker Name="RolePathOwner"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<!-- Leave this without a delete propagation so that we can easily change the component to a non-deprecated relationship -->
				<DomainRole Name="RolePath" PropertyName="PathOwner" Multiplicity="ZeroOne" IsPropertyGenerator="false" DisplayName="RolePath" Id="38B54CE8-5CFD-4CEC-9227-F09B07312C5A">
					<RolePlayer>
						<DomainClassMoniker Name="LeadRolePath"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<!-- UNDONE: Format change, remove old role path constructs. -->
		<DomainRelationship Name="RolePathCompositorHasPathComponent_Deprecated" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="7DC33C12-2B06-4C93-AF58-7BE89A8F66FA">
			<Source>
				<DomainRole Name="Compositor" PropertyName="PathComponentCollection" Multiplicity="ZeroMany" IsPropertyGenerator="false" DisplayName="Compositor" Id="E9444032-C71F-4CC8-83B1-3C5874FB5006">
					<RolePlayer>
						<DomainClassMoniker Name="RolePathCombination"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<!-- Leave this without a delete propagation so that we can easily change the component to a non-deprecated relationship -->
				<DomainRole Name="PathComponent" PropertyName="ParentCompositor" Multiplicity="ZeroOne" IsPropertyGenerator="false" DisplayName="PathComponent" Id="46D07B50-1979-4801-AFDC-A3675656B28C">
					<RolePlayer>
						<DomainClassMoniker Name="RolePathComponent"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="RolePathCombinationHasPathComponent" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="0FFBA6C3-82E6-462F-8569-211213B17235">
			<Properties>
				<DomainProperty Name="SetProjection" DefaultValue="false" DisplayName="DistinctRelations" Id="0F869575-7239-413B-929A-D36949974121" Description="The combination uses distinct results from the combined path.">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
			<Source>
				<DomainRole Name="Combination" PropertyName="PathComponentCollection" Multiplicity="ZeroMany" IsPropertyGenerator="true" DisplayName="Combination" Id="E43E6554-D1AB-480D-9B95-50D33079D43F">
					<RolePlayer>
						<DomainClassMoniker Name="RolePathCombination"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="PathComponent" PropertyName="RolePathCombinationCollection" Multiplicity="ZeroMany" IsPropertyGenerator="true" DisplayName="PathComponent" Id="619CF0C8-C13C-4859-BEAB-A36E67CDDC36">
					<RolePlayer>
						<DomainClassMoniker Name="RolePathComponent"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="RolePathCombinationHasCorrelation" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="D52BED9F-2867-49F2-A6E6-A79415C837C6">
			<Source>
				<DomainRole Name="Combination" PropertyName="CorrelationCollection" Multiplicity="ZeroMany" IsPropertyGenerator="true" DisplayName="Combination" Id="CCF622D2-2CEA-46EE-9EAB-9D11531F48E6">
					<RolePlayer>
						<DomainClassMoniker Name="RolePathCombination"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Correlation" PropertyName="Combination" Multiplicity="One" IsPropertyGenerator="true" DisplayName="Correlation" Id="A850C632-C1F6-4930-9C2E-7D8BFABB3C93">
					<RolePlayer>
						<DomainClassMoniker Name="RolePathCombinationCorrelation"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="RolePathCombinationCorrelationCorrelatesPathedRole" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="AD8CF1CB-5981-490E-A2BD-9EEF31EE0F39">
			<Source>
				<DomainRole Name="Correlation" PropertyName="CorrelatedRoleCollection" Multiplicity="ZeroMany" IsPropertyGenerator="true" DisplayName="Correlation" Id="B8D4EF53-2733-444D-BF4F-8783DD2EDBA4">
					<RolePlayer>
						<DomainClassMoniker Name="RolePathCombinationCorrelation"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="PathedRole" PropertyName="CombinationCorrelationCollection" Multiplicity="ZeroMany" IsPropertyGenerator="true" DisplayName="PathedRole" Id="F03FFA1C-B024-4874-B9F0-81F33B40AC6D">
					<RolePlayer>
						<DomainRelationshipMoniker Name="PathedRole"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="LeadRolePathHasRootObjectType" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="4FFD036F-FC35-41AF-A318-27DB84E2D7B4">
			<Source>
				<DomainRole Name="LeadRolePath" PropertyName="RootObjectType" Multiplicity="ZeroOne" IsPropertyGenerator="true" DisplayName="LeadRolePath" Id="1F8DDC17-4BE8-4BD6-87D2-960058BF9F5B">
					<RolePlayer>
						<DomainClassMoniker Name="LeadRolePath"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="RootObjectType" PropertyName="LeadRolePathCollection" Multiplicity="ZeroMany" IsPropertyGenerator="true" DisplayName="RootObjectType" Id="7C63FEDF-5F2C-4C95-82B5-D20AB15B2A03">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="PathedRole" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="8F28981E-E8B5-4205-82BA-2487B9B3EF13" DisplayName="PathedRole" AllowsDuplicates="true" Description="An occurrence of a &lt;see cref=&quot;Role&quot;/&gt; in a &lt;see cref=&quot;RolePath&quot;/&gt;. A single role may occur multiple times in the same path.">
			<Properties>
				<DomainProperty Name="IsNegated" DefaultValue="False" DisplayName="IsNegated" Id="1260F1F6-15BB-425A-8FF7-B7841E8252AD" Description="Indicates that this step in the path is negated.">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="PathedRolePurpose" DefaultValue="SameFactType" DisplayName="PathedRolePurpose" Id="FFAF4EF9-CA23-4D14-BB6D-8F5B3C90E680" Description="Specifies how this pathed role relates to the previous pathed role.">
					<Type>
						<DomainEnumerationMoniker Name="PathedRolePurpose"/>
					</Type>
				</DomainProperty>
			</Properties>
			<Source>
				<DomainRole Name="RolePath" PropertyName="RoleCollection" Multiplicity="ZeroMany" IsPropertyGenerator="true" DisplayName="RolePath" Id="E1A7228B-6F79-42FB-8877-803ADCFBB54A" Description="The roles included in this path.">
					<RolePlayer>
						<DomainClassMoniker Name="RolePath"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Role" PropertyName="RolePathCollection" Multiplicity="ZeroMany" IsPropertyGenerator="true" DisplayName="Role" Id="18A191C8-4202-4DC2-A40D-9A75F5E6117F" Description="The role paths that reference this role.">
					<RolePlayer>
						<DomainClassMoniker Name="Role"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="PathedRoleIsRemotelyCorrelatedWithPathedRole" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="8019DBA6-0330-4DD5-ABC8-27197537008D">
			<Source>
				<DomainRole Name="CorrelatedChild" PropertyName="CorrelatingParent" Multiplicity="ZeroOne" IsPropertyGenerator="true" DisplayName="CorrelatedChild" Id="9071C1B5-9D76-496B-B3DC-5ACCDA9A3D23" Description="The parent node this pathed role is correlated with.">
					<RolePlayer>
						<DomainRelationshipMoniker Name="PathedRole"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="CorrelatingParent" PropertyName="CorrelatedChildCollection" Multiplicity="ZeroMany" IsPropertyGenerator="true" DisplayName="CorrelatingParent" Id="6EFF9DCC-4BB7-4406-9E90-794681036C51" Description="All pathed roles that are directly correlated with this pathed role.">
					<RolePlayer>
						<DomainRelationshipMoniker Name="PathedRole"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="RoleSubPathIsContinuationOfRolePath" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="79BE9E2C-9E3F-4EA0-957C-4D4EAD2389B2">
			<Source>
				<DomainRole Name="ParentRolePath" PropertyName="SplitPathCollection" Multiplicity="ZeroMany" IsPropertyGenerator="true" DisplayName="ParentRolePath" Id="97C4E687-6FF4-403A-AA36-376476CBA5D4" Description="Sub paths branched from the end of the current path.">
					<RolePlayer>
						<DomainClassMoniker Name="RolePath"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="SubPath" PropertyName="ParentRolePath" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="RoleSubPath" Id="7EA4F7E9-149F-4DB9-8307-2E561ADCFC8A" Description="The containing path this sub path branches off of.">
					<RolePlayer>
						<DomainClassMoniker Name="RoleSubPath"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="ModelDefinesFunction" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="1179E7FD-F395-4BE3-9615-E4F0ED0A09CD">
			<Source>
				<DomainRole Name="Model" PropertyName="FunctionCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="520D2EE0-8507-4EFD-A0D6-A6A8E7FB2D84" Description="Function definitions used for calculated role path values.">
					<RolePlayer>
						<DomainClassMoniker Name="ORMModel"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Function" PropertyName="Model" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Function" Id="176ECDCE-FE02-4563-9C88-BC598E963400" Description="The model defining this function.">
					<RolePlayer>
						<DomainClassMoniker Name="Function"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="FunctionOperatesOnParameter" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="717BFBDA-13E0-4868-ADC3-FF64A45D4CE9">
			<Source>
				<DomainRole Name="Function" PropertyName="ParameterCollection" Multiplicity="ZeroMany" IsPropertyGenerator="true" DisplayName="Function" Id="F519D4E8-375B-4781-8D43-07B9132E9CE6" Description="Parameters defined by this function.">
					<RolePlayer>
						<DomainClassMoniker Name="Function"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Parameter" PropertyName="Function" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Parameter" Id="5605B4DC-2009-49F1-B5B0-714A8F34600A" Description="The function this parameter is defined for.">
					<RolePlayer>
						<DomainClassMoniker Name="FunctionParameter"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="RolePathOwnerCalculatesCalculatedPathValue_Deprecated" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="CC8D4B04-3F04-4C0C-995B-E9B24E3134FB">
			<Source>
				<DomainRole Name="PathOwner" PropertyName="CalculatedValueCollection" Multiplicity="ZeroMany" IsPropertyGenerator="false" DisplayName="PathOwner" Id="60542704-5E9E-4F7C-B0EB-29ECB9C3DF46" Description="The values calculated for all paths in this owner.">
					<RolePlayer>
						<DomainClassMoniker Name="RolePathOwner"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<!-- Eliminate delete propagation so this can easily be moved to another container. -->
				<DomainRole Name="CalculatedValue" PropertyName="PathOwner" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="CalculatedValue" Id="455B7520-EED3-489B-95A6-7EA07B7FAA0A" Description="The primary role path this value is calculated for.">
					<RolePlayer>
						<DomainClassMoniker Name="CalculatedPathValue"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="RolePathComponentCalculatesCalculatedPathValue" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="315ED779-4607-44CB-AC37-A173FA106232">
			<Source>
				<DomainRole Name="PathComponent" PropertyName="CalculatedValueCollection" Multiplicity="ZeroMany" IsPropertyGenerator="true" DisplayName="PathOwner" Id="058C0609-8B18-4C05-86DB-7ABA5D7C42DA" Description="The values calculated using roles in this component.">
					<RolePlayer>
						<DomainClassMoniker Name="RolePathComponent"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<!-- UNDONE: Format change, remove old role path constructs. Multiplicity here should be one when the deprecated embedding is removed. -->
				<DomainRole Name="CalculatedValue" PropertyName="PathComponent" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="CalculatedValue" Id="9C87C629-060B-4C80-BEAE-DCA38A4F4E2C" Description="The path component this value is calculated for.">
					<RolePlayer>
						<DomainClassMoniker Name="CalculatedPathValue"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="RolePathComponentSatisfiesCalculatedCondition" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="34F4B6C3-1575-4A4C-838E-261B981DEE83">
			<Source>
				<DomainRole Name="PathComponent" PropertyName="CalculatedConditionCollection" Multiplicity="ZeroMany" IsPropertyGenerator="true" DisplayName="PathComponent" Id="2F17D947-3334-4729-BAF1-CE5EFAE039EE" Description="The calculated values that must be satisfied by the path.">
					<RolePlayer>
						<DomainClassMoniker Name="RolePathComponent"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="CalculatedCondition" PropertyName="RequiredForPathCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="CalculatedCondition" Id="18F0E45A-F8F2-4AE5-919C-C5946C7193BD" Description="The primary role path that requires this condition to be true.">
					<RolePlayer>
						<DomainClassMoniker Name="CalculatedPathValue"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="CalculatedPathValueHasInput" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="EEF80C4B-8D52-441E-896A-D3BF11724028">
			<Source>
				<DomainRole Name="CalculatedValue" PropertyName="InputCollection" Multiplicity="ZeroMany" IsPropertyGenerator="true" DisplayName="CalculatedValue" Id="5E165681-2C7A-44B2-AAC5-982A36829790" Description="Inputs used to calculate this value.">
					<RolePlayer>
						<DomainClassMoniker Name="CalculatedPathValue"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Input" PropertyName="CalculatedValue" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Input" Id="EC5A5FB9-BEBD-4463-8549-15A0AE77E99A" Description="The calculated value that owns this input for.">
					<RolePlayer>
						<DomainClassMoniker Name="CalculatedPathValueInput"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="CalculatedPathValueScopedWithPathedRole" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="627FCA97-86EF-473F-AAA7-FFF2F8295624">
			<Source>
				<DomainRole Name="CalculatedValue" PropertyName="Scope" Multiplicity="ZeroOne" IsPropertyGenerator="true" DisplayName="CalculatedValue" Id="63C8F93E-641D-4AA5-B42B-1A1E0C4AE9A4" Description="The PathedRole that provides context for this function. The scope must be related in the role path to any path inputs.">
					<RolePlayer>
						<DomainClassMoniker Name="CalculatedPathValue"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Scope" PropertyName="ScopedCalculatedValueCollection" Multiplicity="ZeroMany" IsPropertyGenerator="true" DisplayName="Input" Id="AE6B0A16-55A0-4805-BDDC-35C45C202A30" Description="Calculated values that are scoped using this PathedRole.">
					<RolePlayer>
						<DomainRelationshipMoniker Name="PathedRole"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="CalculatedPathValueIsCalculatedWithFunction" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="B0BB8774-8BF6-440A-ADDA-FA9DB0712824">
			<Source>
				<DomainRole Name="CalculatedValue" PropertyName="Function" Multiplicity="One" IsPropertyGenerator="true" DisplayName="CalculatedValue" Id="620BE174-61D3-48B5-99C7-17F2B7A8694D" Description="The function used to calculate this value.">
					<RolePlayer>
						<DomainClassMoniker Name="CalculatedPathValue"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Function" PropertyName="CalculatedValueCollection" Multiplicity="ZeroMany" IsPropertyGenerator="true" DisplayName="Function" Id="C1C8CCCF-1406-4BAC-B736-9AA2BCC5B07D" Description="The calculated values based on this function.">
					<RolePlayer>
						<DomainClassMoniker Name="Function"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="CalculatedPathValueInputCorrespondsToFunctionParameter" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="441EAE3E-ED05-45B5-B532-17833234930E">
			<Source>
				<DomainRole Name="Input" PropertyName="Parameter" Multiplicity="One" IsPropertyGenerator="true" DisplayName="Input" Id="B70D105D-8639-41AA-9CE8-135BB3CD6F51" Description="The function parameter associated with this input value.">
					<RolePlayer>
						<DomainClassMoniker Name="CalculatedPathValueInput"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Parameter" PropertyName="CalculatedInputCollection" Multiplicity="ZeroMany" IsPropertyGenerator="true" DisplayName="Parameter" Id="EEB1F768-E8D3-49D0-88F8-8DCFF8F87300" Description="The calculated value inputs that use this parameter.">
					<RolePlayer>
						<DomainClassMoniker Name="FunctionParameter"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="CalculatedPathValueInputBindsToPathedRole" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="520DE89C-FA70-46F1-9BE2-03B4D361371A">
			<Source>
				<DomainRole Name="Input" PropertyName="SourcePathedRole" Multiplicity="ZeroOne" IsPropertyGenerator="true" DisplayName="Input" Id="2F9FB7FA-6381-4EF7-B910-B6EC54D34627" Description="The pathed value bound to this function input.">
					<RolePlayer>
						<DomainClassMoniker Name="CalculatedPathValueInput"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Source" PropertyName="BoundInputCollection" Multiplicity="ZeroMany" IsPropertyGenerator="true" DisplayName="Source" Id="391D3273-F1A5-4FC5-814B-5155BD93D92B" Description="The calculated value inputs bound to this path node.">
					<RolePlayer>
						<DomainRelationshipMoniker Name="PathedRole"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="CalculatedPathValueInputBindsToCalculatedPathValue" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="FB107CB5-6A10-4F58-9BD3-F57400AC0352">
			<Source>
				<DomainRole Name="Input" PropertyName="SourceCalculatedValue" Multiplicity="ZeroOne" IsPropertyGenerator="true" DisplayName="Input" Id="C2EFADA8-08E7-4703-8DE5-0456C5DEF1A1" Description="The pathed value bound to this function input.">
					<RolePlayer>
						<DomainClassMoniker Name="CalculatedPathValueInput"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Source" PropertyName="BoundInputCollection" Multiplicity="ZeroMany" IsPropertyGenerator="true" DisplayName="Source" Id="F261C4C8-0974-4690-901F-FC558B14021A" Description="The calculated value inputs bound to this path node.">
					<RolePlayer>
						<DomainClassMoniker Name="CalculatedPathValue"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="CalculatedPathValueInputBindsToPathConstant" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="365F3AB4-212D-4205-BFBC-4B9482E9E6B3">
			<Source>
				<DomainRole Name="Input" PropertyName="SourceConstant" Multiplicity="ZeroOne" IsPropertyGenerator="true" DisplayName="Input" Id="5D3FE47E-C811-43F9-BFC9-ED0635BEC71B" Description="The constant value bound to this function input.">
					<RolePlayer>
						<DomainClassMoniker Name="CalculatedPathValueInput"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Source" PropertyName="BoundInput" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Source" Id="7FDD0F7D-FD00-4A77-88B2-35CDCAD1F211" Description="The calculated value input that uses this path constant.">
					<RolePlayer>
						<DomainClassMoniker Name="PathConstant"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="FactTypeHasDerivationRule" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="BB165F20-D91A-44E3-AED4-687E4C2D6474">
			<Source>
				<DomainRole Name="FactType" PropertyName="DerivationRule" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="8991A795-F786-42B6-ADFE-2645E4FCF91E">
					<RolePlayer>
						<DomainClassMoniker Name="FactType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="DerivationRule" PropertyName="FactType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="DerivationRule" Id="5F0E53BF-A6D2-439A-90B9-465A5E85A7DD">
					<RolePlayer>
						<DomainClassMoniker Name="FactTypeDerivationRule"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="FactTypeDerivationProjection" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="029B0F47-FA95-4ED3-848B-239FDBCEBAF8">
			<Source>
				<DomainRole Name="DerivationRule" PropertyName="ProjectedPathComponentCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="DerivationRule" Id="93BC60F8-A436-406A-B7F3-1200360C34D9">
					<RolePlayer>
						<DomainClassMoniker Name="FactTypeDerivationRule"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="PathComponent" PropertyName="FactTypeDerivationRuleProjection" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="PathComponent" Id="B92067D1-2DD4-4E80-A667-FCF437C84EF8">
					<RolePlayer>
						<DomainClassMoniker Name="RolePathComponent"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="FactTypeRoleProjection" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="E4E47551-0637-443F-ADB0-4BE9CAD921F0">
			<Source>
				<DomainRole Name="DerivationProjection" PropertyName="ProjectedRoleCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="DerivationProjection" Id="F0996660-410E-4147-AD3C-EA5C6629DBB5">
					<RolePlayer>
						<DomainRelationshipMoniker Name="FactTypeDerivationProjection"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ProjectedRole" PropertyName="DerivationProjectionCollection" Multiplicity="ZeroMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ProjectedRole" Id="671F08BD-3497-4B45-98D2-D412750EBBBE">
					<RolePlayer>
						<DomainClassMoniker Name="Role"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="FactTypeRoleProjectedFromPathedRole" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="16C19D21-B699-45E7-BCB3-62649747F94B">
			<Source>
				<DomainRole Name="RoleProjection" PropertyName="ProjectedFromPathedRole" Multiplicity="ZeroOne" IsPropertyGenerator="true" DisplayName="RoleProjection" Id="4025B3A1-84D7-4D17-B1EC-D34FDAAC5E51" Description="The pathed role used to populate the derived fact type for this role in this projection.">
					<RolePlayer>
						<DomainRelationshipMoniker Name="FactTypeRoleProjection"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<!-- Although it is unusual to derive two columns from the same source, it should not illegal. -->
				<DomainRole Name="Source" PropertyName="FactTypeRoleProjections" Multiplicity="ZeroMany" IsPropertyGenerator="false" DisplayName="Source" Id="A13B7D15-8EC9-4775-B9CB-FF041B8652AC" Description="The derived role associated with this pathed role.">
					<RolePlayer>
						<DomainRelationshipMoniker Name="PathedRole"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="FactTypeRoleProjectedFromCalculatedPathValue" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="37216599-11AA-4D3A-90EB-010D21B7E3AB">
			<Source>
				<DomainRole Name="RoleProjection" PropertyName="ProjectedFromCalculatedValue" Multiplicity="ZeroOne" IsPropertyGenerator="true" DisplayName="RoleProjection" Id="DC6C1E96-1D32-4600-8745-85493C7C2088" Description="The calculated value used to populate the derived fact type for this role in this projection.">
					<RolePlayer>
						<DomainRelationshipMoniker Name="FactTypeRoleProjection"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<!-- Although it is unusual to derive two columns from the same source, it should not illegal. -->
				<DomainRole Name="Source" PropertyName="FactTypeRoleProjections" Multiplicity="ZeroMany" IsPropertyGenerator="false" DisplayName="Source" Id="AE9C159E-6909-46CF-8635-059DE2B9E7F3" Description="The derived role associated with this calculated value.">
					<RolePlayer>
						<DomainClassMoniker Name="CalculatedPathValue"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="FactTypeRoleProjectedFromPathConstant" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="205ED2D0-43CE-4141-9A3F-5C33138AD048">
			<Source>
				<DomainRole Name="RoleProjection" PropertyName="ProjectedFromConstant" Multiplicity="ZeroOne" IsPropertyGenerator="true" DisplayName="RoleProjection" Id="AD816A27-E687-46EC-9240-F1C69EDCF9DB" Description="The constant value used to populate this role in the derived fact type.">
					<RolePlayer>
						<DomainRelationshipMoniker Name="FactTypeRoleProjection"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Source" PropertyName="FactTypeRoleProjection" Multiplicity="ZeroOne" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Source" Id="F798AF86-1E64-4789-A840-05B615404544" Description="The derived role that uses this path constant.">
					<RolePlayer>
						<DomainClassMoniker Name="PathConstant"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="RoleDerivesFromPathedRole_Deprecated" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="C57BC0E8-04B7-4A3A-B021-3A6437969762">
			<Source>
				<DomainRole Name="Role" PropertyName="DerivedFromPathedRole" Multiplicity="ZeroOne" IsPropertyGenerator="false" DisplayName="Role" Id="FA366136-A169-4509-BB6A-6028E7886A13" Description="The pathed role used to populate the derived fact type for this role.">
					<RolePlayer>
						<DomainClassMoniker Name="Role"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<!-- Although it is unusual to derive two columns from the same source, it should not illegal. -->
				<DomainRole Name="Source" PropertyName="DerivedRoles" Multiplicity="ZeroMany" IsPropertyGenerator="false" DisplayName="Source" Id="E71D53D7-C4A2-4367-A2F3-5A27CE70DCE0" Description="The derived role associated with this pathed role.">
					<RolePlayer>
						<DomainRelationshipMoniker Name="PathedRole"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="RoleDerivesFromCalculatedPathValue_Deprecated" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Id="A9FEB04A-4C32-4576-95C9-500B6B77CA03">
			<Source>
				<DomainRole Name="Role" PropertyName="DerivedFromCalculatedValue" Multiplicity="ZeroOne" IsPropertyGenerator="false" DisplayName="Role" Id="BAA7CAA2-A8C0-49E0-8965-C3B8F1CA8C8A" Description="The calculated value used to populate the derived fact type for this role.">
					<RolePlayer>
						<DomainClassMoniker Name="Role"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<!-- Although it is unusual to derive two columns from the same source, it should not illegal. -->
				<DomainRole Name="Source" PropertyName="DerivedRoles" Multiplicity="ZeroMany" IsPropertyGenerator="false" DisplayName="Source" Id="0C4A9FEC-D093-43B9-89E5-08B892FC443B" Description="The derived role associated with this calculated value.">
					<RolePlayer>
						<DomainClassMoniker Name="CalculatedPathValue"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="RoleDerivesFromPathConstant_Deprecated" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="1DC37758-2350-4476-883C-5F971AE32B34">
			<Source>
				<DomainRole Name="Role" PropertyName="DerivedFromConstant" Multiplicity="ZeroOne" IsPropertyGenerator="false" DisplayName="Role" Id="B4D23A52-418B-429A-B425-91222DA61D9B" Description="The constant value used to populate this role in the derived fact type.">
					<RolePlayer>
						<DomainClassMoniker Name="Role"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<!-- Omit delete propagation to enable easily moving this constant to another owning relationship. -->
				<DomainRole Name="Source" PropertyName="DerivedRole" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="Source" Id="06D66903-36CD-427B-9473-03FF462B4650" Description="The derived role that uses this path constant.">
					<RolePlayer>
						<DomainClassMoniker Name="PathConstant"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		<DomainRelationship Name="SubtypeHasDerivationRule" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" IsEmbedding="true" Id="54240547-EBF8-4235-8C09-BB3E0876511A">
			<Source>
				<DomainRole Name="Subtype" PropertyName="DerivationRule" Multiplicity="ZeroOne" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Subtype" Id="EEBCAF86-7B3C-4E5B-A4AB-DACE7547947F">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="DerivationRule" PropertyName="Subtype" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="DerivationRule" Id="556D13CB-9B64-46D7-8D2C-312D3F30CC75">
					<RolePlayer>
						<DomainClassMoniker Name="SubtypeDerivationRule"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
	</Relationships>

	<Types>
		<ExternalType Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Name="FactType"/>
		<ExternalType Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Name="ReferenceMode"/>
		<ExternalType Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Name="DataType"/>
		<ExternalType Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Name="ModelErrorDisplayFilter"/>
		<ExternalType Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Name="ObjectType"/>
		<ExternalType Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Name="ReferenceModeKind"/>
		<DomainEnumeration Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Name="DerivationExpressionStorageType" Description="Specify how/whether the contents of the fact should be stored by generated systems.">
			<Literals>
				<EnumerationLiteral Name="Derived" Value="0" Description="The fact instance population is calculated on demand."/>
				<EnumerationLiteral Name="DerivedAndStored" Value="1" Description="The fact instance population is calculated immediately and stored."/>
				<EnumerationLiteral Name="PartiallyDerived" Value="2" Description="The fact instance population can be asserted as well as calculated on demand."/>
				<EnumerationLiteral Name="PartiallyDerivedAndStored" Value="3" Description="The fact instance population can be asserted as well as calculated immediately and stored."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.EnumConverter&lt;DerivationExpressionStorageType, global::ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Name="DerivationCompleteness" Description="Specify if instances of a derived fact can also be directly asserted.">
			<Literals>
				<EnumerationLiteral Name="FullyDerived" Value="0" Description="The fact instance population is calculated on demand."/>
				<EnumerationLiteral Name="PartiallyDerived" Value="1" Description="The fact instance population can be both calculated and asserted."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.EnumConverter&lt;DerivationCompleteness, global::ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Name="DerivationStorage" Description="Specify if derived fact instances should be recalculated on demand or calculated on change and stored.">
			<Literals>
				<EnumerationLiteral Name="NotStored" Value="0" Description="Fact instances are recalculated on demand."/>
				<EnumerationLiteral Name="Stored" Value="1" Description="Fact instances are calculated on change and stored."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.EnumConverter&lt;DerivationStorage, global::ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Name="ConstraintModality">
			<Literals>
				<EnumerationLiteral Name="Alethic" Value="0" Description="The constraint must hold."/>
				<EnumerationLiteral Name="Deontic" Value="1" Description="The constraint should hold."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.EnumConverter&lt;ConstraintModality, global::ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Name="RoleMultiplicity" Description="Defines the multiplicity for the roles. The role multiplicity is currently displayed only on roles associated with binary fact types and is calculated based on the existing mandatory and internal uniqueness constraints associated with the fact.">
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
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.EnumConverter&lt;RoleMultiplicity, global::ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Name="RingConstraintType">
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
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.EnumConverter&lt;RingConstraintType, global::ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Name="ReferenceModeType">
			<Literals>
				<EnumerationLiteral Name="General" Value="0" Description="That other reference mode type."/>
				<EnumerationLiteral Name="Popular" Value="1" Description="The 'in' and 'fashionable' reference mode type."/>
				<EnumerationLiteral Name="UnitBased" Value="2" Description="The reference mode type based on units."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.EnumConverter&lt;ReferenceModeType, global::ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Name="RangeInclusion">
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
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.EnumConverter&lt;RangeInclusion, global::ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>

		<DomainEnumeration Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Name="LogicalCombinationOperator">
			<Literals>
				<EnumerationLiteral Name="And" Value="0" Description="The logical and operator"/>
				<EnumerationLiteral Name="Or" Value="1" Description="The logical inclusive-or operator"/>
				<EnumerationLiteral Name="Xor" Value="2" Description="The logical exclusive-or operator"/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.EnumConverter&lt;LogicalCombinationOperator, global::ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>

		<DomainEnumeration Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Name="PathedRolePurpose">
			<Literals>
				<EnumerationLiteral Name="StartRole" Value="0" Description="The role is the beginning of a path and directly attached to the root object type."/>
				<EnumerationLiteral Name="SameFactType" Value="1" Description="The role is the same fact type as the previous join or start role."/>
				<EnumerationLiteral Name="PostInnerJoin" Value="2" Description="The role represents an inner over a role player shared with the previous role in the path."/>
				<EnumerationLiteral Name="PostOuterJoin" Value="3" Description="The role represents an outer join over a role player shared with the previous role in the path."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.EnumConverter&lt;PathedRolePurpose, global::ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>

		<DomainEnumeration Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Name="NameGeneratorCasingOption">
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
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.EnumConverter&lt;NameGeneratorCasingOption, global::ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>

		<DomainEnumeration Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Name="NameGeneratorSpacingFormat">
			<Literals>
				<EnumerationLiteral Name="Retain" Value="0"/>
				<EnumerationLiteral Name="Remove" Value="1" Description="Indicates that spaces are Removed"/>
				<EnumerationLiteral Name="ReplaceWith" Value="2" Description="Indicates that spaces are ReplacedWith a different string."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.EnumConverter&lt;NameGeneratorSpacingFormat, global::ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>

		<DomainEnumeration Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Name="NameGeneratorSubjectArea">
			<Literals>
				<EnumerationLiteral Name="None" Value="0"/>
				<EnumerationLiteral Name="Prefix" Value="1" Description="Indicates that the chosen subject will be prepended to the generated name."/>
				<EnumerationLiteral Name="Suffix" Value="2" Description="Indicates that the chosen subject will be appended to the generated name."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.EnumConverter&lt;NameGeneratorSubjectArea, global::ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>

		<DomainEnumeration Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel" Name="GroupingMembershipTypeCompliance">
			<Literals>
				<EnumerationLiteral Name="NotExcluded" Value="0" Description="Allow all elements that are not explicitly excluded by a GroupType."/>
				<EnumerationLiteral Name="PartiallyApproved" Value="1" Description="Allow all elements that are explicitly approved by at least one GroupType."/>
				<EnumerationLiteral Name="FullyApproved" Value="2" Description="Allow elements that are explicitly approved by all GroupTypes."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::ORMSolutions.ORMArchitect.Framework.Design.EnumConverter&lt;GroupingMembershipTypeCompliance, global::ORMSolutions.ORMArchitect.Core.ObjectModel.ORMModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
	</Types>

	<XmlSerializationBehavior Name="ORMCoreDomainModelSerializationBehavior" Namespace="ORMSolutions.ORMArchitect.Core.ObjectModel"/>

</Dsl>
