<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet
	version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:dep="http://schemas.orm.net/DIL/DILEP"
	xmlns:dcl="http://schemas.orm.net/DIL/DCIL"
	xmlns:ddt="http://schemas.orm.net/DIL/DILDT"
	xmlns:opt="http://schemas.neumont.edu/ORM/2008-04/LinqToSql/Settings"
	xmlns:exsl="http://exslt.org/common"
	xmlns:oct="urn:ORMCustomTool:ItemProperties"
	extension-element-prefixes="exsl dcl dep ddt oct"
	>
	<xsl:output indent="yes"/>

	<xsl:param name="LinqToSqlSettings" select="document('LinqToSqlSettings.xml')/child::*"/>

	<xsl:variable name="Model" select="dcl:schema"/>
	<xsl:variable name="ModelName" select="string($Model/@name)"/>
	<xsl:variable name="DatabaseSchemaName" select="string($ModelName)"/>
	<xsl:variable name="Entitys" select="$Model/dcl:table"/>
	<xsl:variable name="Enumerations" select="$Model/dcl:domain[dcl:predefinedDataType/@name = 'CHARACTER VARYING' or dcl:predefinedDataType/@name = 'CHARACTER' or dcl:predefinedDataType/@name = 'CHARACTER LARGE OBJECT' and count(dcl:checkConstraint) = 1 and count(dcl:checkConstraint/dep:inPredicate) = 1 and dcl:checkConstraint/dep:inPredicate/@type='IN']"/>
	
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
		<xsl:variable name="setting" select="string($LinqToSqlSettings/opt:ConnectionString/@DataBaseName)"/>
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
		<xsl:variable name="setting" select="string($LinqToSqlSettings/opt:NameParts/@ConnectionStringPropertyName)"/>
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
				<xsl:value-of select="$setting"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="true()"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="GenerateServiceLayer" select="string($GenerateServiceLayerFragment)"/>
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
	
	<xsl:param name="GenerateCodeAnalysisAttributes" select="true()"/>
	<xsl:param name="GenerateAccessedThroughPropertyAttribute" select="false()"/>
	<xsl:param name="GenerateObjectDataSourceSupport" select="true()"/>
	<xsl:param name="RaiseEventsAsynchronously" select="true()"/>
	<xsl:param name="SynchronizeEventAddRemove" select="true()"/>
	<xsl:param name="DefaultNamespace" select="'MyNamespace'"/>
	<xsl:variable name="GenerateLinqAttributes" select="true()"/>
	<xsl:param name="UseXmlMapping" select="$MappingTarget='XmlMapping'"/>
	<xsl:param name="UseAttributeMapping" select="$MappingTarget='AttributeMapping'"/>
	<xsl:param name="EmitDefaultValue" select="true()"/>

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
			<plx:namespaceImport name="System.Diagnostics.CodeAnalysis"/>
			<plx:namespaceImport name="System.Linq"/>
			<plx:namespaceImport name="System.Runtime.Serialization"/>
			<plx:namespaceImport name="System.Security.Permissions"/>
			<xsl:if test="$GenerateServiceLayer">
				<plx:namespaceImport name="System.ServiceModel"/>
			</xsl:if>
			<plx:namespaceImport name="System.Threading"/>
			<plx:namespaceImport name="{$ProjectName}.Properties"/>
			<xsl:apply-templates select="dcl:schema" mode="GenerateNamespace"/>
		</plx:root>
	</xsl:template>

	<xsl:template match="dcl:schema" mode="GenerateNamespace">
		<xsl:variable name="namespaceName" select="$DefaultNamespace"/>
		<plx:namespace name="{$namespaceName}">
			<xsl:if test="$GenerateServiceLayer">
				<xsl:apply-templates select="." mode="GenerateServiceContract"/>
			</xsl:if>
			<xsl:apply-templates select="." mode="GenerateDatabaseContext"/>
			<!--Generate Enumerations for any ValueConstraints that qualify. -->
			<xsl:apply-templates select="$Enumerations" mode="GenerateEnumerations"/>
			<xsl:apply-templates select="dcl:table" mode="GenerateBusinessEntities">
			</xsl:apply-templates>
			<xsl:call-template name="GenerateGlobalSupportClasses"/>
		</plx:namespace>
	</xsl:template>

	<xsl:template match="dcl:schema" mode="GenerateServiceContract">
		<xsl:if test="$GenerateServiceLayer">
			<plx:interface visibility="public" name="I{$ModelName}Service">
				<plx:attribute dataTypeName="ServiceContractAttribute"/>
				<xsl:for-each select="dcl:table">
					<xsl:variable name="entityName" select="@name"/>
					<xsl:variable name="pragmaData" select="concat($CreateKeyword,', ',$ReadKeyword,', ',$UpdateKeyword,', and ',$DeleteKeyword,' Operations for ', $entityName)"/>
					<plx:pragma type="region" data="{$pragmaData}"/>
					<xsl:variable name="createFunctionName" select="concat($CreateKeyword,$entityName)"/>
					<plx:function visibility="public" name="{$createFunctionName}">
						<plx:attribute dataTypeName="OperationContract">
							<plx:passParam>
								<plx:binaryOperator type="assignNamed">
									<plx:left>
										<plx:nameRef name="IsOneWay"/>
									</plx:left>
									<plx:right>
										<plx:trueKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:passParam>
						</plx:attribute>
						<plx:param dataTypeName="{$entityName}" name="{translate(concat(translate(substring($entityName, 1, 1), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'), substring($entityName, 2)),'_','')}To{$CreateKeyword}"/>
					</plx:function>
					<xsl:for-each select="dcl:uniquenessConstraint">
						<plx:function visibility="public">
							<xsl:attribute name="name">
								<xsl:value-of select="concat($ReadKeyword,$entityName,'By')"/>
								<xsl:choose>
									<xsl:when test="@isPrimary = 'true' or @isPrimary = 1">
										<xsl:value-of select="$IsPrimaryKeyword"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:for-each select="dcl:columnRef">
											<xsl:value-of select="concat(translate(substring(@name, 1, 1), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), substring(@name, 2))"/>
											<xsl:if test="position() != last()">
												<xsl:text>And</xsl:text>
											</xsl:if>
										</xsl:for-each>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<plx:attribute dataTypeName="OperationContract">
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
							</plx:attribute>
							<xsl:for-each select="dcl:columnRef">
								<plx:param name="{translate(@name,'_','')}" >
									<xsl:variable name="column" select="../../dcl:column[@name=current()/@name]"/>
									<xsl:attribute name="dataTypeName">
										<xsl:choose>
											<xsl:when test="$column/dcl:domainRef[@name = $Enumerations/@name]">
												<xsl:value-of select="$column/dcl:domainRef/@name"/>
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
						</plx:function>
					</xsl:for-each>
					<xsl:variable name="updateFunctionName" select="concat($UpdateKeyword,$entityName)"/>
					<plx:function visibility="public" name="{$updateFunctionName}">
						<plx:attribute dataTypeName="OperationContract">
							<plx:passParam>
								<plx:binaryOperator type="assignNamed">
									<plx:left>
										<plx:nameRef name="IsOneWay"/>
									</plx:left>
									<plx:right>
										<plx:trueKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:passParam>
						</plx:attribute>
						<plx:param dataTypeName="{@name}" name="{concat(translate(substring(@name, 1, 1), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'), substring(@name, 2))}To{$UpdateKeyword}"/>
					</plx:function>
					<plx:function name="{$DeleteKeyword}{@name}" visibility="public">
						<plx:attribute dataTypeName="OperationContract">
							<plx:passParam>
								<plx:binaryOperator type="assignNamed">
									<plx:left>
										<plx:nameRef name="IsOneWay"/>
									</plx:left>
									<plx:right>
										<plx:trueKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:passParam>
						</plx:attribute>
						<plx:param dataTypeName="{$entityName}" name="{concat(translate(substring($entityName, 1, 1), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'), substring($entityName, 2))}To{$DeleteKeyword}"/>
					</plx:function>
					<plx:pragma type="closeRegion" data="{$pragmaData}"/>
				</xsl:for-each>
			</plx:interface>
		</xsl:if>
	</xsl:template>

	<xsl:template match="dcl:table" mode="GenerateBusinessEntities">
		<xsl:variable name="entityName" select="@name"/>
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
					<xsl:variable name="tableName" select="$entityName"/>
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
			<xsl:variable name="entitySetEntities" select="../dcl:table[dcl:referenceConstraint/@targetTable = $entityName]"/>
			<xsl:variable name="generateServiceLayerInitializationFunction" select="$GenerateServiceLayer and $entitySetEntities"/>
			<plx:function visibility="public" name=".construct">
				<xsl:choose>
					<xsl:when test="$generateServiceLayerInitializationFunction">
						<plx:callThis accessor="this" name="{$InitializeFunctionName}" type="methodCall"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="$entitySetEntities" mode="GenerateSerivceFieldInitialization"/>
					</xsl:otherwise>
				</xsl:choose>
			</plx:function>
			<xsl:variable name="iNotifyPropertyPragmaData" select="concat('INotifyPropertyChanging and INotifyPropertyChanged Information for ',$entityName)"/>
			<plx:pragma type="region" data="{$iNotifyPropertyPragmaData}"/>
			<xsl:call-template name="GenerateINotifyPropertyChangingImplementation"/>
			<xsl:call-template name="GenerateINotifyPropertyChangedImplementation"/>
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
							<plx:callStatic dataTypeName="EditorBrowsableState" name="Never" type="property"/>
						</plx:passParam>
					</plx:attribute>
					<plx:param dataTypeName="StreamingContext" name="context"/>
					<plx:assign>
						<plx:left>
							<plx:callThis accessor="this" name="{$serializationFieldName}" type="field"/>
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
							<plx:callStatic dataTypeName="EditorBrowsableState" name="Never" type="property"/>
						</plx:passParam>
					</plx:attribute>
					<plx:param dataTypeName="StreamingContext" name="context"/>
					<plx:assign>
						<plx:left>
							<plx:callThis accessor="this" name="{$serializationFieldName}" type="field"/>
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
							<plx:callStatic dataTypeName="EditorBrowsableState" name="Never" type="property"/>
						</plx:passParam>
					</plx:attribute>
					<plx:param dataTypeName="StreamingContext" name="context"/>
					<plx:callThis accessor="this" name="{$InitializeFunctionName}" type="methodCall"/>
				</plx:function>
				<plx:pragma type="closeRegion" data="{$serializationPragmaData}"/>
			</xsl:if>
		</plx:class>
	</xsl:template>

	<xsl:template match="dcl:table" mode="GenerateSerivceFieldInitialization">
		<xsl:variable name="entitySetEntityName" select="@name"/>
		<xsl:variable name="entitySetFieldName" select="concat($PrivateMemberPrefix,concat(translate(substring($entitySetEntityName, 1, 1), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), substring($entitySetEntityName, 2)),$CollectionSuffix)"/>
		<plx:assign>
			<plx:left>
				<plx:callThis accessor="this" type="field" name="{$entitySetFieldName}"/>
			</plx:left>
			<plx:right>
				<plx:callNew dataTypeName="EntitySet">
					<plx:passTypeParam dataTypeName="{$entitySetEntityName}"/>
					<plx:passParam>
						<plx:callNew dataTypeName="Action">
							<plx:passTypeParam dataTypeName="{$entitySetEntityName}"/>
							<plx:passParam>
								<plx:callThis accessor="this" name="On{$entitySetEntityName}Added" type="fireCustomEvent"/>
							</plx:passParam>
						</plx:callNew>
					</plx:passParam>
					<plx:passParam>
						<plx:callNew dataTypeName="Action">
							<plx:passTypeParam dataTypeName="{$entitySetEntityName}"/>
							<plx:passParam>
								<plx:callThis accessor="this" type="fireCustomEvent" name="On{$entitySetEntityName}Removed"/>
							</plx:passParam>
						</plx:callNew>
					</plx:passParam>
				</plx:callNew>
			</plx:right>
		</plx:assign>
	</xsl:template>

	<xsl:template match="dcl:table" mode="GenerateEntitySetMembers">
		<xsl:param name="containingEntity"/>
		<xsl:variable name="oppositeEntity" select="."/>
		<xsl:variable name="containingEntityName" select="string($containingEntity/@name)"/>
		<xsl:variable name="entityName" select="@name"/>
		<xsl:variable name="oppositeRoleName" select="@name"/>
		<xsl:variable name="entityFieldName" select="concat($PrivateMemberPrefix,concat(translate(substring($entityName, 1, 1), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), substring($entityName, 2)),$CollectionSuffix)"/>
		<xsl:variable name="entityPropertyName" select="concat($entityName,$CollectionSuffix)"/>
		<xsl:variable name="propertyPragmaData" select="concat('PropertyChanging and PropertyChanged Information for ',$entityPropertyName)"/>
		<plx:pragma type="region" data="{$propertyPragmaData}"/>
		<plx:field visibility="private" static="true" readOnly="true" dataTypeName="PropertyChangingEventArgs" name="{$entityName}PropertyChangingEventArgs">
			<plx:initialize>
				<plx:callNew dataTypeName="PropertyChangingEventArgs">
					<plx:passParam>
						<plx:string data="{$entityName}"/>
					</plx:passParam>
				</plx:callNew>
			</plx:initialize>
		</plx:field>
		<plx:field visibility="private" static="true" readOnly="true" dataTypeName="PropertyChangedEventArgs" name="{$entityName}PropertyChangedEventArgs">
			<plx:initialize>
				<plx:callNew dataTypeName="PropertyChangedEventArgs">
					<plx:passParam>
						<plx:string data="{$entityName}"/>
					</plx:passParam>
				</plx:callNew>
			</plx:initialize>
		</plx:field>
		<xsl:variable name="entityPropertyChangingEventArgsName" select="concat($entityName,'PropertyChangingEventArgs')"/>
		<xsl:variable name="entityPropertyChangedEventArgsName" select="concat($entityName,'PropertyChangedEventArgs')"/>
		<plx:function visibility="private" name="On{$entityName}Added">
			<plx:param dataTypeName="{$entityName}" name="entity"/>
			<plx:callThis accessor="this" name="OnPropertyChanging">
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
			<plx:callThis accessor="this" name="OnPropertyChanged">
				<plx:passParam>
					<plx:callThis accessor="static" type="field" name="{$entityPropertyChangedEventArgsName}"/>
				</plx:passParam>
			</plx:callThis>
		</plx:function>
		<plx:function visibility="private" name="On{$entityName}Removed">
			<plx:param dataTypeName="{$entityName}" name="entity"/>
			<plx:callThis accessor="this" name="OnPropertyChanging">
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
			<plx:callThis accessor="this" name="OnPropertyChanged">
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
										<xsl:variable name="oppositeEntityPropertyName" >
											<xsl:value-of select="$oppositeEntity/dcl:referenceConstraint[@targetName = current()/@name]/@sourceName"/>
										</xsl:variable>
										<xsl:value-of select="translate(concat(translate(substring($oppositeEntityPropertyName, 1, 1), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), substring($oppositeEntityPropertyName, 2)),'_','')"/>
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
									<plx:callThis type="field" accessor="this" name="{$PrivateMemberPrefix}serializing"/>
								</plx:left>
								<plx:right>
									<plx:unaryOperator type="booleanNot">
										<plx:callInstance name="HasLoadedOrAssignedValues" type="property">
											<plx:callObject>
												<plx:callThis accessor="this" type="field" name="{$entityFieldName}"/>
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
					<plx:callThis accessor="this" type="field" name="{$entityFieldName}"/>
				</plx:return>
			</plx:get>
			<plx:set>
				<plx:callInstance type="methodCall" name="Assign">
					<plx:callObject>
						<plx:callThis name="{$entityFieldName}" accessor="this" type="field"/>
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
		<xsl:variable name="columnPropertyName" select="translate(concat(translate(substring(@name, 1, 1), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), substring(@name, 2)),'_','')"/>
		<xsl:variable name="columnFieldName" select="concat($PrivateMemberPrefix,translate($columnName,'_',''))"/>
		<xsl:variable name="columnIsNullable" select="@isNullable = 'true' or @isNullable = 1"/>
		<xsl:variable name="columnIsStringType" select="dcl:predefinedDataType/@name = 'CHARACTER VARYING' or dcl:predefinedDataType/@name = 'CHARACTER' or dcl:predefinedDataType/@name = 'CHARACTER LARGE OBJECT'"/>
		<xsl:call-template name="GeneratePropertyChangeInformation">
			<xsl:with-param name="propertyName" select="$columnPropertyName"/>
		</xsl:call-template>
		<plx:field visibility="private" name="{$columnFieldName}">
			<xsl:choose>
				<xsl:when test="($columnIsNullable) and not($columnIsStringType)">
					<xsl:attribute name="dataTypeName">
						<xsl:value-of select="'Nullable'"/>
					</xsl:attribute>
					<plx:passTypeParam>
						<xsl:choose>
							<xsl:when test="dcl:domainRef[@name = $Enumerations/@name]">
								<xsl:attribute name="dataTypeName">
									<xsl:value-of select="dcl:domainRef/@name"/>
								</xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="dataTypeName">
									<xsl:call-template name="GetDotNetTypeFromDcilPredefinedDataType">
										<xsl:with-param name="predefinedDataType" select="dcl:predefinedDataType"/>
										<xsl:with-param name="column" select="."/>
									</xsl:call-template>
								</xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
					</plx:passTypeParam>
				</xsl:when>
				<xsl:otherwise>
					<xsl:choose>
						<xsl:when test="dcl:domainRef[@name = $Enumerations/@name]">
							<xsl:attribute name="dataTypeName">
								<xsl:value-of select="dcl:domainRef/@name"/>
							</xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="dataTypeName">
								<xsl:call-template name="GetDotNetTypeFromDcilPredefinedDataType">
									<xsl:with-param name="predefinedDataType" select="dcl:predefinedDataType"/>
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
					<xsl:if test="$EmitDefaultValue">
						<plx:passParam>
							<plx:binaryOperator type="assignNamed">
								<plx:left>
									<plx:nameRef name="EmitDefaultValue"/>
								</plx:left>
								<plx:right>
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
					</xsl:if>
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
								<plx:string data="{$columnName}"/>
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
									<xsl:when test="@isNullable = 'false' or @isNullable = 0">
										<plx:falseKeyword/>
									</xsl:when>
									<xsl:otherwise>
										<plx:trueKeyword/>
									</xsl:otherwise>
								</xsl:choose>
							</plx:right>
						</plx:binaryOperator>
					</plx:passParam>
					<xsl:if test="../dcl:uniquenessConstraint[@isPrimary='true' or @IsPrimary = 1]/dcl:columnRef[@name = current()/@name]">
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
					<xsl:if test="@isIdentity = 'true' or @isIdentity = 1">
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
					<xsl:when test="(@isNullable = 'true' or @isNullable = 1) and not(dcl:predefinedDataType/@name = 'CHARACTER VARYING') and not(dcl:predefinedDataType/@name = 'CHARACTER') and not(dcl:predefinedDataType/@name = 'CHARACTER LARGE OBJECT')">
						<xsl:attribute name="dataTypeName">
							<xsl:value-of select="'Nullable'"/>
						</xsl:attribute>
						<plx:passTypeParam>
							<xsl:choose>
								<xsl:when test="dcl:domainRef[@name = $Enumerations/@name]">
									<xsl:attribute name="dataTypeName">
										<xsl:value-of select="dcl:domainRef/@name"/>
									</xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="dataTypeName">
										<xsl:call-template name="GetDotNetTypeFromDcilPredefinedDataType">
											<xsl:with-param name="predefinedDataType" select="dcl:predefinedDataType"/>
											<xsl:with-param name="column" select="."/>
										</xsl:call-template>
									</xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
						</plx:passTypeParam>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="dcl:domainRef[@name = $Enumerations/@name]">
								<xsl:attribute name="dataTypeName">
									<xsl:value-of select="dcl:domainRef/@name"/>
								</xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="dataTypeName">
									<xsl:call-template name="GetDotNetTypeFromDcilPredefinedDataType">
										<xsl:with-param name="predefinedDataType" select="dcl:predefinedDataType"/>
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
					<plx:callThis accessor="this" type="field" name="{$columnFieldName}" />
				</plx:return>
			</plx:get>
			<plx:set>
				<!--<xsl:if test="@isMandatory">
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="identityEquality">
								<plx:left>
									<plx:valueKeyword/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:throw>
							<plx:callNew dataTypeName="ArgumentNullException"/>
						</plx:throw>
					</plx:branch>
				</xsl:if>-->
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<plx:callThis accessor="this" type="field" name="{$columnFieldName}" />
							</plx:left>
							<plx:right>
								<plx:valueKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:callThis accessor="this" type="methodCall" name="OnPropertyChanging">
						<plx:passParam>
							<plx:nameRef name="{$columnPropertyName}PropertyChangingEventArgs"/>
						</plx:passParam>
					</plx:callThis>
					<plx:assign>
						<plx:left>
							<plx:callThis accessor="this" type="field" name="{$columnFieldName}" />
						</plx:left>
						<plx:right>
							<plx:valueKeyword/>
						</plx:right>
					</plx:assign>
					<plx:callThis accessor="this" type="methodCall" name="OnPropertyChanged">
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
		<plx:field visibility="private" static="true" readOnly="true" dataTypeName="PropertyChangingEventArgs" name="{$propertyName}PropertyChangingEventArgs">
			<plx:initialize>
				<plx:callNew dataTypeName="PropertyChangingEventArgs">
					<plx:passParam>
						<plx:string data="{$propertyName}"/>
					</plx:passParam>
				</plx:callNew>
			</plx:initialize>
		</plx:field>
		<plx:field visibility="private" static="true" readOnly="true" dataTypeName="PropertyChangedEventArgs" name="{$propertyName}PropertyChangedEventArgs">
			<plx:initialize>
				<plx:callNew dataTypeName="PropertyChangedEventArgs">
					<plx:passParam>
						<plx:string data="{$propertyName}"/>
					</plx:passParam>
				</plx:callNew>
			</plx:initialize>
		</plx:field>
		<plx:pragma type="closeRegion" data="{$propertyPragmaData}"/>
	</xsl:template>

	<xsl:template match="dcl:referenceConstraint" mode="GenerateEntityMembers">
		<xsl:variable name="entityName" select="@targetTable"/>
		<xsl:variable name="propertyName" select="@targetTable"/>
		<xsl:variable name="fieldName" select="concat($PrivateMemberPrefix,concat(translate(substring($propertyName, 1, 1), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), substring($propertyName, 2)))"/>
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
										<xsl:value-of select="translate(concat(translate(substring(@sourceName, 1, 1), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), substring(@sourceName, 2)),'_','')"/>
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
							<plx:callThis accessor="this" type="field" name="{$fieldName}"/>
						</plx:callObject>
					</plx:callInstance>
				</plx:return>
			</plx:get>
			<plx:set>
				<!--<xsl:if test="@isNullable = 'false' or @isNullable = 0">
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="identityEquality">
								<plx:left>
									<plx:valueKeyword/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:throw>
							<plx:callNew dataTypeName="ArgumentNullException"/>
						</plx:throw>
					</plx:branch>
				</xsl:if>-->
				<plx:local name="previousValue" dataTypeName="{$entityName}">
					<plx:initialize>
						<plx:callInstance type="property" name="Entity">
							<plx:callObject>
								<plx:callThis accessor="this" type="field" name="{$fieldName}"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<xsl:variable name="entityRefsEntitySetPropertyName" select="concat(../@name,$CollectionSuffix)"/>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="booleanOr">
							<plx:left>
								<plx:unaryOperator type="booleanNot">
									<plx:callInstance type="property" name="HasLoadedOrAssignedValue">
										<plx:callObject>
											<plx:callThis type="field" accessor="this" name="{$fieldName}"/>
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
					<plx:callThis accessor="this" name="OnPropertyChanging">
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
										<plx:callThis type="field" name="{$fieldName}" accessor="this"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:assign>
						<plx:callInstance type="methodCall" name="Remove">
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
									<plx:callThis accessor="this" type="field" name="{$fieldName}"/>
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
						<plx:callInstance type="methodCall" name="Add">
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
									<plx:callThis accessor="this" type="field" name="{translate(concat(translate(substring(@sourceName, 1, 1), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), substring(@sourceName, 2)),'_','')}"/>
								</plx:left>
								<plx:right>
									<plx:callInstance type="field" name="{translate(concat(translate(substring(@targetName, 1, 1), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), substring(@targetName, 2)),'_','')}">
										<plx:callObject>
											<plx:valueKeyword/>
										</plx:callObject>
									</plx:callInstance>
								</plx:right>
							</plx:assign>
						</xsl:for-each>
					</plx:branch>
					<plx:callThis accessor="this" name="OnPropertyChanged">
						<plx:passParam>
							<plx:callThis accessor="static" type="field" name="{$propertyName}PropertyChangedEventArgs"/>
						</plx:passParam>
					</plx:callThis>
				</plx:branch>
			</plx:set>
		</plx:property>
	</xsl:template>

	<xsl:template match="dcl:domain" mode="GenerateEnumerations">
		<xsl:param name="enumName" select="@name"/>
		<plx:enum visibility="public" name="{$enumName}">
			<plx:attribute dataTypeName="Serializable"/>
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
				<xsl:variable name="enumItem" select="@value"/>
				<plx:enumItem name="{$enumItem}">
					<xsl:if test="$GenerateServiceLayer">
						<plx:attribute dataTypeName="EnumMember">
							<plx:passParam>
								<plx:binaryOperator type="assignNamed">
									<plx:left>
										<plx:nameRef name="Value"/>
									</plx:left>
									<plx:right>
										<!--TODO: Set this via a settings file.-->
										<plx:string data="{$enumItem}"/>
									</plx:right>
								</plx:binaryOperator>
							</plx:passParam>
						</plx:attribute>
					</xsl:if>
				</plx:enumItem>
			</xsl:for-each>
		</plx:enum>
	</xsl:template>

	<xsl:template match="dcl:schema" mode="GenerateDatabaseContext">
		<xsl:param name="modelName" select="$ModelName"/>
		<xsl:param name="fullyQualifiedNamespace" select="$DefaultNamespace"/>
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
			<xsl:if test="$GenerateServiceLayer">
				<!--<plx:attribute dataTypeName="ServiceBehavior">
						<plx:passParam>
							<plx:binaryOperator type="assignNamed">
								<plx:left>
								</plx:left>
								<plx:right>
								</plx:right>
							</plx:binaryOperator>
						</plx:passParam>
					</plx:attribute>-->
			</xsl:if>
			<plx:derivesFromClass dataTypeName="DataContext"/>
			<xsl:if test="$GenerateServiceLayer">
				<plx:implementsInterface dataTypeName="I{$ModelName}Service"/>
			</xsl:if>
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
							<plx:callStatic dataTypeName="XmlMappingSource" name="FromXml" type="methodCall">
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
			<xsl:if test="$GenerateServiceLayer">
				<xsl:for-each select="dcl:table">
					<xsl:variable name="entityName" select="@name"/>
					<xsl:variable name="interfaceImplementationPragmaData" select="concat('I',$ModelName,' ',$CreateKeyword,', ',$ReadKeyword,', ',$UpdateKeyword,', and ',$DeleteKeyword,' Implementation for ',$entityName)"/>
					<plx:pragma type="region" data="{$interfaceImplementationPragmaData}"/>
					<xsl:variable name="createEntityFunctionName" select="concat($CreateKeyword,$entityName)"/>
					<plx:function name="{$createEntityFunctionName}" visibility="public">
						<!--<plx:attribute dataTypeName="OperationContract">
							<plx:passParam>
								<plx:binaryOperator type="assignNamed">
									<plx:left>
										<plx:nameRef name="IsOneWay"/>
									</plx:left>
									<plx:right>
										<plx:trueKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:passParam>
						</plx:attribute>-->
						<xsl:variable name="parameterName" select="concat(translate(concat(translate(substring($entityName, 1, 1), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'), substring($entityName, 2)),'_',''),'To',$CreateKeyword)"/>
						<plx:param dataTypeName="{$entityName}" name="{$parameterName}"/>
						<!--<plx:try>-->
						<plx:callInstance name="InsertOnSubmit" type="methodCall">
							<plx:callObject>
								<plx:callThis accessor="this" type="property" name="{$entityName}{$TableSuffix}"></plx:callThis>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="{$parameterName}"/>
							</plx:passParam>
						</plx:callInstance>
						<plx:callThis accessor="this" type="methodCall" name="SubmitChanges">
							<plx:passParam>
								<plx:callInstance type="property" name="FailOnFirstConflict">
									<plx:callObject>
										<plx:nameRef name="ConflictMode"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:passParam>
						</plx:callThis>
						<!--<plx:catch>
								
							</plx:catch>
							<plx:fallbackCatch>
								
							</plx:fallbackCatch>
						</plx:try>-->
					</plx:function>
					<xsl:for-each select="dcl:uniquenessConstraint">
						<plx:function visibility="public">
							<xsl:attribute name="name">
								<xsl:value-of select="concat($ReadKeyword,$entityName,'By')"/>
								<xsl:choose>
									<xsl:when test="@isPrimary = 'true' or @isPrimary = 1">
										<xsl:value-of select="$IsPrimaryKeyword"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:for-each select="dcl:columnRef">
											<xsl:value-of select="concat(translate(substring(@name, 1, 1), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), substring(@name, 2))"/>
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
								<plx:param name="{translate(@name,'_','')}" >
									<xsl:variable name="column" select="../../dcl:column[@name=current()/@name]"/>
									<xsl:attribute name="dataTypeName">
										<xsl:choose>
											<xsl:when test="$column/dcl:domainRef[@name = $Enumerations/@name]">
												<!--<xsl:when test="$column/dcl:domainRef[@name = /dcl:schema/dcl:domain[dcl:predefinedDataType/@name = 'CHARACTER VARYING' or dcl:predefinedDataType/@name = 'CHARACTER' or dcl:predefinedDataType/@name = 'CHARACTER LARGE OBJECT' and count(dcl:checkConstraint) = 1 and count(dcl:checkConstraint/dep:inPredicate) = 1 and dcl:checkConstraint/dep:inPredicate/@type='IN']/@name]">-->
												<xsl:value-of select="$column/dcl:domainRef/@name"/>
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
							<plx:return>
								<plx:callInstance name="First" type="methodCall">
									<plx:callObject>
										<plx:callInstance name="Where" type="methodCall">
											<xsl:variable name="typeTableAccessor" select="concat($entityName,$TableSuffix)"/>
											<plx:callObject>
												<plx:nameRef type="parameter" name="{$typeTableAccessor}"/>
											</plx:callObject>
											<plx:passParam>
												<xsl:variable name="parameterName" select="concat(translate(concat(translate(substring($entityName, 1, 1), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'), substring($entityName, 2)),'_',''),'To',$ReadKeyword)"/>
												<plx:anonymousFunction>
													<plx:param dataTypeName="{$entityName}" name="{$parameterName}"/>
													<plx:return>
														<!--<xsl:call-template name="CombineExpressions">
															<xsl:with-param name="Expressions" select="dcl:columnRef"/>
														</xsl:call-template>-->
														<xsl:for-each select="dcl:columnRef">

															<plx:binaryOperator type="equality">
																<plx:left>
																	<plx:callInstance name="{translate(concat(translate(substring(@name, 1, 1), 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), substring(@name, 2)),'_','')}" type="property">
																		<plx:callObject>
																			<plx:nameRef name="{$parameterName}" type="parameter"/>
																		</plx:callObject>
																	</plx:callInstance>
																</plx:left>
																<plx:right>
																	<plx:nameRef name="{translate(@name,'_','')}"/>
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
						<!--<plx:attribute dataTypeName="OperationContract">
							<plx:passParam>
								<plx:binaryOperator type="assignNamed">
									<plx:left>
										<plx:nameRef name="IsOneWay"/>
									</plx:left>
									<plx:right>
										<plx:trueKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:passParam>
						</plx:attribute>-->
						<xsl:variable name="parameterName" select="concat(translate(concat(translate(substring(@name, 1, 1), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'), substring(@name, 2)),'_',''),'To',$UpdateKeyword)"/>
						<plx:param dataTypeName="{@name}" name="{$parameterName}"/>
						<plx:callInstance name="Attach" type="methodCall">
							<plx:callObject>
								<plx:callThis accessor="this" type="property" name="{@name}{$TableSuffix}"></plx:callThis>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="{$parameterName}"/>
							</plx:passParam>
							<plx:passParam>
								<plx:callInstance name="GetOriginalEntityState" type="methodCall">
									<plx:callObject>
										<plx:callThis accessor="this" type="property" name="{@name}{$TableSuffix}"></plx:callThis>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="{$parameterName}"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:passParam>
						</plx:callInstance>
						<plx:callThis accessor="this" type="methodCall" name="SubmitChanges">
							<plx:passParam>
								<plx:callInstance type="property" name="FailOnFirstConflict">
									<plx:callObject>
										<plx:nameRef name="ConflictMode"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:passParam>
						</plx:callThis>
					</plx:function>
					<xsl:variable name="deleteEntityFunctionName" select="concat($DeleteKeyword,$entityName)"/>
					<plx:function name="{$deleteEntityFunctionName}" visibility="public">
						<!--<plx:attribute dataTypeName="OperationContract">
							<plx:passParam>
								<plx:binaryOperator type="assignNamed">
									<plx:left>
										<plx:nameRef name="IsOneWay"/>
									</plx:left>
									<plx:right>
										<plx:trueKeyword/>
									</plx:right>
								</plx:binaryOperator>
							</plx:passParam>
						</plx:attribute>-->
						<xsl:variable name="parameterName" select="concat(translate(concat(translate(substring($entityName, 1, 1), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'), substring($entityName, 2)),'_',''),'To',$DeleteKeyword)"/>
						<plx:param dataTypeName="{$entityName}" name="{$parameterName}"/>
						<plx:callInstance name="DeleteOnSubmit" type="methodCall">
							<plx:callObject>
								<plx:callThis accessor="this" type="property" name="{$entityName}{$TableSuffix}"></plx:callThis>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="{$parameterName}"/>
							</plx:passParam>
						</plx:callInstance>
						<plx:callInstance name="Attach" type="methodCall">
							<plx:callObject>
								<plx:callThis accessor="this" type="property" name="{@name}{$TableSuffix}"></plx:callThis>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="{$parameterName}"/>
							</plx:passParam>
							<plx:passParam>
								<plx:falseKeyword/>
							</plx:passParam>
						</plx:callInstance>
						<plx:callThis accessor="this" type="methodCall" name="SubmitChanges">
							<plx:passParam>
								<plx:callInstance type="property" name="FailOnFirstConflict">
									<plx:callObject>
										<plx:nameRef name="ConflictMode"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:passParam>
						</plx:callThis>
					</plx:function>
					<plx:pragma type="closeRegion" data="{$interfaceImplementationPragmaData}"/>
				</xsl:for-each>
			</xsl:if>
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
		<xsl:variable name="tableName" select="@name"/>
		<plx:property name="{$tableName}{$TableSuffix}" visibility="public">
			<plx:returns dataTypeName="Table">
				<plx:passTypeParam dataTypeName="{$tableName}"/>
			</plx:returns>
			<plx:get>
				<plx:return>
					<plx:callThis accessor="this" name="GetTable">
						<plx:passMemberTypeParam dataTypeName="{$tableName}"/>
					</plx:callThis>
				</plx:return>
			</plx:get>
		</plx:property>
	</xsl:template>

	<xsl:template name="GenerateINotifyPropertyChangedImplementation">
		<plx:field visibility="private" name="{$PrivateMemberPrefix}propertyChangedEventHandler" dataTypeName="PropertyChangedEventHandler"/>
		<plx:event visibility="privateInterfaceMember" name="PropertyChanged">
			<!-- Suppress the 'InterfaceMethodsShouldBeCallableByChildTypes' FxCop warning, since it is not applicable here. -->
			<!-- Child types call the property-specific notification methods, which in turn raise the INotifyPropertyChanged.PropertyChanged event. -->
			<xsl:call-template name="GenerateSuppressMessageAttribute">
				<xsl:with-param name="category" select="'Microsoft.Design'"/>
				<xsl:with-param name="checkId" select="'CA1033:InterfaceMethodsShouldBeCallableByChildTypes'"/>
			</xsl:call-template>
			<plx:interfaceMember memberName="PropertyChanged" dataTypeName="INotifyPropertyChanged"/>
			<plx:param name="sender" dataTypeName=".object"/>
			<plx:param name="e" dataTypeName="PropertyChangedEventArgs"/>
			<plx:explicitDelegateType dataTypeName="PropertyChangedEventHandler"/>
			<plx:onAdd>
				<xsl:call-template name="GetINotifyPropertyChangedImplementationEventOnAddRemoveCode">
					<xsl:with-param name="MethodName" select="'Combine'"/>
				</xsl:call-template>
			</plx:onAdd>
			<plx:onRemove>
				<xsl:call-template name="GetINotifyPropertyChangedImplementationEventOnAddRemoveCode">
					<xsl:with-param name="MethodName" select="'Remove'"/>
				</xsl:call-template>
			</plx:onRemove>
		</plx:event>
		<plx:function visibility="private" overload="{not($RaiseEventsAsynchronously)}" name="OnPropertyChanged">
			<plx:param name="e" dataTypeName="PropertyChangedEventArgs"/>
			<plx:local name="eventHandler" dataTypeName="PropertyChangedEventHandler"/>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:cast type="exceptionCast" dataTypeName=".object">
								<plx:inlineStatement dataTypeName="PropertyChangedEventHandler">
									<plx:assign>
										<plx:left>
											<plx:nameRef type="local" name="eventHandler"/>
										</plx:left>
										<plx:right>
											<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangedEventHandler"/>
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
						<plx:callStatic name="InvokeEventHandlerAsync" dataTypeName="EventHandlerUtility" type="methodCall">
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
			<plx:function visibility="private" overload="true" name="OnPropertyChanged">
				<plx:param name="e" dataTypeName="PropertyChangedEventArgs"/>
				<plx:local name="eventHandler" dataTypeName="PropertyChangedEventHandler"/>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<plx:cast type="exceptionCast" dataTypeName=".object">
									<plx:inlineStatement dataTypeName="PropertyChangedEventHandler">
										<plx:assign>
											<plx:left>
												<plx:nameRef type="local" name="eventHandler"/>
											</plx:left>
											<plx:right>
												<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangedEventHandler"/>
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

	<xsl:template name="GenerateINotifyPropertyChangingImplementation">
		<plx:field visibility="private" name="{$PrivateMemberPrefix}propertyChangingEventHandler" dataTypeName="PropertyChangingEventHandler"/>
		<plx:event visibility="privateInterfaceMember" name="PropertyChanging">
			<!-- Suppress the 'InterfaceMethodsShouldBeCallableByChildTypes' FxCop warning, since it is not applicable here. -->
			<!-- Child types call the property-specific notification methods, which in turn raise the INotifyPropertyChanging.PropertyChanging event. -->
			<xsl:call-template name="GenerateSuppressMessageAttribute">
				<xsl:with-param name="category" select="'Microsoft.Design'"/>
				<xsl:with-param name="checkId" select="'CA1033:InterfaceMethodsShouldBeCallableByChildTypes'"/>
			</xsl:call-template>
			<plx:interfaceMember memberName="PropertyChanging" dataTypeName="INotifyPropertyChanging"/>
			<plx:param name="sender" dataTypeName=".object"/>
			<plx:param name="e" dataTypeName="PropertyChangingEventArgs"/>
			<plx:explicitDelegateType dataTypeName="PropertyChangingEventHandler"/>
			<plx:onAdd>
				<xsl:call-template name="GetINotifyPropertyChangingImplementationEventOnAddRemoveCode">
					<xsl:with-param name="MethodName" select="'Combine'"/>
				</xsl:call-template>
			</plx:onAdd>
			<plx:onRemove>
				<xsl:call-template name="GetINotifyPropertyChangingImplementationEventOnAddRemoveCode">
					<xsl:with-param name="MethodName" select="'Remove'"/>
				</xsl:call-template>
			</plx:onRemove>
		</plx:event>
		<plx:function visibility="private" overload="{not($RaiseEventsAsynchronously)}" name="OnPropertyChanging">
			<plx:param name="e" dataTypeName="PropertyChangingEventArgs"/>
			<plx:local name="eventHandler" dataTypeName="PropertyChangingEventHandler"/>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:cast type="exceptionCast" dataTypeName=".object">
								<plx:inlineStatement dataTypeName="PropertyChangingEventHandler">
									<plx:assign>
										<plx:left>
											<plx:nameRef type="local" name="eventHandler"/>
										</plx:left>
										<plx:right>
											<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangingEventHandler"/>
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
						<plx:callStatic name="InvokeEventHandlerAsync" dataTypeName="EventHandlerUtility" type="methodCall">
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
			<plx:function visibility="private" overload="true" name="OnPropertyChanging">
				<plx:param name="e" dataTypeName="PropertyChangingEventArgs"/>
				<plx:local name="eventHandler" dataTypeName="PropertyChangingEventHandler"/>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<plx:cast type="exceptionCast" dataTypeName=".object">
									<plx:inlineStatement dataTypeName="PropertyChangingEventHandler">
										<plx:assign>
											<plx:left>
												<plx:nameRef type="local" name="eventHandler"/>
											</plx:left>
											<plx:right>
												<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangingEventHandler"/>
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

	<xsl:template name="GetINotifyPropertyChangingImplementationEventOnAddRemoveCode">
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
					<plx:local name="currentHandler" dataTypeName="PropertyChangingEventHandler"/>
					<plx:loop checkCondition="before">
						<plx:condition>
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:cast type="exceptionCast" dataTypeName=".object">
										<plx:callStatic type="methodCall" name="CompareExchange" dataTypeName="Interlocked">
											<plx:passMemberTypeParam dataTypeName="PropertyChangingEventHandler"/>
											<plx:passParam type="inOut">
												<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangingEventHandler"/>
											</plx:passParam>
											<plx:passParam>
												<plx:cast type="exceptionCast" dataTypeName="PropertyChangingEventHandler">
													<!-- PLIX_TODO: Once the PLiX formatters support keyword filtering, remove the dataTypeQualifier attribute from the next line. -->
													<plx:callStatic type="methodCall" name="{$MethodName}" dataTypeName="Delegate" dataTypeQualifier="System">
														<plx:passParam>
															<plx:inlineStatement dataTypeName="PropertyChangingEventHandler">
																<plx:assign>
																	<plx:left>
																		<plx:nameRef type="local" name="currentHandler"/>
																	</plx:left>
																	<plx:right>
																		<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangingEventHandler"/>
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
							<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangingEventHandler"/>
						</plx:left>
						<plx:right>
							<plx:cast type="exceptionCast" dataTypeName="PropertyChangingEventHandler">
								<!-- PLIX_TODO: Once the PLiX formatters support keyword filtering, remove the dataTypeQualifier attribute from the next line. -->
								<plx:callStatic type="methodCall" name="Combine" dataTypeName="Delegate" dataTypeQualifier="System">
									<plx:passParam>
										<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangingEventHandler"/>
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

	<xsl:template name="GetINotifyPropertyChangedImplementationEventOnAddRemoveCode">
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
					<plx:local name="currentHandler" dataTypeName="PropertyChangedEventHandler"/>
					<plx:loop checkCondition="before">
						<plx:condition>
							<plx:binaryOperator type="identityInequality">
								<plx:left>
									<plx:cast type="exceptionCast" dataTypeName=".object">
										<plx:callStatic type="methodCall" name="CompareExchange" dataTypeName="Interlocked">
											<plx:passMemberTypeParam dataTypeName="PropertyChangedEventHandler"/>
											<plx:passParam type="inOut">
												<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangedEventHandler"/>
											</plx:passParam>
											<plx:passParam>
												<plx:cast type="exceptionCast" dataTypeName="PropertyChangedEventHandler">
													<!-- PLIX_TODO: Once the PLiX formatters support keyword filtering, remove the dataTypeQualifier attribute from the next line. -->
													<plx:callStatic type="methodCall" name="{$MethodName}" dataTypeName="Delegate" dataTypeQualifier="System">
														<plx:passParam>
															<plx:inlineStatement dataTypeName="PropertyChangedEventHandler">
																<plx:assign>
																	<plx:left>
																		<plx:nameRef type="local" name="currentHandler"/>
																	</plx:left>
																	<plx:right>
																		<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangedEventHandler"/>
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
							<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangedEventHandler"/>
						</plx:left>
						<plx:right>
							<plx:cast type="exceptionCast" dataTypeName="PropertyChangedEventHandler">
								<!-- PLIX_TODO: Once the PLiX formatters support keyword filtering, remove the dataTypeQualifier attribute from the next line. -->
								<plx:callStatic type="methodCall" name="Combine" dataTypeName="Delegate" dataTypeQualifier="System">
									<plx:passParam>
										<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangedEventHandler"/>
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
						<plx:callInstance type="methodCall" name="GetInvocationList">
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
					<plx:callInstance type="methodCall" name="BeginInvoke">
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
						<plx:callInstance type="methodCall" name="GetInvocationList">
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
					<plx:callInstance type="methodCall" name="BeginInvoke">
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
				<xsl:value-of select="$predefinedDataType/@length"/>
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

	<xsl:template name="GetDotNetTypeFromDcilPredefinedDataType">
		<xsl:param name="predefinedDataType"/>
		<xsl:param name="column"/>
		<xsl:variable name="predefinedDataTypeName" select="$predefinedDataType/@name"/>
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

	<!--<xsl:template name="GenerateToString">
		<xsl:param name="ClassName"/>
		<xsl:param name="Properties"/>
		<xsl:variable name="nonCollectionProperties" select="$Properties[not(@isCollection='true')]"/>
		<plx:function visibility="public" modifier="override" overload="true" name="ToString">
			<plx:returns dataTypeName=".string"/>
			<plx:return>
				<plx:callThis accessor="this" type="methodCall" name="ToString">
					<plx:passParam>
						<plx:nullKeyword/>
					</plx:passParam>
				</plx:callThis>
			</plx:return>
		</plx:function>
		<plx:function visibility="public" modifier="virtual" overload="true" name="ToString">
			<plx:param name="provider" dataTypeName="IFormatProvider"/>
			<plx:returns dataTypeName=".string"/>
			<plx:return>
				<plx:callStatic name="Format" dataTypeName=".string">
					<plx:passParam>
						<plx:nameRef type="parameter" name="provider"/>
					</plx:passParam>
					<plx:passParam>
						<plx:string>
							<xsl:value-of select="concat($ClassName,'{0}{{{0}{1}')"/>
							<xsl:for-each select="$nonCollectionProperties">
								<xsl:value-of select="concat(@name,' = ')"/>
								<xsl:if test="not(@isCustomType='true')">
									<xsl:value-of select="'&quot;'"/>
								</xsl:if>
								<xsl:value-of select="concat('{',position()+1,'}')"/>
								<xsl:if test="not(@isCustomType='true')">
									<xsl:value-of select="'&quot;'"/>
								</xsl:if>
								<xsl:if test="not(position()=last())">
									<xsl:value-of select="',{0}{1}'"/>
								</xsl:if>
							</xsl:for-each>
							<xsl:value-of select="'{0}}}'"/>
						</plx:string>
					</plx:passParam>
					<plx:passParam>
						<plx:callStatic type="field" name="NewLine" dataTypeName="Environment"/>
					</plx:passParam>
					<plx:passParam>
						<plx:string>
							<xsl:text disable-output-escaping="yes">&amp;#x09;</xsl:text>
						</plx:string>
					</plx:passParam>
					<xsl:for-each select="$nonCollectionProperties">
						<plx:passParam>
							<xsl:choose>
								<xsl:when test="@isCustomType='true'">
									<plx:string>TODO: Recursively call ToString for customTypes...</plx:string>
								</xsl:when>
								<xsl:otherwise>
									<plx:callThis accessor="this" type="property" name="{@name}"/>
								</xsl:otherwise>
							</xsl:choose>
						</plx:passParam>
					</xsl:for-each>
				</plx:callStatic>
			</plx:return>
		</plx:function>
	</xsl:template>

	<xsl:template match="oial:conceptType" mode="GetInformationTypesForPreferredIdentifier">
		-->

</xsl:stylesheet>
