<?xml version="1.0" encoding="UTF-8" ?>
<!--
	Copyright © Neumont University. All rights reserved.

	This software is provided 'as-is', without any express or implied warranty. In no event will the authors be held liable for any damages arising from the use of this software.
	Permission is granted to anyone to use this software for any purpose, including commercial applications, and to alter it and redistribute it freely, subject to the following restrictions:
	1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
	2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
	3. This notice may not be removed or altered from any source distribution.
-->
<!-- Contributors: Rexford Morgan, Joshua Arnold -->
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:orm="http://schemas.neumont.edu/ORM/2006-04/ORMCore"
	xmlns:ormDiagram="http://schemas.neumont.edu/ORM/2006-04/ORMDiagram"
	xmlns:ormRoot="http://schemas.neumont.edu/ORM/2006-04/ORMRoot"
	xmlns:exsl="http://exslt.org/common"
	xmlns:odt="http://schemas.orm.net/ORMDataTypes"
	xmlns:oil="http://schemas.orm.net/OIAL"
	extension-element-prefixes="exsl"
	exclude-result-prefixes="odt oil">
	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:template match="oil:model">
		<orm:ORMModel id="{@sourceRef}" Name="{@name}">
			<orm:Objects>
				<xsl:apply-templates select="oil:conceptType" mode="GenerateObjectTypes"/>
			</orm:Objects>
			<orm:Constraints>
				<xsl:apply-templates mode="GenerateConstraints" select="oil:conceptType/child::oil:*"/>
			</orm:Constraints>
			<orm:Facts>
				<xsl:apply-templates mode="GenerateFacts" select="oil:conceptType/child::oil:*"/>
			</orm:Facts>
			<orm:DataTypes>
				<xsl:if test="oil:informationTypeFormats/child::odt:string">
					<orm:VariableLengthTextDataType id="VariableLengthTextDataType" />
				</xsl:if>
				<xsl:if test="boolean(oil:informationTypeFormats/child::odt:decimalNumber) or boolean(oil:informationTypeFormats/child::odt:floatingPointNumber) or boolean(oil:informationTypeFormats/child::odt:identity)">
					<orm:DecimalNumericDataType id="DecimalNumericDataType" />
				</xsl:if>
				<xsl:if test="oil:informationTypeFormats/child::odt:binary">
					<orm:LargeLengthRawDataDataType id="LargeLengthRawDataDataType" />
				</xsl:if>
				<xsl:if test="oil:informationTypeFormats/child::odt:boolean">
					<orm:TrueOrFalseLogicalDataType id="TrueOrFalseLogicalDataType"/>
				</xsl:if>
			</orm:DataTypes>
			<orm:ReferenceModeKinds>
				<orm:ReferenceModeKind id="_94F6CC9F-76A2-461B-A031-E13A5FA5B9C9" ReferenceModeType="General">
					<xsl:attribute name="FormatString">
						<xsl:text>{1}</xsl:text>
					</xsl:attribute>
				</orm:ReferenceModeKind>
				<orm:ReferenceModeKind id="_54981962-590F-428B-92C0-3430BC951E3F" ReferenceModeType="Popular">
					<xsl:attribute name="FormatString">
						<xsl:text>{0}_{1}</xsl:text>
					</xsl:attribute>
				</orm:ReferenceModeKind>
				<orm:ReferenceModeKind id="_401C5824-3C4A-4514-AE6D-0454546E52AC" ReferenceModeType="UnitBased">
					<xsl:attribute name="FormatString">
						<xsl:text>{1}Value</xsl:text>
					</xsl:attribute>
				</orm:ReferenceModeKind>
			</orm:ReferenceModeKinds>
		</orm:ORMModel>
	</xsl:template>
	<!-- Match conceptTypeRef or informationType and generate the appropriate FactType -->
	<xsl:template mode="GenerateFacts" match="*"/>
	<xsl:template mode="GenerateFacts" match="oil:conceptTypeRef | oil:informationType">
		<xsl:variable name="NameDecorator" select="concat(concat(../@name, '_has_'), @name)"/>
		<orm:Fact id="{$NameDecorator}" _Name="{$NameDecorator}">
			<orm:FactRoles>
				<orm:Role Name="{@name}" id="{$NameDecorator}_Role">
					<xsl:attribute name="_IsMandatory">
						<xsl:choose>
							<xsl:when test="@mandatory = 'alethic'">
								<xsl:text>true</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>false</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:choose>
						<xsl:when test="boolean(self::oil:conceptTypeRef)">
							<orm:RolePlayer ref="{@target}Entity"/>
						</xsl:when>
						<xsl:otherwise>
							<orm:RolePlayer ref="{../@sourceRef}_has_{@sourceRef}ValueType"/>
						</xsl:otherwise>
					</xsl:choose>
				</orm:Role>
				<orm:Role Name="{../@name}" id="{$NameDecorator}_Role2">
					<orm:RolePlayer ref="{../@sourceRef}Entity"/>
				</orm:Role>
			</orm:FactRoles>
			<orm:ReadingOrders>
				<orm:ReadingOrder id="{$NameDecorator}ReadingOrder1">
					<orm:Readings>
						<orm:Reading id="{$NameDecorator}Reading1">
							<orm:Data>{0} has {1}</orm:Data>
						</orm:Reading>
					</orm:Readings>
					<orm:RoleSequence>
						<orm:Role ref="{$NameDecorator}_Role2"/>
						<orm:Role ref="{$NameDecorator}_Role"/>
					</orm:RoleSequence>
				</orm:ReadingOrder>
				<orm:ReadingOrder id="{$NameDecorator}ReadingOrder2">
					<orm:Readings>
						<orm:Reading id="{$NameDecorator}Reading2">
							<orm:Data>{0} belongs to {1}</orm:Data>
						</orm:Reading>
					</orm:Readings>
					<orm:RoleSequence>
						<orm:Role ref="{$NameDecorator}_Role"/>
						<orm:Role ref="{$NameDecorator}_Role2"/>
					</orm:RoleSequence>
				</orm:ReadingOrder>
			</orm:ReadingOrders>
			<orm:InternalConstraints>
				<xsl:variable name="CurrentName" select="@name"/>
				<orm:UniquenessConstraint ref="{$NameDecorator}_IUC"/>
				<xsl:if test="@mandatory='alethic'">
					<orm:MandatoryConstraint ref="{$NameDecorator}_MandatoryConstraint" />
				</xsl:if>
				<xsl:if test="boolean(oil:singleRoleUniquenessConstraint)">
					<xsl:variable name="UniqueRoleName" select="concat(concat(concat(@name, '_is_of_'), ../@sourceRef), '_IUC')"/>
					<orm:UniquenessConstraint ref="{$UniqueRoleName}"/>
				</xsl:if>
			</orm:InternalConstraints>
		</orm:Fact>
	</xsl:template>
	<!-- Match each conceptType to generate EntityTypes and cascade down to each child informationType -->
	<xsl:template mode="GenerateObjectTypes" match="*"/>
	<xsl:template mode="GenerateObjectTypes" match="oil:conceptType">
		<orm:EntityType id="{@sourceRef}Entity" Name="{@name}" _ReferenceMode="" />
		<xsl:apply-templates select="oil:informationType" mode="GenerateObjectTypes"/>
	</xsl:template>
	<xsl:template mode="GenerateObjectTypes" match="oil:informationType">
		<xsl:variable name="CurrentName" select="concat(../@name, concat('_', @name))"/>
		<orm:ValueType id="{../@sourceRef}_has_{@sourceRef}ValueType">
			<xsl:attribute name="Name">
				<xsl:if test="count(//oil:informationType[@name = current()/@name]) > 1">
					<xsl:value-of select="../@name"/>
					<xsl:text>_</xsl:text>
				</xsl:if>
				<xsl:value-of select="@name"/>
			</xsl:attribute>
			<orm:ConceptualDataType id="ConceptualDataType{../@name}_has_{@name}">
				<xsl:choose>
					<xsl:when test="boolean(../../oil:informationTypeFormats/odt:boolean[@name = $CurrentName])">
						<xsl:attribute name="ref">
							<xsl:text>TrueOrFalseLogicalDataType</xsl:text>
						</xsl:attribute>
					</xsl:when>
					<xsl:when test="boolean(../../oil:informationTypeFormats/odt:string[@name = $CurrentName])">
						<xsl:attribute name="ref">
							<xsl:text>VariableLengthTextDataType</xsl:text>
						</xsl:attribute>
						<xsl:if test="boolean(@maxLength)">
							<xsl:attribute name="Length">
								<xsl:value-of select="@maxLength"/>
							</xsl:attribute>
						</xsl:if>
						<xsl:attribute name="Scale">
							<xsl:text>0</xsl:text>
						</xsl:attribute>
					</xsl:when>
					<xsl:when test="boolean(../../oil:informationTypeFormats/odt:decimalNumber[@name = $CurrentName]) or boolean(../../oil:informationTypeFormats/odt:floatingPointNumber[@name = $CurrentName])">
						<xsl:attribute name="ref">
							<xsl:text>DecimalNumericDataType</xsl:text>
						</xsl:attribute>
					</xsl:when>
					<xsl:when test="boolean(../../oil:informationTypeFormats/odt:binary[@name = $CurrentName])">
						<xsl:attribute name="ref">
							<xsl:text>LargeLengthRawDataDataType</xsl:text>
						</xsl:attribute>
					</xsl:when>
					<!-- It is assumed that the default is a unary -->
					<xsl:otherwise>
						<xsl:attribute name="ref">
							<xsl:text>TrueOrFalseLogicalDataType</xsl:text>
						</xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
			</orm:ConceptualDataType>
		</orm:ValueType>
	</xsl:template>
	<!-- Templates to generate Constraints -->
	<xsl:template mode="GenerateConstraints" match="*"/>
	<xsl:template mode="GenerateConstraints" match="oil:informationType">
		<xsl:choose>
			<xsl:when test="not(oil:singleRoleUniquenessConstraint)">
				<xsl:variable name="VariableDecorator" select="concat(concat(../@name, '_has_'), @name)"/>
				<orm:UniquenessConstraint id="{$VariableDecorator}_IUC" Name="{$VariableDecorator}_IUC" IsInternal="true">
					<orm:RoleSequence>
						<orm:Role id="{$VariableDecorator}_IUC_Role" ref="{$VariableDecorator}_Role2"/>
					</orm:RoleSequence>
				</orm:UniquenessConstraint>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="VariableDecorator" select="concat(concat(concat(../@name, '_has_'), @name), '_IUC')"/>
				<orm:UniquenessConstraint id="{$VariableDecorator}" Name="{$VariableDecorator}" IsInternal="true">
					<orm:RoleSequence>
						<orm:Role id="ucRole_{$VariableDecorator}" ref="{../@name}_has_{@sourceRef}_Role2"/>
					</orm:RoleSequence>
				</orm:UniquenessConstraint>
				<xsl:variable name="UniqueRoleName" select="concat(concat(concat(@name, '_is_of_'), ../@name), '_IUC')"/>
				<orm:UniquenessConstraint id="{$UniqueRoleName}" Name="{$UniqueRoleName}" IsInternal="true">
					<orm:RoleSequence>
						<orm:Role id="ucRole_{$UniqueRoleName}" ref="{../@name}_has_{@sourceRef}_Role"/>
					</orm:RoleSequence>
					<xsl:if test="oil:singleRoleUniquenessConstraint/@isPreferred = 'true'">
						<orm:PreferredIdentifierFor ref="{../@sourceRef}Entity"/>
					</xsl:if>
				</orm:UniquenessConstraint>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:if test="@mandatory = 'alethic'">
			<xsl:variable name="VariableDecorator" select="concat(concat(../@name, '_has_'), @name)"/>
			<orm:MandatoryConstraint id="{$VariableDecorator}_MandatoryConstraint" Name="{$VariableDecorator}_MandatoryConstraint" IsSimple="true">
				<orm:RoleSequence>
					<orm:Role id="{$VariableDecorator}_MandatoryRole" ref="{$VariableDecorator}_Role2" />
				</orm:RoleSequence>
			</orm:MandatoryConstraint>
		</xsl:if>
	</xsl:template>
	<xsl:template mode="GenerateConstraints" match="oil:conceptTypeRef">
		<xsl:choose>
			<xsl:when test="not(oil:singleRoleUniquenessConstraint)">
				<xsl:variable name="VariableDecorator" select="concat(concat(../@name, '_has_'), @name)"/>
				<orm:UniquenessConstraint id="{$VariableDecorator}_IUC" Name="{$VariableDecorator}_IUC" IsInternal="true">
					<orm:RoleSequence>
						<orm:Role id="{$VariableDecorator}_IUC_Role" ref="{$VariableDecorator}_Role2"/>
					</orm:RoleSequence>
				</orm:UniquenessConstraint>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="VariableDecorator" select="concat(concat(../@name, '_has_'), @name)"/>
				<orm:UniquenessConstraint id="{$VariableDecorator}_IUC" Name="{$VariableDecorator}" IsInternal="true">
					<orm:RoleSequence>
						<orm:Role id="ucRole_{$VariableDecorator}" ref="{../@sourceRef}_has_{@sourceRoleRef}_Role2"/>
					</orm:RoleSequence>
				</orm:UniquenessConstraint>
				<xsl:variable name="UniqueRoleName" select="concat(concat(concat(@name, '_is_of_'), ../@sourceRef), '_IUC')"/>
				<orm:UniquenessConstraint id="{$UniqueRoleName}" Name="{$UniqueRoleName}" IsInternal="true">
					<orm:RoleSequence>
						<orm:Role id="ucRole_{$UniqueRoleName}" ref="{../@name}_has_{@name}_Role"/>
					</orm:RoleSequence>
					<xsl:if test="oil:singleRoleUniquenessConstraint/@isPreferred = 'true'">
						<orm:PreferredIdentifierFor ref="{../@sourceRef}Entity"/>
					</xsl:if>
				</orm:UniquenessConstraint>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:if test="@mandatory = 'alethic'">
			<xsl:variable name="VariableDecorator" select="concat(concat(../@name, '_has_'), @name)"/>
			<orm:MandatoryConstraint id="{$VariableDecorator}_MandatoryConstraint" Name="{$VariableDecorator}_MandatoryConstraint" IsSimple="true">
				<orm:RoleSequence>
					<orm:Role id="{$VariableDecorator}_MandatoryRole" ref="{$VariableDecorator}_Role2" />
				</orm:RoleSequence>
			</orm:MandatoryConstraint>
		</xsl:if>
	</xsl:template>
	<xsl:template mode="GenerateConstraints" match="oil:roleSequenceUniquenessConstraint">
		<orm:UniquenessConstraint id="{@name}" Name="{@name}">
			<orm:RoleSequence>
				<xsl:for-each select="oil:roleSequence/oil:typeRef">
					<orm:Role id="euc_{../../../@name}Role{position()}" ref="{@targetConceptType}_has_{@targetChild}_Role" />
				</xsl:for-each>
			</orm:RoleSequence>
			<xsl:if test="@isPreferred = 'true'">
				<orm:PreferredIdentifierFor ref="{../@name}Entity"/>
			</xsl:if>
		</orm:UniquenessConstraint>
	</xsl:template>
</xsl:stylesheet>