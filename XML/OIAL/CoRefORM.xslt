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
				<xsl:copy>
					<xsl:for-each select="$ObjectifiedTypes[orm:NestedPredicate/@ref=current()/@id]">
						<xsl:attribute name="ObjectificationId">
							<xsl:value-of select="orm:NestedPredicate/@id"/>
						</xsl:attribute>
						<xsl:attribute name="ObjectifiedTypeId">
							<xsl:value-of select="@id"/>
						</xsl:attribute>
					</xsl:for-each>
					<xsl:copy-of select="@*|*"/>
				</xsl:copy>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="ObjectifiedFacts" select="exsl:node-set($ObjectifiedFactsFragment)/child::*"/>
		<xsl:variable name="UnobjectifiedUnaryFacts" select="$Model/orm:Facts/orm:Fact[not(@id=$ObjectifiedFacts/@id) and count(orm:FactRoles/orm:Role)=1]"/>

		<xsl:apply-templates select="$Model" mode="CoRefORM">
			<xsl:with-param name="Model" select="$Model"/>
			<xsl:with-param name="RoleMap" select="$RoleMap"/>
			<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
			<xsl:with-param name="UnobjectifiedUnaryFacts" select="$UnobjectifiedUnaryFacts"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="orm:ORMModel" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="UnobjectifiedUnaryFacts"/>
		<xsl:copy>
			<xsl:apply-templates select="node()|@*" mode="CoRefORM">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="RoleMap" select="$RoleMap"/>
				<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
				<xsl:with-param name="UnobjectifiedUnaryFacts" select="$UnobjectifiedUnaryFacts"/>
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="orm:ImpliedFact" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="UnobjectifiedUnaryFacts"/>
		<orm:Fact>
			<xsl:apply-templates select="node()|@*" mode="CoRefORM">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="RoleMap" select="$RoleMap"/>
				<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
				<xsl:with-param name="UnobjectifiedUnaryFacts" select="$UnobjectifiedUnaryFacts"/>
			</xsl:apply-templates>
		</orm:Fact>
	</xsl:template>
	<xsl:template match="orm:ImpliedByObjectification | orm:NestedPredicate" mode="CoRefORM"/>
	<xsl:template match="orm:ObjectifiedType" mode="CoRefORM">
		<xsl:param name="Model" />
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts" />
		<xsl:param name="UnobjectifiedUnaryFacts"/>
		<xsl:variable name="object" select="$ObjectifiedFacts[@ObjectifiedTypeId=current()/@id]"/>
		<orm:EntityType>
			<xsl:apply-templates select="node()|@*" mode="CoRefORM">
				<xsl:with-param name="Model" select="$Model" />
				<xsl:with-param name="RoleMap" select="$RoleMap"/>
				<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
				<xsl:with-param name="UnobjectifiedUnaryFacts" select="$UnobjectifiedUnaryFacts"/>
			</xsl:apply-templates>
		</orm:EntityType>
	</xsl:template>
	<xsl:template match="*|@*|text()" name="DefaultCoRefORMTemplate" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="UnobjectifiedUnaryFacts"/>
		<xsl:copy>
			<xsl:apply-templates select="node()|@*" mode="CoRefORM">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="RoleMap" select="$RoleMap"/>
				<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
				<xsl:with-param name="UnobjectifiedUnaryFacts" select="$UnobjectifiedUnaryFacts"/>
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="orm:PlayedRoles/orm:Role" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="UnobjectifiedUnaryFacts"/>
		<xsl:if test="not($RoleMap[@fromRoleRef=current()/@ref])">
			<xsl:call-template name="DefaultCoRefORMTemplate">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="RoleMap" select="$RoleMap"/>
				<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
				<xsl:with-param name="UnobjectifiedUnaryFacts" select="$UnobjectifiedUnaryFacts"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<xsl:template match="orm:RoleProxy" mode="CoRefORM">
		<xsl:variable name="proxiedRole" select="../../../orm:Fact/child::orm:FactRoles/child::orm:Role[@id=current()/orm:Role/@ref]"/>
		<orm:Role _Multiplicity="ExactlyOne">
			<xsl:copy-of select="$proxiedRole/@*[not(local-name()='_Multiplicity')]|$proxiedRole/child::*"/>
		</orm:Role>
	</xsl:template>
	<xsl:template match="orm:Role/@ref | orm:ImpliedFact/orm:FactRoles/orm:Role/@id" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="UnobjectifiedUnaryFacts"/>
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
	<xsl:template name="GetUnaryFactRoleName">
		<xsl:param name="Model"/>
		<xsl:param name="UnaryFact"/>
		<xsl:choose>
			<xsl:when test="string-length($UnaryFact/orm:FactRoles/child::*/@Name)">
				<xsl:value-of select="$UnaryFact/orm:FactRoles/child::*/@Name"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="reading" select="translate(normalize-space($UnaryFact/orm:ReadingOrders/orm:ReadingOrder[1]/orm:Readings/orm:Reading[1]/orm:Data/text()),' ','')"/>
				<xsl:value-of select="concat(substring-before($reading,'{0}'),substring-after($reading,'{0}'))"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="orm:Facts/orm:Fact" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="UnobjectifiedUnaryFacts"/>
		<xsl:variable name="factId" select="@id"/>
		<xsl:choose>
			<xsl:when test="$ObjectifiedFacts[@id=$factId]"/>
			<xsl:when test="$UnobjectifiedUnaryFacts[@id=$factId]">
				<xsl:variable name="fact" select="."/>
				<xsl:for-each select="$fact/orm:FactRoles/orm:Role">
					<orm:Fact id="{$factId}{$CoRefFactIdDecorator}{position()}">
						<orm:FactRoles>
							<xsl:copy>
								<xsl:copy-of select="@*[local-name()!='_Multiplicity']"/>
								<xsl:copy-of select="orm:RolePlayer"/>
							</xsl:copy>
							<!-- UNDONE: Consider getting multiplicity right for both these roles -->
							<orm:Role id="{@id}{$CoRefOppositeRoleIdDecorator}" _IsMandatory="true">
								<xsl:attribute name="Name">
									<xsl:call-template name="GetUnaryFactRoleName">
										<xsl:with-param name="Model" select="$Model"/>
										<xsl:with-param name="UnaryFact" select="$fact"/>
									</xsl:call-template>
								</xsl:attribute>
								<orm:RolePlayer ref="{$factId}"/>
							</orm:Role>
						</orm:FactRoles>
						<orm:InternalConstraints>
							<orm:UniquenessConstraint ref="{@id}{$CoRefInternalUniquenessIdDecorator}"/>
						</orm:InternalConstraints>
					</orm:Fact>
				</xsl:for-each>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="DefaultCoRefORMTemplate">
					<xsl:with-param name="Model" select="$Model"/>
					<xsl:with-param name="RoleMap" select="$RoleMap"/>
					<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
					<xsl:with-param name="UnobjectifiedUnaryFacts" select="$UnobjectifiedUnaryFacts"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="orm:Constraints" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="UnobjectifiedUnaryFacts"/>
		<xsl:copy>
			<xsl:apply-templates select="node()|@*" mode="CoRefORM">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="RoleMap" select="$RoleMap"/>
				<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
				<xsl:with-param name="UnobjectifiedUnaryFacts" select="$UnobjectifiedUnaryFacts"/>
			</xsl:apply-templates>
			<xsl:for-each select="$UnobjectifiedUnaryFacts">
				<xsl:variable name="currentId" select="orm:FactRoles/orm:Role/@id"/>
				<orm:UniquenessConstraint id="{$currentId}{$CoRefInternalUniquenessIdDecorator}" Name="{$currentId}{$CoRefInternalUniquenessNameDecorator}" IsInternal="true">
					<orm:RoleSequence>
						<orm:Role ref="{$currentId}"/>
					</orm:RoleSequence>
				</orm:UniquenessConstraint>
				<orm:MandatoryConstraint id="{$currentId}{$CoRefSimpleMandatoryIdDecorator}" Name="{$currentId}{$CoRefSimpleMandatoryNameDecorator}" IsSimple="true">
					<orm:RoleSequence>
						<orm:Role ref="{$currentId}"/>
					</orm:RoleSequence>
				</orm:MandatoryConstraint>
			</xsl:for-each>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="orm:UniquenessConstraint[count(orm:RoleSequence/child::*)>1 and @IsInternal='true']" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="UnobjectifiedUnaryFacts"/>
		<!-- After binarization, no internal uniqueness constraint will have more than one role, so remove the @IsInternal attribute. -->
		<xsl:copy>
			<xsl:copy-of select="@*[not(local-name()='IsInternal')]"/>
			<xsl:copy-of select="child::*"/>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="orm:Objects" mode="CoRefORM">
		<xsl:param name="Model"/>
		<xsl:param name="RoleMap"/>
		<xsl:param name="ObjectifiedFacts"/>
		<xsl:param name="UnobjectifiedUnaryFacts"/>
		<xsl:copy>
			<xsl:apply-templates select="node()|@*" mode="CoRefORM">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="RoleMap" select="$RoleMap"/>
				<xsl:with-param name="ObjectifiedFacts" select="$ObjectifiedFacts"/>
				<xsl:with-param name="UnobjectifiedUnaryFacts" select="$UnobjectifiedUnaryFacts"/>
			</xsl:apply-templates>
			<xsl:for-each select="$UnobjectifiedUnaryFacts">
				<orm:ValueType id="{@id}">
					<xsl:attribute name="Name">
						<xsl:value-of select="$Model/orm:Objects/child::*[@id=current()/orm:FactRoles/orm:Role/orm:RolePlayer/@ref]/@Name"/>
						<xsl:text>_</xsl:text>
						<xsl:call-template name="GetUnaryFactRoleName">
							<xsl:with-param name="Model" select="$Model"/>
							<xsl:with-param name="UnaryFact" select="."/>
						</xsl:call-template>
					</xsl:attribute>
					<orm:PlayedRoles>
						<orm:Role ref="{orm:FactRoles/orm:Role/@id}{$CoRefOppositeRoleIdDecorator}"/>
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
				</orm:ValueType>
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
	<xsl:template match="orm:ModelNotes" mode="CoRefORM">
		<!-- Kill these in the coref orm file. We're not using them anywhere, and we end
		up turning some facts into object types, so the ModelNote/ReferencedByFactType links
		may fail to load. -->
	</xsl:template>
</xsl:stylesheet>
