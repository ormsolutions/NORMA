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
<xsl:stylesheet
    version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX">
	<!-- Indenting is useful for debugging the transform, but a waste of memory at generation time -->
	<xsl:output method="xml" encoding="utf-8" indent="no"/>
	<xsl:template match="ResourceStrings">
		<plx:root xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX">
			<plx:namespace name="Neumont.Tools.ORM">
				<xsl:if test="Copyright">
					<plx:leadingInfo>
						<plx:comment blankLine="true"/>
						<plx:comment>
							<xsl:value-of select="Copyright/@name"/>
						</plx:comment>
						<xsl:for-each select="Copyright/CopyrightLine">
							<plx:comment>
								<xsl:value-of select="."/>
							</plx:comment>
						</xsl:for-each>
						<plx:comment blankLine="true"/>
					</plx:leadingInfo>
				</xsl:if>
				<plx:class name="ResourceStrings" partial="true" visibility="internal">
					<plx:leadingInfo>
						<plx:pragma type="region" data="ResourceStrings class"/>
						<plx:docComment>
							<summary>A helper class to insulate the rest of the code from direct resource manipulation.</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:trailingInfo>
						<plx:pragma type="closeRegion" data="ResourceStrings class"/>
					</plx:trailingInfo>
					<!--<xsl:for-each select="ResourceString">
						<plx:field const="true" visibility="private" dataTypeName=".string" name="{@name}_Id">
							<plx:initialize>
								<plx:string><xsl:value-of select="@resourceName"/></plx:string>
							</plx:initialize>
						</plx:field>
					</xsl:for-each>-->
					<xsl:for-each select="ResourceString">
						<plx:property name="{@name}" modifier="static" visibility="public">
							<xsl:if test="comment">
								<plx:leadingInfo>
									<plx:docComment>
										<xsl:for-each select="comment">
											<xsl:element name="{@type}">
												<xsl:value-of select="."/>
											</xsl:element>
										</xsl:for-each>
									</plx:docComment>
								</plx:leadingInfo>
							</xsl:if>
							<plx:returns dataTypeName=".string"/>
							<plx:get>
								<plx:return>
									<plx:callStatic name="GetString" dataTypeName="ResourceStrings">
										<plx:passParam type="in">
											<plx:callStatic name="{@model}" dataTypeName="ResourceManagers" type="field"/>
										</plx:passParam>
										<plx:passParam type="in">
											<plx:string>
												<xsl:value-of select="@resourceName"/>
											</plx:string>
											<!--<plx:callStatic name="{@name}_Id" dataTypeName="ResourceStrings" type="field"/>-->
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
</xsl:stylesheet>
