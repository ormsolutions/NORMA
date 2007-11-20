<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:oldRvw="http://schemas.neumont.edu/ORM/Views/RelationalView"
	xmlns:rvw="http://schemas.neumont.edu/ORM/2007-11/RelationalView"
	xmlns:rcd="http://schemas.neumont.edu/ORM/Relational/2007-06/ConceptualDatabase"
	xmlns:exsl="http://exslt.org/common"
	exclude-result-prefixes="#default xsl oldRvw"
	extension-element-prefixes="exsl">
	
	<xsl:variable name="targetPrefix" select="'rvw'"/>
	<xsl:variable name="targetNamespace" select="'http://schemas.neumont.edu/ORM/2007-11/RelationalView'"/>
	<xsl:variable name="replaceNamespace" select="'http://schemas.neumont.edu/ORM/Views/RelationalView'"/>
	<xsl:variable name="existingCatalog" select="child::*/rcd:Catalog"/>
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
				<xsl:if test="not(namespace::*[.='http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase'])">
					<xsl:call-template name="AddNamespacePrefix">
						<xsl:with-param name="Prefix" select="'oialtocdb'"/>
						<xsl:with-param name="Namespace" select="'http://schemas.neumont.edu/ORM/Bridge/2007-06/ORMAbstractionToConceptualDatabase'"/>
					</xsl:call-template>
				</xsl:if>
				<xsl:apply-templates select="@*|*|text()|comment()"/>
			</xsl:element>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="@*|*|text()|comment()">
		<xsl:copy>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</xsl:copy>
	</xsl:template>
	<xsl:template match="oldRvw:RelationalModel">
		<!-- Remove this element -->
	</xsl:template>
	<xsl:template match="oldRvw:RelationalDiagram">
		<rvw:RelationalDiagram>
			<xsl:copy-of select="../oldRvw:RelationalModel[@id=current()/@SubjectRef]/@DisplayDataTypes"/>
			<xsl:apply-templates select="@*"/>
			<xsl:attribute name="SubjectRef">
				<xsl:choose>
					<xsl:when test="$existingCatalog">
						<xsl:value-of select="$existingCatalog/@id"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>CatalogFor</xsl:text>
						<xsl:value-of select="@id"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:apply-templates select="*|text()|comment()"/>
		</rvw:RelationalDiagram>
		<xsl:if test="not($existingCatalog)">
			<rcd:Catalog id="CatalogFor{@id}"/>
		</xsl:if>
	</xsl:template>
	<xsl:template match="oldRvw:*">
		<xsl:element name="rvw:{local-name()}">
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</xsl:element>
	</xsl:template>
</xsl:stylesheet>