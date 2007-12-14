<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet
	version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:se="http://NetTiers/2.2/SchemaExplorer"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:exsl="http://exslt.org/common"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:csc="urn:nettiers:CommonSqlCode"
	xmlns:tmp="urn:temporary"
	extension-element-prefixes="exsl msxsl csc"
	exclude-result-prefixes="se xsl tmp">
	<xsl:include href="EntityScript.xslt"/>
	<!-- DEBUG: indent="yes" is for debugging only, has sideeffects on docComment output. -->
	<xsl:output method="xml" indent="no"/>
	<xsl:param name="CustomToolNamespace" select="'TestNamespace'"/>
	<xsl:param name="NetTiersSettings" select="document('NETTiersSettings.xml')/child::*"/>
	<xsl:variable name="settings" select="$NetTiersSettings/child::*"/>
	<xsl:variable name="DALNamespace" select="concat($CustomToolNamespace,'.',$settings[@name='DataAccessLayerNamespace'])"/>
	<xsl:variable name="BLLNamespace" select="concat($CustomToolNamespace,'.',$settings[@name='BusinessLogicLayerNamespace'])"/>
	<xsl:template match="/">
		<xsl:variable name="initializeSettings">
			<xsl:value-of select="csc:SetChangeUnderscoreToPascalCase(boolean($settings[@name='ChangeUnderscoreToPascalCase']))"/>
			<xsl:value-of select="csc:SetUsePascalCasing($settings[@name='UsePascalCasing'])"/>
			<xsl:value-of select="csc:SetManyToManyFormat($settings[@name='ManyToManyFormat'])"/>
		</xsl:variable>
		<plx:root>
			<plx:namespaceImport name="System"/>
			<plx:namespaceImport name="System.Collections"/>
			<plx:namespaceImport name="System.Collections.Generic"/>
			<plx:namespaceImport name="System.ComponentModel"/>
			<plx:namespaceImport name="System.Data"/>
			<plx:namespaceImport name="System.Data.Common"/>
			<plx:namespaceImport name="System.Diagnostics"/>
			<plx:namespaceImport name="System.Runtime.Serialization"/>
			<plx:namespaceImport name="System.Xml.Serialization"/>
			<plx:namespaceImport name="{$CustomToolNamespace}"/>
			<plx:namespaceImport name="{$DALNamespace}"/>
			<plx:namespaceImport name="{$BLLNamespace}"/>
			<plx:namespace name="{$BLLNamespace}.SqlClient">
				<xsl:apply-templates select="child::*"/>
			</plx:namespace>
		</plx:root>
	</xsl:template>
</xsl:stylesheet>