<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:ve="http://schemas.neumont.edu/ORM/SDK/Verbalization"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
>
	<!-- This file is designed as an include in VerbalizationGenerator.xslt. The CustomToolNamespace,
			 VerbalizationTextSnippetType, VerbalizationSet, and VerbalizationSets params are used in
			 this file and defined in the containing file. They are redefined here (along with a root template)
			 to ease development and debugging of this file without the container. -->
	<!--<xsl:param name="CustomToolNamespace" select="'TestNamespace'"/>
	<xsl:param name="VerbalizationTextSnippetType" select="'VerbalizationTextSnippetType'"/>
	<xsl:param name="VerbalizationSet" select="'VerbalizationSet'"/>
	<xsl:param name="VerbalizationSets" select="'VerbalizationSets'"/>
	<xsl:template match="ve:VerbalizationRoot">
		<plx:root>
			<plx:namespace name="{$CustomToolNamespace}">
				<xsl:call-template name="GenerateVerbalizationSets"/>
			</plx:namespace>
		</plx:root>
	</xsl:template>-->

	<!-- Template to sort and normalize ve:Snippet templates for the default (en-US) language. Called from
	     the ve:VerbalizationRoot node.-->
	<xsl:template name="GenerateSortedSnippetsFragment">
		<xsl:for-each select="ve:Languages/ve:Language[@xml:lang='en-US']/ve:Snippets/ve:Snippet">
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
						<xsl:for-each select="msxsl:node-set($matchFragment)/child::*">
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
									<xsl:with-param name="Match" select="msxsl:node-set($matchFragment)/child::*"/>
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
		<xsl:variable name="SortedSnippetsFragment">
			<xsl:call-template name="GenerateSortedSnippetsFragment"/>
		</xsl:variable>
		<xsl:variable name="SortedSnippets" select="msxsl:node-set($SortedSnippetsFragment)/child::*"/>
		<!-- We're using the alethic positive snippet set twice (once for the text,
		     once to generate the enum values, so go ahead and cache it -->
		<xsl:variable name="alethicPositiveFragment">
			<xsl:call-template name="MatchSnippetSet">
				<xsl:with-param name="SortedSnippets" select="$SortedSnippets"/>
				<xsl:with-param name="Modality" select="'alethic'"/>
				<xsl:with-param name="Sign" select="'positive'"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="alethicPositive" select="msxsl:node-set($alethicPositiveFragment)/child::*"/>

		<!-- Spit an enum of all snippet types -->
		<plx:enum visibility="public" name="{$VerbalizationTextSnippetType}">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$VerbalizationTextSnippetType} enum"/>
				<plx:docComment>
					<summary>An enum with one value for each recognized snippet</summary>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{$VerbalizationTextSnippetType} enum"/>
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

		<!-- Spit the VerbalizationSet structure -->
		<plx:structure name="{$VerbalizationSet}" visibility="public">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$VerbalizationSet} structure"/>
				<plx:docComment>
					<summary>
						<xsl:text>A structure holding an array of strings. Strings are retrieved with values from </xsl:text>
						<xsl:value-of select="$VerbalizationTextSnippetType"/>
						<xsl:text>.</xsl:text>
					</summary>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{$VerbalizationSet} structure"/>
			</plx:trailingInfo>
			<plx:field name="mySnippets" visibility="private" dataTypeName=".string" dataTypeIsSimpleArray="true"/>
			<plx:function name=".construct"  visibility="public">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>
							<xsl:value-of select="$VerbalizationSet"/>
							<xsl:text> constructor.</xsl:text>
						</summary>
						<param name="snippets">
							<xsl:text>An array of strings with one string for each value in the </xsl:text>
							<xsl:value-of select="$VerbalizationTextSnippetType"/>
							<xsl:text> enum.</xsl:text>
						</param>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:param name="snippets" dataTypeName=".string" dataTypeIsSimpleArray="true"/>
				<plx:assign>
					<plx:left>
						<plx:callThis name="mySnippets" type="field"/>
					</plx:left>
					<plx:right>
						<plx:nameRef type="parameter" name="snippets"/>
					</plx:right>
				</plx:assign>
			</plx:function>
			<plx:function name="GetSnippet" visibility="public">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>
							<xsl:text>Retrieve a snippet value</xsl:text>
						</summary>
						<param name="snippetType">
							<xsl:text>A value from the </xsl:text>
							<xsl:value-of select="$VerbalizationTextSnippetType"/>
							<xsl:text> enum representing the snippet string to retrieve.</xsl:text>
						</param>
						<returns>Snippet string</returns>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:param name="snippetType" dataTypeName="{$VerbalizationTextSnippetType}"/>
				<plx:returns dataTypeName=".string"/>
				<plx:return>
					<plx:callInstance name=".implied" type="arrayIndexer">
						<plx:callObject>
							<plx:callThis name="mySnippets" type="field"/>
						</plx:callObject>
						<plx:passParam>
							<plx:cast type="unbox" dataTypeName=".i4">
								<plx:nameRef type="parameter" name="snippetType"/>
							</plx:cast>
						</plx:passParam>
					</plx:callInstance>
				</plx:return>
			</plx:function>
		</plx:structure>

		<!-- Spit the VerbalizationSets class -->
		<plx:class name="{$VerbalizationSets}" visibility="public">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$VerbalizationSets} class"/>
				<plx:docComment>
					<summary>
						<xsl:text>A class containing one </xsl:text>
						<xsl:value-of select="$VerbalizationSet"/>
						<xsl:text> structure for each combination of {alethic,deontic} and {positive,negative} snippets.</xsl:text>
					</summary>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{$VerbalizationSets} class"/>
			</plx:trailingInfo>
			<plx:field name="Default" visibility="public" readOnly="true" static="true" dataTypeName="{$VerbalizationSets}">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>The default verbalization snippet set. Contains english HTML snippets.</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:initialize>
					<plx:callStatic name="CreateDefault{$VerbalizationSets}" dataTypeName="{$VerbalizationSets}"/>
				</plx:initialize>
			</plx:field>
			<plx:field name="mySets" visibility="private" dataTypeName="{$VerbalizationSet}" dataTypeIsSimpleArray="true"/>
			<plx:function name=".construct" visibility="private"/>
			<plx:function name="GetSnippet" visibility="public">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>Retrieve a snippet for the specified type and criteria.</summary>
						<param name="snippetType">
							<xsl:text>A value from the </xsl:text>
							<xsl:value-of select="$VerbalizationTextSnippetType"/>
							<xsl:text> enum representing the snippet string to retrieve.</xsl:text>
						</param>
						<param name="isDeontic">Set to true to retrieve the snippet for a deontic verbalization, false for alethic.</param>
						<param name="isNegative">Set to true to retrieve the snippet for a negative reading, false for positive.</param>
						<returns>Snippet string</returns>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:param name="snippetType" dataTypeName="{$VerbalizationTextSnippetType}"/>
				<plx:param name="isDeontic" dataTypeName=".boolean"/>
				<plx:param name="isNegative" dataTypeName=".boolean"/>
				<plx:returns dataTypeName=".string"/>
				<plx:local name="setIndex" dataTypeName=".i4">
					<plx:initialize>
						<plx:value type="i4" data="0"/>
					</plx:initialize>
				</plx:local>
				<plx:branch>
					<plx:condition>
						<plx:nameRef type="parameter" name="isDeontic"/>
					</plx:condition>
					<plx:body>
						<plx:assign>
								<plx:left>
								<plx:nameRef name="setIndex"/>
							</plx:left>
							<plx:right>
								<plx:binaryOperator type="add">
									<plx:left>
										<plx:nameRef name="setIndex"/>
									</plx:left>
									<plx:right>
										<plx:value type="i4" data="1"/>
									</plx:right>
								</plx:binaryOperator>
							</plx:right>
						</plx:assign>
					</plx:body>
				</plx:branch>
				<plx:branch>
					<plx:condition>
						<plx:nameRef type="parameter" name="isNegative"/>
					</plx:condition>
					<plx:body>
						<plx:assign>
								<plx:left>
								<plx:nameRef name="setIndex"/>
							</plx:left>
							<plx:right>
								<plx:binaryOperator type="add">
									<plx:left>
										<plx:nameRef name="setIndex"/>
									</plx:left>
									<plx:right>
										<plx:value type="i4" data="2"/>
									</plx:right>
								</plx:binaryOperator>
							</plx:right>
						</plx:assign>
					</plx:body>
				</plx:branch>
				<plx:return>
					<plx:callInstance name="GetSnippet">
						<plx:callObject>
							<plx:callInstance name=".implied" type="arrayIndexer">
								<plx:callObject>
									<plx:callThis name="mySets" type="field"/>
								</plx:callObject>
								<plx:passParam>
									<plx:nameRef name="setIndex"/>
								</plx:passParam>
							</plx:callInstance>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef type="parameter" name="snippetType"/>
						</plx:passParam>
					</plx:callInstance>
				</plx:return>
			</plx:function>

			<!-- Pull in a default verbalization set from the verbalization file -->
			<plx:function name="CreateDefault{$VerbalizationSets}" visibility="private" modifier="static">
				<plx:returns dataTypeName="{$VerbalizationSets}"/>
				<plx:local name="retVal" dataTypeName="{$VerbalizationSets}">
					<plx:initialize>
						<plx:callNew dataTypeName="{$VerbalizationSets}"/>
					</plx:initialize>
				</plx:local>
				<plx:assign>
						<plx:left>
						<plx:callInstance type="field" name="mySets">
							<plx:callObject>
								<plx:nameRef name="retVal"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:callNew dataTypeName="{$VerbalizationSet}" dataTypeIsSimpleArray="true">
							<plx:arrayInitializer>
								<xsl:call-template name="VerbalizationSetPassParam">
									<xsl:with-param name="SnippetsFragment" select="$alethicPositiveFragment"/>
								</xsl:call-template>
								<xsl:call-template name="VerbalizationSetPassParam">
									<xsl:with-param name="SnippetsFragment">
										<xsl:call-template name="MatchSnippetSet">
											<xsl:with-param name="SortedSnippets" select="$SortedSnippets"/>
											<xsl:with-param name="Modality" select="'deontic'"/>
											<xsl:with-param name="Sign" select="'positive'"/>
										</xsl:call-template>
									</xsl:with-param>
								</xsl:call-template>
								<xsl:call-template name="VerbalizationSetPassParam">
									<xsl:with-param name="SnippetsFragment">
										<xsl:call-template name="MatchSnippetSet">
											<xsl:with-param name="SortedSnippets" select="$SortedSnippets"/>
											<xsl:with-param name="Modality" select="'alethic'"/>
											<xsl:with-param name="Sign" select="'negative'"/>
										</xsl:call-template>
									</xsl:with-param>
								</xsl:call-template>
								<xsl:call-template name="VerbalizationSetPassParam">
									<xsl:with-param name="SnippetsFragment">
										<xsl:call-template name="MatchSnippetSet">
											<xsl:with-param name="SortedSnippets" select="$SortedSnippets"/>
											<xsl:with-param name="Modality" select="'deontic'"/>
											<xsl:with-param name="Sign" select="'negative'"/>
										</xsl:call-template>
									</xsl:with-param>
								</xsl:call-template>
							</plx:arrayInitializer>
						</plx:callNew>
					</plx:right>
				</plx:assign>
				<plx:return>
					<plx:nameRef name="retVal"/>
				</plx:return>
			</plx:function>
		</plx:class>
	</xsl:template>
	<!-- Spit a new verbalizationset with values initialized from the passed in sorted snippets fragment -->
	<xsl:template name="VerbalizationSetPassParam">
		<xsl:param name="SnippetsFragment"/>
		<plx:passParam>
			<plx:callNew dataTypeName="{$VerbalizationSet}">
				<plx:passParam>
					<plx:callNew dataTypeName=".string" dataTypeIsSimpleArray="true">
						<plx:arrayInitializer>
							<xsl:for-each select="msxsl:node-set($SnippetsFragment)/child::*">
								<plx:passParam>
									<plx:string>
										<xsl:value-of select="@text"/>
									</plx:string>
								</plx:passParam>
							</xsl:for-each>
						</plx:arrayInitializer>
					</plx:callNew>
				</plx:passParam>
			</plx:callNew>
		</plx:passParam>
	</xsl:template>
	<xsl:template name="CountReplacementFields">
		<!-- A quick and dirty routine to count the number of replacement fields
			 in a format string. Used in documentation comments. -->
		<xsl:param name="FormatString"/>
		<xsl:variable name="total">
			<xsl:call-template name="CountReplacementFields2">
				<xsl:with-param name="FormatString" select="$FormatString"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:value-of select="string-length($total)"/>
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
					<xsl:text>x</xsl:text>
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
								<xsl:text>x</xsl:text>
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