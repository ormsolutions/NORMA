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
	extension-element-prefixes="exsl"
	exclude-result-prefixes="ve">

	<xsl:output method="xml" encoding="utf-8" indent="no"/>

	<!-- This file is designed as an include in VerbalizationGenerator.xslt and other files
			.that generation default verbalization snippets. The containing template should generate
			 any namespace information, then defer to the GenerateVerbalizationSets template in this file.-->

	<!-- Template to sort and normalize ve:Snippet templates for the default (en-US) language. Called from
	     the ve:VerbalizationRoot node.-->
	<xsl:template name="GenerateSortedSnippetsFragment">
		<xsl:param name="SnippetsLocation"/>
		<xsl:for-each select="exsl:node-set(document($SnippetsLocation))/ve:Languages/ve:Language[@xml:lang='en-US']/ve:Snippets/ve:Snippet">
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
	<!-- Do the actual code generation for the verbalization classes -->
	<xsl:template name="GenerateVerbalizationSets">
		<xsl:param name="SnippetEnumTypeName"/>
		<xsl:param name="VerbalizationSetName"/>
		<xsl:param name="SnippetsLocation"/>
		<xsl:variable name="SortedSnippetsFragment">
			<xsl:call-template name="GenerateSortedSnippetsFragment">
				<xsl:with-param name="SnippetsLocation" select="$SnippetsLocation"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="SortedSnippets" select="exsl:node-set($SortedSnippetsFragment)/child::*"/>
		<!-- We're using the alethic positive snippet set twice (once for the text,
		     once to generate the enum values, so go ahead and cache it -->
		<xsl:variable name="alethicPositiveFragment">
			<xsl:call-template name="MatchSnippetSet">
				<xsl:with-param name="SortedSnippets" select="$SortedSnippets"/>
				<xsl:with-param name="Modality" select="'alethic'"/>
				<xsl:with-param name="Sign" select="'positive'"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="alethicPositive" select="exsl:node-set($alethicPositiveFragment)/child::*"/>

		<!-- Spit an enum of all snippet types -->
		<plx:enum visibility="public" name="{$SnippetEnumTypeName}">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$SnippetEnumTypeName} enum"/>
				<plx:docComment>
					<summary>An enum with one value for each recognized snippet</summary>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{$SnippetEnumTypeName} enum"/>
			</plx:trailingInfo>
			<xsl:for-each select="$alethicPositive">
				<plx:enumItem name="{@type}">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>
								<xsl:text>The '</xsl:text>
								<xsl:value-of select="@type"/>
								<xsl:text>' </xsl:text>
								<xsl:variable name="replacementCountFragment">
									<xsl:call-template name="CountReplacementFields">
										<xsl:with-param name="FormatString" select="@text"/>
									</xsl:call-template>
								</xsl:variable>
								<xsl:variable name="replacementCount" select="string($replacementCountFragment)"/>
								<xsl:choose>
									<xsl:when test="$replacementCount=0">
										<xsl:text>simple snippet value.</xsl:text>
									</xsl:when>
									<xsl:otherwise>
										<xsl:text>format string snippet. Contains </xsl:text>
										<xsl:value-of select="$replacementCount"/>
										<xsl:text> replacement field</xsl:text>
										<xsl:choose>
											<xsl:when test="$replacementCount=1">
												<xsl:text>.</xsl:text>
											</xsl:when>
											<xsl:otherwise>
												<xsl:text>s.</xsl:text>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>
							</summary>
						</plx:docComment>
					</plx:leadingInfo>
				</plx:enumItem>
			</xsl:for-each>
		</plx:enum>
		<plx:class name="{$VerbalizationSetName}" visibility="public">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$VerbalizationSetName} class"/>
				<plx:docComment>
					<summary>
						<xsl:text>A class deriving from VerbalizationSets.</xsl:text>
					</summary>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{$VerbalizationSetName} class"/>
			</plx:trailingInfo>
			<plx:derivesFromClass dataTypeName="VerbalizationSets">
				<plx:passTypeParam dataTypeName="{$SnippetEnumTypeName}"/>
			</plx:derivesFromClass>
			<plx:field name="Default" visibility="public" readOnly="true" static="true" dataTypeName="{$VerbalizationSetName}">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>The default verbalization snippet set. Contains english HTML snippets.</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:initialize>
					<plx:cast dataTypeName="{$VerbalizationSetName}">
						<plx:callStatic name="Create" dataTypeName="VerbalizationSets">
							<plx:passTypeParam dataTypeName="{$SnippetEnumTypeName}"/>
							<plx:passMemberTypeParam dataTypeName="{$VerbalizationSetName}"/>
							<plx:passParam>
								<plx:nullKeyword/>
							</plx:passParam>
						</plx:callStatic>
					</plx:cast>
				</plx:initialize>
			</plx:field>
			<plx:function visibility="protected" modifier="override" name="PopulateVerbalizationSets">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>Populates the snippet sets of the <xsl:value-of select="$VerbalizationSetName"/> object.</summary>
						<param name="sets">The sets to be populated.</param>
						<param name="userData">User-defined data passed to the Create method</param>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:param name="sets" dataTypeName="VerbalizationSet" dataTypeIsSimpleArray="1"/>
				<plx:param name="userData" dataTypeName=".object"/>
				<plx:assign>
					<plx:left>
						<plx:callInstance name=".implied" type="arrayIndexer">
							<plx:callObject>
								<plx:nameRef name="sets"/>
							</plx:callObject>
							<plx:passParam>
								<plx:value type="i4" data="0"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<xsl:call-template name="VerbalizationSetPassParam">
							<xsl:with-param name="SnippetsFragment" select="$alethicPositiveFragment"/>
						</xsl:call-template>
					</plx:right>
				</plx:assign>
				<xsl:variable name="identicalSets" select="count($SortedSnippets) = count($alethicPositive)"/>
				<plx:assign>
					<plx:left>
						<plx:callInstance name=".implied" type="arrayIndexer">
							<plx:callObject>
								<plx:nameRef name="sets"/>
							</plx:callObject>
							<plx:passParam>
								<plx:value type="i4" data="1"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<xsl:choose>
							<xsl:when test="$identicalSets">
								<plx:callInstance name=".implied" type="arrayIndexer">
									<plx:callObject>
										<plx:nameRef name="sets"/>
									</plx:callObject>
									<plx:passParam>
										<plx:value type="i4" data="0"/>
									</plx:passParam>
								</plx:callInstance>						
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="VerbalizationSetPassParam">
									<xsl:with-param name="SnippetsFragment">
										<xsl:call-template name="MatchSnippetSet">
											<xsl:with-param name="SortedSnippets" select="$SortedSnippets"/>
											<xsl:with-param name="Modality" select="'deontic'"/>
											<xsl:with-param name="Sign" select="'positive'"/>
										</xsl:call-template>
									</xsl:with-param>
								</xsl:call-template>
							</xsl:otherwise>
						</xsl:choose>
					</plx:right>
				</plx:assign>
				<plx:assign>
					<plx:left>
						<plx:callInstance name=".implied" type="arrayIndexer">
							<plx:callObject>
								<plx:nameRef name="sets"/>
							</plx:callObject>
							<plx:passParam>
								<plx:value type="i4" data="2"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<xsl:choose>
							<xsl:when test="$identicalSets">
								<plx:callInstance name=".implied" type="arrayIndexer">
									<plx:callObject>
										<plx:nameRef name="sets"/>
									</plx:callObject>
									<plx:passParam>
										<plx:value type="i4" data="0"/>
									</plx:passParam>
								</plx:callInstance>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="VerbalizationSetPassParam">
									<xsl:with-param name="SnippetsFragment">
										<xsl:call-template name="MatchSnippetSet">
											<xsl:with-param name="SortedSnippets" select="$SortedSnippets"/>
											<xsl:with-param name="Modality" select="'alethic'"/>
											<xsl:with-param name="Sign" select="'negative'"/>
										</xsl:call-template>
									</xsl:with-param>
								</xsl:call-template>
							</xsl:otherwise>
						</xsl:choose>
					</plx:right>
				</plx:assign>
				<plx:assign>
					<plx:left>
						<plx:callInstance name=".implied" type="arrayIndexer">
							<plx:callObject>
								<plx:nameRef name="sets"/>
							</plx:callObject>
							<plx:passParam>
								<plx:value type="i4" data="3"/>
							</plx:passParam>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<xsl:choose>
							<xsl:when test="$identicalSets">
								<plx:callInstance name=".implied" type="arrayIndexer">
									<plx:callObject>
										<plx:nameRef name="sets"/>
									</plx:callObject>
									<plx:passParam>
										<plx:value type="i4" data="0"/>
									</plx:passParam>
								</plx:callInstance>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="VerbalizationSetPassParam">
									<xsl:with-param name="SnippetsFragment">
										<xsl:call-template name="MatchSnippetSet">
											<xsl:with-param name="SortedSnippets" select="$SortedSnippets"/>
											<xsl:with-param name="Modality" select="'deontic'"/>
											<xsl:with-param name="Sign" select="'negative'"/>
										</xsl:call-template>
									</xsl:with-param>
								</xsl:call-template>
							</xsl:otherwise>
						</xsl:choose>
					</plx:right>
				</plx:assign>
			</plx:function>
			<plx:function name="ValueToIndex" modifier="override" visibility="protected">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>Converts enum value of <xsl:value-of select="$SnippetEnumTypeName"/> to an integer index value.</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:param dataTypeName="{$SnippetEnumTypeName}" name="enumValue"/>
				<plx:returns dataTypeName=".i4"/>
				<plx:return>
					<plx:cast dataTypeName=".i4">
						<plx:nameRef name="enumValue"/>
					</plx:cast>
				</plx:return>
			</plx:function>
		</plx:class>
	</xsl:template>
	<!-- Spit a new verbalizationset with values initialized from the passed in sorted snippets fragment -->
	<xsl:template name="VerbalizationSetPassParam">
		<xsl:param name="SnippetsFragment"/>
		<plx:callNew dataTypeName="ArrayVerbalizationSet">
			<plx:passParam>
				<plx:callNew dataTypeName=".string" dataTypeIsSimpleArray="true">
					<plx:arrayInitializer>
						<xsl:for-each select="exsl:node-set($SnippetsFragment)/child::*">
							<plx:string data="{@text}"/>
						</xsl:for-each>
					</plx:arrayInitializer>
				</plx:callNew>
			</plx:passParam>
		</plx:callNew>
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
</xsl:stylesheet>
