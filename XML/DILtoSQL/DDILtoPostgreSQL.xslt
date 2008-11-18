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
	xmlns:dsf="urn:schemas-orm-net:DIL:DILSupportFunctions"
	xmlns:dil="http://schemas.orm.net/DIL/DIL"
	xmlns:ddt="http://schemas.orm.net/DIL/DILDT"
	xmlns:dep="http://schemas.orm.net/DIL/DILEP"
	xmlns:dms="http://schemas.orm.net/DIL/DILMS"
	xmlns:dml="http://schemas.orm.net/DIL/DMIL"
	xmlns:ddl="http://schemas.orm.net/DIL/DDIL"
	extension-element-prefixes="exsl dsf"
	exclude-result-prefixes="dil ddt dep dms dml ddl">

	<xsl:import href="DDILtoSQLStandard.xslt"/>
	<xsl:import href="TinyIntRemover.xslt"/>

	<xsl:output method="text" encoding="utf-8" indent="no" omit-xml-declaration="yes"/>
	<xsl:strip-space elements="*"/>

	<xsl:param name="DefaultMaximumStringLength" select="''"/>
	<xsl:param name="DefaultMaximumExactNumericPrecisionAndScale" select="''"/>
	<xsl:param name="DefaultMaximumExactNumericPrecisionWithScale" select="1000"/>
	<xsl:param name="DefaultMaximumApproximateNumericPrecision" select="''"/>

	<xsl:template match="/">
		<xsl:variable name="tinyIntRemovedDilFragment">
			<xsl:apply-templates mode="TinyIntRemover" select="."/>
		</xsl:variable>
		<xsl:apply-templates select="exsl:node-set($tinyIntRemovedDilFragment)/child::*"/>
	</xsl:template>
	
	<xsl:template match="@defaultCharacterSet" mode="ForSchemaDefinition"/>
	
	<xsl:template match="dms:setSchemaStatement">
		<xsl:param name="indent"/>
		<xsl:choose>
			<xsl:when test="ddt:characterStringLiteral">
				<xsl:value-of select="$NewLine"/>
				<xsl:value-of select="$indent"/>
				<xsl:text>SET search_path TO </xsl:text>
				<!-- UNDONE: We need a better way to take the string literal form of the schema name and turn it back in to an identifier. -->
				<xsl:value-of select="dsf:makeValidIdentifier(ddt:characterStringLiteral/@value)"/>
				<xsl:text>,"$user",public</xsl:text>
				<xsl:value-of select="$StatementDelimiter"/>
				<xsl:value-of select="$NewLine"/>
			</xsl:when>
			<xsl:otherwise>
				<!-- UNDONE: Figure out what to do when we have something other than a ddt:characterStringLiteral. -->
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="ddl:generationClause"/>

	<!--
		UNDONE: Adjust rendering of ddl:identityColumnSpecification to explicitly create a sequence generator and use it instead of the SERIAL types,
		so that the sequence generator options the user sets can be respected.
	-->

	<xsl:template match="ddl:columnDefinition[ddl:identityColumnSpecification]">
		<xsl:param name="indent"/>
		<xsl:if test="not(position()=1)">
			<xsl:text>,</xsl:text>
		</xsl:if>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:apply-templates select="@name" mode="ForColumnName"/>
		<xsl:apply-templates select="ddt:boolean | ddt:characterString | ddt:binaryString | ddt:date | ddt:time | ddt:interval | ddt:domain"/>
		<xsl:apply-templates select="ddt:exactNumeric | ddt:approximateNumeric" mode="ForPostgresIdentityColumn"/>
		<xsl:apply-templates select="ddl:defaultClause"/>
		<xsl:apply-templates select="ddl:generationClause"/>
		<xsl:apply-templates select="ddl:columnConstraintDefinition"/>
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

	<xsl:template match="ddt:characterString | ddt:binaryString" mode="ForDataTypeLengthWithMultiplier">
		<xsl:call-template name="GetTotalDataTypeLength"/>
	</xsl:template>

	<xsl:template match="@type[.='CHARACTER LARGE OBJECT']" mode="ForDataType">
		<xsl:text>CHARACTER VARYING</xsl:text>
	</xsl:template>

	<xsl:template match="@type[.='BINARY' or .='BINARY VARYING' or .='BINARY LARGE OBJECT']" mode="ForDataType">
		<xsl:text>BYTEA</xsl:text>
		<!-- UNDONE: Add constraints to support the original length requested. -->
	</xsl:template>
	
	<!-- UNDONE: Handle rendering ddt:binaryStringLiteral. -->

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
		<xsl:value-of select="$StatementDelimiter"/>
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
