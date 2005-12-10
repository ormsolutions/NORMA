<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:oil="http://schemas.orm.net/OIAL"
	xmlns:odt="http://schemas.orm.net/ORMDataTypes"
	xmlns:xs="http://www.w3.org/2001/XMLSchema"
	extension-element-prefixes="msxsl"
	exclude-result-prefixes="oil odt">

	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:strip-space elements="*"/>

	<xsl:template match="oil:model">
		<xs:schema id="{@name}" elementFormDefault="qualified" targetNamespace="http://schemas.neumont.edu/ORM/CodeGeneratedSchema/{@name}" xmlns:oxs="http://schemas.neumont.edu/ORM/CodeGeneratedSchema/{@name}" version="1.0">
			<xsl:variable name="informationTypeFormatMappingsFragment">
				<xsl:apply-templates select="oil:informationTypeFormats/child::*" mode="GenerateMapping"/>
			</xsl:variable>
			<xsl:variable name="informationTypeFormatMappings" select="msxsl:node-set($informationTypeFormatMappingsFragment)/child::*"/>
			<xsl:apply-templates select="oil:informationTypeFormats/child::*[@name=$informationTypeFormatMappings[starts-with(@target,'oxs')]/@name]" mode="GenerateSimpleType"/>
			<xsl:apply-templates select="oil:conceptType">
				<xsl:with-param name="informationTypeFormatMappings" select="$informationTypeFormatMappings"/>
			</xsl:apply-templates>
		</xs:schema>
	</xsl:template>

	<xsl:template match="odt:identity" mode="GenerateMapping">
		<FormatMapping name="{@name}" target="xs:integer"/>
	</xsl:template>
	<xsl:template match="odt:boolean" mode="GenerateMapping">
		<FormatMapping name="{@name}">
			<xsl:attribute name="target">
				<xsl:choose>
					<xsl:when test="@fixed='true'">
						<xsl:value-of select="'oxs:true'"/>
					</xsl:when>
					<xsl:when test="@fixed='false'">
						<xsl:value-of select="'oxs:false'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'xs:boolean'"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</FormatMapping>
	</xsl:template>
	<xsl:template match="odt:decimalNumber" mode="GenerateMapping">
		<FormatMapping name="{@name}">
			<xsl:attribute name="target">
				<xsl:choose>
					<!-- TODO: Optimize this so that we map to smaller integer types when possible. -->
					<xsl:when test="oil:enumeration or oil:range or @totalDigits or not(@fractionDigits=0)">
						<xsl:value-of select="concat('oxs:', @name)"/>
					</xsl:when>
					<xsl:when test="@fractionDigits = 0">
						<xsl:value-of select="'xs:integer'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'xs:decimal'"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</FormatMapping>
	</xsl:template>
	<xsl:template match="odt:floatingPointNumber" mode="GenerateMapping">
		<FormatMapping name="{@name}">
			<xsl:attribute name="target">
				<xsl:choose>
					<xsl:when test="child::*">
						<xsl:value-of select="concat('oxs:', @name)"/>
					</xsl:when>
					<xsl:when test="@precision &lt; 25 or @precision='single'">
						<xsl:value-of select="'xs:float'"/>
					</xsl:when>
					<xsl:when test="@precision &lt; 54 or @precision='double'">
						<xsl:value-of select="'xs:double'"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:message terminate="yes">
							<xsl:text>Sorry, XML Schema doesn't support floating point data types above double-precision.</xsl:text>
						</xsl:message>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</FormatMapping>
	</xsl:template>
	<xsl:template match="odt:string" mode="GenerateMapping">
		<FormatMapping name="{@name}">
			<xsl:attribute name="target">
				<xsl:choose>
					<xsl:when test="@minLength or @maxLength or child::*">
						<xsl:value-of select="concat('oxs:', @name)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'xs:string'"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</FormatMapping>
	</xsl:template>
	<xsl:template match="odt:binary" mode="GenerateMapping">
		<FormatMapping name="{@name}">
			<xsl:attribute name="target">
				<xsl:choose>
					<xsl:when test="@minLength or @maxLength">
						<xsl:value-of select="concat('oxs:', @name)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'xs:hexBinary'"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</FormatMapping>
	</xsl:template>


	<xsl:template match="oil:informationTypeFormats/child::*" mode="GenerateSimpleType">
		<!-- informationTypeFormats that don't have a one-to-one mapping to predefined data types are made into xs:simpleTypes -->
		<xs:simpleType name="{@name}">
			<xsl:apply-templates select="." mode="GenerateSimpleTypeRestriction"/>
		</xs:simpleType>
	</xsl:template>

	<xsl:template match="odt:boolean" mode="GenerateSimpleTypeRestriction">
		<xs:restriction base="xs:boolean">
			<xs:enumeration value="{@fixed}"/>
		</xs:restriction>
	</xsl:template>
	<xsl:template match="odt:decimalNumber" mode="GenerateSimpleTypeRestriction">
		<!-- TODO: Need to finish this. -->
		<xs:restriction base="xs:decimal">
			<xs:fractionDigits value="{@fractionDigits}"/>
			<xsl:if test="@totalDigits">
				<xs:totalDigits value="{@totalDigits}"/>
			</xsl:if>
		</xs:restriction>
		<!-- Stuff relating to having multiple Ranges or enumerations-->
	</xsl:template>
	<xsl:template match="odt:floatingPointNumber" mode="GenerateSimpleTypeRestriction">
		<!-- TODO: Need to finish this. -->
		<!-- Stuff relating to having multiple Ranges or enumerations-->
		<xs:restriction base="xs:float">
			<xsl:if test="@precision">

			</xsl:if>
		</xs:restriction>
	</xsl:template>
	<xsl:template match="odt:string" mode="GenerateSimpleTypeRestriction">
		<xs:restriction base="xs:string">
			<xsl:if test="@minLength">
				<xs:minLength value="{@minLength}"/>
			</xsl:if>
			<xsl:if test="@maxLength">
				<xs:minLength value="{@maxLength}"/>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="odt:enumeration and not(odt:pattern)">
					<xsl:for-each select="odt:enumeration">
						<xs:enumeration value="{@value}"/>
					</xsl:for-each>
				</xsl:when>
				<xsl:when test="odt:pattern">
					<xs:pattern>
						<xsl:attribute name="value">
							<xsl:text>(</xsl:text>
							<xsl:for-each select="child::*">
								<xsl:text>(</xsl:text>
								<xsl:value-of select="@value"/>
								<xsl:text>)</xsl:text>
								<xsl:if test="not(position()=last())">
									<xsl:text>|</xsl:text>
								</xsl:if>
							</xsl:for-each>
							<xsl:text>)</xsl:text>
						</xsl:attribute>
					</xs:pattern>
				</xsl:when>
			</xsl:choose>
		</xs:restriction>
	</xsl:template>
	<xsl:template match="odt:binary" mode="GenerateSimpleTypeRestriction">
		<xs:restriction base="xs:hexBinary">
			<xsl:if test="@minLength">
				<xs:minLength value="{@minLength}"/>
			</xsl:if>
			<xsl:if test="@maxLength">
				<xs:minLength value="{@maxLength}"/>
			</xsl:if>
		</xs:restriction>
	</xsl:template>
	<!--<xsl:template match="" mode="GenerateSimpleTypeRestriction">
	FALLBACK
	</xsl:template>-->

	<xsl:template match="oil:conceptType">
		<xsl:param name="informationTypeFormatMappings"/>
		<xsl:variable name="preferredIdentifier" select="oil:informationType[oil:singleRoleUniquenessConstraint/@isPrimary='true']/@name"/>
		<!--Complex type definitions for each major Object Type-->
		<xs:complexType name="{@name}">
			<!--<xs:annotation>
				<xs:documentation></xs:documentation>
			</xs:annotation>-->
			<!-- UNDONE : gen nested Complex types -->
			<xs:sequence>
				<!--<xs:annotation>
					<xs:documentation></xs:documentation>
				</xs:annotation>-->
				<xsl:for-each select="oil:conceptType">
					<xs:element name="{@name}" type="{concat('oxs:','UNDONE')}">
					</xs:element>
				</xsl:for-each>
				<!-- do somthing with the isMandatory=true oil:informationTypes-->

				<xsl:apply-templates select="oil:equalityConstraint"/>
				<xsl:apply-templates select="oil:disjunctiveMandatoryConstraint"/>
				<xsl:apply-templates select="oil:exclusionConstraint"/>
				<!--<xsl:apply-templates select="oil:roleSequenceFrequencyConstraint"/>
				<xsl:apply-templates select="oil:ringConstraint"/>
				<xsl:apply-templates select="oil:subsetConstraint"/>-->
			</xs:sequence>

			<!--<xs:attribute name="{$preferredIdentifier}" type="{concat('oxs:',$preferredIdentifier)}" use="required"/>-->

			<xsl:for-each select="oil:informationType">
				<xsl:if test="@mandatory ='alethic'">
					<xs:attribute name="{@name}" type="{$informationTypeFormatMappings[@name=current()/@formatRef]/@target}">
						<xsl:attribute name="use">
							<xsl:choose>
								<xsl:when test="@mandatory = 'alethic'">
									<xsl:value-of select="'required'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'optional'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</xs:attribute>
				</xsl:if>
			</xsl:for-each>

		</xs:complexType>
	</xsl:template>
	<xsl:template match="oil:disjunctiveMandatoryConstraint">
		<xs:choice minOccurs="1">
			<xs:annotation>
				<xs:documentation>Disjunctive Mandatory Constraint(Inclusive OR)</xs:documentation>
			</xs:annotation>
			<xsl:for-each select="oil:roleSequence/oil:typeRef">
				<xsl:variable name="elementName" select="@informationTypeTarget"/>
				<xsl:variable name="elementType" select="concat('oxs:',$elementName)"/>
				<xs:element name="{$elementName}" type="{$elementType}">
				</xs:element>
			</xsl:for-each>
		</xs:choice>
	</xsl:template>
	<xsl:template match="oil:equalityConstraint">
		<xs:sequence minOccurs="0">
			<xs:annotation>
				<xs:documentation>Equality Constraint(Both must exist if one exists)</xs:documentation>
			</xs:annotation>
			<xsl:for-each select="oil:roleSequence">
				<xsl:variable name="elementName" select="oil:typeRef/@informationTypeTarget"/>
				<xsl:variable name="elementType" select="concat('oxs:',$elementName)"/>
				<xs:element name="{$elementName}" type="{$elementType}">
				</xs:element>
			</xsl:for-each>
		</xs:sequence>
	</xsl:template>
	<xsl:template match="oil:roleSequenceFrequencyConstraint">
		
	</xsl:template>
	<xsl:template match="oil:exclusionConstraint">
		<xs:choice minOccurs="0" maxOccurs="1">
			<xs:annotation>
				<xs:documentation>Exclusion Constraint(XOR)</xs:documentation>
			</xs:annotation>
			<xsl:for-each select="oil:roleSequence">
				<xsl:variable name="elementName" select="oil:typeRef/@informationTypeTarget"/>
				<xsl:variable name="elementType" select="concat('oxs:',$elementName)"/>
				<xs:element name="{$elementName}" type="{$elementType}">
				</xs:element>
			</xsl:for-each>
		</xs:choice>
	</xsl:template>
	<xsl:template match="oil:ringConstraint">
		<!--UNDONE-->
		<xsl:choose>
			<xsl:when test="@type = 'irreflexive'">
				
			</xsl:when>
			<xsl:when test="@type = 'acyclic'">

			</xsl:when>
			<xsl:when test="@type = 'intransitive'">

			</xsl:when>
			<xsl:when test="@type = 'acyclic intransitive'">

			</xsl:when>
			<xsl:when test="@type = 'symmetric'">

			</xsl:when>
			<xsl:when test="@type = 'asymmetric'">

			</xsl:when>
			<xsl:when test="@type = 'anti-symmetric'">

			</xsl:when>
			<xsl:when test="@type = 'pro-symmetric'">

			</xsl:when>
			<xsl:when test="@type = 'conservative-symmetric'">

			</xsl:when>
			<xsl:when test="@type = 'liberal-symmetric'">

			</xsl:when>
			<xsl:when test="@type = ''">

			</xsl:when>
			<xsl:when test="@type = ''">

			</xsl:when>
			<xsl:when test="@type = ''">

			</xsl:when>
			<xsl:when test="@type = ''">

			</xsl:when>
			
		</xsl:choose>
	</xsl:template>

	
	<xsl:template match="oil:valueConstraint">
		<xsl:variable name="rangeCount" select="count(oil:range)"/>
		<xsl:variable name="valueCount" select="count(oil:value)"/>
		<xsl:variable name="value" select="oil:value/@value"/>
		<!-- Check all value constraints for more multiple ranges or values-->
		<xsl:if test="$rangeCount = 0 and $valueCount > 0">
			<xs:restriction>
				<xsl:attribute name="base">
					<xsl:call-template name="GenerateXsdDataType"/>
				</xsl:attribute>
				<!-- check for length -->
				<xs:enumeration value="{$value}"/>
			</xs:restriction>
		</xsl:if>
		<xsl:if test="$rangeCount > 1 and $valueCount = 0">
			<xs:union>
				<xsl:for-each select="oil:range">
					<xs:simpleType>
						<xs:restriction>
							<xsl:attribute name="base">
								<xsl:call-template name="GenerateXsdDataType"/>
							</xsl:attribute>
							<!-- check for length -->
							<xsl:apply-templates select="."/>
						</xs:restriction>
					</xs:simpleType>
				</xsl:for-each>
			</xs:union>
		</xsl:if>
		<xsl:if test="$rangeCount != 0 and $valueCount != 0">
			<xs:union>
				<xsl:for-each select="oil:range">
					<xs:simpleType>
						<xs:restriction>
							<xsl:attribute name="base">
								<xsl:call-template name="GenerateXsdDataType"/>
							</xsl:attribute>
							<!-- check for length -->
							<xsl:apply-templates select="oil:range"/>
						</xs:restriction>
					</xs:simpleType>
				</xsl:for-each>
				<xsl:for-each select="oil:value">
					<xs:simpleType>
						<xs:restriction>
							<xsl:attribute name="base">
								<xsl:call-template name="GenerateXsdDataType"/>
							</xsl:attribute>
							<!-- check for length -->
							<xs:enumeration value="{$value}"/>
						</xs:restriction>
					</xs:simpleType>
				</xsl:for-each>
			</xs:union>
		</xsl:if>
		<xsl:if test="$rangeCount = 1 and $valueCount = 0">
			<xs:restriction>
				<xsl:attribute name="base">
					<xsl:call-template name="GenerateXsdDataType"/>
				</xsl:attribute>
				<!-- check for length -->
				<xsl:apply-templates select="oil:range"/>
			</xs:restriction>
		</xsl:if>
	</xsl:template>
	<!-- handels clusivity for range constraints -->
	<xsl:template match="oil:range">
		<xsl:variable name="lowerClusivity" select="./oil:lowerBound/@clusivity"/>
		<xsl:variable name="upperClusivity" select="./oil:upperBound/@clusivity"/>
		<xsl:variable name="lowerValue" select="./oil:lowerBound/@value"/>
		<xsl:variable name="upperValue" select="./oil:upperBound/@value"/>
		<xsl:if test="oil:lowerBound">
			<xsl:if test="$lowerClusivity = 'inclusive'">
				<xs:minInclusive value="{$lowerValue}"/>
			</xsl:if>
			<xsl:if test="$lowerClusivity = 'exclusive'">
				<xs:minExclusive value="{$lowerValue}"/>
			</xsl:if>
		</xsl:if>
		<xsl:if test="oil:upperBound">
			<xsl:if test="$upperClusivity = 'inclusive'">
				<xs:maxInclusive value="{$upperValue}"/>
			</xsl:if>
			<xsl:if test="$upperClusivity = 'exclusive'">
				<xs:maxExclusive value="{$upperValue}"/>
			</xsl:if>
		</xsl:if>
	</xsl:template>
	<xsl:template name="GenerateXsdDataType">
		<xsl:param name="Prefix" select="'xs'"/>
		<xsl:param name="ValueType" select="."/>
		<xsl:variable name="dataType" select="$ValueType/@dataType"/>
		<xsl:choose>
			<xsl:when test="$dataType='FixedLengthTextDataType'">
				<!--<xsl:if test="@length and @length!='0'">
							<xs:length value="{@length}"/>
						</xsl:if>-->
				<xsl:value-of select="concat($Prefix,':string')"/>
			</xsl:when>
			<xsl:when test="$dataType='VariableLengthTextDataType'">
				<xsl:value-of select="concat($Prefix,':string')"/>
			</xsl:when>
			<xsl:when test="$dataType='LargeLengthTextDataType'">
				<xsl:value-of select="concat($Prefix,':string')"/>
			</xsl:when>
			<xsl:when test="$dataType='SignedIntegerNumericDataType'">
				<xsl:value-of select="concat($Prefix,':integer')"/>
			</xsl:when>
			<xsl:when test="$dataType='AutoCounterNumericDataType'">
				<xsl:value-of select="concat($Prefix,':')"/>
			</xsl:when>
			<xsl:when test="$dataType='UnsignedIntegerNumericDataType'">
				<xsl:value-of select="concat($Prefix,':unsignedInt')"/>
			</xsl:when>
			<xsl:when test="$dataType='FloatingPointNumericDataType'">
				<xsl:value-of select="concat($Prefix,':float')"/>
			</xsl:when>
			<xsl:when test="$dataType='DecimalNumericDataType'">
				<xsl:value-of select="concat($Prefix,':decimal')"/>
			</xsl:when>
			<xsl:when test="$dataType='MoneyNumericDataType'">
				<xsl:value-of select="concat($Prefix,':decimal')"/>
			</xsl:when>
			<xsl:when test="$dataType='FixedLengthRawDataDataType'">
				<xsl:value-of select="concat($Prefix,':')"/>
			</xsl:when>
			<xsl:when test="$dataType='VariableLengthRawDataDataType'">
				<xsl:value-of select="concat($Prefix,':?')"/>
			</xsl:when>
			<xsl:when test="$dataType='LargeLengthRawDataDataType'">
				<xsl:value-of select="concat($Prefix,':?')"/>
			</xsl:when>
			<xsl:when test="$dataType='PictureRawDataDataType'">
				<xsl:value-of select="concat($Prefix,':?')"/>
			</xsl:when>
			<xsl:when test="$dataType='OleObjectRawDataDataType'">
				<xsl:value-of select="concat($Prefix,':?')"/>
			</xsl:when>
			<xsl:when test="$dataType='AutoTimestampTemporalDataType'">
				<xsl:value-of select="concat($Prefix,':?')"/>
			</xsl:when>
			<xsl:when test="$dataType='TimeTemporalDataType'">
				<xsl:value-of select="concat($Prefix,':time')"/>
			</xsl:when>
			<xsl:when test="$dataType='DateTemporalDataType'">
				<xsl:value-of select="concat($Prefix,':date')"/>
			</xsl:when>
			<xsl:when test="$dataType='DateAndTimeTemporalDataType'">
				<xsl:value-of select="concat($Prefix,':dateTime')"/>
			</xsl:when>
			<xsl:when test="$dataType='TrueOrFalseLogicalDataType'">
				<xsl:value-of select="concat($Prefix,':boolean')"/>
			</xsl:when>
			<xsl:when test="$dataType='YesOrNoLogicalDataType'">
				<xsl:value-of select="concat($Prefix,':boolean')"/>
			</xsl:when>
			<xsl:when test="$dataType='RowIdOtherDataType'">
				<xsl:value-of select="'{$Prefix}:'"/>
			</xsl:when>
			<xsl:when test="$dataType='ObjectIdOtherDataType'">
				<xsl:value-of select="'{$Prefix}:'"/>
			</xsl:when>
			<xsl:otherwise>
				<!--<xsl:message terminate="yes">Could not map DataType.</xsl:message>-->
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!--<xsl:template>
		<xsl:for-each select="$baseType">
			<xsl:copy>
				<xsl:for-each select="*">
					<xsl:copy>
						<xsl:copy-of select="@*|*">
							
						</xsl:copy-of>
					</xsl:copy>
				</xsl:for-each>
			</xsl:copy>
		</xsl:for-each>
	</xsl:template>-->
</xsl:stylesheet>