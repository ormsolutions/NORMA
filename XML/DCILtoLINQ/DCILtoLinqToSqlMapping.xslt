﻿<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet
	version="1.0"
	xmlns="http://schemas.microsoft.com/linqtosql/mapping/2007"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:dcl="http://schemas.orm.net/DIL/DCIL"
	xmlns:exsl="http://exslt.org/common"
	xmlns:dep="http://schemas.orm.net/DIL/DILEP"
	extension-element-prefixes="dcl exsl dep"
	>
	<xsl:output indent="yes" method="xml"/>

	<xsl:variable name="DcilSchemaName" select="/dcl:schema/@name"/>
	<xsl:variable name="DcilTables" select="/dcl:schema/dcl:table"/>
	<xsl:variable name="CollectionSuffix" select="'Set'"/>
	<xsl:variable name="TableSuffix" select="'Table'"/>
	<xsl:param name="DataSource" select="'.'"/>
	<xsl:param name="DatabaseName" select="'DatabaseName'"/>
	<xsl:param name="ProjectName" select="'ProjectName'"/>
	<xsl:param name="DataContextSuffix" select="'DataContext'"/>
	<xsl:param name="EntityNamespace" select="$DcilSchemaName"/>
	<xsl:param name="ContextNamespace" select="$DcilSchemaName"/>
	<xsl:param name="AccessModifier"/>
	<xsl:param name="ClassModifier"/>
	<xsl:param name="PrivateMemberPrefix" select="'_'"/>

	<xsl:template match="/">
		<xsl:apply-templates select="dcl:schema"/>
	</xsl:template>

	<xsl:template match="dcl:schema">
		<Database Name="{$DatabaseName}">
			<!-- TODO: Decide what to do with this.
			<xs:attribute name="Provider" type="xs:string" use="optional" />
			<xs:element name="Function" type="Function" minOccurs="0" maxOccurs="unbounded" />
			-->
			<xsl:apply-templates select="dcl:table" mode="GenerateTableXmlMarkup"/>
		</Database>
	</xsl:template>

	<xsl:template match="dcl:table" mode="GenerateTableXmlMarkup">
		<xsl:variable name="tableName" select="@name"/>
		<Table Name="{$DcilSchemaName}.{$tableName}" Member="{$tableName}{$TableSuffix}">
			<Type Name="{$tableName}">
				<!-- TODO: Decide what to do with these.
				<xs:element name="Type" type="Type" minOccurs="0" maxOccurs="unbounded" />
				<xs:attribute name="InheritanceCode" type="xs:string" use="optional" />
				<xs:attribute name="IsInheritanceDefault" type="xs:boolean" use="optional" />
				-->
				<xsl:apply-templates select="child::*" mode="GenerateAbsorbedMembers"/>
				<xsl:apply-templates select="../dcl:table[dcl:referenceConstraint/@targetTable = current()/@name]" mode="GenerateEntitySetMembers">
					<xsl:with-param select="." name="containingTable"/>
				</xsl:apply-templates>
			</Type>
		</Table>
	</xsl:template>

	<xsl:template match="dcl:referenceConstraint" mode="GenerateAbsorbedMembers">
		<Association Name="{@name}" Member="{@targetTable}" Type="{@targetTable}" IsForeignKey="true" Storage="{$PrivateMemberPrefix}{@targetTable}">
			<!-- TODO: Decide what to do with these.
			<xs:attribute name="IsUnique" type="xs:boolean" use="optional" />
			<xs:attribute name="DeleteRule" type="xs:string" use="optional" />
			<xs:attribute name="DeleteOnNull" type="xs:boolean" use="optional" />
			-->
			<xsl:attribute name="ThisKey">
				<xsl:for-each select="dcl:columnRef">
					<xsl:value-of select="@sourceName"/>
					<xsl:if test="position() != last()">
						<xsl:text>,</xsl:text>
					</xsl:if>
				</xsl:for-each>
			</xsl:attribute>
		</Association>
	</xsl:template>

	<xsl:template match="dcl:table" mode="GenerateEntitySetMembers">
		<xsl:param name="containingTable"/>
		<Association Name="{dcl:referenceConstraint[@targetTable = $containingTable/@name]/@name}" Member="{@name}{$CollectionSuffix}" Storage="{$PrivateMemberPrefix}{@name}">
			<!-- TODO: Decide what to do with these.
			<xs:attribute name="IsUnique" type="xs:boolean" use="optional" />
			<xs:attribute name="DeleteRule" type="xs:string" use="optional" />
			<xs:attribute name="DeleteOnNull" type="xs:boolean" use="optional" />
			-->
			<xsl:attribute name="OtherKey">
				<xsl:for-each select="dcl:uniquenessConstraint[@isPrimary = 'true' or @isPrimary = 1]/dcl:columnRef">
					<xsl:value-of select="@name"/>
					<xsl:if test="position() != last()">
						<xsl:text>,</xsl:text>
					</xsl:if>
				</xsl:for-each>
			</xsl:attribute>
		</Association>
	</xsl:template>

	<xsl:template match="dcl:column" mode="GenerateAbsorbedMembers">
		<Column Name="{@name}" >
			<!-- TODO: Decide what to do with these.
			<xs:attribute name="Member" type="xs:string" use="required" />
			<xs:attribute name="UpdateCheck" type="UpdateCheck" use="optional" />
			<xs:attribute name="IsDiscriminator" type="xs:boolean" use="optional" />
			<xs:attribute name="Expression" type="xs:string" use="optional" />
			<xs:attribute name="AutoSync" type="AutoSync" use="optional" />
			-->
			<xsl:apply-templates select="." mode="GetDbType"/>
			<xsl:choose>
				<xsl:when test="@isNullable = 'true' or @isNullable = 1">
					<xsl:attribute name="CanBeNull">
						<xsl:value-of select="true()"/>
					</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="CanBeNull">
						<xsl:value-of select="false()"/>
					</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:if test="../dcl:uniquenessConstraint[@isPrimary = 'true' or @isPrimary = 1]/dcl:columnRef[@name = current()/@name]">
				<xsl:attribute name="IsPrimaryKey">
					<xsl:value-of select="true()"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="@isIdentity = 'true' or @isIdentity = 1">
				<xsl:attribute name="IsDbGenerated">
					<xsl:value-of select="true()"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="dcl:predefinedDataType/@name = 'TIMESTAMP'">
				<xsl:attribute name="IsVersion">
					<xsl:value-of select="true()"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:attribute name="Storage">
				<xsl:value-of select="$PrivateMemberPrefix"/>
				<xsl:value-of select="@name"/>
			</xsl:attribute>
		</Column>
	</xsl:template>

	<xsl:template match="dcl:column" mode="GetDbType">
		<xsl:variable name="predefinedDataTypeName" select="dcl:predefinedDataType/@name"/>
		<xsl:attribute name="DbType">
			<xsl:choose>
				<xsl:when test="dcl:domainRef">
					<xsl:variable name="predefinedDataType" select="../../dcl:domain[@name = current()/dcl:domainRef/@name]/dcl:predefinedDataType"/>
					<xsl:call-template name="GetDbTypeFromDcilPredefinedDataType">
						<xsl:with-param name="predefinedDataType" select="$predefinedDataType"/>
						<xsl:with-param name="column" select="."/>
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:call-template name="GetDbTypeFromDcilPredefinedDataType">
						<xsl:with-param name="predefinedDataType" select="dcl:predefinedDataType"/>
						<xsl:with-param name="column" select="."/>
					</xsl:call-template>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
	</xsl:template>

	<xsl:template name="GetDbTypeFromDcilPredefinedDataType">
		<xsl:param name="predefinedDataType"/>
		<xsl:param name="column"/>
		<xsl:variable name="predefinedDataTypeName" select="$predefinedDataType/@name"/>
		<xsl:choose>
			<xsl:when test="$predefinedDataTypeName = 'CHARACTER'">
				<xsl:text>NChar(</xsl:text>
				<xsl:choose>
					<xsl:when test="string($predefinedDataType/@length)">
						<xsl:value-of select="$predefinedDataType/@length"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="4000"/>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:text>)</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'CHARACTER VARYING'">
				<xsl:text>NVarChar(</xsl:text>
				<xsl:choose>
					<xsl:when test="string($predefinedDataType/@length)">
						<xsl:value-of select="$predefinedDataType/@length"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>Max</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:value-of select="$predefinedDataType/@length"/>
				<xsl:text>)</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'CHARACTER LARGE OBJECT'">
				<xsl:text>NVarChar(</xsl:text>
				<xsl:choose>
					<xsl:when test="string($predefinedDataType/@length)">
						<xsl:value-of select="$predefinedDataType/@length"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>Max</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:value-of select="$predefinedDataType/@length"/>
				<xsl:text>)</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BINARY LARGE OBJECT'">
				<xsl:text>VarBinary(</xsl:text>
				<xsl:value-of select="$predefinedDataType/@length"/>
				<xsl:text>)</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BINARY VARYING'">
				<xsl:text>VarBinary</xsl:text>
				<xsl:if test="string($predefinedDataTypeName/@length)">
					<xsl:text>(</xsl:text>
					<xsl:value-of select="$predefinedDataType/@length"/>
					<xsl:text>)</xsl:text>
				</xsl:if>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BINARY'">
				<xsl:text>Binary</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'NUMERIC'">
				<xsl:text>Numeric</xsl:text>
				<xsl:if test="$predefinedDataType/@precision">
					<xsl:text>(</xsl:text>
					<xsl:value-of select="$predefinedDataType/@precision"/>
					<xsl:if test="$predefinedDataType/@scale">
						<xsl:text>, </xsl:text>
						<xsl:value-of select="$predefinedDataType/@scale"/>
					</xsl:if>
					<xsl:text>)</xsl:text>
				</xsl:if>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'DECIMAL'">
				<xsl:text>Decimal</xsl:text>
				<xsl:if test="$predefinedDataType/@precision">
					<xsl:text>(</xsl:text>
					<xsl:value-of select="$predefinedDataType/@precision"/>
					<xsl:if test="$predefinedDataType/@scale">
						<xsl:text>, </xsl:text>
						<xsl:value-of select="$predefinedDataType/@scale"/>
					</xsl:if>
					<xsl:text>)</xsl:text>
				</xsl:if>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'SMALLINT'">
				<xsl:text>SmallInt</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'TINYINT'">
				<xsl:text>TinyInt</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'INTEGER'">
				<xsl:text>Int</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BIGINT'">
				<xsl:text>BigInt</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'FLOAT'">
				<xsl:text>Float</xsl:text>
				<xsl:if test="string($predefinedDataType/@precision)">
					<xsl:text>(</xsl:text>
					<xsl:value-of select="$predefinedDataType/@precision"/>
					<xsl:text>)</xsl:text>
				</xsl:if>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'REAL'">
				<xsl:text>Real</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'DOUBLE PRECISION'">
				<xsl:text>Float(53)</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BOOLEAN'">
				<xsl:text>Bit</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'DATE'">
				<xsl:text>DateTime</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'DATETIME'">
				<xsl:text>DateTime</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'TIME'">
				<xsl:text>DateTime</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'TIMESTAMP'">
				<xsl:text>DateTime</xsl:text>
				<!--
				This one is wierd in the default mapping in SQL Server where they use a different meaning for Timestamp.
				[Column(Storage="_Region_code", AutoSync=AutoSync.Always, DbType="rowversion", IsDbGenerated=true, IsVersion=true, UpdateCheck=UpdateCheck.Never)]
				public System.Data.Linq.Binary Region_code
				-->
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'INTERVAL'">
				<xsl:text></xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'UNIQUEIDENTIFIER'">
				<xsl:text>UniqueIdentifier</xsl:text>
			</xsl:when>
			<!--
			<xsl:when test="$predefinedDataTypeName = 'DAY'">
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'DAY TO HOUR'">
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'DAY TO MINUTE'">
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'DAY TO SECOND'">
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'HOUR'">
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'HOUR TO MINUTE'">
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'HOUR TO SECOND'">
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'SECOND'">
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'YEAR'">
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'YEAR TO MONTH'">
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'MONTH'">
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'TIMEZONE_HOUR'">
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'TIMEZONE_MINUTE'">
			</xsl:when>
			-->
		</xsl:choose>
		<xsl:if test="$column/@isNullable[.='false' or .='0']">
			<xsl:variable name="identity" select="boolean($column/@isIdentity[.='true' or .='1'])"/>
			<xsl:if test="$identity and $predefinedDataTypeName='UNIQUEIDENTIFIER'">
				<xsl:text> DEFAULT NEWSEQUENTIALID()</xsl:text>
			</xsl:if>
			<xsl:text> NOT NULL</xsl:text>
			<xsl:if test="$identity and ($predefinedDataTypeName='BIGINT' or $predefinedDataTypeName='INTEGER' or $predefinedDataTypeName='SMALLINT')">
				<xsl:text> IDENTITY</xsl:text>
			</xsl:if>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>
