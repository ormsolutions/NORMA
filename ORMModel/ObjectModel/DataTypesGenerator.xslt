<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet
	version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX">
	<!-- Indenting is useful for debugging the transform, but a waste of memory at generation time -->
	<xsl:output method="xml" encoding="utf-8" indent="no"/>
	<xsl:param name="CustomToolNamespace" select="'TestNamespace'"/>
	<xsl:template match="DataTypes">
		<plx:root xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX">
			<plx:namespaceImport name="System"/>
			<plx:namespace name="{$CustomToolNamespace}">
				<plx:leadingInfo>
					<plx:comment>Common Public License Copyright Notice</plx:comment>
					<plx:comment>/**************************************************************************\</plx:comment>
					<plx:comment>* Neumont Object Role Modeling Architect for Visual Studio                 *</plx:comment>
					<plx:comment>*                                                                          *</plx:comment>
					<plx:comment>* Copyright © Neumont University. All rights reserved.                     *</plx:comment>
					<plx:comment>*                                                                          *</plx:comment>
					<plx:comment>* The use and distribution terms for this software are covered by the      *</plx:comment>
					<plx:comment>* Common Public License 1.0 (http://opensource.org/licenses/cpl) which     *</plx:comment>
					<plx:comment>* can be found in the file CPL.txt at the root of this distribution.       *</plx:comment>
					<plx:comment>* By using this software in any fashion, you are agreeing to be bound by   *</plx:comment>
					<plx:comment>* the terms of this license.                                               *</plx:comment>
					<plx:comment>*                                                                          *</plx:comment>
					<plx:comment>* You must not remove this notice, or any other, from this software.       *</plx:comment>
					<plx:comment>\**************************************************************************/</plx:comment>
				</plx:leadingInfo>
				<plx:enum name="PortableDataType" visibility="public">
					<plx:leadingInfo>
						<plx:pragma type="region" data="PortableDataType Enum"/>
						<plx:docComment>
							<summary>A list of predefined data types. One DataType-derived class is defined for each value.</summary>
						</plx:docComment>
					</plx:leadingInfo>
					<plx:trailingInfo>
						<plx:pragma type="closeRegion" data="PortableDataType Enum"/>
					</plx:trailingInfo>
					<plx:attribute dataTypeName="CLSCompliant">
						<plx:passParam>
							<plx:trueKeyword/>
						</plx:passParam>
					</plx:attribute>
					<xsl:for-each select="DataType">
						<xsl:variable name="dataTypeName" select="@name"/>
						<xsl:for-each select="SubType">
							<plx:enumItem name="{$dataTypeName}{@name}">
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
							</plx:enumItem>
						</xsl:for-each>
					</xsl:for-each>
				</plx:enum>
				<plx:class name="ORMModel" visibility="public" partial="true">
					<plx:leadingInfo>
						<plx:pragma type="region" data="Load-time fixup listeners"/>
					</plx:leadingInfo>
					<plx:trailingInfo>
						<plx:pragma type="closeRegion" data="Load-time fixup listeners"/>
					</plx:trailingInfo>
					<plx:class name="AddIntrinsicDataTypesFixupListener" visibility="private" partial="true" modifier="sealed">
						<plx:field name="typeArray" dataTypeName="Type" dataTypeIsSimpleArray="true" static="true" visibility="private">
							<plx:initialize>
								<plx:callNew dataTypeName="Type" dataTypeIsSimpleArray="true">
									<plx:arrayInitializer>
										<xsl:for-each select="DataType">
											<xsl:variable name="dataTypeName" select="@name"/>
											<xsl:if test="not(@enumOnly)">
												<xsl:for-each select="SubType">
													<plx:passParam type="in">
														<plx:typeOf dataTypeName="{@name}{$dataTypeName}DataType" />
													</plx:passParam>
												</xsl:for-each>
											</xsl:if>
										</xsl:for-each>
									</plx:arrayInitializer>
								</plx:callNew>
							</plx:initialize>
						</plx:field>
					</plx:class>
				</plx:class>
			</plx:namespace>
			<plx:namespace name="{$CustomToolNamespace}">
				<plx:leadingInfo>
					<plx:pragma type="region" data="Bind data types to enums and localized names"/>
				</plx:leadingInfo>
				<plx:trailingInfo>
					<plx:pragma type="closeRegion" data="Bind data types to enums and localized names"/>
				</plx:trailingInfo>
				<xsl:for-each select="DataType">
					<xsl:variable name="dataTypeName" select="@name"/>
					<xsl:if test="not(@enumOnly)">
						<xsl:for-each select="SubType">
							<xsl:variable name="subTypeName" select="@name"/>
							<plx:class name="{$subTypeName}{$dataTypeName}DataType" partial="true" visibility="public">
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
								<plx:property name="PortableDataType" modifier="override" visibility="public">
									<plx:leadingInfo>
										<plx:docComment>
											<summary>PortableDataType enum value for this type</summary>
										</plx:docComment>
									</plx:leadingInfo>
									<plx:returns dataTypeName="PortableDataType"/>
									<plx:get>
										<plx:return>
											<plx:callStatic name="{$dataTypeName}{$subTypeName}" dataTypeName="PortableDataType" type="field"/>
										</plx:return>
									</plx:get>
								</plx:property>
								<plx:function modifier="override" name="ToString" visibility="public">
									<plx:leadingInfo>
										<plx:docComment>
											<summary>Localized data type name</summary>
										</plx:docComment>
									</plx:leadingInfo>
									<plx:returns dataTypeName=".string"/>
									<plx:return>
										<plx:callStatic name="PortableDataType{$dataTypeName}{$subTypeName}" dataTypeName="ResourceStrings" type="property"/>
									</plx:return>
								</plx:function>
							</plx:class>
						</xsl:for-each>
					</xsl:if>
				</xsl:for-each>
			</plx:namespace>
		</plx:root>
	</xsl:template>
</xsl:stylesheet>
