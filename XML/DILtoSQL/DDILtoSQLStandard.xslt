<?xml version="1.0" encoding="utf-8"?>
<!--
	Copyright © 2005 Kevin M. Owen, Corey Kaylor, Korvyn Dornseif, and Neumont University

	This software is provided 'as-is', without any express or implied warranty. In no event will the authors be held liable for any damages arising from the use of this software.
	Permission is granted to anyone to use this software for any purpose, including commercial applications, and to alter it and redistribute it freely, subject to the following restrictions:
	1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
	2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
	3. This notice may not be removed or altered from any source distribution.
-->
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
	
	<xsl:output method="text" encoding="utf-8" indent="no" omit-xml-declaration="yes"/>
	<xsl:strip-space elements="*"/>

	<xsl:variable name="Period" select="'.'"/>
	<xsl:variable name="NewLine" select="'&#x0D;&#x0A;'"/>
	<xsl:variable name="IndentChar" select="'&#x09;'"/>
	<xsl:variable name="LeftParen" select="'('"/>
	<xsl:variable name="RightParen" select="')'"/>

	<!-- Schema Definition pg.519 -->

	<xsl:template match="ddl:schemaDefinition">
		<xsl:param name="indent"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>CREATE SCHEMA </xsl:text>
		<xsl:apply-templates select="@catalogName" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="@schemaName" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="@authorizationIdentifier" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="@defaultCharacterSet" mode="ForSchemaDefinition"/>
		<xsl:apply-templates select="ddl:path" mode="ForSchemaDefinition"/>
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
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates select="ddl:columnDefinition">
			<xsl:with-param name="indent" select="concat($NewLine, concat($indent, $IndentChar))"/>
		</xsl:apply-templates>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$indent"/>
		<xsl:value-of select="$RightParen"/>
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
		<xsl:apply-templates select="ddt:boolean" mode="ForColumnDefinition"/>
		<xsl:apply-templates select="ddt:characterString" mode="ForColumnDefinition"/>
		<xsl:apply-templates select="ddt:binaryString" mode="ForColumnDefinition"/>
		<xsl:apply-templates select="ddt:exactNumeric" mode="ForColumnDefinition"/>
		<xsl:apply-templates select="ddt:approximateNumeric" mode="ForColumnDefinition"/>
		<xsl:apply-templates select="ddt:date" mode="ForColumnDefinition"/>
		<xsl:apply-templates select="ddt:time" mode="ForColumnDefinition"/>
		<xsl:apply-templates select="ddt:interval" mode="ForColumnDefinition"/>
		<xsl:apply-templates select="ddl:defaultClause" mode="ForColumnDefinition"/>
	</xsl:template>

	<xsl:template match="ddl:defaultClause" mode="ForColumnDefinition">
		<xsl:text> DEFAULT </xsl:text>
		<xsl:apply-templates select="ddt:dateLiteral" mode="ForDefaultClause"/>
		<xsl:apply-templates select="ddt:characterStringLiteral" mode="ForDefaultClause"/>
		<xsl:apply-templates select="ddt:binaryStringLiteral" mode="ForDefaultClause"/>
		<xsl:apply-templates select="ddt:timeLiteral" mode="ForDefaultClause"/>
		<xsl:apply-templates select="ddt:dayTimeIntervalLiteral" mode="ForDefaultClause"/>
		<xsl:apply-templates select="ddt:timestampLiteral" mode="ForDefaultClause"/>
		<xsl:apply-templates/>
	</xsl:template>

	<xsl:template match="@name" mode="ForColumnDefinition">
		<xsl:call-template name="RenderIdentifier">
			<xsl:with-param name="name" select="."/>
		</xsl:call-template>
		<xsl:text> </xsl:text>
	</xsl:template>

	<xsl:template match="ddt:boolean" mode="ForColumnDefinition">
		<xsl:value-of select="@type"/>
	</xsl:template>

	<xsl:template match="ddt:characterString" mode="ForColumnDefinition">
		<xsl:value-of select="@type"/>
		<xsl:value-of select="$LeftParen"/>
		<xsl:value-of select="@length"/>
		<xsl:value-of select="$RightParen"/>
		<xsl:apply-templates select="@characterSet" mode="ForColumnDataType"/>
		<xsl:apply-templates select="@collate" mode="ForColumnDataType"/>
	</xsl:template>

	<xsl:template match="ddt:binaryString" mode="ForColumnDefinition">
		<xsl:value-of select="@type"/>
		<xsl:value-of select="$LeftParen"/>
		<xsl:value-of select="@length"/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="ddt:exactNumeric" mode="ForColumnDefinition">
		<xsl:value-of select="@type"/>
		<xsl:apply-templates select="@precision" mode="ForExactNumeric"/>
		<xsl:apply-templates select="@scale" mode="ForExactNumeric"/>
	</xsl:template>

	<xsl:template match="ddt:approximateNumeric" mode="ForColumnDefinition">
		<xsl:value-of select="@type"/>
		<xsl:apply-templates select="@precision" mode="ForExactNumeric"/>
		<xsl:value-of select="$RightParen"/>
	</xsl:template>

	<xsl:template match="ddt:date" mode="ForColumnDefinition">
		<xsl:value-of select="@type"/>
	</xsl:template>

	<xsl:template match="ddt:time" mode="ForColumnDefinition">
		<xsl:value-of select="@type"/>		
		<xsl:apply-templates select="@precision" mode="ForTimeDataType"/>
		<xsl:apply-templates select="@zone" mode="ForDateDataType"/>
	</xsl:template>

	<xsl:template match="ddt:interval" mode="ForColumnDefinition">
		<xsl:value-of select="@type"/>
		<xsl:text> </xsl:text>
		<xsl:apply-templates select="@fields" mode="ForIntervalDataType"/>
		<xsl:apply-templates select="@precision" mode="ForIntervalDataType"/>
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

	<xsl:template match="@precision" mode="ForExactNumeric">
		<xsl:value-of select="$LeftParen"/>
		<xsl:value-of select="."/>		
	</xsl:template>

	<xsl:template match="@scale" mode="ForExactNumeric">
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

	<xsl:template match="ddt:characterStringLiteral" mode="ForDefaultClause">
		<xsl:text>U&amp;'</xsl:text>
		<xsl:value-of select="@value"/>
		<xsl:text>'</xsl:text>
	</xsl:template>

	<xsl:template match="ddt:binaryStringLiteral" mode="ForDefaultClause">
		<xsl:text>X'</xsl:text>
		<xsl:value-of select="@value"/>
		<xsl:text>'</xsl:text>
	</xsl:template>

	<xsl:template match="ddt:dayTimeIntervalLiteral" mode="ForDefaultClause">
		<xsl:text>INTERVAL </xsl:text>
		<xsl:text>'</xsl:text>
		<xsl:value-of select="@value"/>
		<xsl:text>' </xsl:text>
		<xsl:value-of select="@fields"/>
	</xsl:template>

	<xsl:template match="ddt:dateLiteral" mode="ForDefaultClause">
		<xsl:text>DATE '</xsl:text>
		<xsl:value-of select="@value"/>
		<xsl:text>'</xsl:text>
	</xsl:template>

	<xsl:template match="ddt:timeLiteral" mode="ForDefaultClause">
		<xsl:text>TIME '</xsl:text>
		<xsl:value-of select="@value"/>
		<xsl:text>'</xsl:text>
	</xsl:template>

	<xsl:template match="ddt:timestampLiteral" mode="ForDefaultClause">
		<xsl:text>TIMESTAMP '</xsl:text>
		<xsl:value-of select="translate(@value,'T',' ')"/>
		<xsl:text>'</xsl:text>
	</xsl:template>

	<!-- End of Column Definition -->

	<xsl:template name="RenderIdentifier">
		<xsl:param name="name"/>
		<xsl:value-of select="."/>		
	</xsl:template>
	
		
</xsl:stylesheet>