<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
		xmlns="http://schemas.neumont.edu/ORM/2007-11/CustomProperties">
	<xsl:output method="xml" encoding="utf-8" indent="no"/>
	<xsl:template match="/">
		<CustomPropertyGroups>
			<xsl:apply-templates select="*/*"/>
		</CustomPropertyGroups>
	</xsl:template>
	<xsl:template match="*">
		<xsl:element name="{local-name()}">
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="@*|text()|comment()">
		<xsl:copy/>
	</xsl:template>
</xsl:stylesheet>