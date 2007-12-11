<?xml version="1.0" encoding="utf-8"?>
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
	xmlns:exsl="http://exslt.org/common"
	xmlns:odt="http://schemas.neumont.edu/ORM/Abstraction/2007-06/DataTypes/Core"
	xmlns:oil="http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core"
	xmlns:dcl="http://schemas.orm.net/DIL/DCIL"
	extension-element-prefixes="exsl"
	exclude-result-prefixes="dcl">
	<xsl:param name="FindAssociations" select="true()"/>
	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:template match="/">
		<xsl:choose>
			<xsl:when test="$FindAssociations">
				<xsl:variable name="BinaryOIALFragment">
					<xsl:apply-templates select="child::*" mode="BinaryOIAL"/>
				</xsl:variable>
				<!--<xsl:copy-of select="exsl:node-set($BinaryOIALFragment)/child::*"/>-->
				<xsl:apply-templates select="exsl:node-set($BinaryOIALFragment)/child::*" mode="AddAssociations"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="child::*" mode="BinaryOIAL"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="@*|*|text()|comment()" mode="AddAssociations">
		<xsl:copy>
			<xsl:apply-templates select="@*|*|text()|comment()" mode="AddAssociations"/>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="oil:model/oil:conceptTypes/oil:conceptType[count(oil:uniquenessConstraints/oil:uniquenessConstraint[@isPreferred]/oil:uniquenessChild) = count(oil:children/oil:*)]" mode="AddAssociations">
		<xsl:copy>
			<xsl:apply-templates select="@*|*|text()|comment()" mode="AddAssociations"/>
			<oil:association>
				<xsl:for-each select="oil:uniquenessConstraints/oil:uniquenessConstraint[@isPreferred]/oil:uniquenessChild">
				<oil:associationChild>
					<xsl:copy-of select="@ref"/>
				</oil:associationChild>
				</xsl:for-each>
			</oil:association>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="dcl:schema" mode="BinaryOIAL">
		<xsl:variable name="allTables" select="dcl:table"/>
		<oil:model id="{@name}" name="{@name}">
			<oil:informationTypeFormats>
				<xsl:for-each select="$allTables">
					<xsl:variable name="resolvedReferenceConstraintColumnNames" select="dcl:referenceConstraint[@targetTable=$allTables/@name]/dcl:columnRef/@sourceName"/>
					<xsl:for-each select="dcl:column">
						<xsl:if test="not(@name=$resolvedReferenceConstraintColumnNames)">
							<xsl:choose>
								<xsl:when test="dcl:predefinedDataType/@precision">
									<xsl:call-template name="CreateInformationFormat">
										<xsl:with-param name="tableName" select="../@name"/>
										<xsl:with-param name="columnName" select="@name"/>
										<xsl:with-param name="dataType" select="dcl:predefinedDataType/@name"/>
										<xsl:with-param name="maxLength" select="dcl:predefinedDataType/@precision"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="CreateInformationFormat">
										<xsl:with-param name="tableName" select="../@name"/>
										<xsl:with-param name="columnName" select="@name"/>
										<xsl:with-param name="dataType" select="dcl:predefinedDataType/@name"/>
										<xsl:with-param name="maxLength" select="dcl:predefinedDataType/@length"/>
									</xsl:call-template>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:if>
					</xsl:for-each>
				</xsl:for-each>
				<xsl:for-each select="dcl:domain">
					<xsl:choose>
						<xsl:when test="dcl:predefinedDataType/@precision">
							<xsl:call-template name="CreateInformationFormat">
								<xsl:with-param name="tableName" select="../@name"/>
								<xsl:with-param name="columnName" select="@name"/>
								<xsl:with-param name="dataType" select="dcl:predefinedDataType/@name"/>
								<xsl:with-param name="maxLength" select="dcl:predefinedDataType/@precision"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<xsl:call-template name="CreateInformationFormat">
								<xsl:with-param name="tableName" select="../@name"/>
								<xsl:with-param name="columnName" select="@name"/>
								<xsl:with-param name="dataType" select="dcl:predefinedDataType/@name"/>
								<xsl:with-param name="maxLength" select="dcl:predefinedDataType/@length"/>
							</xsl:call-template>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
			</oil:informationTypeFormats>
			<oil:conceptTypes>
				<xsl:for-each select="dcl:table">
					<xsl:variable name="resolvedReferenceConstraintColumnNames" select="dcl:referenceConstraint[@targetTable=$allTables/@name]/dcl:columnRef/@sourceName"/>
					<oil:conceptType id="{@name}" name="{@name}">
						<oil:children>
							<xsl:for-each select="dcl:column">
								<xsl:choose>
									<xsl:when test="@name=$resolvedReferenceConstraintColumnNames">
										<xsl:call-template name="OutputConceptTypeRef"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="OutputInformationType"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:for-each>
						</oil:children>
						<oil:uniquenessConstraints>
							<xsl:for-each select="dcl:uniquenessConstraint">
								<xsl:call-template name="OutputRoleSequenceUniquenessConstraint"/>
							</xsl:for-each>
						</oil:uniquenessConstraints>
					</oil:conceptType>
				</xsl:for-each>
			</oil:conceptTypes>
		</oil:model>
	</xsl:template>
	<!-- Templates to generate information formats -->
	<xsl:template name="CreateInformationFormat">
		<xsl:param name="tableName" />
		<xsl:param name="dataType"/>
		<xsl:param name="columnName"/>
		<xsl:param name="maxLength" select="'0'"/>
		<xsl:variable name="formatName" select="concat('InformationTypeFormat.', $tableName, '.', $columnName)"/>
		<xsl:choose>
			<xsl:when test="$dataType='CHARACTER' or $dataType='CHARACTER VARYING' or $dataType='CHARACTER LARGE OBJECT'">
				<odt:string id="{$formatName}" name="{$formatName}">
					<xsl:if test="$maxLength">
						<xsl:attribute name="maxLength">
							<xsl:value-of select="$maxLength"/>
						</xsl:attribute>
					</xsl:if>
				</odt:string>
			</xsl:when>
			<xsl:when test="$dataType='BIGINT' or $dataType='INTEGER' or $dataType='SMALLINT' or $dataType='DECIMAL' or $dataType='NUMERIC'">
				<odt:decimalNumber id="{$formatName}" name="{$formatName}" fractionDigits="0" />
			</xsl:when>
			<xsl:when test="$dataType='BINARY LARGE OBJECT'">
				<odt:binary id="{$formatName}" name="{$formatName}">
					<xsl:if test="$maxLength">
						<xsl:attribute name="maxLength">
							<xsl:value-of select="$maxLength"/>
						</xsl:attribute>
					</xsl:if>
				</odt:binary>
			</xsl:when>
			<xsl:when test="$dataType='FLOAT' or $dataType='DOUBLE PRECISION' or $dataType='REAL'">
				<odt:floatingPointNumber id="{$formatName}" name="{$formatName}">
					<xsl:if test="$maxLength">
						<xsl:attribute name="precision">
							<xsl:value-of select="$maxLength"/>
						</xsl:attribute>
					</xsl:if>
				</odt:floatingPointNumber>
			</xsl:when>
			<xsl:when test="$dataType='BOOLEAN'">
				<odt:boolean id="{$formatName}" name="{$formatName}"/>
			</xsl:when>
			<xsl:otherwise>
				<odt:string id="{$formatName}" name="{$formatName}" maxLength="255" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="OutputConceptTypeRef">
		<oil:relatedConceptType id="{../@name}.{@name}" name="{@name}" oppositeName="{../@name}" ref="{../dcl:referenceConstraint/dcl:columnRef[@sourceName=current()/@name]/../@targetTable}">
			<xsl:if test="not(@isNullable = 'True' or @isNullable = 'true' or @isNullable = '1')">
				<xsl:attribute name="isMandatory">
					<xsl:value-of select="true()"/>
				</xsl:attribute>
			</xsl:if>
		</oil:relatedConceptType>
	</xsl:template>
	<xsl:template name="OutputInformationType">
		<oil:informationType id="{../@name}.{@name}" name="{@name}" ref="InformationTypeFormat.{../@name}.{@name}">
			<xsl:if test="not(@isNullable = 'True' or @isNullable = 'true' or @isNullable = '1')">
				<xsl:attribute name="isMandatory">
					<xsl:value-of select="true()"/>
				</xsl:attribute>
			</xsl:if>
		</oil:informationType>
	</xsl:template>
	<xsl:template name="OutputRoleSequenceUniquenessConstraint">
		<oil:uniquenessConstraint id="{../@name}.{@name}" name="{@name}">
			<xsl:if test="@isPrimary = 'true' or @isPrimary = 1">
				<xsl:attribute name="isPreferred">
					<xsl:value-of select="true()"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:for-each select="dcl:columnRef">
				<oil:uniquenessChild ref="{../../@name}.{@name}"/>
			</xsl:for-each>
		</oil:uniquenessConstraint>
	</xsl:template>
</xsl:stylesheet>