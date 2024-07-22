﻿<?xml version="1.0" encoding="utf-8"?>
<!--
	Copyright © Neumont University. All rights reserved.

	This software is provided 'as-is', without any express or implied warranty. In no event will the authors be held liable for any damages arising from the use of this software.
	Permission is granted to anyone to use this software for any purpose, including commercial applications, and to alter it and redistribute it freely, subject to the following restrictions:
	1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
	2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
	3. This notice may not be removed or altered from any source distribution.
-->
<!-- Contributors: Kevin M. Owen, Corey Kaylor, Clé Diggins -->
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:exsl="http://exslt.org/common"
	xmlns:dsf="urn:schemas-orm-net:DIL:DILSupportFunctions"
	xmlns:dcl="http://schemas.orm.net/DIL/DCIL"
	xmlns:dil="http://schemas.orm.net/DIL/DIL"
	xmlns:ddt="http://schemas.orm.net/DIL/DILDT"
	xmlns:dep="http://schemas.orm.net/DIL/DILEP"
	xmlns:dms="http://schemas.orm.net/DIL/DILMS"
	xmlns:dml="http://schemas.orm.net/DIL/DMIL"
	xmlns:ddl="http://schemas.orm.net/DIL/DDIL"
	extension-element-prefixes="exsl dsf">

	<xsl:import href="DILSupportFunctions.xslt"/>

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:strip-space elements="*"/>

	<xsl:template match="dcl:schema">
		<dil:root>
			<dms:startTransactionStatement isolationLevel="SERIALIZABLE" accessMode="READ WRITE"/>
			<ddl:schemaDefinition schemaName="{@name}" defaultCharacterSet="UTF8"/>
			<dms:setSchemaStatement>
				<ddt:characterStringLiteral value="{dsf:getInformationSchemaForm(@name)}"/>
			</dms:setSchemaStatement>
			<xsl:apply-templates select="dcl:domain"/>
			<xsl:apply-templates select="dcl:table" mode="GenerateTableBase"/>
			<xsl:apply-templates select="dcl:table" mode="GenerateTableReferences"/>
			<xsl:apply-templates select="dcl:trigger"/>
			<xsl:apply-templates select="dcl:procedure" />
			<dms:commitStatement/>
		</dil:root>
	</xsl:template>

	<xsl:template match="dcl:catalog">
		<!-- Define elements in all schemas so that they can reference each other, then
		add references, triggers, and procedures. -->
		<xsl:variable name="schemas" select="dcl:schema"/>
		<dil:root>
			<dms:startTransactionStatement isolationLevel="SERIALIZABLE" accessMode="READ WRITE"/>
			<xsl:for-each select="$schemas">
				<ddl:schemaDefinition schemaName="{@name}" defaultCharacterSet="UTF8"/>
				<xsl:variable name="domains" select="dcl:domain"/>
				<xsl:if test="$domains">
					<dms:setSchemaStatement>
						<ddt:characterStringLiteral value="{dsf:getInformationSchemaForm(@name)}"/>
					</dms:setSchemaStatement>
					<xsl:apply-templates select="dcl:domain"/>
				</xsl:if>
			</xsl:for-each>
			<xsl:for-each select="$schemas">
				<xsl:variable name="tables" select="dcl:table"/>
				<xsl:if test="$tables">
					<dms:setSchemaStatement>
						<ddt:characterStringLiteral value="{dsf:getInformationSchemaForm(@name)}"/>
					</dms:setSchemaStatement>
					<xsl:apply-templates select="dcl:table" mode="GenerateTableBase"/>
				</xsl:if>
			</xsl:for-each>
			<xsl:for-each select="$schemas">
				<xsl:variable name="otherFragment">
					<xsl:apply-templates select="dcl:table" mode="GenerateTableReferences"/>
					<xsl:apply-templates select="dcl:trigger"/>
					<xsl:apply-templates select="dcl:procedure" />
				</xsl:variable>
				<xsl:variable name="other" select="exsl:node-set($otherFragment)/*"/>
				<xsl:if test="$other">
					<dms:setSchemaStatement>
						<ddt:characterStringLiteral value="{dsf:getInformationSchemaForm(@name)}"/>
					</dms:setSchemaStatement>
					<xsl:copy-of select="$other"/>
				</xsl:if>
			</xsl:for-each>
			<dms:commitStatement/>
		</dil:root>
	</xsl:template>

	<xsl:template name="GenerateSchemaAttribute">
		<xsl:apply-templates select="." mode="GenerateSchemaAttribute"/>
	</xsl:template>
	<xsl:template name="GenerateSchemaAttributeFromContext">
		<xsl:variable name="contextSchema" select="ancestor::dcl:schema[1]"/>
		<xsl:if test="$contextSchema">
			<xsl:attribute name="schema">
				<xsl:value-of select="$contextSchema/@name"/>
			</xsl:attribute>
		</xsl:if>
	</xsl:template>
	<xsl:template match="*" mode="GenerateSchemaAttribute">
		<xsl:call-template name="GenerateSchemaAttributeFromContext"/>
	</xsl:template>
	<xsl:template match="dcl:referenceConstraint" mode="GenerateSchemaAttribute">
		<xsl:variable name="explicitSchema" select="@targetSchema"/>
		<xsl:choose>
			<xsl:when test="$explicitSchema">
				<xsl:attribute name="schema">
					<xsl:value-of select="$explicitSchema"/>
				</xsl:attribute>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="GenerateSchemaAttributeFromContext"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="dcl:domainRef" mode="GenerateSchemaAttribute">
		<xsl:variable name="explicitSchema" select="@schemaName"/>
		<xsl:choose>
			<xsl:when test="$explicitSchema">
				<xsl:attribute name="schema">
					<xsl:value-of select="$explicitSchema"/>
				</xsl:attribute>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="GenerateSchemaAttributeFromContext"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="dcl:domain">
		<ddl:domainDefinition name="{@name}">
			<xsl:call-template name="GenerateSchemaAttribute"/>
			<xsl:apply-templates select="dcl:predefinedDataType"/>
			<xsl:apply-templates select="dcl:checkConstraint" mode="GenerateConstraint">
				<xsl:with-param name="ElementName" select="'ddl:domainConstraintDefinition'"/>
			</xsl:apply-templates>
		</ddl:domainDefinition>
	</xsl:template>

	<xsl:template match="dcl:trigger">
		<ddl:triggerDefinition name="{@name}" actionTime="{@actionTime}" forEach="{@forEach}">
			<xsl:call-template name="GenerateSchemaAttribute"/>
			<ddl:event type="{@event}">
				<xsl:for-each select="dcl:columns/dcl:columnRef">
					<dep:simpleColumnReference name="{@name}"/>
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
			<xsl:call-template name="GenerateSchemaAttribute"/>
			<xsl:apply-templates select="dcl:column"/>
			<xsl:apply-templates select="dcl:uniquenessConstraint" mode="GenerateConstraint"/>
			<xsl:apply-templates select="dcl:checkConstraint" mode="GenerateConstraint"/>
		</ddl:tableDefinition>
	</xsl:template>

	<xsl:template match="dcl:table" mode="GenerateTableReferences">
		<xsl:variable name="tableName" select="@name"/>
		<xsl:variable name="table" select="."/>
		<xsl:for-each select="dcl:referenceConstraint">
			<ddl:alterTableStatement name="{$tableName}">
				<xsl:for-each select="$table">
					<xsl:call-template name="GenerateSchemaAttribute"/>
				</xsl:for-each>
				<xsl:apply-templates select="." mode="GenerateConstraint">
					<xsl:with-param name="ElementName" select="'ddl:addTableConstraintDefinition'"/>
				</xsl:apply-templates>
			</ddl:alterTableStatement>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="dcl:column">
		<ddl:columnDefinition name="{@name}">
			<xsl:apply-templates select="dcl:predefinedDataType"/>
			<xsl:apply-templates select="dcl:domainRef"/>
			<xsl:choose>
				<xsl:when test="@isIdentity='true'">
					<xsl:choose>
						<xsl:when test="dcl:predefinedDataType[@name='UNIQUEIDENTIFIER']">
							<ddl:defaultClause>
								<dep:newUniqueIdentifierKeyword/>
							</ddl:defaultClause>
						</xsl:when>
						<xsl:otherwise>
							<ddl:identityColumnSpecification type="ALWAYS">
								<ddl:sequenceGeneratorStartWithOption>
									<ddt:exactNumericLiteral value="1"/>
								</ddl:sequenceGeneratorStartWithOption>
								<ddl:sequenceGeneratorIncrementByOption>
									<ddt:exactNumericLiteral value="1"/>
								</ddl:sequenceGeneratorIncrementByOption>
							</ddl:identityColumnSpecification>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="dcl:generationCode">
					<ddl:generationClause>
						<xsl:copy-of select="dcl:generationCode/child::*"/>
					</ddl:generationClause>
				</xsl:when>
				<xsl:when test="dil:defaultClause">
					<xsl:copy-of select="."/>
				</xsl:when>
			</xsl:choose>
			<xsl:if test="@isNullable='false'">
				<ddl:columnConstraintDefinition>
					<ddl:notNullKeyword/>
				</ddl:columnConstraintDefinition>
			</xsl:if>
		</ddl:columnDefinition>
	</xsl:template>

	<xsl:template match="dcl:procedure">
		<ddl:sqlInvokedProcedure name="{@name}">
			<xsl:call-template name="GenerateSchemaAttribute"/>
			<xsl:apply-templates select="child::*" />
		</ddl:sqlInvokedProcedure>
	</xsl:template>

	<xsl:template match="dcl:parameter">
		<ddl:sqlParameterDeclaration name="{@name}" mode="{@mode}">
			<xsl:apply-templates select="dcl:predefinedDataType"/>
		</ddl:sqlParameterDeclaration>
	</xsl:template>

	<!-- UNDONE: Why were these even in here in the first place? -->
	<!-- UNDONE: This part needs to be fixed. -->
	<!--<xsl:template match="dml:insertStatement">
		<ddl:sqlRoutineSpec rightsClause="INVOKER">
			<dml:insertStatement schema="{@schema}" name="{@name}">
				<dml:fromConstructor>
					<xsl:for-each select="dml:fromConstructor/dep:simpleColumnReference">
						<dep:simpleColumnReference name="{@name}"/>
					</xsl:for-each>
					<xsl:for-each select="dml:fromConstructor/dep:simpleColumnReference">
						<dep:sqlParameterReference name="{@name}"/>
					</xsl:for-each>
				</dml:fromConstructor>
			</dml:insertStatement>
		</ddl:sqlRoutineSpec>
	</xsl:template>

	<xsl:template match="dml:updateStatement">
		<ddl:sqlRoutineSpec rightsClause="INVOKER">
			<xsl:copy-of select="." />
		</ddl:sqlRoutineSpec>
	</xsl:template>

	<xsl:template match="dml:deleteStatement">
		<ddl:sqlRoutineSpec rightsClause="INVOKER">
		<dml:deleteStatement schema="{@schema}" name="{@name}">
			<xsl:for-each select="dml:whereClause">
				<xsl:copy-of select="." />
			</xsl:for-each>
		</dml:deleteStatement>
		</ddl:sqlRoutineSpec>
	</xsl:template>-->

	<xsl:template match="dcl:checkConstraint | dcl:uniquenessConstraint | dcl:referenceConstraint" mode="GenerateConstraint">
		<xsl:param name="ElementName" select="'ddl:tableConstraintDefinition'"/>
		<xsl:element name="{$ElementName}">
			<xsl:attribute name="name">
				<xsl:value-of select="@name"/>
			</xsl:attribute>
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
				<dep:simpleColumnReference name="{@name}"/>
			</xsl:for-each>
		</ddl:uniqueConstraintDefinition>
	</xsl:template>
	<xsl:template match="dcl:referenceConstraint" mode="GenerateConstraintCore">
		<ddl:referentialConstraintDefinition>
			<xsl:for-each select="dcl:columnRef">
				<dep:simpleColumnReference name="{@sourceName}"/>
			</xsl:for-each>
			<ddl:referencesSpecification name="{@targetTable}" onDelete="RESTRICT" onUpdate="RESTRICT">
				<xsl:call-template name="GenerateSchemaAttribute"/>
				<xsl:for-each select="dcl:columnRef">
					<dep:simpleColumnReference name="{@targetName}"/>
				</xsl:for-each>
			</ddl:referencesSpecification>
		</ddl:referentialConstraintDefinition>
	</xsl:template>


	<xsl:template match="dcl:domainRef">
		<ddt:domain name="{@name}">
			<xsl:call-template name="GenerateSchemaAttribute"/>
		</ddt:domain>
	</xsl:template>

	<xsl:template match="dcl:predefinedDataType[@name='FLOAT' or @name='REAL' or @name='DOUBLE PRECISION']">
		<ddt:approximateNumeric type="{@name}">
			<xsl:if test="@name='FLOAT'">
				<xsl:copy-of select="@precision"/>
			</xsl:if>
		</ddt:approximateNumeric>
	</xsl:template>
	<xsl:template match="dcl:predefinedDataType[@name='BINARY' or @name='BINARY VARYING' or @name='BINARY LARGE OBJECT']">
		<ddt:binaryString type="{@name}">
			<xsl:copy-of select="@length"/>
		</ddt:binaryString>
	</xsl:template>
	<xsl:template match="dcl:predefinedDataType[@name='BOOLEAN']">
		<ddt:boolean type="{@name}"/>
	</xsl:template>
	<xsl:template match="dcl:predefinedDataType[@name='CHARACTER' or @name='CHARACTER VARYING' or @name='CHARACTER LARGE OBJECT']">
		<ddt:characterString type="{@name}">
			<xsl:copy-of select="@length"/>
		</ddt:characterString>
	</xsl:template>
	<xsl:template match="dcl:predefinedDataType[@name='NUMERIC' or @name='DECIMAL' or @name='TINYINT' or @name='SMALLINT' or @name='INTEGER' or @name='BIGINT' or @name='UNIQUEIDENTIFIER']">
		<ddt:exactNumeric type="{@name}">
			<xsl:if test="@name='NUMERIC' or @name='DECIMAL'">
				<xsl:copy-of select="@precision"/>
				<xsl:copy-of select="@scale"/>
			</xsl:if>
		</ddt:exactNumeric>
	</xsl:template>
	<xsl:template match="dcl:predefinedDataType[@name='INTERVAL']">
		<!-- TODO: DCIL currently doesn't have a way for the user to specify the @fields they want for an INTERVAL -->
		<ddt:interval type="{@name}" fields="DAY TO SECOND"/>
	</xsl:template>
	<xsl:template match="dcl:predefinedDataType[@name='DATE']">
		<ddt:date type="{@name}"/>
	</xsl:template>
	<xsl:template match="dcl:predefinedDataType[@name='TIME' or @name='TIMESTAMP' or @name='DATETIME']">
		<!-- TODO: DCIL currently doesn't have a way for the user to specify the @zone they want for a TIME or TIMESTAMP -->
		<ddt:time type="{@name}"/>
	</xsl:template>

</xsl:stylesheet>
