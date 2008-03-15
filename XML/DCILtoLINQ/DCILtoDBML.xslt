<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet
	version="1.0"
	xmlns="http://schemas.microsoft.com/linqtosql/dbml/2007"
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

	<xsl:template match="/">
		<xsl:apply-templates select="dcl:schema"/>
	</xsl:template>

	<xsl:template match="dcl:schema">
		<Database Name="{$DatabaseName}" Class="{$DcilSchemaName}{$DataContextSuffix}">
			<Connection Mode="AppSettings" ConnectionString="Data Source={$DataSource};Initial Catalog={$DatabaseName};Integrated Security=True" SettingsObjectName="{$ProjectName}.Properties.Settings" SettingsPropertyName="{$DatabaseName}ConnectionString" Provider="System.Data.SqlClient"/>
			<xsl:apply-templates select="dcl:table" mode="GenerateTableXmlMarkup"/>
		</Database>
	</xsl:template>

	<xsl:template match="dcl:table" mode="GenerateTableXmlMarkup">
		<xsl:variable name="tableName" select="@name"/>
		<Table Name="{$DcilSchemaName}.{$tableName}" Member="{$tableName}{$TableSuffix}">
			<Type Name="{$tableName}">
				<xsl:apply-templates select="child::*" mode="GenerateAbsorbedMembers"/>
				<xsl:apply-templates select="../dcl:table[dcl:referenceConstraint/@targetTable = current()/@name]" mode="GenerateEntitySetMembers">
					<xsl:with-param select="." name="containingTable"/>
				</xsl:apply-templates>
			</Type>
		</Table>
	</xsl:template>

	<xsl:template match="dcl:referenceConstraint" mode="GenerateAbsorbedMembers">
		<Association Name="{@name}" Member="{@targetTable}" Type="{@targetTable}" IsForeignKey="true">
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
		<Association Name="{dcl:referenceConstraint[@targetTable = $containingTable/@name]/@name}" Member="{@name}{$CollectionSuffix}" Type="{@name}">
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
			<xsl:apply-templates select="." mode="GetDbType"/>
			<xsl:apply-templates select="." mode="GetDotNetType"/>
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
				<xsl:value-of select="'Char'"/>
				<xsl:text>(</xsl:text>
				<xsl:value-of select="$predefinedDataType/@length"/>
				<xsl:text>)</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'CHARACTER VARYING'">
				<xsl:value-of select="'NVarChar'"/>
				<xsl:text>(</xsl:text>
				<xsl:value-of select="$predefinedDataType/@length"/>
				<xsl:text>)</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'CHARACTER LARGE OBJECT'">
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BINARY LARGE OBJECT'">
				<!--text, ntext, or image-->
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'NUMERIC'">
				<xsl:value-of select="'Numeric'"/>
				<xsl:text>(</xsl:text>
				<xsl:value-of select="$predefinedDataType/@precision"/>
				<xsl:text>, </xsl:text>
				<xsl:value-of select="$predefinedDataType/@scale"/>
				<xsl:text>)</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'DECIMAL'">
				<xsl:value-of select="'Decimal'"/>
				<xsl:text>(</xsl:text>
				<xsl:value-of select="$predefinedDataType/@precision"/>
				<xsl:text>, </xsl:text>
				<xsl:value-of select="$predefinedDataType/@scale"/>
				<xsl:text>)</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'SMALLINT'">
				<xsl:value-of select="'SmallInt'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'INTEGER'">
				<xsl:value-of select="'Int'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BIGINT'">
				<xsl:value-of select="'BigInt'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'FLOAT'">
				<xsl:value-of select="'Float'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'REAL'">
				<xsl:value-of select="'Real'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'DOUBLE PRECISION'">
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BOOLEAN'">
				<xsl:value-of select="'Bit'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'DATE'">
				<xsl:value-of select="'DateTime'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'TIME'">
				<xsl:value-of select="'DataTime'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'TIMESTAMP'">
				<xsl:value-of select="'RowVersion'"/>
				<!--This one is wierd.
							[Column(Storage="_Region_code", AutoSync=AutoSync.Always, DbType="rowversion", IsDbGenerated=true, IsVersion=true, UpdateCheck=UpdateCheck.Never)]
							public System.Data.Linq.Binary Region_code
							-->
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'INTERVAL'">
				<xsl:value-of select="''"/>
			</xsl:when>
			<!--<xsl:when test="$predefinedDataTypeName = 'DAY'">
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
				</xsl:when>-->
		</xsl:choose>
		<xsl:if test="$column/@isNullable = 'false' or $column/@isNullable = 0">
				<xsl:text> NOT NULL</xsl:text>
				<xsl:if test="$predefinedDataTypeName = 'BIGINT' or $predefinedDataTypeName = 'INTEGER'">
					<xsl:if test="$column/@isIdentity = 'true' or $column/@isIdentity = 1">
						<xsl:text> IDENTITY</xsl:text>
					</xsl:if>
				</xsl:if>
			</xsl:if>
	</xsl:template>
	
	<xsl:template match="dcl:column" mode="GetDotNetType">
		<xsl:attribute name="Type">
			<xsl:choose>
				<xsl:when test="dcl:domainRef">
					<xsl:variable name="domain" select="../../dcl:domain[@name = current()/dcl:domainRef/@name]"/>
					<xsl:call-template name="GetDotNetTypeFromDcilDomain">
						<xsl:with-param name="domain" select="$domain"/>
						<xsl:with-param name="column" select="."/>
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:call-template name="GetDotNetTypeFromDcilPredefinedDataType">
						<xsl:with-param name="predefinedDataType" select="dcl:predefinedDataType"/>
						<xsl:with-param name="column" select="."/>
					</xsl:call-template>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
	</xsl:template>

	<xsl:template name="GetDotNetTypeFromDcilDomain">
		<xsl:param name="domain"/>
		<xsl:param name="column"/>
		<xsl:call-template name="GetDotNetTypeFromDcilPredefinedDataType">
			<xsl:with-param name="predefinedDataType" select="$domain/dcl:predefinedDataType"/>
			<xsl:with-param name="column" select="$column"/>
		</xsl:call-template>
	</xsl:template>
	
	<xsl:template name="GetDotNetTypeFromDcilPredefinedDataType">
		<xsl:param name="predefinedDataType"/>
		<xsl:param name="column"/>
		<xsl:variable name="predefinedDataTypeName" select="$predefinedDataType/@name"/>
		<xsl:value-of select="'System.'"/>
		<xsl:choose>
			<xsl:when test="$predefinedDataTypeName = 'CHARACTER'">
				<xsl:value-of select="'String'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'CHARACTER VARYING'">
				<xsl:value-of select="'String'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'CHARACTER LARGE OBJECT'">
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BINARY LARGE OBJECT'">
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'NUMERIC'">
				<xsl:value-of select="'Decimal'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'DECIMAL'">
				<xsl:value-of select="'Decimal'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'SMALLINT'">
				<xsl:value-of select="'Int16'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'INTEGER'">
				<xsl:value-of select="'Int32'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BIGINT'">
				<xsl:value-of select="'Int64'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'FLOAT'">
				<xsl:value-of select="'Double'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'REAL'">
				<xsl:value-of select="'Single'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'DOUBLE PRECISION'">
				<xsl:value-of select="''"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BOOLEAN'">
				<xsl:value-of select="'Boolean'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'DATE'">
				<xsl:value-of select="'DateTime'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'TIME'">
				<xsl:value-of select="'DateTime'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'TIMESTAMP'">
				<xsl:value-of select="'Byte[]'"/>
				<!--This one is wierd.
							[Column(Storage="_Region_code", AutoSync=AutoSync.Always, DbType="rowversion", IsDbGenerated=true, IsVersion=true, UpdateCheck=UpdateCheck.Never)]
							public System.Data.Linq.Binary Region_code
							-->
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'INTERVAL'">
			</xsl:when>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
