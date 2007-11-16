<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:oial0="http://schemas.neumont.edu/ORM/2006-01/OIALModel"
	xmlns:exsl="http://exslt.org/common"
	exclude-result-prefixes="#default xsl oial0"
	extension-element-prefixes="exsl">
	
	<xsl:variable name="removeNamespace" select="'http://schemas.neumont.edu/ORM/2006-01/OIALModel'"/>
	<xsl:template match="/">
		<xsl:for-each select="child::*">
			<xsl:variable name="currentNamespace" select="namespace-uri()"/>
			<xsl:element name="{name()}" namespace="{$currentNamespace}">
				<xsl:for-each select="namespace::*[local-name()!='xml' and .!=$removeNamespace and .!=$currentNamespace]">
					<xsl:copy-of select="."/>
				</xsl:for-each>
				<xsl:apply-templates select="@*|*|text()|comment()"/>
			</xsl:element>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="@*|*|text()|comment()">
		<xsl:copy>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="oial0:*">
		<!-- Intentionally empty, remove the elements-->
	</xsl:template>
</xsl:stylesheet>