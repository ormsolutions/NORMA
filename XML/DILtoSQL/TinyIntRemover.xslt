<?xml version="1.0" encoding="utf-8"?>
<!--
	Copyright © Neumont University. All rights reserved.

	This software is provided 'as-is', without any express or implied warranty. In no event will the authors be held liable for any damages arising from the use of this software.
	Permission is granted to anyone to use this software for any purpose, including commercial applications, and to alter it and redistribute it freely, subject to the following restrictions:
	1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
	2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
	3. This notice may not be removed or altered from any source distribution.
-->
<!-- Contributors: Kevin M. Owen -->
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:exsl="http://exslt.org/common"
	xmlns:dsf="urn:schemas-orm-net:DIL:DILSupportFunctions"
	xmlns:dil="http://schemas.orm.net/DIL/DIL"
	xmlns:ddt="http://schemas.orm.net/DIL/DILDT"
	xmlns:dep="http://schemas.orm.net/DIL/DILEP"
	xmlns:dms="http://schemas.orm.net/DIL/DILMS"
	xmlns:dml="http://schemas.orm.net/DIL/DMIL"
	xmlns:ddl="http://schemas.orm.net/DIL/DDIL"
	extension-element-prefixes="exsl dsf">

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:strip-space elements="*"/>

	<xsl:param name="TinyIntRemover.ReplacementDataType" select="'SMALLINT'"/>
	<xsl:param name="TinyIntRemover.ReplacementDataTypePrecision" select="''"/>
	<xsl:param name="TinyIntRemover.ReplacementDataTypeScale" select="''"/>
	
	<xsl:variable name="TinyIntRemover.TINYINT_MinValue" select="0"/>
	<xsl:variable name="TinyIntRemover.TINYINT_MaxValue" select="255"/>

	<xsl:variable name="TinyIntRemover.UseDomainWithoutSchema" select="'UseDomainWithoutSchema'"/>
	<xsl:variable name="TinyIntRemover.UseDomainWithSchema" select="'UseDomainWithSchema'"/>

	<xsl:template name="GenerateTinyIntReplacementDataType">
		<ddt:exactNumeric type="{$TinyIntRemover.ReplacementDataType}">
			<xsl:if test="string($TinyIntRemover.ReplacementDataTypePrecision)">
				<xsl:attribute name="precision">
					<xsl:value-of select="$TinyIntRemover.ReplacementDataTypePrecision"/>
				</xsl:attribute>
				<xsl:if test="string($TinyIntRemover.ReplacementDataTypeScale)">
					<xsl:attribute name="scale">
						<xsl:value-of select="$TinyIntRemover.ReplacementDataTypeScale"/>
					</xsl:attribute>
				</xsl:if>
			</xsl:if>
		</ddt:exactNumeric>
	</xsl:template>
	
	<xsl:template name="GenerateTinyIntCheckConstraint">
		<xsl:param name="columnName"/>
		<ddl:checkConstraintDefinition>
			<dep:betweenPredicate type="BETWEEN">
				<!-- If no column name is specified, the VALUE keyword is used instead. -->
				<xsl:choose>
					<xsl:when test="$columnName">
						<dep:columnReference name="{$columnName}"/>
					</xsl:when>
					<xsl:otherwise>
						<dep:valueKeyword/>
					</xsl:otherwise>
				</xsl:choose>
				<ddt:exactNumericLiteral value="{$TinyIntRemover.TINYINT_MinValue}"/>
				<ddt:exactNumericLiteral value="{$TinyIntRemover.TINYINT_MaxValue}"/>
			</dep:betweenPredicate>
		</ddl:checkConstraintDefinition>
	</xsl:template>

	<xsl:template match="*" mode="TinyIntRemover">
		<xsl:param name="useDomain"/>
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:apply-templates mode="TinyIntRemover">
				<xsl:with-param name="useDomain" select="$useDomain"/>
			</xsl:apply-templates>
		</xsl:copy>
	</xsl:template>

	<!-- We create a DOMAIN for TINYINT if the root element is one that we can safely add things to and there is at least one COLUMN or CAST that will use the DOMAIN. -->
	<xsl:template match="dil:root" mode="TinyIntRemover">
		<xsl:param name="useDomainFragment">
			<xsl:if test=".//ddl:columnDefinition[ddt:exactNumeric[@type='TINYINT']] or .//dep:castSpecification[ddt:exactNumeric[@type='TINYINT']]">
				<xsl:choose>
					<xsl:when test="ddl:schemaDefinition">
						<xsl:value-of select="$TinyIntRemover.UseDomainWithSchema"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$TinyIntRemover.UseDomainWithoutSchema"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
		</xsl:param>
		<xsl:param name="useDomain" select="string($useDomainFragment)"/>
		
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:choose>
				<xsl:when test="not($useDomain)">
					<xsl:apply-templates mode="TinyIntRemover">
						<xsl:with-param name="useDomain" select="$useDomain"/>
					</xsl:apply-templates>
				</xsl:when>
				<xsl:when test="$useDomain = $TinyIntRemover.UseDomainWithoutSchema">
					<xsl:variable name="skipFirstChild" select="$useDomain and child::*[1][self::dms:startTransactionStatement]"/>
					<xsl:if test="$skipFirstChild">
						<xsl:apply-templates select="child::*[1]" mode="TinyIntRemover">
							<xsl:with-param name="useDomain" select="$useDomain"/>
						</xsl:apply-templates>
					</xsl:if>
					<ddl:domainDefinition name="{dsf:makeValidIdentifier('TINYINT')}">
						<xsl:call-template name="GenerateTinyIntReplacementDataType"/>
						<ddl:domainConstraintDefinition name="{dsf:makeValidIdentifier('TINYINT_RangeCheck')}">
							<xsl:call-template name="GenerateTinyIntCheckConstraint"/>
						</ddl:domainConstraintDefinition>
					</ddl:domainDefinition>
					<xsl:apply-templates select="child::*[not($skipFirstChild) or (position() > 1)]" mode="TinyIntRemover">
						<xsl:with-param name="useDomain" select="$useDomain"/>
					</xsl:apply-templates>
				</xsl:when>
				<xsl:otherwise>
					<xsl:for-each select="child::*">
						<xsl:choose>
							<xsl:when test="self::ddl:schemaDefinition[*[not(self::ddl:path)]]">
								<!-- The schema definition has schema elements inside it, so insert the TINYINT domain prior to the first one. -->
								<xsl:copy>
									<xsl:copy-of select="@*"/>
									<xsl:apply-templates select="ddl:path" mode="TinyIntRemover">
										<xsl:with-param name="useDomain" select="$useDomain"/>
									</xsl:apply-templates>
									<ddl:domainDefinition name="{dsf:makeValidIdentifier('TINYINT')}">
										<xsl:call-template name="GenerateTinyIntReplacementDataType"/>
										<ddl:domainConstraintDefinition name="{dsf:makeValidIdentifier('TINYINT_RangeCheck')}">
											<xsl:call-template name="GenerateTinyIntCheckConstraint"/>
										</ddl:domainConstraintDefinition>
									</ddl:domainDefinition>
								</xsl:copy>
							</xsl:when>
							<xsl:otherwise>
								<!-- The schema definition does not have schema elements inside it, so insert the TINYINT domain just after it, or just after the SET SCHEMA statement for it. -->
								<xsl:apply-templates select="." mode="TinyIntRemover">
									<xsl:with-param name="useDomain" select="$useDomain"/>
								</xsl:apply-templates>
								<!-- UNDONE: If the ddl:schemaDefinition is immediately followed by a dms:setSchemaStatement *for that schema*, put the ddl:domainDefintion after the dms:setSchemaStatement instead of after the ddl:schemaDefinition. -->
								<!--<xsl:if test="(self::ddl:schemaDefinition and (not(following-sibling::*[1][self::dms:setSchemaStatement]) or not(@schemaName = following-sibling::dms:setSchemaStatement[1]/ddt:characterStringLiteral/@value) or (self::dms:setSchemaStatement and preceding-sibling::*[1][self::ddl:schemaDefinition])">
								<xsl:variable name="schemaDefinition" select="self::ddl:schemaDefinition | self::dms:setSchemaStatement/preceding-sibling::ddl:schemaDefinition[1]"/>
								<xsl:variable name="catalogName" select="string($schemaDefinition/@catalogName)"/>
								<xsl:variable name="schemaName" select="string($schemaDefinition/@schemaName)"/>-->
								<xsl:if test="self::ddl:schemaDefinition">
									<xsl:variable name="catalogName" select="string(@catalogName)"/>
									<xsl:variable name="schemaName" select="string(@schemaName)"/>
									<ddl:domainDefinition schema="{$schemaName}" name="{dsf:makeValidIdentifier('TINYINT')}">
										<xsl:if test="$catalogName">
											<xsl:attribute name="catalog">
												<xsl:value-of select="$catalogName"/>
											</xsl:attribute>
										</xsl:if>
										<xsl:call-template name="GenerateTinyIntReplacementDataType"/>
										<ddl:domainConstraintDefinition name="{dsf:makeValidIdentifier('TINYINT_RangeCheck')}">
											<xsl:call-template name="GenerateTinyIntCheckConstraint"/>
										</ddl:domainConstraintDefinition>
									</ddl:domainDefinition>
								</xsl:if>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:for-each>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="ddt:exactNumeric[@type='TINYINT']" mode="TinyIntRemover">
		<xsl:param name="useDomain"/>
		<xsl:choose>
			<xsl:when test="$useDomain and (parent::dep:castSpecification or parent::ddl:columnDefinition)">
				<!-- Replace the ddt:exactNumeric with a reference to the DOMAIN we created. -->
				<ddt:domain name="{dsf:makeValidIdentifier('TINYINT')}">
					<xsl:if test="$useDomain = $TinyIntRemover.UseDomainWithSchema">
						<!-- Find the first @schema attribute on an ancestor element. -->
						<!-- For ddl:columnDefinition, this will usually be on the ddl:tableDefinition. -->
						<!-- For dep:castSpecification, this could be anything. -->
						<!-- If we can't find one, hopefully a schema in which we created a TINYINT DOMAIN will be the current schema. -->
						<xsl:copy-of select="ancestor::*/@schema[1]"/>
					</xsl:if>
				</ddt:domain>
			</xsl:when>
			<xsl:otherwise>
				<!-- Copy the ddt:exactNumeric, replacing @type. -->
				<xsl:copy>
					<xsl:copy-of select="@*"/>
					<xsl:attribute name="type">
						<xsl:value-of select="$TinyIntRemover.ReplacementDataType"/>
					</xsl:attribute>
					<xsl:apply-templates mode="TinyIntRemover">
						<xsl:with-param name="useDomain" select="$useDomain"/>
					</xsl:apply-templates>
				</xsl:copy>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="ddl:domainDefinition[ddt:exactNumeric[@type='TINYINT']]" mode="TinyIntRemover">
		<xsl:param name="useDomain"/>
		<!-- Copy the ddl:domainDefinition. -->
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<!-- Process the children (the ddt:exactNumeric, optional default clause, and optional domain constraints). -->
			<xsl:apply-templates mode="TinyIntRemover">
				<xsl:with-param name="useDomain" select="$useDomain"/>
			</xsl:apply-templates>
			<!-- Add an additional domain constraint that restricts the value to the range of TINYINT. -->
			<!-- UNDONE: We should analyze any domain constraints already present to see if they are at least this restrictive. If so, we can omit this constraint. -->
			<ddl:domainConstraintDefinition name="{dsf:makeValidIdentifier(concat(dsf:unescapeIdentifier(@name), '_TINYINT_RangeCheck'))}">
				<xsl:call-template name="GenerateTinyIntCheckConstraint"/>
			</ddl:domainConstraintDefinition>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="ddl:columnDefinition[ddt:exactNumeric[@type='TINYINT']]" mode="TinyIntRemover">
		<xsl:param name="useDomain"/>
		<!-- Copy the ddl:columnDefinition. -->
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<!-- Process the children (the ddt:exactNumeric, optional default / identity / generation clause, and optional column constraints). -->
			<xsl:apply-templates mode="TinyIntRemover">
				<xsl:with-param name="useDomain" select="$useDomain"/>
			</xsl:apply-templates>
			<xsl:if test="not($useDomain)">
				<!-- Since we're not deferring to a DOMAIN, we need to add an additional column constraint that restricts the value to the range of TINYINT. -->
				<!-- UNDONE: We should analyze any column and table constraints already present to see if they are at least this restrictive. If so, we can omit this constraint. -->
				<ddl:columnConstraintDefinition>
					<xsl:call-template name="GenerateTinyIntCheckConstraint">
						<xsl:with-param name="columnName" select="@name"/>
					</xsl:call-template>
				</ddl:columnConstraintDefinition>
			</xsl:if>
		</xsl:copy>
	</xsl:template>

	<xsl:template match="ddl:sequenceGeneratorDefinition[ddt:exactNumeric[@type='TINYINT']]" mode="TinyIntRemover">
		<!-- Copy the ddl:sequenceGeneratorDefinition. -->
		<xsl:copy>
			<xsl:copy-of select="@*"/>
			<xsl:apply-templates select="ddt:exactNumeric" mode="TinyIntRemover"/>
			<xsl:apply-templates select="ddl:sequenceGeneratorStartWithOption" mode="TinyIntRemover"/>
			<xsl:apply-templates select="ddl:sequenceGeneratorIncrementByOption" mode="TinyIntRemover"/>
			
			<xsl:apply-templates select="ddl:sequenceGeneratorMaxValueOption" mode="TinyIntRemover"/>
			<!-- Add in the max value from TINYINT if a max value isn't already specified. -->
			<xsl:if test="not(ddl:sequenceGeneratorMaxValueOption)">
				<ddl:sequenceGeneratorMaxValueOption>
					<ddt:exactNumericLiteral value="{$TinyIntRemover.TINYINT_MaxValue}"/>
				</ddl:sequenceGeneratorMaxValueOption>
			</xsl:if>
			
			<xsl:apply-templates select="ddl:sequenceGeneratorMinValueOption" mode="TinyIntRemover"/>
			<!-- Add in the min value from TINYINT if a min value isn't already specified. -->
			<xsl:if test="not(ddl:sequenceGeneratorMinValueOption)">
				<ddl:sequenceGeneratorMinValueOption>
					<ddt:exactNumericLiteral value="{$TinyIntRemover.TINYINT_MinValue}"/>
				</ddl:sequenceGeneratorMinValueOption>
			</xsl:if>
			
			<xsl:apply-templates select="ddl:sequenceGeneratorCycleOption" mode="TinyIntRemover"/>
		</xsl:copy>
	</xsl:template>

	<!-- UNDONE: Handle TINYINT in a ddl:userDefinedTypeDefinition by inserting the correct CHECK constraints (where possible) at each usage of the user-defined type. -->
	
	<!-- UNDONE: Figure out how to handle TINYINT in a ddl:sqlParameterDeclaration. -->
	
	<!-- UNDONE: Figure out how to handle TINYINT in a dep:castSpecification when we don't create a DOMAIN. -->

</xsl:stylesheet>
