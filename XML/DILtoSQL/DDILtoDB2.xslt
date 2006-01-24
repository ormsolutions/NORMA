<?xml version="1.0" encoding="utf-8" ?>
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

  <xsl:template match="dms:startTransactionStatement"/>    

  <xsl:template match="@defaultCharacterSet" mode="ForSchemaDefinition"/>

  <xsl:template match="dms:commitStatement">
    <xsl:text>COMMIT</xsl:text>
    <xsl:value-of select="$StatementDelimeter"/>
    <xsl:value-of select="$NewLine"/>
    <xsl:value-of select="$NewLine"/>
  </xsl:template>

  <xsl:template match="dep:trimFunction">
    <xsl:choose>
      <xsl:when test="@specification='BOTH'">
        <xsl:text>LTRIM</xsl:text>
        <xsl:value-of select="$LeftParen"/>
        <xsl:text>RTRIM</xsl:text>
        <xsl:value-of select="$LeftParen"/>
        <xsl:apply-templates select="dep:trimSource"/>
        <xsl:value-of select="$RightParen"/>
        <xsl:value-of select="$RightParen"/>
      </xsl:when>
      <xsl:when test="@specification='LEADING'">
        <xsl:text>LTRIM</xsl:text>
        <xsl:value-of select="$LeftParen"/>
        <xsl:apply-templates select="dep:trimSource"/>
        <xsl:value-of select="$RightParen"/>
      </xsl:when>
      <xsl:when test="@specification='TRAILING'">
        <xsl:text>RTRIM</xsl:text>
        <xsl:value-of select="$LeftParen"/>
        <xsl:apply-templates select="dep:trimSource"/>
        <xsl:value-of select="$RightParen"/>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="dep:charLengthExpression">
    <xsl:text>LENGTH</xsl:text>
    <xsl:value-of select="$LeftParen"/>
    <xsl:apply-templates/>
    <xsl:value-of select="$RightParen"/>
  </xsl:template>

  <xsl:template match="ddt:boolean">
    <xsl:text>CHARACTER</xsl:text>
    <xsl:value-of select="$LeftParen"/>
    <xsl:text>1</xsl:text>
    <xsl:value-of select="$RightParen"/>
    <xsl:text> FOR BIT DATA</xsl:text>
  </xsl:template>

  <!-- UNDONE: This isn't going to work for ddl:tableConstraintDefinition elements that are not inside of ddl:tableDefinition elements -->
  <xsl:template match="ddl:tableConstraintDefinition[child::ddl:uniqueConstraintDefinition]">
    <xsl:param name="indent"/>
    <xsl:value-of select="$indent"/>
    <xsl:choose>
      <xsl:when test="parent::ddl:tableDefinition/ddl:columnDefinition[@name=current()/ddl:uniqueConstraintDefinition/ddl:column/@name and not(ddl:columnConstraintDefinition/ddl:notNullKeyword)]">
        
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-imports/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>    
  
</xsl:stylesheet>