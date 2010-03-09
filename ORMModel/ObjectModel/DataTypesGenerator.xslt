<?xml version="1.0" encoding="utf-8"?>
<!--
	Natural Object-Role Modeling Architect for Visual Studio

	Copyright © Neumont University. All rights reserved.
	Copyright © ORM Solutions, LLC. All rights reserved.

	The use and distribution terms for this software are covered by the
	Common Public License 1.0 (http://opensource.org/licenses/cpl) which
	can be found in the file CPL.txt at the root of this distribution.
	By using this software in any fashion, you are agreeing to be bound by
	the terms of this license.

	You must not remove this notice, or any other, from this software.
-->
<xsl:stylesheet
	version="1.0"
	xmlns:dtg="http://schemas.ormsolutions.com/ORM/SDK/DataTypesGenerator" 
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX"
	xmlns:exsl="http://exslt.org/common"
	extension-element-prefixes="exsl">
	<!-- Indenting is useful for debugging the transform, but a waste of memory at generation time -->
	<xsl:output method="xml" encoding="utf-8" indent="no"/>
	<xsl:param name="CustomToolNamespace" select="'TestNamespace'"/>
	<xsl:template match="dtg:DataTypes">
		<plx:root xmlns:plx="http://schemas.neumont.edu/CodeGeneration/PLiX">
			<plx:namespaceImport name="System"/>
			<plx:namespaceImport name="System.Diagnostics"/>
			<plx:namespaceImport name="System.Globalization"/>
			<plx:namespace name="{$CustomToolNamespace}">
				<xsl:variable name="copyright" select="dtg:Copyright"/>
				<xsl:if test="$copyright">
					<plx:leadingInfo>
						<plx:comment blankLine="true"/>
						<plx:comment>
							<xsl:value-of select="$copyright/@name"/>
						</plx:comment>
						<xsl:for-each select="$copyright/dtg:CopyrightLine">
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
					<xsl:for-each select="dtg:DataTypeGroup">
						<xsl:variable name="groupName" select="string(@name)"/>
						<xsl:for-each select="dtg:DataType">
							<plx:enumItem name="{$groupName}{@name}">
								<xsl:variable name="comment" select="dtg:Comment"/>
								<xsl:if test="$comment">
									<plx:leadingInfo>
										<plx:docComment>
											<xsl:for-each select="$comment">
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
										<xsl:for-each select="dtg:DataTypeGroup">
											<xsl:variable name="groupName" select="@name"/>
											<xsl:if test="not(@enumOnly)">
												<xsl:for-each select="dtg:DataType">
													<plx:typeOf dataTypeName="{@name}{$groupName}DataType" />
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
				<xsl:for-each select="dtg:DataTypeGroup">
					<xsl:variable name="groupName" select="string(@name)"/>
					<xsl:variable name="groupCanCompareFalse" select="@canCompare='false' or @canCompare='0'"/>
					<xsl:variable name="groupRangeSupportFragment">
						<xsl:choose>
							<xsl:when test="@rangeSupport">
								<xsl:value-of select="@rangeSupport"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>ContinuousEndPoints</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="groupRangeSupport" select="string($groupRangeSupportFragment)"/>
					<xsl:variable name="groupBackingType" select="string(@backingType)"/>
					<xsl:variable name="groupDiscontinuousRangePattern" select="string(@discontinuousRangePattern)"/>
					<xsl:variable name="groupCultureSensitive" select="@cultureSensitive='true' or @cultureSensitive='1'"/>
					<xsl:if test="not(@enumOnly)">
						<xsl:for-each select="dtg:DataType">
							<xsl:variable name="typeName" select="string(@name)"/>
							<xsl:variable name="typeCanCompareFalse" select="not(@canCompare='true' or @canCompare='1') and ($groupCanCompareFalse or (@canCompare='false' or @canCompare='0'))"/>
							<xsl:variable name="typeRangeSupportFragment">
								<xsl:choose>
									<xsl:when test="@rangeSupport">
										<xsl:value-of select="@rangeSupport"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$groupRangeSupport"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<xsl:variable name="typeRangeSupport" select="string($typeRangeSupportFragment)"/>
							<xsl:variable name="typeBackingTypeFragment">
								<xsl:variable name="backingType" select="string(@backingType)"/>
								<xsl:choose>
									<xsl:when test="$backingType">
										<xsl:value-of select="$backingType"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$groupBackingType"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<xsl:variable name="typeBackingType" select="string($typeBackingTypeFragment)"/>
							<xsl:variable name="typeDiscontinuousRangePatternFragment">
								<xsl:variable name="pattern" select="string(@discontinuousRangePattern)"/>
								<xsl:choose>
									<xsl:when test="$pattern">
										<xsl:value-of select="$pattern"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$groupDiscontinuousRangePattern"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<xsl:variable name="typeDiscontinuousRangePattern" select="string($typeDiscontinuousRangePatternFragment)"/>
							<xsl:variable name="typeCultureSensitive" select="@cultureSensitive='true' or @cultureSensitive='1' or ($groupCultureSensitive and not(@cultureSensitive='false' or @cultureSensitive='0'))"/>
							<xsl:variable name="numberStyles" select="normalize-space(@numberStyles)"/>
							<xsl:variable name="numberStylesFragment">
								<xsl:if test="$numberStyles">
									<xsl:variable name="styleListFragment">
										<xsl:call-template name="SplitList">
											<xsl:with-param name="ItemList" select="$numberStyles"/>
										</xsl:call-template>
									</xsl:variable>
									<xsl:call-template name="OrTogetherEnumItems">
										<xsl:with-param name="EnumType" select="'NumberStyles'"/>
										<xsl:with-param name="ItemNames" select="exsl:node-set($styleListFragment)/child::*"/>
									</xsl:call-template>
								</xsl:if>
							</xsl:variable>
							<xsl:variable name="dateTimeStyles" select="normalize-space(@dateTimeStyles)"/>
							<xsl:variable name="dateTimeStylesFragment">
								<xsl:if test="$dateTimeStyles">
									<xsl:variable name="styleListFragment">
										<xsl:call-template name="SplitList">
											<xsl:with-param name="ItemList" select="$dateTimeStyles"/>
										</xsl:call-template>
									</xsl:variable>
									<xsl:call-template name="OrTogetherEnumItems">
										<xsl:with-param name="EnumType" select="'DateTimeStyles'"/>
										<xsl:with-param name="ItemNames" select="exsl:node-set($styleListFragment)/child::*"/>
									</xsl:call-template>
								</xsl:if>
							</xsl:variable>
							<xsl:variable name="cultureSpecificFormatParamsFragment">
								<!-- Number and date parsing functions take the culture and style params in different order -->
								<xsl:choose>
									<xsl:when test="$numberStyles">
										<plx:passParam>
											<xsl:copy-of select="$numberStylesFragment"/>
										</plx:passParam>
										<plx:passParam>
											<plx:callThis name="CurrentCulture" type="property"/>
										</plx:passParam>
									</xsl:when>
									<xsl:when test="$dateTimeStyles">
										<plx:passParam>
											<plx:callThis name="CurrentCulture" type="property"/>
										</plx:passParam>
										<plx:passParam>
											<xsl:copy-of select="$dateTimeStylesFragment"/>
										</plx:passParam>
									</xsl:when>
								</xsl:choose>
							</xsl:variable>
							<xsl:variable name="cultureInvariantFormatParamsFragment">
								<!-- Number and date parsing functions take the culture and style params in different order -->
								<xsl:choose>
									<xsl:when test="$numberStyles">
										<plx:passParam>
											<xsl:copy-of select="$numberStylesFragment"/>
										</plx:passParam>
										<plx:passParam>
											<plx:callStatic dataTypeName="CultureInfo" name="InvariantCulture" type="property"/>
										</plx:passParam>
									</xsl:when>
									<xsl:when test="$dateTimeStyles">
										<plx:passParam>
											<plx:callStatic dataTypeName="CultureInfo" name="InvariantCulture" type="property"/>
										</plx:passParam>
										<plx:passParam>
											<xsl:copy-of select="$dateTimeStylesFragment"/>
										</plx:passParam>
									</xsl:when>
								</xsl:choose>
							</xsl:variable>
							<plx:class name="{$typeName}{$groupName}DataType" partial="true" visibility="public">
								<xsl:variable name="comment" select="dtg:Comment"/>
								<xsl:if test="$comment">
									<plx:leadingInfo>
										<plx:docComment>
											<xsl:for-each select="$comment">
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
											<plx:callStatic name="{$groupName}{$typeName}" dataTypeName="PortableDataType" type="field"/>
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
										<plx:callStatic name="PortableDataType{$groupName}{$typeName}" dataTypeName="ResourceStrings" type="property"/>
									</plx:return>
								</plx:function>
								<xsl:if test="$typeCanCompareFalse">
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
								<xsl:if test="$typeCultureSensitive">
									<plx:property name="IsCultureSensitive" modifier="override" visibility="public">
										<plx:leadingInfo>
											<plx:docComment>
												<summary>The string form of data for this data type is culture-dependent.</summary>
											</plx:docComment>
										</plx:leadingInfo>
										<plx:returns dataTypeName=".boolean"/>
										<plx:get>
											<plx:return>
												<plx:trueKeyword/>
											</plx:return>
										</plx:get>
									</plx:property>
								</xsl:if>
								<plx:property name="RangeSupport" modifier="override" visibility="public">
									<plx:leadingInfo>
										<plx:docComment>
											<summary>The data type supports '<xsl:value-of select="$typeRangeSupport"/>' ranges</summary>
										</plx:docComment>
									</plx:leadingInfo>
									<plx:returns dataTypeName="DataTypeRangeSupport"/>
									<plx:get>
										<plx:return>
											<plx:callStatic name="{$typeRangeSupport}" dataTypeName="DataTypeRangeSupport" type="field"/>
										</plx:return>
									</plx:get>
								</plx:property>
								<xsl:if test="$typeBackingType">
									<xsl:if test="not($typeBackingType='.string')">
										<plx:function name="CanParse" modifier="override" visibility="public">
											<plx:leadingInfo>
												<plx:docComment>
													<summary>Returns true if the string value can be interpreted as this data type</summary>
												</plx:docComment>
											</plx:leadingInfo>
											<plx:param name="value" dataTypeName=".string"/>
											<plx:returns dataTypeName=".boolean"/>
											<plx:local name="result" dataTypeName="{$typeBackingType}"/>
											<plx:return>
												<plx:callStatic name="TryParse" dataTypeName="{$typeBackingType}">
													<plx:passParam>
														<plx:nameRef name="value" type="parameter"/>
													</plx:passParam>
													<xsl:copy-of select="$cultureSpecificFormatParamsFragment"/>
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
										<xsl:if test="$typeCultureSensitive">
											<plx:function name="CanParseInvariant" modifier="override" visibility="public">
												<plx:leadingInfo>
													<plx:docComment>
														<summary>Returns true if the invariant string value can be interpreted as this data type</summary>
													</plx:docComment>
												</plx:leadingInfo>
												<plx:param name="invariantValue" dataTypeName=".string"/>
												<plx:returns dataTypeName=".boolean"/>
												<plx:local name="result" dataTypeName="{$typeBackingType}"/>
												<plx:return>
													<plx:callStatic name="TryParse" dataTypeName="{$typeBackingType}">
														<plx:passParam>
															<plx:nameRef name="invariantValue" type="parameter"/>
														</plx:passParam>
														<xsl:copy-of select="$cultureInvariantFormatParamsFragment"/>
														<plx:passParam type="out">
															<plx:nameRef name="result"/>
														</plx:passParam>
													</plx:callStatic>
												</plx:return>
											</plx:function>
											<plx:function name="TryConvertToInvariant" modifier="override" visibility="public">
												<plx:leadingInfo>
													<plx:docComment>
														<summary>Convert a culture-dependent string to an invariant string.</summary>
													</plx:docComment>
												</plx:leadingInfo>
												<plx:param name="value" dataTypeName=".string"/>
												<plx:param name="invariantValue" dataTypeName=".string" type="out"/>
												<plx:returns dataTypeName=".boolean"/>
												<plx:local name="typedValue" dataTypeName="{$typeBackingType}"/>
												<plx:branch>
													<plx:condition>
														<plx:callStatic name="TryParse" dataTypeName="{$typeBackingType}">
															<plx:passParam>
																<plx:nameRef name="value" type="parameter"/>
															</plx:passParam>
															<xsl:copy-of select="$cultureSpecificFormatParamsFragment"/>
															<plx:passParam type="out">
																<plx:nameRef name="typedValue"/>
															</plx:passParam>
														</plx:callStatic>
													</plx:condition>
													<plx:assign>
														<plx:left>
															<plx:nameRef name="invariantValue"/>
														</plx:left>
														<plx:right>
															<plx:callInstance name="ToString">
																<plx:callObject>
																	<plx:nameRef name="typedValue"/>
																</plx:callObject>
																<plx:passParam>
																	<plx:callStatic dataTypeName="CultureInfo" name="InvariantCulture" type="property"/>
																</plx:passParam>
															</plx:callInstance>
														</plx:right>
													</plx:assign>
													<plx:return>
														<plx:trueKeyword/>
													</plx:return>
												</plx:branch>
												<plx:assign>
													<plx:left>
														<plx:nameRef name="invariantValue"/>
													</plx:left>
													<plx:right>
														<plx:nullKeyword/>
													</plx:right>
												</plx:assign>
												<plx:return>
													<plx:falseKeyword/>
												</plx:return>
											</plx:function>
											<plx:function name="TryConvertFromInvariant" modifier="override" visibility="public">
												<plx:leadingInfo>
													<plx:docComment>
														<summary>Convert an invariant string to a culture-dependent string.</summary>
													</plx:docComment>
												</plx:leadingInfo>
												<plx:param name="invariantValue" dataTypeName=".string"/>
												<plx:param name="value" dataTypeName=".string" type="out"/>
												<plx:returns dataTypeName=".boolean"/>
												<plx:local name="typedValue" dataTypeName="{$typeBackingType}"/>
												<plx:branch>
													<plx:condition>
														<plx:callStatic name="TryParse" dataTypeName="{$typeBackingType}">
															<plx:passParam>
																<plx:nameRef name="invariantValue" type="parameter"/>
															</plx:passParam>
															<xsl:copy-of select="$cultureInvariantFormatParamsFragment"/>
															<plx:passParam type="out">
																<plx:nameRef name="typedValue"/>
															</plx:passParam>
														</plx:callStatic>
													</plx:condition>
													<plx:assign>
														<plx:left>
															<plx:nameRef name="value"/>
														</plx:left>
														<plx:right>
															<plx:callInstance name="ToString">
																<plx:callObject>
																	<plx:nameRef name="typedValue"/>
																</plx:callObject>
																<plx:passParam>
																	<plx:callThis name="CurrentCulture" type="property"/>
																</plx:passParam>
															</plx:callInstance>
														</plx:right>
													</plx:assign>
													<plx:return>
														<plx:trueKeyword/>
													</plx:return>
												</plx:branch>
												<plx:assign>
													<plx:left>
														<plx:nameRef name="value"/>
													</plx:left>
													<plx:right>
														<plx:nullKeyword/>
													</plx:right>
												</plx:assign>
												<plx:return>
													<plx:falseKeyword/>
												</plx:return>
											</plx:function>
										</xsl:if>
									</xsl:if>
									<plx:function name="Compare" modifier="override" visibility="public">
										<plx:leadingInfo>
											<plx:docComment>
												<summary>Compare two values. Each value should be checked previously with CanParse</summary>
											</plx:docComment>
										</plx:leadingInfo>
										<plx:param name="invariantValue1" dataTypeName=".string"/>
										<plx:param name="invariantValue2" dataTypeName=".string"/>
										<plx:returns dataTypeName=".i4"/>
										<xsl:choose>
											<xsl:when test="$typeBackingType='.string'">
												<plx:return>
													<plx:callInstance name="CompareTo">
														<plx:callObject>
															<plx:nameRef name="invariantValue1" type="parameter"/>
														</plx:callObject>
														<plx:passParam>
															<plx:nameRef name="invariantValue2" type="parameter"/>
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
															<plx:callThis name="CanParseInvariant">
																<plx:passParam>
																	<plx:nameRef name="invariantValue{$currentNumber}" type="parameter"/>
																</plx:passParam>
															</plx:callThis>
														</plx:passParam>
														<plx:passParam>
															<plx:string>Don't call Compare if CanParseInvariant(invariantValue<xsl:value-of select="$currentNumber"/>) returns false</plx:string>
														</plx:passParam>
													</plx:callStatic>
													<!-- Get the typed value -->
													<plx:local name="typedValue{$currentNumber}" dataTypeName="{$typeBackingType}"/>
													<plx:callStatic name="TryParse" dataTypeName="{$typeBackingType}">
														<plx:passParam>
															<plx:nameRef name="invariantValue{$currentNumber}" type="parameter"/>
														</plx:passParam>
														<xsl:copy-of select="$cultureInvariantFormatParamsFragment"/>
														<plx:passParam type="out">
															<plx:nameRef name="typedValue{$currentNumber}"/>
														</plx:passParam>
													</plx:callStatic>
													<!-- End of OneAndTwo-->
												</xsl:for-each>
												<xsl:choose>
													<xsl:when test="$typeRangeSupport='None'">
														<plx:branch>
															<plx:condition>
																<plx:callInstance name="Equals">
																	<plx:callObject>
																		<plx:cast dataTypeName="IEquatable">
																			<plx:passTypeParam dataTypeName="{$typeBackingType}"/>
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
																		<plx:passTypeParam dataTypeName="{$typeBackingType}"/>
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
									<xsl:if test="$typeRangeSupport='DiscontinuousEndPoints' and $typeDiscontinuousRangePattern">
										<xsl:choose>
											<xsl:when test="$typeDiscontinuousRangePattern='Integer'">
												<xsl:variable name="boundFunctionDataFragment">
													<functionData namePart="Lower" commentPart="lower" limit="MaxValue" operator="add"/>
													<functionData namePart="Upper" commentPart="upper" limit="MinValue" operator="subtract"/>
												</xsl:variable>
												<xsl:for-each select="exsl:node-set($boundFunctionDataFragment)/child::*">
													<plx:function modifier="override" name="AdjustDiscontinuous{@namePart}Bound" visibility="public">
														<plx:leadingInfo>
															<plx:docComment>
																<summary>Adjust the <xsl:value-of select="@commentPart"/> bound for open ranges.</summary>
															</plx:docComment>
														</plx:leadingInfo>
														<plx:param name="invariantBound" dataTypeName=".string" type="inOut"/>
														<plx:param name="isOpen" dataTypeName=".boolean" type="inOut"/>
														<plx:returns dataTypeName=".boolean"/>
														<plx:branch>
															<plx:condition>
																<plx:nameRef name="isOpen" type="parameter"/>
															</plx:condition>
															<plx:local name="bound" dataTypeName="{$typeBackingType}"/>
															<plx:local name="formatProvider" dataTypeName="IFormatProvider">
																<plx:initialize>
																	<plx:callStatic name="InvariantCulture" type="property" dataTypeName="CultureInfo"/>
																</plx:initialize>
															</plx:local>
															<plx:branch>
																<plx:condition>
																	<plx:binaryOperator type="booleanOr">
																		<plx:left>
																			<plx:unaryOperator type="booleanNot">
																				<plx:callStatic name="TryParse" dataTypeName="{$typeBackingType}">
																					<plx:passParam>
																						<plx:nameRef name="invariantBound" type="parameter"/>
																					</plx:passParam>
																					<plx:passParam>
																						<plx:callStatic name="Integer" type="field" dataTypeName="NumberStyles"/>
																					</plx:passParam>
																					<plx:passParam>
																						<plx:nameRef name="formatProvider"/>
																					</plx:passParam>
																					<plx:passParam type="out">
																						<plx:nameRef name="bound"/>
																					</plx:passParam>
																				</plx:callStatic>
																			</plx:unaryOperator>
																		</plx:left>
																		<plx:right>
																			<plx:binaryOperator type="equality">
																				<plx:left>
																					<plx:nameRef name="bound"/>
																				</plx:left>
																				<plx:right>
																					<plx:callStatic dataTypeName="{$typeBackingType}" name="{@limit}" type="field"/>
																				</plx:right>
																			</plx:binaryOperator>
																		</plx:right>
																	</plx:binaryOperator>
																</plx:condition>
																<plx:return>
																	<plx:falseKeyword/>
																</plx:return>
															</plx:branch>
															<plx:assign>
																<plx:left>
																	<plx:nameRef name="invariantBound" type="parameter"/>
																</plx:left>
																<plx:right>
																	<plx:callInstance name="ToString">
																		<plx:callObject>
																			<plx:binaryOperator type="{@operator}">
																				<plx:left>
																					<plx:nameRef name="bound"/>
																				</plx:left>
																				<plx:right>
																					<plx:value data="1" type="{substring($typeBackingType,2)}"/>
																				</plx:right>
																			</plx:binaryOperator>
																		</plx:callObject>
																		<plx:passParam>
																			<plx:nameRef name="formatProvider"/>
																		</plx:passParam>
																	</plx:callInstance>
																</plx:right>
															</plx:assign>
															<plx:assign>
																<plx:left>
																	<plx:nameRef name="isOpen" type="parameter"/>
																</plx:left>
																<plx:right>
																	<plx:falseKeyword/>
																</plx:right>
															</plx:assign>
														</plx:branch>
														<plx:return>
															<plx:trueKeyword/>
														</plx:return>
													</plx:function>
												</xsl:for-each>
											</xsl:when>
											<xsl:otherwise>
												<xsl:message terminate="yes">
													<xsl:text>Unrecognized discontinuous end point pattern: </xsl:text>
													<xsl:value-of select="$typeDiscontinuousRangePattern"/>
												</xsl:message>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:if>
								</xsl:if>
								<xsl:if test="@lengthName">
									<plx:property name="LengthName" modifier="override" visibility="public">
										<plx:leadingInfo>
											<plx:docComment>
												<xsl:choose>
													<xsl:when test="string(@lengthName)">
														<summary>
															<xsl:text>Show the Length property for this DataType based on the '</xsl:text>
															<xsl:value-of select="@lengthName"/>
															<xsl:text>' resource string.</xsl:text></summary>
													</xsl:when>
													<xsl:otherwise>
														<summary>Show the Length property with this DataType</summary>
													</xsl:otherwise>
												</xsl:choose>
											</plx:docComment>
										</plx:leadingInfo>
										<plx:returns dataTypeName=".string"/>
										<plx:get>
											<plx:return>
												<xsl:choose>
													<xsl:when test="string(@lengthName)">
														<plx:callStatic name="{@lengthName}" dataTypeName="ResourceStrings" type="property"/>
													</xsl:when>
													<xsl:otherwise>
														<plx:string/>
													</xsl:otherwise>
												</xsl:choose>
											</plx:return>
										</plx:get>
									</plx:property>
									<xsl:variable name="lengthDescription" select="string(@lengthDescription)"/>
									<xsl:if test="$lengthDescription">
										<plx:property name="LengthDescription" modifier="override" visibility="public">
											<plx:leadingInfo>
												<plx:docComment>
													<summary>
														<xsl:text>Show the description for the Length property for this DataType based on the '</xsl:text>
														<xsl:value-of select="$lengthDescription"/>
														<xsl:text>' resource string.</xsl:text>
													</summary>
												</plx:docComment>
											</plx:leadingInfo>
											<plx:returns dataTypeName=".string"/>
											<plx:get>
												<plx:return>
													<plx:callStatic name="{@lengthDescription}" dataTypeName="ResourceStrings" type="property"/>
												</plx:return>
											</plx:get>
										</plx:property>
									</xsl:if>
								</xsl:if>
								<xsl:if test="@scaleName">
									<plx:property name="ScaleName" modifier="override" visibility="public">
										<plx:leadingInfo>
											<plx:docComment>
												<xsl:choose>
													<xsl:when test="string(@scaleName)">
														<summary>
															<xsl:text>Show the Scale property for this DataType based on the '</xsl:text>
															<xsl:value-of select="@lengthName"/>
															<xsl:text>' resource string.</xsl:text></summary>
													</xsl:when>
													<xsl:otherwise>
														<summary>Show the Scale property with this DataType</summary>
													</xsl:otherwise>
												</xsl:choose>
											</plx:docComment>
										</plx:leadingInfo>
										<plx:returns dataTypeName=".string"/>
										<plx:get>
											<plx:return>
												<xsl:choose>
													<xsl:when test="string(@scaleName)">
														<plx:callStatic name="{@scaleName}" dataTypeName="ResourceStrings" type="property"/>
													</xsl:when>
													<xsl:otherwise>
														<plx:string/>
													</xsl:otherwise>
												</xsl:choose>
											</plx:return>
										</plx:get>
									</plx:property>
									<xsl:variable name="scaleDescription" select="string(@scaleDescription)"/>
									<xsl:if test="$scaleDescription">
										<plx:property name="ScaleDescription" modifier="override" visibility="public">
											<plx:leadingInfo>
												<plx:docComment>
													<summary>
														<xsl:text>Show the description for the Scale property for this DataType based on the '</xsl:text>
														<xsl:value-of select="$scaleDescription"/>
														<xsl:text>' resource string.</xsl:text>
													</summary>
												</plx:docComment>
											</plx:leadingInfo>
											<plx:returns dataTypeName=".string"/>
											<plx:get>
												<plx:return>
													<plx:callStatic name="{@scaleDescription}" dataTypeName="ResourceStrings" type="property"/>
												</plx:return>
											</plx:get>
										</plx:property>
									</xsl:if>
								</xsl:if>
							</plx:class>
						</xsl:for-each>
					</xsl:if>
				</xsl:for-each>
			</plx:namespace>
		</plx:root>
	</xsl:template>
	<!-- Create a fragment with one element for each item in a list. The ItemList
	should be normalized before this call. -->
	<xsl:template name="SplitList">
		<xsl:param name="ItemList"/>
		<xsl:variable name="itemString" select="substring-before($ItemList,' ')"/>
		<xsl:choose>
			<xsl:when test="$itemString">
				<element>
					<xsl:value-of select="$itemString"/>
				</element>
				<xsl:variable name="remainder" select="substring-after($ItemList,' ')"/>
				<xsl:if test="remainder">
					<xsl:call-template name="SplitList">
						<xsl:with-param name="ItemList" select="$remainder"/>
					</xsl:call-template>
				</xsl:if>
			</xsl:when>
			<xsl:when test="$ItemList">
				<element>
					<xsl:value-of select="$ItemList"/>
				</element>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<!-- Or together enum values from the given type. ItemNames is any node-set where
	the text of each node is an enum item name.  -->
	<xsl:template name="OrTogetherEnumItems">
		<xsl:param name="EnumType"/>
		<xsl:param name="ItemNames"/>
		<xsl:param name="CurrentIndex" select="1"/>
		<xsl:param name="ItemCount"	select="count($ItemNames)"/>
		<xsl:choose>
			<xsl:when test="$CurrentIndex=$ItemCount">
				<plx:callStatic dataTypeName="{$EnumType}" name="{$ItemNames[$CurrentIndex]}" type="field"/>
			</xsl:when>
			<xsl:otherwise>
				<plx:binaryOperator type="bitwiseOr">
					<plx:left>
						<plx:callStatic dataTypeName="{$EnumType}" name="{$ItemNames[$CurrentIndex]}" type="field"/>
					</plx:left>
					<plx:right>
						<xsl:call-template name="OrTogetherEnumItems">
							<xsl:with-param name="EnumType" select="$EnumType"/>
							<xsl:with-param name="ItemNames" select="$ItemNames"/>
							<xsl:with-param name="CurrentIndex" select="$CurrentIndex+1"/>
							<xsl:with-param name="ItemCount" select="$ItemCount"/>
						</xsl:call-template>
					</plx:right>
				</plx:binaryOperator>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
