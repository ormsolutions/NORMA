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
<!-- Contributors: Joshua Arnold -->
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

	<!-- 
		The current version of MySQL (5.1.12) does not support Schemas or Catalogs.
		The following templates are overridden to take no action for schema/catalog-related operations.
	-->
	<xsl:template match="ddl:schemaDefinition"/>
	<xsl:template match="@defaultCharacterSet" mode="ForSchemaDefinition"/>
	<xsl:template match="dms:setSchemaStatement"/>
	<xsl:template match="ddl:generationClause"/>
	<xsl:template match="@catalog" mode="ForTableDefinition"/>
	<xsl:template match="@schema" mode="ForTableDefinition"/>
	<!-- 
		The base SQL Standard Create, Alter, Insert, Update, and Delete From Table templates include a direct call to the Schema and Catalog names. 
		We need to override these templates to exclude these calls. They also use quotation marks that MySQL does not understand. These must converted to the "`" character instead.
	-->
	<xsl:template match="ddl:tableDefinition">
		<xsl:param name="indent"/>
		<xsl:value-of select="$indent"/>
		<xsl:text>CREATE </xsl:text>
		<xsl:text>TABLE </xsl:text>
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
	<xsl:template match="ddl:alterTableStatement">
		<xsl:text>ALTER TABLE </xsl:text>
		<xsl:call-template name="ConvertToMySQLQuotation">
			<xsl:with-param name="name" select="@name"/>
		</xsl:call-template>
		<xsl:value-of select="@name"/>
		<xsl:text> </xsl:text>
		<xsl:apply-templates>
			<xsl:with-param name="tableName" select="@name"/>
		</xsl:apply-templates>
		<xsl:value-of select="$StatementDelimeter"/>
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$NewLine"/>
	</xsl:template>
	<xsl:template match="dml:insertStatement">
		<xsl:text>INSERT INTO </xsl:text>
		<xsl:value-of select="@name"/>
		<xsl:apply-templates select="child::*" />
	</xsl:template>
	<xsl:template match="dml:updateStatement">
		<xsl:text>UPDATE </xsl:text>
		<xsl:value-of select="@name"/>
		<xsl:apply-templates select="child::*" />
	</xsl:template>
	<xsl:template match="dml:deleteStatement">
		<xsl:text>DELETE FROM </xsl:text>
		<xsl:value-of select="@name" />
		<xsl:value-of select="$NewLine"/>
		<xsl:apply-templates select="child::*"/>
	</xsl:template>
	<xsl:template name="RenderIdentifier">
		<xsl:param name="name"/>
		<xsl:call-template name="ConvertToMySQLQuotation">
			<xsl:with-param name="name" select="$name"/>
		</xsl:call-template>
	</xsl:template>
	<!-- End Table template overrides -->
	<!-- 
		The base SQL Standard Reference template includes a direct call to the Schema and Catalog names. 
		We need to override this template to exclude these calls.
	-->
	<xsl:template match="ddl:referencesSpecification">
		<xsl:text> REFERENCES </xsl:text>
		<xsl:call-template name="ConvertToMySQLQuotation">
			<xsl:with-param name="name" select="@name"/>
		</xsl:call-template>
		<xsl:text> </xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates/>
		<xsl:value-of select="$RightParen"/>
		<xsl:text> </xsl:text>
		<xsl:apply-templates select="@match" mode="ForReferenceSpecification"/>
		<xsl:apply-templates select="@onDelete" mode="ForReferenceSpecification"/>
		<xsl:apply-templates select="@onUpdate" mode="ForReferenceSpecification"/>
	</xsl:template>
	<xsl:template match="ddl:referenceColumn">
		<xsl:value-of select="@name"/>
		<xsl:if test="not(position()=last()) and following-sibling::ddl:referenceColumn">
			<xsl:text>, </xsl:text>
		</xsl:if>
	</xsl:template>
	<!-- End Schema template overrides -->

	<!-- 
		The current version of MySQL (5.1.12) does not support Transactions.
		The following templates are overridden to take no action for Transaction-related operations.
	-->
	<xsl:template match="dms:startTransactionStatement"/>
	<xsl:template match="dms:commitStatement"/>
	<!-- End Transaction template overrides -->

	<!-- 
		The current version of MySQL (5.1.12) does not support Domains.
		The following templates are overridden to take no action for Domain-related operations.
	-->
	<xsl:template match="ddl:domainDefinition"/>
	<!-- End Domain template overrides -->
	
	<!-- Template to match Identity columns-->
	<xsl:template match="ddl:columnDefinition[ddl:identityColumnSpecification]">
		<xsl:param name="indent"/>
		<xsl:value-of select="$indent"/>
		<xsl:apply-templates select="@name" mode="ForColumnDefinition"/>
		<xsl:apply-templates select="ddt:boolean | ddt:characterString | ddt:binaryString | ddt:date | ddt:time | ddt:interval | ddt:domain"/>
		<xsl:apply-templates select="ddt:exactNumeric | ddt:approximateNumeric"/>
		<xsl:apply-templates select="ddl:identityColumnSpecification"/>
		<xsl:apply-templates select="ddl:defaultClause"/>
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
	<!-- Template override to correctly display the Identity specification in MySQL -> "AUTO_INCREMENT" -->
	<xsl:template match="ddl:identityColumnSpecification">
		<xsl:text>AUTO_INCREMENT </xsl:text>
	</xsl:template>
	<!-- Template override to generate a MySQL Stored Procedure declaration -->
	<xsl:template match="ddl:sqlInvokedProcedure"/>
	<!-- Template override to generate a MySQL Stored Procedure defintion -->
	<xsl:template match="ddl:sqlRoutineSpec">
		<xsl:value-of select="$IndentChar" />
		<xsl:apply-templates select="child::*" />
	</xsl:template>

	<xsl:template match="dml:fromConstructor">
		<xsl:value-of select="$LeftParen" />
		<xsl:apply-templates select="ddl:column"/>
		<xsl:value-of select="$RightParen" />
		<xsl:value-of select="$NewLine"/>
		<xsl:value-of select="$IndentChar" />
		<xsl:text>VALUES </xsl:text>
		<xsl:value-of select="$LeftParen"/>
		<xsl:apply-templates select="dep:sqlParameterReference" />
		<xsl:value-of select="$RightParen"/>
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
	<!-- 
		Template override to remove MySQL Domain references. 
		The current version of MySQL (5.1.12) does not support Domains and must be declared as the Domain's primitive type
	-->
		<xsl:template match="ddt:domain">
			<xsl:apply-templates select="../../../ddl:domainDefinition[@name = current()/@name]/ddt:characterString"/>
		</xsl:template>
	<!-- 
		Template override to correct Data Types. 
		The current version of MySQL (5.1.12) does not support Domains and must be declared as the Domain's primitive type
	-->
	<xsl:template match="ddt:characterString">
		<xsl:call-template name="ConvertSQLStandardToMySQLDataType">
			<xsl:with-param name="DataType" select="@type"/>
		</xsl:call-template>
		<xsl:value-of select="$LeftParen"/>
		<xsl:value-of select="@length"/>
		<xsl:value-of select="$RightParen"/>
		<xsl:text> </xsl:text>
		<xsl:apply-templates select="@characterSet" mode="ForColumnDataType"/>
		<xsl:apply-templates select="@collate" mode="ForColumnDataType"/>
	</xsl:template>
	<!-- Template to convert SQL Standard Data Types to MySQL-friendly (5.1.12) data types -->
	<xsl:template name="ConvertSQLStandardToMySQLDataType">
		<xsl:param name="DataType"/>
		<xsl:choose>
			<xsl:when test="$DataType='INTEGER'">
				<xsl:text>INT</xsl:text>
			</xsl:when>
			<xsl:when test="$DataType='CHARACTER'">
				<xsl:text>CHAR</xsl:text>
			</xsl:when>
			<xsl:when test="$DataType='CHARACTER VARYING'">
				<xsl:text>VARCHAR</xsl:text>
			</xsl:when>
			<xsl:when test="$DataType='CHARACTER LARGE OBJECT'">
				<xsl:text>TEXT</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$DataType"/>
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