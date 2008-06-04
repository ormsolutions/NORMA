<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:exsl="http://exslt.org/common"
	xmlns:oil="http://schemas.orm.net/OIAL"
	xmlns:ormdt="http://schemas.orm.net/ORMDataTypes"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:prop="urn:schemas-orm-net:PLiX:CLI:Properties"
	xmlns:xs="http://www.w3.org/2001/XMLSchema"
	exclude-result-prefixes="oil ormdt"
	extension-element-prefixes="exsl">

	<!--<xsl:import href="OIALtoPLiX_GlobalSupportFunctions.xslt"/>-->
	<!--<xsl:import href="../../DIL/Transforms/DILSupportFunctions.xslt"/>-->
	<xsl:output method="xml" encoding="utf-8" media-type="text/xml" indent="yes"/>
	<xsl:param name="OIAL" select="/*"/>
	<xsl:variable name="ConceptTypes" select="$OIAL//oil:conceptType"/>

<xsl:template match="/">
	<plx:root>
		<plx:namespace name="PHP">
			<xsl:for-each select="$ConceptTypes">
				<xsl:variable name="CurrentConceptType" select="."/>
				<!-- Many-to-One Proxies -->
				<xsl:for-each select="oil:conceptTypeRef">
					<xsl:call-template name="GenerateProxy">
						<xsl:with-param name="thisClassName" select="$CurrentConceptType/@name"/>
					</xsl:call-template>
				</xsl:for-each>
				<!-- One-To-Many Proxies -->
				<xsl:for-each select="$OIAL//oil:conceptType/oil:conceptTypeRef[@target=$CurrentConceptType/@name]">
					<xsl:call-template name="GenerateProxy">
						<xsl:with-param name="thisClassName" select="$CurrentConceptType/@name"/>
						<xsl:with-param name="isCollection" select="true()"/>
					</xsl:call-template>
				</xsl:for-each>
			</xsl:for-each>
		</plx:namespace>
	</plx:root>
</xsl:template>

	<xsl:template name="GenerateProxy">
		<xsl:param name="thisClassName"/>
		<xsl:param name="isCollection" select="false()"/>
		<plx:class visibility="public">
			<xsl:attribute name="name">
				<xsl:choose>
					<xsl:when test="not($isCollection)">
						<xsl:value-of select="@oppositeName"/>
						<xsl:text>_</xsl:text>
						<xsl:value-of select="@name"/>
						<xsl:text>_</xsl:text>
						<xsl:value-of select="@target"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$thisClassName"/>
						<xsl:text>_</xsl:text>
						<xsl:value-of select="@name"/>
						<xsl:text>_</xsl:text>
						<xsl:value-of select="@oppositeName"/>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:text>_Proxy</xsl:text>
			</xsl:attribute>
			<plx:leadingInfo>
				<plx:docComment>
					<summary>
						<xsl:text>Class used to proxy a </xsl:text>
						<xsl:choose>
							<xsl:when test="not($isCollection)">
								<xsl:value-of select="@target"/>
								<xsl:text> for the role </xsl:text>
								<xsl:value-of select="@name"/>
								<xsl:text> for use inside of a </xsl:text>
								<xsl:value-of select="../@name"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="@target"/>
								<xsl:text> for the role </xsl:text>
								<xsl:value-of select="@name"/>
								<xsl:text> for use inside of a </xsl:text>
								<xsl:value-of select="$thisClassName"/>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:text> isCollection: </xsl:text>
						<xsl:value-of select="$isCollection"/>
					</summary>
				</plx:docComment>
			</plx:leadingInfo>
			<plx:field name="ref" visibility="private" dataTypeName=".object">
				<plx:initialize>
					<plx:nullKeyword/>
				</plx:initialize>
			</plx:field>
			<plx:field name="value" visibility="private" dataTypeName=".object">
				<plx:initialize>
					<plx:nullKeyword/>
				</plx:initialize>
			</plx:field>
			<plx:function visibility="public" name=".construct">
				<plx:param name="ref" dataTypeName=".object"/>
				<plx:assign>
					<plx:left>
						<plx:callThis accessor="this" name="ref" type="field"/>
					</plx:left>
					<plx:right>
						<plx:nameRef name="ref" type="local"/>
					</plx:right>
				</plx:assign>
			</plx:function>
			<plx:function name="get" visibility="public">
				<plx:branch>
					<plx:condition>
						<plx:unaryOperator type="booleanNot">
							<plx:callStatic dataTypeName=".global" name="isset" type="methodCall">
								<plx:passParam>
									<plx:callThis accessor="this" name="value" type="field"/>
								</plx:passParam>
							</plx:callStatic>
						</plx:unaryOperator>
					</plx:condition>
					<plx:assign>
						<plx:left>
							<plx:callThis accessor="this" name="value" type="field"/>
						</plx:left>
						<plx:right>
							<xsl:choose>
								<xsl:when test="not($isCollection)">
									<plx:callInstance name="getSingle" type="methodCall">
										<plx:callObject>
											<plx:callStatic name="Instance" type="property" dataTypeName="{$thisClassName}DAO"/>
										</plx:callObject>
										<xsl:variable name="uniqueInformationTypesFragment">
											<xsl:apply-templates select="$ConceptTypes[@name=current()/@target]" mode="RecursiveGetPreferredUniqueness"/>
										</xsl:variable>
										<xsl:variable name="uniqueInformationTypes" select="exsl:node-set($uniqueInformationTypesFragment)"/>
										<xsl:for-each select="$uniqueInformationTypes/oil:informationType">
											<plx:passParam>
												<plx:callInstance name="{@name}" type="property">
													<plx:callObject>
														<plx:callThis accessor="this" name="ref" type="field"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:passParam>
										</xsl:for-each>
									</plx:callInstance>
								</xsl:when>
								<xsl:otherwise>
									<!--<plx:function name="get_{@oppositeName}_Collection_By_{@name}" modifier="static" visibility="public">-->
									<plx:callInstance name="get_{@oppositeName}_Collection_By_{@name}" type="methodCall">
										<plx:callObject>
											<plx:callStatic name="Instance" type="property" dataTypeName="{$thisClassName}DAO"/>
										</plx:callObject>
										<xsl:variable name="uniqueInformationTypesFragment">
											<xsl:apply-templates select="$ConceptTypes[@name=current()/@target]" mode="RecursiveGetPreferredUniqueness"/>
										</xsl:variable>
										<xsl:variable name="uniqueInformationTypes" select="exsl:node-set($uniqueInformationTypesFragment)"/>
										<xsl:for-each select="$uniqueInformationTypes/oil:informationType">
											<plx:passParam>
												<plx:callInstance name="{@name}" type="property">
													<plx:callObject>
														<plx:callThis accessor="this" name="ref" type="field"/>
													</plx:callObject>
												</plx:callInstance>
											</plx:passParam>
										</xsl:for-each>
									</plx:callInstance>
								</xsl:otherwise>
							</xsl:choose>
						</plx:right>
					</plx:assign>
				</plx:branch>
				<plx:return>
					<plx:callThis accessor="this" name="value" type="field"/>
				</plx:return>
			</plx:function>
		</plx:class>
	</xsl:template>
	<!-- Recursively retrieves oil:informationTypes for the preferred identification scheme of a given ConceptType -->
	<xsl:template match="oil:conceptType" mode="RecursiveGetPreferredUniqueness">
		<xsl:choose>
			<xsl:when test="parent::oil:conceptType and (not(oil:informationType/oil:singleRoleUniquenessConstraint[@isPreferred='true']) and not(oil:roleSequenceUniquenessConstraint[@isPreferred='true']))">
				<xsl:apply-templates select="parent::oil:conceptType" mode="RecursiveGetPreferredUniqueness"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:for-each select="child::*[oil:singleRoleUniquenessConstraint/@isPreferred='true' or @isPreferred='true']">
					<xsl:apply-templates select="." mode="GetPreferredUniquenessInformationTypes"/>
				</xsl:for-each>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="oil:informationType" mode="GetPreferredUniquenessInformationTypes">
		<xsl:copy-of select="."/>
	</xsl:template>
	<xsl:template match="oil:roleSequenceUniquenessConstraint" mode="GetPreferredUniquenessInformationTypes">
		<xsl:for-each select="oil:roleSequence/oil:typeRef">
			<xsl:variable name="targetConceptType" select="@targetConceptType"/>
			<xsl:variable name="targetChild" select="@targetChild"/>
			<xsl:apply-templates select="$OIAL//oil:conceptType[@name=$targetConceptType]/child::*[@name=$targetChild]" mode="GetPreferredUniquenessInformationTypes"/>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="oil:conceptTypeRef" mode="GetPreferredUniquenessInformationTypes">
		<xsl:apply-templates select="$OIAL//oil:conceptType[@name=current()/@target]" mode="RecursiveGetPreferredUniqueness"/>
	</xsl:template>
	<!-- End preferred identification scheme of a given ConceptType -->
</xsl:stylesheet>