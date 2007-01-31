<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
				xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:edm="http://schemas.microsoft.com/ado/2006/04/edm"
				xmlns:oil="http://schemas.orm.net/OIAL"
				xmlns:odt="http://schemas.orm.net/ORMDataTypes"
				xmlns:exsl="http://exslt.org/common"
				exclude-result-prefixes="odt oil">
	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:strip-space elements="*"/>

	<xsl:template match="/">
		<xsl:apply-templates select="*"/>
	</xsl:template>

	<xsl:template match="oil:model">
		<edm:Schema Namespace="{@name}" Alias="{@name}Alias" xmlns:edm="http://schemas.microsoft.com/ado/2006/04/edm" xmlns:a="http://schemas.microsoft.com/ado/2006/04/codegeneration">
			<xsl:apply-templates select="oil:informationTypeFormats"/>
			<xsl:apply-templates mode="GenerateEntityTypes" select="//oil:conceptType[oil:informationType/oil:singleRoleUniquenessConstraint/@isPreferred='true']"/>
			<xsl:apply-templates mode="GenerateAssociation" select="//oil:conceptType[oil:roleSequenceUniquenessConstraint/@isPreferred='true']"/>
			<!--<xsl:apply-templates mode="GenerateContainment" select="//oil:conceptTypeRef"/>-->
			<xsl:apply-templates mode="GenerateAssociation" select="//oil:conceptTypeRef"/>
		</edm:Schema>
	</xsl:template>

	<xsl:template match="oil:informationTypeFormats">
		<xsl:apply-templates mode="GenerationEnumeration" select="child::*[odt:enumeration]"/>
	</xsl:template>

	<xsl:template match="odt:string" mode="GenerationEnumeration">
		<edm:EnumerationType Name="{@name}">
			<xsl:for-each select="odt:enumeration">
				<edm:EnumerationMember Name="{@value}"/>
			</xsl:for-each>
		</edm:EnumerationType>
	</xsl:template>

	<xsl:template match="oil:conceptType" mode="GenerateEntityTypes">
		<edm:EntityType Name="{@name}">
			<xsl:choose>
				<xsl:when test="oil:informationType/oil:singleRoleUniquenessConstraint/@isPreferred='true'">
					<xsl:attribute name="Key">
						<xsl:value-of select="oil:informationType[oil:singleRoleUniquenessConstraint/@isPreferred='true']/@name"/>
					</xsl:attribute>
				</xsl:when>
			</xsl:choose>
			<xsl:apply-templates mode="GenerateProperties" select="oil:informationType"/>

		</edm:EntityType>
	</xsl:template>




	<xsl:template match="oil:informationType" mode="GenerateProperties">
		<xsl:apply-templates mode="GenerateProperty" select="/oil:model/oil:informationTypeFormats/child::*[@name = current()/@name]">
			<xsl:with-param name="informationTypeName" select="@name"/>
			<xsl:with-param name="isMandatory" select="@mandatory"/>
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="odt:string" mode="GenerateProperty">
		<xsl:param name="informationTypeName"/>
		<xsl:param name="isMandatory"/>
		<edm:Property Name="{$informationTypeName}">
			<xsl:attribute name="Type">
				<xsl:value-of select="'String'"/>
			</xsl:attribute>
			<xsl:variable name="maxLength" select="@maxLength"/>
			<xsl:variable name="minLength" select="@minLength"/>

			<xsl:if test="$maxLength">
				<xsl:attribute name="MaxLength">
					<xsl:value-of select="$maxLength"/>
				</xsl:attribute>
			</xsl:if>


			<xsl:choose>
				<xsl:when test="$isMandatory='true' or $isMandatory=1 or $isMandatory='alethic'">
					<xsl:attribute name="Nullable">
						<xsl:value-of select="false()"/>
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="$isMandatory='false' or $isMandatory=0 or $isMandatory='deontic'">
					<xsl:attribute name="Nullable">
						<xsl:value-of select="true()"/>
					</xsl:attribute>
				</xsl:when>
			</xsl:choose>
		</edm:Property>
	</xsl:template>

	<xsl:template match="odt:identity" mode="GenerateProperty">
		<xsl:param name="informationTypeName"/>
		<xsl:param name="isMandatory"/>
		<edm:Property Name="{$informationTypeName}">
			<xsl:attribute name="Type">
				<xsl:value-of select="'Guid'"/>
			</xsl:attribute>
			<xsl:variable name="maxLength" select="@maxLength"/>
			<xsl:if test="$maxLength">
				<xsl:attribute name="MaxLength">
					<xsl:value-of select="$maxLength"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="$isMandatory='true' or $isMandatory=1 or $isMandatory='alethic'">
					<xsl:attribute name="Nullable">
						<xsl:value-of select="false()"/>
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="$isMandatory='false' or $isMandatory=0 or $isMandatory='deontic'">
					<xsl:attribute name="Nullable">
						<xsl:value-of select="true()"/>
					</xsl:attribute>
				</xsl:when>
			</xsl:choose>
		</edm:Property>
	</xsl:template>
	<xsl:template match="odt:binary" mode="GenerateProperty">
		<xsl:param name="informationTypeName"/>
		<xsl:param name="isMandatory"/>
		<edm:Property Name="{$informationTypeName}">
			<xsl:attribute name="Type">
				<xsl:value-of select="'Binary'"/>
			</xsl:attribute>
			<xsl:variable name="maxLength" select="@maxLength"/>
			<xsl:if test="$maxLength">
				<xsl:attribute name="MaxLength">
					<xsl:value-of select="$maxLength"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="$isMandatory='true' or $isMandatory=1 or $isMandatory='alethic'">
					<xsl:attribute name="Nullable">
						<xsl:value-of select="false()"/>
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="$isMandatory='false' or $isMandatory=0 or $isMandatory='deontic'">
					<xsl:attribute name="Nullable">
						<xsl:value-of select="true()"/>
					</xsl:attribute>
				</xsl:when>
			</xsl:choose>
		</edm:Property>
	</xsl:template>
	<xsl:template match="odt:boolean" mode="GenerateProperty">
		<xsl:param name="informationTypeName"/>
		<xsl:param name="isMandatory"/>
		<edm:Property Name="{$informationTypeName}">
			<xsl:attribute name="Type">
				<xsl:value-of select="'Boolean'"/>
			</xsl:attribute>
			<xsl:variable name="maxLength" select="@maxLength"/>
			<xsl:if test="$maxLength">
				<xsl:attribute name="MaxLength">
					<xsl:value-of select="$maxLength"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="$isMandatory='true' or $isMandatory=1 or $isMandatory='alethic'">
					<xsl:attribute name="Nullable">
						<xsl:value-of select="false()"/>
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="$isMandatory='false' or $isMandatory=0 or $isMandatory='deontic'">
					<xsl:attribute name="Nullable">
						<xsl:value-of select="true()"/>
					</xsl:attribute>
				</xsl:when>
			</xsl:choose>
		</edm:Property>
	</xsl:template>
	<xsl:template match="odt:decimalNumber" mode="GenerateProperty">
		<xsl:param name="informationTypeName"/>
		<xsl:param name="isMandatory"/>
		<edm:Property Name="{$informationTypeName}">
			<xsl:attribute name="Type">
				<xsl:value-of select="'Decimal'"/>
			</xsl:attribute>
			<xsl:variable name="maxLength" select="@maxLength"/>
			<xsl:if test="$maxLength">
				<xsl:attribute name="MaxLength">
					<xsl:value-of select="$maxLength"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="$isMandatory='true' or $isMandatory=1 or $isMandatory='alethic'">
					<xsl:attribute name="Nullable">
						<xsl:value-of select="false()"/>
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="$isMandatory='false' or $isMandatory=0 or $isMandatory='deontic'">
					<xsl:attribute name="Nullable">
						<xsl:value-of select="true()"/>
					</xsl:attribute>
				</xsl:when>
			</xsl:choose>
		</edm:Property>
	</xsl:template>
	<xsl:template match="odt:floatingPointNumber" mode="GenerateProperty">
		<xsl:param name="informationTypeName"/>
		<xsl:param name="isMandatory"/>
		<edm:Property Name="{$informationTypeName}">
			<xsl:attribute name="Type">
				<xsl:value-of select="'Float'"/>
			</xsl:attribute>
			<xsl:variable name="maxLength" select="@maxLength"/>
			<xsl:if test="$maxLength">
				<xsl:attribute name="MaxLength">
					<xsl:value-of select="$maxLength"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:choose>
				<xsl:when test="$isMandatory='true' or $isMandatory=1 or $isMandatory='alethic'">
					<xsl:attribute name="Nullable">
						<xsl:value-of select="false()"/>
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="$isMandatory='false' or $isMandatory=0 or $isMandatory='deontic'">
					<xsl:attribute name="Nullable">
						<xsl:value-of select="true()"/>
					</xsl:attribute>
				</xsl:when>
			</xsl:choose>
		</edm:Property>
	</xsl:template>

	<xsl:template match="oil:conceptType[oil:roleSequenceUniquenessConstraint/@isPreferred='true']" mode="GenerateAssocation">
		<!--UNDONE: Wee need to discover wether or not an Association can have more than one endpoint. Also Wether or not we can complexly identify an Entity.-->
		<!--<edm:Association Name="{@name}">
			<xsl:for-each select="">
				<edm:End Role="" Type="" Multiplicity=""/>
			</xsl:for-each>
		</edm:Association>-->
	</xsl:template>

	<xsl:template match="oil:conceptTypeRef" mode="GenerateAssociation">
		<xsl:variable name="conceptTypeRefName" select="@name"/>
		<xsl:variable name ="conceptTypeName" select="../@name"/>
		<edm:Association Name="{concat($conceptTypeRefName,$conceptTypeName)}">
			<xsl:choose>
				<xsl:when test="@mandatory='false' or @manadatory='0' or @mandatory='deontic'">
					<edm:End Type="{$conceptTypeRefName}" Multiplicity="0..1"/>
				</xsl:when>
				<xsl:when test="not(@manadatory='true' or @mandatory='1' or @mandatory='alethic')">
					<edm:End Type="{$conceptTypeRefName}" Multiplicity="1"/>
				</xsl:when>
			</xsl:choose>
			<edm:End Type="{$conceptTypeName}" Multiplicity="0..*"/>
		</edm:Association>
	</xsl:template>

	<!--<xsl:template match="oil:conceptTypeRef" mode="GenerateContainment">
		<edm:Containment>
			<edm:Parent />
			<edm:Child/>
		</edm:Containment>
	</xsl:template>-->

</xsl:stylesheet>