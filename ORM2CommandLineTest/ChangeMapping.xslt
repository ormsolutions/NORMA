<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" 
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:tr="http://schemas.neumont.edu/ORM/SDK/TestReport"
	xmlns:set="http://schemas.microsoft.com/VisualStudio/2004/01/settings">
	
<xsl:output method="xml" indent="yes"/>
	<xsl:template match="*">
		<xsl:copy>
			<xsl:apply-templates select="@*|text()|comment()|child::*"/>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="@*|comment()">
		<xsl:copy-of select="."/>
	</xsl:template>
	<xsl:template match="text()">
		<xsl:if test="string-length(normalize-space(.))!=0">
			<xsl:copy-of select="."/>
		</xsl:if>
	</xsl:template>
	<xsl:template match="set:SettingsFile/set:Settings/set:Setting">
		<xsl:if>
		<xsl:copy>
			<Value>
				New Value And Junk
			</Value>
		</xsl:copy>
		</xsl:if>
		
	</xsl:template>
	
	
	
	
	
	
	


</xsl:stylesheet>