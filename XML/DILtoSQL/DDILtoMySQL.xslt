<?xml version="1.0" encoding="utf-8"?>
<!--
	Neumont Object-Role Modeling Architect for Visual Studio

	Copyright © Neumont University. All rights reserved.

	The use and distribution terms for this software are covered by the
	Common Public License 1.0 (http://opensource.org/licenses/cpl) which
	can be found in the file CPL.txt at the root of this distribution.
	By using this software in any fashion, you are agreeing to be bound by
	the terms of this license.

	You must not remove this notice, or any other, from this software.
-->
<!-- Contributors: Joshua Arnold, Kevin M. Owen -->
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:exsl="http://exslt.org/common"
	xmlns:dil="http://schemas.orm.net/DIL/DIL"
	xmlns:ddt="http://schemas.orm.net/DIL/DILDT"
	xmlns:dep="http://schemas.orm.net/DIL/DILEP"
	xmlns:dms="http://schemas.orm.net/DIL/DILMS"
	xmlns:dml="http://schemas.orm.net/DIL/DMIL"
	xmlns:ddl="http://schemas.orm.net/DIL/DDIL"
	extension-element-prefixes="exsl"
	exclude-result-prefixes="dil ddt dep dms dml ddl">

	<xsl:import href="DDILtoSQLStandard.xslt"/>
	<xsl:import href="TruthValueTestRemover.xslt"/>
	<xsl:import href="DomainInliner.xslt"/>

	<xsl:output method="text" encoding="utf-8" indent="no" omit-xml-declaration="yes"/>
	<xsl:strip-space elements="*"/>

	<!-- This should be set to the maximum possible number of code points that it could take to encode any Unicode character in the encoding being used. -->
	<!-- This will be 4 for UTF-8 (the default), 2 for UTF-16, and 1 for UTF-32. -->
	<xsl:param name="MySQL.CharacterStringLengthDivisor" select="4"/>

	<xsl:variable name="MySQL.TinyLength" select="255"/>
	<xsl:variable name="MySQL.NormalLength" select="65535"/>
	<xsl:variable name="MySQL.MediumLength" select="16777215"/>
	<xsl:variable name="MySQL.LongLength" select="4294967295"/>

	<xsl:param name="MaximumCharacterNonVaryingStringLength" select="floor($MySQL.TinyLength div $MySQL.CharacterStringLengthDivisor)"/>
	<xsl:param name="MaximumBinaryNonVaryingStringLength" select="$MySQL.TinyLength"/>
	<xsl:param name="MaximumCharacterVaryingStringLength" select="floor($MySQL.NormalLength div $MySQL.CharacterStringLengthDivisor)"/>
	<xsl:param name="MaximumBinaryVaryingStringLength" select="$MySQL.NormalLength"/>
	<xsl:param name="MaximumCharacterLargeObjectStringLength" select="floor($MySQL.LongLength div $MySQL.CharacterStringLengthDivisor)"/>
	<xsl:param name="MaximumBinaryLargeObjectStringLength" select="$MySQL.LongLength"/>

	<xsl:template match="/">
		<xsl:variable name="truthValueTestRemovedDilFragment">
			<xsl:apply-templates mode="TruthValueTestRemover" select="."/>
		</xsl:variable>
		<xsl:variable name="domainInlinedDilFragment">
			<xsl:apply-templates mode="DomainInliner" select="exsl:node-set($truthValueTestRemovedDilFragment)"/>
		</xsl:variable>
		<xsl:apply-templates select="exsl:node-set($domainInlinedDilFragment)/child::*"/>
	</xsl:template>

	<!-- 
		The current version of MySQL (5.1.12) does not support Schemas or Catalogs.
		The following templates are overridden to take no action for schema/catalog-related operations.
	-->
	<xsl:template match="@catalog" mode="ForSchemaQualifiedName"/>
	<xsl:template match="@schema" mode="ForSchemaQualifiedName"/>
	<xsl:template match="ddl:schemaDefinition"/>
	<xsl:template match="@defaultCharacterSet" mode="ForSchemaDefinition"/>
	<xsl:template match="dms:setSchemaStatement"/>
	
	<!--
		Turn SQL Standard delimited identifiers (delimited by ") into MySQL delimited identifiers (delimited by `).
		UNDONE: We should look into setting sql_mode='ANSI_QUOTES', which would seem to let us avoid doing this.
	-->
	<xsl:template name="RenderIdentifier">
		<xsl:param name="name" select="."/>
		<xsl:call-template name="ConvertToMySQLQuotation">
			<xsl:with-param name="name" select="$name"/>
		</xsl:call-template>
	</xsl:template>

	<!-- 
		The current version of MySQL (5.1.12) does not support Transactions.
		The following templates are overridden to take no action for Transaction-related operations.
	-->
	<xsl:template match="dms:startTransactionStatement"/>
	<xsl:template match="dms:commitStatement"/>
	<!-- End Transaction template overrides -->

	<!-- Template override to correctly display the Identity specification in MySQL -> "AUTO_INCREMENT" -->
	<xsl:template match="ddl:identityColumnSpecification">
		<xsl:text> AUTO_INCREMENT</xsl:text>
	</xsl:template>

	<xsl:template match="ddl:generationClause"/>
	<xsl:template match="@scope" mode="ForTableDefinition"/>

	<!--
		For MySQL, "the CHECK clause is parsed but ignored by all storage engines." Therefore, we won't even bother rendering it.
		UNDONE: Figure out a way to enforce these. They might be able to be turned into triggers instead of check constraints.
	-->
	<xsl:template match="ddl:columnConstraintDefinition[ddl:checkConstraintDefinition]"/>
	<xsl:template match="ddl:tableConstraintDefinition[ddl:checkConstraintDefinition]"/>

	<!-- Template override to generate a MySQL Stored Procedure declaration -->
	<xsl:template match="ddl:sqlInvokedProcedure"/>
	<!-- Template override to generate a MySQL Stored Procedure defintion -->
	<xsl:template match="ddl:sqlRoutineSpec">
		<xsl:value-of select="$IndentChar" />
		<xsl:apply-templates select="child::*" />
	</xsl:template>

	<!-- 
		Template override to generate a MySQL Stored Procedure parameter declarations. 
		The current version of MySQL (5.1.12) does not support decorated procedure variables and must be decorated properly if they are the same as the column names.
	-->
	<xsl:template match="ddl:sqlParameterDeclaration">
		<xsl:value-of select="$IndentChar" />
		<xsl:text>var</xsl:text>
		<xsl:value-of select="@name"/>
		<xsl:text> </xsl:text>
		<xsl:apply-templates select="child::*" />
		<xsl:if test="not(position()=last())">
			<xsl:text>, </xsl:text>
		</xsl:if>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>
	<!-- 
		Template override to generate a MySQL Stored Procedure parameter references. 
		The current version of MySQL (5.1.12) does not support decorated procedure variables and must be decorated properly if they are the same as the column names.
	-->
	<xsl:template match="dep:sqlParameterReference">
		<xsl:text>var</xsl:text>
		<xsl:value-of select="@name"/>
		<xsl:if test="not(position()=last())">
			<xsl:text>, </xsl:text>
		</xsl:if>
	</xsl:template>
	<xsl:template match="ddl:atomicBlock"/>


	<!-- Templates to convert SQL Standard Data Types to MySQL-friendly (5.1.12) data types -->
	<xsl:template match="@type[.='TINYINT']" mode="ForDataType">
		<xsl:text>TINYINT UNSIGNED</xsl:text>
	</xsl:template>
	<xsl:template match="@type[.='INTEGER']" mode="ForDataType">
		<xsl:text>INT</xsl:text>
	</xsl:template>
	<xsl:template match="@type[.='TIMESTAMP']" mode="ForDataType">
		<!-- Although MySQL has a TIMESTAMP type, their DATETIME is closer to the SQL Standard's TIMESTAMP. -->
		<xsl:text>DATETIME</xsl:text>
	</xsl:template>
	<xsl:template match="@type[.='CHARACTER']" mode="ForDataType">
		<xsl:text>CHAR</xsl:text>
	</xsl:template>
	<xsl:template match="@type[.='CHARACTER VARYING']" mode="ForDataType">
		<xsl:text>VARCHAR</xsl:text>
	</xsl:template>
	<xsl:template match="@type[.='BINARY VARYING']" mode="ForDataType">
		<xsl:text>VARBINARY</xsl:text>
	</xsl:template>
	<xsl:template match="@type[.='CHARACTER LARGE OBJECT']" mode="ForDataType">
		<xsl:variable name="lengthFragment">
			<xsl:apply-templates select=".." mode="GetTotalDataTypeLength"/>
		</xsl:variable>
		<xsl:variable name="length" select="number($lengthFragment)"/>
		<xsl:choose>
			<xsl:when test="$length &lt;= ($MySQL.TinyLength div $MySQL.CharacterStringLengthDivisor)">
				<xsl:text>TINYTEXT</xsl:text>
			</xsl:when>
			<xsl:when test="$length &lt;= ($MySQL.NormalLength div $MySQL.CharacterStringLengthDivisor)">
				<xsl:text>TEXT</xsl:text>
			</xsl:when>
			<xsl:when test="$length &lt;= ($MySQL.MediumLength div $MySQL.CharacterStringLengthDivisor)">
				<xsl:text>MEDIUMTEXT</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>LONGTEXT</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="@type[.='BINARY LARGE OBJECT']" mode="ForDataType">
		<xsl:variable name="lengthFragment">
			<xsl:apply-templates select=".." mode="GetTotalDataTypeLength"/>
		</xsl:variable>
		<xsl:variable name="length" select="number($lengthFragment)"/>
		<xsl:choose>
			<xsl:when test="$length &lt;= $MySQL.TinyLength">
				<xsl:text>TINYBLOB</xsl:text>
			</xsl:when>
			<xsl:when test="$length &lt;= $MySQL.NormalLength">
				<xsl:text>BLOB</xsl:text>
			</xsl:when>
			<xsl:when test="$length &lt;= $MySQL.MediumLength">
				<xsl:text>MEDIUMBLOB</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>LONGBLOB</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="@type[.='BOOLEAN']" mode="ForDataType">
		<xsl:text>BIT</xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:text>1</xsl:text>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="ddt:characterString | ddt:binaryString" mode="ForDataTypeLengthWithMultiplier">
		<xsl:call-template name="GetTotalDataTypeLength"/>
	</xsl:template>

	<xsl:template match="ddt:booleanLiteral">
		<xsl:choose>
			<xsl:when test="@value='TRUE'">
				<xsl:text>b'1'</xsl:text>
			</xsl:when>
			<xsl:when test="@value='FALSE'">
				<xsl:text>b'0'</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>NULL</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


	<xsl:template name="ConvertToMySQLQuotation">
		<xsl:param name="name"/>
		<xsl:call-template name="globalReplace">
			<xsl:with-param name="outputString" select="$name"/>
			<xsl:with-param name="target" select="'&quot;'"/>
			<xsl:with-param name="replacement" select="'`'"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="globalReplace">
		<xsl:param name="outputString"/>
		<xsl:param name="target"/>
		<xsl:param name="replacement"/>
		<xsl:choose>
			<xsl:when test="contains($outputString,$target)">
				<xsl:value-of select="concat(substring-before($outputString,$target), $replacement)"/>
				<xsl:call-template name="globalReplace">
					<xsl:with-param name="outputString" 
						 select="substring-after($outputString,$target)"/>
					<xsl:with-param name="target" select="$target"/>
					<xsl:with-param name="replacement" 
						 select="$replacement"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$outputString"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>