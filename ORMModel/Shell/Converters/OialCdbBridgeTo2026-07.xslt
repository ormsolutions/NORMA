<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:oldOialtocdb="http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase"
	xmlns:oialtocdb="http://schemas.neumont.edu/ORM/Bridge/2026-07/ORMAbstractionToConceptualDatabase"
	xmlns:exsl="http://exslt.org/common"
	exclude-result-prefixes="#default xsl oldOialtocdb"
	extension-element-prefixes="exsl">
	
	<!-- Implementation note: The beginning of this file will basically be the same
	for any extension upgrade that is not registered as a 'runWith' transform. The root
	element needs to be modified to get the correct prefix, and the individual elements
	need to be upgraded to the new namespace. -->
	<xsl:variable name="targetPrefix" select="'oialtocdb'"/>
	<xsl:variable name="targetNamespace" select="'http://schemas.neumont.edu/ORM/Bridge/2026-07/ORMAbstractionToConceptualDatabase'"/>
	<xsl:variable name="replaceNamespace" select="'http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase'"/>
	<xsl:template name="AddNamespacePrefix">
		<xsl:param name="Prefix"/>
		<xsl:param name="Namespace"/>
		<xsl:variable name="DummyFragment">
			<xsl:choose>
				<xsl:when test="string-length($Prefix)">
					<xsl:element name="{$Prefix}:PickAName" namespace="{$Namespace}"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:element name="PickAName" namespace="{$Namespace}"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:copy-of select="exsl:node-set($DummyFragment)/child::*/namespace::*[local-name()!='xml']"/>
	</xsl:template>
	<xsl:template match="/">
		<xsl:for-each select="child::*">
			<xsl:variable name="currentNamespace" select="namespace-uri()"/>
			<xsl:element name="{name()}" namespace="{$currentNamespace}">
				<xsl:for-each select="namespace::*[local-name()!='xml' and .!=$replaceNamespace and .!=$currentNamespace]">
					<xsl:copy-of select="."/>
				</xsl:for-each>
				<xsl:call-template name="AddNamespacePrefix">
					<xsl:with-param name="Prefix" select="$targetPrefix"/>
					<xsl:with-param name="Namespace" select="$targetNamespace"/>
				</xsl:call-template>
				<xsl:apply-templates select="@*|*|text()|comment()"/>
			</xsl:element>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="@*|*|text()|comment()">
		<xsl:copy>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="oldOialtocdb:*">
		<xsl:element name="oialtocdb:{local-name()}">
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</xsl:element>
	</xsl:template>
</xsl:stylesheet>