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
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:exsl="http://exslt.org/common"
	xmlns:oil="http://schemas.orm.net/OIAL"
	xmlns:odt="http://schemas.orm.net/ORMDataTypes"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:prop="urn:schemas-orm-net:PLiX:CLI:Properties"
	exclude-result-prefixes="oil odt"
	extension-element-prefixes="exsl">

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:strip-space elements="*"/>

	<xsl:include href="OIALtoPLiX_GenerateGlobalSupportClasses.xslt"/>

	<xsl:param name="GenerateGlobalSupportClasses" select="true()"/>
	<xsl:param name="GenerateCodeAnalysisAttributes" select="true()"/>
	<xsl:param name="GenerateAccessedThroughPropertyAttribute" select="true()"/>
	<xsl:param name="GenerateObjectDataSourceSupport" select="true()"/>
	<xsl:param name="RaiseEventsAsynchronously" select="true()"/>
	<xsl:param name="DefaultNamespace" select="''"/>
	<xsl:param name="PrivateMemberPrefix" select="'_'"/>
	<xsl:param name="ImplementationClassSuffix" select="'Core'"/>
	<xsl:param name="ModelContextInterfaceImplementationVisibility" select="'public'"/>

	<xsl:param name="Int32MaxValue" select="number(2147483647)"/>

	<xsl:param name="GeneratedCodeAttributeFragment">
		<plx:attribute dataTypeName="GeneratedCodeAttribute">
			<plx:passParam>
				<plx:string>OIALtoPLiX</plx:string>
			</plx:passParam>
			<plx:passParam>
				<plx:string>1.0</plx:string>
			</plx:passParam>
		</plx:attribute>
	</xsl:param>
	<xsl:param name="GeneratedCodeAttribute" select="exsl:node-set($GeneratedCodeAttributeFragment)/child::*"/>
	<xsl:param name="StructLayoutAttributeFragment">
		<plx:attribute dataTypeName="StructLayoutAttribute">
			<plx:passParam>
				<plx:callStatic type="field" name="Auto" dataTypeName="LayoutKind"/>
			</plx:passParam>
			<plx:passParam>
				<plx:binaryOperator type="assignNamed">
					<plx:left>
						<plx:nameRef type="namedParameter" name="CharSet"/>
					</plx:left>
					<plx:right>
						<plx:callStatic type="field" name="Auto" dataTypeName="CharSet"/>
					</plx:right>
				</plx:binaryOperator>
			</plx:passParam>
		</plx:attribute>
	</xsl:param>
	<xsl:param name="StructLayoutAttribute" select="exsl:node-set($StructLayoutAttributeFragment)/child::*"/>


	<xsl:template name="GenerateCLSCompliantAttributeIfNecessary">
		<xsl:variable name="dataTypeFragment">
			<xsl:choose>
				<xsl:when test="string-length(@dataTypeName)">
					<xsl:copy-of select="."/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="prop:DataType"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="dataType" select="exsl:node-set($dataTypeFragment)/child::*"/>
		<xsl:choose>
			<xsl:when test="starts-with($dataType/@dataTypeName,'.u')">
				<plx:attribute dataTypeName="CLSCompliantAttribute">
					<plx:passParam>
						<plx:falseKeyword/>
					</plx:passParam>
				</plx:attribute>
			</xsl:when>
			<xsl:otherwise>
				<xsl:for-each select="$dataType/child::*">
					<xsl:call-template name="GenerateCLSCompliantAttributeIfNecessary"/>
				</xsl:for-each>
			</xsl:otherwise>
		</xsl:choose>
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
	
	<xsl:template match="/">
		<plx:root>
			<plx:namespaceImport name="System"/>
			<plx:namespaceImport name="System.Collections.Generic"/>
			<plx:namespaceImport name="System.Collections.ObjectModel"/>
			<plx:namespaceImport name="System.ComponentModel"/>
			<plx:namespaceImport name="System.Xml"/>
			<xsl:if test="$GenerateCodeAnalysisAttributes">
				<plx:namespaceImport alias="SuppressMessageAttribute" name="System.Diagnostics.CodeAnalysis.SuppressMessageAttribute"/>
			</xsl:if>
			<xsl:if test="$GenerateAccessedThroughPropertyAttribute">
				<plx:namespaceImport alias="AccessedThroughPropertyAttribute" name="System.Runtime.CompilerServices.AccessedThroughPropertyAttribute"/>
			</xsl:if>
			<plx:namespaceImport alias="GeneratedCodeAttribute" name="System.CodeDom.Compiler.GeneratedCodeAttribute"/>
			<plx:namespaceImport alias="StructLayoutAttribute" name="System.Runtime.InteropServices.StructLayoutAttribute"/>
			<plx:namespaceImport alias="LayoutKind" name="System.Runtime.InteropServices.LayoutKind"/>
			<plx:namespaceImport alias="CharSet" name="System.Runtime.InteropServices.CharSet"/>
			<xsl:if test="$GenerateGlobalSupportClasses">
				<xsl:call-template name="GenerateGlobalSupportClasses">
					<xsl:with-param name="StructLayoutAttribute" select="$StructLayoutAttribute"/>
				</xsl:call-template>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="$DefaultNamespace">
					<plx:namespace name="{$DefaultNamespace}">
						<xsl:apply-templates select="oil:model" mode="OIALtoPLiX"/>
					</plx:namespace>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="oil:model" mode="OIALtoPLiX"/>
				</xsl:otherwise>
			</xsl:choose>
		</plx:root>
	</xsl:template>
	
	<xsl:template match="oil:model" mode="OIALtoPLiX">
		<xsl:variable name="Model" select="."/>
		<xsl:variable name="ModelName" select="$Model/@name"/>
		<xsl:variable name="ModelContextName" select="concat($ModelName,'Context')"/>
		<xsl:variable name="ConceptTypes" select="$Model//oil:conceptType"/>
		<xsl:variable name="InformationTypeFormatMappingsFragment">
			<xsl:apply-templates select="$Model/oil:informationTypeFormats/child::*" mode="GenerateInformationTypeFormatMapping"/>
		</xsl:variable>
		<xsl:variable name="InformationTypeFormatMappings" select="exsl:node-set($InformationTypeFormatMappingsFragment)/child::*"/>
		<xsl:variable name="AllPropertiesFragment">
			<xsl:variable name="ConceptTypeRefs" select="$Model//oil:conceptTypeRef"/>
			<xsl:for-each select="$ConceptTypes">
				<prop:Properties conceptTypeName="{@name}">
					<xsl:apply-templates select="." mode="GenerateProperties">
						<xsl:with-param name="ConceptTypeRefs" select="$ConceptTypeRefs"/>
						<xsl:with-param name="InformationTypeFormatMappings" select="$InformationTypeFormatMappings"/>
					</xsl:apply-templates>
				</prop:Properties>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="AllProperties" select="exsl:node-set($AllPropertiesFragment)/child::*"/>
		<xsl:variable name="AllRoleSequenceUniquenessConstraints" select="$Model//oil:roleSequenceUniquenessConstraint"/>

		<plx:namespace name="{$ModelName}">

			<xsl:for-each select="$ConceptTypes">
				<xsl:apply-templates select="." mode="GenerateAbstractClass">
					<xsl:with-param name="Model" select="$Model"/>
					<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
					<xsl:with-param name="InformationTypeFormatMappings" select="$InformationTypeFormatMappings"/>
					<xsl:with-param name="AllProperties" select="$AllProperties"/>
					<xsl:with-param name="Properties" select="$AllProperties[@conceptTypeName=current()/@name]/prop:Property"/>
				</xsl:apply-templates>
			</xsl:for-each>

			<plx:interface visibility="public" name="IHas{$ModelContextName}">
				<plx:leadingInfo>
					<plx:pragma type="region" data="IHas{$ModelContextName}"/>
				</plx:leadingInfo>
				<plx:trailingInfo>
					<plx:pragma type="closeRegion" data="IHas{$ModelContextName}"/>
				</plx:trailingInfo>
				<xsl:copy-of select="$GeneratedCodeAttribute"/>
				<plx:property visibility="public" modifier="abstract" name="Context">
					<plx:returns dataTypeName="{$ModelContextName}"/>
					<plx:get/>
				</plx:property>
			</plx:interface>
			<plx:interface visibility="public" name="I{$ModelContextName}">
				<plx:leadingInfo>
					<plx:pragma type="region" data="I{$ModelContextName}"/>
				</plx:leadingInfo>
				<plx:trailingInfo>
					<plx:pragma type="closeRegion" data="I{$ModelContextName}"/>
				</plx:trailingInfo>
				<xsl:copy-of select="$GeneratedCodeAttribute"/>
				<xsl:call-template name="GenerateModelContextInterfaceLookupAndExternalConstraintEnforcementMembers">
					<xsl:with-param name="Model" select="$Model"/>
					<xsl:with-param name="AllProperties" select="$AllProperties"/>
					<xsl:with-param name="AllRoleSequenceUniquenessConstraints" select="$AllRoleSequenceUniquenessConstraints"/>
				</xsl:call-template>
				<xsl:for-each select="$ConceptTypes">
					<xsl:apply-templates select="." mode="GenerateModelContextInterfaceObjectMethods">
						<xsl:with-param name="Model" select="$Model"/>
						<xsl:with-param name="Properties" select="$AllProperties[@conceptTypeName=current()/@name]/prop:Property"/>
					</xsl:apply-templates>
				</xsl:for-each>
			</plx:interface>

			<xsl:apply-templates select="$Model" mode="OIALtoPLiX_Implementation">
				<xsl:with-param name="Model" select="$Model"/>
				<xsl:with-param name="ModelContextName" select="$ModelContextName"/>
				<xsl:with-param name="ConceptTypes" select="$ConceptTypes"/>
				<xsl:with-param name="InformationTypeFormatMappings" select="$InformationTypeFormatMappings"/>
				<xsl:with-param name="AllProperties" select="$AllProperties"/>
				<xsl:with-param name="AllRoleSequenceUniquenessConstraints" select="$AllRoleSequenceUniquenessConstraints"/>
			</xsl:apply-templates>
			
		</plx:namespace>
	</xsl:template>

	<xsl:template match="oil:conceptType" mode="GenerateProperties">
		<xsl:param name="ConceptTypeRefs"/>
		<xsl:param name="InformationTypeFormatMappings"/>
		<xsl:variable name="thisClassName" select="@name"/>
		<xsl:variable name="identityFormatRefNames" select="$InformationTypeFormatMappings[@isIdentity='true']/@name"/>

		<!--Process directly contained oil:conceptTypeRef and oil:informationType elements,
			as well as nested oil:conceptType elements and oil:conceptType elements that we are nested within.
			Also process all oil:conceptTypeRef elements that are targetted at us.-->

		<xsl:for-each select="oil:informationType[not(@formatRef=$identityFormatRefNames)]">
			<xsl:variable name="informationTypeFormatMapping" select="$InformationTypeFormatMappings[@name=current()/@formatRef]"/>
			<prop:Property name="{@name}" mandatory="{@mandatory}" isUnique="{boolean(oil:singleRoleUniquenessConstraint)}" canBeNull="{not(@mandatory='alethic') or $informationTypeFormatMapping/@canBeNull='true'}" isCollection="false" isCustomType="false">
				<xsl:choose>
					<xsl:when test="not(@mandatory='alethic') and $informationTypeFormatMapping/@canBeNull='false'">
						<prop:DataType dataTypeName="Nullable">
							<plx:passTypeParam>
								<xsl:copy-of select="$informationTypeFormatMapping/prop:DataType/@*"/>
								<xsl:copy-of select="$informationTypeFormatMapping/prop:DataType/child::*"/>
							</plx:passTypeParam>
						</prop:DataType>
					</xsl:when>
					<xsl:otherwise>
						<xsl:copy-of select="$informationTypeFormatMapping/prop:DataType"/>
					</xsl:otherwise>
				</xsl:choose>
			</prop:Property>
		</xsl:for-each>
		<xsl:for-each select="oil:conceptTypeRef">
			<prop:Property name="{@name}" mandatory="{@mandatory}" isUnique="{boolean(oil:singleRoleUniquenessConstraint)}" isCollection="false" isCustomType="true" canBeNull="true" oppositeName="{@oppositeName}">
				<prop:DataType dataTypeName="{@target}"/>
			</prop:Property>
		</xsl:for-each>
		<xsl:for-each select="oil:conceptType">
			<prop:Property name="{@name}" mandatory="{@mandatory}" isUnique="true" isCollection="false" isCustomType="true" canBeNull="true" oppositeName="{$thisClassName}">
				<prop:DataType dataTypeName="{@name}"/>
			</prop:Property>
		</xsl:for-each>
		<xsl:for-each select="parent::oil:conceptType">
			<prop:Property name="{@name}" mandatory="alethic" isUnique="true" isCollection="false" isCustomType="true" canBeNull="true" oppositeName="{$thisClassName}">
				<prop:DataType dataTypeName="{@name}"/>
			</prop:Property>
		</xsl:for-each>
		<xsl:for-each select="$ConceptTypeRefs[@target=current()/@name]">
			<xsl:variable name="isCollection" select="not(boolean(oil:singleRoleUniquenessConstraint))"/>
			<prop:Property name="{@oppositeName}" mandatory="false" isUnique="true" isCollection="{$isCollection}" isCustomType="true" canBeNull="true" oppositeName="{@name}">
				<xsl:variable name="parentConceptTypeName" select="parent::oil:conceptType/@name"/>
				<xsl:choose>
					<xsl:when test="$isCollection">
						<prop:DataType dataTypeName="ICollection">
							<plx:passTypeParam dataTypeName="{$parentConceptTypeName}"/>
						</prop:DataType>
					</xsl:when>
					<xsl:otherwise>
						<prop:DataType dataTypeName="{$parentConceptTypeName}"/>
					</xsl:otherwise>
				</xsl:choose>
			</prop:Property>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="odt:identity" mode="GenerateInformationTypeFormatMapping">
		<prop:FormatMapping name="{@name}" canBeNull="false" isIdentity="true"/>
	</xsl:template>
	<xsl:template match="odt:boolean" mode="GenerateInformationTypeFormatMapping">
		<prop:FormatMapping name="{@name}" canBeNull="false">
			<prop:DataType dataTypeName=".boolean"/>
			<xsl:if test="string-length(@fixed)">
				<plx:binaryOperator type="identityEquality">
					<plx:left>
						<plx:valueKeyword/>
					</plx:left>
					<plx:right>
						<xsl:element name="plx:{@fixed}Keyword"/>
					</plx:right>
				</plx:binaryOperator>
			</xsl:if>
		</prop:FormatMapping>
	</xsl:template>
	<xsl:template match="odt:decimalNumber" mode="GenerateInformationTypeFormatMapping">
		<prop:FormatMapping name="{@name}" canBeNull="false">
			<xsl:variable name="hasConstraints" select="boolean(child::*)"/>
			<xsl:choose>
				<xsl:when test="@fractionDigits=0">
					<!-- Integral type -->
					<!-- TODO: Process @totalDigits and all child elements -->
					<prop:DataType dataTypeName=".i4"/>
				</xsl:when>
				<xsl:otherwise>
					<!-- Fraction type -->
					<!-- TODO: Process @totalDigits, @fractionDigits, and all child elements -->
					<prop:DataType dataTypeName=".decimal"/>
				</xsl:otherwise>
			</xsl:choose>
		</prop:FormatMapping>
	</xsl:template>
	<xsl:template match="odt:floatingPointNumber" mode="GenerateInformationTypeFormatMapping">
		<prop:FormatMapping name="{@name}" canBeNull="false">
			<xsl:choose>
				<xsl:when test="@precision='single' or @precision&lt;=24">
					<prop:DataType dataTypeName=".r4"/>
				</xsl:when>
				<xsl:when test="@precision='double' or @precision&lt;=53">
					<prop:DataType dataTypeName=".r8"/>
				</xsl:when>
				<xsl:when test="@precision='quad' or @precision&lt;=113">
					<!-- TODO: We need a .NET quad-precision floating point type -->
					<prop:DataType dataTypeName=".r16"/>
				</xsl:when>
				<xsl:otherwise>
					<!-- TODO: We need a .NET arbitrary-precision floating point type -->
					<prop:DataType dataTypeName=".r"/>
				</xsl:otherwise>
			</xsl:choose>
			<!-- TODO: Process all child elements -->
		</prop:FormatMapping>
	</xsl:template>
	<xsl:template match="odt:string" mode="GenerateInformationTypeFormatMapping">
		<prop:FormatMapping name="{@name}" canBeNull="true">
			<prop:DataType dataTypeName=".string"/>
			<!-- TODO: Process all child elements -->
			<xsl:if test="(@minLength and not(@minLength=0)) or @maxLength">
				<xsl:call-template name="GetLengthValidationCode">
					<xsl:with-param name="MinLength" select="@minLength"/>
					<xsl:with-param name="MaxLength" select="@maxLength"/>
				</xsl:call-template>
			</xsl:if>
		</prop:FormatMapping>
	</xsl:template>
	<xsl:template match="odt:binary" mode="GenerateInformationTypeFormatMapping">
		<prop:FormatMapping name="{@name}" canBeNull="true">
			<prop:DataType dataTypeName=".u1" dataTypeIsSimpleArray="true"/>
			<xsl:if test="(@minLength and not(@minLength=0)) or @maxLength">
				<xsl:call-template name="GetLengthValidationCode">
					<xsl:with-param name="MinLength" select="@minLength"/>
					<xsl:with-param name="MaxLength" select="@maxLength"/>
				</xsl:call-template>
			</xsl:if>
		</prop:FormatMapping>
	</xsl:template>
	<xsl:template match="*" mode="GenerateInformationTypeFormatMapping">
		<xsl:variable name="warningMessage" select="concat('WARNING: Data type instance &quot;',@name,'&quot; is of unrecognized data type &quot;',name(),'&quot; from namespace &quot;',namespace-uri(),'&quot;.')"/>
		<xsl:message terminate="no">
			<xsl:value-of select="$warningMessage"/>
		</xsl:message>
		<xsl:comment>
			<xsl:value-of select="$warningMessage"/>
		</xsl:comment>
	</xsl:template>

	<xsl:template name="GetLengthValidationCode">
		<xsl:param name="MinLength"/>
		<xsl:param name="MaxLength"/>
		<xsl:variable name="hasMinLength" select="boolean($MinLength) and not($MinLength=0)"/>
		<xsl:variable name="hasMaxLength" select="boolean($MaxLength)"/>
		<xsl:variable name="minLengthExceedsInt32MaxValue" select="$hasMinLength and $MinLength>$Int32MaxValue"/>
		<xsl:variable name="maxLengthExceedsInt32MaxValue" select="$hasMaxLength and $MaxLength>$Int32MaxValue"/>
		<xsl:variable name="minLengthDataType">
			<xsl:choose>
				<xsl:when test="$minLengthExceedsInt32MaxValue">
					<xsl:value-of select="'i8'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'i4'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="maxLengthDataType">
			<xsl:choose>
				<xsl:when test="$maxLengthExceedsInt32MaxValue">
					<xsl:value-of select="'i8'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'i4'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="plxLeftMinValueFragment">
			<plx:left>
				<plx:callInstance type="property" name="Length">
					<xsl:if test="$minLengthExceedsInt32MaxValue">
						<xsl:attribute name="name">
							<xsl:value-of select="'LongLength'"/>
						</xsl:attribute>
					</xsl:if>
					<plx:callObject>
						<plx:valueKeyword/>
					</plx:callObject>
				</plx:callInstance>
			</plx:left>
		</xsl:variable>
		<xsl:variable name="plxLeftMaxValueFragment">
			<plx:left>
				<plx:callInstance type="property" name="Length">
					<xsl:if test="$maxLengthExceedsInt32MaxValue">
						<xsl:attribute name="name">
							<xsl:value-of select="'LongLength'"/>
						</xsl:attribute>
					</xsl:if>
					<plx:callObject>
						<plx:valueKeyword/>
					</plx:callObject>
				</plx:callInstance>
			</plx:left>
		</xsl:variable>
		<xsl:variable name="plxLeftMinValue" select="exsl:node-set($plxLeftMinValueFragment)/child::*"/>
		<xsl:variable name="plxLeftMaxValue" select="exsl:node-set($plxLeftMaxValueFragment)/child::*"/>
		<xsl:variable name="minLengthTestFragment">
			<plx:binaryOperator type="greaterThanOrEqual">
				<xsl:copy-of select="$plxLeftMinValue"/>
				<plx:right>
					<plx:value type="{$minLengthDataType}" data="{$MinLength}"/>
				</plx:right>
			</plx:binaryOperator>
		</xsl:variable>
		<xsl:variable name="minLengthTest" select="exsl:node-set($minLengthTestFragment)/child::*"/>
		<xsl:variable name="maxLengthTestFragment">
			<plx:binaryOperator type="lessThanOrEqual">
				<xsl:copy-of select="$plxLeftMaxValue"/>
				<plx:right>
					<plx:value type="{$maxLengthDataType}" data="{$MaxLength}"/>
				</plx:right>
			</plx:binaryOperator>
		</xsl:variable>
		<xsl:variable name="maxLengthTest" select="exsl:node-set($maxLengthTestFragment)/child::*"/>
		<xsl:choose>
			<xsl:when test="$hasMinLength and $hasMaxLength">
				<xsl:choose>
					<xsl:when test="$MinLength=$MaxLength">
						<xsl:copy-of select="$minLengthTest"/>
					</xsl:when>
					<xsl:otherwise>
						<plx:binaryOperator type="booleanAnd">
							<plx:left>
								<xsl:copy-of select="$minLengthTest"/>
							</plx:left>
							<plx:right>
								<xsl:copy-of select="$maxLengthTest"/>
							</plx:right>
						</plx:binaryOperator>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:when test="$hasMinLength  and not($hasMaxLength)">
				<xsl:copy-of select="$minLengthTest"/>
			</xsl:when>
			<xsl:when test="not($hasMinLength) and $hasMaxLength">
				<xsl:copy-of select="$maxLengthTest"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:message terminate="yes">
					<xsl:text>SANITY CHECK: This template shouldn't be called if neither a non-zero @minLength nor @maxLength is present.</xsl:text>
				</xsl:message>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="oil:conceptType" mode="GenerateAbstractClass">
		<xsl:param name="Model"/>
		<xsl:param name="ModelContextName"/>
		<xsl:param name="InformationTypeFormatMappings"/>
		<xsl:param name="AllProperties"/>
		<xsl:param name="Properties"/>
		<xsl:variable name="className" select="@name"/>
		<xsl:variable name="eventProperties" select="$Properties[not(@isCollection='true')]"/>
		<plx:class visibility="public" modifier="abstract" partial="true" name="{$className}">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$className}"/>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{$className}"/>
			</plx:trailingInfo>
			<xsl:if test="$GenerateObjectDataSourceSupport">
				<plx:attribute dataTypeName="DataObjectAttribute"/>
			</xsl:if>
			<xsl:copy-of select="$GeneratedCodeAttribute"/>
			<xsl:copy-of select="$StructLayoutAttribute"/>
			<plx:implementsInterface dataTypeName="INotifyPropertyChanged"/>
			<plx:implementsInterface dataTypeName="IHas{$ModelContextName}"/>
			<plx:function visibility="protected" name=".construct"/>
			<!-- PLIX_TODO: Once the PLiX formatters support keyword filtering, remove the dataTypeQualifier attribute from the next line. -->
			<plx:field visibility="private" name="{$PrivateMemberPrefix}events" dataTypeIsSimpleArray="true" dataTypeName="Delegate" dataTypeQualifier="System"/>
			<plx:property visibility="private" name="Events">
				<!-- PLIX_TODO: Once the PLiX formatters support keyword filtering, remove the dataTypeQualifier attribute from the next line. -->
				<plx:returns dataTypeIsSimpleArray="true" dataTypeName="Delegate" dataTypeQualifier="System"/>
				<plx:get>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="identityEquality">
								<plx:left>
									<plx:cast type="exceptionCast" dataTypeName=".object">
										<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}events"/>
									</plx:cast>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:assign>
							<plx:left>
								<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}events"/>
							</plx:left>
							<plx:right>
								<!-- PLIX_TODO: Once the PLiX formatters support keyword filtering, remove the dataTypeQualifier attribute from the next line. -->
								<plx:callNew dataTypeIsSimpleArray="true" dataTypeName="Delegate" dataTypeQualifier="System">
									<plx:passParam>
										<plx:value type="i4" data="{count($eventProperties)}"/>
									</plx:passParam>
								</plx:callNew>
							</plx:right>
						</plx:assign>
					</plx:branch>
					<plx:return>
						<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}events"/>
					</plx:return>
				</plx:get>
			</plx:property>
			<xsl:call-template name="GenerateINotifyPropertyChangedImplementation"/>
			<plx:property visibility="public" modifier="abstract" name="Context">
				<plx:interfaceMember memberName="Context" dataTypeName="IHas{$ModelContextName}"/>
				<plx:returns dataTypeName="{$ModelContextName}"/>
				<plx:get/>
			</plx:property>
			<xsl:apply-templates select="$eventProperties" mode="GeneratePropertyChangeEvents">
				<xsl:with-param name="ClassName" select="$className"/>
			</xsl:apply-templates>
			<xsl:apply-templates select="$Properties" mode="GenerateAbstractProperty"/>
			<xsl:call-template name="GenerateToString">
				<xsl:with-param name="ClassName" select="$className"/>
				<xsl:with-param name="Properties" select="$Properties"/>
			</xsl:call-template>
			<xsl:for-each select="parent::oil:conceptType">
				<xsl:variable name="parentConceptTypeName" select="@name"/>
				<!-- Generate an implicit cast operator for the oil:conceptType elements that contain this oil:conceptType -->
				<xsl:call-template name="GenerateImplicitConversionOperators">
					<xsl:with-param name="SourceClassName" select="$className"/>
					<xsl:with-param name="ConversionCallObject">
						<plx:nameRef type="parameter" name="{$className}"/>
					</xsl:with-param>
				</xsl:call-template>
				<!-- Generate a virtual property for each of the parent's properties (except for the one for this class, because that would be awkward). -->
				<xsl:variable name="excludeNames" select="$className | $Properties/@name"/>
				<xsl:call-template name="GenerateVirtualPropertiesFromParents">
					<xsl:with-param name="ExcludeNames" select="$excludeNames"/>
					<xsl:with-param name="AllProperties" select="$AllProperties"/>
					<xsl:with-param name="ParentProperties" select="$AllProperties[@conceptTypeName=$parentConceptTypeName]/prop:Property[not(@name=$excludeNames)]"/>
					<xsl:with-param name="PropertyCallObject">
						<plx:callInstance type="property" name="{$parentConceptTypeName}">
							<plx:callObject>
								<plx:thisKeyword/>
							</plx:callObject>
						</plx:callInstance>
					</xsl:with-param>
				</xsl:call-template>
			</xsl:for-each>
			<xsl:for-each select="child::oil:conceptType">
				<!-- Generate an explicit cast operator for all oil:conceptType elements nested within this oil:conceptType -->
				<xsl:variable name="childConceptTypeName" select="@name"/>
				<plx:operatorFunction type="castNarrow">
					<plx:param dataTypeName="{$className}" name="{$className}"/>
					<plx:returns dataTypeName="{$childConceptTypeName}"/>
					<plx:branch>
						<plx:condition>
							<plx:binaryOperator type="identityEquality">
								<plx:left>
									<plx:nameRef type="parameter" name="{$className}"/>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:return>
							<plx:nullKeyword/>
						</plx:return>
					</plx:branch>
					<plx:alternateBranch>
						<plx:condition>
							<plx:binaryOperator type="identityEquality">
								<plx:left>
									<plx:callInstance type="property" name="{$childConceptTypeName}">
										<plx:callObject>
											<plx:nameRef type="parameter" name="{$className}"/>
										</plx:callObject>
									</plx:callInstance>
								</plx:left>
								<plx:right>
									<plx:nullKeyword/>
								</plx:right>
							</plx:binaryOperator>
						</plx:condition>
						<plx:throw>
							<plx:callNew dataTypeName="InvalidCastException"/>
						</plx:throw>
					</plx:alternateBranch>
					<plx:fallbackBranch>
						<plx:return>
							<plx:callInstance type="property" name="{$childConceptTypeName}">
								<plx:callObject>
									<plx:nameRef type="parameter" name="{$className}"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:return>
					</plx:fallbackBranch>
				</plx:operatorFunction>
				<xsl:for-each select="child::oil:conceptType">
					<xsl:call-template name="GenerateExplicitConversionOperators">
						<xsl:with-param name="SourceClassName" select="$className"/>
						<xsl:with-param name="ConversionCallObject">
							<plx:cast type="exceptionCast" dataTypeName="{$childConceptTypeName}">
								<plx:nameRef type="parameter" name="{$className}"/>
							</plx:cast>
						</xsl:with-param>
					</xsl:call-template>
				</xsl:for-each>
			</xsl:for-each>
		</plx:class>
	</xsl:template>
	<xsl:template name="GenerateImplicitConversionOperators">
		<xsl:param name="SourceClassName"/>
		<xsl:param name="ConversionCallObject"/>
		<xsl:variable name="destinationClassName" select="@name"/>
		<xsl:variable name="conversionCodeFragment">
			<plx:callInstance type="property" name="{$destinationClassName}">
				<plx:callObject>
					<xsl:copy-of select="$ConversionCallObject"/>
				</plx:callObject>
			</plx:callInstance>
		</xsl:variable>
		<xsl:variable name="conversionCode" select="exsl:node-set($conversionCodeFragment)/child::*"/>
		<plx:operatorFunction type="castWiden">
			<plx:param dataTypeName="{$SourceClassName}" name="{$SourceClassName}"/>
			<plx:returns dataTypeName="{$destinationClassName}"/>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="identityEquality">
						<plx:left>
							<plx:nameRef type="parameter" name="{$SourceClassName}"/>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<plx:return>
					<plx:nullKeyword/>
				</plx:return>
			</plx:branch>
			<plx:fallbackBranch>
				<plx:return>
					<xsl:copy-of select="$conversionCode"/>
				</plx:return>
			</plx:fallbackBranch>
		</plx:operatorFunction>
		<xsl:for-each select="parent::oil:conceptType">
			<xsl:call-template name="GenerateImplicitConversionOperators">
				<xsl:with-param name="SourceClassName" select="$SourceClassName"/>
				<xsl:with-param name="ConversionCallObject" select="$conversionCode"/>
			</xsl:call-template>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="GenerateExplicitConversionOperators">
		<xsl:param name="SourceClassName"/>
		<xsl:param name="ConversionCallObject"/>
		<xsl:variable name="destinationClassName" select="@name"/>
		<xsl:variable name="conversionCodeFragment">
			<plx:cast type="exceptionCast" dataTypeName="{$destinationClassName}">
				<xsl:copy-of select="$ConversionCallObject"/>
			</plx:cast>
		</xsl:variable>
		<xsl:variable name="conversionCode" select="exsl:node-set($conversionCodeFragment)/child::*"/>
		<plx:operatorFunction type="castNarrow">
			<plx:param dataTypeName="{$SourceClassName}" name="{$SourceClassName}"/>
			<plx:returns dataTypeName="{$destinationClassName}"/>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="identityEquality">
						<plx:left>
							<plx:nameRef type="parameter" name="{$SourceClassName}"/>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<plx:return>
					<plx:nullKeyword/>
				</plx:return>
			</plx:branch>
			<plx:fallbackBranch>
				<plx:return>
					<xsl:copy-of select="$conversionCode"/>
				</plx:return>
			</plx:fallbackBranch>
		</plx:operatorFunction>
		<xsl:for-each select="child::oil:conceptType">
			<xsl:call-template name="GenerateExplicitConversionOperators">
				<xsl:with-param name="SourceClassName" select="$SourceClassName"/>
				<xsl:with-param name="ConversionCallObject" select="$conversionCode"/>
			</xsl:call-template>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="GenerateVirtualPropertiesFromParents">
		<xsl:param name="ExcludeNames"/>
		<xsl:param name="AllProperties"/>
		<xsl:param name="ParentProperties"/>
		<xsl:param name="PropertyCallObject"/>
		<xsl:variable name="thisParentConceptTypeName" select="@name"/>
		<xsl:for-each select="$ParentProperties">
			<plx:property visibility="public" modifier="virtual" name="{@name}">
				<xsl:call-template name="GenerateCLSCompliantAttributeIfNecessary"/>
				<plx:returns>
					<xsl:copy-of select="prop:DataType/@*"/>
					<xsl:copy-of select="prop:DataType/child::*"/>
				</plx:returns>
				<plx:get>
					<plx:return>
						<plx:callInstance type="property" name="{@name}">
							<plx:callObject>
								<xsl:copy-of select="$PropertyCallObject"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:return>
				</plx:get>
				<xsl:if test="not(@isCollection='true')">
					<plx:set>
						<plx:assign>
							<plx:left>
								<plx:callInstance type="property" name="{@name}">
									<plx:callObject>
										<xsl:copy-of select="$PropertyCallObject"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:valueKeyword/>
							</plx:right>
						</plx:assign>
					</plx:set>
				</xsl:if>
			</plx:property>
			<xsl:if test="not(@isCollection='true')">
				<plx:event visibility="public" name="{@name}Changing">
					<xsl:call-template name="GenerateCLSCompliantAttributeIfNecessary"/>
					<plx:explicitDelegateType dataTypeName="EventHandler"/>
					<plx:passTypeParam  dataTypeName="PropertyChangingEventArgs">
						<plx:passTypeParam dataTypeName="{$thisParentConceptTypeName}"/>
						<plx:passTypeParam>
							<xsl:copy-of select="prop:DataType/@*"/>
							<xsl:copy-of select="prop:DataType/child::*"/>
						</plx:passTypeParam>
					</plx:passTypeParam>
					<plx:onAdd>
						<plx:attachEvent>
							<plx:left>
								<plx:callInstance type="event" name="{@name}Changing">
									<plx:callObject>
										<xsl:copy-of select="$PropertyCallObject"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:valueKeyword/>
							</plx:right>
						</plx:attachEvent>
					</plx:onAdd>
					<plx:onRemove>
						<plx:detachEvent>
							<plx:left>
								<plx:callInstance type="event" name="{@name}Changing">
									<plx:callObject>
										<xsl:copy-of select="$PropertyCallObject"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:valueKeyword/>
							</plx:right>
						</plx:detachEvent>
					</plx:onRemove>
				</plx:event>
				<plx:event visibility="public" name="{@name}Changed">
					<xsl:call-template name="GenerateCLSCompliantAttributeIfNecessary"/>
					<plx:explicitDelegateType dataTypeName="EventHandler"/>
					<plx:passTypeParam  dataTypeName="PropertyChangedEventArgs">
						<plx:passTypeParam dataTypeName="{$thisParentConceptTypeName}"/>
						<plx:passTypeParam>
							<xsl:copy-of select="prop:DataType/@*"/>
							<xsl:copy-of select="prop:DataType/child::*"/>
						</plx:passTypeParam>
					</plx:passTypeParam>
					<plx:onAdd>
						<plx:attachEvent>
							<plx:left>
								<plx:callInstance type="event" name="{@name}Changed">
									<plx:callObject>
										<xsl:copy-of select="$PropertyCallObject"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:valueKeyword/>
							</plx:right>
						</plx:attachEvent>
					</plx:onAdd>
					<plx:onRemove>
						<plx:detachEvent>
							<plx:left>
								<plx:callInstance type="event" name="{@name}Changed">
									<plx:callObject>
										<xsl:copy-of select="$PropertyCallObject"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:left>
							<plx:right>
								<plx:valueKeyword/>
							</plx:right>
						</plx:detachEvent>
					</plx:onRemove>
				</plx:event>
			</xsl:if>
		</xsl:for-each>
		<xsl:for-each select="parent::oil:conceptType">
			<xsl:variable name="parentParentConceptTypeName" select="@name"/>
			<xsl:variable name="excludeNames" select="$ExcludeNames | $parentParentConceptTypeName | $ParentProperties/@name"/>
			<xsl:call-template name="GenerateVirtualPropertiesFromParents">
				<xsl:with-param name="ExcludeNames" select="$excludeNames"/>
				<xsl:with-param name="AllProperties" select="$AllProperties"/>
				<xsl:with-param name="ParentProperties" select="$AllProperties[@conceptTypeName=$parentParentConceptTypeName]/prop:Property[not(@name=$excludeNames)]"/>
				<xsl:with-param name="PropertyCallObject">
					<plx:callInstance type="property" name="{$parentParentConceptTypeName}">
						<plx:callObject>
							<xsl:copy-of select="$PropertyCallObject"/>
						</plx:callObject>
					</plx:callInstance>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:for-each>
	</xsl:template>
	
	<xsl:template match="prop:Property" mode="GenerateAbstractProperty">
		<plx:property visibility="public" modifier="abstract" name="{@name}" >
			<xsl:call-template name="GenerateCLSCompliantAttributeIfNecessary"/>
			<xsl:if test="$GenerateObjectDataSourceSupport">
				<!--
					TODO: Should we even be generating this for properties where @isCollection='true'?
					If so, do we still handle the 'isNullable' parameter the same way?
				-->
				<plx:attribute dataTypeName="DataObjectFieldAttribute">
					<plx:passParam>
						<plx:falseKeyword/>
					</plx:passParam>
					<plx:passParam>
						<plx:falseKeyword/>
					</plx:passParam>
					<plx:passParam>
						<xsl:choose>
							<xsl:when test="@mandatory='alethic'">
								<plx:falseKeyword/>
							</xsl:when>
							<xsl:otherwise>
								<plx:trueKeyword/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:passParam>
				</plx:attribute>
			</xsl:if>
			<plx:returns>
				<xsl:copy-of select="prop:DataType/@*"/>
				<xsl:copy-of select="prop:DataType/child::*"/>
			</plx:returns>
			<plx:get/>
			<xsl:if test="not(@isCollection='true')">
				<plx:set/>
			</xsl:if>
		</plx:property>
	</xsl:template>

	<xsl:template match="prop:Property" mode="GeneratePropertyChangeEvents">
		<xsl:param name="ClassName"/>
		<xsl:variable name="EventIndex" select="position()-1"/>
		<xsl:call-template name="GeneratePropertyChangeEvent">
			<xsl:with-param name="ClassName" select="$ClassName"/>
			<xsl:with-param name="ChangeType" select="'Changing'"/>
			<xsl:with-param name="EventIndex" select="$EventIndex"/>
		</xsl:call-template>
		<xsl:call-template name="GeneratePropertyChangeEventRaiseMethod">
			<xsl:with-param name="ClassName" select="$ClassName"/>
			<xsl:with-param name="ChangeType" select="'Changing'"/>
			<xsl:with-param name="EventIndex" select="$EventIndex"/>
		</xsl:call-template>
		<xsl:call-template name="GeneratePropertyChangeEvent">
			<xsl:with-param name="ClassName" select="$ClassName"/>
			<xsl:with-param name="ChangeType" select="'Changed'"/>
			<xsl:with-param name="EventIndex" select="$EventIndex"/>
		</xsl:call-template>
		<xsl:call-template name="GeneratePropertyChangeEventRaiseMethod">
			<xsl:with-param name="ClassName" select="$ClassName"/>
			<xsl:with-param name="ChangeType" select="'Changed'"/>
			<xsl:with-param name="EventIndex" select="$EventIndex"/>
		</xsl:call-template>
	</xsl:template>
	<xsl:template name="GeneratePropertyChangeEvent">
		<xsl:param name="ClassName"/>
		<xsl:param name="ChangeType"/>
		<xsl:param name="EventIndex"/>
		<plx:event visibility="public" name="{@name}{$ChangeType}">
			<xsl:call-template name="GenerateCLSCompliantAttributeIfNecessary"/>
			<plx:explicitDelegateType dataTypeName="EventHandler"/>
			<plx:passTypeParam  dataTypeName="Property{$ChangeType}EventArgs">
				<plx:passTypeParam dataTypeName="{$ClassName}"/>
				<plx:passTypeParam>
					<xsl:copy-of select="prop:DataType/@*"/>
					<xsl:copy-of select="prop:DataType/child::*"/>
				</plx:passTypeParam>
			</plx:passTypeParam>
			<plx:onAdd>
				<xsl:call-template name="GetPropertyChangeEventOnAddRemoveCode">
					<xsl:with-param name="EventIndex" select="$EventIndex"/>
					<xsl:with-param name="MethodName" select="'Combine'"/>
				</xsl:call-template>
			</plx:onAdd>
			<plx:onRemove>
				<xsl:call-template name="GetPropertyChangeEventOnAddRemoveCode">
					<xsl:with-param name="EventIndex" select="$EventIndex"/>
					<xsl:with-param name="MethodName" select="'Remove'"/>
				</xsl:call-template>
			</plx:onRemove>
		</plx:event>
	</xsl:template>
	<xsl:template name="GetPropertyChangeEventOnAddRemoveCode">
		<xsl:param name="EventIndex"/>
		<xsl:param name="MethodName"/>
		<plx:assign>
			<plx:left>
				<plx:callInstance type="arrayIndexer" name=".implied">
					<plx:callObject>
						<plx:callThis accessor="this" type="property" name="Events"/>
					</plx:callObject>
					<plx:passParam>
						<plx:value type="i4" data="{$EventIndex}"/>
					</plx:passParam>
				</plx:callInstance>
			</plx:left>
			<plx:right>
				<!-- PLIX_TODO: Once the PLiX formatters support keyword filtering, remove the dataTypeQualifier attribute from the next line. -->
				<plx:callStatic type="methodCall" name="{$MethodName}" dataTypeName="Delegate" dataTypeQualifier="System">
					<plx:passParam>
						<plx:callInstance type="arrayIndexer" name=".implied">
							<plx:callObject>
								<plx:callThis accessor="this" type="property" name="Events"/>
							</plx:callObject>
							<plx:passParam>
								<plx:value type="i4" data="{$EventIndex}"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:passParam>
					<plx:passParam>
						<plx:valueKeyword/>
					</plx:passParam>
				</plx:callStatic>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template name="GeneratePropertyChangeEventRaiseMethod">
		<xsl:param name="ClassName"/>
		<xsl:param name="ChangeType"/>
		<xsl:param name="EventIndex"/>
		<xsl:variable name="isChanging" select="$ChangeType='Changing'"/>
		<xsl:variable name="isChanged" select="$ChangeType='Changed'"/>
		<plx:function visibility="protected" name="Raise{@name}{$ChangeType}Event">
			<xsl:call-template name="GenerateCLSCompliantAttributeIfNecessary"/>
			<xsl:call-template name="GenerateSuppressMessageAttribute">
				<xsl:with-param name="category" select="'Microsoft.Design'"/>
				<xsl:with-param name="checkId" select="'CA1030:UseEventsWhereAppropriate'"/>
			</xsl:call-template>
			<xsl:choose>
				<xsl:when test="$isChanging">
					<plx:param name="newValue">
						<xsl:copy-of select="prop:DataType/@*"/>
						<xsl:copy-of select="prop:DataType/child::*"/>
					</plx:param>
					<plx:returns dataTypeName=".boolean"/>
				</xsl:when>
				<xsl:when test="$isChanged">
					<plx:param name="oldValue">
						<xsl:copy-of select="prop:DataType/@*"/>
						<xsl:copy-of select="prop:DataType/child::*"/>
					</plx:param>
				</xsl:when>
			</xsl:choose>
			<plx:local name="eventHandler" dataTypeName="EventHandler">
				<plx:passTypeParam dataTypeName="Property{$ChangeType}EventArgs">
					<plx:passTypeParam dataTypeName="{$ClassName}"/>
					<plx:passTypeParam>
						<xsl:copy-of select="prop:DataType/@*"/>
						<xsl:copy-of select="prop:DataType/child::*"/>
					</plx:passTypeParam>
				</plx:passTypeParam>
				<plx:initialize>
					<plx:cast type="testCast" dataTypeName="EventHandler">
						<plx:passTypeParam dataTypeName="Property{$ChangeType}EventArgs">
							<plx:passTypeParam dataTypeName="{$ClassName}"/>
							<plx:passTypeParam>
								<xsl:copy-of select="prop:DataType/@*"/>
								<xsl:copy-of select="prop:DataType/child::*"/>
							</plx:passTypeParam>
						</plx:passTypeParam>
						<plx:callInstance type="arrayIndexer" name=".implied">
							<plx:callObject>
								<plx:callThis accessor="this" type="property" name="Events"/>
							</plx:callObject>
							<plx:passParam>
								<plx:value type="i4" data="{$EventIndex}"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:cast>
				</plx:initialize>
			</plx:local>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:cast type="exceptionCast" dataTypeName=".object">
								<plx:nameRef type="local" name="eventHandler"/>
							</plx:cast>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<xsl:variable name="CreateNewEventArgs">
					<xsl:variable name="CurrentValue">
						<plx:callThis accessor="this" type="property"  name="{@name}"/>
					</xsl:variable>
					<plx:callNew dataTypeName="Property{$ChangeType}EventArgs">
						<plx:passTypeParam dataTypeName="{$ClassName}"/>
						<plx:passTypeParam>
							<xsl:copy-of select="prop:DataType/@*"/>
							<xsl:copy-of select="prop:DataType/child::*"/>
						</plx:passTypeParam>
						<plx:passParam>
							<plx:thisKeyword/>
						</plx:passParam>
						<plx:passParam>
							<xsl:choose>
								<xsl:when test="$isChanging">
									<xsl:copy-of select="$CurrentValue"/>
								</xsl:when>
								<xsl:when test="$isChanged">
									<plx:nameRef type="parameter" name="oldValue"/>
								</xsl:when>
							</xsl:choose>
						</plx:passParam>
						<plx:passParam>
							<xsl:choose>
								<xsl:when test="$isChanging">
									<plx:nameRef type="parameter" name="newValue"/>
								</xsl:when>
								<xsl:when test="$isChanged">
									<xsl:copy-of select="$CurrentValue"/>
								</xsl:when>
							</xsl:choose>
						</plx:passParam>
					</plx:callNew>
				</xsl:variable>
				<xsl:if test="$isChanging">
					<plx:local name="eventArgs" dataTypeName="PropertyChangingEventArgs">
						<plx:passTypeParam dataTypeName="{$ClassName}"/>
						<plx:passTypeParam>
							<xsl:copy-of select="prop:DataType/@*"/>
							<xsl:copy-of select="prop:DataType/child::*"/>
						</plx:passTypeParam>
						<plx:initialize>
							<xsl:copy-of select="$CreateNewEventArgs"/>
						</plx:initialize>
					</plx:local>
				</xsl:if>
				<xsl:variable name="commonCallCode">
					<plx:callObject>
						<plx:nameRef type="local" name="eventHandler"/>
					</plx:callObject>
					<plx:passParam>
						<plx:thisKeyword/>
					</plx:passParam>
					<plx:passParam>
						<xsl:choose>
							<xsl:when test="$isChanging">
								<plx:nameRef type="local" name="eventArgs"/>
							</xsl:when>
							<xsl:when test="$isChanged">
								<xsl:copy-of select="$CreateNewEventArgs"/>
							</xsl:when>
						</xsl:choose>
					</plx:passParam>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="$isChanging or not($RaiseEventsAsynchronously)">
						<plx:callInstance name=".implied" type="delegateCall">
							<xsl:copy-of select="$commonCallCode"/>
						</plx:callInstance>
					</xsl:when>
					<xsl:when test="$isChanged and $RaiseEventsAsynchronously">
						<plx:callInstance type="methodCall" name="BeginInvoke">
							<xsl:copy-of select="$commonCallCode"/>
							<plx:passParam>
								<plx:callNew type="newDelegate" dataTypeName="AsyncCallback">
									<plx:passParam>
										<plx:callInstance type="methodReference" name="EndInvoke">
											<plx:callObject>
												<plx:nameRef type="local" name="eventHandler"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:passParam>
								</plx:callNew>
							</plx:passParam>
							<plx:passParam>
								<plx:nullKeyword/>
							</plx:passParam>
						</plx:callInstance>
					</xsl:when>
				</xsl:choose>
				<xsl:choose>
					<xsl:when test="$isChanging">
						<plx:return>
							<plx:unaryOperator type="booleanNot">
								<plx:callInstance name="Cancel" type="property">
									<plx:callObject>
										<plx:nameRef name="eventArgs"/>
									</plx:callObject>
								</plx:callInstance>
							</plx:unaryOperator>
						</plx:return>
					</xsl:when>
					<xsl:when test="$isChanged">
						<plx:callThis name="RaisePropertyChangedEvent">
							<plx:passParam>
								<plx:string>
									<xsl:value-of select="@name"/>
								</plx:string>
							</plx:passParam>
						</plx:callThis>
					</xsl:when>
				</xsl:choose>
			</plx:branch>
			<xsl:if test="$isChanging">
				<plx:return>
					<plx:trueKeyword/>
				</plx:return>
			</xsl:if>
		</plx:function>
	</xsl:template>
	<xsl:template name="GenerateINotifyPropertyChangedImplementation">
		<plx:field visibility="private" name="{$PrivateMemberPrefix}propertyChangedEventHandler" dataTypeName="PropertyChangedEventHandler"/>
		<plx:event visibility="privateInterfaceMember" name="PropertyChanged">
			<xsl:call-template name="GenerateSuppressMessageAttribute">
				<xsl:with-param name="category" select="'Microsoft.Design'"/>
				<xsl:with-param name="checkId" select="'CA1033:InterfaceMethodsShouldBeCallableByChildTypes'"/>
			</xsl:call-template>
			<plx:interfaceMember memberName="PropertyChanged" dataTypeName="INotifyPropertyChanged"/>
			<plx:explicitDelegateType dataTypeName="PropertyChangedEventHandler"/>
			<plx:onAdd>
				<plx:assign>
					<plx:left>
						<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangedEventHandler"/>
					</plx:left>
					<plx:right>
						<plx:cast type="testCast" dataTypeName="PropertyChangedEventHandler">
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
			</plx:onAdd>
			<plx:onRemove>
				<plx:assign>
					<plx:left>
						<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangedEventHandler"/>
					</plx:left>
					<plx:right>
						<plx:cast type="testCast" dataTypeName="PropertyChangedEventHandler">
							<!-- PLIX_TODO: Once the PLiX formatters support keyword filtering, remove the dataTypeQualifier attribute from the next line. -->
							<plx:callStatic type="methodCall" name="Remove" dataTypeName="Delegate" dataTypeQualifier="System">
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
			</plx:onRemove>
		</plx:event>
		<plx:function visibility="private" name="RaisePropertyChangedEvent">
			<plx:param name="propertyName" dataTypeName=".string"/>
			<plx:local name="eventHandler" dataTypeName="PropertyChangedEventHandler">
				<plx:initialize>
					<plx:callThis accessor="this" type="field" name="{$PrivateMemberPrefix}propertyChangedEventHandler"/>
				</plx:initialize>
			</plx:local>
			<plx:branch>
				<plx:condition>
					<plx:binaryOperator type="identityInequality">
						<plx:left>
							<plx:cast type="exceptionCast" dataTypeName=".object">
								<plx:nameRef type="local" name="eventHandler"/>
							</plx:cast>
						</plx:left>
						<plx:right>
							<plx:nullKeyword/>
						</plx:right>
					</plx:binaryOperator>
				</plx:condition>
				<xsl:variable name="commonCallCodeFragment">
					<plx:callObject>
						<plx:nameRef type="local" name="eventHandler"/>
					</plx:callObject>
					<plx:passParam>
						<plx:thisKeyword/>
					</plx:passParam>
					<plx:passParam>
						<plx:callNew dataTypeName="PropertyChangedEventArgs">
							<plx:passParam>
								<plx:nameRef type="parameter" name="propertyName"/>
							</plx:passParam>
						</plx:callNew>
					</plx:passParam>
				</xsl:variable>
				<xsl:variable name="commonCallCode" select="exsl:node-set($commonCallCodeFragment)/child::*"/>
				<xsl:choose>
					<xsl:when test="$RaiseEventsAsynchronously">
						<plx:callInstance type="methodCall" name="BeginInvoke">
							<xsl:copy-of select="$commonCallCode"/>
							<plx:passParam>
								<plx:callNew type="newDelegate" dataTypeName="AsyncCallback">
									<plx:passParam>
										<plx:callInstance type="methodReference" name="EndInvoke">
											<plx:callObject>
												<plx:nameRef type="local" name="eventHandler"/>
											</plx:callObject>
										</plx:callInstance>
									</plx:passParam>
								</plx:callNew>
							</plx:passParam>
							<plx:passParam>
								<plx:nullKeyword/>
							</plx:passParam>
						</plx:callInstance>
					</xsl:when>
					<xsl:otherwise>
						<plx:callInstance type="delegateCall" name=".implied">
							<xsl:copy-of select="$commonCallCode"/>
						</plx:callInstance>
					</xsl:otherwise>
				</xsl:choose>
			</plx:branch>
		</plx:function>
	</xsl:template>

	<xsl:template name="GenerateToString">
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
	
	<xsl:template name="GenerateModelContextInterfaceLookupAndExternalConstraintEnforcementMembers">
		<xsl:param name="Model"/>
		<xsl:param name="AllProperties"/>
		<xsl:param name="AllRoleSequenceUniquenessConstraints"/>
		<!-- TODO: This will break for oil:roleSequenceUniquenessConstraint elements that contain oil:typeRef elements with more than one oil:conceptType reference by @targetConceptType. -->
		<xsl:for-each select="$AllRoleSequenceUniquenessConstraints">
			<xsl:variable name="uniqueConceptTypeName" select="parent::oil:conceptType/@name"/>
			<xsl:variable name="paramsFragment">
				<xsl:for-each select="oil:roleSequence/oil:typeRef">
					<plx:param name="{@targetChild}">
						<xsl:variable name="targetProperty" select="$AllProperties[@conceptTypeName=$uniqueConceptTypeName]/prop:Property[@name=current()/@targetChild]"/>
						<xsl:choose>
							<xsl:when test="$targetProperty/@isCustomType='false' and $targetProperty/@canBeNull='true' and $targetProperty/prop:DataType/@dataTypeName='Nullable'">
								<xsl:copy-of select="$targetProperty/prop:DataType/plx:passTypeParam/@*"/>
								<xsl:copy-of select="$targetProperty/prop:DataType/plx:passTypeParam/child::*"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:copy-of select="$targetProperty/prop:DataType/@*"/>
								<xsl:copy-of select="$targetProperty/prop:DataType/child::*"/>
							</xsl:otherwise>
						</xsl:choose>
					</plx:param>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="params" select="exsl:node-set($paramsFragment)/child::*"/>
			<plx:function visibility="public" modifier="abstract" name="Get{$uniqueConceptTypeName}By{@name}">
				<xsl:copy-of select="$params"/>
				<plx:returns dataTypeName="{$uniqueConceptTypeName}"/>
			</plx:function>
			<plx:function visibility="public" modifier="abstract" name="TryGet{$uniqueConceptTypeName}By{@name}">
				<xsl:copy-of select="$params"/>
				<plx:param type="out" name="{$uniqueConceptTypeName}" dataTypeName="{$uniqueConceptTypeName}"/>
				<plx:returns dataTypeName=".boolean"/>
			</plx:function>
		</xsl:for-each>
		<xsl:for-each select="$AllProperties/prop:Property[@isUnique='true' and not(@isCustomType='true')]">
			<xsl:variable name="uniqueConceptTypeName" select="parent::prop:Properties/@conceptTypeName"/>
			<xsl:variable name="paramFragment">
				<plx:param name="{@name}">
					<xsl:choose>
						<xsl:when test="@isCustomType='false' and @canBeNull='true' and prop:DataType/@dataTypeName='Nullable'">
							<xsl:copy-of select="prop:DataType/plx:passTypeParam/@*"/>
							<xsl:copy-of select="prop:DataType/plx:passTypeParam/child::*"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:copy-of select="prop:DataType/@*"/>
							<xsl:copy-of select="prop:DataType/child::*"/>
						</xsl:otherwise>
					</xsl:choose>
				</plx:param>
			</xsl:variable>
			<xsl:variable name="param" select="exsl:node-set($paramFragment)/child::*"/>
			<!-- TODO: In Get{Thing}By{Name}, {Name} should be oil:singleRoleUniquenessConstraint/@name rather than prop:Property/@name. -->
			<plx:function visibility="public" modifier="abstract" name="Get{$uniqueConceptTypeName}By{@name}">
				<xsl:copy-of select="$param"/>
				<plx:returns dataTypeName="{$uniqueConceptTypeName}"/>
			</plx:function>
			<!-- TODO: In TryGet{Thing}By{Name}, {Name} should be oil:singleRoleUniquenessConstraint/@name rather than prop:Property/@name. -->
			<plx:function visibility="public" modifier="abstract" name="TryGet{$uniqueConceptTypeName}By{@name}">
				<xsl:copy-of select="$param"/>
				<plx:param type="out" name="{$uniqueConceptTypeName}" dataTypeName="{$uniqueConceptTypeName}"/>
				<plx:returns dataTypeName=".boolean"/>
			</plx:function>
		</xsl:for-each>
	</xsl:template>
	
	<xsl:template match="oil:conceptType" mode="GenerateModelContextInterfaceObjectMethods">
		<xsl:param name="Model"/>
		<xsl:param name="Properties"/>
		<plx:function visibility="public" modifier="abstract" name="Create{@name}">
			<xsl:for-each select="$Properties[@mandatory='alethic']">
				<plx:param name="{@name}">
					<xsl:copy-of select="prop:DataType/@*"/>
					<xsl:copy-of select="prop:DataType/child::*"/>
				</plx:param>
			</xsl:for-each>
			<plx:returns dataTypeName="{@name}"/>
		</plx:function>
		<plx:property visibility="public" modifier="abstract" name="{@name}Collection">
				<plx:returns dataTypeName="ReadOnlyCollection">
					<plx:passTypeParam dataTypeName="{@name}"/>
				</plx:returns>
				<plx:get/>
			</plx:property>
	</xsl:template>
	
</xsl:stylesheet>
