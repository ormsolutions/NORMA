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
<xsl:stylesheet
	version="1.0"
	xmlns:rsg="http://schemas.neumont.edu/ORM/SDK/ResourceStringsGenerator"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:exsl="http://exslt.org/common"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX">
	<xsl:param name="ProjectNamespace" select="'TestNamespace'"/>
	<xsl:param name="CustomToolNamespace" select="'TestNamespace'"/>
	<!-- Indenting is useful for debugging the transform, but a waste of memory at generation time -->
	<xsl:output method="xml" encoding="utf-8" indent="no"/>
	<xsl:template match="rsg:ResourceStrings">
		<xsl:variable name="resxCacheFragment">
			<xsl:apply-templates select="rsg:FileAssociations/rsg:FileAssociation" mode="CacheResx"/>
		</xsl:variable>
		<xsl:variable name="resxCache" select="exsl:node-set($resxCacheFragment)/child::*"/>
		<plx:root xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX">
			<plx:namespace name="{$ProjectNamespace}">
				<xsl:if test="@UseProjectNamespace='0' or @UseProjectNamespace='false'">
					<xsl:attribute name="name">
						<xsl:value-of select="$CustomToolNamespace"/>
					</xsl:attribute>
				</xsl:if>
				<xsl:variable name="copyright" select="rsg:Copyright"/>
				<xsl:if test="$copyright">
					<plx:leadingInfo>
						<plx:comment blankLine="true"/>
						<plx:comment>
							<xsl:value-of select="$copyright/@name"/>
						</plx:comment>
						<xsl:for-each select="$copyright/rsg:CopyrightLine">
							<plx:comment>
								<xsl:value-of select="."/>
							</plx:comment>
						</xsl:for-each>
						<plx:comment blankLine="true"/>
					</plx:leadingInfo>
				</xsl:if>
				<xsl:variable name="classNameFragment">
					<xsl:choose>
						<xsl:when test="string(@ClassName)">
							<xsl:value-of select="@ClassName"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>ResourceStrings</xsl:text>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="className" select="string($classNameFragment)"/>
				<plx:class name="{$className}" partial="true" visibility="internal">
					<plx:leadingInfo>
						<plx:pragma type="region" data="{$className} class"/>
						<plx:docComment>
							<summary>A helper class to insulate the rest of the code from direct resource manipulation.</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:trailingInfo>
						<plx:pragma type="closeRegion" data="{$className} class"/>
					</plx:trailingInfo>
					<xsl:for-each select="rsg:ResourceString">
						<xsl:variable name="resxDoc" select="$resxCache[@model=current()/@model]"/>
						<xsl:variable name="resxData" select="$resxDoc/rsg:ResxData[@name=current()/@resourceName]"/>
						<xsl:if test="not($resxData)">
							<xsl:message terminate="yes">
								<xsl:text>Not corresponding value for "</xsl:text>
								<xsl:value-of select="@resourceName"/>
								<xsl:text>" in resource file "</xsl:text>
								<xsl:value-of select="../rsg:FileAssociations/rsg:FileAssociation[@model=current()/@model]/@resourceFile"/>
								<xsl:text>"</xsl:text>
							</xsl:message>
						</xsl:if>
						<plx:property name="{@name}" modifier="static" visibility="public">
							<xsl:variable name="cacheComment" select="string($resxData)"/>
							<xsl:variable name="localComment" select="string(rsg:comment)"/>
							<xsl:variable name="commentFragment">
								<xsl:variable name="commentPreference" select="string($resxDoc/@useFileComments)"/>
								<xsl:choose>
									<xsl:when test="$commentPreference='never'">
										<xsl:value-of select="$localComment"/>
									</xsl:when>
									<xsl:when test="$commentPreference='secondary'">
										<xsl:choose>
											<xsl:when test="$localComment">
												<xsl:value-of select="$localComment"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="$cacheComment"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="$cacheComment">
												<xsl:value-of select="$cacheComment"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="$localComment"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<xsl:variable name="comment" select="string($commentFragment)"/>
							<xsl:if test="$comment">
								<plx:leadingInfo>
									<plx:docComment>
										<summary>
											<xsl:value-of select="$comment"/>
										</summary>
									</plx:docComment>
								</plx:leadingInfo>
							</xsl:if>
							<plx:returns dataTypeName=".string"/>
							<plx:get>
								<plx:return>
									<plx:callStatic name="GetString" dataTypeName="{$className}">
										<plx:passParam type="in">
											<plx:callStatic name="{@model}" dataTypeName="ResourceManagers" type="field"/>
										</plx:passParam>
										<plx:passParam type="in">
											<plx:string>
												<xsl:value-of select="@resourceName"/>
											</plx:string>
										</plx:passParam>
									</plx:callStatic>
								</plx:return>
							</plx:get>
						</plx:property>
					</xsl:for-each>
				</plx:class>
			</plx:namespace>
		</plx:root>
	</xsl:template>
	<xsl:template match="rsg:FileAssociation" mode="CacheResx" xmlns="">
		<xsl:variable name="resx" select="document(@resourceFile)"/>
		<xsl:variable name="loadComments" select="not(@useFileComments='never')"/>
		<rsg:ResxCache model="{@model}" useFileComments="{@useFileComments}">
			<xsl:for-each select="$resx/root/data">
				<rsg:ResxData name="{@name}" xml:space="default">
					<xsl:if test="$loadComments">
						<xsl:value-of select="comment"/>
					</xsl:if>
				</rsg:ResxData>
			</xsl:for-each>
		</rsg:ResxCache>
	</xsl:template>
</xsl:stylesheet>
