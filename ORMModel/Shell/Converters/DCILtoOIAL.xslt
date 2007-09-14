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
	xmlns:odt="http://schemas.orm.net/ORMDataTypes"
	xmlns:oil="http://schemas.orm.net/OIAL"
	xmlns:dcl="http://schemas.orm.net/DIL/DCIL"
	extension-element-prefixes="exsl"
	exclude-result-prefixes="dcl">
	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:template match="dcl:schema">
		<oil:model sourceRef="{@name}" name="{@name}">
			<oil:informationTypeFormats>
				<xsl:for-each select="dcl:table/dcl:column">
					<xsl:if test="not(@name = ../dcl:referenceConstraint/dcl:columnRef/@sourceName)">
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
			<xsl:for-each select="dcl:table">
				<oil:conceptType name="{@name}" sourceRef="{@name}">
					<xsl:for-each select="dcl:column">
						<xsl:choose>
							<xsl:when test="@name = ../dcl:referenceConstraint/dcl:columnRef/@sourceName">
								<xsl:call-template name="OutputConceptTypeRef"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="OutputInformationType"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:for-each>
					<xsl:for-each select="dcl:uniquenessConstraint">
						<xsl:choose>
							<xsl:when test="count(dcl:columnRef) > 1">
								<xsl:call-template name="OutputRoleSequenceUniquenessConstraint"/>
							</xsl:when>
						</xsl:choose>
					</xsl:for-each>
				</oil:conceptType>
			</xsl:for-each>
		</oil:model>
	</xsl:template>
	<!-- Templates to generate information formats -->
	<xsl:template name="CreateInformationFormat">
		<xsl:param name="tableName" />
		<xsl:param name="dataType"/>
		<xsl:param name="columnName"/>
		<xsl:param name="maxLength" select="'0'"/>
		<xsl:variable name="formatName" select="concat(concat($tableName, '_'), $columnName)"/>
		<xsl:choose>
			<xsl:when test="$dataType='CHARACTER' or $dataType='CHARACTER VARYING' or $dataType='CHARACTER LARGE OBJECT'">
				<odt:string name="{$formatName}" maxLength="{$maxLength}" />
			</xsl:when>
			<xsl:when test="$dataType='BIGINT' or $dataType='INTEGER' or $dataType='SMALLINT' or $dataType='DECIMAL' or $dataType='DOUBLE PRECISION' or $dataType='REAL' or $dataType='NUMERIC'">
				<odt:decimalNumber name="{$formatName}" fractionDigits="0" />
			</xsl:when>
			<xsl:when test="$dataType='BINARY LARGE OBJECT'">
				<odt:binary maxLength="{$maxLength}" name="{$formatName}"/>
			</xsl:when>
			<xsl:when test="$dataType='FLOAT'">
				<odt:floatingPointNumber precision="{$maxLength}" name="{$formatName}"/>
			</xsl:when>
			<xsl:when test="$dataType='BOOLEAN'">
				<odt:boolean name="{$formatName}" fixed="false"/>
			</xsl:when>
			<xsl:otherwise>
				<odt:string name="{$formatName}" maxLength="255" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="OutputConceptTypeRef">
		<oil:conceptTypeRef name="{@name}" sourceRoleRef="{@name}" oppositeName="{../@name}" target="{../dcl:referenceConstraint/dcl:columnRef[@sourceName=current()/@name]/../@targetTable}">
			<xsl:attribute name="mandatory">
				<xsl:choose>
					<xsl:when test="@isNullable = 'True' or @isNullable = 'true'">
						<xsl:text>false</xsl:text>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>alethic</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:variable name="columnName" select="@name"/>
			<xsl:variable name="constraint" select="../dcl:uniquenessConstraint[dcl:columnRef[@name = current()/@name]]"/>
			<xsl:if test="../dcl:uniquenessConstraint[dcl:columnRef/@name = $columnName] and count(../dcl:uniquenessConstraint[dcl:columnRef/@name = $columnName]/dcl:columnRef) = 1">
				<oil:singleRoleUniquenessConstraint name="{../@name}_{@name}_UC" modality="alethic">
					<xsl:attribute name="isPreferred">
						<xsl:choose>
							<xsl:when test="$constraint/@isPrimary = 'true'">
								<xsl:text>true</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>false</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</oil:singleRoleUniquenessConstraint>
			</xsl:if>
		</oil:conceptTypeRef>
	</xsl:template>
	<xsl:template name="OutputInformationType">
		<oil:informationType name="{@name}" formatRef="{@name}" sourceRef="{@name}">
			<xsl:attribute name="mandatory">
				<xsl:choose>
					<xsl:when test="@isNullable = 'True' or @isNullable = 'true'">
						<xsl:text>false</xsl:text>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>alethic</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:variable name="columnName" select="@name"/>
			<xsl:variable name="constraint" select="../dcl:uniquenessConstraint[dcl:columnRef[@name = current()/@name]]"/>
			<xsl:if test="../dcl:uniquenessConstraint[dcl:columnRef/@name = $columnName] and count(../dcl:uniquenessConstraint[dcl:columnRef/@name = $columnName]/dcl:columnRef) = 1">
				<oil:singleRoleUniquenessConstraint name="{../@name}_{@name}_UC" modality="alethic">
					<xsl:attribute name="isPreferred">
						<xsl:choose>
							<xsl:when test="$constraint/@isPrimary = 'true'">
								<xsl:text>true</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>false</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</oil:singleRoleUniquenessConstraint>
			</xsl:if>
		</oil:informationType>
	</xsl:template>
	<xsl:template name="OutputRoleSequenceUniquenessConstraint">
		<oil:roleSequenceUniquenessConstraint name="{../@name}_{@name}_EUC" modality="alethic">
			<xsl:if test="@isPrimary = 'true'">
				<xsl:attribute name="isPreferred">
					<xsl:text>true</xsl:text>
				</xsl:attribute>
			</xsl:if>
			<oil:roleSequence>
				<xsl:for-each select="dcl:columnRef">
					<oil:typeRef targetConceptType="{../../@name}" targetChild="{@name}"/>
				</xsl:for-each>
			</oil:roleSequence>
		</oil:roleSequenceUniquenessConstraint>
	</xsl:template>
</xsl:stylesheet>