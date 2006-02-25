<?xml version="1.0" encoding="utf-8"?>
<!--
	Copyright © Neumont University. All rights reserved.

	This software is provided 'as-is', without any express or implied warranty. In no event will the authors be held liable for any damages arising from the use of this software.
	Permission is granted to anyone to use this software for any purpose, including commercial applications, and to alter it and redistribute it freely, subject to the following restrictions:
	1. The origin of this software must not be misrepresented; you must not claim that you wrote the original software. If you use this software in a product, an acknowledgment in the product documentation would be appreciated but is not required.
	2. Altered source versions must be plainly marked as such, and must not be misrepresented as being the original software.
	3. This notice may not be removed or altered from any source distribution.
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:oldRoot="http://schemas.neumont.edu/ORM/ORMRoot"
	xmlns:oldDiagram="http://schemas.neumont.edu/ORM/ORMDiagram"
	xmlns:ormRoot="http://schemas.neumont.edu/ORM/2006-01/ORMRoot"
	xmlns:ormDiagram="http://schemas.neumont.edu/ORM/2006-01/ORMDiagram"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt" 
	exclude-result-prefixes="#default xsl oldRoot oldDiagram"
	extension-element-prefixes="msxsl">
	<xsl:import href="CoreUpgradeTo2006-01.xslt"/>
	<xsl:output method="xml" encoding="utf-8" indent="no"/>
	<!-- Note: processing for default elements done in core -->
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
		<xsl:copy-of select="msxsl:node-set($DummyFragment)/child::*/namespace::*[local-name()!='xml']"/>
	</xsl:template>
	<xsl:template match="oldRoot:ORM2">
		<!-- The extension mechanism requires all namespaces used in the file to be specified on
		     the root element. We need to eliminate the old ones, but keep the ones we're not explicitly
				 upgrading. Note that core and diagram are considered to be extensions by the loader, so
				 it is very important to get these into the file now rather than later. -->
		<xsl:element name="ormRoot:ORM2">
			<xsl:call-template name="AddNamespacePrefix">
				<xsl:with-param name="Prefix" select="'orm'"/>
				<xsl:with-param name="Namespace" select="concat('http://schemas.neumont.edu/ORM/2006-01/ORMCore',@name)"/>
			</xsl:call-template>
			<xsl:call-template name="AddNamespacePrefix">
				<xsl:with-param name="Prefix" select="'ormDiagram'"/>
				<xsl:with-param name="Namespace" select="concat('http://schemas.neumont.edu/ORM/2006-01/ORMDiagram',@name)"/>
			</xsl:call-template>
			<xsl:for-each select="namespace::*[local-name()!='xml' and .!='http://schemas.neumont.edu/ORM/ORMDiagram' and .!='http://schemas.neumont.edu/ORM/ORMCore' and .!='http://schemas.neumont.edu/ORM/ORMRoot']">
				<xsl:copy-of select="."/>
			</xsl:for-each>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="oldRoot:*">
		<!-- The extension mechanism requires all namespaces used in the file to be specified on
		     the root element. We need to eliminate the old ones, but keep the ones we're not explicitly
				 upgrading. Note that core and diagram are considered to be extensions by the loader, so
				 it is very important to get these into the file now rather than later. -->
		<xsl:element name="ormRoot:{local-name()}">
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="oldDiagram:*">
		<xsl:element name="ormDiagram:{local-name()}">
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</xsl:element>
	</xsl:template>
	<xsl:template match="oldDiagram:ValueRangeShape">
		<ormDiagram:ValueConstraintShape>
			<xsl:apply-templates select="@*|*|text()|comment()"/>
		</ormDiagram:ValueConstraintShape>
	</xsl:template>
</xsl:stylesheet>
