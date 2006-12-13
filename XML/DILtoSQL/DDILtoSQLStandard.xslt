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
	<xsl:param name="StatementDelimeter" select="';'"/>
	<xsl:param name="setClauseEqualsOperator" select="'='" />
	<xsl:param name="StatementBlockDelimeter" select="''" />

	<!-- Schema Definition pg.519 -->

	<xsl:template match="ddl:schemaDefinition">
		<xsl:param name="indent"/>
		<!--<xsl:text>DROP SCHEMA </xsl:text>
		<xsl:apply-templates select="@catalogName" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="@schemaName" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="@authorizationIdentifier" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="@defaultCharacterSet" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="ddl:path" mode="ForSchemaDefinition"/>
		<xsl:value-of select="$StatementDelimeter"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>-->
		<xsl:text>CREATE SCHEMA </xsl:text>
		<xsl:apply-templates select="@catalogName" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="@schemaName" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="@authorizationIdentifier" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="@defaultCharacterSet" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="ddl:path" mode="ForSchemaDefinition"/>
		<xsl:value-of select="$StatementDelimeter"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:apply-templates>
			<xsl:with-param name="indent" select="concat($indent, $IndentChar)"/>
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="@catalogName" mode="ForSchemaDefinition">
		<xsl:value-of select="."/>
		<xsl:value-of select="$Period"/>
	</xsl:template>

	<xsl:template match="@schemaName" mode="ForSchemaDefinition">
		<xsl:call-template name="RenderIdentifier">
			<xsl:with-param name="name" select="."/>
		</xsl:call-template>
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
		<xsl:text>SET SCHEMA </xsl:text>
		<xsl:apply-templates/>
		<xsl:value-of select="$StatementDelimeter"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<xsl:template match="dms:commitStatement">
		<xsl:text>COMMIT WORK</xsl:text>
		<xsl:if test="@type">
			<xsl:text> AND </xsl:text>
			<xsl:value-of select="@type"/>
		</xsl:if>
		<xsl:value-of select="$StatementDelimeter"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<!-- End of Schema Definition -->

	<!-- Table Definition pg.525 -->

	<xsl:template match="ddl:tableDefinition">
		<xsl:param name="indent"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>CREATE </xsl:text>
		<xsl:apply-templates select="@scope" mode="ForTableDefinition"/>
		<xsl:text>TABLE </xsl:text>
		<xsl:apply-templates select="@catalog" mode="ForTableDefinition"/>
		<xsl:apply-templates select="@schema" mode="ForTableDefinition"/>
		<xsl:apply-templates select="@name" mode="ForTableDefinition"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:value-of select="$StatementStartBracket"/>
		<xsl:apply-templates select="ddl:columnDefinition">
			<xsl:with-param name="indent" select="concat($NewLine, concat($indent, $IndentChar))"/>
		</xsl:apply-templates>
		<xsl:apply-templates select="ddl:tableConstraintDefinition">
			<xsl:with-param name="indent" select="concat($NewLine, concat($indent, $IndentChar))"/>
		</xsl:apply-templates>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:value-of select="$StatementEndBracket"/>
		<xsl:value-of select="$StatementDelimeter"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<xsl:template match="@scope" mode="ForTableDefinition">
		<xsl:value-of select="."/>
		<xsl:text> </xsl:text>
	</xsl:template>

	<xsl:template match="@catalog" mode="ForTableDefinition">
		<xsl:value-of select="."/>
		<xsl:value-of select="$Period"/>
	</xsl:template>

	<xsl:template match="@schema" mode="ForTableDefinition">
		<xsl:value-of select="."/>
		<xsl:value-of select="$Period"/>
	</xsl:template>

	<xsl:template match="@name" mode="ForTableDefinition">
		<xsl:call-template name="RenderIdentifier">
			<xsl:with-param name="name" select="."/>
		</xsl:call-template>
	</xsl:template>

	<!--End of Table Definition -->

	<!-- Column Definition pg.536 -->

	<xsl:template match="ddl:columnDefinition">
		<xsl:param name="indent"/>
		<xsl:value-of select="$indent"/>
		<xsl:apply-templates select="@name" mode="ForColumnDefinition"/>
		<xsl:apply-templates select="ddt:boolean | ddt:characterString | ddt:binaryString | ddt:exactNumeric | ddt:approximateNumeric | ddt:date | ddt:time | ddt:interval | ddt:domain"/>
		<xsl:apply-templates select="ddl:defaultClause"/>
		<xsl:apply-templates select="ddl:identityColumnSpecification"/>
		<xsl:apply-templates select="ddl:generationClause"/>
		<xsl:apply-templates select="ddl:columnConstraintDefinition"/>
		<xsl:choose>
			<xsl:when test="position()=last() and not(following-sibling::*)">
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>, </xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="ddl:generationClause">
		<xsl:text>GENERATED ALWAYS AS </xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
		<xsl:text> </xsl:text>
	</xsl:template>

	<xsl:template match="ddl:defaultClause">
		<xsl:text> DEFAULT </xsl:text>
		<xsl:apply-templates select="ddt:dateLiteral"/>
		<xsl:apply-templates select="ddt:characterStringLiteral"/>
		<xsl:apply-templates select="ddt:binaryStringLiteral"/>
		<xsl:apply-templates select="ddt:timeLiteral"/>
		<xsl:apply-templates select="ddt:dayTimeIntervalLiteral"/>
		<xsl:apply-templates select="ddt:timestampLiteral"/>
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="@name" mode="ForColumnDefinition">
		<xsl:call-template name="RenderIdentifier">
			<xsl:with-param name="name" select="."/>
		</xsl:call-template>
		<xsl:text> </xsl:text>
	</xsl:template>

	<xsl:template match="ddt:boolean">
		<xsl:value-of select="@type"/>
		<xsl:text> </xsl:text>
	</xsl:template>

	<xsl:template match="ddt:characterString">
		<xsl:value-of select="@type"/>
		<xsl:value-of select="$LeftParen"/>
		<xsl:value-of select="@length"/>
		<xsl:value-of select="$RightParen"/>
		<xsl:text> </xsl:text>
		<xsl:apply-templates select="@characterSet" mode="ForColumnDataType"/>
		<xsl:apply-templates select="@collate" mode="ForColumnDataType"/>
	</xsl:template>

	<xsl:template match="ddt:binaryString">
		<xsl:value-of select="@type"/>
		<xsl:value-of select="$LeftParen"/>
		<xsl:value-of select="@length"/>
		<xsl:value-of select="$RightParen"/>
		<xsl:text> </xsl:text>
	</xsl:template>

	<xsl:template match="ddt:exactNumeric">
		<xsl:value-of select="@type"/>
		<xsl:apply-templates select="@precision" mode="ForNumeric"/>
		<xsl:apply-templates select="@scale" mode="ForNumeric"/>
		<xsl:text> </xsl:text>
	</xsl:template>

	<xsl:template match="ddt:approximateNumeric">
		<xsl:value-of select="@type"/>
		<xsl:apply-templates select="@precision" mode="ForNumeric"/>
		<xsl:text> </xsl:text>
	</xsl:template>

	<xsl:template match="ddt:date">
		<xsl:value-of select="@type"/>
		<xsl:text> </xsl:text>
	</xsl:template>

	<xsl:template match="ddt:time">
		<xsl:value-of select="@type"/>
		<xsl:apply-templates select="@precision" mode="ForTimeDataType"/>
		<xsl:apply-templates select="@zone" mode="ForDateDataType"/>
		<xsl:text> </xsl:text>
	</xsl:template>

	<xsl:template match="ddt:interval">
		<xsl:value-of select="@type"/>
		<xsl:text> </xsl:text>
		<xsl:apply-templates select="@fields" mode="ForIntervalDataType"/>
		<xsl:apply-templates select="@precision" mode="ForIntervalDataType"/>
		<xsl:text> </xsl:text>
	</xsl:template>

	<xsl:template match="ddt:domain">
		<xsl:if test="@schema">
			<xsl:value-of select="@schema"/>
			<xsl:text>.</xsl:text>
		</xsl:if>
		<xsl:value-of select="@name"/>
		<xsl:text> </xsl:text>
	</xsl:template>

	<xsl:template match="ddl:identityColumnSpecification">
		<xsl:text>GENERATED </xsl:text>
		<xsl:value-of select="@type"/>
		<xsl:text> AS IDENTITY</xsl:text>
		<xsl:if test="child::*">
			<xsl:value-of select="$LeftParen"/>
			<xsl:apply-templates/>
			<xsl:value-of select="$RightParen"/>
		</xsl:if>
		<xsl:text> </xsl:text>
	</xsl:template>

	<xsl:template match="ddl:sequenceGeneratorStartWithOption">
		<xsl:text>START WITH </xsl:text>
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="ddl:sequenceGeneratorIncrementByOption">
		<xsl:text> INCREMENT BY </xsl:text>
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="ddl:sequenceGeneratorMaxValueOption">
		<xsl:text>MAX VALUE </xsl:text>
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="ddl:sequenceGeneratorMinValueOption">
		<xsl:text>MIN VALUE </xsl:text>
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="ddl:sequenceGeneratorCycleOption">
		<xsl:if test="not(@shouldCycle)">
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

	<xsl:template match="@precision" mode="ForNumeric">
		<xsl:value-of select="$LeftParen"/>
		<xsl:value-of select="."/>
		<xsl:if test="not(string-length(parent::*/@scale))">
			<xsl:value-of select="$RightParen"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="@scale" mode="ForNumeric">
		<xsl:text>, </xsl:text>
		<xsl:value-of select="."/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="@characterSet" mode="ForColumnDataType">
		<xsl:text> CHARACTER SET </xsl:text>
		<xsl:value-of select="."/>
	</xsl:template>

	<xsl:template match="@collate" mode="ForColumnDataType">
		<xsl:text> COLLATE </xsl:text>
		<xsl:value-of select="."/>
	</xsl:template>

	<xsl:template match="ddt:exactNumericLiteral | ddt:approximateNumericLiteral | ddt:booleanLiteral">
		<xsl:value-of select="@value"/>
	</xsl:template>

	<xsl:template match="ddt:characterStringLiteral">
		<xsl:text>'</xsl:text>
		<xsl:value-of select="@value"/>
		<xsl:text>'</xsl:text>
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

	<xsl:template match="ddt:timestampLiteral">
		<xsl:text>TIMESTAMP '</xsl:text>
		<xsl:value-of select="translate(@value,'T',' ')"/>
		<xsl:text>'</xsl:text>
	</xsl:template>

	<xsl:template match="dep:currentDateKeyword">
		<xsl:text>CURRENT_DATE </xsl:text>
	</xsl:template>

	<xsl:template match="dep:currentTimeKeyword">
		<xsl:text>CURRENT_TIME</xsl:text>
		<xsl:if test="string-length(@precision)">
			<xsl:value-of select="$LeftParen"/>
			<xsl:value-of select="@precision"/>
			<xsl:value-of select="$RightParen"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="dep:currentTimestampKeyword">
		<xsl:text>CURRENT_TIMESTAMP</xsl:text>
		<xsl:if test="string-length(@precision)">
			<xsl:value-of select="$LeftParen"/>
			<xsl:value-of select="@precision"/>
			<xsl:value-of select="$RightParen"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="dep:currentLocalTimeKeyword">
		<xsl:text>LOCALTIME</xsl:text>
		<xsl:if test="string-length(@precision)">
			<xsl:value-of select="$LeftParen"/>
			<xsl:value-of select="@precision"/>
			<xsl:value-of select="$RightParen"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="dep:currentLocalTimestampKeyword">
		<xsl:text>LOCALTIMESTAMP</xsl:text>
		<xsl:if test="string-length(@precision)">
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
		<xsl:apply-templates/>
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

	<xsl:template match="ddt:characterStringLiteral">
		<xsl:text>'</xsl:text>
		<xsl:value-of select="@value"/>
		<xsl:text>'</xsl:text>
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

	<xsl:template match="dep:sqlParameterReference | dep:columnReference">
		<xsl:value-of select="@name"/>
		<xsl:if test="not(position()=last())">
			<xsl:text>, </xsl:text>
		</xsl:if>
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
		<xsl:apply-templates select="@catalog" mode="ForTableDefinition"/>
		<xsl:apply-templates select="@schema" mode="ForTableDefinition"/>
		<xsl:apply-templates select="@name" mode="ForTableDefinition"/>
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

	<xsl:template match="dep:comparisonPredicate">
		<xsl:apply-templates select="child::*[1]"/>
		<xsl:apply-templates select="@operator"/>
		<xsl:apply-templates select="child::*[2]"/>
	</xsl:template>

	<xsl:template match="dep:betweenPredicate">
		<xsl:apply-templates select="child::*[1]"/>
		<xsl:text> </xsl:text>
		<xsl:value-of select="@type"/>
		<xsl:value-of select="@symmetry"/>
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
				<xsl:value-of select="$NewLine"/>
			</xsl:if>
		</xsl:for-each>
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
		<xsl:if test="@schema">
			<xsl:value-of select="@schema"/>
			<xsl:text>.</xsl:text>
		</xsl:if>
		<xsl:value-of select="@name"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$LeftParen"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:apply-templates select="ddl:sqlParameterDeclaration" />
		<xsl:value-of select="$RightParen"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:apply-templates select="ddl:sqlRoutineSpec" />
		<xsl:value-of select="$StatementDelimeter"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<xsl:template match="ddl:sqlParameterDeclaration">
		<xsl:value-of select="$IndentChar" />
		<xsl:value-of select="@name"/>
		<xsl:text> </xsl:text>
		<xsl:apply-templates select="child::*" />
		<xsl:if test="not(position()=last())">
			<xsl:text>, </xsl:text>
		</xsl:if>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<xsl:template match="ddl:sqlRoutineSpec">
		<xsl:text>AS</xsl:text>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$IndentChar" />
		<xsl:apply-templates select="child::*" />
	</xsl:template>

	<xsl:template match="dml:insertStatement">
		<xsl:text>INSERT INTO </xsl:text>
		<xsl:if test="@schema">
			<xsl:value-of select="@schema"/>
			<xsl:text>.</xsl:text>
		</xsl:if>
		<xsl:value-of select="@name"/>
		<xsl:apply-templates select="child::*" />
	</xsl:template>


	<xsl:template match="dml:updateStatement">
		<xsl:text>UPDATE </xsl:text>
		<xsl:if test="@schema">
			<xsl:value-of select="@schema"/>
			<xsl:text>.</xsl:text>
		</xsl:if>
		<xsl:value-of select="@name"/>
		<xsl:value-of select="$NewLine" />
		<xsl:apply-templates select="dml:setClause"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:apply-templates select="dml:whereClause" />
	</xsl:template>

	<xsl:template match="dml:setClause">
		<xsl:choose>
			<xsl:when test="not(position()> 1)">
				<xsl:text>SET </xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>, </xsl:text>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:value-of select="ddl:column/@name"/>
		<xsl:text> </xsl:text>
		<xsl:value-of select="$setClauseEqualsOperator" />
		<xsl:text> </xsl:text>
		<xsl:value-of select="dep:sqlParameterReference/@name"/>

	</xsl:template>

	<xsl:template match="dml:fromConstructor">
		<xsl:value-of select="$LeftParen" />
		<xsl:apply-templates select="ddl:column"/>
		<xsl:value-of select="$RightParen" />
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$IndentChar" />
		<xsl:text>VALUES </xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates select="dep:sqlParameterReference"/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="dml:deleteStatement">
		<xsl:text>DELETE FROM </xsl:text>
		<xsl:if test="@schema">
			<xsl:value-of select="@schema"/>
			<xsl:text>.</xsl:text>
		</xsl:if>
		<xsl:value-of select="@name" />
		<xsl:value-of select="$NewLine"/>
		<xsl:apply-templates select="child::*"/>
	</xsl:template>

	<xsl:template match="dml:whereClause">
		<xsl:value-of select="$IndentChar" />
		<xsl:text>WHERE </xsl:text>
		<xsl:apply-templates select="child::*" />
	</xsl:template>

	<xsl:template match="dml:searchCondition">
		<xsl:apply-templates select="child::*" />
	</xsl:template>

	<!-- End of Stored Procedure Definitions -->

	<!-- Trigger Definitions-->
	
	<xsl:template match="ddl:triggerDefinition">
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:text>CREATE TRIGGER </xsl:text>
		<xsl:if test="@schema">
			<xsl:value-of select="@schema"/>
			<xsl:text>.</xsl:text>
		</xsl:if>
		<xsl:value-of  select="@name"/>
		<xsl:text> </xsl:text>
		<xsl:value-of  select="@actionTime"/>
		<xsl:text> </xsl:text>
		<xsl:call-template name="triggerEvent">
			<xsl:with-param name="event" select="ddl:event" />
		</xsl:call-template>
		<xsl:value-of select="$NewLine"/>
		<xsl:text>ON </xsl:text>
		<xsl:value-of select="ddl:table/@name"/>
		<xsl:apply-templates select="child::*"/>
	</xsl:template>

	<xsl:template name="triggerEvent">
		<xsl:param name="event" select="." />
		<xsl:choose>
			<xsl:when test="$event/@type='UPDATE'">
				<xsl:text>UPDATE </xsl:text>
				<xsl:if test="count($event/ddl:column)>0">
					<xsl:text>OF </xsl:text>
					<xsl:apply-templates select="$event/ddl:column" />
				</xsl:if>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$event/@type"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="ddl:when">
		<xsl:text>WHEN </xsl:text>
		<xsl:apply-templates select="child::*" />
	</xsl:template>

	<xsl:template match="ddl:referencing">
		<xsl:value-of select="$NewLine"/>
		<xsl:text>REFERENCING </xsl:text>
		<xsl:apply-templates select="child::*" />
	</xsl:template>

	<xsl:template match="ddl:newRow">
		<xsl:text>NEW ROW AS </xsl:text>
		<xsl:value-of select="@name"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>
	<xsl:template match="ddl:oldRow">
		<xsl:text>OLD ROW AS </xsl:text>
		<xsl:value-of select="@name"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>
	<xsl:template match="ddl:newTable">
		<xsl:text>NEW TABLE AS </xsl:text>
		<xsl:value-of select="@name"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>
	<xsl:template match="ddl:oldTable">
		<xsl:text>OLD TABLE AS </xsl:text>
		<xsl:value-of select="@name"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<xsl:template match="ddl:atomicBlock">
		<xsl:value-of select="$NewLine"/>
		<xsl:text>BEGIN ATOMIC </xsl:text>
		<xsl:value-of select="$NewLine"/>
		<xsl:apply-templates select="child::*" />
		<xsl:text>; </xsl:text>
		<xsl:value-of select="$NewLine"/>
		<xsl:text>END </xsl:text>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>
	
	<!-- End of Trigger Definitions-->

	<!-- Domain Definition -->

	<xsl:template match="ddl:domainDefinition">
		<xsl:param name="indent"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>CREATE DOMAIN </xsl:text>
		<xsl:if test="@schema">
			<xsl:value-of select="@schema"/>
			<xsl:text>.</xsl:text>
		</xsl:if>
		<xsl:value-of select="@name"/>
		<xsl:text> AS </xsl:text>
		<xsl:apply-templates/>
		<xsl:value-of select="$StatementDelimeter"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<xsl:template match="ddl:domainConstraint">
		<xsl:apply-templates/>
		<xsl:value-of select="@dep:constraintCharacteristics"/>
	</xsl:template>

	<xsl:template match="dep:constraintNameDefinition">
		<xsl:text>CONSTRAINT </xsl:text>
		<xsl:if test="@schema">
			<xsl:value-of select="@schema"/>
			<xsl:text>.</xsl:text>
		</xsl:if>
		<xsl:apply-templates select="@name" mode="ForConstraintNameDefinition"/>
		<xsl:text> </xsl:text>
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="@name" mode="ForConstraintNameDefinition">
		<xsl:value-of select="."/>
	</xsl:template>

	<!-- End of Domain Definition -->

	<!-- Table Constraint Definition -->

	<xsl:template match="ddl:tableConstraintDefinition">
		<xsl:param name="indent"/>
		<xsl:value-of select="$indent"/>
		<xsl:apply-templates/>
		<xsl:if test="not(position()=last())">
			<xsl:text>, </xsl:text>
		</xsl:if>
	</xsl:template>

	<xsl:template match="ddl:uniqueConstraintDefinition">
		<xsl:value-of select="@type"/>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="ddl:column">
		<xsl:value-of select="@name"/>
		<xsl:if test="not(position()=last()) and following-sibling::ddl:column">
			<xsl:text>, </xsl:text>
		</xsl:if>
	</xsl:template>

	<xsl:template match="ddl:referenceColumn">
		<xsl:value-of select="@name"/>
		<xsl:if test="not(position()=last()) and following-sibling::ddl:referenceColumn">
			<xsl:text>, </xsl:text>
		</xsl:if>
	</xsl:template>

	<xsl:template match="ddl:referentialConstraintDefinition">
		<xsl:text>FOREIGN KEY </xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates select="ddl:column"/>
		<xsl:value-of select="$RightParen"/>
		<xsl:text> </xsl:text>
		<xsl:apply-templates select="child::*[not(self::ddl:column)]"/>
	</xsl:template>

	<xsl:template match="ddl:referencesSpecification">
		<xsl:text> REFERENCES </xsl:text>
		<xsl:if test="@schema">
			<xsl:value-of select="@schema"/>
			<xsl:text>.</xsl:text>
		</xsl:if>
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
		<xsl:text> </xsl:text>
	</xsl:template>

	<!-- End of Table Constraint Definition -->

	<!-- Start Transaction Statement -->

	<xsl:template match="dms:startTransactionStatement">
		<xsl:text>START TRANSACTION ISOLATION LEVEL </xsl:text>
		<xsl:value-of select="@isolationLevel"/>
		<xsl:text>, </xsl:text>
		<xsl:value-of select="@accessMode"/>
		<xsl:value-of select="$StatementDelimeter"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<!-- Start Transaction Statement -->

	<!-- Alter Table Statement -->

	<xsl:template match="ddl:alterTableStatement">
		<xsl:text>ALTER TABLE </xsl:text>
		<xsl:if test="@schema">
			<xsl:value-of select="@schema"/>
			<xsl:text>.</xsl:text>
		</xsl:if>
		<xsl:value-of select="@name"/>
		<xsl:text> </xsl:text>
		<xsl:apply-templates>
			<xsl:with-param name="tableName" select="@name"/>
		</xsl:apply-templates>
		<xsl:value-of select="$StatementDelimeter"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>

	<xsl:template match="ddl:addTableConstraintDefinition">
		<xsl:param name="tableName"/>
		<xsl:text>ADD </xsl:text>
		<xsl:apply-templates>
			<xsl:with-param name="tableName" select="$tableName"/>
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="ddl:addColumnDefinition">
		<xsl:text>ADD COLUMN </xsl:text>
		<xsl:apply-templates/>
	</xsl:template>

	<!-- End of Alter Table Statement -->

	<xsl:template name="RenderIdentifier">
		<xsl:param name="name"/>
		<xsl:value-of select="$name"/>
	</xsl:template>


</xsl:stylesheet>
