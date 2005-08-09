<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet
	version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:idMap="id-map-extension"
	exclude-result-prefixes="#default msxsl">

	<!-- Templates to duplicate structure with new Ids -->
	<xsl:template match="*">
		<xsl:copy>
			<xsl:apply-templates select="@*"/>
			<xsl:value-of select="text()"/>
			<xsl:apply-templates select="child::*"/>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="@*">
		<xsl:copy-of select="."/>
	</xsl:template>
	<xsl:template match="@ref | @id">
		<xsl:attribute name="{name()}">
			<xsl:value-of select="idMap:Map(.)"/>
		</xsl:attribute>
	</xsl:template>
</xsl:stylesheet>