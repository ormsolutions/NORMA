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
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:exsl="http://exslt.org/common"
	extension-element-prefixes="exsl">
	<!-- Indenting is useful for debugging the transform, but a waste of memory at generation time -->
	<xsl:output method="xml" encoding="utf-8" indent="no"/>
	<xsl:param name="CustomToolNamespace" select="'TestNamespace'"/>
	<xsl:template match="DataTypes">
		<plx:root xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX">
			<plx:namespaceImport name="System"/>
			<plx:namespaceImport name="System.Diagnostics"/>
			<plx:namespace name="{$CustomToolNamespace}">
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
					<xsl:variable name="dataTypeCanCompareFalse" select="@canCompare='false' or @canCompare='0'"/>
					<xsl:variable name="dataTypeRangeSupportFragment">
						<xsl:choose>
							<xsl:when test="string-length(@rangeSupport)">
								<xsl:value-of select="@rangeSupport"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>Closed</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="dataTypeRangeSupport" select="string($dataTypeRangeSupportFragment)"/>
					<xsl:variable name="dataTypeBackingType" select="string(@backingType)"/>
					<xsl:if test="not(@enumOnly)">
						<xsl:for-each select="SubType">
							<xsl:variable name="subTypeName" select="@name"/>
							<xsl:variable name="subTypeCanCompareFalse" select="not(@canCompare='true' or @canCompare='1') and ($dataTypeCanCompareFalse or (@canCompare='false' or @canCompare='0'))"/>
							<xsl:variable name="subTypeRangeSupportFragment">
								<xsl:choose>
									<xsl:when test="string-length(@rangeSupport)">
										<xsl:value-of select="@rangeSupport"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$dataTypeRangeSupport"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<xsl:variable name="subTypeRangeSupport" select="string($subTypeRangeSupportFragment)"/>
							<xsl:variable name="subTypeBackingTypeFragment">
								<xsl:variable name="backingType" select="string(@backingType)"/>
								<xsl:choose>
									<xsl:when test="string-length($backingType)">
										<xsl:value-of select="$backingType"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$dataTypeBackingType"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<xsl:variable name="subTypeBackingType" select="string($subTypeBackingTypeFragment)"/>
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
								<xsl:if test="$subTypeCanCompareFalse">
									<plx:property name="CanCompare" modifier="override" visibility="public">
										<plx:leadingInfo>
											<plx:docComment>
												<summary>The data type does not support comparison</summary>
											</plx:docComment>
										</plx:leadingInfo>
										<plx:returns dataTypeName=".boolean"/>
										<plx:get>
											<plx:return>
												<plx:falseKeyword/>
											</plx:return>
										</plx:get>
									</plx:property>
									<plx:function name="Compare" modifier="override" visibility="public">
										<plx:leadingInfo>
											<plx:docComment>
												<summary>CanCompare is false. Compare asserts if called.</summary>
											</plx:docComment>
										</plx:leadingInfo>
										<plx:param name="value1" dataTypeName=".string"/>
										<plx:param name="value2" dataTypeName=".string"/>
										<plx:returns dataTypeName=".i4"/>
										<plx:callStatic name="Fail" dataTypeName="Debug">
											<plx:passParam>
												<plx:string>Don't call Compare if CanParse returns false</plx:string>
											</plx:passParam>
										</plx:callStatic>
										<plx:return>
											<plx:value data="0" type="i4"/>
										</plx:return>
									</plx:function>
								</xsl:if>
								<plx:property name="RangeSupport" modifier="override" visibility="public">
									<plx:leadingInfo>
										<plx:docComment>
											<summary>The data type supports '<xsl:value-of select="$subTypeRangeSupport"/>' ranges</summary>
										</plx:docComment>
									</plx:leadingInfo>
									<plx:returns dataTypeName="DataTypeRangeSupport"/>
									<plx:get>
										<plx:return>
											<plx:callStatic name="{$subTypeRangeSupport}" dataTypeName="DataTypeRangeSupport" type="field"/>
										</plx:return>
									</plx:get>
								</plx:property>
								<xsl:if test="string-length($subTypeBackingType)">
									<xsl:if test="not($subTypeBackingType='.string')">
										<plx:function name="CanParse" modifier="override" visibility="public">
											<plx:leadingInfo>
												<plx:docComment>
													<summary>Returns true if the string value can be interpreted as this data type</summary>
												</plx:docComment>
											</plx:leadingInfo>
											<plx:param name="value" dataTypeName=".string"/>
											<plx:returns dataTypeName=".boolean"/>
											<plx:local name="result" dataTypeName="{$subTypeBackingType}"/>
											<plx:return>
												<plx:callStatic name="TryParse" dataTypeName="{$subTypeBackingType}">
													<plx:passParam>
														<plx:nameRef name="value" type="parameter"/>
													</plx:passParam>
													<plx:passParam type="out">
														<plx:nameRef name="result"/>
													</plx:passParam>
												</plx:callStatic>
											</plx:return>
										</plx:function>
										<plx:property name="CanParseAnyValue" visibility="public" modifier="override">
											<plx:leadingInfo>
												<plx:docComment>
													<summary>Returns false, meaning that CanParse can fail for some values</summary>
												</plx:docComment>
											</plx:leadingInfo>
											<plx:returns dataTypeName=".boolean"/>
											<plx:get>
												<plx:return>
													<plx:falseKeyword/>
												</plx:return>
											</plx:get>
										</plx:property>
									</xsl:if>
									<plx:function name="Compare" modifier="override" visibility="public">
										<plx:leadingInfo>
											<plx:docComment>
												<summary>Compare two values. Each value should be checked previously with CanParse</summary>
											</plx:docComment>
										</plx:leadingInfo>
										<plx:param name="value1" dataTypeName=".string"/>
										<plx:param name="value2" dataTypeName=".string"/>
										<plx:returns dataTypeName=".i4"/>
										<xsl:choose>
											<xsl:when test="$subTypeBackingType='.string'">
												<plx:return>
													<plx:callInstance name="CompareTo">
														<plx:callObject>
															<plx:nameRef name="value1" type="parameter"/>
														</plx:callObject>
														<plx:passParam>
															<plx:nameRef name="value2" type="parameter"/>
														</plx:passParam>
													</plx:callInstance>
												</plx:return>
											</xsl:when>
											<xsl:otherwise>
												<xsl:variable name="OneAndTwo">
													<Number>1</Number>
													<Number>2</Number>
												</xsl:variable>
												<xsl:for-each select="exsl:node-set($OneAndTwo)/child::*">
													<!-- Assert precondition -->
													<xsl:variable name="currentNumber" select="string(.)"/>
													<plx:callStatic name="Assert" dataTypeName="Debug">
														<plx:passParam>
															<plx:callThis name="CanParse">
																<plx:passParam>
																	<plx:nameRef name="value{$currentNumber}" type="parameter"/>
																</plx:passParam>
															</plx:callThis>
														</plx:passParam>
														<plx:passParam>
															<plx:string>Don't call Compare if CanParse(value<xsl:value-of select="$currentNumber"/>) returns false</plx:string>
														</plx:passParam>
													</plx:callStatic>
													<!-- Get the typed value -->
													<plx:local name="typedValue{$currentNumber}" dataTypeName="{$subTypeBackingType}"/>
													<plx:callStatic name="TryParse" dataTypeName="{$subTypeBackingType}">
														<plx:passParam>
															<plx:nameRef name="value{$currentNumber}" type="parameter"/>
														</plx:passParam>
														<plx:passParam type="out">
															<plx:nameRef name="typedValue{$currentNumber}"/>
														</plx:passParam>
													</plx:callStatic>
													<!-- End of OneAndTwo-->
												</xsl:for-each>
												<xsl:choose>
													<xsl:when test="$subTypeRangeSupport='None'">
														<plx:branch>
															<plx:condition>
																<plx:callInstance name="Equals">
																	<plx:callObject>
																		<plx:cast dataTypeName="IEquatable">
																			<plx:passTypeParam dataTypeName="{$subTypeBackingType}"/>
																			<plx:nameRef name="typedValue1" type="parameter"/>
																		</plx:cast>
																	</plx:callObject>
																	<plx:passParam>
																		<plx:nameRef name="typedValue2" type="parameter"/>
																	</plx:passParam>
																</plx:callInstance>
															</plx:condition>
															<plx:return>
																<plx:value data="0" type="i4"/>
															</plx:return>
														</plx:branch>
														<plx:return>
															<plx:value data="1" type="i4"/>
														</plx:return>
													</xsl:when>
													<xsl:otherwise>
														<plx:return>
															<plx:callInstance name="CompareTo">
																<plx:callObject>
																	<plx:cast dataTypeName="IComparable">
																		<plx:passTypeParam dataTypeName="{$subTypeBackingType}"/>
																		<plx:nameRef name="typedValue1" type="parameter"/>
																	</plx:cast>
																</plx:callObject>
																<plx:passParam>
																	<plx:nameRef name="typedValue2" type="parameter"/>
																</plx:passParam>
															</plx:callInstance>
														</plx:return>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:otherwise>
										</xsl:choose>
									</plx:function>
								</xsl:if>
							</plx:class>
						</xsl:for-each>
					</xsl:if>
				</xsl:for-each>
			</plx:namespace>
		</plx:root>
	</xsl:template>
</xsl:stylesheet>
