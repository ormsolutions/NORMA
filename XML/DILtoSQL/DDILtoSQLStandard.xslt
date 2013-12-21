<?xml version="1.0" encoding="utf-8"?>
<!--
	Copyright © Neumont University. All rights reserved.

	This software is provided 'as-is', without any express or implied warranty. In no event will the authors be held liable for any damages arising from the use of this software.
	Permission is granted to anyone to use this software for any purpose, including commercial applications, and to alter it and redistribute it freely, subject to the following restrictions:
	1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
	2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
	3. This notice may not be removed or altered from any source distribution.
-->
<!-- Contributors: Corey Kaylor, Kevin M. Owen, Robert Moore -->
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

	<!--
		NOTE: Although the DILSupportFunctions are not currently used directly in this transform, they are imported here so that they
		can be used by the various target-specific renderers and preprocessing transforms without causing duplicate definitions.
	-->
	<xsl:import href="DILSupportFunctions.xslt"/>

	<xsl:output method="text" encoding="utf-8" indent="no" omit-xml-declaration="yes"/>
	<xsl:strip-space elements="*"/>

	<xsl:param name="Period" select="'.'"/>
	<xsl:param name="NewLine" select="'&#x0D;&#x0A;'"/>
	<xsl:param name="IndentChar" select="'&#x09;'"/>
	<xsl:param name="StatementStartBracket" select="'('"/>
	<xsl:param name="StatementEndBracket" select="')'"/>
	<xsl:param name="LeftParen" select="'('"/>
	<xsl:param name="RightParen" select="')'"/>
	<xsl:param name="ConcatenationOperator" select="'||'"/>
	<xsl:param name="StatementDelimiter" select="';'"/>
	<xsl:param name="SetClauseEqualsOperator" select="'='"/>
	<xsl:param name="StatementBlockDelimeter" select="''"/>

	<!-- The DefaultMaximum* parameters are provided to allow derived stylesheets to initialize multiple Maximum* parameters at once. -->
	<!-- They should be referenced only from the default values for these parameters. Everything else should use the specific Maximum* parameters. -->

	<xsl:param name="DefaultMaximumStringLength"/>
	<xsl:param name="DefaultMaximumNonVaryingStringLength" select="$DefaultMaximumStringLength"/>
	<xsl:param name="MaximumCharacterNonVaryingStringLength" select="$DefaultMaximumNonVaryingStringLength"/>
	<xsl:param name="MaximumBinaryNonVaryingStringLength" select="$DefaultMaximumNonVaryingStringLength"/>
	<xsl:param name="DefaultMaximumVaryingStringLength" select="$DefaultMaximumStringLength"/>
	<xsl:param name="MaximumCharacterVaryingStringLength" select="$DefaultMaximumVaryingStringLength"/>
	<xsl:param name="MaximumBinaryVaryingStringLength" select="$DefaultMaximumVaryingStringLength"/>
	<xsl:param name="DefaultMaximumLargeObjectStringLength" select="$DefaultMaximumStringLength"/>
	<xsl:param name="MaximumCharacterLargeObjectStringLength" select="$DefaultMaximumLargeObjectStringLength"/>
	<xsl:param name="MaximumBinaryLargeObjectStringLength" select="$DefaultMaximumLargeObjectStringLength"/>

	<xsl:param name="DefaultMaximumExactNumericPrecisionAndScale"/>
	<xsl:param name="DefaultMaximumExactNumericPrecision" select="$DefaultMaximumExactNumericPrecisionAndScale"/>
	<xsl:param name="DefaultMaximumExactNumericPrecisionWithoutScale" select="$DefaultMaximumExactNumericPrecision"/>
	<xsl:param name="MaximumNumericExactNumericPrecisionWithoutScale" select="$DefaultMaximumExactNumericPrecisionWithoutScale"/>
	<xsl:param name="MaximumDecimalExactNumericPrecisionWithoutScale" select="$DefaultMaximumExactNumericPrecisionWithoutScale"/>
	<xsl:param name="DefaultMaximumExactNumericPrecisionWithScale" select="$DefaultMaximumExactNumericPrecision"/>
	<xsl:param name="MaximumNumericExactNumericPrecisionWithScale" select="$DefaultMaximumExactNumericPrecisionWithScale"/>
	<xsl:param name="MaximumDecimalExactNumericPrecisionWithScale" select="$DefaultMaximumExactNumericPrecisionWithScale"/>
	<xsl:param name="DefaultMaximumExactNumericScale" select="$DefaultMaximumExactNumericPrecisionAndScale"/>
	<xsl:param name="MaximumNumericExactNumericScale" select="$DefaultMaximumExactNumericScale"/>
	<xsl:param name="MaximumDecimalExactNumericScale" select="$DefaultMaximumExactNumericScale"/>

	<xsl:param name="DefaultMaximumApproximateNumericPrecision"/>
	<xsl:param name="MaximumFloatApproximateNumericPrecision" select="$DefaultMaximumApproximateNumericPrecision"/>

	<!--
		UNDONE: Only statements rendered directly inside of dil:root and ddl:atomicBlock should be followed by statement delimiters.
		However, until the other templates have been adjusted to not render statement delimiters, this is commented out.
	-->
	<!--<xsl:template match="dil:root">
		<xsl:param name="indent"/>
		<xsl:for-each select="child::*">
			<xsl:apply-templates select=".">
				<xsl:with-param name="indent" select="$indent"/>
			</xsl:apply-templates>
			<xsl:value-of select="$StatementDelimiter"/>
			<xsl:value-of select="$NewLine"/>
		</xsl:for-each>
	</xsl:template>-->


	<!-- Schema Definition pg.519 -->

	<xsl:template match="ddl:schemaDefinition">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>CREATE SCHEMA </xsl:text>
		<xsl:apply-templates select="@catalogName" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="@schemaName" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="@authorizationIdentifier" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="@defaultCharacterSet" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="ddl:path" mode="ForSchemaDefinition"/>
		<xsl:if test="*[not(self::ddl:path)]">
			<xsl:value-of select="$NewLine"/>
			<xsl:apply-templates select="*[not(self::ddl:path)]">
				<xsl:with-param name="indent" select="concat($indent, $IndentChar)"/>
			</xsl:apply-templates>
		</xsl:if>
		<xsl:value-of select="$StatementDelimiter"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<xsl:template match="@catalogName" mode="ForSchemaDefinition">
		<xsl:call-template name="RenderIdentifier"/>
		<xsl:value-of select="$Period"/>
	</xsl:template>

	<xsl:template match="@schemaName" mode="ForSchemaDefinition">
		<xsl:call-template name="RenderIdentifier"/>
	</xsl:template>

	<xsl:template match="@authorizationIdentifier" mode="ForSchemaDefinition">
		<xsl:text> AUTHORIZATION </xsl:text>
		<xsl:value-of select="."/>
	</xsl:template>

	<xsl:template match="@defaultCharacterSet" mode="ForSchemaDefinition">
		<xsl:text> DEFAULT CHARACTER SET </xsl:text>
		<xsl:value-of select="."/>
	</xsl:template>

	<xsl:template match="ddl:path" mode="ForSchemaDefinition">
		<xsl:text> PATH </xsl:text>
		<xsl:for-each select="ddl:schema">
			<xsl:apply-templates select="@catalogName" mode="ForSchemaDefinition"/>
			<xsl:apply-templates select="@schemaName" mode="ForSchemaDefinition"/>
			<xsl:if test="not(position()=last())">
				<xsl:text>, </xsl:text>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="dms:setSchemaStatement">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>SET SCHEMA </xsl:text>
		<xsl:apply-templates/>
		<xsl:value-of select="$StatementDelimiter"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<xsl:template match="dms:commitStatement">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>COMMIT WORK</xsl:text>
		<xsl:if test="string(@type)">
			<xsl:text> AND </xsl:text>
			<xsl:value-of select="@type"/>
		</xsl:if>
		<xsl:value-of select="$StatementDelimiter"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<!-- End of Schema Definition -->

	<!-- Table Definition pg.525 -->

	<xsl:template match="ddl:tableDefinition">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>CREATE </xsl:text>
		<xsl:apply-templates select="@scope" mode="ForTableDefinition"/>
		<xsl:text>TABLE </xsl:text>
		<xsl:apply-templates select="@catalog" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@schema" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@name" mode="ForSchemaQualifiedName"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:value-of select="$StatementStartBracket"/>
		<xsl:apply-templates select="ddl:columnDefinition">
			<xsl:with-param name="indent" select="concat($indent, $IndentChar)"/>
		</xsl:apply-templates>
		<xsl:apply-templates select="ddl:tableConstraintDefinition">
			<xsl:with-param name="indent" select="concat($indent, $IndentChar)"/>
		</xsl:apply-templates>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:value-of select="$StatementEndBracket"/>
		<xsl:value-of select="$StatementDelimiter"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<xsl:template match="@scope" mode="ForTableDefinition">
		<xsl:value-of select="."/>
		<xsl:text> </xsl:text>
	</xsl:template>

	<xsl:template match="@catalog" mode="ForSchemaQualifiedName">
		<xsl:call-template name="RenderIdentifier"/>
		<xsl:value-of select="$Period"/>
	</xsl:template>

	<xsl:template match="@schema" mode="ForSchemaQualifiedName">
		<xsl:call-template name="RenderIdentifier"/>
		<xsl:value-of select="$Period"/>
	</xsl:template>

	<xsl:template match="@name" mode="ForSchemaQualifiedName">
		<xsl:call-template name="RenderIdentifier"/>
	</xsl:template>

	<!--End of Table Definition -->

	<!-- View Definition pg. #590 -->

	<xsl:template match="ddl:viewDefinition">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>CREATE </xsl:text>
		<xsl:apply-templates select="@recursive" mode="ForViewDefinition"/>
		<xsl:text>VIEW </xsl:text>
		<xsl:apply-templates select="@catalog" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@schema" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@name" mode="ForSchemaQualifiedName"/>
		<xsl:if test="dep:columnNameDefinition">
			<xsl:text> </xsl:text>
			<xsl:value-of select="$LeftParen"/>
			<xsl:apply-templates select="dep:columnNameDefinition"/>
			<xsl:value-of select="$RightParen"/>
		</xsl:if>
		<xsl:apply-templates select="." mode="ForViewDefinitionInBetweenColumnListAndQueryExpression">
			<xsl:with-param name="indent" select="$indent"/>
		</xsl:apply-templates>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>AS</xsl:text>
		<!-- Applys queryExpression template -->
		<xsl:apply-templates select="*[not(self::dep:columnNameDefinition)]">
			<xsl:with-param name="indent" select="concat($indent, $IndentChar)"/>
		</xsl:apply-templates>
		<xsl:apply-templates select="@checkOption">
			<xsl:with-param name="indent" select="$indent"/>
		</xsl:apply-templates>
		<xsl:value-of select="$StatementDelimiter"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<xsl:template match="@recursive" mode="ForViewDefinition">
		<xsl:if test=".='true' or .=1">
			<xsl:text>RECURSIVE </xsl:text>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="@checkOption" mode="ForViewDefinition">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:value-of select="."/>
	</xsl:template>

	<!-- End of View Definition -->

	<!-- Query Expression pg. #351 -->

	<xsl:template match="dml:withClauseQuery">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>WITH </xsl:text>
		<xsl:if test="@recursive = 'true' or @recrusive = 1">
			<xsl:text>RECURSIVE </xsl:text>
		</xsl:if>
		<xsl:apply-templates>
			<xsl:with-param name="indent" select="$indent"/>
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="dml:withListElement">
		<xsl:param name="indent"/>
		<xsl:value-of select="@queryName"/>
		<xsl:text> </xsl:text>
		<xsl:if test="dep:columnNameDefinition">
			<xsl:value-of select="$LeftParen"/>
			<xsl:apply-templates select="dep:columnNameDefinition"/>
			<xsl:value-of select="$RightParen"/>
			<xsl:text> </xsl:text>
		</xsl:if>
		<xsl:text>AS </xsl:text>
		<xsl:value-of select="$NewLine"/>
		<xsl:apply-templates select="*[not(self::dep:columnNameDefinition)]">
			<xsl:with-param name="indent" select="concat($indent, $IndentChar)"/>
		</xsl:apply-templates>
	</xsl:template>

	<!-- TODO - Templates for SearchClause and CycleClause -->

	<xsl:template match="dml:searchClause">
		<!--<xsl:text>SEARCH </xsl:text>
		-->
		<!-- Recursive Search Order -->
		<!--
		<xsl:text> SET </xsl:text>
		-->
		<!-- sequence column -->
		<xsl:text> ***CURRENTLY UNDONE*** </xsl:text>
	</xsl:template>

	<xsl:template match="dml:cycleClause">
		<xsl:text> ***CURRENTLY UNDONE*** </xsl:text>
	</xsl:template>

	<!-- End of Query Expression -->

	<!-- Table Primary -->

	<xsl:template match="dml:tableName | dml:queryName | dml:onlySpec">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:if test="self::dml:onlySpec">
			<xsl:text>ONLY </xsl:text>
		</xsl:if>
		<xsl:apply-templates select="@catalog" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@schema" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@name" mode="ForSchemaQualifiedName"/>
		<xsl:if test="dml:correlation">
			<xsl:text> </xsl:text>
			<xsl:apply-templates select="dml:correlation"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="dml:correlation">
		<xsl:text>AS </xsl:text>
		<xsl:apply-templates select="@name" mode="ForCorrelationName"/>
		<xsl:if test="dep:columnNameDefinition">
			<xsl:text> </xsl:text>
			<xsl:value-of select="$LeftParen"/>
			<xsl:apply-templates select="dep:columnNameDefinition"/>
			<xsl:value-of select="$RightParen"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="@name" mode="ForCorrelationName">
		<xsl:call-template name="RenderIdentifier"/>
	</xsl:template>

	<xsl:template match="@correlationName">
		<xsl:text> AS </xsl:text>
		<xsl:call-template name="RenderIdentifier"/>
	</xsl:template>

	<xsl:template match="dml:derivedTable | dml:lateralDerivedTable">
		<xsl:param name="indent"/>
		<xsl:if test="self::dml:lateralDerivedTable">
			<xsl:text>LATERAL</xsl:text>
		</xsl:if>
		<xsl:apply-templates select="*[not(self::dml:correlation)]">
			<xsl:with-param name="indent" select="$indent"/>
		</xsl:apply-templates>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:apply-templates select="dml:correlation"/>
	</xsl:template>

	<!-- End Table Primary -->

	<!-- Join Specifications -->

	<xsl:template match="dml:qualifiedJoin | dml:crossJoin | dml:naturalJoin">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="substring-after($indent, $IndentChar)"/>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates select="child::*[1]">
			<xsl:with-param name="indent" select="concat($indent, $IndentChar)"/>
		</xsl:apply-templates>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:choose>
			<xsl:when test="self::dml:crossJoin">
				<xsl:text>CROSS </xsl:text>
			</xsl:when>
			<xsl:when test="self::dml:naturalJoin">
				<xsl:text>NATURAL </xsl:text>
			</xsl:when>
		</xsl:choose>
		<xsl:apply-templates select="@joinType"/>
		<xsl:text>JOIN</xsl:text>
		<xsl:apply-templates select="child::*[2]">
			<xsl:with-param name="indent" select="concat($indent, $IndentChar)"/>
		</xsl:apply-templates>
		<xsl:apply-templates select="child::*[3]">
			<xsl:with-param name="indent" select="$indent"/>
		</xsl:apply-templates>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="substring-after($indent, $IndentChar)"/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="dml:joinCondition">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>ON </xsl:text>
		<xsl:apply-templates>
			<xsl:with-param name="indent" select="concat($indent, $IndentChar)"/>
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="@joinType">
		<xsl:value-of select="."/>
		<xsl:text> </xsl:text>
	</xsl:template>

	<!-- End Join specifications-->

	<!-- Query Specificiation pg. #341 -->

	<xsl:template match="dml:union | dml:except | dml:intersect">
		<xsl:param name="indent"/>
		<!--<xsl:value-of select="$NewLine"/>-->
		<!-- Apply template for first query expression -->
		<xsl:apply-templates select="*[1]">
			<xsl:with-param name="indent" select="concat($indent, $IndentChar)"/>
		</xsl:apply-templates>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:choose>
			<xsl:when test="self::dml:union">
				<xsl:text>UNION</xsl:text>
			</xsl:when>
			<xsl:when test="self::dml:except">
				<xsl:text>EXCEPT</xsl:text>
			</xsl:when>
			<xsl:when test="self::dml:intersect">
				<xsl:text>INTERSECT</xsl:text>
			</xsl:when>
		</xsl:choose>
		<xsl:if test="@setQuantifier">
			<xsl:text> </xsl:text>
			<xsl:apply-templates select="@setQuantifier"/>
		</xsl:if>
		<xsl:if test="dml:correspondingSpec">
			<xsl:text> </xsl:text>
			<xsl:apply-templates select="dml:correspondingSpec"/>
		</xsl:if>
		<!--<xsl:value-of select="$NewLine"/>-->
		<!-- apply template for second query expression -->
		<xsl:apply-templates select="*[last()]">
			<xsl:with-param name="indent" select="concat($indent, $IndentChar)"/>
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="@setQuantifier">
		<xsl:value-of select="."/>
	</xsl:template>

	<xsl:template match="dml:correspondingSpec">
		<xsl:text>CORRESPONDING</xsl:text>
		<xsl:if test="dep:simpleColumnReference">
			<xsl:text> BY </xsl:text>
			<xsl:value-of select="$LeftParen"/>
			<xsl:apply-templates select="dep:simpleColumnReference"/>
			<xsl:value-of select="$RightParen"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="dml:querySpecification">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>SELECT </xsl:text>
		<xsl:if test="@setQuantifier">
			<xsl:apply-templates select="@setQuantifier"/>
			<xsl:text> </xsl:text>
		</xsl:if>
		<xsl:apply-templates>
			<!--<xsl:with-param name="indent" select="concat($indent, $IndentChar)"/>-->
			<xsl:with-param name="indent" select="$indent"/>
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="dml:selectList">
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="dml:asterisk">
		<xsl:text>* </xsl:text>
	</xsl:template>

	<xsl:template match="dml:asteriskedIdentifierChain">
		<xsl:value-of select="@identifierChain"/>
		<xsl:value-of select="$Period"/>
		<xsl:text>* </xsl:text>
	</xsl:template>

	<xsl:template match="dml:allFieldsReference">
		<xsl:apply-templates select="child::*[1]"/>
		<xsl:value-of select="$Period"/>
		<xsl:text>*</xsl:text>
		<xsl:if test="dep:columnNameDefinition">
			<xsl:text> AS </xsl:text>
			<xsl:value-of select="$LeftParen"/>
			<xsl:apply-templates select="dep:columnNameDefinition"/>
			<xsl:value-of select="$RightParen"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="dml:derivedColumn">
		<xsl:apply-templates/>
		<xsl:text> AS </xsl:text>
		<xsl:apply-templates select="@columnName"/>
	</xsl:template>

	<xsl:template match="@columnName">
		<xsl:call-template name="RenderIdentifier"/>
	</xsl:template>

	<xsl:template match="dml:fromClause">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>FROM </xsl:text>
		<xsl:apply-templates>
			<xsl:with-param name="indent" select="concat($indent, $IndentChar)"/>
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="dml:whereClause">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>WHERE </xsl:text>
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="dml:havingClause">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>HAVING </xsl:text>
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="dml:groupByClause">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>GROUP BY </xsl:text>
		<xsl:apply-templates select="@setQuantifier"/>
		<xsl:for-each select="child::*[1]">
			<xsl:apply-templates/>
			<xsl:if test="position != last()">
				<xsl:text>, </xsl:text>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="dml:ordinaryGroupingSet">
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
		<xsl:if test="position() != last()">
			<xsl:text>, </xsl:text>
		</xsl:if>
	</xsl:template>

	<xsl:template match="dml:rollupList">
		<xsl:text>ROLLUP </xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="dml:cubeList">
		<xsl:text>CUBE </xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="groupingSetsSpecification">
		<xsl:text>GROUPING SETS </xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="dml:emptyGroupingSet">
		<xsl:value-of select="$LeftParen"/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="dml:ordinaryGroupingSet">
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
		<xsl:if test="position() != last()">
			<xsl:text>,</xsl:text>
		</xsl:if>
		<xsl:text> </xsl:text>
	</xsl:template>

	<xsl:template match="dml:windowClause">
		<!-- TODO -->
		<!-- The xml schema needs to be cleaned up for <window clause> before its corisponding transform is written. -->
		<xsl:text> ***CURRENTLY UNDONE*** </xsl:text>
	</xsl:template>

	<xsl:template match="dml:explicitTable">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>TABLE </xsl:text>
		<xsl:apply-templates select="@catalog" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@schema" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@name" mode="ForSchemaQualifiedName"/>
	</xsl:template>

	<!-- End of Query Specification -->

	<!-- Column Definition pg.536 -->

	<xsl:template match="ddl:columnDefinition">
		<xsl:param name="indent"/>
		<xsl:if test="not(position()=1)">
			<xsl:text>,</xsl:text>
		</xsl:if>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:apply-templates select="@name" mode="ForColumnName"/>
		<xsl:apply-templates select="ddt:boolean | ddt:characterString | ddt:binaryString | ddt:exactNumeric | ddt:approximateNumeric | ddt:date | ddt:time | ddt:interval | ddt:domain"/>
		<xsl:apply-templates select="ddl:defaultClause"/>
		<xsl:apply-templates select="ddl:identityColumnSpecification"/>
		<xsl:apply-templates select="ddl:generationClause"/>
		<xsl:apply-templates select="ddl:columnConstraintDefinition"/>
	</xsl:template>

	<xsl:template match="ddl:generationClause">
		<xsl:text> GENERATED ALWAYS AS </xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="ddl:defaultClause">
		<xsl:text> DEFAULT </xsl:text>
		<xsl:apply-templates select="ddt:dateLiteral"/>
		<xsl:apply-templates select="ddt:datetimeLiteral"/>
		<xsl:apply-templates select="ddt:characterStringLiteral"/>
		<xsl:apply-templates select="ddt:binaryStringLiteral"/>
		<xsl:apply-templates select="ddt:timeLiteral"/>
		<xsl:apply-templates select="ddt:dayTimeIntervalLiteral"/>
		<xsl:apply-templates select="ddt:timestampLiteral"/>
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="@name" mode="ForColumnName">
		<xsl:call-template name="RenderIdentifier"/>
		<xsl:text> </xsl:text>
	</xsl:template>

	<xsl:template match="ddt:boolean">
		<xsl:apply-templates select="@type" mode="ForDataType"/>
	</xsl:template>

	<xsl:template match="ddt:characterString">
		<xsl:apply-templates select="@type" mode="ForDataType"/>
		<xsl:apply-templates select="." mode="ForDataTypeLength"/>
		<xsl:apply-templates select="@characterSet" mode="ForDataType"/>
		<xsl:apply-templates select="@collate" mode="ForDataType"/>
	</xsl:template>

	<xsl:template match="ddt:binaryString">
		<xsl:apply-templates select="@type" mode="ForDataType"/>
		<xsl:apply-templates select="." mode="ForDataTypeLength"/>
	</xsl:template>

	<xsl:template match="ddt:characterString | ddt:binaryString" mode="ForDataTypeLength">
		<xsl:variable name="maximumLengthFragment">
			<xsl:choose>
				<xsl:when test="@type='CHARACTER'">
					<xsl:value-of select="$MaximumCharacterNonVaryingStringLength"/>
				</xsl:when>
				<xsl:when test="@type='CHARACTER VARYING'">
					<xsl:value-of select="$MaximumCharacterVaryingStringLength"/>
				</xsl:when>
				<xsl:when test="@type='CHARACTER LARGE OBJECT'">
					<xsl:value-of select="$MaximumCharacterLargeObjectStringLength"/>
				</xsl:when>
				<xsl:when test="@type='BINARY'">
					<xsl:value-of select="$MaximumBinaryNonVaryingStringLength"/>
				</xsl:when>
				<xsl:when test="@type='BINARY VARYING'">
					<xsl:value-of select="$MaximumBinaryVaryingStringLength"/>
				</xsl:when>
				<xsl:when test="@type='BINARY LARGE OBJECT'">
					<xsl:value-of select="$MaximumBinaryLargeObjectStringLength"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:message>
						<xsl:text>Unknown character string or binary string type "</xsl:text>
						<xsl:value-of select="@type"/>
						<xsl:text>".</xsl:text>
					</xsl:message>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="maximumLength" select="string($maximumLengthFragment)"/>
		<xsl:variable name="length" select="string(@length)"/>
		<!-- If neither a length nor a maximum length is specified, don't render the length portion at all. -->
		<xsl:if test="$length or $maximumLength">
			<xsl:value-of select="$LeftParen"/>
			<xsl:choose>
				<xsl:when test="$length and string(@lengthMultiplier)">
					<xsl:apply-templates select="." mode="ForDataTypeLengthWithMultiplier"/>
				</xsl:when>
				<xsl:when test="$length">
					<xsl:value-of select="$length"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$maximumLength"/>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:value-of select="$RightParen"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="ddt:characterString | ddt:binaryString" mode="ForDataTypeLengthWithMultiplier">
		<xsl:value-of select="@length"/>
		<xsl:value-of select="@lengthMultiplier"/>
	</xsl:template>

	<xsl:template match="ddt:exactNumeric | ddt:approximateNumeric">
		<xsl:apply-templates select="@type" mode="ForDataType"/>
		<xsl:apply-templates select="." mode="ForDataTypeNumericPrecisionAndScale"/>
	</xsl:template>

	<xsl:template match="ddt:exactNumeric[@type='NUMERIC' or @type='DECIMAL']" mode="ForDataTypeNumericPrecisionAndScale">
		<xsl:variable name="precision" select="string(@precision)"/>
		<xsl:variable name="scale" select="string(@scale)"/>

		<xsl:variable name="maximumPrecisionFragment">
			<xsl:choose>
				<xsl:when test="@type='NUMERIC'">
					<xsl:choose>
						<xsl:when test="$scale">
							<xsl:value-of select="$MaximumNumericExactNumericPrecisionWithScale"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$MaximumNumericExactNumericPrecisionWithoutScale"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="@type='DECIMAL'">
					<xsl:choose>
						<xsl:when test="$scale">
							<xsl:value-of select="$MaximumDecimalExactNumericPrecisionWithScale"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$MaximumDecimalExactNumericPrecisionWithoutScale"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:message>
						<xsl:text>Unknown exact numeric type "</xsl:text>
						<xsl:value-of select="@type"/>
						<xsl:text>".</xsl:text>
					</xsl:message>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="maximumPrecision" select="string($maximumPrecisionFragment)"/>
		
		<xsl:variable name="maximumScaleFragment">
			<xsl:choose>
				<xsl:when test="@type='NUMERIC'">
					<xsl:value-of select="$MaximumNumericExactNumericScale"/>
				</xsl:when>
				<xsl:when test="@type='DECIMAL'">
					<xsl:value-of select="$MaximumDecimalExactNumericScale"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:message>
						<xsl:text>Unknown exact numeric type "</xsl:text>
						<xsl:value-of select="@type"/>
						<xsl:text>".</xsl:text>
					</xsl:message>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="maximumScale" select="string($maximumScaleFragment)"/>

		<!-- If none of a precision, a maximum precision, a scale, nor a maximum scale are specified, don't render the precision and scale portion at all. -->
		<xsl:if test="$precision or $maximumPrecision or $scale or $maximumScale">
			<xsl:value-of select="$LeftParen"/>
			<xsl:choose>
				<xsl:when test="$precision">
					<xsl:value-of select="$precision"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$maximumPrecision"/>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:text>,</xsl:text>
			<xsl:choose>
				<xsl:when test="$scale">
					<!-- If a scale is specified, we always use it. -->
					<xsl:value-of select="$scale"/>
				</xsl:when>
				<xsl:when test="$precision">
					<!-- If a scale is not specified but a precision is specified, we use a scale of 0, which is the default defined by the SQL standard for this situtation. -->
					<!-- However, we always explicitly render the 0 to avoid having to deal with it in specific renderers for targets that have a different default. -->
					<xsl:text>0</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$maximumScale"/>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:value-of select="$RightParen"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="ddt:approximateNumeric[@type='FLOAT']" mode="ForDataTypeNumericPrecisionAndScale">
		<xsl:variable name="precision" select="string(@precision)"/>

		<xsl:variable name="maximumPrecisionFragment">
			<xsl:choose>
				<xsl:when test="@type='FLOAT'">
					<xsl:value-of select="$MaximumFloatApproximateNumericPrecision"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:message>
						<xsl:text>Unknown approximate numeric type "</xsl:text>
						<xsl:value-of select="@type"/>
						<xsl:text>".</xsl:text>
					</xsl:message>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="maximumPrecision" select="string($maximumPrecisionFragment)"/>

		<xsl:if test="$precision or $maximumPrecision">
			<xsl:value-of select="$LeftParen"/>
			<xsl:choose>
				<xsl:when test="$precision">
					<xsl:value-of select="$precision"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$maximumPrecision"/>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:value-of select="$RightParen"/>
		</xsl:if>
		
	</xsl:template>
	
	<xsl:template match="ddt:exactNumeric[not(@type='DECIMAL' or @type='NUMERIC')] | ddt:approximateNumeric[not(@type='FLOAT')]" mode="ForDataTypeNumericPrecisionAndScale">
		<!-- Do nothing. Only DECIMAL, NUMERIC, and FLOAT have precision and/or scale. -->
	</xsl:template>

	<xsl:template match="ddt:date">
		<xsl:apply-templates select="@type" mode="ForDataType"/>
	</xsl:template>

	<xsl:template match="ddt:time">
		<xsl:apply-templates select="@type" mode="ForDataType"/>
		<xsl:apply-templates select="@precision" mode="ForTimeDataType"/>
		<xsl:apply-templates select="@zone" mode="ForDateDataType"/>
	</xsl:template>

	<xsl:template match="ddt:interval">
		<xsl:apply-templates select="@type" mode="ForDataType"/>
		<xsl:text> </xsl:text>
		<xsl:apply-templates select="@fields" mode="ForIntervalDataType"/>
		<xsl:apply-templates select="@precision" mode="ForIntervalDataType"/>
	</xsl:template>

	<xsl:template match="@type" mode="ForDataType">
		<xsl:value-of select="."/>
	</xsl:template>

	<xsl:template match="@type[.='DATETIME']" mode="ForDataType">
		<xsl:text>TIMESTAMP</xsl:text>
	</xsl:template>

	<xsl:template match="ddt:characterString | ddt:binaryString" mode="GetTotalDataTypeLength" name="GetTotalDataTypeLength">
		<xsl:param name="length" select="number(@length)"/>
		<xsl:param name="lengthMultiplier" select="string(@lengthMultiplier)"/>
		<xsl:choose>
			<xsl:when test="not($length)"/>
			<xsl:when test="not($lengthMultiplier)">
				<xsl:value-of select="$length"/>
			</xsl:when>
			<xsl:when test="$lengthMultiplier = 'K'">
				<xsl:value-of select="$length * (1024)"/>
			</xsl:when>
			<xsl:when test="$lengthMultiplier = 'M'">
				<xsl:value-of select="$length * (1024 * 1024)"/>
			</xsl:when>
			<xsl:when test="$lengthMultiplier = 'G'">
				<xsl:value-of select="$length * (1024 * 1024 * 1024)"/>
			</xsl:when>
			<xsl:when test="$lengthMultiplier = 'T'">
				<xsl:value-of select="$length * (1024 * 1024 * 1024 * 1024)"/>
			</xsl:when>
			<xsl:when test="$lengthMultiplier = 'P'">
				<xsl:value-of select="$length * (1024 * 1024 * 1024 * 1024 * 1024)"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:message>
					<xsl:text>Ignoring unrecognized large object length multiplier value "</xsl:text>
					<xsl:value-of select="$lengthMultiplier"/>
					<xsl:text>".</xsl:text>
				</xsl:message>
				<xsl:value-of select="$length"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="ddt:domain">
		<xsl:apply-templates select="@catalog" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@schema" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@name" mode="ForSchemaQualifiedName"/>
	</xsl:template>

	<xsl:template match="ddl:identityColumnSpecification">
		<xsl:text> GENERATED </xsl:text>
		<xsl:value-of select="@type"/>
		<xsl:text> AS IDENTITY</xsl:text>
		<xsl:if test="child::*">
			<xsl:value-of select="$LeftParen"/>
			<xsl:apply-templates/>
			<xsl:value-of select="$RightParen"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="ddl:sequenceGeneratorDefinition">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>CREATE SEQUENCE </xsl:text>
		<xsl:apply-templates select="@catalog" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@schema" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@name" mode="ForSchemaQualifiedName"/>
		<xsl:if test="ddt:*">
			<xsl:text> AS </xsl:text>
			<xsl:apply-templates select="ddt:*">
				<xsl:with-param name="indent" select="concat($indent, $IndentChar)"/>
			</xsl:apply-templates>
			<xsl:if test="count(child::*) > 1">
				<xsl:text> </xsl:text>
			</xsl:if>
		</xsl:if>
		<xsl:apply-templates select="*[not(self::ddt:*)]">
			<xsl:with-param name="indent" select="concat($indent, $IndentChar)"/>
		</xsl:apply-templates>
		<xsl:value-of select="$StatementDelimiter"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<xsl:template match="ddl:sequenceGeneratorStartWithOption">
		<xsl:text>START WITH </xsl:text>
		<xsl:apply-templates/>
		<xsl:if test="position() != last()">
			<xsl:text> </xsl:text>
		</xsl:if>
	</xsl:template>

	<xsl:template match="ddl:sequenceGeneratorIncrementByOption">
		<xsl:text>INCREMENT BY </xsl:text>
		<xsl:apply-templates/>
		<xsl:if test="position() != last()">
			<xsl:text> </xsl:text>
		</xsl:if>
	</xsl:template>

	<xsl:template match="ddl:sequenceGeneratorMaxValueOption">
		<xsl:text>MAXVALUE </xsl:text>
		<xsl:apply-templates/>
		<xsl:if test="position() != last()">
			<xsl:text> </xsl:text>
		</xsl:if>
	</xsl:template>

	<xsl:template match="ddl:sequenceGeneratorMinValueOption">
		<xsl:text>MINVALUE </xsl:text>
		<xsl:apply-templates/>
		<xsl:if test="position() != last()">
			<xsl:text> </xsl:text>
		</xsl:if>
	</xsl:template>

	<xsl:template match="ddl:sequenceGeneratorCycleOption">
		<xsl:if test="@shouldCycle != 'true' and @shouldCycle != 1">
			<xsl:text>NO </xsl:text>
		</xsl:if>
		<xsl:text>CYCLE</xsl:text>
	</xsl:template>

	<xsl:template match="@precision" mode="ForIntervalDataType">
		<xsl:value-of select="$LeftParen"/>
		<xsl:value-of select="."/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="@fields" mode="ForIntervalDataType">
		<xsl:value-of select="."/>
	</xsl:template>

	<xsl:template match="@zone" mode="ForDateDataType">
		<xsl:text> </xsl:text>
		<xsl:value-of select="."/>
	</xsl:template>

	<xsl:template match="@precision" mode="ForTimeDataType">
		<xsl:value-of select="$LeftParen"/>
		<xsl:value-of select="."/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="@characterSet" mode="ForDataType">
		<xsl:text> CHARACTER SET </xsl:text>
		<xsl:value-of select="."/>
	</xsl:template>

	<xsl:template match="@collate" mode="ForDataType">
		<xsl:text> COLLATE </xsl:text>
		<xsl:value-of select="."/>
	</xsl:template>

	<xsl:template match="ddt:exactNumericLiteral | ddt:approximateNumericLiteral | ddt:booleanLiteral">
		<xsl:value-of select="@value"/>
	</xsl:template>

	<xsl:template match="ddt:characterStringLiteral">
		<xsl:text>'</xsl:text>
		<xsl:call-template name="RenderStringLiteral">
			<xsl:with-param name="value" select="@value"/>
		</xsl:call-template>
		<xsl:text>'</xsl:text>
	</xsl:template>
	<xsl:template name="RenderStringLiteral">
		<xsl:param name="value"/>
		<xsl:choose>
			<xsl:when test="contains($value, &quot;'&quot;)">
				<xsl:value-of select="substring-before($value, &quot;'&quot;)"/>
				<xsl:text>''</xsl:text>
				<xsl:call-template name="RenderStringLiteral">
					<xsl:with-param name="value" select="substring-after($value, &quot;'&quot;)"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$value"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="ddt:binaryStringLiteral">
		<xsl:text>X'</xsl:text>
		<xsl:value-of select="@value"/>
		<xsl:text>'</xsl:text>
	</xsl:template>

	<xsl:template match="ddt:dayTimeIntervalLiteral">
		<xsl:text>INTERVAL </xsl:text>
		<xsl:text>'</xsl:text>
		<xsl:value-of select="@value"/>
		<xsl:text>' </xsl:text>
		<xsl:value-of select="@fields"/>
	</xsl:template>

	<xsl:template match="ddt:dateLiteral">
		<xsl:text>DATE '</xsl:text>
		<xsl:value-of select="@value"/>
		<xsl:text>'</xsl:text>
	</xsl:template>

	<xsl:template match="ddt:timeLiteral">
		<xsl:text>TIME '</xsl:text>
		<xsl:value-of select="@value"/>
		<xsl:text>'</xsl:text>
	</xsl:template>

	<xsl:template match="ddt:timestampLiteral | ddt:datetimeLiteral">
		<xsl:text>TIMESTAMP '</xsl:text>
		<xsl:value-of select="translate(@value,'T',' ')"/>
		<xsl:text>'</xsl:text>
	</xsl:template>

	<xsl:template match="dep:currentDateKeyword">
		<xsl:text>CURRENT_DATE</xsl:text>
	</xsl:template>

	<xsl:template match="dep:currentTimeKeyword">
		<xsl:text>CURRENT_TIME</xsl:text>
		<xsl:if test="string(@precision)">
			<xsl:value-of select="$LeftParen"/>
			<xsl:value-of select="@precision"/>
			<xsl:value-of select="$RightParen"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="dep:currentTimestampKeyword">
		<xsl:text>CURRENT_TIMESTAMP</xsl:text>
		<xsl:if test="string(@precision)">
			<xsl:value-of select="$LeftParen"/>
			<xsl:value-of select="@precision"/>
			<xsl:value-of select="$RightParen"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="dep:currentLocalTimeKeyword">
		<xsl:text>LOCALTIME</xsl:text>
		<xsl:if test="string(@precision)">
			<xsl:value-of select="$LeftParen"/>
			<xsl:value-of select="@precision"/>
			<xsl:value-of select="$RightParen"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="dep:currentLocalTimestampKeyword">
		<xsl:text>LOCALTIMESTAMP</xsl:text>
		<xsl:if test="string(@precision)">
			<xsl:value-of select="$LeftParen"/>
			<xsl:value-of select="@precision"/>
			<xsl:value-of select="$RightParen"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="dep:userKeyword">
		<xsl:text>USER</xsl:text>
	</xsl:template>

	<xsl:template match="dep:currentUserKeyword">
		<xsl:text>CURRENT_USER</xsl:text>
	</xsl:template>

	<xsl:template match="dep:currentRoleKeyword">
		<xsl:text>CURRENT_ROLE</xsl:text>
	</xsl:template>

	<xsl:template match="dep:sessionUserKeyword">
		<xsl:text>SESSION_USER</xsl:text>
	</xsl:template>

	<xsl:template match="dep:systemUserKeyword">
		<xsl:text>SYSTEM_USER</xsl:text>
	</xsl:template>

	<xsl:template match="dep:currentPathKeyword">
		<xsl:text>CURRENT_PATH</xsl:text>
	</xsl:template>

	<xsl:template match="dep:nullKeyword">
		<xsl:text>NULL</xsl:text>
	</xsl:template>

	<xsl:template match="ddl:notNullKeyword">
		<xsl:text>NOT NULL</xsl:text>
	</xsl:template>

	<xsl:template match="ddl:primaryKeyKeyword">
		<xsl:text>PRIMARY KEY</xsl:text>
	</xsl:template>

	<xsl:template match="ddl:uniqueKeyword">
		<xsl:text>UNIQUE</xsl:text>
	</xsl:template>

	<xsl:template match="dep:currentDefaultTransformGroupKeyword">
		<xsl:text>CURRENT_DEFAULT_TRANSFORM_GROUP</xsl:text>
	</xsl:template>

	<xsl:template match="dep:currentTransformGroupForTypeKeyword">
		<xsl:text>CURRENT_TRANSFORM_GROUP_FOR_TYPE</xsl:text>
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="dep:valueKeyword">
		<xsl:text>VALUE</xsl:text>
	</xsl:template>

	<!-- End of Column Definition -->

	<!-- Column Constraint Definition -->

	<xsl:template match="ddl:columnConstraintDefinition">
		<xsl:text> </xsl:text>
		<xsl:if test="string(@name)">
			<xsl:text>CONSTRAINT </xsl:text>
			<xsl:apply-templates select="@name" mode="ForSchemaQualifiedName"/>
			<xsl:text> </xsl:text>
		</xsl:if>
		<xsl:apply-templates/>
		<xsl:apply-templates select="@constraintCharacteristics"/>
	</xsl:template>

	<!-- End of Column Constraint Definition -->

	<!-- Value Expression -->

	<xsl:template match="dep:numericNegative">
		<xsl:text> -</xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="dep:numericPositive">
		<xsl:text> +</xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="dep:numericSubtraction">
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates select="child::*[1]"/>
		<xsl:text> - </xsl:text>
		<xsl:apply-templates select="child::*[2]"/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="dep:numericDivision">
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates select="child::*[1]"/>
		<xsl:text> / </xsl:text>
		<xsl:apply-templates select="child::*[2]"/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="dep:numericAddition">
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates select="child::*[1]"/>
		<xsl:text> + </xsl:text>
		<xsl:apply-templates select="child::*[2]"/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="dep:numericMultiplication">
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates select="child::*[1]"/>
		<xsl:text> * </xsl:text>
		<xsl:apply-templates select="child::*[2]"/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="dep:concatenation">
		<xsl:apply-templates select="child::*[1]"/>
		<xsl:text> || </xsl:text>
		<xsl:apply-templates select="child::*[2]"/>
	</xsl:template>

	<xsl:template match="dep:hostParameterSpecification">
		<xsl:text>:</xsl:text>
		<xsl:value-of select="@name"/>
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="dep:indicatorParameter">
		<xsl:text> INDICATOR </xsl:text>
		<xsl:value-of select="@hostParameterName"/>
	</xsl:template>

	<xsl:template match="dep:sqlParameterReference | dep:columnReference | dep:columnNameDefinition | dep:simpleColumnReference">
		<xsl:call-template name="RenderIdentifier">
			<xsl:with-param name="name" select="@name"/>
		</xsl:call-template>
		<xsl:if test="not(position()=last()) and (following-sibling::dep:sqlParameterReference or following-sibling::dep:columnReference or following-sibling::dep:columnNameDefinition or following-sibling::dep:simpleColumnReference)">
			<xsl:text>, </xsl:text>
		</xsl:if>
	</xsl:template>

	<xsl:template match="dep:collatedColumnReference">
		<xsl:call-template name="RenderIdentifier">
			<xsl:with-param name="name" select="@name"/>
		</xsl:call-template>
		<xsl:apply-templates select="@collation"/>
		<xsl:if test="not(position()=last())">
			<xsl:text>, </xsl:text>
		</xsl:if>
	</xsl:template>

	<xsl:template match="@collation">
		<xsl:text> COLATE </xsl:text>
		<xsl:value-of select="."/>
	</xsl:template>


	<xsl:template match="dep:dynamicParameterSpecification">
		<xsl:text>?</xsl:text>
	</xsl:template>

	<xsl:template match="dep:currentCollationSpecification">
		<xsl:text>COLLATION FOR </xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="ddt:userDefinedType">
		<xsl:apply-templates select="@catalog" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@schema" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@name" mode="ForSchemaQualifiedName"/>
	</xsl:template>

	<xsl:template match="dep:generalSetFunction">
		<xsl:value-of select="@type"/>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="dep:filterClause">
		<xsl:text>FILTER</xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:text>WHERE </xsl:text>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="dep:nullPredicate">
		<xsl:apply-templates/>
		<xsl:text> IS </xsl:text>
		<xsl:value-of select="@type"/>
	</xsl:template>

	<xsl:template match="dep:comparisonPredicate">
		<xsl:apply-templates select="child::*[1]"/>
		<xsl:apply-templates select="@operator"/>
		<xsl:apply-templates select="child::*[2]"/>
	</xsl:template>

	<xsl:template match="dep:betweenPredicate">
		<xsl:apply-templates select="child::*[1]"/>
		<xsl:text> </xsl:text>
		<xsl:choose>
			<xsl:when test="string(@type)">
				<xsl:value-of select="@type"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>BETWEEN</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:if test="string(@symmetry)">
			<xsl:text> </xsl:text>
			<xsl:value-of select="@symmetry"/>
		</xsl:if>
		<xsl:text> </xsl:text>
		<xsl:apply-templates select="child::*[2]"/>
		<xsl:text> AND </xsl:text>
		<xsl:apply-templates select="child::*[3]"/>
	</xsl:template>

	<xsl:template match="dep:inPredicate">
		<xsl:apply-templates select="child::*[1]"/>
		<xsl:text> IN </xsl:text>
		<xsl:choose>
			<xsl:when test="dml:tableSubquery">
				<xsl:apply-templates select="child::*[position() > 1]"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$LeftParen"/>
				<xsl:for-each select="child::*[position() > 1]">
					<xsl:apply-templates select="."/>
					<xsl:if test="not(position()=last())">
						<xsl:text>, </xsl:text>
					</xsl:if>
				</xsl:for-each>
				<xsl:value-of select="$RightParen"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="dep:parenthesizedValueExpression">
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="dep:charLengthExpression">
		<xsl:text>CHARACTER_LENGTH</xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="@lengthUnits">
		<xsl:text> USING </xsl:text>
		<xsl:value-of select="."/>
	</xsl:template>

	<xsl:template match="dep:trimFunction">
		<xsl:text>TRIM</xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:value-of select="@specification"/>
		<xsl:apply-templates select="dep:trimCharacter"/>
		<xsl:text> FROM </xsl:text>
		<xsl:apply-templates select="dep:trimSource"/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="dep:trimCharacter">
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="dep:trimSource">
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="dep:or">
		<xsl:for-each select="child::*">
			<xsl:apply-templates select="."/>
			<xsl:if test="not(position()=last())">
				<xsl:text> OR </xsl:text>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="dep:and">
		<xsl:for-each select="child::*">
			<xsl:apply-templates select="."/>
			<xsl:if test="not(position()=last())">
				<xsl:text> AND </xsl:text>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="dep:not">
		<xsl:text>NOT </xsl:text>
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="@truthValue">
		<xsl:value-of select="."/>
	</xsl:template>

	<xsl:template match="dep:is">
		<xsl:apply-templates/>
		<xsl:text> IS </xsl:text>
		<xsl:apply-templates select="@truthValue"/>
	</xsl:template>

	<xsl:template match="dep:isNot">
		<xsl:apply-templates/>
		<xsl:text> IS NOT </xsl:text>
		<xsl:apply-templates select="@truthValue"/>
	</xsl:template>

	<xsl:template match="@operator">
		<xsl:choose>
			<xsl:when test=". = 'equals'">
				<xsl:text> = </xsl:text>
			</xsl:when>
			<xsl:when test=". = 'notEquals'">
				<xsl:text> &lt;&gt; </xsl:text>
			</xsl:when>
			<xsl:when test=". = 'lessThan'">
				<xsl:text> &lt; </xsl:text>
			</xsl:when>
			<xsl:when test=". = 'greaterThan'">
				<xsl:text> &gt; </xsl:text>
			</xsl:when>
			<xsl:when test=". = 'lessThanOrEquals'">
				<xsl:text> &lt;= </xsl:text>
			</xsl:when>
			<xsl:when test=". = 'greaterThanOrEquals'">
				<xsl:text> &gt;= </xsl:text>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<!-- End of Value Expression -->

	<!-- Stored Procedure Definitions -->

	<xsl:template match="ddl:sqlInvokedProcedure">
		<xsl:value-of select="$NewLine"/>
		<xsl:text>CREATE PROCEDURE </xsl:text>
		<xsl:apply-templates select="@catalog" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@schema" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@name" mode="ForSchemaQualifiedName"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$LeftParen"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:apply-templates select="ddl:sqlParameterDeclaration"/>
		<xsl:value-of select="$RightParen"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:apply-templates select="ddl:sqlRoutineSpec"/>
		<xsl:value-of select="$StatementDelimiter"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<xsl:template match="ddl:sqlParameterDeclaration">
		<xsl:value-of select="$IndentChar"/>
		<xsl:call-template name="RenderIdentifier">
			<xsl:with-param name="name" select="@name"/>
		</xsl:call-template>
		<xsl:text> </xsl:text>
		<xsl:apply-templates select="child::*"/>
		<xsl:if test="not(position()=last())">
			<xsl:text>, </xsl:text>
		</xsl:if>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<xsl:template match="ddl:sqlRoutineSpec">
		<xsl:text>AS</xsl:text>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$IndentChar"/>
		<xsl:apply-templates select="child::*"/>
	</xsl:template>

	<xsl:template match="dml:insertStatement">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>INSERT INTO </xsl:text>
		<xsl:apply-templates select="@catalog" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@schema" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@name" mode="ForSchemaQualifiedName"/>
		<xsl:text> </xsl:text>
		<xsl:choose>
			<xsl:when test="dml:defaultValues">
				<xsl:apply-templates select="dml:defaultValues"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:if test="dep:simpleColumnReference">
					<xsl:value-of select="$LeftParen"/>
					<xsl:apply-templates select="dep:simpleColumnReference"/>
					<xsl:value-of select="$RightParen"/>
				</xsl:if>
				<xsl:apply-templates select="@overrideClause"/>
				<xsl:apply-templates select="*[not(self::dep:simpleColumnReference)]">
					<xsl:with-param name="indent" select="$indent"/>
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="@overrideClause">
		<xsl:text> </xsl:text>
		<xsl:value-of select="."/>
		<xsl:text> </xsl:text>
	</xsl:template>

	<xsl:template match="dml:defaultValues">
		<xsl:text>DEFAULT VALUES</xsl:text>
	</xsl:template>

	<xsl:template match="dml:tableValueConstructor">
		<xsl:param name="indent"/>
		<xsl:text>VALUES </xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:for-each select="child::*">
			<xsl:apply-templates select=".">
				<xsl:with-param name="indent" select="$indent"/>
			</xsl:apply-templates>
			<xsl:if test="not(position()=last())">
				<xsl:text>, </xsl:text>
			</xsl:if>
		</xsl:for-each>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="dml:updateStatement">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>UPDATE </xsl:text>
		<xsl:apply-templates select="@catalog" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@schema" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@name" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@correlationName"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>SET </xsl:text>
		<xsl:for-each select="dml:multipleColumnAssignment | dml:singleColumnAssignment">
			<xsl:apply-templates select=".">
				<xsl:with-param name="indent" select="$indent"/>
			</xsl:apply-templates>
			<xsl:if test="following-sibling::dml:multipleColumnAssignment | following-sibling::dml:singleColumnAssignment">
				<xsl:text>,</xsl:text>
				<!--No space because each column assignment is currently apearing on a new line -->
			</xsl:if>
		</xsl:for-each>
		<xsl:apply-templates select="dml:whereClause">
			<xsl:with-param name="indent" select="$indent"/>
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="dml:multipleColumnAssignment">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates select="dep:simpleColumnReference"/>
		<xsl:value-of select="$RightParen"/>
		<xsl:text> </xsl:text>
		<xsl:value-of select="$SetClauseEqualsOperator"/>
		<xsl:text> </xsl:text>
		<xsl:apply-templates select="*[not(self::dep:simpleColumnReference)]">
			<xsl:with-param name="indent" select="$indent"/>
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="dml:singleColumnAssignment">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:apply-templates select="@name" mode="ForColumnName"/>
		<xsl:text> </xsl:text>
		<xsl:value-of select="$SetClauseEqualsOperator"/>
		<xsl:text> </xsl:text>
		<xsl:apply-templates>
			<xsl:with-param name="indent" select="$indent"/>
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="dml:deleteStatement">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>DELETE FROM </xsl:text>
		<xsl:apply-templates select="@catalog" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@schema" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@name" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@correlationName"/>
		<xsl:apply-templates select="child::*">
			<xsl:with-param name="indent" select="$indent"/>
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="dml:whereClause">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>WHERE </xsl:text>
		<xsl:apply-templates>
			<xsl:with-param name="indent" select="$indent"/>
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="dml:searchCondition">
		<xsl:param name="indent"/>
		<xsl:apply-templates>
			<xsl:with-param name="indent" select="$indent"/>
		</xsl:apply-templates>
	</xsl:template>

	<!-- End of Stored Procedure Definitions -->

	<!-- Trigger Definitions-->

	<xsl:template match="ddl:triggerDefinition">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>CREATE TRIGGER </xsl:text>
		<xsl:apply-templates select="@catalog" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@schema" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@name" mode="ForSchemaQualifiedName"/>
		<xsl:text> </xsl:text>
		<xsl:value-of select="@actionTime"/>
		<xsl:text> </xsl:text>
		<xsl:apply-templates select="ddl:event"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>ON </xsl:text>
		<xsl:apply-templates select="ddl:table"/>
		<xsl:apply-templates select="ddl:referencing">
			<xsl:with-param name="indent" select="$indent"/>
		</xsl:apply-templates>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:if test="@forEach">
			<xsl:text>FOR EACH </xsl:text>
			<xsl:value-of select="@forEach"/>
		</xsl:if>
		<xsl:apply-templates select="ddl:when"/>
		<xsl:apply-templates select="ddl:atomicBlock">
			<xsl:with-param name="indent" select="$indent"/>
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="ddl:event">
		<xsl:value-of select="@type"/>
		<xsl:if test="@type='UPDATE' and dep:simpleColumnReference">
			<xsl:text> OF </xsl:text>
			<xsl:apply-templates select="dep:simpleColumnReference"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="ddl:triggerDefinition/ddl:table">
		<xsl:apply-templates select="@catalog" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@schema" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@name" mode="ForSchemaQualifiedName"/>	
	</xsl:template>

	<xsl:template match="ddl:when">
		<xsl:text>WHEN </xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="ddl:referencing">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>REFERENCING</xsl:text>
		<xsl:apply-templates>
			<xsl:with-param name="indent" select="concat($indent, $IndentChar)"/>
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="ddl:newRow">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>NEW ROW AS </xsl:text>
		<xsl:call-template name="RenderIdentifier">
			<xsl:with-param name="name" select="@name"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="ddl:oldRow">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>OLD ROW AS </xsl:text>
		<xsl:call-template name="RenderIdentifier">
			<xsl:with-param name="name" select="@name"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="ddl:newTable">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>NEW TABLE AS </xsl:text>
		<xsl:call-template name="RenderIdentifier">
			<xsl:with-param name="name" select="@name"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="ddl:oldTable">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>OLD TABLE AS </xsl:text>
		<xsl:call-template name="RenderIdentifier">
			<xsl:with-param name="name" select="@name"/>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="ddl:atomicBlock">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>BEGIN ATOMIC</xsl:text>
		<xsl:for-each select="child::*">
			<xsl:apply-templates select=".">
				<xsl:with-param name="indent" select="concat($indent, $IndentChar)"/>
			</xsl:apply-templates>
			<xsl:value-of select="$StatementDelimiter"/>
		</xsl:for-each>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>END</xsl:text>
	</xsl:template>

	<!-- End of Trigger Definitions-->

	<!-- Domain Definition -->

	<xsl:template match="ddl:domainDefinition">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>CREATE DOMAIN </xsl:text>
		<xsl:apply-templates select="@catalog" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@schema" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@name" mode="ForSchemaQualifiedName"/>
		<xsl:text> AS </xsl:text>
		<xsl:apply-templates/>
		<xsl:value-of select="$StatementDelimiter"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<xsl:template match="ddl:domainConstraintDefinition | ddl:addDomainConstraintDefinition">
		<xsl:if test="self::ddl:addDomainConstraintDefinition">
			<xsl:text>ADD</xsl:text>
		</xsl:if>
		<xsl:text> CONSTRAINT </xsl:text>
		<xsl:apply-templates select="@name" mode="ForSchemaQualifiedName"/>
		<xsl:text> </xsl:text>
		<xsl:apply-templates/>
		<xsl:apply-templates select="@constraintCharacteristics"/>
	</xsl:template>

	<xsl:template match="@constraintCharacteristics">
		<xsl:text> </xsl:text>
		<xsl:value-of select="."/>
	</xsl:template>

	<!-- End of Domain Definition -->

	<!-- Table Constraint Definition -->

	<xsl:template match="ddl:tableConstraintDefinition | ddl:addTableConstraintDefinition">
		<xsl:param name="indent"/>
		<xsl:choose>
			<xsl:when test="self::ddl:addTableConstraintDefinition">
				<xsl:text>ADD </xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:if test="not(position()=1) or preceding-sibling::*">
					<xsl:text>,</xsl:text>
				</xsl:if>
				<xsl:value-of select="$NewLine"/>
				<xsl:value-of select="$indent"/>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:text>CONSTRAINT </xsl:text>
		<xsl:apply-templates select="@name" mode="ForSchemaQualifiedName"/>
		<xsl:text> </xsl:text>
		<xsl:apply-templates/>
		<xsl:apply-templates select="@constraintCharacteristics"/>
	</xsl:template>

	<xsl:template match="ddl:uniqueConstraintDefinition">
		<xsl:value-of select="@type"/>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="ddl:referentialConstraintDefinition">
		<xsl:text>FOREIGN KEY </xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates select="dep:simpleColumnReference"/>
		<xsl:value-of select="$RightParen"/>
		<xsl:text> </xsl:text>
		<xsl:apply-templates select="child::*[not(self::dep:simpleColumnReference)]"/>
	</xsl:template>

	<xsl:template match="ddl:referencesSpecification">
		<xsl:text>REFERENCES </xsl:text>
		<xsl:apply-templates select="@catalog" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@schema" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@name" mode="ForSchemaQualifiedName"/>
		<xsl:text> </xsl:text>
		<xsl:if test="dep:simpleColumnReference">
			<xsl:value-of select="$LeftParen"/>
			<xsl:apply-templates/>
			<xsl:value-of select="$RightParen"/>
		</xsl:if>
		<xsl:apply-templates select="@match" mode="ForReferenceSpecification"/>
		<xsl:apply-templates select="@onDelete" mode="ForReferenceSpecification"/>
		<xsl:apply-templates select="@onUpdate" mode="ForReferenceSpecification"/>
	</xsl:template>

	<xsl:template match="@onUpdate" mode="ForReferenceSpecification">
		<xsl:text> ON UPDATE </xsl:text>
		<xsl:value-of select="."/>
	</xsl:template>

	<xsl:template match="@onDelete" mode="ForReferenceSpecification">
		<xsl:text> ON DELETE </xsl:text>
		<xsl:value-of select="."/>
	</xsl:template>

	<xsl:template match="@match" mode="ForReferenceSpecification">
		<xsl:text> MATCH </xsl:text>
		<xsl:value-of select="."/>
	</xsl:template>

	<xsl:template match="ddl:checkConstraintDefinition">
		<xsl:text>CHECK </xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<!-- End of Table Constraint Definition -->

	<!-- Start Transaction Statement -->

	<xsl:template match="dms:startTransactionStatement">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>START TRANSACTION ISOLATION LEVEL </xsl:text>
		<xsl:value-of select="@isolationLevel"/>
		<xsl:text>, </xsl:text>
		<xsl:value-of select="@accessMode"/>
		<xsl:value-of select="$StatementDelimiter"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<!-- Start Transaction Statement -->

	<!-- Alter Table Statement -->

	<xsl:template match="ddl:alterTableStatement">
		<xsl:param name="indent"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>ALTER TABLE </xsl:text>
		<xsl:apply-templates select="@catalog" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@schema" mode="ForSchemaQualifiedName"/>
		<xsl:apply-templates select="@name" mode="ForSchemaQualifiedName"/>
		<xsl:text> </xsl:text>
		<xsl:apply-templates>
			<xsl:with-param name="indent" select="$indent"/>
		</xsl:apply-templates>
		<xsl:value-of select="$StatementDelimiter"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<xsl:template match="ddl:addColumnDefinition">
		<xsl:text>ADD COLUMN </xsl:text>
		<xsl:apply-templates/>
	</xsl:template>

	<!-- End of Alter Table Statement -->

	<xsl:template name="RenderIdentifier">
		<xsl:param name="name" select="."/>
		<xsl:value-of select="$name"/>
	</xsl:template>

</xsl:stylesheet>
