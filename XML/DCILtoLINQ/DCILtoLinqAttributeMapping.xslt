﻿<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet
	version="1.0"
	xmlns:dsf="urn:schemas-orm-net:DIL:DILSupportFunctions"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:dep="http://schemas.orm.net/DIL/DILEP"
	xmlns:dcl="http://schemas.orm.net/DIL/DCIL"
	xmlns:ddt="http://schemas.orm.net/DIL/DILDT"
	xmlns:opt="http://schemas.neumont.edu/ORM/2008-04/LinqToSql/Settings"
	xmlns:exsl="http://exslt.org/common"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:fn="urn:functions"
	xmlns:oct="urn:ORMCustomTool:ItemProperties"
	extension-element-prefixes="exsl dsf fn msxsl"
	exclude-result-prefixes="dcl dep ddt oct opt">

	<xsl:output method="xml" encoding="utf-8" indent="yes"/>

	<xsl:param name="LinqToSqlSettings" select="document('LinqToSqlSettings.xml')/child::*"/>

	<xsl:variable name="Model" select="dcl:schema"/>
	<xsl:variable name="ModelName" select="fn:scrubName($Model/@name)"/>
	<xsl:variable name="DatabaseSchemaName" select="string(dcl:schema/@name)"/>
	<xsl:variable name="Entities" select="$Model/dcl:table"/>
	<xsl:variable name="Domains" select="$Model/dcl:domain"/>
	<xsl:variable name="Enumerations" select="$Domains[dcl:predefinedDataType/@name = 'CHARACTER VARYING' or dcl:predefinedDataType/@name = 'CHARACTER' or dcl:predefinedDataType/@name = 'CHARACTER LARGE OBJECT' and count(dcl:checkConstraint) = 1 and count(dcl:checkConstraint/dep:inPredicate) = 1 and dcl:checkConstraint/dep:inPredicate/@type='IN']"/>
	<!--<xsl:variable name="NestedEnumerations" select="$Entities/dcl:checkConstraint[dep:child::*/dep:columnReference = current()../dcl:column]"-->
	<!--<xsl:variable name="ModelNumericConstraints" select="$Model/dcl:domain[dcl:predefinedDataType/@name = 'NUMERIC' or dcl:predefinedDataType/@name = 'INTEGER' or dcl:predefinedDataType/@name = 'DECIMAL' or dcl:predefinedDataType/@name = 'SMALLINT' or dcl:predefinedDataType/@name = 'TINYINT']"/>-->

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
	<xsl:variable name="DatabaseNameFragment">
		<xsl:variable name="setting" select="string($LinqToSqlSettings/opt:ConnectionString/@DatabaseName)"/>
		<xsl:choose>
			<xsl:when test="$setting">
				<xsl:value-of select="$setting"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$ModelName"/>
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
	<xsl:variable name="AssociationReferenceSuffixFragment">
		<xsl:variable name="setting" select="string($LinqToSqlSettings/opt:NameParts/@AssociationReferenceSuffix)"/>
		<xsl:choose>
			<xsl:when test="$setting">
				<xsl:value-of select="$setting"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>Reference</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="AssociationReferenceSuffix" select="string($AssociationReferenceSuffixFragment)"/>
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
	<xsl:variable name="MappingTargetFragment">
		<xsl:apply-templates select="." mode="GetMappingTarget"/>
	</xsl:variable>
	<xsl:variable name="MappingTarget" select="string($MappingTargetFragment)"/>
	<xsl:variable name="GenerateServiceLayerFragment">
		<xsl:variable name="setting" select="string($LinqToSqlSettings/opt:ServiceLayer/@Generate)"/>
		<xsl:choose>
			<xsl:when test="$setting">
				<xsl:value-of select="$setting='true' or $setting='1'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="false()"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="GenerateServiceLayer" select="string($GenerateServiceLayerFragment)='true'"/>
	<xsl:variable name="CreateKeywordFragment">
		<xsl:variable name="setting" select="string($LinqToSqlSettings/opt:ServiceLayer/@CreateKeyword)"/>
		<xsl:choose>
			<xsl:when test="$setting">
				<xsl:value-of select="$setting"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>Insert</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="CreateKeyword" select="string($CreateKeywordFragment)"/>
	<xsl:variable name="ReadKeywordFragment">
		<xsl:variable name="setting" select="string($LinqToSqlSettings/opt:ServiceLayer/@ReadKeyword)"/>
		<xsl:choose>
			<xsl:when test="$setting">
				<xsl:value-of select="$setting"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>Select</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="ReadKeyword" select="string($ReadKeywordFragment)"/>
	<xsl:variable name="UpdateKeywordFragment">
		<xsl:variable name="setting" select="string($LinqToSqlSettings/opt:ServiceLayer/@UpdateKeyword)"/>
		<xsl:choose>
			<xsl:when test="$setting">
				<xsl:value-of select="$setting"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>Update</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="UpdateKeyword" select="string($UpdateKeywordFragment)"/>
	<xsl:variable name="DeleteKeywordFragment">
		<xsl:variable name="setting" select="string($LinqToSqlSettings/opt:ServiceLayer/@DeleteKeyword)"/>
		<xsl:choose>
			<xsl:when test="$setting">
				<xsl:value-of select="$setting"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>Delete</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="DeleteKeyword" select="string($DeleteKeywordFragment)"/>
	<xsl:variable name="PreferredIdFunctionSuffixFragment">
		<xsl:variable name="setting" select="string($LinqToSqlSettings/opt:ServiceLayer/@PreferredIdKeyword)"/>
		<xsl:choose>
			<xsl:when test="$setting">
				<xsl:value-of select="$setting"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>PreferredIdentifier</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="IsPrimaryKeyword" select="string($PreferredIdFunctionSuffixFragment)"/>
	<xsl:variable name="InitializeFunctionNameFragment">
		<xsl:variable name="setting" select="string($LinqToSqlSettings/opt:ServiceLayer/@InitializeFunctionName)"/>
		<xsl:choose>
			<xsl:when test="$setting">
				<xsl:value-of select="$setting"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>Initialize</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="InitializeFunctionName" select="string($InitializeFunctionNameFragment)"/>
	<xsl:variable name="ServiceNameSuffixFragment">
		<xsl:variable name="setting" select="string($LinqToSqlSettings/opt:ServiceLayer/@ServiceNameSuffix)"/>
		<xsl:choose>
			<xsl:when test="$setting">
				<xsl:value-of select="$setting"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>Service</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="ServiceNameSuffix" select="string($ServiceNameSuffixFragment)"/>
	<xsl:variable name="UseTransactionScopesFragment">
		<xsl:variable name="setting" select="string($LinqToSqlSettings/opt:ServiceLayer/@UseTransactionScopes)"/>
		<xsl:choose>
			<xsl:when test="$setting">
				<xsl:value-of select="$setting='true' or $setting='1'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="true()"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="UseTransactionScopes" select="string($UseTransactionScopesFragment)='true'"/>
	<xsl:variable name="UseTransactionFlowFragment">
		<xsl:variable name="setting" select="string($LinqToSqlSettings/opt:ServiceLayer/@UseTransactionFlow)"/>
		<xsl:choose>
			<xsl:when test="$setting">
				<xsl:value-of select="$setting='true' or $setting='1'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="false()"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="UseTransactionFlow" select="string($UseTransactionFlowFragment)='true'"/>
	<xsl:variable name="OptimizeOperationalMethodsFragment">
		<xsl:variable name="setting" select="string($LinqToSqlSettings/opt:ServiceLayer/@OptimizeOperationalMethods)"/>
		<xsl:choose>
			<xsl:when test="$setting">
				<xsl:value-of select="$setting='true' or $setting='1'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="false()"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="OptimizeOperationalMethods" select="string($OptimizeOperationalMethodsFragment)='true'"/>
	<xsl:variable name="InstanceContextModeFragment">
		<xsl:variable name="setting" select="string($LinqToSqlSettings/opt:ServiceLayer/@InstanceContextMode)"/>
		<xsl:choose>
			<xsl:when test="$setting">
				<xsl:value-of select="$setting"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>PerCall</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="InstanceContextMode" select="string($InstanceContextModeFragment)"/>
	<xsl:variable name="UnitTestFrameworkFragment">
		<xsl:variable name="setting" select="string($LinqToSqlSettings/opt:Testing/@UnitTestingFramework)"/>
		<xsl:choose>
			<xsl:when test="$setting">
				<xsl:value-of select="$setting"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>NUnit</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="UnitTestingFramework" select="string($UnitTestFrameworkFragment)"/>

	<xsl:variable name="ServiceName" select="concat('I',$ModelName,$ServiceNameSuffix)"/>

	<xsl:param name="GenerateCodeAnalysisAttributes" select="true()"/>
	<xsl:param name="GenerateAccessedThroughPropertyAttribute" select="false()"/>
	<xsl:param name="GenerateObjectDataSourceSupport" select="true()"/>
	<xsl:param name="RaiseEventsAsynchronously" select="true()"/>
	<xsl:param name="SynchronizeEventAddRemove" select="true()"/>
	<xsl:param name="DefaultNamespace" select="'MyNamespace'"/>
	<xsl:variable name="GenerateLinqAttributes" select="true()"/>
	<xsl:param name="UseXmlMapping" select="$MappingTarget='XmlMapping'"/>
	<xsl:param name="UseAttributeMapping" select="$MappingTarget='AttributeMapping'"/>

	<xsl:template match="*" mode="GetMappingTarget">
		<xsl:text>AttributeMapping</xsl:text>
	</xsl:template>

	<xsl:template match="/">
		<plx:root>
			<plx:namespaceImport name="System"/>
			<plx:namespaceImport name="System.Collections.Generic"/>
			<plx:namespaceImport name="System.ComponentModel"/>
			<plx:namespaceImport name="System.Data"/>
			<plx:namespaceImport name="System.Data.Linq"/>
			<plx:namespaceImport name="System.Data.Linq.Mapping"/>
			<xsl:if test="$GenerateServiceLayer">
				<plx:namespaceImport name="System.Data.SqlClient"/>
			</xsl:if>
			<plx:namespaceImport name="System.Diagnostics.CodeAnalysis"/>
			<plx:namespaceImport name="System.Linq"/>
			<xsl:if test="$GenerateServiceLayer">
				<plx:namespaceImport name="System.Runtime.Serialization"/>
			</xsl:if>
			<plx:namespaceImport name="System.Security.Permissions"/>
			<xsl:if test="$GenerateServiceLayer">
				<plx:namespaceImport name="System.ServiceModel"/>
			</xsl:if>
			<plx:namespaceImport name="System.Threading"/>
			<plx:namespaceImport name="{$ProjectName}.Properties"/>
			<xsl:apply-templates select="dcl:schema" mode="GenerateNamespace"/>
		</plx:root>
		<xsl:if test="function-available('oct:EnsureProjectReference')">
			<xsl:variable name="addedProjectReferenceLinq" select="oct:EnsureProjectReference('System.Data.Linq','System.Data.Linq') and oct:EnsureProjectReference('System.Data','System.Data')"/>
			<xsl:if test="$GenerateServiceLayer">
				<xsl:variable name="addedProjectReferenceServiceLayer" select="oct:EnsureProjectReference('System.Runtime.Serialization','System.Runtime.Serialization') and oct:EnsureProjectReference('System.ServiceModel','System.ServiceModel')"/>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="dcl:schema" mode="GenerateNamespace">
		<xsl:variable name="namespaceName" select="$DefaultNamespace"/>
		<plx:namespace name="{$namespaceName}">
			<xsl:if test="$GenerateServiceLayer">
				<xsl:apply-templates select="." mode="GenerateServiceContract"/>
				<xsl:apply-templates select="." mode="GenerateLinqServiceLayerImplementation"/>
			</xsl:if>
			<xsl:apply-templates select="." mode="GenerateDatabaseContext"/>
			<xsl:apply-templates select="$Enumerations" mode="GenerateEnumerations"/>
			<xsl:apply-templates select="dcl:table" mode="GenerateBusinessEntities">
			</xsl:apply-templates>
			<xsl:call-template name="GenerateGlobalSupportClasses"/>
		</plx:namespace>
	</xsl:template>

	<xsl:template name="GenerateOperationContractAttribute">
		<xsl:param name="isOneWay" select="$OptimizeOperationalMethods"/>
		<plx:attribute dataTypeName="OperationContract">
			<plx:passParam>
				<plx:binaryOperator type="assignNamed">
					<plx:left>
						<plx:nameRef name="IsOneWay"/>
					</plx:left>
					<plx:right>
						<xsl:choose>
							<xsl:when test="$isOneWay">
								<plx:trueKeyword/>
							</xsl:when>
							<xsl:otherwise>
								<plx:falseKeyword/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:right>
				</plx:binaryOperator>
			</plx:passParam>
		</plx:attribute>
	</xsl:template>

	<xsl:template match="dcl:schema" mode="GenerateServiceContract">
		<xsl:if test="$GenerateServiceLayer">
			<plx:interface visibility="public" name="{$ServiceName}">
				<plx:attribute dataTypeName="ServiceContractAttribute"/>
				<xsl:for-each select="dcl:table">
					<xsl:variable name="entityName" select="fn:pascalName(@name)"/>
					<xsl:variable name="pragmaData" select="concat($CreateKeyword,', ',$ReadKeyword,', ',$UpdateKeyword,', and ',$DeleteKeyword,' Operations for ', $entityName)"/>
					<plx:pragma type="region" data="{$pragmaData}"/>
					<xsl:variable name="parameterEntityNamePrefix" select="fn:camelName($entityName)"/>
					<xsl:variable name="to" select="'To'"/>
					<xsl:variable name="old" select="'old'"/>
					<xsl:variable name="new" select="'new'"/>
					<xsl:variable name="createFunctionName" select="concat($CreateKeyword,$entityName)"/>
					<xsl:variable name="createFunctionParameterName" select="concat($parameterEntityNamePrefix,$to,$CreateKeyword)"/>
					<xsl:variable name="updateFunctionName" select="concat($UpdateKeyword,$entityName)"/>
					<xsl:variable name="updateFunctionParameterNameNew" select="concat($new, $entityName)"/>
					<xsl:variable name="updateFunctionParameterNameOld" select="concat($old, $entityName)"/>
					<xsl:variable name="deleteFunctionName" select="concat($DeleteKeyword,$entityName)"/>
					<xsl:variable name="deleteFunctionParameterName" select="concat($parameterEntityNamePrefix,$to,$DeleteKeyword)"/>
					<!--NOTE: readFunctionName & readFunctionParameterName are more complicated and are not defined here.-->
					<plx:function visibility="public" name="{$createFunctionName}">
						<xsl:call-template name="GenerateOperationContractAttribute"/>
						<xsl:if test="$UseTransactionFlow and not($OptimizeOperationalMethods)">
							<plx:attribute dataTypeName="TransactionFlow">
								<plx:passParam>
									<plx:callStatic dataTypeName="TransactionFlowOption" name="Allowed" type="field"/>
								</plx:passParam>
							</plx:attribute>
						</xsl:if>
						<plx:param dataTypeName="{$entityName}" name="{$createFunctionParameterName}"/>
					</plx:function>
					<xsl:for-each select="dcl:uniquenessConstraint">
						<xsl:variable name="isPrimaryUniquenessConstraint" select="@isPrimary = 'true' or @isPrimary = 1"/>
						<plx:function visibility="public">
							<xsl:attribute name="name">
								<xsl:value-of select="concat($ReadKeyword,$entityName,'By')"/>
								<xsl:choose>
									<xsl:when test="$isPrimaryUniquenessConstraint">
										<xsl:value-of select="$IsPrimaryKeyword"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:for-each select="dcl:columnRef">
											<xsl:value-of select="fn:pascalName(@name)"/>
											<xsl:if test="position() != last()">
												<xsl:text>And</xsl:text>
											</xsl:if>
										</xsl:for-each>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:call-template name="GenerateOperationContractAttribute">
								<xsl:with-param name="isOneWay" select="false()"/>
							</xsl:call-template>
							<xsl:for-each select="dcl:columnRef">
								<plx:param name="{fn:camelName(@name)}" >
									<xsl:variable name="column" select="../../dcl:column[@name=current()/@name]"/>
									<xsl:attribute name="dataTypeName">
										<xsl:choose>
											<xsl:when test="$column/dcl:domainRef">
												<xsl:variable name="domainRef" select="$column/dcl:domainRef"/>
												<xsl:choose>
													<xsl:when test="$domainRef[@name = $Enumerations/@name]">
														<xsl:value-of select="translate(translate($column/dcl:domainRef/@name,'_',''),'&quot;','')"/>
													</xsl:when>
													<xsl:when test="$domainRef[@name != $Enumerations/@name or @name != $Enumerations/@name]"></xsl:when>
													<!--<xsl:when test="$domainRef[@name = $NestedEnumerations/@name]">
													</xsl:when>-->
												</xsl:choose>
											</xsl:when>
											<!--<xsl:otherwise>
												<
											</xsl:otherwise>-->

										</xsl:choose>
										<xsl:call-template name="GetDotNetTypeFromDcilPredefinedDataType">
											<xsl:with-param name="column" select="$column"/>
										</xsl:call-template>
									</xsl:attribute>
								</plx:param>
							</xsl:for-each>
							<plx:returns dataTypeName="{$entityName}"/>
						</plx:function>
					</xsl:for-each>
					<plx:function visibility="public" name="{$updateFunctionName}">
						<xsl:call-template name="GenerateOperationContractAttribute"/>
						<xsl:if test="$UseTransactionFlow and not($OptimizeOperationalMethods)">
							<plx:attribute dataTypeName="TransactionFlow">
								<plx:passParam>
									<plx:callStatic dataTypeName="TransactionFlowOption" name="Allowed" type="field"/>
								</plx:passParam>
							</plx:attribute>
						</xsl:if>
						<plx:param dataTypeName="{$entityName}" name="{$updateFunctionParameterNameNew}"/>
						<plx:param dataTypeName="{$entityName}" name="{$updateFunctionParameterNameOld}"/>
					</plx:function>
					<plx:function name="{$deleteFunctionName}" visibility="public">
						<xsl:call-template name="GenerateOperationContractAttribute"/>
						<xsl:if test="$UseTransactionFlow and not($OptimizeOperationalMethods)">
							<plx:attribute dataTypeName="TransactionFlow">
								<plx:passParam>
									<plx:callStatic dataTypeName="TransactionFlowOption" name="Allowed" type="field"/>
								</plx:passParam>
							</plx:attribute>
						</xsl:if>
						<plx:param dataTypeName="{$entityName}" name="{$deleteFunctionParameterName}"/>
					</plx:function>
					<plx:pragma type="closeRegion" data="{$pragmaData}"/>
				</xsl:for-each>
			</plx:interface>
		</xsl:if>
	</xsl:template>

	<xsl:template match="dcl:table" mode="GenerateBusinessEntities">
		<xsl:variable name="tableName" select="@name"/>
		<xsl:variable name="entityName" select="fn:pascalName($tableName)"/>
		<plx:class partial="true" visibility="public" name="{$entityName}">
			<xsl:if test="$GenerateServiceLayer">
				<plx:attribute dataTypeName="DataContract">
					<xsl:variable name="serviceComplexTypeName" select="$entityName"/>
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="Name"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<plx:string data="{$serviceComplexTypeName}"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</plx:attribute>
			</xsl:if>
			<xsl:if test="$GenerateLinqAttributes">
				<plx:attribute dataTypeName="Table">
					<xsl:variable name="schemaQualifiedTableName" select="concat($DatabaseSchemaName,'.',$tableName)"/>
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="Name"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<plx:string data="{$schemaQualifiedTableName}"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</plx:attribute>
			</xsl:if>
			<plx:implementsInterface dataTypeName="INotifyPropertyChanging"/>
			<plx:implementsInterface dataTypeName="INotifyPropertyChanged" />
			<xsl:variable name="entitySetEntities" select="../dcl:table[dcl:referenceConstraint/@targetTable = $tableName]"/>
			<xsl:variable name="generateServiceLayerInitializationFunction" select="$GenerateServiceLayer and $entitySetEntities"/>
			<plx:function visibility="public" name=".construct">
				<xsl:choose>
					<xsl:when test="$generateServiceLayerInitializationFunction">
						<plx:callThis name="{$InitializeFunctionName}"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="$entitySetEntities" mode="GenerateSerivceFieldInitialization"/>
					</xsl:otherwise>
				</xsl:choose>
			</plx:function>
			<xsl:variable name="iNotifyPropertyPragmaData" select="concat('INotifyPropertyChanging and INotifyPropertyChanged Information for ',$entityName)"/>
			<plx:pragma type="region" data="{$iNotifyPropertyPragmaData}"/>
			<xsl:call-template name="GenerateINotifyPropertyChangeImplementation">
				<xsl:with-param name="changeSuffix" select="'ing'"/>
			</xsl:call-template>
			<xsl:call-template name="GenerateINotifyPropertyChangeImplementation">
				<xsl:with-param name="changeSuffix" select="'ed'"/>
			</xsl:call-template>
			<plx:pragma type="closeRegion" data="{$iNotifyPropertyPragmaData}"/>
			<xsl:apply-templates select="child::*" mode="GenerateEntityMembers"/>
			<xsl:variable name="containingEntity" select="."/>
			<xsl:for-each select="$entitySetEntities">
				<xsl:apply-templates select="." mode="GenerateEntitySetMembers">
					<xsl:with-param name="containingEntity" select="$containingEntity"/>
				</xsl:apply-templates>
			</xsl:for-each>
			<xsl:if test="$generateServiceLayerInitializationFunction">
				<xsl:variable name="serializationPragmaData" select="concat('Serialization Information for ',$entityName)"/>
				<plx:pragma type="region" data="{$serializationPragmaData}"/>
				<xsl:variable name="serializationFieldName" select="concat($PrivateMemberPrefix,'serializing')"/>
				<plx:field visibility="private" dataTypeName=".boolean" name="{$serializationFieldName}"/>
				<plx:function visibility="private" name="{$InitializeFunctionName}">
					<xsl:apply-templates select="$entitySetEntities" mode="GenerateSerivceFieldInitialization"/>
				</plx:function>
				<plx:function visibility="private" name="On{$entityName}Serializing">
					<plx:attribute dataTypeName="OnSerializingAttribute"/>
					<plx:attribute dataTypeName="EditorBrowsableAttribute">
						<plx:passParam>
							<plx:callStatic dataTypeName="EditorBrowsableState" name="Never" type="field"/>
						</plx:passParam>
					</plx:attribute>
					<plx:param dataTypeName="StreamingContext" name="context"/>
					<plx:assign>
						<plx:left>
							<plx:callThis name="{$serializationFieldName}" type="field"/>
						</plx:left>
						<plx:right>
							<plx:trueKeyword/>
						</plx:right>
					</plx:assign>
				</plx:function>
				<plx:function visibility="private" name="On{$entityName}Serialized">
					<plx:attribute dataTypeName="OnSerializedAttribute"/>
					<plx:attribute dataTypeName="EditorBrowsableAttribute">
						<plx:passParam>
							<plx:callStatic dataTypeName="EditorBrowsableState" name="Never" type="field"/>
						</plx:passParam>
					</plx:attribute>
					<plx:param dataTypeName="StreamingContext" name="context"/>
					<plx:assign>
						<plx:left>
							<plx:callThis name="{$serializationFieldName}" type="field"/>
						</plx:left>
						<plx:right>
							<plx:falseKeyword/>
						</plx:right>
					</plx:assign>
				</plx:function>
				<plx:function visibility="private" name="On{$entityName}Deserializing">
					<plx:attribute dataTypeName="OnDeserializingAttribute"/>
					<plx:attribute dataTypeName="EditorBrowsableAttribute">
						<plx:passParam>
							<plx:callStatic dataTypeName="EditorBrowsableState" name="Never" type="field"/>
						</plx:passParam>
					</plx:attribute>
					<plx:param dataTypeName="StreamingContext" name="context"/>
					<plx:callThis name="{$InitializeFunctionName}"/>
				</plx:function>
				<plx:pragma type="closeRegion" data="{$serializationPragmaData}"/>
			</xsl:if>
		</plx:class>
	</xsl:template>

	<xsl:template match="dcl:table" mode="GenerateSerivceFieldInitialization">
		<xsl:variable name="entitySetEntityName" select="fn:pascalName(@name)"/>
		<xsl:variable name="entitySetFieldName" select="concat($PrivateMemberPrefix,fn:camelName($entitySetEntityName),$CollectionSuffix)"/>
		<plx:assign>
			<plx:left>
				<plx:callThis type="field" name="{$entitySetFieldName}"/>
			</plx:left>
			<plx:right>
				<plx:callNew dataTypeName="EntitySet">
					<plx:passTypeParam dataTypeName="{$entitySetEntityName}"/>
					<plx:passParam>
						<plx:callNew dataTypeName="Action">
							<plx:passTypeParam dataTypeName="{$entitySetEntityName}"/>
							<plx:passParam>
								<plx:callThis name="On{$entitySetEntityName}Added" type="fireCustomEvent"/>
							</plx:passParam>
						</plx:callNew>
					</plx:passParam>
					<plx:passParam>
						<plx:callNew dataTypeName="Action">
							<plx:passTypeParam dataTypeName="{$entitySetEntityName}"/>
							<plx:passParam>
								<plx:callThis type="fireCustomEvent" name="On{$entitySetEntityName}Removed"/>
							</plx:passParam>
						</plx:callNew>
					</plx:passParam>
				</plx:callNew>
			</plx:right>
		</plx:assign>
	</xsl:template>

	<xsl:template name="GenerateMemberPropertyChangeFields">
		<xsl:param name="changeSuffix" select="'ed'"/>
		<xsl:param name="entityName"/>
		<plx:field visibility="private" static="true" readOnly="true" dataTypeName="PropertyChang{$changeSuffix}EventArgs" name="{$entityName}PropertyChang{$changeSuffix}EventArgs">
			<plx:initialize>
				<plx:callNew dataTypeName="PropertyChang{$changeSuffix}EventArgs">
					<plx:passParam>
						<plx:string data="{$entityName}"/>
					</plx:passParam>
				</plx:callNew>
			</plx:initialize>
		</plx:field>
	</xsl:template>

	<xsl:template match="dcl:table" mode="GenerateEntitySetMembers">
		<xsl:param name="containingEntity"/>
		<xsl:variable name="oppositeEntity" select="."/>
		<xsl:variable name="containingEntityNameFragment">
			<xsl:for-each select="$containingEntity/dcl:uniquenessConstraint[@isPrimary = 'true' or @isPrimary = 1]/dcl:columnRef">
				<xsl:value-of select="fn:pascalName($oppositeEntity/dcl:referenceConstraint/dcl:columnRef[@targetName = current()/@name]/@sourceName)"/>
				<xsl:if test="position() = last()">
					<xsl:value-of select="$AssociationReferenceSuffix"/>
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="containingEntityName" select="fn:pascalName(string($containingEntityNameFragment))"/>
		<xsl:variable name="entityName" select="fn:pascalName(@name)"/>
		<xsl:variable name="entityFieldName" select="concat($PrivateMemberPrefix,fn:camelName($entityName),$CollectionSuffix)"/>
		<xsl:variable name="entityPropertyName" select="concat($entityName,$CollectionSuffix)"/>
		<xsl:variable name="propertyPragmaData" select="concat('PropertyChanging and PropertyChanged Information for ',$entityPropertyName)"/>
		<plx:pragma type="region" data="{$propertyPragmaData}"/>
		<xsl:call-template name="GenerateMemberPropertyChangeFields">
			<xsl:with-param name="changeSuffix" select="'ing'"/>
			<xsl:with-param name="entityName" select="$entityName"/>
		</xsl:call-template>
		<xsl:call-template name="GenerateMemberPropertyChangeFields">
			<xsl:with-param name="changeSuffix" select="'ed'"/>
			<xsl:with-param name="entityName" select="$entityName"/>
		</xsl:call-template>
		<xsl:variable name="entityPropertyChangingEventArgsName" select="concat($entityName,'PropertyChangingEventArgs')"/>
		<xsl:variable name="entityPropertyChangedEventArgsName" select="concat($entityName,'PropertyChangedEventArgs')"/>
		<plx:function visibility="private" name="On{$entityName}Added">
			<plx:param dataTypeName="{$entityName}" name="entity"/>
			<plx:callThis name="OnPropertyChanging">
				<plx:passParam>
					<plx:callThis accessor="static" type="field" name="{$entityPropertyChangingEventArgsName}"/>
				</plx:passParam>
			</plx:callThis>
			<plx:assign>
				<plx:left>
					<plx:callInstance type="property" name="{$containingEntityName}">
						<plx:callObject>
							<plx:nameRef name="entity" type="parameter"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:left>
				<plx:right>
					<plx:thisKeyword/>
				</plx:right>
			</plx:assign>
			<plx:callThis name="OnPropertyChanged">
				<plx:passParam>
					<plx:callThis accessor="static" type="field" name="{$entityPropertyChangedEventArgsName}"/>
				</plx:passParam>
			</plx:callThis>
		</plx:function>
		<plx:function visibility="private" name="On{$entityName}Removed">
			<plx:param dataTypeName="{$entityName}" name="entity"/>
			<plx:callThis name="OnPropertyChanging">
				<plx:passParam>
					<plx:callThis accessor="static" type="field" name="{$entityPropertyChangingEventArgsName}"/>
				</plx:passParam>
			</plx:callThis>
			<plx:assign>
				<plx:left>
					<plx:callInstance type="property" name="{$containingEntityName}">
						<plx:callObject>
							<plx:nameRef name="entity" type="parameter"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:left>
				<plx:right>
					<plx:nullKeyword/>
				</plx:right>
			</plx:assign>
			<plx:callThis name="OnPropertyChanged">
				<plx:passParam>
					<plx:callThis accessor="static" type="field" name="{$entityPropertyChangedEventArgsName}"/>
				</plx:passParam>
			</plx:callThis>
		</plx:function>
		<plx:pragma type="closeRegion" data="{$propertyPragmaData}"/>
		<plx:field visibility="private" dataTypeName="EntitySet" name="{$entityFieldName}">
			<plx:passTypeParam dataTypeName="{$entityName}"/>
		</plx:field>
		<plx:property visibility="public" name="{$entityPropertyName}">
			<xsl:if test="$GenerateServiceLayer">
				<xsl:variable name="serviceComplexTypeName" select="$entityName"/>
				<plx:attribute dataTypeName="DataMember">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="Name"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<plx:string data="{$serviceComplexTypeName}"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="EmitDefaultValue"/>
							</plx:left>
							<plx:right>
								<plx:falseKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</plx:attribute>
			</xsl:if>
			<xsl:if test="$GenerateLinqAttributes">
				<plx:attribute dataTypeName="Association">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="Storage"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<plx:string data="{$entityFieldName}"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="OtherKey"/>
							</plx:left>
							<plx:right>
								<plx:string>
									<!--TODO: Set this via a settings file.-->
									<xsl:for-each select="$containingEntity/dcl:uniquenessConstraint[@isPrimary = 'true' or @isPrimary = 1]/dcl:columnRef">
										<xsl:variable name="oppositeEntityPropertyName"  select="$oppositeEntity/dcl:referenceConstraint/dcl:columnRef[@targetName = current()/@name]/@sourceName"/>
										<xsl:value-of select="fn:pascalName($oppositeEntityPropertyName)"/>
										<xsl:if test="position() != last()">
											<xsl:text>,</xsl:text>
										</xsl:if>
									</xsl:for-each>
								</plx:string>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</plx:attribute>
			</xsl:if>
			<plx:returns dataTypeName="EntitySet">
				<plx:passTypeParam dataTypeName="{$entityName}"/>
			</plx:returns>
			<plx:get>
				<xsl:if test="$GenerateServiceLayer">
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="booleanAnd">
								<plx:left>
									<plx:callThis type="field" name="{$PrivateMemberPrefix}serializing"/>
								</plx:left>
								<plx:right>
									<plx:unaryOperator type="booleanNot">
										<plx:callInstance name="HasLoadedOrAssignedValues" type="property">
											<plx:callObject>
												<plx:callThis type="field" name="{$entityFieldName}"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:unaryOperator>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:return>
							<plx:nullKeyword/>
						</plx:return>
					</plx:branch>
				</xsl:if>
				<plx:return>
					<plx:callThis type="field" name="{$entityFieldName}"/>
				</plx:return>
			</plx:get>
			<plx:set>
				<plx:callInstance name="Assign">
					<plx:callObject>
						<plx:callThis name="{$entityFieldName}" type="field"/>
					</plx:callObject>
					<plx:passParam>
						<plx:valueKeyword/>
					</plx:passParam>
				</plx:callInstance>
			</plx:set>
		</plx:property>
	</xsl:template>

	<xsl:template match="dcl:column" mode="GenerateEntityMembers">
		<xsl:variable name="columnName" select="@name"/>
		<xsl:variable name="columnPropertyName" select="fn:pascalName(@name)"/>
		<xsl:variable name="columnFieldName" select="concat($PrivateMemberPrefix,fn:camelName($columnName))"/>
		<xsl:variable name="columnIsNullable" select="@isNullable = 'true' or @isNullable = 1"/>
		<xsl:variable name="columnIsStringType" select="dcl:predefinedDataType/@name = 'CHARACTER VARYING' or dcl:predefinedDataType/@name = 'CHARACTER' or dcl:predefinedDataType/@name = 'CHARACTER LARGE OBJECT'"/>
		<xsl:variable name="columnIsEnumType" select="dcl:domainRef[@name = $Enumerations/@name]"/>
		<xsl:variable name="columnEnumTypeName" select="fn:pascalName(dcl:domainRef/@name)"/>
		<xsl:variable name="columnIsPartOfPriamryKey" select="../dcl:uniquenessConstraint[@isPrimary='true' or @IsPrimary = 1]/dcl:columnRef[@name = current()/@name]"/>
		<xsl:variable name="columnIsDbGeneratedIdentityColumn" select="@isIdentity = 'true' or @isIdentity = 1"/>
		<xsl:variable name="nullableDotNetTypeName" select="'Nullable'"/>
		<xsl:call-template name="GeneratePropertyChangeInformation">
			<xsl:with-param name="propertyName" select="$columnPropertyName"/>
		</xsl:call-template>
		<plx:field visibility="private" name="{$columnFieldName}">
			<xsl:choose>
				<xsl:when test="($columnIsNullable) and not($columnIsStringType)">
					<xsl:attribute name="dataTypeName">
						<xsl:value-of select="$nullableDotNetTypeName"/>
					</xsl:attribute>
					<plx:passTypeParam>
						<xsl:choose>
							<xsl:when test="$columnIsEnumType">
								<xsl:attribute name="dataTypeName">
									<xsl:value-of select="$columnEnumTypeName"/>
								</xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="dataTypeName">
									<xsl:call-template name="GetDotNetTypeFromDcilPredefinedDataType">
										<xsl:with-param name="column" select="."/>
									</xsl:call-template>
								</xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
					</plx:passTypeParam>
				</xsl:when>
				<xsl:otherwise>
					<xsl:choose>
						<xsl:when test="$columnIsEnumType">
							<xsl:attribute name="dataTypeName">
								<xsl:value-of select="$columnEnumTypeName"/>
							</xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="dataTypeName">
								<xsl:call-template name="GetDotNetTypeFromDcilPredefinedDataType">
									<xsl:with-param name="column" select="."/>
								</xsl:call-template>
							</xsl:attribute>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</plx:field>
		<plx:property visibility="public" name="{$columnPropertyName}">
			<xsl:if test="$GenerateServiceLayer">
				<plx:attribute dataTypeName="DataMember">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="Name"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<plx:string data="{$columnPropertyName}"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="IsRequired"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<xsl:choose>
									<xsl:when test="not($columnIsNullable)">
										<plx:trueKeyword/>
									</xsl:when>
									<xsl:otherwise>
										<plx:falseKeyword/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</plx:attribute>
			</xsl:if>
			<xsl:if test="$GenerateLinqAttributes">
				<plx:attribute dataTypeName="Column">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="Name"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<plx:string data="{fn:bracketQuotedName($columnName)}"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="Storage"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<plx:string data="{$columnFieldName}"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="CanBeNull"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<xsl:choose>
									<xsl:when test="not($columnIsNullable)">
										<plx:falseKeyword/>
									</xsl:when>
									<xsl:otherwise>
										<plx:trueKeyword/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
					<xsl:if test="$columnIsPartOfPriamryKey">
						<plx:passParam>
							<plx:binaryOperator type="assignNamed">
								<plx:left>
									<plx:nameRef name="IsPrimaryKey"/>
								</plx:left>
								<plx:right>
									<!--TODO: Set this via a settings file.-->
									<plx:trueKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:passParam>
					</xsl:if>
					<xsl:if test="$columnIsDbGeneratedIdentityColumn">
						<plx:passParam>
							<plx:binaryOperator type="assignNamed">
								<plx:left>
									<plx:nameRef name="IsDbGenerated"/>
								</plx:left>
								<plx:right>
									<!--TODO: Set this via a settings file.-->
									<plx:trueKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:passParam>
					</xsl:if>
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="DbType"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<plx:string>
									<xsl:attribute name="data">
										<xsl:choose>
											<xsl:when test="dcl:predefinedDataType">
												<xsl:call-template name="GetDbTypeFromDcilPredefinedDataType">
													<xsl:with-param name="predefinedDataType" select="dcl:predefinedDataType"/>
													<xsl:with-param name="column" select="."/>
												</xsl:call-template>
											</xsl:when>
											<xsl:when test="dcl:domainRef">
												<xsl:variable name="domainPredefinedDataType" select="$Enumerations[@name = current()/dcl:domainRef/@name]/dcl:predefinedDataType"/>
												<xsl:call-template name="GetDbTypeFromDcilPredefinedDataType">
													<xsl:with-param name="predefinedDataType" select="$domainPredefinedDataType"/>
													<xsl:with-param name="column" select="."/>
												</xsl:call-template>
											</xsl:when>
											<xsl:otherwise>
												<xsl:message terminate="yes">SANITY CHECK: A Column should always havea predefinedDataType or a domainRef.</xsl:message>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:attribute>
								</plx:string>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</plx:attribute>
			</xsl:if>
			<plx:returns>
				<xsl:choose>
					<xsl:when test="($columnIsNullable) and not($columnIsStringType)">
						<xsl:attribute name="dataTypeName">
							<xsl:value-of select="$nullableDotNetTypeName"/>
						</xsl:attribute>
						<plx:passTypeParam>
							<xsl:choose>
								<xsl:when test="$columnIsEnumType">
									<xsl:attribute name="dataTypeName">
										<xsl:value-of select="$columnEnumTypeName"/>
									</xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="dataTypeName">
										<xsl:call-template name="GetDotNetTypeFromDcilPredefinedDataType">
											<xsl:with-param name="column" select="."/>
										</xsl:call-template>
									</xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
						</plx:passTypeParam>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="$columnIsEnumType">
								<xsl:attribute name="dataTypeName">
									<xsl:value-of select="$columnEnumTypeName"/>
								</xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="dataTypeName">
									<xsl:call-template name="GetDotNetTypeFromDcilPredefinedDataType">
										<xsl:with-param name="column" select="."/>
									</xsl:call-template>
								</xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
			</plx:returns>
			<plx:get>
				<plx:return>
					<plx:callThis type="field" name="{$columnFieldName}" />
				</plx:return>
			</plx:get>
			<plx:set>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<plx:callThis type="field" name="{$columnFieldName}" />
							</plx:left>
							<plx:right>
								<plx:valueKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:callThis name="OnPropertyChanging">
						<plx:passParam>
							<plx:nameRef name="{$columnPropertyName}PropertyChangingEventArgs"/>
						</plx:passParam>
					</plx:callThis>
					<plx:assign>
						<plx:left>
							<plx:callThis type="field" name="{$columnFieldName}" />
						</plx:left>
						<plx:right>
							<plx:valueKeyword/>
						</plx:right>
					</plx:assign>
					<plx:callThis name="OnPropertyChanged">
						<plx:passParam>
							<plx:nameRef name="{$columnPropertyName}PropertyChangedEventArgs"/>
						</plx:passParam>
					</plx:callThis>
				</plx:branch>
			</plx:set>
		</plx:property>
	</xsl:template>

	<xsl:template name="GeneratePropertyChangeInformation">
		<xsl:param name="propertyName"/>
		<xsl:variable name="propertyPragmaData" select="concat('PropertyChanging and PropertyChanged Information for ',$propertyName)"/>
		<plx:pragma type="region" data="{$propertyPragmaData}"/>
		<xsl:call-template name="GenerateMemberPropertyChangeFields">
			<xsl:with-param name="changeSuffix" select="'ing'"/>
			<xsl:with-param name="entityName" select="$propertyName"/>
		</xsl:call-template>
		<xsl:call-template name="GenerateMemberPropertyChangeFields">
			<xsl:with-param name="changeSuffix" select="'ed'"/>
			<xsl:with-param name="entityName" select="$propertyName"/>
		</xsl:call-template>
		<plx:pragma type="closeRegion" data="{$propertyPragmaData}"/>
	</xsl:template>

	<xsl:template match="dcl:referenceConstraint" mode="GenerateEntityMembers">
		<xsl:variable name="entityName" select="fn:pascalName(@targetTable)"/>
		<xsl:variable name="propertyNameFragment">
			<xsl:for-each select="dcl:columnRef">
				<xsl:value-of select="fn:pascalName(@sourceName)"/>
				<xsl:if test="position() = last()">
					<xsl:text>Reference</xsl:text>
				</xsl:if>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="propertyName" select="string($propertyNameFragment)"/>
		<xsl:variable name="fieldName" select="concat($PrivateMemberPrefix,fn:camelName($propertyName))"/>
		<xsl:call-template name="GeneratePropertyChangeInformation">
			<xsl:with-param name="propertyName" select="$propertyName"/>
		</xsl:call-template>
		<plx:field visibility="private" dataTypeName="EntityRef" name="{$fieldName}">
			<plx:passTypeParam dataTypeName="{$entityName}"/>
		</plx:field>
		<plx:property visibility="public" name="{$propertyName}">
			<xsl:if test="$GenerateServiceLayer">
				<plx:attribute dataTypeName="DataMember">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="Name"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<plx:string data="{$propertyName}"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</plx:attribute>
			</xsl:if>
			<xsl:if test="$GenerateLinqAttributes">
				<plx:attribute dataTypeName="Association">
					<!-- TODO: Determin what we should do with: DeleteOnNull, DeleteRule, IsUnique, TypeId, Name -->
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="Storage"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<plx:string data="{$fieldName}"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="IsForeignKey"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<plx:trueKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="ThisKey"/>
							</plx:left>
							<plx:right>
								<plx:string>
									<xsl:for-each select="dcl:columnRef">
										<xsl:value-of select="fn:pascalName(@sourceName)"/>
										<xsl:if test="position() != last()">
											<xsl:text>,</xsl:text>
										</xsl:if>
									</xsl:for-each>
								</plx:string>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</plx:attribute>
			</xsl:if>
			<plx:returns dataTypeName="{$entityName}"/>
			<plx:get>
				<plx:return>
					<plx:callInstance type="property" name="Entity">
						<plx:callObject>
							<plx:callThis type="field" name="{$fieldName}"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:return>
			</plx:get>
			<plx:set>
				<plx:local name="previousValue" dataTypeName="{$entityName}">
					<plx:initialize>
						<plx:callInstance type="property" name="Entity">
							<plx:callObject>
								<plx:callThis type="field" name="{$fieldName}"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<xsl:variable name="entityRefsEntitySetPropertyName" select="concat(fn:pascalName(../@name),$CollectionSuffix)"/>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="booleanOr">
							<plx:left>
								<plx:unaryOperator type="booleanNot">
									<plx:callInstance type="property" name="HasLoadedOrAssignedValue">
										<plx:callObject>
											<plx:callThis type="field" name="{$fieldName}"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:unaryOperator>
							</plx:left>
							<plx:right>
								<plx:binaryOperator type="inequality">
									<plx:left>
										<plx:nameRef name="previousValue"/>
									</plx:left>
									<plx:right>
										<plx:valueKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:callThis name="OnPropertyChanging">
						<plx:passParam>
							<plx:callThis accessor="static" type="field" name="{$propertyName}PropertyChangingEventArgs"/>
						</plx:passParam>
					</plx:callThis>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="inequality">
								<plx:left>
									<plx:nameRef name="previousValue"/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:assign>
							<plx:left>
								<plx:callInstance type="property" name="Entity">
									<plx:callObject>
										<plx:callThis type="field" name="{$fieldName}" />
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:assign>
						<plx:callInstance name="Remove">
							<plx:callObject>
								<plx:callInstance type="property" name="{$entityRefsEntitySetPropertyName}">
									<plx:callObject>
										<plx:nameRef name="previousValue"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
							<plx:passParam>
								<plx:thisKeyword/>
							</plx:passParam>
						</plx:callInstance>
					</plx:branch>
					<plx:assign>
						<plx:left>
							<plx:callInstance type="property" name="Entity">
								<plx:callObject>
									<plx:callThis type="field" name="{$fieldName}"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:valueKeyword/>
						</plx:right>
					</plx:assign>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="inequality">
								<plx:left>
									<plx:valueKeyword/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:callInstance name="Add">
							<plx:callObject>
								<plx:callInstance type="property" name="{$entityRefsEntitySetPropertyName}">
									<plx:callObject>
										<plx:valueKeyword/>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
							<plx:passParam>
								<plx:thisKeyword/>
							</plx:passParam>
						</plx:callInstance>
						<xsl:for-each select="dcl:columnRef">
							<plx:assign>
								<plx:left>
									<plx:callThis type="field" name="{fn:pascalName(@sourceName)}"/>
								</plx:left>
								<plx:right>
									<plx:callInstance type="property" name="{fn:pascalName(@targetName)}">
										<plx:callObject>
											<plx:valueKeyword/>
										</plx:callObject>
									</plx:callInstance>
								</plx:right>
							</plx:assign>
						</xsl:for-each>
					</plx:branch>
					<plx:callThis name="OnPropertyChanged">
						<plx:passParam>
							<plx:callThis accessor="static" type="field" name="{$propertyName}PropertyChangedEventArgs"/>
						</plx:passParam>
					</plx:callThis>
				</plx:branch>
			</plx:set>
		</plx:property>
	</xsl:template>

	<xsl:template match="dcl:domain" mode="GenerateEnumerations">
		<xsl:param name="enumName" select="string(@name)"/>
		<plx:enum visibility="public" name="{fn:pascalName($enumName)}">
			<xsl:if test="$GenerateServiceLayer">
				<plx:attribute dataTypeName="DataContract">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="Name"/>
							</plx:left>
							<plx:right>
								<!--TODO: Set this via a settings file.-->
								<plx:string data="{$enumName}"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</plx:attribute>
			</xsl:if>
			<xsl:for-each select="dcl:checkConstraint/dep:inPredicate/ddt:characterStringLiteral">
				<plx:enumItem name="{fn:pascalName(@value)}">
					<xsl:if test="$GenerateServiceLayer">
						<plx:attribute dataTypeName="EnumMember">
							<plx:passParam>
								<plx:binaryOperator type="assignNamed">
									<plx:left>
										<plx:nameRef name="Value"/>
									</plx:left>
									<plx:right>
										<!--TODO: Set this via a settings file.-->
										<plx:string data="{@value}"/>
									</plx:right>
								</plx:binaryOperator>
							</plx:passParam>
						</plx:attribute>
					</xsl:if>
				</plx:enumItem>
			</xsl:for-each>
		</plx:enum>
	</xsl:template>

	<xsl:template name="GenerateOperationBehaviorAttributeTransactionScopeUsage">
		<plx:passParam>
			<plx:binaryOperator type="assignNamed">
				<plx:left>
					<plx:nameRef name="TransactionScopeRequired"/>
				</plx:left>
				<plx:right>
					<plx:trueKeyword/>
				</plx:right>
			</plx:binaryOperator>
		</plx:passParam>
		<plx:passParam>
			<plx:binaryOperator type="assignNamed">
				<plx:left>
					<plx:nameRef name="TransactionAutoComplete"/>
				</plx:left>
				<plx:right>
					<plx:trueKeyword/>
				</plx:right>
			</plx:binaryOperator>
		</plx:passParam>
	</xsl:template>

	<!--<xsl:template match="dcl:schema" mode="GenerateServiceLayerTests">
		<xsl:variable name="testClassSuffix" select="'Tests'"/>
		<xsl:variable name="testFunctionPrefix" select="'Test'"/>
		-->
	<!--<plx:namespace name="System">
			<plx:
		</plx:namespace>-->
	<!--
		<xsl:variable name="serviceDataTypeName" select="$ServiceName"/>
		<xsl:variable name="servicePropertyName" select="'Service'"/>
		<xsl:variable name="serviceFieldName" select="concat($PrivateMemberPrefix,'service')"/>
		<xsl:variable name="baseTestClassName" select="concat($ModelName,'Service',$testClassSuffix)"/>
		<plx:class visibility="public" modifier="abstract" partual="true" name="{$baseTestClassName}">
			<plx:attribute>
				<xsl:attribute name="dataTypeName">
					<xsl:choose>
						<xsl:when test="$UnitTestingFramework = 'NUnit'">
							<xsl:value-of select="'TestFixture'"/>
						</xsl:when>
						<xsl:when test="$UnitTestingFramework = 'MSTest'">
							<xsl:value-of select="'TestClass'"/>
						</xsl:when>
					</xsl:choose>
				</xsl:attribute>
			</plx:attribute>
			<plx:function visibility="public" modifier="virtual">
				<xsl:attribute name="name">
					<xsl:choose>
						<xsl:when test="$UnitTestingFramework = 'NUnit'">
							<xsl:value-of select="'TestFixtureSetUp'"/>
						</xsl:when>
						<xsl:when test="$UnitTestingFramework = 'MSTest'">
							<xsl:value-of select="'ClassInitialize'"/>
						</xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<plx:attribute>
					<xsl:attribute name="dataTypeName">
						<xsl:choose>
							<xsl:when test="$UnitTestingFramework = 'NUnit'">
								<xsl:value-of select="'TestFixtureSetUp'"/>
							</xsl:when>
							<xsl:when test="$UnitTestingFramework = 'MSTest'">
								<xsl:value-of select="'ClassCleanup'"/>
							</xsl:when>
						</xsl:choose>
					</xsl:attribute>
				</plx:attribute>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="equality">
							<plx:left>
								<plx:callInstance type="property" name="{$servicePropertyName}"/>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:assign>
						<plx:left>
							
						</plx:left>
						<plx:right>
							
						</plx:right>
					</plx:assign>
				</plx:branch>
			</plx:function>
			<plx:field visibility="private" dataTypeName="{$serviceDataTypeName}" name="{$serviceFieldName}"/>
			<plx:property visibility="public" modifier="virtual" name="{$servicePropertyName}">
				<plx:returns dataTypeName="{$serviceDataTypeName}"/>
			</plx:property>
			<xsl:if test="$UseTransactionScopes">
				<xsl:variable name="transactionDataTypeName" select="'TransactionScope'"/>
				<xsl:variable name="transactionPropertyName" select="'Transaction'"/>
				<xsl:variable name="transactionFieldName" select="concat($PrivateMemberPrefix,'transaction')"/>
				<plx:field visibility="private" dataTypeName="{$transactionDataTypeName}" name="{$transactionFieldName}"/>
				<plx:property visibility="public" modifier="virtual" name="{$transactionPropertyName}">
					<plx:returns dataTypeName="{$transactionDataTypeName}"/>
				</plx:property>
			</xsl:if>
			<plx:function visibility="public" modifier="virtual">
				<xsl:attribute name="name">
					<xsl:choose>
						<xsl:when test="$UnitTestingFramework = 'NUnit'">
							<xsl:value-of select="'TestFixtureTearDown'"/>
						</xsl:when>
						<xsl:when test="$UnitTestingFramework = 'MSTest'">
							<xsl:value-of select="'ClassCleanup'"/>
						</xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<plx:attribute>
					<xsl:attribute name="dataTypeName">
						<xsl:choose>
							<xsl:when test="$UnitTestingFramework = 'NUnit'">
								<xsl:value-of select="'TestFixtureTearDown'"/>
							</xsl:when>
							<xsl:when test="$UnitTestingFramework = 'MSTest'">
								<xsl:value-of select="'ClassCleanup'"/>
							</xsl:when>
						</xsl:choose>
					</xsl:attribute>
				</plx:attribute>
				<xsl:if test="$UseTransactionScopes">
					
				</xsl:if>
			</plx:function>
		</plx:class>
		<xsl:for-each select="dcl:table">
			<xsl:variable name="entityName" select="@name"/>
			<xsl:variable name="entityTableAccessorName" select="concat($entityName,$TableSuffix)"/>
			<plx:class visibility="public" partial="true" name="concat($entityName,'ServiceTests')">
				<plx:attribute>
					<xsl:attribute name="dataTypeName">
						<xsl:choose>
							<xsl:when test="$UnitTestingFramework = 'NUnit'">
								<xsl:value-of select="'TestFixture'"/>
							</xsl:when>
							<xsl:when test="$UnitTestingFramework = 'MSTest'">
								<xsl:value-of select="'TestClass'"/>
							</xsl:when>
						</xsl:choose>
					</xsl:attribute>
				</plx:attribute>
				<plx:derivesFromClass dataTypeName="asdfasd1234234f"/>
				
									  
				<xsl:variable name="interfaceImplementationPragmaData" select="concat('I',$ModelName,' ',$CreateKeyword,', ',$ReadKeyword,', ',$UpdateKeyword,', and ',$DeleteKeyword,' Implementation for ',$entityName)"/>
				<plx:pragma type="region" data="{$interfaceImplementationPragmaData}"/>
				<xsl:variable name="createEntityFunctionName" select="concat($CreateKeyword,$entityName)"/>
				<plx:function name="{$createEntityFunctionName}" visibility="public">
					<plx:attribute dataTypeName="OperationBehavior">
						<xsl:attribute name="dataTypeName">
							<xsl:choose>
								<xsl:when test="$UnitTestingFramework = 'NUnit'">

								</xsl:when>
								<xsl:when test="$UnitTestingFramework = 'MSTest'">

								</xsl:when>
							</xsl:choose>
						</xsl:attribute>
						<xsl:if test="$UseTransactionScopes">
						</xsl:if>
					</plx:attribute>
					<xsl:variable name="parameterName" select="fn:camelName($entityName),'To',$CreateKeyword)"/>
					<plx:param dataTypeName="{$entityName}" name="{$parameterName}"/>
					<plx:try>
						<plx:autoDispose localName=".implied" dataTypeName="{$serviceDataContextPropertyName}">
							<plx:initialize>
								<plx:callNew dataTypeName="{$serviceDataContextTypeName}" type="new"/>
							</plx:initialize>
							<plx:callInstance name="InsertOnSubmit">
								<plx:callObject>
									<plx:callInstance type="property" name="{$entityTableAccessorName}">
										<plx:callObject>
											<plx:callThis type="property" name="{$serviceDataContextPropertyName}"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="{$parameterName}"/>
								</plx:passParam>
							</plx:callInstance>
							<plx:callInstance name="SubmitChanges">
								<plx:callObject>
									<plx:callThis type="property" name="{$serviceDataContextPropertyName}"/>
								</plx:callObject>
								<plx:passParam>
									<plx:callInstance type="property" name="FailOnFirstConflict">
										<plx:callObject>
											<plx:nameRef name="ConflictMode"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
							</plx:callInstance>
						</plx:autoDispose>
						<plx:catch dataTypeName="SqlException" localName="exception">
							<plx:comment>TODO: Log the exception somewhere</plx:comment>
							<plx:throw/>
						</plx:catch>
						<plx:catch dataTypeName="Exception" localName="exception">
							<plx:comment>TODO: Log the exception somewhere</plx:comment>
							<plx:throw/>
						</plx:catch>
					</plx:try>
				</plx:function>
				<xsl:for-each select="dcl:uniquenessConstraint">
					<xsl:variable name="isPrimaryUniquenessConstraint" select="@isPrimary = 'true' or @isPrimary = 1"/>
					<plx:function visibility="public">
						<xsl:attribute name="name">
							<xsl:value-of select="concat($ReadKeyword,$entityName,'By')"/>
							<xsl:choose>
								<xsl:when test="$isPrimaryUniquenessConstraint">
									<xsl:value-of select="$IsPrimaryKeyword"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:for-each select="dcl:columnRef">
										<xsl:value-of select="fn:pascalName(@name)"/>
										<xsl:if test="position() != last()">
											<xsl:text>And</xsl:text>
										</xsl:if>
									</xsl:for-each>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						-->
	<!--<plx:attribute dataTypeName="OperationContract">
								<plx:passParam>
									<plx:binaryOperator type="assignNamed">
										<plx:left>
											<plx:nameRef name="IsOneWay"/>
										</plx:left>
										<plx:right>
											<plx:falseKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:passParam>
							</plx:attribute>-->
	<!--
						<xsl:for-each select="dcl:columnRef">
							<plx:param name="{fn:camelName(@name)}" >
								<xsl:variable name="column" select="../../dcl:column[@name=current()/@name]"/>
								<xsl:attribute name="dataTypeName">
									<xsl:choose>
										<xsl:when test="$column/dcl:domainRef[@name = $Enumerations/@name]">
											<xsl:value-of select="translate($column/dcl:domainRef/@name,'_','')"/>
										</xsl:when>
									</xsl:choose>
									<xsl:call-template name="GetDotNetTypeFromDcilPredefinedDataType">
										<xsl:with-param name="predefinedDataType" select="$column/dcl:predefinedDataType"/>
										<xsl:with-param name="column" select="$column"/>
									</xsl:call-template>
								</xsl:attribute>
							</plx:param>
						</xsl:for-each>
						<plx:returns dataTypeName="{$entityName}"/>
						<plx:autoDispose localName=".implied" dataTypeName="{$serviceDataContextPropertyName}">
							<plx:initialize>
								<plx:callNew dataTypeName="{$serviceDataContextTypeName}" type="new"/>
							</plx:initialize>
							<plx:return>
								<plx:callInstance name="First">
									<plx:callObject>
										<plx:callInstance name="Where">
											<plx:callObject>
												<plx:callInstance type="property" name="{$entityTableAccessorName}">
													<plx:callObject>
														<plx:callThis name="{$serviceDataContextPropertyName}" type="property"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:callObject>
											<plx:passParam>
												<xsl:variable name="parameterName" select="fn:camelName($entityName),'To',$ReadKeyword)"/>
												<plx:anonymousFunction>
													<plx:param dataTypeName="{$entityName}" name="{$parameterName}"/>
													<plx:return>
														<xsl:call-template name="GenerateNestedOperations">
															<xsl:with-param name="operands" select="dcl:columnRef"/>
														</xsl:call-template>
														-->
	<!--<xsl:for-each select="dcl:columnRef">
															<plx:binaryOperator type="equality">
																<plx:left>
																	<plx:callInstance name="{fn:pascalName(@name)}" type="property">
																		<plx:callObject>
																			<plx:nameRef name="{$parameterName}" type="parameter"/>
																		</plx:callObject>
																	</plx:callInstance>
																</plx:left>
																<plx:right>
																	<plx:nameRef name="{fn:camelName(@name)}"/>
																</plx:right>
															</plx:binaryOperator>
														</xsl:for-each>-->
	<!--
													</plx:return>
												</plx:anonymousFunction>
											</plx:passParam>
										</plx:callInstance>
									</plx:callObject>
								</plx:callInstance>
							</plx:return>
						</plx:autoDispose>
					</plx:function>
				</xsl:for-each>
				<xsl:variable name="updateEntityFunctionName" select="concat($UpdateKeyword,$entityName)"/>
				<plx:function name="{$updateEntityFunctionName}" visibility="public">
					<plx:attribute dataTypeName="OperationBehavior">
						<xsl:if test="$UseTransactionScopes">
							<xsl:call-template name="GenerateOperationBehaviorAttributeTransactionScopeUsage"/>
						</xsl:if>
					</plx:attribute>
					<xsl:variable name="parameterName" select="fn:camelName(@name),'To',$UpdateKeyword)"/>
					<plx:param dataTypeName="{@name}" name="{$parameterName}"/>
					<plx:try>
						<plx:callInstance name="Attach">
							<plx:callObject>
								<plx:callThis type="property" name="{$entityTableAccessorName}"></plx:callThis>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="{$parameterName}"/>
							</plx:passParam>
							<plx:passParam>
								<plx:callInstance name="GetOriginalEntityState">
									<plx:callObject>
										<plx:callThis type="property" name="{$entityTableAccessorName}"></plx:callThis>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="{$parameterName}"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:passParam>
						</plx:callInstance>
						<plx:callThis name="SubmitChanges">
							<plx:passParam>
								<plx:callInstance type="property" name="FailOnFirstConflict">
									<plx:callObject>
										<plx:nameRef name="ConflictMode"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:passParam>
						</plx:callThis>
						<plx:catch dataTypeName="SqlException" localName="exception">
							<plx:comment>TODO: Log the exception somewhere</plx:comment>
							<plx:throw/>
						</plx:catch>
						<plx:catch dataTypeName="Exception" localName="exception">
							<plx:comment>TODO: Log the exception somewhere</plx:comment>
							<plx:throw/>
						</plx:catch>
					</plx:try>
				</plx:function>
				<xsl:variable name="deleteEntityFunctionName" select="concat($DeleteKeyword,$entityName)"/>
				<plx:function name="{$deleteEntityFunctionName}" visibility="public">
					<plx:attribute dataTypeName="OperationBehavior">
						<xsl:if test="$UseTransactionScopes">
							<xsl:call-template name="GenerateOperationBehaviorAttributeTransactionScopeUsage"/>
						</xsl:if>
					</plx:attribute>
					<xsl:variable name="parameterName" select="fn:camelName($entityName),'To',$DeleteKeyword)"/>
					<plx:param dataTypeName="{$entityName}" name="{$parameterName}"/>
					<plx:try>
						<plx:callInstance name="Attach">
							<plx:callObject>
								<plx:callThis type="property" name="{$entityTableAccessorName}"></plx:callThis>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="{$parameterName}"/>
							</plx:passParam>
							<plx:passParam>
								<plx:falseKeyword/>
							</plx:passParam>
						</plx:callInstance>
						<plx:callInstance name="DeleteOnSubmit">
							<plx:callObject>
								<plx:callThis type="property" name="{$entityTableAccessorName}"></plx:callThis>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="{$parameterName}"/>
							</plx:passParam>
						</plx:callInstance>
						<plx:callThis name="SubmitChanges">
							<plx:passParam>
								<plx:callInstance type="property" name="FailOnFirstConflict">
									<plx:callObject>
										<plx:nameRef name="ConflictMode"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:passParam>
						</plx:callThis>
						<plx:catch dataTypeName="SqlException" localName="exception">
							<plx:comment>TODO: Log the exception somewhere</plx:comment>
							<plx:throw/>
						</plx:catch>
						<plx:catch dataTypeName="Exception" localName="exception">
							<plx:comment>TODO: Log the exception somewhere</plx:comment>
							<plx:throw/>
						</plx:catch>
					</plx:try>
				</plx:function>
				<plx:pragma type="closeRegion" data="{$interfaceImplementationPragmaData}"/>
			</plx:class>
		</xsl:for-each>
	</xsl:template>-->

	<xsl:template match="dcl:schema" mode="GenerateLinqServiceLayerImplementation">
		<plx:class partial="true" visibility="public" name="{$ModelName}Service">
			<plx:attribute dataTypeName="ServiceBehavior">
				<plx:passParam>
					<plx:binaryOperator type="assignNamed">
						<plx:left>
							<plx:nameRef name="InstanceContextMode"/>
						</plx:left>
						<plx:right>
							<plx:callStatic dataTypeName="InstanceContextMode" name="{$InstanceContextMode}" type="field"/>
						</plx:right>
					</plx:binaryOperator>
				</plx:passParam>
			</plx:attribute>
			<plx:implementsInterface dataTypeName="{$ServiceName}"/>
			<plx:function visibility="public" name=".construct">

			</plx:function>
			<xsl:variable name="serviceDataContextTypeName" select="concat($ModelName,$DataContextSuffix)"/>
			<xsl:variable name="serviceDataContextFieldName" select="concat($PrivateMemberPrefix,'serviceDataContext')"/>
			<xsl:variable name="serviceDataContextPropertyName" select="'ServiceDataContext'"/>
			<plx:field visibility="private" dataTypeName="{$serviceDataContextTypeName}" name="{$serviceDataContextFieldName}"/>
			<plx:property visibility="public" name="{$serviceDataContextPropertyName}">
				<plx:returns dataTypeName="{$serviceDataContextTypeName}"/>
				<plx:get>
					<plx:return>
						<plx:inlineStatement dataTypeName="{$serviceDataContextTypeName}">
							<plx:nullFallbackOperator>
								<plx:left>
									<plx:callThis type="field" name="{$serviceDataContextFieldName}"/>
								</plx:left>
								<plx:right>
									<plx:inlineStatement dataTypeName="{$serviceDataContextTypeName}">
										<plx:assign>
											<plx:left>
												<plx:callThis type="field" name="{$serviceDataContextFieldName}"/>
											</plx:left>
											<plx:right>
												<plx:callNew dataTypeName="{$serviceDataContextTypeName}"/>
											</plx:right>
										</plx:assign>
									</plx:inlineStatement>
								</plx:right>
							</plx:nullFallbackOperator>
						</plx:inlineStatement>
					</plx:return>
				</plx:get>
				<plx:set>
					<plx:assign>
						<plx:left>
							<plx:callThis type="field" name="{$serviceDataContextFieldName}"/>
						</plx:left>
						<plx:right>
							<plx:valueKeyword/>
						</plx:right>
					</plx:assign>
				</plx:set>
			</plx:property>
			<xsl:for-each select="dcl:table">
				<xsl:variable name="entityName" select="fn:pascalName(@name)"/>
				<xsl:variable name="entityTableAccessorName" select="concat($entityName,$TableSuffix)"/>
				<xsl:variable name="interfaceImplementationPragmaData" select="concat('I',$ModelName,' ',$CreateKeyword,', ',$ReadKeyword,', ',$UpdateKeyword,', and ',$DeleteKeyword,' Implementation for ',$entityName)"/>
				<plx:pragma type="region" data="{$interfaceImplementationPragmaData}"/>
				<xsl:variable name="createEntityFunctionName" select="concat($CreateKeyword,$entityName)"/>
				<plx:function name="{$createEntityFunctionName}" visibility="public">
					<plx:attribute dataTypeName="OperationBehavior">
						<xsl:if test="$UseTransactionScopes">
							<xsl:call-template name="GenerateOperationBehaviorAttributeTransactionScopeUsage"/>
						</xsl:if>
					</plx:attribute>
					<xsl:variable name="parameterName" select="concat(fn:camelName($entityName),'To',$CreateKeyword)"/>
					<plx:param dataTypeName="{$entityName}" name="{$parameterName}"/>
					<plx:try>
						<plx:autoDispose localName=".implied" dataTypeName="{$serviceDataContextPropertyName}">
							<plx:initialize>
								<plx:callNew dataTypeName="{$serviceDataContextTypeName}" type="new"/>
							</plx:initialize>
							<plx:callInstance name="InsertOnSubmit">
								<plx:callObject>
									<plx:callInstance type="property" name="{$entityTableAccessorName}">
										<plx:callObject>
											<plx:callThis type="property" name="{$serviceDataContextPropertyName}"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="{$parameterName}"/>
								</plx:passParam>
							</plx:callInstance>
							<plx:callInstance name="SubmitChanges">
								<plx:callObject>
									<plx:callThis type="property" name="{$serviceDataContextPropertyName}"/>
								</plx:callObject>
								<plx:passParam>
									<plx:callInstance type="property" name="FailOnFirstConflict">
										<plx:callObject>
											<plx:nameRef name="ConflictMode"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
							</plx:callInstance>
						</plx:autoDispose>
						<plx:catch dataTypeName="SqlException" localName="exception">
							<plx:comment>TODO: Log the exception somewhere</plx:comment>
							<plx:throw/>
						</plx:catch>
						<plx:catch dataTypeName="Exception" localName="exception">
							<plx:comment>TODO: Log the exception somewhere</plx:comment>
							<plx:throw/>
						</plx:catch>
					</plx:try>
				</plx:function>
				<xsl:for-each select="dcl:uniquenessConstraint">
					<xsl:variable name="isPrimaryUniquenessConstraint" select="@isPrimary = 'true' or @isPrimary = 1"/>
					<plx:function visibility="public">
						<xsl:attribute name="name">
							<xsl:value-of select="concat($ReadKeyword,$entityName,'By')"/>
							<xsl:choose>
								<xsl:when test="$isPrimaryUniquenessConstraint">
									<xsl:value-of select="$IsPrimaryKeyword"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:for-each select="dcl:columnRef">
										<xsl:value-of select="fn:pascalName(@name)"/>
										<xsl:if test="position() != last()">
											<xsl:text>And</xsl:text>
										</xsl:if>
									</xsl:for-each>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<!--<plx:attribute dataTypeName="OperationContract">
								<plx:passParam>
									<plx:binaryOperator type="assignNamed">
										<plx:left>
											<plx:nameRef name="IsOneWay"/>
										</plx:left>
										<plx:right>
											<plx:falseKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:passParam>
							</plx:attribute>-->
						<xsl:for-each select="dcl:columnRef">
							<plx:param name="{fn:camelName(@name)}" >
								<xsl:variable name="column" select="../../dcl:column[@name=current()/@name]"/>
								<xsl:attribute name="dataTypeName">
									<xsl:choose>
										<xsl:when test="$column/dcl:domainRef[@name = $Enumerations/@name]">
											<xsl:value-of select="translate($column/dcl:domainRef/@name,'_','')"/>
										</xsl:when>
									</xsl:choose>
									<xsl:call-template name="GetDotNetTypeFromDcilPredefinedDataType">
										<xsl:with-param name="column" select="$column"/>
									</xsl:call-template>
								</xsl:attribute>
							</plx:param>
						</xsl:for-each>
						<plx:returns dataTypeName="{$entityName}"/>
						<plx:autoDispose localName=".implied"  dataTypeName="{$serviceDataContextPropertyName}">
							<plx:initialize>
								<plx:callNew dataTypeName="{$serviceDataContextTypeName}" type="new"/>
							</plx:initialize>
							<plx:assign>
								<plx:left>
									<plx:callInstance name="ObjectTrackingEnabled" type="property">
										<plx:callObject>
											<plx:callThis name="{$serviceDataContextPropertyName}" type="property"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:falseKeyword/>
								</plx:right>
							</plx:assign>
							<plx:return>
								<plx:callInstance name="First">
									<plx:callObject>
										<plx:callInstance name="Where">
											<plx:callObject>
												<plx:callInstance type="property" name="{$entityTableAccessorName}">
													<plx:callObject>
														<plx:callThis name="{$serviceDataContextPropertyName}" type="property"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:callObject>
											<plx:passParam>
												<xsl:variable name="parameterName" select="concat(fn:camelName($entityName),'To',$ReadKeyword)"/>
												<plx:anonymousFunction>
													<plx:param dataTypeName="{$entityName}" name="{$parameterName}"/>
													<plx:return>
														<xsl:call-template name="GenerateNestedOperations">
															<xsl:with-param name="operands" select="dcl:columnRef"/>
															<xsl:with-param name="parameterName" select="$parameterName"/>
														</xsl:call-template>
													</plx:return>
												</plx:anonymousFunction>
											</plx:passParam>
										</plx:callInstance>
									</plx:callObject>
								</plx:callInstance>
							</plx:return>
						</plx:autoDispose>
					</plx:function>
				</xsl:for-each>
				<xsl:variable name="updateEntityFunctionName" select="concat($UpdateKeyword,$entityName)"/>
				<plx:function name="{$updateEntityFunctionName}" visibility="public">
					<plx:attribute dataTypeName="OperationBehavior">
						<xsl:if test="$UseTransactionScopes">
							<xsl:call-template name="GenerateOperationBehaviorAttributeTransactionScopeUsage"/>
						</xsl:if>
					</plx:attribute>
					<xsl:variable name="oldParameterName" select="concat('old',$entityName)"/>
					<xsl:variable name="newParameterName" select="concat('new',$entityName)"/>
					<plx:param dataTypeName="{$entityName}" name="{$newParameterName}"/>
					<plx:param dataTypeName="{$entityName}" name="{$oldParameterName}"/>
					<plx:try>
						<plx:autoDispose localName=".implied" dataTypeName="{$serviceDataContextPropertyName}">
							<plx:initialize>
								<plx:callNew dataTypeName="{$serviceDataContextTypeName}" type="new"/>
							</plx:initialize>
							<plx:callInstance name="Attach">
								<plx:callObject>
									<plx:callInstance type="property" name="{$entityTableAccessorName}">
										<plx:callObject>
											<plx:callThis type="property" name="{$serviceDataContextPropertyName}"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="{$newParameterName}" type="parameter"/>
								</plx:passParam>
								<plx:passParam>
									<plx:nameRef name="{$oldParameterName}" type="parameter"/>
								</plx:passParam>
								<!--<plx:passParam>
									<plx:callInstance name="GetOriginalEntityState">
										<plx:callObject>
											<plx:callInstance type="property" name="{$entityTableAccessorName}">
												<plx:callObject>
													<plx:callThis type="property" name="{$serviceDataContextPropertyName}"/>
												</plx:callObject>
											</plx:callInstance>
										</plx:callObject>
										<plx:passParam>
											<plx:nameRef name="{$oldParameterName}"/>
										</plx:passParam>
									</plx:callInstance>
								</plx:passParam>-->
							</plx:callInstance>
							<plx:callInstance name="SubmitChanges">
								<plx:callObject>
									<plx:callThis type="property" name="{$serviceDataContextPropertyName}"/>
								</plx:callObject>
								<plx:passParam>
									<plx:callInstance type="property" name="FailOnFirstConflict">
										<plx:callObject>
											<plx:nameRef name="ConflictMode"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
							</plx:callInstance>
						</plx:autoDispose>
						<plx:catch dataTypeName="SqlException" localName="exception">
							<plx:comment>TODO: Log the exception somewhere</plx:comment>
							<plx:throw/>
						</plx:catch>
						<plx:catch dataTypeName="Exception" localName="exception">
							<plx:comment>TODO: Log the exception somewhere</plx:comment>
							<plx:throw/>
						</plx:catch>
					</plx:try>
				</plx:function>
				<xsl:variable name="deleteEntityFunctionName" select="concat($DeleteKeyword,$entityName)"/>
				<plx:function name="{$deleteEntityFunctionName}" visibility="public">
					<plx:attribute dataTypeName="OperationBehavior">
						<xsl:if test="$UseTransactionScopes">
							<xsl:call-template name="GenerateOperationBehaviorAttributeTransactionScopeUsage"/>
						</xsl:if>
					</plx:attribute>
					<xsl:variable name="parameterName" select="concat(fn:camelName($entityName),'To',$DeleteKeyword)"/>
					<plx:param dataTypeName="{$entityName}" name="{$parameterName}"/>
					<plx:try>
						<plx:callInstance name="Attach">
							<plx:callObject>
								<plx:callInstance type="property" name="{$entityTableAccessorName}">
									<plx:callObject>
										<plx:callThis type="property" name="{$serviceDataContextPropertyName}"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="{$parameterName}"/>
							</plx:passParam>
							<plx:passParam>
								<plx:falseKeyword/>
							</plx:passParam>
						</plx:callInstance>
						<plx:callInstance name="DeleteOnSubmit">
							<plx:callObject>
								<plx:callInstance type="property" name="{$entityTableAccessorName}">
									<plx:callObject>
										<plx:callThis type="property" name="{$serviceDataContextPropertyName}"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="{$parameterName}"/>
							</plx:passParam>
						</plx:callInstance>
						<plx:callInstance name="SubmitChanges">
							<plx:callObject>
								<plx:callThis type="property" name="{$serviceDataContextPropertyName}"/>
							</plx:callObject>
							<plx:passParam>
								<plx:callInstance type="property" name="FailOnFirstConflict">
									<plx:callObject>
										<plx:nameRef name="ConflictMode"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:passParam>
						</plx:callInstance>
						<plx:catch dataTypeName="SqlException" localName="exception">
							<plx:comment>TODO: Log the exception somewhere</plx:comment>
							<plx:throw/>
						</plx:catch>
						<plx:catch dataTypeName="Exception" localName="exception">
							<plx:comment>TODO: Log the exception somewhere</plx:comment>
							<plx:throw/>
						</plx:catch>
					</plx:try>
				</plx:function>
				<plx:pragma type="closeRegion" data="{$interfaceImplementationPragmaData}"/>
			</xsl:for-each>
		</plx:class>
	</xsl:template>

	<xsl:template name="GenerateNestedOperations">
		<xsl:param name="operator" select="'booleanAnd'"/>
		<xsl:param name="operands"/>
		<xsl:param name="operandsIndex" select="1"/>
		<xsl:param name="operandsCount" select="count($operands)"/>
		<xsl:param name="parameterName"/>
		<xsl:choose>
			<xsl:when test="$operandsIndex = $operandsCount">
				<xsl:variable name="columnRefName" select="$operands[$operandsIndex]/@name"/>
				<plx:binaryOperator type="equality">
					<plx:left>
						<plx:callInstance name="{fn:pascalName($columnRefName)}" type="property">
							<plx:callObject>
								<plx:nameRef name="{$parameterName}" type="parameter"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:nameRef name="{fn:camelName($columnRefName)}" type="parameter"/>
					</plx:right>
				</plx:binaryOperator>
			</xsl:when>
			<xsl:otherwise>
				<plx:binaryOperator type="{$operator}">
					<plx:left>
						<xsl:variable name="columnRefName" select="$operands[$operandsIndex]/@name"/>
						<plx:binaryOperator type="equality">
							<plx:left>
								<plx:callInstance name="{fn:pascalName($columnRefName)}" type="property">
									<plx:callObject>
										<plx:nameRef name="{$parameterName}" type="parameter"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:nameRef name="{fn:camelName($columnRefName)}" type="parameter"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:left>
					<plx:right>
						<xsl:call-template name="GenerateNestedOperations">
							<xsl:with-param name="operator" select="$operator"/>
							<xsl:with-param name="operands" select="$operands"/>
							<xsl:with-param name="operandsIndex" select="$operandsIndex + 1"/>
							<xsl:with-param name="operandsCount" select="$operandsCount"/>
							<xsl:with-param name="parameterName" select="$parameterName"/>
						</xsl:call-template>
					</plx:right>
				</plx:binaryOperator>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>

	<xsl:template match="dcl:schema" mode="GenerateDatabaseContext">
		<plx:class partial="true" visibility="public" name="{$ModelName}{$DataContextSuffix}">
			<plx:attribute dataTypeName="Database">
				<plx:passParam>
					<plx:binaryOperator type="assignNamed">
						<plx:left>
							<plx:nameRef name="Name"/>
						</plx:left>
						<plx:right>
							<plx:string data="{$DatabaseName}"/>
						</plx:right>
					</plx:binaryOperator>
				</plx:passParam>
			</plx:attribute>
			<!--<xsl:if test="$GenerateServiceLayer">
				<plx:attribute dataTypeName="ServiceBehavior">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef name="InstanceContextMode"/>
							</plx:left>
							<plx:right>
								<plx:callStatic dataTypeName="InstanceContextMode" name="{$InstanceContextMode}" type="field"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</plx:attribute>
			</xsl:if>-->
			<plx:derivesFromClass dataTypeName="DataContext"/>
			<!--<xsl:if test="$GenerateServiceLayer">
				<plx:implementsInterface dataTypeName="{$ServiceName}"/>
			</xsl:if>-->
			<xsl:variable name="mappingSourceName" select="'mapping'"/>
			<xsl:variable name="mappingSourceFieldName" select="concat($PrivateMemberPrefix,$mappingSourceName)"/>
			<xsl:variable name="connectionName" select="'connection'"/>
			<xsl:variable name="fileOrServerConnectionStringName" select="'fileOrServerConnection'"/>
			<xsl:variable name="mappingSourceType" select="'MappingSource'"/>
			<xsl:variable name="iDbConnectionType" select="'IDbConnection'"/>
			<plx:field visibility="private" static="true" dataTypeName="{$mappingSourceType}" name="{$mappingSourceFieldName}">
				<plx:initialize>
					<xsl:choose>
						<xsl:when test="$UseAttributeMapping">
							<plx:callNew dataTypeName="AttributeMappingSource"/>
						</xsl:when>
						<xsl:when test="$UseXmlMapping">
							<plx:callStatic dataTypeName="XmlMappingSource" name="FromXml">
								<plx:passParam>
									<plx:string data="Pass in the name of the xml file here."/>
								</plx:passParam>
							</plx:callStatic>
						</xsl:when>
						<xsl:otherwise>
							<xsl:message terminate="yes">Must specify the type of MappingSource being used.</xsl:message>
						</xsl:otherwise>
					</xsl:choose>
				</plx:initialize>
			</plx:field>
			<plx:function visibility="public" name=".construct">
				<plx:initialize>
					<plx:callThis name=".implied" accessor="base">
						<plx:passParam>
							<plx:callInstance name="{$SettingsPropertyName}" type="property">
								<plx:callObject>
									<plx:callStatic dataTypeName="Settings" name="Default" type="property"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef name="{$mappingSourceFieldName}" type="parameter"/>
						</plx:passParam>
					</plx:callThis>
				</plx:initialize>
			</plx:function>
			<plx:function name=".construct" visibility="public">
				<plx:param name="{$connectionName}" dataTypeName="{$iDbConnectionType}"/>
				<plx:initialize>
					<plx:callThis name=".implied" accessor="base">
						<plx:passParam>
							<plx:nameRef name="{$connectionName}" type="parameter"/>
						</plx:passParam>
					</plx:callThis>
				</plx:initialize>
			</plx:function>
			<plx:function name=".construct" visibility="public">
				<plx:param name="{$fileOrServerConnectionStringName}" dataTypeName=".string"/>
				<plx:initialize>
					<plx:callThis name=".implied" accessor="base">
						<plx:passParam>
							<plx:nameRef name="{$fileOrServerConnectionStringName}" type="parameter"/>
						</plx:passParam>
					</plx:callThis>
				</plx:initialize>
			</plx:function>
			<plx:function name=".construct" visibility="public">
				<plx:param name="{$connectionName}" dataTypeName="{$iDbConnectionType}"/>
				<plx:param name="{$mappingSourceName}" dataTypeName="{$mappingSourceType}"/>
				<plx:initialize>
					<plx:callThis name=".implied" accessor="base">
						<plx:passParam>
							<plx:nameRef name="{$connectionName}" type="parameter"/>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef name="{$mappingSourceName}" type="parameter"/>
						</plx:passParam>
					</plx:callThis>
				</plx:initialize>
			</plx:function>
			<plx:function name=".construct" visibility="public">
				<plx:param name="{$fileOrServerConnectionStringName}" dataTypeName=".string"/>
				<plx:param name="{$mappingSourceName}" dataTypeName="{$mappingSourceType}"/>
				<plx:initialize>
					<plx:callThis name=".implied" accessor="base">
						<plx:passParam>
							<plx:nameRef name="{$fileOrServerConnectionStringName}" type="parameter"/>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef name="{$mappingSourceName}" type="parameter"/>
						</plx:passParam>
					</plx:callThis>
				</plx:initialize>
			</plx:function>
			<xsl:apply-templates select="dcl:table" mode="CreateDatabaseContextTableAccessorProperties"/>
			<!--<xsl:if test="$GenerateServiceLayer">
				<xsl:for-each select="dcl:table">
					<xsl:variable name="entityName" select="@name"/>
					<xsl:variable name="entityTableAccessorName" select="concat($entityName,$TableSuffix)"/>
					<xsl:variable name="interfaceImplementationPragmaData" select="concat('I',$ModelName,' ',$CreateKeyword,', ',$ReadKeyword,', ',$UpdateKeyword,', and ',$DeleteKeyword,' Implementation for ',$entityName)"/>
					<plx:pragma type="region" data="{$interfaceImplementationPragmaData}"/>
					<xsl:variable name="createEntityFunctionName" select="concat($CreateKeyword,$entityName)"/>
					<plx:function name="{$createEntityFunctionName}" visibility="public">
						<plx:attribute dataTypeName="OperationBehavior">
							<xsl:if test="$UseTransactionScopes">
								<xsl:call-template name="GenerateOperationBehaviorAttributeTransactionScopeUsage"/>
							</xsl:if>
						</plx:attribute>
						<xsl:variable name="parameterName" select="concat(fn:camelName($entityName),'To',$CreateKeyword)"/>
						<plx:param dataTypeName="{$entityName}" name="{$parameterName}"/>
						<plx:try>
							<plx:callInstance name="InsertOnSubmit">
								<plx:callObject>
									<plx:callThis type="property" name="{$entityTableAccessorName}"></plx:callThis>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="{$parameterName}"/>
								</plx:passParam>
							</plx:callInstance>
							<plx:callThis name="SubmitChanges">
								<plx:passParam>
									<plx:callInstance type="property" name="FailOnFirstConflict">
										<plx:callObject>
											<plx:nameRef name="ConflictMode"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
							</plx:callThis>
							<plx:catch dataTypeName="SqlException" localName="exception">
								<plx:comment>TODO: Log the exception somewhere</plx:comment>
								<plx:throw/>
							</plx:catch>
							<plx:catch dataTypeName="Exception" localName="exception">
								<plx:comment>TODO: Log the exception somewhere</plx:comment>
								<plx:throw/>
							</plx:catch>
						</plx:try>
					</plx:function>
					<xsl:for-each select="dcl:uniquenessConstraint">
						<xsl:variable name="isPrimaryUniquenessConstraint" select="@isPrimary = 'true' or @isPrimary = 1"/>
						<plx:function visibility="public">
							<xsl:attribute name="name">
								<xsl:value-of select="concat($ReadKeyword,$entityName,'By')"/>
								<xsl:choose>
									<xsl:when test="$isPrimaryUniquenessConstraint">
										<xsl:value-of select="$IsPrimaryKeyword"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:for-each select="dcl:columnRef">
											<xsl:value-of select="fn:pascalName(@name)"/>
											<xsl:if test="position() != last()">
												<xsl:text>And</xsl:text>
											</xsl:if>
										</xsl:for-each>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							-->
			<!--<plx:attribute dataTypeName="OperationContract">
								<plx:passParam>
									<plx:binaryOperator type="assignNamed">
										<plx:left>
											<plx:nameRef name="IsOneWay"/>
										</plx:left>
										<plx:right>
											<plx:falseKeyword/>
										</plx:right>
									</plx:binaryOperator>
								</plx:passParam>
							</plx:attribute>-->
			<!--
							<xsl:for-each select="dcl:columnRef">
								<plx:param name="{fn:camelName(@name)}" >
									<xsl:variable name="column" select="../../dcl:column[@name=current()/@name]"/>
									<xsl:attribute name="dataTypeName">
										<xsl:choose>
											<xsl:when test="$column/dcl:domainRef[@name = $Enumerations/@name]">
												<xsl:value-of select="translate($column/dcl:domainRef/@name,'_','')"/>
											</xsl:when>
										</xsl:choose>
										<xsl:call-template name="GetDotNetTypeFromDcilPredefinedDataType">
											<xsl:with-param name="column" select="$column"/>
										</xsl:call-template>
									</xsl:attribute>
								</plx:param>
							</xsl:for-each>
							<plx:returns dataTypeName="{$entityName}"/>
							<plx:return>
								<plx:callInstance name="First">
									<plx:callObject>
										<plx:callInstance name="Where">
											<plx:callObject>
												<plx:nameRef type="parameter" name="{$entityTableAccessorName}"/>
											</plx:callObject>
											<plx:passParam>
												<xsl:variable name="parameterName" select="concat(fn:camelName($entityName),'To',$ReadKeyword)"/>
												<plx:anonymousFunction>
													<plx:param dataTypeName="{$entityName}" name="{$parameterName}"/>
													<plx:return>
														-->
			<!--<xsl:call-template name="CombineExpressions">
															<xsl:with-param name="Expressions" select="dcl:columnRef"/>
														</xsl:call-template>-->
			<!--
														<xsl:for-each select="dcl:columnRef">
															<plx:binaryOperator type="equality">
																<plx:left>
																	<plx:callInstance name="{fn:pascalName(@name)}" type="property">
																		<plx:callObject>
																			<plx:nameRef name="{$parameterName}" type="parameter"/>
																		</plx:callObject>
																	</plx:callInstance>
																</plx:left>
																<plx:right>
																	<plx:nameRef name="{fn:camelName(@name)}"/>
																</plx:right>
															</plx:binaryOperator>
														</xsl:for-each>
													</plx:return>
												</plx:anonymousFunction>
											</plx:passParam>
										</plx:callInstance>
									</plx:callObject>
								</plx:callInstance>
							</plx:return>
						</plx:function>
					</xsl:for-each>
					<xsl:variable name="updateEntityFunctionName" select="concat($UpdateKeyword,$entityName)"/>
					<plx:function name="{$updateEntityFunctionName}" visibility="public">
						<plx:attribute dataTypeName="OperationBehavior">
							<xsl:if test="$UseTransactionScopes">
								<xsl:call-template name="GenerateOperationBehaviorAttributeTransactionScopeUsage"/>
							</xsl:if>
						</plx:attribute>
						<xsl:variable name="parameterName" select="concat(fn:camelName(@name),'To',$UpdateKeyword)"/>
						<plx:param dataTypeName="{@name}" name="{$parameterName}"/>
						<plx:try>
							<plx:callInstance name="Attach">
								<plx:callObject>
									<plx:callThis type="property" name="{$entityTableAccessorName}"></plx:callThis>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="{$parameterName}"/>
								</plx:passParam>
								<plx:passParam>
									<plx:callInstance name="GetOriginalEntityState">
										<plx:callObject>
											<plx:callThis type="property" name="{$entityTableAccessorName}"></plx:callThis>
										</plx:callObject>
										<plx:passParam>
											<plx:nameRef name="{$parameterName}"/>
										</plx:passParam>
									</plx:callInstance>
								</plx:passParam>
							</plx:callInstance>
							<plx:callThis name="SubmitChanges">
								<plx:passParam>
									<plx:callInstance type="property" name="FailOnFirstConflict">
										<plx:callObject>
											<plx:nameRef name="ConflictMode"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
							</plx:callThis>
							<plx:catch dataTypeName="SqlException" localName="exception">
								<plx:comment>TODO: Log the exception somewhere</plx:comment>
								<plx:throw/>
							</plx:catch>
							<plx:catch dataTypeName="Exception" localName="exception">
								<plx:comment>TODO: Log the exception somewhere</plx:comment>
								<plx:throw/>
							</plx:catch>
						</plx:try>
					</plx:function>
					<xsl:variable name="deleteEntityFunctionName" select="concat($DeleteKeyword,$entityName)"/>
					<plx:function name="{$deleteEntityFunctionName}" visibility="public">
						<plx:attribute dataTypeName="OperationBehavior">
							<xsl:if test="$UseTransactionScopes">
								<xsl:call-template name="GenerateOperationBehaviorAttributeTransactionScopeUsage"/>
							</xsl:if>
						</plx:attribute>
						<xsl:variable name="parameterName" select="concat(fn:camelName($entityName),'To',$DeleteKeyword)"/>
						<plx:param dataTypeName="{$entityName}" name="{$parameterName}"/>
						<plx:try>
							<plx:callInstance name="Attach">
								<plx:callObject>
									<plx:callThis type="property" name="{$entityTableAccessorName}"></plx:callThis>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="{$parameterName}"/>
								</plx:passParam>
								<plx:passParam>
									<plx:falseKeyword/>
								</plx:passParam>
							</plx:callInstance>
							<plx:callInstance name="DeleteOnSubmit">
								<plx:callObject>
									<plx:callThis type="property" name="{$entityTableAccessorName}"></plx:callThis>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="{$parameterName}"/>
								</plx:passParam>
							</plx:callInstance>
							<plx:callThis name="SubmitChanges">
								<plx:passParam>
									<plx:callInstance type="property" name="FailOnFirstConflict">
										<plx:callObject>
											<plx:nameRef name="ConflictMode"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
							</plx:callThis>
							<plx:catch dataTypeName="SqlException" localName="exception">
								<plx:comment>TODO: Log the exception somewhere</plx:comment>
								<plx:throw/>
							</plx:catch>
							<plx:catch dataTypeName="Exception" localName="exception">
								<plx:comment>TODO: Log the exception somewhere</plx:comment>
								<plx:throw/>
							</plx:catch>
						</plx:try>
					</plx:function>
					<plx:pragma type="closeRegion" data="{$interfaceImplementationPragmaData}"/>
				</xsl:for-each>
			</xsl:if>-->
		</plx:class>
	</xsl:template>

	<xsl:template name="CombineExpressions">
		<xsl:param name="Expressions"/>
		<xsl:param name="Operator" select="'binaryAnd'"/>
		<xsl:param name="CurrentPosition" select="1"/>
		<xsl:param name="ItemCount" select="count($Expressions)"/>
		<xsl:choose>
			<xsl:when test="$CurrentPosition=$ItemCount">
				<xsl:copy-of select="$Expressions[$CurrentPosition]"/>
			</xsl:when>
			<xsl:otherwise>
				<plx:binaryOperator type="{$Operator}">
					<plx:left>
						<xsl:copy-of select="$Expressions[$CurrentPosition]"/>
					</plx:left>
					<plx:right>
						<xsl:call-template name="CombineExpressions">
							<xsl:with-param name="Expressions" select="$Expressions"/>
							<xsl:with-param name="Operator" select="$Operator"/>
							<xsl:with-param name="CurrentPosition" select="$CurrentPosition + 1"/>
							<xsl:with-param name="ItemCount" select="$ItemCount"/>
						</xsl:call-template>
					</plx:right>
				</plx:binaryOperator>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="dcl:table" mode="CreateDatabaseContextTableAccessorProperties">
		<xsl:variable name="tableName" select="fn:pascalName(@name)"/>
		<plx:property name="{$tableName}{$TableSuffix}" visibility="public">
			<plx:returns dataTypeName="Table">
				<plx:passTypeParam dataTypeName="{$tableName}"/>
			</plx:returns>
			<plx:get>
				<plx:return>
					<plx:callThis name="GetTable">
						<plx:passMemberTypeParam dataTypeName="{$tableName}"/>
					</plx:callThis>
				</plx:return>
			</plx:get>
		</plx:property>
	</xsl:template>

	<xsl:template name="GenerateINotifyPropertyChangeImplementation">
		<xsl:param name="changeSuffix" select="'ed'"/>
		<plx:field visibility="private" name="{$PrivateMemberPrefix}propertyChang{$changeSuffix}EventHandler" dataTypeName="PropertyChang{$changeSuffix}EventHandler"/>
		<plx:event visibility="privateInterfaceMember" name="PropertyChang{$changeSuffix}">
			<!-- Suppress the 'InterfaceMethodsShouldBeCallableByChildTypes' FxCop warning, since it is not applicable here. -->
			<!-- Child types call the property-specific notification methods, which in turn raise the INotifyPropertyChanged.PropertyChanged event. -->
			<xsl:call-template name="GenerateSuppressMessageAttribute">
				<xsl:with-param name="category" select="'Microsoft.Design'"/>
				<xsl:with-param name="checkId" select="'CA1033:InterfaceMethodsShouldBeCallableByChildTypes'"/>
			</xsl:call-template>
			<plx:interfaceMember memberName="PropertyChang{$changeSuffix}" dataTypeName="INotifyPropertyChang{$changeSuffix}"/>
			<plx:param name="sender" dataTypeName=".object"/>
			<plx:param name="e" dataTypeName="PropertyChang{$changeSuffix}EventArgs"/>
			<plx:explicitDelegateType dataTypeName="PropertyChang{$changeSuffix}EventHandler"/>
			<plx:onAdd>
				<xsl:call-template name="GetINotifyPropertyChangeImplementationEventOnAddRemoveCode">
					<xsl:with-param name="changeSuffix" select="$changeSuffix"/>
					<xsl:with-param name="MethodName" select="'Combine'"/>
				</xsl:call-template>
			</plx:onAdd>
			<plx:onRemove>
				<xsl:call-template name="GetINotifyPropertyChangeImplementationEventOnAddRemoveCode">
					<xsl:with-param name="changeSuffix" select="$changeSuffix"/>
					<xsl:with-param name="MethodName" select="'Remove'"/>
				</xsl:call-template>
			</plx:onRemove>
		</plx:event>
		<plx:function visibility="private" overload="{not($RaiseEventsAsynchronously)}" name="OnPropertyChang{$changeSuffix}">
			<plx:param name="e" dataTypeName="PropertyChang{$changeSuffix}EventArgs"/>
			<plx:local name="eventHandler" dataTypeName="PropertyChang{$changeSuffix}EventHandler"/>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:cast type="exceptionCast" dataTypeName=".object">
								<plx:inlineStatement dataTypeName="PropertyChang{$changeSuffix}EventHandler">
									<plx:assign>
										<plx:left>
											<plx:nameRef type="local" name="eventHandler"/>
										</plx:left>
										<plx:right>
											<plx:callThis type="field" name="{$PrivateMemberPrefix}propertyChang{$changeSuffix}EventHandler"/>
										</plx:right>
									</plx:assign>
								</plx:inlineStatement>
							</plx:cast>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<xsl:variable name="commonCallCodeFragment">
					<plx:passParam>
						<plx:thisKeyword/>
					</plx:passParam>
					<plx:passParam>
						<plx:nameRef type="parameter" name="e"/>
					</plx:passParam>
				</xsl:variable>
				<xsl:variable name="commonCallCode" select="exsl:node-set($commonCallCodeFragment)/child::*"/>
				<xsl:choose>
					<xsl:when test="$RaiseEventsAsynchronously">
						<plx:callStatic name="InvokeEventHandlerAsync" dataTypeName="EventHandlerUtility">
							<plx:passParam>
								<plx:nameRef type="local" name="eventHandler"/>
							</plx:passParam>
							<xsl:copy-of select="$commonCallCode"/>
						</plx:callStatic>
					</xsl:when>
					<xsl:otherwise>
						<plx:callInstance type="delegateCall" name=".implied">
							<plx:callObject>
								<plx:nameRef type="local" name="eventHandler"/>
							</plx:callObject>
							<xsl:copy-of select="$commonCallCode"/>
						</plx:callInstance>
					</xsl:otherwise>
				</xsl:choose>
			</plx:branch>
		</plx:function>
		<xsl:if test="not($RaiseEventsAsynchronously)">
			<plx:function visibility="private" overload="true" name="OnPropertyChang{$changeSuffix}">
				<plx:param name="e" dataTypeName="PropertyChang{$changeSuffix}EventArgs"/>
				<plx:local name="eventHandler" dataTypeName="PropertyChang{$changeSuffix}EventHandler"/>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<plx:cast type="exceptionCast" dataTypeName=".object">
									<plx:inlineStatement dataTypeName="PropertyChang{$changeSuffix}EventHandler">
										<plx:assign>
											<plx:left>
												<plx:nameRef type="local" name="eventHandler"/>
											</plx:left>
											<plx:right>
												<plx:callThis type="field" name="{$PrivateMemberPrefix}propertyChang{$changeSuffix}EventHandler"/>
											</plx:right>
										</plx:assign>
									</plx:inlineStatement>
								</plx:cast>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:callInstance type="delegateCall" name=".implied">
						<plx:callObject>
							<plx:nameRef type="local" name="eventHandler"/>
						</plx:callObject>
						<plx:passParam>
							<plx:thisKeyword/>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef type="parameter" name="e"/>
						</plx:passParam>
					</plx:callInstance>
				</plx:branch>
			</plx:function>
		</xsl:if>
	</xsl:template>

	<xsl:template name="GenerateSuppressMessageAttribute">
		<xsl:param name="category"/>
		<xsl:param name="checkId"/>
		<xsl:param name="justification"/>
		<xsl:param name="messageId"/>
		<xsl:param name="scope"/>
		<xsl:param name="target"/>
		<xsl:if test="$GenerateCodeAnalysisAttributes">
			<plx:attribute dataTypeName="SuppressMessageAttribute">
				<plx:passParam>
					<plx:string>
						<xsl:value-of select="$category"/>
					</plx:string>
				</plx:passParam>
				<plx:passParam>
					<plx:string>
						<xsl:value-of select="$checkId"/>
					</plx:string>
				</plx:passParam>
				<xsl:if test="$justification">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef type="namedParameter" name="Justification"/>
							</plx:left>
							<plx:right>
								<plx:string>
									<xsl:value-of select="$justification"/>
								</plx:string>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</xsl:if>
				<xsl:if test="$messageId">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef type="namedParameter" name="MessageId"/>
							</plx:left>
							<plx:right>
								<plx:string>
									<xsl:value-of select="$messageId"/>
								</plx:string>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</xsl:if>
				<xsl:if test="$scope">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef type="namedParameter" name="Scope"/>
							</plx:left>
							<plx:right>
								<plx:string>
									<xsl:value-of select="$scope"/>
								</plx:string>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</xsl:if>
				<xsl:if test="$target">
					<plx:passParam>
						<plx:binaryOperator type="assignNamed">
							<plx:left>
								<plx:nameRef type="namedParameter" name="Target"/>
							</plx:left>
							<plx:right>
								<plx:string>
									<xsl:value-of select="$target"/>
								</plx:string>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
				</xsl:if>
			</plx:attribute>
		</xsl:if>
	</xsl:template>

	<xsl:template name="GetINotifyPropertyChangeImplementationEventOnAddRemoveCode">
		<xsl:param name="changeSuffix" select="'ed'"/>
		<xsl:param name="MethodName"/>
		<plx:branch>
			<plx:condition>
				<plx:binaryOperator type="identityInequality">
					<plx:left>
						<plx:cast type="exceptionCast" dataTypeName=".object">
							<plx:valueKeyword/>
						</plx:cast>
					</plx:left>
					<plx:right>
						<plx:nullKeyword/>
					</plx:right>
				</plx:binaryOperator>
			</plx:condition>
			<xsl:choose>
				<xsl:when test="$SynchronizeEventAddRemove">
					<plx:local name="currentHandler" dataTypeName="PropertyChang{$changeSuffix}EventHandler"/>
					<plx:loop checkCondition="before">
						<plx:condition>
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:cast type="exceptionCast" dataTypeName=".object">
										<plx:callStatic name="CompareExchange" dataTypeName="Interlocked">
											<plx:passMemberTypeParam dataTypeName="PropertyChang{$changeSuffix}EventHandler"/>
											<plx:passParam type="inOut">
												<plx:callThis type="field" name="{$PrivateMemberPrefix}propertyChang{$changeSuffix}EventHandler"/>
											</plx:passParam>
											<plx:passParam>
												<plx:cast type="exceptionCast" dataTypeName="PropertyChang{$changeSuffix}EventHandler">
													<!-- PLIX_TODO: Once the PLiX formatters support keyword filtering, remove the dataTypeQualifier attribute from the next line. -->
													<plx:callStatic name="{$MethodName}" dataTypeName="Delegate" dataTypeQualifier="System">
														<plx:passParam>
															<plx:inlineStatement dataTypeName="PropertyChang{$changeSuffix}EventHandler">
																<plx:assign>
																	<plx:left>
																		<plx:nameRef type="local" name="currentHandler"/>
																	</plx:left>
																	<plx:right>
																		<plx:callThis type="field" name="{$PrivateMemberPrefix}propertyChang{$changeSuffix}EventHandler"/>
																	</plx:right>
																</plx:assign>
															</plx:inlineStatement>
														</plx:passParam>
														<plx:passParam>
															<plx:valueKeyword/>
														</plx:passParam>
													</plx:callStatic>
												</plx:cast>
											</plx:passParam>
											<plx:passParam>
												<plx:nameRef type="local" name="currentHandler"/>
											</plx:passParam>
										</plx:callStatic>
									</plx:cast>
								</plx:left>
								<plx:right>
									<plx:cast type="exceptionCast" dataTypeName=".object">
										<plx:nameRef type="local" name="currentHandler"/>
									</plx:cast>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
					</plx:loop>
				</xsl:when>
				<xsl:otherwise>
					<plx:assign>
						<plx:left>
							<plx:callThis type="field" name="{$PrivateMemberPrefix}propertyChang{$changeSuffix}EventHandler"/>
						</plx:left>
						<plx:right>
							<plx:cast type="exceptionCast" dataTypeName="PropertyChang{$changeSuffix}EventHandler">
								<!-- PLIX_TODO: Once the PLiX formatters support keyword filtering, remove the dataTypeQualifier attribute from the next line. -->
								<plx:callStatic name="Combine" dataTypeName="Delegate" dataTypeQualifier="System">
									<plx:passParam>
										<plx:callThis type="field" name="{$PrivateMemberPrefix}propertyChang{$changeSuffix}EventHandler"/>
									</plx:passParam>
									<plx:passParam>
										<plx:valueKeyword/>
									</plx:passParam>
								</plx:callStatic>
							</plx:cast>
						</plx:right>
					</plx:assign>
				</xsl:otherwise>
			</xsl:choose>
		</plx:branch>
	</xsl:template>

	<xsl:template name="GenerateGlobalSupportClasses">
		<plx:class visibility="public" modifier="static" name="EventHandlerUtility">
			<plx:attribute dataTypeName="HostProtectionAttribute">
				<plx:passParam>
					<plx:callStatic type="field" name="LinkDemand" dataTypeName="SecurityAction" />
				</plx:passParam>
				<plx:passParam>
					<plx:binaryOperator type="assignNamed">
						<plx:left>
							<plx:nameRef type="namedParameter" name="SharedState"/>
						</plx:left>
						<plx:right>
							<plx:trueKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:passParam>
			</plx:attribute>

			<plx:function visibility="public" modifier="static" name="InvokeEventHandlerAsync">
				<plx:param name="eventHandler" dataTypeName="PropertyChangedEventHandler"/>
				<plx:param name="sender" dataTypeName=".object"/>
				<plx:param name="e" dataTypeName="PropertyChangedEventArgs"/>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityEquality">
							<plx:left>
								<plx:cast type="exceptionCast" dataTypeName=".object">
									<plx:nameRef type="parameter" name="eventHandler"/>
								</plx:cast>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:throw>
						<plx:callNew dataTypeName="ArgumentNullException">
							<plx:passParam>
								<plx:string data="eventHandler"/>
							</plx:passParam>
						</plx:callNew>
					</plx:throw>
				</plx:branch>
				<!-- PLIX_TODO: Once the PLiX formatters support keyword filtering, remove the dataTypeQualifier attribute from the next line. -->
				<plx:local name="invocationList" dataTypeName="Delegate" dataTypeQualifier="System" dataTypeIsSimpleArray="true">
					<plx:initialize>
						<plx:callInstance name="GetInvocationList">
							<plx:callObject>
								<plx:nameRef type="parameter" name="eventHandler"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<plx:loop checkCondition="before">
					<plx:initializeLoop>
						<plx:local name="i" dataTypeName=".i4">
							<plx:initialize>
								<plx:value type="i4" data="0"/>
							</plx:initialize>
						</plx:local>
					</plx:initializeLoop>
					<plx:condition>
						<plx:binaryOperator type="lessThan">
							<plx:left>
								<plx:nameRef type="local" name="i"/>
							</plx:left>
							<plx:right>
								<plx:callInstance type="property" name="Length">
									<plx:callObject>
										<plx:nameRef type="local" name="invocationList"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:beforeLoop>
						<plx:increment type="post">
							<plx:nameRef type="local" name="i"/>
						</plx:increment>
					</plx:beforeLoop>
					<plx:local name="currentEventHandler" dataTypeName="PropertyChangedEventHandler">
						<plx:initialize>
							<plx:cast type="exceptionCast" dataTypeName="PropertyChangedEventHandler">
								<plx:callInstance type="arrayIndexer" name=".implied">
									<plx:callObject>
										<plx:nameRef type="local" name="invocationList"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef type="local" name="i"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:cast>
						</plx:initialize>
					</plx:local>
					<plx:callInstance name="BeginInvoke">
						<plx:callObject>
							<plx:nameRef type="local" name="currentEventHandler"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef type="parameter" name="sender"/>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef type="parameter" name="e"/>
						</plx:passParam>
						<plx:passParam>
							<plx:callNew dataTypeName="AsyncCallback">
								<plx:passParam>
									<plx:callInstance type="methodReference" name="EndInvoke">
										<plx:callObject>
											<plx:nameRef type="local" name="currentEventHandler"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
							</plx:callNew>
						</plx:passParam>
						<plx:passParam>
							<plx:nullKeyword/>
						</plx:passParam>
					</plx:callInstance>
				</plx:loop>
			</plx:function>
			<plx:function visibility="public" modifier="static" name="InvokeEventHandlerAsync">
				<plx:param name="eventHandler" dataTypeName="PropertyChangingEventHandler"/>
				<plx:param name="sender" dataTypeName=".object"/>
				<plx:param name="e" dataTypeName="PropertyChangingEventArgs"/>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityEquality">
							<plx:left>
								<plx:cast type="exceptionCast" dataTypeName=".object">
									<plx:nameRef type="parameter" name="eventHandler"/>
								</plx:cast>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:throw>
						<plx:callNew dataTypeName="ArgumentNullException">
							<plx:passParam>
								<plx:string data="eventHandler"/>
							</plx:passParam>
						</plx:callNew>
					</plx:throw>
				</plx:branch>
				<!-- PLIX_TODO: Once the PLiX formatters support keyword filtering, remove the dataTypeQualifier attribute from the next line. -->
				<plx:local name="invocationList" dataTypeName="Delegate" dataTypeQualifier="System" dataTypeIsSimpleArray="true">
					<plx:initialize>
						<plx:callInstance name="GetInvocationList">
							<plx:callObject>
								<plx:nameRef type="parameter" name="eventHandler"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<plx:loop checkCondition="before">
					<plx:initializeLoop>
						<plx:local name="i" dataTypeName=".i4">
							<plx:initialize>
								<plx:value type="i4" data="0"/>
							</plx:initialize>
						</plx:local>
					</plx:initializeLoop>
					<plx:condition>
						<plx:binaryOperator type="lessThan">
							<plx:left>
								<plx:nameRef type="local" name="i"/>
							</plx:left>
							<plx:right>
								<plx:callInstance type="property" name="Length">
									<plx:callObject>
										<plx:nameRef type="local" name="invocationList"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:beforeLoop>
						<plx:increment type="post">
							<plx:nameRef type="local" name="i"/>
						</plx:increment>
					</plx:beforeLoop>
					<plx:local name="currentEventHandler" dataTypeName="PropertyChangingEventHandler">
						<plx:initialize>
							<plx:cast type="exceptionCast" dataTypeName="PropertyChangingEventHandler">
								<plx:callInstance type="arrayIndexer" name=".implied">
									<plx:callObject>
										<plx:nameRef type="local" name="invocationList"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef type="local" name="i"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:cast>
						</plx:initialize>
					</plx:local>
					<plx:callInstance name="BeginInvoke">
						<plx:callObject>
							<plx:nameRef type="local" name="currentEventHandler"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef type="parameter" name="sender"/>
						</plx:passParam>
						<plx:passParam>
							<plx:nameRef type="parameter" name="e"/>
						</plx:passParam>
						<plx:passParam>
							<plx:callNew dataTypeName="AsyncCallback">
								<plx:passParam>
									<plx:callInstance type="methodReference" name="EndInvoke">
										<plx:callObject>
											<plx:nameRef type="local" name="currentEventHandler"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:passParam>
							</plx:callNew>
						</plx:passParam>
						<plx:passParam>
							<plx:nullKeyword/>
						</plx:passParam>
					</plx:callInstance>
				</plx:loop>
			</plx:function>
		</plx:class>
	</xsl:template>

	<xsl:template name="GetDbTypeFromDcilPredefinedDataType">
		<xsl:param name="column"/>
		<xsl:variable name="predefinedDataTypeFragment">
			<xsl:variable name="directType" select="$column/dcl:predefinedDataType"/>
			<xsl:choose>
				<xsl:when test="$directType">
					<xsl:copy-of select="$directType"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="$Domains[@name=$column/dcl:domainRef/@name]/dcl:predefinedDataType"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="predefinedDataType" select="exsl:node-set($predefinedDataTypeFragment)/child::*"/>
		<xsl:variable name="predefinedDataTypeName" select="string($predefinedDataType/@name)"/>
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
				This one is weird in the default mapping in SQL Server where they use a different meaning for Timestamp.
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

	<xsl:template name="GetDotNetTypeFromDcilPredefinedDataType">
		<xsl:param name="column"/>
		<xsl:variable name="predefinedDataTypeFragment">
			<xsl:variable name="directType" select="$column/dcl:predefinedDataType"/>
			<xsl:choose>
				<xsl:when test="$directType">
					<xsl:copy-of select="$directType"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="$Domains[@name=$column/dcl:domainRef/@name]/dcl:predefinedDataType"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="predefinedDataType" select="exsl:node-set($predefinedDataTypeFragment)/child::*"/>
		<xsl:variable name="predefinedDataTypeName" select="string($predefinedDataType/@name)"/>
		<xsl:choose>
			<xsl:when test="$predefinedDataTypeName = 'CHARACTER'">
				<xsl:text>String</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'CHARACTER VARYING'">
				<xsl:text>String</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'CHARACTER LARGE OBJECT'">
				<xsl:text>String</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BINARY'">
				<xsl:text>Byte[]</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BINARY VARYING'">
				<xsl:text>Byte[]</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BINARY LARGE OBJECT'">
				<xsl:text>Byte[]</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'NUMERIC'">
				<xsl:text>Decimal</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'DECIMAL'">
				<xsl:text>Decimal</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'TINYINT'">
				<xsl:text>Byte</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'SMALLINT'">
				<xsl:text>Int16</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'INTEGER'">
				<xsl:text>Int32</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BIGINT'">
				<xsl:text>Int64</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'FLOAT'">
				<xsl:choose>
					<xsl:when test="string($predefinedDataType/@precision)">
						<xsl:choose>
							<xsl:when test="$predefinedDataType/@precision &lt;= 24">
								<xsl:text>Single</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>Double</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>Double</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'REAL'">
				<xsl:text>Single</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'DOUBLE PRECISION'">
				<xsl:text>Double</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'BOOLEAN'">
				<xsl:text>Boolean</xsl:text>
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
				This one is wierd.
				[Column(Storage="_Region_code", AutoSync=AutoSync.Always, DbType="rowversion", IsDbGenerated=true, IsVersion=true, UpdateCheck=UpdateCheck.Never)]
				public System.Data.Linq.Binary Region_code
				-->
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'INTERVAL'">
				<xsl:text>TimeSpan</xsl:text>
			</xsl:when>
			<xsl:when test="$predefinedDataTypeName = 'UNIQUEIDENTIFIER'">
				<xsl:text>Guid</xsl:text>
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
						_scrubNameRegex = regex = new System.Text.RegularExpressions.Regex("([-\"_/\\[\\]{}()!?*+ ])([^-\"_/\\[\\]{}()!?*+ ]*)", System.Text.RegularExpressions.RegexOptions.Compiled);
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
					return "[" + name.Substring(1, length - 2) + "]";
				}
			}
			return name;
		}
		]]>
	</msxsl:script>
</xsl:stylesheet>
