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
	xmlns:cpvg="http://schemas.neumont.edu/ORM/SDK/CustomPropertyVerbalizationGenerator"
	xmlns:exsl="http://exslt.org/common"
	extension-element-prefixes="exsl"
	exclude-result-prefixes="cpvg">

	<!-- Indenting is useful for debugging the transform, but a waste of memory at generation time -->
	<xsl:output method="xml" encoding="utf-8" indent="no"/>
	<!-- Pick up param value supplied automatically by plix loader -->
	<xsl:param name="CustomToolNamespace" select="'ORMSolutions.ORMArchitect.CustomProperties'"/>

	<!-- Names of the different classes we generate -->
	<xsl:param name="CustomPropertyVerbalizationSnippetType" select="'CustomPropertyVerbalizationSnippetType'"/>
	<xsl:param name="CustomPropertyVerbalizationSets" select="'CustomPropertyVerbalizationSets'"/>
	<xsl:include href="..\ORMModel\ObjectModel\VerbalizationGenerator.Sets.xslt"/>

	<xsl:template match="/">
		<plx:root>
			<plx:namespaceImport name="System"/>
			<plx:namespaceImport name="System.IO"/>
			<plx:namespaceImport name="System.Text"/>
			<plx:namespaceImport name="System.Collections.Generic"/>
			<plx:namespaceImport name="Microsoft.VisualStudio.Modeling"/>
			<plx:namespaceImport name="ORMSolutions.ORMArchitect.Core.ObjectModel"/>
			<plx:namespace name="{$CustomToolNamespace}">
				<xsl:for-each select="cpvg:*">
					<xsl:if test="cpvg:Copyright">
						<plx:leadingInfo>
							<plx:comment blankLine="true"/>
							<plx:comment>
								<xsl:value-of select="cpvg:Copyright/@name"/>
							</plx:comment>
							<xsl:for-each select="cpvg:Copyright/cpvg:CopyrightLine">
								<plx:comment>
									<xsl:value-of select="."/>
								</plx:comment>
							</xsl:for-each>
							<plx:comment blankLine="true"/>
						</plx:leadingInfo>
					</xsl:if>
					<!-- Generate verbalization set classes and default populations -->
					<xsl:call-template name="GenerateVerbalizationSets">
						<xsl:with-param name="SnippetEnumTypeName" select="$CustomPropertyVerbalizationSnippetType"/>
						<xsl:with-param name="VerbalizationSetName" select="$CustomPropertyVerbalizationSets"/>
						<xsl:with-param name="SnippetsLocation" select="@snippetsLocation"/>
					</xsl:call-template>
				</xsl:for-each>
			</plx:namespace>
		</plx:root>
	</xsl:template>
</xsl:stylesheet>
