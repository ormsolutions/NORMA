<?xml version="1.0" encoding="utf-8"?>
<!--
	Neumont Object-Role Modeling Architect for Visual Studio

	Copyright © Neumont University. All rights reserved.
	Copyright © ORM Solutions, LLC. All rights reserved.

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
	xmlns:orm="http://schemas.neumont.edu/ORM/2006-04/ORMCore"
	xmlns:ormtooial="http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMToORMAbstraction"
	xmlns:oial="http://schemas.neumont.edu/ORM/Abstraction/2007-06/Core"
	xmlns:odt="http://schemas.neumont.edu/ORM/Abstraction/2007-06/DataTypes/Core"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:prop="urn:schemas-orm-net:PLiX:CLI:Properties"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:fn="urn:functions"
	exclude-result-prefixes="orm oial ormtooial odt"
	extension-element-prefixes="exsl msxsl fn">
	<xsl:include href="OIALtoPLiX_GlobalSupportFunctions.xslt"/>

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:param name="EnableAssertions" select="true()"/>

	<xsl:param name="SmallIntegerMinValue" select="number(-32768)"/>
	<xsl:param name="SmallIntegerMaxValue" select="number(32767)"/>
	<xsl:param name="UnsignedTinyIntegerMinValue" select="number(0)"/>
	<xsl:param name="UnsignedTinyIntegerMaxValue" select="number(255)"/>
	<xsl:param name="UnsignedSmallIntegerMinValue" select="number(0)"/>
	<xsl:param name="UnsignedSmallIntegerMaxValue" select="number(65535)"/>
	<xsl:param name="IntegerMinValue" select="number(-2147483648)"/>
	<xsl:param name="IntegerMaxValue" select="number(2147483647)"/>
	<xsl:param name="UnsignedIntegerMinValue" select="number(0)"/>
	<xsl:param name="UnsignedIntegerMaxValue" select="number(4294967295)"/>
	<xsl:param name="LargeIntegerMinValue" select="number(-9223372036854775808)"/>
	<xsl:param name="LargeIntegerMaxValue" select="number(9223372036854775807)"/>
	<xsl:param name="UnsignedLargeIntegerMinValue" select="number(0)"/>
	<xsl:param name="UnsignedLargeIntegerMaxValue" select="number(18446744073709551615)"/>
	<xsl:param name="SingleFloatingPointPrecision" select="number(24)"/>
	<xsl:param name="DoubleFloatingPointPrecision" select="number(53)"/>

	<xsl:variable name="ORMModel" select="*/orm:ORMModel"/>
	<xsl:variable name="ORMDataTypes" select="$ORMModel/orm:DataTypes/orm:*"/>
	<xsl:variable name="ORMObjectTypes" select="$ORMModel/orm:Objects/orm:*"/>
	<xsl:variable name="ORMValueTypes" select="$ORMObjectTypes[self::orm:ValueType]"/>
	<xsl:variable name="OialBridgeMappings" select="*/ormtooial:Bridge/ormtooial:*"/>
	<xsl:variable name="OialModel" select="*/oial:model"/>
	<xsl:variable name="ConceptTypes" select="$OialModel/oial:conceptTypes/oial:conceptType"/>
	<xsl:variable name="ExpandedInformationTypeFormatsFragment">
		<xsl:apply-templates select="$OialModel/oial:informationTypeFormats/child::*" mode="ExpandInformationTypeFormats">
			<xsl:with-param name="DataTypes" select="$ORMDataTypes"/>
			<xsl:with-param name="ValueTypes" select="$ORMValueTypes"/>
			<xsl:with-param name="BridgeMappings" select="$OialBridgeMappings[self::ormtooial:InformationTypeFormatIsForValueType]"/>
		</xsl:apply-templates>
	</xsl:variable>
	<xsl:variable name="InformationTypeFormatMappingsFragment">
		<xsl:apply-templates select="exsl:node-set($ExpandedInformationTypeFormatsFragment)/child::*" mode="GenerateInformationTypeFormatMapping"/>
	</xsl:variable>
	<xsl:variable name="InformationTypeFormatMappings" select="exsl:node-set($InformationTypeFormatMappingsFragment)/child::*"/>

	<xsl:template match="/">
		<xsl:variable name="ConceptTypeRefs" select="$ConceptTypes/oial:children/child::*[self::oial:relatedConceptType or self::oial:assimilatedConceptType]"/>
		<xsl:variable name="normalizedConceptTypeNamesFragment">
			<xsl:for-each select="$ConceptTypes">
				<oial:conceptType id="{@id}" name="{fn:normalizeName(@name,false())}" paramName="{fn:normalizeName(@name,true())}">
					<xsl:variable name="childNamesFragment">
						<xsl:for-each select="oial:children/oial:*">
							<!-- The native format uses name for information types where opposite name
							is used for other types. It is the opposite names that we need to be unique,
							so we map 'name' if it is the only name attribute specified to oppositeName
							for this temporary structure used to normalize names and look for duplicates. -->
							<xsl:copy>
								<xsl:copy-of select="@id"/>
								<xsl:choose>
									<xsl:when test="@oppositeName">
										<xsl:attribute name="name">
											<xsl:value-of select="fn:normalizeName(@name,false())"/>
										</xsl:attribute>
										<xsl:attribute name="paramName">
											<xsl:value-of select="fn:normalizeName(@name,true())"/>
										</xsl:attribute>
										<xsl:attribute name="oppositeName">
											<xsl:value-of select="fn:normalizeName(@oppositeName,false())"/>
										</xsl:attribute>
										<xsl:attribute name="oppositeParamName">
											<xsl:value-of select="fn:normalizeName(@oppositeName,true())"/>
										</xsl:attribute>
									</xsl:when>
									<xsl:otherwise>
										<xsl:attribute name="oppositeName">
											<xsl:value-of select="fn:normalizeName(@name,false())"/>
										</xsl:attribute>
										<xsl:attribute name="oppositeParamName">
											<xsl:value-of select="fn:normalizeName(@name,true())"/>
										</xsl:attribute>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:copy>
						</xsl:for-each>
					</xsl:variable>
					<xsl:for-each select="exsl:node-set($childNamesFragment)/child::*">
						<xsl:copy>
							<xsl:copy-of select="@id"/>
							<xsl:variable name="previousNameMatches" select="preceding-sibling::*/@oppositeName[.=current()/@oppositeName]"/>
							<xsl:variable name="followingNameMatches" select="following-sibling::*/@oppositeName[.=current()/@oppositeName]"/>
							<xsl:variable name="appendNumberFragment">
								<xsl:choose>
									<xsl:when test="$previousNameMatches">
										<xsl:value-of select="count($previousNameMatches) + 1"/>
									</xsl:when>
									<xsl:when test="$followingNameMatches">
										<xsl:value-of select="1"/>
									</xsl:when>
								</xsl:choose>
							</xsl:variable>
							<xsl:variable name="appendNumber" select="string($appendNumberFragment)"/>
							<xsl:choose>
								<xsl:when test="@name">
									<xsl:copy-of select="@name"/>
									<xsl:copy-of select="@paramName"/>
									<xsl:attribute name="oppositeName">
										<xsl:value-of select="@oppositeName"/>
										<xsl:value-of select="$appendNumber"/>
									</xsl:attribute>
									<xsl:attribute name="oppositeParamName">
										<xsl:value-of select="@oppositeParamName"/>
										<xsl:value-of select="$appendNumber"/>
									</xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="name">
										<xsl:value-of select="@oppositeName"/>
										<xsl:value-of select="$appendNumber"/>
									</xsl:attribute>
									<xsl:attribute name="paramName">
										<xsl:value-of select="@oppositeParamName"/>
										<xsl:value-of select="$appendNumber"/>
									</xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:copy>
					</xsl:for-each>
				</oial:conceptType>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="normalizedConceptTypeNames" select="exsl:node-set($normalizedConceptTypeNamesFragment)/child::*"/>
		<prop:AllProperties modelName="{fn:normalizeName($OialModel/@name,false())}">
			<xsl:for-each select="$ConceptTypes">
				<xsl:variable name="normalizedNameElement" select="$normalizedConceptTypeNames[@id=current()/@id]"/>
				<prop:Properties conceptTypeName="{$normalizedNameElement/@name}" conceptTypeParamName="{$normalizedNameElement/@paramName}"  conceptTypeId="{@id}">
					<xsl:apply-templates select="." mode="GenerateProperties">
						<xsl:with-param name="NormalizedConceptTypeNames" select="$normalizedConceptTypeNames"/>
						<xsl:with-param name="NormalizedConceptTypeChildNames" select="$normalizedConceptTypeNames/*"/>
						<xsl:with-param name="ConceptTypes" select="$ConceptTypes"/>
						<xsl:with-param name="ConceptTypeRefs" select="$ConceptTypeRefs"/>
						<xsl:with-param name="InformationTypeFormatMappings" select="$InformationTypeFormatMappings"/>
					</xsl:apply-templates>
				</prop:Properties>
			</xsl:for-each>
		</prop:AllProperties>
	</xsl:template>

	<xsl:template match="odt:dataType" mode="ExpandInformationTypeFormats">
		<!-- This will expand the orm data type specifications into odt:* formats
		other than data type. This is unabashedly pulled from the original ORMtoOIAL.xslt
		file, which generated an earlier form of Oial without use of an in-memory extension. -->
		<xsl:param name="ValueTypes"/>
		<xsl:param name="DataTypes"/>
		<xsl:param name="BridgeMappings"/>
		<xsl:variable name="dataTypeId" select="@id"/>
		<xsl:variable name="valueType" select="$ValueTypes[@id=$BridgeMappings[@InformationTypeFormat=$dataTypeId]/@ValueType]"/>
		<xsl:variable name="dataTypeName" select="@name"/>
		<xsl:variable name="ormDataTypeRef" select="$valueType/orm:ConceptualDataType"/>
		<xsl:variable name="conceptualDataType" select="$DataTypes[@id=$ormDataTypeRef/@ref]"/>
		<xsl:variable name="valueRanges" select="$valueType/orm:ValueRestriction/orm:ValueConstraint/orm:ValueRanges/orm:ValueRange"/>
		<xsl:variable name="length" select="$ormDataTypeRef/@Length"/>
		<xsl:variable name="scale" select="$ormDataTypeRef/@Scale"/>

		<xsl:choose>
			<xsl:when test="$conceptualDataType/self::orm:AutoCounterNumericDataType or $conceptualDataType/self::orm:RowIdOtherDataType or $conceptualDataType/self::orm:ObjectIdOtherDataType">
				<odt:identity id="{$dataTypeId}" name="{$dataTypeName}"/>
			</xsl:when>
			<xsl:when test="$conceptualDataType/self::orm:TrueOrFalseLogicalDataType or $conceptualDataType/self::orm:YesOrNoLogicalDataType">
				<odt:boolean id="{$dataTypeId}" name="{$dataTypeName}">
					<!-- BOOLEAN_HACK: Remove the false() on the next line to stop forcing open-world-with-negation. -->
					<xsl:if test="false() and $valueRanges">
						<xsl:attribute name="fixed">
							<!-- This is a boolean, so there will only ever be at most one ValueRange for it, and @MinValue will always match @MaxValue -->
							<xsl:choose>
								<xsl:when test="$valueRanges[translate(@MinValue, 'true', 'TRUE') = 'TRUE' or translate(@MinValue, 'yes', 'YES') = 'YES' or @MinValue = 1]">
									<xsl:value-of select="true()"/>
								</xsl:when>
								<xsl:when test="$valueRanges[translate(@MinValue, 'false', 'FALSE') = 'FALSE' or translate(@MinValue, 'no', 'NO') = 'NO' or @MinValue = 0]">
									<xsl:value-of select="false()"/>
								</xsl:when>
								<xsl:when test="$EnableAssertions">
									<xsl:message terminate="yes">
										<xsl:text>ERROR: Unrecognized boolean constraint value "</xsl:text>
										<xsl:value-of select="@MinValue"/>
										<xsl:text>".</xsl:text>
									</xsl:message>
								</xsl:when>
							</xsl:choose>
						</xsl:attribute>
					</xsl:if>
				</odt:boolean>
			</xsl:when>
			<xsl:when test="$conceptualDataType/self::orm:SignedSmallIntegerNumericDataType or $conceptualDataType/self::orm:UnsignedTinyIntegerNumericDataType or $conceptualDataType/self::orm:UnsignedSmallIntegerNumericDataType or $conceptualDataType/self::orm:SignedIntegerNumericDataType or $conceptualDataType/self::orm:UnsignedIntegerNumericDataType or $conceptualDataType/self::orm:SignedLargeIntegerNumericDataType or $conceptualDataType/self::orm:UnsignedLargeIntegerNumericDataType or $conceptualDataType/self::orm:DecimalNumericDataType or $conceptualDataType/self::orm:MoneyNumericDataType">
				<xsl:variable name="isIntegral" select="$conceptualDataType/self::orm:SignedSmallIntegerNumericDataType or $conceptualDataType/self::orm:UnsignedTinyIntegerNumericDataType or $conceptualDataType/self::orm:UnsignedSmallIntegerNumericDataType or $conceptualDataType/self::orm:SignedIntegerNumericDataType or $conceptualDataType/self::orm:UnsignedIntegerNumericDataType or $conceptualDataType/self::orm:SignedLargeIntegerNumericDataType or $conceptualDataType/self::orm:UnsignedLargeIntegerNumericDataType"/>
				<odt:decimalNumber id="{$dataTypeId}" name="{$dataTypeName}">
					<xsl:if test="$length > 0">
						<xsl:attribute name="totalDigits">
							<xsl:value-of select="$length"/>
						</xsl:attribute>
					</xsl:if>
					<xsl:choose>
						<xsl:when test="$isIntegral">
							<xsl:attribute name="fractionDigits">
								<xsl:value-of select="0"/>
							</xsl:attribute>
						</xsl:when>
						<xsl:when test="$scale > 0">
							<xsl:attribute name="fractionDigits">
								<xsl:value-of select="$scale"/>
							</xsl:attribute>
						</xsl:when>
					</xsl:choose>
					<xsl:if test="$isIntegral">
						<odt:range>
							<xsl:choose>
								<xsl:when test="$conceptualDataType/self::orm:SignedSmallIntegerNumericDataType">
									<odt:lowerBound clusivity="inclusive" value="{$SmallIntegerMinValue}"/>
									<odt:upperBound clusivity="inclusive" value="{$SmallIntegerMaxValue}"/>
								</xsl:when>
								<xsl:when test="$conceptualDataType/self::orm:UnsignedTinyIntegerNumericDataType">
									<odt:lowerBound clusivity="inclusive" value="{$UnsignedTinyIntegerMinValue}"/>
									<odt:upperBound clusivity="inclusive" value="{$UnsignedTinyIntegerMaxValue}"/>
								</xsl:when>
								<xsl:when test="$conceptualDataType/self::orm:UnsignedSmallIntegerNumericDataType">
									<odt:lowerBound clusivity="inclusive" value="{$UnsignedSmallIntegerMinValue}"/>
									<odt:upperBound clusivity="inclusive" value="{$UnsignedSmallIntegerMaxValue}"/>
								</xsl:when>
								<xsl:when test="$conceptualDataType/self::orm:SignedIntegerNumericDataType">
									<odt:lowerBound clusivity="inclusive" value="{$IntegerMinValue}"/>
									<odt:upperBound clusivity="inclusive" value="{$IntegerMaxValue}"/>
								</xsl:when>
								<xsl:when test="$conceptualDataType/self::orm:UnsignedIntegerNumericDataType">
									<odt:lowerBound clusivity="inclusive" value="{$UnsignedIntegerMinValue}"/>
									<odt:upperBound clusivity="inclusive" value="{$UnsignedIntegerMaxValue}"/>
								</xsl:when>
								<xsl:when test="$conceptualDataType/self::orm:SignedLargeIntegerNumericDataType">
									<odt:lowerBound clusivity="inclusive" value="{$LargeIntegerMinValue}"/>
									<odt:upperBound clusivity="inclusive" value="{$LargeIntegerMaxValue}"/>
								</xsl:when>
								<xsl:when test="$conceptualDataType/self::orm:UnsignedLargeIntegerNumericDataType">
									<odt:lowerBound clusivity="inclusive" value="{$UnsignedLargeIntegerMinValue}"/>
									<odt:upperBound clusivity="inclusive" value="{$UnsignedLargeIntegerMaxValue}"/>
								</xsl:when>
							</xsl:choose>
						</odt:range>
					</xsl:if>
					<xsl:apply-templates select="$valueRanges" mode="ProcessOrmValueRange"/>
				</odt:decimalNumber>
			</xsl:when>
			<xsl:when test="$conceptualDataType/self::orm:FloatingPointNumericDataType or $conceptualDataType/self::orm:SinglePrecisionFloatingPointNumericDataType or $conceptualDataType/self::orm:DoublePrecisionFloatingPointNumericDataType">
				<odt:floatingPointNumber id="{$dataTypeId}" name="{$dataTypeName}" precision="{$length}">
					<xsl:choose>
						<xsl:when test="$conceptualDataType/self::orm:SinglePrecisionFloatingPointNumericDataType">
							<xsl:if test="not($length) or ($length = 0) or ($length > $SingleFloatingPointPrecision)">
								<xsl:attribute name="precision">
									<xsl:value-of select="$SingleFloatingPointPrecision"/>
								</xsl:attribute>
							</xsl:if>
						</xsl:when>
						<xsl:when test="$conceptualDataType/self::orm:DoublePrecisionFloatingPointNumericDataType">
							<xsl:if test="not($length) or ($length = 0) or ($length > $DoubleFloatingPointPrecision)">
								<xsl:attribute name="precision">
									<xsl:value-of select="$DoubleFloatingPointPrecision"/>
								</xsl:attribute>
							</xsl:if>
						</xsl:when>
					</xsl:choose>
					<xsl:apply-templates select="$valueRanges" mode="ProcessOrmValueRange"/>
				</odt:floatingPointNumber>
			</xsl:when>
			<xsl:when test="$conceptualDataType/self::orm:FixedLengthTextDataType or $conceptualDataType/self::orm:VariableLengthTextDataType or $conceptualDataType/self::orm:LargeLengthTextDataType">
				<odt:string id="{$dataTypeId}" name="{$dataTypeName}">
					<xsl:if test="$length > 0">
						<xsl:attribute name="maxLength">
							<xsl:value-of select="$length"/>
						</xsl:attribute>
						<xsl:if test="$conceptualDataType/self::orm:FixedLengthTextDataType">
							<xsl:attribute name="minLength">
								<xsl:value-of select="$length"/>
							</xsl:attribute>
						</xsl:if>
					</xsl:if>
					<xsl:for-each select="$valueRanges">
						<xsl:choose>
							<xsl:when test="@MinValue=@MaxValue">
								<odt:enumeration value="{@MinValue}"/>
							</xsl:when>
							<xsl:otherwise>
								<odt:pattern>
									<xsl:attribute name="value">
										<xsl:text>[</xsl:text>
										<xsl:value-of select="@MinValue"/>
										<xsl:text>-</xsl:text>
										<xsl:value-of select="@MaxValue"/>
										<xsl:text>]</xsl:text>
									</xsl:attribute>
								</odt:pattern>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:for-each>
				</odt:string>
			</xsl:when>
			<xsl:when test="$conceptualDataType/self::orm:FixedLengthRawDataDataType or $conceptualDataType/self::orm:VariableLengthRawDataDataType or $conceptualDataType/self::orm:LargeLengthRawDataDataType or $conceptualDataType/self::orm:PictureRawDataDataType or $conceptualDataType/self::orm:OleObjectRawDataDataType">
				<odt:binary id="{$dataTypeId}" name="{$dataTypeName}" maxLength="{$length}">
					<xsl:if test="$length > 0">
						<xsl:attribute name="maxLength">
							<xsl:value-of select="$length"/>
						</xsl:attribute>
						<xsl:if test="$conceptualDataType/self::orm:FixedLengthRawDataDataType">
							<xsl:attribute name="minLength">
								<xsl:value-of select="$length"/>
							</xsl:attribute>
						</xsl:if>
					</xsl:if>
				</odt:binary>
			</xsl:when>
			<xsl:when test="$conceptualDataType/self::orm:AutoTimestampTemporalDataType or $conceptualDataType/self::orm:TimeTemporalDataType or $conceptualDataType/self::orm:DateTemporalDataType or $conceptualDataType/self::orm:DateAndTimeTemporalDataType">
				<odt:temporal id="{$dataTypeId}" name="{$dataTypeName}" temporalPart="both">
					<xsl:choose>
						<xsl:when test="$conceptualDataType/self::orm:TimeTemporalDataType">
							<xsl:attribute name="temporalPart">
								<xsl:text>time</xsl:text>
							</xsl:attribute>
						</xsl:when>
						<xsl:when test="$conceptualDataType/self::orm:DateTemporalDataType">
							<xsl:attribute name="temporalPart">
								<xsl:text>date</xsl:text>
							</xsl:attribute>
						</xsl:when>
					</xsl:choose>
				</odt:temporal>
			</xsl:when>
			<xsl:otherwise>
				<xsl:comment>
					<xsl:text>WARNING: We currently don't support the data type '</xsl:text>
					<xsl:value-of select="local-name($conceptualDataType)"/>
					<xsl:text>' that was chosen for value type "</xsl:text>
					<xsl:value-of select="$dataTypeName"/>
					<xsl:text>"</xsl:text>
				</xsl:comment>
			</xsl:otherwise>
		</xsl:choose>

	</xsl:template>

	<xsl:template match="odt:identity" mode="GenerateInformationTypeFormatMapping">
		<prop:FormatMapping name="{@name}" id="{@id}" canBeNull="false" isIdentity="true"/>
	</xsl:template>
	<xsl:template match="odt:boolean" mode="GenerateInformationTypeFormatMapping">
		<prop:FormatMapping name="{@name}" id="{@id}" canBeNull="false">
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
		<prop:FormatMapping name="{@name}" id="{@id}" canBeNull="false">
			<xsl:choose>
				<xsl:when test="not(@fractionDigits) or @fractionDigits=0">
					<!-- Integral type -->
					<xsl:variable name="dataTypeNameFragment">
						<xsl:variable name="integralRange" select="odt:range"/>
						<xsl:choose>
							<xsl:when test="$integralRange">
								<xsl:variable name="lower" select="number($integralRange/odt:lowerBound/@value)"/>
								<xsl:variable name="upper" select="number($integralRange/odt:upperBound/@value)"/>
								<xsl:choose>
									<xsl:when test="$lower&lt;0">
										<xsl:choose>
											<xsl:when test="$lower&gt;=$SmallIntegerMinValue and $upper&lt;=$SmallIntegerMaxValue">
												<xsl:text>i2</xsl:text>
											</xsl:when>
											<xsl:when test="$lower&gt;=$IntegerMinValue and $upper&lt;=$IntegerMaxValue">
												<xsl:text>i4</xsl:text>
											</xsl:when>
											<xsl:when test="$lower&gt;=$LargeIntegerMinValue and $upper&lt;=$LargeIntegerMaxValue">
												<xsl:text>i8</xsl:text>
											</xsl:when>
											<xsl:otherwise>
												<xsl:text>decimal</xsl:text>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:when test="$upper&lt;=$UnsignedTinyIntegerMaxValue">
										<xsl:text>u1</xsl:text>
									</xsl:when>
									<xsl:when test="$upper&lt;=$UnsignedSmallIntegerMaxValue">
										<xsl:text>u2</xsl:text>
									</xsl:when>
									<xsl:when test="$upper&lt;=$UnsignedIntegerMaxValue">
										<xsl:text>u4</xsl:text>
									</xsl:when>
									<xsl:when test="$upper&lt;=$UnsignedLargeIntegerMaxValue">
										<xsl:text>u8</xsl:text>
									</xsl:when>
									<xsl:otherwise>
										<xsl:text>decimal</xsl:text>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>i4</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<!-- TODO: Process @totalDigits and all child elements -->
					<prop:DataType dataTypeName=".{$dataTypeNameFragment}"/>
				</xsl:when>
				<xsl:otherwise>
					<!-- Fraction type -->
					<!-- TODO: Process @totalDigits, @fractionDigits, and all child elements -->
					<prop:DataType dataTypeName=".decimal"/>
				</xsl:otherwise>
			</xsl:choose>
		</prop:FormatMapping>
	</xsl:template>
	<xsl:template match="odt:temporal" mode="GenerateInformationTypeFormatMapping">
		<prop:FormatMapping name="{@name}" id="{@id}" canBeNull="false">
			<prop:DataType dataTypeName=".date"/>
		</prop:FormatMapping>
	</xsl:template>
	<xsl:template match="odt:floatingPointNumber" mode="GenerateInformationTypeFormatMapping">
		<prop:FormatMapping name="{@name}" id="{@id}" canBeNull="false">
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
		<prop:FormatMapping name="{@name}" id="{@id}" canBeNull="true">
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
		<prop:FormatMapping name="{@name}" id="{@id}" canBeNull="true">
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

	<xsl:template match="oial:conceptType" mode="GenerateProperties">
		<xsl:param name="NormalizedConceptTypeNames"/>
		<xsl:param name="NormalizedConceptTypeChildNames"/>
		<xsl:param name="ConceptTypes"/>
		<xsl:param name="ConceptTypeRefs"/>
		<xsl:param name="InformationTypeFormatMappings"/>
		<xsl:variable name="thisClassName" select="string($NormalizedConceptTypeNames[@id=current()/@id]/@name)"/>
		<xsl:variable name="children" select="oial:children/child::*"/>
		<xsl:variable name="identityFormatRefIds" select="$InformationTypeFormatMappings[@isIdentity='true']/@id"/>
		<xsl:variable name="uniquenessConstraints" select="oial:uniquenessConstraints/oial:uniquenessConstraint"/>
		<xsl:variable name="singleValueUniquenessConstraints" select="$uniquenessConstraints[count(oial:uniquenessChild)=1]"/>

		<!--Process directly contained oial:relatedConceptType and oial:informationType elements,
			as well as oial:assimilatedConceptType elements. Also process all oial:relatedConceptType
			and oial:assimilatedConceptType elements that are targetted at us.-->

		<xsl:for-each select="$children[self::oial:informationType]">
			<xsl:variable name="informationTypeFormatMapping" select="$InformationTypeFormatMappings[@id=current()/@ref]"/>
			<xsl:variable name="isMandatory" select="boolean(@isMandatory[.='true' or .=1])"/>
			<xsl:variable name="normalizedNameElement" select="$NormalizedConceptTypeChildNames[@id=current()/@id]"/>
			<xsl:variable name="normalizedName" select="string($normalizedNameElement/@name)"/>
			<xsl:variable name="normalizedParamName" select="string($normalizedNameElement/@paramName)"/>
			<xsl:choose>
				<xsl:when test="@ref=$identityFormatRefIds">
					<prop:IdentityField name="{$normalizedName}" paramName="{$normalizedParamName}"  childId="{@id}">
						<xsl:copy-of select="$informationTypeFormatMapping/prop:DataType"/>
					</prop:IdentityField>
				</xsl:when>
				<xsl:otherwise>
					<prop:Property name="{$normalizedName}" paramName="{$normalizedParamName}" childId="{@id}" mandatory="false" isUnique="{boolean($singleValueUniquenessConstraints/oial:uniquenessChild/@ref=@id)}" canBeNull="{not($isMandatory) or $informationTypeFormatMapping/@canBeNull='true'}" isCollection="false" isCustomType="false">
						<xsl:if test="$isMandatory">
							<xsl:attribute name="mandatory">
								<xsl:text>alethic</xsl:text>
							</xsl:attribute>
						</xsl:if>
						<xsl:choose>
							<xsl:when test="not($isMandatory) and $informationTypeFormatMapping/@canBeNull='false'">
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

		<xsl:for-each select="$children[self::oial:relatedConceptType]">
			<xsl:variable name="normalizedNameElement" select="$NormalizedConceptTypeChildNames[@id=current()/@id]"/>
			<prop:Property name="{$normalizedNameElement/@oppositeName}" paramName="{$normalizedNameElement/@oppositeParamName}" childId="{@id}" mandatory="false" isUnique="{boolean($singleValueUniquenessConstraints/oial:uniquenessChild/@ref=@id)}" isCollection="false" isCustomType="true" canBeNull="true" oppositeName="{$normalizedNameElement/@name}" oppositeParamName="{$normalizedNameElement/@paramName}">
				<xsl:if test="@isMandatory[.='true' or .=1]">
					<xsl:attribute name="mandatory">
						<xsl:text>alethic</xsl:text>
					</xsl:attribute>
				</xsl:if>
				<prop:DataType dataTypeName="{$NormalizedConceptTypeNames[@id=current()/@ref]/@name}"/>
			</prop:Property>
		</xsl:for-each>
		<xsl:for-each select="$children[self::oial:assimilatedConceptType]">
			<xsl:variable name="normalizedNameElement" select="$NormalizedConceptTypeChildNames[@id=current()/@id]"/>
			<prop:Property name="{$normalizedNameElement/@oppositeName}" paramName="{$normalizedNameElement/@oppositeParamName}"  childId="{@id}" mandatory="false" isUnique="true" isCollection="false" isCustomType="true" canBeNull="true" oppositeName="{$normalizedNameElement/@name}" oppositeParamName="{$normalizedNameElement/@paramName}">
				<xsl:if test="@isMandatory[.='true' or .=1]">
					<xsl:attribute name="mandatory">
						<xsl:text>alethic</xsl:text>
					</xsl:attribute>
				</xsl:if>
				<prop:DataType dataTypeName="{$NormalizedConceptTypeNames[@id=current()/@ref]/@name}"/>
			</prop:Property>
		</xsl:for-each>
		<xsl:for-each select="$ConceptTypeRefs[self::oial:assimilatedConceptType[@ref=current()/@id]]">
			<xsl:variable name="normalizedNameElement" select="$NormalizedConceptTypeChildNames[@id=current()/@id]"/>
			<prop:Property name="{$normalizedNameElement/@name}" paramName="{$normalizedNameElement/@paramName}" reverseChildId="{@id}"  mandatory="alethic" isUnique="true" isCollection="false" isCustomType="true" canBeNull="true" oppositeName="{$normalizedNameElement/@oppositeName}" oppositeParamName="{$normalizedNameElement/@oppositeParamName}">
				<prop:DataType dataTypeName="{$NormalizedConceptTypeNames[@id=current()/../../@id]/@name}"/>
			</prop:Property>
		</xsl:for-each>
		<xsl:for-each select="$ConceptTypeRefs[self::oial:relatedConceptType/@ref=current()/@id]">
			<xsl:variable name="normalizedNameElement" select="$NormalizedConceptTypeChildNames[@id=current()/@id]"/>
			<xsl:variable name="isCollection" select="not(boolean(../../oial:uniquenessConstraints/oial:uniquenessConstraint[oial:uniquenessChild[@ref=current()/@id]][count(oial:uniquenessChild)=1]))"/>
			<xsl:variable name="normalizedPropertyName">
				<xsl:value-of select="concat($normalizedNameElement/@name,'Via',$normalizedNameElement/@oppositeName)"/>
				<xsl:if test="$isCollection">
					<xsl:text>Collection</xsl:text>
				</xsl:if>
			</xsl:variable>
			<xsl:variable name="normalizedPropertyParamName">
				<xsl:value-of select="concat($normalizedNameElement/@paramName,'Via',$normalizedNameElement/@oppositeName)"/>
				<xsl:if test="$isCollection">
					<xsl:text>Collection</xsl:text>
				</xsl:if>
			</xsl:variable>
			<prop:Property name="{$normalizedPropertyName}" paramName="{$normalizedPropertyParamName}" reverseChildId="{@id}" mandatory="false" isUnique="true" isCollection="{$isCollection}" isCustomType="true" canBeNull="true" oppositeName="{$normalizedNameElement/@oppositeName}" oppositeParamName="{$normalizedNameElement/@oppositeParamName}">
				<xsl:variable name="parentConceptTypeName" select="$NormalizedConceptTypeNames[@id=current()/../../@id]/@name"/>
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
	<msxsl:script implements-prefix="fn" language="CSharp">
		<![CDATA[
		private static System.Text.RegularExpressions.Regex _splitOnUpperAndNumberRegex;
		private static System.Text.RegularExpressions.Regex _lowerFirstRegex;
		private static System.Text.RegularExpressions.Regex _upperFirstRegex;
		private static readonly char[] nameDelimiterArray = new char[] { ' ', '-' };
		private const string removeSeparatorCharacters = ":_.";
		public static string normalizeName(string name, bool leadLower)
		{
			System.Text.RegularExpressions.Regex splitNameRegex;
			System.Text.RegularExpressions.Regex lowerFirstRegex = null;
			System.Text.RegularExpressions.Regex upperFirstRegex = null;
			if (null == (splitNameRegex = _splitOnUpperAndNumberRegex))
			{
				System.Threading.Interlocked.CompareExchange<Regex>(
					ref _splitOnUpperAndNumberRegex,
					new Regex(
						@"(?n)\G(?(\p{P}|\p{S})(?<PunctuationOrSymbol>(\p{P}|\p{S}))|(?(\p{Nd})((?<Numeric>\p{Nd}+)(?((\p{Ll})+\p{Nd})|\p{Ll}+)?)|(?(\p{Lu})(\p{Lu}(?(\P{Lu})((?!(\p{Nd}|\p{P}|\p{S}))\P{Lu})*|(?<TrailingUpper>((?!\p{Lu}\p{Ll})\p{Lu})*)))|((?!(\p{Nd}|\p{P}|\p{S}))\P{Lu})+)))",
						RegexOptions.Compiled),
					null);
				splitNameRegex = _splitOnUpperAndNumberRegex;
			}
			string[] individualNames = name.Split(nameDelimiterArray, StringSplitOptions.RemoveEmptyEntries);
			System.Text.StringBuilder builder = new System.Text.StringBuilder();
			for (int i = 0; i < individualNames.Length; ++i)
			{
				Match match = splitNameRegex.Match(individualNames[i]);
				while (match.Success)
				{
					string matchText = match.Value;
					GroupCollection groups = match.Groups;
					if (groups["TrailingUpper"].Success)
					{
						builder.Append(matchText);
						leadLower = false;
					}
					else if (groups["PunctuationOrSymbol"].Success)
					{
						if (!removeSeparatorCharacters.Contains(matchText))
						{
							builder.Append(matchText);
							// Leave the upper/lower state alone until we see something other than a symbol
						}
					}
					else if (leadLower)
					{
						leadLower = false;
						if (null == upperFirstRegex &&
							null == (upperFirstRegex = _upperFirstRegex))
						{
							System.Threading.Interlocked.CompareExchange<Regex>(
								ref _upperFirstRegex,
								new Regex(
									@"^\p{Lu}",
									RegexOptions.Compiled),
								null);
							upperFirstRegex = _upperFirstRegex;
						}
						builder.Append(upperFirstRegex.Replace(
							matchText,
							delegate(Match m)
							{
								return m.Value.ToLower();
							}));
					}
					else
					{
						if (null == lowerFirstRegex &&
							null == (lowerFirstRegex = _lowerFirstRegex))
						{
							System.Threading.Interlocked.CompareExchange<Regex>(
								ref _lowerFirstRegex,
								new Regex(
									@"^\p{Ll}",
									RegexOptions.Compiled),
								null);
							lowerFirstRegex = _lowerFirstRegex;
						}
						builder.Append(lowerFirstRegex.Replace(
							matchText,
							delegate(Match m)
							{
								return m.Value.ToUpper();
							}));
					}
					match = match.NextMatch();
				}
			}
			return builder.ToString();
		}
		]]>
	</msxsl:script>
</xsl:stylesheet>
