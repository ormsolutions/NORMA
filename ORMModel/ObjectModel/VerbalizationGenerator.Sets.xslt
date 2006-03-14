<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:ve="http://schemas.neumont.edu/ORM/SDK/Verbalization"
	xmlns:exsl="http://exslt.org/common"
	extension-element-prefixes="exsl">

	<xsl:output method="xml" encoding="utf-8" indent="no"/>

	<!-- This file is designed as an include in VerbalizationGenerator.xslt. The CustomToolNamespace,
			 VerbalizationTextSnippetType, VerbalizationSet, and VerbalizationSets params are used in
			 this file and defined in the containing file. They are redefined here (along with a root template)
			 to ease development and debugging of this file without the container. -->
	<!--<xsl:param name="CustomToolNamespace" select="'TestNamespace'"/>
    <xsl:param name="VerbalizationTextSnippetType" select="'CoreVerbalizationTextSnippetType'"/>
    <xsl:param name="VerbalizationSet" select="'VerbalizationSet'"/>
    <xsl:param name="VerbalizationSets" select="'VerbalizationSets'"/>
    <xsl:param name="CoreVerbalizationSets" select="'CoreVerbalizationSets'"/>
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
		<xsl:variable name="SortedSnippetsFragment">
			<xsl:call-template name="GenerateSortedSnippetsFragment"/>
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
			<plx:attribute dataTypeName="CLSCompliant">
				<plx:passParam>
					<plx:trueKeyword/>
				</plx:passParam>
			</plx:attribute>
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
		<plx:interface name="I{$VerbalizationSets}"  visibility="public">
			<plx:leadingInfo>
				<plx:pragma type="region" data="I{$VerbalizationSets} interface"/>
				<plx:docComment>
					<summary>
						<xsl:text>A base class for the generic </xsl:text>
						<xsl:value-of select="$VerbalizationSets"/>
						<xsl:text> class.</xsl:text>
					</summary>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="I{$VerbalizationSets} interface"/>
			</plx:trailingInfo>
		</plx:interface>
		<plx:class name="{$VerbalizationSets}" modifier="abstract" visibility="public">
			<plx:leadingInfo>
				<plx:pragma type="region" data="Generic {$VerbalizationSets} class"/>
				<plx:docComment>
					<summary>
						<xsl:text>A generic class containing one </xsl:text>
						<xsl:value-of select="$VerbalizationSet"/>
						<xsl:text> structure for each combination of {alethic,deontic} and {positive,negative} snippets.</xsl:text>
					</summary>
					<typeparam name="EnumType">
						<xsl:text>The enumeration type of snippet set</xsl:text>
					</typeparam>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="Generic {$VerbalizationSets} class"/>
			</plx:trailingInfo>
			<plx:typeParam name="EnumType" requireValueType="1"/>
			<plx:implementsInterface dataTypeName="I{$VerbalizationSets}"/>
			<!-- Spit the VerbalizationSet structure -->
			<plx:class name="{$VerbalizationSet}" visibility="protected" modifier="abstract">
				<plx:leadingInfo>
					<plx:pragma type="region" data="{$VerbalizationSet} class"/>
					<plx:docComment>
						<summary>
							<xsl:text>An abstract class holding an array of strings. Strings are retrieved with values from </xsl:text>
							<xsl:value-of select="$VerbalizationTextSnippetType"/>
							<xsl:text>.</xsl:text>
						</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:trailingInfo>
					<plx:pragma type="closeRegion" data="{$VerbalizationSet} class"/>
				</plx:trailingInfo>
				<!--<plx:field name="mySnippets" visibility="private" dataTypeName=".string" dataTypeIsSimpleArray="true"/>
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
                </plx:function>-->
				<plx:function name="GetSnippet" visibility="public" modifier="abstract">
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
							<param name="owner">The <xsl:value-of select="$VerbalizationSets"/> object that is the owner of the snippet sets.</param>
							<returns>Snippet string</returns>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:param name="snippetType" dataTypeName="EnumType"/>
					<plx:param name="owner" dataTypeName="{$VerbalizationSets}">
						<plx:passTypeParam dataTypeName="EnumType"/>
					</plx:param>
					<plx:returns dataTypeName=".string"/>
					<!--<plx:return>
                        <plx:callInstance name=".implied" type="arrayIndexer">
                            <plx:callObject>
                                <plx:callThis name="mySnippets" type="field"/>
                            </plx:callObject>
                            <plx:passParam>
                                <plx:callInstance name="ValueToIndex">
                                    <plx:callObject>
                                        <plx:nameRef name="owner"/>
                                    </plx:callObject>
                                    <plx:passParam>
                                        <plx:nameRef name="snippetType"/>
                                    </plx:passParam>
                                </plx:callInstance>
                            </plx:passParam>
                        </plx:callInstance>
                    </plx:return>-->
				</plx:function>
			</plx:class>





			<plx:class name="{$ArrayVerbalizationSet}" visibility="protected">
				<plx:leadingInfo>
					<plx:pragma type="region" data="{$ArrayVerbalizationSet} class"/>
					<plx:docComment>
						<summary>
							<xsl:text>A class holding an array of strings. Strings are retrieved with values from </xsl:text>
							<xsl:value-of select="$VerbalizationTextSnippetType"/>
							<xsl:text>.</xsl:text>
						</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:trailingInfo>
					<plx:pragma type="closeRegion" data="{$ArrayVerbalizationSet} class"/>
				</plx:trailingInfo>
				<plx:derivesFromClass dataTypeName="{$VerbalizationSet}"/>
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
				<plx:function name="GetSnippet" visibility="public" modifier="override">
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
							<param name="owner">The <xsl:value-of select="$VerbalizationSets"/> object that is the owner of the snippet sets.</param>
							<returns>Snippet string</returns>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:param name="snippetType" dataTypeName="EnumType"/>
					<plx:param name="owner" dataTypeName="{$VerbalizationSets}">
						<plx:passTypeParam dataTypeName="EnumType"/>
					</plx:param>
					<plx:returns dataTypeName=".string"/>
					<plx:return>
						<plx:callInstance name=".implied" type="arrayIndexer">
							<plx:callObject>
								<plx:callThis name="mySnippets" type="field"/>
							</plx:callObject>
							<plx:passParam>
								<plx:callInstance name="ValueToIndex">
									<plx:callObject>
										<plx:nameRef name="owner"/>
									</plx:callObject>
									<plx:passParam>
										<plx:nameRef name="snippetType"/>
									</plx:passParam>
								</plx:callInstance>
							</plx:passParam>
						</plx:callInstance>
					</plx:return>
				</plx:function>
			</plx:class>
			<plx:class name="{$DictionaryVerbalizationSet}" visibility="protected">
				<plx:leadingInfo>
					<plx:pragma type="region" data="{$DictionaryVerbalizationSet} class"/>
					<plx:docComment>
						<summary>
							<xsl:text>A class holding dictionary items that refer to values from the enumeration of </xsl:text>
							<xsl:value-of select="$VerbalizationTextSnippetType"/>
							<xsl:text>.</xsl:text>
						</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:trailingInfo>
					<plx:pragma type="closeRegion" data="{$DictionaryVerbalizationSet} class"/>
				</plx:trailingInfo>
				<plx:derivesFromClass dataTypeName="{$VerbalizationSet}"/>
				<plx:field name="mySnippets" visibility="private" dataTypeName="Dictionary">
					<plx:passTypeParam dataTypeName="EnumType"/>
					<plx:passTypeParam dataTypeName="string"/>
				</plx:field>
				<plx:property name="Dictionary" visibility="public">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>Retrieves all of the IDictionary snippets in the snippet set</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:returns dataTypeName="IDictionary">
						<plx:passTypeParam dataTypeName="EnumType"/>
						<plx:passTypeParam dataTypeName="string"/>
					</plx:returns>
					<plx:get>
						<plx:return>
							<plx:nameRef name="mySnippets"/>
						</plx:return>
					</plx:get>
				</plx:property>
				<plx:function name=".construct"  visibility="public">
					<plx:leadingInfo>
						<plx:docComment>
							<summary>
								<xsl:value-of select="$VerbalizationSet"/>
								<xsl:text> constructor.</xsl:text>
							</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:assign>
						<plx:left>
							<plx:callThis name="mySnippets" type="field"/>
						</plx:left>
						<plx:right>
							<plx:callNew dataTypeName="Dictionary">
								<plx:passTypeParam dataTypeName="EnumType"/>
								<plx:passTypeParam dataTypeName="string"/>
							</plx:callNew>
						</plx:right>
					</plx:assign>
				</plx:function>
				<plx:function name="GetSnippet" visibility="public" modifier="override">
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
							<param name="owner">The <xsl:value-of select="$VerbalizationSets"/> object that is the owner of the snippet sets.</param>
							<returns>Snippet string</returns>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:param name="snippetType" dataTypeName="EnumType"/>
					<plx:param name="owner" dataTypeName="{$VerbalizationSets}">
						<plx:passTypeParam dataTypeName="EnumType"/>
					</plx:param>
					<plx:returns dataTypeName=".string"/>
					<plx:local name="retVal" dataTypeName=".string">
						<plx:initialize>
							<plx:nullKeyword/>
						</plx:initialize>
					</plx:local>
					<plx:callInstance name="TryGetValue">
						<plx:callObject>
							<plx:callThis name="mySnippets" type="field"/>
						</plx:callObject>
						<plx:passParam>
							<plx:nameRef name="snippetType"/>
						</plx:passParam>
						<plx:passParam type="out">
							<plx:nameRef name="retVal"/>
						</plx:passParam>
					</plx:callInstance>
					<plx:return>
						<plx:nameRef name="retVal"/>
					</plx:return>
				</plx:function>
			</plx:class>
			<plx:field name="mySets" visibility="private" dataTypeName="{$VerbalizationSet}" dataTypeIsSimpleArray="true"/>
			<plx:function name="GetSnippet" visibility="public">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>Retrieve a snippet for the specified type with default criteria.</summary>
						<param name="snippetType">
							<xsl:text>A value from the </xsl:text>
							<xsl:value-of select="$VerbalizationTextSnippetType"/>
							<xsl:text> enum representing the snippet string to retrieve.</xsl:text>
						</param>
						<returns>Snippet string</returns>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:param name="snippetType" dataTypeName="EnumType"/>
				<plx:returns dataTypeName=".string"/>
				<plx:return>
					<plx:callThis name="GetSnippet">
						<plx:passParam>
							<plx:nameRef name="snippetType"/>
						</plx:passParam>
						<plx:passParam>
							<plx:falseKeyword/>
						</plx:passParam>
						<plx:passParam>
							<plx:falseKeyword/>
						</plx:passParam>
					</plx:callThis>
				</plx:return>
			</plx:function>
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
				<plx:param name="snippetType" dataTypeName="EnumType"/>
				<plx:param name="isDeontic" dataTypeName=".boolean"/>
				<plx:param name="isNegative" dataTypeName=".boolean"/>
				<plx:returns dataTypeName=".string"/>
				<plx:local name="set" dataTypeName="{$VerbalizationSet}">
					<plx:initialize>
						<plx:callInstance name=".implied" type="arrayIndexer">
							<plx:callObject>
								<plx:callThis name="mySets" type="field"/>
							</plx:callObject>
							<plx:passParam>
								<plx:callStatic name="GetSetIndex" type="methodCall" dataTypeName="{$VerbalizationSets}">
									<plx:passTypeParam dataTypeName="EnumType"/>
									<plx:passParam>
										<plx:nameRef name="isDeontic"/>
									</plx:passParam>
									<plx:passParam>
										<plx:nameRef name="isNegative"/>
									</plx:passParam>
								</plx:callStatic>
							</plx:passParam>
						</plx:callInstance>
					</plx:initialize>
				</plx:local>
				<plx:branch>
					<plx:condition>
						<plx:binaryOperator type="identityInequality">
							<plx:left>
								<plx:nameRef name="set"/>							
							</plx:left>
							<plx:right>
								<plx:nullKeyword/>
							</plx:right>
						</plx:binaryOperator>
					</plx:condition>
					<plx:return>
						<plx:callInstance name="GetSnippet">
							<plx:callObject>
								<plx:nameRef name="set"/>
							</plx:callObject>
							<plx:passParam>
								<plx:nameRef name="snippetType" type="parameter"/>
							</plx:passParam>
							<plx:passParam>
								<plx:thisKeyword/>
							</plx:passParam>
						</plx:callInstance>
					</plx:return>
				</plx:branch>
				<plx:fallbackBranch>
					<plx:return>
						<plx:nullKeyword/>
					</plx:return>
				</plx:fallbackBranch>
			</plx:function>
			<plx:function name="GetSetIndex" modifier="static" visibility="protected">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>Get the snippet index of the deontic/negative VerbalizationSet</summary>
						<param name="isDeontic">Set to true to retrieve the snippet for a deontic verbalization, false for alethic.</param>
						<param name="isNegative">Set to true to retrieve the snippet for a negative reading, false for positive.</param>
						<returns>0-based index</returns>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:param name="isDeontic" dataTypeName=".boolean"/>
				<plx:param name="isNegative" dataTypeName=".boolean"/>
				<plx:returns dataTypeName=".i4"/>
				<plx:local name="setIndex" dataTypeName=".i4">
					<plx:initialize>
						<plx:value data="0" type="i4"/>
					</plx:initialize>
				</plx:local>
				<plx:branch>
					<plx:condition>
						<plx:nameRef name="isDeontic" type="parameter"/>
					</plx:condition>
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
										<plx:value data="1" type="i4"/>
									</plx:right>
								</plx:binaryOperator>
						</plx:right>
					</plx:assign>
				</plx:branch>
				<plx:branch>
					<plx:condition>
						<plx:nameRef name="isNegative" type="parameter"/>
					</plx:condition>
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
										<plx:value data="2" type="i4"/>
									</plx:right>
								</plx:binaryOperator>
						</plx:right>
					</plx:assign>
				</plx:branch>
				<plx:return>
					<plx:nameRef name="setIndex"/>
				</plx:return>
			</plx:function>
			<plx:function modifier="abstract" name="PopulateVerbalizationSets" visibility="protected">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>
							<xsl:text>Method to populate verbalization sets of an abstract </xsl:text>
							<xsl:value-of select="$VerbalizationSets"/>
							<xsl:text> object.</xsl:text>
						</summary>
						<param name="sets">
							<xsl:text>The empty verbalization sets to be populated</xsl:text>
						</param>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:param name="sets" dataTypeIsSimpleArray="true" dataTypeName="VerbalizationSet"/>
			</plx:function>
			<plx:function modifier="abstract" name="ValueToIndex" visibility="protected">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>Method to convert enum value to integer index value</summary>
						<param name="enumValue">
							<xsl:text>The enum value to be converted</xsl:text>
						</param>
						<returns>integer value of enum type</returns>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:param name="enumValue" dataTypeName="EnumType"/>
				<plx:returns dataTypeName=".i4"/>
			</plx:function>
			<plx:function modifier="static" name="Create" visibility="public">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>
							<xsl:text>Creates an instance of the </xsl:text>
							<xsl:value-of select="$VerbalizationSets"/>
							<xsl:text> class calling the PopulateVerbalizationSets method.</xsl:text>
						</summary>
						<typeparam name="DerivedType">Name of class to instantiate that derives from <xsl:value-of select="$VerbalizationSets"/>.</typeparam>
						<returns>Returns a generic {$VerbalizationSets} object with snippet sets</returns>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:typeParam name="DerivedType" requireDefaultConstructor="1">
					<plx:typeConstraint dataTypeName="{$VerbalizationSets}">
						<plx:passTypeParam dataTypeName="EnumType"/>
					</plx:typeConstraint>
				</plx:typeParam>
				<plx:returns dataTypeName="{$VerbalizationSets}">
					<plx:passTypeParam dataTypeName="EnumType"/>
				</plx:returns>
				<plx:local name="retVal" dataTypeName="{$VerbalizationSets}">
					<plx:passTypeParam dataTypeName="EnumType"/>
					<plx:initialize>
						<plx:callNew dataTypeName="DerivedType"/>
					</plx:initialize>
				</plx:local>
				<plx:local name="newSets" dataTypeName="{$VerbalizationSet}" dataTypeIsSimpleArray="1">
					<plx:initialize>
						<plx:callNew dataTypeName="VerbalizationSet" dataTypeIsSimpleArray="1">
							<plx:passParam>
								<plx:value data="4" type="i4"/>
							</plx:passParam>
						</plx:callNew>
					</plx:initialize>
				</plx:local>
				<plx:callInstance name="PopulateVerbalizationSets" type="methodCall">
					<plx:callObject>
						<plx:nameRef name="retVal"/>
					</plx:callObject>
					<plx:passParam>
						<plx:nameRef name="newSets"/>
					</plx:passParam>
				</plx:callInstance>
				<plx:assign>
					<plx:left>
						<plx:callInstance name="mySets" type="property">
							<plx:callObject>
								<plx:nameRef name="retVal"/>
							</plx:callObject>
						</plx:callInstance>
					</plx:left>
					<plx:right>
						<plx:nameRef name="newSets"/>
					</plx:right>
				</plx:assign>
				<plx:return>
					<plx:nameRef name="retVal"/>
				</plx:return>
			</plx:function>
		</plx:class>
		<!-- Spit the VerbalizationSets class -->
		<plx:class name="{$CoreVerbalizationSets}" visibility="public">
			<plx:leadingInfo>
				<plx:pragma type="region" data="{$CoreVerbalizationSets} class"/>
				<plx:docComment>
					<summary>
						<xsl:text>A class derving from </xsl:text>
						<xsl:value-of select="$VerbalizationSets"/>
						<xsl:text>.</xsl:text>
					</summary>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:trailingInfo>
				<plx:pragma type="closeRegion" data="{$CoreVerbalizationSets} class"/>
			</plx:trailingInfo>
			<plx:attribute dataTypeName="CLSCompliant">
				<plx:passParam>
					<plx:trueKeyword/>
				</plx:passParam>
			</plx:attribute>
			<plx:derivesFromClass dataTypeName="{$VerbalizationSets}">
				<plx:passTypeParam dataTypeName="{$VerbalizationTextSnippetType}"/>
			</plx:derivesFromClass>
			<plx:field name="Default" visibility="public" readOnly="true" static="true" dataTypeName="{$CoreVerbalizationSets}">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>The default verbalization snippet set. Contains english HTML snippets.</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:initialize>
					<plx:cast dataTypeName="{$CoreVerbalizationSets}">
						<plx:callStatic name="Create" dataTypeName="{$VerbalizationSets}">
							<plx:passTypeParam dataTypeName="{$VerbalizationTextSnippetType}"/>
							<plx:passMemberTypeParam dataTypeName="{$CoreVerbalizationSets}"/>
						</plx:callStatic>
					</plx:cast>
				</plx:initialize>
			</plx:field>
			<plx:function visibility="protected" modifier="override" name="PopulateVerbalizationSets">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>Populates the snippet sets of the <xsl:value-of select="$CoreVerbalizationSets"/> object.</summary>
						<param name="sets">The sets to be populated.</param>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:param name="sets" dataTypeName="VerbalizationSet" dataTypeIsSimpleArray="1"/>
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
						<xsl:call-template name="VerbalizationSetPassParam">
							<xsl:with-param name="SnippetsFragment">
								<xsl:call-template name="MatchSnippetSet">
									<xsl:with-param name="SortedSnippets" select="$SortedSnippets"/>
									<xsl:with-param name="Modality" select="'deontic'"/>
									<xsl:with-param name="Sign" select="'positive'"/>
								</xsl:call-template>
							</xsl:with-param>
						</xsl:call-template>
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
						<xsl:call-template name="VerbalizationSetPassParam">
							<xsl:with-param name="SnippetsFragment">
								<xsl:call-template name="MatchSnippetSet">
									<xsl:with-param name="SortedSnippets" select="$SortedSnippets"/>
									<xsl:with-param name="Modality" select="'alethic'"/>
									<xsl:with-param name="Sign" select="'negative'"/>
								</xsl:call-template>
							</xsl:with-param>
						</xsl:call-template>
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
						<xsl:call-template name="VerbalizationSetPassParam">
							<xsl:with-param name="SnippetsFragment">
								<xsl:call-template name="MatchSnippetSet">
									<xsl:with-param name="SortedSnippets" select="$SortedSnippets"/>
									<xsl:with-param name="Modality" select="'deontic'"/>
									<xsl:with-param name="Sign" select="'negative'"/>
								</xsl:call-template>
							</xsl:with-param>
						</xsl:call-template>
					</plx:right>
				</plx:assign>
			</plx:function>
			<plx:function name="ValueToIndex" modifier="override" visibility="protected">
				<plx:leadingInfo>
					<plx:docComment>
						<summary>Converts enum value of <xsl:value-of select="$VerbalizationTextSnippetType"/> to an integer index value.</summary>
					</plx:docComment>
				</plx:leadingInfo>
				<plx:param dataTypeName="{$VerbalizationTextSnippetType}" name="enumValue"/>
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
		<plx:callNew dataTypeName="{$ArrayVerbalizationSet}">
			<plx:passParam>
				<plx:callNew dataTypeName=".string" dataTypeIsSimpleArray="true">
					<plx:arrayInitializer>
						<xsl:for-each select="exsl:node-set($SnippetsFragment)/child::*">
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
