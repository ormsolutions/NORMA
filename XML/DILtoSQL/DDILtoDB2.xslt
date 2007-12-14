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
	<xsl:import href="TruthValueTestRemover.xslt"/>
	<xsl:import href="DomainInliner.xslt"/>

	<xsl:output method="text" encoding="utf-8" indent="no" omit-xml-declaration="yes"/>
	<xsl:strip-space elements="*"/>

	<xsl:template match="/">
		<xsl:variable name="truthValueTestRemovedDilFragment">
			<xsl:apply-templates mode="TruthValueTestRemover" select="."/>
		</xsl:variable>
		<xsl:variable name="domainInlinedDilFragment">
			<xsl:apply-templates mode="DomainInliner" select="exsl:node-set($truthValueTestRemovedDilFragment)/child::*"/>
		</xsl:variable>
		<xsl:apply-templates select="exsl:node-set($domainInlinedDilFragment)/child::*"/>
	</xsl:template>

	<xsl:template match="dms:startTransactionStatement"/>

	<xsl:template match="@defaultCharacterSet" mode="ForSchemaDefinition"/>

	<xsl:template match="@type[.='BOOLEAN']" mode="ForDataType">
		<xsl:text>CHARACTER</xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:text>1</xsl:text>
		<xsl:value-of select="$RightParen"/>
		<xsl:text> FOR BIT DATA</xsl:text>
	</xsl:template>

	<xsl:template match="ddt:booleanLiteral">
		<xsl:choose>
			<xsl:when test="@value='TRUE'">
				<xsl:text>X'01'</xsl:text>
			</xsl:when>
			<xsl:when test="@value='FALSE'">
				<xsl:text>X'00'</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>NULL</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="dms:commitStatement">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>COMMIT</xsl:text>
		<xsl:value-of select="$StatementDelimeter"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

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

	<xsl:template match="dep:charLengthExpression">
		<xsl:text>LENGTH</xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<!-- This commentput here by Cle' for testing.
	<!- UNDONE: This isn't going to work for ddl:tableConstraintDefinition elements that are not inside of ddl:tableDefinition elements ->
	<xsl:template match="ddl:tableConstraintDefinition[child::ddl:uniqueConstraintDefinition]kk">
		<xsl:param name="indent"/>
		<xsl:value-of select="$indent"/>
		<xsl:choose>
			<xsl:when test="parent::ddl:tableDefinition/ddl:columnDefinition[@name=current()/ddl:uniqueConstraintDefinition/dep:simpleColumnReference/@name and not(ddl:columnConstraintDefinition/ddl:notNullKeyword)]">
				<!- Rather than just absorb the constraints, triggers need to be generated. ->
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates/>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:if test="not(position()=last())">
			<xsl:choose>
				<xsl:when test="following-sibling::ddl:tableConstraintDefinition[child::ddl:uniqueConstraintDefinition]">
					<xsl:call-template name="ColumnLineTerminator">
						<xsl:with-param name="tableConstraint" select="following-sibling::ddl:tableConstraintDefinition"/>
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>, </xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template name="ColumnLineTerminator">
		<xsl:param name="tableConstraint"/>
		<xsl:for-each select="$tableConstraint/ddl:tableConstraintDefinition">
			<xsl:choose>
				<xsl:when test="parent::ddl:tableDefinition/ddl:columnDefinition[@name=current()/ddl:uniqueConstraintDefinition/dep:simpleColumnReference/@name and not(ddl:columnConstraintDefinition/ddl:notNullKeyword)]">
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>, </xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
	</xsl:template> -->


</xsl:stylesheet>
