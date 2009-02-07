<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet
	version="1.0"
	xmlns="urn:nhibernate-mapping-2.2"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:dep="http://schemas.orm.net/DIL/DILEP"
	xmlns:dcl="http://schemas.orm.net/DIL/DCIL"
	xmlns:ddt="http://schemas.orm.net/DIL/DILDT"
	xmlns:opt="http://schemas.neumont.edu/ORM/2008-04/NHibernate/Settings"
	xmlns:exsl="http://exslt.org/common"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:fn="urn:functions"
	xmlns:oct="urn:ORMCustomTool:ItemProperties"
	extension-element-prefixes="exsl msxsl fn"
	exclude-result-prefixes="dep dcl ddt opt oct"
	>

	<xsl:output indent="yes" method="xml" encoding="utf-8"/>

	<xsl:param name="DefaultNamespace" select="'MyNamespace'"/>
	<xsl:param name="NHibernateGeneratorSettings" select="document('NHibernateGeneratorSettings.xml')/child::*"/>

	<xsl:variable name="DcilSchemaName" select="string(dcl:schema/@name)"/>
	<xsl:variable name="DcilTables" select="dcl:schema/dcl:table"/>
	<xsl:variable name="ProjectNameFragment">
		<xsl:choose>
			<xsl:when test="function-available('oct:GetProjectProperty')">
				<xsl:value-of select="oct:GetProjectProperty('Title')"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>DCILtoNHibernate</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="ProjectName" select="string($ProjectNameFragment)"/>
	<xsl:variable name="DataSource" select="$NHibernateGeneratorSettings/opt:ConnectionString/@DataSource"/>
	<xsl:variable name="DatabaseNameFragment">
		<xsl:variable name="setting" select="string($NHibernateGeneratorSettings/opt:ConnectionString/@DataBaseName)"/>
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
		<xsl:variable name="setting" select="string($NHibernateGeneratorSettings/opt:NameParts/@DataContextClassSuffix)"/>
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
		<xsl:variable name="setting" select="string($NHibernateGeneratorSettings/opt:NameParts/@CollectionSuffix)"/>
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
		<xsl:variable name="setting" select="string($NHibernateGeneratorSettings/opt:NameParts/@DataContextTableSuffix)"/>
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
		<xsl:variable name="setting" select="string($NHibernateGeneratorSettings/opt:NameParts/@PrivateFieldPrefix)"/>
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
		<xsl:variable name="setting" select="string($NHibernateGeneratorSettings/opt:ConnectionString/@SettingsProperty)"/>
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
	<xsl:variable name="AssemblyNameFragment">
		<xsl:variable name="setting" select="string($NHibernateGeneratorSettings/opt:ConnectionString/@AssemblyName)"/>
		<xsl:choose>
			<xsl:when test="$setting">
				<xsl:value-of select="$setting"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$ProjectName"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="AssemblyName" select="string($AssemblyNameFragment)"/>

	<xsl:template match="/">
		<xsl:apply-templates select="dcl:schema"/>
	</xsl:template>

	<xsl:template match="dcl:schema">
		<xsl:variable name="schema" select="$DcilSchemaName"/>
		<xsl:variable name="assembly" select="$AssemblyName"/>
		<xsl:variable name="namespace" select="$DefaultNamespace"/>
		<xsl:variable name="defaultAccess" select="'property'"/> <!-- field|ClassName-->
		<xsl:variable name="defaultCascade" select="'none'"/> <!-- none|save-update -->
		<xsl:variable name="defaultLazy" select="true()"/>
		<xsl:variable name="autoImport" select="true()"/>
		<!--<xsl:variable name="databaseClass" select="concat($DcilSchemaName,$DataContextSuffix)"/>-->
		<hibernate-mapping xmlns="urn:nhibernate-mapping-2.2" assembly="{$assembly}" namespace="{$namespace}" schema="{$schema}" default-access="{$defaultAccess}" default-cascade="{$defaultCascade}" default-lazy="{$defaultLazy}" auto-import="{$autoImport}">
			<!--TODO: determine whether or not to map unobjectified entities/tables.-->
			<!--<xsl:variable name="TopLevelEntities" select="dcl:table[count(dcl:column) = count(dcl:referenceConstraint)][dcl:uniquenessConstraint[@isPrimary = 'true' or @isPrimary = 1]/count(dcl:columnRef) = ../count(dcl:column)]"/>-->
			<xsl:apply-templates select="dcl:table" mode="GenerateMappingClassElement"/>
		</hibernate-mapping>
	</xsl:template>

	<xsl:template match="dcl:table" mode="GenerateMappingClassElement">
		<xsl:variable name="className" select="fn:pascalName(@name)"/>
		<xsl:variable name="table" select="string(@name)"/>
		<!--<xsl:variable name="name" select="$DefaultNamespace.$className,$AssemblyName"/>-->
		<xsl:variable name="discriminatorValue" select="''"/>
		<xsl:variable name="mutable" select="true()"/>
		<xsl:variable name="schema" select="$DcilSchemaName"/>
		<xsl:variable name="abstract" select="''"/>
		<xsl:variable name="batchSize" select="'1'"/><!-- TODO: determine how to use this. -->
		<xsl:variable name="selectBeforeUpdate" select="false()"/>
		<xsl:variable name="dynamicUpdate" select="false()"/>
		<xsl:variable name="dynamicInsert" select="false()"/>
		<xsl:variable name="polymorphism" select="'implicit'"/><!-- Can be 'explicit'-->
		<xsl:variable name="where" select="''"/><!-- TODO: determine how to use this. -->
		<xsl:variable name="proxy" select="''"/>
		<xsl:variable name="check" select="''"/>
		<xsl:variable name="lazy" select="''"/>
		<xsl:variable name="persister" select="''"/>
		<xsl:variable name="optimisticLock" select="'none'"/>
			<!-- none|version|dirty|all  
			If you enable dynamic-update, you will have a choice of optimistic locking strategies: 
			
			version check the version/timestamp columns 
			all check all columns 
			dirty check the changed columns, allowing some concurrent updates 
			none do not use optimistic locking 
			-->
		
		<class name="{$DefaultNamespace}.{$className},{$AssemblyName}"
			   schema="{$schema}" 
			   table="{$table}" 
			   mutable="{$mutable}" 
			   discriminator-value="{$discriminatorValue}" 
			   abstract="{$abstract}" 
			   batch-size="{$batchSize}" 
			   check="{$check}" 
			   dynamic-insert="{$dynamicInsert}" 
			   dynamic-update="{$dynamicUpdate}" 
			   lazy="{$lazy}" 
			   optimistic-lock="{$optimisticLock}" 
			   persister="{$persister}" 
			   polymorphism="{$polymorphism}" 
			   proxy="{$proxy}" 
			   select-before-update="{$selectBeforeUpdate}" 
			   where="{$where}" 
			   >
			<xsl:apply-templates select="dcl:uniquenessConstraint[@isPrimary = 'true' or @isPrimary = 1]" mode="GeneratePreferredIdentification"/>
			<xsl:apply-templates select="child::*" mode="GenerateAbsorbedMembers"/>
			<!--<xsl:apply-templates select="" mode="GenerateSetProperties"/>-->
		</class>
	</xsl:template>

	<xsl:template match="dcl:uniquenessConstraint" mode="GeneratePreferredIdentification">
		<xsl:variable name="columnRefName" select="string(dcl:columnRef/@name)"/>
		<xsl:variable name="dclColumn" select="../dcl:column[@name = $columnRefName]"/>
		<xsl:variable name="identityPropertyName" select="fn:pascalName($columnRefName)"/>
		<xsl:variable name="identityColumnName" select="$columnRefName"/>
		<xsl:variable name="identityDotNetType">
			<xsl:apply-templates select="$dclColumn" mode="GetDotNetType"/>
		</xsl:variable>
		<xsl:variable name="identityLengthFragment">
			<xsl:apply-templates select="$dclColumn" mode="GetColumnLength"/>
		</xsl:variable>
		<xsl:variable name="identityLength" select="string($identityLengthFragment)"/>
		<xsl:variable name="identityUnsavedValue">
			<!--<xsl:apply-templates select="$identityDotNetType" mode="GetUnsavedValueForType"/>-->
			<!--TODO: switch through the dotNetTypes to determine an appropriate save value.-->
		</xsl:variable>
		<xsl:variable name="identityAccess" select="'property'"/><!-- could be 'field' or a ClassName-->
		
		<!--<composite-id></composite-id> Your persistent class must override equals() and hashCode() to implement composite identifier equality. It must also implements Serializable.-->
		<!--<version/>--><!-- Use if long transactions...?-->
		<!--<timestamp/>-->

		<id name="{$identityPropertyName}" 
			column="{$identityColumnName}" 
			type="{$identityDotNetType}"
			access="{$identityAccess}"
			unsaved-value="{$identityUnsavedValue}">
			<xsl:if test="string-length($identityLength) &gt; 0">
				<xsl:attribute name="length">
					<xsl:value-of select="$identityLength"/>
				</xsl:attribute>
			</xsl:if>
			<!--Choosing the assigned generator makes Hibernate use unsaved-value="undefined",-->
			<xsl:choose>
				<xsl:when test="count(dcl:columnRef) = 1">
					<xsl:attribute name="column">
						<xsl:value-of select="$identityColumnName"/>
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="count(dcl:columnRef) &gt; 1">

				</xsl:when>
			</xsl:choose>
			
			<!--TODO: Decide what to do with the 'unsaved-value' attribute-->
			<generator>
				<xsl:attribute name="class">
					<xsl:variable name ="dclColumnIsIdentity" select="string($dclColumn/@isIdentity)"/>
					
					<xsl:choose>
						<xsl:when test="$dclColumnIsIdentity = 'true' or $dclColumnIsIdentity = 1">
							<xsl:value-of select="'native'"/>
						</xsl:when>
						<!--<xsl:when test="">
							<xsl:value-of select="'increment'"/>
							-->
								<!-- generates identifiers of any integral type that are unique only when no other process is inserting data into the same table. Do not use in a cluster. -->
								<!--
						</xsl:when>
						<xsl:when test="">
							<xsl:value-of select="'identity'"/>
							--><!-- supports identity columns in DB2, MySQL, MS SQL Server, Sybase and HypersonicSQL. The returned identifier is of type long, short or int. --><!--
						</xsl:when>
						<xsl:when test="">
							<xsl:value-of select="'sequence'"/>
							--><!-- uses a sequence in DB2, PostgreSQL, Oracle, SAP DB, McKoi or a generator in Interbase. The returned identifier is of type long, short or int. --><!--
						</xsl:when>
						<xsl:when test="">
							<xsl:value-of select="'hilo'"/>
							--><!-- uses a hi/lo algorithm to efficiently generate identifiers of type long, short or int, given a table and column (by default hibernate_unique_key and next_hi respectively) as a source of hi values. The hi/lo algorithm generates identifiers that are unique only for a particular database. --><!--
						</xsl:when>
						<xsl:when test="">
							<xsl:value-of select="'seqhilo'"/>
							--><!-- uses a hi/lo algorithm to efficiently generate identifiers of type long, short or int, given a named database sequence. -->
							<!--
						</xsl:when>
						<xsl:when test="">
							<xsl:value-of select="'uuid.hex'"/>
							-->
							<!-- uses System.Guid and its ToString(string format) method to generate identifiers of type string. The length of the string returned depends on the configured format. -->
							<!--
						</xsl:when>
						<xsl:when test="">
							<xsl:value-of select="'uuid.string'"/>
							-->
						<!-- uses a new System.Guid to create a byte[] that is converted to a string. -->
							<!--
						</xsl:when>
						<xsl:when test="">
							<xsl:value-of select="'native'"/>
							--><!-- picks identity, sequence or hilo depending upon the capabilities of the underlying database. --><!--
						</xsl:when>
						<xsl:when test="">
							<xsl:value-of select="'assigned'"/>
							--><!-- lets the application to assign an identifier to the object before save() is called. This is the default strategy if no <generator> element is specified. -->
							<!--
						</xsl:when>
						<xsl:when test="">
							<xsl:value-of select="'guid.comb'"/>
							-->
							<!-- uses the algorithm to generate a new System.Guid described by Jimmy Nilsson in the article http://www.informit.com/articles/article.asp?p=25862. -->
							<!--
						</xsl:when>
						<xsl:when test="">
							<xsl:value-of select="'foreign'"/>
							--><!-- uses the identifier of another associated object. Usually used in conjunction with a <one-to-one> primary key association. --><!--
						</xsl:when>
						-->
					</xsl:choose>
				</xsl:attribute>
			</generator>
		</id>
	</xsl:template>

	<xsl:template match="dcl:referenceConstraint" mode="GenerateAbsorbedMembers">
		<xsl:variable name="associationStorage" select="fn:camelName(@targetTable)"/>
		<xsl:variable name="propertyType" select="fn:pascalName(@targetTable)"/>
		<xsl:variable name="propertyName" select="fn:pascalName(@name)"/><!--TODO: add suffix-->
		<xsl:variable name="propertyIsNotNullable" select="not(../../dcl:column[@name = current()/dcl:columnRef/@sourceName]/@isNullable)"/>
		<xsl:variable name="propertyAccess" select="'property'"/><!--field|ClassName-->
		<xsl:variable name="columnName" select="fn:camelName(@name)"/>
		<!--TODO: determine if I need to get a SQL type for each column in the referenced entity.-->
		<xsl:variable name="sqlType">
			<!--<xsl:apply-templates select="" mode="GetDbType"/>-->
		</xsl:variable>
		<xsl:variable name="associationMember" select="@targetTable"/>
		<many-to-one name="{$propertyName}" column="{$columnName}" not-null="{$propertyIsNotNullable}" class="{$DefaultNamespace}.{$propertyType},{$AssemblyName}" access="{$propertyAccess}" >
			<column name="{$columnName}" sql-type="{$sqlType}" check="" index="" length="" not-null="" unique="" unique-key="">
				
			</column>
		</many-to-one>

		<!--<Association Name="{$associationName}" Type="{$associationType}" Member="{$associationMember}" Storage="{$associationStorage}" IsForeignKey="true">
			<xsl:attribute name="ThisKey">
				<xsl:for-each select="dcl:columnRef">
					<xsl:value-of select="translate(translate(concat(translate(substring(@sourceName, 1, 1), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), substring(@sourceName, 2)),'_',''),' ','')"/>
					<xsl:if test="position() != last()">
						<xsl:text>,</xsl:text>
					</xsl:if>
				</xsl:for-each>
			</xsl:attribute>
		</Association>-->
	</xsl:template>

	<!--<xsl:template match="dcl:table" mode="GenerateEntitySetMembers">
		<xsl:param name="containingTable"/>
		<xsl:variable name="associationStorage" select="concat($PrivateMemberPrefix,translate(translate(concat(translate(substring(@name, 1, 1), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), substring(@name, 2)),'_',''),' ',''))"/>
		<xsl:variable name="associationType" select="@name"/>
		<xsl:variable name="associationName" select="dcl:referenceConstraint[@targetTable = $containingTable/@name]/@name"/>
		<xsl:variable name="associationMember" select="concat(@name,$CollectionSuffix)"/>
		<Association Name="{$associationName}" Type="{$associationType}" Member="{$associationMember}" Storage="{$associationStorage}">
			<xsl:attribute name="OtherKey">
				<xsl:for-each select="dcl:uniquenessConstraint[@isPrimary = 'true' or @isPrimary = 1]/dcl:columnRef">
					<xsl:value-of select="translate(translate(concat(translate(substring(@name, 1, 1), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), substring(@name, 2)),'_',''),' ','')"/>
					<xsl:if test="position() != last()">
						<xsl:text>,</xsl:text>
					</xsl:if>
				</xsl:for-each>
			</xsl:attribute>
		</Association>
	</xsl:template>-->

	<xsl:template match="dcl:column" mode="GenerateAbsorbedMembers">
		<xsl:variable name="propertyName" select="fn:pascalName(@name)"/>
		<xsl:variable name="propertyColumnName" select="fn:camelName(@name)"/>
		<xsl:variable name="propertyDotNetType">
			<xsl:apply-templates select="." mode="GetDotNetType"/>
		</xsl:variable>
		<xsl:variable name="propertyLengthFragment">
			<xsl:apply-templates select="." mode="GetColumnLength"/>
		</xsl:variable>
		<xsl:variable name="propertyLength" select="string($propertyLengthFragment)"/>
		<xsl:variable name="propertyIsNotNullable" select="not(@isNullable = 'true' or @isNullable = 1)"/>
		<xsl:variable name="propertyIsUnique" select="../dcl:uniquenessConstraint[@isPrimary = 'false' or @isPrimary = 0]/dcl:columnRef/@name = current()/@name"/>
		<!--<xsl:variable name="propertyIsGenerated" select="@isIdentity = 'true' or @isIdentity = 1"/>-->
		<xsl:variable name="propertyIsGenerated" select="'never'"/>
		<xsl:variable name="propertyAccess" select="'property'"/><!-- could be 'field' or a ClassName.-->
		<xsl:variable name="propertyIsLazy" select="false()"/><!---->
		<xsl:variable name="propertyIncludedInUpdate" select="true()"/>
		<xsl:variable name="propertyIncludedInInsert" select="true()"/>
		<!--<xsl:variable name="propertyFormula" select="''"/>--><!--Derivation Rule-->
		<xsl:variable name="propertyRequiresOptomisticLock" select="true()"/>

		<xsl:variable name="columnName" select="$propertyColumnName"/>
		<xsl:variable name="columnLength" select="$propertyLength"/>
		<xsl:variable name="columnIsUnique" select="$propertyIsUnique"/>
		<xsl:variable name="columnSqlType">
			<xsl:apply-templates select="." mode="GetDbType"/>
		</xsl:variable>
		<xsl:variable name="columnIsNotNullable" select="$propertyIsNotNullable"/>
		<xsl:variable name="columnIsPrimaryKey" select="../dcl:uniquenessConstraint[@isPrimary = 'true' or @isPrimary = 1]/dcl:columnRef[@name = current()/@name]"/>
		<xsl:choose>
			<xsl:when test="not($columnIsPrimaryKey)">
				<property name="{$propertyName}" column="{$propertyColumnName}" type="{$propertyDotNetType}" not-null="{$propertyIsNotNullable}" unique="{$propertyIsUnique}" access="{$propertyAccess}" update="{$propertyIncludedInUpdate}" insert="{$propertyIncludedInInsert}" generated="{$propertyIsGenerated}" optimistic-lock="{$propertyRequiresOptomisticLock}">
					<xsl:if test="string-length($propertyLength) &gt; 0">
						<xsl:attribute name="length">
							<xsl:value-of select="$propertyLength"/>
						</xsl:attribute>
					</xsl:if>
					<column name="{$columnName}" sql-type="{$columnSqlType}" not-null="{$columnIsNotNullable}" unique="{$columnIsUnique}" >
						<xsl:if test="string-length($columnLength) &gt; 0">
							<xsl:attribute name="length">
								<xsl:value-of select="$columnLength"/>
							</xsl:attribute>
						</xsl:if>
					</column>
				</property>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="dcl:column" mode="GetColumnLength">
		<xsl:choose>
			<xsl:when test="dcl:predefinedDataType/@length">
				<xsl:value-of select="string(dcl:predefinedDataType/@length)"/>
			</xsl:when>
			<xsl:when test="dcl:domainRef">
				<!--<xsl:choose>
					<xsl:when test=""
				</xsl:choose>-->
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="dcl:column" mode="GetDbType">
		<xsl:variable name="predefinedDataTypeName" select="dcl:predefinedDataType/@name"/>
		<!--<xsl:attribute name="DbType">-->
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
		<!--</xsl:attribute>-->
	</xsl:template>

	<xsl:template name="GetDbTypeFromDcilPredefinedDataType">
		<xsl:param name="predefinedDataType"/>
		<xsl:param name="column"/>
		<xsl:variable name="predefinedDataTypeName" select="$predefinedDataType/@name"/>
		<xsl:choose>
			<xsl:when test="$predefinedDataTypeName = 'CHARACTER'">
				<xsl:value-of select="'NChar'"/>
				<!--<xsl:text>(</xsl:text>
				<xsl:choose>
					<xsl:when test="string($predefinedDataType/@length)">
						<xsl:value-of select="$predefinedDataType/@length"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="4000"/>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:text>)</xsl:text>-->
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'CHARACTER VARYING'">
				<xsl:value-of select="'NVarChar'"/>
				<!--<xsl:text>(</xsl:text>
				<xsl:choose>
					<xsl:when test="string($predefinedDataType/@length)">
						<xsl:value-of select="$predefinedDataType/@length"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'Max'"/>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:text>)</xsl:text>-->
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'CHARACTER LARGE OBJECT'">
				<xsl:value-of select="'NVarChar'"/>
				<!--<xsl:text>(</xsl:text>
				<xsl:choose>
					<xsl:when test="string($predefinedDataType/@length)">
						<xsl:value-of select="$predefinedDataType/@length"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'Max'"/>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:text>)</xsl:text>-->
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BINARY LARGE OBJECT'">
				<xsl:value-of select="'VarBinary'"/>
				<!--<xsl:text>(</xsl:text>
				<xsl:value-of select="$predefinedDataType/@length"/>
				<xsl:text>)</xsl:text>-->
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BINARY VARYING'">
				<xsl:value-of select="'VarBinary'"/>
				<!--<xsl:if test="string($predefinedDataTypeName/@length)">
					<xsl:text>(</xsl:text>
					<xsl:value-of select="$predefinedDataType/@length"/>
					<xsl:text>)</xsl:text>
				</xsl:if>-->
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BINARY'">
				<xsl:value-of select="'Binary'"/>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'NUMERIC'">
				<xsl:value-of select="'Numeric'"/>
				<!--<xsl:if test="$predefinedDataType/@precision">
					<xsl:text>(</xsl:text>
					<xsl:value-of select="$predefinedDataType/@precision"/>
					<xsl:if test="$predefinedDataType/@scale">
						<xsl:text>, </xsl:text>
						<xsl:value-of select="$predefinedDataType/@scale"/>
					</xsl:if>
					<xsl:text>)</xsl:text>
				</xsl:if>-->
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'DECIMAL'">
				<xsl:value-of select="'Decimal'"/>
				<!--<xsl:if test="$predefinedDataType/@precision">
					<xsl:text>(</xsl:text>
					<xsl:value-of select="$predefinedDataType/@precision"/>
					<xsl:if test="$predefinedDataType/@scale">
						<xsl:text>, </xsl:text>
						<xsl:value-of select="$predefinedDataType/@scale"/>
					</xsl:if>
					<xsl:text>)</xsl:text>
				</xsl:if>-->
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
				<!--<xsl:if test="string($predefinedDataType/@precision)">
					<xsl:text>(</xsl:text>
					<xsl:value-of select="$predefinedDataType/@precision"/>
					<xsl:text>)</xsl:text>
				</xsl:if>-->
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
		<!--<xsl:if test="$column/@isNullable = 'false' or $column/@isNullable = 0">
			<xsl:text> NOT NULL</xsl:text>
			<xsl:if test="$predefinedDataTypeName = 'BIGINT' or $predefinedDataTypeName = 'INTEGER'">
				<xsl:if test="$column/@isIdentity = 'true' or $column/@isIdentity = 1">
					<xsl:text> IDENTITY</xsl:text>
				</xsl:if>
			</xsl:if>
		</xsl:if>-->
	</xsl:template>

	<xsl:template match="dcl:column" mode="GetDotNetType">
		<!--<xsl:attribute name="Type">-->
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
		<!--</xsl:attribute>-->
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

	<msxsl:script implements-prefix="fn" language="CSharp">
		<![CDATA[
		private static readonly object _lockObject = new object();
		private static System.Text.RegularExpressions.Regex _scrubNameRegex;
		public static string pascalName(string name)
		{
			string retVal = scrubName(name);
			if (retVal.Length != 0 && char.IsLower(retVal[0]))
			{
				return retVal.Substring(0, 1).ToUpper() + retVal.Substring(1);
			}
			return retVal;
		}
		public static string camelName(string name)
		{
			string retVal = scrubName(name);
			if (retVal.Length != 0 && char.IsUpper(retVal[0]))
			{
				return retVal.Substring(0, 1).ToLower() + retVal.Substring(1);
			}
			return retVal;
		}
		public static string scrubName(string name)
		{
			System.Text.RegularExpressions.Regex regex;
			if (null == (regex = _scrubNameRegex))
			{
				lock (_lockObject)
				{
					if (null == (regex = _scrubNameRegex))
					{
						_scrubNameRegex = regex = new System.Text.RegularExpressions.Regex("([\"_ ])([^\"_ ]*)", System.Text.RegularExpressions.RegexOptions.Compiled);
					}
				}
			}
			return regex.Replace(
				name,
				delegate(System.Text.RegularExpressions.Match match)
				{
					System.Text.RegularExpressions.GroupCollection groups = match.Groups;
					
					if (groups[1].Value != "\"")
					{
						string trailing = groups[2].Value;
						if (!string.IsNullOrEmpty(trailing) && char.IsLower(trailing[0]))
						{
							return trailing.Substring(0, 1).ToUpper() + trailing.Substring(1);
						}
						return trailing;
					}
					return groups[2].Value;
				});
		}
		public static string bracketQuotedName(string name)
		{
			if (name != null)
			{
				int length = name.Length;
				if (length >= 2 && name[0] == '\"' && name[length - 1] == '\"')
				{
					return "`" + name.Substring(1, length - 2) + "`";
				}
			}
			return name;
		}
		]]>
	</msxsl:script>

</xsl:stylesheet>