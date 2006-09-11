<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:tr="http://schemas.neumont.edu/ORM/SDK/TestReport">
  <xsl:output method="xml" indent="yes"/>
  <xsl:template match="*">
    <xsl:copy>
      <xsl:apply-templates select="@*|text()|comment()|child::*"/>
    </xsl:copy>
  </xsl:template>
  <xsl:template match="tr:Compare">
    <xsl:copy>
      <xsl:copy-of select="@*"/>
      <xsl:attribute name="result">pass</xsl:attribute>
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
</xsl:stylesheet>