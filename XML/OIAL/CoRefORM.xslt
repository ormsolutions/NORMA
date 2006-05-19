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
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:orm="http://schemas.neumont.edu/ORM/2006-04/ORMCore"
	xmlns:ormRoot="http://schemas.neumont.edu/ORM/2006-04/ORMRoot"
	xmlns:exsl="http://exslt.org/common"
	xmlns:loc="urn:local-temps"
	xmlns:xs="http://www.w3.org/2001/XMLSchema"
	extension-element-prefixes="exsl"
	exclude-result-prefixes="loc xs">

	<xsl:param name="CoRefOppositeRoleIdDecorator" select="'_opposite'"/>
	<xsl:param name="CoRefInternalUniquenessIdDecorator" select="'_unique'"/>
	<xsl:param name="CoRefInternalUniquenessNameDecorator" select="'_unique'"/>
	<xsl:param name="CoRefSimpleMandatoryIdDecorator" select="'_mandatory'"/>
	<xsl:param name="CoRefSimpleMandatoryNameDecorator" select="'_mandatory'"/>
	<xsl:param name="CoRefFactIdDecorator" select="'_coref_fact'"/>
	<xsl:param name="CoRefFactNameDecorator" select="'_coref_fact'"/>
	<xsl:param name="CoRefValueDataTypeIdDecorator" select="'_Data_Type'"/>
	<xsl:param name="CoRefRoleNamePreposition" select="'As'"/>
	<xsl:variable name="ExistingTrueOrFalseLogicalDataType" select="(ormRoot:ORM2/orm:ORMModel | orm:ORMModel)/orm:DataTypes/orm:TrueOrFalseLogicalDataType"/>
	<xsl:variable name="CoRefLogicalDataTypeIdDecoratorFragment">
		<xsl:choose>
			<xsl:when test="$ExistingTrueOrFalseLogicalDataType">
				<xsl:value-of select="orm:TrueOrFalseLogicalDataType/@id"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'_true_or_false_logical_Data_Type'"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="CoRefLogicalDataTypeIdDecorator" select="string($CoRefLogicalDataTypeIdDecoratorFragment)"/>

	<xsl:output method="xml" encoding="utf-8" media-type="application/orm+xml" indent="yes"/>
	<xsl:strip-space elements="*"/>

	<xs:schema targetNamespace="urn:local-temps" xmlns="urn:local-temps" elementFormDefault="qualified">
		<xs:element name="mappedRole" type="mappedRoleType"/>
		<xs:complexType name="mappedRoleType">
			<xs:annotation>
				<xs:documentation>
					Indicates that a role has been deleted and any reference to it
					needs to map to another role.
				</xs:documentation>
			</xs:annotation>
			<xs:attribute name="fromRoleRef" type="xs:NMTOKEN"/>
			<xs:attribute name="toRoleRef" type="xs:NMTOKEN"/>
		</xs:complexType>
	</xs:schema>

	<xsl:template match="orm:PlayedRoles" mode="ForRoles">
		<xsl:copy-of select="."/>
	</xsl:template>

	<xsl:template name="BuildRoleMap">
		<xsl:param name="RoleProxies"/>
		<xsl:for-each select="$RoleProxies">
			<loc:mappedRole fromRoleRef="{@id}" toRoleRef="{orm:Role/@ref}"/>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="ormRoot:ORM2 | orm:ORMModel" priority="-3">
		<xsl:apply-templates select="child::orm:ORMModel | self::orm:ORMModel" mode="CoRefORMModel"/>
	</xsl:template>

	<xsl:template match="orm:ORMModel" mode="CoRefORMModel">
		<xsl:param name="Model" select="."/>
		<xsl:variable name="ObjectifiedTypes" select="$Model/orm:Objects/orm:ObjectifiedType"/>
		<xsl:variable name="ImpliedFacts" select="$Model/orm:Facts/orm:ImpliedFact[orm:ImpliedByObjectification]" />
		<xsl:variable name="UnmodifiedObjectifiedFacts" select="$Model/orm:Facts/orm:Fact[@id=$ObjectifiedTypes/orm:NestedPredicate/@ref]" />
		<xsl:variable name="RoleMapFragment">
			<xsl:call-template name="BuildRoleMap">
				<xsl:with-param name="RoleProxies" select="$ImpliedFacts/child::orm:FactRoles/child::orm:RoleProxy"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="RoleMap" select="exsl:node-set($RoleMapFragment)/child::*"/>
		<xsl:variable name="ObjectifiedFactsFragment">
			<xsl:for-each select="$UnmodifiedObjectifiedFacts">
				<xsl:variable name="CurrentImpliedFact" select="."/>
				<xsl:copy>
					<xsl:for-each select="$ObjectifiedTypes[orm:NestedPredicate/@ref=current()/@id]">
						<xsl:attribute name="ObjectificationId">
							<xsl:value-of select="orm:NestedPredicate/@id"/>
						</xsl:attribute>
						<xsl:attribute name="ObjectifiedTypeId">
							<xsl:value-of select="@id"/>
						</xsl:attribute>
						<xsl:attribute name="PreferredIdentifier">
							<xsl:value-of select="$CurrentImpliedFact/orm:InternalConstraints/orm:UniquenessConstraint[1]/@ref"/>
						</xsl:attribute>
					</xsl:for-each>
					<xsl:copy-of select="@*|*"/>
				</xsl:copy>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="ObjectifiedFacts" select="exsl:node-set($ObjectifiedFactsFragment)/child::*"/>
		<xsl:variable name="BinarizableFacts" select="$Model/orm:Facts/orm:Fact[not(@id=$ObjectifiedFacts/@id) and count(orm:FactRoles/orm:Role)=1]"/>

		<xsl:apply-templates select="$Model" mode="CoRefORM">
			<xsl:with-param name="Model" select="$Model"/>
			<xsl:with-param name="RoleMap" select="$RoleMap"/>
			<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
			<xsl:with-param name="BinarizableFacts" select="$BinarizableFacts"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="orm:ORMModel" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="BinarizableFacts"/>
		<xsl:copy>
			<xsl:apply-templates select="node()|@*" mode="CoRefORM">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="RoleMap" select="$RoleMap"/>
				<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
				<xsl:with-param name="BinarizableFacts" select="$BinarizableFacts"/>
			</xsl:apply-templates>
			<xsl:if test="not(orm:Constraints) and $BinarizableFacts">
				<orm:Constraints>
					<xsl:for-each select="$BinarizableFacts/orm:InternalConstraints/orm:UniquenessConstraint">
						<xsl:variable name="BinarizedUniquenessConstraint" select="$Model/orm:Constraints/orm:UniquenessConstraint[@id=current()/@ref]"/>
						<orm:UniquenessConstraint>
							<xsl:copy-of select="$BinarizedUniquenessConstraint/@id"/>
							<xsl:copy-of select="$BinarizedUniquenessConstraint/@Name"/>
							<xsl:copy-of select="$BinarizedUniquenessConstraint/@IsInternal"/>
							<xsl:copy-of select="$BinarizedUniquenessConstraint/orm:RoleSequence"/>
						</orm:UniquenessConstraint>
					</xsl:for-each>
				</orm:Constraints>
			</xsl:if>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="orm:ImpliedFact" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="BinarizableFacts"/>
		<orm:Fact>
			<xsl:apply-templates select="node()|@*" mode="CoRefORM">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="RoleMap" select="$RoleMap"/>
				<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
				<xsl:with-param name="BinarizableFacts" select="$BinarizableFacts"/>
			</xsl:apply-templates>
		</orm:Fact>
	</xsl:template>
	<xsl:template match="orm:ImpliedByObjectification | orm:NestedPredicate" mode="CoRefORM"/>
	<xsl:template match="orm:ObjectifiedType" mode="CoRefORM">
		<xsl:param name="Model" />
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts" />
		<xsl:param name="BinarizableFacts"/>
		<xsl:variable name="object" select="$ObjectifiedFacts[@ObjectifiedTypeId=current()/@id]"/>
		<orm:EntityType _ReferenceMode="?">
			<xsl:apply-templates select="node()|@*" mode="CoRefORM">
				<xsl:with-param name="Model" select="$Model" />
				<xsl:with-param name="RoleMap" select="$RoleMap"/>
				<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
				<xsl:with-param name="BinarizableFacts" select="$BinarizableFacts"/>
			</xsl:apply-templates>
			<orm:PreferredIdentifier>
				<xsl:attribute name="ref">
					<xsl:value-of select="$object/@PreferredIdentifier"/>
				</xsl:attribute>
			</orm:PreferredIdentifier>
		</orm:EntityType>
	</xsl:template>
	<xsl:template match="*|@*|text()" name="DefaultCoRefORMTemplate" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="BinarizableFacts"/>
		<xsl:copy>
			<xsl:apply-templates select="node()|@*" mode="CoRefORM">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="RoleMap" select="$RoleMap"/>
				<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
				<xsl:with-param name="BinarizableFacts" select="$BinarizableFacts"/>
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="orm:PlayedRoles/orm:Role" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="BinarizableFacts"/>
		<xsl:if test="not($RoleMap[@fromRoleRef=current()/@ref])">
			<xsl:call-template name="DefaultCoRefORMTemplate">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="RoleMap" select="$RoleMap"/>
				<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
				<xsl:with-param name="BinarizableFacts" select="$BinarizableFacts"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<xsl:template match="orm:RoleProxy" mode="CoRefORM">
		<xsl:variable name="proxiedRole" select="../../../orm:Fact/child::orm:FactRoles/child::orm:Role[@id=current()/orm:Role/@ref]"/>
		<orm:Role id="{$proxiedRole/@id}" _IsMandatory="{$proxiedRole/@_IsMandatory}" _Multiplicity="{$proxiedRole/@_Multiplicity}" Name="{$proxiedRole/@Name}">
			<orm:RolePlayer ref="{$proxiedRole/orm:RolePlayer/@ref}" />
		</orm:Role>
	</xsl:template>
	<xsl:template match="orm:Role/@ref | orm:ImpliedFact/orm:FactRoles/orm:Role/@id" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="BinarizableFacts"/>
		<xsl:variable name="map" select="$RoleMap[@fromRoleRef=current()]"/>
		<xsl:choose>
			<xsl:when test="$map">
				<xsl:attribute name="{local-name()}">
					<xsl:value-of select="$map/@toRoleRef"/>
				</xsl:attribute>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="."/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="orm:Facts/orm:Fact" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="BinarizableFacts"/>
		<xsl:variable name="factId" select="@id"/>
		<xsl:choose>
			<xsl:when test="$ObjectifiedFacts[@id=$factId]"/>
			<xsl:when test="$BinarizableFacts[@id=$factId]">
				<xsl:variable name="fact" select="."/>
				<xsl:for-each select="$fact/orm:FactRoles/orm:Role">
					<orm:Fact id="{$factId}{$CoRefFactIdDecorator}{position()}">
						<orm:FactRoles>
							<xsl:variable name="EntityType" select="$Model/orm:Objects/orm:EntityType[orm:PlayedRoles/orm:Role/@ref=current()/@id]/@Name"/>
							<xsl:variable name="ValueType" select="$Model/orm:Objects/orm:ValueType[orm:PlayedRoles/orm:Role/@ref=current()/@id]/@Name"/>
							<xsl:variable name="RoleName" select="@Name"/>
							<xsl:variable name="RoleNameEndDecorator">
								<xsl:choose>
									<xsl:when test="$RoleName and $RoleName !=''">
										<xsl:value-of select="$RoleName"/>
									</xsl:when>
									<xsl:when test="$EntityType">
										<xsl:value-of select="$EntityType"/>
									</xsl:when>
									<xsl:when test="$ValueType">
										<xsl:value-of select="$ValueType"/>
									</xsl:when>
								</xsl:choose>
							</xsl:variable>
							<!-- UNDONE: Consider getting multiplicity right for both these roles -->
							<xsl:choose>
								<xsl:when test="count(../orm:Role) > 1">
									<orm:Role id="{@id}{$CoRefOppositeRoleIdDecorator}" Name="{../../@Name}{$CoRefRoleNamePreposition}{$RoleNameEndDecorator}" _IsMandatory="true">
										<orm:RolePlayer ref="{$factId}"/>
									</orm:Role>
								</xsl:when>
								<xsl:otherwise>
									<!--Grab the Role Name-->
									<orm:Role id="{@id}{$CoRefOppositeRoleIdDecorator}" Name="{@Name}" _IsMandatory="true">
										<orm:RolePlayer ref="{$factId}"/>
									</orm:Role>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:copy>
								<xsl:copy-of select="@*[local-name()!='_Multiplicity']"/>
								<xsl:copy-of select="orm:RolePlayer"/>
							</xsl:copy>
						</orm:FactRoles>
						<xsl:copy-of select="$fact/orm:InternalConstraints"/>
					</orm:Fact>
				</xsl:for-each>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="DefaultCoRefORMTemplate">
					<xsl:with-param name="Model" select="$Model"/>
					<xsl:with-param name="RoleMap" select="$RoleMap"/>
					<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
					<xsl:with-param name="BinarizableFacts" select="$BinarizableFacts"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="orm:Constraints" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="BinarizableFacts"/>
		<xsl:copy>
			<xsl:apply-templates select="node()|@*" mode="CoRefORM">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="RoleMap" select="$RoleMap"/>
				<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
				<xsl:with-param name="BinarizableFacts" select="$BinarizableFacts"/>
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="orm:UniquenessConstraint[count(orm:RoleSequence/orm:Role)&gt;1 and @IsInternal='true']" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="BinarizableFacts"/>
		<xsl:variable name="InternalUniquenessConstraintsOnObjectifiedFacts" select="$ObjectifiedFacts/child::orm:InternalConstraints/orm:UniquenessConstraint/@ref"/>
		<xsl:choose>
			<xsl:when test="current()/@id=$InternalUniquenessConstraintsOnObjectifiedFacts">
				<orm:UniquenessConstraint id="{@id}" Name="{@Name}">
					<xsl:copy-of select="current()/child::*"/>
				</orm:UniquenessConstraint>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="."/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="orm:Objects" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="BinarizableFacts"/>
		<xsl:copy>
			<xsl:apply-templates select="node()|@*" mode="CoRefORM">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="RoleMap" select="$RoleMap"/>
				<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
				<xsl:with-param name="BinarizableFacts" select="$BinarizableFacts"/>
			</xsl:apply-templates>
			<xsl:for-each select="$BinarizableFacts">
				<xsl:choose>
					<xsl:when test="orm:InternalConstraints/orm:UniquenessConstraint/orm:RoleSequence[count(orm:Role)>1]">
						<orm:EntityType Name="{@Name}" id="{@id}" IsIndependent="true">
							<orm:PlayedRoles>
								<xsl:for-each select="orm:FactRoles/orm:Role">
									<orm:Role ref="{@id}{$CoRefOppositeRoleIdDecorator}"/>
								</xsl:for-each>
							</orm:PlayedRoles>
							<orm:PreferredIdentifier ref="{orm:InternalConstraints/orm:UniquenessConstraint[not(@Modality) or @Modality='Alethic'][1]/@id}"/>
						</orm:EntityType>
					</xsl:when>
					<xsl:otherwise>
						<orm:ValueType Name="{@Name}" id="{@id}" IsExternal="false" IsIndependent="false">
							<orm:PlayedRoles>
								<xsl:for-each select="orm:FactRoles/orm:Role">
									<orm:Role ref="{@id}{$CoRefOppositeRoleIdDecorator}"/>
								</xsl:for-each>
							</orm:PlayedRoles>
							<xsl:choose>
								<xsl:when test="$ExistingTrueOrFalseLogicalDataType">
									<orm:ConceptualDataType id="{@id}{$CoRefValueDataTypeIdDecorator}" ref="{$CoRefLogicalDataTypeIdDecorator}" Scale="0" Length="0" />
								</xsl:when>
								<xsl:otherwise>
									<!--need to add logical data type to the model-->
									<xsl:apply-templates select="orm:DataTypes" />
									<orm:ConceptualDataType id="{@id}{$CoRefValueDataTypeIdDecorator}" ref="{$CoRefLogicalDataTypeIdDecorator}" Scale="0" Length="0" />
								</xsl:otherwise>
							</xsl:choose>
							<!--UNDONE:  To support relative closure do not include the value constraint-->
							<orm:ValueRestriction>
								<orm:ValueConstraint Name="{@id}_Constraint_Name" id="{@id}_Value_Range_Definition">
									<orm:ValueRanges>
										<orm:ValueRange id="{@id}_True_Value" MinValue="true" MaxValue="true" MinInclusion="NotSet" MaxInclusion="NotSet" />
									</orm:ValueRanges>
								</orm:ValueConstraint>
							</orm:ValueRestriction>
						</orm:ValueType>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="orm:DataTypes" mode="CoRefORM">
		<xsl:copy>
			<xsl:if test="not($ExistingTrueOrFalseLogicalDataType)">
				<orm:TrueOrFalseLogicalDataType id="{$CoRefLogicalDataTypeIdDecorator}" />
			</xsl:if>
			<xsl:copy-of select="@*"/>
			<xsl:copy-of select="child::*"/>
		</xsl:copy>
	</xsl:template>
</xsl:stylesheet>
