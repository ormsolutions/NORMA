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
	xmlns:xs="http://www.w3.org/2001/XMLSchema"
	exclude-result-prefixes="oil odt"
	extension-element-prefixes="exsl">

	<xsl:import href="../OIALtoPLiX_GlobalSupportFunctions.xslt"/>
	<!--<xsl:import href="../../DIL/Transforms/DILSupportFunctions.xslt"/>-->
	<xsl:param name="OIAL" select="/*" />
	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>

	<xsl:variable name="ModelName" select="$OIAL/@name"/>
	<xsl:variable name="InfomationTypeFormats" select="$OIAL//oil:informationTypeFormats/child::*"/>
	<xsl:variable name="ConceptTypes" select="$OIAL//oil:conceptType"/>
	<xsl:variable name="AllProperties" select="prop:AllProperties/prop:Properties" />
	<xsl:variable name="AllRoleSequenceUniquenessConstraints" select="$OIAL//oil:roleSequenceUniquenessConstraint"/>
	<xsl:variable name="InformationTypeFormatMappingsFragment">
		<xsl:apply-templates select="oil:model/oil:informationTypeFormats/child::*" mode="GenerateInformationTypeFormatMapping"/>
	</xsl:variable>
	<xsl:variable name="InformationTypeFormatMappings" select="exsl:node-set($InformationTypeFormatMappingsFragment)/child::*"/>

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

	<xsl:template match="*">
		<!--<xsl:message terminate="yes">-->
		<!--<xsl:text>Unhandled Element: </xsl:text>
		<xsl:value-of select="name()"/>-->
		<!--</xsl:message>-->
	</xsl:template>
	<xsl:template match="*" mode="fields">
		<!--<xsl:message terminate="yes">-->
		<!--<xsl:text>Unhandled Element: </xsl:text>
		<xsl:value-of select="name()"/>-->
		<!--</xsl:message>-->
	</xsl:template>
	<xsl:template match="*" mode="construction">
		<!--<xsl:message terminate="yes">-->
		<!--<xsl:text>Unhandled Element: </xsl:text>
		<xsl:value-of select="name()"/>-->
		<!--</xsl:message>-->
	</xsl:template>
	<xsl:template match="*" mode="validation">
		<!--<xsl:message terminate="yes">-->
		<!--<xsl:text>Unhandled Element: </xsl:text>
		<xsl:value-of select="name()"/>-->
		<!--</xsl:message>-->
	</xsl:template>

	<xsl:template match="odt:string" mode="validation">
		<xsl:variable name="dataType" select="$InformationTypeFormatMappings[@name = current()/@name]/prop:DataType/@dataTypeName"/>
		<xsl:if test="@minLength or @maxLength">
			<plx:callInstance name="addValidationRule">
				<plx:callObject>
					<plx:callThis name="validationRules" type="field" accessor="this"/>
				</plx:callObject>
				<plx:passParam>
					<plx:callNew dataTypeName="StringLenthValidator" dataTypeQualifier="PHPEntities">
						<plx:passParam>
							<plx:string>
								<xsl:value-of select="@name"/>
							</plx:string>
						</plx:passParam>
						<plx:passParam>
							<xsl:choose>
								<xsl:when test="@minLength">
									<plx:value data="{@minLength}" type="{$dataType}"/>
								</xsl:when>
								<xsl:otherwise>
									<plx:nullKeyword/>
								</xsl:otherwise>
							</xsl:choose>
						</plx:passParam>
						<plx:passParam>
							<xsl:choose>
								<xsl:when test="@maxLength">
									<plx:value data="{@maxLength}" type="{$dataType}"/>
								</xsl:when>
								<xsl:otherwise>
									<plx:nullKeyword/>
								</xsl:otherwise>
							</xsl:choose>
						</plx:passParam>
					</plx:callNew>
				</plx:passParam>
			</plx:callInstance>
		</xsl:if>
	</xsl:template>
	<xsl:template match="odt:decimalNumber" mode="validation">
		<xsl:variable name="dataType" select="$InformationTypeFormatMappings[@name = current()/@name]/prop:DataType/@dataTypeName"/>
		<xsl:for-each select="odt:range">
			<plx:callInstance name="addValidationRule">
				<plx:callObject>
					<plx:callThis name="validationRules" type="field" accessor="this"/>
				</plx:callObject>
				<plx:passParam>
					<plx:callNew dataTypeName="ValueRangeValidator" dataTypeQualifier="PHPEntities">
						<plx:passParam>
							<plx:string>
								<xsl:value-of select="../@name"/>
							</plx:string>
						</plx:passParam>
						<plx:passParam>
							<xsl:choose>
								<xsl:when test="odt:lowerBound/@value">
									<plx:value data="{odt:lowerBound/@value}" type="{$dataType}"/>
								</xsl:when>
								<xsl:otherwise>
									<plx:nullKeyword/>
								</xsl:otherwise>
							</xsl:choose>
						</plx:passParam>
						<plx:passParam>
							<plx:callStatic name="{odt:lowerBound/@clusivity}" type="field" dataTypeName="ValueRangeValidatorClusivity" dataTypeQualifier="PHPEntities"/>
						</plx:passParam>
						<plx:passParam>
							<xsl:choose>
								<xsl:when test="odt:upperBound/@value">
									<plx:value data="{odt:upperBound/@value}" type="{$dataType}"/>
								</xsl:when>
								<xsl:otherwise>
									<plx:nullKeyword/>
								</xsl:otherwise>
							</xsl:choose>
						</plx:passParam>
						<plx:passParam>
							<plx:callStatic name="{odt:upperBound/@clusivity}" type="field" dataTypeName="ValueRangeValidatorClusivity" dataTypeQualifier="PHPEntities"/>
						</plx:passParam>
					</plx:callNew>
				</plx:passParam>
			</plx:callInstance>
		</xsl:for-each>
	</xsl:template>
	
	<xsl:template match="oil:conceptType" mode="fields">
		<plx:field visibility="private" name="{@name}" dataTypeName="{@name}" />
	</xsl:template>
	<xsl:template match="oil:conceptTypeRef">
		<xsl:variable name="type" select="concat(@oppositeName,'_',@name,'_',@target,'_Proxy')"/>
		<plx:function name="{concat('set',@name)}" visibility="public">
			<plx:param name="value" dataTypeName="{@target}"/>
			<plx:callInstance name="Set">
				<plx:callObject>
					<plx:callThis name="{$type}" type="field"/>
				</plx:callObject>
				<plx:passParam>
					<plx:nameRef name="value" type="parameter"/>
				</plx:passParam>
			</plx:callInstance>
		</plx:function>
		<plx:function name="{concat('get',@name)}" visibility="public">
			<plx:returns dataTypeName="{@target}"/>
			<plx:return>
				<plx:callInstance name="Get">
					<plx:callObject>
						<plx:callThis name="{$type}" type="field"/>
					</plx:callObject>
				</plx:callInstance>
			</plx:return>
		</plx:function>
	</xsl:template>
	<xsl:template match="oil:conceptTypeRef" mode="fields">
		<xsl:variable name="type" select="concat(@oppositeName,'_',@name,'_',@target,'_Proxy')"/>
		<plx:field visibility="private" name="{$type}" dataTypeName="{$type}"/>
	</xsl:template>
	<xsl:template match="oil:conceptTypeRef" mode="construction">
		<xsl:variable name="type" select="concat(@oppositeName,'_',@name,'_',@target,'_Proxy')"/>
		<plx:assign>
			<plx:left>
				<plx:callThis name="{$type}" type="field"/>
			</plx:left>
			<plx:right>
				<plx:callNew dataTypeName="{$type}">
					<plx:passParam>
						<plx:thisKeyword/>
					</plx:passParam>
				</plx:callNew>
			</plx:right>
		</plx:assign>
	</xsl:template>
	<xsl:template match="oil:informationType">
		<xsl:variable name="dataType" select="$InformationTypeFormatMappings[@name = current()/@formatRef]/prop:DataType/@dataTypeName"/>
		<plx:property visibility="public">
			<xsl:copy-of select="@name"/>
			<plx:returns>
				<xsl:attribute name="dataTypeName">
					<xsl:choose>
						<xsl:when test="$dataType">
							<xsl:value-of select="$dataType"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="string('.object')"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
			</plx:returns>
			<plx:get>
				<plx:return>
					<plx:callThis type="field">
						<xsl:copy-of select="@name"/>
					</plx:callThis>
				</plx:return>
			</plx:get>
			<plx:set>
				<plx:assign>
					<plx:left>
						<plx:callThis type="field">
							<xsl:copy-of select="@name"/>
						</plx:callThis>
					</plx:left>
					<plx:right>
						<plx:nameRef name="value" type="parameter"/>
					</plx:right>
				</plx:assign>
			</plx:set>
		</plx:property>
	</xsl:template>
	<xsl:template match="oil:informationType" mode="fields">
		<xsl:variable name="dataType" select="$InformationTypeFormatMappings[@name = current()/@formatRef]/prop:DataType/@dataTypeName"/>
		<plx:field visibility="private">
			<xsl:copy-of select="@name"/>
			<xsl:attribute name="dataTypeName">
				<xsl:choose>
					<xsl:when test="$dataType">
						<xsl:value-of select="$dataType"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="string('.object')"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</plx:field>
	</xsl:template>
	<xsl:template match="oil:informationType" mode="validation">
		<!--<xsl:variable name="dataType" select="$InformationTypeFormatMappings[@name = current()/@formatRef]/prop:DataType"/>-->
		<plx:callInstance name="addValidationRule">
			<plx:callObject>
				<plx:callThis name="validationRules" type="field" accessor="this"/>
			</plx:callObject>
			<plx:passParam>
				<plx:callNew dataTypeName="RequiredFieldValidator" dataTypeQualifier="PHPEntities">
					<plx:passParam>
						<plx:string>
							<xsl:value-of select="@name"/>
						</plx:string>
					</plx:passParam>
				</plx:callNew>
			</plx:passParam>
		</plx:callInstance>
		<xsl:apply-templates select="$InfomationTypeFormats[@name=current()/@formatRef]" mode="validation"/>
	</xsl:template>
	<xsl:template match="oil:model">
		<plx:root>
			<plx:namespace name="PHPEntities">
				<xsl:call-template name="RenderConceptType">
					<xsl:with-param name="ConceptTypes" select="$ConceptTypes"/>
				</xsl:call-template>
			</plx:namespace>
		</plx:root>
	</xsl:template>
	
	<xsl:template name="RenderConceptType">
		<xsl:param name="ConceptTypes"/>
		<xsl:for-each select="$ConceptTypes">
			<plx:class visibility="public" partial="true" name="{@name}">
				<plx:derivesFromClass dataTypeName="Entity"/>
				<xsl:variable name="parentConceptTypeName" select="parent::oil:conceptType"/>
				<xsl:variable name="name" select="@name"/>
				<!-- fields -->
				<xsl:apply-templates select="*" mode="fields"/>
				<xsl:apply-templates select="$parentConceptTypeName" mode="fields"/>
				<!-- Constructor -->
				<plx:function name=".construct" visibility="public">
					<plx:initialize>
						<plx:callThis name=".implied" accessor="base"/>
					</plx:initialize>
					<xsl:apply-templates select="*" mode="construction"/>
				</plx:function>
				<!-- validation -->
				<plx:function name="addValidationRules" visibility="public" modifier="override">
					<xsl:apply-templates select="*" mode="validation"/>
				</plx:function>
				<!-- Parent and super types -->
				<xsl:for-each select="$parentConceptTypeName">
					<xsl:call-template name="RenderSubTypeProperty">
						<xsl:with-param name="current" select="$name"/>
						<xsl:with-param name="other" select="@name"/>
					</xsl:call-template>
				</xsl:for-each>
				<xsl:for-each select="oil:conceptType">
					<xsl:if test="parent::oil:conceptType">
						<xsl:call-template name="RenderSubTypeProperty">
							<xsl:with-param name="current" select="../@name"/>
							<xsl:with-param name="other" select="@name"/>
						</xsl:call-template>
					</xsl:if>
				</xsl:for-each>
				<!-- Proxies -->
				<xsl:apply-templates select="*"/>
			</plx:class>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="RenderSubTypeProperty">
		<xsl:param name="current"/>
		<xsl:param name="other"/>
		<plx:property visibility="public" name="{$other}">
			<plx:returns dataTypeName="{$other}"/>
			<plx:get>
				<plx:return>
					<plx:callThis type="field" name="{$other}"/>
				</plx:return>
			</plx:get>
			<plx:set>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<plx:callThis name="{$other}" type="field"/>
							</plx:left>
							<plx:right>
								<plx:nameRef name="value" type="parameter"/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:assign>
						<plx:left>
							<plx:callThis name="{$other}" type="field"/>
						</plx:left>
						<plx:right>
							<plx:nameRef name="value" type="parameter"/>
						</plx:right>
					</plx:assign>
					<plx:assign>
						<plx:left>
							<plx:callInstance name="{$current}" type="property">
								<plx:callObject>
									<plx:nameRef name="value" type="parameter"/>
								</plx:callObject>
							</plx:callInstance>
						</plx:left>
						<plx:right>
							<plx:thisKeyword/>
						</plx:right>
					</plx:assign>
				</plx:branch>
			</plx:set>
		</plx:property>
	</xsl:template>
</xsl:stylesheet>
