<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet
	version="1.0"
	xmlns="http://schemas.microsoft.com/linqtosql/dbml/2007"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:dcl="http://schemas.orm.net/DIL/DCIL"
	xmlns:exsl="http://exslt.org/common"
	xmlns:dep="http://schemas.orm.net/DIL/DILEP"
	xmlns:opt="http://schemas.neumont.edu/ORM/2008-04/LinqToSql/Settings"
	xmlns:oct="urn:ORMCustomTool:ItemProperties" 
	exclude-result-prefixes="dcl dep opt"
	extension-element-prefixes="exsl oct"
	>
	<xsl:output indent="yes" method="xml"/>

	<xsl:param name="LinqToSqlSettings" select="document('LinqToSqlSettings.xml')/child::*"/>
	<xsl:variable name="DcilSchemaName" select="string(dcl:schema/@name)"/>
	<xsl:variable name="DcilTables" select="dcl:schema/dcl:table"/>
	<xsl:variable name="ProjectNameFragment">
		<xsl:choose>
			<xsl:when test="function-available('oct:GetProjectProperty')">
				<xsl:value-of select="oct:GetProjectProperty('Title')"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>ProjectName</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="ProjectName" select="string($ProjectNameFragment)"/>
	<xsl:variable name="DataSource" select="$LinqToSqlSettings/opt:ConnectionString/@DataSource"/>
	<xsl:variable name="DatabaseNameFragment">
		<xsl:variable name="setting" select="string($LinqToSqlSettings/opt:ConnectionString/@DataBaseName)"/>
		<xsl:choose>
			<xsl:when test="$setting">
				<xsl:value-of select="$setting"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$DcilSchemaName"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="DatabaseName" select="string($DatabaseNameFragment)"/>
	<xsl:variable name="DataContextSuffixFragment">
		<xsl:variable name="setting" select="string($LinqToSqlSettings/opt:NameParts/@DataContextClassSuffix)"/>
		<xsl:choose>
			<xsl:when test="$setting">
				<xsl:value-of select="$setting"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>DataContext</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="DataContextSuffix" select="string($DataContextSuffixFragment)"/>
	<xsl:variable name="CollectionSuffixFragment">
		<xsl:variable name="setting" select="string($LinqToSqlSettings/opt:NameParts/@CollectionSuffix)"/>
		<xsl:choose>
			<xsl:when test="$setting">
				<xsl:value-of select="$setting"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>Collection</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="CollectionSuffix" select="string($CollectionSuffixFragment)"/>
	<xsl:variable name="TableSuffixFragment">
		<xsl:variable name="setting" select="string($LinqToSqlSettings/opt:NameParts/@DataContextTableSuffix)"/>
		<xsl:choose>
			<xsl:when test="$setting">
				<xsl:value-of select="$setting"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>Table</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="TableSuffix" select="string($TableSuffixFragment)"/>
	<xsl:variable name="PrivateMemberPrefixFragment">
		<xsl:variable name="setting" select="string($LinqToSqlSettings/opt:NameParts/@PrivateFieldPrefix)"/>
		<xsl:choose>
			<xsl:when test="$setting">
				<xsl:value-of select="$setting"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>_</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="PrivateMemberPrefix" select="string($PrivateMemberPrefixFragment)"/>
	<xsl:variable name="SettingsPropertyNameFragment">
		<xsl:variable name="setting" select="string($LinqToSqlSettings/opt:ConnectionString/@SettingsProperty)"/>
		<xsl:choose>
			<xsl:when test="$setting">
				<xsl:value-of select="$setting"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="concat($DatabaseName,'ConnectionString')"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="SettingsPropertyName" select="string($SettingsPropertyNameFragment)"/>
	<xsl:variable name="EntityNamespace" select="$DcilSchemaName"/>
	<xsl:variable name="ContextNamespace" select="$DcilSchemaName"/>
	<xsl:variable name="AccessModifier"/>
	<xsl:variable name="ClassModifier"/>

	<xsl:template match="/">
		<xsl:apply-templates select="dcl:schema"/>
	</xsl:template>

	<xsl:template match="dcl:schema">
		<xsl:variable name="databaseClass" select="concat($DcilSchemaName,$DataContextSuffix)"/>
		<Database Name="{$DatabaseName}" Class="{$databaseClass}" EntityNamespace="{$EntityNamespace}" ContextNamespace="{$ContextNamespace}">
			<!-- TODO: Decide what to do with these.
			<xs:attribute name="AccessModifier" type="AccessModifier" use="optional" />
			<xs:attribute name="Modifier" type="ClassModifier" use="optional" />
			<xs:attribute name="BaseType" type="xs:string" use="optional" />
			<xs:attribute name="Provider" type="xs:string" use="optional" />
			<xs:attribute name="ExternalMapping" type="xs:boolean" use="optional" />
			<xs:attribute name="Serialization" type="SerializationMode" use="optional" />
			<xs:attribute name="EntityBase" type="xs:string" use="optional" />
			-->
			<Connection Mode="AppSettings" SettingsObjectName="{$ProjectName}.Properties.Settings" SettingsPropertyName="{$SettingsPropertyName}" Provider="System.Data.SqlClient"/>
			<xsl:apply-templates select="dcl:table" mode="GenerateTableXmlMarkup"/>
		</Database>
	</xsl:template>

	<xsl:template match="dcl:table" mode="GenerateTableXmlMarkup">
		<xsl:variable name="tableName" select="@name"/>
		<Table Name="{$DcilSchemaName}.{$tableName}" Member="{$tableName}{$TableSuffix}">
			<!-- TODO: Decide what to do with these.
			<xs:attribute name="AccessModifier" type="AccessModifier" use="optional" />
			<xs:attribute name="Modifier" type="MemberModifier" use="optional" />
			-->
			<Type Name="{$tableName}">
				<!-- TODO: Decide what to do with these.
				<xs:element name="Type" type="Type" minOccurs="0" maxOccurs="unbounded" />
				<xs:attribute name="IdRef" type="xs:IDREF" use="optional" />
				<xs:attribute name="Id" type="xs:ID" use="optional" />
				<xs:attribute name="InheritanceCode" type="xs:string" use="optional" />
				<xs:attribute name="IsInheritanceDefault" type="xs:boolean" use="optional" />
				<xs:attribute name="AccessModifier" type="AccessModifier" use="optional" />
				<xs:attribute name="Modifier" type="ClassModifier" use="optional" />
				-->
				<xsl:apply-templates select="child::*" mode="GenerateAbsorbedMembers"/>
				<xsl:apply-templates select="../dcl:table[dcl:referenceConstraint/@targetTable = current()/@name]" mode="GenerateEntitySetMembers">
					<xsl:with-param select="." name="containingTable"/>
				</xsl:apply-templates>
			</Type>
		</Table>
	</xsl:template>

	<xsl:template match="dcl:referenceConstraint" mode="GenerateAbsorbedMembers">
		<xsl:variable name="associationStorage" select="concat($PrivateMemberPrefix,translate(translate(concat(translate(substring(@targetTable, 1, 1), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), substring(@targetTable, 2)),'_',''),' ',''))"/>
		<xsl:variable name="associationType" select="@targetTable"/>
		<xsl:variable name="associationName" select="@name"/>
		<xsl:variable name="associationMember" select="@targetTable"/>
		<Association Name="{$associationName}" Type="{$associationType}" Member="{$associationMember}" Storage="{$associationStorage}" IsForeignKey="true">
			<!-- TODO: Decide what to do with these.
			<xs:attribute name="AccessModifier" type="AccessModifier" use="optional" />
			<xs:attribute name="Modifier" type="MemberModifier" use="optional" />
			<xs:attribute name="Cardinality" type="Cardinality" use="optional" />
			<xs:attribute name="DeleteRule" type="xs:string" use="optional" />
			<xs:attribute name="DeleteOnNull" type="xs:boolean" use="optional" />
			-->
			<xsl:attribute name="ThisKey">
				<xsl:for-each select="dcl:columnRef">
					<xsl:value-of select="translate(translate(concat(translate(substring(@sourceName, 1, 1), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), substring(@sourceName, 2)),'_',''),' ','')"/>
					<xsl:if test="position() != last()">
						<xsl:text>,</xsl:text>
					</xsl:if>
				</xsl:for-each>
			</xsl:attribute>
		</Association>
	</xsl:template>

	<xsl:template match="dcl:table" mode="GenerateEntitySetMembers">
		<xsl:param name="containingTable"/>
		<xsl:variable name="associationStorage" select="concat($PrivateMemberPrefix,translate(translate(concat(translate(substring(@name, 1, 1), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), substring(@name, 2)),'_',''),' ',''))"/>
		<xsl:variable name="associationType" select="@name"/>
		<xsl:variable name="associationName" select="dcl:referenceConstraint[@targetTable = $containingTable/@name]/@name"/>
		<xsl:variable name="associationMember" select="concat(@name,$CollectionSuffix)"/>
		<Association Name="{$associationName}" Type="{$associationType}" Member="{$associationMember}" Storage="{$associationStorage}">
			<!-- TODO: Decide what to do with these.
			<xs:attribute name="AccessModifier" type="AccessModifier" use="optional" />
			<xs:attribute name="Modifier" type="MemberModifier" use="optional" />
			<xs:attribute name="Cardinality" type="Cardinality" use="optional" />
			<xs:attribute name="DeleteRule" type="xs:string" use="optional" />
			<xs:attribute name="DeleteOnNull" type="xs:boolean" use="optional" />
			-->
			<xsl:attribute name="OtherKey">
				<xsl:for-each select="dcl:uniquenessConstraint[@isPrimary = 'true' or @isPrimary = 1]/dcl:columnRef">
					<xsl:value-of select="translate(translate(concat(translate(substring(@name, 1, 1), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), substring(@name, 2)),'_',''),' ','')"/>
					<xsl:if test="position() != last()">
						<xsl:text>,</xsl:text>
					</xsl:if>
				</xsl:for-each>
			</xsl:attribute>
		</Association>
	</xsl:template>

	<xsl:template match="dcl:column" mode="GenerateAbsorbedMembers">
		<xsl:variable name="columnName" select="@name"/>
		<xsl:variable name="columnCanBeNull" select="@isNullable = 'true' or @isNullable = 1"/>
		<xsl:variable name="columnIsPrimaryKey" select="../dcl:uniquenessConstraint[@isPrimary = 'true' or @isPrimary = 1]/dcl:columnRef[@name = current()/@name]"/>
		<xsl:variable name="columnIsDbGenerated" select="@isIdentity = 'true' or @isIdentity = 1"/>
		<xsl:variable name="columnMember" select="translate(translate(concat(translate(substring(@name, 1, 1), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), substring(@name, 2)),'_',''),' ','')"/>
		<xsl:variable name="columnStorage" select="concat($PrivateMemberPrefix,translate(@name,'_',''))"/>
		<Column Name="{@name}">
			<!-- TODO: Decide what to do with these.
			<xs:attribute name="Member" type="xs:string" use="optional" />
			<xs:attribute name="AccessModifier" type="AccessModifier" use="optional" />
			<xs:attribute name="Modifier" type="MemberModifier" use="optional" />
			<xs:attribute name="IsReadOnly" type="xs:boolean" use="optional" />
			<xs:attribute name="UpdateCheck" type="UpdateCheck" use="optional" />
			<xs:attribute name="IsDiscriminator" type="xs:boolean" use="optional" />
			<xs:attribute name="Expression" type="xs:string" use="optional" />
			<xs:attribute name="IsDelayLoaded" type="xs:boolean" use="optional" />
			<xs:attribute name="AutoSync" type="AutoSync" use="optional" />
			-->
			<xsl:apply-templates select="." mode="GetDotNetType"/>
			<xsl:attribute name="Member">
				<xsl:value-of select="$columnMember"/>
			</xsl:attribute>
			<xsl:attribute name="Storage">
				<xsl:value-of select="$columnStorage"/>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="$columnCanBeNull">
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
			<xsl:apply-templates select="." mode="GetDbType"/>
			<xsl:if test="$columnIsPrimaryKey">
				<xsl:attribute name="IsPrimaryKey">
					<xsl:value-of select="true()"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="$columnIsDbGenerated">
				<xsl:attribute name="IsDbGenerated">
					<xsl:value-of select="true()"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="dcl:predefinedDataType/@name = 'TIMESTAMP'">
				<xsl:attribute name="IsVersion">
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
				<xsl:value-of select="'NChar'"/>
				<xsl:text>(</xsl:text>
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
				<xsl:value-of select="'NVarChar'"/>
				<xsl:text>(</xsl:text>
				<xsl:choose>
					<xsl:when test="string($predefinedDataType/@length)">
						<xsl:value-of select="$predefinedDataType/@length"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'Max'"/>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:text>)</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'CHARACTER LARGE OBJECT'">
				<xsl:value-of select="'NVarChar'"/>
				<xsl:text>(</xsl:text>
				<xsl:choose>
					<xsl:when test="string($predefinedDataType/@length)">
						<xsl:value-of select="$predefinedDataType/@length"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'Max'"/>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:text>)</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BINARY LARGE OBJECT'">
				<xsl:value-of select="'VarBinary'"/>
				<xsl:text>(</xsl:text>
				<xsl:value-of select="$predefinedDataType/@length"/>
				<xsl:text>)</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BINARY VARYING'">
				<xsl:value-of select="'VarBinary'"/>
				<xsl:if test="string($predefinedDataTypeName/@length)">
					<xsl:text>(</xsl:text>
					<xsl:value-of select="$predefinedDataType/@length"/>
					<xsl:text>)</xsl:text>
				</xsl:if>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BINARY'">
				<xsl:value-of select="'Binary'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'NUMERIC'">
				<xsl:value-of select="'Numeric'"/>
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
				<xsl:value-of select="'Decimal'"/>
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
				<xsl:value-of select="'SmallInt'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'TINYINT'">
				<xsl:value-of select="'TinyInt'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'INTEGER'">
				<xsl:value-of select="'Int'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BIGINT'">
				<xsl:value-of select="'BigInt'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'FLOAT'">
				<xsl:value-of select="'Float'"/>
				<xsl:if test="string($predefinedDataType/@precision)">
					<xsl:text>(</xsl:text>
					<xsl:value-of select="$predefinedDataType/@precision"/>
					<xsl:text>)</xsl:text>
				</xsl:if>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'REAL'">
				<xsl:value-of select="'Real'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'DOUBLE PRECISION'">
				<xsl:value-of select="'Float(53)'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BOOLEAN'">
				<xsl:value-of select="'Bit'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'DATE'">
				<xsl:value-of select="'DateTime'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'DATETIME'">
				<xsl:value-of select="'DateTime'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'TIME'">
				<xsl:value-of select="'DateTime'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'TIMESTAMP'">
				<xsl:value-of select="'DateTime'"/>
				<!--
				This one is wierd in the default mapping in SQL Server where they use a different meaning for Timestamp.
				[Column(Storage="_Region_code", AutoSync=AutoSync.Always, DbType="rowversion", IsDbGenerated=true, IsVersion=true, UpdateCheck=UpdateCheck.Never)]
				public System.Data.Linq.Binary Region_code
				-->
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'INTERVAL'">
				<xsl:value-of select="''"/>
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
				<xsl:value-of select="'String'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BINARY'">
				<xsl:value-of select="'Byte[]'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BINARY VARYING'">
				<xsl:value-of select="'Byte[]'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BINARY LARGE OBJECT'">
				<xsl:value-of select="'Byte[]'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'NUMERIC'">
				<xsl:value-of select="'Decimal'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'DECIMAL'">
				<xsl:value-of select="'Decimal'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'TINYINT'">
				<xsl:value-of select="'Byte'"/>
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
				<xsl:choose>
					<xsl:when test="string($predefinedDataTypeName/@percision)">
						<xsl:choose>
							<xsl:when test="$predefinedDataTypeName/@percision &lt;= 24">
								<xsl:value-of select="'Single'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'Double'"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'Double'"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'REAL'">
				<xsl:value-of select="'Single'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'DOUBLE PRECISION'">
				<xsl:value-of select="'Double'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BOOLEAN'">
				<xsl:value-of select="'Boolean'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'DATE'">
				<xsl:value-of select="'DateTime'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'DATETIME'">
				<xsl:value-of select="'DateTime'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'TIME'">
				<xsl:value-of select="'DateTime'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'TIMESTAMP'">
				<xsl:value-of select="'DateTime'"/>
				<!--
				This one is wierd.
				[Column(Storage="_Region_code", AutoSync=AutoSync.Always, DbType="rowversion", IsDbGenerated=true, IsVersion=true, UpdateCheck=UpdateCheck.Never)]
				public System.Data.Linq.Binary Region_code
				-->
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'INTERVAL'">
				<xsl:value-of select="'TimeSpan'"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>