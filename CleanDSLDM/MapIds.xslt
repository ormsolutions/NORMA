<?xml version="1.0" encoding="utf-8"?>
<!--
	Neumont Object Role Modeling Architect for Visual Studio

	Copyright © Neumont University. All rights reserved.

	The use and distribution terms for this software are covered by the
	Common Public License 1.0 (http://opensource.org/licenses/cpl) which
	can be found in the file CPL.txt at the root of this distribution.
	By using this software in any fashion, you are agreeing to be bound by
	the terms of this license.

	You must not remove this notice, or any other, from this software.
-->
<xsl:stylesheet
	version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	xmlns:idMap="id-map-extension"
	xmlns:dmd="urn:schemas-microsoft-com:dmd"
	exclude-result-prefixes="msxsl">
	<xsl:output method="xml" encoding="utf-8" indent="no"/>
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
	<xsl:template match="@identity">
		<xsl:attribute name="{name()}">
			<xsl:value-of select="idMap:Map(.)"/>
		</xsl:attribute>
	</xsl:template>
</xsl:stylesheet>