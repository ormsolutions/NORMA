<?xml version="1.0" encoding="utf-8"?>
<!--
	Copyright © 2005 Kevin M. Owen, Corey Kaylor, Korvyn Dornseif, and Neumont University

	This software is provided 'as-is', without any express or implied warranty. In no event will the authors be held liable for any damages arising from the use of this software.
	Permission is granted to anyone to use this software for any purpose, including commercial applications, and to alter it and redistribute it freely, subject to the following restrictions:
	1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
	2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
	3. This notice may not be removed or altered from any source distribution.
-->
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:dsf="urn:schemas-orm-net:DIL:DILSupportFunctions" 
	xmlns:dcl="http://schemas.orm.net/DIL/DCIL"
	xmlns:dil="http://schemas.orm.net/DIL/DIL"
	xmlns:ddt="http://schemas.orm.net/DIL/DILDT"
	xmlns:dep="http://schemas.orm.net/DIL/DILEP"
	xmlns:ddl="http://schemas.orm.net/DIL/DDIL"
	xmlns:dml="http://schemas.orm.net/DIL/DMIL"
	xmlns:dms="http://schemas.orm.net/DIL/DILMS"
	extension-element-prefixes="msxsl dsf">

	<xsl:import href="../DIL/DILSupportFunctions.xslt"/>
	
	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:strip-space elements="*"/>

	<xsl:template match="dcl:schema">
		<dil:root>
			<dms:startTransactionStatement isolationLevel="SERIALIZABLE" accessMode="READ WRITE"/>
			<ddl:schemaDefinition schemaName="{@name}" defaultCharacterSet="UTF8"/>
			<dms:setSchemaStatement>
				<ddt:characterStringLiteral value="{dsf:getCaseNormalForm(@name)}"/>
			</dms:setSchemaStatement>
			<xsl:apply-templates select="dcl:domainDataType"/>
			<xsl:apply-templates select="dcl:table" mode="GenerateTableBase"/>
			<xsl:apply-templates select="dcl:table" mode="GenerateTableReferences"/>
			<xsl:apply-templates select="dcl:trigger"/>
			<dms:commitStatement/>
		</dil:root>
	</xsl:template>

	<xsl:template match="dcl:domainDataType">
		<ddl:domainDefinition name="{@name}">
			<xsl:apply-templates select="dcl:predefinedDataType"/>
			<xsl:apply-templates select="dcl:checkConstraint" mode="GenerateConstraint">
				<xsl:with-param name="ElementName" select="'ddl:domainConstraint'"/>
			</xsl:apply-templates>
		</ddl:domainDefinition>
	</xsl:template>

	<xsl:template match="dcl:trigger">
		<ddl:triggerDefinition name="{@name}" actionTime="{@actionTime}" forEach="{@forEach}">
			<ddl:event type="{@event}">
				<xsl:for-each select="dcl:columns/dcl:columnRef">
					<ddl:column name="{@name}"/>
				</xsl:for-each>
			</ddl:event>
			<ddl:table name="{@targetTable}"/>
			<xsl:if test="dcl:referencing">
				<ddl:referencing>
					<xsl:for-each select="dcl:referencing">
						<xsl:choose>
							<xsl:when test="@target='OLD ROW'">
								<ddl:oldRow name="{@name}"/>
							</xsl:when>
							<xsl:when test="@target='NEW ROW'">
								<ddl:newRow name="{@name}"/>
							</xsl:when>
							<xsl:when test="@target='OLD TABLE'">
								<ddl:oldTable name="{@name}"/>
							</xsl:when>
							<xsl:when test="@target='NEW TABLE'">
								<ddl:newTable name="{@name}"/>
							</xsl:when>
						</xsl:choose>
					</xsl:for-each>
				</ddl:referencing>
			</xsl:if>
			<xsl:if test="dcl:when">
				<ddl:when>
					<xsl:copy-of select="dcl:when/child::*"/>
				</ddl:when>
			</xsl:if>
			<ddl:atomicBlock>
				<xsl:copy-of select="dcl:atomicBlock/child::*"/>
			</ddl:atomicBlock>
		</ddl:triggerDefinition>
	</xsl:template>

	<xsl:template match="dcl:table" mode="GenerateTableBase">
		<ddl:tableDefinition name="{@name}">
			<xsl:apply-templates select="dcl:column"/>
			<xsl:apply-templates select="dcl:uniquenessConstraint" mode="GenerateConstraint"/>
			<xsl:apply-templates select="dcl:checkConstraint" mode="GenerateConstraint"/>
		</ddl:tableDefinition>
	</xsl:template>

	<xsl:template match="dcl:table" mode="GenerateTableReferences">
		<xsl:variable name="tableName" select="@name"/>
		<xsl:for-each select="dcl:referenceConstraint">
			<ddl:alterTableStatement name="{$tableName}">
				<xsl:apply-templates select="." mode="GenerateConstraint">
					<xsl:with-param name="ElementName" select="'ddl:addTableConstraintDefinition'"/>
				</xsl:apply-templates>
			</ddl:alterTableStatement>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="dcl:column">
		<ddl:columnDefinition name="{@name}">
			<xsl:apply-templates select="dcl:predefinedDataType"/>
			<xsl:apply-templates select="dcl:domainDataTypeRef"/>
			<xsl:choose>
				<xsl:when test="@isIdentity='true'">
					<ddl:identityColumnSpecification type="ALWAYS">
						<ddl:sequenceGeneratorStartWithOption>
							<ddt:exactNumericLiteral value="1"/>
						</ddl:sequenceGeneratorStartWithOption>
						<ddl:sequenceGeneratorIncrementByOption>
							<ddt:exactNumericLiteral value="1"/>
						</ddl:sequenceGeneratorIncrementByOption>
					</ddl:identityColumnSpecification>
				</xsl:when>
				<xsl:when test="dcl:generationCode">
					<ddl:generationClause>
						<xsl:copy-of select="dcl:generationCode/child::*"/>
					</ddl:generationClause>
				</xsl:when>
			</xsl:choose>
			<xsl:if test="@isNullable='false'">
				<ddl:columnConstraintDefinition>
					<ddl:notNullKeyword/>
				</ddl:columnConstraintDefinition>
			</xsl:if>
		</ddl:columnDefinition>
	</xsl:template>

	<xsl:template match="dcl:checkConstraint | dcl:uniquenessConstraint | dcl:referenceConstraint" mode="GenerateConstraint">
		<xsl:param name="ElementName" select="'ddl:tableConstraintDefinition'"/>
		<xsl:element name="{$ElementName}">
			<xsl:attribute name="dep:constraintCharacteristics">
				<xsl:value-of select="'INITIALLY IMMEDIATE NOT DEFERRABLE'"/>
			</xsl:attribute>
			<dep:constraintNameDefinition name="{@name}"/>
			<xsl:apply-templates select="." mode="GenerateConstraintCore"/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="dcl:checkConstraint" mode="GenerateConstraintCore">
		<ddl:checkConstraintDefinition>
			<xsl:copy-of select="child::*"/>
		</ddl:checkConstraintDefinition>
	</xsl:template>
	<xsl:template match="dcl:uniquenessConstraint" mode="GenerateConstraintCore">
		<xsl:variable name="uniqueConstraintType">
			<xsl:choose>
				<xsl:when test="@isPrimary='true'">
					<xsl:value-of select="'PRIMARY KEY'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'UNIQUE'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<ddl:uniqueConstraintDefinition type="{$uniqueConstraintType}">
			<xsl:for-each select="dcl:columnRef">
				<ddl:column name="{@name}"/>
			</xsl:for-each>
		</ddl:uniqueConstraintDefinition>
	</xsl:template>
	<xsl:template match="dcl:referenceConstraint" mode="GenerateConstraintCore">
		<ddl:referentialConstraintDefinition>
			<xsl:for-each select="dcl:columnRef">
				<ddl:column name="{@sourceName}"/>
			</xsl:for-each>
			<ddl:referencesSpecification name="{@targetTable}" onDelete="RESTRICT" onUpdate="RESTRICT">
				<xsl:for-each select="dcl:columnRef">
					<ddl:referenceColumn name="{@targetName}"/>
				</xsl:for-each>
			</ddl:referencesSpecification>
		</ddl:referentialConstraintDefinition>
	</xsl:template>


	<xsl:template match="dcl:domainDataTypeRef">
		<ddt:domain name="{@name}"/>
	</xsl:template>
	
	<xsl:template match="dcl:predefinedDataType[@name='FLOAT' or @name='REAL' or @name='DOUBLE PRECISION']">
		<ddt:approximateNumeric type="{@name}">
			<xsl:if test="@name='FLOAT'">
				<xsl:copy-of select="@precision"/>
			</xsl:if>
		</ddt:approximateNumeric>
	</xsl:template>
	<xsl:template match="dcl:predefinedDataType[@name='BINARY LARGE OBJECT']">
		<ddt:binaryString type="{@name}" length="{@precision}"/>
	</xsl:template>
	<xsl:template match="dcl:predefinedDataType[@name='BOOLEAN']">
		<ddt:boolean type="{@name}"/>
	</xsl:template>
	<xsl:template match="dcl:predefinedDataType[@name='CHARACTER' or @name='CHARACTER VARYING' or @name='CHARACTER LARGE OBJECT']">
		<ddt:characterString type="{@name}" length="{@precision}"/>
	</xsl:template>
	<xsl:template match="dcl:predefinedDataType[@name='NUMERIC' or @name='DECIMAL' or @name='SMALLINT' or @name='INTEGER' or @name='BIGINT']">
		<ddt:exactNumeric type="{@name}">
			<xsl:if test="@name='NUMERIC' or @name='DECIMAL'">
				<xsl:copy-of select="@precision"/>
				<xsl:copy-of select="@scale"/>
			</xsl:if>
		</ddt:exactNumeric>
	</xsl:template>
	<xsl:template match="dcl:predefinedDataType[@name='INTERVAL']">
		<!-- TODO: DCIL currently doesn't have a way for the user to specify the @fields they want for an INTERVAL -->
	</xsl:template>
	<xsl:template match="dcl:predefinedDataType[@name='DATE']">
		<ddt:date type="{@name}"/>
	</xsl:template>
	<xsl:template match="dcl:predefinedDataType[@name='TIME' or @name='TIMESTAMP']">
		<!-- TODO: DCIL currently doesn't have a way for the user to specify the @zone they want for a TIME or TIMESTAMP -->
	</xsl:template>
	
</xsl:stylesheet>