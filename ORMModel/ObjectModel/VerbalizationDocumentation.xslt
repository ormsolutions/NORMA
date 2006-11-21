<?xml version="1.0" encoding="utf-8"?>
<!--
	Neumont Object-Role Modeling Architect for Visual Studio

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
	xmlns:ve="http://schemas.neumont.edu/ORM/SDK/Verbalization"
	xmlns:exsl="http://exslt.org/common"
	xmlns:cvg="http://schemas.neumont.edu/ORM/SDK/CoreVerbalizationGenerator"
	xmlns:ef="urn:extension-functions"
	xmlns:xs="http://www.w3.org/2001/XMLSchema"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
	extension-element-prefixes="exsl"
	exclude-result-prefixes="ve exsl plx cvg msxsl ef xs">

	<xsl:output method="xml" encoding="utf-8" indent="yes"/>
	<!-- Template to format Snippet documentation -->
	<xsl:template match="*">
		<xsl:variable name="verbalizationGeneratorFragment">
			<xsl:for-each select="child::*">
				<xsl:copy-of select="."/>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="verbalizationGenerator" select="exsl:node-set($verbalizationGeneratorFragment)"/>
		<xsl:variable name="SortedSnippetsFragment">
			<xsl:call-template name="GenerateSortedSnippetsFragment"/>
		</xsl:variable>
		<xsl:variable name="SortedSnippets" select="exsl:node-set($SortedSnippetsFragment)/child::*"/>
		<xsl:variable name="alethicPositiveFragment">
			<xsl:call-template name="MatchSnippetSet">
				<xsl:with-param name="SortedSnippets" select="$SortedSnippets"/>
				<xsl:with-param name="Modality" select="'alethic'"/>
				<xsl:with-param name="Sign" select="'positive'"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="alethicPositive" select="exsl:node-set($alethicPositiveFragment)/child::*"/>
		<xsl:variable name="statementsFragment">
			<xsl:call-template name="GenerateStatementFragment"/>
		</xsl:variable>
		<xsl:variable name="statements" select="exsl:node-set($statementsFragment)"/>
		<xsl:processing-instruction name="xml-stylesheet">
			<xsl:text>type="text/xsl" href="VerbalizationDocumentationHTML.xslt"</xsl:text>
		</xsl:processing-instruction>
		<snippets>
			<xsl:for-each select="$alethicPositive">
				<xsl:variable name="replacementCountFragment">
					<xsl:call-template name="CountReplacementFields">
						<xsl:with-param name="FormatString" select="@text"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:variable name="replacementCount" select="string($replacementCountFragment)"/>
				<xsl:variable name="snippetName" select="@type"/>
				<snippet name="{@type}" replacementCount="{$replacementCount}">
					<statement>
						<xsl:value-of select="$statements/child::xs:enumeration[@value=$snippetName]/xs:annotation/xs:documentation"/>
					</statement>
					<unformattedSnippet>
						<xsl:call-template name="StripTags">
							<xsl:with-param name="xmlText" select="@text"/>
						</xsl:call-template>
					</unformattedSnippet>
					<contains>
						<xsl:variable name="sortedSnippetsFragment">
							<xsl:for-each select="$verbalizationGenerator/descendant::cvg:Snippet[@ref=$snippetName]/descendant::cvg:Snippet">
								<xsl:sort select="@ref"/>
								<xsl:if test="not(@ref='null')">
									<snippet name="{@ref}"/>
								</xsl:if>
							</xsl:for-each>							
						</xsl:variable>
						<xsl:copy-of select="exsl:node-set($sortedSnippetsFragment)/child::*[not(@name=preceding-sibling::*[1]/@name)]"/>
					</contains>
					<containedIn>
						<xsl:variable name="sortedSnippetsFragment">
							<xsl:for-each select="$verbalizationGenerator/descendant::cvg:Snippet[./descendant::cvg:Snippet/@ref=$snippetName]">
								<xsl:sort select="@ref"/>
								<xsl:if test="not(@ref='null')">
									<snippet name="{@ref}"/>
								</xsl:if>
							</xsl:for-each>
							<xsl:for-each select="$verbalizationGenerator/descendant::cvg:Snippet[./descendant::cvg:*/@listStyle=$snippetName]">
								<xsl:sort select="@ref"/>
								<xsl:if test="not(@ref='null')">
									<snippet name="{@ref}"/>
								</xsl:if>
							</xsl:for-each>
						</xsl:variable>
						<xsl:copy-of select="exsl:node-set($sortedSnippetsFragment)/child::*[not(@name=preceding-sibling::*[1]/@name)]"/>
					</containedIn>
					<usedBy>
						<xsl:variable name="sortedConstraintsFragment">
							<xsl:for-each select="$verbalizationGenerator/cvg:Constructs/cvg:Constraints/cvg:Constraint[descendant::cvg:Snippet[@ref=$snippetName]]">
								<xsl:sort select="@type"/>
								<constraint name="{@type}"/>
							</xsl:for-each>
							<xsl:if test="contains($snippetName, 'List')">
								<xsl:for-each select="$verbalizationGenerator/cvg:Constructs/cvg:Constraints/cvg:Constraint[concat(substring-before($snippetName,'List'),'List')=descendant::cvg:*/@listStyle]">
									<xsl:sort select="@type"/>
									<constraint name="{@type}"/>
								</xsl:for-each>
							</xsl:if>
						</xsl:variable>
						<xsl:copy-of select="exsl:node-set($sortedConstraintsFragment)/child::*[not(@name=preceding-sibling::*[1]/@name)]"/>
					</usedBy>
				</snippet>
			</xsl:for-each>
		</snippets>
	</xsl:template>

	<!-- Template to get a list of <Snippet type="" text=""/> elements for the
	     specified Modality and Sign. If the exact modality and sign is not specified, then
		 match as many values as possible. Go ahead and include strings without a perfect match
		 in the current set. -->
	<xsl:template name="MatchSnippetSet">
		<!-- All ve:Snippet elements from the default (en-US) language sorted
		     by type/modality/sign with all default modality and sign values filled in.
			 Pass the child elements of the fragment returned from GeneratedSortedSnippetsFragment
			 to this parameter. -->
		<xsl:param name="SortedSnippets"/>
		<xsl:param name="Modality" select="'alethic'"/>
		<xsl:param name="Sign" select="'positive'"/>
		<xsl:for-each select="$SortedSnippets">
			<xsl:if test="position()=1">
				<xsl:call-template name="MatchSnippetSet2">
					<xsl:with-param name="Modality" select="$Modality"/>
					<xsl:with-param name="Sign" select="$Sign"/>
					<xsl:with-param name="MatchType" select="@type"/>
				</xsl:call-template>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="MatchSnippetSet2">
		<xsl:param name="Modality"/>
		<xsl:param name="Sign"/>
		<xsl:param name="MatchType"/>
		<xsl:param name="Match"/>
		<xsl:param name="BestMatch" select="''"/>
		<xsl:choose>
			<!-- The best match algorithm is based on zero marks for a non-default, non-requested hit,
			1 mark for a default match (alethic positive), two if one of the requested fields is match,
			and 3 if both are match. -->
			<xsl:when test="$MatchType=@type">
				<!-- See how many of the requested attributes the current context item matches -->
				<xsl:variable name="newBestMatch">
					<xsl:choose>
						<xsl:when test="@modality=$Modality">
							<xsl:choose>
								<xsl:when test="@sign=$Sign">
									<xsl:text>xxx</xsl:text>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>xx</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:when test="@sign=$Sign">
							<xsl:text>xx</xsl:text>
						</xsl:when>
						<xsl:when test="@sign='positive' and @modality='alethic'">
							<xsl:text>x</xsl:text>
						</xsl:when>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="matchFragment">
					<xsl:choose>
						<xsl:when test="(string-length($newBestMatch)&gt;string-length($BestMatch)) or (0=string-length($BestMatch))">
							<xsl:copy-of select="."/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:copy-of select="$Match"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<!-- Record if we're the last snippet element, or forward the data on -->
				<xsl:choose>
					<xsl:when test="position()=last()">
						<xsl:for-each select="exsl:node-set($matchFragment)/child::*">
							<Snippet type="{@type}" text="{text()}"/>
						</xsl:for-each>
					</xsl:when>
					<xsl:otherwise>
						<xsl:variable name="passBestMatch">
							<xsl:choose>
								<xsl:when test="string-length($newBestMatch)&gt;string-length($BestMatch)">
									<xsl:value-of select="$newBestMatch"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$BestMatch"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:for-each select="following-sibling::*">
							<xsl:if test="position()=1">
								<xsl:call-template name="MatchSnippetSet2">
									<xsl:with-param name="Modality" select="$Modality"/>
									<xsl:with-param name="Sign" select="$Sign"/>
									<xsl:with-param name="MatchType" select="$MatchType"/>
									<xsl:with-param name="BestMatch" select="string($passBestMatch)"/>
									<xsl:with-param name="Match" select="exsl:node-set($matchFragment)/child::*"/>
								</xsl:call-template>
							</xsl:if>
						</xsl:for-each>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<!-- Move on to a different type, but first record the last -->
				<xsl:if test="$Match">
					<xsl:for-each select="$Match">
						<Snippet type="{@type}" text="{text()}"/>
					</xsl:for-each>
				</xsl:if>
				<xsl:call-template name="MatchSnippetSet2">
					<xsl:with-param name="Modality" select="$Modality"/>
					<xsl:with-param name="Sign" select="$Sign"/>
					<xsl:with-param name="MatchType" select="@type"/>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="CountReplacementFields">
		<!-- A quick and dirty routine to count the number of replacement fields
			 in a format string. Used in documentation comments. -->
		<xsl:param name="FormatString"/>
		<xsl:variable name="replacements">
			<xsl:call-template name="CountReplacementFields2">
				<xsl:with-param name="FormatString" select="$FormatString"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:value-of select="count(exsl:node-set($replacements)/child::*[not(@value=following-sibling::*/@value)])"/>
	</xsl:template>
	<xsl:template name="CountReplacementFields2">
		<xsl:param name="FormatString"/>
		<xsl:if test="string-length($FormatString)">
			<xsl:choose>
				<xsl:when test="substring($FormatString,1,2)='{{'">
					<xsl:call-template name="CountReplacementFields2">
						<xsl:with-param name="FormatString" select="substring($FormatString,3)"/>
					</xsl:call-template>
				</xsl:when>
				<xsl:when test="substring($FormatString,1,1)='{'">
					<replace value="{number(substring-before(substring($FormatString, 2),'}'))}"/>
					<xsl:call-template name="CountReplacementFields2">
						<xsl:with-param name="FormatString" select="substring($FormatString,2)"/>
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:variable name="before" select="substring-before($FormatString,'{')"/>
					<xsl:if test="string-length($before)">
						<xsl:variable name="after" select="substring($FormatString,string-length($before)+2)"/>
						<xsl:choose>
							<xsl:when test="substring($after,1,1)='{'">
								<xsl:call-template name="CountReplacementFields2">
									<xsl:with-param name="FormatString" select="substring($after,2)"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<replace value="{number(substring-before($after,'}'))}"/>
								<xsl:call-template name="CountReplacementFields2">
									<xsl:with-param name="FormatString" select="$after"/>
								</xsl:call-template>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:if>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>
	<!-- Template to sort and normalize ve:Snippet templates for the default (en-US) language. Called from
	     the ve:VerbalizationRoot node.-->
	<xsl:template name="GenerateSortedSnippetsFragment">
		<xsl:for-each select="exsl:node-set(document(@snippetsLocation))/ve:Languages/ve:Language[@xml:lang='en-US']/ve:Snippets/ve:Snippet">
			<xsl:sort select="@type" data-type="text" order="ascending"/>
			<xsl:sort select="@modality" data-type="text" order="ascending"/>
			<!-- We want positive before negative so that we don't have to resort after explicitly adding the default values -->
			<xsl:sort select="@sign" data-type="text" order="descending"/>
			<xsl:copy>
				<xsl:copy-of select="@*"/>
				<xsl:if test="0=string-length(@modality)">
					<xsl:attribute name="modality">
						<xsl:text>alethic</xsl:text>
					</xsl:attribute>
				</xsl:if>
				<xsl:if test="0=string-length(@sign)">
					<xsl:attribute name="sign">
						<xsl:text>positive</xsl:text>
					</xsl:attribute>
				</xsl:if>
				<xsl:copy-of select="text()"/>
			</xsl:copy>
		</xsl:for-each>
	</xsl:template>
	<!-- Template to sort and normalize ve:Snippet templates for the default (en-US) language. Called from
	     the ve:VerbalizationRoot node.-->
	<xsl:template name="GenerateStatementFragment">
		<xsl:for-each select="exsl:node-set(document('VerbalizationCoreSnippets/VerbalizationCoreSnippets.xsd'))/xs:schema/xs:redefine/xs:simpleType[@name='SnippetTypeEnum']/xs:restriction/xs:enumeration">
			<xsl:sort select="@value" data-type="text" order="ascending"/>
			<xsl:copy-of select="."/>
		</xsl:for-each>
	</xsl:template>
	<xsl:template name="StripTags">
		<xsl:param name="xmlText"/>
		<xsl:choose>
			<xsl:when test="contains($xmlText,'&lt;')">
				<xsl:variable name="beforeOpen" select="substring-before($xmlText,'&lt;')"/>
				<xsl:value-of select="$beforeOpen"/>
				<xsl:variable name="remainder" select="substring($xmlText,string-length($beforeOpen)+2)"/>
				<xsl:if test="$remainder and contains($remainder,'&gt;')">
					<xsl:variable name="beforeClose" select="substring-before($remainder,'&gt;')"/>
					<xsl:call-template name="StripTags">
						<xsl:with-param name="xmlText" select="substring($remainder,string-length($beforeClose)+2)"/>
					</xsl:call-template>
				</xsl:if>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$xmlText"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet> 
