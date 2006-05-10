<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:exsl="http://exslt.org/common"
	xmlns:nsu="urn:schemas-neumont-edu:ORM:NamespacesUtility"
	xmlns:orm="http://schemas.neumont.edu/ORM/2006-04/ORMCore"
	xmlns:ormRoot="http://schemas.neumont.edu/ORM/2006-04/ORMRoot"
	extension-element-prefixes="exsl nsu">

	<xsl:output method="xml" encoding="utf-8" media-type="application/orm+xml" indent="no"/>

	<xsl:variable name="whateverFragment">
		<xsl:call-template name="GetNextSelectedNamespace"/>
	</xsl:variable>
	<xsl:variable name="selectedNamespaces" select="exsl:node-set($whateverFragment)/child::*/@namespaceUri"/>

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
		<xsl:copy-of select="exsl:node-set($DummyFragment)/child::*/namespace::node()[local-name()!='xml']"/>
	</xsl:template>

	<xsl:template match="ormRoot:ORM2">
		<ormRoot:ORM2>
			<xsl:for-each select="namespace::node()[not(local-name()='xml') and not(.='http://schemas.neumont.edu/ORM/2006-04/ORMDiagram') and not(.='http://schemas.neumont.edu/ORM/2006-04/ORMCore') and not(.='http://schemas.neumont.edu/ORM/2006-04/ORMRoot')]">
				<xsl:if test="nsu:isNamespaceSelected(.)">
					<xsl:copy-of select="."/>
					<xsl:if test="nsu:addedNamespace(.)"/>
				</xsl:if>
			</xsl:for-each>
			<xsl:for-each select="$selectedNamespaces">
				<xsl:if test="not(nsu:wasNamespaceAdded(.))">
					<xsl:call-template name="AddNamespacePrefix">
						<xsl:with-param name="Prefix" select="nsu:getRandomPrefix()"/>
						<xsl:with-param name="Namespace" select="."/>
					</xsl:call-template>
					<xsl:if test="nsu:addedNamespace(.)"/>
				</xsl:if>
			</xsl:for-each>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</ormRoot:ORM2>
	</xsl:template>

	<xsl:template name="GetNextSelectedNamespace">
		<xsl:variable name="nextSelectedNamespace" select="nsu:getNextSelectedNamespace()"/>
		<xsl:if test="$nextSelectedNamespace">
			<whatever namespaceUri="{$nextSelectedNamespace}"/>
			<xsl:call-template name="GetNextSelectedNamespace"/>
		</xsl:if>
	</xsl:template>

	<xsl:template match="*">
		<xsl:variable name="namespace" select="namespace-uri()"/>
		<xsl:if test="nsu:isNamespaceSelected($namespace) or $namespace='http://schemas.neumont.edu/ORM/2006-04/ORMDiagram' or $namespace='http://schemas.neumont.edu/ORM/2006-04/ORMCore' or $namespace='http://schemas.neumont.edu/ORM/2006-04/ORMRoot'">
			<xsl:copy>
				<xsl:apply-templates select="@*|*|text()|comment()"/>
			</xsl:copy>
		</xsl:if>
	</xsl:template>
	<xsl:template match="@*|text()|comment()">
		<xsl:copy>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</xsl:copy>
	</xsl:template>

</xsl:stylesheet>
