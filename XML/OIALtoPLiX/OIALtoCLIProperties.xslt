<?xml version="1.0" encoding="UTF-8" ?>
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

	<xsl:include href="OIALtoPLiX_GlobalSupportFunctions.xslt"/>

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>

	<xsl:variable name="ModelName" select="oil:model/@name"/>
	<xsl:variable name="ConceptTypes" select="oil:model//oil:conceptType"/>
	<xsl:variable name="AllRoleSequenceUniquenessConstraints" select="oil:model//oil:roleSequenceUniquenessConstraint"/>
	<xsl:variable name="InformationTypeFormatMappingsFragment">
		<xsl:apply-templates select="oil:model/oil:informationTypeFormats/child::*" mode="GenerateInformationTypeFormatMapping"/>
	</xsl:variable>
	<xsl:variable name="InformationTypeFormatMappings" select="exsl:node-set($InformationTypeFormatMappingsFragment)/child::*"/>

	<xsl:template match="/">
		<xsl:variable name="ConceptTypeRefs" select="oil:model//oil:conceptTypeRef"/>
		<prop:AllProperties>
			<xsl:for-each select="$ConceptTypes">
				<prop:Properties conceptTypeName="{@name}">
					<xsl:apply-templates select="." mode="GenerateProperties">
						<xsl:with-param name="ConceptTypeRefs" select="$ConceptTypeRefs"/>
						<xsl:with-param name="InformationTypeFormatMappings" select="$InformationTypeFormatMappings"/>
					</xsl:apply-templates>
				</prop:Properties>
			</xsl:for-each>
		</prop:AllProperties>
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

	<xsl:template match="oil:conceptType" mode="GenerateProperties">
		<xsl:param name="ConceptTypeRefs"/>
		<xsl:param name="InformationTypeFormatMappings"/>
		<xsl:variable name="thisClassName" select="@name"/>
		<xsl:variable name="identityFormatRefNames" select="$InformationTypeFormatMappings[@isIdentity='true']/@name"/>

		<!--Process directly contained oil:conceptTypeRef and oil:informationType elements,
			as well as nested oil:conceptType elements and oil:conceptType elements that we are nested within.
			Also process all oil:conceptTypeRef elements that are targetted at us.-->

		<!--
		All informationTypes of the current conceptType that does not have a singleRoleUniquenessConstraint
		or have a singleRoleUniquenessConstraint/@isPreferred='false'
		or singleRoleUniquenessConstraint/@isPreferred='true' but has a parent conceptType that is not a child of another conceptType
		[not(child::oil:singleRoleUniquenessConstraint) or (child::oil:singleRoleUniquenessConstraint/@isPreferred='false') or (child::oil:singleRoleUniquenessConstraint/@isPreferred='true' and not(ancestor::oil:conceptType[@name!=parent::oil:conceptType/@name]))]
		-->
		<xsl:for-each select="oil:informationType[not(child::oil:singleRoleUniquenessConstraint/@isPreferred='true' and ancestor::oil:conceptType[@name!=parent::oil:conceptType/@name])]" >
			<xsl:variable name="informationTypeFormatMapping" select="$InformationTypeFormatMappings[@name=current()/@formatRef]"/>
			<xsl:choose>
				<xsl:when test="@formatRef=$identityFormatRefNames">
					<prop:IdentityField name="{@name}"/>
				</xsl:when>
				<xsl:otherwise>
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
				</xsl:otherwise>
			</xsl:choose>
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
		<xsl:for-each select="$ConceptTypeRefs[@target=$thisClassName]">
			<xsl:variable name="isCollection" select="not(boolean(oil:singleRoleUniquenessConstraint))"/>
			<xsl:variable name="propertyName">
				<xsl:choose>
					<xsl:when test="parent::oil:conceptType/@name = $thisClassName">
						<xsl:value-of select="@oppositeName"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="concat(@oppositeName,'Via',@name)"/>
						<xsl:if test="$isCollection">
							<xsl:value-of select="'Collection'"/>
						</xsl:if>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<prop:Property name="{$propertyName}" mandatory="false" isUnique="true" isCollection="{$isCollection}" isCustomType="true" canBeNull="true" oppositeName="{@name}">
				<xsl:variable name="parentConceptTypeName" select="parent::oil:conceptType/@name"/>
				<xsl:choose>
					<xsl:when test="$isCollection">
						<prop:DataType dataTypeName="IEnumerable">
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
</xsl:stylesheet>
