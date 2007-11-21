<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet
	version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:idMap="id-map-extension"
	exclude-result-prefixes="#default">

	<!-- Templates to duplicate structure with new Ids -->
	<xsl:template match="*">
		<xsl:copy>
			<xsl:apply-templates select="@*"/>
			<xsl:value-of select="text()"/>
			<xsl:apply-templates select="child::*"/>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="@*">
		<!-- Run all attributes through here to handle referencing attributes
		other than @ref. This was originally @id | @ref, but that does not
		work with top-level serialized links, which use attribute names
		other than @ref to refer to @id attributes. -->
		<xsl:attribute name="{name()}">
			<xsl:value-of select="idMap:Map(.)"/>
		</xsl:attribute>
	</xsl:template>
</xsl:stylesheet>
