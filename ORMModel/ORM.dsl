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
	Name="ORMMetaModel"
	DisplayName="ORMMetaModel"
	CompanyName="Neumont University"
	ProductName="Neumont ORM Architect for Visual Studio"
	MajorVersion="1" MinorVersion="0" Build="0" Revision="0">

	<Classes>

		<DomainClass Name="ORMModelElement" Namespace="Neumont.Tools.ORM.ObjectModel" Id="BFBBEE5E-C691-4299-B958-77AC1B701F28" DisplayName="ORMModelElement" InheritanceModifier="Abstract" Description=""/>

		<DomainClass Name="ORMNamedElement" Namespace="Neumont.Tools.ORM.ObjectModel" Id="C2BE18BA-BC16-4764-BAA1-18E721435BCE" DisplayName="ORMNamedElement" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Name" DefaultValue="" DisplayName="Name" IsElementName="true" Id="4A557C1E-0A89-49B7-B4BD-FA095F6267D7">
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
			</ElementMergeDirectives>
		</DomainClass>


		<DomainClass Name="ObjectType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="2FED415E-1786-4FBF-8556-A507F2F765FD" DisplayName="ObjectType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMNamedElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="IsExternal" DefaultValue="false" DisplayName="IsExternal" Id="D03828FD-1DA7-4804-A16B-CC27F2046F57">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="NoteText" DefaultValue="" DisplayName="NoteText" Id="17C4E23D-CA49-4329-982F-48F4EFCA23BD" Kind="CustomStorage">
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
				<DomainProperty Name="Scale" DefaultValue="0" DisplayName="Scale" Id="BD2D708A-7687-4218-94BC-05834AFAC869" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="Length" DefaultValue="0" DisplayName="Length" Id="C9B01797-2CA1-4FF8-865A-FDA0DDF33F8D" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="NestedFactTypeDisplay" DefaultValue="" DisplayName="NestedFactType" Id="C003F8F1-368C-4058-A5F8-90EF63556743" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.Design.NestedFactTypePicker)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.MergableProperty">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.Design.ExpandableElementConverter)"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/Neumont.Tools.ORM.ObjectModel/FactType"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="ReferenceModeDisplay" DefaultValue="" DisplayName="RefMode" Id="2E56D25A-BD96-4478-A55C-9F17A15C94B6" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.Design.ReferenceModePicker)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.Design.ReferenceModeConverter)"/>
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
				<DomainProperty Name="ReferenceMode" DefaultValue="" DisplayName="ReferenceMode" Id="E2049BA3-F50D-4E1C-9ABD-8A7EBECFEDF5" IsBrowsable="false" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/Neumont.Tools.ORM.ObjectModel/ReferenceMode"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="DataTypeDisplay" DefaultValue="" DisplayName="DataType" Id="3E8893A7-5985-4200-A595-CB1E9EC9ADA7" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.Design.DataTypePicker)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/Neumont.Tools.ORM.ObjectModel/DataType"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="ValueRangeText" DefaultValue="" DisplayName="ValueRangeText" Id="F0662C59-700B-435C-B57B-93E5FD84B71F" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="IsPersonal" DefaultValue="false" DisplayName="IsPersonal" Id="EF9AE461-4327-46DC-8FE0-D1388F061B30">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
		</DomainClass>

		<DomainClass Name="FactType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="723A2B28-6CDA-4185-B597-87866E257265" DisplayName="FactType" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="IsExternal" DefaultValue="false" DisplayName="IsExternal" Id="67EA8C95-FD9A-473B-8AA2-E35FCDD68361">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="NoteText" DefaultValue="" DisplayName="NoteText" Id="AF6200B1-068D-434A-98D3-44E872B921BD" Kind="CustomStorage">
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
				<DomainProperty Name="NameChanged" DefaultValue="" DisplayName="NameChanged" Id="20A75B4B-69D4-4D1B-BEB5-9B0D66FDB1F3" IsBrowsable="false" Kind="CustomStorage">
					<Type>
						<ExternalTypeMoniker Name="/System/Int64"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="NestingTypeDisplay" DefaultValue="" DisplayName="NestingType" Id="08C9243D-38BA-41E5-9864-5BBB8977B676" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.Design.NestingTypePicker)"/>
								<AttributeParameter Value="typeof(global::System.Drawing.Design.UITypeEditor)"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.MergableProperty">
							<Parameters>
								<AttributeParameter Value="false"/>
							</Parameters>
						</ClrAttribute>
						<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.Design.ExpandableElementConverter)"/>
							</Parameters>
						</ClrAttribute>
					</Attributes>
					<Type>
						<ExternalTypeMoniker Name="/Neumont.Tools.ORM.ObjectModel/ObjectType"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="DerivationRuleDisplay" DefaultValue="" DisplayName="DerivationRule" Id="7AF5C436-C28A-49BA-B8E0-05C409B67358" Kind="CustomStorage">
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

		<DomainClass Name="SubtypeFact" Namespace="Neumont.Tools.ORM.ObjectModel" Id="7A957450-AD7E-4C29-AF59-A10F8C8052CC" DisplayName="SubtypeFact">
			<BaseClass>
				<DomainClassMoniker Name="FactType"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="IsPrimary" DefaultValue="false" DisplayName="IsPrimary" Id="9A2A6585-7CAA-41F9-8117-9F357A6C3626">
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
			<BaseClass>
				<DomainClassMoniker Name="RoleBase"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="RolePlayerDisplay" DefaultValue="" DisplayName="RolePlayer" Id="B66FCA99-E6EC-46C9-B445-D549F6D7ABE1" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.Design.RolePlayerPicker)"/>
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
				<DomainProperty Name="ValueRangeText" DefaultValue="" DisplayName="ValueRangeText" Id="3882C0AC-6F4A-4CF1-B856-E57A2DD4650C" Kind="CustomStorage">
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
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="ObjectificationOppositeRoleName" DefaultValue="" DisplayName="ObjectificationOppositeRoleName" Id="4719AAC4-E0E7-467A-B261-CDB8AE9826ED" Kind="CustomStorage">
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
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
				<DomainProperty Name="MaxFrequency" DefaultValue="2" DisplayName="MaxFrequency" Id="F46D9200-3602-435C-B852-C53BE10D99C6">
					<Type>
						<ExternalTypeMoniker Name="/System/Int32"/>
					</Type>
				</DomainProperty>
			</Properties>

		</DomainClass>

		<DomainClass Name="UniquenessConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" Id="49C7E3CE-C4F9-417D-B49C-27EA4016371E" DisplayName="UniquenessConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SetConstraint"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="IsPreferred" DefaultValue="false" DisplayName="IsPreferred" Id="585DE7A0-8E09-43F3-8463-F20609A16790" Kind="CustomStorage">
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

		<DomainClass Name="MandatoryConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" Id="F054BE4D-BFCA-4CD3-A0D8-97F61C165753" DisplayName="MandatoryConstraint" Description="">
			<BaseClass>
				<DomainClassMoniker Name="SetConstraint"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="IsSimple" DefaultValue="false" DisplayName="IsSimple" Id="BBDA6BD3-B3AC-4E26-ABC3-3FE1DAFA0165" IsBrowsable="false">
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

		<DomainClass Name="TooFewRoleSequencesError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="686A4B07-0ED9-4143-8225-5524C4D6C001" DisplayName="TooFewRoleSequencesError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TooManyRoleSequencesError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="1ADACF12-94F5-430D-9E14-6A3B0334139E" DisplayName="TooManyRoleSequencesError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ObjectTypeDuplicateNameError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="798D4CC7-1AD8-4A83-AFD5-5730AC342DC2" DisplayName="ObjectTypeDuplicateNameError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="DuplicateNameError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ConstraintDuplicateNameError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="AA63E81B-6978-49A2-A4AC-86022A172EDD" DisplayName="ConstraintDuplicateNameError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="DuplicateNameError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="DuplicateNameError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="9E29C624-4559-4020-9163-7B5846C94C0C" DisplayName="DuplicateNameError" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TooFewReadingRolesError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="1D2B23EF-456E-4E80-91D8-FB384F779A54" DisplayName="TooFewReadingRolesError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TooManyReadingRolesError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="50C98172-412C-40C0-ADD3-82809C3D82F7" DisplayName="TooManyReadingRolesError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ExternalConstraintRoleSequenceArityMismatchError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="3DA5385A-D9DE-4F3D-9D2E-CA79F10AB542" DisplayName="ExternalConstraintRoleSequenceArityMismatchError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="FactTypeRequiresReadingError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="3ECA7E92-45B2-45BD-BAD3-6AF0C4B40E70" DisplayName="FactTypeRequiresReadingError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="FactTypeRequiresInternalUniquenessConstraintError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="295D4B3D-1351-431D-B72F-28661D744B58" DisplayName="FactTypeRequiresInternalUniquenessConstraintError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="DataTypeNotSpecifiedError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="8AFA102F-529C-4896-AEB3-9D714E28FC61" DisplayName="DataTypeNotSpecifiedError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="NMinusOneError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="497754B3-5176-4712-BC46-2E4377354C8B" DisplayName="NMinusOneError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="CompatibleRolePlayerTypeError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="5C8D3150-2604-44FC-A468-B678F9B4206E" DisplayName="CompatibleRolePlayerTypeError" Description="">
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

		<DomainClass Name="RolePlayerRequiredError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="59A21FDE-D979-4B18-9088-707B79FCE19E" DisplayName="RolePlayerRequiredError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="EqualityImpliedByMandatoryError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="0809316F-AE25-4D6A-8FF2-8CE8A685D32D" DisplayName="EqualityImpliedByMandatoryError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="EntityTypeRequiresReferenceSchemeError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="A9177733-169B-418A-A843-3E3777DC9982" DisplayName="EntityTypeRequiresReferenceSchemeError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="MandatoryImpliedByMandatoryError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="FFDF5043-1B67-4D75-9D83-DD883A604D67" DisplayName="MandatoryImpliedByMandatoryError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="FrequencyConstraintMinMaxError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="5586C408-1A46-4CA7-8B0D-0462CD904009" DisplayName="FrequencyConstraintMinMaxError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ModelError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="16DF5C5E-83EF-4EDC-B54A-56D58D62D982" DisplayName="ModelError" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMNamedElement"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ReferenceModeKind" Namespace="Neumont.Tools.ORM.ObjectModel" Id="7EC5E835-5EEB-4FB1-AA09-9BD6ABA531E1" DisplayName="ReferenceModeKind" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMNamedElement"/>
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
								<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.Design.ReferenceModeKindPicker)"/>
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

		<DomainClass Name="UnsignedIntegerNumericDataType" Namespace="Neumont.Tools.ORM.ObjectModel" Id="9D76D09D-10F6-4DB0-8890-1077A95FB364" DisplayName="UnsignedIntegerNumericDataType" Description="">
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
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="Reading" Namespace="Neumont.Tools.ORM.ObjectModel" Id="7544854F-A4A7-4429-8859-F1D3B0E52B03" DisplayName="Reading" Description="">
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
				<DomainProperty Name="ReadingText" DefaultValue="" DisplayName="ReadingText" Id="4E75AD63-A42B-4571-85CE-81A4C5E02C23" Kind="CustomStorage">
					<Attributes>
						<ClrAttribute Name="global::System.ComponentModel.Editor">
							<Parameters>
								<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.Design.ReadingTextEditor)"/>
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
			</Properties>
		</DomainClass>

		<DomainClass Name="ValueMismatchError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="A18FA855-E7CA-4716-8E8D-1606C09B090A" DisplayName="ValueMismatchError" InheritanceModifier="Abstract" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="MinValueMismatchError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="7E0D53CF-D374-4EDA-B6A6-04D381AA0DC5" DisplayName="MinValueMismatchError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ValueMismatchError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="MaxValueMismatchError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="CCE42465-23A0-4726-8881-3ADB48E2CC67" DisplayName="MaxValueMismatchError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ValueMismatchError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ImpliedInternalUniquenessConstraintError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="B7381F8B-C95E-408D-9747-4B6BB35C1171" DisplayName="ImpliedInternalUniquenessConstraintError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="FrequencyConstraintContradictsInternalUniquenessConstraintError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="A080C2B2-F666-4689-A63E-BD97CB0491E2" DisplayName="FrequencyConstraintContradictsInternalUniquenessConstraintError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="RingConstraintTypeNotSpecifiedError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="15026270-DFD6-470D-A997-233173E644DC" DisplayName="RingConstraintTypeNotSpecifiedError" Description="">
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

		<DomainClass Name="ObjectTypeRequiresPrimarySupertypeError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="C35DEE5A-63C5-457C-A015-6E988CBAB8C5" DisplayName="ObjectTypeRequiresPrimarySupertypeError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="Note" Namespace="Neumont.Tools.ORM.ObjectModel" Id="C3DE6C8C-2215-49B0-BD70-70D2C3630C33" DisplayName="Note" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ORMModelElement"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="Text" DefaultValue="" DisplayName="Text" Id="0EF3BC12-45FF-46A8-B325-CDFCC105A1E1">
					<Type>
						<ExternalTypeMoniker Name="/System/String"/>
					</Type>
				</DomainProperty>
			</Properties>

		</DomainClass>

		<DomainClass Name="CompatibleSupertypesError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="70A9ED25-7A0E-4DEC-B39D-83BB1A6294B8" DisplayName="CompatibleSupertypesError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="UniquenessImpliedByUniquenessError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="C207522C-7723-4E3C-A093-AC7D2AE3A1DD" DisplayName="UniquenessImpliedByUniquenessError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="PreferredIdentifierRequiresMandatoryError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="129CCE68-7CE9-4A97-BAD3-C36B4D372A77" DisplayName="PreferredIdentifierRequiresMandatoryError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="ValueRangeOverlapError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="2CF1EE1A-1737-4868-9B5C-95B2C0F9488B" DisplayName="ValueRangeOverlapError" Description="">
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

		<DomainClass Name="TooFewEntityTypeRoleInstancesError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="39F447EA-8EA4-483D-B791-848AD27544E2" DisplayName="TooFewEntityTypeRoleInstancesError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="TooFewFactTypeRoleInstancesError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="BE44DD74-2569-421E-8E1B-ABCDC7810C92" DisplayName="TooFewFactTypeRoleInstancesError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>

		<DomainClass Name="CompatibleValueTypeInstanceValueError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="D5B21137-31E8-444D-BCD2-58BBF442B4C0" DisplayName="CompatibleValueTypeInstanceValueError" Description="">
			<BaseClass>
				<DomainClassMoniker Name="ModelError"/>
			</BaseClass>
		</DomainClass>
	</Classes>

	<Relationships>
		<!--<DomainRelationship Name="ORMElementLink" Namespace="Neumont.Tools.ORM.ObjectModel" AllowsDuplicates="false" InheritanceModifier="Abstract" Id="D6DC3311-4298-4EE5-9DA2-B1378AB09BF1"/>-->

		<DomainRelationship Name="FactConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" AllowsDuplicates="false" InheritanceModifier="Abstract" Id="BCF635F2-F2C6-4690-956D-2A44C48A9DA9">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="FactType" PropertyName="ConstraintCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="FactType" Id="D60CB2BF-7DE7-4CED-A00F-BF7C3A2E5248">
					<RolePlayer>
						<DomainClassMoniker Name="FactType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Constraint" PropertyName="FactTypeCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="Constraint" Id="9B305629-1EFA-404F-AE8E-475117B287AE">
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
				<DomainRole Name="FactType" PropertyName="SetComparisonConstraintCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="F7733FAF-1029-480E-8FEA-96FDD65AB212">
					<RolePlayer>
						<DomainClassMoniker Name="FactType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="SetComparisonConstraint" PropertyName="FactTypeCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetComparisonConstraint" Id="575F65E4-682E-427A-B273-3D30D909A816">
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
				<DomainRole Name="FactType" PropertyName="SetConstraintCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="DE9A381F-5346-4C95-9D48-E468B8CF8A29">
					<RolePlayer>
						<DomainClassMoniker Name="FactType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="SetConstraint" PropertyName="FactTypeCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetConstraint" Id="7789FD46-6E28-4AB7-AFC5-7F17B95AC4D9">
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
				<DomainRole Name="ExtendedElement" PropertyName="ExtensionCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ExtendedElement" Id="9105A491-7FC1-408E-8E07-F8E79CA0BFA4">
					<RolePlayer>
						<DomainClassMoniker Name="ORMModelElement"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Extension" PropertyName="ExtendedElement" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="false" DisplayName="Extension" Id="0A7BBD8D-0D86-4FB4-991B-365302D1ED63">
					<RolePlayer>
						<DomainClassMoniker Name="/Microsoft.VisualStudio.Modeling/ModelElement"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ORMModelElementHasExtensionModelError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="7A4D2B10-43F3-475F-AA0A-8F880B9A1E4B">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ExtendedElement" PropertyName="ExtensionModelErrorCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ExtendedElement" Id="415C3EF5-7524-45A9-9307-3D8B53BD88D6">
					<RolePlayer>
						<DomainClassMoniker Name="ORMModelElement"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ExtensionModelError" PropertyName="ExtendedElement" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="false" DisplayName="ExtensionModelError" Id="1A7A14EF-01FC-4ED8-A1EA-3533511D1750">
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
				<DomainRole Name="ValueType" PropertyName="DataType" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ValueType" Id="3F6D8D0A-CEC5-47EF-8F81-EF25F59593E0">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="DataType" PropertyName="ValueTypeCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="DataType" Id="0414C824-F797-4F95-8F25-7D275FD632B8">
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
				<DomainRole Name="NestingType" PropertyName="NestedFactType" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="NestingType" Id="2660CF3E-2A56-496D-98CD-BFFAC5E73198">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="NestedFactType" PropertyName="NestingType" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="NestedFactType" Id="69F805CC-874F-4E03-8364-0A0445168B26">
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
				<DomainRole Name="PlayedRole" PropertyName="RolePlayer" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="PlayedRole" Id="A87B6EEB-1753-4AD3-A00D-431E34B05AC2">
					<RolePlayer>
						<DomainClassMoniker Name="Role"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="RolePlayer" PropertyName="PlayedRoleCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="RolePlayer" Id="8EC5C761-2E7C-422C-B5E7-354788A18F59">
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
				<DomainRole Name="Model" PropertyName="ObjectTypeCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="E3FA6F15-EF49-4B72-B02F-EC7C2BA718EC">
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
				<DomainRole Name="Model" PropertyName="FactTypeCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="CC174187-4E88-4230-ADBD-B468F58AB58D">
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
				<DomainRole Name="Model" PropertyName="ErrorCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="Model" Id="8F57FA08-7038-4CDB-900A-450A9A9DD8DC">
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
				<DomainRole Name="Model" PropertyName="ReferenceModeKindCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="966465E7-6BAD-4100-A082-B4AA20511A7D">
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
				<DomainRole Name="Model" PropertyName="ReferenceModeCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="88B428C5-E93F-4739-82E5-440E6B13921A">
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
				<DomainRole Name="Model" PropertyName="SetConstraintCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="54B0D8A2-91B9-41A3-8571-103DCB7BECCD">
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

		<DomainRelationship Name="ModelHasSetComparisonConstraint" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="37FBE5B6-4E18-43E2-B34B-DAB0EF69DDE4">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Model" PropertyName="SetComparisonConstraintCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="0D5738FB-77EF-41C2-82EE-A98E5484B11B">
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
				<DomainRole Name="Model" PropertyName="DataTypeCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Model" Id="41F9A40E-DEDD-4BBA-9C79-8548ACFBCB9A">
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

		<DomainRelationship Name="SetComparisonConstraintHasRoleSequence" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="84B13BEA-FC8C-446C-B643-9688B99AF1B6">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ExternalConstraint" PropertyName="RoleSequenceCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ExternalConstraint" Id="1D11FC93-6110-44F7-BFE1-38FC7DC81170">
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
				<DomainRole Name="Role" PropertyName="ConstraintRoleSequenceCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Role" Id="1A5A347E-1D5D-4045-9EA0-13B2338FC898">
					<RolePlayer>
						<DomainClassMoniker Name="Role"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ConstraintRoleSequence" PropertyName="RoleCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ConstraintRoleSequence" Id="9AD53904-280A-4329-A6F0-20E2C44F5607">
					<RolePlayer>
						<DomainClassMoniker Name="ConstraintRoleSequence"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SetComparisonConstraintHasTooFewRoleSequencesError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="3167F5D3-C234-46E3-AAC2-4CEB791DFB9C">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="SetComparisonConstraint" PropertyName="TooFewRoleSequencesError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetComparisonConstraint" Id="0178C877-8906-4BDC-B3F8-0322A578741D">
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

		<DomainRelationship Name="SetComparisonConstraintHasTooManyRoleSequencesError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="E7C33130-2D1F-4F95-B988-BD7608CF2D1C">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="SetComparisonConstraint" PropertyName="TooManyRoleSequencesError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetComparisonConstraint" Id="76082669-3E03-4837-8824-526BB25DACB8">
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
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ObjectType" PropertyName="DuplicateNameError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ObjectType" Id="A2252380-7CAC-4D36-8857-2426AE558C08">
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
				<DomainRole Name="FactType" PropertyName="ReadingOrderCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="D77A6514-B8F0-4C0C-B856-EE74DBBC1C41">
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
				<DomainRole Name="ReferenceMode" PropertyName="Kind" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="ReferenceMode" Id="67F35299-D67F-4AE2-9159-E5EFF1FF8544">
					<RolePlayer>
						<DomainClassMoniker Name="ReferenceMode"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Kind" PropertyName="ReferenceModeCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Kind" Id="CD7ED96F-DDB9-4242-94CF-B10255822F66">
					<RolePlayer>
						<DomainClassMoniker Name="ReferenceModeKind"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="SetConstraintHasDuplicateNameError" Namespace="Neumont.Tools.ORM.ObjectModel" Id="CB5DF90F-3917-4BD1-9807-A24F6D7C52F9">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="SetConstraint" PropertyName="DuplicateNameError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetConstraint" Id="B4623963-690D-4687-A95E-0FC998AA59EC">
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
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="SetComparisonConstraint" PropertyName="DuplicateNameError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetComparisonConstraint" Id="86A9CB44-0050-4E7C-9DF5-692F980F96EC">
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
				<DomainRole Name="PreferredIdentifierFor" PropertyName="PreferredIdentifier" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="PreferredIdentifierFor" Id="04D998EE-030E-4A81-88BC-666CE4EFB3ED">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="PreferredIdentifier" PropertyName="PreferredIdentifierFor" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="PreferredIdentifier" Id="6195CE84-7CA8-4E13-B8C8-24438E2CF300">
					<RolePlayer>
						<DomainClassMoniker Name="UniquenessConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ReadingHasTooManyRolesError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="D2116BC7-25A8-455E-9347-414BD03B7546">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Reading" PropertyName="TooManyRolesError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Reading" Id="AA517583-0A1B-4129-905E-A9EE3F59EE17">
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

		<DomainRelationship Name="ReadingHasTooFewRolesError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="FC3E0A3C-40CE-4DED-8A6B-241C7B51C099">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Reading" PropertyName="TooFewRolesError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Reading" Id="3A9889AA-6152-4E1E-A1EC-B100AD24A60A">
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

		<DomainRelationship Name="SetComparisonConstraintHasExternalConstraintRoleSequenceArityMismatchError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="C5A25732-F5A7-409E-B56A-6419A951FB13">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Constraint" PropertyName="ArityMismatchError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Constraint" Id="E75C8B14-01C7-4CEF-879D-BE6A1D922AA4">
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

		<DomainRelationship Name="FactTypeHasFactTypeRequiresReadingError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="EEC8EB82-5B15-4B61-8737-DA1A54199A13">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="FactType" PropertyName="ReadingRequiredError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="C79D68BD-DBED-4487-A448-70B9EDC5E4D9">
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

		<DomainRelationship Name="FactTypeHasFactTypeRequiresInternalUniquenessConstraintError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="DD5FF7F8-7169-489B-9B8A-EDE3772F52BE">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="FactType" PropertyName="InternalUniquenessConstraintRequiredError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="42AED551-7F1A-4F16-AA39-682C9DBB8607">
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
				<DomainRole Name="ValueType" PropertyName="ValueConstraint" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ValueType" Id="2F42A8FD-AB49-4E0F-AF3A-1098BA77A4C1">
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
				<DomainRole Name="Role" PropertyName="ValueConstraint" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Role" Id="AF99B941-1811-4DFB-BD26-8F4148D3F1D9">
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

		<DomainRelationship Name="ValueTypeHasUnspecifiedDataTypeError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="F2A79E36-A317-4C36-81DA-D562D2AFBF09">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ValueTypeHasDataType" PropertyName="DataTypeNotSpecifiedError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ValueTypeHasDataType" Id="E8122190-AE46-40D8-8040-118D577735A6">
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

		<DomainRelationship Name="SetComparisonConstraintHasCompatibleRolePlayerTypeError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="13410C4F-FFED-4B0F-AD0B-BD48D09B4310">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="SetComparisonConstraint" PropertyName="CompatibleRolePlayerTypeErrorCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetComparisonConstraint" Id="B1660D4F-F77A-4FE2-9DDC-DFBFAB545B92">
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

		<DomainRelationship Name="SetConstraintHasCompatibleRolePlayerTypeError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="91CDE095-28D9-4852-B171-430FE5A29429">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="SetConstraint" PropertyName="CompatibleRolePlayerTypeError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetConstraint" Id="171D82AC-46AE-486D-B602-62A0F49CEBC0">
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

		<DomainRelationship Name="UniquenessConstraintHasNMinusOneError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="429F7144-1227-4D0E-B4F8-59AD6FFC7EB3">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Constraint" PropertyName="NMinusOneError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Constraint" Id="6165AD47-FB70-4F43-936E-E162D0E8E917">
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

		<DomainRelationship Name="RoleHasRolePlayerRequiredError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="09E6AC31-2CA1-4126-8C95-BFC571088B2D">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Role" PropertyName="RolePlayerRequiredError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Role" Id="529B2BED-F8F3-4A9B-95B7-55D9A1ED5B44">
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

		<DomainRelationship Name="EqualityConstraintHasEqualityImpliedByMandatoryError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="0D352E42-06D6-4AD1-AACE-3EA5AACDE302">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="EqualityConstraint" PropertyName="EqualityImpliedByMandatoryError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="EqualityConstraint" Id="FBF0117A-A2B8-4EF9-B263-24288F40CB4E">
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

		<DomainRelationship Name="ObjectTypeHasEntityTypeRequiresReferenceSchemeError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="1B6CBB8C-D1A6-4949-AC4D-596DC1CE147F">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ObjectType" PropertyName="ReferenceSchemeError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ObjectType" Id="7CFF7155-B46F-4492-A054-8028772D7529">
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

		<DomainRelationship Name="MandatoryConstraintHasMandatoryImpliedByMandatoryError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="3EFCB5A8-2DCB-478E-BD02-5898C36C9143">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="MandatoryConstraint" PropertyName="ImpliedByMandatoryError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="MandatoryConstraint" Id="2A540753-3F31-4B4A-9501-BF4AE4779CCF">
					<RolePlayer>
						<DomainClassMoniker Name="MandatoryConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ImpliedByMandatoryError" PropertyName="MandatoryConstraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ImpliedByMandatoryError" Id="41B3C5F6-37E4-49E3-8F5D-8DA4C377C24F">
					<RolePlayer>
						<DomainClassMoniker Name="MandatoryImpliedByMandatoryError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FrequencyConstraintHasFrequencyConstraintMinMaxError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="2E851B91-FCB9-4B3C-9276-2C2E3A1972C9">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="FrequencyConstraint" PropertyName="FrequencyConstraintMinMaxError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FrequencyConstraint" Id="80542FE3-3450-42EE-9C22-B17E868B7695">
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

		<DomainRelationship Name="ObjectificationImpliesFactType" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="D2706F81-78CC-493E-90C9-D54A10D33FA0">
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

		<DomainRelationship Name="ValueRangeHasMaxValueMismatchError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="1D2620BE-40AC-4F10-B420-5CD52687DD49">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ValueRange" PropertyName="MaxValueMismatchError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ValueRange" Id="A5F8E444-85DA-40D0-97E2-91EB8E36A6B0">
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

		<DomainRelationship Name="ValueRangeHasMinValueMismatchError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="0E8BE672-BCBE-412B-9589-76BFA88FDE38">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ValueRange" PropertyName="MinValueMismatchError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ValueRange" Id="CFAFDA20-C375-431D-89DF-CDBF14419773">
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

		<DomainRelationship Name="FactTypeHasImpliedInternalUniquenessConstraintError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="32D5A7E1-5A80-44AB-BC2E-96A15A4D92CB">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="FactType" PropertyName="ImpliedInternalUniquenessConstraintError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="87737B64-9709-4DDE-8D77-290A2CCEED1C">
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

		<DomainRelationship Name="SetConstraintHasTooFewRoleSequencesError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="6409DBE5-5C44-42AF-B0C6-FB1EE7E3AF2A">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="SetConstraint" PropertyName="TooFewRoleSequencesError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetConstraint" Id="1DF0F9D7-63BD-4577-8C8F-FCAB97FFF98C">
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

		<DomainRelationship Name="SetConstraintHasTooManyRoleSequencesError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="D54EB064-7FC6-4BCD-AF30-C73E2D586FC4">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="SetConstraint" PropertyName="TooManyRoleSequencesError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="SetConstraint" Id="F7EEDD61-28A3-48CC-B83D-C18ECEE2D582">
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

		<DomainRelationship Name="FrequencyConstraintHasFrequencyConstraintInvalidatedByInternalUniquenessConstraintError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="78716CE5-DB71-4367-A912-9B622A3C480B">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="FrequencyConstraint" PropertyName="FrequencyConstraintContradictsInternalUniquenessConstraintErrorCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FrequencyConstraint" Id="4A87A658-BE43-4337-A7E1-DB66219CB52C">
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

		<DomainRelationship Name="FactTypeHasFrequencyConstraintContradictsInternalUniquenessConstraintError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="57656C65-6812-4E80-AB3C-199DEB82B3EF">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="FactType" PropertyName="FrequencyConstraintContradictsInternalUniquenessConstraintErrorCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="D2203F28-3CB7-4474-892C-25EE95AB22A6">
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

		<DomainRelationship Name="RingConstraintHasRingConstraintTypeNotSpecifiedError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="62E65E16-EFA7-43D0-9759-8715D0C8B914">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="RingConstraint" PropertyName="RingConstraintTypeNotSpecifiedError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="RingConstraint" Id="4E7DEA3B-ACF3-4E71-A4F2-5C08FB8077D2">
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
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ValueConstraint" PropertyName="DuplicateNameError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ValueConstraint" Id="8D9CDE68-22D7-48FF-ABE2-1617B5D2BB92">
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

		<DomainRelationship Name="ObjectTypeHasObjectTypeRequiresPrimarySupertypeError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="2231FC51-1B87-45A5-AF53-5A95F1B68E04">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ObjectType" PropertyName="ObjectTypeRequiresPrimarySupertypeError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ObjectType" Id="E968CD16-0FAD-4D46-BE86-478B12CD8FCC">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ObjectTypeRequiresPrimarySupertypeError" PropertyName="ObjectType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ObjectTypeRequiresPrimarySupertypeError" Id="4CDF2EBE-8D1A-48C9-B34F-9CE82C882625">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectTypeRequiresPrimarySupertypeError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="FactTypeHasNote" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="B41C4D61-2A9F-4C91-B948-52E53A8E525F">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="FactType" PropertyName="Note" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="BEDCB56B-CAD7-45FD-B781-2A437AEF5141">
					<RolePlayer>
						<DomainClassMoniker Name="FactType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Note" PropertyName="FactType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Note" Id="0EE3D4E9-714C-404F-AE40-5C140A335F42">
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
				<DomainRole Name="ObjectType" PropertyName="Note" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ObjectType" Id="A75B240C-06CC-4F48-B787-8536C58E4CD8">
					<RolePlayer>
						<DomainClassMoniker Name="ObjectType"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="Note" PropertyName="ObjectType" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="Note" Id="B2BC6ECC-430A-48AE-A862-D4D876748130">
					<RolePlayer>
						<DomainClassMoniker Name="Note"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ObjectTypeHasCompatibleSupertypesError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="4A739F80-00FA-4F02-BD81-ED60C79DEFC3">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ObjectType" PropertyName="CompatibleSupertypesError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ObjectType" Id="FF50F039-B38B-41A2-9D06-0AEFBE62C6A9">
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

		<DomainRelationship Name="UniquenessConstraintHasUniquenessImpliedByUniquenessError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="8ADB8B72-8510-486A-87CE-760C7607460C">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="UniquenessConstraint" PropertyName="ImpliedByUniquenessError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="UniquenessConstraint" Id="DC738B91-F33F-409A-B397-2D18F77ACC65">
					<RolePlayer>
						<DomainClassMoniker Name="UniquenessConstraint"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="ImpliedByUniquenessError" PropertyName="UniquenessConstraint" Multiplicity="One" PropagatesDelete="true" IsPropertyGenerator="true" DisplayName="ImpliedByUniquenessError" Id="8F790B28-0B92-408B-ACBA-86F356B7C7C9">
					<RolePlayer>
						<DomainClassMoniker Name="UniquenessImpliedByUniquenessError"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>

		<DomainRelationship Name="ObjectTypeHasPreferredIdentifierRequiresMandatoryError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="31A1BFF6-47DC-4F00-955B-1935082A3F25">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ObjectType" PropertyName="PreferredIdentifierRequiresMandatoryError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ObjectType" Id="E7851C84-D822-4EA5-AB3F-2648538158C5">
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

		<DomainRelationship Name="ValueConstraintHasValueRangeOverlapError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="9044BE08-D88B-4BCA-B261-0841E1C73B5D">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ValueConstraint" PropertyName="ValueRangeOverlapError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ValueConstraint" Id="6011898A-4CF2-429A-986E-F0ED8D938064">
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
				<DomainRole Name="TargetRole" PropertyName="Proxy" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="TargetRole" Id="AE004027-BE74-4E53-99D7-D3E894F4124D">
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
				<DomainRole Name="FactType" PropertyName="DerivationRule" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="73B1A9D8-42A4-44E0-B906-AEF10E346DB6">
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

		<DomainRelationship Name="EntityTypeHasEntityTypeInstance" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="0F9CDA9D-88CE-47DD-B202-93B1455E08C3">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="EntityType" PropertyName="EntityTypeInstanceCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="EntityType" Id="5A293722-12D6-4B42-A336-3281A404E785">
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
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ValueType" PropertyName="ValueTypeInstanceCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ValueType" Id="9558751B-6AE9-424D-8B62-66B71F01A207">
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

		<!-- RoleInstance is double-derived so that we can implement the Role and ObjectTypeInstance properties. -->
		<DomainRelationship Name="RoleInstance" Namespace="Neumont.Tools.ORM.ObjectModel" AllowsDuplicates="false" InheritanceModifier="Abstract" Id="D3162C67-DE52-4B0D-802F-824E6ED5B74B">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="Role" PropertyName="ObjectTypeInstanceCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="Role" Id="C927C1AA-2E2D-41CF-9D87-0A69A63F3E99">
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

		<DomainRelationship Name="EntityTypeRoleInstance" Namespace="Neumont.Tools.ORM.ObjectModel" AllowsDuplicates="false" Id="5DB3A2C1-C5DE-4C4A-97C2-E09CE11537D3">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="RoleInstance"/>
			</BaseRelationship>
		</DomainRelationship>

		<DomainRelationship Name="FactTypeRoleInstance" Namespace="Neumont.Tools.ORM.ObjectModel" AllowsDuplicates="false" Id="FC7C9715-6886-46C2-A7A0-3BFD95CD0766">
			<BaseRelationship>
				<DomainRelationshipMoniker Name="RoleInstance"/>
			</BaseRelationship>
		</DomainRelationship>

		<!-- UNDONE: 2006-06 DSL Tools port: @IsEmbedding was true on this relationship, but that is no longer allowed when the target role player is itself a DomainRelationship. -->
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
				<DomainRole Name="FactType" PropertyName="FactTypeInstanceCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactType" Id="4690EAFF-667B-4AA6-93ED-9487E109C7BC">
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

		<!-- UNDONE: 2006-06 DSL Tools port: @IsEmbedding was true on this relationship, but that is no longer allowed when the target role player is itself a DomainRelationship. -->
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

		<DomainRelationship Name="EntityTypeInstanceHasTooFewEntityTypeRoleInstancesError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="922E0A74-9384-4D25-9C38-E0AB709FEE8F">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="EntityTypeInstance" PropertyName="TooFewEntityTypeRoleInstancesError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="EntityTypeInstance" Id="715ED45E-2C85-491E-A577-3A6985347687">
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

		<DomainRelationship Name="FactTypeInstanceHasTooFewFactTypeRoleInstancesError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="6AC86DD8-1766-472E-B70F-B788C04ED688">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="FactTypeInstance" PropertyName="TooFewFactTypeRoleInstancesError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactTypeInstance" Id="AC1F89F9-1DBC-4826-8035-6EA0C1D35EB1">
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

		<DomainRelationship Name="ValueTypeInstanceHasCompatibleValueTypeInstanceValueError" Namespace="Neumont.Tools.ORM.ObjectModel" IsEmbedding="true" Id="A8AF2A8F-CDD0-41CB-B8CD-60CF28277288">
			<!--<BaseRelationship>
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</BaseRelationship>-->
			<Source>
				<DomainRole Name="ValueTypeInstance" PropertyName="CompatibleValueTypeInstanceValueError" Multiplicity="One" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="ValueTypeInstance" Id="5C173594-AA0C-45BA-92F1-8D80A74E1300">
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

		
		<!-- Shape relationships -->

		<DomainRelationship Name="FactTypeShapeHasRoleDisplayOrder" Namespace="Neumont.Tools.ORM.ShapeModel" Id="94B3AEEF-4C8D-4D1A-A7CC-42F7EBDC68A2">
			<Source>
				<DomainRole Name="FactTypeShape" PropertyName="RoleDisplayOrderCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="true" DisplayName="FactTypeShape" Id="30C6B725-2D74-47F7-852A-D02C644A447B">
					<RolePlayer>
						<GeometryShapeMoniker Name="FactTypeShape"/>
					</RolePlayer>
				</DomainRole>
			</Source>
			<Target>
				<DomainRole Name="RoleDisplayOrder" PropertyName="FactTypeShapeCollection" Multiplicity="OneMany" PropagatesDelete="false" IsPropertyGenerator="false" DisplayName="RoleDisplayOrder" Id="4CA45C6E-0400-4976-AF8C-0CAD7C7BC2EE">
					<RolePlayer>
						<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/RoleBase"/>
					</RolePlayer>
				</DomainRole>
			</Target>
		</DomainRelationship>
		
	</Relationships>

	<Types>
		<ExternalType Namespace="System" Name="Object"/>
		<ExternalType Namespace="System" Name="Boolean"/>
		<ExternalType Namespace="System" Name="String"/>
		<ExternalType Namespace="System" Name="Int32"/>
		<ExternalType Namespace="System" Name="Int64"/>
		<ExternalType Namespace="System" Name="Double"/>
		<ExternalType Namespace="Neumont.Tools.ORM.ObjectModel" Name="FactType"/>
		<ExternalType Namespace="Neumont.Tools.ORM.ObjectModel" Name="ReferenceMode"/>
		<ExternalType Namespace="Neumont.Tools.ORM.ObjectModel" Name="DataType"/>
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
						<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.Design.ORMEnumConverter&lt;DerivationStorageType, global::Neumont.Tools.ORM.ObjectModel.ORMMetaModel&gt;)"/>
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
						<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.Design.ORMEnumConverter&lt;ConstraintModality, global::Neumont.Tools.ORM.ObjectModel.ORMMetaModel&gt;)"/>
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
						<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.Design.ORMEnumConverter&lt;RoleMultiplicity, global::Neumont.Tools.ORM.ObjectModel.ORMMetaModel&gt;)"/>
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
				<EnumerationLiteral Name="Irreflexive" Value="1" Description=""/>
				<EnumerationLiteral Name="Symmetric" Value="2" Description=""/>
				<EnumerationLiteral Name="Asymmetric" Value="3" Description=""/>
				<EnumerationLiteral Name="Antisymmetric" Value="4" Description=""/>
				<EnumerationLiteral Name="Intransitive" Value="5" Description=""/>
				<EnumerationLiteral Name="Acyclic" Value="6" Description=""/>
				<EnumerationLiteral Name="AcyclicIntransitive" Value="7" Description=""/>
				<EnumerationLiteral Name="AsymmetricIntransitive" Value="8" Description=""/>
				<EnumerationLiteral Name="SymmetricIntransitive" Value="9" Description=""/>
				<EnumerationLiteral Name="SymmetricIrreflexive" Value="10" Description=""/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.Design.ORMEnumConverter&lt;RingConstraintType, global::Neumont.Tools.ORM.ObjectModel.ORMMetaModel&gt;)"/>
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
						<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.Design.ORMEnumConverter&lt;ReferenceModeType, global::Neumont.Tools.ORM.ObjectModel.ORMMetaModel&gt;)"/>
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
						<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.Design.ORMEnumConverter&lt;RangeInclusion, global::Neumont.Tools.ORM.ObjectModel.ORMMetaModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
		<DomainEnumeration Namespace="Neumont.Tools.ORM.ShapeModel" Name="ConstraintDisplayPosition" Description="Determines where internal constraints are drawn on FactTypeShapes.">
			<Literals>
				<EnumerationLiteral Name="Top" Value="0" Description="Draw the constraints above the top of the role boxes."/>
				<EnumerationLiteral Name="Bottom" Value="1" Description="Draw the constraints below the bottom of the role boxes."/>
			</Literals>
			<Attributes>
				<ClrAttribute Name="global::System.ComponentModel.TypeConverter">
					<Parameters>
						<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.Design.ORMEnumConverter&lt;ConstraintDisplayPosition, global::Neumont.Tools.ORM.ObjectModel.ORMMetaModel&gt;)"/>
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
						<AttributeParameter Value="typeof(global::Neumont.Tools.ORM.Design.ORMEnumConverter&lt;DisplayRoleNames, global::Neumont.Tools.ORM.ObjectModel.ORMMetaModel&gt;)"/>
					</Parameters>
				</ClrAttribute>
			</Attributes>
		</DomainEnumeration>
	</Types>

	<Shapes>
		<GeometryShape Name="ORMBaseShape" Namespace="Neumont.Tools.ORM.ShapeModel" InheritanceModifier="Abstract" Id="55131F4B-0F9A-408D-BED0-79451BA7F4F0" HasCustomConstructor="true" FillGradientMode="None">
			<Properties>
				<DomainProperty Name="UpdateCounter" Id="85E23BA2-451A-4CD3-B233-64973E6133F6" GetterAccessModifier="Private" SetterAccessModifier="Private" Kind="CustomStorage" IsBrowsable="false">
					<Type>
						<ExternalTypeMoniker Name="/System/Int64"/>
					</Type>
				</DomainProperty>
			</Properties>
		</GeometryShape>
		<GeometryShape Name="ObjectTypeShape" Namespace="Neumont.Tools.ORM.ShapeModel" Id="00C1F246-D8F1-4EEA-AC88-39BA238143A8" FillGradientMode="None">
			<BaseClass>
				<GeometryShapeMoniker Name="ORMBaseShape"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="ExpandRefMode" DisplayName="ExpandRefMode" Id="B2415BB1-1C83-4F0B-B2C3-58B67BC620DD" DefaultValue="false">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
		</GeometryShape>
		<GeometryShape Name="FactTypeShape" Namespace="Neumont.Tools.ORM.ShapeModel" Id="8E440A3B-275E-42F7-868B-D5D473158ACD" FillGradientMode="None">
			<BaseClass>
				<GeometryShapeMoniker Name="ORMBaseShape"/>
			</BaseClass>
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
				<DomainProperty Name="RolesPosition" Id="89244439-FBB1-4DEB-BFF3-69D47CB90A6B" DefaultValue="0" IsBrowsable="false">
					<Type>
						<ExternalTypeMoniker Name="/System/Double"/>
					</Type>
				</DomainProperty>
			</Properties>
		</GeometryShape>
		<GeometryShape Name="SubtypeLink" Namespace="Neumont.Tools.ORM.ShapeModel" Id="87DDAEDA-1FD8-4433-BB1E-7482C7F471A7" FillGradientMode="None">
			<BaseClass>
				<ConnectorMoniker Name="ORMBaseBinaryLinkShape"/>
			</BaseClass>
		</GeometryShape>
		<GeometryShape Name="ExternalConstraintShape" Namespace="Neumont.Tools.ORM.ShapeModel" Id="00A08F56-73BA-4C8F-8FA1-AE61B8FC1CAE" FillGradientMode="None">
			<BaseClass>
				<GeometryShapeMoniker Name="ORMBaseShape"/>
			</BaseClass>
		</GeometryShape>
		<GeometryShape Name="FrequencyConstraintShape" Namespace="Neumont.Tools.ORM.ShapeModel" Id="EC47CD7D-023B-4971-8B5B-1242DBC7356F" FillGradientMode="None">
			<BaseClass>
				<GeometryShapeMoniker Name="ExternalConstraintShape"/>
			</BaseClass>
		</GeometryShape>
		<GeometryShape Name="RingConstraintShape" Namespace="Neumont.Tools.ORM.ShapeModel" Id="61B334C5-D37F-4A74-90E9-AC793D35BDF1" FillGradientMode="None">
			<BaseClass>
				<GeometryShapeMoniker Name="ExternalConstraintShape"/>
			</BaseClass>
		</GeometryShape>
		<GeometryShape Name="FloatingTextShape" Namespace="Neumont.Tools.ORM.ShapeModel" InheritanceModifier="Abstract" Id="0904999F-D9C5-4C4E-A08F-F8DD4B2F29A3" FillGradientMode="None">
			<BaseClass>
				<GeometryShapeMoniker Name="ORMBaseShape"/>
			</BaseClass>
		</GeometryShape>
		<GeometryShape Name="ObjectifiedFactTypeNameShape" Namespace="Neumont.Tools.ORM.ShapeModel" Id="7FD5183A-8BC2-43BB-8474-A0A2D558D90A" FillGradientMode="None">
			<BaseClass>
				<GeometryShapeMoniker Name="FloatingTextShape"/>
			</BaseClass>
			<Properties>
				<DomainProperty Name="ExpandRefMode" DisplayName="ExpandRefMode" Id="5BDAFE8C-AFA7-4B78-ADC6-CAE876AB2140" DefaultValue="false">
					<Type>
						<ExternalTypeMoniker Name="/System/Boolean"/>
					</Type>
				</DomainProperty>
			</Properties>
		</GeometryShape>
		<GeometryShape Name="ReadingShape" Namespace="Neumont.Tools.ORM.ShapeModel" Id="C567ED6D-D0A6-4FD8-A974-C567AA309D5E" FillGradientMode="None">
			<BaseClass>
				<GeometryShapeMoniker Name="FloatingTextShape"/>
			</BaseClass>
		</GeometryShape>
		<GeometryShape Name="ValueConstraintShape" Namespace="Neumont.Tools.ORM.ShapeModel" Id="B65F916A-06A5-4EFE-BBF9-8D8E55B5C7EB" FillGradientMode="None">
			<BaseClass>
				<GeometryShapeMoniker Name="FloatingTextShape"/>
			</BaseClass>
		</GeometryShape>
		<GeometryShape Name="RoleNameShape" Namespace="Neumont.Tools.ORM.ShapeModel" Id="2CB7393C-4502-4C59-979D-94D6C89B4080" FillGradientMode="None">
			<BaseClass>
				<GeometryShapeMoniker Name="FloatingTextShape"/>
			</BaseClass>
		</GeometryShape>
		<GeometryShape Name="LinkConnectorShape" Namespace="Neumont.Tools.ORM.ShapeModel" Id="51770447-28E0-4BFF-977F-2D2625F7978D" FillGradientMode="None"/>
	</Shapes>

	<Connectors>
		<Connector Name="ORMBaseBinaryLinkShape" Namespace="Neumont.Tools.ORM.ShapeModel" InheritanceModifier="Abstract" Id="CEFF4339-48D0-4FFE-B052-2F9DA167B1DB"/>
		<Connector Name="RolePlayerLink" Namespace="Neumont.Tools.ORM.ShapeModel" Id="2B3F0AAE-B1B1-4727-8862-5C34B494B499">
			<BaseClass>
				<ConnectorMoniker Name="ORMBaseBinaryLinkShape"/>
			</BaseClass>
		</Connector>
		<Connector Name="ExternalConstraintLink" Namespace="Neumont.Tools.ORM.ShapeModel" Id="8815E6D8-238B-422C-A4B3-29FDC8DE9EA5">
			<BaseClass>
				<ConnectorMoniker Name="ORMBaseBinaryLinkShape"/>
			</BaseClass>
		</Connector>
		<Connector Name="ValueRangeLink" Namespace="Neumont.Tools.ORM.ShapeModel" Id="374E43C3-C294-49C4-8A61-3C3CA5FC86E8">
			<BaseClass>
				<ConnectorMoniker Name="ORMBaseBinaryLinkShape"/>
			</BaseClass>
		</Connector>
	</Connectors>

	<XmlSerializationBehavior Name="ORMMetaModelSerializationBehavior" Namespace="Neumont.Tools.ORM.ObjectModel">
		<ClassData>
			<XmlClassData TypeName="ValueTypeHasDataType">
				<DomainRelationshipMoniker Name="ValueTypeHasDataType"/>
				<ElementData>
					<XmlPropertyData XmlName="Scale">
						<DomainPropertyMoniker Name="ValueTypeHasDataType/Scale"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="Length">
						<DomainPropertyMoniker Name="ValueTypeHasDataType/Length"/>
					</XmlPropertyData>

					<XmlRelationshipData RoleElementName="DataTypeNotSpecifiedError">
						<DomainRelationshipMoniker Name="ValueTypeHasUnspecifiedDataTypeError"/>
						<!-- source role = ValueTypeHasDataType -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="Objectification">
				<DomainRelationshipMoniker Name="Objectification"/>
				<ElementData>
					<XmlPropertyData XmlName="IsImplied">
						<DomainPropertyMoniker Name="Objectification/IsImplied"/>
					</XmlPropertyData>

					<XmlRelationshipData RoleElementName="ImpliedFactTypeCollection">
						<DomainRelationshipMoniker Name="ObjectificationImpliesFactType"/>
						<!-- source role = ImpliedByObjectification -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="ObjectTypePlaysRole">
				<DomainRelationshipMoniker Name="ObjectTypePlaysRole"/>
			</XmlClassData>
			<XmlClassData TypeName="ModelHasObjectType">
				<DomainRelationshipMoniker Name="ModelHasObjectType"/>
			</XmlClassData>
			<XmlClassData TypeName="ModelHasFactType">
				<DomainRelationshipMoniker Name="ModelHasFactType"/>
			</XmlClassData>
			<XmlClassData TypeName="FactSetComparisonConstraint" SerializeId="true">
				<DomainRelationshipMoniker Name="FactSetComparisonConstraint"/>
			</XmlClassData>
			<XmlClassData TypeName="FactSetConstraint" SerializeId="true">
				<DomainRelationshipMoniker Name="FactSetConstraint"/>
			</XmlClassData>
			<XmlClassData TypeName="FactConstraint" SerializeId="true">
				<DomainRelationshipMoniker Name="FactConstraint"/>
			</XmlClassData>
			<XmlClassData TypeName="ExternalRoleConstraint" SerializeId="true">
				<DomainRelationshipMoniker Name="ExternalRoleConstraint"/>
			</XmlClassData>
			<XmlClassData TypeName="SetComparisonConstraintHasRoleSequence">
				<DomainRelationshipMoniker Name="SetComparisonConstraintHasRoleSequence"/>
			</XmlClassData>
			<XmlClassData TypeName="ConstraintRoleSequenceHasRole" SerializeId="true">
				<DomainRelationshipMoniker Name="ConstraintRoleSequenceHasRole"/>
				<ElementData>

					<XmlRelationshipData RoleElementName="FactConstraintCollection" UseFullForm="true">
						<!-- This relationship has Properties or is many-many, so UseFullForm is required -->
						<DomainRelationshipMoniker Name="ExternalRoleConstraint"/>
						<!-- source role = ConstrainedRoleCollection -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="ModelHasError">
				<DomainRelationshipMoniker Name="ModelHasError"/>
			</XmlClassData>
			<XmlClassData TypeName="SetComparisonConstraintHasTooFewRoleSequencesError">
				<DomainRelationshipMoniker Name="SetComparisonConstraintHasTooFewRoleSequencesError"/>
			</XmlClassData>
			<XmlClassData TypeName="SetComparisonConstraintHasTooManyRoleSequencesError">
				<DomainRelationshipMoniker Name="SetComparisonConstraintHasTooManyRoleSequencesError"/>
			</XmlClassData>
			<XmlClassData TypeName="ObjectTypeHasDuplicateNameError">
				<DomainRelationshipMoniker Name="ObjectTypeHasDuplicateNameError"/>
			</XmlClassData>
			<XmlClassData TypeName="ReadingOrderHasReading">
				<DomainRelationshipMoniker Name="ReadingOrderHasReading"/>
			</XmlClassData>
			<XmlClassData TypeName="FactTypeHasReadingOrder">
				<DomainRelationshipMoniker Name="FactTypeHasReadingOrder"/>
			</XmlClassData>
			<XmlClassData TypeName="ModelHasReferenceModeKind">
				<DomainRelationshipMoniker Name="ModelHasReferenceModeKind"/>
			</XmlClassData>
			<XmlClassData TypeName="ModelHasReferenceMode">
				<DomainRelationshipMoniker Name="ModelHasReferenceMode"/>
			</XmlClassData>
			<XmlClassData TypeName="ReferenceModeHasReferenceModeKind">
				<DomainRelationshipMoniker Name="ReferenceModeHasReferenceModeKind"/>
			</XmlClassData>
			<XmlClassData TypeName="ModelHasSetConstraint">
				<DomainRelationshipMoniker Name="ModelHasSetConstraint"/>
			</XmlClassData>
			<XmlClassData TypeName="ModelHasSetComparisonConstraint">
				<DomainRelationshipMoniker Name="ModelHasSetComparisonConstraint"/>
			</XmlClassData>
			<XmlClassData TypeName="SetConstraintHasDuplicateNameError">
				<DomainRelationshipMoniker Name="SetConstraintHasDuplicateNameError"/>
			</XmlClassData>
			<XmlClassData TypeName="SetComparisonConstraintHasDuplicateNameError">
				<DomainRelationshipMoniker Name="SetComparisonConstraintHasDuplicateNameError"/>
			</XmlClassData>
			<XmlClassData TypeName="EntityTypeHasPreferredIdentifier">
				<DomainRelationshipMoniker Name="EntityTypeHasPreferredIdentifier"/>
			</XmlClassData>
			<XmlClassData TypeName="ReadingHasTooManyRolesError">
				<DomainRelationshipMoniker Name="ReadingHasTooManyRolesError"/>
			</XmlClassData>
			<XmlClassData TypeName="ReadingHasTooFewRolesError">
				<DomainRelationshipMoniker Name="ReadingHasTooFewRolesError"/>
			</XmlClassData>
			<XmlClassData TypeName="SetComparisonConstraintHasExternalConstraintRoleSequenceArityMismatchError">
				<DomainRelationshipMoniker Name="SetComparisonConstraintHasExternalConstraintRoleSequenceArityMismatchError"/>
			</XmlClassData>
			<XmlClassData TypeName="FactTypeHasFactTypeRequiresReadingError">
				<DomainRelationshipMoniker Name="FactTypeHasFactTypeRequiresReadingError"/>
			</XmlClassData>
			<XmlClassData TypeName="ModelHasDataType">
				<DomainRelationshipMoniker Name="ModelHasDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="FactTypeHasFactTypeRequiresInternalUniquenessConstraintError">
				<DomainRelationshipMoniker Name="FactTypeHasFactTypeRequiresInternalUniquenessConstraintError"/>
			</XmlClassData>
			<XmlClassData TypeName="ValueTypeHasValueConstraint">
				<DomainRelationshipMoniker Name="ValueTypeHasValueConstraint"/>
			</XmlClassData>
			<XmlClassData TypeName="RoleHasValueConstraint">
				<DomainRelationshipMoniker Name="RoleHasValueConstraint"/>
			</XmlClassData>
			<XmlClassData TypeName="ValueConstraintHasValueRange">
				<DomainRelationshipMoniker Name="ValueConstraintHasValueRange"/>
			</XmlClassData>
			<XmlClassData TypeName="ValueTypeHasUnspecifiedDataTypeError">
				<DomainRelationshipMoniker Name="ValueTypeHasUnspecifiedDataTypeError"/>
			</XmlClassData>
			<XmlClassData TypeName="SetComparisonConstraintHasCompatibleRolePlayerTypeError">
				<DomainRelationshipMoniker Name="SetComparisonConstraintHasCompatibleRolePlayerTypeError"/>
			</XmlClassData>
			<XmlClassData TypeName="SetConstraintHasCompatibleRolePlayerTypeError">
				<DomainRelationshipMoniker Name="SetConstraintHasCompatibleRolePlayerTypeError"/>
			</XmlClassData>
			<XmlClassData TypeName="UniquenessConstraintHasNMinusOneError">
				<DomainRelationshipMoniker Name="UniquenessConstraintHasNMinusOneError"/>
			</XmlClassData>
			<XmlClassData TypeName="RoleHasRolePlayerRequiredError">
				<DomainRelationshipMoniker Name="RoleHasRolePlayerRequiredError"/>
			</XmlClassData>
			<XmlClassData TypeName="EqualityConstraintHasEqualityImpliedByMandatoryError">
				<DomainRelationshipMoniker Name="EqualityConstraintHasEqualityImpliedByMandatoryError"/>
			</XmlClassData>
			<XmlClassData TypeName="ObjectTypeHasEntityTypeRequiresReferenceSchemeError">
				<DomainRelationshipMoniker Name="ObjectTypeHasEntityTypeRequiresReferenceSchemeError"/>
			</XmlClassData>
			<XmlClassData TypeName="MandatoryConstraintHasMandatoryImpliedByMandatoryError">
				<DomainRelationshipMoniker Name="MandatoryConstraintHasMandatoryImpliedByMandatoryError"/>
			</XmlClassData>
			<XmlClassData TypeName="FrequencyConstraintHasFrequencyConstraintMinMaxError">
				<DomainRelationshipMoniker Name="FrequencyConstraintHasFrequencyConstraintMinMaxError"/>
			</XmlClassData>
			<!--<XmlClassData TypeName="ORMElementLink" SerializeId="true">
				<DomainRelationshipMoniker Name="ORMElementLink"/>
			</XmlClassData>-->
			<XmlClassData TypeName="ORMModelElementHasExtensionElement">
				<DomainRelationshipMoniker Name="ORMModelElementHasExtensionElement"/>
			</XmlClassData>
			<XmlClassData TypeName="ORMModelElementHasExtensionModelError">
				<DomainRelationshipMoniker Name="ORMModelElementHasExtensionModelError"/>
			</XmlClassData>
			<XmlClassData TypeName="ObjectificationImpliesFactType">
				<DomainRelationshipMoniker Name="ObjectificationImpliesFactType"/>
			</XmlClassData>
			<XmlClassData TypeName="ValueRangeHasMaxValueMismatchError">
				<DomainRelationshipMoniker Name="ValueRangeHasMaxValueMismatchError"/>
			</XmlClassData>
			<XmlClassData TypeName="ValueRangeHasMinValueMismatchError">
				<DomainRelationshipMoniker Name="ValueRangeHasMinValueMismatchError"/>
			</XmlClassData>
			<XmlClassData TypeName="FactTypeHasImpliedInternalUniquenessConstraintError">
				<DomainRelationshipMoniker Name="FactTypeHasImpliedInternalUniquenessConstraintError"/>
			</XmlClassData>
			<XmlClassData TypeName="SetConstraintHasTooFewRoleSequencesError">
				<DomainRelationshipMoniker Name="SetConstraintHasTooFewRoleSequencesError"/>
			</XmlClassData>
			<XmlClassData TypeName="SetConstraintHasTooManyRoleSequencesError">
				<DomainRelationshipMoniker Name="SetConstraintHasTooManyRoleSequencesError"/>
			</XmlClassData>
			<XmlClassData TypeName="FrequencyConstraintHasFrequencyConstraintInvalidatedByInternalUniquenessConstraintError">
				<DomainRelationshipMoniker Name="FrequencyConstraintHasFrequencyConstraintInvalidatedByInternalUniquenessConstraintError"/>
			</XmlClassData>
			<XmlClassData TypeName="FactTypeHasFrequencyConstraintContradictsInternalUniquenessConstraintError">
				<DomainRelationshipMoniker Name="FactTypeHasFrequencyConstraintContradictsInternalUniquenessConstraintError"/>
			</XmlClassData>
			<XmlClassData TypeName="RingConstraintHasRingConstraintTypeNotSpecifiedError">
				<DomainRelationshipMoniker Name="RingConstraintHasRingConstraintTypeNotSpecifiedError"/>
			</XmlClassData>
			<XmlClassData TypeName="ValueConstraintHasDuplicateNameError">
				<DomainRelationshipMoniker Name="ValueConstraintHasDuplicateNameError"/>
			</XmlClassData>
			<XmlClassData TypeName="ObjectTypeHasObjectTypeRequiresPrimarySupertypeError">
				<DomainRelationshipMoniker Name="ObjectTypeHasObjectTypeRequiresPrimarySupertypeError"/>
			</XmlClassData>
			<XmlClassData TypeName="FactTypeHasNote">
				<DomainRelationshipMoniker Name="FactTypeHasNote"/>
			</XmlClassData>
			<XmlClassData TypeName="ObjectTypeHasNote">
				<DomainRelationshipMoniker Name="ObjectTypeHasNote"/>
			</XmlClassData>
			<XmlClassData TypeName="ObjectTypeHasCompatibleSupertypesError">
				<DomainRelationshipMoniker Name="ObjectTypeHasCompatibleSupertypesError"/>
			</XmlClassData>
			<XmlClassData TypeName="UniquenessConstraintHasUniquenessImpliedByUniquenessError">
				<DomainRelationshipMoniker Name="UniquenessConstraintHasUniquenessImpliedByUniquenessError"/>
			</XmlClassData>
			<XmlClassData TypeName="ObjectTypeHasPreferredIdentifierRequiresMandatoryError">
				<DomainRelationshipMoniker Name="ObjectTypeHasPreferredIdentifierRequiresMandatoryError"/>
			</XmlClassData>
			<XmlClassData TypeName="ValueConstraintHasValueRangeOverlapError">
				<DomainRelationshipMoniker Name="ValueConstraintHasValueRangeOverlapError"/>
			</XmlClassData>
			<XmlClassData TypeName="FactTypeHasRole">
				<DomainRelationshipMoniker Name="FactTypeHasRole"/>
			</XmlClassData>
			<XmlClassData TypeName="ReadingOrderHasRole" SerializeId="true">
				<DomainRelationshipMoniker Name="ReadingOrderHasRole"/>
			</XmlClassData>
			<XmlClassData TypeName="RoleProxyHasRole">
				<DomainRelationshipMoniker Name="RoleProxyHasRole"/>
			</XmlClassData>
			<XmlClassData TypeName="FactTypeHasDerivationExpression">
				<DomainRelationshipMoniker Name="FactTypeHasDerivationExpression"/>
			</XmlClassData>
			<XmlClassData TypeName="EntityTypeHasEntityTypeInstance">
				<DomainRelationshipMoniker Name="EntityTypeHasEntityTypeInstance"/>
			</XmlClassData>
			<XmlClassData TypeName="ValueTypeHasValueTypeInstance">
				<DomainRelationshipMoniker Name="ValueTypeHasValueTypeInstance"/>
			</XmlClassData>
			<XmlClassData TypeName="RoleInstance" SerializeId="true">
				<DomainRelationshipMoniker Name="RoleInstance"/>
			</XmlClassData>
			<XmlClassData TypeName="EntityTypeRoleInstance" SerializeId="true">
				<DomainRelationshipMoniker Name="EntityTypeRoleInstance"/>
			</XmlClassData>
			<XmlClassData TypeName="FactTypeRoleInstance" SerializeId="true">
				<DomainRelationshipMoniker Name="FactTypeRoleInstance"/>
			</XmlClassData>
			<XmlClassData TypeName="EntityTypeInstanceHasRoleInstance">
				<DomainRelationshipMoniker Name="EntityTypeInstanceHasRoleInstance"/>
			</XmlClassData>
			<XmlClassData TypeName="FactTypeHasFactTypeInstance">
				<DomainRelationshipMoniker Name="FactTypeHasFactTypeInstance"/>
			</XmlClassData>
			<XmlClassData TypeName="FactTypeInstanceHasRoleInstance">
				<DomainRelationshipMoniker Name="FactTypeInstanceHasRoleInstance"/>
			</XmlClassData>
			<XmlClassData TypeName="EntityTypeInstanceHasTooFewEntityTypeRoleInstancesError">
				<DomainRelationshipMoniker Name="EntityTypeInstanceHasTooFewEntityTypeRoleInstancesError"/>
			</XmlClassData>
			<XmlClassData TypeName="FactTypeInstanceHasTooFewFactTypeRoleInstancesError">
				<DomainRelationshipMoniker Name="FactTypeInstanceHasTooFewFactTypeRoleInstancesError"/>
			</XmlClassData>
			<XmlClassData TypeName="ValueTypeInstanceHasCompatibleValueTypeInstanceValueError">
				<DomainRelationshipMoniker Name="ValueTypeInstanceHasCompatibleValueTypeInstanceValueError"/>
			</XmlClassData>
			<XmlClassData TypeName="ObjectType">
				<DomainClassMoniker Name="ObjectType"/>
				<ElementData>
					<XmlPropertyData XmlName="IsExternal">
						<DomainPropertyMoniker Name="ObjectType/IsExternal"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="NoteText">
						<DomainPropertyMoniker Name="ObjectType/NoteText"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="IsIndependent">
						<DomainPropertyMoniker Name="ObjectType/IsIndependent"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="IsValueType">
						<DomainPropertyMoniker Name="ObjectType/IsValueType"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="Scale">
						<DomainPropertyMoniker Name="ObjectType/Scale"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="Length">
						<DomainPropertyMoniker Name="ObjectType/Length"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="NestedFactTypeDisplay">
						<DomainPropertyMoniker Name="ObjectType/NestedFactTypeDisplay"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="ReferenceModeDisplay">
						<DomainPropertyMoniker Name="ObjectType/ReferenceModeDisplay"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="ReferenceModeString">
						<DomainPropertyMoniker Name="ObjectType/ReferenceModeString"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="ReferenceMode">
						<DomainPropertyMoniker Name="ObjectType/ReferenceMode"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="DataTypeDisplay">
						<DomainPropertyMoniker Name="ObjectType/DataTypeDisplay"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="ValueRangeText">
						<DomainPropertyMoniker Name="ObjectType/ValueRangeText"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="IsPersonal">
						<DomainPropertyMoniker Name="ObjectType/IsPersonal"/>
					</XmlPropertyData>

					<XmlRelationshipData RoleElementName="DataType" UseFullForm="true">
						<!-- This relationship has Properties or is many-many, so UseFullForm is required -->
						<DomainRelationshipMoniker Name="ValueTypeHasDataType"/>
						<!-- source role = ValueTypeCollection -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="NestedFactType" UseFullForm="true">
						<!-- This relationship has Properties or is many-many, so UseFullForm is required -->
						<DomainRelationshipMoniker Name="Objectification"/>
						<!-- source role = NestingType -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="DuplicateNameError">
						<DomainRelationshipMoniker Name="ObjectTypeHasDuplicateNameError"/>
						<!-- source role = ObjectTypeCollection -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="PreferredIdentifier">
						<DomainRelationshipMoniker Name="EntityTypeHasPreferredIdentifier"/>
						<!-- source role = PreferredIdentifierFor -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="ValueConstraint">
						<DomainRelationshipMoniker Name="ValueTypeHasValueConstraint"/>
						<!-- source role = ValueType -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="ReferenceSchemeError">
						<DomainRelationshipMoniker Name="ObjectTypeHasEntityTypeRequiresReferenceSchemeError"/>
						<!-- source role = ObjectType -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="ObjectTypeRequiresPrimarySupertypeError">
						<DomainRelationshipMoniker Name="ObjectTypeHasObjectTypeRequiresPrimarySupertypeError"/>
						<!-- source role = ObjectType -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="Note">
						<DomainRelationshipMoniker Name="ObjectTypeHasNote"/>
						<!-- source role = ObjectType -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="CompatibleSupertypesError">
						<DomainRelationshipMoniker Name="ObjectTypeHasCompatibleSupertypesError"/>
						<!-- source role = ObjectType -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="PreferredIdentifierRequiresMandatoryError">
						<DomainRelationshipMoniker Name="ObjectTypeHasPreferredIdentifierRequiresMandatoryError"/>
						<!-- source role = ObjectType -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="EntityTypeInstanceCollection">
						<DomainRelationshipMoniker Name="EntityTypeHasEntityTypeInstance"/>
						<!-- source role = EntityType -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="ValueTypeInstanceCollection">
						<DomainRelationshipMoniker Name="ValueTypeHasValueTypeInstance"/>
						<!-- source role = ValueType -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="SubtypeFact">
				<DomainClassMoniker Name="SubtypeFact"/>
				<ElementData>
					<XmlPropertyData XmlName="IsPrimary">
						<DomainPropertyMoniker Name="SubtypeFact/IsPrimary"/>
					</XmlPropertyData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="FactType">
				<DomainClassMoniker Name="FactType"/>
				<ElementData>
					<XmlPropertyData XmlName="IsExternal">
						<DomainPropertyMoniker Name="FactType/IsExternal"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="NoteText">
						<DomainPropertyMoniker Name="FactType/NoteText"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="Name">
						<DomainPropertyMoniker Name="FactType/Name"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="NestingTypeDisplay">
						<DomainPropertyMoniker Name="FactType/NestingTypeDisplay"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="DerivationRuleDisplay">
						<DomainPropertyMoniker Name="FactType/DerivationRuleDisplay"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="DerivationStorageDisplay">
						<DomainPropertyMoniker Name="FactType/DerivationStorageDisplay"/>
					</XmlPropertyData>

					<XmlRelationshipData RoleElementName="SetComparisonConstraintCollection" UseFullForm="true">
						<!-- This relationship has Properties or is many-many, so UseFullForm is required -->
						<DomainRelationshipMoniker Name="FactSetComparisonConstraint"/>
						<!-- source role = FactTypeCollection -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="SetConstraintCollection" UseFullForm="true">
						<!-- This relationship has Properties or is many-many, so UseFullForm is required -->
						<DomainRelationshipMoniker Name="FactSetConstraint"/>
						<!-- source role = FactTypeCollection -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="ReadingOrderCollection">
						<DomainRelationshipMoniker Name="FactTypeHasReadingOrder"/>
						<!-- source role = FactType -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="ReadingRequiredError">
						<DomainRelationshipMoniker Name="FactTypeHasFactTypeRequiresReadingError"/>
						<!-- source role = FactType -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="InternalUniquenessConstraintRequiredError">
						<DomainRelationshipMoniker Name="FactTypeHasFactTypeRequiresInternalUniquenessConstraintError"/>
						<!-- source role = FactType -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="ImpliedInternalUniquenessConstraintError">
						<DomainRelationshipMoniker Name="FactTypeHasImpliedInternalUniquenessConstraintError"/>
						<!-- source role = FactType -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="FrequencyConstraintContradictsInternalUniquenessConstraintErrorCollection">
						<DomainRelationshipMoniker Name="FactTypeHasFrequencyConstraintContradictsInternalUniquenessConstraintError"/>
						<!-- source role = FactType -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="Note">
						<DomainRelationshipMoniker Name="FactTypeHasNote"/>
						<!-- source role = FactType -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="RoleCollection">
						<DomainRelationshipMoniker Name="FactTypeHasRole"/>
						<!-- source role = FactType -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="DerivationRule">
						<DomainRelationshipMoniker Name="FactTypeHasDerivationExpression"/>
						<!-- source role = FactType -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="FactTypeInstanceCollection">
						<DomainRelationshipMoniker Name="FactTypeHasFactTypeInstance"/>
						<!-- source role = FactType -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="EqualityConstraint">
				<DomainClassMoniker Name="EqualityConstraint"/>
				<ElementData>

					<XmlRelationshipData RoleElementName="EqualityImpliedByMandatoryError">
						<DomainRelationshipMoniker Name="EqualityConstraintHasEqualityImpliedByMandatoryError"/>
						<!-- source role = EqualityConstraint -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="ExclusionConstraint">
				<DomainClassMoniker Name="ExclusionConstraint"/>
			</XmlClassData>
			<XmlClassData TypeName="SubsetConstraint">
				<DomainClassMoniker Name="SubsetConstraint"/>
			</XmlClassData>
			<XmlClassData TypeName="SetComparisonConstraint">
				<DomainClassMoniker Name="SetComparisonConstraint"/>
				<ElementData>
					<XmlPropertyData XmlName="Modality">
						<DomainPropertyMoniker Name="SetComparisonConstraint/Modality"/>
					</XmlPropertyData>

					<XmlRelationshipData RoleElementName="RoleSequenceCollection">
						<DomainRelationshipMoniker Name="SetComparisonConstraintHasRoleSequence"/>
						<!-- source role = ExternalConstraint -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="TooFewRoleSequencesError">
						<DomainRelationshipMoniker Name="SetComparisonConstraintHasTooFewRoleSequencesError"/>
						<!-- source role = SetComparisonConstraint -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="TooManyRoleSequencesError">
						<DomainRelationshipMoniker Name="SetComparisonConstraintHasTooManyRoleSequencesError"/>
						<!-- source role = SetComparisonConstraint -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="DuplicateNameError">
						<DomainRelationshipMoniker Name="SetComparisonConstraintHasDuplicateNameError"/>
						<!-- source role = SetComparisonConstraintCollection -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="ArityMismatchError">
						<DomainRelationshipMoniker Name="SetComparisonConstraintHasExternalConstraintRoleSequenceArityMismatchError"/>
						<!-- source role = Constraint -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="CompatibleRolePlayerTypeErrorCollection">
						<DomainRelationshipMoniker Name="SetComparisonConstraintHasCompatibleRolePlayerTypeError"/>
						<!-- source role = SetComparisonConstraint -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="Expression">
				<DomainClassMoniker Name="Expression"/>
				<ElementData>
					<XmlPropertyData XmlName="Body">
						<DomainPropertyMoniker Name="Expression/Body"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="Language">
						<DomainPropertyMoniker Name="Expression/Language"/>
					</XmlPropertyData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="Role">
				<DomainClassMoniker Name="Role"/>
				<ElementData>
					<XmlPropertyData XmlName="RolePlayerDisplay">
						<DomainPropertyMoniker Name="Role/RolePlayerDisplay"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="IsMandatory">
						<DomainPropertyMoniker Name="Role/IsMandatory"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="Multiplicity">
						<DomainPropertyMoniker Name="Role/Multiplicity"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="ValueRangeText">
						<DomainPropertyMoniker Name="Role/ValueRangeText"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="MandatoryConstraintName">
						<DomainPropertyMoniker Name="Role/MandatoryConstraintName"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="MandatoryConstraintModality">
						<DomainPropertyMoniker Name="Role/MandatoryConstraintModality"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="Name">
						<DomainPropertyMoniker Name="Role/Name"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="ObjectificationOppositeRoleName">
						<DomainPropertyMoniker Name="Role/ObjectificationOppositeRoleName"/>
					</XmlPropertyData>

					<XmlRelationshipData RoleElementName="RolePlayer">
						<DomainRelationshipMoniker Name="ObjectTypePlaysRole"/>
						<!-- source role = PlayedRoleCollection -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="ConstraintRoleSequenceCollection" UseFullForm="true">
						<!-- This relationship has Properties or is many-many, so UseFullForm is required -->
						<DomainRelationshipMoniker Name="ConstraintRoleSequenceHasRole"/>
						<!-- source role = RoleCollection -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="ValueConstraint">
						<DomainRelationshipMoniker Name="RoleHasValueConstraint"/>
						<!-- source role = Role -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="RolePlayerRequiredError">
						<DomainRelationshipMoniker Name="RoleHasRolePlayerRequiredError"/>
						<!-- source role = Role -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="ObjectTypeInstanceCollection" UseFullForm="true">
						<!-- This relationship has Properties or is many-many, so UseFullForm is required -->
						<DomainRelationshipMoniker Name="RoleInstance"/>
						<!-- source role = RoleCollection -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="SetComparisonConstraintRoleSequence">
				<DomainClassMoniker Name="SetComparisonConstraintRoleSequence"/>
			</XmlClassData>
			<XmlClassData TypeName="RingConstraint">
				<DomainClassMoniker Name="RingConstraint"/>
				<ElementData>
					<XmlPropertyData XmlName="RingType">
						<DomainPropertyMoniker Name="RingConstraint/RingType"/>
					</XmlPropertyData>

					<XmlRelationshipData RoleElementName="RingConstraintTypeNotSpecifiedError">
						<DomainRelationshipMoniker Name="RingConstraintHasRingConstraintTypeNotSpecifiedError"/>
						<!-- source role = RingConstraint -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="FrequencyConstraint">
				<DomainClassMoniker Name="FrequencyConstraint"/>
				<ElementData>
					<XmlPropertyData XmlName="MinFrequency">
						<DomainPropertyMoniker Name="FrequencyConstraint/MinFrequency"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="MaxFrequency">
						<DomainPropertyMoniker Name="FrequencyConstraint/MaxFrequency"/>
					</XmlPropertyData>

					<XmlRelationshipData RoleElementName="FrequencyConstraintMinMaxError">
						<DomainRelationshipMoniker Name="FrequencyConstraintHasFrequencyConstraintMinMaxError"/>
						<!-- source role = FrequencyConstraint -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="FrequencyConstraintContradictsInternalUniquenessConstraintErrorCollection">
						<DomainRelationshipMoniker Name="FrequencyConstraintHasFrequencyConstraintInvalidatedByInternalUniquenessConstraintError"/>
						<!-- source role = FrequencyConstraint -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="UniquenessConstraint">
				<DomainClassMoniker Name="UniquenessConstraint"/>
				<ElementData>
					<XmlPropertyData XmlName="IsPreferred">
						<DomainPropertyMoniker Name="UniquenessConstraint/IsPreferred"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="IsInternal">
						<DomainPropertyMoniker Name="UniquenessConstraint/IsInternal"/>
					</XmlPropertyData>

					<XmlRelationshipData RoleElementName="NMinusOneError">
						<DomainRelationshipMoniker Name="UniquenessConstraintHasNMinusOneError"/>
						<!-- source role = Constraint -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="ImpliedByUniquenessError">
						<DomainRelationshipMoniker Name="UniquenessConstraintHasUniquenessImpliedByUniquenessError"/>
						<!-- source role = UniquenessConstraint -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="MandatoryConstraint">
				<DomainClassMoniker Name="MandatoryConstraint"/>
				<ElementData>
					<XmlPropertyData XmlName="IsSimple">
						<DomainPropertyMoniker Name="MandatoryConstraint/IsSimple"/>
					</XmlPropertyData>

					<XmlRelationshipData RoleElementName="ImpliedByMandatoryError">
						<DomainRelationshipMoniker Name="MandatoryConstraintHasMandatoryImpliedByMandatoryError"/>
						<!-- source role = MandatoryConstraint -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="SetConstraint">
				<DomainClassMoniker Name="SetConstraint"/>
				<ElementData>
					<XmlPropertyData XmlName="Modality">
						<DomainPropertyMoniker Name="SetConstraint/Modality"/>
					</XmlPropertyData>

					<XmlRelationshipData RoleElementName="DuplicateNameError">
						<DomainRelationshipMoniker Name="SetConstraintHasDuplicateNameError"/>
						<!-- source role = SetConstraintCollection -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="CompatibleRolePlayerTypeError">
						<DomainRelationshipMoniker Name="SetConstraintHasCompatibleRolePlayerTypeError"/>
						<!-- source role = SetConstraint -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="TooFewRoleSequencesError">
						<DomainRelationshipMoniker Name="SetConstraintHasTooFewRoleSequencesError"/>
						<!-- source role = SetConstraint -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="TooManyRoleSequencesError">
						<DomainRelationshipMoniker Name="SetConstraintHasTooManyRoleSequencesError"/>
						<!-- source role = SetConstraint -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="ConstraintRoleSequence">
				<DomainClassMoniker Name="ConstraintRoleSequence"/>
			</XmlClassData>
			<XmlClassData TypeName="TooFewRoleSequencesError">
				<DomainClassMoniker Name="TooFewRoleSequencesError"/>
			</XmlClassData>
			<XmlClassData TypeName="TooManyRoleSequencesError">
				<DomainClassMoniker Name="TooManyRoleSequencesError"/>
			</XmlClassData>
			<XmlClassData TypeName="ObjectTypeDuplicateNameError">
				<DomainClassMoniker Name="ObjectTypeDuplicateNameError"/>
			</XmlClassData>
			<XmlClassData TypeName="ConstraintDuplicateNameError">
				<DomainClassMoniker Name="ConstraintDuplicateNameError"/>
			</XmlClassData>
			<XmlClassData TypeName="DuplicateNameError">
				<DomainClassMoniker Name="DuplicateNameError"/>
			</XmlClassData>
			<XmlClassData TypeName="TooFewReadingRolesError">
				<DomainClassMoniker Name="TooFewReadingRolesError"/>
			</XmlClassData>
			<XmlClassData TypeName="TooManyReadingRolesError">
				<DomainClassMoniker Name="TooManyReadingRolesError"/>
			</XmlClassData>
			<XmlClassData TypeName="ExternalConstraintRoleSequenceArityMismatchError">
				<DomainClassMoniker Name="ExternalConstraintRoleSequenceArityMismatchError"/>
			</XmlClassData>
			<XmlClassData TypeName="FactTypeRequiresReadingError">
				<DomainClassMoniker Name="FactTypeRequiresReadingError"/>
			</XmlClassData>
			<XmlClassData TypeName="FactTypeRequiresInternalUniquenessConstraintError">
				<DomainClassMoniker Name="FactTypeRequiresInternalUniquenessConstraintError"/>
			</XmlClassData>
			<XmlClassData TypeName="DataTypeNotSpecifiedError">
				<DomainClassMoniker Name="DataTypeNotSpecifiedError"/>
			</XmlClassData>
			<XmlClassData TypeName="NMinusOneError">
				<DomainClassMoniker Name="NMinusOneError"/>
			</XmlClassData>
			<XmlClassData TypeName="CompatibleRolePlayerTypeError">
				<DomainClassMoniker Name="CompatibleRolePlayerTypeError"/>
				<ElementData>
					<XmlPropertyData XmlName="Column">
						<DomainPropertyMoniker Name="CompatibleRolePlayerTypeError/Column"/>
					</XmlPropertyData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="RolePlayerRequiredError">
				<DomainClassMoniker Name="RolePlayerRequiredError"/>
			</XmlClassData>
			<XmlClassData TypeName="EqualityImpliedByMandatoryError">
				<DomainClassMoniker Name="EqualityImpliedByMandatoryError"/>
			</XmlClassData>
			<XmlClassData TypeName="EntityTypeRequiresReferenceSchemeError">
				<DomainClassMoniker Name="EntityTypeRequiresReferenceSchemeError"/>
			</XmlClassData>
			<XmlClassData TypeName="MandatoryImpliedByMandatoryError">
				<DomainClassMoniker Name="MandatoryImpliedByMandatoryError"/>
			</XmlClassData>
			<XmlClassData TypeName="FrequencyConstraintMinMaxError">
				<DomainClassMoniker Name="FrequencyConstraintMinMaxError"/>
			</XmlClassData>
			<XmlClassData TypeName="ModelError">
				<DomainClassMoniker Name="ModelError"/>
			</XmlClassData>
			<XmlClassData TypeName="ReferenceModeKind">
				<DomainClassMoniker Name="ReferenceModeKind"/>
				<ElementData>
					<XmlPropertyData XmlName="FormatString">
						<DomainPropertyMoniker Name="ReferenceModeKind/FormatString"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="ReferenceModeType">
						<DomainPropertyMoniker Name="ReferenceModeKind/ReferenceModeType"/>
					</XmlPropertyData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="IntrinsicReferenceMode">
				<DomainClassMoniker Name="IntrinsicReferenceMode"/>
			</XmlClassData>
			<XmlClassData TypeName="CustomReferenceMode">
				<DomainClassMoniker Name="CustomReferenceMode"/>
				<ElementData>
					<XmlPropertyData XmlName="CustomFormatString">
						<DomainPropertyMoniker Name="CustomReferenceMode/CustomFormatString"/>
					</XmlPropertyData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="ReferenceMode">
				<DomainClassMoniker Name="ReferenceMode"/>
				<ElementData>
					<XmlPropertyData XmlName="KindDisplay">
						<DomainPropertyMoniker Name="ReferenceMode/KindDisplay"/>
					</XmlPropertyData>

					<XmlRelationshipData RoleElementName="Kind">
						<DomainRelationshipMoniker Name="ReferenceModeHasReferenceModeKind"/>
						<!-- source role = ReferenceModeCollection -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="ORMNamedElement">
				<DomainClassMoniker Name="ORMNamedElement"/>
				<ElementData>
					<XmlPropertyData XmlName="Name" IsMonikerKey="true">
						<DomainPropertyMoniker Name="ORMNamedElement/Name"/>
					</XmlPropertyData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="UnspecifiedDataType">
				<DomainClassMoniker Name="UnspecifiedDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="FixedLengthTextDataType">
				<DomainClassMoniker Name="FixedLengthTextDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="VariableLengthTextDataType">
				<DomainClassMoniker Name="VariableLengthTextDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="LargeLengthTextDataType">
				<DomainClassMoniker Name="LargeLengthTextDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="TextDataType">
				<DomainClassMoniker Name="TextDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="SignedIntegerNumericDataType">
				<DomainClassMoniker Name="SignedIntegerNumericDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="UnsignedIntegerNumericDataType">
				<DomainClassMoniker Name="UnsignedIntegerNumericDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="AutoCounterNumericDataType">
				<DomainClassMoniker Name="AutoCounterNumericDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="FloatingPointNumericDataType">
				<DomainClassMoniker Name="FloatingPointNumericDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="DecimalNumericDataType">
				<DomainClassMoniker Name="DecimalNumericDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="MoneyNumericDataType">
				<DomainClassMoniker Name="MoneyNumericDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="NumericDataType">
				<DomainClassMoniker Name="NumericDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="FixedLengthRawDataDataType">
				<DomainClassMoniker Name="FixedLengthRawDataDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="VariableLengthRawDataDataType">
				<DomainClassMoniker Name="VariableLengthRawDataDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="LargeLengthRawDataDataType">
				<DomainClassMoniker Name="LargeLengthRawDataDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="PictureRawDataDataType">
				<DomainClassMoniker Name="PictureRawDataDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="OleObjectRawDataDataType">
				<DomainClassMoniker Name="OleObjectRawDataDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="RawDataDataType">
				<DomainClassMoniker Name="RawDataDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="AutoTimestampTemporalDataType">
				<DomainClassMoniker Name="AutoTimestampTemporalDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="TimeTemporalDataType">
				<DomainClassMoniker Name="TimeTemporalDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="DateTemporalDataType">
				<DomainClassMoniker Name="DateTemporalDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="DateAndTimeTemporalDataType">
				<DomainClassMoniker Name="DateAndTimeTemporalDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="TemporalDataType">
				<DomainClassMoniker Name="TemporalDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="TrueOrFalseLogicalDataType">
				<DomainClassMoniker Name="TrueOrFalseLogicalDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="YesOrNoLogicalDataType">
				<DomainClassMoniker Name="YesOrNoLogicalDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="LogicalDataType">
				<DomainClassMoniker Name="LogicalDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="RowIdOtherDataType">
				<DomainClassMoniker Name="RowIdOtherDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="ObjectIdOtherDataType">
				<DomainClassMoniker Name="ObjectIdOtherDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="OtherDataType">
				<DomainClassMoniker Name="OtherDataType"/>
			</XmlClassData>
			<XmlClassData TypeName="DataType">
				<DomainClassMoniker Name="DataType"/>
			</XmlClassData>
			<XmlClassData TypeName="ORMModel">
				<DomainClassMoniker Name="ORMModel"/>
				<ElementData>

					<XmlRelationshipData RoleElementName="ObjectTypeCollection">
						<DomainRelationshipMoniker Name="ModelHasObjectType"/>
						<!-- source role = Model -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="FactTypeCollection">
						<DomainRelationshipMoniker Name="ModelHasFactType"/>
						<!-- source role = Model -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="ErrorCollection">
						<DomainRelationshipMoniker Name="ModelHasError"/>
						<!-- source role = Model -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="ReferenceModeKindCollection">
						<DomainRelationshipMoniker Name="ModelHasReferenceModeKind"/>
						<!-- source role = Model -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="ReferenceModeCollection">
						<DomainRelationshipMoniker Name="ModelHasReferenceMode"/>
						<!-- source role = Model -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="SetConstraintCollection">
						<DomainRelationshipMoniker Name="ModelHasSetConstraint"/>
						<!-- source role = Model -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="SetComparisonConstraintCollection">
						<DomainRelationshipMoniker Name="ModelHasSetComparisonConstraint"/>
						<!-- source role = Model -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="DataTypeCollection">
						<DomainRelationshipMoniker Name="ModelHasDataType"/>
						<!-- source role = Model -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="Reading">
				<DomainClassMoniker Name="Reading"/>
				<ElementData>
					<XmlPropertyData XmlName="Text">
						<DomainPropertyMoniker Name="Reading/Text"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="IsPrimaryForReadingOrder">
						<DomainPropertyMoniker Name="Reading/IsPrimaryForReadingOrder"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="Language">
						<DomainPropertyMoniker Name="Reading/Language"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="IsPrimaryForFactType">
						<DomainPropertyMoniker Name="Reading/IsPrimaryForFactType"/>
					</XmlPropertyData>

					<XmlRelationshipData RoleElementName="TooManyRolesError">
						<DomainRelationshipMoniker Name="ReadingHasTooManyRolesError"/>
						<!-- source role = Reading -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="TooFewRolesError">
						<DomainRelationshipMoniker Name="ReadingHasTooFewRolesError"/>
						<!-- source role = Reading -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="ReadingOrder">
				<DomainClassMoniker Name="ReadingOrder"/>
				<ElementData>
					<XmlPropertyData XmlName="ReadingText">
						<DomainPropertyMoniker Name="ReadingOrder/ReadingText"/>
					</XmlPropertyData>

					<XmlRelationshipData RoleElementName="ReadingCollection">
						<DomainRelationshipMoniker Name="ReadingOrderHasReading"/>
						<!-- source role = ReadingOrder -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="RoleCollection" UseFullForm="true">
						<!-- This relationship has Properties or is many-many, so UseFullForm is required -->
						<DomainRelationshipMoniker Name="ReadingOrderHasRole"/>
						<!-- source role = ReadingOrder -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="ValueRange">
				<DomainClassMoniker Name="ValueRange"/>
				<ElementData>
					<XmlPropertyData XmlName="MinValue">
						<DomainPropertyMoniker Name="ValueRange/MinValue"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="MaxValue">
						<DomainPropertyMoniker Name="ValueRange/MaxValue"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="Text">
						<DomainPropertyMoniker Name="ValueRange/Text"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="MinInclusion">
						<DomainPropertyMoniker Name="ValueRange/MinInclusion"/>
					</XmlPropertyData>
					<XmlPropertyData XmlName="MaxInclusion">
						<DomainPropertyMoniker Name="ValueRange/MaxInclusion"/>
					</XmlPropertyData>

					<XmlRelationshipData RoleElementName="MaxValueMismatchError">
						<DomainRelationshipMoniker Name="ValueRangeHasMaxValueMismatchError"/>
						<!-- source role = ValueRange -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="MinValueMismatchError">
						<DomainRelationshipMoniker Name="ValueRangeHasMinValueMismatchError"/>
						<!-- source role = ValueRange -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="ValueTypeValueConstraint">
				<DomainClassMoniker Name="ValueTypeValueConstraint"/>
			</XmlClassData>
			<XmlClassData TypeName="RoleValueConstraint">
				<DomainClassMoniker Name="RoleValueConstraint"/>
			</XmlClassData>
			<XmlClassData TypeName="ValueConstraint">
				<DomainClassMoniker Name="ValueConstraint"/>
				<ElementData>
					<XmlPropertyData XmlName="Text">
						<DomainPropertyMoniker Name="ValueConstraint/Text"/>
					</XmlPropertyData>

					<XmlRelationshipData RoleElementName="ValueRangeCollection">
						<DomainRelationshipMoniker Name="ValueConstraintHasValueRange"/>
						<!-- source role = ValueConstraint -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="DuplicateNameError">
						<DomainRelationshipMoniker Name="ValueConstraintHasDuplicateNameError"/>
						<!-- source role = ValueConstraintCollection -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="ValueRangeOverlapError">
						<DomainRelationshipMoniker Name="ValueConstraintHasValueRangeOverlapError"/>
						<!-- source role = ValueConstraint -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="ORMModelElement">
				<DomainClassMoniker Name="ORMModelElement"/>
				<ElementData>

					<XmlRelationshipData RoleElementName="ExtensionCollection">
						<DomainRelationshipMoniker Name="ORMModelElementHasExtensionElement"/>
						<!-- source role = ExtendedElement -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="ExtensionModelErrorCollection">
						<DomainRelationshipMoniker Name="ORMModelElementHasExtensionModelError"/>
						<!-- source role = ExtendedElement -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="ValueMismatchError">
				<DomainClassMoniker Name="ValueMismatchError"/>
			</XmlClassData>
			<XmlClassData TypeName="MinValueMismatchError">
				<DomainClassMoniker Name="MinValueMismatchError"/>
			</XmlClassData>
			<XmlClassData TypeName="MaxValueMismatchError">
				<DomainClassMoniker Name="MaxValueMismatchError"/>
			</XmlClassData>
			<XmlClassData TypeName="ImpliedInternalUniquenessConstraintError">
				<DomainClassMoniker Name="ImpliedInternalUniquenessConstraintError"/>
			</XmlClassData>
			<XmlClassData TypeName="FrequencyConstraintContradictsInternalUniquenessConstraintError">
				<DomainClassMoniker Name="FrequencyConstraintContradictsInternalUniquenessConstraintError"/>
			</XmlClassData>
			<XmlClassData TypeName="RingConstraintTypeNotSpecifiedError">
				<DomainClassMoniker Name="RingConstraintTypeNotSpecifiedError"/>
			</XmlClassData>
			<XmlClassData TypeName="SubtypeMetaRole">
				<DomainClassMoniker Name="SubtypeMetaRole"/>
			</XmlClassData>
			<XmlClassData TypeName="SupertypeMetaRole">
				<DomainClassMoniker Name="SupertypeMetaRole"/>
			</XmlClassData>
			<XmlClassData TypeName="ObjectTypeRequiresPrimarySupertypeError">
				<DomainClassMoniker Name="ObjectTypeRequiresPrimarySupertypeError"/>
			</XmlClassData>
			<XmlClassData TypeName="Note">
				<DomainClassMoniker Name="Note"/>
				<ElementData>
					<XmlPropertyData XmlName="Text">
						<DomainPropertyMoniker Name="Note/Text"/>
					</XmlPropertyData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="CompatibleSupertypesError">
				<DomainClassMoniker Name="CompatibleSupertypesError"/>
			</XmlClassData>
			<XmlClassData TypeName="UniquenessImpliedByUniquenessError">
				<DomainClassMoniker Name="UniquenessImpliedByUniquenessError"/>
			</XmlClassData>
			<XmlClassData TypeName="PreferredIdentifierRequiresMandatoryError">
				<DomainClassMoniker Name="PreferredIdentifierRequiresMandatoryError"/>
			</XmlClassData>
			<XmlClassData TypeName="ValueRangeOverlapError">
				<DomainClassMoniker Name="ValueRangeOverlapError"/>
			</XmlClassData>
			<XmlClassData TypeName="RoleBase">
				<DomainClassMoniker Name="RoleBase"/>
			</XmlClassData>
			<XmlClassData TypeName="RoleProxy">
				<DomainClassMoniker Name="RoleProxy"/>
				<ElementData>

					<XmlRelationshipData RoleElementName="TargetRole">
						<DomainRelationshipMoniker Name="RoleProxyHasRole"/>
						<!-- source role = Proxy -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="FactTypeDerivationExpression">
				<DomainClassMoniker Name="FactTypeDerivationExpression"/>
				<ElementData>
					<XmlPropertyData XmlName="DerivationStorage">
						<DomainPropertyMoniker Name="FactTypeDerivationExpression/DerivationStorage"/>
					</XmlPropertyData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="ObjectTypeInstance">
				<DomainClassMoniker Name="ObjectTypeInstance"/>
			</XmlClassData>
			<XmlClassData TypeName="EntityTypeInstance">
				<DomainClassMoniker Name="EntityTypeInstance"/>
				<ElementData>

					<XmlRelationshipData RoleElementName="RoleInstanceCollection">
						<DomainRelationshipMoniker Name="EntityTypeInstanceHasRoleInstance"/>
						<!-- source role = EntityTypeInstance -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="TooFewEntityTypeRoleInstancesError">
						<DomainRelationshipMoniker Name="EntityTypeInstanceHasTooFewEntityTypeRoleInstancesError"/>
						<!-- source role = EntityTypeInstance -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="ValueTypeInstance">
				<DomainClassMoniker Name="ValueTypeInstance"/>
				<ElementData>
					<XmlPropertyData XmlName="Value">
						<DomainPropertyMoniker Name="ValueTypeInstance/Value"/>
					</XmlPropertyData>

					<XmlRelationshipData RoleElementName="CompatibleValueTypeInstanceValueError">
						<DomainRelationshipMoniker Name="ValueTypeInstanceHasCompatibleValueTypeInstanceValueError"/>
						<!-- source role = ValueTypeInstance -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="FactTypeInstance">
				<DomainClassMoniker Name="FactTypeInstance"/>
				<ElementData>

					<XmlRelationshipData RoleElementName="RoleInstanceCollection">
						<DomainRelationshipMoniker Name="FactTypeInstanceHasRoleInstance"/>
						<!-- source role = FactTypeInstance -->
					</XmlRelationshipData>

					<XmlRelationshipData RoleElementName="TooFewFactTypeRoleInstancesError">
						<DomainRelationshipMoniker Name="FactTypeInstanceHasTooFewFactTypeRoleInstancesError"/>
						<!-- source role = FactTypeInstance -->
					</XmlRelationshipData>
				</ElementData>
			</XmlClassData>
			<XmlClassData TypeName="TooFewEntityTypeRoleInstancesError">
				<DomainClassMoniker Name="TooFewEntityTypeRoleInstancesError"/>
			</XmlClassData>
			<XmlClassData TypeName="TooFewFactTypeRoleInstancesError">
				<DomainClassMoniker Name="TooFewFactTypeRoleInstancesError"/>
			</XmlClassData>
			<XmlClassData TypeName="CompatibleValueTypeInstanceValueError">
				<DomainClassMoniker Name="CompatibleValueTypeInstanceValueError"/>
			</XmlClassData>
		</ClassData>
	</XmlSerializationBehavior>

	<ExplorerBehavior Name="ORMMetaModelExplorer"/>

	<ConnectionBuilders/>

	<!-- Diagram is double-derived so that we can override ShouldAddShapeForElement and OnChildConfiguring. -->
	<!-- Diagram has custom constructor so that we can turn off snap-to-grid and set the initial name. -->
	<Diagram Name="ORMDiagram" DisplayName="ORMDiagram" Namespace="Neumont.Tools.ORM.ShapeModel" Id="948F992D-C9B8-46F9-BE3C-B48347F8AB0B" GeneratesDoubleDerived="true" HasCustomConstructor="true">
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
		<UnmappedShapesUsed>
			<GeometryShapeMoniker Name="ORMBaseShape"/>
			<GeometryShapeMoniker Name="FloatingTextShape"/>
			<GeometryShapeMoniker Name="LinkConnectorShape"/>
		</UnmappedShapesUsed>
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
		</ConnectorMaps>
	</Diagram>

	<Designer EditorGuid="EDA9E282-8FC6-4AE4-AF2C-C224FD3AE49B">
		<RootClass>
			<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ORMModel"/>
		</RootClass>
		<XmlSerializationDefinition>
			<XmlSerializationBehaviorMoniker Name="/Neumont.Tools.ORM.ObjectModel/ORMMetaModelSerializationBehavior"/>
		</XmlSerializationDefinition>
		<ToolboxTab TabText="ORM Designer">
			<ElementTool Name="EntityType" Order="0" ToolboxIcon="Resources/Toolbox.EntityType.Bitmap.Id.bmp" Caption="Entity Type" Tooltip="New Entity Type">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ObjectType"/>
			</ElementTool>
			<ElementTool Name="ValueType" Order="5" ToolboxIcon="Resources/Toolbox.ValueType.Bitmap.Id.bmp" Caption="Value Type" Tooltip="New Value Type">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ObjectType"/>
			</ElementTool>
			<ElementTool Name="ObjectifiedFactType" Order="7" ToolboxIcon="Resources/Toolbox.ObjectifiedFactType.Bitmap.Id.bmp" Caption="Objectified Fact Type" Tooltip="New Objectified Fact Type">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ObjectType"/>
			</ElementTool>
			<ElementTool Name="UnaryFactType" Order="10" ToolboxIcon="Resources/Toolbox.UnaryFactType.Bitmap.Id.bmp" Caption="Unary Fact Type" Tooltip="New Unary Fact Type">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/FactType"/>
			</ElementTool>
			<ElementTool Name="BinaryFactType" Order="15" ToolboxIcon="Resources/Toolbox.BinaryFactType.Bitmap.Id.bmp" Caption="Binary Fact Type" Tooltip="New Binary Fact Type">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/FactType"/>
			</ElementTool>
			<ElementTool Name="TernaryFactType" Order="20" ToolboxIcon="Resources/Toolbox.TernaryFactType.Bitmap.Id.bmp" Caption="Ternary Fact Type" Tooltip="New Ternary Fact Type">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/FactType"/>
			</ElementTool>
			<ConnectionTool Name="RoleConnector" Order="21" ToolboxIcon="Resources/Toolbox.RoleConnector.Bitmap.Id.bmp" Caption="Role Connector" Tooltip="Role Connector Tool"/>
			<ConnectionTool Name="SubtypeConnector" Order="22" ToolboxIcon="Resources/Toolbox.SubtypeConnector.Bitmap.Id.bmp" Caption="Subtype Connector" Tooltip="Subtype Connector Tool"/>
			<ElementTool Name="InternalUniquenessConstraint" Order="23" ToolboxIcon="Resources/Toolbox.InternalUniquenessConstraint.Bitmap.Id.bmp" Caption="Internal Uniqueness Constraint" Tooltip="New Internal Uniqueness Constraint">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/UniquenessConstraint"/>
			</ElementTool>
			<ElementTool Name="ExternalUniquenessConstraint" Order="25" ToolboxIcon="Resources/Toolbox.ExternalUniquenessConstraint.Bitmap.Id.bmp" Caption="External Uniqueness Constraint" Tooltip="New External Uniqueness Constraint">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/UniquenessConstraint"/>
			</ElementTool>
			<ElementTool Name="EqualityConstraint" Order="35" ToolboxIcon="Resources/Toolbox.EqualityConstraint.Bitmap.Id.bmp" Caption="Equality Constraint" Tooltip="New Equality Constraint">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/EqualityConstraint"/>
			</ElementTool>
			<ElementTool Name="ExclusionConstraint" Order="40" ToolboxIcon="Resources/Toolbox.ExclusionConstraint.Bitmap.Id.bmp" Caption="Exclusion Constraint" Tooltip="New Exclusion Constraint">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/ExclusionConstraint"/>
			</ElementTool>
			<ElementTool Name="InclusiveOrConstraint" Order="45" ToolboxIcon="Resources/Toolbox.InclusiveOrConstraint.Bitmap.Id.bmp" Caption="Inclusive Or Constraint" Tooltip="New Inclusive Or Constraint">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/MandatoryConstraint"/>
			</ElementTool>
			<!-- TODO: <ElementTool Name="ExclusiveOrConstraint" Order="50" ToolboxIcon="Resources/Toolbox.ExclusiveOrConstraint.Bitmap.Id.bmp" Caption="Exclusive Or Constraint" Tooltip="New Exclusive Or Constraint"/>-->
			<ElementTool Name="SubsetConstraint" Order="55" ToolboxIcon="Resources/Toolbox.SubsetConstraint.Bitmap.Id.bmp" Caption="Subset Constraint" Tooltip="New Subset Constraint">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/SubsetConstraint"/>
			</ElementTool>
			<ElementTool Name="FrequencyConstraint" Order="57" ToolboxIcon="Resources/Toolbox.FrequencyConstraint.Bitmap.Id.bmp" Caption="Frequency Constraint" Tooltip="New Frequency Constraint">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/FrequencyConstraint"/>
			</ElementTool>
			<ElementTool Name="RingConstraint" Order="58" ToolboxIcon="Resources/Toolbox.RingConstraint.Bitmap.Id.bmp" Caption="Ring Constraint" Tooltip="New Ring Constraint">
				<DomainClassMoniker Name="/Neumont.Tools.ORM.ObjectModel/RingConstraint"/>
			</ElementTool>
			<ConnectionTool Name="ExternalConstraintConnector" Order="60" ToolboxIcon="Resources/Toolbox.ExternalConstraintConnector.Bitmap.Id.bmp" Caption="Constraint Connector" Tooltip="Constraint Connector Tool"/>
		</ToolboxTab>
		<DiagramMoniker Name="/Neumont.Tools.ORM.ShapeModel/ORMDiagram"/>
	</Designer>

</Dsl>
