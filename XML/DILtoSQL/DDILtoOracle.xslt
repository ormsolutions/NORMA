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
	<xsl:import href="TruthValueTestRemover.xslt"/>
	<xsl:import href="TinyIntRemover.xslt"/>
	<xsl:import href="DomainInliner.xslt"/>

	<xsl:output method="text" encoding="utf-8" indent="no" omit-xml-declaration="yes"/>
	<xsl:strip-space elements="*"/>

	<xsl:param name="DefaultMaximumStringLength" select="2000"/>
	<xsl:param name="DefaultMaximumLargeObjectStringLength" select="''"/>

	<xsl:param name="TinyIntRemover.ReplacementDataType" select="'DECIMAL'"/>
	<xsl:param name="TinyIntRemover.ReplacementDataTypePrecision" select="3"/>
	<xsl:param name="TinyIntRemover.ReplacementDataTypeScale" select="0"/>

	<xsl:template match="/">
		<xsl:variable name="truthValueTestRemovedDilFragment">
			<xsl:apply-templates mode="TruthValueTestRemover" select="."/>
		</xsl:variable>
		<xsl:variable name="tinyIntRemovedDilFragment">
			<xsl:apply-templates mode="TinyIntRemover" select="exsl:node-set($truthValueTestRemovedDilFragment)"/>
		</xsl:variable>
		<xsl:variable name="domainInlinedDilFragment">
			<xsl:apply-templates mode="DomainInliner" select="exsl:node-set($tinyIntRemovedDilFragment)"/>
		</xsl:variable>
		<xsl:apply-templates select="exsl:node-set($domainInlinedDilFragment)/child::*"/>
	</xsl:template>

	<xsl:template match="dms:startTransactionStatement">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>SET TRANSACTION </xsl:text>
		<xsl:choose>
			<xsl:when test="@isolationLevel">
				<xsl:text>ISOLATION LEVEL </xsl:text>
				<xsl:value-of select="@isolationLevel"/>
			</xsl:when>
			<xsl:when test="@accessMode">
				<xsl:value-of select="@accessMode"/>
			</xsl:when>
		</xsl:choose>
		<xsl:value-of select="$StatementDelimiter"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<xsl:template match="ddl:schemaDefinition">
		<!-- We are absorbing the create schema because the schema name must be the Oracle Database username. -->
		<!-- So until we have an easier way of getting that username, it will be useless to us. -->
	</xsl:template>

	<xsl:template match="dms:setSchemaStatement"/>

	<xsl:template match="ddt:characterString | ddt:binaryString" mode="ForDataTypeLengthWithMultiplier">
		<xsl:call-template name="GetTotalDataTypeLength"/>
	</xsl:template>

	<xsl:template match="@type[.='NUMERIC' or .='DECIMAL']" mode="ForDataType">
		<xsl:text>NUMBER</xsl:text>
	</xsl:template>

	<xsl:template match="@type[.='SMALLINT']" mode="ForDataType">
		<xsl:text>NUMBER</xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:text>5,0</xsl:text>
		<xsl:value-of select="$RightParen"/>
		<!-- UNDONE: Add constraints to enforce SMALLINT range. -->
	</xsl:template>

	<xsl:template match="@type[.='INTEGER']" mode="ForDataType">
		<xsl:text>NUMBER</xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:text>10,0</xsl:text>
		<xsl:value-of select="$RightParen"/>
		<!-- UNDONE: Add constraints to enforce INTEGER range. -->
	</xsl:template>

	<xsl:template match="@type[.='BIGINT']" mode="ForDataType">
		<xsl:text>NUMBER</xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:text>19,0</xsl:text>
		<xsl:value-of select="$RightParen"/>
		<!-- UNDONE: Add constraints to enforce BIGINT range. -->
	</xsl:template>

	<xsl:template match="@type[.='REAL']" mode="ForDataType">
		<xsl:text>BINARY_FLOAT</xsl:text>
	</xsl:template>

	<xsl:template match="@type[.='DOUBLE PRECISION']" mode="ForDataType">
		<xsl:text>BINARY_DOUBLE</xsl:text>
	</xsl:template>

	<xsl:template match="@type[.='CHARACTER']" mode="ForDataType">
		<xsl:text>NCHAR</xsl:text>
	</xsl:template>
	
	<xsl:template match="@type[.='CHARACTER VARYING']" mode="ForDataType">
		<xsl:text>NVARCHAR2</xsl:text>
	</xsl:template>

	<xsl:template match="@type[.='CHARACTER LARGE OBJECT']" mode="ForDataType">
		<xsl:text>NCLOB</xsl:text>
	</xsl:template>

	<xsl:template match="@type[.='BINARY' or .='BINARY VARYING']" mode="ForDataType">
		<xsl:text>RAW</xsl:text>
	</xsl:template>

	<xsl:template match="@type[.='BINARY LARGE OBJECT']" mode="ForDataType">
		<xsl:text>BLOB</xsl:text>
	</xsl:template>
	
	<xsl:template match="@type[.='BOOLEAN']" mode="ForDataType">
		<xsl:text>NCHAR(1)</xsl:text>
		<!-- UNDONE: Add constraints to restrict BOOLEAN to the appropriate values. -->
	</xsl:template>

	<xsl:template match="ddt:booleanLiteral">
		<xsl:choose>
			<xsl:when test="@value='TRUE'">
				<xsl:text>'Y'</xsl:text>
			</xsl:when>
			<xsl:when test="@value='FALSE'">
				<xsl:text>'N'</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>NULL</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="ddt:characterStringLiteral">
		<xsl:text>N</xsl:text>
		<xsl:apply-imports/>
	</xsl:template>

	<xsl:template match="@schema" mode="ForSchemaQualifiedName">
		<!-- We also absorb references to the schema name, because the schema name must be the Oracle Database username. -->
	</xsl:template>

	<xsl:template match="ddl:referencesSpecification">
		<xsl:text> REFERENCES </xsl:text>
		<xsl:value-of select="@name"/>
		<xsl:text> </xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
		<xsl:text> </xsl:text>
		<xsl:apply-templates select="@match" mode="ForReferenceSpecification"/>
		<xsl:apply-templates select="@onDelete" mode="ForReferenceSpecification"/>
		<xsl:apply-templates select="@onUpdate" mode="ForReferenceSpecification"/>
	</xsl:template>

	<xsl:template match="ddl:identityColumnSpecification">
		<!-- This should always be absorbed until Oracle provides some kind of auto-incremented number field. -->
		<!-- The only way to make this happen is to create a sequence, trigger, and regular column Integer datatype. -->
	</xsl:template>

	<xsl:template match="dep:charLengthExpression">
		<xsl:text>LENGTH</xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="@onUpdate" mode="ForReferenceSpecification"/>

	<xsl:template match="@onDelete" mode="ForReferenceSpecification">
		<!-- Absorbing but have the option to ON DELETE SET NULL -->
	</xsl:template>

	<xsl:template match="@match" mode="ForReferenceSpecification"/>

</xsl:stylesheet>
