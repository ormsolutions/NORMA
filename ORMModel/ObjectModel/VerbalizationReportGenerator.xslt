<?xml version="1.0" encoding="utf-8"?>
<!--
	Natural Object-Role Modeling Architect for Visual Studio

	Copyright © Neumont University. All rights reserved.

	The use and distribution terms for this software are covered by the
	Common Public License 1.0 (http://opensource.org/licenses/cpl) which
	can be found in the file CPL.txt at the root of this distribution.
	By using this software in any fashion, you are agreeing to be bound by
	the terms of this license.

	You must not remove this notice, or any other, from this software.
-->
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:rvg="http://schemas.neumont.edu/ORM/SDK/ReportVerbalizationGenerator"
	xmlns:exsl="http://exslt.org/common"
	extension-element-prefixes="exsl"
	exclude-result-prefixes="rvg">

	<!-- Indenting is useful for debugging the transform, but a waste of memory at generation time -->
	<xsl:output method="xml" encoding="utf-8" indent="no"/>
	<!-- Pick up param value supplied automatically by plix loader -->
	<xsl:param name="CustomToolNamespace" select="'ORMSolutions.ORMArchitect.Core.ObjectModel'"/>

	<!-- Names of the different classes we generate -->
	<xsl:param name="ReportVerbalizationSnippetType" select="'ReportVerbalizationSnippetType'"/>
	<xsl:param name="ReportVerbalizationSets" select="'ReportVerbalizationSets'"/>
	<xsl:include href="VerbalizationGenerator.Sets.xslt"/>

	<xsl:template match="/">
		<plx:root>
			<plx:namespaceImport name="System"/>
			<plx:namespaceImport name="System.IO"/>
			<plx:namespaceImport name="System.Text"/>
			<plx:namespaceImport name="System.Collections.Generic"/>
			<plx:namespaceImport name="Microsoft.VisualStudio.Modeling"/>
			<plx:namespace name="{$CustomToolNamespace}">
				<xsl:for-each select="rvg:*">
					<xsl:if test="rvg:Copyright">
						<plx:leadingInfo>
							<plx:comment blankLine="true"/>
							<plx:comment>
								<xsl:value-of select="rvg:Copyright/@name"/>
							</plx:comment>
							<xsl:for-each select="rvg:Copyright/rvg:CopyrightLine">
								<plx:comment>
									<xsl:value-of select="."/>
								</plx:comment>
							</xsl:for-each>
							<plx:comment blankLine="true"/>
						</plx:leadingInfo>
					</xsl:if>
					<!-- Generate verbalization set classes and default populations -->
					<xsl:call-template name="GenerateVerbalizationSets">
						<xsl:with-param name="SnippetEnumTypeName" select="$ReportVerbalizationSnippetType"/>
						<xsl:with-param name="VerbalizationSetName" select="$ReportVerbalizationSets"/>
						<xsl:with-param name="SnippetsLocation" select="@snippetsLocation"/>
					</xsl:call-template>
				</xsl:for-each>
			</plx:namespace>
		</plx:root>
	</xsl:template>
</xsl:stylesheet>