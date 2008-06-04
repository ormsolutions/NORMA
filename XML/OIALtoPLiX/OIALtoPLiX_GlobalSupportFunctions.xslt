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
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:exsl="http://exslt.org/common"
	xmlns:oil="http://schemas.orm.net/OIAL"
	xmlns:ormdt="http://schemas.orm.net/ORMDataTypes"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:prop="urn:schemas-orm-net:PLiX:CLI:Properties"
	exclude-result-prefixes="oil ormdt"
	extension-element-prefixes="exsl">

	<xsl:import href="OIALtoPLiX_GlobalSupportParameters.xslt"/>



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

	<xsl:param name="GeneratedCodeAttributeFragment">
		<plx:attribute dataTypeName="GeneratedCodeAttribute" dataTypeQualifier="System.CodeDom.Compiler">
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
		<plx:attribute dataTypeName="StructLayoutAttribute" dataTypeQualifier="System.Runtime.InteropServices">
			<plx:passParam>
				<plx:callStatic type="field" name="Auto" dataTypeName="LayoutKind" dataTypeQualifier="System.Runtime.InteropServices"/>
			</plx:passParam>
			<plx:passParam>
				<plx:binaryOperator type="assignNamed">
					<plx:left>
						<plx:nameRef type="namedParameter" name="CharSet"/>
					</plx:left>
					<plx:right>
						<plx:callStatic type="field" name="Auto" dataTypeName="CharSet" dataTypeQualifier="System.Runtime.InteropServices"/>
					</plx:right>
				</plx:binaryOperator>
			</plx:passParam>
		</plx:attribute>
	</xsl:param>
	<xsl:param name="StructLayoutAttribute" select="exsl:node-set($StructLayoutAttributeFragment)/child::*"/>

	<xsl:template name="GenerateSuppressMessageAttribute">
		<xsl:param name="category"/>
		<xsl:param name="checkId"/>
		<xsl:param name="justification"/>
		<xsl:param name="messageId"/>
		<xsl:param name="scope"/>
		<xsl:param name="target"/>
		<xsl:if test="$GenerateCodeAnalysisAttributes">
			<plx:attribute dataTypeName="SuppressMessageAttribute" dataTypeQualifier="System.Diagnostics.CodeAnalysis">
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
</xsl:stylesheet>