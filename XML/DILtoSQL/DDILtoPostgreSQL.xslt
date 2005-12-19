<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" 
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:dml="http://schemas.orm.net/DIL/DMIL"
	xmlns:dms="http://schemas.orm.net/DIL/DILMS"
	xmlns:dep="http://schemas.orm.net/DIL/DILEP"
	xmlns:ddt="http://schemas.orm.net/DIL/DILDT"
	xmlns:dil="http://schemas.orm.net/DIL/DIL"
	xmlns:ddl="http://schemas.orm.net/DIL/DDIL"
	extension-element-prefixes="msxsl"
	exclude-result-prefixes="dml dms dep ddt dil ddl">

	<xsl:import href="DDILtoSQLStandard.xslt"/>

	<xsl:output method="text" encoding="utf-8" indent="no" omit-xml-declaration="yes"/>
	<xsl:strip-space elements="*"/>

	<xsl:template match="@defaultCharacterSet" mode="ForSchemaDefinition"/>

	<xsl:template match="dms:setSchemaStatement"/>

	<xsl:template match="ddl:generationClause"/>

	<xsl:template match="ddl:columnDefinition[ddl:identityColumnSpecification]">
		<xsl:param name="indent"/>
		<xsl:value-of select="$indent"/>
		<xsl:apply-templates select="@name" mode="ForColumnDefinition"/>
		<xsl:apply-templates select="ddt:boolean | ddt:characterString | ddt:binaryString | ddt:date | ddt:time | ddt:interval | ddt:domain"/>
		<xsl:apply-templates select="ddt:exactNumeric | ddt:approximateNumeric" mode="ForPostgresIdentityColumn"/>
		<xsl:apply-templates select="ddl:defaultClause"/>		
		<xsl:apply-templates select="ddl:generationClause"/>
		<xsl:apply-templates select="ddl:columnConstraintDefinition"/>
		<xsl:choose>
			<xsl:when test="position()=last() and not(following-sibling::*)">
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>, </xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="ddt:exactNumeric | ddt:approximateNumeric" mode="ForPostgresIdentityColumn">
		<xsl:choose>
			<xsl:when test="@type='INTEGER' or @type='SMALLINT'">
				<xsl:text> SERIAL </xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text> BIGSERIAL </xsl:text>
			</xsl:otherwise>
		</xsl:choose>		
	</xsl:template>
	
</xsl:stylesheet>