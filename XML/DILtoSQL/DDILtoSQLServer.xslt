<?xml version="1.0" encoding="utf-8"?>
<!--
	Copyright © Neumont University. All rights reserved.

	This software is provided 'as-is', without any express or implied warranty. In no event will the authors be held liable for any damages arising from the use of this software.
	Permission is granted to anyone to use this software for any purpose, including commercial applications, and to alter it and redistribute it freely, subject to the following restrictions:
	1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
	2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
	3. This notice may not be removed or altered from any source distribution.
-->
<!-- Contributors: Corey Kaylor, Kevin M. Owen, Clé Diggins, Robert Moore -->
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
	xmlns:dpp="urn:schemas-orm-net:DIL:Preprocessor"
	extension-element-prefixes="exsl dsf"
	exclude-result-prefixes="dil ddt dep dms dml ddl dpp">

	<xsl:import href="DDILtoSQLStandard.xslt"/>
	<xsl:import href="TruthValueTestRemover.xslt"/>
	<xsl:import href="DomainInliner.xslt"/>
	<xsl:import href="UniqueNullableOutliner.xslt"/>

	<xsl:output method="text" encoding="utf-8" indent="no" omit-xml-declaration="yes"/>
	<xsl:strip-space elements="*"/>

	<xsl:param name="DefaultMaximumStringLength" select="'max'"/>
	<xsl:param name="MaximumCharacterNonVaryingStringLength" select="4000"/>
	<xsl:param name="MaximumBinaryNonVaryingStringLength" select="8000"/>
	<xsl:param name="DefaultMaximumExactNumericPrecisionAndScale" select="38"/>
	<xsl:param name="DefaultMaximumApproximateNumericPrecision" select="''"/>

	<xsl:param name="StatementDelimiter">
		<xsl:value-of select="$NewLine"/>
		<xsl:text>GO</xsl:text>
		<xsl:value-of select="$NewLine"/>
	</xsl:param>
	<xsl:param name="StatementBlockDelimeter">
		<xsl:value-of select="$NewLine"/>
		<xsl:text>GO</xsl:text>
	</xsl:param>

	<xsl:template match="/">
		<xsl:variable name="truthValueTestRemovedDilFragment">
			<xsl:apply-templates mode="TruthValueTestRemover" select="."/>
		</xsl:variable>
		<xsl:variable name="domainInlinedDilFragment">
			<xsl:apply-templates mode="DomainInliner" select="exsl:node-set($truthValueTestRemovedDilFragment)"/>
		</xsl:variable>
		<xsl:variable name="uniqueNullableOutlinedDilFragment">
			<xsl:apply-templates mode="UniqueNullableOutliner" select="exsl:node-set($domainInlinedDilFragment)"/>
		</xsl:variable>
		<xsl:apply-templates select="exsl:node-set($uniqueNullableOutlinedDilFragment)/child::*"/>
	</xsl:template>

	<xsl:template match="ddl:schemaDefinition">
		<xsl:param name="indent"/>
		<xsl:text>CREATE SCHEMA </xsl:text>
		<xsl:apply-templates select="@catalogName" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="@schemaName" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="@authorizationIdentifier" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="@defaultCharacterSet" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="ddl:path" mode="ForSchemaDefinition"/>
		<xsl:value-of select="$StatementDelimiter"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:text>GO</xsl:text>
		<xsl:value-of select="$NewLine"/>
		<xsl:call-template name="writeOutStartTrans" />
		<xsl:value-of select="$NewLine"/>
		<xsl:apply-templates>
			<xsl:with-param name="indent" select="concat($indent, $IndentChar)"/>
		</xsl:apply-templates>
	</xsl:template>

	<!-- View definition and related templates. -->
	
	<xsl:template match="ddl:viewDefinition[@dpp:isUniqueNullableView = 'true' or @dpp:isUniqueNullableView = 1]">
		<xsl:param name="indent"/>
		<xsl:apply-imports/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>CREATE UNIQUE CLUSTERED INDEX </xsl:text>
		<xsl:value-of select="dsf:makeValidIdentifier(concat(dsf:unescapeIdentifier(@name),'Index'))"/>
		<xsl:text> ON </xsl:text>
		<xsl:apply-templates select="@catalog" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@schema" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@name" mode="ForSchemaQualifiedName"/>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates select="dep:columnNameDefinition"/>
		<xsl:value-of select="$RightParen"/>
		<xsl:value-of select="$StatementDelimiter"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<xsl:template match="ddl:viewDefinition" mode="ForViewDefinitionInBetweenColumnListAndQueryExpression">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>WITH SCHEMABINDING</xsl:text>
	</xsl:template>

	<xsl:template match="@recursive" mode="ForViewDefinition"/>

	<xsl:template match="@checkOption" mode="ForViewDefinition">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>WITH CHECK OPTION</xsl:text>
	</xsl:template>

	<!-- End view definition and related templates. -->
	
	
	<xsl:template match="dms:startTransactionStatement"/>
	
	<xsl:template match="dms:startTransactionStatement" mode="writeOut" name="writeOutStartTrans">
		<!-- Atomicity has been removed becuase multiple sprocs can't be in transaction in t-sql.-->
	</xsl:template>
	
	<xsl:template match="dms:commitStatement">
		<xsl:value-of select="$StatementBlockDelimeter"/>
	</xsl:template>

	<xsl:template match="@defaultCharacterSet" mode="ForSchemaDefinition"/>

	<xsl:template match="dms:setSchemaStatement"/>

	<xsl:template match="ddl:identityColumnSpecification">
		<xsl:text> IDENTITY </xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates select="ddl:sequenceGeneratorStartWithOption"/>
		<xsl:if test="not(ddl:sequenceGeneratorStartWithOption)">
			<xsl:text>1</xsl:text>
		</xsl:if>
		<xsl:text>, </xsl:text>
		<xsl:apply-templates select="ddl:sequenceGeneratorIncrementByOption"/>
		<xsl:if test="not(ddl:sequenceGeneratorIncrementByOption)">
			<xsl:text>1</xsl:text>
		</xsl:if>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="ddl:sequenceGeneratorStartWithOption">
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="ddl:sequenceGeneratorIncrementByOption">
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="ddt:characterString | ddt:binaryString" mode="ForDataTypeLengthWithMultiplier">
		<xsl:call-template name="GetTotalDataTypeLength"/>
	</xsl:template>

	<!-- UNDONE: Handle rendering ddt:characterString and ddt:binaryString with @length greater than 4000 and 8000, respectively.-->
	<!-- In order to do this, we will need to render the length as MAX, and add constraints to enforce the original length requested. -->
	
	<xsl:template match="@type[.='CHARACTER']" mode="ForDataType">
		<xsl:text>nchar</xsl:text>
	</xsl:template>

	<xsl:template match="@type[.='CHARACTER VARYING' or .='CHARACTER LARGE OBJECT']" mode="ForDataType">
		<xsl:text>nvarchar</xsl:text>
	</xsl:template>

	<xsl:template match="@type[.='BINARY']" mode="ForDataType">
		<xsl:text>binary</xsl:text>
	</xsl:template>

	<xsl:template match="@type[.='BINARY VARYING' or .='BINARY LARGE OBJECT']" mode="ForDataType">
		<xsl:text>varbinary</xsl:text>
	</xsl:template>

	<xsl:template match="@type[.='DATE']" mode="ForDataType">
		<xsl:text>date</xsl:text>
	</xsl:template>

	<xsl:template match="@type[.='DATETIME']" mode="ForDataType">
		<xsl:text>datetime</xsl:text>
	</xsl:template>

	<xsl:template match="@type[.='TIME']" mode="ForDataType">
		<xsl:text>time</xsl:text>
	</xsl:template>

	<xsl:template match="@type[.='TIMESTAMP']" mode="ForDataType">
		<xsl:text>rowversion</xsl:text>
	</xsl:template>

	<xsl:template match="@type[.='BOOLEAN']" mode="ForDataType">
		<xsl:text>bit</xsl:text>
	</xsl:template>

	<xsl:template match="@type[.='NUMERIC']" mode="ForDataType">
		<xsl:text>numeric</xsl:text>
	</xsl:template>

	<xsl:template match="@type[.='DECIMAL']" mode="ForDataType">
		<xsl:text>decimal</xsl:text>
	</xsl:template>

	<xsl:template match="@type[.='TINYINT']" mode="ForDataType">
		<xsl:text>tinyint</xsl:text>
	</xsl:template>

	<xsl:template match="@type[.='SMALLINT']" mode="ForDataType">
		<xsl:text>smallint</xsl:text>
	</xsl:template>

	<xsl:template match="@type[.='INTEGER']" mode="ForDataType">
		<xsl:text>int</xsl:text>
	</xsl:template>

	<xsl:template match="@type[.='BIGINT']" mode="ForDataType">
		<xsl:text>bigint</xsl:text>
	</xsl:template>

	<xsl:template match="@type[.='REAL']" mode="ForDataType">
		<xsl:text>real</xsl:text>
	</xsl:template>
	<xsl:template match="@type[.='DOUBLE PRECISION'or .='FLOAT']" mode="ForDataType">
		<xsl:text>float</xsl:text>
	</xsl:template>
	<xsl:template match="ddt:approximateNumeric[@type='DOUBLE PRECISION']" mode="ForDataTypeNumericPrecisionAndScale">
		<xsl:value-of select="$LeftParen"/>
		<xsl:text>53</xsl:text>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>


	<xsl:template match="ddt:booleanLiteral">
		<xsl:choose>
			<xsl:when test="@value='TRUE'">
				<xsl:text>1</xsl:text>
			</xsl:when>
			<xsl:when test="@value='FALSE'">
				<xsl:text>0</xsl:text>
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
	
	<xsl:template match="ddt:binaryStringLiteral">
		<xsl:text>0x</xsl:text>
		<xsl:value-of select="@value"/>
	</xsl:template>

	<xsl:template match="dep:charLengthExpression">
		<xsl:text>LEN</xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="@lengthUnits"/>

	<xsl:template match="ddl:sqlParameterDeclaration">
		<xsl:value-of select="$IndentChar" />
		<xsl:value-of select="'@'"/>
		<xsl:value-of select="@name"/>
		<xsl:text> </xsl:text>
		<xsl:apply-templates select="child::*" />
		<xsl:if test="not(position()=last())">
			<xsl:text>, </xsl:text>
		</xsl:if>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<xsl:template match="dep:sqlParameterReference">
		<xsl:value-of select="'@'"/>
		<xsl:value-of select="@name"/>
		<xsl:if test="not(position()=last())">
			<xsl:text>, </xsl:text>
		</xsl:if>
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

</xsl:stylesheet>
