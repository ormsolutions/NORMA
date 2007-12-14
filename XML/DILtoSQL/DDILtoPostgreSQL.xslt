<?xml version="1.0" encoding="utf-8"?>
<!--
	Copyright © Neumont University. All rights reserved.

	This software is provided 'as-is', without any express or implied warranty. In no event will the authors be held liable for any damages arising from the use of this software.
	Permission is granted to anyone to use this software for any purpose, including commercial applications, and to alter it and redistribute it freely, subject to the following restrictions:
	1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
	2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
	3. This notice may not be removed or altered from any source distribution.
-->
<!-- Contributors: Corey Kaylor, Kevin M. Owen -->
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:exsl="http://exslt.org/common"
	xmlns:dml="http://schemas.orm.net/DIL/DMIL"
	xmlns:dms="http://schemas.orm.net/DIL/DILMS"
	xmlns:dep="http://schemas.orm.net/DIL/DILEP"
	xmlns:ddt="http://schemas.orm.net/DIL/DILDT"
	xmlns:dil="http://schemas.orm.net/DIL/DIL"
	xmlns:ddl="http://schemas.orm.net/DIL/DDIL"
	extension-element-prefixes="exsl"
	exclude-result-prefixes="dml dms dep ddt dil ddl">

	<xsl:import href="DDILtoSQLStandard.xslt"/>

	<xsl:output method="text" encoding="utf-8" indent="no" omit-xml-declaration="yes"/>
	<xsl:strip-space elements="*"/>

	<xsl:template match="@defaultCharacterSet" mode="ForSchemaDefinition"/>

	<xsl:template match="dms:setSchemaStatement"/>

	<xsl:template match="ddl:generationClause"/>

	<xsl:template match="ddl:columnDefinition[ddl:identityColumnSpecification]">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:apply-templates select="@name" mode="ForColumnName"/>
		<xsl:apply-templates select="ddt:boolean | ddt:characterString | ddt:binaryString | ddt:date | ddt:time | ddt:interval | ddt:domain"/>
		<xsl:apply-templates select="ddt:exactNumeric | ddt:approximateNumeric" mode="ForPostgresIdentityColumn"/>
		<xsl:apply-templates select="ddl:defaultClause"/>
		<xsl:apply-templates select="ddl:generationClause"/>
		<xsl:apply-templates select="ddl:columnConstraintDefinition"/>
		<xsl:if test="position()!=last() or following-sibling::*">
			<xsl:text>,</xsl:text>
		</xsl:if>
	</xsl:template>

	<xsl:template match="ddt:exactNumeric | ddt:approximateNumeric" mode="ForPostgresIdentityColumn">
		<xsl:choose>
			<xsl:when test="@type='INTEGER' or @type='SMALLINT'">
				<xsl:text>SERIAL</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>BIGSERIAL</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="ddl:sqlInvokedProcedure">
		<xsl:value-of select="$NewLine"/>
		<xsl:text>CREATE FUNCTION </xsl:text>
		<xsl:apply-templates select="@catalog" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@schema" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@name" mode="ForSchemaQualifiedName"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$LeftParen"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:apply-templates select="ddl:sqlParameterDeclaration" />
		<xsl:value-of select="$RightParen"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:text>RETURNS VOID</xsl:text>
		<xsl:value-of select="$NewLine"/>
		<xsl:text>LANGUAGE SQL</xsl:text>
		<xsl:value-of select="$NewLine"/>
		<xsl:apply-templates select="ddl:sqlRoutineSpec" />
		<xsl:value-of select="$StatementDelimeter"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<xsl:template match="ddl:sqlRoutineSpec">
		<xsl:text>AS</xsl:text>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$IndentChar" />
		<xsl:text>'</xsl:text>
		<xsl:apply-templates select="child::*" />
		<xsl:text>'</xsl:text>
	</xsl:template>

	<xsl:template match="dml:fromConstructor">
		<xsl:value-of select="$LeftParen" />
		<xsl:apply-templates select="dep:simpleColumnReference"/>
		<xsl:value-of select="$RightParen" />
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$IndentChar" />
		<xsl:text>VALUES </xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates select="dep:sqlParameterReference" />
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="dep:sqlParameterReference">
		<xsl:text>$</xsl:text>
		<xsl:variable name="parameterReferenceName" select="@name"/>
		<xsl:for-each select="ancestor::ddl:sqlInvokedProcedure[1]/ddl:sqlParameterDeclaration">
			<xsl:if test="@name=$parameterReferenceName">
				<xsl:value-of select="position()"/>
			</xsl:if>
		</xsl:for-each>
		<xsl:if test="not(position()=last())">
			<xsl:text>, </xsl:text>
		</xsl:if>
	</xsl:template>

	<xsl:template match="ddl:atomicBlock">

	</xsl:template>

</xsl:stylesheet>
