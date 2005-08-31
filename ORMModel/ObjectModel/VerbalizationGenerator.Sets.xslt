<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:plx="http://Schemas.Neumont.edu/CodeGeneration/Plix"
    xmlns:ve="http://Schemas.Neumont.edu/ORM/SDK/Verbalization"
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
	<xsl:template match="ve:Root">
		<plx:Root>
			<plx:Namespace name="{$CustomToolNamespace}">
				<xsl:call-template name="GenerateVerbalizationSets"/>
			</plx:Namespace>
		</plx:Root>
	</xsl:template>-->

	<!-- Template to sort and normalize ve:FormalItem templates for the default (en-us) language. Called from
	     the ve:Root node.-->
	<xsl:template name="GenerateSortedSnippetsFragment">
		<xsl:for-each select="ve:Languages/ve:Language[@code='en-us']/ve:FormalItems/ve:FormalItem">
			<xsl:sort select="@type" data-type="text" order="ascending"/>
			<xsl:sort select="@modality" data-type="text" order="ascending"/>
			<!-- We want +/-, not -/+, so that we don't have to resort after explicitly adding the default values -->
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
						<xsl:text>+</xsl:text>
					</xsl:attribute>
				</xsl:if>
				<xsl:copy-of select="child::*"/>
			</xsl:copy>
		</xsl:for-each>
	</xsl:template>
	<!-- Template to get a list of <Snippet type="" text=""/> elements for the
	     specified Modality and Sign. If the exact modality and sign is not specified, then
		 match as many values as possible. Go ahead and include strings without a perfect match
		 in the current set. -->
	<xsl:template name="MatchSnippetSet">
		<!-- All ve:FormalItem elements from the default (us-english) language sorted
		     by type/modality/sign with all default modality and sign values filled in.
			 Pass the child elements of the fragment returned from GeneratedSortedSnippetsFragment
			 to this parameter. -->
		<xsl:param name="SortedSnippets"/>
		<xsl:param name="Modality" select="'alethic'"/>
		<xsl:param name="Sign" select="'+'"/>
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
						<xsl:when test="@sign='+' and @modality='alethic'">
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
							<Snippet type="{@type}" text="{ve:Form[@style='Basic']}"/>
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
						<Snippet type="{@type}" text="{ve:Form[@style='Basic']}"/>
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
				<xsl:with-param name="Sign" select="'+'"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="alethicPositive" select="msxsl:node-set($alethicPositiveFragment)/child::*"/>

		<!-- Spit an enum of all snippet types -->
		<plx:Enum visibility="Public" name="{$VerbalizationTextSnippetType}">
			<xsl:for-each select="$alethicPositive">
				<plx:EnumItem name="{@type}"/>
			</xsl:for-each>
		</plx:Enum>

		<!-- Spit the VerbalizationSet structure -->
		<plx:Structure name="{$VerbalizationSet}" visibility="Public">
			<plx:Field name="mySnippets" visibility="Private" dataTypeName="String" dataTypeQualifier="System" dataTypeIsSimpleArray="true"/>
			<plx:Function ctor="true" visibility="Public">
				<plx:Param name="snippets" dataTypeName="String" dataTypeQualifier="System" dataTypeIsSimpleArray="true"/>
				<plx:Operator type="Assign">
					<plx:Left>
						<plx:CallInstance name="mySnippets" type="Field">
							<plx:CallObject>
								<plx:ThisKeyword/>
							</plx:CallObject>
						</plx:CallInstance>
					</plx:Left>
					<plx:Right>
						<plx:Value type="Parameter" data="snippets"/>
					</plx:Right>
				</plx:Operator>
			</plx:Function>
			<plx:Function name="GetSnippet" visibility="Public">
				<plx:Param name="" type="RetVal" dataTypeName="String" dataTypeQualifier="System"/>
				<plx:Param name="snippetType" dataTypeName="{$VerbalizationTextSnippetType}"/>
				<plx:Return>
					<plx:CallInstance name="" type="ArrayIndexer">
						<plx:CallObject>
							<plx:CallInstance name="mySnippets" type="Field">
								<plx:CallObject>
									<plx:ThisKeyword/>
								</plx:CallObject>
							</plx:CallInstance>
						</plx:CallObject>
						<plx:PassParam>
							<plx:Cast type="ObjectToValueType">
								<plx:TargetType dataTypeName="Int32" dataTypeQualifier="System"/>
								<plx:CastExpression>
									<plx:Value type="Parameter" data="snippetType"/>
								</plx:CastExpression>
							</plx:Cast>
						</plx:PassParam>
					</plx:CallInstance>
				</plx:Return>
			</plx:Function>
		</plx:Structure>

		<!-- Spit the VerbalizationSets class -->
		<plx:Class name="{$VerbalizationSets}" visibility="Public">
			<plx:Field name="Default" visibility="Public" readOnly="true" static="true" dataTypeName="{$VerbalizationSets}">
				<plx:Initialize>
					<plx:CallStatic name="CreateDefault{$VerbalizationSets}" dataTypeName="{$VerbalizationSets}"/>
				</plx:Initialize>
			</plx:Field>
			<plx:Field name="mySets" visibility="Private" dataTypeName="{$VerbalizationSet}" dataTypeIsSimpleArray="true"/>
			<plx:Function name="" ctor="true" visibility="Private"/>
			<plx:Function name="GetSnippet" visibility="Public">
				<plx:Param name="" type="RetVal" dataTypeName="String" dataTypeQualifier="System"/>
				<plx:Param name="snippetType" dataTypeName="{$VerbalizationTextSnippetType}"/>
				<plx:Param name="isDeontic" dataTypeName="Boolean" dataTypeQualifier="System"/>
				<plx:Param name="isNegative" dataTypeName="Boolean" dataTypeQualifier="System"/>
				<plx:Variable name="setIndex" dataTypeName="Int32" dataTypeQualifier="System">
					<plx:Initialize>
						<plx:Value type="I4" data="0"/>
					</plx:Initialize>
				</plx:Variable>
				<plx:Condition>
					<plx:Test>
						<plx:Value type="Parameter" data="isDeontic"/>
					</plx:Test>
					<plx:Body>
						<plx:Operator type="Assign">
							<plx:Left>
								<plx:Value type="Local" data="setIndex"/>
							</plx:Left>
							<plx:Right>
								<plx:Operator type="Add">
									<plx:Left>
										<plx:Value type="Local" data="setIndex"/>
									</plx:Left>
									<plx:Right>
										<plx:Value type="I4" data="1"/>
									</plx:Right>
								</plx:Operator>
							</plx:Right>
						</plx:Operator>
					</plx:Body>
				</plx:Condition>
				<plx:Condition>
					<plx:Test>
						<plx:Value type="Parameter" data="isNegative"/>
					</plx:Test>
					<plx:Body>
						<plx:Operator type="Assign">
							<plx:Left>
								<plx:Value type="Local" data="setIndex"/>
							</plx:Left>
							<plx:Right>
								<plx:Operator type="Add">
									<plx:Left>
										<plx:Value type="Local" data="setIndex"/>
									</plx:Left>
									<plx:Right>
										<plx:Value type="I4" data="2"/>
									</plx:Right>
								</plx:Operator>
							</plx:Right>
						</plx:Operator>
					</plx:Body>
				</plx:Condition>
				<plx:Return>
					<plx:CallInstance name="GetSnippet">
						<plx:CallObject>
							<plx:CallInstance name="" type="ArrayIndexer">
								<plx:CallObject>
									<plx:CallInstance name="mySets" type="Field">
										<plx:CallObject>
											<plx:ThisKeyword/>
										</plx:CallObject>
									</plx:CallInstance>
								</plx:CallObject>
								<plx:PassParam>
									<plx:Value type="Local" data="setIndex"/>
								</plx:PassParam>
							</plx:CallInstance>
						</plx:CallObject>
						<plx:PassParam>
							<plx:Value type="Parameter" data="snippetType"/>
						</plx:PassParam>
					</plx:CallInstance>
				</plx:Return>
			</plx:Function>

			<!-- Pull in a default verbalization set from the verbalization file -->
			<plx:Function name="CreateDefault{$VerbalizationSets}" visibility="Private" static="true">
				<plx:Param name="" type="RetVal" dataTypeName="{$VerbalizationSets}"/>
				<plx:Variable name="retVal" dataTypeName="{$VerbalizationSets}">
					<plx:Initialize>
						<plx:CallNew dataTypeName="{$VerbalizationSets}"/>
					</plx:Initialize>
				</plx:Variable>
				<plx:Operator type="Assign">
					<plx:Left>
						<plx:CallInstance type="Field" name="mySets">
							<plx:CallObject>
								<plx:Value type="Local" data="retVal"/>
							</plx:CallObject>
						</plx:CallInstance>
					</plx:Left>
					<plx:Right>
						<plx:CallNew dataTypeName="{$VerbalizationSet}" dataTypeIsSimpleArray="true">
							<plx:ArrayInitializer>
								<xsl:call-template name="VerbalizationSetPassParam">
									<xsl:with-param name="SnippetsFragment" select="$alethicPositiveFragment"/>
								</xsl:call-template>
								<xsl:call-template name="VerbalizationSetPassParam">
									<xsl:with-param name="SnippetsFragment">
										<xsl:call-template name="MatchSnippetSet">
											<xsl:with-param name="SortedSnippets" select="$SortedSnippets"/>
											<xsl:with-param name="Modality" select="'deontic'"/>
											<xsl:with-param name="Sign" select="'+'"/>
										</xsl:call-template>
									</xsl:with-param>
								</xsl:call-template>
								<xsl:call-template name="VerbalizationSetPassParam">
									<xsl:with-param name="SnippetsFragment">
										<xsl:call-template name="MatchSnippetSet">
											<xsl:with-param name="SortedSnippets" select="$SortedSnippets"/>
											<xsl:with-param name="Modality" select="'alethic'"/>
											<xsl:with-param name="Sign" select="'-'"/>
										</xsl:call-template>
									</xsl:with-param>
								</xsl:call-template>
								<xsl:call-template name="VerbalizationSetPassParam">
									<xsl:with-param name="SnippetsFragment">
										<xsl:call-template name="MatchSnippetSet">
											<xsl:with-param name="SortedSnippets" select="$SortedSnippets"/>
											<xsl:with-param name="Modality" select="'deontic'"/>
											<xsl:with-param name="Sign" select="'-'"/>
										</xsl:call-template>
									</xsl:with-param>
								</xsl:call-template>
							</plx:ArrayInitializer>
						</plx:CallNew>
					</plx:Right>
				</plx:Operator>
				<plx:Return>
					<plx:Value type="Local" data="retVal"/>
				</plx:Return>
			</plx:Function>
		</plx:Class>
	</xsl:template>
	<!-- Spit a new verbalizationset with values initialized from the passed in sorted snippets fragment -->
	<xsl:template name="VerbalizationSetPassParam">
		<xsl:param name="SnippetsFragment"/>
		<plx:PassParam>
			<plx:CallNew dataTypeName="{$VerbalizationSet}">
				<plx:PassParam>
					<plx:CallNew dataTypeName="String" dataTypeQualifier="System" dataTypeIsSimpleArray="true">
						<plx:ArrayInitializer>
							<xsl:for-each select="msxsl:node-set($SnippetsFragment)/child::*">
								<plx:PassParam>
									<plx:String>
										<xsl:value-of select="@text"/>
									</plx:String>
								</plx:PassParam>
							</xsl:for-each>
						</plx:ArrayInitializer>
					</plx:CallNew>
				</plx:PassParam>
			</plx:CallNew>
		</plx:PassParam>
	</xsl:template>
</xsl:stylesheet>