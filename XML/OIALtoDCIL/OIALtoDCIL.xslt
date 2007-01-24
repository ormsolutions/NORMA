<?xml version="1.0" encoding="utf-8"?>
<!--
	Copyright © Neumont University. All rights reserved.

	This software is provided 'as-is', without any express or implied warranty. In no event will the authors be held liable for any damages arising from the use of this software.
	Permission is granted to anyone to use this software for any purpose, including commercial applications, and to alter it and redistribute it freely, subject to the following restrictions:
	1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
	2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
	3. This notice may not be removed or altered from any source distribution.
-->
<!-- Contributors: Kevin M. Owen, Corey Kaylor, Clé Diggins -->
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:exsl="http://exslt.org/common"
	xmlns:dsf="urn:schemas-orm-net:DIL:DILSupportFunctions"
	xmlns:odt="http://schemas.orm.net/ORMDataTypes"
	xmlns:oil="http://schemas.orm.net/OIAL"
	xmlns:dcl="http://schemas.orm.net/DIL/DCIL"
	xmlns:dil="http://schemas.orm.net/DIL/DIL"
	xmlns:ddt="http://schemas.orm.net/DIL/DILDT"
	xmlns:dep="http://schemas.orm.net/DIL/DILEP"
	xmlns:dml="http://schemas.orm.net/DIL/DMIL" 
	xmlns:ddl="http://schemas.orm.net/DIL/DDIL" 
	extension-element-prefixes="exsl dsf"
	exclude-result-prefixes="odt oil">

	<xsl:import href="../../DIL/Transforms/DILSupportFunctions.xslt"/>

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:strip-space elements="*"/>

	<xsl:template match="oil:model">
		<xsl:variable name="oilModel" select="."/>
		<dcl:schema name="{dsf:makeValidIdentifier(@name)}">
			<xsl:variable name="dataTypesFragment">
				<xsl:for-each select="oil:informationTypeFormats/child::*">
					<FormatMapping name="{@name}">
						<xsl:apply-templates select="." mode="GenerateDataTypeReference"/>
					</FormatMapping>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="dataTypes" select="exsl:node-set($dataTypesFragment)/child::*"/>

			<xsl:variable name="domainDataTypesFragment">
				<xsl:for-each select="oil:informationTypeFormats/child::*[@name = $dataTypes[dcl:domainDataTypeRef]/@name]">
					<dcl:domainDataType name="{dsf:makeValidIdentifier(@name)}" oilRefName="{@name}">
						<xsl:apply-templates select="." mode="GenerateDomain"/>
					</dcl:domainDataType>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="domainDataTypes" select="exsl:node-set($domainDataTypesFragment)/child::*"/>
			<xsl:copy-of select="$domainDataTypes"/>

			<xsl:for-each select="oil:conceptType">

				<xsl:variable name="tableFragment">
					<dcl:table name="{dsf:makeValidIdentifier(@name)}" oilRefName="{@name}">
						<xsl:apply-templates select="." mode="GenerateTableContent">
							<xsl:with-param name="OilModel" select="$oilModel"/>
							<xsl:with-param name="DataTypes" select="$dataTypes"/>
						</xsl:apply-templates>
					</dcl:table>
				</xsl:variable>

				<xsl:variable name="table" select="exsl:node-set($tableFragment)/child::*"/>
				<xsl:copy-of select="$table"/>

				<!--Generic Insert Procedure - Generate entire table (all column values except identity specified) insert-->
				<xsl:apply-templates select="." mode="GenerateInsertProcedure">
					<xsl:with-param name="Table" select="$table"/>
					<xsl:with-param name="DomainDataTypes" select="$domainDataTypes"/>
				</xsl:apply-templates>
				<!--Generic Delete Procedure - delete all rows matching <primary key values>-->
				<xsl:apply-templates select="." mode="GenerateDeleteProcedure">
					<xsl:with-param name="Table" select="$table"/>
					<xsl:with-param name="DomainDataTypes" select="$domainDataTypes"/>
					<xsl:with-param name="OilModel" select="$oilModel"/>
					<xsl:with-param name="DataTypes" select="$dataTypes"/>
				</xsl:apply-templates>
				<!--Generic Update procedures - update for single row matching <primary key values>-->
				<xsl:apply-templates select="." mode="GenerateUpdateProcedures">
					<xsl:with-param name="Table" select="$table"/>
					<xsl:with-param name="DomainDataTypes" select="$domainDataTypes"/>
					<xsl:with-param name="OilModel" select="$oilModel"/>
					<xsl:with-param name="DataTypes" select="$dataTypes"/>
				</xsl:apply-templates>
				<!--Generic Select procedures - select all rows that match <unique identifier>-->
				<xsl:apply-templates select="." mode="GenerateSelectProcedures">
					<xsl:with-param name="Table" select="$table"/>
					<xsl:with-param name="DomainDataTypes" select="$domainDataTypes"/>
					<xsl:with-param name="OilModel" select="$oilModel"/>
					<xsl:with-param name="DataTypes" select="$dataTypes"/>
				</xsl:apply-templates>
			</xsl:for-each>

		</dcl:schema>
	</xsl:template>

	<xsl:template match="odt:identity" mode="GenerateDataTypeReference">
		<xsl:attribute name="isForIdentity">
			<xsl:value-of select="true()"/>
		</xsl:attribute>
		<xsl:attribute name="oilRefName">
			<xsl:value-of select="@name"/>
		</xsl:attribute>
		<dcl:predefinedDataType name="BIGINT"/>
	</xsl:template>
	<xsl:template match="odt:boolean" mode="GenerateDataTypeReference">
		<xsl:attribute name="oilRefName">
			<xsl:value-of select="@name"/>
		</xsl:attribute>
		<dcl:predefinedDataType name="BOOLEAN"/>
	</xsl:template>
	<xsl:template match="odt:decimalNumber" mode="GenerateDataTypeReference">
		<xsl:attribute name="oilRefName">
			<xsl:value-of select="@name"/>
		</xsl:attribute>
		<xsl:choose>
			<xsl:when test="odt:enumeration or odt:range">
				<dcl:domainDataTypeRef name="{dsf:makeValidIdentifier(@name)}"/>
			</xsl:when>
			<xsl:when test="@fractionDigits = 0 and not(@totalDigits)">
				<dcl:predefinedDataType name="BIGINT"/>
			</xsl:when>
			<xsl:otherwise>
				<dcl:predefinedDataType name="DECIMAL">
					<xsl:if test="@totalDigits">
						<xsl:attribute name="precision">
							<xsl:value-of select="@totalDigits"/>
						</xsl:attribute>
					</xsl:if>
					<xsl:if test="@fractionDigits">
						<xsl:attribute name="scale">
							<xsl:value-of select="@fractionDigits"/>
						</xsl:attribute>
					</xsl:if>
				</dcl:predefinedDataType>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="odt:floatingPointNumber" mode="GenerateDataTypeReference">
		<xsl:attribute name="oilRefName">
			<xsl:value-of select="@name"/>
		</xsl:attribute>
		<xsl:choose>
			<xsl:when test="odt:enumeration or odt:range">
				<dcl:domainDataTypeRef name="{dsf:makeValidIdentifier(@name)}"/>
			</xsl:when>
			<xsl:otherwise>
				<dcl:predefinedDataType name="FLOAT">
					<xsl:attribute name="precision">
						<xsl:call-template name="GetFloatPrecisionAsNumber">
							<xsl:with-param name="Precision" select="@precision"/>
						</xsl:call-template>
					</xsl:attribute>
				</dcl:predefinedDataType>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="odt:string" mode="GenerateDataTypeReference">
		<xsl:attribute name="oilRefName">
			<xsl:value-of select="@name"/>
		</xsl:attribute>
		<xsl:choose>
			<xsl:when test="oil:pattern or odt:enumeration or @minLength">
				<dcl:domainDataTypeRef name="{dsf:makeValidIdentifier(@name)}"/>
			</xsl:when>
			<xsl:otherwise>
				<dcl:predefinedDataType name="CHARACTER VARYING">
					<xsl:if test="@maxLength">
						<xsl:attribute name="length">
							<xsl:value-of select="@maxLength"/>
						</xsl:attribute>
					</xsl:if>
				</dcl:predefinedDataType>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="odt:binary" mode="GenerateDataTypeReference">
		<xsl:attribute name="oilRefName">
			<xsl:value-of select="@name"/>
		</xsl:attribute>
		<dcl:predefinedDataType name="BINARY LARGE OBJECT">
			<xsl:if test="@maxLength">
				<xsl:attribute name="length">
					<xsl:value-of select="@maxLength"/>
				</xsl:attribute>
			</xsl:if>
		</dcl:predefinedDataType>
	</xsl:template>
	<xsl:template match="*" mode="GenerateDataTypeReference">
		<TODO value="Fallback for GenerateDataTypeReference. How this is done will depend on how we chose to do data types for ORM."/>
	</xsl:template>

	<xsl:template match="odt:decimalNumber" mode="GenerateDomain">
		<xsl:attribute name="oilRefName">
			<xsl:value-of select="@name"/>
		</xsl:attribute>
		<dcl:predefinedDataType>
			<xsl:choose>
				<xsl:when test="@fractionDigits=0 and not(@totalDigits)">
					<xsl:attribute name="name">
						<xsl:value-of select="'BIGINT'"/>
					</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="name">
						<xsl:value-of select="'DECIMAL'"/>
					</xsl:attribute>
					<xsl:if test="@totalDigits">
						<xsl:attribute name="precision">
							<xsl:value-of select="@totalDigits"/>
						</xsl:attribute>
					</xsl:if>
					<xsl:if test="@fractionDigits">
						<xsl:attribute name="scale">
							<xsl:value-of select="@fractionDigits"/>
						</xsl:attribute>
					</xsl:if>
				</xsl:otherwise>
			</xsl:choose>
		</dcl:predefinedDataType>
		<dcl:checkConstraint name="{dsf:makeValidIdentifier(concat(@name,'_Chk'))}">
			<xsl:variable name="enumerations">
				<xsl:if test="odt:enumeration">
					<dep:inPredicate type="IN">
						<dep:valueKeyword/>
						<xsl:for-each select="odt:enumeration">
							<ddt:exactNumericLiteral value="{@value}"/>
						</xsl:for-each>
					</dep:inPredicate>
				</xsl:if>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="odt:enumeration and odt:range">
					<dep:or>
						<xsl:call-template name="ProcessOilRangeForNumber">
							<xsl:with-param name="predicandToTest">
								<dep:valueKeyword/>
							</xsl:with-param>
							<xsl:with-param name="exactOrApproximate" select="'exact'"/>
						</xsl:call-template>
						<xsl:copy-of select="$enumerations"/>
					</dep:or>
				</xsl:when>
				<xsl:when test="odt:enumeration">
					<xsl:copy-of select="$enumerations"/>
				</xsl:when>
				<xsl:when test="odt:range">
					<xsl:call-template name="ProcessOilRangeForNumber">
						<xsl:with-param name="predicandToTest">
							<dep:valueKeyword/>
						</xsl:with-param>
						<xsl:with-param name="exactOrApproximate" select="'exact'"/>
					</xsl:call-template>
				</xsl:when>
			</xsl:choose>
		</dcl:checkConstraint>
	</xsl:template>
	<xsl:template match="odt:floatingPointNumber" mode="GenerateDomain">
		<xsl:attribute name="oilRefName">
			<xsl:value-of select="@name"/>
		</xsl:attribute>
		<dcl:predefinedDataType name="FLOAT">
			<xsl:attribute name="precision">
				<xsl:call-template name="GetFloatPrecisionAsNumber">
					<xsl:with-param name="Precision" select="@precision"/>
				</xsl:call-template>
			</xsl:attribute>
		</dcl:predefinedDataType>
		<dcl:checkConstraint name="{dsf:makeValidIdentifier(concat(@name,'_Chk'))}">
			<xsl:variable name="enumerations">
				<xsl:if test="odt:enumeration">
					<dep:inPredicate type="IN">
						<dep:valueKeyword/>
						<xsl:for-each select="odt:enumeration">
							<ddt:approximateNumericLiteral value="{@value}"/>
						</xsl:for-each>
					</dep:inPredicate>
				</xsl:if>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="odt:enumeration and odt:range">
					<dep:or>
						<xsl:call-template name="ProcessOilRangeForNumber">
							<xsl:with-param name="predicandToTest">
								<dep:valueKeyword/>
							</xsl:with-param>
							<xsl:with-param name="exactOrApproximate" select="'approximate'"/>
						</xsl:call-template>
						<xsl:copy-of select="$enumerations"/>
					</dep:or>
				</xsl:when>
				<xsl:when test="odt:enumeration">
					<xsl:copy-of select="$enumerations"/>
				</xsl:when>
				<xsl:when test="odt:range">
					<xsl:call-template name="ProcessOilRangeForNumber">
						<xsl:with-param name="predicandToTest">
							<dep:valueKeyword/>
						</xsl:with-param>
						<xsl:with-param name="exactOrApproximate" select="'approximate'"/>
					</xsl:call-template>
				</xsl:when>
			</xsl:choose>
		</dcl:checkConstraint>
	</xsl:template>
	<xsl:template match="odt:string" mode="GenerateDomain">
		<xsl:attribute name="oilRefName">
			<xsl:value-of select="@name"/>
		</xsl:attribute>
		<dcl:predefinedDataType>
			<xsl:attribute name="name">
				<xsl:choose>
					<xsl:when test="@minLength = @maxLength">
						<xsl:value-of select="'CHARACTER'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'CHARACTER VARYING'"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:if test="@maxLength">
				<xsl:attribute name="length">
					<xsl:value-of select="@maxLength"/>
				</xsl:attribute>
			</xsl:if>
		</dcl:predefinedDataType>
		<dcl:checkConstraint name="{dsf:makeValidIdentifier(concat(@name,'_Chk'))}">
			<xsl:variable name="checkConstraintPartsFragment">
				<xsl:if test="@minLength">
					<dep:comparisonPredicate operator="greaterThanOrEquals">
						<dep:parenthesizedValueExpression>
							<dep:charLengthExpression>
								<dep:trimFunction specification="BOTH">
									<dep:trimSource>
										<dep:valueKeyword/>
									</dep:trimSource>
								</dep:trimFunction>
							</dep:charLengthExpression>
						</dep:parenthesizedValueExpression>
						<ddt:exactNumericLiteral value="{@minLength}"/>
					</dep:comparisonPredicate>
				</xsl:if>
				<xsl:if test="odt:enumeration">
					<dep:inPredicate type="IN">
						<dep:valueKeyword/>
						<xsl:for-each select="odt:enumeration">
							<ddt:characterStringLiteral value="{@value}"/>
						</xsl:for-each>
					</dep:inPredicate>
				</xsl:if>
				<xsl:if test="oil:pattern">
					<TODO value="Figure out how to enforce regular expressions in databases."/>
				</xsl:if>
			</xsl:variable>
			<xsl:variable name="checkConstraintParts" select="exsl:node-set($checkConstraintPartsFragment)/child::*"/>
			<xsl:variable name="countCheckConstraintParts" select="count($checkConstraintParts)"/>
			<xsl:choose>
				<xsl:when test="$countCheckConstraintParts >= 2">
					<dep:and>
						<xsl:copy-of select="$checkConstraintParts[1]"/>
						<xsl:choose>
							<xsl:when test="$countCheckConstraintParts = 3">
								<dep:and>
									<xsl:copy-of select="$checkConstraintParts[2]"/>
									<xsl:copy-of select="$checkConstraintParts[3]"/>
								</dep:and>
							</xsl:when>
							<xsl:otherwise>
								<xsl:copy-of select="$checkConstraintParts[2]"/>
							</xsl:otherwise>
						</xsl:choose>
					</dep:and>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="$checkConstraintParts"/>
				</xsl:otherwise>
			</xsl:choose>
		</dcl:checkConstraint>
	</xsl:template>
	<xsl:template match="*" mode="GenerateDomain">
		<TODO value="Fallback for GenerateDomain. How this is done will depend on how we chose to do data types for ORM."/>
	</xsl:template>



	<xsl:template match="oil:conceptType" mode="GenerateTableContent">
		<xsl:param name="OilModel"/>
		<xsl:param name="DataTypes"/>
		<xsl:param name="AlwaysNullable" select="false()"/>
		<xsl:variable name="prefix">
			<xsl:call-template name="GetPrefix">
				<xsl:with-param name="Target" select="."/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:for-each select="oil:informationType[not(child::oil:singleRoleUniquenessConstraint/@isPreferred='true' and ancestor::oil:conceptType[@name!=parent::oil:conceptType/@name])]">
			<xsl:call-template name="GetColumnForInformationType">
				<xsl:with-param name="DataTypes" select="$DataTypes"/>
				<xsl:with-param name="TargetInformationType" select="."/>
				<xsl:with-param name="AlwaysNullable" select="$AlwaysNullable"/>
				<xsl:with-param name="AllowIdentity" select="boolean(@name=@formatRef)"/>
				<xsl:with-param name="Prefix" select="$prefix"/>
			</xsl:call-template>
			<xsl:for-each select="oil:singleRoleUniquenessConstraint[@modality='alethic']">
				<dcl:uniquenessConstraint name="{dsf:makeValidIdentifier(concat($prefix,@name))}" isPrimary="{@isPreferred}" oilRefName="{@name}">
					<dcl:columnRef name="{dsf:makeValidIdentifier(concat($prefix,../@name))}" oilRefName="{$prefix}{../@name}"/>
				</dcl:uniquenessConstraint>
			</xsl:for-each>
		</xsl:for-each>

		<xsl:for-each select="oil:conceptTypeRef">
			<xsl:variable name="conceptTypeRefName" select="@name"/>
			<xsl:variable name="isNullable" select="not(@mandatory='alethic')"/>
			<xsl:variable name="foreignKeyColumnsWithOriginalNamesFragment">
				<xsl:call-template name="GetPreferredIdentifierColumnsForConceptTypeRef">
					<xsl:with-param name="OilModel" select="$OilModel"/>
					<xsl:with-param name="DataTypes" select="$DataTypes"/>
					<xsl:with-param name="TargetConceptTypeRef" select="."/>
					<xsl:with-param name="AlwaysNullable" select="$isNullable"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="foreignKeyColumnsWithOriginalNames" select="exsl:node-set($foreignKeyColumnsWithOriginalNamesFragment)/child::*"/>
			<xsl:variable name="foreignKeyColumnsFragment">
				<xsl:call-template name="AddPrefixToNamesForConceptTypeRef">
					<xsl:with-param name="ConceptTypeRef" select="."/>
					<xsl:with-param name="Targets" select="$foreignKeyColumnsWithOriginalNames"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="foreignKeyColumns" select="exsl:node-set($foreignKeyColumnsFragment)/child::*"/>
			<xsl:copy-of select="$foreignKeyColumns"/>
			<xsl:if test="$isNullable and count($foreignKeyColumns)>1">
				<dcl:checkConstraint name="{dsf:makeValidIdentifier(concat($conceptTypeRefName,'_Chk'))}">
					<!-- TODO: Check clause for all null or all not null -->
				</dcl:checkConstraint>
			</xsl:if>
			<dcl:referenceConstraint name="{dsf:makeValidIdentifier(concat($conceptTypeRefName,'_FK'))}" oilRefName="{$conceptTypeRefName}">
				<xsl:attribute name="targetTable">
					<xsl:call-template name="GetTableNameForConceptTypeName">
						<xsl:with-param name="OilModel" select="$OilModel"/>
						<xsl:with-param name="TargetConceptTypeName" select="@target"/>
					</xsl:call-template>
				</xsl:attribute>
				<xsl:call-template name="GenerateColumnRefsForReferenceConstraint">
					<xsl:with-param name="SourceColumns" select="$foreignKeyColumns"/>
					<xsl:with-param name="TargetColumns" select="$foreignKeyColumnsWithOriginalNames"/>
				</xsl:call-template>
			</dcl:referenceConstraint>
		</xsl:for-each>

		<xsl:for-each select="oil:conceptType">
			<xsl:apply-templates select="." mode="GenerateTableContent">
				<xsl:with-param name="OilModel" select="$OilModel"/>
				<xsl:with-param name="DataTypes" select="$DataTypes"/>
				<xsl:with-param name="AlwaysNullable" select="not(@mandatory='alethic')"/>
			</xsl:apply-templates>
		</xsl:for-each>

		<xsl:if test="$AlwaysNullable">
			<!-- TODO: We're nested and not-alethicly-mandatory in our parent, so add constraints for enforcing our mandatories. -->
		</xsl:if>

		<!-- TODO: This will break if @targetConceptType is not this table. -->
		<xsl:for-each select="oil:roleSequenceUniquenessConstraint[@modality='alethic']">
			<dcl:uniquenessConstraint name="{dsf:makeValidIdentifier(concat($prefix,@name))}" isPrimary="{@isPreferred}" oilRefName="{@name}">
				<xsl:for-each select="oil:roleSequence/oil:typeRef">
					<xsl:variable name="refTarget" select="parent::oil:roleSequence/parent::oil:roleSequenceUniquenessConstraint/parent::oil:conceptType/child::*[@name=current()/@targetChild]"/>
					<xsl:call-template name="GetColumnRef">
						<xsl:with-param name="OilModel" select="$OilModel"/>
						<xsl:with-param name="DataTypes" select="$DataTypes"/>
						<xsl:with-param name="Target" select="$refTarget"/>
					</xsl:call-template>
					<!--
					<xsl:choose>
						<xsl:when test="$refTarget[self::oil:informationType]">
							<dcl:columnRef name="{dsf:makeValidIdentifier(concat($prefix,@targetChild))}"/>
						</xsl:when>
						<xsl:when test="$regTarget[self::oil:conceptTypeRef]">
						</xsl:when>
					</xsl:choose>-->
				</xsl:for-each>
			</dcl:uniquenessConstraint>
		</xsl:for-each>

	</xsl:template>

	<!--Generate the insert procedure-->
	<xsl:template match="oil:conceptType" mode="GenerateInsertProcedure">
		<xsl:param name="Table"/>
		<xsl:param name="DomainDataTypes"/>
		<dcl:procedure name="{dsf:makeValidIdentifier(concat('Insert',@name))}" sqlDataAccessIndication="MODIFIES SQL DATA" oilRefName="{@name}">
			<xsl:for-each select="$Table/dcl:column">
				<dcl:parameter mode="IN" name="{@name}">
					<xsl:copy-of select="dcl:predefinedDataType"/>
					<xsl:copy-of select="$DomainDataTypes[@name=current()/dcl:domainDataTypeRef/@name]/dcl:predefinedDataType"/>
				</dcl:parameter>
			</xsl:for-each>
			<dml:insertStatement schema="{dsf:makeValidIdentifier(../@name)}" name="{dsf:makeValidIdentifier(@name)}">
				<dml:fromConstructor>
					<xsl:for-each select="$Table/dcl:column">
						<ddl:column name="{@name}"/>
					</xsl:for-each>
					<xsl:for-each select="$Table/dcl:column">
						<dep:sqlParameterReference name="{@name}"/>
					</xsl:for-each>
				</dml:fromConstructor>
			</dml:insertStatement>
		</dcl:procedure>
	</xsl:template>
	<!--End of insert statement-->

	<!-- Generate the delete statement -->
	<xsl:template match="oil:conceptType" mode="GenerateDeleteProcedure">
		<xsl:param name="Table" />
		<xsl:param name="OilModel" />
		<xsl:param name="DomainDataTypes" />
		<xsl:param name="DataTypes" />
		<dcl:procedure name="{dsf:makeValidIdentifier(concat('Delete',@name))}" sqlDataAccessIndication="MODIFIES SQL DATA" oilRefName="{@name}">

			<xsl:variable name="preferredIdentifierColumnsFragment">
				<xsl:call-template name="GetPreferredIdentifierColumnsForConceptType">
					<xsl:with-param name="OilModel" select="$OilModel"/>
					<xsl:with-param name="DataTypes" select="$DataTypes"/>
					<xsl:with-param name="TargetConceptType" select="."/>
					<xsl:with-param name="AddPrefixForConceptTypeRef" select="true()"/>
				</xsl:call-template>
			</xsl:variable>

			<xsl:variable name="preferredIdentifierColumns" select="exsl:node-set($preferredIdentifierColumnsFragment)" />

			<xsl:for-each select="$preferredIdentifierColumns/child::*">
				<dcl:parameter mode="IN" name="{@name}">
					<xsl:copy-of select="dcl:predefinedDataType"/>
					<xsl:copy-of select="$DomainDataTypes[@name=current()/dcl:domainDataTypeRef/@name]/dcl:predefinedDataType"/>
				</dcl:parameter>
			</xsl:for-each>

			<dml:deleteStatement schema="{dsf:makeValidIdentifier(../@name)}" name="{dsf:makeValidIdentifier(@name)}">
				<dml:whereClause>
					<dml:searchCondition>
						<xsl:choose>
							<!-- If there is more than one child of the identifier columns it is a role sequence uniqueness contraint.-->
							<xsl:when test="count($preferredIdentifierColumns/child::*) > 1">
								<dep:and>
									<xsl:for-each select="$preferredIdentifierColumns/dcl:column">
										<dep:comparisonPredicate operator="equals">
											<dep:columnReference name="{@name}"/>
											<dep:sqlParameterReference name="{@name}"/>
										</dep:comparisonPredicate>
									</xsl:for-each>
								</dep:and>
							</xsl:when>
							<xsl:when test="count($preferredIdentifierColumns/child::*) = 1">
								<dep:comparisonPredicate operator="equals">
									<dep:columnReference name="{$preferredIdentifierColumns/dcl:column/@name}"/>
									<dep:sqlParameterReference name="{$preferredIdentifierColumns/dcl:column/@name}"/>
								</dep:comparisonPredicate>
							</xsl:when>
							<xsl:otherwise>
								<xsl:message terminate="yes">
									Error creating Delete statement.  Unable to locate a
									oil:roleSequenceUniquenessConstraint that is prefferred or an
									oil:singleRoleUniquenessConstraint that is preferred.
								</xsl:message>
							</xsl:otherwise>
						</xsl:choose>
					</dml:searchCondition>
				</dml:whereClause>
			</dml:deleteStatement>
		</dcl:procedure>
	</xsl:template>
	<!--End of delete statement-->
	<!--Generate update procedures - one for each column, searched by primary key-->
	<xsl:template match="oil:conceptType" mode="GenerateUpdateProcedures">
		<xsl:param name="OilModel" />
		<xsl:param name="DataTypes" />
		<xsl:param name="Table"/>
		<xsl:param name="DomainDataTypes"/>

		<xsl:variable name="preferredIdentifierColumnsFragment">
			<xsl:call-template name="GetPreferredIdentifierColumnsForConceptType">
				<xsl:with-param name="OilModel" select="$OilModel"/>
				<xsl:with-param name="DataTypes" select="$DataTypes"/>
				<xsl:with-param name="TargetConceptType" select="."/>
				<xsl:with-param name="AddPrefixForConceptTypeRef" select="true()"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="preferredIdentifierColumns" select="exsl:node-set($preferredIdentifierColumnsFragment)" />
		<xsl:variable name="SchemaName" select="dsf:makeValidIdentifier(../@name)" />
		<xsl:variable name="TableName" select="dsf:makeValidIdentifier(@name)"/>


		<xsl:for-each select="$Table/dcl:column[not(@isIdentity='true')]">
			<dcl:procedure name="{dsf:makeValidIdentifier(concat('Update',../@name,@name))}" sqlDataAccessIndication="MODIFIES SQL DATA" oilRefName="{../@name}" oilColumnRef="{@name}">
				<xsl:for-each select="$preferredIdentifierColumns/dcl:column">
					<dcl:parameter mode="IN" name="{dsf:makeValidIdentifier(concat('old_',@name))}">
						<xsl:copy-of select="dcl:predefinedDataType"/>
						<xsl:copy-of select="$DomainDataTypes[@name=current()/dcl:domainDataTypeRef/@name]/dcl:predefinedDataType"/>
					</dcl:parameter>
				</xsl:for-each>
				<dcl:parameter mode="IN" name="{@name}">
					<xsl:copy-of select="dcl:predefinedDataType"/>
					<xsl:copy-of select="$DomainDataTypes[@name=current()/dcl:domainDataTypeRef/@name]/dcl:predefinedDataType"/>
				</dcl:parameter>
				<dml:updateStatement schema="{$SchemaName}" name="{$TableName}" >
					<dml:setClause>
						<ddl:column name="{@name}"/>
						<dep:sqlParameterReference name="{@name}"/>
					</dml:setClause>
					<dml:whereClause>
						<dml:searchCondition>
							<xsl:choose>
								<!-- If there is more than one child of the identifier columns it is a role sequence uniqueness contraint.-->
								<xsl:when test="count($preferredIdentifierColumns/child::*) > 1">
									<dep:and>
										<xsl:for-each select="$preferredIdentifierColumns/dcl:column">
											<dep:comparisonPredicate operator="equals">
												<dep:columnReference name="{@name}"/>
												<dep:sqlParameterReference name="{dsf:makeValidIdentifier(concat('old_',@name))}"/>
											</dep:comparisonPredicate>
										</xsl:for-each>
									</dep:and>
								</xsl:when>
								<xsl:when test="count($preferredIdentifierColumns/child::*) = 1">
									<xsl:for-each select="$preferredIdentifierColumns/dcl:column">
										<dep:comparisonPredicate operator="equals">
											<dep:columnReference name="{@name}"/>
											<dep:sqlParameterReference name="{dsf:makeValidIdentifier(concat('old_',@name))}"/>
										</dep:comparisonPredicate>
									</xsl:for-each>
								</xsl:when>
								<xsl:otherwise>
									<xsl:message terminate="yes">
										Error creating Delete statement.  Unable to locate a
										oil:roleSequenceUniquenessConstraint that is prefferred or an
										oil:singleRoleUniquenessConstraint that is preferred.
									</xsl:message>
								</xsl:otherwise>
							</xsl:choose>
						</dml:searchCondition>
					</dml:whereClause>
				</dml:updateStatement>
			</dcl:procedure>
		</xsl:for-each>
	</xsl:template>
	<!--End of update procedure generation-->
	<!--Select procedures generation-->
	<xsl:template match="oil:conceptType" mode="GenerateSelectProcedures">
		<xsl:param name="OilModel" />
		<xsl:param name="DataTypes" />
		<xsl:param name="Table"/>
		<xsl:param name="DomainDataTypes"/>

		<xsl:variable name="preferredIdentifierColumnsFragment">
			<xsl:call-template name="GetPreferredIdentifierColumnsForConceptType">
				<xsl:with-param name="OilModel" select="$OilModel"/>
				<xsl:with-param name="DataTypes" select="$DataTypes"/>
				<xsl:with-param name="TargetConceptType" select="."/>
				<xsl:with-param name="AddPrefixForConceptTypeRef" select="true()"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="preferredIdentifierColumns" select="exsl:node-set($preferredIdentifierColumnsFragment)" />
		<xsl:variable name="SchemaName" select="dsf:makeValidIdentifier(../@name)" />
		<xsl:variable name="TableName" select="dsf:makeValidIdentifier(@name)"/>

		<!-- for each unique identified, generate a select statement taking the columns in that identifier as parameters -->
		
	</xsl:template>
	<!--End of select proceure generation-->


	<xsl:template name="GetTableNameForConceptTypeName">
		<xsl:param name="OilModel"/>
		<xsl:param name="TargetConceptTypeName"/>
		<xsl:value-of select="dsf:makeValidIdentifier($OilModel/oil:conceptType[descendant-or-self::oil:conceptType[@name=$TargetConceptTypeName]]/@name)"/>
	</xsl:template>


	<xsl:template name="GetPrefix">
		<xsl:param name="Target"/>
		<xsl:variable name="parentConceptType" select="$Target/parent::oil:conceptType"/>
		<xsl:if test="$parentConceptType">
			<xsl:call-template name="GetPrefix">
				<xsl:with-param name="Target" select="$parentConceptType"/>
			</xsl:call-template>
			<xsl:if test="$Target/self::oil:conceptType">
				<xsl:value-of select="concat($Target/@name,'_')"/>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template name="AddPrefixToNamesForConceptTypeRef">
		<xsl:param name="ConceptTypeRef"/>
		<xsl:param name="Targets"/>
		<xsl:variable name="prefix" select="concat($ConceptTypeRef/@name,'_')"/>
		<xsl:for-each select="$Targets">
			<xsl:copy>
				<xsl:copy-of select="@*"/>
				<xsl:attribute name="name">
					<xsl:value-of select="dsf:makeValidIdentifier(concat($prefix,@name))"/>
				</xsl:attribute>
				<xsl:copy-of select="child::node()"/>
			</xsl:copy>
		</xsl:for-each>
	</xsl:template>

	<xsl:template name="GetColumnRef">
		<xsl:param name="OilModel"/>
		<xsl:param name="DataTypes"/>
		<xsl:param name="Target"/>
		<xsl:choose>
			<xsl:when test="$Target/self::oil:conceptType or $Target/self::oil:conceptTypeRef">
				<xsl:variable name="conceptTypeRefPrefix">
					<xsl:if test="$Target/self::oil:conceptTypeRef">
						<xsl:value-of select="concat($Target/@name,'_')"/>
					</xsl:if>
				</xsl:variable>
				<xsl:variable name="preferredIdentifierColumnsFragment">
					<xsl:choose>
						<xsl:when test="$Target/self::oil:conceptType">
							<xsl:call-template name="GetPreferredIdentifierColumnsForConceptType">
								<xsl:with-param name="OilModel" select="$OilModel"/>
								<xsl:with-param name="DataTypes" select="$DataTypes"/>
								<xsl:with-param name="TargetConceptType" select="$Target"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:when test="$Target/self::oil:conceptTypeRef">
							<xsl:call-template name="GetPreferredIdentifierColumnsForConceptTypeRef">
								<xsl:with-param name="OilModel" select="$OilModel"/>
								<xsl:with-param name="DataTypes" select="$DataTypes"/>
								<xsl:with-param name="TargetConceptTypeRef" select="$Target"/>
							</xsl:call-template>
						</xsl:when>
					</xsl:choose>
				</xsl:variable>
				<xsl:for-each select="exsl:node-set($preferredIdentifierColumnsFragment)/child::*">
					<dcl:columnRef name="{dsf:makeValidIdentifier(concat($conceptTypeRefPrefix,@name))}" oilRefName="{$conceptTypeRefPrefix}{@name}"/>
				</xsl:for-each>
			</xsl:when>
			<xsl:when test="$Target/self::oil:informationType">
				<xsl:variable name="informationTypeColumnFragment">
					<xsl:call-template name="GetColumnForInformationType">
						<xsl:with-param name="DataTypes" select="$DataTypes"/>
						<xsl:with-param name="TargetInformationType" select="$Target"/>
					</xsl:call-template>
				</xsl:variable>
				<dcl:columnRef name="{exsl:node-set($informationTypeColumnFragment)/child::*/@name}" oilRefName="{$Target/@name}"/>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="GetColumnForInformationType">
		<xsl:param name="DataTypes"/>
		<xsl:param name="TargetInformationType"/>
		<xsl:param name="AlwaysNullable" select="false()"/>
		<xsl:param name="AllowIdentity" select="false()"/>
		<xsl:param name="Prefix">
			<xsl:call-template name="GetPrefix">
				<xsl:with-param name="Target" select="$TargetInformationType"/>
			</xsl:call-template>
		</xsl:param>
		<xsl:variable name="dataType" select="$DataTypes[@name=$TargetInformationType/@formatRef]"/>
		<dcl:column name="{dsf:makeValidIdentifier(concat($Prefix,$TargetInformationType/@name))}" isNullable="{$AlwaysNullable or not($TargetInformationType/@mandatory='alethic')}" isIdentity="{$AllowIdentity and $dataType/@isForIdentity='true'}" oilRefName="{$TargetInformationType/@name}">
			<xsl:copy-of select="$dataType/child::*"/>
		</dcl:column>
	</xsl:template>
	<xsl:template name="GetPreferredIdentifierColumnsForConceptTypeRef">
		<xsl:param name="OilModel"/>
		<xsl:param name="DataTypes"/>
		<xsl:param name="TargetConceptTypeRef"/>
		<xsl:param name="AlwaysNullable" select="false()"/>
		<xsl:param name="AddPrefixForConceptTypeRef" select="false()"/>
		<xsl:variable name="preferredIdentifierColumnsFragment">
			<xsl:call-template name="GetPreferredIdentifierColumnsForConceptType">
				<xsl:with-param name="OilModel" select="$OilModel"/>
				<xsl:with-param name="DataTypes" select="$DataTypes"/>
				<xsl:with-param name="TargetConceptType" select="$OilModel//oil:conceptType[@name=$TargetConceptTypeRef/@target]"/>
				<xsl:with-param name="AlwaysNullable" select="$AlwaysNullable"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="preferredIdentifierColumns" select="exsl:node-set($preferredIdentifierColumnsFragment)/child::*"/>
		<xsl:choose>
			<xsl:when test="$AddPrefixForConceptTypeRef">
				<xsl:call-template name="AddPrefixToNamesForConceptTypeRef">
					<xsl:with-param name="ConceptTypeRef" select="$TargetConceptTypeRef"/>
					<xsl:with-param name="Targets" select="$preferredIdentifierColumns"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="$preferredIdentifierColumns"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="GetPreferredIdentifierColumnsForConceptType">
		<xsl:param name="OilModel"/>
		<xsl:param name="DataTypes"/>
		<xsl:param name="TargetConceptType"/>
		<xsl:param name="AlwaysNullable" select="false()"/>
		<xsl:param name="AddPrefixForConceptTypeRef" select="false()"/>
		<xsl:variable name="preferredIdentifierInformationType" select="$TargetConceptType/oil:informationType[oil:singleRoleUniquenessConstraint[@isPreferred='true']]"/>
		<xsl:variable name="preferredIdentifierRoleSequenceUniquenessConstraint" select="$TargetConceptType/oil:roleSequenceUniquenessConstraint[@isPreferred='true']"/>
		<xsl:choose>
			<xsl:when test="$preferredIdentifierInformationType">
				<xsl:for-each select="$preferredIdentifierInformationType">
					<xsl:call-template name="GetColumnForInformationType">
						<xsl:with-param name="DataTypes" select="$DataTypes"/>
						<xsl:with-param name="TargetInformationType" select="."/>
						<xsl:with-param name="AlwaysNullable" select="$AlwaysNullable"/>
					</xsl:call-template>
				</xsl:for-each>
			</xsl:when>
			<xsl:when test="$preferredIdentifierRoleSequenceUniquenessConstraint">
				<xsl:for-each select="$preferredIdentifierRoleSequenceUniquenessConstraint/oil:roleSequence/oil:typeRef">
					<xsl:choose>
						<xsl:when test="@targetConceptType = $TargetConceptType/@name">
							<!-- If @targetConceptType is the oil:conceptType that we're processing, it means that there must be a @targetChild that points to an oil:informationType or oil:conceptTypeRef within us. -->
							<xsl:variable name="targetChild" select="$TargetConceptType/child::*[@name=current()/@targetChild]"/>
							<xsl:choose>
								<xsl:when test="$targetChild/self::oil:informationType">
									<xsl:call-template name="GetColumnForInformationType">
										<xsl:with-param name="DataTypes" select="$DataTypes"/>
										<xsl:with-param name="TargetInformationType" select="$targetChild"/>
										<xsl:with-param name="AlwaysNullable" select="$AlwaysNullable"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:when test="$targetChild/self::oil:conceptTypeRef">
									<xsl:call-template name="GetPreferredIdentifierColumnsForConceptTypeRef">
										<xsl:with-param name="OilModel" select="$OilModel"/>
										<xsl:with-param name="DataTypes" select="$DataTypes"/>
										<xsl:with-param name="TargetConceptTypeRef" select="$targetChild"/>
										<xsl:with-param name="AlwaysNullable" select="$AlwaysNullable"/>
										<xsl:with-param name="AddPrefixForConceptTypeRef" select="$AddPrefixForConceptTypeRef"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:message terminate="yes">
										<xsl:text>Something has gone very wrong...</xsl:text>
									</xsl:message>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:when test="not(@targetChild)">
							<!-- Not having a @targetChild means that the ref is to an oil:conceptType -->
							<xsl:call-template name="GetPreferredIdentifierColumnsForConceptType">
								<xsl:with-param name="OilModel" select="$OilModel"/>
								<xsl:with-param name="DataTypes" select="$DataTypes"/>
								<xsl:with-param name="TargetConceptType" select="$OilModel//oil:conceptType[@name=current()/@targetConceptType]"/>
								<xsl:with-param name="AlwaysNullable" select="$AlwaysNullable"/>
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>
							<xsl:message terminate="yes">
								<xsl:text>Something has gone very wrong...</xsl:text>
							</xsl:message>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
			</xsl:when>
			<xsl:when test="$TargetConceptType/parent::oil:conceptType">
				<xsl:call-template name="GetPreferredIdentifierColumnsForConceptType">
					<xsl:with-param name="OilModel" select="$OilModel"/>
					<xsl:with-param name="DataTypes" select="$DataTypes"/>
					<xsl:with-param name="TargetConceptType" select="$TargetConceptType/parent::oil:conceptType"/>
					<xsl:with-param name="AlwaysNullable" select="$AlwaysNullable"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:message terminate="yes">
					<xsl:text>ERROR: oil:conceptType doesn't contain a preferred uniqueness constraint and isn't nested inside of another oil:conceptType</xsl:text>
				</xsl:message>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!--
		<xsl:for-each select="oil:roleSequenceUniquenessConstraint[@modality='alethic']">
			<dcl:uniquenessConstraint name="{dsf:makeValidIdentifier(concat($prefix,@name))}" isPrimary="{@isPrimary}">
				<xsl:for-each select="oil:roleSequence/oil:typeRef">
					<!- - TODO: This probably isn't going to work for typeRefs to nested conceptTypes - ->
					<dcl:columnRef name="{dsf:makeValidIdentifier(concat($prefix,@targetChild))}"/>
				</xsl:for-each>
			</dcl:uniquenessConstraint>
		</xsl:for-each>
	-->

	<xsl:template name="ProcessOilRangeForNumber">
		<xsl:param name="currentNr" select="1"/>
		<xsl:param name="totalNr" select="count(odt:range)"/>
		<xsl:param name="predicandToTest"/>
		<xsl:param name="exactOrApproximate"/>
		<xsl:variable name="rangeCode">
			<xsl:variable name="currentRange" select="odt:range[$currentNr]"/>
			<xsl:variable name="lowerBoundLiteral">
				<xsl:if test="$currentRange/odt:lowerBound">
					<xsl:element name="{concat('ddt:',$exactOrApproximate,'NumericLiteral')}">
						<xsl:attribute name="value">
							<xsl:value-of select="$currentRange/odt:lowerBound/@value"/>
						</xsl:attribute>
					</xsl:element>
				</xsl:if>
			</xsl:variable>
			<xsl:variable name="upperBoundLiteral">
				<xsl:if test="$currentRange/odt:upperBound">
					<xsl:element name="{concat('ddt:',$exactOrApproximate,'NumericLiteral')}">
						<xsl:attribute name="value">
							<xsl:value-of select="$currentRange/odt:upperBound/@value"/>
						</xsl:attribute>
					</xsl:element>
				</xsl:if>
			</xsl:variable>
			<xsl:variable name="lowerBoundOperator">
				<xsl:if test="$currentRange/odt:lowerBound">
					<xsl:choose>
						<xsl:when test="$currentRange/odt:lowerBound/@clusivity='exclusive'">
							<xsl:value-of select="'greaterThan'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'greaterThanOrEquals'"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:if>
			</xsl:variable>
			<xsl:variable name="upperBoundOperator">
				<xsl:if test="$currentRange/odt:upperBound">
					<xsl:choose>
						<xsl:when test="$currentRange/odt:upperBound/@clusivity='exclusive'">
							<xsl:value-of select="'lessThan'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'lessThanOrEquals'"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:if>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="$lowerBoundOperator='greaterThanOrEquals' and $upperBoundOperator='lessThanOrEquals'">
					<dep:betweenPredicate type="BETWEEN">
						<xsl:copy-of select="$predicandToTest"/>
						<xsl:copy-of select="$lowerBoundLiteral"/>
						<xsl:copy-of select="$upperBoundLiteral"/>
					</dep:betweenPredicate>
				</xsl:when>
				<xsl:when test="string-length($lowerBoundOperator) and string-length($upperBoundOperator)">
					<dep:and>
						<dep:comparisonPredicate operator="{$lowerBoundOperator}">
							<xsl:copy-of select="$predicandToTest"/>
							<xsl:copy-of select="$lowerBoundLiteral"/>
						</dep:comparisonPredicate>
						<dep:comparisonPredicate operator="{$upperBoundOperator}">
							<xsl:copy-of select="$predicandToTest"/>
							<xsl:copy-of select="$upperBoundLiteral"/>
						</dep:comparisonPredicate>
					</dep:and>
				</xsl:when>
				<xsl:when test="string-length($lowerBoundOperator)">
					<dep:comparisonPredicate operator="{$lowerBoundOperator}">
						<xsl:copy-of select="$predicandToTest"/>
						<xsl:copy-of select="$lowerBoundLiteral"/>
					</dep:comparisonPredicate>
				</xsl:when>
				<xsl:when test="string-length($upperBoundOperator)">
					<dep:comparisonPredicate operator="{$upperBoundOperator}">
						<xsl:copy-of select="$predicandToTest"/>
						<xsl:copy-of select="$upperBoundLiteral"/>
					</dep:comparisonPredicate>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="currentNr &lt; totalNr">
				<dep:or>
					<xsl:copy-of select="$rangeCode"/>
					<xsl:call-template name="ProcessOilRangeForNumber">
						<xsl:with-param name="currentNr" select="$currentNr + 1"/>
						<xsl:with-param name="totalNr" select="$totalNr"/>
						<xsl:with-param name="predicandToTest" select="$predicandToTest"/>
						<xsl:with-param name="exactOrApproximate" select="$exactOrApproximate"/>
					</xsl:call-template>
				</dep:or>
			</xsl:when>
			<xsl:otherwise>
				<xsl:copy-of select="$rangeCode"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="GetFloatPrecisionAsNumber">
		<xsl:param name="Precision"/>
		<xsl:choose>
			<xsl:when test="$Precision='single'">
				<xsl:value-of select="24"/>
			</xsl:when>
			<xsl:when test="$Precision='double'">
				<xsl:value-of select="53"/>
			</xsl:when>
			<xsl:when test="$Precision='quad'">
				<xsl:value-of select="113"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$Precision"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="GenerateColumnRefsForReferenceConstraint">
		<xsl:param name="SourceColumns"/>
		<xsl:param name="TargetColumns"/>
		<xsl:param name="CurrentColumn" select="1"/>
		<xsl:param name="TotalColumns" select="count($SourceColumns)"/>
		<dcl:columnRef sourceName="{$SourceColumns[$CurrentColumn]/@name}" targetName="{$TargetColumns[$CurrentColumn]/@name}" oilRefName="{@name}_{$TargetColumns[$CurrentColumn]/@oilRefName}"/>
		<xsl:if test="$CurrentColumn &lt; $TotalColumns">
			<xsl:call-template name="GenerateColumnRefsForReferenceConstraint">
				<xsl:with-param name="SourceColumns" select="$SourceColumns"/>
				<xsl:with-param name="TargetColumns" select="$TargetColumns"/>
				<xsl:with-param name="CurrentColumn" select="$CurrentColumn + 1"/>
				<xsl:with-param name="TotalColumns" select="$TotalColumns"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>
