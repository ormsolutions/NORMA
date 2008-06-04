<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
				xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:exsl="http://exslt.org/common"
				xmlns:orm="http://schemas.neumont.edu/ORM/2006-04/ORMCore"
				xmlns:ormRoot="http://schemas.neumont.edu/ORM/2006-04/ORMRoot"
				xmlns:ormdt="http://schemas.orm.net/ORMDataTypes"
				xmlns:oil="http://schemas.orm.net/OIAL"
				xmlns:xhtml="http://www.w3.org/1999/xhtml"
				xmlns:xforms="http://www.w3.org/2002/xforms">
	
	<xsl:import href="OIALtoXSD.xslt"/>
	
	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:strip-space elements="*"/>

	<xsl:template match="/">
		<xsl:apply-templates select="*"/>
	</xsl:template>

	<xsl:template match="oil:model">
		<xhtml:html xmlns:xhtml="http://www.w3.org/1999/xhtml" xmlns:xforms="http://www.w3.org/2002/xforms">
			<xhtml:head>
				<xhtml:title></xhtml:title>
				<xforms:model id="default">
					<xsl:apply-imports/>
				</xforms:model>
			</xhtml:head>
			<xhtml:body>
				<xsl:apply-templates select="//oil:conceptType"/>
			</xhtml:body>
		</xhtml:html>
	</xsl:template>

	<xsl:template match="oil:conceptType">
		<xsl:for-each select="oil:informationType">
			<xforms:input ref="{@name}">
				<xforms:label>
					<xsl:value-of select="@name"/>
				</xforms:label>
			</xforms:input>
		</xsl:for-each>
	</xsl:template>
	
</xsl:stylesheet>