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
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:dml="http://schemas.orm.net/DIL/DMIL"
	xmlns:dms="http://schemas.orm.net/DIL/DILMS"
	xmlns:dep="http://schemas.orm.net/DIL/DILEP"
	xmlns:ddt="http://schemas.orm.net/DIL/DILDT"
	xmlns:dil="http://schemas.orm.net/DIL/DIL"
	xmlns:ddl="http://schemas.orm.net/DIL/DDIL"
	extension-element-prefixes="msxsl"
	exclude-result-prefixes="dml dms dep ddt dil ddl">

	<xsl:import href="DDILtoSQLStandard.xslt"/>	

	<xsl:output method="text" encoding="utf-8" indent="no" omit-xml-declaration="yes"/>
	<xsl:strip-space elements="*"/>	

	<xsl:template match="ddl:schemaDefinition">
		<xsl:param name="indent"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>CREATE SCHEMA </xsl:text>
		<xsl:apply-templates select="@catalogName" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="@schemaName" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="@authorizationIdentifier" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="@defaultCharacterSet" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="ddl:path" mode="ForSchemaDefinition"/>
		<xsl:value-of select="$StatementDelimeter"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:text>GO</xsl:text>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:apply-templates>
			<xsl:with-param name="indent" select="concat($indent, $IndentChar)"/>
		</xsl:apply-templates>
	</xsl:template>
	
	<xsl:template match="dms:startTransactionStatement">
		<xsl:text>BEGIN TRANSACTION</xsl:text>
		<xsl:value-of select="$StatementDelimeter"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:text>GO</xsl:text>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<xsl:template match="@defaultCharacterSet" mode="ForSchemaDefinition"/>

	<xsl:template match="dms:setSchemaStatement"/>

	<xsl:template match="ddl:identityColumnSpecification">
		<xsl:text>IDENTITY </xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates select="child::*[1]"/>
		<xsl:text>, </xsl:text>
		<xsl:apply-templates select="child::*[2]"/>
		<xsl:value-of select="$RightParen"/>
		<xsl:text> </xsl:text>
	</xsl:template>

	<xsl:template match="ddl:sequenceGeneratorStartWithOption">		
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="ddl:sequenceGeneratorIncrementByOption">	
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="ddt:characterString">
		<xsl:choose>
			<xsl:when test="@type='CHARACTER' or @type='CHARACTER VARYING'">
				<xsl:if test="@type='CHARACTER'">
					<xsl:text>NATIONAL </xsl:text>					
				</xsl:if>
				<xsl:if test="@type='CHARACTER VARYING'">
					<xsl:text>NATIONAL </xsl:text>					
				</xsl:if>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-imports/>
			</xsl:otherwise>
		</xsl:choose>		
		<xsl:value-of select="@type"/>
		<xsl:value-of select="$LeftParen"/>
		<xsl:value-of select="@length"/>
		<xsl:value-of select="$RightParen"/>
		<xsl:text> </xsl:text>
		<xsl:apply-templates select="@characterSet" mode="ForColumnDataType"/>
		<xsl:apply-templates select="@collate" mode="ForColumnDataType"/>
	</xsl:template>

	<xsl:template match="dep:charLengthExpression">
		<xsl:text>LEN</xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="@lengthUnits"/>

	<xsl:template match="dep:trimFunction">
		<xsl:choose>
			<xsl:when test="@specification='BOTH'">
				<xsl:text>LTRIM</xsl:text>
				<xsl:value-of select="$LeftParen"/>
				<xsl:text>RTRIM</xsl:text>
				<xsl:value-of select="$LeftParen"/>
				<xsl:apply-templates select="dep:trimSource"/>
				<xsl:value-of select="$RightParen"/>
				<xsl:value-of select="$RightParen"/>
			</xsl:when>
			<xsl:when test="@specification='LEADING'">
				<xsl:text>LTRIM</xsl:text>
				<xsl:value-of select="$LeftParen"/>				
				<xsl:apply-templates select="dep:trimSource"/>
				<xsl:value-of select="$RightParen"/>				
			</xsl:when>
			<xsl:when test="@specification='TRAILING'">				
				<xsl:text>RTRIM</xsl:text>
				<xsl:value-of select="$LeftParen"/>
				<xsl:apply-templates select="dep:trimSource"/>
				<xsl:value-of select="$RightParen"/>				
			</xsl:when>
		</xsl:choose>				
	</xsl:template>

	<xsl:template match="@onUpdate" mode="ForReferenceSpecification">
		<xsl:text> ON UPDATE </xsl:text>
		<xsl:choose>
			<xsl:when test=".='RESTRICT'">
				<xsl:text>NO ACTION</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="."/>
			</xsl:otherwise>
		</xsl:choose>		
	</xsl:template>

	<xsl:template match="@onDelete" mode="ForReferenceSpecification">
		<xsl:text> ON DELETE </xsl:text>
		<xsl:choose>
			<xsl:when test=".='RESTRICT'">
				<xsl:text>NO ACTION</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="."/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="dep:constraintNameDefinition">
		<xsl:param name="tableName"/>
		<xsl:text>CONSTRAINT </xsl:text>
		<xsl:if test="@schema">
			<xsl:value-of select="@schema"/>
			<xsl:text>.</xsl:text>
		</xsl:if>
		<xsl:choose>
			<xsl:when test="$tableName">
				<xsl:choose>
					<xsl:when test="contains($tableName, '&quot;')">
						<xsl:variable name="tablePart1" select="substring-after($tableName, '&quot;')"/>
						<xsl:variable name="table" select="substring-before($tablePart1, '&quot;')"/>
						<xsl:text>&quot;</xsl:text>
						<xsl:value-of select="concat($table, @name)"/>
						<xsl:text>&quot;</xsl:text>
					</xsl:when>
					<xsl:otherwise>
						<xsl:variable name="table" select="concat($tableName, '_')"/>
						<xsl:value-of select="concat($table, @name)"/>
					</xsl:otherwise>
				</xsl:choose>				
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="@name"/>
			</xsl:otherwise>
		</xsl:choose>		
		<xsl:text> </xsl:text>
		<xsl:apply-templates/>
	</xsl:template>
	
</xsl:stylesheet>